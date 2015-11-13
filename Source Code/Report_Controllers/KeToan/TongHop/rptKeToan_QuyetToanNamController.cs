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
    public class rptKeToan_QuyetToanNamController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_QuyetToanNam.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_QuyetToanNam.aspx";
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
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_TrangThai"]);

            String NamLamViec_LamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec_LamViec"]);
            String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);


            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["TrangThai"] = TrangThai;

            ViewData["NamLamViec_LamViec"] = NamLamViec_LamViec;
            ViewData["iNgay"] = iNgay;

            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_QuyetToanNam.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String TrangThai, String iNgay, String NamLamViec_LamViec)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_QuyetToanNam");
            LoadData(fr, NamLamViec, ThangLamViec, iID_MaTaiKhoan, TrangThai, iNgay, NamLamViec_LamViec);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam = "năm " + NamLamViec;

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
            if (String.IsNullOrEmpty(iID_MaTaiKhoan))
            {
                fr.SetValue("TK", "");
            }
            else
            {
                string sTenTK = getTenTK(Convert.ToInt32(NamLamViec), iID_MaTaiKhoan).Replace("- NN", "").Replace("-NN", "");
                fr.SetValue("TK", sTenTK);
            }

            fr.SetValue("Ngay", DateTime.Now.Day);
            fr.SetValue("Thang", DateTime.Now.Month);
            fr.SetValue("Nam", DateTime.Now.Year);
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String TrangThai, String iNgay, String NamLamViec_LamViec)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaTaiKhoan,
                                         TrangThai, iNgay, NamLamViec_LamViec);
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
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String TrangThai, String iNgay, String NamLamViec_LamViec)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaTaiKhoan,
                                         TrangThai, iNgay, NamLamViec_LamViec);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String TrangThai, String iNgay, String NamLamViec_LamViec)
        {

            DataTable data = KeToan_QuyetToanNam(NamLamViec, ThangLamViec, iID_MaTaiKhoan, TrangThai, iNgay, NamLamViec_LamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtPhongBan = HamChung.SelectDistinct("PhongBan", data, "iID_MaPhongBan_No", "iID_MaPhongBan_No");
            fr.AddTable("PhongBan", dtPhongBan);
            dtPhongBan.Dispose();
        }
        public  static  string getTenTK(int iNam, String iID_MaTaiKhoan)
        {
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT TOP 1 sTen FROM KT_TaiKhoan WHERE iTrangThai=1  AND iNam=@iNam AND iID_MaTaiKhoan=@iID_MaTaiKhoan");
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            String vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public DataTable KeToan_QuyetToanNam(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String TrangThai, String iNgay, String NamLamViec_LamViec)
        {
            String SQL = "";
            DataTable dt = new DataTable();

            String DK = "";
            if (TrangThai != "0")
            {
                DK = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
               // DK = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            String DenNgay = NamLamViec_LamViec + "/" + ThangLamViec + "/" + iNgay;
            if (iID_MaTaiKhoan.Length < 5) iID_MaTaiKhoan = "00000";
            String iID_MaTaiKhoan_NamTruoc = iID_MaTaiKhoan.Substring(0, 3) + "0" + iID_MaTaiKhoan.Substring(4, iID_MaTaiKhoan.Length - 4);
            String iID_MaTaiKhoan_NamSau = iID_MaTaiKhoan.Substring(0, 3) + "1" + iID_MaTaiKhoan.Substring(4, iID_MaTaiKhoan.Length - 4);
            if (NamLamViec != NamLamViec_LamViec)
            {


                SQL =
                    String.Format(
                        @"SELECT iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
,NamTruoc=CASE WHEN (SUM(NamTruoc_No-NamTruoc_Co)>=0) THEN SUM(NamTruoc_No-NamTruoc_Co) ELSE (SUM(NamTruoc_No-NamTruoc_Co))*-1 END
,NamNay=CASE WHEN (SUM(NamNay_No-NamNay_Co)>=0) THEN SUM(NamNay_No-NamNay_Co) ELSE (SUM(NamNay_No-NamNay_Co))*-1 END
,NamSau=CASE WHEN (SUM(NamSau_No-NamSau_Co)>=0) THEN SUM(NamSau_No-NamSau_Co) ELSE (SUM(NamSau_No-NamSau_Co))*-1 END
--,SUM(NamTruoc_No-NamTruoc_Co) NamTruoc
--,SUM(NamNay_No-NamNay_Co) NamNay
--,SUM(NamSau_No-NamSau_Co) NamSau
FROM (
--no nam nay
SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=0,
NamNay_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END),
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec and iThangCT<>0  {0}   
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END) <>0 
	
	Union
	
	--no truoc
SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec   {0}   
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 
	
	----

union all


SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_NamSau     
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
and iThangCT<>0   
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay 
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 
	Union
	
	--no sau
SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END),
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_NamSau  and iThangCT<>0 {0}  
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay 
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END) <>0 


	UNION
SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END),
NamSau_Co=0

 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec and iThangCT<>0  {0}   
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END) <>0 

-- nam truoc co
	UNION
SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_Co=0,
NamSau_Co=0

 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec  {0}   
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 


union all


SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_NamSau     
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
and iThangCT<>0   
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay 
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 
--
	UNION
SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END)

 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_NamSau and iThangCT<>0 {0}  
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay  
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END) <>0 
	) as CT
	GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No HAVING SUM(NamTruoc_No-NamTruoc_Co) <>0 OR SUM(NamNay_No-NamNay_Co) <>0 
 OR SUM(NamSau_No-NamSau_Co) <>0  Order by iID_MaPhongBan_No,iID_MaDonVi_No",
                        DK);
            }
            else
            {
                SQL = String.Format(@"SELECT iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
,NamTruoc=CASE WHEN (SUM(NamTruoc_No-NamTruoc_Co)>=0) THEN SUM(NamTruoc_No-NamTruoc_Co) ELSE (SUM(NamTruoc_No-NamTruoc_Co))*-1 END
,NamNay=CASE WHEN (SUM(NamNay_No-NamNay_Co)>=0) THEN SUM(NamNay_No-NamNay_Co) ELSE (SUM(NamNay_No-NamNay_Co))*-1 END
,NamSau=CASE WHEN (SUM(NamSau_No-NamSau_Co)>=0) THEN SUM(NamSau_No-NamSau_Co) ELSE (SUM(NamSau_No-NamSau_Co))*-1 END
--,SUM(NamTruoc_No-NamTruoc_Co) NamTruoc
--,SUM(NamNay_No-NamNay_Co) NamNay
--,SUM(NamSau_No-NamSau_Co) NamSau
FROM (
--no nam nay
SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=0,
NamNay_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END),
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec and iThangCT<>0  {0}   
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END) <>0 
	
	Union
	
	--no truoc
SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThangCT=0   {0}   
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 
	
	----

union all


SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec     
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
and iThangCT<>0   
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay 
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 
	Union
	
	--no sau
SELECT iID_MaPhongBan_No, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END),
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_NamSau  and iThangCT<>0 {0}  
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay 
AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No is not null
AND iID_MaPhongBan_No<>'' AND iID_MaPhongBan_No is not null
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
HAVING SUM(CASE WHEN iID_MaTaiKhoan_No=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END) <>0 


	UNION
SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END),
NamSau_Co=0

 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec and iThangCT<>0  {0}   
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan THEN rSoTien ELSE 0 END) <>0 

-- nam truoc co
	UNION
SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_Co=0,
NamSau_Co=0

 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThangCT=0 {0}   
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 


union all


SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END),
NamNay_Co=0,
NamSau_Co=0
 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec     
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
and iThangCT<>0   
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay 
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamTruoc THEN rSoTien ELSE 0 END) <>0 
--
	UNION
SELECT iID_MaPhongBan_Co, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,
NamTruoc_No=0,
NamNay_No=0,
NamSau_No=0,
NamTruoc_Co=0,
NamNay_Co=0,
NamSau_Co=SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END)

 FROM KT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_NamSau and iThangCT<>0 {0}  
AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay  
AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co is not null
AND iID_MaPhongBan_Co<>'' AND iID_MaPhongBan_Co is not null
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
HAVING SUM(CASE WHEN iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_NamSau THEN rSoTien ELSE 0 END) <>0 
	) as CT
	GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No,sTenDonVi_No HAVING SUM(NamTruoc_No-NamTruoc_Co) <>0 OR SUM(NamNay_No-NamNay_Co) <>0 
 OR SUM(NamSau_No-NamSau_Co) <>0  Order by iID_MaPhongBan_No,iID_MaDonVi_No", DK);
            }

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iNamLamViec_NamSau", Convert.ToInt32(NamLamViec) + 1); ;
            if (TrangThai != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_NamTruoc", iID_MaTaiKhoan_NamTruoc);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_NamSau", iID_MaTaiKhoan_NamSau);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        public JsonResult Get_objNgayThang(String ParentID, String MaND, String iThang, String iNgay, String iNamLamViec)
        {
            return Json(get_sNgayThang(ParentID, MaND, iThang, iNgay, iNamLamViec), JsonRequestBehavior.AllowGet);
        }
        public String get_sNgayThang(String ParentID, String MaND, String iThang, String iNgay, String iNamLamViec)
        {
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            String S = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:100%;\"");
            if (dtNgay!=null)
            {
                ViewData["iNgay"] = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["MaNgay"]);
                dtNgay.Dispose(); 
            }

            return S;
        }

    }
}
