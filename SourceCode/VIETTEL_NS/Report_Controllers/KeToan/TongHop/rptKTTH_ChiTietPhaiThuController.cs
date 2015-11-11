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


namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTH_ChiTietPhaiThuController : Controller
    {
        //
        // GET: /rptKTTH_ChiTietPhaiThu/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietPhaiThu.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietPhaiThu1.xls";
        private const String sFilePath2 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietPhaiThu2.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ChiTietPhaiThu.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);
            String DVT = Convert.ToString(Request.Form[ParentID + "_DVT"]);
            String Mucin = Convert.ToString(Request.Form[ParentID + "_Mucin"]);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ChiTietPhaiThu.aspx";
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iNgay"] = iNgay;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["DVT"] = DVT;
            ViewData["Mucin"] = Mucin;
            ViewData["PageLoad"] = 1;
            return View(sViewPath + "ReportView.aspx");
            
        }

        public ExcelFile CreateReport(String path, String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            String DuongDan = "";
            if (Mucin == "0")
            {
                DuongDan = sFilePath;
            }
            else if (Mucin == "1")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(DuongDan));

            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTH_ChiTietPhaiThu");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet, Mucin);
            if (DVT == "0")
            {
                fr.SetValue("DVT", "Đồng");
            }
            else if (DVT == "1")
            {
                fr.SetValue("DVT", "Nghìn đồng");
            }
            else
            {
                fr.SetValue("DVT", "Triệu đồng");
            }

            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("iThang", iThang);
            fr.SetValue("iNgay", iNgay);
            fr.SetValue("ngay", ngay);
            fr.SetValue("LoaiBaoCao", DVT);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.Run(Result);
            return Result;

        }

        public clsExcelResult ExportToPDF(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            HamChung.Language();
            String DuongDan = "";
            if (Mucin == "0")
            {
                DuongDan = sFilePath;
            }
            else if (Mucin == "1")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet, Mucin);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }

        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            HamChung.Language();
            String DuongDan = "";
            if (Mucin == "0")
            {
                DuongDan = sFilePath;
            }
            else if (Mucin == "1")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet, Mucin);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKTTH_ChiTietPhaiThu.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public ActionResult ViewPDF(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            HamChung.Language();
            String DuongDan = "";
            if (Mucin == "0")
            {
                DuongDan = sFilePath;
            }
            else if (Mucin == "1")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet, Mucin);
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

        private void LoadData(FlexCelReport fr, String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            DataTable data = ChiTietPhaiThu1(iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet, Mucin);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable data1 = ChiTietPhaiThu(iNamLamViec, iThang, iNgay, DVT, iID_MaTrangThaiDuyet, Mucin);
            data1.TableName = "ChiTiet1";
            fr.AddTable("ChiTiet1", data1);
            data1.Dispose();

            DataTable dtDonViNew = HamChung.SelectDistinct("ChiTietCha", data, "sKyHieu_2", "sKyHieu_2,sTenTK", "sKyHieu_2", "");
            dtDonViNew.TableName = "ChiTietCha";
            fr.AddTable("ChiTietCha", dtDonViNew);
            dtDonViNew.Dispose();


            //DataTable dtDonVi = HamChung.SelectDistinct("NS_DonVi", data, "iID_MaDonVi", "iID_MaDonVi,TenDV,sTenTomTat,sKyHieu_2", "", "");
            //dtDonVi.TableName = "DonVi";
            //fr.AddTable("DonVi", dtDonVi);
            //dtDonVi.Dispose();
            DataTable dtPB = HamChung.SelectDistinct("KT_TaiKhoanDanhMucChiTiet", data1, "sTen", "sTen", "", "");
            dtPB.TableName = "PB";
            fr.AddTable("PB", dtPB);
            dtPB.Dispose();
            DataTable dtND = HamChung.SelectDistinct("KT_TaiKhoanDanhMucChiTiet", data, "sKyHieu", "sKyHieu,sTen", "", "");
            dtND.TableName = "ND";
            fr.AddTable("ND", dtND);
            dtND.Dispose();


         
        }

        public DataTable ChiTietPhaiThu(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            DataTable dt = null;
            String DKDVT = "";
            if (DVT == "0")
            {
                DKDVT = "";
            }
            else if (DVT == "1")
            {
                DKDVT = "/1000 ";
            }
            else
            {
                DKDVT = "/1000000 ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String SQL = String.Format(@" SELECT sKyHieu,sKyHieu_2,sTenTK,sTen,SUM( rSoTien ){0} as SoTien
                                            FROM(
                                            SELECT * FROM (
                                            SELECT DISTINCT sTenTaiKhoanGiaiThich_No,
                                            rSoTien =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)=311 THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2, sTenTK=(select top 1 sTen from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3))
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_No like '311%' AND iThangCT<>0 AND iNgayCT<>0 AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND sTenTaiKhoanGiaiThich_No<>'' {1}
                                            GROUP BY sTenTaiKhoanGiaiThich_No) as a
                                            INNER JOIN (SELECT DISTINCT sKyHieu,sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1) as b
                                            ON a.sKyHieu_1=b.sKyHieu ) as KT_GiaiThich
                                            GROUP BY sKyHieu_2,sKyHieu,sTen,sTenTK
                                            ORDER BY sTen                                           
                                        ", DKDVT, iID_MaTrangThaiDuyet);

