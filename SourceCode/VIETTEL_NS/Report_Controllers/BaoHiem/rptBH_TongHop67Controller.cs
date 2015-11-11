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

namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_TongHop67Controller : Controller
    {
        //
        // GET: /rptBH_TongHop67/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_TongHop67.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Action thực hiện các điều kiện lọc để xuất ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String Thang_Quy = "";
            String LoaiThangQuy = Convert.ToString(Request.Form[ParentID + "_LoaiThangQuy"]);
            if (LoaiThangQuy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String ToSo = Request.Form[ParentID + "_ToSo"];
            ViewData["PageLoad"] = "1";
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["ToSo"] = ToSo;
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_TongHop67.aspx";
            return View(sViewPath + "ReportView.aspx");
          //  return RedirectToAction("Index", new { LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, LoaiBaoCao = LoaiBaoCao, KhoGiay = KhoGiay, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        /// <summary>
        /// Action view PDF
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String LoaiThangQuy, String Thang_Quy, String LoaiBaoCao, String KhoGiay, int ToSo, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "";
            }
            ExcelFile xls = CreateReport(LoaiThangQuy, Thang_Quy, LoaiBaoCao, KhoGiay, ToSo, iID_MaTrangThaiDuyet);
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
        }
        /// <summary>
        /// DataTable dữ liệu báo cáo
        /// </summary>
        /// <returns></returns>
        /// 
        private String QueryChienSy(String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet, String TruongDonVi)
        {
            String MaND = User.Identity.Name;
            String DK_Duyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = " AND QT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }

            String DKThangQuy = " AND QT.iThang_Quy=@iThangQuy";
            if (LoaiThangQuy == "1")
            {
                int DenThang = Convert.ToInt16(Thang_Quy) * 3;
                int TuThang = DenThang - 2;
                DKThangQuy = String.Format(@" AND QT.iThang_Quy between {0} and {1}", TuThang, DenThang);
            }

            //Truong don vi dang @DonVi1, sau do phai them param vao
            String DK_DonVi = "";
            if (!String.IsNullOrEmpty(TruongDonVi)) DK_DonVi = " AND QT.iID_MaDonVi = " + TruongDonVi;
            String query = String.Format(@"(SELECT TOP 1 TongSo=A.BinhNhat+A.BinhNhi+A.HaSi
                                        +A.ThuongSi+A.TrungSi
                                        FROM
                                        (
                                        SELECT QT.iID_MaDonVi,DV.sTen,SUM(rBinhNhat) BinhNhat
                                        ,SUM(rBinhNhi) BinhNhi,SUM(rHaSi) HaSi
                                        ,SUM(rTrungSi) TrungSi,SUM(rThuongSi) ThuongSi
                                         FROM QTQS_ChungTuChiTiet as QT
                                          INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV 
                                         ON QT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE QT.iTrangThai=1 {0} {1} {2} {3}
                                        GROUP BY QT.iID_MaDonVi,DV.sTen
                                        HAVING SUM(rBinhNhat)!=0 OR SUM(rBinhNhi)!=0 OR SUM(rHaSi)!=0
                                        OR SUM(rTrungSi)!=0 OR SUM(rThuongSi)!=0
                                        )
                                        as A
                                        GROUP BY BinhNhat,BinhNhi,HaSi,ThuongSi,TrungSi)", ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DKThangQuy, DK_DonVi);
            return query;
        }
        private String QueryChienSyDenKy(String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet, String TruongDonVi)
        {
            String MaND = User.Identity.Name;
            String DK_Duyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "AND QT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }

            String DKThangQuy = " AND QT.iThang_Quy >= 1 AND QT.iThang_Quy <= @iThangQuy";
            if (LoaiThangQuy == "1")
            {
                int DenThang = Convert.ToInt16(Thang_Quy) * 3;
                DKThangQuy = String.Format(@" AND QT.iThang_Quy >= 1 AND QT.iThang_Quy <= {0}", DenThang);
            }

            //Truong don vi dang @DonVi1, sau do phai them param vao
            String DK_DonVi = "";
            if (!String.IsNullOrEmpty(TruongDonVi)) DK_DonVi = " AND QT.iID_MaDonVi = " + TruongDonVi;
            String query = String.Format(@"(SELECT TOP 1 TongSo=A.BinhNhat+A.BinhNhi+A.HaSi
                                        +A.ThuongSi+A.TrungSi
                                        FROM
                                        (
                                        SELECT QT.iID_MaDonVi,DV.sTen,SUM(rBinhNhat) BinhNhat
                                        ,SUM(rBinhNhi) BinhNhi,SUM(rHaSi) HaSi
                                        ,SUM(rTrungSi) TrungSi,SUM(rThuongSi) ThuongSi
                                         FROM QTQS_ChungTuChiTiet as QT
                                          INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV 
                                         ON QT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE QT.iTrangThai=1 {0} {1} {2} {3}
                                        GROUP BY QT.iID_MaDonVi,DV.sTen
                                        HAVING SUM(rBinhNhat)!=0 OR SUM(rBinhNhi)!=0 OR SUM(rHaSi)!=0
                                        OR SUM(rTrungSi)!=0 OR SUM(rThuongSi)!=0
                                        )
                                        as A
                                        GROUP BY BinhNhat,BinhNhi,HaSi,ThuongSi,TrungSi)", ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DKThangQuy, DK_DonVi);
            return query;
        }
        private String QueryTrongKy(String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet, String TruongDonVi, String TruongKyHieu)
        {
            String MaND = User.Identity.Name;
            String DK_Duyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = " AND tbl_CT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }

            String DKThangQuy = " AND tbl_CT.iThang_Quy=@iThangQuy";
            if (LoaiThangQuy == "1")
            {
                int DenThang = Convert.ToInt16(Thang_Quy) * 3;
                int TuThang = DenThang - 2;
                DKThangQuy = String.Format(@" AND tbl_CT.iThang_Quy between {0} and {1}", TuThang, DenThang);
            }

            //Truong don vi dang @DonVi1, sau do phai them param vao
            String DK_DonVi = "";
            if (!String.IsNullOrEmpty(TruongDonVi)) DK_DonVi = " AND tbl_CT.iID_MaDonVi = " + TruongDonVi;
            String query = String.Format(@"(SELECT SUM(tbl_CT.rTongSo) FROM BH_PhaiThuChungTuChiTiet AS tbl_CT
                                        WHERE tbl_CT.iTrangThai=1 AND tbl_CT.sKyHieu ={0} {1} {2} {3} {4})", TruongKyHieu, ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DKThangQuy, DK_DonVi);
            return query;
        }
        private String QueryDenKy(String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet, String TruongDonVi, String TruongKyHieu)
        {
            String MaND = User.Identity.Name;
            String DK_Duyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = " AND tbl_CT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }

            String DKThangQuy = " AND tbl_CT.iThang_Quy<=@iThangQuy";
            if (LoaiThangQuy == "1")
            {
                int DenThang = Convert.ToInt16(Thang_Quy) * 3;
                int TuThang = DenThang - 2;
                DKThangQuy = String.Format(@" AND tbl_CT.iThang_Quy between {0} and {1}", "1", DenThang);
            }

            //Truong don vi dang @DonVi1, sau do phai them param vao
            String DK_DonVi = "";
            if (!String.IsNullOrEmpty(TruongDonVi)) DK_DonVi = " AND tbl_CT.iID_MaDonVi = " + TruongDonVi;
            String query = String.Format(@"(SELECT SUM(tbl_CT.rTongSo) FROM BH_PhaiThuChungTuChiTiet AS tbl_CT
                                        WHERE tbl_CT.iTrangThai=1 AND tbl_CT.sKyHieu ={0} {1} {2} {3} {4})", TruongKyHieu, ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DKThangQuy, DK_DonVi);
            return query;
        }
        public DataTable rptBH_ThongTri67(String LoaiThangQuy, String Thang_Quy, String LoaiBaoCao, String KhoGiay, int ToSo, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (LoaiThangQuy == "1")
            {
                int DenThang = Convert.ToInt16(Thang_Quy) * 3;
                cmd.Parameters.AddWithValue("@iThang", DenThang-2);
                cmd.Parameters.AddWithValue("@iThang2", DenThang);
                cmd.Parameters.AddWithValue("@iThangQuy", DenThang);
            }
            else {
                cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
                cmd.Parameters.AddWithValue("@iThang2", Thang_Quy);
                cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
            }
         
            

            String TyLeBH = "";
            // Chọn loại báo cáo 
            switch (LoaiBaoCao)
            {
                case "1": TyLeBH = "rBHXH_CN";
                    break;
                case "2": TyLeBH = "rBHYT_CN";
                    break;
                case "3": TyLeBH = "rBHTN_CN";
                    break;
                case "4": TyLeBH = "rBHXH_DV";
                    break;
                case "5": TyLeBH = "rBHYT_DV";
                    break;
                case "6": TyLeBH = "rBHTN_DV";
                    break;
                case "7": TyLeBH = "(rBHXH_CN + rBHXH_DV)";
                    break;
                case "8": TyLeBH = "(rBHYT_CN + rBHYT_DV)";
                    break;
                case "9": TyLeBH = "(rBHTN_CN + rBHTN_DV)";
                    break;
            }

            //lấy danh sách đơn vị trong tháng được chọn
            DataTable dtDonVi = LayDSDonVi(MaND,LoaiThangQuy, Thang_Quy,iID_MaTrangThaiDuyet);
            String DanhSachDonVi = "";
            if (dtDonVi.Rows.Count > 0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    int Index = (ToSo - 1) * 4 + i;
                    if (Index <= dtDonVi.Rows.Count)
                    {
                        String strDonVi = ",DonVi" + i.ToString() + " = CASE WHEN tbl_DM.iThang = @iThang THEN " + QueryTrongKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "@DonVi" + i.ToString(), "tbl_DM.sKyHieu") + "*" + TyLeBH + " ELSE 0 END";
                        if (LoaiBaoCao == "4" || LoaiBaoCao == "7")
                        {
                            strDonVi = ",DonVi" + i.ToString() + " = CASE WHEN tbl_DM.iThang = @iThang THEN CASE WHEN tbl_DM.sKyHieu=115 THEN " + QueryChienSy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "@DonVi" + i.ToString()) + "*rBHXH_CS*rLuongToiThieu ELSE "
                              + QueryTrongKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "@DonVi" + i.ToString(), "tbl_DM.sKyHieu") + "*" + TyLeBH + " END ELSE 0 END";
                        }
                        DanhSachDonVi += strDonVi;
                        cmd.Parameters.AddWithValue("@DonVi" + i.ToString(), dtDonVi.Rows[Index - 1]["iID_MaDonVi"]);
                    }
                    else
                    {
                        DanhSachDonVi += ",DonVi" + i.ToString() + " = 0";
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 4; i++)
                {
                    DanhSachDonVi += ",DonVi" + i.ToString() + " = 0";
                }
            }
            String TrongKy = ",TrongKy =CASE WHEN tbl_DM.iThang BETWEEN @iThang AND @iThang2 THEN " + QueryTrongKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "", "tbl_DM.sKyHieu") + "*" + TyLeBH + " ELSE 0 END";
            String DenKy = ",DenKy =CASE WHEN tbl_DM.iThang <= @iThang THEN " + QueryDenKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "", "tbl_DM.sKyHieu") + "*" + TyLeBH + " ELSE 0 END";
            if (LoaiBaoCao == "4" || LoaiBaoCao == "7")
            {
                TrongKy = ",TrongKy =CASE WHEN tbl_DM.iThang BETWEEN @iThang AND @iThang2 THEN CASE WHEN tbl_DM.sKyHieu=115 THEN " + QueryChienSy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "") + "*rBHXH_CS*rLuongToiThieu ELSE "
                             + QueryTrongKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "", "tbl_DM.sKyHieu") + "*" + TyLeBH + " END ELSE 0 END";
                DenKy = ",DenKy =CASE WHEN tbl_DM.iThang <= @iThang THEN CASE WHEN tbl_DM.sKyHieu=115 THEN " + QueryChienSyDenKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "") + "*rBHXH_CS*rLuongToiThieu ELSE "
                              + QueryDenKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "", "tbl_DM.sKyHieu") + "*" + TyLeBH + " END ELSE 0 END";
            }
            String SQL = String.Format(@"SELECT sKyHieu, sMoTa, SUM(TrongKy) TrongKy, SUM(DenKy) DenKy, SUM(DonVi1) DonVi1, SUM(DonVi2) DonVi2, SUM(DonVi3) DonVi3, SUM(DonVi4) DonVi4
                                        FROM (
                                                SELECT tbl_DM.sKyHieu,tbl_DM.sMoTa {0} {1} {2}
                                                FROM BH_DanhMucThuBaoHiem AS tbl_DM
                                                WHERE tbl_DM.iTrangThai=1 AND LEN(sKyHieu) = 3 AND iNamLamViec={3}  
                                             ) AS tbl
                                        GROUP BY sKyHieu, sMoTa
                                        ORDER BY sKyHieu", TrongKy, DenKy, DanhSachDonVi, ReportModels.LayNamLamViec(MaND));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public Decimal rptBH_LuyKeDonVi(String LoaiThangQuy, String Thang_Quy, String LoaiBaoCao, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = "";

            String DKThang = " AND iThang <= @iThang";
            if (LoaiThangQuy == "1")
            {
                int DenThang = Convert.ToInt16(Thang_Quy) * 3;
                //int TuThang = DenThang - 2;
                DKThang = String.Format(@" AND iThang <= {0} ", DenThang);
            }
            cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
            cmd.Parameters.AddWithValue("@iThang", Thang_Quy);

            String TyLeBH = "";
            // Chọn loại báo cáo 
            switch (LoaiBaoCao)
            {
                case "1": TyLeBH = "rBHXH_CN";
                    break;
                case "2": TyLeBH = "rBHYT_CN";
                    break;
                case "3": TyLeBH = "rBHTN_CN";
                    break;
                case "4": TyLeBH = "rBHXH_DV";
                    break;
                case "5": TyLeBH = "rBHYT_DV";
                    break;
                case "6": TyLeBH = "rBHTN_DV";
                    break;
                case "7": TyLeBH = "(rBHXH_CN + rBHXH_DV)";
                    break;
                case "8": TyLeBH = "(rBHYT_CN + rBHYT_DV)";
                    break;
                case "9": TyLeBH = "(rBHTN_CN + rBHTN_DV)";
                    break;
            }

            String strDonVi = "LuyKeDonVi = " + QueryDenKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "@iID_MaDonVi", "tbl_DM.sKyHieu") + "*" + TyLeBH;
            if (LoaiBaoCao == "4" || LoaiBaoCao == "7")
            {
                strDonVi = "LuyKeDonVi = CASE WHEN tbl_DM.sKyHieu=115 THEN " + QueryChienSyDenKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "@iID_MaDonVi") + "*rBHXH_CS*rLuongToiThieu ELSE "
                  + QueryDenKy(LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, "@iID_MaDonVi", "tbl_DM.sKyHieu") + "*" + TyLeBH + " END";
            }
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            String SQL = String.Format(@"SELECT SUM(LuyKeDonVi) LuyKeDonVi
                                        FROM (
                                                SELECT {0} 
                                                FROM BH_DanhMucThuBaoHiem AS tbl_DM
                                                WHERE tbl_DM.iTrangThai=1 AND LEN(sKyHieu) = 3 AND iNamLamViec={1} {2}
                                             ) AS tbl", strDonVi, ReportModels.LayNamLamViec(MaND),  DKThang);
            cmd.CommandText = SQL;
            Decimal result = Convert.ToDecimal(Connection.GetValue(cmd,0));
            return result;
        }
        /// <summary>
        /// Hàm xuất ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String LoaiThangQuy, String Thang_Quy, String LoaiBaoCao, String KhoGiay, int ToSo, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(LoaiThangQuy, Thang_Quy, LoaiBaoCao, KhoGiay, ToSo, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopbaoHiem.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String LoaiThangQuy, String Thang_Quy, String LoaiBaoCao, String KhoGiay, int ToSo, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            String TenPhieu = "";
            switch (LoaiBaoCao)
            {
                case "1": TenPhieu = "Bảo hiểm xã hội (cá nhân đóng)";
                    break;
                case "2": TenPhieu = "Bảo hiểm y tế (cá nhân đóng)";
                    break;
                case "3": TenPhieu = "Bảo hiểm thất nghiệp (cá nhân đóng)";
                    break;
                case "4": TenPhieu = "Bảo hiểm xã hội (đơn vị đóng)";
                    break;
                case "5": TenPhieu = "Bảo hiểm y tế (đơn vị đóng)";
                    break;
                case "6": TenPhieu = "Bảo hiểm thất nghiệp (đơn vị đóng)";
                    break;
                case "7": TenPhieu = "Bảo hiểm xã hội (tổng hợp)";
                    break;
                case "8": TenPhieu = "Bảo hiểm y tế (tổng hợp)";
                    break;
                case "9": TenPhieu = "Bảo hiểm thất nghiệp (tổng hợp)";
                    break;
            }
            String path = "";
            if (KhoGiay=="1")
            {
                path = "~/Report_ExcelFrom/BaoHiem/rptBH_TongHop67_A4.xls";
            }
            else
            {
                path = "~/Report_ExcelFrom/BaoHiem/rptBH_TongHop67_A3.xls";
            }
            String Nam = ReportModels.LayNamLamViec(MaND);
            String strThang = "Tháng " + Thang_Quy;
            if (LoaiThangQuy == "1") strThang = "Quý " + Thang_Quy;
            strThang += " năm " + Nam;

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_TongHop67");
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(path));

            //Ten cac cot don vi
            DataTable dtDonVi = LayDSDonVi(MaND, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
            if (dtDonVi.Rows.Count > 0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    int Index = (ToSo - 1) * 4 + i;
                    if (Index > dtDonVi.Rows.Count)
                    {
                        fr.SetValue("DonVi" + i, "");
                        fr.SetValue("LuyKeDonVi" + i, 0);
                    }
                    else
                    {
                        fr.SetValue("DonVi" + i, dtDonVi.Rows[Index - 1]["sTen"]);
                        fr.SetValue("LuyKeDonVi" + i, rptBH_LuyKeDonVi(LoaiThangQuy, Thang_Quy, LoaiBaoCao, iID_MaTrangThaiDuyet, Convert.ToString(dtDonVi.Rows[Index - 1]["iID_MaDonVi"])));
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 4; i++)
                {
                    fr.SetValue("DonVi" + i, "");
                    fr.SetValue("LuyKeDonVi" + i, 0);
                }
            }
            DataTable dt = rptBH_ThongTri67(LoaiThangQuy, Thang_Quy, LoaiBaoCao, KhoGiay, ToSo, iID_MaTrangThaiDuyet);
            fr.AddTable("ChiTiet", dt);
            fr.SetValue("TenPhieu", TenPhieu.ToUpper());
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("ThangQuy", strThang);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("DonViTinh", "Đồng");
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Phong", ReportModels.CauHinhTenDonViSuDung(3));
            fr.Run(Result);
            return Result;
            
        }
        public static DataTable LayDSDonVi(String MaND, String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            String DKThangQuy = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy = "iThang_Quy <= 3";
                        break;
                    case "2":
                        DKThangQuy = "iThang_Quy <= 6";
                        break;
                    case "3":
                        DKThangQuy = "iThang_Quy <= 9";
                        break;
                    case "4":
                        DKThangQuy = "iThang_Quy <= 12";
                        break;
                    default:
                        DKThangQuy="iThang_Quy<=-1";
                        break;
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy <= @iThang_Quy";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@"SELECT DISTINCT BH.iID_MaDonVi,DV.sTen
                                            FROM BH_PhaiThuChungTuChiTiet as BH
                                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV 
                                            ON BH.iID_MaDonVi=DV.iID_MaDonVi AND LTRIM(RTRIM(DV.iID_MaDonVi)) <>'' AND LTRIM(RTRIM(DV.sTen)) <>''
                                            WHERE BH.iTrangThai=1 AND BH.rTongSo>0 {0} {1} AND {2}", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet, DKThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (LoaiThangQuy == "0")
            {
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            }

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public JsonResult DS_To(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet, String ToSo)
        {
            return Json(obj_To(ParentID, MaND, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet, ToSo), JsonRequestBehavior.AllowGet);
        }
        public String obj_To(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet, String ToSo)
        {

            DataTable dtToSo = dtTo(MaND, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            String s = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100px\"");
            return s;
        }
        public static DataTable dtTo(String MaND, String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonVi = LayDSDonVi(MaND, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
            DataTable dt = new DataTable();
            if (dtDonVi.Rows.Count > 0)
            {
                int SoTo = (dtDonVi.Rows.Count - 1) / 4 + 1;
                dtDonVi.Dispose();
                dt.Columns.Add("TenTo", typeof(String));
                dt.Columns.Add("MaTo", typeof(String));
                for (int i = 1; i <= SoTo; i++)
                {
                    DataRow row = dt.Rows.Add();
                    row["TenTo"] = "Tờ " + i;
                    row["MaTo"] = i;
                }
            }
            return dt;
        }
    }
}
