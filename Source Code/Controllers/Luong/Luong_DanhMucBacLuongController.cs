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
    public class Luong_DanhMucBacLuongController : Controller
    {
        //
        // GET: /DanhMucBacLuong/

        public string sViewPath = "~/Views/Luong/DanhMucBacLuong/";

        [Authorize]
        public ActionResult Index(int? DanhMucBacLuong_page)
        {
            Bang bang = new Bang("L_DanhMucBacLuong");
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            ViewData["DanhMucBacLuong_page"] = DanhMucBacLuong_page;
            return View(sViewPath + "Luong_DanhMucBacLuong_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(string iID_MaBacLuong)
        {
            Bang bang = new Bang("L_DanhMucBacLuong");
            bang.GiaTriKhoa = iID_MaBacLuong;
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
        public ActionResult Delete(string iID_MaBacLuong)
        {
            Bang bang = new Bang("L_DanhMucBacLuong");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = iID_MaBacLuong;
            bang.Delete();
            return RedirectToAction("Index", "Luong_DanhMucBacLuong");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaBacLuong, String iID_MaNgachLuong)
        {

            ViewData["iID_MaBacLuong"] = iID_MaBacLuong;
            ViewData["iID_MaNgachLuong"] = iID_MaNgachLuong;
            

            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaBacLuong) == false)
            {

                ViewData["DuLieuMoi"] = "0";
                data = LuongModels.LayThongTinBacLuong(iID_MaBacLuong, iID_MaNgachLuong);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            return View(sViewPath + "Luong_DanhMucBacLuong_Edit.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            Bang bang = new Bang("L_DanhMucBacLuong");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

            String iID_MaBacLuong = Request.Form[ParentID + "_iID_MaBacLuong"];
            iID_MaBacLuong = iID_MaBacLuong.Trim();
            String sTenBacLuong = Request.Form[ParentID + "_sTenBacLuong"];
            if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaBacLuong, "iID_MaBacLuong", iID_MaBacLuong, bang.DuLieuMoi))
            {
                arrLoi.Add("err_iID_MaBacLuong", "Không được nhập trùng ký hiệu");
            }
            if (String.IsNullOrEmpty(iID_MaBacLuong))
            {
                arrLoi.Add("err_iID_MaBacLuong", "Bạn chưa nhập ký hiệu");
            }
            if (String.IsNullOrEmpty(sTenBacLuong))
            {
                arrLoi.Add("err_sTenBacLuong", "Bạn chưa nhập bậc lương");
            }


            if (arrLoi.Count == 0)
            {
                bang.GiaTriKhoa = iID_MaBacLuong;
                SqlCommand cmd = new SqlCommand();
                String SQL = "UPDATE L_DanhMucBacLuong SET sTenBacLuong=@sTenBacLuong,rHeSoLuong=@rHeSoLuong,rHeSo_ANQP=@rHeSo_ANQP,sQuanHam=@sQuanHam WHERE iID_MaBacLuong=@iID_MaBacLuong AND iID_MaNgachLuong=@iID_MaNgachLuong";

                for (int i = 0; i < bang.CmdParams.Parameters.Count; i++)
                {
                    cmd.Parameters.AddWithValue(bang.CmdParams.Parameters[i].ParameterName, bang.CmdParams.Parameters[i].Value);
                }
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
                cmd.CommandText = SQL;
                Connection.UpdateDatabase(cmd);
                return RedirectToAction("Index", "Luong_DanhMucBacLuong");
            }
            else
            {

                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                ViewData["iID_MaBacLuong"] = iID_MaBacLuong;
                ViewData["data"] = dicData["data"];
                return View(sViewPath + "Luong_DanhMucBacLuong_Edit.aspx");
            }
        }
    }
}
