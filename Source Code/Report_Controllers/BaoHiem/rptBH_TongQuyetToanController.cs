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
using System.Collections;

namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_TongQuyetToanController : Controller
    {
        //
        // GET: /rptBH_TongQuyetToan/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay = "", String BaoHiem = "")
        {
            
            String sFilePath = "";
            if (KhoGiay == "0")
            {
                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCN.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDV.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }

            else
            {

                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1A3.xls";
                }
                else if (BaoHiem == "3"  )
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCNA3.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDVA3.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_TongQuyetToan.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID)
        {

            String ThangQuy = "";
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "1")
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaNhomDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomDonVi"]);
            String LuyKe = Convert.ToString(Request.Form[ParentID + "_LuyKe"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String BaoHiem = Convert.ToString(Request.Form[ParentID + "_BaoHiem"]);
            String iLoai = Convert.ToString(Request.Form[ParentID + "_iLoai"]);
            ViewData["ThangQuy"] = ThangQuy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaNhomDonVi"] = iID_MaNhomDonVi;
            ViewData["LuyKe"] = LuyKe;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["BaoHiem"] = BaoHiem;
            ViewData["iLoai"] = iLoai;
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_TongQuyetToan.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public ExcelFile CreateReport(String path, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem,String iLoai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String LoaiThangQuy = "";
            switch (LoaiThang_Quy)
            {
                case "0":
                    LoaiThangQuy = "Tháng";
                    break;
                case "1":
                    LoaiThangQuy = "Quý";
                    break;
                case "2":
                    LoaiThangQuy = "Năm";
                    break;
            }
            
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_TongQuyetToan");
            LoadData(fr, MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem,iLoai);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Ngay", NgayThang);
            if (LoaiThang_Quy == "0")
            {
                fr.SetValue("Thang", "Tháng");
            }
            else if (LoaiThang_Quy == "1")
            {
                fr.SetValue("Thang", "Quý");
            }
            else
            {
                fr.SetValue("Thang", " ");
            }
            if (LoaiThang_Quy == "0")
            {
                fr.SetValue("ThangQuy", ThangQuy);
            }
            else if (LoaiThang_Quy == "1")
            {
                fr.SetValue("ThangQuy", ThangQuy);
            }
            else
            {
                fr.SetValue("ThangQuy", "");
            }
           
            fr.SetValue("LoaiBaoHiem", BaoHiem);
            fr.SetValue("LoaiThangQuy", LoaiThangQuy);
            if (BaoHiem == "0")
            {
                fr.SetValue("LoaiBH", "BÁO CÁO THU NỘP BẢO HIỂM XÃ HỘI");
            }
            else if (BaoHiem == "1")
            {
                fr.SetValue("LoaiBH", "BÁO CÁO THU NỘP BẢO HIỂM Y TẾ");
            }
            else if (BaoHiem == "2")
            {
                fr.SetValue("LoaiBH", "BÁO CÁO THU NỘP BẢO HIỂM THẤT NGHIỆP");
            }
           
            
            fr.Run(Result);
            return Result;

        }
        private SortedDictionary<string, Hashtable> LayDuLieu_HasTable(DataTable dt)
        {
            SortedDictionary<string, Hashtable> DuLieu = XuLyDuLieu_HasTable(dt);
            Hashtable KhoaCha = new Hashtable();
            ArrayList KeyList = new ArrayList();
            foreach (String key in DuLieu.Keys)
            {
                KeyList.Add(key);
            }
            for (int i = DuLieu.Keys.Count - 1; i > 0; i--)
            {

                String key = Convert.ToString(KeyList[i]);
                String parent_key = "";
                Hashtable HangCon = (Hashtable)DuLieu[key];
                if (key.Length > 1) parent_key = key.Substring(0, key.Length - 1);
                if (DuLieu.ContainsKey(parent_key))
                {
                    Hashtable HangCha = (Hashtable)DuLieu[parent_key];
                    if (KhoaCha[parent_key] == null)
                    {
                        HangCha["rBHXH_CN"] = 0;
                        HangCha["rBHXH_DV"] = 0;
                        HangCha["rBHYT_CN"] = 0;
                        HangCha["rBHTN_CN"] = 0;
                        HangCha["rBHYT_DV"] = 0;
                        HangCha["rBHTN_DV"] = 0;
                        HangCha["rSoNguoi"] = 0;
                        KhoaCha.Add(parent_key, true);
                    }
                    HangCha["rBHXH_CN"] = Convert.ToDecimal(HangCha["rBHXH_CN"]) + Convert.ToDecimal(HangCon["rBHXH_CN"]);
                    HangCha["rBHXH_DV"] = Convert.ToDecimal(HangCha["rBHXH_DV"]) + Convert.ToDecimal(HangCon["rBHXH_DV"]);
                    HangCha["rBHYT_CN"] = Convert.ToDecimal(HangCha["rBHYT_CN"]) + Convert.ToDecimal(HangCon["rBHYT_CN"]);
                    HangCha["rBHTN_CN"] = Convert.ToDecimal(HangCha["rBHTN_CN"]) + Convert.ToDecimal(HangCon["rBHTN_CN"]);
                    HangCha["rBHYT_DV"] = Convert.ToDecimal(HangCha["rBHYT_DV"]) + Convert.ToDecimal(HangCon["rBHYT_DV"]);
                    HangCha["rBHTN_DV"] = Convert.ToDecimal(HangCha["rBHTN_DV"]) + Convert.ToDecimal(HangCon["rBHTN_DV"]);
                    HangCha["rSoNguoi"] = Convert.ToDecimal(HangCha["rSoNguoi"]) + Convert.ToDecimal(HangCon["rSoNguoi"]);
                    DuLieu[parent_key] = HangCha;
                }
            }
            return DuLieu;
        }
        private SortedDictionary<string, Hashtable> XuLyDuLieu_HasTable(DataTable dt)
        {
            //DataView dv = new DataView(dt);
            //dv.Sort = "sKyHieu_" + ThuChi + " DESC"; 
            //dt = dv.ToTable();
            SortedDictionary<string, Hashtable> DuLieu = new SortedDictionary<string, Hashtable>();
            foreach (DataRow row in dt.Rows)
            {
                String sKyHieu = Convert.ToString(row["sKyHieu"]);
                Decimal rBHXH_CN = 0, rBHXH_DV= 0, rBHYT_CN = 0, rBHTN_CN = 0, rBHYT_DV = 0, rBHTN_DV = 0, rSoNguoi = 0;
                try { rBHXH_CN = Convert.ToDecimal(row["rBHXH_CN"]); }
                catch { rBHXH_CN = 0; }
                try { rBHXH_DV= Convert.ToDecimal(row["rBHXH_DV"]); }
                catch { rBHXH_DV= 0; }
                try { rBHYT_CN = Convert.ToDecimal(row["rBHYT_CN"]); }
                catch { rBHYT_CN = 0; }
                try { rBHTN_CN = Convert.ToDecimal(row["rBHTN_CN"]); }
                catch { rBHTN_CN = 0; }
                try { rBHYT_DV = Convert.ToDecimal(row["rBHYT_DV"]); }
                catch { rBHYT_DV = 0; }
                try { rBHTN_DV = Convert.ToDecimal(row["rBHTN_DV"]); }
                catch { rBHTN_DV = 0; }
                try { rSoNguoi = Convert.ToDecimal(row["rSoNguoi"]); }
                catch { rSoNguoi = 0; }
                if (!DuLieu.ContainsKey(sKyHieu))
                {
                    Hashtable ChiTiet = new Hashtable();
                    ChiTiet.Add("rBHXH_CN", rBHXH_CN);
                    ChiTiet.Add("rBHXH_DV", rBHXH_DV);
                    ChiTiet.Add("rBHYT_CN", rBHYT_CN);
                    ChiTiet.Add("rBHTN_CN", rBHTN_CN);
                    ChiTiet.Add("rBHYT_DV", rBHYT_DV);
                    ChiTiet.Add("rBHTN_DV", rBHTN_DV);
                    ChiTiet.Add("rSoNguoi", rSoNguoi);
                    DuLieu.Add(sKyHieu, ChiTiet);
                }
                else
                {
                    Hashtable ChiTiet = (Hashtable)DuLieu[sKyHieu];
                    ChiTiet["rBHXH_CN"] = Convert.ToDecimal(ChiTiet["rBHXH_CN"]) + rBHXH_CN;
                    ChiTiet["rBHXH_DV"] = Convert.ToDecimal(ChiTiet["rBHXH_DV"]) + rBHXH_DV;
                    ChiTiet["rBHYT_CN"] = Convert.ToDecimal(ChiTiet["rBHYT_CN"]) + rBHYT_CN;
                    ChiTiet["rBHTN_CN"] = Convert.ToDecimal(ChiTiet["rBHTN_CN"]) + rBHTN_CN;
                    ChiTiet["rBHYT_DV"] = Convert.ToDecimal(ChiTiet["rBHYT_DV"]) + rBHYT_DV;
                    ChiTiet["rBHTN_DV"] = Convert.ToDecimal(ChiTiet["rBHTN_DV"]) + rBHTN_DV;
                    ChiTiet["rSoNguoi"] = Convert.ToDecimal(ChiTiet["rSoNguoi"]) + rSoNguoi;
                    DuLieu[sKyHieu] = ChiTiet;
                }
            }
            return DuLieu;
        }
        public DataTable rptBH_TongQuyetToan(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem, String iLoai)
        {
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                iID_MaTrangThaiDuyet = "AND TB.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
                DKDuyet = "";
            }
            String DKThang = "";
            String DKLKe = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "AND TB.iThang_Quy between 1 and 3";
                        DKLKe = "AND TB.iThang_Quy <= 3";
                        break;
                    case "2": DKThang = "AND TB.iThang_Quy between 4 and 6";
                        DKLKe = "AND TB.iThang_Quy <= 6";
                        break;
                    case "3": DKThang = "AND TB.iThang_Quy between 7 and 9";
                        DKLKe = "AND TB.iThang_Quy <= 9";
                        break;
                    case "4": DKThang = "AND TB.iThang_Quy between 10 and 12";
                        DKLKe = "AND iThang_Quy <= 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "AND TB.iThang_Quy=@ThangQuy";
                DKLKe = "AND TB.iThang_Quy <=@ThangQuy ";
            }
            else
            {
                DKThang = "AND TB.iThang_Quy between 1 and 12";
                DKLKe = "AND TB.iThang_Quy between 1 and 12 ";
            }
            String DKDonvi = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            if (iLoai == "0")
            {
                DKDonvi = " AND DV.iID_MaDonVi =@iID_MaDonVi";
            }
            else if (iLoai == "1")
            {
                DKDonvi = "";
            }
            else
            {
                DKDonvi = " AND DV.iID_MaNhomDonVi = @iID_MaNhomDonVi";
            }
            DataTable dt = new DataTable();
          
                if (LuyKe == "on")
                {

                    String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                    inner join (select * from NS_DonVi where iNamLamViec_DonVi={5} ) as DV
                                    on DV.iID_MaDonVi= TB.iID_MaDonVi
                                    WHERE TB.iTrangThai = 1  {2} {0} {1} {3}
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu ", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKLKe, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    if (iLoai == "0" || iLoai == "2")
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    }
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
                else
                {
                    String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                    inner join (select * from NS_DonVi where iNamLamViec_DonVi={5} ) as DV
                                    on DV.iID_MaDonVi= TB.iID_MaDonVi
                                    WHERE TB.iTrangThai = 1  {2} {0} {1} {3}
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKThang, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();                      
                }
              
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }
        private void LoadData(FlexCelReport fr, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem,String iLoai)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            
                if (BaoHiem == "0" )
                {
                    DataTable data = rptBH_TongQuyetToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem,iLoai);
                    SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                    foreach (DataRow row in data.Rows)
                    {
                         if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                            row["rBHXH_CN"] = hs["rBHXH_CN"];
                            row["rBHXH_DV"] = hs["rBHXH_DV"];
                            row["rSoNguoi"] = hs["rSoNguoi"];
                            
                        }
                        data.TableName = "ChiTiet";
                        fr.AddTable("ChiTiet", data);
                        data.Dispose();
                    }
                }
                else if (BaoHiem == "1")
                {
                    DataTable data = rptBH_TongQuyetToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                    foreach (DataRow row in data.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                            row["rBHYT_CN"] = hs["rBHYT_CN"];
                            row["rBHYT_DV"] = hs["rBHYT_DV"];
                            row["rSoNguoi"] = hs["rSoNguoi"];

                        }
                        data.TableName = "ChiTiet";
                        fr.AddTable("ChiTiet", data);
                        data.Dispose();
                    }

                }
                else if (BaoHiem == "2")
                {
                    DataTable data = rptBH_TongQuyetToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                    foreach (DataRow row in data.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                            row["rBHTN_CN"] = hs["rBHTN_CN"];
                            row["rBHTN_DV"] = hs["rBHTN_DV"];
                            row["rSoNguoi"] = hs["rSoNguoi"];

                        }
                        data.TableName = "ChiTiet";
                        fr.AddTable("ChiTiet", data);
                        data.Dispose();
                    }
                }
                else if (BaoHiem == "3")
                {
                    DataTable data = rptBH_TongQuyetToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                    foreach (DataRow row in data.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                            row["rBHXH_CN"] = hs["rBHXH_CN"];
                            row["rBHYT_CN"] = hs["rBHYT_CN"];
                            row["rBHTN_CN"] = hs["rBHTN_CN"];
                            row["rSoNguoi"] = hs["rSoNguoi"];
                        }
                        data.TableName = "ChiTiet";
                        fr.AddTable("ChiTiet", data);
                        data.Dispose();
                    }
                }
                else if (BaoHiem == "4")
                {
                    DataTable data = rptBH_TongQuyetToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                    foreach (DataRow row in data.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                            row["rBHXH_DV"] = hs["rBHXH_DV"];
                            row["rBHYT_DV"] = hs["rBHYT_DV"];
                            row["rBHTN_DV"] = hs["rBHTN_DV"];
                            row["rSoNguoi"] = hs["rSoNguoi"];
                        }
                        data.TableName = "ChiTiet";
                        fr.AddTable("ChiTiet", data);
                        data.Dispose();
                    }
                }
                else
                {
                    DataTable data = rptBH_TongQuyetToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                    foreach (DataRow row in data.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                            row["rBHXH_CN"] = hs["rBHXH_CN"];
                            row["rBHXH_DV"] = hs["rBHXH_DV"];
                            row["rBHYT_CN"] = hs["rBHYT_CN"];
                            row["rBHTN_CN"] = hs["rBHTN_CN"];
                            row["rBHYT_DV"] = hs["rBHYT_DV"];
                            row["rBHTN_DV"] = hs["rBHTN_DV"];
                            row["rSoNguoi"] = hs["rSoNguoi"];
                        }
                        data.TableName = "ChiTiet";
                        fr.AddTable("ChiTiet", data);
                        data.Dispose();
                    }
                }
                DataTable data1 = DoanhNghiep(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                
                    data1.TableName = "ChiTiet1";
                    fr.AddTable("ChiTiet1", data1);
                    data1.Dispose();
                    DataTable data2 = TongCongCNDV(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    data2.TableName = "ChiTiet2";
                    fr.AddTable("ChiTiet2", data2);
                    data2.Dispose();
                    DataTable data3 = DaNop(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    data3.TableName = "DaNop";
                    fr.AddTable("DaNop", data3);
                    data3.Dispose();
                    DataTable data4 = DonViDuToan(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem, iLoai);
                    data4.TableName = "ChiTiet3";
                    fr.AddTable("ChiTiet3", data4);
                    data4.Dispose();
                
            
            
        }

        public clsExcelResult ExportToPDF(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem,String iLoai)
        {
            clsExcelResult clsResult = new clsExcelResult();
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "0")
            {
                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCN.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDV.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }

            else
            {

                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1A3.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCNA3.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDVA3.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem,iLoai);
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

        public clsExcelResult ExportToExcel(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem,String iLoai)
        {
            clsExcelResult clsResult = new clsExcelResult();
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "0")
            {
                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCN.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDV.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }

            else
            {

                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1A3.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCNA3.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDVA3.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem,iLoai);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                //if (KhoGiay == "1")
                //{
                //    clsResult.FileName = "baocaoquyettoanChiCacCheDoBHXH.xls";
                //}
                //else
                //{
                //    clsResult.FileName = "baocaoquyettoanChiCacCheDoBHXH_A4.xls";
                //}
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem,String iLoai)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "0")
            {
                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCN.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDV.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }

            else
            {

                if (BaoHiem == "0" || BaoHiem == "1" || BaoHiem == "2")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToan1A3.xls";
                }
                else if (BaoHiem == "3")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanCNA3.xls";
                }
                else if (BaoHiem == "4")
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanDVA3.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_TongQuyetToanTH.xls";
                }
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem,iLoai);
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

        public class Data
        {
            public String DV { get; set; }
            public String NDV { get; set; }
        }
        public JsonResult ds_DonVi(String ParentID, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonViTheoLNS(ParentID, MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public Data obj_DonViTheoLNS(String ParentID, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String iID_MaTrangThaiDuyet)
        {
            Data _data = new Data();

            DataTable dtDonvi = HienThiDonViTheoNam(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            _data.DV = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 120px;\"");

            DataTable dtNhomDV = DS_NhomDonVi(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
            SelectOptionList sliID_MaNhomDonVi = new SelectOptionList(dtNhomDV, "iID_MaNhomDonVi", "TenNhom");
            _data.NDV = MyHtmlHelper.DropDownList(ParentID, sliID_MaNhomDonVi, iID_MaNhomDonVi, "iID_MaNhomDonVi", "", "class=\"input1_2\" style=\"width: 120px; padding:2px;\"");
            return _data;

        }

        public static DataTable HienThiDonViTheoNam(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
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
            String DKThang = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "BH.iThang_Quy between 1 and 3";
                        break;
                    case "2": DKThang = "BH.iThang_Quy between 4 and 6";
                        break;
                    case "3": DKThang = "BH.iThang_Quy between 7 and 9";
                        break;
                    case "4": DKThang = "BH.iThang_Quy between 10 and 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "BH.iThang_Quy=@ThangQuy";
            }
            else
            {
                DKThang = "BH.iThang_Quy between 1 and 12";
            }
            String SQL = string.Format(@"SELECT DISTINCT 
                                        BH.iID_MaDonVi,DV.sTen
                                        FROM BH_PhaiThuChungTuChiTiet as BH
                                        inner join (Select * from NS_DonVi where iNamLamViec_DonVi={3}) as DV on BH.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE BH.iTrangThai=1  {1} 
                                         {0}  AND {2}
                                        ORDER BY iID_MaDonVi", iID_MaTrangThaiDuyet, DieuKien_NganSach1(MaND), DKThang,NguoiDungCauHinhModels.iNamLamViec);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
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

        public static DataTable DS_NhomDonVi(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            SqlCommand cmd = new SqlCommand();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND BH.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKThang = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "BH.iThang_Quy between 1 and 3";
                        break;
                    case "2": DKThang = "BH.iThang_Quy between 4 and 6";
                        break;
                    case "3": DKThang = "BH.iThang_Quy between 7 and 9";
                        break;
                    case "4": DKThang = "BH.iThang_Quy between 10 and 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "BH.iThang_Quy=@ThangQuy";
            }
            else
            {
                DKThang = "BH.iThang_Quy between 1 and 12";
            }
            DataTable dtNhomDV = new DataTable();

            String SQL = string.Format(@"SELECT iID_MaNhomDonVi,DC.sGhiChu as TenNhom
                                            FROM BH_PhaiThuChungTuChiTiet as BH
                                            INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={3}) as DV on DV.iID_MaDonVi = BH.iID_MaDonVi
                                            INNER JOIN DC_DanhMuc DC ON DV.iID_MaNhomDonVi=DC.iID_MaDanhMuc
                                            WHERE  {2} {1} {0}
                                            GROUP BY DC.sGhiChu,iID_MaNhomDonVi
                                            ", iID_MaTrangThaiDuyet, DieuKien_NganSach1(MaND), DKThang,NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
            dtNhomDV = Connection.GetDataTable(cmd);
            if (dtNhomDV.Rows.Count == 0)
            {
                DataRow R = dtNhomDV.NewRow();
                R["iID_MaNhomDonVi"] = Guid.Empty.ToString();
                R["TenNhom"] = "Không có nhóm đơn vị";
                dtNhomDV.Rows.InsertAt(R, 0);
            }
            return dtNhomDV;
        }
        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(" AND (TB.iNamLamViec = {0} AND TB.iID_MaNamNganSach={1} AND TB.iID_MaNguonNganSach={2})", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
        }
        public static String DieuKien_NganSach1(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(" AND (iNamLamViec = {0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2})", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
        }
        public DataTable DonViDuToan(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem, String iLoai)
        {
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                iID_MaTrangThaiDuyet = "AND TB.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
                DKDuyet = "";
            }
            String DKThang = "";
            String DKLKe = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "AND TB.iThang_Quy between 1 and 3";
                        DKLKe = "AND TB.iThang_Quy <= 3";
                        break;
                    case "2": DKThang = "AND TB.iThang_Quy between 4 and 6";
                        DKLKe = "AND TB.iThang_Quy <= 6";
                        break;
                    case "3": DKThang = "AND TB.iThang_Quy between 7 and 9";
                        DKLKe = "AND TB.iThang_Quy <= 9";
                        break;
                    case "4": DKThang = "AND TB.iThang_Quy between 10 and 12";
                        DKLKe = "AND TB.iThang_Quy <= 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "AND TB.iThang_Quy=@ThangQuy";
                DKLKe = "AND TB.iThang_Quy <=@ThangQuy ";
            }
            else
            {
                DKThang = "AND TB.iThang_Quy between 1 and 12";
                DKLKe = "AND TB.iThang_Quy between 1 and 12 ";
            }
            String DKDonvi = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            if (iLoai == "0")
            {
                DKDonvi = " AND DV.iID_MaDonVi =@iID_MaDonVi";
            }
            else if (iLoai == "1")
            {
                DKDonvi = "";
            }
            else
            {
                DKDonvi = " AND DV.iID_MaNhomDonVi = @iID_MaNhomDonVi";
            }
            DataTable dt = new DataTable();

            if (LuyKe == "on")
            {

                String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                    INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={5}) as DV on DV.iID_MaDonVi = TB.iID_MaDonVi
                                    WHERE TB.bLaHangCha = 0 AND subString (TB.sKyHieu,1,2)=11 AND TB.iTrangThai = 1  {2} {0} {1} {3}
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu ", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKLKe, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                if (iLoai == "0" || iLoai == "2")
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            else
            {
                String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                    INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={5}) as DV on DV.iID_MaDonVi = TB.iID_MaDonVi
                                    WHERE TB.bLaHangCha = 0 AND subString (TB.sKyHieu,1,2)=11 AND TB.iTrangThai = 1  {2} {0} {1} {3}
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKThang, DKDonvi, DKDuyet, NguoiDungCauHinhModels.iNamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }
        public DataTable DoanhNghiep(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem, String iLoai)
        {
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                iID_MaTrangThaiDuyet = "AND TB.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
                DKDuyet = "";
            }
            String DKThang = "";
            String DKLKe = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "AND  TB.iThang_Quy between 1 and 3";
                        DKLKe = "AND TB.iThang_Quy <= 3";
                        break;
                    case "2": DKThang = "AND TB.iThang_Quy between 4 and 6";
                        DKLKe = "AND TB.iThang_Quy <= 6";
                        break;
                    case "3": DKThang = "AND TB.iThang_Quy between 7 and 9";
                        DKLKe = "AND TB.iThang_Quy <= 9";
                        break;
                    case "4": DKThang = "AND TB.iThang_Quy between 10 and 12";
                        DKLKe = "AND TB.iThang_Quy <= 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "AND TB.iThang_Quy=@ThangQuy";
                DKLKe = "AND TB.iThang_Quy <=@ThangQuy ";
            }
            else
            {
                DKThang = "AND TB.iThang_Quy between 1 and 12";
                DKLKe = "AND TB.iThang_Quy between 1 and 12 ";
            }
            String DKDonvi = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            if (iLoai == "0")
            {
                DKDonvi = " AND DV.iID_MaDonVi =@iID_MaDonVi";
            }
            else if (iLoai == "1")
            {
                DKDonvi = "";
            }
            else
            {
                DKDonvi = " AND DV.iID_MaNhomDonVi = @iID_MaNhomDonVi";
            }
            DataTable dt = new DataTable();

            if (LuyKe == "on")
            {

                String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                     INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={5}) as DV on DV.iID_MaDonVi = TB.iID_MaDonVi
                                    WHERE TB.bLaHangCha = 0 AND subString (TB.sKyHieu,1,2)=12 AND TB.iTrangThai = 1  {2} {0} {1} {3}
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu ", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKLKe, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                if (iLoai == "0" || iLoai == "2")
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            else
            {
                String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                     INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={5}) as DV on DV.iID_MaDonVi = TB.iID_MaDonVi
                                    WHERE TB.bLaHangCha = 0 AND subString (TB.sKyHieu,1,2)=12 AND TB.iTrangThai = 1  {2} {0} {1} {3}
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKThang, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }
        public DataTable DaNop(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem, String iLoai)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }
            String DKThang = "";
            String DKLKe = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "iThang_Quy between 1 and 3";
                        DKLKe = "iThang_Quy <= 3";
                        break;
                    case "2": DKThang = "iThang_Quy between 4 and 6";
                        DKLKe = "iThang_Quy <= 6";
                        break;
                    case "3": DKThang = "iThang_Quy between 7 and 9";
                        DKLKe = "iThang_Quy <= 9";
                        break;
                    case "4": DKThang = "iThang_Quy between 10 and 12";
                        DKLKe = "iThang_Quy <= 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "iThang_Quy=@ThangQuy";
                DKLKe = "iThang_Quy <=@ThangQuy ";
            }
            else
            {
                DKThang = "iThang_Quy<=12";
                DKLKe = "iThang_Quy <=12 ";
            }
            String DKDonvi = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            if (iLoai == "0")
            {
                DKDonvi = " AND iID_MaDonVi =@iID_MaDonVi";
            }
            else if (iLoai == "1")
            {
                DKDonvi = "";
            }
            else
            {
                DKDonvi = " ";
            }            
          
            DataTable dt = new DataTable();
            if (BaoHiem == "0" )
            {
                if (LuyKe == "on")
                {

                    String SQL = String.Format(@"   SELECT DNDT=SUM(rKhoiDT)
                                                 , DNDN= sum(rKhoiDN) 
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 ANd iLoaiBaoHiem = 1 {1} {0} {3} AND {2} ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKLKe, DKDonvi);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    if (iLoai == "0" || iLoai == "2")
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    }
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
                else
                {
                    String SQL = String.Format(@" SELECT 
                                                 DNDT= sum(rKhoiDT)
                                                 , DNDN= sum(rKhoiDN)  
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 AND iLoaiBaoHiem = 1 {1} {0} {3} AND {2} ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThang, DKDonvi);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
            }
            else if (BaoHiem == "1")
            {
                if (LuyKe == "on")
                {

                    String SQL = String.Format(@"  SELECT DNDT= sum(rKhoiDT)
                                                 , DNDN= sum(rKhoiDN) 
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 AND  iLoaiBaoHiem = 2 {1} {0} {3} AND {2} ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKLKe, DKDonvi);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    if (iLoai == "0" || iLoai == "2")
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    }
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
                else
                {
                    String SQL = String.Format(@" SELECT DNDT= sum(rKhoiDT)
                                                 , DNDN=  sum(rKhoiDN)
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 AND iLoaiBaoHiem = 2 {1} {0} {3} AND {2} ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThang, DKDonvi);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
            }
            else if (BaoHiem == "2")
            {
                if (LuyKe == "on")
                {

                    String SQL = String.Format(@"  SELECT DNDT= sum(rKhoiDT)
                                                 , DNDN= sum(rKhoiDN) 
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 AND  iLoaiBaoHiem = 3 {1} {0} {3} AND {2} ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKLKe, DKDonvi);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    if (iLoai == "0" || iLoai == "2")
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    }
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
                else
                {
                    String SQL = String.Format(@" SELECT DNDT= sum(rKhoiDT)
                                                 , DNDN= sum(rKhoiDN)  
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 AND iLoaiBaoHiem = 3 {1} {0} {3} AND {2} ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThang, DKDonvi);
                    SqlCommand cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                    if (LoaiThang_Quy == "0")
                    {
                        cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                    }
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
            }
            else if (BaoHiem == "5")
                {
                    if (LuyKe == "on")
                    {

                        String SQL = String.Format(@"   SELECT DVDT_BHXH=case when iLoaiBaoHiem = 1 then sum(rKhoiDT)else 0 end
                                                 , DN_BHXH= case when iLoaiBaoHiem = 1 then sum(rKhoiDN)  else 0 end
                                                 ,DVDT_BHYT=case when iLoaiBaoHiem = 2 then sum(rKhoiDT)else 0 end
                                                 , DN_BHYT= case when iLoaiBaoHiem = 2 then sum(rKhoiDN)  else 0 end
                                                 ,DVDT_BHTN=case when iLoaiBaoHiem = 3 then sum(rKhoiDT)else 0 end
                                                 , DN_BHTN= case when iLoaiBaoHiem = 3 then sum(rKhoiDN)  else 0 end
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 {1} {0} {3} AND {2} group by iLoaiBaoHiem ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKLKe, DKDonvi);
                        SqlCommand cmd = new SqlCommand(SQL);
                        cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                        if (iLoai == "0" || iLoai == "2")
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        }
                        cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                        if (LoaiThang_Quy == "0")
                        {
                            cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                        }
                        dt = Connection.GetDataTable(cmd);
                        cmd.Dispose();
                    }
                    else
                    {
                        String SQL = String.Format(@" SELECT DVDT_BHXH=case when iLoaiBaoHiem = 1 then sum(rKhoiDT)else 0 end
                                                 , DN_BHXH= case when iLoaiBaoHiem = 1 then sum(rKhoiDN)  else 0 end
                                                 ,DVDT_BHYT=case when iLoaiBaoHiem = 2 then sum(rKhoiDT)else 0 end
                                                 , DN_BHYT= case when iLoaiBaoHiem = 2 then sum(rKhoiDN)  else 0 end
                                                 ,DVDT_BHTN=case when iLoaiBaoHiem = 3 then sum(rKhoiDT)else 0 end
                                                 , DN_BHTN= case when iLoaiBaoHiem = 3 then sum(rKhoiDN)  else 0 end
                                                FROM BH_ChungTuThuChiTiet
                                                WHERE iTrangThai = 1 {1} {0} {3} AND {2} group by iLoaiBaoHiem ", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThang, DKDonvi);
                        SqlCommand cmd = new SqlCommand(SQL);
                        cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                        if (LoaiThang_Quy == "0")
                        {
                            cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                        }
                        dt = Connection.GetDataTable(cmd);
                        cmd.Dispose();
                    }
            }
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }

        public DataTable TongCongCNDV(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaNhomDonVi, String LuyKe, String iID_MaTrangThaiDuyet, String KhoGiay, String BaoHiem, String iLoai)
        {
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                iID_MaTrangThaiDuyet = "AND TB.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
                DKDuyet = "";
            }
            String DKThang = "";
            String DKLKe = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "AND TB.iThang_Quy between 1 and 3";
                        DKLKe = "AND TB.iThang_Quy <= 3";
                        break;
                    case "2": DKThang = "AND TB.iThang_Quy between 4 and 6";
                        DKLKe = "AND TB.iThang_Quy <= 6";
                        break;
                    case "3": DKThang = "AND TB.iThang_Quy between 7 and 9";
                        DKLKe = "AND TB.iThang_Quy <= 9";
                        break;
                    case "4": DKThang = "AND TB.iThang_Quy between 10 and 12";
                        DKLKe = "AND TB.iThang_Quy <= 12";
                        break;
                }
            }
            else if (LoaiThang_Quy == "0")
            {
                DKThang = "AND TB.iThang_Quy=@ThangQuy";
                DKLKe = "AND TB.iThang_Quy <=@ThangQuy ";
            }
            else
            {
                DKThang = "AND TB.iThang_Quy between 1 and 12";
                DKLKe = "AND TB.iThang_Quy between 1 and 12 ";
            }
            String DKDonvi = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            if (iLoai == "0")
            {
                DKDonvi = " AND DV.iID_MaDonVi =@iID_MaDonVi";
            }
            else if (iLoai == "1")
            {
                DKDonvi = "";
            }
            else
            {
                DKDonvi = " AND DV.iID_MaNhomDonVi = @iID_MaNhomDonVi";
            }
            DataTable dt = new DataTable();

            if (LuyKe == "on")
            {

                String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                     INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={5}) as DV on DV.iID_MaDonVi = TB.iID_MaDonVi
                                    WHERE TB.bLaHangCha = 0 AND TB.iTrangThai = 1  {2} {0} {1} {3} 
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu ", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKLKe, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                if (iLoai == "0" || iLoai == "2")
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            else
            {
                String SQL = String.Format(@" SELECT TB.bLaHangCha,TB.sKyHieu,TB.sMoTa,sum(rSoNguoi) rSoNguoi,SUM(rLuongCoBan) rLuongCoBan,SUM(rThamNien) rThamNien
                                    ,SUM(rPCChucVu) rPCChucVu,SUM(rOmDauNgan) rOmDauNgan ,SUM(rKhac) rKhac,SUM(rTongSo) rTongSo
                                    ,SUM(rBHXH_CN)rBHXH_CN,SUM(rBHXH_DV)rBHXH_DV,SUM(rBHYT_CN)rBHYT_CN,SUM(rBHYT_DV)rBHYT_DV
                                    ,SUM(rBHTN_CN) rBHTN_CN,SUM(rBHTN_DV) rBHTN_DV
                                    FROM
                                    (
                                    SELECT bLaHangCha,iNamLamViec,sKyHieu,sMoTa,rLuongCoBan,rThamNien,rPCChucVu,
                                    rOmDauNgan,rKhac,rTongSo,iThang_Quy,iTrangThai,iID_MaTrangThaiDuyet,iID_MaNamNganSach
                                    ,iID_MaNguonNganSach,iID_MaDonVi
                                    ,rSoNguoi=Case WHEN sKyHieu='115' Then 
                                    (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
                                    WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) 
                                    ELSE rSoNguoi END
                                    ,rBHXH_CN=rTongSo*(SELECT rBHXH_CN FROM BH_DanhMucThuBaoHiem 
                                    WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHXH_DV=CASE WHEN sKyHieu='115' Then 
	                                (SELECT rSoNguoi=SUM(rBinhNhat)+SUM(rBinhNhi)+SUM(rHaSi)+SUM(rThuongSi) FROM QTQS_ChungTuChiTiet
	                                 WHERE SUBSTRING(sKyHieu,1,1) ='7' AND iThang_Quy=BH_PhaiThuChungTuChiTiet.iThang_Quy {4}) * 
	                                (SELECT rBHXH_CS*rLuongToiThieu FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                ELSE
	                                rTongSo*(SELECT rBHXH_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
	                                END
                                    ,rBHYT_CN=rTongSo*(SELECT rBHYT_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHYT_DV=rTongSo*(SELECT rBHYT_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_CN=rTongSo*(SELECT rBHTN_CN FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    ,rBHTN_DV=rTongSo*(SELECT rBHTN_DV FROM BH_DanhMucThuBaoHiem WHERE iThang=BH_PhaiThuChungTuChiTiet.iThang_Quy AND sKyHieu=BH_PhaiThuChungTuChiTiet.sKyHieu)
                                    FROM BH_PhaiThuChungTuChiTiet
                                    ) TB
                                     INNER JOIN  (Select * from NS_DonVi where iNamLamViec_DonVi={5}) as DV on DV.iID_MaDonVi = TB.iID_MaDonVi
                                    WHERE TB.bLaHangCha = 0 AND TB.iTrangThai = 1  {2} {0} {1} {3} 
                                    GROUP BY TB.sKyHieu,TB.sMoTa,TB.bLaHangCha
                                    HAVING sum(rSoNguoi) <>0 or SUM(rLuongCoBan) <>0 or SUM(rThamNien) <>0 or 
                                    SUM(rPCChucVu) <>0 or SUM(rOmDauNgan) <>0 or SUM(rKhac) <>0 or SUM(rTongSo)<>0 or 
                                    SUM(rBHXH_CN)<>0 or SUM(rBHXH_DV)<>0 or SUM(rBHYT_CN)<>0 or SUM(rBHYT_DV)<>0 or 
                                     SUM(rBHTN_CN) <>0 or SUM(rBHTN_DV) <>0 ORDER BY sKyHieu", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKThang, DKDonvi, DKDuyet,NguoiDungCauHinhModels.iNamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", ThangQuy);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iID_MaNhomDonVi", iID_MaNhomDonVi);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }
    
    
    }
}
