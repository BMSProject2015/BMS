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


namespace VIETTEL.Report_Controllers.KeToan
{
    public class rptKTTongHop_ChiTietCacKhoanTamUngController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_ChiTietCacKhoanTamUng.xls";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTietCacKhoanTamUng.aspx";
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String DonViTinh = Convert.ToString(Request.Form[ParentID + "_DonViTinh"]);
            String BQL = Convert.ToString(Request.Form[ParentID + "_BQL"]);
            ViewData["PageLoad"] = "1";
            ViewData["iNgay"] = iNgay;
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["DonViTinh"] = DonViTinh;
            ViewData["BQL"] = BQL;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_ChiTietCacKhoanTamUng.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh,String BQL)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTongHop_ChiTietCacKhoanTamUng");
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String DenNgay = "Đến ngày "+iNgay+" tháng "+ iThang +" năm "+iNamLamViec;
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, BQL);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();

         
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            if (DonViTinh == "0")
            {
                fr.SetValue("DVT", "Đồng");
            }
            else if (DonViTinh == "1")
            {
                fr.SetValue("DVT", "Nghìn Đồng");
            }
            else
            {
                fr.SetValue("DVT", "Triệu Đồng");
            }
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("DenNgay", DenNgay);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String BQL)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, BQL);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKeToan_ChiTietCacKhoanTamUng.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String BQL)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, BQL);
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
        public String LayMoTa(String sKyHieu)
        {
            String sTen = "";
            String SQL = @"SELECT sTen FROM KT_TaiKhoanDanhMucChiTiet
                           WHERE  iTrangThai=1 AND sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            sTen = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return sTen;

        }
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String BQL)
        {
            DataTable data = dtKTTongHop_ChiTietCacKhoanTamUng_TienViet(MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, BQL);
            fr.AddTable("TienViet", data);
            //DataTable dtNhomKH;
            //dtNhomKH = HamChung.SelectDistinct("NhomKH6", data, "iID_MaPhongBan_No,sTenPhongBan_No,sKyHieu_6,sTenDonVi_No", "iID_MaPhongBan_No,sTenPhongBan_No,sKyHieu_6,sTenDonVi_No", "");
            //dtNhomKH.Columns.Add("sTen", typeof(String));
            //foreach (DataRow dr in dtNhomKH.Rows)
            //{
            //    String sTen = LayMoTa(dr["sKyHieu_6"].ToString());
            //    dr["sTen"] = sTen;
            //}
            //fr.AddTable("NhomKH6_TienViet", dtNhomKH);
            DataTable dtPhongBan;
            dtPhongBan = HamChung.SelectDistinct("PhongBan_TienViet", data, "iID_MaPhongBan_No,sTenPhongBan_No", "iID_MaPhongBan_No,sTenPhongBan_No,sKyHieu_6", "");
            fr.AddTable("PhongBan_TienViet", dtPhongBan);


            DataTable data2 = dtKTTongHop_ChiTietCacKhoanTamUng_NgoaiTe(MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, BQL);
            fr.AddTable("NgoaiTe", data2);

            //DataTable dtNhomKH2;
            //dtNhomKH2 = HamChung.SelectDistinct("NhomKH62", data2, "iID_MaPhongBan_No,sTenPhongBan_No,sKyHieu_6,sTenDonVi_No", "iID_MaPhongBan_No,sTenPhongBan_No,sKyHieu_6,sTenDonVi_No", "");
            //dtNhomKH2.Columns.Add("sTen", typeof(String));
            //foreach (DataRow dr in dtNhomKH2.Rows)
            //{
            //    String sTen = LayMoTa(dr["sKyHieu_6"].ToString());
            //    dr["sTen"] = sTen;
            //}
            //fr.AddTable("NhomKH6_NgoaiTe", dtNhomKH2);
            DataTable dtPhongBan2;
            dtPhongBan2 = HamChung.SelectDistinct("PhongBan_NgoaiTe", data2, "iID_MaPhongBan_No,sTenPhongBan_No", "iID_MaPhongBan_No,sTenPhongBan_No,sKyHieu_6", "");
            fr.AddTable("PhongBan_NgoaiTe", dtPhongBan2);
            if (data != null)
            {
                data.Dispose();
            }
            if (dtPhongBan != null)
            {
                dtPhongBan.Dispose();
            }
            if (dtPhongBan2 != null)
            {
                dtPhongBan2.Dispose();
            }
        }
        public JsonResult Get_objNgayThang(String ParentID, String MaND, String iThang,String iNgay)
        {
            return Json(get_sNgayThang(ParentID, MaND, iThang,iNgay), JsonRequestBehavior.AllowGet);
        }
        public String get_sNgayThang(String ParentID, String MaND, String iThang,String iNgay)
        {
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            String S = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:55px;padding:2px;border:1px solid #dedede;\"");
            dtNgay.Dispose();

            return S;
        }
        public static DataTable dtKTTongHop_ChiTietCacKhoanTamUng_TienViet(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String BQL)
        {
            DataTable dt = null;
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();

            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String DKTrangThai = "";
            String iID_MaPhongBan = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKTrangThai += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (BQL != "-1")
            {
                iID_MaPhongBan += " AND iID_MaPhongBan_No=@iID_MaPhongBan_No";

            }
            String TyLe = "";
            if (DonViTinh == "0")
            {
                TyLe = "";
            }
            else if (DonViTinh == "1")
            {
                TyLe = "/1000";
            }
            else
            {
                TyLe = "/1000000";
            }
//            String SQL =
//                String.Format(
//                    @"  select iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6, sum(UngKPNN) as UngKPNN, sum(UngKPDacBiet) as UngKPDacBiet,
//  sum(Ungquanly) as Ungquanly, sum(DauTu_XDCB) as DauTu_XDCB, sum(UngSXQP) as UngSXQP, sum(UngKhac) as UngKhac
//  from (SELECT iID_MaPhongBan_No,SUBSTRING(sTenPhongBan_No,CHARINDEX('-',sTenPhongBan_No)+1,LEN(sTenPhongBan_No)) as sTenPhongBan_No,
//	                                            iID_MaDonVi_No,sTenDonVi_No,sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),iID_MaTaiKhoan_No,sKyHieu_6,
//	                                           -- iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1
//                                                   
//	                                             UngKPNN=CASE WHEN SUBSTRING(sKyHieu,1,3)='101' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                  UngKPDacBiet=CASE WHEN SUBSTRING(sKyHieu,1,3)='102' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                        Ungquanly=CASE WHEN SUBSTRING(sKyHieu,1,3)='105' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                            DauTu_XDCB=CASE WHEN SUBSTRING(sKyHieu,1,3)='104' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                         UngSXQP=CASE WHEN SUBSTRING(sKyHieu,1,3)='103' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                         UngKhac=CASE WHEN SUBSTRING(sKyHieu,1,3)='106' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END
//                                         FROM
//(SELECT DISTINCT iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
// iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
// FROM KT_ChungTuChiTiet WHERE iTrangThai=1 {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
// AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>''
// AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>'' AND iID_MaTaiKhoan_No LIKE '312%'
//union all
//                                             
//SELECT DISTINCT iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co, iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
// FROM KT_ChungTuChiTiet WHERE iTrangThai=1 {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
// AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>''
// AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%'
// UNION all
//-- LAY SO DU TAI KHOAN GIAI THICH
//SELECT DISTINCT iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
// iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
// FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>''
// AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>'' AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>''AND iID_MaTaiKhoan_No LIKE '312%'
// union all
//SELECT DISTINCT iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co,
// iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
// FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>''
//AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>'' AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%') AS GiaiThich
//
//
//GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,
//	                                            iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
//	                                            --iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1,sKyHieu_6
//sKyHieu,sKyHieu_6
//                                        HAVING  SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='101' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='102' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='103' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='104' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='105' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='106' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0 
//                                      --  ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu,sKyHieu_6
//                                        ) as CT GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6 ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu_6",
//                    DKTrangThai, TyLe);
            String SQL =
          String.Format(
              @"  select iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6, sum(UngKPNN) as UngKPNN, sum(UngKPDacBiet) as UngKPDacBiet,
  sum(Ungquanly) as Ungquanly, sum(DauTu_XDCB) as DauTu_XDCB, sum(UngSXQP) as UngSXQP, sum(UngKhac) as UngKhac
  from (SELECT iID_MaPhongBan_No,SUBSTRING(sTenPhongBan_No,CHARINDEX('-',sTenPhongBan_No)+1,LEN(sTenPhongBan_No)) as sTenPhongBan_No,
	                                            iID_MaDonVi_No,sTenDonVi_No,sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),iID_MaTaiKhoan_No,sKyHieu_6,
	                                           -- iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1
                                                   
	                                             UngKPNN=CASE WHEN SUBSTRING(sKyHieu,1,3)='101' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                  UngKPDacBiet=CASE WHEN SUBSTRING(sKyHieu,1,3)='102' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                        Ungquanly=CASE WHEN SUBSTRING(sKyHieu,1,3)='105' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                            DauTu_XDCB=CASE WHEN SUBSTRING(sKyHieu,1,3)='104' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                         UngSXQP=CASE WHEN SUBSTRING(sKyHieu,1,3)='103' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                         UngKhac=CASE WHEN SUBSTRING(sKyHieu,1,3)='106' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END
                                         FROM
(SELECT  iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
 iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
 FROM KT_ChungTuChiTiet WHERE iTrangThai=1  and iNamLamViec=@iNamLamViec {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
 AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>''
 AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>'' AND iID_MaTaiKhoan_No LIKE '312%'
union all
                                             
SELECT  iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co, iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
 FROM KT_ChungTuChiTiet WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
 AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>''
 AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%'
 UNION all
-- LAY SO DU TAI KHOAN GIAI THICH
SELECT  iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
 iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
 FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>''
 AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>'' AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>''AND iID_MaTaiKhoan_No LIKE '312%'
 union all
SELECT  iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co,
 iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
 FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>''
AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>'' AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%') AS GiaiThich


GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,
	                                            iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
	                                            --iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1,sKyHieu_6
sKyHieu,sKyHieu_6
                                        HAVING  SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='101' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='102' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='103' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='104' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='105' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='106' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0 
                                      --  ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu,sKyHieu_6
                                        ) as CT WHERE 1=1 {2} GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6 ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu_6",
              DKTrangThai, TyLe, iID_MaPhongBan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (BQL != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaPhongBan_No", BQL);

            }
            int iThangSoDuDauNam = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
            cmd.Parameters.AddWithValue("@iThangCT", iThangSoDuDauNam);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            cmd.Dispose();
            return dt;
        }

        public static DataTable dtKTTongHop_ChiTietCacKhoanTamUng_NgoaiTe(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String BQL)
        {
            DataTable dt = null;
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();

            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String DKTrangThai = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKTrangThai += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            String iID_MaPhongBan = "";
            if (BQL != "-1")
            {
                iID_MaPhongBan += " AND iID_MaPhongBan_No=@iID_MaPhongBan_No";

            }
            String TyLe = "";
            if (DonViTinh == "0")
            {
                TyLe = "";
            }
            else if (DonViTinh == "1")
            {
                TyLe = "/1000";
            }
            else
            {
                TyLe = "/1000000";
            }
//            String SQL =
//                String.Format(
//                    @"select iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6, sum(UngKPNN) as UngKPNN, sum(UngKPDacBiet) as UngKPDacBiet,
//  sum(Ungquanly) as Ungquanly, sum(DauTu_XDCB) as DauTu_XDCB, sum(UngSXQP) as UngSXQP, sum(UngKhac) as UngKhac
//  from (SELECT iID_MaPhongBan_No,SUBSTRING(sTenPhongBan_No,CHARINDEX('-',sTenPhongBan_No)+1,LEN(sTenPhongBan_No)) as sTenPhongBan_No,
//	                                            iID_MaDonVi_No,sTenDonVi_No,sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),iID_MaTaiKhoan_No,
//	                                           -- iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1,                                           
//	                                             sKyHieu_6,UngKPNN=CASE WHEN SUBSTRING(sKyHieu,1,3)='111' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                  UngKPDacBiet=CASE WHEN SUBSTRING(sKyHieu,1,3)='112' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                        Ungquanly=CASE WHEN SUBSTRING(sKyHieu,1,3)='113' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                            DauTu_XDCB=CASE WHEN SUBSTRING(sKyHieu,1,3)='114' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                         UngSXQP=CASE WHEN SUBSTRING(sKyHieu,1,3)='115' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
//	                                         UngKhac=CASE WHEN SUBSTRING(sKyHieu,1,3)='116' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END
//                                         FROM
//(SELECT DISTINCT iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
// iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
// FROM KT_ChungTuChiTiet WHERE iTrangThai=1 {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
// AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>''
// AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>'' AND iID_MaTaiKhoan_No LIKE '312%'
//union all
//                                             
//SELECT DISTINCT iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co, iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
// FROM KT_ChungTuChiTiet WHERE iTrangThai=1 {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
// AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>''
// AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%'
// UNION all
//-- LAY SO DU TAI KHOAN GIAI THICH
//SELECT DISTINCT iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
// iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
// FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>''
// AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>'' AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>''AND iID_MaTaiKhoan_No LIKE '312%'
// union all
//SELECT DISTINCT iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co,
// iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
// SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
// FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>''
//AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>'' AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%') AS GiaiThich
//
//
//GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,
//	                                            iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,sKyHieu,sKyHieu_6
//	                                            --iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1,sKyHieu_6
//                                        HAVING  SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='111' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='112' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='113' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='114' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='115' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
//                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='116' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0 
//                                        --ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu,sKyHieu_6
//                                        ) as CT GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6 ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu_6",
//                    DKTrangThai, TyLe);
            String SQL =
               String.Format(
                   @"select iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6, sum(UngKPNN) as UngKPNN, sum(UngKPDacBiet) as UngKPDacBiet,
  sum(Ungquanly) as Ungquanly, sum(DauTu_XDCB) as DauTu_XDCB, sum(UngSXQP) as UngSXQP, sum(UngKhac) as UngKhac
  from (SELECT iID_MaPhongBan_No,SUBSTRING(sTenPhongBan_No,CHARINDEX('-',sTenPhongBan_No)+1,LEN(sTenPhongBan_No)) as sTenPhongBan_No,
	                                            iID_MaDonVi_No,sTenDonVi_No,sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),iID_MaTaiKhoan_No,
	                                           -- iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1,                                           
	                                             sKyHieu_6,UngKPNN=CASE WHEN SUBSTRING(sKyHieu,1,3)='111' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                  UngKPDacBiet=CASE WHEN SUBSTRING(sKyHieu,1,3)='112' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                        Ungquanly=CASE WHEN SUBSTRING(sKyHieu,1,3)='113' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                            DauTu_XDCB=CASE WHEN SUBSTRING(sKyHieu,1,3)='114' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                         UngSXQP=CASE WHEN SUBSTRING(sKyHieu,1,3)='115' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END,
	                                         UngKhac=CASE WHEN SUBSTRING(sKyHieu,1,3)='116' THEN	CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){1} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{1} END ELSE 0 END
                                         FROM
(SELECT DISTINCT iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
 iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
 FROM KT_ChungTuChiTiet WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
 AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>''
 AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>'' AND iID_MaTaiKhoan_No LIKE '312%'
union all
                                             
SELECT DISTINCT iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co, iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
 FROM KT_ChungTuChiTiet WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec {0} AND iThangCT>@iThangCT AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
 AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>''
 AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%'
 UNION all
-- LAY SO DU TAI KHOAN GIAI THICH
SELECT DISTINCT iID_MaPhongBan_No,sTenPhongBan_No,	iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,
 iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,rSoTien as rSoTienNo, rSoTienCo=0,SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1))
 FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>''
 AND iID_MaPhongBan_No IS NOT NULL AND iID_MaPhongBan_No<>'' AND sTenTaiKhoanGiaiThich_No IS NOT NULL AND sTenTaiKhoanGiaiThich_No<>''AND iID_MaTaiKhoan_No LIKE '312%'
 union all
SELECT DISTINCT iID_MaPhongBan_Co,sTenPhongBan_Co,	iID_MaDonVi_Co,sTenDonVi_Co,iID_MaTaiKhoan_Co,
 iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTienNo=0,rSoTien as rSoTienCo,SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
 SUBSTRING(sTenTaiKhoanGiaiThich_Co,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)+1,LEN(sTenTaiKhoanGiaiThich_Co)) as sKyHieu_6, sKyHieu=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1))
 FROM KT_SoDuTaiKhoanGiaiThich WHERE iTrangThai=1 and iNamLamViec=@iNamLamViec AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>''
