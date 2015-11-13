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
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.GIA
{
    public class rptGia_7aController : Controller
    {
        //
        // GET: /rptGia_7a/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/GIA/rptGia_7a.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/GIA/rptGia_7a.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult List(String ParentID)
        {
            ViewData["Controller"] = "rptGia_7a";
            ViewData["iID_LoaiDonVi"] = 1;
            ViewData["iID_MaDonVi"] = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            ViewData["iID_MaLoaiHinh"] = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiHinh"]);
            return View(sViewPath + "GIA/List.aspx");
        }
        /// <summary>
        ///  Hàm lấy các giá trị trên form khi thực hiện submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, NamLamViec = NamLamViec });
        }

        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String NamLamViec)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptGia_7a");

            LoadData(fr, iID_MaDonVi, iID_MaTrangThaiDuyet, NamLamViec);
            fr.SetValue("PhuLuc", "Biểu 7a");
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("TenDonVi", ReportModels.CauHinhTenDonViSuDung(2));

            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public DataTable rptGia_7a(String iID_MaDonVi, String iID_MaTrangThaiDuyet, String NamLamViec)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeGia) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = "1=1";
            }

            String SQL = string.Format(@"SELECT iID_MaChiTietGia,iID_MaSanPham,iID_MaLoaiHinh,iID_MaDonVi
                                            ,sTen,sMa,rSoLuong,sQuyCach,iDM_MaDonViTinh
                                            ,(SELECT TOP 1 tbl1.rTien_DangThucHien FROM DM_SanPham_DanhMucGia AS tbl1 
                                            WHERE tbl1.iID_MaChiTietGia = tbl0.iID_MaChiTietGia AND LTRIM(RTRIM(sKyHieu)) = '10') AS TienDangThucHien
                                            ,(SELECT TOP 1 tbl2.rTien_DV_DeNghi FROM DM_SanPham_DanhMucGia AS tbl2 
                                            WHERE tbl2.iID_MaChiTietGia = tbl0.iID_MaChiTietGia AND LTRIM(RTRIM(sKyHieu)) = '10') AS TienDVDeNghi
                                        FROM DM_SanPham_ChiTietGia AS tbl0 WHERE tbl0.iTrangThai = 1 AND tbl0.iID_LoaiDonVi = 1
                                        AND YEAR(tbl0.dNgayTao) = @NamLamViec AND tbl0.iID_MaDonVi = @iID_MaDonVi
                                        AND tbl0.iID_MaSanPham IN (SELECT DM_SanPham.iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1)
                                        ORDER BY tbl0.sTen");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String NamLamViec)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable data = rptGia_7a(iID_MaDonVi, iID_MaTrangThaiDuyet, NamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String iID_MaTrangThaiDuyet, String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iID_MaTrangThaiDuyet, NamLamViec);
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
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iID_MaTrangThaiDuyet, String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iID_MaTrangThaiDuyet, NamLamViec);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangGiaiTrinhChiTietTinhGiaSanPham.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDonVi, String iID_MaTrangThaiDuyet, String NamLamViec)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iID_MaTrangThaiDuyet, NamLamViec);
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
        public String tendonvi(String ID)
        {
            String dt;
            if (String.IsNullOrEmpty(ID)) return null;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            dt = Connection.GetValueString(cmd,"");
            cmd.Dispose();
            return dt;
        }

        public static DataTable Danhsach_DonVi()
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT SP.iID_MaDonVi,DV.sTen
                                            FROM DM_SanPham AS SP
                                            INNER JOIN NS_DonVi AS DV ON SP.iID_MaDonVi=DV.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

    }
}
