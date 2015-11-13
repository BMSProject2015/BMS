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
    public class rptQLDA_QTHT_02QTHTController : Controller
    {
        //
        // GET: /rptQLDA_QTHT_02QTHT/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_QTHT_02.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_QTHT_02QTHT.aspx";
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
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_QTHT_02QTHT");
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
        public DataTable rptQLDA_QTHT_02QTHT(String NamLamViec)
        {

            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT DM.iID_MaDanhMucDuAn,sDuAn,sDuAnThanhPhan,sCongTrinh
                                            ,sHangMucCongTrinh,sTenDuAn,TMDTDuocDuyet,TDTDuocDuyet
                                            ,ChuDauTuDeNghiQTHT,PheDuyetQTHT,KPQTCuoiNamTruoc,KPQTTrongNam
                                            FROM
                                            (
                                            SELECT iID_MaDanhMucDuAn,sDuAn
                                            ,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh
                                            ,sTenDuAn
                                            FROM QLDA_DanhMucDuAn) as DM
                                            INNER JOIN 
                                            (
                                            SELECT DT.iID_MaDanhMucDuAn
                                            ,TMDTDuocDuyet=CASE WHEN DT.iTrangThai=1 AND DT.iNamLamViec=@NamLamViec THEN SUM(rSoTien_TongDauTu) ELSE 0 END
                                            ,TDTDuocDuyet=CASE WHEN TDT.iTrangThai=1 AND TDT.iNamLamViec=@NamLamViec THEN SUM(rSoTien_TongDauTu) ELSE 0 END
                                            FROM QLDA_TongDauTu as DT
                                            LEFT JOIN QLDA_TongDuToan as TDT ON DT.iID_MaDanhMucDuAn=TDT.iID_MaDanhMucDuAn
                                            GROUP BY DT.iTrangThai,DT.iNamLamViec,DT.iID_MaDanhMucDuAn,TDT.iNamLamViec,TDT.iTrangThai) as TMDT
                                            ON DM.iID_MaDanhMucDuAn=TMDT.iID_MaDanhMucDuAn
                                            INNER JOIN 
                                            (
                                            SELECT iID_MaDanhMucDuAn
                                            ,ChuDauTuDeNghiQTHT=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rSoTienChuDauTuDeNghi) ELSE 0 END
                                            ,PheDuyetQTHT=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rSoTienPheDuyet) ELSE 0 END
                                            FROM QLDA_QuyetToanHoanThanh
                                            GROUP BY iID_MaDanhMucDuAn,iTrangThai,iNamLamViec) as QTHT
                                            ON QTHT.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn
                                            INNER JOIN 
                                            (
                                            SELECT iID_MaDanhMucDuAn
                                            ,KPQTCuoiNamTruoc=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec-1 THEN SUM(rSoTienQuyetToan) ELSE 0 END
                                            ,KPQTTrongNam=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rSoTienQuyetToan) ELSE 0 END
                                            FROM QLDA_QuyetToan
                                            GROUP BY iID_MaDanhMucDuAn,iNamLamViec,iTrangThai) as QT
                                            ON QT.iID_MaDanhMucDuAn=Dm.iID_MaDanhMucDuAn");
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
            DataTable data = rptQLDA_QTHT_02QTHT(NamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);


            DataTable CongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", data, "sDuAn,sDuAnThanhPhan,sCongTrinh", "sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn", "sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            CongTrinh.TableName = "CongTrinh";
            fr.AddTable("CongTrinh", CongTrinh);


            DataTable DuAnTP = HamChung.SelectDistinct_QLDA("DuAnTP", CongTrinh, "sDuAn,sDuAnThanhPhan", "sDuAn,sDuAnThanhPhan,sTenDuAn", "sDuAn,sDuAnThanhPhan,sCongTrinh");
            DuAnTP.TableName = "DuAnTP";
            fr.AddTable("DuAnTP", DuAnTP);

            DataTable DuAn = HamChung.SelectDistinct_QLDA("DuAn", DuAnTP, "sDuAn", "sDuAn,sTenDuAn", "sDuAn,sDuAnThanhPhan");
            DuAn.TableName = "DuAn";
            fr.AddTable("DuAn", DuAn);

            CongTrinh.Dispose();
            DuAn.Dispose();
            DuAnTP.Dispose();
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
                clsResult.FileName = "rptQLDA_QTHT_02QTHT.xls";
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
