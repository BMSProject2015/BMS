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
    public class MucLucDuAnModels
    {
        public static DataTable GetMLDA(Dictionary<String, String> arrGiaTriTimKiem, String MaND)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 AND iTrangThai=1";
            String[] arrDSTruong = "iID_MaDonVi,iLoai,iThamQuyen,iTinhChat,iNhom,sTen,bHoanThanh".Split(',');
            if (arrGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }
            String SQL = String.Format("SELECT * FROM NS_MucLucDuAn WHERE {0} ORDER BY iID_MaDonVi,iLoai,iThamQuyen,iTinhChat,iNhom,sTen", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int GetMLDA_Count()
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 ";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_MucLucDuAn WHERE {0} ", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        public static DataTable getMaDuAn(String sMaCongTrinh)
        {
            DataTable vR;
            String SQL = String.Format("SELECT iID_MaDanhMucDuAn FROM NS_MucLucDuAn WHERE  sMaCongTrinh = @sMaCongTrinh");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaCongTrinh", sMaCongTrinh);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }

        public static DataTable getDetail(String iID_MaDanhMucDuAn)
        {
            DataTable vR;
            String SQL = String.Format("SELECT * FROM NS_MucLucDuAn WHERE  iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }
        public static int getMax(String sMaCongTrinh)
        {
            String SQL =
                String.Format(
                    @" SELECT MAX(SUBSTRING(sMaCongTrinh,len(sMaCongTrinh)-4,5)) as sMaCongTrinh FROM NS_MucLucDuAn
WHERE sMaCongTrinh LIKE @sMaCongTrinh AND iTrangThai=1");
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaCongTrinh", sMaCongTrinh+"%");
            return Convert.ToInt16( Connection.GetValue(cmd, 0));

        }
      
    }
}