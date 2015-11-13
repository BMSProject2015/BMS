using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class BaoHiem_DuyetChungTuChiModels
    {
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaChungTuChi)
        {
            String iID_MaDuyetChungTu;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetChungTuChiCuoiCung FROM BH_ChungTuChi WHERE iID_MaChungTuChi=@iID_MaChungTuChi");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
            iID_MaDuyetChungTu = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM BH_ChungTuChiChiTiet WHERE iID_MaChungTuChi=@iID_MaChungTuChi AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE BH_DuyetChungTuChi SET sSua=@sSua WHERE iID_MaDuyetChungTu=@iID_MaDuyetChungTu");
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTu", iID_MaDuyetChungTu);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable GetDuyetChungTu(String MaChungTuChi) {
            DataTable vR = null;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM BH_DuyetChungTuChi WHERE iID_MaChungTuChi=@iID_MaChungTuChi ORDER BY dNgayDuyet");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChi", MaChungTuChi);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}