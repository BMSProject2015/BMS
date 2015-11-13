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


namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKTTienGui_UyNhiemChiController : Controller
    {
        //
        // GET: /rptKTTienGui_UyNhiemChi/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePathPT1 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_UyNhiemChi_KhoBac.xls";
        private const String sFilePathPT2 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_UyNhiemChi_NganHang.xls";//2lien tren a4
        private const String sFilePathPT3 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_GiayRutTienMat.xls";//2 lien tren a4
        private const String sFilePathPT4 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_GiayRutTienMat2so.xls";//2so tren a4
        private const String sFilePathPT5 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_GiayRutTienMatlien1.xls";// 1 lien tren a4
        private const String sFilePathPT6 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_UyNhiemChi_NganHang2so.xls";//2so tren a4
        private const String sFilePathPT7 = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_UyNhiemChi_NganHanglien1.xls";//1lien tren a4
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTienGui_UyNhiemChi.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID, String iNamLamViec, String iThang, int ChiSo)
        {
            String sSoChungTuChiTiet = Request.Form["sSoChungTuChiTiet"];
            String iID_MaChungTu = Request.Form[ParentID + "_iID_MaChungTu"];
            if (String.IsNullOrEmpty(sSoChungTuChiTiet))
                return RedirectToAction("Index", "KeToanChiTietTienGui");
            else
            {
                //String InTenMLNS = Request.Form[ParentID + "_InTenMLNS"];
                String[] arrMaChungTuChiTiet = sSoChungTuChiTiet.Split(',');
                String SoChungTuChiTiet = arrMaChungTuChiTiet[ChiSo];
                String LoaiBaoCao = Request.Form[ParentID + "_LoaiBaoCao"];
                String inmuc = Request.Form[ParentID + "_inmuc"];
                String iSoLien = Request.Form[ParentID + "_iSoLien_show"];
                if (String.IsNullOrEmpty(iSoLien))
                {
                    iSoLien = "1";
                }
                String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
                ViewData["sSoChungTuChiTiet"] = sSoChungTuChiTiet;
                ViewData["ChiSo"] = ChiSo;
                ViewData["iNamLamViec"] = iNamLamViec;
                ViewData["iThang"] = "";
                ViewData["iID_MaChungTu"] = iID_MaChungTu;
                ViewData["LoaiBaoCao"] = LoaiBaoCao;
                ViewData["inmuc"] = inmuc;
                ViewData["iSoLien"] = iSoLien;
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTienGui_UyNhiemChi.aspx";
                return View(sViewPath + "ReportView.aspx");
            }

        }
      
        public ExcelFile CreateReport(String path, String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc,int iSoLien=1)
        {
            String KeToan = "(Không có)";

            String SQL = @"SELECT sThamSo FROM KT_DanhMucThamSo
                         WHERE sKyHieu=201";
            String ThamSo = "";
            ThamSo = Connection.GetValueString(SQL, "");
            if (String.Equals(ThamSo.ToUpper(), "C"))
                KeToan = "";
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTienGui_UyNhiemChi");
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iNamLamViec, iThang, iID_MaChungTu, sSoChungTuChiTiet, LoaiBaoCao, inmuc,iSoLien);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("SoPT", CommonFunction.LayTruong("KTTG_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaChungTu, "sSoChungTuChiTiet"));
            fr.SetValue("Thang", iThang);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("Nams", DateTime.Now.Year);
            fr.SetValue("KeToan", KeToan);
            fr.SetValue("ThangCT", CommonFunction.LayTruong("KTTG_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaChungTu, "iThangCT"));
            fr.SetValue("NgayCT", CommonFunction.LayTruong("KTTG_ChungTuChiTiet", "sSoChungTuChiTiet", iID_MaChungTu, "iNgayCT"));
            fr.SetValue("DonVi", CommonFunction.LayTruong("KTTG_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaChungTu, "sTenDonVi_Nhan"));
            //fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("BoQuocPhong", BoQuocPhong);    
            fr.Run(Result);
            return Result;
        }
        public long tong = 0;

        public clsExcelResult ExportToPDF(String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc)
        {
            String DuongDan = "";
            if (LoaiBaoCao == "KB" && inmuc == "1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "2")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "3")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "1")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "2")
            {
                DuongDan = sFilePathPT7;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "3")
            {
                DuongDan = sFilePathPT6;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "1")
            {
                DuongDan = sFilePathPT3;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "2")
            {
                DuongDan = sFilePathPT5;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "3")
            {
                DuongDan = sFilePathPT4;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iID_MaChungTu, sSoChungTuChiTiet, LoaiBaoCao, inmuc);

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

        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc)
        {
            String DuongDan = "";

            if (LoaiBaoCao == "KB" && inmuc == "1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "2")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "3")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "1")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "2")
            {
                DuongDan = sFilePathPT7;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "3")
            {
                DuongDan = sFilePathPT6;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "1")
            {
                DuongDan = sFilePathPT3;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "2")
            {
                DuongDan = sFilePathPT5;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "3")
            {
                DuongDan = sFilePathPT4;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iID_MaChungTu, sSoChungTuChiTiet, LoaiBaoCao, inmuc);
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


        public ActionResult ViewPDF(string MaND, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            //String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            String DuongDan = "";
            if (LoaiBaoCao == "KB" && inmuc == "1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "2")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "3")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "1")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "2")
            {
                DuongDan = sFilePathPT7;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "3")
            {
                DuongDan = sFilePathPT6;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "1")
            {
                DuongDan = sFilePathPT3;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "2")
            {
                DuongDan = sFilePathPT5;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "3")
            {
                DuongDan = sFilePathPT4;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang, iID_MaChungTu, sSoChungTuChiTiet, LoaiBaoCao, inmuc);
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
        public ActionResult ViewPDF_Hiden(string MaND, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc,int iSoLien)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            //String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            String DuongDan = "";
            if (LoaiBaoCao == "KB" && inmuc == "1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "2")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "KB" && inmuc == "3")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "1")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "2")
            {
                DuongDan = sFilePathPT7;
            }
            else if (LoaiBaoCao == "NH" && inmuc == "3")
            {
                DuongDan = sFilePathPT6;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "1")
            {
                DuongDan = sFilePathPT3;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "2")
            {
                DuongDan = sFilePathPT5;
            }
            else if (LoaiBaoCao == "TM" && inmuc == "3")
            {
                DuongDan = sFilePathPT4;
            }
          

            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang, iID_MaChungTu, sSoChungTuChiTiet, LoaiBaoCao, inmuc, iSoLien);            
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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc,int iSoLien=1)
        {
            DataTable data = dtChiTiet(iNamLamViec, iThang, iID_MaChungTu, sSoChungTuChiTiet, inmuc,iSoLien);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();            

        }

        public DataTable dtChiTiet(String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet, String inmuc,int SoLien=1)
        {
            DataTable dt;
            SqlCommand cmd;
            cmd = new SqlCommand();
            String DK = " iTrangThai =1 AND iID_MaChungTu=@iID_MaChungTu ";
           // DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
            String SoChungTucChiTiet = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
            if (inmuc != "3")
            {
                DK += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
                cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTucChiTiet);
            }
            else
            {
                String[] arrSoCT = sSoChungTuChiTiet.Split(',');
                if (arrSoCT.Length > 0)
                    DK += " AND ( ";
                for(int i=0;i<arrSoCT.Length;i++)
                {
                    if (i > 0) DK += " OR ";
                    SoChungTucChiTiet = TachLaySoChungTu_DonViNhan(arrSoCT[i], 0);
                    DK += "sSoChungTuChiTiet=@sSoChungTuChiTiet"+i;
                    cmd.Parameters.AddWithValue("@sSoChungTuChiTiet" + i, SoChungTucChiTiet);
                }
                if (arrSoCT.Length > 0)
                    DK += ")";
            }
            String strSQL = "SELECT sSoChungTuChiTiet,iNgay=0,iThang=0,sTenDonVi_Nhan='',sMaSo_Nhan='',sDiaChi_Nhan='',sSoTaiKhoan_Nhan='',sKhoBac_Nhan=''";
            strSQL += ",sTenDonVi_Co='',sMaSo_Co='',sDiaChi_Co='',sSoTaiKhoan_Co='',sKhoBac_Co=''";
            strSQL += ",iID_MaNhanVien_ThuChi='',sTenNhanVien_ThuChi='',sChungMinhThu='',dNgayCap='',sNoiCap=''";
            strSQL += ",sNoiDung='',SUM(rSoTien) AS rSoTien,Tien=''";
            strSQL += " FROM KTTG_ChungTuChiTiet";
            strSQL += " WHERE {0} ";
            strSQL += " GROUP BY sSoChungTuChiTiet";
            strSQL = String.Format(strSQL,DK);
            cmd.CommandText = strSQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            dt = Connection.GetDataTable(cmd);
            //Lay thong tin don vi nhan
            strSQL = "SELECT CT.sSoChungTuChiTiet,CT.iNgay,CT.iThang,CT.sTenDonVi_Nhan,DVN.sDiaChi AS sDiaChi_Nhan, DVN.sMaSo AS sMaSo_Nhan, DVN.sSoTaiKhoan AS sSoTaiKhoan_Nhan, DVN.sKhoBac AS sKhoBac_Nhan,CT.sNoiDung,CT.dNgayTao";
            strSQL +=" FROM((SELECT * FROM KTTG_ChungTuChiTiet WHERE {0}) CT";
            strSQL += " LEFT JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DVN ON DVN.iID_MaDonVi = CT.iID_MaDonVi_Nhan)";
            strSQL += " ORDER By CT.dNgayTao";
            strSQL = String.Format(strSQL, DK);
            cmd.CommandText = strSQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dtDVN = Connection.GetDataTable(cmd);
            dtDVN = HamChung.SelectDistinct("DVN", dtDVN, "sSoChungTuChiTiet", "sSoChungTuChiTiet,iNgay,iThang,sTenDonVi_Nhan,sDiaChi_Nhan,sMaSo_Nhan,sSoTaiKhoan_Nhan,sKhoBac_Nhan,sNoiDung");
            //Lay thong tin don vi tra
            strSQL = "SELECT CT.sSoChungTuChiTiet,CT.sTenDonVi_Co,DVN.sDiaChi AS sDiaChi_Co, DVN.sMaSo AS sMaSo_Co, DVN.sSoTaiKhoan AS sSoTaiKhoan_Co, DVN.sKhoBac AS sKhoBac_Co,CT.dNgayTao";
            strSQL += " FROM((SELECT * FROM KTTG_ChungTuChiTiet WHERE {0} ) CT";
            strSQL += " LEFT JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)  AS DVN ON DVN.iID_MaDonVi = CT.iID_MaDonVi_Co)";
            strSQL += " ORDER By CT.dNgayTao";
            strSQL = String.Format(strSQL, DK);
            cmd.CommandText = strSQL;
            DataTable dtDVT = Connection.GetDataTable(cmd);

            dtDVT = HamChung.SelectDistinct("DVT", dtDVT, "sSoChungTuChiTiet", "sSoChungTuChiTiet,sTenDonVi_Co,sDiaChi_Co,sMaSo_Co,sSoTaiKhoan_Co,sKhoBac_Co");
            //Lay thong tin nguoi thu chi
            strSQL = "SELECT CT.sSoChungTuChiTiet,CT.iID_MaNhanVien_ThuChi,CT.sTenNhanVien_ThuChi, NV.sChungMinhThu,Convert(varchar(10),NV.dNgayCap,103) AS dNgayCap ,NV.sNoiCap,CT.dNgayTao";
            strSQL += " FROM((SELECT * FROM KTTG_ChungTuChiTiet WHERE {0}) CT";
            strSQL += " LEFT JOIN KT_NhanVien as NV ON NV.iID_MaNhanVien = CT.iID_MaNhanVien_ThuChi)";
            strSQL += " ORDER By CT.dNgayTao";
            strSQL = String.Format(strSQL, DK);
            cmd.CommandText = strSQL;          
              
            DataTable dtNV = Connection.GetDataTable(cmd);
            dtNV = HamChung.SelectDistinct("NV", dtNV, "sSoChungTuChiTiet","sSoChungTuChiTiet,sTenNhanVien_ThuChi,sChungMinhThu,dNgayCap,sNoiCap");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String SoCT_1, SoCT_2;
                SoCT_1 = Convert.ToString(dt.Rows[i]["sSoChungTuChiTiet"]);
                tong = long.Parse(dt.Rows[i]["rSoTien"].ToString());
                dt.Rows[i]["Tien"] = CommonFunction.TienRaChu(tong);
                for (int j = 0; j < dtDVN.Rows.Count; j++)
                {
                    SoCT_2 = Convert.ToString(dtDVN.Rows[j]["sSoChungTuChiTiet"]);
                    if (SoCT_1.Equals(SoCT_2))
                    {
                        for(int c=1;c<dtDVN.Columns.Count;c++)
                        {
                            dt.Rows[i][dtDVN.Columns[c].ColumnName] = dtDVN.Rows[j][dtDVN.Columns[c].ColumnName];
                        }
                        break;
                    }

                }

                for (int j = 0; j < dtDVT.Rows.Count; j++)
                {
                    SoCT_2 = Convert.ToString(dtDVT.Rows[j]["sSoChungTuChiTiet"]);
                    if (SoCT_1.Equals(SoCT_2))
                    {
                        for (int c = 1; c < dtDVT.Columns.Count; c++)
                        {
                            dt.Rows[i][dtDVT.Columns[c].ColumnName] = dtDVT.Rows[j][dtDVT.Columns[c].ColumnName];
                        }
                        break;
                    }

                }

                for (int j = 0; j < dtNV.Rows.Count; j++)
                {
                    SoCT_2 = Convert.ToString(dtNV.Rows[j]["sSoChungTuChiTiet"]);
                    if (SoCT_1.Equals(SoCT_2))
                    {
                        for (int c = 1; c < dtNV.Columns.Count; c++)
                        {
                            dt.Rows[i][dtNV.Columns[c].ColumnName] = dtNV.Rows[j][dtNV.Columns[c].ColumnName];
                        }
                        break;
                    }

                }
            }
            dtDVN.Dispose();
            dtDVT.Dispose();
            dtNV.Dispose();
            cmd.Dispose();
            if (SoLien > 1)
            {
                DataTable dtKQ = dt.Clone();
                dtKQ.TableName = "tb1";
                dt.TableName = "tb2";
                DataRow R;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    R = dt.Rows[i];
                    for (int j = 0; j < SoLien; j++)
                    {

                        dtKQ.ImportRow(R);
                    }
                }
                return dtKQ;
            }
            else
            {
                return dt;
            }
        }

        public DataTable DVNhan(String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet)
        {
            
            String SQL1 = "select";
            SQL1 += " TOP 1 CT.sSoChungTuChiTiet,CT.sTenDonVi_Nhan,CT.sTenDonVi_Co,CT.iID_MaDonVi_Co";
            SQL1 += ",CT.iID_MaDonVi_Nhan,CT.sNoiDung,CT.rSoTien";
            SQL1 += ",DV.sDiaChi, DV.sMaSo , DV.sSoTaiKhoan, DV.sKhoBac, CT.iID_MaNhanVien_ThuChi,CT.sTenNhanVien_ThuChi,NV.sChungMinhThu, NV.dNgayCap,NV.sNoiCap";
            SQL1 += " FROM KTTG_ChungTuChiTiet as CT";
            SQL1 += " LEFT JOIN KT_NhanVien as NV ON NV.iID_MaNhanVien = CT.iID_MaNhanVien_ThuChi";
            SQL1 += " LEFT JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)  as DV ON DV.iID_MaDonVi = CT.iID_MaDonVi_Co ";
            SQL1 += "  WHERE  ";
            SQL1 += "   CT.iTrangThai =1 AND CT.iID_MaChungTu=@iID_MaChungTu";
            // AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
            SqlCommand cmd = new SqlCommand(SQL1);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
           // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Lay_SoChungTu(String iNamLamViec, String iThang, String iID_MaChungTu)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT CT.sSoChungTuChiTiet +'#'+ iID_MaDonVi_Nhan AS sSoChungTuChiTiet,Convert(varchar,CT.sSoChungTuChiTiet)+'-'+ SUBSTRING(CT.sTenDonVi_Nhan,charindex('-',CT.sTenDonVi_Nhan)+1,100) AS sTenDonVi_Nhan";
            SQL += " FROM KTTG_ChungTuChiTiet as CT WHERE  iTrangThai =1 and iNamLamViec = @iNamLamViec";
            SQL += "  AND CT.iID_MaChungTu=@iID_MaChungTu";
            SQL += " Group by iID_MaChungTu,sSoChungTuChiTiet,sTenDonVi_Nhan,iID_MaDonVi_Nhan ORDER BY sTenDonVi_Nhan";
            //AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Tách lấy số chứng từ chi tiết mã đơn vị nhận
        /// </summary>
        /// <param name="sValue">Chuỗi số chứng từ chi tiết và mã đơn vị nhận</param>
        /// <param name="LoaiCanLay">=0 lấy số chứng từ ,1 lấy mã đơn vị nhận</param>
        /// <returns></returns>
        public static String TachLaySoChungTu_DonViNhan(String sValue, int LoaiCanLay)
        {
            String[] arrValue = sValue.Split('#');
            return arrValue[LoaiCanLay];
        }

    }
}
