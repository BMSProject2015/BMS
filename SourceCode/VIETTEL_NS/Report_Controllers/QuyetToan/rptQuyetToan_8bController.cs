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
    public class rptQuyetToan_8bController : Controller
    {
        //
        // GET: /rptQuyetToan_8b_/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_8b.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_8b.aspx";
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
        /// hàm lấy các giá trị trên form
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
            String iID_MaDanhMuc = Request.Form[ParentID + "_iID_MaDanhMuc"];
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_8b.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public long Tong = 0;

        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public DataTable rptQuyetToan_8b_(String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet,String MaND)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(iID_MaDanhMuc))
            {
                iID_MaDanhMuc = Guid.Empty.ToString();
            }

           int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            //nếu là quý
            if (LoaiThangQuy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "iThang_Quy=1 OR iThang_Quy=2 OR iThang_Quy=3";
                        break;
                    case 2: DKThang_Quy = "iThang_Quy=4 OR iThang_Quy=5 OR iThang_Quy=6";
                        break;
                    case 3: DKThang_Quy = "iThang_Quy=7 OR iThang_Quy=8 OR iThang_Quy=9";
                        break;
                    case 4: DKThang_Quy = "iThang_Quy=10 OR iThang_Quy=11 OR iThang_Quy=12";
                        break;
                }
                iThangQuy = iThangQuy * 3;

            }
            else
            {
                DKThang_Quy = "iThang_Quy=@ThangQuy";
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = "1=1";
            }
            String DkDanhMuc = "";
            DataTable dtDanhMuc = LayDSNhomDonVi(Thang_Quy, LoaiThangQuy, iID_MaTrangThaiDuyet, MaND);
            if (iID_MaDanhMuc == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtDanhMuc.Rows.Count; i++)
                {
                    DkDanhMuc += "iID_MaDanhMuc=@iID_MaDanhMuc" + i;
                    if (i < dtDanhMuc.Rows.Count - 1)
                        DkDanhMuc += " OR ";
                }
            }
            else
            {
                DkDanhMuc = "iID_MaDanhMuc=@iID_MaDanhMuc";
            }
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT
                                        B.iID_MaDonVi,B.sTen,SUM(rSoTien) as SoTien
                                        FROM
                                        (
                                        SELECT iID_MaDonVi,SUM(rTuChi) as rSoTien
                                         FROM  QTA_ChungTuChiTiet
                                         WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND  iTrangThai=1 {2} AND {1} AND sNG<>''
                                         AND bLoaiThang_Quy=0 AND ({0})
                                         GROUP BY iID_MaDonVi
                                         ) as QTA
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE  iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi IN ( SELECT iID_MaDonVi FROM NS_DonVi AS DV 
                                         INNER JOIN DC_DanhMuc AS DC ON DV.iID_MaNhomDonVi=DC.iID_MaDanhMuc
                                         WHERE {3}) AND iNamLamViec_DonVi=@iNamLamViec) AS B
                                         ON QTA.iID_MaDonVi=B.iID_MaDonVi
                                         GROUP BY B.iID_MaDonVi,B.sTen
                                         ", DKThang_Quy, DKDuyet, ReportModels.DieuKien_NganSach(MaND), DkDanhMuc);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            if (iID_MaDanhMuc == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtDanhMuc.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDanhMuc" + i, dtDanhMuc.Rows[i]["iID_MaDanhMuc"].ToString());
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iID_MaDanhMuc);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
            }
            int a = dt.Rows.Count;
            if (a <= 12 && a >=0)
            {
                for (int i = 0; i < 12-a; i++)
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
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path,  String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet)
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
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_8b");
            LoadData(fr, Thang_Quy, LoaiThangQuy, iID_MaDanhMuc, iID_MaTrangThaiDuyet,MaND);
            fr.SetValue("Nam",iNamLamViec);
            fr.SetValue("Thang", Thang_Quy);
            fr.SetValue("LoaiThangQuy", LoaiThang_Quy);
            fr.SetValue("Tong", CommonFunction.TienRaChu(Tong).ToString());
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;           
        }
        /// <summary>
        /// Hàm Hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <param name="iNamLamViec"></param>
        private void LoadData(FlexCelReport fr, String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet,String MaND)
        {
            if (String.IsNullOrEmpty(iID_MaDanhMuc))
            {
                iID_MaDanhMuc = Guid.Empty.ToString();
            }
            DataTable data = rptQuyetToan_8b_(Thang_Quy, LoaiThangQuy, iID_MaDanhMuc, iID_MaTrangThaiDuyet,MaND);
            int count = data.Rows.Count;
            if (count > 0 && count < 10)
                for (int i = 0; i < 10 - count; ++i)
                {
                    DataRow r = data.NewRow();
                    data.Rows.Add(r);
                }
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        /// <summary>
        /// Lấy Tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDanhMuc=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        
        /// <summary>
        /// Hàm VIEW PDF
        /// </summary>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),Thang_Quy, LoaiThangQuy, iID_MaDanhMuc, iID_MaTrangThaiDuyet);
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
        /// ExportToExcel
        /// </summary>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="TongHop"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang_Quy, LoaiThangQuy, iID_MaDanhMuc, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BC_8b_QT.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy danh sách nhóm đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_NhomDonVi()
        {
            DataTable dt = HamChung.Lay_dtDanhMuc("Nhomdonvi");
            return dt;
        }


        public static DataTable LayDSNhomDonVi( String Thang_Quy, String LoaiThangQuy, String iID_MaTrangThaiDuyet,String MaND)
        {
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            if (LoaiThangQuy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "iThang_Quy=1 OR iThang_Quy=2 OR iThang_Quy=3";
                        break;
                    case 2: DKThang_Quy = "iThang_Quy=4 OR iThang_Quy=5 OR iThang_Quy=6";
                        break;
                    case 3: DKThang_Quy = "iThang_Quy=7 OR iThang_Quy=8 OR iThang_Quy=9";
                        break;
                    case 4: DKThang_Quy = "iThang_Quy=10 OR iThang_Quy=11 OR iThang_Quy=12";
                        break;
                }
            }
            else
            {
                DKThang_Quy = "iThang_Quy=@ThangQuy";
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = "";
            }
            String SQL = String.Format(@"SELECT DISTINCT c.sTen as TenDM,c.iID_MaDanhMuc 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM QTA_ChungTuChiTiet
	                                           WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {2}  {1}
	                                                 AND ({0}) AND bLoaiThang_Quy=0) a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc) c
                                        ON b.iID_MaNhomDonVi=c.iID_MaDanhMuc", DKThang_Quy, DKDuyet, ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThangQuy != "1")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
           
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            if (dtDonVi.Rows.Count == 0)
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDanhMuc"] = Guid.Empty.ToString();
                R["TenDM"] = "Không có dữ liệu";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            else
            {
                DataRow R = dtDonVi.NewRow();
                R["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000001";
                R["TenDM"] = "Chọn tất cả nhóm";
                dtDonVi.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dtDonVi;
        }
        public JsonResult Ds_Nhom(String ParentID, String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            return Json(obj_Nhom(ParentID,  Thang_Quy, LoaiThangQuy, iID_MaDanhMuc, iID_MaTrangThaiDuyet,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_Nhom(String ParentID,String Thang_Quy, String LoaiThangQuy, String iID_MaDanhMuc, String iID_MaTrangThaiDuyet,String MaND)
        {
            DataTable dt = LayDSNhomDonVi(Thang_Quy, LoaiThangQuy, iID_MaTrangThaiDuyet,MaND);
            if (String.IsNullOrEmpty(iID_MaDanhMuc))
            {
                iID_MaDanhMuc = Guid.Empty.ToString();
            }
            SelectOptionList slNhomDonVi = new SelectOptionList(dt, "iID_MaDanhMuc", "TenDM");
            String strDonVi = MyHtmlHelper.DropDownList(ParentID, slNhomDonVi, iID_MaDanhMuc, "iID_MaDanhMuc", "", "class=\"input1_2\" style=\"width: 100%\"");
            return strDonVi;
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
