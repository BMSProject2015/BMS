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
namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_ThuongXuyen_23_2Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1.xls";
        private const String sFilePath_1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_1.xls";
        private const String sFilePath_NgayNguoi = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_NgayNguoi.xls";
        private const String sFilePath_1_NgayNguoi = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_22_1_1_NgayNguoi.xls";
        public static String NameFile = "";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
          
            ViewData["PageLoad"] = 0;
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_23_2.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
          
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// edit submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {
            String Thang_Quy = "";
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            String iID_MaDanhMuc = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMuc"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            if (LoaiThang_Quy == "0")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            String chkNgayNguoi = Convert.ToString(Request.Form[ParentID + "_chkNgayNguoi"]);
            String TongHop = Convert.ToString(Request.Form[ParentID + "_iTongHop"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
            ViewData["TruongTien"] = TruongTien;
            ViewData["PageLoad"] = 1;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["chkNgayNguoi"] = chkNgayNguoi;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_23_2.aspx";
            return View(sViewPath + "ReportView.aspx");   
            //return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi, iID_MaDanhMuc = iID_MaDanhMuc });
        }
        /// <summary>
        /// khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDanhMuc"> mã nhóm đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc,String TruongTien,String LoaiIn)
        {
            
            String MaND = User.Identity.Name;
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
            String TenDV;
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1" && iID_MaDonVi != "-2")
            {
                TenDV = iID_MaDonVi + "  -  " + Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }
            if (LoaiIn=="TongHop")
            {
                
                DataTable dtNhomDV = DanhSachNhomDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
                for (int i = 0; i < dtNhomDV.Rows.Count;i++)
                {
                    if (iID_MaDanhMuc == dtNhomDV.Rows[i]["iID_MaDanhMuc"].ToString())
                    {
                        TenDV = dtNhomDV.Rows[i]["TenDM"].ToString();
                    }
                }
            }
            String TenLoaiThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                TenLoaiThangQuy = "Quý";
            }
            else
            {
                TenLoaiThangQuy = "Tháng";
            }
             //tính tổng tiền
            DataTable dtTien = QuyetToan_TX_23_2(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, iID_MaDanhMuc, MaND, TruongTien, LoaiIn);
            long TongTien = 0;
            String str = "0";
            for (int i = 0; i < dtTien.Rows.Count; i++)
            {
                str = dtTien.Rows[i]["DotNay"].ToString().Equals("") ? "0" : dtTien.Rows[i]["DotNay"].ToString();
                TongTien += long.Parse(str);
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
             FlexCelReport fr = new FlexCelReport();
             fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_thuongxuyen_23_2");
             LoadData(fr, iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, iID_MaDanhMuc, MaND, TruongTien, LoaiIn);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("Quy", Thang_Quy);
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.SetValue("Tien", Tien);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// tạo các range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>H
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDanhMuc"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc, String MaND, String TruongTien, String LoaiIn)
        {
            DataTable data = QuyetToan_TX_23_2(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, iID_MaDanhMuc, MaND, TruongTien, LoaiIn);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        
        /// <summary>
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc, String TruongTien, String LoaiIn, String chkNgayNguoi)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (chkNgayNguoi == "on")
            {
                if (LoaiThang_Quy == "0")
                    DuongDanFile = sFilePath_1_NgayNguoi;
                else DuongDanFile = sFilePath_NgayNguoi;
            }
            else
            {
                if (LoaiThang_Quy == "0")
                    DuongDanFile = sFilePath_1;
                else DuongDanFile = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, iID_MaDanhMuc, TruongTien, LoaiIn);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_ThuongXuyen_23_2.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// xem file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc, String TruongTien, String LoaiIn, String chkNgayNguoi)
        {
            HamChung.Language();
            String DuongDanFile = "";
            if (chkNgayNguoi == "on")
            {
                if (LoaiThang_Quy == "0")
                    DuongDanFile = sFilePath_1_NgayNguoi;
                else DuongDanFile = sFilePath_NgayNguoi;
            }
            else
            {
                if (LoaiThang_Quy == "0")
                    DuongDanFile = sFilePath_1;
                else DuongDanFile = sFilePath;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, iID_MaDanhMuc, TruongTien, LoaiIn);
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
        /// onchange
        /// </summary>
        public  class NhomDonViData
        {
            public String iID_MaDanhMuc { get; set; }
            public String  iID_MaDonVi { get; set; }
        }
        public NhomDonViData obj_NhomDonVi(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc, String MaND)
        {
            NhomDonViData _data = new NhomDonViData();
            //DataTable dtNhomDonVi = DanhSachNhomDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
            //SelectOptionList slNhomDonVi = new SelectOptionList(dtNhomDonVi, "iID_MaDanhMuc", "TenDM");
            //_data.iID_MaDanhMuc = MyHtmlHelper.DropDownList(ParentID, slNhomDonVi, iID_MaDanhMuc, "iID_MaDanhMuc", "", "class=\"input1_2\" style=\"width: 100%\" onChange=ChonDV()");
            //dtNhomDonVi.Dispose();
            //DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDanhMuc, MaND);
            //String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            //DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dtDonVi, "");
            //String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            //_data.iID_MaDonVi = strDonVi;
            //dtDonVi.Dispose();
            return _data;
        }
        [HttpGet]
        public JsonResult ds_NhomDonVi(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc)
        {
            String MaND = User.Identity.Name;
            NhomDonViData _data = new NhomDonViData();
            DataTable dtNhomDonVi = DanhSachNhomDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
            SelectOptionList slNhomDonVi = new SelectOptionList(dtNhomDonVi, "iID_MaDanhMuc", "TenDM");
            _data.iID_MaDanhMuc = MyHtmlHelper.DropDownList(ParentID, slNhomDonVi, iID_MaDanhMuc, "iID_MaDanhMuc", "", "class=\"input1_2\" style=\"width: 50%\" onChange=ChonDV()");
            if (iID_MaDanhMuc == "undefined")
            {
                iID_MaDanhMuc = Guid.Empty.ToString();
            }
            dtNhomDonVi.Dispose();

            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDanhMuc, MaND);
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dtDonVi, "");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            _data.iID_MaDonVi = strDonVi;
            dtDonVi.Dispose();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// danh sách nhóm đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public static DataTable DanhSachNhomDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy,String MaND)
        {
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "(iThang_Quy between 1  and 3)";
                        break;
                    case 2: DKThang_Quy = "(iThang_Quy between 4  and 6)";
                        break;
                    case 3: DKThang_Quy = "(iThang_Quy between 7 and 9)";
                        break;
                    case 4: DKThang_Quy = "(iThang_Quy between 10  and 12)";
                        break;
                }
            }
            else
            {
                DKThang_Quy = "iThang_Quy=@ThangQuy";
            }
            String DKDuyet1 = "",DKDuyet2 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                DKDuyet2 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
           
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String SQL = String.Format(@"SELECT DISTINCT c.sTen as TenDM,c.iID_MaDanhMuc 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM QTA_ChungTuChiTiet
	                                           WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {1} {2}
	                                                 AND ({0}) AND bLoaiThang_Quy=0) a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc) c
                                        ON b.iID_MaNhomDonVi=c.iID_MaDanhMuc
                                         UNION

                                         SELECT DISTINCT c1.sTen as TenDM,c1.iID_MaDanhMuc 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM PB_PhanBoChiTiet
	                                           WHERE  iTrangThai=1 {1} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
                                                     AND sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {1} {3}
	                                                ) a1
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b1
                                        ON a1.iID_MaDonVi=b1.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc) c1
                                        ON b1.iID_MaNhomDonVi=c1.iID_MaDanhMuc 
