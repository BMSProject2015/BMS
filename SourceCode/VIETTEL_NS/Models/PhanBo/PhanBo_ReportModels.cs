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
    public class PhanBo_ReportModels
    {
        #region Phân bổ tổng
        public static DataTable LayDSDotPhanBoTong(String MaND, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {

            }
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT * FROM (
                                        SELECT iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM PB_DotPhanBo WHERE iID_MaDotPhanBo IN ( SELECT DISTINCT iID_MaDotPhanBo FROM  PB_PhanBo_PhanBo WHERE iTrangThai=1 ) AND  bDotPhanBoTong='1') as a
                                        INNER JOIN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE 1=1  {0} {1}) as b ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo 
                                        ORDER BY NgayPhanBo", ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDotPhanBoTong(String MaND, String iID_MaTrangThaiDuyet,String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {

            }
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = String.Format(@"SELECT * FROM (
                                        SELECT iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM PB_DotPhanBo WHERE iID_MaDotPhanBo IN ( SELECT DISTINCT iID_MaDotPhanBo FROM  PB_PhanBo_PhanBo WHERE iTrangThai=1 ) AND  bDotPhanBoTong='1') as a
                                        INNER JOIN (SELECT iID_MaDotPhanBo,iID_MaPhanBo FROM PB_PhanBo WHERE 1=1 AND iID_MaPhanBo IN (
                                        SELECT DISTINCT iID_MaPhanBoTong
                                        FROM PB_PhanBo_PhanBo
                                        WHERE iID_MaPhanBo IN ( SELECT DISTINCT iID_MaPhanBo
										FROM PB_PhanBoChiTiet
										WHERE 1=1 AND ({2}) ))  {0} {1}) as b ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo 
                                        ORDER BY NgayPhanBo", ReportModels.DieuKien_NganSach(MaND), DK_Duyet,DKLNS);
            cmd.CommandText = SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDotPhanBoTong(String MaND, String iID_MaTrangThaiDuyet, String sLNS,String TruongTien)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {

            }
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            
            String SQL = String.Format(@"SELECT * FROM (
                                        SELECT iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM PB_DotPhanBo WHERE iID_MaDotPhanBo IN ( SELECT DISTINCT iID_MaDotPhanBo FROM  PB_PhanBo_PhanBo WHERE iTrangThai=1 ) AND  bDotPhanBoTong='1') as a
                                        INNER JOIN (SELECT iID_MaDotPhanBo,iID_MaPhanBo FROM PB_PhanBo WHERE 1=1 AND iID_MaPhanBo IN (
                                        SELECT DISTINCT iID_MaPhanBoTong
                                        FROM PB_PhanBo_PhanBo
                                        WHERE iID_MaPhanBo IN ( SELECT DISTINCT iID_MaPhanBo
										FROM PB_PhanBoChiTiet
										WHERE 1=1 AND {3}>0 AND ({2}) ))  {0} {1}) as b ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo 
                                        ORDER BY NgayPhanBo", ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DKLNS,TruongTien);
            cmd.CommandText = SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDonViTong(String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, Boolean HienVat = false, Boolean LuyKe = true)
        {
            String DKDotPhanBo = "";
            DataTable dtDotPhanBo = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (LuyKe == false)
            {
                DKDotPhanBo = " iID_MaDotPhanBo=@iID_MaDotPhanBo ";
            }
            else
            {
                dtDotPhanBo = LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet);
                DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        DKDotPhanBo = "";
                        for (int j = 1; j <= i; j++)
                        {
                            DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                            if (j < i)
                                DKDotPhanBo += " OR ";
                        }
                        break;
                    }

                }
            }
            String DKTruongTien = "";
            if (HienVat == true)
            {
                DKTruongTien = " rTuChi> 0 OR rHienVat >0";
            }
            else
            {
                DKTruongTien = " rTuChi>0 ";
            }
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0") { }
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT
                                        FROM
                                        (
                                        SELECT  DISTINCT iID_MaDonVi
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1 AND ( iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND ({0}) ) ) )   AND ({1}) {2}  {3}
                                        ) AS A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B
                                        ON A.iID_MaDonVi=B.iID_MaDonVi
                                        ORDER BY iID_MaDonVi", DKDotPhanBo, DKTruongTien, ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (LuyKe == false)
            {
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            else
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                        }
                        break;
                    }

                }
            }
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["TenHT"] = "Chọn đơn vị";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;

        }
        public static DataTable LayDSDonViTong(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo, Boolean HienVat = false, Boolean LuyKe = true, String iThongBao = "1")
        {
            SqlCommand cmd = new SqlCommand();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
            String DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    DKDotPhanBo = "";
                    for (int j = 1; j <= i; j++)
                    {
                        DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                        if (j < i)
                            DKDotPhanBo += " OR ";
                    }
                    break;
                }

            }
            String DKTruongTien = "";
            if (HienVat == true)
            {
                DKTruongTien = " rTuChi> 0 OR rHienVat >0";
            }

            else
            {
                if (iThongBao == "1")
                {
                    DKTruongTien = " rTuChi>0 ";
                }
                else
                {
                    DKTruongTien = " rTuChi<0 ";
                }
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }

            //DK Thong bao 1 Cap 2 Thu
            String DKThongBao = "";
            if (iThongBao == "1") DKThongBao = " rTuChi>0";
            else DKThongBao = " rTuChi<0";
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT
                                        FROM
                                        (
                                        SELECT  DISTINCT iID_MaDonVi
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1  AND ({0}) {3} {4} AND ( iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND ({0}) ) ) ) AND ({2})) as A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B
                                        ON A.iID_MaDonVi=B.iID_MaDonVi
                                        ORDER BY iID_MaDonVi", DKLNS, DKDotPhanBo, DKTruongTien, ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    for (int j = 1; j <= i; j++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                    }
                    break;
                }

            }
            DataTable dt = Connection.GetDataTable(cmd);

            DataRow R = dt.NewRow();
            R["TenHT"] = "Chọn đơn vị";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        #endregion


        /// <summary>
        /// lấy danh sách đợt phân bổ theo năm làm việc
        /// </summary>
        /// <param name="NamLamViec">Năm Làm việc</param>
        /// <returns></returns>
        public static DataTable LayDSDotPhanBo(String NamLamViec)
        {
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDotPhanBo2(String MaND, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
            }
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM PB_DotPhanBo WHERE iID_MaDotPhanBo IN ( SELECT DISTINCT iID_MaDotPhanBo FROM  PB_PhanBoChiTiet WHERE iTrangThai=1 {0}  {1})
                                        ORDER BY NgayPhanBo", ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// lấy danh sách đợt phân bổ theo năm làm việc và slNS
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="sLNS"> sLNS</param>
        /// <returns></returns>
        public static DataTable LayDSDotPhanBo(String NamLamViec, String sLNS)
        {
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iNamLamViec=@NamLamViec AND iTrangThai=1 AND ({0}) AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo", DKLNS);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDotPhanBo2(String MaND, String iID_MaTrangThaiDuyet, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iTrangThai=1 AND ({0}) {1} {2}) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1 ) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo", DKLNS,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            cmd.CommandText = SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDotPhanBo(String NamLamViec, String sLNS,String iID_MaDonVi)
        {
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DKDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iNamLamViec=@NamLamViec AND iTrangThai=1 AND ({0}) AND ({1}) AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo", DKLNS,DKDonVi);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable LayDSDotPhanBo2(String MaND,String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi)
        {
            SqlCommand cmd = new SqlCommand();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DKDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iTrangThai=1 AND ({0}) AND ({1}) {2} {3}) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1 ) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo", DKLNS, DKDonVi,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
           cmd.CommandText=SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["dNgayDotPhanBo"] = "Chọn đợt phân bổ";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        ///  Danh sách đơn vị
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String NamLamViec, String iID_MaDotPhanBo,Boolean HienVat=false,Boolean LuyKe=true)
        {
             String DKDotPhanBo="";
            DataTable dtDotPhanBo=null;
            if (LuyKe == false)
            {
                DKDotPhanBo = " iID_MaDotPhanBo=@iID_MaDotPhanBo ";
            }
            else
            {
                dtDotPhanBo = LayDSDotPhanBo(NamLamViec);          
                DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        DKDotPhanBo = "";
                        for (int j = 1; j <= i; j++)
                        {
                            DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                            if (j < i)
                                DKDotPhanBo += " OR ";
                        }
                        break;
                    }

                }
            }
            String DKTruongTien = "";
            if (HienVat == true)
            {
                DKTruongTien = " rTuChi> 0 OR rHienVat >0";
            }
            else
            {
                DKTruongTien = " rTuChi>0 ";
            }

            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT
                                        FROM
                                        (
                                        SELECT  DISTINCT iID_MaDonVi
                                        FROM PB_PhanBoChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND iTrangThai=1  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND ({0}) AND ({1})) as A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi) as B
                                        ON A.iID_MaDonVi=B.iID_MaDonVi
                                        ORDER BY iID_MaDonVi",DKDotPhanBo,DKTruongTien);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            if (LuyKe == false)
            {
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            else
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                        }
                        break;
                    }

                }
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));          
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow R = dt.NewRow();
            R["TenHT"] = "Chọn đơn vị";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        /// 
        public static DataTable DanhSachDonVi(String NamLamViec,String sLNS,String iID_MaDotPhanBo, Boolean HienVat=false,Boolean LuyKe=true)
        {
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = LayDSDotPhanBo(NamLamViec,sLNS);
            String DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    DKDotPhanBo = "";
                    for (int j = 1; j <= i; j++)
                    {
                        DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                        if (j < i)
                            DKDotPhanBo += " OR ";
                    }
                    break;
                }

            }
            String DKTruongTien = "";
            if (HienVat == true)
            {
                DKTruongTien = " rTuChi> 0 OR rHienVat >0";
            }
            else
            {
                DKTruongTien = " rTuChi>0 ";
            }
            String SQL=String.Format(@"SELECT a.iID_MaDonVi,TenHT
                                        FROM
                                        (
                                        SELECT  DISTINCT iID_MaDonVi
                                        FROM PB_PhanBoChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND iTrangThai=1  AND ({0}) AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND ({1}) AND ({2})) as A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi) as B
                                        ON A.iID_MaDonVi=B.iID_MaDonVi
                                        ORDER BY iID_MaDonVi", DKLNS,DKDotPhanBo,DKTruongTien);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    for (int j = 1; j <= i; j++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                    }
                    break;
                }

            }
            DataTable dt = Connection.GetDataTable(cmd);

            DataRow R = dt.NewRow();
            R["TenHT"] = "Chọn đơn vị";
            dt.Rows.InsertAt(R, 0);
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        ///  Danh sách đơn vị
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi2(String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, Boolean HienVat = false, Boolean LuyKe = true,bool ChonTatCa=true)
        {
            String DKDotPhanBo = "";
            DataTable dtDotPhanBo = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (LuyKe == false)
            {
                DKDotPhanBo = " iID_MaDotPhanBo=@iID_MaDotPhanBo ";
            }
            else
            {
                dtDotPhanBo = LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet);
                DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        DKDotPhanBo = "";
                        for (int j = 1; j <= i; j++)
                        {
                            DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                            if (j < i)
                                DKDotPhanBo += " OR ";
                        }
                        break;
                    }

                }
            }
            String DKTruongTien = "";
            if (HienVat == true)
            {
                DKTruongTien = " rTuChi<> 0 OR rHienVat <>0";
            }
            else
            {
                DKTruongTien = " rTuChi<>0 ";
            }
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0"){}
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT, TenHT as sTen
                                        FROM
                                        (
                                        SELECT  DISTINCT iID_MaDonVi
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1 {3} AND ({0}) AND ({1}) {2} 
                                        ) AS A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B
                                        ON A.iID_MaDonVi=B.iID_MaDonVi
                                        ORDER BY iID_MaDonVi", DKDotPhanBo, DKTruongTien,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
           cmd.CommandText = SQL;
           cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (LuyKe == false)
            {
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            else
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                        }
                        break;
                    }

                }
            }
            DataTable dt = Connection.GetDataTable(cmd);
            if (ChonTatCa)
            {
                DataRow R = dt.NewRow();
                R["TenHT"] = "Chọn đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        /// 
        public static DataTable DanhSachDonVi2(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo, Boolean HienVat = false, Boolean LuyKe = true, String iThongBao = "1", bool ChonTatCa = true)
        {
            SqlCommand cmd = new SqlCommand();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet, sLNS);
            String DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    DKDotPhanBo = "";
                    for (int j = 1; j <= i; j++)
                    {
                        DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                        if (j < i)
                            DKDotPhanBo += " OR ";
                    }
                    break;
                }

            }
            String DKTruongTien = "";
            if (HienVat == true)
            {
                DKTruongTien = " rTuChi> 0 OR rHienVat >0";
            }
            
            else
            {
                if (iThongBao == "1")
                {
                    DKTruongTien = " rTuChi>0 ";
                }
                else
                {
                    DKTruongTien = " rTuChi<0 ";
                }
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }

            //DK Thong bao 1 Cap 2 Thu
            String DKThongBao = "";
            if (iThongBao == "1") DKThongBao = " rTuChi>0";
            else DKThongBao = " rTuChi<0";
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT,TenHT as sTen
                                        FROM
                                        (
                                        SELECT  DISTINCT iID_MaDonVi
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1  AND ({0}) {3} {4} AND ({1}) AND ({2})) as A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B
                                        ON A.iID_MaDonVi=B.iID_MaDonVi
                                        ORDER BY iID_MaDonVi", DKLNS, DKDotPhanBo, DKTruongTien,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    for (int j = 1; j <= i; j++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                    }
                    break;
                }

            }
            DataTable dt = Connection.GetDataTable(cmd);

            if (ChonTatCa)
            {
                DataRow R = dt.NewRow();
                R["TenHT"] = "Chọn đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
           
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Kiểu hiện thị 1: Dọc 2 Ngang
        /// </summary>
        /// <returns></returns>
        public static DataTable KieuHienThi()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaKieu", typeof(String));
            dt.Columns.Add("TenKieu", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "In dọc";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "In ngang";
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Kiểu thông báo 1: Cấp 2 Thu
        /// </summary>
        /// <returns></returns>
        public static DataTable KieuThongBao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaThongBao", typeof(String));
            dt.Columns.Add("TenThongBao", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "Cấp";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "Thu";
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Kieu muc sNG=Nganh , sTNG= tieu ngành
        /// </summary>
        /// <returns></returns>
        public static DataTable KieuMuc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMuc", typeof(String));
            dt.Columns.Add("TenMuc", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "sNG";
            R1[1] = "Ngành";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "sTNG";
            R2[1] = "Tiểu ngành";
            dt.Dispose();
            return dt;
        }
    }
}