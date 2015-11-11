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
using System.Text.RegularExpressions;
using System.Collections;
namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptQuyetToan_ThongTri_ChungTuController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_ThongTri_ChungTu.xls";
        public static String MaND = "";
        public static String iID_MaChungTu = "", sGhiChu, iLoaiGhiChu;
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_ThongTri_ChungTu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptQuyetToan_ThongTri_ChungTu()
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);


            SQL =
                   String.Format(@"
SELECT SUBSTRING(sLNS,1,1) as sLNS1,
SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,
sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
,SUM(rTuChi) as rTuChi
 FROM QTA_ChungTuChiTiet
 WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
 HAVING SUM(rTuChi)<>0 

", DK, DKDonVi, DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
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
            iID_MaChungTu = Request.Form["QuyetToan" + "_iID_MaChungTu"];
            sGhiChu = Request.Form["QuyetToan" + "_sGhiChu"];
            iLoaiGhiChu = Request.Form["QuyetToan" + "_iLoaiGhiChu"];
            MaND = User.Identity.Name;
            Update_GhiChu();
            return RedirectToAction("ViewPDF");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport()
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePath));
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_ThongTri_ChungTu");

            LoadData(fr);
            String Nam = ReportModels.LayNamLamViec(MaND);
            DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            String iID_MaDonVi = dtChungTu.Rows[0]["iID_MaDonVi"].ToString();
            String iThang_Quy = dtChungTu.Rows[0]["iThang_Quy"].ToString();
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi, MaND);
            String sTenThongTri = CommonFunction.LayTenDanhMuc(iLoaiGhiChu);

            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1, MaND));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2, MaND));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("NgayLap", "quý " + iThang_Quy + " năm " + Nam);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("sTenThongTri", sTenThongTri.ToUpper());
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr)
        {
            int SoDong = 0;
            DataRow r;
            DataTable data = new DataTable();

            data = rptQuyetToan_ThongTri_ChungTu();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS1,sLNS3,sLNS5,sLNS", "sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");


            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "sLNS1,sLNS3,sLNS5", "sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }
            long TongTien = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["rTuChi"].ToString() != "")
                {
                    TongTien += long.Parse(data.Rows[i]["rTuChi"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();

            //Ghi chú
            DataTable dt = new DataTable();
            dt.Columns.Add("sGhiChu", typeof(String));
            int soChu1Trang = 80;

            String SQL =
                String.Format(
                    @"SELECT sGhiChu FROM QTA_GhiChu WHERE sTen=@sTen");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", iID_MaChungTu);
            String sGhiChu = Connection.GetValueString(cmd, "");
            ArrayList arrDongTong = new ArrayList();
            String[] arrDong = Regex.Split(sGhiChu, "&#10;");

            for (int i = 0; i < arrDong.Length; i++)
            {
                if (arrDong[i] != "")
                {
                    int tg = 0;
                    String s = "";
                    String[] arrDongCon = arrDong[i].Split(' ');
                    for (int j = 0; j < arrDongCon.Length; j++)
                    {

                        int x = arrDongCon[j].Length;
                        tg = tg + x + 1;
                        if (tg > soChu1Trang)
                        {
                            arrDongTong.Add(s);
                            j--;
                            tg = 0;
                            s = "";
                            continue;
                        }
                        s += arrDongCon[j].Trim() + " ";

                    }
                    if (tg <= soChu1Trang) arrDongTong.Add(s);

                }
            }
            for (int j = 0; j < arrDongTong.Count; j++)
            {
                r = dt.NewRow();
                r["sGhiChu"] = arrDongTong[j];
                dt.Rows.Add(r);
            }

            SoDong = data.Rows.Count;

            for (int i = 0; i < dtsTM.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsTM.Rows[i]["sMoTa"])))
                    SoDong++;
            }
            for (int i = 0; i < dtsM.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsM.Rows[i]["sMoTa"])))
                    SoDong++;
            }
            for (int i = 0; i < dtsL.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsL.Rows[i]["sMoTa"])))
                    SoDong++;
            }
            for (int i = 0; i < dtsLNS.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsLNS.Rows[i]["sMoTa"])))
                    SoDong++;
            }
            fr.AddTable("dtDongTrang", dt);
            fr.SetValue("Tien", Tien);
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            int KhoanhCachDong = 120;
            int SoDongTrang1 = 23;
            int SoDongTrang2 = 47;
            int SoDongGhiChu = dt.Rows.Count;

            //trang 1 voi cỡ chữ 10, số dòng trên trang 23 dòng
            if (SoDongGhiChu == 0)
            {
                SoDongTrang1 = 23;
                if (SoDong <= SoDongTrang1 + 3 && SoDong > SoDongTrang1)
                    KhoanhCachDong = 158 + (SoDongTrang1 - SoDong) * 2;
            }
            //có ghi chú
            else
            {
                if (SoDongGhiChu <= 10)
                {
                    if (SoDong + SoDongGhiChu > SoDongTrang1 - 3 && SoDong + SoDongGhiChu < SoDongTrang1 + 3)
                    {
                        KhoanhCachDong = 200;
                    }

                }
            }
            fr.SetExpression("test", "<#Row height(Autofit;" + KhoanhCachDong + ")>");
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
        public ActionResult ViewPDF()
        {
            HamChung.Language();
            ExcelFile xls = CreateReport();
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

        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport();

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "Thongtriquyettoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public static String DsGhiChu(String MaChungTu)
        {
            sGhiChu = "";
            String SQL = "";
            SQL = string.Format(@"SELECT sGhiChu+','+iID_MaDonVi FROM QTA_GhiChu WHERE sTen=@sTen");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", MaChungTu);
            sGhiChu = Connection.GetValueString(cmd, "");
            return sGhiChu;
        }
        public void Update_GhiChu()
        {

            String SQL = "";

            SQL = String.Format(
                "DELETE QTA_GhiChu WHERE sTen=@sTen ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            sGhiChu = sGhiChu.Replace("\r\n", "&#10;");
            SQL = String.Format(
                    @"INSERT INTO QTA_GhiChu(sTen,iID_MaDonVi,sGhiChu) VALUES('{0}','{1}','{2}')",
                    iID_MaChungTu, iLoaiGhiChu, sGhiChu);
            cmd = new SqlCommand(SQL);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

    }
}

