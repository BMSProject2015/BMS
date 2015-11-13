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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuanSo_53Controller : Controller
    {
        //Báo cáo quân số
        //Edit date: 06-07-2012
        //User edit: Lê văn Thương
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuanSo_53.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuanSo_53.aspx";
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
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];            
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iAll = Convert.ToString(Request.Form[ParentID + "_iAll"]);
            ViewData["pageload"] = "1";
            ViewData["iThang"] = iThang_Quy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iAll"] = iAll;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuanSo_53.aspx"; 
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");  
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iThang_Quy">Tháng làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iNamLamViec">Năm làm việc</param>
        /// <param name="TongHop">Tổng hợp tất cả đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
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
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuanSo_53");            
            LoadData(fr, iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2));            
            if (iThang.Equals("12"))
                fr.SetValue("ThangNam", "Năm " + iNamLamViec);
            else
                fr.SetValue("ThangNam", "Đến tháng " + iThang + " năm " + iNamLamViec);
            switch (iAll)
            { 
                case "on":
                    fr.SetValue("TenDV", "Tổng hợp");
                    break;
                case "off":
                    fr.SetValue("TenDV", CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iMaDV, "sTen"));
                    break;
            }
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
        }        
        /// <summary>
        /// Hiển thị dữ liệu báo cáo Quân số
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang_Quy">Tháng làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iNamLamViec">Năm làm việc</param>
        /// <param name="TongHop">Tổng hợp tất cả đơn vị</param>
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            //Thông tin quân số tháng 0
            DataTable dtThang0 = dtQuanSo_Thang0(iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            dtThang0.TableName = "ChiTiet1";
            fr.AddTable("ChiTiet1", dtThang0);
            dtThang0.Dispose();
            //Thông quân số tháng quyết toán
            DataTable dtThangQT = dtQuanSo_ThangQT(iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            fr.AddTable("ChiTiet2", dtThangQT);
            dtThangQT.Dispose();
            //Thông tin quân số tăng tháng
            DataTable dtLuyKe = dtQuanSo_LuyKe(iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            DataTable dtGroup = HamChung.SelectDistinct("Group", dtLuyKe, "TK1", "TK1,TK2,sMoTa,rThieuUy,rTrungUy,rThuongUy,rDaiUy,rThieuTa,rTrungTa,rThuongTa,rDaiTa,rTuong,rBinhNhi,rBinhNhat,rHaSi,rTrungSi,rThuongSi,rQNCN,rTSQ,rCNVQPCT,rQNVQPHD");
            SplitNULL(dtLuyKe);
            //Sắp xếp giảm dần theo trường mô tả
            dtGroup.DefaultView.Sort = "sMoTa DESC";
            dtGroup = dtGroup.DefaultView.ToTable();
            fr.AddTable("Group",dtGroup);
            fr.AddTable("ChiTiet3", dtLuyKe);
            dtLuyKe.Dispose();
            dtGroup.Dispose();
            DataTable dtBinhQuan = dtQuanSo_BinhQuan(iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            fr.AddTable("ChiTiet4", dtBinhQuan);
            dtBinhQuan.Dispose();
        }        
        /// <summary>
        /// //Loại những row có tài khoản cấp 2=NULL
        /// </summary>
        /// <param name="dt2"></param>
        private static void SplitNULL(DataTable dt2)
        {
            if (dt2.Rows.Count > 0)
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (Convert.ToString(dt2.Rows[i]["TK2"]) == "")
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
        /// <param name="iThang">Tháng làm việc</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
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
        /// <param name="iThang">Tháng làm việc</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
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
                clsResult.FileName = "BC_TinhHinh_ThucHien_QS.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="iThang">Tháng làm việc</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
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
        /// Lấy thông tin quân số tháng 0
        /// </summary>
        /// <param name="iNam">Năm làm việc</param>
        /// <param name="iAll">Tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable dtQuanSo_Thang0(String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi ";
            String SQL = String.Format(@"SELECT sMoTa=N'Quân số 1/1'
	                                          ,SUM(QS.rThieuUy) AS rThieuUy
                                              ,SUM(QS.rTrungUy) AS rTrungUy
                                              ,SUM(QS.rThuongUy) AS rThuongUy
                                              ,SUM(QS.rDaiUy) AS rDaiUy
                                              ,SUM(QS.rThieuTa) AS rThieuTa
                                              ,SUM(QS.rTrungTa) AS rTrungTa
                                              ,SUM(QS.rThuongTa) AS rThuongTa
                                              ,SUM(QS.rDaiTa) AS rDaiTa
                                              ,SUM(QS.rTuong) AS rTuong
                                              ,SUM(QS.rBinhNhi) AS rBinhNhi
                                              ,SUM(QS.rBinhNhat) AS rBinhNhat
                                              ,SUM(QS.rHaSi) AS rHaSi
                                              ,SUM(QS.rTrungSi) AS rTrungSi
                                              ,SUM(QS.rThuongSi) AS rThuongSi
                                              ,SUM(QS.rQNCN) AS rQNCN
                                              ,SUM(QS.rTSQ) AS rTSQ
                                              ,SUM(QS.rCNVQPCT) AS rCNVQPCT
                                              ,SUM(QS.rQNVQPHD) AS rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet QS
                                        WHERE QS.iTrangThai=1
                                        AND QS.iThang_Quy=0
                                        AND QS.bLoaiThang_Quy=0
                                        {1}
                                        AND SUBSTRING(QS.sKyHieu,1,1)='1'
                                        {2}
                                        {0}", DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin quân số trong tháng của đơn vị
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable dtQuanSo_ThangQT(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dt = new DataTable();
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT QS.sMoTa
	                                          ,SUM(QS.rThieuUy) AS rThieuUy
                                              ,SUM(QS.rTrungUy) AS rTrungUy
                                              ,SUM(QS.rThuongUy) AS rThuongUy
                                              ,SUM(QS.rDaiUy) AS rDaiUy
                                              ,SUM(QS.rThieuTa) AS rThieuTa
                                              ,SUM(QS.rTrungTa) AS rTrungTa
                                              ,SUM(QS.rThuongTa) AS rThuongTa
                                              ,SUM(QS.rDaiTa) AS rDaiTa
                                              ,SUM(QS.rTuong) AS rTuong
                                              ,SUM(QS.rBinhNhi) AS rBinhNhi
                                              ,SUM(QS.rBinhNhat) AS rBinhNhat
                                              ,SUM(QS.rHaSi) AS rHaSi
                                              ,SUM(QS.rTrungSi) AS rTrungSi
                                              ,SUM(QS.rThuongSi) AS rThuongSi
                                              ,SUM(QS.rQNCN) AS rQNCN
                                              ,SUM(QS.rTSQ) AS rTSQ
                                              ,SUM(QS.rCNVQPCT) AS rCNVQPCT
                                              ,SUM(QS.rQNVQPHD) AS rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet AS QS
                                        WHERE QS.iTrangThai=1
                                          AND QS.iThang_Quy=@iThang_Quy
                                          AND QS.bLoaiThang_Quy=0
                                          {1}
                                          {2}
                                          {0}
                                          AND SUBSTRING(QS.sKyHieu,1,1)='4'
                                        GROUP BY QS.sMoTa
                                        ", DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            if (!String.IsNullOrEmpty(DKDonvi)) {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin quân số trong tháng
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable dtQuanSo_LuyKe(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dtLuyKe = new DataTable();
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT sMoTa=CASE WHEN QS.sKyHieu=2 THEN N'Quân số tăng trong năm' ELSE CASE WHEN QS.sKyHieu=3 THEN N'Quân số giảm trong năm' ELSE QS.sMoTa END END       
                                                ,QS.sKyHieu
                                                ,TK1=CASE 
		                                                WHEN QS.bLaHangCha='TRUE' 
		                                                    THEN CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo)
		                                                ELSE 
		                                                    CASE 
		                                                        WHEN QS.iID_MaMucLucQuanSo_Cha IS NULL
		                                                        THEN CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo)
		                                                        ELSE CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo_Cha)
		                                                    END
		                                            END
                                                ,TK2=CASE 
                                                        WHEN QS.bLaHangCha='FALSE' AND QS.iID_MaMucLucQuanSo_Cha IS NULL 
                                                            THEN '' 
                                                        ELSE
                                                            CASE WHEN QS.bLaHangCha='TRUE' 
                                                            THEN ''
				                                            ELSE	CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo)
				                                            END
                                                    END     
                                                ,SUM(QS.rThieuUy) AS rThieuUy
                                                ,SUM(QS.rTrungUy) AS rTrungUy
                                                ,SUM(QS.rThuongUy) AS rThuongUy
                                                ,SUM(QS.rDaiUy) AS rDaiUy
                                                ,SUM(QS.rThieuTa) AS rThieuTa
                                                ,SUM(QS.rTrungTa) AS rTrungTa
                                                ,SUM(QS.rThuongTa) AS rThuongTa
                                                ,SUM(QS.rDaiTa) AS rDaiTa
                                                ,SUM(QS.rTuong) AS rTuong
                                                ,SUM(QS.rTSQ) AS rTSQ
                                                ,SUM(QS.rBinhNhi) AS rBinhNhi
                                                ,SUM(QS.rBinhNhat) AS rBinhNhat
                                                ,SUM(QS.rHaSi) AS rHaSi
                                                ,SUM(QS.rTrungSi) AS rTrungSi
                                                ,SUM(QS.rThuongSi) AS rThuongSi
                                                ,SUM(QS.rQNCN) AS rQNCN
                                                ,SUM(QS.rCNVQPCT) AS rCNVQPCT
                                                ,SUM(QS.rQNVQPHD) AS rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet AS QS
                                        WHERE QS.iTrangThai=1
                                          AND QS.bLoaiThang_Quy=0
                                          AND QS.iThang_Quy BETWEEN 1 AND @iThang_Quy
                                        {1}
                                          {0}
                                          {2}
                                          AND SUBSTRING(QS.sKyHieu,1,1) IN(2,3)
                                        GROUP BY QS.sKyHieu
		                                        ,QS.sMoTa
		                                        ,QS.iID_MaMucLucQuanSo
		                                        ,QS.iID_MaMucLucQuanSo_Cha
		                                        ,QS.bLaHangCha
                                       ", DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd=new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            dtLuyKe = Connection.GetDataTable(cmd);
            cmd.Dispose();
            SQL = "";
            SQL = String.Format(@"SELECT sMoTa=CASE WHEN QS.sKyHieu=2 THEN N'Quân số tăng trong năm' ELSE CASE WHEN QS.sKyHieu=3 THEN N'Quân số giảm trong năm' ELSE QS.sMoTa END END
                                    ,QS.sKyHieu
                                    ,TK1=CASE 
                                            WHEN QS.bLaHangCha='TRUE' 
                                                THEN CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo)
                                            ELSE 
                                                CASE 
                                                    WHEN QS.iID_MaMucLucQuanSo_Cha IS NULL
                                                    THEN CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo)
                                                    ELSE CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo_Cha)
                                                END
                                        END
                                    ,TK2=CASE 
                                            WHEN QS.bLaHangCha='FALSE' AND QS.iID_MaMucLucQuanSo_Cha IS NULL 
                                                THEN '' 
                                            ELSE
                                                CASE WHEN QS.bLaHangCha='TRUE' 
                                                THEN ''
                                                ELSE	CONVERT(NVARCHAR(50),QS.iID_MaMucLucQuanSo)
                                                END
                                        END  
                                    --Sỹ quan       
	                                ,rThieuUy=0,rTrungUy=0,rThuongUy=0,rDaiUy=0
                                    ,rThieuTa=0,rTrungTa=0,rThuongTa=0,rDaiTa=0
                                    ,rTuong=0
                                    --Hạ sỹ quan-chiến sỹ
                                    ,rTSQ=0,rBinhNhi=0,rBinhNhat=0,rHaSi=0,rTrungSi=0,rThuongSi=0
                                    --Quân nhân chuyên nghiệp
                                    ,rQNCN=0,rCNVQPCT=0,rQNVQPHD=0 
                                FROM NS_MucLucQuanSo QS
                                WHERE QS.iTrangThai=1
                                AND SUBSTRING(QS.sKyHieu,1,1) IN(2,3)
                                GROUP BY QS.sKyHieu
	                                        ,QS.sMoTa
	                                        ,QS.iID_MaMucLucQuanSo
	                                        ,QS.iID_MaMucLucQuanSo_Cha
	                                        ,QS.bLaHangCha
                                ORDER BY QS.sKyHieu");
            DataTable dtResult = Connection.GetDataTable(SQL);
            DataRow R1, R2;
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                R1 = dtResult.Rows[i];
                for (int j = 0; j < dtLuyKe.Rows.Count; j++)
                {
                    R2 = dtLuyKe.Rows[j];
                    if (R1["sKyHieu"].Equals(R2["sKyHieu"]))
                    {
                        for (int c = 1; c < dtLuyKe.Columns.Count; c++)
                        {
                            if (dtLuyKe.Columns[c].ColumnName == dtResult.Columns[c].ColumnName)
                            {
                                dtResult.Rows[i][c] = dtLuyKe.Rows[j][c];
                            }
                        }
                        break;
                    }
                }
            }
            dtLuyKe.Dispose();
            return dtResult;
        }
        /// <summary>
        /// Lấy thông tin quân số bình quân năm
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable dtQuanSo_BinhQuan(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dt = new DataTable();
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT sMoTa=N'Quân số bình quân năm'	  
                                              ,CONVERT(INT,ROUND((SUM(QS.rThieuUy)/CONVERT(INT,@iThang_Quy)),0)) rThieuUy
                                              ,CONVERT(INT,ROUND((SUM(QS.rTrungUy)/CONVERT(INT,@iThang_Quy)),0)) rTrungUy
                                              ,CONVERT(INT,ROUND((SUM(QS.rThuongUy)/CONVERT(INT,@iThang_Quy)),0)) rThuongUy
                                              ,CONVERT(INT,ROUND((SUM(QS.rDaiUy)/CONVERT(INT,@iThang_Quy)),0)) rDaiUy
                                              ,CONVERT(INT,ROUND((SUM(QS.rThieuTa)/CONVERT(INT,@iThang_Quy)),0)) rThieuTa
                                              ,CONVERT(INT,ROUND((SUM(QS.rTrungTa)/CONVERT(INT,@iThang_Quy)),0)) rTrungTa
                                              ,CONVERT(INT,ROUND((SUM(QS.rThuongTa)/CONVERT(INT,@iThang_Quy)),0)) rThuongTa
                                              ,CONVERT(INT,ROUND((SUM(QS.rDaiTa)/CONVERT(INT,@iThang_Quy)),0)) rDaiTa
                                              ,CONVERT(INT,ROUND((SUM(QS.rTuong)/CONVERT(INT,@iThang_Quy)),0)) rTuong
                                              ,CONVERT(INT,ROUND((SUM(QS.rTSQ)/CONVERT(INT,@iThang_Quy)),0)) rTSQ
                                              ,CONVERT(INT,ROUND((SUM(QS.rBinhNhi)/CONVERT(INT,@iThang_Quy)),0)) rBinhNhi
                                              ,CONVERT(INT,ROUND((SUM(QS.rBinhNhat)/CONVERT(INT,@iThang_Quy)),0)) rBinhNhat
                                              ,CONVERT(INT,ROUND((SUM(QS.rHaSi)/CONVERT(INT,@iThang_Quy)),0)) rHaSi
                                              ,CONVERT(INT,ROUND((SUM(QS.rTrungSi)/CONVERT(INT,@iThang_Quy)),0)) rTrungSi
                                              ,CONVERT(INT,ROUND((SUM(QS.rThuongSi)/CONVERT(INT,@iThang_Quy)),0)) rThuongSi
                                              ,CONVERT(INT,ROUND((SUM(QS.rQNCN)/CONVERT(INT,@iThang_Quy)),0)) rQNCN
                                              ,CONVERT(INT,ROUND((SUM(QS.rCNVQPCT)/CONVERT(INT,@iThang_Quy)),0)) rCNVQPCT
                                              ,CONVERT(INT,ROUND((SUM(QS.rQNVQPHD)/CONVERT(INT,@iThang_Quy)),0)) rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet QS
                                        WHERE QS.iTrangThai=1
                                       {1}
                                        {0}
                                        AND SUBSTRING(QS.sKyHieu,1,1)='7'
                                        AND QS.bLoaiThang_Quy=0
                                        AND QS.iThang_Quy BETWEEN 1 AND @iThang_Quy
                                       {2}", DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị theo năm
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public  static DataTable GetDonvi(String iThang, String iID_MaTrangThaiDuyet,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.sTen
                                        FROM NS_DonVi AS DV
                                        WHERE DV.iTrangThai=1
                                          AND DV.iID_MaDonVi IN(SELECT QS.iID_MaDonVi
                                                                FROM QTQS_ChungTuChiTiet QS
                                                                WHERE QS.bLoaiThang_Quy=0
                                                                AND QS.iTrangThai=1 
                                                                AND QS.iThang_Quy BETWEEN 1 AND @iThang_Quy
                                                               {0} {1}
                                                                GROUP BY QS.iID_MaDonVi)",DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
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
        public String obj_DSDonvi(String ParentID, String iThang, String iID_MaTrangThaiDuyet,String iMaDV,String MaND)
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