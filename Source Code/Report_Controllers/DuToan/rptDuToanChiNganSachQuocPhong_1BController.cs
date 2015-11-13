using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Collections;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanChiNganSachQuocPhong_1BController : Controller
    {
        //
        // GET: /rptDuToanChiNganSachQuocPhong_1B/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath1 = "/Report_ExcelFrom/DuToan/rptDuToanChiNganSachQuocPhong_1B_1.xls";
        private const String sFilePath2 = "/Report_ExcelFrom/DuToan/rptDuToanChiNganSachQuocPhong_1B_2.xls";
        private static DataTable dtSoTo;
        public class dataDuLieu
        {
            public DataTable dtDuLieu { get; set; }
            public DataTable dtdtDuLieuAll { get; set; }
            public ArrayList arrMoTa1 { get; set; }
            public ArrayList arrMoTa2 { get; set; }
            public ArrayList arrMoTa3 { get; set; }
        }

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToanChiNganSachQuocPhong_1B.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            
            String Nganh = Request.Form[ParentID + "_Nganh"];
            String ToSo = Request.Form[ParentID + "_ToSo"];
            return RedirectToAction("Index", new {Nganh = Nganh, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        private static dataDuLieu _data;
        public ExcelFile CreateReport(String path, String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String  iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            Result.Open(path);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanChiNganSachQuocPhong_1B");
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
            String TenNganh = "";
            DataTable dtNganh = Connection.GetDataTable("SELECT iID_MaDanhMuc,sTenKhoa, DC_DanhMuc.sTen FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'Nganh' ORDER BY iSTT");
            for (i = 0; i < dtNganh.Rows.Count; i++)
            {
                if (Nganh == dtNganh.Rows[i]["sTenKhoa"].ToString())
                {
                    TenNganh = dtNganh.Rows[i]["sTen"].ToString();
                    break;
                }
            }
            dtNganh.Dispose();
            fr.SetValue("Nganh", TenNganh);
            fr.Run(Result);
            return Result;

        }

        public ActionResult ViewPDF(String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = "";
            if (ToSo == "1")
                DuongDan = sFilePath1;
            else
                DuongDan = sFilePath2;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, Nganh, ToSo, iID_MaTrangThaiDuyet);
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

        public clsExcelResult ExportToExcel(String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = "";
            if (ToSo == "1")
                DuongDan = sFilePath1;
            else
                DuongDan = sFilePath2;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, Nganh, ToSo, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToanChiNSQP_1b.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID, String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {

            return Json(obj_DSDonVi(ParentID, MaND, Nganh, ToSo, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {
            String dsDonVi = "";
            DataTable dtTo = DanhSachToIn(MaND, Nganh, ToSo, iID_MaTrangThaiDuyet);
            SelectOptionList slTo = new SelectOptionList(dtTo, "MaTo", "TenTo");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slTo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }

        #region báo cáo động
        // public void CreateFile(ExcelFile xls,DataTable dtMoTa,DataTable dtDuLieu,int TuHang,int TuCot,int SoCotTo1,int SoCotTo2,int ToSo)
        // {
        //     xls.NewFile(1);    //Create a new Excel file with 3 sheets.

        //     //Set the names of the sheets
        //     xls.ActiveSheet = 1;
        //     xls.SheetName = "Sheet1";

        //     xls.ActiveSheet = 1;    //Set the sheet we are working in.

        //     //Global Workbook Options
        //     xls.OptionsAutoCompressPictures = true;

        //     #region//Styles.
        //     TFlxFormat StyleFmt;
        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Good, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Good, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     StyleFmt.Format = "_-* #,##0.00\\ _₫_-;\\-* #,##0.00\\ _₫_-;_-* \"-\"??\\ _₫_-;_-@_-";
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1_40_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1_40_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2_40_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2_40_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3_40_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3_40_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4_40_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4_40_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5_40_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5_40_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6_40_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6_40_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2_60_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2_60_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3_60_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3_60_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1_60_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1_60_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6_60_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6_60_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4_60_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4_60_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5_60_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5_60_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Explanatory_Text, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Explanatory_Text, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_1, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_1, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Total, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Total, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Bad, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Bad, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     StyleFmt.Format = "_-* #,##0\\ _₫_-;\\-* #,##0\\ _₫_-;_-* \"-\"\\ _₫_-;_-@_-";
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Calculation, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Calculation, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Title, 0));
        //     StyleFmt.Font.Name = "Times New Roman";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Title, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Linked_Cell, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Linked_Cell, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_2, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_2, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_3, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_3, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Warning_Text, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Warning_Text, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     StyleFmt.Format = "_-* #,##0\\ \"₫\"_-;\\-* #,##0\\ \"₫\"_-;_-* \"-\"\\ \"₫\"_-;_-@_-";
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Check_Cell, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Check_Cell, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_4, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Heading_4, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Output, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Output, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Note, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Note, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Neutral, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Neutral, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     StyleFmt.Format = "_-* #,##0.00\\ \"₫\"_-;\\-* #,##0.00\\ \"₫\"_-;_-* \"-\"??\\ \"₫\"_-;_-@_-";
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Input, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Input, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4_20_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent4_20_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5_20_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent5_20_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
        //     StyleFmt.Font.Size20 = 220;
        //     StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
        //     StyleFmt.Font.Family = 2;
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6_20_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent6_20_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3_20_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent3_20_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2_20_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent2_20_percent, 0), StyleFmt);

        //     StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1_20_percent, 0));
        //     StyleFmt.Font.Name = "Arial";
        //     StyleFmt.Font.CharSet = 163;
        //     xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Accent1_20_percent, 0), StyleFmt);
        //     #endregion
        //     //Named Ranges
        //     TXlsNamedRange Range;
        //     string RangeName;
        //     RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
        //     Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$A:$B,'Sheet1'!$1:$6");
        //     //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
        //     xls.SetNamedRange(Range);


        //     //Printer Settings
        //     THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
        //     HeadersAndFooters.AlignMargins = true;
        //     HeadersAndFooters.ScaleWithDoc = true;
        //     HeadersAndFooters.DiffFirstPage = true;
        //     HeadersAndFooters.DiffEvenPages = false;
        //     HeadersAndFooters.DefaultHeader = "";
        //     HeadersAndFooters.DefaultFooter = "";
        //     HeadersAndFooters.FirstHeader = "&CDỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG NĂM  <#Nam>\nPhần phần cấp Ngân sách bảo đảm toàn quân\n&RPhụ lục số 1b";
        //     HeadersAndFooters.FirstFooter = "";
        //     HeadersAndFooters.EvenHeader = "";
        //     HeadersAndFooters.EvenFooter = "";
        //     xls.SetPageHeaderAndFooter(HeadersAndFooters);

        //     //You can set the margins in 2 ways, the one commented here or the one below:
        //     //    TXlsMargins PrintMargins = xls.GetPrintMargins();
        //     //    PrintMargins.Left = 0.590551181102362;
        //     //    PrintMargins.Top = 0.748031496062992;
        //     //    PrintMargins.Right = 0.118110236220472;
        //     //    PrintMargins.Bottom = 0.748031496062992;
        //     //    PrintMargins.Header = 0.31496062992126;
        //     //    PrintMargins.Footer = 0.31496062992126;
        //     //    xls.SetPrintMargins(PrintMargins);
        //     xls.SetPrintMargins(new TXlsMargins(0.590551181102362, 0.748031496062992, 0.118110236220472, 0.748031496062992, 0.31496062992126, 0.31496062992126));
        //     xls.PrintXResolution = 600;
        //     xls.PrintYResolution = 0;
        //     xls.PrintOptions = TPrintOptions.None;
        //     xls.PrintPaperSize = TPaperSize.Letter;

        //     //Printer Driver Settings are a blob of data specific to a printer
        //     //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
        //     //    byte[] PrinterData = {
        //     //        0x00, 0x00, 0x5C, 0x00, 0x5C, 0x00, 0x31, 0x00, 0x39, 0x00, 0x32, 0x00, 0x2E, 0x00, 0x31, 0x00, 0x36, 0x00, 0x38, 0x00, 0x2E, 0x00, 0x31, 0x00, 0x34, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x37, 0x00, 0x5C, 0x00, 0x48, 0x00, 0x50, 0x00, 0x20, 0x00, 0x4C, 0x00, 0x61, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00, 
        //     //        0x4A, 0x00, 0x65, 0x00, 0x74, 0x00, 0x20, 0x00, 0x4D, 0x00, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x15, 0x06, 0xDC, 0x00, 0x34, 0x03, 0x0F, 0xDF, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x00, 0x00, 0x01, 0x00, 0x07, 0x00, 0x58, 0x02, 0x01, 0x00, 0x01, 0x00, 0x58, 0x02, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x37, 0x00, 0x5C, 0x00, 0x48, 0x00, 0x50, 0x00, 0x20, 0x00, 0x4C, 0x00, 0x61, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00, 0x4A, 0x00, 0x65, 0x00, 0x74, 0x00, 0x20, 0x00, 0x4D, 0x00, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x15, 0x06, 
        //     //        0xDC, 0x00, 0x34, 0x03, 0x0F, 0xDF, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x53, 0x44, 0x44, 0x4D, 0x00, 0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x48, 0x50, 0x20, 0x4C, 0x61, 0x73, 0x65, 0x72, 0x4A, 0x65, 0x74, 0x20, 0x4D, 0x31, 0x33, 0x31, 
        //     //        0x39, 0x66, 0x20, 0x4D, 0x46, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 
        //     //        0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
        //     //        0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x1A, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 
        //     //        0x00, 0x00, 0x5A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x80, 0x80, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 
        //     //        0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xDE, 0x03, 0x00, 0x00, 0xDE, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x59, 0xC3, 0x13, 0xAE, 0x34, 0x03, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //     //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        //     //    };
        //     //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
        //     //    xls.SetPrinterDriverSettings(PrinterDriverSettings);

        //     //Set up rows and columns
        //     xls.DefaultColWidth = 2340;
        //     xls.SetColWidth(1, 1024);    //(3.25 + 0.75) * 256
        //     #region //TFlxFormat ColFmt
        //     TFlxFormat ColFmt;

        //     ColFmt = xls.GetFormat(xls.GetColFormat(1));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(1, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(2, 7616);    //(29.00 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(2));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(2, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(3, 3680);    //(13.63 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(3));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(3, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(4, 3328);    //(12.25 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(4));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(4, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(5, 3488);    //(12.88 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(5));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(5, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(6, 3488);    //(12.88 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(6));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(6, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(7, 3488);    //(12.88 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(7));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(7, xls.AddFormat(ColFmt));
        //     xls.SetColWidth(8, 3488);    //(12.88 + 0.75) * 256

        //     ColFmt = xls.GetFormat(xls.GetColFormat(8));
        //     ColFmt.Font.Size20 = 180;
        //     xls.SetColFormat(8, xls.AddFormat(ColFmt));



        //     #endregion

        //     xls.SetRowHeight(4, 375);    //18.75 * 20
        //     xls.SetRowHeight(5, 540);    //27.00 * 20
        //     xls.SetRowHeight(6, 405);    //20.25 * 20

        //     //Merged Cells
        //     xls.MergeCells(4, 3, 4, 4);
        //     xls.MergeCells(4, 1, 6, 1);
        //     xls.MergeCells(5, 3, 6, 3);
        //     xls.MergeCells(5, 4, 6, 4);
        //     xls.MergeCells(1, 1, 1, 2);
        //     xls.MergeCells(3, 6, 3, 8);

        //     //Set the cell values
        //     TFlxFormat fmt;
        //     fmt = xls.GetCellVisibleFormatDef(1, 1);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.HAlignment = THFlxAlignment.left;
        //     xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
        //     xls.SetCellValue(1, 1, "Ngành:");

        //     fmt = xls.GetCellVisibleFormatDef(1, 2);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.HAlignment = THFlxAlignment.left;
        //     xls.SetCellFormat(1, 2, xls.AddFormat(fmt));

        //     fmt = xls.GetCellVisibleFormatDef(3, 6);
        //     fmt.Font.Size20 = 180;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     xls.SetCellFormat(3, 6, xls.AddFormat(fmt));
        //     xls.SetCellValue(3, 6, "Đơn vị  tính :1.000đ  <#ToSo>   <#SoTrang>");

        //     fmt = xls.GetCellVisibleFormatDef(3, 7);
        //     fmt.Font.Size20 = 180;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     xls.SetCellFormat(3, 7, xls.AddFormat(fmt));

        //     fmt = xls.GetCellVisibleFormatDef(3, 8);
        //     fmt.Font.Size20 = 180;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     xls.SetCellFormat(3, 8, xls.AddFormat(fmt));

        //     fmt = xls.GetCellVisibleFormatDef(4, 1);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Left.Color = TExcelColor.Automatic;
        //     fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Right.Color = TExcelColor.Automatic;
        //     fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Top.Color = TExcelColor.Automatic;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     fmt.VAlignment = TVFlxAlignment.center;
        //     xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
        //     xls.SetCellValue(4, 1, "STT");

        //     fmt = xls.GetCellVisibleFormatDef(4, 2);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Left.Color = TExcelColor.Automatic;
        //     fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Right.Color = TExcelColor.Automatic;
        //     fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Top.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.right;
        //     fmt.VAlignment = TVFlxAlignment.center;
        //     xls.SetCellFormat(4, 2, xls.AddFormat(fmt));
        //     xls.SetCellValue(4, 2, "Nội dung - Mục lục ngân sách");

        //     #region Cột tổng cộng khi tờ số <=1
        //     if (ToSo <= 1)
        //     {
        //         fmt = xls.GetCellVisibleFormatDef(4, 3);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Top.Color = TExcelColor.Automatic;
        //         fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(4, 3, xls.AddFormat(fmt));
        //         xls.SetCellValue(4, 3, "Tổng cộng");

        //         fmt = xls.GetCellVisibleFormatDef(4, 4);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Top.Color = TExcelColor.Automatic;
        //         fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(4, 4, xls.AddFormat(fmt));

        //         fmt = xls.GetCellVisibleFormatDef(5, 1);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Top.Color = TExcelColor.Automatic;
        //         fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(5, 1, xls.AddFormat(fmt));

        //         fmt = xls.GetCellVisibleFormatDef(5, 2);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(5, 2, xls.AddFormat(fmt));

        //         fmt = xls.GetCellVisibleFormatDef(5, 3);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Top.Color = TExcelColor.Automatic;
        //         fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(5, 3, xls.AddFormat(fmt));
        //         xls.SetCellValue(5, 3, "Bằng tiền");

        //         fmt = xls.GetCellVisibleFormatDef(5, 4);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Top.Color = TExcelColor.Automatic;
        //         fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(5, 4, xls.AddFormat(fmt));
        //         xls.SetCellValue(5, 4, "Bằng hiện vật");
        //     }
        //     #endregion

        //     int TuCotdt=4;// vì có 4 cột đầu là madv, tendv, và 2 cột tổng cộng
        //     int DenCotdt = 7;
        //     if (ToSo > 1)
        //     {
        //         TuCotdt = 4 + SoCotTo1 + (ToSo - 2) * SoCotTo2; 
        //         DenCotdt = 3 + SoCotTo1 + (ToSo - 1) * SoCotTo2;
        //     }
        //     int TongSoTo=0;
        //     int SoCotCanThem = 0,SoCotDu=0;
        //     int TongSoCot_dt = dtDuLieu.Columns.Count-4;
        //     if (TongSoCot_dt == 4) TongSoTo = 1;
        //     else if (TongSoCot_dt > 4)
        //     {
        //         SoCotDu = TongSoCot_dt - 4 % SoCotTo2;
        //         SoCotCanThem = SoCotTo2 - SoCotDu;
        //     }
        //     else
        //     {
        //         SoCotDu = TongSoCot_dt % SoCotTo1;
        //         SoCotCanThem = SoCotTo1 - SoCotDu;
        //     }


        //     int i=TuCotdt;
        //     int _TuCot = TuCot;
        //     String TenCot = "";
        //     int _CS = 0, ChiSoMoTa = 0;
        //     String BangTien_HienVat = "",MoTa="";
        //     while (i < dtDuLieu.Columns.Count)
        //     {
        //         TenCot = dtDuLieu.Columns[i].ColumnName;
        //         _CS = TenCot.IndexOf("_");


        //         fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Top.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(4,_TuCot,xls.AddFormat(fmt));
        //         xls.SetCellValue(4,_TuCot, TenCot.Substring(0,_CS));

        //         DataRow[] R = dtMoTa.Select("NG='" + TenCot.Substring(0, _CS) + "'");
        //         MoTa = Convert.ToString(R[0]["sMoTa"]);

        //         fmt = xls.GetCellVisibleFormatDef(5, _TuCot);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         fmt.WrapText = true;
        //         xls.SetCellFormat(5, _TuCot, xls.AddFormat(fmt));
        //         xls.SetCellValue(5, _TuCot, MoTa);

        //         if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
        //         else BangTien_HienVat = "Bằng hiện vật";

        //         fmt = xls.GetCellVisibleFormatDef(6, _TuCot);
        //         fmt.Font.Size20 = 180;
        //         fmt.Font.Style = TFlxFontStyles.Bold;
        //         fmt.Font.CharSet = 0;
        //         fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Left.Color = TExcelColor.Automatic;
        //         fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Right.Color = TExcelColor.Automatic;
        //         fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //         fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //         fmt.HAlignment = THFlxAlignment.center;
        //         fmt.VAlignment = TVFlxAlignment.center;
        //         xls.SetCellFormat(6, _TuCot, xls.AddFormat(fmt));
        //         xls.SetCellValue(6, _TuCot, BangTien_HienVat);

        //         _TuCot = _TuCot + 1;

        //         if (i == TuCotdt + SoCotTo1-1) break;
        //         i = i + 1;
        //     }







        //     fmt = xls.GetCellVisibleFormatDef(6, 1);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Left.Color = TExcelColor.Automatic;
        //     fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Right.Color = TExcelColor.Automatic;
        //     fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Top.Color = TExcelColor.Automatic;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     fmt.VAlignment = TVFlxAlignment.center;
        //     xls.SetCellFormat(6, 1, xls.AddFormat(fmt));

        //     fmt = xls.GetCellVisibleFormatDef(6, 2);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Left.Color = TExcelColor.Automatic;
        //     fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Right.Color = TExcelColor.Automatic;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.left;
        //     fmt.VAlignment = TVFlxAlignment.center;
        //     xls.SetCellFormat(6, 2, xls.AddFormat(fmt));
        //     xls.SetCellValue(6, 2, "Đơn vị");

        //     fmt = xls.GetCellVisibleFormatDef(6, 3);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Left.Color = TExcelColor.Automatic;
        //     fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Right.Color = TExcelColor.Automatic;
        //     fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Top.Color = TExcelColor.Automatic;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     fmt.VAlignment = TVFlxAlignment.center;
        //     xls.SetCellFormat(6, 3, xls.AddFormat(fmt));

        //     fmt = xls.GetCellVisibleFormatDef(6, 4);
        //     fmt.Font.Size20 = 180;
        //     fmt.Font.Style = TFlxFontStyles.Bold;
        //     fmt.Font.CharSet = 0;
        //     fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Left.Color = TExcelColor.Automatic;
        //     fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Right.Color = TExcelColor.Automatic;
        //     fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Top.Color = TExcelColor.Automatic;
        //     fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //     fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //     fmt.HAlignment = THFlxAlignment.center;
        //     fmt.VAlignment = TVFlxAlignment.center;
        //     xls.SetCellFormat(6, 4, xls.AddFormat(fmt));



        //     //Cell selection and scroll position.
        //     xls.SelectCell(30, 5, false);

        //     //Protection

        //     TSheetProtectionOptions SheetProtectionOptions;
        //     SheetProtectionOptions = new TSheetProtectionOptions(false);
        //     SheetProtectionOptions.SelectLockedCells = true;
        //     SheetProtectionOptions.SelectUnlockedCells = true;
        //     xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
        // }

        //public static void FillData(ExcelFile xls, DataTable dtDuLieu, int TuHang, int TuCot)
        // {
        //     TFlxFormat fmt;
        //     for (int i = 0; i < dtDuLieu.Columns.Count; i++)
        //     {
        //         Type _Type = typeof(String);
        //         _Type = dtDuLieu.Columns[i].DataType;
        //         switch (_Type.ToString())
        //         {
        //             case "System.Decimal":
        //                 fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
        //                 fmt.Font.Name = "Times New Roman";
        //                 fmt.Font.Family = 1;
        //                 fmt.Font.CharSet = 0;
        //                 fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Left.Color = TExcelColor.Automatic;
        //                 fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Right.Color = TExcelColor.Automatic;
        //                 fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Top.Color = TExcelColor.Automatic;
        //                 fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //                 fmt.Format = "#,##0;-#,##0;;@";
        //                 fmt.HAlignment = THFlxAlignment.right;
        //                 fmt.VAlignment = TVFlxAlignment.center;

        //                 break;
        //             default:
        //                 fmt = xls.GetCellVisibleFormatDef(TuHang, 2);
        //                 fmt.Font.Name = "Times New Roman";
        //                 fmt.Font.Family = 1;
        //                 fmt.Font.CharSet = 0;
        //                 fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Left.Color = TExcelColor.Automatic;
        //                 fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Right.Color = TExcelColor.Automatic;
        //                 fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Top.Color = TExcelColor.Automatic;
        //                 fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
        //                 fmt.Borders.Bottom.Color = TExcelColor.Automatic;
        //                 fmt.HAlignment = THFlxAlignment.left;
        //                 fmt.VAlignment = TVFlxAlignment.center;
        //                 break;
        //         }

        //         for (int h = 0; h < dtDuLieu.Rows.Count; h++)
        //         {

        //             xls.SetCellFormat(h + TuHang, i+1, xls.AddFormat(fmt));
        //             xls.SetCellValue(h + TuHang, i + 1, dtDuLieu.Rows[h][i]);

        //         }


        //     }
        // }
        #endregion

        public static dataDuLieu get_dtDuToan_PhuLuc1B(String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {
            DataTable dtNganh = MucLucNganSach_NganhModels.Get_dtMucLucNganSach_Nganh(Nganh);
            int cs = 0, i = 0;
            String DSNganh = "";
            for (i = 0; i < dtNganh.Rows.Count; i++)
            {
                DSNganh = DSNganh + "'" + Convert.ToString(dtNganh.Rows[i]["iID_MaNganhMLNS"]) + "',";
            }
            if (DSNganh != "") DSNganh = " AND sNG IN (" + DSNganh.Remove(DSNganh.Length - 1) + ")";
            else DSNganh = " AND sNG IN(123)";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQLNganh = "SELECT distinct sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQLNganh += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQLNganh += " FROM DT_ChungTuChiTiet WHERE sLNS='1020800' {1} AND iTrangThai=1 {2}  {0}";
            SQLNganh = String.Format(SQLNganh, DSNganh, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmdNG = new SqlCommand(SQLNganh);
            //cmdNG.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            DataTable dtNG = Connection.GetDataTable(cmdNG);
            cmdNG.Dispose();
            String SQL;
            DataTable dtDonVi;
            //SQL= "SELECT DISTINCT iID_MaDonVi";
            //SQL += " FROM DT_ChungTuChiTiet WHERE sNG IN ({0})";
            //SQL += " GROUP BY iID_MaDonVi";
            //SQL += " HAVING SUM(rTuChi)>0 OR SUM(rHienVat)>0";
            //SQL = String.Format(SQL, DSNganh);
            //dtDonVi = Connection.GetDataTable(SQL);

            // String SQL1 = "SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@iID_MaNganh";

            SQL = "SELECT iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQL += ",SUM(rTuChi) AS rTuChi";
            SQL += ",SUM(rHienVat) AS rHienVat";
            SQL += " FROM DT_ChungTuChiTiet WHERE sLNS='1020800' {1} AND iTrangThai=1 {2}   {0}";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,iID_MaDonVi,sMoTa";
            SQL += " HAVING SUM(rTuChi)>0 OR SUM(rHienVat)>0";
            SQL = String.Format(SQL, DSNganh, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);

            String strSQL = "SELECT CT.iID_MaDonVi,CT.iID_MaDonVi +' - '+ NS_DonVi.sTen AS TenDonVi";
            strSQL += ",sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,CT.sMoTa";
            strSQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            strSQL += ",rTuChi,rHienVat";
            strSQL += " FROM ({0}) CT ";
            strSQL += " INNER JOIN (SELECT iID_MaDonVi as MaDonVi, sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON NS_DonVi.MaDonVi=CT.iID_MaDonVi";
            strSQL = String.Format(strSQL, SQL);
            SqlCommand cmd = new SqlCommand(strSQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            dtDonVi = HamChung.SelectDistinct("dtDonVi", dt, "iID_MaDonVi", "iID_MaDonVi,TenDonVi");

            i = 0;
            //cs = 3;//tờ 1 4 cột
            dtDonVi.Columns.Add("TongTuChi", typeof(Decimal));
            dtDonVi.Columns.Add("TongHienVat", typeof(Decimal));
            while (i < dtNG.Rows.Count)
            {
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_TuChi") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_TuChi", typeof(Decimal));
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_HienVat") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_HienVat", typeof(Decimal));
                i = i + 1;
            }

            i = 0;
            cs = 0;
            String MaDonVi, MaDonVi1, TenCot;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]).Trim();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    MaDonVi1 = Convert.ToString(dt.Rows[j]["iID_MaDonVi"]).Trim();
                    TenCot = Convert.ToString(dt.Rows[j]["NG"]).Trim();
                    if (MaDonVi == MaDonVi1 && dtDonVi.Columns.IndexOf(TenCot + "_TuChi") >= 0)
                    {
                        dtDonVi.Rows[i][TenCot + "_TuChi"] = dt.Rows[j]["rTuChi"];
                        dtDonVi.Rows[i][TenCot + "_HienVat"] = dt.Rows[j]["rHienVat"];
                        dt.Rows.RemoveAt(j);
                        j = j - 1;
                    }
                }
            }
            i = 0;
            //j=4 vì trừ cột madv, đơn vị và 2 cột tổng cộng
            Double Tong = 0;
            for (int j = 4; j < dtDonVi.Columns.Count; j++)
            {
                Tong = 0;
                for (i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        Tong = Tong + Convert.ToDouble(dtDonVi.Rows[i][j]);
                    }
                }
                if (Tong == 0)
                {
                    dtDonVi.Columns.RemoveAt(j);
                    if (j == 1) j = 1;
                    else j = j - 1;
                }
            }
            Double TongHienVat = 0, TongTuChi = 0;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                TongHienVat = 0; TongTuChi = 0;
                //j=4 vì trừ cột MaDV, đơn vị và 2 cột tổng cộng
                for (int j = 4; j < dtDonVi.Columns.Count; j++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        if (dtDonVi.Columns[j].ColumnName.IndexOf("_HienVat") >= 0)
                        {
                            TongHienVat = TongHienVat + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                        else
                        {
                            TongTuChi = TongTuChi + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                    }
                }
                dtDonVi.Rows[i]["iID_MaDonVi"] = (i + 1).ToString();
                dtDonVi.Rows[i]["TongHienVat"] = TongHienVat;
                dtDonVi.Rows[i]["TongTuChi"] = TongTuChi;
            }
            DataTable _dtDonVi = new DataTable();
            DataTable _dtDonVi1 = new DataTable();

            int TongSoCot = 0;
            int SoTrang = 1;
            int SoCotCanThem = 0;
            if ((dtDonVi.Columns.Count - 4) == 0)
            {
                SoCotCanThem = 4;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else if ((dtDonVi.Columns.Count - 4) <= 4)
            {

                int SoCotDu = ((dtDonVi.Columns.Count - 4)) % 4;
                if (SoCotDu != 0)
                    SoCotCanThem = 4 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (dtDonVi.Columns.Count - 4 - 4) % 6;
                if (SoCotDu != 0)
                    SoCotCanThem = 6 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
                SoTrang = 1 + (TongSoCot - 4) / 6;
            }
            for (i = 0; i < SoCotCanThem; i++)
            {
                dtDonVi.Columns.Add();
            }
            int _ToSo = Convert.ToInt16(ToSo);
            int SoCotTrang1 = 4;
            int SoCotTrangLonHon1 = 6;
            _dtDonVi = dtDonVi.Copy();
            int _CS = 0;
            String BangTien_HienVat = "";
            //Mổ tả xâu nối mã
            ArrayList arrMoTa1 = new ArrayList();
            //Mỏ tả ngành
            ArrayList arrMoTa2 = new ArrayList();
            //Bằng Tiền hay bằng hiện vật
            ArrayList arrMoTa3 = new ArrayList();
            if (ToSo == "1")
            {

                for (i = 4; i < 4 + SoCotTrang1; i++)
                {
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + (i - 3);
                }
            }
            else
            {
                int tg = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2);
                int dem = 1;
                for (i = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2); i < 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 1); i++)
                {
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + dem;
                    dem++;
                }
            }

            dataDuLieu _data = new dataDuLieu();
            _data.dtDuLieu = _dtDonVi;
            _data.arrMoTa1 = arrMoTa1;
            _data.arrMoTa2 = arrMoTa2;
            _data.arrMoTa3 = arrMoTa3;
            _data.dtdtDuLieuAll = dtDonVi;            
            return _data;
        }
        public static DataTable DanhSachToIn(String MaND, String Nganh, String ToSo, String iID_MaTrangThaiDuyet)
        {
            _data = get_dtDuToan_PhuLuc1B(MaND, Nganh, ToSo, iID_MaTrangThaiDuyet);
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
