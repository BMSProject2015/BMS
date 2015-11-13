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
namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKB_ThongTriTongHopController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_ThongTriTongHop.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
        if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
        {
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_ThongTriTongHop.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec, String iThang, int ChiSo)
        {
            String sMaChungTuChiTiet = Request.Form[ParentID + "_iID_MaChungTu"];
            String[] arrMaChungTuChiTiet = sMaChungTuChiTiet.Split(',');
            String iID_MaChungTu = arrMaChungTuChiTiet[ChiSo];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            ViewData["siID_MaChungTu"] = sMaChungTuChiTiet;
            ViewData["ChiSo"] = ChiSo;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThang"] = iThang;

            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_ThongTriTongHop.aspx";
            return View(sViewPath + "ReportView.aspx");

        }
        public ExcelFile CreateReport(String path,String iID_MaChungTu)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            FlexCelReport fr = new FlexCelReport();
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr,iID_MaChungTu);
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_ThongTriTongHop");
            //fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("So", iID_MaChungTu);
            //fr.SetValue("Thang", iThang);
            fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("Nams", DateTime.Now.Year);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("ngay", ngay);
            fr.SetValue("ThangCT", CommonFunction.LayTruong("KTKB_ChungTuChiTiet", "iID_MaChungTu", iID_MaChungTu, "iThangCT"));
            fr.SetValue("NgayCT", CommonFunction.LayTruong("KTKB_ChungTuChiTiet", "iID_MaChungTu", iID_MaChungTu, "iNgayCT"));
            fr.Run(Result);
            return Result;
        }

        public clsExcelResult ExportToPDF(String iNamLamViec, String iThang, String iID_MaChungTu)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),iID_MaChungTu);

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
        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String iID_MaChungTu)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),iID_MaChungTu);
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
        public ActionResult ViewPDF(string MaND, String iID_MaChungTu)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            dtCauHinh.Dispose();            
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),iID_MaChungTu);
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
        private void LoadData(FlexCelReport fr,String iID_MaChungTu)
        {
            DataTable data = GET_dtChiTiet(iID_MaChungTu);
            data.TableName = "dt";
            fr.AddTable("dt", data);            
            data.Dispose();            
        }

        public DataTable GET_dtChiTiet(String iID_MaChungTu)
        {

            String SQL = "SELECT sTenDonVi_Nhan,SUM(rDTRut) AS rDTRut";
            SQL += " FROM KTKB_ChungTuChiTiet ";
            SQL += " WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaChungTu=@iID_MaChungTu ";
            SQL += " GROUP BY sTenDonVi_Nhan";
            SQL += " ORDER BY sTenDonVi_Nhan";

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int a = dt.Rows.Count;
            if (a < 25 && a > 0)
            {
                for (int i = 0; i < (25 - a); i++)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, a + 1);
                }
            }
            dt.Dispose();
            return dt;
        }

    }
}
