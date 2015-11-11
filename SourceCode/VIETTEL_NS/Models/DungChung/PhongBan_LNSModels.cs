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
    public class PhongBan_LNSModels
    {
        public static DataTable getList(String MaPhongBan = "", String MaLNS = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = "SELECT * FROM NS_PhongBan as qt WHERE exists (SELECT iID_MaPhongBanLoaiNganSach FROM NS_PhongBan_LoaiNganSach as ns WHERE ns.iTrangThai=1 AND ns.iID_MaPhongBan = qt.iID_MaPhongBan";
            if (MaLNS != "")
            {
                SQL += " AND ns.sLNS= @sLNS)";
                cmd.Parameters.AddWithValue("@sLNS", MaLNS);
            }
            else SQL += ")";
            if (MaPhongBan != "")
            {
                SQL += " AND qt.iID_MaPhongBan= @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }

            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int getList_Count(String MaPhongBan = "", String MaLNS = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_PhongBan_LoaiNganSach WHERE {0}", DK);
            if (MaPhongBan != "")
            {
                SQL += " AND iID_MaPhongBan= @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            if (MaLNS != "")
            {
                SQL += " AND sLNS= @sLNS";
                cmd.Parameters.AddWithValue("@sLNS", MaLNS);
            }
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
            if (MaPhongBan == "")
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanLoaiNganSach, pb.sTen, ns.sLNS, ns.bPublic, ns.iID_MaPhongBan,ns.dNgayTao, ns.sLNS + ' - ' + lns.sTen AS LNS, ns.sID_MaNguoiDungTao FROM NS_PhongBan_LoaiNganSach AS ns, NS_PhongBan AS pb, NS_LoaiNganSach AS lns WHERE ns.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.sLNS = lns.sLNS");
            else
            {
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanLoaiNganSach, pb.sTen, ns.sLNS, ns.bPublic, ns.iID_MaPhongBan,
ns.dNgayTao, ns.sLNS + ' - ' + lns.sTen AS LNS, ns.sID_MaNguoiDungTao FROM NS_PhongBan_LoaiNganSach AS ns, NS_PhongBan AS pb, NS_LoaiNganSach AS lns WHERE ns.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.sLNS = lns.sLNS AND ns.iID_MaPhongBan=@MaPhongBan");
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "pb.sTen ASC, ns.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int getDS_Count(String MaPhongBan = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Empty;
            if (MaPhongBan == "")
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanLoaiNganSach, pb.sTen, ns.sLNS, ns.bPublic, ns.iID_MaPhongBan,
ns.dNgayTao, ns.sID_MaNguoiDungTao FROM NS_PhongBan_LoaiNganSach AS ns, NS_PhongBan AS pb WHERE ns.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan");
            else
            {
                SQL = String.Format(@"SELECT ns.iID_MaPhongBanLoaiNganSach, pb.sTen, ns.sLNS, ns.bPublic, ns.iID_MaPhongBan,
ns.dNgayTao, ns.sID_MaNguoiDungTao FROM NS_PhongBan_LoaiNganSach AS ns, NS_PhongBan AS pb WHERE ns.iTrangThai=1 AND ns.iID_MaPhongBan = pb.iID_MaPhongBan AND ns.iID_MaPhongBan=@MaPhongBan");
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            SQL += " ORDER BY pb.sTen ASC, ns.dNgayTao DESC";
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static string getLNS(string MaPhongBan)
        {
            string value = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sLNS+' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE sLNS IN (SELECT sLNS";
            SQL += " FROM NS_PhongBan_LoaiNganSach";
            SQL += " WHERE iTrangThai=1 AND ";
            SQL += " iID_MaPhongBan=@MaPhongBan)";
            SQL += "AND sL=''";
            SQL += "ORDER By sLNS";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            //DataTable dt = CommonFunction.dtData(cmd, "dv.sTen ASC", 1, 0);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //value += ", " + "<a href=\"/DonVi/Edit?MaDonVi=" + Convert.ToString(dr["iID_MaDonVi"]) + "\">" + Convert.ToString(dr["sTen"]) + "</a>";
                    value += ", " + Convert.ToString(dr["TenHT"]);
                }
            }
            cmd.Dispose();
            if (dt != null) dt.Dispose();
            if (value.Length > 1) value = value.Substring(1, value.Length - 1);
            return value;
        }

        public static DataTable getLNSByPhongBan(String MaPhongBan, String LNS)
        {
            DataTable vR;
            String SQL = String.Format("SELECT iID_MaPhongBanLoaiNganSach FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND iID_MaPhongBan = @MaPhongBan AND sLNS=@LNS");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@LNS", LNS);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }

      

        public static DataTable getDetail(int iID_MaPhongBanLoaiNganSach)
        {
            string sql = "SELECT * FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND iID_MaPhongBanLoaiNganSach=@iID_MaPhongBanLoaiNganSach";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaPhongBanLoaiNganSach", iID_MaPhongBanLoaiNganSach);
            DataTable vR = Connection.GetDataTable(cmd);
            return vR;
        }
        public static DataTable NS_LoaiNganSach(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sTen AS TenHT FROM NS_LoaiNganSach WHERE iTrangThai=1 ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = " ";
                R["TenHT"] = "-- Chọn Loại ngân sách --";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static String getPhongBanLNS(String MaPhongBan)
        {
            String vR = "";
            String SQL = String.Format("SELECT sLNS FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND  iID_MaPhongBan = @MaPhongBan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vR += Convert.ToString(dt.Rows[i]["sLNS"]) + ",";
            }
            if (vR != "")
            {
                vR = vR.Remove(vR.Length - 1);
            }
            return vR;
        }
        public static DataTable getDtLNSByPhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            SqlCommand cmd;
            String SQL = "";
            if (String.IsNullOrEmpty(iID_MaPhongBan))
                iID_MaPhongBan = Guid.Empty.ToString();
            SQL = String.Format(@"SELECT a.sLNS,sMoTa,a.sLNS+' - '+sMoTa as TenHT FROM 
(
SELECT * FROM NS_PhongBan_LoaiNganSach 
WHERE iTrangThai=1 AND  iID_MaPhongBan = @iID_MaPhongBan) as a
INNER JOIN
(SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND LEN(sLNS)=7 AND sL='') as b
ON a.sLNS=b.sLNS
");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public static int getList_Count1(String MaPhongBan = "", String MaLNS = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";

            String SQL = String.Format("SELECT COUNT (iID_MaPhongBan) FROM(SELECT DISTINCT iID_MaPhongBan FROM NS_PhongBan_LoaiNganSach WHERE {0})AS A", DK);
            if (MaPhongBan != "")
            {
                SQL += " AND iID_MaPhongBan= @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            }
            if (MaLNS != "")
            {
                SQL += " AND sLNS= @sLNS";
                cmd.Parameters.AddWithValue("@sLNS", MaLNS);
            }
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}