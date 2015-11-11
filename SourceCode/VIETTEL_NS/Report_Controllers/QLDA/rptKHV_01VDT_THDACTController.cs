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
    public class rptKHV_01VDT_THDACTController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptKHV_01VDT_THDACT.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_01VDT_THDACT.aspx";
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
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_01VDT_THDACT.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NgoaiTe"></param>
        /// <param name="dNgay"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NgoaiTe, String dNgay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            //Toan lục luong
            fr = ReportModels.LayThongTinChuKy(fr, "rptKHV_01VDT_THDACT");

            LoadData(fr, NgoaiTe, dNgay);

          //  DateTime dngaybaocao = Convert.ToDateTime(dNgay);

            String DotCapPhat = " Đến ngày " + dNgay.Substring(0, 2) + " tháng " + dNgay.Substring(3, 2) + " năm " + dNgay.Substring(6, 4);
            String nam = dNgay.Substring(6, 4);
            String DVT = " triệu đồng";
            DataTable dtDVT = QLDA_ReportModel.dt_LoaiTien_CP_03("", "");
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (NgoaiTe == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                    break;
                }

            }
            dtDVT.Dispose();

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
            //DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NgoaiTe, vidDenNgay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKHV_01VDT_THDACT" + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls";
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
            //DateTime dNgay = Convert.ToDateTime(CommonFunction.LayNgayTuXau(vidDenNgay));
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

       

        public DataTable LayDanhSach(String NgoaiTe,String dNgay)
            {
            String DK_KHV_NgoaiTe_rSoTien = "";
            String DK_KHV_LoaiNgoaiTe = "";
            String DK_CP_NgoaiTe_TamUng = "";
            String DK_CP_NgoaiTe_ThuTamUng = "";
            String DK_CP_NgoaiTe_ThanhToan = "";
            String DK_CP_LoaiNgoaiTe = "";
            String DKKHV = "", DKKHV_LoaiNgoaiTe = "";
             String dNam = "";
            if (dNgay != "01/01/2000")
                dNam = dNgay.Substring(6, 4);
            if (NgoaiTe == "0")
            {
                DK_KHV_NgoaiTe_rSoTien = "rSoTien/1000000";
                DK_CP_NgoaiTe_TamUng = "rDeNghiPheDuyetTamUng/1000000";
                DK_CP_NgoaiTe_ThuTamUng = "rDeNghiPheDuyetThuTamUng/1000000";
                DK_CP_NgoaiTe_ThanhToan = "rDeNghiPheDuyetThanhToan/1000000";
                DKKHV = "(rSoTienDauNam+rSoTienDieuChinh)/1000000";
            }
            else
            {
                DKKHV = "(rNgoaiTe_SoTienDauNam+rNgoaiTe_SoTienDieuChinh)";
                DKKHV_LoaiNgoaiTe = "(iID_MaNgoaiTe_SoTienDauNam=@iID_MaNgoaiTe OR iID_MaNgoaiTe_SoTienDieuChinh=@iID_MaNgoaiTe";
                DK_KHV_NgoaiTe_rSoTien = "rNgoaiTe_SoTien";
                DK_CP_NgoaiTe_TamUng = "SUM(rNgoaiTe_DeNghiPheDuyetTamUng)-SUM(rNgoaiTe_DeNghiPheDuyetThuTamUng)";
                DK_CP_NgoaiTe_ThanhToan = "rNgoaiTe_DeNghiPheDuyetThanhToan";

                DK_KHV_LoaiNgoaiTe = " (iID_MaNgoaiTe_SoTien=@iID_MaNgoaiTe) AND ";
                DK_CP_LoaiNgoaiTe = " (iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThanhToan=@iID_MaNgoaiTe) AND ";
            }
            String SQL = String.Format(@"
	SELECT NguonNS,ChiTiet.sLNS,ChiTiet.iID_MaDanhMucDuAn, ChiTiet.sDeAN,ChiTiet.sDuAn,ChiTiet.sDuAnThanhPhan,ChiTiet.sCongTrinh,ChiTiet.sHangMucCongTrinh,ChiTiet.sHangMucChiTiet,ChiTiet.sTenDuAn
	,SUM(rSoTienDauTu) as rSoTienDauTu
	,SUM(rSoTienDuToan) as rSoTienDuToan
	,SUM(rDeNghiPheDuyetThanhToan) as rDeNghiPheDuyetThanhToan
	,SUM(rDeNghiPheDuyetThanhToan_LuyKe) as rDeNghiPheDuyetThanhToan_LuyKe
	,SUM(rDeNghiPheDuyetTamUng) as rDeNghiPheDuyetTamUng
	,SUM(rDeNghiPheDuyetThuTamUng) as rDeNghiPheDuyetThuTamUng
	,SUM(KHV_Nam) as KHV_Nam
	,SUM(KHV_LuyKe) as KHV_LuyKe
	FROM( 
	SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)as sTenDuAn,
			SUM({0}) as rSoTienDauTu,rSoTienDuToan=0,rDeNghiPheDuyetThanhToan=0,rDeNghiPheDuyetThanhToan_LuyKe=0,rDeNghiPheDuyetTamUng=0,rDeNghiPheDuyetThuTamUng=0,KHV_Nam=0,KHV_LuyKe=0
	FROM QLDA_TongDauTu
    WHERE iTrangThai=1 AND dNgayPheDuyet<=@dNgayLap {5} AND iNamLamViec<=@iNamLamViec  AND sHangMucChiTiet<>''
	GROUP BY SUBSTRING(sLNS,1,1), sLNS,iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
	
	UNION
	
     SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)as sTenDuAn,
			rSoTienDauTu=0,SUM({0}) as rSoTienDuToan,rDeNghiPheDuyetThanhToan=0,rDeNghiPheDuyetThanhToan_LuyKe=0,rDeNghiPheDuyetTamUng=0,rDeNghiPheDuyetThuTamUng=0,KHV_Nam=0,KHV_LuyKe=0
	FROM QLDA_TongDuToan
    WHERE iTrangThai=1 AND dNgayPheDuyet<=@dNgayLap {5} AND iNamLamViec<=@iNamLamViec AND sHangMucChiTiet<>''
	GROUP BY SUBSTRING(sLNS,1,1), sLNS,iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
	
	UNION
   SELECT   SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)as sTenDuAn,rSoTienDauTu=0,rSoTienDuToan=0,rDeNghiPheDuyetThanhToan=0,rDeNghiPheDuyetThanhToan_LuyKe=0,rDeNghiPheDuyetTamUng=0,rDeNghiPheDuyetThuTamUng=0,
     KHV_Nam=SUM(CASE WHEN (iNamLamViec=@iNamLamViec AND iLoaiKeHoachVon=1) THEN  {1} ELSE 0 END),
     KHV_LuyKe=SUM(CASE WHEN (iNamLamViec<=@iNamLamViec AND iLoaiKeHoachVon=1)THEN  {1} ELSE 0 END)
     FROM QLDA_KeHoachVon
    WHERE iTrangThai=1 AND {6} dNgayKeHoachVon<=@dNgayLap AND sHangMucChiTiet<>''
   GROUP BY SUBSTRING(sLNS,1,1), sLNS,iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
     UNION 
     SELECT   SUBSTRING(sLNS,1,1) as NguonNS,sLNS,QLDA_CapPhat.iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,
						 rSoTienDauTu=0,rSoTienDuToan=0,
                          rDeNghiPheDuyetThanhToan=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN  {2} ELSE 0 END),
                          rDeNghiPheDuyetThanhToan_LuyKe=SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN  {2} ELSE 0 END),
                          rDeNghiPheDuyetTamUng=SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN  {3} ELSE 0 END),
                           rDeNghiPheDuyetThuTamUng=SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN  {4} ELSE 0 END),KHV_Nam=0,KHV_LuyKe=0
	FROM QLDA_CapPhat
	  INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                  FROM QLDA_DanhMucDuAn 
                  WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as QLDA_DanhMucDuAn
                  ON QLDA_CapPhat.iID_MaDanhMucDuAn=QLDA_DanhMucDuAn.iID_MaDanhMucDuAn
