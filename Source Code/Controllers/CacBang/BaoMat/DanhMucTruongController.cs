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
    public class DanhMucTruongController : Controller
    {
        public string sViewPath = "~/Views/CacBang/BaoMat/DanhMucTruong/";
        public Bang bang = new Bang("PQ_DanhMucTruong");

        [Authorize]
        public ActionResult Index(string TenBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                dicData["sTenBang"] = TenBang;
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(string TenBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection) dicData["data"];
                dicData["DuLieuMoi"] = "1";
                dicData["sTenBang"] = TenBang;
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
        public ActionResult Delete(string TenBang, string MaDanhMucTruong)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = MaDanhMucTruong;
                bang.Delete();
                return RedirectToAction("Index", new {TenBang = TenBang});

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string TenBang, string MaDanhMucTruong)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.GiaTriKhoa = MaDanhMucTruong;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                if (dicData != null)
                {
                    dicData["DuLieuMoi"] = "0";
                    dicData["sTenBang"] = TenBang;
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
                else
                {
                    return RedirectToAction("Index", new {TenBang = TenBang});
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID, string TenBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTenBang", TenBang);
                    bang.Save();
                    return RedirectToAction("Index", new {TenBang = TenBang});
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

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String TenBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection) dicData["data"];
                dicData["sTenBang"] = TenBang;

                dicData["DuLieuMoi"] = "1";
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "Sort.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String ControlID, String TenBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    //bang.Save();
                    string strOrder = Request.Form["hiddenOrder"].ToString();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Update_DanhMucTruong_Order";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DanhMucTruong_Order", strOrder);
                    Connection.UpdateDatabase(cmd);
                    return RedirectToAction("Index", "DanhMucBang");
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
