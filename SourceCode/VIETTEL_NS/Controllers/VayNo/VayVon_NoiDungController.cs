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

namespace VIETTEL.Controllers.DuToan
{
    public class VayVon_NoiDungController : Controller
    {
        //
        // GET: /Vay No/
        public string sViewPath = "~/Views/VayVon/";
        [Authorize]
        public ActionResult Index(string MaNoiDung, string TenNoiDung)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false) {
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            ViewData["iID_MaNoiDung"] = MaNoiDung;
            ViewData["sTenNoiDung"] = TenNoiDung;
            return View(sViewPath + "VayVon_NoiDung_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmitNoiDung(String ParentID, String MaNoiDung, String TenNoiDung)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            String iID_MaNoiDung = Request.Form[ParentID + "_" + "iID_MaNoiDung"];
            String sTenNoiDung = Request.Form[ParentID + "_" + "sTenNoiDung"];
            return RedirectToAction("Index", "VayVon_NoiDung", new {MaNoiDung = iID_MaNoiDung, TenNoiDung = sTenNoiDung});
        }

        [Authorize]
        public ActionResult EditNoiDung(string ID)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Edit") == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(ID))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["ID"] = ID;
            return View(sViewPath + "VayVon_NoiDung_ThemMoi.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmitNoiDung(String ParentID)
        {
            string sChucNang = "Edit";
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("DC_DanhMucNoiDung");
            //Kiểm tra quyền của người dùng với chức năng
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, bang.TenBang, sChucNang) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            string ID = Convert.ToString(Request.Form[ParentID + "_ID"]);
            Guid idNoiDung;
            if (ID == null || ID == string.Empty)
            {
                idNoiDung = Guid.NewGuid();
            }
            else
            {
                idNoiDung = new Guid(ID);
            }
            String iID_MaNoiDung = Convert.ToString(Request.Form[ParentID + "_iID_MaNoiDung"]);
            String sTenNoiDung = Convert.ToString(Request.Form[ParentID + "_sTenNoiDung"]);
            String sMoTaChung = Convert.ToString(Request.Form[ParentID + "_sMoTaChung"]);
            String sLoai = Convert.ToString(Request.Form[ParentID + "_iLoai"]);
            String sHoatDong = "0";
            if (Convert.ToString(Request.Form[ParentID + "_bHoatDong"]).Equals("on"))
            {
                sHoatDong = "1";
            }
            ;
            String dNgayTao = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayTao"];
            if (iID_MaNoiDung == string.Empty || iID_MaNoiDung == "" || iID_MaNoiDung == null)
            {
                arrLoi.Add("err_iID_MaNoiDung", "Bạn phải nhập mã nội dung!");
            }
            if (sTenNoiDung == string.Empty || sTenNoiDung == "" || sTenNoiDung == null)
            {
                arrLoi.Add("err_sTenNoiDung", "Bạn chưa nhập tên nội dung!");
            }
            if (sLoai == string.Empty || sLoai == "" || sLoai == null)
            {
                arrLoi.Add("err_sLoai", "Bạn chưa nhập loại nội dung!");
            }
            if (dNgayTao == string.Empty || dNgayTao == "" || dNgayTao == null)
            {
                arrLoi.Add("err_dNgayTao", "Bạn chưa nhập ngày tạo nội dung!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaNoiDung"] = iID_MaNoiDung;
                ViewData["sTenNoiDung"] = sTenNoiDung;
                ViewData["iLoai"] = sLoai;
                ViewData["sMoTaChung"] = sMoTaChung;
                ViewData["dNgayTao"] = dNgayTao;
                ViewData["bHoatDong"] = sHoatDong;
                return View(sViewPath + "VayVon_NoiDung_ThemMoi.aspx");
            }
            else
            {

                if (ID == null || ID == string.Empty)
                {
                    if (VayNoModels.CheckExistNoiDung(iID_MaNoiDung))
                    {
                        ViewData["ErrorKey"] = 1;
                        ViewData["iID_MaNoiDung"] = iID_MaNoiDung;
                        ViewData["sTenNoiDung"] = sTenNoiDung;
                        ViewData["iLoai"] = sLoai;
                        ViewData["sMoTaChung"] = sMoTaChung;
                        ViewData["dNgayTao"] = dNgayTao;
                        ViewData["bHoatDong"] = sHoatDong;
                        return View(sViewPath + "VayVon_NoiDung_ThemMoi.aspx");
                    }
                    else
                    {
                        StringBuilder strNoiDung = new StringBuilder();
                        strNoiDung.Append(iID_MaNoiDung);
                        strNoiDung.Append(",");
                        strNoiDung.Append(sTenNoiDung);
                        strNoiDung.Append(",");
                        strNoiDung.Append(sMoTaChung);
                        strNoiDung.Append(",");
                        strNoiDung.Append(sLoai);
                        strNoiDung.Append(",");
                        strNoiDung.Append(dNgayTao);
                        strNoiDung.Append(",");
                        strNoiDung.Append(sHoatDong);
                        VayNoModels.ThemMoiNoiDung(strNoiDung.ToString());
                        
                    }
                }
                else
                {
                    StringBuilder strNoiDung = new StringBuilder();
                    strNoiDung.Append(ID);
                    strNoiDung.Append(",");
                    strNoiDung.Append(iID_MaNoiDung);
                    strNoiDung.Append(",");
                    strNoiDung.Append(sTenNoiDung);
                    strNoiDung.Append(",");
                    strNoiDung.Append(sMoTaChung);
                    strNoiDung.Append(",");
                    strNoiDung.Append(sLoai);
                    strNoiDung.Append(",");
                    strNoiDung.Append(dNgayTao);
                    strNoiDung.Append(",");
                    strNoiDung.Append(sHoatDong);
                    VayNoModels.SuaNoiDung(strNoiDung.ToString());
                }

            }
            return RedirectToAction("Index", "VayVon_NoiDung", null);
            
        }

        [Authorize]
        public ActionResult DeleteNoiDung(String ID)
        {
            int iXoa = 0;
            iXoa = VayNoModels.XoaThongTinNoiDung(ID);
            return RedirectToAction("Index", "VayVon_NoiDung", new { MaNoiDung = string.Empty, TenNoiDung = string.Empty});
        }
    }
}
