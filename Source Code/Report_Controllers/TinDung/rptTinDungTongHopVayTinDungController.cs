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

namespace VIETTEL.Report_Controllers.TinDung
{
    public class rptTinDungTongHopVayTinDungController : Controller
    {
        //
        // GET: /rptTinDungTongHopVayTinDung/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TinDung/rptTinDungTongHopVayTinDung.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                FlexCelReport fr = new FlexCelReport();
                ViewData["path"] = "~/Report_Views/TinDung/rptTinDungTongHopVayTinDung.aspx";
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iNam = Convert.ToString(Request.Form[ParentID + "_iNam"]);
            String iThang_1 = Convert.ToString(Request.Form[ParentID + "_iThang_1"]);
            String iNam_1 = Convert.ToString(Request.Form[ParentID + "_iNam_1"]);
            String iThang_2 = Convert.ToString(Request.Form[ParentID + "_iThang_2"]);
            String iNam_2 = Convert.ToString(Request.Form[ParentID + "_iNam_2"]);
            String iThang_3 = Convert.ToString(Request.Form[ParentID + "_iThang_3"]);
            String iNam_3 = Convert.ToString(Request.Form[ParentID + "_iNam_3"]);
            String iThang_4 = Convert.ToString(Request.Form[ParentID + "_iThang_4"]);
            String iNam_4 = Convert.ToString(Request.Form[ParentID + "_iNam_4"]);
            String iThang_5 = Convert.ToString(Request.Form[ParentID + "_iThang_5"]);
            String iNam_5 = Convert.ToString(Request.Form[ParentID + "_iNam_5"]);
            String iThang_6 = Convert.ToString(Request.Form[ParentID + "_iThang_6"]);
            String iNam_6 = Convert.ToString(Request.Form[ParentID + "_iNam_6"]);
            String iThang_7 = Convert.ToString(Request.Form[ParentID + "_iThang_7"]);
            String iNam_7 = Convert.ToString(Request.Form[ParentID + "_iNam_7"]);
            String iThang_8 = Convert.ToString(Request.Form[ParentID + "_iThang_8"]);
            String iNam_8 = Convert.ToString(Request.Form[ParentID + "_iNam_8"]);
            String iThang_9 = Convert.ToString(Request.Form[ParentID + "_iThang_9"]);
            String iNam_9 = Convert.ToString(Request.Form[ParentID + "_iNam_9"]);
            String iThang_10 = Convert.ToString(Request.Form[ParentID + "_iThang_10"]);
            String iNam_10 = Convert.ToString(Request.Form[ParentID + "_iNam_10"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iThang"] = iThang;
            ViewData["iNam"] = iNam;
            ViewData["iThang_1"] = iThang_1;
            ViewData["iNam_1"] = iNam_1;
            ViewData["iThang_2"] = iThang_2;
            ViewData["iNam_2"] = iNam_2;
            ViewData["iThang_3"] = iThang_3;
            ViewData["iNam_3"] = iNam_3;
            ViewData["iThang_4"] = iThang_4;
            ViewData["iNam_4"] = iNam_4;
            ViewData["iThang_5"] = iThang_5;
            ViewData["iNam_5"] = iNam_5;
            ViewData["iThang_6"] = iThang_6;
            ViewData["iNam_6"] = iNam_6;
            ViewData["iThang_7"] = iThang_7;
            ViewData["iNam_7"] = iNam_7;
            ViewData["iThang_8"] = iThang_8;
            ViewData["iNam_8"] = iNam_8;
            ViewData["iThang_9"] = iThang_9;
            ViewData["iNam_9"] = iNam_9;
            ViewData["iThang_10"] = iThang_10;
            ViewData["iNam_10"] = iNam_10;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/TinDung/rptTinDungTongHopVayTinDung.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet,String iThang, String iNam,String iThang1,String iNam1,String iThang2,String iNam2,
                                                              String iThang3, String iNam3, String iThang4, String iNam4, String iThang5, String iNam5, String iThang6,
                                                              String iNam6,String iThang7,String iNam7,String iThang8,String iNam8,String iThang9,String iNam9,String iThang10,String iNam10)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String NgayThang = " ĐẾN THÁNG " + iThang + " NĂM " + iNam;
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTinDungTongHopVayTinDung");
            LoadData(fr, iID_MaTrangThaiDuyet, iThang, iNam, iThang1, iNam1, iThang2, iNam2, iThang3, iNam3, iThang4, iNam4, iThang5, iNam5, iThang6, iNam6, iThang7, iNam7, iThang8, iNam8, iThang9, iNam9, iThang10, iNam10);
            fr.SetValue("NgayThang", NgayThang);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Phong", Phong);
            fr.SetValue("Cot1", iThang1+"/"+iNam1);
            fr.SetValue("Cot2", iThang2 + "/" + iNam2);
            fr.SetValue("Cot3", iThang3 + "/" + iNam3);
            fr.SetValue("Cot4", iThang4 + "/" + iNam4);
            fr.SetValue("Cot5", iThang5 + "/" + iNam5);
            fr.SetValue("Cot6", iThang6 + "/" + iNam6);
            fr.SetValue("Cot7", iThang7 + "/" + iNam7);
            fr.SetValue("Cot8", iThang8 + "/" + iNam8);
            fr.SetValue("Cot9", iThang9 + "/" + iNam9);
            fr.SetValue("Cot10", iThang10 + "/" + iNam10);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iThang, String iNam,String iThang1,String iNam1,String iThang2,String iNam2,
                                                              String iThang3, String iNam3, String iThang4, String iNam4, String iThang5, String iNam5, String iThang6,
                                                              String iNam6,String iThang7,String iNam7,String iThang8,String iNam8,String iThang9,String iNam9,String iThang10,String iNam10)
        {
            //DataTable data = TinDungTongHopVayTinDung(iID_MaTrangThaiDuyet, iThang, iNam);
            //data.TableName = "ChiTiet";
            //fr.AddTable("ChiTiet", data);
            //// giải phóng bộ nhớ
            //data.Dispose();
            DataTable data = dtTinDungTinhHinhDauTuTinDung(iID_MaTrangThaiDuyet, iThang, iNam, iThang1, iNam1, iThang2, iNam2, iThang3, iNam3, iThang4, iNam4, iThang5, iNam5, iThang6, iNam6, iThang7, iNam7, iThang8, iNam8, iThang9, iNam9, iThang10, iNam10);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi;
            dtDonVi = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi,sTenDonVi", "");
            fr.AddTable("DonVi", dtDonVi);

            dtDonVi.Columns.Add("Cot1", typeof(Decimal));
            dtDonVi.Columns.Add("Cot2", typeof(Decimal));
            dtDonVi.Columns.Add("Cot3", typeof(Decimal));
            dtDonVi.Columns.Add("Cot4", typeof(Decimal));
            dtDonVi.Columns.Add("Cot5", typeof(Decimal));
            dtDonVi.Columns.Add("Cot6", typeof(Decimal));
            dtDonVi.Columns.Add("Cot7", typeof(Decimal));
            dtDonVi.Columns.Add("Cot8", typeof(Decimal));
            dtDonVi.Columns.Add("Cot9", typeof(Decimal));
            dtDonVi.Columns.Add("Cot10", typeof(Decimal));
            DataTable dtVonDenHan = dtVonVay(iID_MaTrangThaiDuyet, iThang, iNam, iThang1, iNam1, iThang2, iNam2, iThang3, iNam3, iThang4, iNam4, iThang5, iNam5, iThang6, iNam6, iThang7, iNam7, iThang8, iNam8, iThang9, iNam9, iThang10, iNam10);
            foreach (DataRow r1 in dtVonDenHan.Rows)
            {
                foreach (DataRow r2 in dtDonVi.Rows)
                {
                    if (r1["iID_MaDonVi"].ToString() == r2["iID_MaDonVi"].ToString())
                    {
                        r2["Cot1"] = r1["rDuVon1"];
                        r2["Cot2"] = r1["rDuVon2"];
                        r2["Cot3"] = r1["rDuVon3"];
                        r2["Cot4"] = r1["rDuVon4"];
                        r2["Cot5"] = r1["rDuVon5"];
                        r2["Cot6"] = r1["rDuVon6"];
                        r2["Cot7"] = r1["rDuVon7"];
                        r2["Cot8"] = r1["rDuVon8"];
                        r2["Cot9"] = r1["rDuVon9"];
                        r2["Cot10"] = r1["rDuVon10"];
                    }
                }
            }
            // giải phóng bộ nhớ
            data.Dispose();
            dtDonVi.Dispose();
        }
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet,String iThang, String iNam,String iThang1,String iNam1,String iThang2,String iNam2,
                                                              String iThang3, String iNam3, String iThang4, String iNam4, String iThang5, String iNam5, String iThang6,
                                                              String iNam6,String iThang7,String iNam7,String iThang8,String iNam8,String iThang9,String iNam9,String iThang10,String iNam10)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, iThang, iNam, iThang1, iNam1, iThang2, iNam2, iThang3, iNam3, iThang4, iNam4, iThang5, iNam5, iThang6, iNam6, iThang7, iNam7, iThang8, iNam8, iThang9, iNam9, iThang10, iNam10);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TinhHinhTinDung.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet,String iThang, String iNam,String iThang1,String iNam1,String iThang2,String iNam2,
                                                              String iThang3, String iNam3, String iThang4, String iNam4, String iThang5, String iNam5, String iThang6,
                                                              String iNam6,String iThang7,String iNam7,String iThang8,String iNam8,String iThang9,String iNam9,String iThang10,String iNam10)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, iThang, iNam, iThang1, iNam1, iThang2, iNam2, iThang3, iNam3, iThang4, iNam4, iThang5, iNam5, iThang6, iNam6, iThang7, iNam7, iThang8, iNam8, iThang9, iNam9, iThang10, iNam10);
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
        public static DataTable dtTinDungTinhHinhDauTuTinDung(String iID_MaTrangThaiDuyet,String iThang, String iNam,String iThang1,String iNam1,String iThang2,String iNam2,
                                                              String iThang3, String iNam3, String iThang4, String iNam4, String iThang5, String iNam5, String iThang6,
                                                              String iNam6,String iThang7,String iNam7,String iThang8,String iNam8,String iThang9,String iNam9,String iThang10,String iNam10)
        {
            //Tao dt vay
            String iThang_1 = (Convert.ToInt32(iThang) + 1).ToString();
            String iThang_2 = (Convert.ToInt32(iThang) - 1).ToString();
            String iNam_1 = iNam;
            String iNam_2 = iNam;
            DateTime DenNgay;
            DateTime ThangTruoc;
            if (iThang == "12")
            {
                iThang_1 = "1";
                iNam_1 = (Convert.ToInt32(iNam) + 1).ToString();
                DenNgay = Convert.ToDateTime(iNam_1 + "/" + iThang_1);
            }
            else
            {
                DenNgay = Convert.ToDateTime(iNam + "/" + iThang_1);
            }
            if (iThang == "1")
            {
                iThang_2 = "12";
                iNam_2 = (Convert.ToInt32(iNam) - 1).ToString();
                ThangTruoc = Convert.ToDateTime(iNam_2 + "/" + iThang_2);
            }
            else
            {
                ThangTruoc = Convert.ToDateTime(iNam + "/" + iThang_2);
            }
            DateTime TuNgay = Convert.ToDateTime(iNam + "/" + iThang);
            String SQL_Vay = String.Format(@"SELECT ROW_NUMBER() OVER (Order by iID_MaKhoanVay) as STT,iID_VayChiTiet,
                                            iID_MaKhoanVay,iID_MaDonVi,sTenDonVi,sNoiDungVay,sThuTruongDuyet,
                                            CONVERT(varchar(10),dNgayVay,105) as dNgayVay,
                                            rLaiSuat,CONVERT(varchar(10),dHanPhaiTra,105) as dHanPhaiTra,
                                            rVayTrongThang
                                            FROM VN_VayChiTiet
                                            WHERE iTrangThai=1 AND dHanPhaiTra<=@DenNgay
                                            ");
            SqlCommand cmd_Vay = new SqlCommand(SQL_Vay);
            cmd_Vay.Parameters.AddWithValue("@DenNgay", DenNgay);
            DataTable dt_Vay = Connection.GetDataTable(cmd_Vay);
            cmd_Vay.Dispose();

            //tao dt thu von trong thang
            String SQL_ThuVon = String.Format(@"
                                                SELECT iID_VayChiTiet,SUM(rThuVon) as rThuVon
													,SUM(rThuLai) as rThuLai
													,SUM(rThuVon_GiamLai) as rThuVon_GiamLai
													,SUM(rThuVon_ThangTruoc) as rThuVon_ThangTruoc
													,SUM(rThuLai_ThangTruoc) as rThuLai_ThangTruoc
													,SUM(rThuVon_GiamLai_ThangTruoc) as rThuVon_GiamLai_ThangTruoc 
                                                    FROM(
                                                SELECT iID_VayChiTiet,SUM(rThuVon) as rThuVon,SUM(rThuLai) as rThuLai
                                                        ,rThuVon_GiamLai=SUM(CASE WHEN DAY(dNgayTra)<=15 THEN rThuVon ELSE 0 END)
                                                        ,rThuVon_ThangTruoc=0,rThuLai_ThangTruoc=0,rThuVon_GiamLai_ThangTruoc=0
                                                        FROM VN_ThuVonChiTiet
                                                        WHERE iTrangThai=1 AND dNgayTra<@DenNgay AND dNgayTra>=@TuNgay
                                                        GROUP BY iID_VayChiTiet
                                                UNION
                                                SELECT iID_VayChiTiet,rThuVon=0,rThuLai=0,rThuVon_GiamLai=0,
                                                         SUM(rThuVon) as rThuVon_ThangTruoc,
                                                         SUM(rThuLai) as rThuLai_ThangTruoc,
                                                        rThuVon_GiamLai_ThangTruoc=SUM(CASE WHEN DAY(dNgayTra)<=15 THEN rThuVon ELSE 0 END)
                                                        FROM VN_ThuVonChiTiet
                                                        WHERE iTrangThai=1 AND dNgayTra<@TuNgay
                                                        GROUP BY iID_VayChiTiet
                                                  
                                                ) as a
                                                GROUP BY iID_VayChiTiet
                                                ");
            SqlCommand cmd_ThuVon = new SqlCommand(SQL_ThuVon);
            cmd_ThuVon.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd_ThuVon.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd_ThuVon.Parameters.AddWithValue("@ThangTruoc", ThangTruoc);
            DataTable dt_ThuVon = Connection.GetDataTable(cmd_ThuVon);
            cmd_ThuVon.Dispose();

            //Tao dt thu von thang nam ngay tra
            String SQL_ThuVon1 = String.Format(@"SELECT iID_VayChiTiet,ThangNgayTra,NamNgayTra,SUM(rThuVon) as rThuVon
                                                FROM (
                                                SELECT iID_VayChiTiet,ThangNgayTra=MONTH(dNgayTra),NamNgayTra=YEAR(dNgayTra),
                                                                                                         SUM(rThuVon) as rThuVon
                                                                                                        FROM VN_ThuVonChiTiet
                                                                                                        WHERE iTrangThai=1 AND dNgayTra<@TuNgay
                                                                                                        GROUP BY iID_VayChiTiet,dNgayTra
                                                                                                        ) as a
                                                GROUP BY iID_VayChiTiet,ThangNgayTra,NamNgayTra");
            SqlCommand cmd_ThuVon1 = new SqlCommand(SQL_ThuVon1);
            cmd_ThuVon1.Parameters.AddWithValue("@TuNgay", TuNgay);
            DataTable dt_ThuVon1 = Connection.GetDataTable(cmd_ThuVon1);
            cmd_ThuVon1.Dispose();

            //ghep dt thu von vao dt vay

            #region ghép dt dự toán vào dt dự án
            DataRow addR, R2;
            String sCol = "iID_VayChiTiet,rThuVon,rThuLai,rThuVon_GiamLai,rThuVon_ThangTruoc,rThuLai_ThangTruoc,rThuVon_GiamLai_ThangTruoc";
            String[] arrCol = sCol.Split(',');
            dt_Vay.Columns.Add("rThuVon", typeof(Decimal));
            dt_Vay.Columns.Add("rThuLai", typeof(Decimal));
            dt_Vay.Columns.Add("rThuVon_GiamLai", typeof(Decimal));
            dt_Vay.Columns.Add("rThuVon_ThangTruoc", typeof(Decimal));
            dt_Vay.Columns.Add("rThuLai_ThangTruoc", typeof(Decimal));
            dt_Vay.Columns.Add("rThuVon_GiamLai_ThangTruoc", typeof(Decimal));
            for (int i = 0; i < dt_ThuVon.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"iID_VayChiTiet='{0}'", dt_ThuVon.Rows[i]["iID_VayChiTiet"]);
                DataRow[] R = dt_Vay.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt_Vay.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dt_ThuVon.Rows[i][arrCol[j]];
                    }
                    dt_Vay.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dt_ThuVon.Rows)
                    {

                        for (int j = 0; j < dt_Vay.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt_Vay.Rows[j];

                            for (int c = 0; c < arrCol.Length - 6; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt_Vay.Rows[j]["rThuVon"] = R1["rThuVon"];
                                dt_Vay.Rows[j]["rThuLai"] = R1["rThuLai"];
                                dt_Vay.Rows[j]["rThuVon_GiamLai"] = R1["rThuVon_GiamLai"];
                                dt_Vay.Rows[j]["rThuVon_ThangTruoc"] = R1["rThuVon_ThangTruoc"];
                                dt_Vay.Rows[j]["rThuLai_ThangTruoc"] = R1["rThuLai_ThangTruoc"];
                                dt_Vay.Rows[j]["rThuVon_GiamLai_ThangTruoc"] = R1["rThuVon_GiamLai_ThangTruoc"];
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region //them cot rLaiPhaiTra,rDuVon,rDuLai
            dt_Vay.Columns.Add("DuVon_DauThang", typeof(Decimal));
            dt_Vay.Columns.Add("DuLai_DauThang", typeof(Decimal));
            dt_Vay.Columns.Add("VayTrongThang", typeof(Decimal));
            dt_Vay.Columns.Add("LaiPhaiTra", typeof(Decimal));
            dt_Vay.Columns.Add("ThuVon", typeof(Decimal));
            dt_Vay.Columns.Add("ThuLai", typeof(Decimal));
            dt_Vay.Columns.Add("DuVon_CuoiThang", typeof(Decimal));
            dt_Vay.Columns.Add("DuLai_CuoiThang", typeof(Decimal));
            for (int i = 0; i < dt_Vay.Rows.Count; i++)
            {
                DataRow[] dr = dt_Vay.Select("iID_VayChiTiet='" + dt_Vay.Rows[i]["iID_VayChiTiet"].ToString() + "'");

                if (String.IsNullOrEmpty(dr[0]["rThuVon"].ToString())) dr[0]["rThuVon"] = 0;
                if (String.IsNullOrEmpty(dr[0]["rThuLai"].ToString())) dr[0]["rThuLai"] = 0;
                if (String.IsNullOrEmpty(dr[0]["rThuVon_GiamLai"].ToString())) dr[0]["rThuVon_GiamLai"] = 0;
                if (String.IsNullOrEmpty(dr[0]["rThuVon_ThangTruoc"].ToString())) dr[0]["rThuVon_ThangTruoc"] = 0;
                if (String.IsNullOrEmpty(dr[0]["rThuLai_ThangTruoc"].ToString())) dr[0]["rThuLai_ThangTruoc"] = 0;
                if (String.IsNullOrEmpty(dr[0]["rThuVon_GiamLai_ThangTruoc"].ToString())) dr[0]["rThuVon_GiamLai_ThangTruoc"] = 0;
                String dNgayVay = dt_Vay.Rows[i]["dNgayVay"].ToString();
                String NgayVay = dNgayVay.Substring(0, 2);
                String ThangVay = dNgayVay.Substring(3, 2);
                String NamVay = dNgayVay.Substring(6, 4);
                //Thang dau tien vay
                if (ThangVay == iThang && NamVay == iNam)
                {
                    if (Convert.ToInt32(NgayVay) <= 15)
                    {
                        dr[0]["DuVon_DauThang"] = 0;
                        dr[0]["DuLai_DauThang"] = 0;
                        dr[0]["VayTrongThang"] = dr[0]["rVayTrongThang"];
                        dr[0]["ThuVon"] = dr[0]["rThuVon"];
                        dr[0]["ThuLai"] = dr[0]["rThuLai"];
                        dr[0]["LaiPhaiTra"] = (Convert.ToDecimal(dr[0]["VayTrongThang"]) + Convert.ToDecimal(dr[0]["DuVon_DauThang"]) - Convert.ToDecimal(dr[0]["rThuVon_GiamLai"])) * (Convert.ToDecimal(dr[0]["rLaiSuat"]) / 100);
                        dr[0]["DuVon_CuoiThang"] = Convert.ToDecimal(dr[0]["DuVon_DauThang"]) + Convert.ToDecimal(dr[0]["VayTrongThang"]) - Convert.ToDecimal(dr[0]["ThuVon"]);
                        dr[0]["DuLai_CuoiThang"] = Convert.ToDecimal(dr[0]["DuLai_DauThang"]) + Convert.ToDecimal(dr[0]["LaiPhaiTra"]) - Convert.ToDecimal(dr[0]["ThuLai"]);
                    }
                    else
                    {
                        dr[0]["DuVon_DauThang"] = 0;
                        dr[0]["DuLai_DauThang"] = 0;
                        dr[0]["VayTrongThang"] = dr[0]["rVayTrongThang"];
                        dr[0]["ThuVon"] = dr[0]["rThuVon"];
                        dr[0]["ThuLai"] = dr[0]["rThuLai"];
                        dr[0]["LaiPhaiTra"] = 0;
                        dr[0]["DuVon_CuoiThang"] = Convert.ToDecimal(dr[0]["DuVon_DauThang"]) + Convert.ToDecimal(dr[0]["VayTrongThang"]) - Convert.ToDecimal(dr[0]["ThuVon"]);
                        dr[0]["DuLai_CuoiThang"] = Convert.ToDecimal(dr[0]["DuLai_DauThang"]) + Convert.ToDecimal(dr[0]["LaiPhaiTra"]) - Convert.ToDecimal(dr[0]["ThuLai"]);
                    }
                }
                else
                {
                    Decimal TienVay_KLai = 0;
                    if (Convert.ToInt32(NgayVay) > 15)
                    {
                        TienVay_KLai = Convert.ToDecimal(dr[0]["rVayTrongThang"]);
                    }
                    int SoThang = (Convert.ToInt32(iThang) - Convert.ToInt32(ThangVay)) + (Convert.ToInt32(iNam) - Convert.ToInt32(NamVay)) * 12;
                    //so tien mien lai= so tien vay co ngay lon 15(mien thang dau vay) + Tổng số tiền trả n trước ngày 15 hàng tháng
                    Decimal SoTien_MienLai = 0;
                    SoTien_MienLai = Convert.ToDecimal(dr[0]["rThuVon_GiamLai_ThangTruoc"]) + TienVay_KLai;


                    DataRow[] dr_Von1 = dt_ThuVon1.Select("iID_VayChiTiet='" + dt_Vay.Rows[i]["iID_VayChiTiet"].ToString() + "'");
                    Decimal TongThuVon = 0;
                    if (dr_Von1.Length > 0)
                    {
                        //Tong so tien thu von den thang tinh
                        Decimal[] TongThuVonThang = new Decimal[dr_Von1.Length]; Int32[] SoThangThuVon = new Int32[dr_Von1.Length];
                        for (int j = 0; j < dr_Von1.Length; j++)
                        {
                            SoThangThuVon[j] = (Convert.ToInt32(iThang) - Convert.ToInt32(dr_Von1[j]["ThangNgayTra"])) + (Convert.ToInt32(iNam) - Convert.ToInt32(dr_Von1[j]["NamNgayTra"])) * 12 - 1;
                            TongThuVonThang[j] = Convert.ToDecimal(dr_Von1[j]["rThuVon"]) * SoThangThuVon[j];
                        }
                        for (int z = 0; z < TongThuVonThang.Length; z++)
                        {
                            TongThuVon += TongThuVonThang[z];
                        }
                    }
                    Decimal TongLaiPhaiTra = 0;
                    TongLaiPhaiTra = ((Convert.ToDecimal(dr[0]["rVayTrongThang"])) * SoThang - TongThuVon - SoTien_MienLai) * (Convert.ToDecimal(dr[0]["rLaiSuat"]) / 100);
                    dr[0]["DuVon_DauThang"] = Convert.ToDecimal(dr[0]["rVayTrongThang"]) - Convert.ToDecimal(dr[0]["rThuVon_ThangTruoc"]);
                    dr[0]["DuLai_DauThang"] = TongLaiPhaiTra - Convert.ToDecimal(dr[0]["rThuLai_ThangTruoc"]);
                    dr[0]["VayTrongThang"] = 0;
                    dr[0]["ThuVon"] = dr[0]["rThuVon"];
                    dr[0]["ThuLai"] = dr[0]["rThuLai"];
                    dr[0]["LaiPhaiTra"] = (Convert.ToDecimal(dr[0]["VayTrongThang"]) + Convert.ToDecimal(dr[0]["DuVon_DauThang"]) - Convert.ToDecimal(dr[0]["rThuVon_GiamLai"])) * (Convert.ToDecimal(dr[0]["rLaiSuat"]) / 100);
                    dr[0]["DuVon_CuoiThang"] = Convert.ToDecimal(dr[0]["DuVon_DauThang"]) + Convert.ToDecimal(dr[0]["VayTrongThang"]) - Convert.ToDecimal(dr[0]["ThuVon"]);
                    dr[0]["DuLai_CuoiThang"] = Convert.ToDecimal(dr[0]["DuLai_DauThang"]) + Convert.ToDecimal(dr[0]["LaiPhaiTra"]) - Convert.ToDecimal(dr[0]["ThuLai"]);
                }
            }
            #endregion
            return dt_Vay;
        }
    public static DataTable dtVonVay(String iID_MaTrangThaiDuyet, String iThang, String iNam,String iThang1,String iNam1,String iThang2,String iNam2,
                                                              String iThang3, String iNam3, String iThang4, String iNam4, String iThang5, String iNam5, String iThang6,
                                                              String iNam6,String iThang7,String iNam7,String iThang8,String iNam8,String iThang9,String iNam9,String iThang10,String iNam10)
        {
            DateTime TuNgay1 = Convert.ToDateTime(iNam + "/" +iThang);
            DateTime DenNgay1 = Convert.ToDateTime(iNam1 + "/" + iThang1);
            DateTime TuNgay2 = Convert.ToDateTime(iNam1 + "/" + iThang1);
            DateTime DenNgay2 = Convert.ToDateTime(iNam2 + "/" + iThang2);
            DateTime TuNgay3 = Convert.ToDateTime(iNam2 + "/" + iThang2);
            DateTime DenNgay3 = Convert.ToDateTime(iNam3 + "/" + iThang3);
            DateTime TuNgay4 = Convert.ToDateTime(iNam3 + "/" + iThang3);
            DateTime DenNgay4 = Convert.ToDateTime(iNam4 + "/" + iThang4);
            DateTime TuNgay5 = Convert.ToDateTime(iNam4 + "/" + iThang4);
            DateTime DenNgay5 = Convert.ToDateTime(iNam5 + "/" + iThang5);
            DateTime TuNgay6 = Convert.ToDateTime(iNam5 + "/" + iThang5);
            DateTime DenNgay6 = Convert.ToDateTime(iNam6 + "/" + iThang6);
            DateTime TuNgay7 = Convert.ToDateTime(iNam6 + "/" + iThang6);
            DateTime DenNgay7 = Convert.ToDateTime(iNam7 + "/" + iThang7);
            DateTime TuNgay8 = Convert.ToDateTime(iNam7 + "/" + iThang7);
            DateTime DenNgay8 = Convert.ToDateTime(iNam8 + "/" + iThang8);
            DateTime TuNgay9 = Convert.ToDateTime(iNam8 + "/" + iThang8);
            DateTime DenNgay9 = Convert.ToDateTime(iNam9 + "/" + iThang9);
            DateTime TuNgay10 = Convert.ToDateTime(iNam9 + "/" + iThang9);
            DateTime DenNgay10 = Convert.ToDateTime(iNam10 + "/" + iThang10);
            String SQL = String.Format(@"
 SELECT iID_MaDonVi,sTenDonVi,
rDuVon1=SUM(rVayTrongThang1)-ISNULL(SUM(rThuVon1),0),
rDuVon2=SUM(rVayTrongThang2)-ISNULL(SUM(rThuVon2),0),
rDuVon3=SUM(rVayTrongThang3)-ISNULL(SUM(rThuVon3),0),
rDuVon4=SUM(rVayTrongThang4)-ISNULL(SUM(rThuVon4),0),
rDuVon5=SUM(rVayTrongThang5)-ISNULL(SUM(rThuVon5),0),
rDuVon6=SUM(rVayTrongThang6)-ISNULL(SUM(rThuVon6),0),
rDuVon7=SUM(rVayTrongThang7)-ISNULL(SUM(rThuVon7),0),
rDuVon8=SUM(rVayTrongThang8)-ISNULL(SUM(rThuVon8),0),
rDuVon9=SUM(rVayTrongThang9)-ISNULL(SUM(rThuVon9),0),
rDuVon10=SUM(rVayTrongThang10)-ISNULL(SUM(rThuVon10),0)
   FROM (        
SELECT iID_VayChiTiet,iID_MaDonVi,sTenDonVi,
        rVayTrongThang1 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay1 AND dHanPhaiTra <=@DenNgay1) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang2 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay2 AND dHanPhaiTra <=@DenNgay2) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang3 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay3 AND dHanPhaiTra <=@DenNgay3) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang4 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay4 AND dHanPhaiTra <=@DenNgay4) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang5 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay5 AND dHanPhaiTra <=@DenNgay5) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang6 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay6 AND dHanPhaiTra <=@DenNgay6) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang7 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay7 AND dHanPhaiTra <=@DenNgay7) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang8 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay8 AND dHanPhaiTra <=@DenNgay8) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang9 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay9 AND dHanPhaiTra <=@DenNgay9) THEN rVayTrongThang ELSE 0 END), 
        rVayTrongThang10 =SUM(CASE WHEN (dHanPhaiTra>@TuNgay10 AND dHanPhaiTra <=@DenNgay10) THEN rVayTrongThang ELSE 0 END)
         FROM VN_VayChiTiet
         WHERE iTrangThai=1 
         GROUP BY iID_VayChiTiet,iID_MaDonVi,sTenDonVi) as a
         LEFT JOIN( SELECT iID_VayChiTiet,
        rThuVon1 =SUM(CASE WHEN (dNgayTra>@TuNgay1 AND dNgayTra <=@DenNgay1) THEN rThuVon ELSE 0 END), 
        rThuVon2 =SUM(CASE WHEN (dNgayTra>@TuNgay2 AND dNgayTra <=@DenNgay2) THEN rThuVon ELSE 0 END), 
        rThuVon3 =SUM(CASE WHEN (dNgayTra>@TuNgay3 AND dNgayTra <=@DenNgay3) THEN rThuVon ELSE 0 END), 
        rThuVon4 =SUM(CASE WHEN (dNgayTra>@TuNgay4 AND dNgayTra <=@DenNgay4) THEN rThuVon ELSE 0 END), 
        rThuVon5 =SUM(CASE WHEN (dNgayTra>@TuNgay5 AND dNgayTra <=@DenNgay5) THEN rThuVon ELSE 0 END), 
        rThuVon6 =SUM(CASE WHEN (dNgayTra>@TuNgay6 AND dNgayTra <=@DenNgay6) THEN rThuVon ELSE 0 END), 
        rThuVon7 =SUM(CASE WHEN (dNgayTra>@TuNgay7 AND dNgayTra <=@DenNgay7) THEN rThuVon ELSE 0 END), 
        rThuVon8 =SUM(CASE WHEN (dNgayTra>@TuNgay8 AND dNgayTra <=@DenNgay8) THEN rThuVon ELSE 0 END), 
        rThuVon9 =SUM(CASE WHEN (dNgayTra>@TuNgay9 AND dNgayTra <=@DenNgay9) THEN rThuVon ELSE 0 END), 
        rThuVon10 =SUM(CASE WHEN (dNgayTra>@TuNgay10 AND dNgayTra <=@DenNgay10) THEN rThuVon ELSE 0 END)
					FROM VN_ThuVonChiTiet
					GROUP BY iID_VayChiTiet) as b
         on a.iID_VayChiTiet=b.iID_VayChiTiet
         GROUP BY iID_MaDonVi,sTenDonVi
         ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@TuNgay1", TuNgay1);
            cmd.Parameters.AddWithValue("@DenNgay1", DenNgay1);
            cmd.Parameters.AddWithValue("@TuNgay2", TuNgay2);
            cmd.Parameters.AddWithValue("@DenNgay2", DenNgay2);
            cmd.Parameters.AddWithValue("@TuNgay3", TuNgay3);
            cmd.Parameters.AddWithValue("@DenNgay3", DenNgay3);
            cmd.Parameters.AddWithValue("@TuNgay4", TuNgay4);
            cmd.Parameters.AddWithValue("@DenNgay4", DenNgay4);
            cmd.Parameters.AddWithValue("@TuNgay5", TuNgay5);
            cmd.Parameters.AddWithValue("@DenNgay5", DenNgay5);
            cmd.Parameters.AddWithValue("@TuNgay6", TuNgay6);
            cmd.Parameters.AddWithValue("@DenNgay6", DenNgay6);
            cmd.Parameters.AddWithValue("@TuNgay7", TuNgay7);
            cmd.Parameters.AddWithValue("@DenNgay7", DenNgay7);
            cmd.Parameters.AddWithValue("@TuNgay8", TuNgay8);
            cmd.Parameters.AddWithValue("@DenNgay8", DenNgay8);
            cmd.Parameters.AddWithValue("@TuNgay9", TuNgay9);
            cmd.Parameters.AddWithValue("@DenNgay9", DenNgay9);
            cmd.Parameters.AddWithValue("@TuNgay10", TuNgay10);
            cmd.Parameters.AddWithValue("@DenNgay10", DenNgay10);
            DataTable dt = Connection.GetDataTable(cmd);
        return dt;
        }
    }

}
