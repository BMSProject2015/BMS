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
    public class rptGia_2Controller : Controller
    {
        //
        // GET: /rptGia_2/
        // GET: /rptGia_2b/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/GIA/rptGia_2.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/GIA/rptGia_2.aspx";
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
            ViewData["Controller"] = "rptGia_2";
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
        public ActionResult EditSubmit(String ParentID, String iID_MaSanPham, String iID_MaChiTietGia)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaSanPham = iID_MaSanPham, iID_MaChiTietGia = iID_MaChiTietGia });
        }

        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptGia_2");
            LoadData(fr, iID_MaTrangThaiDuyet, iID_MaSanPham, iID_MaChiTietGia);
            fr.SetValue("PhuLuc", "Biểu số 2");
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public DataTable rptGia_2(String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
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

            String SQL = string.Format(@"SELECT iID_MaDanhMucGia, iID_MaDanhMucGia_Cha, sKyHieu,sTen
                                        ,rTien_DangThucHien AS TienDangThucHien
                                        ,rTien_DV_DeNghi AS TienDVDeNghi
                                        ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE DM_SanPham_DanhMucGia.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonViTinh
                                        FROM DM_SanPham_DanhMucGia
                                        WHERE iTrangThai=1 AND iID_MaChiTietGia=@iID_MaChiTietGia AND UPPER(iID_MaVatTu) = 'DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD'
                                        ORDER BY iSTT ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
            dt = Connection.GetDataTable(cmd);
            DataTable dtKetQua = new DataTable();
            foreach (DataColumn col in dt.Columns)
            {
                DataColumn newCol = dtKetQua.Columns.Add(col.ColumnName);
                newCol.DataType = col.DataType;
            }
            foreach (DataRow row in dt.Rows)
            {
                String iID_MaDanhMucGia = Convert.ToString(row["iID_MaDanhMucGia"]);
                String iID_MaDanhMucGia_Cha = Convert.ToString(row["iID_MaDanhMucGia_Cha"]);
                DataRow newRow = dtKetQua.Rows.Add();
                foreach (DataColumn col in dt.Columns)
                {
                    newRow[col.ColumnName] = row[col.ColumnName];
                }
                if (iID_MaDanhMucGia_Cha != "0")
                {
                    String query = String.Format((@"SELECT CONVERT(nvarchar(25),N'Trong đó: NSQP bảo đảm') AS sTen
                                                ,SUM(rTien_DangThucHien) as TienDangThucHien
                                                ,SUM(rTien_DV_DeNghi) AS TienDVDeNghi
                                                FROM DM_SanPham_DanhMucGia
                                                WHERE iTrangThai=1 AND bNganSach = 1 AND UPPER(iID_MaVatTu) != 'DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD'
                                                AND iID_MaChiTietGia=@iID_MaChiTietGia AND iID_MaDanhMucGia_Cha = @iID_MaDanhMucGia_Cha"));
                    SqlCommand cm = new SqlCommand(query);
                    cm.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                    cm.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia);
                    DataTable d = Connection.GetDataTable(cm);
                    if (d.Rows.Count > 0)
                    {
                        DataRow extraRow = dtKetQua.Rows.Add();
                        foreach (DataColumn col in d.Columns)
                        {
                            extraRow[col.ColumnName] = d.Rows[0][col.ColumnName];
                        }
                    }
                }
            }
            return dtKetQua;
        }
        public DataTable SanPham(String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
        {
             DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT TOP 1 iID_MaSanPham, sTen, iID_MaLoaiHinh, iID_MaDonVi, rSoLuong, sQuyCach, iDM_MaDonViTinh
                                            ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE CTG.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonViTinh
                                            ,(SELECT  NS_DonVi.sTen FROM NS_DonVi WHERE NS_DonVi.iID_MaDonVi=CTG.iID_MaDonVi AND iNamLamViec_DonVi=@iNamLamViec) AS sTen_DonVi
                                            FROM DM_SanPham_ChiTietGia AS CTG
                                            WHERE CTG.iID_MaSanPham = @iID_MaSanPham AND CTG.iID_MaChiTietGia = @iID_MaChiTietGia");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
        {

            DataTable data = rptGia_2(iID_MaTrangThaiDuyet, iID_MaSanPham, iID_MaChiTietGia);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable dtSP = SanPham(iID_MaTrangThaiDuyet, iID_MaSanPham, iID_MaChiTietGia);
            dtSP.TableName = "SanPham";
            fr.AddTable("SanPham", dtSP);
            if (dtSP.Rows.Count > 0)
            {
                switch (dtSP.Rows[0]["iID_MaLoaiHinh"].ToString())
                {
                    case "1":
                        fr.SetValue("LoaiHinh", "Sửa chữa lớn");
                        break;
                    case "2":
                        fr.SetValue("LoaiHinh", "Sửa chữa vừa");
                        break;
                    case "3":
                        fr.SetValue("LoaiHinh", "Sửa chữa nhỏ");
                        break;
                }
            }
            dtSP.Dispose();
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaSanPham, iID_MaChiTietGia);
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
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaSanPham, iID_MaChiTietGia);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangTongHopTinhGiaSanPham";
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
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaSanPham, String iID_MaChiTietGia)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaSanPham, iID_MaChiTietGia);
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
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            if (String.IsNullOrEmpty(ID)) return null;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            dt = Connection.GetDataTable(cmd);
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
