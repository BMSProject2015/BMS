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
namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptPB_TongHopChiTieuNganSachQuocPhong_9Controller : Controller
    {
        //
        // GET: /rptPB_TongHopChiTieuNganSachQuocPhong_9/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/PhanBo/rptTH_9_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/PhanBo/rptTH_9_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/PhanBo/rptTH_9_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/PhanBo/rptTH_9_A3_2.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptPB_TongHopChiTieuNganSachQuocPhong_9.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trịn trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID,String ToDaXem)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaDotPhanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaDotPhanBo"]);
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String TruongTien = Request.Form[ParentID + "_TruongTien"];
            String KhoGiay = Request.Form[ParentID + "_KhoGiay"];
            String ToSo = Request.Form[ParentID + "_ToSo"];
            if (String.IsNullOrEmpty(ToDaXem)) ToDaXem = "";
            String[] arrToDaXem = ToDaXem.Split(',');
            bool DaCo = false;
            for (int i = 0; i < arrToDaXem.Length; i++)
            {
                if (arrToDaXem[i] == ToSo)
                {
                    DaCo = true;
                }
            }
            if (!DaCo)
            {
                if (ToDaXem == "") ToDaXem = ToSo;
                else ToDaXem += "," + ToSo;
            }
            ViewData["ToDaXem"] = ToDaXem;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["TruongTien"] = TruongTien;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["ToSo"] = ToSo;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptPB_TongHopChiTieuNganSachQuocPhong_9.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path,String MaND,String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay,String ToSo)
        {
            XlsFile Result = new XlsFile(true);
            FlexCelReport fr = new FlexCelReport();
            Result.Open(path);
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            String tendot = "";
            for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    try
                    {
                        DateTime ngay=Convert.ToDateTime(dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString());

                        tendot = " Tháng  " + ngay.ToString("MM") + "   Năm  " + ngay.ToString("yyyy");
                    }
                    catch { tendot = ""; }
                }
            }
            String Dot = String.Format("ĐỢT {0} : {1} ", ReportModels.Get_STTDotPhanBo(MaND,iID_MaTrangThaiDuyet, iID_MaDotPhanBo), tendot);
            DataTable dtPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String [] TenDotPhanBo;
            String str_MaDotPhanBo = "";
            if (dtPhanBo.Rows.Count > 0)
            {
                if (iID_MaDotPhanBo == dtPhanBo.Rows[0]["iID_MaDotPhanBo"].ToString())
                {
                    str_MaDotPhanBo = Guid.Empty.ToString();
                }
                else
                {
                    for (int i = 1; i < dtPhanBo.Rows.Count; i++)
                    {
                        String DauPhay = ",";
                        if (i == 1) DauPhay = "";
                        str_MaDotPhanBo += DauPhay + dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                        if (iID_MaDotPhanBo == dtPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString()) break;
                    }
                }
            }
            String DotBoSung = str_MaDotPhanBo;
            String[] arrDotPhanBo = str_MaDotPhanBo.Split(',');
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 8)
                    {
                        int a = 8 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[8];
                    for (int i = 0; i < 8; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[i] = Convert.ToString(dtPhanBo.Rows[i + 1]["dNgayDotPhanBo"]);
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotBoSung" + i, "");
                        }
                        else
                            fr.SetValue("DotBoSung" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
                else
                {
                    int a1 = 8 + 11 * (Convert.ToInt16(ToSo) - 1);
                    int a2 = 8 + 11 * (Convert.ToInt16(ToSo) - 2);
                    if (arrDotPhanBo.Length < a1)
                    {
                        int a = a1 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[11];
                    int x = 1;
                    for (int i = a2; i < a1; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[x - 1] = Convert.ToString(dtPhanBo.Rows[i + 1]["dNgayDotPhanBo"]);
                            x++;
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotBoSung" + i, "");
                        }
                        else
                            fr.SetValue("DotBoSung" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
            }
            else
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 4)
                    {
                        int a = 4 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[4];
                    for (int i = 0; i < 4; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[i] = Convert.ToString(dtPhanBo.Rows[i + 1]["dNgayDotPhanBo"]);
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotBoSung" + i, "");
                        }
                        else
                            fr.SetValue("DotBoSung" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
                else
                {
                     int a1 = 4 + 7 * (Convert.ToInt16(ToSo) - 1);
                    int a2 = 4 + 7 * (Convert.ToInt16(ToSo) - 2);
                    if (arrDotPhanBo.Length < a1)
                    {
                        int a = a1 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            DotBoSung += "," + Guid.Empty.ToString();
                        }
                    }
                    arrDotPhanBo = DotBoSung.Split(',');
                    TenDotPhanBo = new String[7];
                    int x = 1;
                    for (int i = a2; i < a1; i++)
                    {
                        if (arrDotPhanBo[i] != null && arrDotPhanBo[i] != "" + Guid.Empty.ToString() + "" && arrDotPhanBo[i] != "")
                        {
                            TenDotPhanBo[x - 1] = Convert.ToString(dtPhanBo.Rows[i + 1]["dNgayDotPhanBo"]);
                            x++;
                        }
                    }
                    for (int i = 1; i <= TenDotPhanBo.Length; i++)
                    {
                        if (String.IsNullOrEmpty(TenDotPhanBo[i - 1]))
                        {
                            fr.SetValue("DotBoSung" + i, "");
                        }
                        else
                            fr.SetValue("DotBoSung" + i, Convert.ToString(TenDotPhanBo[i - 1]));
                    }
                }
            }
            String TenTruongTien = "";
            switch (TruongTien)
            {
                case "rTuChi":
                    TenTruongTien = "- TỰ CHI";
                    break;
                case "rHienVat":
                    TenTruongTien = "- HIỆN VẬT";
                    break;
            }
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();

            LoadData(fr, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
            fr = ReportModels.LayThongTinChuKy(fr, "rptPB_TongHopChiTieuNganSachQuocPhong_9");
            fr.SetValue("Dot", Dot);
            fr.SetValue("TruongTien", TenTruongTien);
            fr.SetValue("NgayThangNam", NgayThang);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("TenTieuDe", "TỔNG HỢP CHỈ TIÊU NGÂN SÁCH QUỐC PHÒNG ");
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Phong", ReportModels.CauHinhTenDonViSuDung(3));
            fr.Run(Result);
            return Result;
            }
        
        /// <summary>
        /// Hàm lấy mô tả loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A3_1;
                }
                else
                DuongDan = sFilePath_A3_2;
            }
            else
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A4_1;
                }
                DuongDan = sFilePath_A4_2;

            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "TongHopChiTieuQuocPhong_9_A4";
                }
                else
                {
                    clsResult.FileName = "TongHopChiTieuQuocPhong_9_A3";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm xem báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="DotPhanBo"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A3_1;
                }
                else
                    DuongDan = sFilePath_A3_2;
            }
            else
            {
                if (ToSo == "1")
                {
                    DuongDan = sFilePath_A4_1;
                }
                else
                    DuongDan = sFilePath_A4_2;
 
            }
            
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
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
        /// Json đấy lấy các giá trị của Datatable Ds_DotPhanBo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>

        public  class _Data
        {
            public string iID_MaDotPhanBo { get; set; }
            public string ToSo { get; set; }
        }
        public JsonResult Get_dsDotPhanBo(string ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet,String TruongTien,String KhoGiay,String ToSo)
        {

            return Json(obj_DSDotPhanBo(ParentID, MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet,TruongTien,KhoGiay,ToSo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy đợt phân bổ
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <returns></returns>
        /// 
        public _Data obj_DSDotPhanBo(string ParentID, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet,String TruongTien,String KhoGiay,String ToSo)
        {
            _Data _data = new _Data();
            #region Option đợt phân bổ 
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _data.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 80%\"onchange=\"ChonNLV()\"");
             dtDotPhanBo.Dispose();
            #endregion
            #region Option Tờ số
             DataTable dtToSo = dtTo(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay);
             SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
             _data.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 80%\"");
             dtToSo.Dispose();
            #endregion
             return _data;
        }
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {

            DataTable data = PB_TongHopChiTieuNganSachQuocPhong_9(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            // Rows đến kì này
                DataTable data_LK = dtDenKyNay(MaND, sLNS, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, TruongTien, KhoGiay, ToSo);
                if (data_LK.Rows.Count == 0)
                {
                    DataRow dr = data_LK.NewRow();
                    data_LK.Rows.InsertAt(dr, 0);
                }
                fr.AddTable("LuyKe", data_LK);
                data_LK.Dispose();

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);
            if (dtLoaiNS.Rows.Count == 0)
            {
                DataRow dr = dtLoaiNS.NewRow();
                dtLoaiNS.Rows.InsertAt(dr, 0);
            }
            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtLoaiNS, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            if (dtNguonNS.Rows.Count == 0)
            {
                DataRow dr = dtNguonNS.NewRow();
                dtNguonNS.Rows.InsertAt(dr, 0);
            }
            fr.AddTable("NguonNS", dtNguonNS);
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        public DataTable PB_TongHopChiTieuNganSachQuocPhong_9(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay, String ToSo)
        {
                
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
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
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String MaDotDauTien = "";
            String DKSUMDotBS = "";
            String DKHAVINGDotBS = "";
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String str_MaDotPhanBo = "";
            if (dtDotPhanBo.Rows.Count > 0)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"].ToString())
                {
                    str_MaDotPhanBo = Guid.Empty.ToString();
                }
                else
                {
                    for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                    {
                        String DauPhay = ",";
                        if (i == 1) DauPhay = "";
                        str_MaDotPhanBo += DauPhay + dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                        if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString()) break;
                    }
                }
            }

            String DkDotPhanBo = "";

            String[] arrDotPhanBo = str_MaDotPhanBo.Split(',');
            for (int i = 0; i < arrDotPhanBo.Length; i++)
            {
                DkDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBoa" + i;
                if (i < arrDotPhanBo.Length - 1)
                    DkDotPhanBo += " OR ";
                cmd.Parameters.AddWithValue("iID_MaDotPhanBoa" + i, arrDotPhanBo[i]);
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            #region//lấy đợt đầu tiên

            if (dtDotPhanBo.Rows.Count > 0)
            {
                MaDotDauTien = Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            #endregion

            String DKDotBS = "";
            //a3
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 8)
                    {
                        int a = 8 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDotBS += ",SUM(DotBoSung" + i + ") AS DotBoSung" + i;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + i + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + i + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
                else
                {
                    if (arrDotPhanBo.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                    {
                        DKSUMDotBS += ",SUM(DotBoSung" + x + ") AS DotBoSung" + x;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + x + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + x + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + (x - 1), arrDotPhanBo[i]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
            }
             //a 4
            else
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 4)
                    {
                        int a = 4 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDotBS += ",SUM(DotBoSung" + i + ") AS DotBoSung" + i;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + i + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + i + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
                else
                {
                    if (arrDotPhanBo.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {
                        DKSUMDotBS += ",SUM(DotBoSung" + x + ") AS DotBoSung" + x;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + x + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + x + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + (x-1), arrDotPhanBo[i]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
            }

            String SQL = string.Format(@" SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                            ,SUM(TongCong) as TongCong
                                            ,SUM(DotDauNam) as DotDauNam
                                            ,SUM(LuyKeBS) as LuyKe
                                            {3}
                                            FROM (
                                            SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                            ,TongCong=CASE WHEN ({1}) THEN SUM({0}) ELSE 0 END
                                            + CASE WHEN iID_MaDotPhanBo=@MaDotDauNam THEN SUM({0}) ELSE 0 END
                                            ,DotDauNam=CASE WHEN iID_MaDotPhanBo=@MaDotDauNam THEN SUM({0}) ELSE 0 END
                                            ,LuyKeBS=CASE WHEN ({1}) THEN SUM({0}) ELSE 0 END
                                            {2}
                                            FROM PB_PhanBoChiTiet PB
                                            WHERE 1=1 AND ({7}) AND iTrangThai=1 {5} {6} AND sNG<>''
                                            GROUP BY  SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaDotPhanBo,iTrangThai
                                            HAVING SUM({0})!=0
                                            ) as BangPhanBo
                                            GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                            HAVING SUM(TongCong)!=0 or SUM(DotDauNam)!=0 or SUM(LuyKeBS)!=0 {4}", TruongTien, DkDotPhanBo, DKDotBS, DKSUMDotBS, DKHAVINGDotBS, DK_Duyet, ReportModels.DieuKien_NganSach(MaND),DKLNS);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@MaDotDauNam", MaDotDauTien);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

        public static DataTable dtDenKyNay(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien,String KhoGiay, String ToSo)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
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
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String MaDotDauTien = "";
            String DKSUMDotBS = "";
            String DKHAVINGDotBS = "";
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
            String str_MaDotPhanBo = "";
            if (dtDotPhanBo.Rows.Count > 0)
            {
                for (int i = 0; i < dtDotPhanBo.Rows.Count; i++)
                {
                    String DauPhay = ",";
                    if (i == 0) DauPhay = "";
                    str_MaDotPhanBo += DauPhay + dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString()) break;
                }
            }

            String DkDotPhanBo = "";

            String[] arrDotPhanBo = str_MaDotPhanBo.Split(',');
            for (int i = 0; i < arrDotPhanBo.Length; i++)
            {
                DkDotPhanBo += "iID_MaDotPhanBo=@iID_MaDotPhanBoa" + i;
                if (i < arrDotPhanBo.Length - 1)
                    DkDotPhanBo += " OR ";
                cmd.Parameters.AddWithValue("iID_MaDotPhanBoa" + i, arrDotPhanBo[i]);
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            #region//lấy đợt đầu tiên

            if (dtDotPhanBo.Rows.Count > 0)
            {
                MaDotDauTien = Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            #endregion

            String DKDotBS = "";
            //a3
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 8)
                    {
                        int a = 8 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDotBS += ",SUM(DotBoSung" + i + ") AS DotBoSung" + i;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + i + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + i + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
                else
                {
                    if (arrDotPhanBo.Length < 8 + 11 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 8 + 11 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 8 + 11 * tg; i < 8 + 11 * (tg + 1); i++)
                    {
                        DKSUMDotBS += ",SUM(DotBoSung" + x + ") AS DotBoSung" + x;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + x + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + x + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + (x - 1), arrDotPhanBo[i]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
            }
            //a 4
            else
            {
                if (ToSo == "1")
                {
                    if (arrDotPhanBo.Length < 4)
                    {
                        int a = 4 - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        //iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i - 1]["iID_MaDonVi"]);
                        DKSUMDotBS += ",SUM(DotBoSung" + i + ") AS DotBoSung" + i;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + i + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + i + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + i + ") THEN SUM({0}) ELSE 0 END";
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + i, arrDotPhanBo[i - 1]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
                else
                {
                    if (arrDotPhanBo.Length < 4 + 7 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 4 + 7 * (Convert.ToInt16(ToSo) - 1) - arrDotPhanBo.Length;
                        for (int i = 0; i < a; i++)
                        {
                            str_MaDotPhanBo += "," + Guid.Empty.ToString();
                        }
                        arrDotPhanBo = str_MaDotPhanBo.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 4 + 7 * tg; i < 4 + 7 * (tg + 1); i++)
                    {
                        DKSUMDotBS += ",SUM(DotBoSung" + x + ") AS DotBoSung" + x;
                        DKHAVINGDotBS += " OR SUM(DotBoSung" + x + ")<>0 ";
                        DKDotBS += " ,DotBoSung" + x + "=CASE WHEN (iID_MaDotPhanBo=@iID_MaDotPhanBo" + x + ") THEN SUM({0}) ELSE 0 END";
                        x++;
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + (x - 1), arrDotPhanBo[i]);
                    }
                    DKDotBS = string.Format(DKDotBS, TruongTien);
                }
            }

            String SQL = string.Format(@" SELECT
                                            SUM(LuyKeBS) as LuyKe
                                            {3}
                                            FROM (
                                            SELECT 
                                            LuyKeBS=CASE WHEN ({1}) THEN SUM({0}) ELSE 0 END
                                            {2}
                                            FROM PB_PhanBoChiTiet PB
                                            WHERE 1=1 AND ({7}) AND iTrangThai=1 {5} {6} AND sNG<>''
                                            GROUP BY  iID_MaDotPhanBo
                                            HAVING SUM({0})!=0
                                            ) as BangPhanBo
                                            HAVING SUM(LuyKeBS)!=0 {4}", TruongTien, DkDotPhanBo, DKDotBS, DKSUMDotBS, DKHAVINGDotBS, DK_Duyet, ReportModels.DieuKien_NganSach(MaND), DKLNS);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            return dt;
        }



        /// <summary>
        /// Hàm lấy danh sách đợt phân bổ
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public static DataTable DanhSach_DotPhanBo(String MaND, String sLNS, String iID_MaTrangThaiDuyet, String TruongTien)
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
            if (String.IsNullOrEmpty(TruongTien))
            {
                TruongTien = "rTuChi";
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = String.Format(@"SELECT  a.iID_MaDotPhanBo,CONVERT(varchar(10),dNgayDotPhanBo,103) as dNgayDotPhanBo
                                        FROM  (SELECT iID_MaDotPhanBo 
	                                           FROM PB_PhanBoChiTiet
	                                           WHERE 1=1 
			                                   AND iTrangThai=1 {2} AND ({3}) {1}
	                                           GROUP BY iID_MaDotPhanBo
                                                HAVING SUM({0})!=0) as a
                                        INNER JOIN PB_DotPhanBo ON a.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                        ORDER BY MONTH(dNgayDotPhanBo) ,DAY(dNgayDotPhanBo) ", TruongTien, DK_Duyet, ReportModels.DieuKien_NganSach(MaND), DKLNS);
            cmd.CommandText = SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["iID_MaDotPhanBo"] = "" + Guid.Empty.ToString() + "";
                dr["dNgayDotPhanBo"] = "Không có dữ liệu";
                dt.Rows.InsertAt(dr,0);
            }
            return dt;
        }
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        /// 
        public static DataTable dtTo(String MaND, String sLNS, String iID_MaDotPhanBo, String iID_MaTrangThaiDuyet, String TruongTien, String KhoGiay)
        {
            DataTable dtDotPhanBo = DanhSach_DotPhanBo(MaND,sLNS,iID_MaTrangThaiDuyet,TruongTien);
            String strDotPhanBo = "";
            if (dtDotPhanBo.Rows.Count > 0)
            {
                for (int i = 1; i < dtDotPhanBo.Rows.Count; i++)
                {
                   String DauPhay = ",";
                    if(i==1) DauPhay="";
                    strDotPhanBo += DauPhay + dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString();
                    if(iID_MaDotPhanBo==dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())break;
   
                }
            }
            String[] arrDotPhanBo = strDotPhanBo.Split(',');
            dtDotPhanBo.Dispose();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));
            DataRow dr = dt.NewRow();
            dr[0] = "Tờ 1";
            dr[1] = "1";
            dt.Rows.InsertAt(dr, 0);
            //Luy ke
                if (KhoGiay == "1")
                {
                    int a = 2;
                    for (int i = 8; i < arrDotPhanBo.Length; i = i + 11)
                    {
                        DataRow dr1 = dt.NewRow();
                        dt.Rows.Add(dr1);
                        dr1[0] = "Tờ " + a;
                        dr1[1] = a;
                        a++;

                    }
                }
                else
                {
                    int a = 2;
                    for (int i = 4; i < arrDotPhanBo.Length; i = i + 7)
                    {
                        DataRow dr1 = dt.NewRow();
                        dt.Rows.Add(dr1);
                        dr1[0] = "Tờ " + a;
                        dr1[1] = a;
                        a++;

                    }
                }
                return dt;
            }

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