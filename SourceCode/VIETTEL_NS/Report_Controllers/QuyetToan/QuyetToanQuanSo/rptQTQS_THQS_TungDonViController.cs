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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQTQS_THQS_TungDonViController : Controller
    {

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TungDonVi.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TungDonVi.aspx";
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
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String iThang = Request.Form[ParentID + "_iThang"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TungDonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String sTenDonVi = "",sTenPhuLuc="";
            if (iThang != "-1")
            {
                iNamLamViec = "Quý " + iThang + " năm " + iNamLamViec;
            }
            else
            {
                iNamLamViec = "Năm " + iNamLamViec;
            }
            if (iID_MaPhongBan == "-1")
            {
                sTenDonVi = "Toàn quân";
                sTenPhuLuc = "PL01a";
            }
            else if (iID_MaPhongBan == "-2")
            {
                sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
                sTenPhuLuc = "PL01c";
            }
            else
            {
                sTenDonVi = "B " + iID_MaPhongBan;
                sTenPhuLuc = "PL01b";
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaDonVi, iID_MaPhongBan, iThang);
            fr = ReportModels.LayThongTinChuKy(fr, "rptQTQS_THQS_TungDonVi");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("sTenPhuLuc", sTenPhuLuc);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        //1020000
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptQTQS_THQS_TungDonVi(String MaND, String iID_MaDonVi,String iID_MaPhongBan,String iThang)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "",DKDV="";
            SqlCommand cmd = new SqlCommand();
            
            
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            
            if (iThang != "-1")
            {
                int ithang1, ithang2;
                if (iThang == "1")
                {
                    ithang1 = 1;
                    ithang2 = 3;
                }
                else if (iThang == "2")
                {
                    ithang1 = 4;
                    ithang2 = 6;
                }
                else if (iThang == "3")
                {
                    ithang1 = 7;
                    ithang2 = 9;
                }
                else 
                {
                    ithang1 = 10;
                    ithang2 = 12;
                }
                DK += " AND iThang_Quy between @iThangQuy1 AND @iThangQuy2 ";
                cmd.Parameters.AddWithValue("@iThangQuy1", ithang1);
                cmd.Parameters.AddWithValue("@iThangQuy2", ithang2);
            }
            if (iID_MaPhongBan != "-2" && iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan=@iiD_MaPhongBan1";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan1", iID_MaPhongBan);
            }
            if (iID_MaPhongBan == "-2")
            {
                DKDV= " AND iID_MaDonVi in("+iID_MaDonVi+")"; 
            }
            //lay dt danh muc
            String SQL = String.Format(@"
SELECT * FROM NS_MucLucQuanSo
WHERE iTrangThai=1 AND sHienThi <>'1'
ORDER BY sKyHieu");
            DataTable dtDanhMuc = Connection.GetDataTable(SQL);
            //Lay chung tu chi tiet số kế hoạch
            SQL = String.Format(@"SELECT sKyHieu,
rSQ_KH=SUM(rSQ_KH),
rHSQBS_KH=SUM(rHSQBS_KH),
rQNCN_KH=SUM(rQNCN_KH),
rCNVQP_KH=SUM(rCNVQP_KH),
rLDHD_KH=SUM(rLDHD_KH),
rSQ=SUM(rSQ),
rHSQBS=SUM(rHSQBS),
rQNCN=SUM(rQNCN),
rCNVQP=SUM(rCNVQP),
rLDHD=SUM(rLDHD)
FROM(
SELECT sKyHieu
,rSQ_KH=SUM(rSQ_KH)
,rHSQBS_KH=SUM(rHSQBS_KH)
,rQNCN_KH=SUM(rQNCN_KH)
,rCNVQP_KH=SUM(rCNVQP_KH)
,rLDHD_KH=SUM(rLDHD_KH)
,rSQ=0
,rHSQBS=0
,rQNCN=0
,rCNVQP=0
,rLDHD=0

 FROM QTQS_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaChungTu IN (SELECT iID_MaChungTu FROM QTQS_ChungTu WHERE iTrangThai=1 AND iLoai=2)  {0} AND iNamLamViec=@iNamLamViec {1} {2} AND iThang_Quy<>0 {3}
GROUP BY sKyHieu



UNION 


SELECT sKyHieu
,rSQ_KH=0
,rHSQBS_KH=0
,rQNCN_KH=0
,rCNVQP_KH=0
,rLDHD_KH=0
,rSQ=SUM(rThieuUy+rTrungUy+rThuongUy+rDaiUy+rThieuTa+rTrungTa+rThuongTa+rDaiTa+rTuong)
,rHSQBS=SUM(rTSQ+rBinhNhi+rBinhNhat+rHaSi+rTrungSi+rThuongSi)
,rQNCN=SUM(rQNCN)
,rCNVQP=SUM(rCNVQP)
,rLDHD=SUM(rLDHD)
 FROM QTQS_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaChungTu IN (SELECT iID_MaChungTu FROM QTQS_ChungTu WHERE iTrangThai=1 AND iLoai<>2)  {0} AND iNamLamViec=@iNamLamViec {1} {2} AND iThang_Quy<>0 {3}
GROUP BY sKyHieu
HAVING SUM(rThieuUy+rTrungUy+rThuongUy+rDaiUy+rThieuTa+rTrungTa+rThuongTa+rDaiTa+rTuong+rTSQ+rBinhNhi+rBinhNhat+rHaSi+rTrungSi+rThuongSi+rQNCN+rCNVQP+rLDHD)<>0) as a
GROUP BY sKyHieu", DKDV, DKPhongBan, DKDonVi, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);


            //Ghep dtChungTuChiTiet vao dtDanhMuc

            String sTruong = "rSQ_KH,rHSQBS_KH,rQNCN_KH,rCNVQP_KH,rLDHD_KH,rSQ,rHSQBS,rQNCN,rCNVQP,rLDHD";
            String[] arrTruong = sTruong.Split(',');
            for (int i = 0; i < arrTruong.Length; i++)
            {
                dtDanhMuc.Columns.Add(arrTruong[i], typeof(Decimal));
            }

            foreach (DataRow drCT in dtChungTuChiTiet.Rows)
            {
                foreach (DataRow dr in dtDanhMuc.Rows)
                {
                    if (Convert.ToString(drCT["sKyHieu"]) == Convert.ToString(dr["sKyHieu"]))
                    {
                        for (int i = 0; i < arrTruong.Length; i++)
                        {
                            dr[arrTruong[i]] = drCT[arrTruong[i]];
                        }
                    }
                }
            }

            //Danh so thu tu
            dtDanhMuc.Columns.Add("STT", typeof(Int32));
            int tg = 0;
            for (int i = 0; i < dtDanhMuc.Rows.Count; i++)
            {
                if (Convert.ToString(dtDanhMuc.Rows[i]["sKyHieu"]) == "000" || Convert.ToString(dtDanhMuc.Rows[i]["sKyHieu"]) == "001" || Convert.ToString(dtDanhMuc.Rows[i]["sKyHieu"]) == "002" || Convert.ToString(dtDanhMuc.Rows[i]["sKyHieu"]) == "2" || Convert.ToString(dtDanhMuc.Rows[i]["sKyHieu"]) == "3" || Convert.ToString(dtDanhMuc.Rows[i]["sKyHieu"]) == "800")
                {
                    tg++;
                    dtDanhMuc.Rows[i]["STT"] = tg;
                    
                      
                    if (i == 2)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[0]["rSQ"])))
                            dtDanhMuc.Rows[0]["rSQ"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[3]["rSQ"])))
                            dtDanhMuc.Rows[3]["rSQ"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[13]["rSQ"])))
                            dtDanhMuc.Rows[13]["rSQ"] = 0;

                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[0]["rHSQBS"])))
                            dtDanhMuc.Rows[0]["rHSQBS"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[3]["rHSQBS"])))
                            dtDanhMuc.Rows[3]["rHSQBS"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[13]["rHSQBS"])))
                            dtDanhMuc.Rows[13]["rHSQBS"] = 0;

                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[0]["rCNVQP"])))
                            dtDanhMuc.Rows[0]["rCNVQP"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[3]["rCNVQP"])))
                            dtDanhMuc.Rows[3]["rCNVQP"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[13]["rCNVQP"])))
                            dtDanhMuc.Rows[13]["rCNVQP"] = 0;

                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[0]["rLDHD"])))
                            dtDanhMuc.Rows[0]["rLDHD"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[3]["rLDHD"])))
                            dtDanhMuc.Rows[3]["rLDHD"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[13]["rLDHD"])))
                            dtDanhMuc.Rows[13]["rLDHD"] = 0;

                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[0]["rQNCN"])))
                            dtDanhMuc.Rows[0]["rQNCN"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[3]["rQNCN"])))
                            dtDanhMuc.Rows[3]["rQNCN"] = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(dtDanhMuc.Rows[13]["rQNCN"])))
                            dtDanhMuc.Rows[13]["rQNCN"] = 0;
                        //dtDanhMuc.Rows[i]["rSQ"] =
                        //    Convert.ToDecimal(dtDanhMuc.Rows[0]["rSQ"]) + Convert.ToDecimal(dtDanhMuc.Rows[3]["rSQ"]) -
                        //                     Convert.ToDecimal(dtDanhMuc.Rows[13]["rSQ"]);
                        //dtDanhMuc.Rows[i]["rHSQBS"] =
                        //    Convert.ToDecimal(dtDanhMuc.Rows[0]["rHSQBS"]) + Convert.ToDecimal(dtDanhMuc.Rows[3]["rHSQBS"]) -
                        //                     Convert.ToDecimal(dtDanhMuc.Rows[13]["rHSQBS"]);
                        //dtDanhMuc.Rows[i]["rCNVQP"] =
                        //    Convert.ToDecimal(dtDanhMuc.Rows[0]["rCNVQP"]) + Convert.ToDecimal(dtDanhMuc.Rows[3]["rCNVQP"]) -
                        //                     Convert.ToDecimal(dtDanhMuc.Rows[13]["rCNVQP"]);
                        //dtDanhMuc.Rows[i]["rQNCN"] =
                        //    Convert.ToDecimal(dtDanhMuc.Rows[0]["rQNCN"]) + Convert.ToDecimal(dtDanhMuc.Rows[3]["rQNCN"]) -
                        //                     Convert.ToDecimal(dtDanhMuc.Rows[13]["rQNCN"]);
                        //dtDanhMuc.Rows[i]["rLDHD"] =
                        //    Convert.ToDecimal(dtDanhMuc.Rows[0]["rLDHD"]) + Convert.ToDecimal(dtDanhMuc.Rows[3]["rLDHD"]) -
                        //                     Convert.ToDecimal(dtDanhMuc.Rows[13]["rLDHD"]);
                    }
                }
                else
                    dtDanhMuc.Rows[i]["STT"] = 0;

            }
            return dtDanhMuc;
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
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            DataRow r;
            DataTable data = DT_rptQTQS_THQS_TungDonVi(MaND, iID_MaDonVi, iID_MaPhongBan, iThang);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaPhongBan, iThang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_1010000_TungDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaPhongBan, iThang);
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

