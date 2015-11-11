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

namespace VIETTEL.Report_Controllers.NguoiCoCong
{
    public class rptNCC_TongHopTCKK_62Controller : Controller
    {

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A3_2.xls";

        private const String sFilePath_A4_1_RG = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A4_1_RG.xls";
        private const String sFilePath_A4_2_RG = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A4_2_RG.xls";
        private const String sFilePath_A3_1_RG = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A3_1_RG.xls";
        private const String sFilePath_A3_2_RG = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TongHopNganSach_57_A3_2_RG.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_TongHopTCKK_62.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// lấy các giá trị trên form khi thực hiện Action Submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {

            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String ThangQuy = "";
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "1")
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String RutGon = Convert.ToString(Request.Form[ParentID + "_RutGon"]);
            String Solieu = Convert.ToString(Request.Form[ParentID + "_Solieu"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
            ViewData["sLNS"] = sLNS;
            ViewData["ThangQuy"] = ThangQuy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["RutGon"] = RutGon;
            ViewData["Solieu"] = Solieu;
            ViewData["ToSo"] = ToSo;
            ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_TongHopTCKK_62.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="ThangQuy"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
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
            DataTable dtDonVi = DanhSach_DonVi(sLNS, Thang_Quy, MaND, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
            ThangQuy = "Quý  " + Thang_Quy + " năm " + iNamLamViec;
            String TT = "";
            if ("rTuChi" == "rTuChi")
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
            String TenLoaiThangQuy = "";
            if (LoaiThang_Quy == "1")
                TenLoaiThangQuy = "Quý " + Thang_Quy + " năm " + iNamLamViec;
            else
                TenLoaiThangQuy = "Tháng " + Thang_Quy + " năm " + iNamLamViec;
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String Cap1 = ReportModels.CauHinhTenDonViSuDung(2);
            String Cap2 = ReportModels.CauHinhTenDonViSuDung(3);
            LoadData(fr, MaND, sLNS, Thang_Quy, LoaiThang_Quy, iID_MaTrangThaiDuyet, RutGon, KhoGiay, Solieu, ToSo);
            fr = ReportModels.LayThongTinChuKy(fr, "rptNCC_TongHopTCKK_62");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("ThangQuy", ThangQuy);
            fr.SetValue("Cap2", Cap2);
            fr.SetValue("Cap1", Cap1);
            fr.SetValue("Ngay", NgayThangNam);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("TenTieuDe", "TỔNG HỢP NGÂN SÁCH NGƯỜI CÓ CÔNG");
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy mô tả loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra ngoài báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="ThangQuy"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String ThangQuy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
        {
            DataTable data = rptNCC_TongHopNCC_57(MaND, sLNS, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet, RutGon, KhoGiay, Solieu, ToSo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa", "sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtDenKyNay = DenKyNay(MaND, sLNS, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet, RutGon, KhoGiay, Solieu, ToSo);
            fr.AddTable("DenKyNay", dtDenKyNay);
            DataTable dtTrongKy = TrongKy(MaND, sLNS, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet, RutGon, KhoGiay, Solieu, ToSo);
            fr.AddTable("TrongKi", dtTrongKy);
            data.Dispose();
            dtTieuMuc.Dispose();

        }
        /// <summary>
        /// Hàm thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="ThangQuy"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String ThangQuy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
        {
            clsExcelResult clsResult = new clsExcelResult();
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet, KhoGiay, RutGon, Solieu, ToSo);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.type = "xls";
                clsResult.FileName = "rptNCC_TongHopNganSach_57.xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="ThangQuy"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String sLNS, String ThangQuy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
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

            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet, KhoGiay, RutGon, Solieu, ToSo);
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
        /// Hàm lấy danh sách đơn vị
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String sLNS, String ThangQuy, String MaND, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(ThangQuy))
            {
                ThangQuy = Guid.Empty.ToString();
            }

            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DK_ThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DK_ThangQuy = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_ThangQuy = "iThang_Quy IN (1,2,3,4,5,6)";
                        break;
                    case "3": DK_ThangQuy = "iThang_Quy IN (1,2,3,4,5,6,7,8,9)";
                        break;
                    case "4": DK_ThangQuy = "iThang_Quy IN (1,2,3,4,5,6,7,8,9,10,11,12)";
                        break;
                }
            }
            else
            {
                DK_ThangQuy = "iThang_Quy<=@ThangQuy";
            }
            String SQL = String.Format(@"SELECT NS_DonVi.iID_MaDonVi,NS_DonVi.sTen
                                        FROM NCC_ChungTuChiTiet
                                        INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=NCC_ChungTuChiTiet.iID_MaDonVi
                                        WHERE NCC_ChungTuChiTiet.sLNS=@sLNS 
                                        AND NCC_ChungTuChiTiet.iTrangThai=1 AND {2}
                                        AND iLoai=2 
                                        {1}  {0} AND rTuChi>0
                                        GROUP BY NS_DonVi.iID_MaDonVi,NS_DonVi.sTen
                                        ORDER BY  NS_DonVi.iID_MaDonVi,NS_DonVi.sTen", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DK_ThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="ThangQuy"></param>
        /// <returns></returns>
        public DataTable rptNCC_TongHopNCC_57(String MaND, String sLNS, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
        {
            DataTable dt = new DataTable();
            DataTable dtDonVi = DanhSach_DonVi(sLNS, Thang_Quy, MaND, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1")
                    DKRutGon = "iThang_Quy IN(1,2,3)";
                else if (Thang_Quy == "2")
                    DKRutGon = "iThang_Quy IN(4,5,6)";
                else if (Thang_Quy == "3")
                    DKRutGon = "iThang_Quy IN(7,8,9)";
                else
                    DKRutGon = "iThang_Quy IN(10,11,12)";
            }
            else
                DKRutGon = " iThang_Quy =" + Thang_Quy;
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
                        DKCASEDonVi += " ,DonVi" + i + "=CASE WHEN (iID_MaDonVi=@iID_MaDonVi" + i + " AND {1}) THEN SUM(rTuChi) ELSE 0 END";
                    }
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
                }
                String SQLRutGon = "";
                if (Solieu == "1")
                {
                    SQLRutGon = String.Format(@"SELECT  sM, sTM, sTTM, sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy {1}
                                            FROM(
                                            SELECT sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END                                          
                                            {0}
                                            FROM NCC_ChungTuChiTiet
                                            WHERE  iTrangThai=1 {6} AND 1=1  {3} {4} {5} AND iLoai=2 AND sNG<>''
                                            GROUP BY  sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM(rTuChi)<>0
                                            ) as a
                                            GROUP BY  sM, sTM, sTTM, sNG,sTNG,sMoTa
                                           -- HAVING SUM(CongTrongKy)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, "rTuChi", DKRutGon);
                }
                else
                {
                    SQLRutGon = String.Format(@"SELECT  sM, sTM, sTTM, sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy {1}
                                            FROM(
                                            SELECT sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END                                          
                                            {0}
                                            FROM NCC_ChungTuChiTiet
                                            WHERE  iTrangThai=1 {6} AND 1=1  {3} {4} {5} AND iLoai=2 AND sNG<>''
                                            GROUP BY  sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM(rTuChi)<>0
                                            ) as a
                                            GROUP BY sM, sTM, sTTM, sNG,sTNG,sMoTa
                                            HAVING SUM(CongTrongKy)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, "rTuChi", DKRutGon);
                }
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
                        DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                        DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                        DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                        DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
                    }
                }
                String SQLDayDu = "";
                if (Solieu == "1")
                {
                    SQLDayDu = String.Format(@"SELECT sM, sTM, sTTM, sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy,SUM(DenKyNay) as DenKyNay {1}
                                            FROM(
                                            SELECT  sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END
                                            ,DenKyNay=CASE WHEN iThang_Quy<=@ThangQuy THEN SUM({7}) else 0 END
                                            {0}
                                            FROM NCC_ChungTuChiTiet
                                            WHERE  1=1 {6}  AND iTrangThai=1  {3} {4} {5} AND iLoai=2 AND sNG<>''
                                            GROUP BY sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                            GROUP BY  sM, sTM, sTTM, sNG,sTNG,sMoTa
                                           -- HAVING SUM(CongTrongKy)<>0 OR SUM(DenKyNay)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, "rTuChi", DKRutGon);

                }
                else
                {
                    SQLDayDu = String.Format(@"SELECT sM, sTM, sTTM, sNG,sTNG,sMoTa,SUM(CongTrongKy) as CongTrongKy,SUM(DenKyNay) as DenKyNay {1}
                                            FROM(
                                            SELECT  sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            ,CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END
                                            ,DenKyNay=CASE WHEN iThang_Quy<=@ThangQuy THEN SUM({7}) else 0 END
                                            {0}
                                            FROM NCC_ChungTuChiTiet
                                            WHERE  1=1 {6}  AND iTrangThai=1  {3} {4} {5} AND iLoai=2 AND sNG<>''
                                            GROUP BY sM, sTM, sTTM, sNG,sTNG,sMoTa,iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                            GROUP BY  sM, sTM, sTTM, sNG,sTNG,sMoTa
                                           HAVING SUM(CongTrongKy)<>0 OR SUM(DenKyNay)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, "rTuChi", DKRutGon);
                }
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
                if (LoaiThang_Quy == "1")
                    cmdDayDu.Parameters.AddWithValue("@ThangQuy", Convert.ToString(Convert.ToInt32(Thang_Quy) * 3));
                else
                    cmdDayDu.Parameters.AddWithValue("@ThangQuy", Convert.ToString(Convert.ToInt32(Thang_Quy)));
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
                String SQLChiTieu = String.Format(@" SELECT sM,sTM,sTTM,sNG,sTNG,sMoTa, SUM({3}) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                  WHERE PB_PhanBoChiTiet.iTrangThai=1
                                                AND sNG<>''{4}  AND YEAR(dNgayDotPhanBo)=@NamLamViec 
                                                AND MONTH(dNgayDotPhanBo)<= @dNgay  AND PB_PhanBoChiTiet.iTrangThai=1 AND ({0})
                                                 {1} {2}
                                                 GROUP BY sM,sTM,sTTM,sNG,sTNG,sMoTa
                                                 HAVING SUM({3})<>0", DKDonViChiTieu, ReportModels.DieuKien_NganSachThuongXuyen(MaND), DKDuyet1, "rTuChi", DKLNS);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                if (LoaiThang_Quy == "1")
                    cmdChiTieu.Parameters.AddWithValue("@dNgay", Convert.ToString(Convert.ToInt32(Thang_Quy) * 3));
                else
                    cmdChiTieu.Parameters.AddWithValue("@dNgay", Convert.ToString(Convert.ToInt32(Thang_Quy)));

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
                String sCol = "sM,sTM,sTTM,sNG,sTNG,sMoTa,ChiTieu";
                String[] arrCol = sCol.Split(',');

                DataColumn col = dt.Columns.Add("ChiTieu", typeof(Decimal));
                col.SetOrdinal(5);

                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"sM='{0}' AND sTM='{1}'
                                                       AND sTTM='{2}' AND sNG='{3}'  AND sTNG='{4}'",
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["sTNG"]
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
                dv.Sort = "sM,sTM,sTTM,sNG,sTNG";
                dt = dv.ToTable();
                #endregion
            }
            #endregion
            return dt;
        }
        public DataTable DenKyNay(String MaND, String sLNS, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
        {
            DataTable dt = new DataTable();
            DataTable dtDonVi = DanhSach_DonVi(sLNS, Thang_Quy, MaND, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1")
                    DKRutGon = "iThang_Quy between 1 AND 3";
                else if (Thang_Quy == "2")
                    DKRutGon = "iThang_Quy between 1 AND 6";
                else if (Thang_Quy == "3")
                    DKRutGon = "iThang_Quy between 1 AND 9";
                else
                    DKRutGon = "iThang_Quy between 1 AND 12";
            }
            else
            {
                DKRutGon = "iThang_Quy<=" + Thang_Quy;
            }



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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
                }
            }
            String SQLDayDu = "";

            SQLDayDu = String.Format(@"SELECT SUM(CongTrongKy) as CongTrongKy,SUM(DenKyNay) as DenKyNay {1}
                                            FROM(
                                            SELECT  
                                            CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END
                                            ,DenKyNay=CASE WHEN iThang_Quy<=@ThangQuy THEN SUM({7}) else 0 END
                                            {0}
                                            FROM NCC_ChungTuChiTiet
                                            WHERE  1=1 {6}  AND iTrangThai=1  {3} {4} {5} AND iLoai=2 AND sNG<>''
                                            GROUP BY iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                           
                                           -- HAVING SUM(CongTrongKy)<>0 OR SUM(DenKyNay)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, "rTuChi", DKRutGon);


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
            if (LoaiThang_Quy == "1")
                cmdDayDu.Parameters.AddWithValue("@ThangQuy", Convert.ToString(Convert.ToInt32(Thang_Quy) * 3));
            else
                cmdDayDu.Parameters.AddWithValue("@ThangQuy", Convert.ToString(Convert.ToInt32(Thang_Quy)));
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



            return dt;
        }
        public DataTable TrongKy(String MaND, String sLNS, String Thang_Quy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String KhoGiay, String RutGon, String Solieu, String ToSo)
        {
            DataTable dt = new DataTable();
            DataTable dtDonVi = DanhSach_DonVi(sLNS, Thang_Quy, MaND, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
            if (LoaiThang_Quy == "1")
            {
                if (Thang_Quy == "1")
                    DKRutGon = "iThang_Quy between 1 AND 3";
                else if (Thang_Quy == "2")
                    DKRutGon = "iThang_Quy between 4 AND 6";
                else if (Thang_Quy == "3")
                    DKRutGon = "iThang_Quy between 7 AND 9";
                else
                    DKRutGon = "iThang_Quy between 10 AND 12";
            }
            else
            {
                DKRutGon = "iThang_Quy=" + Thang_Quy;
            }



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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
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
                    DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", DKRutGon);
                }
            }
            String SQLDayDu = "";

            SQLDayDu = String.Format(@"SELECT SUM(CongTrongKy) as CongTrongKy,SUM(DenKyNay) as DenKyNay {1}
                                            FROM(
                                            SELECT  
                                            CongTrongKy=CASE WHEN {8} THEN SUM({7}) else 0 END
                                            ,DenKyNay=CASE WHEN iThang_Quy<=@ThangQuy THEN SUM({7}) else 0 END
                                            {0}
                                            FROM NCC_ChungTuChiTiet
                                            WHERE  1=1 {6}  AND iTrangThai=1  {3} {4} {5} AND iLoai=2 AND sNG<>''
                                            GROUP BY iID_MaDonVi,iThang_Quy
                                            HAVING SUM({7})<>0
                                            ) as a
                                           
                                           -- HAVING SUM(CongTrongKy)<>0 OR SUM(DenKyNay)<>0 {2}", DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDonVi, DKLNS, "rTuChi", DKRutGon);


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
            if (LoaiThang_Quy == "1")
                cmdDayDu.Parameters.AddWithValue("@ThangQuy", Convert.ToString(Convert.ToInt32(Thang_Quy) * 3));
            else
                cmdDayDu.Parameters.AddWithValue("@ThangQuy", Convert.ToString(Convert.ToInt32(Thang_Quy)));
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



            return dt;
        }
        public DataTable dtTo(String sLNS, String ThangQuy, String MaND, String LoaiThang_Quy, String iID_MaTrangThaiDuyet, String RutGon, String KhoGiay)
        {
            DataTable dtDonVi = DanhSach_DonVi(sLNS, ThangQuy, MaND, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
        public JsonResult DS_To(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS)
        {
            return Json(obj_To(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, RutGon, MaND, KhoGiay, ToSo, sLNS), JsonRequestBehavior.AllowGet);
        }
        public String obj_To(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String RutGon, String MaND, String KhoGiay, String ToSo, String sLNS)
        {

            DataTable dtToSo = dtTo(sLNS, Thang_Quy, MaND, LoaiThang_Quy, iID_MaTrangThaiDuyet, RutGon, KhoGiay);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            String s = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 50%\"");
            return s;
        }
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
