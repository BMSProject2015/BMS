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
namespace VIETTEL.Report_Controllers.KeToan.TienMat
{
    public class rptChiTietTaiKhoanTheoDonViController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienMat/rptChiTietTaiKhoanTheoDonVi.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptChiTietTaiKhoanTheoDonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "iThang2"]);
            return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2});
        }
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptChiTietTaiKhoanTheoDonVi");
            LoadData(fr, iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2);
            fr.SetValue("MaDV", iID_MaDonVi);
            fr.SetValue("Ma", iID_MaTaiKhoan);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang1", iThang1);
            fr.SetValue("Thang2", iThang2);
            fr.SetValue("Ngay", ngay);

            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2);
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
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2);
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
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2)
        {
            DataTable data = TongHopChungTuGoc(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            //DataTable data1 = SoDuDauKy(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iNgay1, iNgay2);
            //data.TableName = "ChiTiet1";
            //fr.AddTable("ChiTiet1", data1);
            //data1.Dispose();
            //DataTable data2 = LuyKe(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iNgay1, iNgay2);
            //data.TableName = "LuyKe";
            //fr.AddTable("LuyKe", data2);
            //data2.Dispose();
        }
        public DataTable TongHopChungTuGoc(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            String SQL = " SELECT KT_ChungTu.iSoChungTu,C.iNamLamViec AS iNam";
            SQL += ",C.iThang,C.iNgay,C.iNgayCT,C.iThangCT,C.sSoChungTuChiTiet";
            SQL += ",C.sNoiDung,C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan_DoiUng";
            SQL += ",C.rSoTien AS rSoPhatSinhNo,0 AS rSoPhatSinhCo";
            SQL += " FROM KT_ChungTuChiTiet as C";
            SQL += "  INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =C.iID_MaChungTu ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE C.iThang between @iThang1 and @iThang2 and C.iTrangThai = 1 AND C.iNamLamViec=@iNamLamViec";
            SQL += " AND C.iID_MaTaiKhoan_No= @iID_MaTaiKhoan {0}";
            SQL += " UNION";
            SQL += " SELECT  KT_ChungTu.iSoChungTu,C.iNamLamViec AS iNam";
            SQL += ",C.iThang,C.iNgay,C.iNgayCT,C.iThangCT,C.sSoChungTuChiTiet";
            SQL += " ,C.sNoiDung,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan";
            SQL += ",C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan_DoiUng,0 AS rSoPhatSinhNo";

            SQL += ",C.rSoTien AS rSoPhatSinhCo";
            SQL += " FROM KT_ChungTuChiTiet as C";
            SQL += "  INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =C.iID_MaChungTu ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi  ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE C.iNgay between @iThang1 and @iNgay2 and C.iTrangThai = 1 AND C.iNamLamViec=@iNamLamViec";
            SQL += " AND  C.iID_MaTaiKhoan_Co = @iID_MaTaiKhoan {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
       
    }
}
