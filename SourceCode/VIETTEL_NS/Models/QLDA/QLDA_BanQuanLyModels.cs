using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Models
{
    public class QLDA_BanQuanLyModels
    {
        public static NameValueCollection LayThongTinBanQuanLy(String iID_MaBanQuanLy)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_BanQuanLyChiTiet(iID_MaBanQuanLy);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                if (dt.Rows.Count > 0)
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                else
                    Data[colName] = "";
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_BanQuanLyChiTiet(String iID_MaBanQuanLy)
        {
            if (String.IsNullOrEmpty(iID_MaBanQuanLy)) iID_MaBanQuanLy = "-1";
            String SQL = String.Format("SELECT * FROM QLDA_BanQuanLy WHERE iID_MaBanQuanLy={0}", iID_MaBanQuanLy);
            return Connection.GetDataTable(SQL);
        }

        public static DataTable Get_ChuDauTu(Boolean SelectAll = false, String Title = "--- Chọn chủ đầu tư ---")
        {
            String SQL = "SELECT iID_MaChuDauTu,sTen FROM QLDA_ChuDauTu WHERE iTrangThai=1";
            DataTable dt = Connection.GetDataTable(SQL);
            if (SelectAll)
            {
                DataRow R = dt.NewRow();
                R["iID_MaChuDauTu"] = "";
                R["sTen"] = Title;
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }

        public static DataTable Get_DanhSach(String iID_MaChuDauTu, String sTenBanQuanLy, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";
            if (String.IsNullOrEmpty(iID_MaChuDauTu) == false && iID_MaChuDauTu != "")
            {
                DK += " AND iID_MaChuDauTu = @iID_MaChuDauTu";
                cmd.Parameters.AddWithValue("@iID_MaChuDauTu", iID_MaChuDauTu);
            }
            if (String.IsNullOrEmpty(sTenBanQuanLy) == false && sTenBanQuanLy != "")
            {
                DK += " AND sTenBanQuanLy LIKE @sTenBanQuanLy";
                cmd.Parameters.AddWithValue("@sTenBanQuanLy", sTenBanQuanLy+"%");
            }
            String SQL = String.Format("SELECT * FROM QLDA_BanQuanLy WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaChuDauTu,sTenBanQuanLy", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iID_MaChuDauTu, String sTenBanQuanLy)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";
            if (String.IsNullOrEmpty(iID_MaChuDauTu) == false && iID_MaChuDauTu != "")
            {
                DK += " AND iID_MaChuDauTu = @iID_MaChuDauTu";
                cmd.Parameters.AddWithValue("@iID_MaChuDauTu", iID_MaChuDauTu);
            }
            if (String.IsNullOrEmpty(sTenBanQuanLy) == false && sTenBanQuanLy != "")
            {
                DK += " AND sTenBanQuanLy LIKE @sTenBanQuanLy";
                cmd.Parameters.AddWithValue("@sTenBanQuanLy", sTenBanQuanLy + "%");
            }
            String SQL = String.Format("SELECT Count(*) FROM QLDA_ChuDauTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}