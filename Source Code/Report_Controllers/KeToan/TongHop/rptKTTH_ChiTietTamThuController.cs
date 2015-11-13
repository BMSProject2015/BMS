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

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTK_ChiTietTamThuController : Controller
    {
        //
        // GET: /rptKTTK_ChiTietTamThu/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietTamThuA4.xls";
        private const String sFilePath_TV = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietTamThu_TienViet.xls";
        private const String sFilePath_NT = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietTamThu_NgoaiTe.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ChiTietTamThu.aspx";
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
            String NoiDung = Convert.ToString(Request.Form[ParentID + "_NoiDung"]);
            ViewData["PageLoad"] = "1";
            ViewData["iNgay"] = iNgay;
            ViewData["iThang"] = iThang;
            ViewData["DonViTinh"] = DonViTinh;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["PageLoad"] = "1";
            ViewData["NoiDung"] = NoiDung;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ChiTietTamThu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String NoiDung)
        {
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            XlsFile Result = new XlsFile(true);
           // Result.Open(Server.MapPath(path));
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTH_ChiTietPhaiThu");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, iNamLamViec, NoiDung);
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

            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("iThang", iThang);
            fr.SetValue("iNgay", iNgay);
            fr.SetValue("ngay", ngay);
            fr.SetValue("LoaiBaoCao", DonViTinh);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String NoiDung)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (NoiDung == "-1")
            {
                DuongDanFile = sFilePath;

            }
            else if (NoiDung == "0")
            {
                DuongDanFile = sFilePath_NT;

            }
            else
            {
                DuongDanFile = sFilePath_TV;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, NoiDung);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKeToan_ChiTietTamThu.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String NoiDung)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (NoiDung == "-1")
            {
                DuongDanFile = sFilePath;

            }
            else if (NoiDung == "0")
            {
                DuongDanFile = sFilePath_NT;

            }
            else
            {
                DuongDanFile = sFilePath_TV;
            }


            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, NoiDung);
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
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String iNamLamViec, String NoiDung)
        {
            if (NoiDung != "0")// khong la ngoai te
            {
                DataTable data = dtKTTongHop_ChiTietTamThu_TienViet(iNamLamViec, iThang, iNgay, DonViTinh,
                                                                    iID_MaTrangThaiDuyet, NoiDung);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                data.Dispose();

                DataTable dtDonViNew = HamChung.SelectDistinct("ChiTietCha", data, "sKyHieu_2", "sKyHieu_2,sTenCapCha",
                                                               "sKyHieu_2", "");
                dtDonViNew.TableName = "ChiTietCha";
                fr.AddTable("ChiTietCha", dtDonViNew);
                dtDonViNew.Dispose();
            }
            if (NoiDung == "0" || NoiDung == "-1")//  la ngoai te
            {
                DataTable dataNgoaiTe = dtKTTongHop_ChiTietTamThu_NgoaiTe(iNamLamViec, iThang, iNgay, DonViTinh,
                                                                          iID_MaTrangThaiDuyet,NoiDung);
                dataNgoaiTe.TableName = "ChiTietNgoaiTe";
                fr.AddTable("ChiTietNgoaiTe", dataNgoaiTe);
                dataNgoaiTe.Dispose();
            }


        }
        public DataTable dtKTTongHop_ChiTietTamThu_TienViet(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String NoiDung)
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
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String KyHieuCha = "";
            if (NoiDung!="0" && NoiDung!="-1")
            {
                KyHieuCha = " AND sKyHieu_2=@sKyHieu_2";
            }
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String SQL = String.Format(@"select iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No, sTenTaiKhoanGiaiThich_No,sKyHieu_1,sKyHieu_2,
                                           -- SoTien=CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){0} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{0} END,
                                            SoTien=(SUM(rSoTienCo)-SUM(rSoTienNo)){0},
                                            sTenCapCha=(select TOP 1 sTen from KT_TaiKhoanDanhMucChiTiet where sKyHieu=sKyHieu_2 AND sXauNoiMa_Cha IS NULL),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No))as sTen,
                                              SUBSTRING(sTenDonVi_No,CHARINDEX('-',sTenDonVi_No)+1,LEN(sTenDonVi_No))as TenDV,
                                              sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No)
                                            
                                             from (SELECT DISTINCT iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No,
                                            rSoTienNo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)='331' THEN rSoTien ELSE 0 END),
                                            rSoTienCo=0,
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2
										
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1  and iNamLamViec=@iNamLamViec {1} AND iThangCT>@iThangCT AND iID_MaTaiKhoan_No like '331%' AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND sTenTaiKhoanGiaiThich_No<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) IN ('331','332','333','334')
                                            GROUP BY iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No
                                            union all
                                           SELECT DISTINCT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co,
                                                     rSoTienNo=0,
                                            rSoTienCo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1,3)='331' THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) as sKyHieu_2
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec {1} AND iThangCT>@iThangCT AND iID_MaTaiKhoan_Co like '331%' AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND sTenTaiKhoanGiaiThich_Co<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) IN ('331','332','333','334')
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co
                                            union all
                                              SELECT DISTINCT iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No,
                                            rSoTienNo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)='331' THEN rSoTien ELSE 0 END),
                                            rSoTienCo=0,
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2
                                            FROM KT_SoDuTaiKhoanGiaiThich
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_No like '331%'
                                            AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND sTenTaiKhoanGiaiThich_No<>''  AND SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) IN ('331','332','333','334')
                                            GROUP BY iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No
                                            union all
                                           SELECT DISTINCT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co,
                                                     rSoTienNo=0,
                                            rSoTienCo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1,3)='331' THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) as sKyHieu_2
                                            FROM KT_SoDuTaiKhoanGiaiThich
                                            WHERE iTrangThai = 1  and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_Co like '331%'
                                            AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND sTenTaiKhoanGiaiThich_Co<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) IN ('331','332','333','334')
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co) as a
                                            
                                            WHERE 1=1 {2}     GROUP BY iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,sTenTaiKhoanGiaiThich_No,sKyHieu_2,sKyHieu_1
