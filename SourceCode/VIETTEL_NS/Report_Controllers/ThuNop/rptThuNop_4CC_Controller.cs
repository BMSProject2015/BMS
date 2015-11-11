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
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNop_4CC_Controller : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNo_4CC.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_4CC.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable rptThuNop_4CC_(String NamLamViec, String iID_MaDonVi)
        {
            String SQL = String.Format(@"SELECT B2.sTen,  B1.sLNS, SUBSTRING(B1.sLNS,1,3) as Cha, SUBSTRING(B1.sLNS,1,5) as Con, B1.sMoTa, B1.TongTien, B3.iID_MaDanhMuc FROM
                                    (SELECT DISTINCT sLNS, TN_ChungTuChiTiet.sMoTa, rNopNSQP AS TongTien,iID_MaDonVi FROM TN_ChungTuChiTiet WHERE SUBSTRING(TN_ChungTuChiTiet.sLNS,1,1)='8' AND iLoai=1 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec)B1
                                    INNER JOIN
                                    (SELECT DISTINCT iID_MaDonVi,iID_MaKhoiDonVi, sTen FROM NS_DonVi WHERE  iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaKhoiDonVi='687860da-8dc1-4bca-b810-5cc7d6634846' OR iID_MaKhoiDonVi='03DE0E56-E874-4320-A01B-F524BEB47B71') B2
                                    ON B1.iID_MaDonVi=B2.iID_MaDonVi INNER JOIN 
                                    (SELECT DISTINCT iID_MaDanhMuc FROM DC_DanhMuc) B3
                                    ON B2.iID_MaKhoiDonVi=B3.iID_MaDanhMuc ORDER BY B1.sLNS
                                    ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable _rptThuNop_4CC_(String NamLamViec, String iID_MaDonVi)
        {
            String SQL = String.Format(@"SELECT 
                                            DKCap1=SUBSTRING(TN.sLNS,1,1),
                                            DKCap2=case when LEN(SUBSTRING(TN.sLNS,1,3))=3 then SUBSTRING(TN.sLNS,1,3) else '' end,
                                            DKCap3=case when LEN(SUBSTRING(TN.sLNS,1,5))=5 then SUBSTRING(TN.sLNS,1,5) else '' end,
                                            DKCap4=case when LEN(SUBSTRING(TN.sLNS,1,7))=7 then SUBSTRING(TN.sLNS,1,7) else '' end,
                                            DKCap5=case when (LEN(SUBSTRING(TN.sLNS,1,7))=7 AND TN.sL<>'') then SUBSTRING(TN.sLNS,1,7) else '' end,
                                            TN.sMoTa
                                            ,SUM(CASE WHEN (PB.sTen<>N'B6') THEN TN.rNopNSQP ELSE 0 END) DT
                                            ,SUM(CASE WHEN (PB.sTen=N'B6') THEN TN.rNopNSQP ELSE 0 END) DN    
                                        FROM TN_ChungTuChiTiet AS TN
                                        INNER JOIN NS_PhongBan AS PB
                                        ON PB.iID_MaPhongBan=TN.iID_MaPhongBan
                                        WHERE TN.iTrangThai=1
                                              AND TN.iNamLamViec=@iNamLamViec
                                              AND TN.iID_MaDonVi=@iID_MaDonVi
                                              AND SUBSTRING(TN.sLNS,1,1)='8'
                                              AND TN.rNopNSQP<>0
                                              --AND TN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY TN.sMoTa,TN.sLNS,TN.sL
                                        
                                        ORDER BY TN.sLNS");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
           // int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach);
           // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
          
            return RedirectToAction("Index", new { NamLamViec = NamLamViec, iID_MaDonVi = iID_MaDonVi});
        }
        /// <summary>
        /// Lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String iID_MaDonVi)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String tendv = "";
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count>0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_4CC_");
            LoadData(fr, NamLamViec, iID_MaDonVi);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("TenDV", tendv);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec, String iID_MaDonVi)
        {
            DataTable data = _rptThuNop_4CC_(NamLamViec, iID_MaDonVi);
            DataTable dtCap_4 = HamChung.SelectDistinct("dtCap4", data, "DKCap1,DKCap2,DKCap3,DKCap4", "DKCap1,DKCap2,DKCap3,DKCap4,sMoTa");
            SplitNULL(data, "DKCap1");
            SplitNULL(data, "DKCap2");
            SplitNULL(data, "DKCap3");
            SplitNULL(data, "DKCap4");
            SplitNULL(data, "DKCap5");
            DataTable dtCap_3 = HamChung.SelectDistinct("dtCap3", dtCap_4, "DKCap1,DKCap2,DKCap3", "DKCap1,DKCap2,DKCap3,sMoTa");            
            SplitNULL(dtCap_4, "DKCap2");
            SplitNULL(dtCap_4, "DKCap3");
            SplitNULL(dtCap_4, "DKCap4");
            DataTable dtCap_2 = HamChung.SelectDistinct("dtCap2", dtCap_3, "DKCap1,DKCap2", "DKCap1,DKCap2,sMoTa");
            SplitNULL(dtCap_3, "DKCap2");
            SplitNULL(dtCap_3, "DKCap3");
            DataTable dtCap_1 = HamChung.SelectDistinct("dtCap1", dtCap_2, "DKCap1", "DKCap1,sMoTa");
            SplitNULL(dtCap_2, "DKCap2");
            if (dtCap_3.Rows.Count > 0)
            {
                for (int i = 0; i < dtCap_3.Rows.Count; i++)
                {
                    DataRow dr3 = dtCap_3.Rows[i];
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        DataRow dr = data.Rows[j];
                        if (dr3["sMoTa"].Equals(dr["sMoTa"]) && dr3["DKCap3"].Equals(dr["DKCap3"]) && dr3["DKCap2"].Equals(dr["DKCap2"]) && dr3["DKCap1"].Equals(dr["DKCap1"]))
                        {
                            dr3["sMoTa"] = "";
                            break;
                        }
                    }
                }
            }
            fr.AddTable("dtCap4", dtCap_4);
            fr.AddTable("dtCap3", dtCap_3);
            fr.AddTable("dtCap2", dtCap_2);
            fr.AddTable("dtCap1", dtCap_1);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            dtCap_1.Dispose();
            dtCap_2.Dispose();
            dtCap_3.Dispose();
            dtCap_4.Dispose();
            data.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt2"></param>
        private static void SplitNULL(DataTable dt2, String DKCap)
        {
            if (dt2.Rows.Count > 0)
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (Convert.ToString(dt2.Rows[i][DKCap]) == "")
                    {
                        dt2.Rows.RemoveAt(i);
                        if (i == 0)
                            i = 0;
                        else
                            i = i - 1;
                    }
                }
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec, String iID_MaDonVi)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, iID_MaDonVi);
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
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String iID_MaDonVi)
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, iID_MaDonVi);
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
        public static DataTable Get_DonVi()
        {
            String SQL = "SELECT DISTINCT iID_MaDonVi, sTen FROM NS_DonVi";
            DataTable dt = Connection.GetDataTable(SQL);
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
						                                            AND TN.iNamLamViec=@iNamLamViec
						                                          --  AND TN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
						                                            GROUP BY TN.iID_MaDonVi ) AND iNamLamViec_DonVi=@iNamLamViec
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
        /// <summary>
        /// Xuát ra Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String iID_MaDonVi)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, iID_MaDonVi);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThuNop_DTNS_Nam_"+NamLamViec+".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
    }
}