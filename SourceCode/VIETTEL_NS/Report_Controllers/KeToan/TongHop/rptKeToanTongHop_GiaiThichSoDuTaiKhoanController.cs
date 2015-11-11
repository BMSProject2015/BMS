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
using System.Collections.Specialized;
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_GiaiThichSoDuTaiKhoanController : Controller
    {
        //
        // GET: /rptKeToanTongHop_GiaiThichSoDuTaiKhoan/
        public string sViewPath = "~/Report_Views/";
        public string Count = "";
        public decimal TienNo = 0;
        public decimal TienCo = 0;
        public decimal TienDu = 0;
        // public decimal DuCo = 0;
        private const String sFilePath_NoiDung_Doc = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoNoiDung.xls";
        private const String sFilePath_NoiDung_Ngang = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoNoiDung_Ngang.xls";
        private const String sFilePath_DonVi_Doc = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonVi.xls";
        private const String sFilePath_DonVi_Ngang = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonVi_Ngang.xls";
        private const String sFilePath_DonViNoiDung = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung.xls";
        private const String sFilePath_DonViNoiDung_Ngang = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung_Ngang.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTaiKhoan.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            int i;
            String MaTaiKhoan = Request.Form[ParentID + "_MaTaiKhoan"];
            String Thang = Request.Form[ParentID + "_Thang"];
            String Nam = Request.Form[ParentID + "_Nam"];
            String LoaiBieu = Request.Form[ParentID + "_divLoaiBieu"];
            String KieuGiay = Request.Form[ParentID + "_KieuGiay"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            NameValueCollection arrLoi = new NameValueCollection();
            if (String.IsNullOrEmpty(MaTaiKhoan) == true || MaTaiKhoan == "" || MaTaiKhoan == null)
            {
                arrLoi.Add("err_MaTaiKhoan", "Chọn tài khoản");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["PageLoad"] = "0";
                return View("~/Report_Views/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTaiKhoan.aspx");
            }
            else
            {

                ViewPDF(MaTaiKhoan, Thang, Nam, LoaiBieu, KieuGiay, iID_MaTrangThaiDuyet);
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTaiKhoan.aspx";
                ViewData["PageLoad"] = "1";
                ViewData["MaTaiKhoan"] = MaTaiKhoan;
                ViewData["Thang"] = Thang;
                ViewData["Nam"] = Nam;
                ViewData["LoaiBieu"] = LoaiBieu;
                ViewData["KieuGiay"] = KieuGiay;
                ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
                return View(sViewPath + "ReportView.aspx");
            }
        }
        /// <summary>
        /// Tao báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaTaiKhoan = "", String Thang = "", String iNamLamViec = "", String LoaiBieu = "", String iID_MaTrangThaiDuyet = "")
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String TenDVCapTren = ReportModels.CauHinhTenDonViSuDung(1);
            String tendv = ReportModels.CauHinhTenDonViSuDung(2);
            String TenTK = TaiKhoanModels.getTenTK(MaTaiKhoan);
            //decimal TienDu = TienNo - TienCo;
            //using (FlexCelReport fr = new FlexCelReport())
            //{
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_GiaiThichSoDuTaiKhoan");
            LoadData(fr, MaTaiKhoan, Thang, iNamLamViec, LoaiBieu, iID_MaTrangThaiDuyet);
            fr.SetValue("GiaiThich", "GIẢI THÍCH SỐ DƯ TÀI KHOẢN " + MaTaiKhoan);
            fr.SetValue("TaiKhoan", TenTK);
            fr.SetValue("ThoiGian", "Đến tháng " + Thang + " năm " + iNamLamViec);
            fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
            fr.SetValue("TenDV", tendv.ToUpper());
            if (TienNo != 0)
            {
                fr.SetValue("TongTienNo", CommonFunction.DinhDangSo(TienNo));
                if (TienDu != 0)
                {
                    fr.SetValue("DuNo", CommonFunction.DinhDangSo(TienDu));
                }
                else
                {
                    fr.SetValue("DuNo", "");
                }
                fr.SetValue("DuCo", "");
                fr.SetValue("TongTienCo", "");
            }

            else if (TienCo != 0)
            {
                fr.SetValue("TongTienCo", CommonFunction.DinhDangSo(TienCo));
                if (TienDu != 0)
                {
                    fr.SetValue("DuCo", CommonFunction.DinhDangSo(TienDu));
                }
                else
                {
                    fr.SetValue("DuCo", "");
                }
                fr.SetValue("DuNo", "");
                fr.SetValue("TongTienNo", "");
            }
            else
            {
                fr.SetValue("TongTienNo", "");
                fr.SetValue("TongTienCo", "");
                fr.SetValue("DuNo", "");
                fr.SetValue("DuCo", "");
            }

            fr.SetValue("Ngay", ReportModels.Ngay_Thang_Nam_HienTai());

            fr.Run(Result);
            return Result;
            //}
        }
        /// <summary>
        /// Tạo bảng 
        /// </summary>
        /// <returns></returns>
        private DataTable CreatTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaTaiKhoan", typeof(string));
            dt.Columns.Add("DonVi", typeof(string));
            dt.Columns.Add("NoiDung", typeof(string));
            dt.Columns.Add("SoTienNo", typeof(string));
            dt.Columns.Add("SoTienCo", typeof(string));
            return dt;
        }
        /// <summary>
        /// Lấy danh sách chứng từ
        /// </summary>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        /// <returns></returns>
        private DataTable get_DanhSach_ChungTu(String MaTaiKhoan = "", String Thang = "", String iNamLamViec = "", String LoaiBieu = "", String iID_MaTrangThaiDuyet = "")
        {
            if (String.IsNullOrEmpty(MaTaiKhoan) == false && MaTaiKhoan != "")
            {
                DataTable dt = null;
                SqlCommand cmd = new SqlCommand();
                String DKTrangThaiDuyet = "";
                if (iID_MaTrangThaiDuyet == "2")
                {
                    DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                }
                if (iID_MaTrangThaiDuyet == "-100")
                {
                    DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
                }
                String SQL = "";
                if (LoaiBieu == "rNoiDung")
                {
                    SQL = String.Format(@"SELECT iID_MaTaiKhoan_No as MaTaiKhoan,sTen,SUM(Tien_No)*1 as rTienNo,SUM(Tien_Co) as rTienCo
                            FROM(
                            SELECT * FROM(
                            SELECT iID_MaTaiKhoan_No,
                            Tien_No= SUM(rSoTien) ,
                            Tien_Co=0
                            FROM KT_ChungTuChiTiet
                            WHERE iID_MaTaiKhoan_No LIKE '{0}%' AND iTrangThai=1 
                            AND iNamLamViec=@iNamLamViec AND iThangCT<=@Thang {1}
                            GROUP BY iID_MaTaiKhoan_No) as a
                            INNER JOIN (SELECT sTen,iID_MaTaiKhoan FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNamLamViec) as b
                            ON a.iID_MaTaiKhoan_No=b.iID_MaTaiKhoan
                            UNION ALL
							SELECT * FROM(
                            SELECT iID_MaTaiKhoan_Co,
                            Tien_No=0,
                            Tien_Co= SUM(rSoTien) 
                            FROM KT_ChungTuChiTiet
                            WHERE iID_MaTaiKhoan_Co LIKE '{0}%' AND iTrangThai=1 
                            AND iNamLamViec=@iNamLamViec AND iThangCT<=@Thang AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
                            GROUP BY iID_MaTaiKhoan_Co) as a
                             INNER JOIN (SELECT sTen,iID_MaTaiKhoan FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNamLamViec) as b
                            ON a.iID_MaTaiKhoan_Co=b.iID_MaTaiKhoan) as c
                            GROUP BY iID_MaTaiKhoan_No,sTen", MaTaiKhoan, DKTrangThaiDuyet);
                }
                else if (LoaiBieu == "rDonVi")
                {
                    SQL = String.Format(@"SELECT iiD_MaDonVi_No as MaDonVi,SUBSTRING(sTenDonVi_No,5,1000)  as TenDonVi, SUM(Tien_No) as rTien_No,SUM(Tien_Co) as rTien_Co
 FROM(
 SELECT iiD_MaDonVi_No,sTenDonVi_No,
                            Tien_No= SUM(rSoTien) ,
                            Tien_Co=0
                            FROM KT_ChungTuChiTiet
                            WHERE iID_MaTaiKhoan_No LIKE '{0}%' AND iTrangThai=1 
                            AND iNamLamViec=@iNamLamViec AND iThangCT<=@Thang {1}
                            GROUP BY iiD_MaDonVi_No,sTenDonVi_No
                            UNION ALL
SELECT iiD_MaDonVi_Co,sTenDonVi_Co,
                           Tien_No=0,
						   Tien_Co= SUM(rSoTien) 
                            FROM KT_ChungTuChiTiet
                            WHERE iID_MaTaiKhoan_Co LIKE '{0}%' AND iTrangThai=1 
                            AND iNamLamViec=@iNamLamViec AND iThangCT<=@Thang AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
                            GROUP BY iiD_MaDonVi_Co,sTenDonVi_Co     ) as a
                            GROUP BY   iiD_MaDonVi_No,  sTenDonVi_No                
                            ", MaTaiKhoan, DKTrangThaiDuyet);
                }
                else
                {
                    SQL = String.Format(@"SELECT sTen,iiD_MaDonVi_No as MaDonVi,SUBSTRING(sTenDonVi_No,5,1000)  as TenDonVi, SUM(Tien_No) as rTien_No,SUM(Tien_Co) as rTien_Co
                                         FROM(
                                         SELECT * FROM (
							SELECT iID_MaTaiKhoan_No,iiD_MaDonVi_No,sTenDonVi_No,
                            Tien_No= SUM(rSoTien) ,
                            Tien_Co=0
                            FROM KT_ChungTuChiTiet
                            WHERE iID_MaTaiKhoan_No LIKE '{0}%' AND iTrangThai=1 
                            AND iNamLamViec=@iNamLamViec AND iThangCT<=@Thang {1}
                            GROUP BY iID_MaTaiKhoan_No,iiD_MaDonVi_No,sTenDonVi_No) as a
                             INNER JOIN (
                             SELECT sTen,iID_MaTaiKhoan FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=2012) as b
                            ON a.iID_MaTaiKhoan_No=b.iID_MaTaiKhoan
                            UNION ALL
                            SELECT * FROM (
							SELECT iID_MaTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,
                           Tien_No=0,
						   Tien_Co= SUM(rSoTien) 
                            FROM KT_ChungTuChiTiet
                            WHERE iID_MaTaiKhoan_Co LIKE '{0}%' AND iTrangThai=1 
                            AND iNamLamViec=@iNamLamViec AND iThangCT<=@Thang AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
                            GROUP BY iID_MaTaiKhoan_Co,iiD_MaDonVi_Co,sTenDonVi_Co ) as c
                            INNER JOIN (
                             SELECT sTen,iID_MaTaiKhoan FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNamLamViec) as d
                               ON c.iID_MaTaiKhoan_Co=d.iID_MaTaiKhoan
                             ) as e
                            GROUP BY   iiD_MaDonVi_No,  sTenDonVi_No ,sTen               
                            ", MaTaiKhoan, DKTrangThaiDuyet);
                }
                //if (String.IsNullOrEmpty(Thang) == false && Thang != "")
                //{
                //    SQL += " AND (iThangCT = @Thang)";
                //    cmd.Parameters.AddWithValue("@Thang", Thang);

                //}
                cmd.Parameters.AddWithValue("@Thang", Thang);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                // cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoan);
                if (iID_MaTrangThaiDuyet != "-1")
                {
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                }
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                //  DataTable dtMain = CreatTable();
                // string SoTien = "", DonViNo = "", DonViCo = "";

                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        DataRow dr = dt.Rows[i];
                //        DataRow drMain = dtMain.NewRow();
                //        SoTien = HamChung.ConvertToString(dr["SoTien"]);
                //        if (LoaiBieu != "rNoiDung")
                //        {
                //            DonViNo = HamChung.ConvertToString(dr["sTenDonVi_No"]);
                //            DonViCo = HamChung.ConvertToString(dr["sTenDonVi_Co"]);
                //        }
                //        drMain["MaTaiKhoan"] = MaTaiKhoan;
                //        if (String.IsNullOrEmpty(SoTien) == false && SoTien != "" && Convert.ToDouble(SoTien) < 0)
                //        {
                //            drMain["DonVi"] = DonViNo;
                //            drMain["SoTienNo"] = CommonFunction.DinhDangSo(Convert.ToDouble(SoTien));
                //            drMain["SoTienCo"] = "";
                //            TienNo += Convert.ToDecimal(SoTien);
                //        }
                //        else
                //        {
                //            drMain["DonVi"] = DonViCo;
                //            drMain["SoTienNo"] = "";
                //            drMain["SoTienCo"] = CommonFunction.DinhDangSo(SoTien);
                //            TienCo += Convert.ToDecimal(SoTien);
                //        }
                //        drMain["NoiDung"] = HamChung.ConvertToString(dr["NoiDung"]);                      

                //        dtMain.Rows.Add(drMain);
                //    }
                //    TienDu = TienNo - TienCo;
                //}
                cmd.Dispose();
                if (dt != null) dt.Dispose();
                //Đếm số dòng
                //  Count = dtMain.Rows.Count.ToString();
                return dt;
            }
            else
            {
                DataTable dtMain = CreatTable();
                return dtMain;
            }
        }
        /// <summary>
        /// Load ra danh sách chứng từ đổ dữ liệu vào bảng chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        private void LoadData(FlexCelReport fr, String MaTaiKhoan = "", String Thang = "", String Nam = "", String LoaiBieu = "", String iID_MaTrangThaiDuyet = "")
        {

            DataTable data = get_DanhSach_ChungTu(MaTaiKhoan, Thang, Nam, LoaiBieu, iID_MaTrangThaiDuyet);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow r = data.Rows[i];
                if (LoaiBieu == "rNoiDung")
                {
                    if (Convert.ToString(r["rTienNo"]) == Convert.ToString(r["rTienCo"]))
                    {
                        data.Rows.Remove(r);
                    }
                }

                else
                {
                    if (Convert.ToString(r["rTien_No"]) == Convert.ToString(r["rTien_Co"]))
                    {
                        data.Rows.Remove(r);
                    }
                }
            }
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
        public ActionResult ViewPDF(String MaTaiKhoan = "", String Thang = "", String Nam = "", String LoaiBieu = "", String KieuGiay = "", String iID_MaTrangThaiDuyet = "")
        {
            HamChung.Language();
            String sFilePath = "";
            if (KieuGiay == "rdoc")
            {
                if (LoaiBieu == "rDonViNoiDung")
                    sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung.xls";
                else if (LoaiBieu == "rDonVi")
                    sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonVi.xls";
                else
                    sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoNoiDung.xls";















            }
            else
            {

                if (LoaiBieu == "rDonViNoiDung")
                    sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonViNoiDung_Ngang.xls";
                else if (LoaiBieu == "rDonVi")
                    sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoDonVi_Ngang.xls";
                else
                    sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDuTheoNoiDung_Ngang.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaTaiKhoan, Thang, Nam, LoaiBieu, iID_MaTrangThaiDuyet);
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
        public static DataTable dtKieuGiay()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaKieuGiay");
            dt.Columns.Add("TenKieuGiay");
            DataRow dr = dt.NewRow();
            dr[0] = "rdoc";
            dr[1] = "In khổ dọc";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "rngang";
            dr1[1] = "In khổ ngang";
            dt.Rows.Add(dr1);
            dt.Dispose();
            return dt;
        }

        public JsonResult Get_objTaiKhoan(String CapTK,String iNamLamViec)
        {
            var dt = TaiKhoan(Convert.ToInt16(CapTK), iNamLamViec);
            JsonResult value = Json(HamChung.getGiaTri("iID_MaTaiKhoan", "sTen", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        public static DataTable TaiKhoan(int CapTK = 3, String iNamLamViec = "2012")
        {
            String SQL = "SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam AND LEN(iID_MaTaiKhoan)>2 AND SUBSTRING(iID_MaTaiKhoan,1,1)<>0";
            if (CommonFunction.IsNumeric(CapTK) && CapTK != 0)
            {
                SQL += String.Format(" AND LEN(iID_MaTaiKhoan) <= {0} ", CapTK);
            }
            SQL += " group by iID_MaTaiKhoan,sTen ";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }

       
    }
}