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
    public class rptLuong_TongHopThuThueDauVaoController : Controller
    {
        // Edit: Le Van Thuong
        // Edit date: 07-07-2012
        // GET: /rptLuong_TongHopThuThueDauVao/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopThuThueDauVao.xls";
        public static String NameFile = "";
        public int count = 0;
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_TongHopThuThueDauVao.aspx";                        
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;
            String TuThang = Convert.ToString(Request.Form[ParentID + "_iTuThang"]);
            String DenThang = Convert.ToString(Request.Form[ParentID + "_iDenThang"]);             
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);
            String NamLuong = Convert.ToString(Request.Form[ParentID + "_iNam"]);
            ViewData["iTrangThai"] = TrangThai;
            ViewData["TuThang"] = TuThang;
            ViewData["DenThang"] = DenThang;
            ViewData["iNam"] = NamLuong;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_TongHopThuThueDauVao.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        #region "Lấy dữ liệu"
        /// <summary>
        /// Tổng hợp thu thuế đầu vào
        /// </summary>
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        /// <returns></returns>
        public DataTable rptLuong_TongHop_ThuThue(String TuThang, String DenThang,String NamLuong,String TrangThai)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String SQL = String.Format(@"SELECT L.iThangBangLuong as Thang
		                                    ,L.sHoDem_CanBo +' '+L.sTen_CanBo AS HoTen
		                                    ,L.sTenDonVi as DonVi
		                                    ,L.sNopThueDauVao_MoTa as NoiDung
		                                    ,SUM(L.rNopThueDauVao) as SoTien
                                        FROM L_BangLuongChiTiet as L
                                        WHERE L.iTrangThai=1
                                          AND L.iThangBangLuong BETWEEN @TuThang AND @DenThang
                                          AND L.iNamBangLuong=@iNamBangLuong                                      
                                          {0}
                                        GROUP BY L.iThangBangLuong,L.sHoDem_CanBo, L.sTen_CanBo,L.sTenDonVi,L.sNopThueDauVao_MoTa
                                        HAVING SUM(L.rNopThueDauVao)>0
                                        ORDER BY Thang
                                        ", DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            cmd.Parameters.AddWithValue("@iNamBangLuong", NamLuong);
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
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        /// <returns></returns>
        #region "Tạo báo cáo"
        public ExcelFile CreateReport(String path, String TuThang, String DenThang, String NamLuong, String TrangThai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_TongHopThuThueDauVao");                
            LoadData(fr, TuThang, DenThang,NamLuong,TrangThai);
            fr.SetValue("tuThang",TuThang);
            fr.SetValue("denThang", DenThang);            
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;         
        }
        #endregion
        /// <summary>
        /// Đẩy dữ liệu xuống file báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        #region "Đổ dữ liệu xuống file báo cáo"
        private void LoadData(FlexCelReport fr, String TuThang, String DenThang, String NamLuong, String TrangThai)
        {
            DataTable data = rptLuong_TongHop_ThuThue(TuThang, DenThang,NamLuong,TrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        #endregion
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String TuThang, String DenThang, String NamLuong, String TrangThai)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), TuThang, DenThang,NamLuong,TrangThai);
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
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String TuThang, String DenThang, String NamLuong, String TrangThai)
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), TuThang, DenThang,NamLuong,TrangThai);
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
        /// Xuất báo cáo ra file excel
        /// </summary>
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        /// <param name="NamLuong"></param>
        /// <param name="TrangThai"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String TuThang, String DenThang, String NamLuong, String TrangThai)
        {            
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), TuThang,DenThang,NamLuong,TrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHop_ThuThueDauVao.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
    }
}