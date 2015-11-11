using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    public class rptKHV_02VDT_THNSNNController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptKHV_02VDT_THNSNN.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_02VDT_THNSNN.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["PageLoad"] = "0";
            ViewData["NgoaiTe"] = "0";
            ViewData["dDenNgay"] = String.Format("{0:dd/MM/yyyy}", DateTime.Now.Date);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NgoaiTe = Convert.ToString(Request.Form[ParentID + "_NgoaiTe"]);
            String dDenNgay = Convert.ToString(Request.Form[ParentID + "_vidDenNgay"]);
            ViewData["PageLoad"] = "1";
            ViewData["NgoaiTe"] = NgoaiTe;
            ViewData["dDenNgay"] = dDenNgay;
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_02VDT_THNSNN.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NgoaiTe"></param>
        /// <param name="dNgay"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NgoaiTe, DateTime dNgay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            //Toan lục luong
            fr = ReportModels.LayThongTinChuKy(fr, "rptKHV_02VDT_THNSNN");

            LoadData(fr, NgoaiTe, dNgay);

            DateTime dngaybaocao = Convert.ToDateTime(dNgay);

            fr.SetValue("Nam", dngaybaocao.Year);
            fr.SetValue("Ngay", "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString());
            fr.SetValue("DenNgay", "Đến ngày " + dngaybaocao.Day.ToString() + " tháng " + dngaybaocao.Month.ToString() + " năm " + dngaybaocao.Year.ToString());

            string sNgoaiTe = "VNĐ";
            if (NgoaiTe !="0")
                sNgoaiTe = NS_NgoaiTe_Get_Ten(NgoaiTe);

            fr.SetValue("DonVi", sNgoaiTe);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NgoaiTe"></param>
        /// <param name="dNgay"></param>
        private void LoadData(FlexCelReport fr, String NgoaiTe, DateTime dNgay)
        {
            DataTable data = LayDanhSach(NgoaiTe, dNgay);
            data.TableName = "HMCTiet";
            fr.AddTable("HMCTiet", data);
            //Toan lục luong
            DataTable dtHMCT = HamChung.SelectDistinct_QLDA("HMCT", data, "sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh",
                "sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sMoTa",
                "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            fr.AddTable("HMCT", dtHMCT);

            DataTable dtCT = HamChung.SelectDistinct_QLDA("CT", dtHMCT, "sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh",
                "sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sMoTa",
                "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            fr.AddTable("CT", dtCT);

            DataTable dtDAThanhPhan = HamChung.SelectDistinct_QLDA("DAThanhPhan", dtCT, "sLNS,sDeAn,sDuAn,sDuAnThanhPhan",
                "sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sMoTa",
                "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            fr.AddTable("DAThanhPhan", dtDAThanhPhan);

            DataTable dtDA = HamChung.SelectDistinct_QLDA("DA", dtDAThanhPhan, "sLNS,sDeAn,sDuAn",
                "sLNS,sDeAn,sDuAn,sTenDuAn,sMoTa",
                "sDeAn,sDuAn,sDuAnThanhPhan");
            fr.AddTable("DA", dtDA);

            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDA, "sLNS,sDeAn",
               "sLNS,sDeAn,sTenDuAn,sMoTa",
               "sDeAn,sDuAn");
            fr.AddTable("DeAn", dtDeAn);

            DataTable dtLNS = HamChung.SelectDistinct("LNS", data, "sLNS", "STT,sLNS,sMoTa", "sLNS", "sLNS");

            if (dtLNS != null)
            {
                for (int i = 0; i < dtLNS.Rows.Count; i++)
                {
                    dtLNS.Rows[i]["STT"] = getColumnNameFromIndex(i+1);
                }
            }
            fr.AddTable("LNS", dtLNS);

            data.Dispose();
            dtHMCT.Dispose();
            dtCT.Dispose();
            dtDAThanhPhan.Dispose();
            dtDA.Dispose();
            dtDeAn.Dispose();
            dtLNS.Dispose();
        }

        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NgoaiTe"></param>
        /// <param name="dDenNgay"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NgoaiTe, string vidDenNgay)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NgoaiTe, dNgay);
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
        /// <summary>
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NgoaiTe, string vidDenNgay)
        {
            String DuongDanFile = sFilePath;
            DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NgoaiTe, dNgay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKHV_02VDT_THNSNN" + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NgoaiTe, string vidDenNgay)
        {
            //ViewData["bBaoCaoTH"] = "False";
            String DuongDanFile = sFilePath;
            DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NgoaiTe, dNgay);
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

        /// <summary>
        /// Data của báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="bThang"></param>
        /// <returns></returns>
        public DataTable LayDanhSach(String NgoaiTe, DateTime dNgay)
        {
            DataTable dtDanhSach = new DataTable();
            DataTable dtNgoaiTe = new DataTable();

            //lấy danh sách du an
            //sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
            String SQLDSNS =
                "select '' as  STT, t.iID_MaDanhMucDuAn,sTenDuAn, t.sLNS,t.sDeAn,t.sDuAn,t.sDuAnThanhPhan,t.sCongTrinh,t.sHangMucCongTrinh,t.sHangMucChiTiet, '' as sMoTa" +
                " from " +
                " (SELECT iID_MaDanhMucDuAn,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet " +
                " from QLDA_KeHoachVon " +
                " WHERE QLDA_KeHoachVon.iTrangThai=1 and QLDA_KeHoachVon.iNamLamViec = @iNamLamViec " +
                " AND dNgayKeHoachVon <= @dDenNgay";
            //" AND QLDA_KeHoachVon.dNgayKeHoachVon <= @dNgayKeHoachVon ";
            SQLDSNS += " Group By sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,iID_MaDanhMucDuAn) as t  inner join ";
            SQLDSNS += " (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh " +
                       " FROM QLDA_DanhMucDuAn WHERE iTrangThai=1 AND sHangMucCongTrinh<>'' ) as t1  ON t.iID_MaDanhMucDuAn = t1.iID_MaDanhMucDuAn";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            cmdThangNay.Parameters.AddWithValue("@iNamLamViec", dNgay.Year);
            cmdThangNay.Parameters.AddWithValue("@dDenNgay", dNgay);
            //cmdThangNay.Parameters.AddWithValue("@dNgayKeHoachVon", dNgay);
            dtNgoaiTe = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();
            //dtDanhSach = dtNgoaiTe.Clone();
            dtDanhSach = dtNgoaiTe;
            string sqlAllData =
                "select * from QLDA_KeHoachVon where QLDA_KeHoachVon.iTrangThai=1 and QLDA_KeHoachVon.iNamLamViec = @iNamLamViec";
            SqlCommand cmdAllData = new SqlCommand(sqlAllData);
            cmdAllData.Parameters.AddWithValue("@iNamLamViec", dNgay.Year);
            DataTable dtAllData = Connection.GetDataTable(cmdAllData);
            cmdAllData.Dispose();

            //lay du lieu da cap phat cho don vi
            string sqlDaCapDV =
                "select iID_MaDanhMucDuAn,sLNS,iID_MaLoaiKeHoachVon";
            if(NgoaiTe == "0")
            {
                sqlDaCapDV += ", sum(rChuDauTuThanhToan) as iDVNamNay ";   
            }else
            {
                sqlDaCapDV += "iID_MaNgoaiTe_ChuDauTuThanhToan, sum(rNgoaiTe_ChuDauTuThanhToan) as iDVNamNay ";
            }
            sqlDaCapDV += " from QLDA_CapPhat where iTrangThai =1 and iNamLamViec = @iNamLamViec and dNgayLap <= @dDenNgayCapPhat";

            if (NgoaiTe == "0")
            {
                sqlDaCapDV +=
                " group by iID_MaDanhMucDuAn,sLNS,iID_MaLoaiKeHoachVon";
            }
            else
            {
                sqlDaCapDV +=
                " group by iID_MaDanhMucDuAn,sLNS,iID_MaLoaiKeHoachVon,iID_MaNgoaiTe_ChuDauTuThanhToan";
            }
            
            cmdAllData = new SqlCommand(sqlDaCapDV);
            cmdAllData.Parameters.AddWithValue("@iNamLamViec", dNgay.Year);
            cmdAllData.Parameters.AddWithValue("@dDenNgayCapPhat", dNgay);
            DataTable dtDaCap = Connection.GetDataTable(cmdAllData);
            cmdAllData.Dispose();


            //lay du lieu Quyet toán
            //string sqlBTC =
            //    "select sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,iID_MaDanhMucDuAn,sTenDuAn, sum(rDonViDeNghi) as iBTCCapNamNay ";
            //sqlBTC += " from QLDA_DuToan_Nam where iTrangThai =1 and iNamLamViec = @iNamLamViec and dNgayLap <= @dDenNgayDuToan";
            //sqlBTC +=
            //    " group by sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, iID_MaDanhMucDuAn,sTenDuAn";
            //cmdAllData = new SqlCommand(sqlBTC);
            //cmdAllData.Parameters.AddWithValue("@iNamLamViec", dNgay.Year);
            //cmdAllData.Parameters.AddWithValue("@dDenNgayDuToan", dNgay);
            //DataTable dtBTCData = Connection.GetDataTable(cmdAllData);
            //cmdAllData.Dispose();

            //Add colunm
            if (dtDanhSach != null)
            {
                dtDanhSach.Columns.Add("iKHVNamTruocChuyenSang", typeof(Double));
                dtDanhSach.Columns.Add("iKHVNamTruoc", typeof(Double));
                dtDanhSach.Columns.Add("iKHVNamNay", typeof(Double));
                dtDanhSach.Columns.Add("iCPNamTruocChuyenSang", typeof(Double));
                dtDanhSach.Columns.Add("iCPCapNamTruoc", typeof(Double));
                dtDanhSach.Columns.Add("iCPCapNamNay", typeof(Double));
                dtDanhSach.Columns.Add("iQTNamTruocChuyenSang", typeof(Double));
                dtDanhSach.Columns.Add("iQTNamTruoc", typeof(Double));
                dtDanhSach.Columns.Add("iQTNamNay", typeof(Double));
                dtDanhSach.Columns.Add("iThuNop", typeof(Double));
                for (int i = 0; i < dtDanhSach.Rows.Count; i++)
                {
                    //KH von dc su dung
                    string sqlValue =
                        "iID_MaDanhMucDuAn= '" + Convert.ToString(dtDanhSach.Rows[i]["iID_MaDanhMucDuAn"]) + "'";
                    sqlValue += " and sLNS= '" + Convert.ToString(dtDanhSach.Rows[i]["sLNS"]) + "'";
                    sqlValue +=
                        " and sDeAn= '" + Convert.ToString(dtDanhSach.Rows[i]["sDeAn"]) + "'";
                    sqlValue +=
                        " and sDuAn= '" + Convert.ToString(dtDanhSach.Rows[i]["sDuAn"]) + "'";
                    sqlValue +=
                        " and sDuAnThanhPhan= '" + Convert.ToString(dtDanhSach.Rows[i]["sDuAnThanhPhan"]) + "'";
                    sqlValue +=
                        " and sCongTrinh= '" + Convert.ToString(dtDanhSach.Rows[i]["sCongTrinh"]) + "'";
                    sqlValue +=
                        " and sHangMucCongTrinh= '" + Convert.ToString(dtDanhSach.Rows[i]["sHangMucCongTrinh"]) +
                        "'";
                    sqlValue +=
                        " and sHangMucChiTiet= '" + Convert.ToString(dtDanhSach.Rows[i]["sHangMucChiTiet"]) + "'";
                    if (dtAllData != null)
                    {
                        //nam truoc chuyen sang iLoaiKeHoachVon =3
                        string sSqlNgoaiTe = "";
                        if (NgoaiTe != "0")
                            sSqlNgoaiTe = " and iID_MaNgoaiTe_DauNam = '" + NgoaiTe + "'";
                        string sqlloaiKHvon = " and iLoaiKeHoachVon= '3'";
                        DataRow[] row = dtAllData.Select(sqlValue + sqlloaiKHvon+ sSqlNgoaiTe);
                        if (row.Length > 0)
                        {
                            if (NgoaiTe != "0")
                                dtDanhSach.Rows[i]["iKHVNamTruocChuyenSang"] = getSumValue(row, "rNgoaiTe_DauNam");// row[0]["rNgoaiTe_DauNam"];
                            else
                                dtDanhSach.Rows[i]["iKHVNamTruocChuyenSang"] = getSumValue(row, "rSoTienDauNam");//row[0]["rSoTienDauNam"];
                        }

                        //nam nay iLoaiKeHoachVon =1
                         sqlloaiKHvon = " and iLoaiKeHoachVon= '1'";
                         row = dtAllData.Select(sqlValue + sqlloaiKHvon + sSqlNgoaiTe);
                        if (row.Length > 0)
                        {
                            if (NgoaiTe != "0")
                                dtDanhSach.Rows[i]["iKHVNamNay"] = getSumValue(row, "rNgoaiTe_DauNam");//row[0]["rNgoaiTe_DauNam"];
                            else
                                dtDanhSach.Rows[i]["iKHVNamNay"] = getSumValue(row, "rSoTienDauNam");//row[0]["rSoTienDauNam"];
                        }
                        //nam truoc iLoaiKeHoachVon =2
                        sqlloaiKHvon = " and iLoaiKeHoachVon= '2'";
                        row = dtAllData.Select(sqlValue + sqlloaiKHvon + sSqlNgoaiTe);
                        if (row.Length > 0)
                        {
                            if (NgoaiTe != "0")
                                dtDanhSach.Rows[i]["iKHVNamTruoc"] = getSumValue(row,"rNgoaiTe_DauNam");// row[0]["rNgoaiTe_DauNam"];
                            else
                                dtDanhSach.Rows[i]["iKHVNamTruoc"] = getSumValue(row, "rSoTienDauNam");//row[0]["rSoTienDauNam"];
                        }
                    }
                    //du lieu BTC cap
                    //if (dtBTCData != null)
                    //{

                    //    //nam nay 
                    //    DataRow[] row = dtBTCData.Select(sqlValue);
                    //    if (row.Length > 0)
                    //    {
                    //        dtDanhSach.Rows[i]["iBTCCapNamNay"] = getSumValue(row, "iBTCCapNamNay");//row[0]["iBTCCapNamNay"];
                    //    }
                    //    //nam truoc 
                    //    //if (row.Length > 0)
                    //    //{
                    //    //    dtDanhSach.Rows[i]["iKHVNamTruoc"] = row[0]["rSoTienDauNam"];
                    //    //}
                    //}
                    //du lieu don vi da cap
                    if (dtDaCap != null)
                    {
                        string sqlDaCap =
                        "iID_MaDanhMucDuAn= '" + Convert.ToString(dtDanhSach.Rows[i]["iID_MaDanhMucDuAn"]) + "'";
                        sqlDaCap += " and sLNS= '" + Convert.ToString(dtDanhSach.Rows[i]["sLNS"]) + "'";
                        string sSqlNgoaiTe = "";
                        if (NgoaiTe != "0")
                            sSqlNgoaiTe = " and iID_MaNgoaiTe_ChuDauTuThanhToan = '" + NgoaiTe + "'";
                        //nam truoc chuyen sang
                        string sqlloaiKHvon = " and iID_MaLoaiKeHoachVon= '3'";
                        DataRow[] row = dtDaCap.Select(sqlDaCap + sqlloaiKHvon + sSqlNgoaiTe);
                        if (row.Length > 0)
                        {
                            dtDanhSach.Rows[i]["iCPNamTruocChuyenSang"] = getSumValue(row, "iDVNamNay");//row[0]["iDVNamNay"];
                        }
                        // nam nay
                        sqlloaiKHvon = " and iID_MaLoaiKeHoachVon= '1'";
                        row = dtDaCap.Select(sqlDaCap + sqlloaiKHvon + sSqlNgoaiTe);
                        if (row.Length > 0)
                        {
                            dtDanhSach.Rows[i]["iCPCapNamNay"] = getSumValue(row, "iDVNamNay");//row[0]["iDVNamNay"];
                        }
                        //nam truoc 
                        sqlloaiKHvon = " and iID_MaLoaiKeHoachVon= '2'";
                        row = dtDaCap.Select(sqlDaCap + sqlloaiKHvon + sSqlNgoaiTe);
                        if (row.Length > 0)
                        {
                            dtDanhSach.Rows[i]["iCPCapNamTruoc"] = getSumValue(row, "iDVNamNay");//row[0]["iDVNamNay"];
                        }
                    }
                }
            }
            return dtDanhSach;
        }


        public string NS_NgoaiTe_Get_Ten(String sMaNgoaiTe)
        {
            DataTable dtNgoaiTe = new DataTable();

            //lay du lieu thang quy
            string sTen = "";
            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT top 1 sTen" +
                " from QLDA_NgoaiTe WHERE iID_MaNgoaiTe = @iID_MaNgoaiTe";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            cmdThangNay.Parameters.AddWithValue("@iID_MaNgoaiTe", sMaNgoaiTe);
            dtNgoaiTe = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();
            if (dtNgoaiTe != null)
            {
                if (dtNgoaiTe.Rows.Count > 0) sTen = Convert.ToString(dtNgoaiTe.Rows[0]["sTen"]);
            }
            else
            {
                sTen = "";
            }
            return sTen;
        }

        /// <summary>
        /// Ham lay so thu tu theo ky tu a,b,c, ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static String getColumnNameFromIndex(int column)
        {
            column--;
            String col = Convert.ToString((char)('A' + (column % 26)));
            while (column >= 26)
            {
                column = (column / 26) - 1;
                col = Convert.ToString((char)('A' + (column % 26))) + col;
            }
            return col;
        }

        /// <summary>
        /// lay tong gia tri trong mang datarow
        /// </summary>
        /// <param name="drTemp"></param>
        /// <param name="sColumnName"></param>
        /// <returns></returns>
        public static double getSumValue(DataRow[] drTemp, string sColumnName)
        {
            double dValueSume = 0;
            try
            {
                for (int i = 0; i < drTemp.Length; i++)
                {
                    dValueSume += Convert.ToDouble(drTemp[i][sColumnName]);
                }
            }
            catch (Exception)
            {
            }
            return dValueSume;
        }

    }
}
