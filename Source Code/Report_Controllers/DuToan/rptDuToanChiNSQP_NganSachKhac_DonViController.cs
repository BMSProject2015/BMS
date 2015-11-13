using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlexCel.Render;
using FlexCel.Core;
using FlexCel.Report;
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
    public class rptDuToanChiNSQP_NganSachKhac_DonViController : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToanChiNSQP_NganSachKhac_DonVi.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDuToanChiNSQP_NganSachKhac_DonVi.aspx";
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
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS)
        {
            String MaND = User.Identity.Name;
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            XlsFile Result = new XlsFile(true);
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
            Ten.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanChiNSQP_NganSachKhac_DonVi");
            LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi, sLNS, MaND);
            fr.SetValue("iID_MaDonVi", iID_MaDonVi);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("TenDV", TenDv);
            fr.SetValue("ngay", ngay);
            fr.SetValue("LNS", sLNS);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// onnchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi, sLNS,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS,String MaND)
        {
            String dsDonVi = "";
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, sLNS,MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }

        public static DataTable DanhSachDonVi(String iID_MaTrangThaiDuyet,String sLNS, String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT
                                        FROM ( SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1  {0}  {1} AND sLNS=@sLNS) a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1) as b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi",ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["TenHT"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi, sLNS);
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
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi, sLNS);
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
        /// Khởi tạo range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS, String MaND)
        {
            DataTable data = DT_ChiNganSachQuocPhong_NganSachKhac_DonVi(iID_MaTrangThaiDuyet, iID_MaDonVi, sLNS, MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLNS;
            dtLNS = HamChung.SelectDistinct("LNS", dtMuc, "sLNS", "sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);
            data.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }

        /// <summary>
        /// DT_ChiNganSachQuocPhong_NganSachKhac_DonVi
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable DT_ChiNganSachQuocPhong_NganSachKhac_DonVi(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS, String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa, SUM(rTongSo) as rTongSo                                            
                                        FROM DT_ChungTuChiTiet 
                                        WHERE sLNS=@sLNS AND iTrangThai = 1 AND iID_MaDonVi=@iID_MaDonVi {0} {1}
                                        GROUP BY sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa
                                        ORDER BY sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String sLNS)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi, sLNS);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DT_CNSQP_NganSachKhac_DonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

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