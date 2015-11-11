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
using System.Collections.Specialized;

namespace VIETTEL.Report_Controllers.CapPhat
{
    public class rptCapPhatThongTri_ChonChungTuController : Controller
    {
        //
        // GET: /rptCapPhat_ThongTri_78/


        public string sViewPath = "~/Report_Views/";
        private const String sFilePathMuc = "/Report_ExcelFrom/CapPhat/rptCapPhatThongTri_ChonChungTu_Muc.xls";
        private const String sFilePathTieuMuc = "/Report_ExcelFrom/CapPhat/rptCapPhatThongTri_ChonChungTu_TieuMuc.xls";
        private const String sFilePathNganh = "/Report_ExcelFrom/CapPhat/rptCapPhatThongTri_ChonChungTu_Nganh.xls";
        private const String sFilePathLNS = "/Report_ExcelFrom/CapPhat/rptCapPhatThongTri_ChonChungTu_LNS.xls";
        public static String NameFile = "";
        public ActionResult Index(String[] arrMaDonVi, String sLoaiThongTri, String iID_MaCapPhat)
        {

            ViewData["arrMaDonVi"] = arrMaDonVi;
            ViewData["sLoaiThongTri"] = sLoaiThongTri;
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            ViewData["ChiSo"] = 0;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri_ChonChungTu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID, String ChiSo)
        {
            String sMaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String iID_MaCapPhat = Convert.ToString(Request.Form["iID_MaCapPhat"]);
            String iID_MaLoaiCapPhat = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiCapPhat"]);
            String sLoaiThongTri = Convert.ToString(Request.Form[ParentID + "_sLoaiThongTri"]);
            ViewData["iID_MaLoaiCapPhat"] = iID_MaLoaiCapPhat;
            ViewData["sMaDonVi"] = sMaDonVi;
            ViewData["sLoaiThongTri"] = sLoaiThongTri;
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            ViewData["ChiSo"] = ChiSo;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri_ChonChungTu.aspx";
            return View(sViewPath + "ReportView.aspx");

        }

        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            DataTable dt = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            String dNgayCapPhat = Convert.ToString(dt.Rows[0]["dNgayCapPhat"]);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            String LoaiCapPhat = Convert.ToString(CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", Convert.ToString(dt.Rows[0]["iDM_MaLoaiCapPhat"]), "sGhiChu"));/* CommonFunction.LayTenDanhMuc(Convert.ToString(dt.Rows[0]["iDM_MaLoaiCapPhat"])); */
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, iID_MaDonVi, iID_MaCapPhat, iDM_MaLoaiCapPhat, sLoaiThongTri);
            fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_ThongTri_78Controller");
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("LoaiCapPhat", LoaiCapPhat);
            fr.SetValue("Thang", Convert.ToString(dt.Rows[0]["Thang"]));
            fr.SetValue("Nam", Convert.ToString(dt.Rows[0]["Nam"]));
            fr.SetValue("TenDonVi", DonViModels.Get_TenDonVi(iID_MaDonVi));
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("TongTienBangChu", TongTienBangChu(iID_MaDonVi, iID_MaCapPhat, iDM_MaLoaiCapPhat, sLoaiThongTri));
            fr.Run(Result);
            fr.Dispose();
            return Result;

        }

        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
        {
            DataTable data = ThongTri_78(iID_MaDonVi, iID_MaCapPhat, iDM_MaLoaiCapPhat, sLoaiThongTri);
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
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }

        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaCapPhat, iDM_MaLoaiCapPhat, sLoaiThongTri);
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

        public ActionResult ViewPDF(String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
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

            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaCapPhat, iDM_MaLoaiCapPhat, sLoaiThongTri);
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


        public DataTable ThongTri_78(String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
        {
            String DK = "", DKCapPhat = "";
            String MaND = User.Identity.Name;
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != Guid.Empty.ToString() && iDM_MaLoaiCapPhat != "00000000-0000-0000-0000-000000000000")
            {
                DK += " AND iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            String[] arrCapPhat = iID_MaCapPhat.Split(',');

            for (int i = 0; i < arrCapPhat.Length; i++)
            {
                DKCapPhat += "iID_MaCapPhat=@iID_MaCapPhat" + i;
                if (i < arrCapPhat.Length - 1)
                    DKCapPhat += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaCapPhat" + i, arrCapPhat[i]);
            }
            String strSelect = "";
            switch (sLoaiThongTri)
            {
                case "sNG":
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
            String SQL = String.Format("SELECT  {0} ", strSelect);
            SQL += " FROM CP_CapPhatChiTiet ";
            SQL += " WHERE iID_MaDonVi=@iID_MaDonVi AND iTrangThai=1 AND iID_MaTrangThaiDuyet=61 {0} {2} AND ({1})";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi";
            SQL += " ORDER By iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";

            SQL = String.Format(SQL, DK, DKCapPhat, ReportModels.DieuKien_NganSach(MaND));
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public String TongTienBangChu(String iID_MaDonVi, String iID_MaCapPhat, String iDM_MaLoaiCapPhat, String sLoaiThongTri)
        {
            String[] arrCapPhat = iID_MaCapPhat.Split(',');
            String DKCapPhat = "";
            SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrCapPhat.Length; i++)
            {
                DKCapPhat += "iID_MaCapPhat=@iID_MaCapPhat" + i;
                if (i < arrCapPhat.Length - 1)
                    DKCapPhat += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaCapPhat" + i, arrCapPhat[i]);
            }
            String SQL = String.Format("SELECT  {0}", "ISNULL(SUM(rTuChi+rHienVat),0) AS rTongSo");
            SQL += " FROM CP_CapPhatChiTiet ";
            SQL += " WHERE iID_MaDonVi=@iID_MaDonVi AND ({0})";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            SQL = String.Format(SQL, DKCapPhat);
            cmd.CommandText = SQL;
            long vR = Convert.ToInt64(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return CommonFunction.TienRaChu(vR);
        }

        public DataTable getListDonVi(String iDM_LoaiCapPhat, String iID_MaCapPhat)
        {
            String DK = "", DKCapPhat = "";
            String MaND = User.Identity.Name;
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iDM_LoaiCapPhat) == false && iDM_LoaiCapPhat != Guid.Empty.ToString() && iDM_LoaiCapPhat != "00000000-0000-0000-0000-000000000000")
            {
                DK += " AND iDM_MaLoaiCapPhat=@iDM_LoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_LoaiCapPhat", iDM_LoaiCapPhat);
            }
            String[] arrCapPhat = iID_MaCapPhat.Split(',');

            for (int i = 0; i < arrCapPhat.Length; i++)
            {
                DKCapPhat += "iID_MaCapPhat=@iID_MaCapPhat" + i;
                if (i < arrCapPhat.Length - 1)
                    DKCapPhat += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaCapPhat" + i, arrCapPhat[i]);
            }



            String SQL = String.Format(@"
                         SELECT DISTINCT iID_MaDonVi,sTenDonVi,iID_MaDonVi+'-'+sTenDonVi as TenHT FROM CP_CapPhatChiTiet WHERE 
                                                             iTrangThai=1 AND iID_MaTrangThaiDuyet=61 AND iID_MaDonVi IS NOT NULL AND iID_MaDonVi<>''
                                                             {0} {1} AND ({2})
						", DK, ReportModels.DieuKien_NganSach(MaND), DKCapPhat);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public DataTable getListChungTuCapPhat(String iDM_LoaiCapPhat)
        {
            String DK = "";
            String MaND = User.Identity.Name;
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iDM_LoaiCapPhat) == false && iDM_LoaiCapPhat != Guid.Empty.ToString() && iDM_LoaiCapPhat != "00000000-0000-0000-0000-000000000000")
            {
                DK += " AND iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_LoaiCapPhat);
            }
            String SQL = String.Format(@" SELECT CP_CapPhat.iID_MaCapPhat,TenHT FROM (
                         SELECT DISTINCT iID_MaCapPhat,sTienToChungTu,iSoCapPhat, sTienToChungTu+ CAST(iSoCapPhat as nvarchar) as TenHT FROM CP_CapPhat WHERE 
                                                             iTrangThai=1 AND iID_MaTrangThaiDuyet=61 {0} ) as CP_CapPhat
                        INNER JOIN (
                          SELECT DISTINCT iID_MaCapPhat FROM CP_CapPhatChiTiet WHERE iTrangThai=1 {1}  ) as     CP_CapPhatChiTiet ON CP_CapPhat.iID_MaCapPhat=CP_CapPhatChiTiet.iID_MaCapPhat
						 ", DK, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public class data
        {
            public string iID_MaDonVi { get; set; }
            public string iID_MaCapPhat { get; set; }
        }
        [HttpGet]
        public JsonResult ds_DonVi(String iDM_LoaiCapPhat, String iID_MaDonVi, String iID_MaCapPhat)
        {

            #region Chứng từ cấp phát
            DataTable dtChungTu = getListChungTuCapPhat(iDM_LoaiCapPhat);
            String input1 = "";
            StringBuilder stbChungTu = new StringBuilder();
            stbChungTu.Append("<div style=\"width: 90%; height: 400px; overflow: scroll; border:1px solid black;\">");
            stbChungTu.Append("<table class=\"mGrid\">");
            stbChungTu.Append("<tr>");
            stbChungTu.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"ChonallCT(this.checked)\"></td><td> Chọn tất cả CT </td>");

            String TenChungTu = "", MaCapPhat = "";
            String[] arrCapPhat = iID_MaCapPhat.Split(',');
            String _Checked1 = "checked=\"checked\"";
            for (int i = 1; i <= dtChungTu.Rows.Count; i++)
            {
                MaCapPhat = Convert.ToString(dtChungTu.Rows[i - 1]["iID_MaCapPhat"]);
                TenChungTu = Convert.ToString(dtChungTu.Rows[i - 1]["TenHT"]);
                _Checked1 = "";
                for (int j = 1; j <= arrCapPhat.Length; j++)
                {
                    if (MaCapPhat == arrCapPhat[j - 1])
                    {
                        _Checked1 = "checked=\"checked\"";
                        break;
                    }
                }

                input1 = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaCapPhat\" id=\"iID_MaCapPhat\" name=\"iID_MaCapPhat\" onchange=\"ChonDV()\" />", MaCapPhat, _Checked1);
                stbChungTu.Append("<tr>");
                stbChungTu.Append("<td style=\"width: 15%;\">");
                stbChungTu.Append(input1);
                stbChungTu.Append("</td>");
                stbChungTu.Append("<td>" + TenChungTu + "</td>");

                stbChungTu.Append("</tr>");
            }
            stbChungTu.Append("</table>");
            stbChungTu.Append("</div>");
            dtChungTu.Dispose();
            #endregion
            #region Donvi

            DataTable dtDonVi = getListDonVi(iDM_LoaiCapPhat, iID_MaCapPhat);
            String input = "";
            StringBuilder stbDonVi = new StringBuilder();
            stbDonVi.Append("<div style=\"width: 100%; height: 400px; overflow: scroll; border:1px solid black;\">");
            stbDonVi.Append("<table class=\"mGrid\">");
            stbDonVi.Append("<tr>");
            stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"ChonallDV(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

            String TenDonVi = "", MaDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dtDonVi.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                TenDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["TenHT"]);
                _Checked = "";
                for (int j = 1; j <= arrDonVi.Length; j++)
                {
                    if (MaDonVi == arrDonVi[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" onchange=\"ChonTo()\" />", MaDonVi, _Checked);
                stbDonVi.Append("<tr>");
                stbDonVi.Append("<td style=\"width: 15%;\">");
                stbDonVi.Append(input);
                stbDonVi.Append("</td>");
                stbDonVi.Append("<td>" + TenDonVi + "</td>");

                stbDonVi.Append("</tr>");
            }
            stbDonVi.Append("</table>");
            stbDonVi.Append("</div>");
            dtDonVi.Dispose();
            #endregion
            data _data = new data();
            _data.iID_MaDonVi = stbDonVi.ToString();
            _data.iID_MaCapPhat = stbChungTu.ToString();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
    }
}
