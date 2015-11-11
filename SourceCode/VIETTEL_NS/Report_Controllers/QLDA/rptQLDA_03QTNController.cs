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
    public class rptQLDA_03QTNController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_03QTN.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_03QTN.aspx";
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
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_03QTN.aspx";
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_03QTN");
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
            DataTable dtHangMucChiTiet = HamChung.SelectDistinct_QLDA("HMChiTiet", data, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sTienDo");
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", dtHangMucChiTiet, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
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
                clsResult.FileName = "rptQLDA_03QTN" + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls";
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
            HamChung.Language();
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
             String dNam = "-2000";
            if (dNgay != "01/01/2000")
            {
                dNam = dNgay.Substring(6, 4);
            }

            DataTable dt = null;
            String SQL = String.Format(@" 
 SELECT  NguonNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, REPLACE(sTenDuAn,sXauNoiMa_DuAn+'-','') as sTenDuAn,sTenDuAn as sTenDuAn1
 ,SUM(rTongDauTu/1000000) as rTongDauTu
 ,SUM(rKHV/1000000) as rKHV
 ,SUM(rKHV_NamTruocChuyenSang/1000000) as rKHV_NamTruocChuyenSang
  ,SUM(rDeNghiPheDuyetThanhToan/1000000) as rDeNghiPheDuyetThanhToan
  ,SUM(rDeNghiPheDuyetThanhToan_NamTruocChuyenSang/1000000)as rDeNghiPheDuyetThanhToan_NamTruocChuyenSang
  ,SUM(rDeNghiPheDuyetTamUngChuaThuHoi/1000000)as rDeNghiPheDuyetTamUngChuaThuHoi
  ,SUM(rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang/1000000)as rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang
  ,SUM(rDeNghiPheDuyetTamUngChuaThuHoi_LKNamTruoc/1000000) as rDeNghiPheDuyetTamUngChuaThuHoi_LKNamTruoc
  ,SUM(rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang_LKNamTruoc/1000000)as rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang_LKNamTruoc
 FROM(
 SELECT  SUBSTRING(sLNS,1,1) as NguonNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  ,SUM(rSoTien) as rTongDauTu
  ,rKHV=0
  ,rKHV_NamTruocChuyenSang=0
  ,rDeNghiPheDuyetThanhToan=0
  ,rDeNghiPheDuyetThanhToan_NamTruocChuyenSang=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi_LKNamTruoc=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang_LKNamTruoc=0
  FROM QLDA_TongDauTu
   WHERE iTrangThai=1 AND sHangMucChiTiet<>'' AND iNamLamViec<=@iNamLamViec
  GROUP BY  sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  HAVING SUM(rSoTien)<>0
 UNION
 
 
  SELECT  SUBSTRING(sLNS,1,1) as NguonNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  ,rTongDauTu=0
  ,rKHV=SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec THEN rSoTienDieuChinh ELSE 0 END)
 ,rKHV_NamTruocChuyenSang=SUM(CASE WHEN iLoaiKeHoachVon=3 AND iNamLamViec=@iNamLamViec THEN rSoTienDieuChinh ELSE 0 END)
  ,rDeNghiPheDuyetThanhToan=0
  ,rDeNghiPheDuyetThanhToan_NamTruocChuyenSang=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi_LKNamTruoc=0
  ,rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang_LKNamTruoc=0
  FROM QLDA_KeHoachVon
   WHERE iTrangThai=1 AND sHangMucChiTiet<>''
  GROUP BY  sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  HAVING  SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec THEN rSoTienDieuChinh ELSE 0 END)<>0
  OR SUM(CASE WHEN iLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec THEN rSoTienDieuChinh ELSE 0 END)<>0
  
 
 
 
 UNION
  SELECT  SUBSTRING(sLNS,1,1) as NguonNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  ,rTongDauTu=0
  ,rKHV=0
  ,rKHV_NamTruocChuyenSang=0
  ,rDeNghiPheDuyetThanhToan=SUM(CASE WHEN iID_MaLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetThanhToan ELSE 0 END)
  ,rDeNghiPheDuyetThanhToan_NamTruocChuyenSang=SUM(CASE WHEN iID_MaLoaiKeHoachVon=3 AND iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetThanhToan ELSE 0 END)
  ,rDeNghiPheDuyetTamUngChuaThuHoi=SUM(CASE WHEN iID_MaLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetTamUng-rDeNghiPheDuyetThuTamUng ELSE 0 END)
  ,rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang=SUM(CASE WHEN iID_MaLoaiKeHoachVon=3 AND iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetTamUng-rDeNghiPheDuyetThuTamUng ELSE 0 END)
  ,rDeNghiPheDuyetTamUngChuaThuHoi_LKNamTruoc=SUM(CASE WHEN iID_MaLoaiKeHoachVon=1 AND iNamLamViec<=@iNamTruoc THEN rDeNghiPheDuyetTamUng-rDeNghiPheDuyetThuTamUng ELSE 0 END)
  ,rDeNghiPheDuyetTamUngChuaThuHoi_NamTruocChuyenSang_LKNamTruoc=SUM(CASE WHEN iID_MaLoaiKeHoachVon=3 AND iNamLamViec<=@iNamTruoc THEN rDeNghiPheDuyetTamUng-rDeNghiPheDuyetThuTamUng ELSE 0 END)
  FROM QLDA_CapPhat 
  WHERE iTrangThai=1 AND sHangMucChiTiet<>'' AND dNgayLap<=@dNgayLap
  GROUP BY  sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  HAVING 
  SUM(CASE WHEN iID_MaLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetThanhToan ELSE 0 END)<>0
  OR SUM(CASE WHEN iID_MaLoaiKeHoachVon=3 AND iNamLamViec=@iNamLamViec THEN rDeNghiPheDuyetThanhToan ELSE 0 END)<>0
  OR SUM(CASE WHEN iID_MaLoaiKeHoachVon=1 AND iNamLamViec<=@iNamLamViec THEN rDeNghiPheDuyetTamUng-rDeNghiPheDuyetThuTamUng ELSE 0 END)<>0
  OR SUM(CASE WHEN iID_MaLoaiKeHoachVon=3 AND iNamLamViec<=@iNamLamViec THEN rDeNghiPheDuyetTamUng-rDeNghiPheDuyetThuTamUng ELSE 0 END)<>0) as CT
  GROUP BY  NguonNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet, sTenDuAn
  ORDER BY sTenDuAn1
       ");
            SqlCommand cmd = new SqlCommand(SQL);
           cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
           cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(dNam)-1);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgay));
            //if (NgoaiTe != "0")
            //{
            //    cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", NgoaiTe);
            //}
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
