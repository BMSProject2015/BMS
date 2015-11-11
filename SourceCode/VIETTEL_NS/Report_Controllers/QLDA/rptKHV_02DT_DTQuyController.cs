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
    public class rptKHV_02DT_DTQuyController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptKHV_02DT_DTQuy.xls";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_02DT_DTQuy.aspx";
            ViewData["PageLoad"] = "0";
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
            String Quy = Convert.ToString(Request.Form[ParentID + "_Quy"]);
            String sDeAn=Convert.ToString(Request.Form["sDeAn"]);
            String iCapTongHop = Convert.ToString(Request.Form[ParentID + "_iCapTongHop"]);
            ViewData["PageLoad"] = "1";
            ViewData["NgoaiTe"] = NgoaiTe;
            ViewData["Quy"] = Quy;
            ViewData["sDeAn"] = sDeAn;
            ViewData["iCapTongHop"] = iCapTongHop;
            ViewData["path"] = "~/Report_Views/QLDA/rptKHV_02DT_DTQuy.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NgoaiTe"></param>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String sDeAn, String NgoaiTe, String Quy, String iNamLamViec, String iCapTongHop)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            DataTable dtDVT = QLDA_ReportModel.getdtTien();
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (NgoaiTe == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTen"].ToString();
                }

            }
            dtDVT.Dispose();
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            //Toan lục luong
            fr = ReportModels.LayThongTinChuKy(fr, "rptKHV_02DT_DTQuy");

            LoadData(fr, sDeAn, NgoaiTe, Quy, iNamLamViec);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", Quy);
            fr.SetValue("Ngay", "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString());
            fr.SetValue("DonVi", DVT);
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
        private void LoadData(FlexCelReport fr, String sDeAn, String NgoaiTe, String Quy, String iNamLamViec)
        {
            DataTable data = LayDanhSach(sDeAn, NgoaiTe, Quy, iNamLamViec);
            data.Columns.Add("sTienDo", typeof(String));
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", data, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CT", dtHangMucCongTrinh, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DAThanhPhan", dtCongTrinh, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DA", dtDuAnThanhPhan, "NguonNS,sDeAn,sDuAn", "NguonNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sDeAn", "NguonNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("LNS", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");
            data.TableName = "Chitiet";
            fr.AddTable("HMCTiet", data);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CT", dtCongTrinh);
            fr.AddTable("DAThanhPhan", dtDuAnThanhPhan);
            fr.AddTable("DA", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("LNS", dtNguon);
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
        /// <param name="NgoaiTe"></param>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String sDeAn, String NgoaiTe, String Quy, String iNamLamViec, String iCapTongHop)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), sDeAn, NgoaiTe, Quy, iNamLamViec, iCapTongHop);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKHV_02DT_DTQuy" + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NgoaiTe"></param>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String sDeAn, String NgoaiTe, String Quy, String iNamLamViec, String iCapTongHop)
        {
            HamChung.Language();
            //ViewData["bBaoCaoTH"] = "False";
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), sDeAn, NgoaiTe, Quy, iNamLamViec, iCapTongHop);
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
        public DataTable LayDanhSach(String sDeAn, String NgoaiTe, String Quy, String iNamLamViec)
        {
            DataTable dtDanhSach = new DataTable();
            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            String iNamTruoc = Convert.ToString(Convert.ToInt16(iNamLamViec) - 1);
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                DKDeAn += "sDeAn=@sDeAn" + i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR ";
            }
            String DKNgoaiTe_KHV = "";
            String DKLoaiNgoaiTe_KHV = "";
            String DKNgoaiTe_rDeNghiPheDuyetThanhToan = "";
            String DKNgoaiTe_rDeNghiPheDuyetTamUng = "";
            String DKNgoaiTe_rDeNghiPheDuyetThuTamUng = "";
            String DKLoaiNgoaiTe_CP = "";
            String DKNgoaiTe_rSoTienBTCCap = "";
            String DKNgoaiTe_rSoTienDVDeNghi = "";
            String DKNgoaiTe_rSoTienDuToan = "";
            String DKLoaiNgoaiTe_DTQ = "";
            String ĐKDaCapPhat = "";
            if (NgoaiTe == "0")
            {
                DKNgoaiTe_KHV = "(rSoTienDauNam+rSoTienDieuChinh)/1000000";
                DKNgoaiTe_rDeNghiPheDuyetThanhToan = "rDeNghiPheDuyetThanhToan/1000000";
                DKNgoaiTe_rDeNghiPheDuyetTamUng = "rDeNghiPheDuyetTamUng/1000000";
                DKNgoaiTe_rDeNghiPheDuyetThuTamUng = "rDeNghiPheDuyetThuTamUng/1000000";
                DKNgoaiTe_rSoTienBTCCap = "rSoTienBTCCap/1000000";
                DKNgoaiTe_rSoTienDVDeNghi = "rSoTienDVDeNghi/1000000";
                DKNgoaiTe_rSoTienDuToan = "rSoTienDuToan/1000000";
                ĐKDaCapPhat = "rDeNghiPheDuyetDonViThuHuong/1000000";
            }
            else
            {
                DKNgoaiTe_KHV = "rNgoaiTe_DauNam+rNgoaiTe_DieuChinh";
                DKNgoaiTe_rDeNghiPheDuyetThanhToan = "rNgoaiTe_DeNghiPheDuyetThanhToan";
                DKNgoaiTe_rDeNghiPheDuyetTamUng = "rNgoaiTe_DeNghiPheDuyetTamUng";
                DKNgoaiTe_rDeNghiPheDuyetThuTamUng = "rNgoaiTe_DeNghiPheDuyetThuTamUng";
                DKNgoaiTe_rSoTienBTCCap = "rNgoaiTe_rSoTienBTCCap";
                DKNgoaiTe_rSoTienDVDeNghi = "rNgoaiTe_rSoTienDVDeNghi";
                DKNgoaiTe_rSoTienDuToan = "rNgoaiTe_rSoTienDuToan";
                ĐKDaCapPhat = "rNgoaiTe_DeNghiPheDuyetDonViThuHuong";
                DKLoaiNgoaiTe_KHV = " AND (iID_MaNgoaiTe_DauNam=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DieuChinh=@iID_MaNgoaiTe) ";
                DKLoaiNgoaiTe_CP = " AND (iID_MaNgoaiTe_DeNghiPheDuyetThanhToan=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng=@iID_MaNgoaiTe)";
                DKLoaiNgoaiTe_DTQ = " AND (iID_MaNgoaiTe_rSoTienBTCCap=@iID_MaNgoaiTe OR iID_MaNgoaiTe_rSoTienDVDeNghi=@iID_MaNgoaiTe OR iID_MaNgoaiTe_rSoTienDuToan=@iID_MaNgoaiTe)";
            }

            #region danh sách đợt cấp phát
            String SQLDot = String.Format(@"SELECT iID_MaDotCapPhat 
                                            FROM QLDA_CapPhat_Dot
                                            WHERE iTrangThai=1 AND
                                                  iNamLamViec=@iNamLamViec AND 
                                                  MONTH(dNgayLap)<=@iThangQuy");
            SqlCommand cmdDot= new SqlCommand(SQLDot);
            cmdDot.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmdDot.Parameters.AddWithValue("@iThangQuy", Convert.ToString((Convert.ToInt16(Quy)-1) * 3));
            DataTable dtDot = Connection.GetDataTable(cmdDot);
            cmdDot.Dispose();
            String DKCapPhat = "iID_MaDotCapPhat='-1'";

            if (dtDot.Rows.Count > 0)
            {
                DKCapPhat = "";
                for (int i = 0; i < dtDot.Rows.Count; i++)
                {
                    DKCapPhat += "iID_MaDotCapPhat=@iID_MaDotCapPhat" + i;
                    if (i < dtDot.Rows.Count - 1)
                        DKCapPhat += " OR";
                }
            }
            dtDot.Dispose();
            #endregion
            String SQLDSNS = string.Format(@"SELECT iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    sTenDuAn,
	    NguonNS,
	    SUM(KHV_NamTruoc) as KHV_NamTruoc,
	    SUM(KHV_NamNay) as KHV_NamNay,
	    SUM(KHV_UngTruoc) as KHV_UngTruoc,
	    SUM(CapThanhToan_NamTruoc) as CapThanhToan_NamTruoc,
	    SUM(CapTamUng_NamTruoc) as CapTamUng_NamTruoc,
	    SUM(ThuTamUng_NamTruoc) as ThuTamUng_NamTruoc,
	    SUM(rSoTienBTCCap) as rSoTienBTCCap,
	    SUM(rSoTienDVDeNghi) as rSoTienDVDeNghi,
	    SUM(rSoTienDuToan) as rSoTienDuToan,
 SUM(CapPhat) as CapPhat
	    FROM(
SELECT iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	    SUBSTRING(sLNS,1,1) as NguonNS,
		KHV_NamTruoc=SUM(CASE WHEN(iLoaiKeHoachVon=1 AND iNamLamViec<=@iNamTruoc) THEN {0} ELSE 0 END),
	    KHV_NamNay=SUM(CASE WHEN(iLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec) THEN {0} ELSE 0 END),
	    KHV_UngTruoc=SUM(CASE WHEN(iLoaiKeHoachVon=2 AND iNamLamViec=@iNamLamViec) THEN {0} ELSE 0 END),
		CapThanhToan_NamTruoc=0,
		CapTamUng_NamTruoc=0,
		ThuTamUng_NamTruoc=0,
		rSoTienBTCCap=0,
		rSoTienDVDeNghi=0,
		rSoTienDuToan=0,
CapPhat=0
FROM   QLDA_KeHoachVon
WHERE  MONTH(dNgayKeHoachVon)<=@iThangQuy {1} AND ({10}) AND iTrangThai=1  AND sHangMucChiTiet<>''
GROUP BY iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    SUBSTRING(sTenDuAn,19,10000),
	    SUBSTRING(sLNS,1,1)
HAVING 	SUM(CASE WHEN(iLoaiKeHoachVon=1 AND iNamLamViec=@iNamTruoc) THEN {0} ELSE 0 END)<>0
		OR SUM(CASE WHEN(iLoaiKeHoachVon=1 AND iNamLamViec=@iNamLamViec) THEN {0} ELSE 0 END)<>0
		OR SUM(CASE WHEN(iLoaiKeHoachVon=2 AND iNamLamViec=@iNamLamViec) THEN {0} ELSE 0 END)<>0     
	  UNION
	   --Cấp phát 
	    SELECT CP.iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    sTenDuAn,
	    NguonNS,
	    KHV_NamTruoc=0,
	    KHV_NamNay=0,
	    KHV_UngTruoc=0,
	    CapThanhToan_NamTruoc,
	    CapTamUng_NamTruoc,
	    ThuTamUng_NamTruoc,
	    rSoTienBTCCap=0,
		rSoTienDVDeNghi=0,
		rSoTienDuToan=0,
CapPhat
		FROM(
		SELECT iID_MaDanhMucDuAn,
	    SUBSTRING(sLNS,1,1) as NguonNS,
	    CapThanhToan_NamTruoc=SUM(CASE WHEN(iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1)) THEN {2} ELSE 0 END),
	    CapTamUng_NamTruoc=SUM(CASE WHEN(iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1)) THEN {3} ELSE 0 END),
	    ThuTamUng_NamTruoc=SUM(CASE WHEN(iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1)) THEN {4} ELSE 0 END),
	    CapThanhToan_NamNay=SUM(CASE WHEN(iNamLamViec=@iNamLamViec AND ({11})) THEN {2} ELSE 0 END),
	    CapTamUng_NamNay=SUM(CASE WHEN(iNamLamViec=@iNamLamViec AND ({11})) THEN {3} ELSE 0 END),
	    ThuTamUng_NamNay=SUM(CASE WHEN(iNamLamViec=@iNamLamViec AND ({11})) THEN {4} ELSE 0 END),
        CapPhat=SUM(CASE WHEN(iNamLamViec=@iNamLamViec AND ({11})) THEN {12} ELSE 0 END)
		FROM 	QLDA_CapPhat 
        WHERE iTrangThai=1 {5}    
		GROUP BY  iID_MaDanhMucDuAn,
				  SUBSTRING(sLNS,1,1)
		HAVING SUM(CASE WHEN(iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1)) THEN {2} ELSE 0 END)<>0
			   OR SUM(CASE WHEN(iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1)) THEN {3} ELSE 0 END)<>0	
			   OR SUM(CASE WHEN(iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1)) THEN {4} ELSE 0 END)<>0
			   OR SUM(CASE WHEN(iNamLamViec=@iNamLamViec) THEN {2} ELSE 0 END)<>0
			   OR SUM(CASE WHEN(iNamLamViec=@iNamLamViec) THEN {3} ELSE 0 END)<>0
			   OR SUM(CASE WHEN(iNamLamViec=@iNamLamViec) THEN {4} ELSE 0 END)<>0	
                OR SUM(CASE WHEN(iNamLamViec=@iNamLamViec) THEN {12} ELSE 0 END)<>0		  
				  ) as CP  
        INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                    FROM QLDA_DanhMucDuAn 
                    WHERE iTrangThai=1 AND ({10}) AND sHangMucChiTiet<>'') as DM
    ON CP.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn
    --DTQUY
    UNION
    SELECT 
        iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	    SUBSTRING(sLNS,1,1) as NguonNS,
	    KHV_NamTruoc=0,
	    KHV_NamNay=0,
	    KHV_UngTruoc=0,
	    CapThanhToan_NamTruoc=0,
	    CapTamUng_NamTruoc=0,
	    ThuTamUng_NamTruoc=0,
	    SUM({6}) as rSoTienBTCCap,
	    SUM({7}) as rSoTienDVDeNghi,
	    SUM({8}) as rSoTienDuToan  ,
