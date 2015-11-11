using System;
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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_PhongBanController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_PhongBan.xls";
        private const String sFilePath_TongHop = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_PhongBan_TongHop.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_PhongBan.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>ss
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaPhongBan">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_PhongBan(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            else
            {
                String[] arrSLN = sLNS.Split(',');
                for (int i = 0; i < arrSLN.Length; i++)
                {
                    DK += "sLNS=@sLNS" + i;
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrSLN[i]);
                    if (i < arrSLN.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }

            if (!String.IsNullOrEmpty(iThang_Quy) && iThang_Quy != "-1")
            {
                DK += " AND iThang_Quy=@Thang_Quy";
                cmd.Parameters.AddWithValue("@Thang_Quy", iThang_Quy);
            }

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String SQL =
                String.Format(@"SELECT SUBSTRING(sLNS,1,1) AS sLNS1,
                                        SUBSTRING(sLNS,1,3) AS sLNS3,
                                        SUBSTRING(sLNS,1,5) AS sLNS5,
                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                        ,SUM(rTuChi) as rTuChi
                                FROM QTA_ChungTuChiTiet
                                WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                                GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
                                HAVING SUM(rTuChi)<>0 
            ", DK, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);
            return dtAll;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["QuyetToanNganSach" + "_iID_MaDonVi"];
            String iThang_Quy = Request.Form["QuyetToanNganSach" + "_iThang_Quy"];
            String iID_MaPhongBan = Request.Form["QuyetToanNganSach" + "_iID_MaPhongBan"];


            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;

            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_PhongBan.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_PhongBan");

            LoadData(fr, MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);
            String Nam = ReportModels.LayNamLamViec(MaND);

            if (iThang_Quy == "-1")
            {
                iThang_Quy = "Tất cả các Quý ";
            }

            if (iThang_Quy == "5")
            {
                iThang_Quy = "Bổ Sung ";
            }

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = new DataTable();
            data = rptQuyetToan_PhongBan(MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sLNS5,sMoTa");
           
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }

            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3", "iID_MaPhongBan,iID_MaDonVi,sTenPhongBan,sTenDonVi,sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "iID_MaPhongBan,iID_MaDonVi,sLNS1", "sTenPhongBan,sTenDonVi,iID_MaPhongBan,iID_MaDonVi,sLNS1,sMoTa");
            
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            DataTable dtDonVi = HamChung.SelectDistinct("dtDonVi", dtsLNS1, "iID_MaPhongBan,iID_MaDonVi", "iID_MaPhongBan,sTenPhongBan,iID_MaDonVi,sTenDonVi");
            DataTable dtPhongBan = HamChung.SelectDistinct("dtPhongBan", dtDonVi, "iID_MaPhongBan", "iID_MaPhongBan,sTenPhongBan"); 
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            fr.AddTable("dtDonVi", dtDonVi);
            fr.AddTable("dtPhongBan", dtPhongBan);

            
            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";
            sDuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);
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

        }

        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";
            sDuongDan = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaPhongBan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptQuyetToan_PhongBan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        //Ajax don vi
        public JsonResult Ds_DonVi(String ParentID, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan, String MaND)
        {
            return Json(obj_DonVi(ParentID, iThang_Quy, iID_MaDonVi, iID_MaPhongBan, MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iThang_Quy, String iID_MaDonVi, String iID_MaPhongBan, String MaND)
        {
            String input = "";
            DataTable dt = DonViModels.DanhSach_DonVi_QuyetToan_PhongBan(iID_MaPhongBan, MaND);
            SelectOptionList slDonvi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return input;
        }
    }
}

