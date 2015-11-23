using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using VIETTEL.Models.QuyetToan;

namespace VIETTEL.Report_Controllers.QuyetToan.QuyetToanQuanSo
{
    public class rptQTQS_ThangController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuanSo/rptQTQS_Thang.xls";
        private const String sFilePathBinhQuan = "/Report_ExcelFrom/QuyetToan/QuyetToanQuanSo/rptQTQS_ThangBinhQuan.xls";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_Thang.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public JsonResult Ds_DonVi(string ParentID, string PhongBan, string TuThang, string DenThang, string iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            DataTable dt = QuyetToan_ReportModels.QTQS_DSDonVi(PhongBan, TuThang, DenThang, MaND);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSubmit(string ParentID)
        {
            string iIdMaDonVi = Request.Form["iID_MaDonVi"];
            string iIdMaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            string iTuThang = Request.Form[ParentID + "_iTuThang"];
            string iDenThang = Request.Form[ParentID + "_iDenThang"];
            bool bBq = !string.IsNullOrEmpty(Request.Form["chkBinhQuan"]) && Request.Form["chkBinhQuan"].Equals("on");
            ViewData["iID_MaDonVi"] = iIdMaDonVi;
            ViewData["iID_MaPhongBan"] = iIdMaPhongBan;
            ViewData["iTuThang"] = iTuThang;
            ViewData["iDenThang"] = iDenThang;
            ViewData["bBq"] = bBq;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_Thang.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult ViewPDF(string MaND, string iID_MaPhongBan, bool bBq, string iTuThang, string iDenThang,
                                    string iID_MaDonVi)
        {
            HamChung.Language();
            string path = string.Empty;
            if (bBq)
            {
                path = Server.MapPath(sFilePathBinhQuan);
            }
            else
            {
                path = Server.MapPath(sFilePath);
            }
            ExcelFile xls = CreateReport(path, MaND, iID_MaPhongBan, bBq, iTuThang, iDenThang, iID_MaDonVi);
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

        private ExcelFile CreateReport(string path, string MaND, string iID_MaPhongBan, bool bBq, string iTuThang,
                                       string iDenThang, string iID_MaDonVi)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            string iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            if (string.IsNullOrEmpty(iID_MaPhongBan))
            {
                iID_MaPhongBan = "-1";
            }
            String sTenPhuLuc = "";
            if (bBq)
            {
                if (iID_MaPhongBan == "-1")
                    sTenPhuLuc = "PL04a";
                else
                    sTenPhuLuc = "PL04b";
            }
            else
            {
                sTenPhuLuc = "PL04c";
            }
            String sTenPB = LayTenPhongBan(iID_MaPhongBan);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaPhongBan, bBq, iTuThang, iDenThang, iID_MaDonVi);

            fr = ReportModels.LayThongTinChuKy(fr, "rptQTQS_TongHop");
            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("sTenPhuLuc", sTenPhuLuc);
            fr.SetValue("iTuThang", string.IsNullOrEmpty(iTuThang) ? "0" : iTuThang);
            fr.SetValue("iDenThang", string.IsNullOrEmpty(iDenThang) ? "0" : iDenThang);
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("sTenDV", LayTenDonVi(iID_MaDonVi));
            fr.Run(Result);
            return Result;
        }

        private string LayTenPhongBan(string iID_MaPhongBan)
        {
            String sTenPB = string.Empty;
            String SQL = String.Format(@"SELECT sTen FROM NS_PhongBan WHERE sKyHieu=@sKyHieu AND iTrangThai=1");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sKyHieu", iID_MaPhongBan);
            sTenPB = Connection.GetValueString(cmd, "");
            return sTenPB;
        }

        private string LayTenDonVi(string iID_MaDonVi)
        {
            String sTen = string.Empty;
            String SQL =
                String.Format(@"SELECT  DISTINCT sTen FROM NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi AND iTrangThai=1");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            sTen = Connection.GetValueString(cmd, "");
            return sTen;
        }

        private void LoadData(FlexCelReport fr, string MaND, string iID_MaPhongBan, bool bBq, string iTuThang,
                              string iDenThang, string iID_MaDonVi)
        {
            DataTable data = DT_rptQTQS_Thang(MaND, iID_MaPhongBan, bBq, iTuThang, iDenThang, iID_MaDonVi);
            data.TableName = "TongHop";
            fr.AddTable("TongHop", data);
            data.Dispose();
        }

        private DataTable DT_rptQTQS_Thang(string MaND, string iID_MaPhongBan, bool bBq, string iTuThang,
                                           string iDenThang, string iID_MaDonVi)
        {
            String DKDonVi = string.Empty;
            String DKPhongBan = string.Empty;
            String DK = string.Empty;
            String SelectDV = string.Empty;
            String SelectFROM = string.Empty;
            SqlCommand cmd = new SqlCommand();
            string sql = string.Empty;
            iTuThang = string.IsNullOrEmpty(iTuThang) ? "0" : iTuThang;
            iDenThang = string.IsNullOrEmpty(iDenThang) ? "0" : iDenThang;
            if (string.IsNullOrEmpty(iID_MaPhongBan))
            {
                iID_MaPhongBan = "-1";
            }
            if (string.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = "-1";
            }

            Dictionary<string, DataTable> data = new Dictionary<string, DataTable>();

            DataTable dt = new DataTable();
            int tg = 0;
            if (bBq)
            {

                for (int i = Convert.ToInt32(iTuThang); i <= Convert.ToInt32(iDenThang); i++)
                {
                    cmd = new SqlCommand();
                    dt = new DataTable();
                    DKDonVi = ThuNopModels.DKDonVi(MaND, cmd, "A");
                    DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd, "A");
                    sql = string.Format(@"
select
                rThieuUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuUy ELSE 0 END)
                ),
                rTrungUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungUy ELSE 0 END)
                )
                ,
                rThuongUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END)
                )
                ,
                rDaiUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rDaiUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiUy ELSE 0 END)
                )
                ,
                rThieuTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuTa ELSE 0 END)
                )
                ,
                rTrungTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungTa ELSE 0 END)
                )
                ,
                rThuongTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongTa ELSE 0 END)
                )
                ,
                rDaiTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rDaiTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiTa ELSE 0 END)
                )
                ,
                rTuong=(
                SUM(CASE WHEN sKyHieu='2' THEN rTuong ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTuong ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTuong ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTuong ELSE 0 END)
                )
                ,
                rTSQ=(
                SUM(CASE WHEN sKyHieu='2' THEN rTSQ ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTSQ ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTSQ ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTSQ ELSE 0 END)
                )
                ,
                rBinhNhi=(
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhi ELSE 0 END)
                )
                ,
                rBinhNhat=(
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END)
                )
                ,
                rHaSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rHaSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rHaSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rHaSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rHaSi ELSE 0 END)
                )
                ,
                rTrungSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungSi ELSE 0 END)
                )
                ,
                rThuongSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongSi ELSE 0 END)
                )
                ,
                rQNCN=(
                SUM(CASE WHEN sKyHieu='2' THEN rQNCN ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNCN ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNCN ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNCN ELSE 0 END)
                )
                ,
                rCNVQPCT=(
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQPCT ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQPCT ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQPCT ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQPCT ELSE 0 END)
                )
                ,
                rQNVQPHD=(
                SUM(CASE WHEN sKyHieu='2' THEN rQNVQPHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNVQPHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNVQPHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNVQPHD ELSE 0 END)
                )
                ,
                rCNVQP=(
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQP ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQP ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQP ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQP ELSE 0 END)
                )
                ,
                rLDHD=(
                SUM(CASE WHEN sKyHieu='2' THEN rLDHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rLDHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rLDHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rLDHD ELSE 0 END)
                )
