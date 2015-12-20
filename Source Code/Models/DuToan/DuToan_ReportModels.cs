using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;

namespace VIETTEL.Models.DuToan
{
    public class DuToan_ReportModels
    {
        public static int DVT = 1000;
        #region Source cũ, khi chưa update code
        public static DataTable NS_NguonNganSach()
        {
            String SQL = "SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE LEN(sLNS)=1 AND sL='' ORDER BY sLNS";
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
        //Số phân bổ dự toán ngân sách năm(Phần ngấn sách quốc phòng)
        public static DataTable DT_rptPhanBoDuToanNganSachNam(String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@"SELECT sTen,a.iID_MaDonVi,SUM(rTongSo) as rTongSo,SUM(rChiTapTrung) as rChiTapTrung
                                        FROM (SELECT iID_MaDonVi,
                                        CASE WHEN SUBSTRING(sLNS,1,1)=1 THEN SUM(rTongSo) ELSE 0 END as rTongSo,
                                        CASE WHEN (SUBSTRING(sLNS,1,3) IN(103,104,105)) THEN SUM(rTongSo) ELSE 0 END as rChiTapTrung
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1  {1} {0}
                                        GROUP BY iID_MaDonVi,sLNS
                                        HAVING SUM(rTongSo)>0) as A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi) as B
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        GROUP BY a.iID_MaDonVi,sTen
                                        HAVING SUM(rTongSo)<>0 OR SUM(rChiTapTrung)<>0", ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
                SqlCommand cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();         
            return dt;
        }
        //Số phân bổ dự toán ngân sách năm(Phần ngấn sách nhà nước)
        public static DataTable DT_rptPhanBoDuToanNganSachNam_NhaNuoc(String NamLamViec, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT sTen,a.iID_MaDonVi,SUM(rTongSo) as rTongSo,SUM(rChiTaiKhoBac) as rChiTaiKhoBac
                                        FROM (SELECT iID_MaDonVi,
                                        CASE WHEN SUBSTRING(sLNS,1,1)=2 THEN SUM(rTongSo) ELSE 0 END as rTongSo,
                                        CASE WHEN (SUBSTRING(sLNS,1,1)=2 AND iID_MaDonVi=75) THEN SUM(rTongSo) ELSE 0 END as rChiTaiKhoBac
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet {0}
                                        GROUP BY iID_MaDonVi,sLNS
                                        HAVING SUM(rTongSo)>0) as A
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi) as B
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        GROUP BY a.iID_MaDonVi,sTen
                                        HAVING SUM(rTongSo)<>0 OR SUM(rChiTaiKhoBac)<>0", ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        //Dự toán chi ngân sách năm (phần ngân sách đảm bảo) LNS=1040100 phụ lục 1a-c
        public static DataTable DT_DuToanChiNganSachQuocPhong(String MaND, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = "SELECT DM.sTenKhoa as sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa,";
            SQL += "rTongSo1=SUM(rTonKho+rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong)";
            SQL += ",Sum(rTonKho) as rTonKho,";
            SQL += "rTongSo2=SUM(rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "Sum(rHangNhap) as rHangNhap,";
            SQL += "rTongSo3=SUM(rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "SUM(rTuChi) as rTuChi,SUM(rHangMua) as rHangMua,SUM(rPhanCap) as rPhanCap,SUM(rDuPhong) as rDuPhong";
            SQL += " FROM (SELECT  iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,";
            SQL += " rTongSo1=SUM(rTonKho+rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong)";
            SQL += ",Sum(rTonKho) as rTonKho,";
            SQL += "rTongSo2=SUM(rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "Sum(rHangNhap) as rHangNhap,";
            SQL += "rTongSo3=SUM(rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "SUM(rTuChi) as rTuChi,SUM(rHangMua) as rHangMua,SUM(rPhanCap) as rPhanCap,SUM(rDuPhong) as rDuPhong";
            SQL += " FROM DT_ChungTuChiTiet WHERE sLNS='1040100' AND sL='460' AND sK='468' AND bChiNganSach=1 AND ";
            SQL += " iTrangThai=1   {0} {1}";
            SQL += " Group BY  iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTonKho,rHangNhap";
            SQL += ") ChiTiet";
            SQL += " INNER JOIN NS_DonVi ON ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi ";
            SQL += " INNER JOIN (SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang)) DM ";
            SQL += " ON NS_DonVi.iID_MaKhoiDonVi=DM.iID_MaDanhMuc";
            SQL += " GROUP By DM.sTenKhoa,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SQL = String.Format(SQL, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenBang", "KhoiDonVi");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        //1020000
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_DuToanPhanCacNganhPhanCap(String NamLamViec)
        {
            return null;
        }

