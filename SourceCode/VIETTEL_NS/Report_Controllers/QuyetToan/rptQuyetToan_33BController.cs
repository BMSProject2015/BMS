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
    public class rptQuyetToan_33BController : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_33B.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_33B.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Edit Submit: nhận giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String Quy = Convert.ToString(Request.Form[ParentID + "_MaQuy"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["Quy"] = Quy;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_33B.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi)
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
            String TenDV;
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }
            if (iID_MaDonVi == "-1")
            {
                TenDV = "";
            }
            String Quy1 = "", Quy2 = "", Quy3 = "", Quy4 = "";
            if (Quy == "1")
            {
                Quy1 = "Quý 1";
            }
            else if (Quy == "2")
            {
                Quy1 = "Quý 1";
                Quy2 = "Quý 2";
            }
            else if (Quy == "3")
            {
                Quy1 = "Quý 1";
                Quy2 = "Quý 2";
                Quy3 = "Quý 3";
            }
            else if (Quy == "4")
            {
                Quy1 = "Quý 1";
                Quy2 = "Quý 2";
                Quy3 = "Quý 3";
                Quy4 = "Quý 4";
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_33B");
            LoadData(fr, iID_MaTrangThaiDuyet, Quy, iID_MaDonVi,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", Quy);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Phong", Phong);
            fr.SetValue("Quy1", Quy1);
            fr.SetValue("Quy2", Quy2);
            fr.SetValue("Quy3", Quy3);
            fr.SetValue("Quy4", Quy4);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Lấy Data fill ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi,String MaND)
        {
            DataTable data = QT_ThuongXuyen33B(iID_MaTrangThaiDuyet, Quy, iID_MaDonVi,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            //Lấy các group
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
        /// xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Quy, iID_MaDonVi);
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
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Quy, iID_MaDonVi);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_33B.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="Quy">Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Quy, iID_MaDonVi);
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
        /// Bắt sự kiện onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="Quy"> Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, Quy, iID_MaDonVi,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String iID_MaTrangThaiDuyet, String Quy, String iID_MaDonVi,String MaND)
        {
            String dsDonVi = "";
            DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet, Quy,MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi","", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        /// <summary>
        /// lấy danh sách đơn vị theo Năm và Quý có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iID_MaTrangThaiDuyet, String Quy,String MaND)
        {
            String dsThang = "";
            if (Quy == "1")
            {
                dsThang = String.Format(@"(iThang_Quy<=3)");
            }
            if (Quy == "2")
            {
                dsThang = String.Format(@"(iThang_Quy<=6)");
            }
            if (Quy == "3")
            {
                dsThang = String.Format(@"(iThang_Quy<=9)");
            }
            if (Quy == "4")
            {
                dsThang = String.Format(@"(iThang_Quy<=12)");
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
            String SQL = String.Format(@"SELECT NS_DonVi.sTen,QT.iID_MaDonVi
                                         FROM (SELECT iID_MaDonVi FROM QTA_ChungTuChiTiet                                     
                                         WHERE {0} AND bLoaiThang_Quy=0 {1} AND iTrangThai=1 {2} AND sLNS=1010000
                                         GROUP BY iID_MaDonVi
                                         )as QT
					                    INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi on QT.iID_MaDonVi=NS_DonVi.iID_MaDonVi
				                        ORDER BY iID_MaDonVi                                        
                                        ", dsThang,ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtDonVi.Rows.Count > 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả";
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
        /// <summary>
        /// data quyết toán thường xuyên 33B
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable QT_ThuongXuyen33B(String iID_MaTrangThaiDuyet,String Quy,String iID_MaDonVi,String MaND)
        {
            DataTable dt;
            String s1 = "", s2 = "", s3 = "";
            if (Quy == "1")
            {
                s1 = "-1";
                s2 = "-1";
                s3 = "-1";             
            }
            if (Quy == "2")
            {
                s1 = "4,5,6";
                s2 = "-1";
                s3 = "-1";               
            }
            if (Quy == "3")
            {
                s1 = "4,5,6";
                s2 = "7,8,9";
                s3 = "-1";             
            }
            if (Quy == "4")
            {
                s1 = "4,5,6";
                s2 = "7,8,9";
                s3 = "10,11,12";              
            }

            DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet, Quy,MaND);
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
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(Quy1) AS Quy1,SUM(Quy2) AS Quy2,SUM(QUY3) AS QUY3,SUM(QUY4) AS QUY4 
                                        FROM (SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,QUY1=Case WHEN (iThang_Quy<=3) THEN SUM(rTuChi) ELSE 0 END
                                        ,QUY2=Case WHEN (iThang_Quy IN ({0})) THEN SUM(rTuChi) ELSE 0 END
                                        ,QUY3=Case WHEN (iThang_Quy IN ({1})) THEN SUM(rTuChi) ELSE 0 END
                                        ,QUY4=Case WHEN (iThang_Quy IN ({2})) THEN SUM(rTuChi) ELSE 0 END	
                                        FROM QTA_ChungTuChiTiet as QTG                                     
                                        WHERE sLNS='1010000'AND sL='460' AND sK='468' AND sNG<>'' {4} {3} AND iTrangThai=1 AND bLoaiThang_Quy=0 {5}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        HAVING SUM(rTuChi)>0) as TB
                                        GROUP BY TB.NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(Quy1)<>0 or SUM(Quy2)<>0 OR SUM(Quy3)<>0 OR SUM(Quy4)<>0
                                        ORDER BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG
                                       ", s1, s2, s3, DKDonVi,ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
           
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
