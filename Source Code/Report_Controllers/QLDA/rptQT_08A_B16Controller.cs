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

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQT_08A_B16Controller : Controller
    {
        //
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQT_08A_B16.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQT_08A_B16.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            return RedirectToAction("Index", new { NamLamViec = NamLamViec });
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec)
        {
           
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQT_08A_B16");
            LoadData(fr, NamLamViec);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("cuctaichinh", "Cục Tài Chính");
            fr.SetValue("ngay", ngay);
            fr.SetValue("Phong", "Phòng QLNS Các DA");
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public DataTable DatatQT_08A_B16(String NamLamViec)
        {

            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT sDuAn,sCongTrinh,sHangMucCongTrinh,sTenDuAn
                                                ,DuToanDuocDuyet
                                                ,KPTQTDenCuoiNamTruoc
                                                ,KHVNamTruoc
                                                ,KHVNamNay
                                                ,VonChuaTamUng
                                                ,TTHT
                                                ,GTKLQTHoanThanh
                                                ,GTKLQTDeNghi
                                                 FROM
                                                (SELECT 
                                                iID_MaDanhMucDuAn,sDuAn,sCongTrinh,sHangMucCongTrinh,sTenDuAn
                                                FROM QLDA_DanhMucDuAn) as DuAn
                                                INNER JOIN
                                                ( 
                                                SELECT iID_MaDanhMucDuAn,SUM(rCucTaiChinhThu) as DuToanDuocDuyet
                                                ,KPTQTDenCuoiNamTruoc=CASE WHEN iNamLamViec=@NamLamViec-1 THEN SUM(rCucTaiChinhThu) ELSE 0 END
                                                FROM QLDA_DuToan_Nam WHERE iTrangThai=1 
                                                GROUP BY iID_MaDanhMucDuAn,iNamLamViec
                                                HAVING SUM(rCucTaiChinhThu)!=0) as DTN
                                                ON DTN.iID_MaDanhMucDuAn=DuAn.iID_MaDanhMucDuAn
                                                INNER JOIN
                                                (
                                                SELECT 
                                                iID_MaDanhMucDuAn
                                                ,KHVNamTruoc=CASE WHEN iNamLamViec=@NamLamViec-1 AND iTrangThai=1 THEN SUM(rSoTienDauNam)+SUM(rSoTienDieuChinh) ELSE 0 END
                                                ,KHVNamNay=CASE WHEN iNamLamViec=@NamLamViec AND iTrangThai=1 THEN SUM(rSoTienDauNam)+SUM(rSoTienDieuChinh) ELSE 0 END
                                                FROM QLDA_KeHoachVon
                                                GROUP BY iID_MaDanhMucDuAn,iTrangThai,iNamLamViec
                                                HAVING SUM(rSoTienDieuChinh)!=0 OR SUM(rSoTienDauNam)!=0) as KHV
                                                ON KHV.iID_MaDanhMucDuAn=DuAn.iID_MaDanhMucDuAn
                                                INNER JOIN
                                                (
                                                SELECT 
                                                iID_MaDanhMucDuAn
                                                ,VonChuaTamUng=SUM(rPheDuyetThuTamUng)-SUM(rPheDuyetTamUng)
                                                ,SUM(rPheDuyetThanhToanHoanThanh) as TTHT
                                                FROM QLDA_CapPhat WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec
                                                GROUP BY iID_MaDanhMucDuAn,iTrangThai,iNamLamViec
                                                HAVING SUM(rPheDuyetThuTamUng)!=0 OR SUM(rPheDuyetThanhToanHoanThanh)!=0 ) as CP
                                                ON CP.iID_MaDanhMucDuAn=DuAn.iID_MaDanhMucDuAn
                                                INNER JOIN 
                                                (SELECT 
                                                 iID_MaDanhMucDuAn,SUM(rSoTienPheDuyet) as GTKLQTHoanThanh
                                                ,SUM(rSoTienChuDauTuDeNghi) as GTKLQTDeNghi
                                                FROM QLDA_QuyetToanHoanThanh WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec
                                                GROUP BY iID_MaDanhMucDuAn,iTrangThai,iNamLamViec
                                                HAVING SUM(rSoTienPheDuyet)!=0 OR SUM(rSoTienChuDauTuDeNghi)!=0) as QTHT
                                                ON QTHT.iID_MaDanhMucDuAn=DuAn.iID_MaDanhMucDuAn");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec)
        {
            DataTable data = DatatQT_08A_B16(NamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable HangMucCongTrinh = HamChung.SelectDistinct_QLDA("HangMucCongTrinh", data, "sDuAn,sCongTrinh,sHangMucCongTrinh", "sDuAn,sCongTrinh,sHangMucCongTrinh,sTenDuAn", "sDuAn,sCongTrinh,sHangMucCongTrinh");
            HangMucCongTrinh.TableName = "HangMucCongTrinh";
            fr.AddTable("HangMucCongTrinh", HangMucCongTrinh);

            DataTable CongTrinh = HamChung.SelectDistinct_QLDA("DuCongTrinhAn", HangMucCongTrinh, "sDuAn,sCongTrinh", "sDuAn,sCongTrinh,sTenDuAn", "sDuAn,sCongTrinh");
            CongTrinh.TableName = "CongTrinh";
            fr.AddTable("CongTrinh", CongTrinh);

            DataTable DuAn = HamChung.SelectDistinct_QLDA("DuAn", CongTrinh, "sDuAn", "sDuAn,sTenDuAn", "sDuAn");
            DuAn.TableName = "DuAn";
            fr.AddTable("DuAn", DuAn);
            DuAn.Dispose();
            CongTrinh.Dispose();
            data.Dispose();



        }
        /// <summary>
        /// Ham xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
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
        public clsExcelResult ExportToExcel(String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptQT_08A_B16.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
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
