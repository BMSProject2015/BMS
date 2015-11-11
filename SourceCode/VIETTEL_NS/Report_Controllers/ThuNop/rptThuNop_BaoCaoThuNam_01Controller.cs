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

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNop_BaoCaoThuNam_01Controller : Controller
    {
        //Edit: 06-07-2012
        //User:
        // GET: /rptThuNop_BaoCaoThuNam_01/
        
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNop_BAOCAO_THUNAM_01.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_BaoCaoThuNam_01.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Nam = Request.Form[ParentID + "_iNamLamViec"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String MaDV = Request.Form[ParentID + "_iID_MaDonVi"];

            return RedirectToAction("Index", new { Nam = Nam, MaDV = MaDV });
        }
        /// <summary>
        /// CreateReport
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String Nam,String MaDV)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_BaoCaoThuNam_01");
            LoadData(fr, Nam, MaDV);
            fr.SetValue("TenDV", DonViModels.Get_TenDonVi(MaDV));
            fr.SetValue("Nam", Nam);
            fr.SetValue("NgayThang",ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
            
        }
        /// <summary>
        /// Lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static DataTable tendonvi()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT NS_DonVi.sTen,TN_ChungTuChiTiet.iID_MaDonVi
                                                FROM TN_ChungTuChiTiet
                                                INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as  NS_DonVi on  TN_ChungTuChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                                WHERE 
                                                    --TN_ChungTuChiTiet.iLoai=2 AND
                                                    NS_DonVi.iTrangThai=1
                                                ORDER BY iID_MaDonVi");
            //cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        private void LoadData(FlexCelReport fr, String Nam, String MaDV)
        {
            DataTable data = BaoCao_ThuNam_01(Nam,MaDV);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);    
        }
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Nam,String MaDV)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Nam, MaDV);
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
        /// ExportToExcel
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Nam,String MaDV)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Nam,MaDV);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCao_ThuNam_01.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Nam, String MaDV)
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Nam,MaDV);
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
        /// BaoCao_ThuNam_01
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public static DataTable BaoCao_ThuNam_01(String Nam, String MaDV)
        {
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT
	                                        cast((ROW_NUMBER() OVER (ORDER BY ThuNop.sLNS)) as varchar)+'- '+ ThuNop.sMoTa as sMoTa
	                                        ,SUM(ThuNop.rKhauHaoTSCD) rKhauHaoTSCD
	                                        ,SUM(ThuNop.rTienLuong) rTienLuong 
	                                        ,SUM(ThuNop.rQTNSKhac) rQTNSKhac
	                                        ,SUM(ThuNop.rChiPhiKhac) rChiPhiKhac
	                                        ,SUM(ThuNop.rNopNSQP) rNopNSQP
	                                        ,SUM(ThuNop.rNopCapTren) rNopCapTren
	                                        ,SUM(ThuNop.rBoSungKinhPhi) rBoSungKinhPhi
	                                        ,SUM(ThuNop.rTrichQuyDonVi) rTrichQuyDonVi
	                                        ,SUM(ThuNop.rSoChuaPhanPhoi) rSoChuaPhanPhoi
	                                        ,SUM(ThuNop.rChenhLech) rChenhLech
	                                        ,SUM(ThuNop.rTongThu) rTongThu
                                        FROM TN_ChungTuChiTiet as ThuNop
                                        WHERE ThuNop.iLoai=2
		                                        AND ThuNop.iID_MaDonVi=@iID_MaDonVi
		                                        AND ThuNop.iNamLamViec=@iNamLamViec
		                                        AND ThuNop.iTrangThai=1
		                                        AND ThuNop.bLaHangCha='0'
		                                        --AND ThuNop.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        
                                        GROUP BY ThuNop.sMoTa,ThuNop.sLNS

                                         HAVING SUM(ThuNop.rKhauHaoTSCD)<>0  
												OR SUM(ThuNop.rTienLuong)<>0
	                                        OR SUM(ThuNop.rQTNSKhac) <>0
	                                        OR SUM(ThuNop.rChiPhiKhac) <>0
	                                        OR SUM(ThuNop.rNopNSQP) <>0
	                                        OR SUM(ThuNop.rNopCapTren) <>0
	                                        OR SUM(ThuNop.rBoSungKinhPhi) <>0
	                                        OR SUM(ThuNop.rTrichQuyDonVi) <>0
	                                        OR SUM(ThuNop.rSoChuaPhanPhoi) <>0
	                                        OR SUM(ThuNop.rChenhLech) <>0
	                                        OR SUM(ThuNop.rTongThu) <>0
                                        ");
            SqlCommand cmd = new SqlCommand(SQL);          
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDV);
           // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String NamLamViec, String iID_MaDonVi)
        {
            return Json(obj_DSDonVi(ParentID, NamLamViec, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Load đơn vị theo năm và mã loại ngân sách
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String ParentID, String NamLamViec, String iID_MaDonVi)
        {
            String dsDonVi = "";
            DataTable dtDonVi = DonVi(NamLamViec);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        /// <summary>
        /// Lấy danh sách đơn vị có dư liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DonVi(String NamLamViec)
        {
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi, DV.iID_MaDonVi+' - '+DV.sTen AS TenHT
                                         FROM NS_DonVi AS DV
                                         WHERE DV.iID_MaDonVi IN (
						                                            SELECT TN.iID_MaDonVi
						                                            FROM TN_ChungTuChiTiet AS TN 
						                                            WHERE TN.iTrangThai=1 
                                                                            AND TN.iLoai=2
						                                                    AND TN.iNamLamViec=@iNamLamViec
						                                                    --AND TN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
						                                            GROUP BY TN.iID_MaDonVi) AND iNamLamViec_DonVi=@iNamLamViec
						                                            "
                                        );
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
           // int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach);
           // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonVi;
        }
    }
}