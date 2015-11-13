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
    public class rptQLDA_04_CPController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_04_CP.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_04_CP.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_viiID_MaDotCapPhat"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["MaTien"] = MaTien;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_04_CP.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dt_rptQLDA_04_CP(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
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
            dtDotCapPhat.Dispose();
            String NamLamViec = "2000";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            if (dtCauHinh.Rows.Count > 0)
            {
                NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String DK_KHV_NgoaiTe_rSoTien = "";
            String DK_KHV_LoaiNgoaiTe = "";
            String DK_CP_NgoaiTe_TamUng = "";
            String DK_CP_NgoaiTe_ThuTamUng = "";
            String DK_CP_NgoaiTe_ThanhToan = "";
            String DK_CP_LoaiNgoaiTe = "";
            if (MaTien == "0")
            {
                DK_KHV_NgoaiTe_rSoTien = "rSoTien/1000000";
                DK_CP_NgoaiTe_TamUng = "rDeNghiPheDuyetTamUng/1000000";
                DK_CP_NgoaiTe_ThuTamUng = "rDeNghiPheDuyetThuTamUng/1000000";
                DK_CP_NgoaiTe_ThanhToan = "rDeNghiPheDuyetThanhToan/1000000";
                
            }
            else
            {
                DK_KHV_NgoaiTe_rSoTien = "rNgoaiTe_SoTien";
                DK_CP_NgoaiTe_TamUng = "rNgoaiTe_DeNghiPheDuyetTamUng";
                DK_CP_NgoaiTe_ThuTamUng = "rNgoaiTe_DeNghiPheDuyetThuTamUng";
                DK_CP_NgoaiTe_ThanhToan = "rNgoaiTe_DeNghiPheDuyetThanhToan";
                DK_KHV_LoaiNgoaiTe = " (iID_MaNgoaiTe_SoTien=@iID_MaNgoaiTe) AND ";
                DK_CP_LoaiNgoaiTe = " (iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThanhToan=@iID_MaNgoaiTe) AND ";
            }
            DataTable dt = new DataTable() ;
            String SQL = String.Format(@"
	SELECT NguonNS,ChiTiet.sLNS,ChiTiet.iID_MaDanhMucDuAn, ChiTiet.sDeAN,ChiTiet.sDuAn,ChiTiet.sDuAnThanhPhan,ChiTiet.sCongTrinh,ChiTiet.sHangMucCongTrinh,ChiTiet.sHangMucChiTiet,ChiTiet.sTenDuAn
	,SUBSTRING(sTenDonViThiCong,8,1000000) as sTenDonViThiCong,iID_MaDonViThiCong,sSoHopDong
	,SUM(rSoTienHopDong) as rSoTienHopDong
	,SUM(rSoTienDuToan) as rSoTienDuToan
	,SUM(rDeNghiPheDuyetThanhToan) as rDeNghiPheDuyetThanhToan
	,SUM(rDeNghiPheDuyetThanhToan_LuyKe) as rDeNghiPheDuyetThanhToan_LuyKe
	,SUM(rDeNghiPheDuyetTamUng) as rDeNghiPheDuyetTamUng
	,SUM(rDeNghiPheDuyetThuTamUng) as rDeNghiPheDuyetThuTamUng
	FROM( 
	SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000) as sTenDuAn,
			SUM({0}) as rSoTienHopDong,rSoTienDuToan=0,rDeNghiPheDuyetThanhToan=0,rDeNghiPheDuyetThanhToan_LuyKe=0,rDeNghiPheDuyetTamUng=0,rDeNghiPheDuyetThuTamUng=0
	FROM QLDA_HopDongChiTiet
	WHERE iTrangThai=1 AND  dNgayLap<=@dNgayLap AND {5} iNamLamViec<=@iNamLamViec  AND sHangMucChiTiet<>''
    GROUP BY SUBSTRING(sLNS,1,1),sLNS, iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaDonViThiCong,sTenDonViThiCong
    UNION 
    SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn, sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,SUBSTRING(sTenDuAn,19,100000)as sTenDuAn,
			rSoTienHopDong=0,SUM({0}) as rSoTienDuToan,rDeNghiPheDuyetThanhToan=0,rDeNghiPheDuyetThanhToan_LuyKe=0,rDeNghiPheDuyetTamUng=0,rDeNghiPheDuyetThuTamUng=0
	FROM QLDA_TongDuToan
    WHERE iTrangThai=1 AND  dNgayPheDuyet<=@dNgayLap AND {5} iNamLamViec<=@iNamLamViec AND sHangMucChiTiet<>''
	GROUP BY SUBSTRING(sLNS,1,1), sLNS,iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn
    
    UNION
    
     SELECT   SUBSTRING(sLNS,1,1) as NguonNS,sLNS,QLDA_CapPhat.iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,
						 rSoTienHopDong=0,rSoTienDuToan=0,
                          rDeNghiPheDuyetThanhToan=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN  {3} ELSE 0 END),
                          rDeNghiPheDuyetThanhToan_LuyKe=SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN  {3} ELSE 0 END),
                          rDeNghiPheDuyetTamUng=SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN  {1} ELSE 0 END),
                           rDeNghiPheDuyetThuTamUng=SUM(CASE WHEN iNamLamViec<=@iNamLamViec THEN  {2} ELSE 0 END)
	FROM QLDA_CapPhat
	  INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                  FROM QLDA_DanhMucDuAn 
                  WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as QLDA_DanhMucDuAn
                  ON QLDA_CapPhat.iID_MaDanhMucDuAn=QLDA_DanhMucDuAn.iID_MaDanhMucDuAn
