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
using VIETTEL.Models;
//using Excel = Microsoft.Office.Interop.Excel;

using System.Globalization;
using System.Text;

namespace VIETTEL.Controllers
{
    public class SanPham_DanhMucController : Controller
    {
        //
        // GET: /SanPham_DanhMuc/
        public string sViewPath = "~/Views/SanPham/DanhMuc/";
        public ActionResult Index(int? DanhMuc_page, String LoaiDanhMuc, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh, String iDM_MaNhomPhu, String sTen)
        {
            ViewData["DanhMuc_page"] = DanhMuc_page;
            ViewData["LoaiDanhMuc"] = LoaiDanhMuc;
            ViewData["iDM_MaNhomLoaiVatTu"] = iDM_MaNhomLoaiVatTu;
            ViewData["iDM_MaNhomChinh"] = iDM_MaNhomChinh;
            ViewData["iDM_MaNhomPhu"] = iDM_MaNhomPhu;
            ViewData["sTen"] = sTen;
            return View(sViewPath + "Index.aspx");
        }

        public ActionResult Edit(int? TimKiemVatTu_page, String MaDanhMuc, String LoaiDanhMuc, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh, String iDM_MaNhomPhu, String Loi,
            String Searchid, String sMaVatTu_Search, String sTen_Search, String sTenGoc_Search, String sQuyCach_Search, String cbsMaVatTu_Search,
            String cbsTen_Search, String cbsTenGoc_Search, String cbsQuyCach_Search, String MaNhomLoaiVatTu_Search, String MaNhomChinh_Search,
            String MaNhomPhu_Search, String MaChiTietVatTu_Search, String MaXuatXu_Search, String iTrangThai_Search)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(MaDanhMuc))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["LoaiDanhMuc"] = LoaiDanhMuc;
            ViewData["iDM_MaNhomLoaiVatTu"] = iDM_MaNhomLoaiVatTu;
            ViewData["iDM_MaNhomChinh"] = iDM_MaNhomChinh;
            ViewData["iDM_MaNhomPhu"] = iDM_MaNhomPhu;
            ViewData["iID_MaDanhMuc"] = MaDanhMuc;
            ViewData["Loi"] = Loi;

            ViewData["TimKiemVatTu_page"] = TimKiemVatTu_page;
            ViewData["Searchid"] = Searchid;
            ViewData["sMaVatTu_Search"] = sMaVatTu_Search;
            ViewData["sTen_Search"] = sTen_Search;
            ViewData["sTenGoc_Search"] = sTenGoc_Search;
            ViewData["sQuyCach_Search"] = sQuyCach_Search;
            ViewData["cbsMaVatTu_Search"] = cbsMaVatTu_Search;
            ViewData["cbsTen_Search"] = cbsTen_Search;
            ViewData["cbsTenGoc_Search"] = cbsTenGoc_Search;
            ViewData["cbsQuyCach_Search"] = cbsQuyCach_Search;