HAVING SUM(rSoTienNo)-SUM(rSoTienCo)!=0
                                            ORDER BY SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No))", DKDVT, iID_MaTrangThaiDuyet, KyHieuCha);

        
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (NoiDung != "0" && NoiDung != "-1")
            {
                cmd.Parameters.AddWithValue("@sKyHieu_2", NoiDung);
                
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
        public DataTable dtKTTongHop_ChiTietTamThu_NgoaiTe(String iNamLamViec, String iThang, String iNgay, String DVT, String iID_MaTrangThaiDuyet, String NoiDung)
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
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String SQL = String.Format(@"select iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No, sTenTaiKhoanGiaiThich_No,sKyHieu_1,sKyHieu_2,
                                            --SoTien=CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)){0} ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1{0} END,
                                               SoTien=(SUM(rSoTienCo)-SUM(rSoTienNo)){0},
                                            sTenCapCha=( select TOP 1 sTen from KT_TaiKhoanDanhMucChiTiet where sKyHieu=sKyHieu_2 AND sXauNoiMa_Cha IS NULL),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No))as sTen,
                                              SUBSTRING(sTenDonVi_No,CHARINDEX('-',sTenDonVi_No)+1,LEN(sTenDonVi_No))as TenDV,
                                              sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),
                                             rSoTienNgoaiTe=(select top 1 rSoTienNgoaiTe from KT_TaiKhoanDanhMucChiTiet where sKyHieu=sKyHieu_1),
  sTenNgoaiTe=(select top 1 sTenNgoaiTe from KT_TaiKhoanDanhMucChiTiet where sKyHieu=sKyHieu_1)
                                             from (SELECT DISTINCT iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No,
                                            rSoTienNo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)='331' THEN rSoTien ELSE 0 END),
                                            rSoTienCo=0,
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2
										
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec {1} AND iThangCT>@iThangCT AND iID_MaTaiKhoan_No like '331%' AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND sTenTaiKhoanGiaiThich_No<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_No,1,1)='2'
                                            GROUP BY iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No
                                            union
                                           SELECT DISTINCT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co,
                                                     rSoTienNo=0,
                                            rSoTienCo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1,3)='331' THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) as sKyHieu_2
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai = 1 and iNamLamViec=@iNamLamViec {1} AND iThangCT>@iThangCT AND iID_MaTaiKhoan_Co like '331%' AND
                                            CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                            AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND sTenTaiKhoanGiaiThich_Co<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,1)='2'
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co
                                            union
                                              SELECT DISTINCT iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No,
                                            rSoTienNo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_No,1,3)='331' THEN rSoTien ELSE 0 END),
                                            rSoTienCo=0,
                                            SUBSTRING(sTenTaiKhoanGiaiThich_No,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_No,1,3) as sKyHieu_2
                                            FROM KT_SoDuTaiKhoanGiaiThich
                                            WHERE iTrangThai = 1  and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_No like '331%'
                                            AND iID_MaDonVi_No IS NOT NULL AND iID_MaDonVi_No<>'' AND sTenTaiKhoanGiaiThich_No<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_No,1,1)='2'
                                            GROUP BY iID_MaTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,sTenTaiKhoanGiaiThich_No
                                            union
                                           SELECT DISTINCT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co,
                                                     rSoTienNo=0,
                                            rSoTienCo =SUM(CASE WHEN SUBSTRING(iID_MaTaiKhoan_Co,1,3)='331' THEN rSoTien ELSE 0 END),
                                            SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,CHARINDEX('-',sTenTaiKhoanGiaiThich_Co)-1) as sKyHieu_1,
										SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,3) as sKyHieu_2
                                            FROM KT_SoDuTaiKhoanGiaiThich
                                            WHERE iTrangThai = 1  and iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_Co like '331%'
                                            AND iID_MaDonVi_Co IS NOT NULL AND iID_MaDonVi_Co<>'' AND sTenTaiKhoanGiaiThich_Co<>'' AND SUBSTRING(sTenTaiKhoanGiaiThich_Co,1,1)='2'
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,sTenTaiKhoanGiaiThich_Co) as a
                                            
                                                 GROUP BY iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,sTenTaiKhoanGiaiThich_No,sKyHieu_2,sKyHieu_1 
