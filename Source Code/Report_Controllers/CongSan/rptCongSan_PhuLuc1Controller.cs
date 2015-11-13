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
    public class rptCongSan_PhuLuc1Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/CongSan/rptCongSan_PhuLuc1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/CongSan/rptCongSan_PhuLuc1.aspx";
            return View(sViewPath + "ReportView.aspx");
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
        public ExcelFile CreateReport(String path, String iNamLamViec,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            FlexCelReport fr = new FlexCelReport();

            fr = ReportModels.LayThongTinChuKy(fr, "rptCongSan_PhuLuc1");
            LoadData(fr, iNamLamViec);
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
        private void LoadData(FlexCelReport fr, String iNamLamViec)
        {

            DataTable data = dtPhuLuc1(iNamLamViec);
            fr.AddTable("ChiTiet", data);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", iNamLamViec);          
            data.Dispose();
        }
        #endregion
        
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="TuNam"></param>
        /// <param name="DenNam"></param>
        /// <param name="MaDV"></param>
        /// <param name="MaLTS"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iNamLamViec,String KhoGiay)
        {
            //0 TS Chung; 1 Đất; 2 Nhà; 3 Ô tô; 4 Trên 500tr

            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_PhuLuc1_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_PhuLuc1.xls";
            }         
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec,KhoGiay);
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
        public clsExcelResult ExportToExcel(String iNamLamViec, String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_PhuLuc1_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptCongSan_PhuLuc1.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, KhoGiay);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "PhuLuc1_A3";
                }
                else
                {
                    clsResult.FileName = "PhuLuc2.xls";
                }
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public static DataTable dtPhuLuc1(String iNamLamViec)
            {
                DataTable dt=null;
                String SQL = String.Format(@"SELECT iID_MaDonVi,sTenDonVi
,SL_Dat=SUM(CASE WHEN iLoaiTS=1 THEN rSoLuong ELSE 0 END)
,DT_Dat=SUM(CASE WHEN iLoaiTS=1 THEN rDTKhuonVien ELSE 0 END)
,NguyenGia_Dat=SUM(CASE WHEN iLoaiTS=1 THEN rNguyenGia/1000 ELSE 0 END)
,SL_Nha=SUM(CASE WHEN iLoaiTS=2 THEN rSoLuong ELSE 0 END)
,DT_Nha=SUM(CASE WHEN iLoaiTS=2 THEN rTongDTSanNha_Nha ELSE 0 END)
,NguyenGia_Nha=SUM(CASE WHEN iLoaiTS=2 THEN rNguyenGia/1000 ELSE 0 END)
,HaoMon_Nha=SUM(CASE WHEN iLoaiTS=2 THEN rSoTienKhauHao_LuyKe/1000 ELSE 0 END)
,ConLai_Nha=SUM(CASE WHEN iLoaiTS=2 THEN rGiaTriConLai/1000 ELSE 0 END)
,SL_OTO=SUM(CASE WHEN iLoaiTS=3 THEN rSoLuong ELSE 0 END)
,NguyenGia_OTO=SUM(CASE WHEN iLoaiTS=3 THEN rNguyenGia/1000 ELSE 0 END)
,HaoMon_OTO=SUM(CASE WHEN iLoaiTS=3 THEN rSoTienKhauHao_LuyKe/1000 ELSE 0 END)
,ConLai_OTO=SUM(CASE WHEN iLoaiTS=3 THEN rGiaTriConLai/1000 ELSE 0 END)
,SL_TS500=SUM(CASE WHEN iLoaiTS=4 THEN rSoLuong ELSE 0 END)
,NguyenGia_TS500=SUM(CASE WHEN iLoaiTS=4 THEN rNguyenGia/1000 ELSE 0 END)
,HaoMon_TS500=SUM(CASE WHEN iLoaiTS=4 THEN rSoTienKhauHao_LuyKe/1000 ELSE 0 END)
,ConLai_TS500=SUM(CASE WHEN iLoaiTS=4 THEN rGiaTriConLai/1000 ELSE 0 END)
 FROM(
SELECT iID_MaTaiSan,rNguyenGia,rSoTienKhauHao_LuyKe,rGiaTriConLai 
FROM KTCS_KhauHaoHangNam WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec) as KTCS_KhauHaoHangNam
LEFT JOIN 
(
SELECT iID_MaTaiSan,iID_MaDonVi,SUBSTRING(sTenDonVi,6,1000000) as sTenDonVi ,SUM(rSoLuong)as rSoLuong,iLoaiTS
 FROM KTCS_TaiSan
 WHERE iTrangThai=1
GROUP BY iID_MaTaiSan,iID_MaDonVi,sTenDonVi,iLoaiTS) as KTCS_TaiSan
ON KTCS_KhauHaoHangNam.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
LEFT JOIN (
SELECT iID_MaTaiSan,rDTKhuonVien,rTongDTSanNha_Nha 
FROM KTCS_TaiSanChiTiet
WHERE iTrangThai=1) as KTCS_TaiSanChiTiet
ON KTCS_KhauHaoHangNam.iID_MaTaiSan=KTCS_TaiSanChiTiet.iID_MaTaiSan

GROUP BY iID_MaDonVi,sTenDonVi 
HAVING SUM(CASE WHEN iLoaiTS=1 THEN rNguyenGia/1000 ELSE 0 END)<>0
 OR SUM(CASE WHEN iLoaiTS=2 THEN rNguyenGia/1000 ELSE 0 END)<>0
 OR SUM(CASE WHEN iLoaiTS=3 THEN rNguyenGia/1000 ELSE 0 END)<>0
 OR SUM(CASE WHEN iLoaiTS=4 THEN rNguyenGia/1000 ELSE 0 END)<>0
");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                return dt;

            }

        
    }
}