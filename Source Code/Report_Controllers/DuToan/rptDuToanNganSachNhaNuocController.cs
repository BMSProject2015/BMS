using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using VIETTEL.Models.DuToan;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanNganSachNhaNuocController : Controller
    {
        //
        // GET: /DuToanChiNganSachSuDung/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/DuToanNganSachNhaNuoc.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            //return View(sViewPath + "rptDuToanChiNganSachSuDung.aspx");
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToanNganSachNhaNuoc.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["sLNS"] = "2";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, String sLNS)
        {
      
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String sPath = "/Libraries/DuToan/PDF";// + DateTime.Now.ToString("yyyy/MM/dd");
            String path2 = Server.MapPath(sPath);
            HamChung.CreateDirectory(path2);
            NameFile = path2 + "" + FileName;
            return RedirectToAction("Index", new {iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS });
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String sLNS,String MaND,String iID_MaTrangThaiDuyet)
        {
           
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String  iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
                 FlexCelReport fr = new FlexCelReport();
                 fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanNganSachNhaNuocController");
                 LoadData(fr, sLNS, MaND, iID_MaTrangThaiDuyet);
                fr.SetValue("PhuLuc", "Phụ Lục 5-C");
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("TenTieuDe", "DỰ TOÁN NGÂN SÁCH NHÀ NƯỚC GIAO NĂM");
                fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
                fr.SetValue("Nam", iNamLamViec);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// hàm hiẻn thị dữ liệu báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        private void LoadData(FlexCelReport fr, String sLNS,String MaND, String iID_MaTrangThaiDuyet)
        {
            //lấy dữ liệu báo cáo
            DataTable data = DuToan_ReportModels.DT_DuToanNganSachNhaNuoc(MaND, iID_MaTrangThaiDuyet);
            
            fr.AddTable("ChiTiet", data); 
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sM,sTM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLNS;
            dtLNS = HamChung.SelectDistinct("Muc", dtMuc, "sLNS,sL,sK", "sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);
       
        }
        /// <summary>
        /// hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String sLNS, String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sLNS, MaND, iID_MaTrangThaiDuyet);
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
        /// hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String sLNS, String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sLNS, MaND, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTNSNhaNuoc.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String sLNS, String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sLNS, MaND, iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                xls.AutoPageBreaks();
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
