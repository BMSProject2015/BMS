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
    public class rptQuyetToan_TongQuyetToan_LNS_DonViController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongQuyetToan_LNS_DonVi.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongQuyetToan_LNS_DonVi.aspx";
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
        /// 


        public static DataTable rptQuyetToan_TongQuyetToan_LNS_DonVi(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            
            //Begin: HungPH: thêm điều kiện LNS
           
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            else {
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    DK += "sLNS=@sLNS" + i;
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                    if (i < arrLNS.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }
             
            //End:

            //Begin HungPh: thêm điều kiện phòng ban
                if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
                {
                    DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan";
                    cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
                }
            //End:
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String SQL =
                String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                            SUBSTRING(sLNS,1,3) as sLNS3,
                            SUBSTRING(sLNS,1,5) as sLNS5,
                             sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                            ,SUM(rTuChi) as rTuChi
                            ,SUM(rLuyKe) as rLuyKe
                            ,SUM(rQuyetToan) as rQuyetToan
                            FROM
                            (


                            SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                            ,rTuChi
                            ,rLuyKe=SUM(CASE WHEN (iThang_Quy<=@iThang_Quy) THEN rTuChi ELSE 0 END)
                            ,rQuyetToan=SUM(CASE WHEN (iThang_Quy=@iThang_Quy) THEN rTuChi ELSE 0 END)
                             FROM QTA_ChungTuChiTiet
                             WHERE iTrangThai=1 AND 
                             iNamLamViec=@iNamLamViec AND
                             iThang_Quy<=@iThang_Quy{0} {1} {2}
                             GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,iThang_Quy,rTuChi) a
                             GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTuChi
                             HAVING SUM(rTuChi)<>0 
                             OR SUM(rLuyKe) <>0
                             OR SUM(rQuyetToan)<>0
                            ", DK, DKDonVi, DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            //cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            //cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];

            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongQuyetToan_LNS_DonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_LNS_DonVi");

            LoadData(fr, MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, iID_MaPhongBan);
            String Nam = ReportModels.LayNamLamViec(MaND);

            //lay ten nam ngan sach
            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
                NamNganSach = "QUYẾT TOÁN NẮM TRƯỚC";
            else if (iID_MaNamNganSach == "2")
                NamNganSach = "QUYẾT TOÁN NĂM NAY";
            else
            {
                NamNganSach = "TỔNG HỢP";
            }
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = rptQuyetToan_TongQuyetToan_LNS_DonVi(MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, iID_MaPhongBan);
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

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);

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
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";
            
                sDuongDan = sFilePath;
           // else
                //sDuongDan = sFilePath_To2;


            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, iID_MaPhongBan);
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

        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";


                sDuongDan = sFilePath;

           
            
           
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, iID_MaPhongBan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopTinhHinhQuyetToanNganSach_LNS.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String Thang_Quy, String iID_MaDonVi, String sLNS, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            DataTable dt = QuyetToan_ReportModels.dtLNS_DonVi(Thang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);

            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