AND iID_MaPhongBan_Co IS NOT NULL AND iID_MaPhongBan_Co<>'' AND sTenTaiKhoanGiaiThich_Co IS NOT NULL AND sTenTaiKhoanGiaiThich_Co<>'' AND iID_MaTaiKhoan_Co LIKE '312%') AS GiaiThich


GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,
	                                            iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,sKyHieu,sKyHieu_6
	                                            --iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,sKyHieu,sKyHieu_1,sKyHieu_6
                                        HAVING  SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='111' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='112' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='113' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='114' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='115' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0
                                             OR SUM(CASE WHEN SUBSTRING(sKyHieu,1,3)='116' THEN rSoTienNo-rSoTienCo ELSE 0 END)<>0 
                                        --ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu,sKyHieu_6
                                        ) as CT WHERE 1=1 {2} GROUP BY iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTomTat,iID_MaTaiKhoan_No,sKyHieu_6 ORDER BY iID_MaDonVi_No,sTenDonVi_No,sKyHieu_6",
                   DKTrangThai, TyLe, iID_MaPhongBan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (BQL != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaPhongBan_No", BQL);

            }
            int iThangSoDuDauNam = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
            cmd.Parameters.AddWithValue("@iThangCT", iThangSoDuDauNam);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            cmd.Dispose();
            return dt;

        }
        public static DataTable DT_DS_BQL(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "", Boolean TaiKhoanCha = false)
        {
            DataTable vR = new DataTable();
            String SQL =
                "SELECT DISTINCT sKyHieu, sKyHieu + '-' +sTen as sTen FROM NS_PhongBan WHERE iTrangThai=1 ORDER BY sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
           
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["sKyHieu"] = "-1";
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
    }
    
}
