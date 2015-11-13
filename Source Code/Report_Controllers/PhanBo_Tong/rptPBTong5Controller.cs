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

namespace VIETTEL.Report_Controllers.PhanBo_Tong
{
    public class rptPBTong5Controller : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/PhanBo_Tong/rptPBTong5.xls";       
       /// <summary>
        /// Index
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/PhanBo_Tong/rptPBTong5.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
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
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];          
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String DotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String TruongTien = Request.Form[ParentID + "_TruongTien"];
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["sLNS"] = sLNS;
            ViewData["DotPhanBo"] = DotPhanBo;
            ViewData["TruongTien"] = TruongTien;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo_Tong/rptPBTong5.aspx";
             return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
         public ExcelFile CreateReport(String path, String sLNS, String DotPhanBo, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String s=null;
            DataTable dt = MoTa(sLNS);
            if (dt.Rows.Count>0)
            {
                 s= dt.Rows[0][0].ToString();
            }
            String TenDot = "",STTDot="";
            if (!String.IsNullOrEmpty(DotPhanBo))
            {
                TenDot = Convert.ToString(CommonFunction.LayTruong("PB_DotPhanBo", "iID_MaDotPhanBo", DotPhanBo, "dNgayDotPhanBo"));
                if (!String.IsNullOrEmpty(TenDot))
                {
                    TenDot = " - Tháng " + TenDot.Substring(3, 2) + " - Năm " + TenDot.Substring(6, 4);
                }
            }
            STTDot = ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, DotPhanBo).ToString();            
             //lấy ngày hiện tại
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String Phong = ReportModels.CauHinhTenDonViSuDung(3);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTongHopChiTieu");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, sLNS, DotPhanBo, TruongTien);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("sLNS", s);
                fr.SetValue("TenDot", TenDot);
                fr.SetValue("STTDot", STTDot);
                fr.SetValue("TruongTien", TruongTien);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("Phong", Phong);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Lấy mô tả
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
        /// tạo các Range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String DotPhanBo, String TruongTien)
        {
            DataTable data = PB_TongHopChiTieu(MaND, iID_MaTrangThaiDuyet, sLNS, DotPhanBo, TruongTien);
           
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);        
        }
        /// <summary>
        /// Xuất ra pdf
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String sLNS, String DotPhanBo, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sLNS, DotPhanBo, TruongTien, iID_MaTrangThaiDuyet);
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
        /// ExportToExcel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String sLNS, String DotPhanBo, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = sFilePath;
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sLNS, DotPhanBo, TruongTien, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptPhanBo_TongHopChiTieu.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String sLNS, String DotPhanBo, String TruongTien, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), sLNS, DotPhanBo, TruongTien, iID_MaTrangThaiDuyet);
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
        /// Onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo)
        {

            return Json(obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDotPhanBo), JsonRequestBehavior.AllowGet);
        }

        public String obj_DSDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo)
            {
                DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet, sLNS);              
                SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
                String strdsDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\"");
                return strdsDotPhanBo;
            }
       /// <summary>
        /// PB_TongHopChiTieu
       /// </summary>
       /// <param name="NamLamViec"></param>
       /// <param name="sLNS"></param>
       /// <param name="DotPhanBo"></param>
       /// <param name="TruongTien"></param>
       /// <returns></returns>
        public static DataTable PB_TongHopChiTieu(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String DotPhanBo, String TruongTien)
        {
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet);          
            String MaDotPhanBoDauTien="", MaDotPhanBoKyNay="", DKDauKy = "";
            if (dtDotPhanBo.Rows.Count > 1)
            {
                MaDotPhanBoDauTien = dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
                if (MaDotPhanBoDauTien != DotPhanBo)
                    MaDotPhanBoKyNay = DotPhanBo;
            }
            if (dtDotPhanBo.Rows.Count > 3)
            {
                for (int i =3; i < dtDotPhanBo.Rows.Count;i++)
                {
                    if (dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString() == DotPhanBo)
                    {
                        for (int j = 2; j < i; j++)
                        {
                            DKDauKy += @"iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo)" + j;
                            if (j < i - 1)
                                DKDauKy += " OR ";
                        }
                        break;
                    }
                }
            }
            else
            {
                DKDauKy = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            if (String.IsNullOrEmpty(DKDauKy))
            {
                DKDauKy = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            if (String.IsNullOrEmpty(MaDotPhanBoDauTien))
            {
                MaDotPhanBoDauTien = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(MaDotPhanBoKyNay))
            {
                MaDotPhanBoKyNay = Guid.Empty.ToString();
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            String SQL = String.Format(@"SELECT  b.sTen, SUM(ChiTieuDauNam) as ChiTieuDauNam,SUM(KyNay) as KyNay,SUM(DauKy) as DauKy
                                        FROM(
                                        SELECT iID_MaDonVi
                                        ,ChiTieuDauNam=CASE WHEN iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@MaDotPhanBoDauTien))  THEN SUM({0}) ELSE 0 END
                                        ,KyNay=CASE WHEN  iID_MaPhanBo IN (
																	SELECT iID_MaPhanBo
																    FROM PB_PhanBo_PhanBo 
																    WHERE iID_MaPhanBoTong IN (
																		SELECT iID_MaPhanBo 
																		FROM PB_PhanBo 
																		WHERE 1=1 AND iID_MaDotPhanBo=@MaDotPhanBoKyNay)) THEN SUM({0}) ELSE 0 END
                                        ,DauKy=CASE WHEN ({1}) THEN SUM({0}) ELSE 0 END
                                        FROM PB_PhanBoChiTiet
                                        WHERE iTrangThai=1 AND sLNS=@sLNS AND sNG<>'' {2} {3}
                                        GROUP BY iID_MaDotPhanBo,iID_MaDonVi,iID_MaPhanBo
                                        HAVING SUM({0})<>0 
                                        ) as A
                                        INNER JOIN (SELECT iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        GROUP BY a.iID_MaDonVi,b.sTen
                                        HAVING SUM(ChiTieuDauNam) <>0 OR SUM(KyNay)<>0 OR SUM(DauKy)<>0", TruongTien, DKDauKy,ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@MaDotPhanBoDauTien", MaDotPhanBoDauTien);
            cmd.Parameters.AddWithValue("@MaDotPhanBoKyNay", MaDotPhanBoKyNay);
            for (int i = 3; i < dtDotPhanBo.Rows.Count; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo"+(i-1), dtDotPhanBo.Rows[i-1]["iID_MaDotPhanBo"].ToString());
            }
            cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}
