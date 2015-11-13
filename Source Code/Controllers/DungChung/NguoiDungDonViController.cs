//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DomainModel.Abstract;
//using DomainModel;
//using System.Collections.Specialized;
//using System.Data.SqlClient;
//using System.Data;
//using VIETTEL.Models;
//using System.Text;
//namespace VIETTEL.Controllers.DungChung
//{
//    public class NguoiDungDonViController : Controller
//    {
//        //
//        // GET: /NguoiDungDonVi/
//        public string sViewPath = "~/Views/DungChung/NguoiDungDonVi/";
//        public ActionResult Index()
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }
//            return View(sViewPath + "NguoiDung_DonVi_Index.aspx");
//        }
//        [Authorize]
//        public ActionResult Edit(String Code)
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }
//            ViewData["DuLieuMoi"] = "0";
//            if (String.IsNullOrEmpty(Code))
//            {
//                ViewData["DuLieuMoi"] = "1";
//            }
//            ViewData["sMaNguoiDung"] = Code;
//            return View(sViewPath + "NguoiDung_DonVi_List.aspx");
//        }
//        public ActionResult Add(String Code)
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }
//            ViewData["DuLieuMoi"] = "0";
//            if (String.IsNullOrEmpty(Code))
//            {
//                ViewData["DuLieuMoi"] = "1";
//            }
//            ViewData["sMaNguoiDung"] = Code;
//            return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
//        }

//        public ActionResult EditNew(String Code)
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }
//            ViewData["DuLieuMoi"] = "1";           
//            ViewData["sMaNguoiDung"] = Code;
//            return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
//        }
//        [Authorize]
//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult EditSubmit(String ParentID, String Code)
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }
//            NameValueCollection arrLoi = new NameValueCollection();
//            String iID_MaNguoiDungDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaNguoiDungDonVi"]);
//            String sMaNguoiDung = Convert.ToString(Request.Form[ParentID + "_sMaNguoiDung"]);
//            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
//            String bPublic = Convert.ToString(Request.Form[ParentID + "_bPublic"]);
//            Bang bang = new Bang("NS_NguoiDung_DonVi");
//            bang.MaNguoiDungSua = User.Identity.Name;
//            bang.IPSua = Request.UserHostAddress;
//            bang.TruyenGiaTri(ParentID, Request.Form);
//            if (sMaNguoiDung ==  "")
//            {
//                arrLoi.Add("err_sMaNguoiDung", "Bạn chưa chọn người dùng!");
//            }
//            if (iID_MaDonVi == "")
//            {
//                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
//            }
//            if (HamChung.Check_Trung("NS_NguoiDung_DonVi", "iID_MaNguoiDungDonVi", iID_MaNguoiDungDonVi, "sMaNguoiDung,iID_MaDonVi", sMaNguoiDung + "," + iID_MaDonVi, bang.DuLieuMoi))
//            {
//                arrLoi.Add("err_iID_MaDonVi", "Đơn vị chọn đã được phân cho người dùng!");
//            }
           
//            if (arrLoi.Count > 0)
//            {
//                for (int i = 0; i <= arrLoi.Count - 1; i++)
//                {
//                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
//                }
//                ViewData["iID_MaDanhMucDuAn"] = Code;
//                ViewData["sMaNguoiDung"] = sMaNguoiDung;
//                ViewData["iID_MaDonVi"] = iID_MaDonVi;
//                ViewData["bPublic"] = bPublic;
//                return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
//            }
//            else
//            {
            
//                bang.GiaTriKhoa = iID_MaNguoiDungDonVi;
//                bang.Save();
//                ViewData["DuLieuMoi"] = "1";
//                ViewData["sMaNguoiDung"] = sMaNguoiDung;
//                return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
//            }
//        }

//        private Boolean CheckDvi(String User, String MaDonVi)
//        {
//            Boolean vR = false;
//            DataTable dt = NguoiDung_DonViModels.getDonViByNguoiDung(User, MaDonVi);
//            if (dt.Rows.Count > 0)
//            {
//                vR = true;
//            }
//            if (dt != null) dt.Dispose();
//            return vR;
//        }
//        [Authorize]
//        public ActionResult EditDetail(String Code, String NguoiDung)
//        {
//            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
//            //{
//            //    return RedirectToAction("Index", "PermitionMessage");
//            //}
//            ViewData["DuLieuMoi"] = "0";
//            if (String.IsNullOrEmpty(NguoiDung))
//            {
//                ViewData["DuLieuMoi"] = "1";
//            }            
//            ViewData["iID_MaNguoiDungDonVi"] = NguoiDung;
//            ViewData["sMaNguoiDung"] = Code;
//            return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
//        }

//        [Authorize]
//        public ActionResult DeleteDetail(String Code, String NguoiDung)
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Delete") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }
//            Bang bang = new Bang("NS_NguoiDung_DonVi");
//            bang.GiaTriKhoa = Code;
//            bang.Delete();
//           // return View(sViewPath + "NguoiDung_DonVi_List.aspx?Code =" + NguoiDung);
//            return RedirectToAction("Edit", "NguoiDungDonVi", new { Code = NguoiDung });
//        }
//        [Authorize]
//        public ActionResult Delete(String Code)
//        {
//            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Delete") == false)
//            {
//                return RedirectToAction("Index", "PermitionMessage");
//            }        
          
