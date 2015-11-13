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


namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanNganSachNhaNuoc5Controller : Controller
    {
        //
        // GET: /rptDuToanNganSachNhaNuoc5/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToanNganSachNhaNuoc5.xls";
        public static String NameFile = "";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToanNganSachNhaNuoc5.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);      
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            return RedirectToAction("Index", new {iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            Result.Open(path);
            String tendv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = DTtendonvi(iID_MaDonVi);
            if (teN.Rows.Count>0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanNganSachNhaNuoc5");
            LoadData(fr,iID_MaDonVi, MaND, iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("PhuLuc", "Phụ Lục Số 5");
                fr.SetValue("TenTieuDe", "DỰ TOÁN NGÂN SÁCH NHÀ NƯỚC GIAO NĂM");
                fr.SetValue("TenDonVi", tendv);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// hàm lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static DataTable DTtendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable data = DT_DuToanNganSachNhaNuoc5(iID_MaDonVi, MaND, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,LoaiNS,sLNS,sM,sTM", "NguonNS,LoaiNS,sLNS,sK,sM,sTM,sL,sMoTa", "sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,LoaiNS,sLNS,sM", "NguonNS,LoaiNS,sLNS,sM,sK,sL,sMoTa", "sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLK;
            dtLK = HamChung.SelectDistinct("LK", dtMuc, "NguonNS,LoaiNS,sLNS,sL", "NguonNS,LoaiNS,sLNS,sL,sM,sK,sMoTa", "sLNS,sL,sK,sM");
            fr.AddTable("LK", dtLK);

            DataTable dtLNS;
            dtLNS = HamChung.SelectDistinct("LNS", dtLK, "NguonNS,LoaiNS,sLNS", "NguonNS,LoaiNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtLNS, "NguonNS,LoaiNS", "NguonNS,LoaiNS,sMoTa", "sLNS,sL", "LoaiNS");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtLoaiNS, "NguonNS", "NguonNS,sMoTa", "sLNS", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);

            data.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
            dtMuc.Dispose();
            dtLK.Dispose();
            dtLNS.Dispose();
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
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
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
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
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTNganSachNhaNuoc5.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Datatable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable DT_DuToanNganSachNhaNuoc5( String iID_MaDonVi, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();

            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }

            String SQL = string.Format(@"SELECT SUBSTRING(a.sLNS,1,3) as NguonNS,
                                        SUBSTRING(a.sLNS,1,5) as LoaiNS,
                                        a.sLNS,a.sM,a.sL,a.sK,a.sTM,a.sTTM
                                        ,a.sNG,a.sMoTa
                                        ,SUM(a.rChiTaiKhoBac) as rChiTaiKhoBac
                                        ,SUM(a.rTuChi) as rTuChi
                                        ,SUM(a.rPhanCap) as rPhanCap
                                        ,SUM(a.rDuPhong)as rDuPhong
                                        , c.sTen as PhongBan 
                                        FROM DT_ChungTuChiTiet a
                                        INNER JOIN NS_PhongBan c ON a.iID_MaPhongBan=c.iID_MaPhongBan
                                        WHERE SUBSTRING(a.sLNS,1,1)='2' and a.iID_MaDonVi=@iID_MaDonVi AND a.iTrangThai=1
                                       {0} {1}
                                        GROUP BY a.sLNS,a.sM,a.sL,a.sK,a.sTM,a.sTTM,a.sNG,a.sMoTa, c.sTen 
                                        HAVING SUM(a.rDuPhong)!=0 OR SUM(a.rPhanCap)!=0 OR SUM(a.rTuChi)!=0 OR SUM(a.rChiTaiKhoBac)!=0", ReportModels.DieuKien_NganSach(MaND,"a"),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        /// <summary>
        /// lấy dữ liệu cho commbox đơn vị
        /// </summary>
        /// <returns></returns>
//        public static DataTable Get_DonVi()
//        {
//            String SQL = string.Format(@"SELECT DV.iID_MaDonVi,DV.sTen
//                                        FROM DT_ChungTuChiTiet as DT
//                                        INNER JOIN NS_DonVi as DV ON DT.iID_MaDonVi=DV.iID_MaDonVi
//                                        WHERE DT.iTrangThai=1 AND iNamLamViec=2012 AND DT.iID_MaTrangThaiDuyet=34 AND SUBSTRING(DT.sLNS,1,3)='206'
//                                        GROUP BY DV.iID_MaDonVi,DV.sTen,SUBSTRING(DT.sLNS,1,3)
//                                        ORDER BY DV.iID_MaDonVi");
//            SqlCommand cmd = new SqlCommand(SQL);
//            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
//            DataTable dt = Connection.GetDataTable(cmd);
//            return dt;
//        }

        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND,String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonViTheoNam(ParentID, MaND, iID_MaDonVi,iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoNam(String ParentID, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonvi = HienThiDonViTheoNam(MaND,iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strLNS = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return strLNS;

        }

        public static DataTable HienThiDonViTheoNam(String MaND,String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND DT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@" SELECT DISTINCT DV.iID_MaDonVi,DV.sTen
                                            FROM DT_ChungTuChiTiet as DT
                                           INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON DT.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE DT.iTrangThai=1 {0} {1} AND SUBSTRING(DT.sLNS,1,1)='2'
                                            GROUP BY DV.iID_MaDonVi,DV.sTen,SUBSTRING(DT.sLNS,1,3)
                                            ORDER BY DV.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND, "DT"), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
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
