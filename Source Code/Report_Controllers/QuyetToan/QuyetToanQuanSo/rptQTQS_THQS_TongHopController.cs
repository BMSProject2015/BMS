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
    public class rptQTQS_THQS_TongHopController : Controller
    {

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TongHop.xls";
       

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TongHop.aspx";
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
            String iThang = Request.Form[ParentID + "_iThang"];
            ViewData["PageLoad"] = "1";
            ViewData["iThang"] = iThang;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TongHop.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan,String iThang)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            int iNamSau = Convert.ToInt16(iNamLamViec) + 1;
            String NgayThang = DateTime.DaysInMonth(Convert.ToInt16(iNamLamViec), Convert.ToInt16(iThang)).ToString();
            String sTenDonVi = iID_MaPhongBan;
            String sTenPhuLuc = "";
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaPhongBan,iThang);
            fr = ReportModels.LayThongTinChuKy(fr, "rptQTQS_THQS_TongHop");

            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("NgayDK","01/01/" +iNamSau);
            fr.SetValue("Thang", NgayThang +"/"+iThang);
            if (iID_MaPhongBan != "-1")
            {
                fr.SetValue("sTenDonVi", "B " + sTenDonVi);
                sTenPhuLuc = "PL02b";
            }
            else
            {
                fr.SetValue("sTenDonVi", "");
                sTenPhuLuc = "PL02a";
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("sTenPhuLuc", sTenPhuLuc);
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        //1020000
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptQTQS_THQS_TongHop(String MaND,String iID_MaPhongBan,String iThang)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000;

            String SQL = String.Format(@"SELECT * FROM (
SELECT iID_MaDonVi
,QSBC=SUM(CASE WHEN sKyHieu='000' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)
,QSKH=SUM(CASE WHEN sKyHieu='001' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)
,QSHC=SUM(CASE WHEN sKyHieu='002' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)
,KHTang=SUM(CASE WHEN sKyHieu='2' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)
,THTang=SUM(CASE WHEN (sKyHieu='2' AND iThang_Quy<=@iThang) THEN rThieuUy+rTrungUy+rThuongUy+rDaiUy+rThieuTa+rTrungTa+rThuongTa+rDaiTa+rTuong+rTSQ+rBinhNhi+rBinhNhat+rHaSi+rTrungSi+rThuongSi+rQNCN+rCNVQP+rLDHD ELSE 0 END)
,KHGiam=SUM(CASE WHEN sKyHieu='3' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)
,THGiam=SUM(CASE WHEN (sKyHieu='3' AND iThang_Quy<=@iThang) THEN rThieuUy+rTrungUy+rThuongUy+rDaiUy+rThieuTa+rTrungTa+rThuongTa+rDaiTa+rTuong+rTSQ+rBinhNhi+rBinhNhat+rHaSi+rTrungSi+rThuongSi+rQNCN+rCNVQP+rLDHD ELSE 0 END)
,DuKien=SUM(CASE WHEN sKyHieu='800' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)
FROM QTQS_ChungTuChiTiet 
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} AND iThang_Quy<>0
GROUP BY iID_MaDonVi
HAVING SUM(CASE WHEN sKyHieu='000' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)<>0
 OR SUM(CASE WHEN sKyHieu='001' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)<>0
 OR SUM(CASE WHEN sKyHieu='002' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)<>0
 OR SUM(CASE WHEN sKyHieu='2' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)<>0
 OR SUM(CASE WHEN (sKyHieu='2' AND iThang_Quy<=@iThang) THEN rThieuUy+rTrungUy+rThuongUy+rDaiUy+rThieuTa+rTrungTa+rThuongTa+rDaiTa+rTuong+rTSQ+rBinhNhi+rBinhNhat+rHaSi+rTrungSi+rThuongSi+rQNCN+rCNVQP+rLDHD ELSE 0 END)<>0
 OR SUM(CASE WHEN sKyHieu='3' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)<>0
 OR SUM(CASE WHEN (sKyHieu='3' AND iThang_Quy<=@iThang) THEN rThieuUy+rTrungUy+rThuongUy+rDaiUy+rThieuTa+rTrungTa+rThuongTa+rDaiTa+rTuong+rTSQ+rBinhNhi+rBinhNhat+rHaSi+rTrungSi+rThuongSi+rQNCN+rCNVQP+rLDHD ELSE 0 END)<>0
 OR SUM(CASE WHEN sKyHieu='800' THEN rSQ_KH+rHSQBS_KH+rCNVQP_KH+rQNCN_KH+rLDHD_KH ELSE 0 END)<>0
) as a
INNER JOIN (SELECT iID_MaDonVi, sTen as sTenDonVi FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as b
ON a.iID_MaDonVi=b.iID_MaDonVi
ORDER BY a.iID_MaDonVi
", DKDonVi,DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan,String iThang)
        {
            DataRow r;
            DataTable data = DT_rptQTQS_THQS_TongHop(MaND, iID_MaPhongBan, iThang);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
      
        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan,String iThang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan,iThang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_1010000_TongHop.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan,String iThang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan, iThang);
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

