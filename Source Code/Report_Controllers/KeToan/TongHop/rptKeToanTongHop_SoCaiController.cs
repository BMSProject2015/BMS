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
    public class rptKeToanTongHop_SoCaiController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3_Cuoi = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_Cuoi.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_1.xls";
        private const String sFilePath_A3_Le = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_Le.xls";
        private const String sFilePath_A3_Chan = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_Chan.xls";
        private const String sFilePath_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A4.xls";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A4_1.xls";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_SoCai.aspx";
                ViewData["PageLoad"] = "0";
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
            String iID_MaPhuongAn = Convert.ToString(Request.Form[ParentID + "_iID_MaPhuongAn"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + ""]);
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["iID_MaPhuongAn"] = iID_MaPhuongAn;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["ToSo"] = ToSo;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_SoCai.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            DataTable dtTo = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            String ToSo = "";

            for (int i = 0; i < dtTo.Rows.Count; i++)
            {
                if (iID_MaTaiKhoan == dtTo.Rows[i]["MaTo"].ToString())
                {
                    ToSo = dtTo.Rows[i]["ToSo"].ToString();
                    break;
                }
            }
            if (KhoGiay == "2" && ToSo != dtTo.Rows[dtTo.Rows.Count - 1]["ToSo"].ToString())
            {
                fr = ReportModels.LayThongTinChuKy(fr, "");
            }
            else
            {
                fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_SoCai");
            }

            LoadData(fr, NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam = "Tháng " + ThangLamViec + " năm " + NamLamViec;

            String TenTK = "";
            String[] arrMaTK = new String[6];
            String[] arrTenTK = new String[6];


            String[] arrMaTaiKhoan = iID_MaTaiKhoan.Split(',');
            for (int i = 0; i < arrMaTaiKhoan.Length; i++)
            {
                arrMaTK[i] = arrMaTaiKhoan[i];
                arrTenTK[i] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTaiKhoan[i], "sTen"));
            }
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String SoDuDauKy = "Số dư tháng trước chuyển sang:";
            if (ThangLamViec == "1")
            {
                SoDuDauKy = "Số dư năm trước chuyển sang:";
            }
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangNam", ThangNam);
            fr.SetValue("MaTK", iID_MaTaiKhoan);
            fr.SetValue("TenTK", TenTK);
            fr.SetValue("MaTK0", arrMaTK[0]);
            fr.SetValue("MaTK1", arrMaTK[1]);
            fr.SetValue("MaTK2", arrMaTK[2]);
            fr.SetValue("MaTK3", arrMaTK[3]);
            fr.SetValue("MaTK4", arrMaTK[4]);
            fr.SetValue("MaTK5", arrMaTK[5]);
            fr.SetValue("TenTK0", arrTenTK[0]);
            fr.SetValue("TenTK1", arrTenTK[1]);
            fr.SetValue("TenTK2", arrTenTK[2]);
            fr.SetValue("TenTK3", arrTenTK[3]);
            fr.SetValue("TenTK4", arrTenTK[4]);
            fr.SetValue("TenTK5", arrTenTK[5]);
            fr.SetValue("ToSo", "Tờ: " + ToSo);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("SoDuDauKy", SoDuDauKy);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Xuất ra Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            HamChung.Language();

            DataTable dt = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            int ToCuoi = dt.Rows.Count;
            dt.Dispose();
            String DuongDanFile = "";
            String ToSo = "";
            int To = Convert.ToInt16(ToSo);
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    DuongDanFile = sFilePath_A3_1;
                }
                else if (To % 2 == 1 && To != ToCuoi)
                {
                    DuongDanFile = sFilePath_A3_Le;
                }
                else if (To % 2 == 0 && To != ToCuoi)
                {
                    DuongDanFile = sFilePath_A3_Chan;
                }
                else
                {
                    DuongDanFile = sFilePath_A3_Cuoi;
                }
            }
            else
            {
                if (iID_MaTaiKhoan == "-1")
                {
                    DuongDanFile = sFilePath_A4_1;
                }
                else
                {
                    DuongDanFile = sFilePath_A4;
                }

            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_SoCai.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            HamChung.Language();
            DataTable dt = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            int ToCuoi = dt.Rows.Count;
            String ToSo = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (iID_MaTaiKhoan == dt.Rows[i]["MaTo"].ToString())
                {
                    ToSo = dt.Rows[i]["ToSo"].ToString();
                    break;
                }
            }
            dt.Dispose();
            String DuongDanFile = "";
            int To = Convert.ToInt16(ToSo);
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    DuongDanFile = sFilePath_A3_1;
                }
                else if (To % 2 == 1 && To != ToCuoi)
                {
                    DuongDanFile = sFilePath_A3_Le;
                }
                else if (To % 2 == 0 && To != ToCuoi)
                {
                    DuongDanFile = sFilePath_A3_Chan;
                }
                else
                {
                    DuongDanFile = sFilePath_A3_Cuoi;
                }
            }
            else
            {
                if (iID_MaTaiKhoan == "-1")
                {
                    DuongDanFile = sFilePath_A4_1;
                }
                else
                {
                    DuongDanFile = sFilePath_A4;
                }

            }

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);
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
        /// lấy các range đổ vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            DataTable data = KeToan_SoCai(NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay, true);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable CuoiTrang = HamChung.SelectDistinct("CuoiTrang", data, "Trang", "Trang", "", "", "");
            fr.AddTable("CuoiTrang", CuoiTrang);
            CuoiTrang.Dispose();
            DataTable LuyKe = KeToan_SoCai_LuyKe(iID_MaTaiKhoan, NamLamViec, ThangLamViec);
            data.TableName = "LuyKe";
            fr.AddTable("LuyKe", LuyKe);
            DataTable LuyKe0 = KeToan_SoCai_LuyKe0(iID_MaTaiKhoan, NamLamViec, ThangLamViec);
            fr.AddTable("LuyKe0", LuyKe0);
        }
        /// <summary>
        /// onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public String obj_data(String ParentID, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            DataTable dt = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            SelectOptionList slTaiKhoan = new SelectOptionList(dt, "MaTo", "TenTo");
            String s = MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 200px\"");
            return s;
        }
        [HttpGet]
        public JsonResult ds_NhomDonVi(String ParentID, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            return Json(obj_data(ParentID, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// KeToan_SoCai
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <param name="HienThi"></param>
        /// <returns></returns>
        public static DataTable KeToan_SoCai(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay, bool HienThi)
        {
            ///set các DK
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            String DKSoTien = "";
            const String LengthTaiKhoanCap1 = "3";
            const String LengthTaiKhoanCap2 = "4";
            const String LengthTaiKhoanCap3 = "5";
            const String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "3";
            for (int i = 0; i < arrPhuongAn.Length; i++)
            {
                if (arrPhuongAn[i].Length == 3)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap1;
                }
                if (arrPhuongAn[i].Length == 4)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap2;
                }
                if (arrPhuongAn[i].Length == 5)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap3;
                }
                if (arrPhuongAn[i].Length == 6)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap4;
                }
                DKSoTien += "SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " OR SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i;
                if (i < arrPhuongAn.Length - 1)
                    DKSoTien += " OR ";
            }
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                if (arrTaiKhoan[i].Length == 3)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap1;
                }
                if (arrTaiKhoan[i].Length == 4)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap2;
                }
                if (arrTaiKhoan[i].Length == 5)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap3;
                }
                if (arrTaiKhoan[i].Length == 6)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap4;
                }
                DK_DonViSelect += ",SUM(TaiKhoan" + i + "_No) AS TaiKhoan" + i + "_No, SUM(TaiKhoan" + i + "_Co) AS TaiKhoan" + i + "_Co";
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan" + i + "_No)<>0 OR SUM(TaiKhoan" + i + "_Co)<>0";
            }
            DK_DonViSelect = DK_DonViSelect.Substring(1);
            DK_DonViCASE = DK_DonViCASE.Substring(1);
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
            #region//tạo dt
            String SQL = String.Format(@"SELECT  STT=1,b.iiD_MaChungTu,SoGS,NgayGS,CONVERT(INT,LTRIM(RTRIM(SoGS))) as iSoGS,
                                        SOCT=(SELECT TOP 1 sSoChungTuChiTiet FROM KT_ChungTuChiTiet WHERE iID_MaChungTu=B.iID_MaChungTu)
                                        ,NgayCT=(SELECT TOP 1 CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as  NgayCT FROM KT_ChungTuChiTiet WHERE iID_MaChungTu=B.iID_MaChungTu)
                                        ,iNgay=(SELECT TOP 1 iNgay  FROM KT_ChungTuChiTiet WHERE iID_MaChungTu=B.iID_MaChungTu)                                        
,DonVi_No=(SELECT TOP 1 iID_MaDonVi_No FROM KT_ChungTuChiTiet WHERE iID_MaChungTu=B.iID_MaChungTu)
                                        ,DonVi_Co=(SELECT TOP 1 iID_MaDonVi_Co FROM KT_ChungTuChiTiet WHERE iID_MaChungTu=B.iID_MaChungTu)
                                        ,b.sNoiDung
                                        ,SUM(SoTien) as SoPhatSinh, {0}
                                        FROM(
                                        SELECT  iiD_MaChungTu, sSoChungTuChiTiet,iNgayCT,iThangCT,iID_MaDonVi_No,iID_MaDonVi_Co,sNoiDung,
                                        SoTien=CASE WHEN ({3}) THEN SUM(rSoTien) ELSE 0 END,
                                        {1}
                                        FROM KT_ChungTuChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND iThangCT=@ThangLamViec AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY iiD_MaChungTu,iNgayCT,iThangCT,iID_MaDonVi_No,sNoiDung,iID_MaTaiKhoan_No,iID_MaDonVi_Co,iID_MaTaiKhoan_Co,sSoChungTuChiTiet
                                        ) AS A
                                        INNER JOIN (SELECT iiD_MaChungTu,sSoChungTu as SoGS, CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as NgayGS, sNoiDung FROM KT_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND iThang=@ThangLamViec) AS B
                                        ON a.iID_MaChungTu=b.iID_MaChungTu
                                        GROUP BY b.iID_MaChungTu,NgayGS,SoGS,b.sNoiDung,SoGS   HAVING SUM(SoTien)<>0 OR {2}
                                        ORDER BY iSoGS", DK_DonViSelect, DK_DonViCASE, DK_DonViHaVing, DKSoTien);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);

            for (int i = 0; i < arrPhuongAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrPhuongAn[i]);
            }
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            #endregion
            if (arrTaiKhoan[0] == "-1")
            {
                #region // tạo dtChungTu
                String SQLChungTu = String.Format(@"SELECT DisTinct iID_MaChungTu
                                              FROM KT_ChungTuChiTiet
                                              WHERE iTrangThai=1 AND iThangCT=@ThangLamViec AND iNamLamViec=@NamLamViec AND ({0})  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet", DKSoTien);
                SqlCommand cmdChungTu = new SqlCommand(SQLChungTu);
                for (int i = 0; i < arrPhuongAn.Length; i++)
                {
                    cmdChungTu.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrPhuongAn[i]);
                }
                cmdChungTu.Parameters.AddWithValue("NamLamViec", NamLamViec);
                cmdChungTu.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
                cmdChungTu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                DataTable dtChungTu = Connection.GetDataTable(cmdChungTu);
                cmdChungTu.Dispose();

                #endregion
                #region tao dt doi ung
                String SQLDoiUng = String.Format(@"SELECT DisTinct iID_MaChungTu,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co
                                                FROM KT_ChungTuChiTiet
                                             WHERE  iTrangThai=1 AND iNamLamViec=@NamLamViec AND rSoTien<>0 AND iThangCT=@ThangLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet");
                SqlCommand cmdDoiUng = new SqlCommand(SQLDoiUng);
                cmdDoiUng.Parameters.AddWithValue("NamLamViec", NamLamViec);
                cmdDoiUng.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
                cmdDoiUng.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                DataTable dtDoiUng = Connection.GetDataTable(cmdDoiUng);
                cmdDoiUng.Dispose();

                dtChungTu.Columns.Add("DoiUng_No", typeof(String));
                dtChungTu.Columns.Add("DoiUng_Co", typeof(String));
                String[] arrDoiUngNo = new String[dtChungTu.Rows.Count];
                String[] arrDoiUngCo = new String[dtChungTu.Rows.Count];
                int tg = 0;
                for (int i = 0; i < dtChungTu.Rows.Count; i++)
                {
                    for (int j = tg; j < dtDoiUng.Rows.Count; j++)
                    {
                        if (dtChungTu.Rows[i]["iID_MaChungTu"].ToString() == dtDoiUng.Rows[j]["iID_MaChungTu"].ToString())
                        {
                            if (j == 0)
                            {
                                if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Length >= 3)
                                {
                                    arrDoiUngNo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3);
                                }
                                if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Length >= 3)
                                {
                                    arrDoiUngCo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3);
                                }
                            }
                            else
                            {

                                if (dtDoiUng.Rows[j]["iID_MaChungTu"].ToString() != dtDoiUng.Rows[j - 1]["iID_MaChungTu"].ToString())
                                {
                                    if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Length >= 3)
                                    {
                                        arrDoiUngNo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3);
                                    }

                                    if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Length >= 3)
                                    {
                                        arrDoiUngCo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3);
                                    }
                                }
                                else
                                {
                                    if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Length >= 3)
                                    {
                                        if (dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_No"].ToString().Length < 3)
                                        {
                                            arrDoiUngNo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3);
                                        }
                                        else
                                        {
                                            if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3) != dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3))
                                            {
                                                arrDoiUngNo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3);
                                            }
                                        }
                                    }
                                    if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Length >= 3)
                                    {
                                        if (dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_Co"].ToString().Length < 3)
                                        {
                                            arrDoiUngCo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3);
                                        }
                                        else
                                        {
                                            if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3) != dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3))
                                            {
                                                arrDoiUngCo[i] += "," + dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3);
                                            }
                                        }
                                    }
                                }

                            }
                            tg++;
                        }
                        if (j > tg)
                            break;

                    }
                    if (arrDoiUngNo[i] != null && arrDoiUngCo[i] != null)
                    {
                        arrDoiUngNo[i] = arrDoiUngNo[i].Substring(1);
                        arrDoiUngCo[i] = arrDoiUngCo[i].Substring(1);
                    }
                }
                for (int i = 0; i < dtChungTu.Rows.Count; i++)
                {
                    String S = "iID_MaChungTu='" + dtChungTu.Rows[i]["iID_MaChungTu"].ToString() + "'";
                    DataRow[] r = dtChungTu.Select(S);
                    r[0]["DoiUng_No"] = arrDoiUngNo[i];
                    r[0]["DoiUng_Co"] = arrDoiUngCo[i];
                }
                #endregion
                #region //Ghép DT chung tu vào dt
                DataRow addR, R2;
                String sCol = "iID_MaChungTu,DoiUng_No,DoiUng_Co";
                String[] arrCol = sCol.Split(',');
                DataColumn col = dt.Columns.Add("DoiUng_No", typeof(String));
                col.SetOrdinal(8);
                DataColumn col1 = dt.Columns.Add("DoiUng_Co", typeof(String));
                col1.SetOrdinal(9);
                //ghep cac dong khac cua dtchung tu vao dt
                for (int i = 0; i < dtChungTu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"iID_MaChungTu='{0}'", dtChungTu.Rows[i]["iID_MaChungTu"]
                                                      );
                    DataRow[] R = dt.Select(xauTruyVan);

                    if (R == null || R.Length == 0)
                    {
                        addR = dt.NewRow();
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtChungTu.Rows[i][arrCol[j]];
                        }
                        dt.Rows.Add(addR);
                    }
                }
                foreach (DataRow R1 in dtChungTu.Rows)
                {

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        Boolean okTrung = true;
                        R2 = dt.Rows[j];

                        for (int c = 0; c < arrCol.Length - 2; c++)
                        {
                            if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                            {
                                okTrung = false;
                                break;
                            }
                        }
                        if (okTrung)
                        {
                            dt.Rows[j]["DoiUng_No"] = R1["DoiUng_No"];
                            dt.Rows[j]["DoiUng_Co"] = R1["DoiUng_Co"];
                            break;
                        }

                    }
                }




                //sắp xếp datatable sau khi ghép
                DataView dv = dt.DefaultView;
                dv.Sort = "iSoGS";
                dt = dv.ToTable();
                #endregion
            }
            //Định dạng trang IN
            dt.Columns.Add("Trang", typeof(String));
            dt.Columns.Add("SoDong", typeof(String));
            String ToSo = "";
            DataTable dtTo = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            for (int i = 0; i < dtTo.Rows.Count; i++)
            {
                if (iID_MaTaiKhoan == dtTo.Rows[i]["MaTo"].ToString())
                {
                    ToSo = dtTo.Rows[i]["ToSo"].ToString();
                    break;
                }
            }
            dtTo.Dispose();
            int To = Convert.ToInt32(ToSo);
            //Khổ giấy A3
            if (KhoGiay == "1")
            {
                //Định dang dòng cuối trang
                int SoDong1Trang = 46;
                int SoTrang = 1;

                int Count = dt.Rows.Count;
                SoTrang = Count / SoDong1Trang;
                int SoDu = Count % SoDong1Trang;
                for (int i = 0; i < Count; i++)
                {

                    SoTrang = 1 + i / SoDong1Trang;
                    String S = "iID_MaChungTu='" + dt.Rows[i]["iID_MaChungTu"].ToString() + "'";
                    DataRow[] r = dt.Select(S);
                    r[0]["Trang"] = SoTrang.ToString();
                    r[0]["SoDong"] = i + 1;

                }
                int d = SoDong1Trang;
                int a = 0, b = 0;
                DataRow r1, r2;
                DataTable dtToIn = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);


                //Tờ số 1
                if (To == 1)
                {
                    for (int i = 0; i < SoTrang - 1; i++)
                    {
                        double TongSoPhatSinh = 0;
                        double TongTaiKhoan0_No = 0;
                        double TongTaiKhoan0_Co = 0;
                        double TongTaiKhoan1_No = 0;
                        double TongTaiKhoan1_Co = 0;
                        double TongTaiKhoan2_No = 0;
                        double TongTaiKhoan2_Co = 0;


                        a = SoDong1Trang * i;
                        b = SoDong1Trang * (i + 1);
                        if (i > 0)
                        {
                            a = a + 2 * i;
                            b = b + 2 * i;
                        }
                        for (int j = a; j < b; j++)
                        {
                            if (dt.Rows[j]["SoPhatSinh"].ToString() != "")
                            {
                                TongSoPhatSinh += Convert.ToDouble(dt.Rows[j]["SoPhatSinh"].ToString());
                                TongTaiKhoan0_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_No"].ToString());
                                TongTaiKhoan0_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_Co"].ToString());
                                TongTaiKhoan1_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_No"].ToString());
                                TongTaiKhoan1_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_Co"].ToString());
                                TongTaiKhoan2_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_No"].ToString());
                                TongTaiKhoan2_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_Co"].ToString());
                                ;
                            }
                        }
                        r1 = dt.NewRow();
                        r1["sNoiDung"] = "+ Cuối trang " + (i + 1);
                        r1["SoDong"] = "";
                        r1["Trang"] = i + 1;
                        r1["SoPhatSinh"] = TongSoPhatSinh;
                        r1["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r1["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r1["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r1["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r1["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r1["TaiKhoan2_Co"] = TongTaiKhoan2_Co;


                        dt.Rows.InsertAt(r1, d);
                        r2 = dt.NewRow();
                        r2["sNoiDung"] = "Mang sang";
                        r2["SoDong"] = "";
                        r2["Trang"] = i + 1;
                        r2["SoPhatSinh"] = TongSoPhatSinh;
                        r2["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r2["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r2["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r2["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r2["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r2["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        dt.Rows.InsertAt(r2, d + 1);
                        d = d + SoDong1Trang + 2;
                    }

                }
                //Các tờ khác tờ cuối
                else if (To != dtToIn.Rows.Count)
                {
                    for (int i = 0; i < SoTrang - 1; i++)
                    {
                        double TongSoPhatSinh = 0;
                        double TongTaiKhoan0_No = 0;
                        double TongTaiKhoan0_Co = 0;
                        double TongTaiKhoan1_No = 0;
                        double TongTaiKhoan1_Co = 0;
                        double TongTaiKhoan2_No = 0;
                        double TongTaiKhoan2_Co = 0;
                        double TongTaiKhoan3_No = 0;
                        double TongTaiKhoan3_Co = 0;
                        a = SoDong1Trang * i;
                        b = SoDong1Trang * (i + 1);
                        if (i > 0)
                        {
                            a = a + 2 * i;
                            b = b + 2 * i;
                        }
                        for (int j = a; j < b; j++)
                        {
                            if (dt.Rows[j]["SoPhatSinh"].ToString() != "")
                            {
                                TongSoPhatSinh += Convert.ToDouble(dt.Rows[j]["SoPhatSinh"].ToString());
                                TongTaiKhoan0_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_No"].ToString());
                                TongTaiKhoan0_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_Co"].ToString());
                                TongTaiKhoan1_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_No"].ToString());
                                TongTaiKhoan1_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_Co"].ToString());
                                TongTaiKhoan2_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_No"].ToString());
                                TongTaiKhoan2_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_Co"].ToString());
                                TongTaiKhoan3_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan3_No"].ToString());
                                TongTaiKhoan3_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan3_Co"].ToString());
                            }
                        }
                        r1 = dt.NewRow();
                        r1["sNoiDung"] = "+ Cuối trang " + (i + 1);
                        r1["SoDong"] = "";
                        r1["Trang"] = i + 1;
                        r1["SoPhatSinh"] = TongSoPhatSinh;
                        r1["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r1["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r1["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r1["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r1["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r1["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        r1["TaiKhoan3_No"] = TongTaiKhoan3_No;
                        r1["TaiKhoan3_Co"] = TongTaiKhoan3_Co;


                        dt.Rows.InsertAt(r1, d);
                        r2 = dt.NewRow();
                        r2["sNoiDung"] = "Mang sang";
                        r2["SoDong"] = "";
                        r2["Trang"] = i + 1;
                        r2["SoPhatSinh"] = TongSoPhatSinh;
                        r2["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r2["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r2["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r2["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r2["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r2["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        r2["TaiKhoan3_No"] = TongTaiKhoan3_No;
                        r2["TaiKhoan3_Co"] = TongTaiKhoan3_Co;
                        dt.Rows.InsertAt(r2, d + 1);
                        d = d + SoDong1Trang + 2;
                    }
                }
                // Tờ cuối
                else
                {

                    for (int i = 0; i < SoTrang - 1; i++)
                    {
                        double TongSoPhatSinh = 0;
                        double TongTaiKhoan0_No = 0;
                        double TongTaiKhoan0_Co = 0;
                        double TongTaiKhoan1_No = 0;
                        double TongTaiKhoan1_Co = 0;
                        double TongTaiKhoan2_No = 0;
                        double TongTaiKhoan2_Co = 0;
                        double TongTaiKhoan3_No = 0;
                        double TongTaiKhoan3_Co = 0;
                        double TongTaiKhoan4_No = 0;
                        double TongTaiKhoan4_Co = 0;
                        double TongTaiKhoan5_No = 0;
                        double TongTaiKhoan5_Co = 0;
                        a = SoDong1Trang * i;
                        b = SoDong1Trang * (i + 1);
                        if (i > 0)
                        {
                            a = a + 2 * i;
                            b = b + 2 * i;
                        }
                        for (int j = a; j < b; j++)
                        {
                            if (dt.Rows[j]["SoPhatSinh"].ToString() != "")
                            {
                                TongSoPhatSinh += Convert.ToDouble(dt.Rows[j]["SoPhatSinh"].ToString());
                                TongTaiKhoan0_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_No"].ToString());
                                TongTaiKhoan0_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_Co"].ToString());
                                TongTaiKhoan1_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_No"].ToString());
                                TongTaiKhoan1_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_Co"].ToString());
                                TongTaiKhoan2_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_No"].ToString());
                                TongTaiKhoan2_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_Co"].ToString());
                                TongTaiKhoan3_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan3_No"].ToString());
                                TongTaiKhoan3_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan3_Co"].ToString());
                                TongTaiKhoan4_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan4_No"].ToString());
                                TongTaiKhoan4_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan4_Co"].ToString());
                                TongTaiKhoan5_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan5_No"].ToString());
                                TongTaiKhoan5_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan5_Co"].ToString());
                            }
                        }
                        r1 = dt.NewRow();
                        r1["sNoiDung"] = "+ Cuối trang " + (i + 1);
                        r1["SoDong"] = "";
                        r1["Trang"] = i + 1;
                        r1["SoPhatSinh"] = TongSoPhatSinh;
                        r1["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r1["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r1["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r1["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r1["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r1["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        r1["TaiKhoan3_No"] = TongTaiKhoan3_No;
                        r1["TaiKhoan3_Co"] = TongTaiKhoan3_Co;
                        r1["TaiKhoan4_No"] = TongTaiKhoan4_No;
                        r1["TaiKhoan4_Co"] = TongTaiKhoan4_Co;
                        r1["TaiKhoan5_No"] = TongTaiKhoan5_No;
                        r1["TaiKhoan5_Co"] = TongTaiKhoan5_Co;

                        dt.Rows.InsertAt(r1, d);
                        r2 = dt.NewRow();
                        r2["sNoiDung"] = "Mang sang";
                        r2["SoDong"] = "";
                        r2["Trang"] = i + 1;
                        r2["SoPhatSinh"] = TongSoPhatSinh;
                        r2["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r2["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r2["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r2["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r2["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r2["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        r2["TaiKhoan3_No"] = TongTaiKhoan3_No;
                        r2["TaiKhoan3_Co"] = TongTaiKhoan3_Co;
                        r2["TaiKhoan4_No"] = TongTaiKhoan4_No;
                        r2["TaiKhoan4_Co"] = TongTaiKhoan4_Co;
                        r2["TaiKhoan5_No"] = TongTaiKhoan5_No;
                        r2["TaiKhoan5_Co"] = TongTaiKhoan5_Co;
                        dt.Rows.InsertAt(r2, d + 1);
                        d = d + SoDong1Trang + 2;
                    }
                }

                return dt;
            }
            else if (KhoGiay == "2")
            {

                //Định dang dòng cuối trang
                int SoDong1Trang = 32;
                int SoTrang = 1;

                int Count = dt.Rows.Count;
                SoTrang = Count / SoDong1Trang;
                int SoDu = Count % SoDong1Trang;


                for (int i = 0; i < Count; i++)
                {

                    SoTrang = 1 + i / SoDong1Trang;
                    String S = "iID_MaChungTu='" + dt.Rows[i]["iID_MaChungTu"].ToString() + "'";
                    DataRow[] r = dt.Select(S);
                    r[0]["Trang"] = SoTrang.ToString();
                    r[0]["SoDong"] = i + 1;

                }

                int d = SoDong1Trang;
                int a = 0, b = 0;
                DataRow r1, r2;
                if (iID_MaTaiKhoan != "-1")
                {
                    for (int i = 0; i < SoTrang - 1; i++)
                    {
                        double TongSoPhatSinh = 0;
                        double TongTaiKhoan0_No = 0;
                        double TongTaiKhoan0_Co = 0;
                        double TongTaiKhoan1_No = 0;
                        double TongTaiKhoan1_Co = 0;
                        double TongTaiKhoan2_No = 0;
                        double TongTaiKhoan2_Co = 0;
                        double TongTaiKhoan3_No = 0;
                        double TongTaiKhoan3_Co = 0;

                        a = SoDong1Trang * i;
                        b = SoDong1Trang * (i + 1);
                        if (i > 0)
                        {
                            a = a + 2 * i;
                            b = b + 2 * i;
                        }
                        for (int j = a; j < b; j++)
                        {
                            if (dt.Rows[j]["SoPhatSinh"].ToString() != "")
                            {
                                TongSoPhatSinh += Convert.ToDouble(dt.Rows[j]["SoPhatSinh"].ToString());
                                TongTaiKhoan0_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_No"].ToString());
                                TongTaiKhoan0_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan0_Co"].ToString());
                                TongTaiKhoan1_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_No"].ToString());
                                TongTaiKhoan1_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan1_Co"].ToString());
                                TongTaiKhoan2_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_No"].ToString());
                                TongTaiKhoan2_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan2_Co"].ToString());
                                TongTaiKhoan3_No += Convert.ToDouble(dt.Rows[j]["TaiKhoan3_No"].ToString());
                                TongTaiKhoan3_Co += Convert.ToDouble(dt.Rows[j]["TaiKhoan3_Co"].ToString());
                            }
                        }
                        r1 = dt.NewRow();
                        r1["sNoiDung"] = "+ Cuối trang " + (i + 1);
                        r1["SoDong"] = "+ Cuối trang " + (i + 1);
                        r1["Trang"] = i + 1;
                        r1["SoPhatSinh"] = TongSoPhatSinh;
                        r1["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r1["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r1["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r1["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r1["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r1["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        r1["TaiKhoan3_No"] = TongTaiKhoan3_No;
                        r1["TaiKhoan3_Co"] = TongTaiKhoan3_Co;

                        dt.Rows.InsertAt(r1, d);
                        r2 = dt.NewRow();
                        r2["sNoiDung"] = "Mang sang";
                        r2["SoDong"] = "Mang sang";
                        r2["Trang"] = i + 1;
                        r2["SoPhatSinh"] = TongSoPhatSinh;
                        r2["TaiKhoan0_No"] = TongTaiKhoan0_No;
                        r2["TaiKhoan0_Co"] = TongTaiKhoan0_Co;
                        r2["TaiKhoan1_No"] = TongTaiKhoan1_No;
                        r2["TaiKhoan1_Co"] = TongTaiKhoan1_Co;
                        r2["TaiKhoan2_No"] = TongTaiKhoan2_No;
                        r2["TaiKhoan2_Co"] = TongTaiKhoan2_Co;
                        r2["TaiKhoan3_No"] = TongTaiKhoan3_No;
                        r2["TaiKhoan3_Co"] = TongTaiKhoan3_Co;
                        dt.Rows.InsertAt(r2, d + 1);

                        d = d + SoDong1Trang + 2;
                    }
                    return dt;
                }
                else
                {
                    for (int i = 0; i < SoTrang - 1; i++)
                    {
                        r1 = dt.NewRow();
                        r1["sNoiDung"] = "Cuối trang " + (i + 1);
                        r1["Trang"] = i + 1;


                        dt.Rows.InsertAt(r1, d);
                        r2 = dt.NewRow();
                        r2["sNoiDung"] = "Mang sang";
                        r2["Trang"] = i + 1;

                        dt.Rows.InsertAt(r2, d + 1);

                        d = d + SoDong1Trang + 2;
                    }
                    return dt;
                }
            }

            return dt;

        }
        /// <summary>
        /// KeToan_SoCai_LuyKe
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <returns></returns>
        public static DataTable KeToan_SoCai_LuyKe(String iID_MaTaiKhoan, String NamLamViec, String ThangLamViec)
        {
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "3";
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                if (arrTaiKhoan[i].Length == 3)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap1;
                }
                if (arrTaiKhoan[i].Length == 4)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap2;
                }
                if (arrTaiKhoan[i].Length == 5)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap3;
                }
                if (arrTaiKhoan[i].Length == 6)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap4;
                }
                DK_DonViSelect += ",SUM(TaiKhoan" + i + "_No) AS TaiKhoan" + i + "_No, SUM(TaiKhoan" + i + "_Co) AS TaiKhoan" + i + "_Co";
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan" + i + "_No)>0 OR SUM(TaiKhoan" + i + "_Co)>0";
            }
            DK_DonViSelect = DK_DonViSelect.Substring(1);
            DK_DonViCASE = DK_DonViCASE.Substring(1);
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
            String SQL = String.Format(@"SELECT {0}
                                        FROM(
                                        SELECT  iiD_MaChungTu, sSoChungTuChiTiet,iNgayCT,iThangCT,iID_MaDonVi_No,iID_MaDonVi_Co,sNoiDung,
                                        {1}
                                        FROM KT_ChungTuChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND iThangCT<=@ThangLamViec AND iThangCT<>0 AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY iiD_MaChungTu,iNgayCT,iThangCT,iID_MaDonVi_No,sNoiDung,iID_MaTaiKhoan_No,iID_MaDonVi_Co,iID_MaTaiKhoan_Co,sSoChungTuChiTiet
                                        ) AS A
                                        INNER JOIN (SELECT iiD_MaChungTu,sSoChungTu as SoGS, CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as NgayGS FROM KT_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND iThang<=@ThangLamViec) AS B
                                        ON a.iID_MaChungTu=b.iID_MaChungTu
                                        HAVING {2}", DK_DonViSelect, DK_DonViCASE, DK_DonViHaVing);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow r = dt.NewRow();
                dt.Rows.Add(r);

            }
            dt.Dispose();
            return dt;
        }
        public static DataTable KeToan_SoCai_LuyKe0(String iID_MaTaiKhoan, String NamLamViec, String ThangLamViec)
        {
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "3";
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                if (arrTaiKhoan[i].Length == 3)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap1;
                }
                if (arrTaiKhoan[i].Length == 4)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap2;
                }
                if (arrTaiKhoan[i].Length == 5)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap3;
                }
                if (arrTaiKhoan[i].Length == 6)
                {
                    LengthTaiKhoan = LengthTaiKhoanCap4;
                }
                DK_DonViSelect += ",SUM(TaiKhoan" + i + "_No) AS TaiKhoan" + i + "_No, SUM(TaiKhoan" + i + "_Co) AS TaiKhoan" + i + "_Co";
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan" + i + "_No)>0 OR SUM(TaiKhoan" + i + "_Co)>0";
            }
            DK_DonViSelect = DK_DonViSelect.Substring(1);
            DK_DonViCASE = DK_DonViCASE.Substring(1);
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
            String SQL = String.Format(@"SELECT {0}
                                        FROM(
                                        SELECT  iiD_MaChungTu, sSoChungTuChiTiet,iNgayCT,iThangCT,iID_MaDonVi_No,iID_MaDonVi_Co,sNoiDung,
                                        {1}
                                        FROM KT_ChungTuChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND iThangCT<=@ThangLamViec AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY iiD_MaChungTu,iNgayCT,iThangCT,iID_MaDonVi_No,sNoiDung,iID_MaTaiKhoan_No,iID_MaDonVi_Co,iID_MaTaiKhoan_Co,sSoChungTuChiTiet
                                        ) AS A
                                        INNER JOIN (SELECT iiD_MaChungTu,sSoChungTu as SoGS, CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as NgayGS FROM KT_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND iThang<=@ThangLamViec) AS B
                                        ON a.iID_MaChungTu=b.iID_MaChungTu
                                        HAVING {2}", DK_DonViSelect, DK_DonViCASE, DK_DonViHaVing);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow r = dt.NewRow();
                dt.Rows.Add(r);

            }
            dt.Dispose();
            return dt;
        }
        public static DataTable KeToan_ToIn(String iID_MaPhuongAn, String KhoGiay)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaTo", typeof(String));
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("ToSo", typeof(String));

            String[] iID_MaTaiKhoan = new String[100];
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            if (KhoGiay == "2")
            {
                DataRow R = dt.NewRow();
                dt.Rows.Add(R);
                R[0] = "-1";
                R[1] = "Tờ 1: Nội Dung";
                R[2] = "-1";
                int a = arrPhuongAn.Length % 4;
                if (a == 1)
                {
                    iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3";
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                }
                if (a == 2)
                {
                    iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2";
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                }
                if (a == 3)
                {
                    iID_MaPhuongAn = iID_MaPhuongAn + ",DP1";
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                }
                for (int i = 0; i < arrPhuongAn.Length; i = i + 4)
                {
                    iID_MaTaiKhoan[Convert.ToInt16(i / 4)] += arrPhuongAn[i] + "," + arrPhuongAn[i + 1] + "," + arrPhuongAn[i + 2] + "," + arrPhuongAn[i + 3];
                }
                for (int i = 0; i < (arrPhuongAn.Length / 4); i++)
                {
                    DataRow R1 = dt.NewRow();
                    dt.Rows.Add(R1);
                    R1[0] = iID_MaTaiKhoan[i];
                    R1[1] = "Tờ" + Convert.ToInt16(i + 2) + ": " + iID_MaTaiKhoan[i];
                    R1[2] = Convert.ToInt16(i + 2);
                }
            }
            else
            {
                if (arrPhuongAn.Length <= 8)
                {
                    switch (arrPhuongAn.Length)
                    {
                        case 1: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4,DP5,DP6,DP7";
                            break;
                        case 2: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4,DP5,DP6";
                            break;
                        case 3: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4,DP5";
                            break;
                        case 4: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4";
                            break;
                        case 5: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3";
                            break;
                        case 6: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2";
                            break;
                        case 7: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1";
                            break;
                    }
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                    DataRow R = dt.NewRow();
                    dt.Rows.Add(R);
                    R[0] = "-1," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                    R[1] = "Tờ 1: Nội Dung," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                    R[2] = "1";
                    DataRow R1 = dt.NewRow();
                    dt.Rows.Add(R1);
                    R1[0] = arrPhuongAn[2] + "," + arrPhuongAn[3] + "," + arrPhuongAn[4] + "," + arrPhuongAn[5] + "," + arrPhuongAn[6] + "," + arrPhuongAn[7];
                    R1[1] = "Tờ 2: " + arrPhuongAn[2] + "," + arrPhuongAn[3] + "," + arrPhuongAn[4] + "," + arrPhuongAn[5] + "," + arrPhuongAn[6] + "," + arrPhuongAn[7];
                    R1[2] = "2";
                }
                //nếu độ dài phương án lớn hơn 8
                else
                {
                    int a = arrPhuongAn.Length;
                    int SoCotDu = a % 8;
                    int SoCotThem = 0;
                    if (SoCotDu != 0)
                    {
                        SoCotThem = 8 - SoCotDu;
                        for (int i = 0; i < SoCotThem; i++)
                        {
                            iID_MaPhuongAn = iID_MaPhuongAn + ",DP" + (i + 1);
                        }
                    }
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                    int SoTo = 1;
                    a = arrPhuongAn.Length;
                    SoTo = a / 4;
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                    //Tờ đầu gồm nội dung và 2 tài khoản tiếp theo
                    DataRow R = dt.NewRow();
                    dt.Rows.Add(R);
                    R[0] = "-1," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                    R[1] = "Tờ 1: Nội Dung," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                    R[2] = "1";


                    //Các tờ còn lại mỗi tờ có 4 tài khoản
                    int TenTo = 2;
                    for (int i = 2; i < a - 6; i = i + 4)
                    {
                        DataRow R1 = dt.NewRow();
                        dt.Rows.Add(R1);
                        R1[0] = arrPhuongAn[i] + "," + arrPhuongAn[i + 1] + "," + arrPhuongAn[i + 2] + "," + arrPhuongAn[i + 3];
                        R1[1] = "Tờ " + TenTo + ": " + arrPhuongAn[i] + "," + arrPhuongAn[i + 1] + "," + arrPhuongAn[i + 2] + "," + arrPhuongAn[i + 3];
                        R1[2] = TenTo;
                        TenTo++;
                    }

                    // Tờ cuối là tờ số chẵn có 6 tài khoản
                    DataRow Rc = dt.NewRow();
                    dt.Rows.Add(Rc);
                    Rc[0] = arrPhuongAn[a - 6] + "," + arrPhuongAn[a - 5] + "," + arrPhuongAn[a - 4] + "," + arrPhuongAn[a - 3] + "," + arrPhuongAn[a - 2] + "," + arrPhuongAn[a - 1];
                    Rc[1] = "Tờ " + SoTo + "(Tờ cuối): " + arrPhuongAn[a - 6] + "," + arrPhuongAn[a - 5] + "," + arrPhuongAn[a - 4] + "," + arrPhuongAn[a - 3] + "," + arrPhuongAn[a - 2] + "," + arrPhuongAn[a - 1];
                    Rc[2] = SoTo;
                }
                #region 12

                //else if (arrPhuongAn.Length <= 16)
                //{
                //      switch (arrPhuongAn.Length)
                //    {
                //        case 9: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4,DP5,DP6,DP7";
                //            break;
                //        case 10: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4,DP5,DP6";
                //            break;
                //        case 11: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4,DP5";
                //            break;
                //        case 12: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3,DP4";
                //            break;
                //        case 13: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2,DP3";
                //            break;
                //        case 14: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1,DP2";
                //            break;
                //        case 15: iID_MaPhuongAn = iID_MaPhuongAn + ",DP1";
                //            break;
                //    }
                //    arrPhuongAn = iID_MaPhuongAn.Split(',');
                //    DataRow R = dt.NewRow();
                //    dt.Rows.Add(R);
                //    R[0] = "-1,"+arrPhuongAn[0] + "," + arrPhuongAn[1];
                //    R[1] = "Tờ 1: Nội Dung," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                //    R[2] = " Tờ 1";
                //    DataRow R1 = dt.NewRow();
                //    dt.Rows.Add(R1);
                //    R1[0] = arrPhuongAn[2] + "," + arrPhuongAn[3] + "," + arrPhuongAn[4] + "," + arrPhuongAn[5];
                //    R1[1] = "Tờ 2: " + arrPhuongAn[2] + "," + arrPhuongAn[3] + "," + arrPhuongAn[4] + "," + arrPhuongAn[5];
                //    R1[2] = " Tờ 2";
                //    DataRow R2 = dt.NewRow();
                //    dt.Rows.Add(R2);
                //    R2[0] = arrPhuongAn[6] + "," + arrPhuongAn[7] + "," + arrPhuongAn[8] + "," + arrPhuongAn[9];
                //    R2[1] = "Tờ 3: " + arrPhuongAn[6] + "," + arrPhuongAn[7] + "," + arrPhuongAn[8] + "," + arrPhuongAn[9];
                //    R2[2] = " Tờ 3";
                //    DataRow R3 = dt.NewRow();
                //    dt.Rows.Add(R3);
                //    R3[0] = arrPhuongAn[10] + "," + arrPhuongAn[11] + "," + arrPhuongAn[12] + "," + arrPhuongAn[13] + "," + arrPhuongAn[14] + "," + arrPhuongAn[15];
                //    R3[1] = "Tờ 4: " + arrPhuongAn[10] + "," + arrPhuongAn[11] + "," + arrPhuongAn[12] + "," + arrPhuongAn[13] + "," + arrPhuongAn[14] + "," + arrPhuongAn[15];
                //    R3[2] = " Tờ 4";

                //}
                ////nếu độ dài mã phương án lớn hơn 16
                //else
                //{
                //    int a = (arrPhuongAn.Length - 16) % 10;
                //    int b =0;
                //    if (a>0)
                //        b = 10 - a;
                //    for (int i = 1; i <= b;i++ )
                //    {
                //        iID_MaPhuongAn += ",DP" + i;
                //    }
                //    arrPhuongAn = iID_MaPhuongAn.Split(',');           
                //    DataRow R = dt.NewRow();
                //    dt.Rows.Add(R);
                //    R[0] = "-1," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                //    R[1] = "Tờ 1: Nội Dung," + arrPhuongAn[0] + "," + arrPhuongAn[1];
                //    R[2] = " Tờ 1";
                //    DataRow R1 = dt.NewRow();
                //    dt.Rows.Add(R1);
                //    R1[0] = arrPhuongAn[2] + "," + arrPhuongAn[3] + "," + arrPhuongAn[4] + "," + arrPhuongAn[5];
                //    R1[1] = "Tờ 2: " + arrPhuongAn[2] + "," + arrPhuongAn[3] + "," + arrPhuongAn[4] + "," + arrPhuongAn[5];
                //    R1[2] = " Tờ 2";
                //    DataRow R2 = dt.NewRow();
                //    dt.Rows.Add(R2);
                //    R2[0] = arrPhuongAn[6] + "," + arrPhuongAn[7] + "," + arrPhuongAn[8] + "," + arrPhuongAn[9];
                //    R2[1] = "Tờ 3: " + arrPhuongAn[6] + "," + arrPhuongAn[7] + "," + arrPhuongAn[8] + "," + arrPhuongAn[9];
                //    R2[2] = " Tờ 3";
                //    DataRow R3 = dt.NewRow();
                //    dt.Rows.Add(R3);
                //    R3[0] = arrPhuongAn[10] + "," + arrPhuongAn[11] + "," + arrPhuongAn[12] + "," + arrPhuongAn[13];
                //    R3[1] = "Tờ 4: " + arrPhuongAn[10] + "," + arrPhuongAn[11] + "," + arrPhuongAn[12] + "," + arrPhuongAn[13];
                //    R3[2] = " Tờ 4";

                //}  
                #endregion
            }
            dt.Dispose();
            return dt;
        }

        public JsonResult CheckKhoaSo(String iThang, String iNam)
        {
            String[] arrThang;
            SqlCommand cmd = new SqlCommand();
            switch (iThang)
            {
                case "13":
                    {
                        arrThang = "1,2,3".Split(',');
                    }
                    break;
                case "14":
                    {
                        arrThang = "4,5,6".Split(',');
                    }
                    break;
                case "15":
                    {
                        arrThang = "7,8,9".Split(',');
                    }
                    break;
                case "16":
                    {
                        arrThang = "10,11,12".Split(',');
                    }
                    break;
                case "17":
                    {
                        arrThang = "1,2,3,4,5,6".Split(',');
                    }
                    break;
                case "18":
                    {
                        arrThang = "7,8,9,10,11,12".Split(',');
                    }
                    break;
                case "19":
                    {
                        arrThang = "1,2,3,4,5,6,7,8,9,10,11,12".Split(',');
                    }
                    break;
                default:
                    arrThang = iThang.Split(',');
                    break;
            }
            String DK = "";
            for (int i = 0; i < arrThang.Length; i++)
            {
                if (DK != "") DK += " OR ";
                DK += "iThang=@iThang" + i;
                cmd.Parameters.AddWithValue("@iThang" + i, arrThang[i]);
            }
            if (DK != "")
                DK = " AND (" + DK + ")";
            String SQL = String.Format("SELECT distinct iThang FROM KT_LuyKe WHERE iNam=@iNam {0} ORDER BY iThang ", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            Boolean CoThangKhoaSo = false;
            String sThangChuaKhoaSo = "";
            if (iThang != "")
            {
                for (int i = 0; i < arrThang.Length; i++)
                {
                    CoThangKhoaSo = false;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (arrThang[i] == Convert.ToString(dt.Rows[j]["iThang"]))
                        {
                            CoThangKhoaSo = true;
                        }
                    }
                    if (CoThangKhoaSo == false)
                    {
                        sThangChuaKhoaSo += arrThang[i] + ",";
                    }
                }
            }
            return Json(sThangChuaKhoaSo, JsonRequestBehavior.AllowGet);

        }
    }
}
