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
    public class rptDoiChieuSuDungKinhPhiController : Controller
    {
        //
        // GET: /rptDoiChieuSuDungNganSach/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathTH = "/Report_ExcelFrom/KeToan/KhoBac/rptDoiChieuSuDungKinhPhi2.xls";
        private const String sFilePathTH1 = "/Report_ExcelFrom/KeToan/KhoBac/rptDoiChieuSuDungKinhPhi1.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptDoiChieuSuDungKinhPhi.aspx";
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
            String iID_MaNamNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaNamNganSach"]);
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao"]);
            String inmuc = Convert.ToString(Request.Form[ParentID + "_inmuc"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            String iID_MaNguonNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaNguonNganSach"]);
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
            // ViewData["LoaiBaoCao"] = LoaiBaoCao;
            ViewData["inmuc"] = inmuc;
            ViewData["iID_MaNguonNganSach"] = iID_MaNguonNganSach;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptDoiChieuSuDungKinhPhi.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
            // return RedirectToAction("Index", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, LoaiBaoCao = LoaiBaoCao, inmuc = inmuc, iID_MaNguonNganSach = iID_MaNguonNganSach, iID_MaNamNganSach = iID_MaNamNganSach, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaNguonNganSach, String iID_MaNamNganSach, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptDoiChieuSuDungKinhPhi");
            LoadData(fr, NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaNguonNganSach, iID_MaNamNganSach, iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            if (iID_MaNguonNganSach == Guid.Empty.ToString())
                fr.SetValue("Ten", "");
            else
                fr.SetValue("Ten", CommonFunction.LayTruong("NS_LoaiNganSach", "iID_MaLoaiNganSach", iID_MaNguonNganSach, "sTen"));
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("sC", "");
            fr.Run(Result);
            return Result;

        }
        public clsExcelResult ExportToPDF(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaNguonNganSach, String iID_MaNamNganSach, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = "";
            DuongDan = PathFile(inmuc, DuongDan);
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaNguonNganSach, iID_MaNamNganSach, iID_MaTrangThaiDuyet);
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
        /// <summary>
        /// Lấy đường dẫn file Excel
        /// </summary>
        /// <param name="inmuc"></param>
        /// <param name="DuongDan"></param>
        /// <returns></returns>
        private static String PathFile(String inmuc, String DuongDan)
        {
            if (inmuc == "1")
            {
                DuongDan = sFilePathTH;
            }
            else if (inmuc == "2")
            {
                DuongDan = sFilePathTH1;
            }
            return DuongDan;
        }
        public ActionResult ViewPDF(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaNguonNganSach, String iID_MaNamNganSach, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = "";
            DuongDan = PathFile(inmuc, DuongDan);
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaNguonNganSach, iID_MaNamNganSach, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaNguonNganSach, String iID_MaNamNganSach, String iID_MaTrangThaiDuyet)
        {
            String DuongDan = "";
            DuongDan = PathFile(inmuc, DuongDan);
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaNguonNganSach, iID_MaNamNganSach, iID_MaTrangThaiDuyet);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaNguonNganSach, String iID_MaNamNganSach, String iID_MaTrangThaiDuyet)
        {
            DataTable data = DoiChieuDuToan(NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaNguonNganSach, iID_MaNamNganSach, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TM", data, "sL,sK,sM,sTM", "sL,sK,sM,sTM");
            fr.AddTable("TM", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sL,sK,sM", "sL,sK,sM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "sL,sK", "sL,sK");
            fr.AddTable("Khoan", dtLoaiNS);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
        public DataTable DoiChieuDuToan(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaNguonNganSach, String iID_MaNamNganSach, String iID_MaTrangThaiDuyet)
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
            String DKThang = "";
            String DKThang1 = "";
            SqlCommand cmd = new SqlCommand();
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = " MONTH(dNgayDotPhanBo) in (1,2,3) ";
                        DKThang = "iThangCT between 1 and 3";
                        DKThang1 = "iThangCT between 1 and 3";
                        break;
                    case 2: DKThang_Quy = " MONTH(dNgayDotPhanBo) in (4,5,6) ";
                        DKThang = "iThangCT between 1 and 6";
                        DKThang1 = "iThangCT between 4 and 6";
                        break;
                    case 3: DKThang_Quy = " MONTH(dNgayDotPhanBo) in (7,8,9) ";
                        DKThang = "iThangCT between 1 and 9";
                        DKThang1 = "iThangCT between 7 and 9";
                        break;
                    case 4: DKThang_Quy = " MONTH(dNgayDotPhanBo) in (10,11,12) ";
                        DKThang = "iThangCT between 1 and 12";
                        DKThang1 = "iThangCT between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThang_Quy = " MONTH(dNgayDotPhanBo)=@ThangQuy ";
                DKThang = "iThangCT<=@ThangQuy";
                DKThang1 = "iThangCT=@ThangQuy";
            }

            String SQL = " SELECT PB.sL,PB.sK,PB.sM,PB.sTM,PB.iNamLamViec,PB.iID_MaNguonNganSach,sum(rDTRutLK) as rDTRutLK, sum(rSoTienLK) as rSoTienLK,SUM(rDTRut) as rDTRut,SUM(rSoTien) as rSoTien";
            SQL += " FROM(";
            SQL += " select sL,sK,sM,sTM,iNamLamViec,iID_MaNguonNganSach,rDTRutLK = case when {1} then SUM(rDTRut) else 0 end";
            SQL += " ,rSoTienLK = case when {1} then SUM(rSoTien) else 0 end";
            SQL += " ,rDTRut = case when {2} then SUM(rDTRut) else 0 end,rSoTien = case when {2} then SUM(rSoTien) else 0 end";
            SQL += " FROM KTKB_ChungTuChiTiet WHERE iID_MaNguonNganSach=@iID_MaLoaiNganSach and ";
            SQL += " iID_MaNamNganSach=@iID_MaNamNganSach {3} and iNamLamViec =@NamLamViec and iTrangThai=1";
            SQL += " GROUP By sL,sK,sM,sTM,iThangCT,iNamLamViec,iThangCT,iID_MaNguonNganSach)as PB ";
            SQL += " GROUP By PB.sL,PB.sK,PB.sM,PB.sTM,PB.iNamLamViec,PB.iID_MaNguonNganSach";
            SQL += " Having sum(rDTRutLK)+sum(rSoTienLK) +SUM(rDTRut) +SUM(rSoTien) <>0";

            SQL = String.Format(SQL, DKThang_Quy, DKThang, DKThang1, iID_MaTrangThaiDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaLoaiNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable DanhSach_inmuc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMuc", typeof(String));
            dt.Columns.Add("TenMuc", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "Mục";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "Tiểu mục";

            dt.Dispose();
            return dt;
        }
        public static DataTable Lay_LoaiNganSach()
        {
            String SQL = String.Format(@"SELECT * FROM NS_NguonNganSach
                                        ORDER BY iID_MaNguonNganSach");
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
        public string getTen(DataTable dt, String id)
        {
            String ten = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][id].Equals(id))
                    {
                        ten = dt.Rows[i]["sTen"].ToString();
                        break;
                    }
                }
            }
            return ten;
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