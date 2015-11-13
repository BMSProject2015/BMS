using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class PhanBo_DuyetChiTieuModels
    {
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaChiTieu)
        {
            String iID_MaDuyetChiTieu;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetChiTieuCuoiCung FROM PB_ChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            iID_MaDuyetChiTieu = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE PB_DuyetChiTieu SET sSua=@sSua WHERE iID_MaDuyetChiTieu=@iID_MaDuyetChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaDuyetChiTieu", iID_MaDuyetChiTieu);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable GetDuyetChiTieu(String MaChiTieu)
        {
            DataTable vR = null;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM PB_DuyetChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu ORDER BY dNgayDuyet");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", MaChiTieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}