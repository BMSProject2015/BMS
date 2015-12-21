using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using DomainModel;
using VIETTEL.Models;
using System.IO;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_ChiTieuNganSachController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String VIEW_PATH_DUTOANBS_CHITIEU_NS = "~/Report_Views/DuToanBS/rptDuToanBS_ChiTieuNganSach.aspx";
        private const String sFilePath_ChiTieuNganSach = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach.xls";

        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_DUTOANBS_CHITIEU_NS;

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
            String idDot = Request.Form[ParentID+"_iID_MaDot"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];

            //Gán giá trị vào ViewData
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaDot"] = idDot;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = VIEW_PATH_DUTOANBS_CHITIEU_NS;
            
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Xuất file PDF cấp phát thông tri
        /// </summary>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDot, String sLNS, String iID_MaDonVi, String iID_MaPhongBan)
        {
            HamChung.Language();
            
            String sDuongDan = sFilePath_ChiTieuNganSach;

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iID_MaDot, iID_MaDonVi, iID_MaPhongBan);
            
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
        /// <param name="path">Đường dẫn tới file excel</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_PhongBan");

            //Lấy dữ liệu chi tiết
            LoadData(fr, MaND, sLNS, iID_MaDot, iID_MaDonVi, iID_MaPhongBan);

            String Nam = ReportModels.LayNamLamViec(MaND);
            String sTenDonVi = "";
            String sTenPB = "";

            if (iID_MaPhongBan == "-1")
            {
                sTenPB = "Tất cả các phòng ban ";
            }
            else
                sTenPB = "B " + iID_MaPhongBan;

            if (iID_MaDonVi == "-1")
            {
                sTenDonVi = "Tất cả các đơn vị ";
            }
            else
                sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi, MaND);

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Dot", iID_MaDot);
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("sTenDonVi", sTenDonVi);

            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND">Mã Người Dùng</param>
        /// <param name="sLNS">Loại Ngân Sách</param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = new DataTable();
            
            data = DuToanBS_ReportModels.rptDuToanBS_ChiTieuNganSach(MaND, sLNS, iID_MaDot, iID_MaDonVi, iID_MaPhongBan);
            
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

            long TongTien = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["rTuChi"].ToString() != "")
                {
                    TongTien += Convert.ToInt64(data.Rows[i]["rTuChi"]);
                }
            }

            //In loại tiền bằng chữ
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien);

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
            fr.SetValue("Tien", Tien);

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
        /// Lấy danh sách đợt vị dựa vào đợt, phòng ban
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaDot">Mã Đợt</param>
        /// <param name="iID_MaPhongBan">Mã Phòng Ban</param>
        /// <param name="iID_MaDonVi">Mã Đơn Vị</param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";

            DataTable dt = DuToanBS_ReportModels.dtDonVi_Dot(iID_MaDot, iID_MaPhongBan, MaND);
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Lấy danh sách Loại Ngân Sách theo đợt,đơn vị,phòng ban
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
            String ViewNam = "~/Views/DungChung/DonVi/LNS_DanhSach_1.ascx";

            DataTable dt = DuToanBS_ReportModels.dtLNS_Dot(iID_MaDot, iID_MaPhongBan, iID_MaDonVi, MaND);
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt, ParentID);
            String strLNS = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            
            return Json(strLNS, JsonRequestBehavior.AllowGet);
        }  
    }
}