");

                    if (!iID_MaPhongBan.Equals("-1"))
                    {
                        DK = @" AND A.iID_MaPhongBan=@MaPhong ";
                    }
                    sql += string.Format(@" ,A.iID_MaDonVi,B.sTen
                from QTQS_CHungTuChiTiet A
                LEFT JOIN NS_DonVi B on A.iID_MaDonVi =B.iID_MaDonVi
                where A.iTrangThai=1 and A.iNamLamViec=@iNamLamViec AND B.iNamLamViec_DonVi=@iNamLamViec
                and iThang_Quy <=@Thang2 {0} {1} {2}
                group by A.iID_MaDonVi,B.sTen HAVING  
(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuUy ELSE 0 END)
                )<> 0 OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rTrungUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungUy ELSE 0 END)
                )<> 0 OR 
                
                (
                SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END)
                )<> 0 OR 
                
                (
                SUM(CASE WHEN sKyHieu='2' THEN rDaiUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiUy ELSE 0 END)
                )<> 0 OR 
                
               (
                SUM(CASE WHEN sKyHieu='2' THEN rThieuTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuTa ELSE 0 END)
                )<> 0 OR 
                
                (
                SUM(CASE WHEN sKyHieu='2' THEN rTrungTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungTa ELSE 0 END)
                )
                <> 0 OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rThuongTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongTa ELSE 0 END)
                )
                <> 0 OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rDaiTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiTa ELSE 0 END)
                )<> 0 OR 
                
               (
                SUM(CASE WHEN sKyHieu='2' THEN rTuong ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTuong ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTuong ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTuong ELSE 0 END)
                )<> 0 OR 
                
                (
                SUM(CASE WHEN sKyHieu='2' THEN rTSQ ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTSQ ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTSQ ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTSQ ELSE 0 END)
                )<> 0 OR 
                
               (
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhi ELSE 0 END)
                )<> 0 OR 
                
                (
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END)
                )<> 0 OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rHaSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rHaSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rHaSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rHaSi ELSE 0 END)
                
                )<> 0 OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rTrungSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungSi ELSE 0 END)
                )<> 0  OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rThuongSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongSi ELSE 0 END)
                )<> 0 OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rQNCN ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNCN ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNCN ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNCN ELSE 0 END)
                )<> 0  OR 
               (
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQPCT ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQPCT ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQPCT ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQPCT ELSE 0 END)
                )<> 0  OR 
                (
                SUM(CASE WHEN sKyHieu='2' THEN rQNVQPHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNVQPHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNVQPHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNVQPHD ELSE 0 END)
                )
               <> 0  OR  (
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQP ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQP ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQP ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQP ELSE 0 END)
                )
                <> 0  OR  (
                SUM(CASE WHEN sKyHieu='2' THEN rLDHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rLDHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rLDHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rLDHD ELSE 0 END)
                ) <> 0 ORDER BY A.iID_MaDonVi
                ", DK, DKDonVi, DKPhongBan);
                    cmd.CommandText = sql;
                    if (!iID_MaPhongBan.Equals("-1"))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", iID_MaPhongBan);
                    }
                    cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                    //cmd.Parameters.AddWithValue("@Thang1", iTuThang);
                    cmd.Parameters.AddWithValue("@Thang2", i);
                    cmd.Parameters.AddWithValue("@MaDV", iID_MaDonVi);
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        data.Add("dt" + tg, dt);
                        tg++;
                    }
                }
                int tg1 = data.Count - 1;
                if (data.Count > 0)
                {
                    dt = data["dt" + tg1];
                }



                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    decimal iBinhQuan = 0;
                    for (int c = 0; c < dt.Rows.Count; c++)
                    {
                        iBinhQuan = 0;
                        if (dt.Columns[j].ColumnName.StartsWith("r"))
                        {
                            for (int i = 0; i < data.Count; i++)
                            {
                                iBinhQuan += Convert.ToInt32(data["dt" + i].Rows[c][j]);
                            }
                            iBinhQuan = Math.Round(iBinhQuan / data.Count,0);
                            dt.Rows[c][j] = iBinhQuan;
                        }
                
                    }
                }

            }
            //khong ckeck binh quan
            else
            {
                for (int i = Convert.ToInt32(iTuThang); i <= Convert.ToInt32(iDenThang); i++)
                {
                    cmd = new SqlCommand();
                    dt = new DataTable();
                    DKDonVi = ThuNopModels.DKDonVi(MaND, cmd, "A");
                    DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd, "A");
                    sql = string.Format(@"
select
                rThieuUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuUy ELSE 0 END)
                ),
                rTrungUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungUy ELSE 0 END)
                )
                ,
                rThuongUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END)
                )
                ,
                rDaiUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rDaiUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiUy ELSE 0 END)
                )
                ,
                rThieuTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuTa ELSE 0 END)
                )
                ,
                rTrungTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungTa ELSE 0 END)
                )
                ,
                rThuongTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongTa ELSE 0 END)
                )
                ,
                rDaiTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rDaiTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiTa ELSE 0 END)
                )
                ,
                rTuong=(
                SUM(CASE WHEN sKyHieu='2' THEN rTuong ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTuong ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTuong ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTuong ELSE 0 END)
                )
                ,
                rTSQ=(
                SUM(CASE WHEN sKyHieu='2' THEN rTSQ ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTSQ ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTSQ ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTSQ ELSE 0 END)
                )
                ,
                rBinhNhi=(
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhi ELSE 0 END)
                )
                ,
                rBinhNhat=(
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END)
                )
                ,
                rHaSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rHaSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rHaSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rHaSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rHaSi ELSE 0 END)
                )
                ,
                rTrungSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungSi ELSE 0 END)
                )
                ,
                rThuongSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongSi ELSE 0 END)
                )
                ,
                rQNCN=(
                SUM(CASE WHEN sKyHieu='2' THEN rQNCN ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNCN ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNCN ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNCN ELSE 0 END)
                )
                ,
                rCNVQPCT=(
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQPCT ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQPCT ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQPCT ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQPCT ELSE 0 END)
                )
                ,
                rQNVQPHD=(
                SUM(CASE WHEN sKyHieu='2' THEN rQNVQPHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNVQPHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNVQPHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNVQPHD ELSE 0 END)
                )
                ,
                rCNVQP=(
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQP ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQP ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQP ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQP ELSE 0 END)
                )
                ,
                rLDHD=(
                SUM(CASE WHEN sKyHieu='2' THEN rLDHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rLDHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rLDHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rLDHD ELSE 0 END)
                )
