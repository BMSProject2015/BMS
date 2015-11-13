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
namespace VIETTEL.Report_Controllers.KeToan
{
    public class rptKeToan_SoCaiTaiKhoanController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToan_SoCaiTaiKhoan.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_SoCaiTaiKhoan.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }
        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_NamLamViec"]);
            String TuThang = Convert.ToString(Request.Form[ParentID + "_TuThang"]);
            String DenThang = Convert.ToString(Request.Form[ParentID + "_DenThang"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_TrangThai"]);
            ViewData["PageLoad"] = "1";
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["TuThang"] = TuThang;
            ViewData["DenThang"] = DenThang;
            ViewData["TrangThai"] = TrangThai;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToan_SoCaiTaiKhoan.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay });
        }
        public ExcelFile CreateReport(String path, String NamLamViec, String TuThang, String DenThang, String TrangThai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = "";
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToan_SoCaiTaiKhoan");
            LoadData(fr, NamLamViec, TuThang, DenThang, TrangThai);

            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String ThangNam = "Từ tháng " + TuThang + " đến tháng " + DenThang + " Năm " + NamLamViec;
            if (TuThang == DenThang)
                ThangNam = "Tháng " + TuThang + " Năm " + NamLamViec;

            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("ThangNam", ThangNam);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("Ngay", DateTime.Now.Day);
            fr.SetValue("Thang", DateTime.Now.Month);
            fr.SetValue("Nam", DateTime.Now.Year);

            fr.Run(Result);
            return Result;
        }
        public clsExcelResult ExportToExcel(String NamLamViec, String TuThang, String DenThang, String TrangThai)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, TuThang, DenThang, TrangThai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "KeToan_SoCaiTaiKhoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public ActionResult ViewPDF(String NamLamViec, String TuThang, String DenThang, String TrangThai)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), NamLamViec, TuThang, DenThang, TrangThai);
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
        private void LoadData(FlexCelReport fr, String NamLamViec, String TuThang, String DenThang, String TrangThai)
        {

            DataTable data = KeToan_SoCaiTaiKhoan(NamLamViec, TuThang, DenThang, TrangThai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            double TrongKy_No = 0, DenKy_No = 0, TrongKy_Co = 0, DenKy_Co = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow dr = data.Rows[i];
                if (Convert.ToString( dr["bLoai"])=="2")
                {
                    TrongKy_No += HamChung.ConvertToDouble(dr["TrongKy_No"]);
                    DenKy_No += HamChung.ConvertToDouble(dr["DenKy_No"]);
                    TrongKy_Co += HamChung.ConvertToDouble(dr["TrongKy_Co"]);
                    DenKy_Co += HamChung.ConvertToDouble(dr["DenKy_Co"]);
                }
            }
            fr.SetValue("TrongKy_No", TrongKy_No);
            fr.SetValue("DenKy_No", DenKy_No);
            fr.SetValue("TrongKy_Co", TrongKy_Co);
            fr.SetValue("DenKy_Co", DenKy_Co);
            if (data!=null)
            {
                data.Dispose();
            }
        }
        public DataTable KeToan_SoCaiTaiKhoan(String NamLamViec, String TuThang, String DenThang, String TrangThai)
        {
            String SQL = "";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            if (TrangThai != "0")
            {
                DK=" AND iID_MaTrangThaiDuyet='"+LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop)+"'";
            }
            //tao dt ben no
            SQL = String.Format(@"SELECT iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co as DoiUngNo
,TrongKy_No=SUM(CASE WHEN iThangCT>=@TuThang AND iThangCT<=@DenThang THEN rSoTien ELSE 0 END)
,DenKy_No=SUM(CASE WHEN iThangCT<=@DenThang AND iThangCT<>0 THEN rSoTien ELSE 0 END)
 FROM KT_ChungTuChiTiet
 WHERE iNamLamViec=@iNamLamViec {0} AND iTrangThai=1 AND iID_MaTaiKhoan_No<>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>'0' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>'0'
 GROUP BY iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co
 HAVING SUM(CASE WHEN iThangCT<=@DenThang AND iThangCT<>0 THEN rSoTien ELSE 0 END) <>0
  ORDER BY iID_MaTaiKhoan_No",DK);
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            DataTable dtNo = Connection.GetDataTable(cmd);
            //tao dt ben co
            SQL = String.Format(@"SELECT iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No as DoiUngCo
,TrongKy_Co=SUM(CASE WHEN iThangCT>=@TuThang AND iThangCT<=@DenThang THEN rSoTien ELSE 0 END)
,DenKy_Co=SUM(CASE WHEN iThangCT<=@DenThang AND iThangCT<>0 THEN rSoTien ELSE 0 END)
 FROM KT_ChungTuChiTiet
 WHERE iNamLamViec=@iNamLamViec {0} AND iTrangThai=1  AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>'0' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>'0' AND iID_MaTaiKhoan_Co<>''
 GROUP BY iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co
 HAVING SUM(CASE WHEN iThangCT<=@DenThang AND iThangCT<>0 THEN rSoTien ELSE 0 END) <>0
  ORDER BY iID_MaTaiKhoan_Co",DK);
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            DataTable dtCo = Connection.GetDataTable(cmd);
            //dtdauky+congphatsinh_cuoiky_ben no
            DataTable dtNoTong = dtSoCaiTaiKhoan_No(NamLamViec, TuThang, DenThang,TrangThai);
            //dtdauky+congphatsinh_cuoiky_ben Co
            DataTable dtCoTong = dtSoCaiTaiKhoan_Co(NamLamViec, TuThang, DenThang, TrangThai);
            //dtTaiKhoan
            SQL = String.Format(@" SELECT a.iID_MaTaiKhoan,sTen FROM (SELECT DISTINCT iID_MaTaiKhoan_No as iID_MaTaiKhoan
  FROM KT_ChungTuChiTiet
  WHERE iNamLamViec=@iNamLamViec AND iThangCT<=@DenThang AND iTrangThai=1  AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>'0' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>'0'
   AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>0
   AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>0
  UNION 
  SELECT DISTINCT iID_MaTaiKhoan_Co
  FROM KT_ChungTuChiTiet
  WHERE iNamLamViec=@iNamLamViec   AND iThangCT<=@DenThang AND iTrangThai=1  AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>'0' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>'0'
    AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>0
   AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>0 ) as a
   INNER JOIN (SELECT iID_MaTaiKhoan,sTen FROM KT_TaiKhoan WHERE iNam=2012) as  b
   ON a.iID_MaTaiKhoan=b.iID_MaTaiKhoan");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            DataTable dtTaiKhoan = Connection.GetDataTable(cmd);
            dt.Columns.Add("iID_MaTaiKhoan", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            dt.Columns.Add("SoDu", typeof(String));
            dt.Columns.Add("bLoai", typeof(String));
            dt.Columns.Add("DoiUngNo", typeof(String));
            dt.Columns.Add("TrongKy_No", typeof(Decimal));
            dt.Columns.Add("DenKy_No", typeof(Decimal));
            dt.Columns.Add("DoiUngCo", typeof(String));
            dt.Columns.Add("TrongKy_Co", typeof(Decimal));
            dt.Columns.Add("DenKy_Co", typeof(Decimal));

            foreach (DataRow dr in dtTaiKhoan.Rows)
            {
                DataRow[] rowNo = dtNo.Select("iID_MaTaiKhoan_No='" + dr["iID_MaTaiKhoan"] + "'");
                DataRow[] rowNoTong = dtNoTong.Select("iID_MaTaiKhoan_No='" + dr["iID_MaTaiKhoan"] + "'");
                DataRow[] rowCo = dtCo.Select("iID_MaTaiKhoan_Co='" + dr["iID_MaTaiKhoan"] + "'");
                DataRow[] rowCoTong = dtCoTong.Select("iID_MaTaiKhoan_Co='" + dr["iID_MaTaiKhoan"] + "'");
                //Them dong du dauky vao
                DataRow rDuDauKy = dt.NewRow();
                rDuDauKy["iID_MaTaiKhoan"] = Convert.ToString(dr["iID_MaTaiKhoan"]);
                Decimal rDauKyCo = 0, rDauKyNo = 0, rCuoiKyCo = 0, rCuoiKyNo = 0, rTrongKy_No = 0, rLuyKe_No = 0, rTrongKy_Co = 0, rLuyKe_Co = 0;
                if (rowNoTong.Length > 0)
                {
                    if (Convert.ToString(rowNoTong[0]["DauKy_No"]) != "")
                    {
                        rDauKyNo = Convert.ToDecimal(rowNoTong[0]["DauKy_No"]);
                    }
                    if (Convert.ToString(rowNoTong[0]["PhatSinh_No"]) != "")
                    {
                        rTrongKy_No = Convert.ToDecimal(rowNoTong[0]["PhatSinh_No"]);
                    }
                    if (Convert.ToString(rowNoTong[0]["LuyKe_No"]) != "")
                    {
                        rLuyKe_No = Convert.ToDecimal(rowNoTong[0]["LuyKe_No"]);
                    }
                    if (Convert.ToString(rowNoTong[0]["CuoiKy_No"]) != "")
                    {
                        rCuoiKyNo = Convert.ToDecimal(rowNoTong[0]["CuoiKy_No"]);
                    }
                }
                if (rowCoTong.Length > 0)
                {
                    if (Convert.ToString(rowCoTong[0]["DauKy_Co"]) != "")
                    {
                        rDauKyCo = Convert.ToDecimal(rowCoTong[0]["DauKy_Co"]);
                    }
                    if (Convert.ToString(rowCoTong[0]["PhatSinh_Co"]) != "")
                    {
                        rTrongKy_Co = Convert.ToDecimal(rowCoTong[0]["PhatSinh_Co"]);
                    }
                    if (Convert.ToString(rowCoTong[0]["LuyKe_Co"]) != "")
                    {
                        rLuyKe_Co = Convert.ToDecimal(rowCoTong[0]["LuyKe_Co"]);
                    }
                    if (Convert.ToString(rowCoTong[0]["CuoiKy_Co"]) != "")
                    {
                        rCuoiKyCo = Convert.ToDecimal(rowCoTong[0]["CuoiKy_Co"]);
                    }
                }
                if (rDauKyNo - rDauKyCo >= 0)
                {
                    rDuDauKy["DenKy_No"] = rDauKyNo - rDauKyCo;
                }
                else
                    rDuDauKy["DenKy_Co"] = (rDauKyNo - rDauKyCo) * -1;
                rDuDauKy["SoDu"] = "Số dư đầu kỳ:";
                rDuDauKy["bLoai"] = "1";
                rDuDauKy["sTen"] = Convert.ToString(dr["sTen"]); ;
                dt.Rows.Add(rDuDauKy);

                //Neu so dong no lon hon so dong co
                if (rowNo.Length > rowCo.Length)
                {
                    for (int i = 0; i < rowNo.Length; i++)
                    {
                        DataRow r = dt.NewRow();
                        //  r["iID_MaTaiKhoan"] = Convert.ToString(rowNo[i]["iID_MaTaiKhoan_No"]);
                        r["DoiUngNo"] = "   " + Convert.ToString(rowNo[i]["DoiUngNo"]);
                        r["TrongKy_No"] = Convert.ToString(rowNo[i]["TrongKy_No"]);
                        r["DenKy_No"] = Convert.ToString(rowNo[i]["DenKy_No"]);
                        if (i < rowCo.Length)
                        {
                            r["DoiUngCo"] = "   " + Convert.ToString(rowCo[i]["DoiUngCo"]);
                            r["TrongKy_Co"] = Convert.ToString(rowCo[i]["TrongKy_Co"]);
                            r["DenKy_Co"] = Convert.ToString(rowCo[i]["DenKy_Co"]);
                        }
                        dt.Rows.Add(r);
                    }
                }
                else
                {
                    for (int i = 0; i < rowCo.Length; i++)
                    {
                        DataRow r = dt.NewRow();
                        //   r["iID_MaTaiKhoan"] = Convert.ToString(rowCo[i]["iID_MaTaiKhoan_Co"]);
                        r["DoiUngCo"] = "   " + Convert.ToString(rowCo[i]["DoiUngCo"]);
                        r["TrongKy_Co"] = Convert.ToString(rowCo[i]["TrongKy_Co"]);
                        r["DenKy_Co"] = Convert.ToString(rowCo[i]["DenKy_Co"]);
                        if (i < rowNo.Length)
                        {
                            r["DoiUngNo"] = "   " + Convert.ToString(rowNo[i]["DoiUngNo"]);
                            r["TrongKy_No"] = Convert.ToString(rowNo[i]["TrongKy_No"]);
                            r["DenKy_No"] = Convert.ToString(rowNo[i]["DenKy_No"]);
                        }
                        dt.Rows.Add(r);
                    }
                }
                //Thêm dòng ++
                DataRow drCongPhatSinh = dt.NewRow();
                //drCongPhatSinh["iID_MaTaiKhoan"] = Convert.ToString(dr["iID_MaTaiKhoan"]);
                drCongPhatSinh["TrongKy_No"] = rTrongKy_No;
                drCongPhatSinh["DenKy_No"] = rLuyKe_No;
                drCongPhatSinh["TrongKy_Co"] = rTrongKy_Co;
                drCongPhatSinh["DenKy_Co"] = rLuyKe_Co;
                drCongPhatSinh["SoDu"] = "++";
                drCongPhatSinh["bLoai"] = "2";
                dt.Rows.Add(drCongPhatSinh);
                //Thêm dòng So du cuoi ky
                DataRow drDuCuoiKy = dt.NewRow();
                // drDuCuoiKy["iID_MaTaiKhoan"] = Convert.ToString(dr["iID_MaTaiKhoan"]);
                if (rCuoiKyNo - rCuoiKyCo >= 0)
                {
                    drDuCuoiKy["DenKy_No"] = rCuoiKyNo - rCuoiKyCo;
                }
                else
                    drDuCuoiKy["DenKy_Co"] = (rCuoiKyNo - rCuoiKyCo) * -1;
                drDuCuoiKy["SoDu"] = "Số dư cuối kỳ:";
                drDuCuoiKy["bLoai"] = "3";
                dt.Rows.Add(drDuCuoiKy);
            }

            return dt;
        }
        public DataTable dtSoCaiTaiKhoan_No(String NamLamViec, String TuThang, String DenThang,String TrangThai)
        {
            DataTable dt = new DataTable();
            String DK = "";
            if (TrangThai != "0")
            {
                DK = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            String SQL = String.Format(@"SELECT iID_MaTaiKhoan_No
,DauKy_No=SUM(CASE WHEN iThangCT<@TuThang THEN rSoTien ELSE 0 END)
,PhatSinh_No=SUM(CASE WHEN iThangCT>=@TuThang AND iThangCT<=@DenThang THEN rSoTien ELSE 0 END)
,LuyKe_No=SUM(CASE WHEN iThangCT<=@DenThang AND iThangCT<>0 THEN rSoTien ELSE 0 END)
,CuoiKy_No=SUM(CASE WHEN iThangCT<=@DenThang  THEN rSoTien ELSE 0 END)
 FROM KT_ChungTuChiTiet
 WHERE iNamLamViec=@iNamLamViec {0} AND iTrangThai=1 AND iID_MaTaiKhoan_No<>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>'0' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>'0'
 GROUP BY iID_MaTaiKhoan_No
 HAVING SUM(CASE WHEN iThangCT<=@DenThang  THEN rSoTien ELSE 0 END) <>0
  ORDER BY iID_MaTaiKhoan_No",DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            dt = Connection.GetDataTable(cmd);
            return dt; ;
        }
        public DataTable dtSoCaiTaiKhoan_Co(String NamLamViec, String TuThang, String DenThang,String TrangThai)
        {
            DataTable dt = new DataTable();
            String DK = "";
            if (TrangThai != "0")
            {
                DK = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            String SQL = String.Format(@"SELECT iID_MaTaiKhoan_Co
,DauKy_Co=SUM(CASE WHEN iThangCT<@TuThang THEN rSoTien ELSE 0 END)
,PhatSinh_Co=SUM(CASE WHEN iThangCT>=@TuThang AND iThangCT<=@DenThang THEN rSoTien ELSE 0 END)
,LuyKe_Co=SUM(CASE WHEN iThangCT<=@DenThang AND iThangCT<>0 THEN rSoTien ELSE 0 END)
,CuoiKy_Co=SUM(CASE WHEN iThangCT<=@DenThang  THEN rSoTien ELSE 0 END)
 FROM KT_ChungTuChiTiet
 WHERE iNamLamViec=@inamLamViec {0} AND iTrangThai=1 AND iID_MaTaiKhoan_Co<>'' AND SUBSTRING(iID_MaTaiKhoan_No,1,1)<>'0' AND SUBSTRING(iID_MaTaiKhoan_Co,1,1)<>'0'
 GROUP BY iID_MaTaiKhoan_Co
 HAVING SUM(CASE WHEN iThangCT<=@DenThang  THEN rSoTien ELSE 0 END) <>0
  ORDER BY iID_MaTaiKhoan_Co",DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            dt = Connection.GetDataTable(cmd);
            return dt; ;
        }
    }
}
