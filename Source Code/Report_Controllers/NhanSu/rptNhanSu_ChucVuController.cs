using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using DomainModel;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using VIETTEL.Controllers;
using VIETTEL.Models;

namespace VIETTEL.Report_Controllers.NhanSu
{
    public class rptNhanSu_ChucVuController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/NhanSu/rptNhanSu_ChucVu.xls";
        private const String sFilePath_DonVi = "/Report_ExcelFrom/NhanSu/rptNhanSu_ChucVuTheoDonVi.xls";
        private const String sFilePath_LoaiDonVi = "/Report_ExcelFrom/NhanSu/rptNhanSu_ChucVuTheoLoaiDonVi.xls";
        private const String sFilePath_TH = "/Report_ExcelFrom/NhanSu/rptNhanSu_TH_ChucVu.xls";
        private const String sFilePath_DonVi_TH = "/Report_ExcelFrom/NhanSu/rptNhanSu_TH_ChucVu.xls";
        private const String sFilePath_LoaiDonVi_TH = "/Report_ExcelFrom/NhanSu/rptNhanSu_TH_ChucVu.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_ChucVu.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            ViewData["PageLoad"] = "0";
            ViewData["Cap"] = "0";
            ViewData["Nam"] = DateTime.Now.Year;
            ViewData["bBaoCaoTH"] = "False";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String ChucVu = Convert.ToString(Request.Form[ParentID + "_ChucVu"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            String Cap = Convert.ToString(Request.Form[ParentID + "_Cap"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_Nam"]);
            ViewData["PageLoad"] = "1";
            ViewData["ChucVu"] = ChucVu;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["Cap"] = Cap;
            ViewData["Nam"] = Nam;
            //ViewData["bBaoCaoTH"] = "False";
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_ChucVu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(string submitButton,String ParentID)
        {
            String ChucVu = Convert.ToString(Request.Form[ParentID + "_ChucVu"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            String Cap = Convert.ToString(Request.Form[ParentID + "_Cap"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_Nam"]);
            ViewData["PageLoad"] = "1";
            ViewData["ChucVu"] = ChucVu;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["Cap"] = Cap;
            ViewData["Nam"] = Nam;
            ViewData["bBaoCaoTH"] = submitButton.ToUpper().Equals("DANH SÁCH") ? "False" : "true";
            //ViewData["bBaoCaoTH"] = "False";
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_ChucVu.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String ChucVu, string LoaiIn, string Cap,string Nam)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
           
            //tính tổng tiền
            if (Nam == null) Nam = "0";
            DataTable dt = PhanTichDuToanNS_52(ChucVu, LoaiIn,Cap,Nam);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //Toan lục luong
                fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_ChucVu");
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //Theo don vi 
                fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_ChucVuTheoDonVi");
            }
            else
            {
                //Loai don vi: khoi du an va khoi doanh nghiep
                fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_ChucVu_TheoLoaiDonVi");
            }
            LoadData(fr, ChucVu, LoaiIn,Cap,Nam);
            if (Nam != "0" & Nam != "")
                fr.SetValue("Nam", "NĂM " + Nam.ToUpper());
            else
                fr.SetValue("Nam", "");

            //if (Cap != "0" & Cap != "")
            //    fr.SetValue("Cap", " - CÁN BỘ CẤP " + Cap.ToUpper());
            //else
            //    fr.SetValue("Cap","");
            string sChucVu = "";
            if (ChucVu == "0" || ChucVu == "")
            {
                sChucVu = "(TOÀN QUÂN)";
            }
            else
            {
                sChucVu = "(Chức vụ " + L_DanhMucChucVu_Get_Ten(ChucVu) + ")";
            }
            fr.SetValue("ChucVu", sChucVu.ToUpper());
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport_TH(String path, String ChucVu, string LoaiIn, string Cap, string Nam)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            if (Nam == null) Nam = "0";
            //tính tổng tiền
            DataTable dt = PhanTichDuToanNS_52(ChucVu, LoaiIn, Cap, Nam);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //Toan lục luong
                fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_ChucVu");
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //Theo don vi 
                fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_ChucVuTheoDonVi");
            }
            else
            {
                //Loai don vi: khoi du an va khoi doanh nghiep
                fr = ReportModels.LayThongTinChuKy(fr, "rptNhanSu_ChucVu_TheoLoaiDonVi");
            }
            LoadData_TH(fr, ChucVu, LoaiIn, Cap, Nam);
            if (Nam != "0" & Nam != "")
                fr.SetValue("Nam", "NĂM " + Nam.ToUpper());
            else
                fr.SetValue("Nam", "");

            //if (Cap != "0" & Cap != "")
            //    fr.SetValue("Cap", " - CÁN BỘ CẤP " + Cap.ToUpper());
            //else
            //    fr.SetValue("Cap", "");
            string sChucVu = "";
            if (ChucVu == "0" || ChucVu == "")
            {
                sChucVu = "(TOÀN QUÂN)";
            }
            else
            {
                sChucVu = "(Chức vụ " + L_DanhMucChucVu_Get_Ten(ChucVu) + ")";
            }
            fr.SetValue("ChucVu", sChucVu.ToUpper());
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }

        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String ChucVu, String LoaiIn, String Cap, String Nam)
        {

            DataTable data = PhanTichDuToanNS_52(ChucVu, LoaiIn, Cap, Nam);
            data.TableName = "NS";
            fr.AddTable("NS", data);
            DataTable dtGroup;
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //Toan lục luong
                dtGroup = HamChung.SelectDistinct("Group", data, "iID_MaDonViCha,sTenDonViCha", "iID_MaDonViCha,sTenDonViCha");
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //Theo don vi 
                dtGroup = HamChung.SelectDistinct("Group", data, "iID_MaDonViCha,sTenDonViCha", "iID_MaDonViCha,sTenDonViCha");
            }
            else
            {
                //Loai don vi: khoi du an va khoi doanh nghiep
                dtGroup = HamChung.SelectDistinct("Group", data, "iID_MaLoaiDonVi,sTenLoaiDonVi", "iID_MaLoaiDonVi,sTenLoaiDonVi");
            }
            fr.AddTable("GROUP", dtGroup);

            data.Dispose();
            dtGroup.Dispose();
        }


        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData_TH(FlexCelReport fr, String ChucVu, String LoaiIn, String Cap, String Nam)
        {

            DataTable data = PhanTichDuToanNS_52(ChucVu, LoaiIn, Cap, Nam);

            //lay du lieu tong hop

            DataTable dtTongHop = new DataTable();
            //Add Column
            dtTongHop.Columns.Add("iTt", typeof (Int32));
            dtTongHop.Columns.Add("sMaDonVi", typeof(String));
            dtTongHop.Columns.Add("sTenDonVi", typeof(String));
            dtTongHop.Columns.Add("iNam", typeof(Int32));
            dtTongHop.Columns.Add("iNu", typeof(Int32));
            dtTongHop.Columns.Add("iSqCapUy", typeof(Int32));
            dtTongHop.Columns.Add("iSqCapTa", typeof(Int32));
            dtTongHop.Columns.Add("iSqCapTuong", typeof(Int32));
            dtTongHop.Columns.Add("iQncnCapUy", typeof(Int32));
            dtTongHop.Columns.Add("iQncnCapTa", typeof(Int32));
            dtTongHop.Columns.Add("iCnvqpBienChe", typeof(Int32));
            dtTongHop.Columns.Add("iCnvqpHopDong", typeof(Int32));
            dtTongHop.Columns.Add("iHsqcs", typeof(Int32));
            dtTongHop.Columns.Add("iCvLdCapCuc", typeof(Int32));
            dtTongHop.Columns.Add("iCvLdCapPhong", typeof(Int32));
            dtTongHop.Columns.Add("iCvLdCapTBPTKTT", typeof(Int32));
            dtTongHop.Columns.Add("iCvTroLy", typeof(Int32));
            dtTongHop.Columns.Add("iCvNhanVien", typeof(Int32));
            dtTongHop.Columns.Add("iTdTrenDaiHoc", typeof(Int32));
            dtTongHop.Columns.Add("iTdDaiHoc", typeof(Int32));
            dtTongHop.Columns.Add("iTdTrungCap", typeof(Int32));
            dtTongHop.Columns.Add("iTdSoCap", typeof(Int32));
            dtTongHop.Columns.Add("iTdChuaHoc", typeof(Int32));
            dtTongHop.Columns.Add("sMaGroup", typeof(String));
            dtTongHop.Columns.Add("sTenGroup", typeof(String));
            
            DataTable dtGroup;
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //Toan lục luong
                dtGroup = HamChung.SelectDistinct("NS", data, "iID_MaDonVi,sTenDonVi", "iID_MaDonVi,sTenDonVi,iID_MaDonViCha,sTenDonViCha");
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //Theo don vi 
                dtGroup = HamChung.SelectDistinct("NS", data, "iID_MaDonVi,sTenDonVi", "iID_MaDonVi,sTenDonVi,iID_MaDonViCha,sTenDonViCha");
            }
            else
            {
                //Loai don vi: khoi du an va khoi doanh nghiep
                dtGroup = HamChung.SelectDistinct("NS", data, "iID_MaLoaiDonVi,sTenLoaiDonVi", "iID_MaDonVi,sTenDonVi,iID_MaDonViCha,sTenDonViCha");
            }
            
            //Add du lieu Th\
            if(dtGroup != null)
            {
                DataView dvTemp = dtGroup.AsDataView();
                dvTemp.Sort = "iID_MaDonViCha asc";
                dtGroup = dvTemp.ToTable();
                int i = 0;
                foreach (DataRow row in dtGroup.Rows)
                {
                    //dem du lieu
                    DataRow drTemp = dtTongHop.NewRow();
                    drTemp["iTt"] = i +1;
                    drTemp["sMaDonVi"] = Convert.ToString(row["iID_MaDonVi"]);
                    drTemp["sTenDonVi"] = Convert.ToString(row["sTenDonVi"]);
                    drTemp["sMaGroup"] = Convert.ToString(row["iID_MaDonViCha"]);
                    drTemp["sTenGroup"] = Convert.ToString(row["sTenDonViCha"]);
                    drTemp["iNam"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iNuQuanNhan=0").Length;
                    drTemp["iNu"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iNuQuanNhan<>0").Length;
                    drTemp["iSqCapUy"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=1 and iID_MaBacLuong in ('11','12','13','14')").Length;
                    drTemp["iSqCapTa"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=1 and iID_MaBacLuong in ('21','22','23','24')").Length;
                    drTemp["iSqCapTuong"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=1 and iID_MaBacLuong in ('31','32','33','34')").Length;
                    drTemp["iQncnCapUy"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=2 and iID_MaBacLuong in ('11','12','13','14')").Length;
                    drTemp["iQncnCapTa"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=2 and iID_MaBacLuong in ('21','22','23','24')").Length;

                    string sFind = "";
                    int ivalue = 0;
                    //lay danh sach ma theo bien che
                    DataTable dtTemp = HamChung.GetDataTable("L_DanhMucBacLuong", "iID_MaBacLuong", "sQuanHam",
                                                             "rCNVQPCT", true, "");
                    if(dtTemp != null)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (sFind == "")
                                sFind = "'" + dr[0].ToString() + "'";
                            else
                                sFind += ",'" + dr[0].ToString() + "'";
                        }
                        sFind = " iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=3 and iID_MaBacLuong in (" + sFind + ")";
                        ivalue = data.Select(sFind).Length;
                    }
                    drTemp["iCnvqpBienChe"] = ivalue;
                    //lay danh sach ma theo hop dong
                    sFind = "";
                    ivalue = 0;
                    dtTemp = HamChung.GetDataTable("L_DanhMucBacLuong", "iID_MaBacLuong", "sQuanHam",
                                                             "rQNVQPHD", true, "");
                    if (dtTemp != null)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (sFind == "")
                                sFind = "'" + dr[0].ToString() + "'";
                            else
                                sFind += ",'" + dr[0].ToString() + "'";
                        }
                        sFind = " iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=3 and iID_MaBacLuong in (" + sFind + ")";
                        ivalue = data.Select(sFind).Length;
                    }
                    drTemp["iCnvqpHopDong"] = ivalue;
                    //lay danh sach ma theo ha si quan
                     sFind = "";
                    ivalue = 0;
                    dtTemp = HamChung.GetDataTable("L_DanhMucBacLuong", "iID_MaBacLuong", "sQuanHam",
                                                             "rHaSi", true, "");
                    if (dtTemp != null)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (sFind == "")
                                sFind = "'" + dr[0].ToString() + "'";
                            else
                                sFind += ",'" + dr[0].ToString() + "'";
                        }
                        sFind = " iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_MaNgachLuong=3 and iID_MaBacLuong in (" + sFind + ")";
                        ivalue = data.Select(sFind).Length;
                    }
                    drTemp["iHsqcs"] = ivalue;

                    drTemp["iCvLdCapCuc"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_ChucVuHienTai in ('05')").Length;
                    drTemp["iCvLdCapPhong"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_ChucVuHienTai in ('04')").Length;
                    drTemp["iCvLdCapTBPTKTT"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_ChucVuHienTai in ('03')").Length;
                    drTemp["iCvTroLy"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_ChucVuHienTai in ('02')").Length;
                    drTemp["iCvNhanVien"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and iID_ChucVuHienTai in ('01')").Length;
                    drTemp["iTdTrenDaiHoc"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and MaTrinhDo in ('01','02','03')").Length;//tien si, thac si, tren dh
                    drTemp["iTdDaiHoc"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and MaTrinhDo in ('04')").Length;
                    drTemp["iTdTrungCap"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and MaTrinhDo in ('05')").Length;
                    drTemp["iTdSoCap"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and MaTrinhDo in ('06')").Length;
                    drTemp["iTdChuaHoc"] = data.Select(" iID_MaDonVi= '" + row["iID_MaDonVi"].ToString() + "' and MaTrinhDo in ('07')").Length;
                    dtTongHop.Rows.Add(drTemp);
                    i++;
                }
            }
            if (dtTongHop != null)
            {
                DataView dvTemp = dtTongHop.AsDataView();
                dvTemp.Sort = "sMaGroup,sMaDonVi asc";
                dtTongHop = dvTemp.ToTable();
            }
            fr.AddTable("NS", dtTongHop);

            DataTable dtG;
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //Toan lục luong
                dtG = HamChung.SelectDistinct("GROUP", dtTongHop, "sMaGroup,sTenGroup", "sMaGroup,sTenGroup");
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //Theo don vi 
                dtG = HamChung.SelectDistinct("GROUP", dtTongHop, "sMaGroup,sTenGroup", "sMaGroup,sTenGroup");
            }
            else
            {
                //Loai don vi: khoi du an va khoi doanh nghiep
                dtG = HamChung.SelectDistinct("GROUP", dtTongHop, "sMaGroup,sTenGroup", "sMaGroup,sTenGroup");
            }
            if (dtG != null)
            {
                DataView dvTemp = dtG.AsDataView();
                dvTemp.Sort = "sMaGroup asc";
                dtG = dvTemp.ToTable();
            }
            fr.AddTable("GROUP", dtG);
            data.Dispose();
            dtGroup.Dispose();
            dtG.Dispose();
            dtTongHop.Dispose();
        }

        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String ChucVu, String LoaiIn, String Cap, String Nam)
        {
            String DuongDanFile = sFilePath;
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //tat ca don vi
                DuongDanFile = sFilePath;
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //theo don vi 
                DuongDanFile = sFilePath_DonVi;
            }
            else
            {
                //loại đơn vị
                DuongDanFile = sFilePath_LoaiDonVi;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), ChucVu, LoaiIn, Cap, Nam);
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
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String ChucVu, String LoaiIn, String Cap, String Nam)
        {
            String DuongDanFile = sFilePath;

            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //tat ca don vi
                DuongDanFile = sFilePath;
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //theo don vi 
                DuongDanFile = sFilePath_DonVi;
            }
            else
            {
                //loại đơn vị
                DuongDanFile = sFilePath_LoaiDonVi;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), ChucVu, LoaiIn, Cap, Nam);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptNhanSu_ChucVu.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        
        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String ChucVu, String LoaiIn, String Cap, String Nam)
        {
            //ViewData["bBaoCaoTH"] = "False";
            String DuongDanFile = sFilePath;
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //tat ca don vi
                DuongDanFile = sFilePath;
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //theo don vi 
                DuongDanFile = sFilePath_DonVi;
            }
            else
            {
                //loại đơn vị
                DuongDanFile = sFilePath_LoaiDonVi;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), ChucVu, LoaiIn, Cap, Nam);
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
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF_TongHop(String ChucVu, String LoaiIn, String Cap, String Nam)
        {
            ViewData["bBaoCaoTH"] = "True";
            String DuongDanFile = sFilePath_TH;
            if (LoaiIn.Equals("00000000-0000-0000-0000-000000000000"))
            {
                //tat ca don vi
                DuongDanFile = sFilePath_TH;
            }
            else if (LoaiIn.Equals("00000000-0000-0000-0000-000000000001"))
            {
                //theo don vi 
                DuongDanFile = sFilePath_DonVi_TH;
            }
            else
            {
                //loại đơn vị
                DuongDanFile = sFilePath_LoaiDonVi_TH;
            }
            ExcelFile xls = CreateReport_TH(Server.MapPath(DuongDanFile), ChucVu, LoaiIn, Cap, Nam);
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
        /// Data của báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangLamViec"></param>
        /// <param name="bThang"></param>
        /// <returns></returns>
        public DataTable PhanTichDuToanNS_52(String ChucVu, String LoaiIn, String Cap, String Nam)
        {
            DataTable dtChucVu= new DataTable();
            
            //lay du lieu thang quy
            string sThangTinKiem = "1";
            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT iID_MaCanBo, sHoTen,iNuQuanNhan, year(dNgaySinh) as sNamSinh, iID_MaNgachLuong" +
                ", CB_ChucVu.sTen as ChucVuHienTai, iID_MaBacLuong,CB_CanBo.iID_MaDonVi, NS_DonVi.sTen as sTenDonVi,sHoKhauThuongTru," +
                "NS_DonVi.iID_MaLoaiDonVi,NS_DonVi.sTenLoaiDonVi " +
                ",CB_CanBo.sNoiDaoTao, CB_CanBo.sID_MaTrinhDoChuyenMonCaoNhat as MaTrinhDo, ('C' +  CAST(iCap AS nvarchar(50))) as CapTC " +
                ", CB_CanBo.sID_ChucVuHienTai as iID_ChucVuHienTai ,sTenDonViCha,iCapDonViCha, NS_DonVi.iID_MaDonViCha" +
                " from (select *, LTRIM(RTRIM(sHoDem + ' ' + sTen)) as sHoTen from CB_CanBo) as CB_CanBo " +
                " left join CB_ChucVu ON CB_CanBo.sID_ChucVuHienTai = CB_ChucVu.sID_MaChucVu" +
                " left join (select NS_DonViCha.sTen as sTenDonViCha, NS_DonViCha.iCap as iCapDonViCha,NS_DonVi.* " +
                " from NS_DonVi left join  NS_DonVi as NS_DonViCha ON NS_DonVi.iID_MaDonViCha = NS_DonViCha.iID_MaDonVi) as NS_DonVi " +
                "ON CB_CanBo.iID_MaDonVi = NS_DonVi.iID_MaDonVi " +
                " WHERE CB_CanBo.iTrangThai=1 ";
            
            if (ChucVu != "0")
            {
                // theo thang
                SQLDSNS += " AND CB_CanBo.sID_ChucVuHienTai = @ChucVu";
            }
            if (Cap != "0")
            {
                // theo thang
                SQLDSNS += " AND NS_DonVi.iCap <= @Cap";
            }
            if (Nam !="0")
            {
                SQLDSNS += " AND year(CB_CanBo.dNgayVaoCQ) <= @Nam";
            }

            if(LoaiIn !="00000000-0000-0000-0000-000000000000" && LoaiIn !="00000000-0000-0000-0000-000000000001")
            {
                SQLDSNS += " AND NS_DonVi.iID_MaLoaiDonVi = @LoaiIn";
            }

            SQLDSNS +=" order by CB_CanBo.sTen asc";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            if (ChucVu != "0")
            {
                cmdThangNay.Parameters.AddWithValue("@ChucVu", ChucVu);
            }
            if (Cap != "0") 
            {
                // theo cap
                cmdThangNay.Parameters.AddWithValue("@Cap", Convert.ToInt32(Cap));
            }
            if (Nam != "0")
            {
                // theo nam
                cmdThangNay.Parameters.AddWithValue("@Nam", Convert.ToInt32(Nam));
            }
            if (LoaiIn != "00000000-0000-0000-0000-000000000000" && LoaiIn != "00000000-0000-0000-0000-000000000001")
            {
                cmdThangNay.Parameters.AddWithValue("@LoaiIn", LoaiIn);
            }
            dtChucVu = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();

            return dtChucVu;
        }


        public string L_DanhMucChucVu_Get_Ten(String sID_MaChucVu)
        {
            DataTable dtChucVu = new DataTable();

            //lay du lieu thang quy
            string sTen = "";
            //Tao datatable tháng này
            String SQLDSNS =
                "SELECT top 1 sTen" +
                " from CB_ChucVu WHERE sID_MaChucVu = @sID_MaChucVu";
            SqlCommand cmdThangNay = new SqlCommand(SQLDSNS);
            cmdThangNay.Parameters.AddWithValue("@sID_MaChucVu", sID_MaChucVu);
            dtChucVu = Connection.GetDataTable(cmdThangNay);
            cmdThangNay.Dispose();
            if (dtChucVu != null)
            {
                if (dtChucVu.Rows.Count > 0) sTen = Convert.ToString(dtChucVu.Rows[0]["sTen"]);
            }
            else
            {
                sTen = "";
            }
            return sTen;
        }

    }
}
