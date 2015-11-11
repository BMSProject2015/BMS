using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class QuyetToan_DuyetChungTuModels
    {
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaChungTu)
        {
            String iID_MaDuyetChungTu;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetChungTuCuoiCung FROM QTA_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            iID_MaDuyetChungTu = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM QTA_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE QTA_DuyetChungTu SET sSua=@sSua WHERE iID_MaDuyetChungTu=@iID_MaDuyetChungTu");
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTu", iID_MaDuyetChungTu);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable GetDuyetChungTu(String iID_MaChungTu)
        {
            DataTable vR = null;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM QTA_DuyetChungTu WHERE iID_MaChungTu=@iID_MaChungTu ORDER BY dNgayDuyet");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}