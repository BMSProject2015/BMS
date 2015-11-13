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
    public class rptNhanSu_DanhSachController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/NhanSu/rptNhanSu_DanhSach.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_DanhSach.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["PageLoad"] = "0";
            ViewData["Nam"] = DateTime.Now.Year;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Nam = Convert.ToString(Request.Form[ParentID + "_Nam"]);
            ViewData["PageLoad"] = "1";
            ViewData["Nam"] = Nam;
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_DanhSach.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, string Nam)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            //tính tổng tiền
            if (Nam == null) Nam = "0";
            DataTable dt = LayDanhSach(Nam);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_DanhSach");
            LoadData(fr, Nam);
            if (Nam != "0" & Nam != "")
                fr.SetValue("Nam", "NĂM " + Nam.ToUpper());
            else
                fr.SetValue("Nam", "");

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
        private void LoadData(FlexCelReport fr, String Nam)
        {

            DataTable data = LayDanhSach(Nam);
            data.TableName = "NS";
            fr.AddTable("NS", data);
            DataTable dtGroup;
            dtGroup = HamChung.SelectDistinct("Group", data, "iID_MaDonVi,sTenDonVi", "iID_MaDonVi,sTenDonVi");
            fr.AddTable("GROUP", dtGroup);

            data.Dispose();
            dtGroup.Dispose();
        }

        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String DoiTuong, String LoaiIn, String Cap, String Nam)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Nam);
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
        public clsExcelResult ExportToExcel(String Nam)
        {
            String DuongDanFile = sFilePath;


            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Nam);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptNhanSu_DanhSach.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Nam)
        {
            //ViewData["bBaoCaoTH"] = "False";
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Nam);
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
        public DataTable LayDanhSach(String Nam)
        {
            DataTable dtDoiTuong = new DataTable();

            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT iID_MaCanBo, sHoTen,sSoHieuCBCC, CB_CanBo.iID_MaTinh, CB_DM_Tinh.sTenTinh" +
                " ,CB_CanBo.iID_MaHuyen , CB_DM_Huyen.sTenHuyen,CB_CanBo.iID_MaXaPhuong,CB_DM_XaPhuong.sTenXaPhuong" +
                " , CB_CanBo.sDiaChi, sCMND, sNoiCap, " +
                " CONVERT(VARCHAR(10), dNgayVaoDang, 103) as dNgayVaoDang, CONVERT(VARCHAR(10), dNgayChinhThuc, 103) as dNgayChinhThuc " +
                ", CONVERT(VARCHAR(10), dNgaySinh, 103) as dNgaySinh, iID_MaNgachLuong " +
                ", CB_ChucVu.sTen as ChucVuHienTai, iID_MaBacLuong,CB_CanBo.iID_MaDonVi, sTenDonVi,sHoKhauThuongTru,NS_DonVi.iID_MaLoaiDonVi,NS_DonVi.sTenLoaiDonVi " +
                ",CB_CanBo.sNoiDaoTao, CB_CanBo.sID_MaTrinhDoChuyenMonCaoNhat as MaTrinhDo, ('C' +  CAST(iCap AS nvarchar(50))) as CapTC " +
                ", CB_CanBo.sID_ChucVuHienTai as iID_ChucVuHienTai " +
                " from (select *, LTRIM(RTRIM(sHoDem + ' ' + sTen)) as sHoTen from CB_CanBo) as CB_CanBo " +
                " left join CB_DM_Tinh ON CB_CanBo.iID_MaTinh = CB_DM_Tinh.iID_MaTinh" +
                " left join CB_DM_Huyen ON CB_CanBo.iID_MaHuyen = CB_DM_Huyen.iID_MaHuyen " +
                " left join CB_DM_XaPhuong ON CB_CanBo.iID_MaXaPhuong = CB_DM_XaPhuong.iID_MaXaPhuong " +
                " left join CB_ChucVu ON CB_CanBo.sID_ChucVuHienTai = CB_ChucVu.sID_MaChucVu" +
                " left join NS_DonVi ON CB_CanBo.iID_MaDonVi = NS_DonVi.iID_MaDonVi " +
                " WHERE CB_CanBo.iTrangThai=1 ";

            if (Nam != "0")
            {
                SQLDSNS += " AND year(CB_CanBo.dNgayVaoCQ) <= @Nam";
            }

            SQLDSNS += " order by CB_CanBo.sTen asc";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);

            if (Nam != "0")
            {
                // theo nam
                cmdThangNay.Parameters.AddWithValue("@Nam", Convert.ToInt32(Nam));
            }
            dtDoiTuong = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();

            return dtDoiTuong;
        }

    }
}
