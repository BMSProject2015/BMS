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

namespace VIETTEL.Controllers.DungChung
{
    public class DanhMucNganhController : Controller
    {


        public string sViewPath = "~/Views/DungChung/DanhMucNganh/";

        public ActionResult Index(int? page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "DanhMucNganh_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult Edit(String iID, int? page)
        {

            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID"] = iID;
            return View(sViewPath + "DanhMucNganh_Edit.aspx");
        }

        public ActionResult EditSubmit(String ParentID, String iID, int? page)
        {


            Bang bang = new Bang("NS_MucLucNganSach_Nganh");
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            String iID_MaNganhMLNS = Request.Form["iID_MaNganhMLNS"];
            String sMaNguoiQuanLy = Request.Form["sMaNguoiQuanLy"];
            String iID_MaNganh = Request.Form[ParentID + "_iID_MaNganh"];
            if (String.IsNullOrEmpty(iID_MaNganh))
            {
                arrLoi.Add("err_iID_MaNganh", "Mã ngành không được để trống");
            }
            //chi xet truong hop them moi
            if (Request.Form["DuLieuMoi"] == "1")
            {
                if (MucLucNganSach_NganhModels.CheckTonTaiMaNganh(iID_MaNganh))
                {
                    arrLoi.Add("err_iID_MaNganh", "Mã ngành đã tồn tại");
                }
            }
            if (arrLoi.Count == 0)
            {
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNganhMLNS", iID_MaNganhMLNS);
                bang.CmdParams.Parameters.AddWithValue("@sMaNguoiQuanLy", sMaNguoiQuanLy);
                if (Request.Form["DuLieuMoi"] == "1")
                {

                }
                else
                {
                    bang.GiaTriKhoa = iID;
                    bang.DuLieuMoi = false;
                }
                bang.Save();
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID"] = iID;
                return View(sViewPath + "DanhMucNganh_Edit.aspx");
            }

            return RedirectToAction("index", new { page = page });

        }

        public ActionResult Delete(string iID, int? page)
        {
            Bang bang = new Bang("NS_MucLucNganSach_Nganh");
            bang.GiaTriKhoa = iID;
            bang.IPSua = Request.UserHostAddress;
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.Delete();
            return RedirectToAction("index", new { page = page });
        }



    }
}
