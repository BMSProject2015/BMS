using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.TuLieu
{
    public class TuLieu_ThuMucController : Controller
    {
        //
        // GET: /Vay No/
        public string sViewPath = "~/Views/TuLieu/thumuc/";

        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "Index.aspx");
        }

        [Authorize]
        public ActionResult EditThuMuc(string ID)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(ID))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["ID"] = ID;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmitThuMuc(String ParentID)
        {
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_ThuMucTaiLieu", sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            string ID = Convert.ToString(Request.Form[ParentID + "_ID"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            if (sTen == string.Empty || sTen == "" || sTen == null)
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập đường dẫn thư mục!");
            }

            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return RedirectToAction("Index", "TuLieu_ThuMuc");
            }
            else
            {

                Bang bang = new Bang("TL_ThuMucTaiLieu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (ID != "")
                    bang.GiaTriKhoa = ID;
                bang.Save();
                return RedirectToAction("Index", "TuLieu_ThuMuc");
            }


        }
        [Authorize]
        public ActionResult DeleteThuMuc(String ID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_ThuMucTaiLieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = TuLieuLichSuModels.XoaThuMuc(ID);
            return RedirectToAction("Index", "TuLieu_ThuMuc");
        }
    }
}
