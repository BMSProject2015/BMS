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

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_DuToan_QuyController : Controller
    {
        // Edit: Thuong
        // GET: /rptQLDA_DuToan_Quy/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_DuToan_Quy.xls";
        public static String NameFile = "";
        public int count = 0;
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_DuToan_Quy.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNam"]);            
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            return RedirectToAction("Index", new { Quy = Quy, Nam = Nam });
        }        
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        #region "Lấy dữ liệu"
        public DataTable DuToan_Quy(String Quy, String Nam)
        {
            String SQL = String.Format(@"SELECT 
                                            ROW_NUMBER() OVER (PARTITION BY Q.iID_MaNguonNganSach ORDER BY Q.iID_MaNguonNganSach) AS TT,
	                                        Q.iID_MaNguonNganSach AS NGS,
	                                        sTen=NNS.sTen,	
	                                        Q.iID_MaDanhMucDuAn as MaDA,
	                                        sTenDuAn=Da.sTenDuAn,
	                                        Q.rKeHoachVonNamTruoc/1000000 AS NT,
	                                        Q.rKeHoachVonNamNay/1000000 AS NN, 
	                                        Q.rKeHoachVonUngTruoc/1000000 AS UT,
	                                        Q.rSoTienCap/1000000 AS C,
	                                        Q.rSoTienDaCap/1000000 AS DC,
	                                        Q.rSoTienDuToan/1000000 AS DT
                                        FROM QLDA_DuToan_Quy AS Q
                                        INNER JOIN NS_NguonNganSach AS NNS
                                        ON NNS.iID_MaNguonNganSach=Q.iID_MaNguonNganSach
                                        INNER JOIN QLDA_DanhMucDuAn AS DA
                                        ON DA.iID_MaDanhMucDuAn = Q.iID_MaDanhMucDuAn
                                        WHERE Q.iTrangThai=1
                                          AND Q.iQuy=@iQuy
                                          AND Q.iNamLamViec=@iNamLamViec
                                          --AND Q.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iQuy", Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);            
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeVonDauTu);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();           
            return dt;
        }
        #endregion

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        #region "Tạo báo cáo"
        public ExcelFile CreateReport(String path, String Quy, String Nam)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_DuToan_Quy");
            LoadData(fr, Quy, Nam);
            fr.SetValue("quy", Quy);
            fr.SetValue("nam", Nam);
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        #endregion
        /// <summary>
        /// Đổ dữ liệu xuống file báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        #region "Đổ dữ liệu xuống file báo cáo"
        private void LoadData(FlexCelReport fr, String Quy, String Nam)
        {
            DataTable data = DuToan_Quy(Quy, Nam);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtNguon = HamChung.SelectDistinct("NguonNS", data, "NGS", "TT,NGS,sTen", "");
            fr.AddTable("NguonNS", dtNguon);
            data.Dispose();
            dtNguon.Dispose();
        }
        #endregion
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Quy, String Nam)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy, Nam);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "QLDA_DuToan_Quy.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }
            }
        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Quy, String Nam)
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy, Nam);
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
        /// 
        /// </summary>
        /// <returns></returns>
       
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Quy, String Nam)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Quy, Nam);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QLDA_DuToan_Quy.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
    }
}
