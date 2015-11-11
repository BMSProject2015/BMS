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
    public class rptKTTongHop_VonVayDauTuController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_VonVayDauTu.xls";
        private const String sFilePathA3 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_VonVayDauTuA3.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_VonVayDauTu.aspx";
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
            String KieuGiay = Convert.ToString(Request.Form[ParentID + "_KieuGiay"]);
            String DVT = Convert.ToString(Request.Form[ParentID + "_DVT"]);
            ViewData["PageLoad"] = "1";
           
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["DVT"] = DVT;
            ViewData["PageLoad"] = "1";
            ViewData["KieuGiay"] = KieuGiay;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_VonVayDauTu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet, String iNamLamViec, String iThang,
                                      String DonViTinh,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTongHop_VonVayDauTu");
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
                iThang = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
            }
            dtCauHinh.Dispose();

            LoadData(fr, MaND, iID_MaTrangThaiDuyet, iNamLamViec, iThang, DonViTinh, KhoGiay);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String DenNgay = "";
            String ngay =Convert.ToString(DateTime.DaysInMonth(Convert.ToInt16(iNamLamViec), Convert.ToInt16(iThang)));
            DenNgay = "Đến ngày " + ngay + " tháng " + iThang + " năm " + iNamLamViec;

            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            if (DonViTinh == "0")
            {
                fr.SetValue("DVT", "Đồng");
            }
            else if (DonViTinh == "1")
            {
                fr.SetValue("DVT", "Nghìn đồng");
            }
            else
            {
                fr.SetValue("DVT", "Triệu đồng");
            }
            //Lấy tham số vốn vay
            String s = ThamSoVonVay(iNamLamViec, KhoGiay);
            String[] arr = s.Split('_');
            for (int i = 0; i < arr.Length; i++)
            {
                String TK = "";
                if (i < arr.Length - 1)
                {
                    if (!String.IsNullOrEmpty(arr[i]))
                      //  TK = "12/20" + arr[i].Substring(3, arr[i].Length - 3);
                        TK = TenTKChiTiet(arr[i]);
                }
                else
                {
                    if(arr[i].IndexOf(",")>0)
                    {
                        TK = "Khác";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(arr[i]))
                            TK = TenTKChiTiet(arr[i]);
                           // TK = "12/20" + arr[i].Substring(3, arr[i].Length - 3);
                    }
                }
                fr.SetValue("TK" + (Convert.ToInt16(i + 1)), TK);
            }
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("DenNgay", DenNgay);
            fr.Run(Result);
            return Result;
        }

        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet, String iNamLamViec, String iThang,
                                    String DonViTinh, String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (KhoGiay == "1")
            {
                DuongDanFile = sFilePathA3;
            }
            else
            {
                DuongDanFile = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNamLamViec, iThang,
                                        DonViTinh, KhoGiay);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKeToan_VonVayDauTu.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet, String iNamLamViec, String iThang,
                                    String DonViTinh,String KhoGiay)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (KhoGiay == "1")
            {
                DuongDanFile = sFilePathA3;
            }
            else
            {
                DuongDanFile = sFilePath;
            }

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNamLamViec, iThang,
                                         DonViTinh, KhoGiay);
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

        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iNamLamViec,
                                                        String iThang, String DonViTinh, String KhoGiay)
        {
            DataTable data = dtKTTongHop_VonVayDauTu(MaND, iID_MaTrangThaiDuyet, iNamLamViec, iThang, DonViTinh, KhoGiay);
            fr.AddTable("ChiTiet",data);
            DataTable dtPhongBan;
            dtPhongBan = HamChung.SelectDistinct("PhongBan", data, "iID_MaPhongBan_No", "iID_MaPhongBan_No");
            fr.AddTable("PhongBan", dtPhongBan);
            data.Dispose();


        }

        public static DataTable dtKTTongHop_VonVayDauTu(String MaND, String iID_MaTrangThaiDuyet, String iNamLamViec,
                                                        String iThang, String DonViTinh,String KhoGiay)
        {
            DataTable dt = null;
            SqlCommand cmd= new SqlCommand();
            String DKTrangThai = "";
            string mySQL = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKTrangThai += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
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
        
            
            String SQL = "";
            if (KhoGiay == "1")// kho giay A3
            {
                SQL =
                    String.Format(
                        @" SELECT * FROM 
(SELECT
iID_MaPhongBan_No,iID_MaDonVi_No
,SUM(TK1_No) as TK1_No
,SUM(TK2_No) as TK2_No
,SUM(TK3_No) as TK3_No
,SUM(TK4_No) as TK4_No
,SUM(TK5_No) as TK5_No
,SUM(TK6_No) as TK6_No
,SUM(TK7_No) as TK7_No
,SUM(TK8_No) as TK8_No
,SUM(TK9_No) as TK9_No
,SUM(TK10_No) as TK10_No
,SUM(TK1_Co) as TK1_Co
,SUM(TK2_Co) as TK2_Co
,SUM(TK3_Co) as TK3_Co
,SUM(TK4_Co) as TK4_Co
,SUM(TK5_Co) as TK5_Co
,SUM(TK6_Co) as TK6_Co
,SUM(TK7_Co) as TK7_Co
,SUM(TK8_Co) as TK8_Co
,SUM(TK9_Co) as TK9_Co
,SUM(TK10_Co) as TK10_Co
FROM(
SELECT   
iID_MaPhongBan_No,iID_MaDonVi_No
,TK1_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)
,TK6_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK6 THEN rSoTien{0} ELSE 0 END)
,TK7_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK7 THEN rSoTien{0} ELSE 0 END)
,TK8_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK8 THEN rSoTien{0} ELSE 0 END)
,TK9_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK9 THEN rSoTien{0} ELSE 0 END)
,TK10_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK10 THEN rSoTien{0} ELSE 0 END)
,TK1_Co=0
,TK2_Co=0
,TK3_Co=0
,TK4_Co=0
,TK5_Co=0
,TK6_Co=0
,TK7_Co=0
,TK8_Co=0
,TK9_Co=0
,TK10_Co=0
FROM KT_ChungTuChiTiet
WHERE
 sTenTaiKhoanGiaiThich_No is not null
 AND sTenTaiKhoanGiaiThich_No <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0 
