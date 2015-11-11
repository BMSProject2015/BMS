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
    public class rptDuToan_TongHop_THB2Controller : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_TongHop_THB2.xls";
       

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHop_THB2.aspx";
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
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHop_THB2.aspx";
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
            fr = ReportModels.LayThongTinChuKy(fr,"rptDuToan_TongHop_THB2");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Cap2", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        
        public static DataTable DT_rptDuToan_TongHop_THB2(String MaND,String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000;
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen
,SUM(BD) AS BD
,SUM(TX) AS TX
,SUM(NV) AS NV
,SUM(XDCB) AS XDCB
,SUM(DN) AS DN
,SUM(QPKhac) AS QPKhac
,SUM(NN) AS NN
,SUM(NSKhac) AS NSKhac
,SUM(TonKho) AS TonKho
,SUM(HienVat) AS HienVat
FROM
(
SELECT 
iID_MaDonVi
,BD=SUM(CASE WHEN sLNS LIKE '104%' AND MaLoai<>1 THEN (rTuChi+rHangMua+rHangNhap)/{0} ELSE 0 END)
,TX=SUM(CASE WHEN sLNS=1010000 THEN rTuChi /{0} ELSE 0 END)
,NV=SUM(CASE WHEN sLNS like '102%' THEN rTuChi/{0}  ELSE 0 END)
,XDCB=SUM(CASE WHEN sLNS like '103%' THEN rTuChi/{0} ELSE 0 END)
,DN=SUM(CASE WHEN sLNS like '105%' THEN rTuChi/{0} ELSE 0 END)
,QPKhac=SUM(CASE WHEN sLNS like '109%' THEN rTuChi/{0} ELSE 0 END)
,NN=SUM(CASE WHEN sLNS like '2%' THEN rTuChi/{0} ELSE 0 END)
,NSKhac=SUM(CASE WHEN sLNS like '4%' THEN rTuChi/{0} ELSE 0 END)
,TonKho=SUM(CASE  WHEN itrangthai=1 THEN rTonKho/{0} ELSE 0 END)
,HienVat=SUM(CASE WHEN  itrangthai=1 THEN rHienVat/{0} ELSE 0 END)
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec {1} 
GROUP BY iID_MaDonVi

UNION

SELECT 
iID_MaDonVi
,BD=SUM(CASE WHEN sLNS LIKE '104%' AND MaLoai<>1 THEN (rTuChi+rHangMua+rHangNhap)/{0} ELSE 0 END)
,TX=SUM(CASE WHEN sLNS=1010000 THEN rTuChi /{0} ELSE 0 END)
,NV=SUM(CASE WHEN sLNS like '102%' THEN rTuChi/{0}  ELSE 0 END)
,XDCB=SUM(CASE WHEN sLNS like '103%' THEN rTuChi/{0} ELSE 0 END)
,DN=SUM(CASE WHEN sLNS like '105%' THEN rTuChi/{0} ELSE 0 END)
,QPKhac=SUM(CASE WHEN sLNS like '109%' THEN rTuChi/{0} ELSE 0 END)
,NN=SUM(CASE WHEN sLNS like '2%' THEN rTuChi/{0} ELSE 0 END)
,NSKhac=SUM(CASE WHEN sLNS like '4%' THEN rTuChi/{0} ELSE 0 END)
,TonKho=SUM(CASE  WHEN itrangthai=1 THEN rTonKho/{0} ELSE 0 END)
,HienVat=SUM(CASE WHEN  itrangthai=1 THEN rHienVat/{0} ELSE 0 END)
FROM DT_ChungTuChiTiet_PhanCap
WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  {1} 
GROUP BY iID_MaDonVi
)  as a
INNER JOIN (SELECT iid_madonvi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as b
ON a.iID_MaDonVi=b.iID_MaDonVi
GROUP BY a.iID_MaDonVi,sTen
HAVING
SUM(BD)<>0 OR 
SUM(TX)<>0 OR 
SUM(NV)<>0 OR 
SUM(NV)<>0 OR 
SUM(XDCB)<>0 OR 
SUM(DN)<>0 OR 
SUM(QPKhac)<>0 OR 
SUM(NN)<>0 OR 
SUM(NSKhac)<>0 OR 
SUM(TonKho)<>0 OR 
SUM(HienVat)<>0 
ORDER BY a.iID_MaDonVi", DVT,DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable DT_rptDuToan_TongHop_THB2_ChoPhanCap(String MaND, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000;
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen
,SUM(BD) AS BD
,SUM(TX) AS TX
,SUM(NV) AS NV
,SUM(XDCB) AS XDCB
,SUM(DN) AS DN
,SUM(QPKhac) AS QPKhac
,SUM(NN) AS NN
,SUM(NSKhac) AS NSKhac
,SUM(TonKho) AS TonKho
,SUM(HienVat) AS HienVat
FROM
(
SELECT 
iID_MaDonVi
,BD=SUM(CASE WHEN sLNS LIKE '104%' AND MaLoai<>1 THEN (rDuPhong)/{0} ELSE 0 END)
,TX=SUM(CASE WHEN sLNS=1010000 THEN rDuPhong /{0} ELSE 0 END)
,NV=SUM(CASE WHEN sLNS like '102%' THEN rDuPhong/{0}  ELSE 0 END)
,XDCB=SUM(CASE WHEN sLNS like '103%' THEN rDuPhong/{0} ELSE 0 END)
,DN=SUM(CASE WHEN sLNS like '105%' THEN rDuPhong/{0} ELSE 0 END)
,QPKhac=SUM(CASE WHEN sLNS like '109%' THEN rDuPhong/{0} ELSE 0 END)
,NN=SUM(CASE WHEN sLNS like '2%' THEN rDuPhong/{0} ELSE 0 END)
,NSKhac=SUM(CASE WHEN sLNS like '4%' THEN rDuPhong/{0} ELSE 0 END)
,TonKho=0
,HienVat=0
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec {1} 
GROUP BY iID_MaDonVi

)  as a
INNER JOIN (SELECT iid_madonvi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as b
ON a.iID_MaDonVi=b.iID_MaDonVi
GROUP BY a.iID_MaDonVi,sTen
HAVING
SUM(BD)<>0 OR 
SUM(TX)<>0 OR 
SUM(NV)<>0 OR 
SUM(NV)<>0 OR 
SUM(XDCB)<>0 OR 
SUM(DN)<>0 OR 
SUM(QPKhac)<>0 OR 
SUM(NN)<>0 OR 
SUM(NSKhac)<>0 OR 
SUM(TonKho)<>0 OR 
SUM(HienVat)<>0 
ORDER BY a.iID_MaDonVi", DVT,DKPhongBan);
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
            DataTable data = DT_rptDuToan_TongHop_THB2(MaND, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            DataTable dtChoPhanCap = DT_rptDuToan_TongHop_THB2_ChoPhanCap(MaND, iID_MaPhongBan);
            fr.AddTable("ChoPhanCap", dtChoPhanCap);
            dtChoPhanCap.Dispose();
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

