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
    public class MucLucNganSach_NganhModels
    {
        public static DataTable Get_dtMucLucNganSach_Nganh(String iID)
        {
            String SQL = "SELECT * FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID=@iID ";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID", iID);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable LayDanhSachMLNS_Nganh()
        {
            String SQL =String.Format(@"SELECT DISTINCT sNG FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sNG<>''
                                        ORDER BY sNG ");
            SqlCommand cmd = new SqlCommand(SQL);           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String LayDanhSachNganh(String sTenKhoa)
        {
            String SQL = String.Format(@"SELECT iID_MaDanhMuc,sTenKhoa, DC_DanhMuc.sTen
                                        FROM DC_LoaiDanhMuc 
                                        INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                                        WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'Nganh' AND sTenKhoa=@sTenKhoa ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa",sTenKhoa);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String s = "";
            if (dt.Rows.Count > 0)
                s = dt.Rows[0]["sTen"].ToString();
            return s;
        }
        public static DataTable LayDanhSachNganh(int Trang = 1, int SoBanGhi = 0)
        {
            String SQL = String.Format(@"SELECT * FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1");
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = CommonFunction.dtData(cmd, "iID_MaNganh", Trang, SoBanGhi);
            cmd.Dispose();
            return dt;
        }
        public static int  Count_DanhSachNganh()
        {
             int vR;
            String SQL = String.Format(@"SELECT COUNT(*)
                                        FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1");
            SqlCommand cmd = new SqlCommand(SQL);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static bool CheckTonTaiMaNganh(String iID_MaNganh)
        {
            int vR;
            String SQL = String.Format(@"SELECT COUNT(iID_MaNganh)
                                        FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@iID_MaNganh");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNganh", iID_MaNganh);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (vR > 0) return true;
            return false;
        }
        public static DataTable dtDanhSachMLNS_Nganh(String iID_MaNganh)
        {
            String SQL = String.Format(@"SELECT iID_MaNganh,iID_MaNganhMLNS
                                        FROM NS_MucLucNganSach_Nganh
                                        WHERE iID_MaNganh=@iID_MaNganh");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNganh", iID_MaNganh);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String DanhSachMLNS_Nganh(String iID_MaNganh)
        {
            String value = "";
            DataTable dt = dtDanhSachMLNS_Nganh(iID_MaNganh);
            for (int i = 0; i < dt.Rows.Count;i++)
            {
                value += dt.Rows[i]["iID_MaNganhMLNS"] + ",";
            }
            if (value.Length>1)
            {
                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }
    }
}