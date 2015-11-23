using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using System.IO;
using System.Collections;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Controllers.DuToanBS
{
    public class rptDuToanBS_BieuKiem_GomController : Controller
    {
        public static String SQL = "";
        public static SqlCommand cmd;
        public static DataTable dt;
        public static ArrayList data;
        public static String iID_MaDonVi, iID_MaChungTu, sKieuXem, iDonViTinh,iChiTapTrung,dsMaChungTu;

        public string sViewPath = "~/Report_Views/DuToanBS/";
        private const String sFilePathDV1 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV1.xls";
        private const String sFilePathDV2 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV2.xls";
        private const String sFilePathDV3 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV3.xls";
        private const String sFilePathDV4 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV4.xls";
        private const String sFilePathDV5 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV5.xls";
        private const String sFilePathDV6 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV6.xls";
        private const String sFilePathDV7 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV7.xls";
        private const String sFilePathDV8 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_DV8.xls";

        private const String sFilePathNS1 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS1.xls";
        private const String sFilePathNS2 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS2.xls";
        private const String sFilePathNS3 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS3.xls";
        private const String sFilePathNS4 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS4.xls";
        private const String sFilePathNS5 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS5.xls";
        private const String sFilePathNS6 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS6.xls";
        private const String sFilePathNS7 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS7.xls";
        private const String sFilePathNS8 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_BieuKiem_Gom_NS8.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToanBS/rptDuToanBS_BieuKiem_Gom.aspx";
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
            string MaChungTu = Request.Form["rptiID_MaChungTu"]; 
            string MaDonVi = Request.Form["iID_MaDonVi"];
            string iChiTapTrung = Request.Form["DuToanBS" + "_iChiTapTrung"];
            string KieuXem = Request.Form["DuToanBS" + "_sKieuXem"];
            string DonViTinh = Request.Form["DuToanBS" + "_iDonViTinh"];
            return RedirectToAction("ViewPDF", new { MaDonVi = MaDonVi, MaChungTu = MaChungTu, KieuXem = KieuXem, DonViTinh = DonViTinh, ChiTapTrung = iChiTapTrung });
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToanBS_BieuKiem_Gom");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("dNgayChungTu", "Đợt ngày: " + dNgayChungTu);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1,User.Identity.Name));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        public static DataTable DT_rptDuToanBS_BieuKiem_Gom()
        {
            cmd = new SqlCommand();
            String sLNS = "", DKSELECT = "",DKHAVING="";
            data = new ArrayList();

            //Lay sLNS trong chung tu

            string[] arrMaChungTu = dsMaChungTu.Split(',');
            for (int i = 0; i < arrMaChungTu.Length; i++)
            {
                sLNS += Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", arrMaChungTu[i], "sDSLNS"));
                sLNS += ",";
            }
            string dkLNS = "";
            if (!String.IsNullOrEmpty(sLNS))
            {
                string[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    if (!String.IsNullOrEmpty(arrLNS[i]))
                    {
                        dkLNS += " sLNS=@sLNS" + i + " OR ";
                        cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                    }
                }
                dkLNS = dkLNS.Substring(0, dkLNS.Length - 3);
            }
            //Lấy danh sách các trường được nhập
            SQL = String.Format(@"SELECT DISTINCT brTonKho,brTuChi, brChiTapTrung
                    ,brHangNhap,brHangMua,brHienVat,brDuPhong,brPhanCap
                    FROM NS_MucLucNganSach
                    WHERE {0}", dkLNS);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            for(int i=0;i<dt.Rows.Count;i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (Convert.ToBoolean(dt.Rows[i][j]) == true)
                    {
                        string s = dt.Columns[j].ColumnName.Replace("b", "");
                        if(!data.Contains(s))
                            data.Add(s);
                    }
                }
            }
            if (iChiTapTrung == "1")
            {
                data.Add("rChiTapTrung");
            }
            else
            {
                if (data.Count <= 0 || data == null)
                    data.Add("rTuChi");
            }
            if(data.Count>0)
                DKHAVING = "HAVING ";
            for (int i = 0; i < data.Count; i++)
            {
                DKSELECT += String.Format(",COT{1}=SUM({0}/ {2})", data[i], i, iDonViTinh);
                DKHAVING += String.Format("  SUM({0}) <>0 OR", data[i]);
            }
            if (!String.IsNullOrEmpty(DKHAVING))
                DKHAVING = DKHAVING.Substring(0, DKHAVING.Length - 2);

            cmd = new SqlCommand();
            string DKCT = "";
            for (int i = 0; i < arrMaChungTu.Length; i++)
            {
                DKCT += " DTBS_ChungTuChiTiet.iID_MaChungTu = @iID_MaChungTu" + i + " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu"+i, arrMaChungTu[i]);
            }
            if (!String.IsNullOrEmpty(DKCT)) 
            {
                DKCT = DKCT.Substring(0, DKCT.Length - 3);
                DKCT = " AND ( " + DKCT + " ) ";
            }
            SQL = String.Format(@"  SELECT 
                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,
                                        DTBS_ChungTuChiTiet.sMoTa,
                                        DTBS_ChungTuChiTiet.iID_MaDonVi,
                                        sTenDonVi,
                                        sNoiDung,
                                        DTBS_ChungTu.sID_MaNguoiDungTao,
                                        DTBS_ChungTu.dNgayChungTu
                                        {0}
                                    FROM 
                                        DTBS_ChungTuChiTiet INNER JOIN  DTBS_ChungTu 
                                    ON 
                                        DTBS_ChungTuChiTiet.iID_MaChungTu = DTBS_ChungTu.iID_MaChungTu
                                    WHERE 
                                        DTBS_ChungTuChiTiet.iTrangThai=1   AND 
                                        DTBS_ChungTuChiTiet.iID_MaDonVi IN ({1}) {2}
                                    GROUP BY 
                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,
                                        DTBS_ChungTuChiTiet.sMoTa,
                                        DTBS_ChungTuChiTiet.iID_MaDonVi,
                                        sTenDonVi,
                                        sNoiDung,
                                        DTBS_ChungTu.sID_MaNguoiDungTao,
                                        DTBS_ChungTu.dNgayChungTu
                                    {3}", DKSELECT,iID_MaDonVi,DKCT,DKHAVING);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            dt.Columns.Add("TenDot");
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                dt.Rows[i]["TenDot"] = CommonFunction.LayXauNgay(Convert.ToDateTime(dt.Rows[i]["dNgayChungTu"])) + '-' +
                                    Convert.ToString(dt.Rows[i]["sID_MaNguoiDungtao"]);
            }
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
            DataTable dtsM, dtsL, dtsLNS, dtDonVi=null,dtDot= null;
            DataTable data = DT_rptDuToanBS_BieuKiem_Gom();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            if (sKieuXem == "NS") //Neu Kieu xem NS
            {
                dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sMoTa,TenDot", "sLNS,sL,sK,sM,sTM,sTTM,TenDot");
                dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sMoTa,TenDot", "sLNS,sL,sK,sM,sTM,TenDot");
                dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sMoTa,TenDot", "sLNS,sL,sK,sM,TenDot");
                dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sMoTa,TenDot", "sLNS,sL,TenDot");
                dtDot = HamChung.SelectDistinct("dtDot", dtsL, "TenDot", "iID_MaDonVi,sTenDonVi,TenDot", "sLNS,TenDot");
                            }
            else //nếu kiểu xem đơn vi
            {
                dtsTM = HamChung.SelectDistinct("dtsTM", data, "iID_MaDonVi,sLNS,sL,sK,sM,sTM,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sMoTa,TenDot", "sLNS,sL,sK,sM,sTM,sTTM,TenDot");
                dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "iID_MaDonVi,sLNS,sL,sK,sM,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sMoTa,TenDot", "sLNS,sL,sK,sM,sTM,TenDot");
                dtsL = HamChung.SelectDistinct("dtsL", dtsM, "iID_MaDonVi,sLNS,sL,sK,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sMoTa,TenDot", "sLNS,sL,sK,sM,TenDot");
                dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "iID_MaDonVi,sLNS,TenDot", "iID_MaDonVi,sTenDonVi,sLNS,sMoTa,TenDot", "sLNS,sL,TenDot");
                dtDonVi = HamChung.SelectDistinct("dtDonVi", dtsL, "iID_MaDonVi,TenDot", "iID_MaDonVi,sTenDonVi,TenDot");
                dtDot = HamChung.SelectDistinct("dtDot", dtsL, "TenDot", "TenDot");
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtDonVi", dtDonVi);
            fr.AddTable("dtDot", dtDot);

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
        public clsExcelResult ExportToExcel(String MaDonVi, String MaChungTu, String KieuXem, String DonViTinh, String ChiTapTrung)
        {
            clsExcelResult clsResult = new clsExcelResult();
            iID_MaDonVi = MaDonVi;
            iID_MaChungTu = MaChungTu;
            sKieuXem = KieuXem;
            iDonViTinh = DonViTinh;
            iChiTapTrung = ChiTapTrung;
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
        public ActionResult ViewPDF(String MaDonVi, String MaChungTu, String KieuXem, String DonViTinh, String ChiTapTrung)
        {
            HamChung.Language();
            dsMaChungTu = MaChungTu;
            iID_MaDonVi = MaDonVi;
            iID_MaChungTu = MaChungTu;
            sKieuXem = KieuXem;
            iDonViTinh = DonViTinh;
            iChiTapTrung = ChiTapTrung;
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
        public JsonResult Ds_DonVi(string ParentID, string dsMaChungTu)
        {
            String MaND = User.Identity.Name;
            string[] arrChungTu = dsMaChungTu.Split(',');
            DataTable dtDonVi = null;
            for (int i = 0; i < arrChungTu.Length; i++)
            {
                if (dtDonVi != null)
                {
                    dtDonVi.Merge(DuToanBS_ReportModels.dtDonVi_ChungTu(arrChungTu[i]));
                }
                else
                {
                    dtDonVi = DuToanBS_ReportModels.dtDonVi_ChungTu(arrChungTu[i]);
                }
            }
            dtDonVi = HamChung.SelectDistinct("Donvi", dtDonVi, "iID_MaDonVi,TenHT", "iID_MaDonVi,TenHT", "iID_MaDonVi,TenHT");
            string sLNS = "";
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dtDonVi, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        } 
    }
}

