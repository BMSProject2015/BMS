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
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNop_Thu01Controller : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNop_Thu01.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/ThuNop/rptThuNop_Thu01_DonVi.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_Thu01.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable rptThuNop_Thu01(String MaND, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String SQL =
                String.Format(@"SELECT iID_MaDonVi,sTenDonVi,
TN_sLNS as sLNS,sNG,sMoTa
,SUM(CASE WHEN bThoaiThu=0 THEN rTongThu WHEN bThoaiThu=1 THEN rTongThu*(-1) ELSE 0 END) rTongThu
,SUM(CASE WHEN bThoaiThu=0 THEN rKhauHaoTSCD WHEN bThoaiThu=1 THEN rKhauHaoTSCD*(-1) ELSE 0 END) rKhauHaoTSCD
,SUM(CASE WHEN bThoaiThu=0 THEN rTienLuong WHEN bThoaiThu=1 THEN rTienLuong*(-1) ELSE 0 END) rTienLuong
,SUM(CASE WHEN bThoaiThu=0 THEN rQTNSKhac WHEN bThoaiThu=1 THEN rQTNSKhac*(-1) ELSE 0 END) rQTNSKhac
,SUM(CASE WHEN bThoaiThu=0 THEN rChiPhiKhac WHEN bThoaiThu=1 THEN rChiPhiKhac*(-1) ELSE 0 END) rChiPhiKhac
,SUM(CASE WHEN bThoaiThu=0 THEN rNopThueTNDN WHEN bThoaiThu=1 THEN rNopThueTNDN*(-1) ELSE 0 END) rNopThueTNDN
,SUM(CASE WHEN bThoaiThu=0 THEN rNopNSNNQuaBQP WHEN bThoaiThu=1 THEN rNopNSNNQuaBQP*(-1) ELSE 0 END) rNopNSNNQuaBQP
,SUM(CASE WHEN bThoaiThu=0 THEN rNopNSNNKhac WHEN bThoaiThu=1 THEN rNopNSNNKhac*(-1) ELSE 0 END) rNopNSNNKhac
,SUM(CASE WHEN bThoaiThu=0 THEN rNopNSNN WHEN bThoaiThu=1 THEN rNopNSNN*(-1) ELSE 0 END) rNopNSNN
,SUM(CASE WHEN bThoaiThu=0 THEN rNopNSQP WHEN bThoaiThu=1 THEN rNopNSQP*(-1) ELSE 0 END) rNopNSQP
,SUM(CASE WHEN bThoaiThu=0 THEN rNopCapTren WHEN bThoaiThu=1 THEN rNopCapTren*(-1) ELSE 0 END) rNopCapTren
,SUM(CASE WHEN bThoaiThu=0 THEN rBoSungKinhPhi WHEN bThoaiThu=1 THEN rBoSungKinhPhi*(-1) ELSE 0 END) rBoSungKinhPhi
,SUM(CASE WHEN bThoaiThu=0 THEN rTrichQuyDonVi WHEN bThoaiThu=1 THEN rTrichQuyDonVi*(-1) ELSE 0 END) rTrichQuyDonVi
,SUM(CASE WHEN bThoaiThu=0 THEN rSoChuaPhanPhoi WHEN bThoaiThu=1 THEN rSoChuaPhanPhoi*(-1) ELSE 0 END) rSoChuaPhanPhoi
FROM TN_ChungTuChiTiet
WHERE iTrangThai=1 AND iLoai=2 {0} {1} {2} AND iNamLamViec=@iNamLamViec
GROUP BY iID_MaDonVi,sTenDonVi,
TN_sLNS,sNG,sMoTa
HAVING SUM(CASE WHEN bThoaiThu=0 THEN rTongThu WHEN bThoaiThu=1 THEN rTongThu*(-1) ELSE 0 END) <>0 OR 
 SUM(CASE WHEN bThoaiThu=0 THEN rKhauHaoTSCD WHEN bThoaiThu=1 THEN rKhauHaoTSCD*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rTienLuong WHEN bThoaiThu=1 THEN rTienLuong*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rQTNSKhac WHEN bThoaiThu=1 THEN rQTNSKhac*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rChiPhiKhac WHEN bThoaiThu=1 THEN rChiPhiKhac*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rNopThueTNDN WHEN bThoaiThu=1 THEN rNopThueTNDN*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rNopNSNNQuaBQP WHEN bThoaiThu=1 THEN rNopNSNNQuaBQP*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rNopNSNN WHEN bThoaiThu=1 THEN rNopNSNN*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rNopNSNNKhac WHEN bThoaiThu=1 THEN rNopNSNNKhac*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rNopNSQP WHEN bThoaiThu=1 THEN rNopNSQP*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rNopCapTren WHEN bThoaiThu=1 THEN rNopCapTren*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rBoSungKinhPhi WHEN bThoaiThu=1 THEN rBoSungKinhPhi*(-1) ELSE 0 END) <>0 OR
 SUM(CASE WHEN bThoaiThu=0 THEN rTrichQuyDonVi WHEN bThoaiThu=1 THEN rTrichQuyDonVi*(-1) ELSE 0 END) <>0 ", DKDonVi, DKPhongBan, DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, String MaND)
        {
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iLoaiBaoCao = Request.Form[ParentID + "_iLoaiBaoCao"];
            String sMoTa = Request.Form[ParentID + "_sMoTa"];


            //Update sMota
            String SQL = "";
            SQL = String.Format("DELETE KT_DanhMucThamSo_BaoCao WHERE iID_MaBaoCao=@iID_MaBaoCao AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBaoCao", MaND);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            Connection.UpdateDatabase(cmd);

            SQL = "INSERT INTO KT_DanhMucThamSo_BaoCao  (iID_MaBaoCao,sTenBaoCao,sID_MaNguoiDungTao,sTen) values(@iID_MaBaoCao,@sTenBaoCao,@sID_MaNguoiDungTao ,@sTen)";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBaoCao", MaND);
            cmd.Parameters.AddWithValue("@sTenBaoCao", "rptThuNop_Thu01");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            cmd.Parameters.AddWithValue("@sTen", sMoTa);
            Connection.UpdateDatabase(cmd);

            ViewData["sMoTa"] = sMoTa;
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iLoaiBaoCao"] = iLoaiBaoCao;
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_Thu01.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan, String iLoaiBaoCao, String iID_MaDonVi)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_Thu01");

            LoadData(fr, MaND, iID_MaPhongBan, iLoaiBaoCao, iID_MaDonVi);
            String Nam = ReportModels.LayNamLamViec(MaND);
            String TenPB = "";
            if (iID_MaPhongBan != "-1")
                TenPB = " B - " + iID_MaPhongBan;
            String SQL = String.Format("SELECT sTen FROM  KT_DanhMucThamSo_BaoCao WHERE sTenBaoCao=@sTenBaoCao AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenBaoCao", "rptThuNop_Thu01");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            String sMoTa = Connection.GetValueString(cmd, "0");
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("sMoTa", sMoTa);
            fr.SetValue("TenPB", TenPB);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan, String iLoaiBaoCao, String iID_MaDonVi)
        {
            DataTable data = rptThuNop_Thu01(MaND, iID_MaPhongBan, iID_MaDonVi);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtLoaiHinh = new DataTable();
            if (iLoaiBaoCao == "1")
            {
                dtLoaiHinh = HamChung.SelectDistinct("LoaiHinh", data, "sLNS,sNG", "sLNS,sNG,sMoTa");
            }
            else
            {
                dtLoaiHinh = HamChung.SelectDistinct("LoaiHinh", data, "iID_MaDonVi", "iID_MaDonVi,sTenDonVi");
            }

            fr.AddTable("LoaiHinh", dtLoaiHinh);
            dtLoaiHinh.Dispose();
            data.Dispose();
        }

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan, String iLoaiBaoCao, String iID_MaDonVi)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iLoaiBaoCao == "1")
                sDuongDan = sFilePath;
            else sDuongDan = sFilePath1;

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iLoaiBaoCao, iID_MaDonVi);
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

        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan, String iLoaiBaoCao, String iID_MaDonVi)
        {
            String sDuongDan = "";
            if (iLoaiBaoCao == "1")
                sDuongDan = sFilePath;
            else sDuongDan = sFilePath1;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iLoaiBaoCao, iID_MaDonVi);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThuNop_DTNS_Na.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi, String MaND)
        {
            return Json(obj_DonVi(ParentID, iID_MaPhongBan, iID_MaDonVi, MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi, String MaND)
        {
            String input = "";
            DataTable dt = DonViModels.DanhSach_DonVi_ThuNop_PhongBan(iID_MaPhongBan, MaND);
            SelectOptionList slDonvi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return input;
        }

    }
}