//            String SQL = String.Format(@" SELECT sKyHieu,sTen,SUM( rSoTien ){0} as SoTien
//                                            FROM(
//                                            SELECT * FROM (
//                                            SELECT DISTINCT sTenTaiKhoanGiaiThich_No,
//                                            rSoTien =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)=311 THEN rSoTien ELSE 0 END),
//                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1
//                                            FROM KT_ChungTuChiTiet
//                                            WHERE iTrangThai = 1  AND iID_MaTaiKhoan_No like '311%' AND iThangCT<>0 AND iNgayCT<>0 AND
//                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
//                                            AND sTenTaiKhoanGiaiThich_No<>'' {1}
//                                            GROUP BY sTenTaiKhoanGiaiThich_No) as a
//                                            INNER JOIN (SELECT DISTINCT sKyHieu,sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1) as b
//                                            ON a.sKyHieu_1=b.sKyHieu ) as KT_GiaiThich
//                                            GROUP BY sKyHieu,sTen
//                                            ORDER BY sTen                                           
//                                        ", DKDVT, iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            cmd.Dispose();
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;

        }
        public DataTable ChiTietPhaiThu1(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String Mucin)
        {
            DataTable dt = null;
            String DKDVT = "";
            if (DVT == "0")
            {
                DKDVT = "";
            }
            else if (DVT == "1")
            {
                DKDVT = "/1000 ";
            }
            else
            {
                DKDVT = "/1000000 ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String SQL = String.Format(@" SELECT iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,iID_MaDonVi,TenDV,sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),
                                            sTenTaiKhoanGiaiThich_No,sKyHieu,sTen,
--SoTien=CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){0} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{0} END
SoTien= (SUM(rSoTienNo)-SUM(rSoTienCo)){0}
,sKyHieu_2,sTenTK=(select top 1 sTen from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sKyHieuCha,1,4))
                                            FROM(
                                            SELECT * FROM (
                                            -- Lay ra so tien
                                            SELECT DISTINCT iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No,
                                            rSoTienNo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)='311' THEN rSoTien ELSE 0 END),
                                            rSoTienCo=0,
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
sKyHieuCha=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1)),
										SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec {1} AND iThangCT>@iThangCT AND iID_MaTaiKhoan_No like '311%' AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND sTenTaiKhoanGiaiThich_No<>'' 
                                            GROUP BY iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No
                                            union all
                                           SELECT DISTINCT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co,
                                                     rSoTienNo=0,
                                            rSoTienCo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1,3)='311' THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
 sKyHieuCha=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1)),
										SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) as sKyHieu_2
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec {1}  AND iThangCT>@iThangCT AND iID_MaTaiKhoan_Co like '311%' AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND sTenTaiKhoanGiaiThich_Co<>'' 
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co
                                            union all
                                              SELECT DISTINCT iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No,
                                            rSoTienNo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)='311' THEN rSoTien ELSE 0 END),
                                            rSoTienCo=0,
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
sKyHieuCha=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1)),
										SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2
                                            FROM KT_SoDuTaiKhoanGiaiThich
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_No like '311%'
                                            AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND sTenTaiKhoanGiaiThich_No<>'' 
                                            GROUP BY iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No
                                            union all
                                           SELECT DISTINCT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co,
                                                     rSoTienNo=0,
                                            rSoTienCo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1,3)='311' THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
 sKyHieuCha=(select top 1 sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where sKyHieu=SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1)),
										SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) as sKyHieu_2
                                            FROM KT_SoDuTaiKhoanGiaiThich
                                            WHERE iTrangThai = 1  and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_Co like '311%'
                                            AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND sTenTaiKhoanGiaiThich_Co<>'' 
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co
                                            
                                             -- End Lay ra so tien
                                            ) as a
                                            INNER JOIN (select iID_MaDonVi,sTen as TenDV from NS_DonVi where iTrangThai = 1 AND iNamLamViec_DonVi=@iNamLamViec) as C on C.iID_MaDonVi = a.iID_MaDonVi_No
                                            INNER JOIN (SELECT DISTINCT sKyHieu,sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 ) as b
                                            ON a.sKyHieu_1=b.sKyHieu ) as KT_GiaiThich
                                            GROUP BY iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,iID_MaDonVi,sKyHieu,TenDV,sTenTaiKhoanGiaiThich_No,sKyHieu_2,sTen,sKyHieuCha
                                            HAVING SUM(rSoTienNo)-SUM(rSoTienCo)!=0
                                            ORDER BY sTen
                                        ", DKDVT, iID_MaTrangThaiDuyet);

