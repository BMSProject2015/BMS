using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace VIETTEL.Controllers
{
    public class NhaCungCapController : Controller
    {
        //
        // GET: /NhaCungCap/
        public string sViewPath = "~/Views/SanPham/MaVatTu/";
        [Authorize]
        public ActionResult Index(int? NhaCungCap_page, String sTenNhaCungCap, String sDiaChiNhaCungCap)
        {
            ViewData["NhaCungCap_page"] = NhaCungCap_page;
            ViewData["sTenNhaCungCap"] = sTenNhaCungCap;
            ViewData["sDiaChiNhaCungCap"] = sDiaChiNhaCungCap;
            return View(sViewPath + "Index.aspx");
        }

        public ActionResult Edit(String iID_MaNhaCungCap)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaNhaCungCap))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaNhaCungCap"] = iID_MaNhaCungCap;
            return View(sViewPath + "Edit.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaNhaCungCap, String iID_MaDonViDangNhap)
        {
            Bang bang = new Bang("DM_NhaCungCap");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);

            if (!bang.DuLieuMoi)
            {
                bang.GiaTriKhoa = iID_MaNhaCungCap;
            }
            else if (iID_MaDonViDangNhap != "-1")
            {
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonViDangNhap);
            }
            bang.Save();

            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            String sTenNhaCungCap = Request.Form[ParentID + "_sTenNhaCungCap"];
            String sDiaChiNhaCungCap = Request.Form[ParentID + "_sDiaChiNhaCungCap"];

            return RedirectToAction("Index", new { sTenNhaCungCap = sTenNhaCungCap, sDiaChiNhaCungCap = sDiaChiNhaCungCap });
        }

        [Authorize]
        public ActionResult ExportExcel(String ParentID, String SQL)
        {
            Export(SQL);
            return RedirectToAction("Index");
        }

        public void Export(String SQL)
        {
            SqlCommand cmd = new SqlCommand(SQL + " ORDER BY sTen");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i, j;
            Response.BufferOutput = false;
            Response.Charset = string.Empty;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            string attachment = "attachment; filename=DanhMucNhaCungCap.xls";
            Response.AddHeader("content-disposition", attachment);
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());

            string textOutput =
            "<table border='1' bordercolor='black' rules='all'>\r\n";
            Response.Write(textOutput);

            textOutput = "<tr height=17 style='height:12.75pt'>\r\n";
            string sTitle = "<th align='left'>{0}</th>";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "sTen")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Tên nhà cung cấp");
                if (dt.Columns[i].ColumnName == "sTenVietTat")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Tên viết tắt");
                if (dt.Columns[i].ColumnName == "sDiaChi")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Địa chỉ");
                if (dt.Columns[i].ColumnName == "sSoDienThoai")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Số điện thoại");
                if (dt.Columns[i].ColumnName == "sFax")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Fax");
                if (dt.Columns[i].ColumnName == "sEmail")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Email");
            }
            textOutput += "</tr>\r\n";
            Response.Write(textOutput);

            textOutput = string.Empty;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = dt.Rows[i];
                textOutput = "<tr height=17 style='height:12.75pt'>\r\n";

                for (j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    string dataRowText = dataRow[j].ToString().Trim();

                    dataRowText = dataRowText.Replace("\t", " ");
                    dataRowText = dataRowText.Replace("\r", string.Empty);
                    dataRowText = dataRowText.Replace("\n", " ");

                    string sContent = "<td align='left' style='mso-number-format:\\@;'>{0}</td>";

                    if (dt.Columns[j].ColumnName == "sTen")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sTenVietTat")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sDiaChi")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sSoDienThoai")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sFax")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sEmail")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                }
                textOutput += "</tr>\r\n";
                Response.Write(textOutput);
            }

            textOutput = "</table>\r\n";
            Response.Write(textOutput);

            Response.Flush();
            Response.Close();
            Response.End();
        }
    }
}
