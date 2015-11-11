//using System;
//using System.Collections.Specialized;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;
//using System.Net.Mime;
//using System.Text;
//using System.Web.Mvc;
//using DomainModel;
//using FlexCel.Core;
//using FlexCel.Render;
//using FlexCel.Report;
//using FlexCel.XlsAdapter;
//using VIETTEL.Controllers;
//using VIETTEL.Models;

//namespace VIETTEL.Report_Controllers.QLDA
//{
//    public class rptQLDA_02CP_CPVDTController : Controller
//    {
//        //
//        // GET: /rptQLDA_01DT/


//        private const String sFilePath = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT.xls";
//        private const String sFilePathDuAn = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_DA.xls";
//        private const String sFilePathCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_CT.xls";
//        private const String sFilePathDuAnTP = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_DATP.xls";
//        private const String sFilePathHMCongTrinh = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT_HMCT.xls";
//        private const String sFilePathHMChiTiet = "~/Report_ExcelFrom/QLDA/rptQLDA_02CP_CPVDT.xls";
//        public static String NameFile = "";
//        public string sViewPath = "~/Report_Views/";

//        public ActionResult Index()
//        {
//            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02CP_CPVDT.aspx";
//            ViewData["FilePath"] = Server.MapPath(sFilePath);
//            ViewData["srcFile"] = NameFile;
//            ViewData["LoadPage"] = 0;
//            return View(sViewPath + "ReportView.aspx");
//        }

//        /// <summary>
//        /// Hàm lấy các giá trị trên form
//        /// </summary>
//        /// <param name="ParentID"></param>
//        /// <returns></returns>
//        public ActionResult EditSubmit(String ParentID)
//        {
//            String iID_MaDeAn = Convert.ToString(Request.Form["iID_MaDanhMucDuAn"]);
//            String iCapTongHop = Convert.ToString(Request.Form[ParentID + "_iCapTongHop"]);
//            String iLoaiNgoaiTe = Convert.ToString(Request.Form["iID_MaNgoaiTe"]);
//            String dNgayBaoCao = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayBaoCao"];
//            if (String.IsNullOrEmpty(dNgayBaoCao))
//            {
//                int i;
//                var arrLoi = new NameValueCollection();
//                arrLoi.Add(ParentID + "_err_dNgayBaoCao", "Bạn chưa nhập ngày báo cáo!");
//                if (arrLoi.Count > 0)
//                {
//                    for (i = 0; i <= arrLoi.Count - 1; i++)
//                    {
//                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
//                    }
//                    ViewData["LoadPage"] = 1;
//                }
//            }
//            else
//            {
//                ViewData["LoadPage"] = 1;
//            }
//            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02CP_CPVDT.aspx";
//            ViewData["dsDeAn"] = iID_MaDeAn;
//            ViewData["iCapTongHop"] = iCapTongHop;
//            ViewData["iID_MaNgoaiTe"] = iLoaiNgoaiTe;
//            ViewData["dNgayBaoCao"] = dNgayBaoCao;
//            return View(sViewPath + "ReportView.aspx");
//        }

//        /// <summary>
//        /// Hàm khởi tạo báo cáo
//        /// </summary>
//        /// <param name="path"></param>
//        /// <param name="NamLamViec"></param>
//        /// <returns></returns>
//        public ExcelFile CreateReport(String NamLamViec, DateTime dNgayNhap, String iID_MaNgoaiTe, String dsDeAn,
//                                      String iCapTongHop)
//        {
//            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
//            var Result = new XlsFile(true);
//            String path = String.Empty;
//            var fr = new FlexCelReport();
//            switch (iCapTongHop)
//            {
//                case CapTongHop.DuAn:
//                    {
//                        //Do something
//                        path = Server.MapPath(sFilePathDuAn);
//                        Result.Open(path);
//                        break;
//                    }
//                case CapTongHop.DuAnTP:
//                    {
//                        //Do something
//                        path = Server.MapPath(sFilePathDuAnTP);
//                        Result.Open(path);
//                        break;
//                    }
//                case CapTongHop.CongTrinh:
//                    {
//                        //Do something
//                        path = Server.MapPath( sFilePathCongTrinh);
//                        Result.Open(path);
//                        break;
//                    }
               
//                case CapTongHop.HMCongTrinh:
//                    {
//                        //Do something
//                        path = Server.MapPath(sFilePathHMCongTrinh);
//                        Result.Open(path);
//                        break;
//                    }
//                case CapTongHop.HMChiTiet:
//                    {
//                        //Do something
//                        path = Server.MapPath(sFilePathHMChiTiet);
//                        Result.Open(path);
//                        break;
//                    }
//                default:
//                    break;
//            }
//            //Set những giá trị chung cho các loại báo cáo
//            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_02CP_CPVDT");
//            LoadData(fr, NamLamViec, dNgayNhap, iID_MaNgoaiTe, dsDeAn);
//            fr.SetValue("NgayThang", ngay);
//            string ngayNhap = String.Format(@"ngày {0} tháng {1} năm {2}",dNgayNhap.Day,dNgayNhap.Month,dNgayNhap.Year);
//            fr.SetValue("BaoCaoToiNgay", ngayNhap);
//            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
//            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3));
//            String strDonViTinh = string.Empty;
//            switch (iID_MaNgoaiTe)
//            {
//                case DonViTinh.VND:
//                    strDonViTinh = "VND";
//                    break;
//                case DonViTinh.USD:
//                    strDonViTinh = "USD";
//                    break;
//                case DonViTinh.EUR:
//                    strDonViTinh = "EUR";
//                    break;
//                case DonViTinh.JPY:
//                    strDonViTinh = "JPY";
//                    break;
//            }
//            fr.SetValue("DonViTinh",strDonViTinh);
//            fr.Run(Result);
//            return Result;
//        }

