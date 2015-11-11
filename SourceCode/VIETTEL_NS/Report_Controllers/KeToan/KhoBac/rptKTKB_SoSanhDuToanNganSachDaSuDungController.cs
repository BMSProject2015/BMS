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
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Globalization;
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKB_SoSanhDuToanNganSachDaSuDungController : Controller
    {
        //
        // GET: /rpt_TCDN_SoSanhDuToanNganSachDaSuDung/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_SoSanhDuToanNganSachDaSuDung.xls";
       
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_SoSanhDuToanNganSachDaSuDung.aspx";
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
            ViewData["pageload"] = 1;       
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String MucIn = Convert.ToString(Request.Form[ParentID + "_MucIn"]);
            String LoaiThangQuy=Request.Form[ParentID + "_ThangQuy"];
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String GiaTriThangQuy = Quy;
            if(String.IsNullOrEmpty(Thang)==false)
                GiaTriThangQuy=Thang;
            ViewData["PageLoad"] = "1";
            ViewData["MucIn"] = MucIn;            
            ViewData["Thang"] = Thang;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["GiaTriThangQuy"] = GiaTriThangQuy;
            ViewData["Quy"] = Quy;
            ViewData["Thang"] = Thang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;  
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_SoSanhDuToanNganSachDaSuDung.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
  
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn đến file excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>       
        public ExcelFile CreateReport(String path, String MucIn, String Thang, String Quy, String Nam, String LoaiThangQuy, String MaND, String iID_MaTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);           
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_SoSanhDuToanNganSachDaSuDung");
            LoadData(fr,MucIn,Thang,Quy,Nam, LoaiThangQuy,MaND,iID_MaTrangThaiDuyet);
            String ThangQuy = "Quý " + Quy +" ";
            if (LoaiThangQuy == "Thang") ThangQuy = "Tháng " + Thang + " ";
            fr.SetValue("BoQuocPhong",ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("CucTaiChinh", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("MucIn", MucIn);
            fr.SetValue("Nam", Nam);
            fr.SetValue("ThangQuy", ThangQuy);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đẩy dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr">Đường dẫn tới file excel mẫu</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iAll">Lấy trong 1 quý hay tất cả các quý trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
       
        private void LoadData(FlexCelReport fr,String MucIn,String Thang, String Quy, String Nam,String LoaiThangQuy,String MaND,String iID_MaTrangThaiDuyet)
        {
            DataTable data = GET_dtData(MucIn, Thang, Quy, Nam, LoaiThangQuy, MaND, iID_MaTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtM = HamChung.SelectDistinct("Muc", data, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtM);
            DataTable dtLNS = HamChung.SelectDistinct("LNS", data, "sLNS", "sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtLNS);
            dtM.Dispose();
            dtLNS.Dispose();
            data.Dispose();
        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="NgayCT"></param>
        /// <param name="MaDN"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MucIn, String Thang, String Quy, String Nam, String LoaiThangQuy, String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MucIn, Thang, Quy, Nam, LoaiThangQuy, MaND, iID_MaTrangThaiDuyet);
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
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="All">Lấy trong 1 quý hoặc tất cả các quý trong năm</param>
        /// <param name="MaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MucIn, String Thang, String Quy, String Nam, String LoaiThangQuy, String MaND,String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MucIn, Thang, Quy, Nam, LoaiThangQuy, MaND, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CanDoiKeToan_" + Nam + ".xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy danh sách doanh nghiệp
        /// </summary>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm</param>
        /// <param name="LoaiThangQuy">Lấy doanh nghiệp theo năm hay theo quý và năm</param>
        /// <returns></returns>
        public static DataTable GET_dtData(String MucIn, String Thang, String Quy, String Nam, String LoaiThangQuy,String MaND,String iID_MaTrangThaiDuyet)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec=Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }

            String DK_TrongThang = "", DK_DenThang = "", DK_ChiTieu = "";
            String DKDV = "",DVN="",DKG="",DVNG="",DK7="";
            DVN = ",iID_MaDonVi_Nhan='',sTenDonVi_Nhan=''";
            DKDV = ",iID_MaDonVi_Nhan='',sTenDonVi_Nhan=''";
            DK7 = ",iID_MaDonVi_Nhan='',sTenDonVi_Nhan=''";
            if (MucIn == "DonVi")
            {
                DKDV = ",CT.iID_MaDonVi AS iID_MaDonVi_Nhan,sTen AS sTenDonVi_Nhan";
                DVN = ",iID_MaDonVi_Nhan,SUBSTRING(sTenDonVi_Nhan,charindex('-',sTenDonVi_Nhan)+2,100) AS sTenDonVi_Nhan";
                DVNG = ",iID_MaDonVi_Nhan,sTenDonVi_Nhan";
                DKG = ",CT.iID_MaDonVi,sTen";
                DK7 = ",iID_MaDonVi_Nhan,sTenDonVi_Nhan";
            }
            if (LoaiThangQuy == "Thang")
            {                
            
                DK_TrongThang = String.Format(" iThangCT ={0}", Thang);
                DK_DenThang = String.Format(" iThangCT <={0}", Thang);
                DK_ChiTieu = String.Format(" AND MONTH(dNgayDotRutDuToan) ={0}", Thang);
            }
            else
            {
                switch (Quy)
                {
                    case "1":
                        DK_TrongThang = " iThangCT <=3 ";
                        DK_DenThang = " iThangCT <=3 ";
                        DK_ChiTieu = " AND MONTH(dNgayDotRutDuToan) <=3 ";
                        break;
                    case "2":
                        DK_TrongThang = " iThangCT >=3 AND iThangCT <=6 ";
                        DK_ChiTieu = " AND MONTH(dNgayDotRutDuToan) >=3 AND  MONTH(dNgayDotRutDuToan) <= 6 ";
                        DK_DenThang = " iThangCT <=6 ";
                        break;
                    case "3":
                        DK_TrongThang = " iThangCT >=6 AND iThangCT <=9 ";
                        DK_DenThang = " iThangCT <=9 ";
                        DK_ChiTieu = " AND MONTH(dNgayDotRutDuToan) >=6 AND  MONTH(dNgayDotRutDuToan) <= 9 ";
                        break;
                    case "4":
                        DK_TrongThang = " iThangCT >=9 AND iThangCT <=12 ";
                        DK_DenThang = " iThangCT <=12 ";
                        DK_ChiTieu = " AND MONTH(dNgayDotRutDuToan) >=9 AND  MONTH(dNgayDotRutDuToan) <= 12 ";
                        break;
                }
            }
            DataTable dtDN = new DataTable();
            String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sMoTa {7}
                                       ,SUM(ISNULL(ChiTieu,0)) AS ChiTieu,SUM(TrongThang) AS TrongThang
                                        ,SUM(DenThang) AS DenThang
                                        ,ConLai=SUM(ISNULL(ChiTieu,0)) - SUM(ISNULL(DenThang,0))
                                        FROM (                                       
                                        SELECT sLNS,sL,sK,sM,sTM,sMoTa{3}
                                        ,ChiTieu=0
                                        ,TrongThang=SUM(case when {0} then rDTRut else 0 end)
                                        ,DenThang=SUM(case when {1} then rDTRut else 0 end)
                                        FROM KTKB_ChungTuChiTiet 
                                        WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND iID_MaNamNganSach=@iID_MaNamNganSach AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND sTM<>''
                                        {8}
                                        GROUP BY sLNS,sL,sK,sM,sTM,sMoTa{6}
                                        
                                        UNION
                                        SELECT sLNS,sL,sK,sM,sTM,CT.sMoTa{4},SUM(rSoTien) AS ChiTieu
                                        ,TrongThang=0
                                        ,DenThang=0
                                        FROM KT_RutDuToanChiTiet AS CT
                                        LEFT JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON NS_DonVi.iID_MaDonVi=CT.iID_MaDonVi
                                        WHERE iNamLamViec=@iNamLamViec AND iID_MaNamNganSach=@iID_MaNamNganSach AND iID_MaNguonNganSach=@iID_MaNguonNganSach {2} AND sTM<>''
                                        GROUP BY sLNS,sL,sK,sM,sTM,CT.sMoTa{5}
                                        HAVING SUM(rSoTien)>0                                                                              
                                        ) TB
                                        GROUP BY sLNS,sL,sK,sM,sTM,sMoTa{6}
                                        HAVING SUM(ISNULL(ChiTieu,0))>0 OR SUM(TrongThang)>0 OR SUM(DenThang)>0 ", DK_TrongThang, DK_DenThang, DK_ChiTieu, DVN, DKDV, DKG, DVNG, DK7, DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", Nam);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            dtDN = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDN;
        }

        public String get_sThangQuy(string ParentID, String LoaiThangQuy, String GiaTri)
        {
            DataTable dtThang = DanhMucModels.DT_Thang(false);
            SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
            DataTable dtQuy = DanhMucModels.DT_Quy();
            SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
            String ddlThang = MyHtmlHelper.DropDownList(ParentID, slThang, GiaTri, "iThang", "","style=\"width:100%;\"");
            String ddlQuy = MyHtmlHelper.DropDownList(ParentID, slQuy, GiaTri, "iQuy", "", "style=\"width:100%;\"");
            if (LoaiThangQuy == "Thang")
                return ddlThang;
            else
                return ddlQuy;
        }

        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Quy">Quý</param>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="All">Lấy doanh nghiệp trong năm</param>
        /// <param name="iMaDN">Mã doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsThangQuy(string ParentID,String Loai,String GiaTri)
        {
            DataTable dtThang = DanhMucModels.DT_Thang(false);
            SelectOptionList slThang= new SelectOptionList(dtThang,"MaThang","TenThang");
            DataTable dtQuy = DanhMucModels.DT_Quy();
            SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
            String ddlThang=MyHtmlHelper.DropDownList(ParentID, slThang, GiaTri, "iThang","","style=\"width:100%;\"");
            String ddlQuy = MyHtmlHelper.DropDownList(ParentID, slQuy, GiaTri, "iQuy", "","style=\"width:100%;\"");
            if(Loai=="Thang")
                return Json(ddlThang, JsonRequestBehavior.AllowGet);
            else
                return Json(ddlQuy, JsonRequestBehavior.AllowGet);
        }
    }
}