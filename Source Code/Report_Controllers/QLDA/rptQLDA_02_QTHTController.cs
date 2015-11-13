using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using DomainModel;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using VIETTEL.Controllers;
using VIETTEL.Models;

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_02_QTHTController : Controller
    {
        //
        // GET: /rptQLDA_01DT/


        private const String sFilePath = "~/Report_ExcelFrom/QLDA/rptQLDA_02_QTHT.xls";
        private const String sFilePathDuAn = "~/Report_ExcelFrom/QLDA/rptQLDA_02_QTHT_DA.xls";
        private const String sFilePathCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_02_QTHT_CT.xls";
        private const String sFilePathDuAnTP = "~/Report_ExcelFrom/QLDA/rptQLDA_02_QTHT_DATP.xls";
        private const String sFilePathHaMucCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_02_QTHT_HMCT.xls";
        private const String sFilePathHaMucChiTiet = "~/Report_ExcelFrom/QLDA/rptQLDA_02_QTHT.xls";
        public static String NameFile = "";
        public string sViewPath = "~/Report_Views/";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02_QTHT.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["LoadPage"] = 0;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDeAn = Convert.ToString(Request.Form["iID_MaDanhMucDuAn"]);
            String iCapTongHop = Convert.ToString(Request.Form[ParentID + "_iCapTongHop"]);
            String iLoaiNgoaiTe = Convert.ToString(Request.Form["iID_MaNgoaiTe"]);
            String NamLamViec = Convert.ToString(Request.Form["NamLamViec"]);
            //String dNgayBaoCao = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayBaoCao"];
            //if (String.IsNullOrEmpty(dNgayBaoCao))
            //{
            //    int i;
            //    var arrLoi = new NameValueCollection();
            //    arrLoi.Add(ParentID + "err_dNgayBaoCao", "Bạn chưa nhập ngày báo cáo!");
            //    if (arrLoi.Count > 0)
            //    {
            //        for (i = 0; i <= arrLoi.Count - 1; i++)
            //        {
            //            ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
            //        }
            //        ViewData["LoadPage"] = 0;
            //    }
            //}
            //else
            //{
            //    ViewData["LoadPage"] = 1;
            //}

            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02_QTHT.aspx";
            ViewData["dsDeAn"] = iID_MaDeAn;
            ViewData["iCapTongHop"] = iCapTongHop;
            ViewData["iID_MaNgoaiTe"] = iLoaiNgoaiTe;
            ViewData["NamLamViec"] = NamLamViec;
            //ViewData["dNgayBaoCao"] = dNgayBaoCao;
            ViewData["LoadPage"] = 1;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String NamLamViec, String iID_MaNgoaiTe, String dsDeAn,
                                      String iCapTongHop)
        {
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            var Result = new XlsFile(true);
            String path = String.Empty;
            var fr = new FlexCelReport();
            switch (iCapTongHop)
            {
                case CapTongHop.DuAn:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathDuAn);
                        Result.Open(path);
                        break;
                    }
                case CapTongHop.DuAnTP:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathDuAnTP);
                        Result.Open(path);
                        break;
                    }
                case CapTongHop.CongTrinh:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathCongTrinh);
                        Result.Open(path);
                        break;
                    }

                case CapTongHop.HangMucCongTrinh:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathHaMucCongTrinh);
                        Result.Open(path);
                        break;
                    }
                case CapTongHop.HangMucChiTiet:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathHaMucChiTiet);
                        Result.Open(path);
                        break;
                    }
                default:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathHaMucChiTiet);
                        Result.Open(path);
                        break;
                    }
            }
            //Set những giá trị chung cho các loại báo cáo
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_02_QTHT");
            LoadData(fr, NamLamViec, iID_MaNgoaiTe, dsDeAn);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3));
            String strDonViTinh = string.Empty;
            switch (iID_MaNgoaiTe)
            {
                case DonViTinh.VND:
                    strDonViTinh = "Triệu đồng ";
                    break;
                case DonViTinh.USD:
                    strDonViTinh = "USD";
                    break;
                case DonViTinh.EUR:
                    strDonViTinh = "EUR";
                    break;
                case DonViTinh.JPY:
                    strDonViTinh = "JPY";
                    break;
                default :
                    strDonViTinh = "Triệu đồng ";
                    break;
            }
            fr.SetValue("DonViTinh", strDonViTinh);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Hàm hiển thị dữ liệu báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec, String iLoaiNgoaiTe,
                              String dsDeAn)
        {
            DataTable data = dtDuToanNam(NamLamViec, dsDeAn, iLoaiNgoaiTe);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            //if (data == null || data.Rows.Count == 0)
            //{
            //   return;
            //}
            //Lấy danh sách Hạ mục công trình
            fr.AddTable("ChiTiet", data);
            data.Columns.Add("sTienDo", typeof(String));
            DataTable dtHmct = HamChung.SelectDistinct_QLDA("dtHmct", data, "iID_MaNguonNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh",
                                                            "iID_MaNguonNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo",
                                                            "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            dtHmct.TableName = "HangMucCongTrinh";
            fr.AddTable("HangMucCongTrinh", dtHmct);

            DataTable CongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHmct, "iID_MaNguonNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh",
                                                               "iID_MaNguonNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo",
                                                               "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            CongTrinh.TableName = "CongTrinh";
            fr.AddTable("CongTrinh", CongTrinh);

            DataTable DuAnTP = HamChung.SelectDistinct_QLDA("DuAnTP", CongTrinh, "iID_MaNguonNganSach,sDeAn,sDuAn,sDuAnThanhPhan",
                                                            "iID_MaNguonNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo",
                                                            "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            DuAnTP.TableName = "DuAnThanhPhan";
            fr.AddTable("DuAnThanhPhan", DuAnTP);

            DataTable DuAn = HamChung.SelectDistinct_QLDA("DuAn", DuAnTP, "iID_MaNguonNganSach,sDeAn,sDuAn", "iID_MaNguonNganSach,sDeAn,sDuAn,sTenDuAn,sTienDo",
                                                          "sDeAn,sDuAn,sDuAnThanhPhan");
            DuAn.TableName = "DuAn";
            fr.AddTable("DuAn", DuAn);

            DataTable DeAn = HamChung.SelectDistinct_QLDA("DeAn", data, "iID_MaNguonNganSach,sDeAn", "iID_MaNguonNganSach,sDeAn,sTenDuAn,sTienDo",
                                                        "sDeAn,sDuAn");
            DeAn.TableName = "DeAn";
            fr.AddTable("DeAn", DeAn);
            DataTable NguonNganSach = HamChung.SelectDistinct("NguonNganSach", DeAn, "iID_MaNguonNganSach", "iID_MaNguonNganSach,sTenDuAn", "", "iID_MaNguonNganSach");
            DuAn.TableName = "NguonNganSach";
            fr.AddTable("NguonNganSach", NguonNganSach);


            CongTrinh.Dispose();
            DuAn.Dispose();
            DuAnTP.Dispose();
            data.Dispose();
            dtHmct.Dispose();
            NguonNganSach.Dispose();
        }

        /// <summary>
        public clsExcelResult ExportToExcel(String NamLamViec, String iID_MaNgoaiTe, String dsDeAn, String iCapTongHop)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(NamLamViec, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptQLDA_02_QTHT_KHVDT.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String iID_MaNgoaiTe, String dsDeAn,
                                    String iCapTongHop)
        {
            HamChung.Language();

            ExcelFile xls = CreateReport(NamLamViec, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
            using (var pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (var ms = new MemoryStream())
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

        public string sDanhSachDeAn(string strDsDuAn, string iNamLamViec)
        {
            DataTable dtDeAn = dsDeAn(iNamLamViec);
            var stb = new StringBuilder();
            stb.Append("<table  class=\"mGrid\">");
            stb.Append("<tr>");
            stb.Append(
                "<th align=\"left\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAll(this.checked)\" /></th>");
            stb.Append("<th></th>");
            stb.Append("</tr>");

            String strsTen = "", maDuAn = "";
            if (dtDeAn != null)
            {
                for (int i = 0; i < dtDeAn.Rows.Count; i++)
                {
                    strsTen = Convert.ToString(dtDeAn.Rows[i]["sTenDuAn"]);
                    maDuAn = Convert.ToString(dtDeAn.Rows[i]["sDeAn"]);
                    string ckh = string.Empty;
                    if (strDsDuAn.Contains(maDuAn))
                    {
                        ckh = "checked = 'checked'";
                    }
                    stb.Append("<tr>");
                    stb.Append("<td align=\"left\">");
                    stb.Append("<input type=\"checkbox\" value=\"" + maDuAn +
                               "\" check-group=\"iID_MaDanhMucDuAn\" id=\"iID_MaDanhMucDuAn\" name=\"iID_MaDanhMucDuAn\" " +
                               ckh + "/>");
                    stb.Append("</td>");
                    stb.Append("<td align=\"left\">" + strsTen);
                    stb.Append("</td>");
                    stb.Append("</tr>");
                }
            }

            stb.Append("</table>");
            return stb.ToString();
        }

        public DataTable dsNgoaiTe()
        {
            var dt = new DataTable();
            string query =
                string.Format(
                    @"select iID_MaNgoaiTe,sTen from dbo.QLDA_NgoaiTe where iTrangThai = 1");
            try
            {
                dt = Connection.GetDataTable(query);
                DataRow dr = dt.NewRow();
                dr["iID_MaNgoaiTe"] = 0;
                dr["sTen"] = "VND";
                dt.Rows.InsertAt(dr, 0);

            }
            catch (Exception)
            {
                return dt;
            }
            return dt;
        }

        public DataTable dsDeAn(string iNamLamViec)
        {
            var dt = new DataTable();
            string query =
                string.Format(
                    @"SELECT DISTINCT sDeAn,sTenDuAn
 FROM QLDA_DanhMucDuAn
 WHERE sDuAn='' AND iTrangThai=1");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                dt = Connection.GetDataTable(cmd);
            }
            catch (Exception)
            {
                return dt;
            }
            return dt;
        }
        public string sDanhSachNgoaiTe(string iMaNgoaite)
        {
            DataTable dtNgoaiTe = dsNgoaiTe();
            var stb = new StringBuilder();
            stb.Append("<table  class=\"mGrid\">");
            String strsTen = "", maNgoaiTe = "";
            if (dtNgoaiTe != null)
            {
                for (int i = 0; i < dtNgoaiTe.Rows.Count; i++)
                {
                    strsTen = Convert.ToString(dtNgoaiTe.Rows[i]["sTen"]);
                    maNgoaiTe = Convert.ToString(dtNgoaiTe.Rows[i]["iID_MaNgoaiTe"]);
                    stb.Append("<tr>");
                    stb.Append("<td align=\"right\">");
                    string chk = string.Empty;
                    if (maNgoaiTe.Trim().Equals(iMaNgoaite.Trim()))
                    {
                        chk = " CHECKED ";
                    }
                    stb.Append("<input type=\"radio\" value=\"" + maNgoaiTe +
                               "\"  name=\"iID_MaNgoaiTe\" id=\"id_MaTaiKhoan\" " + chk + "/>");
                    stb.Append("&nbsp;&nbsp;");
                    stb.Append("</td>");
                    stb.Append("<td align=\"left\">" + "&nbsp;&nbsp;" + maNgoaiTe + "-" + strsTen);
                    stb.Append("</td>");
                    stb.Append("</tr>");
                }
            }

            stb.Append("</table>");
            return stb.ToString();
        }












        #region Nested type: CapTongHop

        public static class CapTongHop
        {
            public const string DuAn = "0";
            public const string DuAnTP = "1";
            public const string CongTrinh = "2";
            public const string HangMucCongTrinh = "3";
            public const string HangMucChiTiet = "4";
        }
        public static class LoaiDieuChinh
        {
            public const string ThemMoi = "1";
            public const string DieuChinh = "2";
            public const string ThayThe = "3";
        }
        public static class DonViTinh
        {
            public const string VND = "0";
            public const string USD = "1";
            public const string EUR = "2";
            public const string JPY = "3";
        }
        #endregion
        public DataTable dtDuToanNam(String iNamLamViec, String dsDeAn, String iLoaiNgoaiTe)
        {
            DataTable dt = null;
            String SQL = String.Format(@"SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaNguonNganSach,sM
 ,SUM(rTongMucDauTu) as rTongMucDauTu
 ,SUM(rTongDuToan) as rTongDuToan
 ,SUM(rSoTienChuDauTuDeNghi) as rSoTienChuDauTuDeNghi
 ,SUM(rSoTienPheDuyet) as rSoTienPheDuyet
 ,SUM(CP_NamTruoc) as CP_NamTruoc
 ,SUM(CP_NamNay) as CP_NamNay
FROM(
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,sM,
	SUM(rSoTien/1000000) as rTongMucDauTu,
	rTongDuToan=0,
	rSoTienChuDauTuDeNghi=0,
	rSoTienPheDuyet=0,
	CP_NamTruoc=0,
	 CP_NamNay=0
	 FROM QLDA_TongDauTu WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec 
	GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn,sLNS,sM

UNION 

SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet , SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,sM,rTongMucDauTu=0,
	 SUM(rSoTien/1000000) as rTongDuToan ,
	 rSoTienChuDauTuDeNghi=0,
	rSoTienPheDuyet=0,
	CP_NamTruoc=0,
	 CP_NamNay=0
	 FROM QLDA_TongDuToan WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec 
	GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS,sM
	
	UNION
	--QTHT
	SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,sM,rTongMucDauTu=0,
	 rTongDuToan=0,
     SUM(rSoTienChuDauTuDeNghi/1000000) as rSoTienChuDauTuDeNghi,
	 SUM(rSoTienPheDuyet/1000000) as rSoTienPheDuyet,
	 CP_NamTruoc=0,
	 CP_NamNay=0
	 FROM QLDA_QuyetToanHoanThanh WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec 
	 GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS,sM
	 UNION 
	 --CapPhat
	 SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,sM
	 ,rTongMucDauTu=0,
	rTongDuToan=0,
	rSoTienChuDauTuDeNghi=0,
	rSoTienPheDuyet=0,
	 CP_NamTruoc
	 ,CP_NamNay
	  FROM(
	 SELECT iID_MaDanhMucDuAn,
	SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sM
	,CP_NamTruoc=SUM(CASE WHEN iNamLamViec<=@iNamTruoc THEN rDeNghiPheDuyetThanhToan/1000000 ELSE 0 END)
	,CP_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetThanhToan/1000000 ELSE 0 END)
	FROM QLDA_CapPhat
	WHERE iTrangThai=1  AND iID_MaLoaiKeHoachVon=1
	GROUP BY iID_MaDanhMucDuAn,sLNS,sM
	HAVING SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN rDeNghiPheDuyetThanhToan ELSE 0 END)<>0) as CP
	 INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                                                                                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                                                                                FROM QLDA_DanhMucDuAn 
                                                                                WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as DM
                                        ON CP.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn) CT
                                        GROUP BY  sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaNguonNganSach,sM
                                        HAVING  SUM(rTongMucDauTu) <>0 OR 
 SUM(rTongDuToan) <>0 OR
 SUM(rSoTienChuDauTuDeNghi) <>0 OR
 SUM(rSoTienPheDuyet)  <>0 OR
 SUM(CP_NamTruoc)  <>0 OR
 SUM(CP_NamNay) <>0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt32(iNamLamViec)-1);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }

}