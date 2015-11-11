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
using DomainModel.Controls;


namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQSQT_QuanSoRaQuanController : Controller
    {
        ////Báo cáo quân số        
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQTQS_QuanSoRaQuan.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = 0;
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQSQT_QuanSoRaQuan.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iThang = Request.Form[ParentID + "_iThang"];
            ViewData["pageload"] = "1";
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQSQT_QuanSoRaQuan.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iThang_Quy">Tháng làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iNamLamViec">Năm làm việc</param>
        /// <param name="TongHop">Tổng hợp tất cả đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang)
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
               
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQSQT_QuanSoRaQuan");
            LoadData(fr, iThang,iNamLamViec);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("iThang", iThang);
            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Hiển thị dữ liệu báo cáo Quân số
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang_Quy">Tháng làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iNamLamViec">Năm làm việc</param>
        /// <param name="TongHop">Tổng hợp tất cả đơn vị</param>
        private void LoadData(FlexCelReport fr, String iThang, String iNamLamViec)
        {
            //Thông tin quân số tháng 0
            DataTable dtQuanSo =QuanSoRaQuan(iThang,iNamLamViec);
            dtQuanSo.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", dtQuanSo);
            dtQuanSo.Dispose();
            //Thông quân số tháng quyết toán
            DataTable dtThangNay = QuanSoThangNay(iThang,iNamLamViec);
            dtThangNay.TableName = "ThangNay";
            fr.AddTable("ThangNay", dtThangNay);
            dtThangNay.Dispose();
            //Thông tin quân số tăng tháng
            DataTable dtThangTruoc = QuanSoThangTruoc(iThang, iNamLamViec);
            dtThangTruoc.TableName = "ThangTruoc";
            fr.AddTable("ThangTruoc", dtThangTruoc);
            dtThangTruoc.Dispose();
        }
     
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="iThang">Tháng làm việc</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iThang)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="iThang">Tháng làm việc</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iThang)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuanSoRaQuan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="iThang">Tháng làm việc</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iThang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang);
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
        /// Lấy thông tin quân số tháng 
        /// </summary>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable QuanSoRaQuan(String iThang,String iNamLamViec)
        {
            DataTable dt = new DataTable();
            String SQL = String.Format(@"select b.iID_MaDonVi,b.sTen, sum(rPhucVien) as rPhucVien,sum(rXuatNgu) as rXuatNgu,sum(rThoiViec) as rThoiViec ,sum(rVeHuu) as rVeHuu 
                                                from (
                                                SELECT DV.iID_MaDonVi, DV.iID_MaDonVi+'-'+DV.sTen as sTen
                                                ,rPhucVien = sum (case when iThang = @iThang and iNamLamViec = @iNamLamViec then rPhucVien else 0 end)
                                                ,rXuatNgu = sum (case when iThang = @iThang And iNamLamViec = @iNamLamViec then rXuatNgu else 0 end)
                                                ,rThoiViec =sum (case when iThang = @iThang And iNamLamViec = @iNamLamViec  then rThoiViec else 0 end)
                                                ,rVeHuu =sum (case when iThang = @iThang And iNamLamViec = @iNamLamViec  then rVeHuu else 0 end)
                                                 FROM QTQS_QuyetToanRaQuan AS RQ
                                                 INNER JOIN NS_DonVi as DV ON DV.iID_MaDonVi = RQ.iID_MaDonVi
                                                 WHERE RQ.iTrangThai = 1
                                                 group by DV.iID_MaDonVi,sTen
                                                 having sum(rPhucVien)<>0 or sum(rXuatNgu)<> 0 or sum(rThoiViec) <>0 or sum(rVeHuu)<>0) as b
                                                 group by iID_MaDonVi,sTen
                                                     having sum(rPhucVien)<>0 or sum(rXuatNgu)<> 0 or sum(rThoiViec) <>0 or sum(rVeHuu)<>0 ");
            SqlCommand cmd = new SqlCommand(SQL);

            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin quân số trong tháng truoc
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public DataTable QuanSoThangTruoc(String iThang,String iNamLamViec)
        {
            DataTable dt = new DataTable();           
            String SQL = String.Format(@"  SELECT 
                                                rPhucVien = sum (case when iThang < @iThang and iNamLamViec = @iNamLamViec then rPhucVien else 0 end)
                                                ,rXuatNgu = sum (case when iThang < @iThang And iNamLamViec = @iNamLamViec then rXuatNgu else 0 end)
                                                ,rThoiViec =sum (case when iThang < @iThang And iNamLamViec = @iNamLamViec  then rThoiViec else 0 end)
                                                ,rVeHuu =sum (case when iThang < @iThang And iNamLamViec = @iNamLamViec  then rVeHuu else 0 end)
                                                 FROM  QTQS_QuyetToanRaQuan
                                                 WHERE iTrangThai = 1 AND iThang <> 0" );
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang", iThang);
          
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
           
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin quân số trong tháng
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public DataTable QuanSoThangNay(String iThang, String iNamLamViec)
        {
            DataTable dt = new DataTable();
           
            String SQL = String.Format(@" SELECT 
                                                rPhucVien = sum (case when iThang <= @iThang and iNamLamViec = @iNamLamViec then rPhucVien else 0 end)
                                                ,rXuatNgu = sum (case when iThang <= @iThang And iNamLamViec = @iNamLamViec then rXuatNgu else 0 end)
                                                ,rThoiViec =sum (case when iThang <= @iThang And iNamLamViec = @iNamLamViec  then rThoiViec else 0 end)
                                                ,rVeHuu =sum (case when iThang <= @iThang And iNamLamViec = @iNamLamViec  then rVeHuu else 0 end)
                                                 FROM  QTQS_QuyetToanRaQuan
                                                 WHERE iTrangThai = 1 AND iThang<>0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();          
            return dt;
        }  
     
    }
}
