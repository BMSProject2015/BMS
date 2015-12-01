using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.Mvc;
using DomainModel;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using VIETTEL.Models;
using VIETTEL.Models.DuToan;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_NganSachBaoDam_TungDotController : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const string VIEW_PATH_NGANSACH_BAODAM_TUNGDOT= "~/Report_Views/DuToan/rptDuToan_NganSachBaoDam_TungDot.aspx";
        private const String sFilePath1_KG = "/Report_ExcelFrom/DuToan/rptDuToan_1040100_TungDot_1_KG.xls";
        private const String sFilePath2_KG = "/Report_ExcelFrom/DuToan/rptDuToan_1040100_TungDot_2_KG.xls";
        private static dataDuLieu1 _data;

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = VIEW_PATH_NGANSACH_BAODAM_TUNGDOT;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String MaTo = Request.Form["MaTo"];
            String Nganh = Request.Form[ParentID + "_Nganh"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String MaDot = Request.Form[ParentID + "_MaDot"];
            ViewData["PageLoad"] = "1";
            ViewData["MaTo"] = MaTo;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["MaDot"] = MaDot;
            ViewData["Nganh"] = Nganh;
            ViewData["path"] = VIEW_PATH_NGANSACH_BAODAM_TUNGDOT;
            return View(sViewPath + "ReportView.aspx");
        }
        
        public ExcelFile CreateReport(String path, String MaND, String Nganh, String ToSo, String MaDot, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }

            DataTable dtPB = DuToan_ReportModels.dtPhongBanInBaoDam();
            String sTenPB = "";

            for (int j = 0; j < dtPB.Rows.Count; j++)
            {
                if (iID_MaPhongBan == Convert.ToString(dtPB.Rows[j]["iID"]))
                {
                    sTenPB = Convert.ToString(dtPB.Rows[j]["sTen"]);
                }

            }

            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_NganSachBaoDam_TungDot");
            _data = DuToan_ReportModels.get_dtDuToan_1050000(MaND, Nganh, ToSo, MaDot, iID_MaPhongBan);

            DataTable data = _data.dtDuLieu;
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            ArrayList arrMoTa1 = _data.arrMoTa1;
            ArrayList arrMoTa2 = _data.arrMoTa2;
            ArrayList arrMoTa3 = _data.arrMoTa3;
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("ToSo", ToSo);

            int i = 1;
            foreach (object obj in arrMoTa1)
            {
                fr.SetValue("MoTa1_" + i, obj);
                i++;
            }

            i = 1;

            foreach (object obj in arrMoTa2)
            {
                fr.SetValue("MoTa2_" + i, obj);
                i++;
            }

            i = 1;
            
            foreach (object obj in arrMoTa3)
            {
                fr.SetValue("MoTa3_" + i, obj);
                i++;
            }

            String sTenDonVi = "";

            sTenDonVi = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID", Nganh, "sTenNganh")); ;
            fr.SetValue("Cap2", sTenDonVi);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("ngaythang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("thang", ReportModels.Thang_Nam_HienTai());
            fr.SetValue("ThangNam", ReportModels.Thang_Nam_HienTai());
            fr.SetValue("sTenPhongBan", sTenPB);
            fr.Run(Result);
            return Result;

        }

        public ActionResult ViewPDF(String MaND, String Nganh, String ToSo, String MaDot, String iID_MaPhongBan)
        {
            HamChung.Language();
            String DuongDan = "";
            {
                if (ToSo == "1")
                    DuongDan = sFilePath1_KG;
                else
                    DuongDan = sFilePath2_KG;
            }

            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, Nganh, ToSo, MaDot, iID_MaPhongBan);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }
            }
        }

        public JsonResult LayDanhSachDonVi(String ParentID, String Nganh, String ToSo, String MaDot, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            String viewPath = "~/Views/DungChung/DonVi/To_DanhSach.ascx";

            if (String.IsNullOrEmpty(ToSo))
                ToSo = "1";
            DataTable dt = DanhSachToIn(MaND, Nganh, "1", MaDot, iID_MaPhongBan);

            if (String.IsNullOrEmpty(ToSo))
            {
                ToSo = Guid.Empty.ToString();
            }
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, ToSo, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(viewPath, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        public static DataTable DanhSachToIn(String MaND, String Nganh, String ToSo, String MaDot, String iID_MaPhongBan)
        {
            _data = DuToan_ReportModels.get_dtDuToan_1050000(MaND, Nganh, ToSo, MaDot, iID_MaPhongBan);
            DataTable dtToIn = new DataTable();
            dtToIn.Columns.Add("MaTo", typeof(String));
            dtToIn.Columns.Add("TenTo", typeof(String));
            DataRow R = dtToIn.NewRow();
            dtToIn.Rows.Add(R);
            R[0] = "1";
            R[1] = "Tờ 1";
            if (_data.dtdtDuLieuAll != null)
            {
                int a = 2;
                for (int i = 0; i < _data.dtdtDuLieuAll.Columns.Count - 8; i = i + 6)
                {
                    DataRow R1 = dtToIn.NewRow();
                    dtToIn.Rows.Add(R1);
                    R1[0] = a;
                    R1[1] = "Tờ " + a;
                    a++;
                }
            }
            return dtToIn;
        }
    }
}
