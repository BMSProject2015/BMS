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
using System.Text;

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_TongHopLuongPhuCapController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_TongHopLuongPhuCap.xls";
        private const String sFilePath_A3 = "/Report_ExcelFrom/Luong/rptLuong_TongHopLuongPhuCap_A3.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_TongHopLuongPhuCap.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
           
            String NamBangLuong = Convert.ToString(Request.Form[ParentID + "_NamBangLuong"]);
            String ThangBangLuong = Convert.ToString(Request.Form[ParentID + "_ThangBangLuong"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String TongHop = Convert.ToString(Request.Form[ParentID + "_TongHop"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["PageLoad"] = 1;
            ViewData["NamBangLuong"] = NamBangLuong;
            ViewData["ThangBangLuong"] = ThangBangLuong;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["TongHop"] = TongHop;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_TongHopLuongPhuCap.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TongHop,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String Ngay = "";
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            Ngay = "Ngày  " + day + " Tháng  " + month + "  Năm  " + year;
           FlexCelReport fr = new FlexCelReport();
           fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_TongHopLuongPhuCap");
                LoadData(fr, NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet, iID_MaDonVi, TongHop,KhoGiay);
                fr.SetValue("Nam", NamBangLuong);
                fr.SetValue("Thang", ThangBangLuong);
                fr.SetValue("Ngay", Ngay);
                fr.Run(Result);
                return Result;
            

        }
        private void LoadData(FlexCelReport fr, String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TongHop,String KhoGiay)
        {

            DataTable data = Luong_TongHopLuongPhuCap(NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet, iID_MaDonVi, TongHop);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtNgach;
            dtNgach = HamChung.SelectDistinct("Ngach", data, "sTenNgachLuong", "sTenNgachLuong","","","DESC");
            data.TableName = "Ngach";
            fr.AddTable("Ngach", dtNgach);

            data.Dispose();
            dtNgach.Dispose();

        }
      
        public ActionResult ViewPDF(String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TongHop,String KhoGiay)
        {
            String DuongDanFile = "";
            if (KhoGiay == "1")
                DuongDanFile = sFilePath_A3;
            else DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet, iID_MaDonVi, TongHop, KhoGiay);
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
        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID, String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet,String iID_MaDonVi)
        {

            return Json(obj_DSDonVi(ParentID, NamBangLuong, ThangBangLuong,iID_MaTrangThaiDuyet, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet,String iID_MaDonVi)
        {
            String input = "";
            DataTable dt = Luong_ReportModel.DanhSach_DonVi(NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet);
            StringBuilder stbDonVi = new StringBuilder();
            stbDonVi.Append("<div style=\"width:200px; height: 150px; overflow: scroll; border:1px solid black;\">");
            stbDonVi.Append("<table class=\"mGrid\">");
            stbDonVi.Append("<tr>");
            stbDonVi.Append("<td style=\"width: 15%;\"><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");
            String TenDonVi = "", MaDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                TenDonVi = Convert.ToString(dt.Rows[i]["sTenDonVi"]);
                _Checked = "";
                for (int j = 1; j <= arrDonVi.Length; j++)
                {
                    if (MaDonVi == arrDonVi[j-1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" />", MaDonVi, _Checked);
                stbDonVi.Append("<tr>");
                stbDonVi.Append("<td style=\"width: 15%;\">");
                stbDonVi.Append(input);
                stbDonVi.Append("</td>");
                stbDonVi.Append("<td>" + TenDonVi + "</td>");

                stbDonVi.Append("</tr>");
            }
            stbDonVi.Append("</table>");
            stbDonVi.Append("</div>");
            dt.Dispose();
            String DonVi = stbDonVi.ToString();
            return DonVi;         
        }
        public DataTable Luong_TongHopLuongPhuCap(String NamBangLuong, String ThangBangLuong,String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TongHop)
        {
            
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String DKDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            String SQL;
            if (TongHop == "on")
            {
                SQL = String.Format(@"SELECT sTenNgachLuong,SoNguoi,LuongCB,ChucVu,iID_MaDonVi='0000000000'
                                        ,ThamNien,TrNhiem,DacBiet,PCKhac,BHXH,BHYT,BHTN,TruAn,ThueTNCN,TrichLuong,TruKhac FROM(
                                        SELECT iID_MaNgachLuong_CanBo
                                        ,SoNguoi=SUM(CASE WHEN rLuongCoBan>0 THEN 1 ELSE 0 END)
                                        ,LuongCB=SUM(rLuongCoBan)
                                        ,ChucVu=SUM(rPhuCap_ChucVu)
                                        ,ThamNien=CASE WHEN (iID_MaNgachLuong_CanBo=3) THEN SUM(rPhuCap_AnNinhQuocPhong) ELSE SUM(rPhuCap_ThamNien ) END
                                        ,TrNhiem=SUM(rPhuCap_TrachNhiem)
                                        ,DacBiet=CASE WHEN (SUM(rPhuCap_DacBiet))
                                        ,PCKhac=SUM(rPhuCap_Khac)
                                        ,BHXH=SUM(rBaoHiem_XaHoi_CaNhan)
                                        ,BHYT=SUM(rBaoHiem_YTe_CaNhan)
                                        ,BHTN=SUM(rBaoHiem_ThatNghiep_CaNhan)
                                        ,TruAn=SUM(rKhoanTru_TienAn)
                                        ,ThueTNCN=SUM(rThueTNCN)
                                        ,TrichLuong=SUM(rTrichLuong)
                                        ,TruKhac=SUM(rKhoanTru_Khac)
                                        FROM L_BangLuongChiTiet
                                        WHERE iTrangThai=1 AND iThangBangLuong=@ThangBangLuong AND iNamBangLuong=@NamBangLuong AND ({0}) {1}
                                        GROUP BY iID_MaNgachLuong_CanBo) as a
                                        INNER JOIN L_DanhMucNgachLuong on a.iID_MaNgachLuong_CanBo=L_DanhMucNgachLuong.iID_MaNgachLuong
                                        ORDER BY sTenNgachLuong DESC", DKDonVi, DKTrangThaiDuyet);
            }
            else
            {
                SQL = String.Format(@"SELECT sTenNgachLuong,iID_MaDonVi,SoNguoi,LuongCB,ChucVu
                                        ,ThamNien,TrNhiem,DacBiet,PCKhac,BHXH,BHYT,BHTN,TruAn,ThueTNCN,TrichLuong,TruKhac FROM(
                                        SELECT iID_MaNgachLuong_CanBo,iID_MaDonVi
                                        ,SoNguoi=SUM(CASE WHEN rLuongCoBan>0 THEN 1 ELSE 0 END)
                                        ,LuongCB=SUM(rLuongCoBan)
                                        ,ChucVu=SUM(rPhuCap_ChucVu)
                                        ,ThamNien=CASE WHEN (iID_MaNgachLuong_CanBo=3) THEN SUM(rPhuCap_AnNinhQuocPhong) ELSE SUM(rPhuCap_ThamNien ) END
                                        ,TrNhiem=SUM(rPhuCap_TrachNhiem)
                                        ,DacBiet=SUM(rPhuCap_DacBiet)
                                        ,PCKhac=SUM(rPhuCap_Khac)
                                        ,BHXH=SUM(rBaoHiem_XaHoi_CaNhan)
                                        ,BHYT=SUM(rBaoHiem_YTe_CaNhan)
                                        ,BHTN=SUM(rBaoHiem_ThatNghiep_CaNhan)
                                        ,TruAn=SUM(rKhoanTru_TienAn)
                                        ,ThueTNCN=SUM(rThueTNCN)
                                        ,TrichLuong=SUM(rTrichLuong)
                                        ,TruKhac=SUM(rKhoanTru_Khac)
                                        FROM L_BangLuongChiTiet
                                        WHERE iTrangThai=1 AND iThangBangLuong=@ThangBangLuong AND iNamBangLuong=@NamBangLuong AND ({0}) {1}
                                        GROUP BY iID_MaNgachLuong_CanBo,iID_MaDonVi) as a
                                        INNER JOIN L_DanhMucNgachLuong on a.iID_MaNgachLuong_CanBo=L_DanhMucNgachLuong.iID_MaNgachLuong
                                        ORDER BY sTenNgachLuong DESC", DKDonVi, DKTrangThaiDuyet);
            }
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamBangLuong", NamBangLuong);
            cmd.Parameters.AddWithValue("@ThangBangLuong", ThangBangLuong);
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
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
