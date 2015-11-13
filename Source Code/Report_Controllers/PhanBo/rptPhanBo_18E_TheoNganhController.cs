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
    public class rptPhanBo_18E_TheoNganhController : Controller
    {
        //
        // GET: /rptPhanBo_16D_TheoNganh/
        public string sViewPath = "~/Report_Views/DuToan/";

        private const String sFilePath = "/Report_ExcelFrom/PhanBo/rptPhanBo_18E_TheoNganh.xls";

        private const String sFilePath1 = "/Report_ExcelFrom/PhanBo/rptPhanBo_18E_TheoDonVi.xls";

        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_18E_TheoNganh.aspx";
                ViewData["PageLoad"] = "0";
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
        public ActionResult EditSubmit(String ParentID)
        {
            String DotPhanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaDotPhanBo"]);
            String Nganh = Convert.ToString(Request.Form[ParentID + "_sNG"]);
            String LuyKe = Convert.ToString(Request.Form[ParentID + "_iLuyKe"]);
            String opDonViNganh = Request.Form[ParentID + "_opDonViNganh"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_18E_TheoNganh.aspx";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["DotPhanBo"] = DotPhanBo;
            ViewData["Nganh"] = Nganh;
            ViewData["LuyKe"] = LuyKe;
            ViewData["opDonViNganh"] = opDonViNganh;
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn</param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="DotPhanBo"> Mã đợt phân bổ</param>
        /// <param name="Nganh">Ngành</param>
        /// <param name="LuyKe">Chọn lũy kế</param>
        /// <param name="opDonViNganh">chọn in loại ngành hay đơn vị</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String DotPhanBo, String Nganh, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
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
                    TenDot = " Tháng " + TenDot.Substring(3, 2) + " năm " + TenDot.Substring(6, 4);
                }
            }
            String TenNganh;
            DataTable dtNganh = Get_Nganh(Nganh);
            if (dtNganh.Rows.Count > 0)
            {
                TenNganh = Convert.ToString(dtNganh.Rows[0]["sTenKhoa"]) + " - " + Convert.ToString(dtNganh.Rows[0]["sTen"]);
            }
            else
            {
                TenNganh = "";
            }
              String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_18E_TheoNganh");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, DotPhanBo, Nganh, LuyKe, opDonViNganh);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("TenDot", TenDot);
                fr.SetValue("TenTT", TenTT);
                fr.SetValue("TenNganh", TenNganh);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.Run(Result);
                return Result;
            
        }

        /// <summary>
        /// lấy dt ngành
        /// </summary>
        /// <param name="sNG"></param>
        /// <returns></returns>
        public DataTable Get_Nganh(String sNG)
        {
            if (String.IsNullOrEmpty(sNG))
            {
                sNG = Guid.Empty.ToString();
            }
            String SQL = String.Format(@"SELECT iID_MaDanhMuc, DC_DanhMuc.sTen,DC_DanhMuc.sTenKhoa 
                                        FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc 
                                        WHERE DC_DanhMuc.sTenKhoa=@sNg AND DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'{0}' ORDER BY iSTT", "Nganh");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sNg", sNG);
            DataTable dtNganh = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtNganh;
        }

        /// <summary>
        /// tạo các range fill báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="Nganh"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String DotPhanBo, String Nganh, String LuyKe, String opDonViNganh)
        {
            tableData _tableData = PB_TongHopNganSach_TheoNganh(MaND, iID_MaTrangThaiDuyet, DotPhanBo, Nganh, LuyKe, opDonViNganh);
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
                DataRow addR, R2;
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
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            if (dtNguonNS.Rows.Count <= 0)
            {
                DataRow r = dtNguonNS.NewRow();
                r[0] = "";
                dtNguonNS.Rows.Add(r);
            }
            fr.AddTable("NguonNS", dtNguonNS);

            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="Nganh"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String DotPhanBo, String Nganh, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = sFilePath;
            if (opDonViNganh == "DonVi") DuongDanFile = sFilePath1;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile),  DotPhanBo, Nganh, LuyKe, opDonViNganh, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopNganSach18E.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        /// <summary>
        /// xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="Nganh"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String DotPhanBo, String Nganh, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = sFilePath;
            if (opDonViNganh == "DonVi") DuongDanFile = sFilePath1;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DotPhanBo, Nganh, LuyKe, opDonViNganh, iID_MaTrangThaiDuyet);
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
        /// <param name="Nganh"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String DotPhanBo, String Nganh, String LuyKe, String opDonViNganh, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            if (opDonViNganh == "DonVi") DuongDanFile = sFilePath1;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), DotPhanBo, Nganh, LuyKe, opDonViNganh, iID_MaTrangThaiDuyet);
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
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND,String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo)
        {

            return Json(obj_DSDotPhanBo(ParentID, MaND,iID_MaTrangThaiDuyet, iID_MaDotPhanBo), JsonRequestBehavior.AllowGet);
        }

        public String obj_DSDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo)
        {
            String sdsDotPhanBo = "";
            DataTable dtDotPhanBo = LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            sdsDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\"");

            return sdsDotPhanBo;

        }
        public static DataTable LayDSDotPhanBo2(String MaND, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = "";
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
            }
            else
            {
                DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format(@"SELECT a.iID_MaDotPhanBo, dNgayDotPhanBo as NgayPhanBo, Convert(varchar,dNgayDotPhanBo,103) as dNgayDotPhanBo 
                                        FROM( SELECT  DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet WHERE iTrangThai=1 {1} {0}) as A
                                        INNER JOIN  (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM  PB_DotPhanBo WHERE iTrangThai=1) as B ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo
                                        ORDER BY NgayPhanBo", ReportModels.DieuKien_NganSach(MaND), DK_Duyet);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDotPhanBo"] = Guid.Empty.ToString();
                R["dNgayDotPhanBo"] = "Không có Dữ liệu";
                dt.Rows.InsertAt(R, 0);
            }
            dt.Dispose();
            return dt;
        }
        public class tableData
        {
            public DataTable dtChiTieu { get; set; }
            public DataTable dtPhanBo { get; set; }
        }
        /// <summary>
        /// PB_TongHopNganSach_TheoNganh
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="Nganh"></param>
        /// <param name="LuyKe"></param>
        /// <param name="opDonViNganh"></param>
        /// <returns></returns>
        public tableData PB_TongHopNganSach_TheoNganh(String MaND, String iID_MaTrangThaiDuyet, String DotPhanBo, String Nganh, String LuyKe, String opDonViNganh)
        {
            tableData _tableData = new tableData();
            DataTable dtPhanBo;
            DataTable dtChiTieu;
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);           
            String DK = "";
            if (opDonViNganh == "DonVi")
            {

                if (LuyKe == "on")
                {
                    DK += "iID_MaDotPhanBo in (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo <=(SELECT dNgayDotPhanBo FROM PB_DotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo))";
                }
                else
                {
                    DK += "iID_MaDotPhanBo in (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo =(SELECT dNgayDotPhanBo FROM PB_DotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo))";
                }
                //thêm điều kiện đã duyệt
                String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                {
                    DK_Duyet = "";
                }
                else
                {
                    DK += DK_Duyet;
                }
                String SQL = "SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa";
                SQL += ",NS_DonVi.sTen";
                SQL += ",NS_DonVi.iID_MaDonVi";
                SQL += ",SUM(rTuChi_ChiTieu) as rTuChi_ChiTieu,SUM(rHienVat_ChiTieu) as rHienVat_ChiTieu, SUM(rTuChi) as rTuChi,SUM(rHienVat) as rHienVat";
                SQL += " FROM PB_PhanBoChiTiet";
                SQL += " INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON NS_DonVi.iID_MaDonVi=PB_PhanBoChiTiet.iID_MaDonVi";
                SQL += " WHERE PB_PhanBoChiTiet.iTrangThai=1 AND {0} {1} AND sNG IN (SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@Nganh)";
                SQL += " GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa,NS_DonVi.sTen,NS_DonVi.iID_MaDonVi";
                SQL += " HAVING SUM(rTuChi) <> 0 OR SUM(rHienVat)<>0";
                SQL += " ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc";
                SQL = String.Format(SQL, DK, ReportModels.DieuKien_NganSach(MaND, "PB_PhanBoChiTiet"));
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                cmd.Parameters.AddWithValue("@Nganh", Nganh);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                dtPhanBo = Connection.GetDataTable(cmd);
                cmd.Dispose();

                //Tạo dt chi tiêu

                String SQLChiTieu = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                                    SUM(rTuChi) as TuChiChiTieu,SUM(rHienVat) as HienVatChiTieu
                                                    FROM PB_ChiTieuChiTiet
                                                    WHERE  ({0}) AND sNG<>'' {1} {2} AND sNG IN (SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@Nganh)
                                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DK, ReportModels.DieuKien_ChiTieu(MaND),DK_Duyet);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@Nganh", Nganh);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose(); 
              
            }
            else
            {

                if (LuyKe == "on")
                {
                    DK += "iID_MaDotPhanBo in (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo <=(SELECT dNgayDotPhanBo FROM PB_DotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo))";
                }
                else
                {
                    DK += "iID_MaDotPhanBo in (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE dNgayDotPhanBo =(SELECT dNgayDotPhanBo FROM PB_DotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo))";
                }
                String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                {
                    DK_Duyet = "";
                }
                String SQLLuyKe = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi) as rTuChi,SUM(rHienVat) as rHienVat
                                            FROM PB_PhanBoChiTiet
                                        WHERE  {0} {2} AND sNG<>'' AND iTrangThai=1 {1} AND sNG IN (SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@Nganh)
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rTuChi)<>0 OR  SUM(rHienVat)<>0                                       
                                        ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DK,DK_Duyet,ReportModels.DieuKien_NganSach(MaND));
                SqlCommand cmdLuyKe = new SqlCommand(SQLLuyKe);
                cmdLuyKe.Parameters.AddWithValue("@Nganh", Nganh);
                cmdLuyKe.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                dtPhanBo = Connection.GetDataTable(cmdLuyKe);
                cmdLuyKe.Dispose();
                //Tạo dt chi tiêu

                String SQLChiTieu = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                                    SUM(rTuChi) as TuChiChiTieu,SUM(rHienVat) as HienVatChiTieu
                                                    FROM PB_ChiTieuChiTiet
                                                    WHERE  ({0}) {2} AND sNG<>'' {1} AND sNG IN (SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@Nganh)
                                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                    HAVING SUM(rTuChi)<>0 OR SUM(rHienVat)<>0                                       
                                                    ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DK,DK_Duyet,ReportModels.DieuKien_ChiTieu(MaND));
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDotPhanBo", DotPhanBo);
                cmdChiTieu.Parameters.AddWithValue("@Nganh", Nganh);
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
            _tableData.dtPhanBo = dtPhanBo;
            _tableData.dtChiTieu = dtChiTieu;
            return _tableData;
        }
    }
}

