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
    public class PhanHe_TrangThaiDuyetController : Controller
    {
        //
        // GET: /PhanHe_TrangThaiDuyet/
        public string sViewPath = "~/Views/DungChung/PhanHe_TrangThaiDuyet/";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "PhanHe_TrangThaiDuyet_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String MaTrangThaiDuyet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(MaTrangThaiDuyet))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTrangThaiDuyet"] = MaTrangThaiDuyet;
            return View(sViewPath + "PhanHe_TrangThaiDuyet_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaTrangThaiDuyet)
        {

            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();

            String MaNhomNguoiDung = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomNguoiDung"]);
            String MaPhanHe = Convert.ToString(Request.Form[ParentID + "_iID_MaPhanHe"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sLoaiTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iLoaiTrangThaiDuyet"]);
            String sMauSac = Convert.ToString(Request.Form[ParentID + "_sMauSac"]);
            if (MaNhomNguoiDung == Convert.ToString(Guid.Empty) || MaNhomNguoiDung == "")
            {
                arrLoi.Add("err_iID_MaNhomNguoiDung", "Bạn chưa chọn mã nhóm người dùng");
            }
            if (MaPhanHe == Convert.ToString(Guid.Empty) || MaPhanHe == "-1")
            {
                arrLoi.Add("err_iID_MaPhanHe", "Bạn chưa chọn mã phân hệ");
            }
            if (MaPhanHe == Convert.ToString(Guid.Empty) || sLoaiTrangThaiDuyet == "-1")
            {
                arrLoi.Add("err_iLoaiTrangThaiDuyet", "Bạn chưa chọn loại trạng thái duyệt");
            }
            if (sTen == Convert.ToString(Guid.Empty) || sTen == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên trạng thái duyệt");
            }
            if (sMauSac == Convert.ToString(Guid.Empty) || sMauSac == "")
            {
                arrLoi.Add("err_sMauSac", "Bạn chưa nhập màu sắc");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaTrangThaiDuyet"] = MaTrangThaiDuyet;
                return View(sViewPath + "PhanHe_TrangThaiDuyet_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = MaTrangThaiDuyet;
                bang.Save();
                return RedirectToAction("Index", "PhanHe_TrangThaiDuyet", new {MaPhanHe = MaPhanHe});
            }
        }

        [Authorize]
        public ActionResult Loc(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
                return RedirectToAction("Index", "PhanHe_TrangThaiDuyet", new {MaPhanHe = MaPhanHe});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Delete(String MaTrangThaiDuyet)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet");
                bang.GiaTriKhoa = MaTrangThaiDuyet;
                bang.Delete();
                return View(sViewPath + "PhanHe_TrangThaiDuyet_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        // Edit Phan he

        [Authorize]
        public ActionResult Edit_PhanHe(String MaTrangThaiDuyet)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet", "Edit_PhanHe") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaTrangThaiDuyet))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaTrangThaiDuyet"] = MaTrangThaiDuyet;
                return View(sViewPath + "Edit_PhanHe.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_PhanHeSubmit(String ParentID, String MaTrangThaiDuyet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet", "Edit") == false ||
                !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.GiaTriKhoa = MaTrangThaiDuyet;
            bang.Save();
            return View(sViewPath + "PhanHe_TrangThaiDuyet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitEdit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
                String sMaTrangThai = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
                String[] arrMaTrangThai = sMaTrangThai.Split(',');

                for (int i = 0; i < arrMaTrangThai.Length; i++)
                {
                    String iID_MaTrangThaiDuyet_TuChoi =
                        Request.Form[arrMaTrangThai[i] + "_iID_MaTrangThaiDuyet_TuChoi"];
                    String iID_MaTrangThaiDuyet_TrinhDuyet =
                        Request.Form[arrMaTrangThai[i] + "_iID_MaTrangThaiDuyet_TrinhDuyet"];
                    String iID_MaNhomNguoiDung = Request.Form[arrMaTrangThai[i] + "_iID_MaNhomNguoiDung"];
                    Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    bang.DuLieuMoi = false;
                    bang.GiaTriKhoa = arrMaTrangThai[i];
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNhomNguoiDung", iID_MaNhomNguoiDung);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TuChoi", iID_MaTrangThaiDuyet_TuChoi);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet",
                                                           iID_MaTrangThaiDuyet_TrinhDuyet);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyetCapCaoNhat", -1);
                    bang.Save();

                }
                return RedirectToAction("Index", "PhanHe_TrangThaiDuyet", new {MaPhanHe = MaPhanHe});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Sort(String MaPhanHe)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["MaPhanHe"] = MaPhanHe;
                return View(sViewPath + "PhanHe_TrangThaiDuyet_Sort.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult SortSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
                String sMaTrangThai = Request.Form["iID_MaTrangThaiDuyet"];
                sMaTrangThai = sMaTrangThai.Remove(sMaTrangThai.Length - 1, 1);
                String[] arrMaTrangThai = sMaTrangThai.Split(',');
                for (int i = 0; i < arrMaTrangThai.Length; i++)
                {

                    String iSTT = Request.Form[arrMaTrangThai[i] + "_iSTT"];
                    Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    bang.DuLieuMoi = false;
                    bang.GiaTriKhoa = arrMaTrangThai[i];
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                    bang.Save();
                }
                return RedirectToAction("Index", "PhanHe_TrangThaiDuyet", new {MaPhanHe = MaPhanHe});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
