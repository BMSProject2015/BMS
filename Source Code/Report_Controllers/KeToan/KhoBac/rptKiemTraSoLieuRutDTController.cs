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

namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKiemTraSoLieuRutDTController : Controller
    {
        //
        // GET: /rptKTKhoBac_DoiChieuDuToan/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKiemTraSoLieuRutDT.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKiemTraSoLieuRutDT.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String Thang_Quy = "";
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKiemTraSoLieuRutDT.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
           // return RedirectToAction("Index", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            String TenLoaiThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                TenLoaiThangQuy = "Quý";
            }
            else
            {
                TenLoaiThangQuy = "Tháng";
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKiemTraSoLieuRutDT");
            LoadData(fr, NamLamViec, Thang_Quy, LoaiThang_Quy,iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("CucTaiChinh", CucTaiChinh);

            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;

        }

        public clsExcelResult ExportToPDF(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, Thang_Quy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, Thang_Quy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, Thang_Quy, LoaiThang_Quy,iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        private void LoadData(FlexCelReport fr, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable data = DoiChieuDuToan(NamLamViec, Thang_Quy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
           
            String TC = "";
            if (data.Rows.Count > 0)
            {
                if (Convert.ToString(data.Rows[0]["sThuChi"]) != " ")
                {
                    TC = "X";
                }
                else
                {
                    TC = " ";
                }
            }
            else TC = "";
            fr.SetValue("TC", TC);
            data.Dispose();
        }
        public DataTable DoiChieuDuToan(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
           
            SqlCommand cmd = new SqlCommand();
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = " iThangCT in (1,2,3) ";
                        break;
                    case 2: DKThang_Quy = " iThangCT in (4,5,6) ";
                        break;
                    case 3: DKThang_Quy = " iThangCT in (7,8,9) ";
                        break;
                    case 4: DKThang_Quy = " iThangCT in (10,11,12) ";
                        break;
                }
            }
            else
            {
                DKThang_Quy = " iThangCT=@ThangQuy ";
               
            }
         
            String SQL = " SELECT sSoChungTuGhiSo,iNgay,iThang,sLNS,sL,sK,sM,substring(sTM,1,2) as sTM,sTTM,sLoaiST,sNoiDung,iID_MaDonVi_Nhan,iID_MaDonVi_Tra,rDTRut,sThuChi,rSoTien,rDTKhoiPhuc";
            SQL += " FROM KTKB_ChungTuChiTiet ";
            SQL += " WHERE iTrangThai = 1 {1} AND iNamLamViec = @NamLamViec AND {0} order by iNgay,iThang,sLNS,sL,sK,sM, sTM";
            SQL = String.Format(SQL, DKThang_Quy, iID_MaTrangThaiDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }
         
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
    }
}
