using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Text;
namespace VIETTEL.Models
{
    public class KTCS_CauHinhHachToanModels
    {
        public static NameValueCollection LayThongTinCauHinhHachToan(String iID_MaKyHieuHachToanChiTiet)
        {
            NameValueCollection data = new NameValueCollection();

            DataTable dt = Get_dtChiTietCauHinhHachToan(iID_MaKyHieuHachToanChiTiet);
            String ColName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ColName = dt.Columns[i].ColumnName;
                    data.Add(ColName, Convert.ToString(dt.Rows[0][ColName]));
                }
            }
            return data;
        }

        public static DataTable Get_dtDSCauHinhHachToan()
        {
            String SQL = "SELECT * FROM KTCS_KyHieuHachToanChiTiet WHERE iTrangThai=1 ORDER BY iID_MaKyHieuHachToan";
            return Connection.GetDataTable(SQL);
        }

        public static DataTable Get_dtChiTietCauHinhHachToan(String iID_MaKyHieuHachToanChiTiet)
        {
            DataTable dt;
            String SQL = "SELECT * FROM KTCS_KyHieuHachToanChiTiet WHERE iTrangThai=1 AND iID_MaKyHieuHachToanChiTiet=@iID_MaKyHieuHachToanChiTiet";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaKyHieuHachToanChiTiet", iID_MaKyHieuHachToanChiTiet);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtDSKyHieu()
        {
            DataTable vR;
            String SQL = "SELECT DISTINCT iID_MaKyHieuHachToan FROM KTCS_KyHieuHachToanChiTiet WHERE iTrangThai = 1 ORDER BY iID_MaKyHieuHachToan";
            vR = Connection.GetDataTable(SQL);
            return vR;
        }

        public static DataTable Get_dtCauHinhHachToanChiTiet(String iID_MaKyHieuHachToan)
        {
            String SQL = "SELECT * FROM KTCS_KyHieuHachToanChiTiet WHERE iID_MaKyHieuHachToan=@iID_MaKyHieuHachToan";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaKyHieuHachToan", iID_MaKyHieuHachToan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static String Get_MaCauHinhHachToan(String iID_MaChungTuChiTiet)
        {
            String SQL = "SELECT iID_MaKyHieuHachToan FROM KTCS_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
            
        }

        public static String Get_DSHachToan(String ParentID, String iID_MaKyHieuHachToan)
        {
            DataTable dt=Get_dtCauHinhHachToanChiTiet(iID_MaKyHieuHachToan);
            String iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co,KyHieuTruongTien="";
            StringBuilder stb = new StringBuilder();
            NameValueCollection data = new NameValueCollection();
            stb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"table_form2\" width=\"100%\">");
            stb.Append("<tr>");
            stb.Append("<th>");
            stb.Append("Tài khoản nợ");
            stb.Append("</th>");
            stb.Append("<th>");
            stb.Append("Tài khoản có");
            stb.Append("</th>");
            stb.Append("<th>");
            stb.Append("Số tiền");
            stb.Append("</th>");
            stb.Append("</tr>");

            for (int i = 0; i<dt.Rows.Count; i++)
            {
                iID_MaTaiKhoan_Co=Convert.ToString(dt.Rows[i]["iID_MaTaiKhoan_Co"]);
                iID_MaTaiKhoan_No=Convert.ToString(dt.Rows[i]["iID_MaTaiKhoan_No"]);
                KyHieuTruongTien="r"+Convert.ToString(dt.Rows[i]["sGiaTri"]);

                stb.Append("<tr>");
                stb.Append("<td>");
                stb.Append( MyHtmlHelper.TextBox(ParentID,iID_MaTaiKhoan_No,"iID_MaTaiKhoan_No",""));
                stb.Append("</td>");
                stb.Append("<td>");
                stb.Append(MyHtmlHelper.TextBox(ParentID, iID_MaTaiKhoan_No, "iID_MaTaiKhoan_No", ""));
                stb.Append("</td>");
                stb.Append("<td>");
                stb.Append(MyHtmlHelper.TextBox(ParentID, data, KyHieuTruongTien, ""));
                stb.Append("</td>");
                stb.Append("</tr>");
            }
            stb.Append("</table>");
            return stb.ToString();
        }
    }
}