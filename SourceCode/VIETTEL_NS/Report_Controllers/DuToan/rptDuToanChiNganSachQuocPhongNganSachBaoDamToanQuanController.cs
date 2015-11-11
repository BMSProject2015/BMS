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

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuanController : Controller
    {
        //
        // GET: /rptDuToanChiNganSachQuocPhong/        
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan.xls";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptrptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["sNG"] = "";
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
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String sNG = Request.Form[ParentID + "_sNG"];
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sNG = sNG });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String sNG)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan");
            LoadData(fr, iID_MaTrangThaiDuyet, sNG,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;            
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public DataTable DT_DuToanChiNganSachQuocPhong(String iID_MaTrangThaiDuyet, String sNG,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = "SELECT DM.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa,";
            SQL += "rTongSo1=SUM(rTonKho+rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong)";
            SQL += ",Sum(rTonKho) as rTonKho,";
            SQL += "rTongSo2=SUM(rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "Sum(rHangNhap) as rHangNhap,";
            SQL += "rTongSo3=SUM(rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "SUM(rTuChi) as rTuChi,SUM(rHangMua) as rHangMua,SUM(rPhanCap) as rPhanCap,SUM(rDuPhong) as rDuPhong";
            SQL += " FROM (SELECT  iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,";
            SQL += " rTongSo1=SUM(rTonKho+rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong)";
            SQL += ",Sum(rTonKho) as rTonKho,";
            SQL += "rTongSo2=SUM(rHangNhap+rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "Sum(rHangNhap) as rHangNhap,";
            SQL += "rTongSo3=SUM(rTuChi+rHangMua+rPhanCap+rDuPhong),";
            SQL += "SUM(rTuChi) as rTuChi,SUM(rHangMua) as rHangMua,SUM(rPhanCap) as rPhanCap,SUM(rDuPhong) as rDuPhong";
            SQL += " FROM DT_ChungTuChiTiet WHERE sLNS='1040100' AND sL='460' AND sK='468' AND bChiNganSach=1 AND ";
            SQL += " iTrangThai=1   {0} {1} and sNG IN (SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@Nganh)";
            SQL += " Group BY  iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTonKho,rHangNhap";
            SQL += ") ChiTiet";
            SQL += " INNER JOIN NS_DonVi ON ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi ";
            SQL += " INNER JOIN (SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang)) DM ";
            SQL += " ON NS_DonVi.iID_MaKhoiDonVi=DM.iID_MaDanhMuc";
            SQL += " GROUP By DM.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SQL = string.Format(SQL, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@Nganh", sNG);
            cmd.Parameters.AddWithValue("@sTenBang", "KhoiDonVi");
            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo 
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String sNG,String MaND)
        {
            DataTable data = DT_DuToanChiNganSachQuocPhong(iID_MaTrangThaiDuyet, sNG,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sTM", "sM,sTM,sMoTa", "sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sM", "sM,sMoTa","sM,sTM");
            fr.AddTable("Muc", dtMuc);
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String sNG)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sNG);
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
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String sNG)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sNG);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTCNS_QP_NS_ToanQuan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String sNG)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sNG);
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
        public ActionResult DanhSach()
        {
            return View(sViewPath + "ReportList.aspx");
        }

        public ActionResult XDCB()
        {
            return View(sViewPath + "rptChiNganSachQuocPhong_XDCB.aspx");
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