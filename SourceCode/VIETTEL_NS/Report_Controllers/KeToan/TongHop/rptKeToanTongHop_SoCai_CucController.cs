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
    public class rptKeToanTongHop_SoCai_CucController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3_Cuoi = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_Cuoi_Cuc.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_1_Cuc.xls";
        private const String sFilePath_A3_Le = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_Le_Cuc.xls";
        private const String sFilePath_A3_Chan = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_Chan_Cuc.xls";
        private const String sFilePath_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A4_Cuc.xls";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A4_1_Cuc.xls";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_SoCai_Cuc.aspx";
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
            String strInLienTiep = Convert.ToString(Request.Form[ParentID + "_InLienTiep"]);
            Boolean InLienTiep = false;
            if (strInLienTiep == "on")
            {
                InLienTiep = true;
            }
            String MatIn = Convert.ToString(Request.Form[ParentID + "_MatIn"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["iID_MaPhuongAn"] = iID_MaPhuongAn;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["ToSo"] = ToSo;
            ViewData["PageLoad"] = "1";
            ViewData["InLienTiep"] = InLienTiep;
            ViewData["MatIn"] = MatIn;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_SoCai_Cuc.aspx";
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
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay, bool InLienTiep, String MatIn)
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
                fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_SoCai_Cuc");
            }

            //  LoadData(fr, NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);

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
            fr.SetValue("ToCuoi", dtTo.Rows.Count);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("SoDuDauKy", SoDuDauKy);
            if (InLienTiep == true)
            {
                DataSet ds = KeToan_SoCaiCuc_InLienTiep(NamLamViec, ThangLamViec, iID_MaPhuongAn, "", KhoGiay,
                                                        InLienTiep, MatIn);

                LuyKe _LuyKe = KeToan_SoCai_Cuc_LuyKe_InLienTiep(iID_MaPhuongAn, KhoGiay, NamLamViec, ThangLamViec, MatIn);

                DataTable dtLuyKeToDau = _LuyKe.LuyKeToDau;
                dtLuyKeToDau.TableName = "LuyKeToDau";
                ds.Tables.Add(dtLuyKeToDau);

                DataTable dtLuyKeChan = _LuyKe.LuyKeToChan;
                dtLuyKeChan.TableName = "LuyKeToChan";
                ds.Tables.Add(dtLuyKeChan);

                DataTable dtLuyKeToCuoi = _LuyKe.LuyKeToCuoi;
                dtLuyKeToCuoi.TableName = "LuyKeToCuoi";
                ds.Tables.Add(dtLuyKeToCuoi);

                DataTable dtLuyKeToLeTrangCuoi = _LuyKe.LuyKeToLeTrangCuoi;
                dtLuyKeToLeTrangCuoi.TableName = "LuyKeToLeTrangCuoi";
                ds.Tables.Add(dtLuyKeToLeTrangCuoi);



                LuyKe0 _luyKe0 = KeToan_SoCai_Cuc_LuyKe0_InLienTiep(iID_MaPhuongAn, KhoGiay, NamLamViec, ThangLamViec, MatIn);

                DataTable dtLuyKe0ToDau = _luyKe0.LuyKe0ToDau;
                dtLuyKe0ToDau.TableName = "LuyKe0ToDau";
                ds.Tables.Add(dtLuyKe0ToDau);

                DataTable dtLuyKe0Chan = _luyKe0.LuyKeTo0Chan;
                dtLuyKe0Chan.TableName = "LuyKe0ToChan";
                ds.Tables.Add(dtLuyKe0Chan);

                DataTable dtLuyKe0ToCuoi = _luyKe0.LuyKeTo0ToCuoi;
                dtLuyKe0ToCuoi.TableName = "LuyKe0ToCuoi";
                ds.Tables.Add(dtLuyKe0ToCuoi);

                DataTable dtLuyKe0ToLeTrangCuoi = _luyKe0.LuyKe0ToLeTrangCuoi;
                dtLuyKe0ToLeTrangCuoi.TableName = "LuyKe0ToLeTrangCuoi";
                ds.Tables.Add(dtLuyKe0ToLeTrangCuoi);

                AddRelationships(ds);
                ds.EnforceConstraints = true;
                fr.AddTable(ds);
                dtLuyKeToDau.Dispose();
                dtLuyKeChan.Dispose();
                dtLuyKeToCuoi.Dispose();
                dtLuyKeToLeTrangCuoi.Dispose();
                dtLuyKe0ToDau.Dispose();
                dtLuyKe0Chan.Dispose();
                dtLuyKe0ToCuoi.Dispose();
                dtLuyKe0ToLeTrangCuoi.Dispose();

            }
            else
            {
                LoadData(fr, NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay); 
            }
            fr.Run(Result);

            return Result;
        }
        private static void AddRelationships(DataSet ds)
        {
            DataRelation relationOrder_Details_FK00 = new DataRelation("Order Details_FK00", new DataColumn[] {
                        ds.Tables["DanhSachToChan"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["ToChan"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK00);

            DataRelation relationOrder_Details_FK01 = new DataRelation("Order Details_FK01", new DataColumn[] {
                        ds.Tables["DanhSachToChan"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["TenToChan"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK01);

            DataRelation relationOrder_Details_FK02 = new DataRelation("Order Details_FK02", new DataColumn[] {
                        ds.Tables["DanhSachToChan"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["LuyKeToChan"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK02);

            DataRelation relationOrder_Details_FK03 = new DataRelation("Order Details_FK03", new DataColumn[] {
                        ds.Tables["DanhSachToChan"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["LuyKe0ToChan"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK03);


            //To Le
            DataRelation relationOrder_Details_FK10 = new DataRelation("Order Details_FK10", new DataColumn[] {
                        ds.Tables["TrangLeToDau"].Columns["Trang"]}, new DataColumn[] {
                        ds.Tables["ToDauTrangGiua"].Columns["Trang"]}, false);
            ds.Relations.Add(relationOrder_Details_FK10);

            DataRelation relationOrder_Details_FK04 = new DataRelation("Order Details_FK04", new DataColumn[] {
                        ds.Tables["DanhSachToLe"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["ToLeTrangCuoi"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK04);

            DataRelation relationOrder_Details_FK041 = new DataRelation("Order Details_FK041", new DataColumn[] {
                        ds.Tables["DanhSachToLe"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["ToLe"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK041);

            DataRelation relationOrder_Details_FK05 = new DataRelation("Order Details_FK05", new DataColumn[] {
                        ds.Tables["DanhSachToLe"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["TenToLe"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK05);
            DataRelation relationOrder_Details_FK051 = new DataRelation("Order Details_FK051", new DataColumn[] {
                        ds.Tables["DanhSachToLe"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["TenToLe1"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK051);

            DataRelation relationOrder_Details_FK06 = new DataRelation("Order Details_FK06", new DataColumn[] {
                        ds.Tables["DanhSachToLe"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["LuyKeToLeTrangCuoi"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK06);

            DataRelation relationOrder_Details_FK07 = new DataRelation("Order Details_FK07", new DataColumn[] {
                        ds.Tables["DanhSachToLe"].Columns["ToSo"]}, new DataColumn[] {
                        ds.Tables["LuyKe0ToLeTrangCuoi"].Columns["ToSo"]}, false);
            ds.Relations.Add(relationOrder_Details_FK07);


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
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay, bool InLienTiep, String MatIn)
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
            if (InLienTiep != true)
            {
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
            }
            else
            {
                if (MatIn == "chan")
                    DuongDanFile = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_ChanLT_Cuc.xls";
                else
                {
                    DuongDanFile = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_LeLT_Cuc.xls";

                }
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay, InLienTiep, MatIn);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_SoCai_Cuc.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay, bool InLienTiep, String MatIn)
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
            if (InLienTiep != true)
            {
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
            }
            else
            {
                if (MatIn == "chan")
                    DuongDanFile = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_ChanLT_Cuc.xls";
                else
                {
                    DuongDanFile = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoCai_A3_LeLT_Cuc.xls";

                }
            }

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay, InLienTiep, MatIn);
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
            DataTable data = KeToan_SoCai_Cuc(NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay, true);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable CuoiTrang = HamChung.SelectDistinct("CuoiTrang", data, "Trang", "Trang", "", "", "");
            fr.AddTable("CuoiTrang", CuoiTrang);
            CuoiTrang.Dispose();
            DataTable LuyKe = KeToan_SoCai_Cuc_LuyKe(iID_MaTaiKhoan, NamLamViec, ThangLamViec);
            data.TableName = "LuyKe";
            fr.AddTable("LuyKe", LuyKe);
            DataTable LuyKe0 = KeToan_SoCai_Cuc_LuyKe0(iID_MaTaiKhoan, NamLamViec, ThangLamViec);
            fr.AddTable("LuyKe0", LuyKe0);
            data.Dispose();
            CuoiTrang.Dispose(); 
            LuyKe.Dispose();
            LuyKe0.Dispose();
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
            String s = MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 250px\" size=\"5\"");
            return s;
        }
        [HttpGet]
        public JsonResult ds_NhomDonVi(String ParentID, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            return Json(obj_data(ParentID, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// KeToan_SoCai_Cuc
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <param name="HienThi"></param>
        /// <returns></returns>
        public static DataTable KeToan_SoCai_Cuc(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay, bool InLienTiep)
        {
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');


            ///set các DK
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
                                        ,DonVi=(SELECT sDonVi FROM KT_ChungTu  WHERE iID_MaChungTu=B.iID_MaChungTu)
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
                        //if (j > tg)
                        //    break;

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
        /// KeToan_SoCai_Cuc_LuyKe
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <returns></returns>
        public static DataTable KeToan_SoCai_Cuc_LuyKe(String iID_MaTaiKhoan, String NamLamViec, String ThangLamViec)
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
        public static DataTable KeToan_SoCai_Cuc_LuyKe0(String iID_MaTaiKhoan, String NamLamViec, String ThangLamViec)
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
        public static DataSet KeToan_SoCaiCuc_InLienTiep(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay, bool InLienTiep, String MatIn)
        {
            //InLienTiep

            DataSet dt1 = new DataSet();
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            String TKTrang1 = "";
            String[] TKToChan = new String[arrPhuongAn.Length];
            String[] TKToLe = new String[arrPhuongAn.Length];
            String TKTrangCuoi = "";

            if (KhoGiay == "2")
            {

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
                }
            }
            ///set các DK
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
                DK_DonViSelect += ",SUM(TaiKhoan" + i + "_No) AS TaiKhoan" + i + "_No, SUM(TaiKhoan" + i + "_Co) AS TaiKhoan" + i + "_Co";
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN  SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
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
                                        ,DonVi=(SELECT sDonVi FROM KT_ChungTu  WHERE iID_MaChungTu=B.iID_MaChungTu)
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
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            #endregion

            #region // tạo dtChungTu
            String SQLChungTu = String.Format(@"SELECT  iID_MaChungTu,SUM(rSoTien)
                                              FROM KT_ChungTuChiTiet
                                              WHERE iTrangThai=1 AND iThangCT=@ThangLamViec AND iNamLamViec=@NamLamViec AND ({0})  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                                GROUP BY iID_MaChungTu
                                                 HAVING SUM(rSoTien)<>0", DKSoTien);
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
                                             WHERE  iTrangThai=1 AND iNamLamViec=@NamLamViec AND rSoTien<>0 AND iThangCT=@ThangLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ");
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
                if (arrDoiUngNo[i] == null)
                    arrDoiUngNo[i] = "";
                if (arrDoiUngCo[i] == null)
                    arrDoiUngCo[i] = "";
                for (int j = tg; j < dtDoiUng.Rows.Count; j++)
                {
                    if (dtChungTu.Rows[i]["iID_MaChungTu"].ToString() == dtDoiUng.Rows[j]["iID_MaChungTu"].ToString())
                    {
                        if (j == 0)
                        {
                            if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Length >= 3)
                            {
                                if (arrDoiUngNo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3)) < 0)
                                {
                                    arrDoiUngNo[i] += "," +
                                                      dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0,
                                                                                                                 3);
                                }
                            }
                            if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Length >= 3)
                            {
                                if (arrDoiUngCo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3)) == -1)
                                {
                                    arrDoiUngCo[i] += "," +
                                                      dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0,
                                                                                                                 3);
                                }
                            }
                        }
                        else
                        {

                            if (dtDoiUng.Rows[j]["iID_MaChungTu"].ToString() != dtDoiUng.Rows[j - 1]["iID_MaChungTu"].ToString())
                            {
                                if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Length >= 3)
                                {
                                    if (arrDoiUngNo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3)) < 0)
                                    {
                                        arrDoiUngNo[i] += "," +
                                                          dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0,
                                                                                                                     3);
                                    }
                                }

                                if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Length >= 3)
                                {
                                    if (arrDoiUngCo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3)) == -1)
                                    {
                                        arrDoiUngCo[i] += "," +
                                                          dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0,
                                                                                                                     3);
                                    }
                                }
                            }
                            else
                            {
                                if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Length >= 3)
                                {
                                    if (dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_No"].ToString().Length < 3)
                                    {
                                        if (arrDoiUngNo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3)) < 0)
                                        {
                                            arrDoiUngNo[i] += "," +
                                                              dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0,
                                                                                                                         3);
                                        }
                                    }
                                    else
                                    {
                                        if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3) != dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3))
                                        {
                                            if (arrDoiUngNo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0, 3)) < 0)
                                            {
                                                arrDoiUngNo[i] += "," +
                                                                  dtDoiUng.Rows[j]["iID_MaTaiKhoan_No"].ToString().Substring(0,
                                                                                                                             3);
                                            }
                                        }
                                    }
                                }
                                if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString() != "" && dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Length >= 3)
                                {
                                    if (dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_Co"].ToString().Length < 3)
                                    {
                                        if (arrDoiUngCo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3)) == -1)
                                        {
                                            arrDoiUngCo[i] += "," +
                                                              dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0,
                                                                                                                         3);
                                        }
                                    }
                                    else
                                    {
                                        if (dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3) != dtDoiUng.Rows[j - 1]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3))
                                        {
                                            if (arrDoiUngCo[i].IndexOf(dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0, 3)) == -1)
                                            {
                                                arrDoiUngCo[i] += "," +
                                                                  dtDoiUng.Rows[j]["iID_MaTaiKhoan_Co"].ToString().Substring(0,
                                                                                                                             3);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        tg++;
                    }
                    //if (j > tg)
                    //    break;

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

            dt.Dispose();

            //Định dạng trang IN
            dt.Columns.Add("Trang", typeof(int));
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
            //Khổ giấy A3
            int SoDong1Trang = 46;
            if (KhoGiay == "1")
            {
                //Định dang dòng cuối trang

                int SoTrang = 1;

                int Count = dt.Rows.Count;
                int SoDu = (Count + 5) % SoDong1Trang;
                SoTrang = (Count + 5) / SoDong1Trang;
                if (SoDu > 0) SoTrang++;
                DataTable dtTrang = new DataTable();
                dtTrang.Columns.Add("Trang", typeof(String));
                DataRow dr1 = dtTrang.NewRow();
                for (int i = 1; i <= SoTrang; i++)
                {
                    dr1 = dtTrang.NewRow();
                    dr1[0] = i;
                    dtTrang.Rows.Add(dr1);
                }
               
                DataTable dtTrangCuoi = dtTrang.Copy();
                if (SoDu >= 39)
                {
                    dr1 = dtTrangCuoi.NewRow();
                    dr1[0] = dtTrangCuoi.Rows.Count + 1;
                    dtTrangCuoi.Rows.Add(dr1);
                    dr1 = dtTrang.NewRow();
                    dr1[0] = dtTrang.Rows.Count + 1;
                    dtTrang.Rows.Add(dr1);
                }
                dtTrang.TableName = "Trang";
                dt1.Tables.Add(dtTrang);
                dtTrangCuoi.TableName = "TrangCuoi";
                dt1.Tables.Add(dtTrangCuoi);

                //Trang le sap xep nguoc lai tru trang cuoi
                DataTable dtTrangLe = new DataTable();
                dtTrangLe.Columns.Add("Trang", typeof(int));
                DataRow drLe = dtTrangLe.NewRow();
                for (int i = SoTrang - 1; i >= 1; i--)
                {
                    drLe = dtTrangLe.NewRow();
                    drLe[0] = i;
                    dtTrangLe.Rows.Add(drLe);
                }
                dtTrangLe.TableName = "TrangLe";
                dt1.Tables.Add(dtTrangLe);
                dtTrangLe.Dispose();

                DataTable dtTrangLeToDau = dtTrangLe.Copy();
                if (dtTrangLeToDau.Rows.Count > 0)
                {
                    dtTrangLeToDau.Rows.RemoveAt(dtTrangLeToDau.Rows.Count - 1);
                }

                dtTrangLeToDau.TableName = "TrangLeToDau";
                dt1.Tables.Add(dtTrangLeToDau);
                dtTrangLeToDau.Dispose();

                SoTrang = Count / SoDong1Trang;
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
                for (int i = 0; i < SoTrang - 1; i++)
                {
                    double TongSoPhatSinh = 0;
                    a = SoDong1Trang * i;
                    b = SoDong1Trang * (i + 1);
                    if (i > 0)
                    {
                        a = a + 2 * i;
                        b = b + 2 * i;
                    }
                    r1 = dt.NewRow();
                    r2 = dt.NewRow();
                    for (int k = 14; k < dt.Columns.Count - 2; k++)
                    {
                        for (int j = a; j < b; j++)
                        {
                            if (dt.Rows[j]["SoPhatSinh"].ToString() != "")
                            {
                                TongSoPhatSinh += Convert.ToDouble(dt.Rows[j][k].ToString());
                                r1[k] = TongSoPhatSinh;
                                r2[k] = TongSoPhatSinh;
                            }
                        }
                        TongSoPhatSinh = 0;
                    }

                    r1["sNoiDung"] = "+ Cuối trang " + (i + 1);
                    r1["SoDong"] = "";
                    r1["Trang"] = i + 1;
                    r1["STT"] = "0";
                    dt.Rows.InsertAt(r1, d);
                    r2["sNoiDung"] = "Mang sang";
                    r2["SoDong"] = "";
                    r2["Trang"] = i + 2;
                    r1["STT"] = "2";

                    dt.Rows.InsertAt(r2, d + 1);
                    d = d + SoDong1Trang + 2;
                }
            }
            //neu kho giay A4
            else if (KhoGiay == "2")
            {


            }



            int count = dt.Columns.Count;
            //to dau
            DataTable dtToDau = dt.Copy();
            for (int i = count - 3; i > 18; i--)
            {
                dtToDau.Columns.RemoveAt(i);
            }
            DataTable dtToDauTrangGiua = dtToDau.Copy();
            int Count2 = dt.Rows.Count;
            int SoDuToCuoi = (Count2) % (SoDong1Trang + 2);
            int SoTrangToCuoi = (Count2) / (SoDong1Trang + 2);
            if (SoDuToCuoi > 0) SoTrangToCuoi++;
            int rowcount = dt.Rows.Count;
            if (MatIn == "chan")
            {
                for (int i = rowcount - 1; i > SoDong1Trang; i--)
                {
                    dtToDau.Rows.RemoveAt(i);
                }
            }
            else
            {
                if (SoDuToCuoi >= 34)
                {
                    for (int i = 0; i < rowcount; i++)
                    {
                        dtToDau.Rows.RemoveAt(0);
                    }
                }
                else
                {
                    for (int i = 0; i < (SoDong1Trang + 2) * (SoTrangToCuoi - 1) - 1; i++)
                    {
                        dtToDau.Rows.RemoveAt(0);
                    }
                    for (int i = rowcount; i > (SoDong1Trang + 2) * (SoTrangToCuoi - 1) - 1; i--)
                    {
                        dtToDauTrangGiua.Rows.RemoveAt(dtToDauTrangGiua.Rows.Count - 1);
                    }
                }
                
                for (int i = 0; i <= SoDong1Trang; i++)
                {
                    dtToDauTrangGiua.Rows.RemoveAt(0);
                }
               
            }
            if (SoDuToCuoi >= 34)
            {
                for (int i = 0; i < SoDong1Trang + 1 - SoDuToCuoi; i++)
                {
                    
                        DataRow row1 = dtToDauTrangGiua.NewRow();
                        row1["STT"] = 999;
                        row1["Trang"] = SoTrangToCuoi;
                        dtToDauTrangGiua.Rows.Add(row1);
                    
                }
            }
            dtToDau.TableName = "ToDau";

            dtToDauTrangGiua.TableName = "ToDauTrangGiua";
            dt1.Tables.Add(dtToDauTrangGiua);
            dtToDauTrangGiua.Dispose();
            //To cuoi
            dt1.Tables.Add(dtToDau);
            dtToDau.Dispose();
            DataTable dtToCuoi = dt.Copy();
            for (int i = count - 15; i > 14; i--)
            {
                dtToCuoi.Columns.RemoveAt(i);
            }
            dtToCuoi.Columns[15].ColumnName = "TaiKhoan0_No";
            dtToCuoi.Columns[16].ColumnName = "TaiKhoan0_Co";
            dtToCuoi.Columns[17].ColumnName = "TaiKhoan1_No";
            dtToCuoi.Columns[18].ColumnName = "TaiKhoan1_Co";
            dtToCuoi.Columns[19].ColumnName = "TaiKhoan2_No";
            dtToCuoi.Columns[20].ColumnName = "TaiKhoan2_Co";
            dtToCuoi.Columns[21].ColumnName = "TaiKhoan3_No";
            dtToCuoi.Columns[22].ColumnName = "TaiKhoan3_Co";
            dtToCuoi.Columns[23].ColumnName = "TaiKhoan4_No";
            dtToCuoi.Columns[24].ColumnName = "TaiKhoan4_Co";
            dtToCuoi.Columns[25].ColumnName = "TaiKhoan5_No";
            dtToCuoi.Columns[26].ColumnName = "TaiKhoan5_Co";
            dtToCuoi.TableName = "ToCuoi";

            if (SoDuToCuoi >= 34)
            {
                for (int i = 0; i < SoDong1Trang + 1 - SoDuToCuoi; i++)
                {
                    DataRow row1 = dtToCuoi.NewRow();
                    row1["STT"] = 999;
                    row1["Trang"] = SoTrangToCuoi;
                    dtToCuoi.Rows.Add(row1);
                }
            }
            dt1.Tables.Add(dtToCuoi);

            dtTo = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            DataTable dtToChan = new DataTable();
            dtToChan.Columns.Add("STT", typeof(string));
            dtToChan.Columns.Add("iID_MaChungTu", typeof(string));
            dtToChan.Columns.Add("SoGS", typeof(string));
            dtToChan.Columns.Add("NgayGS", typeof(string));
            dtToChan.Columns.Add("iSoGS", typeof(string));
            dtToChan.Columns.Add("SoCT", typeof(string));
            dtToChan.Columns.Add("NgayCT", typeof(string));
            dtToChan.Columns.Add("iNgay", typeof(string));
            dtToChan.Columns.Add("DoiUng_No", typeof(string));
            dtToChan.Columns.Add("DoiUng_Co", typeof(string));
            dtToChan.Columns.Add("DonVi_No", typeof(string));
            dtToChan.Columns.Add("DonVi_Co", typeof(string));
            dtToChan.Columns.Add("DonVi", typeof(string));
            dtToChan.Columns.Add("sNoiDung", typeof(string));
            dtToChan.Columns.Add("SoPhatSinh", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToChan.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToChan.Columns.Add("Trang", typeof(String));
            dtToChan.Columns.Add("SoDong", typeof(String));
            dtToChan.Columns.Add("ToSo", typeof(string));
            //chia to
            for (int i = 1; i < dtTo.Rows.Count - 2; i = i + 2)
            {
                DataTable dtChan = dt.Copy();
                int j = count;
                for (j = count - 3; j > count - (15 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtChan.Columns.RemoveAt(j);
                }
                int z = count - (15 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c > 14; c--)
                {
                    dtChan.Columns.RemoveAt(c);
                }
                //dtChan.TableName = "To" + Convert.ToInt32(i + 1);
                //dt1.Tables.Add(dtChan);
                for (int x = 0; x < dtChan.Rows.Count; x++)
                {
                    DataRow dr = dtToChan.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtChan.Columns.Count; y++)
                    {
                        dr[y] = dtChan.Rows[x][y];
                    }
                    dtToChan.Rows.Add(dr);
                }
            }
            if (SoDuToCuoi >= 34)
            {
                for (int i = 0; i < SoDong1Trang + 1 - SoDuToCuoi; i++)
                {
                    for (int j = 2; j <= dtTo.Rows.Count - 2; j = j + 2)
                    {
                        DataRow row1 = dtToChan.NewRow();
                        row1["STT"] = 999;
                        row1["Trang"] = SoTrangToCuoi;
                        row1["ToSo"] = j;
                        dtToChan.Rows.Add(row1);
                    }
                }
            }
            dtToChan.TableName = "ToChan";
            dt1.Tables.Add(dtToChan);

            //Danh sach cac to chan
            DataTable dtSoToChan = new DataTable();
            dtSoToChan.Columns.Add("ToSo", typeof(string));
            for (int i = 1; i < dtTo.Rows.Count - 1; i = i + 2)
            {
                DataRow dr = dtSoToChan.NewRow();
                dr["ToSo"] = Convert.ToInt16(i + 1);
                dtSoToChan.Rows.Add(dr);
            }
            dtSoToChan.TableName = "DanhSachToChan";
            dt1.Tables.Add(dtSoToChan);
            //danh sach ten tai khoan, ma tai khoan to dau
            DataTable dtTenTKToDau = new DataTable();
            dtTenTKToDau.Columns.Add("MaTK0", typeof(string));
            dtTenTKToDau.Columns.Add("TenTK0", typeof(string));
            dtTenTKToDau.Columns.Add("MaTK1", typeof(string));
            dtTenTKToDau.Columns.Add("TenTK1", typeof(string));
            String[] arrMaTKToDau = new string[3];
            arrMaTKToDau = Convert.ToString(dtTo.Rows[0]["MaTo"]).Split(',');
            DataRow drToDau = dtTenTKToDau.NewRow();
            drToDau["MaTK0"] = arrMaTKToDau[1];
            drToDau["MaTK1"] = arrMaTKToDau[2];
            drToDau["TenTK0"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToDau[1], "sTen"));
            drToDau["TenTK1"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToDau[2], "sTen"));
            dtTenTKToDau.Rows.Add(drToDau);

            dtTenTKToDau.TableName = "TenToDau";
            dt1.Tables.Add(dtTenTKToDau);
            //danh sach ten tai khoan, ma tai khoan cac to chan
            DataTable dtTenTKToChan = new DataTable();
            dtTenTKToChan.Columns.Add("ToSo", typeof(string));
            dtTenTKToChan.Columns.Add("MaTK1", typeof(string));
            dtTenTKToChan.Columns.Add("TenTK1", typeof(string));
            dtTenTKToChan.Columns.Add("MaTK2", typeof(string));
            dtTenTKToChan.Columns.Add("TenTK2", typeof(string));
            dtTenTKToChan.Columns.Add("MaTK3", typeof(string));
            dtTenTKToChan.Columns.Add("TenTK3", typeof(string));
            dtTenTKToChan.Columns.Add("MaTK4", typeof(string));
            dtTenTKToChan.Columns.Add("TenTK4", typeof(string));
            for (int i = 1; i < dtTo.Rows.Count - 1; i = i + 2)
            {
                DataRow dr = dtTenTKToChan.NewRow();
                dr["ToSo"] = Convert.ToInt16(i + 1);
                String[] arrMaTKToChan = new string[8];
                arrMaTKToChan = Convert.ToString(dtTo.Rows[i]["MaTo"]).Split(',');
                String[] arrTenTKToChan = new String[arrMaTKToChan.Length];
                int tg1 = 0;
                for (int j = 1; j < 9; j = j + 2)
                {
                    dr[j] = arrMaTKToChan[tg1];
                    arrMaTKToChan[tg1] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToChan[tg1], "sTen"));
                    dr[j + 1] = arrMaTKToChan[tg1];
                    tg1++;
                }
                dtTenTKToChan.Rows.Add(dr);
            }
            dtTenTKToChan.TableName = "TenToChan";
            dt1.Tables.Add(dtTenTKToChan);
            //danh sach ten tai khoan, ma tai khoan cac to Le
            DataTable dtTenTKToLe = new DataTable();
            dtTenTKToLe.Columns.Add("ToSo", typeof(string));
            dtTenTKToLe.Columns.Add("MaTK1", typeof(string));
            dtTenTKToLe.Columns.Add("TenTK1", typeof(string));
            dtTenTKToLe.Columns.Add("MaTK2", typeof(string));
            dtTenTKToLe.Columns.Add("TenTK2", typeof(string));
            dtTenTKToLe.Columns.Add("MaTK3", typeof(string));
            dtTenTKToLe.Columns.Add("TenTK3", typeof(string));
            dtTenTKToLe.Columns.Add("MaTK4", typeof(string));
            dtTenTKToLe.Columns.Add("TenTK4", typeof(string));
            for (int i = 2; i < dtTo.Rows.Count - 1; i = i + 2)
            {
                DataRow dr = dtTenTKToLe.NewRow();
                dr["ToSo"] = Convert.ToInt16(i + 1);
                String[] arrMaTKToLe = new string[8];
                arrMaTKToLe = Convert.ToString(dtTo.Rows[i]["MaTo"]).Split(',');
                String[] arrTenTKToLe = new String[arrMaTKToLe.Length];
                int tg1 = 0;
                for (int j = 1; j < 9; j = j + 2)
                {
                    dr[j] = arrMaTKToLe[tg1];
                    arrMaTKToLe[tg1] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToLe[tg1], "sTen"));
                    dr[j + 1] = arrMaTKToLe[tg1];
                    tg1++;
                }
                dtTenTKToLe.Rows.Add(dr);
            }
            dtTenTKToLe.TableName = "TenToLe";
            dt1.Tables.Add(dtTenTKToLe);
            DataTable dtTenTKToLe1 = dtTenTKToLe.Copy();
            dtTenTKToLe1.TableName = "TenToLe1";
            dt1.Tables.Add(dtTenTKToLe1);
            //Danh sach ten va ma tk to cuoi
            DataTable dtTenTKToCuoi = new DataTable();
            dtTenTKToCuoi.Columns.Add("MaTK0", typeof(string));
            dtTenTKToCuoi.Columns.Add("TenTK0", typeof(string));
            dtTenTKToCuoi.Columns.Add("MaTK1", typeof(string));
            dtTenTKToCuoi.Columns.Add("TenTK1", typeof(string));
            dtTenTKToCuoi.Columns.Add("MaTK2", typeof(string));
            dtTenTKToCuoi.Columns.Add("TenTK2", typeof(string));
            dtTenTKToCuoi.Columns.Add("MaTK3", typeof(string));
            dtTenTKToCuoi.Columns.Add("TenTK3", typeof(string));
            dtTenTKToCuoi.Columns.Add("MaTK4", typeof(string));
            dtTenTKToCuoi.Columns.Add("TenTK4", typeof(string));
            dtTenTKToCuoi.Columns.Add("MaTK5", typeof(string));
            dtTenTKToCuoi.Columns.Add("TenTK5", typeof(string));
            String[] arrMaTKToCuoi = new string[6];
            arrMaTKToCuoi = Convert.ToString(dtTo.Rows[dtTo.Rows.Count - 1]["MaTo"]).Split(',');
            DataRow drToCuoi = dtTenTKToCuoi.NewRow();
            drToCuoi["MaTK0"] = arrMaTKToCuoi[0];
            drToCuoi["MaTK1"] = arrMaTKToCuoi[1];
            drToCuoi["MaTK2"] = arrMaTKToCuoi[2];
            drToCuoi["MaTK3"] = arrMaTKToCuoi[3];
            drToCuoi["MaTK4"] = arrMaTKToCuoi[4];
            drToCuoi["MaTK5"] = arrMaTKToCuoi[5];
            drToCuoi["TenTK0"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToCuoi[0], "sTen"));
            drToCuoi["TenTK1"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToCuoi[1], "sTen"));
            drToCuoi["TenTK2"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToCuoi[2], "sTen"));
            drToCuoi["TenTK3"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToCuoi[3], "sTen"));
            drToCuoi["TenTK4"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToCuoi[4], "sTen"));
            drToCuoi["TenTK5"] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTKToCuoi[5], "sTen"));
            dtTenTKToCuoi.Rows.Add(drToCuoi);

            dtTenTKToCuoi.TableName = "TenToCuoi";
            dt1.Tables.Add(dtTenTKToCuoi);


            #region Cac to le
            dv = dt.DefaultView;
            dv.Sort = "Trang DESC,STT,iSoGS";
            dt = dv.ToTable();
            int Count1 = dt.Rows.Count;
            int SoDu1 = Count1 % 46;
            int SoTrang1 = Count1 / 46;
            if (SoDu1 > 0) SoTrang1++;

            DataTable dtTrangCuoiToLe = dt.Copy();
            DataTable dtToLe = dt.Copy();
            for (int i = Count1 - 1; i >= 0; i--)
            {
                if (Convert.ToString(dt.Rows[i]["Trang"]) != SoTrang1.ToString())
                {
                    dtTrangCuoiToLe.Rows.RemoveAt(i);
                }
                else if (Convert.ToString(dt.Rows[i]["Trang"]) == SoTrang1.ToString() || Convert.ToString(dt.Rows[i]["Trang"]) == "1")
                {
                    dtToLe.Rows.RemoveAt(i);
                }
            }
            int SoDongTrangCuoi = 10;
            if (SoDuToCuoi >= 34)
            {
                SoDongTrangCuoi = 4;
            }
            else
            {
                SoDongTrangCuoi = SoDuToCuoi+5;
            }
            int SoDongTrang = 48 - SoDongTrangCuoi;
            DataTable dtDongTrang = new DataTable();
            dtDongTrang.Columns.Add("DongTrang", typeof(String));
            for (int i = 0; i < SoDongTrang; i++)
            {
                DataRow drDongTrang = dtDongTrang.NewRow();
                dtDongTrang.Rows.Add(drDongTrang);
            }
            dtDongTrang.TableName = "DongTrang";
            dt1.Tables.Add(dtDongTrang);
            dtDongTrang.Dispose();
            DataTable dtDongTrangToDau = dtDongTrang.Copy();
            dtDongTrangToDau.TableName = "DongTrangToDau";
            dt1.Tables.Add(dtDongTrangToDau);
            dtDongTrangToDau.Dispose();
            DataTable dtToLeTrangCuoi = new DataTable();
            dtToLeTrangCuoi.Columns.Add("STT", typeof(string));
            dtToLeTrangCuoi.Columns.Add("iID_MaChungTu", typeof(string));
            dtToLeTrangCuoi.Columns.Add("SoGS", typeof(string));
            dtToLeTrangCuoi.Columns.Add("NgayGS", typeof(string));
            dtToLeTrangCuoi.Columns.Add("iSoGS", typeof(string));
            dtToLeTrangCuoi.Columns.Add("SoCT", typeof(string));
            dtToLeTrangCuoi.Columns.Add("NgayCT", typeof(string));
            dtToLeTrangCuoi.Columns.Add("iNgay", typeof(string));
            dtToLeTrangCuoi.Columns.Add("DoiUng_No", typeof(string));
            dtToLeTrangCuoi.Columns.Add("DoiUng_Co", typeof(string));
            dtToLeTrangCuoi.Columns.Add("DonVi_No", typeof(string));
            dtToLeTrangCuoi.Columns.Add("DonVi_Co", typeof(string));
            dtToLeTrangCuoi.Columns.Add("DonVi", typeof(string));
            dtToLeTrangCuoi.Columns.Add("sNoiDung", typeof(string));
            dtToLeTrangCuoi.Columns.Add("SoPhatSinh", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToLeTrangCuoi.Columns.Add("Trang", typeof(String));
            dtToLeTrangCuoi.Columns.Add("SoDong", typeof(String));
            dtToLeTrangCuoi.Columns.Add("ToSo", typeof(string));
            //chia to

            for (int i = 2; i < dtTo.Rows.Count - 1; i = i + 2)
            {
                DataTable dtLeTrangCuoi = dtTrangCuoiToLe.Copy();
                int j = count;
                for (j = count - 3; j > count - (15 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtLeTrangCuoi.Columns.RemoveAt(j);
                }
                int z = count - (15 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c > 14; c--)
                {
                    dtLeTrangCuoi.Columns.RemoveAt(c);
                }
                dtLeTrangCuoi.TableName = "To" + Convert.ToInt32(i + 1);
                for (int x = 0; x < dtLeTrangCuoi.Rows.Count; x++)
                {
                    DataRow dr = dtToLeTrangCuoi.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtLeTrangCuoi.Columns.Count; y++)
                    {
                        dr[y] = dtLeTrangCuoi.Rows[x][y];
                    }
                    dtToLeTrangCuoi.Rows.Add(dr);
                }
            }
            dtToLeTrangCuoi.TableName = "ToLeTrangCuoi";
            dt1.Tables.Add(dtToLeTrangCuoi);

            //cacToLe tru trangcuoi va trang 1
            DataTable dtToLeConLai = new DataTable();
            dtToLeConLai.Columns.Add("STT", typeof(string));
            dtToLeConLai.Columns.Add("iID_MaChungTu", typeof(string));
            dtToLeConLai.Columns.Add("SoGS", typeof(string));
            dtToLeConLai.Columns.Add("NgayGS", typeof(string));
            dtToLeConLai.Columns.Add("iSoGS", typeof(string));
            dtToLeConLai.Columns.Add("SoCT", typeof(string));
            dtToLeConLai.Columns.Add("NgayCT", typeof(string));
            dtToLeConLai.Columns.Add("iNgay", typeof(string));
            dtToLeConLai.Columns.Add("DoiUng_No", typeof(string));
            dtToLeConLai.Columns.Add("DoiUng_Co", typeof(string));
            dtToLeConLai.Columns.Add("DonVi_No", typeof(string));
            dtToLeConLai.Columns.Add("DonVi_Co", typeof(string));
            dtToLeConLai.Columns.Add("DonVi", typeof(string));
            dtToLeConLai.Columns.Add("sNoiDung", typeof(string));
            dtToLeConLai.Columns.Add("SoPhatSinh", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToLeConLai.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToLeConLai.Columns.Add("Trang", typeof(String));
            dtToLeConLai.Columns.Add("SoDong", typeof(String));
            dtToLeConLai.Columns.Add("ToSo", typeof(string));
            //chia to
            for (int i = 2; i < dtTo.Rows.Count - 1; i = i + 2)
            {
                DataTable dtLeConLai = dtToLe.Copy();
                int j = count;
                for (j = count - 3; j > count - (15 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtLeConLai.Columns.RemoveAt(j);
                }
                int z = count - (15 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c > 14; c--)
                {
                    dtLeConLai.Columns.RemoveAt(c);
                }
                dtLeConLai.TableName = "To" + Convert.ToInt32(i + 1);
                for (int x = 0; x < dtLeConLai.Rows.Count; x++)
                {
                    DataRow dr = dtToLeConLai.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtLeConLai.Columns.Count; y++)
                    {
                        dr[y] = dtLeConLai.Rows[x][y];
                    }
                    dtToLeConLai.Rows.Add(dr);
                }
            }
            if (SoDuToCuoi >= 34)
            {
                for (int i = 0; i < SoDong1Trang + 1 - SoDuToCuoi; i++)
                {
                    for (int j = 3; j <= dtTo.Rows.Count - 1; j = j + 2)
                    {
                        DataRow row1 = dtToLeConLai.NewRow();
                        row1["STT"] = 999;
                        row1["Trang"] = SoTrangToCuoi;
                        row1["ToSo"] = j;
                        dtToLeConLai.Rows.Add(row1);
                    }
                }
            }
            dtToLeConLai.TableName = "ToLe";

            dt1.Tables.Add(dtToLeConLai);

            //Danh sach cac to le
            DataTable dtSoToLe = new DataTable();
            dtSoToLe.Columns.Add("ToSo", typeof(string));
            for (int i = dtTo.Rows.Count - 2; i >= 2; i = i - 2)
            {
                DataRow dr = dtSoToLe.NewRow();
                dr["ToSo"] = Convert.ToInt16(i + 1);
                dtSoToLe.Rows.Add(dr);
            }
            dtSoToLe.TableName = "DanhSachToLe";
            dt1.Tables.Add(dtSoToLe);
            #endregion
            dt1.Dispose();
            return dt1;

        }

        public class LuyKe
        {
            public DataTable LuyKeToDau { get; set; }
            public DataTable LuyKeToChan { get; set; }
            public DataTable LuyKeToCuoi { get; set; }
            public DataTable LuyKeToLeTrangCuoi { get; set; }
        }
        public class LuyKe0
        {
            public DataTable LuyKe0ToDau { get; set; }
            public DataTable LuyKeTo0Chan { get; set; }
            public DataTable LuyKeTo0ToCuoi { get; set; }
            public DataTable LuyKe0ToLeTrangCuoi { get; set; }
        }
        public static LuyKe KeToan_SoCai_Cuc_LuyKe_InLienTiep(String iID_MaPhuongAn, String KhoGiay, String NamLamViec, String ThangLamViec, String MatIn)
        {
            LuyKe _LuyKe = new LuyKe();
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            //Giay a 4
            if (KhoGiay == "2")
            {

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
                }
            }
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
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
            for (int i = 0; i < arrPhuongAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrPhuongAn[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow r = dt.NewRow();
                dt.Rows.Add(r);
            }
            int count = dt.Columns.Count;
            //to dau
            DataTable dtLuyKeToDau = dt.Copy();
            for (int i = count - 1; i > 3; i--)
            {
                dtLuyKeToDau.Columns.RemoveAt(i);
            }
            dtLuyKeToDau.TableName = "ToDau";
            //to cuoi
            DataTable dtLuyKeToCuoi = dt.Copy();
            for (int i = count - 13; i >= 0; i--)
            {
                dtLuyKeToCuoi.Columns.RemoveAt(i);
            }
            dtLuyKeToCuoi.Columns[0].ColumnName = "TaiKhoan0_No";
            dtLuyKeToCuoi.Columns[1].ColumnName = "TaiKhoan0_Co";
            dtLuyKeToCuoi.Columns[2].ColumnName = "TaiKhoan1_No";
            dtLuyKeToCuoi.Columns[3].ColumnName = "TaiKhoan1_Co";
            dtLuyKeToCuoi.Columns[4].ColumnName = "TaiKhoan2_No";
            dtLuyKeToCuoi.Columns[5].ColumnName = "TaiKhoan2_Co";
            dtLuyKeToCuoi.Columns[6].ColumnName = "TaiKhoan3_No";
            dtLuyKeToCuoi.Columns[7].ColumnName = "TaiKhoan3_Co";
            dtLuyKeToCuoi.Columns[8].ColumnName = "TaiKhoan4_No";
            dtLuyKeToCuoi.Columns[9].ColumnName = "TaiKhoan4_Co";
            dtLuyKeToCuoi.Columns[10].ColumnName = "TaiKhoan5_No";
            dtLuyKeToCuoi.Columns[11].ColumnName = "TaiKhoan5_Co";
            dtLuyKeToCuoi.TableName = "ToCuoi";
            DataTable dtToChan_LuyKe = new DataTable();
            dtToChan_LuyKe.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("ToSo", typeof(string));
            DataTable dtTo = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            for (int i = 1; i < dtTo.Rows.Count - 2; i = i + 2)
            {
                DataTable dtChan = dt.Copy();
                int j = count;
                for (j = count - 1; j > count - (13 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtChan.Columns.RemoveAt(j);
                }
                int z = count - (13 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c >= 0; c--)
                {
                    dtChan.Columns.RemoveAt(c);
                }
                for (int x = 0; x < dtChan.Rows.Count; x++)
                {
                    DataRow dr = dtToChan_LuyKe.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtChan.Columns.Count; y++)
                    {
                        dr[y] = dtChan.Rows[x][y];
                    }
                    dtToChan_LuyKe.Rows.Add(dr);
                }
            }

            //To le 
            DataTable dtToLe_LuyKe = new DataTable();
            dtToLe_LuyKe.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("ToSo", typeof(string));
            for (int i = 2; i <= dtTo.Rows.Count - 2; i = i + 2)
            {
                DataTable dtLe = dt.Copy();
                int j = count;
                for (j = count - 1; j > count - (13 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtLe.Columns.RemoveAt(j);
                }
                int z = count - (13 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c >= 0; c--)
                {
                    dtLe.Columns.RemoveAt(c);
                }
                for (int x = 0; x < dtLe.Rows.Count; x++)
                {
                    DataRow dr = dtToLe_LuyKe.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtLe.Columns.Count; y++)
                    {
                        dr[y] = dtLe.Rows[x][y];
                    }
                    dtToLe_LuyKe.Rows.Add(dr);
                }
            }
            _LuyKe.LuyKeToChan = dtToChan_LuyKe;
            _LuyKe.LuyKeToDau = dtLuyKeToDau;
            _LuyKe.LuyKeToCuoi = dtLuyKeToCuoi;
            _LuyKe.LuyKeToLeTrangCuoi = dtToLe_LuyKe;
            dt.Dispose();
            dtToChan_LuyKe.Dispose();
            dtToLe_LuyKe.Dispose();
            return _LuyKe;
        }
        public static LuyKe0 KeToan_SoCai_Cuc_LuyKe0_InLienTiep(String iID_MaPhuongAn, String KhoGiay, String NamLamViec, String ThangLamViec, String MatIn)
        {
            LuyKe0 _LuyKe0 = new LuyKe0();
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            //Giay a 4
            if (KhoGiay == "2")
            {

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
                }
            }
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
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
            for (int i = 0; i < arrPhuongAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrPhuongAn[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow r = dt.NewRow();
                dt.Rows.Add(r);
            }
            int count = dt.Columns.Count;
            //To Dau
            DataTable dtLuyKeToDau = dt.Copy();
            for (int i = count - 1; i > 3; i--)
            {
                dtLuyKeToDau.Columns.RemoveAt(i);
            }
            dtLuyKeToDau.TableName = "ToDau";
            //ToCuoi
            DataTable dtLuyKeToCuoi = dt.Copy();
            for (int i = count - 13; i >= 0; i--)
            {
                dtLuyKeToCuoi.Columns.RemoveAt(i);
            }
            dtLuyKeToCuoi.Columns[0].ColumnName = "TaiKhoan0_No";
            dtLuyKeToCuoi.Columns[1].ColumnName = "TaiKhoan0_Co";
            dtLuyKeToCuoi.Columns[2].ColumnName = "TaiKhoan1_No";
            dtLuyKeToCuoi.Columns[3].ColumnName = "TaiKhoan1_Co";
            dtLuyKeToCuoi.Columns[4].ColumnName = "TaiKhoan2_No";
            dtLuyKeToCuoi.Columns[5].ColumnName = "TaiKhoan2_Co";
            dtLuyKeToCuoi.Columns[6].ColumnName = "TaiKhoan3_No";
            dtLuyKeToCuoi.Columns[7].ColumnName = "TaiKhoan3_Co";
            dtLuyKeToCuoi.Columns[8].ColumnName = "TaiKhoan4_No";
            dtLuyKeToCuoi.Columns[9].ColumnName = "TaiKhoan4_Co";
            dtLuyKeToCuoi.Columns[10].ColumnName = "TaiKhoan5_No";
            dtLuyKeToCuoi.Columns[11].ColumnName = "TaiKhoan5_Co";
            dtLuyKeToCuoi.TableName = "ToCuoi";
            //ToChan
            DataTable dtToChan_LuyKe = new DataTable();
            dtToChan_LuyKe.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToChan_LuyKe.Columns.Add("ToSo", typeof(string));
            DataTable dtTo = KeToan_ToIn(iID_MaPhuongAn, KhoGiay);
            for (int i = 1; i < dtTo.Rows.Count - 2; i = i + 2)
            {
                DataTable dtChan = dt.Copy();
                int j = count;
                for (j = count - 1; j > count - (13 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtChan.Columns.RemoveAt(j);
                }
                int z = count - (13 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c >= 0; c--)
                {
                    dtChan.Columns.RemoveAt(c);
                }
                for (int x = 0; x < dtChan.Rows.Count; x++)
                {
                    DataRow dr = dtToChan_LuyKe.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtChan.Columns.Count; y++)
                    {
                        dr[y] = dtChan.Rows[x][y];
                    }
                    dtToChan_LuyKe.Rows.Add(dr);
                }
            }
            //To le 
            DataTable dtToLe_LuyKe = new DataTable();
            dtToLe_LuyKe.Columns.Add("TaiKhoan1_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan1_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan2_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan2_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan3_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan3_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan4_No", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("TaiKhoan4_Co", typeof(decimal));
            dtToLe_LuyKe.Columns.Add("ToSo", typeof(string));
            for (int i = 2; i <= dtTo.Rows.Count - 2; i = i + 2)
            {
                DataTable dtLe = dt.Copy();
                int j = count;
                for (j = count - 1; j > count - (13 + 8 * (dtTo.Rows.Count - i - 2)); j--)
                {
                    dtLe.Columns.RemoveAt(j);
                }
                int z = count - (13 + 8 * (dtTo.Rows.Count - i - 2)) - 8;
                for (int c = z; c >= 0; c--)
                {
                    dtLe.Columns.RemoveAt(c);
                }
                for (int x = 0; x < dtLe.Rows.Count; x++)
                {
                    DataRow dr = dtToLe_LuyKe.NewRow();
                    dr["ToSo"] = Convert.ToInt32(i + 1);
                    for (int y = 0; y < dtLe.Columns.Count; y++)
                    {
                        dr[y] = dtLe.Rows[x][y];
                    }
                    dtToLe_LuyKe.Rows.Add(dr);
                }
            }
            dt.Dispose();
            dtToChan_LuyKe.Dispose();
            dtToLe_LuyKe.Dispose();
            _LuyKe0.LuyKe0ToDau = dtLuyKeToDau;
            _LuyKe0.LuyKeTo0Chan = dtToChan_LuyKe;
            _LuyKe0.LuyKeTo0ToCuoi = dtLuyKeToCuoi;
            _LuyKe0.LuyKe0ToLeTrangCuoi = dtToLe_LuyKe;
            return _LuyKe0;
        }
        public static DataTable KeToan_MatIn()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MatIn", typeof(String));
            dt.Columns.Add("TenMatIn", typeof(String));
            DataRow R = dt.NewRow();
            R["MatIn"] = "chan";
            R["TenMatIn"] = "In tất cả các tờ chẵn";
            dt.Rows.Add(R);

            DataRow R1 = dt.NewRow();
            R1["MatIn"] = "le";
            R1["TenMatIn"] = "In tất cả các tờ lẻ";
            dt.Rows.Add(R1);
            return dt;
        }

    }
}
