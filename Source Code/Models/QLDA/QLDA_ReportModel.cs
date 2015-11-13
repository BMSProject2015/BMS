using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Controls;
using System.Collections.Specialized;
using DomainModel.Abstract;
namespace VIETTEL.Models
{
    public class QLDA_ReportModel
    {
        public static DataTable dt_DotCapPhat(String MaND)
        {
            String NamLamViec = "2000";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            if (dtCauHinh.Rows.Count > 0)
            {
                NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT CP.iID_MaDotCapPhat,dNgayCapPhat,dNgayLap FROM(
										SELECT DISTINCT iID_MaDotCapPhat
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec
                                        ) as CP
                                        INNER JOIN (SELECT iID_MaDotCapPhat,dNgayLap,CONVERT(varchar(10),dNgayLap,103) as dNgayCapPhat
													FROM QLDA_CapPhat_Dot
													WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec) as DotCP
										ON CP.iID_MaDotCapPhat=DotCP.iID_MaDotCapPhat
										ORDER BY dNgayLap");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable getdtTien()
        {
            String SQL=String.Format(@"SELECT iID_MaNgoaiTe,sTen FROM QLDA_NgoaiTe WHERE iTrangThai=1");
            DataTable dt = Connection.GetDataTable(SQL);
            DataRow r = dt.NewRow();
            r[0] = "0";
            r[1] = "VND";
            dt.Rows.InsertAt(r, 0);
            return dt;
        }
        /// <summary>
        /// lấy loại tiền
        /// </summary>
        /// <param name="sDeAn"></param>
        /// <param name="dNgay"></param>
        /// <returns></returns>
        public static DataTable dt_LoaiTien(String sDeAn, String dNgay)
        {
            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length;i++)
            {
                DKDeAn += "sDeAn=@sDeAn"+i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR ";
            }
            String SQL = String.Format(@"SELECT DISTINCT iID_MaNgoaiTe,sTenNgoaiTe
                                        FROM QLDA_TongDauTu
                                        WHERE iTrangThai=1 AND 
                                              dNgayPheDuyet<=@dNgay AND
                                              ({0}) AND
                                              iID_MaNgoaiTe IS NOT NULL AND
                                              rNgoaiTe>0
                                        UNION
                                        SELECT DISTINCT iID_MaNgoaiTe,sTenNgoaiTe
                                        FROM QLDA_TongDuToan
                                        WHERE iTrangThai=1 AND
                                              dNgayPheDuyet<=@dNgay AND
                                              ({0}) AND
                                              iID_MaNgoaiTe IS NOT NULL AND
                                              rNgoaiTe>0",DKDeAn);
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn"+i, arrDeAn[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow r = dt.NewRow();
            r[0] = "0";
            r[1] = "VND";
            dt.Rows.InsertAt(r, 0);
            return dt;
        }
        /// <summary>
        /// lấy danh sách đề án
        /// </summary>
        /// <param name="dNgay"></param>
        /// <returns></returns>
        public static DataTable dt_DeAn(String dNgay)
        {
            String SQL = String.Format(@"SELECT a.sDeAn,sTenDuAn  FROM (SELECT DISTINCT sDeAn
                                        FROM QLDA_TongDauTu
                                        WHERE iTrangThai=1 AND 
                                              dNgayPheDuyet<=@dNgay AND
                                               ( (
                                              iID_MaNgoaiTe IS NOT NULL AND
                                              ABS(rNgoaiTe)>=0.5) OR ABS(rSoTien/1000000)>=0.5)
                                        UNION
                                        SELECT DISTINCT sDeAn
                                        FROM QLDA_TongDuToan
                                        WHERE iTrangThai=1 AND
                                              dNgayPheDuyet<=@dNgay AND
                                              ((iID_MaNgoaiTe IS NOT NULL AND
                                              ABS(rNgoaiTe)>=0.5) OR ABS(rSoTien/1000000)>=0.5)) as a
                                        INNER JOIN (SELECT DISTINCT sDeAn,sTenDuAn FROM QLDA_DanhMucDuAn WHERE sDuAn='' AND iTrangThai=1) as b
                                              ON a.sDeAn=b.sDeAn
                                        ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// danh sách đề án có dữ liệu
        /// </summary>
        /// <returns></returns>
        public static DataTable dt_DeAn()
        {
            String SQL = String.Format(@"SELECT a.sDeAn,sTenDuAn  FROM (SELECT DISTINCT sDeAn
                                        FROM QLDA_TongDauTu
                                        WHERE iTrangThai=1 AND 
                                               ( (
                                              iID_MaNgoaiTe IS NOT NULL AND
                                              ABS(rNgoaiTe)<>0) OR ABS(rSoTien/1000000)<>0)
                                        UNION
                                        SELECT DISTINCT sDeAn
                                        FROM QLDA_TongDuToan
                                        WHERE iTrangThai=1 AND
                                              ((iID_MaNgoaiTe IS NOT NULL AND
                                              ABS(rNgoaiTe)<>0) OR ABS(rSoTien/1000000)<>0)) as a
                                        INNER JOIN (SELECT DISTINCT sDeAn,sTenDuAn FROM QLDA_DanhMucDuAn WHERE sDuAn='' AND iTrangThai=1) as b
                                              ON a.sDeAn=b.sDeAn
                                        ");
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// danh sách đề án KHV có dữ liệu
        /// </summary>
        /// <param name="dNgay"></param>
        /// <returns></returns>
        public static DataTable dt_DeAn_KHV(String dNgay)
        {
            String SQL = String.Format(@"SELECT a.sDeAn,sTenDuAn  FROM (SELECT DISTINCT sDeAn
                                        FROM QLDA_KeHoachVon
                                        WHERE iTrangThai=1 AND 
                                              ((iID_MaNgoaiTe_SoTienDauNam IS NOT NULL AND ABS(rNgoaiTe_SoTienDauNam)>=0.5)
											   OR ( iID_MaNgoaiTe_SoTienDieuChinh IS NOT NULL AND ABS(rNgoaiTe_SoTienDieuChinh)>=0.5)
                                               OR (ABS(rSoTienDauNam)>0)
                                               OR (ABS(rSoTienDieuChinh)>0)
                                                )
                                               AND sM IN(9200,9250,9300,9350,9400) 
                                               AND dNgayKeHoachVon<=@dNgay
                                       ) as a
                                        INNER JOIN (SELECT DISTINCT sDeAn,sTenDuAn FROM QLDA_DanhMucDuAn WHERE sDuAn='' AND iTrangThai=1) as b
                                              ON a.sDeAn=b.sDeAn
                                        ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách loại tiền KHV
        /// </summary>
        /// <param name="sDeAn"></param>
        /// <param name="dNgay"></param>
        /// <returns></returns>
        public static DataTable dt_LoaiTien_KHV(String sDeAn, String dNgay)
        {
            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                DKDeAn += "sDeAn=@sDeAn" + i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR ";
            }
            String SQL = String.Format(@"SELECT DISTINCT iID_MaNgoaiTe_DauNam as iID_MaNgoaiTe,sTenNgoaiTe_DauNam as sTenNgoaiTe
                                        FROM QLDA_KeHoachVon
                                        WHERE iTrangThai=1 AND ({0}) AND 
                                              ((iID_MaNgoaiTe_SoTienDauNam IS NOT NULL AND ABS(rNgoaiTe_SoTienDauNam)>=0.5)
											   OR ( iID_MaNgoaiTe_SoTienDieuChinh IS NOT NULL AND ABS(rNgoaiTe_SoTienDieuChinh)>=0.5)
                                                )
                                               AND sM IN(9200,9250,9300,9350,9400) 
                                               AND dNgayKeHoachVon=@dNgay
                                       ", DKDeAn);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow r = dt.NewRow();
            r[0] = "0";
            r[1] = "VND";
            dt.Rows.InsertAt(r, 0);
            return dt;
        }
        /// <summary>
        /// Lấy loại kế hoạch vốn
        /// </summary>
        /// <returns></returns>
        public static DataTable dt_LoaiKeHoachVon()
        {
            String SQL = @" SELECT iID_MaLoaiKeHoachVon,sTen FROM QLDA_KeHoachVon_Loai WHERE iTrangThai=1";
            DataTable dt = Connection.GetDataTable(SQL);
            dt.Dispose();
            return dt;
        }

        public static DataTable dt_LoaiNganSachKHV(String dNgay, String sDeAn)
        {
            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                DKDeAn += "sDeAn=@sDeAn" + i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR ";
            }
            String SQL = String.Format(@"SELECT a.sLNS,sL,sK,sMoTa,a.sLNS+'-'+sMoTa as TenHT FROM(
                                        SELECT DISTINCT sLNS,sL,sK
                                        FROM QLDA_KeHoachVon
                                        WHERE iTrangThai=1
	                                          AND dNgayKeHoachVon<=@dNgay
                                              AND ({0})
	                                          AND sM IN(9200,9250,9300,9350,9400)) as a
                                        INNER JOIN (SELECT sLNS,sMoTa FROM NS_MucLucNganSach	
			                                        WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7
			                                        ) as b  
                                        ON a.sLNS=b.sLNS",DKDeAn);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }


        public static DataTable dt_LoaiTien_CP(String dNgay, String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = "2000";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT iID_MaNgoaiTe,sTenNgoaiTe FROM(
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_ChuDauTuTamUng as iID_MaNgoaiTe,sTenNgoaiTe_ChuDauTuTamUng as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_ChuDauTuTamUng IS NOT NULL AND iID_MaNgoaiTe_ChuDauTuTamUng<>0 AND iNamLamViec=@iNamLamViec 
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_ChuDauTuThanhToan as iID_MaNgoaiTe,sTenNgoaiTe_ChuDauTuThanhToan as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_ChuDauTuThanhToan IS NOT NULL AND iID_MaNgoaiTe_ChuDauTuThanhToan<>0 AND iNamLamViec=@iNamLamViec 
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_ChuDauTuThuTamUng as iID_MaNgoaiTe,sTenNgoaiTe_ChuDauTuThuTamUng as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_ChuDauTuThuTamUng IS NOT NULL AND iID_MaNgoaiTe_ChuDauTuThuTamUng<>0 AND iNamLamViec=@iNamLamViec 
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetTamUng as iID_MaNgoaiTe,sTenNgoaiTe_DeNghiPheDuyetTamUng as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetTamUng IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetTamUng<>0 AND iNamLamViec=@iNamLamViec 
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetThanhToan as iID_MaNgoaiTe,sTenNgoaiTe_DeNghiPheDuyetThanhToan as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetThanhToan IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetThanhToan<>0 AND iNamLamViec=@iNamLamViec
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng as iID_MaNgoaiTe,sTenNgoaiTe_DeNghiPheDuyetThuTamUng as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng<>0 AND iNamLamViec=@iNamLamViec 
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetThuKhac as iID_MaNgoaiTe,sTenNgoaiTe_DeNghiPheDuyetThuKhac as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetThuKhac IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetThuKhac<>0 AND iNamLamViec=@iNamLamViec) as CP
                                                    INNER JOIN (SELECT iID_MaDotCapPhat,dNgayLap,CONVERT(varchar(10),dNgayLap,103) as dNgayCapPhat
																FROM QLDA_CapPhat_Dot
																WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND CONVERT(varchar,dNgayLap,103)<=@dNgay) as DotCP
												    ON CP.iID_MaDotCapPhat=DotCP.iID_MaDotCapPhat");
            SQL = "SELECT iID_MaNgoaiTe,sTen as sTenNgoaiTe FROM QLDA_NgoaiTe";
            SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
           // cmd.Parameters.AddWithValue("@dNgay",dNgay);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow r = dt.NewRow();
            r[0] = "0";
            r[1] = "VND";
            dt.Rows.InsertAt(r, 0);
            return dt;

        }

        /// <summary>
        /// Loại tiền cấp phát báo cáo 03-CP
        /// </summary>
        /// <param name="dNgay"></param>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable dt_LoaiTien_CP_03(String dNgay, String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = "2000";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT iID_MaNgoaiTe,sTenNgoaiTe FROM(
                                                    SELECT DISTINCT iID_MaDotCapPhat=0,iID_MaNgoaiTe_SoTienDauNam as iID_MaNgoaiTe,sTenNgoaiTe_SoTienDauNam as sTenNgoaiTe
                                                    FROM QLDA_KeHoachVon
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_SoTienDieuChinh IS NOT NULL AND iID_MaNgoaiTe_SoTienDieuChinh<>0 AND iNamLamViec=@iNamLamViec AND dNgayKeHoachVon<=@dNgay
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat=0,iID_MaNgoaiTe_SoTienDieuChinh as iID_MaNgoaiTe,sTenNgoaiTe_SoTienDieuChinh as sTenNgoaiTe
                                                    FROM QLDA_KeHoachVon
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_SoTienDieuChinh IS NOT NULL AND iID_MaNgoaiTe_SoTienDieuChinh<>0  AND iNamLamViec=@iNamLamViec  AND dNgayKeHoachVon<=@dNgay
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetTamUng as sTenNgoaiTe,sTenNgoaiTe_DeNghiPheDuyetTamUng as sTenNgoaiTe
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetTamUng IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetTamUng<>0 AND iNamLamViec=@iNamLamViec
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetThuKhac,sTenNgoaiTe_DeNghiPheDuyetThuKhac
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetThuKhac IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetThuKhac<>0 AND iNamLamViec=@iNamLamViec
                                                    UNION
                                                    SELECT DISTINCT iID_MaDotCapPhat,iID_MaNgoaiTe_DeNghiPheDuyetThanhToan,sTenNgoaiTe_DeNghiPheDuyetThanhToan
                                                    FROM QLDA_CapPhat
                                                    WHERE iTrangThai=1 AND iID_MaNgoaiTe_DeNghiPheDuyetThanhToan IS NOT NULL AND iID_MaNgoaiTe_DeNghiPheDuyetThanhToan<>0 AND iNamLamViec=@iNamLamViec) as CP
                                                    INNER JOIN (SELECT iID_MaDotCapPhat,dNgayLap,CONVERT(varchar(10),dNgayLap,103) as dNgayCapPhat
																FROM QLDA_CapPhat_Dot
																WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND CONVERT(varchar,dNgayLap,103)<=@dNgay) as DotCP
												    ON CP.iID_MaDotCapPhat=DotCP.iID_MaDotCapPhat
                                                    ");
            SQL = "SELECT iID_MaNgoaiTe,sTen as sTenNgoaiTe FROM QLDA_NgoaiTe";
            SqlCommand cmd = new SqlCommand(SQL);
           // cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
           // cmd.Parameters.AddWithValue("@dNgay", dNgay);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow r = dt.NewRow();
            r[0] = "0";
            r[1] = "VND";
            dt.Rows.InsertAt(r, 0);
            return dt;

        }

        /// <summary>
        /// danh sách đề án trong danh mục dự án
        /// </summary>
        /// <returns></returns>
        public static DataTable dt_DeAn_all()
        {
            String SQL = @"SELECT DISTINCT sDeAn,sTenDuAn FROM QLDA_DanhMucDuAn
                          WHERE iTrangThai=1 AND sDuAn=''";
            DataTable dt=Connection.GetDataTable(SQL);
            return dt;
        }
    }
}