//        /// <summary>
//        /// Hàm hiển thị dữ liệu báo cáo
//        /// </summary>
//        /// <param name="fr"></param>
//        /// <param name="NamLamViec"></param>
//        private void LoadData(FlexCelReport fr, String NamLamViec, DateTime dNgayNhap, String iLoaiNgoaiTe,
//                              String dsDeAn)
//        {
//            DataTable data = dtCPV(NamLamViec, dNgayNhap, iLoaiNgoaiTe, dsDeAn);
//            //if (data == null || data.Rows.Count == 0)
//            //{
//            //    return;
//            //}
//            data.TableName = "ChiTiet";
//            fr.AddTable("ChiTiet", data);
//            //Lấy danh sách Hạ mục công trình
//            DataTable dtHmct = HamChung.SelectDistinct_QLDA("dtHmct", data,"sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh",
//                                                            "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo",
//                                                            "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
//            dtHmct.TableName = "HaMucCongTrinh";
//            fr.AddTable("HaMucCongTrinh", dtHmct);

//            DataTable CongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHmct, "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh",
//                                                               "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo",
//                                                               "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
//            CongTrinh.TableName = "CongTrinh";
//            fr.AddTable("CongTrinh", CongTrinh);

//            DataTable DuAnTP = HamChung.SelectDistinct_QLDA("DuAnTP", CongTrinh, "sTen,sDeAn,sDuAn,sDuAnThanhPhan",
//                                                            "sTen,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo",
//                                                            "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
//            DuAnTP.TableName = "DuAnTP";
//            fr.AddTable("DuAnTP", DuAnTP);

//            DataTable DuAn = HamChung.SelectDistinct_QLDA("DuAn", DuAnTP, "sTen,sDeAn,sDuAn", "sTen,sDeAn,sDuAn,sTenDuAn,sTienDo",
//                                                          "sDeAn,sDuAn,sDuAnThanhPhan");
//            DuAn.TableName = "DuAn";
//            fr.AddTable("DuAn", DuAn);

//            DataTable DeAn = HamChung.SelectDistinct_QLDA("DeAn", data, "sTen,sDeAn", "sTen,sDeAn,sTenDuAn,sTienDo",
//                                                        "sDeAn,sDuAn");
//            DeAn.TableName = "DeAn";
//            fr.AddTable("DeAn", DeAn);

//            DataTable NguonNganSach = HamChung.SelectDistinct("NguonNganSach", data, "sTen", "sTen,sTienDo",
//                                                         "sTen");
//            DuAn.TableName = "NguonNganSach";
//            fr.AddTable("NguonNganSach", NguonNganSach);

//            CongTrinh.Dispose();
//            DuAn.Dispose();
//            DuAnTP.Dispose();
//            data.Dispose();
//            dtHmct.Dispose();
//            NguonNganSach.Dispose();
//        }

//        /// <summary>
//        /// Ham xuất dữ liệu ra file PDF
//        /// </summary>
//        /// <param name="NamLamViec"></param>
//        /// <param name="iID_MaDonVi"></param>
//        /// <returns></returns>
//        //public clsExcelResult ExportToPDF(String NamLamViec)
//        //{
//        //    clsExcelResult clsResult = new clsExcelResult();
//        //    ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
//        //    using (FlexCelPdfExport pdf = new FlexCelPdfExport())
//        //    {
//        //        pdf.Workbook = xls;
//        //        using (MemoryStream ms = new MemoryStream())
//        //        {
//        //            pdf.BeginExport(ms);
//        //            pdf.ExportAllVisibleSheets(false, "AA");
//        //            pdf.EndExport();
//        //            ms.Position = 0;
//        //            clsResult.FileName = "Test.pdf";
//        //            clsResult.type = "pdf";
//        //            clsResult.ms = ms;
//        //            return clsResult;
//        //        }
//        //    }
//        //}
//        public clsExcelResult ExportToExcel(String NamLamViec, String strNgayNhap, String iID_MaNgoaiTe, String dsDeAn,String iCapTongHop)
//        {
//            try
//            {
//                HamChung.Language();
//                clsExcelResult clsResult = new clsExcelResult();
//                DateTime dNgayNhap = Convert.ToDateTime(strNgayNhap);
//                ExcelFile xls = CreateReport(NamLamViec, dNgayNhap, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    xls.Save(ms);
//                    ms.Position = 0;
//                    clsResult.ms = ms;
//                    clsResult.FileName = "rptQLDA_02CP_CPVDT.xls";
//                    clsResult.type = "xls";
//                    return clsResult;
//                }
//            }
//            catch (Exception)
//            {
//                return null;
//            }
           
