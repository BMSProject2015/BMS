using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    /// <summary>
    /// Lớp quản lý luồng dữ liệu
    /// </summary>
    public class LuongCongViecModel
    {
        public static int iLoaiTrangThaiDuyet_MoiTao = 1;
        public static int iLoaiTrangThaiDuyet_DangDuyet = 2;
        public static int iLoaiTrangThaiDuyet_TuChoi = 3;
        public static int iLoaiTrangThaiDuyet_DaDuyet = 4;

        public static int iDoiTuongNguoiDung_TroLyPhongBan = 1;
        public static int iDoiTuongNguoiDung_TroLyTongHop = 2;
        public static int iDoiTuongNguoiDung_TruongPhong = 3;
        public static int iDoiTuongNguoiDung_ThuTruong = 4;
        public static int iDoiTuongNguoiDung_TroLyTongHopCuc = 5;
        public static Boolean KiemTra_TroLyPhongBan(String MaND)
        {
            Boolean vR = false;
            SqlCommand cmd = new SqlCommand("SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            vR = (Convert.ToInt32(Connection.GetValue(cmd, 0)) == iDoiTuongNguoiDung_TroLyPhongBan);
            cmd.Dispose();
            return vR;
        }

        public static Boolean KiemTra_TroLyTongHop(String MaND)
        {
            Boolean vR = false;
            SqlCommand cmd = new SqlCommand("SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            vR = (Convert.ToInt32(Connection.GetValue(cmd, 0)) == iDoiTuongNguoiDung_TroLyTongHop);
            cmd.Dispose();
            return vR;
        }

        public static Boolean KiemTra_TruongPhong(String MaND)
        {
            Boolean vR = false;
            SqlCommand cmd = new SqlCommand("SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            vR = (Convert.ToInt32(Connection.GetValue(cmd, 0)) == iDoiTuongNguoiDung_TruongPhong);
            cmd.Dispose();
            return vR;
        }

        public static Boolean KiemTra_ThuTruong(String MaND)
        {
            Boolean vR = false;
            SqlCommand cmd = new SqlCommand("SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            vR = (Convert.ToInt32(Connection.GetValue(cmd, 0)) == iDoiTuongNguoiDung_ThuTruong);
            cmd.Dispose();
            return vR;
        }
        public static Boolean KiemTra_TroLyTongHopCuc(String MaND)
        {
            Boolean vR = false;
            SqlCommand cmd = new SqlCommand("SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            vR = (Convert.ToInt32(Connection.GetValue(cmd, 0)) == iDoiTuongNguoiDung_TroLyTongHopCuc);
            cmd.Dispose();
            return vR;
        }
        public static Boolean KiemTra_NguoiDungDuocDuyet(String MaND, int iID_MaPhanHe)
        {
            Boolean vR = false;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaNhomNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            String iID_MaNhomNguoiDung = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            int iTrangThaiDaDuyet = Get_iID_MaTrangThaiDuyet_DaDuyet(iID_MaPhanHe);
            cmd = new SqlCommand("SELECT iID_MaNhomNguoiDung FROM NS_PhanHe_TrangThaiDuyet WHERE iID_MaPhanHe=@iID_MaPhanHe AND iID_MaTrangThaiDuyet_TrinhDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet");
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iTrangThaiDaDuyet);
            String iID_MaNhomNguoiDung_Duyet = Convert.ToString(Connection.GetValue(cmd, "0000"));
            vR = (iID_MaNhomNguoiDung_Duyet == iID_MaNhomNguoiDung);
            return vR;
        }


        /// <summary>
        /// Hàm lấy danh sách tất cả các trạng thái duyệt của phân hệ được sắp xếp theo số thứ tự
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <returns></returns>
        public static DataTable Get_dtDSTrangThaiDuyet(int iID_MaPhanHe)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_PhanHe_TrangThaiDuyet WHERE iTrangThai=1 AND iID_MaPhanHe=@iID_MaPhanHe ORDER BY iSTT");
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Hàm lấy danh sách các trạng thái duyệt của phân hệ của 1 người dùng được sắp xếp theo số thứ tự
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <param name="MaND">Mã người dùng đang làm việc</param>
        /// <returns></returns>
        public static DataTable Get_dtDSTrangThaiDuyet_DuocXem(int iID_MaPhanHe, String MaND)
        {
            String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
            DataTable vR;
            vR = Get_dtDSTrangThaiDuyet_NhomNguoiDung_DuocXem(iID_MaPhanHe, iID_MaNhomNguoiDung);
            return vR;
        }
        public static DataTable Get_dtDSTrangThaiDuyet_NhomNguoiDung_DuocXem(int iID_MaPhanHe, String iID_MaNhomNguoiDung)
        {
            
            DataTable vR;
            String SQL = "SELECT NS_PhanHe_TrangThaiDuyet.* " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                              "INNER JOIN NS_PhanHe_TrangThaiDuyet_NhomNguoiDung ON (NS_PhanHe_TrangThaiDuyet.iID_MaTrangThaiDuyet=NS_PhanHe_TrangThaiDuyet_NhomNguoiDung.iID_MaTrangThaiDuyet) " +
                         "WHERE NS_PhanHe_TrangThaiDuyet.iTrangThai=1 AND " +
                               "NS_PhanHe_TrangThaiDuyet_NhomNguoiDung.iTrangThai=1 AND " +
                               "NS_PhanHe_TrangThaiDuyet.iID_MaPhanHe=@iID_MaPhanHe AND " +
                               "NS_PhanHe_TrangThaiDuyet_NhomNguoiDung.iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung " +
                         "ORDER BY NS_PhanHe_TrangThaiDuyet.iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", iID_MaNhomNguoiDung);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy xâu điều kiện để kiểm tra các chứng từ mà người dùng có quyền xem
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <param name="MaND">Mã người dùng đang làm việc</param>
        /// <returns>Xâu điều kiện luôn khác rỗng</returns>
        public static String Get_DieuKien_TrangThaiDuyet_DuocXem(int iID_MaPhanHe, String MaND)
        {   
            String vR=" 0=1 ";
            DataTable dt = Get_dtDSTrangThaiDuyet_DuocXem(iID_MaPhanHe, MaND);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (vR != "") vR += " OR ";
                vR += String.Format("iID_MaTrangThaiDuyet={0}", dt.Rows[i]["iID_MaTrangThaiDuyet"]);
            }
            dt.Dispose();
            if (vR != "") vR = String.Format("({0})", vR);
            return vR;
        }

        /// <summary>
        /// Kiểm tra xem người dùng có được quyền thêm chứng từ hay không
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <param name="MaND">Mã người dùng đang làm việc</param>
        /// <returns>True: nếu người dùng được thêm chứng từ, False: ngược lại</returns>
        public static Boolean NguoiDung_DuocThemChungTu(int iID_MaPhanHe, String MaND)
        {
            SqlCommand cmd;
            String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
            Boolean vR=false;
            String SQL = "SELECT COUNT(*) " + 
                         "FROM NS_PhanHe_TrangThaiDuyet " + 
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=1 AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe AND " +
                               "iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", iID_MaNhomNguoiDung);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0;
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Kiểm tra xem người dùng có được quyền sửa chứng từ hay không
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <param name="MaND">Mã người dùng đang làm việc</param>
        /// <returns>True: nếu người dùng được thêm chứng từ, False: ngược lại</returns>
        public static Boolean NguoiDung_DuocSuaChungTu(int iID_MaPhanHe, String MaND, int iID_MaTrangThaiDuyet)
        {
            String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
            Boolean vR = false;
            String SQL = "SELECT Count(*) " +
                         "FROM NS_PhanHe_TrangThaiDuyet " + 
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe AND "+
                               "iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung AND "+
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", iID_MaNhomNguoiDung);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0;
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Hàm lấy mã trạng thái tạo mới của luồng
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <returns>-1: Nếu không có mã nào</returns>
        public static int Get_iID_MaTrangThaiDuyetMoi(int iID_MaPhanHe)
        {
            int vR;
            String SQL = "SELECT iID_MaTrangThaiDuyet " + 
                         "FROM NS_PhanHe_TrangThaiDuyet " + 
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=1 AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1));
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Hàm lấy mã trạng thái đã duyệt của luồng
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <returns>-1: Nếu không có mã nào</returns>
        public static int Get_iID_MaTrangThaiDuyet_DaDuyet(int iID_MaPhanHe)
        {
            int vR;
            String SQL = "SELECT iID_MaTrangThaiDuyet " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=4 AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1));
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Hàm lấy mã DataTable của 1 mã trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet">iID_MaTrangThaiDuyet</param>
        /// <returns></returns>
        public static DataTable Get_dtTrangThaiDuyet(int iID_MaTrangThaiDuyet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_PhanHe_TrangThaiDuyet WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet");
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy mã trạng thái duyệt được sửa của người dùng trong phân hệ
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <param name="MaND">Mã người dùng đang làm việc</param>
        /// <returns> >=0: nếu có mã trạng thái duyệt được sửa, -1: ngược lại</returns>
        public static int Get_iID_MaTrangThaiDuyet_DuocSua(int iID_MaPhanHe, String MaND)
        {
            String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
            int vR = -1;
            String SQL = "SELECT iID_MaTrangThaiDuyet " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe AND " +
                               "iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", iID_MaNhomNguoiDung);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1));
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Kiểm tra 1 trạng thái có phải là trạng thái từ chối hay không
        /// </summary>
        /// <param name="iID_MaPhanHe"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns>True: Nếu là trạng thái từ chối, False: Ngược lại</returns>
        public static Boolean KiemTra_TrangThaiTuChoi(int iID_MaPhanHe, int iID_MaTrangThaiDuyet)
        {
            Boolean vR;
            String SQL = "SELECT Count(*) " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=3 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1)) > 0;
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Kiểm tra 1 trạng thái có phải là trạng thái trình duyệt hay không
        /// </summary>
        /// <param name="iID_MaPhanHe"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns>True: Nếu là trạng thái từ chối, False: Ngược lại</returns>
        public static Boolean KiemTra_TrangThaiTrinhDuyet(int iID_MaPhanHe, int iID_MaTrangThaiDuyet)
        {
            Boolean vR;
            String SQL = "SELECT Count(*) " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=2 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1)) > 0;
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Kiểm tra 1 trạng thái có phải là trạng thái khởi tạo hay không
        /// </summary>
        /// <param name="iID_MaPhanHe"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns>True: Nếu là trạng thái từ chối, False: Ngược lại</returns>
        public static Boolean KiemTra_TrangThaiKhoiTao(int iID_MaPhanHe, int iID_MaTrangThaiDuyet)
        {
            Boolean vR;
            String SQL = "SELECT Count(*) " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1)) > 0;
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Kiểm tra 1 trạng thái có phải là trạng thái đã duyệt hay không
        /// </summary>
        /// <param name="iID_MaPhanHe"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns>True: Nếu là trạng thái từ chối, False: Ngược lại</returns>
        public static Boolean KiemTra_TrangThaiDaDuyet(int iID_MaPhanHe, int iID_MaTrangThaiDuyet)
        {
            Boolean vR;
            String SQL = "SELECT Count(*) " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=4 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToInt32(Connection.GetValue(cmd, -1)) > 0;
            cmd.Dispose();
            return vR;
        }

        public static int Luong_iID_MaTrangThaiDuyet_TuChoi(int iID_MaTrangThaiDuyet)
        {
            int vR=-1;
            DataTable dt = Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet);
            if (dt.Rows.Count > 0)
            {
                //if (Convert.ToInt32(dt.Rows[0]["iLoaiTrangThaiDuyet"]) == iLoaiTrangThaiDuyet_DangDuyet)
                //{
                    vR = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet_TuChoi"]);
                //}
            }
            dt.Dispose();
            return vR;
        }

        public static int Luong_iID_MaTrangThaiDuyet_TrinhDuyet(int iID_MaTrangThaiDuyet)
        {
            int vR = -1;
            DataTable dt = Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet);
            if (dt.Rows.Count > 0)
            {
                vR = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet_TrinhDuyet"]);
            }
            dt.Dispose();
            return vR;
        }

        public static string TrangThaiDuyet(int iID_MaTrangThaiDuyet)
        {
            string vR = "";
            DataTable dt = Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet);
            if (dt.Rows.Count > 0)
            {
                vR = Convert.ToString(dt.Rows[0]["sTen"]);               
            }
            if (dt != null) dt.Dispose();
            return vR;
        }

        public static string TrangThaiDuyet_MauSac(int iID_MaTrangThaiDuyet)
        {
            string vR = "";
            DataTable dt = Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet);
            if (dt.Rows.Count > 0)
            {
                vR = Convert.ToString(dt.Rows[0]["sMauSac"]);
            }
            if (dt != null) dt.Dispose();
            return vR;
        }
        public static bool NguoiDungDuyet (int iID_MaPhanHe, string MaND)
        {
            bool value = false;
            String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
            string query =
                string.Format(
                    @"select iID_MaNhomNguoiDung from NS_PhanHe_TrangThaiDuyet
                                            where iLoaiTrangThaiDuyet = 4 and  iID_MaPhanHe = @iID_MaPhanHe");
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count != 0)
            {
                if (iID_MaNhomNguoiDung.Equals(Convert.ToString(dt.Rows[0][0])))
                {
                    value = true;
                }
                
            }
            cmd.Dispose();
            return value;
        }
        public static bool NguoiDungTaoMoi(int iID_MaPhanHe, string MaND)
        {
            bool value = false;
            String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
            string query =
                string.Format(
                    @"select iID_MaNhomNguoiDung from NS_PhanHe_TrangThaiDuyet
                                            where iLoaiTrangThaiDuyet = 1 and  iID_MaPhanHe = @iID_MaPhanHe");
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count != 0)
            {
                if (iID_MaNhomNguoiDung.Equals(Convert.ToString(dt.Rows[0][0])))
                {
                    value = true;
                }

            }
            cmd.Dispose();
            return value;
        }
        public static int layTrangThaiDuyet(int iID_MaPhanHe)
        {
            int vR = -1;
            String SQL = "SELECT iID_MaTrangThaiDuyet " +
                         "FROM NS_PhanHe_TrangThaiDuyet " +
                         "WHERE iTrangThai=1 AND " +
                               "iLoaiTrangThaiDuyet=4 AND " +
                               "iID_MaPhanHe=@iID_MaPhanHe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count != 0)
            {
                vR = Convert.ToInt32(dt.Rows[0][0]);
            }
            cmd.Dispose();
            dt.Dispose();
            return vR;
        }
    }
}