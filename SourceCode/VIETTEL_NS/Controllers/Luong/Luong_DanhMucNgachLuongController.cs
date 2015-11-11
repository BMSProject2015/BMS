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
namespace VIETTEL.Controllers.Luong
{
    public class Luong_DanhMucNgachLuongController : Controller
    {
        //
        // GET: /DanhMucNgachLuong/

        public string sViewPath = "~/Views/Luong/DanhMucNgachLuong/";

        [Authorize]
        public ActionResult Index(int? DanhMucNgachLuong_page)
        {
            Bang bang = new Bang("L_DanhMucNgachLuong");
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            ViewData["DanhMucNgachLuong_page"] = DanhMucNgachLuong_page;                        
            return View(sViewPath + "Luong_DanhMucNgachLuong_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(string iID_MaNgachLuong)
        {
            Bang bang = new Bang("L_DanhMucNgachLuong");
            bang.GiaTriKhoa = iID_MaNgachLuong;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
            if (dicData != null)
            {
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "Detail.aspx");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string iID_MaNgachLuong)
        {
            Bang bang = new Bang("L_DanhMucNgachLuong");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = iID_MaNgachLuong;
            bang.Delete();
            return RedirectToAction("Index", "Luong_DanhMucNgachLuong");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaNgachLuong)
        {

            ViewData["iID_MaNgachLuong"] = iID_MaNgachLuong;

            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaNgachLuong)==false)
            {
               
                ViewData["DuLieuMoi"] = "0";
                data = LuongModels.LayThongTinNgachLuong(iID_MaNgachLuong);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            return View(sViewPath + "Luong_DanhMucNgachLuong_Edit.aspx");
            
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            Bang bang = new Bang("L_DanhMucNgachLuong");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

           String iID_MaNgachLuong = Request.Form[ParentID + "_iID_MaNgachLuong"];
            iID_MaNgachLuong = iID_MaNgachLuong.Trim();
            String sTenNgachLuong = Request.Form[ParentID + "_sTenNgachLuong"];
            if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaNgachLuong, "iID_MaNgachLuong", iID_MaNgachLuong, bang.DuLieuMoi))
            {
                arrLoi.Add("err_iID_MaNgachLuong", "Không được nhập trùng ký hiệu");
            }

            if (String.IsNullOrEmpty(iID_MaNgachLuong))
            {
                arrLoi.Add("err_iID_MaNgachLuong", "Bạn chưa nhập ký hiệu");
            }

            if (String.IsNullOrEmpty(sTenNgachLuong))
            {
                arrLoi.Add("err_sTenNgachLuong", "Bạn chưa nhập ngạch lương");
            }


            if (arrLoi.Count == 0)
            {
                bang.GiaTriKhoa = iID_MaNgachLuong;
                bang.Save();
                return RedirectToAction("Index", "Luong_DanhMucNgachLuong");
            }
            else
            {

                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData["DuLieuMoi"] =Convert.ToInt16(bang.DuLieuMoi);
                ViewData["iID_MaNgachLuong"] = iID_MaNgachLuong;
                ViewData["data"] = dicData["data"];
                return View(sViewPath + "Luong_DanhMucNgachLuong_Edit.aspx");
            }
        }

    }
}
