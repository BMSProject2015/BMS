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
using System.Globalization;
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.TCDN
{
    public class rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02Controller : Controller
    {
        // Edit: Thương
        // GET: /rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_ChiTieuTaiChinh_Quy_Nam.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["pageload"] = 0;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02.aspx";
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
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNam"]);
            String iALL = Convert.ToString(Request.Form[ParentID + "_iALL"]);
            ViewData["iQuy"] = iQuy;
            ViewData["iNam"] = iNam;
            ViewData["iMaDN"] = iMaDN;
            ViewData["iAll"] = iALL;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02.aspx";
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
        public DataTable BC_MS_ChiTieuTaiChinh(String iQuy, String iNam, String iAll, String iMaDN)
        {
            String DKQuy = iAll.Equals("on") ? "" : "AND TC.iQuy=@iQuy";
            String strTruyVan = String.Format(@"SELECT CTTC.TT
                                                        ,CTTC.sTen
                                                        ,CTTC.rSoCuoiKy
                                                        ,CTTC.rSoDauNam
                                                        ,CTTC.iSTT,CTTC.MS
                                                        ,MaSo=CASE WHEN (CTTC.MS=CT.sKyHieu AND CT.iLoai=0 AND CTTC.MS<>'')
                                                            THEN CTTC.MS+N'-BCĐKT'
                                                            ELSE 
                                                                CASE WHEN (CTTC.MS=CT.sKyHieu AND CT.iLoai=2 AND CTTC.MS<>'') THEN CTTC.MS+'-BCKQKD' ELSE '' END
                                                            END
                                                        ,TK=CASE WHEN (ISNUMERIC(CTTC.TT)=1 AND CTTC.TT<>'')
                                                            THEN 0 ELSE
                                                               CASE WHEN CTTC.TT<>'' THEN 1 ELSE 2 END 
                                                            END
                                                FROM(SELECT TC.sKyHieu as TT
	                                                    ,TC.sTen
	                                                    ,SUBSTRING(TC.sMaSoBangCanDoi,1,CHARINDEX(' -',TC.sMaSoBangCanDoi,0)) AS MS
	                                                    ,SUM(TC.rSoDauNam) AS rSoDauNam
	                                                    ,SUM(TC.rSoCuoiKy) AS rSoCuoiKy
	                                                    ,TC.iSTT
                                                FROM TCDN_ChiTieuTaiChinh AS TC
                                                WHERE TC.iTrangThai=1
                                                    AND TC.iID_MaDoanhNghiep=@iID_MaDoanhNghiep
                                                    {0}
                                                    AND TC.iNamLamViec=@iNamLamViec
                                                GROUP BY TC.sKyHieu
		                                                ,TC.sTen
		                                                ,TC.sMaSoBangCanDoi
		                                                ,TC.iSTT
                                                ) AS CTTC
                                                INNER JOIN (SELECT CT.sKyHieu,CT.iLoai FROM TCDN_ChiTieu AS CT WHERE CT.iTrangThai=1 AND CT.iLoai IN(0,2)) AS CT
                                                ON CT.sKyHieu=CTTC.MS
                                                GROUP BY CTTC.TT,CTTC.sTen,CTTC.rSoCuoiKy,CTTC.rSoDauNam,CTTC.iSTT,CTTC.MS,CT.sKyHieu,CT.iLoai
                                                ORDER BY CTTC.iSTT", DKQuy);
            SqlCommand cmd = new SqlCommand(strTruyVan);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iMaDN);
            if (!String.IsNullOrEmpty(DKQuy))
            {
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
            }
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
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        private void LoadData(FlexCelReport fr, String iQuy, String iNam, String iAll, String iMaDN)
        {            
            DataTable data = BC_MS_ChiTieuTaiChinh(iQuy, iNam, iAll, iMaDN);
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
        public ExcelFile CreateReport(String path, String iQuy, String iNam, String iAll, String iMaDN)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02");
            LoadData(fr, iQuy, iNam, iAll, iMaDN);            
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
            fr.SetValue("SoDauNam", SoDauQuyOrNam);
            fr.SetValue("SoCuoiKy", SoCuoiQuyOrNam);
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
        public ActionResult ViewPDF(String iQuy, String iNam, String iAll, String iMaDN)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iAll, iMaDN);
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
        public clsExcelResult ExportToExcel(String Quy, String Nam, String All, String MaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy, Nam, All, MaDN);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;                
                switch (All)
                { 
                    case "on":
                        clsResult.FileName = "BC_ChiTieuTaiChinh_Nam_" + Nam + ".xls";
                        break;
                    case "off":
                        clsResult.FileName = "BC_ChiTieuTaiChinh_Quy_"+ Quy +"_" + Nam + ".xls";
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
        /// <returns></returns>
        public static DataTable GetDoanhNghiep(String Quy, String Nam, String All)
        {
            String DKQuy = All.Equals("on") ? "" : "AND TCDN.iQuy=@iQuy";
            DataTable dtDN = new DataTable();
            String SQL = String.Format(@"SELECT TC.iID_MaDoanhNghiep,TC.sTenDoanhNghiep
                                        FROM TCDN_DoanhNghiep AS TC
                                        WHERE TC.iTrangThai=1
                                          AND TC.iID_MaDoanhNghiep IN(
	                                        SELECT TCDN.iID_MaDoanhNghiep 
	                                        FROM TCDN_ChiTieuTaiChinh AS TCDN
	                                        WHERE TCDN.iTrangThai=1 
	                                        AND TCDN.iNamLamViec=@iNamLamViec
	                                        {0}	                                        
                                        )", DKQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            if (!String.IsNullOrEmpty(DKQuy))
            {
                cmd.Parameters.AddWithValue("@iQuy", Quy);
            }
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
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public String obj_DSDoanhNghiep(String ParentID, String Quy, String Nam, String All, String iMaDN)
        {
            String dsDN = "";
            DataTable dtDoanhNghiep = GetDoanhNghiep(Quy, Nam, All);
            SelectOptionList slDoanhNghiep = new SelectOptionList(dtDoanhNghiep, "iID_MaDoanhNghiep", "sTenDoanhNghiep");
            dsDN = MyHtmlHelper.DropDownList(ParentID, slDoanhNghiep, iMaDN, "iMaDN", "", "class=\"input1_2\" style=\"width: 90%; padding:2px;\"");
            return dsDN;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="All">Lấy doanh nghiệp trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDoanhNghiep(string ParentID, String Quy, String Nam, String All, String iMaDN)
        {
            return Json(obj_DSDoanhNghiep(ParentID, Quy, Nam, All, iMaDN), JsonRequestBehavior.AllowGet);
        }        
    }
}