CapPhat=0
	 FROM QLDA_DuToan_Quy
	 WHERE iTrangThai=1 {9} AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy AND ({10})  AND sHangMucChiTiet<>''
	 GROUP BY 	iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    SUBSTRING(sTenDuAn,19,10000),
	    SUBSTRING(sLNS,1,1)
	  HAVING SUM({6})<>0 OR SUM({7})<>0 OR SUM({8})<>0
    ) as DTQUY
    GROUP BY iID_MaDanhMucDuAn,
	    sDeAn,
	    sDuAn,
	    sDuAnThanhPhan,
	    sCongTrinh,
	    sHangMucCongTrinh,
	    sHangMucChiTiet,
	    sTenDuAn,
	    NguonNS", DKNgoaiTe_KHV, DKLoaiNgoaiTe_KHV, DKNgoaiTe_rDeNghiPheDuyetThanhToan, DKNgoaiTe_rDeNghiPheDuyetTamUng, DKNgoaiTe_rDeNghiPheDuyetThuTamUng, DKLoaiNgoaiTe_CP, DKNgoaiTe_rSoTienBTCCap, DKNgoaiTe_rSoTienDuToan, DKNgoaiTe_rSoTienDVDeNghi, DKLoaiNgoaiTe_DTQ, DKDeAn, DKCapPhat, ĐKDaCapPhat);
            SqlCommand cmd = new SqlCommand(SQLDSNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iNamTruoc", iNamTruoc);
            cmd.Parameters.AddWithValue("@iQuy", Quy);
            cmd.Parameters.AddWithValue("@iThangQuy", Convert.ToString(Convert.ToInt16(Quy)*3));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            if (dtDot.Rows.Count > 0)
            {
                for (int i = 0; i < dtDot.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDotCapPhat" + i, dtDot.Rows[i]["iID_MaDotCapPhat"].ToString());    
                }
            }
            if (NgoaiTe != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe",NgoaiTe);
            }
            dtDanhSach = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDanhSach;
        }
        public class QLDA_DTQuy
        {
            public String DeAn { get; set; }
            public String NgoaiTe { get; set; }
        }
        [HttpGet]
        public JsonResult ds_QLDA(String ParentID, String sDeAn, String NgoaiTe)
        {
            return Json(obj_QLDA(ParentID, sDeAn, NgoaiTe), JsonRequestBehavior.AllowGet);
        }
        public QLDA_DTQuy obj_QLDA(String ParentID, String sDeAn, String NgoaiTe)
        {
            QLDA_DTQuy data = new QLDA_DTQuy();

            #region đề án
            String input = "";
            DataTable dtDeAn = QLDA_ReportModel.dt_DeAn_all();
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

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDeAn\" id=\"sDeAn\" name=\"sDeAn\"  />", MaDeAn, _Checked);
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
            DataTable dtNgoaiTe = QLDA_ReportModel.getdtTien();
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTen");
            String sNgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, NgoaiTe, "NgoaiTe", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            data.NgoaiTe = sNgoaiTe;
            #endregion
            return data;
        }
    }
}
