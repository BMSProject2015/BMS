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

namespace VIETTEL.Controllers.QuyetToan
{
    public class QuyetToan_QuanSo_RaQuanController : Controller
    {
        //
        // GET: /QuyetToan_QuanSo_RaQuan/
        public string sViewPath = "~/Views/QuyetToan/QuanSo/RaQuan/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "QuyetToan_QuanSo_RaQuan_Index.aspx");
        }
        [Authorize]
        public ActionResult Detail(String iThang)
        {
            ViewData["iThang"]=iThang;
            return View(sViewPath + "QuyetToan_QuanSo_RaQuan_Detail.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String Loai)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            NameValueCollection arrLoi = new NameValueCollection();
            if (HamChung.isDate(TuNgay) == false && TuNgay != "")
            {
                arrLoi.Add("err_dTuNgay", "Ngày nhập sai");
            }

            if (HamChung.isDate(DenNgay) == false && DenNgay != "")
            {
                arrLoi.Add("err_dDenNgay", "Ngày nhập sai");
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["Loai"] = Loai;
                return View(sViewPath + "QuyetToan_ChungTu_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "QuyetToan_ChungTu", new { Loai = Loai, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
            }
        }

        [Authorize]
        public ActionResult Edit(String iThang)
        {
            String MaND = User.Identity.Name;
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "DT_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
           
            ViewData["DuLieuMoi"] = "1";          
            ViewData["iThang"] = iThang;           
            return View(sViewPath + "QuyetToan_QuanSo_RaQuan_Edit.aspx");
            
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]

        public ActionResult EditSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("QTQS_QuyetToanRaQuan");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            DataTable dtDonVi = DonViModels.Get_dtDonVi();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String sGiaTri = iThang + "," + iNamLamViec;
            if (HamChung.Check_Trung("QTQS_QuyetToanRaQuan", "iID_MaQuanSoRaQuan", "", "iThang,iNamLamViec", sGiaTri, true))
            {
                arrLoi.Add("err_iThang", "Tháng này đã có!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }               
                ViewData["DuLieuMoi"] = "1";               
                return View(sViewPath + "QuyetToan_QuanSo_RaQuan_Edit.aspx");
            }
            else
            {

                DataTable dtQS = QuyetToan_QuanSo_RaQuanModels.LayDuLieuTuQuanSo(iThang, iNamLamViec);
                DataRow R = dtCauHinh.Rows[0];

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.CmdParams.Parameters.AddWithValue("@iThang", iThang);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                Object rHuu=0, rXuatNgu=0, rThoiViec=0, rPhucVien=0 ,rTongSo=0;
                int j = 0;
                String MaDonVi1, MaDonVi2,sTenDonVi;
                for (i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    rHuu = 0; rXuatNgu = 0; rThoiViec = 0; rPhucVien = 0; rTongSo=0;
                    MaDonVi1 = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]);
                    sTenDonVi = Convert.ToString(dtDonVi.Rows[i]["sTen"]);
                    for (j = 0; j < dtQS.Rows.Count; j++)
                    {
                        MaDonVi2 = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]);
                        if (MaDonVi1.Equals(MaDonVi2))
                        {
                            rHuu = dtQS.Rows[j]["rHuu"];
                            rXuatNgu = dtQS.Rows[j]["rXuatNgu"];
                            rThoiViec = dtQS.Rows[j]["rThoiViec"];
                            rPhucVien = dtQS.Rows[j]["rPhucVien"];
                            rTongSo = Convert.ToDouble(rHuu) + Convert.ToDouble(rXuatNgu) + Convert.ToDouble(rThoiViec) + Convert.ToDouble(rPhucVien);
                            dtQS.Rows.RemoveAt(j);
                        }                        
                        break;
                    }
                    if (i == 0)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi1);
                        bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                        bang.CmdParams.Parameters.AddWithValue("@rVeHuu", rHuu);
                        bang.CmdParams.Parameters.AddWithValue("@rXuatNgu", rXuatNgu);
                        bang.CmdParams.Parameters.AddWithValue("@rThoiViec", rThoiViec);
                        bang.CmdParams.Parameters.AddWithValue("@rPhucVien", rPhucVien);
                        bang.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);
                        
                    }
                    else
                    {
                        bang.CmdParams.Parameters["@iID_MaDonVi"].Value= MaDonVi1;
                        bang.CmdParams.Parameters["@sTenDonVi"].Value = sTenDonVi;
                        bang.CmdParams.Parameters["@rVeHuu"].Value = rHuu;
                        bang.CmdParams.Parameters["@rXuatNgu"].Value = rXuatNgu;
                        bang.CmdParams.Parameters["@rThoiViec"].Value = rThoiViec;
                        bang.CmdParams.Parameters["@rPhucVien"].Value = rPhucVien;
                        bang.CmdParams.Parameters["@rTongSo"].Value = rTongSo;
                    }
                    bang.Save();
                }

                dtCauHinh.Dispose();
            }
            return RedirectToAction("Detail", "QuyetToan_QuanSo_RaQuan", new { iThang = iThang });
        }

        
        [Authorize]
        public ActionResult Delete(String iThang, String Loai)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = QuyetToan_QuanSo_RaQuanModels.Delete_RaQuan(iThang, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "QuyetToan_QuanSo_RaQuan");
        }
       

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID, String Loai)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Duyet", "QuyetToan_ChungTu", new { Loai = Loai, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        public JsonResult get_List_QuyetToan_ChungTu(String Loai, String MaND, String iSoChungTu, String dTuNgay, String dDenNgay, String iID_MaTrangThaiDuyet, String page)
        {
            QuyetToanListModels ModelData = new QuyetToanListModels(Loai, MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet, page);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/QuyetToan/ChungTu/QuyetToan_ChungTu_List_Partial.ascx", ModelData);

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

       

    }
}
