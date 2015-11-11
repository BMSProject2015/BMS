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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_42_6bController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_42_6b.xls";
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
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_42_6b.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        } 
        /// <summary>
        /// EditSubmit nhận và trả các giá tri với VIEW
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Thang_Quy = "";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            //nếu loại là tháng thì tháng quý mang ý nghĩa là tháng
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            //nếu loại là tháng thì tháng quý mang ý nghĩa là tháng
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String TongHop = Convert.ToString(Request.Form[ParentID + "_iTongHop"]);
            String iID_MaDanhMuc = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMuc"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["TruongTien"] = TruongTien;
            ViewData["sLNS"] = sLNS;
            ViewData["TongHop"] = TongHop;
            ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_42_6b.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"> đường dẫn</param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1: là quý</param>
        /// <param name="TruongTien">Tự chi hay hiện vật</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng/ Quý làm việc</param>
        /// <param name="iID_MaDanhMuc"> Mã nhóm đơn vị</param>
        /// <param name="TongHop">nếu =on là tổng hợp các nhóm</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy, String iID_MaDanhMuc, String TongHop)
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
            String TenLoaiThangQuy = "";
            switch (LoaiThang_Quy)
            {
                case "0":
                    TenLoaiThangQuy = "tháng";
                    break;
                case "1":
                    TenLoaiThangQuy = "quý";
                    break;
                case "2":
                    TenLoaiThangQuy = "năm";
                    break;
            }
            //lấy mổ tả LNS
            String TenLNS = "";
            DataTable dt = MoTa(sLNS);
            if (dt.Rows.Count > 0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
            //tính tổng tiền
            DataTable dtTien = QT_42_6b(iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy, iID_MaDanhMuc, TongHop,MaND);
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
            //Cấu hình tên đơn vị sử dụng
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_42_6b");
            LoadData(fr, iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy, iID_MaDanhMuc, TongHop,MaND);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("Thang_Quy", Thang_Quy);
                fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
                fr.SetValue("TenLNS", TenLNS);
                fr.SetValue("Tien", Tien);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Hàm lấy dữ liệu đổ vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1: là quý</param>
        /// <param name="TruongTien">Tự chi hay hiện vật</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng/ Quý làm việc</param>
        /// <param name="iID_MaDanhMuc"> Mã nhóm đơn vị</param>
        /// <param name="TongHop">nếu =on là tổng hợp các nhóm</param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy, String iID_MaDanhMuc, String TongHop,String MaND)
        {

            DataTable data = QT_42_6b(iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy, iID_MaDanhMuc, TongHop,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1: là quý</param>
        /// <param name="TruongTien">Tự chi hay hiện vật</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng/ Quý làm việc</param>
        /// <param name="iID_MaDanhMuc"> Mã nhóm đơn vị</param>
        /// <param name="TongHop">nếu =on là tổng hợp các nhóm</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String LoaiThangQuy, String TruongTien, String sLNS, String Thang_Quy, String iID_MaDanhMuc, String TongHop)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, LoaiThangQuy, TruongTien, sLNS, Thang_Quy, iID_MaDanhMuc, TongHop);
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
        /// Xuất báo cáo ra excel
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1: là quý</param>
        /// <param name="TruongTien">Tự chi hay hiện vật</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng/ Quý làm việc</param>
        /// <param name="iID_MaDanhMuc"> Mã nhóm đơn vị</param>
        /// <param name="TongHop">nếu =on là tổng hợp các nhóm</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String LoaiThangQuy, String TruongTien, String sLNS, String Thang_Quy, String iID_MaDanhMuc, String TongHop)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, LoaiThangQuy, TruongTien, sLNS, Thang_Quy, iID_MaDanhMuc, TongHop);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_42_6b.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1: là quý</param>
        /// <param name="TruongTien">Tự chi hay hiện vật</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng/ Quý làm việc</param>
        /// <param name="iID_MaDanhMuc"> Mã nhóm đơn vị</param>
        /// <param name="TongHop">nếu =on là tổng hợp các nhóm</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy, String iID_MaDanhMuc, String TongHop)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, LoaiThang_Quy, TruongTien, sLNS, Thang_Quy, iID_MaDanhMuc, TongHop);
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
        /// lấy datatable quyết toán 42_6b
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="LoaiThang_Quy">0: là tháng 1: là quý</param>
        /// <param name="TruongTien">Tự chi hay hiện vật</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="Thang_Quy">Tháng/ Quý làm việc</param>
        /// <param name="iID_MaDanhMuc"> Mã nhóm đơn vị</param>
        /// <param name="TongHop">nếu =on là tổng hợp các nhóm</param>
        /// <returns></returns>
        public DataTable QT_42_6b(String iID_MaTrangThaiDuyet, String LoaiThang_Quy, String TruongTien, String sLNS, String Thang_Quy, String iID_MaDanhMuc, String TongHop,String MaND)
        {
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            DataTable dt = null;
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
                DK_ThangQuy = String.Format("iThang_Quy={0}", Thang_Quy);
            }
            if (TongHop == "on")
            {
                String SQL1 = String.Format(@"SELECT QTA.iID_MaDonVi,sTen,SUM(QTA.{0}) as SoTien
                                        FROM (SELECT iID_MaDonVi,{0} FROM QTA_ChungTuChiTiet 
                                        WHERE {3}  AND sLNS=@sLNS AND sNG<>'' {1} {2} AND iTrangThai=1) as QTA
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON QTA.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        GROUP BY QTA.iID_MaDonVi,sTen
                                        ORDER BY QTA.iID_MaDonVi", TruongTien, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet, DK_ThangQuy);
                SqlCommand cmd1 = new SqlCommand(SQL1);
                //cmd1.Parameters.AddWithValue(@"Thang_Quy", Thang_Quy);
                //cmd1.Parameters.AddWithValue(@"LoaiThangQuy", LoaiThang_Quy);
                cmd1.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd1.Parameters.AddWithValue(@"sLNS", sLNS);
                dt = Connection.GetDataTable(cmd1);
                cmd1.Dispose();

            }
            else
            {
                String SQL = String.Format(@"SELECT QTA.iID_MaDonVi,NS_DonVi.sTen,SUM(QTA.{0}) as SoTien
                                        FROM (SELECT iID_MaDonVi,{0} FROM QTA_ChungTuChiTiet 
                                        WHERE {3} AND sLNS=@sLNS AND sNG<>'' {1} AND iTrangThai=1 {2} ) as QTA
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaNhomDonVi FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON QTA.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc) AS DM ON NS_DonVi.iID_MaNhomDonVi=DM.iID_MaDanhMuc 
                                        GROUP BY QTA.iID_MaDonVi,NS_DonVi.sTen
                                        ORDER BY QTA.iID_MaDonVi", TruongTien, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet, DK_ThangQuy);
                SqlCommand cmd = new SqlCommand(SQL);                
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iID_MaDanhMuc);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            // thêm dòng trống nếu dt bé hơn 15
            int a = dt.Rows.Count;
            if (a < 16 && a>=0)
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
        /// <summary>
        /// dt Trạng Thái Duyệt
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
