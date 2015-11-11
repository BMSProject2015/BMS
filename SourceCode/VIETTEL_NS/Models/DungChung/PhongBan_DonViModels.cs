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
    public class PhongBan_DonViModels
    {
        public static DataTable getList(String MaPhongBan = "", String MaDonVi = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = "SELECT * FROM NS_PhongBan as qt WHERE qt.iTrangThai=1 AND exists (SELECT iID_MaPhongBanDonVi FROM NS_PhongBan_DonVi as ns WHERE iNamLamViec=@iNamLamViec AND ns.iTrangThai=1 AND ns.iID_MaPhongBan = qt.iID_MaPhongBan";
            if (MaDonVi != "")
            {
                SQL += " AND ns.iID_MaDonVi= @MaDonVi)";
                cmd.Parameters.AddWithValue("@MaDonVi", MaDonVi);
            }
            else SQL += ")";
            if (MaPhongBan != "")
            {
                SQL += " AND qt.iID_MaPhongBan= @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int getList_Count(String MaPhongBan = "", String MaDonVi = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iNamLamViec=@iNamLamViec ";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_PhongBan_DonVi WHERE {0}", DK);
            if (MaPhongBan != "")
            {
                SQL += " AND iID_MaPhongBan= @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
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
        public static DataTable getDS(String MaPhongBan = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Empty;
            if (MaPhongBan == "" )
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanDonVi, ns.iID_MaPhongBan, ns.iID_MaDonVi, pb.sTen AS TenPB, dv.sTen AS TenDV, ns.bPublic, ns.dNgayTao, ns.sID_MaNguoiDungTao 
FROM NS_PhongBan_DonVi AS ns, NS_PhongBan AS pb, NS_DonVi AS dv 
WHERE ns.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec AND ns.iTrangThai=1 AND pb.iTrangThai=1 AND dv.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.iID_MaDonVi = dv.iID_MaDonVi");
            else
            {
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanDonVi, ns.iID_MaPhongBan, ns.iID_MaDonVi, pb.sTen AS TenPB, dv.sTen AS TenDV, ns.bPublic, ns.dNgayTao, ns.sID_MaNguoiDungTao FROM 
NS_PhongBan_DonVi AS ns, NS_PhongBan AS pb, NS_DonVi AS dv 
WHERE ns.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec AND ns.iTrangThai=1 AND pb.iTrangThai=1 AND dv.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.iID_MaDonVi = dv.iID_MaDonVi AND ns.iID_MaPhongBan=@MaPhongBan");
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            vR = CommonFunction.dtData(cmd, "pb.sTen ASC, ns.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int getDS_Count(String MaPhongBan = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Empty;
            if (MaPhongBan == "" )
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanDonVi, ns.iID_MaPhongBan, ns.iID_MaDonVi, pb.sTen AS TenPB, dv.sTen AS TenDV, ns.bPublic, ns.dNgayTao FROM 
NS_PhongBan_DonVi AS ns, NS_PhongBan AS pb, NS_DonVi AS dv 
WHERE ns.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec AND ns.iTrangThai=1 AND pb.iTrangThai=1 AND dv.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.iID_MaDonVi = dv.iID_MaDonVi");
            else
            {
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanDonVi, ns.iID_MaPhongBan, ns.iID_MaDonVi, pb.sTen AS TenPB, dv.sTen AS TenDV, ns.bPublic, ns.dNgayTao FROM 
NS_PhongBan_DonVi AS ns, NS_PhongBan AS pb, NS_DonVi AS dv
WHERE ns.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec AND ns.iTrangThai=1 AND pb.iTrangThai=1 AND dv.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.iID_MaDonVi = dv.iID_MaDonVi AND ns.iID_MaPhongBan=@MaPhongBan");
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            SQL += " ORDER BY pb.sTen ASC, ns.dNgayTao DESC";
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Dispose();
            return vR;
        }
        public static string getDonVi(string MaPhongBan)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT dv.iID_MaDonVi, dv.sTen FROM NS_PhongBan_DonVi AS ns, NS_DonVi AS dv 
WHERE ns.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec AND ns.iTrangThai=1 AND dv.iTrangThai=1 AND 
ns.iID_MaDonVi = dv.iID_MaDonVi AND ns.iID_MaPhongBan=@MaPhongBan ORDER BY dv.sTen ASC");
            cmd.CommandText = SQL;

            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            //DataTable dt = CommonFunction.dtData(cmd, "dv.sTen ASC", 1, 0);
            DataTable dt = Connection.GetDataTable(cmd);
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
        public static DataTable getDonViByBQL(String MaBQL = "", Boolean All = false, String TieuDe = "")
        {

            DataTable vR = new DataTable();
            if (String.IsNullOrEmpty(MaBQL) == false && MaBQL != "")
            {
                SqlCommand cmd = new SqlCommand();
                String SQL = String.Format(@"SELECT dv.iID_MaDonVi, dv.iID_MaDonVi + ' - ' +  dv.sTen as sTen FROM NS_PhongBan_DonVi AS ns, NS_DonVi AS dv 
WHERE ns.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec AND ns.iTrangThai=1 AND dv.iTrangThai=1 AND 
ns.iID_MaDonVi = dv.iID_MaDonVi AND ns.iID_MaPhongBan=@MaPhongBan ORDER BY dv.iID_MaDonVi ASC");
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.Parameters.AddWithValue("@MaPhongBan", MaBQL);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd); cmd.Dispose();
            }
            if (All)
            {
                if (vR.Columns.Count == 0)
                {
                    vR.Columns.Add("iID_MaDonVi", typeof(string));
                    vR.Columns.Add("sTen", typeof(string));
                }
                DataRow R = vR.NewRow();
                R["iID_MaDonVi"] = -1;
                R["sTen"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }

        public static DataTable getDonViByPhongBan(String MaPhongBan, String MaDonVi)
        {
            DataTable vR;
            String SQL = String.Format("SELECT iID_MaPhongBanDonVi FROM NS_PhongBan_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaPhongBan = @MaPhongBan AND iID_MaDonVi=@MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@MaDonVi", MaDonVi);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }


        public static DataTable getDetail(int iID_MaPhongBanDonVi)
        {
            string sql = "SELECT * FROM NS_PhongBan_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaPhongBanDonVi=@iID_MaPhongBanDonVi";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaPhongBanDonVi", iID_MaPhongBanDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static String getPhongBanDonVi(String MaPhongBan)
        {
            String vR = "";
            String SQL = String.Format("SELECT iID_MaDonVi FROM NS_PhongBan_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND  iID_MaPhongBan = @MaPhongBan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vR += Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) + ",";
            }
            if (vR != "")
            {
                vR = vR.Remove(vR.Length - 1);
            }
            cmd.Dispose();
            return vR;
        }
        public static DataTable DS_PhongBan_DonVi(string sMaPhongBan, Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT DISTINCT  pb_dv.iID_MaDonVi, dv.sTen" +
                " FROM NS_PhongBan_DonVi AS pb_dv,NS_NguoiDung_PhongBan AS nd_pb, NS_PhongBan AS pb,  NS_DonVi AS dv" +
                " WHERE pb_dv.iNamLamViec=@iNamLamViec AND dv.iNamLamViec_DonVi=@iNamLamViec nd_pb.iID_MaPhongBan = pb.iID_MaPhongBan AND pb.iID_MaPhongBan = pb_dv.iID_MaPhongBan AND pb_dv.iID_MaDonVi = dv.iID_MaDonVi";
            if (String.IsNullOrEmpty(sMaPhongBan) == false && sMaPhongBan != "")
            {
                SQL += " AND pb_dv.iID_MaPhongBan=@sMaPhongBan";
                cmd.Parameters.AddWithValue("@sMaPhongBan", sMaPhongBan);
            }
            SQL += " ORDER BY dv.sTen";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
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
        public static int getList_Count1(String MaPhongBan = "", String MaDonVi = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iNamLamViec=@iNamLamViec ";

            String SQL = String.Format("SELECT COUNT(*) FROM (SELECT DISTINCT iID_MaPhongBan FROM NS_PhongBan_DonVi WHERE {0}) AS A ", DK);
            if (MaPhongBan != "")
            {
                SQL += " AND iID_MaPhongBan= @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
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