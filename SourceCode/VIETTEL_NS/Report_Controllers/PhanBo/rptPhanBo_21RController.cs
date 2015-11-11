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

namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptPhanBo_21RController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/PhanBo/rptPhanBo_21R.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_21R.aspx";
                ViewData["PageLoad"] = "0";
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
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_21R.aspx";
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TruongTien"] = TruongTien;
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV;
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }
            String TenTruongTien = "";
            if (TruongTien == "rTuChi")
            {
                TenTruongTien = "Tự chi";
            }
            else
            {
                TenTruongTien = "Hiện vật";
            }
               String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_21R");
                LoadData(fr, MaND, iID_MaTrangThaiDuyet, iID_MaDonVi,TruongTien);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));               
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("TruongTien", TenTruongTien);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.Run(Result);
                return Result;
            
        }
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TruongTien)
        {
            DataTable data = PhanBo_21R(MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, TruongTien);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sTen,NhomNS,sLNS,sL,sK,sM,sTM", "NguonNS,sTen,NhomNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sTen,NhomNS,sLNS,sL,sK,sM", "NguonNS,sTen,NhomNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sTen,NhomNS,sLNS", "NguonNS,sTen,NhomNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNhomNS;
            dtNhomNS = HamChung.SelectDistinct("NhomNS", dtLoaiNS, "NguonNS,sTen,NhomNS", "NguonNS,sTen,NhomNS,sLNS,sMoTa", "sLNS,sL", "NhomNS");                
            fr.AddTable("NhomNS", dtNhomNS);

            DataTable dtsTen;
            dtsTen = HamChung.SelectDistinct("sTen", dtNhomNS, "NguonNS,sTen", "NguonNS,sTen,sLNS,sMoTa");
            fr.AddTable("sTen", dtsTen);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtsTen, "NguonNS", "NguonNS,sTen,sLNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);

            dtsTen.Dispose();
            dtNhomNS.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDonVi, TruongTien, iID_MaTrangThaiDuyet);
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
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile),  iID_MaDonVi, TruongTien,iID_MaTrangThaiDuyet);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "phanbo_21R.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile),  iID_MaDonVi, TruongTien,iID_MaTrangThaiDuyet);
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
        /// onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String TruongTien)
        {

            return Json(obj_DSDonVi(ParentID, MaND,iID_MaTrangThaiDuyet, iID_MaDonVi, TruongTien), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TruongTien)
        {
            String dsDonVi = "";
            DataTable dtDonVi = DanhSach_DonVi(MaND, iID_MaTrangThaiDuyet, TruongTien);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi","", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        /// <summary>
        /// phân bổ 21 R
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public DataTable PhanBo_21R(String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi,String TruongTien)
        {
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = "2000";
            String iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            String SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1)as NguonNS,sTen,SUBSTRING(sLNS,1,3)as NhomNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM({0}) as SoThongBao
                                        FROM (SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,{0},iID_MaNamNganSach 
											  FROM QTA_ChungTuChiTiet
											  WHERE iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND sNG<>'' AND iTrangThai=1 {1} {2} ) as A
                                        INNER JOIN NS_NamNganSach ON A.iID_MaNamNganSach=NS_NamNganSach.iID_MaNamNganSach
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,sTen,SUBSTRING(sLNS,1,3) 
                                        HAVING SUM({0})<>0
                                        ", TruongTien,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (!String.IsNullOrEmpty(DK_Duyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String MaND, String iID_MaTrangThaiDuyet, String TruongTien)
        {
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = "2000";
            String iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                 iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                 iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen 
                                        FROM (
						                      SELECT iID_MaDonVi
						                      FROM QTA_ChungTuChiTiet
						                      WHERE iTrangThai=1 {1} AND iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach
						                      GROUP BY iID_MaDonVi
						                      HAVING SUM({0})<>0) a
                                        INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b
                                        ON a.iID_MaDonVi=b.MaDonVi
                                        ORDER BY iID_MaDonVi", TruongTien,ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            if (!String.IsNullOrEmpty(DK_Duyet))
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            DataTable dtDonVi = Connection.GetDataTable(cmd);    
            cmd.Dispose();
            if (dtDonVi.Rows.Count <= 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            return dtDonVi;
        }
    }
}
