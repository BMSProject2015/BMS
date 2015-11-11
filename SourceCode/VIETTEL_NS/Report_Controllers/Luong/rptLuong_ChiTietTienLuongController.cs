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
    public class rptLuong_ChiTietTienLuongController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_ChiTietTienLuong.xls";
        private const String sFilePath_A3 = "/Report_ExcelFrom/Luong/rptLuong_ChiTietTienLuong_A3.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["pageload"] = "0";
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_ChiTietTienLuong.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {           
            String NamBangLuong = Convert.ToString(Request.Form[ParentID + "_NamBangLuong"]);
            String ThangBangLuong = Convert.ToString(Request.Form[ParentID + "_ThangBangLuong"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay=Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["NamBangLuong"] = NamBangLuong;
            ViewData["ThangBangLuong"] = ThangBangLuong;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["pageload"] = "1";
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_ChiTietTienLuong.aspx";
            return View(sViewPath + "ReportView.aspx");
           
        }
        public ExcelFile CreateReport(String path, String NamBangLuong, String ThangBangLuong, String iID_MaTrangThaiDuyet,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            
             FlexCelReport fr = new FlexCelReport();

             fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_ChiTietTienLuong");
                LoadData(fr, NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam", NamBangLuong);
                fr.SetValue("Thang", ThangBangLuong);
                fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
                fr.Run(Result);
                return Result;
            

        }
        private void LoadData(FlexCelReport fr, String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet)
        {

            DataTable data = ChiTiet_Luong(NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtNgach;
            dtNgach = HamChung.SelectDistinct("Ngach", data, "sTenNgachLuong", "sTenNgachLuong", "", "", "DESC");
            data.TableName = "Ngach";
            fr.AddTable("Ngach", dtNgach);

            data.Dispose();
            dtNgach.Dispose();

        }
        
        public ActionResult ViewPDF(String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet,String KhoGiay)
        {
            String DuongDanFile = "";
            if (KhoGiay == "1") DuongDanFile = sFilePath_A3;
            else DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet,KhoGiay);
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

        public DataTable ChiTiet_Luong(String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet)
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
            String SQL = String.Format(@"SELECT sTenNgachLuong,iID_MaNgachLuong_CanBo,iID_MaBacLuong_CanBo,rLuongCoBan_HeSo_CanBo,LuongCoBan_Nguoi,LuongCoBanTien,VK_BL_Tien,VK_BL_Nguoi,
                                        ChucVu_Nguoi,ChucVu_Tien,ThamNien_Nguoi,ThamNien_Tien,KhuVuc_Nguoi,KhuVuc_Tien,PCKhac_Nguoi,PCKhac_Tien,BHXH,BHYT,BHTN
                                         FROM (SELECT iID_MaNgachLuong_CanBo, iID_MaBacLuong_CanBo,rLuongCoBan_HeSo_CanBo,
                                        LuongCoBan_Nguoi=SUM(CASE WHEN rLuongCoBan_HeSo_CanBo>0 THEN 1 ELSE 0 END)
                                        ,LuongCoBanTien=SUM(rLuongCoBan)
                                        ,VK_BL_Nguoi=SUM(CASE WHEN (rPhuCap_BaoLuu_HeSo=0 AND rPhuCap_VuotKhung_HeSo=0) THEN 0 ELSE 1 END)
                                        ,VK_BL_Tien=SUM(rPhuCap_BaoLuu)+ SUM(rPhuCap_VuotKhung)
                                        ,ChucVu_Nguoi=SUM(CASE WHEN rPhuCap_ChucVu>0 THEN 1 ELSE 0 END)
                                        ,ChucVu_Tien=SUM(rPhuCap_ChucVu)
                                        ,ThamNien_Nguoi=SUM(CASE WHEN (rPhuCap_ThamNien>0 OR rPhuCap_AnNinhQuocPhong>0)  THEN 1 ELSE 0 END )
                                        ,ThamNien_Tien=CASE WHEN (iID_MaNgachLuong_CanBo=3) THEN SUM(rPhuCap_AnNinhQuocPhong) ELSE SUM(rPhuCap_ThamNien ) END
                                        ,KhuVuc_Nguoi=SUM(CASE WHEN rPhuCap_KhuVuc>0 THEN 1 ELSE 0 END)
                                        ,KhuVuc_Tien=SUM(rPhuCap_ChucVu)
                                        ,PCKhac_Nguoi=SUM(CASE WHEN rPhuCap_Khac>0 THEN 1 ELSE 0 END)
                                        ,PCKhac_Tien=SUM(rPhuCap_Khac)
                                        ,BHXH=SUM(rBaoHiem_XaHoi_CaNhan)
                                        ,BHYT=SUM(rBaoHiem_YTe_CaNhan)
                                        ,BHTN=SUM(rBaoHiem_ThatNghiep_CaNhan)
                                        FROM L_BangLuongChiTiet
                                        WHERE iTrangThai=1 AND iThangBangLuong=@ThangBangLuong AND iNamBangLuong=@NamBangLuong
                                        {0}
                                        GROUP BY rLuongCoBan_HeSo_CanBo,iID_MaBacLuong_CanBo,iID_MaNgachLuong_CanBo) as a
                                        INNER JOIN L_DanhMucNgachLuong on a.iID_MaNgachLuong_CanBo=L_DanhMucNgachLuong.iID_MaNgachLuong
                                        ORDER BY sTenNgachLuong DESC", DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamBangLuong", NamBangLuong);
            cmd.Parameters.AddWithValue("@ThangBangLuong", ThangBangLuong);
            if (iID_MaTrangThaiDuyet != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }      

    }
}
