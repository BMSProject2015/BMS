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

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptTCDN_BaoCaoTaiChinhNamController : Controller
    {
        //
        // GET: /rptDT_2bc/

        //
        // GET: /DuToanChiNganSachSuDung/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhNam.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTaiChinhNam.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm lấy giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String To = Request.Form[ParentID + "_iTo"];
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["To"] = To;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTaiChinhNam.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BaoCaoTaiChinhNam");
            LoadData(fr, NamLamViec);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Nam", NamLamViec);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="MaKhoi"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_BaoCaoTaiChinhNam(String NamLamViec)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY sTenDoanhNghiep) AS iRow
                                              ,iID_MaDoanhNghiep,sTenDoanhNghiep,rVongDieuLe,rVonNhaNuoc,rTyLe
                                              ,SUM(rTongVon_ChuSoHuu) AS rTongVon_ChuSoHuu ,SUM(rVonDauTu_ChuSoHuu) AS rVonDauTu_ChuSoHuu
                                              ,SUM(rThangDu_ChuSoHuu) AS rThangDu_ChuSoHuu,SUM(rQuyDTPT_ChuSoHuu) AS rQuyDTPT_ChuSoHuu,SUM(rQuyDPPT_ChuSoHuu) AS rQuyDPPT_ChuSoHuu
                                              ,SUM(rChenhLechTyGia_ChuSoHuu) AS rChenhLechTyGia_ChuSoHuu,SUM(rLoiNhuanChuaPP_ChuSoHuu) AS rLoiNhuanChuaPP_ChuSoHuu,SUM(rVonKhac_ChuSoHuu) AS rVonKhac_ChuSoHuu
                                              ,SUM(rDoanhThu) AS rDoanhThu,SUM(rLoiNhuanTruocThue) AS rLoiNhuanTruocThue, SUM(rLoiNhuanSauThue) AS rLoiNhuanSauThue, SUM(rBangTien_VonNhaNuoc) AS rBangTien_VonNhaNuoc
                                              ,SUM(rBangCoPhieu_VonNhaNuoc) AS rBangCoPhieu_VonNhaNuoc, SUM(rCong_VonNhaNuoc) AS rCong_VonNhaNuoc, SUM(rNopNganSach) AS rNopNganSach, SUM(rLaoDongBinhQuan) AS rLaoDongBinhQuan
                                              ,SUM(rTongQuyLuong) AS rTongQuyLuong, SUM(rThuNhapBinhQuan) AS rThuNhapBinhQuan, SUM(rPhaiNop_VonNhaNuocKhiCoPhanHoa) AS rPhaiNop_VonNhaNuocKhiCoPhanHoa, SUM(rDaNop_VonNhaNuocKhiCoPhanHoa) AS rDaNop_VonNhaNuocKhiCoPhanHoa
                                              ,SUM(rConPhaiNop_VonNhaNuocKhiCoPhanHoa) AS rConPhaiNop_VonNhaNuocKhiCoPhanHoa, SUM(rCoTucNamTruoc_CoTuc) AS rCoTucNamTruoc_CoTuc, SUM(rCoTucNamNay_CoTuc) AS rCoTucNamNay_CoTuc
                                              ,SUM(rDaNop_CoTuc) AS rDaNop_CoTuc, SUM(rConPhaiNop_CoTuc) AS rConPhaiNop_CoTuc, SUM(rTienDatChuaNop_TienThueDat) AS rTienDatChuaNop_TienThueDat, SUM(rTienDatNamNay_TienThueDat) AS rTienDatNamNay_TienThueDat
                                              ,SUM(rDaNop_TienThueDat) AS rDaNop_TienThueDat, SUM(rConPhaiNop_TienThueDat) AS rConPhaiNop_TienThueDat, SUM(rTongSoConPhaiNop) AS rTongSoConPhaiNop, SUM(rTongSoDaNop) AS rTongSoDaNop
                                          FROM TCDN_BaoCaoTaiChinh
                                        WHERE TCDN_BaoCaoTaiChinh.iTrangThai=1 
                                        AND iNamLamViec=@iNamLamViec
                                        GROUP BY iID_MaDoanhNghiep,sTenDoanhNghiep,rVongDieuLe,rVonNhaNuoc,rTyLe
                                        ORDER BY sTenDoanhNghiep");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dt = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec)
        {
            DataTable data =DT_BaoCaoTaiChinhNam(NamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            //DataTable dtTieuMuc;
            //dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa", "sTM,sTTM");
            //fr.AddTable("TieuMuc", dtTieuMuc);
            //DataTable dtMuc;
            //dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sM", "sM,sMoTa", "sM,sTM");
            //fr.AddTable("Muc", dtMuc);
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public clsExcelResult ExportToExcel(String NamLamViec, String To)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String xfilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhNam" + To + ".xls";
            ExcelFile xls = CreateReport(Server.MapPath(xfilePath), NamLamViec);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTChiNganSachQuocPhong.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec,String To)
        {
            String xfilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhNam" + To + ".xls";
            ExcelFile xls = CreateReport(Server.MapPath(xfilePath), NamLamViec);
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
       
    }
}
