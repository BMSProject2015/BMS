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

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_BangKeChiTietController : Controller
    {
        // Edit: Thương
        // GET: /rptLuong_Giay_CCTC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_BangKeChiTiet.xls";
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_BangKeChiTiet.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Sự kiện submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;            
            String TuThang = Convert.ToString(Request.Form[ParentID + "_iTuThang"]);
            String DenThang = Convert.ToString(Request.Form[ParentID + "_iDenThang"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);            
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iDenThang"] = DenThang;
            ViewData["iTuThang"] = TuThang;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_BangKeChiTiet.aspx";            
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="TuThang">Từ tháng</param>
        /// <param name="DenThang">Đến tháng</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public DataTable BangKeChiTiet(String TuThang, String DenThang, String Duyet)
        {
            String DKDuyet = Duyet.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String SQL = String.Format(@"SELECT L.iID_MaDonVi --MÃ ĐƠN VỊ
                                                ,L.sTenDonVi --TÊN ĐƠN VỊ	  
                                                ,L.sHoDem_CanBo as HD --HỌ ĐỆM
                                                ,L.sTen_CanBo AS TEN --TÊN CÁN BỘ
                                                ,SUM(L.rLuongCoBan+l.rPhuCap_ChucVu+L.rThuong+L.rLoiIchKhac+l.rDieuChinhThuNhap) AS LCB --THU NHẬP TỪ LƯƠNG VÀ TIỀN CÔNG
                                                ,SUM(L.rNopThueDauVao)*10 AS TNK --THU NHẬP KHÁC
                                                ,SUM(L.rGiamTruBanThan+L.rGiamTruGiaCanh+rBaoHiem_Tong_CaNhan+L.rGiamTruKhac+l.rDieuChinhThueGiamTru) AS GTK --GIẢM TRỪ KHÁC
                                                ,SUM(L.rLuongCoBan+l.rPhuCap_ChucVu+L.rThuong+L.rLoiIchKhac+l.rDieuChinhThuNhap)+SUM(L.rNopThueDauVao)*10-SUM(L.rGiamTruBanThan+L.rGiamTruGiaCanh+rBaoHiem_Tong_CaNhan+L.rGiamTruKhac+l.rDieuChinhThueGiamTru) AS TNTT --THU NHẬP TÍNH THUẾ
                                                ,SUM(L.rDuocGiamThue) AS STDG --SỐ THUẾ ĐƯỢC GIẢM
                                                ,SUM(L.rNopThueDauVao) AS KTTN --KHẤU TRỪ TẠI NGUỒN
                                                ,SUM(L.rThueTNCN) as KTHT --KHẤU TRỪ HÀNG THÁNG
                                                ,STPN=0 --SỐ PHẢI NỘP
                                                --,SUM(l.rThueTNCN)-SUM(l.rDuocGiamThue)-SUM(L.rNopThueDauVao)-SUM(L.rThueTNCN) AS STCPN --SỐ THUẾ CÒN PHẢI NỘP
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                            AND L.iThangBangLuong BETWEEN @TuThang AND @DenThang
                                            {0}   
                                        GROUP BY L.iID_MaDonVi
                                                ,L.sTenDonVi
                                                ,L.sTen_CanBo
                                                ,L.sHoDem_CanBo
                                        HAVING SUM(L.rLuongCoBan+l.rPhuCap_ChucVu+L.rThuong+L.rLoiIchKhac+L.rDieuChinhThuNhap)+SUM(L.rNopThueDauVao)*10-SUM(L.rGiamTruBanThan+L.rGiamTruGiaCanh+rBaoHiem_Tong_CaNhan+L.rGiamTruKhac+L.rDieuChinhThueGiamTru)>0", DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);           
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dtKQ = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtKQ;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="TuThang">Từ tháng</param>
        /// <param name="DenThang">Đến tháng</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String TuThang, String DenThang, String Duyet)
        {
            HamChung.Language();            
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), TuThang, DenThang, Duyet);
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
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="TuThang">Từ tháng</param>
        /// <param name="DenThang">Đến tháng</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        private void LoadData(FlexCelReport fr, String TuThang, String DenThang, String Duyet)
        {
            DataTable dt = BangKeChiTiet(TuThang, DenThang,Duyet);
            dt = GetData(dt, "TNTT", "STPN");
            DataTable dtDonvi = HamChung.SelectDistinct("Donvi", dt, "iID_MaDonVi", "iID_MaDonVi");
            dt.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", dt);
            fr.AddTable("Donvi", dtDonvi);
            dt.Dispose();
            dtDonvi.Dispose();
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn đến file Excel mẫu</param>
        /// <param name="TuThang">Từ tháng</param>
        /// <param name="DenThang">Đến tháng</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String TuThang, String DenThang, String Duyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_BangKeChiTiet");
            LoadData(fr, TuThang, DenThang, Duyet);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("thang1", TuThang);
            fr.SetValue("thang2", DenThang);
            fr.SetValue("Cap1", ReportModels.LayDonviSD("L_DanhMucThamSo","sNoiDung","N'Đơn vị cấp trên'","sThamSo").ToUpper());
            fr.SetValue("Cap2", ReportModels.LayDonviSD("L_DanhMucThamSo", "sNoiDung", "N'Đơn vị làm lương'", "sThamSo").ToUpper());
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Xuất báo cáo ra file excel
        /// </summary>
        /// <param name="TuThang">Từ tháng</param>
        /// <param name="DenThang">Đến tháng</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String TuThang, String DenThang, String Duyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), TuThang, DenThang, Duyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangKeChiTiet.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public DataTable GetData(DataTable dtSource, String Field_DK, String Field_Result)
        {
            double tien=0;
            if (dtSource.Rows.Count > 0)
            {
                for (int i = 0; i < dtSource.Rows.Count; i++)
                { 
                    tien=dtSource.Rows[i][Field_DK].ToString().Equals("")?0:double.Parse(dtSource.Rows[i][Field_DK].ToString());
                    dtSource.Rows[i][Field_Result] = CauHinhLuongModels.TinhThueTNCN(tien);
                }
            }
            return dtSource;
        }
    }
}
