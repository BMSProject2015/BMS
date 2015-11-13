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
    public class rptQuyetToan_NghiepVu_46AController : Controller
    {
        //
        // GET: /rptQuyetToan_46A/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A3_2.xls";

        private const String sFilePath_A4_1_RG = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A4_1_RG.xls";
        private const String sFilePath_A4_2_RG = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A4_2_RG.xls";
        private const String sFilePath_A3_1_RG = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A3_1_RG.xls";
        private const String sFilePath_A3_2_RG = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_34_A3_2_RG.xls";
        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_NghiepVu_46A.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID,String ToDaXem)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String Thang_Quy = Convert.ToString(Request.Form[ParentID + "_Thang_Quy"]);
            String RutGon = Convert.ToString(Request.Form[ParentID + "_RutGon"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["RutGon"] = RutGon;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["ToSo"] = ToSo;
            ViewData["sLNS"] = sLNS;
            ViewData["TruongTien"] = TruongTien;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_NghiepVu_46A.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="RutGon"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            HamChung.Language();
            String DuongDan = "";
            if (RutGon == "on")
            {
                if (KhoGiay == "1")
                {
                    if (ToSo == "1") DuongDan = sFilePath_A3_1_RG;
                    else DuongDan = sFilePath_A3_2_RG;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePath_A4_1_RG;
                    else DuongDan = sFilePath_A4_2_RG;
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    if (ToSo == "1") DuongDan = sFilePath_A3_1;
                    else DuongDan = sFilePath_A3_2;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePath_A4_1;
                    else DuongDan = sFilePath_A4_2;
                }
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS, TruongTien);
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
        /// khoi tao bao cao
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="RutGon"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            FlexCelReport fr = new FlexCelReport();
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

            //lấy tên đơn vị
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, sLNS, TruongTien);
            String[] TenDV;
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }
            String DonVi = iID_MaDonVi;
            String[] arrDonVi = iID_MaDonVi.Split(',');
            dtDonVi.Dispose();
            String ThangQuy = "";
            ThangQuy = "Tháng  " + Thang_Quy + " năm " + iNamLamViec;
            String TT = "";
            if (TruongTien == "rTuChi")
            {
                TT = " PHẦN TỰ CHI";
            }
            else TT = " PHẦN HIỆN VẬT";
            //Luy ke
            if (RutGon != "on")
            {
                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {
                        if (arrDonVi.Length < 8)
                        {
                            int a1 = 8 - arrDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                DonVi += ",-1";
                            }
                        }
                        arrDonVi = DonVi.Split(',');
                        TenDV = new String[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                            {
                                TenDV[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                            }
                        }

                        for (int i = 1; i <= TenDV.Length; i++)
                        {
                            fr.SetValue("DonVi" + i, TenDV[i - 1]);
                        }
                    }
                    else
                    {
                        if (arrDonVi.Length < 8 + 11 * (Convert.ToInt16(ToSo) - 1))
                        {
                            int a1 = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        TenDV = new String[11];
                        int x = 1;
                        for (int i = 8 + 11 * ((Convert.ToInt16(ToSo) - 2)); i < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)); i++)
                        {
                            if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                            {
                                TenDV[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                                x++;
                            }
                        }

                        for (int i = 1; i <= TenDV.Length; i++)
                        {
                            fr.SetValue("DonVi" + i, TenDV[i - 1]);
                        }
                    }
                }
                //A4
                else
                {
                    if (ToSo == "1")
                    {
                        if (arrDonVi.Length < 4)
                        {
                            int a1 = 4 - arrDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                DonVi += ",-1";
                            }
                        }
                        arrDonVi = DonVi.Split(',');
                        TenDV = new String[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                            {
                                TenDV[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                            }
                        }

                        for (int i = 1; i <= TenDV.Length; i++)
                        {
                            fr.SetValue("DonVi" + i, TenDV[i - 1]);
                        }
                    }
                    else
                    {
                        if (arrDonVi.Length < 4 + 7 * (Convert.ToInt16(ToSo) - 1))
                        {
                            int a1 = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        TenDV = new String[7];
                        int x = 1;
                        for (int i = 4 + 7 * ((Convert.ToInt16(ToSo) - 2)); i < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)); i++)
                        {
                            if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                            {
                                TenDV[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                                x++;
                            }
                        }

                        for (int i = 1; i <= TenDV.Length; i++)
                        {
                            fr.SetValue("DonVi" + i, TenDV[i - 1]);
                        }
                    }
                }
            }
            //rut gon
            else
            {
                int SoCotTrang1 = 0;
                int SoCotTrang2 = 0;
                if (KhoGiay == "1")
                {
                    SoCotTrang1 = 10;
                    SoCotTrang2 = 11;
                }
                else
                {
                    SoCotTrang1 = 6;
                    SoCotTrang2 = 7;
                }
                if (ToSo == "1")
                {
                    if (arrDonVi.Length < SoCotTrang1)
                    {
                        int a1 = SoCotTrang1 - arrDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            DonVi += ",-1";
                        }
                    }
                    arrDonVi = DonVi.Split(',');
                    TenDV = new String[SoCotTrang1];
                    for (int i = 0; i < SoCotTrang1; i++)
                    {
                        if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                        {
                            TenDV[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                        }
                    }

                    for (int i = 1; i <= TenDV.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, TenDV[i - 1]);
                    }
                }
                else
                {
                    if (arrDonVi.Length < SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1))
                    {
                        int a1 = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    TenDV = new String[SoCotTrang2];
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 2)); i < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)); i++)
                    {
                        if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                        {
                            TenDV[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                            x++;
                        }
                    }

                    for (int i = 1; i <= TenDV.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, TenDV[i - 1]);
                    }
                }
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
            LoadData(fr, iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS, TruongTien);
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_NghiepVu_46A");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("ThangQuy", ThangQuy);
            fr.SetValue("Phong", Phong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("TruongTien", TT);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// Xuat excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="RutGon"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            HamChung.Language();
            String DuongDan = "";
            if (RutGon == "on")
            {
                if (KhoGiay == "1")
                {
                    if (ToSo == "1") DuongDan = sFilePath_A3_1_RG;
                    else DuongDan = sFilePath_A3_2_RG;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePath_A4_1_RG;
                    else DuongDan = sFilePath_A4_2_RG;
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    if (ToSo == "1") DuongDan = sFilePath_A3_1;
                    else DuongDan = sFilePath_A3_2;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePath_A4_1;
                    else DuongDan = sFilePath_A4_2;
                }
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS, TruongTien);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToanNghiepVu_40_2.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            DataTable data = QuyetToan_NghiepVu_40_2(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS, TruongTien);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            if (RutGon != "on")
            {
                DataTable data_LK = LuyKe(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS, TruongTien);
                if (data_LK.Rows.Count == 0)
                {
                    DataRow dr = data_LK.NewRow();
                    data_LK.Rows.InsertAt(dr, 0);
                }
                fr.AddTable("LuyKe", data_LK);
                data_LK.Dispose();
            }
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);
            if (dtLoaiNS.Rows.Count == 0)
            {
                DataRow dr = dtLoaiNS.NewRow();
                dtLoaiNS.Rows.InsertAt(dr, 0);
            }
            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtLoaiNS, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            if (dtNguonNS.Rows.Count == 0)
            {
                DataRow dr = dtNguonNS.NewRow();
                dtNguonNS.Rows.InsertAt(dr, 0);
            }
            fr.AddTable("NguonNS", dtNguonNS);

            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// QuyetToan_ThuongXuyen_34C
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="RutGon"></param>
        /// <returns></returns>
        public DataTable QuyetToan_NghiepVu_40_2(String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            DataTable dt = new DataTable();
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, sLNS, TruongTien);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String DKDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            if (!String.IsNullOrEmpty(DKDonVi))
            {
                DKDonVi = " AND (" + DKDonVi + ")";
            }
            dtDonVi.Dispose();
            String DKSUMDonVi = "", DKCASEDonVi = "", DKHAVINGDonVi = "";
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            //DKLNS
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            if (!String.IsNullOrEmpty(DKLNS))
                DKLNS = " AND (" + DKLNS + ")";


            String DKRutGon = "";
            DKRutGon = "iThang_Quy="+Thang_Quy;

            #region Mầu rút gọn
            if (RutGon == "on")
            {
                int SoCotTrang1 = 0;
                int SoCotTrang2 = 0;
                //Giay a3
                if (KhoGiay == "1")
                {
                    SoCotTrang1 = 10;
                    SoCotTrang2 = 11;
                }
                else
                {
                    SoCotTrang1 = 6;
                    SoCotTrang2 = 7;
                }
                if (ToSo == "1")
                {
                    if (arrDonVi.Length < SoCotTrang1)
                    {
                        int a = SoCotTrang1 - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = 1; i <= SoCotTrang1; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                        DKCASEDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {1}) THEN SUM({0}) ELSE 0 END";
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                }
                else
                {
                    if (arrDonVi.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                    {
                        // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                        DKCASEDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {1}) THEN SUM({0}) ELSE 0 END";
                        x++;
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                }
                String SQLRutGon = String.Format(@"SELECT NguonNS, sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy {1}
                                            FROM(
                                            SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END                                          
                                            {0}
                                            FROM QTA_ChungTuChiTiet
                                            WHERE  iTrangThai=1 {6} AND sNG<>''  {3} {4} {5}
                                            GROUP BY SUBSTRING(sLNS,1,1),sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                            GROUP BY NguonNS,sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa
                                            HAVING SUM(CongTrongKy)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, TruongTien, DKRutGon);
                SqlCommand cmd = new SqlCommand(SQLRutGon);
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmd.Parameters.AddWithValue("iID_MaDonVia" + i, arrDonVi[i]);
                }
                if (ToSo == "1")
                {
                    for (int i = 1; i <= SoCotTrang1; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion

            #region Mẫu đầy đủ
            else
            {
                //Giay a3
                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {

                        if (arrDonVi.Length < 8)
                        {
                            int a = 8 - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaDonVi += ",-1";
                            }
                            arrDonVi = iID_MaDonVi.Split(',');
                        }
                        for (int i = 1; i <= 8; i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                            DKCASEDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {1}) THEN SUM({0}) ELSE 0 END";
                        }
                        DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                    }
                    else
                    {
                        if (arrDonVi.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaDonVi += ",-1";
                            }
                            arrDonVi = iID_MaDonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                            DKCASEDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {1}) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                    }
                }
                //Giay A4
                else
                {
                    if (ToSo == "1")
                    {

                        if (arrDonVi.Length < 4)
                        {
                            int a = 4 - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaDonVi += ",-1";
                            }
                            arrDonVi = iID_MaDonVi.Split(',');
                        }
                        for (int i = 1; i <= 4; i++)
                        {

                            DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                            DKCASEDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {1}) THEN SUM({0}) ELSE 0 END";
                        }
                        DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                    }
                    else
                    {
                        if (arrDonVi.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaDonVi += ",-1";
                            }
                            arrDonVi = iID_MaDonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                            DKCASEDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {1}) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                    }
                }

                String SQLDayDu = String.Format(@"SELECT NguonNS,sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy,SUM(DenKyNay) as DenKyNay {1}
                                            FROM(
                                            SELECT SUBSTRING(sLNS,1,1) as NguonNS, sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END
                                            ,DenKyNay=CASE WHEN iThang_Quy<=@ThangQuy THEN SUM({7}) else 0 END
                                            {0}
                                            FROM QTA_ChungTuChiTiet
                                            WHERE  sNG<>'' {6}  AND iTrangThai=1  {3} {4} {5}
                                            GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                            GROUP BY NguonNS, sLNS, sL, sK, sM, sTM, sTTM, sNG,sTNG,sMoTa
                                            HAVING SUM(CongTrongKy)<>0 OR SUM(DenKyNay)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, TruongTien, DKRutGon);
                SqlCommand cmdDayDu = new SqlCommand(SQLDayDu);

                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                else
                {
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                cmdDayDu.Parameters.AddWithValue("@ThangQuy",Thang_Quy);
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmdDayDu.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
                }
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmdDayDu.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                dt = Connection.GetDataTable(cmdDayDu);
                cmdDayDu.Dispose();

                //Tao dt ChiTieu

                String DKDonViChiTieu = "";
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DKDonViChiTieu += "iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < dtDonVi.Rows.Count - 1)
                            DKDonViChiTieu += " OR ";
                    }
                }
                else
                {
                    DKDonViChiTieu = " iID_MaDonVi=@iID_MaDonVi";
                }
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iNamLamViec = DateTime.Now.Year.ToString();
                String iID_MaNguonNganSach = "1";
                String iID_MaNamNganSach = "2";
                String DKDuyet1 = "";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
                }
                else
                {
                    DKDuyet1 = "";
                }
                if (dtCauHinh.Rows.Count > 0)
                {
                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

                }
                dtCauHinh.Dispose();
                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS, sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa, SUM({3}) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                  WHERE PB_PhanBoChiTiet.iTrangThai=1
                                                AND sNG<>''{4}  AND YEAR(dNgayDotPhanBo)=@NamLamViec 
                                                AND MONTH(dNgayDotPhanBo)<= @dNgay  AND PB_PhanBoChiTiet.iTrangThai=1 AND ({0})
                                                 {1} {2}
                                                 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                                 HAVING SUM({3})<>0", DKDonViChiTieu, ReportModels.DieuKien_NganSachThuongXuyen(MaND), DKDuyet1, TruongTien, DKLNS);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@dNgay", Thang_Quy);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"]);
                    }
                }
                else
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", Guid.Empty.ToString());
                }
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmdChiTieu.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                //Ghep dtChiTieu vao dt

                #region  //Ghép DTChiTieu vào dt
                DataRow addR, R2;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,ChiTieu";
                String[] arrCol = sCol.Split(',');

                DataColumn col = dt.Columns.Add("ChiTieu", typeof(Decimal));
                col.SetOrdinal(8);

                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}' AND NguonNS='{7}' AND sTNG='{8}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"], dtChiTieu.Rows[i]["sTNG"]
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
                dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
                dt = dv.ToTable();
                #endregion
            }
            #endregion
            return dt;
        }

        /// <summary>
        /// Tính lũy kế
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="RutGon"></param>
        /// <returns></returns>
        public DataTable LuyKe(String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            DataTable dt = new DataTable();
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, sLNS, TruongTien);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');

            String DKSUMDonVi = "", DKCASEDonVi = "", DKHAVINGDonVi = "";
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            //DKLNS
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            if (!String.IsNullOrEmpty(DKLNS))
                DKLNS = " AND (" + DKLNS + ")";


            String DKRutGon = "";
            DKRutGon = "iThang_Quy<=" + Thang_Quy;

            //Giay a3
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {

                    if (arrDonVi.Length < 8)
                    {
                        int a = 8 - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                        DKCASEDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {1}) THEN SUM({0}) ELSE 0 END";
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                }
                else
                {
                    if (arrDonVi.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                    {
                        DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                        DKCASEDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {1}) THEN SUM({0}) ELSE 0 END";
                        x++;
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                }
            }
            //Giay A4
            else
            {
                if (ToSo == "1")
                {

                    if (arrDonVi.Length < 4)
                    {
                        int a = 4 - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                        DKCASEDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {1}) THEN SUM({0}) ELSE 0 END";
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                }
                else
                {
                    if (arrDonVi.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {

                        DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                        DKCASEDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {1}) THEN SUM({0}) ELSE 0 END";
                        x++;
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, TruongTien, DKRutGon);
                }
            }
            String DKDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            if (!String.IsNullOrEmpty(DKDonVi))
            {
                DKDonVi = " AND (" + DKDonVi + ")";
            }
            dtDonVi.Dispose();
            String SQLDayDu = String.Format(@"SELECT SUM(DenKyNay) as DenKyNay {1} 
                                            FROM(
                                            SELECT SUBSTRING(sLNS,1,1) as NguonNS, sLNS, sL, sK, sM, sTM, sTTM, sNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END
                                            ,DenKyNay=CASE WHEN iThang_Quy<=@ThangQuy THEN SUM({7}) else 0 END
                                            {0}
                                            FROM QTA_ChungTuChiTiet
                                            WHERE  iTrangThai=1 {6}  {3} {4} {5}
                                            GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                            --GROUP BY NguonNS, sLNS, sL, sK, sM, sTM, sTTM, sNG,sMoTa
                                            HAVING  SUM(DenKyNay)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, TruongTien, DKRutGon);
            SqlCommand cmdDayDu = new SqlCommand(SQLDayDu);

            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                    {
                        cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
            }
            else
            {
                if (ToSo == "1")
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {
                        cmdDayDu.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
            }
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmdDayDu.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
            }
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmdDayDu.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            cmdDayDu.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            dt = Connection.GetDataTable(cmdDayDu);
            cmdDayDu.Dispose();

            return dt;
        }
        /// <summary>
        /// danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="RutGon"></param>
        /// <returns></returns>
        public JsonResult DS_To(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {
            return Json(obj_To(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS, TruongTien), JsonRequestBehavior.AllowGet);
        }
        public String obj_To(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS, String TruongTien)
        {

            DataTable dtToSo = dtTo(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, sLNS, TruongTien);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            String s = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 50%\"");
            return s;
        }
        public static DataTable DanhSachDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String sLNS, String TruongTien)
        {
            String DKRutGon = "";

            if (RutGon == "on")
            {
                DKRutGon = "iThang_Quy=" + Thang_Quy;
            }
            else
            {
                DKRutGon = "iThang_Quy<="+ Thang_Quy;
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            if (!String.IsNullOrEmpty(DKLNS))
                DKLNS = " AND (" + DKLNS + ")";
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQl = String.Format(@"SELECT iID_MaDonVi
                                         FROM QTA_ChungTuChiTiet
                                         WHERE 1=1   {1} {2} AND {0}  AND iTrangThai=1 {3} AND iNamLamViec=@iNamLamViec
                                         GROUP BY iID_MaDonVi
                                         HAVING SUM({4})<>0
ORDER BY iID_MaDonVi ", DKRutGon, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKLNS, TruongTien);
            SqlCommand cmd = new SqlCommand(SQl);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable dtTo(String iID_MaTrangThaiDuyet, String Thang_Quy, String RutGon, String MaND, String KhoGiay, String sLNS, String TruongTien)
        {
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, sLNS, TruongTien);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }

            String[] arrDonVi = iID_MaDonVi.Split(',');
            dtDonVi.Dispose();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));
            DataRow dr = dt.NewRow();
            dr[0] = "Tờ 1";
            dr[1] = "1";
            dt.Rows.InsertAt(dr, 0);
            //Luy ke
            if (RutGon != "on")
            {
                //giay a3
                if (KhoGiay == "1")
                {
                    int a = 2;
                    for (int i = 8; i < arrDonVi.Length; i = i + 11)
                    {
                        DataRow dr1 = dt.NewRow();
                        dt.Rows.Add(dr1);
                        dr1[0] = "Tờ " + a;
                        dr1[1] = a;
                        a++;

                    }
                }
                else
                {
                    int a = 2;
                    for (int i = 4; i < arrDonVi.Length; i = i + 7)
                    {
                        DataRow dr1 = dt.NewRow();
                        dt.Rows.Add(dr1);
                        dr1[0] = "Tờ " + a;
                        dr1[1] = a;
                        a++;

                    }
                }
            }
            else
            {
                int SoCotTrang1, SoCotTrang2;
                //giay a3
                if (KhoGiay == "1")
                {
                    SoCotTrang1 = 10;
                    SoCotTrang2 = 11;
                }
                else
                {
                    SoCotTrang1 = 6;
                    SoCotTrang2 = 7;
                }
                int a = 2;
                for (int i = SoCotTrang1; i < arrDonVi.Length; i = i + SoCotTrang2)
                {
                    DataRow dr1 = dt.NewRow();
                    dt.Rows.Add(dr1);
                    dr1[0] = "Tờ " + a;
                    dr1[1] = a;
                    a++;

                }
            }
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
