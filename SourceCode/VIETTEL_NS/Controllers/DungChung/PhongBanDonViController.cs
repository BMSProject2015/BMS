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
    public class PhongBanDonViController : Controller
    {
        //
        // GET: /PhongBanDonVi/
        public string sViewPath = "~/Views/DungChung/PhongBanDonVi/";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "PhongBan_DonVi_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(Code))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_DonVi_List.aspx");
        }
        public ActionResult Add(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(Code))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_DonVi_Edit.aspx");
        }

        public ActionResult EditNew(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "1";
            ViewData["MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_DonVi_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaPhongBanDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBanDonVi"]);
            String iID_MaPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String bPublic = Convert.ToString(Request.Form[ParentID + "_bPublic"]);

            Bang bang = new Bang("NS_PhongBan_DonVi");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (iID_MaPhongBan == "" || iID_MaPhongBan == Convert.ToString(Guid.Empty))
            {
                arrLoi.Add("err_MaPhongBan", "Bạn chưa chọn phòng ban!");
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_MaDonVi", "Bạn chưa chọn đơn vị!");
            }
               
            //if (HamChung.Check_Trung(bang.TenBang,bang.TruongKhoa,iID_MaPhongBanDonVi,"iID_MaDonVi",iID_MaDonVi,bang.DuLieuMoi))
            //{
            //    arrLoi.Add("err_MaDonVi", "Đơn vị chọn đã được phân cho phòng ban!");
            //}
            
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaPhongBanDonVi"] = Code;
                ViewData["MaPhongBan"] = iID_MaPhongBan;
                ViewData["iID_MaDonVi"] = iID_MaDonVi;
                ViewData["bPublic"] = bPublic;
                return View(sViewPath + "PhongBan_DonVi_Edit.aspx");
            }
            else
            {
                String SQL = "DELETE FROM NS_PhongBan_DonVi WHERE iID_MaPhongBan=@iID_MaPhongBan AND iNamLamViec=@iNamLamViec";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                String[] arrMaDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrMaDonVi.Length; i++)
                {


                    if (i == 0)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", arrMaDonVi[i]);

                    }
                    else
                    {
                        bang.CmdParams.Parameters["@iID_MaPhongBan"].Value = iID_MaPhongBan;
                        bang.CmdParams.Parameters["@iID_MaDonVi"].Value = arrMaDonVi[i];
                    }
                    bang.Save();
                }
                
                //bang.GiaTriKhoa = iID_MaPhongBanDonVi;
                //bang.Save();
                ViewData["DuLieuMoi"] = "1";
                ViewData["MaPhongBan"] = iID_MaPhongBan;
                return View(sViewPath + "PhongBan_DonVi_Edit.aspx");
            }
        }

        private Boolean CheckDvi(string MaPhongBan, string MaDonVi)
        {
            Boolean vR = false;
            DataTable dt = PhongBan_DonViModels.getDonViByPhongBan(MaPhongBan, MaDonVi);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }
        [Authorize]
        public ActionResult EditDetail(String Code, String MaID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Edit") == false)
                //{
                //    return RedirectToAction("Index", "PermitionMessage");
                //}
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaID))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaPhongBanDonVi"] = MaID;
                ViewData["MaPhongBan"] = Code;
                return View(sViewPath + "PhongBan_DonVi_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult DeleteDetail(String Code, String MaID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("NS_PhongBan_DonVi");
            bang.GiaTriKhoa = Code;
            bang.Delete();
            // return View(sViewPath + "NguoiDung_DonVi_List.aspx?Code =" + NguoiDung);
            return RedirectToAction("Edit", "PhongBanDonVi", new { Code = MaID });
        }
        [Authorize]
        public ActionResult Delete(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_DonVi", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("NS_PhongBan_DonVi");
            bang.TruongKhoa = "iID_MaPhongBan";
            bang.GiaTriKhoa = Code;
            bang.Delete();
            return View(sViewPath + "PhongBan_DonVi_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String MaPhongBan = "";
                String key = Request.Form[ParentID + "_search_iID_MaPhongBan"];
                if (key != Convert.ToString(Guid.Empty)) MaPhongBan = key;
                String MaDonVi = Request.Form[ParentID + "_search_iID_MaDonVi"];
                return RedirectToAction("Index", "PhongBanDonVi",
                                        new {MaPhongBan = MaPhongBan.Trim(), MaDonVi = MaDonVi.Trim()});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");   
            }
        }
        public JsonResult ObjDanhSachDonVi(String MaPhongBan = "", String MaDonVi = "", int CurrentPage = 1, int PageSize = 1)
        {
            return Json(get_sDanhSachDonVi(MaPhongBan, MaDonVi, CurrentPage, Globals.PageSize), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String MaPhongBan = "", String MaDonVi = "", int CurrentPage = 1, int PageSize = 1)
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
                stb.Append("<input type=\"checkbox\" " + strChecked + " value=\"" + MaDonVi + "\"check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\"/>");
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
