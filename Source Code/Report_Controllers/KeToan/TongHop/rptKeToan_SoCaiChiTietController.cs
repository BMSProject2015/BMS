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
    public class rptKeToan_SoCaiChiTietController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_SoCaiChiTiet_A3.xls";
        private const String sFilePath_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_SoCaiChiTiet_A4.xls";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_SoCaiChiTiet.aspx";
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
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["iID_MaPhuongAn"] = iID_MaPhuongAn;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_SoCaiChiTiet.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            String ToSo = "";

            DataTable dtTo = KeToan_ToIn(iID_MaPhuongAn);
            for (int i = 0; i < dtTo.Rows.Count; i++)
            {
                if (iID_MaTaiKhoan == dtTo.Rows[i]["MaTo"].ToString())
                {
                    ToSo = dtTo.Rows[i]["ToSo"].ToString();
                }
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            if (ToSo != dtTo.Rows[dtTo.Rows.Count - 1]["ToSo"].ToString())
            {
                fr = ReportModels.LayThongTinChuKy(fr, "");
            }
            else
            {
                fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_SoCaiChiTiet");
            }
            LoadData(fr, NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam = "Tháng " + ThangLamViec + " Năm " + NamLamViec;

            String  TenTK = "";
            String[] arrMaTK = new String[4];
            String[] arrTenTK = new String[4];
           
                String[] arrMaTaiKhoan = iID_MaTaiKhoan.Split(',');
                for (int i = 0; i < arrMaTaiKhoan.Length; i++)
                {
                    arrMaTK[i] = arrMaTaiKhoan[i];
                    arrTenTK[i] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTaiKhoan[i], "sTen"));
                }
                String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
                String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangNam", ThangNam);
            fr.SetValue("MaTK", iID_MaTaiKhoan);
            fr.SetValue("TenTK", TenTK);
            fr.SetValue("MaTK0", arrMaTK[0]);
            fr.SetValue("MaTK1", arrMaTK[1]);
            fr.SetValue("MaTK2", arrMaTK[2]);
            fr.SetValue("MaTK3", arrMaTK[3]);
            fr.SetValue("TenTK0", arrTenTK[0]);
            fr.SetValue("TenTK1", arrTenTK[1]);
            fr.SetValue("TenTK2", arrTenTK[2]);
            fr.SetValue("TenTK3", arrTenTK[3]);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);

            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";
          
                if (KhoGiay == "1")
                {
                    DuongDanFile = sFilePath_A3;
                }
                else
                {
                    DuongDanFile = sFilePath_A4;
                }
            
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_SoCaiChiTiet.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public clsExcelResult ExportToPDF(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (KhoGiay == "1")
            {
                DuongDanFile = sFilePath_A3;
            }
            else
            {
                DuongDanFile = sFilePath_A4;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";

            if (KhoGiay == "1")
            {
                DuongDanFile = sFilePath_A3;
            }
            else
            {
                DuongDanFile = sFilePath_A4;
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String ThangLamViec , String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay)
        {
            
                DataTable data = KeToan_SoCaiChiTiet(NamLamViec, ThangLamViec, iID_MaTaiKhoan);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                DataTable LuyKe = KeToan_SoCaiChiTiet_LuyKe(NamLamViec, ThangLamViec, iID_MaTaiKhoan);
                LuyKe.TableName = "LuyKe";
                fr.AddTable("LuyKe", LuyKe);

                DataTable LuyKe0 = KeToan_SoCaiChiTiet_LuyKe0(NamLamViec, ThangLamViec, iID_MaTaiKhoan);
                LuyKe0.TableName = "LuyKe0";
                fr.AddTable("LuyKe0", LuyKe0);
        }
        public DataTable KeToan_SoCaiChiTiet(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan)
        {
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";         
            String DK_DonViHaVing = "";
            const String LengthTaiKhoanCap1 = "3";
            const String LengthTaiKhoanCap2 = "4";
            const String LengthTaiKhoanCap3 = "5";
            const String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "3";     
           
            for (int i = 0; i < arrTaiKhoan.Length;i++ )
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
                DK_DonViSelect +=",SUM(TaiKhoan" + i + "_No) AS TaiKhoan" + i + "_No, SUM(TaiKhoan" + i + "_Co) AS TaiKhoan" + i + "_Co";
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan" + i + "_No)>0 OR SUM(TaiKhoan" + i + "_Co)>0";
            }
            DK_DonViSelect = DK_DonViSelect.Substring(1);
            DK_DonViCASE = DK_DonViCASE.Substring(1);           
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
            String SQL = String.Format(@"SELECT SoGS,NgayGS,iNgayCT,CONVERT (varchar,iNgayCT)+'-'+CONVERT(varchar,iThangCT) as  NgayCT,sSoChungTuChiTiet,iID_MaDonVi_No,iID_MaDonVi_Co,sNoiDung, {0}
                                        FROM(
                                        SELECT  iiD_MaChungTu, sSoChungTuChiTiet,iNgayCT,iThangCT,iID_MaDonVi_No,iID_MaDonVi_Co,sNoiDung,
                                        {1}
                                        FROM KT_ChungTuChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND iThangCT=@ThangLamViec AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY iiD_MaChungTu,iNgayCT,iThangCT,iID_MaDonVi_No,sNoiDung,iID_MaTaiKhoan_No,iID_MaDonVi_Co,iID_MaTaiKhoan_Co,sSoChungTuChiTiet
                                        ) AS A
                                        INNER JOIN (SELECT iiD_MaChungTu,sSoChungTu as SoGS, CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as NgayGS FROM KT_ChungTu) AS B
                                        ON a.iID_MaChungTu=b.iID_MaChungTu
                                        GROUP BY SoGS,NgayGS,iNgayCT,iThangCT,iID_MaDonVi_No,sNoiDung,iID_MaDonVi_Co,sSoChungTuChiTiet HAVING {2} ORDER BY iNgayCT,SoGS", DK_DonViSelect, DK_DonViCASE, DK_DonViHaVing);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec",NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec",ThangLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable KeToan_SoCaiChiTiet_LuyKe(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan)
        {
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            const String LengthTaiKhoanCap1 = "3";
            const String LengthTaiKhoanCap2 = "4";
            const String LengthTaiKhoanCap3 = "5";
            const String LengthTaiKhoanCap4 = "6";
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
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
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
                                        INNER JOIN (SELECT iiD_MaChungTu,sSoChungTu as SoGS, CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as NgayGS FROM KT_ChungTu) AS B
                                        ON a.iID_MaChungTu=b.iID_MaChungTu
                                        HAVING {2}", DK_DonViSelect, DK_DonViCASE, DK_DonViHaVing);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable KeToan_SoCaiChiTiet_LuyKe0(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan)
        {
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE = "";
            String DK_DonViHaVing = "";
            const String LengthTaiKhoanCap1 = "3";
            const String LengthTaiKhoanCap2 = "4";
            const String LengthTaiKhoanCap3 = "5";
            const String LengthTaiKhoanCap4 = "6";
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
                DK_DonViCASE += ",TaiKhoan" + i + "_No= CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END, TaiKhoan" + i + "_Co= CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@iID_MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
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
                                        INNER JOIN (SELECT iiD_MaChungTu,sSoChungTu as SoGS, CONVERT (varchar,iNgay)+'-'+CONVERT(varchar,iThang) as NgayGS FROM KT_ChungTu) AS B
                                        ON a.iID_MaChungTu=b.iID_MaChungTu
                                        HAVING {2}", DK_DonViSelect, DK_DonViCASE, DK_DonViHaVing);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public String obj_data(String ParentID, String iID_MaPhuongAn, String iID_MaTaiKhoan)
        {
            DataTable dt = KeToan_ToIn(iID_MaPhuongAn);       
            SelectOptionList slTaiKhoan = new SelectOptionList(dt, "MaTo", "TenTo");
            String s = MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"");
            return s;
        }
        [HttpGet]
        public JsonResult ds_NhomDonVi(String ParentID, String iID_MaPhuongAn, String iID_MaTaiKhoan)
        {
            return Json(obj_data(ParentID, iID_MaPhuongAn, iID_MaTaiKhoan), JsonRequestBehavior.AllowGet);
        }
        public static DataTable KeToan_ToIn(String iID_MaPhuongAn)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("MaTo", typeof(String));
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("ToSo", typeof(String));
            String[] iID_MaTaiKhoan = new String[100];
            String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
            int a = arrPhuongAn.Length % 4;
            if (a == 1)
            {
                iID_MaPhuongAn = iID_MaPhuongAn + ",DP,DP,DP";
                arrPhuongAn = iID_MaPhuongAn.Split(',');
            }
            if (a == 2)
            {
                iID_MaPhuongAn = iID_MaPhuongAn + ",DP,DP";
                arrPhuongAn = iID_MaPhuongAn.Split(',');
            }
            if (a == 3)
            {
                iID_MaPhuongAn = iID_MaPhuongAn + ",DP";
                arrPhuongAn = iID_MaPhuongAn.Split(',');
            }
            for (int i = 0; i < arrPhuongAn.Length; i = i + 4)
            {
                iID_MaTaiKhoan[Convert.ToInt16(i / 4)] += arrPhuongAn[i] + "," + arrPhuongAn[i + 1] + "," + arrPhuongAn[i + 2]+","+arrPhuongAn[i+3];
            }
            for (int i = 0; i < (arrPhuongAn.Length / 4); i++)
            {
                DataRow R1 = dt.NewRow();
                dt.Rows.Add(R1);
                R1[0] = iID_MaTaiKhoan[i];
                R1[1] = "Tờ" + Convert.ToInt16(i + 1) + ": " + iID_MaTaiKhoan[i];
                R1[2] = "Tờ " + Convert.ToInt16(i + 1);
            }
            dt.Dispose();
            return dt;
        }
        
        
    }
}
