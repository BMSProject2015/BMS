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
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_1040100_TungDonVi_KiemSoLieuController : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_1040100_TungDonVi_KiemSoLieu.xls";
       

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1040100_TungDonVi_KiemSoLieu.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_MaNganh = Request.Form["iID_MaNganh"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaNganh"] = iID_MaNganh;
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1040100_TungDonVi_KiemSoLieu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi,String iID_MaNganh)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);


            String sTenDonVi = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID", iID_MaDonVi, "sTenNganh"));
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaDonVi, iID_MaNganh);
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_1040100_TungDonVi_KiemSoLieu");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        //1020000
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptDuToan_1040100_TungDonVi_KiemSoLieu(String MaND, String iID_MaDonVi, String iID_MaNganh)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "",DKChonDV="",DKChonnganh="";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
          //  DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String[] arrDonVi = iID_MaDonVi.Split(',');
            String[] arrNganh = iID_MaNganh.Split(',');

            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKChonDV += "  iiD_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                if (i < arrDonVi.Length - 1)
                    DKChonDV += " OR ";
            }
            if (!string.IsNullOrEmpty(DKChonDV)) DKChonDV = " AND (" + DKChonDV + ")";


            for (int i = 0; i < arrNganh.Length; i++)
            {
                String DSNganh = "";
                String iID_MaNganhMLNS = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID_MaNganh", arrNganh[i], "iID_MaNganhMLNS"));
                DKChonnganh += " sNG IN (" + iID_MaNganhMLNS + ")";
                if (i < arrNganh.Length - 1)
                    DKChonnganh += " OR ";
            }
            if (!string.IsNullOrEmpty(DKChonnganh)) DKChonnganh = " AND (" + DKChonnganh + ")";


           
         

            int DVT = 1000;
            String SQL = String.Format(@"SELECT Nguon='1020100',Loai=1,iID_MaPhongBan,iID_MaPhongBanDich=iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
,SUM(rTuChi) as rTuChi
,SUM(rHienVat) as rHienVat
FROM DT_ChungTuChiTiet
WHERE sLNS='1020100' AND iTrangThai=1  AND iNamLamViec=@iNamLamViec {0} {1}
GROUP BY iID_MaPhongBan,iID_MaPhongBanDich,iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0

UNION

SELECT Nguon='1040100',Loai=2,iID_MaPhongBan,iID_MaPhongBanDich,iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
,SUM(rTuChi) as rTuChi
,SUM(rHienVat) as rHienVat
FROM DT_ChungTuChiTiet_PhanCap
WHERE sLNS='1020100'  AND iTrangThai=1 AND (MaLoai='' OR MaLoai='1') AND iNamLamViec=@iNamLamViec {0} {1}
GROUP BY iID_MaPhongBan,iID_MaPhongBanDich,iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0

UNION

SELECT Nguon='1040100',Loai=3,iID_MaPhongBan,iID_MaPhongBanDich,iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
,SUM(rTuChi) as rTuChi
,SUM(rHienVat) as rHienVat
FROM DT_ChungTuChiTiet_PhanCap
WHERE sLNS='1020100'  AND iTrangThai=1 AND (MaLoai='2') AND iNamLamViec=@iNamLamViec {0} {1}
GROUP BY iID_MaPhongBan,iID_MaPhongBanDich,iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0
ORDER BY iID_MaDonVi
", DKChonDV, DKChonnganh);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaNganh)
        {
            DataRow r;
            DataTable data = DT_rptDuToan_1040100_TungDonVi_KiemSoLieu(MaND, iID_MaDonVi, iID_MaNganh);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtNG = HamChung.SelectDistinct("dtsNG", data, "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "");
            fr.AddTable("dtsNG", dtNG);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", dtNG, "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich,sLNS,sL,sK,sM,sTM", "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich,sLNS,sL,sK,sM", "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtPhongBanDich = HamChung.SelectDistinct("dtPhongBanDich", dtsM, "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich", "Nguon,Loai,iID_MaPhongBan,iID_MaPhongBanDich", "");
            DataTable dtPhongBanNguon = HamChung.SelectDistinct("dtPhongBanNguon", dtPhongBanDich, "Nguon,Loai,iID_MaPhongBan", "Nguon,Loai,iID_MaPhongBan", "");
            DataTable dtLoai = HamChung.SelectDistinct("dtLoai", dtPhongBanNguon, "Nguon,Loai", "Nguon,Loai", "");
            DataTable dtNguon = HamChung.SelectDistinct("dtNguon", dtLoai, "Nguon", "Nguon", "");

            fr.AddTable("dtsNG", dtNG);
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtPhongBanDich", dtPhongBanDich);
            fr.AddTable("dtPhongBanNguon", dtPhongBanNguon);
            fr.AddTable("dtLoai", dtLoai);
            fr.AddTable("dtNguon", dtNguon);
            data.Dispose();
        }
      
        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaNganh)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaNganh);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_1020000_TungDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_MaNganh)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaNganh);
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
        }
    }
}

