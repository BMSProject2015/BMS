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
using System.Collections.Specialized;
using System.Collections;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_BieuKiem_1010000Controller : Controller
    {
        public static String SQL = "";
        public static SqlCommand cmd;
        public static DataTable dt;
        public static ArrayList data;
        public static String iID_MaDonVi, iID_MaChungTu, sKieuXem, iDonViTinh;

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePathDV1 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV1.xls";
        private const String sFilePathDV2 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV2.xls";
        private const String sFilePathDV3 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV3.xls";
        private const String sFilePathDV4 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV4.xls";
        private const String sFilePathDV5 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV5.xls";
        private const String sFilePathDV6 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV6.xls";
        private const String sFilePathDV7 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV7.xls";
        private const String sFilePathDV8 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_DV8.xls";

        private const String sFilePathNS1 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS1.xls";
        private const String sFilePathNS2 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS2.xls";
        private const String sFilePathNS3 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS3.xls";
        private const String sFilePathNS4 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS4.xls";
        private const String sFilePathNS5 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS5.xls";
        private const String sFilePathNS6 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS6.xls";
        private const String sFilePathNS7 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS7.xls";
        private const String sFilePathNS8 = "/Report_ExcelFrom/DuToan/rptDuToan_BieuKiem_1010000_NS8.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_BieuKiem_1010000.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String MaDonVi = Request.Form["iID_MaDonVi"];
            String MaChungTu = Request.Form["DuToan" + "_iID_MaChungTu"];
            String KieuXem = Request.Form["DuToan" + "_sKieuXem"];
            String DonViTinh = Request.Form["DuToan" + "_iDonViTinh"];
            return RedirectToAction("ViewPDF", new { MaDonVi = MaDonVi, MaChungTu = MaChungTu, KieuXem = KieuXem, DonViTinh = DonViTinh });
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport()
        {
            String MaND = User.Identity.Name;
            String DVT = "";
            String sTen = "";
            String DuongDan = "";
            int count;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String sTenDonVi = "B " + NguoiDung_PhongBanModels.getMoTaPhongBan_NguoiDung(MaND);
            String dNgayChungTu = "";
            dNgayChungTu =
                Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "dNgayChungTu"));
            if (!String.IsNullOrEmpty(dNgayChungTu))
            {
                dNgayChungTu = dNgayChungTu.Substring(0, 2) + "-" + dNgayChungTu.Substring(3, 2) + "-" +
                               dNgayChungTu.Substring(6, 4);
            }

            XlsFile Result = new XlsFile(true);

            FlexCelReport fr = new FlexCelReport();
            LoadData(fr);
            //so cot dc cau hinh theo LNS
            count = data.Count;
            if (sKieuXem == "NS")
            {
                switch (count)
                {
                    case 1: DuongDan = sFilePathNS1; break;
                    case 2: DuongDan = sFilePathNS2; break;
                    case 3: DuongDan = sFilePathNS3; break;
                    case 4: DuongDan = sFilePathNS4; break;
                    case 5: DuongDan = sFilePathNS5; break;
                    case 6: DuongDan = sFilePathNS6; break;
                    case 7: DuongDan = sFilePathNS7; break;
                    case 8: DuongDan = sFilePathNS8; break;
                    default: DuongDan = sFilePathNS1; break;
                }
            }
            else
            {
                switch (count)
                {
                    case 1: DuongDan = sFilePathDV1; break;
                    case 2: DuongDan = sFilePathDV2; break;
                    case 3: DuongDan = sFilePathDV3; break;
                    case 4: DuongDan = sFilePathDV4; break;
                    case 5: DuongDan = sFilePathDV5; break;
                    case 6: DuongDan = sFilePathDV6; break;
                    case 7: DuongDan = sFilePathDV7; break;
                    case 8: DuongDan = sFilePathDV8; break;
                    default: DuongDan = sFilePathDV1; break;
                }
            }

            Result.Open(Server.MapPath(DuongDan));
            for (int i = 0; i < data.Count; i++)
            {
                sTen = "";
                switch (data[i].ToString())
                {
                    case "rTuChi": sTen = "Tự chi"; break;
                    case "rChiTapTrung": sTen = "Chi tập trung"; break;
                    case "rTonKho": sTen = "Tồn kho"; break;
                    case "rHangNhap": sTen = "Hàng nhập"; break;
                    // case "rChiTaiKhoBac": sTen = "Chi tại kho bạc"; break;
                    case "rHangMua": sTen = "Hàng mua"; break;
                    case "rHienVat": sTen = "Hiện vật"; break;
                    case "rDuPhong": sTen = "Dự phòng"; break;
                    case "rPhanCap": sTen = "Phân cấp"; break;
                }
                fr.SetValue("COT" + i, sTen);
            }
            //set DVT
            switch (iDonViTinh)
            {
                case "1": DVT = "Đồng"; break;
                case "1000": DVT = "Nghìn đồng"; break;
                case "1000000": DVT = "Triệu đồng"; break;
                default: DVT = "Đồng"; break;
            }
            fr.SetValue("DVT", DVT);
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_BieuKiem_1010000");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("dNgayChungTu", "Đợt ngày: " + dNgayChungTu);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        public static DataTable DT_rptDuToan_BieuKiem_1010000()
        {
            String sLNS = "", DKSELECT = "",DKHAVING="";
            data = new ArrayList();

            //Lay sLNS trong chung tu

            sLNS = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sDSLNS"));
            if (String.IsNullOrEmpty(sLNS)) sLNS = Guid.Empty.ToString();
            //Lấy danh sách các trường được nhập
            SQL = @"SELECT DISTINCT brTonKho,brTuChi, brChiTapTrung
                    ,brHangNhap,brHangMua,brHienVat,brDuPhong,brPhanCap
                    FROM NS_MucLucNganSach
                    WHERE sLNS=@sLNS";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (Convert.ToBoolean(dt.Rows[0][i]) == true)
                    {
                        data.Add(dt.Columns[i].ColumnName.Replace("b", ""));
                    }
                }
            }
            if (data.Count <= 0 || data == null)
                data.Add("rTuChi");
            if(data.Count>0)
                DKHAVING = "HAVING ";
            for (int i = 0; i < data.Count; i++)
            {
                DKSELECT += String.Format(",COT{1}=SUM({0}/ {2})", data[i], i, iDonViTinh);
                DKHAVING += String.Format("  SUM({0}) <>0 OR", data[i]);
            }
            if (!String.IsNullOrEmpty(DKHAVING))
                DKHAVING = DKHAVING.Substring(0, DKHAVING.Length - 2);

            SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
{1}
 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1   AND iID_MaDonVi IN ({0}) AND iID_MaChungTu=@iID_MaChungTu
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi {2}", iID_MaDonVi, DKSELECT,DKHAVING);
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr)
        {
            DataTable dtsTM;
            DataTable dtsM, dtsL, dtsLNS, dtDonVi= null;
            DataTable data = DT_rptDuToan_BieuKiem_1010000();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            if (sKieuXem == "NS") //Neu Kieu xem NS
            {
                dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
                dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
                dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS", "iID_MaDonVi,sTenDonVi,sLNS,sMoTa", "sLNS,sL");
                            }
            else //nếu kiểu xem đơn vi
            {
                dtsTM = HamChung.SelectDistinct("dtsTM", data, "iID_MaDonVi,sLNS,sL,sK,sM,sTM", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "iID_MaDonVi,sLNS,sL,sK,sM", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
                dtsL = HamChung.SelectDistinct("dtsL", dtsM, "iID_MaDonVi,sLNS,sL,sK", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
                dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "iID_MaDonVi,sLNS", "iID_MaDonVi,sTenDonVi,sLNS,sMoTa", "sLNS,sL");
                dtDonVi = HamChung.SelectDistinct("dtDonVi", dtsL, "iID_MaDonVi", "iID_MaDonVi,sTenDonVi");
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtDonVi", dtDonVi);


            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();

        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaDonVi, String MaChungTu, String KieuXem, String DonViTinh)
        {
            clsExcelResult clsResult = new clsExcelResult();
            iID_MaDonVi = MaDonVi;
            iID_MaChungTu = MaChungTu;
            sKieuXem = KieuXem;
            iDonViTinh = DonViTinh;
            ExcelFile xls = CreateReport();

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangKiem_1010000.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaDonVi, String MaChungTu, String KieuXem, String DonViTinh)
        {
            HamChung.Language();
            iID_MaDonVi = MaDonVi;
            iID_MaChungTu = MaChungTu;
            sKieuXem = KieuXem;
            iDonViTinh = DonViTinh;
            ExcelFile xls = CreateReport();
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
        }
    }
}

