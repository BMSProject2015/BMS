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
using System.Collections.Specialized;
using System.Collections;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptQuyetToan_BieuKiemController : Controller
    {
        public static String SQL = "";
        public static SqlCommand cmd;
        public static DataTable dt;
        public static ArrayList data;
        public static String iID_MaDonVi, iID_MaChungTu, sKieuXem, iDonViTinh,sTenDonVi;

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_BieuKiem.xls";

        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_BieuKiem.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {

            iID_MaChungTu = Request.Form["QuyetToan" + "_iID_MaChungTu"];
            return RedirectToAction("ViewPDF");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport()
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
           
            String dNgayChungTu = "";
            dNgayChungTu =
                Convert.ToString(CommonFunction.LayTruong("QTA_ChungTu", "iID_MaChungTu", iID_MaChungTu, "dNgayChungTu"));
            if (!String.IsNullOrEmpty(dNgayChungTu))
            {
                dNgayChungTu = dNgayChungTu.Substring(0, 2) + "-" + dNgayChungTu.Substring(3, 2) + "-" +
                               dNgayChungTu.Substring(6, 4);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePath));
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr);
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_BieuKiem");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("dNgayChungTu", "Đợt ngày: " + dNgayChungTu);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1,MaND));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        public static DataTable DT_rptQuyetToan_BieuKiem()
        {
            

            SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,SUM(rTuChi) as rTuChi, SUM(rDonViDeNghi) as rDonViDeNghi
 FROM QTA_ChungTuChiTiet
 WHERE iTrangThai=1  AND iID_MaChungTu=@iID_MaChungTu
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
 ");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
                sTenDonVi = Convert.ToString(dt.Rows[0]["sTenDonVi"]);
            cmd.Dispose();
            return dt;
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr)
        {
            DataTable dtsTM;
            DataTable dtsM, dtsL, dtsLNS;
            DataTable data = DT_rptQuyetToan_BieuKiem();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS", "iID_MaDonVi,sTenDonVi,sLNS,sMoTa", "sLNS,sL");

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();

        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel()
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport();

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangKiem_QuyetToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF()
        {
            HamChung.Language();
            ExcelFile xls = CreateReport();
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
        }
    }
}

