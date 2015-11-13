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

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanChiNganSachQuocPhongNamTuyVien_DonViController : Controller
    {
        //
        // GET: /rptDuToanChiNganSachQuocPhongNamTuyVien_DonVi/

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/DuToanChiNganSachQuocPhongNamTuyVien_DonVi.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDuToanChiNganSachQuocPhongNamTuyVien_DonVi.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
           
            return RedirectToAction("Index", new {iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            XlsFile Result= new XlsFile(true);
            Result.Open(path);
            String TenDv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable Ten = TenDonVi(iID_MaDonVi);
          
            if (Ten.Rows.Count > 0)
            {
                TenDv = Ten.Rows[0][0].ToString();
            }
           
             FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanChiNganSachQuocPhongNamTuyVien_DonVi");
            LoadData(fr, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam",iNamLamViec);
                fr.SetValue("_iID_MaDonVi", iID_MaDonVi);
                fr.SetValue("TenDV", TenDv);            
                fr.SetValue("ngay", ngay);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable data = DT_ChiNganSachQuocPhongNamTuyVien_DonVi(MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            data.Dispose();
            dtMuc.Dispose();

            dtTieuMuc.Dispose();             
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
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
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
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
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DSDonVi(ParentID, MaND, iID_MaDonVi,iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            String dsDonVi = "";
            DataTable dtDonVi = DanhSachDonVi(MaND,iID_MaTrangThaiDuyet);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="iID_MaDonVi">Đơn vị</param>
        /// <returns></returns>
        public static DataTable DT_ChiNganSachQuocPhongNamTuyVien_DonVi(String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,SUM(rHienVat) rHienVat, SUM(rTuchi) rTuchi
                                        FROM DT_ChungTuChiTiet 
                                        WHERE sLNS='1020200' AND sL='460' AND sK='468' AND iTrangThai= 1 AND iID_MaDonVi=@iID_MaDonVi {1}
                                        {0}
                                        GROUP BY sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa
                                        ORDER BY sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa", ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTCNS_QP_Nam_TuyVien_DonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String MaND, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen
                                        FROM ( SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1 {0} {1} AND sLNS='1020200') a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1) as b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);

            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
    }
}