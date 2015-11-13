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

namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_Chi_73_3Controller : Controller
    {
        //
        // GET: /rptBH_Chi_73_3/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A3_2.xls";

        private const String sFilePath_A4_1_RG = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A4_1_RG.xls";
        private const String sFilePath_A4_2_RG = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A4_2_RG.xls";
        private const String sFilePath_A3_1_RG = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A3_1_RG.xls";
        private const String sFilePath_A3_2_RG = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_73_3_A3_2_RG.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_73_3.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trịn trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, String ToDaXem)
        {
            // String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String ThangQuy = "";
            String LoaiThangQuy = Convert.ToString(Request.Form[ParentID + "_LoaiThangQuy"]);
            if (LoaiThangQuy == "1")
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
            String RutGon = Convert.ToString(Request.Form[ParentID + "_RutGon"]);
            if (String.IsNullOrEmpty(ToDaXem)) ToDaXem = "";
            String[] arrToDaXem = ToDaXem.Split(',');
            bool DaCo = false;
            for (int i = 0; i < arrToDaXem.Length; i++)
            {
                if (arrToDaXem[i] == ToSo)
                {
                    DaCo = true;
                }
            }
            if (!DaCo)
            {
                if (ToDaXem == "") ToDaXem = ToSo;
                else ToDaXem += "," + ToSo;
            }
            ViewData["ToDaXem"] = ToDaXem;
            ViewData["ThangQuy"] = ThangQuy;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["TruongTien"] = TruongTien;
            ViewData["ToSo"] = ToSo;
            ViewData["RutGon"] = RutGon;
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_73_3.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
            // return RedirectToAction("Index", new {  ThangQuy = ThangQuy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo, String RutGon)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            String iNamLamViec = ReportModels.LayNamLamViec(MaND);
            String iID_MaDonVi = DanhSach_DonVi(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet);

            String[] TenDV;
            String DonVi = iID_MaDonVi;
            String[] arrDonVi = iID_MaDonVi.Split(',');

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
            String Thang_Quy = "";
            if (LoaiThangQuy == "0")
            {
                Thang_Quy = " Tháng " + ThangQuy + " Năm " + iNamLamViec;
            }
            else
            {
                Thang_Quy = " Quy " + ThangQuy + " Năm " + iNamLamViec;
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            LoadData(fr, MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo, RutGon);
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_Chi_73_3");
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangQuy", Thang_Quy);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("BLT", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;
        }


        /// <summary>
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo, String RutGon)
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo, RutGon);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToanThuongXuyen_34C.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm xem báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo, String RutGon)
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo, RutGon);
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

        private void LoadData(FlexCelReport fr, String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo, String RutGon)
        {

            DataTable data = BH_Chi_73_3(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo, RutGon);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            if (RutGon != "on")
            {
                DataTable data_LK = dtDenKyNay(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo, RutGon);
                if (data_LK.Rows.Count == 0)
                {
                    DataRow dr = data_LK.NewRow();
                    data_LK.Rows.InsertAt(dr, 0);
                }
                fr.AddTable("LuyKe", data_LK);
                data_LK.Dispose();
            }
            DataTable dtNG;
            dtNG = HamChung.SelectDistinct("NG", data, "sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG");
            fr.AddTable("NG", dtNG);

            DataTable dtTTMuc;
            dtTTMuc = HamChung.SelectDistinct("TietMuc", dtNG, "sLNS,sL,sK,sM,sTM,sTTM", "sLNS,sL,sK,sM,sTM,sTTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM,sNG");
            fr.AddTable("TietMuc", dtTTMuc);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", dtTTMuc, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            if (dtTieuMuc.Rows.Count == 0)
            {
                DataRow dr = dtTieuMuc.NewRow();
                dtTieuMuc.Rows.InsertAt(dr, 0);
            }
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }

        /// <summary>
        /// DataTable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public DataTable BH_Chi_73_3(String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo, String RutGon)
        {

            DataTable dt = new DataTable();
            String TrangThai_PB = "", TrangThai_BH = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                TrangThai_PB = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
                TrangThai_BH = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                TrangThai_PB = "";
                TrangThai_BH = " ";
            }
            String DK_Thang = "", DK_ThangDenKi = "", Thang_ChiTieu = ""; ;
            if (LoaiThangQuy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DK_Thang = "iThang_Quy IN (1,2,3)";
                        DK_ThangDenKi = "iThang_Quy<=3";
                        Thang_ChiTieu = "Thang<=3";
                        break;
                    case "2": DK_Thang = "iThang_Quy IN (4,5,6)";
                        DK_ThangDenKi = "iThang_Quy<=6";
                        Thang_ChiTieu = "Thang<=6";
                        break;
                    case "3": DK_Thang = "iThang_Quy IN (7,8,9)";
                        DK_ThangDenKi = "iThang_Quy<=9";
                        Thang_ChiTieu = "Thang<=9";
                        break;
                    case "4": DK_Thang = "iThang_Quy IN (10,11,12)";
                        DK_ThangDenKi = "iThang_Quy<=12";
                        Thang_ChiTieu = "Thang<=12";
                        break;
                    default:
                        DK_Thang = "iThang_Quy IN (-1)";
                        DK_ThangDenKi = "iThang_Quy<=-1";
                        Thang_ChiTieu = "Thang<=-1";
                        break;
                }
            }
            else
            {
                DK_Thang = "iThang_Quy=@ThangQuy";
                DK_ThangDenKi = "iThang_Quy<=@ThangQuy";
                Thang_ChiTieu = "Thang<=@ThangQuy";
            }
            String DonVi = DanhSach_DonVi(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet);
            String[] arrDonVi = DonVi.Split(',');
            String DKCASEDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKCASEDonVi += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DKCASEDonVi += " OR ";
            }
            //if (!String.IsNullOrEmpty(DKCASEDonVi))
            //{
            //    DKCASEDonVi = " AND (" + DKCASEDonVi + ")";
            //}

            String DKDDonVi = "";
            String DKSUMDonVi = ""; String DKHANGVINGDonVi = "";

            #region Mẫu rút gọn
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
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    for (int i = 1; i <= SoCotTrang1; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHANGVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                        DKDDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                    }
                    DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                }
                else
                {
                    if (arrDonVi.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                    {
                        // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHANGVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                        DKDDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                        x++;
                    }
                    DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                }
                String SQL_RutGon = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        ,SUM(TongCong) as TongCong
                                        {1}
                                        FROM
                                        (
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        ,TongCong=CASE WHEN {6} THEN SUM(rTongSo) ELSE 0 END
                                        {0}
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE bChi=1 AND sLNS='2200000'  {4} {3} AND ({5}) AND sTNG<>''
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iThang_Quy,iID_MaDonVi
                                        ) as BANGTAM
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        HAVING SUM(TongCong)!=0  {2} ", DKDDonVi, DKSUMDonVi, DKHANGVINGDonVi, TrangThai_BH, ReportModels.DieuKien_NganSach(MaND), DKCASEDonVi, DK_Thang);
                SqlCommand cmd_RutGon = new SqlCommand(SQL_RutGon);
                if (LoaiThangQuy == "0")
                {
                    cmd_RutGon.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmd_RutGon.Parameters.AddWithValue("iID_MaDonVia" + i, arrDonVi[i]);
                }
                if (ToSo == "1")
                {
                    for (int i = 1; i <= SoCotTrang1; i++)
                    {
                        cmd_RutGon.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                    {
                        cmd_RutGon.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
                dt = Connection.GetDataTable(cmd_RutGon);
                cmd_RutGon.Dispose();
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
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        for (int i = 1; i <= 8; i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHANGVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                            DKDDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                        }
                        DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                    }
                    else
                    {
                        if (arrDonVi.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHANGVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                            DKDDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                            x++;
                        }
                        DKDDonVi = string.Format(DKDDonVi, DK_Thang);
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
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        for (int i = 1; i <= 4; i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHANGVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                            DKDDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                        }
                        DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                    }
                    else
                    {
                        if (arrDonVi.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHANGVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                            DKDDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                            x++;
                        }
                        DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                    }
                }

                String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy
                                        ,SUM(DenKyNay) as DenKyNay
                                        {1}
                                        FROM
                                        (
                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        ,CongTrongKy=CASE WHEN {6} THEN SUM(rTongSo) ELSE 0 END
                                        ,DenKyNay=CASE WHEN {7} THEN SUM(rTongSo) ELSE 0 END
                                        {0}
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE bChi=1 AND sLNS='2200000'  {4} {3} AND ({5}) AND sTNG<>''
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iThang_Quy,iID_MaDonVi
                                        ) as BANGTAM
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        HAVING SUM(DenKyNay)!=0 OR SUM(CongTrongKy)!=0  {2} ", DKDDonVi, DKSUMDonVi, DKHANGVINGDonVi, TrangThai_BH, ReportModels.DieuKien_NganSach(MaND), DKCASEDonVi, DK_Thang, DK_ThangDenKi);
                SqlCommand cmdDayDu = new SqlCommand(SQL);

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
                if (LoaiThangQuy == "0")
                {
                    cmdDayDu.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmdDayDu.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
                }

                dt = Connection.GetDataTable(cmdDayDu);
                cmdDayDu.Dispose();
                DataTable dtChiTieu = new DataTable();
                String DKThang = "";
                if (LoaiThangQuy == "1")
                {
                    switch (ThangQuy)
                    {
                        case "1": DKThang = "MONTH(dNgayDotPhanBo) <= 3";
                            break;
                        case "2": DKThang = "MONTH(dNgayDotPhanBo) <= 6";
                            break;
                        case "3": DKThang = "MONTH(dNgayDotPhanBo) <= 9";
                            break;
                        case "4": DKThang = "MONTH(dNgayDotPhanBo) <= 12";
                            break;
                    }
                }
                else
                {
                    DKThang = "MONTH(dNgayDotPhanBo) <=@ThangQuy";
                }
                String SQLChiTieu = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,ChiTieu
                                                        FROM(
                                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iID_MaDotPhanBo,sum(rTuChi) as ChiTieu
                                                        FROM PB_PhanBoChiTiet
                                                        WHERE iTrangThai = 1  AND  ({0}) {1}
                                                        {1} AND sLNS = 2200000 
                                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iID_MaDotPhanBo
                                                        HAVING SUM(rTuChi)<>0) as a
                                                        INNER JOIN (SELECT iID_MaDotPhanBo,dNgayDotPhanBo FROM PB_DotPhanBo WHERE {3}) as b
                                                       ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo 
                                                    ", DKCASEDonVi, ReportModels.DieuKien_NganSach(MaND), TrangThai_PB, DKThang);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                if (LoaiThangQuy == "0")
                {
                    cmdChiTieu.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
                }
                dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                #region ghep dt vao dt chi tieu
                DataRow _addR;
                String _sCol = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,ChiTieu,sMoTa";
                String[] _arrCol = _sCol.Split(',');

                dt.Columns.Add("ChiTieu", typeof(Decimal));
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@" sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                    AND sTTM='{5}' AND sNG='{6}' AND sTNG='{7}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["sTNG"]
                                                      );
                    DataRow[] R = dt.Select(xauTruyVan);
                    DataRow R2;
                    if (R == null || R.Length == 0)
                    {
                        _addR = dt.NewRow();
                        for (int j = 0; j < _arrCol.Length; j++)
                        {
                            _addR[_arrCol[j]] = dtChiTieu.Rows[i][_arrCol[j]];
                        }
                        dt.Rows.Add(_addR);
                    }
                    else
                    {
                        foreach (DataRow R1 in dtChiTieu.Rows)
                        {

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                Boolean okTrung = true;
                                R2 = dt.Rows[j];

                                for (int c = 0; c < _arrCol.Length - 2; c++)
                                {
                                    if (R2[_arrCol[c]].Equals(R1[_arrCol[c]]) == false)
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
                DataView dv = dt.DefaultView;
                dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
                dt = dv.ToTable();

                #endregion
            }
            #endregion
            return dt;
        }

        public DataTable dtDenKyNay(String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo, String RutGon)
        {
            DataTable dtDenky = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DK_Thang = "", DK_ThangDenKi = "";
            if (LoaiThangQuy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DK_Thang = "iThang_Quy IN (1,2,3)";
                        DK_ThangDenKi = "iThang_Quy<=3";

                        break;
                    case "2": DK_Thang = "iThang_Quy IN (1,2,3,4,5,6)";
                        DK_ThangDenKi = "iThang_Quy<=6";

                        break;
                    case "3": DK_Thang = "iThang_Quy IN (1,2,3,4,5,6,7,8,9)";
                        DK_ThangDenKi = "iThang_Quy<=9";

                        break;
                    case "4": DK_Thang = "iThang_Quy IN (1,2,3,4,5,6,7,8,9,10,11,12)";
                        DK_ThangDenKi = "iThang_Quy<=12";

                        break;
                    default:
                        DK_Thang = "iThang_Quy IN (-1)";
                        DK_ThangDenKi = "iThang_Quy<=-1";

                        break;

                }
            }
            else
            {
                DK_Thang = "iThang_Quy<=@ThangQuy";
                DK_ThangDenKi = "iThang_Quy<=@ThangQuy";
            }
            String DonVi = DanhSach_DonVi(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet);
            String[] arrDonVi = DonVi.Split(',');
            String DKCASEDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKCASEDonVi += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DKCASEDonVi += " OR ";
            }
            String DKDDonVi = "";
            String DKSUMDonVi = ""; String DKHANGVINGDonVi = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {

                    if (arrDonVi.Length < 8)
                    {
                        int a = 8 - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHANGVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                        DKDDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                    }
                    DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                }
                else
                {
                    if (arrDonVi.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                    {
                        DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHANGVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                        DKDDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                        x++;
                    }
                    DKDDonVi = string.Format(DKDDonVi, DK_Thang);
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
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHANGVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                        DKDDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                    }
                    DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                }
                else
                {
                    if (arrDonVi.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHANGVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                        DKDDonVi += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND {0}) THEN SUM(rTongSo) ELSE 0 END";
                        x++;
                    }
                    DKDDonVi = string.Format(DKDDonVi, DK_Thang);
                }
            }

            String SQLDayDu = String.Format(@"SELECT SUM(CongTrongKy) as CongTrongKy
                                        ,SUM(DenKyNay) as DenKyNay
                                        {1}
                                        FROM
                                        (
                                        SELECT CongTrongKy=0
                                        ,DenKyNay=CASE WHEN {6} THEN SUM(rTongSo) ELSE 0 END
                                        {0}
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE bChi=1 AND sLNS='2200000'  {4} {3} AND sTNG<>'' AND ({5})
                                        GROUP BY iThang_Quy,iID_MaDonVi
                                        ) as BANGTAM
                                        HAVING SUM(DenKyNay)!=0 OR SUM(CongTrongKy)!=0  {2} ", DKDDonVi, DKSUMDonVi, DKHANGVINGDonVi, iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKCASEDonVi, DK_ThangDenKi);
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
            if (LoaiThangQuy == "0")
            {
                cmdDayDu.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmdDayDu.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
            }

            dtDenky = Connection.GetDataTable(cmdDayDu);
            cmdDayDu.Dispose();
            return dtDenky;
        }

        /// <summary>
        /// Hàm lấy danh sách đợt phân bổ
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public static String DanhSach_DonVi(String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DK_Thang = "";
            if (LoaiThangQuy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DK_Thang = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_Thang = "iThang_Quy IN (1,2,3,4,5,6)";
                        break;
                    case "3": DK_Thang = "iThang_Quy IN (1,2,3,4,5,6,7,8,9)";
                        break;
                    case "4": DK_Thang = "iThang_Quy IN (1,2,3,4,5,6,7,8,9,10,11,12)";
                        break;
                    default: DK_Thang = "iThang_Quy IN (-1)";
                        break;
                }
            }
            else
            {
                DK_Thang = "iThang_Quy<=@ThangQuy";
            }
            String SQL = String.Format(@" SELECT DISTINCT iID_MaDonVi,sTenDonVi
                                                     FROM BH_ChungTuChiChiTiet
                                                     WHERE bChi=1 AND sLNS=2200000 
                                                     AND {2} AND rTongSo>0 {1}  {0}", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DK_Thang);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThangQuy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String DonVi = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DonVi += dt.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (DonVi.Length > 1)
                DonVi = DonVi.Substring(0, DonVi.Length - 1);
            if (String.IsNullOrEmpty(DonVi))
            {
                DonVi = Guid.Empty.ToString();
            }
            return DonVi;
        }
        public static DataTable DanhSachTo(String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon)
        {
            String DonVi = DanhSach_DonVi(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet);
            String[] arrDonVi = DonVi.Split(',');
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
        public class data
        {

            public String ToSo { get; set; }
        }
        [HttpGet]
        public JsonResult ds_To(String ParentID, String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String ToSo)
        {
            return Json(obj_DanhSachTo(ParentID, MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, KhoGiay, RutGon, ToSo), JsonRequestBehavior.AllowGet);
        }
        public data obj_DanhSachTo(String ParentID, String MaND, String ThangQuy, String LoaiThangQuy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String ToSo)
        {
            data _data = new data();
            DataTable dtToSo = DanhSachTo(MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, KhoGiay, RutGon);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _data.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100%\"");
            return _data;
        }

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
