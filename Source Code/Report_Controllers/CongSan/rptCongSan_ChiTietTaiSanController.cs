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
using System.Globalization;

namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptCongSan_ChiTietTaiSanController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_SoTaiSanCoDinh.xls";
            }
            ViewData["path"] = "~/Report_Views/CongSan/rptCongSan_SoTaiSanCoDinh.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView_NoMaster.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String TuNam = Convert.ToString(Request.Form[ParentID + "_iTuNam"]);
            String DenNam = Convert.ToString(Request.Form[ParentID + "_iDenNam"]);
            String MaDV = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String MaLTS = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiTaiSan"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            return RedirectToAction("Index", new { TuNam = TuNam, DenNam = DenNam, MaDV = MaDV, MaLTS = MaLTS, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }

        
        /// <summary>
        /// CreateReport
        /// </summary>
        /// <param name="path"></param>
        /// <param name="TuNam"></param>
        /// <param name="DenNam"></param>
        /// <param name="MaDV"></param>
        /// <param name="MaLTS"></param>
        /// <returns></returns>
        #region "Tạo báo cáo"
        public ExcelFile CreateReport(String path, String iID_MaTaiSan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            FlexCelReport fr = new FlexCelReport();
           
            fr = ReportModels.LayThongTinChuKy(fr, "rptCongSan_SoTaiSanCoDinh");
            String TenLoaiTaiSan = "";            
            
            LoadData(fr, iID_MaTaiSan);
            fr.Run(Result);
            return Result;
        }
        #endregion
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="TuNam"></param>
        /// <param name="DenNam"></param>
        /// <param name="MaDV"></param>
        /// <param name="MaLTS"></param>
        #region "Đổ dữ liệu xuống file báo cáo"
        private void LoadData(FlexCelReport fr, String iID_MaTaiSan)
        {
             DataTable dtTaiSan = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
            DataTable data = KTCS_TaiSanModels.Get_dtTaiSanChiTiet(iID_MaTaiSan);
            //data.Columns.Add("sTenTaiSan");
            data.Columns.Add("sTenDonVi",typeof(String));
            data.Columns.Add("iID_MaDonVi", typeof(String));
            data.Columns.Add("sLoaiHinhDonVi", typeof(String));
            DataTable dtDV=DonViModels.Get_dtDonVi(Convert.ToString(dtTaiSan.Rows[0]["iID_MaDonVi"]));
            String sLoaiHinhDonVi=CommonFunction.LayTenDanhMuc(Convert.ToString(dtDV.Rows[0]["iID_MaLoaiDonVi"]));
            if (data.Rows.Count == 0)
            {
                DataRow R = data.NewRow();
                R["sTenDonVi"] = dtTaiSan.Rows[0]["sTenDonVi"];
                R["iID_MaDonVi"] = dtTaiSan.Rows[0]["iID_MaDonVi"];
                R["sLoaiHinhDonVi"] = sLoaiHinhDonVi;
            }
            else
            {
                data.Rows[0]["sTenDonVi"] = dtTaiSan.Rows[0]["sTenDonVi"];
                data.Rows[0]["iID_MaDonVi"] = dtTaiSan.Rows[0]["iID_MaDonVi"];
                data.Rows[0]["sLoaiHinhDonVi"] = sLoaiHinhDonVi;
            }
            
            data.Rows[0]["sTenTaiSan"]=Convert.ToString(dtTaiSan.Rows[0]["sTenTaiSan"]);
            data.TableName = "CT";
            fr.AddTable("CT", data);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());            
            data.Dispose();
        }
        #endregion
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="TuNam"></param>
        /// <param name="DenNam"></param>
        /// <param name="MaDV"></param>
        /// <param name="MaLTS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTaiSan)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            DataTable dtTaiSan = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
            String iLoaiTS = Convert.ToString(dtTaiSan.Rows[0]["iLoaiTS"]);
            if (iLoaiTS == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_SoTaiSanCoDinh.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),iID_MaTaiSan);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
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
        /// ViewPDF
        /// </summary>
        /// <param name="TuNam"></param>
        /// <param name="DenNam"></param>
        /// <param name="MaDV"></param>
        /// <param name="MaLTS"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTaiSan)
        {
            //0 TS Chung; 1 Đất; 2 Nhà; 3 Ô tô; 4 Trên 500tr
            DataTable dtTaiSan = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
            String iLoaiTS=Convert.ToString(dtTaiSan.Rows[0]["iLoaiTS"]);

            String sFilePath = "";
            switch (iLoaiTS)
            {
                case "0":
                    sFilePath = "/Report_ExcelFrom/CongSan/rptCS0.xls";
                    break;
                case "1":
                    sFilePath = "/Report_ExcelFrom/CongSan/rptCS1.xls";
                    break;
                case "2":
                    sFilePath = "/Report_ExcelFrom/CongSan/rptCS2.xls";
                    break;
                case "3":
                    sFilePath = "/Report_ExcelFrom/CongSan/rptCS3.xls";
                    break;
                case "4":
                    sFilePath = "/Report_ExcelFrom/CongSan/rptCS4.xls";
                    break;
            }         
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTaiSan);
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
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTaiSan)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            //0 TS Chung; 1 Đất; 2 Nhà; 3 Ô tô; 4 Trên 500tr
            DataTable dtTaiSan = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
            String iLoaiTS = Convert.ToString(dtTaiSan.Rows[0]["iLoaiTS"]);
            if (iLoaiTS == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_SoTaiSanCoDinh.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTaiSan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (iLoaiTS == "1")
                {
                    clsResult.FileName = "SoTaiSanCoDinh_A3";
                }
                else
                {
                    clsResult.FileName = "SoTaiSanCoDinh.xls";
                }
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// lấy danh sách loại tài sản
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
       

        
    }
}