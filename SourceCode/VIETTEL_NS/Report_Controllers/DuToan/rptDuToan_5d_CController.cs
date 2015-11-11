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
    public class rptDuToan_5d_CController : Controller
    {
        //
        // GET: /rptDuToan_5d_C/

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_5d_C.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_5d_C.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_5d_C.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_5d_C");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("NgayThangNam", NgayThang);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dtDuToan_rptDuToan_5d_C(MaND, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);         
        }
        public static DataTable dtDuToan_rptDuToan_5d_C( String MaND, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,TENHT,SUM(rTroCap_Nguoi) as rTroCap_Nguoi,SUM(rTroCap_Tien) as rTroCap_Tien,
                                        SUM(rPhuCap_Nguoi) as rPhuCap_Nguoi,SUM(rTroCap_Tien) as rTroCap_Tien,SUM(rTienKhoiNghia_Nguoi) as rTienKhoiNghia_Nguoi,
                                        SUM(rPhuCap_Tien) as rPhuCap_Tien,SUM(rTienKhoiNghia_Nguoi) as rTienKhoiNghia_Nguoi,SUM(rTienKhoiNghia_Tien) as rTienKhoiNghia_Tien,
                                        SUM(rAnhHung_Nguoi) as rAnhHung_Nguoi,SUM(rAnhHung_Tien) as rAnhHung_Tien,SUM(rThuongBinhA_Nguoi) as rThuongBinhA_Nguoi,
                                        SUM(rThuongBinhA_Tien) as rThuongBinhA_Tien,SUM(rThuongBinhB_Nguoi) as rThuongBinhB_Nguoi,SUM(rThuongBinhB_Tien) as rThuongBinhB_Tien,
                                        SUM(rTroCapBaoTu_Nguoi) as rTroCapBaoTu_Nguoi,SUM(rTroCapBaoTu_Tien) as rTroCapBaoTu_Tien,SUM(rLePhi) as rLePhi
                                        FROM(
                                        SELECT  iID_MaDonVi
                                        ,rTroCap_Nguoi=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=01 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rTroCap_Tien=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=01 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rPhuCap_Nguoi=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=02 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rPhuCap_Tien=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=02 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rTienKhoiNghia_Nguoi=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=03 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rTienKhoiNghia_Tien=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=03 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rAnhHung_Nguoi=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=04 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rAnhHung_Tien=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=04 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rThuongBinhA_Nguoi=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=05 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rThuongBinhA_Tien=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=05 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rThuongBinhB_Nguoi=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=06 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rThuongBinhB_Tien=CASE WHEN (sM=7150 AND sTM=7151 AND sTTM=06 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rTroCapBaoTu_Nguoi=CASE WHEN (sM=7150 AND sTM=7199 AND sTTM=70 AND sNG=38) THEN SUM(rSoNguoi) ELSE 0 END
                                        ,rTroCapBaoTu_Tien=CASE WHEN (sM=7150 AND sTM=7199 AND sTTM=70 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        ,rLePhi=CASE WHEN (sM=7150 AND sTM=7164 AND sTTM=00 AND sNG=38) THEN SUM(rTuChi) ELSE 0 END
                                        FROM DT_ChungTuChiTiet
                                        WHERE iTrangThai=1
                                             {0}
                                             {1}
                                              AND SUBSTRING(sLNS,1,3)='206'
                                        GROUP BY sM,sTM,sTTM,sNG,iID_MaDonVi
                                        HAVING SUM(rSoNguoi)<>0 OR SUM(rTuChi) <>0
                                        ) a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen,iID_MaDonVi+'   '+sTen AS TENHT FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec AND iTrangThai=1) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        GROUP BY a.iID_MaDonVi,TENHT  HAVING SUM(rTroCap_Nguoi) <> 0 OR 
                                        SUM(rTroCap_Tien) <> 0 OR 
                                        SUM(rPhuCap_Nguoi)<> 0 OR 
                                        SUM(rTroCap_Tien)<> 0 OR 
                                        SUM(rTienKhoiNghia_Nguoi) <> 0 OR 
                                        SUM(rPhuCap_Tien) <> 0 OR 
                                        SUM(rTienKhoiNghia_Nguoi) <> 0 OR 
                                        SUM(rTienKhoiNghia_Tien) <> 0 OR 
                                        SUM(rAnhHung_Nguoi) <> 0 OR 
                                        SUM(rAnhHung_Tien) <> 0 OR 
                                        SUM(rThuongBinhA_Nguoi) <> 0 OR 
                                        SUM(rThuongBinhA_Tien) <> 0 OR 
                                        SUM(rThuongBinhB_Nguoi) <> 0 OR 
                                        SUM(rThuongBinhB_Tien) <> 0 OR 
                                        SUM(rTroCapBaoTu_Nguoi) <> 0 OR 
                                        SUM(rTroCapBaoTu_Tien) <> 0 OR 
                                        SUM(rLePhi) <> 0
                                        ", ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
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
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
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
