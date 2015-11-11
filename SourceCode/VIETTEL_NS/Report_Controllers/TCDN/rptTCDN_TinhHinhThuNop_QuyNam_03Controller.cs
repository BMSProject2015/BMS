using System;
using System.Collections.Generic;
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
using System.Globalization;
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.TCDN
{
    public class rptTCDN_TinhHinhThuNop_QuyNam_03Controller : Controller
    {
        // Created: Thương 
        // GET: /rptTCDN_TinhHinhThuNop_QuyNam_03/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_TinhHinhThuNop_Quy_Nam.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["pageload"] = 0;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_TinhHinhThuNop_QuyNam_03.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["pageload"] = 1;
            String iMaDN = Convert.ToString(Request.Form[ParentID + "_iMaDN"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iALL = Convert.ToString(Request.Form[ParentID + "_iALL"]);
            String iLoaiDN = Convert.ToString(Request.Form[ParentID + "_iLoaiDN"]);
            ViewData["iQuy"] = iQuy;
            ViewData["iNam"] = iNam;
            ViewData["iMaDN"] = iMaDN;
            ViewData["iAll"] = iALL;
            ViewData["iLoaiDN"] = iLoaiDN;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_TinhHinhThuNop_QuyNam_03.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp theo quý hay theo năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public DataTable BC_TinhHinhThuNop(String iQuy, String iNam, String iAll,String iLoaiDN, String iMaDN)
        {
            String DKQuy = iAll.Equals("on") ? "" : "AND CTTN.iQuy=@iQuy";
            String strTruyVan = String.Format(@"SELECT CTTN.sKyHieu
                                                    ,CTTN.sTen
                                                    --,CTTN.iSTT
                                                    ,SUM(CTTN.rKyTruocChuyenSang) rKyTruocChuyenSang
                                                    ,SUM(CTTN.rTrongKyPhaiNop) rTrongKyPhaiNop
                                                    ,SUM(CTTN.rTrongKyDaNop) rTrongKyDaNop
                                                    ,SUM(CTTN.rSoConNoCuoiKy) rSoConNoCuoiKy
                                                    ,TK=CASE WHEN CTTN.bLaHangCha='TRUE' THEN 1 ELSE 0 END
                                                FROM TCDN_ChiTieuThuNop AS CTTN
                                                INNER JOIN (SELECT DN.iID_MaDoanhNghiep FROM TCDN_DoanhNghiep DN WHERE DN.iTrangThai=1 AND DN.iID_MaLoaiDoanhNghiep=@iID_MaLoaiDoanhNghiep) DN
                                                ON DN.iID_MaDoanhNghiep=CTTN.iID_MaDoanhNghiep
                                                WHERE CTTN.iTrangThai=1
                                                  {0}
                                                  AND CTTN.iNamLamViec=@iNamLamViec
                                                  AND CTTN.iID_MaDoanhNghiep=@iID_MaDoanhNghiep
                                                GROUP BY CTTN.iSTT,CTTN.sTen,CTTN.sKyHieu,CTTN.bLaHangCha", DKQuy);
            SqlCommand cmd = new SqlCommand(strTruyVan);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iMaDN);
            if (!String.IsNullOrEmpty(DKQuy))
            {
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
            }
            cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iLoaiDN);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dtTruyVan = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtTruyVan;
        }
        /// <summary>
        /// Đẩy dữ xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cao</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp theo quý hay theo năm</param>
        /// <param name="iLoaiDN">Loại hình doanh nghiệp</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        private void LoadData(FlexCelReport fr, String iQuy, String iNam, String iAll, String iLoaiDN, String iMaDN)
        {
            DataTable data = BC_TinhHinhThuNop(iQuy, iNam, iAll,iLoaiDN, iMaDN);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp theo quý hay theo năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iQuy, String iNam, String iAll, String iLoaiDN, String iMaDN)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_TinhHinhThuNop_QuyNam_03");
            LoadData(fr, iQuy, iNam, iAll,iLoaiDN, iMaDN);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            String SoDauQuyOrNam = "Số đầu năm";
            String SoCuoiQuyOrNam = "Số cuối kỳ";
            String QuyNam = "Năm " + iNam;
            String SoCuoiQuy = iQuy.Equals("0") ? "" : HamChung.GetDaysInMonth(ThangQuy(int.Parse(iQuy)), int.Parse(iNam)).ToString() + "/" + ThangQuy(int.Parse(iQuy)) + "/" + iNam;
            switch (iAll)
            {
                case "on":
                    SoDauQuyOrNam = "Số đầu năm";
                    SoCuoiQuyOrNam = "Số cuối kỳ";
                    QuyNam = "Năm " + iNam;
                    break;
                case "off":
                    SoDauQuyOrNam = "01/01/" + iNam;
                    SoCuoiQuyOrNam = SoCuoiQuy;
                    QuyNam = "Quý " + iQuy + " năm " + iNam;
                    break;
            }
            fr.SetValue("QuyNam", QuyNam.ToUpper());           
            fr.SetValue("TenDN", CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep", iMaDN, "sTenDoanhNghiep"));
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Lấy tháng cuối của quý
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <returns></returns>
        public int ThangQuy(int iQuy)
        {
            int thang = 3;
            switch (iQuy)
            {
                case 1:
                    thang = 3;
                    break;
                case 2:
                    thang = 6;
                    break;
                case 3:
                    thang = 9;
                    break;
                case 4:
                    thang = 12;
                    break;
            }
            return thang;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp theo quý hay theo năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iQuy, String iNam, String iAll, String iLoaiDN, String iMaDN)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iAll,iLoaiDN, iMaDN);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Tổng hợp theo quý hay theo năm</param>
        /// <param name="MaDN">Mã doanh nghiệp báo cáo</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iQuy, String iNam, String iAll, String iLoaiDN, String iMaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iAll,iLoaiDN, iMaDN);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                switch (iAll)
                {
                    case "on":
                        clsResult.FileName = "BC_TinhHinhThuNop_Nam_" + iNam + ".xls";
                        break;
                    case "off":
                        clsResult.FileName = "BC_TinhHinhThuNop_Quy_" + iQuy + "_" + iNam + ".xls";
                        break;
                }

                clsResult.type = "xls";
                return clsResult;
            }
        }        
        /// <summary>
        /// Lấy danh sách doanh nghiệp
        /// </summary>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Lấy doanh nghiệp theo năm hay theo quý và năm</param>
        /// <param name="iLoaiDN">Loại hình doanh nghiệp</param>
        /// <returns></returns>
        public static DataTable GetDoanhNghiep(String Quy, String Nam, String All,String iLoaiDN)
        {
            String DKQuy = All.Equals("on") ? "" : "AND TCDN.iQuy=@iQuy";
            DataTable dtDN = new DataTable();
            String SQL = String.Format(@"SELECT TC.iID_MaDoanhNghiep,TC.sTenDoanhNghiep
                                        FROM TCDN_DoanhNghiep AS TC
                                        WHERE TC.iTrangThai=1
                                          AND TC.iID_MaDoanhNghiep IN(
	                                        SELECT TCDN.iID_MaDoanhNghiep 
	                                        FROM TCDN_ChiTieuThuNop AS TCDN
	                                        WHERE TCDN.iTrangThai=1 
	                                        AND TCDN.iNamLamViec=@iNamLamViec
	                                        {0})
                                          AND TC.iID_MaLoaiDoanhNghiep=@iID_MaLoaiDoanhNghiep
                                        ", DKQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            if (!String.IsNullOrEmpty(DKQuy))
            {
                cmd.Parameters.AddWithValue("@iQuy", Quy);
            }
            cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iLoaiDN);
            dtDN = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDN;
        }
        /// <summary>
        /// Hiện thị danh sách lên View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Lấy danh sách doanh nghiệp theo quý hay theo năm</param>
        /// <param name="iLoaiDN">Loại hình doanh nghiệp</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public String obj_DSDoanhNghiep(String ParentID, String Quy, String Nam, String All, String iLoaiDN, String iMaDN)
        {
            String dsDN = "";
            DataTable dtDoanhNghiep = GetDoanhNghiep(Quy, Nam, All,iLoaiDN);
            SelectOptionList slDoanhNghiep = new SelectOptionList(dtDoanhNghiep, "iID_MaDoanhNghiep", "sTenDoanhNghiep");
            dsDN = MyHtmlHelper.DropDownList(ParentID, slDoanhNghiep, iMaDN, "iMaDN", "", "class=\"input1_2\" style=\"width: 140px; padding:2px;\"");
            return dsDN;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="All">Lấy doanh nghiệp trong năm</param>
        /// <param name="iLoaiDN">Loại hình doanh nghiệp</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDoanhNghiep(string ParentID, String Quy, String Nam, String All, String iLoaiDN, String iMaDN)
        {
            return Json(obj_DSDoanhNghiep(ParentID, Quy, Nam, All,iLoaiDN, iMaDN), JsonRequestBehavior.AllowGet);
        }        
    }
}