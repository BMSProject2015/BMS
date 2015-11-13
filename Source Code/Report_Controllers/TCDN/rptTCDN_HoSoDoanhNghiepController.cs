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

namespace VIETTEL.Report_Controllers.TCDN
{
    public class rptTCDN_HoSoDoanhNghiepController : Controller
    {
        // Edit: Thương
        // GET: /rptTCDN_HoSoDoanhNghiep/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_HoSoDN_01_BC_DNK.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["pageload"] = 0;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_HoSoDoanhNghiep.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["pageload"] = 1;
            String iMaDN = Convert.ToString(Request.Form[ParentID + "_iMaDN"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);            
            ViewData["iQuy"] = iQuy;
            ViewData["iNam"] = iNam;
            ViewData["iMaDN"] = iMaDN;           
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_HoSoDoanhNghiep.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);            
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <param name="DKNot">Loại chỉ tiêu tài chính doanh nghiệp 0 hoặc 1</param>
        /// <param name="Orderby">Sắp xếp theo trường</param>
        /// <returns></returns>        
        public DataTable HoSoDoanhNghiep(String iQuy, String iNam, String iMaDN)
        {
            String DKQuy = "";
            String DKSelect = "";
            String DKNam = "";
            SqlCommand cmd = new SqlCommand();                              
            switch (iQuy)
            { 
                case "1":                
                    DKSelect = @",SUM(HS.rNamBaoCao) rNamBaoCao ,SUM(HS.rNamTruoc) rNamTruoc";
                    DKQuy = "AND HS.iQuy=@iQuy";
                    DKNam = "AND HS.iNamLamViec=@iNamLamViec";
                    break;
                case "-1":
                case "2":
                case "3":
                case "4":
                    DKSelect = @",rNamBaoCao=SUM(CASE WHEN HS.iQuy=@iQuy   THEN HS.rNamBaoCao ELSE 0 END)
                                 ,rNamTruoc =SUM(CASE WHEN HS.iQuy=@iQuy-1 THEN HS.rNamBaoCao ELSE 0 END)";
                    DKQuy = "";
                    DKNam = "AND HS.iNamLamViec=@iNamLamViec";
                    break;
                case "5":
                case "6":
                    DKSelect = @",rNamBaoCao=SUM(CASE WHEN HS.iQuy=@iQuy-3 THEN HS.rNamBaoCao ELSE 0 END) ,rNamTruoc=SUM(CASE WHEN HS.iQuy=1 THEN HS.rNamTruoc ELSE 0 END)";
                    DKQuy = "";
                    DKNam = "AND HS.iNamLamViec=@iNamLamViec";
                    break;
                case "7":
                    DKSelect = @",rNamBaoCao=SUM(CASE WHEN HS.iQuy=@iQuy-3 AND HS.iNamLamViec=@iNamLamViec THEN HS.rNamBaoCao ELSE 0 END)
                                 ,rNamTruoc =SUM(CASE WHEN HS.iQuy=1 AND HS.iNamLamViec=@iNamLamViec THEN HS.rNamTruoc ELSE 0 END)";
                    DKQuy = "";
                    DKNam = "";
                    break;
            }      
            String strTruyVan = String.Format(@"SELECT 
                                                sTen=CASE WHEN HS.bLaHangCha='TRUE' THEN HS.sTen
                                                          ELSE
			                                                CASE WHEN HS.iID_MaChiTieuHoSo_Cha=0 THEN HS.sTen
			                                                     ELSE CASE WHEN HS.sTen LIKE N'Trong đó%' OR HS.sTen LIKE N'-%' THEN HS.sTen ELSE ' - ' + HS.sTen END
		                                                    END
                                                    END
                                                ,HS.sDonViTinh
                                                ,TK1=CASE WHEN HS.bLaHangCha='TRUE' THEN CONVERT(VARCHAR,HS.iID_MaChiTieuHoSo) ELSE CASE WHEN HS.iID_MaChiTieuHoSo_Cha=0 THEN CONVERT(VARCHAR,HS.iID_MaChiTieuHoSo)ELSE CONVERT(VARCHAR, HS.iID_MaChiTieuHoSo_Cha) END END
                                                ,TK2=CASE WHEN HS.bLaHangCha='FALSE' AND HS.iID_MaChiTieuHoSo_Cha<>0 THEN CONVERT(VARCHAR,HS.iID_MaChiTieuHoSo) ELSE '' END
                                                {1}
                                                ,TT=''
                                                ,HS.iSTT
                                                FROM TCDN_HoSoDoanhNghiepChiTiet AS HS
                                                WHERE HS.iTrangThai=1
                                                  AND HS.sKyHieu IN(SELECT CT.sKyHieu FROM TCDN_HoSoDoanhNghiep_ChiTieu AS CT WHERE CT.iTrangThai=1 AND CT.iLoai=2)
                                                  AND HS.iID_MaDoanhNghiep=@iID_MaDoanhNghiep
                                                  {2}
                                                  {0}                            
                                                GROUP BY HS.iSTT
		                                                ,HS.sKyHieu
		                                                ,HS.bLaHangCha
		                                                ,HS.iID_MaChiTieuHoSo
		                                                ,HS.iID_MaChiTieuHoSo_Cha
		                                                ,HS.sTen
		                                                ,HS.iID_MaChiTieuHoSo
		                                                ,HS.iID_MaChiTieuHoSo_Cha		
		                                                ,HS.sDonViTinh
                                                ORDER BY HS.iSTT", DKQuy,DKSelect,DKNam);            
            cmd.CommandText = strTruyVan;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iMaDN);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dtTruyVan = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtTruyVan;
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn đến file excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>       
        public ExcelFile CreateReport(String path, String iQuy, String iNam, String iMaDN)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_HoSoDoanhNghiep");
            DataTable dt = TCSN_DoanhNghiepModels.Get_DoanhNghiep_Row(iMaDN);
            String sTenDoanhNghiep = "", sSoDanhKy = "", dNgayDangKy = "", sNganhNgheKinhDoanh = "", sDiaChi = "", sDienThoai = "", sFax = "",
            sMaChungKhoan = "", rVonDieuLe = "", rVonNhaNuoc = "", sNguoiDaiDienVonNhaNuoc = "", sSoCoPhan = "",
            sChucVu = "";
            if (dt.Rows.Count > 0)
            {
                DataRow R = dt.Rows[0];
                sTenDoanhNghiep = Convert.ToString(R["sTenDoanhNghiep"]);
                sSoDanhKy = Convert.ToString(R["sSoDanhKy"]);
                dNgayDangKy = Convert.ToString(R["dNgayDangKy"]);
                sNganhNgheKinhDoanh = Convert.ToString(R["sNganhNgheKinhDoanh"]);
                sDiaChi = Convert.ToString(R["sDiaChi"]);
                sDienThoai = Convert.ToString(R["sDienThoai"]);
                sFax = Convert.ToString(R["sFax"]);
                sMaChungKhoan = Convert.ToString(R["sMaChungKhoan"]);
                rVonDieuLe = Convert.ToString(R["rVonDieuLe"]);
                rVonNhaNuoc = Convert.ToString(R["rVonNhaNuoc"]);
                sNguoiDaiDienVonNhaNuoc = Convert.ToString(R["sNguoiDaiDienVonNhaNuoc"]);
                sSoCoPhan = Convert.ToString(R["sSoCoPhan"]);
                sChucVu = Convert.ToString(R["sChucVu"]);
            }
            LoadData(fr, iQuy, iNam, iMaDN);
            fr.SetValue("sTenDoanhNghiep", sTenDoanhNghiep);
            fr.SetValue("sSoDanhKy", sSoDanhKy);
            fr.SetValue("dNgayDangKy", dNgayDangKy.Equals("") ? dNgayDangKy : String.Format("{0:dd - MM - yyyy}", Convert.ToDateTime(dNgayDangKy)));
            fr.SetValue("sNganhNgheKinhDoanh", sNganhNgheKinhDoanh);
            fr.SetValue("sDiaChi", sDiaChi);
            fr.SetValue("sDienThoai", CommonFunction.DinhDangSo(sDienThoai));
            fr.SetValue("sFax", sFax);
            fr.SetValue("sMaChungKhoan", sMaChungKhoan);
            fr.SetValue("rVonDieuLe", CommonFunction.DinhDangSo(rVonDieuLe));
            fr.SetValue("rVonNhaNuoc", CommonFunction.DinhDangSo(rVonNhaNuoc));
            fr.SetValue("sNguoiDaiDienVonNhaNuoc", sNguoiDaiDienVonNhaNuoc);
            fr.SetValue("sSoCoPhan", sSoCoPhan);
            fr.SetValue("sChucVu", sChucVu);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("nam", iNam);
            String DauKy="", CuoiKy = "";          
            if (iQuy.Equals("1"))
            {
                DauKy = "Số đầu năm";
                CuoiKy = "Số cuối kỳ";
            }
            else if (iQuy.Equals("5") || iQuy.Equals("6"))
            {
                DauKy = "Đầu năm";
                CuoiKy = "Cuối kỳ";
            }
            else if (iQuy.Equals("7"))
            {
                DauKy = "Đầu năm";
                CuoiKy = "Năm nay";
            }
            else
            {
                DauKy = "Đầu kỳ";
                CuoiKy = "Cuối kỳ";
            }          
            fr.SetValue("DauKy", DauKy);
            fr.SetValue("CuoiKy", CuoiKy);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">Đường dẫn tới file excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        private void LoadData(FlexCelReport fr, String iQuy, String iNam, String iMaDN)
        {
            //Bảng cân đối kế toán
            DataTable data = HoSoDoanhNghiep(iQuy, iNam, iMaDN);
            data.TableName = "ChiTiet";
            DataTable dtGroup = HamChung.SelectDistinct("Group", data, "TK1", "TK1,TK2,sTen,sDonViTinh,rNamBaoCao,rNamTruoc,TT,iSTT");
            SplitNULL(data, "TK2");
            dtGroup.DefaultView.Sort = "iSTT ASC";
            dtGroup = dtGroup.DefaultView.ToTable();
            if (dtGroup.Rows.Count > 0)
            {
                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    dtGroup.Rows[i]["TT"] = (i + 1).ToString();
                }
            }            
            fr.AddTable("ChiTiet", data);
            fr.AddTable("Group", dtGroup);
            data.Dispose();
            dtGroup.Dispose();
        }
        /// <summary>
        /// Xóa dòng nơi TK2=NULL
        /// </summary>
        /// <param name="dt2"></param>
        /// <param name="DKCap"></param>
        private static void SplitNULL(DataTable dt2, String DKCap)
        {
            if (dt2.Rows.Count > 0)
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (Convert.ToString(dt2.Rows[i][DKCap]) == "")
                    {
                        dt2.Rows.RemoveAt(i);
                        if (i == 0)
                            i = 0;
                        else
                            i = i - 1;
                    }
                }
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="iQuy"></param>
        /// <param name="iNam"></param>
        /// <param name="iAll"></param>
        /// <param name="iMaDN"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iQuy, String iNam, String iMaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iMaDN);
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
        /// <param name="NgayCT"></param>
        /// <param name="MaDN"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iQuy, String iNam, String iMaDN)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iMaDN);
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
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Lấy trong 1 quý hoặc tất cả các quý trong năm</param>
        /// <param name="MaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iQuy, String iNam, String iMaDN)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iNam, iMaDN);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "HoSoDoanhNghiep_" + iNam + ".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy danh sách doanh nghiệp
        /// </summary>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Lấy doanh nghiệp theo năm hay theo quý và năm</param>
        /// <returns></returns>
        public static DataTable GetDoanhNghiep(String Quy, String Nam)
        {
            String DKQuy = "";
            if (Quy.Equals("5") || Quy.Equals("6"))            
                DKQuy = "AND TCDN.iQuy=@iQuy-3";            
            else if (Quy.Equals("7"))
                DKQuy = "";
            else            
                DKQuy = "AND TCDN.iQuy=@iQuy";            
            DataTable dtDN = new DataTable();
            String SQL = String.Format(@"SELECT TC.iID_MaDoanhNghiep,TC.sTenDoanhNghiep
                                        FROM TCDN_DoanhNghiep AS TC
                                        WHERE TC.iTrangThai=1
                                          AND TC.iID_MaDoanhNghiep IN(
	                                        SELECT TCDN.iID_MaDoanhNghiep 
	                                        FROM TCDN_HoSoDoanhNghiepChiTiet AS TCDN
	                                        WHERE TCDN.iTrangThai=1 
	                                        AND TCDN.iNamLamViec=@iNamLamViec
                                            {0}                                        	                                        
                                        )",DKQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            if(!String.IsNullOrEmpty(DKQuy))
                cmd.Parameters.AddWithValue("@iQuy", Quy);            
            dtDN = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDN;
        }
        /// <summary>
        /// Hiện thị danh sách lên View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Tất cả doanh nghiệp trng năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public String obj_DSDoanhNghiep(String ParentID, String Quy, String Nam, String iMaDN)
        {
            String dsDN = "";
            DataTable dtDoanhNghiep = GetDoanhNghiep(Quy, Nam);
            SelectOptionList slDoanhNghiep = new SelectOptionList(dtDoanhNghiep, "iID_MaDoanhNghiep", "sTenDoanhNghiep");
            dsDN = MyHtmlHelper.DropDownList(ParentID, slDoanhNghiep, iMaDN, "iMaDN", "", "class=\"input1_2\" style=\"width: 140px; padding:2px;\"");
            return dsDN;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="All">Lấy doanh nghiệp trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDoanhNghiep(string ParentID, String Quy, String Nam, String iMaDN)
        {
            return Json(obj_DSDoanhNghiep(ParentID, Quy, Nam, iMaDN), JsonRequestBehavior.AllowGet);
        }
    }
}