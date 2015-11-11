using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class Luong_DanhMucThamSoModels
    {
        public static String Get_ThamSo(String sKyHieu)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sThamSo FROM L_DanhMucThamSo WHERE iTrangThai=1 AND bConSuDung=1 AND sKyHieu=@sKyHieu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);

            return Convert.ToString(Connection.GetValue(cmd,""));
        }
    }
}