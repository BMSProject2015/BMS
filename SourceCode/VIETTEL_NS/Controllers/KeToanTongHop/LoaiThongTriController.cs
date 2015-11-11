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

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class LoaiThongTriController : Controller
    {
        //
        // GET: /LoaiThongTri/
        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/LoaiThongTri/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_LoaiThongTri", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "LoaiThongTri_Index.aspx");
        }
           /// <summary>
        /// Thêm mới hoặc sửa chứng từ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaThongTri)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaThongTri))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaThongTri"] = iID_MaThongTri;
            return View(sViewPath + "LoaiThongTri_Edit.aspx");
        }
        /// <summary>
        /// Cập nhật và lưu vào csdl
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaThongTri)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_LoaiThongTri", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaTaiKhoanNo = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanNo"]);
            String iID_MaTaiKhoanCo = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanCo"]);
            String sLoaiThongTri = Convert.ToString(Request.Form[ParentID + "_sLoaiThongTri"]);
            //if ((iID_MaTaiKhoanNo == "" || iID_MaTaiKhoanNo == Convert.ToString(Guid.Empty)) &&
            //    (iID_MaTaiKhoanCo == "" || iID_MaTaiKhoanCo == Convert.ToString(Guid.Empty)))
            //{
            //    arrLoi.Add("err_iID_MaTaiKhoanNo", "Bạn phải chọn tài khoản nợ hoặc có!");
            //}
            //if (sLoaiThongTri == "" && String.IsNullOrEmpty(sLoaiThongTri)==true)
            //{
            //    arrLoi.Add("err_sLoaiThongTri", "Bạn phải chọn loại thông tri!");
            //}
           
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaThongTri"] = iID_MaThongTri;              
                return View(sViewPath + "LoaiThongTri_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("KT_LoaiThongTri");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (iID_MaThongTri == "" || iID_MaThongTri == Convert.ToString(Guid.Empty))
                    bang.GiaTriKhoa = Guid.NewGuid();
                else
                    bang.GiaTriKhoa = iID_MaThongTri;
                bang.Save();              
                return View(sViewPath + "LoaiThongTri_Edit.aspx");
            }
        }
        /// <summary>
        ///Xóa loại thông tri
        /// </summary>
        /// <param name="iID_MaThongTri"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaThongTri)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_LoaiThongTri", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("KT_LoaiThongTri");
            bang.TruongKhoa = "iID_MaThongTri";
            bang.GiaTriKhoa = iID_MaThongTri;
            bang.Delete();
            return View(sViewPath + "LoaiThongTri_Index.aspx");
        }
        /// <summary>
        /// Tìm kiếm thông tri
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String TKNo = "", TKCo = "";
            String iID_MaTaiKhoanNo = Request.Form[ParentID + "_iID_MaTaiKhoanNo"];
            if (iID_MaTaiKhoanNo != Convert.ToString(Guid.Empty)) TKNo = iID_MaTaiKhoanNo;

            String iID_MaTaiKhoanCo = Request.Form[ParentID + "_iID_MaTaiKhoanCo"];
            if (iID_MaTaiKhoanCo != Convert.ToString(Guid.Empty)) TKNo = iID_MaTaiKhoanCo;
            String LoaiTT = Request.Form[ParentID + "_sLoaiThongTri"];
            String LoaiNS = Request.Form[ParentID + "_sTenLoaiNS"];
            return RedirectToAction("Index", "LoaiThongTri", new { 
                TKNo = TKNo.Trim(),
                TKCo = TKCo.Trim() ,
                LoaiTT = LoaiTT.Trim(),
                LoaiNS = LoaiNS.Trim() 
            });
        }
        /// <summary>
        /// Chuyển hướng lấy danh sách thông tri
        /// </summary>
        /// <param name="TKNo"></param>
        /// <param name="TKCo"></param>
        /// <param name="LoaiTT"></param>
        /// <param name="LoaiNS"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult get_List(String TKNo, String TKCo, String LoaiTT, String LoaiNS, String page, String UserName)
        {
            LoaiThongTriModels ModelData = new LoaiThongTriModels(TKNo, TKCo, LoaiTT, LoaiNS, page, UserName);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/KeToanTongHop/DanhMuc/LoaiThongTri/LoaiThongTri_List.ascx", ModelData);

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
