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
using DomainModel.Controls;
namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDT_ChiNganSachSD_II_02Controller : Controller
    {
        //
        // GET: /rptDT_ChiNganSachSD_II_02/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
       // private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDT_ChiNganSachSD_II_02.xls";
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(String Check = "")
        {
            String sFilePath = "";
            if (Check == "off" )
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_ChiNganSachSD_II_02.xls";
            }
            else 
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_Chi_SuDungNganSachNam.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDT_ChiNganSachSD_II_02.aspx";
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
            String iID_MaDanhMuc = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMuc"]);
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String Check = Convert.ToString(Request.Form[ParentID + "_Check"]);
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Check"] = Check;
            ViewData["path"] = "~/Report_Views/DuToan/rptDT_ChiNganSachSD_II_02.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi,iID_MaNhomDonVi=iID_MaNhomDonVi,sLNS=sLNS, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet ,Check= Check});
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet, String Check)
        {

            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String tendv = "";
            String Ten = "";
            
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable TenNS = MoTa(sLNS);
            if (TenNS.Rows.Count > 0)
            {
                Ten = TenNS.Rows[0][0].ToString();
            }
            tendv = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            String NamTruoc = (Convert.ToInt32(iNamLamViec) - 1).ToString();
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDT_ChiNganSachSD_II_02");
            LoadData(fr, MaND, iID_MaDonVi, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet, Check);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("TenDV", tendv);
            fr.SetValue("Ten", Ten);
            fr.SetValue("iID_MaDonVi", iID_MaDonVi);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("NamTruoc", NamTruoc);
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("CucTaiChinh", ReportModels.CauHinhTenDonViSuDung(2));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable tendonvi(String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }            
            String SQL = string.Format(@"SELECT DISTINCT 
                                        BH.iID_MaDonVi,DV.sTen
                                        FROM DT_ChungTuChiTiet as BH
                                        inner join (Select * from NS_DonVi where iNamLamViec_DonVi={2}) as DV on BH.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE BH.iTrangThai=1  {1} 
                                         {0}  
                                        ORDER BY iID_MaDonVi", iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND), NguoiDungCauHinhModels.iNamLamViec);
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
        /// <summary>
        /// tạo range trong báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet, String Check)
        {
            DataTable data = rptDT_ChiNganSachSD_II_02(MaND, iID_MaDonVi, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet, Check);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM" , "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtNS;
            dtNS = HamChung.SelectDistinct("LNS", dtMuc, "sLNS", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL");
            fr.AddTable("LNS", dtNS);
            data.Dispose();
            dtMuc.Dispose();
            dtTieuMuc.Dispose();
            dtNS.Dispose();
        }

        /// <summary>
        /// xuất ra PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet, String Check)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (Check == "off")
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_ChiNganSachSD_II_02.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_Chi_SuDungNganSachNam.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet, Check);
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
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet, String Check)
        {
            HamChung.Language();
            String sFilePath = "";
            if (Check == "off")
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_ChiNganSachSD_II_02.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_Chi_SuDungNganSachNam.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet, Check);
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
        /// Xuát ra Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet, String Check)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (Check == "off")
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_ChiNganSachSD_II_02.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/DuToan/rptDT_Chi_SuDungNganSachNam.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet, Check);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptDT_ChiNganSachSD_II_02.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable rptDT_ChiNganSachSD_II_02(String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet, String Check)
        {
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            String[] arrLNS = sLNS.Split(',');
            String DK = "";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                // DK += "DV.iID_MaDonVi=" + "'" + "@iID_MaDonVi" + i + "'";
                if (i > 0)
                    DK += " OR ";
                DK += "sLNS= @sLNS" + i;

            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                sLNS = Guid.Empty.ToString();
            }
            String DKDV = "";
            DataTable dtDonVi = DanhSachDonVi(MaND, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet);
            String[] arrDonVi = iID_MaDonVi.Split(',');
            if (iID_MaDonVi == "-1")
            {
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    DKDV += "iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDV += " OR ";
                }
            }
            else
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    // DK += "DV.iID_MaDonVi=" + "'" + "@iID_MaDonVi" + i + "'";
                    if (i > 0)
                        DKDV += " OR ";
                    DKDV += "iID_MaDonVi= @iID_MaDonVi" + i;

                }
            }
            dtDonVi.Dispose();
            DataTable dt = new DataTable();
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else if (iID_MaTrangThaiDuyet == "2")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet<>'" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String SQL = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rTongSoNamTruoc) as DuToan,SUM(rUocThucHien) as UocThucHien,";
            SQL += " SUM(rTongSo) as TongSo,SUM(rTuChi) as rTuChi ,SUM(rHienVat) as rHienVat";
            SQL += " FROM DT_ChungTuChiTiet ";
            SQL += " WHERE  ({2}) {0} AND iTrangThai=1 AND ({3})  {1}";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa ";
            SQL += " HAVING SUM(rTongSoNamTruoc)<>0 OR SUM(rUocThucHien)<>0 OR SUM(rTongSo)<>0 OR SUM(rTuChi)<>0 OR SUM(rHienVat)<>0";
            SQL = String.Format(SQL, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DK, DKDV);
            SqlCommand cmd = new SqlCommand(SQL);
            if (iID_MaDonVi == "-1")
            {
               for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                }
            }
            else
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                }
            }
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>

        public class LNSdata
        {
            public string iID_MaDanhMuc { get; set; }
            public string iID_MaDonVi { get; set; }
        }
        public JsonResult ds_NhomDonVi(String ParentID, String MaND, String iID_MaDonVi, String iID_MaDanhMuc, String sLNS, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonVi(ParentID,MaND,iID_MaDonVi,iID_MaDanhMuc,sLNS,iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public LNSdata obj_DonVi(String ParentID, String MaND, String iID_MaDonVi, String iID_MaDanhMuc,String sLNS, String iID_MaTrangThaiDuyet)
        {
            LNSdata _LNSdata = new LNSdata();
            #region Nhóm đơn vị
            DataTable dtNhomDonVi = DS_NhomDonVi(MaND,sLNS,iID_MaTrangThaiDuyet);
            SelectOptionList slNhomDonVi = new SelectOptionList(dtNhomDonVi, "iID_MaDanhMuc", "TenDM");
            _LNSdata.iID_MaDanhMuc = MyHtmlHelper.DropDownList(ParentID, slNhomDonVi, iID_MaDanhMuc, "iID_MaDanhMuc", "", "class=\"input1_2\" style=\"width: 85%\" onchange=\"ChonDV()\"");
            dtNhomDonVi.Dispose();
            #endregion
            #region Đơn vị
            DataTable dtDonVi = DanhSachDonVi(MaND,iID_MaDanhMuc,sLNS,iID_MaTrangThaiDuyet);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenDV");
            _LNSdata.iID_MaDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 85%\"");
            dtDonVi.Dispose();
            #endregion
            return _LNSdata;
        }
        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String MaND, String iID_MaDanhMuc,String sLNS, String iID_MaTrangThaiDuyet)
        {
            String DkDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else if (iID_MaTrangThaiDuyet == "2")
            {
                DkDuyet = "AND iID_MaTrangThaiDuyet<>'" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            else
            {
                DkDuyet = " ";
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = String.Format(@"SELECT c.iID_MaDanhMuc,b.sTen as TenDV,a.iID_MaDonVi
                                     FROM( SELECT DISTINCT iID_MaDonVi
                                           FROM DT_ChungTuChiTiet
                                           WHERE iTrangThai=1  {0} {1}  AND ({2})) a
                                    INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ) b
                                    ON a.iID_MaDonVi=b.iID_MaDonVi
                                    INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc WHERE 1=1 AND iID_MaDanhMuc=@iID_MaDanhMuc) c
                                    ON b.iID_MaNhomDonVi=c.iID_MaDanhMuc", ReportModels.DieuKien_NganSach(MaND), DkDuyet, DKLNS);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iID_MaDanhMuc);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            //cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["TenDV"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);

            }
            else
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["TenDV"] = "--Chọn tất cả đơn vị--";
                dt.Rows.InsertAt(R, 0);

            }
            cmd.Dispose();
            return dt;
        }

        public static DataTable DtLoaiNganSach(String MaND)
        {
          
            DataTable dt = new DataTable();

            String SQL = string.Format(@"SELECT DISTINCT sLNS,sLNS+'-'+ sMoTa as sTen
                                        FROM NS_MucLucNganSach
                                        WHERE sL='' AND iTrangThai=1 {0} AND LEN(sLNS)=7
                                        ORDER By sXauNoiMa", ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public static DataTable DS_NhomDonVi(String MaND,String sLNS,String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();

            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0;i<arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i<arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String TrangThaiDuyet="";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    TrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
                }
                else if (iID_MaTrangThaiDuyet == "2")
                {
                    TrangThaiDuyet = "AND iID_MaTrangThaiDuyet<>'" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
                }
                else
                {
                    TrangThaiDuyet = " ";
                }
            DataTable dtNhomDV = new DataTable();

            String SQL = string.Format(@"SELECT DISTINCT c.sTen as TenDM,c.iID_MaDanhMuc 
                                        FROM( SELECT DISTINCT iID_MaDonVi
                                        FROM DT_ChungTuChiTiet
                                        WHERE ({0}) AND iTrangThai=1 {1} {2}) a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        INNER JOIN (SELECT iID_MaDanhMuc,sTen FROM DC_DanhMuc) c
                                        ON b.iID_MaNhomDonVi=c.iID_MaDanhMuc", DKLNS, ReportModels.DieuKien_NganSach(MaND), TrangThaiDuyet);
                                       cmd.CommandText = SQL;
                                       cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                                       for (int i = 0; i < arrLNS.Length; i++)
                                       {
                                           cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                                       }
                                           dtNhomDV = Connection.GetDataTable(cmd);
        if (dtNhomDV.Rows.Count >= 0)
            {
                                           DataRow R1 = dtNhomDV.NewRow();
                                           R1["iID_MaDanhMuc"] = Guid.Empty.ToString();
                                           R1["TenDM"] = "Chọn nhóm đơn vị";
                                           dtNhomDV.Rows.InsertAt(R1, 0);
            }
            if (dtNhomDV.Rows.Count == 0)
            {
                DataRow R = dtNhomDV.NewRow();
                R["iID_MaDanhMuc"] = Guid.Empty.ToString();
                R["TenDM"] = "Không có nhóm đơn vị";
                dtNhomDV.Rows.InsertAt(R, 0);
            }
            return dtNhomDV;
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

            DataRow dr1 = dt.NewRow();
            DataRow dr2 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 0);

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 1);


            dr2["iID_MaTrangThaiDuyet"] = "2";
            dr2["TenTrangThai"] = "Chưa duyệt";
            dt.Rows.InsertAt(dr2, 2);

            return dt;
        }
    }
}

