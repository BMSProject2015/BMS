using System;
using System.Data.SqlClient;
using System.Data;
using DomainModel;


namespace VIETTEL.Models
{
    public class QuyetToan_ReportModels
    {
        /// <summary>
        /// Lấy danh sách đơn vị có dữ liệu đã duyệt theo tháng của QTTX 1010000
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="iThangQuy">Tháng làm việc</param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String NamLamViec, String iThangQuy, String sLNS = "1010000")
        {
            String SQL = String.Format(@"SELECT DISTINCT NS_DonVi.sTen,QT.iID_MaDonVi
                                         FROM (SELECT iID_MaDonVi,rTuChi FROM QTA_ChungTuChiTiet                                     
                                         WHERE 1=1 AND rTuChi>0 AND bLoaiThang_Quy=0 AND iNamLamViec=@NamLamViec AND sLNS=@sLNS AND iThang_Quy<=@iThang_Quy AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                         )as QT
					                    INNER JOIN NS_DonVi on QT.iID_MaDonVi=NS_DonVi.iID_MaDonVi
					                    ORDER BY iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThangQuy);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                        LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(
                                            PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả";
                dt.Rows.InsertAt(R, 0);
            }
            else
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách đơn vị có dữ liệu đã duyệt theo quý của QTTX 1010000
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="Quy"> Quý</param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi_Quy_TX(String iID_MaTrangThaiDuyet, String Quy, String MaND)
        {
            String dsThang = "";
            if (Quy == "1")
            {
                dsThang = String.Format(@"(iThang_Quy=1 OR iThang_Quy=2 OR iThang_Quy=3)");
            }
            if (Quy == "2")
            {
                dsThang = String.Format(@"(iThang_Quy=4 OR iThang_Quy=5 OR iThang_Quy=6)");
            }
            if (Quy == "3")
            {
                dsThang = String.Format(@"(iThang_Quy=7 OR iThang_Quy=8 OR iThang_Quy=9)");
            }
            if (Quy == "4")
            {
                dsThang = String.Format(@"(iThang_Quy=10 OR iThang_Quy=11 OR iThang_Quy=12)");
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" +
                                       LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(
                                           PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT NS_DonVi.sTen,QT.iID_MaDonVi
                                         FROM (SELECT iID_MaDonVi FROM QTA_ChungTuChiTiet                                     
                                         WHERE {0} AND bLoaiThang_Quy=0 {1} AND iTrangThai=1 {2} AND sLNS=1010000
                                         GROUP BY iID_MaDonVi
                                         )as QT
					                    INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi on QT.iID_MaDonVi=NS_DonVi.iID_MaDonVi
				                        ORDER BY iID_MaDonVi                                        
                                        ", dsThang,
                                       ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            // nếu có đơn vị
            if (dtDonVi.Rows.Count > 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            // nếu không có đơn vị nào
            else
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            dtDonVi.Dispose();
            return dtDonVi;
        }

        /// <summary>
        /// danh sách đơn vị có dữ liệu đã duyệt theo tháng và quý LNS 1010000
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy,
                                              String MaND)
        {
            String DkDuyet = "";
            String DkDuyet_PB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet = "AND iID_MaTrangThaiDuyet='" +
                          LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                DkDuyet_PB = "AND iID_MaTrangThaiDuyet='" +
                             LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String SQL = String.Format(@"SELECT b.sTen as TenDV,a.iID_MaDonVi, b.sTen as sTen 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM QTA_ChungTuChiTiet
	                                           WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {1} {2}
	                                                 AND iThang_Quy<=@dNgay AND bLoaiThang_Quy=0 AND rTuChi>0) a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        UNION
                                         SELECT DISTINCT b.sTen,a.iID_MaDonVi, b.sTen as sTen
                                        FROM (SELECT iID_MaDonVi
	                                          FROM PB_PhanBoChiTiet
	                                          WHERE  iTrangThai=1 {1} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
	                                                AND sLNS=1010000 {3} AND rTuChi>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi      
                                        ORDER BY   iID_MaDonVi  
", "", ReportModels.DieuKien_NganSach(MaND), DkDuyet, DkDuyet_PB);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@dNgay", NgayChiTieu);
            if (LoaiThang_Quy != "1")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtDonVi.Rows.Count > 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["TenDV"] = "Chọn tất cả đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            else
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["TenDV"] = "Không có đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            return dtDonVi;
        }

        /// <summary>
        /// Danh sach don vi theo LNS
        /// </summary>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable dtLNS_DonVi(String iThang_Quy, String iID_MaNamNganSach, String MaND, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            if (String.IsNullOrEmpty(sLNS)) sLNS = "-100";
            String[] arrDonVi = sLNS.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += " sLNS=@LNS" + i;
                cmd.Parameters.AddWithValue("@LNS" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DK += " OR ";
                }
            }
            if (!String.IsNullOrEmpty(DK))
                DK = " AND (" + DK + ")";
            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            SQL = String.Format(@"SELECT DISTINCT  iID_MaDonVi,sTenDonVi as TenHT
FROM QTA_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND sTenDonVi<>'' AND iID_MaDonVi<>''
AND iThang_Quy=@iThang_Quy {0} {1} {2} {3}  AND rTuChi<>0

--UNION
--SELECT DISTINCT  iID_MaDonVi,iID_MaDonVi+'-'+sTenDonVi as TenHT
--FROM PB_PhanBoChiTiet
--WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND sTenDonVi<>''  AND rTuChi<>0 AND iID_MaDonVi<>'' {0} {1} {2}  {3} 
 ", DK, DKLNS, DKPhongBan, DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// danh sach LNS theo don vi
        /// </summary>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable dtDonVi_LNS(String iThang_Quy, String iID_MaNamNganSach, String MaND, String iID_MaDonVi)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);

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

            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            SQL = String.Format(@"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT
FROM( 
SELECT DISTINCT  sLNS
FROM QTA_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0 {0} {1} {2} {3}
AND iThang_Quy<=@iThang_Quy 

UNION
SELECT DISTINCT  sLNS
FROM PB_PhanBoChiTiet
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0  {0} {1} {2} {3}) as a

INNER JOIN 

(SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7) as b
ON a.sLNS=b.sLNS
ORDER BY sLNS

 ", DK, DKLNS, DKPhongBan, DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable dtDonVi_ChuyenNamSau(String iID_MaPhongBan, String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKPhongBan = "", DKDonVi = "";
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            SQL = String.Format(@"
SELECT DISTINCT  iID_MaDonVi,iID_MaDonVi+' - '+sTenDonVi as TenHT
FROM QTA_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 {0} {1} {2}
AND (rDaCapTien<>0 OR rChuaCapTien >0)
 ", DK, DKPhongBan, DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }



        /// <summary>
        /// dtTo chon LNS
        /// </summary>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable dtTo_LNS(String iThang_Quy, String iID_MaNamNganSach, String MaND, String sLNS, String iID_MaPhongBan)
        {
            DataTable dtDonVi = dtLNS_DonVi(iThang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);
            int SoToTrang1 = 4;
            int SoToTrang2 = 7;
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }

            String[] arrDonVi = iID_MaDonVi.Split(',');
            dtDonVi.Dispose();
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
            dtDonVi.Dispose();
            dt.Dispose();
            return dt;

        }

        //VungNV: 2015/09/21: add new param iID_MaPhongBan
        public static DataTable dtLoaiThongTri_LNS(String iThang_Quy, String iID_MaNamNganSach, String MaND,
                                                   String LoaiThongTri, String iID_MaPhongBan)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sLNS", typeof(String));
            dt.Columns.Add("TenHT", typeof(String));

            String sLNS = "";
            if (LoaiThongTri == "ThuongXuyen")
                sLNS = "1010000";
            if (LoaiThongTri == "NghiepVu")
                sLNS = "1020000";

            dt = DanhSachLNSCoDuLieu(iThang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);
            return dt;

        }
        //VungNV: 2015/09/21 add new param iID_MaPhongBan
        public static DataTable DanhSachLNSCoDuLieu(String iThang_Quy, String iID_MaNamNganSach, String MaND, String sLNS, String iID_MaPhongBan)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            String DKPhongBan = "";

            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            if (String.IsNullOrEmpty(sLNS))
                sLNS = "-100";

            String[] arrLNS = sLNS.Split(',');

            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += " sLNS=@LNS" + i;
                cmd.Parameters.AddWithValue("@LNS" + i, arrLNS[i]);

                if (i < arrLNS.Length - 1)
                    DK += " OR ";
            }

            if (!String.IsNullOrEmpty(DK))
                DK = " AND (" + DK + ")";

            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }

            //VungNV: 2015/09/21: Thêm điều kiện phòng ban
            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            String SQL = String.Format(@"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT FROM 
                            (
                            SELECT DISTINCT sLNS FROM QTA_ChungTuChiTiet
                            WHERE iTrangThai=1 {0} {1} AND iThang_Quy=@iThang_Quy ) a

                            INNER JOIN
                            (SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND LEN(sLNS)=7 AND sL='') as b
                            ON a.sLNS=b.sLNS", DK, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("iThang_Quy", iThang_Quy);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

        public static DataTable dtDonVi_QS(String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKPhongBan = "", DKDonVi = "";

            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT FROM (
SELECT DISTINCT iID_MaDonVi  FROM QTQS_ChungTuChiTiet WHERE iTrangThai=1   {0} {1} {2} ) as a
INNER JOIN (SELECT iID_MaDonVi,sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as b
ON a.iID_MaDonVi=b.iID_MaDonVi
 ", DK, DKDonVi, DKPhongBan);
            cmd.CommandText = SQL;
            //cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        internal static DataTable QTQS_DSDonVi(string PhongBan, string TuThang, string DenThang, string MaND)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = string.Empty;
            string DK = string.Empty;
            string DKPhongBan = string.Empty;
            string DKDonVi = string.Empty;

            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd, "A");
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd, "A");
            if (string.IsNullOrEmpty(PhongBan))
            {
                PhongBan = "-1";
            }
            TuThang = string.IsNullOrEmpty(TuThang) ? "0" : TuThang;
            DenThang = string.IsNullOrEmpty(DenThang) ? "0" : DenThang;
            DK = string.Format(@" AND iThang_Quy<= @iDenThang ");
            if (!PhongBan.Equals("-1"))
            {
                DK += string.Format(@" AND A.iID_MaPhongBan=@PhongBan");
            }
            sql = string.Format(@"SELECT DISTINCT  A.iID_MaDonVi,A.iID_MaDonVi+'-'+B.sTen as TenHT
                    FROM QTQS_ChungTuChiTiet A
                    LEFT JOIN NS_DonVi B on A.iID_MaDonVi = b.iID_MaDonVi
                    WHERE (rThieuUy<>0 OR rTrungUy<>0 OR rThuongUy<>0 OR rDaiUy<>0 OR rThieuTa<>0 OR rTrungTa<>0 OR rThuongTa<>0 OR rDaiTa<>0 OR rTuong<>0 OR rQNCN<>0 OR rCNVQP<>0 OR rLDHD<>0) AND sKyHieu IN (2,3,500,600) AND iNamLamViec=@iNamLamViec AND iNamLamViec_DonVi=@iNamLamViec AND A.iTrangThai=1 {0} {1} {2}", DK, DKPhongBan, DKDonVi);

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iTuThang", TuThang);
            cmd.Parameters.AddWithValue("@iDenThang", DenThang);
            cmd.Parameters.AddWithValue("@PhongBan", PhongBan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }

        public static DataTable dtDonVi_LNS_PhongBan(String iThang_Quy, String iID_MaNamNganSach, String MaND, String iID_MaDonVi, String iID_MaPhongBan)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);

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

            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBanND);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan = @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }
            SQL = String.Format(@"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT
                                    FROM( 
                                        SELECT DISTINCT  sLNS
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0 {0} {1} {2} {3}
                                        AND iThang_Quy<=@iThang_Quy 
                                        ) as a

                                        INNER JOIN 

                                        (SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7) as b
                                        ON a.sLNS=b.sLNS
                                        ORDER BY sLNS
                                     ", DK, DKLNS, DKPhongBan, DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtLNS_DonVi(String iThang_Quy, String iID_MaNamNganSach, String MaND, String sLNS, String iID_MaPhongBan)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBanND);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
            }

