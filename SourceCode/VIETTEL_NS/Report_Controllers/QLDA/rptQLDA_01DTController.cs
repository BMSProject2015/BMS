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

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_01DTController : Controller
    {
        //
        // GET: /rptQLDA_01DT/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_01DT.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01DT.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            return RedirectToAction("Index", new { NamLamViec = NamLamViec });
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec)
        {

            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_01DT");
            LoadData(fr, NamLamViec);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("cuctaichinh", "Cục Tài Chính");
            fr.SetValue("ngay", ngay);
            fr.SetValue("Phong", "Phòng QLNS Các DA");
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public DataTable dtrptQLDA_01DT(String NamLamViec)
        {

            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn
                                        ,TMDTDuocDuyet=SUM(TMDTDuocDuyet)/1000000
                                        ,TDTDuocDuyet=SUM(TDTDuocDuyet)/1000000
                                        ,KHVDenNamTruoc=SUM(KHVDenNamTruoc)/1000000
                                        ,KHVTrongNamTruoc=SUM(KHVTrongNamTruoc)/1000000
                                        ,KHVUngTruocChuaThuHoi=SUM(KHVUngTruocChuaThuHoi)/1000000
                                        ,Tong=SUM(Tong)/1000000
                                        ,ThuHoiVonTamUngTruoc=SUM(ThuHoiVonTamUngTruoc)/1000000
                                        ,ThuNop=SUM(ThuNop)/1000000
                                        ,TongCucTaiChinhDeNghe=SUM(TongCucTaiChinhDeNghe)/1000000
                                        ,ThuTamUngCucTaiChinh=SUM(ThuTamUngCucTaiChinh)/1000000
                                        ,ThuNopCucTaiChinh=SUM(ThuNopCucTaiChinh)/1000000
                                         FROM
                                        (
                                        (
                                        SELECT iID_MaDanhMucDuAn,sDuAn,sDuAnThanhPhan
                                        ,sCongTrinh,sTenDuAn
                                         FROM QLDA_DanhMucDuAn
                                        WHERE sHangMucCongTrinh<>'') as DM
                                         INNER JOIN 
                                         (
                                        SELECT DT.iID_MaDanhMucDuAn
                                            ,TMDTDuocDuyet=CASE WHEN DT.iTrangThai=1 AND DT.iNamLamViec=@NamLamViec THEN SUM(rSoTien_TongDauTu) ELSE 0 END
                                            ,TDTDuocDuyet=CASE WHEN TDT.iTrangThai=1 AND TDT.iNamLamViec=@NamLamViec THEN SUM(rSoTien_TongDauTu) ELSE 0 END
                                            FROM QLDA_TongDauTu as DT
                                            LEFT JOIN QLDA_TongDuToan as TDT ON DT.iID_MaDanhMucDuAn=TDT.iID_MaDanhMucDuAn
                                            GROUP BY DT.iTrangThai,DT.iNamLamViec,DT.iID_MaDanhMucDuAn,TDT.iNamLamViec,TDT.iTrangThai) as TDT
                                        ON DM.iID_MaDanhMucDuAn=TDT.iID_MaDanhMucDuAn

                                        INNER JOIN
                                        (
                                        SELECT iID_MaDanhMucDuAn
                                        ,KHVDenNamTruoc=CASE WHEN iTrangThai=1 AND iNamLamViec<=@NamLamViec-1 THEN SUM(rSoTienDauNam)+SUM(rSoTienDieuChinh) ELSE 0 END
                                        ,KHVTrongNamTruoc=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec-1 THEN SUM(rSoTienDauNam)+SUM(rSoTienDieuChinh) ELSE 0 END
                                        ,KHVUngTruocChuaThuHoi=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec AND iLoaiKeHoachVon=2 THEN SUM(rSoTienDauNam)+SUM(rSoTienDieuChinh) ELSE 0 END
                                        FROM QLDA_KeHoachVon
                                        GROUP BY iID_MaDanhMucDuAn,iTrangThai,iNamLamViec,iLoaiKeHoachVon) as KHV
                                        ON DM.iID_MaDanhMucDuAn=KHV.iID_MaDanhMucDuAn

                                        INNER JOIN 
                                        (
                                        SELECT iID_MaDanhMucDuAn
                                        ,Tong=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rDonViDeNghi)+SUM(rDonViThu)+SUM(rDonViThuTamUng) ELSE 0 END
                                        ,ThuHoiVonTamUngTruoc=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rDonViThuTamUng) ELSE 0 END
                                        ,ThuNop=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rDonViThu) ELSE 0 END
                                        ,TongCucTaiChinhDeNghe=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rCucTaiChinhDeNghi)+SUM(rCucTaiChinhThu)+SUM(rCucTaiChinhThuTamUng) ELSE 0 END
                                        ,ThuTamUngCucTaiChinh=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec THEN SUM(rCucTaiChinhThuTamUng) ELSE 0 END
                                        ,ThuNopCucTaiChinh=CASE WHEN iTrangThai=1 AND iNamLamViec=@NamLamViec  THEN SUM(rCucTaiChinhThu) ELSE 0 END
                                        FROM QLDA_DuToan_Nam
                                        GROUP BY iID_MaDanhMucDuAn,iTrangThai,iNamLamViec) as DTN
                                        ON DM.iID_MaDanhMucDuAn=DTN.iID_MaDanhMucDuAn)
                                        GROUP BY sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn");
                            SqlCommand cmd = new SqlCommand(SQL);
                            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                            dt = Connection.GetDataTable(cmd);
                            cmd.Dispose();
                            return dt;
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec)
        {
            DataTable data = dtrptQLDA_01DT(NamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable CongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", data, "sDuAn,sDuAnThanhPhan,sCongTrinh", "sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn", "sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            CongTrinh.TableName = "CongTrinh";
            fr.AddTable("CongTrinh", CongTrinh);


            DataTable DuAnTP = HamChung.SelectDistinct_QLDA("DuAnTP", CongTrinh, "sDuAn,sDuAnThanhPhan", "sDuAn,sDuAnThanhPhan,sTenDuAn", "sDuAn,sDuAnThanhPhan,sCongTrinh");
            DuAnTP.TableName = "DuAnTP";
            fr.AddTable("DuAnTP", DuAnTP);

            DataTable DuAn = HamChung.SelectDistinct_QLDA("DuAn", DuAnTP, "sDuAn", "sDuAn,sTenDuAn", "sDuAn,sDuAnThanhPhan");
            DuAn.TableName = "DuAn";
            fr.AddTable("DuAn", DuAn);

            CongTrinh.Dispose();
            DuAn.Dispose();
            DuAnTP.Dispose();
            data.Dispose();



        }
        /// <summary>
        /// Ham xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
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
        public clsExcelResult ExportToExcel(String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptQLDA_QTHT_02QTHT.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
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
