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
    public class TinhChatCapThuModels
    {
        public static DataTable Get_dt(Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            
            DK = "iTrangThai=1";

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }

            SQL = String.Format("SELECT * FROM KTTG_TinhChatCapThu WHERE {0} ORDER BY sTen", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Get_dtTinhChatCapThu
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_dtTinhChatCapThu()
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1";

            SQL = String.Format("SELECT * FROM KTTG_TinhChatCapThu WHERE {0} ORDER BY sTen", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_RowTinhChatCapThu(String iID_MaTinhChatCapThu)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaTinhChatCapThu=@iID_MaTinhChatCapThu";
            cmd.Parameters.AddWithValue("@iID_MaTinhChatCapThu", iID_MaTinhChatCapThu);

            SQL = String.Format("SELECT * FROM KTTG_TinhChatCapThu WHERE {0}", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}