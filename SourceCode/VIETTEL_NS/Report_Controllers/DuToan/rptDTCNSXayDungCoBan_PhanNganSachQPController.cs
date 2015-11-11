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
    public class rptDTCNSXayDungCoBan_PhanNganSachQPController : Controller
    {
        // Edit :Le
        // GET: /rptDTCNSXayDungCoBan_PhanNganSachQP/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDTCNSXBCB_NSQP.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDTCNSXDCoBan_NganSachQuocPhong.aspx";
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
        /// EditSubmit
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
        /// Tạo báo cáo
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
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
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
             FlexCelReport fr = new FlexCelReport();
             fr = ReportModels.LayThongTinChuKy(fr, "rptDTCNSXayDungCoBan_PhanNganSachQP");
             LoadData(fr, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("TenDV", tendv);
                fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("ngay", ngay);
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
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public long Tong = 0;
        public DataTable DTCNSXDCoBan_NganSachQuocPhong(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {

            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@"SELECT sM,sTM,sTTM,sNG,dbo.NS_MucLucDuAn.sMaCongTrinh
                                            ,dbo.NS_MucLucDuAn.sTen AS TenDuAn
                                            ,NS_DonVi.sTen as TenDonVi
                                            ,LoaiDuAn.sTen AS TenLoaiDuAn
                                            ,ThamQuyen.sTen AS TenThamQuyen
                                            ,TCDuAn.sTen as TenTinhChatDuAn
                                            ,SUM(ChiTiet.rPhanCap) as rPhanCap
                                            ,ChiTiet.sTenCongTrinh 
                                            ,ChiTiet.sMoTa
                                            FROM  DT_ChungTuChiTiet as ChiTiet 
                                           INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS NS_DonVi On ChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                            INNER JOIN dbo.NS_MucLucDuAn ON ChiTiet.sMaCongTrinh=NS_MucLucDuAn.sMaCongTrinh 
                                            INNER JOIN dbo.DC_DanhMuc AS LoaiDuAn ON NS_MucLucDuAn.iID_LoaiDuAn = LoaiDuAn.iID_MaDanhMuc 
                                            INNER JOIN dbo.DC_DanhMuc AS ThamQuyen ON dbo.NS_MucLucDuAn.iID_MaThamQuyen = ThamQuyen.iID_MaDanhMuc 
                                            INNER JOIN dbo.DC_DanhMuc AS TCDuAn ON dbo.NS_MucLucDuAn.iID_TinhChatDuAn = TCDuAn.iID_MaDanhMuc 
                                            WHERE  ChiTiet.iID_MaDonVi=@iID_MaDonVi  
                                            AND sLNS='1030100' AND sL='460' AND sK='468'  AND ChiTiet.iTrangThai = 1 {1} {0}
                                            GROUP BY ThamQuyen.sTen,LoaiDuAn.sTen,NS_DonVi.sTen,dbo.NS_MucLucDuAn.sMaCongTrinh,ChiTiet.sMoTa
                                            ,ChiTiet.sTenCongTrinh ,NS_MucLucDuAn.sTen,TCDuAn.sTen,sM,sTM,sTTM,sNG
                                            HAVING SUM(ChiTiet.rPhanCap)!=0", ReportModels.DieuKien_NganSach(MaND,"ChiTiet"),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Tong += long.Parse(dt.Rows[i]["rPhanCap"].ToString());
            }
                cmd.Dispose();
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable data = DTCNSXDCoBan_NganSachQuocPhong(MaND, iID_MaDonVi,iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", data, "TenLoaiDuAn,TenThamQuyen,TenTinhChatDuAn,sM", "TenLoaiDuAn,TenThamQuyen,TenTinhChatDuAn,sM,sTM,sTTM,sNG,sMoTa","");
            fr.AddTable("Muc", dtMuc);

            DataTable dtTinhChatDuAn;
            dtTinhChatDuAn = HamChung.SelectDistinct("TCDuAn", dtMuc, "TenLoaiDuAn,TenThamQuyen,TenTinhChatDuAn", "TenLoaiDuAn,TenThamQuyen,TenTinhChatDuAn,sM", "");
            fr.AddTable("TCDuAn", dtTinhChatDuAn);

            DataTable dtThamQuyen;
            dtThamQuyen = HamChung.SelectDistinct("ThamQuyen", dtTinhChatDuAn, "TenLoaiDuAn,TenThamQuyen", "TenLoaiDuAn,TenThamQuyen", "");
            fr.AddTable("ThamQuyen", dtThamQuyen);

            DataTable dtLoaiDuAn;
            dtLoaiDuAn = HamChung.SelectDistinct("LoaiDuAn", dtThamQuyen, "TenLoaiDuAn", "TenLoaiDuAn","");
            fr.AddTable("LoaiDuAn", dtLoaiDuAn);

            dtLoaiDuAn.Dispose();
            dtThamQuyen.Dispose();
            dtTinhChatDuAn.Dispose();
            data.Dispose();
        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
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
                clsResult.FileName = "rptDTCNSXayDungCoBan_PhanNganSachQP.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Xuất báo cáo ra file PDF
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
        /// Hiển thị báo cáo theo định dang PDF
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

        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonViTheoNam(ParentID, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoNam(String ParentID, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
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
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@" SELECT DV.iID_MaDonVi,DV.sTen
                                            FROM DT_ChungTuChiTiet as DT
                                           INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON DT.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE DT.iTrangThai=1 {0} {1} AND sLNS='1030100'
                                            GROUP BY DV.iID_MaDonVi,DV.sTen,sLNS
                                            ORDER BY DV.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND,"DT"),iID_MaTrangThaiDuyet);
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
