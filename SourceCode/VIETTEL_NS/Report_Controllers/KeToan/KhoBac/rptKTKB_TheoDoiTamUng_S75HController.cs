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
using System.Globalization;
using DomainModel.Controls;
namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKB_TheoDoiTamUng_S75HController : Controller
    {
        // Create: Thương
        // GET: /rptKTKB_TheoDoiTamUng_S75H/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_TheoDoiTamUng.xls";
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["pageload"] = 0;
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_TheoDoiTamUng_S75H.aspx";
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
            ViewData["pageload"] = 1;
            //Lấy thông tin truy vấn trên Form
            String iLoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_iLoaiThang_Quy"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String iNguonNS = Convert.ToString(Request.Form[ParentID + "_iNguonNS"]);
            String iMuc_TieuMuc = Convert.ToString(Request.Form[ParentID + "_iMuc_TieuMuc"]);
            String iTrangThai = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            //String iNam = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            //Lưu thông tin truy vấn
            ViewData["iLoaiThang_Quy"] = iLoaiThang_Quy;
            ViewData["iThang"] = iThang;
            ViewData["iQuy"] = iQuy;
            ViewData["iNguonNS"] = iNguonNS;
            ViewData["iMuc_TieuMuc"] = iMuc_TieuMuc;
            ViewData["iID_MaTrangThaiDuyet"] = iTrangThai;
            //ViewData["iNam"] = iNam;
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_TheoDoiTamUng_S75H.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");  
        }
        /// <summary>
        /// Theo dõi tạm ứng
        /// </summary>
        /// <param name="iLoaiThang_Quy">0: Chọn tháng | 1: Chọn quý</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNguonNS">Nguồn ngân sách</param>
        /// <param name="iMuc_TieuMuc">In đến mức: Mục | Tiểu mục</param>
        /// <param name="MaND">Năm</param>
        /// <returns>DataTable</returns>
        public DataTable GetTamUng(String iLoaiThang_Quy, String iThang, String iQuy, String iNguonNS, String iMuc_TieuMuc,String MaND,String iTrangThai)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows.Count>0? Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]):"-1";
            String iNamNganSach = dtCauHinh.Rows.Count>0? Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]):"-1";
            dtCauHinh.Dispose();
            DataTable dtResult = new DataTable();
            String DKThangQuy = "";
            String DKDuyet = iTrangThai.Equals("0") ? "" : "AND KTKB.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            if (iLoaiThang_Quy.Equals("1"))
            {
                switch (iQuy)
                {
                    case "1":
                        DKThangQuy = "AND KTKB.iThang BETWEEN 1 AND 3";
                        break;
                    case "2":
                        DKThangQuy = "AND KTKB.iThang BETWEEN 4 AND 6";
                        break;
                    case "3":
                        DKThangQuy = "AND KTKB.iThang BETWEEN 7 AND 9";
                        break;
                    case "4":
                        DKThangQuy = "AND KTKB.iThang BETWEEN 10 AND 12";
                        break;
                }
            }
            else
                DKThangQuy = "AND KTKB.iThang=@iThang";
            String DKIn = iMuc_TieuMuc.Equals("rTM") ? ",KTKB.sTM" : "";
            String SQL = String.Format(@"SELECT --Ghi sổ ngày tháng			
			                                    KTKB.sM
			                                    {1}
			                                    ,CONVERT(VARCHAR,KTKB.iNgay)+'-'+ CONVERT(VARCHAR,KTKB.iThang) GS
			                                    --chứng từ
			                                    ,KTKB.sSoChungTuGhiSo
			                                    ,CONVERT(VARCHAR,KTKB.iNgayCT)+'-'+CONVERT(VARCHAR,KTKB.iThangCT) CT
			                                    ,KTKB.sNoiDung
			                                    ,SUM(KTKB.rSoTien) rSoTien
			                                    ,CONVERT(VARCHAR,KTKB.sGhiChu) AS sGhiChu
                                        FROM KTKB_ChungTuChiTiet KTKB
                                        WHERE KTKB.iTrangThai=1
                                           AND KTKB.iLoai=2
	                                       {0}
	                                       AND KTKB.iNamLamViec=@iNamLamViec
	                                       AND KTKB.iID_MaNguonNganSach=@iID_MaNguonNganSach
                                           AND KTKB.iID_MaNamNganSach=@iID_MaNamNganSach
	                                       {2}
                                        GROUP BY KTKB.sM
                                            {1}
                                            ,KTKB.sNoiDung
                                            ,CONVERT(VARCHAR,KTKB.sGhiChu)
                                            ,CONVERT(VARCHAR,KTKB.iNgay)+'-'+ CONVERT(VARCHAR,KTKB.iThang)
                                            ,CONVERT(VARCHAR,KTKB.iNgayCT)+'-'+CONVERT(VARCHAR,KTKB.iThangCT)
                                            ,KTKB.sSoChungTuGhiSo", DKThangQuy,DKIn,DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iNamNganSach);
            if(!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iNguonNS);
            if(iLoaiThang_Quy.Equals("0"))
                cmd.Parameters.AddWithValue("@iThang",iThang);
            dtResult = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtResult;
        }
        /// <summary>
        /// Lũy kế
        /// </summary>
        /// <param name="iLoaiThang_Quy">0: Chọn tháng | 1: Chọn quý</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNguonNS">Nguồn ngân sách</param>
        /// <param name="iMuc_TieuMuc">In đến mức: Mục | Tiểu mục</param>
        /// <param name="MaND">Năm</param>
        /// <returns></returns>
        public DataTable Get_LuyKe(String iLoaiThang_Quy, String iThang, String iQuy, String iNguonNS, String iMuc_TieuMuc, String MaND, String iTrangThai)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]) : "-1";
            String iNamNganSach = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]) : "-1";
            dtCauHinh.Dispose();
            DataTable dtLuyKe = new DataTable();
            String DKThangQuy = "";
            String DKDuyet = iTrangThai.Equals("0") ? "" : "AND KT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            if (iLoaiThang_Quy.Equals("1"))
            {
                switch (iQuy)
                {
                    case "1":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 1 AND 3";
                        break;
                    case "2":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 1 AND 6";
                        break;
                    case "3":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 1 AND 9";
                        break;
                    case "4":
                        DKThangQuy = "AND KT.iThangCT BETWEEN 1 AND 12";
                        break;
                }
            }
            else
                DKThangQuy = "AND KT.iThangCT<=@iThang";
            String SQL = String.Format(@"SELECT SUM(KT.rSoTien) rSoTien
                                        FROM KTKB_ChungTuChiTiet KT
                                        WHERE KT.iTrangThai=1 
                                          {0}
                                          AND KT.iID_MaNguonNganSach=@iID_MaNguonNganSach
                                          AND KT.iLoai=2
                                          AND KT.iNamLamViec=@iNamLamViec
                                          AND KT.iID_MaNamNganSach=@iID_MaNamNganSach
                                          {1}", DKThangQuy,DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iNguonNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iNamNganSach);
            if(!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            if(iLoaiThang_Quy.Equals("0"))
                cmd.Parameters.AddWithValue("@iThang",iThang);
            dtLuyKe=Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtLuyKe;
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iLoaiThang_Quy">0: Chọn tháng | 1: Chọn quý</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNguonNS">Nguồn ngân sách</param>
        /// <param name="iMuc_TieuMuc">In đến mức: Mục | Tiểu mục</param>
        /// <param name="iNam">Năm</param>
        public void LoadData(FlexCelReport fr, String iLoaiThang_Quy, String iThang, String iQuy, String iNguonNS, String iMuc_TieuMuc, String iNam, String iTrangThai)
        {
            DataTable dtResult = GetTamUng(iLoaiThang_Quy, iThang, iQuy, iNguonNS, iMuc_TieuMuc, iNam,iTrangThai);
            fr.AddTable("ChiTiet", dtResult);
            dtResult.Dispose();
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file báo cáo</param>
        /// <param name="iLoaiThang_Quy">0: Chọn tháng | 1: Chọn quý</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNguonNS">Nguồn ngân sách</param>
        /// <param name="iMuc_TieuMuc">In đến mức: Mục | Tiểu mục</param>
        /// <param name="MaND">Năm</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iLoaiThang_Quy, String iThang, String iQuy, String iNguonNS, String iMuc_TieuMuc, String MaND, String iTrangThai)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]) : "-1";
            //String iNamNganSach = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]) : "-1";           
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_TheoDoiTamUng_S75H");
            LoadData(fr, iLoaiThang_Quy, iThang, iQuy, iNguonNS, iMuc_TieuMuc, MaND,iTrangThai);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            long LK = 0;
            DataTable dtLK = Get_LuyKe(iLoaiThang_Quy, iThang, iQuy, iNguonNS, iMuc_TieuMuc, MaND,iTrangThai);
            if (dtLK.Rows.Count > 0)
                for (int i = 0; i < dtLK.Rows.Count; i++)
                    LK += dtLK.Rows[i]["rSoTien"].Equals("") ? 0 : long.Parse(dtLK.Rows[i]["rSoTien"].ToString());
            fr.SetValue("LK_TU", LK);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", iNamLamViec);
            if (iLoaiThang_Quy.Equals("1"))
                fr.SetValue("ThangQuy", "Quý " + iQuy + " năm " + iNamLamViec);
            else
                fr.SetValue("ThangQuy", "Tháng " + iThang + " năm " + iNamLamViec);
            String SoDu = "Số dư đầu kỳ";
            switch(iLoaiThang_Quy)
            {
                case "0":
                    if (iThang.Equals("1")) 
                         SoDu="Số năm trước chuyển sang";
                    break;
                case "1":
                    if (iQuy.Equals("1"))
                        SoDu = "Số năm trước chuyển sang";
                    break;
            }
            fr.SetValue("SoDu", SoDu);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>        
        /// <param name="iLoaiThang_Quy">0: Chọn tháng | 1: Chọn quý</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNguonNS">Nguồn ngân sách</param>
        /// <param name="iMuc_TieuMuc">In đến mức: Mục | Tiểu mục</param>
        /// <param name="MaND">Năm</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iLoaiThang_Quy, String iThang, String iQuy, String iNguonNS, String iMuc_TieuMuc, String MaND, String iTrangThai)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iLoaiThang_Quy, iThang, iQuy, iNguonNS, iMuc_TieuMuc, MaND,iTrangThai);
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
        /// Danh sách nguồn ngân sách
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetNguonNS(String MaND,String iTrangThai)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]) : "-1";
            String iNamNganSach = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]) : "-1";
            String DKDuyet = iTrangThai.Equals("0") ? "" : "AND KTKB.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            dtCauHinh.Dispose();
            DataTable dtNS = new DataTable();
            String SQL = String.Format(@"SELECT NS.iID_MaNguonNganSach,NS.sTen sTen
                                        FROM NS_NguonNganSach NS
                                        WHERE NS.iTrangThai=1
                                          AND NS.iID_MaNguonNganSach IN(SELECT KTKB.iID_MaNguonNganSach
                                                                        FROM KTKB_ChungTuChiTiet KTKB
                                                                        WHERE KTKB.iTrangThai=1
                                                                          AND KTKB.iLoai=2
                                                                          AND KTKB.rSoTien>0 --OR KTKB.rDTQuyetToan>0
                                                                          AND KTKB.iNamLamViec=@iNamLamViec
                                                                          AND KTKB.iID_MaNamNganSach=@iID_MaNamNganSach
                                                                          {0})",DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iNamNganSach);
            if(!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            dtNS = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtNS;
        }
        /// <summary>
        /// DropDownList danh sách nguồn ngân sách
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaND">Người dùng</param>
        /// <param name="iTrangThai">Trạng thái duyệt</param>
        /// <param name="iNguonNS">Giá trị nguồn ngân sách</param>
        /// <returns></returns>
        public String obj_DSNguonNS(String ParentID, String MaND, String iTrangThai, String iNguonNS)
        {
            String dsNguon = "";
            DataTable dtNguonNS = GetNguonNS(MaND, iTrangThai);
            SelectOptionList slNguon = new SelectOptionList(dtNguonNS, "iID_MaNguonNganSach", "sTen");
            dsNguon = MyHtmlHelper.DropDownList(ParentID, slNguon, iNguonNS, "iNguonNS", "", "class=\"input1_2\" style=\"width:185px; padding:2px;\"");
            return dsNguon;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaND">Người dùng</param>
        /// <param name="iTrangThai">Trạng thái duyệt</param>
        /// <param name="iNguonNS">Giá trị nguồn ngân sách</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_DSNguonNS(String ParentID, String MaND, String iTrangThai, String iNguonNS)
        {
            return Json(obj_DSNguonNS(ParentID, MaND, iTrangThai, iNguonNS), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>        
        /// <param name="iLoaiThang_Quy">0: Chọn tháng | 1: Chọn quý</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNguonNS">Nguồn ngân sách</param>
        /// <param name="iMuc_TieuMuc">In đến mức: Mục | Tiểu mục</param>
        /// <param name="MaND">Năm</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iLoaiThang_Quy, String iThang, String iQuy, String iNguonNS, String iMuc_TieuMuc, String MaND,String iTrangThai)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iLoaiThang_Quy, iThang, iQuy, iNguonNS, iMuc_TieuMuc, MaND,iTrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TheoDoiTamUng.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }      
    }
}