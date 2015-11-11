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
using System.Collections.Specialized;
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_TongHopPhanHoTheoTaiKhoanController : Controller
    {
        //
        // GET: /rptKeToanTongHop_TongHopPhanHoTheoTaiKhoan/

        public string sViewPath = "~/Report_Views/";
        public string Count = "";
        public decimal Tien = 0;

        public ActionResult Index(String LoaiBieu = "")
        {
            String sFilePath = "";
            if (LoaiBieu == "rAll")
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung.xls";
            else
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_TongHopPhanHoTheoTaiKhoan.xls";
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_TongHopPhanHoTheoTaiKhoan.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }



        public ActionResult EditSubmit(String ParentID)
        {
            int i;
            String MaTaiKhoan = Request.Form[ParentID + "_MaTaiKhoan"];
            String Thang = Request.Form[ParentID + "_Thang"];
            String LoaiBieu = Request.Form[ParentID + "_divLoaiBieu"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            NameValueCollection arrLoi = new NameValueCollection();
            if (String.IsNullOrEmpty(MaTaiKhoan) == true || MaTaiKhoan == "" || MaTaiKhoan == null)
            {
                arrLoi.Add("err_MaTaiKhoan", "Chọn tài khoản");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return View("~/Report_Views/KeToan/TongHop/rptKeToanTongHop_TongHopPhanHoTheoTaiKhoan.aspx");
            }
            else
            {

                ViewPDF(MaTaiKhoan, Thang);
                return RedirectToAction("Index", new { MaTaiKhoan = MaTaiKhoan, Thang = Thang, LoaiBieu = LoaiBieu });
            }
        }
       
        public ExcelFile CreateReport(String path,  String MaTaiKhoan = "", String Thang = "")
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDVCapTren = "Bộ quốc phòng";
            String tendv = "Cục Tài chính";
            String TenTK = TaiKhoanModels.getTenTK(MaTaiKhoan);
            using (FlexCelReport fr = new FlexCelReport())
            {
                LoadData(fr, MaTaiKhoan, Thang);
 
                fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
                fr.SetValue("TenDV", tendv.ToUpper());
                fr.SetValue("GiaiThich", "TỔNG HỢP TÀI KHOẢN THEO ĐƠN VỊ - TÀI KHOẢN " + MaTaiKhoan);
                fr.SetValue("ThoiGian", Count);
                fr.SetValue("TaiKhoan", TenTK);
                fr.SetValue("Nam", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);
       
                fr.Run(Result);
                return Result;
            }
        }
        private DataTable CreatTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DonVi", typeof(string));
            dt.Columns.Add("NoDauKy", typeof(string));
            dt.Columns.Add("CoDauKy", typeof(string));
            dt.Columns.Add("NoPS", typeof(string));
            dt.Columns.Add("CoPS", typeof(string));
            dt.Columns.Add("NoLK", typeof(string));
            dt.Columns.Add("CoLK", typeof(string));
            dt.Columns.Add("DuNo", typeof(string));
            dt.Columns.Add("DuCo", typeof(string));   
            return dt;
        }
        private DataTable get_DanhSach_ChungTu(String MaTaiKhoan = "", String Thang = "")
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = @"SELECT DISTINCT CT.sSoChungTuChiTiet, CT.iNgay, CT.iThang, CT.sNoiDung, CT.rSoTien, CT.iID_MaTaiKhoan_No, 
                        CT.iID_MaTaiKhoan_Co, KT.iSoChungTu, KT.iNgay AS NgayGhiSo, KT.iThang AS ThangGhiSo, CT.sKyHieuPhongBan_Co, CT.sKyHieuPhongBan_No
FROM          dbo.KT_ChungTuChiTiet AS CT INNER JOIN
                        dbo.KT_ChungTu AS KT ON CT.iID_MaChungTu = KT.iID_MaChungTu
