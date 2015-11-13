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
    public class NguoiDung_PhongBanModels
    {
        public static DataTable getList(String MaNguoiDung = "", String MaPhongBan = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = "SELECT * FROM QT_NguoiDung as qt WHERE exists (SELECT iID_MaNguoiDungPhongBan FROM NS_NguoiDung_PhongBan as ns WHERE ns.iTrangThai=1 AND ns.sMaNguoiDung=qt.sID_MaNguoiDung";
            if (MaPhongBan != "")
            {
                SQL += " AND ns.iID_MaPhongBan= @MaPhongBan)";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            else SQL += ")";
            if (MaNguoiDung != "")
            {
                SQL += " AND qt.sID_MaNguoiDung like N'%' + @sID_MaNguoiDung + '%'";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
            }

            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int getList_Count(String MaNguoiDung = "", String MaPhongBan = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_NguoiDung_PhongBan WHERE {0}", DK);
            if (MaPhongBan != "")
            {
                SQL += " AND iID_MaPhongBan= @MaPhongBan)";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }          
            if (MaNguoiDung != "")
            {
                SQL += " AND sMaNguoiDung like N'%' + @sID_MaNguoiDung + '%'";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
            }
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable getDS(string sMaNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Empty;
            if (sMaNguoiDung == "")
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungPhongBan, nd.sMaNguoiDung, nd.iID_MaPhongBan, dv.sTen, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan");
            else
            {
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungPhongBan, nd.sMaNguoiDung, nd.iID_MaPhongBan, dv.sTen, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan AND nd.sMaNguoiDung=@sMaNguoiDung");
                cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            }
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "nd.sMaNguoiDung ASC, nd.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int getDS_Count(String sMaNguoiDung = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Empty;
            if (sMaNguoiDung == "")
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungPhongBan, nd.sMaNguoiDung, nd.iID_MaPhongBan, dv.sTen, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan");
            else
            {
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungPhongBan, nd.sMaNguoiDung, nd.iID_MaPhongBan, dv.sTen, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan AND nd.sMaNguoiDung=@sMaNguoiDung");
                cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            }
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static string getPhongBan(string sMaNguoiDung)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT      dv.iID_MaPhongBan, dv.sTen
FROM          NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan AND nd.sMaNguoiDung=@sMaNguoiDung");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            DataTable dt = CommonFunction.dtData(cmd, "dv.sTen ASC", 1, 0);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    value += ", " + "<a href=\"/PhongBan/Edit?Code=" + Convert.ToString(dr["iID_MaPhongBan"]) + "\">" + Convert.ToString(dr["sTen"]) + "</a>";
                }
            }
            cmd.Dispose();
            if (dt != null) dt.Dispose();
            if (value.Length > 1) value = value.Substring(1, value.Length - 1);
            return value;
        }

        public static string getMaPhongBan_NguoiDung(string sMaNguoiDung)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT      dv.iID_MaPhongBan
FROM          NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan AND nd.sMaNguoiDung=@sMaNguoiDung");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            value = Connection.GetValueString(cmd, "");
            return value;
        }
        public static string getTenPhongBan_NguoiDung(string sMaNguoiDung)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT      dv.sKyHieu
FROM          NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan AND nd.sMaNguoiDung=@sMaNguoiDung");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            value = Connection.GetValueString(cmd, "");
            return value;
        }
        public static string getMoTaPhongBan_NguoiDung(string sMaNguoiDung)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT      dv.sKyHieu
FROM          NS_NguoiDung_PhongBan AS nd, NS_PhongBan AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaPhongBan = dv.iID_MaPhongBan AND nd.sMaNguoiDung=@sMaNguoiDung");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            value = Connection.GetValueString(cmd, "");
            return value;
        }
        public static DataTable getPhongBanByNguoiDung(String sMaNguoiDung, String MaPhongBan)
        {
            DataTable vR;
            String SQL = String.Format("SELECT iID_MaNguoiDungPhongBan FROM NS_NguoiDung_PhongBan WHERE iTrangThai=1 AND sMaNguoiDung = @sMaNguoiDung AND iID_MaPhongBan=@MaPhongBan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }


        public static DataTable getDetail(int iID_MaNguoiDungPhongBan)
        {
            string sql = "SELECT * FROM NS_NguoiDung_PhongBan WHERE iID_MaNguoiDungPhongBan=@iID_MaNguoiDungPhongBan";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaNguoiDungPhongBan", iID_MaNguoiDungPhongBan);
            DataTable vR = Connection.GetDataTable(cmd);
            return vR;
        }
        public static int getList_Count1(String MaNguoiDung = "", String MaPhongBan = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";

            String SQL = String.Format("SELECT COUNT (sMaNguoiDung) FROM(SELECT DISTINCT sMaNguoiDung FROM NS_NguoiDung_PhongBan WHERE {0})AS A", DK);
            if (MaPhongBan != "")
            {
                SQL += " AND iID_MaPhongBan= @MaPhongBan)";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            if (MaNguoiDung != "")
            {
                SQL += " AND sMaNguoiDung like N'%' + @sID_MaNguoiDung + '%'";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
            }
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Tra ve ma phong ban cua nguoi dung
        /// </summary>
        /// <param name="sMaNguoiDung">ma nguoi dung</param>
        /// <returns></returns>
        public static String getiID_MaPhongBanBysMaNguoiDung(String sMaNguoiDung)
        {
            String vR = "";
            String SQL = String.Format(@"SELECT TOP 1 iID_MaPhongBan FROM NS_NguoiDung_PhongBan WHERE iTrangThai=1 AND sMaNguoiDung=@sMaNguoiDung");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
                vR = dt.Rows[0]["iID_MaPhongBan"].ToString();
            else
                vR = Guid.Empty.ToString();
            return vR;
        }
    }
}