//        }
//        /// <summary>
//        /// Hàm View PDF
//        /// </summary>
//        /// <param name="NamLamViec"></param>
//        /// <returns></returns>
//        public ActionResult ViewPDF(String NamLamViec, String strNgayNhap, String iID_MaNgoaiTe, String dsDeAn,
//                                    String iCapTongHop)
//        {
//            HamChung.Language();
           
//                DateTime dNgayNhap = Convert.ToDateTime(strNgayNhap);
//                ExcelFile xls = CreateReport(NamLamViec, dNgayNhap, iID_MaNgoaiTe, dsDeAn, iCapTongHop);
//                using (var pdf = new FlexCelPdfExport())
//                {
//                    pdf.Workbook = xls;
//                    using (var ms = new MemoryStream())
//                    {
//                        pdf.BeginExport(ms);
//                        pdf.ExportAllVisibleSheets(false, "BaoCao");
//                        pdf.EndExport();
//                        ms.Position = 0;
//                        return File(ms.ToArray(), "application/pdf");
//                    }
//                }
           

//            return null;
//        }

//        public string sDanhSachDeAn(string strDsDuAn, string iNamLamViec)
//        {
//            DataTable dtDeAn = dsDeAn(iNamLamViec);
//            var stb = new StringBuilder();
//            stb.Append("<table  class=\"mGrid\">");
//            stb.Append("<tr>");
//            stb.Append(
//                "<th align=\"left\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAll(this.checked)\" /></th>");
//            stb.Append("<th></th>");
//            stb.Append("</tr>");

//            String strsTen = "", maDuAn = "";
//            if (dtDeAn != null)
//            {
//                for (int i = 0; i < dtDeAn.Rows.Count; i++)
//                {
//                    strsTen = Convert.ToString(dtDeAn.Rows[i]["sTenDuAn"]);
//                    maDuAn = Convert.ToString(dtDeAn.Rows[i]["sDeAn"]);
//                    string ckh = string.Empty;
//                    if (strDsDuAn.Contains(maDuAn))
//                    {
//                        ckh = "checked = 'checked'";
//                    }
//                    stb.Append("<tr>");
//                    stb.Append("<td align=\"left\">");
//                    stb.Append("<input type=\"checkbox\" value=\"" + maDuAn +
//                               "\" check-group=\"iID_MaDanhMucDuAn\" id=\"iID_MaDanhMucDuAn\" name=\"iID_MaDanhMucDuAn\" " +
//                               ckh + "/>");
//                    stb.Append("</td>");
//                    stb.Append("<td align=\"left\">" + strsTen);
//                    stb.Append("</td>");
//                    stb.Append("</tr>");
//                }
//            }

//            stb.Append("</table>");
//            return stb.ToString();
//        }

//        public DataTable dsNgoaiTe()
//        {
//            var dt = new DataTable();
//            string query =
//                string.Format(
//                    @"select iID_MaNgoaiTe,sTen from dbo.QLDA_NgoaiTe where iTrangThai = 1");
//            try
//            {
//                dt = Connection.GetDataTable(query);
//                DataRow dr = dt.NewRow();
//                dr["iID_MaNgoaiTe"] = 0;
//                dr["sTen"] = "VND";
//                dt.Rows.InsertAt(dr, 0);

//            }
//            catch (Exception)
//            {
//                return dt;
//            }
//            return dt;
//        }

//        public DataTable dsDeAn(string iNamLamViec)
//        {
//            var dt = new DataTable();
//            string query =
//                string.Format(
//                      @"SELECT DISTINCT sDeAn,sTenDuAn
// FROM QLDA_DanhMucDuAn
// WHERE sDuAn='' AND iTrangThai=1");

//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                cmd.CommandText = query;
//                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
//                dt = Connection.GetDataTable(cmd);
//            }
//            catch (Exception)
//            {
//                return dt;
//            }
//            return dt;
//        }

