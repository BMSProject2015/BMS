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
//using DomainModel.Controls;
using VIETTEL.Models;
//using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_ChiTieuNganSach2Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach2.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_ChiTieuNganSach2.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String idDot = Request.Form[ParentID + "_iID_MaDot"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            // String iID_DonVi = Request.Form[ParentID + "_tdDonVi"];
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];

            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaDot"] = idDot;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;

            ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_ChiTieuNganSach2.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult ViewPDF(String MaND, String iID_MaDot, String sLNS, String iID_MaDonVi, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";
            sDuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iID_MaDot, iID_MaDonVi, iID_MaPhongBan);
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

        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            //Hungpx : chua dung den chu ky
            //fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_PhongBan");

            LoadData(fr, MaND, sLNS, iID_MaDot, iID_MaDonVi, iID_MaPhongBan);
            String Nam = ReportModels.LayNamLamViec(MaND);


            String sTenDonVi = "";
            String sTenPB = "";
            if (iID_MaPhongBan == "-1")
            {
                sTenPB = "Tất cả các phòng ban ";
            }
            else
                sTenPB = "B " + iID_MaPhongBan;
            if (iID_MaDonVi == "-1")
            {
                sTenDonVi = "Tất cả các đơn vị ";
            }
            else
                sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi, MaND);
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("ThangNam", ReportModels.Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Dot", iID_MaDot);
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3));
            fr.Run(Result);
            return Result;
        }

        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = new DataTable();
            data = rptDuToanBS_ChiTieuNganSach(MaND, sLNS, iID_MaDot, iID_MaDonVi, iID_MaPhongBan);
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

            long TongTien = 0;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["rTuChi"].ToString() != "")
                {
                    //TongTien += Convert.ToInt64(data.Rows[i]["rTuChi"].ToString());
                    TongTien += Convert.ToInt64(data.Rows[i]["rTuChi"]);
                }
            }

            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();

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
            fr.SetValue("Tien", Tien);

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
        public static DataTable rptDuToanBS_ChiTieuNganSach(String MaND, String sLNS, String iID_MaDot, String iID_MaDonVi, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            int donvitinh = 1000;
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
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaDot", iID_MaDot);
                DK += "and iID_MaDotNganSach=@iID_MaDot";
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            String SQL =
                String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,
 sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
,SUM(rTuChi/{1}) as rTuChi, SUM(rHienVat/{1}) as rHienVat, SUM(rDuPhong/{1}) as rDuPhong
 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {2} {3}
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi,iID_MaPhongBan,sTenPhongBan
 HAVING SUM(rTuChi)<>0 
", DK, donvitinh, DKDonVi, DKPhongBan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            // cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dtAll = Connection.GetDataTable(cmd);
            return dtAll;
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            DataTable dt = DuToanBS_ReportModels.dtDonVi_Dot(iID_MaDot, iID_MaPhongBan, MaND);
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Ds_LNS(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi, String sLNS)
        {
            String MaND = User.Identity.Name;
            DataTable dt = DuToanBS_ReportModels.dtLNS_Dot(iID_MaDot, iID_MaPhongBan, iID_MaDonVi, MaND);
            String ViewNam = "~/Views/DungChung/DonVi/LNS_DanhSach_1.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt, ParentID);
            String strLNS = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strLNS, JsonRequestBehavior.AllowGet);
        }
    }
}