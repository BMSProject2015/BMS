using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
namespace VIETTEL.Models
{
    public class QLDA_CongTrinhDuAnModels
    {
        public static DataTable ddl_DanhMucDuAn(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMucDuAn, sDeAn +' - '+ sDuAn +' - '+ sDuAnThanhPhan +' - '+ sCongTrinh +' - '+ sHangMucCongTrinh +' - '+ sHangMucChiTiet +' - '+ sTenDuAn AS TenHT " +
                    "FROM QLDA_DanhMucDuAn WHERE iTrangThai = 1 ORDER By sXauNoiMa");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDanhMucDuAn"] = Guid.Empty;
                R["TenHT"] = "--- Danh sách công trình dự án ---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        public static DataTable Row_DanhMucDuAn(String iID_MaDanhMucDuAn)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT *, sDeAn +' - '+ sDuAn +' - '+ sDuAnThanhPhan +' - '+ sCongTrinh +' - '+ sHangMucCongTrinh +' - '+ sHangMucChiTiet +' - '+ sTenDuAn AS TenHT " +
                    "FROM QLDA_DanhMucDuAn WHERE iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND iTrangThai = 1 ORDER By sXauNoiMa");
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn); 
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}