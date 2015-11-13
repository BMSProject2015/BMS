using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Data;

using VIETTEL.Models;
using System.Data.SqlClient;
using DomainModel;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using FlexCel.Render;
using System.IO;
using VIETTEL.Controllers;


namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_TongHop_ChiNganSachController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_TongHop_ChiNganSach.xls";
        private const String sFilePath_ChiTiet = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_TongHop_ChiNganSach_ChiTiet.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_TongHop_ChiNganSach.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public static DataTable rptDuToanBS_TongHop_ChiNganSach(String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao)
        {
            String DKDonVi = ""; String DKPhongBan = ""; String DK = ""; String DKDot = "";
            SqlCommand cmd = new SqlCommand();


            //Điều kiện đơn vị
            DataTable dtNĐonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dtNĐonVi.Rows.Count; i++)
            {
                DKDonVi += "A.iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < dtNĐonVi.Rows.Count - 1)
                    DKDonVi += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtNĐonVi.Rows[i]["iID_MaDonVi"]);
            }
            if (String.IsNullOrEmpty(DKDonVi)) DKDonVi = " AND 0=1";
            else
            {
                DKDonVi = "AND (" + DKDonVi + ")";
            }
            //if (LoaiBaoCao == "ChiTiet")
            //{
            //    if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
            //    {
            //        DK += " AND iID_MaDonVi=@iID_MaDonVi";
            //        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            //    }
            //}
            //else
            //{
                if (String.IsNullOrEmpty(iID_MaDonVi))
                {
                    iID_MaDonVi = Guid.Empty.ToString();
                }
                String[] arrDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DK += "A.iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
           // }
            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND ( " + DK + ") ";
            }

            //điều kiện LNS
            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN ( " + sLNS + ") ";
            }

            //điều kiện phòng ban
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DK += " AND A.iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }
            DKDot += " AND CONVERT(VARCHAR(24),B.dNgayChungTu,105) BETWEEN @MaDotTu AND @MaDotDen ";

            int Dvt = 1000;
            //DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            String Sql = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,
 sLNS,sL,sK,sM,sTM,sTTM,sNG,A.sMoTa,A.iID_MaDonVi,sTenDonVi,CONVERT(VARCHAR(24),B.dNgayChungTu,105) as NgayChungTu
,SUM(rTuChi/{4}) as rTuChi,
SUM(rHangMua/{4}) as rHangMua,
SUM(rHangNhap/{4}) as rHangNhap,
SUM(rHienVat/{4}) as rHienVat,
SUM(rPhanCap/{4}) as rPhanCap,
SUM(rDuPhong/{4}) as rDuPhong
 FROM DT_ChungTuChiTiet as A INNER JOIn DT_ChungTu as B
 ON A.iID_MaChungTu = B.iID_MaChungTu
 WHERE A.iTrangThai=1 AND A.iNamLamViec=@iNamLamViec {0} {1} {2} {3}
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,A.sMoTa,A.iID_MaDonVi,sTenDonVi,CONVERT(VARCHAR(24),B.dNgayChungTu,105) 
 HAVING SUM(rTuChi)<>0 
        OR SUM(rHangMua)<>0
        OR SUM(rHangNhap)<>0
        OR SUM(rHienVat)<>0
        OR SUM(rPhanCap)<>0
        OR SUM(rDuPhong)<>0

", DK, DKDonVi, DKPhongBan,DKDot, Dvt);
            cmd.CommandText = Sql;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("MaDotTu", iID_MaDotTu);
            cmd.Parameters.AddWithValue("MaDotDen", iID_MaDotDen);
            DataTable dtAll = Connection.GetDataTable(cmd);
            return dtAll;
        }

        //lấy dữ liệu
        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaPhongBan = Request.Form["DuToanBS" + "_iID_MaPhongBan"];
            String iID_MaDonVi = Request.Form["sDV"];
            String iID_MaDotTu = Request.Form["DuToanBS" + "_iID_MaDotTu"];
            String iID_MaDotDen = Request.Form["DuToanBS" + "_iID_MaDotDen"];
            String LoaiTongHop = Request.Form["DuToanBS" + "_LoaiTongHop"];
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaDotTu"] = iID_MaDotTu;
            ViewData["iID_MaDotDen"] = iID_MaDotDen;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_TongHop_ChiNganSach.aspx";
            return View(sViewPath + "ReportView.aspx");            
        }

        //Tạo báo cáo
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanBS_TongHop_ChiNganSach");

            LoadData(fr, iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND,LoaiBaoCao);
            String sTenPB = "";
            if (iID_MaPhongBan == "-1")
            {
                sTenPB = "Tất cả các phòng ban ";
            }
            else
                sTenPB = "B " + iID_MaPhongBan;
            String sTenDonVi = "";
            if (LoaiBaoCao == "ChiTiet")
            {
                sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            }            
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("tungay", iID_MaDotTu);
            fr.SetValue("denngay", iID_MaDotDen);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("nam", ReportModels.LayNamLamViec(MaND));
            fr.Run(Result);
            return Result;      
        }

        //đổ dữ liệu xuống báo cáo
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao) { 
            DataTable dtDonVi = new DataTable();
            DataTable data = new DataTable();
            DataRow r;

            if (LoaiBaoCao == "ChiTiet")
            {
                data = rptDuToanBS_TongHop_ChiNganSach(iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND, LoaiBaoCao);
            }
            else
            {
                dtDonVi = rptDuToanBS_TongHop_ChiNganSach(iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND, LoaiBaoCao);
                data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
                fr.AddTable("dtDonVi", dtDonVi);
                dtDonVi.Dispose();
            }
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS", "NgayChungTu,sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "NgayChungTu,sLNS1,sLNS3,sLNS5", "NgayChungTu,sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "NgayChungTu,sLNS1,sLNS3", "NgayChungTu,sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "NgayChungTu,sLNS1", "NgayChungTu,sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }
            DataTable dtDot = HamChung.SelectDistinct("dtDot", dtsLNS1, "NgayChungTu", "NgayChungTu");
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            fr.AddTable("dtDot", dtDot);
            

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();
            dtDot.Dispose();
          

        }
        //Lay mo ta
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }

        //hien thi dang PDF
        public ActionResult ViewPDF(String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (LoaiBaoCao == "ChiTiet")
            {
                sDuongDan = sFilePath_ChiTiet;
            }
            else
            {
                sDuongDan = sFilePath;
            }
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND,LoaiBaoCao);
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

        public clsExcelResult ExportToExcel(String iID_MaDonVi, String sLNS, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String MaND,String LoaiBaoCao)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (LoaiBaoCao == "ChiTiet")
            {
                sDuongDan = sFilePath_ChiTiet;
            }
            else
            {
                sDuongDan = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND,LoaiBaoCao);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptDuToanBS_TongHop_ChiNganSach.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public JsonResult Ds_LNS(String ParentID, String iID_MaDot, String iID_MaPhongBan, String iID_MaDonVi, String sLNS)
        {
            String MaND = User.Identity.Name;
            DataTable dt1 = DuToanBS_ReportModels.dtDonVi_LNS(iID_MaDot, iID_MaPhongBan, MaND, iID_MaDonVi);
            String ViewNam = "~/Views/DungChung/DonVi/LNS_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt1, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Ds_DonVi(String ParentID, String iID_MaDotTu, String iID_MaDotDen, String iID_MaPhongBan, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            DataTable dt = DuToanBS_ReportModels.dtDonVi_Dot2(iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan);
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_Dot_DuToanBS.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}