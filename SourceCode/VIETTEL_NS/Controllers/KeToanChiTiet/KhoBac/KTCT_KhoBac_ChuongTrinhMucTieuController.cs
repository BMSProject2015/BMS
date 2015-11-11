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


namespace VIETTEL.Controllers.KeToanChiTiet.KhoBac
{
    public class KTCT_KhoBac_ChuongTrinhMucTieuController : Controller
    {
        //
        // GET: /KTCT_KhoBac_ChuongTrinhMucTieu/
        public string sViewPath = "~/Views/KeToanChiTiet/KhoBac/ChuongTrinhMucTieu/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_ChuongTrinhMucTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "ChuongTrinhMucTieu_Index.aspx");
        }

        [Authorize]
        public ActionResult Edit(String iID_MaChuongTrinhMucTieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_ChuongTrinhMucTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChuongTrinhMucTieu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaChuongTrinhMucTieu"] = iID_MaChuongTrinhMucTieu;
            return View(sViewPath + "ChuongTrinhMucTieu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChuongTrinhMucTieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_ChuongTrinhMucTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sMaChuongTrinhMucTieu = Convert.ToString(Request.Form[ParentID + "_iiID_MaChuongTrinhMucTieu"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);

            if (sMaChuongTrinhMucTieu == string.Empty || sMaChuongTrinhMucTieu == "")
            {
                arrLoi.Add("err_iiID_MaChuongTrinhMucTieu", "Bạn chưa nhập mã chương trình mục tiêu!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (CheckMaChuongTrinhMucTieu(sMaChuongTrinhMucTieu) == true)
                {
                    arrLoi.Add("err_iiID_MaChuongTrinhMucTieu", "Mã chương trình mục tiêu đã tồn tại!");
                }
            }
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên chương trình mục tiêu!");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaChuongTrinhMucTieu"] = sMaChuongTrinhMucTieu;
                return View(sViewPath + "ChuongTrinhMucTieu_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("KT_ChuongTrinhMucTieu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = sMaChuongTrinhMucTieu;
                bang.Save();
                return View(sViewPath + "ChuongTrinhMucTieu_Index.aspx");
            }
        }

        public Boolean CheckMaChuongTrinhMucTieu(String iID_MaChuongTrinhMucTieu)
        {
            Boolean vR = false;
            DataTable dt = KTCT_KhoBac_ChuongTrinhMucTieuModels.Get_RowChuongTrinhMucTieu(iID_MaChuongTrinhMucTieu);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            return vR;
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChuongTrinhMucTieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_ChuongTrinhMucTieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("KT_ChuongTrinhMucTieu");
            bang.GiaTriKhoa = iID_MaChuongTrinhMucTieu;
            bang.Delete();
            return View(sViewPath + "ChuongTrinhMucTieu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_ChuongTrinhMucTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "ChuongTrinhMucTieu_Sort.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaTaiKhoan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_ChuongTrinhMucTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("KT_ChuongTrinhMucTieu");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
