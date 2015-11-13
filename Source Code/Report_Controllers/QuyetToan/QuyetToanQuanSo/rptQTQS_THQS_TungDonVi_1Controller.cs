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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQTQS_THQS_TungDonVi_1Controller : Controller
    {

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TungDonVi_1.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TungDonVi_1.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String iThang = Request.Form[ParentID + "_iThang"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_THQS_TungDonVi_1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String sTenDonVi = "",sTenPhuLuc="";
            if (iThang != "-1")
            {
                iNamLamViec = "Quý " + iThang + " năm " + iNamLamViec;
            }
            else
            {
                iNamLamViec = "Năm " + iNamLamViec;
            }
            if (iID_MaPhongBan == "-1")
            {
                sTenDonVi = "Toàn quân";
                sTenPhuLuc = "PL01a";
            }
            else if (iID_MaPhongBan == "-2")
            {
                sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
                sTenPhuLuc = "PL01c";
            }
            else
            {
                sTenDonVi = "B " + iID_MaPhongBan;
                sTenPhuLuc = "PL01b";
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaDonVi, iID_MaPhongBan, iThang);
            fr = ReportModels.LayThongTinChuKy(fr, "rptQTQS_THQS_TungDonVi_1");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("sTenPhuLuc", sTenPhuLuc);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.Run(Result);
            return Result;

        }

        //1020000
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptQTQS_THQS_TungDonVi_1(String MaND, String iID_MaDonVi,String iID_MaPhongBan,String iThang)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            String sTruongTien = MucLucQuanSoModels.strDSTruongTien + ",rTongSo,iID_MaChungTuChiTiet,iThang_Quy";
            String[] arrDSTruongTien = sTruongTien.Split(',');


            int iNamLamViec = 0;
            DataTable dtBienChe = QuyetToan_QuanSo_ChungTuChiTietModels.Get_QuanSoBienChe(iNamLamViec, Convert.ToInt32(iThang), iID_MaDonVi, "1");
            DataTable dtThangtruoc = QuyetToan_QuanSo_ChungTuChiTietModels.Get_QuanSoThangTruoc(iNamLamViec, Convert.ToInt32(iThang), iID_MaDonVi, "1");
            DataTable dtThangNay = QuyetToan_QuanSo_ChungTuChiTietModels.Get_QuanSoThangTruoc(iNamLamViec, Convert.ToInt32(iThang) + 1, iID_MaDonVi, "1");
            //So ke Hoach
            DK = "iTrangThai=1 ";
            
                DK += "AND sHienThi<>'2' ";
            SQL = String.Format("SELECT * FROM NS_MucLucQuanSo WHERE {0} ORDER BY sKyHieu", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            //lay du lieu bang cchi tiet
            cmd.Dispose();
            DataColumn column;
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j].StartsWith("r") == true)
                {
                    column = new DataColumn(arrDSTruongTien[j], typeof(Double));
                    column.DefaultValue = 0;
                    vR.Columns.Add(column);
                }
                else
                {
                    column = new DataColumn(arrDSTruongTien[j], typeof(String));
                    column.AllowDBNull = true;
                    vR.Columns.Add(column);
                }
            }
            DK = "iTrangThai=1";
            SQL = String.Format("SELECT * FROM QTQS_ChungTuChiTiet WHERE {0} ORDER BY sKyHieu", DK);
            cmd = new SqlCommand();
            cmd.CommandText = SQL;
            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;

            dtChungTuChiTiet.Columns.Add("sHienThi", typeof(String));
            int vRCount = vR.Rows.Count;

            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    //for (int k = 0; k < arrDSTruong.Length; k++)
                    //{
                    if (Convert.ToString(vR.Rows[i]["sKyHieu"]) != Convert.ToString(dtChungTuChiTiet.Rows[j]["sKyHieu"]))
                    {
                        ok = false;
                        //  break;
                    }
                    //}
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < vR.Columns.Count; k++)
                            {
                                if (vR.Columns[k].ColumnName == "sMoTa") continue;
                                vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row = vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count; k++)
                            {
                                row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            vR.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }
                if (Convert.ToString(vR.Rows[i]["sKyHieu"]) == "000")
                {
                    for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                    {

                        if (dtBienChe.Rows.Count > 0)
                            vR.Rows[i][dtBienChe.Columns[j + 1].ColumnName] = dtBienChe.Rows[0][j + 1];


                    }
                }
                if (Convert.ToString(vR.Rows[i]["sKyHieu"]) == "100")
                {
                    for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                    {
                        vR.Rows[i][dtThangtruoc.Columns[j].ColumnName] = dtThangtruoc.Rows[0][j];
                    }
                }
                if (Convert.ToString(vR.Rows[i]["sKyHieu"]) == "700")
                {
                    for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                    {
                        vR.Rows[i][dtThangNay.Columns[j].ColumnName] = dtThangNay.Rows[0][j];
                    }
                }
                //Tinh o tong số

                decimal tong = 0;

                for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(vR.Rows[i][arrDSTruongTien[j]])))
                        tong += Convert.ToDecimal(vR.Rows[i][arrDSTruongTien[j]]);
                }
                vR.Rows[i]["rTongSo"] = tong;



            }
            return vR;
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            DataRow r;
            DataTable data = DT_rptQTQS_THQS_TungDonVi_1(MaND, iID_MaDonVi, iID_MaPhongBan, iThang);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }

        /// <summary>
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaPhongBan, iThang);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_1010000_TungDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String iID_MaPhongBan, String iThang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, iID_MaPhongBan, iThang);
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
        }
    }
}