            String[] arrDonVi = sLNS.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += " sLNS=@LNS" + i;
                cmd.Parameters.AddWithValue("@LNS" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                {
                    DK += " OR ";
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }

            // VungNV: Thêm điều kiện phòng ban
            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            SQL = String.Format(@"SELECT DISTINCT  iID_MaDonVi,sTenDonVi as TenHT
                        FROM QTA_ChungTuChiTiet
                        WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND sTenDonVi<>'' AND iID_MaDonVi<>''
                        AND iThang_Quy<=@iThang_Quy {0} {1} {2} {3}  AND rTuChi<>0 ORDER BY iID_MaDonVi", DK, DKLNS, DKPhongBan, DKDonVi);
            //VungNV: 2015/09/21: fix iThang_Quy<=@iThang_Quy

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtDonVi_LNS(String iThang_Quy, String iID_MaNamNganSach, String MaND, String iID_MaDonVi, String MaPhongBan)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            String DK = "", DKLNS = "", DKPhongBan = "", DKDonVi = "";
            String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);

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

            DKLNS = ThuNopModels.DKLNS(MaND, cmd, iID_MaPhongBan);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            // HungPX: Thêm di?u ki?n phòng ban
            if (MaPhongBan != "-1" && MaPhongBan != null)
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }

