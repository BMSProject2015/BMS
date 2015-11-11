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

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_DanhSachCaNhan_NguoiPhuThuocController : Controller
    {
        //
        // GET: /rptLuong_DanhSachCaNhan_NguoiPhuThuoc/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/Luong/rptLuong_DS_CaNhan_NguoiPT.xls";
        public static String NameFile = "";
        public int count = 0;
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_DanhSachCaNhan_NguoiPhuThuoc.aspx";
            return View(sViewPath + "ReportView_NoMaster.aspx");            
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThangLuong"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNamLuong"]);            
            return RedirectToAction("Index", new { Thang = Thang, Nam = Nam });
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        #region "Lấy dữ liệu"
        public static DataTable DanhSachCaNhan_NguoiPhuThuoc(String MaBL)
        {
            String SQL = String.Format(@"SELECT
                                        ROW_NUMBER() OVER (ORDER BY iID_MaDonVi) AS STT
	                                    ,L.iID_MaDonVi
                                        ,L.sHoDem_CanBo as HoDem
                                        ,sTen_CanBo AS Ten
                                        ,L.sSoSoLuong_CanBo AS SSL
                                        ,L.iSoNguoiPhuThuoc_CanBo AS SNPT
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                          AND L.iID_MaBangLuong=@iID_MaBangLuong                                       		
                                        ORDER BY
                                            L.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);                  
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", MaBL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        #endregion
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        #region "Tạo báo cáo"
        public ExcelFile CreateReport(String path, String MaBL)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            using (FlexCelReport fr = new FlexCelReport())
            {
                LoadData(fr, MaBL);
                fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
                fr.Run(Result);
                return Result;
            }
        }
        #endregion
        /// <summary>
        /// Đổ dữ liệu xuống file báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        #region "Đổ dữ liệu xuống file báo cáo"
        private void LoadData(FlexCelReport fr, String MaBL)
        {
            int SoHang1Trang = 43;
            DataTable data = DanhSachCaNhan_NguoiPhuThuoc(MaBL);            
            int SoHang = data.Rows.Count;
            DataTable data1 = new DataTable();
            String TenCot = "";
            for (int i = 0; i < data.Columns.Count; i++)
            {
                TenCot = data.Columns[i].ColumnName;
                data1.Columns.Add(TenCot, data.Columns[i].DataType);
            }
            int SoTrang =(int)SoHang / SoHang1Trang;
            if (SoHang > SoHang1Trang)
            {
                dtDuLieu _dtDuLieu = TachData(data, SoTrang, SoHang1Trang);
                data = _dtDuLieu.data;
                data1 = _dtDuLieu.data1;
            }
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDonVi = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi", "", "");
            dtDonVi.TableName = "DonVi";
            fr.AddTable("DonVi", dtDonVi);

            data1.TableName = "ChiTiet1";
            fr.AddTable("ChiTiet1", data1);
            DataTable dtDonVi1 = HamChung.SelectDistinct("DonVi1", data1, "iID_MaDonVi", "iID_MaDonVi", "", "");
            
            dtDonVi1.TableName = "DonVi1";
            fr.AddTable("DonVi1", dtDonVi1);

            dtDonVi1.Dispose();
            data1.Dispose();
            dtDonVi.Dispose();
            data.Dispose();
        }
        #endregion

        public class dtDuLieu
        {
            public DataTable data { get; set; }
            public DataTable data1 { get; set; }

        }
        public dtDuLieu TachData(DataTable dtNguon,int SoLanTach,int SoBanGhi)
        {
            DataTable data = new DataTable();
            String TenCot = "";        
            for (int i = 0; i < dtNguon.Columns.Count; i++)
            {
                TenCot = dtNguon.Columns[i].ColumnName;
                data.Columns.Add(TenCot, dtNguon.Columns[i].DataType);
            }
            int SoHang = 0;
            for(int i=0;i<SoLanTach;i++)
            {
                if (i * SoBanGhi + SoBanGhi < dtNguon.Rows.Count) SoHang = i * SoBanGhi + SoBanGhi;
                else SoHang = dtNguon.Rows.Count;
                for(int j=i*SoBanGhi;j< SoHang;j++)
                {                  
                        data.NewRow();                                                
                        data.Rows.Add(dtNguon.Rows[j].ItemArray);                                           
                }
                int d=0;
                int CS;
                int SoBaGhi_Remove=SoHang-i*SoBanGhi;
                while (d < SoBaGhi_Remove)
                {
                    CS = i * SoBaGhi_Remove;
                    dtNguon.Rows.RemoveAt(CS);
                    d = d + 1;
                }
            }

            dtDuLieu _dtDuLieu = new dtDuLieu();
            _dtDuLieu.data = data;
            _dtDuLieu.data1 = dtNguon;
            return _dtDuLieu;
        }
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaBL)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaBL);
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

        public clsExcelResult ExportToExcel(String MaBL)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath),MaBL);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "Luong_NguoiPhuThuoc.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaBL)
        {
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaBL);
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
    }
}