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
    public class rptKT_TongHop_RutDuToanController : Controller
    {
        // Create: Thương
        // GET: /rptKT_TongHop_RutDuToan/
        public string sViewPath = "~/Report_Views/";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = 0;
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKT_TongHop_RutDuToan.aspx";
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
            String iTuNgay = Convert.ToString(Request.Form[ParentID + "_iTuNgay"]);
            String iDenNgay = Convert.ToString(Request.Form[ParentID + "_iDenNgay"]);
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNam"]);
            String iTuThang = Convert.ToString(Request.Form[ParentID + "_iTuThang"]);
            String iDenThang = Convert.ToString(Request.Form[ParentID + "_iDenThang"]);
            String iDonViTinh = Convert.ToString(Request.Form[ParentID + "_iDVT"]);
            String iPages = Convert.ToString(Request.Form[ParentID + "_divPages"]);
            String iSoTo = Convert.ToString(Request.Form[ParentID + "_iSoTo"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["iTuNgay"] = iTuNgay;
            ViewData["iDenNgay"] = iDenNgay;
            ViewData["iTuThang"] = iTuThang;
            ViewData["iDenThang"] = iDenThang;
            ViewData["iNam"] = iNam;
            ViewData["iDonViTinh"] = iDonViTinh;
            ViewData["iPage"] = iPages;
            ViewData["iSoTo"] = iSoTo;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            String sFilePath = getPath(iPages,iSoTo);
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKT_TongHop_RutDuToan.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy đường dẫn tới file Excel
        /// </summary>
        /// <param name="iPage"></param>
        /// <returns></returns>
        public String getPath(String iPage,String iSoTo)
        {
            String iPath = "";
            switch (iPage)
            {
                case "A4Dung":
                    iPath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_TongHop_RutDuToan_A4Dung.xls";
                    break;
                case "A4Ngang":
                    if (iSoTo.Equals("1"))
                        iPath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_TongHop_RutDuToan_A4Ngang.xls";
                    else
                        iPath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_TongHop_RutDuToan_A4Ngang_Khac.xls";
                    break;
            }
            return iPath;
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iDVTinh">Đơn vị tính</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <param name="iReport">Loại báo cáo</param>
        /// <param name="iSoTo">Số tờ</param>
        /// <param name="UserID">Người dùng</param>
        /// <returns></returns>
        public DataTable TH_RutDuToan(String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iDVTinh, String iSoTo, String iReport, String UserID, String iID_MaTrangThaiDuyet)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);           
            String iNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            DataTable dt = new DataTable();
            String dvt = "";
            dvt = DonViTinh(iDVTinh, dvt);
            String DKSelect = "";
            DataTable dtLNS = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            dtLNS = sLNS(GetLNS(iTuNgay, iDenNgay, iTuThang, iDenThang, iNam,UserID,iID_MaTrangThaiDuyet),iSoTo,iReport);
            String DKTong = "";
            DataTable dtLNS_Sum = GetLNS(iTuNgay, iDenNgay, iTuThang, iDenThang, iNam,UserID,iID_MaTrangThaiDuyet);
            if (dtLNS_Sum.Rows.Count > 0)
            {
                DKTong += ",Tong=SUM(CASE WHEN NS.sLNS IN(";
                for (int j = 0; j < dtLNS_Sum.Rows.Count; j++)
                {
                    DKTong += dtLNS_Sum.Rows[j]["sLNS"].ToString();
                    if (j < dtLNS_Sum.Rows.Count - 1)
                        DKTong += ",";
                }
                DKTong += ") THEN "+dvt +" ELSE 0 END)";
            }
            if (dtLNS.Rows.Count > 0)            
                for (int i = 0; i < dtLNS.Rows.Count; i++)                
                    DKSelect += ",LNS" + (i + 1).ToString() + "=SUM(CASE WHEN NS.sLNS=" + dtLNS.Rows[i]["sLNS"] + " THEN " + dvt + " ELSE 0 END)";                       
            String SQL = String.Format(@"SELECT KT.sTenDonVi_Nhan as sDonVi
                                                {0}{1}                                       
                                        FROM KTKB_ChungTuChiTiet KT
                                        INNER JOIN NS_MucLucNganSach NS
                                        ON SUBSTRING(KT.sLNS,1,3)=NS.sLNS
                                        WHERE KT.iTrangThai=1 
                                        AND KT.rDTRut>0
                                        AND KT.iThangCT<>0
                                        AND KT.iID_MaNamNganSach=@iID_MaNamNganSach
                                        AND CONVERT(DATETIME,CONVERT(VARCHAR,KT.iNgayCT)+'/'+CONVERT(VARCHAR,KT.iThangCT)+'/'+CONVERT(VARCHAR,KT.iNamLamViec),103) BETWEEN convert(datetime,convert(varchar,@iTuNgay)+'/'+convert(varchar,@iTuThang)+'/'+convert(varchar,@iNamLamViec),103) AND convert(datetime,convert(varchar,@iDenNgay)+'/'+convert(varchar,@iDenThang)+'/'+convert(varchar,@iNamLamViec),103)
                                        {2}
                                        GROUP BY KT.iID_MaDonVi_Nhan,KT.sTenDonVi_Nhan", DKSelect, DKTong, iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iTuNgay", iTuNgay);
            cmd.Parameters.AddWithValue("@iDenNgay", iDenNgay);
            cmd.Parameters.AddWithValue("@iTuThang", iTuThang);
            cmd.Parameters.AddWithValue("@iDenThang", iDenThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iNamNganSach);            
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        /// <param name="iDVTinh">Loại đơn vị tính: Đồng | Ngàn đồng | Triệu đồng </param>
        /// <param name="dvt">Chuỗi</param>
        /// <returns></returns>
        private static String DonViTinh(String iDVTinh, String dvt)
        {
            switch (iDVTinh)
            {
                case "rD"://Đồng
                    dvt = "(KT.rDTRut)";
                    break;
                case "rND"://Ngàn đồng
                    dvt = "(KT.rDTRut)/1000";
                    break;
                case "rTrD"://Triệu đồng
                    dvt = "(KT.rDTRut)/1000000";
                    break;
            }
            return dvt;
        }
        /// <summary>
        /// Lấy loại ngân sách
        /// </summary>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="dtLNS">DataTable</param>
        /// <param name="tt">Trạng thái duyệt</param>
        /// <returns></returns>
        private static DataTable GetLNS(String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String UserID, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }
            DataTable dtAccess = NguoiDungCauHinhModels.LayCauHinh(UserID);
            String iNamNganSach = dtAccess.Rows[0]["iID_MaNamNganSach"].ToString();
            DataTable dtLNS = new DataTable();
            String LNS = String.Format(@"SELECT NS.sLNS,NS.sMoTa
                                        FROM NS_MucLucNganSach NS
                                        WHERE NS.iTrangThai=1
                                          AND NS.sLNS IN(SELECT SUBSTRING(KT.sLNS,1,3)
                                                         FROM KTKB_ChungTuChiTiet KT
                                                         WHERE KT.iTrangThai=1 AND KT.iID_MaNamNganSach=@iID_MaNamNganSach
                                                        AND KT.rDTRut>0 {0} AND KT.iThang<>0
                                                        AND CONVERT(DATETIME,CONVERT(VARCHAR,KT.iNgayCT)+'/'+CONVERT(VARCHAR,KT.iThangCT)+'/'+CONVERT(VARCHAR,KT.iNamLamViec),103) 
                                                        BETWEEN convert(datetime,convert(varchar,@iTuNgay)+'/'+convert(varchar,@iTuThang)+'/'+convert(varchar,@iNamLamViec),103)
                                                        AND convert(datetime,convert(varchar,@iDenNgay)+'/'+convert(varchar,@iDenThang)+'/'+convert(varchar,@iNamLamViec),103))"
                                                        ,iID_MaTrangThaiDuyet);
            SqlCommand cmdLNS = new SqlCommand(LNS);
            cmdLNS.Parameters.AddWithValue("@iTuNgay", iTuNgay);
            cmdLNS.Parameters.AddWithValue("@iTuThang", iTuThang);
            cmdLNS.Parameters.AddWithValue("@iDenNgay", iDenNgay);
            cmdLNS.Parameters.AddWithValue("@iDenThang", iDenThang);
            cmdLNS.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmdLNS.Parameters.AddWithValue("@iID_MaNamNganSach", iNamNganSach);
            dtLNS = Connection.GetDataTable(cmdLNS);
            cmdLNS.Dispose();
            if (dtLNS.Rows.Count >= 0)
            {
                int count = 0;// Số lượng loại ngân sách cần thêm    
                if (dtLNS.Rows.Count == 0)
                    count = 9;
                else if (dtLNS.Rows.Count > 0 && dtLNS.Rows.Count % 9 != 0)
                    count = 9 - dtLNS.Rows.Count % 9;
                for (int i = 0; i < count; i++)
                {
                    DataRow dR = dtLNS.NewRow();
                    dR["sLNS"] = "-1";
                    dtLNS.Rows.Add(dR);
                }
            }
            return dtLNS;
        }
        /// <summary>
        /// Lấy danh sách loại ngân sách theo báo cáo
        /// </summary>
        /// <param name="dtSource">Danh sách loại ngân sách có dữ liệu</param>
        /// <param name="iSoTo">Tờ báo cáo cần xem</param>
        /// <param name="iReport">Loại báo cáo</param>
        /// <returns></returns>
        public DataTable sLNS(DataTable dtSource, String iSoTo, String iReport)
        {
            DataTable dtResult = new DataTable();
            int _iSoTo = String.IsNullOrEmpty(iSoTo) ? 1 : int.Parse(iSoTo);
            dtResult = dtSource.Clone();
            switch (iReport)
            { 
                case "A4Dung":
                    QueryLNS(dtSource, dtResult, 1);
                    break;
                case "A4Ngang":
                    QueryLNS(dtSource, dtResult, _iSoTo);
                    break;
            }
            dtSource.Dispose();
            return dtResult;
        }
        /// <summary>
        /// Lấy danh sách loại ngân sách cần tìm kiếm
        /// </summary>
        /// <param name="dtSource">Danh sách loại ngân sách có dữ liệu</param>
        /// <param name="dtResult">Danh sách loại ngân sách theo mục đích tìm kiếm</param>
        /// <param name="_iSoTo">Số tờ</param>
        private static void QueryLNS(DataTable dtSource, DataTable dtResult, int _iSoTo)
        {
            for (int i = 0; i < 9; i++)
            {
                DataRow dR = dtResult.NewRow();
                dR["sLNS"] = dtSource.Rows[i + _iSoTo * 9 - 9]["sLNS"];
                dR["sMoTa"] = dtSource.Rows[i + _iSoTo * 9 - 9]["sMoTa"];
                dtResult.Rows.Add(dR);
            }
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iDVTinh">Đơn vị tính</param>
        public void LoadData(FlexCelReport fr, String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iDVTinh, String iSoTo, String iReport, String UserID,String iID_MaTrangThaiDuyet)
        {
            DataTable dtDuToan = TH_RutDuToan(iTuNgay, iDenNgay, iTuThang, iDenThang, iNam, iDVTinh,iSoTo,iReport,UserID,iID_MaTrangThaiDuyet);
            fr.AddTable("ChiTiet", dtDuToan);
            dtDuToan.Dispose();
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file báo cáo</param>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iDVTinh">Đơn vị tính</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iDVTinh, String iSoTo, String iReport, String UserID, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            //Thêm chữ ký vào báo cáo
            fr = ReportModels.LayThongTinChuKy(fr, "rptKT_TongHop_RutDuToan");
            LoadData(fr, iTuNgay, iDenNgay, iTuThang, iDenThang, iNam, iDVTinh,iSoTo,iReport,UserID,iID_MaTrangThaiDuyet);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            DataTable dt = sLNS(GetLNS(iTuNgay, iDenNgay, iTuThang, iDenThang, iNam,UserID,iID_MaTrangThaiDuyet),iSoTo,iReport);
            if (dt.Rows.Count > 0)
                for (int i = 0; i < dt.Rows.Count; i++)                
                    fr.SetValue("LNS" + (i + 1), dt.Rows[i]["sMoTa"]);              
            if (iDVTinh.Equals("rD"))
                fr.SetValue("DVT", "Đồng");
            else if (iDVTinh.Equals("rND"))
                fr.SetValue("DVT", "Nghìn đồng");
            else if (iDVTinh.Equals("rTrD"))
                fr.SetValue("DVT", "Triệu đồng");
            fr.SetValue("NgayThangTK", "Từ ngày " + iTuNgay + "/ " + iTuThang + " Đến ngày " + iDenNgay + " /" + iDenThang + "/" + iNam);
            if (iReport.Equals("A4Ngang"))
                fr.SetValue("SoTo", "Tờ số: "+iSoTo);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iDVTinh">Đơn vị tính</param>
        /// <param name="iPages">Khổ giấy giấy báo cáo</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iDVTinh, String iSoTo, String iReport, String UserID, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(getPath(iReport,iSoTo)), iTuNgay, iDenNgay, iTuThang, iDenThang, iNam, iDVTinh,iSoTo,iReport,UserID,iID_MaTrangThaiDuyet);
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
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iDVTinh">Đơn vị tính</param>
        /// <param name="iPages">In ngang | In đứng</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iDVTinh, String iSoTo, String iReport, String UserID, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(getPath(iReport,iSoTo)), iTuNgay, iDenNgay, iTuThang, iDenThang, iNam, iDVTinh,iSoTo,iReport,UserID,iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHop_RutDuToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy ngày theo tháng năm
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iNgay">Ngày</param>
        /// <returns></returns>
        public String obj_DSNgay(String ParentID, String iThang, String iNam, String iNgay,String FromOrTo)
        {
            String dsNgay = "";
            DataTable dtNgay = DanhMucModels.DT_Ngay(int.Parse(iThang), int.Parse(iNam));
            dtNgay.Rows.RemoveAt(0);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            dsNgay = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, FromOrTo, "", "class=\"input1_2\" style=\"width: 46px; padding:2px;\"");
            return dsNgay;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iNgay">Ngày</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsNgay(string ParentID, String iThang, String iNam, String iNgay, String FromOrTo)
        {
            return Json(obj_DSNgay(ParentID, iThang, iNam, iNgay,FromOrTo), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách số tờ
        /// </summary>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>        
        /// <returns></returns>
        public DataTable GetSoTo(String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String UserID,String iID_MaTrangThaiDuyet)
        {
            DataTable dt = GetLNS(iTuNgay, iDenNgay, iTuThang, iDenThang, iNam,UserID,iID_MaTrangThaiDuyet);
            int count = 0;
            DataTable dtSoTo = new DataTable();
            dtSoTo.Columns.Add("MaTo", typeof(String));
            dtSoTo.Columns.Add("TenTo", typeof(String));
            int soto = 0;
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows.Count;      
                if (count % 9 == 0)
                    soto = count / 9;
                else
                    soto = count / 9 + 1;
                for (int i = 0; i < soto; i++)
                {
                    DataRow dr = dtSoTo.NewRow();
                    dr["MaTo"] = i+1;
                    dr["TenTo"] = "Tờ " + (i+1);
                    dtSoTo.Rows.Add(dr);
                }
            }
            return dtSoTo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iSoTo">Số tờ</param>
        /// <returns></returns>
        public String obj_DSSoTo(String ParentID, String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iSoTo, String attr, String UserID, String iID_MaTrangThaiDuyet)
        {
            String dsSoTo = "";
            DataTable dtSoTo = GetSoTo(iTuNgay, iDenNgay, iTuThang, iDenThang, iNam,UserID,iID_MaTrangThaiDuyet);
            SelectOptionList slSoTo = new SelectOptionList(dtSoTo, "MaTo", "TenTo");
            dsSoTo = MyHtmlHelper.DropDownList(ParentID, slSoTo, iSoTo, "iSoTo", "", "class=\"input1_2\" style=\"width: 66px; padding:2px;\" "+attr+" ");
            return dsSoTo;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iTuNgay">Từ ngày</param>
        /// <param name="iDenNgay">Đến ngày</param>
        /// <param name="iTuThang">Từ tháng</param>
        /// <param name="iDenThang">Đến tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iSoTo">Số tờ</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsSoTo(string ParentID, String iTuNgay, String iDenNgay, String iTuThang, String iDenThang, String iNam, String iSoTo, String attr, String UserID,String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DSSoTo(ParentID, iTuNgay,iDenNgay,iTuThang,iDenNgay, iNam, iSoTo,attr,UserID,iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }        
    }
}