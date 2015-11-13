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

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_InTaiKhoanChiTietController : Controller
    {
        //
        // GET: /rptKeToanTongHop_InTaiKhoanChiTiet/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_InTaiKhoanChiTiet.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                //ViewData["PageLoad"] = "0";
                ViewData["PageLoad"] = "1";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_InTaiKhoanChiTiet.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }
        public ActionResult EditSubmit(String ParentID)
        {
           
            ViewData["PageLoad"] = "1";
         
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_QuyetToanNam.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String iID_MaTaiKhoan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_InTaiKhoanChiTiet");
            LoadData(fr, iID_MaTaiKhoan);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
          
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            fr.SetValue("NgayThangNam", NgayThangNam);
          
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
         
            fr.SetValue("Ngay", DateTime.Now.Day);
            fr.SetValue("Thang", DateTime.Now.Month);
            fr.SetValue("Nam", DateTime.Now.Year);
            if (iID_MaTaiKhoan == Guid.Empty.ToString())
            {
                fr.SetValue("TaiKhoan", "");
            }
            else
            {
                fr.SetValue("TaiKhoan", iID_MaTaiKhoan);  
            }
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String iID_MaTaiKhoan)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTaiKhoan);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DanhMuc_TaiKhoanChiTiet.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ActionResult ViewPDF(String iID_MaTaiKhoan)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTaiKhoan);
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
        private void LoadData(FlexCelReport fr, String iID_MaTaiKhoan )
        {

            DataTable data = KeToan_QuyetToanNam(iID_MaTaiKhoan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            
        }
        public DataTable KeToan_QuyetToanNam(String iID_MaTaiKhoan)
        {
            String SQL = "";
            SqlCommand cmd =new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaTaiKhoan) || iID_MaTaiKhoan == Guid.Empty.ToString())
            {
                SQL = String.Format(@"select iID_MaTaiKhoanDanhMucChiTiet,sKyHieu,sTen,iID_MaTaiKhoanDanhMucChiTiet_Cha,bLaHangCha, ISNULL(rSoTienNgoaiTe,0) as rSoTienNgoaiTe,sTenNgoaiTe,ISNULL(iID_MaNgoaiTe,0) as iID_MaNgoaiTe,sXauNoiMa from KT_TaiKhoanDanhMucChiTiet order by skyhieu");
            }
            else
            {

                SQL = String.Format(@"SELECT iID_MaTaiKhoanDanhMucChiTiet,sKyHieu,sTen,iID_MaTaiKhoanDanhMucChiTiet_Cha,bLaHangCha, ISNULL(rSoTienNgoaiTe,0) as rSoTienNgoaiTe,sTenNgoaiTe,ISNULL(iID_MaNgoaiTe,0) as iID_MaNgoaiTe,sXauNoiMa FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet and iID_MaTaiKhoan=@iID_MaTaiKhoan) ORDER BY ct.sKyHieu");
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            }
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int ThuTu = 0;
            return TaiKhoanDanhMucChiTietModels.getstring(dt, 0, ref ThuTu,true);
        }


    }
}
