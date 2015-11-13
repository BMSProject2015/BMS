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
    public class rptKTCS_KeKhaiTaiSanController : Controller
    {
        //
        // GET: /rptKTCS_KeKhaiTaiSan/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String LoaiBieu = "",String KhoGiay="")
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiTaiSan_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiTaiSan.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_KeKhaiTaiSan.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
            }
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

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
            String optLoai = Convert.ToString(Request.Form[ParentID + "_optLoai"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHopDonVi"] = TongHopDonVi;
            ViewData["TongHopLTS"] = TongHopLTS;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["optLoai"] = optLoai;
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_KeKhaiTaiSan.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Hiển thị dữ liệu
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay, String optLoai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên đơn vị
            String tendv = "";
            DataTable teN = TenDonVi(iID_MaDonVi);
            if (teN.Rows.Count>0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
          
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            String Loai = "";
            if (optLoai == "1") Loai = "Loại tài sản =4";
            else if (optLoai == "2") Loai = "Toàn bộ - Trừ Đất,Nhà và Ô tô";
            else Loai = "Tổng hợp toàn bộ tài sản";
            FlexCelReport fr = new FlexCelReport();
            //Hàm đổi số tiền ra chữ
                fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_KeKhaiTaiSan");
                LoadData(fr, NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay, optLoai);
                fr.SetValue("Nam", NamChungTu);
                if (TongHopDonVi == "on")
                {
                    fr.SetValue("TenDV", "Tổng hợp đơn vị");
                }
                else
                {
                    fr.SetValue("TenDV", "Tên đơn vị :" + tendv);
                }
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("Loai", Loai);
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
        public static DataTable rptKTCS_KeKhaiTaiSan(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay,String optLoai)
        {

            DataTable dt = null;
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }

            String DKDonVi = "", DKNhomTaiSan = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = ListDonVi();
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += "KTCS.iID_MaDonVi = '" + dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + "'";
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";

                }
                dtDonVi.Dispose();
            }
            else
            {
                DKDonVi = " KTCS.iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'";
            }
            if (TongHopLTS == "on")
            {
                DataTable dtNhomTaiSan = KTCS_ReportModel.DT_LoaiTS();
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
            String DKLoai="";
            if (optLoai == "1")
                DKLoai = "iLoaiTS=4";
            else if (optLoai == "2")
                DKLoai = "iLoaiTS NOT IN (1,2,3)";
            else DKLoai = "1=1";
            String SQL = String.Format(@" SELECT 
                                            A.iID_MaTaiSan,sTenTaiSan,sNuocSX,iNamSX,B.rThoiGianDuaVaoSuDung,
                                                    SUM(NguonNS) as NguonNS
                                                    ,SUM(NguonKhac) as NguonKhac
                                                    ,SUM(D.rGiaTriConLai/1000) as GiaTriCon
                                                    ,sDonViTinh
                                                    FROM
                                                    (
                                                            SELECT KTCS.iID_MaTaiSan,KTCS.sTenTaiSan
                                                            ,SUM(rSoTien) as NguonNS
                                                            ,SUM(rNguonKhac) as NguonKhac
                                                            ,SUM(rGiaTriCon) as GiaTriCon
                                                            ,sDonViTinh
                                                            FROM KTCS_ChungTuChiTiet AS KTCS
                                                            INNER JOIN KTCS_TaiSan AS TS
                                                            ON KTCS.iID_MaTaiSan=TS.iID_MaTaiSan
                                                            WHERE KTCS.iTrangThai=1 AND KTCS.iNamLamViec=@NamChungTu AND( {0})
                                                            GROUP BY KTCS.iID_MaTaiSan,KTCS.sTenTaiSan,sDonViTinh
                                                           -- HAVING SUM(rNguonNganSach)>=500000000 OR SUM(rNguonKhac)>=500000000 
                                                    ) as A
                                                    INNER JOIN
                                                    (
                                                            SELECT iID_MaTaiSan,iLoaiTS,sNuocSX,iNamSX,YEAR(dNgayDuaVaoKhauHao) as rThoiGianDuaVaoSuDung
                                                            FROM KTCS_TaiSanChiTiet 
                                                            WHERE iTrangThai=1 AND {2} 
                                                    ) as B 
                                            ON A.iID_MaTaiSan=B.iID_MaTaiSan
                                            INNER JOIN(SELECT iID_MaTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 AND( {1})) as C
                                          ON A.iID_MaTaiSan=C.iID_MaTaiSan
                                        INNER JOIN (SELECT * FROM KTCS_KhauHaoHangNam WHERE iTrangThai=1 AND iNamLamViec=@NamChungTu)  AS D ON A.iID_MaTaiSan=D.iID_MaTaiSan
                                            WHERE  {2} 
                                            GROUP BY A.iID_MaTaiSan,sTenTaiSan,sNuocSX,iNamSX,rThoiGianDuaVaoSuDung,sDonViTinh ", DKDonVi, DKNhomTaiSan, DKLoai);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamChungTu", NamChungTu);
            
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
   
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
        private void LoadData(FlexCelReport fr, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay,String optLoai)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            DataTable data = rptKTCS_KeKhaiTaiSan(NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay, optLoai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

        }
       
        /// <summary>
        /// Action thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay, String optLoai)
        {
            String sFilePath = "";

            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiTaiSan_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiTaiSan.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay, optLoai);

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
        public ActionResult ViewPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay, String optLoai)
        {
            String sFilePath = "";

            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiTaiSan_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_KeKhaiTaiSan.xls";
            }
                HamChung.Language();
                ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay, optLoai);
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
       
    }
}
