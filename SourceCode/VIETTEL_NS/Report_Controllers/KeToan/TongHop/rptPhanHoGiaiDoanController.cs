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

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptPhanHoGiaiDoanController : Controller
    {
        //
        // GET: /rptPhanHoGiaiDoan/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_Doc= "/Report_ExcelFrom/KeToan/TongHop/rptPhanHoGiaiDoan_Doc.xls";
        private const String sFilePath_Ngang = "/Report_ExcelFrom/KeToan/TongHop/rptPhanHoGiaiDoan_Ngang.xls";
        public ActionResult Index()
        {
            
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptPhanHoGiaiDoan.aspx";
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
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID+"_iID_MaDonVi"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            if (Convert.ToInt16(iThang1) > Convert.ToInt16(iThang2))
                ViewData["PageLoad"] = "0";
            else
                 ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThang1"] = iThang1;
            ViewData["iThang2"] = iThang2;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptPhanHoGiaiDoan.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenTK = "";
            DataTable dtTK = dtTenTaiKhoan(iID_MaTaiKhoan);
            if (dtTK.Rows.Count > 0)
            {
                TenTK = dtTK.Rows[0][0].ToString();
            }
            else
            {
                TenTK = "";
            } 
            dtTK.Dispose();
            String TaiKhoan = "";
            String TenDonVi = "";
            DataTable dtDonVi = DanhSachDonVi(MaND, iID_MaTaiKhoan, iThang1,iThang2,iID_MaTrangThaiDuyet);
            if (iID_MaDonVi == "-1")
            {
                TaiKhoan = "TỔNG HỢP TÀI KHOẢN - TK" + iID_MaTaiKhoan;
                TenDonVi = "";
            }
            else
            {
                TaiKhoan = "CHI TIẾT TÀI KHOẢN THEO ĐƠN VỊ - TK" + iID_MaTaiKhoan;
                for (int i = 0; i < dtDonVi.Rows.Count;i++)
                {
                    if (iID_MaDonVi == dtDonVi.Rows[i]["iID_MaDonVi"].ToString())
                    {
                        TenDonVi = dtDonVi.Rows[i]["sTenDonVi"].ToString();
                        break;
                    }
                }
                
            }
            dtDonVi.Dispose();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanHoGiaiDoan");
            LoadData(fr, MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaDonVi, iID_MaTrangThaiDuyet);
            fr.SetValue("TenTaiKhoan", iID_MaTaiKhoan + "-" + TenTK);
            fr.SetValue("MaDV", iID_MaDonVi);
            fr.SetValue("Ma", iID_MaTaiKhoan);
            fr.SetValue("TaiKhoan", TaiKhoan);
            fr.SetValue("Thang1", iThang1);
            fr.SetValue("Thang2", iThang2);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("nam", iNamLamViec);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("TenDonVi", TenDonVi.ToUpper());
            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "rDoc")
                DuongDan = sFilePath_Doc;
            else DuongDan = sFilePath_Ngang;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaDonVi, iID_MaTrangThaiDuyet,KhoGiay);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCaoPhanHoGiaiDoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String KhoGiay)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "rDoc")
                DuongDan = sFilePath_Doc;
            else DuongDan = sFilePath_Ngang;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaDonVi, iID_MaTrangThaiDuyet,KhoGiay);
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
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dt_PhanHoGiaiDoan(MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaDonVi,iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable LuyKe = dt_PhanHoGiaiDoan_LuyKe(MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaDonVi, iID_MaTrangThaiDuyet);
            LuyKe.TableName = "LuyKe";
            fr.AddTable("LuyKe", LuyKe);

            DataTable SoDuCuoiKy = dt_PhanHoGiaiDoan_LuyKe(MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaDonVi, iID_MaTrangThaiDuyet, false);
            SoDuCuoiKy.TableName = "SoDuCuoiKy";
            fr.AddTable("SoDuCuoiKy", SoDuCuoiKy);
            DataTable dtThang;
            dtThang = HamChung.SelectDistinct("Thang", data, "iThangCT", "iThangCT", "", "", "");
            fr.AddTable("Thang", dtThang);
            dtThang.Dispose();
        }
        public static DataTable TenTaiKhoan(String NamChungTu)
        {
            DataTable dt;
            String KyHieu = "37";
            String[] arrThamSo;
            String ThamSo = "";
            String DKSelect = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sKyHieu=@sKyHieu";
            SqlCommand cmdThamSo = new SqlCommand(DKSelect);
            cmdThamSo.Parameters.AddWithValue("@sKyHieu", KyHieu);
            cmdThamSo.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            DataTable dtThamSo = Connection.GetDataTable(cmdThamSo);
            arrThamSo = Convert.ToString(dtThamSo.Rows[0]["sThamSo"]).Split(',');
            for (int i = 0; i < arrThamSo.Length; i++)
            {
                ThamSo += arrThamSo[i];
                if (i < arrThamSo.Length - 1)
                    ThamSo += " , ";
            }
            String SQL = String.Format(@"SELECT iID_MaTaiKhoan,iID_MaTaiKhoan+'-'+sTen as TenTK FROM KT_TaiKhoan WHERE iID_MaTaiKhoan IN ({0}) AND iNam=@Nam", ThamSo);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@Nam", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        //dt lấy tên tài khoản
        public static DataTable dtTenTaiKhoan(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "rDoc";
            R1[1] = "In khổ dọc";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "rNgang";
            R2[1] = "In khổ ngang";
            dt.Dispose();
            return dt;
        }
        public static DataTable dt_PhanHoGiaiDoan(String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();

            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            //điều kiện đơn vị
            String DKDonVi_No = "";
            String DKDonVi_Co = "";
            DataTable dtDonVi = DanhSachDonVi(MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaTrangThaiDuyet);
            if (iID_MaDonVi == "-1")
            {
               
                for (int i = 0; i < dtDonVi.Rows.Count - 1; i++)
                {
                    DKDonVi_No += "iID_MaDonVi_No=@iID_MaDonVi" + i;
                    DKDonVi_Co += "iID_MaDonVi_Co=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 2)
                    {
                        DKDonVi_No += " OR ";
                        DKDonVi_Co += " OR ";
                    }
                }
               
            }
            else
            {
                DKDonVi_No = "iID_MaDonVi_No=@iID_MaDonVi";
                DKDonVi_Co = "iID_MaDonVi_Co=@iID_MaDonVi";
            }
            #region data chi tiết
            String SQL = String.Format(@"SELECT iThangCT,
	                                               iNgayCT,
	                                               sSoChungTuGhiSo,
	                                               sSoChungTuChiTiet, 
	                                               sNoiDung,
	                                               SUM(rSoTien_No) as rSoTien_No,
	                                               SUM(rSoTien_Co) as rSoTien_Co
                                            FROM(
                                            SELECT iThangCT,
	                                               iNgayCT,
	                                               sSoChungTuGhiSo,
	                                               sSoChungTuChiTiet, 
	                                               sNoiDung,
	                                               rSoTien_No=CASE WHEN (iID_MaTaiKhoan_No LIKE  '{1}%') THEN SUM(rSoTien) ELSE 0 END,
	                                               rSoTien_Co=0
                                            FROM KT_ChungTuChiTiet
                                            WHERE iTrangThai=1
                                                  AND iNamLamViec=@iNamLamViec
                                                  AND (iThangCT BETWEEN @iThang1 AND @iThang2)
                                                  AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                                  AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                  {0}
                                                  AND ({2})
                                            GROUP BY iThangCT,
	                                               iNgayCT,
	                                               sSoChungTuGhiSo,
	                                               sSoChungTuChiTiet, 
	                                               sNoiDung,
	                                               iID_MaTaiKhoan_No	   
                                            UNION
                                            SELECT iThangCT,
	                                               iNgayCT,
	                                               sSoChungTuGhiSo,
	                                               sSoChungTuChiTiet, 
	                                               sNoiDung,
	                                               rSoTien_No=0,
	                                               rSoTien_Co=CASE WHEN (iID_MaTaiKhoan_Co LIKE '{1}%') THEN SUM(rSoTien) ELSE 0 END
                                            FROM KT_ChungTuChiTiet
                                            WHERE  iTrangThai=1
                                                  AND iNamLamViec=@iNamLamViec
                                                  AND (iThangCT BETWEEN @iThang1 AND @iThang2)
                                                  AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                                  AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                  {0}
                                                  AND ({3})
                                            GROUP BY iThangCT,
	                                               iNgayCT,
	                                               sSoChungTuGhiSo,
	                                               sSoChungTuChiTiet, 
	                                               sNoiDung,
	                                               iID_MaTaiKhoan_Co
	                                               ) as KT
	                                            GROUP BY
	                                               iThangCT,
	                                               iNgayCT,
	                                               sSoChungTuGhiSo,
	                                               sSoChungTuChiTiet, 
	                                               sNoiDung
	                                             HAVING SUM(rSoTien_No)>0 OR SUM(rSoTien_Co)>0
	                                             ORDER BY iThangCT,iNgayCT,sSoChungTuGhiSo,sSoChungTuChiTiet", DKTrangThaiDuyet,iID_MaTaiKhoan,DKDonVi_No,DKDonVi_Co);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            if (iID_MaTrangThaiDuyet != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 0; i < dtDonVi.Rows.Count - 1; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            dtDonVi.Dispose();
            #endregion
            return dt;
        }
        public static DataTable dt_PhanHoGiaiDoan_LuyKe(String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaDonVi, String iID_MaTrangThaiDuyet, Boolean LuyKe=true)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();
            String DKLuyKe = "";
            if (LuyKe == true)
                DKLuyKe = " AND iThangCT<>0 ";
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            //điều kiện đơn vị
            String DKDonVi_No = "";
             String DKDonVi_Co = "";
             DataTable dtDonVi = DanhSachDonVi(MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaTrangThaiDuyet);
            if (iID_MaDonVi == "-1")
            {
                for (int i = 0; i < dtDonVi.Rows.Count-1;i++)
                {
                    DKDonVi_No += "iID_MaDonVi_No=@iID_MaDonVi" + i;
                    DKDonVi_Co += "iID_MaDonVi_Co=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 2)
                    {
                        DKDonVi_No += " OR ";
                        DKDonVi_Co += " OR ";
                    }
                }
            }
            else
            {
                DKDonVi_No="iID_MaDonVi_No=@iID_MaDonVi";
                DKDonVi_Co = "iID_MaDonVi_Co=@iID_MaDonVi";
            }
            dtDonVi.Dispose();
            #region data chi tiết
            String SQL = String.Format(@"SELECT iThangCT,
	                                           SUM(rSoTien_No) as rSoTien_No,
	                                           SUM(rSoTien_Co) as rSoTien_Co
                                        FROM(
                                        SELECT iThangCT,
	                                           rSoTien_No=CASE WHEN (iID_MaTaiKhoan_No LIKE '{2}%') THEN SUM(rSoTien) ELSE 0 END,
	                                           rSoTien_Co=0
                                        FROM KT_ChungTuChiTiet
                                        WHERE iTrangThai=1
                                                  AND iNamLamViec=@iNamLamViec
                                                  AND iThangCT <= @iThang2  {0}
                                                  AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                                  AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                  {1}
                                                  AND ({3})
                                        GROUP BY iThangCT,
	                                           iID_MaTaiKhoan_No
                                        UNION
                                        SELECT iThangCT,
	                                           rSoTien_No=0,
	                                           rSoTien_Co=CASE WHEN (iID_MaTaiKhoan_Co  LIKE '{2}%') THEN SUM(rSoTien) ELSE 0 END
                                        FROM KT_ChungTuChiTiet
                                        WHERE iTrangThai=1
                                                  AND iNamLamViec=@iNamLamViec
                                                  AND iThangCT <= @iThang2  {0}
                                                  AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                                  AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                  {1}
                                                  AND ({4})
                                        GROUP BY iThangCT,
	                                           iID_MaTaiKhoan_Co
	                                           ) as KT
	                                        GROUP BY
	                                           iThangCT
	                                         HAVING SUM(rSoTien_No)>0 OR SUM(rSoTien_Co)>0
	                                         ORDER BY iThangCT", DKLuyKe,DKTrangThaiDuyet,iID_MaTaiKhoan,DKDonVi_No,DKDonVi_Co);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            if (iID_MaTrangThaiDuyet != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 0; i < dtDonVi.Rows.Count - 1; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("rNo",typeof(decimal) );
                dt.Columns.Add("rCo", typeof(decimal));              
                for (int i = 0; i < dt.Rows.Count;i++)
                {
                    decimal x = 0;
                    decimal y = 0;
                    for (int j = 0; j <= i; j++)
                    {
                        decimal temp = 0;
                        decimal temp1 = 0;
                        if (dt.Rows[j]["rSoTien_No"] != DBNull.Value)
                        {
                            temp = Convert.ToDecimal(dt.Rows[j]["rSoTien_No"]);
                        }
                        if (dt.Rows[j]["rSoTien_Co"] != DBNull.Value)
                        {
                            temp1 = Convert.ToDecimal(dt.Rows[j]["rSoTien_Co"]);
                        }
                        x =  x + temp;
                        y = y + temp1;   
                    }
                    dt.Rows[i]["rNo"] = x;
                    dt.Rows[i]["rCo"] = y;
                }
            }
            cmd.Dispose();
            #endregion
            return dt;
        }

        public static DataTable DanhSachDonVi(String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaTrangThaiDuyet)
        {
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi_No as iID_MaDonVi,sTenDonVi_No as sTenDonVi
                                        FROM KT_ChungTuChiTiet
                                        WHERE 
                                        iTrangThai=1
                                        AND rSoTien>0
                                        AND iNamLamViec=@iNamLamViec
                                        AND (iThangCT BETWEEN @iThang1 AND @iThang2)
                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                        {0}
                                        AND iID_MaTaiKhoan_No LIKE '{1}%'
                                        AND iID_MaDonVi_No IS NOT NULL
                                        UNION 
                                        SELECT DISTINCT iID_MaDonVi_Co,sTenDonVi_Co 
                                        FROM KT_ChungTuChiTiet
                                        WHERE 
                                        iTrangThai=1
                                        AND rSoTien>0
                                        AND iNamLamViec=@iNamLamViec
                                        AND (iThangCT BETWEEN @iThang1 AND @iThang2)
                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                       {0}
                                        AND iID_MaTaiKhoan_Co LIKE '{1}%'
                                        AND iID_MaDonVi_Co IS NOT NULL
                                        ", DKTrangThaiDuyet,iID_MaTaiKhoan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            if (iID_MaTrangThaiDuyet != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr[0] = "-1";
                dr[1] = "--Tổng hơp tất cả --";
                dt.Rows.Add(dr);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr[0] = Guid.Empty.ToString();
                dr[1] = "--Không có đơn vị--";
                dt.Rows.Add(dr);
            }
            dt.Dispose();
            return dt;

        }
        [HttpGet]
        public JsonResult Get_dsDonVi(String ParentID,String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaTrangThaiDuyet,String iID_MaDonVi)
        {

            return Json(obj_DSDonVi(ParentID,MaND, iID_MaTaiKhoan, iThang1, iThang2, iID_MaTrangThaiDuyet, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String obj_DSDonVi(String ParentID,String MaND, String iID_MaTaiKhoan, String iThang1, String iThang2, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            String dsDonVi = "";
            DataTable dtDonVi = DanhSachDonVi(MaND, iID_MaTaiKhoan, iThang1,iThang2,iID_MaTrangThaiDuyet);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTenDonVi");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 180px;\"");
            dtDonVi.Dispose();
            return dsDonVi;
        }
    }
}
