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
namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKeToanTongHopNgayController : Controller
    {
        //
        // GET: /rptKeToanTongHopNgay/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKeToanTongHopNgay.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKeToanTongHopNgay.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTaiKhoan = Convert.ToString(Request.Form["iID_MaTaiKhoan"]);
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            DateTime TuNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang1 + "/" + iNgay1);
            DateTime DenNgay = Convert.ToDateTime(iNamLamViec + "/" + iThang2 + "/" + iNgay2);
            TimeSpan time = DenNgay - TuNgay;
            if (time.Days < 0)
            {
                ViewData["PageLoad"] = "0";
            }
            else
            {
                ViewData["PageLoad"] = "1";
            }
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iNgay1"] = iNgay1;
            ViewData["iThang1"] = iThang1;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iNgay2"] = iNgay2;
            ViewData["iThang2"] = iThang2;
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKeToanTongHopNgay.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable get_DanhSachTaiKhoan(String MaND)
        {
            //lấy dt danh sách tài khoản ký hiệu 68

            String SQLKyHieu = String.Format(@"SELECT sThamSo FROM KT_DanhMucThamSo WHERE sKyHieu=68");
            SqlCommand cmdKyHieu = new SqlCommand(SQLKyHieu);
            String sKyHieu = Connection.GetValueString(cmdKyHieu, "-1");
            cmdKyHieu.Dispose();
            String[] arrKyHieu = sKyHieu.Split(',');
            String DK = "";
            for (int i = 0; i < arrKyHieu.Length;i++)
            {
                DK += "iID_MaTaiKhoan=@iID_MaTaiKhoan" + i;
                if (i < arrKyHieu.Length - 1)
                    DK += " OR ";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            dtCauHinh.Dispose();
            //lay dt danh sach tai khoan
            String SQL = String.Format(@"SELECT iID_MaTaiKhoan,sTen,iID_MaTaiKhoan+'--'+sTen as TenHT
                                        FROM KT_TaiKhoan
                                        WHERE iTrangThai=1
	                                          AND iNam=@iNamLamViec AND ({0})
                                                    ",DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            for (int i = 0; i < arrKyHieu.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrKyHieu[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dt_KeToanTongHopNgay(String MaND, String iID_MaTaiKhoan, String iNgay1, String iThang1, String iNgay2, String iThang2,String iID_MaTrangThaiDuyet)
        {

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();

            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;

            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            String DKTaiKhoan_No = "",DKTaiKhoan_Co="";
            for (int i = 0; i < arrTaiKhoan.Length;i++)
            {
                DKTaiKhoan_No += "iID_MaTaiKhoan_No=@iID_MaTaiKhoan" + i;
                DKTaiKhoan_Co += "iID_MaTaiKhoan_Co=@iID_MaTaiKhoan" + i;
                if (i < arrTaiKhoan.Length - 1)
                {
                    DKTaiKhoan_No += " OR ";
                    DKTaiKhoan_Co += " OR ";
                }
            }
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            String SQL = String.Format(@"SELECT STT=0, *
                                        FROM(
                                        SELECT  iNgayCT,iThangCT,iID_MaTaiKhoan,TenTaiKhoan
                                          ,SUM(TrongKy_No) as TrongKy_No
                                          ,SUM(TrongKy_Co) as TrongKy_Co
                                          ,SUM(DauKy_No) as DauKy_No
                                          ,SUM(DauKy_Co) as DauKy_Co
                                    FROM
                                    (
                                    -- trong ky no
                                    SELECT iNgayCT,iThangCT,iID_MaTaiKhoan_No as iID_MaTaiKhoan,sTenTaiKhoan_No as TenTaiKhoan,
                                    TrongKy_No=CASE WHEN iThangCT<>0 AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) >= @TuNgay
                                                          AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgay
                                                          AND ({0})) THEN SUM(rSoTien) ELSE 0 END,
                                    TrongKy_Co=0,DauKy_No=0,DauKy_Co=0
                                    FROM KTTG_ChungTuChiTiet
                                    WHERE   iTrangThai=1 AND
                                            iThangCT<>0 AND
                                            iNamLamViec=@iNamLamViec {4} AND ({0})
                                    GROUP BY iNgayCT,iThangCT,iID_MaTaiKhoan_No,sTenTaiKhoan_No,iNamLamViec
                                    --trong ky co
                                     UNION 
                                    SELECT iNgayCT,iThangCT,iID_MaTaiKhoan_Co as iID_MaTaiKhoan,sTenTaiKhoan_Co as TenTaiKhoan,
                                    TrongKy_No=0,                   
                                    TrongKy_Co=CASE WHEN iThangCT<>0 AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)>=@TuNgay
                                                          AND  CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<=@DenNgay
                                                          AND ({1})) THEN SUM(rSoTien) ELSE 0 END
                                    ,DauKy_No=0
                                    ,DauKy_Co=0
                                    FROM KTTG_ChungTuChiTiet
                                    WHERE   iTrangThai=1 AND
                                            iThangCT<>0 AND
                                            iNamLamViec=@iNamLamViec {4} AND ({1})
                                    GROUP BY iNgayCT,iThangCT,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iNamLamViec
                                    
                                    --dau ky no
                                    UNION 
                                    SELECT iNgayCT={2},iThangCT={3},iID_MaTaiKhoan_No as iID_MaTaiKhoan,sTenTaiKhoan_No as TenTaiKhoan,TrongKy_No=0,TrongKy_Co=0,SUM(rSoTien) as DauKy_No,DauKy_Co=0
                                    FROM KTTG_ChungTuChiTiet
                                    WHERE   iTrangThai=1 AND
											CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<@TuNgay AND
                                            iThangCT<>0 AND
                                            iNamLamViec=@iNamLamViec {4} AND ({0})
                                    GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No
                                    --dau ky co
                                    UNION 
                                    SELECT iNgayCT={2},iThangCT={3},iID_MaTaiKhoan_Co as iID_MaTaiKhoan,sTenTaiKhoan_Co as TenTaiKhoan,TrongKy_No=0,TrongKy_Co=0,DauKy_No=0,SUM(rSoTien) as DauKy_Co
                                    FROM KTTG_ChungTuChiTiet
                                    WHERE   iTrangThai=1 AND
											CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111)<@TuNgay AND
                                            iThangCT<>0 AND
                                            iNamLamViec=@iNamLamViec {4} AND ({1})
                                    GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co
                                    ) as  KTTG
                                    GROUP BY iNgayCT,iThangCT,iID_MaTaiKhoan,TenTaiKhoan
                                    HAVING SUM(TrongKy_No)>0 OR SUM(TrongKy_Co)>0 OR SUM(DauKy_No)<>0 OR SUM(DauKy_Co)<>0
                                    ) as a ORDER BY iThangCT,iNgayCT,iID_MaTaiKhoan,TenTaiKhoan
                                    ", DKTaiKhoan_No,DKTaiKhoan_Co,iNgay1,iThang1,DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
                
            }
            DataTable dt = Connection.GetDataTable(cmd);

            //Lấy số dư tháng 0 chuyển từ kề toán tổng hợp
            String SQL_Thang0 = String.Format(@"SELECT *
                                        FROM(
                                        SELECT  iID_MaTaiKhoan
                                                ,SUM(DauKy_No) as DauKy_No
                                                ,SUM(DauKy_Co) as DauKy_Co
                                    FROM
                                    (
                                    --dau ky no
                                    SELECT iID_MaTaiKhoan_No as iID_MaTaiKhoan,SUM(rSoTien) as DauKy_No,DauKy_Co=0
                                    FROM KT_ChungTuChiTiet
                                    WHERE   iTrangThai=1 AND    
                                            iThangCT=0 AND
                                            iNamLamViec=@iNamLamViec {2} AND ({0})
                                    GROUP BY iID_MaTaiKhoan_No
                                    --dau ky co
                                    UNION 
                                    SELECT iID_MaTaiKhoan_Co as iID_MaTaiKhoan,DauKy_No=0,SUM(rSoTien) as DauKy_Co
                                    FROM KT_ChungTuChiTiet
                                    WHERE   iTrangThai=1 AND
											iThangCT=0 AND
                                            iNamLamViec=@iNamLamViec {2} AND ({1})
                                    GROUP BY iID_MaTaiKhoan_Co
                                    ) as  KTTG
                                    GROUP BY iID_MaTaiKhoan
                                    HAVING  SUM(DauKy_No)<>0 OR SUM(DauKy_Co)<>0
                                    ) as a ORDER BY iID_MaTaiKhoan
                                    ", DKTaiKhoan_No, DKTaiKhoan_Co, DKTrangThaiDuyet);
            SqlCommand cmd_Thang0 = new SqlCommand(SQL_Thang0);
            cmd_Thang0.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd_Thang0.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                cmd_Thang0.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
            }
            DataTable dt_Thang0 = Connection.GetDataTable(cmd_Thang0);
            if (dt_Thang0.Rows.Count > 0)
            {
                if (dt_Thang0.Rows[0]["DauKy_No"].ToString() == null || dt_Thang0.Rows[0]["DauKy_No"].ToString() == "")
                {
                    dt_Thang0.Rows[0]["DauKy_No"] = 0;
                }
                if (dt_Thang0.Rows[0]["DauKy_Co"].ToString() == null || dt_Thang0.Rows[0]["DauKy_Co"].ToString() == "")
                {
                    dt_Thang0.Rows[0]["DauKy_Co"] = 0;
                }
            }


            #region Số dư
            dt.Columns.Add("DuDauKy_No",typeof(Decimal));
            dt.Columns.Add("DuDauKy_Co", typeof(Decimal));
            dt.Columns.Add("SoDu", typeof(Decimal));
            foreach (String arr in arrTaiKhoan)
            {
                foreach (DataRow r1 in dt_Thang0.Rows)
                {
                    DataRow[] r = dt.Select("iID_MaTaiKhoan='" + arr + "'");
                    if (r.Length > 0)
                    {


                        if (r[0]["DuDauKy_No"].ToString() == null || r[0]["DuDauKy_Co"].ToString() == "")
                        {
                            r[0]["DuDauKy_No"] = 0;
                        }
                        if (r[0]["DuDauKy_Co"].ToString() == null || r[0]["DuDauKy_Co"].ToString() == "")
                        {
                            r[0]["DuDauKy_Co"] = 0;
                        }
                        if (String.Equals(r[0]["iID_MaTaiKhoan"], r1["iID_MaTaiKhoan"]))
                        {
                            r[0]["DuDauKy_No"] = (Convert.ToDecimal(r1["DauKy_No"]) + Convert.ToDecimal(r[0]["DuDauKy_No"])).ToString();
                            r[0]["DuDauKy_Co"] = (Convert.ToDecimal(r1["DauKy_Co"]) + Convert.ToDecimal(r[0]["DuDauKy_Co"])).ToString();
                            break;
                        }
                    }
                }
            }
            for (int j = 0; j < arrTaiKhoan.Length; j++)
            {
                String DK = "";
                DK = "iID_MaTaiKhoan='" + arrTaiKhoan[j] + "'";
                DataRow[] R = dt.Select(DK);
                for (int z = 0; z < R.Length; z++)
                {
                    foreach (DataRow row in R)
                    {
                        if (row["DuDauKy_No"].ToString() == null || row["DuDauKy_Co"].ToString() == "")
                        {
                            row["DuDauKy_No"] = 0;
                        }
                        if (row["DuDauKy_Co"].ToString() == null || row["DuDauKy_Co"].ToString() == "")
                        {
                            row["DuDauKy_Co"] = 0;
                        }
                    }
                    if (R[0]["DauKy_No"].ToString() == null || R[0]["DauKy_No"].ToString() == "")
                    {
                        R[0]["DauKy_No"] = 0;
                    }
                    if (R[0]["DauKy_Co"].ToString() == null || R[0]["DauKy_Co"].ToString() == "")
                    {
                        R[0]["DauKy_Co"] = 0;
                    }

                    if (Convert.ToDecimal(R[z]["DauKy_No"]) - Convert.ToDecimal(R[z]["DauKy_Co"]) >= 0)
                    {
                        if (z == 0)
                        {
                            R[z]["DuDauKy_No"] = Convert.ToDecimal(R[z]["DuDauKy_No"]) + Convert.ToDecimal(R[z]["DauKy_No"]) - Convert.ToDecimal(R[z]["DauKy_Co"]);
                            R[z]["DuDauKy_Co"] = 0;
                        }
                        else
                        {
                            R[z]["DuDauKy_No"] = Convert.ToDecimal(R[z]["DauKy_No"]) - Convert.ToDecimal(R[z]["DauKy_Co"]);
                            R[z]["DuDauKy_Co"] = 0;
                        }
                    }
                    else
                    {
                        if (z == 0)
                        {
                            R[z]["DuDauKy_No"] = 0;
                            R[z]["DuDauKy_Co"] = Convert.ToDecimal(R[z]["DuDauKy_Co"]) + Convert.ToDecimal(R[z]["DauKy_Co"]) - Convert.ToDecimal(R[z]["DauKy_No"]);
                        }
                        else
                        {
                            R[z]["DuDauKy_No"] = 0;
                            R[z]["DuDauKy_Co"] = Convert.ToDecimal(R[z]["DauKy_Co"]) - Convert.ToDecimal(R[z]["DauKy_No"]);
                        }
                    }
                    if (Convert.ToString(R[z]["DuDauKy_No"]) != "")
                    {
                        if (z > 0)
                        {
                            if (Convert.ToDecimal(R[0]["DuDauKy_No"]) >= 0 && Convert.ToDecimal(R[0]["DuDauKy_Co"]) <= 0)
                                R[z]["SoDu"] = Convert.ToDecimal(R[z - 1]["SoDu"]) + (Convert.ToDecimal(R[z]["TrongKy_No"]) - Convert.ToDecimal(R[z]["TrongKy_Co"]));
                            else
                                R[z]["SoDu"] = Convert.ToDecimal(R[z - 1]["SoDu"]) + (Convert.ToDecimal(R[z]["TrongKy_Co"]) - Convert.ToDecimal(R[z]["TrongKy_No"]));
                        }
                        else
                        {
                            if (Convert.ToDecimal(R[0]["DuDauKy_No"]) >= 0 && Convert.ToDecimal(R[0]["DuDauKy_Co"]) <= 0)
                                R[0]["SoDu"] = Convert.ToDecimal(R[0]["DuDauKy_No"]) + (Convert.ToDecimal(R[0]["TrongKy_No"]) - Convert.ToDecimal(R[0]["TrongKy_Co"]));
                            else
                                R[0]["SoDu"] = Convert.ToDecimal(R[0]["DuDauKy_Co"]) + (Convert.ToDecimal(R[0]["TrongKy_Co"]) - Convert.ToDecimal(R[0]["TrongKy_No"]));
                        }
                    }
                }
            }
            #endregion
            int tg = 1;
            foreach (DataRow dr in dt.Rows)
            {
                dr["STT"] = tg;
                tg++;
            }
            cmd.Dispose();
            return dt;
        }
        public ActionResult ViewPDF(String MaND, String iID_MaTaiKhoan, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTaiKhoan, iNgay1, iThang1, iNgay2,iThang2,iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTaiKhoan, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTaiKhoan, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BangKeTrichThue_TNCN.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTaiKhoan, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String Ngay = "Từ ngày "+iNgay1+"/"+iThang1+" đến ngày "+iNgay2+"/"+iThang2;
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_BangKeRutDuToan");
            LoadData(fr, MaND, iID_MaTaiKhoan, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTaiKhoan, String iNgay1, String iThang1, String iNgay2, String iThang2, String iID_MaTrangThaiDuyet)
        {
            DataTable data = dt_KeToanTongHopNgay(MaND, iID_MaTaiKhoan, iNgay1, iThang1, iNgay2, iThang2, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtNgayThang = HamChung.SelectDistinct("NgayThang", data, "iThangCT,iNgayCT", "iNgayCT,iThangCT");
            fr.AddTable("NgayThang", dtNgayThang);
            dtNgayThang.Dispose();
            data.Dispose();
        }
        public class RutDuToan
        {
            public String Ngay1 { get; set; }
            public String Ngay2 { get; set; }
        }
        public JsonResult Get_objNgayThang(String ParentID,String iNamLamViec, String iNgay1, String iThang1, String iNgay2, String iThang2)
        {
            return Json(get_sNgayThang(ParentID,iNamLamViec, iNgay1, iThang1, iNgay2, iThang2), JsonRequestBehavior.AllowGet);
        }
        public RutDuToan get_sNgayThang(String ParentID, String iNamLamViec, String iNgay1, String iThang1, String iNgay2, String iThang2)
        {
            RutDuToan a = new RutDuToan();
            DataTable dtNgay1 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang1), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay1 = new SelectOptionList(dtNgay1, "MaNgay", "TenNgay");
            a.Ngay1 = MyHtmlHelper.DropDownList(ParentID, slNgay1, iNgay1, "iNgay1", "", "style=\"width:55px;padding:2px;border:1px solid #dedede;\" onchange=\"ChonThang()\"");
            dtNgay1.Dispose();
            int SoNgayTrongThang = DateTime.DaysInMonth(Convert.ToInt16(iNamLamViec), Convert.ToInt16(iThang1));
            if (String.IsNullOrEmpty(iNgay1) == false)
            {
                if (Convert.ToInt16(iNgay1) > SoNgayTrongThang)
                    iNgay1 = "1";
            }
            DataTable dtNgay2 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang2), Convert.ToInt16(iNamLamViec), false);
            SelectOptionList slNgay2 = new SelectOptionList(dtNgay2, "MaNgay", "TenNgay");
            a.Ngay2 = MyHtmlHelper.DropDownList(ParentID, slNgay2, iNgay2, "iNgay2", "", "style=\"width:60px; padding:2px;border:1px solid #dedede;\" onchange=\"ChonThang()\"");
            return a;
        }
    }
}
