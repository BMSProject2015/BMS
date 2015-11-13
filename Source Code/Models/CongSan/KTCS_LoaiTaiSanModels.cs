using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
namespace VIETTEL.Models
{
    public class KTCS_LoaiTaiSanModels
    {
        public static NameValueCollection LayThongTinLoaiTaiSan(String iID_Ma)
        {
            NameValueCollection data = new NameValueCollection();
            
            DataTable dt = Get_dtChiTietLoaiTaiSan(iID_Ma);
            String ColName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ColName = dt.Columns[i].ColumnName;
                    data.Add(ColName, Convert.ToString(dt.Rows[0][ColName]));
                }
            }
            return data;
        }

        public static DataTable Get_dtDSLoaiTaiSan()
        {            
            String SQL = "SELECT * FROM KTCS_LoaiTaiSan WHERE iTrangThai=1";
            return Connection.GetDataTable(SQL);
        }

        public static DataTable Get_dtChiTietLoaiTaiSan(String iID_Ma)
        {
            DataTable dt;
            String SQL = "SELECT * FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 AND iID_Ma=@iID_Ma";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_Ma", iID_Ma);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}