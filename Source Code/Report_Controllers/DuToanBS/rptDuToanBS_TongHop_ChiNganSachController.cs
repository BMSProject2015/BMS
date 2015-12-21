using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using VIETTEL.Models;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_TongHop_ChiNganSachController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public string VIEW_PATH_TONGHOP_CHINGANSACH = "~/Report_Views/DuToanBS/rptDuToanBS_TongHop_ChiNganSach.aspx";
        private const String sFilePath_TongHop = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_TongHop_ChiNganSach.xls";
        private const String sFilePath_ChiTiet = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_TongHop_ChiNganSach_ChiTiet.xls";

        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_TONGHOP_CHINGANSACH;

            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Lấy các giá trị từ Form gán vào ViewData
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult FormSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaPhongBan = Request.Form["DuToanBS" + "_iID_MaPhongBan"];
            String iID_MaDonVi = Request.Form["sDV"];
            String iID_MaDotTu = Request.Form["DuToanBS" + "_iID_MaDotTu"];
            String iID_MaDotDen = Request.Form["DuToanBS" + "_iID_MaDotDen"];
            String LoaiTongHop = Request.Form["DuToanBS" + "_LoaiTongHop"];

            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaDotTu"] = iID_MaDotTu;
            ViewData["iID_MaDotDen"] = iID_MaDotDen;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["path"] = VIEW_PATH_TONGHOP_CHINGANSACH;

            return View(sViewPath + "ReportView.aspx");            
        }

        /// <summary>
        /// Xuất file PDF 
        /// </summary>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <param name="iID_MaDotTu">Mã Đợt Từ</param>
        /// <param name="iID_MaDotDen">Mã Đợt Đến</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="LoaiBaoCao">Loại Báo Cáo</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND, String LoaiBaoCao)
        {
            HamChung.Language();
            String sDuongDan = "";

            if (LoaiBaoCao == "ChiTiet")
            {
                sDuongDan = sFilePath_ChiTiet;
            }
            else
            {
                sDuongDan = sFilePath_TongHop;
            }
            
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND, LoaiBaoCao);
            
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
        /// Tạo file PDF xuất dữ liệu
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <param name="iID_MaDotTu">Mã Đợt</param>
        /// <param name="iID_MaDotDen">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="LoaiBaoCao">Loại Báo Cáo</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String sTenPB = "";
            String sTenDonVi = "";

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanBS_TongHop_ChiNganSach");

            LoadData(fr, iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND,LoaiBaoCao);
            
            if (iID_MaPhongBan == "-1")
            {
                sTenPB = "Tất cả các phòng ban ";
            }
            else
                sTenPB = "B " + iID_MaPhongBan;
            
            
            if (LoaiBaoCao == "ChiTiet")
            {
                sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            }
            
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("tungay", iID_MaDotTu);
            fr.SetValue("denngay", iID_MaDotDen);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("nam", ReportModels.LayNamLamViec(MaND));
            
            fr.Run(Result);
            return Result;      
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iID_MaDonVi">Mã Đơn VỊ</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <param name="iID_MaDotTu">Mã Đợt</param>
        /// <param name="iID_MaDotDen">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="LoaiBaoCao"></param>
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao) 
        { 
            DataTable dtDonVi = new DataTable();
            DataTable data = new DataTable();
            DataRow r;

            if (LoaiBaoCao == "ChiTiet")
            {
                data = DuToanBS_ReportModels.rptDuToanBS_TongHop_ChiNganSach(iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND, LoaiBaoCao);
            }
            else
            {
                dtDonVi = DuToanBS_ReportModels.rptDuToanBS_TongHop_ChiNganSach(iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND, LoaiBaoCao);
                data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
                fr.AddTable("dtDonVi", dtDonVi);
                dtDonVi.Dispose();
            }
            
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "NgayChungTu,sLNS1,sLNS3,sLNS5", "NgayChungTu,sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            
            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "NgayChungTu,sLNS1,sLNS3", "NgayChungTu,sLNS1,sLNS3,sMoTa");
            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "NgayChungTu,sLNS1", "NgayChungTu,sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            DataTable dtDot = HamChung.SelectDistinct("dtDot", dtsLNS1, "NgayChungTu", "NgayChungTu");
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            fr.AddTable("dtDot", dtDot);
            
            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();
            dtDot.Dispose();
        }

        /// <summary>
        /// lấy danh sách loại ngân sách theo đơn vị, đợt, phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <returns></returns>
        public JsonResult LayDanhSachLNS(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi, String sLNS)
        {
            String MaND = User.Identity.Name;
            String ViewNam = "~/Views/DungChung/DonVi/LNS_DanhSach.ascx";

            DataTable dt1 = DuToanBS_ReportModels.dtDonVi_LNS(iID_MaDot, iID_MaPhongBan, MaND, iID_MaDonVi);
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt1, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// lấy đơn vị theo đợt,phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaDotTu">Mã Đợt</param>
        /// <param name="iID_MaDotDen">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_Dot_DuToanBS.ascx";
            
            DataTable dt = DuToanBS_ReportModels.dtDonVi_Dot2(iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan);
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
    }
}