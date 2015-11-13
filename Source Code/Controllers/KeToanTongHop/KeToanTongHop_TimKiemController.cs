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
    public class KeToanTongHop_TimKiemController : Controller
    {
        //
        // GET: /KeToanTongHop_TimKiem/
        public string sViewPath = "~/Views/KeToanTongHop/TimKiem/";
        public ActionResult Index()
        {
            return View(sViewPath + "KeToanTongHop_TimKiem_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String iSoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iNgayCT = Request.Form[ParentID + "_iNgayCT"];
            String iThangCT = Request.Form[ParentID + "_iThangCT"];
            String iNgay = Request.Form[ParentID + "_iNgay"];
            String iThang = Request.Form[ParentID + "_iThang"];
            String sSoTienTu = Request.Form[ParentID + "_iSoTienTu"];
            String sSoTienDen = Request.Form[ParentID + "_iSoTienDen"];
            String sTaiKhoanNo = Request.Form[ParentID + "_sTaiKhoanNo"];
            String sTaiKhoanCo = Request.Form[ParentID + "_sTaiKhoanCo"];
            String sDonViNo = Request.Form[ParentID + "_sDonViNo"];
            String sDonViCo = Request.Form[ParentID + "_sDonViCo"];
            String sNoiDung = Request.Form[ParentID + "_sNoiDung"];

            String iDenNgayCT = Request.Form[ParentID + "_iDenNgayCT"];


            String iDenThangCT = Request.Form[ParentID + "_iDenThangCT"];
            String iDenNgay = Request.Form[ParentID + "_iDenNgay"];
            String iDenThang = Request.Form[ParentID + "_iDenThang"];
            String sNguoiTao = Request.Form[ParentID + "_sNguoiTao"];
            String sChiTietCo = Request.Form[ParentID + "_sChiTietCo"];
            String sChiTietNo = Request.Form[ParentID + "_sChiTietNo"];
            String sBNo = Request.Form[ParentID + "_sBNo"];
            String sBCo = Request.Form[ParentID + "_sBCo"];

            return RedirectToAction("Index", "KeToanTongHop_TimKiem", new
                                                                          {
                                                                              iSoChungTu = iSoChungTu,
                                                                              iNgayCT = iNgayCT,
                                                                              iThangCT = iThangCT,
                                                                              iNgay = iNgay,
                                                                              iThang = iThang,
                                                                              sSoTienTu = sSoTienTu,
                                                                              sSoTienDen = sSoTienDen,
                                                                              sTaiKhoanNo = sTaiKhoanNo,
                                                                              sTaiKhoanCo = sTaiKhoanCo,
                                                                              sDonViNo = sDonViNo,
                                                                              sDonViCo = sDonViCo,
                                                                              sNoiDung = sNoiDung,

                                                                              iDenNgayCT = iDenNgayCT,
                                                                              iDenThangCT = iDenThangCT,
                                                                              iDenNgay = iDenNgay,
                                                                              iDenThang = iDenThang,
                                                                              sNguoiTao = sNguoiTao,
                                                                              sChiTietCo = sChiTietCo,
                                                                              sChiTietNo = sChiTietNo,
                                                                              sBNo = sBNo,
                                                                              sBCo = sBCo
                                                                          });
        }
        public JsonResult Get_objNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        {
            return Json(get_sNgayThang(ParentID, MaND, iThang, iNgay), JsonRequestBehavior.AllowGet);
        }
        public String get_sNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        {
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            if (iThang == "-1")
            {
                iThang = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
            }

            dtCauHinh.Dispose();
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), true);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            String S = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:55px;padding:2px;border:1px solid #dedede;\"");
            dtNgay.Dispose();

            return S;
        }
    }
}
