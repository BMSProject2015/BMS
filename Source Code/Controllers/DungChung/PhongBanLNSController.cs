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
    public class PhongBanLNSController : Controller
    {
        //
        // GET: /PhongBanLNS/
        public string sViewPath = "~/Views/DungChung/PhongBanLNS/";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "PhongBan_LNS_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(Code))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_LNS_List.aspx");
        }

        public ActionResult Add(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(Code))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_LNS_Edit.aspx");
        }

        public ActionResult EditNew(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "1";
            ViewData["MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_LNS_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditSubmit(String ParentID, String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaPhongBanLoaiNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBanLoaiNganSach"]);
            String iID_MaPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String bPublic = Convert.ToString(Request.Form[ParentID + "_bPublic"]);
            Bang bang = new Bang("NS_PhongBan_LoaiNganSach");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (iID_MaPhongBan == "" || iID_MaPhongBan == Convert.ToString(Guid.Empty))
            {
                arrLoi.Add("err_MaPhongBan", "Bạn chưa chọn phòng ban!");
            }
            if (String.IsNullOrEmpty(sLNS))
            {
                arrLoi.Add("err_MaDonVi", "Bạn chưa chọn loại ngân sách!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && sLNS != "")
            {
                if (CheckDvi(iID_MaPhongBan, sLNS) == true)
                {
                    arrLoi.Add("err_MaDonVi", "Loại ngân sách bạn chọn đã được phân cho phòng ban!");
                }
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaDanhMucDuAn"] = Code;
                ViewData["MaPhongBan"] = iID_MaPhongBan;
                ViewData["iID_MaPhongBanLoaiNganSach"] = iID_MaPhongBanLoaiNganSach;
                ViewData["LNS"] = sLNS;
                ViewData["bPublic"] = bPublic;
                return View(sViewPath + "PhongBan_LNS_Edit.aspx");
            }
            else
            {
                String SQL = "DELETE FROM NS_PhongBan_LoaiNganSach WHERE iID_MaPhongBan=@iID_MaPhongBan ";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
             
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                String[] arrLoaiNS = sLNS.Split(',');
                for (int i = 0; i < arrLoaiNS.Length; i++)
                {


                    if (i == 0)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                        bang.CmdParams.Parameters.AddWithValue("@sLNS", arrLoaiNS[i]);

                    }
                    else
                    {
                        bang.CmdParams.Parameters["@iID_MaPhongBan"].Value = iID_MaPhongBan;
                        bang.CmdParams.Parameters["@sLNS"].Value = arrLoaiNS[i];
                    }
                    bang.Save();
                }


                //bang.GiaTriKhoa = iID_MaPhongBanLNS;
                //bang.Save();
                ViewData["DuLieuMoi"] = "1";
                ViewData["MaPhongBan"] = iID_MaPhongBan;
                ViewData["iID_MaPhongBanLoaiNganSach"] = iID_MaPhongBanLoaiNganSach;
                ViewData["LNS"] = sLNS;
                return View(sViewPath + "PhongBan_LNS_Edit.aspx");
            }
        }

        private Boolean CheckDvi(string MaPhongBan, string LNS)
        {
            Boolean vR = false;
            //DataTable dt = PhongBan_LNSModels.getLNSByPhongBan(MaPhongBan, LNS);
            //if (dt.Rows.Count > 0)
            //{
            //    vR = true;
            //}
            //if (dt != null) dt.Dispose();
            return vR;
        }

        [Authorize]
        public ActionResult EditDetail(String Code, String MaID, String LNS)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Edit") == false)
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
                ViewData["LNS"] = LNS;
                return View(sViewPath + "PhongBan_LNS_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult DeleteDetail(String Code, String MaID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Delete") == false ||
                    !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("NS_PhongBan_LoaiNganSach");
                bang.GiaTriKhoa = Code;
                bang.Delete();
                // return View(sViewPath + "NguoiDung_DonVi_List.aspx?Code =" + NguoiDung);
                return RedirectToAction("Edit", "PhongBanDonVi", new {Code = MaID});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Delete(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan_LoaiNganSach", "Delete") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("NS_PhongBan_LoaiNganSach");
            bang.TruongKhoa = "iID_MaPhongBan";
            bang.GiaTriKhoa = Code;
            bang.Delete();
            return View(sViewPath + "PhongBan_LNS_Index.aspx");
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
                String MaLNS = Request.Form[ParentID + "_search_sLNS"];
                return RedirectToAction("Index", "PhongBanLNS",
                                        new {MaPhongBan = MaPhongBan.Trim(), MaLNS = MaLNS.Trim()});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }



        public JsonResult ObjDanhSachDonVi(String sLNS = "", int CurrentPage = 1, int PageSize = 1)
        {
            return Json(get_sDanhSachDonVi(sLNS, CurrentPage, Globals.PageSize), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String sLNS = "", int CurrentPage = 1, int PageSize = 1)
        {
            String ParentID = "NDDV";

            DataTable dtLNS = DanhMucModels.NS_LoaiNganSach();
            //DataTable dt = NguoiDung_DonViModels.getList(MaNguoiDung, MaDonVi, CurrentPage, Globals.PageSize);
            int SoCot = 2;
            if (dtLNS.Rows.Count >= 10)
                SoCot = 5;

            StringBuilder stb = new StringBuilder();
            String[] arrLoaiNS = sLNS.Split(',');
            stb.Append("<table  class=\"mGrid\">");
            String strsTen = "", strChecked = "";
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                stb.Append("<tr>");
                strChecked = "";
                strsTen = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
                sLNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                for (int j = 0; j < arrLoaiNS.Length; j++)
                {
                    if (sLNS.Equals(arrLoaiNS[j]))
                    {
                        strChecked = "checked=\"checked\"";
                        break;
                    }
                }
                stb.Append("<td align=\"center\" style=\"width:25px;\">");
                stb.Append("<input type=\"checkbox\" " + strChecked + " value=\"" + sLNS +
                           "\"check-group=\"DonVi\" id=\"sLNS\" name=\"sLNS\"/>");
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