AND iThangCT>@iThang AND iThangCT<=@iThangLamViec {1}
AND iID_MaTaiKhoan_No='313'
AND iID_MaPhongBan_No IS NOT NULL 
AND iID_MaPhongBan_No <>''
AND iID_MaDonVi_No IS NOT NULL
AND iID_MaDonVi_No<>''
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No
UNION ALL

SELECT   
iID_MaPhongBan_No,iID_MaDonVi_No
,TK1_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)
,TK6_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK6 THEN rSoTien{0} ELSE 0 END)
,TK7_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK7 THEN rSoTien{0} ELSE 0 END)
,TK8_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK8 THEN rSoTien{0} ELSE 0 END)
,TK9_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK9 THEN rSoTien{0} ELSE 0 END)
,TK10_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK10 THEN rSoTien{0} ELSE 0 END)
,TK1_Co=0
,TK2_Co=0
,TK3_Co=0
,TK4_Co=0
,TK5_Co=0
,TK6_Co=0
,TK7_Co=0
,TK8_Co=0
,TK9_Co=0
,TK10_Co=0
FROM KT_SoDuTaiKhoanGiaiThich
WHERE sTenTaiKhoanGiaiThich_No is not null
 AND sTenTaiKhoanGiaiThich_No <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0
AND iID_MaTaiKhoan_No='313'
AND iID_MaPhongBan_No IS NOT NULL 
AND iID_MaPhongBan_No <>''
AND iID_MaDonVi_No IS NOT NULL
AND iID_MaDonVi_No<>''
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No

UNION ALL

SELECT   
iID_MaPhongBan_Co,iID_MaDonVi_Co
,TK1_No=0
,TK2_No=0
,TK3_No=0
,TK4_No=0
,TK5_No=0
,TK6_No=0
,TK7_No=0
,TK8_No=0
,TK9_No=0
,TK10_No=0
,TK1_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)
,TK6_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK6 THEN rSoTien{0} ELSE 0 END)
,TK7_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK7 THEN rSoTien{0} ELSE 0 END)
,TK8_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK8 THEN rSoTien{0} ELSE 0 END)
,TK9_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK9 THEN rSoTien{0} ELSE 0 END)
,TK10_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK10 THEN rSoTien{0} ELSE 0 END)
FROM KT_ChungTuChiTiet
WHERE sTenTaiKhoanGiaiThich_Co is not null
 AND sTenTaiKhoanGiaiThich_Co <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0
AND iThangCT>@iThang  AND iThangCT<=@iThangLamViec {1}
AND iID_MaTaiKhoan_Co='313'
AND iID_MaPhongBan_Co IS NOT NULL 
AND iID_MaPhongBan_Co <>''
AND iID_MaDonVi_Co IS NOT NULL
AND iID_MaDonVi_Co<>''
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co

