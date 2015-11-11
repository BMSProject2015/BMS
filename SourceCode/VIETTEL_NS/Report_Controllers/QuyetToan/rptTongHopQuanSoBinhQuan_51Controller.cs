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
using DomainModel.Controls;
namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptTongHopQuanSoBinhQuan_51Controller : Controller
    {
        // created: Huyền Lê
        // GET: /rptTongHopQuanSoBinhQuan_51/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptTongHopQuanSoBinhQuan_51.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptTongHopQuanSoBinhQuan_51.aspx";
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iMaDV = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iAll = Convert.ToString(Request.Form[ParentID + "_iALL"]);
            ViewData["pageload"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iThang"] = iThang;
            ViewData["iMaDV"] = iMaDV;
            ViewData["iAll"] = iAll;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptTongHopQuanSoBinhQuan_51.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
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
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(3);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTongHopQuanSoBinhQuan_51");
            LoadData(fr, iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("ngay", ngay);
            fr.SetValue("cap1", BoQuocPhong);
            fr.SetValue("cap2", QuanKhu);
            switch (iAll)
            {
                case "on":
                    fr.SetValue("TenDV", "Tổng hợp");
                    break;
                case "off":
                    if (iMaDV == Guid.Empty.ToString())
                        fr.SetValue("TenDV", "");
                    else
                        fr.SetValue("TenDV", CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iMaDV, "sTen"));
                    break;
            }
            fr.SetValue("Temp", iThang);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaTrangThaiDuyet, iAll, iMaDV);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaTrangThaiDuyet, iAll, iMaDV);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "THQS_BinhQuan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaTrangThaiDuyet, iAll, iMaDV);
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
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            //Quân số đến tháng
            DataTable data = QuyetToanQuanSo(iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable QuyetToanQuanSo(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            String SQL = String.Format(@"SELECT N'Tháng '+ convert(varchar,QS.iThang_Quy) as iThang
	                                            --Sỹ quan
                                                ,SUM(QS.rThieuUy) rThieuUy,SUM(QS.rTrungUy) rTrungUy,SUM(QS.rThuongUy) rThuongUy,SUM(QS.rDaiUy) rDaiUy
                                                ,SUM(QS.rThieuTa) rThieuTa,SUM(QS.rTrungTa) rTrungTa,SUM(QS.rThuongTa) rThuongTa,SUM(QS.rDaiTa) rDaiTa
                                                ,SUM(QS.rTuong) rTuong
                                                --Hạ sỹ quan
                                                ,SUM(QS.rTSQ) rTSQ
                                                ,SUM(QS.rBinhNhi) rBinhNhi,SUM(QS.rBinhNhat) rBinhNhat
                                                ,SUM(QS.rHaSi) rHaSi,SUM(QS.rTrungSi) rTrungSi,SUM(QS.rThuongSi) rThuongSi
                                                --quân nhân chuyên nghiệp
                                                ,SUM(QS.rQNCN) rQNCN,SUM(QS.rCNVQPCT) rCNVQPCT,SUM(QS.rQNVQPHD) rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet QS
                                        WHERE QS.iTrangThai=1 
                                            {1}
                                            AND QS.bLoaiThang_Quy=0
                                            AND QS.iThang_Quy BETWEEN 1 AND @iThang_Quy
                                            {0}
                                            {2}
                                            AND SUBSTRING(QS.sKyHieu,1,1)='7'
                                        GROUP BY QS.iThang_Quy", DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị theo năm
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public static DataTable GetDonvi(String iThang, String iID_MaTrangThaiDuyet,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.iID_MaDonVi+' - '+DV.sTen AS sTen
                                        FROM NS_DonVi AS DV
                                        WHERE DV.iTrangThai=1
                                          AND DV.iID_MaDonVi IN(SELECT QS.iID_MaDonVi
                                                                FROM QTQS_ChungTuChiTiet QS
                                                                WHERE QS.bLoaiThang_Quy=0
                                                                AND QS.iTrangThai=1 
                                                                AND QS.iThang_Quy BETWEEN 1 AND @iThang_Quy
                                                               {1}
                                                                {0}
                                                                GROUP BY QS.iID_MaDonVi) order by DV.iID_MaDonVi", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public String obj_DSDonvi(String ParentID, String iThang, String iID_MaTrangThaiDuyet, String iMaDV,String MaND)
        {
            String dsDV = "";
            DataTable dtDonvi = GetDonvi(iThang, iID_MaTrangThaiDuyet,MaND);
            SelectOptionList slDonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            dsDV = MyHtmlHelper.DropDownList(ParentID, slDonvi, iMaDV, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 140px; padding:2px;\"");
            return dsDV;
        }
        /// <summary>
        /// Hàm Ajax load danh sách đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonvi(string ParentID, String iThang, String iID_MaTrangThaiDuyet, String iMaDV)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonvi(ParentID, iThang, iID_MaTrangThaiDuyet, iMaDV,MaND), JsonRequestBehavior.AllowGet);
        }
        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(" AND (QS.iNamLamViec={0} AND QS.iID_MaNamNganSach={1} AND QS.iID_MaNguonNganSach={2})", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
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