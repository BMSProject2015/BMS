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

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_DanhSachChiTraController : Controller
    {
        // Edit date: 08-07-2012
        // GET: /rptLuong_DanhSachChiTra/
        public string sViewPath = "~/Report_Views/";        
        private const String sFilePath = "";
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DanhSachChiTra.aspx";           
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EDITSUBMIT
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;
            String Thang = Request.Form[ParentID + "_iThangLuong"];
            String Nam = Request.Form[ParentID + "_iNamLuong"];
            String MaDV = Request.Form["iID_MaDonVi"];
            String LoaiBieu = Request.Form[ParentID + "_divReport"];
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);            
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iNamLuong"] = Nam;
            ViewData["iThangLuong"] = Thang;
            ViewData["iMaDV"] = MaDV;
            ViewData["iReport"] = LoaiBieu;            
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DanhSachChiTra.aspx";           
            return View(sViewPath + "ReportView.aspx");            
        }       
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Nam">Năm lương</param>
        /// <param name="Thang">Tháng lương</param>
        /// <param name="TrangThai">Trạng thái duyệt</param>
        /// <param name="MaDV">Mã đơn vị</param>
        /// <returns></returns>       
        public DataTable DanhSachChiTra(String Nam, String Thang, String TrangThai,String MaDV)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String DKDonvi = "AND L.iID_MaDonVi IN (";
            String[] arrMaDV = MaDV.Split(',');
            for (int i = 0; i < arrMaDV.Length; i++)
            {
                DKDonvi += "@iID_MaDonVi" + i;
                if (i < arrMaDV.Length - 1)
                    DKDonvi += " , ";
            }
            DKDonvi += ")";
            String SQL = String.Format(@"SELECT ROW_NUMBER() OVER (PARTITION BY L.iID_MaDonVi ORDER BY L.iID_MaDonVi) AS TT_DV
                                              ,ROW_NUMBER() OVER (ORDER BY L.iID_MaDonVi) AS TT_HT
                                              ,L.iID_MaDonVi
	                                          ,L.sHoDem_CanBo +' '+ L.sTen_CanBo as TenCB
	                                          ,L.sSoTaiKhoan_CanBo
	                                          ,L.sSoSoLuong_CanBo
	                                          ,SUM(L.rLuongCoBan) AS LCB
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                          AND L.iNamBangLuong=@iNamBangLuong
                                          AND L.iThangBangLuong=@iThangBangLuong
                                          {0}
                                          {1}
                                          AND L.sSoTaiKhoan_CanBo<>''
                                        GROUP BY L.iID_MaDonVi
	                                           ,L.sHoDem_CanBo
	                                           ,L.sTen_CanBo
	                                           ,L.sSoTaiKhoan_CanBo
	                                           ,L.sSoSoLuong_CanBo
                                        ORDER BY L.iID_MaDonVi,TT_DV,L.sTen_CanBo", DKDonvi, DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);           
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            for (int i = 0; i < arrMaDV.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrMaDV[i]);
            }
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            }
            DataTable dt = Connection.GetDataTable(cmd);            
            cmd.Dispose();              
            return dt;
        }
        /// <summary>
        /// Lấy tổng lương chi trả theo đơn vị
        /// </summary>
        /// <param name="Nam">Năm lương</param>
        /// <param name="Thang">Tháng lương</param>
        /// <param name="TrangThai">Trạng thái duyệt</param>
        /// <param name="MaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable GetLuong_Donvi(String Nam, String Thang, String TrangThai, String MaDV)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String DKDonvi = "AND L.iID_MaDonVi IN (";
            String[] arrMaDV = MaDV.Split(',');
            for (int i = 0; i < arrMaDV.Length; i++)
            {
                DKDonvi += " @iID_MaDonVi" + i;
                if (i < arrMaDV.Length - 1)
                    DKDonvi += " , ";
            }
            DKDonvi += ")";
            String SQL = String.Format(@"SELECT L.iID_MaDonVi, SUM(L.rLuongCoBan) AS TC
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                        AND L.iNamBangLuong=@iNamBangLuong
                                        AND L.iThangBangLuong=@iThangBangLuong
                                        {0}
                                        {1}
                                        AND L.sSoTaiKhoan_CanBo<>''
                                        GROUP BY L.iID_MaDonVi", DKDonvi, DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            for (int i = 0; i < arrMaDV.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrMaDV[i]);
            }
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="Thang"></param>
        /// <param name="TrangThai"></param>
        /// <returns></returns>
        public DataTable Get_DV(String Nam, String Thang, String TrangThai)
        {
            String DKDuyet = TrangThai.Equals("0") ? "" : "AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            String SQL = String.Format(@"SELECT L.iID_MaDonVi
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                          AND L.iNamBangLuong=@iNamBangLuong
                                          AND L.iThangBangLuong=@iThangBangLuong
                                          {0}
                                          AND L.sSoTaiKhoan_CanBo<>''
                                        GROUP BY L.iID_MaDonVi", DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", Nam);
            cmd.Parameters.AddWithValue("@iThangBangLuong", Thang);
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dtDonvi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonvi;
        }
        /// <summary>
        /// Ghép dữ liệu
        /// </summary>
        /// <param name="dtDonvi_Tong">Dữ liệu đơn vị hoặc tổng lương theo đơn vị</param>
        /// <param name="dtSource">Dữ liệu danh sách cán bộ</param>
        /// <param name="DKJoin">Điều kiện ghép theo đơn vị hoặc theo tổng lương</param>
        /// <returns></returns>
        public DataTable Ghep_Data(DataTable dtDonvi_Tong,DataTable dtSource,String DKJoin)
        {            
            int count = dtSource.Rows.Count;
            switch(DKJoin)
            {
                case "Donvi":
                    for (int i = 0; i < dtDonvi_Tong.Rows.Count; i++)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (Convert.ToString(dtSource.Rows[j]["iID_MaDonVi"]) == Convert.ToString(dtDonvi_Tong.Rows[i]["iID_MaDonVi"]))
                            {
                                DataRow dr = dtSource.NewRow();                                
                                dr["sSoTaiKhoan_CanBo"] = dtDonvi_Tong.Rows[i]["iID_MaDonVi"];
                                dtSource.Rows.InsertAt(dr, j);
                                j++;
                                count++;
                                break;
                            }
                        }
                    }
                    break;
                case "Tong":
                    for (int i = dtDonvi_Tong.Rows.Count - 1; i >= 0; i--)
                    {
                        for (int z = count - 1; z >= 0; z--)
                        {
                            if (Convert.ToString(dtSource.Rows[z]["iID_MaDonVi"]) == Convert.ToString(dtDonvi_Tong.Rows[i]["iID_MaDonVi"]))
                            {
                                DataRow dr1 = dtSource.NewRow();                      
                                dr1["sSoTaiKhoan_CanBo"] = "+";                                
                                dr1["LCB"] = dtDonvi_Tong.Rows[i]["TC"];
                                dtSource.Rows.InsertAt(dr1, z + 1);
                                z++;
                                count++;
                                break;
                            }
                        }
                    }    
                    break;
            }
            return dtSource;
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn đến file mẫu Excel</param>
        /// <param name="Nam">Năm lương</param>
        /// <param name="Thang">Tháng lương</param>
        /// <param name="TrangThai">Trạng thái duyệt</param>
        /// <param name="MaDV">Mã đơn vị</param>
        /// <returns></returns>        
        public ExcelFile CreateReport(String Nam, String Thang, String TrangThai, String MaDV, String LoaiBieu)
        {
            XlsFile Result = new XlsFile(true);
            DataTable data = new DataTable();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_DanhSachChiTra");            
            switch (LoaiBieu)
            {
                case "rTheoDV":                    
                    data = Ghep_Data(Get_DV(Nam, Thang, TrangThai), DanhSachChiTra(Nam, Thang, TrangThai, MaDV), "Donvi");
                    data = Ghep_Data(GetLuong_Donvi(Nam, Thang, TrangThai, MaDV), data, "Tong");
                    data.Columns.Remove("iID_MaDonVi");
                    data.Columns.Remove("TenCB");
                    data.Columns.Remove("sSoSoLuong_CanBo");
                    data.Columns.Remove("TT_HT");
                    Result = TaoTieuDe(Result);
                    FillData(Result, data, "TT_DV", "sSoTaiKhoan_CanBo", "LCB");                    
                    break;
                case "rLietKe":
                    data = Ghep_Data(Get_DV(Nam, Thang, TrangThai), DanhSachChiTra(Nam, Thang, TrangThai, MaDV), "Donvi");                    
                    data.Columns.Remove("iID_MaDonVi");
                    data.Columns.Remove("TenCB");
                    data.Columns.Remove("sSoSoLuong_CanBo");
                    data.Columns.Remove("TT_HT");
                    Result = TaoTieuDe(Result);
                    FillData(Result, data, "TT_DV", "sSoTaiKhoan_CanBo", "LCB");
                    break;
                case "rCoHoTen":
                    data = Ghep_Data(Get_DV(Nam, Thang, TrangThai), DanhSachChiTra(Nam, Thang, TrangThai, MaDV), "Donvi");
                    data = Ghep_Data(GetLuong_Donvi(Nam, Thang, TrangThai, MaDV), data, "Tong");
                    data.Columns.Remove("iID_MaDonVi");
                    data.Columns.Remove("TT_DV");
                    data.Columns.Remove("sSoSoLuong_CanBo");
                    data.Columns.Remove("TT_HT");
                    Result = TaoTieuDe_HoTen(Result);
                    FillData(Result, data, "TenCB", "sSoTaiKhoan_CanBo", "LCB");
                    break;
            }
            data.Dispose();
            tong=TongLuong(DanhSachChiTra(Nam, Thang, TrangThai, MaDV),tong);
            //fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            //fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Thang", Thang);
            fr.SetValue("Nam", Nam);
            int count=DanhSachChiTra(Nam,Thang,TrangThai,MaDV).Rows.Count>0? DanhSachChiTra(Nam,Thang,TrangThai,MaDV).Rows.Count:0;
            fr.SetValue("SoNguoi", count);
            fr.SetValue("SoTien", String.Format("{0:0,0}",tong));
            fr.SetValue("TienRaChu", CommonFunction.TienRaChu(tong));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;            
        }        
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="Nam">Năm lương</param>
        /// <param name="Thang">Tháng lương</param>
        /// <param name="TrangThai">Trạng thái duyệt</param>
        /// <param name="MaDV">Mã đơn vị</param>
        /// <param name="LoaiBieu">Loại biểu báo cáo</param>
        /// <param name="KhoGiay">Khổ giấy A3 hoặc A4</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Nam, String Thang, String TrangThai,String MaDV, String LoaiBieu)
        {
            HamChung.Language();            
            ExcelFile xls = CreateReport( Nam, Thang,TrangThai,MaDV,LoaiBieu);
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
        /// <param name="Nam"></param>
        /// <param name="Thang"></param>
        /// <param name="TrangThai"></param>
        /// <param name="MaDV"></param>
        /// <param name="?"></param>
        /// <returns></returns>

        public clsExcelResult ExportToExcel(String Nam, String Thang, String TrangThai, String MaDV, String LoaiBieu)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();            
            ExcelFile xls = CreateReport(Nam,Thang,TrangThai,MaDV,LoaiBieu);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DanhSach_ChiTra.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
        /// <summary>
        /// Lấy danh sách đơn vị có dữ liệu
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DuyetLuong"></param>
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
        /// Hiển thị danh sách đơn vị lên trình duyệt
        /// </summary>
        /// <param name="NamLuong">Năm</param>
        /// <param name="ThangLuong">Tháng</param>
        /// <param name="DuyetLuong">Trạng thái</param>
        /// <param name="arrDV"></param>
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
        /// Ajax
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DuyetLuong"></param>
        /// <param name="arrDV"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ds_DonVi(String NamLuong, String ThangLuong, String DuyetLuong, String[] arrDV)
        {
            return Json(obj_DSDonVi(NamLuong, ThangLuong, DuyetLuong, arrDV), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Tạo tiêu đề
        /// </summary>
        /// <param name="xls"></param>
        /// <returns></returns>
        public XlsFile TaoTieuDe(XlsFile xls)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;
            xls.OptionsCheckCompatibility = false;

            //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);

            //Named Ranges
            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$4:$4");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 1, 4, 1, 4, FlxConsts.Max_Columns + 1, 32);
            xls.SetNamedRange(Range);


            //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = true;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "&R&\"Times New Roman,Italic\"Trang: &P             ";
            HeadersAndFooters.DefaultFooter = "";
            HeadersAndFooters.FirstHeader = "&R&\"Times New Roman,Italic\"\n\n\n\nTrang: &P             ";
            HeadersAndFooters.FirstFooter = "";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.590551181102362;
            //    PrintMargins.Top = 0.551181102362205;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.393700787401575;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.590551181102362, 0.551181102362205, 0.196850393700787, 0.393700787401575, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.Orientation;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x4D, 0x00, 0x69, 0x00, 0x63, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x73, 0x00, 0x6F, 0x00, 0x66, 0x00, 0x74, 0x00, 0x20, 0x00, 0x58, 0x00, 0x50, 0x00, 0x53, 0x00, 0x20, 0x00, 0x44, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x20, 0x00, 0x57, 0x00, 
            //        0x72, 0x00, 0x69, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x78, 0x03, 0x03, 0xAF, 0x00, 0x00, 0x01, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x03, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x47, 0x49, 0x53, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x44, 0x49, 0x4E, 0x55, 0x22, 0x00, 0x20, 0x01, 0x5C, 0x03, 0x1C, 0x00, 0xCA, 0xD2, 0xF6, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x01, 0x00, 0x00, 0x53, 0x4D, 
            //        0x54, 0x4A, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x10, 0x01, 0x7B, 0x00, 0x30, 0x00, 0x46, 0x00, 0x34, 0x00, 0x31, 0x00, 0x33, 0x00, 0x30, 0x00, 0x44, 0x00, 0x44, 0x00, 0x2D, 0x00, 0x31, 0x00, 0x39, 0x00, 0x43, 0x00, 0x37, 0x00, 0x2D, 0x00, 0x37, 0x00, 0x61, 0x00, 0x62, 0x00, 0x36, 0x00, 0x2D, 0x00, 
            //        0x39, 0x00, 0x39, 0x00, 0x41, 0x00, 0x31, 0x00, 0x2D, 0x00, 0x39, 0x00, 0x38, 0x00, 0x30, 0x00, 0x46, 0x00, 0x30, 0x00, 0x33, 0x00, 0x42, 0x00, 0x32, 0x00, 0x45, 0x00, 0x45, 0x00, 0x34, 0x00, 0x45, 0x00, 0x7D, 0x00, 0x00, 0x00, 0x49, 0x6E, 0x70, 0x75, 0x74, 0x42, 0x69, 0x6E, 0x00, 0x46, 0x4F, 0x52, 
            //        0x4D, 0x53, 0x4F, 0x55, 0x52, 0x43, 0x45, 0x00, 0x52, 0x45, 0x53, 0x44, 0x4C, 0x4C, 0x00, 0x55, 0x6E, 0x69, 0x72, 0x65, 0x73, 0x44, 0x4C, 0x4C, 0x00, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x6C, 0x65, 0x61, 0x76, 0x69, 0x6E, 0x67, 0x00, 0x4F, 0x46, 0x46, 0x00, 0x49, 0x6D, 0x61, 0x67, 0x65, 0x54, 0x79, 0x70, 
            //        0x65, 0x00, 0x4A, 0x50, 0x45, 0x47, 0x4D, 0x65, 0x64, 0x00, 0x4F, 0x72, 0x69, 0x65, 0x6E, 0x74, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x50, 0x4F, 0x52, 0x54, 0x52, 0x41, 0x49, 0x54, 0x00, 0x43, 0x6F, 0x6C, 0x6C, 0x61, 0x74, 0x65, 0x00, 0x4F, 0x46, 0x46, 0x00, 0x52, 0x65, 0x73, 0x6F, 0x6C, 0x75, 0x74, 
            //        0x69, 0x6F, 0x6E, 0x00, 0x4F, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x31, 0x00, 0x50, 0x61, 0x70, 0x65, 0x72, 0x53, 0x69, 0x7A, 0x65, 0x00, 0x4C, 0x45, 0x54, 0x54, 0x45, 0x52, 0x00, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x4D, 0x6F, 0x64, 0x65, 0x00, 0x32, 0x34, 0x62, 0x70, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x56, 0x34, 0x44, 0x4D, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);

            //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 1901);    //(6.68 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 5778);    //(21.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 3840);    //(14.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 1901);    //(6.68 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 5778);    //(21.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 3840);    //(14.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(8));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(8, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(9));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(9, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(10));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(10, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(11));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(11, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(12));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(12, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(13));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(13, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(14));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(14, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(15));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(15, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(16));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(16, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(17));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(17, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(18));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(18, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(19));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(19, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(20));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(20, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(21));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(21, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(22));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(22, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(23));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(23, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(24));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(24, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(25));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(25, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(26));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(26, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(27));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(27, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(28));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(28, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(29));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(29, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(30));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(30, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(31));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(31, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(32));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(32, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(33));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(33, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(34));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(34, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(35));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(35, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(36));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(36, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(37));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(37, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(38));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(38, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(39));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(39, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(40));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(40, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(41));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(41, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(42));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(42, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(43));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(43, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(44));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(44, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(45));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(45, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(46));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(46, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(47));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(47, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(48));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(48, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(49));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(49, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(50));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(50, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(51));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(51, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(52));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(52, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(53));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(53, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(54));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(54, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(55));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(55, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(56));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(56, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(57));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(57, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(58));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(58, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(59));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(59, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(60));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(60, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(61));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(61, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(62));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(62, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(63));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(63, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(64));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(64, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(65));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(65, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(66));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(66, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(67));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(67, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(68));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(68, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(69));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(69, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(70));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(70, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(71));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(71, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(72));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(72, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(73));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(73, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(74));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(74, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(75));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(75, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(76));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(76, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(77));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(77, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(78));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(78, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(79));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(79, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(80));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(80, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(81));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(81, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(82));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(82, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(83));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(83, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(84));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(84, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(85));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(85, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(86));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(86, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(87));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(87, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(88));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(88, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(89));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(89, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(90));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(90, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(91));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(91, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(92));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(92, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(93));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(93, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(94));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(94, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(95));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(95, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(96));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(96, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(97));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(97, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(98));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(98, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(99));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(99, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(100));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(100, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(101));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(101, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(102));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(102, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(103));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(103, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(104));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(104, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(105));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(105, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(106));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(106, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(107));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(107, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(108));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(108, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(109));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(109, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(110));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(110, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(111));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(111, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(112));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(112, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(113));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(113, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(114));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(114, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(115));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(115, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(116));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(116, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(117));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(117, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(118));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(118, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(119));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(119, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(120));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(120, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(121));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(121, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(122));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(122, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(123));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(123, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(124));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(124, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(125));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(125, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(126));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(126, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(127));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(127, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(128));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(128, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(129));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(129, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(130));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(130, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(131));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(131, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(132));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(132, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(133));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(133, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(134));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(134, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(135));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(135, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(136));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(136, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(137));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(137, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(138));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(138, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(139));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(139, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(140));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(140, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(141));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(141, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(142));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(142, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(143));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(143, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(144));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(144, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(145));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(145, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(146));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(146, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(147));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(147, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(148));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(148, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(149));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(149, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(150));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(150, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(151));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(151, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(152));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(152, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(153));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(153, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(154));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(154, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(155));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(155, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(156));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(156, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(157));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(157, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(158));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(158, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(159));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(159, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(160));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(160, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(161));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(161, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(162));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(162, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(163));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(163, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(164));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(164, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(165));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(165, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(166));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(166, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(167));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(167, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(168));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(168, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(169));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(169, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(170));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(170, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(171));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(171, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(172));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(172, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(173));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(173, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(174));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(174, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(175));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(175, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(176));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(176, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(177));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(177, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(178));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(178, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(179));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(179, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(180));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(180, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(181));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(181, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(182));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(182, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(183));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(183, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(184));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(184, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(185));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(185, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(186));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(186, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(187));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(187, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(188));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(188, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(189));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(189, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(190));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(190, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(191));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(191, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(192));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(192, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(193));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(193, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(194));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(194, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(195));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(195, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(196));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(196, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(197));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(197, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(198));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(198, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(199));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(199, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(200));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(200, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(201));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(201, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(202));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(202, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(203));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(203, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(204));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(204, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(205));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(205, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(206));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(206, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(207));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(207, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(208));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(208, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(209));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(209, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(210));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(210, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(211));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(211, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(212));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(212, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(213));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(213, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(214));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(214, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(215));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(215, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(216));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(216, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(217));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(217, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(218));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(218, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(219));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(219, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(220));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(220, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(221));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(221, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(222));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(222, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(223));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(223, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(224));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(224, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(225));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(225, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(226));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(226, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(227));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(227, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(228));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(228, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(229));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(229, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(230));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(230, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(231));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(231, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(232));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(232, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(233));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(233, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(234));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(234, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(235));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(235, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(236));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(236, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(237));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(237, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(238));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(238, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(239));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(239, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(240));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(240, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(241));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(241, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(242));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(242, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(243));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(243, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(244));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(244, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(245));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(245, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(246));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(246, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(247));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(247, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(248));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(248, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(249));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(249, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(250));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(250, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(251));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(251, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(252));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(252, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(253));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(253, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(254));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(254, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(255));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(255, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(256));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(256, xls.AddFormat(ColFmt));
            xls.DefaultRowHeight = 300;

            xls.SetRowHeight(1, 330);    //16.50 * 20
            xls.SetRowHeight(2, 315);    //15.75 * 20
            xls.SetRowHeight(4, 390);    //19.50 * 20

            //Merged Cells
            xls.MergeCells(3, 3, 3, 5);
            xls.MergeCells(1, 1, 1, 6);
            xls.MergeCells(2, 1, 2, 6);
            xls.MergeCells(2, 3, 2, 5);

            //Set the cell values
            TFlxFormat fmt;
            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 280;
            fmt.Font.Family = 1;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "DANH SÁCH CHI TRẢ CÁ NHÂN");

            fmt = xls.GetCellVisibleFormatDef(1, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 8, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 9, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 1, xls.AddFormat(fmt));
            xls.SetCellValue(2, 1, "Tháng <#Thang> / <#Nam>");

            fmt = xls.GetCellVisibleFormatDef(2, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 3, xls.AddFormat(fmt));
            xls.MergeCells(2, 1, 2, 6);
            xls.SetCellValue(2, 3, "DANH SÁCH CHI TRẢ CÁ NHÂN");

            fmt = xls.GetCellVisibleFormatDef(2, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 8, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 9, xls.AddFormat(fmt));
            fmt = xls.GetCellVisibleFormatDef(3, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(3, 1, xls.AddFormat(fmt));
            xls.SetCellValue(3, 1, "<#Auto page breaks>");
            fmt = xls.GetCellVisibleFormatDef(3, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            xls.SetCellFormat(3, 3, xls.AddFormat(fmt));
            xls.SetCellValue(3, 3, "");

            fmt = xls.GetCellVisibleFormatDef(3, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            xls.SetCellFormat(3, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(3, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            xls.SetCellFormat(3, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "TT");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));
            xls.SetCellValue(4, 2, "SỐ TÀI KHOẢN");

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));
            xls.SetCellValue(4, 3, "SỐ TIỀN");

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));
            xls.SetCellValue(4, 4, "TT");

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));
            xls.SetCellValue(4, 5, "SỐ TÀI KHOẢN");

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));
            xls.SetCellValue(4, 6, "SỐ TIỀN");

            //Cell selection and scroll position.
            xls.SelectCell(11, 2, false);

            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="dt"></param>
        /// <param name="DK1"></param>
        /// <param name="DK2"></param>
        /// <param name="DK3"></param>
        public void FillData(XlsFile xls, DataTable dt, String DK1, String DK2, String DK3)
        {
            TFlxFormat fmt, fmt1,fmt2,fmt_TL,fmt_CD,fmt_Ten;
            Object GiaTriO;
            int sohang = 45;
            int sotrang = 1;
            ///Fill nửa bên trái            
            for (int i = 0; i < dt.Rows.Count; i = i + sohang)
            {
                i = i + sohang;
                #region "Fill nửa bên trái"
                for (int j = i - sohang; j < i; j++)
                {
                    if ((j + i - sohang) < dt.Rows.Count)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            fmt = xls.GetCellVisibleFormatDef(5 + j + i - sohang * sotrang, c + 1);
                            fmt.Font.Name = "Times New Roman";
                            fmt.Font.Size20 = 200;
                            fmt.Font.Family = 1;
                            fmt.VAlignment = TVFlxAlignment.center;                            
                            fmt.WrapText = true;
                            xls.DefaultRowHeight = 300;
                            xls.AutofitRow(5 + j + i - sohang * sotrang, true, 1);
                            GiaTriO = null;                            
                            if (Convert.ToString(dt.Rows[j + i - sohang][DK1].ToString()) == "" 
                                && Convert.ToString(dt.Rows[j + i - sohang][DK2].ToString()) != ""
                                && Convert.ToString(dt.Rows[j + i - sohang][DK3].ToString())=="")
                            {
                                fmt.Font.Style = TFlxFontStyles.Bold;
                                fmt.Font.Family = 1;
                                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;                    
                                fmt.Borders.Top.Color = TExcelColor.Automatic;
                                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                                if (c == 0)
                                {
                                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                    fmt.Borders.Right.Style = TFlxBorderStyle.None;
                                    GiaTriO = dt.Rows[j + i - sohang][c+1];
                                }
                                else
                                {
                                    fmt.Borders.Left.Style = TFlxBorderStyle.None;
                                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                    GiaTriO = dt.Rows[j + i - sohang][c];
                                }
                                xls.MergeCells(5 + j + i - sohang * sotrang, 1, 5 + j + i - sohang * sotrang, 3);                                
                                
                                xls.SetCellFormat(5 + j + i - sohang * sotrang, c +1, xls.AddFormat(fmt));
                                xls.SetCellValue(5 + j + i - sohang * sotrang, c +1, GiaTriO);
                            }
                            else if (Convert.ToString(dt.Rows[j + i - sohang][DK1].ToString()) == ""
                                && Convert.ToString(dt.Rows[j + i - sohang][DK2].ToString()) == "+"
                                && Convert.ToString(dt.Rows[j + i - sohang][DK3].ToString()) != "")
                            {
                                fmt.Font.Style = TFlxFontStyles.Bold;                                
                                fmt.HAlignment = THFlxAlignment.center;                                
                                fmt.Borders.Top.Color = TExcelColor.Automatic;
                                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Bottom.Color = TExcelColor.Automatic;                                
                                fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
                                xls.MergeCells(5 + j + i - sohang * sotrang, 1, 5 + j + i - sohang * sotrang, 2);
                                if(c==0)
                                    GiaTriO = dt.Rows[j + i - sohang][c+1];
                                else
                                    GiaTriO = dt.Rows[j + i - sohang][c];
                                xls.SetCellFormat(5 + j + i - sohang * sotrang, c + 1, xls.AddFormat(fmt));
                                xls.SetCellValue(5 + j + i - sohang * sotrang, c + 1, GiaTriO);
                            }
                            else if (Convert.ToString(dt.Rows[j + i - sohang][DK1].ToString()) != ""
                                && Convert.ToString(dt.Rows[j + i - sohang][DK2].ToString()) != "+"
                                && Convert.ToString(dt.Rows[j + i - sohang][DK3].ToString()) != "")
                            {
                                fmt.Font.Style = TFlxFontStyles.None;
                                fmt.Font.Family = 1;
                                fmt.Borders.Top.Color = TExcelColor.Automatic;
                                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                                fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;  
                                switch(c)
                                {
                                    case 0:
                                        if (DK1 == "TT_DV")
                                        {
                                            fmt.HAlignment = THFlxAlignment.center;
                                        }
                                        else if (DK1 == "TenCB")
                                        {
                                            fmt.HAlignment = THFlxAlignment.left;
                                        }
                                        GiaTriO = dt.Rows[j + i - sohang][c];
                                        xls.SetCellFormat(5 + j + i - sohang * sotrang, c + 1, xls.AddFormat(fmt));
                                        xls.SetCellValue(5 + j + i - sohang * sotrang, c + 1, GiaTriO);
                                        break;
                                    case 1:                                        
                                        fmt.HAlignment = THFlxAlignment.left;
                                        fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
                                        GiaTriO = dt.Rows[j + i - sohang][c];
                                        xls.SetCellFormat(5 + j + i - sohang * sotrang, c + 1, xls.AddFormat(fmt));
                                        xls.SetCellValue(5 + j + i - sohang * sotrang, c + 1, GiaTriO);
                                        break;
                                    case 2:
                                        fmt.HAlignment = THFlxAlignment.right;
                                        fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
                                        GiaTriO = dt.Rows[j + i - sohang][c];
                                        xls.SetCellFormat(5 + j + i - sohang * sotrang, c + 1, xls.AddFormat(fmt));
                                        xls.SetCellValue(5 + j + i - sohang * sotrang, c + 1, GiaTriO);
                                        break;
                                }
                            }                            
                        }
                    }
                }
                #endregion
                ///Fill nửa bên phải
                #region "Fill nửa bên phải"
                for (int z = i; z < i + sohang; z++)
                {
                    if ((z + i - sohang) < dt.Rows.Count)
                    {
                        for (int h = 3; h < 6; h++)
                        {
                            fmt = xls.GetCellVisibleFormatDef(5 + z + i - sohang - sohang * sotrang, h + 1);
                            fmt.Font.Name = "Times New Roman";
                            fmt.Font.Size20 = 200;
                            fmt.Font.Family = 1;
                            fmt.VAlignment = TVFlxAlignment.center;
                            fmt.WrapText = true;
                            xls.DefaultRowHeight = 300;
                            xls.AutofitRow(5 + z + i - sohang - sohang * sotrang, true, 1);
                            GiaTriO = null;
                            GiaTriO = dt.Rows[z + i - sohang][h - 3];
                            xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, h + 1, xls.AddFormat(fmt));
                            xls.SetCellValue(5 + z + i - sohang - sohang * sotrang, h + 1, GiaTriO);
                            if (Convert.ToString(dt.Rows[z + i - sohang][DK1].ToString()) == ""
                                && Convert.ToString(dt.Rows[z + i - sohang][DK2].ToString()) != ""
                                && Convert.ToString(dt.Rows[z + i - sohang][DK3].ToString()) == "")
                            {
                                fmt.Font.Style = TFlxFontStyles.Bold;
                                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Top.Color = TExcelColor.Automatic;
                                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                                //fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                //fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                if (h == 3)
                                {
                                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                    fmt.Borders.Right.Style = TFlxBorderStyle.None;
                                    GiaTriO = dt.Rows[z + i - sohang][h - 2];
                                }
                                else
                                {
                                    fmt.Borders.Left.Style = TFlxBorderStyle.None;
                                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                    GiaTriO = dt.Rows[z + i - sohang][h - 3];
                                }
                                xls.MergeCells(5 + z + i - sohang - sohang * sotrang, 4, 5 + z + i - sohang - sohang * sotrang, 6);
                                //GiaTriO = dt.Rows[z + i - sohang][h - 3];
                                xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, h + 1, xls.AddFormat(fmt));
                                xls.SetCellValue(5 + z + i - sohang - sohang * sotrang, h + 1, GiaTriO);
                            }
                            else if (Convert.ToString(dt.Rows[z + i - sohang][DK1].ToString()) == ""
                                && Convert.ToString(dt.Rows[z + i - sohang][DK2].ToString()) == "+"
                                && Convert.ToString(dt.Rows[z + i - sohang][DK3].ToString()) != "")
                            {
                                fmt.Font.Style = TFlxFontStyles.Bold;
                                fmt.HAlignment = THFlxAlignment.center;
                                fmt.Borders.Top.Color = TExcelColor.Automatic;
                                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                                fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
                                xls.MergeCells(5 + z + i - sohang - sohang * sotrang, 4, 5 + z + i - sohang - sohang * sotrang, 5);
                                //xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, 4, xls.AddFormat(fmt));                                
                                if (h == 3)
                                    GiaTriO = dt.Rows[z + i - sohang][h - 3+1];
                                else
                                    GiaTriO = dt.Rows[z + i - sohang][h - 3];
                                xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, h + 1, xls.AddFormat(fmt));
                                xls.SetCellValue(5 + z + i - sohang - sohang * sotrang, h + 1, GiaTriO);
                            }
                            else if (Convert.ToString(dt.Rows[z + i - sohang][DK1].ToString()) != ""
                                && Convert.ToString(dt.Rows[z + i - sohang][DK2].ToString()) != "+"
                                && Convert.ToString(dt.Rows[z + i - sohang][DK3].ToString()) != "")
                            {
                                fmt.Font.Style = TFlxFontStyles.None;
                                fmt.Font.Family = 1;
                                fmt.Borders.Top.Color = TExcelColor.Automatic;
                                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                                fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                                switch (h)
                                {
                                    case 3:
                                        if (DK1 == "TT_DV")
                                        {
                                            fmt.HAlignment = THFlxAlignment.center;
                                        }
                                        else if (DK1 == "TenCB")
                                        {
                                            fmt.HAlignment = THFlxAlignment.left;
                                        }                                        
                                        GiaTriO = dt.Rows[z + i - sohang][h - 3];
                                        xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, h + 1, xls.AddFormat(fmt));
                                        xls.SetCellValue(5 + z + i - sohang - sohang * sotrang, h + 1, GiaTriO);
                                        break;
                                    case 4:
                                        fmt.HAlignment = THFlxAlignment.left;
                                        fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
                                        GiaTriO = dt.Rows[z + i - sohang][h - 3];
                                        xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, h + 1, xls.AddFormat(fmt));
                                        xls.SetCellValue(5 + z + i - sohang - sohang * sotrang, h + 1, GiaTriO);
                                        break;
                                    case 5:
                                        fmt.HAlignment = THFlxAlignment.right;
                                        fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
                                        GiaTriO = dt.Rows[z + i - sohang][h - 3];
                                        xls.SetCellFormat(5 + z + i - sohang - sohang * sotrang, h + 1, xls.AddFormat(fmt));
                                        xls.SetCellValue(5 + z + i - sohang - sohang * sotrang, h + 1, GiaTriO);
                                        break;
                                }
                            }          
                        }
                    }
                }
                #endregion
                i = i - sohang;
                sotrang++;
            }            
            int hangs = sohang > dt.Rows.Count ? dt.Rows.Count + 1 + 5 : sohang + 1 + 5;
            fmt1 = xls.GetCellVisibleFormatDef(hangs, 2);
            fmt1.Font.Style = TFlxFontStyles.Bold | TFlxFontStyles.Italic;
            fmt1.Font.Name = "Times New Roman";
            fmt1.Format = "";
            fmt1.Font.Size20 = 220;
            fmt1.Font.Family = 1;
            fmt1.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
            if (DK1 == "TT_DV")
            {
                xls.SetCellFormat(hangs + 1, 2, xls.AddFormat(fmt1));
                xls.SetCellValue(hangs - 1, 2, "Tổng số người: ");
                xls.SetCellValue(hangs - 1, 3, "<#SoNguoi>");
                xls.SetCellValue(hangs, 2, "Tổng số tiền là: ");
                xls.SetCellValue(hangs, 3, "<#SoTien>");
                xls.SetCellValue(hangs + 1, 2, "Bằng chữ:  " + "<#TienRaChu>");
            }
            else if (DK1 == "TenCB")
            {
                xls.SetCellFormat(hangs + 1, 1, xls.AddFormat(fmt1));
                xls.SetCellValue(hangs - 1, 1, "     Tổng số người: ");
                xls.SetCellValue(hangs - 1, 3, "<#SoNguoi>");
                xls.SetCellValue(hangs, 1, "     Tổng số tiền là: ");
                xls.SetCellValue(hangs, 3, "<#SoTien>");
                xls.SetCellValue(hangs + 1, 1, "     Bằng chữ:  " + "<#TienRaChu>");
            }

            fmt2 = xls.GetCellVisibleFormatDef(hangs, 6);
            fmt2.Font.Style = TFlxFontStyles.Italic;
            fmt2.Font.Name = "Times New Roman";
            fmt2.Format = "";
            fmt2.Font.Size20 = 220;
            fmt2.Font.Family = 1;
            fmt2.HAlignment = THFlxAlignment.right;
            xls.MergeCells(hangs+2,1,hangs+2, 6);
            xls.SetCellFormat(hangs + 2, 1, xls.AddFormat(fmt2));
            xls.SetCellValue(hangs + 2, 1, "<#NgayThang>");
            //Thừa lệnh 1
            fmt_TL = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_TL.Font.Style = TFlxFontStyles.None;
            fmt_TL.Font.Name = "Times New Roman";
            fmt_TL.Format = "";
            fmt_TL.Font.Size20 = 220;
            fmt_TL.Font.Family = 1;
            fmt_TL.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 4, 1, hangs + 4, 2);
            xls.SetCellFormat(hangs + 4, 1, xls.AddFormat(fmt_TL));
            xls.SetCellValue(hangs + 4, 1, "<#ThuaLenh1>");
            //Thừa lệnh 2
            fmt_TL = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_TL.Font.Style = TFlxFontStyles.None;
            fmt_TL.Font.Name = "Times New Roman";
            fmt_TL.Format = "";
            fmt_TL.Font.Size20 = 220;
            fmt_TL.Font.Family = 1;
            fmt_TL.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 4, 3, hangs + 4, 3);
            xls.SetCellFormat(hangs + 4, 3, xls.AddFormat(fmt_TL));
            xls.SetCellValue(hangs + 4, 1, "<#ThuaLenh2>");
            //Thừa lệnh 3
            fmt_TL = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_TL.Font.Style = TFlxFontStyles.None;
            fmt_TL.Font.Name = "Times New Roman";
            fmt_TL.Format = "";
            fmt_TL.Font.Size20 = 220;
            fmt_TL.Font.Family = 1;
            fmt_TL.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 4, 5, hangs + 4, 6);
            xls.SetCellFormat(hangs + 4, 5, xls.AddFormat(fmt_TL));
            xls.SetCellValue(hangs + 4, 5, "<#ThuaLenh3>");

            //Chức danh 1
            fmt_CD = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_CD.Font.Style = TFlxFontStyles.Bold;
            fmt_CD.Font.Name = "Times New Roman";
            fmt_CD.Format = "";
            fmt_CD.Font.Size20 = 220;
            fmt_CD.Font.Family = 1;
            fmt_CD.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 5, 1, hangs + 5, 2);
            xls.SetCellFormat(hangs + 5, 1, xls.AddFormat(fmt_CD));
            xls.SetCellValue(hangs + 5, 1, "<#ChucDanh1>");
            //Chức danh 2
            fmt_CD = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_CD.Font.Style = TFlxFontStyles.Bold;
            fmt_CD.Font.Name = "Times New Roman";
            fmt_CD.Format = "";
            fmt_CD.Font.Size20 = 220;
            fmt_CD.Font.Family = 1;
            fmt_CD.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 5, 3, hangs + 5, 4);
            xls.SetCellFormat(hangs + 5, 3, xls.AddFormat(fmt_CD));
            xls.SetCellValue(hangs + 5, 3, "<#ChucDanh2>");
            //Chức danh 3
            fmt_CD = xls.GetCellVisibleFormatDef(hangs, 4);
            fmt_CD.Font.Style = TFlxFontStyles.Bold;
            fmt_CD.Font.Name = "Times New Roman";
            fmt_CD.Format = "";
            fmt_CD.Font.Size20 = 220;
            fmt_CD.Font.Family = 1;
            fmt_CD.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 5, 5, hangs + 5, 6);
            xls.SetCellFormat(hangs + 5, 5, xls.AddFormat(fmt_CD));
            xls.SetCellValue(hangs + 5, 5, "<#ChucDanh3>");
            //Tên 1
            fmt_Ten = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_Ten.Font.Style = TFlxFontStyles.None;
            fmt_Ten.Font.Name = "Times New Roman";
            fmt_Ten.Format = "";
            fmt_Ten.Font.Size20 = 220;
            fmt_Ten.Font.Family = 1;
            fmt_Ten.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 9, 1, hangs + 9, 2);
            xls.SetCellFormat(hangs + 9, 1, xls.AddFormat(fmt_Ten));
            xls.SetCellValue(hangs + 9, 1, "<#Ten1>");
            //Tên 2
            fmt_Ten = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_Ten.Font.Style = TFlxFontStyles.None;
            fmt_Ten.Font.Name = "Times New Roman";
            fmt_Ten.Format = "";
            fmt_Ten.Font.Size20 = 220;
            fmt_Ten.Font.Family = 1;
            fmt_Ten.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 9, 3, hangs + 9, 4);
            xls.SetCellFormat(hangs + 9, 3, xls.AddFormat(fmt_Ten));
            xls.SetCellValue(hangs + 9, 3, "<#Ten2>");
            //Tên 3
            fmt_Ten = xls.GetCellVisibleFormatDef(hangs, 1);
            fmt_Ten.Font.Style = TFlxFontStyles.None;
            fmt_Ten.Font.Name = "Times New Roman";
            fmt_Ten.Format = "";
            fmt_Ten.Font.Size20 = 220;
            fmt_Ten.Font.Family = 1;
            fmt_Ten.HAlignment = THFlxAlignment.center;
            xls.MergeCells(hangs + 9, 5, hangs + 9, 6);
            xls.SetCellFormat(hangs + 9, 5, xls.AddFormat(fmt_Ten));
            xls.SetCellValue(hangs + 9, 5, "<#Ten3>");          
            //
            TXlsNamedRange Range;
            Range = new TXlsNamedRange("KeepRows_1_", 0, 1, 4, 1, hangs + 10, 6, 0);
            xls.SetNamedRange(Range);
            Range = new TXlsNamedRange("KeepRows_2_", 0, 1, hangs, 1, hangs + 10, 6, 0);
            xls.SetNamedRange(Range);
        }
        long tong = 0;
        private static long TongLuong(DataTable dt, long tong)
        {
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                string temp = string.IsNullOrEmpty(dt.Rows[t]["LCB"].ToString()) ? "0" : dt.Rows[t]["LCB"].ToString();
                tong += long.Parse(temp);
            }
            return tong;
        }
        /// <summary>
        /// Tạo tiêu đề có họ tên
        /// </summary>
        /// <param name="xls">Tên file Excel</param>
        /// <returns></returns>
        public XlsFile TaoTieuDe_HoTen(XlsFile xls)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;
            xls.OptionsCheckCompatibility = false;

            //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);

            //Named Ranges
            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$4:$4");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 1, 4, 1, 4, FlxConsts.Max_Columns + 1, 32);
            xls.SetNamedRange(Range);


            //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = true;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "&R&\"Times New Roman,Italic\"Trang: &P             ";
            HeadersAndFooters.DefaultFooter = "";
            HeadersAndFooters.FirstHeader = "&R&\"Times New Roman,Italic\"\n\n\n\nTrang: &P         ";
            HeadersAndFooters.FirstFooter = "";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.393700787401575;
            //    PrintMargins.Top = 0.551181102362205;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.393700787401575;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.393700787401575, 0.551181102362205, 0.196850393700787, 0.393700787401575, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.Orientation;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x4D, 0x00, 0x69, 0x00, 0x63, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x73, 0x00, 0x6F, 0x00, 0x66, 0x00, 0x74, 0x00, 0x20, 0x00, 0x58, 0x00, 0x50, 0x00, 0x53, 0x00, 0x20, 0x00, 0x44, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x20, 0x00, 0x57, 0x00, 
            //        0x72, 0x00, 0x69, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x78, 0x03, 0x03, 0xAF, 0x00, 0x00, 0x01, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x03, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x47, 0x49, 0x53, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x44, 0x49, 0x4E, 0x55, 0x22, 0x00, 0x20, 0x01, 0x5C, 0x03, 0x1C, 0x00, 0xCA, 0xD2, 0xF6, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x01, 0x00, 0x00, 0x53, 0x4D, 
            //        0x54, 0x4A, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x10, 0x01, 0x7B, 0x00, 0x30, 0x00, 0x46, 0x00, 0x34, 0x00, 0x31, 0x00, 0x33, 0x00, 0x30, 0x00, 0x44, 0x00, 0x44, 0x00, 0x2D, 0x00, 0x31, 0x00, 0x39, 0x00, 0x43, 0x00, 0x37, 0x00, 0x2D, 0x00, 0x37, 0x00, 0x61, 0x00, 0x62, 0x00, 0x36, 0x00, 0x2D, 0x00, 
            //        0x39, 0x00, 0x39, 0x00, 0x41, 0x00, 0x31, 0x00, 0x2D, 0x00, 0x39, 0x00, 0x38, 0x00, 0x30, 0x00, 0x46, 0x00, 0x30, 0x00, 0x33, 0x00, 0x42, 0x00, 0x32, 0x00, 0x45, 0x00, 0x45, 0x00, 0x34, 0x00, 0x45, 0x00, 0x7D, 0x00, 0x00, 0x00, 0x49, 0x6E, 0x70, 0x75, 0x74, 0x42, 0x69, 0x6E, 0x00, 0x46, 0x4F, 0x52, 
            //        0x4D, 0x53, 0x4F, 0x55, 0x52, 0x43, 0x45, 0x00, 0x52, 0x45, 0x53, 0x44, 0x4C, 0x4C, 0x00, 0x55, 0x6E, 0x69, 0x72, 0x65, 0x73, 0x44, 0x4C, 0x4C, 0x00, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x6C, 0x65, 0x61, 0x76, 0x69, 0x6E, 0x67, 0x00, 0x4F, 0x46, 0x46, 0x00, 0x49, 0x6D, 0x61, 0x67, 0x65, 0x54, 0x79, 0x70, 
            //        0x65, 0x00, 0x4A, 0x50, 0x45, 0x47, 0x4D, 0x65, 0x64, 0x00, 0x4F, 0x72, 0x69, 0x65, 0x6E, 0x74, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x50, 0x4F, 0x52, 0x54, 0x52, 0x41, 0x49, 0x54, 0x00, 0x43, 0x6F, 0x6C, 0x6C, 0x61, 0x74, 0x65, 0x00, 0x4F, 0x46, 0x46, 0x00, 0x52, 0x65, 0x73, 0x6F, 0x6C, 0x75, 0x74, 
            //        0x69, 0x6F, 0x6E, 0x00, 0x4F, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x31, 0x00, 0x50, 0x61, 0x70, 0x65, 0x72, 0x53, 0x69, 0x7A, 0x65, 0x00, 0x4C, 0x45, 0x54, 0x54, 0x45, 0x52, 0x00, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x4D, 0x6F, 0x64, 0x65, 0x00, 0x32, 0x34, 0x62, 0x70, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x56, 0x34, 0x44, 0x4D, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);

            //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 5156);    //(19.39 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 3840);    //(14.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 3218);    //(11.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 5156);    //(19.39 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 3840);    //(14.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 3218);    //(11.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(8));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(8, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(9));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(9, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(10));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(10, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(11));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(11, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(12));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(12, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(13));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(13, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(14));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(14, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(15));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(15, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(16));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(16, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(17));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(17, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(18));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(18, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(19));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(19, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(20));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(20, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(21));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(21, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(22));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(22, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(23));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(23, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(24));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(24, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(25));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(25, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(26));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(26, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(27));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(27, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(28));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(28, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(29));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(29, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(30));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(30, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(31));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(31, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(32));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(32, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(33));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(33, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(34));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(34, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(35));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(35, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(36));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(36, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(37));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(37, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(38));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(38, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(39));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(39, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(40));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(40, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(41));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(41, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(42));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(42, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(43));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(43, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(44));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(44, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(45));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(45, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(46));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(46, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(47));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(47, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(48));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(48, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(49));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(49, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(50));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(50, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(51));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(51, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(52));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(52, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(53));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(53, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(54));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(54, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(55));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(55, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(56));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(56, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(57));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(57, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(58));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(58, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(59));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(59, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(60));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(60, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(61));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(61, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(62));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(62, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(63));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(63, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(64));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(64, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(65));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(65, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(66));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(66, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(67));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(67, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(68));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(68, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(69));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(69, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(70));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(70, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(71));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(71, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(72));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(72, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(73));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(73, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(74));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(74, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(75));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(75, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(76));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(76, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(77));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(77, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(78));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(78, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(79));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(79, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(80));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(80, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(81));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(81, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(82));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(82, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(83));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(83, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(84));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(84, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(85));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(85, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(86));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(86, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(87));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(87, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(88));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(88, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(89));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(89, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(90));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(90, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(91));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(91, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(92));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(92, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(93));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(93, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(94));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(94, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(95));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(95, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(96));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(96, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(97));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(97, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(98));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(98, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(99));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(99, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(100));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(100, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(101));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(101, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(102));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(102, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(103));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(103, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(104));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(104, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(105));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(105, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(106));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(106, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(107));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(107, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(108));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(108, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(109));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(109, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(110));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(110, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(111));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(111, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(112));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(112, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(113));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(113, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(114));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(114, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(115));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(115, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(116));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(116, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(117));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(117, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(118));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(118, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(119));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(119, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(120));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(120, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(121));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(121, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(122));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(122, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(123));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(123, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(124));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(124, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(125));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(125, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(126));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(126, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(127));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(127, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(128));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(128, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(129));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(129, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(130));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(130, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(131));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(131, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(132));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(132, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(133));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(133, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(134));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(134, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(135));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(135, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(136));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(136, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(137));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(137, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(138));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(138, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(139));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(139, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(140));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(140, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(141));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(141, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(142));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(142, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(143));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(143, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(144));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(144, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(145));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(145, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(146));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(146, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(147));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(147, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(148));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(148, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(149));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(149, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(150));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(150, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(151));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(151, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(152));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(152, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(153));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(153, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(154));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(154, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(155));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(155, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(156));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(156, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(157));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(157, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(158));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(158, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(159));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(159, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(160));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(160, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(161));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(161, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(162));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(162, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(163));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(163, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(164));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(164, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(165));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(165, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(166));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(166, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(167));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(167, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(168));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(168, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(169));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(169, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(170));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(170, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(171));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(171, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(172));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(172, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(173));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(173, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(174));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(174, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(175));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(175, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(176));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(176, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(177));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(177, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(178));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(178, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(179));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(179, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(180));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(180, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(181));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(181, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(182));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(182, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(183));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(183, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(184));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(184, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(185));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(185, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(186));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(186, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(187));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(187, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(188));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(188, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(189));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(189, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(190));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(190, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(191));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(191, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(192));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(192, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(193));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(193, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(194));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(194, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(195));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(195, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(196));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(196, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(197));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(197, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(198));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(198, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(199));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(199, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(200));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(200, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(201));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(201, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(202));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(202, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(203));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(203, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(204));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(204, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(205));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(205, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(206));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(206, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(207));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(207, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(208));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(208, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(209));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(209, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(210));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(210, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(211));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(211, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(212));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(212, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(213));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(213, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(214));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(214, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(215));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(215, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(216));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(216, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(217));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(217, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(218));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(218, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(219));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(219, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(220));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(220, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(221));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(221, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(222));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(222, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(223));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(223, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(224));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(224, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(225));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(225, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(226));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(226, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(227));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(227, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(228));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(228, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(229));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(229, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(230));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(230, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(231));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(231, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(232));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(232, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(233));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(233, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(234));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(234, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(235));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(235, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(236));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(236, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(237));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(237, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(238));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(238, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(239));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(239, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(240));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(240, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(241));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(241, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(242));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(242, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(243));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(243, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(244));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(244, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(245));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(245, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(246));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(246, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(247));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(247, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(248));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(248, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(249));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(249, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(250));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(250, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(251));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(251, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(252));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(252, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(253));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(253, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(254));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(254, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(255));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(255, xls.AddFormat(ColFmt));

            ColFmt = xls.GetFormat(xls.GetColFormat(256));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(256, xls.AddFormat(ColFmt));
            xls.DefaultRowHeight = 300;

            xls.SetRowHeight(1, 330);    //16.50 * 20
            xls.SetRowHeight(2, 315);    //15.75 * 20
            xls.SetRowHeight(4, 390);    //19.50 * 20

            //Merged Cells
            xls.MergeCells(3, 3, 3, 5);
            xls.MergeCells(1, 1, 1, 6);
            xls.MergeCells(2, 1, 2, 6);
            //xls.MergeCells(2, 3, 2, 5);

            //Set the cell values
            TFlxFormat fmt;
            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Family = 1;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "DANH SÁCH CHI TRẢ CÁ NHÂN");

            fmt = xls.GetCellVisibleFormatDef(1, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 8, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(1, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 260;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 9, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 1, xls.AddFormat(fmt));
            xls.SetCellValue(2, 1, "Tháng <#Thang> / <#Nam>");

            fmt = xls.GetCellVisibleFormatDef(2, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;            
            xls.SetCellFormat(2, 3, xls.AddFormat(fmt));            
            xls.SetCellValue(2, 1, "");

            fmt = xls.GetCellVisibleFormatDef(2, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 8, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(2, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(2, 9, xls.AddFormat(fmt));
            fmt = xls.GetCellVisibleFormatDef(3, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 240;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(3, 1, xls.AddFormat(fmt));
            xls.SetCellValue(3, 1, "<#Auto page breaks>");

            fmt = xls.GetCellVisibleFormatDef(3, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(3, 3, xls.AddFormat(fmt));            
            xls.SetCellValue(3, 3, "");

            fmt = xls.GetCellVisibleFormatDef(3, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(3, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(3, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(3, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 210;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "HỌ TÊN");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 210;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));
            xls.SetCellValue(4, 2, "SỐ TÀI KHOẢN");

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 210;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));
            xls.SetCellValue(4, 3, "SỐ TIỀN");

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 210;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));
            xls.SetCellValue(4, 4, "HỌ TÊN");

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 210;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));
            xls.SetCellValue(4, 5, "SỐ TÀI KHOẢN");

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 210;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));
            xls.SetCellValue(4, 6, "SỐ TIỀN");

            fmt = xls.GetCellVisibleFormatDef(6, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Format = "_(* #,##0_);_(* \\-#,##0_);_(* \"\"_);_(@_)";
            xls.SetCellFormat(6, 2, xls.AddFormat(fmt));

            //Cell selection and scroll position.
            xls.SelectCell(9, 3, false);

            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;
        }
    }    
}    