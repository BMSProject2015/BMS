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

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_1030100_TungDonViController : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_1030100_TungDonVi.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1030100_TungDonVi.aspx";
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
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1030100_TungDonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);


            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaDonVi);
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_1030100_TungDonVi");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        //1020200
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptDuToan_1030100_TungDonVi(String MaND, String iID_MaDonVi)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            int DVT = 1000;

            String SQL = "";
            SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,a.sMaCongTrinh,sTen,iLoai,iThamQuyen,iTinhChat,iNhom,
rTuChi
FROM 
(
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,sMaCongTrinh,REPLaCE(sTenCongTrinh,sMaCongTrinh+' - ','') as sTenCongTrinh
,rTuChi=SUM(rTuChi/{3})

 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1  AND sLNS LIKE '1030100%' 
 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec {0} {1} {2}
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,sMaCongTrinh,sTenCongTrinh
 )a
 INNER JOIN 
 (SELECT * FROM NS_MucLucDuAn WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec) b
 ON a.sMaCongTrinh=b.sMaCongTrinh
 
ORDER BY sTen,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa"
 , "", DKPhongBan, DKDonVi, DVT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DataTable dt = Connection.GetDataTable(cmd);
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
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi)
        {
            DataRow r;
            DataTable data = DT_rptDuToan_1030100_TungDonVi(MaND, iID_MaDonVi);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsNG = HamChung.SelectDistinct("dtsNG", data,
                                                      "iLoai,iThamQuyen,iTinhChat,iNhom,sLNS,sL,sK,sM,sTM,sTTM,sNG",
                                                      "iLoai,iThamQuyen,iTinhChat,iNhom,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "");
            DataTable dtNhom = HamChung.SelectDistinct("dtNhom", dtsNG, "iLoai,iThamQuyen,iTinhChat,iNhom",
                                                           "iLoai,iThamQuyen,iTinhChat,iNhom,sMoTa");
            DataTable dtTinhChat = HamChung.SelectDistinct("dtTinhChat", dtNhom, "iLoai,iThamQuyen,iTinhChat",
                                                           "iLoai,iThamQuyen,iTinhChat,sMoTa");

            DataTable dtThamQuyen = HamChung.SelectDistinct("dtThamQuyen", dtTinhChat, "iLoai,iThamQuyen",
                                                            "iLoai,iThamQuyen,sMoTa");

            DataTable dtLoai = HamChung.SelectDistinct("dtLoai", dtThamQuyen, "iLoai", "iLoai,sMoTa");



            //Lay mo ta
            String sMoTa = "";
            DataTable dtThamQuyenDM = DanhMucModels.DT_DanhMuc_All("ThamQuyenCongTrinhDuAn");
            DataTable dtLoaiCTDM = DanhMucModels.DT_DanhMuc_All("LoaiCongTrinhDuAn");
            DataTable dtTinhChatCTDM = DanhMucModels.DT_DanhMuc_All("TinhChatCongTrinhDuAn");
            DataTable dtNhomCTDM = DanhMucModels.DT_DanhMuc_All("NhomCongTrinhDuAn");
            //dttinh chat

            for (int i = 0; i < dtTinhChat.Rows.Count; i++)
            {
                sMoTa = "";
                r = dtTinhChat.Rows[i];
                for (int j = 0; j < dtTinhChatCTDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["iTinhChat"]) == Convert.ToString(dtTinhChatCTDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtTinhChatCTDM.Rows[j]["sTen"]);
                }


                r["sMoTa"] = sMoTa;
            }

            // dtTham Quyen

            for (int i = 0; i < dtThamQuyen.Rows.Count; i++)
            {
                sMoTa = "";
                r = dtThamQuyen.Rows[i];
                for (int j = 0; j < dtThamQuyenDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["iThamQuyen"]) == Convert.ToString(dtThamQuyenDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtThamQuyenDM.Rows[j]["sTen"]);
                }

                r["sMoTa"] = sMoTa;
            }

            //dt Loai

            for (int i = 0; i < dtLoai.Rows.Count; i++)
            {
                sMoTa = "";
                r = dtLoai.Rows[i];
                for (int j = 0; j < dtLoaiCTDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["iLoai"]) == Convert.ToString(dtLoaiCTDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtLoaiCTDM.Rows[j]["sTen"]);
                }

                r["sMoTa"] = sMoTa;
            }
            //nhom
            for (int i = 0; i < dtNhom.Rows.Count; i++)
            {
                sMoTa = "";
                r = dtNhom.Rows[i];
                for (int j = 0; j < dtNhomCTDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["iNhom"]) == Convert.ToString(dtNhomCTDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtNhomCTDM.Rows[j]["sTen"]);
                }

                r["sMoTa"] = sMoTa;
            }
            fr.AddTable("dtsNG", dtsNG);
            fr.AddTable("dtNhom", dtNhom);
            fr.AddTable("dtTinhChat", dtTinhChat);
            fr.AddTable("dtThamQuyen", dtThamQuyen);
            fr.AddTable("dtLoai", dtLoai);


            data.Dispose();
            dtsNG.Dispose();
            dtTinhChat.Dispose();
            dtThamQuyen.Dispose();
            dtLoai.Dispose();
            dtNhom.Dispose();
        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_1020200_TungDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi);
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

