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
    public class rptDuToan_1030100_TongHopController : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_1030100_TongHop.xls";
       

        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1030100_TongHop.aspx";
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
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_1030100_TongHop.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);


            String sTenDonVi = "B -  " ;
           
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            Result.MergeCells(3, 1, 3, 2);
            Result.MergeCells(3, 3, 3, 5);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND);
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_1030100_TongHop");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Cap2", sTenDonVi);
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
        public static DataTable DT_rptDuToan_1030100_TongHop(String MaND)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            int DVT = 1000;

            String SQL = "";
            SQL = String.Format(@"SELECT iID_MaDonVi,sTenDonVi,sMaCongTrinh,REPLACE(sTenCongTrinh,sMaCongTrinh+' - ','') as sTenCongTrinh
,SUBSTRING(sMaCongTrinh,5,1) as Loai
,SUBSTRING(sMaCongTrinh,6,1) as ThamQuyen
,SUBSTRING(sMaCongTrinh,7,1) as TinhChat
,rTuChi=SUM(rTuChi/{3})
,sMoTa=''
 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1  AND sLNS LIKE '1030100%' AND SUBSTRING(sMaCongTrinh,5,1) IN (2,4) AND iNamLamViec=@iNamLamViec
  {0} {1} {2}
 GROUP BY iID_MaDonVi,sTenDonVi,sMaCongTrinh,sTenCongTrinh
ORDER BY iID_MaDonVi,sMaCongTrinh"
 , "", DKPhongBan, DKDonVi, DVT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
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
        private void LoadData(FlexCelReport fr, String MaND)
        {
            DataRow r;
            DataTable data = DT_rptDuToan_1030100_TongHop(MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTinhChat = HamChung.SelectDistinct("dtTinhChat", data, "iID_MaDonVi,Loai,ThamQuyen,TinhChat",
                                                           "iID_MaDonVi,sTenDonVi,Loai,ThamQuyen,TinhChat,sMoTa");

            DataTable dtThamQuyen = HamChung.SelectDistinct("dtThamQuyen", dtTinhChat, "iID_MaDonVi,Loai,ThamQuyen",
                                                            "iID_MaDonVi,sTenDonVi,Loai,ThamQuyen,sMoTa");

            DataTable dtLoai = HamChung.SelectDistinct("dtLoai", dtThamQuyen, "iID_MaDonVi,Loai", "iID_MaDonVi,sTenDonVi,Loai,sMoTa");

            DataTable dtDonVi = HamChung.SelectDistinct("dtDonVi", dtLoai, "iID_MaDonVi", "iID_MaDonVi,sTenDonVi");
            //Lay mo ta
            String sMoTa = "";
            DataTable dtThamQuyenDM = DanhMucModels.DT_DanhMuc_All("ThamQuyenCongTrinhDuAn");
            DataTable dtLoaiCTDM = DanhMucModels.DT_DanhMuc_All("LoaiCongTrinhDuAn");
            DataTable dtTinhChatCTDM = DanhMucModels.DT_DanhMuc_All("TinhChatCongTrinhDuAn");
            //dttinh chat

            for (int i = 0; i < dtTinhChat.Rows.Count; i++)
            {
                r = dtTinhChat.Rows[i];
                for (int j = 0; j < dtTinhChatCTDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["Loai"]) == Convert.ToString(dtTinhChatCTDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtTinhChatCTDM.Rows[j]["sTen"]);
                }


                r["sMoTa"] = sMoTa;
            }

            // dtTham Quyen

            for (int i = 0; i < dtThamQuyen.Rows.Count; i++)
            {
                r = dtThamQuyen.Rows[i];
                for (int j = 0; j < dtThamQuyenDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["Loai"]) == Convert.ToString(dtThamQuyenDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtThamQuyenDM.Rows[j]["sTen"]);
                }

                r["sMoTa"] = sMoTa;
            }

            //dt Loai

            for (int i = 0; i < dtLoai.Rows.Count; i++)
            {
                r = dtLoai.Rows[i];
                for (int j = 0; j < dtLoaiCTDM.Rows.Count; j++)
                {
                    if (Convert.ToString(r["Loai"]) == Convert.ToString(dtLoaiCTDM.Rows[j]["sTenKhoa"]))
                        sMoTa = Convert.ToString(dtLoaiCTDM.Rows[j]["sTen"]);
                }

                r["sMoTa"] = sMoTa;
            }
            fr.AddTable("dtDonVi", dtDonVi);
            fr.AddTable("dtTinhChat", dtTinhChat);
            fr.AddTable("dtThamQuyen", dtThamQuyen);
            fr.AddTable("dtLoai", dtLoai);


            data.Dispose();
            dtDonVi.Dispose();
            dtTinhChat.Dispose();
            dtThamQuyen.Dispose();
            dtLoai.Dispose();
          
        }
      
        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_1020200_TongHop.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND);
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

