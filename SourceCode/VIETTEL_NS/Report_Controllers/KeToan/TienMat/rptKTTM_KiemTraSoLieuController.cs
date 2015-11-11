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

namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKiemTraSoLieuUNCController : Controller
       
    {
        //
        // GET: /rptChiTieu_TongHopChiTieu_7/
        public string sViewPath = "~/Report_Views/";
        public string Count = "";
        public decimal Tien = 0;
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKiemTraSoLieuUNC.xls";
        private const String sFilePath_CTPS = "/Report_ExcelFrom/KeToan/TienGui/rptKiemTraSoLieuUNCSai.xls";
        public ActionResult Index()
        {
           if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
               ViewData["PageLoad"]="0";
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKiemTraSoLieuUNC.aspx";
           // ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
            }
           else
           {
               return RedirectToAction("Index", "PermitionMessage");
           }
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec)
        {
            String TuNgay = Request.Form[ParentID + "_TuNgay"];
            String DenNgay = Request.Form[ParentID + "_DenNgay"];
            String TuThang = Request.Form[ParentID + "_TuThang"];
            String DenThang = Request.Form[ParentID + "_DenThang"];
            String Loai = Request.Form[ParentID + "_Loai"];
            String iTrangThai = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String sSoChungTuGhiSo = Request.Form[ParentID + "_sSoChungTuGhiSo"];
            ViewData["PageLoad"] = "1";
            ViewData["TuNgay"] = TuNgay;
            ViewData["DenNgay"] = DenNgay;
            ViewData["TuThang"] = TuThang;
            ViewData["DenThang"] = DenThang;
            ViewData["Loai"] = Loai;
            ViewData["sSoChungTuGhiSo"] = sSoChungTuGhiSo;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKiemTraSoLieuUNC.aspx";
            // ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
            // return RedirectToAction("Index", new { sSoChungTuGhiSo = sSoChungTuGhiSo , iNamLamViec = iNamLamViec, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, Loai = Loai, iTrangThai = iTrangThai});
        }
        private string GetGhiChu(String TaiKhoanCo = "", String TaiKhoanNo = "", String DonViNo="", String DonViCo="")
        {
            string str = "";
            if (!CheckTaiKhoan(TaiKhoanNo)) str += "KN";
            if (!CheckTaiKhoan(TaiKhoanCo)) str += " KC";
            if (CheckTaiKhoanCha(TaiKhoanNo)) str += " N";
            if (CheckTaiKhoanCha(TaiKhoanCo)) str += " C";
            if (!CheckTuDienTaiKhoanNo(TaiKhoanNo) || !CheckTuDienTaiKhoanCo(TaiKhoanCo)) str += " !";
            if (CheckDonVi(DonViNo) || CheckDonVi(DonViCo)) str += " D";
            return str;
        }
        private Boolean CheckTuDienTaiKhoanNo(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoanNo FROM KT_TuDien WHERE iID_MaTaiKhoanNo <> @MaTaiKhoan AND iID_MaTaiKhoanCo=@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckTuDienTaiKhoanCo(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoanNo FROM KT_TuDien WHERE iID_MaTaiKhoanNo= @MaTaiKhoan AND iID_MaTaiKhoanCo<>@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckDonVi(String sKyHieu = "")
        {
            string sql = "SELECT sKyHieu FROM NS_PhongBan WHERE sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckTaiKhoan(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoan FROM KT_TaiKhoan WHERE iID_MaTaiKhoan=@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        private Boolean CheckTaiKhoanCha(String MaTaiKhoan = "")
        {
            string sql = "SELECT iID_MaTaiKhoan FROM KT_TaiKhoan WHERE bLaHangCha = 1 AND iID_MaTaiKhoan=@MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = sql;
            var dt = Connection.GetDataTable(cmd);
            int Count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (Count > 0) return true;
            else return false;
        }
        public static DataTable dtLoaiBangKe()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaBangKe");
            dt.Columns.Add("TenBangKe");
            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "1. Tất cả";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "0";
            dr1[1] = "2. Riêng số ghi sổ";
            dt.Rows.Add(dr1);
            dt.Dispose();
            return dt;
        }
       
        public static DataTable dtSoChungTu(String iNamLamViec, String TuThang = "", String DenThang = "")
        {
           
            SqlCommand cmd = new SqlCommand();
            String SQL = @" select sSoChungTuGhiSo from (
                            SELECT DISTINCT CT.sSoChungTuGhiSo,CT.sSoChungTuChiTiet
                            , CT.iNgay, CT.iThang, CT.sNoiDung, CT.rSoTien, CT.iID_MaTaiKhoan_No, 
                            CT.iID_MaTaiKhoan_Co, KT.sSoChungTu, KT.iNgay AS NgayGhiSo, KT.iThang AS ThangGhiSo, CT.iID_MaDonVi_Co, CT.iID_MaDonVi_No
                            FROM  dbo.KTTG_ChungTuChiTiet AS CT INNER JOIN
                            dbo.KTTG_ChungTu AS KT ON CT.iID_MaChungTu = KT.iID_MaChungTu
                            WHERE (CT.iTrangThai = 1 ) AND ((CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT iID_MaTaiKhoan
                            FROM dbo.KT_TaiKhoan AS TK1))  OR
                            (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT iID_MaTaiKhoan
                            FROM dbo.KT_TaiKhoan AS TK2)) OR(CT.iThang <> KT.iThang) OR(CT.iID_MaDonVi_No NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB1 WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ))OR(CT.iID_MaDonVi_Co NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB2 WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)) OR(CT.iID_MaTaiKhoan_No IS NULL) OR
                            (CT.iID_MaTaiKhoan_No = '') OR(CT.iID_MaTaiKhoan_Co IS NULL) OR(CT.iID_MaTaiKhoan_Co = '') OR
                            (CT.iID_MaTaiKhoan_No IN(SELECT iID_MaTaiKhoan FROM dbo.KT_TaiKhoan AS TK3 WHERE (bLaHangCha = 1))) OR
                            (CT.iID_MaTaiKhoan_Co IN(SELECT iID_MaTaiKhoan FROM  dbo.KT_TaiKhoan AS TK3 WHERE (bLaHangCha = 1))) OR
                            (CT.iID_MaTaiKhoan_No NOT IN (SELECT iID_MaTaiKhoanNo FROM dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co IN
                            (SELECT iID_MaTaiKhoanCo FROM dbo.KT_TuDien AS TD1)) OR (CT.iID_MaTaiKhoan_No IN (SELECT iID_MaTaiKhoanNo
                            FROM dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co NOT IN (SELECT iID_MaTaiKhoanCo
                            FROM dbo.KT_TuDien AS TD1))) 
                                   AND CT.iThang between @TuThang and @DenThang) as bang
                            GROUP BY sSoChungTuGhiSo        
                             order by sSoChungTuGhiSo";
            SQL = String.Format(SQL);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", Convert.ToString(TuThang));
            cmd.Parameters.AddWithValue("@DenThang", Convert.ToString(DenThang));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
        public ExcelFile CreateReport(String path, String sSoChungTuGhiSo, String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDVCapTren = ReportModels.CauHinhTenDonViSuDung(1);
            String tendv = ReportModels.CauHinhTenDonViSuDung(2);
            
              FlexCelReport fr = new FlexCelReport();
              fr = ReportModels.LayThongTinChuKy(fr, "rptKiemTraSoLieuUNC");
              LoadData(fr, sSoChungTuGhiSo, iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
                if (Loai == "0")
                {
                    fr.SetValue("TuNgay", TuNgay + " - " + TuThang);
                    fr.SetValue("DenNgay", DenNgay + " - " + DenThang);
                    fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
                    fr.SetValue("TenDV", tendv.ToUpper());
                    fr.SetValue("Count", Count);
                    fr.SetValue("Ngay1", TuNgay);
                    fr.SetValue("Ngay2", DenNgay);
                    fr.SetValue("Thang1", TuThang);
                    fr.SetValue("Thang2", DenThang);
                    fr.SetValue("Ngay", DateTime.Now.Day);
                    fr.SetValue("Thang", DateTime.Now.Month);
                    fr.SetValue("Nam", DateTime.Now.Year);
                    fr.SetValue("TongTien", CommonFunction.DinhDangSo(Tien));
                }
                else {
                    fr.SetValue("ThoiGian", "Từ ngày " + TuNgay + " - " + TuThang + " đến ngày " + DenNgay + " - " + DenThang);
                    fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
                    fr.SetValue("TenDV", tendv.ToUpper());
                    fr.SetValue("Ngay", DateTime.Now.Day);
                    fr.SetValue("Thang", DateTime.Now.Month);
                    fr.SetValue("Nam", DateTime.Now.Year);
                    fr.SetValue("Ngay1", TuNgay);
                    fr.SetValue("Ngay2", DenNgay);
                    fr.SetValue("Thang1", TuThang);
                    fr.SetValue("Thang2", DenThang);
                }
                fr.Run(Result);
                return Result;
        }

        private DataTable CreatTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SoChungTu", typeof(string));
            dt.Columns.Add("NgayChungTu", typeof(string));
            dt.Columns.Add("NoiDung", typeof(string));
            dt.Columns.Add("SoTien", typeof(string));
            dt.Columns.Add("TaiKhoanNo", typeof(string));
            dt.Columns.Add("TaiKhoanCo", typeof(string));
            dt.Columns.Add("SoGhiSo", typeof(string));
            dt.Columns.Add("NgayGhiSo", typeof(string));          
            dt.Columns.Add("GhiChu", typeof(string));
            dt.Columns.Add("DonViNo", typeof(string));
            dt.Columns.Add("DonViCo", typeof(string));
            return dt;
        }
        private DataTable get_DanhSach_ChungTu( String sSoChungTuGhiSo,String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "1")
        {
            
            String iTuNgay = iNamLamViec + "/" + TuThang + "/" + TuNgay;
            String iDenNgay = iNamLamViec + "/" + DenThang + "/" + DenNgay;
            if (Loai == "0")//ke chung tu sai
            {
                DataTable dt = null;
                SqlCommand cmd = new SqlCommand();
                String SQL = @"SELECT DISTINCT CT.sSoChungTuChiTiet, CT.iNgay, CT.iThang, CT.sNoiDung, CT.rSoTien, CT.iID_MaTaiKhoan_No, 
                        CT.iID_MaTaiKhoan_Co, KT.sSoChungTu, KT.iNgay AS NgayGhiSo, KT.iThang AS ThangGhiSo, CT.iID_MaDonVi_Co, CT.iID_MaDonVi_No
                        FROM  dbo.KTTG_ChungTuChiTiet AS CT INNER JOIN
                        dbo.KTTG_ChungTu AS KT ON CT.iID_MaChungTu = KT.iID_MaChungTu
                        WHERE (CT.iTrangThai = 1)";
                if (iTrangThai != "0") SQL += " AND (KT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet)";
                SQL += @" AND ((CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK1))  OR
                        (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK2)) OR
                        (CT.iThang <> KT.iThang) OR
                        (CT.iID_MaDonVi_No NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB1  WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ))OR
                        (CT.iID_MaDonVi_Co NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB2  WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)) OR
                        (CT.iID_MaTaiKhoan_No IS NULL) OR
                        (CT.iID_MaTaiKhoan_No = '') OR
                        (CT.iID_MaTaiKhoan_Co IS NULL) OR
                        (CT.iID_MaTaiKhoan_Co = '') OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1))) OR
                        (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1))) OR
                        (CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1)) OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1)))";
                if (String.IsNullOrEmpty(TuThang) == false && TuThang != "" && String.IsNullOrEmpty(DenThang) == false && DenThang != ""
                    && String.IsNullOrEmpty(TuNgay) == false && TuNgay != "" && String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
                {

                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, CT.iThang) + '/' + CONVERT(varchar, CT.iNgay), 111) >= @TuNgay)";
                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, CT.iThang) + '/' + CONVERT(varchar, CT.iNgay), 111) <= @DenNgay) AND (CT.iThang<>0)";
                    cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay));
                    cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay));

                }
                SQL += " ORDER BY CT.iThang, CT.iNgay";
                if (iTrangThai != "0")
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                DataTable dtMain = CreatTable();
                string GhiChu = "", TaiKhoanNo = "", TaiKhoanCo = "", DonViCo = "", DonViNo = "";

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DataRow drMain = dtMain.NewRow();
                        TaiKhoanNo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_No"]);
                        TaiKhoanCo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_Co"]);
                        DonViCo = HamChung.ConvertToString(dr["iID_MaDonVi_Co"]);
                        DonViNo = HamChung.ConvertToString(dr["iID_MaDonVi_No"]);


                        drMain["SoChungTu"] = HamChung.ConvertToString(dr["sSoChungTuChiTiet"]);
                        drMain["NgayChungTu"] = HamChung.ConvertToString(dr["iNgay"]) + " - " + HamChung.ConvertToString(dr["iThang"]);
                        drMain["NoiDung"] = HamChung.ConvertToString(dr["sNoiDung"]);
                        drMain["SoTien"] = CommonFunction.DinhDangSo(HamChung.ConvertToString(dr["rSoTien"]));
                        drMain["TaiKhoanNo"] = TaiKhoanNo;
                        drMain["TaiKhoanCo"] = TaiKhoanCo;
                        drMain["SoGhiSo"] = HamChung.ConvertToString(dr["sSoChungTu"]);
                        drMain["NgayGhiSo"] = HamChung.ConvertToString(dr["NgayGhiSo"]) + " - " + HamChung.ConvertToString(dr["ThangGhiSo"]);
                        //lấy ghi chú
                        GhiChu = GetGhiChu(TaiKhoanCo, TaiKhoanNo, DonViNo, DonViCo);
                        if (HamChung.ConvertToString(dr["iThang"]) != HamChung.ConvertToString(dr["ThangGhiSo"])) GhiChu += " T";
                        if (TaiKhoanNo != "" && TaiKhoanCo != "" && TaiKhoanNo == TaiKhoanCo) GhiChu += " =";
                        if (TaiKhoanNo == "" && TaiKhoanCo == "") GhiChu += " ?";
                        drMain["GhiChu"] = GhiChu;
                        drMain["DonViNo"] = DonViNo;
                        drMain["DonViCo"] = DonViCo;
                        Tien += Convert.ToDecimal(HamChung.ConvertToString(dr["rSoTien"]));
                        dtMain.Rows.Add(drMain);
                    }
                }
                cmd.Dispose();
                if (dt != null) dt.Dispose();
                //Đếm số dòng
                Count = dtMain.Rows.Count.ToString();
                return dtMain;
            }
            else
            {
                DataTable dt = null;
                SqlCommand cmd = new SqlCommand();
                String SQL = @"SELECT DISTINCT CT.sSoChungTuChiTiet, CT.iNgay, CT.iThang, CT.sNoiDung, CT.rSoTien, CT.iID_MaTaiKhoan_No, 
                        CT.iID_MaTaiKhoan_Co, KT.sSoChungTu, KT.iNgay AS NgayGhiSo, KT.iThang AS ThangGhiSo, CT.iID_MaDonVi_Co, CT.iID_MaDonVi_No
                        FROM  dbo.KTTG_ChungTuChiTiet AS CT INNER JOIN
                        dbo.KTTG_ChungTu AS KT ON CT.iID_MaChungTu = KT.iID_MaChungTu
                        WHERE (CT.iTrangThai = 1 and sSoChungTuGhiSo=@sSoChungTuGhiSo)";//and sSoChungTuGhiSo=@sSoChungTuGhiSo
                if (iTrangThai != "0") SQL += " AND (KT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet)";
                SQL += @" AND ((CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK1))  OR
                        (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK2)) OR
                        (CT.iThang <> KT.iThang) OR
                        (CT.iID_MaDonVi_No NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB1 WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec))OR
                        (CT.iID_MaDonVi_Co NOT IN
                            (SELECT iID_MaDonVi  FROM NS_DonVi AS PB2 WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec)) OR
                        (CT.iID_MaTaiKhoan_No IS NULL) OR
                        (CT.iID_MaTaiKhoan_No = '') OR
                        (CT.iID_MaTaiKhoan_Co IS NULL) OR
                        (CT.iID_MaTaiKhoan_Co = '') OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1))) OR
                        (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoan
                              FROM           dbo.KT_TaiKhoan AS TK3
                              WHERE       (bLaHangCha = 1))) OR
                        (CT.iID_MaTaiKhoan_No NOT IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1)) OR
                        (CT.iID_MaTaiKhoan_No IN
                            (SELECT      iID_MaTaiKhoanNo
                              FROM           dbo.KT_TuDien AS TD2)) AND (CT.iID_MaTaiKhoan_Co NOT IN
                            (SELECT      iID_MaTaiKhoanCo
                              FROM           dbo.KT_TuDien AS TD1)))";
                cmd.Parameters.AddWithValue("@sSoChungTuGhiSo", Convert.ToString(sSoChungTuGhiSo));
                if (String.IsNullOrEmpty(TuThang) == false && TuThang != "" && String.IsNullOrEmpty(DenThang) == false && DenThang != ""
                    && String.IsNullOrEmpty(TuNgay) == false && TuNgay != "" && String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
                {

                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, CT.iThang) + '/' + CONVERT(varchar, CT.iNgay), 111) >= @TuNgay)";
                    SQL += " AND (CONVERT(Datetime, CONVERT(varchar, CT.iNamLamViec) + '/' + CONVERT(varchar, CT.iThang) + '/' + CONVERT(varchar, CT.iNgay), 111) <= @DenNgay) AND (CT.iThang<>0)";
                    cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay));
                    cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay));

                }
                SQL += " ORDER BY CT.iThang, CT.iNgay";
                if (iTrangThai != "0")
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                DataTable dtMain = CreatTable();
                string GhiChu = "", TaiKhoanNo = "", TaiKhoanCo = "", DonViCo = "", DonViNo = "";

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DataRow drMain = dtMain.NewRow();
                        TaiKhoanNo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_No"]);
                        TaiKhoanCo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_Co"]);
                        DonViCo = HamChung.ConvertToString(dr["iID_MaDonVi_Co"]);
                        DonViNo = HamChung.ConvertToString(dr["iID_MaDonVi_No"]);


                        drMain["SoChungTu"] = HamChung.ConvertToString(dr["sSoChungTuChiTiet"]);
                        drMain["NgayChungTu"] = HamChung.ConvertToString(dr["iNgay"]) + " - " + HamChung.ConvertToString(dr["iThang"]);
                        drMain["NoiDung"] = HamChung.ConvertToString(dr["sNoiDung"]);
                        drMain["SoTien"] = CommonFunction.DinhDangSo(HamChung.ConvertToString(dr["rSoTien"]));
                        drMain["TaiKhoanNo"] = TaiKhoanNo;
                        drMain["TaiKhoanCo"] = TaiKhoanCo;
                        drMain["SoGhiSo"] = HamChung.ConvertToString(dr["sSoChungTu"]);
                        drMain["NgayGhiSo"] = HamChung.ConvertToString(dr["NgayGhiSo"]) + " - " + HamChung.ConvertToString(dr["ThangGhiSo"]);
                        //lấy ghi chú
                        GhiChu = GetGhiChu(TaiKhoanCo, TaiKhoanNo, DonViNo, DonViCo);
                        if (HamChung.ConvertToString(dr["iThang"]) != HamChung.ConvertToString(dr["ThangGhiSo"])) GhiChu += " T";
                        if (TaiKhoanNo != "" && TaiKhoanCo != "" && TaiKhoanNo == TaiKhoanCo) GhiChu += " =";
                        if (TaiKhoanNo == "" && TaiKhoanCo == "") GhiChu += " ?";
                        drMain["GhiChu"] = GhiChu;
                        drMain["DonViNo"] = DonViNo;
                        drMain["DonViCo"] = DonViCo;
                        Tien += Convert.ToDecimal(HamChung.ConvertToString(dr["rSoTien"]));
                        dtMain.Rows.Add(drMain);
                    }
                }
                cmd.Dispose();
                if (dt != null) dt.Dispose();
                //Đếm số dòng
                Count = dtMain.Rows.Count.ToString();
                return dtMain;
            }
        }
        private void LoadData(FlexCelReport fr, String sSoChungTuGhiSo, String iNamLamViec, String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {

            DataTable data = get_DanhSach_ChungTu( sSoChungTuGhiSo,iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);       
            data.Dispose();
        }

        public clsExcelResult ExportToPDF( String sSoChungTuGhiSo,String iNamLamViec = "", String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            HamChung.Language();
            string DuongDan = "";
            if (Loai == "0")
            {
                DuongDan = sFilePath;
            }
            else
            {
                DuongDan = sFilePath_CTPS;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sSoChungTuGhiSo, iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
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
        public clsExcelResult ExportToExcel( String sSoChungTuGhiSo,String iNamLamViec = "", String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {
            HamChung.Language();
            string DuongDan = "";
            if (Loai == "0")
            {
                DuongDan = sFilePath;
            }
            else
            {
                DuongDan = sFilePath_CTPS;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sSoChungTuGhiSo, iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKiemTraSoLieuUNC.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF( String sSoChungTuGhiSo,String iNamLamViec = "", String TuNgay = "", String DenNgay = "", String TuThang = "", String DenThang = "", String Loai = "0", String iTrangThai = "0")
        {

            HamChung.Language();
            string DuongDan = "";
            if (Loai == "0")
            {
                DuongDan = sFilePath;
            }
            else
            {
                DuongDan = sFilePath_CTPS;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sSoChungTuGhiSo, iNamLamViec, TuNgay, DenNgay, TuThang, DenThang, Loai, iTrangThai);
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

        public JsonResult ObjDanhSachDonVi(String ParentID, String TuThang, String iNamLamviec, String DenThang,String sSoChungTuGhiSo)
        {
            return Json(get_sDanhSachDonVi(ParentID, TuThang, iNamLamviec, DenThang,sSoChungTuGhiSo), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String ParentID, String TuThang, String iNamLamviec, String DenThang, String sSoChungTuGhiSo)
        {
            DataTable dtSoCT = rptKiemTraSoLieuUNCController.dtSoChungTu(iNamLamviec, TuThang, DenThang);

            SelectOptionList slSoCT = new SelectOptionList(dtSoCT, "sSoChungTuGhiSo", "sSoChungTuGhiSo");
            return MyHtmlHelper.DropDownList(ParentID, slSoCT, sSoChungTuGhiSo, "sSoChungTuGhiSo", "", "class=\"input1_2\" style=\"width: 60px;\" size='1' tab-index='-1' ");
        }
        


        //MyHtmlHelper.DropDownList(ParentID, slSoCT, sSoChungTuGhiSo, "sSoChungTuGhiSo", "", "class=\"input1_2\" style=\"width: 60px;\" size='1' tab-index='-1'")
        public JsonResult Get_objNgayThang(String ParentID, String TenTruong, String Ngay, String Thang, String iNam)
        {
            return Json(get_sNgayThang(ParentID, TenTruong, Ngay, Thang, iNam), JsonRequestBehavior.AllowGet);
        }
        public string get_sNgayThang(String ParentID, String TenTruong, String Ngay, String Thang, String iNam)
        {
            DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(Thang), Convert.ToInt16(iNam), false);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            int SoNgayTrongThang = DateTime.DaysInMonth(Convert.ToInt16(iNam), Convert.ToInt16(Thang));
            if (String.IsNullOrEmpty(Ngay) == false)
            {
                if (Convert.ToInt16(Ngay) > SoNgayTrongThang)
                    Ngay = "1";
            }
            return MyHtmlHelper.DropDownList(ParentID, slNgay, Ngay, TenTruong, "", "style=\"width:60px;\"");
        }
    }
}
 