", DKThang_Quy, ReportModels.DieuKien_NganSach(MaND), DKDuyet1, DKDuyet2);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy != "1")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("dNgay", NgayChiTieu);
            DataTable dtNhomDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtNhomDonVi.Rows.Count > 0)
            {
                DataRow R = dtNhomDonVi.NewRow();
                R["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000001";
                R["TenDM"] = "Chọn tất cả nhóm";
                dtNhomDonVi.Rows.InsertAt(R, 0);
                DataRow R1 = dtNhomDonVi.NewRow();
                R1["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000002";
                R1["TenDM"] = "--Chọn nhóm--";
                dtNhomDonVi.Rows.InsertAt(R1, 0);
            }
            else
            {
                DataRow R = dtNhomDonVi.NewRow();
                R["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000002";
                R["TenDM"] = "Không có nhóm";
                dtNhomDonVi.Rows.InsertAt(R, 0);
            }
            return dtNhomDonVi;
        }
        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDanhMuc,String MaND)
        {
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            String DKiID_MaDanhMuc = "";
            if (iID_MaDanhMuc == "00000000-0000-0000-0000-000000000001")
            {
                DKiID_MaDanhMuc = "";
            }
            else if (iID_MaDanhMuc == "00000000-0000-0000-0000-000000000002")
            {

                DKiID_MaDanhMuc = " AND iID_MaDanhMuc='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                DKiID_MaDanhMuc = " AND iID_MaDanhMuc=@iID_MaDanhMuc";
            }
          
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "(iThang_Quy between 1  and 3)";
                        break;
                    case 2: DKThang_Quy = "(iThang_Quy between 4  and 6)";
                        break;
                    case 3: DKThang_Quy = "(iThang_Quy between 7  and 9)";
                        break;
                    case 4: DKThang_Quy = "(iThang_Quy between 10  and 12)";
                        break;
                }
            }
            else
            {
                DKThang_Quy = "iThang_Quy<=@ThangQuy";
            }
            String DKDuyet1 = "", DKDuyet2 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                DKDuyet2 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();

            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String SQL = String.Format(@"SELECT b.sTen as TenDV,b.sTen AS sTen,a.iID_MaDonVi 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM QTA_ChungTuChiTiet
	                                           WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {2} {3}
	                                                 AND ({0}) AND bLoaiThang_Quy=0) a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc WHERE 1=1 {1}) c
                                        ON b.iID_MaNhomDonVi=c.iID_MaDanhMuc
                                        
                                        UNION

                                         SELECT b1.sTen as TenDV,b1.sTen AS sTen,a1.iID_MaDonVi 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM PB_PhanBoChiTiet
	                                           WHERE  iTrangThai=1 {2} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
                                                     AND sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {2} {4}
	                                                ) a1
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE  iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b1
                                        ON a1.iID_MaDonVi=b1.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc WHERE 1=1 {1}) c1
                                        ON b1.iID_MaNhomDonVi=c1.iID_MaDanhMuc 
