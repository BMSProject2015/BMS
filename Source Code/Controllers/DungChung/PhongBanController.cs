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
namespace VIETTEL.Controllers.DuToan
{
    public class PhongBanController : Controller
    {
        //
        // GET: /PhongBan/
        public string sViewPath = "~/Views/DungChung/PhongBan/";
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "PhongBan_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(Code))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaPhongBan"] = Code;
            return View(sViewPath + "PhongBan_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sMaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            //if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            //{
            //    if (CheckMaDonVi(sMaDonVi) == true)
            //    {
            //        arrLoi.Add("err_iMaDonVi", "Mã đơn vị đã tồn tại!");
            //    }
            //}
            // ViewData["iID_MaPhongBan"] = "1";
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên phòng ban!");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaPhongBan"] = Code;
                return View(sViewPath + "PhongBan_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("NS_PhongBan");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (sMaDonVi == Convert.ToString(Guid.Empty) || sMaDonVi == "")
                {
                    bang.GiaTriKhoa = Guid.NewGuid();
                }
                else
                    bang.GiaTriKhoa = sMaDonVi;
                bang.Save();
                return View(sViewPath + "PhongBan_Edit.aspx");
            }
        }

        [Authorize]
        public ActionResult Delete(String Code)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhongBan", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("NS_PhongBan");
            bang.GiaTriKhoa = Code;
            bang.Delete();
            return View(sViewPath + "PhongBan_Index.aspx");
        }

    }
}
