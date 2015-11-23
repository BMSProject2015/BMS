using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Controls;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using VIETTEL.Models;

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_PhongBanController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public string VIEW_PATH_QUYETTOAN_PHONGBAN = "~/Report_Views/QuyetToan/rptQuyetToan_PhongBan.aspx";
        private const String sFilePath_ChiTiet = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_PhongBan.xls";

        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_QUYETTOAN_PHONGBAN;

            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            //Lấy giá trị từ Form
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["QuyetToanNganSach" + "_iID_MaDonVi"];
            String iThang_Quy = Request.Form["QuyetToanNganSach" + "_iThang_Quy"];
            String iID_MaPhongBan = Request.Form["QuyetToanNganSach" + "_iID_MaPhongBan"];

            //Gán giá trị vào ViewData
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = VIEW_PATH_QUYETTOAN_PHONGBAN;

            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
        /// QuyDQ
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";

            sDuongDan = sFilePath_ChiTiet;

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);
           
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
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();

            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_PhongBan");

            LoadData(fr, MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);
            
            String Nam = ReportModels.LayNamLamViec(MaND);

            if (iThang_Quy == "-1")
            {
                iThang_Quy = "Tất cả các Quý ";
            }

            if (iThang_Quy == "5")
            {
                iThang_Quy = "Bổ Sung ";
            }

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="sLNS">Loại Ngân Sach</param>
        /// <param name="iThang_Quy">Tháng Quý</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = new DataTable();
            
            data = QuyetToan_ReportModels.rptQuyetToan_PhongBan(MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);
           
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sMoTa");
           
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS5"]));
            }

            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "iID_MaPhongBan,iID_MaDonVi,sLNS1", "sTenPhongBan,sTenDonVi,iID_MaPhongBan,iID_MaDonVi,sLNS1,sMoTa");
            
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            DataTable dtDonVi = HamChung.SelectDistinct("dtDonVi", dtsLNS1, "iID_MaPhongBan,iID_MaDonVi", "iID_MaPhongBan,sTenPhongBan,iID_MaDonVi,sTenDonVi");
            DataTable dtPhongBan = HamChung.SelectDistinct("dtPhongBan", dtDonVi, "iID_MaPhongBan", "iID_MaPhongBan,sTenPhongBan"); 
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            fr.AddTable("dtDonVi", dtDonVi);
            fr.AddTable("dtPhongBan", dtPhongBan);

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
        /// lấy danh sách đơn vị theo tháng quý,đơn vị,phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang_Quy">Tháng Quý</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan, String MaND)
        {
            return Json(obj_DonVi(ParentID, iThang_Quy, iID_MaDonVi, iID_MaPhongBan, MaND), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonVi(String ParentID, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan, String MaND)
        {
            String input = "";

            DataTable dt = DonViModels.DanhSach_DonVi_QuyetToan_PhongBan(iID_MaPhongBan, MaND);
            SelectOptionList slDonvi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");

            return input;
        }
       
    }
}

