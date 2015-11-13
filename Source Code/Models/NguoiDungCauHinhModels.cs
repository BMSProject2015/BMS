using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using DomainModel.Abstract;
using DomainModel;
namespace VIETTEL.Models
{
    public class NguoiDungCauHinhModels
    {

        public static Object iNamLamViec
        {
            get;
            set;
        }
       
        public static String MaNguoiDung { get; set; }
        
        public static Boolean SuaCauHinh(String sID_MaNguoiDung, Object options)
        {
            Boolean vR = false;
            Bang bang = new Bang("DC_NguoiDungCauHinh");
            bang.MaNguoiDungSua = sID_MaNguoiDung;

            Boolean okUpdate = false;
            PropertyInfo[] properties = options.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                bang.CmdParams.Parameters.AddWithValue("@" + properties[i].Name, properties[i].GetValue(options, null));
                okUpdate = true;
            }
            if (okUpdate)
            {
                SqlCommand cmd = new SqlCommand("SELECT iID_MaNguoiDungCauHinh FROM DC_NguoiDungCauHinh WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sID_MaNguoiDung);
                int iID_MaNguoiDungCauHinh = Convert.ToInt32( Connection.GetValue(cmd, -1));
                cmd.Dispose();
                if (iID_MaNguoiDungCauHinh >= 0)
                {
                    bang.DuLieuMoi = false;
                    bang.GiaTriKhoa = iID_MaNguoiDungCauHinh;
                }
                bang.Save();
                vR = true;
                iNamLamViec=LayCauHinhChiTiet(sID_MaNguoiDung,"iNamLamViec");
            }
            return vR;
        }

        public static DataTable LayCauHinh(String sID_MaNguoiDung)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DC_NguoiDungCauHinh WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sID_MaNguoiDung);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static Object LayCauHinhChiTiet(String sID_MaNguoiDung, String TenTruong)
        {
            Object vR = null;
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DC_NguoiDungCauHinh WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sID_MaNguoiDung);
            dt = Connection.GetDataTable(cmd);
            if (dt!=null && dt.Rows.Count > 0)
            {
                if (dt.Columns.IndexOf(TenTruong) >= 0 && dt.Rows.Count > 0)
                {
                    vR = dt.Rows[0][TenTruong];
                }
                dt.Dispose();
            }
            cmd.Dispose();
         
            return vR;
        }
        public static String ThangTinhSoDu_TKChiTiet(String iNamLamViec)
        {
            String vR = "";
            String SQLTK = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sKyHieu=0";
            SqlCommand cmd = new SqlCommand(SQLTK);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            vR = Connection.GetValueString(cmd, "0");
            cmd.Dispose();
            return vR;
        }
      
    }
}