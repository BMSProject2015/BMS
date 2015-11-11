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

namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptPhanBo_16D_TheoNganhController : Controller
    {
        //
        // GET: /rptPhanBo_16D_TheoNganh/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/PhanBo/rptPhanBo_16D_TheoNganh.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_16D_TheoDonVi.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_16D_TheoNganh.aspx";
                ViewData["PageLoad"] = "0";
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
            String DotPhanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaDotPhanBo"]);
            String LuyKe = Convert.ToString(Request.Form[ParentID + "_iLuyKe"]);
            String opDonViNganh = Request.Form[ParentID + "_opDonViNganh"];
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["DotPhanBo"] = DotPhanBo;
            ViewData["LuyKe"] = LuyKe;
            ViewData["opDonViNganh"] = opDonViNganh;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_16D_TheoNganh.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, DotPhanBo = DotPhanBo, LuyKe = LuyKe, opDonViNganh = opDonViNganh });
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn</param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="DotPhanBo">Mã đợt phân bổ</param>
        /// <param name="LuyKe">Lũy kế</param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String DotPhanBo, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);

            Result.Open(path);
            String TenDot = "";
            String TenTT = "";
            if (LuyKe == "on")
            {
                TenTT = "Đến đợt: " + ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, DotPhanBo);
            }
            else
            {
                TenTT = "Đợt " + ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, DotPhanBo);
            }
            if (!String.IsNullOrEmpty(DotPhanBo))
            {
                TenDot = Convert.ToString(CommonFunction.LayTruong("PB_DotPhanBo", "iID_MaDotPhanBo", DotPhanBo, "dNgayDotPhanBo"));
               
                if (!String.IsNullOrEmpty(TenDot))
                {
                    DateTime dt = Convert.ToDateTime(TenDot);
                    TenDot = Convert.ToString(CommonFunction.LayXauNgay(dt));
                    
                    TenDot = " Tháng " + TenDot.Substring(3, 2) + " năm " + TenDot.Substring(6, 4);
                }
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_16D_TheoNganh");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, DotPhanBo, LuyKe, opDonViNganh);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("TenDot", TenDot);
            fr.SetValue("TenTT", TenTT);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// tạo các range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String DotPhanBo, String LuyKe, String opDonViNganh)
        {
            tableData _tableData = PB_TongHopNganSach_TheoNganh(MaND, iID_MaTrangThaiDuyet, DotPhanBo, LuyKe, opDonViNganh);
            DataTable data;
            DataTable dtChiTieu = _tableData.dtChiTieu;
            DataTable dtTieuMuc;
            if (opDonViNganh == "DonVi")
            {
                 data = _tableData.dtPhanBo;
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                DataTable dtTieuTietMuc;
                dtTieuTietMuc = HamChung.SelectDistinct("TieuTietMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG", "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
                fr.AddTable("TieuTietMuc", dtTieuTietMuc);

                dtTieuTietMuc.Columns.Add("TuChiChiTieu", typeof(Decimal));
                dtTieuTietMuc.Columns.Add("HienVatChiTieu", typeof(Decimal));
                DataRow addR;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,TuChiChiTieu,HienVatChiTieu";
                String[] arrCol = sCol.Split(',');
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"]
                                                      );
                    DataRow[] R = dtTieuTietMuc.Select(xauTruyVan);

                    if (R == null || R.Length == 0)
                    {
                        addR = dtTieuTietMuc.NewRow();
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
                        }
                        dtTieuTietMuc.Rows.Add(addR);
                    }
                    else
                    {
                        foreach (DataRow R1 in R)
                        {
                            dtTieuTietMuc.Rows[dtTieuTietMuc.Rows.IndexOf(R1)]["TuChiChiTieu"] = dtChiTieu.Rows[i]["TuChiChiTieu"];
                            dtTieuTietMuc.Rows[dtTieuTietMuc.Rows.IndexOf(R1)]["HienVatChiTieu"] = dtChiTieu.Rows[i]["HienVatChiTieu"];
                        }

                    }

                }
                dtTieuMuc = HamChung.SelectDistinct("TieuMuc", dtTieuTietMuc, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                fr.AddTable("TieuMuc", dtTieuMuc);
            }
            else
            {
                data = _tableData.dtChiTieu;
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                fr.AddTable("TieuMuc", dtTieuMuc);
            }       
                        
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa,sL,sK", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            if (dtNguonNS.Rows.Count <= 0)
            {
                DataRow r = dtNguonNS.NewRow();
                r[0] = "";
                dtNguonNS.Rows.Add(r);
            }
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String DotPhanBo, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = "";
            if (opDonViNganh == "Nganh") DuongDanFile = sFilePath;
            else DuongDanFile = sFilePath1;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DotPhanBo, LuyKe, opDonViNganh, iID_MaTrangThaiDuyet);
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
        /// View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public FileContentResult ViewPDF(String DotPhanBo, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = "";
            if (opDonViNganh == "Nganh") DuongDanFile = sFilePath;
            else DuongDanFile = sFilePath1;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DotPhanBo, LuyKe, opDonViNganh, iID_MaTrangThaiDuyet);
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
        public clsExcelResult ExportToExcel(String DotPhanBo, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = "";
            if (opDonViNganh == "Nganh") DuongDanFile = sFilePath;
            else DuongDanFile = sFilePath1;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DotPhanBo, LuyKe, opDonViNganh, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "PhanBo16D.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String DotPhanBo)
        {

            return Json(obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, DotPhanBo), JsonRequestBehavior.AllowGet);
        }


        public String obj_DSDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String DotPhanBo)
        {
            String sdsDotPhanBo = "";
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet);
            SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            sdsDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, DotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" ");
            return sdsDotPhanBo;
        }

        public class tableData
        {
            public DataTable dtChiTieu { get; set; }
            public DataTable dtPhanBo { get; set; }
        }
        /// <summary>
        /// phân bổ 16D theo ngành
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public tableData PB_TongHopNganSach_TheoNganh(String MaND, String iID_MaTrangThaiDuyet, String DotPhanBo, String LuyKe, String opDonViNganh)
        {
            tableData _tableData = new tableData();
            DataTable dtChiTieu;
            DataTable dtPhanBo;
            #region//nếu loại báo cáo đơn vị
            if (opDonViNganh == "DonVi")
            {
                SqlCommand cmd = new SqlCommand();
                SqlCommand cmdChiTieu = new SqlCommand();
                String DK_Duyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
                if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                {
                    DK_Duyet = "";
                }
                else
                {
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                }
                String DK = "";
                if (LuyKe == "on")
                {
                    DK += "iID_MaDotPhanBo in (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo <=(SELECT dNgayDotPhanBo FROM PB_DotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo))";
                }
                else
                {
                    DK += "iID_MaDotPhanBo in (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo =(SELECT dNgayDotPhanBo FROM PB_DotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo))";
                }              
                String SQL = "SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa";
                SQL += ",NS_DonVi.sTen";
                SQL += ",NS_DonVi.iID_MaDonVi";
                SQL += ",SUM(rTuChi) as rTuChi,SUM(rHienVat) as rHienVat";
                SQL += " FROM PB_PhanBoChiTiet";
                SQL += " INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON NS_DonVi.iID_MaDonVi=PB_PhanBoChiTiet.iID_MaDonVi";
                SQL += " WHERE  {0} AND sNG<>'' {1} " + DK_Duyet;
                SQL += " GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa,NS_DonVi.sTen,NS_DonVi.iID_MaDonVi";
                SQL += " HAVING SUM(rTuChi) <> 0 OR SUM(rHienVat)<>0";
                SQL += " ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc";
                SQL = String.Format(SQL, DK, ReportModels.DieuKien_NganSach(MaND, "PB_PhanBoChiTiet"));
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                dtPhanBo = Connection.GetDataTable(cmd);
                cmd.Dispose();

                String SQLChiTieu = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                                    SUM(rTuChi) as TuChiChiTieu,SUM(rHienVat) as HienVatChiTieu,sTen=0,iID_MaDonVi=0
                                                    FROM PB_ChiTieuChiTiet
                                                    WHERE  ({0}) AND sNG<>'' {1} {2}
                                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DK, ReportModels.DieuKien_ChiTieu(MaND), DK_Duyet);
                cmdChiTieu.CommandText = SQLChiTieu;
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();
               
            }
            #endregion
            #region // báo cáo theo ngành
            else
            {
                DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
                SqlCommand cmdLuyKe = new SqlCommand();
                //nếu chọn lũy kế
                String DKDotPhanBo = "";
                if (LuyKe == "on")
                {
                    
                    for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (DotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            DKDotPhanBo = "";
                          
                            for (int j = 1; j <= i; j++)
                            {
                                DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                                if (j < i)
                                    DKDotPhanBo += " OR ";
                            }
                            break;
                        }

                    }
                }
                else
                {
                    DKDotPhanBo = "IID_MaDotPhanBo=@IID_MaDotPhanBo";
                }
                if (String.IsNullOrEmpty(DKDotPhanBo))
                {
                    DKDotPhanBo="iID_MaDotPhanBo='"+Guid.Empty.ToString()+"'";
                }
                String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                {
                    DK_Duyet = "";
                }
                else
                {
                    cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                }
                String SQLLuyKe = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                                    SUM(rTuChi) as rTuChi,SUM(rHienVat) as rHienVat
                                                    FROM PB_PhanBoChiTiet
                                                    WHERE  ({0}) AND sNG<>'' {1} {2}
                                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DKDotPhanBo, ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
                cmdLuyKe.CommandText=SQLLuyKe;
                if (LuyKe == "on")
                {
                    for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (DotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 1; j <= i; j++)
                            {
                                cmdLuyKe.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                }
                else
                {
                    cmdLuyKe.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                }
                dtPhanBo = Connection.GetDataTable(cmdLuyKe);
                cmdLuyKe.Dispose();

                //Tạo dt chi tiêu

                SqlCommand cmdChiTieu = new SqlCommand();
                String DK_Duyet_ChiTIeu = ReportModels.DieuKien_TrangThaiDuyet;
                if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                {
                    DK_Duyet_ChiTIeu = "";
                }
                else
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                }
                String SQLChiTieu = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                                    SUM(rTuChi) as TuChiChiTieu,SUM(rHienVat) as HienVatChiTieu
                                                    FROM PB_ChiTieuChiTiet
                                                    WHERE  ({0}) AND sNG<>'' {1} {2}
                                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DKDotPhanBo,ReportModels.DieuKien_ChiTieu(MaND),DK_Duyet_ChiTIeu);
                cmdChiTieu.CommandText=SQLChiTieu;
                if (LuyKe == "on")
                {
                    for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (DotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (int j = 1; j <= i; j++)
                            {
                                cmdChiTieu.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                }
                else
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                }
                 dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();
                #region //ghép dtphanbo vao dtchitieu
                DataRow addR, R2;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTuChi,rHienVat";
                String[] arrCol = sCol.Split(',');

                dtChiTieu.Columns.Add("rTuChi", typeof(Decimal));
                dtChiTieu.Columns.Add("rHienVat", typeof(Decimal));
                for (int i = 0; i < dtPhanBo.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtPhanBo.Rows[i]["sLNS"], dtPhanBo.Rows[i]["sL"],
                                                      dtPhanBo.Rows[i]["sK"],
                                                      dtPhanBo.Rows[i]["sM"], dtPhanBo.Rows[i]["sTM"],
                                                      dtPhanBo.Rows[i]["sTTM"], dtPhanBo.Rows[i]["sNG"], dtPhanBo.Rows[i]["NguonNS"]
                                                      );
                    DataRow[] R = dtChiTieu.Select(xauTruyVan);

                    if (R == null || R.Length == 0)
                    {
                        addR = dtChiTieu.NewRow();
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtPhanBo.Rows[i][arrCol[j]];
                        }
                        dtChiTieu.Rows.Add(addR);
                    }
                    else
                    {
                        foreach (DataRow R1 in dtPhanBo.Rows)
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
                                    dtChiTieu.Rows[j]["rTuChi"] = R1["rTuChi"];
                                    dtChiTieu.Rows[j]["rHienVat"] = R1["rHienVat"];
                                    break;
                                }

                            }
                        }

                    }

                }
                //sắp xếp datatable sau khi ghép
                DataView dv = dtChiTieu.DefaultView;
                dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG";
                dtChiTieu = dv.ToTable();
                #endregion
            }
            #endregion

            _tableData.dtChiTieu = dtChiTieu;
            _tableData.dtPhanBo = dtPhanBo;
            return _tableData;
        }              
    }
}

