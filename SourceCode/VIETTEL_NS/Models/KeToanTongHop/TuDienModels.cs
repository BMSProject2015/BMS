using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Models
{
    public class TuDienModels
    {
        /// <summary>
        /// lấy danh sách từ điển
        /// </summary>
        /// <param name="iID_MaTaiKhoanGoc"></param>
        /// <returns></returns>
        public static String get_TuDien(String iID_MaTaiKhoanGoc, String iNam)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT DISTINCT  TD.iID_MaTuDien, TD.iID_MaTaiKhoanGoc, TD.sLoai, TD.sNhom, TD.sNoiDung, TD.iID_MaTaiKhoanNo, TD.iID_MaTaiKhoanCo, TD.sTaiKhoanCo," +
                " TD.bPublic, TD.dNgayTao, TK1.iID_MaTaiKhoan + ' - ' + TK1.sTen AS TenTKNo," +
                " TK2.iID_MaTaiKhoan + ' - ' + TK2.sTen AS TenTKCo FROM KT_TuDien AS TD INNER  JOIN" +
                        " KT_TaiKhoan AS TK1 ON TD.iID_MaTaiKhoanNo = TK1.iID_MaTaiKhoan INNER  JOIN" +
                        " KT_TaiKhoan AS TK2 ON TD.iID_MaTaiKhoanCo = TK2.iID_MaTaiKhoan WHERE (TD.iTrangThai=1) AND (TK1.iNam = @iNam) AND (TK2.iNam = @iNam)";
            if (String.IsNullOrEmpty(iID_MaTaiKhoanGoc) == false && iID_MaTaiKhoanGoc != "" && iID_MaTaiKhoanGoc != Convert.ToString(Guid.Empty))
            {
                SQL += " AND TD.iID_MaTaiKhoanGoc=@iID_MaTaiKhoanGoc";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanGoc", iID_MaTaiKhoanGoc);
            }
            cmd.Parameters.AddWithValue("@iNam", iNam);
            SQL += " ORDER By TD.sNoiDung ASC";
            cmd.CommandText = SQL;         
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String strData = string.Empty;
            StringBuilder builder = new StringBuilder();
            if (dt != null)
            {
                builder.Append("<table class='mGrid'>");
                builder.Append("<tr>");
                builder.Append("<th style=\"width: 3%;\" align=\"center\">STT</th>");
                builder.Append("<th style=\"width: 8%;\" align=\"center\">Nhóm</th>");
                builder.Append("<th style=\"width: 8%;\" align=\"center\">Loại</th>");
                builder.Append("<th align=\"center\">Nội dung</th>");
                builder.Append("<th style=\"width: 20%;\" align=\"center\">Tài khoản nợ</th>");
                builder.Append("<th style=\"width: 20%;\" align=\"center\">Tài khoản có</th>");
                builder.Append("<th style=\"width: 5%;\" align=\"center\">Sửa</th>");
                builder.Append("<th style=\"width: 5%;\" align=\"center\">Xóa</th>");
                builder.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    int STT = i + 1;
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }

                    String sNhom = HamChung.ConvertToString(R["sNhom"]);
                    String sLoai = HamChung.ConvertToString(R["sLoai"]);
                    String sNoiDung = HamChung.ConvertToString(R["sNoiDung"]);
                    String DKNo = HamChung.ConvertToString(R["TenTKNo"]);
                    String DKCo = HamChung.ConvertToString(R["TenTKCo"]);

                    String urlEdit = "/TuDien/Edit?iID_MaTuDien=" + HamChung.ConvertToString(R["iID_MaTuDien"]) + "&iID_MaTaiKhoanGoc=" + HamChung.ConvertToString(R["iID_MaTaiKhoanGoc"]);
                    String urlDelete = "/TuDien/Delete?iID_MaTuDien=" + HamChung.ConvertToString(R["iID_MaTuDien"]);

                    builder.Append("<tr " + classtr + ">");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + STT + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(sNhom);
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(sLoai);
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(sNoiDung);
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(DKNo);
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(DKCo);
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\">{1}</a>", urlEdit, "<img src='../Content/Themes/images/edit.gif' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\">{1}</a>", urlDelete, "<img src='../Content/Themes/images/delete.gif' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }
                builder.Append("</table>");
                strData = builder.ToString();
            }
            return strData;
        }

        /// <summary>
        /// Lấy chi tiết thông tri
        /// </summary>
        /// <param name="iID_MaThongTri"></param>
        /// <returns></returns>
        public static DataTable getChiTiet(String iID_MaThongTri)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_TuDien WHERE iTrangThai=1 AND iID_MaTuDien=@iID_MaTuDien");
            cmd.Parameters.AddWithValue("@iID_MaTuDien", iID_MaThongTri);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}