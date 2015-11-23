using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.CapPhat
{
    public class rptCapPhat_ThongTri_78Controller : Controller
    {
        //
        // GET: /rptCapPhat_ThongTri_78/


        public string sViewPath = "~/Report_Views/";
        private const String sFilePathMuc = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_78_Muc.xls";
        private const String sFilePathTieuMuc = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_78_TieuMuc.xls";
        private const String sFilePathNganh = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_78_Nganh.xls";
        private const String sFilePathLNS = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_78_LNS.xls";
        public static String NameFile = "";
        public ActionResult Index(String[] arrMaDonVi, String sLoaiThongTri, String iID_MaCapPhat)
        {
            ViewData["arrMaDonVi"] = arrMaDonVi;
            ViewData["sLoaiThongTri"] = sLoaiThongTri;
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            ViewData["ChiSo"] = 0;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri_78.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID, String iID_MaCapPhat)
        {

            String sMaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);          
            String sLoaiThongTri = Convert.ToString(Request.Form[ParentID + "_sLoaiThongTri"]);
            String sLyDo = Convert.ToString(Request.Form[ParentID + "_sLyDo"]);
            String SQL = "UPDATE CP_CapPhat SET sLyDo=@sLyDo WHERE iID_MaCapPhat=@iID_MaCapPhat";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.Parameters.AddWithValue("@sLyDo", sLyDo);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            if(String.IsNullOrEmpty(sMaDonVi))
            {
                ViewData["MaDotNganSach"] = "";
                ViewData["sLNS"] = "";
                return View("~/Views/CapPhat/ChungTu/CapPhat_ChungTu_Index.aspx");
            }

            ViewData["sMaDonVi"] = sMaDonVi;
            ViewData["sLoaiThongTri"] = sLoaiThongTri;
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            ViewData["ChiSo"] = 0;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri_78.aspx";
            
            return View(sViewPath + "ReportView.aspx");
           
        }



        public ActionResult EditSubmit_Next(String ParentID,String iID_MaCapPhat, String sLoaiThongTri, int ChiSo)
        {
            String sMaDonVi = Convert.ToString(Request.Form[ParentID + "_sMaDonVi"]);

            ViewData["sMaDonVi"] = sMaDonVi;
            ViewData["sLoaiThongTri"] = sLoaiThongTri;
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            ViewData["ChiSo"] = ChiSo;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri_78.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ExcelFile CreateReport(String path,String iID_MaDonVi, String iID_MaCapPhat,String sLoaiThongTri)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

                DataTable dt=  CapPhat_ChungTuModels.LayChungTuCapPhat(iID_MaCapPhat);
                String dNgayCapPhat = Convert.ToString(dt.Rows[0]["dNgayCapPhat"]);
                String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
                String LoaiCapPhat = Convert.ToString(CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", Convert.ToString(dt.Rows[0]["iDM_MaLoaiCapPhat"]), "sGhiChu"));/* CommonFunction.LayTenDanhMuc(Convert.ToString(dt.Rows[0]["iDM_MaLoaiCapPhat"])); */
                FlexCelReport fr = new FlexCelReport();           
                LoadData(fr, iID_MaDonVi, iID_MaCapPhat, sLoaiThongTri);
                fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_ThongTri_78Controller");
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("LoaiCapPhat", LoaiCapPhat);
                fr.SetValue("Thang", Convert.ToString(dt.Rows[0]["Thang"]));
                fr.SetValue("Nam", Convert.ToString(dt.Rows[0]["Nam"]));
                fr.SetValue("TenDonVi",DonViModels.Get_TenDonVi(iID_MaDonVi));
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("TongTienBangChu", TongTienBangChu(iID_MaDonVi, iID_MaCapPhat, sLoaiThongTri));
                fr.Run(Result);
                fr.Dispose();
                return Result;
           
        }

        private void LoadData(FlexCelReport fr,String iID_MaDonVi, String iID_MaCapPhat,String sLoaiThongTri)
        {
            DataTable data = ThongTri_78(iID_MaDonVi, iID_MaCapPhat, sLoaiThongTri);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            if (sLoaiThongTri == "sNG")
            {
                DataTable dtLoaiK = HamChung.SelectDistinct("LoaiK", dtMuc, "NguonNS,sLNS,sL,sK", "NguonNS,sLNS,sL,sK");
                fr.AddTable("LoaiK", dtLoaiK);
                dtLoaiK.Dispose();
            }

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            String sLyDo = "";
            sLyDo = Convert.ToString(CommonFunction.LayTruong("CP_CapPhat", "iID_MaCapPhat", iID_MaCapPhat, "sLyDo"));
            String SQL = "SELECT sLyDo FROM CP_CapPhat WHERE iID_MaCapPhat=@iID_MaCapPhat";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            DataTable dtLyDo = Connection.GetDataTable(cmd);
            cmd.Dispose();

            fr.AddTable("LyDo", dtLyDo);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
        {
            HamChung.Language();
            String DuongDan = "";
            
                DuongDan = sFilePathNganh;
            
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaCapPhat, sLoaiThongTri);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }

        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat,String sLoaiThongTri)
        {
            HamChung.Language();
            String DuongDan = "";
            switch (sLoaiThongTri)
            {
                case "sNG":
                    DuongDan = sFilePathNganh;
                    break;
                case "sTM":
                    DuongDan = sFilePathTieuMuc;
                    break;
                case "sM":
                    DuongDan = sFilePathMuc;
                    break;
                case "sLNS":
                    DuongDan = sFilePathLNS;
                    break;

            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaCapPhat, sLoaiThongTri);            
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CapPhat_ThongTri_78.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        
        public ActionResult ViewPDF(String iID_MaDonVi, String iID_MaCapPhat, String sLoaiThongTri)
        {
            HamChung.Language();
            String DuongDan = "";
            switch (sLoaiThongTri)
            {
                case "sNG":
                    DuongDan = sFilePathNganh;
                    break;
                case "sTM":
                    DuongDan = sFilePathTieuMuc;
                    break;
                case "sM":
                    DuongDan = sFilePathMuc;
                    break;
                case "sLNS":
                    DuongDan = sFilePathLNS;
                    break;

            }
          
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaCapPhat, sLoaiThongTri);
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
     
      
        public DataTable ThongTri_78(String iID_MaDonVi, String iID_MaCapPhat, String sLoaiThongTri)
        {
            String DK=sLoaiThongTri;
            String strSelect = "";
            switch (sLoaiThongTri)
            {
                case"sNG":
                    strSelect = "SUBSTRING(sLNS,1,1) as NguonNs,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,SUM(rTuChi+rHienVat) AS rTongSo";
                    break;
                case "sTM":
                    strSelect = "SUBSTRING(sLNS,1,1) as NguonNs,sLNS,sL,sK,sM,sTM,sTTM='',sNG='',sMoTa='',iID_MaDonVi,SUM(rTuChi+rHienVat) AS rTongSo";
                    break;
                case "sM":
                    strSelect = "SUBSTRING(sLNS,1,1) as NguonNs,sLNS,sL,sK,sM,sTM='',sTTM='',sNG='',sMoTa='',iID_MaDonVi,SUM(rTuChi+rHienVat) AS rTongSo";
                    break;
                case "sLNS":
                    strSelect = "SUBSTRING(sLNS,1,1) as NguonNs,sLNS,sL='',sK='',sM='',sTM='',sTTM='',sNG='',sMoTa='',iID_MaDonVi,SUM(rTuChi+rHienVat) AS rTongSo";
                    break;
            }
            String SQL = String.Format("SELECT  {0} ",strSelect);            
            SQL += " FROM CP_CapPhatChiTiet ";
            SQL += " WHERE iID_MaDonVi=@iID_MaDonVi AND iID_MaCapPhat=@iID_MaCapPhat";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi";
            SQL += " ORDER By iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public String TongTienBangChu(String iID_MaDonVi, String iID_MaCapPhat, String sLoaiThongTri)
        {
            String SQL = String.Format("SELECT  {0}", "ISNULL(SUM(rTuChi+rHienVat),0) AS rTongSo");
            SQL += " FROM CP_CapPhatChiTiet ";
            SQL += " WHERE iID_MaDonVi=@iID_MaDonVi AND iID_MaCapPhat=@iID_MaCapPhat";           
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            long vR = Convert.ToInt64(Connection.GetValue(cmd,0));
            cmd.Dispose();            
            return CommonFunction.TienRaChu(vR);
        }
       
    }
}
