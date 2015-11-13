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


namespace VIETTEL.Report_Controllers.DuToan
{
    public class  rptDuToanChiNganSachSuDung_PhanNganSachNghiepVuController : Controller
    {
        // Edit: Nghiep
        // GET: /rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDTCNSSuDungPhanNghiepVu.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDTChiNganSachSuDungPhanNghiepVu.aspx";
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
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path,String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String  iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
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
            String tendv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN .Rows.Count>0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
           
             FlexCelReport fr = new FlexCelReport();
             fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu");
             LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND);
                fr.SetValue("TenDV", tendv);
                fr.SetValue("Nam", iNamLamViec); 
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("ngay", ngay);
                fr.Run(Result);
                return Result;
            
        }

        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            if (String.IsNullOrEmpty(ID)) return null;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String MaND)
        {
            DataTable data = DT_CNSSuDungNganSachNghiepVu(iID_MaTrangThaiDuyet, iID_MaDonVi,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            data.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND,String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, iID_MaTrangThaiDuyet, iID_MaDonVi);
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
        /// ViewPDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND,String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, iID_MaTrangThaiDuyet, iID_MaDonVi);
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
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND,String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, iID_MaTrangThaiDuyet, iID_MaDonVi);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String MaND)
        {
            return Json(obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Ajax lấy đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String MaND)
        {
            String dsDonVi = "";
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, "1020100",MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 50%\"");
            return dsDonVi;
        }

        public static DataTable DanhSachDonVi(String iID_MaTrangThaiDuyet, String sLNS, String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TenHT
                                        FROM ( SELECT DISTINCT iID_MaDonVi FROM DT_ChungTuChiTiet WHERE iTrangThai=1  {0}  {1}) a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["TenHT"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable DT_CNSSuDungNganSachNghiepVu(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND a.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            DataTable dt = new DataTable();
            String SQL = "SELECT b.iID_MaDonVi,b.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,SUM(rHienVat) as rHienVat ,SUM(rChiTapTrung) as rChiTapTrung,SUM(rTuChi) as rTuChi";
            SQL += " FROM DT_ChungTuChiTiet a";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi";
            SQL += " WHERE  a.iTrangThai = 1 {0} and sLNS IN (1020800, 1020000, 1020200)   {1}";
            SQL += " GROUP BY b.iID_MaDonVi,b.sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa HAVING SUM(rHienVat)<>0 OR SUM(rChiTapTrung)<>0 OR  SUM(rTuChi)<>0";
            SQL = string.Format(SQL, ReportModels.DieuKien_NganSach(MaND,"a"), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
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