            ViewData["MaNhomLoaiVatTu_Search"] = MaNhomLoaiVatTu_Search;
            ViewData["MaNhomChinh_Search"] = MaNhomChinh_Search;
            ViewData["MaNhomPhu_Search"] = MaNhomPhu_Search;
            ViewData["MaChiTietVatTu_Search"] = MaChiTietVatTu_Search;
            ViewData["MaXuatXu_Search"] = MaXuatXu_Search;
            ViewData["iTrangThai_Search"] = iTrangThai_Search;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search_Search(String ParentID, int Searchid, String MaDanhMuc, String LoaiDanhMuc, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh, String iDM_MaNhomPhu, String Loi)
        {
            if (Searchid == 1)
            {
                String sMaVatTu_Search = Request.Form[ParentID + "_sMaVatTu_Search"];
                String sTen_Search = Request.Form[ParentID + "_sTen_Search"];
                String sTenGoc_Search = Request.Form[ParentID + "_sTenGoc_Search"];
                String sQuyCach_Search = Request.Form[ParentID + "_sQuyCach_Search"];
                String cbsMaVatTu_Search = Request.Form[ParentID + "_cbsMaVatTu_Search"];
                String cbsTen_Search = Request.Form[ParentID + "_cbsTen_Search"];
                String cbsTenGoc_Search = Request.Form[ParentID + "_cbsTenGoc_Search"];
                String cbsQuyCach_Search = Request.Form[ParentID + "_cbsQuyCach_Search"];

                return RedirectToAction("Edit", new
                {
                    MaDanhMuc = MaDanhMuc,
                    LoaiDanhMuc = LoaiDanhMuc,
                    iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu,
                    iDM_MaNhomChinh = iDM_MaNhomChinh,
                    iDM_MaNhomPhu = iDM_MaNhomPhu,
                    Loi = Loi,
                    Searchid = Searchid,
                    sMaVatTu_Search = sMaVatTu_Search,
                    sTen_Search = sTen_Search,
                    sTenGoc_Search = sTenGoc_Search,
                    sQuyCach_Search = sQuyCach_Search,
                    cbsMaVatTu_Search = cbsMaVatTu_Search,
                    cbsTen_Search = cbsTen_Search,
                    cbsTenGoc_Search = cbsTenGoc_Search,
                    cbsQuyCach_Search = cbsQuyCach_Search
                });
            }
            else
            {
                String iDM_MaNhomLoaiVatTu_Search = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu_Search"]; ;
                String iDM_MaNhomChinh_Search = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String iDM_MaNhomPhu_Search = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String iDM_MaChiTietVatTu_Search = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
                String iDM_MaXuatXu_Search = Request.Form[ParentID + "_iDM_MaXuatXu_Search"];
                String iTrangThai_Search = Request.Form[ParentID + "_iTrangThai_Search"];
                return RedirectToAction("Edit", new
                {
                    MaDanhMuc = MaDanhMuc,
                    LoaiDanhMuc = LoaiDanhMuc,
                    iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu,
                    iDM_MaNhomChinh = iDM_MaNhomChinh,
                    iDM_MaNhomPhu = iDM_MaNhomPhu,
                    Loi = Loi,
                    Searchid = Searchid,
                    MaNhomLoaiVatTu_Search = iDM_MaNhomLoaiVatTu_Search,
                    MaNhomChinh_Search = iDM_MaNhomChinh_Search,
                    MaNhomPhu_Search = iDM_MaNhomPhu_Search,
                    MaChiTietVatTu_Search = iDM_MaChiTietVatTu_Search,
                    MaXuatXu_Search = iDM_MaXuatXu_Search,
                    iTrangThai_Search = iTrangThai_Search
                });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaDanhMuc, String LoaiDanhMuc)
        {
            String iDM_MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"];
            String iDM_MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
            String iDM_MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
            String sTenKhoa = Request.Form[ParentID + "_sTenKhoa"];
            String sTen = Request.Form[ParentID + "_sTen"];
            String sGhiChu = Request.Form[ParentID + "_sGhiChu"]; 

            Bang bang = new Bang("DC_DanhMuc");
            String iID_MaDanhMucCha = "";
            String iID_MaDanhMuc = "";
            switch (LoaiDanhMuc)
            {
                case "NhomChinh":
                    iID_MaDanhMucCha = iDM_MaNhomLoaiVatTu;
                    break;

                case "NhomPhu":
                    iID_MaDanhMucCha = iDM_MaNhomChinh;
                    break;

                case "ChiTietVatTu":
                    iID_MaDanhMucCha = iDM_MaNhomPhu;
                    break;
            }

            //Kiểm tra trùng mã dữ liệu
            if (LoaiDanhMuc == "DonViTinh")
                //Kiểm tra tùng tên
                iID_MaDanhMuc = KiemTraTrungDuLieu(Request.Form[ParentID + "_sTen"], LoaiDanhMuc, iID_MaDanhMucCha);
            else
                //Kiểm tra trùng mã danh mục
                iID_MaDanhMuc = KiemTraTrungDuLieu(sTenKhoa, LoaiDanhMuc, iID_MaDanhMucCha);

            if (iID_MaDanhMuc != "" && iID_MaDanhMuc != MaDanhMuc)
                return RedirectToAction("Edit", new { MaDanhMuc = MaDanhMuc, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu, sTen = sTen, sGhiChu = sGhiChu, Loi = "1" });
            //------------

            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.GiaTriKhoa = MaDanhMuc;
            if (!String.IsNullOrEmpty(iID_MaDanhMucCha))
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iID_MaDanhMucCha);
            bang.Save();
            if (!String.IsNullOrEmpty(sTenKhoa))
                Connection.DeleteRecord("DM_ChiTietVatTuDaXoa", "MaChiTietVatTu", sTenKhoa);
            return RedirectToAction("Index", new { LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu });
        }

