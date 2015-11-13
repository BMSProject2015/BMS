using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using DomainModel.Abstract;
using System.Data.SqlClient;
using System.Collections.Specialized;
using VIETTEL.Models;


namespace VIETTEL.Controllers.DungChung
{
    public class DanhMucChuKyController : Controller
    {
        //
        // GET: /DanhMucChuKy/
        public string sViewPath = "~/Views/DungChung/DanhMucChuKy/";

        [Authorize]
        public ActionResult Index(int? DanhMucChuKy_page, String ThongBao)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("NS_DanhMucChuKy");
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                ViewData["DanhMucChuKy_page"] = DanhMucChuKy_page;
                ViewData["ThongBao"] = ThongBao;
                return View(sViewPath + "DanhMucChuKy_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string iID_MaChuKy)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("NS_DanhMucChuKy");
                bang.GiaTriKhoa = iID_MaChuKy;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
                if (dicData != null)
                {
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Detail.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string iID_MaChuKy, int? page)
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Boolean DuocXoa = true;
                for (int i = 1; i <= 5; i++)
                {
                    if (HamChung.Checked_Delete("NS_DanhMuc_BaoCao_ChuKy", "iID_MaChucDanh" + i, iID_MaChuKy) == false ||
                        HamChung.Checked_Delete("NS_DanhMuc_BaoCao_ChuKy", "iID_MaThuaLenh" + i, iID_MaChuKy) == false ||
                        HamChung.Checked_Delete("NS_DanhMuc_BaoCao_ChuKy", "iID_MaTen" + i, iID_MaChuKy) == false)
                    {
                        DuocXoa = false;
                        break;
                    }
                }

                Bang bang = new Bang("NS_DanhMucChuKy");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = iID_MaChuKy;
                String ThongBao = "";
                if (DuocXoa)
                {
                    bang.Delete();
                }
                else
                {
                    ThongBao = "Không được xóa chữ ký vì chữ ký này đang được sử dụng trong báo cáo";
                }
                ViewData["DanhMucChuKy_page"] = page;
                ViewData["ThongBao"] = ThongBao;
                return View(sViewPath + "DanhMucChuKy_Index.aspx");

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaChuKy, int? page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {

                ViewData["iID_MaChuKy"] = iID_MaChuKy;

                NameValueCollection data = new NameValueCollection();
                if (String.IsNullOrEmpty(iID_MaChuKy) == false)
                {

                    ViewData["DuLieuMoi"] = "0";
                    data = DanhMucChuKyModels.LayThongTinChuKy(iID_MaChuKy);
                }
                else
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["data"] = data;
                return View(sViewPath + "DanhMucChuKy_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChuKy)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("NS_DanhMucChuKy");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

                String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
                String sChuKy = Request.Form[ParentID + "_sChuKy"];
                if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaChuKy, "sKyHieu", sKyHieu, bang.DuLieuMoi))
                {
                    arrLoi.Add("err_sKyHieu", "Không được nhập trùng ký hiệu");
                }

                if (arrLoi.Count == 0)
                {
                    bang.Save();
                    return RedirectToAction("Index", "DanhMucChuKy");
                }
                else
                {

                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }

                    Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                    ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                    ViewData["iID_MaChuKy"] = iID_MaChuKy;
                    ViewData["data"] = dicData["data"];
                    return View(sViewPath + "DanhMucChuKy_Edit.aspx");
                }

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
