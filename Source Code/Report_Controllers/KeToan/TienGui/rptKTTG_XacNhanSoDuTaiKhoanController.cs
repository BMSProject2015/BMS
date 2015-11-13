using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKTTG_XacNhanSoDuTaiKhoanController : Controller
    {
        //
        // GET: /rptKTTG_TongHopSoDuVonBangTien/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTG_XacNhanSoDuTaiKhoan.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_XacNhanSoDuTaiKhoan.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String sSoHieuTaiKhoan = Convert.ToString(Request.Form[ParentID + "_sSoHieuTaiKhoan"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String bCheck = Convert.ToString(Request.Form[ParentID + "_bCheck"]);
            ViewData["PageLoad"] = "1";
            ViewData["iThang"] = iThang;
            ViewData["bCheck"] = bCheck;
            ViewData["sSoHieuTaiKhoan"] = sSoHieuTaiKhoan;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_XacNhanSoDuTaiKhoan.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String MaND,String iThang,String iID_MaTrangThaiDuyet,String sSoHieuTaiKhoan,String bCheck)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iThang, iID_MaTrangThaiDuyet, sSoHieuTaiKhoan,bCheck);
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
        public clsExcelResult ExportToExcel(String MaND, String iThang, String iID_MaTrangThaiDuyet, String sSoHieuTaiKhoan, String bCheck)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iThang, iID_MaTrangThaiDuyet, sSoHieuTaiKhoan,bCheck);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTG_XacNhanSoDuTaiKhoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iThang, String iID_MaTrangThaiDuyet, String sSoHieuTaiKhoan, String bCheck)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
             DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Tháng " + iThang + " năm " + iNamLamViec;
            data _data = dtKTTG_XacNhanSoDuTaiKhoan(MaND, iThang, iID_MaTrangThaiDuyet, sSoHieuTaiKhoan);
            Decimal DauKy = 0, DauKy_KB = 0, Tang = 0,Tang_KB=0,Giam = 0,Giam_KB=0,CuoiKy = 0,CuoiKy_KB=0;
            DauKy = _data.DauKy;
            Tang = _data.Tang;
            Giam = _data.Giam;
            CuoiKy = DauKy + Tang - Giam;
            String ChenhLech1 = "", ChenhLech2 = "", ChenhLech3 = "", ChenhLech4 = "";
            if (bCheck == "on")
            {
                DauKy_KB = DauKy;
                Tang_KB = Tang;
                Giam_KB = Giam;
                CuoiKy_KB = CuoiKy;
                ChenhLech1 = "0";
                ChenhLech2 = "0";
                ChenhLech3 = "0";
                ChenhLech4 = "0";
            }
           
            String NgayThang = "Tháng " + iThang + " năm " + iNamLamViec;
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTG_XacNhanSoDuTaiKhoan");
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("DauKy", DauKy);
            fr.SetValue("Tang", Tang);
            fr.SetValue("Giam", Giam);
            fr.SetValue("CuoiKy", CuoiKy);
            fr.SetValue("DauKy_KB", DauKy_KB);
            fr.SetValue("Tang_KB", Tang_KB);
            fr.SetValue("Giam_KB", Giam_KB);
            fr.SetValue("CuoiKy_KB", CuoiKy_KB);
            fr.SetValue("sSoHieuTaiKhoan", sSoHieuTaiKhoan);
            fr.SetValue("NgayThang", NgayThang);
            fr.SetValue("ChenhLech1", ChenhLech1);
            fr.SetValue("ChenhLech2", ChenhLech2);
            fr.SetValue("ChenhLech3", ChenhLech3);
            fr.SetValue("ChenhLech4", ChenhLech4);
            fr.Run(Result);
            return Result;
        }

        public class data
        {
            public Decimal DauKy { get; set; }
            public Decimal Tang { get; set; }
            public Decimal Giam { get; set; }
        }
        public static data dtKTTG_XacNhanSoDuTaiKhoan(String MaND, String iThang, String iID_MaTrangThaiDuyet, String sSoHieuTaiKhoan)
        {
            data _data= new data();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            //Lấy số dư tháng 0 từ kế toán tổng hợp
            String SQL_Thang0 = String.Format(@"SELECT SUM(rNo) as rNo,SUM(rCo) as rCo
FROM(
SELECT iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi='',sNoiDung,sTenDonVi_No,rNo,rCo=0
FROM (
SELECT iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuThu,sNoiDung,iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rNo
FROM KT_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
      AND iTrangThai=1 AND iThangCT=0 AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'  {0} 
GROUP BY iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_No,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich
INNER JOIN (SELECT sKyHieu,sTen
            FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sSoHieuTaiKhoan='{1}') as c
ON b.sKyHieu=c.sKyHieu
UNION

SELECT iNgayCT,iThangCT,sSoChungTuThu='',sSoChungTuChi,sNoiDung,sTenDonVi_Co,rNo=0,rCo
FROM (
SELECT iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuChi,sNoiDung,iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rCo
FROM KT_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
      AND iTrangThai=1 AND iThangCT=0  AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112' {0} 
GROUP BY iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_Co,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich
INNER JOIN (SELECT sKyHieu,sTen
            FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sSoHieuTaiKhoan='{1}') as c
ON b.sKyHieu=c.sKyHieu) as KT
                                    ", DKTrangThaiDuyet, sSoHieuTaiKhoan);
            SqlCommand cmd_Thang0 = new SqlCommand(SQL_Thang0);
            cmd_Thang0.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd_Thang0.Parameters.AddWithValue("@iThang1", iThang);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd_Thang0.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt_Thang0 = Connection.GetDataTable(cmd_Thang0);
            cmd_Thang0.Dispose();
            Decimal sSoDuDauKy_Thang0 = 0;
            if (dt_Thang0.Rows.Count > 0)
            {
                if (dt_Thang0.Rows[0]["rNo"].ToString() == null || dt_Thang0.Rows[0]["rNo"].ToString() == "")
                {
                    dt_Thang0.Rows[0]["rNo"] = 0;
                }
                if (dt_Thang0.Rows[0]["rCo"].ToString() == null || dt_Thang0.Rows[0]["rCo"].ToString() == "")
                {
                    dt_Thang0.Rows[0]["rCo"] = 0;
                }
                sSoDuDauKy_Thang0 = Convert.ToDecimal(dt_Thang0.Rows[0]["rNo"]) - Convert.ToDecimal(dt_Thang0.Rows[0]["rCo"]);
            }


            //Lấy số dư đầu ky
            String SQL_DauKy = String.Format(@"SELECT SUM(rNo) as rNo,SUM(rCo) as rCo
FROM(
SELECT iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi='',sNoiDung,sTenDonVi_No,rNo,rCo=0
FROM (
SELECT iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuThu,sNoiDung,iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rNo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
      AND iTrangThai=1 AND iThangCT<@iThang1 AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'  {0} 
GROUP BY iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_No,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich
INNER JOIN (SELECT sKyHieu,sTen
            FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sSoHieuTaiKhoan='{1}') as c
ON b.sKyHieu=c.sKyHieu
UNION

SELECT iNgayCT,iThangCT,sSoChungTuThu='',sSoChungTuChi,sNoiDung,sTenDonVi_Co,rNo=0,rCo
FROM (
SELECT iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuChi,sNoiDung,iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rCo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
      AND iTrangThai=1 AND iThangCT<@iThang1  AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112' {0} 
GROUP BY iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_Co,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich
INNER JOIN (SELECT sKyHieu,sTen
            FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sSoHieuTaiKhoan='{1}') as c
ON b.sKyHieu=c.sKyHieu) as KT
                                    ", DKTrangThaiDuyet, sSoHieuTaiKhoan);
            SqlCommand cmd = new SqlCommand(SQL_DauKy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang1", iThang);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            Decimal sSoDuDauKy = 0;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["rNo"].ToString() == null || dt.Rows[0]["rNo"].ToString() == "")
                {
                    dt.Rows[0]["rNo"] = 0;
                }
                if (dt.Rows[0]["rCo"].ToString() == null || dt.Rows[0]["rCo"].ToString() == "")
                {
                    dt.Rows[0]["rCo"] = 0;
                }
                sSoDuDauKy = Convert.ToDecimal(dt.Rows[0]["rNo"]) - Convert.ToDecimal(dt.Rows[0]["rCo"]);
            }
            sSoDuDauKy = sSoDuDauKy + sSoDuDauKy_Thang0;
            _data.DauKy = sSoDuDauKy;
            //Lay phat sinh tang giam

            String SQL_TrongKy = String.Format(@"SELECT SUM(rNo) as rNo,SUM(rCo) as rCo
FROM(
SELECT iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi='',sNoiDung,sTenDonVi_No,rNo,rCo=0
FROM (
SELECT iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuThu,sNoiDung,iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rNo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
      AND iTrangThai=1 AND iThangCT=@iThang1 AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'  {0} 
GROUP BY iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_No,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich
INNER JOIN (SELECT sKyHieu,sTen
            FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sSoHieuTaiKhoan='{1}') as c
ON b.sKyHieu=c.sKyHieu

UNION

SELECT iNgayCT,iThangCT,sSoChungTuThu='',sSoChungTuChi,sNoiDung,sTenDonVi_Co,rNo=0,rCo
FROM (
SELECT iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuChi,sNoiDung,iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rCo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
      AND iTrangThai=1 AND iThangCT=@iThang1  AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112' {0} 
GROUP BY iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_Co,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich
INNER JOIN (SELECT sKyHieu,sTen
            FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sSoHieuTaiKhoan='{1}') as c
ON b.sKyHieu=c.sKyHieu
) as KT
                                    ", DKTrangThaiDuyet, sSoHieuTaiKhoan);
            SqlCommand cmd_TrongKy = new SqlCommand(SQL_TrongKy);
            cmd_TrongKy.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd_TrongKy.Parameters.AddWithValue("@iThang1", iThang);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd_TrongKy.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dtTrongKy = Connection.GetDataTable(cmd_TrongKy);
            cmd_TrongKy.Dispose();
            Decimal sTrongKyTang = 0;
            Decimal sTrongKyGiam = 0;
            if (dtTrongKy.Rows.Count > 0)
            {
                if (dtTrongKy.Rows[0]["rNo"].ToString() == null || dtTrongKy.Rows[0]["rNo"].ToString() == "")
                {
                    dtTrongKy.Rows[0]["rNo"] = 0;
                }
                if (dtTrongKy.Rows[0]["rCo"].ToString() == null || dtTrongKy.Rows[0]["rCo"].ToString() == "")
                {
                    dtTrongKy.Rows[0]["rCo"] = 0;
                }
                sTrongKyTang = Convert.ToDecimal(dtTrongKy.Rows[0]["rNo"]);
                sTrongKyGiam = Convert.ToDecimal(dtTrongKy.Rows[0]["rCo"]);
            }
            _data.Tang = sTrongKyTang;
            _data.Giam = sTrongKyGiam;
            return _data;
        }
     
        public static DataTable dsTaiKhoan()
        {
            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT sSoHieuTaiKhoan,sKyHieu+' - '+sTen as sTen
FROM KT_TaiKhoanDanhMucChiTiet
WHERE  iTrangThai=1");
            dt = Connection.GetDataTable(SQL);
            return dt;
        }
    }
}
