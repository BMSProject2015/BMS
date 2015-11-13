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

namespace VIETTEL.Controllers.QLDA
{
    public class QLDA_BanQuanLyController : Controller
    {
        //
        // GET: /QLDA_BanQuanLy/
        public string sViewPath = "~/Views/QLDA/DanhMuc/BanQuanLy/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_BanQuanLy", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_BanQuanLy_Index.aspx");
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
            String iID_MaChuDauTu = Request.Form[ParentID + "_iID_MaChuDauTu"];
            String sTenBanQuanLy = Request.Form[ParentID + "_sTenBanQuanLy"];

            return RedirectToAction("Index", "QLDA_BanQuanLy", new { iID_MaChuDauTu = iID_MaChuDauTu, sTenBanQuanLy = sTenBanQuanLy });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaBanQuanLy)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaBanQuanLy))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaBanQuanLy"] = iID_MaBanQuanLy;
            return View(sViewPath + "QLDA_BanQuanLy_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_BanQuanLy", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaBanQuanLy = Convert.ToString(Request.Form[ParentID + "_iID_MaBanQuanLy"]);
            String sMaChuDauTu = Convert.ToString(Request.Form[ParentID + "_iID_MaChuDauTu"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            if (sMaChuDauTu == "" && String.IsNullOrEmpty(sMaChuDauTu) == true)
            {
                arrLoi.Add("err_iID_MaChuDauTu", "Bạn phải chọn chủ đầu tư!");
            }
            if (sTen == "" && String.IsNullOrEmpty(sTen) == true)
            {
                arrLoi.Add("err_sTenBanQuanLy", "Bạn phải nhập tên ban quản lý!");
            }
           
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaBanQuanLy"] = iID_MaBanQuanLy;
                ViewData["DuLieuMoi"] = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
                return View(sViewPath + "QLDA_BanQuanLy_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("QLDA_BanQuanLy");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);                
                bang.Save();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(String iID_MaBanQuanLy)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_BanQuanLy", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("QLDA_BanQuanLy");
            bang.GiaTriKhoa = iID_MaBanQuanLy;
            bang.Delete();
            return View(sViewPath + "QLDA_BanQuanLy_Index.aspx");
        }
       
    
    }
}