        /// <summary>
        ///Dự toán chi ngân sách sử dụng năm (phần ngân sách thường xuyên) 
        /// </summary>
        /// <param name="MaKhoi">LNS=1010000</param>
        /// <returns></returns>
        public static DataTable DT_DuToanChiNganSachSuDung(String MaKhoi, String MaND,String iID_MaTrangThaiDuyet)
        {
            String[] dsKhoi;
            dsKhoi = MaKhoi.Split(',');
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < dsKhoi.Length; i++)
            {
                DK += "iID_MaKhoiDonVi=@iID_MaKhoiDonVi" + i;
                cmd.Parameters.AddWithValue("@iID_MaKhoiDonVi" + i, dsKhoi[i]);
                if (i < dsKhoi.Length - 1)
                    DK += " OR ";
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = " AND (" + DK + ")";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
           
            String SQL = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,DT_ChungTuChiTiet.sMoTa";
            SQL += " FROM DT_ChungTuChiTiet";
            SQL += " inner join (SELECT iID_MaDonVi as MaDonVi,iID_MaKhoiDonVi FROM NS_DonVi  WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ) as NS_DonVi ON DT_ChungTuChiTiet.iID_MaDonVi=NS_DonVi.MaDonVi";
            SQL += " WHERE sLNS='1010000' AND sL='460' AND sK='468' AND bChiNganSach=1 AND DT_ChungTuChiTiet.iTrangThai=1  {0} " + DK +  ReportModels.DieuKien_NganSach(MaND) ;
            SQL += " Group By sLNS,sL,sK,sM,sTM,sTTM,sNG,DT_ChungTuChiTiet.sMoTa";
            SQL = string.Format(SQL, iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);

            cmd.Dispose();
            cmd = new SqlCommand();
            for (int i = 0; i < dsKhoi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaKhoiDonVi" + i, dsKhoi[i]);
            }
            String SQLData = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaKhoiDonVi,SUM(rTongSo)";
            SQLData += " FROM DT_ChungTuChiTiet";
            SQLData += " inner join (SELECT iID_MaDonVi as MaDonVi,iID_MaKhoiDonVi FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON DT_ChungTuChiTiet.iID_MaDonVi=NS_DonVi.MaDonVi";
            SQLData += " WHERE sLNS='1010000'  AND sL='460' AND sK='468' AND bChiNganSach=1 AND DT_ChungTuChiTiet.iTrangThai=1  {0} " + DK + ReportModels.DieuKien_NganSach(MaND);
            SQLData += " Group By sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaKhoiDonVi";
            SQLData=string.Format(SQLData,iID_MaTrangThaiDuyet);
            cmd.CommandText = SQLData;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dtData = Connection.GetDataTable(cmd);

            DataTable dtKQ = new DataTable();
            for (int i = 0; i < dsKhoi.Length; i++)
            {
                dt.Columns.Add("rK" + i, typeof(Decimal));
            }
            dt.Columns.Add("rT", typeof(Decimal));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Decimal T = 0;
                for (int k = 0; k < dsKhoi.Length; k++)
                {
                    String xauTruyVan = String.Format("sM='{0}' AND sTM='{1}' AND sTTM='{2}' AND sNG='{3}' AND iID_MaKhoiDonVi='{4}'",
                                                       dt.Rows[i]["sM"], dt.Rows[i]["sTM"], dt.Rows[i]["sTTM"], dt.Rows[i]["sNG"], dsKhoi[k]);
                    DataRow[] R = dtData.Select(xauTruyVan);
                    for (int j = 0; j < R.Length; j++)
                    {

                        dt.Rows[i]["rK" + k] = R[0][8];
                        if (R[0][8] != DBNull.Value)
                            T += Convert.ToDecimal(R[0][8]);
                    }
                }

                dt.Rows[i]["rT"] = T;
            }


