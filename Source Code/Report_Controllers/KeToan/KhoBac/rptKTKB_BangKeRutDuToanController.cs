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
using System.Text;
namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKB_BangKeRutDuToanController : Controller
    {
        //
        // GET: /rptKTKB_BangKeRutDuToan/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_1_A4= "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_BangKeRutDuToan_1_A4.xls";
        private const String sFilePath_2_A4 = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_BangKeRutDuToan_2_A4.xls";
        private const String sFilePath_1_A3 = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_BangKeRutDuToan_1_A3.xls";
        private const String sFilePath_2_A3 = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_BangKeRutDuToan_2_A3.xls";
        public ActionResult Index()
        {
               if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_BangKeRutDuToan.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
            }
               else
               {
                   return RedirectToAction("Index", "PermitionMessage");
               }   
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String MaTo = Convert.ToString(Request.Form[ParentID + "_MaTo"]);
            String DVT = Convert.ToString(Request.Form[ParentID + "_DVT"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            DateTime TuNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang1 + "/" + iNgay1);
            DateTime DenNgay =Convert.ToDateTime(iNamLamViec + "/" + iThang2 + "/" + iNgay2);
            String iTrangThai = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            TimeSpan time = DenNgay - TuNgay;
            if(time.Days<0)
            {
                ViewData["PageLoad"] = "0";
            }
            else
            {
                ViewData["PageLoad"] = "1";
            }
            ViewData["iNgay1"] = iNgay1;
            ViewData["iThang1"] = iThang1;
            ViewData["iNgay2"] = iNgay2;
            ViewData["iThang2"] = iThang2;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["MaTo"] = MaTo;
            ViewData["DVT"] = DVT;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iID_MaTrangThaiDuyet"] = iTrangThai;
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_BangKeRutDuToan.aspx";
            return View(sViewPath + "ReportView.aspx");   
        }
        /// <summary>
        /// Danh sách đơn vị có dữ liệu
        /// </summary>
        /// <param name="iThang1"></param>
        /// <param name="iNgay1"></param>
        /// <param name="iThang2"></param>
        /// <param name="iNgay2"></param>
        /// <returns></returns>
        public static DataTable dt_DonVi(String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String iID_MaNguonNganSach,String iID_MaNamNganSach)
        {
            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            String SQL = String.Format(@"SELECT * FROM(
                                        SELECT DISTINCT iID_MaDonVi_Nhan,sTenDonVi_Nhan
                                        FROM KTKB_ChungTuChiTiet
                                        WHERE iTrangThai=1
                                              AND iID_MaNguonNganSach=@iID_MaNguonNganSach 
                                              AND iID_MaNamNganSach=@iID_MaNamNganSach
                                              AND iNamLamViec=@iNamLamViec
                                              AND rDTRut>0
                                              AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                              AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) >= @TuNgay)
                                              AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgay)
                                         )a
                                         INNER JOIN (SELECT iID_MaDonVi,sTenTomTat FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b
                                         ON a.iID_MaDonVi_Nhan=b.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dt_ToIn(String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String iID_MaNguonNganSach,String iID_MaNamNganSach, String KhoGiay)
        {
            DataTable dtDonVi = dt_DonVi(iThang1, iNgay1, iThang2, iNgay2, iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaTo", typeof(String));
            dt.Columns.Add("TenTo", typeof(String));
            if (KhoGiay == "2")
            {
                DataRow R = dt.NewRow();
                dt.Rows.Add(R);
                R[0] = "1";
                R[1] = "Tờ 1";
                int dem = 2;
                for (int i = 10; i < dtDonVi.Rows.Count; i=i+10)
                {
                    DataRow R1 = dt.NewRow();
                    dt.Rows.Add(R1);
                    R1[0] = dem;
                    R1[1] = "Tờ " + dem;
                    dem++;
                }
            }
            else
            {
                DataRow R = dt.NewRow();
                dt.Rows.Add(R);
                R[0] = "1";
                R[1] = "Tờ 1";
                int dem = 2;
                for (int i = 19; i < dtDonVi.Rows.Count; i = i + 19)
                {
                    DataRow R1 = dt.NewRow();
                    dt.Rows.Add(R1);
                    R1[0] = dem;
                    R1[1] = "Tờ " + dem;
                    dem++;
                }
            }
            dt.Dispose();
            dtDonVi.Dispose();
            return dt;
        }
        public static DataTable dt_DVT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDVT", typeof(String));
            dt.Columns.Add("TenDVT", typeof(String));
            DataRow R = dt.NewRow();
            dt.Rows.Add(R);
            R[0] = "1";
            R[1] = "Đồng";
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "2";
            R1[1] = "Nghìn đồng";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "3";
            R2[1] = "Triệu đồng";
            dt.Dispose();

            return dt;
        }
        public static DataTable dt_rptKTKB_BangKeRutDuToan(String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String KhoGiay, String MaTo, String DVT, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            DataTable dtDonVi = dt_DonVi(iThang1, iNgay1, iThang2, iNgay2, iNamLamViec, iID_MaNguonNganSach,iID_MaNamNganSach);
            String DKDonVi_SELECT = "";
            String DKDonVi_CASE = "";
            String DKDonVi_HAVING = "";
            String DKDonVi = "";

            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count;i++)
            {
                iID_MaDonVi += ","+dtDonVi.Rows[i]["iID_MaDonVi"].ToString();
               
            }
            if (iID_MaDonVi.Length > 0)
                iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length-1);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            int length = arrDonVi.Length;
            int SoCotTrang1_A4 = 9;
            int SoCotTrang2_A4 = 10;
            int SoCotTrang1_A3 =18;
            int SoCotTrang2_A3 = 19;
            int ToSo=Convert.ToInt16(MaTo);
            if (KhoGiay == "2")
            {
                if (MaTo == "1")
                {
                    if (length <= SoCotTrang1_A4)
                    {
                        for (int i = 0; i < SoCotTrang1_A4 - length;i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    iID_MaDonVi = "";
                    for (int i = 0; i < SoCotTrang1_A4; i++)
                    {
                        iID_MaDonVi +=","+arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                else
                {
                    length = arrDonVi.Length - SoCotTrang1_A4-SoCotTrang2_A4*(ToSo-2);

                    if (length <= SoCotTrang2_A4)
                    {
                        for (int i = 0; i < SoCotTrang2_A4 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    iID_MaDonVi = "";
                    for (int i = SoCotTrang1_A4 + SoCotTrang2_A4 * (ToSo - 2); i < SoCotTrang2_A4 + SoCotTrang1_A4 + SoCotTrang2_A4 * (ToSo - 2); i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
            }
                //Giay a3
            else
            {
                if (MaTo == "1")
                {
                    if (length <= SoCotTrang1_A3)
                    {
                        for (int i = 0; i < SoCotTrang1_A3 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    iID_MaDonVi = "";
                    for (int i = 0; i < SoCotTrang1_A3; i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                    //to lon 2
                else
                {
                    length = arrDonVi.Length - SoCotTrang1_A3 - SoCotTrang2_A3 * (ToSo - 2);

                    if (length <= SoCotTrang1_A3)
                    {
                        for (int i = 0; i < SoCotTrang2_A3 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = SoCotTrang1_A3 + SoCotTrang2_A3 * (ToSo - 2); i < SoCotTrang2_A3 + SoCotTrang1_A3 + SoCotTrang1_A3 * (ToSo - 2); i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
            }
            String DKDVT = "";
            if (DVT == "1")
            {
                DKDVT = "";
            }
            else if (DVT == "2")
            {
                DKDVT = "/1000 ";
            }
            else
            {
                DKDVT = "/1000000 ";
            }
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DKDonVi_SELECT += ",SUM(DonVi" + i + ") as DonVi" + i;
                DKDonVi_CASE += ",DONVI" + i + "=CASE WHEN iID_MaDonVi_Nhan=@iID_MaDonVi_Nhan" + i + " THEN SUM(rDTRut)"+DKDVT+" ELSE 0 END";
                DKDonVi += "iID_MaDonVi_Nhan=@iID_MaDonVi" + i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sMoTa,SUM(rDTRut){2}  as rDTRut {0}
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sMoTa,SUM(rDTRut) as rDTRut
                                        {1}
                                        FROM KTKB_ChungTuChiTiet
                                        WHERE iTrangThai=1 AND ({3})
                                        AND iNamLamViec=@iNamLamViec
                                        AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                        AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) >= @TuNgay)
                                        AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgay)
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan) a
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", DKDonVi_SELECT, DKDonVi_CASE,DKDVT,DKDonVi);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            for (int i = 0; i < arrDonVi.Length;i++ )
            {
                cmd.Parameters.AddWithValue("iID_MaDonVi_Nhan" + i, arrDonVi[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            dtDonVi.Dispose();
            cmd.Dispose();
            return dt;
        }
        public class RutDuToan
        {
            public String Ngay1 { get; set; }
            public String Ngay2 { get; set; }
            public String SoTo { get; set; }
        }
        public JsonResult Get_objNgayThang(String ParentID,String iNamLamViec,String iID_MaNguonNganSach,String iID_MaNamNganSach, String iNgay1, String iThang1, String iNgay2, String iThang2, String KhoGiay,String MaTo)
        {
            return Json(get_sNgayThang(ParentID, iNamLamViec,iID_MaNguonNganSach, iID_MaNamNganSach,iNgay1, iThang1, iNgay2, iThang2, KhoGiay, MaTo), JsonRequestBehavior.AllowGet);
        }
        public RutDuToan get_sNgayThang(String ParentID, String iNamLamViec,String iID_MaNguonNganSach,String iID_MaNamNganSach, String iNgay1, String iThang1, String iNgay2, String iThang2, String KhoGiay, String MaTo)
        {
            RutDuToan a = new RutDuToan();
            DataTable dtNgay1 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang1), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay1 = new SelectOptionList(dtNgay1, "MaNgay", "TenNgay");
            a.Ngay1 = MyHtmlHelper.DropDownList(ParentID, slNgay1, iNgay1, "iNgay1", "", "style=\"width:80px;padding:2px;border:1px solid #dedede;\" onchange=\"ChonThang()\"");
            dtNgay1.Dispose();
            int SoNgayTrongThang = DateTime.DaysInMonth(Convert.ToInt16(iNamLamViec), Convert.ToInt16(iThang1));
            if (String.IsNullOrEmpty(iNgay1) == false)
            {
                if (Convert.ToInt16(iNgay1) > SoNgayTrongThang)
                    iNgay1 = "1";
            }
            DataTable dtNgay2 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang2), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay2 = new SelectOptionList(dtNgay2, "MaNgay", "TenNgay");
            a.Ngay2 = MyHtmlHelper.DropDownList(ParentID, slNgay2, iNgay2, "iNgay2", "", "style=\"width:80px; padding:2px;border:1px solid #dedede;\" onchange=\"ChonThang()\"");
            DataTable dtTo = dt_ToIn(iThang1, iNgay1, iThang2, iNgay2, iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach, KhoGiay);
            SelectOptionList slTo = new SelectOptionList(dtTo, "MaTo", "TenTo");
            dtTo.Dispose();
            a.SoTo = MyHtmlHelper.DropDownList(ParentID, slTo, MaTo, "MaTo", "", "style=\"width:150px; padding:2px;border:1px solid #dedede;\"");
            return a;
        }
        public ActionResult ViewPDF(String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String KhoGiay, String MaTo, String DVT, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "2")
            {
                if (MaTo == "1")
                    DuongDan = sFilePath_1_A4;
                else
                    DuongDan = sFilePath_2_A4;
            }
            else
            {
                if (MaTo == "1")
                    DuongDan = sFilePath_1_A3;
                else
                    DuongDan = sFilePath_2_A3;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iThang1, iNgay1, iThang2, iNgay2, iNamLamViec, KhoGiay, MaTo, DVT, iID_MaNguonNganSach, iID_MaNamNganSach);
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
        public clsExcelResult ExportToExcel(String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String KhoGiay, String MaTo, String DVT, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "2")
            {
                if (MaTo == "1")
                    DuongDan = sFilePath_1_A4;
                else
                    DuongDan = sFilePath_2_A4;
            }
            else
            {
                if (MaTo == "1")
                    DuongDan = sFilePath_1_A3;
                else
                    DuongDan = sFilePath_2_A3;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iThang1, iNgay1, iThang2, iNgay2, iNamLamViec, KhoGiay, MaTo, DVT, iID_MaNguonNganSach, iID_MaNamNganSach);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangKeTrichThue_TNCN.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }        
        public ExcelFile CreateReport(String path, String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String KhoGiay, String MaTo, String DVT, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Từ ngày " + iNgay1 + "/" + iThang1 + " đến ngày " + iNgay2 + "/" + iThang2 + "/" + iNamLamViec;
            String[] arrTenDonVi = new String[20];
            DataTable dtDonVi = dt_DonVi(iThang1, iNgay1, iThang2, iNgay2, iNamLamViec, iID_MaNguonNganSach, iID_MaNamNganSach);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += "," + dtDonVi.Rows[i]["iID_MaDonVi"].ToString();
            }
            if (iID_MaDonVi.Length > 0)
                        iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
            String[] arrDonVi = iID_MaDonVi.Split(',');
            int length = arrDonVi.Length;
            int SoCotTrang1_A4 = 9;
            int SoCotTrang2_A4 = 10;
            int SoCotTrang1_A3 = 18;
            int SoCotTrang2_A3 = 19;
            int ToSo = Convert.ToInt16(MaTo);
            if (KhoGiay == "2")
            {
                if (MaTo == "1")
                {
                    if (length <= SoCotTrang1_A4)
                    {
                        for (int i = 0; i < SoCotTrang1_A4 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    iID_MaDonVi = "";
                    for (int i = 0; i < SoCotTrang1_A4; i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                else
                {
                    length = arrDonVi.Length - SoCotTrang1_A4 - SoCotTrang2_A4 * (ToSo - 2);

                    if (length <= SoCotTrang2_A4)
                    {
                        for (int i = 0; i < SoCotTrang2_A4 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    iID_MaDonVi = "";
                    for (int i = SoCotTrang1_A4 + SoCotTrang2_A4 * (ToSo - 2); i < SoCotTrang2_A4 + SoCotTrang1_A4 + SoCotTrang2_A4 * (ToSo - 2); i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
            }
            //Giay a3
            else
            {
                if (MaTo == "1")
                {
                    if (length <= SoCotTrang1_A3)
                    {
                        for (int i = 0; i < SoCotTrang1_A3 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    iID_MaDonVi = "";
                    for (int i = 0; i < SoCotTrang1_A3; i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                //to lon 2
                else
                {
                    length = arrDonVi.Length - SoCotTrang1_A3 - SoCotTrang2_A3 * (ToSo - 2);

                    if (length <= SoCotTrang1_A3)
                    {
                        for (int i = 0; i < SoCotTrang2_A3 - length; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = SoCotTrang1_A3 + SoCotTrang2_A3 * (ToSo - 2); i < SoCotTrang2_A3 + SoCotTrang1_A3 + SoCotTrang1_A3 * (ToSo - 2); i++)
                    {
                        iID_MaDonVi += "," + arrDonVi[i];
                    }
                    iID_MaDonVi = iID_MaDonVi.Substring(1, iID_MaDonVi.Length - 1);
                    arrDonVi = iID_MaDonVi.Split(',');
                }
            }
            for (int i = 0; i < dtDonVi.Rows.Count;i++ )
            {
                if (arrDonVi[i] != "-1" && arrDonVi[i]==dtDonVi.Rows[i]["iID_MaDonVi"].ToString())
                {
                    arrTenDonVi[i]=dtDonVi.Rows[i]["sTenTomTat"].ToString();
                }
            }
            DataTable dtDVT = dt_DVT();
            String TenDVT = "";
            for (int i = 0; i < dtDVT.Rows.Count;i++)
            {
                if (DVT == dtDVT.Rows[i]["MaDVT"].ToString())
                {
                    TenDVT = dtDVT.Rows[i]["TenDVT"].ToString();
                    break;
                }
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_BangKeRutDuToan");
            LoadData(fr, iThang1, iNgay1, iThang2, iNgay2, iNamLamViec, KhoGiay,MaTo,DVT,iID_MaNguonNganSach,iID_MaNamNganSach);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            for (int i = 0; i < arrTenDonVi.Length;i++)
            {
                fr.SetValue("DonVi" + i, arrTenDonVi[i]);
            }
            fr.SetValue("TenDVT", TenDVT);
            fr.SetValue("To", MaTo);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iThang1, String iNgay1, String iThang2, String iNgay2, String iNamLamViec, String KhoGiay, String MaTo, String DVT, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            DataTable data = dt_rptKTKB_BangKeRutDuToan(iThang1, iNgay1, iThang2, iNgay2, iNamLamViec,KhoGiay,MaTo,DVT,iID_MaNguonNganSach,iID_MaNamNganSach);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", data, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLoai;
            dtLoai = HamChung.SelectDistinct("dtLoai", dtMuc, "NguonNS,sLNS,sL,sK","NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            fr.AddTable("Loai", dtLoai);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtLoai, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtLoaiNS, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            if (dtNguonNS.Rows.Count == 0)
            {
                DataRow r = dtNguonNS.NewRow();
                dtNguonNS.Rows.Add(r);
            }
            fr.AddTable("NguonNS", dtNguonNS);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtLoai.Dispose();
        }
    }
}
