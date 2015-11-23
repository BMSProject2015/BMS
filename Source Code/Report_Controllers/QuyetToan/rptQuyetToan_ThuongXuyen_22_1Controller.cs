using System;
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
using VIETTEL.Models.QuyetToan;

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_ThuongXuyen_22_1Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1.xls";
        private const String sFilePath_1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_1.xls";
        private const String sFilePath_NgayNguoi = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_NgayNguoi.xls";
        private const String sFilePath_1_NgayNguoi = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_1_NgayNguoi.xls";
        private const String sFilePath_Tien = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_Tien.xls";
        private const String sFilePath_Loi = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_Loi.xls";
        public static String NameFile = "";
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_22_1.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm editsubmit nhận giá trị từ view
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {

            String Thang_Quy = "";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }

            String LoaiBaoCao1 = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao1"]);
            String LoaiBaoCao2 = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao2"]);
            String LoaiBaoCao3 = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao3"]);
            String chkNgayNguoi = Convert.ToString(Request.Form[ParentID + "_chkNgayNguoi"]);

            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["PageLoad"] = 1;
            ViewData["LoaiBaoCao1"] = LoaiBaoCao1;
            ViewData["LoaiBaoCao2"] = LoaiBaoCao2;
            ViewData["LoaiBaoCao3"] = LoaiBaoCao3;
            ViewData["chkNgayNguoi"] = chkNgayNguoi;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_22_1.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String LoaiBaoCao, String chkNgayNguoi)
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
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1" && iID_MaDonVi != "-2" && iID_MaDonVi != Guid.Empty.ToString())
            {
                TenDV = iID_MaDonVi + "  -  " + Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }

            String TenLoaiThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                TenLoaiThangQuy = "Quý";
            }
            else
            {
                TenLoaiThangQuy = "Tháng";
            }
            //tính tổng tiền
            DataTable dtTien = QT_TX_22_1(iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
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
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_thuongxuyen_22_1");
            LoadData(fr, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND, LoaiBaoCao);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Tien", Tien);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// tạo range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND, String LoaiBaoCao)
        {

            String[] arrLoaiBaoCao = LoaiBaoCao.Split(',');
            for (int i = 0; i < arrLoaiBaoCao.Length; i++)
            {
                if (arrLoaiBaoCao[i] == "1")
                {
                    DataTable data = QT_TX_22_1(iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
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
                else if (arrLoaiBaoCao[i] == "2")
                {
                    DataTable dt1 = Get_dtTienLuongXinQuyetToan(Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND, iID_MaTrangThaiDuyet);
                    DataTable dt2 = dt_GTTien(Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);


                    // set tien luong xin quyet toan
                    if (dt1.Rows.Count >= 6)
                    {
                        fr.SetValue("rSiQuan01", dt1.Rows[0][1]);
                        fr.SetValue("rSiQuan02", dt1.Rows[1][1]);
                        fr.SetValue("rSiQuan03", dt1.Rows[2][1]);
                        fr.SetValue("rSiQuan04", dt1.Rows[3][1]);
                        fr.SetValue("rSiQuan05", dt1.Rows[4][1]);
                        fr.SetValue("rSiQuan06", dt1.Rows[5][1]);

                        fr.SetValue("rQNCN01", dt1.Rows[0][2]);
                        fr.SetValue("rQNCN02", dt1.Rows[1][2]);
                        fr.SetValue("rQNCN03", dt1.Rows[2][2]);
                        fr.SetValue("rQNCN04", dt1.Rows[3][2]);
                        fr.SetValue("rQNCN05", dt1.Rows[4][2]);
                        fr.SetValue("rQNCN06", dt1.Rows[5][2]);

                        fr.SetValue("rCNVCQP01", dt1.Rows[0][3]);
                        fr.SetValue("rCNVCQP02", dt1.Rows[1][3]);
                        fr.SetValue("rCNVCQP03", dt1.Rows[2][3]);
                        fr.SetValue("rCNVCQP04", dt1.Rows[3][3]);
                        fr.SetValue("rCNVCQP05", dt1.Rows[4][3]);
                        fr.SetValue("rCNVCQP06", dt1.Rows[5][3]);

                        fr.SetValue("rHD01", dt1.Rows[0][4]);
                        fr.SetValue("rHD02", dt1.Rows[1][4]);
                        fr.SetValue("rHD03", dt1.Rows[2][4]);
                        fr.SetValue("rHD04", dt1.Rows[3][4]);
                        fr.SetValue("rHD05", dt1.Rows[4][4]);
                        fr.SetValue("rHD06", dt1.Rows[5][4]);
                    }
                    //set quan so phai cung cap tien an

                    fr.SetValue("iSoNgayAn_1", dt2.Rows[0]["iSoNgayAn_1"]);
                    fr.SetValue("iSoNgayAn_2", dt2.Rows[0]["iSoNgayAn_2"]);
                    fr.SetValue("iSoNgayAn_3", dt2.Rows[0]["iSoNgayAn_3"]);
                    fr.SetValue("iSoNgayAn_4", dt2.Rows[0]["iSoNgayAn_4"]);

                    //set ra quan trong thang
                    fr.SetValue("iSiQuan_XuatNgu", dt2.Rows[0]["iSiQuan_XuatNgu"]);
                    fr.SetValue("rSiQuan_XuatNgu", dt2.Rows[0]["rSiQuan_XuatNgu"]);
                    fr.SetValue("iQNCN_XuatNgu", dt2.Rows[0]["iQNCN_XuatNgu"]);
                    fr.SetValue("rQNCN_XuatNgu", dt2.Rows[0]["rQNCN_XuatNgu"]);
                    fr.SetValue("iCNVCQP_XuatNgu", dt2.Rows[0]["iCNVCQP_XuatNgu"]);
                    fr.SetValue("rCNVCQP_XuatNgu", dt2.Rows[0]["rCNVCQP_XuatNgu"]);
                    fr.SetValue("iHSQCS_XuatNgu", dt2.Rows[0]["iHSQCS_XuatNgu"]);
                    fr.SetValue("rHSQCS_XuatNgu", dt2.Rows[0]["rHSQCS_XuatNgu"]);

                    fr.SetValue("iSiQuan_Huu", dt2.Rows[0]["iSiQuan_Huu"]);
                    fr.SetValue("rSiQuan_Huu", dt2.Rows[0]["rSiQuan_Huu"]);
                    fr.SetValue("iQNCN_Huu", dt2.Rows[0]["iQNCN_Huu"]);
                    fr.SetValue("rQNCN_Huu", dt2.Rows[0]["rQNCN_Huu"]);
                    fr.SetValue("iCNVCQP_Huu", dt2.Rows[0]["iCNVCQP_Huu"]);
                    fr.SetValue("rCNVCQP_Huu", dt2.Rows[0]["rCNVCQP_Huu"]);
                    fr.SetValue("iHSQCS_Huu", dt2.Rows[0]["iHSQCS_Huu"]);
                    fr.SetValue("rHSQCS_Huu", dt2.Rows[0]["rHSQCS_Huu"]);

                    fr.SetValue("iSiQuan_ThoiViec", dt2.Rows[0]["iSiQuan_ThoiViec"]);
                    fr.SetValue("rSiQuan_ThoiViec", dt2.Rows[0]["rSiQuan_ThoiViec"]);
                    fr.SetValue("iQNCN_ThoiViec", dt2.Rows[0]["iQNCN_ThoiViec"]);
                    fr.SetValue("rQNCN_ThoiViec", dt2.Rows[0]["rQNCN_ThoiViec"]);
                    fr.SetValue("iCNVCQP_ThoiViec", dt2.Rows[0]["iCNVCQP_ThoiViec"]);
                    fr.SetValue("rCNVCQP_ThoiViec", dt2.Rows[0]["rCNVCQP_ThoiViec"]);
                    fr.SetValue("iHSQCS_ThoiViec", dt2.Rows[0]["iHSQCS_ThoiViec"]);
                    fr.SetValue("rHSQCS_ThoiViec", dt2.Rows[0]["rHSQCS_ThoiViec"]);
                }
                else
                {
                    DataTable GiaiThich = dt_GiaiThich(Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
                    DataTable KienNghi = dt_KienNghi(Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
                    int a = GiaiThich.Rows.Count;
                    int b = KienNghi.Rows.Count;

                    if (a < 10)
                    {
                        for (i = 0; i < 11 - a; i++)
                        {
                            DataRow r = GiaiThich.NewRow();
                            GiaiThich.Rows.Add(r);
                        }
                    }
                    if (b < 7)
                    {
                        for (i = 0; i < 7 - b; i++)
                        {
                            DataRow r = KienNghi.NewRow();
                            KienNghi.Rows.Add(r);
                        }
                    }
                    fr.AddTable("GiaiThich", GiaiThich);
                    fr.AddTable("KienNghi", KienNghi);
                }
            }

        }
        /// <summary>
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String LoaiBaoCao, String chkNgayNguoi)
        {
            String DuongDanFile = "";
            if (LoaiBaoCao == "1")
            {
                if (chkNgayNguoi == "on")
                {
                    if (LoaiThang_Quy == "0")
                        DuongDanFile = sFilePath_1_NgayNguoi;
                    else DuongDanFile = sFilePath_NgayNguoi;
                }
                else
                {
                    if (LoaiThang_Quy == "0")
                        DuongDanFile = sFilePath_1;
                    else DuongDanFile = sFilePath;
                }
            }
            else if (LoaiBaoCao == "2")
            {
                DuongDanFile = sFilePath_Tien;
            }
            else
            {
                DuongDanFile = sFilePath_Loi;
            }
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, LoaiBaoCao, chkNgayNguoi);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_ThuongXuyen_22_1.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String LoaiBaoCao, String chkNgayNguoi)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (LoaiBaoCao == "1")
            {
                if (chkNgayNguoi == "on")
                {
                    if (LoaiThang_Quy == "0")
                        DuongDanFile = sFilePath_1_NgayNguoi;
                    else DuongDanFile = sFilePath_NgayNguoi;
                }
                else
                {
                    if (LoaiThang_Quy == "0")
                        DuongDanFile = sFilePath_1;
                    else DuongDanFile = sFilePath;
                }
            }
            else if (LoaiBaoCao == "2")
            {
                DuongDanFile = sFilePath_Tien;
            }
            else
            {
                DuongDanFile = sFilePath_Loi;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, LoaiBaoCao, chkNgayNguoi);
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
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>

        /// <summary>
        /// Quyết toán thường xuyên 22_1
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public DataTable QT_TX_22_1(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        {
            DataTable dt;
            String DKDotNay = "";
            String DKLuyKe = "";
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            //nếu là quý tổng hợp 3 tháng 1 quý
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKDotNay = "(iThang_Quy between 1  and 3)";
                        DKLuyKe = "(iThang_Quy between 1  and 3)";
                        break;
                    case 2: DKDotNay = "(iThang_Quy between 4  and 6)";
                        DKLuyKe = "(iThang_Quy between 1  and 6)";
                        break;
                    case 3: DKDotNay = "(iThang_Quy between 7  and 9)";
                        DKLuyKe = "(iThang_Quy between 1  and 9)";
                        break;
                    case 4: DKDotNay = "(iThang_Quy between 10  and 12)";
                        DKLuyKe = "(iThang_Quy between 1  and 12)";
                        break;

                }

            }
            else
            {
                DKDotNay = " iThang_Quy=@ThangQuy";
                DKLuyKe = " iThang_Quy<=@ThangQuy";
            }
            if (String.IsNullOrEmpty(DKDotNay) || String.IsNullOrEmpty(DKLuyKe))
            {
                if (LoaiThang_Quy == "1")
                {
                    DKDotNay = "(iThang_Quy between 1  and 3)";
                    DKLuyKe = "(iThang_Quy between 1  and 3)";
                }
                else
                {
                    DKDotNay = " iThang_Quy=1";
                    DKLuyKe = "(iThang_Quy=1)";
                }

            }
            //dk ĐƠn vị
            String DKDonVi = "";
            DataTable dtDonVi = QuyetToan_ReportModels.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
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
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe,SUM(rSoNguoi) as rNguoi,SUM(rNgay) as rNgay
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rSoNguoi,rNgay
                                        ,DotNay = CASE WHEN {0} THEN SUM(rTuChi) ELSE 0 END
                                        ,LuyKe = CASE WHEN  {2} THEN SUM(rTuChi) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {3} AND sLNS=1010000 AND sL=460 AND sK=468 AND sNG<>'' {1} AND bLoaiThang_Quy=0 {4}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy,rSoNguoi,rNgay
                                        HAVING SUM(rTuChi)<>0) as a
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDotNay, DKDonVi, DKLuyKe, ReportModels.DieuKien_NganSach(MaND), DKDuyet1);
            SqlCommand cmd = new SqlCommand(SQL);

            if (LoaiThang_Quy != "1")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
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


            dt = Connection.GetDataTable(cmd);

            //tao dt chi tieu
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
            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
                                                 AND sLNS=1010000 AND sL=460 AND sK= 468 AND sNG<>'' AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay AND PB_PhanBoChiTiet.iTrangThai=1 {0} {1} 
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", DKDonVi, DKDuyet);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
            cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmdChiTieu.Parameters.AddWithValue("@dNgay", iThangQuy);
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
            if (iID_MaTrangThaiDuyet == "0")
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }
            DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();

            //ghep 2 dt
            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
            String[] arrCol = sCol.Split(',');
            dt.Columns.Add("ChiTieu", typeof(Decimal));
            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS={7} AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
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
            dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG";
            dt = dv.ToTable();
            return dt;


        }
        public DataTable dt_GiaiThich(String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        {
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1") DKThangQuy = "iThang_Quy IN (1,2,3)";
                else if (Thang_Quy == "2") DKThangQuy = "iThang_Quy IN(4,5,6)";
                else if (Thang_Quy == "3") DKThangQuy = "iThang_Quy IN(7,8,9)";
                else DKThangQuy = "iThang_Quy IN(10,11,12)";
            }
            else
                DKThangQuy = "iThang_Quy=@iThang_Quy";
            String SQL = String.Format(@"SELECT sGiaiThich FROM QTA_GiaiThichBangLoi
                                         WHERE iTrangThai=1  AND {1}  AND iID_MaDonVi=@iID_MaDonVi  {0}", ReportModels.DieuKien_NganSach(MaND), DKThangQuy);

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (LoaiThang_Quy != "1")
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable dt_KienNghi(String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        {
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1") DKThangQuy = "iThang_Quy IN (1,2,3)";
                else if (Thang_Quy == "2") DKThangQuy = "iThang_Quy IN(4,5,6)";
                else if (Thang_Quy == "3") DKThangQuy = "iThang_Quy IN(7,8,9)";
                else DKThangQuy = "iThang_Quy IN(10,11,12)";
            }
            else DKThangQuy = "iThang_Quy=@iThang_Quy";
            String SQL = String.Format(@"SELECT sKienNghi FROM QTA_GiaiThichBangLoi
                                         WHERE iTrangThai=1 AND  {1}  AND iID_MaDonVi=@iID_MaDonVi  {0}", ReportModels.DieuKien_NganSach(MaND), DKThangQuy);

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (LoaiThang_Quy != "1")
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_dtTienLuongXinQuyetToan(String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND, String iID_MaTrangThaiDuyet)
        {
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1") DKThangQuy = "iThang_Quy IN (1,2,3)";
                else if (Thang_Quy == "2") DKThangQuy = "iThang_Quy IN(4,5,6)";
                else if (Thang_Quy == "3") DKThangQuy = "iThang_Quy IN(7,8,9)";
                else DKThangQuy = "iThang_Quy IN(10,11,12)";
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThang_Quy";
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                DKDuyet = "";
            }
            String SQL = String.Format(@"SELECT ISNULL(SUM(rTuChi),0) as rTuChi FROM QTA_ChungTuChiTiet 
                        WHERE  iTrangThai=1 AND iID_MaDonVi=@iiD_MaDonVi AND {0} {1} {2}
                        AND sXauNoiMa IN (SELECT sXauNoiMa FROM BH_CauHinh_DoiTuong_NganSach WHERE sMaTX=@sMaTX)", DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (LoaiThang_Quy != "1")
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            if (iID_MaTrangThaiDuyet == "0")
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            cmd.Parameters.AddWithValue("@sMaTX", "A1");
            Double A1 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "A2";
            Double A2 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "A3";
            Double A3 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "A4";
            Double A4 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "B1";
            Double B1 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "B2";
            Double B2 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "B3";
            Double B3 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sMaTX"].Value = "B4";
            Double B4 = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            //DataTable dt = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);

            SQL = String.Format(@"SELECT SUM(rLuongCoBan) as rLuongCoBan FROM QTA_QuyetToanBaoHiem 
                    WHERE iID_MaDonVi=@iID_MaDonVi AND {0} {1}  AND SUBSTRING(sKyHieuDoiTuong,2,1)=@sKyHieuDoiTuong", DKThangQuy, ReportModels.DieuKien_NganSach(MaND));
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (LoaiThang_Quy != "1")
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            cmd.Parameters.AddWithValue("@sKyHieuDoiTuong", "1");
            Double LCB1 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sKyHieuDoiTuong"].Value = "2";
            Double LCB2 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sKyHieuDoiTuong"].Value = "3";
            Double LCB3 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            cmd.Parameters["@sKyHieuDoiTuong"].Value = "4";
            Double LCB4 = Convert.ToDouble(Connection.GetValue(cmd, 0));

            SQL = String.Format(@"SELECT SUM(rTongSo) - SUM(rLuongCoBan) as rPhuCap FROM QTA_QuyetToanBaoHiem 
                    WHERE iID_MaDonVi=@iID_MaDonVi AND {0} {1}  AND SUBSTRING(sKyHieuDoiTuong,2,1)=@sKyHieuDoiTuong", DKThangQuy, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            cmd.Parameters["@sKyHieuDoiTuong"].Value = "1";
            Double PC1 = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Parameters["@sKyHieuDoiTuong"].Value = "2";
            Double PC2 = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Parameters["@sKyHieuDoiTuong"].Value = "3";
            Double PC3 = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Parameters["@sKyHieuDoiTuong"].Value = "4";
            Double PC4 = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            DataTable dtKQ = new DataTable();
            dtKQ.Columns.Add("iHang", typeof(Int16));
            dtKQ.Columns.Add("rSiQuan", typeof(Decimal));
            dtKQ.Columns.Add("rQNCN", typeof(Decimal));
            dtKQ.Columns.Add("rCNVCQP", typeof(Decimal));
            dtKQ.Columns.Add("rHopDong", typeof(Decimal));
            DataRow R = dtKQ.NewRow();
            R[0] = 1;
            R[1] = LCB1 + A1;
            R[2] = LCB2 + A2;
            R[3] = LCB3 + A3;
            R[4] = LCB4 + A4;
            dtKQ.Rows.Add(R);

            R = dtKQ.NewRow();
            R[0] = 2;
            R[1] = PC1 + B1;
            R[2] = PC2 + B2;
            R[3] = PC3 + B3;
            R[4] = PC4 + B4;
            dtKQ.Rows.Add(R);

            R = dtKQ.NewRow();
            R[0] = 3;
            R[1] = LCB1;
            R[2] = LCB2;
            R[3] = LCB3;
            R[4] = LCB4;
            dtKQ.Rows.Add(R);

            R = dtKQ.NewRow();
            R[0] = 4;
            R[1] = PC1;
            R[2] = PC2;
            R[3] = PC3;
            R[4] = PC4;
            dtKQ.Rows.Add(R);

            R = dtKQ.NewRow();
            R[0] = 5;
            R[1] = A1;
            R[2] = A2;
            R[3] = A3;
            R[4] = A4;
            dtKQ.Rows.Add(R);

            R = dtKQ.NewRow();
            R[0] = 6;
            R[1] = B1;
            R[2] = B2;
            R[3] = B3;
            R[4] = B4;
            dtKQ.Rows.Add(R);
            return dtKQ;
        }
        public DataTable dt_GTTien(String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        {

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
            String SQL = String.Format(@"SELECT 
SUM(iSoNgayAn_1) as iSoNgayAn_1,
	   SUM(iSoNgayAn_2) as iSoNgayAn_2,
	   SUM(iSoNgayAn_3) as iSoNgayAn_3,
	   SUM(iSoNgayAn_4) as iSoNgayAn_4,
iSiQuan_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN iSiQuan ELSE 0 END)
,rSiQuan_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN rSiQuan ELSE 0 END)
,iQNCN_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN iQNCN ELSE 0 END)
,rQNCN_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN rQNCN ELSE 0 END)
,iCNVCQP_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN iCNVCQP ELSE 0 END)
,rCNVCQP_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN rCNVCQP ELSE 0 END)
,iHSQCS_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN iHSQCS ELSE 0 END)
,rHSQCS_XuatNgu=SUM(CASE WHEN sKyHieuDoiTuong=310 THEN rHSQCS ELSE 0 END)
,iSiQuan_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN iSiQuan ELSE 0 END)
,rSiQuan_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN rSiQuan ELSE 0 END)
,iQNCN_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN iQNCN ELSE 0 END)
,rQNCN_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN rQNCN ELSE 0 END)
,iCNVCQP_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN iCNVCQP ELSE 0 END)
,rCNVCQP_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN rCNVCQP ELSE 0 END)
,iHSQCS_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN iHSQCS ELSE 0 END)
,rHSQCS_Huu=SUM(CASE WHEN sKyHieuDoiTuong=320 THEN rHSQCS ELSE 0 END)
,iSiQuan_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN iSiQuan ELSE 0 END)
,rSiQuan_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN rSiQuan ELSE 0 END)
,iQNCN_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN iQNCN ELSE 0 END)
,rQNCN_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN rQNCN ELSE 0 END)
,iCNVCQP_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN iCNVCQP ELSE 0 END)
,rCNVCQP_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN rCNVCQP ELSE 0 END)
,iHSQCS_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN iHSQCS ELSE 0 END)
,rHSQCS_ThoiViec=SUM(CASE WHEN sKyHieuDoiTuong=330 THEN rHSQCS ELSE 0 END)
FROM QTA_GiaiThichSoTien
 WHERE iTrangThai=1 AND  {1}  AND iID_MaDonVi=@iID_MaDonVi  {0}", ReportModels.DieuKien_NganSach(MaND), DKThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (LoaiThang_Quy != "1")
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
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

        public static String SoQuyetToan(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        {


            DataTable dt;
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            String DKThang_Quy = "";

            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "(iThang_Quy between 1  and 3)";

                        break;
                    case 2: DKThang_Quy = "(iThang_Quy between 4  and 6)";

                        break;
                    case 3: DKThang_Quy = "(iThang_Quy between 7  and 9)";

                        break;
                    case 4: DKThang_Quy = "(iThang_Quy between 10  and 12)";

                        break;
                    default: DKThang_Quy = "iThang_Quy=-1";
                        break;
                }

            }
            else
            {
                DKThang_Quy = " iThang_Quy=@ThangQuy";
            }

            String DKDonVi = "";
            DataTable dtDonVi = QuyetToan_ReportModels.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
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
            String SQL = String.Format(@"SELECT SUM(DotNay) as DotNay
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = CASE WHEN {4} THEN SUM({1}) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {2} AND sLNS=1010000 AND sNG<>'' {0} AND bLoaiThang_Quy=0 {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        ) as a                                        
                                        HAVING SUM(DotNay)<>0", DKDonVi, "rTuChi", ReportModels.DieuKien_NganSach(MaND), DKDuyet1, DKThang_Quy);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
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


            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSoQT = "";
            if (dt.Rows.Count > 0) sSoQT = Convert.ToString(dt.Rows[0]["DotNay"]);
            return CommonFunction.DinhDangSo(sSoQT);
        }
        public static String TongQuyetToan(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        {
            DataTable dt;
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String DKThang_Quy = "";

            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "(iThang_Quy between 1  and 3)";

                        break;
                    case 2: DKThang_Quy = "(iThang_Quy between 4  and 6)";

                        break;
                    case 3: DKThang_Quy = "(iThang_Quy between 7  and 9)";

                        break;
                    case 4: DKThang_Quy = "(iThang_Quy between 10  and 12)";
                        break;
                    default: DKThang_Quy = "(iThang_Quy =-1)";

                        break;
                }

            }
            else
            {
                DKThang_Quy = " iThang_Quy=@ThangQuy";
            }
            //DK đơn vị
            String DKDonVi = "";
            DataTable dtDonVi = QuyetToan_ReportModels.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);

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
            String SQL = String.Format(@"SELECT SUM(DotNay) as DotNay
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = CASE WHEN {4} THEN SUM({1}) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {2} AND sLNS='1010000' AND sNG<>'' {0} AND bLoaiThang_Quy=0 {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        ) as a                                        
                                        HAVING SUM(DotNay)<>0", DKDonVi, "rTuChi", ReportModels.DieuKien_NganSach(MaND), DKDuyet1, DKThang_Quy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@ThangQuy", NgayChiTieu);
            cmd.Parameters.AddWithValue("@LoaiThangQuy", LoaiThang_Quy);

            for (int i = 1; i < dtDonVi.Rows.Count; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
            }


            //cmd.Parameters.AddWithValue("@sLNS", sLNS);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sTongQT = "";
            if (dt.Rows.Count > 0) sTongQT = Convert.ToString(dt.Rows[0]["DotNay"]);
            return CommonFunction.DinhDangSo(sTongQT);
        }

        //        public static String TongSo(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String LoaiThang_Quy, String MaND)
        //        {
        //            DataTable dt;
        //            String DKDotNay = "";

        //            int iThangQuy = Convert.ToInt32(Thang_Quy);
        //            //nếu là quý tổng hợp 3 tháng 1 quý
        //            if (LoaiThang_Quy == "1")
        //            {
        //                switch (iThangQuy)
        //                {
        //                    case 1: DKDotNay = "(iThang_Quy between 1  and 3)";

        //                        break;
        //                    case 2: DKDotNay = "(iThang_Quy between 4  and 6)";

        //                        break;
        //                    case 3: DKDotNay = "(iThang_Quy between 7  and 9)";

        //                        break;
        //                    case 4: DKDotNay = "(iThang_Quy between 10  and 12)";

        //                        break;

        //                }
        //                iThangQuy = iThangQuy * 3;

        //            }
        //            else
        //            {
        //                DKDotNay = " iThang_Quy=@ThangQuy";
        //            }
        //            //dk ĐƠn vị
        //            string DKDonVi = "";

        //            if (iID_MaTrangThaiDuyet == "0")
        //            {
        //                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
        //            }
        //            else
        //            {
        //                iID_MaTrangThaiDuyet = " ";
        //            }
        //            String SQL = String.Format(@"SELECT SUM(DotNay) as DotNay
        //                                        FROM(
        //                                        SELECT 
        //                                        DotNay = CASE WHEN {0} THEN SUM(rTuChi) ELSE 0 END                                        
        //                                        FROM QTA_ChungTuChiTiet
        //                                        WHERE iTrangThai=1 {2} AND sLNS=1010000 AND sL=460 AND sK=468 AND sNG<>'' {1} AND bLoaiThang_Quy=0 
        //                                        GROUP BY iThang_Quy
        //                                        HAVING SUM(rTuChi)<>0) as a
        //                                        
        //                                        HAVING SUM(DotNay)<>0 ", DKDotNay, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
        //            SqlCommand cmd = new SqlCommand(SQL);

        //            if (LoaiThang_Quy != "1")
        //            {
        //                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
        //            }

        //            dt = Connection.GetDataTable(cmd);
        //            cmd.Dispose();
        //            String sTongQT = "";
        //            if (dt.Rows.Count > 0) sTongQT = Convert.ToString(dt.Rows[0]["DotNay"]);
        //            return CommonFunction.DinhDangSo(sTongQT);

        //        }


        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, MaND), JsonRequestBehavior.AllowGet);
        }
        Object item = new
        {
            dsDonVi = ""
        };
        public Object obj_DSDonVi(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String MaND)
        {
            String dsDonVi = "";
            DataTable dtDonVi = QuyetToan_ReportModels.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenDV");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 99%\" onchange=\"Chon();\"");
            String sSoQT = SoQuyetToan(iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
            //String sSoQT = DonVi(iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
            String sTongQT = TongQuyetToan(iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, LoaiThang_Quy, MaND);
            item = new
            {
                dsDonVi = dsDonVi,
                sSoQT = sSoQT,
                sTongQT = sTongQT
            };
            return item;
        }
        public static DataTable dtLoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai");
            dt.Columns.Add("TenLoai");

            DataRow r;
            r = dt.NewRow();
            r[0] = "1";
            r[1] = "Tờ số liệu";
            dt.Rows.Add(r);
            r = dt.NewRow();
            r[0] = "2";
            r[1] = "Tờ giải thích bằng số";
            dt.Rows.Add(r);
            r = dt.NewRow();
            r[0] = "3";
            r[1] = "Tờ giải thích bằng lời";
            dt.Rows.Add(r);
            dt.Dispose();
            return dt;
        }
    }
}
