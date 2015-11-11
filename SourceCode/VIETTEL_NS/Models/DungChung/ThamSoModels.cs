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
    public class ThamSoModels
    {
        public static string getThamSo(int MaPhanHe, string sKyhieu)
        {
            string vR = String.Empty;
            DataTable dt = null;
            string sql = "SELECT sThamSo FROM DC_ThamSo WHERE iID_MaPhanHe=@iID_MaPhanHe AND sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", MaPhanHe);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyhieu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                vR = Convert.ToString(dt.Rows[0]["sThamSo"]);
            }
            if (dt != null) dt.Dispose();
            return vR;
        }
    }
}