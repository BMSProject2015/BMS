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
    public class rptQuyetToan_43_7Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_43_7.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public ActionResult Index(String sLNS)
        {
            ViewData["PageLoad"] = 0;
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["sLNS"] = sLNS;
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_43_7.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm EditSubmit nhận và truyền các giá trị với VIEW
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String ThangQuy = Convert.ToString(Request.Form[ParentID + "_Quy"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["sLNS"] = sLNS;
            ViewData["ThangQuy"] = ThangQuy;
            ViewData["TruongTien"] = TruongTien;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_43_7.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path">đường dẫn</param>
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <param name="iID_MaDonVi">mã đơn vị</param>
        /// <param name="ThangQuy">Quý làm việc</param>
        /// <param name="TruongTien">tự chi hay hiện vật</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String ThangQuy, String TruongTien)
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
            String TenDV = "";
            //lấy tên đơn vị theo mã
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }

            String Quy1 = "", Quy2 = "", Quy3 = "", Quy4 = "";
            if (ThangQuy == "1")
            {
                Quy1 = "Quý 1";
            }
            else if (ThangQuy == "2")
            {
                Quy1 = "Quý 1";
                Quy2 = "Quý 2";
            }
            else if (ThangQuy == "3")
            {
                Quy1 = "Quý 1";
                Quy2 = "Quý 2";
                Quy3 = "Quý 3";
            }
            else if (ThangQuy == "4")
            {
                Quy1 = "Quý 1";
                Quy2 = "Quý 2";
                Quy3 = "Quý 3";
                Quy4 = "Quý 4";
            }
            //set các ĐK
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_43_7");
            LoadData(fr, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, ThangQuy, TruongTien, MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", ThangQuy);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("Quy1", Quy1);
            fr.SetValue("Quy2", Quy2);
            fr.SetValue("Quy3", Quy3);
            fr.SetValue("Quy4", Quy4);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Phong", Phong);
            fr.SetValue("TruongTien", TruongTien);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// lấy dữ liệu đổ vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <param name="iID_MaDonVi">mã đơn vị</param>
        /// <param name="ThangQuy">Quý làm việc</param>
        /// <param name="TruongTien">tự chi hay hiện vật</param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String ThangQuy, String TruongTien, String MaND)
        {
            DataTable data = QT_43_7(iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, ThangQuy, TruongTien, MaND);
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

            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <param name="iID_MaDonVi">mã đơn vị</param>
        /// <param name="ThangQuy">Quý làm việc</param>
        /// <param name="TruongTien">tự chi hay hiện vật</param>
        /// <returns></returns>>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String ThangQuy, String TruongTien)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, ThangQuy, TruongTien);
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
        /// Xuất báo cáo ra Excel
        /// </summary>
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <param name="iID_MaDonVi">mã đơn vị</param>
        /// <param name="ThangQuy">Quý làm việc</param>
        /// <param name="TruongTien">tự chi hay hiện vật</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String ThangQuy, String TruongTien)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, ThangQuy, TruongTien);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_43_7.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        /// xem PDF
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <param name="iID_MaDonVi">mã đơn vị</param>
        /// <param name="ThangQuy">Quý làm việc</param>
        /// <param name="TruongTien">tự chi hay hiện vật</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String ThangQuy, String TruongTien)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, ThangQuy, TruongTien);
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
        /// bắt sự kiện onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String iID_MaTrangThaiDuyet, String sLNS, String Thang_Quy, String TruongTien, String MaND, String iID_MaDonVi)
        {
            return Json(obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, TruongTien, MaND, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String iID_MaTrangThaiDuyet, String sLNS, String Thang_Quy, String TruongTien, String MaND, String iID_MaDonVi)
        {
            String dsDonVi = "";
            DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet, sLNS, Thang_Quy, TruongTien, MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        /// <summary>
        /// Lấy danh sách đơn vị theo Năm, Quý  có dữ liệu được duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iID_MaTrangThaiDuyet, String sLNS, String Thang_Quy, String TruongTien, String MaND)
        {
            {
                String DKThangQuy = "";
                {
                    if (Thang_Quy == "1")
                        DKThangQuy = "iThang_Quy <=3";
                    else if (Thang_Quy == "2")
                        DKThangQuy = "iThang_Quy <=6";
                    else if (Thang_Quy == "3")
                        DKThangQuy = "iThang_Quy <=9";
                    else
                        DKThangQuy = "iThang_Quy <=12";
                }
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iNamLamViec = DateTime.Now.Year.ToString();
                String iID_MaNguonNganSach = "1";
                String iID_MaNamNganSach = "2";
                String DKDuyet = "", DKDuyet_PB = "";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                    DKDuyet_PB = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
                }
                else
                {
                    DKDuyet = "";
                    DKDuyet_PB = "";
                }
                if (dtCauHinh.Rows.Count > 0)
                {
                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

                }
                dtCauHinh.Dispose();
                int iThangQuy = Convert.ToInt32(Thang_Quy);
                int NgayChiTieu = iThangQuy;
                //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
                NgayChiTieu = iThangQuy * 3;
                //DKLoaiNganSach
                String DKLNS = "";
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    DKLNS += "sLNS=@sLNS" + i;
                    if (i < arrLNS.Length - 1)
                        DKLNS += " OR ";
                }
                
                String SQL = String.Format(@"SELECT DISTINCT b.sTen,a.iID_MaDonVi
                                        FROM (SELECT iID_MaDonVi
	                                          FROM QTA_ChungTuChiTiet
	                                          WHERE   iTrangThai=1 {1}
	                                                AND ({5}) AND {3} {2} AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi
                                        UNION  
                                        SELECT DISTINCT b.sTen,a.iID_MaDonVi
                                        FROM (SELECT iID_MaDonVi
	                                          FROM PB_PhanBoChiTiet
	                                          WHERE  iTrangThai=1 {1} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
	                                                AND ({5}) {4} AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi      
                                        ORDER BY   iID_MaDonVi                           

", TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKThangQuy, DKDuyet_PB,DKLNS);
                SqlCommand cmd = new SqlCommand(SQL);
                //cmd.Parameters.AddWithValue("@LoaiThangQuy", LoaiThang_Quy);

                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS"+i, arrLNS[i]);
                }
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("dNgay", NgayChiTieu);
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
                    cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                }
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
        }
        /// <summary>
        /// quyết toán 43_7
        /// </summary>
        /// <param name="NamLamViec">năm làm việc</param>
        /// <param name="sLNS">slns</param>
        /// <param name="iID_MaDonVi">mã đơn vị</param>
        /// <param name="ThangQuy">Quý làm việc</param>
        /// <param name="TruongTien">tự chi hay hiện vật</param>
        /// <returns></returns>
        public DataTable QT_43_7(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String ThangQuy, String TruongTien, String MaND)
        {
            DataTable dt;
            DataTable dtChiTieu;
            String s1 = "", s2 = "", s3 = "", sNgay = "", DKLNS = "";
            if (ThangQuy == "1")
            {
                s1 = "iThang_Quy IN(-1)";
                s2 = "iThang_Quy IN(-1)";
                s3 = "iThang_Quy IN(-1)";
                sNgay = "4";
            }
            if (ThangQuy == "2")
            {
                s1 = "iThang_Quy IN(4,5,6)";
                s2 = "iThang_Quy IN(-1)";
                s3 = "iThang_Quy IN(-1)";
                sNgay = "7";
            }
            if (ThangQuy == "3")
            {
                s1 = "iThang_Quy IN(4,5,6)";
                s2 = "iThang_Quy IN(7,8,9)";
                s3 = "iThang_Quy IN(-1)";
                sNgay = "10";
            }
            if (ThangQuy == "4")
            {
                s1 = "iThang_Quy IN(4,5,6)";
                s2 = "iThang_Quy IN(7,8,9)";
                s3 = "iThang_Quy IN(10,11,12)";
                sNgay = "13";
            }
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet,sLNS,ThangQuy,TruongTien,MaND);
            String DKDonVi = "";
            if (iID_MaDonVi == "-1")
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
                DKDonVi = " AND iID_MaDonVi='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            String DkDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DkDuyet = " ";
            }
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(Quy1) AS Quy1,SUM(Quy2) AS Quy2,SUM(QUY3) AS QUY3,SUM(QUY4) AS QUY4 
                                        FROM (SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,QUY1=Case WHEN iThang_Quy IN(1,2,3) THEN SUM({0}) ELSE 0 END
                                        ,QUY2=Case WHEN {1} THEN SUM({0}) ELSE 0 END
                                        ,QUY3=Case WHEN {2} THEN SUM({0}) ELSE 0 END
                                        ,QUY4=Case WHEN {3} THEN SUM({0}) ELSE 0 END	
                                        FROM QTA_ChungTuChiTiet as QTG                                     
                                        WHERE ({4}) AND sNG<>''{6} {5} AND iTrangThai=1  {7}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        HAVING SUM({0})>0) as TB
                                        GROUP BY TB.NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(Quy1)<>0 or SUM(Quy2)<>0 OR SUM(Quy3)<>0 OR SUM(Quy4)<>0
                                        ORDER BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG
                                       ", TruongTien, s1, s2, s3, DKLNS, DKDonVi, ReportModels.DieuKien_NganSach(MaND), DkDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }

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
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            String DKDuyet_PB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet_PB = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                DKDuyet_PB = "";
            }
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM({0}) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE ({1}) AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)< @dNgay  AND PB_PhanBoChiTiet.iTrangThai=1 {2} AND sNG<>'' {3}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM({0})<>0", TruongTien, DKLNS, DKDonVi, DKDuyet_PB);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
            cmdChiTieu.Parameters.AddWithValue("@dNgay", sNgay);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmdChiTieu.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count; i++)
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }
            cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();
            #region  //Ghép DTChiTieu vào dt
            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
            String[] arrCol = sCol.Split(',');

            dt.Columns.Add("ChiTieu", typeof(Decimal));

            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                  dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                  dtChiTieu.Rows[i]["sK"],
                                                  dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                  dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"]
                                                  );
                DataRow[] R = dt.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
                    }
                    dt.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtChiTieu.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < arrCol.Length - 2; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

                                break;
                            }

                        }
                    }

                }

            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dt.DefaultView;
            dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG";
            dt = dv.ToTable();
            #endregion
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
