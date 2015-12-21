using System;
using System.Data.SqlClient;
using System.Data;
using DomainModel;

namespace VIETTEL.Models.DuToanBS
{
    public class DuToanBS_ReportModels
    {
        public static int DVT = 1000;

        #region Source cũ khi chưa update code.
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
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec")));
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
        public static DataTable dtDonVi_DoanhNghiep(String MaND, String sLNS)
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
            String iID_MaPhongBanQuanLy = "06";
            
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec")));
            SQL = String.Format(@" SELECT a.iID_MaDonVi,a.iID_MaDonVi+' - '+sTen as TenHT FROM(
SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaPhongBan='06' AND iID_MaDonVi <>'' AND iID_MaDonVi  NOT IN (SELECT  iID_MaDonVi FROM 
(SELECT * FROM NS_PhongBan_DonVi
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 ) a
INNER JOIN (
SELECT  * FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu IN (06))b
ON a.iID_MaPhongBan=b.iID_MaPhongBan
)
{0} {1} {2} {3}  AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)
                                    UNION
                                SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet_PhanCap WHERE iTrangThai=1  AND iID_MaPhongBan='06' AND iID_MaDonVi <>'' AND iID_MaDonVi  NOT IN (SELECT  iID_MaDonVi FROM 
(SELECT * FROM NS_PhongBan_DonVi
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 
 ) a
INNER JOIN (
SELECT  * FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu IN (06))b
ON a.iID_MaPhongBan=b.iID_MaPhongBan
)
{0} {1} {2} {3}   AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)
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

            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu AND iID_MaDonVi<>''");
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

        public static DataTable getDSPhongBan(String iNamLamViec, String MaND,String sLNS)
        {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            String DK = "";


            if (sTenPB != "02" && sTenPB != "2")
            {
                DK = " AND iID_MaPhongBan=@iID_MaPhongBan";
            }
            
            DK += " AND (SUBSTRING(sLNS,1,1) IN ( " + sLNS + ") OR SUBSTRING(sLNS,1,3) IN ( " + sLNS + ") OR SUBSTRING(sLNS,1,7) IN ( " + sLNS + ") )";
                
            String SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}", DK);

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
            dr["sTen"] = "-- Tất cả --";
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
        //quydq
        
