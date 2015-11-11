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
    public class rptKT_SoQuyController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_SoQuy.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKT_SoQuy.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
             }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String TuNgay = Request.Form[ParentID + "_TuNgay"];
            String DenNgay = Request.Form[ParentID + "_DenNgay"];
            String TuThang = Request.Form[ParentID + "_TuThang"];
            String DenThang = Request.Form[ParentID + "_DenThang"];
            String iID_MaTaiKhoan = Request.Form[ParentID + "_iID_MaTaiKhoan"];
            ViewData["PageLoad"] = "1";
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["TuNgay"] = TuNgay;
            ViewData["DenNgay"] = DenNgay;
            ViewData["TuThang"] = TuThang;
            ViewData["DenThang"] = DenThang;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKT_SoQuy.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamChungTu = NamChungTu, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, iID_MaTaiKhoan = iID_MaTaiKhoan });
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamChungTu"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        /// <param name="TuThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaTaiKhoan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy Tên tài khoản
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
            //Lấy ngày tháng năm hiện tài
            String Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, NamChungTu, TuNgay, DenNgay, TuThang, DenThang, iID_MaTaiKhoan);
                fr = ReportModels.LayThongTinChuKy(fr, "rptKT_SoQuy");
                fr.SetValue("Nam", NamChungTu);
                fr.SetValue("TenTaiKhoan", iID_MaTaiKhoan + "-" + TenTK);
                fr.SetValue("MaTK", iID_MaTaiKhoan);
                fr.SetValue("TuNgay", TuNgay);
                fr.SetValue("TuThang", TuThang);
                fr.SetValue("DenNgay", DenNgay);
                fr.SetValue("DenThang", DenThang);
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("cuctaichinh", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("Ngay", Ngay);
                fr.Run(Result);
                return Result;
            
        }
        //dt  tài khoản
        public static DataTable TenTaiKhoan(String NamChungTu)
        {
            DataTable dt;
            String KyHieu="64";
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
            cmd.Parameters.AddWithValue("@Nam",NamChungTu);
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
        /// Hàm lấy lũy kế sổ quỹ
        /// </summary>
        /// <param name="NamChungTu"></param>
        /// <param name="TuThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable rptKT_LuyKeSoQuy(String NamChungTu,String TuNgay,String DenNgay,String TuThang,String DenThang, String iID_MaTaiKhoan)
        {

            DataTable dt = new DataTable();

            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String SQL = String.Format(@" SELECT SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo
                                            FROM(
                                            SELECT sSoChungTuChiTiet as SoChungTu
                                            ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            FROM KT_ChungTuChiTiet KTCT
                                            INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu
                                            WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@NamLamViec AND KTCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                            AND ((KTCT.iNgayCT<=@DenNgay AND KTCT.iThangCT=@DenThang) OR KTCT.iThangCT<@DenThang ) AND KTCT.iThangCT<>0 AND KTCT.iNgayCT<>0
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No,sSoChungTuChiTiet
                                            )as BANGTEM
                                            HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamChungTu);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.InsertAt(dr, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy dư cuối ki
        /// </summary>
        /// <param name="NamChungTu"></param>
        /// <param name="TuThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable rptKT_DuCuoiKi(String NamChungTu,String TuNgay,String DenNgay, String TuThang,String DenThang, String iID_MaTaiKhoan)
        {

            DataTable dt = new DataTable();
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String SQL = String.Format(@" SELECT SUM(TaiKhoanNo) as TaiKhoanNo,SUM(TaiKhoanCo) as TaiKhoanCo
                                            FROM(
                                            SELECT sSoChungTuChiTiet as SoChungTu
                                            ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            FROM KT_ChungTuChiTiet KTCT
                                            INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu
                                            WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@NamLamViec AND KTCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                             AND ((KTCT.iNgayCT<=@DenNgay AND KTCT.iThangCT=@DenThang) OR KTCT.iThangCT<@DenThang )
                                            GROUP BY iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No,sSoChungTuChiTiet
                                            )as BANGTEM
                                            HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamChungTu);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.InsertAt(dr, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm lấy dữ liệu cho sổ quỹ
        /// </summary>
        /// <param name="NamChungTu"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        /// <param name="TuThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable rptKT_SoQuy(String NamChungTu, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaTaiKhoan)
        {

            DataTable dt = new DataTable();
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String SQL = String.Format(@" SELECT SoGhiSo,NgayGhiSo,SoChungTu,NgayChungTu,sNoiDung,doichieu,doiung,iNgayCT,iThangCT
                                            ,SUM(TaiKhoanCo) as TaiKhoanCo,SUM(TaiKhoanNo) as TaiKhoanNo
                                            FROM(
                                            SELECT sSoChungTuChiTiet as SoChungTu,CAST(iNgayCT as nvarchar)+'/' +CAST(iThangCT as nvarchar) as NgayChungTu
                                            ,sSoChungTu as SoGhiSo
                                            ,iNgayCT
                                            ,iThangCT
                                            ,CAST(CT.iNgay as nvarchar)+'/'+CAST(CT.iThang as nvarchar) as NgayGhiSo
                                            ,KTCT.sNoiDung,SUM(rSoTien) as SoTien
                                            ,iID_MaTaiKhoan_Co as doichieu
                                            ,iID_MaTaiKhoan_No as doiung
                                            ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            FROM KT_ChungTuChiTiet KTCT
                                            INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu
                                            WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@NamLamViec AND KTCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet 
                                            AND ((KTCT.iNgayCT>=@TuNgay AND KTCT.iThangCT=@TuThang) OR KTCT.iThangCT>@TuThang )
                                             AND ((KTCT.iNgayCT<=@DenNgay AND KTCT.iThangCT=@DenThang) OR KTCT.iThangCT<@DenThang )
                                            AND iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%'
                                            GROUP BY sSoChungTuChiTiet,KTCT.iTrangThai ,iNgayCT,iThangCT,KTCT.sNoiDung,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co,sSoChungTu,CT.iThang,CT.iNgay,KTCT.iThang
                                            HAVING SUM(rSoTien)!=0
                                            )as BANGTEM
                                            GROUP BY SoGhiSo,NgayGhiSo,SoChungTu,NgayChungTu,sNoiDung,SoTien,doichieu,doiung,iNgayCT,iThangCT
                                            HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0

                                            UNION

                                            SELECT SoGhiSo,NgayGhiSo,SoChungTu,NgayChungTu,sNoiDung,doichieu,doiung,iNgayCT,iThangCT
                                            ,SUM(TaiKhoanCo) as TaiKhoanCo,SUM(TaiKhoanNo) as TaiKhoanNo
                                            FROM(
                                            SELECT sSoChungTuChiTiet as SoChungTu,CAST(iNgayCT as nvarchar)+'/' +CAST(iThangCT as nvarchar) as NgayChungTu
                                            ,sSoChungTu as SoGhiSo
                                            ,iNgayCT
                                            ,iThangCT
                                            ,CAST(CT.iNgay as nvarchar)+'/'+CAST(CT.iThang as nvarchar) as NgayGhiSo
                                            ,KTCT.sNoiDung,SUM(rSoTien) as SoTien
                                            ,iID_MaTaiKhoan_No as doichieu
                                            ,iID_MaTaiKhoan_Co as doiung
                                            ,TaiKhoanNo=CASE WHEN iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            ,TaiKhoanCo=CASE WHEN iID_MaTaiKhoan_Co Like @iID_MaTaiKhoan+'%' THEN SUM(rSoTien) ELSE 0 END
                                            FROM KT_ChungTuChiTiet KTCT
                                            INNER JOIN KT_ChungTu as CT ON KTCT.iID_MaChungTu=CT.iID_MaChungTu
                                            WHERE KTCT.iTrangThai=1 AND KTCT.iNamLamViec=@NamLamViec AND KTCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                            AND KTCT.iNgayCT>=@TuNgay AND KTCT.iNgayCT<=@DenNgay AND KTCT.iThangCT>=@TuThang AND KTCT.iThangCT<=@DenThang AND iID_MaTaiKhoan_No Like @iID_MaTaiKhoan+'%'
                                            GROUP BY sSoChungTuChiTiet,KTCT.iTrangThai ,iNgayCT,iThangCT,KTCT.sNoiDung,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co,sSoChungTu,CT.iThang,CT.iNgay,KTCT.iThang
                                            HAVING SUM(rSoTien)!=0
                                            )as BANGTEM
                                            GROUP BY SoGhiSo,NgayGhiSo,SoChungTu,NgayChungTu,sNoiDung,SoTien,doichieu,doiung,iNgayCT,iThangCT
                                            HAVING SUM(TaiKhoanNo)!=0 OR SUM(TaiKhoanCo)!=0
                                            ORDER BY iThangCT,iNgayCT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamChungTu);
            cmd.Parameters.AddWithValue("@TuNgay", TuNgay);
            cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@DenThang", DenThang);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra ngoài báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamChungTu"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        /// <param name="TuThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        private void LoadData(FlexCelReport fr, String NamChungTu, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaTaiKhoan)
        {
            //fill dữ liệu Chi tiết
            DataTable data = rptKT_SoQuy(NamChungTu, TuNgay, DenNgay, TuThang, DenThang, iID_MaTaiKhoan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            //fill dữ liệu dư Lũy kế
            DataTable dataLuyKe = rptKT_LuyKeSoQuy(NamChungTu,TuNgay,DenNgay, TuThang, DenThang, iID_MaTaiKhoan);
            dataLuyKe.TableName = "LuyKe";
            fr.AddTable("LuyKe", dataLuyKe);
            dataLuyKe.Dispose();
            //fill dữ liệu dư cuối kì
            DataTable dataDuCuoiKi = rptKT_DuCuoiKi(NamChungTu,TuNgay,DenNgay, TuThang,DenThang,iID_MaTaiKhoan);
            dataDuCuoiKi.TableName = "DuCuoiKi";
            fr.AddTable("DuCuoiKi", dataDuCuoiKi);
            dataDuCuoiKi.Dispose();




        }
        /// <summary>
        /// Hàm thực hiện việc xuất dữ liệu ra excel
        /// </summary>
        /// <param name="NamChungTu"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        /// <param name="TuThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaTaiKhoan)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, TuNgay, DenNgay, TuThang, DenThang, iID_MaTaiKhoan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "SoQuy.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
 /// <summary>
 /// Hàm xuất dữ liệu ra file PDF
 /// </summary>
 /// <param name="NamChungTu"></param>
 /// <param name="TuNgay"></param>
 /// <param name="DenNgay"></param>
 /// <param name="TuThang"></param>
 /// <param name="iID_MaTaiKhoan"></param>
 /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamChungTu, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaTaiKhoan)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, TuNgay, DenNgay, TuThang, DenThang, iID_MaTaiKhoan);
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
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamChungTu"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        /// <param name="TuThang"></param>
        /// <param name="DenThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu, String TuNgay, String DenNgay, String TuThang, String DenThang, String iID_MaTaiKhoan)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, TuNgay, DenNgay, TuThang, DenThang, iID_MaTaiKhoan);
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
        public String obj_DSNgay(String ParentID, String iThang, String iNam, String iNgay, String FromOrTo)
        {
            String dsNgay = "";
            DataTable dtNgay = DanhMucModels.DT_Ngay(int.Parse(iThang), int.Parse(iNam));
            dtNgay.Rows.RemoveAt(0);
            SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
            dsNgay = MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, FromOrTo, "", "class=\"input1_2\" style=\"width: 60px; padding:2px;\"");
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

    }
}
