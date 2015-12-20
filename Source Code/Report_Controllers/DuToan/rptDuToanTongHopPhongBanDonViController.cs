using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using VIETTEL.Models;
using System.IO;
using VIETTEL.Models.DuToan;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanTongHopPhongBanDonViController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String VIEW_PATCH_DUTOAN_TONGHOP_PHONGBAN_DONVI = "~/Report_Views/DuToan/rptDuToan_TongHop_PhongBan_DonVi.aspx";
        private const String sFilePath_ChiTiet = "/Report_ExcelFrom/DuToan/rptDuToan_TongHop_PhongBan_DonVi_ChiTiet.xls";
        private const String sFilePath_TongHop = "/Report_ExcelFrom/DuToan/rptDuToan_TongHop_PhongBan_DonVi_TongHop.xls";

        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATCH_DUTOAN_TONGHOP_PHONGBAN_DONVI;

            return View(sViewPath + "ReportView.aspx");
        }
        
        /// <summary>
        /// Lấy các giá trị từ Form gán vào ViewData
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult FormSubmit(String ParentID)
        {   
            //Lấy giá trị từ Form
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_MaDot = Request.Form[ParentID + "_iID_MaDot"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String LoaiTongHop = Request.Form[ParentID + "_LoaiTongHop"];

            //Gán giá trị vào ViewData
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDot"] = iID_MaDot;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["path"] = VIEW_PATCH_DUTOAN_TONGHOP_PHONGBAN_DONVI;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Xuất file pdf báo cáo dự toán tổng hợp chọn phòng ban đơn vị
        /// </summary>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <param name="maDot">Mã đợt cấp phát</param>
        /// <param name="maPhongBan">Mã phòng ban</param>
        /// <param name="loaiBaoCao">Chọn báo cáo xuất ra là tổng hợp hay chi tiết</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String maND, String sLNS, String maDonVi, String maDot,
                    String maPhongBan, String loaiBaoCao)
            //public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot,
            //        String iID_MaPhongBan, String LoaiTongHop)
        {
            HamChung.Language();
            String sDuongDan = "";

            //Xuất báo cáo chi tiết
            if (loaiBaoCao == "ChiTiet")
            {
                sDuongDan = sFilePath_ChiTiet;
            }
            //Xuất báo cáo tổng hợp
            else
            {
                sDuongDan = sFilePath_TongHop;
            }

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), maND, sLNS, maDonVi, maDot, maPhongBan, loaiBaoCao);

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
        /// tạo file pdf báo cáo dự toán tổng hợp 
        /// </summary>
        /// <param name="path">Đường dẫn đến file excel</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Mã phòng ban</param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <param name="maDot">Mã đợt</param>
        /// <param name="maPhongBan">Mã phòng ban</param>
        /// <param name="loaiBaoCao">Loại báo cáo tổng hợp hay chi tiết</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String maND, String sLNS, String maDonVi,
                        String maDot, String maPhongBan, String loaiBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanTongHopPhongBanDonVi");

            //Lấy dữ liệu
            LoadData(fr, maND, sLNS, maDonVi, maDot, maPhongBan, loaiBaoCao);

            //Lấy năm làm việc của người dùng
            String Nam = ReportModels.LayNamLamViec(maND);

            //Lấy đơn vị của người dùng
            String sTenDonVi = DonViModels.Get_TenDonVi(maDonVi);

            //Lấy tên đơn vị, phòng ban
            if (loaiBaoCao == "ChiTiet")
            {
                fr.SetValue("sTenDonVi", sTenDonVi);
                fr.SetValue("sTenPhongBan", "");
            }
            else
            {
                fr.SetValue("sTenDonVi", "");
                if (maPhongBan != "-1")
                {
                    String sTenPhongBan = "Tổng hợp theo B" + maPhongBan;
                    fr.SetValue("sTenPhongBan", sTenPhongBan);
                }
                else
                {
                    String sTenPhongBan = "";
                    fr.SetValue("sTenPhongBan", sTenPhongBan);
                }
            }

            ViewData["iID_MaPhongBan"] = maPhongBan;
            fr.SetValue("Nam", Nam);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("madot", maDot);
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết của dự toán tổng hợp
        /// </summary>
        /// <param name="fr">Flecel report</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Mã loại ngân sách</param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <param name="maDot">Mã đợt dự toán</param>
        /// <param name="maPhongBan">Mã phòng ban</param>
        /// <param name="loaiBaoCao">Loại báo cáo tổng hợp hay chi tiết</param>
        /// HungPH: 2015/11/16
        private void LoadData(FlexCelReport fr, String maND, String sLNS, String maDonVi, String maDot, String maPhongBan, String loaiBaoCao)
        {
            DataTable dtDonVi = new DataTable();
            dtDonVi = DuToan_ReportModels.rptDuToanTongHopPhongBanDonVi(maND, sLNS, maDonVi, maDot, maPhongBan, loaiBaoCao);
            DataTable data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
            data.TableName = "ChiTiet";            
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK", "sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS", "sLNS,sMoTa", "sLNS,sL");

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("ChiTiet", data);
            fr.AddTable("dtDonVi", dtDonVi);

            dtDonVi.Dispose();
            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
        }

        /// <summary>
        /// Lấy danh sách đơn vị dựa vào loại ngân sách, đợt dự toán, phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <param name="maDot">Đợt dự toán</param>
        /// <param name="maPhongBan">Mã phòng ban</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String maDonVi, String maDot, String maPhongBan, String sLNS)
        {
            String MaND = User.Identity.Name;
            String sViewPatch = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";

            DataTable dt = DuToan_ReportModels.laydtPhongBan_LNS_DonVi(MaND, maDot, maPhongBan, sLNS);

            if (String.IsNullOrEmpty(maDonVi))
            {
                maDonVi = Guid.Empty.ToString();
            }

            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, maDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(sViewPatch, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
           
    }
 }
