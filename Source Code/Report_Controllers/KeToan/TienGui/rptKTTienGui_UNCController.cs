using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.NhanSu
{
    public class rptKTTienGui_UNCController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_UNC.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTienGui_UNC.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["PageLoad"] = "0";
            ViewData["DoiTuong"] = "0";
            ViewData["dTuNgayGhiSo"] = "1";
            ViewData["dDenNgayGhiSo"] = "1";
            ViewData["dTuThangGhiSo"] = "1";
            ViewData["dDenThangGhiSo"] = DateTime.Now.Month;
            ViewData["dTuNgayPS"] = "";
            ViewData["dDenNgayPS"] = "";
            ViewData["dTuThangPS"] = "";
            ViewData["dDenThangPS"] = "";
            ViewData["sTKDoiUng"] = "";
            ViewData["sDVNo"] = "";
            ViewData["sDVCo"] = "";
            ViewData["sDVNhan"] = "";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String DoiTuong = Convert.ToString(Request.Form[ParentID + "_DoiTuong"]);
            String dTuNgayGhiSo = Convert.ToString(Request.Form[ParentID + "_dTuNgayGhiSo"]);
            String dDenNgayGhiSo = Convert.ToString(Request.Form[ParentID + "_dDenNgayGhiSo"]);
            String dTuNgayPS = Convert.ToString(Request.Form[ParentID + "_dTuNgayPS"]);

            String dDenNgayPS = Convert.ToString(Request.Form[ParentID + "_dDenNgayPS"]);
            String sTKDoiUng = Convert.ToString(Request.Form[ParentID + "_sTKDoiUng"]);
            String sDVNo = Convert.ToString(Request.Form[ParentID + "_sDVNo"]);
            String sDVCo = Convert.ToString(Request.Form[ParentID + "_sDVCo"]);
            String sDVNhan = Convert.ToString(Request.Form[ParentID + "_sDVNhan"]);

            String dTuThangGhiSo = Convert.ToString(Request.Form[ParentID + "_dTuThangGhiSo"]);
            String dDenThangGhiSo = Convert.ToString(Request.Form[ParentID + "_dDenThangGhiSo"]);
            String dTuThangPS = Convert.ToString(Request.Form[ParentID + "_dTuThangPS"]);
            String dDenThangPS = Convert.ToString(Request.Form[ParentID + "_dDenThangPS"]);


            ViewData["PageLoad"] = "1";
            ViewData["DoiTuong"] = DoiTuong;
            ViewData["dTuNgayGhiSo"] = dTuNgayGhiSo;
            ViewData["dDenNgayGhiSo"] = dDenNgayGhiSo;
            ViewData["dTuNgayPS"] = dTuNgayPS;
            ViewData["dDenNgayPS"] = dDenNgayPS;
            ViewData["sTKDoiUng"] = sTKDoiUng;
            ViewData["sDVNo"] = sDVNo;
            ViewData["sDVCo"] = sDVCo;
            ViewData["sDVNhan"] = sDVNhan;

            ViewData["dTuThangGhiSo"] = dTuThangGhiSo;
            ViewData["dDenThangGhiSo"] = dDenThangGhiSo;
            ViewData["dTuThangPS"] = dTuThangPS;
            ViewData["dDenThangPS"] = dDenThangPS;

            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTienGui_UNC.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tao báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="DoiTuong"></param>
        /// <param name="dTuNgayGhiSo"></param>
        /// <param name="dDenNgayGhiSo"></param>
        /// <param name="dTuNgayPS"></param>
        /// <param name="dDenNgayPS"></param>
        /// <param name="sTKDoiUng"></param>
        /// <param name="sDVNo"></param>
        /// <param name="sDVCo"></param>
        /// <param name="sDVNhan"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String DoiTuong, String dTuNgayGhiSo, String dDenNgayGhiSo, String dTuNgayPS,
                String dDenNgayPS, String sTKDoiUng, String sDVNo, String sDVCo, String sDVNhan, String dTuThangGhiSo, 
            String dDenThangGhiSo, String dTuThangPS, String dDenThangPS)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();

            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTienGui_UNC");

            LoadData(fr, DoiTuong, dTuNgayGhiSo, dDenNgayGhiSo, dTuNgayPS,
                dDenNgayPS, sTKDoiUng, sDVNo, sDVCo, sDVNhan, dTuThangGhiSo, dDenThangGhiSo, dTuThangPS, dDenThangPS);
            fr.SetValue("TaiKhoan", KT_TaiKhoan_Get_Ten(DoiTuong));
            fr.SetValue("Ngay", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);
            string sDKLoc = "ĐK lọc Ghi sổ từ " + dTuNgayGhiSo + "/" + dTuThangGhiSo + " đến " + dDenNgayGhiSo +
                            "/" + dDenThangGhiSo;

            if (dTuNgayPS != null && !dTuNgayPS.Trim().Equals("")
                && dDenNgayPS != null && !dDenNgayPS.Trim().Equals("")
                && dDenThangPS != null && !dDenThangPS.Trim().Equals("")
                && dTuThangPS != null && !dTuThangPS.Trim().Equals(""))
                sDKLoc += " PS từ " + dTuNgayPS + "/" + dTuThangPS + " đến " + dDenNgayPS + "/" + dDenThangPS;
            sDKLoc += " TK TG " + DoiTuong;
            fr.SetValue("DKLoc", sDKLoc);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="DoiTuong"></param>
        /// <param name="dTuNgayGhiSo"></param>
        /// <param name="dDenNgayGhiSo"></param>
        /// <param name="dTuNgayPS"></param>
        /// <param name="dDenNgayPS"></param>
        /// <param name="sTKDoiUng"></param>
        /// <param name="sDVNo"></param>
        /// <param name="sDVCo"></param>
        /// <param name="sDVNhan"></param>
        private void LoadData(FlexCelReport fr, String DoiTuong, String dTuNgayGhiSo, String dDenNgayGhiSo, String dTuNgayPS,
                String dDenNgayPS, String sTKDoiUng, String sDVNo, String sDVCo, String sDVNhan,
            String dTuThangGhiSo, String dDenThangGhiSo, String dTuThangPS, String dDenThangPS)
        {

            DataTable data = LayDanhSach(DoiTuong, dTuNgayGhiSo, dDenNgayGhiSo, dTuNgayPS,
                dDenNgayPS, sTKDoiUng, sDVNo, sDVCo, sDVNhan, dTuThangGhiSo, dDenThangGhiSo, dTuThangPS, dDenThangPS);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String DoiTuong, String dTuNgayGhiSo, String dDenNgayGhiSo, String dTuNgayPS,
                String dDenNgayPS, String sTKDoiUng, String sDVNo, String sDVCo, String sDVNhan
            , String dTuThangGhiSo, String dDenThangGhiSo, String dTuThangPS, String dDenThangPS)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DoiTuong, dTuNgayGhiSo, dDenNgayGhiSo, dTuNgayPS,
                dDenNgayPS, sTKDoiUng, sDVNo, sDVCo, sDVNhan, dTuThangGhiSo, dDenThangGhiSo, dTuThangPS, dDenThangPS);
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
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String DoiTuong, String dTuNgayGhiSo, String dDenNgayGhiSo, String dTuNgayPS,
                String dDenNgayPS, String sTKDoiUng, String sDVNo, String sDVCo, String sDVNhan
            , String dTuThangGhiSo, String dDenThangGhiSo, String dTuThangPS, String dDenThangPS)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DoiTuong, dTuNgayGhiSo, dDenNgayGhiSo, dTuNgayPS,
                dDenNgayPS, sTKDoiUng, sDVNo, sDVCo, sDVNhan, dTuThangGhiSo, dDenThangGhiSo, dTuThangPS, dDenThangPS);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTienGui_UNC.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String DoiTuong, String dTuNgayGhiSo, String dDenNgayGhiSo, String dTuNgayPS,
                String dDenNgayPS, String sTKDoiUng, String sDVNo, String sDVCo, String sDVNhan, String dTuThangGhiSo, String dDenThangGhiSo, String dTuThangPS, String dDenThangPS)
        {
            //ViewData["bBaoCaoTH"] = "False";
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DoiTuong, dTuNgayGhiSo, dDenNgayGhiSo, dTuNgayPS,
                dDenNgayPS, sTKDoiUng, sDVNo, sDVCo, sDVNhan, dTuThangGhiSo, dDenThangGhiSo, dTuThangPS, dDenThangPS);
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
        /// Data của báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="bThang"></param>
        /// <returns></returns>
        public DataTable LayDanhSach(String DoiTuong, String dTuNgayGhiSo, String dDenNgayGhiSo, String dTuNgayPS,
                String dDenNgayPS, String sTKDoiUng, String sDVNo, String sDVCo, String sDVNhan,
            String dTuThangGhiSo, String dDenThangGhiSo, String dTuThangPS, String dDenThangPS)
        {
            DataTable dtDoiTuong = new DataTable();
            
            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT sSoChungTuGhiSo,iNgay,sSoChungTuChiTiet,iNgayCT,sNoiDung,iID_MaDonVi_No,iID_MaDonVi_Co,iID_MaDonVi_Nhan," +
                " TKDoiUng , TKDoiChieu ,rSoTien" +
                " FROM (Select *," +
                " CASE WHEN iID_MaTaiKhoan_Co <> @DoiTuong THEN iID_MaTaiKhoan_Co ELSE iID_MaTaiKhoan_No END AS TKDoiUng , @DoiTuong as TKDoiChieu" +
                " from KTTG_ChungTuChiTiet WHERE iID_MaTaiKhoan_No = @DoiTuong OR iID_MaTaiKhoan_Co = @DoiTuong) as  KTTG_ChungTuChiTiet" +
                " WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet " +
                " AND iNamLamViec = @TuNam AND (iThang>@TuThang or(iThang = @TuThang and iNgay >= @TuNgay)) " +
                " AND (iThang<@DenThang or(iThang = @DenThang and iNgay <= @DenNgay)) ";

            if (dTuNgayPS != null && !dTuNgayPS.Trim().Equals("") 
                && dDenNgayPS != null && !dDenNgayPS.Trim().Equals("")
                && dDenThangPS != null && !dDenThangPS.Trim().Equals("")
                && dTuThangPS != null && !dTuThangPS.Trim().Equals(""))
                SQLDSNS += " AND (iThangCT>@TuThangPS or(iThangCT = @TuThangPS and iNgayCT >= @TuNgayPS))";
            if (dTuNgayPS != null && !dTuNgayPS.Trim().Equals("")
                && dDenNgayPS != null && !dDenNgayPS.Trim().Equals("")
                && dDenThangPS != null && !dDenThangPS.Trim().Equals("")
                && dTuThangPS != null && !dTuThangPS.Trim().Equals(""))
                SQLDSNS += " AND (iThangCT<@DenThangPS or(iThangCT = @DenThangPS and iNgayCT <= @DenNgayPS))";

            if (sTKDoiUng != null && !sTKDoiUng.Trim().Equals(""))
                SQLDSNS += " AND TKDoiUng = @TKDoiUng";
            if (sDVNo != null && !sDVNo.Trim().Equals(""))
                SQLDSNS += " AND iID_MaDonVi_No = @sDVNo";
            if (sDVCo != null && !sDVCo.Trim().Equals(""))
                SQLDSNS += " AND iID_MaDonVi_Co = @sDVCo";
            if (sDVNhan != null && !sDVNhan.Trim().Equals(""))
                SQLDSNS += " AND iID_MaDonVi_Nhan = @sDVNhan";

            SQLDSNS += " order by KTTG_ChungTuChiTiet.iNgay asc";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            cmdThangNay.Parameters.AddWithValue("@DoiTuong", DoiTuong);
            cmdThangNay.Parameters.AddWithValue("@TuNam", DateTime.Now.Year);
            cmdThangNay.Parameters.AddWithValue("@TuThang", dTuThangGhiSo);
            cmdThangNay.Parameters.AddWithValue("@TuNgay", dTuNgayGhiSo);
            cmdThangNay.Parameters.AddWithValue("@DenThang", dDenThangGhiSo);
            cmdThangNay.Parameters.AddWithValue("@DenNgay", dDenNgayGhiSo);

            if (dTuNgayPS != null && !dTuNgayPS.Trim().Equals("")
               && dDenNgayPS != null && !dDenNgayPS.Trim().Equals("")
               && dDenThangPS != null && !dDenThangPS.Trim().Equals("")
               && dTuThangPS != null && !dTuThangPS.Trim().Equals(""))
            {
                cmdThangNay.Parameters.AddWithValue("@TuThangPS", dTuThangPS);
                cmdThangNay.Parameters.AddWithValue("@TuNgayPS", dTuNgayPS);
            }
            if (dTuNgayPS != null && !dTuNgayPS.Trim().Equals("")
               && dDenNgayPS != null && !dDenNgayPS.Trim().Equals("")
               && dDenThangPS != null && !dDenThangPS.Trim().Equals("")
               && dTuThangPS != null && !dTuThangPS.Trim().Equals(""))
            {
                cmdThangNay.Parameters.AddWithValue("@DenThangPS", dDenThangPS);
                cmdThangNay.Parameters.AddWithValue("@DenNgayPS", dDenNgayPS);
            }

            if (sTKDoiUng != null && !sTKDoiUng.Equals(""))
                cmdThangNay.Parameters.AddWithValue("@TKDoiUng", sTKDoiUng);
            if (sDVNo != null && !sDVNo.Trim().Equals(""))
                cmdThangNay.Parameters.AddWithValue("@sDVNo", sDVNo);
            if (sDVCo != null && !sDVCo.Trim().Equals(""))
                cmdThangNay.Parameters.AddWithValue("@sDVCo", sDVCo);
            if (sDVNhan != null && !sDVNhan.Trim().Equals(""))
                cmdThangNay.Parameters.AddWithValue("@sDVNhan", sDVNhan);
            cmdThangNay.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            dtDoiTuong = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();

            return dtDoiTuong;
        }


        public string KT_TaiKhoan_Get_Ten(String iID_MaTaiKhoan)
        {
            DataTable dtDoiTuong = new DataTable();

            //lay du lieu thang quy
            string sTen = "";
            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT top 1 iID_MaTaiKhoan, sTen" +
                " from KT_TaiKhoan WHERE iID_MaTaiKhoan = @iID_MaTaiKhoan";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            cmdThangNay.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            dtDoiTuong = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();
            if (dtDoiTuong != null)
            {
                if (dtDoiTuong.Rows.Count > 0) sTen = Convert.ToString(dtDoiTuong.Rows[0]["iID_MaTaiKhoan"]) + " - " + Convert.ToString(dtDoiTuong.Rows[0]["sTen"]);
            }
            else
            {
                sTen = "";
            }
            return sTen;
        }

        public JsonResult Get_objNgayThang(String ParentID, String TenTruong, String Ngay, String Thang, String iNam)
        {
            return Json(get_sNgayThang(ParentID, TenTruong, Ngay, Thang, iNam), JsonRequestBehavior.AllowGet);
        }
        public string get_sNgayThang(String ParentID, String TenTruong, String Ngay, String Thang, String iNam)
        {
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(Thang), Convert.ToInt16(iNam), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            int SoNgayTrongThang = DateTime.DaysInMonth(Convert.ToInt16(iNam), Convert.ToInt16(Thang));
            if (String.IsNullOrEmpty(Ngay) == false)
            {
                if (Convert.ToInt16(Ngay) > SoNgayTrongThang)
                    Ngay = "1";
            }
            return MyHtmlHelper.DropDownList(ParentID, slNgay, Ngay, TenTruong, "", "class=\"input1_2\" style=\"width:100%\"");
        }

    }
}
