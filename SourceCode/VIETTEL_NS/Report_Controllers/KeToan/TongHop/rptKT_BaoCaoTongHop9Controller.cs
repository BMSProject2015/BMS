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

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKT_BaoCaoTongHop9Controller : Controller
    {
        //
        // GET: /rptKT_BaoCaoTongHop9C/

        public string sViewPath = "~/Report_Views/";
        public ActionResult Index(String LoaiBaoCao="")
        {
            String sFilePath = "";
            if (LoaiBaoCao == "0")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9A.xls";
            }
            else if (LoaiBaoCao == "1")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9B.xls";
            }
            else if (LoaiBaoCao == "2")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9C.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKT_BaoCaoTongHop9.aspx";      
            return View(sViewPath + "ReportView.aspx");
             }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String LoaiBaoCao = Request.Form[ParentID + "_LoaiBaoCao"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String TuNgay = Request.Form[ParentID + "_TuNgay"];
            String DenNgay = Request.Form[ParentID + "_DenNgay"];
            String TuThang = Request.Form[ParentID + "_TuThang"];
            String DenThang = Request.Form[ParentID + "_DenThang"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            return RedirectToAction("Index", new { NamLamViec = NamLamViec,LoaiBaoCao=LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaPhongBan = iID_MaPhongBan, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String LoaiBaoCao, String iID_MaLoaiTaiSan, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String tendv = "";
            DataTable teN = TenPhongBan(iID_MaPhongBan);
            if (teN != null)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            using (FlexCelReport fr = new FlexCelReport())
            {
                LoadData(fr, NamLamViec,LoaiBaoCao, iID_MaLoaiTaiSan, TuNgay, DenNgay, TuThang, DenThang, iID_MaPhongBan);
                fr.SetValue("Nam", NamLamViec);
                fr.SetValue("TuNgay", TuNgay);
                fr.SetValue("DenNgay", DenNgay);
                fr.SetValue("TuThang", TuThang);
                fr.SetValue("DenThang", DenThang);
                fr.SetValue("TenPhongBan", tendv);
                fr.Run(Result);
                return Result;
            }
        }
        public static DataTable DTLoaiBaoCao()
        {
            DataTable dtloaiBC = new DataTable();
            dtloaiBC.Columns.Add("MaLoaiBaoCao",(typeof(string)));
            dtloaiBC.Columns.Add("TenLoaiBaoCao", (typeof(string)));
            DataRow r = dtloaiBC.NewRow();
            r["MaLoaiBaoCao"] = "0";
            r["TenLoaiBaoCao"] = "Tổng hợp sử dụng nhà đất Mẫu - 9A";
            dtloaiBC.Rows.InsertAt(r, 0);
            DataRow r1 = dtloaiBC.NewRow();
            r1["MaLoaiBaoCao"] = "1";
            r1["TenLoaiBaoCao"] = "Tổng hợp sử dụng nhà đất Mẫu - 9B";
            dtloaiBC.Rows.InsertAt(r1, 1);
            DataRow r2 = dtloaiBC.NewRow();
            r2["MaLoaiBaoCao"] = "2";
            r2["TenLoaiBaoCao"] = "Tổng hợp sử dụng nhà đất Mẫu - 9C";
            dtloaiBC.Rows.InsertAt(r2, 2);
          
            return dtloaiBC;
        }
        public static DataTable rptKT_BaoCao9(String NamLamViec,String LoaiBaoCao, String iID_MaLoaiTaiSan, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaPhongBan)
        {
            DataTable dt = null;
           
            if (LoaiBaoCao == "0")
            #region Mãu báo cáo tổng hợp 9A
            {
                String DKMaLoaiTaiSan = "";
                if (iID_MaLoaiTaiSan.Equals(Guid.Empty.ToString()))
                {
                    DKMaLoaiTaiSan = "";
                }
                else
                {
                    DKMaLoaiTaiSan = " AND LTS.iID_MaLoaiTaiSan=@iID_LoaiTaiSan";
                }
                String DKMaPhongBan = "";
                if (iID_MaPhongBan.Equals(Guid.Empty.ToString()))
                {
                    DKMaPhongBan = "";
                }
                else
                {
                    DKMaPhongBan = " iID_MaPhongBan=@iID_MaPhongBan";
                }
                String SQL = " SELECT sTenTaiSan,sKyHieuPhongBan";
                SQL += " ,SUM(rSoLuong) as SoLuong";
                SQL += " ,SUM(rDTKhuonVien) as KhuonVien";
                SQL += " ,SUM(rDTLamNhaLamViec) as NhaLamViec";
                SQL += " ,SUM(rDTLamCoSoHDSN) as CoSoHDBS";
                SQL += " ,SUM(rDTLamNhaO) as LamNhaO";
                SQL += " ,SUM(rDTChoThue) as ChoThue";
                SQL += " ,SUM(rDTBoTrong) as BoTrong";
                SQL += " ,SUM(rDTBiLanChiem) as BiLanChiem";
                SQL += " ,SUM(rDTKhac) as DTKhac";
                SQL += " FROM KTCS_ChungTuChiTiet CTCT ";
                SQL += " INNER JOIN KTCS_TaiSan TS ON TS.sKyHieu=CTCT.sKyHieuTaiSan";
                SQL += " INNER JOIN KTCS_LoaiTaiSan LTS ON LTS.iID_MaLoaiTaiSan=TS.iID_MaLoaiTaiSan";
                SQL += " WHERE {0}";
                SQL += " AND CTCT.iNgayCT >=@TuNgay AND CTCT.iNgayCT<=@DenNgay AND CTCT.iThangCT>=@TuThang AND CTCT.iThangCT<=@DenThang";
                SQL += " {1}";
                SQL += " GROUP BY sTenTaiSan,sKyHieuPhongBan";
                SQL += " HAVING SUM(rSoLuong)!=0 OR SUM(rDTKhuonVien)!=0 OR SUM(rDTLamNhaLamViec)!=0";
                SQL += " OR SUM(rDTLamCoSoHDSN)!=0 OR SUM(rDTLamNhaO)!=0 OR SUM(rDTChoThue)!=0";
                SQL += " OR SUM(rDTBoTrong)!=0 OR SUM(rDTBiLanChiem)!=0 OR SUM(rDTKhac)!=0";
                SQL = String.Format(SQL, DKMaPhongBan, DKMaLoaiTaiSan);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                if (!String.IsNullOrEmpty(DKMaPhongBan))
                {
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                }
                if (!String.IsNullOrEmpty(DKMaLoaiTaiSan))
                {
                    cmd.Parameters.AddWithValue("@iID_LoaiTaiSan", iID_MaLoaiTaiSan);
                }
                cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                cmd.Parameters.AddWithValue("@TuThang", TuThang);
                cmd.Parameters.AddWithValue("@DenThang", DenThang);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion  
            if (LoaiBaoCao == "1")
            #region Mẫu báo cáo tổng hợp nhà đất 9B
            {
                String DKMaLoaiTaiSan = "";
                if (iID_MaLoaiTaiSan.Equals(Guid.Empty.ToString()))
                {
                    DKMaLoaiTaiSan = "";
                }
                else
                {
                    DKMaLoaiTaiSan = " AND LTS.iID_MaLoaiTaiSan=@iID_LoaiTaiSan";
                }
                String DKMaPhongBan = "";
                if (iID_MaPhongBan.Equals(Guid.Empty.ToString()))
                {
                    DKMaPhongBan = "";
                }
                else
                {
                    DKMaPhongBan = " iID_MaPhongBan=@iID_MaPhongBan";
                }
                String SQL = " SELECT sTenTaiSan,sKyHieuPhongBan";
                SQL += " ,SUM(rSoLuong) as SoLuong";
                SQL += " ,SUM(rDTKhuonVien) as KhuonVien";
                SQL += " ,SUM(rDTLamNhaLamViec) as NhaLamViec";
                SQL += " ,SUM(rDTLamCoSoHDSN) as CoSoHDBS";
                SQL += " ,SUM(rDTLamNhaO) as LamNhaO";
                SQL += " ,SUM(rDTChoThue) as ChoThue";
                SQL += " ,SUM(rDTBoTrong) as BoTrong";
                SQL += " ,SUM(rDTBiLanChiem) as BiLanChiem";
                SQL += " ,SUM(rDTKhac) as DTKhac";
                SQL += " FROM KTCS_ChungTuChiTiet CTCT ";
                SQL += " INNER JOIN KTCS_TaiSan TS ON TS.sKyHieu=CTCT.sKyHieuTaiSan";
                SQL += " INNER JOIN KTCS_LoaiTaiSan LTS ON LTS.iID_MaLoaiTaiSan=TS.iID_MaLoaiTaiSan";
                SQL += " WHERE {0}";
                SQL += " AND CTCT.iNgayCT >=@TuNgay AND CTCT.iNgayCT<=@DenNgay AND CTCT.iThangCT>=@TuThang AND CTCT.iThangCT<=@DenThang";
                SQL += " {1}";
                SQL += " GROUP BY sTenTaiSan,sKyHieuPhongBan";
                SQL += " HAVING SUM(rSoLuong)!=0 OR SUM(rDTKhuonVien)!=0 OR SUM(rDTLamNhaLamViec)!=0";
                SQL += " OR SUM(rDTLamCoSoHDSN)!=0 OR SUM(rDTLamNhaO)!=0 OR SUM(rDTChoThue)!=0";
                SQL += " OR SUM(rDTBoTrong)!=0 OR SUM(rDTBiLanChiem)!=0 OR SUM(rDTKhac)!=0";
                SQL = String.Format(SQL, DKMaPhongBan, DKMaLoaiTaiSan);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                if (!String.IsNullOrEmpty(DKMaPhongBan))
                {
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                }
                if (!String.IsNullOrEmpty(DKMaLoaiTaiSan))
                {
                    cmd.Parameters.AddWithValue("@iID_LoaiTaiSan", iID_MaLoaiTaiSan);
                }
                cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                cmd.Parameters.AddWithValue("@TuThang", TuThang);
                cmd.Parameters.AddWithValue("@DenThang", DenThang);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion
            if (LoaiBaoCao == "2")
            #region mẫu Báo cáo tổng hợp nhà đất 9C
            {
                String DKMaLoaiTaiSan = "";
                if (iID_MaLoaiTaiSan.Equals(Guid.Empty.ToString()))
                {
                    DKMaLoaiTaiSan = "";
                }
                else
                {
                    DKMaLoaiTaiSan = " AND LTS.iID_MaLoaiTaiSan=@iID_LoaiTaiSan";
                }
                String DKMaPhongBan = "";
                if (iID_MaPhongBan.Equals(Guid.Empty.ToString()))
                {
                    DKMaPhongBan = "";
                }
                else
                {
                    DKMaPhongBan = " PhongBan.iID_MaPhongBan=@iID_MaPhongBan";
                }
                String SQL = " SELECT PhongBan.sTen ,sTenTaiSan,sKyHieuPhongBan";
                SQL += " ,SUM(rSoLuong) as SoLuong";
                SQL += " ,SUM(rDTKhuonVien) as KhuonVien";
                SQL += " ,SUM(rDTLamNhaLamViec) as NhaLamViec";
                SQL += " ,SUM(rDTLamCoSoHDSN) as CoSoHDBS";
                SQL += " ,SUM(rDTLamNhaO) as LamNhaO";
                SQL += " ,SUM(rDTChoThue) as ChoThue";
                SQL += " ,SUM(rDTBoTrong) as BoTrong";
                SQL += " ,SUM(rDTBiLanChiem) as BiLanChiem";
                SQL += " ,SUM(rDTKhac) as DTKhac";
                SQL += " FROM KTCS_ChungTuChiTiet CTCT ";
                SQL += " INNER JOIN NS_PhongBan AS PhongBan ON CTCT.iID_MaPhongBan=PhongBan.iID_MaPhongBan";
                SQL += " INNER JOIN KTCS_TaiSan TS ON TS.sKyHieu=CTCT.sKyHieuTaiSan";
                SQL += " INNER JOIN KTCS_LoaiTaiSan LTS ON LTS.iID_MaLoaiTaiSan=TS.iID_MaLoaiTaiSan";
                SQL += " WHERE {0}";
                SQL += " AND CTCT.iNgayCT >=@TuNgay AND CTCT.iNgayCT<=@DenNgay AND CTCT.iThangCT>=@TuThang AND CTCT.iThangCT<=@DenThang AND CTCT.iTrangThai=1";
                SQL += " {1}";
                SQL += " GROUP BY sTenTaiSan,sKyHieuPhongBan,PhongBan.sTen";
                SQL += " HAVING SUM(rSoLuong)!=0 OR SUM(rDTKhuonVien)!=0 OR SUM(rDTLamNhaLamViec)!=0";
                SQL += " OR SUM(rDTLamCoSoHDSN)!=0 OR SUM(rDTLamNhaO)!=0 OR SUM(rDTChoThue)!=0";
                SQL += " OR SUM(rDTBoTrong)!=0 OR SUM(rDTBiLanChiem)!=0 OR SUM(rDTKhac)!=0";
                SQL = String.Format(SQL, DKMaPhongBan, DKMaLoaiTaiSan);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                if (!String.IsNullOrEmpty(DKMaPhongBan))
                {
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                }
                if (!String.IsNullOrEmpty(DKMaLoaiTaiSan))
                {
                    cmd.Parameters.AddWithValue("@iID_LoaiTaiSan", iID_MaLoaiTaiSan);
                }
                cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                cmd.Parameters.AddWithValue("@TuThang", TuThang);
                cmd.Parameters.AddWithValue("@DenThang", DenThang);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
                #endregion
            return dt;
        }
        public DataTable TenPhongBan(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_PhongBan WHERE iID_MaPhongBan=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        private void LoadData(FlexCelReport fr, String NamLamViec,String LoaiBaoCao, String iID_MaLoaiTaiSan, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaPhongBan)
        {
            if (String.IsNullOrEmpty(iID_MaPhongBan))
            {
                iID_MaPhongBan = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            DataTable data = rptKT_BaoCao9(NamLamViec, LoaiBaoCao, iID_MaLoaiTaiSan, TuNgay, DenNgay, TuThang, DenThang, iID_MaPhongBan);
            if (LoaiBaoCao == "0")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
            else if (LoaiBaoCao == "1")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
            else if(LoaiBaoCao == "2")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);

                DataTable dtPhongBan;
                dtPhongBan = HamChung.SelectDistinct("PhongBan", data, "sTen", "sTen", "");
                fr.AddTable("PhongBan", dtPhongBan);
                dtPhongBan.Dispose();
            }
            data.Dispose();
           
        }
        public clsExcelResult ExportToPDF(String NamLamViec,String LoaiBaoCao, String iID_MaLoaiTaiSan, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sFilePath = "";
            if (LoaiBaoCao == "0")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9A.xls";
            }
            else if (LoaiBaoCao == "1")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9B.xls";
            }
            else if (LoaiBaoCao == "2")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9C.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, LoaiBaoCao, iID_MaLoaiTaiSan, TuNgay, DenNgay, TuThang, DenThang, iID_MaPhongBan);
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
        public ActionResult ViewPDF(String NamLamViec,String LoaiBaoCao, String iID_MaLoaiTaiSan, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sFilePath = "";
            if (LoaiBaoCao == "0")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9A.xls";
            }
            else if (LoaiBaoCao == "1")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9B.xls";
            }
            else if (LoaiBaoCao == "2")
            {
                sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_BaoCaoTongHop9C.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, LoaiBaoCao, iID_MaLoaiTaiSan, TuNgay, DenNgay, TuThang, DenThang, iID_MaPhongBan);
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

    }
}
