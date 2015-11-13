
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
    public class rptDT_3bCController : Controller
    {
        //
        // GET: /rptDT_3bC/

        public string sViewPath = "~/Report_Views/DuToan/";
        public static String DuongDan;
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDT_3bC.xls";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            DuongDan = Server.MapPath(sFilePath);
            ViewData["path"] = "~/Report_Views/DuToan/rptDT_3bC.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form LNS = 2
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            
            return RedirectToAction("Index", new {iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });

        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDT_3bC");
            LoadData(fr,MaND, iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("PhuLuc", "Phụ Lục Số 3b-C");
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// hàm xuất dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        public DataTable rptDT_3bC_PhanCap( String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@" SELECT 
                                        iID_MaDonVi,sTen
                                        ,SUM(CongTrinhCD) as CongTrinhCD
                                        ,SUM(XayDungDT) as XayDungDT
                                        ,SUM(HuanLuyenCD) as HuanLuyenCD 
                                        ,SUM(NaoVetQS) as NaoVetQS
                                        ,SUM(CongTrinhTT) as CongTrinhTT
                                        ,SUM(AnDuongDD) as AnDuongDD
                                        ,SUM(CTVH) as CTVH
                                        ,SUM(CongTrinhKhac) as CongTrinhKhac
                                        FROM
                                        (
                                        SELECT DV.iID_MaDonVi,DV.sTen
                                        ,CongTrinhCD=CASE WHEN sTTM IN ('10','00') AND sNG='21' THEN SUM(rPhanCap) ELSE 0 END
                                        ,XayDungDT=CASE WHEN sTTM='50' AND sNG='56' THEN SUM(rPhanCap) ELSE 0 END
                                        ,HuanLuyenCD=CASE WHEN sTTM='40' AND sNG='22' THEN SUM(rPhanCap) ELSE 0 END
                                        ,NaoVetQS=CASE WHEN sTTM='20' AND sNG='21' THEN SUM(rPhanCap) ELSE 0 END
                                        ,CongTrinhTT=CASE WHEN sTTM='30' AND sNG='04' THEN SUM(rPhanCap) ELSE 0 END
                                        ,AnDuongDD=CASE WHEN  sTTM='70' AND sNG='37' THEN SUM(rPhanCap) ELSE 0 END
                                        ,CTVH=CASE WHEN sTTM='50' AND sNG='32' THEN SUM(rPhanCap) ELSE 0 END
                                        ,CongTrinhKhac=CASE WHEN  sTTM+sNG NOT IN('5056','4022','1022','3004','5032','7037') THEN SUM(rPhanCap) ELSE 0 END
                                        FROM DT_ChungTuChiTiet AS DT
                                        INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON DT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE  DT.iTrangThai=1 AND sLNS='1030100' AND sL='460' AND sK='468' {1} AND sNG<>'' {0}
                                        GROUP BY DV.iID_MaDonVi,DV.sTen,sLNS,sTTM,sNG
                                        HAVING SUM(rPhanCap)!=0
                                        )
                                        as BANGTAM
                                        GROUP BY iID_MaDonVi,sTen
                                        HAVING SUM(CongTrinhCD)!=0 OR SUM(XayDungDT) !=0 OR SUM(HuanLuyenCD)!=0 OR SUM(NaoVetQS)!=0
                                        OR SUM(CongTrinhTT)!=0 OR SUM(AnDuongDD)!=0 OR SUM(CTVH)!=0 OR SUM(CongTrinhKhac)!=0", ReportModels.DieuKien_NganSach(MaND,"DT"),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// Dt đơn vị chờ phân cấp
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public DataTable rptDT_3bC_DuPhong(String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@"SELECT SUM(CongTrinhCD) as CongTrinhCD
                                            ,SUM(XayDungDT) as XayDungDT
                                            ,SUM(HuanLuyenCD) as HuanLuyenCD 
                                            ,SUM(NaoVetQS) as NaoVetQS
                                            ,SUM(CongTrinhTT) as CongTrinhTT
                                            ,SUM(AnDuongDD) as AnDuongDD
                                            ,SUM(CTVH) as CTVH
                                            ,SUM(CongTrinhKhac) as CongTrinhKhac
                                            FROM
                                            (
                                            SELECT
                                            CongTrinhCD=CASE WHEN sTTM IN ('10','00') AND sNG='21' THEN SUM(rDuPhong) ELSE 0 END
                                            ,XayDungDT=CASE WHEN sTTM='50' AND sNG='56' THEN SUM(rDuPhong) ELSE 0 END
                                            ,HuanLuyenCD=CASE WHEN sTTM='40' AND sNG='22' THEN SUM(rDuPhong) ELSE 0 END
                                            ,NaoVetQS=CASE WHEN sTTM='20' AND sNG='21' THEN SUM(rDuPhong) ELSE 0 END
                                            ,CongTrinhTT=CASE WHEN sTTM='30' AND sNG='04' THEN SUM(rDuPhong) ELSE 0 END
                                            ,AnDuongDD=CASE WHEN  sTTM='70' AND sNG='37' THEN SUM(rDuPhong) ELSE 0 END
                                            ,CTVH=CASE WHEN sTTM='50' AND sNG='32' THEN SUM(rDuPhong) ELSE 0 END
                                            ,CongTrinhKhac=CASE WHEN  sTTM+sNG NOT IN('5056','4022','1022','3004','5032','7037') THEN SUM(rDuPhong) ELSE 0 END
                                            FROM DT_ChungTuChiTiet AS DT
                                            WHERE DT.iTrangThai=1 AND sLNS='1030100' AND sL='460' AND sK='468' {1} AND sNG<>'' {0}
                                            GROUP BY sLNS,sTTM,sNG
                                            HAVING SUM(rDuPhong)!=0
                                            )
                                            as BANGTAM
                                            HAVING SUM(CongTrinhCD)!=0 OR SUM(XayDungDT) !=0 OR SUM(HuanLuyenCD)!=0 OR SUM(NaoVetQS)!=0
                                            OR SUM(CongTrinhTT)!=0 OR SUM(AnDuongDD)!=0 OR SUM(CTVH)!=0 OR SUM(CongTrinhKhac)!=0", ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);

            return dt;
        }
        private void LoadData(FlexCelReport fr,  String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable data = rptDT_3bC_PhanCap( MaND, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTietPhanCap";
            fr.AddTable("ChiTietPhanCap", data);

            DataTable DataDuPhong = rptDT_3bC_DuPhong(MaND, iID_MaTrangThaiDuyet);
            DataDuPhong.TableName = "ChiTietDuphong";
            fr.AddTable("ChiTietDuphong", DataDuPhong);
        }
        /// <summary>
        /// hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptDT_3bC.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                xls.Save(Server.MapPath("/Report_ExcelFrom/DuToan/Test.pdf"));
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
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>

        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(true, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }

            }
            return null;

        }
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
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


