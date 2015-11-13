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
    public class rptQuyetToan_BieuKiemAllController : Controller
    {
       
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_BieuKiemAll.xls";

        public static String iID_MaPhongBan, iID_MaDonVi, iThang_Quy, iID_MaNamNganSach, MaND;
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_BieuKiemAll.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_BieuKiemAll()
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaNamNganSach) && iID_MaNamNganSach != "0")
            {
                DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String SQL =
                String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,
sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,SUM(rTuChi) as rTuChi, SUM(rDonViDeNghi) as rDonViDeNghi
 FROM QTA_ChungTuChiTiet
 WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy {0} {1} {2} 
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan
HAVING SUM(rTuChi)<>0 OR SUM(rDonViDeNghi)<>0
 ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan", DKDonVi, DKPhongBan, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
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
        public ActionResult EditSubmit(String ParentID)
        {
            iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_BieuKiemAll.aspx";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport()
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePath));
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_BieuKiemAll");
            LoadData(fr);
            String Nam = ReportModels.LayNamLamViec(MaND);
            String TenPB = "";
            if (iID_MaPhongBan != "-1")
                TenPB = " B - " + iID_MaPhongBan;
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1,MaND));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2, MaND));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
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
        private void LoadData(FlexCelReport fr)
        {
            DataTable data = rptQuyetToan_BieuKiemAll();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
           
            DataTable dtsTM=null,dtsM=null,dtsL=null,dtsLNS=null,dtsLNS5=null,dtsLNS3=null,dtsLNS1=null,dtDonVi=null,dtPhongBan=null;
            ReportModels.getDataTable9Cap(data, ref dtsTM, ref dtsM, ref dtsL, ref dtsLNS, ref dtsLNS5, ref dtsLNS3, ref dtsLNS1, ref dtDonVi, ref dtPhongBan);
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
            dtDonVi.Dispose();
            dtPhongBan.Dispose();
        }
       

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String viID_MaPhongBan, String viID_MaDonVi, String viThang_Quy, String viID_MaNamNganSach)
        {
            HamChung.Language();
            MaND = User.Identity.Name;
            iID_MaPhongBan = viID_MaPhongBan;
            iID_MaDonVi = viID_MaDonVi;
            iThang_Quy = viThang_Quy;
            iID_MaNamNganSach = viID_MaNamNganSach;
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

        public clsExcelResult ExportToExcel(String viID_MaPhongBan, String viID_MaDonVi, String viThang_Quy, String viID_MaNamNganSach)
        {
            HamChung.Language();
            MaND = User.Identity.Name;
            iID_MaPhongBan = viID_MaPhongBan;
            iID_MaDonVi = viID_MaDonVi;
            iThang_Quy = viThang_Quy;
            iID_MaNamNganSach = viID_MaNamNganSach;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport();

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_BieuKiem.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String oiID_MaPhongBan, String oiThang_Quy, String oiID_MaNamNganSach, String oiID_MaDonVi, String oMaND)
        {
            string s = User.Identity.Name;
            return Json(obj_DonVi(ParentID, oiID_MaPhongBan, oiThang_Quy, oiID_MaNamNganSach, oiID_MaDonVi, oMaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String oiID_MaPhongBan,String oiThang_Quy,String oiID_MaNamNganSach, String oiID_MaDonVi,String oMaND)
        {
            String input = "";
            DataTable dt = DonViModels.DanhSach_DonVi_QuyetToan_PhongBan(oiID_MaPhongBan, oiThang_Quy, oiID_MaNamNganSach,oMaND);
            SelectOptionList slDonvi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return input;
        }
    }
}

