using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class PhanBo_DuyetPhanBoModels
    {
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaPhanBo)
        {
            String iID_MaDuyetPhanBo;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetPhanBoCuoiCung FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo");
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            iID_MaDuyetPhanBo = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM PB_PhanBoChiTiet WHERE iID_MaPhanBo=@iID_MaPhanBo AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE PB_DuyetPhanBo SET sSua=@sSua WHERE iID_MaDuyetPhanBo=@iID_MaDuyetPhanBo");
            cmd.Parameters.AddWithValue("@iID_MaDuyetPhanBo", iID_MaDuyetPhanBo);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable GetDuyetPhanBo(String MaPhanBo)
        {
            DataTable vR = null;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM PB_DuyetPhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo ORDER BY dNgayDuyet");
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", MaPhanBo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}