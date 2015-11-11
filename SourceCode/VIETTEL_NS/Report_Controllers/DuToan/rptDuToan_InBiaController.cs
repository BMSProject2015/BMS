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
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_InBiaController : Controller
    {
        // Edit: Thương
        // GET: /rptDTCNSSuDungTheoDonVi/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_InBia.xls";
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_InBia.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NgayGui = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dtNgayGui"]; 
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            return RedirectToAction("Index", new { NgayGui = NgayGui, iID_MaDonVi = iID_MaDonVi });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NgayGui"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NgayGui, String iID_MaDonVi)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String tendv = "";
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }           
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_InBia");            
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("nam", ("năm "+DateTime.Now.Year).ToUpper());
            fr.SetValue("Donvi", tendv.ToUpper());
            fr.SetValue("NgayGui", NgayGui); 
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// xuất ra PDF
        /// </summary>
        /// <param name="NgayGui"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NgayGui, String iID_MaDonVi)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NgayGui, iID_MaDonVi);
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
        /// Xem PDF
        /// </summary>
        /// <param name="NgayGui"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NgayGui, String iID_MaDonVi)
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NgayGui, iID_MaDonVi);
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
        /// Xuát ra Excel
        /// </summary>
        /// <param name="NgayGui"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NgayGui, String iID_MaDonVi)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NgayGui, iID_MaDonVi);             
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_InBia.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
        /// <summary>
        /// Lấy danh sách đơn vị của dự toán
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_DonVi()
        {
            String SQL = String.Format(@"select dv.iID_MaDonVi,dv.iID_MaDonVi+' - '+dv.sTen as TenHT
                                        from NS_DonVi as dv
                                        where dv.iID_MaLoaiDonVi=(select dm.iID_MaDanhMuc
                                                                  from DC_DanhMuc as dm
                                                                  where dm.sTen=N'Dự toán')");
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt=Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
