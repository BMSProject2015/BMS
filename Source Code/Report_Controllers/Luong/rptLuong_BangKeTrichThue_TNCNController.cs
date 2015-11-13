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

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_BangKeTrichThue_TNCNController : Controller
    {
        // Edit date: 07-07-2012
        // User edit: Thuong
        // GET: /rptLuong_BangKeTrichThue_TNCN/

        public string sViewPath = "~/Report_Views/";
        //private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;                      
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_BangKeTrichThue_TNCN.aspx";            
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;
            String OnOff = Convert.ToString(Request.Form[ParentID + "_divOnOrOff"]);
            String isGroup = Convert.ToString(Request.Form[ParentID + "_divGroup"]);
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThangLuong"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNamLuong"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_divPages"]);
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iNamLuong"] = Nam;
            ViewData["iThangLuong"] = Thang;
            ViewData["iOnOff"] = OnOff;
            ViewData["iGroup"] = isGroup;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_BangKeTrichThue_TNCN.aspx";
            String sFilePath = "";
            switch(KhoGiay)
            {
                case "A4":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi.xls";                    
                    break;
                case "A3":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_A3.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi_A3.xls";                    
                    break;
            }
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        #region "Lấy dữ liệu"
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public DataTable rptLuong_ThueTNCN(String Thang, String Nam, String TrangThai, String isALlDonvi)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String DKHaving = isALlDonvi.Equals("on") ? "" : " HAVING SUM(L.rThueTNCN)>0 ";
            String SQL = String.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY L.sTen_CanBo) AS [TT]
                                               ,ROW_NUMBER() OVER (PARTITION BY L.iID_MaDonVi ORDER BY L.iID_MaDonVi) AS [TT1]
	                                           ,L.sHoDem_CanBo as HoDem
                                               ,L.sTen_CanBo as Ten 
                                               --THU NHẬP TÍNH THUẾ
                                               ,SUM(L.rLuongCoBan+L.rPhuCap_ChucVu) rLuongCoBan
                                               ,SUM(L.rThuong) AS THUONG
                                               ,SUM(L.rLoiIchKhac) AS tKHAC	
                                        --CÁC KHOẢN GIẢM TRỪ
                                               ,SUM(L.rGiamTruGiaCanh) as PT
                                               ,SUM(L.rBaoHiem_Tong_CaNhan) as BAOHIEM
                                               ,SUM(L.rGiamTruKhac) AS gtKHAC
                                               ,SUM(l.rGiamTruBanThan) as rGiamTruBanThan
                                               ,SUM(L.rTienTinhThueTNCN) AS rTienTinhThueTNCN           
                                        --SỐ TIỀN THUẾ PHẢI NỘP
	                                          ,SUM(L.rThueTNCN) AS rThueTNCN
                                              ,SUM(L.rDuocGiamThue) AS GIAMTHUE
                                        --TIỀN THUẾ ĐÃ NỘP
                                              ,SUM(L.rDaNopThue) rDaNopThue
                                              ,L.iID_MaDonVi
                                        FROM L_BangLuongChiTiet L
                                        WHERE iTrangThai=1
                                            AND L.iNamBangLuong=@iNamBangLuong                                        
                                            AND L.iThangBangLuong=@iThangBangLuong
                                            {0}                                                      
                                        GROUP BY L.sHoDem_CanBo,L.sTen_CanBo,L.iID_MaDonVi   
                                        {1}                                                                           
                                        ORDER BY L.sTen_CanBo ASC
                                        ", DKDuyet,DKHaving);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        #endregion
        #region "Tạo báo cáo"
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String Thang, String Nam, String TrangThai, String isALLDonVi, String isGroup)
        {
            XlsFile Result = new XlsFile(true);            
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();           
            //Thêm chữ ký vào báo cáo
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_BangKeTrichThue_TNCN");
            LoadData(fr, Thang, Nam, TrangThai,isALLDonVi,isGroup);
            fr.SetValue("cap1", "");
            fr.SetValue("cap2", "");
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("ThangNam", "Tháng " + Thang + " / " + Nam);
            fr.Run(Result);
            return Result;           
        }
        #endregion
        #region "Đổ dữ liệu xuống file báo cáo"
        /// <summary>
        /// "Đổ dữ liệu xuống file báo cáo"
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        private void LoadData(FlexCelReport fr, String Thang, String Nam, String TrangThai, String isALLDonVi, String isGroup)
        {
            switch (isGroup)
            {
                case "rTheoTen":
                    DataTable data = rptLuong_ThueTNCN(Thang, Nam, TrangThai,isALLDonVi);
                    data.TableName = "ChiTiet";
                    fr.AddTable("ChiTiet", data);
                    data.Dispose();
                    break;
                case "rTheoDonVi":
                    DataTable dt = rptLuong_ThueTNCN(Thang, Nam, TrangThai,isALLDonVi);
                    dt.TableName = "ChiTiet";
                    DataTable dtDonvi = HamChung.SelectDistinct("Donvi", dt, "iID_MaDonVi", "iID_MaDonVi");
                    fr.AddTable("ChiTiet", dt);
                    fr.AddTable("Donvi", dtDonvi);
                    dtDonvi.Dispose();
                    dt.Dispose();                    
                    break;                                   
            }
        }
        #endregion
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Thang, String Nam, String TrangThai, String isALLDonVi, String KhoGiay, String isGroup)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi.xls";
                    break;
                case "A3":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_A3.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi_A3.xls";
                    break;
            }            
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang, Nam,TrangThai,isALLDonVi,isGroup);
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
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Thang, String Nam, String TrangThai,String isALLDonVi,String KhoGiay,String isGroup)
        {
            HamChung.Language();
            String sFilePath = "";
            switch(KhoGiay)
            {
                case "A4":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi.xls";                    
                    break;
                case "A3":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_A3.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi_A3.xls";                    
                    break;
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang, Nam, TrangThai,isALLDonVi,isGroup);
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
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Thang, String Nam, String TrangThai, String isALLDonVi, String KhoGiay, String isGroup)
        {
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi.xls";                    
                    break;
                case "A3":
                    if (isGroup == "rTheoTen")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_A3.xls";
                    else if (isGroup == "rTheoDonVi")
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeTrichThue_TNCN_TheoDonVi_A3.xls";                    
                    break;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang, Nam, TrangThai,isALLDonVi,isGroup);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangKeTrichThue_TNCN.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
    }
}