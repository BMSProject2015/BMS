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
using System.Text;
namespace VIETTEL.Report_Controllers.KeToan
{
    public class rptKeToan_InBiaController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathSoCai_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_SoCai_A3.xls";
        private const String sFilePathSoCai_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_SoCai_A4.xls";

        private const String sFilePathSoPhanHo_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_SoPhanHo_A3.xls";
        private const String sFilePathSoPhanHo_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_SoPhanHo_A4.xls";

        private const String sFilePathSoChiTietTaiKhoan_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_SoChiTietTaiKhoan_A3.xls";
        private const String sFilePathSoChiTietTaiKhoan_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_SoChiTietTaiKhoan_A4.xls";

        private const String sFilePathBangKeChungTu_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_BangKeChungTu_A3.xls";
        private const String sFilePathBangKeChungTu_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_BangKeChungTu_A4.xls";

        private const String sFilePathBiaTaiSan_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_BiaTaiSan_A3.xls";
        private const String sFilePathBiaTaiSan_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_InBia_BiaTaiSan_A4.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_InBia.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec"]);
            String ThangLamViec = Convert.ToString(Request.Form[ParentID + "_ThangLamViec"]);
            String LoaiNam_Thang = Convert.ToString(Request.Form[ParentID + "_LoaiNam_Thang"]);
            String LoaiBiaSo = Convert.ToString(Request.Form[ParentID + "_LoaiBiaSo"]);
            String TenDonVi = Convert.ToString(Request.Form[ParentID + "_TenDonVi"]);
            String LoaiBia = Convert.ToString(Request.Form[ParentID + "_LoaiBia"]);
            String LoaiKhoGiay = Convert.ToString(Request.Form[ParentID + "_MaLoaiGiay"]);
            String TuTo = Convert.ToString(Request.Form[ParentID + "_iTuTo"]);
            String DenTo = Convert.ToString(Request.Form[ParentID + "_iDenTo"]);
            String sLoaiMau = Convert.ToString(Request.Form[ParentID + "_sLoaiMau"]);
            String sTenMau = Convert.ToString(Request.Form[ParentID + "_sTenMau"]); 
            return RedirectToAction("Index", new {NamLamViec=NamLamViec,ThangLamViec=ThangLamViec,LoaiNam_Thang=LoaiNam_Thang, LoaiBiaSo = LoaiBiaSo, TenDonVi = TenDonVi, LoaiBia = LoaiBia, LoaiKhoGiay = LoaiKhoGiay, TuTo = TuTo, DenTo=DenTo,pageload=1,sLoaiMau=sLoaiMau,sTenMau=sTenMau });
        }
        public ExcelFile CreateReport(String path, String NamLamViec,String ThangLamViec,String LoaiNam_Thang,String LoaiBiaSo,String LoaiBia,String TenDonVi,String LoaiKhoGiay,String TuTo,String DenTo,String sLoaiMau,String sTenMau)
        {
           
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NamThang = "";
            if (LoaiNam_Thang == "0")
            {
                NamThang = "Năm " + NamLamViec;
            }
            else if(LoaiNam_Thang=="2")
            {
                NamThang = "Quý" + ThangLamViec + " Năm " + NamLamViec;
            }
            else
            {
                NamThang = "Tháng " + ThangLamViec + " Năm " + NamLamViec;
            }
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            if (LoaiBia == "2")
            {
                fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_InBia");
                NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            }
            else
            {
                fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_InBia",false);
            }
            String So = "";
            if (LoaiBiaSo == "4")
            {
                So = "Từ số " + TuTo + " Đến số " + DenTo;
            }
            if (String.IsNullOrEmpty(sLoaiMau))
                sLoaiMau = "báo cáo";
            if (String.IsNullOrEmpty(sTenMau))
                sTenMau = "Kê khai tài sản cố định";
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("NamThang", NamThang);
            fr.SetValue("TENDV", TenDonVi.ToUpper());
            fr.SetValue("So", So.ToUpper());
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("sMau", sLoaiMau.ToUpper());
            fr.SetValue("sTen", sTenMau.ToUpper());
            fr.Run(Result);
            return Result;
            
        }
      
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String LoaiNam_Thang, String LoaiBiaSo, String LoaiBia, String TenDonVi, String LoaiKhoGiay, String TuTo, String DenTo, String sLoaiMau, String sTenMau)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (LoaiBiaSo == "1")
            {
                if (LoaiKhoGiay == "1")
                    DuongDanFile = sFilePathSoCai_A3;
                else
                    DuongDanFile = sFilePathSoCai_A4;
            }
            else if (LoaiBiaSo == "2")
            {
                if (LoaiKhoGiay == "1")
                    DuongDanFile = sFilePathSoPhanHo_A3;
                else
                    DuongDanFile = sFilePathSoPhanHo_A4;
            }
            else if (LoaiBiaSo == "3")
            {
                if (LoaiKhoGiay == "1")
                    DuongDanFile = sFilePathSoChiTietTaiKhoan_A3;
                else
                    DuongDanFile = sFilePathSoChiTietTaiKhoan_A4;
            }
            else  if (LoaiBiaSo == "4")
            {
                if (LoaiKhoGiay == "1")
                    DuongDanFile = sFilePathBangKeChungTu_A3;
                else
                    DuongDanFile = sFilePathBangKeChungTu_A4;
            }
            else
            {

                if (LoaiKhoGiay == "1")
                    DuongDanFile = sFilePathBiaTaiSan_A3;
                else
                    DuongDanFile = sFilePathBiaTaiSan_A4;
            }

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, LoaiNam_Thang, LoaiBiaSo, LoaiBia, TenDonVi, LoaiKhoGiay, TuTo, DenTo,sLoaiMau,sTenMau);
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
        public static DataTable LoaiBiaSo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoaiBiaSo", typeof(String));
            dt.Columns.Add("TenLoaiBiaSo", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "Nhật ký sổ cái";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "Sổ phân hộ";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "3";
            R3[1] = "Sổ chi tiết tài khoản";

            DataRow R5 = dt.NewRow();
            dt.Rows.Add(R5);
            R5[0] = "5";
            R5[1] = "Bìa tài sản";

            DataRow R4 = dt.NewRow();
            dt.Rows.Add(R4);
            R4[0] = "4";
            R4[1] = "Bảng kê chứng từ";
            dt.Dispose();      
            return dt;
        }
        public static DataTable LoaiBia()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoaiBia", typeof(String));
            dt.Columns.Add("TenLoaiBia", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "In bìa ngoài";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "In bìa trong";
            dt.Dispose();
            return dt;
        }
        public static DataTable LoaiKhoGiay()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoaiGiay", typeof(String));
            dt.Columns.Add("TenLoaiGiay", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "In khổ giấy a3";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "In khổ giấy a4";
            dt.Dispose();
            return dt;
        }       

    }
}
