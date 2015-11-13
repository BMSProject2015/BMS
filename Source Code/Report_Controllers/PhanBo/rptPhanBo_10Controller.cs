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
    public class rptPhanBo_10Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_10_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_10_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_10_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_10_A3_2.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_10.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, String ToDaXem)
        {
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String iID_MaDotPhanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaDotPhanBo"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
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
            if (!DaCo) {
                if (ToDaXem == "") ToDaXem = ToSo;
                else ToDaXem += "," + ToSo;
            }
            ViewData["ToDaXem"] = ToDaXem;
            ViewData["sLNS"] = sLNS;
            ViewData["TruongTien"] = TruongTien;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["ToSo"] = ToSo;
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_10.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm xem báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A3_1;
                }
                else
                    DuongDan = sFilePath_A3_2;
            }
            else
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A4_1;
                }
                else
                    DuongDan = sFilePath_A4_2;

            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
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
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A3_1;
                }
                else
                    DuongDan = sFilePath_A3_2;
            }
            else
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A4_1;
                }
                DuongDan = sFilePath_A4_2;

            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "TongHopChiTieuQuocPhong_9_A4.xls";
                }
                else
                {
                    clsResult.FileName = "TongHopChiTieuQuocPhong_9_A3.xls";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm khởi tạo Báo Cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            XlsFile Result = new XlsFile(true);
            FlexCelReport fr = new FlexCelReport();
            Result.Open(path);
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            String tendot = "";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    try
                    {
                        DateTime ngay = Convert.ToDateTime(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString());

                        tendot = " Tháng  " + ngay.ToString("MM") + "   Năm  " + ngay.ToString("yyyy");
                    }
                    catch { tendot = ""; }
                }
            }
            String Dot = String.Format("ĐỢT {0} : {1} ", ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo), tendot);
            DataTable dtPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String[] TenDotPhanBo;
            String str_MaDotPhanBo = "";
            if (dtPhanBo.Rows.Count > 0)
            {
                    for (int i = 0; i < dtPhanBo.Rows.Count; i++)
                    {
                        String DauPhay = ",";
                        if (i == 0) DauPhay = "";
                        str_MaDotPhanBo += DauPhay + dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                        if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString()) break;
                    }
            }
            String DotBoSung = str_MaDotPhanBo;
            String[] arrDotPhanBo = str_MaDotPhanBo.Split(',');
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 8)
                    {
                        int a = 8 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[8];
                    for (int i = 0; i < 8; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[i] = Convert.ToString(dtPhanBo.Rows[i]["dNgayDotPhanBo"]);
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotPhanBo" + i, "");
                        }
                        else
                            fr.SetValue("DotPhanBo" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
                else
                {
                    int a1 = 8 + 11 * (Convert.ToInt16(ToSo) - 1);
                    int a2 = 8 + 11 * (Convert.ToInt16(ToSo) - 2);
                    if (arrDotPhanBo.Length < a1)
                    {
                        int a = a1 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[11];
                    int x = 1;
                    for (int i = a2; i < a1; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[x - 1] = Convert.ToString(dtPhanBo.Rows[i]["dNgayDotPhanBo"]);
                            x++;
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotPhanBo" + i, "");
                        }
                        else
                            fr.SetValue("DotPhanBo" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
            }
            else
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 4)
                    {
                        int a = 4 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[4];
                    for (int i = 0; i < 4; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[i] = Convert.ToString(dtPhanBo.Rows[i]["dNgayDotPhanBo"]);
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotPhanBo" + i, "");
                        }
                        else
                            fr.SetValue("DotPhanBo" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
                else
                {
                    int a1 = 4 + 7 * (Convert.ToInt16(ToSo) - 1);
                    int a2 = 4 + 7 * (Convert.ToInt16(ToSo) - 2);
                    if (arrDotPhanBo.Length < a1)
                    {
                        int a = a1 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[7];
                    int x = 1;
                    for (int i = a2; i < a1; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[x - 1] = Convert.ToString(dtPhanBo.Rows[i]["dNgayDotPhanBo"]);
                            x++;
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotPhanBo" + i, "");
                        }
                        else
                            fr.SetValue("DotPhanBo" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
            }
            String TenTruongTien = "";
            switch (TruongTien)
            {
                case "rTuChi":
                    TenTruongTien = " PHẦN TỰ CHI";
                    break;
                case "rHienVat":
                    TenTruongTien = " PHẦN HIỆN VẬT";
                    break;
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            LoadData(fr, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_10");
                fr.SetValue("TruongTien", TenTruongTien);
                fr.SetValue("NgayThangNam", NgayThang);
                fr.SetValue("Dot", Dot);
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(3));
                fr.SetValue("TieuDe", "TỔNG HỢP CHỈ TIÊU NGÂN SÁCH QUỐC PHÒNG");
                fr.Run(Result);
                return Result;
            
                
        }

        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {

            DataTable data = rptPhanBo_10(MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,KhoGiay,ToSo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "sLNS", "sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        /// 
        public DataTable rptPhanBo_10(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien,String KhoGiay,String ToSo)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            String DK_Duyet = "", DK_Duyet_ChiTieu = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
                DK_Duyet_ChiTieu = "";
            }
            else
            {
                DK_Duyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
                DK_Duyet_ChiTieu = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeChiTieu) + "'";
            }
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String str_MaDotPhanBo = "";
            String DKSUMDOTPHANBO = "", DKCASE = "", DKHAVING = "", DotPhanBo_ChiTieu = "";
            if (dtDotPhanBo.Rows.Count > 0)
            {

                    for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        String DauPhay = ",";
                        if (i == 0) DauPhay = "";
                        str_MaDotPhanBo += DauPhay + dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                        if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString()) break;
                    }
 
            }

            //String DkDotPhanBo = "";

            String[] arrDotPhanBo = str_MaDotPhanBo.Split(',');
            //for (int i = 0; i < arrDotPhanBo.Length; i++)
            //{
            //    DkDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBoa" + i;
            //    if (i < arrDotPhanBo.Length - 1)
            //        DkDotPhanBo += " OR ";
            //    cmd.Parameters.AddWithValue("iID_MaDotPhanBoa" + i, arrDotPhanBo[i]);
            //}
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }

            //a3
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 8)
                    {
                        int a = 8 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        DotPhanBo_ChiTieu += ",DotPhanBo" + i + "=0";
                        DKSUMDOTPHANBO += ",SUM(DotPhanBo" + i + ") AS DotPhanBo" + i;
                        DKHAVING += " OR SUM(DotPhanBo" + i + ")<>0 ";
                        DKCASE += " ,DotPhanBo" + i + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                    }
                    DKCASE = string.Format(DKCASE, TruongTien);
                }
                else
                {
                    if (arrDotPhanBo.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                    {
                        DotPhanBo_ChiTieu += ",DotPhanBo" + x + "=0";
                        DKSUMDOTPHANBO += ",SUM(DotPhanBo" + x + ") AS DotPhanBo" + x;
                        DKHAVING += " OR SUM(DotPhanBo" + x + ")<>0 ";
                        DKCASE += " ,DotPhanBo" + x + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + (x-1), arrDotPhanBo[i]);
                    }
                    DKCASE = string.Format(DKCASE, TruongTien);
                }
            }
            //a 4
            else
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 4)
                    {
                        int a = 4 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        DotPhanBo_ChiTieu += ",DotPhanBo" + i + "=0";
                        DKSUMDOTPHANBO += ",SUM(DotPhanBo" + i + ") AS DotPhanBo" + i;
                        DKHAVING += " OR SUM(DotPhanBo" + i + ")<>0 ";
                        DKCASE += " ,DotPhanBo" + i + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                    }
                    DKCASE = string.Format(DKCASE, TruongTien);
                }
                else
                {
                    if (arrDotPhanBo.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {
                        DotPhanBo_ChiTieu += ",DotPhanBo" + x + "=0";
                        DKSUMDOTPHANBO += ",SUM(DotPhanBo" + x + ") AS DotPhanBo" + x;
                        DKHAVING += " OR SUM(DotPhanBo" + x + ")<>0 ";
                        DKCASE += " ,DotPhanBo" + x + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + (x -1), arrDotPhanBo[i]);
                    }
                    DKCASE = string.Format(DKCASE, TruongTien);
                }
            }
            String SQL = String.Format(@"
                                    SELECT 
                                    sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                    ,SUM(ChiTieu) as ChiTieu
                                    ,SUM(PhanBo) as PhanBo
                                    {0}
                                    FROM
                                    (
                                    SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                    ,ChiTieu=SUM({1})
                                    ,PhanBo=0
                                     {4}
                                     FROM PB_ChiTieuChiTiet
                                     WHERE iTrangThai=1  AND sNG<>'' {3} {9}
                                     AND ({2})
                                     GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                     HAVING SUM({1})!=0
 
                                     UNION
 
                                     SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                     ,ChiTieu=0
                                     ,PhanBo=SUM({1})
                                     {6}
                                     FROM PB_PhanBoChiTiet
                                    WHERE iTrangThai=1 {5} {7}
                                    AND ({2}) AND sNG<>''
                                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDotPhanBo
                                    HAVING SUM(rTuChi)!=0
                                    ) as tblMain
                                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                    HAVING SUM(ChiTieu)<>0 OR SUM(PhanBo)<>0 {8}", DKSUMDOTPHANBO, TruongTien, DKLNS, DK_Duyet_ChiTieu, DotPhanBo_ChiTieu, ReportModels.DieuKien_NganSach(MaND), DKCASE, DK_Duyet,DKHAVING,ReportModels.DieuKien_ChiTieu(MaND));
            cmd.CommandText=SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách đợt phân bổ theo năm làm việc
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public class LNSdata
        {
            public string iID_MaDotPhanBo { get; set; }
            public string ToSo { get; set; }
        }

        public JsonResult ds_DotPhanBo(String ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien,String KhoGiay,String ToSo)
        {
            return Json(obj_DotPhanBo(ParentID, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien,KhoGiay,ToSo), JsonRequestBehavior.AllowGet);
        }

        public LNSdata obj_DotPhanBo(String ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien,String KhoGiay,String ToSo)
        {
            LNSdata _LNSdata = new LNSdata();

            #region Danh sách đơn vị
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            SelectOptionList slDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _LNSdata.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\"onchange=\"ChonNLV()\"");
            #endregion
            #region Option Tờ số
            DataTable dtToSo = dtTo(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _LNSdata.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100%\"");
            dtToSo.Dispose();
            #endregion

            return _LNSdata;
        }
        /// <summary>
        /// Lấy danh sách đợt phân bổ
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>

        public static DataTable DanhSach_DotPhanBo(String MaND, String sLNS, String iID_MaTrangThaiDuyet, String TruongTien)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(TruongTien))
            {
                TruongTien = "rTuChi";
            }
            String DK = "";
            String[] ArrLNS = sLNS.Split(',');

            for (int i = 0; i < ArrLNS.Length; i++)
            {
                DK += "sLNS=@sLNS" + i;
                if (i < ArrLNS.Length - 1)
                    DK += " OR ";
            }
            if (String.IsNullOrEmpty(DK) == false)
                DK = " (" + DK + ")";

            String DK_Duyet = " AND PBCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            
            String SQL = String.Format(@"SELECT PBCT.iID_MaDotPhanBo,CONVERT(varchar,dNgayDotPhanBo,103) AS dNgayDotPhanBo
                                        FROM PB_PhanBoChiTiet AS PBCT
                                        INNER JOIN PB_PhanBo AS PB ON PBCT.iID_MaDotPhanBo=PB.iID_MaDotPhanBo
                                        WHERE PBCT.iTrangThai=1 AND ({0}) AND PBCT.{1}>0 {2} {3} 
                                        GROUP BY PBCT.iID_MaDotPhanBo,dNgayDotPhanBo
                                        ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)", DK, TruongTien, DK_Duyet, ReportModels.DieuKien_NganSach(MaND,"PBCT"));
            cmd.CommandText = SQL;
            for (int i = 0; i < ArrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, ArrLNS[i]);
            }
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotPhanBo"]=Guid.Empty.ToString();
                dr["dNgayDotPhanBo"] = "Không có dữ liệu";
                dt.Rows.InsertAt(dr, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtTo(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay)
        {
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String strDotPhanBo = "";
            if (dtDotPhanBo.Rows.Count > 0)
            {
                for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                {
                    String DauPhay = ",";
                    if (i == 0) DauPhay = "";
                    strDotPhanBo += DauPhay + dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString()) break;

                }
            }
            String[] arrDotPhanBo = strDotPhanBo.Split(',');
            dtDotPhanBo.Dispose();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));
            DataRow dr = dt.NewRow();
            dr[0] = "Tờ 1";
            dr[1] = "1";
            dt.Rows.InsertAt(dr, 0);
            //Luy ke
            if (KhoGiay == "1")
            {
                int a = 2;
                for (int i = 8; i < arrDotPhanBo.Length; i = i + 11)
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
                for (int i = 4; i < arrDotPhanBo.Length; i = i + 7)
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

