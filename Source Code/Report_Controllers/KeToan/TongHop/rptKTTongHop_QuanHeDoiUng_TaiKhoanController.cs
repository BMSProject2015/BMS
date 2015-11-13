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
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTongHop_QuanHeDoiUng_TaiKhoanController : Controller
    {
        // Create: Nguyễn Huyền Lê
        // Date create:
        // Edit: Lê Văn Thương
        // Date edit: 15/09/2012
        // GET: /rptKTTongHop_QuanHeDoiUng_TaiKhoan/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKTTongHop_QuanHeDoiUng_TaiKhoan.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["pageload"] = "0";
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_QuanHeDoiUng_TaiKhoan.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["pageload"] = "1";
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String CapTK = Convert.ToString(Request.Form[ParentID + "_divCap"]);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTongHop_QuanHeDoiUng_TaiKhoan.aspx";
            ViewData["iThang1"] = iThang1;
            ViewData["iThang2"] = iThang2;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iNgay1"] = iNgay1;
            ViewData["iNgay2"] = iNgay2;
            ViewData["CapTK"] = CapTK;
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file Excel mẫu</param>
        /// <param name="iNamLamViec">Năm</param>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>
        /// <param name="iThang1">Từ tháng</param>
        /// <param name="iThang2">Đến tháng</param>
        /// <param name="iNgay1">Từ ngày</param>
        /// <param name="iNgay2">Đến ngày</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtTK = dtTenTaiKhoan(iID_MaTaiKhoan);
            String TenTK = dtTK.Rows.Count > 0 ? dtTK.Rows[0][0].ToString() : "";
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTongHop_QuanHeDoiUng_TaiKhoan");
            LoadData(fr, iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2);
            fr.SetValue("TenTaiKhoan", iID_MaTaiKhoan + "-" + TenTK);
            fr.SetValue("Ma", iID_MaTaiKhoan);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang1", iThang1);
            fr.SetValue("Thang2", iThang2);
            fr.SetValue("Ngay1", iNgay1);
            fr.SetValue("Ngay2", iNgay2);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("ngay", ngay);
            fr.Run(Result);
            return Result;
        }
        /// <summary>
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="iNamLamViec">Năm</param>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>
        /// <param name="iThang1">Từ tháng</param>
        /// <param name="iThang2">Đến tháng</param>
        /// <param name="iNgay1">Từ ngày</param>
        /// <param name="iNgay2">Đến ngày</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuanHeDoiUng_TaiKhoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="iNamLamViec">Năm</param>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>
        /// <param name="iThang1">Từ tháng</param>
        /// <param name="iThang2">Đến tháng</param>
        /// <param name="iNgay1">Từ ngày</param>
        /// <param name="iNgay2">Đến ngày</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2);
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
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="iNamLamViec">Năm</param>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>
        /// <param name="iThang1">Từ tháng</param>
        /// <param name="iThang2">Đến tháng</param>
        /// <param name="iNgay1">Từ ngày</param>
        /// <param name="iNgay2">Đến ngày</param>
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2)
        {
            DataTable data = TongHopTaiKhoan(iNamLamViec, iID_MaTaiKhoan, iThang1, iThang2, iNgay1, iNgay2);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable data2 = TinhCuoiKy(iNamLamViec, iID_MaTaiKhoan, iThang2, iNgay2);
            data.TableName = "TinhCuoiKy";
            fr.AddTable("TinhCuoiKy", data2);
            data2.Dispose();
        }
        /// <summary>
        /// Lấy thông tin quan hệ đối ứng
        /// </summary>
        /// <param name="iNamLamViec">Năm</param>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>
        /// <param name="iThang1">Từ tháng</param>
        /// <param name="iThang2">Đến tháng</param>
        /// <param name="iNgay1">Từ ngày</param>
        /// <param name="iNgay2">Đến ngày</param>
        /// <returns>Danh sách</returns>
        public static DataTable TongHopTaiKhoan(String iNamLamViec, String iID_MaTaiKhoan, String iThang1, String iThang2, String iNgay1, String iNgay2)
        {
            DataTable dt = new DataTable();
            String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String SQL = String.Format(@"  SELECT DoiChieu,DoiUng
                                     ,sum(rSoPhatSinhNo) AS rSoPhatSinhNo 
                                     ,sum(rSoPhatSinhCo) AS rSoPhatSinhCo
                                     FROM(
                                        SELECT C.sSoChungTuChiTiet
                                         ,substring(C.iID_MaTaiKhoan_No,1,{0})AS DoiChieu,substring(C.iID_MaTaiKhoan_Co,1,3) AS DoiUng
                                         ,sum(C.rSoTien) AS rSoPhatSinhNo,0 AS rSoPhatSinhCo
                                         FROM KT_ChungTuChiTiet as C
                                         WHERE  C.iTrangThai = 1 AND C.iNamLamViec=@iNamLamViec
                                         AND (CONVERT(Datetime, CONVERT(varchar, C.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, C.iNgayCT), 111)  BETWEEN @TuNgay AND @DenNgay)     
                                         AND  C.iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%' AND C.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                         group by C.sSoChungTuChiTiet,C.iID_MaTaiKhoan_No,C.iID_MaTaiKhoan_Co
                                         UNION 
                                         SELECT  C.sSoChungTuChiTiet
                                         ,substring(C.iID_MaTaiKhoan_Co,1,{0}) AS DoiChieu
                                         ,substring(C.iID_MaTaiKhoan_No,1,3) AS DoiUng,0 AS rSoPhatSinhNo
                                         ,sum(C.rSoTien) AS rSoPhatSinhCo
                                         FROM KT_ChungTuChiTiet as C 
                                         WHERE C.iTrangThai = 1 AND C.iNamLamViec=@iNamLamViec
                                         AND (CONVERT(Datetime, CONVERT(varchar, C.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, C.iNgayCT), 111) BETWEEN @TuNgay AND @DenNgay)    
                                         AND  C.iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%' AND C.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                          group by C.sSoChungTuChiTiet,C.iID_MaTaiKhoan_No,C.iID_MaTaiKhoan_Co) TB
                                    group by DoiChieu,DoiUng ", iID_MaTaiKhoan.Length);
    
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Tính số dư cuối kỳ
        /// </summary>
        /// <param name="iNamLamViec">Năm</param>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>        
        /// <param name="iThang2">Đến tháng</param>        
        /// <param name="iNgay2">Đến ngày</param>
        /// <returns></returns>
        public static DataTable TinhCuoiKy(String iNamLamViec, String iID_MaTaiKhoan, String iThang2, String iNgay2)
        {
            DataTable dt = new DataTable();
            String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String SQL = String.Format(@" SELECT SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo
                                            FROM(
                                            SELECT KTCT.iThang,KTCT.iNgay,KTCT.iNgayCT,KTCT.iThangCT,sSoChungTuChiTiet as SoChungTu
                                            ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            FROM KT_ChungTuChiTiet KTCT
                                            INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu
                                            WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@iNamLamViec AND KTCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                            AND KTCT.iThangCT<>0
                                             AND (CONVERT(Datetime, CONVERT(varchar, KTCT.iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, KTCT.iNgayCT), 111) <= @DenNgay)
                                            GROUP BY KTCT.iThang,KTCT.iNgay,KTCT.iNgayCT,KTCT.iThangCT,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No,sSoChungTuChiTiet
                                            )as BANGTEM
                                            HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }
        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        /// <param name="NamChungTu"></param>
        /// <returns></returns>

        public static DataTable TenTaiKhoan(String NamChungTu)
        {
            DataTable dt;
            String KyHieu = "38";
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
        /// <summary>
        /// Lấy ngày theo tháng năm
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iNgay">Ngày</param>
        /// <returns></returns>
        public String obj_DSNgay(String ParentID, String iThang, String iNam, String iNgay, String FromOrTo)
        {
            String dsNgay = "";
            DataTable dtNgay = DanhMucModels.DT_Ngay(int.Parse(iThang), int.Parse(iNam));
            dtNgay.Rows.RemoveAt(0);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            dsNgay = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, FromOrTo, "", "class=\"input1_2\" style=\"width: 45px; padding:2px;\"");
            return dsNgay;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iThang">Tháng</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iNgay">Ngày</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsNgay(string ParentID, String iThang, String iNam, String iNgay, String FromOrTo)
        {
            return Json(obj_DSNgay(ParentID, iThang, iNam, iNgay, FromOrTo), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách tài khoản
        /// </summary>
        /// <param name="iNam">Năm</param>
        /// <param name="LenTK">Độ dài mã tài khoản</param>
        /// <returns></returns>
        public static DataTable GetTaiKhoan(String iNam, String LenTK)
        {
            DataTable dtTK = new DataTable();
            String DKTK = LenTK.Equals("6") ? "" : "AND LEN(KT.iID_MaTaiKhoan) BETWEEN 3 AND @LenTK";
            String SQL = String.Format(@"SELECT KT.iID_MaTaiKhoan,KT.iID_MaTaiKhoan+'  '+ KT.sTen sTen
                                        FROM KT_TaiKhoan KT
                                        WHERE KT.iTrangThai=1
                                        AND KT.iNam=@iNam
                                        {0}
                                        GROUP BY KT.iID_MaTaiKhoan,KT.sTen", DKTK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            if (!String.IsNullOrEmpty(DKTK))
                cmd.Parameters.AddWithValue("@LenTK", LenTK);
            dtTK = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtTK;
        }
        /// <summary>
        /// Chuỗi hiện thị danh sách tài khoản
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNam">Năm</param>
        /// <param name="LenTK">Độ dài của mã tài khoản</param>
        /// <param name="iID_MaTaiKhoan">Giá trị mã tài khoản</param>
        /// <returns></returns>
        public String obj_DSTaiKhoan(String ParentID, String iNam, String LenTK, String iID_MaTaiKhoan)
        {
            String dsTaiKhoan = "";
            DataTable dtTaiKhoan = GetTaiKhoan(iNam, LenTK);
            SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
            dsTaiKhoan = MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 245px; padding:2px;\" size='12' tab-index='-1'");
            return dsTaiKhoan;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNam">Năm</param>
        /// <param name="LenTK">Độ dài của mã tài khoản</param>
        /// <param name="iID_MaTaiKhoan">Giá trị của mã tài khoản</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsTaiKhoan(String ParentID, String iNam, String LenTK, String iID_MaTaiKhoan)
        {
            return Json(obj_DSTaiKhoan(ParentID, iNam, LenTK, iID_MaTaiKhoan), JsonRequestBehavior.AllowGet);
        }
    }
}