having SUM(rSoTienNo)-SUM(rSoTienCo)!=0
                                            ORDER BY SUBSTRING(sTenTaiKhoanGiaiThich_No,CHARINDEX('-',sTenTaiKhoanGiaiThich_No)+1,LEN(sTenTaiKhoanGiaiThich_No))", DKDVT, iID_MaTrangThaiDuyet);


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
        public JsonResult Get_objNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        {
            return Json(get_sNgayThang(ParentID, MaND, iThang, iNgay), JsonRequestBehavior.AllowGet);
        }
        public String get_sNgayThang(String ParentID, String MaND, String iThang, String iNgay)
        {
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            String S = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:100%;\"");
            dtNgay.Dispose();

            return S;
        }
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable DonViTinh()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_DonViTinh", (typeof(string)));
            dt.Columns.Add("TenDonVi", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_DonViTinh"] = "0";
            dr["TenDonVi"] = "Đồng";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_DonViTinh"] = "1";
            dr1["TenDonVi"] = "Nghìn Đồng";
            dt.Rows.InsertAt(dr1, 1);

            DataRow dr2 = dt.NewRow();
            dr2["iID_DonViTinh"] = "2";
            dr2["TenDonVi"] = "Triệu đồng";
            dt.Rows.InsertAt(dr2, 2);


            return dt;
        }

        public static DataTable DT_Loai(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "", Boolean TaiKhoanCha = false)
        {
            DataTable vR = new DataTable();
            String SQL =
                @"select distinct ct.sKyHieu, ct.sTen from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and
tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet = tk.iID_MaTaiKhoanDanhMucChiTiet and tk.iID_MaTaiKhoan like '331%'
and ct.sXauNoiMa=1 order by ct.sTen";
            SqlCommand cmd = new SqlCommand(SQL);

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow R_NT = vR.NewRow();
            R_NT["sKyHieu"] = "0";
            R_NT["sTen"] = "In riêng Ngoại tệ";
            vR.Rows.Add(R_NT);
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

