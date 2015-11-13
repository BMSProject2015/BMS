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
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_PhanHoController : Controller
    {
        //
        // GET: /rptPhanHo/
        // GET: /rptBangTongHopChungTuGoc/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_PhanHo.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = 0;
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_PhanHo.aspx";
                return View("~/Report_Views/KeToan/TongHop/rptKeToanTongHop_PhanHo_Loc.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit_Loc(String ParentID)
        {

            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_PhanHo.aspx";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["ChiSo"] = 0;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iThang"] = iThang;
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID, String iThang, int? ChiSo)
        {
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["ChiSo"] = ChiSo;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_PhanHo.aspx";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iThang"] = iThang;
            if (ChiSo == 0) return RedirectToAction("Index");
            return View(sViewPath + "ReportView.aspx");
        }

        public ExcelFile CreateReport(String path, String iThang, String iID_MaDonVi, String UserName, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            //Result = TaoTieuDe(Result);
            // DataTable data = dt_PhanHo(iThang, iID_MaDonVi, UserName);
            //FillData(Result, data, 3, 4, 4, 4, "");
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_PhanHo");

            DataTable dtNamLamViec = NguoiDungCauHinhModels.LayCauHinh(UserName);
            LoadData(fr, iThang, iID_MaDonVi, UserName, iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", dtNamLamViec.Rows[0]["iNamLamViec"]);
            fr.SetValue("Thang", iThang);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("Ngay", ngay);
            fr.Run(Result);
            return Result;

        }

        public clsExcelResult ExportToPDF(String iThang, String iID_MaDonVi, String UserName, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaDonVi, UserName, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String iThang, String iID_MaDonVi, String UserName, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaDonVi, UserName, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKeToanTongHopPhanHo.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String iThang, String iID_MaDonVi, String UserName, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iThang, iID_MaDonVi, UserName, iID_MaTrangThaiDuyet);

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
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaDonVi, String UserName, String iID_MaTrangThaiDuyet)
        {
            String TenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            DataTable data = dt_PhanHo(iThang, iID_MaDonVi, UserName, iID_MaTrangThaiDuyet);
            //DataView dv = data.DefaultView;
            //dv.Sort = "iID_MaTaiKhoan";
            //data = dv.ToTable();
            //for (int i = 0; i < data.Rows.Count; i++)
            //{
            //    DataRow r=data.Rows[i];
            //    if (String.IsNullOrEmpty(Convert.ToString(r["rSoPhatSinhNo"]))  &&  String.IsNullOrEmpty(Convert.ToString(r["rSoPhatSinhCo"])))
            //    {
            //        data.Rows.Remove(r);
            //    }
            //}
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow r = data.Rows[i];
                if (String.IsNullOrEmpty(Convert.ToString(r["rSoPhatSinhNo"])) && String.IsNullOrEmpty(Convert.ToString(r["rSoPhatSinhCo"])) && Convert.ToString(r["bLahangCha"]) != "4")
                {
                    data.Rows.Remove(r);
                }
            }

            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            fr.SetValue("TenDonVi", iID_MaDonVi + "-" + TenDonVi);
            data.Dispose();
        }
        public DataTable dt_PhanHo(String iThang, String iID_MaDonVi, String UserName, String iID_MaTrangThaiDuyet)
        {
            String SQLDT = @"SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung";
            SqlCommand cmd_DT = new SqlCommand(SQLDT);
            cmd_DT.Parameters.AddWithValue("@sID_MaNguoiDung", UserName);
            String sDoiTuong = Connection.GetValueString(cmd_DT, "-1");
            String DKDoiTuong = "", DKTrangThai = "";
            if (sDoiTuong == "1")
            {
                DKDoiTuong = " AND CT.sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
            }
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKTrangThai = " AND CT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }

            DataTable dtNamLamViec = NguoiDungCauHinhModels.LayCauHinh(UserName);
            String iNamLamViec = Convert.ToString(dtNamLamViec.Rows[0]["iNamLamViec"]);
            String sDSTaiKhoan = KeToan_DanhMucThamSoModels.LayThongTinThamSo("39", Convert.ToString(dtNamLamViec.Rows[0]["iNamLamViec"]));
            String[] arrDSTaiKhoan = sDSTaiKhoan.Split(',');
            String DK_Co = "", DK_No = "";
            SqlCommand cmd = new SqlCommand();

            for (int i = 0; i < arrDSTaiKhoan.Length; i++)
            {
                DK_Co += " iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan_Co" + i.ToString();
                if (i < arrDSTaiKhoan.Length - 1) DK_Co += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co" + i.ToString(), arrDSTaiKhoan[i] + "%");
                DK_No += " iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan_No" + i.ToString();
                if (i < arrDSTaiKhoan.Length - 1) DK_No += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No" + i.ToString(), arrDSTaiKhoan[i] + "%");
            }

            if (String.IsNullOrEmpty(DK_Co) == false) DK_Co = " AND (" + DK_Co + ")";
            if (String.IsNullOrEmpty(DK_No) == false) DK_No = " AND (" + DK_No + ")";

            if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = "100000";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String DKPB_No = "", DKPB_Co = "";

            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKPB_No += "CT.iID_MaDonVi_No=@iID_MaDonVi" + i.ToString();
                if (i < arrDonVi.Length - 1) DKPB_No += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);

                DKPB_Co += "CT.iID_MaDonVi_Co=@iID_MaDonVi" + i.ToString();
                if (i < arrDonVi.Length - 1) DKPB_Co += " OR ";

            }
            if (String.IsNullOrEmpty(DKPB_No) == false)
            {
                DKPB_No = " AND (" + DKPB_No + ")";
                DKPB_Co = " AND (" + DKPB_Co + ")";
            }


            String SQL = " SELECT CT.iNgayCT,Convert(varchar,CT.iNgayCT) + ' - ' + Convert(varchar,CT.iThang) as NgayGS";
            SQL += ",convert(int, sSoChungTu) as sSoChungTu,Convert(varchar,CT.iNgay) + ' - ' + Convert(varchar,CT.iThang) as NgayCT,sSoChungTuChiTiet";
            SQL += ",iID_MaTaiKhoan_Co as iID_MaTaiKhoan ";
            SQL += ",iID_MaTaiKhoan_No as TKDoiUng ";
            SQL += ",iID_MaTaiKhoan_Cha";
            SQL += ",CT.sNoiDung";
            SQL += ",MoTa=''";
            SQL += ",bLaHangCha=0";
            SQL += ",rSoPhatSinhNo=0.0,rSoTien as rSoPhatSinhCo";
            SQL += " FROM KT_ChungTuChiTiet AS CT";
            SQL += " INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu=CT.iID_MaChungTu";
            SQL += " INNER JOIN KT_TaiKhoan ON CT.iID_MaTaiKhoan_Co=KT_TaiKhoan.iID_MaTaiKhoan";
            SQL += " WHERE CT.iTrangThai=1 AND CT.iNamLamViec=@iNamLamViec ";
            SQL += " {2}";
            SQL += " AND CT.iThangCT =@iThang {4} {5} ";
            SQL += " {0}";
            SQL += " UNION	";
            SQL += "  SELECT CT.iNgayCT,Convert(varchar,CT.iNgayCT) + ' - ' + Convert(varchar,CT.iThang) as NgayGS";
            SQL += ",convert(int, sSoChungTu) as sSoChungTu,Convert(varchar,CT.iNgayCT) +' - ' + Convert(varchar,CT.iThangCT) as NgayCT,sSoChungTuChiTiet";
            SQL += ",iID_MaTaiKhoan_No as iID_MaTaiKhoan ";
            SQL += ",iID_MaTaiKhoan_Co as TKDoiUng ";
            SQL += ",iID_MaTaiKhoan_Cha";
            SQL += ",CT.sNoiDung";
            SQL += ",MoTa=''";
            SQL += ",bLaHangCha=0";
            SQL += ",rSoTien AS rSoPhatSinhNo,rSoPhatSinhCo=0.0";
            SQL += " FROM KT_ChungTuChiTiet AS CT";
            SQL += " INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu=CT.iID_MaChungTu";
            SQL += " INNER JOIN KT_TaiKhoan ON CT.iID_MaTaiKhoan_No=KT_TaiKhoan.iID_MaTaiKhoan";
            SQL += " WHERE CT.iTrangThai=1 AND CT.iNamLamViec=@iNamLamViec ";
            SQL += " {3}";
            SQL += " AND CT.iThangCT =@iThang ";
            SQL += " {1} {4} {5}";
            SQL += "  ORDER BY iID_MaTaiKhoan,CT.iNgayCT,sSoChungTu ";
            SQL = String.Format(SQL, DK_Co, DK_No, DKPB_Co, DKPB_No, DKTrangThai, DKDoiTuong);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", dtNamLamViec.Rows[0]["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", UserName);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            //Tính dt dư đầu kỳ
            int iThangTruoc = Convert.ToInt16(iThang) - 1;
            DataTable dtDuDauKy = dt_DauKy_CuoiKy_LuyKe(iThangTruoc.ToString(), UserName, iID_MaTrangThaiDuyet, iID_MaDonVi, arrDSTaiKhoan, 1);



            DataTable dtCuoiKy = dt_DauKy_CuoiKy_LuyKe(iThang, UserName, iID_MaTrangThaiDuyet, iID_MaDonVi, arrDSTaiKhoan, 1);



            DataTable dtCPS = dt_DauKy_CuoiKy_LuyKe(iThang, UserName, iID_MaTrangThaiDuyet, iID_MaDonVi, arrDSTaiKhoan, 2);


            DataTable dtLuyKe = dt_DauKy_CuoiKy_LuyKe(iThang, UserName, iID_MaTrangThaiDuyet, iID_MaDonVi, arrDSTaiKhoan, 3);


            DataTable dtKQ = Insert_TaiKhoanCha(dt, dtDuDauKy, dtCuoiKy, dtCPS, dtLuyKe, iNamLamViec);
            dt.Dispose();
            dtDuDauKy.Dispose();
            dtCuoiKy.Dispose();
            dtLuyKe.Dispose();
            cmd.Dispose();
            return dtKQ;
        }

        public DataTable dt_DauKy_CuoiKy_LuyKe(String iThang, String UserName, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String[] arrDSTaiKhoan, int LoaiDT)
        {
            String SQLDT = @"SELECT iDoiTuongNguoiDung FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung";
            SqlCommand cmd_DT = new SqlCommand(SQLDT);
            cmd_DT.Parameters.AddWithValue("@sID_MaNguoiDung", UserName);
            String sDoiTuong = Connection.GetValueString(cmd_DT, "-1");
            String DKDoiTuong = "", DKTrangThai = "";
            if (sDoiTuong == "1")
            {
                //DKDoiTuong = " AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
                DKDoiTuong = "";
            }
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKTrangThai = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            DataTable dtNamLamViec = NguoiDungCauHinhModels.LayCauHinh(UserName);
            String iNamLamViec = Convert.ToString(dtNamLamViec.Rows[0]["iNamLamViec"]);
            dtNamLamViec.Dispose();
            String DKThang = " AND iThangCT <= @iThangCT ";
            switch (LoaiDT)
            {
                case 1:
                    DKThang = " AND iThangCT <= @iThangCT "; //Đầu kỳ cuối kỳ
                    break;
                case 2:
                    DKThang = " AND iThangCT = @iThangCT "; //Cộng phát sinh
                    break;
                case 3:
                    DKThang = " AND iThangCT <= @iThangCT AND iThangCT > 0 "; //lũy kế
                    break;
            }


            String DK_Co = "", DK_No = "";
            SqlCommand cmd = new SqlCommand();

            for (int i = 0; i < arrDSTaiKhoan.Length; i++)
            {
                DK_Co += " iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan_Co" + i.ToString();
                if (i < arrDSTaiKhoan.Length - 1) DK_Co += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co" + i.ToString(), arrDSTaiKhoan[i] + "%");
                DK_No += " iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan_No" + i.ToString();
                if (i < arrDSTaiKhoan.Length - 1) DK_No += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No" + i.ToString(), arrDSTaiKhoan[i] + "%");
            }

            if (String.IsNullOrEmpty(DK_Co) == false)
            {
                DK_No = " AND (" + DK_No + ")";
                DK_Co = " AND (" + DK_Co + ")";
            }


            String SQL = "SELECT iID_MaTaiKhoan_No as iID_MaTaiKhoan ,SUM(rPS_No) AS rLK_No,SUM(rPS_Co) AS rLK_Co";
            SQL += ",rCK_No=CASE WHEN SUM(rPS_No)-SUM(rPS_Co)>0 THEN SUM(rPS_No)-SUM(rPS_Co) ELSE 0 END";
            SQL += ",rCK_Co=CASE WHEN SUM(rPS_No)-SUM(rPS_Co)<0 THEN SUM(rPS_Co)-SUM(rPS_No) ELSE 0 END";
            SQL += " FROM (";
            SQL += " SELECT ";
            SQL += " iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co=iID_MaTaiKhoan_No,SUM(rSoTien) as rPS_No,rPS_Co=0.0";
            SQL += " FROM KT_ChungTuChiTiet";
            SQL += " WHERE iTrangThai=1 {3} {4} AND iID_MaTaiKhoan_No <> ''";
            SQL += " {0} ";
            SQL += " AND iNamLamViec=@iNamLamViec";
            SQL += " AND iID_MaDonVi_No=@iID_MaDonVi";
            SQL += " {1}";
            SQL += " GROUP BY iID_MaTaiKhoan_No";
            SQL += " UNION";
            SQL += " SELECT ";
            SQL += " iID_MaTaiKhoan_No=iID_MaTaiKhoan_Co,iID_MaTaiKhoan_Co,rPS_No=0.0,SUM(rSoTien) as rPS_Co";
            SQL += " FROM KT_ChungTuChiTiet";
            SQL += " WHERE iTrangThai=1  {3} {4}  AND iID_MaTaiKhoan_Co <> ''";
            SQL += " {0} ";
            SQL += " AND iNamLamViec=@iNamLamViec";
            SQL += " AND iID_MaDonVi_Co=@iID_MaDonVi";
            SQL += " {2}";
            SQL += " GROUP BY iID_MaTaiKhoan_Co";
            SQL += " ) KT";
            SQL += " GROUP BY iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co";
            String strSQL = String.Format(SQL, DKThang, DK_No, DK_Co, DKTrangThai, DKDoiTuong);
            cmd.CommandText = strSQL;
            cmd.Parameters.AddWithValue("@iThangCT", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", UserName);
            DataTable dt = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return dt;
        }

        public static DataTable Insert_TaiKhoanCha(DataTable dtChiTiet, DataTable dtDuDauKy, DataTable dtCuoiKy, DataTable dtCPS, DataTable dtLuyKe, String iNamLamViec)
        {
            String vR = "";
            String SQL = String.Format("SELECT * FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam='{0}' ORDER BY iID_MaTaiKhoan", iNamLamViec);

            DataTable dtKQ = new DataTable();
            for (int c = 0; c < dtChiTiet.Columns.Count; c++)
            {
                dtKQ.Columns.Add(dtChiTiet.Columns[c].ColumnName, dtChiTiet.Columns[c].DataType);
            }
            dtKQ.Columns.Add("CS", typeof(int));


            DataTable dtTaiKhoan = Connection.GetDataTable(SQL);
            String TK_Cha = "", TK_Con = "", TK_KeTiep = "", TK_ChaGanNhat = "", strTruyVan = "";
            DataRow R, R1;
            DataRow[] arrR;
            int i = 0;
            //for (int i = 0; i < dtTaiKhoan.Rows.Count; i++)
            //{
            //    TK_Cha = Convert.ToString(dtTaiKhoan.Rows[i]["iID_MaTaiKhoan"]);
            DataRow[] Rdk, RTK_KOPS;
            int d = 0;
            
            //Điền tài khoản cha
            dtDuDauKy = KeToanTongHopModels.DienTaiKhoanCha(dtDuDauKy, iNamLamViec, "iID_MaTaiKhoan");
            dtCuoiKy = KeToanTongHopModels.DienTaiKhoanCha(dtCuoiKy, iNamLamViec, "iID_MaTaiKhoan");
            dtCPS = KeToanTongHopModels.DienTaiKhoanCha(dtCPS, iNamLamViec, "iID_MaTaiKhoan");
            dtLuyKe = KeToanTongHopModels.DienTaiKhoanCha(dtLuyKe, iNamLamViec, "iID_MaTaiKhoan");

            DataTable dtCT = HamChung.SelectDistinct("CT", dtChiTiet, "iID_MaTaiKhoan", "iID_MaTaiKhoan");            
            String iID_MaTaiKhoan = "";
            for (i = 0; i < dtDuDauKy.Rows.Count; i++)
            {
                iID_MaTaiKhoan = Convert.ToString(dtDuDauKy.Rows[i]["iID_MaTaiKhoan"]);
                RTK_KOPS = dtChiTiet.Select("iID_MaTaiKhoan='" + iID_MaTaiKhoan + "'");
                if (RTK_KOPS == null || RTK_KOPS.Length == 0)
                {
                    DataRow Rct=dtChiTiet.NewRow();
                    Rct["iID_MaTaiKhoan"]=iID_MaTaiKhoan;
                    Rct["sNoiDung"] = iID_MaTaiKhoan + "-" + TaiKhoanModels.LayTenTaiKhoan(iID_MaTaiKhoan);
                    dtChiTiet.Rows.Add(Rct);
                }
            }
            //Sắp xếp lại dtchitiet
            DataView dv = dtChiTiet.DefaultView;
            dv.Sort = "iID_MaTaiKhoan";
            dtChiTiet = dv.ToTable();

            

            #region Thêm rows

            for (int j = 0; j < dtChiTiet.Rows.Count; j++)
            {

                R1 = dtChiTiet.Rows[j];
                TK_Con = Convert.ToString(dtChiTiet.Rows[j]["iID_MaTaiKhoan"]);
                TK_ChaGanNhat = Convert.ToString(dtChiTiet.Rows[j]["iID_MaTaiKhoan_Cha"]);
                if (j < dtChiTiet.Rows.Count - 1) TK_KeTiep = Convert.ToString(dtChiTiet.Rows[j + 1]["iID_MaTaiKhoan"]);

                if (TK_ChaGanNhat == "")
                {
                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_Con;
                    R["iID_MaTaiKhoan_Cha"] = TK_Con + "_DK";
                    R["sNoiDung"] = TK_Con + "-" + TaiKhoanModels.LayTenTaiKhoan(TK_Con);
                    R["MoTa"] = "Dư đầu:";
                    R["bLaHangCha"] = 1;
                    Rdk = dtDuDauKy.Select("iID_MaTaiKhoan='" + TK_Con + "'");
                    if (Rdk.Length > 0)
                    {
                        R["rSoPhatSinhNo"] = Rdk[0]["rCK_No"];
                        R["rSoPhatSinhCo"] = Rdk[0]["rCK_Co"];
                    }
                    R["CS"] = d;
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;
                }
                else
                {
                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_ChaGanNhat + "_DK";
                    R["sNoiDung"] = TK_ChaGanNhat + "-" + TaiKhoanModels.LayTenTaiKhoan(TK_ChaGanNhat);
                    R["MoTa"] = "Dư đầu:";
                    R["bLaHangCha"] = 1;
                    R["CS"] = d;
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_ChaGanNhat + "_CPS"; ;
                    R["MoTa"] = "Cộng phát sinh:";
                    R["bLaHangCha"] = 2;
                    R["CS"] = d;
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_ChaGanNhat + "_CK"; ;
                    R["MoTa"] = "Số dư cuối:";
                    R["bLaHangCha"] = 3;
                    R["CS"] = d;
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_ChaGanNhat + "_LK"; ;
                    R["MoTa"] = "Lũy kế đến tháng này:";
                    R["bLaHangCha"] = 4;
                    R["CS"] = d;
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    //add row số dư đầu kỳ cho tài khoản con
                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_Con;
                    R["iID_MaTaiKhoan_Cha"] = TK_Con + "_DK";
                    R["sNoiDung"] = TK_Con + "-" + TaiKhoanModels.LayTenTaiKhoan(TK_Con);
                    R["MoTa"] = "Dư đầu:";
                    R["bLaHangCha"] = 1;
                    R["CS"] = d;
                    Rdk = dtDuDauKy.Select("iID_MaTaiKhoan='" + TK_Con + "'");
                    if (Rdk.Length > 0)
                    {
                        R["rSoPhatSinhNo"] = Rdk[0]["rCK_No"];
                        R["rSoPhatSinhCo"] = Rdk[0]["rCK_Co"];
                    }
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;
                }

                if (String.IsNullOrEmpty(TK_ChaGanNhat)) TK_ChaGanNhat = TK_Con;
                String select = "iID_MaTaiKhoan LIKE '" + TK_ChaGanNhat + "%'";
                arrR = dtChiTiet.Select(select);
                String TK_Con1 = TK_Con;
                Decimal Tong_No = 0, Tong_Co = 0;
                for (int c = 0; c < arrR.Length; c++)
                {

                    TK_Con1 = Convert.ToString(arrR[c]["iID_MaTaiKhoan"]);
                    if (TK_Con1 == TK_Con)
                    {
                        dtKQ.Rows.Add(arrR[c].ItemArray);
                        if (arrR[c]["rSoPhatSinhNo"] != DBNull.Value)
                            Tong_No = Tong_No + Convert.ToDecimal(arrR[c]["rSoPhatSinhNo"]);

                        if (arrR[c]["rSoPhatSinhCo"] != DBNull.Value)
                            Tong_Co = Tong_Co + Convert.ToDecimal(arrR[c]["rSoPhatSinhCo"]);

                    }
                    else
                    {


                        R = dtKQ.NewRow();
                        R["iID_MaTaiKhoan"] = TK_Con;
                        R["iID_MaTaiKhoan_Cha"] = TK_Con + "_CPS";
                        R["MoTa"] = "Cộng phát sinh:";
                        R["bLaHangCha"] = 2;
                        R["CS"] = d;
                        R["rSoPhatSinhNo"] = Tong_No;
                        R["rSoPhatSinhCo"] = Tong_Co;
                        dtKQ.Rows.Add(R);
                        d = d + 1; ;

                        R = dtKQ.NewRow();
                        R["iID_MaTaiKhoan"] = TK_Con;
                        R["iID_MaTaiKhoan_Cha"] = TK_Con + "_CK";
                        R["MoTa"] = "Số dư cuối:";
                        R["bLaHangCha"] = 3;
                        R["CS"] = d;
                        dtKQ.Rows.Add(R);
                        d = d + 1; ;

                        R = dtKQ.NewRow();
                        R["iID_MaTaiKhoan"] = TK_Con;
                        R["iID_MaTaiKhoan_Cha"] = TK_Con + "_LK";
                        R["MoTa"] = "Lũy kế đến tháng này:";
                        R["bLaHangCha"] = 4;
                        dtKQ.Rows.Add(R);
                        d = d + 1; ;

                        // add row số dư đầu kỳ của tài khoản con tiếp theo
                        TK_Con = TK_Con1;
                        R = dtKQ.NewRow();
                        R["iID_MaTaiKhoan"] = TK_Con;
                        R["iID_MaTaiKhoan_Cha"] = TK_Con + "_DK";
                        R["sNoiDung"] = TK_Con + "-" + TaiKhoanModels.LayTenTaiKhoan(TK_Con);
                        R["MoTa"] = "Dư đầu:";
                        R["bLaHangCha"] = 1;
                        R["CS"] = d;
                        dtKQ.Rows.Add(R);
                        dtKQ.Rows.Add(arrR[c].ItemArray);
                        d = d + 1; ;

                        Tong_No = 0;
                        Tong_Co = 0;
                        if (arrR[c]["rSoPhatSinhNo"] != DBNull.Value)
                            Tong_No = Tong_No + Convert.ToDecimal(arrR[c]["rSoPhatSinhNo"]);

                        if (arrR[c]["rSoPhatSinhCo"] != DBNull.Value)
                            Tong_Co = Tong_Co + Convert.ToDecimal(arrR[c]["rSoPhatSinhCo"]);


                    }

                    dtChiTiet.Rows.Remove(arrR[c]);
                }

                R = dtKQ.NewRow();
                R["iID_MaTaiKhoan"] = TK_Con;
                R["iID_MaTaiKhoan_Cha"] = TK_Con + "_CPS";
                R["MoTa"] = "Cộng phát sinh:";
                R["bLaHangCha"] = 2;
                R["CS"] = d;
                R["rSoPhatSinhNo"] = Tong_No;
                R["rSoPhatSinhCo"] = Tong_Co;
                dtKQ.Rows.Add(R);
                d = d + 1; ;

                R = dtKQ.NewRow();
                R["iID_MaTaiKhoan"] = TK_Con;
                R["iID_MaTaiKhoan_Cha"] = TK_Con + "_CK";
                R["MoTa"] = "Số dư cuối :";
                R["bLaHangCha"] = 3;
                R["CS"] = d;
                dtKQ.Rows.Add(R);
                d = d + 1; ;

                R = dtKQ.NewRow();
                R["iID_MaTaiKhoan"] = TK_Con;
                R["iID_MaTaiKhoan_Cha"] = TK_Con + "_LK";
                R["MoTa"] = "Lũy kế đến tháng này:";
                R["bLaHangCha"] = 4;
                dtKQ.Rows.Add(R);
                d = d + 1; ;

                j = j - 1;



            }
            #endregion

            String TK, TK1, TK2;
            Decimal TongPhatSinh_No = 0, TongPhatSinh_Co = 0;
            arrR = dtKQ.Select("bLaHangCha=1");
            for (i = 0; i < arrR.Length; i++)
            {
                TK = Convert.ToString(arrR[i]["iID_MaTaiKhoan_Cha"]).Trim();
                for (int j = 0; j < dtDuDauKy.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtDuDauKy.Rows[j]["iID_MaTaiKhoan"]).Trim();
                    if (TK == TK1 + "_DK")
                    {

                        int id = dtKQ.Rows.IndexOf(arrR[i]);
                        dtKQ.Rows[id]["rSoPhatSinhNo"] = dtDuDauKy.Rows[j]["rCK_No"];
                        dtKQ.Rows[id]["rSoPhatSinhCo"] = dtDuDauKy.Rows[j]["rCK_Co"];
                        dtDuDauKy.Rows.RemoveAt(j);
                        break;

                    }

                }

            }
            arrR = dtKQ.Select("bLaHangCha=2");// Cộng phát sinh
            for (i = 0; i < arrR.Length; i++)
            {
                TK = Convert.ToString(arrR[i]["iID_MaTaiKhoan_Cha"]).Trim();

                for (int j = 0; j < dtCPS.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtCPS.Rows[j]["iID_MaTaiKhoan"]).Trim();
                    if (TK == TK1 + "_CPS")
                    {

                        int id = dtKQ.Rows.IndexOf(arrR[i]);
                        dtKQ.Rows[id]["rSoPhatSinhNo"] = dtCPS.Rows[j]["rLK_No"];
                        dtKQ.Rows[id]["rSoPhatSinhCo"] = dtCPS.Rows[j]["rLK_Co"];
                        dtCPS.Rows.RemoveAt(j);
                        break;

                    }

                }
            }
            arrR = dtKQ.Select("bLaHangCha=3");// Cuối kỳ
            for (i = 0; i < arrR.Length; i++)
            {
                TK = Convert.ToString(arrR[i]["iID_MaTaiKhoan_Cha"]).Trim();
                for (int j = 0; j < dtCuoiKy.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtCuoiKy.Rows[j]["iID_MaTaiKhoan"]).Trim();
                    if (TK == TK1 + "_CK")
                    {

                        int id = dtKQ.Rows.IndexOf(arrR[i]);
                        dtKQ.Rows[id]["rSoPhatSinhNo"] = dtCuoiKy.Rows[j]["rCK_No"];
                        dtKQ.Rows[id]["rSoPhatSinhCo"] = dtCuoiKy.Rows[j]["rCK_Co"];
                        dtCuoiKy.Rows.RemoveAt(j);
                        break;
                    }

                }
            }
            arrR = dtKQ.Select("bLaHangCha=4");// Lũy kế
            for (i = 0; i < arrR.Length; i++)
            {
                TK = Convert.ToString(arrR[i]["iID_MaTaiKhoan_Cha"]).Trim();
                for (int j = 0; j < dtLuyKe.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtLuyKe.Rows[j]["iID_MaTaiKhoan"]).Trim();
                    if (TK == TK1 + "_LK")
                    {

                        int id = dtKQ.Rows.IndexOf(arrR[i]);
                        dtKQ.Rows[id]["rSoPhatSinhNo"] = dtLuyKe.Rows[j]["rLK_No"];
                        dtKQ.Rows[id]["rSoPhatSinhCo"] = dtLuyKe.Rows[j]["rLK_Co"];
                        dtLuyKe.Rows.RemoveAt(j);
                        break;

                    }

                }

            }

            //add những tài khoản có trong điều kiện nhưng không có phát sinh
            if (dtDuDauKy.Rows.Count > 0)
            {
                for (i = 0; i < dtDuDauKy.Rows.Count; i++)
                {
                    TK = Convert.ToString(dtDuDauKy.Rows[i]["iID_MaTaiKhoan"]);
                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK;
                    R["iID_MaTaiKhoan_Cha"] = TK + "_DK";
                    R["sNoiDung"] = TK + "-" + TaiKhoanModels.LayTenTaiKhoan(TK);
                    R["MoTa"] = "Dư đầu:";
                    R["bLaHangCha"] = 1;
                    R["CS"] = d;
                    if (i < dtDuDauKy.Rows.Count)
                    {
                        R["rSoPhatSinhNo"] = dtDuDauKy.Rows[i]["rCK_No"];
                        R["rSoPhatSinhCo"] = dtDuDauKy.Rows[i]["rCK_Co"];
                    }
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_Con + "_CPS";
                    R["MoTa"] = "Cộng phát sinh:";
                    R["bLaHangCha"] = 2;
                    R["CS"] = d;
                    if (i < dtCPS.Rows.Count)
                    {
                        R["rSoPhatSinhNo"] = 0;
                        R["rSoPhatSinhCo"] = 0;
                    }
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_Con + "_CK";
                    R["MoTa"] = "Số dư cuối:";
                    R["bLaHangCha"] = 3;
                    R["CS"] = d;
                    if (i < dtCuoiKy.Rows.Count)
                    {
                        R["rSoPhatSinhNo"] = dtCuoiKy.Rows[i]["rCK_No"];
                        R["rSoPhatSinhCo"] = dtCuoiKy.Rows[i]["rCK_Co"];
                    }
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;

                    R = dtKQ.NewRow();
                    R["iID_MaTaiKhoan"] = TK_ChaGanNhat;
                    R["iID_MaTaiKhoan_Cha"] = TK_Con + "_LK";
                    R["MoTa"] = "Lũy kế đến tháng này:";
                    R["bLaHangCha"] = 4;
                    if (i < dtCuoiKy.Rows.Count)
                    {
                        R["rSoPhatSinhNo"] = dtDuDauKy.Rows[i]["rLK_No"];
                        R["rSoPhatSinhCo"] = dtDuDauKy.Rows[i]["rLK_Co"];
                    }
                    dtKQ.Rows.Add(R);
                    d = d + 1; ;
                }
            }

            dtChiTiet.Dispose();
            dtTaiKhoan.Dispose();

            return dtKQ;
        }

        public JsonResult ObjDanhSachDonVi(String iThangCT, String iNamLamViec, String MaND, String iID_MaTrangThaiDuyet)
        {
            return Json(get_sDanhSachDonVi(iThangCT, iNamLamViec, MaND, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String iThangCT, String iNamLamViec, String MaND, String iID_MaTrangThaiDuyet)
        {
            String DK = "",DK1="",DK2="";
            if ((iID_MaTrangThaiDuyet!="0" && !String.IsNullOrEmpty(iID_MaTrangThaiDuyet))) DK += " AND iID_MaTrangThaiDuyet='" + iID_MaTrangThaiDuyet + "'";
            String SQLTK = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec='" + iNamLamViec + "' AND sKyHieu=39";
            SqlCommand cmd1 = new SqlCommand(SQLTK);
            String sTaiKhoan = Connection.GetValueString(cmd1, "");
            if (!String.IsNullOrEmpty(sTaiKhoan))
            {
                DK2+=" AND iID_MaTaiKhoan_No IN ("+sTaiKhoan+")";
                DK1 += " AND iID_MaTaiKhoan_Co IN (" + sTaiKhoan + ")";
            }
            String SQL = "SELECT distinct iID_MaDonVi_Co AS iID_MaDonVi,sTenDonVi_Co as sTenDonVi FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND  iThangCT<=@iThangCT AND iNamLamViec=@iNamLamViec AND iID_MaDonVi_Co<>'' "+DK+DK2;
            SQL += " UNION ";
            SQL += " SELECT distinct iID_MaDonVi_No AS iID_MaDonVi,sTenDonVi_No as sTenDonVi FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iThangCT<=@iThangCT AND iNamLamViec=@iNamLamViec AND iID_MaDonVi_No<>''"+DK+DK1;
            SQL += " ORDER BY iID_MaDonVi";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String MaDonVi1 = "", MaDonVi2 = "", TenDonVi = "";
            DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MaDonVi1 = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < dtDonVi.Rows.Count; j++)
                {
                    MaDonVi2 = Convert.ToString(dtDonVi.Rows[j]["iID_MaDonVi"]);
                    TenDonVi = Convert.ToString(dtDonVi.Rows[j]["sTen"]);
                    if (MaDonVi1.Equals(MaDonVi2))
                    {
                        dtDonVi.Rows[j]["sTen"] = TenDonVi + " (<b style=\"color:Red;\">*</b>)";
                    }
                }
            }
            int SoCot = 1;
           // int SoCot = 2;
           // if (dtDonVi.Rows.Count >= 10)
               // SoCot = 3;//chuyển 5->3

            StringBuilder stb = new StringBuilder();
            //String[] arrMaDonVi;
            stb.Append("<table  class=\"mGrid\">");
            stb.Append("<tr>");
            stb.Append("<th align=\"center\" style=\"width: 30px;\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAll(this.checked)\" /></th>");//Chuyển left->center
            //for (int c = 0; c < SoCot * 2 - 1; c++)
            //{
            //    stb.Append("<th></th>");
            //}
            stb.Append("<th colspan=" + (SoCot * 2 - 1) + " align=\"left\">Chọn đơn vị để in</th>");
            stb.Append("</tr>");
            stb.Append("</table>");
            stb.Append("<div style=\"width: 100%; height: 300px; overflow: scroll; border: 1px solid #dedede;\" ><table  class=\"mGrid\" >");
            String strsTen = "", MaDonVi = "", strChecked = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i = i + SoCot)
            {
                stb.Append("<tr>");
                for (int c = 0; c < SoCot; c++)
                {
                    if (i + c < dtDonVi.Rows.Count)
                    {
                        strChecked = "";
                        strsTen = Convert.ToString(dtDonVi.Rows[i + c]["sTen"]);
                        MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["iID_MaDonVi"]);
                        //for (int j = 0; j < arrMaDonVi.Length; j++)
                        //{
                        //    if (MaDonVi.Equals(arrMaDonVi[j]))
                        //    {
                        //        strChecked = "checked=\"checked\"";
                        //        break;
                        //    }
                        //}
                        stb.Append("<td align=\"center\" style=\"width: 30px;\">");//Chuyển left->center
                        stb.Append("<input type=\"checkbox\" value=\"" + MaDonVi + "\"check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\"/>");
                        stb.Append("</td>");
                        stb.Append("<td align=\"left\">" + MaDonVi + " - " + strsTen + "</td>");

                    }
                }
                stb.Append("</tr>");
            }
            stb.Append(" </div></table>");
            stb.Append("<script type=\"text/javascript\">");
            stb.Append("function CheckAll(value) {");
            stb.Append("$(\"input:checkbox[check-group='DonVi']\").each(function (i) {");
            stb.Append("this.checked = value;");
            stb.Append("});");
            stb.Append("}");
            stb.Append("</script>");
            return stb.ToString(); ;
        }

    }
}