//           
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            int iThangSoDuDauNam = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
            cmd.Parameters.AddWithValue("@iThangCT", iThangSoDuDauNam);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;

        }
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }

        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "0";
            R1[1] = "Đồng";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            R2[1] = "Nghìn đồng";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "2";
            R3[1] = "Triệu đồng";
            dt.Dispose();
            return dt;
        }
        public static DataTable DanhSach_Mucin()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MucIn", typeof(String));
            dt.Columns.Add("TenIn", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "0";
            R1[1] = "In theo đơn vị - nội dung tài khoản chi tiết";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            R2[1] = "In theo đơn vị - nội dung tài khoản chi tiết";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "2";
            R3[1] = "In theo nội dung tài khoản chi tiết - theo đơn vị";
            dt.Dispose();
            return dt;
        }

        //public JsonResult Get_objNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        //{
        //    return Json(get_sNgayThang(ParentID, MaND, iThang, iNgay), JsonRequestBehavior.AllowGet);
        //}
        //public String get_sNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        //{
        //    String iNamLamViec = DateTime.Now.ToString();
        //   DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        //    {
        //        iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
        //    }
        //    dtCauHinh.Dispose();
        //    DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), false);
        //    SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        //    String S = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:55px;padding:2px;border:1px solid #dedede;\"onchange=ChonThang()");
        //    dtNgay.Dispose();

        //    return S;
        //}
        public JsonResult Get_objNgayThang(String ParentID,String MaND, String TenTruong, String iNgay, String iThang)
        {
            return Json(get_sNgayThang(ParentID,MaND, TenTruong, iNgay, iThang), JsonRequestBehavior.AllowGet);
        }
        public string get_sNgayThang(String ParentID,String MaND, String TenTruong, String iNgay, String iThang)
        {
            String iNam = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNam= dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNam), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            int SoNgayTrongThang = DateTime.DaysInMonth(Convert.ToInt16(iNam), Convert.ToInt16(iThang));
            if (String.IsNullOrEmpty(iNgay) == false)
            {
                if (Convert.ToInt16(iNgay) > SoNgayTrongThang)
                    iNgay = "1";
            }
            return MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, TenTruong, "", "style=\"width:70px\"");
        }
    }
}
