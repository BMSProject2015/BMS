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

namespace VIETTEL.Report_Views.QuyetToan
{
    public class rptQuyetToan_42_6Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_42_6.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_42_6.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// EditSubmit nhận giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Thang_Quy = "";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            //nếu là tháng
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            //nếu là quý
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            String TruongTien=Convert.ToString(Request.Form[ParentID+"_TruongTien"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["TruongTien"] = TruongTien;
            ViewData["sLNS"] = sLNS;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_42_6.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// khởi tạo báo cáo
        /// </summary>
        /// <param name="path"> Đường dẫn Excel</param>
        /// <param name="NamLamViec"> Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1:là Quý</param>
        /// <param name="TruongTien">Tự chi hay Hiện vật</param>
        /// <param name="sLNS">loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng hay quý mấy làm việc</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy)
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenLoai = "";
            switch (LoaiThang_Quy)
            {
                case "0":
                    TenLoai = "tháng";
                    break;
                case "1":
                    TenLoai = "quý";
                    break;
                case "2":
                    TenLoai = "năm";
                    break;
            }
            String TenLNS = "";
            DataTable dt = MoTa(sLNS);
            if (dt.Rows.Count>0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
            //tính tổng tiền
            DataTable dtTien = QT_42_6(iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy,MaND);
            long TongTien = 0;
            for (int i = 0; i < dtTien.Rows.Count; i++)
            {
                if (dtTien.Rows[i]["SoTien"].ToString() != "")
                {
                    TongTien += long.Parse(dtTien.Rows[i]["SoTien"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            //lấy ngày hiện tại
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_42_6");
                LoadData(fr, iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy,MaND);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("Thang_Quy", Thang_Quy);
                fr.SetValue("LoaiThangQuy", TenLoai);
                fr.SetValue("TenLNS", TenLNS);
                fr.SetValue("Tien", Tien);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.Run(Result);
                return Result;          
        }
        /// <summary>
        /// Lấy data Fill lên báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="TruongTien"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy,String MaND)
        {

            DataTable data = QT_42_6(iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>       
        /// <param name="NamLamViec"> Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1:là Quý</param>
        /// <param name="TruongTien">Tự chi hay Hiện vật</param>
        /// <param name="sLNS">loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng hay quý mấy làm việc</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy);
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
        /// Xuất ra file excel
        /// </summary>       
        /// <param name="NamLamViec"> Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1:là Quý</param>
        /// <param name="TruongTien">Tự chi hay Hiện vật</param>
        /// <param name="sLNS">loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng hay quý mấy làm việc</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_42_6.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec"> Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1:là Quý</param>
        /// <param name="TruongTien">Tự chi hay Hiện vật</param>
        /// <param name="sLNS">loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng hay quý mấy làm việc</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy);
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
        /// datatable quyết toán nghiệp vụ 42_6
        /// </summary>
        /// <param name="NamLamViec"> Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1:là Quý</param>
        /// <param name="TruongTien">Tự chi hay Hiện vật</param>
        /// <param name="sLNS">loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng hay quý mấy làm việc</param>
        /// <returns></returns>
        public DataTable QT_42_6(String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }

            String DK_DenKy = "";
            String DK_ThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_ThangQuy = "  (iThang_Quy between 1  and 3)";
                        DK_DenKy = "  (iThang_Quy between 1  and 3)";
                        break;
                    case "2": DK_ThangQuy = "  (iThang_Quy between 4  and 6)";
                        DK_DenKy = "  (iThang_Quy between 1  and 6)";
                        break;
                    case "3": DK_ThangQuy = "  (iThang_Quy between 7  and 9)";
                        DK_DenKy = "  (iThang_Quy between 1  and 9)";
                        break;
                    case "4": DK_ThangQuy = " (iThang_Quy between 10  and 12)";
                        DK_DenKy = "  (iThang_Quy between 1  and 12)";
                        break;
                }
            }
            else
            {
                DK_ThangQuy = String.Format("(iThang_Quy ={0})",Thang_Quy);
            }
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT QTA.iID_MaDonVi,sTen,SUM(QTA.{0}) as SoTien
                                        FROM (SELECT iID_MaDonVi,{0} FROM QTA_ChungTuChiTiet 
                                        WHERE {3}  AND sLNS=@sLNS AND sNG<>'' {1} {2}) as QTA
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON QTA.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        GROUP BY QTA.iID_MaDonVi,sTen
                                        ORDER BY QTA.iID_MaDonVi", TruongTien, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet, DK_ThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue(@"Thang_Quy", Thang_Quy);
            //cmd.Parameters.AddWithValue(@"LoaiThangQuy", LoaiThang_Quy);
            cmd.Parameters.AddWithValue(@"sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            // thêm dòng trống nếu dt bé hơn 15
            int a = dt.Rows.Count;
            if (a < 16 && a >= 0)
            {
                for (int i = 0; i < 16 - a; i++)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, a + 1);
                }
            }
            dt.Dispose();
            return dt;
        }

        /// <summary>
        /// lấy mô tả của LNS
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

    }
}
