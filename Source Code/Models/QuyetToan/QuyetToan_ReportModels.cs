using System;
using System.Data.SqlClient;
using System.Data;
using DomainModel;


namespace VIETTEL.Models.QuyetToan
{
    public class QuyetToan_ReportModels
    {
        //VungNV: định nghĩa hardcode của loại quyết toán và ghi chú
        private const string sGHICHU_QUYETTOAN = "rptQuyetToan_ThongTri";
        private const string sLOAI_QUYETTOAN = "rptQuyetToan_ThongTri_LoaiCapPhat";

        #region Source cũ, trước khi update code ngày 18/11/2015
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
        /// Lấy danh sách đơn vị theo Quý, Năm ngân sách, Mã người dùng, Loại ngân sách
        /// </summary>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <returns></returns>
        public static DataTable dtLNS_DonVi(String iThang_Quy, String iID_MaNamNganSach, String MaND, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";
            String DKLNS = "";
            String DKPhongBan = "";
            String DKDonVi = "";
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

            SQL = String.Format(
                @"SELECT DISTINCT iID_MaDonVi,sTenDonVi as TenHT
                FROM QTA_ChungTuChiTiet
                WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND sTenDonVi<>'' AND iID_MaDonVi<>''
                        AND iThang_Quy=@iThang_Quy {0} {1} {2} {3}  AND rTuChi<>0"
                , DK, DKLNS, DKPhongBan, DKDonVi);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách Đơn vị theo Quý, năm ngân sách, mã người dùng, mã đơn vị
        /// </summary>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <returns></returns>
        public static DataTable dtDonVi_LNS(String iThang_Quy, String iID_MaNamNganSach, String MaND, String iID_MaDonVi)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";
            String DKLNS = "";
            String DKPhongBan = "";
            String DKDonVi = "";

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

            SQL = String.Format(
                        @"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT
                        FROM
                        ( 
                            SELECT DISTINCT  sLNS
                            FROM QTA_ChungTuChiTiet
                            WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0 {0} {1} {2} {3}
                            AND iThang_Quy<=@iThang_Quy 

                            UNION
                            SELECT DISTINCT  sLNS
                            FROM PB_PhanBoChiTiet
                            WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1  AND rTuChi<>0  {0} {1} {2} {3}
                        ) as a

                        INNER JOIN 
                        (
                            SELECT sLNS,sMoTa 
                            FROM NS_MucLucNganSach 
                            WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7
                        ) as b

                        ON a.sLNS=b.sLNS
                        ORDER BY sLNS", DK, DKLNS, DKPhongBan, DKDonVi);

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
        /// 
        /// </summary>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaND"></param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaPhongBan"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaND"></param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
        /// VungNV
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
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
            // HungPX: Thêm điều kiện phòng ban
            if (MaPhongBan != "-1" && MaPhongBan != null)
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }

            SQL = String.Format(
                        @"SELECT a.sLNS,a.sLNS+' - '+sMoTa as TenHT
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
                    ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG", strTruong, DK);

            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        #endregion 

        #region Source mới, Update code:18/11/2015 chuyển method từ Controller xuống Models
        #region VungNV: rptQuyetToan_ThongTriController ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ thông tri quyết toán
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="LoaiTongHop">Loại báo cáo: tổng hợp hay chi tiết</param>
        /// <returns></returns>
        /// VungNV: 2015/11/18
        public static DataTable rptQuyetToan_ThongTri(String MaND, String sLNS, String iThang_Quy, String iID_MaNamNganSach, String iID_MaDonVi, String LoaiTongHop)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            //Báo cáo chi tiết từng đơn vị
            if (LoaiTongHop == "ChiTiet")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
                {
                    DK += " AND iID_MaDonVi=@iID_MaDonVi";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            //Báo cáo tổng hợp
            if (LoaiTongHop == "TongHop")
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();
                String[] arrDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DK += "iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DK += " OR ";
                }
                if (!String.IsNullOrEmpty(DK))
                    DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
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

            //Nếu là báo cáo tổng hợp thì lấy thêm mã đơn vị và tên đơn vị
            string strDonVi = "";

            //Báo cáo tổng hợp các đơn vị
            if (LoaiTongHop == "TongHop")
            {
                strDonVi = " ,iID_MaDonVi, sTenDonVi";
            }

            SQL = String.Format(@"
                SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                        SUBSTRING(sLNS,1,3) as sLNS3,
                        SUBSTRING(sLNS,1,5) as sLNS5,
                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa {4}
                        ,SUM(rTuChi) as rTuChi
                    FROM QTA_ChungTuChiTiet
                    WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy {0} {1} {2}
                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                    HAVING SUM(rTuChi)<>0 ", DK, DKDonVi, DKPhongBan, strDonVi);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        /// <summary>
        /// Lấy loại quyết toán
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <returns></returns>
        public static String LayLoaiQuyetToan(String ParentID, String MaND)
        {
            String sLoaiQuyetToan = "";
            String SQL = "";

            SQL = String.Format(
                @"SELECT sGhiChu FROM QTA_GhiChu WHERE sTen=@sTen AND sID_MaNguoiDung=@sID_MaNguoiDung");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@sTen", sLOAI_QUYETTOAN);
            sLoaiQuyetToan = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            return sLoaiQuyetToan;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật loại quyết toán
        /// </summary>
        /// <param name="LoaiCapPhat">Loại quyết toán</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <returns></returns>
        public static void CapNhatLoaiQuyetToan(String LoaiCapPhat, String MaND)
        {
            String SQL = "";
            SQL = String.Format(
                @"IF NOT EXISTS
                        (
	                        SELECT sGhiChu 
	                        FROM QTA_GhiChu 
	                        WHERE sTen = @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung
	                    )
                        INSERT INTO QTA_GhiChu(sTen, sID_MaNguoiDung, sGhiChu) 
		                        VALUES( @sTen, @sID_MaNguoiDung, @sGhiChu)
                    ELSE 
                        UPDATE QTA_GhiChu 
                        SET sGhiChu=@sGhiChu 
                        WHERE sTen = @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTen", sLOAI_QUYETTOAN);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@sGhiChu", LoaiCapPhat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

        }

        /// <summary>
        /// Lấy ghi chú quyết toán của đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        public static String LayGhiChuQuyetToan(String MaND, String iID_MaDonVi)
        {
            String sGhiChu = "";
            String SQL = "";

            SQL = String.Format(
                    @"SELECT sGhiChu 
                    FROM QTA_GhiChu 
                    WHERE sTen=@sTen AND sID_MaNguoiDung=@sID_MaNguoiDung AND iID_MaDonVi=@iID_MaDonVi");

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", sGHICHU_QUYETTOAN);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            sGhiChu = Connection.GetValueString(cmd, "");

            return sGhiChu;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật ghi chú của đơn vị trong quyết toán thông tri
        /// </summary>
        /// <param name="sGhiChu">Nội dung ghi chú</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        public static void CapNhatGhiChuQuyetToan(String sGhiChu, String MaND, String iID_MaDonVi)
        {
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            SQL = String.Format(
                @"IF NOT EXISTS(
		                SELECT sGhiChu 
		                FROM QTA_GhiChu 
		                WHERE sTen = @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung AND iID_MaDonVi = @iID_MaDonVi    
	                )
                    INSERT INTO QTA_GhiChu(sTen, sID_MaNguoiDung, iID_MaDonVi, sGhiChu) 
		            VALUES(@sTen, @sID_MaNguoiDung, @iID_MaDonVi, @sGhiChu)
                ELSE 
	                UPDATE QTA_GhiChu 
	                SET sGhiChu=@sGhiChu 
	                WHERE  sTen = @sTen AND	 sID_MaNguoiDung = @sID_MaNguoiDung AND iID_MaDonVi =@iID_MaDonVi");

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTen", sGHICHU_QUYETTOAN);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            sGhiChu = sGhiChu.Replace("^", "&#10;");
            cmd.Parameters.AddWithValue("@sGhiChu", sGhiChu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

        }
        #endregion

        #region VungNV: rptQuyetToan_LNS_DonVi ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu quyết toán loại ngân sách theo đơn vị
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        /// VungNV
        public static DataTable rptQuyetToan_LNS_DonVi(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String MaPhongBan)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

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

            if (MaPhongBan != "-1")
            {
                DK += "AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            SQL = String.Format(
                @"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                        SUBSTRING(sLNS,1,3) as sLNS3,
                        SUBSTRING(sLNS,1,5) as sLNS5,
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                        ,SUM(rTuChi) as rTuChi
                        ,SUM(rLuyKe) as rLuyKe
                        ,SUM(rQuyetToan) as rQuyetToan
                FROM
                    (  SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                        ,rTuChi=0
                        ,rLuyKe=SUM(CASE WHEN (iThang_Quy<=@iThang_Quy) THEN rTuChi ELSE 0 END)
                        ,rQuyetToan=SUM(CASE WHEN (iThang_Quy=@iThang_Quy) THEN rTuChi ELSE 0 END)
                        FROM QTA_ChungTuChiTiet
                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, rTuChi
                    ) a

                    GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, rTuChi
                    HAVING SUM(rTuChi)<>0  OR SUM(rLuyKe) <>0 OR SUM(rQuyetToan)<>0
                    ", DK, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        #endregion

        #region VungNV: rptQuyetToan_DonVi_LNS ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu loại ngân sách của đơn vị
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_DonVi_LNS(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String MaPhongBan)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
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

            if (MaPhongBan != "-1" && MaPhongBan != null)
            {
                DK += "AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String SQL = String.Format(
                    @"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                        SUBSTRING(sLNS,1,3) as sLNS3,
                        SUBSTRING(sLNS,1,5) as sLNS5,
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                        ,SUM(rTuChi) as rTuChi
                        ,SUM(rLuyKe) as rLuyKe
                        ,SUM(rQuyetToan) as rQuyetToan
                    FROM
                    (
                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                            ,rTuChi=0
                            ,rLuyKe=SUM(CASE WHEN (iThang_Quy<=@iThang_Quy) THEN rTuChi ELSE 0 END)
                            ,rQuyetToan=SUM(CASE WHEN (iThang_Quy=@iThang_Quy) THEN rTuChi ELSE 0 END)
                            FROM QTA_ChungTuChiTiet
                            WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                            GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, rTuChi
                    ) a
                    GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, rTuChi
                    HAVING SUM(rTuChi)<>0 OR SUM(rLuyKe) <>0 OR SUM(rQuyetToan)<>0"
                , DK, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        #endregion

        #region VungNV: rptQuyetToan_TongHop_LNS ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu quyết toán tổng hợp loại ngân sách
        /// </summary>
        /// <param name="MaND">Mã người dung</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        ///  <param name="iThang_Quy">Số tờ</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_TongHop_LNS(String MaND, String sLNS, String iThang_Quy,
                                                    String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dtDonVi = QuyetToan_ReportModels.dtLNS_DonVi(iThang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);
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
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan = @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            dtDonVi.Dispose();

            String DKSUMDonVi = "", DKCASEDonVi = "", DKHAVINGDonVi = "";
            int SoCotTrang1 = 4;
            int SoCotTrang2 = 7;

            if (MaTo == "1")
            {
                if (arrDonVi.Length < SoCotTrang1)
                {
                    int a = SoCotTrang1 - arrDonVi.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDonVi += ",-1";
                    }
                    arrDonVi = iID_MaDonVi.Split(',');
                }

                for (int i = 1; i <= SoCotTrang1; i++)
                {

                    DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                    DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                    DKCASEDonVi += " ,DonVi" + i + "=SUM(CASE WHEN (iID_MaDonVi=@MaDonVi" + i + " AND {1}) THEN {0} ELSE 0 END)";
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i - 1]);
                }

                DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", "iThang_Quy=@iThang_Quy");
            }
            else
            {
                if (arrDonVi.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(MaTo) - 1)))
                {
                    int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(MaTo) - 1) - arrDonVi.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDonVi += ",-1";
                    }
                    arrDonVi = iID_MaDonVi.Split(',');
                }

                int tg = Convert.ToInt16(MaTo) - 2;
                int x = 1;

                for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                {
                    DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                    DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                    DKCASEDonVi += " ,DonVi" + x + "=SUM(CASE WHEN (iID_MaDonVi=@MaDonVi" + x + " AND {1}) THEN {0} ELSE 0 END)";
                    cmd.Parameters.AddWithValue("@MaDonVi" + x, arrDonVi[i]);
                    x++;

                }

                DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", "iThang_Quy=@iThang_Quy");
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
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

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            String SQL = "";

            SQL =
                String.Format(
                        @"SELECT 
                            SUBSTRING(sLNS,1,1) as sLNS1,
                            SUBSTRING(sLNS,1,3) as sLNS3,
                            SUBSTRING(sLNS,1,5) as sLNS5,
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                            SUM(rTuChi) as rTuChi,
                            SUM(CongTrongKy) as CongTrongKy,
                            SUM(DenKyNay) as DenKyNay
                            {4}
                        FROM
                        (
                            SELECT 
                                sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                ,rTuChi=0
                                ,CongTrongKy=SUM(CASE WHEN iThang_Quy=@iThang_Quy THEN rTuChi ELSE 0 END)
                                ,DenKyNay=SUM(CASE WHEN iThang_Quy<=@iThang_Quy THEN rTuChi ELSE 0 END)
                                {3}
                            FROM 
                                QTA_ChungTuChiTiet
                            WHERE 
                                iTrangThai=1 AND 
                                iNamLamViec=@iNamLamViec AND 
                                iThang_Quy<=@iThang_Quy {0} {1} {2}
                            GROUP BY 
                                sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,iThang_Quy
                        ) a
                        GROUP BY  
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                        HAVING 
                            SUM(rTuChi)<>0 OR 
                            SUM(CongTrongKy) <>0 OR 
                            SUM(DenKyNay)<>0 
                            {5} ", DK, DKDonVi, DKPhongBan, DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        #endregion

        #region HungPH: rptQuyetToan_TongHop_Nam_Quy ngày 18/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ quyết toán tổng hợp theo quý, năm
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_TongHop_Nam_Quy(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String LoaiBaoCao, String iID_MaPhongBan)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            //Báo cáo chi tiết từng đơn vị
            if (LoaiBaoCao == "ChiTiet")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
                {
                    DK += " AND iID_MaDonVi=@iID_MaDonVi";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            //Báo cáo tổng hợp
            else
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();
                String[] arrDonVi = iID_MaDonVi.Split(',');

                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DK += "iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DK += " OR ";
                }

                if (String.IsNullOrEmpty(DK) == false)
                    DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
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

            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan = @MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            
            String SQL = "";
            {
                SQL = String.Format(@"
                    SELECT 
                        SUBSTRING(sLNS,1,1) as sLNS1
                        ,SUBSTRING(sLNS,1,3) as sLNS3
                        ,SUBSTRING(sLNS,1,5) as sLNS5
                        ,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                        ,SUM(rTuChi) as rTuChi
                        ,SUM(Quy1) as Quy1
                        ,SUM(Quy2) as Quy2
                        ,SUM(Quy3) as Quy3
                        ,SUM(Quy4) as Quy4
                    FROM
                        (
                            SELECT 
                                sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                                ,rTuChi=0
                                ,Quy1=SUM(CASE WHEN (iThang_Quy=1) THEN rTuChi ELSE 0 END)
                                ,Quy2=SUM(CASE WHEN (iThang_Quy=2) THEN rTuChi ELSE 0 END)
                                ,Quy3=SUM(CASE WHEN (iThang_Quy=3) THEN rTuChi ELSE 0 END)
                                ,Quy4=SUM(CASE WHEN (iThang_Quy=4) THEN rTuChi ELSE 0 END)
                            FROM 
                                QTA_ChungTuChiTiet
                             WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThang_Quy<=@iThang_Quy {0} {1} {2}
                             GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                         ) a

                         GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                         HAVING 
                            SUM(rTuChi)<>0  
                            OR SUM(Quy1) <>0
                            OR SUM(Quy2)<>0
                            OR SUM(Quy3)<>0
                            OR SUM(Quy4)<>0 ", DK, DKDonVi, DKPhongBan);
            }

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dtAll = Connection.GetDataTable(cmd);
            DataView view = new DataView(dtAll);
            DataTable dtChiTiet = view.ToTable(false, "sLNS1", "sLNS3", "sLNS5", "sLNS", "sL",
                                        "sK", "sM", "sTM", "sTTM", "sNG", "sMoTa", "rTuChi", "Quy1", "Quy2", "Quy3", "Quy4");
            cmd.Dispose();
            if (LoaiBaoCao == "chitiet")
                return dtChiTiet;

            return dtAll;
        }
        #endregion

        #region QuyDQ: rptQuyetToan_PhongBan ngày 19/11/2015
        /// <summary>
        /// lấy dữ liệu chứng từ quyết toán phòng ban
        /// </summary>ss
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_PhongBan(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
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

            if (!String.IsNullOrEmpty(iThang_Quy) && iThang_Quy != "-1")
            {
                DK += " AND iThang_Quy=@Thang_Quy";
                cmd.Parameters.AddWithValue("@Thang_Quy", iThang_Quy);
            }

            String SQL =
                String.Format(@"
                                SELECT SUBSTRING(sLNS,1,1) AS sLNS1,
                                        SUBSTRING(sLNS,1,3) AS sLNS3,
                                        SUBSTRING(sLNS,1,5) AS sLNS5,
                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                        ,SUM(rTuChi) as rTuChi
                                FROM QTA_ChungTuChiTiet
                                WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                                GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                HAVING SUM(rTuChi)<>0 ", DK, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);

            return dtAll;
        }
        #endregion

        #region HungPH: rptQuyetToan_TongQuyetToan_LNS_DonVi ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ quyết toán tổng hợp theo lns, đơn vị
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_TongQuyetToan_LNS_DonVi(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();

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

            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
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

            DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String SQL =
                String.Format(@"
                            SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                                   SUBSTRING(sLNS,1,3) as sLNS3,
                                   SUBSTRING(sLNS,1,5) as sLNS5,
                                   sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                   ,SUM(rTuChi) as rTuChi
                                   ,SUM(rLuyKe) as rLuyKe
                                   ,SUM(rQuyetToan) as rQuyetToan
                            FROM
                            (
                                SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                    ,rTuChi
                                    ,rLuyKe=SUM(CASE WHEN (iThang_Quy<=@iThang_Quy) THEN rTuChi ELSE 0 END)
                                    ,rQuyetToan=SUM(CASE WHEN (iThang_Quy=@iThang_Quy) THEN rTuChi ELSE 0 END)
                                FROM QTA_ChungTuChiTiet
                                WHERE iTrangThai=1 
                                    AND iNamLamViec=@iNamLamViec 
                                    AND iThang_Quy<=@iThang_Quy{0} {1} {2}
                                 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,iThang_Quy,rTuChi
                            ) a
                             GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTuChi
                             HAVING SUM(rTuChi)<>0 OR SUM(rLuyKe) <>0 OR SUM(rQuyetToan)<>0", DK, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        #endregion
        
        #region HungPH: rptQuyetToan_TongHop_NhapSoLieu ngày 19/11/2015
        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ quyết toán tổng hợp số liệu nhập
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_TongHop_NhapSoLieu(String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String DKPhongBan = "";
            String DK = "";
            String DKDonVi = "";
            SqlCommand cmd = new SqlCommand();

            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
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

            String SQL = String.Format(@"
                            SELECT A.sTenDonVi as sTenDonVi, A.sLNS as sLNS, SUM(A.rTuChi) as rTuChi,A.sLNS + '-' + B.sMoTa as sMoTa 
                            FROM QTA_ChungTuChiTiet as A 
                            INNER JOIN NS_MucLucNganSach as B ON A.sLNS=B.sLNS
                            WHERE sTenDonVi<>'' AND rTuChi<>'0' AND A.iTrangThai=1  
                                AND LEN(B.sLNS)=7 AND SUBSTRING(B.sLNS,1,1)<>'8' AND B.sL = ''  
                                AND iNamLamViec=@iNamLamViec 
                                AND iThang_Quy=@iThang_Quy {0} {1} {2}
                            GROUP BY sTenDonVi, A.sLNS, B.sMoTa
                            ORDER BY sTenDonVi", DKPhongBan, DKDonVi, DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        #endregion

        #endregion
    }
}