using System;
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
using VIETTEL.Models.QuyetToan;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptQuyetToanNam_ChiTietSoChuyenNamSauController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_ChiTietSoChuyenNamSau.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_ChiTietSoChuyenNamSau_TongHopDenDonVi.xls";

        public ActionResult Index()
        {
            FlexCelReport fr= new FlexCelReport();
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_ChiTietSoChuyenNamSau.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptQuyetToanNam_ChiTietSoChuyenNamSau(String MaND,String iID_MaPhongBan,String iID_MaDonVi,String iID_MaNamNganSach)
        {
            String DKDonVi = "",DKPhongBan="",DK=""; 
            SqlCommand cmd = new SqlCommand();
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += "iID_MaDonVi=@MaDonVi" + i;
                cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
            }
            if (!String.IsNullOrEmpty(DK))
                DK = " AND (" + DK + ")";
           
                DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
                DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
              
            String SQL =
                String.Format(@"SELECT a.iID_MaDonVi
,sTenDonVi,sTenTomTat
,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,sXauNoiMa
,rChuaCap_NamNay
,rChuaCap_NamTruoc
,rDaCap_NamNay
,rDaCap_NamTruoc
 FROM
(
SELECT iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,sXauNoiMa
,rChuaCap_NamNay=SUM(CASE WHEN iID_MaNamNganSach=2 THEN rChuaCapTien ELSE 0 END)
,rChuaCap_NamTruoc=SUM(CASE WHEN iID_MaNamNganSach=1 THEN rChuaCapTien ELSE 0 END)
,rDaCap_NamNay=SUM(CASE WHEN iID_MaNamNganSach=2 THEN rDaCapTien ELSE 0 END)
,rDaCap_NamTruoc=SUM(CASE WHEN iID_MaNamNganSach=1 THEN rDaCapTien ELSE 0 END)
FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
GROUP BY iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,sXauNoiMa
HAVING SUM(CASE WHEN iID_MaNamNganSach=2 THEN rChuaCapTien ELSE 0 END)<>0
OR SUM(CASE WHEN iID_MaNamNganSach=1 THEN rChuaCapTien ELSE 0 END)<>0
OR SUM(CASE WHEN iID_MaNamNganSach=2 THEN rDaCapTien ELSE 0 END)<>0
OR SUM(CASE WHEN iID_MaNamNganSach=1 THEN rDaCapTien ELSE 0 END)<>0
) a
INNER JOIN (
SELECT iID_MaDonVi,sTenTomTat 
FROM NS_DonVi
WHERE iNamLamViec_DonVi=@iNamLamViec AND iTrangThai=1) b
ON a.iID_MaDonVi=b.iID_MaDonVi
", DKPhongBan,DKDonVi,DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            dt.Columns.Add("sGhiChu", typeof (String));
            SQL = String.Format(@"SELECT sXauNoiMa,sGhiChu FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND sGhiChu<>'' AND iNamLamViec=@iNamLamViec {0} {1} {2}", DKPhongBan, DKDonVi, DK);

           // cmd = new SqlCommand(); ;
            cmd.CommandText = SQL;
            //cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtGhiChu = Connection.GetDataTable(cmd);

            foreach(DataRow dr in dtGhiChu.Rows)
            {
                string sXauNoiMa = Convert.ToString(dr["sXauNoiMa"]);
                DataRow[] arrR = dt.Select("sXauNoiMa='" + sXauNoiMa + "'");
                for (int i = 0; i < arrR.Length; i++)
                {
                    if(sXauNoiMa==Convert.ToString(arrR[i]["sXauNoiMa"]))
                    {
                        arrR[i]["sGhiChu"] = dr["sGhiChu"];
                    }
                }

            }
            return dt;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaPhongBan = Request.Form["QuyetToanNhanh" + "_iID_MaPhongBan"];
            String MaND = Request.Form["QuyetToanNhanh" + "_MaND"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_ChiTietSoChuyenNamSau.aspx";
            return RedirectToAction("ViewPDF", new { MaND = MaND,iID_MaDonVi = iID_MaDonVi, iID_MaPhongBan = iID_MaPhongBan });
           }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan, String iID_MaDonVi, String iID_MaNamNganSach,String iLoai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToanNam_ChiTietSoChuyenNamSau");

            LoadData(fr, MaND, iID_MaPhongBan, iID_MaDonVi,iID_MaNamNganSach);
            String Nam = ReportModels.LayNamLamViec(MaND);

            //lay ten nam ngan sach
            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
                NamNganSach = "QUYẾT TOÁN NẮM TRƯỚC";
            else if (iID_MaNamNganSach == "2")
                NamNganSach = "QUYẾT TOÁN NĂM NAY";
            else
            {
                NamNganSach = "TỔNG HỢP";
            }
            //Ten mâu bieu
            String TenBieu = "";
            if (iID_MaDonVi =="-1")
            {
                if (iID_MaPhongBan == "-1") TenBieu = "Tổng hợp cục - Biểu 2A";
                else TenBieu = "Tổng hợp B - Biểu 2A";
            }
            else
            {
                TenBieu = "Chi tiết - Biểu 2";
            }
            //Ten BQL
            String BQuanLy = "";
            if (iID_MaPhongBan != "-1")
                BQuanLy = " BQL - " + iID_MaPhongBan;
            //Ten Don vi
            String sTenDonVi = "";
            if (iID_MaDonVi != "-1")
                sTenDonVi = "Đơn vị : " + DonViModels.Get_TenDonVi(iID_MaDonVi);
            String TenPB = "";
            if (iID_MaPhongBan != "-1")
                TenPB = " B - " + iID_MaPhongBan;
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("NamNganSach", NamNganSach);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("TenBieu", TenBieu);
            fr.SetValue("BQuanLy", BQuanLy);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan, String iID_MaDonVi,String iID_MaNamNganSach)
        {
            DataRow r;
            DataTable data = rptQuyetToanNam_ChiTietSoChuyenNamSau(MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,iID_MaDonVi,sL,sK,sM,sTM", "sLNS,iID_MaDonVi,sTenTomTat,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,iID_MaDonVi,sL,sK,sM", "sLNS,iID_MaDonVi,sTenTomTat,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,iID_MaDonVi,sL,sK", "sLNS,iID_MaDonVi,sTenTomTat,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtDonVi = HamChung.SelectDistinct("dtDonVi", dtsL, "sLNS,iID_MaDonVi,sL,sK", "sLNS,iID_MaDonVi,sTenTomTat,sL,sK,sMoTa", "");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtDonVi, "sLNS", "sLNS,sMoTa", "sLNS,sL");

            fr.AddTable("dtsTM",dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtDonVi", dtDonVi);
            fr.AddTable("dtsLNS", dtsLNS);

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtDonVi.Dispose();
            dtsLNS.Dispose();

        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}",sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan, String iID_MaDonVi,String iID_MaNamNganSach,String iLoai)
        {
            HamChung.Language();
            String sDuongDan = "";
               if (iLoai == "DonVi")
                    sDuongDan = sFilePath1;
                else sDuongDan = sFilePath;

               ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach, iLoai);
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

        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan, String iID_MaDonVi, String iID_MaNamNganSach, String iLoai)
        {
            String sDuongDan = "";
            if (iLoai == "DonVi")
                sDuongDan = sFilePath1;
            else sDuongDan = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach, iLoai);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChiTietSoDeNghiChuyenSangNamSau.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi, String MaND)
        {
            DataTable dt = QuyetToan_ReportModels.dtDonVi_ChuyenNamSau(iID_MaPhongBan,MaND);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

