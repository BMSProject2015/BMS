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
    public class rptCapPhat_80_4Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePathMuc = "/Report_ExcelFrom/CapPhat/rptCapPhat_80_4_Muc.xls";
        private const String sFilePathTieuMuc = "/Report_ExcelFrom/CapPhat/rptCapPhat_80_4_TieuMuc.xls";
        private const String sFilePathNganh = "/Report_ExcelFrom/CapPhat/rptCapPhat_80_4_Nganh.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_80_4.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String dNgayCapPhat = Convert.ToString(Request.Form[ParentID + "_dNgayCapPhat"]);
            String LoaiBaoCao = Convert.ToString(Request.Form[ParentID + "_MaLoai"]);
            String LuyKe = Convert.ToString(Request.Form[ParentID + "_LuyKe"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["dNgayCapPhat"] = dNgayCapPhat;
            ViewData["LoaiBaoCao"] = LoaiBaoCao;
            ViewData["LuyKe"] = LuyKe;
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_80_4.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, dNgayCapPhat = dNgayCapPhat, LoaiBaoCao = LoaiBaoCao });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String LoaiBaoCao,String LuyKe)
        {
            String MaND = User.Identity.Name;
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
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
            String NgayCapPhat = dNgayCapPhat;
            if (dNgayCapPhat.StartsWith("--"))
            {
                NgayCapPhat = "";
            }
            String NgayThangNam = "";
            NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
                FlexCelReport fr = new FlexCelReport();
                 fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_80_4");
                 LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, LoaiBaoCao,MaND,LuyKe);
                fr.SetValue("Nam", iNamLamViec);
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("NgayCapPhat", NgayCapPhat);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// tạo các range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="LoaiBaoCao"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String LoaiBaoCao,String MaND,String LuyKe)
        {
            DataTable data = CapPhat_80_4(iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, LoaiBaoCao,MaND,LuyKe);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtNganh;
            dtNganh = HamChung.SelectDistinct("Nganh", data, "NguonNS,sLNS,sL,sK,sM,sTM,sNG", "NguonNS,sLNS,sL,sK,sM,sTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM,sNG");
            fr.AddTable("TieuMuc", dtNganh);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", dtNganh, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
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
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String LoaiBaoCao,String LuyKe)
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, LoaiBaoCao,LuyKe);
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
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String LoaiBaoCao,String LuyKe)
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, LoaiBaoCao,LuyKe);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "capphat_80_4.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String LoaiBaoCao,String LuyKe)
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
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, LoaiBaoCao,LuyKe);
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
        public class NLVData
        {
            public String NgayCapPhat { get; set; }
            public String iID_MaDonVi { get; set; }
        }
        [HttpGet]
        public JsonResult ds_NgayCapPhat(String ParentID, String iID_MaTrangThaiDuyet, String dNgayCapPhat, String iID_MaDonVi,String LuyKe)
        {
            String MaND = User.Identity.Name;
            return Json(obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, dNgayCapPhat, iID_MaDonVi,MaND,LuyKe), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ds_DonVi(String ParentID, String iID_MaTrangThaiDuyet, String dNgayCapPhat, String iID_MaDonVi,String MaND,String LuyKe)
        {
            return Json(obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, dNgayCapPhat, iID_MaDonVi, MaND, LuyKe), JsonRequestBehavior.AllowGet);
        }
        public NLVData obj_NgayCapPhat(String ParentID, String iID_MaTrangThaiDuyet, String dNgayCapPhat, String iID_MaDonVi, String MaND,String LuyKe)
        {
            NLVData _data = new NLVData();
            DataTable dtNgayCapPhat = DanhSach_Ngay_CapPhat(iID_MaTrangThaiDuyet,MaND);
            if (dNgayCapPhat.StartsWith("--") || String.Equals(dNgayCapPhat, "2000/01/01"))
            {
                dNgayCapPhat = "01/01/2000";
            }
            SelectOptionList slNgayCapPhat = new SelectOptionList(dtNgayCapPhat, "dNgayCapPhat", "dNgayCapPhat");
            _data.NgayCapPhat = MyHtmlHelper.DropDownList(ParentID, slNgayCapPhat, dNgayCapPhat, "dNgayCapPhat", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNCP()\"");
            DataTable dtDonVi = DanhSach_DonVi(iID_MaTrangThaiDuyet, dNgayCapPhat,MaND,LuyKe);
            SelectOptionList slDonvi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            _data.iID_MaDonVi = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            dtNgayCapPhat.Dispose();
            dtDonVi.Dispose();
            return _data;
        }
        /// <summary>
        /// cấp phát 80_4
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="LoaiBaoCao"></param>
        /// <returns></returns>
        public DataTable CapPhat_80_4(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String dNgayCapPhat, String LoaiBaoCao,String MaND,String LuyKe)
        {
            DataTable dtCapPhat = new DataTable();
            SqlCommand cmdCapPhat = new SqlCommand();
            SqlCommand cmdQuyetToan = new SqlCommand();
            SqlCommand cmdChiTieu = new SqlCommand();
            #region Tạo datatable cấp phát

            if (dNgayCapPhat.StartsWith("--") || String.Equals(dNgayCapPhat, "2000/01/01"))
            {
                dNgayCapPhat = "01/01/2000";
            }

            String DKDonVi = "";
            DataTable dtDonVi = DanhSach_DonVi(iID_MaTrangThaiDuyet, dNgayCapPhat, MaND,LuyKe);
            if (iID_MaDonVi == "-1")
            {
                for (int i = 2; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";
                }

            }
            else if (iID_MaDonVi == "-2")
            {
                DKDonVi = "iID_MaDonVi='" + Guid.Empty.ToString() + "'";
            }
            else
            {
                DKDonVi = "iID_MaDonVi=@iID_MaDonVi";
            }
            String DK_DuyetQT = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_DuyetQT = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_QT";
                cmdQuyetToan.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_QT", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            }
            else
            {
                DK_DuyetQT = " ";
            }
            String DK_DuyetCP = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_DuyetCP = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_CP";
                cmdCapPhat.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_CP", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat));
            }
            else
            {
                DK_DuyetCP = " ";
            }
            String DK_DuyetCT = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DK_DuyetCT = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_CT";
                cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_CT", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }
            else
            {
                DK_DuyetCT = " ";
            } 
            String SQLCapPhat = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,SUM(rTuChi)as SoDaCap 
											    FROM 	CP_CapPhatChiTiet
												WHERE 1=1  AND ({0}) {1} AND iTrangThai=1 AND sNG<>'' {2}
													  AND iID_MaCapPhat IN (SELECT iID_MaCapPhat 
																			FROM CP_CapPhat 
																			WHERE dNgayCapPhat<=@dNgayCapPhat
																				  AND iTrangThai=1  {2})
                                                      GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
													  HAVING SUM(rTuChi)<>0", DKDonVi, ReportModels.DieuKien_NganSach(MaND), DK_DuyetCP);
         
            cmdCapPhat.CommandText = SQLCapPhat;
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
            cmdCapPhat.Parameters.AddWithValue("@dNgayCapPhat", CommonFunction.LayNgayTuXau(dNgayCapPhat));

            dtCapPhat = Connection.GetDataTable(cmdCapPhat);
            cmdCapPhat.Dispose();
            #endregion
            #region Tạo datatable quyết toán
            DataTable dtQuyetToan = new DataTable();

            String SQLQuyetToan = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa, sum(SoDaQT) as SoDaQT
                                                            FROM (SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTu,
                                                            SoDaQT=SUM( case when iThang_Quy <> 0 AND iThang_Quy <= month(@dNgayChungTu) then rTuChi else 0 end)   
                                                            FROM 	QTA_ChungTuChiTiet
                                                            WHERE 1=1 AND {0} {1} AND iTrangThai=1 AND sNG<>'' {2}
                                                            GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTu
                                                            HAVING SUM(rTuChi)<>0) as a
                                                            INNER JOIN (SELECT iID_MaChungTu,dNgayChungTu FROM QTA_ChungTu WHERE dNgayChungTu<=@dNgayChungTu) as b
                                                            ON a.iID_MaChungTu=b.iID_MaChungTu 
                                                            WHERE sNG<>'' 
                                                            group by NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa ", DKDonVi, ReportModels.DieuKien_NganSach(MaND), DK_DuyetQT);

            cmdQuyetToan.CommandText = SQLQuyetToan;
            if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
            {
                cmdQuyetToan.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (iID_MaDonVi == "-1")
            {
                for (int i = 2; i < dtDonVi.Rows.Count; i++)
                {
                    cmdQuyetToan.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                }
            }
            cmdQuyetToan.Parameters.AddWithValue("@dNgayChungTu", CommonFunction.LayNgayTuXau(dNgayCapPhat));
           
            dtQuyetToan = Connection.GetDataTable(cmdQuyetToan);
            cmdQuyetToan.Dispose();
            #endregion
            #region Tạo datatable chỉ tiêu
            DataTable dtChiTieu = new DataTable();

            String SQLChiTieu = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,ChiTieu 
                                                FROM (SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaDotPhanBo,SUM(rTuChi)as ChiTieu 
													  FROM 	PB_PhanBoChiTiet
													  WHERE 1=1 AND {0} {1} AND iTrangThai=1 AND sNG<>'' {2}
													  GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaDotPhanBo
													  HAVING SUM(rTuChi)<>0) as a
												INNER JOIN (SELECT iID_MaDotPhanBo,dNgayDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo<=@dNgayDotPhanBo) as b
												ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo ", DKDonVi, ReportModels.DieuKien_NganSach(MaND),DK_DuyetCT);
        
            cmdChiTieu.CommandText = SQLChiTieu;
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
            cmdChiTieu.Parameters.AddWithValue("@dNgayDotPhanBo", CommonFunction.LayNgayTuXau(dNgayCapPhat));
            dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();
            #endregion
            #region Ghép cấp phát với quyết toán
            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,SoDaQT";
            String[] arrCol = sCol.Split(',');

            dtCapPhat.Columns.Add("SoDaQT", typeof(Decimal));
            for (int i = 0; i < dtQuyetToan.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}'
                                                    AND sM='{3}' AND sTM='{4}'
                                                      AND sTTM='{5}' AND sNG='{6}' AND sTNG='{8}'",
                                                  dtQuyetToan.Rows[i]["sLNS"], dtQuyetToan.Rows[i]["sL"],
                                                  dtQuyetToan.Rows[i]["sK"],
                                                  dtQuyetToan.Rows[i]["sM"], dtQuyetToan.Rows[i]["sTM"],
                                                  dtQuyetToan.Rows[i]["sTTM"], dtQuyetToan.Rows[i]["sNG"], dtQuyetToan.Rows[i]["NguonNS"], dtQuyetToan.Rows[i]["sTNG"]
                                                  );
                DataRow[] R = dtCapPhat.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dtCapPhat.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtQuyetToan.Rows[i][arrCol[j]];
                    }
                    dtCapPhat.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtQuyetToan.Rows)
                    {

                        for (int j = 0; j < dtCapPhat.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dtCapPhat.Rows[j];

                            for (int c = 0; c < arrCol.Length - 1; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dtCapPhat.Rows[j]["SoDaQT"] = R1["SoDaQT"];

                                break;
                            }

                        }
                    }
                }
            }
            #endregion
            #region Ghép Cấp phát,quyết toán với chỉ tiêu
            DataRow _addR;
            String _sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,ChiTieu";
            String[] _arrCol = _sCol.Split(',');

            dtCapPhat.Columns.Add("ChiTieu", typeof(Decimal));
            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                    AND sTTM='{5}' AND sNG='{6}' AND sTNG='{8}'",
                                                  dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                  dtChiTieu.Rows[i]["sK"],
                                                  dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                  dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"], dtChiTieu.Rows[i]["sTNG"]
                                                  );
                DataRow[] R = dtCapPhat.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    _addR = dtCapPhat.NewRow();
                    for (int j = 0; j < _arrCol.Length; j++)
                    {
                        _addR[_arrCol[j]] = dtChiTieu.Rows[i][_arrCol[j]];
                    }
                    dtCapPhat.Rows.Add(_addR);
                }
                else
                {
                    foreach (DataRow R1 in dtChiTieu.Rows)
                    {

                        for (int j = 0; j < dtCapPhat.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dtCapPhat.Rows[j];

                            for (int c = 0; c < _arrCol.Length - 1; c++)
                            {
                                if (R2[_arrCol[c]].Equals(R1[_arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dtCapPhat.Rows[j]["ChiTieu"] = R1["ChiTieu"];
                                break;
                            }

                        }
                    }

                }
                DataView dv = dtCapPhat.DefaultView;
                dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
                dtCapPhat = dv.ToTable();
            #endregion
            }
            return dtCapPhat;

        }
        /// <summary>
        /// Danh sách ngày cấp phát đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DanhSach_Ngay_CapPhat(String iID_MaTrangThaiDuyet,String MaND)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = String.Format(@"    SELECT DISTINCT dNgayCapPhat as Ngay,Convert(varchar(10),CP_CapPhat.dNgayCapPhat,103) as dNgayCapPhat  FROM(
                                        SELECT iID_MaCapPhat FROM CP_CapPhatChiTiet WHERE iTrangThai=1 {0} ) as CP_CapPhatChiTiet
                                        INNER JOIN (SELECT iID_MaCapPhat,dNgayCapPhat FROM CP_CapPhat WHERE itrangThai=1  {1}) as CP_CapPhat
                                        ON CP_CapPhatChiTiet.iID_MaCapPhat=CP_CapPhat.iID_MaCapPhat
                                        ORDER BY Ngay", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
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
            return dt;
        }
        /// <summary>
        /// danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String iID_MaTrangThaiDuyet, String dNgayCapPhat,String MaND,String LuyKe)
        {
            if (dNgayCapPhat.StartsWith("--") || String.Equals(dNgayCapPhat, "2000/01/01") )
            {
                dNgayCapPhat = "01/01/2000";
            }
            DataTable dtDonvi = new DataTable();

            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }

            String DKNgayCapPhat = "";
            if (LuyKe == "on") DKNgayCapPhat = "dNgayCapPhat<=@dNgayCapPhat";
            else DKNgayCapPhat = "dNgayCapPhat=@dNgayCapPhat";
                
            String SQL = String.Format(@"SELECT a.iID_MaDonVi,c.sTen
                                        FROM (SELECT DISTINCT  iID_MaDonVi,iID_MaCapPhat FROM CP_CapPhatChiTiet 
                                              WHERE iTrangThai=1 {0} {1} AND
                                                iID_MaDonVi NOT IN (00,99)) as A
                                        INNER JOIN(SELECT iID_MaCapPhat,dNgayCapPhat 
		                                             FROM CP_CapPhat
		                                             WHERE iTrangThai=1 AND {2} {1}) as B
                                        ON  A.iID_MaCapPhat=B.iID_MaCapPhat
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as C
                                        ON a.iID_MaDonVi=c.iID_MaDonVi
                                        GROUP BY  C.sTen,a.iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet,DKNgayCapPhat);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgayCapPhat", CommonFunction.LayNgayTuXau(dNgayCapPhat));
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat));
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
        /// Hàm trả về danh sách loại báo cáo Muc,TM,NG
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "Muc";
            R1[1] = "Đến Mục";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "TieuMuc";
            R2[1] = "Đến Tiểu Mục";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "Nganh";
            R3[1] = "Đến Ngành";
            dt.Dispose();
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
    }
}
