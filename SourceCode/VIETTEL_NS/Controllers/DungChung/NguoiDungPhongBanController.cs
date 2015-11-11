using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.DungChung
{
    public class NguoiDungPhongBanController : Controller
    {
        //
        // GET: /NguoiDungPhongBan/
        public string sViewPath = "~/Views/DungChung/NguoiDungPhongBan/";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "NguoiDung_PhongBan_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String sMaNguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(sMaNguoiDung))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["sMaNguoiDung"] = sMaNguoiDung;
                return View(sViewPath + "NguoiDung_PhongBan_List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult Add(String sMaNguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(sMaNguoiDung))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["sMaNguoiDung"] = sMaNguoiDung;
                return View(sViewPath + "NguoiDung_PhongBan_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditNew(String sMaNguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "1";
                ViewData["sMaNguoiDung"] = sMaNguoiDung;
                return View(sViewPath + "NguoiDung_PhongBan_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                NameValueCollection arrLoi = new NameValueCollection();
                String iID_MaNguoiDungPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaNguoiDungPhongBan"]);
                String iID_MaPhongBan = Convert.ToString(Request.Form["iID_MaPhongBan"]);
                String bPublic = Convert.ToString(Request.Form[ParentID + "_bPublic"]);
                String sMaNguoiDung = Convert.ToString(Request.Form[ParentID + "_sMaNguoiDung"]);
                Bang bang = new Bang("NS_NguoiDung_PhongBan");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                if (String.IsNullOrEmpty(sMaNguoiDung))
                {
                    arrLoi.Add("err_sMaNguoiDung", "Bạn chưa chọn người dùng!");
                }
                if (String.IsNullOrEmpty(iID_MaPhongBan))
                {
                    arrLoi.Add("err_iID_MaPhongBan", "Bạn chưa chọn phòng ban!");
                }
                //if (HamChung.Check_Trung("NS_NguoiDung_PhongBan", "iID_MaNguoiDungPhongBan", iID_MaNguoiDungPhongBan, "sMaNguoiDung,iID_MaPhongBan", sMaNguoiDung + "," + iID_MaPhongBan, bang.DuLieuMoi))
                //{
                //    arrLoi.Add("err_iID_MaPhongBan", "Phòng ban chọn đã được phân cho người dùng!");
                //}         
                if (arrLoi.Count > 0)
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["iID_MaDanhMucDuAn"] = sMaNguoiDung;
                    ViewData["sMaNguoiDung"] = sMaNguoiDung;
                    ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
                    ViewData["bPublic"] = bPublic;
                    return View(sViewPath + "NguoiDung_PhongBan_Edit.aspx");
                }

                else
                {
                    String SQL = "DELETE FROM NS_NguoiDung_PhongBan WHERE sMaNguoiDung=@sMaNguoiDung  ";
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    String[] arrMaPhongBan = iID_MaPhongBan.Split(',');
                    for (int i = 0; i < arrMaPhongBan.Length; i++)
                    {


                        if (i == 0)
                        {
                            bang.CmdParams.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", arrMaPhongBan[i]);

                        }
                        else
                        {
                            bang.CmdParams.Parameters["@sMaNguoiDung"].Value = sMaNguoiDung;
                            bang.CmdParams.Parameters["@iID_MaPhongBan"].Value = arrMaPhongBan[i];
                        }
                        bang.Save();
                    }
                    //bang.GiaTriKhoa = iID_MaNguoiDungPhongBan;
                    //bang.Save();
                    ViewData["DuLieuMoi"] = "1";
                    ViewData["sMaNguoiDung"] = sMaNguoiDung;
                    ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
                    return View(sViewPath + "NguoiDung_PhongBan_Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        private Boolean CheckDvi(String User, String MaPhongBan)
        {
            Boolean vR = false;
            DataTable dt = NguoiDung_PhongBanModels.getPhongBanByNguoiDung(User, MaPhongBan);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }

        [Authorize]
        public ActionResult EditDetail(String sMaNguoiDung, String MaNguoiDungPhongBan)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Edit") == false)
                //{
                //    return RedirectToAction("Index", "PermitionMessage");
                //}
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaNguoiDungPhongBan))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaNguoiDungPhongBan"] = MaNguoiDungPhongBan;
                ViewData["sMaNguoiDung"] = sMaNguoiDung;
                return View(sViewPath + "NguoiDung_PhongBan_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult DeleteDetail(String sMaNguoiDung, String MaNguoiDungPhongBan)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("NS_NguoiDung_PhongBan");
                bang.GiaTriKhoa = sMaNguoiDung;
                bang.Delete();
                // return View(sViewPath + "NguoiDung_DonVi_List.aspx?sMaNguoiDung =" + NguoiDung);
                return RedirectToAction("Edit", "NguoiDungPhongBan", new {sMaNguoiDung = sMaNguoiDung});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Delete(String sMaNguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_PhongBan", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }

                Bang bang = new Bang("NS_NguoiDung_PhongBan");
                bang.TruongKhoa = "sMaNguoiDung";
                bang.GiaTriKhoa = sMaNguoiDung;
                bang.Delete();
                return View(sViewPath + "NguoiDung_PhongBan_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String search_MaNguoiDung = Request.Form[ParentID + "_search_MaNguoiDung"];
                String search_iID_MaPhongBan = Request.Form[ParentID + "_search_iID_MaPhongBan"];
                return RedirectToAction("Index", "NguoiDungPhongBan",
                                        new
                                            {
                                                MaNguoiDung = search_MaNguoiDung.Trim(),
                                                MaPhongBan = search_iID_MaPhongBan.Trim()
                                            });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public JsonResult ObjDanhSachDonVi(String MaNguoiDung = "", String MaDonVi = "", int CurrentPage = 1,
                                           int PageSize = 1)
        {
            return Json(get_sDanhSachDonVi(MaNguoiDung, MaDonVi, CurrentPage, Globals.PageSize),
                        JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String MaNguoiDung = "", String MaDonVi = "", int CurrentPage = 1,
                                         int PageSize = 1)
        {
            String ParentID = "NDDV";
            DataTable dtTrangThai = DanhMucModels.NS_DonVi();
            dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
            dtTrangThai.Rows[0]["iID_MaDonVi"] = "";
            DataTable dtDonVi = DanhMucModels.getPhongBanByCombobox();
            //DataTable dt = NguoiDung_DonViModels.getList(MaNguoiDung, MaDonVi, CurrentPage, Globals.PageSize);
            int SoCot = 2;
            if (dtDonVi.Rows.Count >= 10)
                SoCot = 5;

            StringBuilder stb = new StringBuilder();
            String[] arrMaDonVi = MaDonVi.Split(',');
            stb.Append("<table  class=\"mGrid\">");
            String strsTen = "", strChecked = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                stb.Append("<tr>");
                strChecked = "";
                strsTen = Convert.ToString(dtDonVi.Rows[i]["sTen"]);
                MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < arrMaDonVi.Length; j++)
                {
                    if (MaDonVi.Equals(arrMaDonVi[j]))
                    {
                        strChecked = "checked=\"checked\"";
                        break;
                    }
                }
                stb.Append("<td align=\"center\" style=\"width:25px;\">");
                stb.Append("<input type=\"checkbox\" " + strChecked + " value=\"" + MaDonVi +
                           "\"check-group=\"DonVi\" id=\"iID_MaPhongBan\" name=\"iID_MaPhongBan\"/>");
                stb.Append("</td>");
                stb.Append("<td align=\"left\">" + strsTen + "</td>");
                stb.Append("</tr>");
            }
            stb.Append("</table>");
            stb.Append("<script type=\"text/javascript\">");
            stb.Append("function CheckAll(value) {");
            stb.Append("$(\"input:checkbox[check-group='DonVi']\").each(function (i) {");
            stb.Append("this.checked = value;");
            stb.Append("});");
            stb.Append("}");
            stb.Append("</script>");

            return stb.ToString();

        }
    }
}
