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

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptTCDN_BaoCaoInKiemController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_HoSoDoanhNghiep = "/Report_ExcelFrom/TCDN/rptTCDN_HoSoDoanhNghiep.xls";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoInKiem.xls";
        private const String sFilePath_Loai4 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoInKiem_Loai4.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoInKiem.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String iID_MaDoanhNghiep = Request.Form["iID_MaDonVi"];
            String DVT = Request.Form[ParentID + "_DVT"];
            String bTrongKy = Request.Form[ParentID + "_bTrongKy"];
            String iLoai = Request.Form[ParentID + "_iLoai"];
            ViewData["PageLoad"] = "1";
            ViewData["iQuy"] = iQuy;
            ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
            ViewData["DVT"] = DVT;
            ViewData["bTrongKy"] = bTrongKy;
            ViewData["iLoai"] = iLoai;
            ViewData["path"] = "~/Report_Views//TCDN/rptTCDN_BaoCaoInKiem.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String iLoai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BaoCaoInKiem");
            LoadData(fr, iID_MaChungTu, arrGiaTriTimKiem,iLoai);
            String Nam = "";
            String TenBieu1 = "", TenBieu2 = "";
            if (iLoai == "1")
            {
                TenBieu1 = "Biểu số 1";
                TenBieu2 = "CHỈ TIÊU HOẠT ĐỘNG SẢN XUẤT KINH DOANH";
            }
            else if (iLoai == "2")
            {
                TenBieu1 = "Biểu số 2";
                TenBieu2 = "CHỈ TIÊU THU CHI NGÂN SÁCH VÀ THU NHẬP";
            }
            else if (iLoai == "3")
            {
                TenBieu1 = "Biểu số 3";
                TenBieu2 = "CHỈ TIÊU TÀI CHÍNH";
            }
            else if (iLoai == "4")
            {
                TenBieu1 = "Biểu số 4";
                TenBieu2 = "CHỈ TIÊU ĐÁNH GIÁ HIỆU QUẢ";
            }
            String ThongTin = "";
            DataTable dtChungTu = TCDN_ChungTuModels.GetChungTu(iID_MaChungTu);
            String sTenDoanhNghiep = Convert.ToString(CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep",
                                                      dtChungTu.Rows[0]["iID_MaDoanhNghiep"].ToString(),
                                                      "sTenDoanhNghiep"));
            String iQuy = dtChungTu.Rows[0]["iQuy"].ToString();
            ThongTin = " Doanh nghiệp: " + sTenDoanhNghiep + "          Quý:" + iQuy;
            dtChungTu.Dispose();
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("ThongTin",ThongTin);
            fr.SetValue("TenBieu1", TenBieu1);
            fr.SetValue("TenBieu2", TenBieu2);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String iLoai)
        {
            DataRow r;
            DataTable data = TCDN_ChungTuChiTietModels.Get_dtChungTuChiTiet(iID_MaChungTu, arrGiaTriTimKiem, iLoai);
            CapNhapHangTongCong(data);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

        }
        protected void CapNhapHangTongCong(DataTable _dtChiTiet)
        {
            String strDSTruongTien = "rNamTruoc,rKeHoach,rDaThucHien,rThucHien";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            int len = arrDSTruongTien.Length;
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaTong"]))
                {
                    //TInh tong cac truong tien
                    String iiD_MaChiTieu = Convert.ToString(_dtChiTiet.Rows[i]["iiD_MaChiTieu"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S;
                        S = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iiD_MaChiTieu == Convert.ToString(_dtChiTiet.Rows[j]["iiD_MaChiTieu_Cha"]))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[j][arrDSTruongTien[k]])))
                                {
                                    S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                }
                            }
                        }
                        _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                    }
                    //Tinh 2 cot ty le
                    if (Convert.ToDecimal(_dtChiTiet.Rows[i]["rNamTruoc"]) != 0)
                        _dtChiTiet.Rows[i]["rNTNN"] = (Math.Round((Convert.ToDecimal(_dtChiTiet.Rows[i]["rThucHien"]) + Convert.ToDecimal(_dtChiTiet.Rows[i]["rDaThucHien"])) /
                                              Convert.ToDecimal(_dtChiTiet.Rows[i]["rNamTruoc"]) * 100, 0));
                    if (Convert.ToDecimal(_dtChiTiet.Rows[i]["rKeHoach"]) != 0)
                        _dtChiTiet.Rows[i]["rTHKH"] = (Math.Round((Convert.ToDecimal(_dtChiTiet.Rows[i]["rThucHien"]) + Convert.ToDecimal(_dtChiTiet.Rows[i]["rDaThucHien"])) /
                                            Convert.ToDecimal(_dtChiTiet.Rows[i]["rKeHoach"]) * 100, 0));
                }
            }

        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String iLoai)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iLoai == "0")
            {
                sDuongDan = sFilePath_HoSoDoanhNghiep;
            }
            else if(iLoai=="4")
            {
                sDuongDan = sFilePath_Loai4;
            }
            else
            {
                sDuongDan = sFilePath;
            }


            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaChungTu, arrGiaTriTimKiem, iLoai);
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

        public clsExcelResult ExportToExcel(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String iLoai)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iLoai == "0")
            {
                sDuongDan = sFilePath_HoSoDoanhNghiep;
            }
            else if (iLoai == "4")
            {
                sDuongDan = sFilePath_Loai4;
            }
            else
            {
                sDuongDan = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaChungTu, arrGiaTriTimKiem, iLoai);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "HoSoDoanhNghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            DataTable dt = TCSN_DoanhNghiepModels.Get_ListDoanhNghiep();
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

