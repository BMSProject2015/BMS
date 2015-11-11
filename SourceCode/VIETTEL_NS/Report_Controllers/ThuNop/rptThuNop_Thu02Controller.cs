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

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNop_Thu02Controller : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNop_Thu02.xls";
        private const String sFilePath_TO2 = "/Report_ExcelFrom/ThuNop/rptThuNop_Thu02_To2.xls";
        private const String sFilePath_B = "/Report_ExcelFrom/ThuNop/rptThuNop_Thu02_B.xls";
        private const String sFilePath_B_TO2 = "/Report_ExcelFrom/ThuNop/rptThuNop_Thu02_B_To2.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_Thu02.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable rptThuNop_Thu02(String MaND,String iID_MaPhongBan)
        {
            String DK = "", DKDonVi = "", DKPhongBan = "", DKHAVING = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dtThongTinBaoCao = ThuNopModels.getThongTinCotBaoCao("1");
            int SoCot = 23;
            for (int i = 1; i <= SoCot; i++)
            {
                DK += ",COT" + i + "=SUM(CASE WHEN (sNG IN (" + i + ")  AND bThoaiThu=0) THEN (rNopNSQP) WHEN (sNG IN (" + i + ")  AND bThoaiThu=1) THEN rNopNSQP*(-1)  ELSE 0 END)";
                DKHAVING += " SUM(CASE WHEN (sNG IN (" + i + ")  AND bThoaiThu=0) THEN (rNopNSQP) WHEN (sNG IN (" + i + ")  AND bThoaiThu=1) THEN rNopNSQP*(-1)  ELSE 0 END) <>0 OR ";
            }
            //NS nhà nước
            DK += ",COT" + 24 + "=SUM(CASE WHEN  bThoaiThu=0 THEN (rNopThueTNDN+rNopNSNN) WHEN  bThoaiThu=1 THEN (rNopThueTNDN+rNopNSNN)*(-1) ELSE 0 END)";
            DK += ",COT" + 25 + "=SUM(CASE WHEN  bThoaiThu=0 THEN (rNopNSNNQuaBQP) WHEN  bThoaiThu=1 THEN (rNopNSNNQuaBQP)*(-1)  ELSE 0 END)";
            DK += ",COT" + 26 + "=SUM(CASE WHEN  bThoaiThu=0 THEN (rNopNSNNKhac) WHEN  bThoaiThu=1 THEN (rNopNSNNKhac)*(-1) ELSE 0 END)";
            DKHAVING += " SUM(CASE WHEN  bThoaiThu=0 THEN (rNopThueTNDN+rNopNSNN) WHEN bThoaiThu=1 THEN (rNopThueTNDN+rNopNSNN)*(-1) ELSE 0 END) <>0 OR ";
            DKHAVING += " SUM(CASE WHEN  bThoaiThu=0 THEN (rNopNSNNQuaBQP) WHEN  bThoaiThu=1 THEN (rNopNSNNQuaBQP)*(-1)  ELSE 0 END  ) <>0 OR ";
            DKHAVING += " SUM(CASE WHEN  bThoaiThu=0 THEN (rNopNSNNKhac) WHEN  bThoaiThu=1 THEN (rNopNSNNKhac)*(-1) ELSE 0 END) <>0";
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String SQL =
                String.Format(@"SELECT iID_MaPhongBan,iID_MaDonVi,sTenDonVi 
                                {0}
                                 FROM TN_ChungTuChiTiet
                                 WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec {1} {2}
                                 GROUP BY iID_MaPhongBan,iID_MaDonVi,sTenDonVi
                                    HAVING {3}
                                    ", DK,DKDonVi,DKPhongBan,DKHAVING);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            dt.Columns.Add("sGhiChu",typeof (String));

            SQL = String.Format(@"SELECT iID_MaPhongBan,iID_MaDonVi,sGhiChu
                                    FROM TN_ChungTuChiTiet
                                    WHERE  iTrangThai=1 AND sGhiChu is not null AND iNamLamViec=@iNamLamViec {0} {1}
                                    ORDER BY iID_MaPhongBan,iID_MaDonVi",DKDonVi,DKPhongBan);
            cmd.CommandText = SQL;
            DataTable dtGhiChu = Connection.GetDataTable(cmd);


            SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,iID_MaDonVi,sGhiChu=''
                                    FROM TN_ChungTuChiTiet
                                    WHERE  iTrangThai=1 AND sGhiChu is not null AND iNamLamViec=@iNamLamViec {0} {1}
                                    ORDER BY iID_MaPhongBan,iID_MaDonVi", DKDonVi, DKPhongBan);
            cmd.CommandText = SQL;
            DataTable dtGhiChuDINTINCT = Connection.GetDataTable(cmd);

            for (int i = 0; i < dtGhiChuDINTINCT.Rows.Count; i++)
            {
                String tg = "";
                String S = "iID_MaPhongBan='" + dtGhiChuDINTINCT.Rows[i]["iID_MaPhongBan"] + "' AND iID_MaDonVi='" +
                           dtGhiChuDINTINCT.Rows[i]["iID_MaDonVi"] + "'";
                DataRow[] dr = dtGhiChu.Select(S);
                if(dr.Length>0)
                {
                    for (int j = 0; j < dr.Length; j++)
                    {
                        if (j != 0 && tg!="")
                            tg += ";";
                        tg +=dr[j]["sGhiChu"];
                    }
                    dtGhiChuDINTINCT.Rows[i]["sGhiChu"] = tg;
                }
                //lay row dt
                DataRow[] dr1 = dt.Select(S);
                if (dr1.Length == 1)
                    dr1[0]["sGhiChu"] = tg;

            }
            return dt;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String iLoaiBaoCao = Request.Form[ParentID + "_iLoaiBaoCao"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iLoaiBaoCao"] = iLoaiBaoCao;
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_Thu02.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND,String iID_MaPhongBan,String iLoaiBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_Thu02");
            LoadData(fr, MaND, iID_MaPhongBan, iLoaiBaoCao);
            DataTable dtThongTinBaoCao = ThuNopModels.getThongTinCotBaoCao("1");
            for (int i = 0; i < 12; i++)
            {
                fr.SetValue("sTen" + i, dtThongTinBaoCao.Rows[i]["sTen"]);
            }
            String TenPB = "";
            if(iID_MaPhongBan!="-1")
                TenPB = " B - " + iID_MaPhongBan;
            String Nam = ReportModels.LayNamLamViec(MaND);
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("TenPB", TenPB);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan, String iLoaiBaoCao)
        {
            DataTable data = rptThuNop_Thu02(MaND, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtPhongBan = HamChung.SelectDistinct("PhongBan", data, "iID_MaPhongBan", "iID_MaPhongBan");
            fr.AddTable("PhongBan", dtPhongBan);
            dtPhongBan.Dispose();
            data.Dispose();
        }

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan, String iLoaiBaoCao)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iID_MaPhongBan == "-1")
            {
                if (iLoaiBaoCao == "1")
                    sDuongDan = sFilePath;
                else sDuongDan = sFilePath_TO2;
            }
            else
            {
                if (iLoaiBaoCao == "1")
                    sDuongDan = sFilePath_B;
                else sDuongDan = sFilePath_B_TO2;
            }

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iLoaiBaoCao);
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

        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan, String iLoaiBaoCao)
        {
            String sDuongDan = "";
            if (iID_MaPhongBan == "-1")
            {
                if (iLoaiBaoCao == "1")
                    sDuongDan = sFilePath;
                else sDuongDan = sFilePath_TO2;
            }
            else
            {
                if (iLoaiBaoCao == "1")
                    sDuongDan = sFilePath_B;
                else sDuongDan = sFilePath_B_TO2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iLoaiBaoCao);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThuNop_DTNS_Na.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

    }
}

