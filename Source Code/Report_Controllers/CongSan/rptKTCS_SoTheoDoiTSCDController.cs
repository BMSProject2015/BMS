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
    public class rptKTCS_SoTheoDoiTSCDController : Controller
    {
        //
        // GET: /rptKTCS_SoTheoDoiTSCD/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTheoDoiTaiSan_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTheoDoiTSCD.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_SoTheoDoiTSCD.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Acion thực hiện truyền các tham số trên form
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String ThangChungTu = Request.Form[ParentID + "_ThangChungTu"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);

            ViewData["PageLoad"] = "1";
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["ThangChungTu"] = ThangChungTu;
            ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHopDonVi"] = TongHopDonVi;
            ViewData["TongHopLTS"] = TongHopLTS;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_SoTheoDoiTSCD.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Xuất ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên tài sản
            String TenLoaiTaiSan = "";
            DataTable dtLoaiTaiSan = dtTenLoaiTaiSan(iID_MaLoaiTaiSan);
            if (dtLoaiTaiSan.Rows.Count > 0)
            {
                TenLoaiTaiSan = dtLoaiTaiSan.Rows[0][0].ToString();
            }
            //Lấy tên đơn vị
            String tendv = "";
            DataTable teN = TenDonVi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //Hàm đổi tiền từ số sang chữ
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_SoTheoDoiTSCD");
            LoadData(fr, NamChungTu, ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            fr.SetValue("Nam", NamChungTu);
            fr.SetValue("Thang", ThangChungTu);
            if (TongHopDonVi == "on")
            {
                fr.SetValue("TenDV", "Tổng hợp đơn vị");
            }
            else
            {
                fr.SetValue("TenDV", tendv);
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("TongCucKiThuat", "TỔNG CỤC KĨ THUẬT");
            fr.SetValue("Ngay", NgayThang);
            if (TongHopLTS == "on")
            {
                fr.SetValue("TenLoaiTS", "Tất cả loại tài sản");
            }
            else { fr.SetValue("TenLoaiTS", TenLoaiTaiSan); }
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// DataTable lấy tên loại tài sản
        /// </summary>
        /// <returns></returns>
        public DataTable dtTenLoaiTaiSan(String iID_MaLoaiTaiSan)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM KTCS_LoaiTaiSan WHERE iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
            return dt = Connection.GetDataTable(cmd);
        }

        /// <summary>
        /// DataTable lấy dự liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable rptKTCS_SoTheoDoiTSCD(String NamChungTu, String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {

            DataTable dt = null;
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }

            String DKDonVi = "KTCS_TaiSan.iID_MaDonVi='-111'", DKNhomTaiSan = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = KTCS_ReportModel.ListDonVi();
                if (dtDonVi.Rows.Count > 0)
                {
                    DKDonVi = "";
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DKDonVi += "KTCS_TaiSan.iID_MaDonVi = '" + dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + "'";
                        if (i < dtDonVi.Rows.Count - 1)
                            DKDonVi += " OR ";

                    }
                    dtDonVi.Dispose();
                }
            }
            else
            {
                DKDonVi = "";
                DKDonVi = "  KTCS_TaiSan.iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'";
            }
            if (TongHopLTS == "on")
            {
                DataTable dtNhomTaiSan = KTCS_ReportModel.DT_LoaiTS();
                for (int i = 0; i < dtNhomTaiSan.Rows.Count; i++)
                {
                    DKNhomTaiSan += "iID_MaNhomTaiSan like N'" + dtNhomTaiSan.Rows[i]["iID_MaNhomTaiSan"].ToString() + "%'";
                    if (i < dtNhomTaiSan.Rows.Count - 1)
                        DKNhomTaiSan += " OR ";
                }
            }
            else
            {
                DKNhomTaiSan = " iID_MaNhomTaiSan LIKE N'" + iID_MaLoaiTaiSan + "%'";
            }

            String SQL = String.Format(@" 
SELECT iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,a.iID_MaTaiSan,sTenTaiSan,sLyDo,sDonViTinh
,SUM(rSoLuong) as rSoLuong,SUM(rSoTien) as rSoTien ,SUM(rSoLuong_1) as rSoLuong_1,SUM(rSoTien_1) as rSoTien_1
FROM(
SELECT iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,KTCS_TaiSan.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan
,sDonViTinh
,SUM(CASE WHEN sTinhChat='1' OR sTinhChat='T' THEN rSoLuong ELSE 0 END) as rSoLuong
,SUM(CASE WHEN sTinhChat='1' OR sTinhChat='T' THEN rSoTien ELSE 0 END) as rSoTien
,sLyDo
,rSoLuong_1=0
,rSoTien_1=0
FROM KTCS_ChungTuChiTiet
INNER JOIN KTCS_TaiSan
on KTCS_ChungTuChiTiet.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
WHERE  iThangCT=@iThangLamViec AND KTCS_TaiSan.iNamLamViec=@iNamLamViec AND KTCS_TaiSan.iTrangThai=1 AND KTCS_ChungTuChiTiet.iTrangThai=1  AND ({0})
GROUP BY iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,KTCS_TaiSan.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan,sDonViTinh,sLyDo

UNION

SELECT iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,KTCS_TaiSan.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan
,sDonViTinh
,rSoLuong=0
,rSoTien=0
,sLyDo
,SUM(CASE WHEN sTinhChat='2' THEN rSoLuong ELSE 0 END) as rSoLuong_1
,SUM(CASE WHEN sTinhChat='2' THEN rSoTien ELSE 0 END) as rSoTien_1
FROM KTCS_ChungTuChiTiet
INNER JOIN KTCS_TaiSan
on KTCS_ChungTuChiTiet.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
WHERE iThangCT=@iThangLamViec AND KTCS_TaiSan.iNamLamViec=@iNamLamViec AND KTCS_TaiSan.iTrangThai=1 AND KTCS_ChungTuChiTiet.iTrangThai=1 AND ({0})
GROUP BY iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,KTCS_TaiSan.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan,sDonViTinh,sLyDo) as a
INNER JOIN(SELECT iID_MaTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 AND ( {1})) as B
                                          ON a.iID_MaTaiSan=B.iID_MaTaiSan 
GROUP BY iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,a.iID_MaTaiSan,sTenTaiSan,sLyDo,sDonViTinh
HAVING SUM(rSoTien)<>0 OR  SUM(rSoTien_1)<>0
", DKDonVi, DKNhomTaiSan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            cmd.Parameters.AddWithValue("@iThangLamViec", ThangChungTu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// DataTable lấy tên đơn vị
        /// </summary>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTenDonVi FROM KTCS_ChungTuChiTiet WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Load dự liệu ra báo cao
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            DataTable data = rptKTCS_SoTheoDoiTSCD(NamChungTu, ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        /// <summary>
        /// Action thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTheoDoiTaiSan_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTheoDoiTSCD.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "SoTheoDoiTaiSanCoDinh_A3";
                }
                else
                {
                    clsResult.FileName = "SoTheoDoiTaiSanCoDinh";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Action Xem báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu, String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTheoDoiTaiSan_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTheoDoiTSCD.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
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
    }
}
