using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class KTCT_TienGui_DuyetChungTuModels
    {
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaChungTu)
        {
            String iID_MaDuyetChungTu;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetChungTuCuoiCung FROM KTTG_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            iID_MaDuyetChungTu = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM KTTG_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("{0}<br/>", dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE KTTG_DuyetChungTu SET sSua=@sSua WHERE iID_MaDuyetChungTu=@iID_MaDuyetChungTu");
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTu", iID_MaDuyetChungTu);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable GetDuyetChungTu(String iID_MaChungTu)
        {
            DataTable vR = null;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM KTTG_DuyetChungTu WHERE iID_MaChungTu=@iID_MaChungTu ORDER BY dNgayDuyet");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String CheckRutDuToan(String iID_MaChungTu_Duyet)
        {
            String vR = "";

            SqlCommand cmd;
            cmd = new SqlCommand("SELECT SUM(rDTRut) FROM KTKB_ChungTuChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();

            return vR;
        }

        public static String CheckUyNhiemChi(String iID_MaChungTu_Duyet)
        {
            String vR = "";

            SqlCommand cmd;
            cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTTG_ChungTuChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();

            return vR;
        }
    }
}