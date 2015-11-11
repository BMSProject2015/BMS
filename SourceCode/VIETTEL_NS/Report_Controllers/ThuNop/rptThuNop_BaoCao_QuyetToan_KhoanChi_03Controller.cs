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
using System;
using System.Text;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNop_BaoCao_QuyetToan_KhoanChi_03Controller : Controller
    {
        //
        // GET: /rptThuNop_BaoCao_QuyetToan_KhoanChi_03/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNop_BaoCao_QuyetToan_KhoanChi_03.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_BaoCao_QuyetToan_KhoanChi_03.aspx";
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
            String Nam = Request.Form[ParentID + "_iNam"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String MaDV = Request.Form["iID_MaDonVi"];
            return RedirectToAction("Index", new { Nam = Nam, MaDV = MaDV });
        }
        /// <summary>
        /// CreateReport
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String Nam, String MaDV)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_BaoCao_QuyetToan_KhoanChi_03");
            LoadData(fr, Nam, MaDV);                
            fr.SetValue("Nam", Nam);
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;            
        }        
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        private void LoadData(FlexCelReport fr, String Nam, String MaDV)
        {
            DataTable data = BaoCao_QT_KhoanChi_03(Nam, MaDV);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi,sTen");
            dtDonVi.TableName = "DonVi";
            fr.AddTable("DonVi", dtDonVi);
            dtDonVi.Dispose();
            data.Dispose();
        }
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Nam, String MaDV)
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
        public clsExcelResult ExportToExcel(String Nam, String MaDV)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Nam, MaDV);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCao_QuyetToan_KhoanChi_Nam_"+Nam+".xls";
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
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Nam, MaDV);
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
        /// BaoCao_QT_KhoanChi_03
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="MaDV"></param>
        /// <returns></returns>
        public static DataTable BaoCao_QT_KhoanChi_03(String Nam, String MaDV)
        {
            String _maDV = " TN.iID_MaDonVi IN (";
            String[] arrMaDV = MaDV.Split(',');
            for (int i = 0; i < arrMaDV.Length; i++)
            {
                _maDV += "@iID_MaDonVi" + i;
                if (i < arrMaDV.Length - 1)
                    _maDV += " , ";
            }
            _maDV += " ) ";
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT 
                                             SUM(TN.rSoXacNhan) as SoXN
                                            ,SUM(TN.rThucHien) as rThucHien
                                            ,SUM(TN.rDuToanDuocDuyet) as rDuToanDuocDuyet
                                            ,' - '+ TN.sMoTa as NoiDung
                                            ,TN.iID_MaDonVi,TN.iID_MaDonVi+'- '+DV.sTen as sTen                                 
                                        FROM TN_ChungTuChiTiet AS TN
                                        INNER JOIN (select dv.iID_MaDonVi,dv.sTen from NS_DonVi as dv where dv.iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)as DV
                                        ON DV.iID_MaDonVi=TN.iID_MaDonVi
                                        WHERE TN.iLoai=3
                                              AND TN.iTrangThai=1
                                              AND {0}
                                              AND TN.iNamLamViec=@iNamLamViec
                                              AND TN.bLaHangCha='0'
	                                         -- AND TN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY TN.sLNS,TN.sMoTa,DV.sTen,TN.iID_MaDonVi
                                        HAVING SUM(TN.rSoXacNhan)<>0
                                        OR SUM(TN.rThucHien)<>0 
                                        OR SUM(TN.rDuToanDuocDuyet)<>0
                                        ORDER BY TN.iID_MaDonVi,TN.sLNS
                                        ", _maDV);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            for (int i = 0; i < arrMaDV.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrMaDV[i]);
            }
         //   cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="arrDV"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ds_DonVi(String NamLamViec, String[] arrDV)
        {
            return Json(obj_DSDonVi(NamLamViec, arrDV), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Hiển thị danh sách đơn vị có dư liệu lên màn hình
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="arrDV"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String NamLamViec, String[] arrDV)
        {
            DataTable dt = GetDonVi(NamLamViec);
            String stbDonVi = "<table class=\"mGrid\">";
            String TenDV = ""; String idDV = "";
            String _Checked1 = "checked=\"checked\"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Checked1 = "";
                TenDV = Convert.ToString(dt.Rows[i]["TenHT"]);
                idDV = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < arrDV.Length; j++)
                {
                    if (idDV == arrDV[j])
                    {
                        _Checked1 = "checked=\"checked\"";
                        break;
                    }
                }
                stbDonVi += "<tr style=\" height: 20px; font-size: 12px; \"><td style=\"width: 20px; text-align:center; height:auto; line-height:7px;\">";
                stbDonVi += "<input type=\"checkbox\" value=\"" + idDV + "\"" + _Checked1 + " check-group=\"iID_MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" style=\"cursor:pointer;\" />";
                stbDonVi += "</td><td>" + TenDV + "</td></tr>";
            }
            stbDonVi += "</table>";
            dt.Dispose();
            return stbDonVi;
        }
        /// <summary>
        /// Lấy danh sách đơn vị có dữ liệu
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public static DataTable GetDonVi(String Nam)
        {
            String SQL = String.Format(@"SELECT TN.iID_MaDonVi,TN.iID_MaDonVi+'- '+DV.sTen AS TenHT
                                            FROM TN_ChungTuChiTiet AS TN
                                            INNER JOIN(SELECT DV.iID_MaDonVi,DV.sTen FROM NS_DonVi AS DV WHERE DV.iTrangThai=1 AND DV.iNamLamViec_DonVi=@iNamLamViec) DV
                                            ON DV.iID_MaDonVi=TN.iID_MaDonVi
                                            WHERE TN.iTrangThai=1
	                                                AND TN.iLoai=3
	                                                AND TN.iNamLamViec=@iNamLamViec
	                                               -- AND TN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                            GROUP BY TN.iID_MaDonVi,DV.sTen"); ;
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
         //   cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}