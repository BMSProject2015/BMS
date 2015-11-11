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
using System.Text;
namespace VIETTEL.Report_Controllers.KeToan
{
    public class rptKeToan_QuyetToanNam_DonViController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_QuyetToanNam_DonVi.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_QuyetToanNam_DonVi.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec"]);
            String ThangLamViec = Convert.ToString(Request.Form[ParentID + "_ThangLamViec"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_QuyetToanNam_DonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaDonVi = iID_MaDonVi, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String iID_MaDonVi, String TrangThai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_QuyetToanNam_DonVi");
            LoadData(fr, NamLamViec, ThangLamViec, iID_MaDonVi, TrangThai);
            String sTenDonVi = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam ="Tháng "+ ThangLamViec+"/năm " + NamLamViec;

            String NamTruoc = "Số năm " + (Convert.ToInt16(NamLamViec) - 1) + " chuyển sang";
            String NamNay = "Số trong năm " + NamLamViec;
            String NamSau = "Số trong năm " + (Convert.ToInt16(NamLamViec) + 1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangNam", ThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NamTruoc", NamTruoc);
            fr.SetValue("NamNay", NamNay);
            fr.SetValue("NamSau", NamSau);
            fr.SetValue("DonVi", sTenDonVi);
            fr.SetValue("Ngay", DateTime.Now.Day);
            fr.SetValue("Thang", DateTime.Now.Month);
            fr.SetValue("Nam", DateTime.Now.Year);
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String iID_MaDonVi, String TrangThai)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaDonVi, TrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_QuyetToanNam_DonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String iID_MaDonVi, String TrangThai)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaDonVi, TrangThai);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String ThangLamViec, String iID_MaDonVi, String TrangThai)
        {

            DataTable data = KeToan_QuyetToanNam(NamLamViec, ThangLamViec, iID_MaDonVi, TrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

        }
        public DataTable KeToan_QuyetToanNam(String NamLamViec, String ThangLamViec, String iID_MaDonVi, String TrangThai)
        {
            String SQL = "";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            String DK = "", DK_No_NamSau = "", DK_Co_NamSau = "", DK_No_NamNay = "", DK_Co_NamNay = "", DK_No_DuDau = "", DK_Co_DuDau = "", DK_DV_No = "", DK_DV_Co = "";
            //Lay ds tai khoan theo tham so KH 203
            SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND sKyHieu=203 AND  iNamLamViec='" + NamLamViec + "'";
            String sThamSo = Connection.GetValueString(SQL, "-1111");
            if (String.IsNullOrEmpty(sThamSo)) sThamSo = "-1";
            String[] arrTK = sThamSo.Split(',');
            for (int i = 0; i < arrTK.Length; i++)
            {
                DK_No_NamSau += "iID_MaTaiKhoan_No='" + arrTK[i] + "'";
                DK_No_NamNay += "iID_MaTaiKhoan_No='" + arrTK[i].Substring(0, 3) + "2" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                DK_No_DuDau += "iID_MaTaiKhoan_No='" + arrTK[i].Substring(0, 3) + "3" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                DK_Co_NamSau += "iID_MaTaiKhoan_Co='" + arrTK[i] + "'";
                DK_Co_NamNay += "iID_MaTaiKhoan_Co='" + arrTK[i].Substring(0, 3) + "2" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                DK_Co_DuDau += "iID_MaTaiKhoan_Co='" + arrTK[i].Substring(0, 3) + "3" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                if (i < arrTK.Length - 1)
                {
                    DK_No_NamSau += " OR ";
                    DK_No_NamNay += " OR ";
                    DK_No_DuDau += " OR ";
                    DK_Co_NamSau += " OR ";
                    DK_Co_NamNay += " OR ";
                    DK_Co_DuDau += " OR ";
                }
            }
            //Neu chon tat ca
            if (iID_MaDonVi != "-1")
            {
                DK_DV_No += " AND iID_MaDonVi_No=@iID_MaDonVi";
                DK_DV_Co += " AND iID_MaDonVi_Co=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (TrangThai != "0")
            {
                DK += " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            SQL = String.Format(@"SELECT iID_MaTaiKhoan,
SUM(DuDau_No) as DuDau_No,
SUM(NamNay_No) as NamNay_No,
SUM(NamSau_No) as NamSau_No,
SUM(DuDau_Co) as DuDau_Co,
SUM(NamNay_Co) as NamNay_Co,
SUM(NamSau_Co) as NamSau_Co
FROM (

SELECT SUBSTRING(iID_MaTaiKhoan_No,1,3) as iID_MaTaiKhoan,
DuDau_No=SUM(CASE WHEN (iNamLamViec=@iNamTruoc1 AND iThangCT<>0 AND ({1})) THEN rSoTien ELSE 0 END),
NamNay_No=SUM(CASE WHEN (iNamLamViec=@iNamTruoc AND iThangCT<>0 AND ({2})) THEN rSoTien ELSE 0 END),
NamSau_No=SUM(CASE WHEN (iNamLamViec=@iNamLamViec AND iThangCT<>0 AND ({3}))THEN rSoTien ELSE 0 END),
DuDau_Co=0,
NamNay_Co=0,
NamSau_Co=0
FROM KT_ChungTuChiTiet
WHERE  iID_MaTaiKhoan_No<>'' AND  iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan_No,1,1) <>'0' {0} {7}
GROUP BY SUBSTRING(iID_MaTaiKhoan_No,1,3)
HAVING SUM(CASE WHEN (iNamLamViec=@iNamTruoc1 AND iThangCT<>0 AND ({1})) THEN rSoTien ELSE 0 END)<>0
OR SUM(CASE WHEN (iNamLamViec=@iNamTruoc AND iThangCT<>0 AND ({2})) THEN rSoTien ELSE 0 END)<>0
OR SUM(CASE WHEN (iNamLamViec=@iNamLamViec AND iThangCT<>0 AND iThangCT<=@iThangLamViec AND ({3})) THEN rSoTien ELSE 0 END)<>0

UNION

SELECT SUBSTRING(iID_MaTaiKhoan_Co,1,3) as iID_MaTaiKhoan,
DuDau_No=0,
NamNay_No=0,
NamSau_No=0,
DuDau_Co=SUM(CASE WHEN (iNamLamViec=@iNamTruoc1 AND iThangCT<>0 AND ({4})) THEN rSoTien ELSE 0 END),
NamNay_Co=SUM(CASE WHEN (iNamLamViec=@iNamTruoc AND iThangCT<>0 AND ({5})) THEN rSoTien ELSE 0 END),
NamSau_Co=SUM(CASE WHEN (iNamLamViec=@iNamLamViec AND iThangCT<>0   AND iThangCT<=@iThangLamViec AND ({6}))THEN rSoTien ELSE 0 END)
FROM KT_ChungTuChiTiet
WHERE  iID_MaTaiKhoan_Co<>'' AND  iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan_Co,1,1) <>'0' {0} {8}
GROUP BY SUBSTRING(iID_MaTaiKhoan_Co,1,3)
HAVING SUM(CASE WHEN (iNamLamViec=@iNamTruoc1 AND iThangCT<>0 AND ({4})) THEN rSoTien ELSE 0 END)<>0
OR SUM(CASE WHEN (iNamLamViec=@iNamTruoc AND iThangCT<>0 AND ({5})) THEN rSoTien ELSE 0 END)<>0
OR SUM(CASE WHEN (iNamLamViec=@iNamLamViec AND iThangCT<>0 AND ({6})) THEN rSoTien ELSE 0 END)<>0) a
GROUP BY iID_MaTaiKhoan
", DK, DK_No_DuDau, DK_No_NamNay, DK_No_NamSau, DK_Co_DuDau, DK_Co_NamNay, DK_No_NamSau,DK_DV_No,DK_DV_Co);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamTruoc1", Convert.ToInt16(NamLamViec) - 2);
            cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(NamLamViec) - 1);
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt16(NamLamViec));
            cmd.Parameters.AddWithValue("@iThangLamViec", Convert.ToInt16(ThangLamViec));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        public DataTable DanhSachDonVi(String NamLamViec, String ThangLamViec, String TrangThai)
        {
            String SQL = "";
            String DK = "", DK_No_NamSau = "", DK_Co_NamSau = "", DK_No_NamNay = "", DK_Co_NamNay = "", DK_No_DuDau = "", DK_Co_DuDau = "";
            SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND sKyHieu=203 AND iNamLamViec='" + NamLamViec + "'";
            String sThamSo = Connection.GetValueString(SQL, "-1111");
            if (String.IsNullOrEmpty(sThamSo)) sThamSo = "-1";
            String[] arrTK = sThamSo.Split(',');
            for (int i = 0; i < arrTK.Length; i++)
            {
                DK_No_NamSau += "iID_MaTaiKhoan_No='" + arrTK[i] + "'";
                DK_No_NamNay += "iID_MaTaiKhoan_No='" + arrTK[i].Substring(0, 3) + "2" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                DK_No_DuDau += "iID_MaTaiKhoan_No='" + arrTK[i].Substring(0, 3) + "3" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                DK_Co_NamSau += "iID_MaTaiKhoan_Co='" + arrTK[i] + "'";
                DK_Co_NamNay += "iID_MaTaiKhoan_Co='" + arrTK[i].Substring(0, 3) + "2" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                DK_Co_DuDau += "iID_MaTaiKhoan_Co='" + arrTK[i].Substring(0, 3) + "3" + arrTK[i].Substring(4, arrTK[i].Length - 4) + "'";
                if (i < arrTK.Length - 1)
                {
                    DK_No_NamSau += " OR ";
                    DK_No_NamNay += " OR ";
                    DK_No_DuDau += " OR ";
                    DK_Co_NamSau += " OR ";
                    DK_Co_NamNay += " OR ";
                    DK_Co_DuDau += " OR ";
                }
            }
            DK_No_DuDau = DK_No_DuDau + " AND iNamLamViec=@iNamTruoc1 AND iThangCT<>0";
            DK_No_NamNay = DK_No_NamNay + " AND iNamLamViec=@iNamTruoc AND iThangCT<>0";
            DK_No_NamSau = DK_No_NamSau + " AND iNamLamViec=@iNamLamViec AND iThangCT<>0 AND iThangCT<=@iThangLamViec";

            DK_Co_DuDau = DK_Co_DuDau + " AND iNamLamViec=@iNamTruoc1";
            DK_Co_NamNay = DK_Co_NamNay + " AND iNamLamViec=@iNamTruoc";
            DK_Co_NamSau = DK_Co_NamSau + " AND iNamLamViec=@iNamLamViec AND iThangCT<>0 AND iThangCT<=@iThangLamViec";

            if (TrangThai != "0")
            {
                DK = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi_No,sTenDonVi_No
                                FROM KT_ChungTuChiTiet
                                WHERE (({1}) OR ({2}) OR ({3}) ) AND iTrangThai=1 AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND  iID_MaTaiKhoan_No<>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,1) <>'0' {0}

                                UNION

                                SELECT DISTINCT iID_MaDonVi_Co,sTenDonVi_Co
                                FROM KT_ChungTuChiTiet
                                WHERE (({4}) OR ({5}) OR ({6}) ) AND iTrangThai=1 AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND  iID_MaTaiKhoan_Co<>'' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1) <>'0' {0}", DK, DK_No_DuDau,DK_No_NamNay,DK_No_NamSau,DK_Co_DuDau,DK_Co_NamNay,DK_Co_NamSau);

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamTruoc1", Convert.ToInt16(NamLamViec) - 2);
            cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(NamLamViec) - 1);
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt16(NamLamViec));
            cmd.Parameters.AddWithValue("@iThangLamViec", Convert.ToInt16(ThangLamViec));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDonVi_No"] = "-1";
                dr["sTenDonVi_No"] = "--Chọn tất cả--";
                dt.Rows.InsertAt(dr,0);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDonVi_No"] = "-2";
                dr["sTenDonVi_No"] = "--Không có đơn vị--";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }
        public JsonResult ds_DotPhanBo(String ParentID ,String NamLamViec, String ThangLamViec, String TrangThai,String iID_MaDonVi)
        {

            return Json(obj_DotPhanBo(ParentID,NamLamViec, ThangLamViec, TrangThai, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        public String  obj_DotPhanBo(String ParentID,String NamLamViec, String ThangLamViec, String TrangThai,String iID_MaDonVi)
        {
           
            DataTable dtDonVi = DanhSachDonVi(NamLamViec,ThangLamViec,TrangThai);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi_No", "sTenDonVi_No");
            String s = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return s;
        }
    }
}
