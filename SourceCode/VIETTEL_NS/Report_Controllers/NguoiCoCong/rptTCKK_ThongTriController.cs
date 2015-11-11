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
using System.Text;
namespace VIETTEL.Report_Controllers.NguoiCoCong
{
    public class rptTCKK_ThongTriController : Controller
    {
        //
        // GET: /rptTCKK_ThongTri/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String Kieu = "")
        {
           
            String sFilePath = "";
            if (Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_6.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_6.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/NguoiCoCong/rptTCKK_ThongTri.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Action lấy các giá trị trên form khi thực hiện submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
           
            String Thang_Quy = "";
            String LoaiThangQuy = Convert.ToString(Request.Form[ParentID + "_LoaiThangQuy"]);
            if (LoaiThangQuy == "1")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String Kieu = Convert.ToString(Request.Form[ParentID + "_Kieu"]);
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["TruongTien"] = TruongTien;
            ViewData["Kieu"] = Kieu;
            ViewData["path"] = "~/Report_Views/NguoiCoCong/rptTCKK_ThongTri.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public long Tong = 0;
        /// <summary>
        /// hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable rptTCKK_ThongTri(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String Kieu)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(LoaiThangQuy))
            {
                LoaiThangQuy = "1";
            }
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "1";
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
           
            String TrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                TrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
            }
            else
            {
                TrangThaiDuyet = " ";
            }
            // Tổng hợp đơn vị và chi tiết
            String DK_ThangQuy = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_ThangQuy = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_ThangQuy = "iThang_Quy IN (4,5,6)";
                        break;
                    case "3": DK_ThangQuy = "iThang_Quy IN (7,8,9)";
                        break;
                    case "4": DK_ThangQuy = "iThang_Quy IN (10,11,12)";
                        break;
                }
            }
            else { DK_ThangQuy = "iThang_Quy=@Thang_Quy"; }
            if (Kieu == "1")
            {
                String SQL = " SELECT b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,SUM(rTuChi) as  rTuChi";
                SQL += " ,Tong=SUM(rTuChi)";
                SQL += " FROM ((SELECT iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,rTuChi";
                SQL += " FROM NCC_ChungTuChiTiet";
                SQL += " WHERE sLNS=@sLNS AND sL<>'' AND sNG<>''  AND {3} {1}  {0} AND iLoai=2 AND iTrangThai=1";
                SQL += " GROUP BY iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,rTuChi";
                SQL += " HAVING SUM({2})!=0";
                SQL += " ) a";
                SQL += "  INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi)";
                SQL += " GROUP BY b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa";
                SQL = string.Format(SQL, TrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), TruongTien,DK_ThangQuy);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue(@"Thang_Quy", Thang_Quy);
                }

                cmd.Parameters.AddWithValue(@"sLNS", sLNS);
                cmd.Parameters.AddWithValue(@"iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["Tong"].ToString());
                }

                dt.Dispose();
            }
            else
            {
                DataTable dtDonVi = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet, TruongTien);
                String DK_DonVi = "";
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DK_DonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < dtDonVi.Rows.Count - 1)
                            DK_DonVi += " OR ";
                    }
                }
               String SQL = String.Format(@" SELECT b.iID_MaDonVi,b.sTen,SUM(a.rTuChi) as  SoTien
                                     FROM ((SELECT iID_MaDonVi,rTuChi  FROM NCC_ChungTuChiTiet
                                     WHERE {2} AND sLNS=@sLNS AND sNG<>'' 
                                     {1} AND iLoai=2  AND iTrangThai=1  {0}
                                     GROUP BY iID_MaDonVi,sLNS,sL,sK, sM,sMoTa,rTuChi)a
                                     INNER JOIN (SELECT * FROM NS_DonVi WHERE  iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec  AND {3}) b ON a.iID_MaDonVi=b.iID_MaDonVi)
                                     GROUP BY b.iID_MaDonVi,b.sTen", TrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND),DK_ThangQuy,DK_DonVi);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
                }
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi"+i,dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }
                }
                cmd.Parameters.AddWithValue(@"sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }
                int a = dt.Rows.Count;
                if (a < 10 && a > 0)
                {
                    for (int i = 0; i < 11 - a; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();
            }
            
            return dt;
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String Kieu)
        {
          
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String LoaiThang_Quy = "";
            switch (LoaiThangQuy)
            {
                case "0":
                    LoaiThang_Quy = "Tháng";
                    break;
                case "1":
                    LoaiThang_Quy = "Quý";
                    break;
                case "2":
                    LoaiThang_Quy = "Năm";
                    break;
            }
            String TenLNS = "";
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            DataTable dt = ReportModels.MoTa(sLNS);
            if (dt.Rows.Count > 0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
            String tendv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCKK_ThongTri");
            LoadData(fr, MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, Kieu);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("Thang_Quy", Thang_Quy);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
            fr.SetValue("LoaiThangQuy", LoaiThang_Quy);
            fr.SetValue("TenLNS", TenLNS);
            fr.SetValue("TenDV", tendv);
            fr.SetValue("BLT", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// lấy tên đơn vị
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
        /// Hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String Kieu)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            DataTable data = rptTCKK_ThongTri(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, Kieu);
            if (Kieu == "1")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);

                data.Dispose();
                DataTable dtTieuMuc;
                dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                fr.AddTable("TieuMuc", dtTieuMuc);

                DataTable dtMuc;
                dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
                fr.AddTable("Muc", dtMuc);

                DataTable dtLoaiNS;
                dtLoaiNS = HamChung.SelectDistinct("LNS", dtMuc, "sLNS", "sLNS,sL,sK,sMoTa", "sLNS,sL");
                fr.AddTable("LNS", dtLoaiNS);
                data.Dispose();
                dtTieuMuc.Dispose();
                dtLoaiNS.Dispose();
                dtMuc.Dispose();
            }
            else
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
        }
        /// <summary>
        /// Xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String Kieu)
        {


            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_6.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_6.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, Kieu);
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
        /// Xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String Kieu)
        {


            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_6.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_6.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, Kieu);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTriTongHopMuc2.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien, String Kieu)
        {

            HamChung.Language();
            String sFilePath = "";
            if (Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_6.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_6.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, TruongTien, Kieu);
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
        /// Ngân sách người có công
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNguoiCoCong()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT sLNS,sLNS+'-'+sMoTa AS TenHT
                                            FROM NCC_ChungTuChiTiet AS NCC
                                            WHERE LEN(sLNS)=7 AND LEFT(sLNS,3) = '206' AND sL = '' 
                                            ORDER BY sLNS");
            dt = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return dt;
        }

        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String TruongTien)
        {
            return Json(obj_DonViTheoNam(ParentID, MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,TruongTien), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoNam(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String TruongTien)
        {
            //String input = "";
            DataTable dt = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            DataTable dtDonvi = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strDonVi = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 90%\"");
            return strDonVi;
            //StringBuilder stbDonVi = new StringBuilder();
            //stbDonVi.Append("<fieldset style=\"text-align:left;padding:2px 2px 4px 4px;font-size:11px;width:260px;height:100px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;\">");
            //stbDonVi.Append("<legend><b>Chọn đơn vị</b></legend>");
            //stbDonVi.Append("<div style=\"width: 255px; height: 85px; overflow: scroll; border:1px solid #006666;\">");
            //stbDonVi.Append("<table class=\"mGrid\">");
            //stbDonVi.Append("<tr>");
            //stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

            //String TenDonVi = "", MaDonVi = "";
            //String[] arrDonVi = iID_MaDonVi.Split(',');
            //String _Checked = "checked=\"checked\"";
            //for (int i = 1; i <= dt.Rows.Count; i++)
            //{
            //    MaDonVi = Convert.ToString(dt.Rows[i - 1]["iID_MaDonVi"]);
            //    TenDonVi = Convert.ToString(dt.Rows[i - 1]["sTen"]);
            //    _Checked = "";
            //    for (int j = 1; j <= arrDonVi.Length; j++)
            //    {
            //        if (MaDonVi == arrDonVi[j - 1])
            //        {
            //            _Checked = "checked=\"checked\"";
            //            break;
            //        }
            //    }

            //    input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" />", MaDonVi, _Checked);
            //    stbDonVi.Append("<tr>");
            //    stbDonVi.Append("<td style=\"width: 15%;\">");
            //    stbDonVi.Append(input);
            //    stbDonVi.Append("</td>");
            //    stbDonVi.Append("<td>" + TenDonVi + "</td>");

            //    stbDonVi.Append("</tr>");
            //}
            //stbDonVi.Append("</table>");
            //stbDonVi.Append("</div>");
            //stbDonVi.Append("</fieldset>");
            //dt.Dispose();
            //String DonVi = stbDonVi.ToString();
            //return DonVi;

        }
        /// <summary>
        /// Hàm lấy dữ liệu theo năm và tháng đổ vào commbox
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable DSDonVi(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaTrangThaiDuyet,String TruongTien)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DK_ThangQuy = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_ThangQuy = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_ThangQuy = "iThang_Quy IN (4,5,6)";
                        break;
                    case "3": DK_ThangQuy = "iThang_Quy IN (7,8,9)";
                        break;
                    case "4": DK_ThangQuy = "iThang_Quy IN (10,11,12)";
                        break;
                }
            }
            else
            {
                DK_ThangQuy = "iThang_Quy=@ThangQuy";
            }
            String SQL = string.Format(@"SELECT DV.iID_MaDonVi,DV.iID_MaDonVi+'-'+DV.sTen AS sTen
                                        FROM NCC_ChungTuChiTiet AS NCC
                                         INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE NCC.iTrangThai=1 {1} AND sLNS=@sLNS  {0}
                                        AND {3} AND iLoai=2
                                        GROUP BY DV.iID_MaDonVi,DV.sTen
                                        HAVING SUM({2})!=0
                                        ORDER BY DV.iID_MaDonVi,DV.sTen", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND),TruongTien,DK_ThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThangQuy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDonVi"] = "";
                dr["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }


        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }

    }
}