//        public string sDanhSachNgoaiTe(string iMaNgoaite)
//        {
//            DataTable dtNgoaiTe = dsNgoaiTe();
//            var stb = new StringBuilder();
//            stb.Append("<table  class=\"mGrid\">");
//            //stb.Append("<tr>");
//            //stb.Append("<th align=\"left\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAllNgoaiTe(this.checked)\" /></th>");
//            //stb.Append("<th></th>");
//            //stb.Append("</tr>");
//            String strsTen = "", maNgoaiTe = "";
//            if (dtNgoaiTe != null)
//            {
//                for (int i = 0; i < dtNgoaiTe.Rows.Count; i++)
//                {
//                    strsTen = Convert.ToString(dtNgoaiTe.Rows[i]["sTen"]);
//                    maNgoaiTe = Convert.ToString(dtNgoaiTe.Rows[i]["iID_MaNgoaiTe"]);
//                    stb.Append("<tr>");
//                    stb.Append("<td align=\"right\">");
//                    string chk = string.Empty;
//                    if (maNgoaiTe.Trim().Equals(iMaNgoaite.Trim()))
//                    {
//                        chk = " CHECKED ";
//                    }
//                    //stb.Append("<input type=\"checkbox\" value=\"" + maNgoaiTe + "\" check-group=\"iID_MaNgoaiTe\" id=\"id_MaTaiKhoan\" name=\"id_MaTaiKhoan\" />");
//                    stb.Append("<input type=\"radio\" value=\"" + maNgoaiTe +
//                               "\"  name=\"iID_MaNgoaiTe\" id=\"id_MaTaiKhoan\" " + chk + "/>");
//                    stb.Append("&nbsp;&nbsp;");
//                    stb.Append("</td>");
//                    stb.Append("<td align=\"left\">" + "&nbsp;&nbsp;" + maNgoaiTe + "-" + strsTen);
//                    stb.Append("</td>");
//                    stb.Append("</tr>");
//                }
//            }

//            stb.Append("</table>");
//            return stb.ToString();
//        }

