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
namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptPhanBo_15Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_15.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            
            String iID_MaDotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String KieuTrang = Convert.ToString(Request.Form[ParentID + "_KieuTrang"]);
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["TruongTien"] = TruongTien;
            ViewData["KieuTrang"] = KieuTrang;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_15.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hàm Xem Báo Cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KieuTrang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KieuTrang);
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
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KieuTrang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KieuTrang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KieuTrang == "1")
                {
                    clsResult.FileName = "PhanBo15_Doc.xls";
                }
                else
                {
                    clsResult.FileName = "PhanBo15.xls";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm Xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KieuTrang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KieuTrang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "PhanBo15.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hàm lấy mô tả loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KieuTrang)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            DataTable dt = rptPhanBo_15(MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KieuTrang);
            if (KieuTrang == "1")
            {
                Result = TaoTieuDe_Doc(Result, dt, 4, 4, 3, dt.Columns.Count, 4, 5);
                Filldata(Result, dt, 5, 4, 3, dt.Columns.Count, 4, 5, "1,2,3|0,1,2");
            }
            else
            {
                Result = TaoTieuDe(Result, dt, 4, 4, 3, dt.Columns.Count, 7, 8);
                Filldata(Result, dt, 5, 4, 3, dt.Columns.Count, 7, 8, "1,2,3|0,1,2");
            }
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND,iID_MaTrangThaiDuyet,TruongTien);
            String tendot = "";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    try
                    {
                        DateTime ngay =Convert.ToDateTime(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString());
                        tendot = "Tháng  " + ngay.ToString("MM") + "  Năm  " + ngay.ToString("yyyy");
                    }
                    catch { tendot = ""; }
                }
            }
            String Dot = String.Format("Bổ Sung Lần {0} : {1} ", ReportModels.Get_STTDotPhanBo(MaND,iID_MaTrangThaiDuyet, iID_MaDotPhanBo)-1, tendot);
            String TenTruongTien = "";
            String[] sLNS = new String[100];
            DataTable dtLNS = DanhSach_LoaiNS(MaND,iID_MaDotPhanBo,iID_MaTrangThaiDuyet, TruongTien);
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
               
                sLNS[i] = Convert.ToString(dtLNS.Rows[i]["sMoTa"]);
            }
            switch (TruongTien)
            {
                case "rTuChi": TenTruongTien = " TỰ CHI";
                    break;
                case "rHienVat": TenTruongTien = "HIỆN VẬT";
                    break;
            }
            //Ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //Lấy thông tin chữ kí
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_15");
                fr.SetValue("TruongTien", TenTruongTien);
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"].ToString())
                {
                    fr.SetValue("Dot", "");
                }
                else
                {
                    fr.SetValue("Dot", Dot);
                }
                for (int i = 0; i < sLNS.Length; i++)
                {
                    fr.SetValue("LNS" + Convert.ToString(i), sLNS[i]);
                }
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                fr.Run(Result);
                return Result;
            


        }
        public static DataTable TenLNS(String sLNS)
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT sMoTa FROM PB_PhanBoChiTiet WHERE iTrangThai=1 AND sL='' WHERE slNS=@slNS ORDER BY sLNS");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS",sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
        /// <summary>
        /// DataTable dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public DataTable rptPhanBo_15(String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KieuTrang)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            DataTable dtLNS = DanhSach_LoaiNS(MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien);
            String DKDLNS = ""; String iID_LNS = "";
            String DKSUMLNS = "", DKHAVINGLNS = "";
           
                if (dtLNS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLNS.Rows.Count; i++)
                    {
                        DKSUMLNS += ",SUM(LNS" + i + ") AS LNS" + i;
                        DKHAVINGLNS += " OR SUM(LNS" + i + ")<>0 ";
                        iID_LNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                        DKDLNS += ",LNS" + i + " = CASE WHEN sLNS=" + "'" + iID_LNS + "'" +
                                                 " AND PB.iTrangThai=1 THEN SUM({0}) ELSE 0 END";
                    }
                    DKDLNS = String.Format(DKDLNS, TruongTien);
                }
                else
                {
                    iID_MaDotPhanBo = Guid.Empty.ToString();
                }
            

            String DKLNSSELECT = "";
            //DK Dot phan bo
            String S = "";
            if (dtLNS.Rows.Count > 0)
            {
                for (int i = 0; i < dtLNS.Rows.Count; i++)
                {
                    S += " OR " + "sLNS= '" + dtLNS.Rows[i]["sLNS"].ToString() + "'";
                    DKLNSSELECT = "(sLNS = " + "'" + Convert.ToString(dtLNS.Rows[0]["sLNS"] + "'") + S + ") ";
                }
            }
            else
            {
                DKLNSSELECT = "sLNS='" + Guid.Empty.ToString() + "'";
            }

            String SQL = String.Format(@"SELECT PhanBo.iID_MaDonVi,PhanBo.sTen
                                        ,SUM(TongLoaNS) as TongCong
                                        {3}
                                        FROM 
                                        (
                                        SELECT c.iID_MaDonVi,c.sTen
                                        ,TongLoaNS=CASE WHEN {1}
                                        AND PB.iTrangThai=1 THEN SUM({2}) ELSE 0 END
                                        {0}
                                        FROM PB_PhanBoChiTiet as PB 
                                        INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as c ON PB.iID_MaDonVi=c.iID_MaDonVi
                                        WHERE sNG<>'' AND iID_MaDotPhanBo=@iID_MaDotPhanBo {5} {6}
                                        GROUP BY  sLNS,iNamLamViec,c.sTen,PB.iID_MaDotPhanBo,PB.iTrangThai,c.iID_MaDonVi
                                        HAVING SUM({2})!=0
                                        )
                                        as PhanBo
                                        GROUP BY PhanBo.sTen,PhanBo.iID_MaDonVi
                                        HAVING  SUM(TongLoaNS)!=0 {4} ", DKDLNS, DKLNSSELECT, TruongTien, DKSUMLNS, DKHAVINGLNS,DK_Duyet, ReportModels.DieuKien_NganSach(MaND,"PB"));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iNamLamViec",NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Json Đợt phân bổ lấy từ Obj_DanhSachDotPhanBo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet,String TruongTien)
        {

            return Json(obj_DanhSachDotPhatBo(ParentID, MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet,TruongTien), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy đợt phân bổ fill vào DropDowlist
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public String obj_DanhSachDotPhatBo(String ParentID, String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet,String TruongTien)
        {
            DataTable dtDotPhatBo = DanhSach_DotPhanBo(MaND, iID_MaTrangThaiDuyet,TruongTien);
            SelectOptionList slDotPhatBo = new SelectOptionList(dtDotPhatBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            String StrDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slDotPhatBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width:70%;\"");
            return StrDotPhanBo;
        }
        /// <summary>
        /// Danh sách đợt phân bổ theo năm làm việc
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DanhSach_DotPhanBo(String MaND, String iID_MaTrangThaiDuyet,String TruongTien)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = " AND PB.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }

            String SQL = String.Format(@" SELECT  PB.iID_MaDotPhanBo,CONVERT(varchar(10),dNgayDotPhanBo,103) as dNgayDotPhanBo
                                        FROM PB_PhanBoChiTiet as PB
                                        INNER JOIN PB_DotPhanBo as DPB ON PB.iID_MaDotPhanBo=DPB.iID_MaDotPhanBo 
                                        WHERE PB.iTrangThai=1 {0} {1} AND {2}>0
                                        GROUP BY PB.iID_MaDotPhanBo,dNgayDotPhanBo
                                        ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)", DK_Duyet, ReportModels.DieuKien_NganSach(MaND,"PB"),TruongTien);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDotPhanBo"] = Guid.Empty.ToString();
                R["dNgayDotPhanBo"] = "Không có dữ liệu";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
        /// <summary>
        /// Hàm lấy danh sách loại ngân sách theo đợt phân bổ
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public static DataTable DanhSach_LoaiNS(String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT DISTINCT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa FROM PB_PhanBoChiTiet
                                            WHERE sL='' AND iID_MaDotPhanBo=@iID_MaDotPhanBo
                                            {2} AND LEN(sLNS)=7 AND SUBSTRING(sLNS,1,1)=1 OR SUBSTRING(sLNS,1,2)=2
                                           ORDER BY sLNS", TruongTien, DK_Duyet, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// tạo tiêu để cho báo cáo
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="dt"></param>
        /// <param name="TuHang"></param>
        /// <param name="TuCot"></param>
        /// <param name="TuCotCua_DT"></param>
        /// <param name="DenCotCua_DT"></param>
        /// <param name="SoCotTrang1"></param>
        /// <param name="SoCotTrangLonHon1"></param>
        /// <returns></returns>
        public XlsFile TaoTieuDe(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;



            #region //Styles.
            //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "#,##0;-#,##0;;@";
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
            StyleFmt.Format = "#,##0;-#,##0;;@";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "#,##0;-#,##0;;@";
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
            StyleFmt.Format = "#,##0;-#,##0;;@";
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
            #endregion

            #region  //Named Ranges

            //Named Ranges
            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$A:$B,'Sheet1'!$1:$4");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);

            #endregion

            #region //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = false;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "&L&\"Times New Roman,Bold\"              <#Cap1>\n     <#Cap2>&C&\"Times New Roman,Bold\"TỔNG HỢP CẤP NGÂN SÁCH - <#TruongTien>\n<#Dot>&R&\"Times New Roman,Italic\"\n\n\nĐơn vị tính:1000 đồng      Trang:&P/&N                      ";
            HeadersAndFooters.DefaultFooter = "";
            HeadersAndFooters.FirstHeader = "";
            HeadersAndFooters.FirstFooter = "";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.708661417322835;
            //    PrintMargins.Top = 0.393700787401575;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.590551181102362;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.708661417322835, 0.393700787401575, 0.196850393700787, 0.590551181102362, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.None;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x00, 0x00, 0x03, 0x2F, 0x00, 0x00, 0x02, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x03, 0x00, 0x01, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);


            #endregion
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;
               
                    SoCotCanThem = SoCotTrang1 - SoCotDu;
               
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;
                
                    SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                    TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
                
                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            #region //Set up rows and columns
            //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 1170);    //(3.82 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 4827);    //(18.11 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 3657);    //(13.54 + 0.75) * 256

            for (int i = 0; i < TongSoCot; i++)
            {
                xls.SetColWidth(_TuCot + i, 3600);
            }
            xls.SetRowHeight(4, 800);
            xls.DefaultRowHeight = 300;
            #endregion
            #region MagerCell
            #endregion

            #region //Set the cell values
            #region set tieu de cot tinh
            TFlxFormat fmt;

            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "STT");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));
            xls.SetCellValue(4, 2, "Tên đơn vị");

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));
            xls.SetCellValue(4, 3, "Tổng cộng");

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));
            xls.SetCellValue(4, 4, 1);

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));
            xls.SetCellValue(4, 5, 2);

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));
            xls.SetCellValue(4, 6, 3);

            fmt = xls.GetCellVisibleFormatDef(4, 7);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 7, xls.AddFormat(fmt));
            xls.SetCellValue(4, 7, 4);

            fmt = xls.GetCellVisibleFormatDef(4, 8);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 8, xls.AddFormat(fmt));
            xls.SetCellValue(4, 8, 5);

            fmt = xls.GetCellVisibleFormatDef(4, 9);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 9, xls.AddFormat(fmt));
            xls.SetCellValue(4, 9, 6);

            fmt = xls.GetCellVisibleFormatDef(4, 10);
            fmt.Font.Name = "Times New Roman";
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
            fmt.WrapText = true;
            xls.SetCellFormat(4, 10, xls.AddFormat(fmt));
            xls.SetCellValue(4, 10, 7);

            #endregion
            #region cau hinh chu ku

            xls.MergeCells(TongSoHang + TuHang + 4, 1, TongSoHang + TuHang + 4, 2);
            xls.MergeCells(TongSoHang + TuHang + 4, 3, TongSoHang + TuHang + 4, 4);
            xls.MergeCells(TongSoHang + TuHang + 4, 5, TongSoHang + TuHang + 4, 6);
            xls.MergeCells(TongSoHang + TuHang +4, 7, TongSoHang + TuHang + 4, 8);
            xls.MergeCells(TongSoHang + TuHang + 4, 9, TongSoHang + TuHang + 4, 10);
            // Thua lenh - chuc danh - ten 
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 4, 1, "<#row height(autofit)><#ThuaLenh1> \n<#ChucDanh1>\n\n\n\n\n\n\n<#Ten1>");

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang +4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 3, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 4, 3, "<#row height(autofit)><#ThuaLenh2> \n<#ChucDanh2>\n\n\n\n\n\n\n<#Ten2>");

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 5, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang +4, 5, "<#row height(autofit)><#ThuaLenh3> \n<#ChucDanh3>\n\n\n\n\n\n\n<#Ten3>");

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 5, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 5, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 5, 7, "<#row height(autofit)><#ThuaLenh4>\n<#ChucDanh4>\n\n\n\n\n\n\n<#Ten4>");

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 9, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 4, 9, "<#row height(autofit)><#ThuaLenh5>\n<#ChucDanh5>\n\n\n\n\n\n\n<#Ten5>");
            #endregion
            #region set tieu de cot dong
            #region set hang LNS
            _TuCot = TuCot;
            //neu so trang =1
          
                //fmt = xls.GetCellVisibleFormatDef(3, _TuCot +5);
                //fmt.Font.Name = "Times New Roman";
                //fmt.Font.Style = TFlxFontStyles.Italic;
                //fmt.Font.Family = 1;
                //xls.SetCellFormat(3, _TuCot + 5, xls.AddFormat(fmt));
                //xls.SetCellValue(3, _TuCot + 5, "Đơn vị tính: 1000 đ");

               // ngay
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 9);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 9);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Italic;
                fmt.HAlignment = THFlxAlignment.left;
                fmt.VAlignment = TVFlxAlignment.bottom;
                xls.SetRowHeight(TongSoHang + TuHang + 2, 400);
                fmt.Font.Family = 1;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 9, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 9, "<#Ngay>");
                
                for (int j = _TuCot + 1; j <= _TuCot + SoCotTrang1 - 1; j++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                }
            
               _TuCot = _TuCot + SoCotTrang1;
                //set cac trang con lai            
                for (int i = 1; i < SoTrang; i++)
                {
                    //fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 5);
                    //fmt.Font.Name = "Times New Roman";
                    //fmt.Font.Style = TFlxFontStyles.Italic;
                    //fmt.Font.Family = 1;
                    //xls.SetCellFormat(3, _TuCot + 5, xls.AddFormat(fmt));
                    //xls.SetCellValue(3, _TuCot + 5, "Đơn vị tính: 1000 đ");


                    //Ngay
                    fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 10 + SoCotTrangLonHon1 * i);
                    fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 10 + SoCotTrangLonHon1 * i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Style = TFlxFontStyles.Italic;
                    fmt.Font.Family = 1;
                    fmt.HAlignment = THFlxAlignment.right;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(TongSoHang + TuHang + 1, 10 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang + 1, 10 + SoCotTrangLonHon1 * i, "<#Ngay>");
                    
                    //cau hinh chu ki cho trang lon hon 1
                    fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 3 + SoCotTrangLonHon1 * i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 220;
                    fmt.Font.Family = 1;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    fmt.WrapText = true;
                    xls.SetCellFormat(TongSoHang + TuHang + 4, 3 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang + 4, 3 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh2> \n<#ChucDanh2>\n\n\n\n\n\n\n<#Ten2>");

                    fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 5 + SoCotTrangLonHon1 * i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 220;
                    fmt.Font.Family = 1;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    fmt.WrapText = true;
                    xls.SetCellFormat(TongSoHang + TuHang +4, 5 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang + 4, 5 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh3> \n<#ChucDanh3>\n\n\n\n\n\n\n<#Ten3>");

                    fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 7 + SoCotTrangLonHon1 * i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 220;
                    fmt.Font.Family = 1;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    fmt.WrapText = true;
                    xls.SetCellFormat(TongSoHang + TuHang + 4, 7 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang + 4, 7 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh4> \n<#ChucDanh4>\n\n\n\n\n\n\n<#Ten4>");

                    fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang +4, 9 + SoCotTrangLonHon1 * i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 220;
                    fmt.Font.Family = 1;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    fmt.WrapText = true;
                    xls.SetCellFormat(TongSoHang + TuHang + 4, 9 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang + 4, 9 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh5> \n<#ChucDanh5>\n\n\n\n\n\n\n<#Ten5>");
                    //
                    for (int j = _TuCot + 1; j <= _TuCot + SoCotTrangLonHon1 - 1; j++)
                    {
                        fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 160;
                        fmt.Font.Style = TFlxFontStyles.Bold;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.center;
                        fmt.VAlignment = TVFlxAlignment.center;
                        xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                    }
                    _TuCot = _TuCot + SoCotTrangLonHon1;
                }
            
            #endregion
            #endregion
            #region set cac cot loai ngan sach
             _TuCot = TuCot;
            String TenCot;
            int _TuCotCua_DT = TuCotCua_DT;
            if (SoTrang == 1)
            {
                for (int i = 0; i < TongSoCot; i++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot + i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.WrapText = true;
                    fmt.VAlignment = TVFlxAlignment.center;
                    TenCot = "";
                    if (DenCotCua_DT > _TuCotCua_DT)
                    {
                        TenCot = "<#LNS" + i + ">";
                    }
                    xls.SetCellFormat(4, _TuCot + i, xls.AddFormat(fmt));
                    xls.SetCellValue(4, _TuCot + i, TenCot);
                }
            }
            else
            {
                for (int i = 0; i < SoCotTrang1; i++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot + i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.WrapText = true;
                    fmt.VAlignment = TVFlxAlignment.center;
                    TenCot = "";
                    if (DenCotCua_DT > _TuCotCua_DT)
                    {
                        TenCot = "<#LNS" + i + ">";
                    }
                    xls.SetCellFormat(4, _TuCot + i, xls.AddFormat(fmt));
                    xls.SetCellValue(4, _TuCot + i, TenCot);
                }
                _TuCotCua_DT = _TuCotCua_DT + SoCotTrang1;
                _TuCot = _TuCot + SoCotTrang1;
                for (int i = 0; i < TongSoCot - SoCotTrang1; i++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot + i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.WrapText = true;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    TenCot = "";
                    int a = Convert.ToInt16(SoCotTrang1) + i;
                    if (DenCotCua_DT > _TuCotCua_DT)
                    {
                        TenCot = "<#LNS" + a + ">";
                    }
                    xls.SetCellFormat(4, _TuCot + i, xls.AddFormat(fmt));
                    xls.SetCellValue(4, _TuCot + i, TenCot);
                }
            }
            #endregion
            #endregion
            //Cell selection and scroll position.


            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;

           

        }

        public XlsFile TaoTieuDe_Doc(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "BaoCao";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;

            //Sheet Options
            xls.SheetName = "BaoCao";


            #region //Styles.
            //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "#,##0;-#,##0;;@";
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
            StyleFmt.Format = "#,##0;-#,##0;;@";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "#,##0;-#,##0;;@";
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
            StyleFmt.Format = "#,##0;-#,##0;;@";
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
            #endregion

            #region  //Named Ranges

            //Named Ranges
            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='BaoCao'!$A:$C,'BaoCao'!$1:$4");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);

            #endregion

            #region //Printer Settings

            //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = false;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "&L&\"Times New Roman,Bold\"<#Cap1>\n <#Cap2>&C&\"Times New Roman,Bold\"TỔNG HỢP CẤP NGÂN SÁCH - <#TruongTien>\n<#Dot>&R&\"Times New Roman,Italic\"\n\n\n\n\nĐơn vị tính:1000 đồng      Trang:&P/&N            ";
            HeadersAndFooters.DefaultFooter = "";
            HeadersAndFooters.FirstHeader = "";
            HeadersAndFooters.FirstFooter = "";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.393700787401575;
            //    PrintMargins.Top = 0.590551181102362;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.590551181102362;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.393700787401575, 0.590551181102362, 0.196850393700787, 0.590551181102362, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.Orientation;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x00, 0x00, 0x03, 0x2F, 0x00, 0x00, 0x01, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x03, 0x00, 0x01, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);

            #endregion
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;

                SoCotCanThem = SoCotTrang1 - SoCotDu;

                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;

                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;

                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            #region //Set up rows and columns
            //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 1170);    //(3.82 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.VAlignment = TVFlxAlignment.center;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 6400);    //(24.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            ColFmt.VAlignment = TVFlxAlignment.center;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 4059);    //(15.11 + 0.75) * 256

            for (int i = 0; i < TongSoCot; i++)
            {
                xls.SetColWidth(_TuCot + i, 3400);
            }
            xls.SetRowHeight(4, 800);
            xls.DefaultRowHeight = 300;
            #endregion
            #region MagerCell
            #endregion

            #region //Set the cell values
            #region set tieu de cot tinh
            TFlxFormat fmt;
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
            xls.SetCellValue(4, 1, "STT");

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
            xls.SetCellValue(4, 2, "Tên đơn vị");

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
            xls.SetCellValue(4, 3, "Tổng cộng");

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


            fmt = xls.GetCellVisibleFormatDef(4, 7);
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
            xls.SetCellFormat(4, 7, xls.AddFormat(fmt));



            #endregion
            #region cau hinh chu ku
            xls.MergeCells(TongSoHang + TuHang + 4, 1, TongSoHang + TuHang + 4, 2);
            xls.MergeCells(TongSoHang + TuHang + 4, 3, TongSoHang + TuHang + 4, 4);
            xls.MergeCells(TongSoHang + TuHang + 4, 5, TongSoHang + TuHang + 4, 7);

            // Thua lenh - chuc danh - ten 
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 4, 1, "<#row height(autofit)><#ThuaLenh1> \n<#ChucDanh1>\n\n\n\n\n\n\n<#Ten1>");

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 3, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 4, 3, "<#row height(autofit)><#ThuaLenh3> \n<#ChucDanh3>\n\n\n\n\n\n\n<#Ten3>");

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 220;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 4, 5, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 4, 5, "<#row height(autofit)><#ThuaLenh5> \n<#ChucDanh5>\n\n\n\n\n\n\n<#Ten5>");

            #endregion
            #region set tieu de cot dong
            #region set hang LNS
            _TuCot = TuCot;
            //neu so trang =1

            //fmt = xls.GetCellVisibleFormatDef(3, _TuCot +5);
            //fmt.Font.Name = "Times New Roman";
            //fmt.Font.Style = TFlxFontStyles.Italic;
            //fmt.Font.Family = 1;
            //xls.SetCellFormat(3, _TuCot + 5, xls.AddFormat(fmt));
            //xls.SetCellValue(3, _TuCot + 5, "Đơn vị tính: 1000 đ");

            // ngay
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2,6);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Italic;
            fmt.HAlignment = THFlxAlignment.left;
            fmt.VAlignment = TVFlxAlignment.bottom;
            xls.SetRowHeight(TongSoHang + TuHang + 2, 400);
            fmt.Font.Family = 1;
            xls.SetCellFormat(TongSoHang + TuHang + 2,6, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 2, 6, "<#Ngay>");

            for (int j = _TuCot + 1; j <= _TuCot + SoCotTrang1 - 1; j++)
            {
                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, j, xls.AddFormat(fmt));
            }

            _TuCot = _TuCot + SoCotTrang1;
            //set cac trang con lai            
            for (int i = 1; i < SoTrang; i++)
            {
                //fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 5);
                //fmt.Font.Name = "Times New Roman";
                //fmt.Font.Style = TFlxFontStyles.Italic;
                //fmt.Font.Family = 1;
                //xls.SetCellFormat(3, _TuCot + 5, xls.AddFormat(fmt));
                //xls.SetCellValue(3, _TuCot + 5, "Đơn vị tính: 1000 đ");


                //Ngay
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1,6+ SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 6 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Italic;
                fmt.Font.Family = 1;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(TongSoHang + TuHang + 1, 6 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 1, 6 + SoCotTrangLonHon1 * i, "<#Ngay>");

                //cau hinh chu ki cho trang lon hon 1
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 3 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 220;
                fmt.Font.Family = 1;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 4, 3 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 4, 3 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh3> \n<#ChucDanh3>\n\n\n\n\n\n\n<#Ten3>");

                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 5 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 220;
                fmt.Font.Family = 1;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 4, 5 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 4, 5 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh5> \n<#ChucDanh5>\n\n\n\n\n\n\n<#Ten5>");

               
                //
                for (int j = _TuCot + 1; j <= _TuCot + SoCotTrangLonHon1 - 1; j++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                }
                _TuCot = _TuCot + SoCotTrangLonHon1;
            }

            #endregion
            #endregion
            #region set cac cot loai ngan sach
            _TuCot = TuCot;
            String TenCot;
            int _TuCotCua_DT = TuCotCua_DT;
            if (SoTrang == 1)
            {
                for (int i = 0; i < TongSoCot; i++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot + i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.WrapText = true;
                    fmt.VAlignment = TVFlxAlignment.center;
                    TenCot = "";
                    if (DenCotCua_DT > _TuCotCua_DT)
                    {
                        TenCot = "<#LNS" + i + ">";
                    }
                    xls.SetCellFormat(4, _TuCot + i, xls.AddFormat(fmt));
                    xls.SetCellValue(4, _TuCot + i, TenCot);
                }
            }
            else
            {
                for (int i = 0; i < SoCotTrang1; i++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot + i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.WrapText = true;
                    fmt.VAlignment = TVFlxAlignment.center;
                    TenCot = "";
                    if (DenCotCua_DT > _TuCotCua_DT)
                    {
                        TenCot = "<#LNS" + i + ">";
                    }
                    xls.SetCellFormat(4, _TuCot + i, xls.AddFormat(fmt));
                    xls.SetCellValue(4, _TuCot + i, TenCot);
                }
                _TuCotCua_DT = _TuCotCua_DT + SoCotTrang1;
                _TuCot = _TuCot + SoCotTrang1;
                for (int i = 0; i < TongSoCot - SoCotTrang1; i++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot + i);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.WrapText = true;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    TenCot = "";
                    int a = Convert.ToInt16(SoCotTrang1) + i;
                    if (DenCotCua_DT > _TuCotCua_DT)
                    {
                        TenCot = "<#LNS" + a + ">";
                    }
                    xls.SetCellFormat(4, _TuCot + i, xls.AddFormat(fmt));
                    xls.SetCellValue(4, _TuCot + i, TenCot);
                }
            }
            #endregion
            #endregion
            //Cell selection and scroll position.

            xls.SelectCell(12, 3, false);

            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;



        }
        /// <summary>
        /// Hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="dt"></param>
        /// <param name="TuHang"></param>
        /// <param name="TuCot"></param>
        /// <param name="TuCotCua_DT"></param>
        /// <param name="DenCotCua_DT"></param>
        /// <param name="SoCotTrang1"></param>
        /// <param name="SoCotTrangLonHon1"></param>
        /// <param name="MapCotCoDinh"></param>
        public void Filldata(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, String MapCotCoDinh)
        {
            TFlxFormat fmt;
            Object GiaTriO;
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;


                    SoCotCanThem = SoCotTrang1 - SoCotDu;
                
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;
               
                    SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                    TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
                
                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            //set border cho cot can them
            for (int c = DenCotCua_DT - TuCotCua_DT + TuCot; c < TongSoCot + TuCot; c++)
            {
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    fmt = xls.GetCellVisibleFormatDef(h + TuHang, c);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 160;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.left;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(h + TuHang, c, xls.AddFormat(fmt));
                }
            }
            String[] arrMapCot = MapCotCoDinh.Split('|');
            String[] arrCot_Excel = arrMapCot[0].Split(',');
            String[] arrCot_DT = arrMapCot[1].Split(',');

            #region Fill dữ liệu những cột động
            _TuCot = TuCot;
            int d = 0;
            for (int c = 0; c < TongSoCot; c++)
            {
                Type _Type = typeof(String);
                if (c + TuCotCua_DT < DenCotCua_DT)
                    _Type = dt.Columns[c + TuCotCua_DT].DataType;
                switch (_Type.ToString())
                {
                    case "System.Decimal":
                        fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 160;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.center;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "_(* #,###_);_(* \\(#,###\\);_(* \"\"??_);_(@_)";
                        break;
                    default:
                        fmt = xls.GetCellVisibleFormatDef(TuHang, 2);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 160;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.right;
                        fmt.VAlignment = TVFlxAlignment.center;
                        break;
                }
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    GiaTriO = null;
                    xls.SetCellFormat(h + TuHang, _TuCot, xls.AddFormat(fmt));
                    if (c + TuCotCua_DT < DenCotCua_DT)
                        xls.SetCellValue(h + TuHang, _TuCot, dt.Rows[h][c + TuCotCua_DT]);
                }
                _TuCot++;
            }
            #endregion

            #region Fill dữ liệu những cột tĩnh
            _TuCot = TuCot;
            String KyTu1, KyTu2, strSum;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int c = 0; c < arrCot_Excel.Length; c++)
                {
                    fmt = xls.GetCellVisibleFormatDef(TuHang + i, Convert.ToInt32(arrCot_Excel[c]));
                    if (c >= arrCot_Excel.Length - 3)
                    {
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 160;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "_(* #,###_);_(* \\(#,###\\);_(* \"\"??_);_(@_)";

                    }
                    else
                    {
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 160;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;


                    }
                    fmt.WrapText = true;
                    xls.AutofitRow(i + TuHang, true, 1);
                    GiaTriO = null;
                    if (c < arrCot_DT.Length)
                        GiaTriO = dt.Rows[i][Convert.ToInt16(arrCot_DT[c])];
                    if (c < arrCot_Excel.Length)
                    {
                        xls.SetCellFormat(i + TuHang, Convert.ToInt16(arrCot_Excel[c]), xls.AddFormat(fmt));
                        xls.SetCellValue(i + TuHang, Convert.ToInt16(arrCot_Excel[c]), GiaTriO);
                    }
                }
            }
            _TuCot = TuCot;

            //set tiêu đề cho hàng tổng số
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 1);
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

            xls.SetCellFormat(TongSoHang + TuHang, 2, xls.AddFormat(fmt));
            
            xls.SetRowHeight(TongSoHang + TuHang, 400);
            for (int i = 1; i < TongSoCot + TuCotCua_DT; i++)
            {
                xls.SetCellFormat(TongSoHang + TuHang, i, xls.AddFormat(fmt));
            }

            for (int i = 0; i <= TongSoCot + 2; i++)
            {
                xls.SetCellValue(TongSoHang + TuHang, 2, "Tổng Cộng:                  ");
                fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 180;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Format = "#,##0;-#,##0;;@";
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang, _TuCot - 3 + i, xls.AddFormat(fmt));
                KyTu1 = HamChung.ExportExcel_MaCot(_TuCot - 3 + i);
                if (TongSoHang > 1)
                {
                    strSum = String.Format("=SUMIF(C{1}:C{3},\"<>\"&\"\",{0}{1}:{2}{3})", KyTu1, TuHang, KyTu1, TongSoHang + TuHang - 1);
                    xls.SetCellFormat(TongSoHang + TuHang, _TuCot - 3 + i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang, _TuCot - 3 + i, new TFormula(strSum));
                }
            }

            #endregion

            
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
