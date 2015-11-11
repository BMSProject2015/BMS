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
    public class rptKTTG_SoChiTietTienGuiController : Controller
    {
        //
        // GET: /rptKTTG_TongHopSoDuVonBangTien/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTG_SoChiTietTienGui.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_SoChiTietTienGui.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
           
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            if(Convert.ToInt16(iThang1)>Convert.ToInt16(iThang2))
                ViewData["PageLoad"] = "0";
            else
                ViewData["PageLoad"] = "1";
            ViewData["iThang1"] = iThang1;
            ViewData["iThang2"] = iThang2;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_SoChiTietTienGui.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String MaND,String iThang1,String iThang2, String iID_MaTrangThaiDuyet,String iID_MaTaiKhoan)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iThang1, iThang2, iID_MaTrangThaiDuyet, iID_MaTaiKhoan);
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
        public clsExcelResult ExportToExcel(String MaND, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaTaiKhoan)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iThang1, iThang2, iID_MaTrangThaiDuyet, iID_MaTaiKhoan);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTG_SoChiTietTienGui.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaTaiKhoan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
             DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String TenTaiKhoan="";
            DataTable dtTaiKhoan = dsTaiKhoan();
            for (int i = 0; i < dtTaiKhoan.Rows.Count;i++ )
            {
                if (iID_MaTaiKhoan == dtTaiKhoan.Rows[i]["sKyHieu"].ToString())
                    TenTaiKhoan = dtTaiKhoan.Rows[i]["sTen"].ToString();
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Từ tháng " + iThang1 + " đến tháng "+iThang2+" năm " + iNamLamViec;
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTG_SoChiTietTienGui");
            LoadData(fr, MaND, iThang1, iThang2, iID_MaTrangThaiDuyet,iID_MaTaiKhoan);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("TenTaiKhoan", TenTaiKhoan);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaTaiKhoan)
        {
            DataTable data = dtKTTG_TongHopThuVonBangTien(MaND, iThang1, iThang2, iID_MaTrangThaiDuyet, iID_MaTaiKhoan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            DataTable data1 = dtDauKy(MaND, iThang1, iThang2, iID_MaTrangThaiDuyet, iID_MaTaiKhoan);
            data.TableName = "DauKy";
            fr.AddTable("DauKy", data1);
            data1.Dispose();

            DataTable data2 = dtLuyKe(MaND, iThang1, iThang2, iID_MaTrangThaiDuyet, iID_MaTaiKhoan);
            data.TableName = "LuyKe";
            fr.AddTable("LuyKe", data2);
            data2.Dispose();
        }
        public static DataTable dtKTTG_TongHopThuVonBangTien(String MaND, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaTaiKhoan)
        {
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
            String SQL = String.Format(@"SELECT iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi,sNoiDung,sTenDonVi_No as sTenDonVi,SUM(rNo) as rNo,SUM(rCo) as rCo
FROM(
SELECT iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi='',sNoiDung,sTenDonVi_No,rNo,rCo=0
FROM (
SELECT iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuThu,sNoiDung,iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rNo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
      AND iTrangThai=1 AND iThangCT>=@iThang1 AND iThangCT<=@iThang2  AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'  {0} 
GROUP BY iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_No,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich

UNION

SELECT iNgayCT,iThangCT,sSoChungTuThu='',sSoChungTuChi,sNoiDung,sTenDonVi_Co,rNo=0,rCo
FROM (
SELECT iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuChi,sNoiDung,iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rCo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
      AND iTrangThai=1 AND iThangCT>=@iThang1 AND iThangCT<=@iThang2  AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112' {0} 
GROUP BY iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_Co,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich) as KT
GROUP BY iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi,sNoiDung,sTenDonVi_No
                                    ", DKTrangThaiDuyet,iID_MaTaiKhoan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dtDauKy(String MaND, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaTaiKhoan)
        {
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
            String SQL = String.Format(@"SELECT SUM(rNo) as rNo,SUM(rCo) as rCo
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
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich

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
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich) as KT
                                    ", DKTrangThaiDuyet,iID_MaTaiKhoan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
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
            }
            //Lấy số dư tháng 0
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
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich

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
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich) as KT
                                    ", DKTrangThaiDuyet, iID_MaTaiKhoan);
            SqlCommand cmd_Thang0 = new SqlCommand(SQL_Thang0);
            cmd_Thang0.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd_Thang0.Parameters.AddWithValue("@iThang1", iThang1);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd_Thang0.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt_Thang0 = Connection.GetDataTable(cmd_Thang0);
            cmd_Thang0.Dispose();
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
            }
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataRow dr_0 in dt_Thang0.Rows)
                {
                        dr["rNo"] = (Convert.ToDecimal(dr["rNo"]) + Convert.ToDecimal(dr_0["rNo"])).ToString();
                        dr["rCo"] = (Convert.ToDecimal(dr["rCo"]) + Convert.ToDecimal(dr_0["rCo"])).ToString();
                        break;
                }
            }

            return dt;
        }
        public static DataTable dtLuyKe(String MaND, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaTaiKhoan)
        {
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
            String SQL = String.Format(@"SELECT SUM(rNo) as rNo,SUM(rCo) as rCo
FROM(
SELECT iNgayCT,iThangCT,sSoChungTuThu,sSoChungTuChi='',sNoiDung,sTenDonVi_No,rNo,rCo=0
FROM (
SELECT iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuThu,sNoiDung,iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rNo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>''
      AND iTrangThai=1 AND iThangCT<=@iThang2 AND iThangCT<>0 AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'  {0}
GROUP BY iID_MaDonVi_No,sTenDonVi_No,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_No,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich

UNION

SELECT iNgayCT,iThangCT,sSoChungTuThu='',sSoChungTuChi,sNoiDung,sTenDonVi_Co,rNo=0,rCo
FROM (
SELECT iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet as sSoChungTuChi,sNoiDung,iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rCo
FROM KTTG_ChungTuChiTiet
WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>''
      AND iTrangThai=1 AND iThangCT<=@iThang2 AND iThangCT<>0  AND iNamLamViec=@iNamLamViec  AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112' {0} 
GROUP BY iID_MaDonVi_Co,sTenDonVi_Co,iNgayCT,iThangCT,sSoChungTuChiTiet,iID_MaTaiKhoanGiaiThich_Co,sNoiDung
) as a
INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen 
			FROM KT_TaiKhoanGiaiThich
			WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112' AND sKyHieu like '{1}%') as b
ON a.iID_MaTaiKhoanGiaiThich_Co=b.iID_MaTaiKhoanGiaiThich) as KT
                                    ", DKTrangThaiDuyet, iID_MaTaiKhoan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dsTaiKhoan()
        {

            DataTable dt = new DataTable();
            String SQL = String.Format(@"SELECT sKyHieu,sKyHieu+' - '+sTen as sTen
FROM KT_TaiKhoanDanhMucChiTiet
WHERE  iTrangThai=1");
            dt = Connection.GetDataTable(SQL);
            return dt;
        }
    }
}
