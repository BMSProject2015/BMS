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
    public class rptPhanBo_12Controller : Controller
    {
        //
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1_RG = "/Report_ExcelFrom/PhanBo/rptTH_12_A4_1_RG.xls";
        private const String sFilePath_A4_2_RG = "/Report_ExcelFrom/PhanBo/rptTH_12_A4_2_RG.xls";
        private const String sFilePath_A3_1_RG = "/Report_ExcelFrom/PhanBo/rptTH_12_A3_1_RG.xls";
        private const String sFilePath_A3_2_RG = "/Report_ExcelFrom/PhanBo/rptTH_12_A3_2_RG.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_12.aspx";
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
        public ActionResult EditSubmit(String ParentID,String ToDaXem)
        {
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
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
            if (!DaCo)
            {
                if (ToDaXem == "") ToDaXem = ToSo;
                else ToDaXem += "," + ToSo;
            }
            ViewData["ToDaXem"] = ToDaXem;
            ViewData["sLNS"] = sLNS;
            ViewData["TruongTien"] = TruongTien;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["ToSo"] = ToSo;
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_12.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm Xem Báo Cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";

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

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
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
        /// Hàm xuất ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";

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

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);

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
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            FlexCelReport fr = new FlexCelReport();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtDotPhanBo = DanhSach_DotPB(MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien);
            String[] Dot;
            String iID_MaDotPhanBo = "";
            for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
            {
                iID_MaDotPhanBo += dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = iID_MaDotPhanBo.Substring(0, iID_MaDotPhanBo.Length - 1);
            }
            String DotPhanBo = iID_MaDotPhanBo;
            String[] arrDotPhanBo = iID_MaDotPhanBo.Split(',');
            dtDotPhanBo.Dispose();

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
                if (arrDotPhanBo.Length < SoCotTrang1)
                {
                    int a1 = SoCotTrang1 - arrDotPhanBo.Length;
                    for (int i = 0; i < a1; i++)
                    {
                        DotPhanBo += "," + Guid.Empty.ToString();
                    }
                }
                arrDotPhanBo = DotPhanBo.Split(',');
                Dot = new String[SoCotTrang1];
                for (int i = 0; i < SoCotTrang1; i++)
                {
                    if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != Guid.Empty.ToString() && arrDotPhanBo[i] != "")
                    {
                        Dot[i] = Convert.ToString(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"]);
                    }
                }

                for (int i = 1; i <= Dot.Length; i++)
                {
                    fr.SetValue("Dot" + i, Dot[i - 1]);
                }
            }
            else
            {
                if (arrDotPhanBo.Length < SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1))
                {
                    int a1 = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                    for (int i = 0; i < a1; i++)
                    {
                        DotPhanBo += "," + Guid.Empty.ToString();
                    }
                    arrDotPhanBo = DotPhanBo.Split(',');
                }
                Dot = new String[SoCotTrang2];
                int x = 1;
                for (int i = SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 2)); i < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)); i++)
                {
                    if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != Guid.Empty.ToString() && arrDotPhanBo[i] != "")
                    {
                        Dot[x - 1] = Convert.ToString(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"]);
                        x++;
                    }
                }

                for (int i = 1; i <= Dot.Length; i++)
                {
                    fr.SetValue("Dot" + i, Dot[i - 1]);
                }
            }

            String TenTruongTien = "";
            switch (TruongTien)
            {
                case "rTuChi": TenTruongTien = " - TỰ CHI";
                    break;
                case "rHienVat": TenTruongTien = " - HIỆN VẬT";
                    break;

            }

            String TenDv = "";
            DataTable Ten = TenDonVi(iID_MaDonVi);
            if (Ten.Rows.Count > 0)
            {
                TenDv = Ten.Rows[0][0].ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            LoadData(fr, MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_12");
            fr.SetValue("TruongTien", TenTruongTien);
            fr.SetValue("NgayThangNam", NgayThang);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("TenTieuDe", "TỔNG HỢP CHỈ TIÊU NGÂN SÁCH QUỐC PHÒNG");
            fr.SetValue("Thang", "ĐẾN THÁNG " + DateTime.Now.Month + "  NĂM " + ReportModels.LayNamLamViec(MaND));
            fr.SetValue("TenDV", TenDv);
            fr.Run(Result);
            return Result;


        }
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {

            DataTable data = rptPhanBo_12(MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            //if (LuyKe == "on")
            //{
            //    DataTable data_LK = dtDenKyNay(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, LuyKe, ToSo);
            //    if (data_LK.Rows.Count == 0)
            //    {
            //        DataRow dr = data_LK.NewRow();
            //        data_LK.Rows.InsertAt(dr, 0);
            //    }
            //    fr.AddTable("LuyKe", data_LK);
            //    data_LK.Dispose();
            //}
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
        /// Danh sách đơn vị đổ vào dropdowlist
        /// </summary>
        /// <returns></returns>
        public static DataTable dtDonVi(String MaND, String sLNS, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
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

            String SQL = String.Format(@" SELECT DV.iID_MaDonVi,DV.sTen
                                            FROM PB_PhanBoChiTiet as PB
                                            INNER JOIN NS_DonVi as DV ON PB.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE PB.iTrangThai=1 {2} AND {0} {1}
                                            GROUP BY DV.iID_MaDonVi,sTen
                                            ORDER BY DV.iID_MaDonVi", DK, DK_Duyet, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            for (int i = 0; i < ArrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, ArrLNS[i]);
            }

            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public DataTable rptPhanBo_12(String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
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
            DataTable dtDotPhanBo = DanhSach_DotPB(MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien);
            String iID_MaDotPhanBo = "";
            String DKDotPhanBo = "";
            if (dtDotPhanBo.Rows.Count > 0)
            {
                for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                {
                    DKDotPhanBo += "PB.iID_MaDotPhanBo=@iID_MaDotPhanBoa" + i;
                    if (i < dtDotPhanBo.Rows.Count - 1)
                        DKDotPhanBo += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaDotPhanBoa" + i, dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString());
                }
            }
            
            for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
            {
                iID_MaDotPhanBo += dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = iID_MaDotPhanBo.Substring(0, iID_MaDotPhanBo.Length - 1);
            }
            String[] arrDotPhanBo = iID_MaDotPhanBo.Split(',');
            String DKDdot = "";
            String DKLNS = "";
            String DKSUMDOT = "", DKHAVINGDOT = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }


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
                if (arrDotPhanBo.Length < SoCotTrang1)
                {
                    int a = SoCotTrang1 - arrDotPhanBo.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDotPhanBo += "," + Guid.Empty.ToString();
                    }
                    arrDotPhanBo = iID_MaDotPhanBo.Split(',');
                }
                for (int i = 1; i <= SoCotTrang1; i++)
                {
                    //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                    DKSUMDOT += ",SUM(Dot" + i + ") AS Dot" + i;
                    DKHAVINGDOT += " OR SUM(Dot" + i + ")<>0 ";
                    DKDdot += " ,Dot" + i + "=CASE WHEN (PB.iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                    cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                }
                DKDdot = string.Format(DKDdot, TruongTien);
            }
            else
            {
                if (arrDotPhanBo.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(ToSo) - 1)))
                {
                    int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDotPhanBo += "," + Guid.Empty.ToString();
                    }
                    arrDotPhanBo = iID_MaDotPhanBo.Split(',');
                }
                int tg = Convert.ToInt16(ToSo) - 2;
                int x = 1;
                for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                {
                    // iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                    DKSUMDOT += ",SUM(Dot" + x + ") AS Dot" + x;
                    DKHAVINGDOT += " OR SUM(Dot" + x + ")<>0 ";
                    DKDdot += " ,Dot" + x + "=CASE WHEN (PB.iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                    cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + x, arrDotPhanBo[i]);
                    x++;
                }
                DKDdot = string.Format(DKDdot, TruongTien);
            }

            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        ,SUM(TongChiTieu) as TongChiTieu
                                        {3}
                                        FROM
                                        (
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        ,TongChiTieu=CASE WHEN ({7})  THEN SUM({2}) ELSE 0 END
                                        {1}
                                        FROM PB_PhanBoChiTiet AS PB
                                        INNER JOIN PB_DotPhanBo  AS DPB ON PB.iID_MaDotPhanBo=DPB.iID_MaDotPhanBo
                                        WHERE PB.iTrangThai=1 {6} {5} AND  CONVERT(nvarchar,MONTH(dNgayDotPhanBo),103)<= DATEPART(Month,GETDATE())
                                        AND ({0}) AND PB.iID_MaDonVi=@iID_MaDonVi AND sNG<>''
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,PB.iID_MaDotPhanBo,dNgayDotPhanBo
                                        HAVING SUM({2})!=0
                                        ) AS BANGTAM
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        HAVING SUM(TongChiTieu)!=0 {4}", DKLNS, DKDdot, TruongTien, DKSUMDOT, DKHAVINGDOT, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), DKDotPhanBo);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy tên đơn vị theo mã đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Hàm lấy danh sách đợt phân bổ
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable DanhSach_DotPB(String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien)
        {
            DataTable dt = new DataTable();
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
            String[] arrsLNS = sLNS.Split(',');
            String DK = "";
            for (int i = 0; i < arrsLNS.Length; i++)
            {
                DK += "sLNS=@sLNS" + i;
                if (i < arrsLNS.Length - 1)
                {
                    DK += " OR ";
                }
            }
            String SQL = String.Format(@" SELECT  PB_DotPhanBo.iID_MaDotPhanBo,CONVERT(varchar(10),dNgayDotPhanBo,103) as dNgayDotPhanBo  
                                            FROM PB_PhanBoChiTiet as PB
                                            INNER JOIN PB_DotPhanBo ON PB.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                            WHERE PB.iID_MaDonVi=@iID_MaDonVi {3}
                                            AND CONVERT(nvarchar,MONTH(dNgayDotPhanBo),103)<= DATEPART(Month,GETDATE())
                                            AND ({0}) {2}
                                            GROUP BY PB_DotPhanBo.iID_MaDotPhanBo,dNgayDotPhanBo
                                            HAVING SUM({1})!=0
                                            ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)", DK, TruongTien, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            for (int i = 0; i < arrsLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrsLNS[i]);
            }

            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotPhanBo"] = "" + Guid.Empty.ToString() + "";
                dr["dNgayDotPhanBo"] = "Không có dữ liệu";
                dt.Rows.InsertAt(dr, 0);
            }
            cmd.Dispose();
            return dt;
        }

        public static String MaPhongBanCuaMaND(String MaND)
        {
            String vR = "";
            String SQL = "SELECT iID_MaPhongBan FROM NS_NguoiDung_PhongBan WHERE sMaNguoiDung=@sMaNguoiDung";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            vR = Connection.GetValueString(cmd, Guid.Empty.ToString());
            cmd.Dispose();
            return vR;
        }




        public static DataTable DanhSachDonVi(String MaND, String sLNS, String iID_MaTrangThaiDuyet, String TruongTien)
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
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.iID_MaDonVi+'-'+DV.sTen as TenHT
                                        FROM PB_PhanBoChiTiet as PB
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON PB.iID_MaDonVi=DV.iID_MaDonVi
                                        INNER JOIN PB_DotPhanBo AS DPB ON PB.iID_MaDotPhanBo=DPB.iID_MaDotPhanBo
                                        WHERE PB.iTrangThai=1 {2} AND ({0}) {1}
                                        AND CONVERT(nvarchar,MONTH(dNgayDotPhanBo),103)<= DATEPART(Month,GETDATE())
                                        GROUP BY DV.iID_MaDonVi,sTen
                                        HAVING SUM({3})!=0
                                        ORDER BY DV.iID_MaDonVi", DKLNS, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "PB"), TruongTien);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDonVi"] = "" + Guid.Empty.ToString() + "";
                dr["TenHT"] = "Không có dữ liệu";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }

        public class LNSdata
        {
            public string iID_MaDonVi { get; set; }
            public string ToSo { get; set; }
        }

        public JsonResult ds_DonVi(String ParentID, String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            return Json(obj_DonVi(ParentID, MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo), JsonRequestBehavior.AllowGet);
        }

        public LNSdata obj_DonVi(String ParentID, String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            LNSdata _LNSdata = new LNSdata();

            #region Danh sách đơn vị
            DataTable dtDonVi = DanhSachDonVi(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            _LNSdata.iID_MaDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"onchange=\"ChonNLV()\"");
            #endregion
            #region ToSo
            DataTable dtToSo = dtTo(MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, KhoGiay);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _LNSdata.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 85%\"");
            dtToSo.Dispose();
            #endregion
            return _LNSdata;
        }
        public static DataTable dtTo(String MaND, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay)
        {
            DataTable dtDotPhanBo = DanhSach_DotPB(MaND, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien);
            String iID_MaDotPhanBo = "";
            for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
            {
                iID_MaDotPhanBo += dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDotPhanBo = iID_MaDotPhanBo.Substring(0, iID_MaDotPhanBo.Length - 1);
            }

            String[] arrDotPhanBo = iID_MaDotPhanBo.Split(',');
            dtDotPhanBo.Dispose();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));
            DataRow dr = dt.NewRow();
            dr[0] = "Tờ 1";
            dr[1] = "1";
            dt.Rows.InsertAt(dr, 0);
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
            for (int i = SoCotTrang1; i < arrDotPhanBo.Length; i = i + SoCotTrang2)
            {
                DataRow dr1 = dt.NewRow();
                dt.Rows.Add(dr1);
                dr1[0] = "Tờ " + a;
                dr1[1] = a;
                a++;

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
