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
    public class rptDuToanNganSachNhaNuoc5aController : Controller
    {
        //
        // GET: /rptDuToanNganSachNhaNuoc5a/

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToanNganSachNhaNuoc5a.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToanNganSachNhaNuoc5a.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);                  
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];        
            String sNG = Request.Form[ParentID + "_sNG"];
            String sLNS = Request.Form[ParentID + "_sLNS"];
            return RedirectToAction("Index", new {sNG = sNG, sLNS = sLNS,iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// hàm khỏi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String sNG, String sLNS, String iID_MaTrangThaiDuyet)
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
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanNganSachNhaNuoc5a");
            DataTable dtNganh = Get_sNG();
            String TenNganh = "";
            for (int i = 0; i < dtNganh.Rows.Count;i++ )
            {
                if (sNG == dtNganh.Rows[i]["sTenKhoa"].ToString())
                {
                    TenNganh = dtNganh.Rows[i]["sTen"].ToString();
                    break;
                }
            }
            dtNganh.Dispose();

            String TenLNS = "";
            TenLNS = MoTaLNS_Cha(sLNS);
            LoadData(fr, MaND,sNG, sLNS,iID_MaTrangThaiDuyet);
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("LNS", sLNS);
                fr.SetValue("Nganh", TenNganh);
                fr.SetValue("TenLNS", TenLNS);
                fr.SetValue("PhuLuc", "Phụ Lục Số 5A");
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// DataTable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public static DataTable DT_DuToanNganSachNhaNuoc5a(String MaND, String sNG, String sLNS,String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL =String.Format(@"SELECT sLNS,sM,sL,sK,sTM,sTTM,sNG,sMoTa
                                    ,SUM(rTuChi) as rTuChi,SUM(rPhanCap) as rPhanCap,SUM(rDuPhong) as rDuPhong 
                                    FROM DT_ChungTuChiTiet
                                    WHERE iTrangThai=1 
                                    {0}
                                    AND sLNS=@sLNS
                                    AND sNG IN (SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@Nganh) 
                                    {1}
                                    GROUP BY sLNS,sM,sL,sK,sTM,sTTM,sNG,sMoTa",ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@Nganh", sNG);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu báo cáo      
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sNG, String sLNS,String iID_MaTrangThaiDuyet)
        {

            DataTable data = DT_DuToanNganSachNhaNuoc5a(MaND, sNG, sLNS,iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sLNS,sK,sM,sTM,sL,sMoTa", "slNS,sl,sk,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sM", "sLNS,sM,sK,sL,sMoTa", "slNS,sl,sk,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLNS;
            dtLNS = HamChung.SelectDistinct("Muc", dtMuc, "sLNS", "sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);
        }
        /// <summary>
        /// hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF( String sNG, String sLNS,String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sNG, sLNS, iID_MaTrangThaiDuyet);
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
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String sNG, String sLNS, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sNG, sLNS, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTNganSachNhaNuocGiao.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String sNG, String sLNS, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sNG, sLNS, iID_MaTrangThaiDuyet);
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
        /// DataTable lấy ngành
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_sNG()
        {
            DataTable dt = Lay_dtDanhMuc("Nganh");
            return dt;
        }
        public static DataTable Lay_dtDanhMuc(String TenLoaiDanhMuc)
        {
            String SQL = String.Format(@"SELECT iID_MaDanhMuc
                                       , DC_DanhMuc.sTen,DC_DanhMuc.sTenKhoa FROM DC_LoaiDanhMuc
                                       INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                                       WHERE DC_DanhMuc.bHoatDong=1
                                       AND DC_LoaiDanhMuc.sTenBang=N'{0}' ORDER BY sTenKhoa ", TenLoaiDanhMuc);
            return Connection.GetDataTable(SQL);
        }
         public static String MoTaLNS_Cha(String sLNS)
        {
            String SQL = String.Format(@"SELECT sLNS,sMoTa
                                        FROM NS_MucLucNganSach
                                        WHERE sLNS=SUBSTRING('{0}',1,3)",sLNS);
            DataTable dt = Connection.GetDataTable(SQL);
            String MoTa = "";
            if (dt.Rows.Count > 0)
                MoTa = dt.Rows[0]["sMota"].ToString();
            return MoTa;
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