//            Bang bang = new Bang("NS_NguoiDung_DonVi");
//            bang.TruongKhoa = "sMaNguoiDung";
//            bang.GiaTriKhoa = Code;
//            bang.Delete();                
//            return View(sViewPath + "NguoiDung_DonVi_Index.aspx");
//        }
//        [Authorize]
//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult SearchSubmit(String ParentID)
//        {
//            String search_MaNguoiDung = Request.Form[ParentID + "_search_MaNguoiDung"];
//            String search_iID_MaDonVi = Request.Form[ParentID + "_search_iID_MaDonVi"];
//            return RedirectToAction("Index", "NguoiDungDonVi", new { MaNguoiDung = search_MaNguoiDung.Trim(), MaDonVi = search_iID_MaDonVi.Trim() });
//        }

//        public JsonResult get_objDonViQuanLy(String MaNguoiDung)
//        {            
//           // return RedirectToAction("Edit", "NguoiDungDonVi", new { Code = MaNguoiDung });
//            return Json(NguoiDung_DonViModels.DS_NguoiDung_DonVi(MaNguoiDung, false, ""), JsonRequestBehavior.AllowGet);
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;

namespace VIETTEL.Controllers.DungChung
{
    public class NguoiDungDonViController : Controller
    {
        //
        // GET: /NguoiDungDonVi/
        public string sViewPath = "~/Views/DungChung/NguoiDungDonVi/";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "NguoiDung_DonVi_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }


        [Authorize]
        public ActionResult Edit(String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(Code))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["sMaNguoiDung"] = Code;
                return View(sViewPath + "NguoiDung_DonVi_List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult Add(String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(Code))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["sMaNguoiDung"] = Code;
                return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditNew(String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "1";
                ViewData["sMaNguoiDung"] = Code;
                return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                NameValueCollection arrLoi = new NameValueCollection();
                String iID_MaNguoiDungDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaNguoiDungDonVi"]);
                String sMaNguoiDung = Convert.ToString(Request.Form[ParentID + "_sMaNguoiDung"]);
                String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);

                String bPublic = Convert.ToString(Request.Form[ParentID + "_bPublic"]);
                Bang bang = new Bang("NS_NguoiDung_DonVi");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                if (String.IsNullOrEmpty(sMaNguoiDung))
                {
                    arrLoi.Add("err_sMaNguoiDung", "Bạn chưa chọn người dùng!");
                }
                if (String.IsNullOrEmpty(iID_MaDonVi))
                {
                    arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
                }


                if (arrLoi.Count > 0)
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["iID_MaDanhMucDuAn"] = Code;
                    ViewData["sMaNguoiDung"] = sMaNguoiDung;
                    ViewData["iID_MaDonVi"] = iID_MaDonVi;
                    ViewData["bPublic"] = bPublic;
                    return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
                }
                else
                {

                    String SQL = "DELETE FROM NS_NguoiDung_DonVi WHERE sMaNguoiDung=@sMaNguoiDung AND iNamLamViec=@iNamLamViec";
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
                    cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    String[] arrMaDonVi = iID_MaDonVi.Split(',');

                    for (int i = 0; i < arrMaDonVi.Length; i++)
                    {

                        if (i == 0)
                        {
                            bang.CmdParams.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", arrMaDonVi[i]);

                        }
                        else
                        {
                            bang.CmdParams.Parameters["@sMaNguoiDung"].Value = sMaNguoiDung;
                            bang.CmdParams.Parameters["@iID_MaDonVi"].Value = arrMaDonVi[i];
                        }
                        bang.Save();
                    }

                    ViewData["DuLieuMoi"] = "1";
                    ViewData["sMaNguoiDung"] = sMaNguoiDung;
                    ViewData["iID_MaDonVi"] = iID_MaDonVi;
                    return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        private Boolean CheckDvi(String User, String MaDonVi)
        {
            Boolean vR = false;
            //DataTable dt = NguoiDung_DonViModels.getDonViByNguoiDung(User);
            //if (dt.Rows.Count > 0)
            //{
            //    vR = true;
            //}
            //if (dt != null) dt.Dispose();
            return vR;
        }

        [Authorize]
        public ActionResult EditDetail(String Code, String NguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(NguoiDung))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaNguoiDungDonVi"] = NguoiDung;
                ViewData["sMaNguoiDung"] = Code;
                return View(sViewPath + "NguoiDung_DonVi_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult DeleteDetail(String Code, String NguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("NS_NguoiDung_DonVi");
                bang.GiaTriKhoa = Code;
                bang.Delete();
                // return View(sViewPath + "NguoiDung_DonVi_List.aspx?Code =" + NguoiDung);
                return RedirectToAction("Edit", "NguoiDungDonVi", new {Code = NguoiDung});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Delete(String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_NguoiDung_DonVi", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }

                Bang bang = new Bang("NS_NguoiDung_DonVi");
                bang.TruongKhoa = "sMaNguoiDung";
                bang.GiaTriKhoa = Code;
                bang.Delete();
                return View(sViewPath + "NguoiDung_DonVi_Index.aspx");
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
                String search_iID_MaDonVi = Request.Form[ParentID + "_search_iID_MaDonVi"];
                return RedirectToAction("Index", "NguoiDungDonVi",
                                        new
                                            {
                                                MaNguoiDung = search_MaNguoiDung.Trim(),
                                                MaDonVi = search_iID_MaDonVi.Trim()
                                            });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public JsonResult get_objDonViQuanLy(String MaNguoiDung)
        {
            // return RedirectToAction("Edit", "NguoiDungDonVi", new { Code = MaNguoiDung });
            return Json(NguoiDung_DonViModels.DS_NguoiDung_DonVi(MaNguoiDung, false, ""), JsonRequestBehavior.AllowGet);
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
            DataTable dtDonVi = NguoiDung_DonViModels.DS_NguoiDung_DonVi(Convert.ToString(MaNguoiDung), true, "");
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
                           "\"check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\"/>");
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

