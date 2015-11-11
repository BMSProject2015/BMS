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

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_TongHop_2060101Controller : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_TongHop_2060101.xls";
       

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHop_2060101.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHop_2060101.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);


            String sTenDonVi = "B -  "+iID_MaPhongBan;
            if (iID_MaPhongBan == "-1" || iID_MaPhongBan == "")
                sTenDonVi = ReportModels.CauHinhTenDonViSuDung(2);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaPhongBan);
            fr = ReportModels.LayThongTinChuKy(fr,"rptDuToan_TongHop_2060101");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Cap2", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        
        public static DataTable DT_rptDuToan_TongHop_2060101(String MaND,String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000;
            String SQL = String.Format(@"SELECT iID_MaDonVi,REPLACE(sTenDonVi,iID_MaDonVi+' - ','') as sTenDonVi
,TroCap_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(00,01) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,TroCap_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(00,01) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,PhuCap_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(02) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,PhuCap_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(02) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,TienKhoiNghia_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(03) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,TienKhoiNghia_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(03) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,AnhHung_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(04) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,AnhHung_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(04) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,ThuongBinhA_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(05) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,ThuongBinhA_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(05) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,ThuongBinhB_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(06) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,ThuongBinhB_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7151 AND sTTM IN(06) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,BaoTu_Tien=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7199 AND sTTM IN(70) AND sNG=38) THEN rTuChi/{3} ELSE 0 END)
,BaoTu_Nguoi=SUM(CASE WHEN (sLNS=2060101 AND sM=7150 AND sTM=7199 AND sTTM IN(70) AND sNG=38) THEN rSoNguoi ELSE 0 END)
,LePhi=SUM(CASE WHEN sLNS=2060102 THEN rTuChi/{3} ELSE 0 END)
,DieuDuong=SUM(CASE WHEN (sM=7150 AND sTM=7166) THEN rTuChi/{3} ELSE 0 END)
 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1   {0} AND iNamLamViec=@iNamLamViec {1} {2} AND rTuChi<>0
 GROUP BY iID_MaDonVi,sTenDonVi", "", DKPhongBan, DKDonVi, DVT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = DT_rptDuToan_TongHop_2060101(MaND, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
      
        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_TongHop_NganSachQuocPhong.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan);
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
        }
    }
}

