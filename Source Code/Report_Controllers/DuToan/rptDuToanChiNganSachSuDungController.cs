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
    public class rptDuToanChiNganSachSuDungController : Controller
    {
        //
        // GET: /DuToanChiNganSachSuDung/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/DTCNS_SuDung.xls";
        public static String NameFile="";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDTCNS_SuDung.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            
            String FileName =DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()+".pdf";
            String sPath = "/Libraries/DuToan/PDF";// + DateTime.Now.ToString("yyyy/MM/dd");
            String path2= Server.MapPath(sPath);
            HamChung.CreateDirectory(path2);
             NameFile= path2 + "" + FileName;
             return RedirectToAction("Index", new {  iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet)
        {
           
                XlsFile Result = new XlsFile(true);
                DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
                if (dt.Rows.Count > 0)
                {
                    iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                    iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                    iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
                }
                Result.Open(path);
                String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
                FlexCelReport fr = new FlexCelReport();
                fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanChiNganSachSuDungController");
                LoadData(fr,  MaND, iID_MaTrangThaiDuyet);
                fr.SetValue("Ngay",NgayThang);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("TenTieuDe", " DỰ TOÁN NGÂN SÁCH SỬ DỤNG ");
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// hàm hiển thị dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr,  String MaND, String iID_MaTrangThaiDuyet)
        {
            String dsKhoi = "687860da-8dc1-4bca-b810-5cc7d6634846,2ea265f1-6db9-42d8-8fb3-067356f31bc9,bb052603-be5d-494d-a4bf-7651894d5a0a";
            DataTable data = DuToan_ReportModels.DT_DuToanChiNganSachSuDung(dsKhoi, MaND, iID_MaTrangThaiDuyet);
            data.TableName = "NSSuDung";
            fr.AddTable("NSSuDung", data);

            DataTable dtTieuMuc ;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa","sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sM", "sM,sMoTa","sM,sTM");
            fr.AddTable("Muc", dtMuc);
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
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
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

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
                clsResult.FileName = "DTChiNganSachSuDung.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
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