");

                    if (!iID_MaPhongBan.Equals("-1"))
                    {
                        DK = @" AND iID_MaPhongBan=@MaPhong ";
                    }
                    sql += string.Format(@"
                ,iID_MaDonVi,iThang_Quy={1}
                from QTQS_CHungTuChiTiet 
                where iTrangThai=1 and iNamLamViec=@iNamLamViec and iID_MaDonVi=@MaDV  {0}
                and iThang_Quy <=@Thang
                group by iID_MaDonVi
                ", DK, i);
                    cmd.CommandText = sql;
                    if (!iID_MaPhongBan.Equals("-1"))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", iID_MaPhongBan);
                    }
                    cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                    cmd.Parameters.AddWithValue("@Thang", i);
                    cmd.Parameters.AddWithValue("@MaDV", iID_MaDonVi);
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        data.Add("dt" + tg, dt);
                        tg++;
                    }
                }
                if (data.Count > 0)
                {
                    dt = data["dt" + 0];
                }

                for (int i = 1; i < data.Count; i++)
                {
                    DataRow dr = data["dt" + i].Rows[0];
                    DataRow dr1 = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dr1[j] = dr[j];
                    }
                    dt.Rows.Add(dr1);
                }
            }
            dt.Dispose();
            return dt;
        }
    }
}
