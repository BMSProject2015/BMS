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
using System.Collections.Specialized;

namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptKTCS_KeKhaiNhaDat_6Controller : Controller
    {
        //
        // GET: /rptKTCS_KeKhaiNhaDat_6/
        //
        // GET: /rptKTCS_SoTheoDoiTSCD/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiNhaDat_6_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiNhaDat_6.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_KeKhaiNhaDat_6.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Acion thực hiện truyền các tham số trên form
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHopDonVi"] = TongHopDonVi;
            ViewData["TongHopLTS"] = TongHopLTS;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_KeKhaiNhaDat_6.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Xuất ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên tài sản
            String TenLoaiTaiSan = "";
            DataTable dtLoaiTaiSan = dtTenLoaiTaiSan(iID_MaLoaiTaiSan);
            if (dtLoaiTaiSan.Rows.Count > 0)
            {
                TenLoaiTaiSan = dtLoaiTaiSan.Rows[0][0].ToString();
            }
            //Lấy tên đơn vị
            String tendv = "";
            DataTable teN = TenDonVi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            else
            {
                tendv = "";
            }
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //Hàm đổi tiền từ số sang chữ
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_KeKhaiNhaDat_6");
            LoadData(fr, NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            fr.SetValue("Nam", NamChungTu);
            if (TongHopDonVi == "on")
            {
                fr.SetValue("TenDV", "Tổng hợp các đơn vị");
            }
            else
            {
                fr.SetValue("TenDV", tendv);
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("TongCucKiThuat", "TỔNG CỤC KĨ THUẬT");
            if (TongHopDonVi == "on")
            {
                fr.SetValue("iID_MaDonVi", "");
            }
            fr.SetValue("Ngay", NgayThang);
            if (TongHopLTS == "on")
            {
                fr.SetValue("TenLoaiTS", "Tất cả loại tài sản");
            }
            else { fr.SetValue("TenLoaiTS", TenLoaiTaiSan); }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// DataTable lấy tên loại tài sản
        /// </summary>
        /// <returns></returns>
        public DataTable dtTenLoaiTaiSan(String iID_MaLoaiTaiSan)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM KTCS_LoaiTaiSan WHERE iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
            return dt = Connection.GetDataTable(cmd);
        }
      
        /// <summary>
        /// DataTable lấy dự liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable rptKTCS_KeKhaiTaiSan(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {

            DataTable dt = null;
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }

            String DKDonVi = "iID_MaDonVi='-111'", DKNhomTaiSan = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = KTCS_ReportModel.ListDonVi();
                if (dtDonVi.Rows.Count > 0)
                {
                    DKDonVi = "";
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DKDonVi += "iID_MaDonVi = '" + dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + "'";
                        if (i < dtDonVi.Rows.Count - 1)
                            DKDonVi += " OR ";

                    }
                    dtDonVi.Dispose();
                }
            }
            else
            {
                DKDonVi = "";
                DKDonVi = " iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'";
            }
            if (TongHopLTS == "on")
            {
                DataTable dtNhomTaiSan = DT_LoaiTS();
                for (int i = 0; i < dtNhomTaiSan.Rows.Count; i++)
                {
                    DKNhomTaiSan += "iID_MaNhomTaiSan like N'" + dtNhomTaiSan.Rows[i]["iID_MaNhomTaiSan"].ToString() + "%'";
                    if (i < dtNhomTaiSan.Rows.Count - 1)
                        DKNhomTaiSan += " OR ";
                }
            }
            else
            {
                DKNhomTaiSan = " iID_MaNhomTaiSan LIKE N'" + iID_MaLoaiTaiSan + "%'";
            }


            String SQL = string.Format(@" SELECT 
                                        C.iID_MaTaiSan,C.sTenTaiSan,sCapHang_Nha,iSoTang_Nha,rThoiGianDuaVaoSuDung
                                        ,SUM(NguonNS)/1000 as NguonNS
                                        ,SUM(NguonKhac)/1000 as NguonKhac
                                        ,SUM(DTSanNha) as DTSanNha,SUM(TongDTSanNha) as TongDTSanNha,SUM(NhaLamViec) as NhaLamViec
                                        ,SUM(CoSoHDBS) as CoSoHDBS,SUM(D.rGiaTriConLai/1000) as GiaTriCon,SUM(LamNhaO) as LamNhaO
                                        ,SUM(ChoThue) as ChoThue,SUM(BoTrong) as BoTrong,SUM(BiLanChiem) as BiLanChiem
                                        ,SUM(DTKhac) as DTKhac
                                        FROM
                                        (
                                            SELECT iID_MaTaiSan,sTenTaiSan,SUM(rSoTien) as NguonNS
                                            ,SUM(rNguonKhac) as NguonKhac
                                             FROM KTCS_ChungTuChiTiet
                                             WHERE iTrangThai=1 AND iNamLamViec<=@NamLamViec AND( {0})
                                             GROUP BY   iID_MaTaiSan,sTenTaiSan
                                         )AS A
                                         INNER JOIN(SELECT iID_MaTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 AND( {1})) as B
                                          ON A.iID_MaTaiSan=B.iID_MaTaiSan
                                         INNER JOIN
                                         (
                                             SELECT iID_MaTaiSan,iLoaiTS,sTenTaiSan
                                             ,sCapHang_Nha,iSoTang_Nha,YEAR(dNgayDuaVaoKhauHao) as rThoiGianDuaVaoSuDung
                                             ,SUM(rDTXayDung_Nha) as DTSanNha
                                            ,SUM(rTongDTSanNha_Nha) as TongDTSanNha
                                            ,SUM(rDTLamNhaLamViec) as NhaLamViec
                                            ,SUM(rDTLamCoSoHDSN) as CoSoHDBS
                                            --,SUM(rGiaTriConLai) as GiaTriCon
                                            ,SUM(rDTLamNhaO) as LamNhaO
                                            ,SUM(rDTChoThue) as ChoThue 
                                            ,SUM(rDTBoTrong) as BoTrong
                                            ,SUM(rDTBiLanChiem) as BiLanChiem 
                                            ,SUM(rDTKhac) as DTKhac 
                                             FROM KTCS_TaiSanChiTiet
                                             WHERE iTrangThai=1 AND iLoaiTS IN (2) 
                                             GROUP BY iID_MaTaiSan,iLoaiTS,sTenTaiSan
                                             ,sCapHang_Nha,iSoTang_Nha,dNgayDuaVaoKhauHao
                                             HAVING SUM(rDTXayDung_Nha)!=0 
                                            OR SUM(rGiaTriConLai)!=0 OR SUM(rDTLamNhaLamViec)!=0 OR SUM(rDTLamCoSoHDSN)!=0 
                                            OR SUM(rDTLamNhaO)!=0 OR SUM(rDTChoThue)!=0 OR SUM(rDTBoTrong)!=0 OR SUM(rDTBiLanChiem)!=0 OR SUM(rDTKhac)!=0
                                        ) as C
                                        ON A.iID_MaTaiSan=C.iID_MaTaiSan
                                        INNER JOIN (SELECT * FROM KTCS_KhauHaoHangNam WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec)  AS D ON A.iID_MaTaiSan=D.iID_MaTaiSan
                                        GROUP BY C.iID_MaTaiSan,C.sTenTaiSan,sCapHang_Nha,iSoTang_Nha,rThoiGianDuaVaoSuDung", DKDonVi, DKNhomTaiSan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamLamViec", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable dtDienTichDat(String NamChungTu)
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT SUM(B.rDTKhuonVien) as DTKhuonVien
                                        ,HienTrangSuDung=SUM(rDTLamNhaLamViec)+SUM(rDTLamCoSoHDSN)+SUM(rDTLamNhaO)
                                        +SUM(rDTChoThue)+SUM(rDTBoTrong)+SUM(rDTBiLanChiem)+SUM(rDTKhac)
                                        ,SUM(rNguyenGia) as GiaTriNguyenGia FROM KTCS_KhauHaoHangNam AS A
                                        INNER JOIN KTCS_TaiSanChiTiet AS B 
                                        ON A.iID_MaTaiSan=B.iID_MaTaiSan
                                        WHERE A.iTrangThai=1 AND iNamLamViec=@NamChungTu AND iLoaiTS=1");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamChungTu", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// DataTable lấy tên đơn vị
        /// </summary>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen as sTenDonVi FROM NS_DonVi WHERE iID_MaDonVi=@ID AND iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec");
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Load dự liệu ra báo cao
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            DataTable data = rptKTCS_KeKhaiTaiSan(NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            DataTable DienTich = dtDienTichDat(NamChungTu);
            DienTich.TableName = "DienTich";
            fr.AddTable("DienTich", DienTich);
            DienTich.Dispose();
        }
        /// <summary>
        /// Action thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiNhaDat_6_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiNhaDat_6.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "SoTheoDoiTaiSanCoDinh_A3";
                }
                else
                {
                    clsResult.FileName = "SoTheoDoiTaiSanCoDinh";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Action Xem báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiNhaDat_6_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiNhaDat_6.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
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
        public static DataTable ListDonVi()
        {
            DataTable dt;
            String SQL = String.Format(@"SELECT NS_DonVi.iID_MaDonVi,sTen, NS_DonVi.iID_MaDonVi + '-' + sTen as TenHT
                                        FROM KTCS_TaiSan_DonVi
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi 
                                        ON KTCS_TaiSan_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        ORDER BY NS_DonVi.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", "2012");
            return dt = Connection.GetDataTable(cmd);
        }
        public static DataTable DT_LoaiTS()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT iID_MaNhomTaiSan
                                                ,iID_MaNhomTaiSan+' - '+sTen as TenHT
                                                 FROM KTCS_NhomTaiSan
                                                 WHERE iTrangThai=1 
                                                 AND (SUBSTRING(iID_MaNhomTaiSan,1,2)='11' 
                                                 OR SUBSTRING(iID_MaNhomTaiSan,1,2)='21')");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
