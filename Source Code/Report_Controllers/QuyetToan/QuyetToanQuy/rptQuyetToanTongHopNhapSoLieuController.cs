using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using VIETTEL.Models;
using System.IO;
using VIETTEL.Models.QuyetToan;

namespace VIETTEL.Report_Controllers.QuyetToan.QuyetToanQuy
{
    public class rptQuyetToanTongHopNhapSoLieuController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String VIEW_PATH_QUYETTOAN_TONGHOP_SOLIEU = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_NhapSoLieu.aspx";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_NhapSoLieu.xls";
       
        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_QUYETTOAN_TONGHOP_SOLIEU;
            return View(sViewPath + "ReportView.aspx");
        }
        
        /// <summary>
        /// Lấy các giá trị từ Form gán vào ViewData
        /// </summary>
        /// <param name="ParentID"></param>
        /// HungPH: 2015/11/17
        public ActionResult FormSubmit(String ParentID)
        {
            //Lấy giá trị từ Form
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];

            //Gán giá trị vào ViewData
            ViewData["PageLoad"] = "1";
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = VIEW_PATH_QUYETTOAN_TONGHOP_SOLIEU;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        ///  Xuất file PDF quyết toán tổng hợp dữ liệu
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Mã năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";

            sDuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);
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

        /// <summary>
        /// Tạo file PDF xuất dữ liệu của quyết toán tổng hợp dữ liệu
        /// </summary>
        /// <param name="path">Đường dẫn tới file excel</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Mã năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_LNS_DonVi");

            //lấy dữ liệu chi tiết
            LoadData(fr, MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);
            String Nam = "";

            //lấy năm ngân sách
            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
            {
                NamNganSach = "NGÂN SÁCH NĂM TRƯỚC";
                Nam = Convert.ToString(Int32.Parse(ReportModels.LayNamLamViec(MaND)) - 1);
            }
            else if (iID_MaNamNganSach == "2")
            {
                NamNganSach = "NGÂN SÁCH NĂM NAY";
                Nam = ReportModels.LayNamLamViec(MaND);
            }
            else
            {
                NamNganSach = "NGÂN SÁCH TỔNG HỢP";
                Nam = Convert.ToString(Int32.Parse(ReportModels.LayNamLamViec(MaND)) - 1) + "," + ReportModels.LayNamLamViec(MaND);
            }

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);

            //lấy tên phòng ban
            if (iID_MaPhongBan != "-1")
            {
                String sTenPhongBan = "B" + iID_MaPhongBan;
                fr.SetValue("sTenPhongBan", sTenPhongBan);
            }
            else
            {
                String sTenPhongBan = "Tất cả các B";
                fr.SetValue("sTenPhongBan", sTenPhongBan);
            }

            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết của quyết toán tổng hợp dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaNamNganSach">Mã năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        private void LoadData(FlexCelReport fr, String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            DataTable data = QuyetToan_ReportModels.rptQuyetToanTongHopNhapSoLieu(MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi = HamChung.SelectDistinct("dtDonVi", data, "sTenDonVi", "sTenDonVi");
            dtDonVi.Columns.Add("STT");

            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                dtDonVi.Rows[i]["STT"] = i + 1;
            }

            fr.AddTable("dtDonVi", dtDonVi);
            data.Dispose();
            dtDonVi.Dispose();
        }        
    }
}