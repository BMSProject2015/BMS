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


namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptBangTongHopChungTuGocController : Controller
    {
        //
        // GET: /rptBangTongHopChungTuGoc/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptBangTongHopChungTuGoc.xls";
        public static String NameFile = "";
        public static String soCT = "";
        public static String ngayGS = "";
        public static String NoiDung = "";
        public ActionResult Index()
        {
           if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
           {
               HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptBangTongHopChungTuGoc.aspx";
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
         String sSoChungTu=Convert.ToString(Request.Form[ParentID+"_sSoChungTu"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            return RedirectToAction("Index", new { iNamLamViec = iNamLamViec, sSoChungTu = sSoChungTu });
        }

        public ExcelFile CreateReport(String path, String iNamLamViec, String sSoChungTu)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
           // string soCT = "";
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptBangTongHopChungTuGoc");
                DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                LoadData(fr, iNamLamViec, sSoChungTu);
                fr.SetValue("So", soCT);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("CucTaiChinh", CucTaiChinh);
                fr.SetValue("NgayGS", ngayGS);
                fr.SetValue("Ngay", ngay);
                fr.SetValue("NoiDung", NoiDung);
                fr.SetValue("Tien", CommonFunction.TienRaChu(tong));
                fr.Run(Result);
                return Result;
        }
        public long tong = 0;
        public clsExcelResult ExportToPDF(String iNamLamViec, String sSoChungTu)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, sSoChungTu);
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
        public clsExcelResult ExportToExcel(String iNamLamViec, String sSoChungTu)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, sSoChungTu);
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
        public ActionResult ViewPDF(String iNamLamViec, String sSoChungTu)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, sSoChungTu);
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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String sSoChungTu)
        {
            DataTable data = TongHopChungTuGoc(iNamLamViec, sSoChungTu);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            //DataTable data1 = Ngay();
            //data.TableName = "N";
            //fr.AddTable("N", data);
            //data.Dispose();
           
        }
        public DataTable TongHopChungTuGoc(String iNamLamViec, String sSoChungTu)
        {
            String SQL = " SELECT KT_ChungTu.sSoChungTu,KT_ChungTu.iNgay AS NgayCT,KT_ChungTu.iThang AS ThangCT,KT_ChungTu.iNamLamViec,KT_ChungTu.sSoChungTu,KT_ChungTuChiTiet.iNgay,KT_ChungTuChiTiet.iThang";
            SQL += " ,KT_ChungTuChiTiet.sSoChungTuChiTiet, KT_ChungTu.sNoiDung";
            SQL += " ,KT_ChungTuChiTiet.iID_MaDonVi_No,KT_ChungTuChiTiet.iID_MaDonVi_Co";
            SQL += ",KT_ChungTuChiTiet.iID_MaTaiKhoan_No,KT_ChungTuChiTiet.iID_MaTaiKhoan_Co,KT_ChungTuChiTiet.rSoTien";
            SQL += " FROM KT_ChungTuChiTiet";
            SQL += " INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =KT_ChungTuChiTiet.iID_MaChungTu";
            SQL += " WHERE KT_ChungTuChiTiet.iTrangThai =1";
            SQL += " AND KT_ChungTu.iID_MaChungTu=@iID_MaChungTu";
            //iNamLamViec !="": chứng từ gốc
            if (iNamLamViec == "1")
            {
                SQL += " AND KT_ChungTuChiTiet.iID_MaTaiKhoan_No !='' AND KT_ChungTuChiTiet.iID_MaTaiKhoan_No IS NOT NULL AND KT_ChungTuChiTiet.iID_MaTaiKhoan_Co !='' AND KT_ChungTuChiTiet.iID_MaTaiKhoan_Co IS NOT NULL";
            }
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", sSoChungTu);
           // cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    tong += long.Parse(dt.Rows[i]["rSoTien"].ToString());
                soCT = HamChung.ConvertToString(dt.Rows[0]["sSoChungTu"]);
                ngayGS = "Ngày  " + HamChung.ConvertToString(dt.Rows[0]["NgayCT"]) + " Tháng  " +
                    HamChung.ConvertToString(dt.Rows[0]["ThangCT"]) + "  Năm  " +
                    HamChung.ConvertToString(dt.Rows[0]["iNamLamViec"]);
                NoiDung = HamChung.ConvertToString(dt.Rows[0]["sNoiDung"]);
            }
            return dt;
        }
       
        public static DataTable Lay_SoChungTu(String iNamLamViec)
        {
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT DISTINCT sSoChungTu
                                                FROM KT_ChungTu 
                                                WHERE iTrangThai =1  AND iNamLamViec = @iNamLamViec                                         
                                                ORDER BY sSoChungTu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}