UNION ALL

SELECT   
iID_MaPhongBan_Co,iID_MaDonVi_Co
,TK1_No=0
,TK2_No=0
,TK3_No=0
,TK4_No=0
,TK5_No=0
,TK6_No=0
,TK7_No=0
,TK8_No=0
,TK9_No=0
,TK10_No=0
,TK1_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)
,TK6_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK6 THEN rSoTien{0} ELSE 0 END)
,TK7_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK7 THEN rSoTien{0} ELSE 0 END)
,TK8_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK8 THEN rSoTien{0} ELSE 0 END)
,TK9_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK9 THEN rSoTien{0} ELSE 0 END)
,TK10_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK10 THEN rSoTien{0} ELSE 0 END)
FROM KT_SoDuTaiKhoanGiaiThich
WHERE sTenTaiKhoanGiaiThich_Co is not null
 AND sTenTaiKhoanGiaiThich_Co <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0
AND iID_MaTaiKhoan_Co='313'
AND iID_MaPhongBan_Co IS NOT NULL 
AND iID_MaPhongBan_Co <>''
AND iID_MaDonVi_Co IS NOT NULL
AND iID_MaDonVi_Co<>''
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co
) CT
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No) as a
INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTenTomTat FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi
ON a.iID_MaDonVi_No=NS_DonVi.MaDonVi",
                        TyLe, DKTrangThai);
            }
            else
            {
                SQL =
                    String.Format(
                        @" SELECT * FROM 
(SELECT
iID_MaPhongBan_No,iID_MaDonVi_No
,SUM(TK1_No) as TK1_No
,SUM(TK2_No) as TK2_No
,SUM(TK3_No) as TK3_No
,SUM(TK4_No) as TK4_No
,SUM(TK5_No) as TK5_No
,SUM(TK1_Co) as TK1_Co
,SUM(TK2_Co) as TK2_Co
,SUM(TK3_Co) as TK3_Co
,SUM(TK4_Co) as TK4_Co
,SUM(TK5_Co) as TK5_Co
FROM(
SELECT   
iID_MaPhongBan_No,iID_MaDonVi_No
,TK1_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)
,TK1_Co=0
,TK2_Co=0
,TK3_Co=0
,TK4_Co=0
,TK5_Co=0
FROM KT_ChungTuChiTiet
WHERE
 sTenTaiKhoanGiaiThich_No is not null
 AND sTenTaiKhoanGiaiThich_No <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0 
AND iThangCT>@iThang AND iThangCT<=@iThangLamViec {1}
AND iID_MaTaiKhoan_No='313'
AND iID_MaPhongBan_No IS NOT NULL 
AND iID_MaPhongBan_No <>''
AND iID_MaDonVi_No IS NOT NULL
AND iID_MaDonVi_No<>''

GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No

UNION ALL

SELECT   
iID_MaPhongBan_No,iID_MaDonVi_No
,TK1_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_No=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)
,TK1_Co=0
,TK2_Co=0
,TK3_Co=0
,TK4_Co=0
,TK5_Co=0
FROM KT_SoDuTaiKhoanGiaiThich
WHERE sTenTaiKhoanGiaiThich_No is not null
 AND sTenTaiKhoanGiaiThich_No <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0
AND iID_MaTaiKhoan_No='313'
AND iID_MaPhongBan_No IS NOT NULL 
AND iID_MaPhongBan_No <>''
AND iID_MaDonVi_No IS NOT NULL
AND iID_MaDonVi_No<>''
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No

UNION ALL

SELECT   
iID_MaPhongBan_Co,iID_MaDonVi_Co
,TK1_No=0
,TK2_No=0
,TK3_No=0
,TK4_No=0
,TK5_No=0
,TK1_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)

FROM KT_ChungTuChiTiet
WHERE sTenTaiKhoanGiaiThich_Co is not null
 AND sTenTaiKhoanGiaiThich_Co <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0
AND iThangCT>@iThang  AND iThangCT<=@iThangLamViec {1}
AND iID_MaTaiKhoan_Co='313'
AND iID_MaPhongBan_Co IS NOT NULL 
AND iID_MaPhongBan_Co <>''
AND iID_MaDonVi_Co IS NOT NULL
AND iID_MaDonVi_Co<>''
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co

UNION ALL

SELECT   
iID_MaPhongBan_Co,iID_MaDonVi_Co
,TK1_No=0
,TK2_No=0
,TK3_No=0
,TK4_No=0
,TK5_No=0
,TK1_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK1 THEN rSoTien{0} ELSE 0 END)
,TK2_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK2 THEN rSoTien{0} ELSE 0 END)
,TK3_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK3 THEN rSoTien{0} ELSE 0 END)
,TK4_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK4 THEN rSoTien{0} ELSE 0 END)
,TK5_Co=SUM(CASE WHEN SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co,1)-1) LIKE @TK5 THEN rSoTien{0} ELSE 0 END)

