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
    public class rptThuNop_KeHoachNganSachController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNop_KeHoachNganSach.xls";
        private const String sFilePath_B = "/Report_ExcelFrom/ThuNop/rptThuNop_KeHoachNganSach_TongHop.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_KeHoachNganSach.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable rptThuNop_KeHoachNganSach(String MaND, String iID_MaPhongBan, String iThang, String bTrongThang)
        {
            String DK = "", DKDonVi = "", DKPhongBan = "", DKHAVING = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dtThongTinBaoCao = ThuNopModels.getThongTinCotBaoCao("2");
            int SoCot = 5;
            int iThang_So = 0;
            iThang_So = Convert.ToInt32(iThang) - 12;
            int iNamSau = Convert.ToInt32(ReportModels.LayNamLamViec(MaND)) + 1;
            if (Convert.ToInt32(iThang) <= 12)
            {
                if (bTrongThang == "on")
                {
                    DKDonVi = " AND MONTH(dNgayChungTu)=@iThang AND YEAR(dNgayChungTu)=@iNamLamViec ";
                }
                else
                {
                    DKDonVi = " AND MONTH(dNgayChungTu)<=@iThang AND YEAR(dNgayChungTu)=@iNamLamViec ";
                }
            }
            else
            {

                if (bTrongThang == "on")
                {
                    DKDonVi = " AND MONTH(dNgayChungTu)=@iThang_So AND YEAR(dNgayChungTu)=@iNamLamViec_NamSau ";
                }
                else
                {
                    DKDonVi = " AND (( MONTH(dNgayChungTu)<=@iThang_So AND YEAR(dNgayChungTu)=@iNamLamViec_NamSau) OR ( MONTH(dNgayChungTu)<=12 AND YEAR(dNgayChungTu)=@iNamLamViec)) ";
                }
            }
            for (int i = 0; i < SoCot; i++)
            {
                DK += ",COT" + i + "=SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[i]["sLNS"] + ") AND bThoaiThu=0) THEN rNopNSQP ELSE 0 END)";
                DK += ",COT_ThoaiThu" + i + "=SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[i]["sLNS"] + ")  AND bThoaiThu=1) THEN rNopNSQP ELSE 0 END)";
                DKHAVING += "SUM(CASE WHEN sNG IN (" + dtThongTinBaoCao.Rows[i]["sLNS"] + ") THEN rNopNSQP ELSE 0 END) <>0 OR ";
            }
            //NS nhà nước
            DK += ",TNDN_Thu=SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[5]["sLNS"] + ")  AND bThoaiThu=0) THEN rNopNSNNQuaBQP ELSE 0 END)";
            DK += ",TNDN_ThoaiThu=SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[5]["sLNS"] + ")  AND bThoaiThu=1) THEN rNopNSNNQuaBQP ELSE 0 END)";
            DK += ",TNCN_Thu=SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[6]["sLNS"] + ")  AND bThoaiThu=0) THEN rNopNSNNKhac ELSE 0 END)";
            DK += ",TNCN_ThoaiThu=SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[6]["sLNS"] + ")  AND bThoaiThu=1) THEN rNopNSNNKhac ELSE 0 END)";
            DKHAVING += "SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[5]["sLNS"] + ")  ) THEN rNopNSNNQuaBQP ELSE 0 END) <>0 OR ";
            DKHAVING += "SUM(CASE WHEN (sNG IN (" + dtThongTinBaoCao.Rows[6]["sLNS"] + ")  ) THEN rNopNSNNKhac ELSE 0 END) <>0  ";
            DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String SQL = "";
            if (iID_MaPhongBan == "-1")
            {
                SQL =
            String.Format(@"SELECT iID_MaPhongBan 
                                {0}
                                 FROM TN_ChungTuChiTiet
                                 WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec    {1} {2}
                                 GROUP BY iID_MaPhongBan
                                    HAVING {3}
                                    ", DK, DKDonVi, DKPhongBan, DKHAVING);
            }
            else
            {
                SQL =
               String.Format(@"SELECT iID_MaPhongBan,iID_MaDonVi,sTenDonVi 
                                {0}
                                 FROM TN_ChungTuChiTiet
                                 WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec {1} {2}
                                 GROUP BY iID_MaPhongBan,iID_MaDonVi,sTenDonVi
                                    HAVING {3}
                                    ", DK, DKDonVi, DKPhongBan, DKHAVING);
            }


            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            if (Convert.ToInt32(iThang) > 12)
            {
                cmd.Parameters.AddWithValue("@iNamLamViec_NamSau", iNamSau);
                cmd.Parameters.AddWithValue("@iThang_So", iThang_So);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataTable dtKeHoach = ThuNopModels.getKeHoach(MaND, iID_MaPhongBan);
            for (int i = 0; i < SoCot; i++)
            {
                dt.Columns.Add("Cot_ThucThu" + i, typeof(decimal));
            }
            dt.Columns.Add("TNCN_ThucThu", typeof(decimal));
            dt.Columns.Add("TNDN_ThucThu", typeof(decimal));
            dt.Columns.Add("Cot_TongThoaiThu", typeof(decimal));
            dt.Columns.Add("TongThoaiThu", typeof(decimal));
            dt.Columns.Add("KeHoachNSQP", typeof(decimal));
            dt.Columns.Add("KeHoachNSNN", typeof(decimal));
            decimal TongThoaiThu = 0;
            string iID_MaPhongBan_DK = "", iID_MaDonVi_DK = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                decimal S = 0;
                iID_MaPhongBan_DK += "," + r["iID_MaPhongBan"];
                if (iID_MaPhongBan != "-1")
                    iID_MaDonVi_DK += "," + r["iID_MaDonVi"];
                for (int j = 0; j < SoCot; j++)
                {
                    if (Convert.ToDecimal(dt.Rows[i]["COT_ThoaiThu" + j]) > 0)
                    {
                        r["Cot_ThucThu" + j] = Convert.ToDecimal(r["COT" + j]) - Convert.ToDecimal(r["COT_ThoaiThu" + j]);
                    }
                    else
                    {
                        r["Cot_ThucThu" + j] = 0;
                    }

                    S += Convert.ToDecimal(r["COT_ThoaiThu" + j]);
                    r["Cot_TongThoaiThu"] = S;
                }
                if (Convert.ToDecimal(dt.Rows[i]["TNDN_ThoaiThu"]) > 0)
                {
                    r["TNDN_ThucThu"] = Convert.ToDecimal(r["TNDN_Thu"]) - Convert.ToDecimal(r["TNDN_ThoaiThu"]);
                }
                else
                {
                    r["TNDN_ThucThu"] = 0;
                }
                if (Convert.ToDecimal(dt.Rows[i]["TNCN_ThoaiThu"]) > 0)
                {
                    r["TNCN_ThucThu"] = Convert.ToDecimal(r["TNCN_Thu"]) - Convert.ToDecimal(r["TNCN_ThoaiThu"]);
                }
                else
                {
                    r["TNCN_ThucThu"] = 0;
                }
                TongThoaiThu += Convert.ToDecimal(r["Cot_TongThoaiThu"]);
                //Ghép chỉ tiêu
                String str = "";
                if (iID_MaPhongBan == "-1")
                {
                    str = "iID_MaPhongBan='" + r["iID_MaPhongBan"] +
                                "'";
                }
                else
                {
                    str = "iID_MaPhongBan='" + r["iID_MaPhongBan"] + "' AND iID_MaDonVi='" + r["iID_MaDonVi"] +
                                "'";
                }
                DataRow[] arrdr = dtKeHoach.Select(str);
                if (arrdr.Length > 0)
                {
                    r["KeHoachNSQP"] = Convert.ToDecimal(arrdr[0]["KeHoachNSQP"]);
                    r["KeHoachNSNN"] = Convert.ToDecimal(arrdr[0]["KeHoachNSNN"]);
                }
            }
            if (dt.Rows.Count > 0)
            {
                if (TongThoaiThu > 0)
                    dt.Rows[0]["TongThoaiThu"] = TongThoaiThu;
                else
                    dt.Rows[0]["TongThoaiThu"] = 0;
            }
            ////Truong hop chi co ke hoach ma ko co thu nop thi them vao
            for (int k = 0; k < dtKeHoach.Rows.Count; k++)
            {
                DataRow r = dtKeHoach.Rows[k];
                if (iID_MaPhongBan == "-1")
                {
                    String iID_MaPhongBan_KeHoach = Convert.ToString(r["iID_MaPhongBan"]);
                    if (iID_MaPhongBan_DK.Trim().IndexOf(iID_MaPhongBan_KeHoach.Trim()) == -1)
                    {
                        DataRow dr1 = dt.NewRow();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName != "iID_MaPhongBan" &&
                                dt.Columns[i].ColumnName != "KeHoachNSQP" && dt.Columns[i].ColumnName != "KeHoachNSNN")
                            {
                                dr1[dt.Columns[i].ColumnName] = 0;
                            }

                        }
                        dr1["iID_MaPhongBan"] = iID_MaPhongBan_KeHoach;
                        dr1["KeHoachNSQP"] = Convert.ToDecimal(r["KeHoachNSQP"]);
                        dr1["KeHoachNSNN"] = Convert.ToDecimal(r["KeHoachNSNN"]);

                        dt.Rows.Add(dr1);
                    }
                }
                //chon phong ban
                else
                {
                    String iID_MaDonVi_KeHoach = Convert.ToString(r["iID_MaDonVi"]);
                    String sTenDonVi_KeHoach = Convert.ToString(r["sTenDonVi"]);
                    if (iID_MaDonVi_DK.Trim().IndexOf(iID_MaDonVi_KeHoach.Trim()) == -1)
                    {
                        DataRow dr1 = dt.NewRow();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName != "iID_MaPhongBan" &&
                                dt.Columns[i].ColumnName != "KeHoachNSQP" && dt.Columns[i].ColumnName != "KeHoachNSNN")
                            {
                                dr1[dt.Columns[i].ColumnName] = 0;
                            }

                        }
                        dr1["iID_MaDonVi"] = iID_MaDonVi_KeHoach;
                        dr1["sTenDonVi"] = sTenDonVi_KeHoach;
                        dr1["iID_MaPhongBan"] = Convert.ToString(r["iID_MaPhongBan"]);
                        dr1["KeHoachNSQP"] = Convert.ToDecimal(r["KeHoachNSQP"]);
                        dr1["KeHoachNSNN"] = Convert.ToDecimal(r["KeHoachNSNN"]);

                        dt.Rows.Add(dr1);
                    }

                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String iiD_PB = Convert.ToString(dt.Rows[i]["iID_MaPhongBan"]);
                if (iiD_PB == "02" || iiD_PB == "03" || iiD_PB == "08" || iiD_PB == "16")
                {
                    dt.Rows[i]["KeHoachNSQP"] = Convert.ToDecimal(dt.Rows[i]["Cot0"]) +
                                                 Convert.ToDecimal(dt.Rows[i]["Cot1"]) +
                                                 Convert.ToDecimal(dt.Rows[i]["Cot2"]) +
                                                 Convert.ToDecimal(dt.Rows[i]["Cot3"]) +
                                                 Convert.ToDecimal(dt.Rows[i]["Cot4"]);
                    dt.Rows[i]["KeHoachNSNN"] = Convert.ToDecimal(dt.Rows[i]["TNCN_Thu"]) +
                                                 Convert.ToDecimal(dt.Rows[i]["TNDN_Thu"]);

                }

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
            String iThang = Request.Form[ParentID + "_iThang"];
            String bTrongThang = Request.Form[ParentID + "_bTrongThang"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iThang"] = iThang;
            ViewData["bTrongThang"] = bTrongThang;
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_KeHoachNganSach.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan, String iThang, String bTrongThang)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_KeHoachNganSach");
            LoadData(fr, MaND, iID_MaPhongBan, iThang, bTrongThang);
            DataTable dtThongTinBaoCao = ThuNopModels.getThongTinCotBaoCao("2");
            for (int i = 0; i < 7; i++)
            {
                fr.SetValue("sTen" + i, dtThongTinBaoCao.Rows[i]["sTen"]);
            }
            String TenPB = "";
            if (iID_MaPhongBan != "-1")
                TenPB = " B - " + iID_MaPhongBan;
            String Nam = "";
            if (bTrongThang == "on")
            {
                Nam = " Tháng " + iThang + " năm " + ReportModels.LayNamLamViec(MaND);
            }
            else
            {
                Nam = " Đến tháng " + iThang + " năm " + ReportModels.LayNamLamViec(MaND);
            }

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
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan, String iThang, String bTrongThang)
        {
            DataTable data = rptThuNop_KeHoachNganSach(MaND, iID_MaPhongBan, iThang, bTrongThang);
            data.TableName = "ChiTiet";
            Decimal TongThoaiThu = 0;
            if (data.Rows.Count > 0)
            {
                TongThoaiThu = Convert.ToDecimal(data.Rows[0]["TongThoaiThu"]);
            }
            else
            {

            }
            fr.AddTable("ChiTiet", data);
            DataTable dtPhongBan;
            if (iID_MaPhongBan == "-1")
            {
                dtPhongBan = HamChung.SelectDistinct("DonVi", data, "iID_MaPhongBan", "iID_MaPhongBan");
            }
            else
            {
                dtPhongBan = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi,sTenDonVi");
            }
            fr.AddTable("DonVi", dtPhongBan);
            dtPhongBan.Dispose();
            data.Dispose();
            fr.SetValue("TongThoaiThu", TongThoaiThu);
        }

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan, String iThang, String bTrongThang)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iID_MaPhongBan == "-1")
            {
                sDuongDan = sFilePath_B;
            }
            else
            {
                sDuongDan = sFilePath;
            }

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iThang, bTrongThang);
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

        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan, String iThang, String bTrongThang)
        {
            String sDuongDan = "";
            if (iID_MaPhongBan == "-1")
            {
                sDuongDan = sFilePath_B;
            }
            else
            {
                sDuongDan = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iThang, bTrongThang);

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