//        public DataTable dtCPV(string iNamLamViec, DateTime dNgayNhap, string iLoaiNgoaiTe, string dsDeAn)
//        {
//            var dtCPV = new DataTable();
//            //Lấy danh sách kế hoạch vốn năm nay iLoaiKeHoachVon = 1
////            String query = string.Format(@"SELECT iID_MaDanhMucDuAn, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
////                                          FROM [QLDA_KeHoachVon]  where  iNamLamViec = @iNamLamViec and  iTrangThai = 1 
////                                          and dNgayKeHoachVon < @dNgayKeHoachVon and sDeAn in (@dsDeAn)
////                                          and iLoaiKeHoachVon = 1 and iID_MaNgoaiTe_DauNam = @iLoaiNgoaiTe and iID_MaNgoaiTe_DieuChinh = @iLoaiNgoaiTe
////                                          Group by iID_MaDanhMucDuAn");
//            //Lấy danh sách kế hoạch vốn năm trước
////            query =
////                string.Format(
////                    @"
////Select tbl5.iID_MaDanhMucDuAn, sum(tbl5.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl5.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl5.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl5.rNgoaiTe_VonTamUngChuaThuHoiNamNay) rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl5.rNgoaiTe_ChuDauTuThanhToanNamNay) rNgoaiTe_ChuDauTuThanhToanNamNay , sum( tbl6.rNgoaiTe_ChuDauTuTamUng - tbl6.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamTruoc , sum(tbl6.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamTruoc 
////From
////	(Select tbl3.iID_MaDanhMucDuAn,sum(tbl3.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl3.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl3.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl4.rNgoaiTe_ChuDauTuTamUng - tbl4.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl4.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamNay 
////	from 
////		(select tbl1.iID_MaDanhMucDuAn, sum( tbl2.rNgoaiTe_ChuDauTuTamUng - tbl2.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoi,sum(tbl1.rNgoaiTe_SoTienDauNam) rNgoaiTe_SoTienDauNam, sum(tbl1.rNgoaiTe_DieuChinh)rNgoaiTe_DieuChinh 
////		from
////			(SELECT iID_MaDanhMucDuAn, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
////				FROM [QLDA_KeHoachVon]  
////				where  iNamLamViec = 2012 and  iTrangThai = 1 
////				and dNgayKeHoachVon <= '2012-09-18 00:00:00.000' and sDeAn in (1,2)
////				and iLoaiKeHoachVon = 1 and iID_MaNgoaiTe_DauNam = 1 and iID_MaNgoaiTe_DieuChinh = 1
////			Group by iID_MaDanhMucDuAn)  tbl1  
////		left join dbo.QLDA_CapPhat tbl2 on (tbl1.iID_MaDanhMucDuAn = tbl2.iID_MaDanhMucDuAn and tbl2.iID_MaLoaiKeHoachVon = 2 and tbl2.iNamLamViec = 2012 and tbl2.dNgayLap <= '2012-09-18 00:00:00.000' and tbl2.iID_MaNgoaiTe_ChuDauTuTamUng = 1 and tbl2.iID_MaNgoaiTe_ChuDauTuThuTamUng = 1)
////		Group by tbl1.iID_MaDanhMucDuAn) tbl3
////	left join dbo.QLDA_CapPhat tbl4 on (tbl3.iID_MaDanhMucDuAn =  tbl4.iID_MaDanhMucDuAn and tbl4.iID_MaLoaiKeHoachVon = 1 and tbl4.iNamLamViec = 2012 and tbl4.iTrangThai = 1 and tbl4.dNgayLap <= '2012-09-18 00:00:00.000' and tbl4.iID_MaNgoaiTe_ChuDauTuTamUng = 1 and tbl4.iID_MaNgoaiTe_ChuDauTuThuTamUng = 1 and tbl4.iID_MaNgoaiTe_ChuDauTuThanhToan = 1)
////	Group by tbl3.iID_MaDanhMucDuAn) tbl5
////left join dbo.QLDA_CapPhat tbl6 on (tbl5.iID_MaDanhMucDuAn =  tbl6.iID_MaDanhMucDuAn and tbl6.iID_MaLoaiKeHoachVon = 2 and tbl6.iNamLamViec = 2012 and tbl6.iTrangThai = 1 and tbl6.dNgayLap <= '2012-09-18 00:00:00.000' and tbl6.iID_MaNgoaiTe_ChuDauTuTamUng = 1 and tbl6.iID_MaNgoaiTe_ChuDauTuThuTamUng = 1 and tbl6.iID_MaNgoaiTe_ChuDauTuThanhToan = 1)
////Group by tbl5.iID_MaDanhMucDuAn");
//            String query =
//                string.Format(
//                    @"
//Select ns.sTen,tbl7.*,dmda.sDeAn,dmda.sDuAn,dmda.sDuAnThanhPhan,dmda.sCongTrinh,dmda.sHangMucCongTrinh,dmda.sHangMucChiTiet,dmda.sTenDuAn, '' sTienDo
//From 
//	(Select tbl5.iID_MaDanhMucDuAn,tbl5.iID_MaNguonNganSach, sum(tbl5.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl5.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl5.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl5.rNgoaiTe_VonTamUngChuaThuHoiNamNay) rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl5.rNgoaiTe_ChuDauTuThanhToanNamNay) rNgoaiTe_ChuDauTuThanhToanNamNay , sum( tbl6.rNgoaiTe_ChuDauTuTamUng - tbl6.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamTruoc , sum(tbl6.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamTruoc 
//	From
//		(Select tbl3.iID_MaDanhMucDuAn,tbl3.iID_MaNguonNganSach,sum(tbl3.rNgoaiTe_VonTamUngChuaThuHoi) rNgoaiTe_VonTamUngChuaThuHoi, sum(tbl3.rNgoaiTe_SoTienDauNam)rNgoaiTe_SoTienDauNam,sum(tbl3.rNgoaiTe_DieuChinh) rNgoaiTe_DieuChinh, sum( tbl4.rNgoaiTe_ChuDauTuTamUng - tbl4.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoiNamNay , sum(tbl4.rNgoaiTe_ChuDauTuThanhToan) rNgoaiTe_ChuDauTuThanhToanNamNay 
//		from 
//			(select tbl1.iID_MaDanhMucDuAn,tbl1.iID_MaNguonNganSach, sum( tbl2.rNgoaiTe_ChuDauTuTamUng - tbl2.rNgoaiTe_ChuDauTuThuTamUng) as rNgoaiTe_VonTamUngChuaThuHoi,sum(tbl1.rNgoaiTe_SoTienDauNam) rNgoaiTe_SoTienDauNam, sum(tbl1.rNgoaiTe_DieuChinh)rNgoaiTe_DieuChinh 
//			from
//				(SELECT iID_MaDanhMucDuAn,iID_MaNguonNganSach, sum(rNgoaiTe_SoTienDauNam )as rNgoaiTe_SoTienDauNam , sum (rNgoaiTe_DieuChinh) as rNgoaiTe_DieuChinh
//					FROM [QLDA_KeHoachVon]  
//					where  iNamLamViec = @iNamLamViec and  iTrangThai = 1 
//					and dNgayKeHoachVon <= @dNgayNhap and sDeAn in ({0})
//					and iLoaiKeHoachVon = 1 and iID_MaNgoaiTe_DauNam = @iLoaiNgoaiTe and iID_MaNgoaiTe_DieuChinh = @iLoaiNgoaiTe
//				Group by iID_MaDanhMucDuAn,iID_MaNguonNganSach)  tbl1  
//			left join dbo.QLDA_CapPhat tbl2 on (tbl1.iID_MaDanhMucDuAn = tbl2.iID_MaDanhMucDuAn and tbl2.iID_MaLoaiKeHoachVon = 2 and tbl2.iNamLamViec = @iNamLamViec and tbl2.dNgayLap <= @dNgayNhap and tbl2.iID_MaNgoaiTe_ChuDauTuTamUng = @iLoaiNgoaiTe and tbl2.iID_MaNgoaiTe_ChuDauTuThuTamUng = @iLoaiNgoaiTe)
//			Group by tbl1.iID_MaDanhMucDuAn,tbl1.iID_MaNguonNganSach) tbl3
//		left join dbo.QLDA_CapPhat tbl4 on (tbl3.iID_MaDanhMucDuAn =  tbl4.iID_MaDanhMucDuAn and tbl4.iID_MaLoaiKeHoachVon = 1 and tbl4.iNamLamViec = @iNamLamViec and tbl4.iTrangThai = 1 and tbl4.dNgayLap <=  @dNgayNhap and tbl4.iID_MaNgoaiTe_ChuDauTuTamUng = @iLoaiNgoaiTe and tbl4.iID_MaNgoaiTe_ChuDauTuThuTamUng = @iLoaiNgoaiTe and tbl4.iID_MaNgoaiTe_ChuDauTuThanhToan = @iLoaiNgoaiTe)
//		Group by tbl3.iID_MaDanhMucDuAn,tbl3.iID_MaNguonNganSach) tbl5
//	left join dbo.QLDA_CapPhat tbl6 on (tbl5.iID_MaDanhMucDuAn =  tbl6.iID_MaDanhMucDuAn and tbl6.iID_MaLoaiKeHoachVon = 2 and tbl6.iNamLamViec = @iNamLamViec and tbl6.iTrangThai = 1 and tbl6.dNgayLap <=  @dNgayNhap and tbl6.iID_MaNgoaiTe_ChuDauTuTamUng = @iLoaiNgoaiTe and tbl6.iID_MaNgoaiTe_ChuDauTuThuTamUng = @iLoaiNgoaiTe and tbl6.iID_MaNgoaiTe_ChuDauTuThanhToan = @iLoaiNgoaiTe)
//	Group by tbl5.iID_MaDanhMucDuAn,tbl5.iID_MaNguonNganSach) tbl7
//join QLDA_DanhMucDuAn dmda on dmda.iID_MaDanhMucDuAn = tbl7.iID_MaDanhMucDuAn
//join NS_NguonNganSach ns on ns.iID_MaNguonNganSach = tbl7.iID_MaNguonNganSach",
//                    dsDeAn);
//            var cmd = new SqlCommand();
//            cmd.CommandText = query;
//            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
//            cmd.Parameters.AddWithValue("@dNgayNhap", dNgayNhap);
//            cmd.Parameters.AddWithValue("@iLoaiNgoaiTe", iLoaiNgoaiTe);
//            dtCPV = Connection.GetDataTable(cmd);
//            return dtCPV;
//        }