WHERE      (CT.iTrangThai = 1) AND (KT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) AND ((CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK1))  OR
                        (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK2)) OR
                        (CT.iThang <> KT.iThang) OR
                        (CT.sKyHieuPhongBan_No NOT IN
                            (SELECT sKyHieu  FROM NS_PhongBan AS PB1))OR
                        (CT.sKyHieuPhongBan_Co NOT IN
                            (SELECT sKyHieu  FROM NS_PhongBan AS PB2)) OR
                        (CT.iID_MaTaiKhoan_No IS NULL) OR
                        (CT.iID_MaTaiKhoan_No = '') OR
                        (CT.iID_MaTaiKhoan_Co IS NULL) OR
                        (CT.iID_MaTaiKhoan_Co = '') OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1))) OR
                        (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1))) OR
                        (CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1)) OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1)))";           
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            DataTable dtMain = CreatTable();
            string GhiChu = "", TaiKhoanNo = "", TaiKhoanCo = "", DonViCo = "", DonViNo = "";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DataRow drMain = dtMain.NewRow();
                    TaiKhoanNo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_No"]);
                    TaiKhoanCo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_Co"]);
                    DonViCo = HamChung.ConvertToString(dr["sKyHieuPhongBan_Co"]);
                    DonViNo = HamChung.ConvertToString(dr["sKyHieuPhongBan_No"]);


                    drMain["SoChungTu"] = HamChung.ConvertToString(dr["sSoChungTuChiTiet"]);
                    drMain["NgayChungTu"] = HamChung.ConvertToString(dr["iNgay"]) + " - " + HamChung.ConvertToString(dr["iThang"]);
                    drMain["NoiDung"] = HamChung.ConvertToString(dr["sNoiDung"]);
                    drMain["SoTien"] = CommonFunction.DinhDangSo(HamChung.ConvertToString(dr["rSoTien"]));
                    drMain["TaiKhoanNo"] = TaiKhoanNo;
                    drMain["TaiKhoanCo"] = TaiKhoanCo;
                    drMain["SoGhiSo"] = HamChung.ConvertToString(dr["iSoChungTu"]);
                    drMain["NgayGhiSo"] = HamChung.ConvertToString(dr["NgayGhiSo"]) + " - " + HamChung.ConvertToString(dr["ThangGhiSo"]);
                    //lấy ghi chú
                    //GhiChu = GetGhiChu(TaiKhoanCo, TaiKhoanNo, DonViNo, DonViCo);
                    if (HamChung.ConvertToString(dr["iThang"]) != HamChung.ConvertToString(dr["ThangGhiSo"])) GhiChu += " T";
                    if (TaiKhoanNo != "" && TaiKhoanCo != "" && TaiKhoanNo == TaiKhoanCo) GhiChu += " =";
                    if (TaiKhoanNo == "" && TaiKhoanCo == "") GhiChu += " ?";
                    drMain["GhiChu"] = GhiChu;
                    drMain["DonVi"] = DonViNo;
                    Tien += Convert.ToDecimal(HamChung.ConvertToString(dr["rSoTien"]));
                    dtMain.Rows.Add(drMain);
                }
            }
            cmd.Dispose();
            if (dt != null) dt.Dispose();
            //Đếm số dòng
            Count = dtMain.Rows.Count.ToString();
            return dtMain;
        }
        private void LoadData(FlexCelReport fr, String MaTaiKhoan = "", String Thang = "")
        {

            DataTable data = get_DanhSach_ChungTu(MaTaiKhoan, Thang);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        public clsExcelResult ExportToPDF(String MaTaiKhoan = "", String Thang = "", String LoaiBieu = "")
        {
            HamChung.Language();
            String sFilePath = "";
            if (LoaiBieu == "rAll")
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung.xls";
            else
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_TongHopPhanHoTheoTaiKhoan.xls";       
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaTaiKhoan, Thang);
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
        public ActionResult ViewPDF(String MaTaiKhoan = "", String Thang = "", String LoaiBieu = "")
        {
            HamChung.Language();
            String sFilePath = "";
            if (LoaiBieu == "rAll")
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung.xls";
            else
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_TongHopPhanHoTheoTaiKhoan.xls";       
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaTaiKhoan, Thang);
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
