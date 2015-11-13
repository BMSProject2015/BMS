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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptTongHopQuanSoTangGiamTheoDonVi_48Controller : Controller
    {
        // Created: Huyền Lê
        // Edited: Thương
        // GET: /rptTongHopQuanSoTangGiamTheoDonVi_48/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptTongHopQuanSoTangGiamTheoDonVi_48.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["pageload"] = "0";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptTongHopQuanSoTangGiamTheoDonVi_48.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["pageload"] = "1";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang_Quy"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptTongHopQuanSoTangGiamTheoDonVi_48.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(3);
            string Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTongHopQuanSoTangGiamTheoDonVi_48");
            LoadData(fr, iThang, iID_MaTrangThaiDuyet,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("NgayThang", Ngay);
            fr.SetValue("cap1", BoQuocPhong);
            fr.SetValue("cap2", QuanKhu);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaTrangThaiDuyet);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuanSo_TangGiam.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iThang"></param>
        /// <param name="iNam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaTrangThaiDuyet);
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
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaTrangThaiDuyet,String MaND)
        {
            DataTable data = QuanSo_TangGiam(iThang, iID_MaTrangThaiDuyet,MaND);
            data.TableName = "ChiTiet";
            DataTable dtDonvi = HamChung.SelectDistinct("Donvi", data, "iID_MaDonVi", "iID_MaDonVi,sTen");
            fr.AddTable("Donvi", dtDonvi);
            fr.AddTable("ChiTiet", data);
            dtDonvi.Dispose();
            data.Dispose();
        }
        /// <summary>
        ///Lấy thông tin quân số tăng giảm trong tháng của đơn vị
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public DataTable QuanSo_TangGiam(String iThang, String iID_MaTrangThaiDuyet,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKThang = iThang.Equals("") ? "" : "AND QS.iThang_Quy=@iThang_Quy";
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT DV.sTen,sMoTa=CASE WHEN QS.sKyHieu=100 THEN N'Tháng trước' ELSE CASE WHEN QS.sKyHieu=2 THEN N'Tăng' 
                                        ELSE CASE WHEN QS.sKyHieu=3 THEN N'Giảm' 
                                        ELSE N'Quyết toán' END END END
                                        ,QS.sKyHieu,QS.iID_MaDonVi
                                        --SY QUAN
                                        ,SUM(QS.rThieuUy) rThieuUy,SUM(QS.rTrungUy) rTrungUy,SUM(QS.rThuongUy) rThuongUy,SUM(QS.rDaiUy) rDaiUy
                                        ,SUM(QS.rThieuTa) rThieuTa,SUM(QS.rTrungTa) rTrungTa,SUM(QS.rThuongTa) rThuongTa,SUM(QS.rDaiTa) rDaiTa
                                        ,SUM(QS.rTuong) rTuong
                                        --HA SY QUAN
                                        ,SUM(QS.rTSQ) rTSQ,SUM(QS.rBinhNhi) rBinhNhi,SUM(QS.rBinhNhat) rBinhNhat
                                        ,SUM(QS.rHaSi) rHaSi,SUM(QS.rThuongSi) rThuongSi,SUM(QS.rTrungSi) rTrungSi
                                        --QUAN NHAN CHUYEN NGHIEP
                                        ,SUM(QS.rQNCN) rQNCN,SUM(QS.rCNVQPCT) rCNVQPCT,SUM(QS.rQNVQPHD) rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet QS
                                        INNER JOIN (SELECT DV.iID_MaDonVi,DV.sTen
                                                    FROM NS_DonVi DV
                                                    WHERE DV.iTrangThai=1) DV
                                        ON DV.iID_MaDonVi=QS.iID_MaDonVi
                                        WHERE QS.iTrangThai=1
                                          AND QS.bLoaiThang_Quy=0
                                          {0}
                                          {1}
                                          {2}
                                          AND	QS.sKyHieu IN(100,2,700,3)
                                        GROUP BY QS.iID_MaDonVi,QS.sKyHieu,sMoTa,DV.sTen", DKThang,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            if (!String.IsNullOrEmpty(DKThang))
            {
                cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
               
            }
            DK = String.Format(" AND (QS.iNamLamViec={0} )", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
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