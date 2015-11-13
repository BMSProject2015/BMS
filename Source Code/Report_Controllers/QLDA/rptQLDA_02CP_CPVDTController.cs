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
    public class rptQLDA_02CP_CPVDTController : Controller
    {
        //
        // GET: /rptQLDA_01DT/


        private const String sFilePath = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT.xls";
        private const String sFilePathDuAn = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_DA.xls";
        private const String sFilePathCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_CT.xls";
        private const String sFilePathDuAnTP = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_DATP.xls";
        private const String sFilePathHMCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_HMCT.xls";
        private const String sFilePathHMChiTiet = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT.xls";
        public static String NameFile = "";
        public string sViewPath = "~/Report_Views/";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02CP_CPVDT.aspx";
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
            String dNgayBaoCao = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayBaoCao"];
            if (String.IsNullOrEmpty(dNgayBaoCao))
            {
                int i;
                var arrLoi = new NameValueCollection();
                arrLoi.Add(ParentID + "_err_dNgayBaoCao", "Bạn chưa nhập ngày báo cáo!");
                if (arrLoi.Count > 0)
                {
                    for (i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["LoadPage"] = 1;
                }
            }
            else
            {
                ViewData["LoadPage"] = 1;
            }
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02CP_CPVDT.aspx";
            ViewData["dsDeAn"] = iID_MaDeAn;
            ViewData["iCapTongHop"] = iCapTongHop;
            ViewData["iID_MaNgoaiTe"] = iLoaiNgoaiTe;
            ViewData["dNgayBaoCao"] = dNgayBaoCao;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String NamLamViec, DateTime dNgayNhap, String iID_MaNgoaiTe, String dsDeAn,
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
                        path = Server.MapPath( sFilePathCongTrinh);
                        Result.Open(path);
                        break;
                    }
               
                case CapTongHop.HMCongTrinh:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathHMCongTrinh);
                        Result.Open(path);
                        break;
                    }
                case CapTongHop.HMChiTiet:
                    {
                        //Do something
                        path = Server.MapPath(sFilePathHMChiTiet);
                        Result.Open(path);
                        break;
                    }
                default:
                    break;
            }
            //Set những giá trị chung cho các loại báo cáo
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_02CP_CPVDT");
            LoadData(fr, NamLamViec, dNgayNhap, iID_MaNgoaiTe, dsDeAn);
            fr.SetValue("NgayThang", ngay);
            string ngayNhap = String.Format(@"ngày {0} tháng {1} năm {2}",dNgayNhap.Day,dNgayNhap.Month,dNgayNhap.Year);
            fr.SetValue("BaoCaoToiNgay", ngayNhap);
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
            fr.SetValue("DonViTinh",strDonViTinh);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Hàm hiển thị dữ liệu báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec, DateTime dNgayNhap, String iLoaiNgoaiTe,
                              String dsDeAn)
        {
            DataTable data = dtCPV(NamLamViec, dNgayNhap, iLoaiNgoaiTe, dsDeAn);
            //if (data == null || data.Rows.Count == 0)
            //{
            //    return;
            //}
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            //Lấy danh sách Hạ mục công trình
            DataTable dtHmct = HamChung.SelectDistinct_QLDA("dtHmct", data,"sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh",
                                                            "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo",
                                                            "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            dtHmct.TableName = "HaMucCongTrinh";
            fr.AddTable("HaMucCongTrinh", dtHmct);

            DataTable CongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHmct, "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh",
                                                               "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo",
                                                               "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            CongTrinh.TableName = "CongTrinh";
            fr.AddTable("CongTrinh", CongTrinh);

            DataTable DuAnTP = HamChung.SelectDistinct_QLDA("DuAnTP", CongTrinh, "sTen,sDeAn,sDuAn,sDuAnThanhPhan",
                                                            "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo",
                                                            "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            DuAnTP.TableName = "DuAnTP";
            fr.AddTable("DuAnTP", DuAnTP);

            DataTable DuAn = HamChung.SelectDistinct_QLDA("DuAn", DuAnTP, "sTen,sDeAn,sDuAn", "sTen,sDeAn,sDuAn,sTenDuAn,sTienDo",
                                                          "sDeAn,sDuAn,sDuAnThanhPhan");
            DuAn.TableName = "DuAn";
            fr.AddTable("DuAn", DuAn);

            DataTable DeAn = HamChung.SelectDistinct_QLDA("DeAn", data, "sTen,sDeAn", "sTen,sDeAn,sTenDuAn,sTienDo",
                                                        "sDeAn,sDuAn");
            DeAn.TableName = "DeAn";
            fr.AddTable("DeAn", DeAn);

            DataTable NguonNganSach = HamChung.SelectDistinct("NguonNganSach", data, "sTen", "sTen,sTienDo",
                                                         "sTen");
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
        //public clsExcelResult ExportToPDF(String NamLamViec)
        //{
        //    clsExcelResult clsResult = new clsExcelResult();
        //    ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
        //    using (FlexCelPdfExport pdf = new FlexCelPdfExport())
        //    {
        //        pdf.Workbook = xls;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            pdf.BeginExport(ms);
        //            pdf.ExportAllVisibleSheets(false, "AA");
        //            pdf.EndExport();
        //            ms.Position = 0;
        //            clsResult.FileName = "Test.pdf";
        //            clsResult.type = "pdf";
        //            clsResult.ms = ms;
        //            return clsResult;
        //        }
        //    }
        //}
        public clsExcelResult ExportToExcel(String NamLamViec, String strNgayNhap, String iID_MaNgoaiTe, String dsDeAn,String iCapTongHop)
        {
            try
            {
                HamChung.Language();
                clsExcelResult clsResult = new clsExcelResult();
                DateTime dNgayNhap = Convert.ToDateTime(strNgayNhap);
                ExcelFile xls = CreateReport(NamLamViec, dNgayNhap, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
                using (MemoryStream ms = new MemoryStream())
                {
                    xls.Save(ms);
                    ms.Position = 0;
                    clsResult.ms = ms;
                    clsResult.FileName = "rptQLDA_02CP_CPVDT.xls";
                    clsResult.type = "xls";
                    return clsResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String strNgayNhap, String iID_MaNgoaiTe, String dsDeAn,
                                    String iCapTongHop)
        {
            HamChung.Language();
           
                DateTime dNgayNhap = Convert.ToDateTime(strNgayNhap);
                ExcelFile xls = CreateReport(NamLamViec, dNgayNhap, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
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
                    @"select dma.iID_MaDanhMucDuAn,dma.sDeAn,dma.sTenDuAn from (
                    select sDeAn from dbo.QLDA_TongDauTu 
                    where iNamLamViec = @iNamLamViec
                    group by sDeAn)tbl1
                    join dbo.QLDA_DanhMucDuAn dma on dma.sDeAn = tbl1.sDeAn and (dma.sDuan = '' or dma.sDuAn is null) and dma.itrangthai = 1");

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
            //stb.Append("<tr>");
            //stb.Append("<th align=\"left\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAllNgoaiTe(this.checked)\" /></th>");
            //stb.Append("<th></th>");
            //stb.Append("</tr>");
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
                    //stb.Append("<input type=\"checkbox\" value=\"" + maNgoaiTe + "\" check-group=\"iID_MaNgoaiTe\" id=\"id_MaTaiKhoan\" name=\"id_MaTaiKhoan\" />");
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
            //Lấy danh sách kế hoạch vốn năm nay iLoaiKeHoachVon = 1
//            String query = string.Format(@"SELECT iID_MaDanhMucDuAn, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
//                                          FROM [QLDA_KeHoachVon]  where  iNamLamViec = @iNamLamViec and  iTrangThai = 1 
//                                          and dNgayKeHoachVon < @dNgayKeHoachVon and sDeAn in (@dsDeAn)
//                                          and iLoaiKeHoachVon = 1 and iID_MaNgoaiTe_DauNam = @iLoaiNgoaiTe and iID_MaNgoaiTe_DieuChinh = @iLoaiNgoaiTe
//                                          Group by iID_MaDanhMucDuAn");
            //Lấy danh sách kế hoạch vốn năm trước
//            query =
//                string.Format(
//                    @"
//Select tbl5.iID_MaDanhMucDuAn, sum(tbl5.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl5.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl5.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl5.rNgoaiTe_VonTamUngChuaThuHoiNamNay) rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl5.rNgoaiTe_ChuDauTuThanhToanNamNay) rNgoaiTe_ChuDauTuThanhToanNamNay , sum( tbl6.rNgoaiTe_ChuDauTuTamUng - tbl6.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamTruoc , sum(tbl6.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamTruoc 
//From
//	(Select tbl3.iID_MaDanhMucDuAn,sum(tbl3.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl3.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl3.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl4.rNgoaiTe_ChuDauTuTamUng - tbl4.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl4.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamNay 
//	from 
//		(select tbl1.iID_MaDanhMucDuAn, sum( tbl2.rNgoaiTe_ChuDauTuTamUng - tbl2.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoi,sum(tbl1.rNgoaiTe_SoTienDauNam) rNgoaiTe_SoTienDauNam, sum(tbl1.rNgoaiTe_DieuChinh)rNgoaiTe_DieuChinh 
//		from
//			(SELECT iID_MaDanhMucDuAn, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
//				FROM [QLDA_KeHoachVon]  
//				where  iNamLamViec = 2012 and  iTrangThai = 1 
//				and dNgayKeHoachVon <= '2012-09-18 00:00:00.000' and sDeAn in (1,2)
//				and iLoaiKeHoachVon = 1 and iID_MaNgoaiTe_DauNam = 1 and iID_MaNgoaiTe_DieuChinh = 1
//			Group by iID_MaDanhMucDuAn)  tbl1  
//		left join dbo.QLDA_CapPhat tbl2 on (tbl1.iID_MaDanhMucDuAn = tbl2.iID_MaDanhMucDuAn and tbl2.iID_MaLoaiKeHoachVon = 2 and tbl2.iNamLamViec = 2012 and tbl2.dNgayLap <= '2012-09-18 00:00:00.000' and tbl2.iID_MaNgoaiTe_ChuDauTuTamUng = 1 and tbl2.iID_MaNgoaiTe_ChuDauTuThuTamUng = 1)
//		Group by tbl1.iID_MaDanhMucDuAn) tbl3
//	left join dbo.QLDA_CapPhat tbl4 on (tbl3.iID_MaDanhMucDuAn =  tbl4.iID_MaDanhMucDuAn and tbl4.iID_MaLoaiKeHoachVon = 1 and tbl4.iNamLamViec = 2012 and tbl4.iTrangThai = 1 and tbl4.dNgayLap <= '2012-09-18 00:00:00.000' and tbl4.iID_MaNgoaiTe_ChuDauTuTamUng = 1 and tbl4.iID_MaNgoaiTe_ChuDauTuThuTamUng = 1 and tbl4.iID_MaNgoaiTe_ChuDauTuThanhToan = 1)
//	Group by tbl3.iID_MaDanhMucDuAn) tbl5
//left join dbo.QLDA_CapPhat tbl6 on (tbl5.iID_MaDanhMucDuAn =  tbl6.iID_MaDanhMucDuAn and tbl6.iID_MaLoaiKeHoachVon = 2 and tbl6.iNamLamViec = 2012 and tbl6.iTrangThai = 1 and tbl6.dNgayLap <= '2012-09-18 00:00:00.000' and tbl6.iID_MaNgoaiTe_ChuDauTuTamUng = 1 and tbl6.iID_MaNgoaiTe_ChuDauTuThuTamUng = 1 and tbl6.iID_MaNgoaiTe_ChuDauTuThanhToan = 1)
//Group by tbl5.iID_MaDanhMucDuAn");
            String query =
                string.Format(
                    @"
Select ns.sTen,tbl7.*,dmda.sDeAn,dmda.sDuAn,dmda.sDuAnThanhPhan,dmda.sCongTrinh,dmda.sHangMucCongTrinh,dmda.sHangMucChiTiet,dmda.sTenDuAn, '' sTienDo
From 
	(Select tbl5.iID_MaDanhMucDuAn,tbl5.iID_MaNguonNganSach, sum(tbl5.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl5.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl5.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl5.rNgoaiTe_VonTamUngChuaThuHoiNamNay) rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl5.rNgoaiTe_ChuDauTuThanhToanNamNay) rNgoaiTe_ChuDauTuThanhToanNamNay , sum( tbl6.rNgoaiTe_ChuDauTuTamUng - tbl6.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamTruoc , sum(tbl6.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamTruoc 
	From
		(Select tbl3.iID_MaDanhMucDuAn,tbl3.iID_MaNguonNganSach,sum(tbl3.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl3.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl3.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl4.rNgoaiTe_ChuDauTuTamUng - tbl4.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl4.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamNay 
		from 
			(select tbl1.iID_MaDanhMucDuAn,tbl1.iID_MaNguonNganSach, sum( tbl2.rNgoaiTe_ChuDauTuTamUng - tbl2.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoi,sum(tbl1.rNgoaiTe_SoTienDauNam) rNgoaiTe_SoTienDauNam, sum(tbl1.rNgoaiTe_DieuChinh)rNgoaiTe_DieuChinh 
			from
				(SELECT iID_MaDanhMucDuAn,iID_MaNguonNganSach, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
					FROM [QLDA_KeHoachVon]  
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

        #region Nested type: CapTongHop

        public static class CapTongHop
        {
            public const string DuAn = "0";
            public const string DuAnTP = "1";
            public const string CongTrinh = "2";
            public const string HMCongTrinh = "3";
            public const string HMChiTiet = "4";
        }

        public static class DonViTinh
        {
            public const string VND = "0";
            public const string USD = "1";
            public const string EUR = "2";
            public const string JPY = "3";
        }
        #endregion
    }
}