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



namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_ThongTri_5_NganhController : Controller
    {
        //
        // GET: /rptQuyetToan_ThongTri_5/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToanThongTri_5_Nganh.xls";
        private String iQuy, sLNS, iID_MaTrangThaiDuyet, iID_MaDonVi, LoaiIn,MaND;
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_Thongtri_5_Nganh.aspx";
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
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {
            iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            ViewData["iQuy"] = iQuy;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_Thongtri_5_Nganh.aspx";
            return View(sViewPath + "ReportView.aspx");

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="TruongTien"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public long Tong = 0;
        public DataTable rptQT_ThongTri_5()
        {
            SqlCommand cmd = new SqlCommand();
            String DK_DonVi = "";
            if (LoaiIn == "TongHop")
            {
                String[] arrDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    if (i != 0) DK_DonVi += " OR ";
                    DK_DonVi += "iID_MaDonVi=@iID_MaDonVi" + i.ToString();
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);
                }
            }
            else
            {
                DK_DonVi += " iID_MaDonVi=@iID_MaDonVi ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                DKDuyet = "1=1";
            }
            String DKThangQuy = "";
                DKThangQuy = "iThang_Quy=@ThangQuy";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@" SELECT b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,SUM({0}) as  SoTien
             FROM ((SELECT iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,{0} FROM QTA_ChungTuChiTiet
             WHERE {4}  AND sLNS=@sLNS AND sNG<>'' {2} AND {1} AND iTrangThai=1
             GROUP BY iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,{0}
             HAVING SUM({0})!=0) a
             INNER JOIN (SELECT * FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec AND ({3})) b ON a.iID_MaDonVi=b.iID_MaDonVi)
             GROUP BY b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa", "rTuChi", DKDuyet, ReportModels.DieuKien_NganSach(MaND), DK_DonVi, DKThangQuy);
            cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@ThangQuy", iQuy);
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue(@"sLNS", sLNS);

            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm lấy mô tả loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="TruongTien"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path)
        {
            MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenLNS = "";
            DataTable dtLNS = MoTa(sLNS);
            if (dtLNS.Rows.Count > 0)
            {
                TenLNS = dtLNS.Rows[0][0].ToString();
            }
           
            String tendv = "";
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_ThongTri_5");
            LoadData(fr);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
            fr.SetValue("TenLNS", TenLNS);
            fr.SetValue("Thang_Quy", iQuy);
            fr.SetValue("TenDV", tendv);
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="TruongTien"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr)
        {
            DataTable data = rptQT_ThongTri_5();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", data, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Lấy tên đơn vị theo mã
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="TruongTien"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String TruongTien, String iID_MaTrangThaiDuyet, String sMaDonVi, String LoaiIn)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath));

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTriQuyetToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="TruongTien"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iQuy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String LoaiIn)
        {
            this.iQuy = iQuy;
            this.sLNS = sLNS;
            this.iID_MaDonVi = iID_MaDonVi;
            this.iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet;
            this.LoaiIn = LoaiIn;
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath));
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
        /// Danh Sách Loại Ngân Sach
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DSLoaiNganSach(String MaND)
        {
            DataTable dtLNS = new DataTable();
            String SQL = string.Format(@"SELECT a.sLNS,A.sLNS+ ' '+sMoTa AS TenHT 
FROM (SELECT DISTINCT sLNS
FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND LEN(sLNS)=7 {0} ) as a
INNER JOIN (SELECT * FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sL='') as b
ON a.sLNS=b.sLNS", ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            dtLNS = Connection.GetDataTable(cmd);
            return dtLNS;
        }
        /// <summary>
        /// Lấy danh sách đơn vị
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iQuy, String sLNS, String iID_MaTrangThaiDuyet, String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = "1=1";
            }
            String DK_Thang = "";

            DK_Thang = String.Format(" AND iThang_Quy= {0}", iQuy);

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT QTA.iID_MaDonVi,DV.sTen,QTA.iID_MaDonVi+' - '+DV.sTen as TenHT
                                        FROM QTA_ChungTuChiTiet AS QTA
                                        INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTen,iTrangThai FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON QTA.iID_MaDonVi=DV.MaDonVi
                                        WHERE DV.iTrangThai=1 {1} AND sLNS=@sLNS  {2}
                                        AND {0} ORDER BY DV.sTen", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DK_Thang);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            //if (dtDonVi.Rows.Count == 0)
            //{
            //    DataRow R = dtDonVi.NewRow();
            //    R["iID_MaDonVi"] = "";
            //    R["sTen"] = "Không có đơn vị";
            //    dtDonVi.Rows.InsertAt(R, 0);

            //}
            cmd.Dispose();
            return dtDonVi;
        }
        public JsonResult Ds_DonVi(String ParentID, String iQuy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            DataTable dt = LayDSDonVi(iQuy, sLNS, iID_MaTrangThaiDuyet, MaND);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, "rptQuyetToan_ThongTri_5_Nganh");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iQuy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String MaND)
        {
            DataTable dt = LayDSDonVi(iQuy, sLNS, iID_MaTrangThaiDuyet, MaND);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, "");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return strDonVi;
        }

    }
}
