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
    public class rptTCDN_BaoCaoTaiChinhQuyController : Controller
    {
        //
        // GET: /rptDT_2bc/

        //
        // GET: /rptTCDN_BaoCaoTaiChinhQuy/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhQuy.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTaiChinhQuy.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult Reports(String iNam, String iQuy)
        {
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = iNam;
            ViewData["Quy"] = iQuy;
            ViewData["To"] = 1;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTaiChinhQuy.aspx";
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
            String Quy = Request.Form[ParentID + "_iQuy"];
            String To = Request.Form[ParentID + "_iTo"];
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["Quy"] = Quy;
            ViewData["To"] = To;
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTaiChinhQuy.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String Quy)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BaoCaoTaiChinhQuy");
            LoadData(fr, NamLamViec, Quy);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Nam", NamLamViec);
            if (Quy == "0") Quy = "";
            fr.SetValue("Quy", Quy);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="MaKhoi"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_BaoCaoTaiChinhQuy(String NamLamViec,String Quy)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY sTenDoanhNghiep) AS iRow
                                              ,iID_MaDoanhNghiep,sTenDoanhNghiep,rVongDieuLe,rVonNhaNuoc,rTyLe
                                              ,rTongVon_ChuSoHuu ,rVonDauTu_ChuSoHuu
                                              ,rThangDu_ChuSoHuu,rQuyDTPT_ChuSoHuu,rQuyDPPT_ChuSoHuu
                                              ,rChenhLechTyGia_ChuSoHuu,rLoiNhuanChuaPP_ChuSoHuu,rVonKhac_ChuSoHuu
                                              ,rDoanhThu,rLoiNhuanTruocThue,rLoiNhuanSauThue,rBangTien_VonNhaNuoc
                                              ,rBangCoPhieu_VonNhaNuoc,rCong_VonNhaNuoc,rNopNganSach,rLaoDongBinhQuan
                                              ,rTongQuyLuong,rThuNhapBinhQuan,rPhaiNop_VonNhaNuocKhiCoPhanHoa,rDaNop_VonNhaNuocKhiCoPhanHoa
                                              ,rConPhaiNop_VonNhaNuocKhiCoPhanHoa,rCoTucNamTruoc_CoTuc,rCoTucNamNay_CoTuc
                                              ,rDaNop_CoTuc,rConPhaiNop_CoTuc,rTienDatChuaNop_TienThueDat,rTienDatNamNay_TienThueDat
                                              ,rDaNop_TienThueDat,rConPhaiNop_TienThueDat,rTongSoConPhaiNop,rTongSoDaNop
                                          FROM TCDN_BaoCaoTaiChinh
                                        WHERE TCDN_BaoCaoTaiChinh.iTrangThai=1 AND (rVongDieuLe<>0 OR rVonNhaNuoc<>0 OR rTyLe<>0 OR
rTongVon_ChuSoHuu <>0 OR rVonDauTu_ChuSoHuu <>0  OR rThangDu_ChuSoHuu <>0 OR rQuyDTPT_ChuSoHuu<> 0 OR rQuyDPPT_ChuSoHuu<>0 OR
rChenhLechTyGia_ChuSoHuu<>0 OR rLoiNhuanChuaPP_ChuSoHuu <>0 OR rVonKhac_ChuSoHuu <>0 OR rDoanhThu <>0 OR rLoiNhuanTruocThue <>0 OR 
rLoiNhuanSauThue <>0 OR rBangTien_VonNhaNuoc <> 0 OR rBangCoPhieu_VonNhaNuoc<>0 OR rCong_VonNhaNuoc<>0 OR rNopNganSach <>0 OR rLaoDongBinhQuan <>0 OR
rTongQuyLuong<>0 OR rThuNhapBinhQuan<>0 OR rPhaiNop_VonNhaNuocKhiCoPhanHoa<>0 OR rDaNop_VonNhaNuocKhiCoPhanHoa <>0 OR rConPhaiNop_VonNhaNuocKhiCoPhanHoa<>0 OR
rCoTucNamTruoc_CoTuc<>0 OR rCoTucNamNay_CoTuc <>0 OR rDaNop_CoTuc<>0 OR rConPhaiNop_CoTuc<>0 OR rTienDatChuaNop_TienThueDat<>0 OR rTienDatNamNay_TienThueDat <>0 OR 
rDaNop_TienThueDat<>0 OR rConPhaiNop_TienThueDat<>0 OR rTongSoConPhaiNop<>0 OR rTongSoDaNop<>0)
                                        AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy
                                        ORDER BY sTenDoanhNghiep");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", Quy);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String Quy)
        {
            DataTable data = DT_BaoCaoTaiChinhQuy(NamLamViec, Quy);
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

        public clsExcelResult ExportToExcel(String NamLamViec,String Quy, String To)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String xfilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhQuy" + To + ".xls";
            if (To == "" || To==null)
            {
                xfilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhQuy_1.xls";
            }
            
            ExcelFile xls = CreateReport(Server.MapPath(xfilePath), NamLamViec, Quy);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTChiNganSachQuocPhongQuy.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec,String Quy, String To)
        {
            String xfilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTaiChinhQuy" + To + ".xls";
            ExcelFile xls = CreateReport(Server.MapPath(xfilePath), NamLamViec, Quy);
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
