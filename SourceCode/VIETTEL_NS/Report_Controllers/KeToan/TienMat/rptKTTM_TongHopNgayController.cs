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
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.KeToan.TienMat
{
    public class rptKTTM_TongHopNgayController : Controller
    {
        //
        // GET: /rptChiTietTheoDonVi/
        
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienMat/rptKTTienMatBangThuChi.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/KeToan/TienMat/rptKTTienMatTongHopThuChi.xls";     
        public static String NameFile = "";
        public long Tien = 0;
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptKTTM_TongHopNgay_View.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";         
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao"]);           
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            return RedirectToAction("Index", new { iNgay1 = iNgay1, iNgay2 = iNgay2, iThang1 = iThang1, iThang2 = iThang2, LoaiBaoCao = LoaiBaoCao, iNamLamViec = iNamLamViec });
        }
        public ExcelFile CreateReport(String path, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iNamLamViec)
        {
              XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iNamLamViec);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTM_TongHopNgay");
            fr.SetValue("TenDVCapTren", BoQuocPhong);
            fr.SetValue("tendv", CucTaiChinh);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tien));
            if (iNgay1 == iNgay2 && iThang1 == iThang2)
                fr.SetValue("ThoiGian", "Ngày " + iNgay1 + " tháng " + iThang1 + " năm " + iNamLamViec);
            else
                fr.SetValue("ThoiGian", "Từ ngày " + iNgay1 + "/" + iThang1 + "/" + iNamLamViec + " đến ngày " + iNgay2 + "/" + iThang2 + "/" + iNamLamViec);
            fr.SetValue("Ngay", "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString());
            if (LoaiBaoCao == "PC2")
            {
                fr.SetValue("TieuDe", "BẢNG TỔNG HỢP PHIẾU THU");
            }
            else if (LoaiBaoCao == "PC3")
            {
                fr.SetValue("TieuDe", "BẢNG TỔNG HỢP PHIẾU CHI");
            }
           
            //
            //fr.SetValue("MaDV", iID_MaDonVi);
            //fr.SetValue("Ma", iID_MaTaiKhoan);
            //fr.SetValue("Nam", iNamLamViec);
            //fr.SetValue("Thang", iThang);
            //fr.SetValue("Ngay1", iNgay1);
            //fr.SetValue("Ngay2", iNgay2);
            //fr.SetValue("KTT", "KẾ TOÁN TRƯỞNG ");
            //fr.SetValue("TP", "P.TRƯỞNG PHÒNG");
            //fr.SetValue("TTDV", "THỦ TRƯỞNG ĐƠN VỊ");
            //fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            //fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            //fr.SetValue("Nams", DateTime.Now.Year);
            fr.Run(Result);
            return Result;
        }


        public clsExcelResult ExportToPDF(String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iNamLamViec)
        {
            String sFilePathName = "";
            if (LoaiBaoCao == "PC1") sFilePathName = sFilePath;
            else sFilePathName = sFilePath1;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePathName), iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iNamLamViec);
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
        public clsExcelResult ExportToExcel(String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iNamLamViec)
        {
            String sFilePathName = "";
            if (LoaiBaoCao == "PC1") sFilePathName = sFilePath;
            else sFilePathName = sFilePath1;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePathName), iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iNamLamViec);
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
        public ActionResult ViewPDF(String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iNamLamViec)
        {
            HamChung.Language();
            String sFilePathName = "";
            if (LoaiBaoCao == "PC1") sFilePathName = sFilePath;
            else sFilePathName = sFilePath1;
            ExcelFile xls = CreateReport(Server.MapPath(sFilePathName), iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iNamLamViec);
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
        private void LoadData(FlexCelReport fr, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iNamLamViec)
        {
            DataTable data = GetDanhSachChungTu(iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iNamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
           
        }

       
        public DataTable GetDanhSachChungTu(String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iNamLamViec)
        {
            String iTuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String iDenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            if (LoaiBaoCao == "PC1")
            {
                SQL += @"SELECT      CONVERT(nvarchar, iNgay) + '/' + CONVERT(nvarchar, iThang) AS NgayCT, sSoChungTuChiTiet, sNoiDung, 
sTenNguoiThuChi, iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co, iID_MaDonVi_No, iID_MaDonVi_Co, rSoTien, sDiaChi, sGhiChuLyDo FROM KTTM_ChungTuChiTiet WHERE iTrangThai=1";
            }
            else if (LoaiBaoCao == "PC2")
            {
                SQL += @"SELECT      CT.rSoTien, CT.sTenNguoiThuChi, DV.sTenTomTat AS TenDonVi, CT.sSoChungTuChiTiet, CT.sNoiDung,
                        CT.iID_MaTaiKhoan_Co AS TKDoiUng
FROM          dbo.KTTM_ChungTuChiTiet AS CT INNER JOIN
                        (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON CT.iID_MaDonVi_No = DV.iID_MaDonVi WHERE CT.bThu=1 AND CT.iTrangThai=1";
            }
            else
            {
                SQL += @"SELECT      CT.rSoTien, CT.sTenNguoiThuChi, DV.sTenTomTat AS TenDonVi, CT.sSoChungTuChiTiet, CT.sNoiDung, CT.iID_MaTaiKhoan_No AS TKDoiUng FROM KTTM_ChungTuChiTiet AS CT INNER JOIN 
(SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON CT.iID_MaDonVi_Co = DV.iID_MaDonVi WHERE CT.bChi=1 AND CT.iTrangThai=1"; 
            }
            if (String.IsNullOrEmpty(iThang1) == false && iThang1 != "" && String.IsNullOrEmpty(iThang2) == false && iThang2 != ""
                      && String.IsNullOrEmpty(iNgay1) == false && iNgay1 != "" && String.IsNullOrEmpty(iNgay2) == false && iNgay2 != "")
            {

                SQL += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) >= @TuNgay)";
                SQL += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)";
                cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay));
                cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay));

            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    Tien += long.Parse(HamChung.ConvertToString(dr["rSoTien"]));
                }
            }
            return dt;

        }

    }
}
