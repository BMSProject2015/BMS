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
    public class rptLuong_DSCaNhan_NopThueDauVaoController : Controller
    {
        // Edit: Le Van Thuong
        // Edit date: 07-07-2012
        // GET: /rptLuong_DSCaNhan_NopThueDauVao/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_DSCaNhan_NopThueDauVao.xls";
        public static String NameFile = "";        
        public ActionResult Index()
        {            
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DSCaNhan_NopThueDauVao.aspx";
            ViewData["PageLoad"] = 0;
            return View(sViewPath + "ReportView_NoMaster.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
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
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DSCaNhan_NopThueDauVao.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>        
        public static DataTable rptLuong_NopThueDauVao(String iMaBL)
        {            
            String SQL = String.Format(@"SELECT L.sHoDem_CanBo+' '+L.sTen_CanBo As TenCB
                                               ,L.sNopThueDauVao_MoTa AS NoiDung
                                               ,SUM(L.rNopThueDauVao) AS SoTien
                                               ,L.iID_MaDonVi
                                               ,L.sTenDonVi AS TenHT
                                        FROM L_BangLuongChiTiet L                               
                                        WHERE L.iTrangThai=1
                                          AND L.iID_MaBangLuong=@iID_MaBangLuong                              
                                        GROUP BY L.sTen_CanBo,L.sHoDem_CanBo,L.sNopThueDauVao_MoTa,L.iID_MaDonVi,L.sTenDonVi
                                        HAVING SUM(L.rNopThueDauVao)>0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iMaBL);           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }        
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        
        public ExcelFile CreateReport(String path, String iMaBL)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_DSCaNhan_NopThueDauVao");         
            LoadData(fr, iMaBL);                      
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;           
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iMaBL"></param>
     
        private void LoadData(FlexCelReport fr, String iMaBL)
        {
            DataTable data = rptLuong_NopThueDauVao(iMaBL);
            data.TableName = "ChiTiet";
            DataTable dtDonVi = HamChung.SelectDistinct("Donvi", data, "iID_MaDonVi", "iID_MaDonVi,TenHT");
            fr.AddTable("ChiTiet", data);
            fr.AddTable("Donvi", dtDonVi);
            dtDonVi.Dispose();
            data.Dispose();
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="iMaBL"></param>
        /// <returns></returns>
        
        public clsExcelResult ExportToPDF(String iMaBL)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iMaBL);
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
        public ActionResult ViewPDF(String iMaBL)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iMaBL);
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
        public clsExcelResult ExportToExcel(String iMaBL)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),iMaBL);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DSCaNhan_NopThueDauVao.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang"></param>
        /// <returns></returns>
        public static DataTable Get_DonVi_(String Nam, String Thang,String TrangThai)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : "AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT L.iID_MaDonVi,L.sTenDonVi AS TenHT
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
	                                        AND L.iNamBangLuong=@iNamBangLuong
	                                        AND L.iThangBangLuong=@iThangBangLuong
	                                        {0}
                                        GROUP BY L.iID_MaDonVi,L.sTenDonVi",DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            int tt=LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String NamLuong,String ThangLuong,String TrangThai, String iID_MaDonVi)
        {
            return Json(obj_DSDonVi(ParentID, NamLuong,ThangLuong,TrangThai, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Ajax lấy đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLuong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String ParentID, String NamLuong, String ThangLuong,String TrangThai,String iID_MaDonVi)
        {
            String dsDonVi = "";
            DataTable dtDonVi = Get_DonVi_(NamLuong,ThangLuong,TrangThai);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }        
    }
}