        //quydq : Chọn 1 đợt ( lấy 1 giá trị) + phòng ban = > ĐƠN VỊ
        public static DataTable dtDonVi_Dot1(String iID_MaDot, String iID_MaPhongBan)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";
            String DKDot = "";
            DK += " AND iID_MaDonVi NOT IN ('')";
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                DKDot += " AND CONVERT(VARCHAR(24),dNgayChungTu,105)=@MaDot";
                cmd.Parameters.AddWithValue("@MaDot", iID_MaDot);
            }
            else
            {
                DK += " ";
            }
            if (iID_MaPhongBan == "-1")
            {
                DK += " ";
            }
            else
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi, sTenDonVi 
            FROM DTBS_ChungTuChiTiet
            WHERE iTrangThai=1 AND iID_MaChungTu IN
                        (SELECT iID_MaChungTu FROM DT_ChungTu WHERE iTrangThai=1 {0} ) {1} 
            ORDER BY iID_MaDonVi
            ", DKDot, DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        //quydq :  (lấy giá trị đợt từ ngày ,đến ngày) + phòng ban = > đơn vị
        public static DataTable dtDonVi_Dot2(String iID_MaDotTu,String iID_MaDotDen, String iID_MaPhongBan)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";
            String DKDOT = "";
            DK += " AND iID_MaDonVi NOT IN ('')";
            DKDOT += " AND CONVERT(VARCHAR(24),dNgayChungTu,105) BETWEEN @MaDotTu AND @MaDotDen ";
            cmd.Parameters.AddWithValue("MaDotTu", iID_MaDotTu);
            cmd.Parameters.AddWithValue("MaDotDen", iID_MaDotDen);
            //else
            //{
            //    DK += " ";
            //}
            if (iID_MaPhongBan == "-1")
            {
                DK += " ";
            }
            else
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi, sTenDonVi 
            FROM DTBS_ChungTuChiTiet
            WHERE iTrangThai=1 AND iID_MaChungTu IN
                        (SELECT iID_MaChungTu FROM DTBS_ChungTu WHERE iTrangThai=1 {0} ) {1} 
            ORDER BY iID_MaDonVi
            ",DKDOT, DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        //quydq : Chọn đơn vị => loại ngân sách
        public static DataTable dtDonVi_LNS(String iID_MaDot, String iID_MaPhongBan, String MaND, String iID_MaDonVi)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            //dieu kien don vi
            if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = "-100";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += " iID_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DK += " OR ";
                }
            }
            if (!String.IsNullOrEmpty(DK))
                DK = " AND (" + DK + ")";
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, MaPhongBan);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            //dieu kien phong ban
            if (iID_MaPhongBan != "-1" && iID_MaPhongBan != null)
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }
            SQL = String.Format(@"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT
                FROM( 
                SELECT DISTINCT  sLNS
                FROM DTBS_ChungTuChiTiet
                WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0 {0} {1} {2} {3}

                ) as a

                INNER JOIN 

                (SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7) as b
                ON a.sLNS=b.sLNS
                ORDER BY sLNS

                 ", DK, DKLNS, DKPhongBan, DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        
        
        //hungpx
        public static DataTable dtDonVi_Dot(String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";
            
            // DKPhongban
            String DKPhongBan = LayDKPhongBan(MaND,cmd);
            String DKDonVi = LayDKDonVi(MaND, cmd);
            
            // hungpx neu khong co ma dot thi chon tat ca cac dot
            if(!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot!="-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaDot",iID_MaDot);
                DK += "and CONVERT(VARCHAR(24),a.dNgayChungTu,105)=@iID_MaDot";
            }
            // hungpx: neu khong co ma phong ban thi chon tat ca phong ban
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND b.iID_MaPhongBan = @iID_MaPhongBan ";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }
            
            SQL = String.Format(@"SELECT DISTINCT b.iID_MaDonVi, b.sTenDonVi as TenHT
                                FROM DTBS_ChungTu a,DTBS_ChungTuChiTiet b
                                WHERE a.iID_MaChungTu = b.iID_MaChungTu and b.iID_MaDonVi NOT IN ('') AND b.iTrangThai=1 AND b.iNamLamViec=@iNamLamViec  {0} {1} {2}
                                ", DK,DKDonVi,DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtLNS_Dot(String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi,String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";

            String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            String DKLNS = LayDKLNS(MaND, cmd, iID_MaPhongBanND);
            String DKPhongBan = LayDKPhongBan(MaND, cmd);
            String DKDonVi = LayDKDonVi(MaND, cmd);
           
            // hungpx: neu khong co ma don vi thi khong chon don vi nao
            if (String.IsNullOrEmpty(iID_MaDonVi) || iID_MaDonVi == "-1")
            {
                iID_MaDonVi = "-100";
               
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += " b.iID_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DK += " OR ";
                }
            }
            if (!String.IsNullOrEmpty(DK))
                DK = " AND (" + DK + ")";
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaDot", iID_MaDot);
                DK += "and CONVERT(VARCHAR(24),a.dNgayChungTu,105)=@iID_MaDot";
            }
            // hungpx: neu khong co ma phong ban thi chon tat ca phong ban
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += "AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }
            SQL = String.Format(@"SELECT DISTINCT a1.sLNS,a1.sLNS+' - '+b1.sMoTa as TenHT
                                FROM(
                                SELECT DISTINCT b.sLNS,b.sLNS+' - '+b.sMoTa as TenHT
                                FROM DTBS_ChungTu a, DTBS_ChungTuChiTiet b
                                WHERE a.iID_MaChungTu = b.iID_MaChungTu and a.iTrangThai=1 AND a.iNamLamViec=@iNamLamViec {0} {1} {2} {3}
                                ) as a1
                                INNER JOIN 

                                (SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7) as b1
                                ON a1.sLNS=b1.sLNS
                                ORDER BY a1.sLNS

                                ", DK,DKDonVi,DKLNS,DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
     #endregion

        #region Source mới, Update code: 19/11/2015 chuyển method từ Controller xuống Models

        #region QuyQD: rptDuToanBS_TongHop_ChiNganSach ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ Tổng hợp chi ngân sách
        /// </summary>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <param name="iID_MaDotTu">Mã Đợt</param>
        /// <param name="iID_MaDotDen">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public static DataTable rptDuToanBS_TongHop_ChiNganSach(String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND, String LoaiBaoCao)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            String DKDot = "";
            SqlCommand cmd = new SqlCommand();
            int Dvt = 1000;
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            //Điều kiện đơn vị
            DataTable dtNĐonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dtNĐonVi.Rows.Count; i++)
            {
                DKDonVi += "A.iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < dtNĐonVi.Rows.Count - 1)
                    DKDonVi += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtNĐonVi.Rows[i]["iID_MaDonVi"]);
            }

            if (String.IsNullOrEmpty(DKDonVi))
                DKDonVi = " AND 0=1";
            else
            {
                DKDonVi = "AND (" + DKDonVi + ")";
            }

            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            String[] arrDonVi = iID_MaDonVi.Split(',');

            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += "A.iID_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DK += " OR ";
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND ( " + DK + ") ";
            }

            //điều kiện Loại Ngân Sách
            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN ( " + sLNS + ") ";
            }

            //điều kiện phòng ban
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND A.iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            DKDot += " AND CONVERT(VARCHAR(24),B.dNgayChungTu,105) BETWEEN @MaDotTu AND @MaDotDen ";

            String Sql = String.Format(@"
                                        SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                                                SUBSTRING(sLNS,1,3) as sLNS3,
                                                SUBSTRING(sLNS,1,5) as sLNS5,
                                                 sLNS,sL,sK,sM,sTM,sTTM,sNG,A.sMoTa,A.iID_MaDonVi,sTenDonVi,CONVERT(VARCHAR(24),B.dNgayChungTu,105) as NgayChungTu
                                                ,SUM(rTuChi/{4}) as rTuChi,
                                                SUM(rHangMua/{4}) as rHangMua,
                                                SUM(rHangNhap/{4}) as rHangNhap,
                                                SUM(rHienVat/{4}) as rHienVat,
                                                SUM(rPhanCap/{4}) as rPhanCap,
                                                SUM(rDuPhong/{4}) as rDuPhong
                                        FROM DTBS_ChungTuChiTiet as A INNER JOIn DTBS_ChungTu as B
                                                ON A.iID_MaChungTu = B.iID_MaChungTu
                                             WHERE A.iTrangThai=1 AND A.iNamLamViec=@iNamLamViec {0} {1} {2} {3}
                                             GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,A.sMoTa,A.iID_MaDonVi,sTenDonVi,CONVERT(VARCHAR(24),B.dNgayChungTu,105) 
                                             HAVING SUM(rTuChi)<>0 
                                                    OR SUM(rHangMua)<>0
                                                    OR SUM(rHangNhap)<>0
                                                    OR SUM(rHienVat)<>0
                                                    OR SUM(rPhanCap)<>0
                                                    OR SUM(rDuPhong)<>0", DK, DKDonVi, DKPhongBan, DKDot, Dvt);

            cmd.CommandText = Sql;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@MaDotTu", iID_MaDotTu);
            cmd.Parameters.AddWithValue("@MaDotDen", iID_MaDotDen);

            DataTable dtAll = Connection.GetDataTable(cmd);
            return dtAll;
        }

        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable LayDSPhongBan(String iNamLamViec, String MaND)
        {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            if (sTenPB != "02" && sTenPB != "2")
            {
                DK = " AND iID_MaPhongBan=@iID_MaPhongBan";
            }

            String SQL = String.Format(@"
                                        SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
                                        FROM DTBS_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} AND iID_MaPhongBan NOT IN (02)", DK);

            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["sTenPhongBan"] = "--Chọn tất cả các B--";

            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        /// <summary>
        /// lấy danh sách đợt
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable LayDSDot(String iNamLamViec, String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            dt.Columns.Add("MaDot", typeof(String));
            dt.Columns.Add("iDotCap", typeof(String));

            String SQL = String.Format(@"
                    Select DISTINCT CONVERT(VARCHAR(24),dNgayChungTu,105) as iDotCap, dNgayChungTu
                    FROM DTBS_ChungTu
                    WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec
                            AND iID_MaChungTu 
                            IN (
                                    SELECT iID_MaChungTu 
                                    FROM DTBS_ChungTuChiTiet 
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

        #region QuyDQ: rptDuToanBS_ChiTieuNganSach2 ngày 19/11/2015        
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ Chi Tiêu Ngân Sách 2
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Mã loại ngân sách</param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <returns></returns>
        public static DataTable rptDuToanBS_ChiTieuNganSach2(String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            int donvitinh = 1000;
            SqlCommand cmd = new SqlCommand();

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            else
            {
                String[] arrSLN = sLNS.Split(',');
                for (int i = 0; i < arrSLN.Length; i++)
                {
                    DK += "sLNS=@sLNS" + i;
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrSLN[i]);
                    if (i < arrSLN.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }

            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaDot", iID_MaDot);
                DK += "and iID_MaDotNganSach=@iID_MaDot";
            }

            String SQL =
                String.Format(@"
                                SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                                        SUBSTRING(sLNS,1,3) as sLNS3,
                                        SUBSTRING(sLNS,1,5) as sLNS5,
                                         sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                        ,SUM(rTuChi/{1}) as rTuChi, SUM(rHienVat/{1}) as rHienVat, SUM(rDuPhong/{1}) as rDuPhong
                                 FROM DT_ChungTuChiTiet
                                 WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {2} {3}
                                 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                 HAVING SUM(rTuChi)<>0 ", DK, donvitinh, DKDonVi, DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);

            return dtAll;
        }
        #endregion

        #region QuyDQ: rptDuToanBS_ChiTieuNganSach ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ Chi Tiêu Ngân Sách
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Mã loại ngân sách</param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <returns></returns>
        public static DataTable rptDuToanBS_ChiTieuNganSach(String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            int donvitinh = 1000;
            SqlCommand cmd = new SqlCommand();

            DKDonVi = LayDKDonVi(MaND, cmd);
            DKPhongBan = LayDKPhongBan(MaND, cmd);

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
                DK += "b.sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            else
            {
                String[] arrSLN = sLNS.Split(',');

                for (int i = 0; i < arrSLN.Length; i++)
                {
                    DK += "b.sLNS=@sLNS" + i;
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrSLN[i]);
                    if (i < arrSLN.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND b.iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND b.iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }

            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaDot", iID_MaDot);
                DK += " and CONVERT(VARCHAR(24),a.dNgayChungTu,105)=@iID_MaDot";
            }

            String SQL =
                String.Format(@"
                                SELECT SUBSTRING(b.sLNS,1,1) as sLNS1,
                                        SUBSTRING(b.sLNS,1,3) as sLNS3,
                                        SUBSTRING(b.sLNS,1,5) as sLNS5,
                                        b.sLNS,b.sL,b.sK,b.sM,b.sTM,b.sTTM,b.sNG,b.sMoTa,b.iID_MaDonVi,b.sTenDonVi,b.iID_MaPhongBan,b.sTenPhongBan
                                        ,SUM(b.rTuChi/{1}) as rTuChi, SUM(b.rHienVat/{1}) as rHienVat
                                 FROM DTBS_ChungTu a, DTBS_ChungTuChiTiet b
                                 WHERE a.iID_MaChungTu = b.iID_MaChungTu and b.iTrangThai=1 AND b.iNamLamViec=@iNamLamViec {0} {2} {3}
                                 GROUP BY b.sLNS,b.sL,b.sK,b.sM,b.sTM,b.sTTM,b.sNG,b.sMoTa,b.iID_MaDonVi,b.sTenDonVi,b.iID_MaPhongBan,b.sTenPhongBan
                                 HAVING SUM(b.rTuChi)<>0 ", DK, donvitinh, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);

            return dtAll;
        }
        #endregion

        #region QuyDQ: rptDuToanBS_NganSachBaoDam ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách dự toán bổ sung 1040100
        /// </summary>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_Dot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="LoaiTongHop">Loại Tổng Hợp</param>
        /// <returns></returns>
        public static DataTable rptDuToanBS_NganSachBaoDam(String MaND, String iID_MaDonVi, String iID_Dot, String iID_MaPhongBan, String LoaiTongHop)
        {
            SqlCommand cmd = new SqlCommand();
            String DKPhongBan = "";
            String DKDonVi = "";
            String DK = "";
            int DVT = 1000;

            DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            if (!String.IsNullOrEmpty(iID_Dot) && iID_Dot != "-1")
            {
                DK += "AND iID_MaDotNganSach=@MaDot";
                cmd.Parameters.AddWithValue("@MaDot", iID_Dot);
            }

            if (iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            if (LoaiTongHop == "ChiTiet")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
                {
                    DKDonVi += " AND iID_MaDonVi=@iID_MaDonVi ";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();

                String[] arrDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DKDonVi += " iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DKDonVi += " OR ";
                }

                if (String.IsNullOrEmpty(DKDonVi) == false)
                    DKDonVi = " AND (" + DKDonVi + ") ";
            }

            String SQL =
                String.Format(@"
                                SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                                        ,rTuChi=SUM(rTuChi)/{3}
                                        ,rTonKho=SUM(rTonKho)/{3}
                                        ,rHangNhap=SUM(rHangNhap)/{3}
                                        ,rHangMua=SUM(rHangMua)/{3}
                                        ,rPhanCap=SUM(rPhanCap)/{3}
                                        ,rDuPhong=SUM(rDuPhong)/{3}
                                FROM DTBS_ChungTuChiTiet where sLNS='1040100' AND iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                                GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                                HAVING SUM(rTuChi)<>0 
                                        OR SUM(rHangNhap)<>0 
                                        OR SUM(rTonKho)<>0  
                                        OR SUM(rHangMua)<>0  
                                        OR SUM(rPhanCap)<>0 
                                        OR SUM(rDuPhong)<>0", DK, DKDonVi, DKPhongBan, DVT);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);

            return dtAll;
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
                                        FROM DTBS_ChungTuChiTiet
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
        #endregion

        #region VungNV: rptDuToanBS_PhanCap ngay 30/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách dự toán bổ sng phân cấp
        /// </summary>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="sLNS">Mã loại ngân sách</param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <returns></returns>
        public static DataTable rptDuToanBS_PhanCap(String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            String DKDonVi = ""; String DKPhongBan = ""; String DK = "";
            SqlCommand cmd = new SqlCommand();

            //Điều kiện đơn vị
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += "iID_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DK += " OR ";
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND ( " + DK + ") ";
            }

            //điều kiện LNS
            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN ( " + sLNS + ") ";
            }

            //điều kiện phòng ban
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String Sql = String.Format(
                        @"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                                SUBSTRING(sLNS,1,3) as sLNS3,
                                SUBSTRING(sLNS,1,5) as sLNS5,
                                sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                ,SUM(rTuChi/@donvitinh) as rTuChi,SUM(rHienVat/@donvitinh) as rHienVat
                        FROM DTBS_ChungTuChiTiet
                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                        HAVING SUM(rTuChi)<>0 OR SUM(rHienVat) <>0
                        ", DK, DKDonVi, DKPhongBan);

            cmd.CommandText = Sql;
            cmd.Parameters.AddWithValue("@donvitinh", "1000");
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);
            return dtAll;
        }
        #endregion
        #endregion

        #region dieu kien
        private static String LayDKPhongBan(String MaND, SqlCommand cmd)
        {
            String DKPhongBan = "";
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            if (sTenPB != "02")
            {
                DKPhongBan = " AND b.iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
            }
            return DKPhongBan;
        }
        private static String LayDKLNS(String MaND, SqlCommand cmd, String iID_MaPhongBan)
        {
            String DKLNS = "";
            DataTable dtLNS = DanhMucModels.NS_LoaiNganSachFull(iID_MaPhongBan);
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                DKLNS += "b.sLNS=@sLNS" + i;
                if (i < dtLNS.Rows.Count - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dtLNS.Rows[i]["sLNS"]);
            }
            if (String.IsNullOrEmpty(DKLNS)) DKLNS = " AND 0=1";
            else
            {
                DKLNS = "AND (" + DKLNS + ")";
            }
            return DKLNS;
        }
        private static String LayDKDonVi(String MaND, SqlCommand cmd)
        {
            String DKDonVi = "";
            DataTable dtNĐonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dtNĐonVi.Rows.Count; i++)
            {
                DKDonVi += "b.iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < dtNĐonVi.Rows.Count - 1)
                    DKDonVi += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtNĐonVi.Rows[i]["iID_MaDonVi"]);
            }
            if (String.IsNullOrEmpty(DKDonVi)) DKDonVi = " AND 0=1";
            else
            {
                DKDonVi = "AND (" + DKDonVi + ")";
            }
            return DKDonVi;
        }
        #endregion

    }
}