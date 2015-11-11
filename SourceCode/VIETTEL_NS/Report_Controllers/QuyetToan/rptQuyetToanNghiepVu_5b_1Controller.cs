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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToanNghiepVu_5b_1Controller : Controller
    {
        //
        // GET: /rptQuyetToanNghiepVu_5b_1/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToanNghiepVu_5b_1.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToanNghiepVu_5b_2.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToanNghiepVu_5b_1.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {

            String Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThangQuy"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToanNghiepVu_5b_1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
      
        public ExcelFile CreateReport(String path, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String LoaiIn,String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
           
           
           
            String tendv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi) == false  && iID_MaDonVi !="-111")
            {
                tendv = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen").ToString();
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToanNghiepVu_5b_1");
            LoadData(fr, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,LoaiIn,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang_Quy", Thang_Quy);
            fr.SetValue("Tien", CommonFunction.TienRaChu(tong).ToString());
            fr.SetValue("TenDV", tendv);
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        public long tong = 0;
        private void LoadData(FlexCelReport fr, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet , String LoaiIn,String MaND)
        {
            DataTable data = QuyetToanNghiepVu_5b_1(Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, LoaiIn, MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLNS;
            dtLNS = HamChung.SelectDistinct("LNS", dtMuc, "sLNS,sL,sK", "sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);
            if (LoaiIn == "TongHop")
            {
                int a = dtLNS.Rows.Count;
                if (a < 16 && a >= 0)
                {
                    for (int i = 0; i < 16 - a; i++)
                    {
                        DataRow r = dtLNS.NewRow();
                        dtLNS.Rows.InsertAt(r, a + 1);
                    }
                }
            }
            dtLNS.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
    
        public clsExcelResult ExportToExcel(String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String LoaiIn,String MaND)
        {
            String DuongDan = "";
            if(LoaiIn=="ChiTiet") DuongDan=sFilePath;
            else DuongDan=sFilePath1;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, LoaiIn, MaND);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTriQuyetToan5b_1.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
  
        public ActionResult ViewPDF(String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String LoaiIn,String MaND)
        {
            String DuongDan = "";
            if (LoaiIn == "ChiTiet") DuongDan = sFilePath;
            else DuongDan = sFilePath1;
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, LoaiIn, MaND);
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
        public  DataTable QuyetToanNghiepVu_5b_1(String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String LoaiIn,String MaND)
        {
            DataTable dt=new DataTable();
            SqlCommand cmd = new SqlCommand();

            //DK qúy 1 quý = 3 tháng
            String DKThang_Quy = "";
            switch (Thang_Quy)
            {
                case "1": DKThang_Quy = "iThang_Quy IN(1,2,3)";
                    break;
                case "2": DKThang_Quy = "iThang_Quy IN(4,5,6)";
                    break;
                case "3": DKThang_Quy = "iThang_Quy IN(7,8,9)";
                    break;
                case "4": DKThang_Quy = "iThang_Quy IN(10,11,12)";
                    break;
                default: DKThang_Quy = "iThang_Quy=-1";
                    break;
            }
            // DK LNS
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length;i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (!String.IsNullOrEmpty(DKLNS))
            {
                DKLNS = " AND (" + DKLNS + ")";
            }
            //DK Don vi
            String DKDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            //// IN tổng hợp các đơn  vị đã chọn
            //if (LoaiIn == "TongHop")
            //{

            //    for (int i = 0; i < arrDonVi.Length; i++)
            //    {
            //        DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
            //        if (i < arrDonVi.Length - 1)
            //            DKDonVi += " OR ";
            //        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            //    }
            //    if (!String.IsNullOrEmpty(DKDonVi))
            //    {
            //        DKDonVi += " AND(" + DKDonVi + ")";
            //    }
            //}
            ////Chi tiết từng đơn vị
            //else
            //{
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            //}
            // DK trạng thái duyệt
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = "";
            }
            // truy vẫn SQL
            String SQL = String.Format(@"SELECT sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi+rHienVat) as rSoTien
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND sNG<>'' {0} {1} {2} {3}  AND {4}
                                        GROUP BY sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa",ReportModels.DieuKien_NganSach(MaND),DKDuyet,DKLNS,DKDonVi,DKThang_Quy);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tong += long.Parse(dt.Rows[i]["rSoTien"].ToString());
            }
            cmd.Dispose();
           
           
                return dt;
        }
        public static DataTable DanhSachDonVi(String Thang_Quy, String sLNS, String iID_MaTrangThaiDuyet, String MaND)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            //DK qúy 1 quý = 3 tháng
            String DKThang_Quy = "";
            switch (Thang_Quy)
            {
                case "1": DKThang_Quy = "iThang_Quy IN(1,2,3)";
                    break;
                case "2": DKThang_Quy = "iThang_Quy IN(4,5,6)";
                    break;
                case "3": DKThang_Quy = "iThang_Quy IN(7,8,9)";
                    break;
                case "4": DKThang_Quy = "iThang_Quy IN(10,11,12)";
                    break;
                default: DKThang_Quy = "iThang_Quy=-1";
                    break;
            }
            // DK LNS
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (!String.IsNullOrEmpty(DKLNS))
            {
                DKLNS = " AND (" + DKLNS + ")";
            }
            // DK trạng thái duyệt
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = "";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            }
            dtCauHinh.Dispose();
            String SQL = "";
            SQL = String.Format(@"SELECT a.iID_MaDonVi,a.iID_MaDonVi+' - '+ sTen as sTen
                                  FROM (SELECT DISTINCT iID_MaDonVi
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {0} AND {1} {2} {3}) as a
                                  INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as b
                                   ON a.iID_MaDonVi=b.MaDonVi", DKLNS, DKThang_Quy, DKDuyet, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable DanhSachLNS(String iID_MaPhongBan)
        {
            DataTable dt = new DataTable();
            String SQL = "";

            SQL = @"SELECT A.sLNS as sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7  AND A.sLNS <> '1010000' AND A.sL = '' AND (SUBSTRING(A.sLNS,1,1)<>1 OR A.sLNS IN(1090100,1090200,1090300)) ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public JsonResult Ds_DonVi(String ParentID, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            DataTable dt = DanhSachDonVi(Thang_Quy, sLNS, iID_MaTrangThaiDuyet, MaND);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, "rptQuyetToanNghiepVu_5b_1");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
    }
}
