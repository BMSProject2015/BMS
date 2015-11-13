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
    public class rptPhanBo_11_11Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathA4_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_11_11_A4_1.xls";
        private const String sFilePathA4_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_11_11_A4_2.xls";
        private const String sFilePathA3_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_11_11_A3_1.xls";
        private const String sFilePathA3_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_11_11_A3_2.xls";
        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_11_11.aspx";
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
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String iID_MaDotPhanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaDotPhanBo"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["sLNS"] = sLNS;
            ViewData["TruongTien"] = TruongTien;
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["ToSo"] = ToSo;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_11_11.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1") DuongDan = sFilePathA3_1;
                else DuongDan = sFilePathA3_2;
            }
            else
            {
                if (ToSo == "1") DuongDan = sFilePathA4_1;
                else DuongDan = sFilePathA4_2;
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, TruongTien, KhoGiay, ToSo, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                    if (ToSo == "1") DuongDan = sFilePathA3_1;
                    else DuongDan = sFilePathA3_2;
            }
            else
            {
                    if (ToSo == "1") DuongDan = sFilePathA4_1;
                    else DuongDan = sFilePathA4_2;
            }
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan),MaND, sLNS, iID_MaDotPhanBo, TruongTien, KhoGiay, ToSo, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "PhanBo_11_11.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ExcelFile CreateReport(String path,String MaND, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenTruongTien = "";
            switch (TruongTien)
            {
                case "rTuChi": TenTruongTien = "TỰ CHI";
                    break;
                case "rHienVat": TenTruongTien = "HIỆN VẬT";
                    break;

            }
            String Dot = "";
            DataTable dtDot = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
            String NgayDot = "";
            for (int i = 1; i < dtDot.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDot.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    NgayDot = dtDot.Rows[i]["dNgayDotPhanBo"].ToString();
                    break;
                }
            }
            dtDot.Dispose();
            if (!String.IsNullOrEmpty(NgayDot))
            {
                Dot = ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo).ToString() + " Tháng " + NgayDot.Substring(3, 2) + " năm " + NgayDot.Substring(6, 4);
            }
                Dot = " Đến đợt " + Dot;
            DataTable dtDotPhanBo = ReportModels.GET_dtDotPhanBo(iID_MaDotPhanBo);
            String sDonVi = DanhSach_DonVi(MaND, iID_MaDotPhanBo, TruongTien, sLNS, iID_MaTrangThaiDuyet);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(2);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(3);

            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, sLNS, iID_MaDotPhanBo, TruongTien, KhoGiay, ToSo, iID_MaTrangThaiDuyet);
            String DonVi = DanhSach_DonVi(MaND, iID_MaDotPhanBo, TruongTien, sLNS, iID_MaTrangThaiDuyet);
            String[] arrDonVi1 = DonVi.Split(',');
            String[] arrTenDonVi;
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDonVi1.Length < 8)
                    {
                        int a = 8 - arrDonVi1.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                    }
                    arrDonVi1 = DonVi.Split(',');
                    arrTenDonVi = new String[8];
                    for (int i = 0; i < 8; i++)
                    {
                        if (arrDonVi1[i] != null && arrDonVi1[i] != "-1" && arrDonVi1[i] != Guid.Empty.ToString())
                        {
                            arrTenDonVi[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi1[i], "sTen").ToString();
                        }
                    }

                    for (int i = 1; i <= arrTenDonVi.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, arrTenDonVi[i - 1]);
                    }
                }
                else
                {
                    if (arrDonVi1.Length < 8 + 11 * (Convert.ToInt16(ToSo) - 1))
                    {
                        int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi1.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi1 = DonVi.Split(',');
                    }
                    arrTenDonVi = new String[11];
                    int x = 1;
                    for (int i = 8 + 11 * ((Convert.ToInt16(ToSo) - 2)); i < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)); i++)
                    {
                        if (arrDonVi1[i] != null && arrDonVi1[i] != "-1" && arrDonVi1[i] != Guid.Empty.ToString())
                        {
                            arrTenDonVi[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi1[i], "sTen").ToString();
                            x++;
                        }
                    }

                    for (int i = 1; i <= arrTenDonVi.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, arrTenDonVi[i - 1]);
                    }
                }
            }
            //A4
            else
            {
                if (ToSo == "1")
                {
                    if (arrDonVi1.Length < 4)
                    {
                        int a = 4 - arrDonVi1.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                    }
                    arrDonVi1 = DonVi.Split(',');
                    arrTenDonVi = new String[4];
                    for (int i = 0; i < 4; i++)
                    {
                        if (arrDonVi1[i] != null && arrDonVi1[i] != "-1" && arrDonVi1[i] != Guid.Empty.ToString())
                        {
                            arrTenDonVi[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi1[i], "sTen").ToString();
                        }
                    }

                    for (int i = 1; i <= arrTenDonVi.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, arrTenDonVi[i - 1]);
                    }
                }
                else
                {
                    if (arrDonVi1.Length < 4 + 7 * (Convert.ToInt16(ToSo) - 1))
                    {
                        int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi1.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi1 = DonVi.Split(',');
                    }
                    arrTenDonVi = new String[7];
                    int x = 1;
                    for (int i = 4 + 7 * ((Convert.ToInt16(ToSo) - 2)); i < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)); i++)
                    {
                        if (arrDonVi1[i] != null && arrDonVi1[i] != "-1" && arrDonVi1[i] != Guid.Empty.ToString())
                        {
                            arrTenDonVi[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi1[i], "sTen").ToString();
                            x++;
                        }
                    }

                    for (int i = 1; i <= arrTenDonVi.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, arrTenDonVi[i - 1]);
                    }
                }
            }
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_11_11");
            fr.SetValue("TruongTien", TenTruongTien);
            fr.SetValue("Dot", Dot);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("TruongTien", TenTruongTien);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.Run(Result);
            return Result;


        }
        public class data
        {
            public String iID_MaDotPhanBo { get; set; }
            public String ToSo { get; set; }
        }
        [HttpGet]
        public JsonResult ds_DotPhanBo(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay, String ToSo)
        {
            return Json(obj_DanhSachDotPhatBo(ParentID, MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDotPhanBo, TruongTien, KhoGiay, ToSo), JsonRequestBehavior.AllowGet);
        }
        public data obj_DanhSachDotPhatBo(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo,String TruongTien,String KhoGiay,String ToSo)
        {
            data _data = new data();
            DataTable dtDotPhatBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
            SelectOptionList slDotPhatBo = new SelectOptionList(dtDotPhatBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _data.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slDotPhatBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonDPB()\"");
            DataTable dtToSo = DanhSachTo(MaND, iID_MaTrangThaiDuyet,sLNS,iID_MaDotPhanBo,TruongTien,KhoGiay);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _data.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100%\"");
            return _data;
        }
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {

            DataTable data = PhanBo_11_11(MaND, sLNS, iID_MaDotPhanBo, TruongTien, KhoGiay, ToSo, iID_MaTrangThaiDuyet);
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
        public DataTable PhanBo_11_11(String MaND, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay, String ToSo,String iID_MaTrangThaiDuyet)
        {
            String DkDuyet_CT = "";
              String DkDuyet_PB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet_CT = "";
                DkDuyet_PB="";
            }
            else
            {
                DkDuyet_CT = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_CT ";
                 DkDuyet_PB = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB ";
            }
            DataTable dt = new DataTable();
            String DonVi = DanhSach_DonVi(MaND, iID_MaDotPhanBo, TruongTien, sLNS, iID_MaTrangThaiDuyet);
            String[] arrDonVi = DonVi.Split(',');
            String[] arrTenDonVi;
            String DKSELECT = "", DKCASE = "", DKHAVING = "",DKDONVI_ChiTieu = "";
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            //Danh sách đợt phân bổ
            DataTable dtPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet,sLNS);
            String DKDotPhanBo = "iID_MaDotPhanBo='"+Guid.Empty.ToString()+"'";

            for (int i = 1; i < dtPhanBo.Rows.Count; i++)
            {
                DKDotPhanBo = "";
                if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    for (int j = 1; j <= i; j++)
                    {
                        DKDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                        if (j < i)
                            DKDotPhanBo += " OR ";
                    }
                    break;
                }

            }

            #region Khổ giấy A3
            if (KhoGiay == "1")
            {
                if (ToSo == "1")//8 don vi
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
                        DKDONVI_ChiTieu += ",DONVI" + i + "=0";
                        DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                        DKCASE += ",DONVI" + i + "=SUM(CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + ") THEN {0} ELSE 0 END)";
                        DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                    }
                }
                else // 7 don vi
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
                        DKDONVI_ChiTieu += ",DONVI" + x + "=0";
                        DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                        DKCASE += ",DONVI" + x + "=SUM(CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + ") THEN {0} ELSE 0 END)";
                        DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                        x++;
                    }
                }
                DKCASE = String.Format(DKCASE, TruongTien);

                String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        SUM(ChiTieu) as ChiTieu,
                                        SUM(PhanBo) as PhanBo 
                                        {0}
                                        FROM 
                                        --ChiTieu--
                                        (SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        ChiTieu=SUM({2}),
                                        PhanBo=0
                                        {1}
                                        FROM PB_ChiTieuChiTiet
                                        WHERE iTrangThai=1 AND sNG<>'' {10} {6}  AND ({8}) AND ({9})
                                        GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM({2})<>0
                                        UNION

                                        --PhanBo--
                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        ChiTieu=0,
                                        PhanBo=SUM({2})
                                        {3}
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1 AND sNG<>'' {5} {7} AND ({8}) AND ({9})
                                        GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM({2})<>0 
                                        ) as  a
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(ChiTieu)<>0 OR SUM(PhanBo)<>0 {4}", DKSELECT, DKDONVI_ChiTieu, TruongTien, DKCASE, DKHAVING, ReportModels.DieuKien_NganSach(MaND), DkDuyet_CT, DkDuyet_PB, DKDotPhanBo, DKLNS,ReportModels.DieuKien_ChiTieu(MaND));
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_CT", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeChiTieu));
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
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
                            cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                if (dtPhanBo.Rows.Count > 1)
                {
                    for (int i = 1; i < dtPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 1; j <= i; j++)
                            {
                                cmd.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                }
                dt = Connection.GetDataTable(cmd);
            }
            #endregion
            else
            {
                #region Khổ giấy A4
                if (ToSo == "1")//4 don vi
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
                        DKDONVI_ChiTieu += ",DONVI" + i + "=0";
                        DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                        DKCASE += ",DONVI" + i + "=SUM(CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + ") THEN {0} ELSE 0 END)";
                        DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                    }
                }
                else // 7 don vi
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
                        DKDONVI_ChiTieu += ",DONVI" + x + "=0";
                        DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                        DKCASE += ",DONVI" + x + "=SUM(CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + ") THEN {0} ELSE 0 END)";
                        DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                        x++;
                    }
                }
                DKCASE = String.Format(DKCASE, TruongTien);

                String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        SUM(ChiTieu) as ChiTieu,
                                        SUM(PhanBo) as PhanBo 
                                        {0}
                                        FROM 
                                        --ChiTieu--
                                        (SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        ChiTieu=SUM({2}),
                                        PhanBo=0
                                        {1}
                                        FROM PB_ChiTieuChiTiet
                                        WHERE iTrangThai=1 AND sNG<>'' {10} {6}  AND ({8}) AND ({9})
                                        GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM({2})<>0
                                        UNION

                                        --PhanBo--
                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        ChiTieu=0,
                                        PhanBo=SUM({2})
                                        {3}
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1 AND sNG<>'' {5} {7} AND ({8}) AND ({9})
                                        GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM({2})<>0 
                                        ) as  a
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(ChiTieu)<>0 OR SUM(PhanBo)<>0 {4}", DKSELECT, DKDONVI_ChiTieu, TruongTien, DKCASE, DKHAVING, ReportModels.DieuKien_NganSach(MaND), DkDuyet_CT, DkDuyet_PB, DKDotPhanBo, DKLNS,ReportModels.DieuKien_ChiTieu(MaND));
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_CT", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeChiTieu));
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                if (ToSo == "1")
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
                if (dtPhanBo.Rows.Count > 1)
                {
                    for (int i = 1; i < dtPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 1; j <= i; j++)
                            {
                                cmd.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                }
                dt = Connection.GetDataTable(cmd);
                #endregion
            }
            return dt;
        }
        public static String DanhSach_DonVi(String MaND, String iID_MaDotPhanBo, String TruongTien, String sLNS,String iID_MaTrangThaiDuyet)
        {
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DkDuyet="";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet = "";
            }
            else
            {
                DkDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            }
            DataTable dtPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
            String DKDotPhanBo = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";

            for (int i = 1; i < dtPhanBo.Rows.Count; i++)
            {
                DKDotPhanBo = "";
                if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    for (int j = 1; j <= i; j++)
                    {
                        DKDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                        if (j < i)
                            DKDotPhanBo += " OR ";
                    }
                    break;
                }

            }
            dtPhanBo.Dispose();
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,sTen
                                         FROM (SELECT iID_MaDonVi FROM PB_PhanBoChiTiet
						                                        WHERE  1=1
								                                        AND iTrangThai=1
								                                       AND ({4})
                                                                        AND ({1}) {2} {3}
								                                        GROUP BY iID_MaDonVi
                                                                        HAVING SUM({0})<>0) a
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as  NS_DonVi ON a.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                         ORDER BY a.iID_MaDonVi", TruongTien, DKLNS, ReportModels.DieuKien_NganSach(MaND), DkDuyet, DKDotPhanBo);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (dtPhanBo.Rows.Count > 1)
            {
                for (int i = 1; i < dtPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            cmd.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                        }
                        break;
                    }
                }
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            DataTable dt = Connection.GetDataTable(cmd);
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
        public static DataTable DanhSachTo(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo, String TruongTien, String KhoGiay)
        {
            String DonVi = DanhSach_DonVi(MaND, iID_MaDotPhanBo, TruongTien, sLNS, iID_MaTrangThaiDuyet);
            String[] arrDonVi = DonVi.Split(',');
            DataTable dtToIn = new DataTable();
            dtToIn.Columns.Add("MaTo", typeof(String));
            dtToIn.Columns.Add("TenTo", typeof(String));
            DataRow R = dtToIn.NewRow();
            dtToIn.Rows.Add(R);
            R[0] = "1";
            R[1] = "Tờ 1";
            if (KhoGiay == "1")
            {
                    int a = 2;
                    for (int i = 0; i < arrDonVi.Length - 8; i = i + 11)
                    {
                        DataRow R1 = dtToIn.NewRow();
                        dtToIn.Rows.Add(R1);
                        R1[0] = a;
                        R1[1] = "Tờ " + a;
                        a++;
                    }
            }
            else
            {
                    int a = 2;
                    for (int i = 0; i < arrDonVi.Length - 4; i = i + 7)
                    {
                        DataRow R1 = dtToIn.NewRow();
                        dtToIn.Rows.Add(R1);
                        R1[0] = a;
                        R1[1] = "Tờ " + a;
                        a++;
                    }
            }
            return dtToIn;
        }
    }
}
