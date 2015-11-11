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
namespace VIETTEL.Controllers.NguoiCoCong
{
    public class NguoiCoCong_ChungTuController : Controller
    {
        //
        // GET: /NguoiCoCong_ChungTu/
        public string sViewPath = "~/Views/NguoiCoCong/ChungTu/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "NguoiCoCong_ChungTu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iLoai)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Index", "NguoiCoCong_ChungTu", new { iLoai = iLoai, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        [Authorize]
        public ActionResult Edit(String iID_MaChungTu, String iLoai)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(NguoiCoCongModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "DT_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iLoai"] = iLoai;
            ViewData["MaChungTu"] = iID_MaChungTu;
            return View(sViewPath + "NguoiCoCong_ChungTu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaChungTu)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(NguoiCoCongModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("NCC_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iLoai = Convert.ToString(Request.Form[ParentID + "_iLoai"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sDSLNS"]);
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String bLoaiThang_Quy = Request.Form[ParentID + "_ThangQuy"];
            if (bLoaiThang_Quy == "Quy") bLoaiThang_Quy = "1";
            else bLoaiThang_Quy = "0";
            String optThangQuy = Convert.ToString(Request.Form[ParentID + "_ThangQuy"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (sLNS == string.Empty || sLNS == "" || sLNS == null)
            {
                arrLoi.Add("err_sDSLNS", "Bạn chưa chọn loại ngân sách!");
            }
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            switch (optThangQuy)
            { 
                case "Thang":
                    if (iThang == string.Empty || iThang == "-1" || iThang == null)
                    {
                        arrLoi.Add("err_ThangQuy", "Bạn chưa chọn tháng!");
                    }
                    break;
                case "Quy":
                    if (iQuy == string.Empty || iQuy == "" || iQuy == null)
                    {
                        arrLoi.Add("err_ThangQuy", "Bạn chưa chọn quý!");
                    }
                    break;
                case "Nam":
                    break;
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iLoai"] = iLoai;
                ViewData["MaChungTu"] = MaChungTu;
                ViewData["DuLieuMoi"] = Request.Form[ParentID + "_DuLieuMoi"];
                ViewData["sDSLNS"] = sLNS;
                ViewData["sNoiDung"] = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
                return View(sViewPath + "NguoiCoCong_ChungTu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    switch (optThangQuy)
                    {
                        case "Thang":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang);
                            break;
                        case "Quy":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", Convert.ToString(Convert.ToInt16(iQuy) * 3));
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                            break;
                    }
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(NguoiCoCongModels.iID_MaPhanHe));
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(NguoiCoCongModels.iID_MaPhanHe));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    NguoiCoCong_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    NguoiCoCong_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    switch (optThangQuy)
                    {
                        case "Thang":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 0);
                            break;
                        case "Quy":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", Convert.ToString(Convert.ToInt16(iQuy) * 3));
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                            break;
                    }

                    bang.GiaTriKhoa = MaChungTu;
                    bang.Save();
                    switch (optThangQuy)
                    {
                        case "Thang":
                            NguoiCoCong_ChungTuModels.UpdateBangChiTiet(User.Identity.Name, Request.UserHostAddress, MaChungTu, iID_MaDonVi, iThang, bLoaiThang_Quy);
                            break;
                        case "Quy":
                            NguoiCoCong_ChungTuModels.UpdateBangChiTiet(User.Identity.Name, Request.UserHostAddress, MaChungTu, iID_MaDonVi, Convert.ToString(Convert.ToInt16(iQuy) * 3), bLoaiThang_Quy);
                            break;
                    }
                   
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "NguoiCoCong_ChungTu", new { iLoai = iLoai });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChungTu, String iLoai)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = NguoiCoCong_ChungTuModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "NguoiCoCong_ChungTu", new { iLoai = iLoai });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return View(sViewPath + "NguoiCoCong_ChungTu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID, String Loai)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Duyet", "NguoiCoCong_ChungTu", new { Loai = Loai, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        public JsonResult get_List_NguoiCoCong_ChungTu(String Loai, String MaND, String iSoChungTu, String dTuNgay, String dDenNgay, String iID_MaTrangThaiDuyet, String page)
        {
            QuyetToanListModels ModelData = new QuyetToanListModels(Loai, MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet, page);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/QuyetToan/ChungTu/NguoiCoCong_ChungTu_List_Partial.ascx", ModelData);

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

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = NguoiCoCong_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            NguoiCoCong_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = NguoiCoCong_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            NguoiCoCong_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = NguoiCoCong_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            return RedirectToAction("Index", "NguoiCoCong_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = NguoiCoCong_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            NguoiCoCong_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            NguoiCoCong_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = NguoiCoCong_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            NguoiCoCong_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "NguoiCoCong_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        #region Chuyển năm sau
        [Authorize]
        public ActionResult ChuyenNamSau()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "NguoiCoCong_ChuyenNamSau.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ChuyenNamSau()
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            int MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong);
            NganSach_HamChungModels.ChuyenNamSau(MaND, IPSua, MaTrangThaiDuyet, "NCC_ChungTu", "NCC_ChungTuChiTiet", false);
            return RedirectToAction("Index");

        }
        #endregion
    }
}
