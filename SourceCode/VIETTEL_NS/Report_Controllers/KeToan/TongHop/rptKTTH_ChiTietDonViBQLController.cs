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
//namespace VIETTEL.Report_Controllers.KeToan.TongHop
namespace VIETTEL.Report_Controllers.KeToan.TienMat
{


    public class rptKTTH_ChiTietDonViBQLController : Controller
    {
        //
        // GET: /rptChiTietTheoDonVi/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath2 = "/Report_ExcelFrom/KeToan/TongHop/rptKTTH_ChiTietDonViBQL.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/KeToan/TienMat/rptChiTietTheoDonVi.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
             {
                 HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ChiTietDonViBQL.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_LoaiBaoCao"]);
            ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptChiTietTheoDonVi_2.aspx";
            ViewData["iThang1"] = iThang1;
            ViewData["iThang2"] = iThang2;
            ViewData["iNgay1"] = iNgay1;
            ViewData["iNgay2"] = iNgay2;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            ViewData["PageLoad"] = 1;
            return View(sViewPath + "ReportView.aspx");
            // return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iNgay1 = iNgay1, iNgay2 = iNgay2, LoaiBaoCao = LoaiBaoCao });
        }
        
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2, String LoaiBaoCao)
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
            String TenDV = "";
            if (LoaiBaoCao == "0")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi))
                {
                    TenDV = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen").ToString();
                }
            }
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptChiTietTheoDonVi_2");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2, LoaiBaoCao);
            fr.SetValue("MaDV", iID_MaDonVi);
            fr.SetValue("Ma", iID_MaTaiKhoan);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang1", iThang1);
            fr.SetValue("Thang2", iThang2);
            fr.SetValue("Ngay1", iNgay1);
            fr.SetValue("Ngay2", iNgay2);
            fr.SetValue("ngay", ngay);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("LoaiBaoCao", LoaiBaoCao);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("TenTaiKhoan", iID_MaTaiKhoan + "-" + TenTK);
            fr.Run(Result);
            return Result;

        }
        
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "0")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2, LoaiBaoCao);
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
        
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "0")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2, LoaiBaoCao);
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
        
        public ActionResult ViewPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2, String LoaiBaoCao)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            String DuongDan = "";
            if (LoaiBaoCao == "0")
            {
                DuongDan = sFilePath1;
            }
            else
            {
                DuongDan = sFilePath2;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2, LoaiBaoCao);
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
        
        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2, String LoaiBaoCao)
        {
            DataTable data = TongHopChungTuGoc(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2, LoaiBaoCao);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtPhongBan = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi,sTen", "", "");
            dtPhongBan.TableName = "PhongBan";
            fr.AddTable("PhongBan", dtPhongBan);

            DataTable dtThang = HamChung.SelectDistinct("Thang", data, "iThangCT", "iThangCT");
            fr.AddTable("Thang", dtThang);
            data.Dispose();

            DataTable dtDuDauKy=SoDauKy(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang1, iNgay1);
            dtDuDauKy = DienDuLieuKhoiTao(dtDuDauKy, dtPhongBan);
            fr.AddTable("DuDauKy", dtDuDauKy);
            dtDuDauKy.Dispose();
            
            DataTable dataLK = LuyKe(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang2,iNgay2);
            dataLK.TableName = "LuyKe";
            dataLK = DienDuLieuKhoiTao(dataLK, dtPhongBan);
            fr.AddTable("LuyKe", dataLK);
            dataLK.Dispose();
           
            
            dtPhongBan.Dispose();

        }
        
        public DataTable TongHopChungTuGoc(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2, String LoaiBaoCao)
        {
            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            String DKPB = " AND iID_MaDonVi IN (";
            String[] arrPhongBan = iID_MaDonVi.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrPhongBan.Length; i++)
            {
                DKPB += " @iID_MaDonVi" + i;
                if (i < arrPhongBan.Length - 1)
                    DKPB += " , ";
            }
            DKPB += " ) ";
            String SQL = " SELECT sSoChungTu,ithang,iNgay,iThangCT,iNgayCT,sSoChungTuChiTiet,sNoiDung,iID_MaTaiKhoan,iID_MaTaiKhoan_DoiUng,iID_MaDonVi,sTen,SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo "; 
            SQL +=" FROM (SELECT bang.sSoChungTu, bang.ithang,bang.iNgay,bang.iThangCT,bang.iNgayCT,bang.sSoChungTuChiTiet";
            SQL += " ,bang.sNoiDung,bang.iID_MaTaiKhoan,bang.iID_MaTaiKhoan_DoiUng,bang.iID_MaDonVi,bang.sTen";
            SQL += " ,SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo";
            SQL += " FROM(SELECT KT_ChungTu.sSoChungTu,C.iNamLamViec AS iNam";
            SQL += " ,C.iThang,C.iNgay,C.iNgayCT,C.iThangCT,C.sSoChungTuChiTiet";
            SQL += " ,C.sNoiDung,C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan_DoiUng";
            SQL += " ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No like @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co like @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END,";
            SQL += " NS_DonVi.iID_MaDonVi as iID_MaDonVi,NS_DonVi.sTen as sTen  FROM KT_ChungTuChiTiet as C";
            SQL += "  INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =C.iID_MaChungTu ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi ";
            SQL += " WHERE C.iTrangThai = 1 and C.iThangCT<>0 AND iID_MaTaiKhoan_No like  @iID_MaTaiKhoan";
            SQL += " AND (iThangCT>@iThang1 OR (iThangCT=@iThang1 AND iNgayCT>=@iNgay1))";
            SQL += " AND (iThangCT<@iThang2 OR (iThangCT=@iThang2 AND iNgayCT<=@iNgay2))";
            SQL += "  AND C.iNamLamViec = @iNamLamViec {0} AND C.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            SQL += " GROUP BY KT_ChungTu.sSoChungTu,C.iNamLamViec,C.iThang,C.iNgay,C.iNgayCT,C.iThangCT,C.sSoChungTuChiTiet";
            SQL += " ,C.sNoiDung,C.iID_MaTaiKhoan_No ,C.iID_MaTaiKhoan_Co,NS_DonVi.iID_MaDonVi ,NS_DonVi.sTen";
            SQL += "  HAVING SUM(rSoTien)!=0 )as bang";
            SQL += "  GROUP BY sSoChungTu, ithang,iNgay,iThangCT,iNgayCT,sSoChungTuChiTiet,sNoiDung,iID_MaTaiKhoan,iID_MaTaiKhoan_DoiUng";
            SQL += ",TaiKhoanNo,TaiKhoanCo,iID_MaDonVi,sTen HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0 ";
            SQL += " UNION";
            SQL += " SELECT bang1.sSoChungTu, bang1.ithang,bang1.iNgay,bang1.iThangCT,bang1.iNgayCT,bang1.sSoChungTuChiTiet";
            SQL += ",bang1.sNoiDung,bang1.iID_MaTaiKhoan,bang1.iID_MaTaiKhoan_DoiUng,bang1.iID_MaDonVi,bang1.sTen";
            SQL += ",SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo";
            SQL += " from(SELECT KT_ChungTu.sSoChungTu,C.iNamLamViec AS iNam";
            SQL += ",C.iThang,C.iNgay,C.iNgayCT,C.iThangCT,C.sSoChungTuChiTiet";
            SQL += ",C.sNoiDung,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan,C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan_DoiUng";
            SQL += ",TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No like @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co like @iID_MaTaiKhoan THEN SUM(rSoTien) ELSE 0 END,";
            SQL += " NS_DonVi.iID_MaDonVi as iID_MaDonVi,NS_DonVi.sTen as sTen ";
            SQL += " FROM KT_ChungTuChiTiet as C  INNER JOIN KT_ChungTu ON KT_ChungTu.iID_MaChungTu =C.iID_MaChungTu ";
            SQL += "INNER JOIN  (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi  ON C.iID_MaDonVi_Co =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE C.iTrangThai = 1 and C.iThangCT<>0";
            SQL += " AND (iThangCT>@iThang1 OR (iThangCT=@iThang1 AND iNgayCT>=@iNgay1))";
            SQL += " AND (iThangCT<@iThang2 OR (iThangCT=@iThang2 AND iNgayCT<=@iNgay2))";
            SQL += "  AND iID_MaTaiKhoan_Co like @iID_MaTaiKhoan ";
            SQL += "  AND C.iNamLamViec = @iNamLamViec {0} AND C.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            SQL += " GROUP BY KT_ChungTu.sSoChungTu,C.iNamLamViec ,C.iThang,C.iNgay,C.iNgayCT,C.iThangCT,C.sSoChungTuChiTiet,C.sNoiDung,C.iID_MaTaiKhoan_No ";
            SQL += ",C.iID_MaTaiKhoan_Co,NS_DonVi.iID_MaDonVi ,NS_DonVi.sTen   HAVING SUM(rSoTien)!=0 )as bang1";
            SQL += "  GROUP BY sSoChungTu, ithang,iNgay,iThangCT,iNgayCT,sSoChungTuChiTiet,sNoiDung,iID_MaTaiKhoan,iID_MaTaiKhoan_DoiUng,TaiKhoanNo";
            SQL += "  ,TaiKhoanCo ,iID_MaDonVi,sTen HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0";
            SQL += " ) as KT GROUP BY sSoChungTu,ithang,iNgay,iThangCT,iNgayCT,sSoChungTuChiTiet,sNoiDung,iID_MaTaiKhoan,iID_MaTaiKhoan_DoiUng,iID_MaDonVi,sTen ";
            SQL += " ORDER By iThangCT,iID_MaDonVi,iNgay";
            SQL = String.Format(SQL, DKPB);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan+"%");
            cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            cmd.Parameters.AddWithValue("@iNgay1", iNgay1);
            cmd.Parameters.AddWithValue("@iNgay2", iNgay2);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            for (int i = 0; i < arrPhongBan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrPhongBan[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
        
        public DataTable LuyKe(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang2, String iNgay2)
        {

            DataTable dt = new DataTable();

            //String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String DK = "";
            String[] arrPhongBan = iID_MaDonVi.Split(',');
            SqlCommand cmd = new SqlCommand();
            String DK_No = " AND iID_MaDonVi_No IN (";
            String DK_Co = " AND iID_MaDonVi_Co IN (";
            for (int i = 0; i < arrPhongBan.Length; i++)
            {
                DK += " @iID_MaDonVi" + i;
                if (i < arrPhongBan.Length - 1)
                    DK += " , ";
            }
            DK += " ) ";

            String SQL = @"SELECT iID_MaDonVi,SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo
                                FROM (
                                SELECT iID_MaDonVi_No as iID_MaDonVi,TaiKhoanNo=SUM(rSoTien),TaiKhoanCo=0
                                FROM KT_ChungTuChiTiet
                                WHERE iTrangThai=1 AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No IS NOT NULL
	                                AND iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_No like @iID_MaTaiKhoan AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet {0}
                                    AND (iThangCT<>0  AND  iThangCT<@iThang2 OR (iThangCT=@iThang2 AND iNgayCT<=@iNgay2))
                                GROUP BY iID_MaDonVi_No

                                UNION

                                SELECT iID_MaDonVi_Co as iID_MaDonVi,TaiKhoanNo=0,TaiKhoanCo=SUM(rSoTien)
                                FROM KT_ChungTuChiTiet
                                WHERE iTrangThai=1 AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co IS NOT NULL
	                                AND iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_Co like @iID_MaTaiKhoan AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet {1}
                                    AND (iThangCT<>0 AND iThangCT<@iThang2 OR (iThangCT=@iThang2 AND iNgayCT<=@iNgay2))
                                GROUP BY iID_MaDonVi_Co) a
                                GROUP BY iID_MaDonVi
                                ORDER BY iID_MaDonVi
                                ";
            SQL = String.Format(SQL, DK_No + DK, DK_Co + DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan + "%");
            cmd.Parameters.AddWithValue("@iThang2", iThang2);
            cmd.Parameters.AddWithValue("@iNgay2", iNgay2);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            for (int i = 0; i < arrPhongBan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrPhongBan[i]);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }

        
        public DataTable DienDuLieuKhoiTao(DataTable dt,DataTable dtDV)     
        {

             DataRow R;
                Boolean CoDV = false;
                String iID_MaDonVi_1, iID_MaDonVi_2;
                for (int i = 0; i < dtDV.Rows.Count; i++)
                {
                    CoDV = false;
                    int d = 0;
                    iID_MaDonVi_1 = Convert.ToString(dtDV.Rows[i]["iID_MaDonVi"]);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        iID_MaDonVi_2 = Convert.ToString(dt.Rows[j]["iID_MaDonVi"]);
                        if (iID_MaDonVi_1.Equals(iID_MaDonVi_2))
                        {
                            CoDV = true;
                        }
                    }
                    if (CoDV == false)
                    {
                        R = dt.NewRow();
                        R["iID_MaDonVi"] = iID_MaDonVi_1;
                        R["TaiKhoanNo"] = 0;
                        R["TaiKhoanCo"] = 0;
                        dt.Rows.Add(R);
                    }
                }
            return dt;
            
        }
        public DataTable SoDauKy(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iNgay1)
        {
            DataTable dt = new DataTable();
           
                //String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
                int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                String DK = "";
                String[] arrPhongBan = iID_MaDonVi.Split(',');
                SqlCommand cmd = new SqlCommand();
                String DK_No = " AND iID_MaDonVi_No IN (";
                String DK_Co = " AND iID_MaDonVi_Co IN (";
                for (int i = 0; i < arrPhongBan.Length; i++)
                {
                    DK += " @iID_MaDonVi" + i;
                    if (i < arrPhongBan.Length - 1)
                        DK += " , ";
                }
                DK += " ) ";

                String SQL = @"SELECT iID_MaDonVi,SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo
                                FROM (
                                SELECT iID_MaDonVi_No as iID_MaDonVi,TaiKhoanNo=SUM(rSoTien),TaiKhoanCo=0
                                FROM KT_ChungTuChiTiet
                                WHERE iTrangThai=1 AND iID_MaDonVi_No<>'' AND iID_MaDonVi_No IS NOT NULL
	                                AND iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_No like @iID_MaTaiKhoan AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet {0}
                                    AND (iThangCT<@iThang1 OR (iThangCT=@iThang1 AND iNgayCT<@iNgay1))
                                GROUP BY iID_MaDonVi_No

                                UNION

                                SELECT iID_MaDonVi_Co as iID_MaDonVi,TaiKhoanNo=0,TaiKhoanCo=SUM(rSoTien)
                                FROM KT_ChungTuChiTiet
                                WHERE iTrangThai=1 AND iID_MaDonVi_Co<>'' AND iID_MaDonVi_Co IS NOT NULL
	                                AND iNamLamViec=@iNamLamViec AND iID_MaTaiKhoan_Co like @iID_MaTaiKhoan AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet {1}
                                    AND (iThangCT<@iThang1 OR (iThangCT=@iThang1 AND iNgayCT<@iNgay1))
                                GROUP BY iID_MaDonVi_Co) a
                                GROUP BY iID_MaDonVi
                                ORDER BY iID_MaDonVi
                                ";
                SQL = String.Format(SQL, DK_No + DK, DK_Co + DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan+"%");
                cmd.Parameters.AddWithValue("@iThang1", iThang1);
                cmd.Parameters.AddWithValue("@iNgay1", iNgay1);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                for (int i = 0; i < arrPhongBan.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrPhongBan[i]);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    Double SoDu = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SoDu = 0;

                        SoDu = Convert.ToDouble(dt.Rows[i]["TaiKhoanNo"]) - Convert.ToDouble(dt.Rows[i]["TaiKhoanCo"]);
                        if (SoDu > 0)
                            dt.Rows[i]["TaiKhoanNo"] = SoDu;
                        else
                            dt.Rows[i]["TaiKhoanCo"] = SoDu * (-1);
                    }
                }
            return dt;
        }
        
        public DataTable SoDuCuoiKy(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iNgay)
        {
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String DK = " AND iID_MaDonVi IN (";
            String[] arrPhongBan = iID_MaDonVi.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrPhongBan.Length; i++)
            {
                DK += " @iID_MaDonVi" + i;
                if (i < arrPhongBan.Length - 1)
                    DK += " , ";
            }
            DK += " ) ";

            String SQL = " SELECT iThangCT,bang.iID_MaDonVi ,SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo";
            SQL += " FROM(SELECT  iThangCT,C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan_DoiUng";
            SQL += ",TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan +'%' THEN SUM(rSoTien) ELSE 0 END";
            SQL += ",TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan +'%' THEN SUM(rSoTien) ELSE 0 END,";
            SQL += " NS_DonVi.iID_MaDonVi as iID_MaDonVi FROM KT_ChungTuChiTiet as C  ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi  ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi  or C.iID_MaDonVi_Co =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE C.iTrangThai = 1  AND iID_MaTaiKhoan_No Like @iID_MaTaiKhoan +'%'  ";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, C.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, C.iNgayCT), 111) <= @DenNgay)";
            SQL += " AND C.iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet AND C.iNamLamViec = @iNamLamViec {0}";
            SQL += " GROUP BY C.iID_MaTaiKhoan_No ,C.iID_MaTaiKhoan_Co,NS_DonVi.iID_MaDonVi,iThangCT ";
            SQL += " HAVING SUM(rSoTien)!=0)as bang";
            SQL += " GROUP BY iID_MaDonVi,iThangCT HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0";
            SQL += " UNION";
            SQL += " SELECT iThangCT,bang1.iID_MaDonVi";
            SQL += " ,SUM(TaiKhoanCo) as TaiKhoanCo,SUM(TaiKhoanNo) as TaiKhoanNo  from(SELECT ";
            SQL += "iThangCT,C.iID_MaTaiKhoan_Co AS iID_MaTaiKhoan,C.iID_MaTaiKhoan_No AS iID_MaTaiKhoan_DoiUng";
            SQL += " ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan +'%' THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan +'%' THEN SUM(rSoTien) ELSE 0 END,";
            SQL += " NS_DonVi.iID_MaDonVi as iID_MaDonVi";
            SQL += " FROM KT_ChungTuChiTiet as C  ";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON C.iID_MaDonVi_No =NS_DonVi.iID_MaDonVi  or C.iID_MaDonVi_Co =NS_DonVi.iID_MaDonVi";
            SQL += " WHERE  C.iTrangThai = 1  AND iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan +'%'";
            SQL += " AND (CONVERT(Datetime, CONVERT(varchar, C.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, C.iNgayCT), 111) <= @DenNgay)";
            SQL += " AND C.iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet AND C.iNamLamViec = @iNamLamViec {0}";
            SQL += " GROUP BY C.iID_MaTaiKhoan_No,iThangCT ";
            SQL += " ,C.iID_MaTaiKhoan_Co,NS_DonVi.iID_MaDonVi HAVING SUM(rSoTien)!=0)as bang1";
            SQL += " GROUP BY iID_MaDonVi,iThangCT HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0";
            SQL += " ORDER By iThangCT,iID_MaDonVi";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            //cmd.Parameters.AddWithValue("@iThang1", iThang1);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            //cmd.Parameters.AddWithValue("@iNgay2", iNgay2);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            for (int i = 0; i < arrPhongBan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrPhongBan[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.InsertAt(dr, 0);
            }
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
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            dt.Dispose();
            return dt;
        }
        
        public static DataTable TenTaiKhoan(String NamChungTu)
        {
            DataTable dt;
            String KyHieu = "62";
            String[] arrThamSo;
            String ThamSo = "";
            String DKSELECT = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sKyHieu=@sKyHieu";
            SqlCommand cmdThamSo = new SqlCommand(DKSELECT);
            cmdThamSo.Parameters.AddWithValue("@sKyHieu", KyHieu);
            cmdThamSo.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            DataTable dtThamSo = Connection.GetDataTable(cmdThamSo);
            if (dtThamSo.Rows.Count > 0)
            {
                arrThamSo = Convert.ToString(dtThamSo.Rows[0]["sThamSo"]).Split(',');
                for (int i = 0; i < arrThamSo.Length; i++)
                {
                    ThamSo += arrThamSo[i];
                    if (i < arrThamSo.Length - 1)
                        ThamSo += " , ";
                }
            }
            else
            {
                ThamSo = "-1";
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
            return MyHtmlHelper.DropDownList(ParentID, slNgay, Ngay, TenTruong, "", "style=\"width:20%\"");
        }

        public JsonResult ObjDanhSachDonVi(String TuNgay, String DenNgay, String MaND, String iID_MaDonVi)
        {
            return Json(get_sDanhSachDonVi(TuNgay, DenNgay, MaND, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String TuNgay, String DenNgay, String MaND,String iID_MaDonVi)
        {
            String DK = " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) >= @TuNgay) ";
            DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgay)";
            DK += " AND iThangCT>0";

            String SQL = "SELECT distinct iID_MaDonVi_Co AS iID_MaDonVi FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi_Co<>'' {0}";
            SQL += " UNION ";
            SQL += " SELECT distinct iID_MaDonVi_No AS iID_MaDonVi FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi_No<>'' {0}";
            SQL += " ORDER BY iID_MaDonVi";
            SQL = String.Format(SQL, DK);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String MaDonVi1 = "", MaDonVi2 = "", TenDonVi = "";
            DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MaDonVi1 = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < dtDonVi.Rows.Count; j++)
                {
                    MaDonVi2 = Convert.ToString(dtDonVi.Rows[j]["iID_MaDonVi"]);
                    TenDonVi = Convert.ToString(dtDonVi.Rows[j]["sTen"]);
                    if (MaDonVi1.Equals(MaDonVi2))
                    {
                        dtDonVi.Rows[j]["sTen"] = TenDonVi + " (<b style=\"color:Red;\">*</b>)";
                    }
                }
            }
            int SoCot = 2;
            if (dtDonVi.Rows.Count >= 10)
                SoCot = 5;

            StringBuilder stb = new StringBuilder();
            String[] arrMaDonVi = iID_MaDonVi.Split(',');
            stb.Append("<table  class=\"mGrid\">");
            //stb.Append("<tr>");
            //stb.Append("<th align=\"center\"> <input type=\"checkbox\"  id=\"abc\" onclick=\"CheckAll(this.checked)\" /></th>");
            ////for (int c = 0; c < SoCot * 2 - 1; c++)
            ////{
            ////    stb.Append("<th></th>");
            ////}
            //stb.Append("<th align=\"center\" style=\"font-size:13.7px;\">Tên đơn vị</th>");
            //stb.Append("</tr>");

            String strsTen = "", MaDonVi = "", strChecked = "";
            //for (int i = 0; i < dtDonVi.Rows.Count; i = i + SoCot)
            //{
            //    stb.Append("<tr>");
            //    for (int c = 0; c < SoCot; c++)
            //    {
            //        if (i + c < dtDonVi.Rows.Count)
            //        {
            //            strChecked = "";
            //            strsTen = Convert.ToString(dtDonVi.Rows[i + c]["sTen"]);
            //            MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["iID_MaDonVi"]);
            //            for (int j = 0; j < arrMaDonVi.Length; j++)
            //            {
            //                if (MaDonVi.Equals(arrMaDonVi[j]))
            //                {
            //                    strChecked = "checked=\"checked\"";
            //                    break;
            //                }
            //            }
            //            stb.Append("<td align=\"left\">");
            //            stb.Append("<input type=\"checkbox\" "+ strChecked +" value=\"" + MaDonVi + "\"check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\"/>");
            //            stb.Append("</td>");
            //            stb.Append("<td align=\"left\">" + strsTen + "</td>");
            //        }
            //    }
            //    stb.Append("</tr>");
            //}
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                stb.Append("<tr>");
                strChecked = "";
                strsTen = Convert.ToString(dtDonVi.Rows[i]["sTen"]);
                MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < arrMaDonVi.Length; j++)
                {
                    if (MaDonVi.Equals(arrMaDonVi[j]))
                    {
                        strChecked = "checked=\"checked\"";
                        break;
                    }
                }
                stb.Append("<td align=\"center\" style=\"width:25px;\">");
                stb.Append("<input type=\"checkbox\" " + strChecked + " value=\"" + MaDonVi + "\"check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\"/>");
                stb.Append("</td>");
                stb.Append("<td align=\"left\">" + strsTen + "</td>");
                stb.Append("</tr>");
            }
            stb.Append("</table>");
            stb.Append("<script type=\"text/javascript\">");
            stb.Append("function CheckAll(value) {");
            stb.Append("$(\"input:checkbox[check-group='DonVi']\").each(function (i) {");
            stb.Append("this.checked = value;");
            stb.Append("});");
            stb.Append("}");
            stb.Append("</script>");
            return stb.ToString(); ;
        }
    }
}