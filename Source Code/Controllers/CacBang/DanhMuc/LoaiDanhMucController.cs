using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using DomainModel.Abstract;
using System.Data.SqlClient;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.DanhMuc
{
    public class LoaiDanhMucController : Controller
    {
        public string sViewPath = "~/Views/CacBang/DanhMuc/LoaiDanhMuc/";



        [Authorize]
        public ActionResult Index(int? LoaiDanhMuc_page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData["LoaiDanhMuc_page"] = LoaiDanhMuc_page;
                ViewData["DC_LoaiDanhMuc_dicData"] = dicData;
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string MaLoaiDanhMuc, int? DanhMuc_page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_LoaiDanhMuc");
                bang.GiaTriKhoa = MaLoaiDanhMuc;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
                if (dicData != null)
                {
                    dicData["iID_MaLoaiDanhMuc"] = MaLoaiDanhMuc;
                    dicData["DanhMuc_page"] = DanhMuc_page;
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Detail.aspx");
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
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(string MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_LoaiDanhMuc");
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection) dicData["data"];
                data["iID_MaLoaiDanhMuc"] = MaLoaiDanhMuc.ToString();

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
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_LoaiDanhMuc");
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                //dicData["iID_MaBanHang"] = bang.GiaTriKhoa;
                //dicData["LienHe_page"] = 1;

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
        public ActionResult Delete(string MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_LoaiDanhMuc");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = MaLoaiDanhMuc;
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
        public ActionResult Edit(string MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_LoaiDanhMuc");
                bang.GiaTriKhoa = MaLoaiDanhMuc;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                if (dicData != null)
                {
                    //dicData["iID_MaBanHang"] = MaBanHang;
                    //dicData["LienHe_page"] = 1;

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
                Bang bang = new Bang("DC_LoaiDanhMuc");
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

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String ControlID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_LoaiDanhMuc");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    //bang.Save();
                    string strOrder = Request.Form["hiddenOrder"].ToString();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Update_DanhMuc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DanhMuc_Order", strOrder);
                    Connection.UpdateDatabase(cmd);
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
