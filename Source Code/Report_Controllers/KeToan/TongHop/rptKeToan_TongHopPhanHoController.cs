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
    public class rptKeToan_TongHopPhanHoController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathTongHop_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_TongHopPhanHo_TongHop_A3.xls";
        private const String sFilePathTongHop_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_TongHopPhanHo_TongHop_A4.xls";
        private const String sFilePathChiTiet_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_TongHopPhanHo_ChiTiet_A3.xls";
         private const String sFilePathChiTiet_A4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_TongHopPhanHo_ChiTiet_A4.xls";
        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_TongHopPhanHo.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        /// <summary>
        /// Ham nhan cac gia tri khi an nut submit ben VIEW
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec"]);
            String ThangLamViec = Convert.ToString(Request.Form[ParentID + "_ThangLamViec"]);
            String KieuIn=Convert.ToString(Request.Form[ParentID+"_KieuIn"]);
            String iID_MaPhuongAn = Convert.ToString(Request.Form[ParentID + "_iID_MaPhuongAn"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangLamViec"] = ThangLamViec;
            ViewData["KieuIn"] = KieuIn;
            ViewData["iID_MaPhuongAn"] = iID_MaPhuongAn;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_TongHopPhanHo.aspx";
            return View(sViewPath + "ReportView.aspx");  
            //  return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, KieuIn = KieuIn, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay});
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="KieuIn"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String KieuIn , String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay,String iID_MaTrangThaiDuyet)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(path);          
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();          
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_TongHopPhanHo");
            LoadData(fr, NamLamViec, ThangLamViec, KieuIn, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay, iID_MaTrangThaiDuyet);
            
            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam = "Tháng " + ThangLamViec + " Năm " + NamLamViec;

            String MaTK = "", TenTK = "";
            String[] arrMaTK = new String[3];
            String[] arrTenTK = new String[3];
            String ToSo = "";
            if (KieuIn == "2")
            {
                
                MaTK = iID_MaTaiKhoan;
                TenTK = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", iID_MaTaiKhoan, "sTen"));
            }
            else
            {

                DataTable dtTo = KeToan_ToIn(iID_MaPhuongAn, KieuIn, iID_MaTrangThaiDuyet);
                for (int i = 0; i < dtTo.Rows.Count;i++ )
                {
                    if (iID_MaTaiKhoan == dtTo.Rows[i]["MaTo"].ToString())
                    {
                        ToSo = dtTo.Rows[i]["ToSo"].ToString();
                    }
                }
                String[] arrMaTaiKhoan = iID_MaTaiKhoan.Split(',');            
                for (int i = 0; i < arrMaTaiKhoan.Length;i++ )
                {
                    arrMaTK[i] = arrMaTaiKhoan[i];
                    arrTenTK[i] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", arrMaTaiKhoan[i], "sTen"));
                }
            }
           
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangNam", ThangNam);
            fr.SetValue("MaTK", MaTK);
            fr.SetValue("TenTK", TenTK);
            fr.SetValue("MaTK0", arrMaTK[0]);
            fr.SetValue("MaTK1", arrMaTK[1]);
            fr.SetValue("MaTK2", arrMaTK[2]);
            fr.SetValue("TenTK0", arrTenTK[0]);
            fr.SetValue("TenTK1", arrTenTK[1]);
            fr.SetValue("TenTK2", arrTenTK[2]);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="KieuIn"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String KieuIn, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay,String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (KieuIn == "1")
            {
                if (KhoGiay == "1")
                {
                    DuongDanFile = sFilePathTongHop_A3;
                }
                else
                {
                    DuongDanFile = sFilePathTongHop_A4;
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    DuongDanFile = sFilePathChiTiet_A3;
                }
                else
                {
                    DuongDanFile = sFilePathChiTiet_A4;
                }
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, KieuIn, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay,iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_TongHopPhanHo.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        
      
        /// <summary>
        /// hàm xem báo cáo dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="KieuIn"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String KieuIn, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay,String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (KieuIn == "1")
            {
                if (KhoGiay == "1")
                {
                    DuongDanFile=sFilePathTongHop_A3;
                }
                else
                {
                    DuongDanFile = sFilePathTongHop_A4;
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    DuongDanFile=sFilePathChiTiet_A3;
                }
                else
                {
                    DuongDanFile = sFilePathChiTiet_A4;
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, KieuIn, iID_MaPhuongAn, iID_MaTaiKhoan, KhoGiay,iID_MaTrangThaiDuyet);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String ThangLamViec, String KieuIn, String iID_MaPhuongAn, String iID_MaTaiKhoan, String KhoGiay,String iID_MaTrangThaiDuyet)
        {
            if (KieuIn == "2")
            {
                DataTable data = KeToan_TongHopPhanHo_ChiTiet(NamLamViec, ThangLamViec, iID_MaTaiKhoan, iID_MaTrangThaiDuyet);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
            else
            {
                DataTable data = KeToan_TongHopPhanHo_TongHop(NamLamViec, ThangLamViec, iID_MaTaiKhoan, iID_MaTrangThaiDuyet);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);

                DataTable LuyKe = KeToan_TongHopPhanHo_TongHop_LuyKe(NamLamViec, ThangLamViec, iID_MaTaiKhoan, iID_MaTrangThaiDuyet);
                data.TableName = "LuyKe";
                fr.AddTable("LuyKe", LuyKe);

                DataTable LuyKe0 = KeToan_TongHopPhanHo_TongHop_LuyKe0(NamLamViec, ThangLamViec, iID_MaTaiKhoan, iID_MaTrangThaiDuyet);
                data.TableName = "LuyKe0";
                fr.AddTable("LuyKe0", LuyKe0);
            }

        }
      public  class KeToan_TongHopPhanHoData
        {
            public String Text { get; set; }
            public String NoiDung { get; set; }
        }
        /// <summary>
        /// hàm jSON khi click onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaPhuongAn"></param>
        /// <param name="KieuIn"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
      public KeToan_TongHopPhanHoData obj_data(String ParentID, String iID_MaPhuongAn, String KieuIn, String iID_MaTaiKhoan, String iID_MaTrangThaiDuyet)
        {
            KeToan_TongHopPhanHoData _data = new KeToan_TongHopPhanHoData();
            DataTable dt = KeToan_ToIn(iID_MaPhuongAn, KieuIn, iID_MaTrangThaiDuyet);
            if (KieuIn == "1")
            {
                _data.Text = "Chọn tờ cần in: ";             
            }
            else
            {
                _data.Text = "Chọn tài khoản: ";
            }
            SelectOptionList slTaiKhoan = new SelectOptionList(dt, "MaTo", "TenTo");
            _data.NoiDung = MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 180px; min-width:120px;\"");
            return _data;
        }
        [HttpGet]
      public JsonResult ds_NhomDonVi(String ParentID, String iID_MaPhuongAn, String KieuIn, String iID_MaTaiKhoan, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_data(ParentID, iID_MaPhuongAn, KieuIn, iID_MaTaiKhoan, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// data khi chọn tổng hợp
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable KeToan_TongHopPhanHo_TongHop(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String iID_MaTrangThaiDuyet)
        {
          
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE_No = "";
            String DK_DonViCASE_Co = "";
            String DK_DonViHaVing = "";
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "";
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
                DK_DonViCASE_No += ",TaiKhoan" + i + "_No= CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END,TaiKhoan" + i + "_Co=0";
                DK_DonViCASE_Co += ",TaiKhoan" + i + "_No=0,TaiKhoan" + i + "_Co= CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan" + i + "_No)>0 OR SUM(TaiKhoan" + i + "_Co)>0";
            }
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"SELECT DONVI {0}
                                          FROM(
                                                SELECT sTenDonVi_No as DonVi {1}
                                                FROM KT_ChungTuChiTiet
                                                WHERE   iThangCT=@ThangLamViec AND iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaDonVi_No<>'' {4}
                                                GROUP BY sTenDonVi_No,iID_MaTaiKhoan_No
                                                UNION
                                                SELECT sTenDonVi_Co {2}
                                                FROM KT_ChungTuChiTiet
                                                WHERE   iThangCT=@ThangLamViec AND iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaDonVi_Co<>'' {4}
                                                GROUP BY sTenDonVi_Co,iID_MaTaiKhoan_Co) AS a
                                        GROUP BY DonVi
                                        HAVING  {3}", DK_DonViSelect, DK_DonViCASE_No, DK_DonViCASE_Co, DK_DonViHaVing,DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary> 
        /// Tổng tiền lũy kế
        /// </summary>                      
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable KeToan_TongHopPhanHo_TongHop_LuyKe(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String iID_MaTrangThaiDuyet)
        {
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE_No = "";
            String DK_DonViCASE_Co = "";
            String DK_DonViHaVing = "";
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "";
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
                DK_DonViCASE_No += ",TaiKhoan" + i + "_No= CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END,TaiKhoan" + i + "_Co=0";
                DK_DonViCASE_Co += ",TaiKhoan" + i + "_No=0,TaiKhoan" + i + "_Co= CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan"+i+"_No)>0 OR SUM(TaiKhoan"+i+"_Co)>0";            
            }
            DK_DonViSelect = DK_DonViSelect.Substring(1);
            DK_DonViCASE_No = DK_DonViCASE_No.Substring(1);
            DK_DonViCASE_Co = DK_DonViCASE_Co.Substring(1);
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
             String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"SELECT  {0}
                                          FROM(
                                                SELECT    {1}
                                                FROM KT_ChungTuChiTiet
                                                WHERE   iThangCT<=@ThangLamViec AND iThangCT<>0 AND iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaDonVi_No<>'' {4}
                                                GROUP BY iID_MaTaiKhoan_No
                                                UNION
                                                SELECT  {2}
                                                FROM KT_ChungTuChiTiet
                                                WHERE  iThangCT<=@ThangLamViec AND iThangCT<>0 AND iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaDonVi_Co<>'' {4}
                                                GROUP BY iID_MaTaiKhoan_Co) AS a
                                                HAVING {3}", DK_DonViSelect, DK_DonViCASE_No, DK_DonViCASE_Co, DK_DonViHaVing,DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
     /// <summary>
        /// Tổng tiền lũy kế cả tháng 0
     /// </summary>
     /// <param name="NamLamViec"></param>
     /// <param name="ThangLamViec"></param>
     /// <param name="iID_MaTaiKhoan"></param>
     /// <returns></returns>
        public static DataTable KeToan_TongHopPhanHo_TongHop_LuyKe0(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String iID_MaTrangThaiDuyet)
        {
            String LengthTaiKhoanCap1 = "3";
            String LengthTaiKhoanCap2 = "4";
            String LengthTaiKhoanCap3 = "5";
            String LengthTaiKhoanCap4 = "6";
            String LengthTaiKhoan = "";
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //set các điều kiện
            String DK_DonViSelect = "";
            String DK_DonViCASE_No = "";
            String DK_DonViCASE_Co = "";
            String DK_DonViHaVing = "";

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
                DK_DonViCASE_No += ",TaiKhoan" + i + "_No= CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END,TaiKhoan" + i + "_Co=0";
                DK_DonViCASE_Co += ",TaiKhoan" + i + "_No=0,TaiKhoan" + i + "_Co= CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1," + LengthTaiKhoan + ")=@MaTaiKhoan" + i + " THEN SUM(rSoTien) ELSE 0 END";
                DK_DonViHaVing += "OR SUM(TaiKhoan" + i + "_No)>0 OR SUM(TaiKhoan" + i + "_Co)>0";
            }
            DK_DonViSelect = DK_DonViSelect.Substring(1);
            DK_DonViCASE_No = DK_DonViCASE_No.Substring(1);
            DK_DonViCASE_Co = DK_DonViCASE_Co.Substring(1);
            DK_DonViHaVing = DK_DonViHaVing.Substring(2);
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"SELECT  {0}
                                          FROM(
                                                SELECT    {1}
                                                FROM KT_ChungTuChiTiet
                                                WHERE   iThangCT<=@ThangLamViec AND iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaDonVi_No<>'' {4}
                                                GROUP BY iID_MaTaiKhoan_No
                                                UNION
                                                SELECT  {2}
                                                FROM KT_ChungTuChiTiet
                                                WHERE  iThangCT<=@ThangLamViec AND iNamLamViec=@NamLamViec AND iTrangThai=1 AND iID_MaDonVi_Co<>'' {4}
                                                GROUP BY iID_MaTaiKhoan_Co) AS a
                                                HAVING {3}", DK_DonViSelect, DK_DonViCASE_No, DK_DonViCASE_Co, DK_DonViHaVing, DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Data khi chọn chi tiết
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable KeToan_TongHopPhanHo_ChiTiet(String NamLamViec, String ThangLamViec, String iID_MaTaiKhoan, String iID_MaTrangThaiDuyet)
        {
            String LengthTaiKhoan = iID_MaTaiKhoan.Length.ToString();
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"SELECT DonVi,SUM(TrongKy_No) as TrongKy_No,SUM(TrongKy_Co) as TrongKy_Co,SUM(LuyKe_No) as LuyKe_No,SUM(LuyKe_Co) as LuyKe_Co,SUM(LuyKe0_No) as LuyKe0_No,SUM(LuyKe0_Co) as LuyKe0_Co
                                        FROM(
                                        SELECT sTenDonVi_No as DonVi,
                                        TrongKy_No=CASE WHEN iThangCT=@ThangLamViec THEN SUM(rSoTien) ELSE 0 END,
                                        TrongKy_Co=0,
                                        LuyKe_No=CASE WHEN (iThangCT<=@ThangLamViec AND iThangCT!=0) THEN SUM(rSoTien) ELSE 0 END,
                                        LuyKe_Co=0,
                                        LuyKe0_No=CASE WHEN iThangCT<=@ThangLamViec THEN SUM(rSoTien) ELSE 0 END,
                                        LuyKe0_Co=0
                                        FROM KT_ChungTuChiTiet
                                        WHERE   1=1 AND iNamLamViec=@NamLamViec AND SUBSTRING(iID_MaTaiKhoan_No,1,{0})=@iID_MaTaiKhoan AND iTrangThai=1 AND iID_MaDonVi_No<>'' {1}
                                        GROUP BY sTenDonVi_No,iThangCT
                                        UNION 
                                        SELECT sTenDonVi_Co,
                                        TrongKy_No=0,
                                        TrongKy_Co=CASE WHEN iThangCT=@ThangLamViec THEN SUM(rSoTien) ELSE 0 END,
                                        LuyKe_No=0,
                                        LuyKe_Co=CASE WHEN (iThangCT<=@ThangLamViec AND iThangCT!=0) THEN SUM(rSoTien) ELSE 0 END,
                                        LuyKe0_No=0,
                                        LuyKe0_Co=CASE WHEN (iThangCT<=@ThangLamViec) THEN SUM(rSoTien) ELSE 0 END
                                        FROM KT_ChungTuChiTiet
                                        WHERE iNamLamViec=@NamLamViec AND  SUBSTRING(iID_MaTaiKhoan_Co,1,{0})=@iID_MaTaiKhoan AND iID_MaDonVi_Co<>'' AND iTrangThai=1 {1}
                                        GROUP BY sTenDonVi_Co,iThangCT) as a
                                        GROUP BY DonVi
                                        HAVING   SUM(TrongKy_No)>0 OR SUM(TrongKy_Co)>0 OR SUM(LuyKe_No)>0 OR SUM(LuyKe_Co)>0 OR SUM(LuyKe0_No)>0 OR SUM(LuyKe0_Co)>0", LengthTaiKhoan, DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@ThangLamViec", ThangLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hiện thị danh sách các tờ in
        /// </summary>
        /// <param name="iID_MaPhuongAn"> phương án chọn</param>
        /// <param name="KieuIn"> kiểu in 1. Tổng hợp 2, Chi Tiết</param>
        /// <returns></returns>
        public static DataTable KeToan_ToIn(String iID_MaPhuongAn, String KieuIn, String iID_MaTrangThaiDuyet)
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("MaTo", typeof(String));
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("ToSo", typeof(String));
            if (KieuIn == "1")
            {   
                String[] iID_MaTaiKhoan = new String[100]; 
                String[] arrPhuongAn = iID_MaPhuongAn.Split(',');
                int a = arrPhuongAn.Length % 3;
                if (a == 1)
                {
                    iID_MaPhuongAn = iID_MaPhuongAn + ",,";
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                }
                if (a == 2)
                {
                    iID_MaPhuongAn = iID_MaPhuongAn + ",";
                    arrPhuongAn = iID_MaPhuongAn.Split(',');
                }
                for (int i = 0; i < arrPhuongAn.Length; i = i + 3)
                {
                    iID_MaTaiKhoan[Convert.ToInt16(i / 3)] += arrPhuongAn[i] + "," + arrPhuongAn[i + 1] + "," + arrPhuongAn[i + 2];
                }                   
                for (int i = 0; i < (arrPhuongAn.Length / 3); i++)
                {
                    DataRow R1 = dt.NewRow();
                    dt.Rows.Add(R1);
                    R1[0] = iID_MaTaiKhoan[i];
                    R1[1] = "Tờ" + Convert.ToInt16(i + 1) + ": " + iID_MaTaiKhoan[i];
                    R1[2] = "Tờ " + Convert.ToInt16(i + 1);
                }
                dt.Dispose();
            }
            else
            {
                String[] iID_MaTaiKhoan = iID_MaPhuongAn.Split(',');
                String[] TenTaiKhoan = new String[100];
                for (int i = 0; i < iID_MaTaiKhoan.Length;i++)
                {
                    TenTaiKhoan[i] = Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", iID_MaTaiKhoan[i], "sTen"));
                }
                for (int i = 0; i < iID_MaTaiKhoan.Length;i++)
                {
                    DataRow R1 = dt.NewRow();
                    dt.Rows.Add(R1);
                    R1[0] = iID_MaTaiKhoan[i];
                    R1[1] = iID_MaTaiKhoan[i] + "    " + TenTaiKhoan[i];
                }
            }
            return dt;
        }

        /// <summary>
        /// Chọn kiểu in
        /// </summary>
        /// <returns></returns>
        public static DataTable KieuIn()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaKieuIn", typeof(String));
            dt.Columns.Add("TenKieuIn", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "Tất cả các tài khoản";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "Từng tài khoản";
            return dt;
        }       
    }
}
