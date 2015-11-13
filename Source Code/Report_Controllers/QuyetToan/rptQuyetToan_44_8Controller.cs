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


namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_44_8Controller : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_44_8.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_44_8.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm EditSubmit nhận dũ liệu từ view
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String Quy = Convert.ToString(Request.Form[ParentID + "_Quy"]);         
            String TongHop = Convert.ToString(Request.Form[ParentID + "_iTongHop"]);
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_44_8.aspx";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Quy"] = Quy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHop"] = TongHop;
            ViewData["sLNS"] = sLNS;
            ViewData["PageLoad"] = 1;
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Quy = Quy, iID_MaDonVi = iID_MaDonVi, TongHop = TongHop, sLNS = sLNS });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi, String sLNS)
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV="";
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }
            
            String cot1 = "", cot2 = "", cot3 = "";
            if (Quy == "1")
            {
                cot1 = "Tháng 1";
                cot2 = "Tháng 2";
                cot3 = "Tháng 3";
            }
            if (Quy == "2")
            {
                cot1 = "Tháng 4";
                cot2 = "Tháng 5";
                cot3 = "Tháng 6";
            }
            if (Quy == "3")
            {
                cot1 = "Tháng 7";
                cot2 = "Tháng 8";
                cot3 = "Tháng 9";
            }
            if (Quy == "4")
            {
                cot1 = "Tháng 10";
                cot2 = "Tháng 11";
                cot3 = "Tháng 12";
            }
            String TenLNS = "";
            DataTable dt = MoTa(sLNS);
            if (dt != null)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
             FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_44_8");
            LoadData(fr, iID_MaTrangThaiDuyet, Quy, iID_MaDonVi, sLNS, MaND);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("Quy", Quy);
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("cot1", cot1);
                fr.SetValue("cot2", cot2);
                fr.SetValue("cot3", cot3);
                fr.SetValue("TenLNS", TenLNS);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("Phong", Phong);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.Run(Result);
                
                return Result;
            
        }
        /// <summary>
        /// Đổ dữ liệu vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi,String sLNS,String MaND)
        {
            DataTable data = QT_ThuongXuyen_44_8(iID_MaTrangThaiDuyet, Quy, iID_MaDonVi, sLNS,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);

            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi, String sLNS)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Quy, iID_MaDonVi, sLNS);
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
        /// xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi, String sLNS)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Quy, iID_MaDonVi, sLNS);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_44_8.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi, String sLNS)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Quy, iID_MaDonVi, sLNS);
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
        /// bắt sự kiên onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String iID_MaTrangThaiDuyet, String Quy, String sLNS, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, Quy, sLNS, iID_MaDonVi,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String iID_MaTrangThaiDuyet, String Quy, String sLNS, String iID_MaDonVi,String MaND)
        {
            String dsDonVi = "";
            DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet, Quy, sLNS,MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        /// <summary>
        /// lấy danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iID_MaTrangThaiDuyet, String Quy, String sLNS,String MaND)
        {
            String dsThang = "";
            if (Quy == "1")
            {
                dsThang = String.Format(@"(iThang_Quy=1 OR iThang_Quy=2 OR iThang_Quy=3)");
            }
            if (Quy == "2")
            {
                dsThang = String.Format(@"(iThang_Quy=4 OR iThang_Quy=5 OR iThang_Quy=6)");
            }
            if (Quy == "3")
            {
                dsThang = String.Format(@"(iThang_Quy=7 OR iThang_Quy=8 OR iThang_Quy=9)");
            }
            if (Quy == "4")
            {
                dsThang = String.Format(@"(iThang_Quy=10 OR iThang_Quy=11 OR iThang_Quy=12)");
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT NS_DonVi.sTen,QTA_ChungTuChiTiet.iID_MaDonVi
                                         FROM QTA_ChungTuChiTiet
                                         INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi on QTA_ChungTuChiTiet.iID_MaDonVi=NS_DonVi.MaDonVi
                                         WHERE {0}   AND sLNS=@sLNS AND QTA_ChungTuChiTiet.iTrangThai=1 {2} {1}
                                         ORDER BY iID_MaDonVi", dsThang,iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtDonVi.Rows.Count > 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            else
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            return dtDonVi;
        }
        public DataTable QT_ThuongXuyen_44_8(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi, String sLNS,String MaND)
        {
            DataTable dt = new DataTable();
            String s0 = "", s1 = "", s2 = "";
            if (Quy == "1")
            {
                s0 = "1";
                s1 = "2";
                s2 = "3";
            }
            if (Quy == "2")
            {
                s0 = "4";
                s1 = "5";
                s2 = "6";
            }
            if (Quy == "3")
            {
                s0 = "7";
                s1 = "8";
                s2 = "9";
            }
            if (Quy == "4")
            {
                s0 = "10";
                s1 = "11";
                s2 = "12";
            }
             String DKDonVi = "";
             DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet, Quy, sLNS,MaND);
            if (iID_MaDonVi=="-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";
                }
                DKDonVi = " AND(" + DKDonVi + ")";
            }
            else if (iID_MaDonVi == "-2")
            {
                DKDonVi = " AND iID_MaDonVi='"+Guid.Empty.ToString()+"'";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(COT1) AS COT1,SUM(COT2) AS COT2,SUM(COT3) AS COT3 
                                        FROM (SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,COT1=Case WHEN (iThang_Quy={0}) THEN SUM(rTuChi+rHienVat) ELSE 0 END
                                        ,COT2=Case WHEN (iThang_Quy={1}) THEN SUM(rTuChi+rHienVat) ELSE 0 END
                                        ,COT3=Case WHEN (iThang_Quy={2}) THEN SUM(rTuChi+rHienVat) ELSE 0 END	
                                        FROM QTA_ChungTuChiTiet as QTG                                     
                                        WHERE sLNS=@sLNS AND sNG<>'' {4} {3} AND iTrangThai=1 {5}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0) as TB
                                        GROUP BY TB.NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(COT1)<>0 or SUM(COT2)<>0 OR SUM(COT3)<>0 
                                        ORDER BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG
                                       ", s0,s1, s2, DKDonVi,ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);           
           
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSach()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sLNS<> '1010000' AND sL = '' AND iTrangThai=1 ORDER By sXauNoiMa");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS AND iTrangThai=1 ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// dt Trạng Thái Duyệt
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
