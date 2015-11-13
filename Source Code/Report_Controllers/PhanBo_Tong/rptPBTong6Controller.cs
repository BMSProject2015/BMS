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

namespace VIETTEL.Report_Controllers.PhanBo_Tong
{
    public class rptPBTong6Controller : Controller
    {

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A3_2.xls";
        private const String sFilePath_A4_1_RG = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A4_1_RG.xls";
        private const String sFilePath_A4_2_RG = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A4_2_RG.xls";
        private const String sFilePath_A3_1_RG = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A3_1_RG.xls";
        private const String sFilePath_A3_2_RG = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong6_A3_2_RG.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo_Tong/rptPBTong6.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
         //<summary>
         
         //</summary>
         //<param name="ParentID"></param>
         //<returns></returns>
        public ActionResult EditSubmit(String ParentID, String ToDaXem)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaDotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String TruongTien = Request.Form[ParentID + "_TruongTien"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String LuyKe = Request.Form[ParentID + "_LuyKe"];
            String KhoGiay = Request.Form[ParentID + "_KhoGiay"];
            String ToSo = Request.Form[ParentID + "_ToSo"];
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
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["TruongTien"] = TruongTien;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["LuyKe"] = LuyKe;
            ViewData["ToSo"] = ToSo;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo_Tong/rptPBTong6.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
         //<summary>
         //Hàm khởi tạo báo cáo
         //</summary>
         //<param name="path"></param>
         //<param name="NamLamViec"></param>
         //<param name="sLNS"></param>
         //<param name="iID_MaDotPhanBo"></param>
         //<param name="TruongTien"></param>
         //<returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            FlexCelReport fr = new FlexCelReport();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtDonVi = DS_DonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, LuyKe);
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
         //   Luy ke
            if (LuyKe == "on")
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
             //   A4
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
          //  rut gon
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
            String TenTruongTien = "";
            if (TruongTien == "rTuChi")
            {
                TenTruongTien = " - PHẦN TỰ CHI ";
            }
            else
            {
                TenTruongTien = " - PHẦN HIỆN VẬT ";
            }
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet);
            String tendot = "";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    try
                    {
                        DateTime ngay = Convert.ToDateTime(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString());
                        tendot = " Tháng  " + ngay.ToString("MM") + "  Năm  " + ngay.ToString("yyyy");
                    }
                    catch { tendot = ""; }
                }
            }
            String Dot = String.Format("Đợt {0} : {1} ", ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo), tendot);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            LoadData(fr, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo);
            fr = ReportModels.LayThongTinChuKy(fr, "rptTongHopChiTieuDonVi");
            fr.SetValue("TruongTien", TenTruongTien);
            fr.SetValue("NgayThangNam", NgayThang);
            if (iID_MaDotPhanBo == dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString())
            {
                fr.SetValue("Dot", Dot);

            }
            else
            {
                fr.SetValue("Dot", "Đến " + Dot);
            }
            if (iID_MaDotPhanBo == dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString())
            {
                fr.SetValue("vl_Colum", "Đợt này");
            }
            else
            {
                fr.SetValue("vl_Colum", "BS Đợt này");
            }
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("Phong", ReportModels.CauHinhTenDonViSuDung(3));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("TenTieuDe", "TỔNG HỢP CHỈ TIÊU THEO ĐƠN VỊ");
            fr.Run(Result);
            return Result;


        }
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {

            DataTable data = TongHopChiTieuDonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            if (LuyKe == "on")
            {
                DataTable data_LK = dtDenKyNay(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo);
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

         //<summary>
         //Hàm lấy dữ liệu cho báo cáo
         //</summary>
         //<param name="NamLamViec"></param>
         //<param name="sLNS"></param>
         //<param name="iID_MaDotPhanBo"></param>
         //<param name="TruongTien"></param>
         //<returns></returns>
        public static DataTable TongHopChiTieuDonVi(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(TruongTien))
            {
                TruongTien = "rTuChi";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND,iID_MaTrangThaiDuyet, sLNS,  TruongTien);
            String DKDotPhanBo = "", MaDotDauNam = "";
            DataTable dtDV = DS_DonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, LuyKe);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDV.Rows.Count; i++)
            {
                iID_MaDonVi += dtDV.Rows[i]["iID_MaDonVi"].ToString() + ",";
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
            dtDV.Dispose();

            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DKDV = "";
            String DKSUMDV = "", DKHAVINGDV = "";

            #region Mẫu Lũy kế
            if (dtDotPhanBo.Rows.Count > 1)
            {
                MaDotDauNam = dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
                if (iID_MaDotPhanBo == MaDotDauNam)
                {
                    DKDotPhanBo = "iID_MaPhanBo='" + Guid.Empty.ToString() + "'";
                }
                else
                {
                    for (int i = 2; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 2; j <= i; j++)
                            {
                                DKDotPhanBo += @"iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo"+j+") ) ";
                                if (j < i)
                                    DKDotPhanBo += " OR ";
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                DKDotPhanBo = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                MaDotDauNam = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            if (LuyKe == "on")
            {
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
                            DKSUMDV += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHAVINGDV += " OR SUM(DonVi" + i + ")<>0 ";
                            DKDV += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
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
                            DKSUMDV += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHAVINGDV += " OR SUM(DonVi" + x + ")<>0 ";
                            DKDV += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
                    }
                }
               // Giay A4
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
                            //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMDV += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHAVINGDV += " OR SUM(DonVi" + i + ")<>0 ";
                            DKDV += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
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
                            // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMDV += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHAVINGDV += " OR SUM(DonVi" + x + ")<>0 ";
                            DKDV += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
                    }
                }

                String SQLLuyKe = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                       ,TongCong=SUM(DauNam) + SUM(LuyKe)
                                        ,SUM(DauNam) as DauNam
                                        ,SUM(LuyKe) as LuyKe
                                        {2}
                                      FROM
                                      (
                                      SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa
                                     ,DauNam=CASE WHEN  iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@MaDotDauNam) )  THEN SUM({1}) ELSE 0 END
                                     ,LuyKe=CASE WHEN ({7}) THEN SUM({1}) ELSE 0 END
                                     {0}
                                     FROM PB_PhanBoChiTiet as PB
                                     WHERE pB.iTrangThai=1 
                                     AND ({6}) {4} {5} AND sNG<>''
                                     GROUP BY iID_MaPhanBo, SUBSTRING(sLNS,1,1) ,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa,iID_MaDotPhanBo,iID_MaDonVi,PB.iTrangThai
                                     HAVING SUM({1})!=0
                                     ) as DATATABLE
                                     GROUP BY  NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                     HAVING SUM(DauNam)!=0 OR SUM(LuyKe)!=0 {3}
                                     ORDER BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa", DKDV, TruongTien, DKSUMDV, DKHAVINGDV, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS, DKDotPhanBo);
                SqlCommand cmdLuyKe = new SqlCommand(SQLLuyKe);
                cmdLuyKe.Parameters.AddWithValue("@MaDotDauNam", MaDotDauNam);
                cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmdLuyKe.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                if (dtDotPhanBo.Rows.Count > 1)
                {
                    for (int i = 2; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 2; j <= i; j++)
                            {
                                cmdLuyKe.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                }
                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
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
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                dt = Connection.GetDataTable(cmdLuyKe);
                cmdLuyKe.Dispose();
            }
            #endregion

            #region Mẫu không lũy kế
            else
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
                       // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDV += ",SUM(DonVi" + i + ") AS DonVi" + i;
                        DKHAVINGDV += " OR SUM(DonVi" + i + ")<>0 ";
                        DKDV += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + ") THEN SUM({0}) ELSE 0 END";
                    }
                    DKDV = string.Format(DKDV, TruongTien);
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
                         //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDV += ",SUM(DonVi" + x + ") AS DonVi" + x;
                        DKHAVINGDV += " OR SUM(DonVi" + x + ")<>0 ";
                        DKDV += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                    }
                    DKDV = string.Format(DKDV, TruongTien);
                }
                String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                    ,SUM(DotNay) as DotNay
                                    {2}
                                      FROM
                                      (
                                      SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa
                                     ,DotNay=CASE WHEN iID_MaDotPhanBo=@iID_MaDotPhanBo THEN SUM({1}) ELSE 0 END
                                     {0}
                                     FROM PB_PhanBoChiTiet as PB
                                     WHERE pB.iTrangThai=1 AND iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo)) 
                                     AND ({6}) {4} {5} AND sNG<>''
                                     GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa,iID_MaDotPhanBo,iID_MaDonVi,PB.iTrangThai
                                     HAVING SUM({1})!=0
                                     ) as DATATABLE
                                     GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                     HAVING SUM(DotNay)!=0 {3}
                                     ORDER BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa", DKDV, TruongTien, DKSUMDV, DKHAVINGDV, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
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
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion
            return dt;
        }

         //<summary>
         //Dữ liệu kỳ này cho báo cáo
         //</summary>
         //<param name="NamLamViec"></param>
         //<param name="sLNS"></param>
         //<param name="iID_MaDotPhanBo"></param>
         //<param name="TruongTien"></param>
         //<returns></returns>
        public static DataTable dtDenKyNay(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(TruongTien))
            {
                TruongTien = "rTuChi";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet,sLNS,  TruongTien);
            String DKDotPhanBo = "";
            DataTable dtDV = DS_DonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, LuyKe);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDV.Rows.Count; i++)
            {
                iID_MaDonVi += dtDV.Rows[i]["iID_MaDonVi"].ToString() + ",";
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
            dtDV.Dispose();

            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DKDV = "";
            String DKSUMDV = "", DKHAVINGDV = "";
            if (dtDotPhanBo.Rows.Count > 1)
            {
                for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 0; j <= i; j++)
                        {
                            DKDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                            if (j < i)
                                DKDotPhanBo += " OR ";
                        }
                        break;
                    }
                }
            }
            else
            {
                DKDotPhanBo = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            if (LuyKe == "on")
            {
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
                            DKSUMDV += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHAVINGDV += " OR SUM(DonVi" + i + ")<>0 ";
                            DKDV += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + ") THEN SUM({0}) ELSE 0 END";
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
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
                            DKSUMDV += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHAVINGDV += " OR SUM(DonVi" + x + ")<>0 ";
                            DKDV += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + ") THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
                    }
                }
               // Giay A4
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
                            //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMDV += ",SUM(DonVi" + i + ") AS DonVi" + i;
                            DKHAVINGDV += " OR SUM(DonVi" + i + ")<>0 ";
                            DKDV += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + ") THEN SUM({0}) ELSE 0 END";
                        }
                        DKDV = string.Format(DKDV, TruongTien, DKDotPhanBo);
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
                            // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMDV += ",SUM(DonVi" + x + ") AS DonVi" + x;
                            DKHAVINGDV += " OR SUM(DonVi" + x + ")<>0 ";
                            DKDV += " ,DonVi" + x + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + x + ") THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKDV = string.Format(DKDV, TruongTien);
                    }
                }

                String SQLLuyKe = String.Format(@"SELECT 
                                                SUM(a) as a
                                                {1}
                                                FROM
                                                (
                                                SELECT 
                                                a=0
                                                {0}
                                                FROM PB_PhanBoChiTiet as PB
                                                WHERE pB.iTrangThai=1 
                                                AND ({5}) {4} AND sNG<>'' {3}
                                                AND ({6})
                                                GROUP BY iID_MaDonVi
                                                HAVING SUM({7})!=0
                                                ) as DATATABLE
                                                HAVING SUM(a)!=0 {2} ", DKDV, DKSUMDV, DKHAVINGDV, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS, DKDotPhanBo, TruongTien);
                SqlCommand cmdLuyKe = new SqlCommand(SQLLuyKe);
                cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmdLuyKe.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                if (dtDotPhanBo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                cmdLuyKe.Parameters.AddWithValue(@"iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                }
                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
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
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                            x++;
                        }
                    }
                }
                dt = Connection.GetDataTable(cmdLuyKe);
                cmdLuyKe.Dispose();

            }
            return dt;
        }
      


         //<summary>
         //Hàm View PDF
         //</summary>
         //<param name="NamLamViec"></param>
         //<param name="sLNS"></param>
         //<param name="iID_MaDotPhanBo"></param>
         //<param name="TruongTien"></param>
         //<returns></returns>
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LuyKe != "on")
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo);
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
         //<summary>
         //Hàm thực hiện việc xuất dữ liệu ra file excel
         //</summary>
         //<param name="NamLamViec"></param>
         //<param name="sLNS"></param>
         //<param name="iID_MaDotPhanBo"></param>
         //<param name="TruongTien"></param>
         //<returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LuyKe != "on")
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopChiTieuDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
         //<summary>
         //Lấy ngay đợt phân bổ
         //</summary>
         //<param name="iID_MaDotPhanBo"></param>
         //<returns></returns>
        public static DataTable NgayDotPhanBo(String iID_MaDotPhanBo)
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT DISTINCT CONVERT(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo
                                        FROM PB_PhanBoChiTiet as PBCT
                                        INNER JOIN PB_DotPhanBo as DPB ON PBCT.iID_MaDotPhanBo=DPB.iID_MaDotPhanBo
                                        WHERE DPB.iID_MaDotPhanBo=@iID_MaDotPhanBo");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

         //<summary>
         //Loại ngân sách
         //</summary>
         //<returns></returns>
        public static DataTable LoaiNganSach()
        {
            DataTable dt = null;
            String SQL = " SELECT distinct sLNS, sLNS+' - '+sTen as sTen FROM NS_LoaiNganSach WHERE LEN(sLNS)=7 AND sL = '' ORDER By iSTT ";
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }


         //Lấy danh sách đơn vị 
         //</summary>
         //<param name="NamLamViec"></param>
         //<param name="sLNS"></param>
         //<param name="iID_MaDotPhanBo"></param>
         //<param name="TruongTien"></param>
         //<returns></returns>
        public static DataTable DS_DonVi(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String LuyKe)
        {
            DataTable dtDonVi = new DataTable();
            String DK_Duyet = " AND PB.iID_MaTrangThaiDuyet='" + iID_MaTrangThaiDuyet + "'";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND,iID_MaTrangThaiDuyet, sLNS, TruongTien);
            String DKDotPhanBo = "";
            if (LuyKe == "on")
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            DKDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBo" + j;
                            if (j < i)
                                DKDotPhanBo += " OR ";
                        }
                    }
                }
            }
            else
            {
                DKDotPhanBo = "iID_MaDotPhanBo=@iID_MaDotPhanBo";
            }
            if (String.IsNullOrEmpty(DKDotPhanBo))
            {
                DKDotPhanBo = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            String SQL = string.Format(@"SELECT DISTINCT DV.iID_MaDonVi,DV.sTen
                                            FROM PB_PhanBoChiTiet AS PB
                                            INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON PB.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE PB.iTrangThai=1  AND {3} 
                                            AND ( iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND ({4}) ) ) ) {1} {2}
                                            GROUP BY DV.iID_MaDonVi,DV.sTen
                                            HAVING SUM({0})!=0
                                            ORDER BY iID_MaDonVi", TruongTien, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS, DKDotPhanBo);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (LuyKe == "on")
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                        }
                    }
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            dtDonVi = Connection.GetDataTable(cmd);
            return dtDonVi;
        }

        public class LNSdata
        {
            public string iID_MaDotPhanBo { get; set; }
            public string ToSo { get; set; }
        }

      
        public JsonResult ds_DotPhanBo(String ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            return Json(obj_DotPhanBo(ParentID, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo), JsonRequestBehavior.AllowGet);
        }
        public LNSdata obj_DotPhanBo(String ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            LNSdata _LNSdata = new LNSdata();
            #region Đợt phân bổ
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet, sLNS, TruongTien);
            SelectOptionList slPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _LNSdata.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 80%\"onchange=\"Chon()\"");
            dtDotPhanBo.Dispose();
            #endregion
            #region Tờ Số
            DataTable dtToSo = dtTo(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _LNSdata.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtToSo.Dispose();
            #endregion
            return _LNSdata;
        }
        public static DataTable dtTo(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe)
        {
            DataTable dtDonVi = DS_DonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, LuyKe);
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
           // Luy ke
            if (LuyKe == "on")
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
               // giay a3
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
        public static DataTable NS_LoaiNganSach()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sL = '' ORDER By sLNS");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
