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
using DomainModel.Abstract;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDAThongTri_1Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_ThanhToan = "/Report_ExcelFrom/QLDA/rptQLDAThongTri_1_ThanhToan.xls";
        private const String sFilePath_TamUng = "/Report_ExcelFrom/QLDA/rptQLDAThongTri_1_TamUng.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ThongTri_CP_1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_iID_MaDotCapPhat"]);
            String iLoai = Convert.ToString(Request.Form[ParentID + "_iLoai"]);
            String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["iLoai"] = iLoai;
            ViewData["sNoiDung"] = sNoiDung;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ThongTri_CP_1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        private void LoadData(FlexCelReport fr, String iID_MaDotCapPhat, String iLoai)
        {
            DataTable data = dtQLDA_ThongTri_1(iID_MaDotCapPhat, iLoai);
            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            data.Dispose();
        }
        public ActionResult ViewPDF(String iID_MaDotCapPhat, String iLoai, String sNoiDung)
        {
            HamChung.Language();
            String DuongDan = "";
            if (iLoai == "1")
                DuongDan = sFilePath_ThanhToan;
            else
                DuongDan = sFilePath_TamUng;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDotCapPhat, iLoai, sNoiDung);
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
        public clsExcelResult ExportToExcel(String iID_MaDotCapPhat, String iLoai, String sNoiDung)
        {
            HamChung.Language();
            String DuongDan = "";
            if (iLoai == "1")
                DuongDan = sFilePath_ThanhToan;
            else
                DuongDan = sFilePath_TamUng;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDotCapPhat, iLoai, sNoiDung);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTri_1.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String iID_MaDotCapPhat, String iLoai, String sNoiDung)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dt = dtQLDA_ThongTri_1(iID_MaDotCapPhat, iLoai);
            String dNgayLap = "", Tien = "";
            Decimal NamTruoc = 0, NamNay = 0, ThuKhac = 0, TuChoi = 0, Tong = 0;
            DataTable dt_2 = dtQLDA_ThongTri_2(iID_MaDotCapPhat, iLoai);
            if (dt_2.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(dt_2.Rows[0]["NamTruoc"].ToString()))
                {
                    NamTruoc = Convert.ToDecimal(dt_2.Rows[0]["NamTruoc"]);
                }
                if (!String.IsNullOrEmpty(dt_2.Rows[0]["NamNay"].ToString()))
                {
                    NamNay = Convert.ToDecimal(dt_2.Rows[0]["NamNay"]);
                }
                if (!String.IsNullOrEmpty(dt_2.Rows[0]["ThuKhac"].ToString()))
                {
                    ThuKhac = Convert.ToDecimal(dt_2.Rows[0]["ThuKhac"]);
                }
                if (!String.IsNullOrEmpty(dt_2.Rows[0]["TuChoi"].ToString()))
                {
                    TuChoi = Convert.ToDecimal(dt_2.Rows[0]["TuChoi"]);
                }
            }
            Tong = NamTruoc + NamNay;
            dt_2.Dispose();
            if (dt.Rows.Count > 0)
            {
                dNgayLap = dt.Rows[0]["dNgayLap"].ToString();
            }
            long TongTien = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["rSoTien"].ToString() != "")
                {
                    TongTien += long.Parse(dt.Rows[i]["rSoTien"].ToString());
                }
            }
            long _TongTien = TongTien - long.Parse(Tong.ToString()) - long.Parse(ThuKhac.ToString());
            if (_TongTien < 0)
            {
                _TongTien = _TongTien * (-1);
                Tien = "Âm " + CommonFunction.TienRaChu(_TongTien).ToString();
            }
            else
            {
                Tien = CommonFunction.TienRaChu(_TongTien).ToString();
            }
            dt.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDAThongTri_1");
            LoadData(fr, iID_MaDotCapPhat, iLoai);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Tien", Tien);
            fr.SetValue("sNoiDung", sNoiDung);
            fr.SetValue("NamTruoc", NamTruoc);
            fr.SetValue("NamNay", NamNay);
            fr.SetValue("ThuKhac", ThuKhac);
            fr.SetValue("TuChoi", TuChoi);
            fr.SetValue("dNgayLap", dNgayLap);
            fr.Run(Result);
            return Result;
        }
        //tam ung or thanh toan
        public static DataTable dtQLDA_ThongTri_1(String iID_MaDotCapPhat, String iLoai)
        {
            DataTable dt = new DataTable();
            String SQL = "";
            if (iLoai == "1")
            {
                SQL = String.Format(@"SELECT sLNS,sL,sK,sM
                                         ,dNgayLap,SUM(rSoTien) as rSoTien
                                           FROM
                                        (
                                        SELECT sLNS,sL,sK,sM,iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDotCapPhat,
                                        SUM(rDeNghiPheDuyetThanhToan) as rSoTien
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 
	                                          AND sM IN (9200,9250,9300,9350,9400)
                                              AND iID_MaDotCapPhat=@iID_MaDotCapPhat
                                        GROUP BY sLNS,sL,sK,sM,iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDotCapPhat
                                        HAVING SUM(rDeNghiPheDuyetThanhToan)<>0) as CP
                                        INNER JOIN (SELECT iID_MaDotCapPhat,CONVERT(varchar,dNgayLap,103) as dNgayLap 
			                                        FROM QLDA_CapPhat_Dot 
			                                        WHERE iTrangThai=1) as CPDot
                                        ON CP.iID_MaDotCapPhat=CPDot.iID_MaDotCapPhat
                                        GROUP BY sLNS,sL,sK,sM,dNgayLap");
            }
            else
            {
                SQL = String.Format(@"SELECT sLNS,sL,sK,sM
                                         ,dNgayLap,SUM(rSoTien) as rSoTien
                                           FROM
                                        (
                                        SELECT sLNS,sL,sK,sM,iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDotCapPhat,
                                        SUM(rDeNghiPheDuyetTamUng) as rSoTien
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 
	                                          AND sM IN (9200,9250,9300,9350,9400)
                                              AND iID_MaDotCapPhat=@iID_MaDotCapPhat
                                        GROUP BY sLNS,sL,sK,sM,iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDotCapPhat
                                        HAVING SUM(rDeNghiPheDuyetTamUng)<>0) as CP
                                        INNER JOIN (SELECT iID_MaDotCapPhat,CONVERT(varchar,dNgayLap,103) as dNgayLap 
			                                        FROM QLDA_CapPhat_Dot 
			                                        WHERE iTrangThai=1) as CPDot
                                        ON CP.iID_MaDotCapPhat=CPDot.iID_MaDotCapPhat
                                        GROUP BY sLNS,sL,sK,sM,dNgayLap");
            }
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("iID_MaDotCapPhat", iID_MaDotCapPhat);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        //thu hoi tam ung
        public static DataTable dtQLDA_ThongTri_2(String iID_MaDotCapPhat, String iLoai)
        {
            DataTable dt = new DataTable();
            String SQL = "";
            //Tam ung
            if (iLoai == "2")
            {
                SQL = String.Format(@"SELECT 
                                        NamTruoc=0,
                                        NamNay=0,
                                        ThuKhac=0,
                                        TuChoi=SUM(rChuDauTuTamUng)-SUM(rDeNghiPheDuyetTamUng)
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1
                                              AND iID_MaDotCapPhat=@iID_MaDotCapPhat 
	                                          AND sM IN (9200,9250,9300,9350,9400)
	                                          ");
            }
             //Thanh Toan
            else
            {
                SQL = String.Format(@"
                                        SELECT 
                                        NamTruoc=0,
                                        NamNay=SUM(rDeNghiPheDuyetThuTamUng),
                                        ThuKhac=SUM(rDeNghiPheDuyetThuKhac),
                                        TuChoi=SUM(rChuDauTuThanhToan)-SUM(rDeNghiPheDuyetThanhToan)
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 
                                              AND iID_MaDotCapPhat=@iID_MaDotCapPhat
	                                          AND sM IN (9200,9250,9300,9350,9400)
	                                          ");
            }
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("iID_MaDotCapPhat", iID_MaDotCapPhat);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}
