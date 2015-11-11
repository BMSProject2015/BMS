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
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_DSCaNhan_CoThuLao_ThuongController : Controller
    {
        // Edit: Le Van Thuong
        // GET: /rptLuong_DSCaNhan_CoThuLao_Thuong/
        //        
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_DSCaNhan_CoThuLao_Thuong.xls";
        public static String NameFile = "";
        public int count = 0;
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DSCaNhan_CoThuLao_Thuong.aspx";           
            ViewData["PageLoad"] = 0;
            return View(sViewPath + "ReportView_NoMaster.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;
            String MaDV = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);           
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThangLuong"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNamLuong"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);            
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iNamLuong"] = Nam;
            ViewData["iThangLuong"] = Thang;
            ViewData["iID_MaDonVi"] = MaDV;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DSCaNhan_CoThuLao_Thuong.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        #region "Lấy dữ liệu"
        /// <summary>
        /// rptLuong_ThuLao_Thuong
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public static DataTable rptLuong_ThuLao_Thuong(String iID_MaBangLuong)
        {
            //String DKDuyet = TrangThai.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String SQL = String.Format(@"SELECT 
                                            L.sHoDem_CanBo +' '+ L.sTen_CanBo AS TenCB                                            
                                            ,L.sThuong_MoTa AS NoiDung
                                            ,SUM(L.rThuong) as rThuong
                                            ,l.iID_MaDonVi
                                            ,DV.TenHT
                                        FROM L_BangLuongChiTiet L
                                        INNER JOIN(SELECT DV.iID_MaDonVi,DV.iID_MaDonVi+'- '+DV.sTen AS TenHT FROM NS_DonVi AS DV WHERE DV.iTrangThai=1) DV
                                        ON DV.iID_MaDonVi=L.iID_MaDonVi
                                        WHERE iTrangThai=1                                            
                                            AND L.iID_MaBangLuong=@iID_MaBangLuong
                                            AND L.rThuong>0
                                        GROUP BY L.sTen_CanBo,L.sHoDem_CanBo,L.sThuong_MoTa,L.iID_MaDonVi,DV.TenHT                                        
                                        ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        #endregion

        #region "Tạo báo cáo"
        public ExcelFile CreateReport(String path, String iID_MaBangLuong)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_DSCaNhan_CoThuLao_Thuong");
            LoadData(fr, iID_MaBangLuong);
            //fr.SetValue("ThangNam", ("Tháng " + Thang + " / " + Nam));
            //fr.SetValue("MaDV", MaDV);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;             
        }
        #endregion

        #region "Đổ dữ liệu xuống file báo cáo"
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        private void LoadData(FlexCelReport fr, String iID_MaBangLuong)
        {
            DataTable data = rptLuong_ThuLao_Thuong(iID_MaBangLuong);          
            DataTable dtDonVi = HamChung.SelectDistinct("Donvi", data, "iID_MaDonVi", "iID_MaDonVi,TenHT");
            fr.AddTable("ChiTiet", data);
            fr.AddTable("Donvi", dtDonVi);
            dtDonVi.Dispose();
            data.Dispose();
        }
        #endregion
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaBangLuong)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaBangLuong);
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
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaBangLuong)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaBangLuong);
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
        public clsExcelResult ExportToExcel(String iID_MaBangLuong)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaBangLuong);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DSCaNhan_CoThuLao_Thuong.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy danh sách trạng thái duyệt
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTrangThai()
        {
            String SQL = String.Format(@"SELECT PH.iID_MaTrangThaiDuyet,PH.sTen
                                         FROM NS_PhanHe_TrangThaiDuyet AS PH
                                         WHERE PH.iTrangThai=1
                                           AND PH.iID_MaPhanHe=13
                                           AND PH.iID_MaTrangThaiDuyet=167");
            return Connection.GetDataTable(SQL);
        }
        /// <summary>
        /// Lấy danh sách đơn vị
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="Thang"></param>
        /// <returns></returns>
        public static DataTable Get_DonVi_(String Nam, String Thang)
        {
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT dv.iID_MaDonVi,dv.iID_MaDonVi+' - '+dv.sTen AS TenHT
                                        FROM NS_DonVi AS dv
                                        INNER JOIN (SELECT l.iID_MaDonVi FROM L_BangLuongChiTiet AS l
		                                            WHERE l.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
		                                            AND l.iNamBangLuong=@iNamBangLuong
		                                            AND l.iThangBangLuong=@iThangBangLuong
		                                            AND l.iTrangThai=1) l
                                        ON l.iID_MaDonVi=dv.iID_MaDonVi
                                        WHERE dv.iTrangThai=1
                                        GROUP BY dv.iID_MaDonVi,dv.sTen");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String NamLuong, String ThangLuong, String iID_MaDonVi)
        {
            return Json(obj_DSDonVi(ParentID, NamLuong, ThangLuong, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Ajax lấy đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLuong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String ParentID, String NamLuong, String ThangLuong, String iID_MaDonVi)
        {
            String dsDonVi = "";
            DataTable dtDonVi = Get_DonVi_(NamLuong, ThangLuong);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
    }
}