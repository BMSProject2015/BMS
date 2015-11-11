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

namespace VIETTEL.Report_Controllers.TCDN
{
    public class rptTCDN_BaoCaoTongHopController : Controller
    {
   
        //
        // GET: /DuToanChiNganSachSuDung/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop1.xls";
        public static String NameFile = "";
        public ActionResult Index(String iNamLamViec, String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTongHop.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["PageLoad"] = "0";
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iQuy"] = iQuy;
            ViewData["iID_MaLoaiDoanhNghiep"] = iID_MaLoaiDoanhNghiep;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm lấy giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String iID_MaLoaiDoanhNghiep = Request.Form[ParentID + "_iID_MaLoaiDoanhNghiep"];
            ViewData["PageLoad"] = "1";
            ViewData["iNamLamViec"] = NamLamViec;
            ViewData["iQuy"] = iQuy;
            ViewData["iID_MaLoaiDoanhNghiep"] = iID_MaLoaiDoanhNghiep;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTongHop.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iNamLamViec, String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BaoCaoTongHop");
            LoadData(fr, iNamLamViec,iQuy,iID_MaLoaiDoanhNghiep);
            String Thang = "";
            if (iQuy == "1")
                Thang = "3";
            else if (iQuy == "2")
            {
                Thang = "6";
            }
            else if (iQuy == "3")
            {
                Thang = "9";
            }
            else
            {
                Thang = "cả năm";
            }

            fr.SetValue("Thang", Thang);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("NamTruoc", Convert.ToInt16(iNamLamViec)-1);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="MaKhoi"></param>
        /// <param name="iQuy"></param>
        /// <returns></returns>
        public static DataTable DT_BaoCaoTongHop(String iNamLamViec,String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            if (String.IsNullOrEmpty(iQuy)) iQuy = "5";
            if (String.IsNullOrEmpty(iID_MaLoaiDoanhNghiep)) iID_MaLoaiDoanhNghiep = Guid.Empty.ToString();
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT *                                              
                                          FROM TCDN_BaoCaoTongHop
                                        WHERE TCDN_BaoCaoTongHop.iTrangThai=1 
                                        AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy  AND iID_MaLoaiDoanhNghiep=@iID_MaLoaiDoanhNghiep
                                        ORDER BY sTenDoanhNghiep");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);            
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iID_MaLoaiDoanhNghiep);            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            DataTable data = DT_BaoCaoTongHop(iNamLamViec,iQuy,iID_MaLoaiDoanhNghiep);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public clsExcelResult ExportToExcel(String iNamLamViec, String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String xfilePath = sFilePath;
            if(iQuy=="4") xfilePath=sFilePath1;
            ExcelFile xls = CreateReport(Server.MapPath(xfilePath), iNamLamViec, iQuy, iID_MaLoaiDoanhNghiep);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTChiNganSachQuocPhong.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iNamLamViec, String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            HamChung.Language();
            String xfilePath = sFilePath;
            if (iQuy == "4") xfilePath = sFilePath1;
            ExcelFile xls = CreateReport(Server.MapPath(xfilePath), iNamLamViec,iQuy,iID_MaLoaiDoanhNghiep);
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
    }
}