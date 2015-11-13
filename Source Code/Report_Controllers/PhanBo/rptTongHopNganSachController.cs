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
    public class rptTongHopNganSachController : Controller
    {
        //
        // GET: /TongHopNganSach/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/PhanBo/rptTongHopNganSach.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptTongHopNganSach_Fillter.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Hàm Lấy các giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaDotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptTongHopNganSach_Fillter.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public long Tong = 0;
        public DataTable rptTongHopNganSach(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            DataTable dtLuyKe = new DataTable();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            DataTable dtPhanBo = dtDotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet);
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DKDotPhanBo = "";
            String S = "";
               if (dtPhanBo.Rows.Count > 0)
              {
                for (int i = 0; i < dtPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == Convert.ToString(dtPhanBo.Rows[i]["iID_MaDotPhanBo"]))
                    {
                       
                            if (i == 0)
                            {
                                DKDotPhanBo = "iID_MaDotPhanBo = " + "'" + Convert.ToString(dtPhanBo.Rows[0]["iID_MaDotPhanBo"] + "'");
                            }
                            else
                            {
                                S += " OR " + "iID_MaDotPhanBo= '" + dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString() + "'";
                                DKDotPhanBo = "(iID_MaDotPhanBo = " + "'" + Convert.ToString(dtPhanBo.Rows[0]["iID_MaDotPhanBo"] + "'") + S + ") ";
                            }
                        }

                    }
               }
               else
               {
                   DKDotPhanBo = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
               }
                    String SQLLuyKe = String.Format(@" SELECT SUBSTRING(ChiTiet.sLNS,1,1) as NguonNS,ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sMoTa,
                                                    SUM(rTuChi) as rTuChi,SUM(ChiTiet.rHienVat) as rHienVat,SUM(rTuChi)+SUM(rHienVat) as rTongSoLuyKe
                                                    FROM PB_PhanBoChiTiet as ChiTiet
                                                    WHERE {0}  AND ({1}) AND  sNG<>'' AND ChiTiet.iTrangThai=1 {2} {3}
                                                    GROUP BY SUBSTRING(ChiTiet.sLNS,1,1),ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sMoTa
                                                    HAVING SUM(rTuChi)+ SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DKDotPhanBo,DKLNS,DK_Duyet,ReportModels.DieuKien_NganSach(MaND));
                    SqlCommand cmdLuyKe = new SqlCommand(SQLLuyKe);
                    cmdLuyKe.Parameters.AddWithValue("iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    for (int i = 0; i < arrLNS.Length; i++)
                    {
                        cmdLuyKe.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                    }
                   
                    dtLuyKe = Connection.GetDataTable(cmdLuyKe);
                    String GiaTri = "";
                    for (int i = 0; i < dtLuyKe.Rows.Count; i++)
                    {
                        GiaTri = Convert.ToString(dtLuyKe.Rows[i]["rTongSoLuyKe"]);
                        Tong += long.Parse(GiaTri);
                    }
            
                        cmdLuyKe.Dispose();
                    return dtLuyKe;

         }
        /// <summary>
        /// Hàm lấy đợt phân bổ
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable dtDotPhanBo(String MaND, String sLNS, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            String DK_Duyet = " AND dbo.PB_PhanBoChiTiet.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
             String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = string.Format(@"SELECT dbo.PB_DotPhanBo.iID_MaDotPhanBo, Convert(varchar,dbo.PB_DotPhanBo.dNgayDotPhanBo,103) as dNgayDotPhanBo
                            FROM dbo.PB_PhanBoChiTiet  
                            INNER JOIN  dbo.PB_DotPhanBo ON dbo.PB_PhanBoChiTiet.iID_MaDotPhanBo = dbo.PB_DotPhanBo.iID_MaDotPhanBo 
                            WHERE  ({0}) {1} {2}
                            GROUP BY  dbo.PB_DotPhanBo.iID_MaDotPhanBo, dbo.PB_DotPhanBo.dNgayDotPhanBo,dbo.PB_DotPhanBo.iNamLamViec
                            ORDER BY MONTH(dNgayDotPhanBo),DAY(dNgayDotPhanBo)", DKLNS, DK_Duyet, ReportModels.DieuKien_NganSach(MaND, "dbo.PB_PhanBoChiTiet"));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
           
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDotPhanBo"] = Guid.Empty.ToString();
                R["dNgayDotPhanBo"] = "Không có dữ liệu";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
       
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path,String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
           
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }

            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            String tendot = "";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    try
                    {
                        DateTime ngay = Convert.ToDateTime(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString());
                        tendot = "  Tháng  " + ngay.ToString("MM") + "  Năm  " + ngay.ToString("yyyy");
                    }
                    catch { tendot = ""; }
                }
            }
            String Dot = String.Format("Đợt {0} : {1} ", ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo), tendot);

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTongHopNganSach");
            LoadData(fr, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("TenTieuDe", "TỔNG HỢP NGÂN SÁCH");
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("sLNS", sLNS);
                //fr.SetValue("Tien", "");
                fr.SetValue("NgayPhanBo", Dot);
                fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            DataTable data = rptTongHopNganSach(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet);
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
            dtTieuMuc.Dispose();
            dtLoaiNS.Dispose();
            dtNguonNS.Dispose();
            dtMuc.Dispose();

        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND,String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND,sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet);
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
        /// Hàm View PDf
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet);
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
        /// Lấy Loại Ngân Sách
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DtLoaiNganSach(String MaND)
        {
            DataTable dt = new DataTable();
           
            String SQL = string.Format(@"SELECT DISTINCT sLNS,sLNS+'-'+ sMoTa as sTen
                                        FROM PB_PhanBoChiTiet
                                        WHERE sL='' AND iTrangThai=1 {0} AND LEN(sLNS)=7
                                        ORDER BY sLNS", ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// Lấy Đợt Phân Bổ
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable LayDotPhanBo(String MaND)
        {
            String SQL = "SELECT dbo.PB_DotPhanBo.iID_MaDotPhanBo, Convert(varchar,dbo.PB_DotPhanBo.dNgayDotPhanBo,103) as dNgayDotPhanBo,dbo.PB_DotPhanBo.iNamLamViec";
            SQL += " FROM dbo.PB_PhanBoChiTiet INNER JOIN  dbo.PB_DotPhanBo ON dbo.PB_PhanBoChiTiet.iID_MaDotPhanBo = dbo.PB_DotPhanBo.iID_MaDotPhanBo";
            SQL += " WHERE 1 = 1 " + ReportModels.DieuKien_NganSach(MaND);
            SQL += " GROUP BY  dbo.PB_DotPhanBo.iID_MaDotPhanBo, dbo.PB_DotPhanBo.dNgayDotPhanBo,dbo.PB_DotPhanBo.iNamLamViec";
            SQL += " ORDER BY dNgayDotPhanBo asc";
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }



        [HttpGet]
        public JsonResult ds_DotPhanBo(String ParentID,String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DSDotPhanBo(ParentID, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDotPhanBo(String ParentID, String MaND, String sLNS,String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet)
        {
            //String input = "";
            DataTable dt = dtDotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet);
            SelectOptionList slDotPhanBo = new SelectOptionList(dt, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            String strDonVi = MyHtmlHelper.DropDownList(ParentID, slDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\"");
            return strDonVi;

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



