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
    public class rptDuToan_1030100_TongHop_3bCController : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_1030100_TongHop_3bC.xls";
       

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1030100_TongHop_3bC.aspx";
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
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1030100_TongHop_3bC.aspx";
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


            String sTenDonVi = "B -  " + iID_MaPhongBan;
            if (iID_MaPhongBan == "-1" || iID_MaPhongBan == "")
                sTenDonVi = ReportModels.CauHinhTenDonViSuDung(2);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaPhongBan);
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_1030100_TongHop_3bC");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Cap2", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        //1020200
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptDuToan_1030100_TongHop_3bC(String MaND,String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
          //  DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000;
            String SQL = "";
            SQL = String.Format(@"SELECT DT_ChungTuChiTiet.iID_MaDonVi,REPLACE (sTenDonVi,DT_ChungTuChiTiet.iID_MaDonVi+' - ','') as  sTenDonVi
,CTPT=SUM(CASE WHEN iLoai=2 THEN rTuChi/{2} ELSE 0 END)
,Cot1=SUM(CASE WHEN iLoai=3 AND iNhom=1 THEN rTuChi/{2} ELSE 0 END)
,Cot2=SUM(CASE WHEN iLoai=3 AND iNhom=8 THEN rTuChi/{2} ELSE 0 END)
,Cot3=SUM(CASE WHEN iLoai=3 AND iNhom=7 THEN rTuChi/{2} ELSE 0 END)
,Cot4=SUM(CASE WHEN iLoai=3 AND iNhom=9 THEN rTuChi/{2} ELSE 0 END)
,Cot5=SUM(CASE WHEN iLoai=3 AND iNhom=2 THEN rTuChi/{2} ELSE 0 END)
,Cot6=SUM(CASE WHEN iLoai=3 AND iNhom=6 THEN rTuChi/{2} ELSE 0 END)
,Cot7=SUM(CASE WHEN iLoai=3 AND iNhom=4 THEN rTuChi/{2} ELSE 0 END)
,Cot8=SUM(CASE WHEN iLoai=3 AND iNhom IN (3,5) THEN rTuChi/{2} ELSE 0 END)
 FROM DT_ChungTuChiTiet,NS_MucLucDuAn
 WHERE DT_ChungTuChiTiet.iTrangThai=1  AND sLNS LIKE '1030100%' AND DT_ChungTuChiTiet.iNamLamViec=@iNamLamViec {0} {1}
 AND DT_ChungTuChiTiet.sMaCongTrinh=NS_MucLucDuAn.sMaCongTrinh
 GROUP BY DT_ChungTuChiTiet.iID_MaDonVi,sTenDonVi 
 HAVING SUM(rTuChi)<>0
",  DKPhongBan, DKDonVi, DVT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }


        public static DataTable DT_rptDuToan_1030100_TongHop_3bC_DuPhong(String MaND, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
         //   DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000;
            String SQL = "";
            SQL = String.Format(@"SELECT DT_ChungTuChiTiet.iID_MaDonVi,REPLACE (sTenDonVi,DT_ChungTuChiTiet.iID_MaDonVi+' - ','') as  sTenDonVi
,CTPT=SUM(CASE WHEN iLoai=2 THEN rDuPhong/{2} ELSE 0 END)
,Cot1=SUM(CASE WHEN iLoai=3 AND iNhom=1 THEN rDuPhong/{2} ELSE 0 END)
,Cot2=SUM(CASE WHEN iLoai=3 AND iNhom=8 THEN rDuPhong/{2} ELSE 0 END)
,Cot3=SUM(CASE WHEN iLoai=3 AND iNhom=7 THEN rDuPhong/{2} ELSE 0 END)
,Cot4=SUM(CASE WHEN iLoai=3 AND iNhom=9 THEN rDuPhong/{2} ELSE 0 END)
,Cot5=SUM(CASE WHEN iLoai=3 AND iNhom=2 THEN rDuPhong/{2} ELSE 0 END)
,Cot6=SUM(CASE WHEN iLoai=3 AND iNhom=6 THEN rDuPhong/{2} ELSE 0 END)
,Cot7=SUM(CASE WHEN iLoai=3 AND iNhom=4 THEN rDuPhong/{2} ELSE 0 END)
,Cot8=SUM(CASE WHEN iLoai=3 AND iNhom IN (3,5) THEN rDuPhong/{2} ELSE 0 END)
 FROM DT_ChungTuChiTiet,NS_MucLucDuAn
 WHERE DT_ChungTuChiTiet.iTrangThai=1  AND sLNS LIKE '1030100%' AND DT_ChungTuChiTiet.iNamLamViec=@iNamLamViec {0} {1}
 AND DT_ChungTuChiTiet.sMaCongTrinh=NS_MucLucDuAn.sMaCongTrinh
 GROUP BY DT_ChungTuChiTiet.iID_MaDonVi,sTenDonVi
 HAVING SUM(rDuPhong)<>0
", DKPhongBan, DKDonVi, DVT);
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
            DataTable data = DT_rptDuToan_1030100_TongHop_3bC(MaND, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dataDuPhong = DT_rptDuToan_1030100_TongHop_3bC_DuPhong(MaND, iID_MaPhongBan);
            fr.AddTable("DuPhong", dataDuPhong);
          
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
                clsResult.FileName = "DuToan_1020200_TongHop.xls";
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