WHERE {7}   QLDA_CapPhat.iTrangThai=1 AND QLDA_CapPhat.iID_MaLoaiKeHoachVon='1' AND iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat
																        FROM QLDA_CapPhat_Dot
																       WHERE dNgayLap<=@dNgayLap)
                  GROUP BY SUBSTRING(sLNS,1,1),sLNS, QLDA_CapPhat.iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet) as ChiTiet
       GROUP BY NguonNS,ChiTiet.sLNS,ChiTiet.iID_MaDanhMucDuAn, ChiTiet.sDeAN,ChiTiet.sDuAn,ChiTiet.sDuAnThanhPhan,ChiTiet.sCongTrinh,ChiTiet.sHangMucCongTrinh,ChiTiet.sHangMucChiTiet,ChiTiet.sTenDuAn
    HAVING SUM(rSoTienDauTu) <>0 OR
	SUM(rSoTienDuToan) <>0 OR
	SUM(rDeNghiPheDuyetThanhToan) <>0 OR
	SUM(rDeNghiPheDuyetThanhToan_LuyKe) <>0 OR
	SUM(rDeNghiPheDuyetTamUng) <>0 OR
	SUM(rDeNghiPheDuyetThuTamUng) <>0 OR
	SUM(KHV_Nam) <>0 OR
	SUM(KHV_LuyKe) <>0
	
	", DK_KHV_NgoaiTe_rSoTien, DKKHV, DK_CP_NgoaiTe_ThanhToan, DK_CP_NgoaiTe_TamUng, DK_CP_NgoaiTe_ThuTamUng, DK_KHV_LoaiNgoaiTe,DKKHV_LoaiNgoaiTe,DK_CP_LoaiNgoaiTe);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgay));
            if (NgoaiTe != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", NgoaiTe);
            }
            DataTable dt = Connection.GetDataTable(cmd);
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