        public String KiemTraTrungDuLieu(String sTenKhoa, String LoaiDanhMuc, String iID_MaDanhMucCha)
        {
            String iID_MaDanhMuc = "";
            if (LoaiDanhMuc == "DonViTinh")
            {
                SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                    "WHERE sTen = @sTen AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = @LoaiDanhMuc)");
                cmd.Parameters.AddWithValue("@sTen", sTenKhoa);
                cmd.Parameters.AddWithValue("@LoaiDanhMuc", LoaiDanhMuc);
                iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            else
            {
                String DK = "";
                if (iID_MaDanhMucCha != "") DK = "AND iID_MaDanhMucCha = '" + iID_MaDanhMucCha + "' ";
                SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                    "WHERE sTenKhoa = @sTenKhoa " + DK +
                        "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                "FROM DC_LoaiDanhMuc " +
                                                "WHERE sTenBang = @LoaiDanhMuc)");
                cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
                cmd.Parameters.AddWithValue("@LoaiDanhMuc", LoaiDanhMuc);
                iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            return iID_MaDanhMuc;
        }

        public ActionResult Delete(String MaDanhMuc, String LoaiDanhMuc, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh, String iDM_MaNhomPhu, String MaChiTietVatTuDaXoa)
        {
            if (LoaiDanhMuc == "ChiTietVatTu" && !String.IsNullOrEmpty(MaChiTietVatTuDaXoa))
            {
                if (MaChiTietVatTuDaXoa.Length == 4)
                {
                    Bang bangDM_ChiTietVatTuDaXoa = new Bang("DM_ChiTietVatTuDaXoa");
                    bangDM_ChiTietVatTuDaXoa.GiaTriKhoa = MaChiTietVatTuDaXoa;
                    bangDM_ChiTietVatTuDaXoa.Save();
                }    
            }
            
            Bang bang = new Bang("DC_DanhMuc");
            bang.GiaTriKhoa = MaDanhMuc;
            bang.Delete();
            return RedirectToAction("Index", new { LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID, String LoaiDanhMuc)
        {
            String iDM_MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"];
            String iDM_MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
            String iDM_MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
            String sTen = Request.Form[ParentID + "_sTen"];

            return RedirectToAction("Index", new { LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu, sTen = sTen });
        }

        [Authorize]
        public ActionResult ExportExcel(String ParentID, String LoaiDanhMuc, String SQL)
        {
            Export(SQL, LoaiDanhMuc);
            return RedirectToAction("Search", new { ParentID = ParentID, LoaiDanhMuc = LoaiDanhMuc });
        }

        public void Export(String SQL, String LoaiDanhMuc)
        {
            SqlCommand cmd = new SqlCommand(SQL + " ORDER BY sTenKhoa");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i, j;
            Response.BufferOutput = false;
            Response.Charset = string.Empty;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            string attachment = "attachment; filename=" + LoaiDanhMuc + ".xls";
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
                if (dt.Columns[i].ColumnName == "sTenKhoa")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Mã");
                if (dt.Columns[i].ColumnName == "sTen")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Tên");
                if (dt.Columns[i].ColumnName == "sGhiChu")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Ghi chú");
                if (dt.Columns[i].ColumnName == "bHoatDong")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Hoạt động");
            }
            textOutput += "</tr>\r\n";
            Response.Write(textOutput);

            textOutput = string.Empty;
            for ( i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = dt.Rows[i];
                textOutput = "<tr height=17 style='height:12.75pt'>\r\n";

                for ( j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    string dataRowText = dataRow[j].ToString().Trim();

                    dataRowText = dataRowText.Replace("\t", " ");
                    dataRowText = dataRowText.Replace("\r", string.Empty);
                    dataRowText = dataRowText.Replace("\n", " ");

                    string sContent = "<td align='left' style='mso-number-format:\\@;'>{0}</td>";

                    if (dt.Columns[j].ColumnName == "sTenKhoa")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sTen")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sGhiChu")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "bHoatDong")
                    {
                        if (dataRowText == "True")
                            textOutput += string.Format(CultureInfo.InvariantCulture, sContent, "x");
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

        static private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public class  NhomChinh
        {
           public String ddlNhomChinh;
           public String iDM_MaNhomChinh;
        }

        public JsonResult get_dtNhomChinh(String ParentID, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh)
        {
            return Json(get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh), JsonRequestBehavior.AllowGet);
        }

        public static NhomChinh get_objNhomChinh(String ParentID, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh)
        {
            String strNhomChinh = String.Empty;
            String DK = String.Empty;
            SqlCommand cmd;
            DataTable dt = null;

            if (!string.IsNullOrEmpty(iDM_MaNhomLoaiVatTu))
            {
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomLoaiVatTu + "'";
                cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            else
            {
                

                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
                DataRow R = dt.NewRow();
                //if (ParentID == "Index") R[1] = "---All---";
                dt.Rows.InsertAt(R, 0);
            }
          
            SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strNhomChinh = MyHtmlHelper.DropDownList(ParentID, slNhomChinh, iDM_MaNhomChinh, "iDM_MaNhomChinh", "", "onchange=\"ChonNhomChinh(this.value)\" style=\"width: 100%;\"");
            
            NhomChinh _NhomChinh = new NhomChinh();
            _NhomChinh.ddlNhomChinh = strNhomChinh;
            if (dt.Rows.Count > 0)
                _NhomChinh.iDM_MaNhomChinh = Convert.ToString(dt.Rows[0]["iID_MaDanhMuc"]);
            return _NhomChinh;
        }

        public JsonResult get_dtNhomPhu(String ParentID, String iDM_MaNhomChinh, String iDM_MaNhomPhu)
        {
            return Json(get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu), JsonRequestBehavior.AllowGet);
        }

        public static String get_objNhomPhu(String ParentID, String iDM_MaNhomChinh, String iDM_MaNhomPhu)
        {
            String strNhomPhu = String.Empty;
            String DK = String.Empty;
            String tg = "0";
            SqlCommand cmd;
            DataTable dt = null;

            if (!string.IsNullOrEmpty(iDM_MaNhomChinh))
            {
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomChinh + "'";
                if (iDM_MaNhomChinh == "null")
                    tg = "1";
            }
            else
                iDM_MaNhomPhu = "";

            if (tg == "0")
            {
                if (string.IsNullOrEmpty(iDM_MaNhomChinh))
                {
                   

                    dt = new DataTable();
                    dt.Columns.Add("iID_MaDanhMuc");
                    dt.Columns.Add("sTenKhoa");
                    DataRow R = dt.NewRow();
                    //if (ParentID == "Index") R[1] = "---All---";
                    dt.Rows.InsertAt(R, 0);
                }
                else
                {
                    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
            }
            SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strNhomPhu = MyHtmlHelper.DropDownList(ParentID, slNhomPhu, iDM_MaNhomPhu, "iDM_MaNhomPhu", "", "style=\"width: 100%;\" onchange=\"MaChiTietVatTuGoiY()\"");
            
            return strNhomPhu ;
        }

        public JsonResult get_dtMaChiTietVatTu(String ParentID, String iDM_MaNhomPhu)
        {
            return Json(get_objMaChiTietVatTu(ParentID, iDM_MaNhomPhu), JsonRequestBehavior.AllowGet);
        }

        public static String get_objMaChiTietVatTu(String ParentID, String iDM_MaNhomPhu)
        {
            SqlCommand cmd = new SqlCommand("SELECT sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1 AND iID_MaDanhMucCha = @iDM_MaNhomPhu " + 
                        "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                    "FROM DC_LoaiDanhMuc " +
                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa DESC");
            cmd.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);
            String MaChiTietVatTuGoiY = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            if (MaChiTietVatTuGoiY != "")
            {
                MaChiTietVatTuGoiY = Convert.ToString(Convert.ToInt32(MaChiTietVatTuGoiY) + 1);
                while (MaChiTietVatTuGoiY.Length < 5)
                    MaChiTietVatTuGoiY = MaChiTietVatTuGoiY.Insert(0, "0");
            }
            else
                MaChiTietVatTuGoiY = "00000";

            return MaChiTietVatTuGoiY;
        }
        public JsonResult jsKiemTraTrungMaVatTu(String sTenKhoa, String LoaiDanhMuc)
        {
            return Json(csKiemTraTrungMaVatTu(sTenKhoa, LoaiDanhMuc), JsonRequestBehavior.AllowGet);
        }

        public static int csKiemTraTrungMaVatTu(String sTenKhoa, String LoaiDanhMuc)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(sTenKhoa) FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa " +
                        "AND iID_MaLoaiDanhMuc = (SELECT TOP 1 iID_MaLoaiDanhMuc " +
                                                    "FROM DC_LoaiDanhMuc " +
                                                    "WHERE sTenBang = @sTenBang)");
            cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
            cmd.Parameters.AddWithValue("@sTenBang", LoaiDanhMuc);
            return Convert.ToInt32(Connection.GetValue(cmd,0));
        }
    }
}
