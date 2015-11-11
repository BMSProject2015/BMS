using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using DomainModel.Abstract;
using VIETTEL.Models;

namespace Oneres.Controllers.ChucNangCam
{
    public class DanhMucBangController : Controller
    {
        public string sViewPath = "~/Views/CacBang/BaoMat/DanhMucBang/";
        public Bang bang = new Bang("PQ_DanhMucBang");

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
            NameValueCollection data = (NameValueCollection) dicData["data"];
            data["bXem"] = "True";
            data["bThem"] = "True";
            data["bXoa"] = "True";
            data["bSua"] = "True";
            dicData["DuLieuMoi"] = "1";
            ViewData[bang.TenBang + "_dicData"] = dicData;
            return View(sViewPath + "Edit.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string MaDanhMucBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = MaDanhMucBang;
                bang.Delete();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string MaDanhMucBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.GiaTriKhoa = MaDanhMucBang;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                if (dicData != null)
                {
                    dicData["DuLieuMoi"] = "0";
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    bang.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ControlID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
