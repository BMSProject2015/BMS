using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models.DungChung;

namespace VIETTEL.Models.DuToanBS
{
    public class DuToanBS_ChungTuModels
    {
        public static Boolean UpdateRecord(String iID_MaChungTu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("DTBS_ChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }
        public static DataTable getDanhSachChungTuNganhKyThuatChuyenBKhac(String sMaND)
        {
            DataTable vR;
            int iID_MaTrangThaiDuyet;
            SqlCommand cmd = new SqlCommand();
            bool bTrolyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(sMaND);
            String iID_MaPhongBan = "";
            String DK = "";

            if (bTrolyTongHop)
            {
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(sMaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                }
                DK += " AND 1=1 AND iID_MaPhongBan<>iID_MaPhongBanDich ";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                dtPhongBan.Dispose();
                DK += " AND 1=1 AND iID_MaPhongBanDich=@iID_MaPhongBan";

            }
            else
            {
                DK += " AND 0=1";
            }
            //nếu là ngân sách bảo đảm

            iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeChiTieu));


            string SQL = String.Format(@"SELECT * FROM (
SELECT DISTINCT iID_MaChungTu FROM DTBS_ChungTuChiTiet_PhanCap
WHERE sLNS=1040100 AND  iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}
 ) as a
INNER JOIN 
(SELECT iID_MaChungTuChiTiet,iID_MaChungTu as MaChungTu
 FROM DTBS_ChungTuChiTiet 
 WHERE iTrangThai=1 AND sLNS='1040100' AND iNamLamViec=@iNamLamViec) as CTCT
 ON CTCT.iID_MaChungTuChiTiet=a.iID_MaChungTu
INNER JOIN (SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND sDSLNS='1040100' AND iNamLamViec=@iNamLamViec AND iKyThuat=0) as b
ON CTCT.MaChungTu=b.iID_MaChungTu", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static void CapNhatLyDoChungTu(String iID_MaChungTu, String sLyDo)
        {
            String SQL = "";
            SqlCommand cmd = new SqlCommand(SQL);

            if (String.IsNullOrEmpty(sLyDo)) sLyDo = "";
            SQL = "UPDATE DTBS_ChungTu SET sLyDo=@sLyDo WHERE iID_MaChungTu=@iID_MaChungTu";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@sLyDo", sLyDo);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        public static DataTable getNguon()
        {
            String SQL = "SELECT iID_MaNguon,sKyHieu+sTen as TenHT FROM DT_Nguon";
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }

        #region Lấy danh sách đơn vị ngành kỹ thuật
        /// <summary>
        /// Lấy danh sách đơn vị ngành kỹ thuật
        /// </summary>
        /// <param name="maND"></param>
        /// <param name="maChungTu"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachDonViKyThuat(string maND, string maChungTu)
        {
            DataTable dtResult;
            SqlCommand cmd = new SqlCommand();
            string dk = "";
            string dkCT = "";
            string dkDV = "";
            string sql = ""; 

            //Trang thai tro ly tong hop duyet lan 1
            int iID_MaTrangThaiDuyet = DuToan_ChungTuChiTietModels.iID_MaTrangThaiDuyetKT;
            string iNamLamViec = ReportModels.LayNamLamViec(maND);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);

            string iID_MaNguonNganSach = "";
            string iID_MaNamNganSach = "";
            string iMaPhongBan = "";
            string sTenPhongBan = "";

            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                iMaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                sTenPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sTen"]);
                dtPhongBan.Dispose();
            }

            if (dtCauHinh!=null && dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            dk = @" iTrangThai=1 AND 
                    iNamLamViec=@iNamLamViec AND 
                    iID_MaNamNganSach=@iID_MaNamNganSach AND 
                    iID_MaNguonNganSach=@iID_MaNguonNganSach AND 
                    iKyThuat=1 AND 
                    MaLoai=1 AND 
                    iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);

            dkCT = "iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)";

