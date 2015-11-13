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
using System.Text;

namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKhoBac_DoiChieuDuToanController : Controller
    {
        //
        // GET: /rptKTKhoBac_DoiChieuDuToan/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePathTH = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKhoBac_DoiChieuDuToanTH.xls";
        private const String sFilePathCT = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKhoBac_DoiChieuDuToanCT.xls";
        private const String sFilePathGom = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKhoBac_DoiChieuDuToanTHCT.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["pageload"] = "0";
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKhoBac_DoiChieuDuToan.aspx";
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
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iLoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_divThangQuy"]);
            String iThangQuy = Convert.ToString(Request.Form[ParentID + "_iThangQuy"]);
            String iReport = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao"]);
            String iLoaiNS = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiNganSach"]);
            String iInmuc = Convert.ToString(Request.Form[ParentID + "_divReport"]);
            String iGhep = Convert.ToString(Request.Form[ParentID + "_divGhep"]);
            String iGom = Convert.ToString(Request.Form[ParentID + "_divGom"]);
            ViewData["iNamLamViec"] = iNam;
            ViewData["iLoaiThang_Quy"] = iLoaiThang_Quy;
            ViewData["iThang_Quy"] = iThangQuy;
            ViewData["iID_MaLoaiNganSach"] = iLoaiNS;
            ViewData["iLoaiBaoCao"] = iReport;
            ViewData["inmuc"] = iInmuc;
            ViewData["iGhep"] = iGhep;
            ViewData["iGom"] = iGom;
            String sFilePath = "";
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKhoBac_DoiChieuDuToan.aspx";
            sFilePath = GetPath(iReport, iGom, sFilePath);
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy đường dẫn file Excel
        /// </summary>
        /// <param name="iReport">Loại báo cáo</param>
        /// <param name="iGom">on: Gom 2 phần trên 1 báo cáo | off: Không gom 2 phần trên 1 báo cáo</param>
        /// <param name="sFilePath">Đường dẫn file Excel</param>
        /// <returns></returns>
        private static String GetPath(String iReport, String iGom, String sFilePath)
        {
            switch (iGom)
            {
                case "on"://Gom 2 phần trên 1 báo cáo
                    sFilePath = sFilePathGom;
                    break;
                case "off":
                    sFilePath = iReport.Equals("TH") ? sFilePathTH : sFilePathCT;
                    break;
            }
            return sFilePath;
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel</param>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="LoaiBaoCao">Loại báo cáo</param>
        /// <param name="inmuc">In đến mức Loại khoản| Mục | Tiểu mục</param>
        /// <param name="iID_MaNguonNganSach">Loại ngân sách</param>
        /// <param name="UserID">Người dùng</param>
        /// <param name="iGom">On: Ghép 2 báo cáo trên file | off: Ngược lại</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String LoaiBaoCao, String inmuc, String iID_MaNguonNganSach, String iGom, String UserID)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            String TenLoaiThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                TenLoaiThangQuy = "Quý";
            }
            else
            {
                TenLoaiThangQuy = "Tháng";
            }
            String KeToan = "(Không có)";

            String SQL = @"SELECT sThamSo FROM KT_DanhMucThamSo
                         WHERE sKyHieu=201";
            String ThamSo = "";
            ThamSo = Connection.GetValueString(SQL, "");
            if (String.Equals(ThamSo.ToUpper(), "C"))
                KeToan = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKhoBac_DoiChieuDuToan");
            LoadData(fr, NamLamViec, Thang_Quy, LoaiThang_Quy, LoaiBaoCao, inmuc, iID_MaNguonNganSach, iGom,UserID);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("MucIn", inmuc);
            fr.SetValue("KeToan", KeToan);
            String st = getValues(NamLamViec, Thang_Quy, LoaiThang_Quy, iID_MaNguonNganSach, UserID, "sC");
            fr.SetValue("Chuong",getValues(NamLamViec,Thang_Quy,LoaiThang_Quy,iID_MaNguonNganSach,UserID,"sC"));
            switch (iGom)
            { 
                case "on":
                    fr.SetValue("NgayKB", "Ngày .... tháng..... năm " + DateTime.Now.Year.ToString());
                    fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
                    break;
                case "off":
                    if (LoaiBaoCao.Equals("CT"))
                    {
                        fr.SetValue("NgayKB", "Ngày .... tháng..... năm " + DateTime.Now.Year.ToString());
                        fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
                    }
                    break;
            }
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="LoaiBaoCao">Loại báo cáo in ra</param>
        /// <param name="inmuc">In đến mục loại khoản - mục - tiểu mục</param>
        /// <param name="iID_MaNguonNganSach">Nguồn ngân sách</param>
        /// <param name="iGom">On: Ghép 2 báo cáo trên file | off: Ngược lại</param>
        /// <param name="UserID">Người dùng</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String LoaiBaoCao, String inmuc, String iID_MaNguonNganSach, String iGom, String UserID)
        {
            HamChung.Language();
            String DuongDan = "";
            DuongDan = GetPath(LoaiBaoCao, iGom, DuongDan);
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, LoaiBaoCao, inmuc, iID_MaNguonNganSach, iGom,UserID);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="LoaiBaoCao">Loại báo cáo in ra</param>
        /// <param name="inmuc">In đến mục loại khoản - mục - tiểu mục</param>
        /// <param name="iID_MaNguonNganSach">Nguồn ngân sách</param>
        /// <param name="iGom">On: Ghép 2 báo cáo trên file | off: Ngược lại</param>
        /// <param name="UserID">Người dùng</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String LoaiBaoCao, String inmuc, String iID_MaNguonNganSach, String iGom, String UserID)
        {
            String DuongDan = "";
            DuongDan = GetPath(LoaiBaoCao, iGom, DuongDan);
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, LoaiBaoCao, inmuc, iID_MaNguonNganSach, iGom,UserID);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DoiChieuDuToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="LoaiBaoCao">Loại báo cáo</param>
        /// <param name="inmuc">In đến Loại khoản | Mục | Tiểu mục</param>
        /// <param name="iID_MaNguonNganSach">Mã loại ngân sách</param>
        /// <param name="iGom">On: Ghép 2 phần trên 1 báo cáo | Off: In 1 phần trên mỗi báo cáo</param>
        private void LoadData(FlexCelReport fr, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String LoaiBaoCao, String inmuc, String iID_MaNguonNganSach, String iGom,String UserID)
        {
            DataTable data = DoiChieuDuToan(NamLamViec, Thang_Quy, LoaiThang_Quy, LoaiBaoCao, inmuc, iID_MaNguonNganSach,UserID);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", data, "iID_MaNguonNganSach,sL,sK,sM", "iID_MaNguonNganSach,sL,sK,sM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("Khoan", dtMuc, "iID_MaNguonNganSach,sL,sK", "iID_MaNguonNganSach,sL,sK");
            fr.AddTable("Khoan", dtLoaiNS);           
            if (iGom.Equals("on"))
            {
                DataTable data1 = DoiChieuDuToan1(NamLamViec, Thang_Quy, LoaiThang_Quy, LoaiBaoCao, inmuc, iID_MaNguonNganSach, UserID);
                data1.TableName = "ChiTiet1";
                fr.AddTable("ChiTiet1", data1);
            }
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
        }
        /// <summary>
        /// Lấy thông tin 
        /// </summary>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="LoaiBaoCao">Loại báo cáo</param>
        /// <param name="inmuc">In báo cáo đến loại khoản-mục-tiểu mục</param>
        /// <param name="iID_MaNguonNganSach">Nguồn ngân sách</param>
        /// <param name="UserID">Người dùng</param>
        /// <returns></returns>
        public DataTable DoiChieuDuToan(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String LoaiBaoCao, String inmuc, String iID_MaNguonNganSach,String UserID)
        {
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            String DKThang = "";
            String DKThang1 = "";
            DataTable dtCH = NguoiDungCauHinhModels.LayCauHinh(UserID);
            String iID_MaNamNganSach = dtCH.Rows.Count > 0 ? Convert.ToString(dtCH.Rows[0]["iID_MaNamNganSach"]) : "-1";

            SqlCommand cmd = new SqlCommand();
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (1,2,3) ";
                        DKThang = "iThangCT between 1 and 3";
                        DKThang1 = "iThangCT between 1 and 3";
                        break;
                    case 2: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (4,5,6) ";
                        DKThang = "iThangCT between 4 and 6";
                        DKThang1 = "iThangCT between 4 and 6";
                        break;
                    case 3: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (7,8,9) ";
                        DKThang = "iThangCT between 7 and 9";
                        DKThang1 = "iThangCT between 7 and 9";
                        break;
                    case 4: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (10,11,12) ";
                        DKThang = "iThangCT between 10 and 12";
                        DKThang1 = "iThangCT between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThang_Quy = " MONTH(dNgayDotRutDuToan)<=@ThangQuy ";
                DKThang = "iThangCT<=@ThangQuy";
                DKThang1 = "iThangCT=@ThangQuy";
            }

            String SQL = " SELECT sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach,SUM(TongSo) TongSo";
            SQL += " ,sum(rDTRutLK) as rDTRutLK ,sum(rDTKhoiPhucLK) as rDTKhoiPhucLK,sum(rDTHuyLK) as rDTHuyLK";
            SQL += "  ,sum(rDTRut) as rDTRut ,sum(rDTKhoiPhuc) as rDTKhoiPhuc,sum(rDTHuy) as rDTHuy";
            SQL += " FROM( ";
            SQL += " SELECT sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach,0 as TongSo,";
            SQL += " rDTRutLK = sum(case when {1} then rDTRut else 0 end) ,";
            SQL += " rDTKhoiPhucLK = sum(case when {1} then rDTKhoiPhuc else 0 end ),";
            SQL += " rDTHuyLK = sum(case when {1} then rDTHuy   else 0 end),";
            SQL += "  rDTRut = sum(case when {2} then rDTRut else 0 end ),";
            SQL += "  rDTKhoiPhuc = sum(case when {2} then rDTKhoiPhuc else 0 end) ,";
            SQL += " rDTHuy = sum(case when {2} then rDTHuy   else 0 end)";
            SQL += " FROM KTKB_ChungTuChiTiet";
            SQL += " WHERE  iID_MaNguonNganSach=@iID_MaLoaiNganSach and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet AND iTrangThai =1 and iID_MaNamNganSach=@iID_MaNamNganSach and iNamLamViec =@NamLamViec";
            SQL += " GROUP By sLNS,sL,sK,sM,sTM,iNamLamViec,iThangCT,iID_MaNguonNganSach";
            SQL += " HAVING sum(rDTRut)<>0 or sum(rDTKhoiPhuc)<>0 or sum(rDTHuy)<>0";
            SQL += " UNION";
            SQL += " SELECT sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach";
            SQL += " ,TongSo = sum(case when  {0}  then rSoTien else 0 end)";
            SQL += " ,0 as rDTRutLK	,0 as rDTKhoiPhucLK	,0 as rDTHuyLK ,0 as rDTRut  ,0 as rDTKhoiPhuc  ,0 as rDTHuy ";
            SQL += " FROM  KT_RutDuToanChitiet WHERE iID_MaNamNganSach=@iID_MaNamNganSach and iNamLamViec =@NamLamViec AND iID_MaNguonNganSach=@iID_MaLoaiNganSach";
            SQL += " GROUP by sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach";
            SQL += " HAVING sum(rSoTien)<>0  )as bang";
            SQL += " GROUP by sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach";
            SQL += " HAVING  SUM(rDTRutLK)<>0 OR SUM(rDTKhoiPhucLK) <>0";
            SQL += " OR SUM(rDTHuyLK)<>0 OR SUM(rDTRut)<>0 OR SUM(rDTKhoiPhuc)<>0 OR SUM(rDTHuy)<>0 OR SUM(TongSo)<>0";
            
            SQL = String.Format(SQL, DKThang_Quy, DKThang, DKThang1);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaLoaiNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            if (LoaiThang_Quy.Equals("0"))
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable DoiChieuDuToan1(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String LoaiBaoCao, String inmuc, String iID_MaNguonNganSach, String UserID)
        {
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            String DKThang = "";
            String DKThang1 = "";
            DataTable dtCH = NguoiDungCauHinhModels.LayCauHinh(UserID);
            String iID_MaNamNganSach = dtCH.Rows.Count > 0 ? Convert.ToString(dtCH.Rows[0]["iID_MaNamNganSach"]) : "-1";

            SqlCommand cmd = new SqlCommand();
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (1,2,3) ";
                        DKThang = "iThangCT between 1 and 3";
                        DKThang1 = "iThangCT between 1 and 3";
                        break;
                    case 2: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (4,5,6) ";
                        DKThang = "iThangCT between 4 and 6";
                        DKThang1 = "iThangCT between 4 and 6";
                        break;
                    case 3: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (7,8,9) ";
                        DKThang = "iThangCT between 7 and 9";
                        DKThang1 = "iThangCT between 7 and 9";
                        break;
                    case 4: DKThang_Quy = "  MONTH(dNgayDotRutDuToan) in (10,11,12) ";
                        DKThang = "iThangCT between 10 and 12";
                        DKThang1 = "iThangCT between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThang_Quy = " MONTH(dNgayDotRutDuToan)<=@ThangQuy ";
                DKThang = "iThangCT<=@ThangQuy";
                DKThang1 = "iThangCT=@ThangQuy";
            }

            String SQL = " SELECT sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach,SUM(TongSo) TongSo";
            SQL += " ,sum(rDTRutLK) as rDTRutLK ,sum(rDTKhoiPhucLK) as rDTKhoiPhucLK,sum(rDTHuyLK) as rDTHuyLK";
            SQL += "  ,sum(rDTRut) as rDTRut ,sum(rDTKhoiPhuc) as rDTKhoiPhuc,sum(rDTHuy) as rDTHuy";
            SQL += " FROM( ";
            SQL += " SELECT sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach,0 as TongSo,";
            SQL += " rDTRutLK = sum(case when {1} then rDTRut else 0 end) ,";
            SQL += " rDTKhoiPhucLK = sum(case when {1} then rDTKhoiPhuc else 0 end ),";
            SQL += " rDTHuyLK = sum(case when {1} then rDTHuy   else 0 end),";
            SQL += "  rDTRut = sum(case when {2} then rDTRut else 0 end ),";
            SQL += "  rDTKhoiPhuc = sum(case when {2} then rDTKhoiPhuc else 0 end) ,";
            SQL += " rDTHuy = sum(case when {2} then rDTHuy   else 0 end)";
            SQL += " FROM KTKB_ChungTuChiTiet";
            SQL += " WHERE  iID_MaNguonNganSach=@iID_MaLoaiNganSach and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet AND iTrangThai =1 and iID_MaNamNganSach=@iID_MaNamNganSach and iNamLamViec =@NamLamViec";
            SQL += " GROUP By sLNS,sL,sK,sM,sTM,iNamLamViec,iThangCT,iID_MaNguonNganSach";
            SQL += " HAVING sum(rDTRut)<>0 or sum(rDTKhoiPhuc)<>0 or sum(rDTHuy)<>0";
            SQL += " UNION";
            SQL += " SELECT sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach";
            SQL += " ,TongSo = sum(case when  {0}  then rSoTien else 0 end)";
            SQL += " ,0 as rDTRutLK	,0 as rDTKhoiPhucLK	,0 as rDTHuyLK ,0 as rDTRut  ,0 as rDTKhoiPhuc  ,0 as rDTHuy ";
            SQL += " FROM  KT_RutDuToanChitiet WHERE iID_MaNamNganSach=@iID_MaNamNganSach and iNamLamViec =@NamLamViec AND iID_MaNguonNganSach=@iID_MaLoaiNganSach";
            SQL += " GROUP by sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach";
            SQL += " HAVING sum(rSoTien)<>0  )as bang";
            SQL += " GROUP by sLNS,sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach";
            SQL += " HAVING  SUM(rDTRutLK)<>0 OR SUM(rDTKhoiPhucLK) <>0";
            SQL += " OR SUM(rDTHuyLK)<>0 OR SUM(rDTRut)<>0 OR SUM(rDTKhoiPhuc)<>0 OR SUM(rDTHuy)<>0";

            SQL = String.Format(SQL, DKThang_Quy, DKThang, DKThang1);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaLoaiNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            if (LoaiThang_Quy.Equals("0"))
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "TH";
            R1[1] = "Phần 1 - Tổng hợp";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "CT";
            R2[1] = "Phần 2 - Chi tiết";
            return dt;
        }
        /// <summary>
        /// Danh sách loại ngân sách
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static DataTable Lay_LoaiNganSach(String UserID)
        {
            String SQL = String.Format(@"SELECT * FROM NS_NguonNganSach
                                        ORDER BY iID_MaNguonNganSach");
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iLoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="iThangQuy">Quý</param>
        /// <param name="iThang">Tháng</param>
        /// <returns></returns>
        public String obj_DSThangQuy(String ParentID, String iLoaiThang_Quy, String iThangQuy, String UserID)
        {
            String dsThangQuy = "";
            DataTable dtThangQuy = new DataTable();
            SelectOptionList slThangQuy;
            switch (iLoaiThang_Quy)
            {
                case "1":
                    dtThangQuy = DanhMucModels.DT_Quy(false);
                    if (String.IsNullOrEmpty(iThangQuy))
                    {
                        iThangQuy = "1";
                    }
                    slThangQuy = new SelectOptionList(dtThangQuy, "MaQuy", "TenQuy");
                    dsThangQuy = MyHtmlHelper.DropDownList(ParentID, slThangQuy, iThangQuy, "iThangQuy", "", "class=\"input1_2\" style=\"width: 55px; padding:2px; border-radius:2px; -webkit-border-radius:2px;border-color:#cecece; margin-right:0px; \" ");
                    break;
                case "0":
                    dtThangQuy = DanhMucModels.DT_Thang(false);
                    if (String.IsNullOrEmpty(iThangQuy))
                        iThangQuy = DanhMucModels.ThangLamViec(UserID).ToString();
                    slThangQuy = new SelectOptionList(dtThangQuy, "MaThang", "TenThang");
                    dsThangQuy = MyHtmlHelper.DropDownList(ParentID, slThangQuy, iThangQuy, "iThangQuy", "", "class=\"input1_2\" style=\"width: 55px; padding:2px; border-radius:2px; -webkit-border-radius:2px;border-color:#cecece;margin-right:0px; \" ");
                    break;
            }
            return dsThangQuy;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iLoaiThang_Quy">1: Quý | 0: Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iThang">Tháng</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsThangQuy(String ParentID, String iLoaiThang_Quy, String iThangQuy, String UserID)
        {
            return Json(obj_DSThangQuy(ParentID, iLoaiThang_Quy, iThangQuy, UserID), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy giá trị của trường
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaNguonNganSach"></param>
        /// <param name="UserID"></param>
        /// <param name="sParameter"></param>
        /// <returns></returns>
        public string getValues(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaNguonNganSach, String UserID,String sParameter)
        {
            String values = "";
            String DKThangQuy = "";
            if (LoaiThang_Quy.Equals("1"))
            {
                switch (Thang_Quy) { 
                    case "1":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 1 AND 3";
                        break;
                    case "2":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 4 AND 6";
                        break;
                    case "3":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 7 AND 9";
                        break;
                    case "4":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 10 AND 12";
                        break;
                }
            }
            else{
                DKThangQuy = "AND KT.iThangCT=@iThangCT";
            }
            DataTable dtCH = NguoiDungCauHinhModels.LayCauHinh(UserID);
            String iID_MaNamNganSach = dtCH.Rows.Count > 0 ? Convert.ToString(dtCH.Rows[0]["iID_MaNamNganSach"]) : "-1";
            String SQL = String.Format(@"SELECT TOP 1 KT.{0}
                                        FROM KTKB_ChungTuChiTiet KT
                                        WHERE KT.iTrangThai=1
                                          AND KT.iNamLamViec=@iNamLamViec
                                          {1}
                                          AND KT.iID_MaNamNganSach=@iID_MaNamNganSach
                                          AND KT.iID_MaNguonNganSach=@iID_MaNguonNganSach
                                          AND KT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet", sParameter, DKThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            if (LoaiThang_Quy.Equals("0"))
                cmd.Parameters.AddWithValue("@iThangCT", Thang_Quy);
            values = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return values;
        }
    }
}