//        #region Nested type: CapTongHop

//        public static class CapTongHop
//        {
//            public const string DuAn = "0";
//            public const string DuAnTP = "1";
//            public const string CongTrinh = "2";
//            public const string HMCongTrinh = "3";
//            public const string HMChiTiet = "4";
//        }

//        public static class DonViTinh
//        {
//            public const string VND = "0";
//            public const string USD = "1";
//            public const string EUR = "2";
//            public const string JPY = "3";
//        }
//        #endregion
//    }
//}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using VIETTEL.Controllers;
using System.IO;
namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_02_CPVDTController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_02_CP.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02_CP.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_viiID_MaDotCapPhat"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["MaTien"] = MaTien;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_02_CP.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dt_rptQLDA_02_CP(String iID_MaDotCapPhat, String MaND, String MaTien)
        {
           // DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            String dNgayLap = "01/01/2000";
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            dNgayLap = iID_MaDotCapPhat;
            String dNam = "";
            if (dNgayLap != "01/01/2000")
                dNam = dNgayLap.Substring(6, 4);
            String NamLamViec = "2000";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            if (dtCauHinh.Rows.Count > 0)
            {
                NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String DK_KHV_NgoaiTe_rSoTienDauNam = "";
            String DK_KHV_NgoaiTe_rSoTienDieuChinh = "";
            String DK_KHV_LoaiNgoaiTe = "";
            String DK_CP_NgoaiTe_TamUng = "";
            String DK_CP_NgoaiTe_ThuTamUng = "";
            String DK_CP_NgoaiTe_ThanhToan = "";
            String DK_CP_LoaiNgoaiTe = "";
            if (MaTien == "0")
            {
                DK_KHV_NgoaiTe_rSoTienDauNam = "rSoTienDauNam/1000000";
                DK_KHV_NgoaiTe_rSoTienDieuChinh = "rSoTienDieuChinh/1000000";
                DK_CP_NgoaiTe_TamUng = "rDeNghiPheDuyetTamUng/1000000";
                DK_CP_NgoaiTe_ThuTamUng = "rDeNghiPheDuyetThuTamUng/1000000";
                DK_CP_NgoaiTe_ThanhToan = "rDeNghiPheDuyetThanhToan/1000000";

            }
            else
            {
                DK_KHV_NgoaiTe_rSoTienDauNam = "rNgoaiTe_SoTienDauNam";
                DK_KHV_NgoaiTe_rSoTienDieuChinh = "rNgoaiTe_SoTienDieuChinh";
                DK_CP_NgoaiTe_TamUng = "SUM(rNgoaiTe_DeNghiPheDuyetTamUng)-SUM(rNgoaiTe_DeNghiPheDuyetThuKhac)";
                DK_CP_NgoaiTe_ThanhToan = "rNgoaiTe_DeNghiPheDuyetThanhToan";
                DK_KHV_LoaiNgoaiTe = " (iID_MaNgoaiTe_SoTienDauNam=@iID_MaNgoaiTe OR iID_MaNgoaiTe_SoTienDieuChinh=@iID_MaNgoaiTe ) AND ";
                DK_CP_LoaiNgoaiTe = " (iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThuKhac=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThanhToan=@iID_MaNgoaiTe) AND ";
            }
            #region Tạo datatable kế hoạch vốn
            String SQLKHV = String.Format(@"SELECT	iID_MaDanhMucDuAn,
	                                        sDeAn,
	                                        sDuAn,
	                                        sDuAnThanhPhan,
	                                        sCongTrinh,
	                                        sHangMucCongTrinh,
                                            sHangMucChiTiet,
	                                        SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                        SUBSTRING(sLNS,1,1) as NguonNS,sLNS,
                                             rKeHoachVonNamTruoc=SUM(CASE WHEN iNamLamViec<=@iNamTruoc THEN {0}+{1} ELSE 0 END),
                                              rSoTienDauNam=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN {0} ELSE 0 END),
                                             rSoTienDieuChinh=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN {1} ELSE 0 END)
                                            FROM QLDA_KeHoachVon
                                            WHERE 
										          iTrangThai=1 AND {2}
                                                 dNgayKeHoachVon<=@dNgay  AND iLoaiKeHoachVon=1
                                            GROUP BY   iID_MaDanhMucDuAn,
											           sDeAn,
	                                                   sDuAn,
	                                                   sDuAnThanhPhan,
	                                                   sCongTrinh,
	                                                   sHangMucCongTrinh,
                                                       sHangMucChiTiet,
	                                                   SUBSTRING(sTenDuAn,19,10000),
	                                                   SUBSTRING(sLNS,1,1),sLNS
                                            HAVING SUM({0})<>0 OR SUM({1}) <>0 OR SUM(CASE WHEN iNamLamViec<=@iNamTruoc THEN {0}+{1} ELSE 0 END) <>0 
", DK_KHV_NgoaiTe_rSoTienDauNam, DK_KHV_NgoaiTe_rSoTienDieuChinh, DK_KHV_LoaiNgoaiTe);
            SqlCommand cmdKHV = new SqlCommand(SQLKHV);
            cmdKHV.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmdKHV.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(NamLamViec) - 1);

            cmdKHV.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgayLap));
            cmdKHV.Parameters.AddWithValue("@dNam", dNam);
            if (MaTien != "0")
            {
                cmdKHV.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dtKHV = Connection.GetDataTable(cmdKHV);
            cmdKHV.Dispose();
            #endregion
            #region tạo dt cấp phát

            String SQL = String.Format(@"
                                       SELECT * FROM(
                                                      SELECT iID_MaDanhMucDuAn,
			                                                 SUBSTRING(sLNS,1,1) as NguonNS,sLNS,
			                                                 -- cap phat Nam nay iLoai=1
			                                                 rDeNghiPheDuyetThanhToan_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {2} ELSE 0 END),
			                                                 rDeNghiPheDuyetThanhToan_NamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {2} ELSE 0 END),
			                                                rDeNghiPheDuyetTongThanhToan_NamTruoc=SUM(CASE WHEN (iNamLamViec<@iNamLamViec AND iID_MaLoaiKeHoachVon IN(1,3)) THEN {2} ELSE 0 END),
    
															rDeNghiPheDuyetTamUng_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {0} ELSE 0 END),
															rDeNghiPheDuyetTamUng_NamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {0} ELSE 0 END),
															
															rDeNghiPheDuyetThuTamUng_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {1} ELSE 0 END),
															rDeNghiPheDuyetThuTamUng_NamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {1} ELSE 0 END),
			                                               rCapLoaiNamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {0}-{1}+{2} ELSE 0 END),
                                                            rCapLoaiNamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {0}-{1}+{2} ELSE 0 END),
                                                        rSoDuTamUng_NamTruoc=SUM(CASE WHEN (iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1,3)) THEN   {0}-{1} ELSE 0 END)
             FROM QLDA_CapPhat
             WHERE iTrangThai=1 AND {3}
                   iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE iTrangThai=1 AND dNgayLap<=@dNgayLap )
