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
    public class rptQuyetToanQuanSo_47Controller : Controller
    {
        //Quyết toán quân số
        //Edit date: 07-7-2012
        //User edit: Le van Thuong
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQTQS_QuanSo_47.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToanQuanSo_47.aspx";            
            ViewData["pageload"] = "0";
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
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToanQuanSo_47.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu thông tin quân số
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public DataTable QuanSo_47(String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND QS.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKDonvi = iAll.Equals("on")? "" : " AND QS.iID_MaDonVi=@iID_MaDonVi ";
            String SQL = String.Format(@"SELECT QS.sMoTa,QS.sKyHieu
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
                                        AND QS.iThang_Quy=@iThang_Quy
                                       {1}
                                        {0}
                                       {2}
                                        AND SUBSTRING(QS.sKyHieu,1,1) IN(1,2,3,7)
                                        GROUP BY QS.sKyHieu
		                                        ,QS.sMoTa
		                                        ,QS.iID_MaMucLucQuanSo
		                                        ,QS.iID_MaMucLucQuanSo_Cha
		                                        ,QS.bLaHangCha", DKDonvi,DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            
            if (!String.IsNullOrEmpty(DKDonvi))
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iMaDV);   
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            SQL = "";
            SQL = String.Format(@"SELECT QS.sMoTa,QS.sKyHieu
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
                                AND SUBSTRING(QS.sKyHieu,1,1) IN(1,2,3,7)
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
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV)
        {
            String MaND = User.Identity.Name;
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString();
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
               
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);            
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToanQuanSo_47");
            LoadData(fr, iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);            
            switch (iAll)
            {
                case "on":
                    fr.SetValue("TenDV", "");
                    break;
                case "off":
                    fr.SetValue("TenDV", CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iMaDV, "sTen"));
                    break;
            }            
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.Run(Result);
            return Result;
        }       
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Tổng hợp tất cả đơn vị</param>
        /// <param name="iMaDV">Mã đơn vị</param>
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaTrangThaiDuyet, String iAll, String iMaDV,String MaND)
        {
            DataTable dtQuanSo = QuanSo_47(iThang, iID_MaTrangThaiDuyet, iAll, iMaDV,MaND);
            //DataTable dtGroup = HamChung.SelectDistinct("Group", dtQuanSo, "TK1", "TK1,TK2,sMoTa,sKyHieu,rThieuUy,rTrungUy,rThuongUy,rDaiUy,rThieuTa,rTrungTa,rThuongTa,rDaiTa,rTuong,rBinhNhi,rBinhNhat,rHaSi,rTrungSi,rThuongSi,rQNCN,rTSQ,rCNVQPCT,rQNVQPHD");
            //SplitNULL(dtQuanSo);            
            //dtGroup.DefaultView.Sort = "sKyHieu ASC";
            //dtGroup = dtGroup.DefaultView.ToTable();
            fr.AddTable("ChiTiet2", dtQuanSo);
            //fr.AddTable("Group", dtGroup);
            //dtGroup.Dispose();
            dtQuanSo.Dispose();
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
                        //if (i == 0)
                        //    i = 0;
                        //else
                            i = i-1;
                    }
                }
        }          
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
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
                clsResult.FileName = "BC_QuanSo_QT.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
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
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.sTen
                                        FROM NS_DonVi AS DV
                                        WHERE DV.iTrangThai=1
                                          AND DV.iID_MaDonVi IN(SELECT QS.iID_MaDonVi
                                                                FROM QTQS_ChungTuChiTiet QS
                                                                WHERE QS.bLoaiThang_Quy=0
                                                                AND QS.iTrangThai=1 
                                                                AND QS.iThang_Quy=@iThang_Quy
                                                              {0}
                                                             {1}
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
            return Json(obj_DSDonvi(ParentID, iThang, iID_MaTrangThaiDuyet, iMaDV, MaND), JsonRequestBehavior.AllowGet);
        }
        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                //iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                //iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(" AND (QS.iNamLamViec={0} )", iNamLamViec);
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