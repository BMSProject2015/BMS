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
    public class rptTinDungQuaHanTrongThangController : Controller
    {
        //
        // GET: /rptTinDungTinhHinhDauTuTinDung/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TinDung/rptTinDungQuaHan.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                FlexCelReport fr = new FlexCelReport();
                ViewData["path"] = "~/Report_Views/TinDung/rptTinDungQuaHanTrongThang.aspx";
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
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iThang"] = iThang;
            ViewData["iNam"] = iNam;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/TinDung/rptTinDungQuaHanTrongThang.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iThang, String iNam)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String NgayThang = " Quá hạn tháng " + iThang + " năm " + iNam;
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTinDungQuaHanTrongThang");
            LoadData(fr, iID_MaTrangThaiDuyet, iThang, iNam);
            fr.SetValue("NgayThang", NgayThang);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Phong", Phong);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iThang, String iNam)
        {
            DataTable data = dtTinDungTinhHinhDauTuTinDung(iID_MaTrangThaiDuyet, iThang, iNam);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi;
            dtDonVi = HamChung.SelectDistinct("DonVi", data, "iID_MaKhoanVay,iID_MaDonVi", "iID_MaKhoanVay,iID_MaDonVi,sTenDonVi", "");
            fr.AddTable("DonVi", dtDonVi);

            DataTable dtDauMoi;
            dtDauMoi = HamChung.SelectDistinct("DauMoi", dtDonVi, "iID_MaKhoanVay", "iID_MaKhoanVay", "");
            fr.AddTable("DauMoi", dtDauMoi);

            // giải phóng bộ nhớ
            data.Dispose();
            dtDonVi.Dispose();
            dtDauMoi.Dispose();

        }
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iThang, String iNam)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, iThang, iNam);

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

        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iThang, String iNam)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, iThang, iNam);
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
        /// dtTinDungTinhHinhDauTuTinDung
        /// </summary>
        /// <param name="iThang"></param>
        /// <param name="iNam"></param>
        /// <returns></returns>
        public static DataTable dtTinDungTinhHinhDauTuTinDung(String iID_MaTrangThaiDuyet, String iThang, String iNam)
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
                                            WHERE iTrangThai=1 AND dHanPhaiTra=@DenNgay
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
                String NgayVay = "0";
                String ThangVay = "0";
                String NamVay = "0";
                if (!String.IsNullOrEmpty(dNgayVay))
                {
                    NgayVay = dNgayVay.Substring(0, 2);
                    ThangVay = dNgayVay.Substring(3, 2);
                    NamVay = dNgayVay.Substring(6, 4);
                }
                else
                    {
                        dr[0]["rLaiSuat"] = 0;
                        dr[0]["rVayTrongThang"] = 0;
                    }

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
                    if (String.IsNullOrEmpty(dNgayVay))
                    {
                        dr[0]["DuVon_DauThang"] = 0;
                        dr[0]["DuLai_DauThang"] = 0;
                    }
                }
            }
            #endregion
            return dt_Vay;
        }
    }
}
