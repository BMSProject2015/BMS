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
    public class rptKeToan_BangCanDoiSoDuController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_BangCanDoiSoDu.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_BangCanDoiSoDu.aspx";
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
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_BangCanDoiSoDu.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String TrangThai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_BangCanDoiSoDu");
            LoadData(fr, NamLamViec, ThangLamViec, TrangThai);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam = "Tháng "+ThangLamViec+" năm " + NamLamViec;

            String NamTruoc = "Số năm " + (Convert.ToInt16(NamLamViec) - 1) + " chuyển sang";
            String NamNay = "Số trong năm " + NamLamViec;
            String NamSau = "Số trong năm " + (Convert.ToInt16(NamLamViec) + 1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangNam", ThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NamTruoc", NamTruoc);
            fr.SetValue("NamNay", NamNay);
            fr.SetValue("NamSau", NamSau);
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String TrangThai)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, TrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_QuyetToanNam.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String TrangThai)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, TrangThai);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String ThangLamViec, String TrangThai)
        {

            DataTable data = KeToan_QuyetToanNam(NamLamViec, ThangLamViec, TrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
        }
        public DataTable KeToan_QuyetToanNam(String NamLamViec, String ThangLamViec, String TrangThai)
        {
            String SQL = "";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            if (TrangThai != "0")
            {
                DK = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            SQL = String.Format(@"SELECT CT.iID_MaTaiKhoan,sTen,rPS_No,rPS_Co FROM (
SELECT iID_MaTaiKhoan,SUM(rPS_No) as rPS_No,SUM(rPS_Co) as rPS_Co
FROM(
SELECT SUBSTRING(iID_MaTaiKhoan_No,1,3) as iID_MaTaiKhoan,SUM(rSoTien) as rPS_No,rPS_Co=0
FROM KT_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec AND iThangCT<=@iThangLamViec AND iID_MaTaiKhoan_No<>'' AND  iTrangThai=1 {0}
GROUP BY SUBSTRING(iID_MaTaiKhoan_No,1,3)

UNION 
SELECT SUBSTRING(iID_MaTaiKhoan_Co,1,3) as iID_MaTaiKhoan,rPS_No=0,SUM(rSoTien) as rPS_Co
FROM KT_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec AND iThangCT<=@iThangLamViec  AND iID_MaTaiKhoan_Co<>'' AND  iTrangThai=1 {0}
GROUP BY SUBSTRING(iID_MaTaiKhoan_Co,1,3)) as a
GROUP BY iID_MaTaiKhoan
HAVING SUM(rPS_No)-SUM(rPS_Co)<>0) CT
INNER JOIN (SELECT iID_MaTaiKhoan,sTen FROM KT_TaiKhoan WHERE LEN(iID_MaTaiKhoan)=3 AND iNam=@iNamLamViec AND iTrangThai=1) b
ON CT.iID_MaTaiKhoan=b.iID_MaTaiKhoan where SUBSTRING(CT.iID_MaTaiKhoan,1,1)<>0
", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iThangLamViec", ThangLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

    }
}
