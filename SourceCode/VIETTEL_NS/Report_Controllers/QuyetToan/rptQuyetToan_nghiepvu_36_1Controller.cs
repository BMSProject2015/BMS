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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_nghiepvu_36_1Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_nghiepvu_36_1.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_nghiepvu_36_1.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String Thang_Quy = "";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String iID_MaPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_nghiepvu_36_1.aspx";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["TruongTien"] = TruongTien;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien)
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
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

            String TenLoaiThangQuy = "";
            String TenLoaiThangQuy1 = "";
            if (LoaiThang_Quy == "1")
            {
                TenLoaiThangQuy = "quý";
                TenLoaiThangQuy1 = "Quý";
            }
            else
            {
                TenLoaiThangQuy = "tháng";
                TenLoaiThangQuy1 = "Tháng";
            }
            //lấy mô tả lns
            String TenLNS = "";
            DataTable dt = MoTa(sLNS);
            if (dt.Rows.Count > 0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            //tính tổng tiền
            DataTable dtTien = QT_NV_36_1(iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien, MaND);
            long TongTien = 0;
            for (int i = 0; i < dtTien.Rows.Count; i++)
            {
                if (dtTien.Rows[i]["DotNay"].ToString() != "")
                {
                    TongTien += long.Parse(dtTien.Rows[i]["DotNay"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);

            String TenKhoi = Convert.ToString(CommonFunction.LayTruong("NS_PhongBan", "iID_MaPhongBan", iID_MaPhongBan, "sTen"));

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_nghiepvu_36_1");
            LoadData(fr, iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien, MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("LoaiThangQuy1", TenLoaiThangQuy1);
            fr.SetValue("TenLNS", TenLNS);
            fr.SetValue("TruongTien", TruongTien);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Tien", Tien);
            fr.SetValue("TenKhoi", TenKhoi);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// tạo các range trong báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        private void LoadData(FlexCelReport fr, String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThangQuy, String sLNS, String TruongTien, String MaND)
        {
            DataTable data = QT_NV_36_1(iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThangQuy, sLNS, TruongTien, MaND);
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
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien);
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
        /// Xuất ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_NghiepVu_36_1.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien);
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
        /// Onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID, String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String sLNS, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String TruongTien)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonVi(ParentID, iID_MaPhongBan, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, TruongTien, MaND), JsonRequestBehavior.AllowGet);
        }
        Object item = new
        {
            dsDonVi = ""
        };
        public static DataTable NS_LoaiNganSachNghiepVuKhac_PhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS as sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7  AND A.sLNS <> '1010000' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public Object obj_DSDonVi(String ParentID, String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String sLNS, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String TruongTien, String MaND)
        {
            String dsDonVi = "", dsLNS = "";
            DataTable dtLNS = NS_LoaiNganSachNghiepVuKhac_PhongBan(iID_MaPhongBan);
            SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
            dsLNS = MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 99%\" onchange=\"ChonLNS();\"");
            DataTable dtDonVi = LayDSDonVi(iID_MaPhongBan, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, TruongTien, MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 99%\" onchange=\"ChonDV();\" ");
            String sSoQT = SoQuyetToan(iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien, MaND);
            String sTongQT = TongQuyetToan(iID_MaPhongBan, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, sLNS, TruongTien, MaND);
            item = new
            {
                dsDonVi = dsDonVi,
                dsLNS = dsLNS,
                sSoQT = sSoQT,
                sTongQT = sTongQT
            };
            return item;
        }

        public static String SoQuyetToan(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien, String MaND)
        {
            DataTable dt;
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String DKThangQuy = "";
              if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1") DKThangQuy = "iThang_Quy IN (1,2,3)";
                else if (Thang_Quy == "2") DKThangQuy = "iThang_Quy IN(4,5,6)";
                else if (Thang_Quy == "3") DKThangQuy = "iThang_Quy IN(7,8,9)";
                else if (Thang_Quy == "4") DKThangQuy = "iThang_Quy IN(10,11,12)";
                else DKThangQuy = "iThang_Quy IN(-1)";
            }
            else
                DKThangQuy = "iThang_Quy=@iThang_Quy";
            //DK đơn vị
            String DKDonVi = "";
            DataTable dtDonVi = LayDSDonVi(iID_MaPhongBan, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, TruongTien, MaND);
            if (iID_MaDonVi == "-1")
            {

                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";
                }
                if (!String.IsNullOrEmpty(DKDonVi))
                {
                    DKDonVi = " AND(" + DKDonVi + ")";
                }
                else
                {
                    DKDonVi = " AND iID_MaDonVi='" + Guid.Empty.ToString() + "'";
                }

            }
            else if (iID_MaDonVi == "-2")
            {
                DKDonVi = " AND iID_MaDonVi='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            String SQL = String.Format(@"SELECT SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = CASE WHEN {4} THEN SUM({1}) ELSE 0 END
                                        ,LuyKe = CASE WHEN  iThang_Quy<=@iThang_Quy THEN SUM({1}) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {2} AND sLNS=@sLNS AND sNG<>'' {0}  {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        ) as a                                        
                                        HAVING SUM(DotNay)<>0", DKDonVi, TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet1,DKThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", NgayChiTieu);
            //cmd.Parameters.AddWithValue("@LoaiThangQuy", 0);
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }

            cmd.Parameters.AddWithValue("@sLNS", sLNS);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSoQT = "";
            if (dt.Rows.Count > 0) sSoQT = Convert.ToString(dt.Rows[0]["DotNay"]);
            return CommonFunction.DinhDangSo(sSoQT);
        }
        public static String TongQuyetToan(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien, String MaND)
        {
            DataTable dt;
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1") DKThangQuy = "iThang_Quy IN (1,2,3)";
                else if (Thang_Quy == "2") DKThangQuy = "iThang_Quy IN(4,5,6)";
                else if (Thang_Quy == "3") DKThangQuy = "iThang_Quy IN(7,8,9)";
                else if (Thang_Quy == "4") DKThangQuy = "iThang_Quy IN(10,11,12)";
                else DKThangQuy = "iThang_Quy IN(-1)";
            }
            else
                DKThangQuy = "iThang_Quy=@iThang_Quy";
            //DK đơn vị
            String DKDonVi = "";
            DataTable dtDonVi = LayDSDonVi(iID_MaPhongBan, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, TruongTien, MaND);

            for (int i = 1; i < dtDonVi.Rows.Count; i++)
            {
                DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < dtDonVi.Rows.Count - 1)
                    DKDonVi += " OR ";
            }
            if (!String.IsNullOrEmpty(DKDonVi))
            {
                DKDonVi = " AND(" + DKDonVi + ")";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi='" + Guid.Empty.ToString() + "'";
            }
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            String SQL = String.Format(@"SELECT SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = CASE WHEN {4} THEN SUM({1}) ELSE 0 END
                                        ,LuyKe = CASE WHEN  iThang_Quy<=@iThang_Quy THEN SUM({1}) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {2} AND sLNS=@sLNS AND sNG<>'' {0} {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        ) as a                                        
                                        HAVING SUM(DotNay)<>0", DKDonVi, TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet1,DKThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang_Quy", NgayChiTieu);
            cmd.Parameters.AddWithValue("@LoaiThangQuy", LoaiThang_Quy);

            for (int i = 1; i < dtDonVi.Rows.Count; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
            }


            cmd.Parameters.AddWithValue("@sLNS", sLNS);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sTongQT = "";
            if (dt.Rows.Count > 0) sTongQT = Convert.ToString(dt.Rows[0]["DotNay"]);
            return CommonFunction.DinhDangSo(sTongQT);
        }
        /// <summary>
        /// Quyết toán nghiệp vụ 36_1
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public DataTable QT_NV_36_1(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String sLNS, String TruongTien, String MaND)
        {
            DataTable dt;
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            //DK đơn vị
            String DKDonVi = "";
            DataTable dtDonVi = LayDSDonVi(iID_MaPhongBan, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, TruongTien, MaND);
            if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";
                }
                DKDonVi = " AND(" + DKDonVi + ")";
            }
            else if (iID_MaDonVi == "-2")
            {
                DKDonVi = " AND iID_MaDonVi='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            String DKThangQuy = "", DKThangQuyLK = "";
            if (LoaiThang_Quy == "0")
            {
                DKThangQuy = "iThang_Quy=@ThangQuy";
                DKThangQuyLK = "iThang_Quy<=@ThangQuy";
            }
            else
            {
                if (Thang_Quy == "1")
                {
                    DKThangQuy = "iThang_Quy IN(1,2,3)";
                    DKThangQuyLK = "iThang_Quy<=3";
                }
                else if (Thang_Quy == "2")
                {
                    DKThangQuy = "iThang_Quy IN(4,5,6)";
                    DKThangQuyLK = "iThang_Quy<=6";
                }
                else if (Thang_Quy == "3")
                {
                    DKThangQuy = "iThang_Quy IN(7,8,9)";
                    DKThangQuyLK = "iThang_Quy<=9";
                }
                else
                {
                    DKThangQuy = "iThang_Quy IN(10,11,12)";
                    DKThangQuyLK = "iThang_Quy<=12";
                }
            }
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = CASE WHEN {4} THEN SUM({1}) ELSE 0 END
                                        ,LuyKe = CASE WHEN  {5} THEN SUM({1}) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {2} AND sLNS=@sLNS AND sNG<>'' {0}  {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        HAVING SUM({1})<>0) as a
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDonVi, TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet1, DKThangQuy, DKThangQuyLK);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Convert.ToInt16(Thang_Quy) * 3);
            }
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }

            cmd.Parameters.AddWithValue("@sLNS", sLNS);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
            }
            else
            {
                DKDuyet = "";
            }
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM({0}) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                  WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
                                                AND sLNS=@sLNS AND sNG<>'' AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay  AND PB_PhanBoChiTiet.iTrangThai=1 {1} AND sNG<>'' {2}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM({0})<>0", TruongTien, DKDonVi, DKDuyet);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
            cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmdChiTieu.Parameters.AddWithValue("@dNgay", NgayChiTieu);

            cmdChiTieu.Parameters.AddWithValue("@sLNS", sLNS);
            if (iID_MaTrangThaiDuyet == "0")
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }

            DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();
            #region  //Ghép DTChiTieu vào dt
            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
            String[] arrCol = sCol.Split(',');

            dt.Columns.Add("ChiTieu", typeof(Decimal));

            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                  dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                  dtChiTieu.Rows[i]["sK"],
                                                  dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                  dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"]
                                                  );
                DataRow[] R = dt.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
                    }
                    dt.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtChiTieu.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < arrCol.Length - 2; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

                                break;
                            }

                        }
                    }

                }

            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dt.DefaultView;
            dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG";
            dt = dv.ToTable();
            #endregion
            return dt;

        }
        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="sLNS">LNS</param>
        /// <param name="Thang_Quy">Tháng làm việc hay quý làm việc</param>
        /// <param name="LoaiThang_Quy">0: tháng ;1: quý</param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iID_MaPhongBan, String iID_MaTrangThaiDuyet, String sLNS, String Thang_Quy, String LoaiThang_Quy, String TruongTien, String MaND)
        {
            String DKThangQuy = "";
            if (LoaiThang_Quy == "0")
            {
                DKThangQuy = "iThang_Quy<=@ThangQuy";
            }
            else
            {
                if (Thang_Quy == "1")
                    DKThangQuy = "iThang_Quy <=3";
                else if (Thang_Quy == "2")
                    DKThangQuy = "iThang_Quy <=6";
                else if (Thang_Quy == "3")
                    DKThangQuy = "iThang_Quy <=9";
                else
                    DKThangQuy = "iThang_Quy <=12";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            String DKDuyet = "", DKDuyet_PB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                DKDuyet_PB = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
            }
            else
            {
                DKDuyet = "";
                DKDuyet_PB = "";
            }
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }

            String SQL = String.Format(@"SELECT DISTINCT b.sTen,a.iID_MaDonVi
                                        FROM (SELECT iID_MaDonVi
	                                          FROM QTA_ChungTuChiTiet
	                                          WHERE  iTrangThai=1 {1}
	                                                AND iID_MaPhongBan=@iID_MaPhongBan AND sLNS=@sLNS AND {3} {2} AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi
                                        UNION  
                                        SELECT DISTINCT b.sTen,a.iID_MaDonVi
                                        FROM (SELECT iID_MaDonVi
	                                          FROM PB_PhanBoChiTiet
	                                          WHERE  iTrangThai=1 {1} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
	                                         AND iID_MaPhongBan=@iID_MaPhongBan   AND sLNS=@sLNS {4} AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi      
                                        ORDER BY   iID_MaDonVi                           

", TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKThangQuy, DKDuyet_PB);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
            //cmd.Parameters.AddWithValue("@LoaiThangQuy", LoaiThang_Quy);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("dNgay", NgayChiTieu);
            if (iID_MaTrangThaiDuyet == "0")
            {
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtDonVi.Rows.Count > 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "--Chọn đơn vị--";
                dtDonVi.Rows.InsertAt(R, 0);

                R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả đơn vị";
                dtDonVi.Rows.InsertAt(R, 1);
            }
            else
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            return dtDonVi;
        }
        /// <summary>
        /// Lấy mô tả LNS
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
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