            SQL = String.Format(@"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT
FROM( 
SELECT DISTINCT  sLNS
FROM QTA_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0 {0} {1} {2} {3}
AND iThang_Quy<=@iThang_Quy 
) as a

INNER JOIN 

(SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7) as b
ON a.sLNS=b.sLNS
ORDER BY sLNS

 ", DK, DKLNS, DKPhongBan, DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static void GhepChiTieuVaoQuyetToan(DataTable dtQuyetToan, String iID_MaDonVi, String iNamLamViec, String iID_MaNguonNganSach, String iID_MaNamNganSach,
                                                   String iThangQuy, String iID_MaPhongBan, String sLNS, String MaND)
        {
            DataTable dtChiTieu = getChiTieuDonVi(iID_MaDonVi, iNamLamViec, iID_MaNguonNganSach, sLNS, iID_MaNamNganSach, iThangQuy, iID_MaPhongBan, MaND);
            String iID_MaDonViQT = "", iID_MaDonViCT = "", sXauNoiMaQT = "", sXauNoiMaCT = "";
            for (int i = 0; i < dtQuyetToan.Rows.Count; i++)
            {
                iID_MaDonViQT = Convert.ToString(dtQuyetToan.Rows[i]["iID_MaDonVi"]);
                sXauNoiMaQT = Convert.ToString(dtQuyetToan.Rows[i]["sXauNoiMa"]);
                for (int j = 0; j < dtChiTieu.Rows.Count; j++)
                {
                    iID_MaDonViCT = Convert.ToString(dtChiTieu.Rows[j]["iID_MaDonVi"]);
                    sXauNoiMaCT = Convert.ToString(dtChiTieu.Rows[j]["sLNS"]) + "-" + Convert.ToString(dtChiTieu.Rows[j]["sL"]) + "-" +
                           Convert.ToString(dtChiTieu.Rows[j]["sK"]) + "-" + Convert.ToString(dtChiTieu.Rows[j]["sM"]) + "-" +
                           Convert.ToString(dtChiTieu.Rows[j]["sTM"]) + "-" + Convert.ToString(dtChiTieu.Rows[j]["sTTM"]) + "-" + Convert.ToString(dtChiTieu.Rows[j]["sNG"]);

                }
            }

        }

