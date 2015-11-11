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
    public class QLDA_DonViThiCongController : Controller
    {
        //
        // GET: /QLDA_DonViThiCong/
        public string sViewPath = "~/Views/QLDA/DanhMuc/DonViThiCong/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DonViThiCong", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_DonViThiCong_Index.aspx");
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
            String sMaDonViThiCong = Request.Form[ParentID + "_sMaDonViThiCong"];
            String sTenDonViThiCong = Request.Form[ParentID + "_sTenDonViThiCong"];

            return RedirectToAction("Index", "QLDA_DonViThiCong", new { sMaDonViThiCong = sMaDonViThiCong, sTenDonViThiCong = sTenDonViThiCong });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaDonViThiCong)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaDonViThiCong))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDonViThiCong"] = iID_MaDonViThiCong;
            return View(sViewPath + "QLDA_DonViThiCong_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DonViThiCong", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sMaDonViThiCong = Convert.ToString(Request.Form[ParentID + "_iID_MaDonViThiCong"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            if (sMaDonViThiCong == "" && String.IsNullOrEmpty(sMaDonViThiCong) == true)
            {
                arrLoi.Add("err_iID_MaDonViThiCong", "Bạn phải chọn nhập mã nhà thầu!");
            }
            if (sTen == "" && String.IsNullOrEmpty(sTen) == true)
            {
                arrLoi.Add("err_sTen", "Bạn phải nhập tên nhà thầu!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && sMaDonViThiCong != "")
            {
                if (CheckMaTrung(sMaDonViThiCong) == true)
                {
                    arrLoi.Add("err_iID_MaDonViThiCong", "Mã nhà thầu đã tồn tại!");
                }
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaDonViThiCong"] = sMaDonViThiCong;
                return View(sViewPath + "QLDA_DonViThiCong_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("QLDA_DonViThiCong");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = sMaDonViThiCong;
                bang.Save();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaDonViThiCong)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DonViThiCong", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("QLDA_DonViThiCong");
            bang.GiaTriKhoa = iID_MaDonViThiCong;
            bang.Delete();
            return View(sViewPath + "QLDA_DonViThiCong_Index.aspx");
        }
        public Boolean CheckMaTrung(String iID_MaDonViThiCong)
        {
            Boolean vR = false;
            DataTable dt = QLDA_DonViThiCongModels.Get_Row_Data(iID_MaDonViThiCong);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }
        public JsonResult get_MaChuDauTu(String iID_MaDonViThiCong)
        {
            return Json(get_objCheckMaTrung(iID_MaDonViThiCong), JsonRequestBehavior.AllowGet);
        }
        public static String get_objCheckMaTrung(String iID_MaDonViThiCong)
        {
            String strMess = "";
            Boolean vR = false;
            DataTable dt = QLDA_DonViThiCongModels.Get_Row_Data(iID_MaDonViThiCong);
            if (dt.Rows.Count > 0)
            {
                strMess = "Mã nhà thầu đã tồn tại!";
            }
            else
            {
                strMess = "Mã này có thể thêm được!";
            }
            return strMess;
        }
    }
}
