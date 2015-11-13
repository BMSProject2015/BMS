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
using System.Text;
namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKB_TheoDoiDuToanController : Controller
    {
        //
        // GET: /rptKTKB_TheoDoiDuToan/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKB_TheoDoiDuToan.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_TheoDoiDuToan.aspx";
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
            String iID_MaNguonNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaNguonNganSach"]);
            String Loai = Convert.ToString(Request.Form[ParentID + "_Loai"]);
            String iThang_Quy = "";
            String bLoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_bLoaiThang_Quy"]);
            if (bLoaiThang_Quy == "0") iThang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            else iThang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iTapHop = Convert.ToString(Request.Form[ParentID + "_iTapHop"]);
            ViewData["iID_MaNguonNganSach"] = iID_MaNguonNganSach;
            ViewData["Loai"] = Loai;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["bLoaiThang_Quy"] = bLoaiThang_Quy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iTapHop"] = iTapHop;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKB_TheoDoiDuToan.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult ViewPDF(String MaND, String iID_MaNguonNganSach, String Loai, String iThang_Quy, String bLoaiThang_Quy, String iID_MaTrangThaiDuyet, String iTapHop)
        {
            HamChung.Language();
            String DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, iID_MaNguonNganSach, Loai, iThang_Quy, bLoaiThang_Quy, iID_MaTrangThaiDuyet, iTapHop);
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
        public clsExcelResult ExportToExcel(String MaND, String iID_MaNguonNganSach, String Loai, String iThang_Quy, String bLoaiThang_Quy, String iID_MaTrangThaiDuyet, String iTapHop)
        {
            HamChung.Language();
            String DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaND, iID_MaNguonNganSach, Loai, iThang_Quy, bLoaiThang_Quy, iID_MaTrangThaiDuyet, iTapHop);
            clsExcelResult clsResult = new clsExcelResult();
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "reportKTKB_TheoDoiDuToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String MaND, String iID_MaNguonNganSach, String Loai, String iThang_Quy, String bLoaiThang_Quy, String iID_MaTrangThaiDuyet, String iTapHop)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();


            String sL = "";
            String sK = "";
            String NamND = "";
            String LoaiThangQuy = "";
            if (bLoaiThang_Quy == "0") LoaiThangQuy = "Tháng";
            else LoaiThangQuy = "Quý";
            String[] arrLoai = Loai.Split('.');
            if (arrLoai.Length == 2)
            {
                sL = arrLoai[0];
                sK = arrLoai[1];
            }
            if (iID_MaNamNganSach == "1") NamND = Convert.ToString(Convert.ToInt16(iNamLamViec) - 1);
            if (iID_MaNamNganSach == "2") NamND = Convert.ToString(Convert.ToInt16(iNamLamViec));
            if (iID_MaNamNganSach == "3") NamND = Convert.ToString(Convert.ToInt16(iNamLamViec) + 1);
            //Ten loai
            String TenLoai = "";
            DataTable dtLoai = get_DanhSachLoai_Khoan(MaND, iID_MaNguonNganSach);
            for (int i = 0; i < dtLoai.Rows.Count; i++)
            {
                if (Loai == dtLoai.Rows[i]["Loai"].ToString())
                {
                    TenLoai = dtLoai.Rows[i]["sMoTa"].ToString();
                    break;
                }
            }
            dtLoai.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKB_BangKeRutDuToan");
            LoadData(fr, MaND, iID_MaNguonNganSach, Loai, iThang_Quy, bLoaiThang_Quy, iID_MaTrangThaiDuyet,iTapHop);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("NamND", NamND);
            fr.SetValue("sL", sL);
            fr.SetValue("sK", sK);
            fr.SetValue("Thang_Quy", iThang_Quy);
            fr.SetValue("LoaiThang_Quy", LoaiThangQuy);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("TenLoai", TenLoai);
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaNguonNganSach, String Loai, String iThang_Quy, String bLoaiThang_Quy, String iID_MaTrangThaiDuyet, String iTapHop)
        {
            DataTable data = dt_DuToan(MaND, iID_MaNguonNganSach, Loai, iThang_Quy, bLoaiThang_Quy, iID_MaTrangThaiDuyet, iTapHop);
            data.TableName = "DuToan";
            fr.AddTable("DuToan", data);
            data.Dispose();

            DataTable LuyKe = dt_LuyKe(MaND, iID_MaNguonNganSach, Loai, iThang_Quy, bLoaiThang_Quy, iID_MaTrangThaiDuyet);
            data.TableName = "LuyKe";
            fr.AddTable("LuyKe", LuyKe);
            LuyKe.Dispose();

        }
        public JsonResult Get_objLoai(String ParentID, String MaND, String iID_MaNguonNganSach, String Loai)
        {
            return Json(get_Loai(ParentID, MaND, iID_MaNguonNganSach, Loai), JsonRequestBehavior.AllowGet);
        }
        public String get_Loai(String ParentID, String MaND, String iID_MaNguonNganSach, String Loai)
        {
            String s;
            DataTable dtLoai = get_DanhSachLoai_Khoan(MaND, iID_MaNguonNganSach);
            SelectOptionList slLoai = new SelectOptionList(dtLoai, "Loai", "TenHT");
            s = MyHtmlHelper.DropDownList(ParentID, slLoai, Loai, "Loai", "", "style=\"width:100%\"");
            dtLoai.Dispose();
            return s;
        }
        public static DataTable get_DanhSachNguon(String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT CT.iID_MaNguonNganSach,sTen
                                        FROM
                                        (SELECT DISTINCT iID_MaNguonNganSach
                                        FROM KTKB_ChungTuChiTiet
                                        WHERE iTrangThai=1
                                         AND (rSoTien>0
                                         OR rDTRut>0)
                                         --AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                         AND iID_MaNamNganSach=@iID_MaNamNganSach
                                         AND iNamLamViec=@iNamLamViec
                                         ) as CT
                                         INNER JOIN(SELECT iID_MaNguonNganSach,sTen FROM NS_NguonNganSach) NS
                                         ON CT.iID_MaNguonNganSach=NS.iID_MaNguonNganSach");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách loại khoản
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaNguonNganSach"></param>
        /// <returns></returns>
        public static DataTable get_DanhSachLoai_Khoan(String MaND, String iID_MaNguonNganSach)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT CT.sL,CT.sK,sMoTa,CT.sL+'.'+CT.sK+' - - '+sMoTa as TenHT,CT.sL+'.'+CT.sK as Loai
                                        FROM
                                        (SELECT DISTINCT sL,SK
                                        FROM KTKB_ChungTuChiTiet
                                        WHERE iTrangThai=1
                                         AND (rSoTien>0
                                         OR rDTRut>0)
                                         --AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                                         AND iID_MaNguonNganSach=@iID_MaNguonNganSach
                                         AND iID_MaNamNganSach=@iID_MaNamNganSach
                                         AND iNamLamViec=@iNamLamViec
                                         ) as CT
                                         INNER JOIN(SELECT sL,sK,sMoTa FROM NS_MucLucNganSach WHERE sL<>'' AND sK<>'' AND sM='') NS
                                         ON CT.sL=NS.sL AND CT.sK=NS.sK");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Datatable
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaNguonNganSach"></param>
        /// <param name="Loai"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="bLoaiThang_Quy"></param>
        /// <returns></returns>
        public static DataTable dt_LuyKe(String MaND, String iID_MaNguonNganSach, String Loai, String iThang_Quy, String bLoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            String[] arrLoaiKhoan = Loai.Split('.');
            String sL = "-1", sK = "-1";
            if (arrLoaiKhoan.Length == 2)
            {
                sL = arrLoaiKhoan[0].ToString();
                sK = arrLoaiKhoan[1].ToString();
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();
            String dNgay = "";
            if (bLoaiThang_Quy == "0") dNgay = " =" + iThang_Quy;
            else
            {
                if (iThang_Quy == "1") dNgay = "IN(1,2,3)";
                else if (iThang_Quy == "2") dNgay = "IN(4,5,6)";
                else if (iThang_Quy == "3") dNgay = "IN(7,8,9)";
                else dNgay = "IN(10,11,12)";
            }
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            // tạo đợt rút dự toán
            String SQLDotRutdt = String.Format(@"SELECT DISTINCT dNgayDotRutDuToan,iID_MaDotRutDuToan
                                                FROM KT_RutDuToanChiTiet
                                                WHERE iTrangThai=1 AND sL=@sL AND sK=@sK
	                                                 AND iNamLamViec=@iNamLamViec 
	                                                 AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                                 AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                ORDER BY dNgayDotRutDuToan
                                                     ");
            SqlCommand cmdDotRutdt = new SqlCommand(SQLDotRutdt);
            cmdDotRutdt.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmdDotRutdt.Parameters.AddWithValue("@sL", sL);
            cmdDotRutdt.Parameters.AddWithValue("@sK", sK);
            cmdDotRutdt.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmdDotRutdt.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            DataTable dtDotRutdt = Connection.GetDataTable(cmdDotRutdt);
            cmdDotRutdt.Dispose();
            String iID_MaDotDauNam = Guid.Empty.ToString();
            if (dtDotRutdt.Rows.Count > 0)
                iID_MaDotDauNam = dtDotRutdt.Rows[0]["iID_MaDotRutDuToan"].ToString();

            if (bLoaiThang_Quy == "1") iThang_Quy = Convert.ToString(Convert.ToInt16(iThang_Quy) * 3);
            //Tạo SQL rút dự toán
            String SQL = String.Format(@"SELECT SUM(DT_DauNam) as DT_DauNam	
	                                   ,SUM(DT_BoSung) as DT_BoSung
	                                   ,SUM(DT_Rut) as DT_Rut
	                                   ,SUM(DT_KhoiPhuc) as DT_KhoiPhuc
	                                    FROM(
	                                    SELECT 
                                        DT_DauNam=CASE WHEN iID_MaDotRutDuToan=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_BoSung=CASE WHEN iID_MaDotRutDuToan!=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_Rut=0
                                        ,DT_KhoiPhuc=0
                                        FROM KT_RutDuToanChiTiet
                                        WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                        AND iTrangThai=1
                                            AND iNamLamViec=@iNamLamViec
	                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                            AND MONTH(dNgayDotRutDuToan) <=@iThang_Quy
                                            GROUP BY iID_MaDotRutDuToan
                                        UNION 
                                       SELECT 
                                              DT_DauNam=0,
                                              DT_BoSung=0
                                              ,DT_Rut= SUM(rDTRut)
                                              ,DT_KhoiPhuc= SUM(rDTKhoiPhuc)
                                       FROM KTKB_ChungTuChiTiet
                                       WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                                            AND iTrangThai=1
                                                                AND iNamLamViec=@iNamLamViec {0}
	                                                            AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                                            AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                                AND MONTH(iThang) <=@iThang_Quy) as a
                                        ", DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@sL", sL);
            cmd.Parameters.AddWithValue("@sK", sK);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaDotDauNam", iID_MaDotDauNam);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable dt_DuToan(String MaND, String iID_MaNguonNganSach, String Loai, String iThang_Quy, String bLoaiThang_Quy, String iID_MaTrangThaiDuyet, String iTapHop)
        {
            String[] arrLoaiKhoan = Loai.Split('.');
            String sL = "-1", sK = "-1";
            if (arrLoaiKhoan.Length == 2)
            {
                sL = arrLoaiKhoan[0].ToString();
                sK = arrLoaiKhoan[1].ToString();
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
            dtCauHinh.Dispose();
            String dNgay = "";
            if (bLoaiThang_Quy == "0") dNgay = " =" + iThang_Quy;
            else
            {
                if (iThang_Quy == "1") dNgay = "IN(1,2,3)";
                else if (iThang_Quy == "2") dNgay = "IN(4,5,6)";
                else if (iThang_Quy == "3") dNgay = "IN(7,8,9)";
                else dNgay = "IN(10,11,12)";
            }
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            // tạo đợt rút dự toán
            String SQLDotRutdt = String.Format(@"SELECT DISTINCT dNgayDotRutDuToan,iID_MaDotRutDuToan
                                                FROM KT_RutDuToanChiTiet
                                                WHERE iTrangThai=1 AND sL=@sL AND sK=@sK
	                                                 AND iNamLamViec=@iNamLamViec
	                                                 AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                                 AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                ORDER BY dNgayDotRutDuToan
                                                     ", DKTrangThaiDuyet);
            SqlCommand cmdDotRutdt = new SqlCommand(SQLDotRutdt);
            cmdDotRutdt.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmdDotRutdt.Parameters.AddWithValue("@sL", sL);
            cmdDotRutdt.Parameters.AddWithValue("@sK", sK);
            cmdDotRutdt.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmdDotRutdt.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);

            DataTable dtDotRutdt = Connection.GetDataTable(cmdDotRutdt);
            cmdDotRutdt.Dispose();
            String iID_MaDotDauNam = Guid.Empty.ToString();
            if (dtDotRutdt.Rows.Count > 0)
                iID_MaDotDauNam = dtDotRutdt.Rows[0]["iID_MaDotRutDuToan"].ToString();

            //Tạo SQL rút dự toán
            //Đên chi tiêt
            String SQL = "";
            if (iTapHop == "1")
            {
                 SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sSoChungTuGhiSo='',iNgay=DAY(dNgayDotRutDuToan),iThang=MONTH(dNgayDotRutDuToan),sNoiDung='',
                                        DT_DauNam=CASE WHEN iID_MaDotRutDuToan=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_BoSung=CASE WHEN iID_MaDotRutDuToan!=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_Rut=0
                                        ,DT_KhoiPhuc=0
                                        FROM KT_RutDuToanChiTiet
                                        WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                        AND iTrangThai=1
                                            AND iNamLamViec=@iNamLamViec
	                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                            AND MONTH(dNgayDotRutDuToan) {0} 
                                        GROUP BY sLNS,sL,sK,sM,sTM,iID_MaDotRutDuToan,dNgayDotRutDuToan
                                        HAVING SUM(rSoTien)>0
                                        UNION 
                                       SELECT sLNS,sL,sK,sM,sTM,sSoChungTuGhiSo,iNgay,iThang,sNoiDung,
                                              DT_DauNam=0,
                                              DT_BoSung=0
                                              ,DT_Rut= SUM(rDTRut)
                                              ,DT_KhoiPhuc= SUM(rDTKhoiPhuc)
                                       FROM KTKB_ChungTuChiTiet
                                       WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                                            AND iTrangThai=1
                                                                AND iNamLamViec=@iNamLamViec
	                                                            AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                                            AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                                AND iThang {0} {1}
                                       GROUP BY sLNS,sL,sK,sM,sTM,sSoChungTuGhiSo,iNgay,iThang,sNoiDung
                                       HAVING SUM(rDTRut)<>0 OR SUM(rDTKhoiPhuc)<>0
                                       ORDER BY iThang,iNgay,sSoChungTuGhiSo
                                        ", dNgay, DKTrangThaiDuyet);
            }
                //Den so chung tu
            else if (iTapHop == "2")
            {
                SQL = String.Format(@"SELECT sSoChungTuGhiSo='',sNoiDung='',iNgay='',iThang='',
                                        DT_DauNam=CASE WHEN iID_MaDotRutDuToan=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_BoSung=CASE WHEN iID_MaDotRutDuToan!=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_Rut=0
                                        ,DT_KhoiPhuc=0
                                        FROM KT_RutDuToanChiTiet
                                        WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                        AND iTrangThai=1
                                            AND iNamLamViec=@iNamLamViec
	                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                            AND MONTH(dNgayDotRutDuToan) {0} 
                                        GROUP BY iID_MaDotRutDuToan,dNgayDotRutDuToan
                                        HAVING SUM(rSoTien)>0
                                        UNION 
                                       SELECT sSoChungTuGhiSo,sNoiDung='',iNgay='',iThang='',
                                              DT_DauNam=0,
                                              DT_BoSung=0
                                              ,DT_Rut= SUM(rDTRut)
                                              ,DT_KhoiPhuc= SUM(rDTKhoiPhuc)
                                       FROM KTKB_ChungTuChiTiet
                                       WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                                            AND iTrangThai=1
                                                                AND iNamLamViec=@iNamLamViec
	                                                            AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                                            AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                                AND iThang {0} {1}
                                       GROUP BY sSoChungTuGhiSo
                                       HAVING SUM(rDTRut)<>0 OR SUM(rDTKhoiPhuc)<>0
                                       ORDER BY sSoChungTuGhiSo
                                        ", dNgay, DKTrangThaiDuyet);
            }
                //den ngay
            else
            {
                SQL = String.Format(@"SELECT iNgay=DAY(dNgayDotRutDuToan),iThang=MONTH(dNgayDotRutDuToan),sNoiDung='',sSoChungTuGhiSo='',
                                        DT_DauNam=CASE WHEN iID_MaDotRutDuToan=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_BoSung=CASE WHEN iID_MaDotRutDuToan!=@iID_MaDotDauNam THEN SUM(rSoTien) ELSE 0 END
                                        ,DT_Rut=0
                                        ,DT_KhoiPhuc=0
                                        FROM KT_RutDuToanChiTiet
                                        WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                        AND iTrangThai=1
                                            AND iNamLamViec=@iNamLamViec
	                                        AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                        AND iID_MaNamNganSach=@iID_MaNamNganSach
                                            AND MONTH(dNgayDotRutDuToan) {0} 
                                        GROUP BY iID_MaDotRutDuToan,dNgayDotRutDuToan
                                        HAVING SUM(rSoTien)>0
                                        UNION 
                                       SELECT iNgay,iThang,sNoiDung='',sSoChungTuGhiSo='',
                                              DT_DauNam=0,
                                              DT_BoSung=0
                                              ,DT_Rut= SUM(rDTRut)
                                              ,DT_KhoiPhuc= SUM(rDTKhoiPhuc)
                                       FROM KTKB_ChungTuChiTiet
                                       WHERE sTM<>'' AND sL=@sL AND sK=@sK
	                                                            AND iTrangThai=1
                                                                AND iNamLamViec=@iNamLamViec
	                                                            AND iID_MaNguonNganSach=@iID_MaNguonNganSach
	                                                            AND iID_MaNamNganSach=@iID_MaNamNganSach
                                                                AND iThang {0} {1}
                                       GROUP BY iNgay,iThang
                                       HAVING SUM(rDTRut)<>0 OR SUM(rDTKhoiPhuc)<>0
                                       ORDER BY iThang,iNgay,sSoChungTuGhiSo
                                        ", dNgay, DKTrangThaiDuyet);
            }
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@sL", sL);
            cmd.Parameters.AddWithValue("@sK", sK);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaDotDauNam", iID_MaDotDauNam);
            if (iID_MaTrangThaiDuyet != "1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

    }
}