            return dt;
        }
        /// <summary>
        /// Dự toán chi ngân sách quốc phòng phần ngân sách xây dựng cơ bản mẫu 3-C
        /// </summary>
        /// <param name="NamLamViec">LNS:1030100; L:460;K:468</param>
        /// <returns></returns>
        public static DataTable DT_DuToanChiNganSachQuocPhong_XDCB(String NamLamViec)
        {
            String SQL = "SELECT DM.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SQL += ",SUM(rTongSo) as rTongSo";
            SQL += ",SUM(rPhanCap) as rTuChi";
            SQL += ",SUM(rDuPhong) as rDuPhong";
            SQL += " FROM (SELECT ";
            SQL += " iID_MaDonVi";
            SQL += ",sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";
            SQL += ",rTongSo=SUM(rPhanCap+rDuPhong)";
            SQL += ",SUM(rPhanCap) as rPhanCap";
            SQL += ",SUM(rDuPhong) as rDuPhong";
            SQL += " FROM DT_ChungTuChiTiet ";
            SQL += " WHERE sLNS='1030100' AND sL='460' AND sK='468'  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iNamLamViec=@iNamLamViec";
            SQL += " Group BY  iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTonKho,rHangNhap";
            SQL += " ) ChiTiet";
            SQL += " INNER JOIN NS_DonVi ON ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi ";
            SQL += " INNER JOIN (SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang)) DM ";
            SQL += " ON NS_DonVi.iID_MaKhoiDonVi=DM.iID_MaDanhMuc";
            SQL += " GROUP By DM.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@sTenBang", "KhoiDonVi");
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Dự toán chi ngân sách sử dụng năm Phụ lục 2d-c
        /// </summary>
        /// <param name="NamLamViec">LNS:1020200; L:460;K:468</param>
        /// <returns></returns>
        public static DataTable DT_DuToanChiNganSachQuocPhongNamTuyVien(String MaND,String iID_MaTrangThaiDuyet)
        {
            String SQL = string.Format(@"select sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa, SUM(rHangNhap) rHangNhap, SUM(rTuchi) rTuchi,SUM(rHangNhap) + SUM(rTuchi) rTongSo  from
                                        DT_ChungTuChiTiet
                                        where sLNS='1020200' AND sL='460' AND sK='468' and iTrangThai = 1
                                        and iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
                                        group by sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa
                                        order by sLNS asc, sL asc, sK asc, sM asc, sTM asc, sTTM asc, sNG asc", ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// DỰ TOÁN NGÂN SÁCH NHÀ NƯỚC GIAO NĂM 2012 PHỤ LỤC 5-C
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_DuToanNganSachNhaNuoc(String MaND,String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
           
            String SQL = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";
            SQL += ",rTongSo1=Sum(rChiTaiKhoBac+rTuChi+rPhanCap+rDuPhong)";
            SQL += ",Sum(rChiTaiKhoBac) AS rChiTaiKhoBac";
            SQL += ",rTongSo2=Sum(rTuChi+rPhanCap+rDuPhong)";
            SQL += ",Sum(rTuChi) AS rTuChi";
            SQL += ",Sum(rPhanCap) AS rPhanCap";
            SQL += ",Sum(rDuPhong) as rDuPhong";
            SQL += " FROM DT_ChungTuChiTiet";
            SQL += " WHERE SUBSTRING(sLNS,1,1)=2";
            SQL += " AND iTrangThai=1";
            SQL += " {0}";
            SQL += " {1}";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";
            SQL = String.Format(SQL, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable DT_DTCNSQP_ChiChoDoanhNghiep(String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = null;
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = "SELECT NS_DonVi.sTen,sM,sTM,sTTM,sNG,rTongSo,ChiTiet.sMoTa ";
            SQL += " FROM ((SELECT iID_MaDonVi,sM,sTM,sTTM,sNG,rTongSo,sMoTa FROM DT_ChungTuChiTiet";
            SQL += " WHERE sLNS=@sLNS AND sL=@sL AND sK=@sK {0} AND iTrangThai=1 {1}";
            //SQL += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            SQL += ") ChiTiet";
            SQL += " INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=ChiTiet.iID_MaDonVi)";
            SQL += " ORDER BY NS_DonVi.sTen";
            SQL = String.Format(SQL, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", "1050000");
            cmd.Parameters.AddWithValue("@sL", "460");
            cmd.Parameters.AddWithValue("@sK", "468");
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            dt = Connection.GetDataTable(cmd);

     
            cmd.Dispose();

            SQL = "SELECT distinct sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SQL += " FROM ((SELECT iID_MaDonVi,sM,sTM,sTTM,sNG,rTongSo,sMoTa FROM DT_ChungTuChiTiet";
            SQL += " WHERE sLNS=@sLNS AND sL=@sL AND sK=@sK  AND iTrangThai=1 {0} {1}";
            //SQL += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
             SQL +=" ) ChiTiet";
            SQL += " INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=ChiTiet.iID_MaDonVi)";
            SQL = String.Format(SQL, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", "1050000");
            cmd.Parameters.AddWithValue("@sL", "460");
            cmd.Parameters.AddWithValue("@sK", "468");
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dtMucLuc = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataTable dtKQ = new DataTable();
            dtKQ.Columns.Add("sTen", typeof(String));
            dtKQ = HamChung.SelectDistinct("dtKQ", dt, "sTen", "sTen", "");
            for (int i = 0; i < dtMucLuc.Rows.Count; i++)
            {

                String TenCot = Convert.ToString(dtMucLuc.Rows[i]["sM"]);
                TenCot += "." + Convert.ToString(dtMucLuc.Rows[i]["sTM"]);
                TenCot += "." + Convert.ToString(dtMucLuc.Rows[i]["sTTM"]);
                TenCot += "." + Convert.ToString(dtMucLuc.Rows[i]["sNG"]);
                TenCot +="   "+ Convert.ToString(dtMucLuc.Rows[i]["sMoTa"]);
                //TenCot += " Bằng tiền";
                dtKQ.Columns.Add(TenCot, typeof(Decimal));


            }            
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String TenCot = Convert.ToString(dt.Rows[i]["sM"]);
                TenCot += "." + Convert.ToString(dt.Rows[i]["sTM"]);
                TenCot += "." + Convert.ToString(dt.Rows[i]["sTTM"]);
                TenCot += "." + Convert.ToString(dt.Rows[i]["sNG"]);
                TenCot += "   " + Convert.ToString(dt.Rows[i]["sMoTa"]);
                //TenCot += " Bằng tiền";
                for (int j = 0; j < dtKQ.Rows.Count; j++)
                {
                    if (dt.Rows[i]["sTen"].Equals(dtKQ.Rows[j]["sTen"]))
                    {
                        dtKQ.Rows[j][TenCot] = dt.Rows[i]["rTongSo"];
                        break;
                    }
                }
                
            }

            return dtKQ;
        }

        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String NamLamViec)
        {
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen,TenHT
                                        FROM ( SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1) as b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String NamLamViec, String sLNS)
        {
            String[] arrLNS = sLNS.Split(',');
            String Dk = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                Dk += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    Dk += " OR ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen,TenHT
                                        FROM ( SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND ({0}) AND iNamLamViec=@NamLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1) as b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi", Dk);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy đợt ngân sách
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static DataTable LayDotNganSach(String UserName,String sLNS)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserName);
            String iNamLamViec = "0000";
            String iiD_MaNamNganSach = Guid.Empty.ToString();
            String iID_MaNguonNganSach = Guid.Empty.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
                iiD_MaNamNganSach = dtCauHinh.Rows[0]["iiD_MaNamNganSach"].ToString();
                iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT  DISTINCT iID_MaDotNganSach,dNgayDotNganSach as Ngay, Convert(varchar(10),dNgayDotNganSach,103) as dNgayDotNganSach
                                        FROM DT_DotNganSach
                                        WHERE iNamLamViec=@iNamLamViec AND 
                                             iTrangThai=1 AND
                                             iID_MaNamNganSach=@iID_MaNamNganSach AND
                                             iID_MaNguonNganSach=@iID_MaNguonNganSach AND
                                            (CHARINDEX(';{0}',sDSLNS)>0  OR CHARINDEX('{0}',sDSLNS)=1)
                                        ORDER BY  Ngay",sLNS);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iiD_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();          
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotNganSach"] = "00000000-0000-0000-0000-000000000001";
                dr["dNgayDotNganSach"] = "-- Chọn tất cả đợt --";
                dt.Rows.InsertAt(dr, 0);
            }
            DataRow dr1 = dt.NewRow();
            dr1["iID_MaDotNganSach"] = "00000000-0000-0000-0000-000000000000";
            dr1["dNgayDotNganSach"] = "-- Chọn đợt --";
            dt.Rows.InsertAt(dr1, 0);
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy chứng từ
        /// </summary>
        /// <param name="iID_MaDotNganSach">Đợt ngân sách</param>
        /// <param name="UserName">Mã người dùng</param>
        /// <returns></returns>
        public static DataTable LayChungTu(String iID_MaDotNganSach, String UserName,String sLNS)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserName);
            String iNamLamViec = "0000";
            String iiD_MaNamNganSach = Guid.Empty.ToString();
            String iID_MaNguonNganSach = Guid.Empty.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
                iiD_MaNamNganSach = dtCauHinh.Rows[0]["iiD_MaNamNganSach"].ToString();
                iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT  DISTINCT iID_MaChungTu,iSoChungTu, Convert(varchar(10),dNgayDotNganSach,103) as dNgayDotNganSach,sTienToChungTu + Convert(varchar,iSoChungTu) as sSoChungTu
                                        FROM DT_ChungTu
                                        WHERE   iNamLamViec=@iNamLamViec AND 
                                                iTrangThai=1 AND 
                                                iiD_MaNamNganSach=@iiD_MaNamNganSach AND
                                                iID_MaNguonNganSach=@iID_MaNguonNganSach AND
                                                iID_MaDotNganSach=@iID_MaDotNganSach AND 
                                                sID_MaNguoiDungTao=@sID_MaNguoiDungTao AND 
                                                (CHARINDEX(';{0}',sDSLNS)>0  OR CHARINDEX('{0}',sDSLNS)=1)
                                        ORDER BY  iSoChungTu",sLNS);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iiD_MaNamNganSach", iiD_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", UserName);
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", iID_MaDotNganSach);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaChungTu"] = "00000000-0000-0000-0000-000000000001";
                dr["sSoChungTu"] = "-- Chọn tất cả chứng từ --";
                dt.Rows.InsertAt(dr, 0);
            }
            DataRow dr1 = dt.NewRow();
            dr1["iID_MaChungTu"] = "00000000-0000-0000-0000-000000000000";
            dr1["sSoChungTu"] = "-- Chọn chứng từ --";
            dt.Rows.InsertAt(dr1, 0);
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// danh sách ngành theo MaND, và sLNS
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable dtNganh_PC(String MaND, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "",DK1="";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            if (String.IsNullOrEmpty(sLNS)) sLNS = Guid.Empty.ToString();
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += " sLNS LIKE '" + arrLNS[i] + "%'";
                if (i < arrLNS.Length - 1) DK += " OR ";
            }
            DK = " AND (" + DK + ")";

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            SQL = String.Format(@"
SELECT DISTINCT sNG  FROM DT_ChungTuChiTiet_PhanCap WHERE iTrangThai=1 {0} {1} {2} {3}
 ", DK, DKDonVi, DKLNS, DKPhongBan);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DK = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DK+=" iID_MaNganhMLNS LIKE '%"+dt.Rows[i]["sNG"]+"%'";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
            }

            if (sTenPB != "02" && sTenPB != "11")
            {
                DK1 += " AND sMaNguoiQuanLy like '%" + MaND + "%'";
            }
            SQL = String.Format(@"SELECT * FROM NS_MucLucNganSach_Nganh
            WHERE iTrangThai=1 AND ({0}) {1}",DK,DK1);
            dt = Connection.GetDataTable(SQL);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtNganh(String MaND, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "", DK1 = "";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            if (String.IsNullOrEmpty(sLNS)) sLNS = Guid.Empty.ToString();
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += " sLNS LIKE '" + arrLNS[i] + "%'";
                if (i < arrLNS.Length - 1) DK += " OR ";
            }
            DK = " AND (" + DK + ")";

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            SQL = String.Format(@"
SELECT DISTINCT sNG  FROM DT_ChungTuChiTiet WHERE iTrangThai=1 {0} {1} {2} {3}
 ", DK, DKDonVi, DKLNS, DKPhongBan);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DK = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DK += " iID_MaNganhMLNS LIKE '%" + dt.Rows[i]["sNG"] + "%'";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
            }

            if (sTenPB != "02" && sTenPB != "11")
            {
                DK1 += " AND sMaNguoiQuanLy like '%" + MaND + "%'";
            }
            SQL = String.Format(@"SELECT * FROM NS_MucLucNganSach_Nganh
            WHERE iTrangThai=1 AND ({0}) {1}", DK, DK1);
            dt = Connection.GetDataTable(SQL);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtDonVi(String MaND,String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
          //  DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            if (String.IsNullOrEmpty(sLNS)) sLNS = Guid.Empty.ToString();
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += " sLNS LIKE '"+arrLNS[i]+"%'";
                if (i < arrLNS.Length - 1) DK += " OR ";
            }
            DK = " AND (" + DK + ")";

            //Lay toan bo LNS
            if (sLNS == "ALL") DK = "";

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            SQL = String.Format(@" SELECT a.iID_MaDonVi,a.iID_MaDonVi+' - '+sTen as TenHT FROM(
SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi <>''  {0} {1} {2} {3}  AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)
                                    UNION
                                SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet_PhanCap WHERE iTrangThai=1 AND iID_MaDonVi <>''  {0} {1} {2} {3}   AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)
) a
  
  INNER JOIN (SELECT DISTINCT iID_MaDonVi,sTen FROM
		 NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as b
ON a.iID_MaDonVi=b.iID_MaDonVi ORDER BY a.iID_MaDonVi", DK, DKDonVi, DKLNS, DKPhongBan);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtDonVi_KT(String MaND, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            //  DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            if (String.IsNullOrEmpty(sLNS)) sLNS = Guid.Empty.ToString();
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += " sLNS LIKE '" + arrLNS[i] + "%'";
                if (i < arrLNS.Length - 1) DK += " OR ";
            }
            DK = " AND (" + DK + ")";

            //Lay toan bo LNS
            if (sLNS == "ALL") DK = "";

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND sM IN (6900,7000,7750,9050) AND iID_MaDonVi <>''  {0} {1} {2} {3}", DK, DKDonVi, DKLNS, DKPhongBan);
            cmd.CommandText = SQL;


            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtDonVi_ChungTu(String iID_MaChungTu)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";

            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu AND iID_MaDonVi<>''");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtDonVi_1010000(String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";

            DK = " SLNS ='1010000'";
            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,iID_MaDonVi+'-'+sTenDonVi as TenHT FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.CommandText = SQL;
           

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        

        public static DataTable dtPhongBan(String MaND, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBanLNS = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            String iID_MaPhongBan = "";
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                dtPhongBan.Dispose();
            }
           
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBanLNS);
            if (String.IsNullOrEmpty(sLNS)) sLNS = Guid.Empty.ToString();
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += " sLNS LIKE '" + arrLNS[i] + "%'";
                if (i < arrLNS.Length - 1) DK += " OR ";
            }
            DK = " AND (" + DK + ")";

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan as TenHT FROM DT_ChungTuChiTiet WHERE iTrangThai=1  {0} {1} {2} {3}", DK, DKDonVi, DKLNS, DKPhongBan);
            cmd.CommandText = SQL;


            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["TenHT"] = "--Chọn tất cả các B--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public static DataTable dtTo(String MaND,String sLNS)
        {
            DataTable dtDV = dtDonVi(MaND, sLNS);
            int SoToTrang1 = 5;
            int SoToTrang2 = 6;
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDV.Rows.Count; i++)
            {
                iID_MaDonVi += dtDV.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }

