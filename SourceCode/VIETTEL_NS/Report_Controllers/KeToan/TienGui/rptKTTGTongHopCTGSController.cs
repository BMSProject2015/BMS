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
using System.Text;

namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKTTGTongHopCTGSController : Controller
    {
        //
        // GET: /rptKTTGTongHopCTGS/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTGTongHopCTGS.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTGTongHopCTGS.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaChungTu = Convert.ToString(Request.Form[ParentID + "_iID_MaChungTu"]);
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTGTongHopCTGS.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String iID_MaChungTu)
        {
            String DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaChungTu);
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
         public clsExcelResult ExportToExcel(String iID_MaChungTu)
        {
            String DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaChungTu);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCaoKTTGTongHopCTGS.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
         public ExcelFile CreateReport(String path, String iID_MaChungTu)
         {
             XlsFile Result = new XlsFile(true);
             Result.Open(path);
             String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
             String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
             String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
             DataTable dt = dt_rptKTTGTongHopCTGS(iID_MaChungTu);
             String SoGhiSo = "";
             String Ngay = "";
             String Thang = "";
             String Nam = "";
             if (dt.Rows.Count > 0)
             {
                 SoGhiSo = dt.Rows[0]["sSoChungTuGhiSo"].ToString();
                 Ngay = dt.Rows[0]["iNgayCT"].ToString();
                 Thang =dt.Rows[0]["iThangCT"].ToString();
                 Nam = dt.Rows[0]["iNamLamViec"].ToString();
             }
             String NgayThang = Ngay + "/" + Thang + "/" + Nam;
             String Count = dt.Rows.Count.ToString();
             FlexCelReport fr = new FlexCelReport();
             fr = ReportModels.LayThongTinChuKy(fr, "rptKTTGTongHopCTGS");
             LoadData(fr,iID_MaChungTu);
             fr.SetValue("QuanKhu", QuanKhu);
             fr.SetValue("BoQuocPhong", BoQuocPhong);
             fr.SetValue("SoGhiSo", SoGhiSo);
             fr.SetValue("NgayThang", NgayThang);
             fr.SetValue("NgayThangNam", NgayThangNam);
             fr.SetValue("Count", Count);
             fr.Run(Result);
             return Result;
         }
         private void LoadData(FlexCelReport fr,String iID_MaChungTu)
         {
             DataTable data = dt_rptKTTGTongHopCTGS(iID_MaChungTu);
             data.TableName = "ChiTiet";
             fr.AddTable("ChiTiet", data);
             data.Dispose();
         }
         public static DataTable dt_rptKTTGTongHopCTGS(String iID_MaChungTu)
         {
             String SQL=String.Format(@"SELECT iID_MaChungTu
	                                           ,sSoChungTuGhiSo
                                               ,sSoChungTuChiTiet
	                                           ,iNgayCT
	                                           ,iThangCT
	                                           ,CONVERT(nvarchar(10),iNgayCT) +'-'+CONVERT(nvarchar(10),iThangCT) as NgayThang
	                                           ,iID_MaDonVi_No
	                                           ,iID_MaDonVi_Co
	                                           ,iID_MaDonVi_Nhan
	                                           ,iID_MaTaiKhoan_No
	                                           ,iID_MaTaiKhoan_Co
                                               ,sNoiDung
                                               ,iNamLamViec
	                                           ,SUM(rSoTien) as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaChungTu=@iID_MaChungTu
                                        GROUP BY iID_MaChungTu
	                                           ,sSoChungTuGhiSo
	                                           ,iNgayCT
	                                           ,iThangCT
	                                           ,iID_MaDonVi_No
	                                           ,iID_MaDonVi_Co
	                                           ,iID_MaDonVi_Nhan
	                                           ,iID_MaTaiKhoan_No
	                                           ,iID_MaTaiKhoan_Co
                                               ,sSoChungTuChiTiet
                                               ,sNoiDung
                                               ,iNamLamViec
                                        ORDER BY iThangCT,iNgayCT
");
             SqlCommand cmd = new SqlCommand(SQL);
             cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
             DataTable dt = Connection.GetDataTable(cmd);
             return dt;
         }
    }
}