WHERE   {4} QLDA_CapPhat.iTrangThai=1 AND iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE iTrangThai=1 AND dNgayLap<=@dNgayLap)
                  GROUP BY SUBSTRING(sLNS,1,1),sLNS, QLDA_CapPhat.iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet) as ChiTiet
         LEFT JOIN (SELECT * FROM QLDA_HopDongChiTiet WHERE iTrangThai=1) as QLDA_HopDongChiTiet ON  ChiTiet.iID_MaDanhMucDuAn= QLDA_HopDongChiTiet. iID_MaDanhMucDuAn 
           LEFT JOIN (SELECT iID_MaHopDong,sSoHopDong FROM QLDA_HopDong WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec) QLDA_HopDong 
            ON  QLDA_HopDongChiTiet.iID_MaHopDong=QLDA_HopDong.iID_MaHopDong
       GROUP BY NguonNS,ChiTiet.sLNS,ChiTiet.iID_MaDanhMucDuAn, ChiTiet.sDeAN,ChiTiet.sDuAn,ChiTiet.sDuAnThanhPhan,ChiTiet.sCongTrinh,ChiTiet.sHangMucCongTrinh,ChiTiet.sHangMucChiTiet,ChiTiet.sTenDuAn,sTenDonViThiCong,iID_MaDonViThiCong,sSoHopDong
    HAVING SUM(rSoTienHopDong) <>0 OR
	SUM(rSoTienDuToan) <>0 OR
	SUM(rDeNghiPheDuyetThanhToan) <>0 OR
	SUM(rDeNghiPheDuyetThanhToan_LuyKe) <>0 OR
	SUM(rDeNghiPheDuyetTamUng) <>0 OR
	SUM(rDeNghiPheDuyetThuTamUng) <>0
	", DK_KHV_NgoaiTe_rSoTien, DK_CP_NgoaiTe_TamUng, DK_CP_NgoaiTe_ThuTamUng, DK_CP_NgoaiTe_ThanhToan, DK_CP_LoaiNgoaiTe, DK_KHV_LoaiNgoaiTe);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmd.Parameters.AddWithValue("@dNgayLap",  CommonFunction.LayNgayTuXau(dNgayLap));
            if (MaTien != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public ActionResult ViewPDF(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDotCapPhat, MaND,MaTien);
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
        public clsExcelResult ExportToExcel(String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

         

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDotCapPhat, MaND, MaTien);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopChiTieuDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ExcelFile CreateReport(String path, String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
           // DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            String dNgayLap = "01/01/2000";
            dNgayLap = iID_MaDotCapPhat;
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            //dtDotCapPhat.Dispose();
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_04_CP");
            LoadData(fr, iID_MaDotCapPhat, MaND,MaTien);
            fr.SetValue("DVT", DVT);
            fr.SetValue("DotCapPhat", DotCapPhat);

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iID_MaDotCapPhat, String MaND,String MaTien)
        {

            DataTable data = dt_rptQLDA_04_CP(iID_MaDotCapPhat, MaND, MaTien);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Columns.Add("sTienDo", typeof(String));
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
            for (int i = 0; i < dtLNS.Rows.Count;i++)
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
            fr.AddTable("HMChiTiet", dtHangMucChiTiet);
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
