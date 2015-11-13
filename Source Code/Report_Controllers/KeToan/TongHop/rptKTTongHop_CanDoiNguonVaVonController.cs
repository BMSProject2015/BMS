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
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTongHop_CanDoiNguonVaVonController : Controller
    {
        //
        // GET: /rptKTTongHop_CanDoiNguonVaVon/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathCD1 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_CanDoiNguonVaVon1.xls";
        private const String sFilePathCD2 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_CanDoiNguonVaVon2.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_CanDoiNguonVaVon.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            //String sTKNo = Convert.ToString(Request.Form[ParentID + "sTKNo"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao"]);
            ViewData["PageLoad"] = "1";
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThang"] = iThang;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_CanDoiNguonVaVon.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iNamLamViec = iNamLamViec, iThang = iThang, LoaiBaoCao = LoaiBaoCao });
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iThang, String LoaiBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTongHop_CanDoiNguonVaVon");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iNamLamViec, iThang, LoaiBaoCao);
            //fr.SetValue("Ma", sTKNo);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("LoaiBaoCao", LoaiBaoCao);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.Run(Result);
            return Result;

        }
        public clsExcelResult ExportToPDF(String iNamLamViec, String iThang, String LoaiBaoCao)
        {
            String DuongDan = "";
            if (LoaiBaoCao == "0")
            {
                DuongDan = sFilePathCD1;
            }
            else
            {
                DuongDan = sFilePathCD2;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, LoaiBaoCao);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }
        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "0")
            {
                DuongDan = sFilePathCD1;
            }
            else
            {
                DuongDan = sFilePathCD2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, LoaiBaoCao);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String iNamLamViec, String iThang, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "0")
            {
                DuongDan = sFilePathCD1;
            }
            else
            {
                DuongDan = sFilePathCD2;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, LoaiBaoCao);
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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iThang, String LoaiBaoCao)
        {
            DataTable data = TongHopChungTuGoc(iNamLamViec, iThang, LoaiBaoCao);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="sTKNo"></param>
        /// <param name="iThang"></param>
        /// <param name="LoaiBaoCao">=0 la phan tien;=1 la hien vat</param>
        /// <returns></returns>
        public DataTable TongHopChungTuGoc(String iNamLamViec, String iThang, String LoaiBaoCao)
        {
            String TS_Nguon, TS_Von;
            String[] arrTS_Nguon;
            String[] arrTS_Von;
            if (LoaiBaoCao == "0")
            {
                TS_Nguon = "94";
                TS_Von = "95";
            }
            else
            {
                TS_Nguon = "96";
                TS_Von = "97";
            }
            DataTable dt1 = KeToan_DanhMucThamSoModels.Get_ThamSoCuaBaoCao(TS_Nguon, iNamLamViec);
            DataTable dt2 = KeToan_DanhMucThamSoModels.Get_ThamSoCuaBaoCao(TS_Von, iNamLamViec);
            arrTS_Nguon = Convert.ToString(dt1.Rows[0]["sThamSo"]).Split(',');
            arrTS_Von = Convert.ToString(dt2.Rows[0]["sThamSo"]).Split(',');
            //Tao dtKQ gom 4 cot va add cac hang tai khoan theo tham so
            DataTable dtKQ = new DataTable();
            dtKQ.Columns.Add("sTK_Nguon", typeof(String));
            dtKQ.Columns.Add("rSoDu_Nguon", typeof(Decimal));
            dtKQ.Columns.Add("sTK_Von", typeof(String));
            dtKQ.Columns.Add("rSoDu_Von", typeof(Decimal));

            DataRow R;
            if (arrTS_Nguon.Length >= arrTS_Von.Length)
            {
                for (int i = 0; i < arrTS_Nguon.Length; i++)
                {
                    R = dtKQ.NewRow();
                    R["sTK_Nguon"] = arrTS_Nguon[i];
                    if (i < arrTS_Von.Length)
                    {
                        R["sTK_Von"] = arrTS_Von[i];
                    }
                    dtKQ.Rows.Add(R);
                }
            }
            else
            {
                for (int i = 0; i < arrTS_Von.Length; i++)
                {
                    R = dtKQ.NewRow();
                    R["sTK_Von"] = arrTS_Von[i];
                    if (i < arrTS_Nguon.Length)
                    {
                        R["sTK_Nguon"] = arrTS_Nguon[i];
                    }
                    dtKQ.Rows.Add(R);
                }
            }
            SqlCommand cmd = new SqlCommand();
            String SQL = " SELECT LK.iThang,TK=substring(LK.sTKNo,1,3)";
            SQL += ",rCK_No=case when LEN(substring(LK.sTKNo,1,3))='3'  then case when SUM(LK.rCK_No)-SUM(LK.rCK_Co) >0 then SUM(LK.rCK_No)-SUM(LK.rCK_Co) else 0 end end";
            SQL += ",rCK_Co=case when LEN(substring(LK.sTKNo,1,3))='3'  then case when SUM(LK.rCK_No)-SUM(LK.rCK_Co)<0 then (SUM(LK.rCK_No)-SUM(LK.rCK_Co))*(-1) else 0 end end";
            SQL += " FROM KT_LuyKe as LK";
            SQL += " WHERE LK.iThang =@iThang AND LK.iTrangThai=1 ";
            SQL += " group by substring(LK.sTKNo,1,3),LK.iThang having SUM(LK.rCK_No)<>0 or SUM(LK.rCK_Co)<>0";
            SQL = String.Format(SQL);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            DataTable dt = Connection.GetDataTable(cmd);
            //Add so du nguon
            for (int i = 0; i < dtKQ.Rows.Count; i++)
            {
                String TK = Convert.ToString(dtKQ.Rows[i]["sTK_Nguon"]).Trim();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    String TK1 = Convert.ToString(dt.Rows[j]["TK"]).Trim();
                    if (TK == TK1)
                    {
                        dtKQ.Rows[i]["rSoDu_Nguon"] = dt.Rows[j]["rCK_Co"];
                        break;
                    }
                }
            }
            //Add so du Von
            for (int i = 0; i < dtKQ.Rows.Count; i++)
            {
                String TK = Convert.ToString(dtKQ.Rows[i]["sTK_Von"]).Trim();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    String TK1 = Convert.ToString(dt.Rows[j]["TK"]).Trim();
                    if (TK == TK1)
                    {
                        dtKQ.Rows[i]["rSoDu_Von"] = dt.Rows[j]["rCK_No"];
                        break;
                    }
                }
            }
            cmd.Dispose();
            return dtKQ;
        }
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "0";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            dt.Dispose();
            return dt;
        }

    }
}
