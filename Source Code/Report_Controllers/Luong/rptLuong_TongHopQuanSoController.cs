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
    public class rptLuong_TongHopQuanSoController : Controller
    {
        // Edit: Thương
        // GET: /rptLuong_TongHopQuanSo/
        public string sViewPath = "~/Report_Views/";        
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_TongHopQuanSo.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;                        
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThangLuong"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNamLuong"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_divReport"]);
            String iMaDV = Convert.ToString(Request.Form["iID_MaDonVi"]);
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iNamLuong"] = Nam;
            ViewData["iThangLuong"] = Thang;                     
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iMaDV"] = iMaDV;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_TongHopQuanSo.aspx";
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":                    
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo.xls";                    
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo_A3.xls";
                    break;
            }
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Thang">Tháng lương</param>
        /// <param name="Nam">Năm lương</param>
        /// <param name="TrangThai">Trạng thái duyệt</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable Luong_TongHopQuanSo(String Thang, String Nam, String TrangThai, String iMaDV)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String DKDonvi = "AND L.iID_MaDonVi IN(";
            String[] arrDonvi = iMaDV.Split(',');
            for (int i = 0; i < arrDonvi.Length; i++)
            {
                DKDonvi += "@iID_MaDonVi" + i;
                if (i < arrDonvi.Length - 1)
                    DKDonvi += ",";
            }
            DKDonvi += ")";
            String SQL = String.Format(@"SELECT CONVERT(VARCHAR,ROW_NUMBER() OVER (ORDER BY L.iID_MaDonVi))+' - '+DV.sTen AS TenDV
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='11' THEN 1 END) ThieuUy --Thiếu úy
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='12' THEN 1 END) TrungUy--Trung úy
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='13' THEN 1 END) ThuongUy--Thượng úy
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='14' THEN 1 END) DaiUy--Đại úy
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='21' THEN 1 END) ThieuTa--Thiếu tá
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='22' THEN 1 END) TrungTa--Trung tá
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='23' THEN 1 END) ThuongTa--Thượng tá
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='24' THEN 1 END) DaiTa--Đại tá
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2) IN (31,32,33,34) THEN 1 END) Tuong--Tướng
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=1 THEN 1 END) TSyQuan--Tổng sỹ quan
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=4 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='01' THEN 1 END) B2--Binh nhì
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=4 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='02' THEN 1 END) B1--Binh nhất
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=4 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='03' THEN 1 END) H1--Hạ sỹ
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=4 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='04' THEN 1 END) H2--Trung sỹ
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=4 AND SUBSTRING(L.iID_MaBacLuong_CanBo,1,2)='05' THEN 1 END) H3--Thượng sỹ
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=4 THEN 1 END) THaSyQuan--Tổng hạ sỹ quan
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=2 THEN 1 END) QNCN--Quân nhân chuyên nghiệp
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=3 THEN 1 END) CNV--Công nhân viên quốc phòng
	                                          ,COUNT(case when L.iID_MaNgachLuong_CanBo=5 THEN 1 END) HD--Hợp đồng
                                        FROM L_BangLuongChiTiet AS L
                                        INNER JOIN NS_DonVi AS DV
                                        ON DV.iID_MaDonVi=L.iID_MaDonVi
                                        WHERE L.iTrangThai=1
                                        AND L.iThangBangLuong=@iThangBangLuong
                                        AND L.iNamBangLuong=@iNamBangLuong
                                        {1}
                                        {0}
                                        GROUP BY L.iID_MaDonVi,DV.sTen
                                        ORDER BY l.iID_MaDonVi
                                        ", DKDuyet,DKDonvi);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            }
            for (int j = 0; j < arrDonvi.Length; j++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + j, arrDonvi[j]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="Thang">Tháng lương</param>
        /// <param name="Nam">Năm lương</param>
        /// <param name="TrangThai">Trạng thái duyệt</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String Thang, String Nam, String TrangThai, String iMaDV)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            //Thêm chữ ký vào báo cáo
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_TongHopQuanSo");
            LoadData(fr, Thang,Nam,TrangThai,iMaDV);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("ThangNam", "Tháng " + Thang + " / " + Nam);
            fr.Run(Result);
            return Result;
        }        
        
        private void LoadData(FlexCelReport fr, String Thang, String Nam, String TrangThai, String iMaDV)
        {
            DataTable dt = Luong_TongHopQuanSo(Thang, Nam, TrangThai, iMaDV);
            dt.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", dt);
            dt.Dispose();
        }

        public clsExcelResult ExportToPDF(String Thang, String Nam, String TrangThai, String iMaDV, String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";            
            switch (KhoGiay)
            {
                case "A4":                    
                        sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo.xls";                    
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo_A3.xls";
                    break;
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),Thang,Nam,TrangThai,iMaDV );
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
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Thang, String Nam, String TrangThai, String iMaDV, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo.xls";
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo_A3.xls";
                    break;
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang, Nam, TrangThai,iMaDV);
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
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Thang, String Nam, String TrangThai, String iMaDV, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo.xls";
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopQuanSo_A3.xls";
                    break;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang, Nam, TrangThai, iMaDV);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHop_QuanSo.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy danh sách đơn vị
        /// </summary>
        /// <param name="NamLuong">Năm lương</param>
        /// <param name="ThangLuong">Tháng lương</param>
        /// <param name="DuyetLuong">Trạng thái duyệt</param>
        /// <returns></returns>
        public static DataTable GetDonVi(String NamLuong, String ThangLuong, String DuyetLuong)
        {
            String DKDuyet = DuyetLuong.Equals("0") ? "" : "AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            String SQL = String.Format(@"SELECT L.iID_MaDonVi,L.sTenDonVi AS TenHT
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                          AND L.iNamBangLuong=@iNamBangLuong
                                          AND L.iThangBangLuong=@iThangBangLuong
                                          {0}
                                        GROUP BY L.iID_MaDonVi,L.sTenDonVi", DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", NamLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", ThangLuong);
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dtDonvi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonvi;
        }
        /// <summary>
        /// Hiện thị danh sách đơn vị lên trình duyệt
        /// </summary>
        /// <param name="NamLuong">Năm lương</param>
        /// <param name="ThangLuong">Tháng lương</param>
        /// <param name="DuyetLuong">Trạng thái duyệt</param>
        /// <param name="arrDV">Danh sách mã đơn vị</param>
        /// <returns></returns>
        public String obj_DSDonVi(String NamLuong, String ThangLuong, String DuyetLuong, String[] arrDV)
        {
            DataTable dt = GetDonVi(NamLuong, ThangLuong, DuyetLuong);
            String stbDonVi = "<table class=\"mGrid\">";
            String TenDV = ""; String idDV = "";
            String _Checked1 = "checked=\"checked\"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Checked1 = "";
                TenDV = Convert.ToString(dt.Rows[i]["TenHT"]);
                idDV = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < arrDV.Length; j++)
                {
                    if (idDV == arrDV[j])
                    {
                        _Checked1 = "checked=\"checked\"";
                        break;
                    }
                }
                stbDonVi += "<tr style=\" height: 20px; font-size: 12px; \"><td style=\"width: 20px; text-align:center; height:auto; line-height:7px;\">";
                stbDonVi += "<input type=\"checkbox\" value=\"" + idDV + "\"" + _Checked1 + " check-group=\"iID_MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" style=\"cursor:pointer;\" onchange=\"ChonCB()\" />";
                stbDonVi += "</td><td>" + TenDV + "</td></tr>";
            }
            stbDonVi += "</table>";
            dt.Dispose();
            return stbDonVi;
        }
        /// <summary>
        /// Ajax load danh sách đơn vị
        /// </summary>
        /// <param name="NamLuong">Năm lương</param>
        /// <param name="ThangLuong">Tháng lương</param>
        /// <param name="DuyetLuong">Trạng thái duyệt</param>
        /// <param name="arrDV">Danh sách mã đơn vị</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ds_DonVi(String NamLuong, String ThangLuong, String DuyetLuong, String[] arrDV)
        {
            return Json(obj_DSDonVi(NamLuong, ThangLuong, DuyetLuong, arrDV), JsonRequestBehavior.AllowGet);
        }
    }
}
