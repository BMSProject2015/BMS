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
    public class HomeModels
    {
        /// <summary>
        /// Lấy tổng số chứng từ dự toán theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int DuToan_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM DT_ChungTu " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ chỉ tiêu theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int ChiTieu_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM PB_ChiTieu " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ phân bổ theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int PhanBo_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM PB_PhanBo " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ cấp phát theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int CapPhat_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM CP_CapPhat " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ quyết toán theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int QuyetToan_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM QTA_ChungTu " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ quyết toán quân số theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int QuyetToanQuanSo_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM QTQS_ChungTu " +
                         "WHERE iTrangThai=1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ người có công theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int NguoiCoCong_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM NCC_ChungTu " +
                         "WHERE iTrangThai=1 AND iLoai = 1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ trợ cấp khó khăn theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int TroCapKhoKhan_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM NCC_ChungTu " +
                         "WHERE iTrangThai=1 AND iLoai = 2 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ kế hoạch thu theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int KeHoachThuNop_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM TN_ChungTu " +
                         "WHERE iTrangThai=1 AND iLoai = 1 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ tổng thu năm theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int ThuNamThuNop_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM TN_ChungTu " +
                         "WHERE iTrangThai=1 AND iLoai = 3 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ quyết toán thu theo trạng thái duyệt
        /// </summary>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static int QuyetToanThuNop_TheoTrangThai(String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd;
            int vR = 0;
            String SQL = "SELECT COUNT(*) " +
                         "FROM TN_ChungTu " +
                         "WHERE iTrangThai=1 AND iLoai = 3 AND " +
                               "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}