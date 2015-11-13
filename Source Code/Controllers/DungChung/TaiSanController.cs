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

namespace VIETTEL.Controllers.CongSan
{
    public class TaiSanController : Controller
    {
        //
        // GET: /LoaiTaiSan/
        public string sViewPath = "~/Views/DungChung/TaiSan/";

        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_TaiSan", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "TaiSan_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Thêm mới hoặc sửa chứng từ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaTaiSan)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaTaiSan))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaTaiSan"] = iID_MaTaiSan;
                return View(sViewPath + "TaiSan_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Cập nhật và lưu vào csdl
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaTaiSan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_TaiSan", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sKyHieu = Convert.ToString(Request.Form[ParentID + "_sKyHieu"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);

            String iID_MaLoaiTaiSan = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiTaiSan"]);
            String iID_MaDanhMuc = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMuc"]);
            if (sKyHieu == "" && String.IsNullOrEmpty(sKyHieu) == true)
            {
                arrLoi.Add("err_sKyHieu", MessageModels.sKyHieuTaiSan);
            }
            if (sTen == "" && String.IsNullOrEmpty(sTen) == true)
            {
                arrLoi.Add("err_sTen", MessageModels.sTenTaiSan);
            }


            if ((iID_MaLoaiTaiSan == "" && String.IsNullOrEmpty(iID_MaLoaiTaiSan) == true) ||
                (iID_MaLoaiTaiSan == Convert.ToString(Guid.Empty)))
            {
                arrLoi.Add("err_iID_MaLoaiTaiSan", MessageModels.sLoaiTS);
            }
            if ((iID_MaDanhMuc == "" && String.IsNullOrEmpty(iID_MaDanhMuc) == true) ||
                (iID_MaDanhMuc == Convert.ToString(Guid.Empty)))
            {
                arrLoi.Add("err_iID_MaDanhMuc", MessageModels.sDV);
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaTaiSan"] = iID_MaTaiSan;
                return View(sViewPath + "TaiSan_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("KTCS_TaiSan");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (iID_MaTaiSan == "" || iID_MaTaiSan == Convert.ToString(Guid.Empty))
                    bang.GiaTriKhoa = Guid.NewGuid();
                else
                    bang.GiaTriKhoa = iID_MaTaiSan;
                bang.Save();
                return View(sViewPath + "TaiSan_Edit.aspx");
            }
        }

        /// <summary>
        ///Xóa loại 
        /// </summary>
        /// <param name="iID_MaTaiSan"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaTaiSan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_TaiSan", "Delete") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("KTCS_TaiSan");
            bang.TruongKhoa = "iID_MaTaiSan";
            bang.GiaTriKhoa = iID_MaTaiSan;
            bang.Delete();
            return View(sViewPath + "TaiSan_Index.aspx");
        }

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
                String sTen = Request.Form[ParentID + "_sTen"];

                String LoaiTS = "", DV = "";
                //loại tài sản
                String iID_MaTaiSan = Request.Form[ParentID + "_iID_MaTaiSan"];
                if (iID_MaTaiSan != Convert.ToString(Guid.Empty)) LoaiTS = iID_MaTaiSan;

                //Đơn vị tính
                String iID_MaDanhMuc = Request.Form[ParentID + "_iID_MaDanhMuc"];
                if (iID_MaDanhMuc != Convert.ToString(Guid.Empty)) DV = iID_MaDanhMuc;

                return RedirectToAction("Index", "TaiSan", new
                                                               {
                                                                   KyHieu = sKyHieu.Trim(),
                                                                   Ten = sTen.Trim(),
                                                                   LoaiTS = LoaiTS.Trim(),
                                                                   DV = DV.Trim()
                                                               });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Chuyển hướng lấy danh sách
        /// </summary>
        /// <param name="KyHieu"></param>
        /// <param name="Ten"></param>
        /// <param name="LoaiTS"></param>
        /// <param name="DV"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult get_List(String KyHieu, String Ten, String LoaiTS, String DV, String page)
        {
            TaiSanModel ModelData = new TaiSanModel(KyHieu, Ten, LoaiTS, DV, page);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/DungChung/TaiSan/TaiSan_List.ascx", ModelData);

            return Json(strList, JsonRequestBehavior.AllowGet);
        }

        public string RenderPartialViewToStringLoad(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }


    }
}
