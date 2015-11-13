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
    public class rptThongBaoBoSungNganSachController : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoBoSungNganSach.xls";
        private long Tong;
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptThongBaoBoSungNganSach.aspx";
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
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String DotPhanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaDotPhanBo"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["DotPhanBo"] = DotPhanBo;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptThongBaoBoSungNganSach.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"> đường dẫn</param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="DotPhanBo">Mã đợt phân bổ</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String DotPhanBo, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV = "";
            DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND,iID_MaTrangThaiDuyet, DotPhanBo);
            for (int i = 0; i < dtDonVi.Rows.Count;i++ )
            {
                if (iID_MaDonVi == dtDonVi.Rows[i]["iID_MaDonVi"].ToString())
                {
                    TenDV = dtDonVi.Rows[i]["TenHT"].ToString();
                    break;
                }
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String Dot = ReportModels.Get_STTDotPhanBo(MaND, iID_MaTrangThaiDuyet, DotPhanBo).ToString();
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongBaoBoSungNganSach");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, DotPhanBo);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("TenDv", TenDV);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("Dot", Dot);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// set các Range trong báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="DotPhanBo"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String DotPhanBo)
        {
            
            DataTable data = PB_BoSungNganSach(MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, DotPhanBo);
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
        /// <param name="DotPhanBo"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String DotPhanBo, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDonVi, DotPhanBo,iID_MaTrangThaiDuyet);
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
        /// <param name="DotPhanBo"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String DotPhanBo, String iID_MaTrangThaiDuyet)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDonVi, DotPhanBo, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongBaoBoSungNganSach.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="DotPhanBo"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDonVi, String DotPhanBo, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDonVi, DotPhanBo, iID_MaTrangThaiDuyet);
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
         public class Data
        {
            public String iID_MaDotPhanBo { get; set; }
            public String iID_MaDonVi { get; set; }
        }
        [HttpGet]
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo,String iID_MaDonVi)
        {

            return Json(obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public Data obj_DSDotPhanBo(string ParentID, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, String iID_MaDonVi)
        {
            Data _data = new Data();
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _data.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonDPB()\"");
            DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, true);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            _data.iID_MaDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "","class=\"input1_2\" style=\"width: 100%\"");
            dtDotPhanBo.Dispose();
            dtDonVi.Dispose();
            return _data;
        }
       /// <summary>
        /// Bổ sung ngân sách
       /// </summary>
       /// <param name="NamLamViec"></param>
       /// <param name="iID_MaDonVi"></param>
       /// <param name="DotPhanBo"></param>
       /// <returns></returns>
        public DataTable PB_BoSungNganSach(String MaND, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String DotPhanBo)
        {
            Tong=0;
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            int i, j;
            DataTable dtDotNay = new DataTable();
            DataTable dtLuyKe = new DataTable();
             String DKDotPhanBo = "IID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            for (i = 0; i < dtDotPhanBo.Rows.Count; i++)
            {
                //tạo dt dotnay
                if (DotPhanBo == Convert.ToString(dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"]))
                {
                    SqlCommand cmdDotNay = new SqlCommand();
                    String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
                    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
                    {
                        DK_Duyet = "";
                    }
                    else
                    {
                        cmdDotNay.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    }
                    String SQLDotNay = String.Format(@"SELECT SUBSTRING(ChiTiet.sLNS,1,1) as NguonNS,ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sMoTa,
                                        SUM(rTuChi) as rTuChiDotNay,SUM(ChiTiet.rHienVat) as rHienVatDotNay,SUM(rTuChi)+SUM(rHienVat) as rTongSo
                                        FROM PB_PhanBoChiTiet as ChiTiet
                                        WHERE sNG<>'' AND ChiTiet.iID_MaDotPhanBo=@DotPhanBo AND ChiTiet.iID_MaDonVi=@iID_MaDonVi {0} {1}
                                        GROUP BY SUBSTRING(ChiTiet.sLNS,1,1),ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sMoTa
                                        HAVING SUM(rTuChi) <>0 OR SUM(rHienVat)<>0                                       
                                        ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc",ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
                    cmdDotNay.CommandText = SQLDotNay;
                    cmdDotNay.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmdDotNay.Parameters.AddWithValue("@DotPhanBo", DotPhanBo);
                    dtDotNay = Connection.GetDataTable(cmdDotNay);
                    if (cmdDotNay != null) cmdDotNay.Dispose();
                    for (i = 0; i < dtDotNay.Rows.Count; i++)
                    {
                        if (dtDotNay.Rows[i]["rTongSo"].ToString() != "")
                        {
                            Tong += long.Parse(dtDotNay.Rows[i]["rTongSo"].ToString());
                        }
                    }
                    break;
                }
            }
             for (i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (DotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    DKDotPhanBo = "";
                    for (j = 1; j <= i; j++)
                    {
                        DKDotPhanBo += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                        if (j < i)
                            DKDotPhanBo += " OR ";
                    }
                    break;
                }

            }
             SqlCommand cmdLuyKe = new SqlCommand();
             String DK_Duyet_LK = ReportModels.DieuKien_TrangThaiDuyet;
             if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
             {
                 DK_Duyet_LK = "";
             }
             else
             {
                 cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
             }
                    String SQLLuyKe = String.Format(@"SELECT SUBSTRING(ChiTiet.sLNS,1,1) as NguonNS,ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sMoTa,
                                        SUM(rTuChi) as rTuChiLuyKe,SUM(ChiTiet.rHienVat) as rHienVatLuyKe
                                        FROM PB_PhanBoChiTiet as ChiTiet
                                        WHERE  ({0}) AND sNG<>'' AND ChiTiet.iID_MaDonVi=@iID_MaDonVi {1} {2}
                                        GROUP BY SUBSTRING(ChiTiet.sLNS,1,1),ChiTiet.sLNS,ChiTiet.sL,ChiTiet.sK,ChiTiet.sM,ChiTiet.sTM,ChiTiet.sTTM,ChiTiet.sNG,ChiTiet.sMoTa
                                        HAVING SUM(rTuChi)<>0 OR  SUM(rHienVat)<>0                                       
                                        ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc", DKDotPhanBo,ReportModels.DieuKien_NganSach(MaND),DK_Duyet_LK);
                    cmdLuyKe.CommandText = SQLLuyKe;
                    cmdLuyKe.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    //cmdLuyKe.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    for (i = 1; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        if (DotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                        {
                            for (j = 1; j <= i; j++)
                            {
                                cmdLuyKe.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                            }
                            break;
                        }
                    }
                        dtLuyKe = Connection.GetDataTable(cmdLuyKe);
                        cmdLuyKe.Dispose();
                    
                   
            //ghép 2 dt
            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,rTuChiDotNay,rHienVatDotNay";
            String[] arrCol = sCol.Split(',');
            dtLuyKe.Columns.Add("rTuChiDotNay", typeof(Decimal));
            dtLuyKe.Columns.Add("rHienVatDotNay", typeof(Decimal));                      
            for (i = 0; i < dtDotNay.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"sLNS={0} AND sL={1} AND sK={2} AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}' AND NguonNS='{7}'",
                                                   dtDotNay.Rows[i]["sLNS"], dtDotNay.Rows[i]["sL"],
                                                   dtDotNay.Rows[i]["sK"],
                                                   dtDotNay.Rows[i]["sM"], dtDotNay.Rows[i]["sTM"],
                                                   dtDotNay.Rows[i]["sTTM"], dtDotNay.Rows[i]["sNG"], dtDotNay.Rows[i]["NguonNS"]                                                
                                                   );
                DataRow[] R = dtLuyKe.Select(xauTruyVan);
                if (R == null || R.Length == 0)
                {
                    addR = dtLuyKe.NewRow();
                    for (j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtDotNay.Rows[i][arrCol[j]];
                    }
                    dtLuyKe.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtDotNay.Rows)
                    {

                        for (j = 0; j < dtLuyKe.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dtLuyKe.Rows[j];

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
                                dtLuyKe.Rows[j]["rTuChiDotNay"] = R1["rTuChiDotNay"];
                                dtLuyKe.Rows[j]["rHienVatDotNay"] = R1["rHienVatDotNay"];
                                break;
                            }
                            
                        }
                    }
                }
            }
            dtLuyKe.Dispose();
            dtDotNay.Dispose();
            dtDotPhanBo.Dispose();
            return dtLuyKe;
        }      
    }
}

