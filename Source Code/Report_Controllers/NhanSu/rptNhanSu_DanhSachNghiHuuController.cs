using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.NhanSu
{
    public class rptNhanSu_DanhSachNghiHuuController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/NhanSu/rptNhanSu_DanhSachNghiHuu.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_DanhSachNghiHuu.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["PageLoad"] = "0";
            ViewData["Province"] = "";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Province = Convert.ToString(Request.Form[ParentID + "_Province"]);
            ViewData["PageLoad"] = "1";
            ViewData["Province"] = Province;
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_DanhSachNghiHuu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Province"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, string Province)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            //tính tổng tiền
            if (Province == null) Province = "0";
            DataTable dt = LayDanhSach(Province);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_DanhSachNghiHuu");
            LoadData(fr, Province);
            if (Province != "0" & Province != "")
            {
                string sProvinceName = Province_Get_Ten(Province);
                fr.SetValue("Province", "ĐANG SINH SỐNG TẠI " + sProvinceName.ToUpper());
            }
            else
                fr.SetValue("Province", "");

            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String Province)
        {

            DataTable data = LayDanhSach(Province);
            data.TableName = "NS";
            fr.AddTable("NS", data);

            data.Dispose();
        }

        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Province)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Province);
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
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Province)
        {
            String DuongDanFile = sFilePath;


            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Province);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptNhanSu_DanhSachNghiHuu.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Province)
        {
            //ViewData["bBaoCaoTH"] = "False";
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Province);
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
        /// Data của báo cáo
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public DataTable LayDanhSach(String Province)
        {
            DataTable dtDoiTuong = new DataTable();

            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT iID_MaCanBo, sHoTen,sSoHieuCBCC, CB_CanBo.iID_MaTinh" +
                " ,CB_CanBo.iID_MaHuyen" +
                " , CB_CanBo.sDiaChi, sCMND, sNoiCap " +
                ", year(dNgaySinh) as NamSinh, iID_MaNgachLuong " +
                ", CB_ChucVu.sTen as ChucVuHienTai, iID_MaBacLuong,CB_CanBo.iID_MaDonVi, NS_DonVi.sTen as sTenDonVi,sHoKhauThuongTru,NS_DonVi.iID_MaLoaiDonVi,NS_DonVi.sTenLoaiDonVi " +
                ", CB_CanBo.sID_ChucVuHienTai as iID_ChucVuHienTai , CB_LyDoTangGiam.sTen as sTenLyDoTangGiam " +
                " from (select *, LTRIM(RTRIM(sHoDem + ' ' + sTen)) as sHoTen from CB_CanBo) as CB_CanBo " +
                " left join CB_ChucVu ON CB_CanBo.sID_ChucVuHienTai = CB_ChucVu.sID_MaChucVu" +
                " left join NS_DonVi ON CB_CanBo.iID_MaDonVi = NS_DonVi.iID_MaDonVi " +
                " left join CB_LyDoTangGiam ON CB_CanBo.iID_MaLyDoTangGiam = CB_LyDoTangGiam.iID_MaLyDoTangGiam" +
                " WHERE CB_CanBo.iTrangThai=1 AND CB_CanBo.iID_MaTinhTrangCanBo = -1 AND CB_CanBo.iID_MaLyDoTangGiam = 1 "; // iID_MaLyDoTangGiam: 1 la nghi huu
            //iID_MaTinhTrangCanBo: -1: la giam; 1: tang; 0 la binh thuong
            if (Province != "0")
            {
                SQLDSNS += " AND CB_CanBo.iID_MaTinh = @Province";
            }

            SQLDSNS += " order by CB_CanBo.sTen asc";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);

            if (Province != "0")
            {
                // theo tỉnh
                cmdThangNay.Parameters.AddWithValue("@Province",Province);
            }
            dtDoiTuong = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();

            return dtDoiTuong;
        }

        public string Province_Get_Ten(String iID_MaTinh)
        {
            DataTable dtDoiTuong = new DataTable();

            //lay du lieu thang quy
            string sTen = "";
            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT top 1 sTenTinh" +
                " from CB_DM_Tinh WHERE iID_MaTinh = @iID_MaTinh";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            cmdThangNay.Parameters.AddWithValue("@iID_MaTinh", iID_MaTinh);
            dtDoiTuong = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();
            if (dtDoiTuong != null)
            {
                if (dtDoiTuong.Rows.Count > 0) sTen = Convert.ToString(dtDoiTuong.Rows[0]["sTenTinh"]);
            }
            else
            {
                sTen = "";
            }
            return sTen;
        }
    }
}
