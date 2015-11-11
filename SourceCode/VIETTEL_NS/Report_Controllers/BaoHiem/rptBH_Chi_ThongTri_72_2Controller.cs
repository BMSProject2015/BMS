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

namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_Chi_ThongTri_72_2Controller : Controller
    {
        //
        // GET: /rptBH_Chi_ThongTri_72_2/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public static int dtNull = 0;
        public ActionResult Index(String DonViTinh = "", String Kieu = "")
        {
            String sFilePath = "";
            if (DonViTinh == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau1.xls";
            }
            else if (DonViTinh == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau2.xls";
            }
            else if (DonViTinh == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            else if (DonViTinh == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_ThongTri_72_2.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }

        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {
            String UserID = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String Thang_Quy = "";
            String LoaiThangQuy = Convert.ToString(Request.Form[ParentID + "_LoaiThangQuy"]);
            if (LoaiThangQuy == "1")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String DonViTinh = Request.Form[ParentID + "_DonViTinh"];
            String Kieu = Request.Form[ParentID + "_Kieu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["DonViTinh"] = DonViTinh;
            ViewData["Kieu"] = Kieu;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["PageLoad"] = "1";
            ViewData["ChiSo"] = ChiSo;
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_ThongTri_72_2.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi, DonViTinh = DonViTinh, Kieu = Kieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        public long Tong = 0;
        public DataTable rptBH_Chi_72_2(String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String DonViTinh, String Kieu, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(LoaiThangQuy))
            {
                LoaiThangQuy = "1";
            }
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "1";
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String[] arrDonvi = iID_MaDonVi.Split(',');
            String DK = "";
            for (int i = 0; i < arrDonvi.Length; i++)
            {
                // DK += "DV.iID_MaDonVi=" + "'" + "@iID_MaDonVi" + i + "'";
                if (i > 0)
                    DK += " OR ";
                DK += "DV.iID_MaDonVi= @iID_MaDonVi" + i;

            }
            // Mảng Loại Ngân Sách

            String TrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                TrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                TrangThaiDuyet = "";
            }
            //dieu kien thang quy
            String DKThang = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DKThang = "AND iThang_Quy between 1 and 3";
                        break;
                    case "2": DKThang = "AND iThang_Quy between 4 and 6";
                        break;
                    case "3": DKThang = "AND iThang_Quy between 7 and 9";
                        break;
                    case "4": DKThang = "AND iThang_Quy between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThang = "AND iThang_Quy=@ThangQuy";
            }
            // Tổng hợp đơn vị và chi tiết
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]) : "-1";
            String iID_MaNamNganSach = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]) : "-1";
            String iID_MaNguonNganSach = dtCauHinh.Rows.Count > 0 ? Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]) : "-1";
            if (DonViTinh == "1" && Kieu == "1")
            {
                #region tổng hợp đơn vị và chi tiết
                String SQL = string.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaDonVi,sTenDonVi AS sTen,SUM(rTongSo) as SoTien
                                                FROM BH_ChungTuChiChiTiet AS DV
                                                WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND iID_MaNamNganSach = @iID_MaNamNganSach AND iID_MaNguonNganSach = @iID_MaNguonNganSach
                                                {1} AND bChi=1
                                                 {0} AND ({2})
                                                GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaDonVi,sTenDonVi
                                                HAVING SUM(rTongSo)!=0 ORDER BY iID_MaDonVi,sTenDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa", TrangThaiDuyet, DKThang, DK);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
                }
                //cmd.Parameters.AddWithValue(@"LoaiThangQuy", LoaiThangQuy);
                cmd.Parameters.AddWithValue(@"NamLamViec", NamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                //cmd.Parameters.AddWithValue(@"iID_MaDonVi", iID_MaDonVi);
                for (int i = 0; i < arrDonvi.Length; i++)
                {
                    cmd.Parameters.AddWithValue(@"iID_MaDonVi" + i.ToString(), arrDonvi[i]);

                }
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["sTNG"]) != "")
                        Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }

                cmd.Dispose();
                #endregion
            }

            if (DonViTinh == "1" && Kieu == "2")
            {
                #region Tổng hợp đơn vị và tổng hợp
                String SQL = string.Format(@"SELECT sLNS,sMoTa,SUM(rTongSo) as SoTien
                                            FROM BH_ChungTuChiChiTiet
                                            WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec AND iID_MaNamNganSach = @iID_MaNamNganSach AND iID_MaNguonNganSach = @iID_MaNguonNganSach
                                            {1} AND bChi=1
                                            AND iID_MaDonVi=@iID_MaDonVi  {0} AND sL=''
                                            GROUP BY sLNS,sMoTa
                                            HAVING SUM(rTongSo)!=0", TrangThaiDuyet, DKThang);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
                }
                //cmd.Parameters.AddWithValue(@"LoaiThangQuy", LoaiThangQuy);
                cmd.Parameters.AddWithValue(@"NamLamViec", NamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                cmd.Parameters.AddWithValue(@"iID_MaDonVi", iID_MaDonVi);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }

                cmd.Dispose();
                #endregion
            }

            if (DonViTinh == "2" && Kieu == "1")
            {
                #region Tổng hợp và chi tiết
                String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.sTen,SUM(rTongSo) as SoTien
                                            FROM BH_ChungTuChiChiTiet as BHCT
                                            INNER JOIN (SELECT * FROM NS_DonVi WHERE iNamLamViec_DonVi=@NamLamViec) as DV ON BHCT.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE bLaHangCha=0 AND BHCT.iTrangThai=1 AND iNamLamViec=@NamLamViec AND iID_MaNamNganSach = @iID_MaNamNganSach AND iID_MaNguonNganSach = @iID_MaNguonNganSach
                                            {2} AND bChi=1 AND ({0})  {1}
                                            GROUP BY DV.iID_MaDonVi,DV.sTen", DK, TrangThaiDuyet, DKThang);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
                }
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                for (int i = 0; i < arrDonvi.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonvi[i]);
                }
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }
                int a = dt.Rows.Count;
                if (a < 10 && a > 0)
                {
                    for (int i = 0; i < 11 - a; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();
                #endregion
            }
            if (DonViTinh == "2" && Kieu == "2")
            {
                #region Tổng hợp và tổng hợp
                String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.sTen,SUM(rTongSo) as SoTien
                                            FROM BH_ChungTuChiChiTiet as BHCT
                                            INNER JOIN (SELECT * FROM NS_DonVi WHERE iNamLamViec_DonVi=@NamLamViec) as DV ON BHCT.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE bLaHangCha=0 AND BHCT.iTrangThai=1 AND iNamLamViec=@NamLamViec AND iID_MaNamNganSach = @iID_MaNamNganSach AND iID_MaNguonNganSach = @iID_MaNguonNganSach {2} 
                                             AND bChi=1 AND ({0})  {1}
                                            GROUP BY DV.iID_MaDonVi,DV.sTen", DK, TrangThaiDuyet, DKThang);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
                }
                for (int i = 0; i < arrDonvi.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonvi[i]);
                }
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }
                int a = dt.Rows.Count;
                if (a < 10 && a > 0)
                {
                    for (int i = 0; i < 11 - a; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();
                #endregion
            }
            return dt;
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String DonViTinh, String Kieu, String iID_MaTrangThaiDuyet)
        {

            String LoaiThang_Quy = "";
            switch (LoaiThangQuy)
            {
                case "0":
                    LoaiThang_Quy = "Tháng";
                    break;
                case "1":
                    LoaiThang_Quy = "Quý";
                    break;
                case "2":
                    LoaiThang_Quy = "Năm";
                    break;
            }

            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
            String tendv = "";
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_Chi_ThongTri_72_2");
            LoadData(fr, NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi, DonViTinh, Kieu, iID_MaTrangThaiDuyet);
            if (dtNull == 1)
            {
                path = Server.MapPath("/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau1_Temp.xls");
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Thang_Quy", Thang_Quy);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("BieuMau", "Mẫu số C08 - D");
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
            fr.SetValue("LoaiThangQuy", LoaiThang_Quy);
            fr.SetValue("TenDV", tendv);
            fr.Run(Result);
            return Result;

        }
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        private void LoadData(FlexCelReport fr, String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String DonViTinh, String Kieu, String iID_MaTrangThaiDuyet)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            DataTable data = rptBH_Chi_72_2(NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi, DonViTinh, Kieu, iID_MaTrangThaiDuyet);
            DataTable dtDV = new DataTable();
            if (DonViTinh != "1" || Kieu!="2")
            {
                dtDV = HamChung.SelectDistinct("DV", data, "iID_MaDonVi", "iID_MaDonVi,sTen");
                dtDV.Dispose();
            }
            if (data.Rows.Count == 0)
            {
                dtNull = 1;
            }
            else
            {
                dtNull = 0;
            }
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            fr.AddTable("DV", dtDV);

            data.Dispose();

        }
        public clsExcelResult ExportToPDF(String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String DonViTinh, String Kieu, String iID_MaTrangThaiDuyet)
        {


            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (DonViTinh == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau1.xls";
            }
            else if (DonViTinh == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau2.xls";
            }
            else if (DonViTinh == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            else if (DonViTinh == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi, DonViTinh, Kieu, iID_MaTrangThaiDuyet);
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
        /// Xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String DonViTinh, String Kieu, String iID_MaTrangThaiDuyet)
        {


            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (DonViTinh == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau1.xls";
            }
            else if (DonViTinh == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau2.xls";
            }
            else if (DonViTinh == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            else if (DonViTinh == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi, DonViTinh, Kieu, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTriChiBHXH.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String DonViTinh, String Kieu, String iID_MaTrangThaiDuyet)
        {

            HamChung.Language();
            String sFilePath = "";
            if (DonViTinh == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau1.xls";
            }
            else if (DonViTinh == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau2.xls";
            }
            else if (DonViTinh == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            else if (DonViTinh == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_72_2_Mau3.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi, DonViTinh, Kieu, iID_MaTrangThaiDuyet);

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

        public static DataTable LayDSDonVi(String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKThang = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DKThang = "AND iThang_Quy between 1 and 3";
                        break;
                    case "2": DKThang = "AND iThang_Quy between 4 and 6";
                        break;
                    case "3": DKThang = "AND iThang_Quy between 7 and 9";
                        break;
                    case "4": DKThang = "AND iThang_Quy between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThang = "AND iThang_Quy=@ThangQuy";
            }
            String SQL = string.Format(@" SELECT DV.iID_MaDonVi,DV.sTen
                                        FROM BH_ChungTuChiChiTiet as BHCT
                                        INNER JOIN NS_DonVi as DV ON BHCT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE BHCT.iTrangThai=1 AND iNamLamViec=@NamLamViec {1}
                                        AND bChi=1 {0} AND rTongSo>0
                                        GROUP BY DV.iID_MaDonVi,DV.sTen", iID_MaTrangThaiDuyet, DKThang);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            if (LoaiThangQuy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
            dt = Connection.GetDataTable(cmd);

            return dt;
        }
        public JsonResult Ds_DonVi(String ParentID, String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            DataTable dt = LayDSDonVi(NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, "rptBH_Chi_ThongTri_72_2");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
            // return Json(obj_DonVi(ParentID, NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        //public String obj_DonVi(String ParentID, String NamLamViec, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        //{
        //    String input = "";
        //    DataTable dt = LayDSDonVi(NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
        //    StringBuilder stbDonVi = new StringBuilder();

        //    stbDonVi.Append("<table class=\"mGrid\">");
        //    stbDonVi.Append("<tr>");
        //    stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

        //    String TenDonVi = "", MaDonVi = "";
        //    String[] arrDonVi = iID_MaDonVi.Split(',');
        //    String _Checked = "checked=\"checked\"";
        //    for (int i = 1; i <= dt.Rows.Count; i++)
        //    {
        //        MaDonVi = Convert.ToString(dt.Rows[i - 1]["iID_MaDonVi"]);
        //        TenDonVi = Convert.ToString(dt.Rows[i - 1]["sTen"]);
        //        _Checked = "";
        //        for (int j = 1; j <= arrDonVi.Length; j++)
        //        {
        //            if (MaDonVi == arrDonVi[j - 1])
        //            {
        //                _Checked = "checked=\"checked\"";
        //                break;
        //            }
        //        }

        //        input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" />", MaDonVi, _Checked);
        //        stbDonVi.Append("<tr>");
        //        stbDonVi.Append("<td style=\"width: 15%;\">");
        //        stbDonVi.Append(input);
        //        stbDonVi.Append("</td>");
        //        stbDonVi.Append("<td>" + TenDonVi + "</td>");

        //        stbDonVi.Append("</tr>");
        //    }
        //    stbDonVi.Append("</table>");            
        //    dt.Dispose();
        //    String DonVi = stbDonVi.ToString();
        //    return DonVi;
        //}

        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }


    }
}
