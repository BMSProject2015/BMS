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
namespace VIETTEL.Report_Controllers.CapPhat
{
    public class rptCapPhat_TheoDonViController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/CapPhat/rptCP_DonVi.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {        
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
                {
                    ViewData["PageLoad"] = "0";
                    ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_TheoDonVi.aspx";
                    return View(sViewPath + "ReportView.aspx");
                 }
            else
                 {
                          return RedirectToAction("Index", "PermitionMessage");
                  }
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String Thang = Convert.ToString(Request.Form[ParentID + "_Thang"]);

            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["Thang"] = Thang;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_TheoDonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
           
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang)
        {
            String MaND = User.Identity.Name;
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String  iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV = "";
            if (iID_MaDonVi != "" && iID_MaDonVi != "-1" && iID_MaDonVi != "-2")
            {
            TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else if (iID_MaDonVi != "-1")
            {

            }
            FlexCelReport fr = new FlexCelReport();
            
            fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_TheoDonVi");
            LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi, Thang, MaND);              
                fr.SetValue("Nam","  Năm  "+ iNamLamViec);
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("Thang", Thang);
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3));
                fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// tạo range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang,String MaND)
        {
            DataTable data = CapPhat_TheoDonVi(iID_MaTrangThaiDuyet, iID_MaDonVi, Thang, MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            DataTable dtNgay;
            dtNgay = HamChung.SelectDistinct("dNgay", data, "TenDonVi,dNgay", "TenDonVi,dNgay", "TenDonVi,dNgay");
            fr.AddTable("dNgay", dtNgay);
            dtNgay.Dispose();

            //DataTable dtTieuMuc;
            //dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            //fr.AddTable("TieuMuc", dtTieuMuc);

            //DataTable dtMuc;
            //dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            //fr.AddTable("Muc", dtMuc);

            //DataTable dtLoaiNS;
            //dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL");
            //fr.AddTable("LoaiNS", dtLoaiNS);

            //DataTable dtNguonNS;
            //dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            //fr.AddTable("NguonNS", dtNguonNS);
            //data.Dispose();
            //dtLoaiNS.Dispose();
            //dtMuc.Dispose();
            //dtNguonNS.Dispose();
            //dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang)
        {
            HamChung.Language();

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi, Thang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CapPhat_TheoDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi, Thang);
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
        /// xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, iID_MaDonVi, Thang);
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
        /// onchage
        /// </summary>
        public class LCPData
        {
            public string Thang { get; set; }
            public string DonVi { get; set; }
            public string LuyKe { get; set; }
        }
        public JsonResult ds_NgayCapPhat(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang)
        {
            String MaND = User.Identity.Name;
            return Json(obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi, Thang, MaND), JsonRequestBehavior.AllowGet);
        }
        public LCPData obj_NgayCapPhat(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang, String MaND)
        {
            
            LCPData _LCPData = new LCPData();
            DataTable dtThang = DanhSach_Thang_CapPhat(iID_MaTrangThaiDuyet,MaND);
            SelectOptionList slThang = new SelectOptionList(dtThang, "Thang", "TenThang");
            _LCPData.Thang = MyHtmlHelper.DropDownList(ParentID, slThang, Thang , "Thang", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonThang()\" ");


            DataTable dtDonVi = DanhSach_DonVi(iID_MaTrangThaiDuyet, Thang ,MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            _LCPData.DonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi","", "class=\"input1_2\" style=\"width: 100%\"");          
            return _LCPData;
        }
        /// <summary>
        /// CapPhat_TheoDonVi
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public DataTable CapPhat_TheoDonVi(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang,String MaND)
        {
            #region //Các đk lọc


            //Điều kiện đơn vị
            String DKDonVi = "";
            String queryDonVi = "(SELECT TOP 1 sTen FROM NS_DonVi WHERE NS_DonVi.iID_MaDonVi = CP_CapPhatChiTiet.iID_MaDonVi) + ' - '";
            if (iID_MaDonVi != "" && iID_MaDonVi != "-1" && iID_MaDonVi != "-2")
            {
                queryDonVi = "''";
                DKDonVi = " AND CP_CapPhatChiTiet.iID_MaDonVi=@iID_MaDonVi";
            }
            
            String DKDuyet = "", DKDuyetPB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND CP_CapPhatChiTiet.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
                DKDuyetPB = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            #endregion
            #region //Tạo datatable cấp phát

            String SQLCapPhat = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sMoTa,dNgayCapPhat,DAY(dNgayCapPhat) as dNgay,TenDonVi
                    ,SUM(HanMuc) AS HanMuc,SUM(TienMat) AS TienMat,SUM(TienGui) AS TienGui,SUM(NgoaiTe) AS NgoaiTe,SUM(Khac) AS Khac
                    FROM(
                    SELECT SUBSTRING(sLNS,1,1) AS NguonNS,sLNS,sL,sK,sM,sTM,sMoTa,CP_CapPhat.dNgayCapPhat 
                    ,HanMuc = CASE WHEN CP_CapPhatChiTiet.iDM_MaLoaiCapPhat IN (SELECT iID_MaDanhMuc FROM DC_DanhMuc INNER JOIN 
                    DC_LoaiDanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                    WHERE DC_LoaiDanhMuc.sTenBang = 'LoaiCapPhat' AND DC_DanhMuc.sTenKhoa = 'HanMuc') THEN SUM(rTuChi+rHienVat) ELSE 0 END
                    ,TienMat = CASE WHEN CP_CapPhatChiTiet.iDM_MaLoaiCapPhat IN (SELECT iID_MaDanhMuc FROM DC_DanhMuc INNER JOIN 
                    DC_LoaiDanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                    WHERE DC_LoaiDanhMuc.sTenBang = 'LoaiCapPhat' AND DC_DanhMuc.sTenKhoa = 'TienMat') THEN SUM(rTuChi+rHienVat) ELSE 0 END
                    ,TienGui = CASE WHEN CP_CapPhatChiTiet.iDM_MaLoaiCapPhat IN (SELECT iID_MaDanhMuc FROM DC_DanhMuc INNER JOIN 
                    DC_LoaiDanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                    WHERE DC_LoaiDanhMuc.sTenBang = 'LoaiCapPhat' AND DC_DanhMuc.sTenKhoa = 'TienGui') THEN SUM(rTuChi+rHienVat) ELSE 0 END
                    ,NgoaiTe = CASE WHEN CP_CapPhatChiTiet.iDM_MaLoaiCapPhat IN (SELECT iID_MaDanhMuc FROM DC_DanhMuc INNER JOIN 
                    DC_LoaiDanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                    WHERE DC_LoaiDanhMuc.sTenBang = 'LoaiCapPhat' AND DC_DanhMuc.sTenKhoa = 'NgoaiTe') THEN SUM(rTuChi+rHienVat) ELSE 0 END
                    ,Khac = CASE WHEN CP_CapPhatChiTiet.iDM_MaLoaiCapPhat IN (SELECT iID_MaDanhMuc FROM DC_DanhMuc INNER JOIN 
                    DC_LoaiDanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc
                    WHERE DC_LoaiDanhMuc.sTenBang = 'LoaiCapPhat' AND NOT (DC_DanhMuc.sTenKhoa IN ('HanMuc','TienMat','TienGui','NgoaiTe')))
                    THEN SUM(rTuChi+rHienVat) ELSE 0 END 
                    ,TenDonVi = {3}         
                    FROM CP_CapPhatChiTiet INNER JOIN 
                    CP_CapPhat ON CP_CapPhat.iID_MaCapPhat = CP_CapPhatChiTiet.iID_MaCapPhat
                    WHERE  sNG<>'' AND MONTH(CP_CapPhat.dNgayCapPhat) = @Thang {0} {1} {2}
                    GROUP BY  SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sMoTa,CP_CapPhat.dNgayCapPhat,CP_CapPhatChiTiet.iID_MaDonVi,CP_CapPhatChiTiet.iDM_MaLoaiCapPhat) AS tbl
                    GROUP BY  NguonNS,sLNS,sL,sK,sM,sTM,sMoTa,dNgayCapPhat,TenDonVi
                    HAVING SUM(HanMuc) <> 0 OR SUM(TienMat) <> 0 OR SUM(TienGui) <> 0 OR SUM(NgoaiTe) <> 0 OR SUM(Khac) <> 0
                    ORDER BY dNgayCapPhat",  DKDonVi, ReportModels.DieuKien_NganSach(MaND,"CP_CapPhatChiTiet"), DKDuyet,queryDonVi);
            SqlCommand cmdCapPhat = new SqlCommand(SQLCapPhat);
            if (iID_MaDonVi != "" && iID_MaDonVi != "-1" && iID_MaDonVi != "-2")
            {
                cmdCapPhat.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
           
            cmdCapPhat.Parameters.AddWithValue("@Thang", Thang);               
            DataTable dtCapPhat = Connection.GetDataTable(cmdCapPhat);
            cmdCapPhat.Dispose();
#endregion
            return dtCapPhat;
        }
        /// <summary>
        /// Lấy danh sách ngày cấp phát
        /// </summary>
        /// <param name="iIDM_MaLoaiCapPhat"> Mã loại cấp phát</param>
        /// <returns></returns>
        public static DataTable DanhSach_Thang_CapPhat(String iID_MaTrangThaiDuyet,String MaND)
        {
            // Nều là chọn toàn bộ loại cấp phát
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"SELECT DISTINCT CONVERT(varchar(10),MONTH(dNgayCapPhat)) as Thang, CONVERT(varchar(10),MONTH(dNgayCapPhat)) as TenThang
                                        FROM CP_CapPhat
                                        WHERE iTrangThai=1  {0} {1}
                                        ORDER BY Thang", ReportModels.DieuKien_NganSach(MaND), DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count <= 0)
            {
                DataRow R = dt.NewRow();
                R["TenThang"] = "--Không có đợt CP--";
                R["Thang"] = "0";
                dt.Rows.InsertAt(R, 0);
               
            }
            else
            {
                DataRow R = dt.NewRow();
                R["TenThang"] = "--Chọn tháng--";
                R["Thang"] = "0";
                dt.Rows.InsertAt(R, 0);
            }
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm trả về danh sách đơn vị
        /// </summary>
        /// <param name="NamLamVIec">Năm lằm việc</param>
        /// <param name="dNgayCapPhat">Ngày cấp phát</param>
        /// <param name="LuyKe"> Chọn lũy kế</param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String iID_MaTrangThaiDuyet, String Thang, String MaND)
        {
            DataTable dtDonvi = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            if (!String.IsNullOrEmpty(Thang)) {
                DK = " AND MONTH(CP_CapPhat.dNgayCapPhat)=@Thang";
                cmd.Parameters.AddWithValue("@Thang", Thang);
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND CP_CapPhatChiTiet.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
            }
            String iNamLamViec = DateTime.Now.Year.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,b.sTen
                                        FROM (SELECT CP_CapPhatChiTiet.iID_MaDonVi FROM CP_CapPhatChiTiet 
                                        INNER JOIN CP_CapPhat on CP_CapPhatChiTiet.iID_MaCapPhat=CP_CapPhat.iID_MaCapPhat
                                        WHERE 1=1 {0} {1} {2} AND CP_CapPhatChiTiet.iID_MaDonVi NOT IN (00,99) AND CP_CapPhatChiTiet.iTrangThai=1 ) as a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as  b ON a.iID_MaDonVi=b.iID_MaDonVi
                                        GROUP BY  b.sTen,a.iID_MaDonVi", DK,  DieuKien_NganSach(MaND), DKDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
           
            dtDonvi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtDonvi.Rows.Count > 0)
            {
                DataRow R = dtDonvi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả đơn vị";
                dtDonvi.Rows.InsertAt(R, 0);
                //DataRow R1 = dtDonvi.NewRow();
                //R1["iID_MaDonVi"] = "-2";
                //R1["iID_MaDonVi"] = Guid.Empty.ToString();
                //R1["sTen"] = "-- Chọn đơn vị --";
                //dtDonvi.Rows.InsertAt(R1, 0);
            }
            else
            {
                DataRow R = dtDonvi.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dtDonvi.Rows.InsertAt(R, 0);
            }
            return dtDonvi;
        }
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
        public static String DieuKien_NganSach(String MaND)
        {
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung(MaND);
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (CP_CapPhatChiTiet.iNamLamViec={0} AND CP_CapPhatChiTiet.iID_MaNamNganSach={1} AND CP_CapPhatChiTiet.iID_MaNguonNganSach={2}
                                  AND (CP_CapPhatChiTiet.iID_MaPhongBan ='{3}' )
                                 AND (CP_CapPhatChiTiet.iID_MaDonVi IN ({4})))", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, iID_MaPhongBan, iID_MaDonVi);
            return DK;
        }
         public static String DieuKien_NganSach_DtChieuTieu(String MaND)
        {
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung(MaND);
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (PB_PhanBoChiTiet.iNamLamViec={0} AND PB_PhanBoChiTiet.iID_MaNamNganSach={1} AND PB_PhanBoChiTiet.iID_MaNguonNganSach={2}
                                AND (PB_PhanBoChiTiet.iID_MaPhongBan ='{3}' )
                                 AND (PB_PhanBoChiTiet.iID_MaDonVi IN ({4})))", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, iID_MaPhongBan, iID_MaDonVi);
            return DK;
        }

    }
}
