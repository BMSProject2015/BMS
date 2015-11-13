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
    public class TuDienController : Controller
    {
        //
        // GET: /TuDien/
        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/TuDien/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TuDien", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "TuDien_Index.aspx");
        }

        public JsonResult get_TuDien(String iID_MaTaiKhoanGoc, String iNam)
        {
            return Json(TuDienModels.get_TuDien(iID_MaTaiKhoanGoc, iNam), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Thêm từ điển
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Add(String iID_MaTaiKhoanGoc)
        {
            ViewData["DuLieuMoi"] = "0";          
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoanGoc;
            return View(sViewPath + "TuDien_Edit.aspx");
        }
        /// <summary>
        /// Sửa từ điển
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaTuDien, String iID_MaTaiKhoanGoc)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaTuDien))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTuDien"] = iID_MaTuDien;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoanGoc;
            return View(sViewPath + "TuDien_Edit.aspx");
        }
        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TuDien", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaTaiKhoanGoc = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanGoc"]);
            if ((iID_MaTaiKhoanGoc == "" || iID_MaTaiKhoanGoc == Convert.ToString(Guid.Empty)))
            {
                arrLoi.Add("err_iID_MaTaiKhoan", "Bạn phải chọn tài khoản gốc");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
              
                return RedirectToAction("Index", "TuDien");
            }
            else
            {
                return RedirectToAction("Edit", "TuDien", new { iID_MaTaiKhoanGoc = iID_MaTaiKhoanGoc });
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
        public ActionResult EditSubmit(String ParentID, String iID_MaTuDien)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TuDien", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaTaiKhoanGoc = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanGoc"]);
            String iID_MaTaiKhoanNo = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanNo"]);
            String iID_MaTaiKhoanCo = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanCo"]);
            String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
            if ((iID_MaTaiKhoanGoc == "" || iID_MaTaiKhoanGoc == Convert.ToString(Guid.Empty)) && (ViewData["DuLieuMoi"]=="0"))
            {
                arrLoi.Add("err_iID_MaTaiKhoan", "Bạn phải chọn tài khoản gốc!");
            }
            //if ((iID_MaTaiKhoanNo == "" || iID_MaTaiKhoanNo == Convert.ToString(Guid.Empty)) &&
            //    (iID_MaTaiKhoanCo == "" || iID_MaTaiKhoanCo == Convert.ToString(Guid.Empty)))
            //{
            //    arrLoi.Add("err_iID_MaTaiKhoanNo", "Bạn phải chọn tài khoản nợ hoặc có!");
            //}

            if ((iID_MaTaiKhoanNo == "" || iID_MaTaiKhoanNo == Convert.ToString(Guid.Empty)) || iID_MaTaiKhoanNo == "-1")
            {
                arrLoi.Add("err_iID_MaTaiKhoanNo", "Bạn phải chọn tài khoản nợ");
            }
            if ((iID_MaTaiKhoanCo == "" || iID_MaTaiKhoanCo == Convert.ToString(Guid.Empty)) || iID_MaTaiKhoanCo == "-1")
            {
                arrLoi.Add("err_iID_MaTaiKhoanCo", "Bạn phải chọn tài khoản có");
            }

            if (sNoiDung == "" && String.IsNullOrEmpty(sNoiDung) == true)
            {
                arrLoi.Add("err_sNoiDung", "Bạn phải nhập nội dung từ điển");
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaTuDien"] = iID_MaTuDien;
               // return View(sViewPath + "TuDien_Edit.aspx");
                return RedirectToAction("Edit", "TuDien", new { iID_MaTaiKhoanGoc = iID_MaTaiKhoanGoc });
            }
            else
            {
                Bang bang = new Bang("KT_TuDien");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (iID_MaTuDien == "" || iID_MaTuDien == Convert.ToString(Guid.Empty))
                    bang.GiaTriKhoa = Guid.NewGuid();
                else
                    bang.GiaTriKhoa = iID_MaTuDien;
                bang.Save();
                return RedirectToAction("Edit", "TuDien", new { iID_MaTaiKhoanGoc = iID_MaTaiKhoanGoc });
                //return View(sViewPath + "TuDien_Edit.aspx");
            }
        }
        /// <summary>
        ///Xóa loại từ điển
        /// </summary>
        /// <param name="iID_MaThongTri"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaTuDien)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TuDien", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("KT_TuDien");
            bang.TruongKhoa = "iID_MaTuDien";
            bang.GiaTriKhoa = iID_MaTuDien;
            bang.Delete();
            return View(sViewPath + "TuDien_Index.aspx");
        }
       

    }
}
