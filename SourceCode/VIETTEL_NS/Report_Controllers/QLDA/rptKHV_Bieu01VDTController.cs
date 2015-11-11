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
    public class rptKHV_Bieu01VDTController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptKHV_Bieu01VDT.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_Bieu01VDT.aspx";
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
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_Bieu01VDT.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NgoaiTe, String dNgay)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(path);
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
            dNgayLap = dNgay;
            String DotCapPhat = " Đến ngày " + dNgayLap.Substring(0, 2) + " tháng " + dNgayLap.Substring(3, 2) + " năm " + dNgayLap.Substring(6, 4);
            String nam = dNgayLap.Substring(6, 4);
            DataTable dtDVT = QLDA_ReportModel.dt_LoaiTien_CP_03("", "");
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (NgoaiTe == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                    break;
                }

            }
            dtDVT.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKHV_Bieu01VDT");
            LoadData(fr, NgoaiTe, dNgay);
            fr.SetValue("DVT", DVT);
            fr.SetValue("DotCapPhat", DotCapPhat);
            fr.SetValue("Nam", nam);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NgoaiTe"></param>
        /// <param name="dNgay"></param>
        private void LoadData(FlexCelReport fr, String NgoaiTe, String dNgay)
        {
            DataTable data = LayDanhSach(NgoaiTe, dNgay);
            data.Columns.Add("sTienDo", typeof(String));
            data.TableName = "ChiTiet";
            //Hạng mục chi tiết
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


        public clsExcelResult ExportToExcel(String NgoaiTe, string vidDenNgay)
        {
            String DuongDanFile = sFilePath;
            // DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NgoaiTe, vidDenNgay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKHV_Bieu01VDT" + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls";
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
            // DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NgoaiTe, vidDenNgay);
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



        public DataTable LayDanhSach(String NgoaiTe, String dNgay)
        {
            String DK_KHV = "";
            String DK_CP = "";
            String DK_DTQuy = "";
            String DK_DTQuy_UngTruoc = "";
            String DK_LoaiNgoaiTe_CP = "";
            String DK_LoaiNgoaiTe_KHV = "";
            String DK_LoaiNgoaiTe_DTQuy = "";
            if (NgoaiTe == "0")
            {
                DK_KHV = "rSoTienDieuChinh/1000000+rSoTienDauNam/1000000";
                DK_CP = "rDeNghiPheDuyetDonViThuHuong/1000000";
                DK_DTQuy = "rSoTienBTCCap/1000000";
                DK_DTQuy_UngTruoc = "rSoTienBTCCap_UngTruoc/1000000";
            }
            else
            {
                DK_KHV = "rNgoaiTe_SoTienDieuChinh+rNgoaiTe_SoTienDauNam";
                DK_CP = "rNgoaiTe_DeNghiPheDuyetDonViThuHuong";
                DK_DTQuy = "rNgoaiTe_rSoTienBTCCap";
                DK_DTQuy_UngTruoc = "rNgoaiTe_rSoTienBTCCap_UngTruoc";
                DK_LoaiNgoaiTe_CP = " iID_MaNgoaiTe_DeNghiPheDuyetDonViThuHuong=@iID_MaNgoaiTe AND ";
                DK_LoaiNgoaiTe_KHV = "(iID_MaNgoaiTe_SoTienDieuChinh=@iID_MaNgoaiTe OR iID_MaNgoaiTe_SoTienDauNam=@iID_MaNgoaiTe) AND ";
                DK_LoaiNgoaiTe_DTQuy = "(iID_MaNgoaiTe_rSoTienBTCCap=@iID_MaNgoaiTe OR iID_MaNgoaiTe_rSoTienBTCCap_UngTruoc=@iID_MaNgoaiTe) AND";
            }
            String dNam = "";
            String dThang = "",dQuy = "";
            if (dNgay != "01/01/2000")
            {
                dNam = dNgay.Substring(6, 4);
                dThang = dNgay.Substring(4, 2);
                if (dThang == "1" || dThang == "2" || dThang == "3")
                    dQuy = "1";
                else if (dThang == "4" || dThang == "5" || dThang == "6")
                    dQuy = "2";
                else if (dThang == "4" || dThang == "5" || dThang == "6")
                    dQuy = "2";
                else if (dThang == "7" || dThang == "8" || dThang == "9")
                    dQuy = "3";
                else
                    dQuy = "4";
            }

            DataTable dt = null;
            String SQL = String.Format(@"SELECT NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
,SUM(KHVUngTruoc) as KHVUngTruoc
,SUM(KHVNamNay) as KHVNamNay
,SUM(BTCCapUngTruoc) as BTCCapUngTruoc
,SUM(BTCCapNamNay) as BTCCapNamNay
,SUM(CapPhatUngTruoc) as CapPhatUngTruoc
,SUM(CapPhatNamNay) as CapPhatNamNay
FROM(
SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)as sTenDuAn
	,KHVUngTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iLoaiKeHoachVon=2 THEN {0} ELSE 0 END)
   ,KHVNamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iLoaiKeHoachVon=1 THEN {0} ELSE 0 END)
	,BTCCapUngTruoc=0
	,BTCCapNamNay=0
	,CapPhatUngTruoc=0
	,CapPhatNamNay=0
	FROM QLDA_KeHoachVon
	WHERE iTrangThai=1  AND {4} sHangMucChiTiet<>'' AND  dNgayKeHoachVon<=@dNgayLap
	GROUP BY sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)
	
	UNION
	
	SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)as sTenDuAn
	,KHVUngTruoc=0
	,KHVNamNay=0
	,BTCCapUngTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec  THEN {1} ELSE 0 END)
   ,BTCCapNamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec  THEN {2} ELSE 0 END)
	,CapPhatUngTruoc=0
	,CapPhatNamNay=0
	FROM QLDA_DuToan_Quy
	WHERE iTrangThai=1 AND {5} sHangMucChiTiet<>'' AND  iQuy<=@iQuy
	GROUP BY sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)
	UNION
	
	 SELECT   SUBSTRING(sLNS,1,1) as NguonNS,sLNS,QLDA_CapPhat.iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
	,KHVUngTruoc=0
	,KHVNamNay=0
	,BTCCapUngTruoc=0
	,BTCCapNamNay=0
	,CapPhatUngTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=2  THEN {3} ELSE 0 END)
   ,CapPhatNamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1  THEN {3} ELSE 0 END)
	FROM QLDA_CapPhat
	INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                  FROM QLDA_DanhMucDuAn 
                  WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as QLDA_DanhMucDuAn
                  ON QLDA_CapPhat.iID_MaDanhMucDuAn=QLDA_DanhMucDuAn.iID_MaDanhMucDuAn
    WHERE QLDA_CapPhat.iTrangThai=1 AND {6} iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE iTrangThai=1 AND dNgayLap<=@dNgayLap)
	GROUP BY SUBSTRING(sLNS,1,1),sLNS, QLDA_CapPhat.iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet) as ChiTiet
                          GROUP BY NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
                          HAVING SUM(KHVUngTruoc) <>0 OR
								SUM(KHVNamNay)  <>0 OR
								SUM(BTCCapUngTruoc) <>0 OR
								SUM(BTCCapNamNay) <>0 OR
								SUM(CapPhatUngTruoc) <>0 OR
								SUM(CapPhatNamNay) <>0",DK_KHV,DK_DTQuy_UngTruoc,DK_DTQuy,DK_CP,DK_LoaiNgoaiTe_KHV,DK_LoaiNgoaiTe_DTQuy,DK_LoaiNgoaiTe_CP);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgay));
            cmd.Parameters.AddWithValue("@iQuy", dQuy);
            if (NgoaiTe != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", NgoaiTe);
            }
            dt = Connection.GetDataTable(cmd);
            return dt;
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
