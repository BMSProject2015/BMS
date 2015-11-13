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
    public class rptDTCNSQP_XayDungCoBanController : Controller
    {    
        // GET: /rptDTCNSQP_XayDungCoBan/       
              
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/NSQP_XayDungCoBan.xls";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDTCNSQP_XayDungCoBan.aspx";
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
            
            return RedirectToAction("Index", new {  iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn đến file excel mẫu</param>
        /// <param name="NamLamViec">Năm dự toán</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDTCNSQP_XayDungCoBan");
            LoadData(fr,  MaND, iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);            
            return Result;           
        }
        /// <summary>
        /// Dự toán chi ngân sách quốc phòng phần ngân sách xây dựng cơ bản mẫu 3-C
        /// </summary>
        /// <param name="NamLamViec">LNS:1030100; L:460;K:468</param>
        /// <returns></returns>
        public DataTable DT_DuToanChiNganSachQuocPhong_XDCB(String MaND, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = "SELECT DM.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SQL += ",SUM(rTongSo) as rTongSo";
            SQL += ",SUM(rPhanCap) as rTuChi";
            SQL += ",SUM(rDuPhong) as rDuPhong";
            SQL += " FROM (SELECT ";
            SQL += " iID_MaDonVi";
            SQL += ",sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";
            SQL += ",rTongSo=SUM(rPhanCap+rDuPhong)";
            SQL += ",SUM(rPhanCap) as rPhanCap";
            SQL += ",SUM(rDuPhong) as rDuPhong";
            SQL += " FROM DT_ChungTuChiTiet ";
            SQL += " WHERE iTrangThai=1 AND sLNS='1030100' AND sL='460' AND sK='468'  {1}  {0}";
            SQL += " Group BY  iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTonKho,rHangNhap";
            SQL += " ) ChiTiet";
            SQL += " INNER JOIN NS_DonVi ON ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi ";
            SQL += " INNER JOIN (SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang)) DM ";
            SQL += " ON NS_DonVi.iID_MaKhoiDonVi=DM.iID_MaDanhMuc";
            SQL += " GROUP By DM.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,ChiTiet.sMoTa";
            SQL = String.Format(SQL, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenBang", "KhoiDonVi");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec">Năm dự toán</param>
        private void LoadData(FlexCelReport fr,String MaND,String iID_MaTrangThaiDuyet)
        {
            DataTable data = DT_DuToanChiNganSachQuocPhong_XDCB(MaND,iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa", "sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", data, "sM", "sM,sMoTa", "sM,sTM");
            fr.AddTable("Muc", dtMuc);

            data.Dispose();
            dtTieuMuc.Dispose();
            dtMuc.Dispose();
        }
        /// <summary>
        /// Hiển thị theo định dạng pdf
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND,String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
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
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptDTCNSQP_XayDungCoBan.xls";
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
