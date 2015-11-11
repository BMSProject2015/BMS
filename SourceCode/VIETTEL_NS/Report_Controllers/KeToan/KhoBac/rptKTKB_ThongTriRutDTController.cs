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
    public class rptKTKB_ThongTriRutDTController : Controller
    {
          
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_ThongTriRutDT.xls";
      

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_ThongTriRutDT.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
          public ActionResult EditSubmit(String ParentID,String iNamLamViec,  String iID_MaChungTu)
        {
            //StringiNamLamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec"]);
          // String iID_MaChungTu= Convert.ToString(Request.Form[ParentID + "_iID_MaChungTu"]);
            return RedirectToAction("Index", new {iNamLamViec =iNamLamViec, iID_MaChungTu = iID_MaChungTu});
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iID_MaChungTu)
          {
              String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
              String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            DataTable dt=KTLB_ThongTriRutDT(iNamLamViec,iID_MaChungTu);
            String NgayCT="";
            String ThangCT="";
            String NgayThang="";
             if(dt.Rows.Count >0)
             {
                 NgayCT = dt.Rows[0]["iNgayCT"].ToString();
                 ThangCT = dt.Rows[0]["iThangCT"].ToString();
                 NgayThang = "ngày " + NgayCT + " tháng " + ThangCT;
             }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_ThongTriRutDT");
            LoadData(fr,iNamLamViec, iID_MaChungTu);      
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Nam",iNamLamViec);
            fr.SetValue("NgayThang", NgayThang);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh); 
            fr.Run(Result);
            return Result;
        }
          public clsExcelResult ExportToExcel(String iNamLamViec, String iID_MaChungTu)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;                                
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile),iNamLamViec, iID_MaChungTu);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToanKB_ThongTriRutDt.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public clsExcelResult ExportToPDF(String iNamLamViec, String iID_MaChungTu)
        {
            HamChung.Language();
             String DuongDanFile = sFilePath;
           
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile),iNamLamViec, iID_MaChungTu);
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
        public ActionResult ViewPDF(string MaND)
        {
            HamChung.Language();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            dtCauHinh.Dispose();
            String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            String DuongDanFile = sFilePath;
           
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile),iNam, iID_MaChungTu);
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
        public String obj_data(String ParentID, String iNamLamViec,String iID_MaChungTu)
        {
            DataTable dt = DanhSach_ChungTu(iNamLamViec);       
            SelectOptionList slChungTu = new SelectOptionList(dt, "iID_MaChungTu", "iSoChungTu");
            String s = MyHtmlHelper.DropDownList(ParentID, slChungTu, iID_MaChungTu, "iID_MaChungTu", "", "class=\"input1_2\" style=\"width: 100%\"");
            return s;
        }
        [HttpGet]
        public JsonResult ds_NhomDonVi(String ParentID, String iNamLamViec, String iID_MaChungTu)
        {
            return Json(obj_data(ParentID,iNamLamViec, iID_MaChungTu), JsonRequestBehavior.AllowGet);
        }
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iID_MaChungTu)
        {            
                DataTable data = KTLB_ThongTriRutDT(iNamLamViec, iID_MaChungTu);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);           
        }
        public static DataTable KTLB_ThongTriRutDT(String iNamLamViec,String iID_MaChungTu)
        {
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String SQL=String.Format(@"SELECT iID_MaDonVi_Nhan,sTenDonVi_Nhan,iNgayCT,iThangCT,SUM(ThuongXuyen) as ThuongXuyen, SUM(NghiepVu) as NghiepVu
                                        FROM(
                                        SELECT iID_MaDonVi_Nhan,sTenDonVi_Nhan,iNgayCT,iThangCT
                                        ,ThuongXuyen=CASE WHEN sLNS=1010000 THEN SUM(rDTRut) ELSE 0 END
                                        ,NghiepVu=CASE WHEN (sLNS<>'' AND sLNS<>1010000) THEN SUM(rDTRut) ELSE 0 END
                                        FROM KTKB_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND iNamLamViec=@NamLamViec
                                        AND iTrangThai=1 AND iLoai=1 and iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
                                        GROUP BY iID_MaDonVi_Nhan,sTenDonVi_Nhan,iNgayCT,iThangCT,sLNS) as a
                                        GROUP BY sTenDonVi_Nhan,iID_MaDonVi_Nhan,iNgayCT,iThangCT
                                        HAVING SUM(ThuongXuyen)<>0 OR SUM(NghiepVu)<>0");
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec",iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaChungTu",iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(P))
            DataTable dt=Connection.GetDataTable(cmd);
            cmd.Dispose();
            int a = dt.Rows.Count;
            if (a < 15 && a>0)
            {
                for (int i = 0; i < (15 - a); i++)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, a + 1);
                }
            }
            dt.Dispose();
            return dt;
        }
        public static DataTable DanhSach_ChungTu(String iNamLamViec)
        {
            String SQL=String.Format(@"SELECT  DISTINCT iID_MaChungTu,iSoChungTu
                                       FROM KTKB_ChungTu
                                       WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec
                                        ORDER By iSoChungTu");
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec",iNamLamViec);           
            DataTable dt=Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
