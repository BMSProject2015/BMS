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
    public class rptTongHopTaiKhoanController : Controller
    {
        //
        // GET: /rptTongHopTaiKhoan/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptTongHopTaiKhoan.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptTongHopTaiKhoan.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptTongHopTaiKhoan.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet)
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
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            String TenDV = "";
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen").ToString();
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTongHopTaiKhoan");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang,iID_MaTrangThaiDuyet);
            fr.SetValue("TenTaiKhoan", iID_MaTaiKhoan + "-" + TenTK);
            fr.SetValue("Ma", iID_MaTaiKhoan);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("Nams", DateTime.Now.Year);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("ngay", ngay);
            fr.SetValue("TenDV", TenDV);
            fr.Run(Result);
            return Result;

        }
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyet);

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
        public ActionResult ViewPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyet);
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



        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyets)
        {
            DataTable data = TongHopTaiKhoan(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyets);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable data1 = DuCuoiKy(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyets);
            data1.TableName = "ChiTiet1";
            fr.AddTable("ChiTiet1", data1);
            data1.Dispose();
            DataTable data2 = LuyKe(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyets);
            data2.TableName = "LuyKe";
            fr.AddTable("LuyKe", data2);
            data2.Dispose();
        }
        public DataTable TongHopTaiKhoan(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyets)
        {
            String DKDuyet = iID_MaTrangThaiDuyets.Equals("0") ? "" : "and C.iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
            String DK = "";
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            String SQL = " SELECT KT_ChungTu.sSoChungTu";
            SQL += ",C.iThang,C.iNgay,C.sSoChungTuChiTiet";
            SQL += ",C.sNoiDung,C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan_DoiUng";
            SQL += ",C.rSoTien AS rSoPhatSinhNo,0 AS rSoPhatSinhCo";
            SQL += " FROM KT_ChungTuChiTiet as C";
            SQL += "  INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =C.iID_MaChungTu ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE C.iTrangThai = 1 AND C.iNamLamViec=@iNamLamViec AND C.iThangCT=@iThang AND C.iID_MaTaiKhoan_No = @iID_MaTaiKhoan {1} {0}";
            SQL += " UNION";

            SQL += " SELECT  KT_ChungTu.sSoChungTu";
            SQL += ",C.iThang,C.iNgay,C.sSoChungTuChiTiet";
            SQL += " ,C.sNoiDung,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan";
            SQL += ",C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan_DoiUng,0 AS rSoPhatSinhNo";
            SQL += ",C.rSoTien AS rSoPhatSinhCo";
            SQL += " FROM KT_ChungTuChiTiet as C";
            SQL += "  INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =C.iID_MaChungTu ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE C.iTrangThai = 1 AND C.iNamLamViec=@iNamLamViec AND C.iThangCT=@iThang AND C.iID_MaTaiKhoan_Co = @iID_MaTaiKhoan {1} {0}";
            SQL += " ORDER BY iThang,iNgay";
            SQL = String.Format(SQL, DK,DKDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            return dt;

        }
        public DataTable DuCuoiKy(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang,String iID_MaTrangThaiDuyets)
        {
            String DKDuyet = iID_MaTrangThaiDuyets.Equals("0") ? "" : "and KTCT.iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            String SQL = " SELECT SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo";
            SQL += " FROM(";
            SQL += " SELECT NS_DonVi.iID_MaDonVi,KTCT.sSoChungTuChiTiet as SoChungTu";
            SQL += " ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No = @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co = @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END";
            SQL += " FROM KT_ChungTuChiTiet KTCT";
            SQL += " INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON KTCT.iID_MaDonVi_No=NS_DonVi.iID_MaDonVi";
            SQL += " WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@iNamLamViec {1}";
            SQL += "  AND KTCT.iThangCT<=@iThang  {0}";
            SQL += " GROUP BY iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No,sSoChungTuChiTiet,iID_MaDonVi)as BANGTEM ";
            SQL += " HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0";
            SQL = String.Format(SQL, DK,DKDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            //cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang", iThang);
           
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }
        String DK = "";

        public DataTable LuyKe(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyets)
        {
            String DKDuyet = iID_MaTrangThaiDuyets.Equals("0") ? "" : "and KTCT. iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            String SQL = " SELECT SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo";
            SQL += " FROM(";
            SQL += " SELECT NS_DonVi.iID_MaDonVi,KTCT.sSoChungTuChiTiet as SoChungTu";
            SQL += " ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No = @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co = @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END";
            SQL += " FROM KT_ChungTuChiTiet KTCT";
            SQL += " INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON KTCT.iID_MaDonVi_No=NS_DonVi.iID_MaDonVi";
            SQL += " WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@iNamLamViec {1}";
            SQL += "  AND KTCT.iThangCT<=@iThang AND KTCT.iThang<>0  {0}";
            SQL += " GROUP BY iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No,sSoChungTuChiTiet,iID_MaDonVi)as BANGTEM ";
            SQL += " HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0";
            SQL = String.Format(SQL, DK,DKDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            //cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count==0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable TenTaiKhoan(String NamChungTu)
        {
            DataTable dt;
            String KyHieu = "36";
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
    }
}

