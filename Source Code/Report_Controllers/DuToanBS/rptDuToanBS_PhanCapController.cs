using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using VIETTEL.Models;
using System.Data.SqlClient;
using DomainModel;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using FlexCel.Render;
using System.IO;
using VIETTEL.Controllers;
using VIETTEL.Models.DuToanBS;


namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_PhanCapController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_PhanCap.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_PhanCap.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public static DataTable rptDuToanBS_PhanCap(String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            String DKDonVi = ""; String DKPhongBan = ""; String DK = "";
            SqlCommand cmd = new SqlCommand();

            
            //Điều kiện đơn vị
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++) {
                DK += "iID_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1) {
                    DK += " OR ";
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND ( " + DK + ") ";
            }

            //điều kiện LNS
            if (!String.IsNullOrEmpty(sLNS)) {
                DK += " AND sLNS IN ( " + sLNS + ") ";
            }

            //điều kiện phòng ban
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan); 
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            String Sql = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,
 sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
,SUM(rTuChi/@donvitinh) as rTuChi,SUM(rHienVat/@donvitinh) as rHienVat
 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
 HAVING SUM(rTuChi)<>0 
OR SUM(rHienVat) <>0
", DK, DKDonVi, DKPhongBan);
            cmd.CommandText = Sql;
            cmd.Parameters.AddWithValue("@donvitinh", "1000");
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);
            return dtAll;
        }

        //lấy dữ liệu
        public ActionResult EditSubmit(String ParentID)
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

            ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_PhanCap.aspx";
            return View(sViewPath + "ReportView.aspx");            
        }

        //Tạo báo cáo
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

        //đổ dữ liệu xuống báo cáo
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND) { 
            DataTable dtDonVi = new DataTable();
            DataTable data = new DataTable();
            DataRow r;

            dtDonVi = rptDuToanBS_PhanCap(iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan,MaND);
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
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
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
        //Lay mo ta
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }

        //hien thi dang PDF
        public ActionResult ViewPDF(String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            HamChung.Language();
            String sDuongDan = "";
            sDuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan, MaND);
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
            return null;
        }

        public clsExcelResult ExportToExcel(String iID_MaDonVi, String sLNS, String iID_MaDot, String iID_MaPhongBan, String MaND)
        {
            HamChung.Language();
            String sDuongDan = "";
            sDuongDan = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan, MaND);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptDuToanBS_PhanCap.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public JsonResult Ds_LNS(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi,String sLNS) {
            String MaND = User.Identity.Name;
            DataTable dt1 = DuToanBS_ReportModels.dtDonVi_LNS(iID_MaDot, iID_MaPhongBan, MaND, iID_MaDonVi);
            String ViewNam = "~/Views/DungChung/DonVi/LNS_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt1, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Ds_DonVi(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            DataTable dt = DuToanBS_ReportModels.dtDonVi_Dot1(iID_MaDot, iID_MaPhongBan);
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_Dot_DuToanBS.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}