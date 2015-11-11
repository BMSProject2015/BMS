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
    public class rptQuanSoQuyetToan_52Controller : Controller
    {         
        //Created: Huyền Lê
        //Edited: Thương
        //Dated edit: 28/08/2012
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuanSoQuyetToan_52.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = 0;
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuanSoQuyetToan_52.aspx";
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iAll = Convert.ToString(Request.Form[ParentID + "_iAll"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iAll"] = iAll;
            ViewData["iQuy"] = iQuy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuanSoQuyetToan_52.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");            
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuanSoQuyetToan_52");
            LoadData(fr, iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);                
            fr.SetValue("Nam", iNamLamViec);            
            fr.SetValue("Thang", iQuy);
            switch (iAll)
            {
                case "on":
                    fr.SetValue("TenDV", "Tổng hợp");
                    break;
                case "off":
                    if (iMaDV == Guid.Empty.ToString())
                        fr.SetValue("TenDV", "");
                    else
                        fr.SetValue("TenDV", iMaDV + "-" + CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iMaDV, "sTen"));
                    break;
            }
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
            
        }
        /// <summary>
        /// Xuất báo cáo theo ra file PDF
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            HamChung.Language();
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
               
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "QSQT_Quy_"+iQuy+"_Nam_"+iNamLamViec+".pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }
            }
        }
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            HamChung.Language();
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
               
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QSQT_Quy_"+iQuy+"_Nam_"+iNamLamViec+".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV);
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
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        private void LoadData(FlexCelReport fr, String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable data = QuyetToanQuanSo(iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            //DataTable dtGroup = HamChung.SelectDistinct("Group", data, "TK1", "TK1,TK2,sMoTa,sKyHieu,rThieuUy,rTrungUy,rThuongUy,rDaiUy,rThieuTa,rTrungTa,rThuongTa,rDaiTa,rTuong,rBinhNhi,rBinhNhat,rHaSi,rTrungSi,rThuongSi,rQNCN,rTSQ,rCNVQPCT,rQNVQPHD");
            //SplitNULL(data);
            //dtGroup.DefaultView.Sort = "sKyHieu ASC";
            //dtGroup = dtGroup.DefaultView.ToTable();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            //fr.AddTable("Group", dtGroup);
            data.Dispose();
            //dtGroup.Dispose();
            DataTable dataQT = QuanSo_Thang(iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);           
            fr.AddTable("Quy", dataQT);
            dataQT.Dispose();
            DataTable data1 = QuyetToanQSQuy(iQuy, iID_MaTrangThaiDuyet, iAll, iMaDV, MaND);
            data1.TableName = "QTQuy";
            fr.AddTable("QTQuy", data1);
            data1.Dispose();
        }


        public DataTable QuyetToanQSQuy(String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV, String MaND)
        {
            DataTable dt = new DataTable();
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            String DKQuy = "";
            switch (iQuy)
            {
                case "1":
                    DKQuy = "QS.iThang_Quy BETWEEN 13 AND 15 ";
                    break;
                case "2":
                    DKQuy = "QS.iThang_Quy BETWEEN 1 AND 3 ";
                    break;
                case "3":
                    DKQuy = "QS.iThang_Quy BETWEEN 4 AND 6 ";
                    break;
                case "4":
                    DKQuy = "QS.iThang_Quy BETWEEN 7 AND 9 ";
                    break;
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT sMoTa=CASE WHEN SUBSTRING(QS.SKYHIEU,1,1)=7 THEN N'QSQT quý trước' ELSE QS.sMoTa END  ,QS.sKyHieu    
                                                ,SUM(case when {0} {2} then QS.rThieuUy else 0 end) AS rThieuUy
                                                ,SUM(case when {0} {2} then QS.rTrungUy else 0 end) AS rTrungUy
                                                ,SUM(case when {0} {2} then QS.rThuongUy else 0 end) AS rThuongUy
                                                ,SUM(case when {0} {2} then QS.rDaiUy else 0 end) AS rDaiUy
                                                ,SUM(case when {0} {2} then QS.rThieuTa else 0 end) AS rThieuTa
                                                ,SUM(case when {0} {2} then QS.rTrungTa else 0 end) AS rTrungTa
                                                ,SUM(case when {0} {2} then QS.rThuongTa else 0 end) AS rThuongTa
                                                ,SUM(case when {0} {2} then QS.rDaiTa else 0 end) AS rDaiTa
                                                ,SUM(case when {0} {2} then QS.rTuong else 0 end) AS rTuong
                                                ,SUM(case when {0} {2} then QS.rTSQ else 0 end) AS rTSQ
                                                ,SUM(case when {0} {2} then QS.rBinhNhi else 0 end) AS rBinhNhi
                                                ,SUM(case when {0} {2} then QS.rBinhNhat else 0 end) AS rBinhNhat
                                                ,SUM(case when {0} {2} then QS.rHaSi else 0 end) AS rHaSi
                                                ,SUM(case when {0} {2} then QS.rTrungSi else 0 end) AS rTrungSi
                                                ,SUM(case when {0} {2} then QS.rThuongSi else 0 end) AS rThuongSi
                                                ,SUM(case when {0} {2} then QS.rQNCN else 0 end) AS rQNCN                                                
                                                ,SUM(case when {0} {2} then QS.rCNVQPCT else 0 end) AS rCNVQPCT
                                                ,SUM(case when {0} {2} then QS.rQNVQPHD else 0 end) AS rQNVQPHD      
                                        FROM QTQS_ChungTuChiTiet AS QS
                                        WHERE QS.iTrangThai=1
                                        AND QS.bLoaiThang_Quy=0                                    
                                        {1}
                                       {3}
                                        AND SUBSTRING(QS.sKyHieu,1,1) IN(7) AND sKyHieu<>290 AND sKyHieu<>390
                                        GROUP BY QS.sKyHieu
		                                        ,QS.sMoTa", DKQuy, DKDonvi, DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();            
           // dt.Dispose();
            return dt;
        }
        /// <summary>
        /// //Loại những row có tài khoản cấp 2=NULL
        /// </summary>
        /// <param name="dt2">Datatable cần loại bỏ row</param>
        private static void SplitNULL(DataTable dt2)
        {
            if (dt2.Rows.Count > 0)
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (Convert.ToString(dt2.Rows[i]["TK2"]) == "")
                    {
                        dt2.Rows.RemoveAt(i);                        
                        i = i - 1;
                    }
                }
        }          
        /// <summary>
        /// Lấy thông tin quân số trong quý
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable QuanSo_Thang(String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dt = new DataTable();
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            String DKQuy = "";
            switch (iQuy)
            {
                case "1":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 1 AND 3 ";
                    break;
                case "2":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 4 AND 6 ";
                    break;
                case "3":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 7 AND 9 ";
                    break;
                case "4":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 10 AND 12 ";
                    break;
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT N'Q.Số QT tháng '+convert(varchar,QS.iThang_Quy) sMoTa
	                                          ,SUM(QS.rThieuUy) rThieuUy
	                                          ,SUM(QS.rTrungUy) rTrungUy
	                                          ,SUM(QS.rThuongUy) rThuongUy
	                                          ,SUM(QS.rDaiUy) rDaiUy
	                                          ,SUM(QS.rThieuTa) rThieuTa
	                                          ,SUM(QS.rTrungTa) rTrungTa
	                                          ,SUM(QS.rThuongTa) rThuongTa
	                                          ,SUM(QS.rDaiTa) rDaiTa
	                                          ,SUM(QS.rTuong) rTuong
	                                          ,SUM(QS.rTSQ) rTSQ
	                                          ,SUM(QS.rBinhNhi) rBinhNhi
	                                          ,SUM(QS.rBinhNhat) rBinhNhat
	                                          ,SUM(QS.rHaSi) rHaSi
	                                          ,SUM(QS.rTrungSi) rTrungSi
	                                          ,SUM(QS.rThuongSi) rThuongSi
	                                          ,SUM(QS.rQNCN) rQNCN
	                                          ,SUM(QS.rCNVQPCT) rCNVQPCT
	                                          ,SUM(QS.rQNVQPHD) rQNVQPHD
                                        FROM QTQS_ChungTuChiTiet QS
                                        WHERE QS.iTrangThai=1
                                          AND QS.bLoaiThang_Quy=0
                                          {0}
                                         {2}
                                          {1}
                                         {3}
                                          AND SUBSTRING(QS.sKyHieu,1,1)=7
                                        GROUP BY QS.iThang_Quy", DKDonvi,DKQuy,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
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
        /// Lấy thông tin quân số trong quý
        /// </summary>
        /// <param name="iQuy">Quý</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iAll">Năm</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable QuyetToanQuanSo(String iQuy, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dt = new DataTable();
            String DKDonvi = iAll.Equals("on") ? "" : "AND QS.iID_MaDonVi=@iID_MaDonVi";
            String DKQuy = "";
            switch (iQuy)
            { 
                case "1":
                    DKQuy = "QS.iThang_Quy BETWEEN 1 AND 3 ";
                    break;
                case "2":
                    DKQuy = "QS.iThang_Quy BETWEEN 4 AND 6 ";
                    break;
                case "3":
                    DKQuy = "QS.iThang_Quy BETWEEN 7 AND 9 ";
                    break;
                case "4":
                    DKQuy = "QS.iThang_Quy BETWEEN 10 AND 12 ";
                    break;
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT sMoTa= CASE WHEN QS.SKYHIEU=2 THEN N'Tăng trong quý' ELSE 
                                                       CASE WHEN QS.SKYHIEU=3 THEN N'Giảm trong quý'
                                                       ELSE CASE WHEN SUBSTRING(QS.SKYHIEU,1,1)=7 THEN N'QSQT quý này' ELSE QS.sMoTa END
                                                       END END ,QS.sKyHieu
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
                                                ,SUM(case when {0} {2} then QS.rThieuUy else 0 end) AS rThieuUy
                                                ,SUM(case when {0} {2} then QS.rTrungUy else 0 end) AS rTrungUy
                                                ,SUM(case when {0} {2} then QS.rThuongUy else 0 end) AS rThuongUy
                                                ,SUM(case when {0} {2} then QS.rDaiUy else 0 end) AS rDaiUy
                                                ,SUM(case when {0} {2} then QS.rThieuTa else 0 end) AS rThieuTa
                                                ,SUM(case when {0} {2} then QS.rTrungTa else 0 end) AS rTrungTa
                                                ,SUM(case when {0} {2} then QS.rThuongTa else 0 end) AS rThuongTa
                                                ,SUM(case when {0} {2} then QS.rDaiTa else 0 end) AS rDaiTa
                                                ,SUM(case when {0} {2} then QS.rTuong else 0 end) AS rTuong
                                                ,SUM(case when {0} {2} then QS.rTSQ else 0 end) AS rTSQ
                                                ,SUM(case when {0} {2} then QS.rBinhNhi else 0 end) AS rBinhNhi
                                                ,SUM(case when {0} {2} then QS.rBinhNhat else 0 end) AS rBinhNhat
                                                ,SUM(case when {0} {2} then QS.rHaSi else 0 end) AS rHaSi
                                                ,SUM(case when {0} {2} then QS.rTrungSi else 0 end) AS rTrungSi
                                                ,SUM(case when {0} {2} then QS.rThuongSi else 0 end) AS rThuongSi
                                                ,SUM(case when {0} {2} then QS.rQNCN else 0 end) AS rQNCN                                                
                                                ,SUM(case when {0} {2} then QS.rCNVQPCT else 0 end) AS rCNVQPCT
                                                ,SUM(case when {0} {2} then QS.rQNVQPHD else 0 end) AS rQNVQPHD      
                                        FROM QTQS_ChungTuChiTiet AS QS
                                        WHERE QS.iTrangThai=1
                                        AND QS.bLoaiThang_Quy=0                                    
                                        {1}
                                       {3}
                                        AND SUBSTRING(QS.sKyHieu,1,1) IN(2,3,7) AND sKyHieu<>290 AND sKyHieu<>390
                                        GROUP BY QS.sKyHieu
		                                        ,QS.sMoTa
		                                        ,QS.iID_MaMucLucQuanSo
		                                        ,QS.iID_MaMucLucQuanSo_Cha
		                                        ,QS.bLaHangCha", DKQuy, DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            SQL = "";
            SQL = String.Format(@"SELECT sMoTa=CASE WHEN QS.SKYHIEU=2 THEN N'Tăng trong quý' ELSE 
                                               CASE WHEN QS.SKYHIEU=3 THEN N'Giảm trong quý'
                                               ELSE CASE WHEN SUBSTRING(QS.SKYHIEU,1,1)=7 THEN N'QSQT quý này' ELSE QS.sMoTa END
                                               END END ,QS.sKyHieu
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
                                AND SUBSTRING(QS.sKyHieu,1,1) IN(2,3,7) AND sKyHieu<>290 AND sKyHieu<>390
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
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    R2 = dt.Rows[j];
                    if (R1["sKyHieu"].Equals(R2["sKyHieu"]))
                    {
                        for (int c = 1; c < dt.Columns.Count; c++)
                        {
                            if (dt.Columns[c].ColumnName == dtResult.Columns[c].ColumnName)
                            {
                                dtResult.Rows[i][c] = dt.Rows[j][c];
                            }
                        }
                        break;
                    }
                }
            }
            dt.Dispose();
            return dtResult;
        }
        /// <summary>
        /// Lấy danh sách đơn vị theo năm
        /// </summary>
        /// <param name="iQuy">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <returns></returns>
        public static DataTable GetDonvi(String iQuy, String iID_MaTrangThaiDuyet,String MaND)
        {
            String DKQuy = "";
            switch (iQuy)
            { 
                case "1":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 1 AND 3";
                    break;
                case "2":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 4 AND 6";
                    break;
                case "3":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 7 AND 9";
                    break;
                case "4":
                    DKQuy = "AND QS.iThang_Quy BETWEEN 10 AND 12";
                    break;
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.iID_MaDonVi+'-'+DV.sTen AS sTen
                                        FROM NS_DonVi AS DV
                                        WHERE DV.iTrangThai=1
                                          AND DV.iID_MaDonVi IN(SELECT QS.iID_MaDonVi
                                                                FROM QTQS_ChungTuChiTiet QS
                                                                WHERE QS.bLoaiThang_Quy=0
                                                                AND QS.iTrangThai=1 
                                                                {0}
                                                                {1}
                                                                {2}
                                                                GROUP BY QS.iID_MaDonVi ) order by DV.iID_MaDonVi", DKQuy, DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);            
          
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iQuy">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public String obj_DSDonvi(String ParentID, String iQuy, String iID_MaTrangThaiDuyet, String iMaDV,String MaND)
        {
            String dsDV = "";
            DataTable dtDonvi = GetDonvi(iQuy, iID_MaTrangThaiDuyet,MaND);
            SelectOptionList slDonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            dsDV = MyHtmlHelper.DropDownList(ParentID, slDonvi, iMaDV, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 140px; padding:2px;\"");
            return dsDV;
        }
        /// <summary>
        /// Hàm Ajax load danh sách đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iQuy">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonvi(string ParentID, String iQuy, String iID_MaTrangThaiDuyet, String iMaDV)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonvi(ParentID, iQuy, iID_MaTrangThaiDuyet, iMaDV,MaND), JsonRequestBehavior.AllowGet);
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