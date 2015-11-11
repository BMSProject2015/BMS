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
    public class Luong_DanhMucThamSoController : Controller
    {
        //
        // GET: /DanhMucThamSo/
        public string sViewPath = "~/Views/Luong/DanhMucThamSo/";

        [Authorize]
        public ActionResult Index(int? DanhMucThamSo_page)
        {
            Bang bang = new Bang("L_DanhMucThamSo");
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            ViewData["DanhMucThamSo_page"] = DanhMucThamSo_page;
            return View(sViewPath + "Luong_DanhMucThamSo_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(string iID_MaThamSo)
        {
            Bang bang = new Bang("L_DanhMucThamSo");
            bang.GiaTriKhoa = iID_MaThamSo;
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
        public ActionResult Delete(string iID_MaThamSo)
        {
           
            String SQL = "UPDATE L_DanhMucThamSo SET iTrangThai=0 WHERE iID_MaThamSo=@iID_MaThamSo";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaThamSo", iID_MaThamSo);
            Connection.UpdateDatabase(cmd, User.Identity.Name, Request.UserHostAddress);

            return RedirectToAction("Index", "Luong_DanhMucThamSo");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaThamSo)
        {

            ViewData["iID_MaThamSo"] = iID_MaThamSo;

            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaThamSo) == false)
            {

                ViewData["DuLieuMoi"] = "0";
                data = LuongModels.LayThongTinThamSo(iID_MaThamSo);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            return View(sViewPath + "Luong_DanhMucThamSo_Edit.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            Bang bang = new Bang("L_DanhMucThamSo");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

            String iID_MaThamSo = Request.Form[ParentID + "_iID_MaThamSo"];
            iID_MaThamSo = iID_MaThamSo.Trim();
            String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
            String sThamSo = Request.Form[ParentID + "_sThamSo"];
            String dThoiGianApDung_BatDau = Request.Form[ParentID + "_vidThoiGianApDung_BatDau"];
            //if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaThamSo, "sKyHieu", sKyHieu, bang.DuLieuMoi))
            //{
            //    arrLoi.Add("err_sKyHieu", "Không được nhập trùng ký hiệu");
            //}
            if (String.IsNullOrEmpty(dThoiGianApDung_BatDau))
            {
                arrLoi.Add("err_dThoiGianApDung_BatDau", "Bạn chưa nhập ngày áp dụng");
            }

            if (String.IsNullOrEmpty(sKyHieu))
            {
                arrLoi.Add("err_sKyHieu", "Bạn chưa nhập ký hiệu");
            }
            if (String.IsNullOrEmpty(sThamSo))
            {
                arrLoi.Add("err_sThamSo", "Bạn chưa nhập tham số");
            }

            if (arrLoi.Count == 0)
            {
                if (bang.DuLieuMoi == false)
                    bang.DuLieuMoi = true;
                //bang.GiaTriKhoa = iID_MaThamSo;
                bang.Save();
                return RedirectToAction("Index", "Luong_DanhMucThamSo");
            }
            else
            {

                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                ViewData["iID_MaThamSo"] = iID_MaThamSo;
                ViewData["data"] = dicData["data"];
                return View(sViewPath + "Luong_DanhMucThamSo_Edit.aspx");
            }
        }

    }
}