GROUP BY 
					iID_MaDanhMucDuAn
					,SUBSTRING(sLNS,1,1),sLNS               
HAVING                                                         
SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {2} ELSE 0 END) <> 0
			                                                 OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {2} ELSE 0 END) <>0
			                                                OR SUM(CASE WHEN (iNamLamViec<@iNamLamViec AND iID_MaLoaiKeHoachVon IN(1,3)) THEN {2} ELSE 0 END)<>0
    
															 OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {0} ELSE 0 END)<>0
															 OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {0} ELSE 0 END)<>0
															
															OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {1} ELSE 0 END)<>0
															OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {1} ELSE 0 END)<>0
			                                               OR SUM(CASE WHEN (iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1,3)) THEN   {0}-{1} ELSE 0 END)  <>0           

           ) as CP
            INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                                                                                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTienDo 
                                                                                FROM QLDA_DanhMucDuAn 
                                                                                WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as DM
                                        ON CP.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn
            ", DK_CP_NgoaiTe_TamUng, DK_CP_NgoaiTe_ThuTamUng, DK_CP_NgoaiTe_ThanhToan, DK_CP_LoaiNgoaiTe);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(NamLamViec) - 1);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgayLap));
            if (MaTien != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            #endregion
            #region Ghép dtKHV vào dtCapPhat
            DataRow addR, R2;
            String sCol = "iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,NguonNS,sLNS,sTenDuAn,sHangMucChiTiet,rKeHoachVonNamTruoc,rSoTienDauNam,rSoTienDieuChinh";
            String[] arrCol = sCol.Split(',');
            dt.Columns.Add("rKeHoachVonNamTruoc", typeof(Decimal));
            dt.Columns.Add("rSoTienDauNam", typeof(Decimal));
            dt.Columns.Add("rSoTienDieuChinh", typeof(Decimal));
            for (int i = 0; i < dtKHV.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"iID_MaDanhMucDuAn='{0}' AND sDeAn='{1}' AND sDuAn='{2}' AND sDuAnThanhPhan='{3}' AND sCongTrinh='{4}' AND sHangMucCongTrinh='{5}'
                                                    AND NguonNS='{6}' AND sLNS='{7}'",
                                                  dtKHV.Rows[i]["iID_MaDanhMucDuAn"], dtKHV.Rows[i]["sDeAn"], dtKHV.Rows[i]["sDuAn"], dtKHV.Rows[i]["sDuAnThanhPhan"], dtKHV.Rows[i]["sCongTrinh"], dtKHV.Rows[i]["sHangMucCongTrinh"],
                                                  dtKHV.Rows[i]["NguonNS"], dtKHV.Rows[i]["sLNS"]);
                DataRow[] R = dt.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtKHV.Rows[i][arrCol[j]];
                    }
                    dt.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtKHV.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < arrCol.Length - 3; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt.Rows[j]["rKeHoachVonNamTruoc"] = R1["rKeHoachVonNamTruoc"];
                                dt.Rows[j]["rSoTienDauNam"] = R1["rSoTienDauNam"];
                                dt.Rows[j]["rSoTienDieuChinh"] = R1["rSoTienDieuChinh"];
                                break;
                            }
                        }
                    }
                }
            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dt.DefaultView;
            dv.Sort = "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet";
            dt = dv.ToTable();
            #endregion
            return dt;
        }
        public ActionResult ViewPDF(String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDotCapPhat, MaND, MaTien);
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
        public ExcelFile CreateReport(String path, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
           String dNgayLap = "01/01/2000";
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            //dtDotCapPhat.Dispose();
            dNgayLap = iID_MaDotCapPhat;
            String nam = dNgayLap.Substring(6, 4);
            String DotCapPhat = " Đến ngày " + dNgayLap.Substring(0, 2) + " tháng " + dNgayLap.Substring(3, 2) + " năm " + dNgayLap.Substring(6, 4);
            DataTable dtDVT = QLDA_ReportModel.dt_LoaiTien_CP_03(dNgayLap, MaND);
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                    break;
                }

            }
            dtDVT.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_02_CPVDT");
            LoadData(fr, iID_MaDotCapPhat, MaND, MaTien);
            fr.SetValue("DVT", DVT);
            fr.SetValue("DotCapPhat", DotCapPhat);
            fr.SetValue("Nam", nam);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iID_MaDotCapPhat, String MaND, String MaTien)
        {

            DataTable data = dt_rptQLDA_02_CP(iID_MaDotCapPhat, MaND, MaTien);
            // Hạng mục công trình
            DataTable dtHangMucChiTiet = HamChung.SelectDistinct_QLDA("HMChiTiet", data, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sTienDo");
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", dtHangMucChiTiet, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHangMucCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DATP", dtCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DuAn", dtDuAnThanhPhan, "NguonNS,sLNS,sDeAn,sDuAn", "NguonNS,sLNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sLNS,sDeAn", "NguonNS,sLNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //sLNS
            DataTable dtLNS = HamChung.SelectDistinct("sLNS", dtDeAn, "NguonNS,sLNS", "NguonNS,sLNS,sTenDuAn", "", "");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");

            //Thêm tên Loại ngân sách của dtLNS
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                DataRow r = dtLNS.Rows[i];
                String SQL = "SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND sLNS=@sLNS";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                String sMoTa = "";
                sMoTa = Connection.GetValueString(cmd, "");
                r["sTenDuAn"] = sMoTa;
            }

            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CongTrinh", dtCongTrinh);
            fr.AddTable("DATP", dtDuAnThanhPhan);
            fr.AddTable("DuAn", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("sLNS", dtLNS);
            fr.AddTable("Nguon", dtNguon);
            dtDeAn.Dispose();
            dtDuAn.Dispose();
            dtDuAnThanhPhan.Dispose();
            dtCongTrinh.Dispose();
            dtNguon.Dispose();
            data.Dispose();
        }
        public JsonResult ds_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            return Json(obj_QLDA(ParentID, iID_MaDotCapPhat, MaND, MaTien), JsonRequestBehavior.AllowGet);
        }
        public String obj_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            String dNgayLap = "01/01/2000";
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            {
                if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
                {
                    dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
                    break;
                }
            }
            dtDotCapPhat.Dispose();
            DataTable dtNgoaiTe = QLDA_ReportModel.dt_LoaiTien_CP_03(dNgayLap, MaND);
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTenNgoaiTe");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            return NgoaiTe;
        }
    }
}
