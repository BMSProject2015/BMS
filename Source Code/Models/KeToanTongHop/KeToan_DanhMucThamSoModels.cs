using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;


namespace VIETTEL.Models
{
    public class KeToan_DanhMucThamSoModels
    {
        public static String LayThongTinThamSo(String sKyHieu, String iNamLamViec)
        {


            String SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);            
            String sVR = Connection.GetValueString(cmd,"");
            cmd.Dispose();
            return sVR;
        }
        /// <summary>
        /// Lấy dt danh sách các tham số được dùng cho báo cáo.
        /// </summary>
        /// <param name="sBaoCao_ControllerName"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iThangLamViec"></param>
        /// <returns></returns>
        public static DataTable Get_dtDanhSachThamSoCuaBaoCao(String sBaoCao_ControllerName, String iNamLamViec)
        {


            String SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sBaoCao_ControllerName=@sBaoCao_ControllerName";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sBaoCao_ControllerName", sBaoCao_ControllerName);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);            
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy dt danh sách các tham số được dùng cho báo cáo.
        /// </summary>
        /// <param name="sBaoCao_ControllerName"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iThangLamViec"></param>
        /// <returns></returns>
        public static DataTable Get_dtDanhSachThamSoCuaBaoCao(String sBaoCao_ControllerName,String sKyHieu, String iNamLamViec)
        {


            String SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sBaoCao_ControllerName=@sBaoCao_ControllerName AND sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            cmd.Parameters.AddWithValue("@sBaoCao_ControllerName", sBaoCao_ControllerName);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy dt danh sách các tham số được dùng cho báo cáo.
        /// </summary>
        /// <param name="sKyHieu"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable Get_ThamSoCuaBaoCao(String sKyHieu, String iNamLamViec)
        {

            String SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Danh sách tham số
        /// </summary>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public static DataTable GetDanhSach(int Trang = 1, int SoBanGhi = 0, int Nam = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String SQL = "SELECT * FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec";
            String SQL = @"SELECT TS.iID_MaThamSo, TS.sKyHieu, TS.sBaoCao_ControllerName, BC.sTen, TS.sNoiDung, TS.sThamSo, 
TS.bChoPhepXoa FROM KT_DanhMucThamSo AS TS LEFT OUTER JOIN KT_DanhMucThamSo_BaoCao AS BC ON TS.sBaoCao_ControllerName = BC.iID_MaBaoCao WHERE TS.iTrangThai=1 AND TS.iNamLamViec=@iNamLamViec";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            vR = CommonFunction.dtData(cmd, "TS.dNgayTao", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Đếm số bản ghi
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public static int GetDanhSach_Count(int Nam = 0)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT COUNT(*) FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết tham số
        /// </summary>
        /// <param name="iID_MaThamSo"></param>
        /// <returns></returns>
        public static DataTable Get_ChiTiet(String iID_MaThamSo)
        {
            DataTable vR;
            String SQL = String.Format("SELECT * FROM KT_DanhMucThamSo WHERE  iTrangThai=1 AND iID_MaThamSo = @iID_MaThamSo");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaThamSo", iID_MaThamSo);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }
        /// <summary>
        /// Danh sách báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_BaoCao(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaBaoCao, sTen FROM KT_DanhMucThamSo_BaoCao WHERE  iTrangThai=1 ORDER BY sTen");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaBaoCao"] = string.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaThamSo"></param>
        /// <returns></returns>
        public static DataTable GetRow_ThamSo(String sKyHieu)
        {
            DataTable vR;
            String SQL = String.Format("SELECT iID_MaThamSo FROM KT_DanhMucThamSo WHERE  sKyHieu = @sKyHieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}