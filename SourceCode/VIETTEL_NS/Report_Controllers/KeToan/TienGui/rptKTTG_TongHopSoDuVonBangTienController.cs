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
    public class rptKTTG_TongHopSoDuVonBangTienController : Controller
    {
        //
        // GET: /rptKTTG_TongHopSoDuVonBangTien/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTG_TongHopSoDuVonBangTien.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_TongHopSoDuVonBangTien.aspx";
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            DateTime TuNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang1 + "/" + iNgay1);
            DateTime DenNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang2 + "/" + iNgay2);
            TimeSpan time = DenNgay - TuNgay;
            if (time.Days < 0)
            {
                ViewData["PageLoad"] = "0";
            }
            else
            {
                ViewData["PageLoad"] = "1";
            }
            ViewData["iNgay1"] = iNgay1;
            ViewData["iThang1"] = iThang1;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iNgay2"] = iNgay2;
            ViewData["iThang2"] = iThang2;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTG_TongHopSoDuVonBangTien.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTG_TongHopSoDuVonBangTien.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
             DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Từ ngày " + iNgay1 + "/" + iThang1 + " đến ngày " + iNgay2 + "/" + iThang2 + "/" + iNamLamViec;
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTG_TongHopSoDuVonBangTien");
            LoadData(fr, MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dtKTTG_TongHopSoDuVonBangTien(MaND, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        public static DataTable dtKTTG_TongHopSoDuVonBangTien(String MaND, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();

            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;

          
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            String SQL = String.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY sKyHieu ) as STT, sKyHieu,sTen,SUM(DauKyNo) as DauKyNo,SUM(DauKyCo) as DauKyCo,SUM(TrongKyNo) as TrongKyNo,SUM(TrongKyCo) as TrongKyCo FROM (
                                        --So DU Dau Ky--
                                        SELECT sKyHieu,sTen,iID_MaTaiKhoanGiaiThich_No,DauKyNo,DauKyCo,TrongKyNo=0,TrongKyCo=0
                                        FROM (
                                        --NO--
                                        SELECT * FROM(
                                        SELECT sKyHieu,sTen,iID_MaTaiKhoanGiaiThich_No,rSoTien as DauKyNo,DauKyCo=0 FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'
	                                          AND iTrangThai=1 AND (iThangCT<@iThang1 OR (iThangCT=@iThang1 AND iNgayCT<@iNgay1)) AND iNamLamViec=@iNamLamViec {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_No
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich) as DKN

                                        UNION

                                        -- Co
                                        SELECT * FROM (
                                        SELECT sKyHieu,sTen,iID_MaTaiKhoanGiaiThich_Co,DauKyNo=0,DauKyCo=rSoTien FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>'' AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112'
	                                          AND iTrangThai=1 AND (iThangCT<@iThang1 OR (iThangCT=@iThang1 AND iNgayCT<@iNgay1)) AND iNamLamViec=@iNamLamViec {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_Co
                                        ) as c
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as d
                                        ON c.iID_MaTaiKhoanGiaiThich_Co=d.iID_MaTaiKhoanGiaiThich
                                        ) as DKC) DK

                                        UNION
                                        --Trong Ky
                                        SELECT sKyHieu,sTen,iID_MaTaiKhoanGiaiThich_No,DauKyNo=0,DauKyCo=0,TrongKyNo,TrongKyCo
                                        FROM (
                                        --NO--
                                        SELECT * FROM(
                                        SELECT sKyHieu,sTen,iID_MaTaiKhoanGiaiThich_No,rSoTien as TrongKyNo,TrongKyCo=0 FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'
	                                          AND iTrangThai=1 AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)>=@TuNgay
                                                               AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_No
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich) as DKN

                                        UNION

                                        -- Co
                                        SELECT * FROM (
                                        SELECT sKyHieu,sTen,iID_MaTaiKhoanGiaiThich_Co,TrongKyNo=0,TrongKyCo=rSoTien FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rSoTien
                                        FROM KTTG_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>'' AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112'
	                                          AND iTrangThai=1 AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)>=@TuNgay
                                                               AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_Co
                                        ) as c
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as d
                                        ON c.iID_MaTaiKhoanGiaiThich_Co=d.iID_MaTaiKhoanGiaiThich
                                        ) as DKC) DK) as KT
                                        GROUP BY sKyHieu,sTen", DKTrangThaiDuyet);
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iNgay1", iNgay1);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            //Lấy số dư tháng 0 của kế toán tổng hợp

            String SQL_Thang0 = String.Format(@"    SELECT sKyHieu,SUM(DauKyNo) as DauKyNo,SUM(DauKyCo) as DauKyCo FROM(
                                        SELECT sKyHieu,rSoTien as DauKyNo,DauKyCo=0 FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_No,SUM(rSoTien)as rSoTien
                                        FROM KT_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_No IS NOT NULL AND iID_MaTaiKhoanGiaiThich_No <>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,3)='112'
	                                          AND iTrangThai=1 AND iThangCT=0  AND iNamLamViec=@iNamLamViec {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_No
                                        ) as a
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as b
                                        ON a.iID_MaTaiKhoanGiaiThich_No=b.iID_MaTaiKhoanGiaiThich

                                        UNION

                                        -- Co
                                       
                                        SELECT sKyHieu,DauKyNo=0,DauKyCo=rSoTien FROM (
                                        SELECT iID_MaTaiKhoanGiaiThich_Co,SUM(rSoTien)as rSoTien
                                        FROM KT_ChungTuChiTiet
                                        WHERE iID_MaTaiKhoanGiaiThich_Co IS NOT NULL AND iID_MaTaiKhoanGiaiThich_Co <>'' AND SUBSTRING(iID_MaTaiKhoan_Co,1,3)='112'
	                                          AND iTrangThai=1 AND iThangCT=0 AND iNamLamViec=@iNamLamViec {0}
                                        GROUP BY iID_MaTaiKhoanGiaiThich_Co
                                        ) as c
                                        INNER JOIN (SELECT iID_MaTaiKhoanGiaiThich,sKyHieu,sTen FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND SUBSTRING(iID_MaTaiKhoan,1,3)='112') as d
                                        ON c.iID_MaTaiKhoanGiaiThich_Co=d.iID_MaTaiKhoanGiaiThich) as CT
                                        GROUP BY sKyHieu
                                        ",DKTrangThaiDuyet);
            SqlCommand cmd_Thang0 = new SqlCommand(SQL_Thang0);
            cmd_Thang0.Parameters.AddWithValue("iNamLamViec", iNamLamViec); ;
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd_Thang0.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt_Thang0 = Connection.GetDataTable(cmd_Thang0);
            cmd.Dispose();
            

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataRow dr_0 in dt_Thang0.Rows)
                {
                    if (String.Equals(dr["sKyHieu"],dr_0["sKyHieu"]))
                    {
                        dr["DauKyNo"] = (Convert.ToDecimal(dr["DauKyNo"]) + Convert.ToDecimal(dr_0["DauKyNo"])).ToString();
                        dr["DauKyCo"] = (Convert.ToDecimal(dr["DauKyCo"]) + Convert.ToDecimal(dr_0["DauKyCo"])).ToString();
                        break;
                    }
                }
            }
            return dt;
        }
    }
}
