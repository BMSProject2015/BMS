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
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.KeToan.TienMat
{
    public class rptKT_ChiTiet_TaiKhoanTheoDonVi1Controller : Controller
    {
        //
        // GET: /rptKT_ChiTiet_TaiKhoanTheoDonVi1/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienMat/rptKT_ChiTiet_TaiKhoanTheoDonVi1.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptKT_ChiTiet_TaiKhoanTheoDonVi1.aspx";
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
           ViewData["PageLoad"] = "1";
           ViewData["iNamLamViec"] = iNamLamViec;
           ViewData["iThang"] = iThang;
           ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
           ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptKT_ChiTiet_TaiKhoanTheoDonVi1.aspx";
           return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iNamLamViec = iNamLamViec,iThang = iThang});
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iThang, String iTrangThai)
        {
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKT_ChiTiet_TaiKhoanTheoDonVi1");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iNamLamViec, iThang, iTrangThai);
            fr.SetValue("Nam", NguoiDungCauHinhModels.iNamLamViec.ToString());
            fr.SetValue("Thang", iThang);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;

        }
      
        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang,String iTrangThai)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iThang, iTrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptChiTietTaiKhoanTheoDonVi1.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String iNamLamViec, String iThang,String iTrangThai)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iThang, iTrangThai);
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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iThang, String iTrangThai)
        {
            DataTable data = TongHopChungTuGoc(iNamLamViec, iThang, iTrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtPB = HamChung.SelectDistinct("PB", data, "PhongBan,MaTaiKhoan", "PhongBan,MaTaiKhoan,sTen", "", "");            
            fr.AddTable("PB", dtPB);

            DataTable dtTK = HamChung.SelectDistinct("TK", dtPB, "MaTaiKhoan", "MaTaiKhoan,sTen", "", "");
            fr.AddTable("TK", dtTK);
            data.Dispose();
            dtPB.Dispose();
            dtTK.Dispose();

            DataTable data2 = TongHopChungTuGoc(iNamLamViec, iThang, iTrangThai);
            data.TableName = "SoDu";
            fr.AddTable("SoDu", data2);
            data2.Dispose();
        }

        public DataTable TongHopChungTuGoc(String iNamLamViec, String iThang, String iTrangThai)
        {
            String DKDuyet = "AND CT.iTrangThai=1 ";
            if (iTrangThai != "0")
            {
                DKDuyet = "AND CT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MaTaiKhoan,sTen,PhongBan,DonVi,sum(DENTHANGNO)as denthangno, SUM(DENTHANGCO)as denthangco";
            SQL+=" ,sum(benno)as benno,sum(benco)as benco,sum(ckno) as ckno,sum(ckco)as ckco";
            SQL+=" FROM(";
            SQL += " select BANG.MaTaiKhoan,BANG.sTen,BANG.PhongBan,BANG.DonVi,sum(DENTHANGNO)as denthangno, SUM(DENTHANGCO)as denthangco";
            SQL += ",sum(benno)as benno,sum(benco)as benco,sum(Cuoikyno) as ckno,sum(Cuoikyco)as ckco FROM(";
            SQL += " SELECT CT.iID_MaTaiKhoan_No as MaTaiKhoan,TK.sTen";
            SQL += " ,CT.iID_MaDonVi_No as DonVi,CT.iID_MaPhongBan_No as PhongBan";
            SQL += " ,benno= case when iThangCT =@iThang then sum(rSoTien) else 0 end";
            SQL += " ,benco = 0";
            SQL += " ,DENTHANGNO=CASE WHEN ithangCT<=@iThang AND iThangCT<>0 THEN SUM(rSoTien) ELSE 0 END, DENTHANGCO=0";
            SQL += " ,Cuoikyno= case when iThangCT<=@iThang then sum(rSoTien) else 0 end,Cuoikyco=0";
            SQL += " FROM KT_ChungTuChiTiet as CT";
            SQL += " INNER JOIN KT_TaiKhoan as TK on TK.iID_MaTaiKhoan =CT.iID_MaTaiKhoan_No and TK.iNam = @iNamLamViec";
            SQL += " WHERE  CT.iNamLamViec=@iNamLamViec  {0} and iID_MaPhongBan_No is not null AND iID_MaPhongBan_No!=' ' AND CT.iID_MaTaiKhoan_No IS NOT NULL AND SUBSTRING(CT.iID_MaTaiKhoan_No,1,1)<>0";
            SQL += " GROUP BY CT.iID_MaTaiKhoan_No,TK.sTen,CT.iID_MaDonVi_No,CT.iID_MaPhongBan_No,CT.ithangCT) AS BANG";
            SQL += " GROUP BY BANG.MaTaiKhoan,BANG.sTen,BANG.PhongBan,BANG.DonVi";
            SQL += " UNION";
            SQL += " SELECT BANG1.MaTaiKhoan,BANG1.sTen,BANG1.PhongBan,BANG1.DonVi,sum(DENTHANGNO)as denthangno, SUM(DENTHANGCO)as denthangco,sum(benno)as benno,sum(benco)as benco";
            SQL += ",sum(Cuoikyno) as ckno,sum(Cuoikyco)as ckco FROM(";
            SQL += " SELECT  CT.iID_MaTaiKhoan_Co as MaTaiKhoan,TK.sTen";
            SQL += " ,CT.iID_MaDonVi_Co as DonVi,CT.iID_MaPhongBan_Co as PhongBan,benno =0";
            SQL += " ,benco = case when iThangCT = @iThang then sum(rSoTien) else 0 end";
            SQL += " ,DENTHANGNO=0,DENTHANGCO=CASE WHEN ithangCT<=@iThang AND iThangCT<>0 THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,Cuoikyno=0,Cuoikyco= case when iThangCT<=@iThang then sum(rSoTien) else 0 end";
            SQL += "  FROM KT_ChungTuChiTiet as CT";
            SQL += " INNER JOIN KT_TaiKhoan as TK on TK.iID_MaTaiKhoan =CT.iID_MaTaiKhoan_Co and TK.iNam = @iNamLamViec";
            SQL += " WHERE CT.iNamLamViec=@iNamLamViec  {0} and iID_MaPhongBan_Co is not null AND iID_MaPhongBan_Co!=' ' AND CT.iID_MaTaiKhoan_Co IS NOT NULL AND SUBSTRING(CT.iID_MaTaiKhoan_Co,1,1)<>0";
            SQL += " GROUP BY CT.iID_MaTaiKhoan_Co ,TK.sTen,CT.iID_MaDonVi_Co  ,CT.iID_MaPhongBan_Co,CT.ithangCT)AS BANG1";
            SQL += " GROUP BY BANG1.MaTaiKhoan,BANG1.sTen,BANG1.PhongBan,BANG1.DonVi ) as a";
            SQL += " GROUP BY MaTaiKhoan,sTen,PhongBan,DonVi";
            SQL += " HAVING sum(DENTHANGNO)<>0 OR SUM(DENTHANGCO)<>0 OR sum(benno)<>0 OR SUM(benco)<>0 OR sum(ckno)<>0 OR sum(ckco)<>0"; 
            SQL = String.Format(SQL,DKDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }


        public DataTable TaiKhoanNgoaiBang(String iNamLamViec, String iThang, String iTrangThai)
        {
            String DKDuyet = "AND CT.iTrangThai=1 ";
            if (iTrangThai != "0")
            {
                DKDuyet = "AND CT.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MaTaiKhoan,sTen,PhongBan,DonVi,sum(DENTHANGNO)as denthangno, SUM(DENTHANGCO)as denthangco";
            SQL += " ,sum(benno)as benno,sum(benco)as benco,sum(ckno) as ckno,sum(ckco)as ckco";
            SQL += " FROM(";
            SQL += " select BANG.MaTaiKhoan,BANG.sTen,BANG.PhongBan,BANG.DonVi,sum(DENTHANGNO)as denthangno, SUM(DENTHANGCO)as denthangco";
            SQL += ",sum(benno)as benno,sum(benco)as benco,sum(Cuoikyno) as ckno,sum(Cuoikyco)as ckco FROM(";
            SQL += " SELECT CT.iID_MaTaiKhoan_No as MaTaiKhoan,TK.sTen";
            SQL += " ,CT.iID_MaDonVi_No as DonVi,CT.iID_MaPhongBan_No as PhongBan";
            SQL += " ,benno= case when iThangCT =@iThang then sum(rSoTien) else 0 end";
            SQL += " ,benco = 0";
            SQL += " ,DENTHANGNO=CASE WHEN ithangCT<=@iThang AND iThangCT<>0 THEN SUM(rSoTien) ELSE 0 END, DENTHANGCO=0";
            SQL += " ,Cuoikyno= case when iThangCT<=@iThang then sum(rSoTien) else 0 end,Cuoikyco=0";
            SQL += " FROM KT_ChungTuChiTiet as CT";
            SQL += " INNER JOIN KT_TaiKhoan as TK on TK.iID_MaTaiKhoan =CT.iID_MaTaiKhoan_No and TK.iNam = @iNamLamViec";
            SQL += " WHERE  CT.iNamLamViec=@iNamLamViec  {0} and iID_MaPhongBan_No is not null AND iID_MaPhongBan_No!=' ' AND CT.iID_MaTaiKhoan_No IS NOT NULL AND SUBSTRING(CT.iID_MaTaiKhoan_No,1,1)=0";
            SQL += " GROUP BY CT.iID_MaTaiKhoan_No,TK.sTen,CT.iID_MaDonVi_No,CT.iID_MaPhongBan_No,CT.ithangCT) AS BANG";
            SQL += " GROUP BY BANG.MaTaiKhoan,BANG.sTen,BANG.PhongBan,BANG.DonVi";
            SQL += " UNION";
            SQL += " SELECT BANG1.MaTaiKhoan,BANG1.sTen,BANG1.PhongBan,BANG1.DonVi,sum(DENTHANGNO)as denthangno, SUM(DENTHANGCO)as denthangco,sum(benno)as benno,sum(benco)as benco";
            SQL += ",sum(Cuoikyno) as ckno,sum(Cuoikyco)as ckco FROM(";
            SQL += " SELECT  CT.iID_MaTaiKhoan_Co as MaTaiKhoan,TK.sTen";
            SQL += " ,CT.iID_MaDonVi_Co as DonVi,CT.iID_MaPhongBan_Co as PhongBan,benno =0";
            SQL += " ,benco = case when iThangCT = @iThang then sum(rSoTien) else 0 end";
            SQL += " ,DENTHANGNO=0,DENTHANGCO=CASE WHEN ithangCT<=@iThang AND iThangCT<>0 THEN SUM(rSoTien) ELSE 0 END";
            SQL += " ,Cuoikyno=0,Cuoikyco= case when iThangCT<=@iThang then sum(rSoTien) else 0 end";
            SQL += "  FROM KT_ChungTuChiTiet as CT";
            SQL += " INNER JOIN KT_TaiKhoan as TK on TK.iID_MaTaiKhoan =CT.iID_MaTaiKhoan_Co and TK.iNam = @iNamLamViec";
            SQL += " WHERE CT.iNamLamViec=@iNamLamViec  {0} and iID_MaPhongBan_Co is not null AND iID_MaPhongBan_Co!=' ' AND CT.iID_MaTaiKhoan_Co IS NOT NULL AND SUBSTRING(CT.iID_MaTaiKhoan_Co,1,1)=0";
            SQL += " GROUP BY CT.iID_MaTaiKhoan_Co ,TK.sTen,CT.iID_MaDonVi_Co  ,CT.iID_MaPhongBan_Co,CT.ithangCT)AS BANG1";
            SQL += " GROUP BY BANG1.MaTaiKhoan,BANG1.sTen,BANG1.PhongBan,BANG1.DonVi ) as a";
            SQL += " GROUP BY MaTaiKhoan,sTen,PhongBan,DonVi";
            SQL += " HAVING sum(DENTHANGNO)<>0 OR SUM(DENTHANGCO)<>0 OR sum(benno)<>0 OR SUM(benco)<>0 OR sum(ckno)<>0 OR sum(ckco)<>0";
            SQL = String.Format(SQL, DKDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
    }
}
