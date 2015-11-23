using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using VIETTEL.Models;
using System.IO;

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_TongHop_Nam_QuyController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String VIEW_PATH_QUYETTOAN_TONGHOP_NAM_QUY = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_Nam_Quy.aspx";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_Nam_Quy.xls";
        private const String sFilePath_TongHop = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_Nam_Quy_TongHop.xls";
        private const String sFilePath_TongHop_denLNS = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_Nam_Quy_TongHop_denLNS.xls";
        private const String sFilePath_TongHop_denM = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_Nam_Quy_TongHop_denM.xls";
        private const String sFilePath_TongHop_denTM = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_Nam_Quy_TongHop_denTM.xls";
        
        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_QUYETTOAN_TONGHOP_NAM_QUY;
            return View(sViewPath + "ReportView.aspx");
        }


        /// <summary>
        /// Lấy các giá trị từ Form gán vào ViewData
        /// </summary>
        /// HungPH: 2015/11/18
        public ActionResult EditSubmit(String ParentID)
        {
            //Lấy giá trị từ Form
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iThang_Quy = Request.Form["QuyetToanNganSach" + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form["QuyetToanNganSach" + "_iID_MaNamNganSach"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String LoaiTongHop = Request.Form["QuyetToanNganSach" + "_LoaiTongHop"];
            String iID_TuyChon = Request.Form["QuyetToanNganSach" + "_iID_TuyChon"];

            //Gán giá trị vào ViewData
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["iID_TuyChon"] = iID_TuyChon;
            ViewData["path"] = VIEW_PATH_QUYETTOAN_TONGHOP_NAM_QUY;

            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        ///  Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_TuyChon"></param>
        /// <returns></returns>
        /// HungPH: 18/11/2015
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaNamNganSach, String LoaiBaoCao, String iID_MaPhongBan, String iID_TuyChon)
        {
            HamChung.Language();
            String sDuongDan = "";

            if (LoaiBaoCao == "ChiTiet")
            {
                sDuongDan = sFilePath;
            }
            else
            {
                if (iID_TuyChon == "1")
                {
                    sDuongDan = sFilePath_TongHop;
                }
                else if (iID_TuyChon == "2")
                {
                    sDuongDan = sFilePath_TongHop_denLNS;
                }
                else if (iID_TuyChon == "3")
                {
                    sDuongDan = sFilePath_TongHop_denM;
                }
                else if (iID_TuyChon == "4")
                {
                    sDuongDan = sFilePath_TongHop_denTM;
                }
            }

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, LoaiBaoCao, iID_MaPhongBan);
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
        /// Tạo file PFD
        /// </summary>
        /// <param name="path"></param>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
        /// HungPH: 18/11/2015
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String LoaiBaoCao, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_TongHop_Nam_Quy");

            LoadData(fr, MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, LoaiBaoCao, iID_MaPhongBan);
            String Nam = ReportModels.LayNamLamViec(MaND);

            //lay ten nam ngan sach
            String NamNganSach = "";

            if (iID_MaNamNganSach == "1")
                NamNganSach = "QUYẾT TOÁN NẮM TRƯỚC";
            else if (iID_MaNamNganSach == "2")
                NamNganSach = "QUYẾT TOÁN NĂM NAY";
            else
            {
                NamNganSach = "TỔNG HỢP";
            }

            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            String sTenPhongBan = "";

            if (iID_MaPhongBan != "-1")
            {
                sTenPhongBan = "B" + iID_MaPhongBan;
            }

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("sTenPhongBan", sTenPhongBan);
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// HungPH: 18/11/2015
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String LoaiBaoCao, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = new DataTable();
            DataTable dtDonVi = new DataTable();

            if (LoaiBaoCao == "ChiTiet")
            {
                data = QuyetToan_ReportModels.rptQuyetToan_TongHop_Nam_Quy(MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, LoaiBaoCao, iID_MaPhongBan);
            }
            else
            {
                dtDonVi = QuyetToan_ReportModels.rptQuyetToan_TongHop_Nam_Quy(MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, LoaiBaoCao, iID_MaPhongBan);
                data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
                fr.AddTable("dtDonVi", dtDonVi);
                dtDonVi.Dispose();
            }

            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS1,sLNS3,sLNS5,sLNS", "sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "sLNS1,sLNS3,sLNS5", "sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

        }
        
        /// <summary>
        /// Lấy danh sách LNS từ quý, đơn vị, phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Thang_Quy">Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// HungPH: 2015/11/17
        public JsonResult LayDanhSachLNS(String ParentID, String Thang_Quy, String iID_MaDonVi, String sLNS, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            String sViewPath = "~/Views/DungChung/DonVi/LNS_DanhSach.ascx";

            DataTable dt = QuyetToan_ReportModels.dtDonVi_LNS_PhongBan(Thang_Quy, iID_MaNamNganSach, MaND, iID_MaDonVi, iID_MaPhongBan);

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }

            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(sViewPath, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

