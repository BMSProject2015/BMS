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

namespace VIETTEL.Report_Controllers.CapPhat
{
    public class rptCP_ChiTieu_QTController : Controller
    {
        //
        // GET: /rptCP_ChiTieu_QT/

        // GET: /rptPhanBo_16D_TheoNganh/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/CapPhat/rptCP_ChiTieu_QT.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/CapPhat/rptCP_ChiTieu_QT.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String Thang_Quy = "";
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCP_ChiTieu_QT.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy)
        {
            String MaND = User.Identity.Name;
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String Thang = "tháng";
            if (LoaiThang_Quy == "1")
            {
                Thang = "quý";
            }

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_TongHopChiTieuCapNganSach_81");
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            LoadData(fr, iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", Thang);
            fr.SetValue("Thang_Quy", Thang_Quy);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.Run(Result);
            fr.Dispose();
            return Result;

        }

        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String MaND)
        {

            DataTable data = dt_TongHopChiTieuCapNganSach_81(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtLNS;
            dtLNS = HamChung.SelectDistinct("LNS", data, "NguonNS,LoaiNS,sLNS", "NguonNS,sLNS,LoaiNS,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);


            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", data, "NguonNS,LoaiNS", "NguonNS,LoaiNS,sMoTa", "sLNS,sL", "LoaiNS");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", data, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            if (dtNguonNS.Rows.Count == 0)
            {
                DataRow r = dtNguonNS.NewRow();
                r["sMoTa"] = "";
                dtNguonNS.Rows.InsertAt(r, 0);
            }
            dtLoaiNS.Dispose();
            dtNguonNS.Dispose();
        }

        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopChiTieuCapPhatCapNganSach_81.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy);
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

        public FileContentResult ViewPDF(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy);
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

        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2})
                                 AND (iID_MaPhongBan ='{3}' )
                                 AND (ChiTiet.iID_MaDonVi IN ({4}))"
                                , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, iID_MaPhongBan, iID_MaDonVi);
            return DK;
        }
        public DataTable dt_TongHopChiTieuCapNganSach_81(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = ReportModels.DieuKien_TrangThaiDuyet;

            if (LoaiThang_Quy == "1")
            {
                Thang_Quy = Convert.ToString(Convert.ToInt16(Thang_Quy) * 3);
            }
            DataTable dt = null;

            String DKDUyet_QT = "", DKDuyet_PB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDUyet_QT = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_QT";
                DKDuyet_PB = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_QT", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }


            String SQL = @"SELECT NguonNS,LoaiNS,sLNS,sTenDonVi,sMoTa,SUM(rTuChi_QT) as rTuChi_QT,SUM(rHienVat_QT) as rHienVat_QT,SUM(rTuChi_PB) as rTuChi_PB,SUM(rHienVat_PB) as rHienVat_PB
                            FROM(
		                            SELECT NguonNS,LoaiNS,sLNS,iID_MaDonVi+' - '+ sTen AS sTenDonVi,sMoTa='',rTuChi as rTuChi_QT,
		                            rHienVat as rHienVat_QT,rTuChi_PB=0,rHienVat_PB=0
		                            FROM (
				                            select SUBSTRING(sLNS,1,1) AS NguonNS,SUBSTRING(sLNS,1,3) AS LoaiNS,sLNS,NS_DonVi.iID_MaDonVi,NS_DonVi.sTen,SUM(rTuChi) as rTuChi,
				                            SUM(rHienVat) as rHienVat
				                            FROM QTA_ChungTuChiTiet as ChiTiet
				                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as  NS_DonVi
				                            ON ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi
				                            WHERE iTrangThai=1 AND iThang_Quy<=@iThang {0} {1}
				                            GROUP BY sLNS,NS_DonVi.iID_MaDonVi,NS_DonVi.sTen
				                            ) as tbla

                            UNION

			                            SELECT NguonNS,LoaiNS,sLNS,PB.iID_MaDonVi+' - '+NS_DonVi.sTen AS sTenDonVi,sMoTa='',rTuChi_QT=0,rHienVat_QT=0,rTuChi_PB=rTuChi,rHienVat_PB=rHienVat
			                            FROM(
			                            SELECT SUBSTRING(sLNS,1,1) AS NguonNS,SUBSTRING(sLNS,1,3) AS LoaiNS,sLNS,iID_MaDonVi,SUM(rTuChi) as rTuChi,
			                            SUM(rHienVat) as rHienVat
			                            FROM PB_PhanBoChiTiet as ChiTiet
			                            WHERE iTrangThai=1  AND bLaHangCha=0 AND IID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo 
													                             FROM PB_DotPhanBo WHERE iTrangThai=1 AND MONTH(dNgayDotPhanBo)<=@iThang) {0} {2}
			                            GROUP BY sLNS,iID_MaDonVi) as PB
			                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as  NS_DonVi
			                            ON PB.iID_MaDonVi=NS_DonVi.iID_MaDonVi
			                            )as a
                            GROUP BY NguonNS,LoaiNS,sLNS,sTenDonVi,sMoTa
                            HAVING SUM(rTuChi_QT)<>0 OR SUM(rHienVat_QT)<>0 OR SUM(rTuChi_PB)<>0 OR SUM(rHienVat_PB)<>0
                            ORDER BY NguonNS,LoaiNS,sLNS,sTenDonVi,sMoTa";
            SQL = string.Format(SQL, DieuKien_NganSach(MaND), DKDUyet_QT, DKDuyet_PB);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

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
