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
    public class rptDuToan_TongHopDuToanNganSach_NamController : Controller
    {
        // GET: /rptDuToan_TongHopDuToanNganSach_Nam/
        // Edit: Thương       
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_THDTNS_Nam.xls";
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHopDuToanNganSach_Nam.aspx";
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
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            return RedirectToAction("Index", new {iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet )
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String NamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                NamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
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
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_TongHopDuToanNganSach_Nam");
            fr.SetValue("Donvi", "Đơn vị: " + tendv);
            fr.SetValue("nam", NamLamViec);
            //Thu nộp ngân sách nhà quốc phòng
            DataTable dtTN1 = Data(ThuNop(MaND, iID_MaDonVi, "80101",iID_MaTrangThaiDuyet));
            fr.SetValue("TN1CD", GetTong(dtTN1, "TIEN"));
            DataTable dtTN2 = Data(ThuNop(MaND, iID_MaDonVi, "80102", iID_MaTrangThaiDuyet));
            fr.SetValue("TN1QL", GetTong(dtTN2, "TIEN"));
            //Thu nộp ngân sách nhà nước
            DataTable dtTNNN = Data(ThuNop(MaND, iID_MaDonVi, "802", iID_MaTrangThaiDuyet));
            fr.SetValue("TNNN", GetTong(dtTNNN, "TIEN"));
            //Tiền trong nước 
            DataTable tc = Data(PhanChi(MaND, iID_MaDonVi, "1040100", "rTuChi,rDuPhong", iID_MaTrangThaiDuyet));
            
            fr.SetValue("tc", GetTong(tc, "rTuChi"));
            fr.SetValue("choPC", GetTong(tc, "rDuPhong"));
            //Ngoại tệ
            DataTable NT = Data(PhanChi(MaND, iID_MaDonVi, "1040100", "rHangNhap", iID_MaTrangThaiDuyet));
            fr.SetValue("NT", GetTong(NT, "rHangNhap"));
            //Lương phụ cấp trợ cấp tiền ăn
            DataTable _2arTuChi = Data(PhanChi(MaND, iID_MaDonVi, "1010000", "rTuChi", iID_MaTrangThaiDuyet));
            fr.SetValue("2arTuChi", GetTong(_2arTuChi, "rTuChi"));
            //Nghiệp vụ-Tự chi
            DataTable _2brTuChi = Data(PhanChi(MaND, iID_MaDonVi, "1020100", "rTuChi", iID_MaTrangThaiDuyet));
            fr.SetValue("2brTuChi", GetTong(_2brTuChi, "rTuChi"));
            //Nghiệp vụ-Hiện vật
            DataTable _2b1rHienVat = Data(PhanChi(MaND, iID_MaDonVi, "1020100", "rHienVat", iID_MaTrangThaiDuyet));
            fr.SetValue("2b1rHienVat", GetTong(_2b1rHienVat, "rHienVat"));
            //Ngân sách xây dựng cơ bản
            DataTable PL3rPhanCap = Data(PhanChi(MaND, iID_MaDonVi, "1030100", "rTongSo", iID_MaTrangThaiDuyet));
            fr.SetValue("PL3rPhanCap", GetTong(PL3rPhanCap, "rTongSo"));
            //Ngân sách hỗ trợ doanh nghiệp
            DataTable _4arTuChi = Data(PhanChi(MaND, iID_MaDonVi, "1050000", "rTongSo", iID_MaTrangThaiDuyet));
            fr.SetValue("4arTuChi", GetTong(_4arTuChi, "rTongSo"));
            //Phân sử dụng tồn kho
            DataTable dtTK = Data(PhanChi(MaND, iID_MaDonVi, "1040100", "rTonKho", iID_MaTrangThaiDuyet));
            fr.SetValue("tk", GetTong(dtTK, "rTonKho"));
            //Số đã phân cấp cho đơn vị
            DataTable dtPC = Data(PhanChi(MaND, iID_MaDonVi, "1040100", "rPhanCap", iID_MaTrangThaiDuyet));
            fr.SetValue("DPC", GetTong(dtPC, "rPhanCap"));
            //Ngân sách khác
            DataTable dt4b = Data(GetData(MaND, iID_MaDonVi, "109", 3, iID_MaTrangThaiDuyet));
            fr.SetValue("4brTongSo", GetTong(dt4b, "rTongSo"));
            //Ngân sách nhà nước giao
            DataTable dtPL5 = Data(GetData(MaND, iID_MaDonVi, "2", 1, iID_MaTrangThaiDuyet));
            fr.SetValue("NSNG", GetTong(dtPL5, "rTongSo"));
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
        /// Nếu số dòng của data=0 thì thêm 1 dòng trống vào data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable Data(DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                DataRow dR = dt.NewRow();
                dt.Rows.Add(dR);
            }
            return dt;
        }
        /// <summary>
        /// Tính tổng theo cột
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        public long GetTong(DataTable dt, String Field)
        {
            long tong = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tong += long.Parse(dt.Rows[i][Field].ToString().Equals("") ? "0" : dt.Rows[i][Field].ToString());
                }
            }
            return tong;
        }
        /// <summary>
        /// xuất ra PDF
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
                    clsResult.FileName = "DuToan_THDTNS_Nam.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }
            }
        }
        /// <summary>
        /// Xem PDF
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
        /// Xuát ra Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_THDTNS_Nam.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy dữ liệu phần chi cho đơn vị
        /// </summary>
        /// <param name="NamLamViec">Năm chi ngân sách</param>
        /// <param name="iID_MaDonVi">Mã đơn vị hưởng ngân sách</param>
        /// <returns></returns>
        public DataTable PhanChi(String MaND, String iID_MaDonVi, String LNS, String Field,  String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            String[] arrField = Field.Split(',');
            String DKField = "";
            for (int i = 0; i < arrField.Length; i++)
            {
                DKField += "SUM(" + arrField[i] + ") " + arrField[i] + " , ";
            }
            DKField += " ";// AND DT.iNamLamViec=@iNamLamViec
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND DT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else 
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"SELECT {0} DT.iID_MaDonVi
                                        FROM DT_ChungTuChiTiet AS DT
                                        WHERE DT.iTrangThai=1 
	                                          AND DT.iID_MaDonVi=@iID_MaDonVi
	                                          AND DT.sLNS={1}
	                                          {3}
	                                          {2}
	                                          GROUP BY DT.iID_MaDonVi
                                        ", DKField, LNS, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy dữ liệu thu nộp của đơn vị
        /// </summary>
        /// <param name="NamLamViec">Năm thu nộp</param>
        /// <param name="Donvi">Mã đơn vị thu nộp</param>
        /// <param name="LNS">Loại ngân sách thu nộp</param>
        /// <returns></returns>
        public DataTable ThuNop(String MaND, String Donvi, String LNS,  String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();// AND TN.iNamLamViec=@iNamLamViec
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND TN.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"select SUM(TN.rNopNSQP) TIEN  
                                        from TN_ChungTuChiTiet as TN
                                        where TN.iTrangThai=1
	                                         {2}
	                                          AND TN.iID_MaDonVi=@iID_MaDonVi
	                                         {1}
	                                          AND TN.sLNS={0}
                                        GROUP BY TN.sMoTa,TN.sLNS,TN.iID_MaDonVi
                                        ORDER BY TN.sLNS", LNS, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", Donvi);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy dữ liệu ngân sách
        /// </summary>
        /// <param name="NamLamViec">Năm ngân sách</param>
        /// <param name="MaDV">Mã đơn vị</param>
        /// <param name="LNS">loại ngân sách</param>
        /// <param name="len">vị trí cắt</param>
        /// <returns></returns>
        public DataTable GetData(String MaND, String MaDV, String LNS, int len ,String iID_MaTrangThaiDuyet)
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
            String SQL = String.Format(@"SELECT SUM(DT.rTongSo) rTongSo
                                            FROM DT_ChungTuChiTiet AS DT
                                            WHERE DT.iTrangThai=1
                                            AND DT.iID_MaDonVi=@iID_MaDonVi
                                            {2}
                                           {3}
                                            AND SUBSTRING(DT.sLNS,1,{0})='{1}'", len, LNS, ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDV);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị thuộc khối dự toán
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonViTheoNam(ParentID, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoNam(String ParentID, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonvi = getDonVi(MaND, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strLNS = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return strLNS;

        }

        public static DataTable getDonVi(String MaND,String iID_MaTrangThaiDuyet)
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
            String SQL = String.Format(@" SELECT DISTINCT
                                             A.iID_MaDonVi,sTen
                                             FROM
                                             (                               
                                            SELECT iID_MaDonVi,sTenDonVi FROM DT_ChungTuChiTiet 
                                            WHERE iTrangThai=1 {0} {1}
                                            ) as A
                                            INNER JOIN
                                            (
                                            SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec
                                            ) as B
                                            ON A.iID_MaDonVi=B.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
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