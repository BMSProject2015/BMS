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
    public class KeToanTongHopController : Controller
    {
        //
        // GET: /KeToanTongHop/
        public string sViewPath = "~/Views/KeToanTongHop/ChungTuChiTiet/";

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_ChungTu_Detail.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public JsonResult get_List_KeToanTongHop_ChungTuChiTiet(String iID_MaChungTu, String MaND)
        {
            KeToanTongHopListModels ModelData = new KeToanTongHopListModels(iID_MaChungTu, MaND);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_DanhSach.ascx", ModelData);

            return Json(strList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
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

        [Authorize]
        public JavaScriptResult Edit_Fast_ChungTu_Submit(String ParentID, String OnSuccess)
        {
            String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iTapSo = Convert.ToString(Request.Form[ParentID + "_sTapSo"]);
            String sDonVi = Convert.ToString(Request.Form[ParentID + "_sDonVi"]);
            String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);

            String MaND = User.Identity.Name;
            String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            //Insert into data vào bảng: KT_ChungTu
            Bang bang = new Bang("KT_ChungTu");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(KeToanTongHopModels.iID_MaPhanHe));
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBanNguoiDung);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
            bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
            bang.CmdParams.Parameters.AddWithValue("@iThang", iThang);
            bang.CmdParams.Parameters.AddWithValue("@iTapSo", iTapSo);
            bang.CmdParams.Parameters.AddWithValue("@sDonVi", sDonVi);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", sNoiDung);
            bang.CmdParams.Parameters.AddWithValue("@dNgayChungTu", DateTime.Now);
            String MaChungTuAddNew = Convert.ToString(bang.Save());
            KeToanTongHop_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);

            String strJ = "";
            if (String.IsNullOrEmpty(OnSuccess) == false)
            {
                strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
            }
            else
            {
                strJ = String.Format("Dialog_close('{0}');", ParentID);
            }
            return JavaScript(strJ);
        }

        [Authorize]
        public JsonResult UpdateCauHinhNamLamViec(String MaND, String iThangLamViec, String iNamLamViec)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (CommonFunction.IsNumeric(iThangLamViec))
            {
                DK = " iThangLamViec=@iThangLamViec";
                cmd.Parameters.AddWithValue("@iThangLamViec", iThangLamViec);
            }
            if (CommonFunction.IsNumeric(iNamLamViec))
            {
                if (String.IsNullOrEmpty(DK) == false)
                {
                    DK = DK + ",iNamLamViec=@iNamLamViec ";
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                }
                else
                {
                    DK = " iNamLamViec=@iNamLamViec ";
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                }
            }

            if (String.IsNullOrEmpty(DK) == false)
            {
                String SQL = String.Format("UPDATE DC_NguoiDungCauHinh SET {0} WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao", DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                Connection.UpdateDatabase(cmd);
            }
            cmd.Dispose();
            String strJ = "";
            
                strJ = String.Format("Dialog_close(location_reload();)");


          return Json("a", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult SoDoLuong()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_SoDoLuong.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult GiaiThichTaiKhoan_DauNam()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_GiaiThichTaiKhoan_DauNam.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
