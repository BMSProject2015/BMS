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
using System.Text;

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTH_TongHopCapVonController : Controller
    {
        //
        // GET: /rptKTTH_TongHopCapVon/
        //public string sViewPath = "~/Report_Views/";
        //private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_TongHopCapVon.xls";
        //public static String NameFile = "";
        //public ActionResult Index()
        //{
        //      ViewData["PageLoad"] = 0;
        //    ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_TongHopCapVon.aspx";
        //    return View(sViewPath + "ReportView.aspx");
        //}     
          
        //public ActionResult EditSubmit(String ParentID)
        //{
        //    String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
        //    String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
        //    String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
        //    String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);
        //    String DVT = Convert.ToString(Request.Form[ParentID + "_DVT"]);
        //    String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
        //    ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_TongHopCapVon.aspx";
        //    ViewData["iThang"] = iThang;
        //    ViewData["iNgay"] = iNgay;;
        //    ViewData["iNamLamViec"] = iNamLamViec;
        //    ViewData["DVT"] = DVT;
        //    ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
        //    ViewData["PageLoad"] = 1;
        //    return View(sViewPath + "ReportView.aspx");
        //}
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_TongHopCapVon.xls";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_TongHopCapVon_A3.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
             {
                 HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_TongHopCapVon.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
           String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);
            String DVT = Convert.ToString(Request.Form[ParentID + "_DVT"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_TongHopCapVon.aspx";
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;           
            ViewData["iNgay"] = iNgay;
            ViewData["iNamLamViec"] = iNamLamViec;            
            ViewData["DVT"] = DVT;
            ViewData["PageLoad"] = 1;
            ViewData["KhoGiay"] = KhoGiay;
            return View(sViewPath + "ReportView.aspx");
            // return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iNgay1 = iNgay1, iNgay2 = iNgay2, LoaiBaoCao = LoaiBaoCao });
        }

        public ExcelFile CreateReport(String path, String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTH_TongHopCapVon");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet);
            if (DVT == "0")
            {
                fr.SetValue("DVT","Đồng");
            }
            else if (DVT == "1")
            {
                fr.SetValue("DVT", "Nghìn đồng");
            }
            else
            {
                fr.SetValue("DVT", "Triệu đồng");
            }

            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("iThang", iThang);
            fr.SetValue("iNgay", iNgay);
            fr.SetValue("ngay", ngay);
            fr.SetValue("LoaiBaoCao", DVT);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.Run(Result);
            return Result;

        }

        public clsExcelResult ExportToPDF( String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
          
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet);
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

        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            HamChung.Language();
            String fileName = "";
            if (KhoGiay == "1")
            {
                fileName = sFilePath_A3;
            }
            else
                fileName = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(fileName), iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHoCapVon.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public ActionResult ViewPDF(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            HamChung.Language();
            String fileName = "";
            if (KhoGiay == "1")
            {
                fileName = sFilePath_A3;
            }
            else
                fileName = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(fileName), iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet);
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

        private void LoadData(FlexCelReport fr, String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet)
        {
            DataTable data = TongHopCapVon(iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable dtPhongBan = HamChung.SelectDistinct("NS_PhongBan", data, "iID_MaPhongBan", "sTen", "", "");
            dtPhongBan.TableName = "PhongBan";
            fr.AddTable("PhongBan", dtPhongBan);
        }

        public DataTable TongHopCapVon(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            String DKDVT = "";
            if (DVT == "0")
            {
                DKDVT = "";
            }
            else if (DVT == "1")
            {
                DKDVT = "/1000 ";
            }
            else
            {
                DKDVT = "/1000000 ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                          LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(
                                              PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
          
            String SQL =
                String.Format(
                    @"SELECT bang.iID_MaPhongBan_No as iID_MaPhongBan,bang.sTenPhongBan_No as sTen, 
  GoiDau=CASE WHEN (SUM(GoiDau_No)-SUM(GoiDau_Co)>=0) THEN (SUM(GoiDau_No)-SUM(GoiDau_Co)){1} ELSE (SUM(GoiDau_No)-SUM(GoiDau_Co))*-1{1} END,
  Tien=CASE WHEN (SUM(Tien_No)-SUM(Tien_Co)>=0) THEN (SUM(Tien_No)-SUM(Tien_Co)){1} ELSE (SUM(Tien_No)-SUM(Tien_Co))*-1{1} END,
  TGSX=CASE WHEN (SUM(TGSX_No)-SUM(TGSX_Co)>=0) THEN (SUM(TGSX_No)-SUM(TGSX_Co)){1} ELSE (SUM(TGSX_No)-SUM(TGSX_Co))*-1{1} END,
  VonKhac=CASE WHEN (SUM(VonKhac_No)-SUM(VonKhac_Co)>=0) THEN (SUM(VonKhac_No)-SUM(VonKhac_Co)){1} ELSE (SUM(VonKhac_No)-SUM(VonKhac_Co))*-1{1} END,
  BDam=CASE WHEN (SUM(BDam_No)-SUM(BDam_Co)>=0) THEN (SUM(BDam_No)-SUM(BDam_Co)){1} ELSE (SUM(BDam_No)-SUM(BDam_Co))*-1{1} END,
  HV=CASE WHEN (SUM(HV_No)-SUM(HV_Co)>=0) THEN (SUM(HV_No)-SUM(HV_Co)){1} ELSE (SUM(HV_No)-SUM(HV_Co))*-1{1} END
  FROM (
   SELECT iID_MaPhongBan_No,SUBSTRING(sTenPhongBan_No,5,1000) as sTenPhongBan_No, 
   GoiDau_No = CASE WHEN iID_MaTaiKhoan_No = 3441 THEN SUM(rsoTien) ELSE 0 END, 
   GoiDau_Co=0,
   Tien_No = CASE WHEN iID_MaTaiKhoan_No = 34421 THEN SUM(rsoTien) ELSE 0 END,
   Tien_Co=0,
   TGSX_No = CASE WHEN iID_MaTaiKhoan_No = 3443 THEN SUM(rsoTien) ELSE 0 END, 
   TGSX_Co=0,
   VonKhac_No = CASE WHEN iID_MaTaiKhoan_No = 3448 THEN SUM(rsoTien) ELSE 0 END, 
   VonKhac_Co=0,
   BDam_No = CASE WHEN iID_MaTaiKhoan_No = 3471 THEN SUM(rsoTien) ELSE 0 END, 
   BDam_Co=0,
   HV_No = CASE WHEN iID_MaTaiKhoan_No = 3473 THEN SUM(rsoTien) ELSE 0 END,  
   HV_Co=0 FROM
   KT_ChungTuChiTiet   WHERE  iTrangThai = 1  {0}
   AND (iThangCT<@iThang OR (iThangCT=@iThang AND iNgayCT<=@iNgay)) and iNamLamViec=@iNamLamViec
   GROUP BY sTenPhongBan_No,iID_MaPhongBan_No,iID_MaTaiKhoan_No HAVING sum (rSoTien)!=0
   union
  
  SELECT iID_MaPhongBan_Co as iID_MaPhongBan_No,SUBSTRING(sTenPhongBan_Co,5,1000) as sTenPhongBan_No, 
  GoiDau_No=0,
  GoiDau_Co = CASE WHEN iID_MaTaiKhoan_Co = 3441 THEN SUM(rsoTien) ELSE 0 END, 
  Tien_No=0,
  Tien_Co = CASE WHEN iID_MaTaiKhoan_Co = 34421 THEN SUM(rsoTien) ELSE 0 END,
  TGSX_No=0,
  TGSX_Co = CASE WHEN iID_MaTaiKhoan_Co = 3443 THEN SUM(rsoTien) ELSE 0 END, 
  VonKhac_No=0,
  VonKhac_Co = CASE WHEN iID_MaTaiKhoan_Co = 3448 THEN SUM(rsoTien) ELSE 0 END, 
  BDam_No=0,
  BDam_Co = CASE WHEN iID_MaTaiKhoan_Co = 3471 THEN SUM(rsoTien) ELSE 0 END, 
  HV_No=0 ,
  HV_Co = CASE WHEN iID_MaTaiKhoan_Co = 3473 THEN SUM(rsoTien) ELSE 0 END 
  FROM   KT_ChungTuChiTiet   WHERE  iTrangThai = 1 {0} AND (iThangCT<@iThang OR (iThangCT=@iThang AND iNgayCT<=@iNgay)) and iNamLamViec=@iNamLamViec
  GROUP BY sTenPhongBan_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co HAVING sum (rSoTien)!=0
   )as bang 
  GROUP BY iID_MaPhongBan_No,sTenPhongBan_No     
  HAVING sum(GoiDau_No) !=0 or sum(GoiDau_Co) !=0 or sum(Tien_No) !=0 or sum(Tien_Co) !=0 or
  sum(TGSX_No) !=0 or sum(TGSX_Co) !=0 or sum(VonKhac_No) !=0 or sum(VonKhac_Co) !=0 or sum(BDam_No) !=0 or sum(BDam_Co) !=0 or sum(HV_No) !=0 or sum(HV_Co) !=0  order  by  iID_MaPhongBan ",
                    iID_MaTrangThaiDuyet, DKDVT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNgay", iNgay);
            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
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
       
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "0";
            R1[1] = "Đồng";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            R2[1] = "Nghìn đồng";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "2";
            R3[1] = "Triệu đồng";
            dt.Dispose();
            return dt;
        }
        public JsonResult Get_objNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        {
            return Json(get_sNgayThang(ParentID, MaND, iThang, iNgay), JsonRequestBehavior.AllowGet);
        }
        public String get_sNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        {
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            String S = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:80px;\"");
            dtNgay.Dispose();

            return S;
        }
       

    }
}
  