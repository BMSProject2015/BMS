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


namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_ThuongXuyen_32A3Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_A4_1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_32A3_A4_1.xls";
        private const String sFilePath_A4_2 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_32A3_A4_2.xls";
        private const String sFilePath_A3_1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_32A3_A3_1.xls";
        private const String sFilePath_A3_2 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_32A3_A3_2.xls";
        /// <summary>
        /// Hàm Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
                ViewData["PageLoad"] = "0";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_32A3.aspx";
            return View(sViewPath + "ReportView.aspx");
                 }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// EditSubmit nhận các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, String ToDaXem)
        {
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String Thang_Quy = "";
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if(LoaiThang_Quy=="1")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }

            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String ToSo = Convert.ToString(Request.Form[ParentID + "_ToSo"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["ToSo"] = ToSo;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_32A3.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path">Đường dẫn</param>
        /// <param name="NamLamViec">Năm làm việc</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="Thang_Quy"> Tháng/Quý làm việc</param>
        /// <param name="LoaiThang_Quy">0 : tháng  1:quý</param>
        /// <param name="KhoGiay">A3 hay a4</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang_Quy, String LoaiThang_Quy, String MaND, String KhoGiay, String ToSo)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            //lấy tên đơn vị
            DataTable dtDonVi = LayDSDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);
            String[] TenDV;
            String[] arrDonVi = iID_MaDonVi.Split(',');
            

            String DonVi1 = "";
            String DonVi = iID_MaDonVi;
            int a = arrDonVi.Length;
            int b = dtDonVi.Rows.Count;
            if (arrDonVi.Length > 0)
            {
                DonVi1 = "Tổng hợp " + a + "/" + b + " đơn vị";
                if (a == b)
                {
                    DonVi1 = "Tổng hợp tất cả đơn vị";
                }
                if (b == 0)
                {
                    DonVi1 = "";
                }
            }
            String TenThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                TenThangQuy = "Quý " + Thang_Quy;
            }
            else
            {
                TenThangQuy = "Tháng " + Thang_Quy;
            }
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDonVi.Length < 5)
                    {
                        int a1 = 5 - arrDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            DonVi += ",-1";
                        }
                    }
                    arrDonVi = DonVi.Split(',');
                    TenDV = new String[5];
                    for (int i = 0; i < 5; i++)
                    {
                        if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString())
                        {
                            TenDV[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                        }
                    }

                    for (int i = 1; i <= TenDV.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, TenDV[i - 1]);
                    }
                }
                else
                {
                    if (arrDonVi.Length < 5 + 6 * (Convert.ToInt16(ToSo) - 1))
                    {
                        int a1 = 5 + 6 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    TenDV = new String[6];
                    int x = 1;
                    for (int i = 5 + 6 * ((Convert.ToInt16(ToSo) - 2)); i < 5 + 6 * ((Convert.ToInt16(ToSo) - 1)); i++)
                    {
                        if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString())
                        {
                            TenDV[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                            x++;
                        }
                    }

                    for (int i = 1; i <= TenDV.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, TenDV[i - 1]);
                    }
                }
            }
            //A4
            else
            {
                if (ToSo == "1")
                {
                    if (arrDonVi.Length < 3)
                    {
                        int a1 = 3 - arrDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            DonVi += ",-1";
                        }
                    }
                    arrDonVi = DonVi.Split(',');
                    TenDV = new String[3];
                    for (int i = 0; i < 3; i++)
                    {
                        if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString())
                        {
                            TenDV[i] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                        }
                    }

                    for (int i = 1; i <= TenDV.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, TenDV[i - 1]);
                    }
                }
                else
                {
                    if (arrDonVi.Length < 3+ 4 * (Convert.ToInt16(ToSo) - 1))
                    {
                        int a1 = 3 + 4 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a1; i++)
                        {
                            DonVi += ",-1";
                        }
                        arrDonVi = DonVi.Split(',');
                    }
                    TenDV = new String[4];
                    int x = 1;
                    for (int i = 3 + 4 * ((Convert.ToInt16(ToSo) - 2)); i < 3 + 4 * ((Convert.ToInt16(ToSo) - 1)); i++)
                    {
                        if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString())
                        {
                            TenDV[x - 1] = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", arrDonVi[i], "sTen").ToString();
                            x++;
                        }
                    }

                    for (int i = 1; i <= TenDV.Length; i++)
                    {
                        fr.SetValue("DonVi" + i, TenDV[i - 1]);
                    }
                }
            }
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
        
            LoadData(fr, iID_MaTrangThaiDuyet, iID_MaDonVi, Thang_Quy, LoaiThang_Quy, MaND, KhoGiay, ToSo);
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_ThuongXuyen_32A3");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("ThangQuy", TenThangQuy);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("DonVi", DonVi1);
            fr.SetValue("ToSo", ToSo);
            fr.Run(Result);                
                return Result;

        }
       /// <summary>
       /// xem PDF
       /// </summary>
       /// <param name="NamLamViec"></param>
       /// <param name="iID_MaDonVi"></param>
       /// <param name="Thang_Quy"></param>
       /// <param name="LoaiThang_Quy"></param>
       /// <param name="KhoGiay"></param>
       /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang_Quy, String LoaiThang_Quy,String MaND,String KhoGiay,String ToSo)
        {
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1") DuongDan = sFilePath_A3_1;
                else DuongDan = sFilePath_A3_2;
            }
            else
            {
                if (ToSo == "1") DuongDan = sFilePath_A4_1;
                else DuongDan = sFilePath_A4_2;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, Thang_Quy, LoaiThang_Quy,MaND, KhoGiay,ToSo);
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
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="KhoGiay"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang_Quy, String LoaiThang_Quy, String MaND, String KhoGiay, String ToSo)
        {
            String DuongDan = "";
            if (KhoGiay == "1")
            {
                if (ToSo == "1") DuongDan = sFilePath_A3_1;
                else DuongDan = sFilePath_A3_2;
            }
            else
            {
                if (ToSo == "1") DuongDan = sFilePath_A4_1;
                else DuongDan = sFilePath_A4_2;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaTrangThaiDuyet, iID_MaDonVi, Thang_Quy, LoaiThang_Quy,MaND, KhoGiay,ToSo);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }


        public class data
        {
            public String iID_MaDonVi { get; set; }
            public String ToSo { get; set; }
        }
        /// <summary>
        /// onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public JsonResult Ds_DonVi(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi,String MaND, String KhoGiay, String ToSo)
        {
            return Json(obj_DonVi(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, MaND, KhoGiay, ToSo), JsonRequestBehavior.AllowGet);
        }
        public data obj_DonVi(String ParentID, String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String iID_MaDonVi, String MaND, String KhoGiay, String ToSo)
        {
            data _data = new data();
            String input = "";
            DataTable dt = LayDSDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy,MaND);
            StringBuilder stbDonVi = new StringBuilder();
            stbDonVi.Append("<div style=\"width: 99%; height: 350px; overflow: scroll; border:1px solid black;\">");
            stbDonVi.Append("<table class=\"mGrid\">");
            stbDonVi.Append("<tr>");
            stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

            String TenDonVi = "", MaDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dt.Rows[i - 1]["iID_MaDonVi"]);
                TenDonVi = Convert.ToString(dt.Rows[i - 1]["sTen"]);
                _Checked = "";
                for (int j = 1; j <= arrDonVi.Length; j++)
                {
                    if (MaDonVi == arrDonVi[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" onchange=\"ChonTo()\" name=\"iID_MaDonVi\" />", MaDonVi, _Checked);
                stbDonVi.Append("<tr>");
                stbDonVi.Append("<td style=\"width: 15%;\">");
                stbDonVi.Append(input);
                stbDonVi.Append("</td>");
                stbDonVi.Append("<td>" + TenDonVi + "</td>");

                stbDonVi.Append("</tr>");
            }
            stbDonVi.Append("</table>");
            stbDonVi.Append("</div>");
            dt.Dispose();
            _data.iID_MaDonVi = stbDonVi.ToString();

            //to

            DataTable dtToSo = dtTo(KhoGiay, iID_MaDonVi);
            SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
            _data.ToSo = MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 50%\"");
            return _data;
        }
        /// <summary>
        /// QuyetToan_ThuongXuyen_32A3
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang_Quy, String LoaiThang_Quy, String MaND, String KhoGiay, String ToSo)
        {

            DataTable data = QuyetToan_ThuongXuyen_32A3(iID_MaTrangThaiDuyet, iID_MaDonVi, Thang_Quy, LoaiThang_Quy,MaND,KhoGiay,ToSo);
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
        public DataTable QuyetToan_ThuongXuyen_32A3(String iID_MaTrangThaiDuyet, String iID_MaDonVi, String Thang_Quy, String LoaiThang_Quy,String MaND,String KhoGiay,String ToSo)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy = "(iThang_Quy=1 OR iThang_Quy=2 OR iThang_Quy=3)";
                        break;
                    case "2":
                        DKThangQuy = "(iThang_Quy=4 OR iThang_Quy=5 OR iThang_Quy=6)";
                        break;
                    case "3":
                        DKThangQuy = "(iThang_Quy=7 OR iThang_Quy=8 OR iThang_Quy=9)";
                        break;
                    case "4":
                        DKThangQuy = "(iThang_Quy=10 OR iThang_Quy=11 OR iThang_Quy=12)";
                        break;
                    default:
                        DKThangQuy = "(iThang_Quy=-1)";
                        break;
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@ThangQuy";
            }
            String DKDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length;i++ )
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVia"+i;
                if (i < arrDonVi.Length - 1)
                    DKDonVi += " OR ";
            }
            if (!String.IsNullOrEmpty(DKDonVi))
            {
                DKDonVi = " AND (" + DKDonVi + ")";
            }
            String DKDonViSELECT="", DKDonViNguoiCASE="",DKDonViNgayCASE="",DKDonViTienCASE="", DKDonViHaVing = "";
            //A3
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    if (arrDonVi.Length < 5)
                    {
                        int a = 5 - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = 1; i <= 5; i++)
                    {
                        DKDonViSELECT += ",SUM(DonVi" + i + "Nguoi) AS DonVi" + i + "Nguoi,SUM(DonVi" + i + "Ngay) AS DonVi" + i + "Ngay,SUM(DonVi" + i + "Tien) as DonVi" + i + "Tien";
                        DKDonViNguoiCASE += ",DonVi" + i + "Nguoi=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + i + " THEN SUM(rSoNguoi) ELSE 0 END";
                        DKDonViNgayCASE += ",DonVi" + i + "Ngay=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + i + " THEN SUM(rNgay) ELSE 0 END";
                        DKDonViTienCASE += ",DonVi" + i + "Tien=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + i + " THEN SUM(rTuChi) ELSE 0 END";
                        DKDonViHaVing += " OR SUM(DonVi" + i + "Nguoi)<>0 OR SUM(DonVi" + i + "Ngay)<>0 OR SUM(DonVi" + i + "Tien)<>0 ";
                    }
                }
                else
                {
                    if (arrDonVi.Length < 5 + 6 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 5 + 6 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 5 + 6 * tg; i < 5 + 6 * (tg + 1); i++)
                    {
                        DKDonViSELECT += ",SUM(DonVi" + x + "Nguoi) AS DonVi" + x + "Nguoi,SUM(DonVi" + x + "Ngay) AS DonVi" + x + "Ngay,SUM(DonVi" + x + "Tien) as DonVi" + x + "Tien";
                        DKDonViNguoiCASE += ",DonVi" + x + "Nguoi=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + x + " THEN SUM(rSoNguoi) ELSE 0 END";
                        DKDonViNgayCASE += ",DonVi" + x + "Ngay=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + x + " THEN SUM(rNgay) ELSE 0 END";
                        DKDonViTienCASE += ",DonVi" + x + "Tien=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + x + " THEN SUM(rTuChi) ELSE 0 END";
                        DKDonViHaVing += " OR SUM(DonVi" + x + "Nguoi)<>0 OR SUM(DonVi" + x + "Ngay)<>0 OR SUM(DonVi" + x + "Tien)<>0 ";
                        x++;
                    }
                }
            }
            else
            {
                if (ToSo == "1")
                {
                    if (arrDonVi.Length < 3)
                    {
                        int a = 3 - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    for (int i = 1; i <= 3; i++)
                    {
                        DKDonViSELECT += ",SUM(DonVi" + i + "Nguoi) AS DonVi" + i + "Nguoi,SUM(DonVi" + i + "Ngay) AS DonVi" + i + "Ngay,SUM(DonVi" + i + "Tien) as DonVi" + i + "Tien";
                        DKDonViNguoiCASE += ",DonVi" + i + "Nguoi=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + i + " THEN SUM(rSoNguoi) ELSE 0 END";
                        DKDonViNgayCASE += ",DonVi" + i + "Ngay=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + i + " THEN SUM(rNgay) ELSE 0 END";
                        DKDonViTienCASE += ",DonVi" + i + "Tien=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + i + " THEN SUM(rTuChi) ELSE 0 END";
                        DKDonViHaVing += " OR SUM(DonVi" + i + "Nguoi)<>0 OR SUM(DonVi" + i + "Ngay)<>0 OR SUM(DonVi" + i + "Tien)<>0 ";
                    }
                }
                else
                {
                    if (arrDonVi.Length < 3 + 4 * ((Convert.ToInt16(ToSo) - 1)))
                    {
                        int a = 3 + 4 * (Convert.ToInt16(ToSo) - 1) - arrDonVi.Length;
                        for (int i = 0; i < a; i++)
                        {
                            iID_MaDonVi += ",-1";
                        }
                        arrDonVi = iID_MaDonVi.Split(',');
                    }
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 3 + 4 * tg; i < 3 + 4 * (tg + 1); i++)
                    {
                        DKDonViSELECT += ",SUM(DonVi" + x + "Nguoi) AS DonVi" + x + "Nguoi,SUM(DonVi" + x + "Ngay) AS DonVi" + x + "Ngay,SUM(DonVi" + x + "Tien) as DonVi" + x + "Tien";
                        DKDonViNguoiCASE += ",DonVi" + x + "Nguoi=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + x + " THEN SUM(rSoNguoi) ELSE 0 END";
                        DKDonViNgayCASE += ",DonVi" + x + "Ngay=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + x + " THEN SUM(rNgay) ELSE 0 END";
                        DKDonViTienCASE += ",DonVi" + x + "Tien=CASE WHEN iID_MaDonVi=@iID_MaDonVi" + x + " THEN SUM(rTuChi) ELSE 0 END";
                        DKDonViHaVing += " OR SUM(DonVi" + x + "Nguoi)<>0 OR SUM(DonVi" + x + "Ngay)<>0 OR SUM(DonVi" + x + "Tien)<>0 ";
                        x++;
                    }
                }
            }
            
            DKDonViHaVing = DKDonViHaVing.Substring(3);
            String SQL = String.Format(@"SELECT  NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rSoNguoi) as Nguoi,SUM(rngay)as Ngay,SUM(rTuChi) as TuChi
                                        {0}
                                        FROM(SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rSoNguoi) as rSoNguoi,SUM(rNgay)as rNgay,SUM(rTuChi) as rTuChi
                                        {1}
                                        {2}
                                        {3}
                                            FROM QTA_ChungTuChiTiet
                                        WHERE  sLNS='1010000' AND sM<>'' AND {5} AND bLoaiThang_Quy=0 {6} {7} {8}
                                        GROUP BY  SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi) as A
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rSoNguoi) <>0 OR SUM(rNgay) <>0 OR SUM(rTuChi) <>0", DKDonViSELECT, DKDonViNguoiCASE, DKDonViNgayCASE, DKDonViTienCASE, "", DKThangQuy, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet, DKDonVi);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
            }
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
            if (KhoGiay == "1")
            {
                if (ToSo == "1")
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 5 + 6 * tg; i < 5 + 6 * (tg + 1); i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
            }
            else
            {
                if (ToSo == "1")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i - 1]);
                    }
                }
                else
                {
                    int tg = Convert.ToInt16(ToSo) - 2;
                    int x = 1;
                    for (int i = 3 + 4 * tg; i < 3 + 4 * (tg + 1); i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + x, arrDonVi[i]);
                        x++;
                    }
                }
            }
          
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
       
        /// <summary>
        /// Danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iID_MaTrangThaiDuyet,String Thang_Quy,String LoaiThang_Quy,String MaND)
        {
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy = "iThang_Quy between 1 and 3";
                        break;
                    case "2":
                        DKThangQuy = "iThang_Quy between 4 and 6";
                        break;
                    case "3":
                        DKThangQuy = "iThang_Quy between 7 and 9";
                        break;
                    case "4":
                        DKThangQuy = "iThang_Quy between 10 and 12";
                        break;
                    default:
                        DKThangQuy="iThang_Quy=-1";
                        break;
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@ThangQuy";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            }
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT b.sTen,a.iID_MaDonVi
                                        FROM (SELECT iID_MaDonVi FROM QTA_ChungTuChiTiet 
	                                          WHERE  bLoaiThang_Quy=0 AND iTrangThai=1 AND sLNS='1010000' {1} AND ({0}) {2}
	                                          ) as A
                                        INNER JOIN ( SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec )as b ON a.iID_MaDonVi=b.iID_MaDonVi
                                        ORDER BY iID_MaDonVi
                                        ", DKThangQuy,ReportModels.DieuKien_NganSach(MaND),iID_MaTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            if(LoaiThang_Quy=="0")
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonVi;
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
        public DataTable dtTo(String KhoGiay, String iID_MaDonVi)
        {
            String[] arrDomVi = iID_MaDonVi.Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("TenTo", typeof(String));
            dt.Columns.Add("MaTo", typeof(String));
            DataRow dr = dt.NewRow();
            dr[0] = "Tờ 1";
            dr[1] = "1";
            dt.Rows.InsertAt(dr, 0);

            //giay a3
            if(KhoGiay=="1")
            {
                int a = 2;
                for (int i = 5; i < arrDomVi.Length;i=i+6)
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
                for (int i = 3; i < arrDomVi.Length; i = i + 4)
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
    }
}
