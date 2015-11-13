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
    public class rptTCND_BC_KQHDKinhDoanhController : Controller
    {
        // Created by: Thương
        // GET: /rptTCND_BaoCaoKetQuaHoatDongKinhDoanh/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoKetQuaHoatDongKinhDoanh_B02.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["pageload"] = 0;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCND_BC_KQHDKinhDoanh.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["pageload"]=1;
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String MaDN = Convert.ToString(Request.Form[ParentID + "_iMaDN"]);
            String Quy = Convert.ToString(Request.Form[ParentID +"_iQuy"]);            
            ViewData["iNamLamViec"] = Nam;
            ViewData["iMaDN"] = MaDN;
            ViewData["iQuy"] = Quy;            
            ViewData["path"] = "~/Report_Views/TCDN/rptTCND_BC_KQHDKinhDoanh.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");            
        }
        /// <summary>
        /// Lấy dữ liệu hoạt động kinh doanh
        /// </summary>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="MaDN">Mã doanh nghiệp</param>
        /// <returns></returns>       
        public DataTable Hoat_Dong_Kinh_Doanh(String iQuy,String Nam, String MaDN)
        {
            String DKQuy="",DKSelect = "", DKNam = "";
            if (String.IsNullOrEmpty(iQuy)) iQuy = "-1";
            if (String.IsNullOrEmpty(MaDN)) MaDN = Guid.Empty.ToString();
            if (iQuy.Equals("1"))//Quý 1
            {
                DKQuy = "AND TCDN.iQuy=@iQuy";
                DKNam = "AND TCDN.iNamLamViec=@iNamLamViec";
                DKSelect = @",SUM(TCDN.rNamNay) AS rNamNay,SUM(TCDN.rNamTruoc) AS rNamTruoc";
            }
            else if (iQuy.Equals("7"))//Cả năm lấy số cuối năm của quý 4 của năm chọn và quý 4 năm trước 
            {
                DKQuy = "";
                DKNam = "";
                DKSelect = @",rNamNay=SUM(CASE WHEN TCDN.iQuy=@iQuy-3 AND TCDN.iNamLamViec=@iNamLamViec THEN TCDN.rNamNay ELSE 0 END)
                             ,rNamTruoc=SUM(CASE WHEN TCDN.iQuy=1 AND TCDN.iNamLamViec=@iNamLamViec THEN TCDN.rNamTruoc ELSE 0 END)";
            }
            else if (iQuy.Equals("5") || iQuy.Equals("6"))//6 tháng hoặc 9 tháng
            {
                DKSelect = @",rNamNay=SUM(CASE WHEN TCDN.iQuy=@iQuy-3 THEN TCDN.rNamNay ELSE 0 END),rNamTruoc=SUM(CASE WHEN TCDN.iQuy=1 THEN TCDN.rNamTruoc ELSE 0 END)";
                DKQuy = "";
                DKNam = "AND TCDN.iNamLamViec=@iNamLamViec";
            }
            else //Quý 2->4
            {
                DKSelect = @",rNamNay=SUM(CASE WHEN TCDN.iQuy=@iQuy   THEN TCDN.rNamNay ELSE 0 END)
                             ,rNamTruoc =SUM(CASE WHEN TCDN.iQuy=@iQuy-1 THEN TCDN.rNamNay ELSE 0 END)";
                DKQuy = "";
                DKNam = "AND TCDN.iNamLamViec=@iNamLamViec";
            }
            String strTruyVan = String.Format(@"SELECT TCDN.sTen,TCDN.sKyHieu,TCDN.sThuyetMinh
                                                  {1}
                                                  ,TCDN.iSTT                                                                                        
                                                FROM TCDN_KinhDoanh_ChungTuChiTiet AS TCDN	                                            
                                                WHERE TCDN.iTrangThai=1
                                                      AND TCDN.sKyHieu in(select tc.sKyHieu from TCDN_ChiTieu as tc where tc.iLoai=2)
	                                                  AND TCDN.iID_MaDoanhNghiep=@iID_MaDoanhNghiep	
	                                                  {2}
                                                      {0}                                                      
                                                GROUP BY TCDN.sTen, TCDN.sKyHieu,TCDN.sThuyetMinh,TCDN.iSTT
                                                ORDER BY TCDN.iSTT,TCDN.sKyHieu",DKQuy,DKSelect,DKNam);
            SqlCommand cmd = new SqlCommand(strTruyVan);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);            
            cmd.Parameters.AddWithValue("@iQuy", iQuy);            
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", MaDN);
            DataTable dtTruyVan = Connection.GetDataTable(cmd);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            cmd.Dispose();           
            return dtTruyVan;
        }       
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDN"></param>
        /// <returns></returns>        
        public ExcelFile CreateReport(String path, String iQuy, String Nam, String MaDN)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDN = MaDN.Equals(Guid.Empty.ToString()) ? "..................." : Convert.ToString(CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep", MaDN, "sTenDoanhNghiep"));
            String DiaChi = MaDN.Equals(Guid.Empty.ToString()) ? "..................." : Convert.ToString(CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep", MaDN, "sDiaChi"));
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCND_BC_KQHDKinhDoanh"); 
            LoadData(fr, iQuy,Nam, MaDN);                
            String footer = "Ghi chú: \n(*) Chỉ tiêu này chỉ áp dụng đối với công ty cổ phần";
            fr.SetValue("GhiChu", footer);
            fr.SetValue("DV", TenDN);
            fr.SetValue("DC", DiaChi);           
            String QuyNam = "Năm " + Nam;
            String SoCuoiQuy = String.IsNullOrEmpty(iQuy) ? "" : HamChung.GetDaysInMonth(ThangQuy(int.Parse(iQuy)), int.Parse(Nam)).ToString() + "/" + ThangQuy(int.Parse(iQuy)) + "/" + Nam;

            String DauKy = "", CuoiKy = "";
            if (iQuy.Equals("1"))
            {
                DauKy = "Số đầu năm";
                CuoiKy = "Số cuối kỳ";
            }
            else if (iQuy.Equals("5") || iQuy.Equals("6"))
            {
                DauKy = "Đầu năm";
                CuoiKy = "Cuối kỳ";
            }
            else if (iQuy.Equals("7"))
            {
                DauKy = "Đầu năm";
                CuoiKy = "Năm nay";
            }
            else
            {
                DauKy = "Đầu kỳ";
                CuoiKy = "Cuối kỳ";
            }            
            fr.SetValue("QuyNam", QuyNam.ToUpper());
            fr.SetValue("SoDauNam", DauKy);
            fr.SetValue("SoCuoiKy", CuoiKy);
            fr.SetValue("NgayThang", " Lập, " + ReportModels.Ngay_Thang_Nam_HienTai());
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
        /// Đổ dữ liệu xuống file báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NgayCT"></param>
        /// <param name="MaDN"></param>       
        private void LoadData(FlexCelReport fr, String Quy, String Nam, String MaDN)
        {            
            DataTable data = Hoat_Dong_Kinh_Doanh(Quy,Nam, MaDN);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);      
            data.Dispose();
        }       
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="MaDV">Mã doanh nghiệp</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Quy, String Nam, String MaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),Quy,Nam,MaDN);
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
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="MaDV">Mã doanh nghiệp</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Quy, String Nam, String MaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy,Nam,MaDN);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "HoatDong_KD_"+Nam+".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng file PDF
        /// </summary>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="MaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Quy, String Nam, String MaDN)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy, Nam, MaDN);
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
        /// Lấy danh sách doanh nghiệp
        /// </summary>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Lấy doanh nghiệp theo năm hay theo quý và năm</param>
        /// <returns></returns>
        public static DataTable GetDoanhNghiep(String Quy,String Nam)
        {
            String DKQuy = "";
            if (Quy.Equals("5") || Quy.Equals("6"))
                DKQuy = "AND TCDN.iQuy=@iQuy-3";
            else if (Quy.Equals("7"))
                DKQuy = "";
            else
                DKQuy = "AND TCDN.iQuy=@iQuy";
            DataTable dtDN = new DataTable();
            String SQL = String.Format(@"SELECT TC.iID_MaDoanhNghiep,TC.sTenDoanhNghiep
                                        FROM TCDN_DoanhNghiep AS TC
                                        WHERE TC.iTrangThai=1
                                          AND TC.iID_MaDoanhNghiep IN(
	                                        SELECT TCDN.iID_MaDoanhNghiep 
	                                        FROM TCDN_KinhDoanh_ChungTuChiTiet AS TCDN
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
        /// <param name="All">Tất cả doanh nghiệp trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public String obj_DSDoanhNghiep(String ParentID, String Quy,String Nam, String iMaDN)
        {
            String dsDN = "";
            DataTable dtDoanhNghiep = GetDoanhNghiep(Quy, Nam);
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
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDoanhNghiep(string ParentID, String Quy, String Nam, String iMaDN)
        {
            return Json(obj_DSDoanhNghiep(ParentID, Quy,Nam,iMaDN), JsonRequestBehavior.AllowGet);
        }
    }
}