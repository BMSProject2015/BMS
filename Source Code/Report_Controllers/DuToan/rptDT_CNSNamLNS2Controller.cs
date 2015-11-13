using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDT_CNSNamLNS2Controller : Controller
    {
        //
        // GET: /rptDT_CNSNamLNS2/
        // GET: /NS_QuocPhong/
        public string sViewPath = "~/Report_Views/DuToan/";
        public static String DuongDan;
        private const String sFilePath = "/Report_ExcelFrom/DuToan/PhanBoDTNSNam_2.xls";
        public ActionResult Index()
        {
            DuongDan = Server.MapPath(sFilePath);
            ViewData["path"] = "~/Report_Views/DuToan/rptDT_CNSNamLNS2.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form LNS = 2
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Request.QueryString["_iID_MaTrangThaiDuyet"];
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            ViewData["PageLoad"] = "1";
              ViewData["path"] = "~/Report_Views/DuToan/rptDT_CNSNamLNS2.aspx";
           return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
         public ExcelFile CreateReport(String path, String NamLamViec,String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDT_CNSNamLNS2");
            LoadData(fr, NamLamViec, MaND, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam", NamLamViec);
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("PhuLuc", "Phụ Lục Số : 4d2-C");
               String  TenNganSach = "Ngân sách nhà nước";
                fr.SetValue("TenNganSach", TenNganSach);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// hàm xuất dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec,String MaND,String iID_MaTrangThaiDuyet)
        {
            DataTable data = DuToan_ReportModels.DT_rptPhanBoDuToanNganSachNam_NhaNuoc(NamLamViec, MaND, iID_MaTrangThaiDuyet);
            data.TableName = "PhanBoDTNSNam";
            fr.AddTable("PhanBoDTNSNam", data);
        }
        /// <summary>
        /// hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;    
                clsResult.ms = ms;
                clsResult.FileName = "DuToanChiNganSach4d2-C.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                xls.Save(Server.MapPath("/Report_ExcelFrom/DuToan/Test.pdf"));
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>

        public ActionResult ViewPDF(String NamLamViec, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(true, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }

            }
            return null;

        }

    }
}
