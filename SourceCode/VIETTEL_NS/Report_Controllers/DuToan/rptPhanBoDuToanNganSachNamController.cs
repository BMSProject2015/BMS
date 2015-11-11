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
    public class rptPhanBoDuToanNganSachNamController : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptPhanBoDuToanNganSachNam.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptPhanBoDuToanNganSachNam.aspx";
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
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = "~/Report_Views/DuToan/rptPhanBoDuToanNganSachNam.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);


            String sTenDonVi = iID_MaPhongBan;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaPhongBan);
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBoDuToanNganSachNam");
            fr.SetValue("Nam", iNamLamViec);
            if (iID_MaPhongBan != "-1")
                fr.SetValue("sTenDonVi", "B " + sTenDonVi);
            else
            {
                fr.SetValue("sTenDonVi", "");
            }
            for (int i = 1; i <= 11; i++)
            {
                 fr.SetValue("COT"+i, "COT");
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
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
        public static DataTable DT_rptPhanBoDuToanNganSachNam(String MaND, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            int DVT = 1000000;
            String SQL = String.Format(@"SELECT * FROM
(
SELECT iiD_MaDonVi,Loai,
TONG=(SUM(Cot1)+SUM(Cot2)+SUM(Cot3)+SUM(Cot4)+SUM(Cot5)+SUM(Cot6)+SUM(Cot7)+SUM(Cot8)+SUM(Cot9)+SUM(Cot10)+SUM(Cot11))/{3}
,COT1=SUM(Cot1/{3})
,COT2=SUM(COT2/{3})
,COT3=SUM(COT3/{3})
,COT4=SUM(COT4/{3})
,COT5=SUM(COT5/{3})
,COT6=SUM(COT6/{3})
,COT7=SUM(COT7/{3})
,COT8=SUM(COT8/{3})
,COT9=SUM(COT9/{3})
,COT10=SUM(Cot10/{3})
,COT11=SUM(Cot11/{3})
FROM (
SELECT  iID_MaDonVi,Loai=N'I. Phân cấp cho đơn vị'
,COT1=SUM(CASE WHEN sLNS=1020100 AND sM=6600 AND stm=6612 THEN rTuChi+rHangMua ELSE 0 END) 
,COT2=SUM(CASE WHEN sLNS=1020100 AND sM=6600 AND stm=6649 AND sTTM IN(10,20) THEN rTuChi+rHangMua ELSE 0 END) 
,COT3=SUM(CASE WHEN sLNS=1020100 AND sM=7750 AND stm=7799 AND sTTM=10 AND sNG=22 THEN rTuChi+rHangMua ELSE 0 END)
,COT4=SUM(CASE WHEN sLNS=1020100 AND sM=6200  THEN rTuChi+rHangMua ELSE 0 END) 
,COT5=SUM(CASE WHEN sLNS=1020100 AND sM=6250 THEN rTuChi+rHangMua ELSE 0 END) 
,COT6=SUM(CASE WHEN sLNS=1020100 AND sM=6700 THEN rTuChi+rHangMua ELSE 0 END) 
,COT7=SUM(CASE WHEN sLNS=1020100 AND sM=6900 AND stm=6905 AND sTTM=00 AND sNG=65 THEN rTuChi+rHangMua ELSE 0 END) 
,COT8=SUM(CASE WHEN sLNS=1020100 AND sM=6500 AND stm IN (6501,6502) AND sTTM=00 AND sNG=56 THEN rTuChi+rHangMua ELSE 0 END) 
,COT9=SUM(CASE WHEN sLNS=1020100 AND sM=6900 AND stm=6907 AND sTTM=00 AND sNG=56  THEN rTuChi+rHangMua ELSE 0 END) 
,COT10=SUM(CASE WHEN sLNS=1020100 AND sM=7000 AND stm=7001 AND sTTM IN(20,30) THEN rTuChi+rHangMua ELSE 0 END) 
,COT11=0
  FROM DT_ChungTuChiTiet_PhanCap
  WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  {0} {1} {2}
  GROUP BY iID_MaDonVi
  
  UNION
  SELECT iID_MaDonVi,Loai=N'I. Phân cấp cho đơn vị'
  ,COT1=SUM(CASE WHEN sLNS=1020000 AND sM=6600 AND stm=6612 THEN rTuChi+rHangMua ELSE 0 END) 
,COT2=SUM(CASE WHEN sLNS=1020000 AND sM=6600 AND stm=6649 AND sTTM IN(10,20) THEN rTuChi+rHangMua ELSE 0 END) 
,COT3=SUM(CASE WHEN sLNS=1020000 AND sM=7750 AND stm=7799 AND sTTM=10 AND sNG=22 THEN rTuChi+rHangMua ELSE 0 END)
,COT4=SUM(CASE WHEN sLNS=1020000 AND sM=6200  THEN rTuChi+rHangMua ELSE 0 END) 
,COT5=SUM(CASE WHEN sLNS=1020000 AND sM=6250 THEN rTuChi+rHangMua ELSE 0 END) 
,COT6=SUM(CASE WHEN sLNS=1020000 AND sM=6700 THEN rTuChi+rHangMua ELSE 0 END) 
,COT7=SUM(CASE WHEN sLNS=1020000 AND sM=6900 AND stm=6905 AND sTTM=00 AND sNG=65 THEN rTuChi+rHangMua ELSE 0 END) 
,COT8=SUM(CASE WHEN sLNS=1020000 AND sM=6500 AND stm IN (6501,6502) AND sTTM=00 AND sNG=56 THEN rTuChi+rHangMua ELSE 0 END) 
,COT9=SUM(CASE WHEN sLNS=1020000 AND sM=6900 AND stm=6907 AND sTTM=00 AND sNG=56  THEN rTuChi+rHangMua ELSE 0 END) 
,COT10=SUM(CASE WHEN sLNS=1020000 AND sM=7000 AND stm=7001 AND sTTM IN(20,30) THEN rTuChi+rHangMua ELSE 0 END)
,COT11=0
FROM DT_ChungTuChiTiet
  WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  {0} {1} {2}
  GROUP BY iID_MaDonVi
  UNION
  SELECT  iID_MaDonVi,Loai=N'I. Phân cấp cho đơn vị'
,COT1=0
,COT2=0
,COT3=0
,COT4=0
,COT5=0
,COT6=0
,COT7=0
,COT8=0
,COT9=0
,COT10=0
,COT11=SUM(rTuChi)
  FROM DT_ChungTuChiTiet
  WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  {0} {1} {2}
  AND sMaCongTrinh IN (SELECT sMaCongTrinh FROM NS_MucLucDuAn WHERE iTrangThai=1 AND iLoai='2' AND iNamLamViec=@iNamLamViec)
  GROUP BY iID_MaDonVi
  UNION 
  SELECT iID_MaDonVi,Loai=N'II. Tự chi tại ngành'
  ,COT1=SUM(CASE WHEN sLNS=1040100 AND sM=6600 AND stm=6612 THEN rTuChi+rHangMua ELSE 0 END) 
,COT2=SUM(CASE WHEN sLNS=1040100 AND sM=6600 AND stm=6649 AND sTTM IN(10,20) THEN rTuChi+rHangMua ELSE 0 END) 
,COT3=SUM(CASE WHEN sLNS=1040100 AND sM=7750 AND stm=7799 AND sTTM=10 AND sNG=22 THEN rTuChi+rHangMua ELSE 0 END)
,COT4=SUM(CASE WHEN sLNS=1040100 AND sM=6200  THEN rTuChi+rHangMua ELSE 0 END) 
,COT5=SUM(CASE WHEN sLNS=1040100 AND sM=6250 THEN rTuChi+rHangMua ELSE 0 END) 
,COT6=SUM(CASE WHEN sLNS=1040100 AND sM=6700 THEN rTuChi+rHangMua ELSE 0 END) 
,COT7=SUM(CASE WHEN sLNS=1040100 AND sM=6900 AND stm=6905 AND sTTM=00 AND sNG=65 THEN rTuChi+rHangMua ELSE 0 END) 
,COT8=SUM(CASE WHEN sLNS=1040100 AND sM=6500 AND stm IN (6501,6502) AND sTTM=00 AND sNG=56 THEN rTuChi+rHangMua ELSE 0 END) 
,COT9=SUM(CASE WHEN sLNS=1040100 AND sM=6900 AND stm=6907 AND sTTM=00 AND sNG=56  THEN rTuChi+rHangMua ELSE 0 END) 
,COT10=SUM(CASE WHEN sLNS=1040100 AND sM=7000 AND stm=7001 AND sTTM IN(20,30) THEN rTuChi+rHangMua ELSE 0 END)
,COT11=0
FROM DT_ChungTuChiTiet
  WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec  {0} {1} {2} AND (MaLoai='' OR MaLoai='2')
  GROUP BY iID_MaDonVi
UNION
 SELECT iID_MaDonVi,Loai=N'III. Chờ phân cấp'
  ,COT1=SUM(CASE WHEN sLNS=1040100 AND sM=6600 AND stm=6612 THEN rDuPhong ELSE 0 END) 
,COT2=SUM(CASE WHEN sLNS=1040100 AND sM=6600 AND stm=6649 AND sTTM IN(10,20) THEN rDuPhong ELSE 0 END) 
,COT3=SUM(CASE WHEN sLNS=1040100 AND sM=7750 AND stm=7799 AND sTTM=10 AND sNG=22 THEN rDuPhong ELSE 0 END)
,COT4=SUM(CASE WHEN sLNS=1040100 AND sM=6200  THEN rDuPhong ELSE 0 END) 
,COT5=SUM(CASE WHEN sLNS=1040100 AND sM=6250 THEN rDuPhong ELSE 0 END) 
,COT6=SUM(CASE WHEN sLNS=1040100 AND sM=6700 THEN rDuPhong ELSE 0 END) 
,COT7=SUM(CASE WHEN sLNS=1040100 AND sM=6900 AND stm=6905 AND sTTM=00 AND sNG=65 THEN rDuPhong ELSE 0 END) 
,COT8=SUM(CASE WHEN sLNS=1040100 AND sM=6500 AND stm IN (6501,6502) AND sTTM=00 AND sNG=56 THEN rDuPhong ELSE 0 END) 
,COT9=SUM(CASE WHEN sLNS=1040100 AND sM=6900 AND stm=6907 AND sTTM=00 AND sNG=56  THEN rDuPhong ELSE 0 END) 
,COT10=SUM(CASE WHEN sLNS=1040100 AND sM=7000 AND stm=7001 AND sTTM IN(20,30) THEN rDuPhong ELSE 0 END)
,COT11=0
FROM DT_ChungTuChiTiet
  WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec  {0} {1} {2}  AND (MaLoai='' OR MaLoai='2')
  GROUP BY iID_MaDonVi
  ) as a
  GROUP BY iID_MaDonVi,Loai
HAVING SUM(COT1)<>0 OR
SUM(COT2)<>0 OR
SUM(COT3)<>0 OR
SUM(COT4)<>0 OR
SUM(COT5)<>0 OR
SUM(COT6)<>0 OR
SUM(COT7)<>0 OR
SUM(COT8)<>0 OR
SUM(COT9)<>0 OR
SUM(COT10)<>0 OR
SUM(COT11)<>0 
   ) CT
INNER JOIN 
(SELECT iID_MaDonVi as MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)  as b
ON CT.iID_MaDonVi=b.MaDonVi
 ORDER BY Loai, iID_MaDonVi
", DK, DKPhongBan, DKDonVi, DVT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data = DT_rptPhanBoDuToanNganSachNam(MaND, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CongKhaiPhanBoDuToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan);
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

