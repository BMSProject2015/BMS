using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.KeToan
{
    public class rptPhanTichDuToanNS_52Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptPhanTichDuToanNS_52.xls";
        private const String sFilePath_TieuMuc = "/Report_ExcelFrom/KeToan/TongHop/rptPhanTichDuToanNS_52_TieuMuc.xls";
        private const String sFilePath_Muc = "/Report_ExcelFrom/KeToan/TongHop/rptPhanTichDuToanNS_52_Muc.xls";
        private const String sFilePath_LNS = "/Report_ExcelFrom/KeToan/TongHop/rptPhanTichDuToanNS_52_LNS.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptPhanTichDuToanNS_52.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String ThangLamViec = Convert.ToString(Request.Form[ParentID + "_iThangLamViec"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["ThangLamViec"] = iThang + "," + iQuy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["iThang"] = iThang;
            ViewData["iQuy"] = iQuy;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptPhanTichDuToanNS_52.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String ThangLamViec, String iID_MaDonVi,string LoaiThang_Quy, string LoaiIn)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV = "";
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = "Đơn vị: " + Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            if (iID_MaDonVi == "-1")
            {
                TenDV = "";
            }
            //tính tổng tiền
            DataTable dt = PhanTichDuToanNS_52(NamLamViec, ThangLamViec, iID_MaDonVi, LoaiThang_Quy);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanTichDuToanNS_52");
            /// tach thang or nam
            string[] SThangQuy = ThangLamViec.Split(',');
            LoadData(fr, NamLamViec, ThangLamViec, iID_MaDonVi, LoaiThang_Quy);
            fr.SetValue("Nam", NamLamViec);
            if (LoaiThang_Quy == "0")
            {
                // theo thang
                fr.SetValue("Thang", "Tháng " + SThangQuy[0]);
            }
            else
            {
                fr.SetValue("Thang", "Quý " + SThangQuy[1]);
            }
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec,String ThangLamViec, String iID_MaDonVi,String Thang)
        {

            DataTable data = PhanTichDuToanNS_52(NamLamViec, ThangLamViec, iID_MaDonVi, Thang);
            data.TableName = "DonVi";
            fr.AddTable("DonVi", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "sLNS", "sLNS,sMoTa", "sLNS,sL");
            //update Muc

            dtMuc.Columns.Add("sL1", typeof (string));
            dtMuc.Columns.Add("sK1", typeof(string));
            string sLNS = "";
            string sL = "";
            string sK = "";
            for (int i = 0; i < dtMuc.Rows.Count; i++)
            {
                if(sLNS != "" & sLNS.Equals(Convert.ToString(dtMuc.Rows[i]["sLNS"]))
                    & sL.Equals(Convert.ToString(dtMuc.Rows[i]["sL"]))
                    & sK.Equals(Convert.ToString(dtMuc.Rows[i]["sK"])))
                {
                    dtMuc.Rows[i]["sL1"] = "";
                    dtMuc.Rows[i]["sK1"] = "";
                }
                else
                {
                    sLNS = Convert.ToString(dtMuc.Rows[i]["sLNS"]);
                    sL = Convert.ToString(dtMuc.Rows[i]["sL"]);
                    sK = Convert.ToString(dtMuc.Rows[i]["sK"]);
                    dtMuc.Rows[i]["sL1"] = sL;
                    dtMuc.Rows[i]["sK1"] = sK;
                }
            }

            fr.AddTable("Muc", dtMuc);

            fr.AddTable("LoaiNS", dtLoaiNS);

            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamLamViec, String ThangLamViec, String iID_MaDonVi, String LoaiThang_Quy, String LoaiIn)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            if (LoaiIn.Equals("0"))
            {
                //Loại ngân sách
                DuongDanFile = sFilePath_LNS;
            }
            else if (LoaiIn.Equals("1"))
            {
                //Mục 
                DuongDanFile = sFilePath_Muc;
            }
            else if (LoaiIn.Equals("2"))
            {
                //Tiểu mục
                DuongDanFile = sFilePath_TieuMuc;
            }
            else
            {
                //đơn vị
                DuongDanFile = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaDonVi, LoaiThang_Quy, LoaiIn);
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
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String ThangLamViec, String iID_MaDonVi, String LoaiThang_Quy, String LoaiIn)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            if (LoaiIn.Equals("0"))
            {
                //Loại ngân sách
                DuongDanFile = sFilePath_LNS;
            }
            else if (LoaiIn.Equals("1"))
            {
                //Mục 
                DuongDanFile = sFilePath_Muc;
            }
            else if (LoaiIn.Equals("2"))
            {
                //Tiểu mục
                DuongDanFile = sFilePath_TieuMuc;
            }
            else
            {
                //đơn vị
                DuongDanFile = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaDonVi, LoaiThang_Quy, LoaiIn);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptPhanTichDuToanNS_52.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String ThangLamViec, String iID_MaDonVi, String LoaiThang_Quy, String LoaiIn)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            if (LoaiIn.Equals("0"))
            {
                //Loại ngân sách
                DuongDanFile = sFilePath_LNS;
            }
            else if (LoaiIn.Equals("1"))
            {
                //Mục 
                DuongDanFile = sFilePath_Muc;
            }
            else if (LoaiIn.Equals("2"))
            {
                //Tiểu mục
                DuongDanFile = sFilePath_TieuMuc;
            }
            else
            {
                //đơn vị
                DuongDanFile = sFilePath;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, ThangLamViec, iID_MaDonVi, LoaiThang_Quy, LoaiIn);
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
       
        
        /// <summary>
        /// Data của báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="bThang"></param>
        /// <returns></returns>
        public DataTable PhanTichDuToanNS_52(String NamLamViec, String ThangLamViec, String iID_MaDonVi, string bThang)
        {
                     
            DataTable dtDenThangNay=new DataTable();
            DataTable dtThangNay= new DataTable();
            
            //lay du lieu thang quy
            string[] sThangQuy = ThangLamViec.Split(',');
            string sThangTinKiem = "1";
            //Tao datatable tháng này
            String SQLThangNay = "SELECT sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,sTenDonVi_Nhan,'' as rDTRut " +
                                                "FROM KTKB_ChungTuChiTiet " +
                                                "WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iNamLamViec=@NamLamViec AND iID_MaDonVi_Nhan <>''";

            String SQLThanghientai = "SELECT sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,sTenDonVi_Nhan,sum(rDTRut) as rDTRut " +
                                                "FROM KTKB_ChungTuChiTiet " +
                                                "WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iNamLamViec=@NamLamViec1 AND iID_MaDonVi_Nhan <> ''";

            if(bThang=="0")
            {
                // theo thang
                SQLThangNay += " AND iThang<=@ThangQuy";
                SQLThanghientai += " AND iThang=@ThangQuy1";
                sThangTinKiem = sThangQuy[0];
            }
            else
            {
                //theo quy
                sThangTinKiem = sThangQuy[1];
                if (sThangTinKiem == "1")
                {
                    SQLThangNay += " AND iThang<=3";
                    SQLThanghientai += " AND iThang<=3";
                }
                else if (sThangTinKiem == "2")
                {
                    SQLThangNay += " AND iThang<=6";
                    SQLThanghientai += " AND iThang>=4 AND iThang<=6";
                }
                else if (sThangTinKiem == "3")
                {
                    SQLThangNay += " AND iThang<=9";
                    SQLThanghientai += " AND iThang>=7 AND iThang<=9";
                }
                else
                {
                    SQLThangNay += " AND iThang<=12";
                    SQLThanghientai += " AND iThang>=10 AND iThang<=12";
                }
            }
            SQLThangNay += " Group By sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,sTenDonVi_Nhan Having sum(rDTRut)>0";
            SQLThanghientai += " Group By sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,sTenDonVi_Nhan Having sum(rDTRut)>0";

            String SQLThang = "select t.sLNS,t.sL,t.sK,t.sM,t.sTM,t.sMoTa,t.iID_MaDonVi_Nhan,t.sTenDonVi_Nhan,t1.rDTRut as rDTRut " +
                            "from (" + SQLThangNay + ") as t left join (" + SQLThanghientai +
                              ") as t1 ON t.sLNS = t1.sLNS and t.sL = t1.sL and t.sK = t1.sK and t.sM = t1.sM and t.sTM = t1.sTM and t.iID_MaDonVi_Nhan = t1.iID_MaDonVi_Nhan";

            SqlCommand cmdThangNay = new SqlCommand(SQLThang);
            cmdThangNay.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmdThangNay.Parameters.AddWithValue("@NamLamViec1", NamLamViec);
            cmdThangNay.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            if (bThang=="0")
            {
                // theo thang
                cmdThangNay.Parameters.AddWithValue("@ThangQuy", sThangTinKiem);
                cmdThangNay.Parameters.AddWithValue("@ThangQuy1", sThangTinKiem);
            }
            dtThangNay = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();

            // tạo data đến tháng này
            String SQlDenThangNay = "SELECT sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,sTenDonVi_Nhan,Sum(rDTRut) as rDTRut " +
                                                "FROM KTKB_ChungTuChiTiet " +
                                                "WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iNamLamViec=@NamLamViec AND iID_MaDonVi_Nhan <> ''";
            sThangTinKiem = sThangQuy[0];
            string sThang = sThangTinKiem;
            if (bThang!="0")
            {
                //theo quy
                sThangTinKiem = sThangQuy[1];
                if (sThangTinKiem == "1")
                {
                    sThang = "3";
                }
                else if (sThangTinKiem == "2")
                {
                    sThang = "6";
                }
                else if (sThangTinKiem == "3")
                {
                    sThang = "9";
                }
                else
                {
                    sThang = "12";
                }
            }
            SQlDenThangNay += " AND iThang <=@ThangQuy ";// AND sLNS='1010000' AND sL='460' AND sK='468' AND sM='6400' AND sTM='6404'
            SQlDenThangNay += " Group By sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,sTenDonVi_Nhan Having sum(rDTRut)>0";
            SqlCommand cmdDenThangNay = new SqlCommand(SQlDenThangNay);
            cmdDenThangNay.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmdDenThangNay.Parameters.AddWithValue("@ThangQuy", sThang);
            cmdDenThangNay.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            dtDenThangNay = Connection.GetDataTable(cmdDenThangNay);
            cmdDenThangNay.Dispose();

            #region //Ghep 2 dt thang nay+ den thang nay

            dtThangNay.Columns.Add("rDTRutDT", typeof(Decimal));
            if (dtDenThangNay != null && dtDenThangNay.Rows.Count > 0)
            {
                String sCol = "sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,rDTRut";
                String[] arrCol = sCol.Split(',');
                for (int i = 0; i < dtThangNay.Rows.Count; i++)
                {
                    String xauTruyVan =
                        String.Format(
                            @"sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}' AND iID_MaDonVi_Nhan='{5}'",
                            dtThangNay.Rows[i]["sLNS"],
                            dtThangNay.Rows[i]["sL"], dtThangNay.Rows[i]["sK"],
                            dtThangNay.Rows[i]["sM"], dtThangNay.Rows[i]["sTM"],
                            dtThangNay.Rows[i]["iID_MaDonVi_Nhan"]);
                    DataRow[] R = dtDenThangNay.Select(xauTruyVan);
                    if (R != null && R.Length >0)
                    {
                        dtThangNay.Rows[i]["rDTRutDT"] = R[0]["rDTRut"];
                    }
                }
            }

            #endregion
            return dtThangNay;
        }

    }
}
