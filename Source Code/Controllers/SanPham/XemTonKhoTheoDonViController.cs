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
    public class XemTonKhoTheoDonViController : Controller
    {
        //
        // GET: /XemTonKhoTheoDonVi/
        public string sViewPath = "~/Views/SanPham/XemTonKhoTheoDonVi/";
        public ActionResult Index(int? XemTonKhoTheoDonVi_page, String iID_MaDonVi)
        {
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            return View(sViewPath + "Index.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi });
        }

        [Authorize]
        public ActionResult ExportExcel(String ParentID, String iID_MaDonVi)
        {
            Export(iID_MaDonVi);
            return RedirectToAction("Search", new { ParentID = ParentID });
        }

        public void Export(String iID_MaDonVi)
        {
            String SQL = "";
            String DK = "";
            DataTable dtDonVi_TonKho = null;
            SqlCommand cmd;
            if (iID_MaDonVi == "-1")
            {
                SQL = "SELECT iID_MaVatTu, sMaVatTu, sTen, rSoLuongTonKho, iDM_MaDonViTinh, dNgayCapNhatTonKho FROM DM_VatTu ";
            }
            else if (iID_MaDonVi != "")
            {
                DK = " WHERE iID_MaDonVi = " + iID_MaDonVi;
                SQL = "SELECT iID_MaVatTu, sMaVatTu, sTen, rSoLuongTonKho, iDM_MaDonViTinh, dNgayCapNhatTonKho FROM DM_VatTu " +
                      "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_DonVi_TonKho " + DK + ")";

                cmd = new SqlCommand("SELECT iID_MaVatTu, rSoLuongTonKho, dNgaySua AS dNgayCapNhatTonKho FROM DM_DonVi_TonKho" + DK);
                dtDonVi_TonKho = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }

            cmd = new SqlCommand(SQL + " ORDER BY sMaVatTu");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt == null) return;

            cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                 "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                             "FROM DC_LoaiDanhMuc " +
                                                             "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
            DataTable dtDonViTinh = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i, j, z;
            Response.BufferOutput = false;
            Response.Charset = string.Empty;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            string attachment = "attachment; filename=TonKhoDonVi.xls";
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
                if (dt.Columns[i].ColumnName == "sMaVatTu")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Mã vật tư");
                if (dt.Columns[i].ColumnName == "sTen")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Tên");
                if (dt.Columns[i].ColumnName == "rSoLuongTonKho")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Số lượng tồn kho");
                if (dt.Columns[i].ColumnName == "iDM_MaDonViTinh")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Đơn vị tính");
                if (dt.Columns[i].ColumnName == "dNgayCapNhatTonKho")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Ngày cập nhật tồn kho");
            }
            textOutput += "</tr>\r\n";
            Response.Write(textOutput);

            textOutput = string.Empty;

            DateTime dNgayCapNhatTonKho = DateTime.Now;
            String DonViTinh = "";
            String SoLuongTonKho = "0";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = dt.Rows[i];
                textOutput = "<tr height=17 style='height:12.75pt'>\r\n";

                if (iID_MaDonVi == "-1")
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["rSoLuongTonKho"])))
                        SoLuongTonKho = CommonFunction.DinhDangSo(dt.Rows[i]["rSoLuongTonKho"]);
                    else
                        SoLuongTonKho = "0";
                    if (!(dt.Rows[i]["dNgayCapNhatTonKho"] is DBNull))
                        dNgayCapNhatTonKho = Convert.ToDateTime(dt.Rows[i]["dNgayCapNhatTonKho"]);
                }
                else
                {
                    if (dtDonVi_TonKho != null)
                    {
                        for (z = 0; z < dtDonVi_TonKho.Rows.Count; z++)
                        {
                            if (Convert.ToString(dt.Rows[i]["iID_MaVatTu"]) == Convert.ToString(dtDonVi_TonKho.Rows[z]["iID_MaVatTu"]))
                            {
                                SoLuongTonKho = CommonFunction.DinhDangSo(dtDonVi_TonKho.Rows[z]["rSoLuongTonKho"]);
                                dNgayCapNhatTonKho = Convert.ToDateTime(dtDonVi_TonKho.Rows[z]["dNgayCapNhatTonKho"]);
                                break;
                            }
                        }
                    }
                }
                for (j = 0; j < dtDonViTinh.Rows.Count; j++)
                {
                    if (Convert.ToString(dt.Rows[i]["iDM_MaDonViTinh"]) == Convert.ToString(dtDonViTinh.Rows[j]["iID_MaDanhMuc"]))
                    {
                        DonViTinh = Convert.ToString(dtDonViTinh.Rows[j]["sTen"]);
                        break;
                    }
                }

                for (j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    string dataRowText = dataRow[j].ToString().Trim();

                    dataRowText = dataRowText.Replace("\t", " ");
                    dataRowText = dataRowText.Replace("\r", string.Empty);
                    dataRowText = dataRowText.Replace("\n", " ");

                    string sContent = "<td align='left' style='mso-number-format:\\@;'>{0}</td>";

                    if (dt.Columns[j].ColumnName == "sMaVatTu")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sTen")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "rSoLuongTonKho")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, SoLuongTonKho);
                    if (dt.Columns[j].ColumnName == "iDM_MaDonViTinh")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, DonViTinh);
                    if (dt.Columns[j].ColumnName == "dNgayCapNhatTonKho")
                    {
                        try
                        {
                            textOutput += string.Format(CultureInfo.InvariantCulture, sContent, String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho));
                        }
                        catch
                        {
                            textOutput += string.Format(CultureInfo.InvariantCulture, sContent, string.Empty);
                        }
                    }
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
