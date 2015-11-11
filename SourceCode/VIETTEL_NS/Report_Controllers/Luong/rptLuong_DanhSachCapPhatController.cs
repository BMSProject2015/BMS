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

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_DanhSachCapPhatController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_DanhSachCapPhat.xls";
        private const String sFilePath_A3 = "/Report_ExcelFrom/Luong/rptLuong_DanhSachCapPhat_A3.xls";
        private const String sFilePath_A3_iHuongLuong0 = "/Report_ExcelFrom/Luong/rptLuong_DSCapPhatPhuCap_A3.xls";
        private const String sFilePath_iHuongLuong0 = "/Report_ExcelFrom/Luong/rptLuong_DSCapPhatPhuCap.xls";
        private int Count;
        private long Tong;
        public ActionResult Index()
        {
            ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DanhSachCapPhat.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamBangLuong = Convert.ToString(Request.Form[ParentID + "_NamBangLuong"]);
            String ThangBangLuong = Convert.ToString(Request.Form[ParentID + "_ThangBangLuong"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["NamBangLuong"] = NamBangLuong;
            ViewData["ThangBangLuong"] = ThangBangLuong;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DanhSachCapPhat.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="NamBangLuong">Năm  bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <param name="KhoGiay">Khổ giấy in A4 hoặc A3</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV = "";
            DataTable dtDonVi = Luong_ReportModel.DanhSach_DonVi(NamBangLuong, ThangBangLuong,iID_MaTrangThaiDuyet);
            if (dtDonVi.Rows.Count > 0 && Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]) != "-2")
            {
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    if (iID_MaDonVi == Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]))
                    {
                        TenDV = Convert.ToString(dtDonVi.Rows[i]["sTenDonVi"]);
                    }
                }
            }
            DataTable dtBH = dtBaoHiem(NamBangLuong, ThangBangLuong, iID_MaDonVi, iID_MaTrangThaiDuyet);
            String BHXH = "", BHYT = "", BHTN = "";
            if (dtBH.Rows[0]["BHXH"] != null) BHXH = dtBH.Rows[0]["BHXH"].ToString();
            if (dtBH.Rows[0]["BHYT"] != null) BHYT = dtBH.Rows[0]["BHYT"].ToString();
            if (dtBH.Rows[0]["BHTN"] != null) BHTN = dtBH.Rows[0]["BHTN"].ToString();
            if (BHXH == "0") BHXH = "";
            if (BHYT == "0") BHYT = "";
            if (BHTN == "0") BHTN = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_DanhSachCapPhat");
            LoadData(fr, NamBangLuong, ThangBangLuong, iID_MaDonVi,iID_MaTrangThaiDuyet);
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("Nams", DateTime.Now.Year);
            fr.SetValue("ThangNam", "Tháng " + ThangBangLuong + " / " + NamBangLuong);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Nam", NamBangLuong);
            fr.SetValue("Thang", ThangBangLuong);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("Count", Count);
            fr.SetValue("BHXH", BHXH);
            fr.SetValue("BHYT", BHYT);
            fr.SetValue("BHTN", BHTN);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Ngay",DateTime.Now.Day+"-"+DateTime.Now.Month+"-"+DateTime.Now.Year);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong));            
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="NamBangLuong">Năm bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        private void LoadData(FlexCelReport fr, String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String iHuongLuong = "1";
            DataTable dt_DonVi = dtDonVi();
            for (int i = 0; i < dt_DonVi.Rows.Count; i++)
            {
                if (iID_MaDonVi == dt_DonVi.Rows[i]["iID_MaDonVi"].ToString())
                {
                    iHuongLuong = dt_DonVi.Rows[i]["iHuongLuong"].ToString();
                    break;
                }
            }
            dt_DonVi.Dispose();
            if (iHuongLuong == "1")
            {

                DataTable data = DanhSach_CapPhatLuong(NamBangLuong, ThangBangLuong, iID_MaDonVi, iID_MaTrangThaiDuyet);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);

                DataTable dtNgach;
                dtNgach = HamChung.SelectDistinct("Ngach", data, "sTenNgachLuong", "sTenNgachLuong,iSTT");
                DataView dv = dtNgach.DefaultView;
                dv.Sort="iSTT";
                dtNgach = dv.ToTable();
                data.TableName = "Ngach";
                fr.AddTable("Ngach", dtNgach);
                 
                data.Dispose();
                dtNgach.Dispose();
            }
            else
            {
                DataTable data = DanhSach_CapPhatLuong(NamBangLuong, ThangBangLuong, iID_MaDonVi, iID_MaTrangThaiDuyet);
                if (data.Rows.Count > 0)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        Tong += data.Rows[i]["Cong"].ToString().Equals("") ? 0 : long.Parse(data.Rows[i]["Cong"].ToString());
                    }
                }
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                data.Dispose();
            }                     
        }
        
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF lên trình duyệt
        /// </summary>
        /// <param name="NamBangLuong">Năm bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <param name="KhoGiay">Khổ giấy in A3 hoặc A4</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile="";

            String iHuongLuong = "1";
            DataTable dt_DonVi = dtDonVi();
            for (int i = 0; i < dt_DonVi.Rows.Count; i++)
            {
                if (iID_MaDonVi == dt_DonVi.Rows[i]["iID_MaDonVi"].ToString())
                {
                    iHuongLuong = dt_DonVi.Rows[i]["iHuongLuong"].ToString();
                    break;
                }
            }
            dt_DonVi.Dispose();
            if (iHuongLuong == "1")
            {

                if (KhoGiay == "1")
                    DuongDanFile = sFilePath_A3;
                else DuongDanFile = sFilePath;
            }
            else
            {
                if (KhoGiay == "1")
                    DuongDanFile = sFilePath_A3_iHuongLuong0;
                else DuongDanFile = sFilePath_iHuongLuong0;
            }
          
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamBangLuong, ThangBangLuong, iID_MaDonVi,iID_MaTrangThaiDuyet,KhoGiay);
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
        /// <param name="NamBangLuong">Năm bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <param name="KhoGiay">Khổ giấy in</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            String DuongDanFile = "";
            if (KhoGiay == "1")
                DuongDanFile = sFilePath_A3;
            else DuongDanFile = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamBangLuong, ThangBangLuong, iID_MaDonVi, iID_MaTrangThaiDuyet, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTC_NSQP_NganSachKhac.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hàm Ajax lấy danh sách đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamBangLuong">Năm bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID, String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {

            return Json(obj_DSDonVi(ParentID, NamBangLuong, ThangBangLuong, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamBangLuong"></param>
        /// <param name="ThangBangLuong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String ParentID, String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String dsDonVi = "";
            DataTable dtDonVi = Luong_ReportModel.DanhSach_DonVi(NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\"");
            return dsDonVi;
        }
        /// <summary>
        /// Danh sách 
        /// </summary>
        /// <param name="NamBangLuong"></param>
        /// <param name="ThangBangLuong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public static DataTable dtBaoHiem(String NamBangLuong, String ThangBangLuong, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
                String SQL=String.Format(@"SELECT SUM(rBaoHiem_XaHoi_CaNhan) as BHXH,SUM(rBaoHiem_YTe_CaNhan)as BHYT,SUM(rBaoHiem_ThatNghiep_CaNhan) as BHTN
                                        FROM L_BangLuongChiTiet
                                        WHERE iThangBangLuong=@ThangBangLuong AND iNamBangLuong=@NamBangLuong AND iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi {0}
                                         ", DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamBangLuong", NamBangLuong);
            cmd.Parameters.AddWithValue("@ThangBangLuong", ThangBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (iID_MaTrangThaiDuyet != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong));
            }
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "";
                    dt.Rows.InsertAt(dr, 0);
                    dt.Dispose();
                }
            return dt;
            }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamBangLuong">Năm bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public DataTable DanhSach_CapPhatLuong(String NamBangLuong,String ThangBangLuong,String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            String iHuongLuong = "1";
            DataTable dt_DonVi = dtDonVi();
            for (int i = 0; i < dt_DonVi.Rows.Count; i++)
            {
                if (iID_MaDonVi == dt_DonVi.Rows[i]["iID_MaDonVi"].ToString())
                {
                    iHuongLuong = dt_DonVi.Rows[i]["iHuongLuong"].ToString();
                    break;
                }
            }
            dt_DonVi.Dispose();
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
           
            DataTable dt = new DataTable() ;
            //Lấy danh sách cán bộ thuộc đơn vị hưởng lương
            if (iHuongLuong == "1")
            {
                String SQL = String.Format(@"SELECT   iSTT,a.sHoDem_CanBo
		, a.sTen_CanBo
		, a.iID_MaBacLuong_CanBo
		, dbo.L_DanhMucNgachLuong.sTenNgachLuong
		, a.sSoSoLuong_CanBo
		, a.sSoTaiKhoan_CanBo
		, a.dNgayNhapNgu_CanBo
		, a.dNgayXuatNgu_CanBo
		, a.dNgayTaiNgu_CanBo
		, a.rLuongCoBan_HeSo_CanBo
		, a.rPhuCap_BaoLuu_HeSo
		, a.rPhuCap_VuotKhung_HeSo
		, a.rLuongCoBan
		, a.rPhuCap_ChucVu
		, a.rPhuCap_ThamNien
		, a.rPhuCap_TrachNhiem
		, a.rPhuCap_KhuVuc
        , a.rPhuCap_DacBiet
		, sPhuCap_DacBiet_MoTa=CASE WHEN (sPhuCap_DacBiet_MoTa='') THEN sPhuCap_DacBiet_MoTa ELSE 'PC: '+sPhuCap_DacBiet_MoTa END
		, a.rPhuCap_Khac
		, a.sPhuCap_Khac_MoTa
		, a.rBaoHiem_Tong_CaNhan
		, a.rThueTNCN
		, a.rKhoanTru_TienAn as rTienAn1Ngay
		, a.rPhuCap_BaoLuu+a.rPhuCap_VuotKhung
		, a.BaoLuu_VuotKhung*1 as BaoLuu_VuotKhung
        , a.rKhoanTru_Khac
        , a.rTrichLuong
        , a.rBaoHiem_XaHoi_CaNhan
        ,a.rBaoHiem_ThatNghiep_CaNhan
        ,a.rBaoHiem_YTe_CaNhan
         FROM         (SELECT sHoDem_CanBo
										, sTen_CanBo
										, iID_MaBacLuong_CanBo
										, iID_MaNgachLuong_CanBo
										, sSoSoLuong_CanBo
										, sSoTaiKhoan_CanBo
										, SUBSTRING(CONVERT(varchar(10),dNgayNhapNgu_CanBo, 5), 4, 5) AS dNgayNhapNgu_CanBo
										, SUBSTRING(CONVERT(varchar(10), dNgayXuatNgu_CanBo, 5), 4, 5) AS dNgayXuatNgu_CanBo
                                        , SUBSTRING(CONVERT(varchar(10), dNgayTaiNgu_CanBo, 5), 4, 5) AS dNgayTaiNgu_CanBo
                                        ,CASE WHEN CAST(rLuongCoBan_HeSo_CanBo AS varchar)= '0.00' THEN '' ELSE CAST(rLuongCoBan_HeSo_CanBo AS varchar) END AS rLuongCoBan_HeSo_CanBo
                                        ,CASE WHEN CAST(rPhuCap_BaoLuu_HeSo AS varchar)='0.00' THEN '' ELSE CAST(rPhuCap_BaoLuu_HeSo AS varchar) END AS rPhuCap_BaoLuu_HeSo
                                        ,CASE WHEN CAST (rPhuCap_VuotKhung_HeSo AS varchar)='0.00' THEN '' ELSE CAST(rPhuCap_VuotKhung_HeSo AS varchar) END AS rPhuCap_VuotKhung_HeSo
                                        ,CASE WHEN CAST((rPhuCap_BaoLuu+rPhuCap_VuotKhung) as varchar)='0' THEN '' ELSE CAST((rPhuCap_BaoLuu+rPhuCap_VuotKhung) as varchar) END as BaoLuu_VuotKhung
                                        ,rPhuCap_BaoLuu
                                        ,rPhuCap_VuotKhung
                                        ,rLuongCoBan
                                        ,rPhuCap_ChucVu 
                                        ,rPhuCap_ThamNien=CASE WHEN iID_MaNgachLuong_CanBo IN(1,2) THEN rPhuCap_ThamNien ELSE rPhuCap_AnNinhQuocPhong END
										,rPhuCap_TrachNhiem
										,rPhuCap_KhuVuc
                                        ,rPhuCap_DacBiet + rPhuCap_CongVu as rPhuCap_DacBiet
										,sPhuCap_DacBiet_MoTa + sPhuCap_CongVu_MoTa as sPhuCap_DacBiet_MoTa                                  
										,rPhuCap_Khac
                                        ,sPhuCap_Khac_MoTa
                                        ,rBaoHiem_Tong_CaNhan
                                        ,rThueTNCN
                                        ,rKhoanTru_TienAn
                                        ,rKhoanTru_Khac
                                        ,rTrichLuong
										,rBaoHiem_XaHoi_CaNhan
                                        ,rBaoHiem_ThatNghiep_CaNhan
                                        ,rBaoHiem_YTe_CaNhan
                                    FROM  dbo.L_BangLuongChiTiet
                                    WHERE   iID_MaNgachLuong_CanBo <> 4  
											AND iThangBangLuong = @ThangBangLuong 
											AND iNamBangLuong = @NamBangLuong
											AND iTrangThai = 1
											AND iID_MaDonVi = @iID_MaDonVi
                                            AND bOmDaiNgay=0
                                            {0} 
									) AS a 
                INNER JOIN dbo.L_DanhMucNgachLuong 
                    ON a.iID_MaNgachLuong_CanBo = dbo.L_DanhMucNgachLuong.iID_MaNgachLuong
                ORDER BY iSTT
                                                ", DKTrangThaiDuyet);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@NamBangLuong", NamBangLuong);
                cmd.Parameters.AddWithValue("@ThangBangLuong", ThangBangLuong);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                if (iID_MaTrangThaiDuyet != "-1")
                {
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong));
                }
                 dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                Count = 0;
                Count = dt.Rows.Count;
                Tong = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["BaoLuu_VuotKhung"].ToString())+long.Parse(dt.Rows[i]["rLuongCoBan"].ToString()) + long.Parse(dt.Rows[i]["rPhuCap_ChucVu"].ToString()) + long.Parse(dt.Rows[i]["rPhuCap_ThamNien"].ToString()) + long.Parse(dt.Rows[i]["rPhuCap_TrachNhiem"].ToString()) + long.Parse(dt.Rows[i]["rPhuCap_KhuVuc"].ToString()) + long.Parse(dt.Rows[i]["rPhuCap_Khac"].ToString());
                }
                if (Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "";
                    dt.Rows.InsertAt(dr, 0);
                    dt.Dispose();
                }
            }
            //nếu iHuongLuong=0
            //Lấy danh sách cán bộ thuộc đơn vị hưởng phụ cấp
            else
            {
               // String DKDuyet = iID_MaTrangThaiDuyet.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
                String DKDonvi = " AND L.iID_MaDonVi IN (";
                String[] arrDonvi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonvi.Length; i++)
                {
                    DKDonvi += "@iID_MaDonVi" + i;
                    if (i < arrDonvi.Length - 1)
                        DKDonvi += ",";
                }
                DKDonvi += ")";
                String SQL = String.Format(@"SELECT L.HoDem,L.Ten,L.SHPC,L.CB,L.iID_MaDonVi,L.NG
                                                  ,SUM(L.QH) AS QH,SUM(L.CV) CV,SUM(L.HD) HD,SUM(L.TN) TN,SUM(L.KVDB) KVDB
                                                  ,SUM(L.NCS) NCS,SUM(L.KHAC+L.pBG+L.pNU+L.pTemThu) KHAC,SUM(L.TRICHLUONG) TRICHLUONG
                                                  ,Cong=SUM(l.QH+l.CV+L.HD+L.TN+L.KVDB+L.NCS+L.KHAC+L.pBG+L.pNU+L.pTemThu+L.TRICHLUONG)
                                            FROM(
	                                            SELECT L.sHoDem_CanBo as HoDem,L.sTen_CanBo AS Ten,L.sSoSoLuong_CanBo as SHPC
                                                ,L.iID_MaBacLuong_CanBo as CB,L.iID_MaDonVi
                                                ,SUBSTRING(CONVERT(varchar(10),L.dNgayNhapNgu_CanBo, 5), 4, 5) as NG
                                                ,SUM(L.rLuongCoBan) as QH,SUM(L.rPhuCap_ChucVu) as CV,SUM(L.rPhuCap_TrenHanDinh) as HD
                                                ,SUM(L.rPhuCap_TrachNhiem) as TN,SUM(L.rPhuCap_KhuVuc+L.rPhuCap_DacBiet+L.rPhuCap_CongVu) AS KVDB,SUM(L.rPhuCap_NuQuanNhan) as NCS
                                                ,SUM(L.rPhuCap_Khac) as KHAC    
                                                ,pNU=CONVERT(DECIMAL,(SELECT  TOP 1 L.sThamSo FROM L_DanhMucThamSo L WHERE L.iTrangThai=1 AND L.sNoiDung=N'Nước uống' ORDER BY L.dThoiGianApDung_BatDau DESC))
                                                ,pTemThu=CONVERT(DECIMAL,(SELECT  TOP 1 L.sThamSo FROM L_DanhMucThamSo L WHERE L.iTrangThai=1 AND L.sNoiDung=N'Tem thư' ORDER BY L.dThoiGianApDung_BatDau DESC))
                                                ,pBG=CONVERT(DECIMAL,(SELECT  TOP 1 L.sThamSo FROM L_DanhMucThamSo L WHERE L.iTrangThai=1 AND L.sNoiDung=N'Bù giá gạo' ORDER BY L.dThoiGianApDung_BatDau DESC))
                                                ,SUM(L.rTrichLuong) AS TRICHLUONG                                                                                  
	                                            FROM L_BangLuongChiTiet AS L
	                                            WHERE L.iTrangThai=1
                                                  AND L.iThangBangLuong=@iThangBangLuong AND L.iNamBangLuong=@iNamBangLuong
                                                  {1}
                                                  AND L.iID_MaNgachLuong_CanBo=4
                                                  {0}
                                               GROUP BY L.sHoDem_CanBo
                                                ,L.sTen_CanBo
                                                ,L.sSoSoLuong_CanBo
                                                ,L.iID_MaBacLuong_CanBo	
                                                ,L.iID_MaDonVi,
                                                L.dNgayNhapNgu_CanBo
                                                ) AS L  
                                            GROUP BY L.HoDem,L.Ten,L.SHPC,L.CB,L.iID_MaDonVi,L.NG
                                            HAVING SUM(l.QH+l.CV+L.HD+L.TN+L.KVDB+L.NCS+L.KHAC+L.pBG+L.pNU+L.pTemThu+L.TRICHLUONG)>0
                                            ORDER BY L.Ten", DKTrangThaiDuyet, DKDonvi);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThangBangLuong", ThangBangLuong);
                cmd.Parameters.AddWithValue("@iNamBangLuong", NamBangLuong);
                for (int z = 0; z < arrDonvi.Length; z++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + z, arrDonvi[z]);
                }                
                int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
                if (iID_MaTrangThaiDuyet != "-1")
                {
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose(); 
            }           
            return dt;
        }
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable dtDonVi()
        {
            String SQL = "SELECT DISTINCT iHuongLuong,iID_MaDonVi FROM NS_DonVi";
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }

    }
}
