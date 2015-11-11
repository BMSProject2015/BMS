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
    public class rptQLDA_01DT_KHVDTController : Controller
    {
        //
        // GET: /rptQLDA_01DT/


        private const String sFilePath = "~/Report_ExcelFrom/QLDA/rptQLDA_01CP_KHVDT.xls";
        private const String sFilePathDuAn = "~/Report_ExcelFrom/QLDA/rptQLDA_01CP_KHVDT_DA.xls";
        private const String sFilePathCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_01CP_KHVDT_CT.xls";
        private const String sFilePathDuAnTP = "~/Report_ExcelFrom/QLDA/rptQLDA_01CP_KHVDT_DATP.xls";
        private const String sFilePathHaMucCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_01CP_KHVDT_HMCT.xls";
        private const String sFilePathHaMucChiTiet = "~/Report_ExcelFrom/QLDA/rptQLDA_01CP_KHVDT.xls";
        public static String NameFile = "";
        public string sViewPath = "~/Report_Views/";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01DT_KHVDT.aspx";
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

            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01DT_KHVDT.aspx";
            ViewData["dsDeAn"] = iID_MaDeAn;
            ViewData["iCapTongHop"] = iCapTongHop;
            ViewData["iID_MaNgoaiTe"] = iLoaiNgoaiTe;
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
                    break;
            }
            //Set những giá trị chung cho các loại báo cáo
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_01DT_KHVDT");
            LoadData(fr, NamLamViec, iID_MaNgoaiTe, dsDeAn);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3));
            String strDonViTinh = string.Empty;
            switch (iID_MaNgoaiTe)
            {
                case DonViTinh.VND:
                    strDonViTinh = "VND";
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
        /// Ham xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec, String iID_MaNgoaiTe, String dsDeAn, String iCapTongHop)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(NamLamViec, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
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
        public clsExcelResult ExportToExcel(String NamLamViec, String iID_MaNgoaiTe, String dsDeAn, String iCapTongHop)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(NamLamViec, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptQLDA_01CP_KHVDT.xls";
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

        public DataTable dtCPV(string iNamLamViec, DateTime dNgayNhap, string iLoaiNgoaiTe, string dsDeAn)
        {
            var dtCPV = new DataTable();
            String query =
                string.Format(
                    @"
Select ns.sTen,tbl7.*,dmda.sDeAn,dmda.sDuAn,dmda.sDuAnThanhPhan,dmda.sCongTrinh,dmda.sHangMucCongTrinh,dmda.sHangMucChiTiet,dmda.sTenDuAn
From 
	(Select tbl5.iID_MaDanhMucDuAn,tbl5.iID_MaNguonNganSach, sum(tbl5.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl5.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl5.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl5.rNgoaiTe_VonTamUngChuaThuHoiNamNay) rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl5.rNgoaiTe_ChuDauTuThanhToanNamNay) rNgoaiTe_ChuDauTuThanhToanNamNay , sum( tbl6.rNgoaiTe_ChuDauTuTamUng - tbl6.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamTruoc , sum(tbl6.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamTruoc 
	From
		(Select tbl3.iID_MaDanhMucDuAn,tbl3.iID_MaNguonNganSach,sum(tbl3.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl3.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl3.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl4.rNgoaiTe_ChuDauTuTamUng - tbl4.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl4.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamNay 
		from 
			(select tbl1.iID_MaDanhMucDuAn,tbl1.iID_MaNguonNganSach, sum( tbl2.rNgoaiTe_ChuDauTuTamUng - tbl2.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoi,sum(tbl1.rNgoaiTe_SoTienDauNam) rNgoaiTe_SoTienDauNam, sum(tbl1.rNgoaiTe_DieuChinh)rNgoaiTe_DieuChinh 
			from
				(SELECT iID_MaDanhMucDuAn,iID_MaNguonNganSach, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
					FROM [QLDA_TongDauTu]  
					where  iNamLamViec = @iNamLamViec and  iTrangThai = 1 
					and dNgayKeHoachVon <= @dNgayNhap and sDeAn in ({0})
					and iLoaiKeHoachVon = 1 and iID_MaNgoaiTe_DauNam = @iLoaiNgoaiTe and iID_MaNgoaiTe_DieuChinh = @iLoaiNgoaiTe
				Group by iID_MaDanhMucDuAn,iID_MaNguonNganSach)  tbl1  
			left join dbo.QLDA_CapPhat tbl2 on (tbl1.iID_MaDanhMucDuAn = tbl2.iID_MaDanhMucDuAn and tbl2.iID_MaLoaiKeHoachVon = 2 and tbl2.iNamLamViec = @iNamLamViec and tbl2.dNgayLap <= @dNgayNhap and tbl2.iID_MaNgoaiTe_ChuDauTuTamUng = @iLoaiNgoaiTe and tbl2.iID_MaNgoaiTe_ChuDauTuThuTamUng = @iLoaiNgoaiTe)
			Group by tbl1.iID_MaDanhMucDuAn,tbl1.iID_MaNguonNganSach) tbl3
		left join dbo.QLDA_CapPhat tbl4 on (tbl3.iID_MaDanhMucDuAn =  tbl4.iID_MaDanhMucDuAn and tbl4.iID_MaLoaiKeHoachVon = 1 and tbl4.iNamLamViec = @iNamLamViec and tbl4.iTrangThai = 1 and tbl4.dNgayLap <=  @dNgayNhap and tbl4.iID_MaNgoaiTe_ChuDauTuTamUng = @iLoaiNgoaiTe and tbl4.iID_MaNgoaiTe_ChuDauTuThuTamUng = @iLoaiNgoaiTe and tbl4.iID_MaNgoaiTe_ChuDauTuThanhToan = @iLoaiNgoaiTe)
		Group by tbl3.iID_MaDanhMucDuAn,tbl3.iID_MaNguonNganSach) tbl5
	left join dbo.QLDA_CapPhat tbl6 on (tbl5.iID_MaDanhMucDuAn =  tbl6.iID_MaDanhMucDuAn and tbl6.iID_MaLoaiKeHoachVon = 2 and tbl6.iNamLamViec = @iNamLamViec and tbl6.iTrangThai = 1 and tbl6.dNgayLap <=  @dNgayNhap and tbl6.iID_MaNgoaiTe_ChuDauTuTamUng = @iLoaiNgoaiTe and tbl6.iID_MaNgoaiTe_ChuDauTuThuTamUng = @iLoaiNgoaiTe and tbl6.iID_MaNgoaiTe_ChuDauTuThanhToan = @iLoaiNgoaiTe)
	Group by tbl5.iID_MaDanhMucDuAn,tbl5.iID_MaNguonNganSach) tbl7
join QLDA_DanhMucDuAn dmda on dmda.iID_MaDanhMucDuAn = tbl7.iID_MaDanhMucDuAn
join NS_NguonNganSach ns on ns.iID_MaNguonNganSach = tbl7.iID_MaNguonNganSach",
                    dsDeAn);
            var cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@dNgayNhap", dNgayNhap);
            cmd.Parameters.AddWithValue("@iLoaiNgoaiTe", iLoaiNgoaiTe);
            dtCPV = Connection.GetDataTable(cmd);
            return dtCPV;
        }

        #region Lấy danh sách các hạ mục chi tiết

        public DataTable dsGhepKetQua(string iNamLamViec, string dsDeAn, String iLoaiNgoaiTe)
        {
            DataTable result = new DataTable();
            result.TableName = "ChiTiet";
            result.Columns.Add("iID_MaNguonNganSach", typeof(string));
            result.Columns.Add("sTen", typeof(string));
            result.Columns.Add("iID_MaDanhMucDuAn", typeof(string));
            result.Columns.Add("sDeAn", typeof(string));
            result.Columns.Add("sDuAn", typeof(string));
            result.Columns.Add("sDuAnThanhPhan", typeof(string));
            result.Columns.Add("sCongTrinh", typeof(string));
            result.Columns.Add("sHangMucCongTrinh", typeof(string));
            result.Columns.Add("sHangMucChiTiet", typeof(string));
            result.Columns.Add("sTenDuAn", typeof(string));
            result.Columns.Add("sTienDo", typeof(string));
            result.Columns.Add("rTongMucDauTu", typeof(Decimal));
            result.Columns.Add("rTongDuToan", typeof(Decimal));
            result.Columns.Add("rKHVDenNamTruoc", typeof(Decimal));
            result.Columns.Add("rKHVNamTruoc", typeof(Decimal));
            result.Columns.Add("rKHVUngTruoc", typeof(Decimal));
            result.Columns.Add("rDonViDeNghi", typeof(Decimal));
            result.Columns.Add("rDonViThuTamUng", typeof(Decimal));
            result.Columns.Add("rDonViThu", typeof(Decimal));
            result.Columns.Add("rCucTaiChinhDeNghi", typeof(Decimal));
            result.Columns.Add("rCucTaiChinhThuTamUng", typeof(Decimal));
            result.Columns.Add("rCucTaiChinhThu", typeof(Decimal));

            DataTable dtHMCT = dsHangMucChiTiet(iNamLamViec, dsDeAn);
            DataTable dtTongDuToan = dsTinhTongDuToan(iNamLamViec, dsDeAn, iLoaiNgoaiTe);
            DataTable dtTongMucDauTu = dsTinhTongMucDauTu(iNamLamViec, dsDeAn, iLoaiNgoaiTe);
            DataTable dtKHVDenNamTruoc = dsKHVDenNamTruoc(iNamLamViec, dsDeAn, iLoaiNgoaiTe);
            DataTable dtKHVNamTruoc = dsKHVNamTruoc(iNamLamViec, dsDeAn, iLoaiNgoaiTe);
            DataTable dtKHVNamNay = dsKeHoachVonNamNay(iNamLamViec, dsDeAn, iLoaiNgoaiTe);

            foreach (DataRow dataRow in dtHMCT.Rows)
            {
                DataRow row = result.NewRow();
                String iID_MaNguonNganSach = Convert.ToString(dataRow["iID_MaNguonNganSach"]);
                String iID_MaDanhMucDuAn = Convert.ToString(dataRow["iID_MaDanhMucDuAn"]);
                row["iID_MaNguonNganSach"] = dataRow["iID_MaNguonNganSach"];
                row["iID_MaDanhMucDuAn"] = dataRow["iID_MaDanhMucDuAn"];
                row["sDeAn"] = dataRow["sDeAn"];
                row["sDuAn"] = dataRow["sDuAn"];
                row["sDuAnThanhPhan"] = dataRow["sDuAnThanhPhan"];
                row["sCongTrinh"] = dataRow["sCongTrinh"];
                row["sHangMucCongTrinh"] = dataRow["sHangMucCongTrinh"];
                row["sHangMucChiTiet"] = dataRow["sHangMucChiTiet"];
                row["sTenDuAn"] = dataRow["sTenDuAn"];
                row["sTen"] = dataRow["sTen"];
                foreach (DataRow dataRow1 in dtTongMucDauTu.Rows)
                {
                    String iID_MaNguonNganSach1 = Convert.ToString(dataRow1["iID_MaNguonNganSach"]).Trim();
                    String iID_MaDanhMucDuAn1 = Convert.ToString(dataRow1["iID_MaDanhMucDuAn"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(iID_MaDanhMucDuAn1) && iID_MaNguonNganSach.Equals(iID_MaNguonNganSach1))
                    {
                        Decimal rTien = 0;
                        if (dataRow1["rSoTien"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rSoTien"]);
                        }
                        row["rTongMucDauTu"] = rTien;
                        break;

                    }
                }
                foreach (DataRow dataRow1 in dtTongDuToan.Rows)
                {
                    String iID_MaNguonNganSach1 = Convert.ToString(dataRow1["iID_MaNguonNganSach"]).Trim();
                    String iID_MaDanhMucDuAn1 = Convert.ToString(dataRow1["iID_MaDanhMucDuAn"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(iID_MaDanhMucDuAn1) && iID_MaNguonNganSach.Equals(iID_MaNguonNganSach1))
                    {
                        Decimal rTien = 0;
                        if (dataRow1["rSoTien"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rSoTien"]);
                        }
                        row["rTongDuToan"] = rTien;
                        break;

                    }
                }
                foreach (DataRow dataRow1 in dtKHVDenNamTruoc.Rows)
                {
                    String iID_MaNguonNganSach1 = Convert.ToString(dataRow1["iID_MaNguonNganSach"]).Trim();
                    String iID_MaDanhMucDuAn1 = Convert.ToString(dataRow1["iID_MaDanhMucDuAn"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(iID_MaDanhMucDuAn1) && iID_MaNguonNganSach.Equals(iID_MaNguonNganSach1))
                    {
                        Decimal rTien = 0;
                        if (dataRow1["rCucTaiChinhDeNghi"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rCucTaiChinhDeNghi"]);
                        }
                        row["rKHVDenNamTruoc"] = rTien;
                        break;

                    }
                }
                foreach (DataRow dataRow1 in dtKHVNamTruoc.Rows)
                {
                    String iID_MaNguonNganSach1 = Convert.ToString(dataRow1["iID_MaNguonNganSach"]).Trim();
                    String iID_MaDanhMucDuAn1 = Convert.ToString(dataRow1["iID_MaDanhMucDuAn"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(iID_MaDanhMucDuAn1) && iID_MaNguonNganSach.Equals(iID_MaNguonNganSach1))
                    {
                        Decimal rTien = 0;
                        if (dataRow1["rCucTaiChinhDeNghi"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rCucTaiChinhDeNghi"]);
                        }
                        row["rKHVNamTruoc"] = rTien;
                        break;

                    }
                }
                foreach (DataRow dataRow1 in dtKHVNamNay.Rows)
                {
                    String iID_MaNguonNganSach1 = Convert.ToString(dataRow1["iID_MaNguonNganSach"]).Trim();
                    String iID_MaDanhMucDuAn1 = Convert.ToString(dataRow1["iID_MaDanhMucDuAn"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(iID_MaDanhMucDuAn1) && iID_MaNguonNganSach.Equals(iID_MaNguonNganSach1))
                    {
                        Decimal rTien = 0;
                        if (dataRow1["rDonViDeNghi"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rDonViDeNghi"]);
                        }
                        row["rDonViDeNghi"] = rTien;

                        if (dataRow1["rDonViThuTamUng"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rDonViThuTamUng"]);
                        }
                        row["rDonViThuTamUng"] = rTien;
                        if (dataRow1["rDonViThu"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rDonViThu"]);
                        }
                        row["rDonViThu"] = rTien;

                        if (dataRow1["rCucTaiChinhDeNghi"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rCucTaiChinhDeNghi"]);
                        }
                        row["rCucTaiChinhDeNghi"] = rTien;

                        if (dataRow1["rCucTaiChinhThuTamUng"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rCucTaiChinhThuTamUng"]);
                        }
                        row["rCucTaiChinhThuTamUng"] = rTien;
                        if (dataRow1["rCucTaiChinhThu"] != DBNull.Value)
                        {
                            rTien = Convert.ToDecimal(dataRow1["rCucTaiChinhThu"]);
                        }
                        row["rCucTaiChinhThu"] = rTien;
                        break;

                    }
                }
                result.Rows.Add(row);
            }


            dtHMCT.Dispose();
            dtTongDuToan.Dispose();
            dtTongMucDauTu.Dispose();
            dtKHVDenNamTruoc.Dispose();
            dtKHVNamNay.Dispose();
            dtKHVNamTruoc.Dispose();
            //Chuyển số tiền từ Đồng sang Triệu đồng
            DataTable dt = new DataTable();
            dt = result.Copy();
            if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
            {
                int milion = 1000000;
                if (result.Rows.Count != 0)
                {

                    dt.Clear();
                    foreach (DataRow dataRow in result.Rows)
                    {
                        int count = 0;

                        if (dataRow["rTongMucDauTu"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rTongMucDauTu"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rTongMucDauTu"] = convertedValue;
                        }

                        if (dataRow["rTongDuToan"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rTongDuToan"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rTongDuToan"] = convertedValue;
                        }
                        if (dataRow["rKHVDenNamTruoc"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rKHVDenNamTruoc"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rKHVDenNamTruoc"] = convertedValue;
                        }
                        if (dataRow["rKHVNamTruoc"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rKHVNamTruoc"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rKHVNamTruoc"] = convertedValue;
                        }
                        if (dataRow["rKHVUngTruoc"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rKHVUngTruoc"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rKHVUngTruoc"] = convertedValue;
                        }
                        if (dataRow["rDonViDeNghi"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rDonViDeNghi"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rDonViDeNghi"] = convertedValue;
                        }
                        if (dataRow["rDonViThuTamUng"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rDonViThuTamUng"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rDonViThuTamUng"] = convertedValue;
                        }
                        if (dataRow["rDonViThu"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rDonViThu"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rDonViThu"] = convertedValue;
                        }
                        if (dataRow["rCucTaiChinhDeNghi"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rCucTaiChinhDeNghi"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rCucTaiChinhDeNghi"] = convertedValue;
                        }
                        if (dataRow["rCucTaiChinhThuTamUng"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rCucTaiChinhThuTamUng"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rCucTaiChinhThuTamUng"] = convertedValue;
                        }
                        if (dataRow["rCucTaiChinhThu"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rCucTaiChinhThu"]) / milion, 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rCucTaiChinhThu"] = convertedValue;
                        }
                        if (count != 10)
                        {
                            dt.ImportRow(dataRow);
                        }
                    }
                }
            }
            else
            {
                if (result.Rows.Count != 0)
                {
                    dt.Clear();
                    foreach (DataRow dataRow in result.Rows)
                    {
                        int count = 0;
                        if (dataRow["rTongMucDauTu"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Convert.ToDecimal(dataRow["rTongMucDauTu"]);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rTongMucDauTu"] = convertedValue;
                        }

                        if (dataRow["rTongDuToan"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rTongDuToan"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rTongDuToan"] = convertedValue;
                        }
                        if (dataRow["rKHVDenNamTruoc"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rKHVDenNamTruoc"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rKHVDenNamTruoc"] = convertedValue;
                        }
                        if (dataRow["rKHVNamTruoc"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rKHVNamTruoc"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rKHVNamTruoc"] = convertedValue;
                        }
                        if (dataRow["rKHVUngTruoc"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rKHVUngTruoc"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rKHVUngTruoc"] = convertedValue;
                        }
                        if (dataRow["rDonViDeNghi"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rDonViDeNghi"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rDonViDeNghi"] = convertedValue;
                        }
                        if (dataRow["rDonViThuTamUng"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rDonViThuTamUng"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rDonViThuTamUng"] = convertedValue;
                        }
                        if (dataRow["rDonViThu"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rDonViThu"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rDonViThu"] = convertedValue;
                        }
                        if (dataRow["rCucTaiChinhDeNghi"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rCucTaiChinhDeNghi"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rCucTaiChinhDeNghi"] = convertedValue;
                        }
                        if (dataRow["rCucTaiChinhThuTamUng"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rCucTaiChinhThuTamUng"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rCucTaiChinhThuTamUng"] = convertedValue;
                        }
                        if (dataRow["rCucTaiChinhThu"] != DBNull.Value)
                        {
                            Decimal convertedValue = 0;
                            convertedValue = Math.Round(Convert.ToDecimal(dataRow["rCucTaiChinhThu"]), 1);
                            if (Math.Abs(convertedValue) <= (Decimal)0.5)
                            {
                                convertedValue = 0;
                                count++;
                            }
                            dataRow["rCucTaiChinhThu"] = convertedValue;
                        }
                        if (count != 10)
                        {
                            dt.ImportRow(dataRow);
                        }
                    }
                }

            }
            return dt;
        }

        /// <summary>
        /// Hàm tính tổng mức đầu tư
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDeAn"></param>
        /// <returns></returns>
        public DataTable dsTinhTongMucDauTu(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            dt = dsTongMucDauTu(iNamLamViec, dsDeAn, iLoaiNgoaiTe);
            DataTable dtHMCT = dsHangMucChiTiet(iNamLamViec, dsDeAn);
            DataTable result = new DataTable();
            result.TableName = "Result";
            result.Columns.Add("iID_MaNguonNganSach", typeof(string));
            result.Columns.Add("iID_MaDanhMucDuAn", typeof(string));
            result.Columns.Add("rSoTien", typeof(decimal));
            foreach (DataRow dataRow in dtHMCT.Rows)
            {
                DataRow newRow = result.NewRow();
                String iID_MaNguonNganSach = Convert.ToString(dataRow["iID_MaNguonNganSach"]).Trim();
                String iID_MaDanhMucDuAn = Convert.ToString(dataRow["iID_MaDanhMucDuAn"]).Trim();
                Boolean isFirst = true;
                Decimal value = 0;
                foreach (DataRow row in dt.Rows)
                {
                    String New_iID_MaNguonNganSach = Convert.ToString(row["iID_MaNguonNganSach"]).Trim();
                    String New_iID_MaDanhMucDuAn = Convert.ToString(row["iID_MaDanhMucDuAn"]).Trim();
                    String iID_MaLoaiDieuChinh = Convert.ToString(row["iID_MaLoaiDieuChinh"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(New_iID_MaDanhMucDuAn) && iID_MaNguonNganSach.Equals(New_iID_MaNguonNganSach))
                    {
                        if (isFirst && (iID_MaLoaiDieuChinh.Equals(LoaiDieuChinh.ThayThe) || iID_MaLoaiDieuChinh.Equals(LoaiDieuChinh.ThemMoi)))
                        {
                            value = Convert.ToDecimal(row["rSoTien"]);
                            break;
                        }
                        else
                        {
                            if (iID_MaLoaiDieuChinh.Equals(LoaiDieuChinh.DieuChinh))
                            {
                                value = value + Convert.ToDecimal(row["rSoTien"]);
                            }
                            else
                            {
                                value = value + Convert.ToDecimal(row["rSoTien"]);
                                break;
                            }
                        }
                        isFirst = false;
                    }
                }
                newRow["iID_MaNguonNganSach"] = iID_MaNguonNganSach;
                newRow["iID_MaDanhMucDuAn"] = iID_MaDanhMucDuAn;
                newRow["rSoTien"] = value;
                result.Rows.Add(newRow);
            }
            return result;
        }

        /// <summary>
        /// Tính tổng dự toán
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDeAn"></param>
        /// <returns></returns>
        public DataTable dsTinhTongDuToan(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            dt = dsTongDuToan(iNamLamViec, dsDeAn, iLoaiNgoaiTe);
            DataTable dtHMCT = dsHangMucChiTiet(iNamLamViec, dsDeAn);
            DataTable result = new DataTable();
            result.TableName = "Result";
            result.Columns.Add("iID_MaNguonNganSach", typeof(string));
            result.Columns.Add("iID_MaDanhMucDuAn", typeof(string));
            result.Columns.Add("rSoTien", typeof(decimal));
            foreach (DataRow dataRow in dtHMCT.Rows)
            {
                DataRow newRow = result.NewRow();
                String iID_MaNguonNganSach = Convert.ToString(dataRow["iID_MaNguonNganSach"]).Trim();
                String iID_MaDanhMucDuAn = Convert.ToString(dataRow["iID_MaDanhMucDuAn"]).Trim();
                Boolean isFirst = true;
                Decimal value = 0;
                foreach (DataRow row in dt.Rows)
                {
                    String New_iID_MaNguonNganSach = Convert.ToString(row["iID_MaNguonNganSach"]).Trim();
                    String New_iID_MaDanhMucDuAn = Convert.ToString(row["iID_MaDanhMucDuAn"]).Trim();
                    String iID_MaLoaiDieuChinh = Convert.ToString(row["iID_MaLoaiDieuChinh"]).Trim();
                    if (iID_MaDanhMucDuAn.Equals(New_iID_MaDanhMucDuAn) && iID_MaNguonNganSach.Equals(New_iID_MaNguonNganSach))
                    {
                        if (isFirst && (iID_MaLoaiDieuChinh.Equals(LoaiDieuChinh.ThayThe) || iID_MaLoaiDieuChinh.Equals(LoaiDieuChinh.ThemMoi)))
                        {
                            value = Convert.ToDecimal(row["rSoTien"]);
                            break;
                        }
                        else
                        {
                            if (iID_MaLoaiDieuChinh.Equals(LoaiDieuChinh.DieuChinh))
                            {
                                value = value + Convert.ToDecimal(row["rSoTien"]);
                            }
                            else
                            {
                                value = value + Convert.ToDecimal(row["rSoTien"]);
                                break;
                            }
                        }
                        isFirst = false;
                    }
                }
                newRow["iID_MaNguonNganSach"] = iID_MaNguonNganSach;
                newRow["iID_MaDanhMucDuAn"] = iID_MaDanhMucDuAn;
                newRow["rSoTien"] = value;
                result.Rows.Add(newRow);
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách các hạng mục chi tiết theo từng năm và danh sách các đề án theo dạng sDeAn = 1,2,3
        /// </summary>
        /// <param name="dsDeAn"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public DataTable dsHangMucChiTiet(string iNamLamViec, string dsDeAn)
        {
            if (String.IsNullOrEmpty(dsDeAn))
            {
                dsDeAn = "-1";
            }
            DataTable dt = new DataTable();
            try
            {
                string query =
                    string.Format(
                        @"select tbl1.iID_MaNguonNganSach,nns.sTen,tbl1.iID_MaDanhMucDuAn,dmda.sDeAn,dmda.sDuAn,dmda.sDuAnThanhPhan,dmda.sCongTrinh,dmda.sHangMucCongTrinh,dmda.sHangMucChiTiet,dmda.sTenDuAn
                        From    (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu           
	                            where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1
                        join QLDA_DanhMucDuAn dmda on dmda.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn
                        join NS_NguonNganSach nns on nns.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach", dsDeAn);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                dt = Connection.GetDataTable(cmd);
            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Hàm lấy danh sách các tổng mức đầu tư theo năm làm việc và danh sách đề án theo dạng sDeAn = 1,2,3
        /// Danh sách được Order theo loại Nguồn ngân sách, Mã dự án, ngày phê duyệt và loại điều chỉnh
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDuAn"></param>
        /// <returns></returns>
        public DataTable dsTongMucDauTu(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            try
            {
                if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
                {
                    string query =
                    string.Format(
                        @"select 
		                    tbl2.[iID_MaNguonNganSach]
		                    ,tbl2.[iID_MaDanhMucDuAn]
		                    ,tbl2.[dNgayPheDuyet]
		                    ,tbl2.[iID_MaLoaiDieuChinh] 
		                    ,tbl2.[dNgayLap]      
		                    ,tbl2.[iNamLamViec]
		                    ,tbl2.[rSoTien]
		                    ,tbl2.[rNgoaiTe_SoTien]
		                    ,tbl2.[iID_MaNgoaiTe_SoTien]
		                    ,tbl2.[sTenNgoaiTe_SoTien]
		                    ,tbl2.[rNgoaiTe]
		                    ,tbl2.[iID_MaNgoaiTe]
		                    ,tbl2.[sTenNgoaiTe]
		                    ,tbl2.[rTyGia]
                    from QLDA_TongDauTu tbl2
	                    right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0}) ) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	
                    where tbl2.iNamLamViec = @iNamLamViec and tbl2.iTrangThai = 1	
                    Order by tbl2.iID_MaNguonNganSach,tbl2.iID_MaDanhMucDuAn,tbl2.dNgayPheDuyet desc, tbl2.iID_MaLoaiDieuChinh desc", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    dt = Connection.GetDataTable(cmd);
                }
                else
                {
                    string query =
                    string.Format(
                        @"select 
		                    tbl2.[iID_MaNguonNganSach]
		                    ,tbl2.[iID_MaDanhMucDuAn]
		                    ,tbl2.[dNgayPheDuyet]
		                    ,tbl2.[iID_MaLoaiDieuChinh] 
		                    ,tbl2.[dNgayLap]      
		                    ,tbl2.[iNamLamViec]		                   
		                    ,tbl2.[rNgoaiTe] rSoTien
		                    ,tbl2.[iID_MaNgoaiTe]
		                    ,tbl2.[sTenNgoaiTe]
		                    ,tbl2.[rTyGia]
                    from QLDA_TongDauTu tbl2
	                    right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0}) ) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and tbl2.[iID_MaNgoaiTe] = @iLoaiNgoaiTe
                    where tbl2.iNamLamViec = @iNamLamViec and tbl2.iTrangThai = 1	
                    Order by tbl2.iID_MaNguonNganSach,tbl2.iID_MaDanhMucDuAn,tbl2.dNgayPheDuyet desc, tbl2.iID_MaLoaiDieuChinh desc", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iLoaiNgoaiTe", iLoaiNgoaiTe);
                    dt = Connection.GetDataTable(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Hàm lấy danh sách các tổng dự toán theo năm làm việc và danh sách đề án theo dạng sDeAn = 1,2,3
        /// Danh sách được Order theo loại Nguồn ngân sách, Mã dự án, ngày phê duyệt và loại điều chỉnh
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDuAn"></param>
        /// <returns></returns>
        public DataTable dsTongDuToan(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            try
            {
                if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
                {
                    string query =
                    string.Format(
                        @"select 
		                    tbl2.[iID_MaNguonNganSach]
		                    ,tbl2.[iID_MaDanhMucDuAn]
		                    ,tbl2.[dNgayPheDuyet]
		                    ,tbl2.[iID_MaLoaiDieuChinh] 
		                    ,tbl2.[dNgayLap]      
		                    ,tbl2.[iNamLamViec]
		                    ,tbl2.[rSoTien]
		                    ,tbl2.[rNgoaiTe_SoTien]
		                    ,tbl2.[iID_MaNgoaiTe_SoTien]
		                    ,tbl2.[sTenNgoaiTe_SoTien]
		                    ,tbl2.[rNgoaiTe]
		                    ,tbl2.[iID_MaNgoaiTe]
		                    ,tbl2.[sTenNgoaiTe]
		                    ,tbl2.[rTyGia]
                    from QLDA_TongDuToan tbl2
	                   right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	
                    where tbl2.iNamLamViec = @iNamLamViec and tbl2.iTrangThai = 1	
                    Order by tbl2.iID_MaNguonNganSach,tbl2.iID_MaDanhMucDuAn,tbl2.dNgayPheDuyet desc, tbl2.iID_MaLoaiDieuChinh desc", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    dt = Connection.GetDataTable(cmd);
                }
                else
                {
                    string query =
                   string.Format(
                       @"select 
		                    tbl2.[iID_MaNguonNganSach]
		                    ,tbl2.[iID_MaDanhMucDuAn]
		                    ,tbl2.[dNgayPheDuyet]
		                    ,tbl2.[iID_MaLoaiDieuChinh] 
		                    ,tbl2.[dNgayLap]      
		                    ,tbl2.[iNamLamViec]
		                    ,tbl2.[rNgoaiTe] rSoTien
		                    ,tbl2.[iID_MaNgoaiTe]
		                    ,tbl2.[sTenNgoaiTe]
		                    ,tbl2.[rTyGia]
                    from QLDA_TongDuToan tbl2
	                   right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and tbl2.iID_MaNgoaiTe = @iID_MaNgoaiTe
                    where tbl2.iNamLamViec = @iNamLamViec and tbl2.iTrangThai = 1	
                    Order by tbl2.iID_MaNguonNganSach,tbl2.iID_MaDanhMucDuAn,tbl2.dNgayPheDuyet desc, tbl2.iID_MaLoaiDieuChinh desc", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iLoaiNgoaiTe);
                    dt = Connection.GetDataTable(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }



        /// <summary>
        /// Hàm lấy danh sách kế hoạch vốn theo năm làm việc và danh sách đề án theo dạng sDeAn = 1,2,3
        /// Danh sách được Order theo loại Nguồn ngân sách, Mã dự án, ngày phê duyệt và loại điều chỉnh
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDuAn"></param>
        /// <returns></returns>
        public DataTable dsKeHoachVonNamNay(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            try
            {
                if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
                {
                    string query =
                   string.Format(
                       @"SELECT  tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn,sum(tbl2.rDonViDeNghi) rDonViDeNghi, sum(tbl2.rDonViThuTamUng)rDonViThuTamUng , sum(rDonViThu) rDonViThu, sum(rCucTaiChinhDeNghi)rCucTaiChinhDeNghi,sum(rCucTaiChinhThuTamUng)rCucTaiChinhThuTamUng, sum(rCucTaiChinhThu)rCucTaiChinhThu
                        FROM [QLDA_DuToan_Nam] tbl2
                        right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and tbl2.iNamLamViec = @iNamLamViec and tbl2.iTrangThai = 1                        
                        Group by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn
                        Order by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    dt = Connection.GetDataTable(cmd);
                }
                else
                {
                    string query =
                   string.Format(
                       @"SELECT  tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn,sum(tbl2.rNgoaiTe_rDonViDeNghi) rDonViDeNghi, sum(tbl2.rNgoaiTe_rDonViThuTamUng)rDonViThuTamUng , sum(tbl2.rNgoaiTe_rDonViThu) rDonViThu, sum(tbl2.rNgoaiTe_rCucTaiChinhDeNghi)rCucTaiChinhDeNghi,sum(tbl2.rNgoaiTe_rCucTaiChinhThuTamUng)rCucTaiChinhThuTamUng, sum(tbl2.rNgoaiTe_rCucTaiChinhThu)rCucTaiChinhThu
                        FROM [QLDA_DuToan_Nam] tbl2
                        right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and tbl2.iNamLamViec = @iNamLamViec and tbl2.iTrangThai = 1 and tbl2.iID_MaNgoaiTe_rDonViDeNghi = @iLoaiNgoaiTe and tbl2.iID_MaNgoaiTe_rDonViThuTamUng = @iLoaiNgoaiTe and tbl2.iID_MaNgoaiTe_rDonViThu =  @iLoaiNgoaiTe and   tbl2.iID_MaNgoaiTe_rCucTaiChinhDeNghi = @iLoaiNgoaiTe and tbl2.iID_MaNgoaiTe_rCucTaiChinhThuTamUng = @iLoaiNgoaiTe and tbl2.iID_MaNgoaiTe_rCucTaiChinhThu = @iLoaiNgoaiTe
                        Group by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn
                        Order by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iLoaiNgoaiTe", iLoaiNgoaiTe);
                    dt = Connection.GetDataTable(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Hàm lấy danh sách kế hoạch vốn đến năm trước theo năm làm việc và danh sách đề án theo dạng sDeAn = 1,2,3
        /// Danh sách được Order theo loại Nguồn ngân sách, Mã dự án, ngày phê duyệt và loại điều chỉnh
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDuAn"></param>
        /// <returns></returns>
        public DataTable dsKHVDenNamTruoc(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            try
            {
                if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
                {
                    string query =
                    string.Format(
                        @"SELECT  tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn,sum(tbl2.rCucTaiChinhDeNghi) rCucTaiChinhDeNghi
                        FROM [QLDA_DuToan_Nam] tbl2
                        right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and tbl2.iNamLamViec <= (@iNamLamViec - 1) and tbl2.iTrangThai = 1
                        Group by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn
                        Order by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    dt = Connection.GetDataTable(cmd);
                }
                else
                {
                    string query =
                    string.Format(
                        @"SELECT  tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn,sum(tbl2.rNgoaiTe_rCucTaiChinhDeNghi) rCucTaiChinhDeNghi
                        FROM [QLDA_DuToan_Nam] tbl2
                        right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and tbl2.iNamLamViec <= (@iNamLamViec - 1) and tbl2.iTrangThai = 1 and tbl2.iID_MaNgoaiTe_rCucTaiChinhDeNghi = @iLoaiNgoaiTe
                        Group by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn
                        Order by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn", dsDeAn);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iLoaiNgoaiTe", iLoaiNgoaiTe);
                    dt = Connection.GetDataTable(cmd);

                }

            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Hàm lấy danh sách kế hoạch vốn năm trước theo năm làm việc và danh sách đề án theo dạng sDeAn = 1,2,3
        /// Danh sách được Order theo loại Nguồn ngân sách, Mã dự án, ngày phê duyệt và loại điều chỉnh
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="dsDuAn"></param>
        /// <returns></returns>
        public DataTable dsKHVNamTruoc(string iNamLamViec, string dsDeAn, string iLoaiNgoaiTe)
        {
            DataTable dt = new DataTable();
            try
            {
                if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
                {
                    string query =
                    string.Format(
                        @"SELECT  tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn,sum(tbl2.rNgoaiTe_rCucTaiChinhDeNghi) rCucTaiChinhDeNghi
                        FROM [QLDA_DuToan_Nam] tbl2
                        right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and  tbl2.iNamLamViec = (@iNamLamViec - 1) and tbl2.iTrangThai = 1   and    tbl2.iID_MaNgoaiTe_rCucTaiChinhDeNghi = @iLoaiNgoaiTe                  
                        Group by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn
                        Order by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iLoaiNgoaiTe", iLoaiNgoaiTe);
                    dt = Connection.GetDataTable(cmd);
                }
                else
                {
                    string query =
                    string.Format(
                        @"SELECT  tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn,sum(tbl2.rCucTaiChinhDeNghi) rCucTaiChinhDeNghi
                        FROM [QLDA_DuToan_Nam] tbl2
                        right join (select distinct iID_MaNguonNganSach,iID_MaDanhMucDuAn from dbo.QLDA_TongDauTu
                        where iNamLamViec = @iNamLamViec and iTrangThai = 1 and sDeAn in ({0})) tbl1 on tbl2.iID_MaNguonNganSach = tbl1.iID_MaNguonNganSach and tbl2.iID_MaDanhMucDuAn = tbl1.iID_MaDanhMucDuAn	and  tbl2.iNamLamViec = (@iNamLamViec - 1) and tbl2.iTrangThai = 1                       
                        Group by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn
                        Order by tbl1.iID_MaNguonNganSach , tbl1.iID_MaDanhMucDuAn", dsDeAn);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    dt = Connection.GetDataTable(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }
        #endregion


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
            if (iLoaiNgoaiTe.Equals(DonViTinh.VND))
            {
                String SQL = String.Format(@"
SELECT *,sTienDo='' FROM(
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,19999) as sTenDuAn,iID_MaNguonNganSach,
SUM(rTongMucDauTu/1000000) as rTongMucDauTu,
SUM(rTongDuToan/1000000) as rTongDuToan,
SUM(rDenNamTruoc/1000000) as rDenNamTruoc,
SUM(rNamTruoc/1000000) as rNamTruoc,
SUM(rKHVUngTruoc/1000000) as rKHVUngTruoc,
SUM(rDonViDeNghi/1000000) as rDonViDeNghi,
SUM(rDonViThuTamUng/1000000) as rDonViThuTamUng,
SUM(rDonViThu/1000000) as rDonViThu,
SUM(rCucTaiChinhDeNghi/1000000) as rCucTaiChinhDeNghi,
SUM(rCucTaiChinhThuTamUng/1000000) as rCucTaiChinhThuTamUng,
SUM(rCucTaiChinhThuTamUng_NamTruoc/1000000) as rCucTaiChinhThuTamUng_NamTruoc,
SUM(rCucTaiChinhThu/1000000) as rCucTaiChinhThu
FROM
(
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,
	SUM(rSoTien) as rTongMucDauTu,
	rTongDuToan=0,
	rDenNamTruoc=0,
	rNamTruoc=0,
	rKHVUngTruoc=0,
	rDonViDeNghi=0,
	rDonViThuTamUng=0,
	rDonViThu=0,
	rCucTaiChinhDeNghi=0,
	rCucTaiChinhThuTamUng=0,
    rCucTaiChinhThuTamUng_NamTruoc=0,
	rCucTaiChinhThu=0
	 FROM QLDA_TongDauTu WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec AND sDeAn IN ({0})
	GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS

UNION 

SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,rTongMucDauTu=0,
	 SUM(rSoTien) as rTongDuToan ,
	 rDenNamTruoc=0,
	 rNamTruoc=0,
	rKHVUngTruoc=0,
	rDonViDeNghi=0,
	rDonViThuTamUng=0,
	rDonViThu=0,
	rCucTaiChinhDeNghi=0,
	rCucTaiChinhThuTamUng=0,
    rCucTaiChinhThuTamUng_NamTruoc=0,
	rCucTaiChinhThu=0
	 FROM QLDA_TongDuToan WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec AND sDeAn IN ({0})
	GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS
--KHV
UNION
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,
rTongMucDauTu=0,
rTongDuToan=0,
rDenNamTruoc=SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec<=@iNamTruoc THEN (rSoTienDauNam+rSoTienDieuChinh) ELSE 0 END),
rNamTruoc=SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec=@iNamTruoc THEN (rSoTienDauNam+rSoTienDieuChinh) ELSE 0 END),
rKHVUngTruoc=SUM(CASE WHEN iLoaiKeHoachVon=2 AND iNamLamViec<=@iNamLamViec THEN (rSoTienDauNam+rSoTienDieuChinh) ELSE 0 END),
rDonViDeNghi=0,
rDonViThuTamUng=0,
rDonViThu=0,
rCucTaiChinhDeNghi=0,
rCucTaiChinhThuTamUng=0,
rCucTaiChinhThuTamUng_NamTruoc=0,
rCucTaiChinhThu=0
 FROM QLDA_KeHoachVon
WHERE iTrangThai=1  AND sDeAn IN ({0})
  GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS
UNION 
--DT_nam 
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,
rTongMucDauTu=0,
rTongDuToan=0,
rDenNamTruoc=0,
rNamTruoc=0,
rKHVUngTruoc=0,
rDonViDeNghi=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rDonViDeNghi ELSE 0 END),
rDonViThuTamUng=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rDonViThuTamUng ELSE 0 END),
rDonViThu=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rDonViThu ELSE 0 END),
rCucTaiChinhDeNghi=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rCucTaiChinhDeNghi ELSE 0 END),
rCucTaiChinhThuTamUng=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rCucTaiChinhThuTamUng ELSE 0 END),
rCucTaiChinhThuTamUng_NamTruoc=SUM(CASE WHEN (iNamLamViec<@iNamLamViec) THEN  rCucTaiChinhThuTamUng ELSE 0 END),
rCucTaiChinhThu=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rCucTaiChinhThu ELSE 0 END)
 FROM QLDA_DuToan_Nam
WHERE iTrangThai=1 AND sDeAn IN ({0})
 GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS) as A
 GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaNguonNganSach
 HAVING SUM(rTongMucDauTu)<>0
 OR SUM(rTongDuToan) <>0
 OR SUM(rDenNamTruoc) <>0
 OR SUM(rNamTruoc) <>0
 OR SUM(rKHVUngTruoc) <>0
 OR SUM(rDonViDeNghi) <>0
 OR SUM(rDonViThuTamUng) <>0
 OR SUM(rDonViThu) <>0
 OR SUM(rCucTaiChinhDeNghi) <>0
 OR SUM(rCucTaiChinhThuTamUng) <>0
OR SUM(rCucTaiChinhThuTamUng_NamTruoc) <>0
 OR SUM(rCucTaiChinhThu) <>0) as QLDA
", dsDeAn);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt32(iNamLamViec)-1);
                dt = Connection.GetDataTable(cmd);
            }
                //Loai tien khac
            else
            {
                String SQL = String.Format(@"
SELECT *,sTienDo='' FROM(
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,19999) as sTenDuAn,iID_MaNguonNganSach,
SUM(rTongMucDauTu/1000000) as rTongMucDauTu,
SUM(rTongDuToan/1000000) as rTongDuToan,
SUM(rDenNamTruoc/1000000) as rDenNamTruoc,
SUM(rNamTruoc/1000000) as rNamTruoc,
SUM(rKHVUngTruoc/1000000) as rKHVUngTruoc,
SUM(rDonViDeNghi/1000000) as rDonViDeNghi,
SUM(rDonViThuTamUng/1000000) as rDonViThuTamUng,
SUM(rDonViThu/1000000) as rDonViThu,
SUM(rCucTaiChinhDeNghi/1000000) as rCucTaiChinhDeNghi,
SUM(rCucTaiChinhThuTamUng/1000000) as rCucTaiChinhThuTamUng,
SUM(rCucTaiChinhThuTamUng_NamTruoc/1000000) as rCucTaiChinhThuTamUng_NamTruoc,
SUM(rCucTaiChinhThu/1000000) as rCucTaiChinhThu
FROM
(
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,
	SUM(rNgoaiTe) as rTongMucDauTu,
	rTongDuToan=0,
	rDenNamTruoc=0,
	rNamTruoc=0,
	rKHVUngTruoc=0,
	rDonViDeNghi=0,
	rDonViThuTamUng=0,
	rDonViThu=0,
	rCucTaiChinhDeNghi=0,
	rCucTaiChinhThuTamUng=0,
    rCucTaiChinhThuTamUng_NamTruoc=0,
	rCucTaiChinhThu=0
	 FROM QLDA_TongDauTu WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec AND sDeAn IN ({0}) AND iID_MaNgoaiTe=@iID_MaNgoaiTe
	GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS

UNION 

SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,rTongMucDauTu=0,
	 SUM(rNgoaiTe) as rTongDuToan ,
	 rDenNamTruoc=0,
	 rNamTruoc=0,
	rKHVUngTruoc=0,
	rDonViDeNghi=0,
	rDonViThuTamUng=0,
	rDonViThu=0,
	rCucTaiChinhDeNghi=0,
	rCucTaiChinhThuTamUng=0,
    rCucTaiChinhThuTamUng_NamTruoc=0,
	rCucTaiChinhThu=0
	 FROM QLDA_TongDuToan WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec AND sDeAn IN ({0}) AND iID_MaNgoaiTe=@iID_MaNgoaiTe
	GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS
--KHV
UNION
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,
rTongMucDauTu=0,
rTongDuToan=0,
rDenNamTruoc=SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec<=@iNamTruoc THEN (rNgoaiTe_DauNam+rNgoaiTe_DieuChinh) ELSE 0 END),
rNamTruoc=SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec=@iNamTruoc THEN (rNgoaiTe_DauNam+rNgoaiTe_DieuChinh) ELSE 0 END),
rKHVUngTruoc=SUM(CASE WHEN iLoaiKeHoachVon=2 AND iNamLamViec<=@iNamLamViec THEN (rNgoaiTe_DauNam+rNgoaiTe_DieuChinh) ELSE 0 END),
rDonViDeNghi=0,
rDonViThuTamUng=0,
rDonViThu=0,
rCucTaiChinhDeNghi=0,
rCucTaiChinhThuTamUng=0,
rCucTaiChinhThuTamUng_NamTruoc=0,
rCucTaiChinhThu=0
 FROM QLDA_KeHoachVon
WHERE iTrangThai=1  AND sDeAn IN ({0}) AND ( iID_MaNgoaiTe_DauNam=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DieuChinh=@iID_MaNgoaiTe)
  GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS
UNION 
--DT_nam 
SELECT sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,SUBSTRING(sLNS,1,1) as iID_MaNguonNganSach,
rTongMucDauTu=0,
rTongDuToan=0,
rDenNamTruoc=0,
rNamTruoc=0,
rKHVUngTruoc=0,
rDonViDeNghi=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rNgoaiTe_rDonViDeNghi ELSE 0 END),
rDonViThuTamUng=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rNgoaiTe_rDonViThuTamUng ELSE 0 END),
rDonViThu=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rNgoaiTe_rDonViThu ELSE 0 END),
rCucTaiChinhDeNghi=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rNgoaiTe_rCucTaiChinhDeNghi ELSE 0 END),
rCucTaiChinhThuTamUng=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rNgoaiTe_rCucTaiChinhThuTamUng ELSE 0 END),
rCucTaiChinhThuTamUng_NamTruoc=SUM(CASE WHEN (iNamLamViec<@iNamLamViec) THEN  rNgoaiTe_rCucTaiChinhThuTamUng ELSE 0 END),
rCucTaiChinhThu=SUM(CASE WHEN (iNamLamViec=@iNamLamViec) THEN  rNgoaiTe_rCucTaiChinhThu ELSE 0 END)
 FROM QLDA_DuToan_Nam 
WHERE iTrangThai=1 AND sDeAn IN ({0}) AND ( iID_MaNgoaiTe_rDonViDeNghi=@iID_MaNgoaiTe OR   iID_MaNgoaiTe_rDonViThuTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_rDonViThu=@iID_MaNgoaiTe OR  iID_MaNgoaiTe_rCucTaiChinhDeNghi=@iID_MaNgoaiTe OR   iID_MaNgoaiTe_rCucTaiChinhThuTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_rCucTaiChinhThu=@iID_MaNgoaiTe )
 GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sLNS) as A
 GROUP BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaNguonNganSach
 HAVING SUM(rTongMucDauTu)<>0
 OR SUM(rTongDuToan) <>0
 OR SUM(rDenNamTruoc) <>0
 OR SUM(rNamTruoc) <>0
 OR SUM(rKHVUngTruoc) <>0
 OR SUM(rDonViDeNghi) <>0
 OR SUM(rDonViThuTamUng) <>0
 OR SUM(rDonViThu) <>0
 OR SUM(rCucTaiChinhDeNghi) <>0
 OR SUM(rCucTaiChinhThuTamUng) <>0
 OR SUM(rCucTaiChinhThu) <>0) as QLDA
INNER JOIN (SELECT iID_MaNguonNganSach,sTen FROM NS_NguonNganSach) as NS_NguonNganSach
ON QLDA.iID_MaNguonNganSach=NS_NguonNganSach.iID_MaNguonNganSach
", dsDeAn);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt32(iNamLamViec) - 1);
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iLoaiNgoaiTe);
                dt = Connection.GetDataTable(cmd);
            }
            return dt;
        }
    }

}