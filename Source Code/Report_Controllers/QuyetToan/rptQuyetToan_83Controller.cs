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
using System.Text;

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_83Controller : Controller
    {
        //
        // GET: /rptQuyetToan_83_/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_83.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_83.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public long Tong = 0;
        /// <summary>
        /// lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public DataTable rptQuyetToan_83_(String MaND,String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            String DKDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi += "DV.iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "1=1";
            }
            
            String SQL = string.Format(@" SELECT DV.iID_MaDonVi,DV.sTen,SUM(rTuChi) as SoTien
                                            FROM QTA_ChungTuChiTiet as QTA
                                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)  AS DV ON QTA.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE QTA.iTrangThai=1 {2}
                                            AND bLoaiThang_Quy=0 AND iThang_Quy=@Thang_Quy AND sLNS='1010000'
                                            AND {1}  AND ({0}) AND sNG<>''
                                            GROUP BY DV.iID_MaDonVi,DV.sTen
                                            HAVING SUM(rTuChi)!=0 ", DKDonVi,iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND,"QTA"));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
           
         
            dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Tong+=long.Parse(dt.Rows[i]["SoTien"].ToString());
            }
            //thêm dòng trắng vào báo cáo nếu dữ liệu ra ít
            int a = dt.Rows.Count;
            if (a <= 10 && a > 0)
            {
                for (int i = 0; i < 10 - a; i++)
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    dt.Rows.InsertAt(dr, a + 1);
                }
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm lấy các giá trị từ  form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String  Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            ViewData["PageLoad"] = "1";
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_83.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path,String MaND, String Thang_Quy, String iID_MaDonVi,  String iID_MaTrangThaiDuyet)
        {
           
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String tendv = "";
           
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count>0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
           String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_83");
            LoadData(fr,MaND, Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam",ReportModels.LayNamLamViec(MaND));
                fr.SetValue("Thang", Thang_Quy);
                fr.SetValue("Tong", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("TenDV", tendv);
                fr.SetValue("NgayThang", NgayThangNam);
                fr.SetValue("Cap1", BoQuocPhong);
                fr.SetValue("Cap2", QuanKhu);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        private void LoadData(FlexCelReport fr,String MaND, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable data = rptQuyetToan_83_(MaND,Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet);
           
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// dataTable lấy tên đơn vị theo mã
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
        /// Hàm xuất dữ liệu ra  PDF
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet);
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
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND,String Thang_Quy, String iID_MaDonVi,  String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, Thang_Quy, iID_MaDonVi,  iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToanChiNganSach.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm view PDF
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND,String Thang_Quy, String iID_MaDonVi,  String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, Thang_Quy, iID_MaDonVi,  iID_MaTrangThaiDuyet);
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


        [HttpGet]

        public JsonResult ds_DonVi(String ParentID,String MaND, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = HienThiDonViTheoNam(MaND,Thang_Quy,iID_MaTrangThaiDuyet);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, "rptQuyetToan_83");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Hàm lấy dữ liệu theo năm và tháng đổ vào commbox
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable HienThiDonViTheoNam(String MaND,String Thang_Quy,  String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "1=1";
            }
            String SQL = string.Format(@" SELECT QT.iID_MaDonVi,NS_DonVi.sTen
                                            FROM (SELECT iID_MaDonVi FROM QTA_ChungTuChiTiet                                     
                                            WHERE bLoaiThang_Quy=0 {1}
                                            AND sLNS='1010000' AND iThang_Quy=@Thang_Quy AND rTuChi>0
                                            AND iTrangThai=1 AND {0} 
                                            GROUP BY iID_MaDonVi
                                            )as QT
                                            INNER JOIN (SELECT * FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi  on QT.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                            ORDER BY iID_MaDonVi", iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
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
