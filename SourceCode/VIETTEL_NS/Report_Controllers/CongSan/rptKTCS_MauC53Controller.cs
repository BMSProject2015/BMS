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
using System.Collections.Specialized;

namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptKTCS_MauC53Controller : Controller
    {
        //
        // GET: /rptKTCS_MauC53/
 public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay = "")
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_Mau53_A3.xls";
            }
            else 
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_Mau53.xls";
            }

            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_MauC53.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Thực hiện truyền các tham số trên form 
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String KhoGiay = Request.Form[ParentID + "_KhoGiay"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHopDonVi"] = TongHopDonVi;
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_MauC53.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Hiển thị báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String iID_MaDonVi,String TongHopDonVi, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên Loại tài sản
            String TenLoaiTaiSan = "";
            String tendv = "";
            
          
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //hàm đổi tiền từ số sang chữ
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_MauC53");
            //Load dữ liệu ra báo cáo
            LoadData(fr, NamChungTu, iID_MaDonVi, TongHopDonVi,KhoGiay);
            fr.SetValue("Nam", NamChungTu);
            fr.SetValue("TenDV", tendv);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("TenLoaiTS", TenLoaiTaiSan);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// DataTable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable rptKTCS_MauC53(String NamChungTu, String iID_MaDonVi, String TongHopDonVi, String KhoGiay)
        {

            DataTable dt = null;
            // int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCongSan);


            String DKDonVi = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = KTCS_ReportModel.ListDonVi();
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += "iID_MaDonVi = '" + dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + "'";
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";

                }
                dtDonVi.Dispose();
            }
            else
            {
                DKDonVi = " iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'";
            }

            String SQL = String.Format(@"
SELECT SUBSTRING(b.iID_MaNhomTaiSan,1,1) as Cap1,SUBSTRING(b.iID_MaNhomTaiSan,1,2) as Cap2,a.iID_MaTaiSan,b.sTenTaiSan,b.iID_MaNhomTaiSan,e.iID_MaDonVi,e.iID_MaDonVi+ '-'+ e.sTen as sTen,e.sTen as sTenDV,NguyenGiaTheoSoSach,GiaTriConTheoSoSach FROM (
SELECT iID_MaTaiSan,SUM(rNguyenGia) as NguyenGiaTheoSoSach, SUM(rGiaTriConLai) as GiaTriConTheoSoSach 
FROM KTCS_KhauHaoHangNam
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec 
GROUP BY iID_MaTaiSan) as a
INNER JOIN (SELECT iID_MaTaiSan,iID_MaNhomTaiSan,sTenTaiSan
			FROM KTCS_TaiSan
			WHERE iTrangThai=1 ) as b
ON a.iID_MaTaiSan=b.iID_MaTaiSan
INNER JOIN(
SELECT iID_MaTaiSan,iID_MaDonVi
FROM KTCS_TaiSan_DonVi
WHERE iTrangThai=1  AND ({0})) as c
ON a.iID_MaTaiSan=c.iID_MaTaiSan
INNER JOIN(
SELECT iID_MaNhomTaiSan
FROM KTCS_NhomTaiSan
WHERE iTrangThai=1 AND bLahangCha=0) as d
ON b.iID_MaNhomTaiSan=d.iID_MaNhomTaiSan
INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as e
on c.iID_MaDonVi=e.iID_MaDonVi
", DKDonVi);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
      
        /// <summary>
        /// Hiển thị dữ liệu
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String iID_MaDonVi, String TongHopDonVi, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            DataTable data = rptKTCS_MauC53(NamChungTu, iID_MaDonVi, TongHopDonVi, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

        }
       
        /// <summary>
        /// Action thực hiện xuất dữ liệu ra file EXcel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String iID_MaDonVi, String TongHopDonVi, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_Mau53_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_Mau53.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaDonVi, TongHopDonVi,KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "BienBanKiemkeTaiSan_A3";
                }
                else
                {
                    clsResult.FileName = "BienBanKiemkeTaiSan_A4";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ActionResult ViewPDF(String NamChungTu, String iID_MaDonVi, String TongHopDonVi, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_Mau53_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_Mau53.xls";
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaDonVi, TongHopDonVi,KhoGiay);
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
        /// <summary>
        /// lấy danh sách loại tài sản
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
    
    }
}
