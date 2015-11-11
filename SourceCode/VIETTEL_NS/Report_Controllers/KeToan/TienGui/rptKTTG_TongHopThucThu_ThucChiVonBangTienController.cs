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

namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKTTG_TongHopThucThu_ThucChiVonBangTienController : Controller
    {
        //
        // GET: /rptKTTG_TongHopSoDuVonBangTien/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTG_TongHopThucThu_ThucChiVonBangTien.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_TongHopThucThu_ThucChiVonBangTien.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {

            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            DateTime TuNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang1 + "/" + iNgay1);
            DateTime DenNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang2 + "/" + iNgay2);
            TimeSpan time = DenNgay - TuNgay;
            if (time.Days < 0)
            {
                ViewData["PageLoad"] = "0";
            }
            else
            {
                ViewData["PageLoad"] = "1";
            }
            ViewData["iNgay1"] = iNgay1;
            ViewData["iThang1"] = iThang1;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iNgay2"] = iNgay2;
            ViewData["iThang2"] = iThang2;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_TongHopThucThu_ThucChiVonBangTien.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
       public ActionResult ViewPDF(String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTG_TongHopThucThu_ThucChiVonBangTien.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
             DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Từ ngày " + iNgay1 + "/" + iThang1 + " đến ngày " + iNgay2 + "/" + iThang2 + "/" + iNamLamViec;
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTG_TongHopThucThu_ThucChiVonBangTien");
            LoadData(fr, MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dtKTTG_TongHopThucThu_ThucChiVonBangTien(MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        public static DataTable dtKTTG_TongHopThucThu_ThucChiVonBangTien(String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
      
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();

            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            String SQL = String.Format(@"
                                    SELECT  ROW_NUMBER() OVER (ORDER BY sKyHieu ) as STT,sKyHieu,sTen,SUM(rNo) as rNo,SUM(rCo) as rCo
                                    FROM(
                                        --NO--
                                        SELECT * FROM(
                                        SELECT sKyHieu,sTen,rNo=rSoTien,rCo=0 FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
	                                          AND iTrangThai=1 AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)>=@TuNgay
                                                               AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                                              AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112' {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_No
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich) as DKN

                                       UNION

                                        -- Co
                                        SELECT * FROM(
                                        SELECT sKyHieu,sTen,rNo=0,rCo=rSoTien FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
	                                          AND iTrangThai=1 AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)>=@TuNgay
                                                               AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                              AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112' {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_Co
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich) as DKC) as KT
                                        GROUP BY sKyHieu,sTen
                                         ", DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
