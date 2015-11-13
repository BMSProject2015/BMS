
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
    public class rptKTTongHop_ChiTiet_TongHopTaiKhoanController : Controller
    {
        //
        // GET: /rptKTTongHop_ChiTiet_TongHopTaiKhoan/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan.xls";
        public static String NameFile = "";

        public ActionResult Loc()
        {

            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan_Loc.aspx";
            return View("~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan_Loc.aspx");
        }
        public ActionResult Index()
        {           
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan_Loc.aspx";
                return View("~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan_Loc.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult Loc_Submit(String ParentID, String iNamLamViec)
        {
            String iID_MaTaiKhoan = Convert.ToString(Request.Form["id_MaTaiKhoan"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String LoaiBaoCao = Request.Form[ParentID + "_iLoaiBaoCao"];
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan.aspx";
            ViewData["DSMaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iThang1"] = iThang1;
            ViewData["iThang2"] = iThang2;
            ViewData["iNgay1"] = iNgay1;
            ViewData["iNgay2"] = iNgay2;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["ChiSo"] = 0;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            if (String.IsNullOrEmpty(iID_MaTaiKhoan)) return RedirectToAction("Index");
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, int ChiSo, String LoaiBaoCao)
        {
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTiet_TongHopTaiKhoan.aspx";
            ViewData["DSMaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iThang1"] = iThang1;
            ViewData["iThang2"] = iThang2;
            ViewData["iNgay1"] = iNgay1;
            ViewData["iNgay2"] = iNgay2;
            ViewData["ChiSo"] = ChiSo;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iID_MaTaiKhoan, String UserName)
        {
            XlsFile Result = new XlsFile(true);
            //Result = TaoTieuDe(Result);
            // DataTable data = dt_PhanHo(iThang, iID_MaTaiKhoan, UserName);
            //FillData(Result, data, 3, 4, 4, 4, "");
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTongHop_ChiTiet_TongHopTaiKhoan");
            DataTable dtNamLamViec = NguoiDungCauHinhModels.LayCauHinh(UserName);
            LoadData(fr, iNamLamViec, iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iID_MaTaiKhoan, UserName);
            String TieuDeBaoCao = "SỔ CHI TIẾT";
            if (LoaiBaoCao == "1")
            {
                TieuDeBaoCao = "SỔ TỔNG HỢP";
            }
            String TenTaiKhoan = "";
            if (!String.IsNullOrEmpty(iID_MaTaiKhoan))
            {
                TenTaiKhoan =iID_MaTaiKhoan+" - "+ Convert.ToString(CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", iID_MaTaiKhoan, "sTen"));
            }
            fr.SetValue("TieuDeBaoCao", TieuDeBaoCao);
            fr.SetValue("Nam", dtNamLamViec.Rows[0]["iNamLamViec"]);
            fr.SetValue("Thang1", iThang1);
            fr.SetValue("Thang2", iThang2);
            fr.SetValue("iNgay1", iNgay1);
            fr.SetValue("iNgay2", iNgay2);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("iNamLamViec", iNamLamViec);
            fr.SetValue("TaiKhoan", TenTaiKhoan);
            fr.Run(Result);
            return Result;

        }
        public clsExcelResult ExportToPDF(String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iID_MaTaiKhoan, String UserName)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iID_MaTaiKhoan, UserName);
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
        public clsExcelResult ExportToExcel(String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iID_MaTaiKhoan, String UserName)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iID_MaTaiKhoan, UserName);
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
        public ActionResult ViewPDF(String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iID_MaTaiKhoan, String UserName)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iNgay1, iNgay2, iThang1, iThang2, LoaiBaoCao, iID_MaTaiKhoan, UserName);

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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, String LoaiBaoCao, String iID_MaTaiKhoan, String UserName)
        {
            DataTable data = dtChiTiet(iNamLamViec, iNgay1, iNgay2, iThang1, iThang2, iID_MaTaiKhoan, UserName);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        public DataTable dtChiTiet(String iNamLamViec, String iNgay1, String iNgay2, String iThang1, String iThang2, String iID_MaTaiKhoan, String CapTaiKhoan)
        {
            String strSQL;
            int NamTruoc = Convert.ToInt16(iNamLamViec) - 1;
            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            String DK_Co = "", DK_No = "";
            String[] arrDSTaiKhoan = iID_MaTaiKhoan.Split(',');
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

            if (String.IsNullOrEmpty(iID_MaTaiKhoan)) iID_MaTaiKhoan = "100000";



            String SQL = " SELECT CT.iNgayCT,Convert(varchar,CT.iNgayCT) + ' - ' + Convert(varchar,CT.iThang) as NgayGS";
            SQL += ",sSoChungTu,Convert(varchar,CT.iNgayCT) + ' - ' + Convert(varchar,CT.iThangCT) as NgayCT,sSoChungTuChiTiet";
            SQL += ",iID_MaTaiKhoan_Co as iID_MaTaiKhoan ";
            SQL += ",iID_MaTaiKhoan_No as TKDoiUng ";
            SQL += ",iID_MaTaiKhoan_Cha";
            SQL += ",CT.sNoiDung";
            SQL += ",MoTa=''";
            SQL += ",bLaHangCha=0";
            SQL += ",rSoPhatSinhNo=0.0,rSoTien as rSoPhatSinhCo ,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co,iID_MaDonVi_Co,iID_MaDonVi_No ";
            SQL += " FROM KT_ChungTuChiTiet AS CT";
            SQL += " INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu=CT.iID_MaChungTu";
            SQL += " INNER JOIN KT_TaiKhoan ON CT.iID_MaTaiKhoan_Co=KT_TaiKhoan.iID_MaTaiKhoan";
            SQL += " WHERE CT.iTrangThai=1 AND CT.iThangCT > 0 ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) >= @TuNgay)";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay)";
            SQL += " AND CT.iID_MaTrangThaiDuyet='{2}'";
            SQL += " {0} ";
            SQL += " UNION	";
            SQL += " SELECT CT.iNgayCT,Convert(varchar,CT.iNgayCT) + ' - ' + Convert(varchar,CT.iThang) as NgayGS";
            SQL += ",sSoChungTu,Convert(varchar,CT.iNgayCT) +' - ' + Convert(varchar,CT.iThangCT) as NgayCT,sSoChungTuChiTiet";
            SQL += ",iID_MaTaiKhoan_No as iID_MaTaiKhoan ";
            SQL += ",iID_MaTaiKhoan_Co as TKDoiUng ";
            SQL += ",iID_MaTaiKhoan_Cha";
            SQL += ",CT.sNoiDung";
            SQL += ",MoTa=''";
            SQL += ",bLaHangCha=0";
            SQL += ",rSoTien AS rSoPhatSinhNo,rSoPhatSinhCo=0.0 ,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, iID_MaDonVi_Co,iID_MaDonVi_No";
            SQL += " FROM KT_ChungTuChiTiet AS CT";
            SQL += " INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu=CT.iID_MaChungTu";
            SQL += " INNER JOIN KT_TaiKhoan ON CT.iID_MaTaiKhoan_No=KT_TaiKhoan.iID_MaTaiKhoan";
            SQL += " WHERE CT.iTrangThai=1 AND CT.iThangCT > 0 ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) >= @TuNgay)";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay)";
            SQL += " {1} ";
            SQL += " AND CT.iID_MaTrangThaiDuyet='{2}'";
            SQL += " ORDER BY iID_MaTaiKhoan,CT.iNgayCT ";
            SQL = String.Format(SQL, DK_Co, DK_No, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int ThangCuoiCuaNamTruoc = LayThangCuoiCuaNam(NamTruoc);
            Boolean CoDuLieuNamTruoc = true;
            if (ThangCuoiCuaNamTruoc == 0)
            {
                CoDuLieuNamTruoc = false;
                ThangCuoiCuaNamTruoc = 12;
            }

            String TuNgayNamTruoc = NamTruoc.ToString() + "/" + ThangCuoiCuaNamTruoc.ToString() + "/01";
            DataTable dtThangKhong = LayDuLieuThangKhong(iNamLamViec, arrDSTaiKhoan);
            DataTable dtDauKy = GET_dtDauKy_CuoiKy(TuNgay, TuNgay, arrDSTaiKhoan, dtThangKhong, CoDuLieuNamTruoc, iNamLamViec);
            dtDauKy = KeToanTongHopModels.DienTaiKhoanCha(dtDauKy, iNamLamViec, "iID_MaTaiKhoan");
            DataTable dtLuyKeCuoiKy = GET_dtDauKy_CuoiKy(DenNgay, DenNgay, arrDSTaiKhoan, dtThangKhong, CoDuLieuNamTruoc,iNamLamViec);
            dtLuyKeCuoiKy = KeToanTongHopModels.DienTaiKhoanCha(dtLuyKeCuoiKy, iNamLamViec, "iID_MaTaiKhoan");

            DataTable dtCPS = Get_dtCongPhatSing(TuNgay, DenNgay, arrDSTaiKhoan);
            dtCPS = KeToanTongHopModels.DienTaiKhoanCha(dtCPS, iNamLamViec, "iID_MaTaiKhoan");

            DataTable dtLuyKe = Get_dtLuyKe(TuNgay, DenNgay, arrDSTaiKhoan,iNamLamViec);
            dtLuyKe = KeToanTongHopModels.DienTaiKhoanCha(dtLuyKe, iNamLamViec, "iID_MaTaiKhoan");
            DataTable dtKQ = Insert_TaiKhoanCha(arrDSTaiKhoan, iNamLamViec, dt, dtDauKy, dtLuyKeCuoiKy, dtCPS, dtLuyKe);
            dt.Dispose();
            cmd.Dispose();
            return dtKQ;
        }
        /// <summary>
        /// Lấy dt đầu kỳ và cuối kỳ chỉ cần thay đổi tham số đến ngày
        /// </summary>
        /// <param name="TuNgay">Từ ngày 1 Tháng cuối phát sinh của năm trước</param>
        /// <param name="DenNgay">Từ ngày của năm nay là lấy dtDauKy;=Đến ngày của năm nay là dtCuoiKy</param>
        /// <param name="DK_No"></param>
        /// <param name="DK_Co"></param>
        /// <returns></returns>
        public static DataTable GET_dtDauKy_CuoiKy(String TuNgay, String DenNgay, String[] arrDSTaiKhoan, DataTable dtThangKhong, Boolean CoDuLieuNamTruoc = true,String iNamLamViec="")
        {
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

            String strSQL = "";
            String SQL_LuyKe_DauKy = "SELECT iID_MaTaiKhoan_No as iID_MaTaiKhoan ,SUM(rPS_No) AS rLK_No,SUM(rPS_Co) AS rLK_Co";
            SQL_LuyKe_DauKy += ",rCK_No=CASE WHEN SUM(rPS_No)-SUM(rPS_Co)>0 THEN SUM(rPS_No)-SUM(rPS_Co) ELSE 0 END";
            SQL_LuyKe_DauKy += ",rCK_Co=CASE WHEN SUM(rPS_No)-SUM(rPS_Co)<0 THEN SUM(rPS_Co)-SUM(rPS_No) ELSE 0 END";
            SQL_LuyKe_DauKy += " FROM (";
            SQL_LuyKe_DauKy += " SELECT ";
            SQL_LuyKe_DauKy += " iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co=iID_MaTaiKhoan_No,SUM(rSoTien) as rPS_No,rPS_Co=0.0";
            SQL_LuyKe_DauKy += " FROM KT_ChungTuChiTiet";
            SQL_LuyKe_DauKy += " WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND (  iThangCT=0 OR (iThangCT>0 ";
            SQL_LuyKe_DauKy += " AND (CONVERT(Datetime, CONVERT(varchar, KT_ChungTuChiTiet.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @TuNgay)";
            SQL_LuyKe_DauKy += " AND (CONVERT(Datetime, CONVERT(varchar, KT_ChungTuChiTiet.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgay)  )) AND iID_MaTrangThaiDuyet={2} AND iID_MaTaiKhoan_No <> ''";
            SQL_LuyKe_DauKy += " {0}";
            SQL_LuyKe_DauKy += " GROUP BY iID_MaTaiKhoan_No";
            SQL_LuyKe_DauKy += " UNION";
            SQL_LuyKe_DauKy += " SELECT ";
            SQL_LuyKe_DauKy += " iID_MaTaiKhoan_No=iID_MaTaiKhoan_Co,iID_MaTaiKhoan_Co,rPS_No=0.0,SUM(rSoTien) as rPS_Co";
            SQL_LuyKe_DauKy += " FROM KT_ChungTuChiTiet";
            SQL_LuyKe_DauKy += " WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND (  iThangCT=0 OR( iThangCT>0 ";
            SQL_LuyKe_DauKy += " AND (CONVERT(Datetime, CONVERT(varchar, KT_ChungTuChiTiet.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @TuNgay)";
            SQL_LuyKe_DauKy += " AND (CONVERT(Datetime, CONVERT(varchar, KT_ChungTuChiTiet.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgay) )) AND iID_MaTrangThaiDuyet={2} AND iID_MaTaiKhoan_Co <> ''";
            SQL_LuyKe_DauKy += " {1}";
            SQL_LuyKe_DauKy += " GROUP BY iID_MaTaiKhoan_Co";
            SQL_LuyKe_DauKy += " ) KT";
            SQL_LuyKe_DauKy += " GROUP BY iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co";
            strSQL = String.Format(SQL_LuyKe_DauKy, DK_No, DK_Co, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            cmd.CommandText = strSQL;

            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            //if (CoDuLieuNamTruoc == false)
            //{
            //    String strSelect = "", TK = "";
            //    DataRow[] arrR;
            //    DataRow R;
            //    for (int i = 0; i < dtThangKhong.Rows.Count; i++)
            //    {
            //        TK = Convert.ToString(dtThangKhong.Rows[i]["sTKNo"]);
            //        strSelect = "iID_MaTaiKhoan ='" + TK.Trim() + "'";
            //        arrR = dt.Select(strSelect);
            //        if (arrR.Length > 0)
            //        {
            //            Double rCK_No = 0, rCK_Co = 0, rCK_No_ThangKo = 0, rCK_Co_ThangKo = 0, Tong = 0; ;
            //            if (dtThangKhong.Rows[i]["rCK_No"] != DBNull.Value)
            //                rCK_No_ThangKo = Convert.ToDouble(dtThangKhong.Rows[i]["rCK_No"]);
            //            if (dtThangKhong.Rows[i]["rCK_Co"] != DBNull.Value)
            //                rCK_Co_ThangKo = Convert.ToDouble(dtThangKhong.Rows[i]["rCK_Co"]);
            //            if (arrR[0]["rCK_No"] != DBNull.Value)
            //                rCK_No = Convert.ToDouble(arrR[0]["rCK_No"]);
            //            if (arrR[0]["rCK_Co"] != DBNull.Value)
            //                rCK_Co = Convert.ToDouble(arrR[0]["rCK_Co"]);

            //            Tong = rCK_No + rCK_No_ThangKo - rCK_Co - rCK_Co_ThangKo;
            //            if (Tong > 0)
            //            {
            //                dt.Rows[i]["rCK_No"] = Tong;
            //                dt.Rows[i]["rCK_Co"] = 0;
            //            }
            //            else
            //            {
            //                dt.Rows[i]["rCK_No"] = 0;
            //                dt.Rows[i]["rCK_Co"] = Tong * (-1);
            //            }
            //        }
            //        else
            //        {

            //            R = dt.NewRow();
            //            R["rCK_No"] = dtThangKhong.Rows[i]["rCK_No"];
            //            R["rCK_Co"] = dtThangKhong.Rows[i]["rCK_Co"];
            //            R["iID_MaTaiKhoan"] = dtThangKhong.Rows[i]["sTKNo"];
            //            dt.Rows.Add(R);
            //        }
            //    }
            //}

            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtCongPhatSing(String TuNgay, String DenNgay, String[] arrDSTaiKhoan)
        {
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
            String strSQL = "";
            String SQL = " SELECT iID_MaTaiKhoan,SUM(rCPS_No) AS rCPS_No, SUM(rCPS_Co) AS rCPS_Co";
            SQL += " FROM(";
            SQL += " SELECT iID_MaTaiKhoan_Co aS iID_MaTaiKhoan,SUM(rSoTien) as rCPS_Co,rCPS_No=0.0";
            SQL += " FROM KT_ChungTuChiTiet AS CT  ";
            SQL += " WHERE CT.iTrangThai=1   ";
            SQL += " AND iThangCT>0";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) >= @TuNgay) ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay) ";
            SQL += "  {0} AND CT.iID_MaTrangThaiDuyet={2}   ";
            SQL += " GROUP BY iID_MaTaiKhoan_Co";
            SQL += " UNION";
            SQL += " SELECT iID_MaTaiKhoan_No aS iID_MaTaiKhoan,rCPS_Co=0.0,SUM(rSoTien) as rCPS_No";
            SQL += " FROM KT_ChungTuChiTiet AS CT  ";
            SQL += " WHERE CT.iTrangThai=1   ";
            SQL += " AND iThangCT>0";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) >= @TuNgay) ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay) ";
            SQL += " {1} AND CT.iID_MaTrangThaiDuyet={2} ";
            SQL += " GROUP BY iID_MaTaiKhoan_No ";
            SQL += " ) KT";
            SQL += " GROUP BY iID_MaTaiKhoan";
            SQL += " ORDER BY iID_MaTaiKhoan";
            strSQL = String.Format(SQL, DK_Co, DK_No, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));

            cmd.CommandText = strSQL;
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_dtLuyKe(String TuNgay, String DenNgay, String[] arrDSTaiKhoan,String iNamLamViec)
        {
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
            String strSQL = "";
            String SQL = " SELECT iID_MaTaiKhoan,SUM(rCPS_No) AS rCPS_No, SUM(rCPS_Co) AS rCPS_Co";
            SQL += " FROM(";
            SQL += " SELECT iID_MaTaiKhoan_Co aS iID_MaTaiKhoan,SUM(rSoTien) as rCPS_Co,rCPS_No=0.0";
            SQL += " FROM KT_ChungTuChiTiet AS CT  ";
            SQL += " WHERE CT.iTrangThai=1   ";
            SQL += " AND iThangCT>0 AND iNamLamViec=@iNamLamViec ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay) ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay) ";
            SQL += "  {0} AND CT.iID_MaTrangThaiDuyet={2}   ";
            SQL += " GROUP BY iID_MaTaiKhoan_Co";
            SQL += " UNION";
            SQL += " SELECT iID_MaTaiKhoan_No aS iID_MaTaiKhoan,rCPS_Co=0.0,SUM(rSoTien) as rCPS_No";
            SQL += " FROM KT_ChungTuChiTiet AS CT  ";
            SQL += " WHERE CT.iTrangThai=1   ";
            SQL += " AND iThangCT>0 AND iNamLamViec=@iNamLamViec ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay) ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, CT.iNgayCT), 111) <= @DenNgay) ";
            SQL += " {1} AND CT.iID_MaTrangThaiDuyet={2} ";
            SQL += " GROUP BY iID_MaTaiKhoan_No ";
            SQL += " ) KT";
            SQL += " GROUP BY iID_MaTaiKhoan";
            SQL += " ORDER BY iID_MaTaiKhoan";
            strSQL = String.Format(SQL, DK_Co, DK_No, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));

            cmd.CommandText = strSQL;
           // cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable LayDuLieuThangKhong(String iNamLamViec, String[] arrDSTaiKhoan)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            for (int i = 0; i < arrDSTaiKhoan.Length; i++)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " OR ";
                DK += " sTKNo LIKE @sTKNo" + i.ToString();
                cmd.Parameters.AddWithValue("@sTKNo" + i.ToString(), arrDSTaiKhoan[i] + "%");
            }
            if (String.IsNullOrEmpty(DK) == false) DK = " AND " + DK;
            String SQL = String.Format("SELECT * FROM KT_LuyKe WHERE iThang=0 AND iNam=@iNam {0}", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static int LayThangCuoiCuaNam(int iNam)
        {
            String SQL = String.Format("SELECT MAX(iThang) FROM KT_LuyKe WHERE iNam={0}", iNam);
            int vR = Convert.ToInt16(Connection.GetValue(SQL, 0));
            return vR;
        }

        public static DataTable Insert_TaiKhoanCha(String[] arrDSTaiKhoan, String iNamLamViec, DataTable dtChiTiet, DataTable dtDuDauKy, DataTable dtCuoiKy, DataTable dtCPS,DataTable dtLuyKe)
        {
            String vR = "", DK = "";
            String SQL = "SELECT iID_MaTaiKhoan,sTen,iID_MaTaiKhoan_Cha as sTKCha,bLaHangCha FROM KT_TaiKhoan WHERE iNam=@iNam {0} ORDER BY iID_MaTaiKhoan";

            SqlCommand cmd = new SqlCommand();
            int i = 0;
            for (i = 0; i < arrDSTaiKhoan.Length; i++)
            {
                DK += " iID_MaTaiKhoan LIKE @iID_MaTaiKhoan" + i.ToString();
                if (i < arrDSTaiKhoan.Length - 1) DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i.ToString(), arrDSTaiKhoan[i] + "%");

            }
            if (String.IsNullOrEmpty(DK) == false) DK = " AND (" + DK + ")";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
            DataTable dtTaiKhoan = Connection.GetDataTable(cmd);
            cmd.Dispose();

            DataTable dtKQ = new DataTable();
            for (int c = 0; c < dtChiTiet.Columns.Count; c++)
            {
                dtKQ.Columns.Add(dtChiTiet.Columns[c].ColumnName, dtChiTiet.Columns[c].DataType);
            }
            dtKQ.Columns.Add("CS", typeof(int));


            String TK = "", TK_Cha = "", TK_Con = "", TK_KeTiep = "", strTruyVan = "";
            DataRow R, R1, RTK, RCT;
            DataRow[] arrR, Rdk;
            int d = 0;
            Object objPS_No = null, objPS_Co = null;
            Boolean Co_DuDauKy = false, Co_PhatSinh = false;
            for (i = 0; i < dtTaiKhoan.Rows.Count; i++)
            {
                Co_DuDauKy = false; Co_PhatSinh = false;
                RTK = dtTaiKhoan.Rows[i];
                TK = Convert.ToString(dtTaiKhoan.Rows[i]["iID_MaTaiKhoan"]);
                Rdk = dtDuDauKy.Select("iID_MaTaiKhoan='" + TK + "'");
                if (Rdk.Length > 0)
                {
                    Co_DuDauKy = true;
                }
                Rdk = dtCPS.Select("iID_MaTaiKhoan='" + TK + "'");
                if (Rdk.Length > 0)
                {
                    Co_PhatSinh = true;
                }
                if (Convert.ToBoolean(RTK["bLaHangCha"]) == false)
                {
                    // Nếu có số dư đầu kỳ hay phát sinh thì mới add tài khoản.
                    //Số dư đầu kỳ
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtDuDauKy.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCK_No"];
                        objPS_Co = Rdk[0]["rCK_Co"];
                        dtDuDauKy.Rows.Remove(Rdk[0]);
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_DK", TK, TK, TK + "-" + Convert.ToString(RTK["sTen"]), "Số dư đầu kỳ:", 1, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }
                    // Add chi tiết
                    objPS_No = 0;
                    objPS_Co = 0;
                    String select = "iID_MaTaiKhoan ='" + TK + "'";
                    arrR = dtChiTiet.Select(select);
                    for (int j = 0; j < arrR.Length; j++)
                    {
                        dtKQ.Rows.Add(arrR[j].ItemArray);
                        dtChiTiet.Rows.Remove(arrR[j]);
                    }

                    //Cộng phát sinh
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtCPS.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCPS_No"];
                        objPS_Co = Rdk[0]["rCPS_Co"];
                        dtCPS.Rows.Remove(Rdk[0]);
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_CPS", TK, TK, "", "Cộng phát sinh:", 2, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }
                    //Lũy kế
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtLuyKe.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCPS_No"];
                        objPS_Co = Rdk[0]["rCPS_Co"];
                        dtLuyKe.Rows.Remove(Rdk[0]);
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_LK", TK, TK, "", "Lũy kế đến kỳ:", 4, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }
                    //Số dư cuối kỳ
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtCuoiKy.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0 )
                    {
                        objPS_No = Rdk[0]["rCK_No"];
                        objPS_Co = Rdk[0]["rCK_Co"];
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_CK", TK, TK, "", "Số dư cuối kỳ:", 3, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }
                    
                }
                else
                {
                    // Nếu có số dư đầu kỳ hay phát sinh thì mới add tài khoản.
                    // Số dư đầu kỳ
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtDuDauKy.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCK_No"];
                        objPS_Co = Rdk[0]["rCK_Co"];
                        dtDuDauKy.Rows.Remove(Rdk[0]);
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_DK", TK, TK, TK + "-" + Convert.ToString(RTK["sTen"]), "Số dư đầu kỳ:", 1, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }
                    //Cộng phát sinh
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtCPS.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCPS_No"];
                        objPS_Co = Rdk[0]["rCPS_Co"];
                        dtCPS.Rows.Remove(Rdk[0]);
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_CPS", TK, TK, "", "Cộng phát sinh:", 2, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }

                    //Lũy kế
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtLuyKe.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCPS_No"];
                        objPS_Co = Rdk[0]["rCPS_Co"];
                        dtLuyKe.Rows.Remove(Rdk[0]);
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_LK", TK, TK, "", "Lũy kế đến kỳ:", 4, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }

                    //Số dư cuối kỳ
                    objPS_No = 0;
                    objPS_Co = 0;
                    Rdk = dtCuoiKy.Select("iID_MaTaiKhoan='" + TK + "'");
                    if (Rdk.Length > 0)
                    {
                        objPS_No = Rdk[0]["rCK_No"];
                        objPS_Co = Rdk[0]["rCK_Co"];
                    }
                    if (Co_PhatSinh || Co_DuDauKy)
                    {
                        R = dtKQ.NewRow();
                        dtKQ.Rows.Add(AddRow(R, "_CK", TK, TK, "", "Số dư cuối kỳ:", 3, d, objPS_No, objPS_Co));
                        d = d + 1;
                    }

                  
                }
            }



            dtChiTiet.Dispose();
            dtTaiKhoan.Dispose();

            return dtKQ;
        }

        public static DataRow AddRow(DataRow R, String TenTruong, String iID_MaTaiKhoan, String iID_MaTaiKhoan_Cha, String sNoiDung, String MoTa, int bLaHangCha, int CS, Object rSoPhatSinhNo, Object rSoPhatSinhCo)
        {
            R["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            R["iID_MaTaiKhoan_Cha"] = iID_MaTaiKhoan_Cha + TenTruong; ;
            R["sNoiDung"] = sNoiDung;
            R["MoTa"] = MoTa;
            R["bLaHangCha"] = bLaHangCha;
            R["CS"] = CS;
            if (String.IsNullOrEmpty(HamChung.ConvertToString(rSoPhatSinhNo)) == true)
                //if (rSoPhatSinhNo == null)
                rSoPhatSinhNo = 0;
            R["rSoPhatSinhNo"] = rSoPhatSinhNo;
            if (String.IsNullOrEmpty(HamChung.ConvertToString(rSoPhatSinhCo)) == true)
                // if (rSoPhatSinhCo == null)
                rSoPhatSinhCo = 0;
            R["rSoPhatSinhCo"] = rSoPhatSinhCo;

            return R;
        }

        public static DataTable TaiKhoan(int CapTK = 3,String TuThang="1",String DenThang="1",String iNamLamViec="2012")
        {
            String DKTK = String.Format(" AND iTrangThai=1 AND iNamLamViec={0} AND iThangCT >= {1} AND iThangCT <= {2} ",iNamLamViec, TuThang, DenThang);
            String SQL_No = String.Format("SELECT Distinct(SubString(iID_MaTaiKhoan_No,1,3)) AS TK FROM KT_ChungTuChiTiet WHERE iID_MaTaiKhoan_No<>'' {0}", DKTK);
            DataTable dt_TK_No = Connection.GetDataTable(SQL_No);
            String SQL_Co = String.Format("SELECT Distinct(SubString(iID_MaTaiKhoan_Co,1,3)) AS TK FROM KT_ChungTuChiTiet WHERE iID_MaTaiKhoan_Co<>'' {0}", DKTK);
            DataTable dt_TK_Co = Connection.GetDataTable(SQL_Co);

            String TK = "";
            String DK = "";
            for (int i = 0; i < dt_TK_No.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(DK) == false)
                    DK += " OR ";
                TK = Convert.ToString(dt_TK_No.Rows[i]["TK"]);
                DK += " iID_MaTaiKhoan LIKE '" + TK + "%'";
            }
            for (int i = 0; i < dt_TK_Co.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(DK) == false)
                    DK += " OR ";
                TK = Convert.ToString(dt_TK_Co.Rows[i]["TK"]);
                DK += " iID_MaTaiKhoan LIKE '" + TK + "%'";
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = String.Format(" WHERE iTrangThai=1 AND iNam={0} AND (" + DK + ")",iNamLamViec);
            }
            else
            {
                DK = " WHERE 1=2";
            }


            String SQL = "select iID_MaTaiKhoan,sTen";
            SQL += " from KT_TaiKhoan";
            SQL += " {0}";
            SQL += " group by iID_MaTaiKhoan,sTen ";


            if (CommonFunction.IsNumeric(CapTK))
            {
                DK += String.Format(" AND LEN(KT_TaiKhoan.iID_MaTaiKhoan) <= {0} ", CapTK);
            }
            SQL = String.Format(SQL, DK);
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }

        public JsonResult Get_objNgayThang(String ParentID, String TenTruong, String Ngay, String Thang, String iNam)
        {
            return Json(get_sNgayThang(ParentID, TenTruong, Ngay, Thang, iNam), JsonRequestBehavior.AllowGet);
        }
        public string get_sNgayThang(String ParentID, String TenTruong, String Ngay, String Thang, String iNam)
        {
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(Thang), Convert.ToInt16(iNam), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            int SoNgayTrongThang = DateTime.DaysInMonth(Convert.ToInt16(iNam), Convert.ToInt16(Thang));
            if (String.IsNullOrEmpty(Ngay) == false)
            {
                if (Convert.ToInt16(Ngay) > SoNgayTrongThang)
                    Ngay = "1";
            }
            return MyHtmlHelper.DropDownList(ParentID, slNgay, Ngay, TenTruong, "", "style=\"width:60px\"");
        }

        public JsonResult Get_objTaiKhoan(String CapTK,String TuThang,String DenThang,String iNamLamViec)
        {
            return Json(sDanhSachTaiKhoan(CapTK,TuThang,DenThang,iNamLamViec), JsonRequestBehavior.AllowGet);
        }
        public string sDanhSachTaiKhoan(String CapTK, String TuThang, String DenThang, String iNamLamViec)
        {
            DataTable dtTaiKhoan = TaiKhoan(Convert.ToInt16(CapTK),TuThang,DenThang,iNamLamViec);
            StringBuilder stb = new StringBuilder();
            stb.Append("<table  class=\"mGrid\">");
            //stb.Append("<tr>");
            //stb.Append("<th align=\"center\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAll(this.checked)\" /></th>");
            //stb.Append("<th></th>");
            //stb.Append("</tr>");

            String strsTen = "", MaTaiKhoan = "";
            for (int i = 0; i < dtTaiKhoan.Rows.Count; i++)
            {
                strsTen = Convert.ToString(dtTaiKhoan.Rows[i]["sTen"]);
                MaTaiKhoan = Convert.ToString(dtTaiKhoan.Rows[i]["iID_MaTaiKhoan"]);
                stb.Append("<tr>");
                stb.Append("<td align=\"center\" style=\"width: 25px;\">");
                stb.Append("<input type=\"checkbox\" value=\"" + MaTaiKhoan + "\" check-group=\"iID_MaTaiKhoan\" id=\"id_MaTaiKhoan\" name=\"id_MaTaiKhoan\" />");
                stb.Append("</td>");
                stb.Append("<td align=\"left\">" + MaTaiKhoan + "-" + strsTen);
                stb.Append("</td>");
                stb.Append("</tr>");
            }
            stb.Append("</table>");
            return stb.ToString();
        }
    }
}