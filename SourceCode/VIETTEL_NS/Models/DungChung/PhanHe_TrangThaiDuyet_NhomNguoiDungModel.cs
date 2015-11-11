using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class PhanHe_TrangThaiDuyet_NhomNguoiDungModel
    {
       
        //Table PhanHe_TrangThaiDuyet_NhomNguoiDung
        public static DataTable NS_PhanHe_TrangThaiDuyet_NhomNguoiDung(String MaPhanHe,String MaNhomNguoiDung)
        {
            String DK = "1=1";
            String SQL = "";
            String SQL1 = "";
            DataTable dt; 
            SqlCommand cmd = new SqlCommand();
            
            if (!String.IsNullOrEmpty(MaPhanHe) && MaPhanHe!="-1")
            {
                if (!String.IsNullOrEmpty(DK)) DK += " AND ";
                DK += "iID_MaPhanHe=@iID_MaPhanHe";
                cmd.Parameters.AddWithValue("@iID_MaPhanHe", MaPhanHe);
            }
            if (!String.IsNullOrEmpty(MaNhomNguoiDung))
            {
                if (!String.IsNullOrEmpty(DK)) DK += " AND ";
                DK += "iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung";
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
            }
            SQL1 = "SELECT * FROM NS_PhanHe_TrangThaiDuyet_NhomNguoiDung  WHERE {0} ORDER BY iID_MaPhanHe,iID_MaPhanHe_TrangThaiDuyet_Xem";
            SQL = String.Format(SQL1, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            dt.Dispose();
            return dt;
        }

        public static DataTable DT_TrangThaiDuyet(Boolean ThemDongTieuDe, String sDongTieuDe,String MaPhanHe)
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaTrangThaiDuyet,sTen FROM NS_PhanHe_TrangThaiDuyet Where iID_MaPhanHe=@iID_MaPhanHe");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", MaPhanHe);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaTrangThaiDuyet"] = -1;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            dt.Dispose();
            return dt;
        }
        public static DataTable GetRow_PhanHe_TrangThaiDuyet_NhomNguoiDung(String MaPhanHe_TrangThaiDuyet_Xem)
        {
            DataTable dt;
            String SQL = "SELECT * FROM NS_PhanHe_TrangThaiDuyet_NhomNguoiDung WHERE iID_MaPhanHe_TrangThaiDuyet_Xem=@iID_MaPhanHe_TrangThaiDuyet_Xem";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe_TrangThaiDuyet_Xem", MaPhanHe_TrangThaiDuyet_Xem);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}