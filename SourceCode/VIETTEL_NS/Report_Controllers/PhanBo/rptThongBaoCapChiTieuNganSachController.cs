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

namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptThongBaoCapChiTieuNganSachController : Controller
    {
        //
        // GET: /rptThongBaoCapChiTieuNganSach/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KieuTrang = "", String Muc = "")
        {
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                if (Muc == "1")
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_Doc.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_Doc_TieuNganh.xls";
                }
            }
            else
            {
                if (Muc == "1")
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_TieuNganh.xls";
                }
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptThongBaoCapChiTieuNganSach_Fillter.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy các giá trị trên form khi thực hiện Action Sumbmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String ThongBao = Request.Form[ParentID + "_ThongBao"];
            String Muc = Request.Form[ParentID + "_Muc"];
            String KieuTrang = Request.Form[ParentID + "_KieuTrang"];
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["ThongBao"] = ThongBao;
            ViewData["Muc"] = Muc;
            ViewData["KieuTrang"] = KieuTrang;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptThongBaoCapChiTieuNganSach_Fillter.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public long Tong = 0;
        public DataTable rptThongbaocapchitieungansach(String MaND, String iID_MaDotPhanBo, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String ThongBao, String Muc, String KieuTrang)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }

            DataTable dtDotPhanBo = LayDotPhanBo1(MaND, iID_MaTrangThaiDuyet);
            DataTable dtLuyKe = null;
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == Convert.ToString(dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"]))
                {
                    String DK = "";
                    String S = "";
                    for (int j = 1; j <= i; j++)
                    {

                        if (j == 1)
                        {
                            DK = " iID_MaDotPhanBo = " + "'" + Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"] + "'");
                        }
                        else
                        {
                            S += "OR " + "iID_MaDotPhanBo= '" + dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString() + "'";
                            DK = "(iID_MaDotPhanBo = " + "'" + Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"] + "'") + S + ")";
                        }
                    }
                    SqlCommand cmdLuyKe = new SqlCommand();
                    String DK_Duyet_LuyKe = ReportModels.DieuKien_TrangThaiDuyet;
                    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                    {
                        DK_Duyet_LuyKe = "";
                    }
                    else
                    {
                        cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    }

                    if (ThongBao == "1")
                    {
                        String SQLLuyKe = String.Format(@"SELECT SUBSTRING(ChiTiet.sLNS,1,1) as NguonNS,ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sTNG,ChiTiet.sMoTa,
                                                    SUM(rTuChi) as rTuChi,SUM(ChiTiet.rHienVat) as rHienVat,SUM(rTuChi)+SUM(rHienVat) as TongSo
                                                    FROM PB_PhanBoChiTiet as ChiTiet
                                                    WHERE  ChiTiet.iID_MaDonVi=@iID_MaDonVi AND ({0})  AND sNG<>'' {1} AND ChiTiet.iTrangThai=1 {2}
                                                    GROUP BY SUBSTRING(ChiTiet.sLNS,1,1),ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sTNG,ChiTiet.sMoTa
                                                    HAVING SUM(rTuChi)+ SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc,sTNG asc", DK, DK_Duyet_LuyKe, ReportModels.DieuKien_NganSach(MaND));
                        cmdLuyKe.CommandText = SQLLuyKe;
                        cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        dtLuyKe = Connection.GetDataTable(cmdLuyKe);
                        for (int k = 0; k < dtLuyKe.Rows.Count; k++)
                        {
                            Tong += long.Parse(dtLuyKe.Rows[k]["TongSo"].ToString());
                        }
                        cmdLuyKe.Dispose();
                    }
                    else
                    {
                        String SQLLuyKe = String.Format(@"SELECT SUBSTRING(ChiTiet.sLNS,1,1) as NguonNS,ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sTNG,ChiTiet.sMoTa,
                                                    SUM(rTuChi) as rTuChi,SUM(ChiTiet.rHienVat) as rHienVat,SUM(rTuChi)+SUM(rHienVat) as TongSo
                                                    FROM PB_PhanBoChiTiet as ChiTiet
                                                    WHERE  ChiTiet.iID_MaDonVi=@iID_MaDonVi AND ({0}) AND sNG<>'' {1} AND (rTuChi <= 0 AND rHienVat<=0) AND ChiTiet.iTrangThai=1 {2}
                                                    GROUP BY SUBSTRING(ChiTiet.sLNS,1,1),ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sTNG,ChiTiet.sMoTa
                                                    HAVING SUM(rTuChi)+ SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc,sTNG asc", DK, DK_Duyet_LuyKe, ReportModels.DieuKien_NganSach(MaND));
                        cmdLuyKe.CommandText = SQLLuyKe;
                        cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        dtLuyKe = Connection.GetDataTable(cmdLuyKe);
                        for (int k = 0; k < dtLuyKe.Rows.Count; k++)
                        {
                            Tong += long.Parse(dtLuyKe.Rows[k]["TongSo"].ToString());
                        }
                        cmdLuyKe.Dispose();
                    }
                }
            }
            if (dtLuyKe == null)
            {
                String SQL = @"SELECT SUBSTRING(ChiTiet.sLNS,1,1) as NguonNS,ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sTNG,ChiTiet.sMoTa,
                                                    SUM(rTuChi) as rTuChi,SUM(ChiTiet.rHienVat) as rHienVat,SUM(rTuChi)+SUM(rHienVat) as TongSo
                                                    FROM PB_PhanBoChiTiet as ChiTiet WHERE 1=0
                                GROUP BY SUBSTRING(ChiTiet.sLNS,1,1),ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sTNG,ChiTiet.sMoTa
                                                    HAVING SUM(rTuChi)+ SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc,sTNG asc";
                dtLuyKe = Connection.GetDataTable(SQL);
            }

            return dtLuyKe;

        }


        /// <summary>
        /// Hàm lấy tên đơn vị theo mã
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
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDotPhanBo, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String ThongBao, String Muc, String KieuTrang)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            DataTable dtDotPhanBo = LayDotPhanBo(MaND);
            String tendot = "";
            for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    try
                    {
                        DateTime ngay = Convert.ToDateTime(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"]);
                        tendot = "Tháng " + ngay.ToString("MM") + " Năm " + ngay.ToString("yyyy");
                    }
                    catch
                    { tendot = ""; }
                }
            }
            String Dot = String.Format("ĐỢT {0} : {1} ", ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo) , tendot);

            String tendv = "";
            //Lấy tên đơn vị hiển thị ra báo cáo
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongBaoCapChiTieuNganSach");
            LoadData(fr, MaND, iID_MaDotPhanBo, iID_MaDonVi, iID_MaTrangThaiDuyet, ThongBao, Muc, KieuTrang);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
            if (ThongBao == "1")
            {
                fr.SetValue("TieuDe", "THÔNG BÁO CẤP CHỈ TIÊU NGÂN SÁCH NĂM ");
            }
            else
            {
                fr.SetValue("TieuDe", "THÔNG BÁO THU CHỈ TIÊU NGÂN SÁCH NĂM ");
            }
            fr.SetValue("TenDV", tendv);
            if (iID_MaDotPhanBo == dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"].ToString())
            {
                fr.SetValue("NgayPhanBo", "");
                fr.SetValue("STT", "1");
            }
            else
            {
                fr.SetValue("NgayPhanBo", "ĐẾN " + Dot);
                fr.SetValue("STT", ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo) - 1);
            }
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDotPhanBo, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String ThongBao, String Muc, String KieuTrang)
        {
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            DataTable data = rptThongbaocapchitieungansach(MaND, iID_MaDotPhanBo, iID_MaDonVi, iID_MaTrangThaiDuyet, ThongBao, Muc, KieuTrang);
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

        }
        /// <summary>
        /// hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaDotPhanBo, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String ThongBao, String Muc, String KieuTrang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                if (Muc == "1")
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_Doc.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_Doc_TieuNganh.xls";
                }
            }
            else
            {
                if (Muc == "1")
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_TieuNganh.xls";
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDotPhanBo, iID_MaDonVi, iID_MaTrangThaiDuyet, ThongBao, Muc, KieuTrang);
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
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDotPhanBo, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String ThongBao, String Muc, String KieuTrang)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                if (Muc == "1")
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_Doc.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_Doc_TieuNganh.xls";
                }
            }
            else
            {
                if (Muc == "1")
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoCapChiTieuNganSach_TieuNganh.xls";
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaDotPhanBo, iID_MaDonVi, iID_MaTrangThaiDuyet, ThongBao, Muc, KieuTrang);
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
        /// Hàm lấy đợt phân bổ theo năm làm việc
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable LayDotPhanBo(String MaND)
        {
            SqlCommand cmd = new SqlCommand();

            String SQL = "SELECT dbo.PB_DotPhanBo.iID_MaDotPhanBo,dNgayDotPhanBo";
            SQL += " FROM dbo.PB_PhanBoChiTiet INNER JOIN  dbo.PB_DotPhanBo ON dbo.PB_PhanBoChiTiet.iID_MaDotPhanBo = dbo.PB_DotPhanBo.iID_MaDotPhanBo";
            SQL += " WHERE 1=1 " + ReportModels.DieuKien_NganSach(MaND, "PB_PhanBoChiTiet");
            SQL += " GROUP BY  dbo.PB_DotPhanBo.iID_MaDotPhanBo, dbo.PB_DotPhanBo.dNgayDotPhanBo";
            SQL += " ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm lấy đợt phân bổ theo năm làm việc
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable LayDotPhanBo1(String MaND, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }

            String SQL = "SELECT dbo.PB_DotPhanBo.iID_MaDotPhanBo,convert(nvarchar,dNgayDotPhanBo,103) as dNgayDotPhanBo";
            SQL += " FROM dbo.PB_PhanBoChiTiet INNER JOIN  dbo.PB_DotPhanBo ON dbo.PB_PhanBoChiTiet.iID_MaDotPhanBo = dbo.PB_DotPhanBo.iID_MaDotPhanBo";
            SQL += " WHERE 1=1 " + ReportModels.DieuKien_NganSach(MaND, "PB_PhanBoChiTiet") + DK_Duyet;
            SQL += " GROUP BY  dbo.PB_DotPhanBo.iID_MaDotPhanBo, dbo.PB_DotPhanBo.dNgayDotPhanBo";
            SQL += " ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotPhanBo"] = Guid.Empty;
                dr["dNgayDotPhanBo"] = "Không có đợt phân bổ";
                dt.Rows.InsertAt(dr, 0);
            }

            else
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotPhanBo"] = Guid.Empty;
                dr["dNgayDotPhanBo"] = "--Chọn đợt phân bổ--";
                dt.Rows.InsertAt(dr, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Json lấy danh sách đợt phân bổ từ obj_DSDotPhanBo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>

        public class ThongBaoChiNganSach
        {
            public String iID_MaDonPhanBo { get; set; }
            public String iID_MaDonVi { get; set; }
        }
        [HttpGet]
        public JsonResult ds_DonVi(String ParentID, String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            return Json(obj_DSDonVi(ParentID, MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        public ThongBaoChiNganSach obj_DSDonVi(String ParentID, String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            ThongBaoChiNganSach _data = new ThongBaoChiNganSach();

            //String input = "";
            DataTable dtDonvi = DSDonVi(ParentID, MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strDonVi = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 90%\"size='6' tabindex='-1'");
            _data.iID_MaDonVi = strDonVi;

            //Dot Phan Bo
            DataTable dtDotPhanBo = LayDotPhanBo1(MaND, iID_MaTrangThaiDuyet);
            SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            String strDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 95%;height:23px;\"onchange=ChonDonVi()");
            _data.iID_MaDonPhanBo = strDotPhanBo;
            return _data;
        }

        /// <summary>
        /// Hàm lấy dữ liệu theo năm và tháng đổ vào commbox
        /// </summary>
        /// <param name="Thang_Quy"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable DSDonVi(String ParentID, String MaND, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = string.Format(@" SELECT DISTINCT PB.iID_MaDonVi,DV.sTen
                                        FROM PB_PhanBoChiTiet AS PB
                                        INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON PB.iID_MaDonVi=DV.MaDonVi
                                        WHERE PB.iTrangThai=1 {0} {1}
                                        AND iID_MaDotPhanBo=@iID_MaDotPhanBo
                                        ORDER BY iID_MaDonVi", DK_Duyet, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDonVi"] = "";
                dr["sTen"] = "không có đơn vị";
                dt.Rows.InsertAt(dr, 0);
            }
            cmd.Dispose();

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
