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
    public class LoaiTaiSanController : Controller
    {
        //
        // GET: /LoaiTaiSan/
        public string sViewPath = "~/Views/DungChung/LoaiTS/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "LoaiTaiSan_Index.aspx");
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
        public ActionResult Edit(String iID_MaLoaiTaiSan)
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
            return View(sViewPath + "LoaiTaiSan_Edit.aspx");
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
        public ActionResult EditSubmit(String ParentID, String iID_MaLoaiTaiSan)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String MaLoaiTaiSan = Convert.ToString(Request.Form[ParentID + "_MaLoaiTaiSan"]);
            String sKyHieu = Convert.ToString(Request.Form[ParentID + "_sKyHieu"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            if (sKyHieu == "" && String.IsNullOrEmpty(sKyHieu) == true)
            {
                arrLoi.Add("err_sKyHieu",MessageModels.sKyHieuTaiSan);
            }
            if (sTen == "" && String.IsNullOrEmpty(sTen) == true)
            {
                arrLoi.Add("err_sTen", MessageModels.sTenTaiSan);
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
                return View(sViewPath + "LoaiTaiSan_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("KTCS_LoaiTaiSan");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = MaLoaiTaiSan;
                bang.Save();
                return View(sViewPath + "LoaiTaiSan_Edit.aspx");
            }

             }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        ///Xóa loại 
        /// </summary>
        /// <param name="iID_MaLoaiTaiSan"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaLoaiTaiSan)
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {

            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("KTCS_LoaiTaiSan");
            bang.TruongKhoa = "iID_MaLoaiTaiSan";
            bang.GiaTriKhoa = iID_MaLoaiTaiSan;
            bang.Delete();
            return View(sViewPath + "LoaiTaiSan_Index.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
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
                return RedirectToAction("Index", "LoaiTaiSan", new
                                                                   {
                                                                       KyHieu = sKyHieu.Trim(),
                                                                       Ten = sTen.Trim()
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
        /// <param name="TKNo"></param>
        /// <param name="TKCo"></param>
        /// <param name="LoaiTT"></param>
        /// <param name="LoaiNS"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult get_List(String KyHieu, String Ten, String page)
        {

            LoaiTaiSanModels ModelData = new LoaiTaiSanModels(KyHieu, Ten, page);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/DungChung/LoaiTS/LoaiTaiSan_List.ascx", ModelData);

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