            String[] arrDonVi = iID_MaDonVi.Split(',');
            dtDV.Dispose();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));

            if (arrDonVi.Length > 0 && !String.IsNullOrEmpty(iID_MaDonVi))
            {
                for (int i = 0; i < SoToTrang1; i = i + SoToTrang1)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "Tờ 1";
                    dr[1] = "1";
                    dt.Rows.InsertAt(dr, 0);
                }
                int a = 2;
                for (int i = SoToTrang1; i < arrDonVi.Length; i = i + SoToTrang2)
                {
                    DataRow dr1 = dt.NewRow();
                    dt.Rows.Add(dr1);
                    dr1[0] = "Tờ " + a;
                    dr1[1] = a;
                    a++;
                }
            }
            dtDV.Dispose();
            dt.Dispose();
            return dt;

        }
        //HungPH: them dt Lay tat ca phong ban; Ngay 25-9-2015
        public static DataTable getDSPhongBanAll(String iNamLamViec, String MaND) {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            String DK = "";
            if (sTenPB == "02" || sTenPB == "2")
            {

            }
            else
            {
                DK = " AND iID_MaPhongBan=@iID_MaPhongBanND";
            }

            String SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}
                                        ", DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaPhongBanND", sTenPB);
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["sTenPhongBan"] = "--Chọn tất cả các B--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        //HungPH: them dt lay dsDonVi tu dieu kien phong ban va Dot
        public static DataTable getdtPhongBan_DonVi(String iID_Dot, String iID_MaPhongBan) 
        {
            String DKPhongBan = "";
            String DKDot = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (!String.IsNullOrEmpty(iID_Dot) && iID_Dot != "-1")
            {
                //DKDot += "AND iID_MaDotNganSach=@MaDot";
                //cmd.Parameters.AddWithValue("@MaDot", iID_Dot);
            }
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DKPhongBan += "AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }
            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND sLNS='1040100' AND sTenDonVi<>'' AND iID_MaDonVi<>'' {0} {1}
                                        GROUP BY iID_MaDonVi,sTenDonVi
                                        HAVING SUM(rTuChi)<>0 
										OR SUM(rHangNhap)<>0 
										OR SUM(rTonKho)<>0  
										OR SUM(rHangMua)<>0  
										OR SUM(rPhanCap)<>0 
										OR SUM(rDuPhong)<>0
                                        ORDER BY iID_MaDonVi
                                        ", DKDot, DKPhongBan);
           
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }

        //HungPH: them dt lay dsDonVi tu dieu kien phong ban Dot va LNS

        public static DataTable getdtPhongBan_LNS_DonVi(String MaND, String iID_MaDot, String iID_MaPhongBan, String sLNS)
        {
            String DKPhongBan = "";
            String DKDot = "";
            String DKLNS = "";
            String DKDonVi = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            //DKLNS += ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBanND);
            //DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            //DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                DKDot += " AND CONVERT(VARCHAR(24),dNgayChungTu,105)=@MaDot";
                cmd.Parameters.AddWithValue("@MaDot", iID_MaDot);
            }
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
            }

            String[] arrDonVi = sLNS.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKLNS += " sLNS=@LNS" + i;
                cmd.Parameters.AddWithValue("@LNS" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DKLNS += " OR ";
                }
            }

            if (!String.IsNullOrEmpty(DKLNS))
            {
                DKLNS = " AND (" + DKLNS + ")";
            }

            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND sTenDonVi<>'' AND iID_MaDonVi<>'' AND iID_MaChungTu IN
                                                                            (SELECT iID_MaChungTu FROM DT_ChungTu WHERE iTrangThai=1 {0} ) {1} {2} {3}
                                        GROUP BY iID_MaDonVi,sTenDonVi
                                        HAVING SUM(rTuChi)<>0 
										OR SUM(rHangNhap)<>0 
										OR SUM(rHienVat)<>0  
										OR SUM(rHangMua)<>0  
										OR SUM(rPhanCap)<>0 
										OR SUM(rDuPhong)<>0
                                        ORDER BY iID_MaDonVi
                                        ", DKDot, DKPhongBan, DKLNS, DKDonVi);

            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
        public static DataTable getDSPhongBan(String iNamLamViec, String MaND,String sLNS)
        {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            String DK = "";


            if (sTenPB == "02" || sTenPB == "2")
            {

            }
            else
            {
                DK = " AND iID_MaPhongBan=@iID_MaPhongBan";
            }

            DK += " AND (SUBSTRING(sLNS,1,1) IN ( " + sLNS + ") OR SUBSTRING(sLNS,1,3) IN ( " + sLNS + ") OR SUBSTRING(sLNS,1,7) IN ( " + sLNS + ") )";
                
            String SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}

", DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["sTenPhongBan"] = "--Chọn tất cả các B--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public static String getTenNganh(String iMaNganh)
        {
            String SQL = String.Format(@"SELECT sTen FROM DC_DanhMuc
WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc
WHERE sTenBang='Nganh' AND iTrangThai=1) AND sTenKhoa=@sTenKhoa");
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", iMaNganh);
            String  sTenNganh = Connection.GetValueString(cmd, "");
            return sTenNganh;
        }
        public static DataTable dtKieuXem()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iID"] = "DonVi";
            dr["sTen"] = "Theo đơn vị";
            dt.Rows.Add(dr);

             dr = dt.NewRow();
            dr["iID"] = "NS";
            dr["sTen"] = "Theo loại NS";
            dt.Rows.Add(dr);
            dt.Dispose();

            return dt;
        }
        public static DataTable dtDonViTinh()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iID"] = "1";
            dr["sTen"] = "Đồng";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "1000";
            dr["sTen"] = "Nghìn đồng";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "1000000";
            dr["sTen"] = "Triệu đồng";
            dt.Rows.Add(dr);
            dt.Dispose();

            return dt;
        }
        public static DataTable dtPhongBanInBaoDam()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iID"] = "0";
            dr["sTen"] = "--Tất cả--";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "06";
            dr["sTen"] = "Chỉ cục quản lý doanh nghiệp";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "07";
            dr["sTen"] = "Phòng quản lý ngân sách sử dụng";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "10";
            dr["sTen"] = "Phòng quản lý ngân sách bảo đảm";
            dt.Rows.Add(dr);
            dt.Dispose();

            return dt;
        }
        public static DataTable dtNguonInBaoDam()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iID"] = "0";
            dr["sTen"] = "-- Tất cả --";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "1040100";
            dr["sTen"] = "Từ 104";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["iID"] = "109";
            dr["sTen"] = "Từ 109";
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable getdtTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("sTen", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["sTen"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["sTen"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
        #endregion

        #region Source mới, khi chuyển method từ Controller xuống Models
        #region HungPH: rptDuToan_TongHop_PhongBan_DonVi ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu từ data
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaDot">Mã đợt</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <param name="LoaiTongHop">Xuất chi tiết hoặc tổng hợp</param>
        /// HungPH
        public static DataTable rptDuToanTongHopPhongBanDonVi(String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String LoaiTongHop)
        {
            SqlCommand cmd = new SqlCommand();
            String DKPhongBan = "";
            String DKDonVi = "";
            String DK = "";
            String DKDot = "";
            String DKLoaiBaoCao = "";
            String SQL = "";
            int DVT = 1000;

            DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            //dk LNS
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            else
            {
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    DK += "sLNS=@sLNS" + i;
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                    if (i < arrLNS.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            //dk Dot
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                DK += " AND CONVERT(VARCHAR(24),dNgayChungTu,105)=@MaDot ";
                cmd.Parameters.AddWithValue("@MaDot", iID_MaDot);
            }

            //dk phong ban
            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            ////Báo cáo chi tiết các đơn vị
            if (LoaiTongHop == "ChiTiet")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
                {
                    DKLoaiBaoCao += " AND iID_MaDonVi=@iID_MaDonVi ";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            //Báo cáo tổng hợp các đơn vị
            else
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();

                String[] arrDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DKLoaiBaoCao += " iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DKLoaiBaoCao += " OR ";
                }

                if (!String.IsNullOrEmpty(DKLoaiBaoCao))
                {
                    DKLoaiBaoCao = " AND (" + DKLoaiBaoCao + ") ";
                }
                   
            }

            SQL = String.Format(@"
                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                                ,rTuChi=SUM(rTuChi)/{3}
                                ,rHienVat=SUM(rHienVat)/{3}
                                ,rHangNhap=SUM(rHangNhap)/{3}
                                ,rHangMua=SUM(rHangMua)/{3}
                                ,rPhanCap=SUM(rPhanCap)/{3}
                                ,rDuPhong=SUM(rDuPhong)/{3}
                        FROM DT_ChungTuChiTiet 
                        WHERE iTrangThai=1 
                                AND iNamLamViec=@iNamLamViec 
                                AND iID_MaChungTu IN (SELECT iID_MaChungTu FROM DT_ChungTu WHERE iTrangThai=1 {0} ) {1} {5} {2} {4}
                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                        HAVING SUM(rTuChi)<>0 
                                OR SUM(rHangNhap)<>0 
                                OR SUM(rHienVat)<>0  
                                OR SUM(rHangMua)<>0  
                                OR SUM(rPhanCap)<>0 
                                OR SUM(rDuPhong)<>0", DK, DKDonVi, DKPhongBan, DVT, DKDot, DKLoaiBaoCao);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);

            return dtAll;
        }

        /// <summary>
        /// Lấy danh sách đơn vị từ đợt, phòng ban, SLN
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDot"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable laydtPhongBan_LNS_DonVi(String MaND, String iID_MaDot, String iID_MaPhongBan, String sLNS)
        {
            String DKPhongBan = "";
            String DKDot = "";
            String DKLNS = "";
            String DKDonVi = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            //DKLNS += ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBanND);
            //DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            //DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                DKDot += " AND CONVERT(VARCHAR(24),dNgayChungTu,105)=@MaDot";
                cmd.Parameters.AddWithValue("@MaDot", iID_MaDot);
            }
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
            }

            String[] arrDonVi = sLNS.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKLNS += " sLNS=@LNS" + i;
                cmd.Parameters.AddWithValue("@LNS" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DKLNS += " OR ";
                }
            }

            if (!String.IsNullOrEmpty(DKLNS))
            {
                DKLNS = " AND (" + DKLNS + ")";
            }

            SQL = String.Format(@"
                                    SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT
                                    FROM DT_ChungTuChiTiet
                                    WHERE iTrangThai=1 AND sTenDonVi<>'' 
                                                        AND iID_MaDonVi<>'' 
                                                        AND iID_MaChungTu 
                                                        IN (SELECT iID_MaChungTu FROM DT_ChungTu WHERE iTrangThai=1 {0} ) {1} {2} {3}
                                    GROUP BY iID_MaDonVi,sTenDonVi
                                    HAVING SUM(rTuChi)<>0 
										OR SUM(rHangNhap)<>0 
										OR SUM(rHienVat)<>0  
										OR SUM(rHangMua)<>0  
										OR SUM(rPhanCap)<>0 
										OR SUM(rDuPhong)<>0
                                    ORDER BY iID_MaDonVi
                                    ", DKDot, DKPhongBan, DKLNS, DKDonVi);

            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        //HungPH: lấy danh sách đợt dự toán
        public static DataTable LayDotDuToan(String iNamLamViec, String MaND)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDot", typeof(String));
            dt.Columns.Add("iDotCap", typeof(String));
            DataRow dr = dt.NewRow();
            String SQL = String.Format(@"
                SELECT DISTINCT CONVERT(VARCHAR(24),dNgayChungTu,105) as iDotCap,dNgayChungTu
                FROM DT_ChungTu
                WHERE iTrangThai=1 
                    AND iNamLamViec=@iNamLamViec
                    AND iID_MaChungTu IN 
                    (
                        SELECT iID_MaChungTu 
                        FROM DT_ChungTuChiTiet 
                        WHERE iTrangThai=1 AND rTuChi<>0 AND iID_MaDonVi<>''
                    ) 
                ORDER BY dNgayChungTu");

            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        #endregion

        #region HungPX: VungNV update code 
                public static dataDuLieu1 get_dtDuToan_1050000(String MaND, String Nganh, String ToSo, String MaDot, String iID_MaPhongBan)
        {
            int i = 0;
            String DSNganh = "", DK = "", DKPB = "";
            SqlCommand cmdNG = new SqlCommand();
            if (!String.IsNullOrEmpty(MaDot) && MaDot != "-1")
            {

                DK += "and iID_MaDotNganSach = @MaDot";
                cmdNG.Parameters.AddWithValue("@MaDot", MaDot);
            }

            String iID_MaNganhMLNS = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID", Nganh, "iID_MaNganhMLNS"));

            if (iID_MaPhongBan != "0")
            {
                DKPB = " AND iID_MaPhongBanDich=@iID_MaPhongBan";
                cmdNG.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }
            //DSNganh = " AND sNG IN (" + iID_MaNganhMLNS + ")";
            DSNganh = " AND sNG IN (@iID_MaNganhMLNS)";
            cmdNG.Parameters.AddWithValue("@iID_MaNganhMLNS", iID_MaNganhMLNS);

            if (String.IsNullOrEmpty(iID_MaNganhMLNS)) DSNganh = " AND sNG IN (123)";
            String SQLNganh = "SELECT distinct sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQLNganh += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQLNganh += " FROM DT_ChungTuChiTiet_PhanCap WHERE sLNS='1020100' AND MaLoai<>'1' {1} AND iTrangThai=1 {2}  {0} {3} {4}";

            SQLNganh += " UNION SELECT distinct sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQLNganh += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQLNganh += " FROM DT_ChungTuChiTiet WHERE sLNS=1040100 AND iKyThuat=1 AND MaLoai=1 {1} AND iTrangThai=1 {2}  {0}  {3} {4} ORDER BY sM,sTM,sTTM,sNG";
            SQLNganh = String.Format(SQLNganh, DSNganh, ReportModels.DieuKien_NganSach_KhongDV(MaND), "", DKPB, DK);

            cmdNG.CommandText = SQLNganh;

            DataTable dtNG = Connection.GetDataTable(cmdNG);
            cmdNG.Dispose();
            String SQL;
            DataTable dtDonVi;

            String DVT = "1000";
            SQL = "SELECT iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQL += ",SUM(rTuChi/{3}) AS rTuChi";
            SQL += ",SUM(rHienVat/{3}) AS rHienVat";
            SQL += " FROM DT_ChungTuChiTiet_PhanCap WHERE sLNS='1020100' AND MaLoai<>'1' {1}      AND iTrangThai=1 {2}   {0} {4} {5}";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,iID_MaDonVi,sMoTa";
            SQL += " HAVING SUM(rTuChi)>0 OR SUM(rHienVat)>0";
            SQL += " UNION SELECT iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQL += ",SUM(rTuChi/{3}) AS rTuChi";
            SQL += ",SUM(rHienVat/{3}) AS rHienVat";
            SQL += " FROM DT_ChungTuChiTiet WHERE sLNS='1040100' AND iKyThuat=1   AND MaLoai=1 {1} AND iTrangThai=1 {2}   {0} {4} {5}";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,iID_MaDonVi,sMoTa";
            SQL += " HAVING SUM(rTuChi)>0 OR SUM(rHienVat)>0";
            SQL = String.Format(SQL, DSNganh, ReportModels.DieuKien_NganSach_KhongDV(MaND), "", DVT, DKPB, DK);

            String strSQL = "SELECT CT.iID_MaDonVi,CT.iID_MaDonVi+' - '+ NS_DonVi.sTen AS TenDonVi";
            strSQL += ",sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,CT.sMoTa";
            strSQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            strSQL += ",rTuChi,rHienVat";
            strSQL += " FROM ({0}) CT ";
            strSQL += " INNER JOIN (SELECT iID_MaDonVi as MaDonVi, sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON NS_DonVi.MaDonVi=CT.iID_MaDonVi";
            strSQL = String.Format(strSQL, SQL);
            SqlCommand cmd = new SqlCommand(strSQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNganhMLNS", iID_MaNganhMLNS);
            if (iID_MaPhongBan != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }
            if (!String.IsNullOrEmpty(MaDot) && MaDot != "-1")
            {
                cmd.Parameters.AddWithValue("@MaDot", MaDot);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            dtDonVi = HamChung.SelectDistinct("dtDonVi", dt, "iID_MaDonVi", "iID_MaDonVi,TenDonVi");

            i = 0;
            //cs = 3;//tờ 1 4 cột
            dtDonVi.Columns.Add("TongTuChi", typeof(Decimal));
            dtDonVi.Columns.Add("TongHienVat", typeof(Decimal));
            while (i < dtNG.Rows.Count)
            {
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_TuChi") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_TuChi", typeof(Decimal));
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_HienVat") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_HienVat", typeof(Decimal));
                i = i + 1;
            }

            i = 0;

            String MaDonVi, MaDonVi1, TenCot;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]).Trim();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    MaDonVi1 = Convert.ToString(dt.Rows[j]["iID_MaDonVi"]).Trim();
                    TenCot = Convert.ToString(dt.Rows[j]["NG"]).Trim();
                    if (MaDonVi == MaDonVi1 && dtDonVi.Columns.IndexOf(TenCot + "_TuChi") >= 0)
                    {
                        dtDonVi.Rows[i][TenCot + "_TuChi"] = dt.Rows[j]["rTuChi"];
                        dtDonVi.Rows[i][TenCot + "_HienVat"] = dt.Rows[j]["rHienVat"];
                        dt.Rows.RemoveAt(j);
                        j = j - 1;
                    }
                }
            }
            i = 0;
            //j=4 vì trừ cột madv, đơn vị và 2 cột tổng cộng
            Double Tong = 0;
            for (int j = 4; j < dtDonVi.Columns.Count; j++)
            {
                Tong = 0;
                for (i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        Tong = Tong + Convert.ToDouble(dtDonVi.Rows[i][j]);
                    }
                }
                if (Tong == 0)
                {
                    dtDonVi.Columns.RemoveAt(j);
                    if (j == 1) j = 1;
                    else j = j - 1;
                }
            }
            Double TongHienVat = 0, TongTuChi = 0;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                TongHienVat = 0; TongTuChi = 0;
                //j=4 vì trừ cột MaDV, đơn vị và 2 cột tổng cộng
                for (int j = 4; j < dtDonVi.Columns.Count; j++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        if (dtDonVi.Columns[j].ColumnName.IndexOf("_HienVat") >= 0)
                        {
                            TongHienVat = TongHienVat + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                        else
                        {
                            TongTuChi = TongTuChi + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                    }
                }
                dtDonVi.Rows[i]["iID_MaDonVi"] = (i + 1).ToString();
                dtDonVi.Rows[i]["TongHienVat"] = TongHienVat;
                dtDonVi.Rows[i]["TongTuChi"] = TongTuChi;
            }
            DataTable _dtDonVi = new DataTable();
            DataTable _dtDonVi1 = new DataTable();

            int TongSoCot = 0;
            int SoTrang = 1;
            int SoCotCanThem = 0;
            if ((dtDonVi.Columns.Count - 4) == 0)
            {
                SoCotCanThem = 4;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else if ((dtDonVi.Columns.Count - 4) <= 4)
            {

                int SoCotDu = ((dtDonVi.Columns.Count - 4)) % 4;
                if (SoCotDu != 0)
                    SoCotCanThem = 4 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (dtDonVi.Columns.Count - 4 - 4) % 6;
                if (SoCotDu != 0)
                    SoCotCanThem = 6 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
                SoTrang = 1 + (TongSoCot - 4) / 6;
            }
            for (i = 0; i < SoCotCanThem; i++)
            {
                dtDonVi.Columns.Add();
            }
            int _ToSo = Convert.ToInt16(ToSo);
            int SoCotTrang1 = 4;
            int SoCotTrangLonHon1 = 6;
            _dtDonVi = dtDonVi.Copy();
            int _CS = 0;
            String BangTien_HienVat = "";
            //Mổ tả xâu nối mã
            ArrayList arrMoTa1 = new ArrayList();
            //Mỏ tả ngành
            ArrayList arrMoTa2 = new ArrayList();
            //Bằng Tiền hay bằng hiện vật
            ArrayList arrMoTa3 = new ArrayList();
            if (ToSo == "1")
            {

                for (i = 4; i < 4 + SoCotTrang1; i++)
                {
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + (i - 3);
                }
            }
            else
            {
                int tg = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2);
                int dem = 1;
                for (i = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2); i < 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 1); i++)
                {
                    if (i < 0)
                        break;
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + dem;
                    dem++;
                }
            }

            dataDuLieu1 _data = new dataDuLieu1();
            _data.dtDuLieu = _dtDonVi;
            _data.arrMoTa1 = arrMoTa1;
            _data.arrMoTa2 = arrMoTa2;
            _data.arrMoTa3 = arrMoTa3;
            _data.dtdtDuLieuAll = dtDonVi;
            return _data;
        }
    }

    public class dataDuLieu1
    {
        public DataTable dtDuLieu { get; set; }
        public DataTable dtdtDuLieuAll { get; set; }
        public ArrayList arrMoTa1 { get; set; }
        public ArrayList arrMoTa2 { get; set; }
        public ArrayList arrMoTa3 { get; set; }
    }
       
        #endregion

        #endregion

}