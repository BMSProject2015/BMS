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
    public class rtprptDuToanChiNganSachQuocPhongXDCBCongTrinhPhoThongController : Controller
    {
        // Edited
        // GET: /DuToanChiNganSachQuocPhongXDCBCongTrinhPhoThong/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToanChiNganSachQuocPhongXDCBCongTrinhPhoThong.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rpDTCNS_XDCB_CongTrinhPhoThong.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
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
            String sPath = "/Libraries/DuToan/PDF";
            String path2 = Server.MapPath(sPath);
            HamChung.CreateDirectory(path2);
            NameFile = path2 + "" + FileName;
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// Tạo báo cáo
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
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rtprptDuToanChiNganSachQuocPhongXDCBCongTrinhPhoThong");
            LoadData(fr, iID_MaTrangThaiDuyet,MaND);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
            
        }

        //1030100
        /// <summary>
        /// Phụ lục 3c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_DuToanChiNganSachQuocPhongXDCBCongTrinhPhoThong(String iID_MaTrangThaiDuyet,String MaND)
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
            String SQL = string.Format(@"SELECT sM,sTM,sTTM,sNG,CTCT.sMoTa,SUM(rPhanCap) as rPhanCap ,SUM(rDuPhong) as rDuPhong,DV.sTen
                                            FROM DT_ChungTuChiTiet as CTCT
                                            INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON CTCT.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE sLNS='1030100' AND sL='460' AND sK='468' {0} {1}
                                            GROUP BY sM,sTM,sTTM,sNG,CTCT.sMoTa,DV.sTen
                                            HAVING SUM(rDuPhong)!=0 OR SUM(rPhanCap)!=0", ReportModels.DieuKien_NganSach(MaND,"CTCT"),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet,String MaND)
        {
            DataTable data = DT_DuToanChiNganSachQuocPhongXDCBCongTrinhPhoThong(iID_MaTrangThaiDuyet,MaND);
            data.TableName = "DTCNSQPConTrinhPhoThong";
            fr.AddTable("DTCNSQPConTrinhPhoThong", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa","sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sM", "sM,sMoTa","sM,sTM");
            fr.AddTable("Muc", dtMuc);
        }
        /// <summary>
        /// Xuất ra file PDF 
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ExportToPDF(String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(true, "AA");
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
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTCNSQP_XDCB_CongTrinhPhoThong.xls";
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