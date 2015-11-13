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
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptQuyetToanNam_2aController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_2a_TongHopDenLNS.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_2a_TongHopDenDonVi.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_2a.aspx";
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
        public static DataTable rptQuyetToanNam_2a(String MaND,String iID_MaPhongBan,String iID_MaDonVi,String iID_MaNamNganSach)
        {
            String DKDonVi = "",DKPhongBan="",DK=""; 
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi!="-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if(iID_MaNamNganSach=="2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if(iID_MaNamNganSach=="1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
                 DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
                 DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
              
            String SQL =
                String.Format(@"SELECT iID_MaDonVi,sTenDonVi,SUBSTRING(a.sLNS,1,1) as sLNS1
,SUBSTRING(a.sLNS,1,3) as sLNS3
,SUBSTRING(a.sLNS,1,5)as sLNS5,a.sLNS,sMoTa
,rChiTieu
,rQuyetToan
,rDonViDeNghi
,rVuotChiTieu
,rTonThatTonDong
,rDaCapTien
,rChuaCapTien
FROM(
SELECT iID_MaDonVi,sTenDonVi,sLNS
,SUM(rChiTieu) as rChiTieu
,SUM(rQuyetToan) as rQuyetToan
,SUM(rDonViDeNghi) as rDonViDeNghi
,SUM(rVuotChiTieu) as rVuotChiTieu
,SUM(rTonThatTonDong) as rTonThatTonDong
,SUM(rDaCapTien) as rDaCapTien
,SUM(rChuaCapTien) as rChuaCapTien
FROM (
SELECT iID_MaDonVi,sTenDonVi,sLNS
,rChiTieu=0
,SUM(rTuChi) as rQuyetToan
,SUM(rDonViDeNghi) as rDonViDeNghi
,SUM(rVuotChiTieu) rVuotChiTieu
,SUM(rTonThatTonDong) as rTonThatTonDong 
,SUM(rDaCapTien)as rDaCapTien
,SUM(rChuaCapTien) as rChuaCapTien 
FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
GROUP BY iID_MaDonVi,sTenDonVi,sLNS

UNION

SELECT iID_MaDonVi,sTenDonVi,sLNS,
SUM(rTuChi) as rChiTieu
,rQuyetToan=0
,rDonViDeNghi=0
,rVuotChiTieu=0
,rTonThatTonDong=0
,rDaCapTien=0
,rChuaCapTien=0
FROM PB_PhanBoChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
GROUP BY iID_MaDonVi,sTenDonVi,sLNS) as ChiTiet
GROUP BY iID_MaDonVi,sTenDonVi,sLNS
HAVING SUM(rQuyetToan) <>0 OR
SUM(rDonViDeNghi) <>0 OR
SUM(rVuotChiTieu) <>0 OR
SUM(rTonThatTonDong) <>0 OR
SUM(rDaCapTien) <>0 OR
SUM(rChuaCapTien) <>0
) a
INNER JOIN 
(SELECT sLNS,sMoTa FROM NS_MucLucNganSach
WHERE iTrangThai=1 AND LEN(sLNS)=7 AND sL='') b
ON a.sLNS=b.sLNS
",DKPhongBan,DKDonVi,DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
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
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iLoai = Request.Form[ParentID + "_iLoai"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iLoai"] = iLoai;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_2a.aspx";
            return View(sViewPath + "ReportView.aspx");
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToanNam_2a");

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
            DataTable data = rptQuyetToanNam_2a(MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtLNS = HamChung.SelectDistinct("dtLNS", data, "sLNS1,sLNS3,sLNS5,sLNS", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");

           
           
            DataTable dtLNS5 = HamChung.SelectDistinct("dtLNS5", dtLNS, "sLNS1,sLNS3,sLNS5", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");
            for (int i = 0; i < dtLNS5.Rows.Count; i++)
            {
                r = dtLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
           
            DataTable dtLNS3 = HamChung.SelectDistinct("dtLNS3", dtLNS5, "sLNS1,sLNS3", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");
            for (int i = 0; i < dtLNS3.Rows.Count; i++)
            {
                r = dtLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }

            DataTable dtLNS1 = HamChung.SelectDistinct("dtLNS1", dtLNS3, "sLNS1", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");

            for (int i = 0; i < dtLNS1.Rows.Count; i++)
            {
                r = dtLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }
            fr.AddTable("dtLNS", dtLNS);
            fr.AddTable("dtLNS5", dtLNS5);
            fr.AddTable("dtLNS3", dtLNS3);
            fr.AddTable("dtLNS1", dtLNS1);
            dtLNS.Dispose();
            dtLNS5.Dispose();
            dtLNS3.Dispose();
            dtLNS1.Dispose();
            data.Dispose();
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
                clsResult.FileName = "BaoCaoSoLieuKetLuanQuyetToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi, String MaND)
        {
            return Json(obj_DonVi(ParentID, iID_MaPhongBan, iID_MaDonVi,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi,String MaND)
        {
            String input = "";
            DataTable dt = DonViModels.DanhSach_DonVi_QuyetToan_PhongBan(iID_MaPhongBan, MaND);
            SelectOptionList slDonvi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return input;
        }

    }
}