            //nếu là ngân sách bảo đảm nganh ky thuat
            if (iMaPhongBan == "06")
            {
                sql = String.Format(@"SELECT DISTINCT iID_MaDonVi 
                                      FROM DTBS_ChungTuChiTiet
                                      WHERE 
                                            iID_MaPhongBanDich='06' AND                                         
                                            {0} AND 
                                            {1}
                                      ORDER BY iID_MaDonVi", dk, dkCT);
            }
            else
            {
                sql = String.Format(@"SELECT DISTINCT iID_MaDonVi 
                                      FROM DTBS_ChungTuChiTiet
                                      WHERE 
                                            {0} AND 
                                            {1} AND
                                            iID_MaDonVi IN (SELECT iID_MaNganh FROM NS_MucLucNganSach_Nganh WHERE sMaNguoiQuanLy LIKE '%{2}%') 
                                      ORDER BY iID_MaDonVi", dk,dkCT,maND);
            }
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            dtResult = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return dtResult;
        } 
        #endregion

        #region Kiểm tra trạng thái chứng từ
        #region Kiểm tra trạng thái chứng từ TLTH sau khi thay đổi
        /// <summary>
        /// Kiểm tra trạng thái chứng từ TLTH sau khi thay đổi
        /// </summary>
        /// <param name="maChungTuTLTH"></param>
        /// <param name="maND"></param>
        /// <returns></returns>
        public static int KiemTraTrangThaiChungTuTLTH(string maChungTuTLTH, string maND)
        {
            SqlCommand cmd = new SqlCommand();
            string dk = "";
            // Lấy danh sách các chứng từ của chứng từ TLTH
            string dsMaChungTu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", maChungTuTLTH, "iID_MaChungTu"));
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                dsMaChungTu = Guid.Empty.ToString();
            }
            string[] arrMaChungTu = dsMaChungTu.Split(',');
            for (int j = 0; j < arrMaChungTu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungTu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTu[j]);

            }
            string SQL = String.Format(@"SELECT A.iID_MaTrangThaiDuyet, iID_MaNhomNguoiDung 
                                         FROM 
                                            (
                                                SELECT DISTINCT iID_MaTrangThaiDuyet
                                                FROM DTBS_ChungTu
                                                WHERE iTrangThai=1 AND ({0})
                                            ) AS A
                                            INNER JOIN 
                                            (
                                                SELECT * FROM NS_PhanHe_TrangThaiDuyet
                                                WHERE iID_MaPhanHe=@iID_MaPhanHe AND iTrangThai=1
                                            ) AS B
                                            ON A.iID_MaTrangThaiDuyet=B.iID_MaTrangThaiDuyet ", dk);
            cmd.CommandText = SQL;
            //Danh sách các trạng thái
            DataTable dtTrangThai = Connection.GetDataTable(cmd);

            bool daTuChoi = false;
            int iTrangThaiDangDuyet = 0;
            int iTrangThaiDaDuyet = 0;
            for (int i = 0; i < dtTrangThai.Rows.Count; i++)
            {
                daTuChoi = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]));
                if (daTuChoi)
                {
                    return Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                }
                else
                {
                    string iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(maND);

                    //Còn trạng thái đang duyệt
                    if (iID_MaNhomNguoiDung == Convert.ToString(dtTrangThai.Rows[i]["iID_MaNhomNguoiDung"]))
                    {
                        iTrangThaiDangDuyet = Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                    }
                    else
                    {
                        iTrangThaiDaDuyet = Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                    }
                }
            }
            if (iTrangThaiDangDuyet > 0)
            {
                return iTrangThaiDangDuyet;
            }
            return iTrangThaiDaDuyet;
        }
        #endregion

        #region Kiểm tra trạng thái chứng từ TLTHCuc sau khi thay đổi
        /// <summary>
        /// Kiểm tra trạng thái chứng từ TLTHCuc sau khi thay đổi
        /// </summary>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <param name="maND"></param>
        /// <returns></returns>
        public static int KiemTraTrangThaiChungTuTLTHCuc(string maChungTuTLTHCuc, string maND)
        {
            SqlCommand cmd = new SqlCommand();
            string dk = "";
            if (String.IsNullOrEmpty(maChungTuTLTHCuc))
            {
                maChungTuTLTHCuc = Guid.Empty.ToString();
            }
            //Lấy danh sách chứng từ TLTH của chứng từ TLTHCuc
            string dsMaChungTuTLTH = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuTLTH[j]);

            }
            string SQL = String.Format(@"SELECT A.iID_MaTrangThaiDuyet, iID_MaNhomNguoiDung 
                                         FROM 
                                            (
                                                SELECT DISTINCT iID_MaTrangThaiDuyet
                                                FROM DTBS_ChungTu_TLTH
                                                WHERE iTrangThai=1 AND ({0})
                                            ) AS A
                                            INNER JOIN 
                                            (
                                                SELECT * FROM NS_PhanHe_TrangThaiDuyet
                                                WHERE iID_MaPhanHe=@iID_MaPhanHe AND iTrangThai=1
                                            ) AS B
                                            ON A.iID_MaTrangThaiDuyet=B.iID_MaTrangThaiDuyet ", dk);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", PhanHeModels.iID_MaPhanHeDuToan);
            DataTable dtTrangThai = Connection.GetDataTable(cmd);
            bool daTuChoi = false;
            int iTrangThaiDangDuyet = 0;
            int iTrangThaiDaDuyet = 0;
            for (int i = 0; i < dtTrangThai.Rows.Count; i++)
            {
                daTuChoi = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]));
                if (daTuChoi)
                {
                    return Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                }
                else
                {
                    String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(maND);
                    //Còn chứng từ đang chờ duyệt
                    if (iID_MaNhomNguoiDung == Convert.ToString(dtTrangThai.Rows[i]["iID_MaNhomNguoiDung"]))
                    {
                        iTrangThaiDangDuyet = Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                    }
                    else
                    {
                        iTrangThaiDaDuyet = Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                    }
                }
            }
            if (iTrangThaiDangDuyet > 0)
            {
                return iTrangThaiDangDuyet;
            }
            return iTrangThaiDaDuyet;
        }
        #endregion 
        #endregion
       
        #region Lấy thông tin chứng từ
        /// <summary>
        /// Lấy thông tin chứng từ
        /// </summary>
        /// <param name="maChungTu">Mã chứng từ</param>
        /// <returns></returns>
        public static NameValueCollection LayThongTinChungTu(string maChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = LayChungTu(maChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        /// <summary>
        /// Lấy thông tin chứng từ TLTH
        /// </summary>
        /// <param name="maChungTuTLTH">Mã chứng từ TLTH Cục</param>
        /// <returns></returns>
        public static NameValueCollection LayThongTinChungTuTLTH(string maChungTuTLTH)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = LayChungTuTLTH(maChungTuTLTH);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }

            dt.Dispose();
            return Data;
        }

        /// <summary>
        /// Lấy thông tin chứng từ TLTH Cục
        /// </summary>
        /// <param name="maChungTuTLTHCuc">Mã chứng từ TLTH Cục</param>
        /// <returns></returns>
        public static NameValueCollection LayThongTinChungTuTLTHCuc(string maChungTuTLTHCuc)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = LayChungTuTLTHCuc(maChungTuTLTHCuc);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }

            dt.Dispose();
            return Data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTinChungTuKyThuatLan2(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = LayChungTuKyThuatLan2(iID_MaChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }

            dt.Dispose();
            return Data;
        }
        #endregion 

        #region Lấy dữ liệu chứng từ theo mã
        /// <summary>
        /// Lấy chứng từ
        /// </summary>
        /// <param name="maChungTu">Mã chứng từ</param>
        /// <returns>Row dữ liệu DB</returns>
        public static DataTable LayChungTu(String maChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy chứng từ TLTH
        /// </summary>
        /// <param name="maChungTuTLTH">Mã chứng từ TLTH</param>
        /// <returns>Row dữ liệu DB</returns>
        public static DataTable LayChungTuTLTH(String maChungTuTLTH)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu_TLTH WHERE iTrangThai=1 AND iID_MaChungTu_TLTH=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTuTLTH);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy chứng từ TLTH Cục
        /// </summary>
        /// <param name="maChungTuTLTHCuc">Mã chứng từ TLTH Cục</param>
        /// <returns>Row dữ liệu DB</returns>
        public static DataTable LayChungTuTLTHCuc(String maChungTuTLTHCuc)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu_TLTHCUc WHERE iTrangThai=1 AND iID_MaChungTu_TLTHCUc=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTuTLTHCuc);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable LayChungTuKyThuatLan2(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTuChiTiet=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        #endregion

        #region Lấy danh sách chứng từ 
        #region Lấy danh sách chứng từ theo điều kiện
        /// <summary>
        /// Lấy danh sách chứng từ theo điều kiện
        /// </summary>
        /// <param name="maChungTuTLTH">Mã chứng từ TLTH</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="ngayDotNganSach">Ngày đợt ngân sách</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="soChungTu">Số chứng từ</param>
        /// <param name="tuNgay">Ngày bắt đầu</param>
        /// <param name="denNgay">Ngày kết thúc</param>
        /// <param name="sLNS_TK">Mã loại ngân sách </param>
        /// <param name="maTrangThaiDuyet">Mã trạng thái duyệt</param>
        /// <param name="LayTheoMaNDTao">Lấy theo mã người dùng hay không</param>
        /// <param name="iKyThuat">Ngành kỹ thuật</param>
        /// <param name="trang">Trang</param>
        /// <param name="soBanGhi">Số bản ghi</param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTu(string maChungTuTLTH, string sLNS, string ngayDotNganSach, string maND, string soChungTu,
            string tuNgay, string denNgay, string sLNS_TK, string maTrangThaiDuyet, bool LayTheoMaNDTao = false, string iKyThuat = "0", int trang = 0, int soBanGhi = 0)
        {
            DataTable result;
            string dk = "";
            SqlCommand cmd = new SqlCommand();

            dk = "iTrangThai = 1 AND iiD_MaDonVi is null";
            dk += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(maND));

            //Xem danh sách chứng từ của 1 chứng từ TLTH
            if (maChungTuTLTH != Convert.ToString(Guid.Empty) && !String.IsNullOrEmpty(maChungTuTLTH))
            {
                string dsMaChungTu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", maChungTuTLTH, "iID_MaChungTu"));
                string[] arrMaChungtu = dsMaChungTu.Split(',');
                dk += " AND(";
                for (int j = 0; j < arrMaChungtu.Length; j++)
                {
                    dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                    if (j < arrMaChungtu.Length - 1)
                        dk += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungtu[j]);

                }
                dk += " )";
            }
            //Xem danh sách tất cả chứng từ
            else
            {
                //Ngân sách bảo đảm ngành kỹ thuật
                if (sLNS == "1040100" && iKyThuat == "1")
                {
                    dk += " AND ";
                    dk += LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeChiTieu, maND);
                }
                //Ngân sách bảo đảm ngành khác
                else if (sLNS == "1040100,109")
                {
                    dk += " AND ";
                    dk += LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, maND);
                }
                //Các loại ngân sách khác
                else
                {
                    dk += " AND ";
                    dk += LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, maND);
                    dk += " AND (sDSLNS NOT LIKE '104%'  AND sDSLNS NOT LIKE '109%'  )  ";
                }

                //Điều kiện phòng ban
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
                string maPhongBan = "";
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    maPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                    dtPhongBan.Dispose();
                }
                if (maPhongBan != Convert.ToString(Guid.Empty) && !String.IsNullOrEmpty(maPhongBan))
                {
                    dk += " AND iID_MaPhongBan = @iID_MaPhongBan";
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", maPhongBan);
                }

                // Điều kiện ngành kỹ thuật
                if (!String.IsNullOrEmpty(iKyThuat) && iKyThuat != "0")
                {
                    dk += " AND iKyThuat = @iKyThuat";
                    cmd.Parameters.AddWithValue("@iKyThuat", iKyThuat);
                }
                else
                {
                    dk += " AND iKyThuat = @iKyThuat";
                    cmd.Parameters.AddWithValue("@iKyThuat", "0");
                }
                //kiem tra tro ly tong hop
                Boolean laTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(maND);
                if (laTroLyTongHop == false)
                    LayTheoMaNDTao = true;

                //Điều kiện loại ngân sách
            }

            if (!String.IsNullOrEmpty(sLNS) && sLNS != "-1")
            {
                String[] arrLNS = sLNS.Split(',');
                dk += " AND ( ";
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    dk += "  sDSLNS LIKE @sLNS" + i;
                    if (i < arrLNS.Length - 1)
                        dk += " OR ";
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i] + "%");
                }
                dk += " ) ";
            }
            //Điều kiện loại ngân sách tìm kiế
            if (!String.IsNullOrEmpty(sLNS_TK))
            {
                dk += String.Format(" AND sDSLNS LIKE '{0}%'", sLNS_TK);
            }

            // Điều kiện ngày đợt ngân sách
            if (String.IsNullOrEmpty(ngayDotNganSach) == false && ngayDotNganSach != "")
            {
                dk += " AND CONVERT(nvarchar, dNgayDotNganSach, 103) = @dNgayDotNganSach";
                cmd.Parameters.AddWithValue("@dNgayDotNganSach", ngayDotNganSach);
            }

            //Lấy theo người dùng tạo
            if (LayTheoMaNDTao && !BaoMat.KiemTraNguoiDungQuanTri(maND))
            {
                dk += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", maND);
            }

            //Điều kiện số chứng từ
            if (CommonFunction.IsNumeric(soChungTu))
            {
                dk += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", soChungTu);
            }

            //Điều kiện mã trạng thái
            if (!String.IsNullOrEmpty(maTrangThaiDuyet) && maTrangThaiDuyet != "-1")
            {
                dk += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiDuyet);
            }

            //Điều kiện ngày bắt đầu
            if (!String.IsNullOrEmpty(tuNgay))
            {
                dk += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(tuNgay));
            }

            // Điều kiện ngày kết thúc
            if (!String.IsNullOrEmpty(denNgay))
            {
                dk += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(denNgay));
            }

            String SQL = String.Format(@"SELECT * FROM DTBS_ChungTu WHERE {0} ", dk);
            cmd.CommandText = SQL;

            //Trường hợp lấy tất cả
            if (trang == 0 && soBanGhi == 0)
            {
                result = Connection.GetDataTable(cmd);
            }
            else
            //Chỉ lấy theo trang
            {
                result = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,sDSLNS, iSoChungTu,dNgayChungTu", trang, soBanGhi);
            }
            cmd.Dispose();
            return result;
        }
        #endregion

        #region Lấy danh sách chứng từ TLTH theo điều kiện
        /// <summary>
        /// Lấy danh sách chứng từ TLTH theo điều kiện
        /// </summary>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <param name="sLNS"></param>
        /// <param name="maND"></param>
        /// <param name="layTheoMaNDTao"></param>
        /// <param name="tuNgay"></param>
        /// <param name="denNgay"></param>
        /// <param name="maTrangThaiDuyet"></param>
        /// <param name="trang"></param>
        /// <param name="soBanGhi"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuTLTH(string maChungTuTLTHCuc, string maND, bool layTheoMaNDTao, string tuNgay, string denNgay, string maTrangThaiDuyet, int trang = 0, int soBanGhi = 0)
        {
            DataTable result;
            SqlCommand cmd = new SqlCommand();
            string dk = "";
            dk = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, maND);
            dk += " AND iTrangThai = 1 ";
            dk += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(maND));

            //Xem danh sách chứng từ TLTH của chứng từ TLTH cục
            if (maChungTuTLTHCuc != Convert.ToString(Guid.Empty) && !String.IsNullOrEmpty(maChungTuTLTHCuc))
            {
                String dsMaChungTuTLTH = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
                String[] arrMaChungtuTLTH = dsMaChungTuTLTH.Split(',');
                dk += " AND(";
                for (int j = 0; j < arrMaChungtuTLTH.Length; j++)
                {
                    dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                    if (j < arrMaChungtuTLTH.Length - 1)
                        dk += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungtuTLTH[j]);

                }
                dk += " )";
            }
            //Lấy điều kiện phòng ban
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            string maPhongBan = "";
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                maPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                dtPhongBan.Dispose();
            }
            if (maPhongBan != Convert.ToString(Guid.Empty) && !String.IsNullOrEmpty(maPhongBan))
            {
                dk += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", maPhongBan);
            }

            //Lấy theo mã người dùng tạo
            if (layTheoMaNDTao && !BaoMat.KiemTraNguoiDungQuanTri(maND))
            {
                dk += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", maND);
            }

            //Điều kiện ngày bắt đầu
            if (!String.IsNullOrEmpty(tuNgay))
            {
                dk += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(tuNgay));
            }

            //Điều kiện ngày kết thúc
            if (!String.IsNullOrEmpty(denNgay))
            {
                dk += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(denNgay));
            }

            //Điều kiện trạng thái duyệt
            if (!String.IsNullOrEmpty(maTrangThaiDuyet))
            {
                dk += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiDuyet);
            }
            String SQL = String.Format("SELECT * FROM DTBS_ChungTu_TLTH WHERE {0}", dk);
            cmd.CommandText = SQL;

            //Lấy tất cả
            if (trang == 0 && soBanGhi == 0)
            {
                result = Connection.GetDataTable(cmd);
            }
            //Lấy theo trang
            else
            {
                result = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,dNgayChungTu", trang, soBanGhi);
            }
            cmd.Dispose();
            return result;
        }
        #endregion

        #region Lấy danh sách chứng từ TLTH cục theo điều kiện
        /// <summary>
        /// Lấy danh sách chứng từ TLTH Cục theo điều kiện
        /// </summary>
        /// <param name="maND"></param>
        /// <param name="tuNgay"></param>
        /// <param name="denNgay"></param>
        /// <param name="maTrangThaiDuyet"></param>
        /// <param name="bLayTheoMaNDTao"></param>
        /// <param name="trang"></param>
        /// <param name="soBanGhi"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuTLTHCuc(string maND, string tuNgay, string denNgay, string maTrangThaiDuyet, bool bLayTheoMaNDTao = false, int trang = 0, int soBanGhi = 0)
        {
            DataTable result;
            SqlCommand cmd = new SqlCommand();

            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, maND);
            DK += " AND iTrangThai = 1 ";
            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(maND));

            DataTable dtPhongban = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            string maPhongban = "";
            if (dtPhongban != null && dtPhongban.Rows.Count > 0)
            {
                maPhongban = Convert.ToString(dtPhongban.Rows[0]["sKyHieu"]);
                dtPhongban.Dispose();
            }
            if (maPhongban != Convert.ToString(Guid.Empty) && !String.IsNullOrEmpty(maPhongban))
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", maPhongban);
            }

            if (bLayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(maND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", maND);
            }
            if (!String.IsNullOrEmpty(tuNgay))
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(tuNgay));
            }
            if (!String.IsNullOrEmpty(denNgay))
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(denNgay));
            }
            if (!String.IsNullOrEmpty(maTrangThaiDuyet))
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiDuyet);
            }

            String SQL = String.Format("SELECT * FROM DTBS_ChungTu_TLTHCuc WHERE {0}", DK);
            cmd.CommandText = SQL;
            if (trang == 0 && soBanGhi == 0)
            {
                result = Connection.GetDataTable(cmd);
            }
            else
            {
                result = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,dNgayChungTu", trang, soBanGhi);
            }
            cmd.Dispose();
            return result;
        }
        #endregion 

        #region Lấy danh sách chứng từ để gom TLTH
        /// <summary>
        /// Lấy danh sách chứng từ để gom
        /// </summary>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungDeGomTLTH(string maND)
        {
            DataTable result;
            SqlCommand cmd = new SqlCommand();

            string iNamLamViec = ReportModels.LayNamLamViec(maND);
            string maPhongBan = "";
            string dk = "";

            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                maPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                dtPhongBan.Dispose();
            }
            if (!String.IsNullOrEmpty(maPhongBan))
            {
                dk += "iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", maPhongBan);
            }
            else
            {
                dk += "1=1";
            }
            // Lay ma trang thai chung tu donvi trinh duyet
            int maTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan);
            maTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(maTrangThaiDuyet);

            string SQL = String.Format(@"SELECT * 
                                FROM 
                                    DTBS_ChungTu 
                                WHERE 
                                    iTrangThai=1 AND 
                                    iNamLamViec=@iNamLamViec AND 
                                    iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND 
                                    iCheck=0 AND {0}", dk);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiDuyet);
            result = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return result;
        } 
        #endregion

        #region Lấy danh sách chứng từ dể sửa chứng từ TLTH
        /// <summary>
        /// Lấy danh sách chứng từ dể sửa chứng từ TLTH
        /// </summary>
        /// <param name="maND"></param>
        /// <param name="sLNS"></param>
        /// <param name="maChungTuTLTH"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuDeSuaTLTH(string maND, string maChungTuTLTH)
        {
            //Danh sách chứng từ chưa gom
            DataTable dtChungTuChuaGom = LayDanhSachChungDeGomTLTH(maND);
            DataTable result;
            SqlCommand cmd = new SqlCommand();

            //Lấy dan sách chúng từ của chứng từ TLTH
            string dk = "";
            string dsMaChungTu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", maChungTuTLTH, "iID_MaChungTu"));
            string[] arrMaChungTu = dsMaChungTu.Split(',');
            dk += " (";
            for (int j = 0; j < arrMaChungTu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungTu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTu[j]);

            }
            dk += " )";
            string SQL = string.Format(@"SELECT * FROM DTBS_ChungTu 
                                        WHERE 
                                            iTrangThai=1 AND 
                                            iNamLamViec=@iNamLamViec AND {0}", dk);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(maND));
            result = Connection.GetDataTable(cmd);
            cmd.Dispose();

            //Gộp kết quả
            result.Merge(dtChungTuChuaGom);
            return result;
        } 
        #endregion

        #region Lấy danh sách chứng từ TLTH để gom TLTHCuc
        /// <summary>
        /// Lấy danh sách chứng từ TLTH để gom
        /// </summary>
        /// <param name="maND"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuDeGomTLTHCuc(string maND)
        {
            DataTable result;
            SqlCommand cmd = new SqlCommand();

            //Mới tạo
            int iMaTrangThaiTLTH = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan);
            //Trợ lý đơn vị trình duyệt
            iMaTrangThaiTLTH = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iMaTrangThaiTLTH);
            //Trọ lý tổng hợp trình duyệt
            iMaTrangThaiTLTH = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iMaTrangThaiTLTH);

            string SQL = @"SELECT * 
                           FROM DTBS_ChungTu_TLTH 
                           WHERE 
                                iTrangThai=1 AND 
                                iNamLamViec=@iNamLamViec AND 
                                iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND 
                                iCheck=0";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(maND));
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iMaTrangThaiTLTH);
            result = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return result;
        } 
        #endregion

        #region Lấy danh sách chứng từ TLTH để sửa chứng từ TLTHCuc
        /// <summary>
        /// Lấy danh sách chứng từ TLTH để sửa chứng từ TLTHCuc
        /// </summary>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="maTrangThaiDuyet"></param>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuDeSuaTLTHCuc(string maND, string maChungTuTLTHCuc)
        {
            //Lấy danh sách chứng từ TLTH chưa gom
            DataTable dtChungTuChuaGom = LayDanhSachChungTuDeGomTLTHCuc(maND);

            DataTable result;
            SqlCommand cmd = new SqlCommand();

            //Lấy danh sách chứng từ TLTH của chứng từ TLTH Cục
            string dk = "";
            String dsMaChungTuTLTH = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            String[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            dk += " (";
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuTLTH[j]);

            }
            dk += " )";

            string SQL = String.Format(@" SELECT * 
                            FROM 
                                DTBS_ChungTu_TLTH 
                            WHERE 
                                iTrangThai=1 AND 
                                iNamLamViec=@iNamLamViec AND {0}", dk);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(maND));
            result = Connection.GetDataTable(cmd);
            cmd.Dispose();

            //Gộp kết quả
            result.Merge(dtChungTuChuaGom);
            return result;
        } 
        #endregion

        #region Lấy tờ bìa danh sách chứng từ kỹ thuật
        /// <summary>
        /// Lấy tờ bìa danh sách chứng từ kỹ thuật
        /// </summary>
        /// <param name="maND"></param>
        /// <returns></returns>
        public static DataTable LayDSChungTuKyThuatBia(string maND)
        {
            DataTable dtResult;
            SqlCommand cmd = new SqlCommand();
            string dk = "";
            //Trang thai tro ly tong hop duyet lan 1
            int iID_MaTrangThaiDuyet = DuToanBS_ChungTuChiTietModels.iID_MaTrangThaiDuyetKT;
            string iNamLamViec = ReportModels.LayNamLamViec(maND);
            string iID_MaNguonNganSach = "";
            string iID_MaNamNganSach = "";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);

            if (dtCauHinh != null && dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            dk = " iTrangThai=1";
            dk += @" AND iNamLamViec=@iNamLamViec AND iID_MaNamNganSach=@iID_MaNamNganSach AND 
                    iID_MaNguonNganSach = @iID_MaNguonNganSach AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);

            string SQL = String.Format(@"SELECT * FROM DTBS_ChungTu
                                         WHERE {0}   
                                         ORDER BY dNgayChungTu DESC", dk);
            cmd.CommandText = SQL;
            dtResult = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return dtResult;
        } 
        #endregion

        #region Lấy danh sách chứng từ kỹ thuật
        /// <summary>
        /// Lấy danh sách chứng từ kỹ thuật
        /// </summary>
        /// <param name="maND"></param>
        /// <param name="maChungTu"></param>
        /// <param name="maDonVi"></param>
        /// <param name="sM"></param>
        /// <param name="sTM"></param>
        /// <param name="sTTM"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuKyThuat(string maND, string maChungTu, string maDonVi, string sM, string sTM, string sTTM, string sNG)
        {
            DataTable dtResult;
            string dk = "";
            //Trang thai tro ly tong hop duyet lan 1
            int iID_MaTrangThaiDuyet = DuToan_ChungTuChiTietModels.iID_MaTrangThaiDuyetKT;
            string iNamLamViec = ReportModels.LayNamLamViec(maND);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);

            String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "", SQL = ""; ;

            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }

            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            dk = String.Format(@" iTrangThai=1 AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2}) "
                               , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(maDonVi))
            {
                dk += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", maDonVi);
            }
            if (!String.IsNullOrEmpty(sM))
            {
                dk += " AND sM=@sM";
                cmd.Parameters.AddWithValue("@sM", sM);
            }
            if (!String.IsNullOrEmpty(sTM))
            {
                dk += " AND sTM=@sTM";
                cmd.Parameters.AddWithValue("@sTM", sTM);
            }
            if (!String.IsNullOrEmpty(sTTM))
            {
                dk += " AND sTTM=@sTTM";
                cmd.Parameters.AddWithValue("@sTTM", sTTM);
            }
            if (!String.IsNullOrEmpty(sNG))
            {
                dk += " AND sNG=@sNG";
                cmd.Parameters.AddWithValue("@sNG", sNG);
            }

            //nếu là ngân sách bảo đảm nganh ky thuat
            if (iID_MaPhongBan == "06")
            {

                SQL = String.Format(@"SELECT * FROM DTBS_ChungTuChiTiet
WHERE {1} AND iKyThuat=1 AND MaLoai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaPhongBanDich='06' AND iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)
ORDER BY iID_MaDonVi,sM,sTM,sTTM,sNG", maND, dk);
            }
            else
            {
                SQL = String.Format(@"SELECT * FROM DTBS_ChungTuChiTiet
WHERE {1} AND iKyThuat=1 AND MaLoai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaDonVi IN (SELECT iID_MaNganh FROM NS_MucLucNganSach_Nganh
WHERE sMaNguoiQuanLy LIKE '%{0}%') AND iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)
ORDER BY iID_MaDonVi,sM,sTM,sTTM,sNG", maND, dk);
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            dtResult = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return dtResult;
        } 
        #endregion
        #endregion

        #region Thêm Chứng Từ Dự Toán Bổ Sung
        #region Thêm Chứng Từ
        /// <summary>
        /// Thêm chứng từ 
        /// </summary>
        /// <param name="bang">bảng dữ liệu</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <returns>Mã chứng từ mới</returns>
        public static string ThemChungTu(Bang bang, string maND, string sLNS, string iKyThuat)
        {
            String iNamLamViec = ReportModels.LayNamLamViec(maND);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);

            //Kiểm tra dữ liệu
            if (String.IsNullOrEmpty(sLNS))
            {
                throw new Exception("err_sLNS|Bạn chưa chọn loại ngân sách. Hãy chọn loại ngân sách!");
            }

            if (bang.CmdParams.Parameters["@dNgayChungTu"].Value == null)
            {
                throw new Exception("err_dNgayChungTu|Ngày chứng từ không hợp lệ. Hãy nhập lại ngày chứng từ!");
            }

            string iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
            int maTrangThai;
            if (iKyThuat == "1")
            {
                maTrangThai = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeChiTieu);
            }
            else
            { 
                maTrangThai =LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(DuToanModels.iID_MaPhanHe);
            }
            //Gán giá trị trường
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",maTrangThai);

            return Convert.ToString(bang.Save());
        }
        #endregion

        #region Thêm Chứng Từ TLTH
        /// <summary>
        /// Thêm Chứng Từ TLTH
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maND">Mã Người dùng</param>
        /// <param name="dsMaChungTu">Danh sách mã chứng từ</param>
        /// <returns></returns>
        public static string ThemChungTuTLTH(Bang bang, string maND, string dsMaChungTu)
        {

            //Kiểm tra dữ liệu
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                throw new Exception("err_ChungTu|bạn chưa chọn đợt chứng từ nào. Hãy chọn đợt chứng từ!");
            }

            if (bang.CmdParams.Parameters["@dNgayChungTu"].Value == null)
            {
                throw new Exception("err_dNgayChungTu|Ngày chứng từ không hợp lệ. Hãy nhập lại ngày chứng từ!");
            }

            string iNamLamViec = ReportModels.LayNamLamViec(maND);
            string iID_MaNguonNganSach = "";
            string iID_MaNamNganSach = "";
            string iID_MaPhongBan = "";
            string sTenPhongBan = "";
            int iID_MaTrangThaiDuyet;
            //int iSoChungTu = 0;

            //Lấy cấu hình.
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);
            if (dtCauHinh != null && dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }
            //iSoChungTu = DuToanBS_ChungTuModels.iSoChungTu(iNamLamViec) + 1;
            //Lấy mã phòng ban.
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                iID_MaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                sTenPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sTen"]);
                dtPhongBan.Dispose();
            }

            #region Update trạng thái check chứng từ

            string dk = "";
            string[] arrMaChungtu = dsMaChungTu.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungtu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungtu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungtu[j]);
            }

            string SQL = @"UPDATE DTBS_ChungTu SET iCheck=1 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            #endregion

            iID_MaTrangThaiDuyet = Convert.ToInt32(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", arrMaChungtu[0], "iID_MaTrangThaiDuyet"));
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", dsMaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
            //bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
            return Convert.ToString(bang.Save());
        }
        #endregion 

        #region Thêm Chứng Từ TLTH Cục
        /// <summary>
        /// Thêm Chứng Từ TLTH Cục
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="dsMaChungTuTLTH">Danh sách mã chứng từ TLTH</param>
        /// <returns>Mã chứng từ TLTHCuc mới tạo</returns>
        public static string ThemChungTuTLTHCuc(Bang bang, string maND, string dsMaChungTuTLTH)
        {
            if (String.IsNullOrEmpty(dsMaChungTuTLTH))
            {
                throw new Exception("err_ChungTu|bạn chưa chọn đợt chứng từ nào. Hãy chọn đợt chứng từ!");
            }

            if (bang.CmdParams.Parameters["@dNgayChungTu"].Value == null)
            {
                throw new Exception("err_dNgayChungTu|Ngày chứng từ không hợp lệ. Hãy nhập lại ngày chứng từ!");
            }


            //int iSoChungTu = 0;
            string iNamLamViec = ReportModels.LayNamLamViec(maND);
            string iID_MaNguonNganSach = "";
            string iID_MaNamNganSach = "";
            string iID_MaPhongBan = "";
            string sTenPhongBan = "";

            //iSoChungTu = DuToanBS_ChungTuModels.iSoChungTu(iNamLamViec)+1;

            //Lấy cấu hinh
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);
            if (dtCauHinh!=null && dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            //Lấy phòng ban
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                iID_MaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                sTenPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sTen"]);
                dtPhongBan.Dispose();
            }
            #region Update trạng thái check chứng từ TLTH
            string dk = "";
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTuTLTH[j]);
            }
            String SQL = @"UPDATE DTBS_ChungTu_TLTH SET iCheck=1 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose(); 
            #endregion

            //Lấy mã trạng thái duyệt
            int iID_MaTrangThaiDuyet = Convert.ToInt32(
                CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", arrMaChungTuTLTH[0], "iID_MaTrangThaiDuyet"));
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_TLTH", dsMaChungTuTLTH);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
            //bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
            return Convert.ToString(bang.Save());
        } 
        #endregion
        #endregion

        #region Sửa Chứng Từ Dự Toán Bổ Sung
        #region Sửa Chứng Từ
        /// <summary>
        /// Sửa chứng từ
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="MaChungTu">Mã chứng từ</param>
        /// <returns>kết quả</returns>
        public static int SuaChungTu(Bang bang, string MaChungTu)
        {
            //Kiểm tra dữ liệu
            if (bang.CmdParams.Parameters["@dNgayChungTu"].Value == null)
            {
                throw new Exception("err_dNgayChungTu|Ngày chứng từ không hợp lệ. Hãy nhập lại ngày chứng từ!");
            }
            bang.GiaTriKhoa = MaChungTu;
            bang.DuLieuMoi = false;
            bang.Save();
            return 1;
        }
        #endregion

        #region Sửa Chứng Từ TLTH
        /// <summary>
        /// Sửa Chứng Từ TLTH
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maChungTuTLTH">Mã chứng từ TLTH</param>
        /// <param name="dsMaChungTu">Danh sách mã chứng từ</param>
        /// <returns></returns>
        public static int SuaChungTuTLTH(Bang bang, string maChungTuTLTH, string dsMaChungTu)
        {
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                throw new Exception("err_ChungTu|bạn chưa chọn đợt chứng từ nào. Hãy chọn đợt chứng từ!");
            }

            if (bang.CmdParams.Parameters["@dNgayChungTu"].Value == null)
            {
                throw new Exception("err_dNgayChungTu|Ngày chứng từ không hợp lệ. Hãy nhập lại ngày chứng từ!");
            }

            string SQL = "";
            string dk = "";
            SqlCommand cmd = new SqlCommand();

            //update lai trang thai icheck=0 tat ca các đợt chứng từ của TLTH
            String dsMaChungTuCu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", maChungTuTLTH, "iID_MaChungTu"));
            String[] arrMaChungTuCu = dsMaChungTuCu.Split(',');
            dk += "AND (";
            for (int j = 0; j < arrMaChungTuCu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungTuCu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTuCu[j]);

            }
            dk += " )";
            SQL = String.Format("UPDATE DTBS_ChungTu SET iCheck=0 WHERE iTrangThai=1 {0}", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);

            //update lai trang thai icheck=1 cac chung tu duoc chon
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                dsMaChungTu = Guid.Empty.ToString();
            }
            string[] arrMaChungTu = dsMaChungTu.Split(',');
            cmd = new SqlCommand();
            dk = "";
            for (int j = 0; j < arrMaChungTu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungTu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTu[j]);

            }
            SQL = String.Format("UPDATE DTBS_ChungTu SET iCheck=1 WHERE iTrangThai=1 AND({0})", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Cập nhật bản DTBS_ChungTu_TLTH
            bang.GiaTriKhoa = maChungTuTLTH;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", dsMaChungTu);
            bang.DuLieuMoi = false;
            bang.Save();
            return 1;
        }
        #endregion 

        #region Sửa Shứng Từ TLTH Cục
        /// <summary>
        /// Sửa chứng từ TLTH Cục
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maChungTuTLTHCuc">Mã chứng từ TLTH Cục</param>
        /// <param name="dsMaChungTuTLTH">Danh sách chứng từ TLTH</param>
        /// <returns></returns>
        public static int SuaChungTuTLTHCuc(Bang bang, string maChungTuTLTHCuc, string dsMaChungTuTLTH)
        {
            //Kiểm tra dữ liệu
            if (String.IsNullOrEmpty(dsMaChungTuTLTH))
            {
                throw new Exception("err_ChungTu|bạn chưa chọn đợt chứng từ nào. Hãy chọn đợt chứng từ!");
            }

            if (bang.CmdParams.Parameters["@dNgayChungTu"].Value == null)
            {
                throw new Exception("err_dNgayChungTu|Ngày chứng từ không hợp lệ. Hãy nhập lại ngày chứng từ!");
            }

            string SQL = "";
            string dk = "";
            SqlCommand cmd = new SqlCommand();

            //update lai trang thai icheck=0 tat ca các đợt chứng từ của TLTH
            String dsMaChungTuCu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            String[] arrMaChungTuCu = dsMaChungTuCu.Split(',');
            dk += "AND (";
            for (int j = 0; j < arrMaChungTuCu.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuCu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuCu[j]);

            }
            dk += " )";
            SQL = String.Format("UPDATE DTBS_ChungTu_TLTH SET iCheck=0 WHERE iTrangThai=1 {0}", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);

            //update lai trang thai icheck=1 cac chung tu duoc chon
            if (String.IsNullOrEmpty(dsMaChungTuTLTH))
            {
                dsMaChungTuTLTH = Guid.Empty.ToString();
            }
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            cmd = new SqlCommand();
            dk = "";
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuTLTH[j]);

            }
            SQL = String.Format("UPDATE DTBS_ChungTu_TLTH SET iCheck=1 WHERE iTrangThai=1 AND({0})", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Cập nhật bản DTBS_ChungTu_TLTH
            bang.GiaTriKhoa = maChungTuTLTHCuc;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_TLTH", dsMaChungTuTLTH);
            bang.DuLieuMoi = false;
            bang.Save();
            return 1;
        } 
        #endregion

        #region Cập nhật trạng thái duyệt của chứng từ
        /// <summary>
        /// Cập nhật trạng thái duyệt của chứng từ
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="maTrangThaiTiepTheo"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static bool CapNhatTrangThaiDuyetChungTu(string maChungTu, int maTrangThaiTiepTheo, bool TrangThaiTrinhDuyet, string MaND, string IPSua)
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiTiepTheo);
            //update trang Thai duyet dtChungTu
            DuToanBS_ChungTuModels.UpdateRecord(maChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng DTBS_ChungTuChiTiet            
            String SQL = "UPDATE DTBS_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiTiepTheo);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);

            //Update Trang Thai duyet bang DTBS_ChungTuChiTiet_PhanCap
            SQL = "UPDATE DTBS_ChungTuChiTiet_PhanCap SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu IN (SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiTiepTheo);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);

            //Update Trang Thai duyet bang DTBS_ChungTuChiTiet NSBD lan 1
            SQL = "UPDATE DTBS_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu IN (SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiTiepTheo);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return true;
        }
        
        #endregion

        #region Cập nhật trạng thái duyệt của chứng từ TLTH
        /// <summary>
        /// Cập nhật trạng thái duyệt của chứng từ TLTH
        /// </summary>
        /// <param name="maChungTuTLTH"></param>
        /// <param name="maTrangThaiTiepTheo"></param>
        /// <param name="maND"></param>
        /// <param name="ipSua"></param>
        /// <returns></returns>
        public static bool CapNhatTrangThaiDuyetChungTuTLTH(string maChungTuTLTH, int maTrangThaiTiepTheo, string maND, string ipSua)
        {
            //Update trangthai bang DTBS_ChungTu_TLTH
            string sql = "";
            sql = String.Format(@"UPDATE DTBS_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@maTrangThaiTiepTheo WHERE iID_MaChungTu_TLTH=@iID_MaChungTu");
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@maTrangThaiTiepTheo", maTrangThaiTiepTheo);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTuTLTH);
            Connection.UpdateDatabase(cmd);

            //update trangthai bang DTBS_ChungTu
            String dsMaChungTu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", maChungTuTLTH, "iID_MaChungTu"));
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                dsMaChungTu = Guid.Empty.ToString();
            }
            string[] arrMaChungTu = dsMaChungTu.Split(',');

            for (int i = 0; i < arrMaChungTu.Length; i++)
            {
                CapNhatTrangThaiDuyetChungTu(arrMaChungTu[i], maTrangThaiTiepTheo, false, maND, ipSua);
            }
            return true;
        } 
        #endregion

        #region Cập nhật trạng thái duyệt của chứng từ TLTH Cục
        /// <summary>
        /// Cập nhật trạng thái duyệt của chứng từ TLTH Cục
        /// </summary>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <param name="maTrangThaiTiepTheo"></param>
        /// <param name="maND"></param>
        /// <param name="ipSua"></param>
        /// <returns></returns>
        public static bool CapNhatTrangThaiDuyetChungTuTLTHCuc(string maChungTuTLTHCuc, int maTrangThaiTiepTheo, string maND, string ipSua)
        {
            string sql = "";
            //update trangthai bang DTBS_ChungTu_TLTHCuc
            sql = String.Format(@"UPDATE DTBS_ChungTu_TLTHCuc SET iID_MaTrangThaiDuyet=@maTrangThaiTiepTheo WHERE iID_MaChungTu_TLTHCuc=@iID_MaChungTu");
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@maTrangThaiTiepTheo", maTrangThaiTiepTheo);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTuTLTHCuc);
            Connection.UpdateDatabase(cmd);

            //update trangthai bang DTBS_ChungTu_TLTH
            String dsMaChungTuTLTHCuc = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            if (String.IsNullOrEmpty(dsMaChungTuTLTHCuc))
            {
                dsMaChungTuTLTHCuc = Guid.Empty.ToString();
            }
            String[] arrMaChungTuTLTHCuc = dsMaChungTuTLTHCuc.Split(',');
            for (int i = 0; i < arrMaChungTuTLTHCuc.Length; i++)
            {
                CapNhatTrangThaiDuyetChungTuTLTH(arrMaChungTuTLTHCuc[i], maTrangThaiTiepTheo, maND, ipSua);
            }
            return true;
        } 
        #endregion

        #region Cập nhật chứng từ TLTH
        /// <summary>
        /// Cập nhật trạng thái chứng từ TLTH sau khi trình duyệt/từ chối 1 chứng từ
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="maND"></param>
        /// <param name="sLNS"></param>
        public static void CapNhatChungTuTLTH(string maChungTu, string maND)
        {
            String SQL = "";
            SQL = String.Format(@"SELECT * FROM DTBS_ChungTu_TLTH
                                    WHERE iTrangThai=1 
                                    AND iID_MaChungTu LIKE @iID_MaChungTu
                                    ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", "%" + maChungTu + "%");
            //Lấy mã chứng từ TLTH
            String iID_MaChungTu_TLTH = Connection.GetValueString(cmd, Guid.Empty.ToString());

            if (iID_MaChungTu_TLTH != Guid.Empty.ToString())
            {
                int iID_MaTrangThaiDuyet = KiemTraTrangThaiChungTuTLTH(iID_MaChungTu_TLTH, maND);

                SQL = "UPDATE DTBS_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu_TLTH=@iID_MaChungTu_TLTH";
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH", iID_MaChungTu_TLTH);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
        } 
        #endregion

        #region Cập nhật chứng từ TLTH Cục
        /// <summary>
        /// Cập nhật trạng thái chứng từ TLTHCuc sau khi trình duyệt/từ chối 1 chứng từ TLTH
        /// </summary>
        /// <param name="maChungTuTLTH"></param>
        /// <param name="maND"></param>
        public static void CapNhatChungTuTLTHCuc(string maChungTuTLTH, string maND)
        {

            String SQL = "";
            SQL = String.Format(@"SELECT * FROM DTBS_ChungTu_TLTHCuc
                                    WHERE iTrangThai=1 
                                    AND iID_MaChungTu_TLTH LIKE @iID_MaChungTu_TLTH
                                    ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH", "%" + maChungTuTLTH + "%");
            //Lấy mã chứng từ TLTH Cục
            String maChungTuTLTHCuc = Connection.GetValueString(cmd, Guid.Empty.ToString());
            if (maChungTuTLTHCuc != Guid.Empty.ToString())
            {
                int iID_MaTrangThaiDuyet = KiemTraTrangThaiChungTuTLTHCuc(maChungTuTLTHCuc, maND);

                SQL = "UPDATE DTBS_ChungTu_TLTHCuc SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu_TLTHCuc=@iID_MaChungTu_TLTHCuc";
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
        } 
        #endregion
        #endregion

        #region Xóa Chứng Từ Dự Toán Bổ Sung
        #region Xóa Chứng Từ
        /// <summary>
        /// Xóa chứng dự toán bổ sung
        /// </summary>
        /// <param name="iID_MaChungTu">Mã chứng từ</param>
        /// <returns></returns>
        public static int XoaChungTu(string iID_MaChungTu)
        {
            SqlCommand cmd;
            //Xoa dữ liệu bang DTBS_ChungTuChiTiet_PhanCap
            string sql = String.Format(@"UPDATE DTBS_ChungTuChiTiet_PhanCap 
                                         SET iTrangThai = 0 
                                         WHERE iID_MaChungTuChiTiet IN (
                                             SELECT iID_MaChungTuChiTiet 
                                             FROM DTBS_ChungTuChiTiet_PhanCap
                                             WHERE iID_MaChungTu IN (
                                                 SELECT iID_MaChungTuChiTiet
                                                 FROM DTBS_ChungTuChiTiet
                                                 WHERE iID_MaChungTu=@iID_MaChungTu
                                            )
                                        ) ");
            cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu bảng DTBS_ChungTuChiTiet
            cmd = new SqlCommand("UPDATE DTBS_ChungTuChiTiet SET iTrangThai = 0 WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu bảng DTBS_ChungTu
            Bang bang = new Bang("DTBS_ChungTu");
            //bang.MaNguoiDungSua = MaNguoiDungSua;
            //bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.Delete();
            return 1;
        }
        #endregion

        #region Xóa Chứng Từ TLTH
        /// <summary>
        /// Xóa chứng từ gom TLTH
        /// </summary>
        /// <param name="iID_MaChungTu">Mã chứng từ TLTH</param>
        /// <returns></returns>
        public static int XoaChungTuTLTH(string iID_MaChungTuTLTH)
        {

            // Xóa dữ liệu bảng DTBS_ChungTu_TLTH
            Bang bang = new Bang("DTBS_ChungTu_TLTH");
            bang.GiaTriKhoa = iID_MaChungTuTLTH;
            bang.Delete();

            // Update trạng thái gom của chứng từ bảng DTBS_ChungTu
            string dsMaChungTu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTuTLTH,
                    "iID_MaChungTu"));
            string dk = "";
            string[] arrMaChungtu = dsMaChungTu.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungtu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungtu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungtu[j]);
            }
            string SQL = @"UPDATE DTBS_ChungTu SET iCheck=0 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return 1;
        }
        #endregion

        #region Xóa Chứng Từ TLTHCuc
        /// <summary>
        /// Xóa chứng từ TLTH Cục
        /// </summary>
        /// <param name="iID_MaChungTuTLTHCuc">Mã chứng tứ TLTH Cuc</param>
        /// <returns></returns>
        public static int XoaChungTuTLTHCuc(string iID_MaChungTuTLTHCuc)
        {
            //Xóa dữ liệu bảng DTBS_ChungTu_TLTHCuc
            Bang bang = new Bang("DTBS_ChungTu_TLTHCuc");
            bang.GiaTriKhoa = iID_MaChungTuTLTHCuc;
            bang.Delete();

            //update lai trang thai check chứng từ TLTH
            string dsMaChungTuTLTH = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc",
                iID_MaChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            string dk = "";
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuTLTH[j]);
            }
            String SQL = @"UPDATE DTBS_ChungTu_TLTH SET iCheck=0 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return 1;
        }
        #endregion
        #endregion

        #region Lấy mã trạng thái tiếp theo
        public static int LayMaTrangThaiTrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            String iID_MaNguoiDungTao = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);
            dt.Dispose();

            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }

            }

                //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else if (checkTroLyTongHop && MaND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            else
            {
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                {
                    int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                    if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                    }
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTrinhDuyetBaoDam(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            String iID_MaNguoiDungTao = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);
            String iKyThuat = Convert.ToString(dt.Rows[0]["iKyThuat"]);
            dt.Dispose();
            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = false;
            int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
            bool bcheckTC = false;
            if (iKyThuat == "1")
            {
                //     CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet);
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
                return vR;
            }
            else
            {
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
                bcheckTC = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet);
            }

            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {

                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }

            }

                //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else if (checkTroLyTongHop && MaND == iID_MaNguoiDungTao && bcheckTC)
            {
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            else
            {
                if (iKyThuat == "1")
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
                    {
                        if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                        }
                    }
                }
                else
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                    {
                        if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                        }
                    }
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTrinhDuyetTLTH(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTrinhDuyetTLTHBaoDam(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTuChoiTLTHCuc(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTHCuc(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static int LayMaTrangThaiTuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            dt.Dispose();


            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                }

            }
            //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else
            {
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                {
                    int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                    if (iID_MaTrangThaiDuyet_TuChoi > 0)
                    {
                        //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                        //cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                        //if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                        //{
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        //}
                        //cmd.Dispose();
                    }
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTuChoiBaoDam(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            String iKyThuat = Convert.ToString(dt.Rows[0]["iKyThuat"]);
            dt.Dispose();


            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = false;
            if (iKyThuat == "1")
            {
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet);
            }
            else
            {
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
            }
            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                }

            }
            //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else
            {
                if (iKyThuat == "1")
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
                    {
                        int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                        if (iID_MaTrangThaiDuyet_TuChoi > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        }
                    }
                }
                else
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                    {
                        int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                        if (iID_MaTrangThaiDuyet_TuChoi > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        }
                    }
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTuChoiTLTHBaoDam(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    //if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    //{
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    //}
                    //cmd.Dispose();
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTuChoiTLTH(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    //if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    //{
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    //}
                    //cmd.Dispose();
                }
            }
            return vR;
        }
        public static int LayMaTrangThaiTrinhDuyetTLTHCuc(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTHCuc(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        } 
        #endregion
    }
}