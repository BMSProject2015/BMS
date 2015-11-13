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
    public class rptCapPhat_79_3Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathMuc = "/Report_ExcelFrom/CapPhat/rptCapPhat_79_3_Muc.xls";
        private const String sFilePathTieuMuc = "/Report_ExcelFrom/CapPhat/rptCapPhat_79_3_TieuMuc.xls";
        private const String sFilePathNganh = "/Report_ExcelFrom/CapPhat/rptCapPhat_79_3_Nganh.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {        
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
                {
                    ViewData["PageLoad"] = "0";
                    ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_79_3.aspx";
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
            String iDM_MaLoaiCapPhat = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMuc"]);
            String dNgayCapPhat = Convert.ToString(Request.Form[ParentID + "_dNgayCapPhat"]);
            String LuyKe = Convert.ToString(Request.Form[ParentID + "_LuyKe"]);
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_MaLoai"]);

            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iDM_MaLoaiCapPhat"] = iDM_MaLoaiCapPhat;
            ViewData["dNgayCapPhat"] = dNgayCapPhat;
            ViewData["LuyKe"] = LuyKe;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_79_3.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, dNgayCapPhat = dNgayCapPhat, LuyKe = LuyKe, LoaiBaoCao = LoaiBaoCao });
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
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe, String LoaiBaoCao)
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
            TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));

            String LoaiCapPhat="";
            //nếu chọn toàn bộ loại cấp phát
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                LoaiCapPhat = "Toàn bộ loại kinh phí";
            }
            else
            {
                DataTable dtLoaiCapPhat = DanhSach_LoaiCapPhat();
                if (dtLoaiCapPhat.Rows.Count > 0)
                {
                    for (int i = 1; i < dtLoaiCapPhat.Rows.Count;i++)
                    {
                        if (iDM_MaLoaiCapPhat == Convert.ToString(dtLoaiCapPhat.Rows[i]["iID_MaDanhMuc"]))
                        {
                            LoaiCapPhat = Convert.ToString(dtLoaiCapPhat.Rows[i]["sTen"]);
                            break;
                        }
                    }
                }
                dtLoaiCapPhat.Dispose();
            }
            String Ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            String NgayCapPhat = dNgayCapPhat;
             if (dNgayCapPhat.StartsWith("--"))
             {
                 NgayCapPhat = "";
             }
            
            FlexCelReport fr = new FlexCelReport();
            
            fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_79_3");
            LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, iDM_MaLoaiCapPhat, LuyKe, LoaiBaoCao,MaND);              
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("Ngay", Ngay);
                fr.SetValue("LoaiCapPhat", LoaiCapPhat);
                fr.SetValue("NgayCapPhat", NgayCapPhat);
                fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2)); 
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
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe, String LoaiBaoCao,String MaND)
        {
            DataTable data = CapPhat_79_3(iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, iDM_MaLoaiCapPhat, LuyKe, LoaiBaoCao,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL");
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
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "Muc")
            {
                DuongDan = sFilePathMuc;
            }
            else if (LoaiBaoCao == "TieuMuc")
            {
                DuongDan = sFilePathTieuMuc;
            }
            else
            {
                DuongDan = sFilePathNganh;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, iDM_MaLoaiCapPhat, LuyKe, LoaiBaoCao);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CapPhat_79_3.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "Muc")
            {
                DuongDan = sFilePathMuc;
            }
            else if (LoaiBaoCao == "TieuMuc")
            {
                DuongDan = sFilePathTieuMuc;
            }
            else
            {
                DuongDan = sFilePathNganh;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, iDM_MaLoaiCapPhat, LuyKe, LoaiBaoCao);
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
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe, String LoaiBaoCao)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "Muc")
            {
                DuongDan = sFilePathMuc;
            }
            else if (LoaiBaoCao == "TieuMuc")
            {
                DuongDan = sFilePathTieuMuc;
            }
            else
            {
                DuongDan = sFilePathNganh;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, iDM_MaLoaiCapPhat, LuyKe, LoaiBaoCao);
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
            public string NgayCapPhat { get; set; }
            public string DonVi { get; set; }
            public string LuyKe { get; set; }
        }
        public JsonResult ds_NgayCapPhat(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe)
        {
            String MaND = User.Identity.Name;
            return Json(obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, iDM_MaLoaiCapPhat, LuyKe,MaND), JsonRequestBehavior.AllowGet);
        }
        public LCPData obj_NgayCapPhat(String ParentID, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe,String MaND)
        {
            
            LCPData _LCPData = new LCPData();
            DataTable dtNgayCapPhat = DanhSach_Ngay_CapPhat(iDM_MaLoaiCapPhat, iID_MaTrangThaiDuyet,MaND);
            SelectOptionList slNgayCapPhat = new SelectOptionList(dtNgayCapPhat, "dNgayCapPhat", "dNgayCapPhat");
            _LCPData.NgayCapPhat = MyHtmlHelper.DropDownList(ParentID, slNgayCapPhat, dNgayCapPhat, "dNgayCapPhat", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNCP()\" ");

            if (dNgayCapPhat.StartsWith("--"))
            {
                dNgayCapPhat = "01/01/2000";
            }

            DataTable dtDonVi = DanhSach_DonVi(iID_MaTrangThaiDuyet, dNgayCapPhat, LuyKe, iDM_MaLoaiCapPhat,MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            _LCPData.DonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi","", "class=\"input1_2\" style=\"width: 100%\"");          
            return _LCPData;
        }
        /// <summary>
        /// CapPhat_79_3
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="LuyKe"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public DataTable CapPhat_79_3(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String iDM_MaLoaiCapPhat, String LuyKe, String LoaiBaoCao,String MaND)
        {
            #region //Các đk lọc

            DataTable dtNgayCapPhat = DanhSach_Ngay_CapPhat(iDM_MaLoaiCapPhat, iID_MaTrangThaiDuyet,MaND);
            String DK_LoaiCapPhat ="", DK_LuyKe = "";
            if (dNgayCapPhat.StartsWith("--"))
            {
                dNgayCapPhat = "01/01/2000";
            }
            // điều kiện loại cấp phát
            DataTable dtLoaiCapPhat = DanhSach_LoaiCapPhat();
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtLoaiCapPhat.Rows.Count; i++)
                {
                    DK_LoaiCapPhat += " CP_CapPhatChiTiet.iDM_MaLoaiCapPhaT=@iDM_MaLoaiCapPhat" + i;
                    if (i < dtLoaiCapPhat.Rows.Count - 1)
                        DK_LoaiCapPhat += " OR ";
                }
                DK_LoaiCapPhat = " AND (" + DK_LoaiCapPhat + ")";
            }
            else
            {
                DK_LoaiCapPhat = " AND  CP_CapPhatChiTiet.iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
            }

            //Điều kiện đơn vị
            String DKDonVi = "";
            DataTable dtDonVi = DanhSach_DonVi(iID_MaTrangThaiDuyet, dNgayCapPhat, LuyKe, iDM_MaLoaiCapPhat,MaND);
            if (iID_MaDonVi == "-1")
            {
                for (int i = 2; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";
                }
                DKDonVi = " AND(" + DKDonVi + ")";
            }
            else if (iID_MaDonVi == "-2")
            {
                DKDonVi = " AND iID_MaDonVi='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            String DKDuyet = "", DKDuyetPB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
                DKDuyetPB = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            #endregion
            #region //Tạo datatable cấp phát

            String SQLCapPhat = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,SUM(DotNay) AS DotNay,SUM(DenDotNay) as DenDotNay
                                                FROM( 
                                                SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa                                   
                                                ,DotNay=CASE WHEN iID_MaCapPhat IN (SELECT  iID_MaCapPhat
                                                                                            FROM CP_CapPhat
                                                                                            WHERE dNgayCapPhat=@dNgayCapPhat AND 
                                                                                            iTrangThai=1  {4})
                                                        THEN SUM(rTuChi) ELSE 0 END 
                                                ,DenDotNay=CASE WHEN iID_MaCapPhat IN (SELECT  iID_MaCapPhat
                                                                                            FROM CP_CapPhat
                                                                                            WHERE dNgayCapPhat<=@dNgayCapPhat AND 
                                                                                            iTrangThai=1  {4})
                                                        THEN SUM(rTuChi) ELSE 0 END                                    
                                                FROM CP_CapPhatChiTiet
                                                WHERE 1=1 {3} AND sNG<>'' {1} {2}  {4} 
                                                GROUP BY  SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaCapPhat
                                                HAVING SUM(rTuChi)<>0) a
                                                GROUP BY  NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                                HAVING SUM(DenDotNay)<>0 or SUM(DotNay)<>0 ", DK_LuyKe, DKDonVi, DK_LoaiCapPhat, ReportModels.DieuKien_NganSach(MaND), DKDuyet);
            SqlCommand cmdCapPhat = new SqlCommand(SQLCapPhat);
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmdCapPhat.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 2; i < dtDonVi.Rows.Count; i++)
                {
                    cmdCapPhat.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }
           
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtLoaiCapPhat.Rows.Count; i++)
                {
                    cmdCapPhat.Parameters.AddWithValue("@iDM_MaLoaiCapPhat" + i, dtLoaiCapPhat.Rows[i]["iID_MaDanhMuc"]);
                }
            }
            else
            {
                cmdCapPhat.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            cmdCapPhat.Parameters.AddWithValue("@dNgayCapPhat", CommonFunction.LayNgayTuXau(dNgayCapPhat));               
            DataTable dtCapPhat = Connection.GetDataTable(cmdCapPhat);
            cmdCapPhat.Dispose();
#endregion
            #region //Tạo datatable chỉ tiểu
            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE dNgayDotPhanBo<=@dNgayCapPhat AND PB_PhanBoChiTiet.iTrangThai=1 {0} AND sNG<>'' {1} {2}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", DKDonVi,DieuKien_NganSach_DtChieuTieu(MaND),DKDuyetPB);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 2; i < dtDonVi.Rows.Count; i++)
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }
            cmdChiTieu.Parameters.AddWithValue("@dNgayCapPhat", CommonFunction.LayNgayTuXau(dNgayCapPhat));
            DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();
          #endregion

            #region  //Ghép DTChiTieu với dtCapPhat
            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,DotNay,DenDotNay";
            String[] arrCol = sCol.Split(',');

            dtChiTieu.Columns.Add("DotNay", typeof(Decimal));
            dtChiTieu.Columns.Add("DenDotNay", typeof(Decimal));
            for (int i = 0; i < dtCapPhat.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}' AND sTNG='{8}' ",
                                                  dtCapPhat.Rows[i]["sLNS"], dtCapPhat.Rows[i]["sL"],
                                                  dtCapPhat.Rows[i]["sK"],
                                                  dtCapPhat.Rows[i]["sM"], dtCapPhat.Rows[i]["sTM"],
                                                  dtCapPhat.Rows[i]["sTTM"], dtCapPhat.Rows[i]["sNG"], dtCapPhat.Rows[i]["NguonNS"], dtCapPhat.Rows[i]["sTNG"]
                                                  );
                DataRow[] R = dtChiTieu.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dtChiTieu.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtCapPhat.Rows[i][arrCol[j]];
                    }
                    dtChiTieu.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtCapPhat.Rows)
                    {

                        for (int j = 0; j < dtChiTieu.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dtChiTieu.Rows[j];

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
                                dtChiTieu.Rows[j]["DotNay"] = R1["DotNay"];
                                dtChiTieu.Rows[j]["DenDotNay"] = R1["DenDotNay"];
                                break;
                            }

                        }
                    }

                }
           
            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dtChiTieu.DefaultView;
            dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            dtChiTieu = dv.ToTable();
            #endregion
            return dtChiTieu;
        }
        /// <summary>
        /// Lấy danh sách ngày cấp phát
        /// </summary>
        /// <param name="iIDM_MaLoaiCapPhat"> Mã loại cấp phát</param>
        /// <returns></returns>
        public static DataTable DanhSach_Ngay_CapPhat(String iDM_MaLoaiCapPhat, String iID_MaTrangThaiDuyet,String MaND)
        {
            String DK_LoaiCapPhat = "";
            DataTable dtLoaiCapPhat = DanhSach_LoaiCapPhat();
            // Nều là chọn toàn bộ loại cấp phát
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtLoaiCapPhat.Rows.Count;i++ )
                {
                    DK_LoaiCapPhat += " iDM_MaLoaiCapPhaT=@iDM_MaLoaiCapPhat" + i;
                    if (i < dtLoaiCapPhat.Rows.Count - 1)
                        DK_LoaiCapPhat += " OR ";
                }
                DK_LoaiCapPhat = "(" + DK_LoaiCapPhat + ")";
            }
            else
            {
                DK_LoaiCapPhat = " iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"    SELECT DISTINCT dNgayCapPhat as Ngay,Convert(varchar(10),CP_CapPhat.dNgayCapPhat,103) as dNgayCapPhat  FROM(
                                        SELECT iID_MaCapPhat FROM CP_CapPhatChiTiet WHERE iTrangThai=1 {1} ) as CP_CapPhatChiTiet
                                        INNER JOIN (SELECT iID_MaCapPhat,dNgayCapPhat FROM CP_CapPhat WHERE itrangThai=1 AND {0} {2}) as CP_CapPhat
                                        ON CP_CapPhatChiTiet.iID_MaCapPhat=CP_CapPhat.iID_MaCapPhat
                                        ORDER BY Ngay", DK_LoaiCapPhat, ReportModels.DieuKien_NganSach(MaND), DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtLoaiCapPhat.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat" + i, dtLoaiCapPhat.Rows[i]["iID_MaDanhMuc"]);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count <= 0)
            {
                DataRow R = dt.NewRow();
                R["dNgayCapPhat"] = "--Không có ngày CP--";
                dt.Rows.InsertAt(R, 0);
               
            }
            else
            {
                DataRow R = dt.NewRow();
                R["dNgayCapPhat"] = "--Chọn ngày CP--";
                dt.Rows.InsertAt(R, 0);
            }
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm trả về danh sách loại báo cáo Muc,TM,NG
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1= dt.NewRow();                          
             dt.Rows.Add(R1);
             R1[0] = "Muc";
             R1[1] = "Đến Mục";
           DataRow R2= dt.NewRow();
            dt.Rows.Add(R2);
            R2[0]="TieuMuc";
            R2[1]="Đến Tiểu Mục";            
            DataRow R3= dt.NewRow();
            dt.Rows.Add(R3);
            R3[0]="Nganh";
            R3[1] = "Đến Ngành";
            dt.Dispose();
            return dt;           
        }
        /// <summary>
        /// Hàm trả về danh sách loại cấp phát
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_LoaiCapPhat()
        {
            DataTable dtLoaiCapPhat = CommonFunction.Lay_dtDanhMuc("LoaiCapPhat");
            DataRow R = dtLoaiCapPhat.NewRow();
            R["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000001";
            R["sTen"] = "Chọn tất cả";
            dtLoaiCapPhat.Rows.InsertAt(R, 0);

            DataRow R1 = dtLoaiCapPhat.NewRow();
            R1["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000000";
            R1["sTen"] = "-- Chọn loại cấp phát --";
            dtLoaiCapPhat.Rows.InsertAt(R1, 0);
            dtLoaiCapPhat.Dispose();
            return dtLoaiCapPhat;
        }
        /// <summary>
        /// Hàm trả về danh sách đơn vị
        /// </summary>
        /// <param name="NamLamVIec">Năm lằm việc</param>
        /// <param name="dNgayCapPhat">Ngày cấp phát</param>
        /// <param name="LuyKe"> Chọn lũy kế</param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String iID_MaTrangThaiDuyet, String dNgayCapPhat, String LuyKe, String iDM_MaLoaiCapPhat,String MaND)
        {
            if (dNgayCapPhat.StartsWith("--"))
            {
                dNgayCapPhat = "01/01/2000";
            }
            DataTable dtDonvi = new DataTable();
            String DK = "";
            if (LuyKe == "on")
            {
                DK = " AND CP_CapPhat.dNgayCapPhat<=@dNgayCapPhat";
            }
            else
            {
                DK = " AND CP_CapPhat.dNgayCapPhat=@dNgayCapPhat";
            }
            String DK_LoaiCapPhat = "";
            DataTable dtLoaiCapPhat = DanhSach_LoaiCapPhat();
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtLoaiCapPhat.Rows.Count; i++)
                {
                    DK_LoaiCapPhat += " CP_CapPhatChiTiet.iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat" + i;
                    if (i < dtLoaiCapPhat.Rows.Count - 1)
                        DK_LoaiCapPhat += " OR ";
                }
                DK_LoaiCapPhat = "AND (" + DK_LoaiCapPhat + ")";
            }
            else
            {
                DK_LoaiCapPhat = " AND CP_CapPhatChiTiet.iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
            }
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND CP_CapPhatChiTiet.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                DKDuyet = " ";
            }
            String iNamLamViec = DateTime.Now.Year.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,b.sTen
                                        FROM (SELECT CP_CapPhatChiTiet.iID_MaDonVi FROM CP_CapPhatChiTiet 
                                        INNER JOIN CP_CapPhat on CP_CapPhatChiTiet.iID_MaCapPhat=CP_CapPhat.iID_MaCapPhat
                                        WHERE 1=1 {0} {1} AND CP_CapPhatChiTiet.iID_MaDonVi NOT IN (00,99) AND CP_CapPhatChiTiet.iTrangThai=1 {2} {3}) as a
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as  b ON a.iID_MaDonVi=b.iID_MaDonVi
                                        GROUP BY  b.sTen,a.iID_MaDonVi", DK, DK_LoaiCapPhat, DieuKien_NganSach(MaND), DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@dNgayCapPhat",CommonFunction.LayNgayTuXau(dNgayCapPhat));
            if (iDM_MaLoaiCapPhat == "00000000-0000-0000-0000-000000000001")
            {
                for (int i = 1; i < dtLoaiCapPhat.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat" + i, dtLoaiCapPhat.Rows[i]["iID_MaDanhMuc"]);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
           
            dtDonvi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtDonvi.Rows.Count > 0)
            {
                DataRow R = dtDonvi.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả đơn vị";
                dtDonvi.Rows.InsertAt(R, 0);
                DataRow R1 = dtDonvi.NewRow();
                R1["iID_MaDonVi"] = Guid.Empty.ToString();
                R1["sTen"] = "-- Chọn đơn vị --";
                dtDonvi.Rows.InsertAt(R1, 0);
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
                                 AND (CP_CapPhatChiTiet.iID_MaDonVi IN ({4})))", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, iID_MaPhongBan,iID_MaDonVi);
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
