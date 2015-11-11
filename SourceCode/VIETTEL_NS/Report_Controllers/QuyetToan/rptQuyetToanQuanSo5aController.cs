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
    public class rptQuyetToanQuanSo5aController : Controller
    {
        // Created: Huyền Lê
        // Edit: Thương
        // GET: /rptQuyetToanQuanSo5a/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToanQuanSo5a.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["pageload"] = "0";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToanQuanSo5a.aspx";
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
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang_Quy"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iAll = Convert.ToString(Request.Form[ParentID + "_iALL"]);
            ViewData["pageload"] = "1";
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iMaDV"] = iID_MaDonVi;
            ViewData["iAll"] = iAll;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToanQuanSo5a.aspx";
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
            HamChung.Language();
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToanQuanSo5a");
            LoadData(fr, iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
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
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("CucTaiChinh", ReportModels.CauHinhTenDonViSuDung(2));
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
                    clsResult.FileName = "QS_Tang_Giam_Thang.pdf";
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
                clsResult.FileName = "QS_Tang_Giam_Thang.xls";
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
        /// Đẩy dữ liệu xuống báo cáo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang">Tháng quân số</param>
        /// <param name="iNam">Năm quân số</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>   
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable data = QuyetToanQuanSo(iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Lấy thông tin quân số tăng giảm
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
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            //Lấy thông tin quân số của 1 đơn vi hoặc tất cả đơn vị đến tháng
            String DKDonvi = iAll.Equals("on") ? "" : "AND iID_MaDonVi=@iID_MaDonVi";
            SqlCommand cmd = new SqlCommand();
            String SQL = "Select sKyHieu";
            SQL += ",LaHangCha=CASE WHEN bLaHangCha='TRUE' THEN 1 ELSE 0 END";
            SQL += ", Sum(rThieuUy) as rThieuUy";
            SQL += ",Sum(rTrungUy) as rTrungUy";
            SQL += ",Sum(rThuongUy) as rThuongUy";
            SQL += ",Sum(rDaiUy) as rDaiUy";
            SQL += ",Sum(rThieuTa) as rThieuTa";
            SQL += ",Sum(rTrungTa) as rTrungTa";
            SQL += ",Sum(rThuongTa) as rThuongTa";
            SQL += ",Sum(rDaiTa) as rDaiTa";
            SQL += ",Sum(rTuong) as rTuong";
            SQL += ",Sum(rTSQ) as rTSQ";
            SQL += ",Sum(rBinhNhi) as rBinhNhi";
            SQL += ",Sum(rBinhNhat) as rBinhNhat";
            SQL += ",Sum(rHaSi) as rHaSi";
            SQL += ",Sum(rTrungSi) as rTrungSi";
            SQL += ",Sum(rThuongSi) as rThuongSi";
            SQL += ",Sum(rQNCN) as rQNCN";
            SQL += ",Sum(rCNVQPCT) as rCNVQPCT";
            SQL += ",Sum(rQNVQPHD) as rQNVQPHD";
            SQL += " From QTQS_ChungTuChiTiet";
            SQL += "  Where iTrangThai= 1";
            SQL += " AND bLoaiThang_Quy = 0";
            SQL += " AND iThang_Quy BETWEEN 1 AND @iThang_Quy";
            SQL += " {1}";
            SQL += " AND SUBSTRING (sKyHieu,1,1) IN (2,3)";
            SQL += " {2} ";
            SQL += " {0} ";
            SQL += " Group by sKyHieu,bLaHangCha ";
            SQL += " Order by sKyHieu ";
            SQL = String.Format(SQL, DKDonvi,ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            cmd.CommandText = SQL;
           
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //Lấy danh sách mô tả quân số
            SQL = "";
            SQL = " Select sKyHieu,LaHangCha=CASE WHEN bLaHangCha='TRUE' THEN 1 ELSE 0 END, sMoTa";
            SQL += ",rThieuUy=0";
            SQL += ",rTrungUy =0";
            SQL += ",rThuongUy=0";
            SQL += ",rDaiUy=0";
            SQL += ",rThieuTa=0";
            SQL += ",rTrungTa=0";
            SQL += ",rThuongTa=0";
            SQL += ",rDaiTa=0";
            SQL += ",rTuong=0";
            SQL += ",rTSQ=0";
            SQL += ",rBinhNhi=0";
            SQL += ",rBinhNhat=0";
            SQL += ",rHaSi=0";
            SQL += ",rTrungSi=0";
            SQL += ",rThuongSi=0";
            SQL += ",rQNCN=0";
            SQL += ",rCNVQPCT=0";
            SQL += ",rQNVQPHD=0";
            SQL += " FROM NS_MucLucQuanSo";
            SQL += " WHERE SUBSTRING (sKyHieu,1,1) IN(2,3)";
            SQL += " ORDER BY sKyHieu ";
            DataTable dtKQ = Connection.GetDataTable(SQL);
            //Ghép 2 datatable
            DataRow R1, R2;
            for (int i = 0; i < dtKQ.Rows.Count; i++)
            {
                R1 = dtKQ.Rows[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    R2 = dt.Rows[j];
                    if (R1["sKyHieu"].Equals(R2["sKyHieu"]))
                    {
                        for (int c = 1; c < dt.Columns.Count; c++)
                        {
                            if (dt.Columns[c].ColumnName == dtKQ.Columns[c + 1].ColumnName)
                            {
                                dtKQ.Rows[i][c + 1] = dt.Rows[j][c];
                            }
                        }
                        break;
                    }
                }
            }
            dt.Dispose();
            return dtKQ;
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
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.iID_MaDonVi+' - ' + DV.sTen AS sTen
                                        FROM NS_DonVi AS DV
                                        WHERE DV.iTrangThai=1
                                          AND DV.iID_MaDonVi IN(SELECT QS.iID_MaDonVi
                                                                FROM QTQS_ChungTuChiTiet QS
                                                                WHERE QS.bLoaiThang_Quy=0
                                                                AND QS.iTrangThai=1 
                                                                AND QS.iThang_Quy BETWEEN 1 AND @iThang_Quy
                                                                {0} {1}
                                                                GROUP BY QS.iID_MaDonVi ) order by DV.iID_MaDonVi", DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
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
