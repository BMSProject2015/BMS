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
    public class rtCongKhaiPhanBoVonDauTuCongTrinhPhoThong_TongHopController : Controller
    {
        //
        // GET: /rtCongKhaiPhanBoVonDauTuCongTrinhPhoThong/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptCongKhaiPhanBoVonDauTuCongTrinhPhoThong_TongHop.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptCongKhaiPhanBoVonDauTuCongTrinhPhoThong_TongHop.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet)
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
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rtCongKhaiPhanBoVonDauTuCongTrinhPhoThong_TongHop");
                LoadData(fr, MaND,iID_MaTrangThaiDuyet);
                fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("ThangHienTai", DateTime.Now.Month);
                fr.SetValue("NamHienTai", DateTime.Now.Year);
                fr.SetValue("ngay",ngay);
                fr.SetValue("Nam", iNamLamViec);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public long Tong = 0;
        public  DataTable CongKhaiPhanBoVonDauTu_CongTrinhPhoThong(String MaND,String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT dbo.NS_MucLucDuAn.sMaCongTrinh
                                        ,dbo.NS_MucLucDuAn.sTen AS TenDuAn
                                        ,NS_DonVi.sTen as TenDonVi
                                        ,LoaiDuAn.sTen AS TenLoaiDuAn
                                        ,ThamQuyen.sTen AS TenThamQuyen
                                        ,TCDuAn.sTen as TenTinhChatDuAn
                                        ,SUM(ChiTiet.rPhanCap) as rPhanCap
                                        ,ChiTiet.sTenCongTrinh 
                                        FROM  DT_ChungTuChiTiet as ChiTiet 
                                        INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi On ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi 
                                        INNER JOIN dbo.NS_MucLucDuAn ON ChiTiet.sMaCongTrinh=NS_MucLucDuAn.sMaCongTrinh 
                                        INNER JOIN dbo.DC_DanhMuc AS LoaiDuAn ON NS_MucLucDuAn.iID_LoaiDuAn = LoaiDuAn.iID_MaDanhMuc 
                                        INNER JOIN dbo.DC_DanhMuc AS ThamQuyen ON dbo.NS_MucLucDuAn.iID_MaThamQuyen = ThamQuyen.iID_MaDanhMuc 
                                        INNER JOIN dbo.DC_DanhMuc AS TCDuAn ON dbo.NS_MucLucDuAn.iID_TinhChatDuAn = TCDuAn.iID_MaDanhMuc 
                                        WHERE  ChiTiet.iTrangThai = 1 {0} {1}
                                        GROUP BY dbo.NS_MucLucDuAn.sMaCongTrinh
                                        ,dbo.NS_MucLucDuAn.sTen
                                        ,NS_DonVi.sTen 
                                        ,LoaiDuAn.sTen 
                                        ,ThamQuyen.sTen 
                                        ,TCDuAn.sTen 
                                         ,ChiTiet.sTenCongTrinh 
                                        HAVING SUM(ChiTiet.rPhanCap)!=0", ReportModels.DieuKien_NganSach(MaND,"ChiTiet"),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Tong += long.Parse(dt.Rows[i]["rPhanCap"].ToString());
            }
                cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr,String MaND, String iID_MaTrangThaiDUyet)
        {
            DataTable data = CongKhaiPhanBoVonDauTu_CongTrinhPhoThong(MaND,iID_MaTrangThaiDUyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTinhChatDuAn;
            dtTinhChatDuAn = HamChung.SelectDistinct("TCDuAn", data, "TenDonVi,TenLoaiDuAn,TenThamQuyen,TenTinhChatDuAn", "TenDonVi,TenLoaiDuAn,TenThamQuyen,TenTinhChatDuAn", "");
            fr.AddTable("TCDuAn", dtTinhChatDuAn);

            DataTable dtThamQuyen;
            dtThamQuyen = HamChung.SelectDistinct("ThamQuyen", dtTinhChatDuAn, "TenDonVi,TenLoaiDuAn,TenThamQuyen", "TenDonVi,TenLoaiDuAn,TenThamQuyen", "");
            fr.AddTable("ThamQuyen", dtThamQuyen);
            DataTable dtLoaiDuAn;
            dtLoaiDuAn = HamChung.SelectDistinct("LoaiDuAn", dtThamQuyen, "TenDonVi,TenLoaiDuAn", "TenDonVi,TenLoaiDuAn", "");
            fr.AddTable("LoaiDuAn", dtLoaiDuAn);
            DataTable dtDonVi;
            dtDonVi = HamChung.SelectDistinct("DonVi", dtThamQuyen, "TenDonVi", "TenDonVi", "");
            fr.AddTable("DonVi", dtDonVi);
            data.Dispose();
            dtLoaiDuAn.Dispose();
            dtThamQuyen.Dispose();
            dtDonVi.Dispose();


        }
        /// <summary>
        /// Ham xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CongKhaiPhanBoVonDauTuCongTrinhPhoThong.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet);
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