        public static DataTable getChiTieuDonVi(String iID_MaDonVi, String iNamLamViec, String iID_MaNguonNganSach, String iID_MaNamNganSach,
                                                   String iThangQuy, String iID_MaPhongBan, String sLNS, String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            //DK += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan));
            DK += " AND iID_MaDonVi=@iID_MaDonVi";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            //DK += " AND (iThang_Quy<@iThang_Quy OR (iThang_Quy=@iThang_Quy AND iID_MaChungTu IN (SELECT iID_MaChungTu FROM QTA_ChungTu WHERE iTrangThai=1 AND dNgayChungTu<@dNgayChungTu)))";
            //cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            //cmd.Parameters.AddWithValue("@dNgayChungTu", dNgayChungTu);
            //DK += " AND bLoaiThang_Quy=@bLoaiThang_Quy";
            //cmd.Parameters.AddWithValue("@bLoaiThang_Quy", bLoaiThang_Quy);

            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = "";
            for (int i = 0; i < arrDSTruongTien_So.Length; i++)
            {
                if (i > 0) strTruong += ",";
                strTruong += String.Format("SUM({0}) AS {0}", arrDSTruongTien_So[i]);
            }

            cmd.CommandText = String.Format(@"SELECT iID_MaMucLucNganSach,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,{0}
                    FROM DT_ChungTuChiTiet 
                    WHERE {1} AND (MaLoai='' OR MaLoai='2')
                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaMucLucNganSach,sXauNoiMa
                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0
                    UNION
                    SELECT iID_MaMucLucNganSach,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,{0}
                    FROM DT_ChungTuChiTiet_PhanCap 
                    WHERE {1}  AND (sLNS='1020100' OR sLNS='1020000') 
                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaMucLucNganSach,sXauNoiMa
                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0
                    ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG
", strTruong, DK);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}