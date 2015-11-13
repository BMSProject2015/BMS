using System;
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

namespace VIETTEL.Report_Controllers.QuyetToan.QuyetToanQuy
{
    public class rptQuyetToan_TongHop_NhapSoLieuController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_NhapSoLieu.xls";
       
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_NhapSoLieu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        
        public static DataTable rptQuyetToan_TongHop_NhapSoLieu(String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            //dkPhongBan
            String DKPhongBan = "", DK = "", DKDonVi = "";
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaPhongBan) && iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }

            //dkNamNganSach
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

            //DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            //DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            String SQL = String.Format(@"
                        SELECT A.sTenDonVi as sTenDonVi, A.sLNS as sLNS, SUM(A.rTuChi) as rTuChi,A.sLNS + '-' + B.sMoTa as sMoTa 
                        FROM QTA_ChungTuChiTiet as A 
                            INNER JOIN NS_MucLucNganSach as B ON A.sLNS=B.sLNS
                        WHERE sTenDonVi<>'' AND rTuChi<>'0' AND A.iTrangThai=1  
                            AND LEN(B.sLNS)=7 AND SUBSTRING(B.sLNS,1,1)<>'8' AND B.sL = ''  
                            AND iNamLamViec=@iNamLamViec 
                            AND iThang_Quy=@iThang_Quy {0} {1} {2}
                        GROUP BY sTenDonVi, A.sLNS, B.sMoTa
                        ORDER BY sTenDonVi", DKPhongBan, DKDonVi, DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];

            ViewData["PageLoad"] = "1";
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_NhapSoLieu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        private void LoadData(FlexCelReport fr, String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            DataTable data = rptQuyetToan_TongHop_NhapSoLieu(MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi = HamChung.SelectDistinct("dtDonVi", data, "sTenDonVi", "sTenDonVi");
            dtDonVi.Columns.Add("STT");

            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                dtDonVi.Rows[i]["STT"] = i + 1;
            }

            fr.AddTable("dtDonVi", dtDonVi);
            data.Dispose();
            dtDonVi.Dispose();
        }

        public ExcelFile CreateReport(String path, String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_LNS_DonVi");

            LoadData(fr, MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);
            String Nam = "";

            //lay ten nam ngan sach
            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
            {
                NamNganSach = "NGÂN SÁCH NĂM TRƯỚC";
                Nam = Convert.ToString(Int32.Parse(ReportModels.LayNamLamViec(MaND)) - 1);
            }
            else if (iID_MaNamNganSach == "2")
                {
                    NamNganSach = "NGÂN SÁCH NĂM NAY";
                    Nam = ReportModels.LayNamLamViec(MaND);
                }
                else
                {
                    NamNganSach = "NGÂN SÁCH TỔNG HỢP";
                    Nam = Convert.ToString(Int32.Parse(ReportModels.LayNamLamViec(MaND)) - 1) + "," + ReportModels.LayNamLamViec(MaND);
                }

            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);

            if (iID_MaPhongBan != "-1")
            {
                String sTenPhongBan = "B" + iID_MaPhongBan;
                fr.SetValue("sTenPhongBan", sTenPhongBan);
            }
            else
            {
                String sTenPhongBan = "Tất cả các B";
                fr.SetValue("sTenPhongBan", sTenPhongBan);
            }

            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            fr.Run(Result);
            
            return Result;
        }
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";

            sDuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);
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

        public clsExcelResult ExportToExcel(String MaND, String iThang_Quy, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";

            sDuongDan = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan);

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
    }
}