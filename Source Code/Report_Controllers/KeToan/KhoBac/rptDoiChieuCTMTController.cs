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


namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptDoiChieuCTMTController : Controller
    {
        //
        // GET: /rptDoiChieuCTMT/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePathTH = "/Report_ExcelFrom/KeToan/KhoBac/rptDoiChieuCTMTM.xls";
        private const String sFilePathTH1 = "/Report_ExcelFrom/KeToan/KhoBac/rptDoiChieuCTMTTM.xls";
        private const String sFilePathTH2 = "/Report_ExcelFrom/KeToan/KhoBac/rptDoiChieuCTMT.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {   
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptDoiChieuCTMT.aspx";
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
            String Thang_Quy = "";
            String NamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String inmuc = Convert.ToString(Request.Form[ParentID + "_inmuc"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            String iID_MaChuongTrinhMucTieu = Convert.ToString(Request.Form[ParentID + "_iID_MaChuongTrinhMucTieu"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            
            ViewData["NamLamViec"] = NamLamViec;
            ViewData["ThangQuy"] = Thang_Quy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["inmuc"] = inmuc;
            ViewData["iID_MaChuongTrinhMucTieu"] = iID_MaChuongTrinhMucTieu;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptDoiChieuCTMT.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
            //String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, inmuc = inmuc, iID_MaChuongTrinhMucTieu = iID_MaChuongTrinhMucTieu, iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn tới file excel mẫu</param>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">0: Tháng |1: Quý</param>
        /// <param name="inmuc">In đến mục</param>
        /// <param name="iID_MaChuongTrinhMucTieu">Mã chương trình mục tiêu</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaChuongTrinhMucTieu, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            String TenLoaiThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                TenLoaiThangQuy = "Quý";
            }
            else
            {
                TenLoaiThangQuy = "Tháng";
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDoiChieuCTMT");
            LoadData(fr, NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaChuongTrinhMucTieu,iID_MaTrangThaiDuyet);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("Ngay", Ngay);
            fr.Run(Result);
            return Result;
        }        
        /// <summary>
        /// Lấy đường dẫn file Excel mẫu theo mục in
        /// </summary>
        /// <param name="inmuc">In đến mục</param>
        /// <returns></returns>
        private static String PathFile(String inmuc)
        {
            String DuongDan = "";
            if (inmuc == "1")
            {
                DuongDan = sFilePathTH;
            }
            else if (inmuc == "2")
            {
                DuongDan = sFilePathTH1;
            }
            else if (inmuc == "3")
            {
                DuongDan = sFilePathTH2;
            }
            return DuongDan;
        }
        /// <summary>
        /// Hiện thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">0: Tháng | 1: Quý</param>
        /// <param name="inmuc">In đến mục</param>
        /// <param name="iID_MaChuongTrinhMucTieu">Mã chương trình mục tiêu</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaChuongTrinhMucTieu, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = PathFile(inmuc); 
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaChuongTrinhMucTieu,iID_MaTrangThaiDuyet);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">0: Tháng | 1: Quý</param>
        /// <param name="inmuc">In đến mục</param>
        /// <param name="iID_MaChuongTrinhMucTieu">Mã chương trình mục tiêu</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaChuongTrinhMucTieu,String iID_MaTrangThaiDuyet)
        {
            String DuongDan = PathFile(inmuc);
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaChuongTrinhMucTieu,iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DoiChieuCTMT.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">File báo cáo</param>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">0: Tháng | 1: Quý</param>
        /// <param name="inmuc">In đến mục</param>
        /// <param name="iID_MaChuongTrinhMucTieu">Mã chương trình mục tiêu</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        private void LoadData(FlexCelReport fr, String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaChuongTrinhMucTieu,String iID_MaTrangThaiDuyet)
        {
            DataTable data = DoiChieuDuToan(NamLamViec, Thang_Quy, LoaiThang_Quy, inmuc, iID_MaChuongTrinhMucTieu,iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi;
            dtDonVi = HamChung.SelectDistinct("DonVi", data, "sLNS,sL,sK,sM,sTM,iID_MaDonVi", "sLNS,sL,sK,sM,sTM,iID_MaDonVi");
            fr.AddTable("DonVi", dtDonVi);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TM", dtDonVi, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("TM", dtTieuMuc);
            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM");
            fr.AddTable("LoaiNS", dtLoaiNS);            
            data.Dispose();            
            dtLoaiNS.Dispose();
            dtDonVi.Dispose();
            dtTieuMuc.Dispose();           
        }
        /// <summary>
        /// Lấy thông tin đối chiếu dự toán
        /// </summary>
        /// <param name="NamLamViec">Năm</param>
        /// <param name="Thang_Quy">Tháng | Quý</param>
        /// <param name="LoaiThang_Quy">0: Tháng | 1: Quý</param>
        /// <param name="inmuc">In đến mục</param>
        /// <param name="iID_MaChuongTrinhMucTieu">Mã chương trình mục tiêu</param>
        /// <param name="iID_MaTrangThaiDuyet">Trạng thái duyệt</param>
        /// <returns>Danh sách</returns>
        public DataTable DoiChieuDuToan(String NamLamViec, String Thang_Quy, String LoaiThang_Quy, String inmuc, String iID_MaChuongTrinhMucTieu,String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }
            //String DKDuyet = iID_MaTrangThaiDuyet.Equals("0") ? "" : "and iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
            //int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            String DKCuoiThang = "";
            String DKTrongThang = "";
            String DKDauThang = "";
            
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = " MONTH(dNgayDotRutDuToan) in (1,2,3) ";
                        DKCuoiThang = "iThangCT between 1 and 3";
                        DKTrongThang = "iThangCT between 1 and 3";
                        DKDauThang = "iThangCT <1";
                        break;
                    case 2: DKThang_Quy = " MONTH(dNgayDotRutDuToan) in (4,5,6) ";
                        DKCuoiThang = "iThangCT between 4 and 6";
                        DKTrongThang = "iThangCT between 1 and 3";
                        DKDauThang = "iThangCT <4";
                        break;
                    case 3: DKThang_Quy = " MONTH(dNgayDotRutDuToan) in (7,8,9) ";
                        DKCuoiThang = "iThangCT between 7 and 9";
                        DKTrongThang = "iThangCT <7";
                        break;
                    case 4: DKThang_Quy = " MONTH(dNgayDotRutDuToan) in (10,11,12) ";
                        DKCuoiThang = "iThangCT between 10 and 12";
                        DKTrongThang = "iThangCT between 1 and 3";
                        DKDauThang = "iThangCT <10";
                        break;
                }
            }
            else
            {
                DKThang_Quy = " MONTH(dNgayDotRutDuToan) <=@ThangQuy ";
                DKCuoiThang = "iThangCT<=@ThangQuy";
                DKTrongThang = "iThangCT=@ThangQuy";
                DKDauThang = "iThangCT<@ThangQuy";
            }
            String SQL = String.Format(@" SELECT sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi,ctrinh,tenct,sum(chitieu) as chitieu,sum(dauky) as dauky,sum(trongthang)as trongthang,sum(cuoiky) as cuoiky FROM(
                                            SELECT sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan as iID_MaDonVi
                                                , iID_MaChuongTrinhMucTieu_Tra as ctrinh,sTenChuongTrinhMucTieu_Tra as tenct
                                                , 0 as chitieu
                                                , dauky = sum(case when {3} then rDTRut else 0 end)
                                                , trongthang = sum(case when {2} then rDTRut else 0 end)
                                                , cuoiky = sum(case when {1} then rDTRut else 0 end)  
                                        FROM  KTKB_ChungTuChiTiet
                                        WHERE iNamLamViec = @iNamLamViec 
                                          {4}                                           
                                          and iID_MaChuongTrinhMucTieu_Tra = @iID_MaChuongTrinhMucTieu 
                                        GROUP BY sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi_Nhan,iID_MaChuongTrinhMucTieu_Tra,sTenChuongTrinhMucTieu_Tra 
                                        union  
                                        SELECT sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi as iID_MaDonVi
                                              ,iID_MaChuongTrinhMucTieu as ctrinh
                                              ,sTenChuongTrinhMucTieu as tenct
                                              ,chitieu = sum(case when  {0}  then rSoTien else 0 end) 
                                              ,0 as dauky
                                              ,0 as trongthang
                                              ,0 as cuoiky  
                                        FROM  KT_RutDuToanChitiet WHERE iNamLamViec = @iNamLamViec and iID_MaChuongTrinhMucTieu = @iID_MaChuongTrinhMucTieu 
                                        GROUP by sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi,iID_MaChuongTrinhMucTieu,sTenChuongTrinhMucTieu) as bang
                                        GROUP by sLNS,sL,sK,sM,sTM,sMoTa,iID_MaDonVi,ctrinh,tenct
                                        having sum(dauky)<>0 or sum(chitieu)<>0 or Sum(trongthang)<> 0 or sum(cuoiky)<>0", DKThang_Quy, DKCuoiThang, DKTrongThang, DKDauThang,iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaChuongTrinhMucTieu", iID_MaChuongTrinhMucTieu);
            //if(!String.IsNullOrEmpty(DKDuyet))
            //    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            if (LoaiThang_Quy.Equals("0"))
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();  
            return dt;
        }
        
        public static DataTable DanhSach_inmuc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMuc", typeof(String));
            dt.Columns.Add("TenMuc", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = " Mục ";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "Tiểu mục";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "3";
            R3[1] = "Đơn vị";
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách chương trình mục tiêu
        /// </summary>
        /// <returns></returns>
        public static DataTable Lay_CTMT()
        {
            String SQL = String.Format(@"select iID_MaChuongTrinhMucTieu ,
                                        Convert(varchar,iID_MaChuongTrinhMucTieu)+'-'+ SUBSTRING(sTenchuongTrinhMucTieu,charindex('-',sTenchuongTrinhMucTieu)+1,100) AS sTenchuongTrinhMucTieu
                                        from KT_RutDuToanChiTiet
                                          WHERE iID_MaChuongTrinhMucTieu IS NOT NULL AND iID_MaChuongTrinhMucTieu<>''
                                        group by iID_MaChuongTrinhMucTieu,sTenchuongTrinhMucTieu ");
                DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
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
