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
    public class rptPhanBo_8_8Controller : Controller
    {
        //
        // GET: /rptPhanBo_8_8/
        private const String sFilePathA3_LK_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A3_LK_1.xls";
        private const String sFilePathA3_LK_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A3_LK_2.xls";
        private const String sFilePathA3_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A3_1.xls";
        private const String sFilePathA3_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A3_2.xls";
        private const String sFilePathA4_LK_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A4_LK_1.xls";
        private const String sFilePathA4_LK_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A4_LK_2.xls";
        private const String sFilePathA4_1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A4_1.xls";
        private const String sFilePathA4_2 = "/Report_ExcelFrom/PhanBo/rptPhanBo_8_8_A4_2.xls";
       
        
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_8_8.aspx";
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
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String LuyKe = Convert.ToString(Request.Form[ParentID + "_LuyKe"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["sLNS"] = sLNS;
            ViewData["TruongTien"] = TruongTien;
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["LuyKe"] = LuyKe;
            ViewData["ToSo"] = ToSo;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_8_8.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult ViewPDF(String sLNS, String iID_MaDonVi, String iID_MaDotPhatBo, String TruongTien, String LuyKe, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (LuyKe == "on")
                {
                    if (ToSo == "1") DuongDan = sFilePathA3_LK_1;
                    else DuongDan = sFilePathA3_LK_2;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePathA3_1;
                    else DuongDan = sFilePathA3_2;
                }
            }
            else
            {
                if (LuyKe == "on")
                {
                    if (ToSo == "1") DuongDan = sFilePathA4_LK_1;
                    else DuongDan = sFilePathA4_LK_2;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePathA4_1;
                    else DuongDan = sFilePathA4_2;
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sLNS, iID_MaDonVi, iID_MaDotPhatBo, TruongTien, LuyKe, KhoGiay, ToSo, iID_MaTrangThaiDuyet);
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
        public ExcelFile CreateReport(String path, String sLNS, String iID_MaDonVi, String iID_MaDotPhatBo, String TruongTien, String LuyKe, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String Dot = "";
            DataTable dtDot = PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi);
            String NgayDot = "";
            for (int i = 1; i < dtDot.Rows.Count;i++ )
            {
                if (iID_MaDotPhatBo == dtDot.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    NgayDot = dtDot.Rows[i]["dNgayDotPhanBo"].ToString();
                    break;
                }
            }
            dtDot.Dispose();
            if (!String.IsNullOrEmpty(NgayDot))
            {
                Dot = ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhatBo).ToString() + " Tháng " + NgayDot.Substring(3, 2) + " năm " + NgayDot.Substring(6, 4);
            }
            if (LuyKe == "on")
            {
                Dot = " Đến đợt " + Dot;
            }
            else
            {
                Dot = " Đợt " + Dot;
            }

            String[] arrDonVi = iID_MaDonVi.Split(',');
            String[] TenDV = new String[100];
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                TenDV[i] = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen"));
            }
            String TenTruongTien = "";
            if (TruongTien == "rTuChi")
            {
                TenTruongTien = " TỰ CHI";
            }
            else
            {
                TenTruongTien = "HIỆN VẬT";
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(2);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(3);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_8_8");


            String DonVi = DSDonVi_CoDuLieu(MaND,iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhatBo, TruongTien, LuyKe);
            String[] arrDonVi1 = DonVi.Split(',');
            String[] arrTenDonVi;
            //A3
            if (KhoGiay == "1")
            {
                #region LuyKe
                if (LuyKe == "on")
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
                #endregion
                #region Ko Chon luy ke
                else
                {
                    if (ToSo == "1")
                    {
                        if (arrDonVi1.Length < 10)
                        {
                            int a = 10 - arrDonVi1.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                        }
                        arrDonVi1 = DonVi.Split(',');
                        arrTenDonVi = new String[10];
                        for (int i = 0; i < 10; i++)
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
                        if (arrDonVi1.Length < 10 + 11 * (Convert.ToInt16(ToSo) - 1))
                        {
                            int a = 10 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi1.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi1 = DonVi.Split(',');
                        }
                        arrTenDonVi = new String[11];
                        int x = 1;
                        for (int i = 10 + 11 * ((Convert.ToInt16(ToSo) - 2)); i < 10 + 11 * ((Convert.ToInt16(ToSo) - 1)); i++)
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
                #endregion
            }
            //A4
            else
            {
                #region LuyKe
                if (LuyKe == "on")
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
                            for (int i=0;i<4;i++)
                            {
                                if (arrDonVi1[i] != null && arrDonVi1[i] != "-1" && arrDonVi1[i] !=Guid.Empty.ToString())
                                {
                                    arrTenDonVi[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi1[i], "sTen").ToString();
                                }
                            }
                            
                            for (int i = 1; i <= arrTenDonVi.Length; i++)
                            {
                                fr.SetValue("DonVi" + i, arrTenDonVi[i-1]);
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
                                arrTenDonVi[x-1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi1[i], "sTen").ToString();
                                x++;
                            }
                        }

                        for (int i = 1; i <= arrTenDonVi.Length; i++)
                        {
                            fr.SetValue("DonVi" + i, arrTenDonVi[i - 1]);
                        }      
                    }
                }
                #endregion
                #region Ko Chon luy ke
                else
                {
                    if (ToSo == "1")
                    {
                        if (arrDonVi1.Length < 6)
                        {
                            int a = 6 - arrDonVi1.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                        }
                        arrDonVi1 = DonVi.Split(',');
                        arrTenDonVi = new String[6];
                        for (int i = 0; i < 6; i++)
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
                        if (arrDonVi1.Length < 6 + 7 * (Convert.ToInt16(ToSo) - 1))
                        {
                            int a = 6 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi1.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi1 = DonVi.Split(',');
                        }
                        arrTenDonVi = new String[7];
                        int x = 1;
                        for (int i = 6 + 7 * ((Convert.ToInt16(ToSo) - 2)); i < 6 + 7 * ((Convert.ToInt16(ToSo) - 1)); i++)
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
                #endregion
            }

            
                fr.SetValue("Dot", Dot);
                LoadData(fr, MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhatBo, TruongTien, LuyKe, KhoGiay, ToSo);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("TruongTien", TenTruongTien);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.SetValue("ToSo", ToSo);
                fr.Run(Result);
                return Result;
            
        }
        private void LoadData(FlexCelReport fr, String MaND,String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String iID_MaDotPhatBo, String TruongTien, String LuyKe, String KhoGiay, String ToSo)
        {

            DataTable data = PhanBo_8_8(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhatBo, TruongTien, LuyKe, KhoGiay, ToSo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            if (LuyKe == "on")
            {
                DataTable dtLuyKe = LuyKe_8_8(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhatBo, TruongTien, LuyKe, KhoGiay, ToSo);
                fr.AddTable("LuyKe", dtLuyKe);
                dtLuyKe.Dispose();
            }
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
        public clsExcelResult ExportToExcel(String sLNS, String iID_MaDonVi, String iID_MaDotPhatBo, String TruongTien, String LuyKe, String KhoGiay, String ToSo, String iID_MaTrangThaiDuyet)
        {
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (LuyKe == "on")
                {
                    if (ToSo == "1") DuongDan = sFilePathA3_LK_1;
                    else DuongDan = sFilePathA3_LK_2;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePathA3_1;
                    else DuongDan = sFilePathA3_2;
                }
            }
            else
            {
                if (LuyKe == "on")
                {
                    if (ToSo == "1") DuongDan = sFilePathA4_LK_1;
                    else DuongDan = sFilePathA4_LK_2;
                }
                else
                {
                    if (ToSo == "1") DuongDan = sFilePathA4_1;
                    else DuongDan = sFilePathA4_2;
                }
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sLNS, iID_MaDonVi, iID_MaDotPhatBo, TruongTien, LuyKe, KhoGiay, ToSo, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public class data
        {
            public String iID_MaDotPhanBo { get; set; }
            public String ToSo { get; set; }
        }
        [HttpGet]
        public JsonResult ds_DotPhanBo(String ParentID, String sLNS, String MaND,String iID_MaTrangThaiDuyet, String iID_MaDonVi, String iID_MaDotPhanBo, String TruongTien, String LuyKe, String KhoGiay, String ToSo)
        {
            return Json(obj_DanhSachDotPhatBo(ParentID, sLNS, MaND, iID_MaTrangThaiDuyet,iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe, KhoGiay, ToSo), JsonRequestBehavior.AllowGet);
        }
        public data obj_DanhSachDotPhatBo(String ParentID, String sLNS, String MaND,String iID_MaTrangThaiDuyet, String iID_MaDonVi, String iID_MaDotPhanBo,String TruongTien, String LuyKe,String KhoGiay,String ToSo)
        {
            data _data = new data();
            DataTable dtDotPhatBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi);
            SelectOptionList slDotPhatBo = new SelectOptionList(dtDotPhatBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _data.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slDotPhatBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonDPB()\"");

            DataTable dtToSo = DanhSachTo(MaND,iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe, KhoGiay);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _data.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100%\"");
            return _data;
        }
        public DataTable PhanBo_8_8(String MaND,String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String iID_MaDotPhanBo, String TruongTien, String LuyKe,String KhoGiay,String ToSo)
        {
            DataTable dt = new DataTable();
          
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DKSELECT = "", DKCASE = "", DKHAVING = "";
            String DonVi = DSDonVi_CoDuLieu(MaND, iID_MaTrangThaiDuyet,sLNS, iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe);
            String[] arrDonVi = DonVi.Split(',');
            DataTable dtto = DanhSachTo(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe, KhoGiay);
            DataTable dtPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi);
            String MaDotDauNam = "", MaDotBoSung = "";                   
            if (dtPhanBo.Rows.Count > 2)
            {                
                MaDotDauNam = dtPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
                if (iID_MaDotPhanBo == MaDotDauNam || iID_MaDotPhanBo == dtPhanBo.Rows[0]["iID_MaDotPhanBo"].ToString())
                {
                    MaDotBoSung = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                }
                else
                {                 
                    for (int i = 2; i < dtPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 2; j <= i; j++)
                            {
                                MaDotBoSung += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                                if (j < i)
                                    MaDotBoSung += " OR ";
                            }
                            break;
                        }
                        
                    }
                }
            }
            else if (dtPhanBo.Rows.Count == 2 && iID_MaDotPhanBo == dtPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString())
            {
                MaDotDauNam = dtPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
                MaDotBoSung = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                MaDotDauNam = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                MaDotBoSung = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            #region Giấy A3         
            if (KhoGiay == "1")
            {
                #region Chọn lũy kế
                if (LuyKe == "on")
                {
                    String DKDonVi = "";
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < arrDonVi.Length - 1)
                            DKDonVi += " OR ";
                    }
                    //Chọn tờ số 1
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
                            DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                            DKCASE += ",DONVI" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + i + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                        }

                    }
                    //Chọn các tờ lơn hơn 1
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
                            DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                            DKCASE += ",DONVI" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + x + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                            x++;
                        }
                    }

                    DKCASE = String.Format(DKCASE, MaDotBoSung, TruongTien);
                    String SQL = String.Format(@"SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,SUM(DauNam) AS DauNam,SUM(BoSung) as BoSung {1}
                                                FROM
                                                (
                                                SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                ,DauNam=CASE WHEN (iID_MaDotPhanBo=@MaDotDauNam AND ({5})) THEN SUM({0}) ELSE 0 END
                                                ,BoSung=CASE WHEN (({4}) AND ({5}) ) THEN SUM({0}) ELSE 0 END
                                                {2}                                                
                                                FROM PB_PhanBoChiTiet
                                                WHERE iTrangThai=1 AND sNG<>'' AND ({6}) {7}
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,iID_MaDotPhanBo,iID_MaDonVi
                                                ) as A
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                HAVING SUM(DauNam)<>0 OR SUM(BoSung)<>0 {3}", TruongTien, DKSELECT, DKCASE, DKHAVING, MaDotBoSung, DKDonVi, DKLNS, ReportModels.DieuKien_NganSach(MaND));
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("MaDotDauNam", MaDotDauNam);
                    if (dtPhanBo.Rows.Count > 1)
                    {
                        for (int i = 2; i < dtPhanBo.Rows.Count; i++)
                        {
                            if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                            {
                                for (int j = 2; j <= i; j++)
                                {
                                    cmd.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                                }
                                break;
                            }
                        }
                    }
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi"+i, arrDonVi[i]);
                    }
                    for (int i = 0; i < arrLNS.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                }
                #endregion
                #region Nếu không chọn lũy kế
                else
                {
                    String DKDonVi = "";
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < arrDonVi.Length - 1)
                            DKDonVi += " OR ";
                    }
                    //Chọn tờ số 1
                    if (ToSo == "1")
                    {
                        if (arrDonVi.Length < 10)
                        {
                            int a = 10 - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        for (int i = 1; i <= 10; i++)
                        {
                            DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                            DKCASE += ",DONVI" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + i + " AND iID_MaDotPhanBo=@iID_MaDotPhanBo) THEN SUM({0}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                        }

                    }
                    //Chọn các tờ lơn hơn 1
                    else
                    {
                        if (arrDonVi.Length < 10 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 10 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 10 + 11 * tg; i < 10 + 11 * (tg + 1); i++)
                        {
                            DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                            DKCASE += ",DONVI" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + x + " AND iID_MaDotPhanBo=@iID_MaDotPhanBo) THEN SUM({0}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                            x++;
                        }
                    }

                    DKCASE = String.Format(DKCASE, TruongTien);
                    String SQL = String.Format(@"SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,SUM(DotNay) AS DotNay {1}
                                                FROM
                                                (
                                                SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                ,DotNay=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo AND ({5})) THEN SUM({0}) ELSE 0 END                                               
                                                {2}                                                
                                                FROM PB_PhanBoChiTiet
                                                WHERE  iTrangThai=1 AND sNG<>'' AND ({6}) {7}
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,iID_MaDotPhanBo,iID_MaDonVi
                                                ) as A
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                HAVING SUM(DotNay)<>0 {3}", TruongTien, DKSELECT, DKCASE, DKHAVING, MaDotBoSung, DKDonVi, DKLNS, ReportModels.DieuKien_NganSach(MaND));
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("iID_MaDotPhanBo", iID_MaDotPhanBo);
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 10 + 11 * tg; i < 10 + 11 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + x, arrDonVi[i]);
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
            }
            #endregion
            #region Giấy A4
            else
            {
                String DKDonVi = "";
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < arrDonVi.Length - 1)
                        DKDonVi += " OR ";
                }
             
                #region Chọn lũy kế
                if (LuyKe == "on")
                {
                    //Chọn tờ số 1
                    if (ToSo == "1")
                    {
                        if (arrDonVi.Length < 4)
                        {
                            int a=4-arrDonVi.Length;
                            for (int i=0;i<a;i++)
                            {
                                DonVi+=",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        for (int i = 1; i <= 4;i++)
                        {
                            DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                            DKCASE += ",DONVI" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia"+i+" AND ({0})) THEN SUM({1}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                        }
                       
                    }
                    //Chọn các tờ lơn hơn 1
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
                        int tg=Convert.ToInt16(ToSo)-2;
                        int x = 1;
                        for (int i = 4+7*tg; i < 4+7*(tg+1); i++)
                        {
                            DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                            DKCASE += ",DONVI" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + x + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                            x++;
                        } 
                    }

                    DKCASE = String.Format(DKCASE, MaDotBoSung, TruongTien);                                           
                    String SQL = String.Format(@"SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,SUM(DauNam) AS DauNam,SUM(BoSung) as BoSung {1}
                                                FROM
                                                (
                                                SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                ,DauNam=CASE WHEN (iID_MaDotPhanBo=@MaDotDauNam AND ({5})) THEN SUM({0}) ELSE 0 END
                                                ,BoSung=CASE WHEN (({4}) AND ({5}) ) THEN SUM({0}) ELSE 0 END
                                                {2}                                                
                                                FROM PB_PhanBoChiTiet
                                                WHERE iTrangThai=1 AND sNG<>'' AND ({6}) {7}
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,iID_MaDotPhanBo,iID_MaDonVi
                                                ) as A
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                HAVING SUM(DauNam)<>0 OR SUM(BoSung)<>0 {3}", TruongTien, DKSELECT, DKCASE, DKHAVING, MaDotBoSung, DKDonVi, DKLNS,ReportModels.DieuKien_NganSach(MaND));
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("MaDotDauNam", MaDotDauNam);
                    cmd.Parameters.AddWithValue("iID_MaDotPhanBo", iID_MaDotPhanBo);
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                    if (dtPhanBo.Rows.Count > 1)
                    {
                        for (int i = 2; i < dtPhanBo.Rows.Count; i++)
                        {
                            if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                            {
                                for (int j = 2; j <= i; j++)
                                {
                                    cmd.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                                }
                                break;
                            }
                        }
                    }
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                           int tg=Convert.ToInt16(ToSo)-2;
                            int x = 1;
                           for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                           {
                               cmd.Parameters.AddWithValue("@iID_MaDonVia" + x, arrDonVi[i]);
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
                #region Nếu không chọn lũy kế
                else
                {
                     DKDonVi = "";
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                         DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                            if (i < arrDonVi.Length-1)
                                DKDonVi += " OR ";
                    }
                    //Chọn tờ số 1
                    if (ToSo == "1")
                    {
                        if (arrDonVi.Length < 6)
                        {
                            int a = 6 - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        for (int i = 1; i <= 6; i++)
                        {
                           
                            DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                            DKCASE += ",DONVI" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + i + " AND iID_MaDotPhanBo=@iID_MaDotPhanBo) THEN SUM({0}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                        }

                    }
                    //Chọn các tờ lơn hơn 1
                    else
                    {
                        if (arrDonVi.Length < 6 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 6 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                DonVi += ",-1";
                            }
                            arrDonVi = DonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 6 + 7 * tg; i < 6 + 7 * (tg + 1); i++)
                        {
                            
                            DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                            DKCASE += ",DONVI" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + x + " AND iID_MaDotPhanBo=@iID_MaDotPhanBo) THEN SUM({0}) ELSE 0 END";
                            DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                            x++;
                        }
                    }

                    DKCASE = String.Format(DKCASE, TruongTien);
                    String SQL = String.Format(@"SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,SUM(DotNay) AS DotNay {1}
                                                FROM
                                                (
                                                SELECT sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                ,DotNay=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo AND ({5})) THEN SUM({0}) ELSE 0 END                                               
                                                {2}                                                
                                                FROM PB_PhanBoChiTiet
                                                WHERE  iTrangThai=1 AND sNG<>'' AND ({6}) {7}
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa,iID_MaDotPhanBo,iID_MaDonVi
                                                ) as A
                                                GROUP BY sLNS, sL, sK, sM, sTM, sTTM, sNG, sMoTa
                                                HAVING SUM(DotNay)<>0 {3}", TruongTien, DKSELECT, DKCASE, DKHAVING, MaDotBoSung, DKDonVi, DKLNS,ReportModels.DieuKien_NganSach(MaND));
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("iID_MaDotPhanBo", iID_MaDotPhanBo);
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                         cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 6 + 7 * tg; i < 6 + 7 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + x, arrDonVi[i]);
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
            }
           
            #endregion
            return dt;
            
        }
        public static DataTable LuyKe_8_8(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String iID_MaDotPhanBo, String TruongTien, String LuyKe, String KhoGiay, String ToSo)
        {
            DataTable dt = new DataTable();
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }

            String DKSELECT = "", DKCASE = "", DKHAVING = "";
            String DonVi = DSDonVi_CoDuLieu(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe);
            String[] arrDonVi = DonVi.Split(',');
            DataTable dtto = DanhSachTo(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe, KhoGiay);
            DataTable dtPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi);
            String MaDotDauNam = "", MaDotBoSung = "";
            if (dtPhanBo.Rows.Count > 2)
            {
                MaDotDauNam = dtPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();

                for (int i = 1; i < dtPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            MaDotBoSung += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                            if (j < i)
                                MaDotBoSung += " OR ";
                        }
                        break;
                    }
                }
            }
            else if (dtPhanBo.Rows.Count == 2 && iID_MaDotPhanBo == dtPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString())
            {
                MaDotDauNam = dtPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
                MaDotBoSung = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                MaDotDauNam = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                MaDotBoSung = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }

            String DKDonVi = "";
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            //Khổ giấy A3
            if (KhoGiay == "1")
            {
                //tờ số 1
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
                        DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                        DKCASE += ",DONVI" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + i + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                        DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                    }
                }
                //các tờ lớn hơn 1
                else
                {
                    if (arrDonVi.Length < 8 + 11 * (Convert.ToInt16(ToSo) - 1))
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
                        DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                        DKCASE += ",DONVI" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + x + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                        DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                        x++;
                    }
                }
            }

            #region //Khổ giấy A4
            else
            {
                //tờ số 1
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
                        DKSELECT += ",SUM(DONVI" + i + ") AS DONVI" + i;
                        DKCASE += ",DONVI" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + i + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                        DKHAVING += " OR SUM(DONVI" + i + ") <>0";
                    }
                }
                //các tờ lớn hơn 1
                else
                {
                    if (arrDonVi.Length < 4 + 7 * (Convert.ToInt16(ToSo) - 1))
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
                        DKSELECT += ",SUM(DONVI" + x + ") AS DONVI" + x;
                        DKCASE += ",DONVI" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVia" + x + " AND ({0})) THEN SUM({1}) ELSE 0 END";
                        DKHAVING += " OR SUM(DONVI" + x + ") <>0";
                        x++;
                    }
                }
            }
            #endregion
                DKCASE = String.Format(DKCASE, MaDotBoSung, TruongTien);

                String SQL = String.Format(@"SELECT SUM(BoSung) as BoSung {1}
                                                FROM
                                                (
                                                SELECT                                               
                                                BoSung=CASE WHEN (({4}) AND ({5}) ) THEN SUM({0}) ELSE 0 END
                                                {2}                                                
                                                FROM PB_PhanBoChiTiet
                                                WHERE iTrangThai=1 AND sNG<>'' {6} AND ({7})
                                                GROUP BY iID_MaDotPhanBo,iID_MaDonVi
                                                ) as A                                               
                                              HAVING SUM(BoSung)<>0 {3}", TruongTien, DKSELECT, DKCASE, DKHAVING, MaDotBoSung, DKDonVi, ReportModels.DieuKien_NganSach(MaND),DKLNS);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("MaDotDauNam", MaDotDauNam);
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
                //Giay a3
                if (KhoGiay == "1")
                {

                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                //giay a4
                else
                {
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVia" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                 for (int i = 0; i < arrDonVi.Length; i++)
                    {
                         cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count == 0)
                {
                    DataRow r = dt.NewRow();
                    r["BoSung"] = 0.0;
                    dt.Rows.InsertAt(r, 0);
                }
                dt.Dispose();
                return dt;
            
        }
        public static String DSDonVi_CoDuLieu(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String iID_MaDotPhanBo,String TruongTien,String LuyKe)
        {
            String DonVi = "";
            String[] arrDonVi=iID_MaDonVi.Split(',');
            String DKDonVi = "";
            for (int i = 0; i < arrDonVi.Length;i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            DataTable dtDotPhanBo=PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet, sLNS,iID_MaDonVi);
            String DKDotPhanBo = "";
            if (LuyKe == "on")
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 0; j <i;j++ )
                        {
                            DKDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                            if (j < i - 1)
                                DKDotPhanBo += " OR ";
                        }
                        break;
                    }                 
                }
            }
            else
            {
                DKDotPhanBo = "iID_MaDotPhanBo=@iID_MaDotPhanBo";
            }
            if (String.IsNullOrEmpty(DKDotPhanBo))
            {
                DKDotPhanBo ="iID_MaDotPhanBo='"+ Guid.Empty.ToString()+"'";
            }
            String DKLNS = "";
            String[] arrLNS=sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length;i++ )
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            String SQL = String.Format(@"SELECT  iID_MaDonVi,SUM({1}) as TruongTien
                                        FROM PB_PhanBoChiTiet
                                        WHERE ({2}) AND iTrangThai=1 AND({3}) AND ({0}) {4} {5}
                                        GROUP BY iID_MaDonVi
                                        HAVING SUM({1})<>0
                                        ORDER BY iID_MaDonVi", DKDonVi,TruongTien,DKDotPhanBo,DKLNS,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i = 0; i < arrDonVi.Length;i++ )
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
            for (int i = 0; i < arrLNS.Length;i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (LuyKe == "on")
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 0; j < i; j++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j + 1]["iID_MaDotPhanBo".ToString()]);
                        }
                        break;
                    }
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            for (int i=0;i<dt.Rows.Count;i++)
            {
                DonVi += dt.Rows[i]["iID_MaDonVi"].ToString() +",";
            }
            if (DonVi.Length > 1)
                DonVi = DonVi.Substring(0, DonVi.Length - 1);
            if (String.IsNullOrEmpty(DonVi))
            {
                DonVi = Guid.Empty.ToString();
            }

            return DonVi;
        }
        public static DataTable DanhSachTo(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String iID_MaDotPhanBo, String TruongTien, String LuyKe,String KhoGiay)
        {
            String DonVi = DSDonVi_CoDuLieu(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, iID_MaDotPhanBo, TruongTien, LuyKe);
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
                if (LuyKe == "on")
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
                    for (int i = 0; i < arrDonVi.Length - 10; i = i + 11)
                    {
                        DataRow R1 = dtToIn.NewRow();
                        dtToIn.Rows.Add(R1);
                        R1[0] = a;
                        R1[1] = "Tờ " + a;
                        a++;
                    }
                }
            }
            else
            {
                if (LuyKe == "on")
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
                else
                {
                    int a = 2;
                    for (int i = 0; i < arrDonVi.Length - 6; i = i +7)
                    {
                        DataRow R1 = dtToIn.NewRow();
                        dtToIn.Rows.Add(R1);
                        R1[0] = a;
                        R1[1] = "Tờ " + a;
                        a++;
                    }
                }
            }
            return dtToIn;
        }
        
    }
}
