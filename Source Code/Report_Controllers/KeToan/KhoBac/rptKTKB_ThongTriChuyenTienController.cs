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
    public class rptKTKB_ThongTriChuyenTienController : Controller
    {
        //
        // GET: /rptKTKB_ThongTriChuyenTien/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptThongTriChuyenTien.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_ThongTriChuyenTien.aspx";
            return View(sViewPath + "ReportView.aspx");
             }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec, String iID_MaChungTu)
        {
            //StringiNamLamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec"]);
            // String iID_MaChungTu= Convert.ToString(Request.Form[ParentID + "_iID_MaChungTu"]);
            return RedirectToAction("Index", new { iNamLamViec = iNamLamViec, iID_MaChungTu = iID_MaChungTu });
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi_Nhan)
        {
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            DataTable dt = dt_KTKB_ThongTriChuyenTien(MaND,iID_MaDonVi_Nhan);
            String NgayCT = "";
            String ThangCT = "";
            String NgayThang = "";
            if (dt.Rows.Count > 0)
            {
              //  NgayCT = dt.Rows[0]["iNgayCT"].ToString();
               // ThangCT = dt.Rows[0]["iThangCT"].ToString();
                NgayThang = "ngày " + NgayCT + " tháng " + ThangCT;
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongTriChuyenTien");
            LoadData(fr, MaND, iID_MaDonVi_Nhan);
            fr.SetValue("NgayThangNam", iID_MaDonVi_Nhan);
           // fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("NgayThang", NgayThang);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// danh sách đơn vị
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable dtDanhSachDonVi_Nhan(String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi_Nhan,sTenDonVi_Nhan
                                         FROM KTKB_ChungTuChiTiet
                                         WHERE iTrangThai=1 
                                                AND iNamLamViec=@iNam
                                                AND iThangCT=@iThang
                                                AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                                AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                                AND rDTRut>0
            ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dt_KTKB_ThongTriChuyenTien(String MaND, String iID_MaDonVi_Nhan)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sMoTa,sTenDonVi_Nhan,SUM(rDTRut) as rDTRut
                                        FROM KTKB_ChungTuChiTiet
                                        WHERE iTrangThai=1 
                                              AND iNamLamViec=@iNam
                                              AND iThangCT=@iThang
                                              AND iID_MaDonVi_Nhan=@iID_MaDonVi_Nhan
                                              AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                              AND iID_MaNamNganSach=@iID_MaNamNganSach
                                              AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        GROUP BY sLNS,sL,sK,sM,sTM,sMoTa,sTenDonVi_Nhan
                                        HAVING SUM(rDTRut)>0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi_Nhan", iID_MaDonVi_Nhan);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int a = dt.Rows.Count;
            if (a < 25 && a > 0)
            {
                for (int i = 0; i < (25 - a); i++)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, a + 1);
                }
            }
            dt.Dispose();
            return dt;
        }
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi_Nhan)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaDonVi_Nhan);
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
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi_Nhan)
        {
            DataTable data = dt_KTKB_ThongTriChuyenTien(MaND, iID_MaDonVi_Nhan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TM", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("TM", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("Loai", dtMuc, "sLNS,sL,sK", "sLNS,sL,sK");
            fr.AddTable("Loai", dtLoaiNS);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
    }
}
