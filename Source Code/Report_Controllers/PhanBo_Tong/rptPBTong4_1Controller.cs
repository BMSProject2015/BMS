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

namespace VIETTEL.Report_Controllers.PhanBo_Tong
{
    public class rptPBTong4_1Controller : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath_Dot1 = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong4_1.xls";
        private const String sFilePath_Doc = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong4_1_doc.xls";
        private const String sFilePath_Ngang = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong4_1_ngang.xls";
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="iFile"></param>
        /// <returns></returns>
        public ActionResult Index(int? iFile = 0)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo_Tong/rptPBTong4_1.aspx";
                ViewData["iFile"] = iFile;
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String DotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String ThongBao = Request.Form[ParentID + "_ThongBao"];
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(User.Identity.Name, iID_MaTrangThaiDuyet);
            int iFIle = 0;
            if (dtDotPhanBo.Rows.Count > 1)
            {
                if (DotPhanBo == Convert.ToString(dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"]))
                {
                    iFIle = 0;
                }
                else
                {
                    iFIle = 1;
                }
            }
            //String Muc = Request.Form[ParentID + "_Muc"];
            String KieuTrang = Request.Form[ParentID + "_KieuTrang"];
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["DotPhanBo"] = DotPhanBo;
            ViewData["ThongBao"] = ThongBao;
            //ViewData["Muc"] = Muc;
            ViewData["KieuTrang"] = KieuTrang;
            ViewData["PageLoad"] = "1";
            ViewData["iFIle"] = iFIle;
            ViewData["path"] = "~/Report_Views/PhanBo_Tong/rptPBTong4_1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult DongNoiDung(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_ThamSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            foreach (String key in Request.Form.AllKeys)
            {
                String dong = Request.Form[key];
                SqlCommand cmd = new SqlCommand("UPDATE DC_ThamSo SET sThamSo=@sThamSo WHERE sNoiDung=@sNoiDung AND sKyHieu='rptPhanBo_4_1'");
                cmd.Parameters.AddWithValue("@sNoiDung", key);
                cmd.Parameters.AddWithValue("@sThamSo", dong);
                Connection.UpdateDatabase(cmd);
            }

            ViewData["path"] = "~/Report_Views/PhanBo/rptPB_CapChiTieuNganSachNam_From.aspx";
            return RedirectToAction("Index", "rptThongBaoCapChiTieuNganSachNam");
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String ThongBao, int iFile, String KieuTrang)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String Dot = "", SoDot = "", Dot_1 = "";
            String TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            //lấy ngày hiện tại
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            String TenDot = "";
            if (ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo) > 1)
            {

                Dot = "Đợt " + (ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo) - 1);
                Dot_1 = "đợt " + (ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo) - 1);
                for (int i = 2; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        TenDot = dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString();
                        if (!String.IsNullOrEmpty(TenDot))
                        {
                            TenDot = " Tháng " + TenDot.Substring(3, 2) + " Năm " + TenDot.Substring(6, 4);
                        }
                    }
                }
                SoDot = Convert.ToString(ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo) - 1);
            }
            DataTable dt = PB_ChiTieuNganSachNam(MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, iID_MaDotPhanBo, ThongBao, iFile);
            long TongTien = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["rTongSo"].ToString() != "")
                {
                    TongTien += long.Parse(dt.Rows[i]["rTongSo"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            String _ThongBao = "";
            if (ThongBao == "1") _ThongBao = "CẤP";
            else _ThongBao = "THU";
            // dòng nội dung
            String[] NoiDung = new String[5];
            DataTable dtNoiDung = LayDongNoiDung();
            for (int i = 0; i < dtNoiDung.Rows.Count; i++)
            {

                NoiDung[i] = Convert.ToString(dtNoiDung.Rows[i]["sThamSo"]);
            }
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongBaoCapChiTieuNganSachNam");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, iID_MaDotPhanBo, ThongBao, iFile);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("TenDv", TenDV);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Tien", Tien);
            for (int i = 0; i < dtNoiDung.Rows.Count; i++)
            {
                fr.SetValue("noidung" + Convert.ToString(i), NoiDung[i]);
            }
            fr.SetValue("Dot", Dot);
            fr.SetValue("Dot_1", Dot_1);
            fr.SetValue("SoDot", SoDot);
            fr.SetValue("TenDot", TenDot);
            fr.SetValue("ThongBao", _ThongBao);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// tạo các Range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String iID_MaDotPhanBo, String ThongBao, int iFile)
        {
            DataTable data = PB_ChiTieuNganSachNam(MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, iID_MaDotPhanBo, ThongBao, iFile);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);


        }
        /// <summary>
        /// Xuất ra Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iFile"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String ThongBao, int iFile, String KieuTrang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String DuongDan = "";
            if (iFile == 0)
            {
                DuongDan = sFilePath_Dot1;
            }
            else
            {
                //doc
                if (KieuTrang == "1")
                {
                    DuongDan = sFilePath_Doc;
                }
                else DuongDan = sFilePath_Ngang;
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, ThongBao, iFile, KieuTrang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptThongBaoCapChiTieuNganSachNam.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iFile"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDonVi, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String ThongBao, int iFile, String KieuTrang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String DuongDan = "";
            if (iFile == 0)
            {
                DuongDan = sFilePath_Dot1;
            }
            else
            {
                //doc
                if (KieuTrang == "1")
                {
                    DuongDan = sFilePath_Doc;
                }
                else DuongDan = sFilePath_Ngang;
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, ThongBao, iFile, KieuTrang);
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
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, String iID_MaDonVi)
        {

            return Json(obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        public class cl_DotPhanBo
        {
            public String iID_MaDotPhanBo { get; set; }
            public String iID_MaDonVi { get; set; }
        }
        public cl_DotPhanBo obj_DSDotPhanBo(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, String iID_MaDonVi)
        {
            cl_DotPhanBo a = new cl_DotPhanBo();
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet);
            SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            a.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonDotPhanBo()\"");
            dtDotPhanBo.Dispose();
            DataTable dtDonVi = PhanBo_ReportModels.LayDSDonViTong(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, false, false);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            a.iID_MaDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            dtDotPhanBo.Dispose();
            return a;
        }
        /// <summary>
        /// PB_ChiTieuNganSachNam
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public DataTable PB_ChiTieuNganSachNam(String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String iID_MaDotPhanBo, String ThongBao, int iFile)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (ThongBao == "1")
            {
                String SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        SUM(rTuChi) as rTuChi,SUM(rHienVat) as rHienVat,SUM(rTuChi)+SUM(rHienVat) as rTongSo
                                        FROM PB_PhanBoChiTiet 
                                        WHERE iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo))  AND (rTuChi > 0 OR rHienVat>0)  AND iID_MaDonVi=@iID_MaDonVi AND sNG<>'' AND iTrangThai=1 {1} {0}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                        ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
            else
            {
                String SQL = "";
                if (iFile == 1)
                {
                    SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        SUM(-rTuChi) as rTuChi,SUM(-rHienVat) as rHienVat,SUM(-rTuChi)+SUM(-rHienVat) as rTongSo
                                        FROM PB_PhanBoChiTiet 
                                        WHERE iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo))  AND iID_MaDonVi=@iID_MaDonVi AND sNG<>'' AND (rTuChi < 0 OR rHienVat<0) AND iTrangThai=1 {1} {0}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                        ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
                }
                else
                {
                    SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        SUM(rTuChi) as rTuChi,SUM(rHienVat) as rHienVat,SUM(rTuChi)+SUM(rHienVat) as rTongSo
                                        FROM PB_PhanBoChiTiet 
                                        WHERE iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo) )AND (rTuChi > 0 OR rHienVat>0)  AND iID_MaDonVi=@iID_MaDonVi AND sNG<>'' AND iTrangThai=1 {1} {0}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                        ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
                }
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }

        }
        public static DataTable LayDongNoiDung()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String SQL = string.Format(@"SELECT  sNoiDung,sThamSo FROM DC_ThamSo WHERE sKyHieu='rptPhanBo_4_1' AND iTrangThai=1");
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

    }
}

