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
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;


namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_35FController : Controller
    {      
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_35F.xls";
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
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_35F.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }
        /// <summary>
        /// Hàm EditSubmit nhận giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_35F.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
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
             String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
             String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
             String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_35F");
                LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.Run(Result);
                return Result;            
        }
        /// <summary>
        /// Lấy dữ liệu Fill ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String MaND)
        {
            DataTable data = QT_ThuongXuyen_35F(iID_MaTrangThaiDuyet, iID_MaDonVi,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi);
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
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, iID_MaDonVi);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_35F.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi);
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
        public JsonResult Ds_DonVi(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DonVi(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String MaND)
        {
            String input = "";
            DataTable dt = Lay_DSDonVi(iID_MaTrangThaiDuyet,MaND);
            StringBuilder stbDonVi = new StringBuilder();
            stbDonVi.Append("<div style=\"width: 99%; height: 150px; overflow: scroll; border:1px solid black;\">");
            stbDonVi.Append("<table class=\"mGrid\">");
            stbDonVi.Append("<tr>");
            stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

            String TenDonVi = "", MaDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dt.Rows[i - 1]["iID_MaDonVi"]);
                TenDonVi = Convert.ToString(dt.Rows[i - 1]["sTen"]);
                _Checked = "";
                for (int j = 1; j <= arrDonVi.Length; j++)
                {
                    if (MaDonVi == arrDonVi[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" />", MaDonVi, _Checked);
                stbDonVi.Append("<tr>");
                stbDonVi.Append("<td style=\"width: 15%;\">");
                stbDonVi.Append(input);
                stbDonVi.Append("</td>");
                stbDonVi.Append("<td>" + TenDonVi + "</td>");

                stbDonVi.Append("</tr>");
            }
            stbDonVi.Append("</table>");
            stbDonVi.Append("</div>");
            dt.Dispose();
            String DonVi = stbDonVi.ToString();
            return DonVi;
        }
        /// <summary>
        /// Lấy danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable Lay_DSDonVi(String iID_MaTrangThaiDuyet,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT  a.iID_MaDonVi,sTen
                                        FROM (SELECT DISTINCT iID_MaDonVi
                                        FROM QTA_ChungTuChiTiet WHERE iTrangThai=1 AND sLNS='1010000' {0} {1}) as a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON a.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        ORDER BY a.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// datatable quyết toán thường xuyên 35F
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable QT_ThuongXuyen_35F(String iID_MaTrangThaiDuyet, String iID_MaDonVi,String MaND)
        {
            String[] arrDonvi = iID_MaDonVi.Split(',');
            String DK = "";
            for (int i = 0; i < arrDonvi.Length; i++)
            {
                DK += "iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonvi.Length - 1)
                    DK += " OR ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT iID_MaDonVi,sTen,SUM(Thang1) as Thang1,SUM(Thang2) as Thang2,SUM(Thang3) as Thang3,SUM(Thang4) as Thang4
										,SUM(Thang5) as Thang5,SUM(Thang6) as Thang6,SUM(Thang7) as Thang7,SUM(Thang8) as Thang8,SUM(Thang9) as Thang9,SUM(Thang10) as Thang10
										,SUM(Thang11) as Thang11,SUM(Thang12) as Thang12
										FROM(
										SELECT QTG.iID_MaDonVi,sTen
										,Thang1=CASE WHEN iThang_Quy=1 THEN SUM(rTuChi) ELSE 0 END
										,Thang2=CASE WHEN iThang_Quy=2 THEN SUM(rTuChi) ELSE 0 END
										,Thang3=CASE WHEN iThang_Quy=3 THEN SUM(rTuChi) ELSE 0 END
										,Thang4=CASE WHEN iThang_Quy=4 THEN SUM(rTuChi) ELSE 0 END
										,Thang5=CASE WHEN iThang_Quy=5 THEN SUM(rTuChi) ELSE 0 END
										,Thang6=CASE WHEN iThang_Quy=6 THEN SUM(rTuChi) ELSE 0 END
										,Thang7=CASE WHEN iThang_Quy=7 THEN SUM(rTuChi) ELSE 0 END
										,Thang8=CASE WHEN iThang_Quy=8 THEN SUM(rTuChi) ELSE 0 END
										,Thang9=CASE WHEN iThang_Quy=9 THEN SUM(rTuChi) ELSE 0 END
										,Thang10=CASE WHEN iThang_Quy=10 THEN SUM(rTuChi) ELSE 0 END
										,Thang11=CASE WHEN iThang_Quy=11 THEN SUM(rTuChi) ELSE 0 END
										,Thang12=CASE WHEN iThang_Quy=12 THEN SUM(rTuChi) ELSE 0 END
                                        FROM (SELECT iID_MaDonVi,rTuChi,iThang_Quy FROM QTA_ChungTuChiTiet
												WHERE ({0}) AND sNG<>''AND sLNS='1010000' AND sL='460' AND sK='468' AND iTrangThai=1 {1} {2}) as QTG                                
                                        INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON QTG.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        GROUP BY QTG.iID_MaDonVi,sTen,iThang_Quy) as a
                                        GROUP BY iID_MaDonVi,sTen", DK,ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i = 0; i < arrDonvi.Length;i++ )
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonvi[i]);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
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