FROM KT_SoDuTaiKhoanGiaiThich
WHERE sTenTaiKhoanGiaiThich_Co is not null
 AND sTenTaiKhoanGiaiThich_Co <>''
AND iTrangThai=1 AND iNamLamViec=@iNamLamViec AND rSoTien<>0
AND iID_MaTaiKhoan_Co='313'
AND iID_MaPhongBan_Co IS NOT NULL 
AND iID_MaPhongBan_Co <>''
AND iID_MaDonVi_Co IS NOT NULL
AND iID_MaDonVi_Co<>''
GROUP BY iID_MaPhongBan_Co,iID_MaDonVi_Co
) CT
GROUP BY iID_MaPhongBan_No,iID_MaDonVi_No) as a
INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTenTomTat FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi
ON a.iID_MaDonVi_No=NS_DonVi.MaDonVi",
                        TyLe, DKTrangThai); 
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangLamViec", iThang);
            String sThamSoVonVay = ThamSoVonVay(iNamLamViec, KhoGiay);
            String[] arrThamSoVonVay = sThamSoVonVay.Split('_');
            for (int i = 0; i < arrThamSoVonVay.Length; i++)
            {
                if (String.IsNullOrEmpty(arrThamSoVonVay[i]))
                    arrThamSoVonVay[i] = "-1";
                cmd.Parameters.AddWithValue("@TK" + Convert.ToInt16(i + 1), arrThamSoVonVay[i] + "%");
            }
            // cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            int iThangSoDuDauNam = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
            cmd.Parameters.AddWithValue("@iThang", iThangSoDuDauNam);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public  static  DataTable get_TaiKhoanDanhMucChiTiet()
        {
            String SQL = @"SELECT sKyHieu,sKyHieu+'-'+sTen as TenHT
FROM KT_TaiKhoanDanhMucChiTiet
WHERE sXauNoiMa='313'AND iTrangThai=1";
            return Connection.GetDataTable(SQL);
        }
        public static String TenTKChiTiet(string sKyHieu)
        {
            string SQL =
                      "SELECT TOP 1 sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE sKyHieu=@sKyHieu";
             SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            String vR = Convert.ToString(Connection.GetValue(cmd, ""));
            return vR;
        }
        public static String ThamSoVonVay(String iNamLamViec, String KhoGiay)
        {
            String SQL = @"
SELECT sThamSo FROM KT_DanhMucThamSo
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND sKyHieu='163'";
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            String s= Connection.GetValueString(cmd, "");
            String[] arr = s.Split(',');
            int a = arr.Length;
            s = "";
            for (int i = 0; i < a; i++)
            {
                if (i<a-1)
                {
                    s += arr[i] + "_";
                }

                else
                {
                    s += arr[i];
                }
            }
            String x = "";
            if (KhoGiay == "2")
            {
                if (a < 5)
                {
                    for (int i = 0; i < 5 - a; i++)
                    {
                        s += "_";
                    }
                }
                else
                {
                    for (int i = 4; i < a; i++)
                    {
                        if (i < a - 1)
                        {
                            x += arr[i] + ",";
                        }

                        else
                        {
                            x += arr[i];
                        }
                    }
                    arr[4] = x;
                    s = "";
                    for (int i = 0; i < 5; i++)
                    {
                        if (i < 4)
                        {
                            s += arr[i] + "_";
                        }
                        else
                        {
                            s += arr[i];
                        }
                    }
                }
            }
            else
            {
                if (a < 10)
                {
                    for (int i = 0; i < 10 - a; i++)
                    {
                        s += "_";
                    }
                }
                else
                {
                    for (int i = 9; i < a; i++)
                    {
                        if (i < a - 1)
                        {
                            x += arr[i] + ",";
                        }

                        else
                        {
                            x += arr[i];
                        }
                    }
                    arr[9] = x;
                    s = "";
                    for (int i = 0; i < 10; i++)
                    {
                        if (i < 9)
                        {
                            s += arr[i] + "_";
                        }
                        else
                        {
                            s += arr[i];
                        }
                    }
                }
            }
            return s;
        }
    }

}
    

