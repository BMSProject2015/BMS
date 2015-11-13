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
    public class rptKTCS_KeKhaiOtO_Mau7Controller : Controller
    {
        //
        // GET: /rptKTCS_KeKhaiOtO_Mau7/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String LoaiBieu = "",String KhoGiay="")
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO_A3.xls";
            }
            else
            {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO.xls";
            }
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_KeKhaiOtO_Mau7.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Action Thực hiện truyền các tham số trên form
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String LoaiBieu = Request.Form[ParentID + "_LoaiBieu"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Hiển thị dữ liệu
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên đơn vị
            String tendv = "";
            DataTable teN = TenDonVi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
             String TenLoaiTaiSan = "";
            DataTable dtLoaiTaiSan = dtTenLoaiTaiSan(iID_MaLoaiTaiSan);
            if (dtLoaiTaiSan.Rows.Count > 0)
            {
                TenLoaiTaiSan = dtLoaiTaiSan.Rows[0][0].ToString();
            }
            
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //Hàm đổi số tiền ra chữ
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_KeKhaiOtO_Mau7");
            LoadData(fr, NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS, KhoGiay);
            fr.SetValue("Nam", NamChungTu);
            if (TongHopDonVi == "on")
            {
                fr.SetValue("TenDV", "Tổng hợp đơn vị");
            }
            else
            {
                fr.SetValue("TenDV", "Tên đơn vị :" + tendv);
            }
            if (TongHopLTS == "on")
            {
                fr.SetValue("TenLoaiTaiSan", "Tổng hợp loại tài sản");
            }
            else
            {
                fr.SetValue("TenLoaiTaiSan","Tên Loại Tài Sản :"+TenLoaiTaiSan);
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Ngay", NgayThang);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// DataTable lấy tên Loại tại sản
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
        /// DataTable lấy danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable ListDonVi()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT iID_MaDonVi,sTenDonVi FROM KTCS_ChungTuChiTiet WHERE iTrangThai=1 ORDER BY iID_MaDonVi");
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// DataTable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable rptKTCS_KeKhaiTaiSan(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS, String KhoGiay)
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
            String SQL = string.Format(@"SELECT
		                        B.iID_MaNhomTaiSan,D.sTen AS TenLoaiTaiSan,A.sTenTaiSan,sNhanHieu_OTo,sNuocSX,sBienKiemSoat_OTo,sSoChoNgoi_OTo,iNamSX,rThoiGianDuaVaoSuDung
		                        ,SUM(NguonNS) as NguonNS
		                        ,SUM(nguonKhac) as nguonKhac
		                        ,sDonViTinh,SUM(E.rGiaTriConLai/1000) as GiaTriCon
                        FROM
                        (
		                       SELECT iID_MaTaiSan,sTenTaiSan,SUM(rSoTien) as NguonNS
                                            ,SUM(rNguonKhac) as NguonKhac
                                             FROM KTCS_ChungTuChiTiet
                                             WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND( {0})
                                             GROUP BY   iID_MaTaiSan,sTenTaiSan
                        ) as A
                         INNER JOIN(SELECT iID_MaTaiSan,iID_MaNhomTaiSan,sDonViTinh FROM KTCS_TaiSan WHERE iTrangThai=1 AND( {1})) as B
                                          ON A.iID_MaTaiSan=B.iID_MaTaiSan
		                        INNER JOIN
		                        (
					                        SELECT iID_MaTaiSan,sTenTaiSan 
					                        ,sNhanHieu_OTo,sNuocSX,sBienKiemSoat_OTo,sSoChoNgoi_OTo,iNamSX,YEAR(dNgayDuaVaoKhauHao) as rThoiGianDuaVaoSuDung
                                            ,SUM(rGiaTriConLai) as GiaTriCon
					                        FROM KTCS_TaiSanChiTiet AS TSCT
					                        WHERE iTrangThai=1 AND iLoaiTS=3 
                                            GROUP BY iID_MaTaiSan,sTenTaiSan 
					                        ,sNhanHieu_OTo,sNuocSX,sBienKiemSoat_OTo,sSoChoNgoi_OTo,iNamSX,dNgayDuaVaoKhauHao
		                        ) as C
                        ON A.iID_MaTaiSan=C.iID_MaTaiSan
		                        INNER JOIN
		                        (
				                        SELECT iID_MaNhomTaiSan,sTen 
				                        FROM KTCS_NhomTaiSan AS LTS
				                        WHERE iTrangThai=1 
		                        ) as D
                        ON B.iID_MaNhomTaiSan=D.iID_MaNhomTaiSan
 INNER JOIN (SELECT * FROM KTCS_KhauHaoHangNam WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec)  AS E ON A.iID_MaTaiSan=E.iID_MaTaiSan
                        GROUP BY B.iID_MaNhomTaiSan,A.sTenTaiSan,sNhanHieu_OTo,sNuocSX,sBienKiemSoat_OTo,sSoChoNgoi_OTo
                        ,iNamSX,rThoiGianDuaVaoSuDung,sDonViTinh,D.sTen,GiaTriCon", DKDonVi, DKNhomTaiSan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue(@"NamLamViec",NamChungTu);
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
            SqlCommand cmd = new SqlCommand("SELECT  sTenDonVi FROM KTCS_ChungTuChiTiet WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// hiển thị dữ liệu
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            DataTable data = rptKTCS_KeKhaiTaiSan(NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            DataTable dtTaiSan;
            dtTaiSan = HamChung.SelectDistinct("TaiSan", data, "TenLoaiTaiSan", "TenLoaiTaiSan", "");
            fr.AddTable("TaiSan", dtTaiSan);
            dtTaiSan.Dispose();

        }
        /// <summary>
        /// Action Thực hiện xuất dữ liệu ra file PDF
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS, KhoGiay);
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
        /// <summary>
        /// Action thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeKhaiTaiSan.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Xem báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiXeOTO.xls";
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS, KhoGiay);
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
        /// <summary>
        /// lấy danh sách loại tài sản
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_LoaiTS()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT iID_MaNhomTaiSan
                                                ,iID_MaNhomTaiSan+' - '+sTen as TenHT
                                                 FROM KTCS_NhomTaiSan
                                                 WHERE iTrangThai=1 
                                                 AND SUBSTRING(iID_MaNhomTaiSan,1,2)='13' 
                                                ");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
