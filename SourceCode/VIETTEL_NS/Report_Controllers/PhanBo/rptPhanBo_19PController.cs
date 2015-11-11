using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptPhanBo_19PController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/PhanBo/rptPhanBo_19P_A3.xls";
        private const String sFilePath_A4 = "/Report_ExcelFrom/PhanBo/rptPhanBo_19P_A4.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_19P.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {

            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_19P.aspx";
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TruongTien"] = TruongTien;
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String TenDV;
            String TenTruongTien = "";
            if (TruongTien == "rTuChi")
            {
                TenTruongTien = "Tự chi";
            }
            else
            {
                TenTruongTien = "Hiện vật";
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_19P");
            LoadData(fr, MaND, sLNS, iID_MaDonVi, TruongTien, iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("TruongTien", TenTruongTien);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.Run(Result);
            return Result;

        }
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dtPhanBo_19R(MaND, sLNS, iID_MaDonVi, TruongTien, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,Nam,Loai,sLNS,sL,sK,sM,sTM", "NguonNS,Nam,Loai,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,Nam,Loai,sLNS,sL,sK,sM", "NguonNS,Nam,Loai,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,Nam,Loai,sLNS", "NguonNS,Nam,Loai,sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtLoai;
            dtLoai = HamChung.SelectDistinct("Loai", dtLoaiNS, "NguonNS,Nam,Loai", "NguonNS,Nam,Loai,sMoTa", "sLNS,sL", "Loai");
            fr.AddTable("Loai", dtLoai);
            DataRow[] dr = dtLoai.Select("Loai='101'");
            for (int i = 0; i < dr.Length; i++)
            {
                dr[i]["sMoTa"] = "1.Ngân sách sử dụng";
            }

            DataTable dtNam;
            dtNam = HamChung.SelectDistinct("Nam", dtLoai, "NguonNS,Nam", "NguonNS,Nam,sMoTa");
            fr.AddTable("Nam", dtNam);
            DataRow[] dr1 = dtNam.Select("Nam='0'");
            for (int i = 0; i < dr1.Length; i++)
            {
                dr1[i]["sMoTa"] = "I. Năm trước";
            }
            DataRow[] dr2 = dtNam.Select("Nam='1'");
            for (int i = 0; i < dr2.Length; i++)
            {
                dr2[i]["sMoTa"] = "II. Năm nay";
            }
            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtNam, "NguonNS", "NguonNS,sMoTa", "", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);

            dtNam.Dispose();
            dtLoai.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet,String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if(KhoGiay=="1")
                DuongDanFile=sFilePath_A3;
            else DuongDanFile = sFilePath_A4;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, sLNS, iID_MaDonVi, TruongTien, iID_MaTrangThaiDuyet,KhoGiay);
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
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (KhoGiay == "1")
                DuongDanFile = sFilePath_A3;
            else DuongDanFile = sFilePath_A4;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, sLNS, iID_MaDonVi, TruongTien, iID_MaTrangThaiDuyet, KhoGiay);
            clsExcelResult clsResult = new clsExcelResult();
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "phanbo_19P.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public static DataTable dtPhanBo_19R(String MaND, String sLNS, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            //lay cai hinh nguoi dung
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(); String iID_MaNguonNganSach = "1"; String DKPhongBan_DonVi = "";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
                iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
                String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
                String iID_MaDonVi_PQ = "";
                iID_MaDonVi_PQ = NguoiDung_DonViModels.getDonViByNguoiDung_1(MaND);
                DKPhongBan_DonVi = String.Format(@" AND (iID_MaPhongBan='{0}' ) AND (iID_MaDonVi IN ({1}))",iID_MaPhongBan,iID_MaDonVi_PQ);
            }
            dtCauHinh.Dispose();
            //sLNS
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            //iID_MaDonVi
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String DKDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }

            //Trang thai duyeit 
            String DKTrangThai_PB = "";
            String DKTrangThai_QT = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKTrangThai_PB = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
                DKTrangThai_QT = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_QT";
            }
            String SQL = String.Format(@"SELECT NguonNS=SUBSTRING(sLNS,1,1),
	   Nam,
	   Loai=CASE WHEN (SUBSTRING(sLNS,1,3)=101 OR SUBSTRING(sLNS,1,3)=102) THEN 101 ELSE  SUBSTRING(sLNS,1,3) END
	   ,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
       SUM(TongSo) as TongSo,
       SUM(ChuyenNamSau) as ChuyenNamSau,
       SUM(ThucHien) as ThucHien
       FROM(       
--Nam Truoc
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,Nam=0,
       TongSo=SUM(CASE WHEN (iID_MaNamNganSach=1) THEN {3} ELSE 0 END),
       ChuyenNamSau=SUM(CASE WHEN (iID_MaNamNganSach=4) THEN {3} ELSE 0 END),
       ThucHien=0
FROM PB_PhanBoChiTiet
WHERE iTrangThai=1 AND sNG<>''
	  AND iNamLamViec=@iNamLamViec
      AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND ({0}) AND ({1}) {2} {5}
GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING SUM(CASE WHEN (iID_MaNamNganSach=1) THEN {3} ELSE 0 END)<>0
       OR SUM(CASE WHEN (iID_MaNamNganSach=4) THEN {3} ELSE 0 END)<>0

UNION 

SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,Nam=0,
       TongSo=0,
       ChuyenNamSau=0,
       ThucHien=SUM(CASE WHEN (iID_MaNamNganSach=1) THEN {3} ELSE 0 END)
FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND sNG<>''
	  AND iNamLamViec=@iNamLamViec
      AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND ({0}) AND ({1}) {4} {5}
GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING SUM(CASE WHEN (iID_MaNamNganSach=1) THEN {3} ELSE 0 END)<>0
 UNION 
--Nam nay
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,Nam=1,
       TongSo=SUM(CASE WHEN (iID_MaNamNganSach=2) THEN {3} ELSE 0 END),
       ChuyenNamSau=SUM(CASE WHEN (iID_MaNamNganSach=3) THEN {3} ELSE 0 END),
       ThucHien=0
FROM PB_PhanBoChiTiet
WHERE iTrangThai=1 AND sNG<>''
	  AND iNamLamViec=@iNamLamViec
      AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND ({0}) AND ({1}) {2}  {5}
GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING  SUM(CASE WHEN (iID_MaNamNganSach=2) THEN {3} ELSE 0 END)<>0
        OR SUM(CASE WHEN (iID_MaNamNganSach=3) THEN {3} ELSE 0 END)<>0
UNION 
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,Nam=1,
       TongSo=0,
       ChuyenNamSau=0,
       ThucHien=SUM(CASE WHEN (iID_MaNamNganSach=2) THEN {3} ELSE 0 END)
FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND sNG<>''
	  AND iNamLamViec=@iNamLamViec
      AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND ({0}) AND ({1}) {4}  {5}
GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING SUM(CASE WHEN (iID_MaNamNganSach=2) THEN {3} ELSE 0 END) <>0) a
GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,Nam,sMoTa
HAVING SUM(TongSo)<>0 OR   SUM(ChuyenNamSau)<>0 OR  SUM(ThucHien)<>0
ORDER BY NguonNS,Nam,Loai
", DKLNS, DKDonVi, DKTrangThai_PB, TruongTien, DKTrangThai_QT, DKPhongBan_DonVi);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("iID_MaNguonNganSach", iID_MaNguonNganSach);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_QT", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
        //phần cấu hình tiêu dề

        public class TieuDe
        {
            public String sDong1 { get; set; }
            public String sDong2 { get; set; }
            public String sDong3 { get; set; }
            public String sDong4 { get; set; }
            public String sMauBaoCao { get; set; }
        }
        public static TieuDe HienThiTieuDe(String bHienThi)
        {
            TieuDe tieude = new TieuDe();
            String SQL = @"SELECT  * FROM PB_ThamSo
                         WHERE bHienThi=@bHienThi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@bHienThi", bHienThi);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                tieude.sDong1 = dt.Rows[0]["sNoiDung"].ToString();
                tieude.sDong2 = dt.Rows[1]["sNoiDung"].ToString();
                tieude.sDong3 = dt.Rows[2]["sNoiDung"].ToString();
                tieude.sDong4 = dt.Rows[3]["sNoiDung"].ToString();
                tieude.sMauBaoCao = dt.Rows[4]["sNoiDung"].ToString();
            }
            return tieude;
        }
        public static void UpdateThamSo(String sDong1, String sDong2, String sDong3, Single sMauBaoCao, String bHienThi)
        {
            String SQL = String.Format(@"UPDATE PB_ThamSo
                                         SET sDong1=@sDong1 AND sDong2=@sDong2 AND sDong3=@sDong3 AND sMauBaoCao=@sMauBaoCao
                                         WHERE bHienThi=@bHienThi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@bHienThi", bHienThi);
            cmd.Parameters.AddWithValue("@sDong1", sDong1);
            cmd.Parameters.AddWithValue("@sDong2", sDong2);
            cmd.Parameters.AddWithValue("@sDong3", sDong3);
            cmd.Parameters.AddWithValue("@sMauBaoCao", sMauBaoCao);
            int update = Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
    }
}

