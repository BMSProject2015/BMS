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
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.DuToan
{

    public class rptDTCNSQP_NganSachHoTroDoanhNghiep_CucController : Controller
    {
        
        // EDIT MaND, Trang thai duyet: LE
        // GET: /rptDTCNSQP_NganSachHoTroDoanhNghiepTheoDonVi/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDTCNSQP_NganSachHoTroDoanhNghiep_Cuc.xls";

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
             {
              ViewData["path"] = "~/Report_Views/DuToan/rptDTCNSQP_NganSachHoTroDoanhNghiep_Cuc.aspx";          
              return View(sViewPath + "ReportView.aspx");
             }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
                       
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi,iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// CreateReport
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {

            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
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
              FlexCelReport fr = new FlexCelReport();
              fr = ReportModels.LayThongTinChuKy(fr, "rptDTCNSQP_NganSachHoTroDoanhNghiep_Cuc");
              LoadData(fr, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("TenDV", tendv);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.Run(Result);
                return Result;
            
        }
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// tạo các range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyets)
        {
            DataTable data = DTCNSQP_NganSachHoTroDoanhNghiep(MaND, iID_MaDonVi, iID_MaTrangThaiDuyets);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            data.Dispose();
            dtTieuMuc.Dispose();
            dtMuc.Dispose();
        }

        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
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
        /// View PDF
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
        /// Xuất ra Excel
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
                clsResult.FileName = "DTCNSQP_NSHoTroTheoDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        /// <summary>
        /// DTCNSQP_NganSachHoTroDoanhNghiep
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable DTCNSQP_NganSachHoTroDoanhNghiep(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,rTuChi, rHangNhap,rDuPhong
                                         FROM (SELECT iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi) as rTuChi,SUM(rHangNhap) as rHangNhap,SUM(rDuPhong) as rDuPhong
                                                FROM DT_ChungTuChiTiet 
                                                WHERE sLNS='1050000' and sL='460' and sK='468'  AND iTrangThai = 1 {0} {1}
                                                GROUP BY iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa
                                                HAVING SUM(rTuChi) <>0 OR SUM(rHangNhap)<>0
                                                ) as a
                                        INNER JOIN (SELECT * FROM NS_DonVi WHERE  iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND,"DT_ChungTuChiTiet"),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
          //  cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DSDonVi(ParentID, MaND, iID_MaDonVi,iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Load đơn vị theo năm và mã loại ngân sách
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String ParentID, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            String dsDonVi = "";
            DataTable dtDonVi = HienThiDonViTheoNam(MaND,iID_MaTrangThaiDuyet);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
        public static DataTable HienThiDonViTheoNam(String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND DT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@" SELECT DV.iID_MaDonVi,DV.sTen
                                            FROM DT_ChungTuChiTiet as DT
                                            INNER JOIN NS_DonVi as DV ON DT.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE DT.iTrangThai=1 {0} {1} AND sLNS='1050000'
                                            GROUP BY DV.iID_MaDonVi,DV.sTen,sLNS
                                            ORDER BY DV.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND,"DT"), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
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
