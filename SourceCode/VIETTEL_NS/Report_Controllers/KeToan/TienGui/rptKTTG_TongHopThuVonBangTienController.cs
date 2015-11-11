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
    public class rptKTTG_TongHopThuVonBangTienController : Controller
    {
        //
        // GET: /rptKTTG_TongHopSoDuVonBangTien/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTG_TongHopThuVonBangTien.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_TongHopThuVonBangTien.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
           
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_TongHopThuVonBangTien.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String MaND,String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iThang, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String MaND, String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iThang, iID_MaTrangThaiDuyet);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTG_TongHopThuVonBangTien.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iThang, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
             DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Tháng "+iThang+" năm "+iNamLamViec;
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTG_TongHopThuVonBangTien");
            LoadData(fr, MaND,iThang, iID_MaTrangThaiDuyet);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iThang, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dtKTTG_TongHopThuVonBangTien(MaND, iThang,iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        public static DataTable dtKTTG_TongHopThuVonBangTien(String MaND, String iThang, String iID_MaTrangThaiDuyet)
        {

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();

          
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
                                    SELECT  sNoiDung,iNgayCT,sSoChungTuChiTiet,sKyHieu,sTen,SUM(rSoTien) as rSoTien
                                    FROM(
                                        --NO--
                                        SELECT * FROM(
                                        SELECT sNoiDung,iNgayCT,sSoChungTuChiTiet,sKyHieu,sTen,rSoTien FROM (
                                        SELECT sNoiDung,iNgayCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
	                                          AND iTrangThai=1 AND iThangCT=@iThang  AND iNamLamViec=@iNamLamViec {0} AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'
                                        GROUP BY sNoiDung,iID_MaTaiKhoanGiaiThich_No,iNgayCT,sSoChungTuChiTiet
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich) as DKN

                                       UNION

                                        -- Co
                                        SELECT * FROM(
                                        SELECT sNoiDung,iNgayCT,sSoChungTuChiTiet,sKyHieu,sTen,rSoTien FROM (
                                        SELECT sNoiDung,iNgayCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
	                                          AND iTrangThai=1 AND iThangCT=@iThang AND iNamLamViec=@iNamLamViec {0} AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112'
                                        GROUP BY sNoiDung,iID_MaTaiKhoanGiaiThich_Co,iNgayCT,sSoChungTuChiTiet
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich) as DKC) as KT
                                        GROUP BY sNoiDung,iNgayCT,sSoChungTuChiTiet,sKyHieu,sTen
                                        ORDER BY iNgayCT", DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
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
