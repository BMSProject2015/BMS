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
    public class rptDTCNSQP_ChiChoDoanhNghiepController : Controller
    {
        //
        // GET: /rptDTCNSQP_ChiChoDoanhNghiep/

        //
        // GET: /rptDTCNSQP_XayDungCoBan/
        //
        // GET: /rptDuToanChiNganSachQuocPhong/        
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDTCNSQP_ChiChoDoanhNghiep.xls";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDTCNSQP_ChiChoDoanhNghiep.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet });
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            //Result.Open(path);
            DataTable dt = DuToan_ReportModels.DT_DTCNSQP_ChiChoDoanhNghiep(MaND, iID_MaTrangThaiDuyet);

            String SetDienDuLieu = "2-0";
            Result = TaoTieuDe(Result, dt, 8, 4, 1, dt.Columns.Count - 1, 6);
            ReportModels.FillData(Result, dt, 8, 4, 1, dt.Columns.Count - 1, 6,SetDienDuLieu,true);
            using (FlexCelReport fr = new FlexCelReport())
            {
                fr.SetValue("Nam", iNamLamViec);
                fr.Run(Result);
                return Result;
            }
        }
        //private void LoadData(FlexCelReport fr, String NamLamViec)
        //{
        //    DataTable data = DuToan_ReportModels.DT_DTCNSQP_ChiChoDoanhNghiep(NamLamViec);
        //    DataTable dtNew = new DataTable();
        //    for (int c = 0; c < data.Columns.Count; c++)
        //    {
        //        Type _Type = data.Columns[c].DataType;
        //        dtNew.Columns.Add(c.ToString(), _Type);
        //    }
        //    DataRow R;
        //    for (int i = 0; i < data.Rows.Count; i++)
        //    {
        //        R = dtNew.NewRow();
        //        for (int c = 0; c < data.Columns.Count; c++)
        //        {
        //            R[c] = data.Rows[i][c];
        //        }
        //        dtNew.Rows.Add(R);
        //    }
        //    data.TableName = "ChiTiet";
        //    fr.AddTable("ChiTiet", dtNew);
        //}

        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
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
            return null;

        }

        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public static XlsFile TaoTieuDe(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotCuaMotTrang)
        {


            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            
                      

            xls.ActiveSheet = 1;    //Set the sheet we are working in.
            xls.SheetName = "BaoCao";//Set the names of the sheets
            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;

            //Tính số trang và số cột cần thêm để đủ một trang
            int SoCotDu = (DenCotCua_DT - TuCotCua_DT) % SoCotCuaMotTrang;
            int SoCotCanThem = 0;
            int TongSoCot = 0;
            if (SoCotDu != 0)
            {
                SoCotCanThem = SoCotCuaMotTrang - SoCotDu;
            }
            TongSoCot = DenCotCua_DT + SoCotCanThem - TuCotCua_DT;
            int SoTrang = TongSoCot / SoCotCuaMotTrang;
            int _C = TuCot;

           

            //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Good, 0));
            StyleFmt.Font.CharSet = 163;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Good, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Font.CharSet = 163;
            StyleFmt.Format = "_-* #,##0.00\\ _₫_-;\\-* #,##0.00\\ _₫_-;_-* \"-\"??\\ _₫_-;_-@_-";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

           

            //Named Ranges
            TXlsNamedRange Range;

            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='BaoCao'!$A:$B,'BaoCao'!$1:$7");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);


            #region //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = false;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "";
            HeadersAndFooters.DefaultFooter = "";
            HeadersAndFooters.FirstHeader = "";
            HeadersAndFooters.FirstFooter = "";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.236220472440945;
            //    PrintMargins.Top = 0.748031496062992;
            //    PrintMargins.Right = 0.236220472440945;
            //    PrintMargins.Bottom = 0.748031496062992;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.236220472440945, 0.748031496062992, 0.236220472440945, 0.748031496062992, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 0;
            xls.PrintOptions = TPrintOptions.LeftToRight;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x5C, 0x00, 0x5C, 0x00, 0x31, 0x00, 0x39, 0x00, 0x32, 0x00, 0x2E, 0x00, 0x31, 0x00, 0x36, 0x00, 0x38, 0x00, 0x2E, 0x00, 0x31, 0x00, 0x34, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x37, 0x00, 0x5C, 0x00, 0x48, 0x00, 0x50, 0x00, 0x20, 0x00, 0x4C, 0x00, 0x61, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00, 
            //        0x4A, 0x00, 0x65, 0x00, 0x74, 0x00, 0x20, 0x00, 0x4D, 0x00, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x15, 0x06, 0xDC, 0x00, 0x34, 0x03, 0x0F, 0xDF, 0x00, 0x00, 0x02, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x00, 0x00, 0x01, 0x00, 0x07, 0x00, 0x58, 0x02, 0x01, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBF, 0x03, 0xC1, 0x02, 0x00, 0x00, 0x00, 0x00, 0x08, 0x04, 0xF8, 0x02, 0x00, 0x00, 0x00, 0x00, 0x52, 0x04, 0x2E, 0x03, 0x00, 0x00, 0x00, 0x00, 0x9C, 0x04, 0x64, 0x03, 0x00, 0x00, 0x00, 0x00, 0xE6, 0x04, 0x9A, 0x03, 0x00, 0x00, 0x00, 0x00, 0x2F, 0x05, 0xD1, 0x03, 
            //        0x00, 0x00, 0x00, 0x00, 0x79, 0x05, 0x07, 0x04, 0x00, 0x00, 0x00, 0x00, 0xC3, 0x05, 0x3D, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x53, 0x44, 0x44, 0x4D, 0x00, 0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x48, 0x50, 0x20, 0x4C, 0x61, 0x73, 0x65, 0x72, 0x4A, 0x65, 0x74, 0x20, 0x4D, 0x31, 0x33, 0x31, 
            //        0x39, 0x66, 0x20, 0x4D, 0x46, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
            //        0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x1A, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 
            //        0x00, 0x00, 0x5A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x80, 0x80, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 
            //        0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xDE, 0x03, 0x00, 0x00, 0xDE, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x59, 0xC3, 0x13, 0xAE, 0x34, 0x03, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);
            #endregion

            #region //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 1609);    //(5.54 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));

            xls.SetColWidth(2, 6326);    //(23.96 + 0.75) * 256  

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));

            xls.SetColWidth(3, 4205);    //(15.68 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 4059);    //(15.11 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 4059);    //(15.11 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 4059);    //(15.11 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));
            xls.SetColWidth(7, 4059);    //(15.11 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));
            xls.SetColWidth(8, 4059);    //(15.11 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(8));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(8, xls.AddFormat(ColFmt));

            xls.SetColWidth(9, 4059);    //(15.11 + 0.75) * 256
            ColFmt = xls.GetFormat(xls.GetColFormat(9));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.Font.CharSet = 0;
            xls.SetColFormat(9, xls.AddFormat(ColFmt));
            #endregion

            #region set độ rộng cột
            for (int c = 0; c < TongSoCot + SoTrang; c++)
            {
                ColFmt = xls.GetFormat(xls.GetColFormat(c + TuCot));
                ColFmt.Font.Name = "Times New Roman";
                ColFmt.Font.Family = 1;
                ColFmt.Font.CharSet = 0;
                xls.SetColFormat(c + TuCot, xls.AddFormat(ColFmt));
                xls.SetColWidth(c + TuCot + 1, 4059);
            }
            #endregion

            xls.DefaultRowHeight = 300;
            xls.SetRowHeight(7, 1545);
            xls.SetRowHeight(8, 360); 

            #region //Merged Cells

            xls.MergeCells(6, 1, 7, 1);
            xls.MergeCells(6, 3, 7, 3);
            xls.MergeCells(6, 4, 6, 9);
            xls.MergeCells(5, 1, 5, 2);
            xls.MergeCells(1, 1, 1, 2);
            xls.MergeCells(1, 3, 1, 9);
            xls.MergeCells(2, 3, 2, 9);

            for (int t = 0; t < SoTrang; t++)
            {                
                xls.MergeCells(6, _C, 6, _C + SoCotCuaMotTrang-1);
                _C = _C + SoCotCuaMotTrang;
            }

            #endregion

            //Set the cell values
            TFlxFormat fmt;
            #region
            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.HAlignment = THFlxAlignment.left;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "BỘ QUỐC PHÒNG");
            

            fmt = xls.GetCellVisibleFormatDef(1, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.HAlignment = THFlxAlignment.center;
            
            xls.SetCellValue(1, 3, "DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG NĂM <#Nam>");

            xls.SetCellFormat(1, 2, xls.AddFormat(fmt));
            xls.SetCellFormat(1, 3, xls.AddFormat(fmt));
            xls.SetCellFormat(1, 4, xls.AddFormat(fmt));
            xls.SetCellFormat(1, 5, xls.AddFormat(fmt));
            xls.SetCellFormat(1, 6, xls.AddFormat(fmt));
            xls.SetCellFormat(1, 7, xls.AddFormat(fmt));

          
            fmt = xls.GetCellVisibleFormatDef(2, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.HAlignment = THFlxAlignment.center;
            xls.SetCellFormat(2, 3, xls.AddFormat(fmt));
            xls.SetCellValue(2, 3, "(Phần chi cho doanh nghiệp)");

          
            fmt = xls.GetCellVisibleFormatDef(5, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            xls.SetCellFormat(5, 1, xls.AddFormat(fmt));
            xls.SetCellValue(5, 1, "LNS: 1050000 Loại: 460 Khoản: 468");

            fmt = xls.GetCellVisibleFormatDef(5, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            xls.SetCellFormat(5, 2, xls.AddFormat(fmt));

            //fmt = xls.GetCellVisibleFormatDef(5, 8);
            //fmt.Font.Name = "Times New Roman";
            //fmt.Font.Family = 1;
            //fmt.Font.CharSet = 0;
            //xls.SetCellFormat(5, 8, xls.AddFormat(fmt));
            //xls.SetCellValue(5, 8, "Đơn vị tính:1.000 đ");

            fmt = xls.GetCellVisibleFormatDef(6, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(6, 1, xls.AddFormat(fmt));
            xls.SetCellValue(6, 1, "STT");

            fmt = xls.GetCellVisibleFormatDef(6, 2);
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(6, 2, xls.AddFormat(fmt));
            //xls.SetCellValue(6, 2, "Nội dung ngân sách");

            fmt = xls.GetCellVisibleFormatDef(6, 3);
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(6, 3, xls.AddFormat(fmt));
            xls.SetCellValue(6, 3, "Tổng cộng");

            
            fmt = xls.GetCellVisibleFormatDef(7, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(7, 1, xls.AddFormat(fmt));

            xls.SetCellFormat(7, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(7, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(7, 2, xls.AddFormat(fmt));
            xls.SetCellValue(7, 2, "Đơn vị");

            #region //Tạo tiêu đề cột
            _C = TuCot;
            for (int c = 0; c < SoTrang; c++)
            {
                xls.MergeCells(1, _C, 1, _C + SoCotCuaMotTrang-1);

                fmt = xls.GetCellVisibleFormatDef(1, _C);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.HAlignment = THFlxAlignment.center;
                xls.SetCellFormat(1, _C, xls.AddFormat(fmt));
                xls.SetCellValue(1, _C, "DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG NĂM <#Nam>");

                xls.MergeCells(2, _C, 2, _C + SoCotCuaMotTrang-1);

                fmt = xls.GetCellVisibleFormatDef(2, _C);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.HAlignment = THFlxAlignment.center;
                xls.SetCellFormat(2, _C, xls.AddFormat(fmt));
                xls.SetCellValue(2, _C, "(Phần chi cho doanh nghiệp)");


                fmt = xls.GetCellVisibleFormatDef(5, _C + SoCotCuaMotTrang - 2);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                xls.SetCellFormat(5, _C + SoCotCuaMotTrang - 1, xls.AddFormat(fmt));
                xls.SetCellValue(5, _C + SoCotCuaMotTrang - 1, "Đơn vị tính:1.000 đ");
                _C = _C + SoCotCuaMotTrang;

            }
            

            int csdt = TuCotCua_DT;
            String TenCot = "";
            for (int i = 0; i < TongSoCot ; i++)
            {

                fmt = xls.GetCellVisibleFormatDef(7, TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(7, TuCot, xls.AddFormat(fmt));

                xls.SetCellFormat(6, TuCot+i, xls.AddFormat(fmt));
                xls.SetCellValue(6, TuCot+i, "Trong đó");
                TenCot = "";
                if (csdt <= DenCotCua_DT)
                    TenCot = dt.Columns[csdt].ColumnName;
                xls.SetCellValue(7, TuCot+i, TenCot);
                xls.SetCellFormat(7, TuCot+i, xls.AddFormat(fmt));
                csdt++;


            }
            #endregion

            
            #endregion
            //Cell selection and scroll position.
            xls.SelectCell(17, 2, false);

            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);

            return xls;
        }

        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
    }
}
