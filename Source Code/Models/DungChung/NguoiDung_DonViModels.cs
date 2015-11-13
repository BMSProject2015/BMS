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
    public class NguoiDung_DonViModels
    {
        public static DataTable getList(String MaNguoiDung = "", String MaDonVi = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = "SELECT * FROM QT_NguoiDung as qt WHERE  exists (SELECT iID_MaNguoiDungDonVi FROM NS_NguoiDung_DonVi as ns WHERE ns.iTrangThai=1 AND ns.iNamLamViec=@iNamLamViec AND ns.sMaNguoiDung=qt.sID_MaNguoiDung";
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (MaDonVi != "")
            {
                SQL += " AND ns.iID_MaDonVi= @MaDonVi)";
                cmd.Parameters.AddWithValue("@MaDonVi", MaDonVi);
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

        public static int getList_Count(String MaNguoiDung = "", String MaDonVi = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 ";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {0}", DK);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (MaNguoiDung != "")
            {
                SQL += " AND sMaNguoiDung like N'%' + @sID_MaNguoiDung + '%'";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
            }
            if (MaDonVi != "")
            {
                SQL += " AND iID_MaDonVi= @MaDonVi";
                cmd.Parameters.AddWithValue("@MaDonVi", MaDonVi);
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
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungDonVi, nd.sMaNguoiDung, nd.iID_MaDonVi, dv.sTen, nd.bPublic, nd.dNgayTao, 
                                    nd.sID_MaNguoiDungTao FROM NS_NguoiDung_DonVi AS nd, NS_DonVi AS dv 
WHERE nd.iTrangThai=1 AND nd.iNamLamviec=@iNamLamviec AND dv.iNamLamviec_DonVi=@iNamLamviec AND nd.iID_MaDonVi = dv.iID_MaDonVi");
            else
            {
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungDonVi, nd.sMaNguoiDung, nd.iID_MaDonVi, dv.sTen,nd.iID_MaDonVi+'-'+dv.sTen as TenHT, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_DonVi AS nd, NS_DonVi AS dv WHERE nd.iTrangThai=1  AND nd.iNamLamviec=@iNamLamviec AND dv.iNamLamviec_DonVi=@iNamLamviec AND nd.iID_MaDonVi = dv.iID_MaDonVi AND nd.sMaNguoiDung=@sMaNguoiDung");
                cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            vR = CommonFunction.dtData(cmd, "nd.iID_MaDonVi ASC, nd.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int getDS_Count(String sMaNguoiDung = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();  
            String SQL = String.Empty;
            if (sMaNguoiDung == "")
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungDonVi, nd.sMaNguoiDung, nd.iID_MaDonVi, dv.sTen, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_DonVi AS nd, NS_DonVi AS dv WHERE nd.iTrangThai=1 AND nd.iNamLamviec=@iNamLamviec AND dv.iNamLamviec_DonVi=@iNamLamviec AND nd.iID_MaDonVi = dv.iID_MaDonVi");
            else
            {
                SQL = String.Format(@"SELECT nd.iID_MaNguoiDungDonVi, nd.sMaNguoiDung, nd.iID_MaDonVi, dv.sTen, nd.bPublic, nd.dNgayTao, 
nd.sID_MaNguoiDungTao FROM NS_NguoiDung_DonVi AS nd, NS_DonVi AS dv WHERE nd.iTrangThai=1 AND nd.iID_MaDonVi = dv.iID_MaDonVi AND nd.sMaNguoiDung=@sMaNguoiDung");
                cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static string getDonVi(string sMaNguoiDung)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT      dv.iID_MaDonVi, dv.sTen
FROM          NS_NguoiDung_DonVi AS nd, NS_DonVi AS dv WHERE nd.iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iNamLamViec_DonVi=@iNamLamViec AND nd.iID_MaDonVi = dv.iID_MaDonVi AND nd.sMaNguoiDung=@sMaNguoiDung");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            DataTable dt = CommonFunction.dtData(cmd, "dv.sTen ASC", 1, 0);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    value += ", " + "<a href=\"/DonVi/Edit?MaDonVi=" + Convert.ToString(dr["iID_MaDonVi"]) + "\">" + Convert.ToString(dr["sTen"]) + "</a>";
                }               
            }
            cmd.Dispose();
            if (dt != null) dt.Dispose();
            if (value.Length > 1) value = value.Substring(1, value.Length - 1);
            return value;
        }

        public static String getDonViByNguoiDung(String sMaNguoiDung)
        {
            String vR="";
            String SQL = String.Format("SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND  sMaNguoiDung = @sMaNguoiDung");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vR += Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) + ",";
            }
            if (vR != "")
            {
                vR=vR.Remove(vR.Length-1);
            }
            return vR;
        }
        public static String getDonViByNguoiDung_1(String sMaNguoiDung)
        {
            String vR = "";
            String SQL = String.Format("SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND  sMaNguoiDung = @sMaNguoiDung");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vR +="'"+Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) + "',";
            }
            if (vR != "")
            {
                vR = vR.Remove(vR.Length - 1);
            }
            return vR;
        }

        public static DataTable getDetail(int iID_MaNguoiDungDonVi)
        {
            string sql = "SELECT * FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaNguoiDungDonVi=@iID_MaNguoiDungDonVi";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaNguoiDungDonVi", iID_MaNguoiDungDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable vR = Connection.GetDataTable(cmd);
            return vR;
        }
        public static DataTable DS_NguoiDung_DonVi(string sMaNguoiDung, Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT DISTINCT  pb_dv.iID_MaDonVi, pb_dv.iID_MaDonVi + ' - ' + dv.sTen as sTen,pb_dv.iID_MaDonVi + ' - ' + dv.sTen as TenHT" +
                " FROM NS_NguoiDung_PhongBan AS nd_pb, NS_PhongBan AS pb, NS_PhongBan_DonVi AS pb_dv, NS_DonVi AS dv" +
                " WHERE iNamLamViec_DonVi=@iNamLamViec AND iNamLamViec=@iNamLamViec AND nd_pb.iID_MaPhongBan = pb.iID_MaPhongBan AND pb.iID_MaPhongBan = pb_dv.iID_MaPhongBan AND pb_dv.iID_MaDonVi = dv.iID_MaDonVi";
            if (String.IsNullOrEmpty(sMaNguoiDung) == false && sMaNguoiDung != "")
            {
                SQL += " AND nd_pb.sMaNguoiDung=@sMaNguoiDung";
                cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            }
            SQL += " ORDER BY pb_dv.iID_MaDonVi";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaNguoiDung));
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = string.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static String getPhongBanByNguoiDung(String sMaNguoiDung)
        {
            String vR = "";
            String SQL = String.Format("SELECT iID_MaPhongBan FROM NS_NguoiDung_PhongBan WHERE iTrangThai=1 AND  sMaNguoiDung = @sMaNguoiDung");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vR += Convert.ToString(dt.Rows[i]["iID_MaPhongBan"]) + ",";
            }
            if (vR != "")
            {
                vR = vR.Remove(vR.Length - 1);
            }
            return vR;
        }
        public static int getList_Count1(String MaNguoiDung = "", String MaDonVi = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 ";

           // String SQL = String.Format("SELECT COUNT(*) FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND {0}", DK);
            String SQL = String.Format("SELECT COUNT (sMaNguoiDung) from(SELECT DISTINCT sMaNguoiDung FROM  NS_NguoiDung_DonVi  WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND {0}) as a", DK);
                
            if (MaNguoiDung != "")
            {
                SQL += " AND sMaNguoiDung like N'%' + @sID_MaNguoiDung + '%'";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
            }
            if (MaDonVi != "")
            {
                SQL += " AND iID_MaDonVi= @MaDonVi";
                cmd.Parameters.AddWithValue("@MaDonVi", MaDonVi);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}