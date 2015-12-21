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
using VIETTEL.Models.DuToan;

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_NganSachBaoDamController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String VIEW_PATH_DUTOANBS_NGANSACH_BAODAM = "~/Report_Views/DuToanBS/rptDuToanBS_NganSachBaoDam.aspx";
        private const String sFilePath_TongHop = "/Report_ExcelFrom/DuToanBS/rptDuToan_1040100_BoSung_TongHop.xls";

        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_DUTOANBS_NGANSACH_BAODAM;

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
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_Dot = Request.Form[ParentID + "_iID_Dot"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String LoaiTongHop = Request.Form[ParentID + "_LoaiTongHop"];

            //Gán giá trị vào ViewData
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_Dot"] = iID_Dot;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["path"] = VIEW_PATH_DUTOANBS_NGANSACH_BAODAM;

            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        ///  Xuất file PDF
        /// </summary>
        /// <param name="MaND">Mã Người DÙng</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_Dot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="LoaiTongHop">Loại Tổng Hợp</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_Dot, String iID_MaPhongBan, String LoaiTongHop)
        {
            HamChung.Language();

            String sDuongDan = sFilePath_TongHop;

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaDonVi, iID_Dot, iID_MaPhongBan, LoaiTongHop);
            
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
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_Dot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="LoaiTongHop">Loại Tổng Hợp</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_Dot, String iID_MaPhongBan, String LoaiTongHop) 
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanBS_NganSachBaoDam");

            LoadData(fr, MaND, iID_MaDonVi, iID_Dot, iID_MaPhongBan, LoaiTongHop);

            String Nam = ReportModels.LayNamLamViec(MaND);
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            //lay ten phong ban
            if (iID_MaPhongBan != "-1")
            {
                String sTenPhongBan = "B" + iID_MaPhongBan;
                fr.SetValue("sTenPhongBan", sTenPhongBan);
            }
            else
            {
                String sTenPhongBan = "";
                fr.SetValue("sTenPhongBan", sTenPhongBan);
            }
            
            fr.SetValue("Nam", Nam);

            if (LoaiTongHop == "ChiTiet")
            {
                 fr.SetValue("sTenDonVi", sTenDonVi);
            }
            else
            {
                 fr.SetValue("sTenDonVi", "");
            }
             
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Lấy dự liệu chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_Dot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="LoaiTongHop">Loại Tổng Hợp</param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_Dot, String iID_MaPhongBan, String LoaiTongHop) 
        {
            DataTable dtDonVi = new DataTable();
            DataTable data = new DataTable();

            dtDonVi = DuToanBS_ReportModels.rptDuToanBS_NganSachBaoDam(MaND, iID_MaDonVi, iID_Dot, iID_MaPhongBan, LoaiTongHop);
           
            data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            fr.AddTable("dtDonVi", dtDonVi);
            dtDonVi.Dispose();
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK", "sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS", "sLNS,sMoTa", "sLNS,sL");
            
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
        /// Lấy danh sách đơn vị theo đợt và phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_Dot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String iID_MaDonVi, String iID_Dot, String iID_MaPhongBan) 
        {
            String MaND = User.Identity.Name;
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";

            DataTable dt = DuToanBS_ReportModels.getdtPhongBan_DonVi(iID_Dot, iID_MaPhongBan);

            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
    }
}