using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using VIETTEL.Models;
namespace VIETTEL.Controllers.DungChung
{
    public class PhanHe_TrangThaiDuyet_NhomNguoiDungController : Controller
    {
        //
        // GET: /PhanHe_TrangThaiDuyet_NhomNguoiDung/
        public string sViewPath = "~/Views/DungChung/PhanHe_TrangThaiDuyet_NhomNguoiDung/";

        public ActionResult Loc(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String MaPhanHe = Request.Form[ParentID + "_MaPhanHe"];
                String MaNhomNguoiDung = Request.Form[ParentID + "_MaNhomNguoiDung"];
                return RedirectToAction("Edit", "PhanHe_TrangThaiDuyet_NhomNguoiDung",
                                        new {MaPhanHe = MaPhanHe, MaNhomNguoiDung = MaNhomNguoiDung});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult Loc_Index(String ParentID)
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name) || HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
                String MaNhomNguoiDung = Request.Form[ParentID + "_iID_MaNhomNguoiDung"];
                return RedirectToAction("Index", "PhanHe_TrangThaiDuyet_NhomNguoiDung",
                                        new {MaPhanHe = MaPhanHe, MaNhomNguoiDung = MaNhomNguoiDung});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet_NhomNguoiDung", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "PhanHe_TrangThaiDuyet_NhomNguoiDung_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet_NhomNguoiDung", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }            
            return View(sViewPath + "PhanHe_TrangThaiDuyet_NhomNguoiDung_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaPhanHe_TrangThaiDuyet_Xem)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet_NhomNguoiDung", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String MaPhanHe = Convert.ToString(Request.Form[ParentID + "_iID_MaPhanHe"]);
            String MaNhomNguoiDung = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomNguoiDung"]);
            String MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            if (MaPhanHe == Convert.ToString(Guid.Empty) || MaPhanHe == "-1")
            {
                arrLoi.Add("err_iID_MaPhanHe", "Bạn chưa chọn phân hệ");
            }
            if (MaNhomNguoiDung == Convert.ToString(Guid.Empty) || MaNhomNguoiDung == "")
            {
                arrLoi.Add("err_iID_MaNhomNguoiDung", "Bạn chưa chọn nhóm người dùng");
            }
             
            //if (MaTrangThaiDuyet == Convert.ToString(Guid.Empty) || MaTrangThaiDuyet == "-1")
            //{
            //    arrLoi.Add("err_iID_MaTrangThaiDuyet", "Bạn chưa chọn trạng thái duyệt");
            //}
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaPhanHe_TrangThaiDuyet_Xem"] = MaPhanHe_TrangThaiDuyet_Xem;
                return View(sViewPath + "PhanHe_TrangThaiDuyet_NhomNguoiDung_Edit.aspx");
            }
            else
            {
                String sMaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
                String[] arrMaTrangThaiDuyet = sMaTrangThaiDuyet.Split(',');
                String SQL = "DELETE NS_PhanHe_TrangThaiDuyet_NhomNguoiDung WHERE iID_MaPhanHe=@iID_MaPhanHe AND iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaPhanHe", MaPhanHe);
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                for (int i = 0; i < arrMaTrangThaiDuyet.Length; i++)
                {
                    Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet_NhomNguoiDung");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;                    
                    bang.DuLieuMoi = true;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanHe",MaPhanHe);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", arrMaTrangThaiDuyet[i]);
                    bang.Save();
                }

                return RedirectToAction("Edit", "PhanHe_TrangThaiDuyet_NhomNguoiDung", new { MaPhanHe = MaPhanHe, MaNhomNguoiDung = MaNhomNguoiDung });
            }
        }
        [Authorize]
        public ActionResult Delete(String MaPhanHe_TrangThaiDuyet_Xem)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_PhanHe_TrangThaiDuyet_NhomNguoiDung", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("NS_PhanHe_TrangThaiDuyet_NhomNguoiDung");
            bang.GiaTriKhoa = MaPhanHe_TrangThaiDuyet_Xem;
            bang.Delete();
            return View(sViewPath + "PhanHe_TrangThaiDuyet_NhomNguoiDung_Index.aspx");
        }
    }
}