", DKThang_Quy, DKiID_MaDanhMuc, ReportModels.DieuKien_NganSach(MaND), DKDuyet1,DKDuyet2);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy != "1")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("dNgay", NgayChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iID_MaDanhMuc);
           
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //if (dtDonVi.Rows.Count > 0)
            //{
            //    DataRow R = dtDonVi.NewRow();
            //    R["iID_MaDonVi"] = "-1";
            //    R["TenDV"] = "Chọn tất cả đơn vị";
            //    dtDonVi.Rows.InsertAt(R, 0);
            //    DataRow R1 = dtDonVi.NewRow();
            //    R1["iID_MaDonVi"] = "-2";
            //    R1["TenDV"] = "--Chọn đơn vị--";
            //    dtDonVi.Rows.InsertAt(R1, 0);
            //}
            //else
            //{
            //    DataRow R = dtDonVi.NewRow();
            //    R["iID_MaDonVi"] = "-2";
            //    R["TenDV"] = "Không có đơn vị";
            //    dtDonVi.Rows.InsertAt(R, 0);
            //}
            return dtDonVi;
        }
        /// <summary>
        /// quyết toán thường xuyên 23-2
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <returns></returns>
        public DataTable QuyetToan_TX_23_2(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaDanhMuc, String MaND, String TruongTien, String LoaiIn)
        {
            DataTable dt= new DataTable();
            if (String.IsNullOrEmpty(iID_MaDonVi))
                iID_MaDonVi = Guid.Empty.ToString();
            //Set DKDonVi
            String DKDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            if (LoaiIn=="TongHop")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < arrDonVi.Length - 1)
                        DKDonVi += " OR ";
                }
                DKDonVi = " AND(" + DKDonVi + ")";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKThang_Quy = "";
            //nếu là quý
            if (LoaiThang_Quy == "1")
            {
                switch (iThangQuy)
                {
                    case 1: DKThang_Quy = "(iThang_Quy=1 OR iThang_Quy=2 OR iThang_Quy=3)";
                        break;
                    case 2: DKThang_Quy = "(iThang_Quy=4 OR iThang_Quy=5 OR iThang_Quy=6)";
                        break;
                    case 3: DKThang_Quy = "(iThang_Quy=7 OR iThang_Quy=8 OR iThang_Quy=9)";
                        break;
                    case 4: DKThang_Quy = "(iThang_Quy=10 OR iThang_Quy=11 OR iThang_Quy=12)";
                        break;
                }
               

            }
            else
            {
                DKThang_Quy = "iThang_Quy=@dNgay";
            }
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe,SUM(rSoNguoi) as rNguoi,SUM(rNgay) as rNgay
                                        FROM( SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rSoNguoi,rNgay
                                        ,DotNay=Case WHEN ({0}) THEN SUM(rTuChi) ELSE 0 END
                                        ,LuyKe=Case WHEN (iThang_Quy<=@dNgay) THEN SUM(rTuChi) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND sNG<>'' AND  iTrangThai=1 {2} AND bLoaiThang_Quy=0 {1} {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy,rSoNguoi,rNgay) a
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(LuyKe)<>0 OR SUM(DotNay)<>0", DKThang_Quy, DKDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet1);
                SqlCommand cmd = new SqlCommand(SQL);

                cmd.Parameters.AddWithValue("@dNgay", NgayChiTieu);
                if (LoaiIn == "TongHop")
                {
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            
           
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
            }
            else
            {
                DKDuyet = "";
            }
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
                                                 AND sLNS=1010000 AND sL=460 AND sK= 468 AND sNG<>'' AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay AND PB_PhanBoChiTiet.iTrangThai=1 {0} {1}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", DKDonVi, DKDuyet);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
            cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmdChiTieu.Parameters.AddWithValue("@dNgay", iThangQuy);
           if(LoaiIn=="TongHop")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                }
            }
            else
               cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            if (iID_MaTrangThaiDuyet == "0")
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }
            DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();

            //ghep 2 dt
             DataRow addR, R2;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
                String[] arrCol = sCol.Split(',');
                dt.Columns.Add("ChiTieu", typeof(Decimal));         
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"NguonNS={7} AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"],dtChiTieu.Rows[i]["NguonNS"]
                                                      );
                    DataRow[] R = dt.Select(xauTruyVan);

                    if (R == null || R.Length == 0)
                    {
                        addR = dt.NewRow();
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
                        }
                        dt.Rows.Add(addR);
                    }
                    else
                    {
                        foreach (DataRow R1 in dtChiTieu.Rows)
                        {

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                Boolean okTrung = true;
                                R2 = dt.Rows[j];

                                for (int c = 0; c < arrCol.Length - 2; c++)
                                {
                                    if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                    {
                                        okTrung = false;
                                        break;
                                    }
                                }
                                if (okTrung)
                                {
                                    dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

                                    break;
                                }

                            }
                        }

                    }

                }
                //sắp xếp datatable sau khi ghép
                DataView dv = dt.DefaultView;
                dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG";
                dt = dv.ToTable();
                
            return dt;
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
        public static DataTable LoaiIn()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "0";
            R1[1] = "Chi tiet don vi";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            R2[1] = "Tong hop";
            DataRow R3 = dt.NewRow();
            return dt;
        }
    }
}
