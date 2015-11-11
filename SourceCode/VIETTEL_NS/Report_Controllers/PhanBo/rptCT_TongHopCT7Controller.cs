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

namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptCT_TongHopCT7Controller : Controller
    {
        //
        // GET: /rptCT_TongHopCT7/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/PhanBo/rptTH_7_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/PhanBo/rptTH_7_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/PhanBo/rptTH_7_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/PhanBo/rptTH_7_A3_2.xls";

        private const String sFilePath_A4_1_RG = "/Report_ExcelFrom/PhanBo/rptTH_7_A4_1_RG.xls";
        private const String sFilePath_A4_2_RG = "/Report_ExcelFrom/PhanBo/rptTH_7_A4_2_RG.xls";
        private const String sFilePath_A3_1_RG = "/Report_ExcelFrom/PhanBo/rptTH_7_A3_1_RG.xls";
        private const String sFilePath_A3_2_RG = "/Report_ExcelFrom/PhanBo/rptTH_7_A3_2_RG.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptChiTieu_TongHopChiTieu_7.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID,String ToDaXem)
        {
           
            String sLNS = Request.Form["sLNS"];
            String iID_MaDotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String TruongTien = Request.Form[ParentID + "_TruongTien"];
            String KhoGiay = Request.Form[ParentID + "_KhoGiay"];
            String LuyKe = Request.Form[ParentID + "_LuyKe"];
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
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["TruongTien"] = TruongTien;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["LuyKe"] = LuyKe;
            ViewData["ToSo"] = ToSo;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptChiTieu_TongHopChiTieu_7.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path,String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay,String LuyKe,String ToSo)
        {
            FlexCelReport fr = new FlexCelReport();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtNhomDonVi = DS_NhomDonVi(MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,LuyKe);
            String[] TenNhomDV;
            String iID_MaNhomDonVi = "";
            for (int i = 0; i < dtNhomDonVi.Rows.Count; i++)
            {
                iID_MaNhomDonVi += dtNhomDonVi.Rows[i]["iID_MaNhomDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaNhomDonVi))
            {
                iID_MaNhomDonVi = iID_MaNhomDonVi.Substring(0, iID_MaNhomDonVi.Length - 1);
            }
            String NhomDonVi = iID_MaNhomDonVi;
            String[] arrNhomDonVi = iID_MaNhomDonVi.Split(',');
            dtNhomDonVi.Dispose();
            //Luy ke
            if (LuyKe == "on")
            {
                if (KhoGiay == "1")
                {
                    if (ToSo == "1")
                    {
                        if (arrNhomDonVi.Length < 8)
                        {
                            int a1 = 8 - arrNhomDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                NhomDonVi += ","+Guid.Empty.ToString();
                            }
                        }
                        arrNhomDonVi = NhomDonVi.Split(',');
                        TenNhomDV = new String[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (arrNhomDonVi[i] != null && arrNhomDonVi[i] != ""+Guid.Empty.ToString()+"" && arrNhomDonVi[i] != Guid.Empty.ToString() && arrNhomDonVi[i] != "")
                            {
                                TenNhomDV[i] = CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", arrNhomDonVi[i], "sGhiChu").ToString();
                            }
                        }

                        for (int i = 1; i <= TenNhomDV.Length; i++)
                        {
                            fr.SetValue("NhomDonVi" + i, TenNhomDV[i - 1]);
                        }
                    }
                    else
                    {
                        if (arrNhomDonVi.Length < 8 + 11 * (Convert.ToInt16(ToSo) - 1))
                        {
                            int a1 = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                NhomDonVi += ","+Guid.Empty.ToString();
                            }
                            arrNhomDonVi = NhomDonVi.Split(',');
                        }
                        TenNhomDV = new String[11];
                        int x = 1;
                        for (int i = 8 + 11 * ((Convert.ToInt16(ToSo) - 2)); i < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)); i++)
                        {
                            if (arrNhomDonVi[i] != null && arrNhomDonVi[i] != ""+Guid.Empty.ToString()+"" && arrNhomDonVi[i] != Guid.Empty.ToString() && arrNhomDonVi[i] != "")
                            {
                                TenNhomDV[x - 1] = CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", arrNhomDonVi[i], "sGhiChu").ToString();
                                x++;
                            }
                        }

                        for (int i = 1; i <= TenNhomDV.Length; i++)
                        {
                            fr.SetValue("NhomDonVi" + i, TenNhomDV[i - 1]);
                        }
                    }
                }
                //A4
                else
                {
                    if (ToSo == "1")
                    {
                        if (arrNhomDonVi.Length < 4)
                        {
                            int a1 = 4 - arrNhomDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                NhomDonVi += ","+Guid.Empty.ToString();
                            }
                        }
                        arrNhomDonVi = NhomDonVi.Split(',');
                        TenNhomDV = new String[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (arrNhomDonVi[i] != null && arrNhomDonVi[i] != ""+Guid.Empty.ToString()+"" && arrNhomDonVi[i] != Guid.Empty.ToString() && arrNhomDonVi[i] != "")
                            {
                                TenNhomDV[i] = CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", arrNhomDonVi[i], "sGhiChu").ToString();
                            }
                        }

                        for (int i = 1; i <= TenNhomDV.Length; i++)
                        {
                            fr.SetValue("NhomDonVi" + i, TenNhomDV[i - 1]);
                        }
                    }
                    else
                    {
                        if (arrNhomDonVi.Length < 4 + 7 * (Convert.ToInt16(ToSo) - 1))
                        {
                            int a1 = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                            for (int i = 0; i < a1; i++)
                            {
                                NhomDonVi += ","+Guid.Empty.ToString();
                            }
                            arrNhomDonVi = NhomDonVi.Split(',');
                        }
                        TenNhomDV = new String[7];
                        int x = 1;
                        for (int i = 4 + 7 * ((Convert.ToInt16(ToSo) - 2)); i < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)); i++)
                        {
                            if (arrNhomDonVi[i] != null && arrNhomDonVi[i] != ""+Guid.Empty.ToString()+"" && arrNhomDonVi[i] != Guid.Empty.ToString() && arrNhomDonVi[i] != "")
                            {
                                TenNhomDV[x - 1] = CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", arrNhomDonVi[i], "sGhiChu").ToString();
                                x++;
                            }
                        }

                        for (int i = 1; i <= TenNhomDV.Length; i++)
                        {
                            fr.SetValue("NhomDonVi" + i, TenNhomDV[i - 1]);
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
                    if (arrNhomDonVi.Length < SoCotTrang1)
                    {
                        int a1 = SoCotTrang1 - arrNhomDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            NhomDonVi += ","+Guid.Empty.ToString();
                        }
                    }
                    arrNhomDonVi = NhomDonVi.Split(',');
                    TenNhomDV = new String[SoCotTrang1];
                    for (int i = 0; i < SoCotTrang1; i++)
                    {
                        if (arrNhomDonVi[i] != null && arrNhomDonVi[i] != ""+Guid.Empty.ToString()+"" && arrNhomDonVi[i] != Guid.Empty.ToString() && arrNhomDonVi[i] != "")
                        {
                            TenNhomDV[i] = CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", arrNhomDonVi[i], "sGhiChu").ToString();
                        }
                    }

                    for (int i = 1; i <= TenNhomDV.Length; i++)
                    {
                        fr.SetValue("NhomDonVi" + i, TenNhomDV[i - 1]);
                    }
                }
                else
                {
                    if (arrNhomDonVi.Length < SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1))
                    {
                        int a1 = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            NhomDonVi += ","+Guid.Empty.ToString();
                        }
                        arrNhomDonVi = NhomDonVi.Split(',');
                    }
                    TenNhomDV = new String[SoCotTrang2];
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 2)); i < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)); i++)
                    {
                        if (arrNhomDonVi[i] != null && arrNhomDonVi[i] != ""+Guid.Empty.ToString()+"" && arrNhomDonVi[i] != Guid.Empty.ToString() && arrNhomDonVi[i] != "")
                        {
                            TenNhomDV[x - 1] = CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", arrNhomDonVi[i], "sGhiChu").ToString();
                            x++;
                        }
                    }

                    for (int i = 1; i <= TenNhomDV.Length; i++)
                    {
                        fr.SetValue("NhomDonVi" + i, TenNhomDV[i - 1]);
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
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptCT_TongHopCT7");
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
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("TenTieuDe", "TỔNG HỢP CHỈ TIÊU THEO NHÓM ĐƠN VỊ");
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {

            DataTable data = TongHopChiTieuTheoNhom(MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,KhoGiay,LuyKe,ToSo);
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
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public static DataTable TongHopChiTieuTheoNhom(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
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
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                DK_Duyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String DKDotPhanBo = "", MaDotDauNam = "";
            DataTable dtNhomDV = DS_NhomDonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, LuyKe);
            String iID_MaNhomDonVi = "";
            for (int i = 0; i < dtNhomDV.Rows.Count; i++)
            {
                iID_MaNhomDonVi += dtNhomDV.Rows[i]["iID_MaNhomDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaNhomDonVi))
            {
                iID_MaNhomDonVi = iID_MaNhomDonVi.Substring(0, iID_MaNhomDonVi.Length - 1);
            }
            String[] arrNhomDonVi = iID_MaNhomDonVi.Split(',');
            String DKCASEDonVi = "";
            for (int i = 0; i < arrNhomDonVi.Length; i++)
            {
                DKCASEDonVi += "iID_MaNhomDonVi=@iID_MaNhomDonVia" + i;
                if (i < arrNhomDonVi.Length - 1)
                    DKCASEDonVi += " OR ";
            }
            if (!String.IsNullOrEmpty(DKCASEDonVi))
            {
                DKCASEDonVi = " AND (" + DKCASEDonVi + ")";
            }
            dtNhomDV.Dispose();

            String DKNhomDV = "";
            String DKSUMNhomDV = "", DKHAVINGNhomDV = "";

            #region Mẫu lũy kế
            if (dtDotPhanBo.Rows.Count > 0)
            {
                MaDotDauNam = dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"].ToString();
                if (iID_MaDotPhanBo == MaDotDauNam)
                {
                    DKDotPhanBo = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
                }
                else
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

                        if (arrNhomDonVi.Length < 8)
                        {
                            int a = 8 - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += ","+Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        for (int i = 1; i <= 8; i++)
                        {
                            DKSUMNhomDV += ",SUM(NhomDonVi" + i + ") AS NhomDonVi" + i;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + i + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + i + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + i + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                    else
                    {
                        if (arrNhomDonVi.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += ","+Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            DKSUMNhomDV += ",SUM(NhomDonVi" + x + ") AS NhomDonVi" + x;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + x + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + x + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + x + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                }
                //Giay A4
                else
                {
                    if (ToSo == "1")
                    {

                        if (arrNhomDonVi.Length < 4)
                        {
                            int a = 4 - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += ","+Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        for (int i = 1; i <= 4; i++)
                        {
                            //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMNhomDV += ",SUM(NhomDonVi" + i + ") AS NhomDonVi" + i;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + i + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + i + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + i + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                    else
                    {
                        if (arrNhomDonVi.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += ","+Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMNhomDV += ",SUM(NhomDonVi" + x + ") AS NhomDonVi" + x;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + x + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + x + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + x + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                }
                String SQLLuyKe = String.Format(@"SELECT  NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                                ,TongCong=SUM(DauNam)+SUM(LuyKe)
                                                ,SUM(DauNam) as DauNam
                                                ,SUM(LuyKe) as LuyKe
                                                {2}
                                                FROM
                                                (
                                                SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa
                                                ,DauNam=CASE WHEN iID_MaDotPhanBo=@MaDotDauNam THEN SUM({1}) ELSE 0 END
                                                ,LuyKe=CASE WHEN ({7}) THEN SUM({1}) ELSE 0 END
                                                {0}
                                                FROM PB_PhanBoChiTiet as PB
                                                INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON PB.iID_MaDonVi=DV.iID_MaDonVi
                                                WHERE PB.iTrangThai=1 {5} {4}
                                                AND ({6}) AND sNG<>''
                                                GROUP BY SUBSTRING(sLNS,1,1) ,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa,iID_MaDotPhanBo,iID_MaNhomDonVi,PB.iTrangThai
                                                HAVING SUM({1})<>0
                                                ) as A
                                                GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                                HAVING SUM(DauNam)!=0 OR SUM(LuyKe)!=0 {3} ", DKNhomDV, TruongTien, DKSUMNhomDV, DKHAVINGNhomDV, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS, DKDotPhanBo);
                SqlCommand cmdLuyKe = new SqlCommand(SQLLuyKe);
                cmdLuyKe.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmdLuyKe.Parameters.AddWithValue("@MaDotDauNam", MaDotDauNam);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmdLuyKe.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                if (dtDotPhanBo.Rows.Count > 0)
                {
                    for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 1; j <= i; j++)
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
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + i, arrNhomDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + x, arrNhomDonVi[i]);
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
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + i, arrNhomDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + x, arrNhomDonVi[i]);
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
                    if (arrNhomDonVi.Length < SoCotTrang1)
                    {
                        int a = SoCotTrang1 - arrNhomDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaNhomDonVi += "," + Guid.Empty.ToString();
                        }
                        arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                    }
                    for (int i = 1; i <= SoCotTrang1; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMNhomDV += ",SUM(NhomDonVi" + i + ") AS NhomDonVi" + i;
                        DKHAVINGNhomDV += " OR SUM(NhomDonVi" + i + ")<>0 ";
                        DKNhomDV += " ,NhomDonVi" + i + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + i + ") THEN SUM({0}) ELSE 0 END";
                    }
                    DKNhomDV = string.Format(DKNhomDV, TruongTien);
                }
                else
                {
                    if (arrNhomDonVi.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaNhomDonVi += ","+Guid.Empty.ToString();
                        }
                        arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                    {
                        // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMNhomDV += ",SUM(NhomDonVi" + x + ") AS NhomDonVi" + x;
                        DKHAVINGNhomDV += " OR SUM(NhomDonVi" + x + ")<>0 ";
                        DKNhomDV += " ,NhomDonVi" + x + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                    }
                    DKNhomDV = string.Format(DKNhomDV, TruongTien);
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
                                     INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)  as DV ON PB.iID_MaDonVi=DV.iID_MaDonVi
                                    WHERE pB.iTrangThai=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo 
                                    AND ({6}) {4} {5} AND sNG<>''
                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,PB.sMoTa,iID_MaDotPhanBo,iID_MaNhomDonVi,PB.iTrangThai
                                    HAVING SUM({1})!=0
                                    ) as DATATABLE
                                    GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                    HAVING SUM(DotNay)!=0 {3}
                                    ORDER BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa", DKNhomDV, TruongTien, DKSUMNhomDV, DKHAVINGNhomDV, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS);
                SqlCommand  cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                if (ToSo == "1")
                {
                    for (int i = 1; i <= SoCotTrang1; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaNhomDonVi" + i, arrNhomDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaNhomDonVi" + x, arrNhomDonVi[i]);
                        x++;
                    }
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion
            return dt;
        }
       
        /// <summary>
        /// Dt Đến Kì Này
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public static DataTable dtDenKyNay(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe,String ToSo)
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
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                DK_Duyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String DKDotPhanBo = "";
            DataTable dtNhomDV = DS_NhomDonVi(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, LuyKe);
            String iID_MaNhomDonVi = "";
            for (int i = 0; i < dtNhomDV.Rows.Count; i++)
            {
                iID_MaNhomDonVi += dtNhomDV.Rows[i]["iID_MaNhomDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaNhomDonVi))
            {
                iID_MaNhomDonVi = iID_MaNhomDonVi.Substring(0, iID_MaNhomDonVi.Length - 1);
            }
            String[] arrNhomDonVi = iID_MaNhomDonVi.Split(',');
            String DKCASEDonVi = "";
            for (int i = 0; i < arrNhomDonVi.Length; i++)
            {
                DKCASEDonVi += "iID_MaNhomDonVi=@iID_MaNhomDonVia" + i;
                if (i < arrNhomDonVi.Length - 1)
                    DKCASEDonVi += " OR ";
            }
            if (!String.IsNullOrEmpty(DKCASEDonVi))
            {
                DKCASEDonVi = " AND (" + DKCASEDonVi + ")";
            }
            dtNhomDV.Dispose();

            String DKNhomDV = "";
            String DKSUMNhomDV = "", DKHAVINGNhomDV = "";

            if (dtDotPhanBo.Rows.Count > 0)
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

                        if (arrNhomDonVi.Length < 8)
                        {
                            int a = 8 - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += "," + Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        for (int i = 1; i <= 8; i++)
                        {
                            DKSUMNhomDV += ",SUM(NhomDonVi" + i + ") AS NhomDonVi" + i;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + i + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + i + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + i + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                    else
                    {
                        if (arrNhomDonVi.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += "," + Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            DKSUMNhomDV += ",SUM(NhomDonVi" + x + ") AS NhomDonVi" + x;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + x + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + x + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + x + " AND ({1})) THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                }
                //Giay A4
                else
                {
                    if (ToSo == "1")
                    {

                        if (arrNhomDonVi.Length < 4)
                        {
                            int a = 4 - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += "," + Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        for (int i = 1; i <= 4; i++)
                        {
                            //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMNhomDV += ",SUM(NhomDonVi" + i + ") AS NhomDonVi" + i;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + i + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + i + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + i + " AND ({1}))  THEN SUM({0}) ELSE 0 END";
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                    else
                    {
                        if (arrNhomDonVi.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                        {
                            int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrNhomDonVi.Length;
                            for (int i = 0; i < a; i++)
                            {
                                iID_MaNhomDonVi += "," + Guid.Empty.ToString();
                            }
                            arrNhomDonVi = iID_MaNhomDonVi.Split(',');
                        }
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                            DKSUMNhomDV += ",SUM(NhomDonVi" + x + ") AS NhomDonVi" + x;
                            DKHAVINGNhomDV += " OR SUM(NhomDonVi" + x + ")<>0 ";
                            DKNhomDV += " ,NhomDonVi" + x + "=CASE WHEN (iID_MaNhomDonVi=@iID_MaNhomDonVi" + x + " AND ({1}))  THEN SUM({0}) ELSE 0 END";
                            x++;
                        }
                        DKNhomDV = string.Format(DKNhomDV, TruongTien, DKDotPhanBo);
                    }
                }
                String SQLLuyKe = String.Format(@"SELECT SUM(a) as a
                                                    {2}
                                                    FROM
                                                    (
                                                    SELECT a=0
                                                    {0}
                                                    FROM PB_PhanBoChiTiet as PB
                                                    INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON PB.iID_MaDonVi=DV.iID_MaDonVi
                                                    WHERE PB.iTrangThai=1 {4} {5} AND ({7})
                                                    AND ({6}) 
                                                    GROUP BY iID_MaNhomDonVi,iID_MaDotPhanBo
                                                    HAVING SUM({1})<>0
                                                    ) as A
                                                    HAVING SUM(a)!=0 {3}", DKNhomDV, TruongTien, DKSUMNhomDV, DKHAVINGNhomDV, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS, DKDotPhanBo);
                SqlCommand cmdLuyKe = new SqlCommand(SQLLuyKe);
                cmdLuyKe.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
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
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + i, arrNhomDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + x, arrNhomDonVi[i]);
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
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + i, arrNhomDonVi[i - 1]);
                        }
                    }
                    else
                    {
                        int tg = Convert.ToInt16(ToSo) - 2;
                        int x = 1;
                        for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                        {
                            cmdLuyKe.Parameters.AddWithValue("@iID_MaNhomDonVi" + x, arrNhomDonVi[i]);
                            x++;
                        }
                    }
                }
                dt = Connection.GetDataTable(cmdLuyKe);
                cmdLuyKe.Dispose();
            }
            return dt;
        }
        /// <summary>
        /// Dt Đến Kì Này Lũy Kế
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
    
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
        /// <summary>
        /// Hàm thực hiện việc xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
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
        /// <summary>
        /// lấy ngày đợt phân bổ
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Danh sách các đợt
        /// </summary>
        /// <returns></returns>
        public static DataTable ListDot()
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT DISTINCT DPB.iID_MaDotPhanBo,CONVERT(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo
                                            FROM PB_PhanBoChiTiet as PBCT
                                            INNER JOIN PB_DotPhanBo as DPB ON PBCT.iID_MaDotPhanBo=DPB.iID_MaDotPhanBo
                                            WHERE PBCT.iTrangThai=1");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// Lấy nhóm đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_NhomDonVi()
        {
            DataTable dt = HamChung.Lay_dtDanhMuc("Nhomdonvi");
            return dt;
        }

        public static DataTable LoaiNganSach()
        {
            DataTable dt = null;
            String SQL = " SELECT distinct sLNS, sLNS+' - '+sTen as sTen FROM NS_LoaiNganSach WHERE sLNS!=''  ";
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        /// <summary>
        /// Lấy đợt phân bổ theo năm và loại ngân sách
        /// </summary>
        /// <param name="NamlamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable LayDotPhanBo(String MaND, String sLNS, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = " AND PB.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = string.Format(@"SELECT DPB.iID_MaDotPhanBo,CONVERT(nvarchar,dNgayDotPhanBo,103) as dNgayDotPhanBo
                                        FROM PB_PhanBoChiTiet as PB
                                        INNER JOIN PB_DotPhanBo as DPB ON PB.iID_MaDotPhanBo=DPB.iID_MaDotPhanBo
                                        WHERE PB.iTrangThai=1 AND sLNS=@sLNS {2} AND ({0})>0 {1}
                                        GROUP BY DPB.iID_MaDotPhanBo,dNgayDotPhanBo
                                        ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)", TruongTien, ReportModels.DieuKien_NganSach(MaND,"PB"),DK_Duyet);
           cmd.CommandText=SQL;
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        ///  Danh sách nhóm đơn vị
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public static DataTable DS_NhomDonVi(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien,String LuyKe)
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
            DataTable dtNhomDV = new DataTable();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String DKDotPhanBo = "";
            if (LuyKe == "on")
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

            String SQL = string.Format(@"SELECT iID_MaNhomDonVi,DC.sGhiChu as TenNhom
                                        FROM PB_PhanBoChiTiet as PB
                                        INNER JOIN  (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON DV.iID_MaDonVi=PB.iID_MaDonVi
                                        INNER JOIN DC_DanhMuc DC ON DV.iID_MaNhomDonVi=DC.iID_MaDanhMuc
                                        WHERE {3} AND ({4}) {1} {2}
                                        GROUP BY DC.sGhiChu,DV.iID_MaNhomDonVi
                                        HAVING SUM({0})!=0
                                        ORDER BY DC.sGhiChu", TruongTien, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKLNS,DKDotPhanBo);
            cmd.CommandText= SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (LuyKe == "on")
            {
                for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 0; j <= i; j++)
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
            dtNhomDV = Connection.GetDataTable(cmd);
            return dtNhomDV;
        }

        public class LNSdata
        {
            public string iID_MaDotPhanBo { get; set; }
            public string ToSo { get; set; }
        }

        /// <summary>
        /// onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="sLNS"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public JsonResult ds_DotPhanBo(String ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe, String ToSo)
        {
            return Json(obj_DotPhanBo(ParentID,MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,KhoGiay,LuyKe,ToSo), JsonRequestBehavior.AllowGet);
        }
        public LNSdata obj_DotPhanBo(String ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien,String KhoGiay,String LuyKe,String ToSo)
        {
            LNSdata _LNSdata = new LNSdata();
            #region Đợt phân bổ
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND, sLNS, iID_MaTrangThaiDuyet,TruongTien);
            SelectOptionList slPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _LNSdata.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 85%\" onchange=\"Chon()\"");
            dtDotPhanBo.Dispose();
            #endregion
            #region ToSo
            DataTable dtToSo = dtTo(MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,KhoGiay,LuyKe);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _LNSdata.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 85%\"");
            dtToSo.Dispose();
            #endregion
            return _LNSdata;
        }
        public static DataTable LayDSDotPhanBo2(String MaND, String sLNS, String iID_MaTrangThaiDuyet,String TruongTien)
        {
            SqlCommand cmd = new SqlCommand();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iTrangThai=1 AND ({0}) {1} {2} AND {3}>0) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo", DKLNS, ReportModels.DieuKien_NganSach(MaND), DK_Duyet,TruongTien);
            cmd.CommandText = SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotPhanBo"]=Guid.Empty.ToString();
                dr["dNgayDotPhanBo"] = "Không có dữ liệu";
                dt.Rows.InsertAt(dr,0);
            }
            cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        public static DataTable dtTo(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String LuyKe)
        {
            DataTable dtNhom = DS_NhomDonVi(MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,LuyKe);
            String iID_MaNhomDonVi = "";
            for (int i = 0; i < dtNhom.Rows.Count; i++)
            {
                iID_MaNhomDonVi += dtNhom.Rows[i]["iID_MaNhomDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaNhomDonVi))
            {
                iID_MaNhomDonVi = iID_MaNhomDonVi.Substring(0, iID_MaNhomDonVi.Length - 1);
            }

            String[] arrNhomDonVi = iID_MaNhomDonVi.Split(',');
            dtNhom.Dispose();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));
            DataRow dr = dt.NewRow();
            dr[0] = "Tờ 1";
            dr[1] = "1";
            dt.Rows.InsertAt(dr, 0);
            //Luy ke
            if (LuyKe == "on")
            {
                //giay a3
                if (KhoGiay == "1")
                {
                    int a = 2;
                    for (int i = 8; i < arrNhomDonVi.Length; i = i + 11)
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
                    for (int i = 4; i < arrNhomDonVi.Length; i = i + 7)
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
                for (int i = SoCotTrang1; i < arrNhomDonVi.Length; i = i + SoCotTrang2)
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
