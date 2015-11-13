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
using DomainModel.Abstract;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;
namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_ThongBaoKHVController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_ThongBaoKHV.xls";
        private const String sFilePath_HMCT = "/Report_ExcelFrom/QLDA/rptQLDA_ThongBaoKHV_HMCT.xls";
        private const String sFilePath_CT = "/Report_ExcelFrom/QLDA/rptQLDA_ThongBaoKHV_CT.xls";
        private const String sFilePath_DATP = "/Report_ExcelFrom/QLDA/rptQLDA_ThongBaoKHV_DATP.xls";
        private const String sFilePath_DuAn = "/Report_ExcelFrom/QLDA/rptQLDA_ThongBaoKHV_DuAn.xls";
        private const String sFilePath_DeAn = "/Report_ExcelFrom/QLDA/rptQLDA_ThongBaoKHV_DeAn.xls";
        /// <summary>
        /// Index
        /// </summary>        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ThongBaoKHV.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String dNgay = Convert.ToString(Request.Form[ParentID + "_vidNgay"]);
            String sDeAn = Convert.ToString(Request.Form["sDeAn"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            String iCapTongHop = Convert.ToString(Request.Form[ParentID + "_iCapTongHop"]);
            String iID_MaLoaiKeHoachVon = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiKeHoachVon"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "sLNS"]);
            ViewData["dNgay"] = dNgay;
            ViewData["sDeAn"] = sDeAn;
            ViewData["MaTien"] = MaTien;
            ViewData["iCapTongHop"] = iCapTongHop;
            ViewData["iID_MaLoaiKeHoachVon"] = iID_MaLoaiKeHoachVon;
            ViewData["sLNS"] = sLNS;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ThongBaoKHV.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dtQLDA_ThongBaoKHV(String dNgay, String MaTien, String sDeAn, String iID_MaLoaiKeHoachVon,String sLNS)
        {
            String DKNgoaiTe = "";
            String DKLoaiNgoaiTe = "";
            //VND
            if (MaTien == "0")
            {
                DKNgoaiTe = "(rSoTienDauNam+rSoTienDieuChinh)/1000000";
            }
            else
            {
                DKNgoaiTe = "rNgoaiTe_DauNam+rNgoaiTe_DieuChinh";
                DKLoaiNgoaiTe = "iID_MaNgoaiTe_DauNam=@iID_MaNgoaiTe_DauNam AND";
            }

            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                DKDeAn += "sDeAn=@sDeAn" + i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR ";
            }
            String SQL = String.Format(@"SELECT iID_MaDanhMucDuAn,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS,
	                                           KHV_9200=SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END),
                                               KHV_9250=SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END),
                                               KHV_9300=SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END),
                                               KHV_9350=SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END),
                                               KHV_9400=SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END)
                                            FROM QLDA_KeHoachVon
                                            WHERE iTrangThai=1
                                                    AND sLNS=@sLNS
                                                    AND iLoaiKeHoachVon=@iID_MaLoaiKeHoachVon
                                                     AND dNgayKeHoachVon<=@dNgay AND {2}
                                                      ({0})
                                            GROUP BY iID_MaDanhMucDuAn,
		                                             sDeAn,
		                                             sDuAn,
		                                             sDuAnThanhPhan,
		                                             sCongTrinh,
		                                             sHangMucCongTrinh,
		                                             sHangMucChiTiet,
		                                             SUBSTRING(sTenDuAn,19,10000),
		                                             SUBSTRING(sLNS,1,1)
                                            HAVING ABS(SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END))>=0.5
	                                               OR ABS(SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END))>=0.5
                                                   OR ABS(SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END))>=0.5
                                                   OR ABS(SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END))>=0.5
                                                   OR ABS(SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END))>=0.5	  
		                                    ORDER BY SUBSTRING(sLNS,1,1),
		                                             sDeAn,
		                                             sDuAn,
		                                             sDuAnThanhPhan,
		                                             sCongTrinh,
		                                             sHangMucCongTrinh,
		                                             sHangMucChiTiet", DKDeAn, DKNgoaiTe,DKLoaiNgoaiTe);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe_DauNam", MaTien);
            }
            cmd.Parameters.AddWithValue("@iID_MaLoaiKeHoachVon", iID_MaLoaiKeHoachVon);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        private void LoadData(FlexCelReport fr, String dNgay, String MaTien, String sDeAn, String iID_MaLoaiKeHoachVon, String sLNS)
        {
            DataTable data = dtQLDA_ThongBaoKHV(dNgay, MaTien, sDeAn, iID_MaLoaiKeHoachVon,sLNS);
            data.Columns.Add("sTienDo",typeof(String));
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", data, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHangMucCongTrinh, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DATP", dtCongTrinh, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DuAn", dtDuAnThanhPhan, "NguonNS,sDeAn,sDuAn", "NguonNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sDeAn", "NguonNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");

            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CongTrinh", dtCongTrinh);
            fr.AddTable("DATP", dtDuAnThanhPhan);
            fr.AddTable("DuAn", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("Nguon", dtNguon);
            dtDeAn.Dispose();
            dtDuAn.Dispose();
            dtDuAnThanhPhan.Dispose();
            dtCongTrinh.Dispose();
            dtNguon.Dispose();
            data.Dispose();
        }
        public ActionResult ViewPDF(String dNgay, String MaTien, String sDeAn, String iCapTongHop, String iID_MaLoaiKeHoachVon, String sLNS)
        {
            String DuongDan = "";
            if (iCapTongHop == "0") DuongDan = sFilePath_DeAn;
            else if (iCapTongHop == "1") DuongDan = sFilePath_DuAn;
            else if (iCapTongHop == "2") DuongDan = sFilePath_DATP;
            else if (iCapTongHop == "3") DuongDan = sFilePath_CT;
            else if (iCapTongHop == "4") DuongDan = sFilePath_HMCT;
            else DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), dNgay, MaTien, sDeAn, iID_MaLoaiKeHoachVon,sLNS);
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
        public ExcelFile CreateReport(String path, String dNgay, String MaTien, String sDeAn, String iID_MaLoaiKeHoachVon,String sLNS)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String Ngay = "ngày " + dNgay.Substring(0, 2) + " tháng " + dNgay.Substring(3, 2) + " năm " + dNgay.Substring(6, 4);
            String Nam = dNgay.Substring(6, 4);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_ThongBaoKHV");
            DataTable dtDVT = QLDA_ReportModel.dt_LoaiTien(sDeAn, dNgay);
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                }

            }
            dtDVT.Dispose();

            //lấy tên sLNS
            String TenLNS="",sL="",sK="";
            DataTable dtLNS = QLDA_ReportModel.dt_LoaiNganSachKHV(dNgay, sDeAn);
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                if (sLNS == dtLNS.Rows[i]["sLNS"].ToString())
                {
                    TenLNS = dtLNS.Rows[i]["sMoTa"].ToString();
                    sL = dtLNS.Rows[i]["sL"].ToString();
                    sK = dtLNS.Rows[i]["sK"].ToString();
                }
            }
            dtLNS.Dispose();
            //lấy tên KHV

            String TenKHV = "";
            if (!String.IsNullOrEmpty(iID_MaLoaiKeHoachVon))
            {
                TenKHV = Convert.ToString(CommonFunction.LayTruong("QLDA_KeHoachVon_Loai", "iID_MaLoaiKeHoachVon", iID_MaLoaiKeHoachVon, "sTen"));
            }
            LoadData(fr, dNgay, MaTien, sDeAn, iID_MaLoaiKeHoachVon,sLNS);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("DVT", DVT);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.SetValue("Nam", Nam);
            fr.SetValue("TenLNS", TenLNS.ToUpper());
            fr.SetValue("sL", sL);
            fr.SetValue("sK", sK);
            fr.SetValue("TenKHV", TenKHV);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
        }
        public class QLDA_ThongBaoKHV
        {
            public String DeAn { get; set; }
            public String NgoaiTe { get; set; }
            public String sLNS { get; set; }
        }
        [HttpGet]
        public JsonResult ds_QLDA(String ParentID, String dNgay, String MaTien, String sDeAn,String sLNS)
        {
            return Json(obj_QLDA(ParentID, dNgay, MaTien, sDeAn,sLNS), JsonRequestBehavior.AllowGet);
        }
        public QLDA_ThongBaoKHV obj_QLDA(String ParentID, String dNgay, String MaTien, String sDeAn,String sLNS)
        {
            QLDA_ThongBaoKHV data = new QLDA_ThongBaoKHV();
            #region đề án
            String input = "";
            DataTable dtDeAn = QLDA_ReportModel.dt_DeAn_KHV(dNgay);
            StringBuilder stbDeAn = new StringBuilder();
            stbDeAn.Append("<fieldset>");
            stbDeAn.Append("<legend><b>Đề án</b></legend>");
            stbDeAn.Append("<div style=\"width: 99%; height: 150px; overflow: scroll; border:1px solid black;\">");
            stbDeAn.Append("<table class=\"mGrid\">");
            stbDeAn.Append("<tr>");
            stbDeAn.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đề án </td>");
            stbDeAn.Append("</fieldset>");
            String TenDeAn = "", MaDeAn = "";
            String[] arrDeAn = sDeAn.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dtDeAn.Rows.Count; i++)
            {
                MaDeAn = Convert.ToString(dtDeAn.Rows[i - 1]["sDeAn"]);
                TenDeAn = Convert.ToString(dtDeAn.Rows[i - 1]["sTenDuAn"]);
                _Checked = "";
                for (int j = 1; j <= arrDeAn.Length; j++)
                {
                    if (MaDeAn == arrDeAn[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDeAn\" id=\"sDeAn\" name=\"sDeAn\" onchange=\"ChonThang()\" />", MaDeAn, _Checked);
                stbDeAn.Append("<tr>");
                stbDeAn.Append("<td style=\"width: 15%;\">");
                stbDeAn.Append(input);
                stbDeAn.Append("</td>");
                stbDeAn.Append("<td>" + TenDeAn + "</td>");

                stbDeAn.Append("</tr>");
            }
            stbDeAn.Append("</table>");
            stbDeAn.Append("</div>");
            dtDeAn.Dispose();
            String DeAn = stbDeAn.ToString();
            data.DeAn = DeAn;
            #endregion
            #region ngoại tệ
            DataTable dtNgoaiTe = QLDA_ReportModel.dt_LoaiTien_KHV(sDeAn, dNgay);
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTenNgoaiTe");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            data.NgoaiTe = NgoaiTe;
            #endregion
            #region sLNS
            DataTable dtLNS = QLDA_ReportModel.dt_LoaiNganSachKHV(dNgay, sDeAn);
            SelectOptionList slLNS = new SelectOptionList(dtLNS,"sLNS","TenHT");
            String LoaiNganSach = MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 80%\"");
            data.sLNS = LoaiNganSach;
            #endregion
            return data;
        }
    }
}
