using System;
using System.Web.Mvc;
using System.Data;
using VIETTEL.Models;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using FlexCel.Render;
using System.IO;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_PhanCapController : Controller
    {
        private string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_PhanCap.xls";
        private const string VIEW_PATH_PHANCAP = "~/Report_Views/DuToanBS/rptDuToanBS_PhanCap.aspx";

        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = VIEW_PATH_PHANCAP;
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
            String iID_MaDot = Request.Form["DuToanBS" + "_iID_MaDot"];

            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaDot"] = iID_MaDot;

            ViewData["path"] = VIEW_PATH_PHANCAP;
            return View(sViewPath + "ReportView.aspx");            
        }

        /// <summary>
        /// Xuất file PDF 
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDot"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            HamChung.Language();

            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan, MaND);

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
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDot"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanBS_PhanCap");

            LoadData(fr, iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan, MaND);

            String sTenPB = "";
            if (iID_MaPhongBan == "-1")
            {
                sTenPB = "Tất cả các phòng ban ";
            }
            else
                sTenPB = "B " + iID_MaPhongBan;
            
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("Dot", iID_MaDot);
            fr.Run(Result);
            return Result;      
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDot"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="MaND"></param>
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND) { 
            DataTable dtDonVi = new DataTable();
            DataTable data = new DataTable();
            DataRow r;

            dtDonVi = DuToanBS_ReportModels.rptDuToanBS_PhanCap(iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan,MaND);

            data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
            fr.AddTable("dtDonVi", dtDonVi);
            dtDonVi.Dispose();

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
            DataTable dtPhongBan = HamChung.SelectDistinct("dtPhongBan", dtDonVi, "iID_MaPhongBan", "iID_MaPhongBan,sTenPhongBan");
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            fr.AddTable("dtPhongBan",dtPhongBan);
            

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();
            dtPhongBan.Dispose();
          

        }

        /// <summary>
        /// Lấy danh sách loại ngân sách theo đợt, phòng ban, đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaDot"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public JsonResult LayDanhSachLNS(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi,String sLNS) 
        {
            String MaND = User.Identity.Name;
            String viewPath = "~/Views/DungChung/DonVi/LNS_DanhSach.ascx";

            DataTable dt1 = DuToanBS_ReportModels.dtDonVi_LNS(iID_MaDot, iID_MaPhongBan, MaND, iID_MaDonVi);
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt1, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(viewPath, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy dnah sách đơn vị theo phòng ban, đợt
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaDot"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            String viewPath = "~/Views/DungChung/DonVi/DonVi_Dot_DuToanBS.ascx";

            DataTable dt = DuToanBS_ReportModels.dtDonVi_Dot1(iID_MaDot, iID_MaPhongBan);
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(viewPath, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}