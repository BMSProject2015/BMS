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
    public class rptLuong_GiaiThich_PhuCapController : Controller
    {
        // create: Thương
        // GET: /rptLuong_GiaiThich_PhuCap/
        public string sViewPath = "~/Report_Views/";       
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_GiaiThich_PhuCap.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Sự kiện submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNam"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);
            String iReport = Convert.ToString(Request.Form[ParentID + "_iReport"]);
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iNam"] = Nam;
            ViewData["iThang"] = Thang;
            ViewData["iReport"] = iReport;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_GiaiThich_PhuCap.aspx";
            String sFilePath = "";
            sFilePath = Path_Report(iReport);
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy đường dẫn tới file Excel mẫu
        /// </summary>
        /// <param name="iReport">Loại báo cáo</param>
        /// <returns></returns>
        private static String Path_Report(String iReport)
        {
            String sFilePath="";
            switch (iReport)
            {
                case "rVKHD":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_ChiTietVuotKhung_HanDinh.xls";
                    break;
                case "rTrN":
                case "rKV":
                case "rDB":
                case "rCV":
                case "rKHAC":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_PhuCap_TrachNhiem.xls";
                    break;
            }
            return sFilePath;
        }
        /// <summary>
        /// Giải thích phụ cấp
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iTrangThai">Trạng thái duyệt</param>
        /// <param name="iReport">Loại báo cáo</param>
        /// <returns></returns>
        public DataTable GiaiThich_PhuCap(String iThang, String iNam, String iTrangThai,String iReport)
        {
            String DKDuyet = iTrangThai.Equals("0") ? "" : "AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            String DKSelect = "";
            String DKGroup = "";
            String DKWhere = "";
            String DKOrder = "";
            switch (iReport)
            {
                //Phụ cấp vượt khung hạn định
                case "rVKHD":
                    DKSelect = @",rHeSo=CASE WHEN L.iID_MaNgachLuong_CanBo=4 THEN L.rPhuCap_TrenHanDinh_HeSo ELSE L.rPhuCap_VuotKhung_HeSo END
                                ,SUM(CASE WHEN L.iID_MaNgachLuong_CanBo = 4 THEN L.rPhuCap_TrenHanDinh ELSE 0 END) AS HD
                                ,SUM(CASE WHEN L.iID_MaNgachLuong_CanBo <>4 THEN L.rPhuCap_VuotKhung ELSE 0 END) AS VK";
                    DKGroup = @"GROUP BY  NL.iID_MaNgachLuong,NL.sTenNgachLuong,L.iID_MaNgachLuong_CanBo,L.rPhuCap_TrenHanDinh_HeSo,L.rPhuCap_VuotKhung_HeSo";
                    DKWhere = @"";
                    DKOrder = @"ORDER BY NL.iID_MaNgachLuong";
                    break;
                //Phụ cấp trách nhiệm
                case "rTrN":
                    DKSelect = @",NL.iSTT     
                                 ,'7'+SUBSTRING(convert(varchar,L.rPhuCap_TrachNhiem_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_TrachNhiem_HeSo),0)-1) rHeSo
                                -- ,N'Mức '+left(CONVERT(varchar,L.rPhuCap_TrachNhiem_HeSo),2)+' % ' rMuc  
                                 ,L.sPhuCap_TrachNhiem_MoTa rMuc                            
                                 ,SUM(l.rPhuCap_TrachNhiem) rPC";
                    DKWhere = @"AND L.rPhuCap_TrachNhiem>0";
                    DKGroup = @"GROUP BY NL.iID_MaNgachLuong,NL.sTenNgachLuong,NL.iSTT,L.rPhuCap_TrachNhiem_HeSo,L.sPhuCap_TrachNhiem_MoTa";
                    DKOrder = @"ORDER BY NL.iSTT";
                    break;
                //Phụ cấp khu vực
                case "rKV":
                    DKSelect = @",NL.iSTT     
                                 ,'K'+SUBSTRING(convert(varchar,L.rPhuCap_KhuVuc_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_KhuVuc_HeSo),0)-1) rHeSo
                                --,N'Mức '+SUBSTRING(convert(varchar,L.rPhuCap_KhuVuc_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_KhuVuc_HeSo),0)-1)+' % ' rMuc                              
                                 ,L.sPhuCap_KhuVuc_MoTa rMuc
                                 ,SUM(l.rPhuCap_KhuVuc) rPC";
                    DKWhere = @"AND L.rPhuCap_KhuVuc>0";
                    DKGroup = @"GROUP BY NL.iID_MaNgachLuong,NL.sTenNgachLuong,NL.iSTT,L.rPhuCap_KhuVuc_HeSo,L.sPhuCap_KhuVuc_MoTa";
                    DKOrder = @"ORDER BY NL.iSTT";
                    break;
                //Phụ cấp đặc biệt
                case "rDB":
                    DKSelect = @",NL.iSTT     
                                 ,'0'+ SUBSTRING(convert(varchar,L.rPhuCap_DacBiet_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_DacBiet_HeSo),0)-1) rHeSo
                                 --,N'Mức '+ SUBSTRING(convert(varchar,L.rPhuCap_DacBiet_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_DacBiet_HeSo),0)-1)+' % ' rMuc     
                                 ,L.sPhuCap_DacBiet_MoTa rMuc                        
                                 ,SUM(l.rPhuCap_DacBiet) rPC";
                    DKWhere = @"AND L.rPhuCap_DacBiet>0";
                    DKGroup = @"GROUP BY NL.iID_MaNgachLuong,NL.sTenNgachLuong,NL.iSTT,L.rPhuCap_DacBiet_HeSo,L.sPhuCap_DacBiet_MoTa,L.sPhuCap_DacBiet_MoTa";
                    DKOrder = @"ORDER BY NL.iSTT";
                    break;
                //Phụ cấp công vụ
                case "rCV":
                    DKSelect = @",NL.iSTT     
                                 ,'0'+SUBSTRING(convert(varchar,L.rPhuCap_CongVu_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_CongVu_HeSo),0)-1) rHeSo
                                 --,N'Mức '+ SUBSTRING(convert(varchar,L.rPhuCap_CongVu_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_CongVu_HeSo),0)-1)+' % ' rMuc                              
                                 ,L.sPhuCap_CongVu_MoTa rMuc
                                 ,SUM(l.rPhuCap_CongVu) rPC";
                    DKWhere = @"AND L.rPhuCap_CongVu>0";
                    DKGroup = @"GROUP BY NL.iID_MaNgachLuong,NL.sTenNgachLuong,NL.iSTT,L.rPhuCap_CongVu_HeSo,L.sPhuCap_CongVu_MoTa";
                    DKOrder = @"ORDER BY NL.iSTT";
                    break;
                //Phụ cấp khác
                case "rKHAC":
                    DKSelect = @",NL.iSTT     
                                 ,'G'+SUBSTRING(convert(varchar,L.rPhuCap_Khac_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_Khac_HeSo),0)-1) rHeSo
                                 --,N'Mức '+SUBSTRING(convert(varchar,L.rPhuCap_Khac_HeSo),1,CHARINDEX('.',convert(varchar,L.rPhuCap_Khac_HeSo),0)-1)+' % ' rMuc                              
                                 ,L.sPhuCap_Khac_MoTa rMuc
                                 ,SUM(l.rPhuCap_Khac) rPC";
                    DKWhere = @"AND L.rPhuCap_Khac>0";
                    DKGroup = @"GROUP BY NL.iID_MaNgachLuong,NL.sTenNgachLuong,NL.iSTT,L.rPhuCap_Khac_HeSo,L.sPhuCap_Khac_MoTa";
                    DKOrder = @"ORDER BY NL.iSTT";
                    break;
            }
            String SQL = String.Format(@"SELECT NL.iID_MaNgachLuong, NL.sTenNgachLuong AS Ten
                                            ,COUNT(L.iID_MaNgachLuong_CanBo) rSN
	                                        ,SUM(L.rLuongCoBan) LC
                                            {0}--Điều kiện select
                                        FROM L_BangLuongChiTiet L
                                        INNER JOIN L_DanhMucNgachLuong NL
                                        ON L.iID_MaNgachLuong_CanBo=NL.iID_MaNgachLuong
                                        WHERE L.iTrangThai=1
                                        AND L.iThangBangLuong=@iThangBangLuong
                                        AND L.iNamBangLuong=@iNamBangLuong
                                        {1}--Điều kiện where
                                        {2}--Điều kiện trạng thái duyệt
                                        {3}--Điều kiện group by
                                        {4}--Điều kiện order by",DKSelect,DKWhere,DKDuyet,DKGroup,DKOrder);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThang);
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNam);
            if (!String.IsNullOrEmpty(DKDuyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iTrangThai);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iTrangThai">Trạng thái duyệt</param>
        /// <param name="iReport">Loại báo cáo</param>
        public void LoadData(FlexCelReport fr, String iThang, String iNam, String iTrangThai,String iReport)
        {
            DataTable dt = new DataTable();
            DataTable dtGroup = new DataTable();         
            dt = GiaiThich_PhuCap(iThang, iNam, iTrangThai,iReport);        
            dtGroup = HamChung.SelectDistinct("Group", dt, "iID_MaNgachLuong", "iID_MaNgachLuong,Ten");
            fr.AddTable("Group", dtGroup);
            fr.AddTable("ChiTiet", dt);
            dtGroup.Dispose();
            dt.Dispose();
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iTrangThai">Trạng thái duyệt</param>
        /// <param name="iReport">Loại báo cáo</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang, String iNam, String iTrangThai,String iReport)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_GiaiThich_PhuCap");
            LoadData(fr, iThang, iNam, iTrangThai,iReport);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Thang", iThang);
            fr.SetValue("Nam", iNam);
            String TenPC = "";
            switch (iReport)
            {
                case "rVKHD":
                    TenPC = "vượt khung & trên hạn định";
                    break;
                case "rTrN":
                    TenPC = "trách nhiệm";
                    break;
                case "rKV":
                    TenPC = "khu vực";
                    break;
                case "rDB":
                    TenPC = "đặc biệt";
                    break;
                case "rCV":
                    TenPC = "công vụ";
                    break;
                case "rKHAC":
                    TenPC = "khác";
                    break;
            }
            fr.SetValue("TenPC", TenPC.ToUpper());
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iTrangThai">Trạng thái</param>
        /// <param name="iReport">Loại báo cáo</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iThang, String iNam, String iTrangThai,String iReport)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(Path_Report(iReport)), iThang, iNam,iTrangThai, iReport);
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
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iTrangThai">Trạng thái duyệt</param>
        /// <param name="iReport">Loại báo cáo</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iThang, String iNam, String iTrangThai, String iReport)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(Path_Report(iReport)), iThang, iNam, iTrangThai,iReport);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "Giai_Thich_PC.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
    }
}
