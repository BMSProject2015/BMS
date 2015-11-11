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
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_KiemTraChungTuController : Controller
    {
        //
        // GET: /rptChiTieu_TongHopChiTieu_7/
        public string sViewPath = "~/Report_Views/";
        public string Count = "";
        public decimal Tien = 0;
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_KiemTraDinhDangChungTu.xls";
        private const String sFilePath_CTPS = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_KiemTraChungTuPS.xls";
        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_KiemTraDinhDangChungTu.aspx";
                // ViewData["FilePath"] = Server.MapPath(sFilePath);
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }



        public ActionResult EditSubmit(String ParentID, String iNamLamViec)
        {
            String TuNgay = Request.Form[ParentID + "_TuNgay"];
            String DenNgay = Request.Form[ParentID + "_DenNgay"];
            String TuThang = Request.Form[ParentID + "_TuThang"];
            String DenThang = Request.Form[ParentID + "_DenThang"];
            String Loai = Request.Form[ParentID + "_Loai"];
            String iTrangThai = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            ViewData["PageLoad"] = "1";
            ViewData["TuNgay"] = TuNgay;
            ViewData["DenNgay"] = DenNgay;
            ViewData["TuThang"] = TuThang;
            ViewData["DenThang"] = DenThang;
            ViewData["Loai"] = Loai;
            ViewData["iID_MaTrangThaiDuyet"] = iTrangThai;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_KiemTraDinhDangChungTu.aspx";
            // ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");

        }
        private string GetGhiChu(String TaiKhoanCo = "", String TaiKhoanNo = "", String DonViNo = "", String DonViCo = "")
        {
            string str = "";
            if (CheckTaiKhoan(TaiKhoanNo)==false) str += "KN";
            if (CheckTaiKhoan(TaiKhoanCo) == false) str += " KC";
            if (CheckTaiKhoanCha(TaiKhoanNo)==true) str += " N";
            if (CheckTaiKhoanCha(TaiKhoanCo) == true) str += " C";
            //if (String.IsNullOrEmpty(TaiKhoanCo) == true && String.IsNullOrEmpty(TaiKhoanNo) == true)
            //    str += " ?";
            //if (String.IsNullOrEmpty(TaiKhoanCo) == false && String.IsNullOrEmpty(TaiKhoanNo) == false && TaiKhoanCo.Trim() == TaiKhoanNo.Trim())
            //    str += " :";
            // if (String.IsNullOrEmpty(ThangCT) == false && String.IsNullOrEmpty(ThangCTGS) == false && ThangCT.Trim() == ThangCTGS.Trim())
            //    str += " T";
            if (CheckTuDienTaiKhoanNo(TaiKhoanNo) == false || CheckTuDienTaiKhoanCo(TaiKhoanCo) == false) str += " !";
            if (CheckDonVi(DonViNo) == false || CheckDonVi(DonViCo) == false) str += " D";
            return str;
        }
        private Boolean CheckTuDienTaiKhoanNo(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoanNo FROM KT_TuDien WHERE iID_MaTaiKhoanNo <> @MaTaiKhoan AND iID_MaTaiKhoanCo=@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckTuDienTaiKhoanCo(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoanNo FROM KT_TuDien WHERE iID_MaTaiKhoanNo= @MaTaiKhoan AND iID_MaTaiKhoanCo<>@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckDonVi(String iID_MaDonVi = "")
        {
            string sql = "SELECT iID_MaDonVi FROM NS_DonVi WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckTaiKhoan(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoan FROM KT_TaiKhoan WHERE iTrangThai=1 AND iID_MaTaiKhoan=@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckTaiKhoanCha(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoan FROM KT_TaiKhoan WHERE bLaHangCha = 1 AND iTrangThai=1 AND iID_MaTaiKhoan=@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        //private static string getSCTGSThieu(String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "")
        //{
        //    String iTuNgay = iNamLamViec + "/" + TuThang + "/" + TuNgay;
        //    String iDenNgay = iNamLamViec + "/" + DenThang + "/" + DenNgay;
        //    SqlCommand cmd = new SqlCommand();
        //    string SQL = "SELECT sSoChungTu FROM KT_ChungTu";
        //    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) >= @TuNgay)";
        //    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)";
        //    cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay));
        //    cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay));
        //    SQL += " ORDER BY CONVERT(int, sSoChungTu) ASC";
        //    cmd.CommandText = SQL;
        //    var dt = Connection.GetDataTable(cmd);
        //}
        public static DataTable dtLoaiBangKe()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaBangKe");
            dt.Columns.Add("TenBangKe");
            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "1. Bảng kê chứng từ phát sinh";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "0";
            dr1[1] = "2. Bảng kê chứng từ sai";
            dt.Rows.Add(dr1);
            dt.Dispose();
            return dt;
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            String sChungTuSai = get_DanhSach_ChungTuThieu(iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, iTrangThai);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDVCapTren = ReportModels.CauHinhTenDonViSuDung(1);
            String tendv = ReportModels.CauHinhTenDonViSuDung(2);
            //using (FlexCelReport fr = new FlexCelReport())
            //{
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_KiemTraChungTu");
            LoadData(fr, iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
            if (Loai == "0")
            {
                fr.SetValue("TuNgay", TuNgay + " - " + TuThang);
                fr.SetValue("DenNgay", DenNgay + " - " + DenThang);
                fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
                fr.SetValue("TenDV", tendv.ToUpper());
                fr.SetValue("Count", Count);
                fr.SetValue("Ngay", DateTime.Now.Day);
                fr.SetValue("Thang", DateTime.Now.Month);
                fr.SetValue("Nam", DateTime.Now.Year);
                fr.SetValue("TongTien", CommonFunction.DinhDangSo(Tien));
            }
            else
            {
                fr.SetValue("ThoiGian", "Từ ngày " + TuNgay + " - " + TuThang + " đến ngày " + DenNgay + " - " + DenThang);
                fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
                fr.SetValue("TenDV", tendv.ToUpper());
                fr.SetValue("Ngay", DateTime.Now.Day);
                fr.SetValue("Thang", DateTime.Now.Month);
                fr.SetValue("Nam", DateTime.Now.Year);
                fr.SetValue("sChungTuSai", sChungTuSai);
            }
            fr.Run(Result);
            return Result;

        }



        private DataTable CreatTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SoChungTu", typeof(string));
            dt.Columns.Add("NgayChungTu", typeof(string));
            dt.Columns.Add("NoiDung", typeof(string));
            dt.Columns.Add("SoTien", typeof(string));
            dt.Columns.Add("TaiKhoanNo", typeof(string));
            dt.Columns.Add("TaiKhoanCo", typeof(string));
            dt.Columns.Add("SoGhiSo", typeof(string));
            dt.Columns.Add("NgayGhiSo", typeof(string));
            dt.Columns.Add("GhiChu", typeof(string));
            dt.Columns.Add("DonVi", typeof(string));
            return dt;
        }
        private DataTable get_DanhSach_ChungTu(String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {

            String iTuNgay = iNamLamViec + "/" + TuThang + "/" + TuNgay;
            String iDenNgay = iNamLamViec + "/" + DenThang + "/" + DenNgay;
            if (Loai == "0")//ke chung tu sai
            {
                DataTable dt = null;
                SqlCommand cmd = new SqlCommand();
                String SQL = @"SELECT DISTINCT CT.sSoChungTuChiTiet, CT.iNgay, CT.iThang, CT.sNoiDung, CT.rSoTien, CT.iID_MaTaiKhoan_No, 
                        CT.iID_MaTaiKhoan_Co, CT.sSoChungTuGhiSo as sSoChungTu, CT.iNgayCT AS NgayGhiSo, CT.iThangCT AS ThangGhiSo, CT.iID_MaDonVi_Co, CT.iID_MaDonVi_No
FROM          dbo.KT_ChungTuChiTiet AS CT 
WHERE      (CT.iTrangThai = 1)";
                if (iTrangThai != "0") SQL += " AND (KT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet)";
                SQL += @" AND ((CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK1 where TK1.iTrangThai=1))  OR
                        (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK2 where TK2.iTrangThai=1)) OR
                        (CT.iThang <> CT.iThangCT) OR
                        (CT.iID_MaDonVi_No NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB1 WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec))OR
                        (CT.iID_MaDonVi_Co NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB2 WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)) OR
                        (CT.iID_MaTaiKhoan_No IS NULL) OR
                        (CT.iID_MaTaiKhoan_No = '') OR
                        (CT.iID_MaTaiKhoan_Co IS NULL) OR
                        (CT.iID_MaTaiKhoan_Co = '') OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1) AND TK3.iTrangThai=1)) OR
                        (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1) AND TK3.iTrangThai=1)) OR
                        ((CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2 WHERE TD2.iTrangThai=1 and TD2.iID_MaTaiKhoanNo IS NOT NULL)) AND (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1 WHERE TD1.iTrangThai=1 and TD1.iID_MaTaiKhoanCo IS NOT NULL))) OR
                        ((CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2  WHERE TD2.iTrangThai=1 and TD2.iID_MaTaiKhoanNo IS NOT NULL)) AND (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1  WHERE TD1.iTrangThai=1 and TD1.iID_MaTaiKhoanCo IS NOT NULL))))";
                if (String.IsNullOrEmpty(TuThang) == false && TuThang != "" && String.IsNullOrEmpty(DenThang) == false && DenThang != ""
                    && String.IsNullOrEmpty(TuNgay) == false && TuNgay != "" && String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
                {

                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, CT.iThang) + '/' + CONVERT(varchar, CT.iNgay), 111) >= @TuNgay)";
                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, CT.iThang) + '/' + CONVERT(varchar, CT.iNgay), 111) <= @DenNgay) AND (CT.iThang<>0)";
                    cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay));
                    cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay));

                }
                SQL += " ORDER BY CT.iThang, CT.iNgay";
                if (iTrangThai != "0")
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                DataTable dtMain = CreatTable();
                string GhiChu = "", TaiKhoanNo = "", TaiKhoanCo = "", DonViCo = "", DonViNo = "";

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DataRow drMain = dtMain.NewRow();
                        TaiKhoanNo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_No"]);
                        TaiKhoanCo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_Co"]);
                        DonViCo = HamChung.ConvertToString(dr["iID_MaDonVi_Co"]);
                        DonViNo = HamChung.ConvertToString(dr["iID_MaDonVi_No"]);
                       
                        drMain["SoChungTu"] = HamChung.ConvertToString(dr["sSoChungTuChiTiet"]);
                        drMain["NgayChungTu"] = HamChung.ConvertToString(dr["iNgay"]) + " - " + HamChung.ConvertToString(dr["iThang"]);
                        drMain["NoiDung"] = HamChung.ConvertToString(dr["sNoiDung"]);
                        drMain["SoTien"] = CommonFunction.DinhDangSo(HamChung.ConvertToString(dr["rSoTien"]));
                        drMain["TaiKhoanNo"] = TaiKhoanNo;
                        drMain["TaiKhoanCo"] = TaiKhoanCo;
                        drMain["SoGhiSo"] = HamChung.ConvertToString(dr["sSoChungTu"]);
                        drMain["NgayGhiSo"] = HamChung.ConvertToString(dr["NgayGhiSo"]) + " - " + HamChung.ConvertToString(dr["ThangGhiSo"]);
                        //lấy ghi chú
                        GhiChu = GetGhiChu(TaiKhoanCo, TaiKhoanNo, DonViNo, DonViCo);
                        if (HamChung.ConvertToString(dr["iThang"]) != HamChung.ConvertToString(dr["ThangGhiSo"])) GhiChu += " T";
                        if (TaiKhoanNo != "" && TaiKhoanCo != "" && TaiKhoanNo.Trim() == TaiKhoanCo.Trim()) GhiChu += " =";
                        if (TaiKhoanNo == "" && TaiKhoanCo == "") GhiChu += " ?";
                        drMain["GhiChu"] = GhiChu.Trim();
                        drMain["DonVi"] = DonViNo;
                        Tien += Convert.ToDecimal(HamChung.ConvertToString(dr["rSoTien"]));
                        dtMain.Rows.Add(drMain);
                    }
                }
                cmd.Dispose();
                if (dt != null) dt.Dispose();
                //Đếm số dòng
                Count = dtMain.Rows.Count.ToString();
                return dtMain;
            }
            else
            {
                DataTable dt = null;
                SqlCommand cmd = new SqlCommand();
                String SQL = @"SELECT sSoChungTuGhiSo, CONVERT(varchar, iNgayCT) + '/' + CONVERT(varchar, iThangCT) AS NgayCTGS, sSoChungTuChiTiet, CONVERT(varchar, iNgay) + '/' + CONVERT(varchar, iThang) AS NgayCT, iID_MaDonVi_No, 
iID_MaDonVi_Co, sNoiDung, rSoTien, iID_MaTaiKhoan_Co, iID_MaTaiKhoan_No FROM KT_ChungTuChiTiet WHERE 1=1";
                if (String.IsNullOrEmpty(TuThang) == false && TuThang != "" && String.IsNullOrEmpty(DenThang) == false && DenThang != ""
                   && String.IsNullOrEmpty(TuNgay) == false && TuNgay != "" && String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
                {

                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) >= @TuNgay)";
                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)";
                    cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay));
                    cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay));

                }
                if (iTrangThai != "0")
                {
                    SQL += " AND (iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet)";
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                }
                SQL += " ORDER BY iThang, iNgay";
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                return dt;
            }
        }
        private void LoadData(FlexCelReport fr, String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {

            DataTable data = get_DanhSach_ChungTu(iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        public clsExcelResult ExportToPDF(String iNamLamViec = "", String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            string PathFile = "";
            if (Loai == "0") PathFile = sFilePath;
            else PathFile = sFilePath_CTPS;
            ExcelFile xls = CreateReport(Server.MapPath(PathFile), iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
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
        public clsExcelResult ExportToExcel(String iNamLamViec = "", String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            string PathFile = "";
            if (Loai == "0") PathFile = sFilePath;
            else PathFile = sFilePath_CTPS;
            ExcelFile xls = CreateReport(Server.MapPath(PathFile), iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
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
        public ActionResult ViewPDF(String iNamLamViec = "", String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            HamChung.Language();
            string PathFile = "";
            if (Loai == "0") PathFile = sFilePath;
            else PathFile = sFilePath_CTPS;
            ExcelFile xls = CreateReport(Server.MapPath(PathFile), iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
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
        public String get_DanhSach_ChungTuThieu(String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String iTrangThai = "0")
        {
            String SQL = "",sDanhSach="";
            Boolean tg=false;
            SqlCommand cmd= new SqlCommand();
            String iTuNgay = iNamLamViec + "/" + TuThang + "/" + TuNgay;
            String iDenNgay = iNamLamViec + "/" + DenThang + "/" + DenNgay;
            //Lay so chung tu max
            SQL = String.Format(@"SELECT MAX(CAST (LTrim(RTrim(sSoChungTu)) as int))
                                    FROM KT_ChungTu
                                    WHERE iTrangThai=1  AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) >= @TuNgay)
                                         AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@TuNgay", iTuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", iDenNgay);
            int Max = Convert.ToInt16(Connection.GetValue(cmd, 0));
            //Lay so chung tu min
            SQL = String.Format(@"SELECT MIN(CAST (LTrim(RTrim(sSoChungTu)) as int))
                                    FROM KT_ChungTu
                                    WHERE iTrangThai=1  AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) >= @TuNgay)
                                         AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)");
            cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@TuNgay", iTuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", iDenNgay);
            int Min = Convert.ToInt16(Connection.GetValue(cmd, 0));
            //Lay Danh sach so chung tu
            SQL = String.Format(@"SELECT sSoChungTu
                                    FROM KT_ChungTu
                                    WHERE iTrangThai=1  AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) >= @TuNgay)
                                         AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)");
            cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@TuNgay", iTuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", iDenNgay);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            for (int i = Min+1; i < Max;i++)
            {
                tg = false;
                for (int j = 0;j< dt.Rows.Count; j++)
                {
                    if (i == Convert.ToInt16(dt.Rows[j]["sSoChungTu"]))
                        tg = true;
                }
                if (tg == false)
                    sDanhSach += i + ",";
            }
            if (String.IsNullOrEmpty(sDanhSach) == false) sDanhSach = "Các CTGS thiếu: " + sDanhSach.Substring(0, sDanhSach.Length - 1);
            return sDanhSach;
        }
    }
}
