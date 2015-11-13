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
    public class rptTCDN_BangCanDoiKeToan_B01DNController : Controller
    {
        // Edit date: 07-07-2012
        // GET: /rptTCDN_BangCanDoiKeToan_B01DN/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BangCanDoiKeToan_B01.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["pageload"] = 0;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BangCanDoiKeToan_B01.aspx";
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
            String NgayCT = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];           
            String iMaDN = Convert.ToString(Request.Form[ParentID + "_iMaDN"]);            
            String iQuy=Convert.ToString(Request.Form[ParentID+"_iQuy"]);
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iALL = Convert.ToString(Request.Form[ParentID + "_iALL"]);
            ViewData["iQuy"] = iQuy;
            ViewData["iNam"] = iNam;
            ViewData["iMaDN"] = iMaDN;
            ViewData["iAll"] = iALL;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BangCanDoiKeToan_B01.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");    
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <param name="DKNot">Loại chỉ tiêu tài chính doanh nghiệp 0 hoặc 1</param>
        /// <param name="Orderby">Sắp xếp theo trường</param>
        /// <returns></returns>        
        public static DataTable Bang_CanDoi_KeToan(String iQuy,String iNam, String iMaDN,String DKNot,String Orderby,String sKyHieu)
        {            
            String DKQuy="",DKSelect = "", DKNam = "";
            if (iQuy.Equals("1"))//Quý 1
            {
                DKQuy = "AND TCDN.iQuy=@iQuy";
                DKNam = "AND TCDN.iNamLamViec=@iNamLamViec";
                DKSelect = @",SUM(TCDN.rSoCuoiNam) AS rSoCuoiNam,SUM(TCDN.rSoDauNam) AS rSoDauNam";
            }
            else if (iQuy.Equals("7"))//Cả năm lấy số cuối năm của quý 4 của năm chọn và quý 4 năm trước 
            {
                DKQuy = "";
                DKNam = "";
                DKSelect = @",rSoCuoiNam=SUM(CASE WHEN TCDN.iQuy=@iQuy-3 AND TCDN.iNamLamViec=@iNamLamViec THEN TCDN.rSoCuoiNam ELSE 0 END)
                             ,rSoDauNam =SUM(CASE WHEN TCDN.iQuy=1 AND TCDN.iNamLamViec=@iNamLamViec THEN TCDN.rSoDauNam ELSE 0 END)";
            }
            else if (iQuy.Equals("5") || iQuy.Equals("6"))//6 tháng hoặc 9 tháng
            {
                DKSelect = @",rSoCuoiNam=SUM(CASE WHEN TCDN.iQuy=@iQuy-3 THEN TCDN.rSoCuoiNam ELSE 0 END),rSoDauNam=SUM(CASE WHEN TCDN.iQuy=1 THEN TCDN.rSoDauNam ELSE 0 END)";
                DKQuy = "";
                DKNam = "AND TCDN.iNamLamViec=@iNamLamViec";
            }
            else //Quý 2->4
            {
                DKSelect = @",rSoCuoiNam=SUM(CASE WHEN TCDN.iQuy=@iQuy   THEN TCDN.rSoCuoiNam ELSE 0 END)
                             ,rSoDauNam =SUM(CASE WHEN TCDN.iQuy=@iQuy-1 THEN TCDN.rSoCuoiNam ELSE 0 END)";
                DKQuy = "";
                DKNam = "AND TCDN.iNamLamViec=@iNamLamViec";
            }
            String strTruyVan = String.Format(@"SELECT TCDN.sTen,TCDN.sKyHieu,TCDN.sThuyetMinh
                                                  {4}--Điều kiện select
                                                  ,TCDN.iSTT
                                                  ,HangCha=CASE TCDN.bLaHangCha WHEN 'True' THEN '1' else '0' end                                                                                        
                                                FROM TCDN_ChungTuChiTiet AS TCDN
                                                LEFT JOIN TCDN_ChungTu AS CT
                                                ON CT.iID_MaChungTu=TCDN.iID_MaChungTu
                                                 INNER JOIN (SELECT iLoai,iID_MaChiTieu FROM TCDN_ChiTieu WHERE iLoai{3}1 AND sTen<> N'CÁC CHỈ TIÊU NGOÀI BẢNG CÂN ĐỐI KẾ TOÁN') as a ON a.IID_MaChiTieu= TCDN.IID_MaChiTieu 
                                                WHERE TCDN.iTrangThai=1                                                                                                        
                                                    AND TCDN.sKyHieu in(select tc.sKyHieu from TCDN_ChiTieu as tc where tc.iLoai='{0}')
                                                    AND TCDN.iID_MaDoanhNghiep=@iID_MaDoanhNghiep
                                                    {5}--Điều kiện năm  
                                                    {2}--Điều kiện quý
                                                     --Điều kiện loại ký hiệu
                                                    --AND TCDN.sTen<>N'CÁC CHỈ TIÊU NGOÀI BẢNG CÂN ĐỐI KẾ TOÁN'
                                                      --AND TCDN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                                GROUP BY TCDN.sTen, TCDN.sKyHieu,TCDN.sThuyetMinh,TCDN.iSTT,TCDN.bLaHangCha
                                                ORDER BY {1}", DKNot,Orderby,DKQuy,sKyHieu,DKSelect,DKNam);
            SqlCommand cmd = new SqlCommand(strTruyVan);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iMaDN);            
            cmd.Parameters.AddWithValue("@iQuy", iQuy);            
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dtTruyVan = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtTruyVan;
        }   
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn đến file excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>       
        public ExcelFile CreateReport(String path, String iQuy, String iNam, String iMaDN)
        {            
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDN = iMaDN.Equals(Guid.Empty.ToString()) ? "..................." : Convert.ToString(CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep", iMaDN, "sTenDoanhNghiep"));
            String DiaChi = iMaDN.Equals(Guid.Empty.ToString()) ? "..................." : Convert.ToString(CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep", iMaDN, "sDiaChi"));
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BangCanDoiKeToan_B01DN");       
            LoadData(fr, iQuy,iNam,iMaDN);
            fr.SetValue("TenDN", TenDN);
            fr.SetValue("DiaChi", DiaChi);                
            String footer="Ghi chú:           \n           (1) Những chỉ tiêu không số liệu có thể không phải trình bày nhưng không được đánh lại số thứ tự chỉ\n           tiêu và \"Mã số\"\n           (2) Số liệu trong các chỉ tiêu có dấu (*) được ghi bằng số âm dưới hình thức ghi trong ngoặc đơn (…)        \n           (3) Đối với doanh nghiệp có kỳ kế toán năm là năm dương lịch (X) thì \"\"Số cuối năm\"\" có thể ghi là\n           \"31.12.X\"\"; \"\"Số đầu năm\"\" có thể ghi là \"01.01.X\"";
            fr.SetValue("Footer", footer);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());            
            String QuyNam = "Năm "+iNam;
            String SoCuoiQuy = iQuy.Equals("0") ? "" : HamChung.GetDaysInMonth(ThangQuy(int.Parse(iQuy)), int.Parse(iNam)).ToString() + "/" + ThangQuy(int.Parse(iQuy)) + "/" + iNam;
            
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
            fr.SetValue("QuyNam", QuyNam);
            fr.SetValue("SoDauQuy",DauKy );            
            fr.SetValue("SoCuoiQuy", CuoiKy);
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
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">Đường dẫn tới file excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        private void LoadData(FlexCelReport fr, String iQuy, String iNam, String iMaDN)
        {            
            //Bảng cân đối kế toán
            DataTable data = Bang_CanDoi_KeToan(iQuy, iNam, iMaDN, "0", " TCDN.iSTT,TCDN.sKyHieu ", "<>");
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);            
            //Các chỉ tiêu ngoài bảng cân đối kế toán
            DataTable dt = Bang_CanDoi_KeToan(iQuy, iNam, iMaDN, "1", " TCDN.iSTT ", "=");
            data.TableName = "ChiTiet1";
            fr.AddTable("ChiTiet1", dt);
            dt.Dispose();
            data.Dispose();
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="iQuy"></param>
        /// <param name="iNam"></param>
        /// <param name="iAll"></param>
        /// <param name="iMaDN"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iQuy, String iNam, String iMaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();            
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy,iNam,iMaDN);
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
        /// ViewPDF
        /// </summary>
        /// <param name="NgayCT"></param>
        /// <param name="MaDN"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iQuy, String iNam, String iMaDN)
        {
            HamChung.Language();            
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iMaDN);
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
        /// <param name="All">Lấy trong 1 quý hoặc tất cả các quý trong năm</param>
        /// <param name="MaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Quy, String Nam,String MaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy, Nam, MaDN);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CanDoiKeToan_" + Nam + ".xls";
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
        public static DataTable GetDoanhNghiep(String Quy, String Nam)
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
	                                        FROM TCDN_ChungTuChiTiet AS TCDN
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
        /// <param name="All">Tất cả doanh nghiệp trng năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public String obj_DSDoanhNghiep(String ParentID, String Quy, String Nam, String iMaDN)
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
            return Json(obj_DSDoanhNghiep(ParentID, Quy, Nam, iMaDN), JsonRequestBehavior.AllowGet);
        }
    }    
}