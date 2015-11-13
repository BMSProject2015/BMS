using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace VIETTEL.Models
{
    public class PhongBanModels
    {
        public static DataTable Get_Table(String iID_MaPhongBan)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_PhongBan WHERE iTrangThai=1 AND iID_MaPhongBan = @iID_MaPhongBan");
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable GetDanhSachPhongBan()
        {
            SqlCommand cmd = new SqlCommand("SELECT *,sTen+'-'+sMoTa as TenHT  FROM NS_PhongBan WHERE iTrangThai=1 ORDER BY sKyHieu");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static String Get_TenPhongBan(String iID_MaPhongBan)
        {
            String vR = "";
            SqlCommand cmd = new SqlCommand("SELECT sTenPhongBan FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaPhongBan=@iID_MaPhongBan");
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
    }
}