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


namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNop_TongHopPhanThuNganSach_42C_CController : Controller
    {
        //
        // GET: /rptThuNop_TongHopPhanThuNganSach_42C_C/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNop_TongHopPhanThuNganSach_42C_C.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_TongHopNganSachNam_42C_C.ascx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
       
            return RedirectToAction("Index", new { NamLamViec = NamLamViec, iID_MaDonVi = iID_MaDonVi });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNop_TongHopPhanThuNganSach_42C_C");
            LoadData(fr, NamLamViec);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;
           
        }
        /// <summary>
        /// Lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec)
        {
            DataTable data = _DT_ThuNop_TongHopThuNganSach_42C_C(NamLamViec);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
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
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThuNop_42C_C.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec )
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec);
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

        //801 thu cân đối
        //8010101 Thu tiền sử dụng đất
        //8010102 thanh lý (Tạm thời vì chưa biết mục th.Xử lý)
        //8010103 Phí, lệ phí để lại
        //8010199 Thu cân đối khác


        //80102 thu quản lý
        //8010201-Đơn vị dự toán nộp theo chế độ    
        //8010202-Rà phá bom mìn, vật cản
        //8010203-Khấu hao cơ bản
        //8010204-Thu điều tiết lợi nhuận sau thuế
        //8010299 Thu khác 
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public DataTable _DT_ThuNop_TongHopThuNganSach_42C_C(String Nam)
        {
            String SQL = String.Format(@"SELECT TN.iID_MaDonVi,dv.sTen,
                                        sum(case TN.sLNS when 8010101 then TN.rNopNSQP else 0 end) '8010101',
                                        sum(case TN.sLNS when 8010102 then TN.rNopNSQP else 0 end) '8010102',
                                        sum(case TN.sLNS when 8010103 then TN.rNopNSQP else 0 end) '8010103',
                                        sum(case TN.sLNS when 8010104 then TN.rNopNSQP else 0 end) '8010104',
                                        sum(case TN.sLNS when 8010199 then TN.rNopNSQP else 0 end) '8010199',
                                        sum(case TN.sLNS when 8010201 then TN.rNopNSQP else 0 end) '8010201',
                                        sum(case TN.sLNS when 8010202 then TN.rNopNSQP else 0 end) '8010202',
                                        sum(case TN.sLNS when 8010203 then TN.rNopNSQP else 0 end) '8010203',
                                        sum(case TN.sLNS when 8010204 then TN.rNopNSQP else 0 end) '8010204',
                                        sum(case TN.sLNS when 8010205 then TN.rNopNSQP else 0 end) '8010205',
                                        sum(case TN.sLNS when 8010206 then TN.rNopNSQP else 0 end) '8010206',
                                        sum(case TN.sLNS when 8010299 then TN.rNopNSQP else 0 end) '8010299',
                                        sum(case when SUBSTRING(TN.sLNS,1,3)='802'and TN.sLNS<>'802'then TN.rNopNSQP else 0 end) '802'
                                        FROM TN_ChungTuChiTiet as TN
                                        INNER JOIN ( SELECT dv.iID_MaDonVi,dv.sTen FROM NS_DonVi as dv WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                        ON DV.iID_MaDonVi=tn.iID_MaDonVi
                                        where TN.iNamLamViec=@iNamLamViec and tn.iTrangThai=1 and tn.sNG<>'' 
                                       -- and TN.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        group by TN.iID_MaDonVi,dv.sTen
                                        order by TN.iID_MaDonVi
                                        ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
          //  int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach);
           // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable DT_ThuNop_TongHopThuNganSach_42C_C(String NamLamViec)
        {
            DataTable dt = new DataTable();

            String[] arrThuCanDoi = { "8010101", "8010102", "8010103","8010199"};
            String[] arrThuQuanLy = { "8010201", "8010202", "8010203", "8010204", "8010299" };
            
            String SQL = "SELECT distinct iID_MaDonVi FROM TN_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 {0}";

            SQL = String.Format(SQL, "");//ReportModels.DieuKien_TrangThaiDuyet
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
           // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach));
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            dtDonVi.Columns.Add("TenDonVi", typeof(String));
            dtDonVi.Columns.Add("TongNSQP", typeof(Decimal));
            dtDonVi.Columns.Add("TongThuCanDoi", typeof(Decimal));            
            dtDonVi.Columns.Add("8010101", typeof(Decimal));
            dtDonVi.Columns.Add("8010102", typeof(Decimal));
            dtDonVi.Columns.Add("8010103", typeof(Decimal));
            dtDonVi.Columns.Add("8010199", typeof(Decimal));

            dtDonVi.Columns.Add("TongThuQuanLy", typeof(Decimal));
            dtDonVi.Columns.Add("8010201", typeof(Decimal));
            dtDonVi.Columns.Add("8010202", typeof(Decimal));
            dtDonVi.Columns.Add("8010203", typeof(Decimal));
            dtDonVi.Columns.Add("8010204", typeof(Decimal));
            dtDonVi.Columns.Add("8010299", typeof(Decimal));
            dtDonVi.Columns.Add("TongNSNN", typeof(Decimal));
            dtDonVi.Columns.Add("TongThu", typeof(Decimal));

            cmd.Dispose();

            DataTable dtChiTiet = null;

            SQL = "SELECT NS_DonVi.sTen,ChiTiet.iID_MaDonVi,sLNS,ChiTiet.sMoTa";
            SQL +=",SUM(rNopNSQP) AS rNopNSQP,SUM(rNopNSNN) AS rNopNSNN ";
            SQL +=" FROM(( SELECT iID_MaDonVi,sLNS,sMoTa,SUM(rNopNSQP) AS rNopNSQP,SUM(rNopNSNN) AS rNopNSNN";
            SQL += " FROM  TN_ChungTuChiTiet";
            SQL += " WHERE iNamLamViec=2012 AND sNG <>'' AND iTrangThai=1 {0}";
            SQL += " GROUP BY iID_MaDonVi,sLNS,sMoTa) ChiTiet";
            SQL+=" INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=ChiTiet.iID_MaDonVi)";
            SQL+=" GROUP BY sTen,ChiTiet.iID_MaDonVi,sLNS,ChiTiet.sMoTa";
            SQL = String.Format(SQL, "");//ReportModels.DieuKien_TrangThaiDuyet

            cmd = new SqlCommand(SQL);            
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach));
            dtChiTiet = Connection.GetDataTable(cmd);

            String XauTruyVan = "";
            DataRow R1;
            DataRow[] arrR;
            Decimal TongNSNN = 0,TongThuCanDoi,TongThuQuanLy,TongNSQP;
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                R1 = dtDonVi.Rows[i];
                XauTruyVan = String.Format("iID_MaDonVi='{0}'", R1["iID_MaDonVi"]);
                arrR = dtChiTiet.Select(XauTruyVan);
                TongNSNN = 0;
                TongThuCanDoi=0;
                TongThuQuanLy=0;
                TongNSQP=0;
                if (arrR != null)
                {                                        
                    foreach (DataRow _R in arrR)
                    {
                        R1["TenDonVi"] =_R["sTen"];
                        for (int c = 0; c < arrThuCanDoi.Length; c++)
                        {
                            if (arrThuCanDoi[c].Equals(_R["sLNS"]))
                            {
                                R1[arrThuCanDoi[c]] = _R["rNopNSQP"];
                                TongThuCanDoi =TongThuCanDoi + Convert.ToDecimal(_R["rNopNSQP"]);
                            }
                        }

                        for (int c = 0; c < arrThuQuanLy.Length; c++)
                        {
                            if (arrThuQuanLy[c].Equals(_R["sLNS"]))
                            {
                                R1[arrThuQuanLy[c]] = _R["rNopNSQP"];
                                TongThuQuanLy = TongThuQuanLy + Convert.ToDecimal(_R["rNopNSQP"]);
                            }
                        }
                        TongNSQP = TongThuCanDoi + TongThuQuanLy;
                        TongNSNN = TongNSNN + Convert.ToDecimal(_R["rNopNSNN"]);
                    }
                   
                    R1["TongThuCanDoi"] = TongThuCanDoi;
                    R1["TongThuQuanLy"] = TongThuQuanLy;
                    R1["TongNSQP"] = TongNSQP;
                    R1["TongNSNN"] = TongNSNN;
                    R1["TongThu"] = TongNSNN+TongNSQP;
                }
            }
            return dtDonVi;
        }
    }
}
//801 thu cân đối
//8010101 Thu tiền sử dụng đất
//8010103 Phí, lệ phí để lại
//8010199 Thu cân đối khác
//Thu khác các mục còn lại

//802 thu quản lý
//8010201-Đơn vị dự toán nộp theo chế độ    
//8010202-Rà phá bom mìn, vật cản
//8010203-Khấu hao cơ bản
//8010204-Thu điều tiết lợi nhuận sau thuế
//Thu khác các mục còn lại