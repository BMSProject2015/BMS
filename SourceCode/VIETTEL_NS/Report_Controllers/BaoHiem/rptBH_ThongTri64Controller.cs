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
using System.Collections;

namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_ThongTri64Controller : Controller
    {
        //
        // GET: /rptBH_ThongTri64/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String LoaiThongTri = "", String LoaiBaoHiem = "", String iDonViDong = "")
        {
            String sFilePath = "";
            //kiểm tra loại thông tri
            if (LoaiThongTri == "TongHopDonVi")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64.xls";
            }
            else if (LoaiThongTri == "TongHop")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHop.xls";
            }
            if (LoaiThongTri == "TongHop" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiem.xls";
            }
            else if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinh.xls";
            }
            if (LoaiThongTri == "TongHop" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiemVaDonViTinh.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopLoaiBaoHiem.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinhVaTongHopBaoHiem.xls";
            }
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_ThongTri64.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Action thực hiện điều kiện lọc dữ liệu ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {

            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String Thang_Quy = "";
            String LoaiThangQuy = Convert.ToString(Request.Form[ParentID + "_LoaiThangQuy"]);
            if (LoaiThangQuy == "1")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String LoaiBaoHiem = Request.Form[ParentID + "_LoaiBaoHiem"];
            String LoaiThongTri = Request.Form[ParentID + "_LoaiThongTri"];
            String iDonViDong = Request.Form[ParentID + "_iDonViDong"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["LoaiBaoHiem"] = LoaiBaoHiem;
            ViewData["LoaiThongTri"] = LoaiThongTri;
            ViewData["iDonViDong"] = iDonViDong;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_ThongTri64.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String tendv = "";
            // lấy tên đơn vị
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = TenDonVi(sMaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            String TenDonViTinh = "";
            if (iDonViDong == "1")
            {
                TenDonViTinh = "Cá nhân ";
            }
            if (iDonViDong == "2")
            {
                TenDonViTinh = "Đơn vị ";
            }
            // nếu đơn vị tính bằng 3
            if (iDonViDong == "3")
            {
                TenDonViTinh = "Tổng cộng";
            }
            String TenLoaiBaoHiem = "";
            if (LoaiBaoHiem == "1")
            {
                TenLoaiBaoHiem = "Bảo Hiểm Xã Hội ";
            }
            // nếu loại bảo hiểm bằng 2
            if (LoaiBaoHiem == "2")
            {
                TenLoaiBaoHiem = "Bảo Hiểm y Tế";
            }
            // nếu loại bảo hiểm bằng 3
            if (LoaiBaoHiem == "3")
            {
                TenLoaiBaoHiem = "Bảo Hiểm Thất nghiệp";
            }
            // nếu loại bảo hiểm bằng 4
            if (LoaiBaoHiem == "4")
            {
                TenLoaiBaoHiem = "BHXH , BHYT , BHTN";
            }
            String LoaiThang_Quy = "";
            switch (LoaiThangQuy)
            {
                case "0":
                    LoaiThang_Quy = "Tháng";
                    break;
                case "1":
                    LoaiThang_Quy = "Quý";
                    break;
                case "2":
                    LoaiThang_Quy = "Năm";
                    break;
            }
            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
            if (iDonViDong == "3" && LoaiThongTri == "TongHop")
            {
                if (LoaiBaoHiem == "1")
                {
                    TenLoaiBaoHiem = "Thu Bảo Hiểm Xã Hội";
                }
                if (LoaiBaoHiem == "2")
                {
                    TenLoaiBaoHiem = "Thu Bảo Hiểm Y Tế";
                }
                if (LoaiBaoHiem == "3")
                {
                    TenLoaiBaoHiem = "Thu Bảo Hiểm Thất Nghiệp";
                }
                else if (LoaiBaoHiem == "4")
                {
                    TenLoaiBaoHiem = "Thu BHXH , BHYT , BHTN";
                }
            }
            if (iDonViDong == "3" && LoaiThongTri == "TongHopDonVi")
            {
                if (LoaiBaoHiem == "1")
                {
                    TenLoaiBaoHiem = "Thu Bảo Hiểm Xã Hội";
                }
                if (LoaiBaoHiem == "2")
                {
                    TenLoaiBaoHiem = "Thu Bảo Hiểm Y Tế";
                }
                if (LoaiBaoHiem == "3")
                {
                    TenLoaiBaoHiem = "Thu Bảo Hiểm Thất Nghiệp";
                }
                else if (LoaiBaoHiem == "4")
                {
                    TenLoaiBaoHiem = "Thu BHXH , BHYT , BHTN";
                }
            }
            /// <summary>
            /// lấy ngày tháng năm hiện tại
            /// </summary>
            /// <returns></returns>
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            /// <summary>
            /// Hàm đổi số tiền thành chữ
            /// </summary>
            /// <returns></returns>
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_ThongTri64");
            /// <summary>
            /// Hàm load dữ liệu ra báo cáo
            /// </summary>
            /// <returns></returns>

            LoadData(fr, MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);

            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            if (LoaiThongTri == "TongHop")
            {
                fr.SetValue("TenDV", "");
            }
            else
            {
                fr.SetValue("TenDV", "Tên đơn vị : " + tendv);
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("BTLThongTinLienLac", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Thang_Quy", Thang_Quy);
            fr.SetValue("LoaiThangQuy", LoaiThang_Quy);
            fr.SetValue("DVTinh", TenDonViTinh);
            fr.SetValue("LoaiBH", TenLoaiBaoHiem);
            fr.SetValue("SoTien", CommonFunction.TienRaChu(Tong1).ToString());
            fr.SetValue("Tien2", CommonFunction.TienRaChu(Tong2).ToString());
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Tien", CommonFunction.TienRaChu(Tong));
            fr.SetValue("TongTien", CommonFunction.TienRaChu(TongTien));
            fr.Run(Result);
            return Result;

        }
        public long Tong = 0;
        /// <summary>
        /// Hiển thị danh sách Tên đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        private SortedDictionary<string, Decimal> LayDuLieu(DataTable dt)
        {
            SortedDictionary<string, Decimal> DuLieu = XuLyDuLieu(dt);
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
                Decimal TienHangCon = (Decimal)DuLieu[key];
                if (key.Length > 1) parent_key = key.Substring(0, key.Length - 1);
                if (DuLieu.ContainsKey(parent_key))
                {
                    Decimal TienHangCha = 0;
                    if (KhoaCha[parent_key] != null)
                    {
                        TienHangCha = (Decimal)DuLieu[parent_key];
                    }
                    else
                    {
                        KhoaCha.Add(parent_key, true);
                    }
                    DuLieu[parent_key] = TienHangCha + TienHangCon;
                }
            }
            return DuLieu;
        }
        private SortedDictionary<string, Decimal> XuLyDuLieu(DataTable dt)
        {
            SortedDictionary<string, Decimal> DuLieu = new SortedDictionary<string, Decimal>();
            foreach (DataRow row in dt.Rows)
            {
                String sKyHieu = Convert.ToString(row["sKyHieu"]);
                if (!String.IsNullOrEmpty(sKyHieu))
                {
                    Decimal SoTien = 0;
                    try { SoTien = Convert.ToDecimal(row["SoTien"]); }
                    catch { SoTien = 0; }
                    if (!DuLieu.ContainsKey(sKyHieu))
                    {
                        DuLieu.Add(sKyHieu, SoTien);
                    }
                    else
                    {
                        DuLieu[sKyHieu] = Convert.ToDecimal(DuLieu[sKyHieu]) + SoTien;
                    }
                }
            }
            return DuLieu;
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
                        HangCha["CaNhanDong"] = 0;
                        HangCha["DonViDong"] = 0;
                        KhoaCha.Add(parent_key, true);
                    }
                    HangCha["CaNhanDong"] = Convert.ToDecimal(HangCha["CaNhanDong"]) + Convert.ToDecimal(HangCon["CaNhanDong"]);
                    HangCha["DonViDong"] = Convert.ToDecimal(HangCha["DonViDong"]) + Convert.ToDecimal(HangCon["DonViDong"]);
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
                Decimal CaNhanDong = 0, DonViDong = 0;
                try { CaNhanDong = Convert.ToDecimal(row["CaNhanDong"]); }
                catch { CaNhanDong = 0; }
                try { DonViDong = Convert.ToDecimal(row["DonViDong"]); }
                catch { DonViDong = 0; }
                if (!DuLieu.ContainsKey(sKyHieu))
                {
                    Hashtable ChiTiet = new Hashtable();
                    ChiTiet.Add("CaNhanDong", CaNhanDong);
                    ChiTiet.Add("DonViDong", DonViDong);
                    DuLieu.Add(sKyHieu, ChiTiet);
                }
                else
                {
                    Hashtable ChiTiet = (Hashtable)DuLieu[sKyHieu];
                    ChiTiet["CaNhanDong"] = Convert.ToDecimal(ChiTiet["CaNhanDong"]) + CaNhanDong;
                    ChiTiet["DonViDong"] = Convert.ToDecimal(ChiTiet["DonViDong"]) + DonViDong;
                    DuLieu[sKyHieu] = ChiTiet;
                }
            }
            return DuLieu;
        }

        public long Tong1 = 0;
        public DataTable rptBH_ThongTri64TongHopDVTinhVaLoaiBaoHiem(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String DKThangQuy = "", DK_ThangQuy_DM = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy = "iThang_Quy between 1 and 3";
                        DK_ThangQuy_DM = "iThang between 1 and 3";
                        break;
                    case "2":
                        DKThangQuy = "iThang_Quy between 4 and 6";
                        DK_ThangQuy_DM = "iThang between 4 and 6";
                        break;
                    case "3":
                        DKThangQuy = "iThang_Quy between 7 and 9";
                        DK_ThangQuy_DM = "iThang between 7 and 9";
                        break;
                    case "4":
                        DKThangQuy = "iThang_Quy between 10 and 12";
                        DK_ThangQuy_DM = "iThang between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThang_Quy";
                DK_ThangQuy_DM = "iThang=@iThang";
            }
            String DKDonVi = "";
            if (LoaiThongTri == "TongHop")
            {
                String[] arrDonVi = sMaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    if (i != 0) DKDonVi += " OR ";
                    DKDonVi += "BH.iID_MaDonVi=@iID_MaDonVi" + i.ToString();
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);
                }
            }
            else
            {
                DKDonVi += " BH.iID_MaDonVi=@iID_MaDonVi ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            if (iDonViDong == "3" && LoaiThongTri == "TongHop" && LoaiBaoHiem == "4")
            {

                String SQL = String.Format(@" SELECT sTen
                                        ,SUM(BHXH) as BHXH
                                        ,SUM(BHYT) as BHYT
                                        ,SUM(BHTN) as BHTN
                                        FROM
                                        (
                                        SELECT
                                        sTen
                                        ,BHXH=rTongSo*rBHXH_CN + rTongSo*rBHXH_DV
                                        ,BHYT=rTongSo*rBHYT_CN + rTongSo*rBHYT_DV
                                        ,BHTN=rTongSo*rBHTN_CN + rTongSo*rBHTN_DV
                                        FROM
                                        (
                                        SELECT bLaHangCha,DV.iID_MaDonVi,DV.sTen,sKyHieu,BH.sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                        FROM BH_PhaiThuChungTuChiTiet as BH
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV 
                                        ON BH.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE BH.iTrangThai=1 {3} AND ({1}) AND {2}
                                        GROUP BY bLaHangCha,sKyHieu,BH.sMoTa,DV.iID_MaDonVi,DV.sTen,iThang_Quy
                                        ) as tblA
                                        INNER JOIN
                                        (
                                        SELECT bLaHangCha,sKyHieu,sMoTa
                                        ,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,rBHXH_CS,rLuongToiThieu,iThang
                                        FROM BH_DanhMucThuBaoHiem
                                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {4}
                                        ) as tblB
                                           ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                        ) as tblMain
                                        GROUP By sTen", iID_MaTrangThaiDuyet, DKDonVi, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DK_ThangQuy_DM);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
                    cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
                }
                dt = Connection.GetDataTable(cmd);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Tong1 += long.Parse(dt.Rows[i]["TongCong"].ToString());
                //}
                /// <summary>
                /// nếu dữ liệu nhỏ hơn 10 rows thì insert thêm dòng trắng 
                /// </summary>
                /// <returns></returns>
                if (LoaiThongTri == "TongHop" && iDonViDong == "3" && LoaiBaoHiem == "4")
                {
                    int a = dt.Rows.Count;
                    if (a <= 14 && a > 0)
                    {
                        for (int i = 0; i < 15 - a; i++)
                        {
                            DataRow dr;
                            dr = dt.NewRow();
                            dt.Rows.InsertAt(dr, a + 1);
                        }
                    }
                }
                cmd.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Lây dự liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public long TongTien = 0;
        public DataTable rptBH_ThongTri64(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {

            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String DKThangQuy = "";
            if (LoaiThangQuy == "1")
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
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThangQuy";
            }
            String DKThangQuy_1 = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy_1 = "iThang between 1 and 3";
                        break;
                    case "2":
                        DKThangQuy_1 = "iThang between 4 and 6";
                        break;
                    case "3":
                        DKThangQuy_1 = "iThang between 7 and 9";
                        break;
                    case "4":
                        DKThangQuy_1 = "iThang between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThangQuy_1 = "iThang=@iThang";
            }
            //Kiểm tra trạng thái duyệt
            String TK_Duyet_QT = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
                TK_Duyet_QT = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
                TK_Duyet_QT = "";
            }
            //Kiểm tra Loại bảo hiểm
            String SubSelect = String.Format(@"(SELECT TOP 1 TongSo=A.BinhNhat+A.BinhNhi+A.HaSi
                                        +A.ThuongSi+A.TrungSi
                                        FROM
                                        (
                                        SELECT QT.iID_MaDonVi,DV.sTen,SUM(rBinhNhat) BinhNhat
                                        ,SUM(rBinhNhi) BinhNhi,SUM(rHaSi) HaSi
                                        ,SUM(rTrungSi) TrungSi,SUM(rThuongSi) ThuongSi
                                         FROM QTQS_ChungTuChiTiet as QT
                                          INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                         ON QT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE QT.iTrangThai=1 AND {2} {0} {1} AND QT.iID_MaDonVi=@iID_MaDonVi 
                                        AND SUBSTRING(QT.sKyHieu,1,1) IN(7)
                                        GROUP BY QT.iID_MaDonVi,DV.sTen
                                        HAVING SUM(rBinhNhat)!=0 OR SUM(rBinhNhi)!=0 OR SUM(rHaSi)!=0
                                        OR SUM(rTrungSi)!=0 OR SUM(rThuongSi)!=0
                                        )
                                        as A
                                        GROUP BY BinhNhat,BinhNhi,HaSi,ThuongSi,TrungSi)*rBHXH_CS*rLuongToiThieu", ReportModels.DieuKien_NganSach(MaND), TK_Duyet_QT, DKThangQuy);

            String Cl_ChienSi = "";
            if (LoaiBaoHiem == "1")
            {
                if (iDonViDong == "1")
                {
                    LoaiBaoHiem = "SUM(rTongSo)*rBHXH_CN";
                    Cl_ChienSi = "SUM(rTongSo)*rBHXH_CN";
                }
                else if (iDonViDong == "2")
                {
                    LoaiBaoHiem = "SUM(rTongSo)*rBHXH_DV";
                    Cl_ChienSi = SubSelect;
                }

            }
            else if (LoaiBaoHiem == "2")
            {
                if (iDonViDong == "1")
                {
                    Cl_ChienSi = "SUM(rTongSo)*rBHYT_CN";
                    LoaiBaoHiem = "SUM(rTongSo)*rBHYT_CN";
                }
                else if (iDonViDong == "2")
                {
                    LoaiBaoHiem = "SUM(rTongSo)*rBHYT_DV";
                    Cl_ChienSi = "SUM(rTongSo)*rBHYT_DV";
                }
            }
            else if (LoaiBaoHiem == "3")
            {
                if (iDonViDong == "1")
                {
                    Cl_ChienSi = "SUM(rTongSo)*rBHTN_CN";
                    LoaiBaoHiem = "SUM(rTongSo)*rBHTN_CN";
                }
                else if (iDonViDong == "2")
                {
                    LoaiBaoHiem = "SUM(rTongSo)*rBHTN_DV";
                    Cl_ChienSi = "SUM(rTongSo)*rBHTN_DV";
                }
            }
            String DKDonVi = "";
            String[] arrDonVi = sMaDonVi.Split(',');
            if (LoaiThongTri == "TongHop")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    if (i != 0) DKDonVi += " OR ";
                    DKDonVi += "BH.iID_MaDonVi=@iID_MaDonVi" + i.ToString();
                    //cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);
                }
            }
            else
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi ";
                //cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            //Kiểm tra Loại Thông Tri
            if (LoaiThongTri == "TongHopDonVi")
            #region Từng  hợp đơn vị
            {
                String SQL = String.Format(@" SELECT
                                            B.bLaHangCha,B.sKyHieu,B.sMoTa
                                            ,SoTien=CASE WHEN B.sKyHieu=115 THEN {5} ELSE {0} END
                                            FROM
                                            (
                                            SELECT bLaHangCha,sKyHieu,sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy FROM BH_PhaiThuChungTuChiTiet
                                            WHERE iTrangThai=1 AND {2} {3}
                                            AND iID_MaDonVi=@iID_MaDonVi {1}
                                            GROUP BY bLaHangCha,sKyHieu,sMoTa,iThang_Quy
                                            HAVING SUM(rTongSo)!=0
                                            ) as A
                                            RIGHT JOIN
                                            (
                                            SELECT bLaHangCha,sKyHieu,sMoTa,iThang
                                            ,rBHTN_CN,rBHTN_DV
                                            ,rBHXH_CN,rBHXH_DV
                                            ,rBHYT_CN,rBHYT_DV 
                                            ,rBHXH_CS,rLuongToiThieu
                                            FROM BH_DanhMucThuBaoHiem
                                            WHERE iTrangThai=1 AND {4} AND iNamLamViec=@iNamLamViec
                                            ) as B
                                            ON A.sKyHieu=B.sKyHieu AND iThang_Quy=iThang
                                            GROUP BY B.bLaHangCha,B.sKyHieu,B.sMoTa
                                            ,rBHTN_CN,rBHTN_DV
                                            ,rBHXH_CN,rBHXH_DV
                                            ,rBHYT_CN,rBHYT_DV ,rBHXH_CS,rLuongToiThieu
                                            ORDER BY sKyHieu", LoaiBaoHiem, iID_MaTrangThaiDuyet, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DKThangQuy_1, Cl_ChienSi);

                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThongTri == "TongHop")
                {
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
                }
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);

                }
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
                }
                dt = Connection.GetDataTable(cmd);
                //tính tổng tiền để đổi số tiền ra chữ
                //for (int i=0; i < dt.Rows.Count; i++)
                //{
                //    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                //}
                //Nếu dòng dữ liệu nhỏ hơn hoặc bằng 10 và >0 thì insert thêm dòng dữ liệu trắng
                int a = dt.Rows.Count;
                if (a <= 14 && a > 0)
                {
                    for (int i = 0; i < 15 - a; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                //
                dt.Dispose();
                cmd.Dispose();
            }
            #endregion

            if (LoaiThongTri == "TongHop")
            //rptBH_ThongTri64TongHop.xls

            #region Tổng hợp
            {

                String SQLTongHop = String.Format(@"SELECT
                                            sTen,SUM(SoTien) as SoTien
                                            FROM
                                            (
                                            SELECT
                                            sTen,{0} as SoTien
                                            FROM
                                            (
                                            SELECT sKyHieu,DV.sTen,BH.sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                            FROM BH_PhaiThuChungTuChiTiet as BH
                                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                            ON BH.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE BH.iTrangThai=1 AND {3} {4}
                                            AND ({2}) {1}
                                            GROUP BY sKyHieu,BH.sMoTa,DV.sTen,iThang_Quy
                                            ) as tblA
                                            INNER JOIN
                                            (
                                            SELECT sKyHieu,sMoTa,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,iThang
                                             FROM BH_DanhMucThuBaoHiem
                                             WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {5}
                                             ) as tblB
                                             ON tblA.sKyHieu=tblB.sKyHieu  AND iThang_Quy=iThang
                                             GROUP BY sTen,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV
                                             ) as tblMain
                                             GROUP BY sTen
                                             HAVING SUM(SoTien)!=0", LoaiBaoHiem, iID_MaTrangThaiDuyet, DKDonVi, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DKThangQuy_1);
                SqlCommand cmdTongHop = new SqlCommand(SQLTongHop);
                if (LoaiThongTri == "TongHop")
                {
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmdTongHop.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                }
                else
                {
                    cmdTongHop.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                cmdTongHop.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThangQuy == "0")
                {
                    cmdTongHop.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                }
                if (LoaiThangQuy == "0")
                {
                    cmdTongHop.Parameters.AddWithValue("@iThang", Thang_Quy);
                }
                dt = Connection.GetDataTable(cmdTongHop);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    TongTien += long.Parse(dt.Rows[i]["SoTien"].ToString());
                //}
                //Nếu dòng dữ liệu nhỏ hơn hoặc bằng 10 và >0 thì insert thêm dòng dữ liệu trắng
                int a = dt.Rows.Count;
                if (a <= 14 && a > 0)
                {
                    for (int i = 0; i < 15 - a; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmdTongHop.Dispose();

            }

            String TenBaoHiem = "";
            if (iDonViDong == "3" && LoaiThongTri == "TongHop")
            {

                if (LoaiBaoHiem == "1")
                {
                    TenBaoHiem = "SUM(rTongSo)*rBHXH_CN + SUM(rTongSo)*rBHXH_DV";
                }
                if (LoaiBaoHiem == "2")
                {
                    TenBaoHiem = "SUM(rTongSo)*rBHYT_CN + SUM(rTongSo)*rBHYT_DV";
                }
                if (LoaiBaoHiem == "3")
                {
                    TenBaoHiem = "SUM(rTongSo)*rBHTN_CN + SUM(rTongSo)*rBHTN_DV";
                }

                String SQLTongHopDonVi = string.Format(@"SELECT
                                            sTen,SUM(SoTien) as SoTien
                                            FROM
                                            (
                                            SELECT
                                            sTen,{0} as SoTien
                                            FROM
                                            (
                                            SELECT sKyHieu,DV.sTen,BH.sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                            FROM BH_PhaiThuChungTuChiTiet as BH
                                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                            ON BH.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE BH.iTrangThai=1 AND {3} {4}
                                            AND ({2}) {1}
                                            GROUP BY sKyHieu,BH.sMoTa,DV.sTen,iThang_Quy
                                            ) as tblA
                                            INNER JOIN
                                            (
                                            SELECT sKyHieu,sMoTa,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,iThang
                                             FROM BH_DanhMucThuBaoHiem
                                             WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {5}
                                             ) as tblB
                                             ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                             GROUP BY sTen,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV
                                             ) as tblMain
                                             GROUP BY sTen
                                             HAVING SUM(SoTien)!=0", TenBaoHiem, iID_MaTrangThaiDuyet, DKDonVi, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DKThangQuy_1);
                SqlCommand cmdTHDV = new SqlCommand(SQLTongHopDonVi);
                if (LoaiThongTri == "TongHop")
                {
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmdTHDV.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                }
                else
                {
                    cmdTHDV.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                cmdTHDV.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThangQuy == "0")
                {
                    cmdTHDV.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                    cmdTHDV.Parameters.AddWithValue("@iThang", Thang_Quy);
                }
                dt = Connection.GetDataTable(cmdTHDV);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                //}
                //Nếu dòng dữ liệu nhỏ hơn hoặc bằng 10 và >0 thì insert thêm dòng dữ liệu trắng
                int a = dt.Rows.Count;
                if (a <= 14 && a > 0)
                {
                    for (int i = 0; i < 15 - a; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmdTHDV.Dispose();
            }
            #endregion

            return dt;
        }

        #region//Tổng hợp đơn vị tính
        /// <summary>
        /// Lấy dữ liệu theo đơn vị tính - rptBH_ThongTri64TongHopDVTinh.xls
        /// </summary>
        /// <returns></returns>
        public DataTable rptBH_ThongTri64TongHopDVTinh(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            DataTable dt = null;
            String DKThangQuy = "";
            if (LoaiThangQuy == "1")
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
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThangQuy";
            }
            String DK_ThangQuy_DM = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DK_ThangQuy_DM = "iThang between 1 and 3";
                        break;
                    case "2":
                        DK_ThangQuy_DM = "iThang between 4 and 6";
                        break;
                    case "3":
                        DK_ThangQuy_DM = "iThang between 7 and 9";
                        break;
                    case "4":
                        DK_ThangQuy_DM = "iThang between 10 and 12";
                        break;
                }
            }
            else
            {
                DK_ThangQuy_DM = "iThang=@iThang";
            }
            String TK_Duyet_QT = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
                TK_Duyet_QT = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
                TK_Duyet_QT = "";
            }
            //Kiểm tra Trạng thái duyệt 
            String DKDonVi = "";
            String[] arrDonVi = sMaDonVi.Split(',');
            if (LoaiThongTri == "TongHop")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    if (i != 0) DKDonVi += " OR ";
                    DKDonVi += "BH.iID_MaDonVi=@iID_MaDonVi" + i.ToString();
                    //cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);
                }
            }
            else
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi ";
                //cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            String SubSelect = String.Format(@"(SELECT TOP 1 TongSo=A.BinhNhat+A.BinhNhi+A.HaSi
                                        +A.ThuongSi+A.TrungSi
                                        FROM
                                        (
                                        SELECT QT.iID_MaDonVi,DV.sTen,SUM(rBinhNhat) BinhNhat
                                        ,SUM(rBinhNhi) BinhNhi,SUM(rHaSi) HaSi
                                        ,SUM(rTrungSi) TrungSi,SUM(rThuongSi) ThuongSi
                                         FROM QTQS_ChungTuChiTiet as QT
                                          INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                         ON QT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE QT.iTrangThai=1 AND {2} {0} {1} AND QT.iID_MaDonVi=@iID_MaDonVi 
                                         AND SUBSTRING(QT.sKyHieu,1,1) IN(7)
                                        GROUP BY QT.iID_MaDonVi,DV.sTen
                                        HAVING SUM(rBinhNhat)!=0 OR SUM(rBinhNhi)!=0 OR SUM(rHaSi)!=0
                                        OR SUM(rTrungSi)!=0 OR SUM(rThuongSi)!=0
                                        )
                                        as A
                                        GROUP BY BinhNhat,BinhNhi,HaSi,ThuongSi,TrungSi)*rBHXH_CS*rLuongToiThieu", ReportModels.DieuKien_NganSach(MaND), TK_Duyet_QT, DKThangQuy);

            if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3")
            {
                String LoaiBaoHiem1 = "", LoaiBaoHiem2 = "", ChienSi = "";
                if (LoaiBaoHiem == "1")
                {
                    LoaiBaoHiem1 = "SUM(rTongSo)*rBHXH_CN";
                    LoaiBaoHiem2 = "SUM(rTongSo)*rBHXH_DV";
                    ChienSi = SubSelect;

                }
                if (LoaiBaoHiem == "2")
                {
                    LoaiBaoHiem1 = "SUM(rTongSo)*rBHYT_CN";
                    LoaiBaoHiem2 = "SUM(rTongSo)*rBHYT_DV";
                    ChienSi = "SUM(rTongSo)*rBHYT_DV"; ;

                }
                if (LoaiBaoHiem == "3")
                {
                    LoaiBaoHiem1 = "SUM(rTongSo)*rBHTN_CN";
                    LoaiBaoHiem2 = "SUM(rTongSo)*rBHTN_DV";
                    ChienSi = "SUM(rTongSo)*rBHTN_DV";

                }


                String SQL = String.Format(@"SELECT bLaHangCha,sKyHieu,sMoTa,SUM(CaNhanDong) as CaNhanDong
                                                ,SUM(DonViDong) as DonViDong
                                                FROM
                                                (
                                                SELECT tblB.bLaHangCha,tblB.sKyHieu,tblB.sMoTa
                                                ,{0} as CaNhanDong
                                                ,DonViDong=CASE WHEN tblB.sKyHieu=115 THEN {5} ELSE
										                                               {1}  END
                                                FROM
                                                (
                                                SELECT bLaHangCha,sKyHieu,sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                                FROM BH_PhaiThuChungTuChiTiet 
                                                WHERE iTrangThai=1 AND {3}  AND iID_MaDonVi=@iID_MaDonVi {2}
                                                {4}
                                                GROUP BY sKyHieu,sMoTa,bLaHangCha,iThang_Quy
                                                ) as tblA
                                                INNER JOIN
                                                (
                                                SELECT bLaHangCha,sKyHieu,sMoTa,rBHTN_CN,rBHXH_CS,rLuongToiThieu,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,iThang
                                                FROM BH_DanhMucThuBaoHiem
                                                WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {6} 
                                                ) as tblB
                                                ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                                GROUP BY tblB.bLaHangCha,tblB.sKyHieu,tblB.sMoTa,rBHTN_CN,rBHXH_CS,rLuongToiThieu,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV
                                               ) as tblMain
GROUP BY bLaHangCha,sKyHieu,sMoTa
ORDER BY sKyHieu", LoaiBaoHiem1, LoaiBaoHiem2, iID_MaTrangThaiDuyet, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), ChienSi, DK_ThangQuy_DM);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                    cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
                }

                dt = Connection.GetDataTable(cmd);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Tong += long.Parse(dt.Rows[i]["Tong"].ToString());
                //}
                //Nếu dòng dữ liệu nhỏ hơn hoặc bằng 10 và >0 thì insert thêm dòng dữ liệu trắng
                int a = dt.Rows.Count;
                if (a <= 14 && a > 0)
                {
                    for (int i = 0; i < 15 - a; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();

            }

            return dt;
        }
        #endregion

        #region Tổng hợp đơn vị tính và loại bảo hiểm

        /// <summary>
        /// Mẫu  rptBH_ThongTri64TongHopDVTinhVaTongHopBaoHiem.xls
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiBaoHiem"></param>
        /// <param name="LoaiThongTri"></param>
        /// <param name="iDonViDong"></param>
        /// <returns></returns>
        public DataTable rptBH_ThongTri64TongHopDonVi(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String DKThangQuy = "", DK_Thang_Quy = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy = "iThang_Quy between 1 and 3";
                        DK_Thang_Quy = "iThang between 1 and 3";
                        break;
                    case "2":
                        DKThangQuy = "iThang_Quy between 4 and 6";
                        DK_Thang_Quy = "iThang between 4 and 6";
                        break;
                    case "3":
                        DKThangQuy = "iThang_Quy between 7 and 9";
                        DK_Thang_Quy = "iThang between 7 and 9";
                        break;
                    case "4":
                        DKThangQuy = "iThang_Quy between 10 and 12";
                        DK_Thang_Quy = "iThang between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThangQuy";
                DK_Thang_Quy = "iThang=@iThang";
            }

            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKDonVi = "";
            String[] arrDonVi = sMaDonVi.Split(',');
            if (LoaiThongTri == "TongHop")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    if (i != 0) DKDonVi += " OR ";
                    DKDonVi += "BH.iID_MaDonVi=@iID_MaDonVi" + i.ToString();
                    //cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);
                }
            }
            else
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi ";
                //cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {

                String SQL = String.Format(@"SELECT
                                            SUM(rBHXH_CN) as rBHXH_CN,SUM(rBHXH_DV) as rBHXH_DV
                                            ,SUM(rBHYT_CN) as rBHYT_CN, SUM(rBHYT_DV) as rBHYT_DV
                                            ,SUM(rBHTN_CN) as rBHTN_CN,SUM(rBHTN_DV) as rBHTN_DV
                                            ,BHXH_TongCong=SUM(rBHXH_CN)+SUM(rBHXH_DV)
                                            ,BHYT_TongCong=SUM(rBHYT_CN)+SUM(rBHYT_DV)
                                            ,BHTN_TongCong=SUM(rBHTN_CN)+SUM(rBHTN_DV)
                                            FROM
                                            (
                                            SELECT 
                                             SUM(rTongSo)*rBHXH_CN as rBHXH_CN,SUM(rTongSo)*rBHXH_DV as rBHXH_DV
                                             ,SUM(rTongSo)*rBHYT_CN as rBHYT_CN,SUM(rTongSo)*rBHYT_DV as rBHYT_DV
                                            ,SUM(rTongSo)*rBHTN_CN as rBHTN_CN,SUM(rTongSo)*rBHTN_DV as rBHTN_DV
                                            FROM
                                            (
                                            SELECT bLaHangCha,sKyHieu,sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                            FROM BH_PhaiThuChungTuChiTiet 
                                            WHERE iTrangThai=1 AND {1}  AND iID_MaDonVi=@iID_MaDonVi {0}
                                            {2}
                                            GROUP BY sKyHieu,sMoTa,bLaHangCha,iThang_Quy
                                            )as tblA
                                            INNER JOIN
                                            (
                                            SELECT bLaHangCha,sKyHieu,sMoTa,rBHTN_CN,rBHXH_CS,rLuongToiThieu,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,iThang
                                            FROM BH_DanhMucThuBaoHiem
                                            WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {3}
                                            ) as tblB
                                              ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                            GROUP By rBHTN_CN,rBHXH_CS,rLuongToiThieu,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV
                                            ) as tblMain
                                            ", iID_MaTrangThaiDuyet, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DK_Thang_Quy);
                cmd.CommandText = SQL;
                //if (LoaiThongTri == "TongHop")
                //{
                //    for (int i = 0; i < arrDonVi.Length; i++)
                //    {
                //        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                //    }
                //}
                //else
                //{
                cmd.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
                //}
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                    cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
                }

                dt = Connection.GetDataTable(cmd);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Tong1 += long.Parse(dt.Rows[i]["TongCong"].ToString());
                //}
                /// <summary>
                /// nếu dữ liệu nhỏ hơn 10 rows thì insert thêm dòng trắng 
                /// </summary>
                /// <returns></returns>

                cmd.Dispose();

            }

            return dt;
        }

        #endregion

        #region Tổng hợp Bảo Hiểm
        /// <summary>
        /// Loại Thông tri Tổng hợp và loại bảo hiểm = 4
        /// </summary>
        /// <returns></returns>
        public long Tong2 = 0;
        public DataTable rptBH_ThongTri64TongHopBaoHiem(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            DataTable dt = null;

            String DKDonVi = "";
            String[] arrDonVi = sMaDonVi.Split(',');
            if (LoaiThongTri == "TongHop")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    if (i != 0) DKDonVi += " OR ";
                    DKDonVi += "BH.iID_MaDonVi=@iID_MaDonVi" + i.ToString();
                    //cmd.Parameters.AddWithValue("@iID_MaDonVi" + i.ToString(), arrDonVi[i]);
                }
            }
            else
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi ";
                //cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            String DK_Duyet_QT = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
                DK_Duyet_QT = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
                DK_Duyet_QT = "";
            }
            String DKThangQuy = "", DK_ThangQuy_DM = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1":
                        DKThangQuy = "iThang_Quy between 1 and 3";
                        DK_ThangQuy_DM = "iThang between 1 and 3";
                        break;
                    case "2":
                        DKThangQuy = "iThang_Quy between 4 and 6";
                        DK_ThangQuy_DM = "iThang between 4 and 6";
                        break;
                    case "3":
                        DKThangQuy = "iThang_Quy between 7 and 9";
                        DK_ThangQuy_DM = "iThang between 7 and 9";
                        break;
                    case "4":
                        DKThangQuy = "iThang_Quy between 10 and 12";
                        DK_ThangQuy_DM = "iThang between 10 and 12";
                        break;
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThangQuy";
                DK_ThangQuy_DM = "iThang=@iThang";
            }
            String LoaiBaoHiem1 = "", LoaiBaoHiem2 = "", LoaiBaoHiem3 = "";
            if (LoaiThongTri == "TongHop" && LoaiBaoHiem == "4")
            {

                if (iDonViDong == "1")
                {
                    LoaiBaoHiem1 = ",SUM(rTongSo)*rBHXH_CN";
                    LoaiBaoHiem2 = ",SUM(rTongSo)*rBHYT_CN";
                    LoaiBaoHiem3 = ",SUM(rTongSo)*rBHTN_CN";
                }
                if (iDonViDong == "2")
                {
                    LoaiBaoHiem1 = ",SUM(rTongSo)*rBHXH_DV";
                    LoaiBaoHiem2 = ",SUM(rTongSo)*rBHYT_DV";
                    LoaiBaoHiem3 = ",SUM(rTongSo)*rBHTN_DV";
                }

                String SQL = String.Format(@" SELECT sTen
                                    ,SUM(rBHXH) as rBHXH
                                    ,SUM(rBHYT) as rBHYT
                                    ,SUM(rBHTN) as rBHTN
                                    FROM
                                    (
                                    SELECT
                                    iID_MaDonVi,sTen
                                    {0} as rBHXH
                                    {1} as rBHYT
                                    {2} as rBHTN
                                    FROM
                                    (
                                    SELECT bLaHangCha,sKyHieu,DV.iID_MaDonVi,DV.sTen,SUM(rTongSo) as rTongSo,iThang_Quy
                                    FROM BH_PhaiThuChungTuChiTiet as BH
                                     INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                    ON BH.iID_MaDonVi=DV.iID_MaDonVi
                                    WHERE BH.iTrangThai=1 AND {5} AND {4} {3}
                                    {6}
                                    GROUP BY sKyHieu,BH.sMoTa,bLaHangCha,DV.iID_MaDonVi,DV.sTen,iThang_Quy
                                    ) as tblA
                                    INNER JOIN
                                    (
                                    SELECT bLaHangCha,sKyHieu,sMoTa,rBHTN_CN,rBHXH_CS,rLuongToiThieu,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,iThang
                                    FROM BH_DanhMucThuBaoHiem
                                    WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {7}
                                    ) as tblB
                                    ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                    GROUP BY iID_MaDonVi,sTen,rBHTN_CN,rBHXH_CS,rLuongToiThieu,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV
                                    ) as tblMain
                                    GROUP BY sTen", LoaiBaoHiem1, LoaiBaoHiem2, LoaiBaoHiem3, iID_MaTrangThaiDuyet, DKDonVi, DKThangQuy, ReportModels.DieuKien_NganSach(MaND), DK_ThangQuy_DM);
                SqlCommand cmd = new SqlCommand(SQL);
                //if (LoaiThongTri == "TongHop")
                //{
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                }
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
                //}
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                    cmd.Parameters.AddWithValue("@iThang", Thang_Quy);
                }
                dt = Connection.GetDataTable(cmd);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Tong2 += long.Parse(dt.Rows[i]["Tong"].ToString());
                //}
                int a = dt.Rows.Count;
                if (a <= 14 && a > 0)
                {
                    for (int i = 0; i < 15 - a; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }


                cmd.Dispose();

            }
            // Mẫu rptBH_ThongTri64TongHopLoaiBaoHiem.xls
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4")
            {
                String SubSelect = String.Format(@"(SELECT TOP 1 TongSo=A.BinhNhat+A.BinhNhi+A.HaSi
                                        +A.ThuongSi+A.TrungSi
                                        FROM
                                        (
                                        SELECT QT.iID_MaDonVi,DV.sTen,SUM(rBinhNhat) BinhNhat
                                        ,SUM(rBinhNhi) BinhNhi,SUM(rHaSi) HaSi
                                        ,SUM(rTrungSi) TrungSi,SUM(rThuongSi) ThuongSi
                                         FROM QTQS_ChungTuChiTiet as QT
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                         ON QT.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE QT.iTrangThai=1 AND {2} {0} {1} AND QT.iID_MaDonVi=@iID_MaDonVi 
                                        GROUP BY QT.iID_MaDonVi,DV.sTen
                                        HAVING SUM(rBinhNhat)!=0 OR SUM(rBinhNhi)!=0 OR SUM(rHaSi)!=0
                                        OR SUM(rTrungSi)!=0 OR SUM(rThuongSi)!=0
                                        )
                                        as A
                                        GROUP BY BinhNhat,BinhNhi,HaSi,ThuongSi,TrungSi)*rBHXH_CS*rLuongToiThieu", ReportModels.DieuKien_NganSach(MaND), DK_Duyet_QT, DKThangQuy);

                if (iDonViDong == "1")
                {
                    LoaiBaoHiem1 = "SUM(rTongSo)*rBHXH_CN";
                    LoaiBaoHiem2 = "SUM(rTongSo)*rBHYT_CN";
                    LoaiBaoHiem3 = "SUM(rTongSo)*rBHTN_CN";

                    String SQLChiTiet = String.Format(@" SELECT
                                    SUM(SoTienXH) as SoTienXH
                                    ,SUM(SoTienYT) as SoTienYT
                                    ,SUM(SoTienTN) as SoTienTN
                                    FROM
                                    (
                                    SELECT 
                                    tblB.bLaHangCha,tblB.sKyHieu,tblB.sMoTa
                                    , SoTienXH={0}
                                    , SoTienYT={1}
                                    , SoTienTN={2}

                                    FROM
                                    (
                                    SELECT bLaHangCha,sKyHieu,sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                    FROM BH_PhaiThuChungTuChiTiet
                                    WHERE iTrangThai=1 {4} AND iID_MaDonVi=@iID_MaDonVi AND {5} {3}
                                    GROUP BY bLaHangCha,sKyHieu,sMoTa,iThang_Quy
                                    ) as tblA
                                    INNER JOIN
                                    (
                                    SELECT bLaHangCha,sKyHieu,sMoTa
                                    ,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,rBHXH_CS,rLuongToiThieu,iThang
                                    FROM BH_DanhMucThuBaoHiem
                                    WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {6}
                                    ) as tblB
                                    ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                    GROUP BY tblB.bLaHangCha,tblB.sKyHieu,tblB.sMoTa
                                    ,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,rBHXH_CS,rLuongToiThieu
                                    ) as tblMain ", LoaiBaoHiem1, LoaiBaoHiem2, LoaiBaoHiem3, iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThangQuy, DK_ThangQuy_DM);
                    SqlCommand cmdChiTiet = new SqlCommand(SQLChiTiet);
                    //if (LoaiThongTri == "TongHop")
                    //{
                    //    for (int i = 0; i < arrDonVi.Length; i++)
                    //    {
                    //        cmdChiTiet.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    //    }
                    //}
                    //else
                    //{
                    cmdChiTiet.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
                    //}
                    cmdChiTiet.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                    if (LoaiThangQuy == "0")
                    {
                        cmdChiTiet.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                        cmdChiTiet.Parameters.AddWithValue("@iThang", Thang_Quy);
                    }

                    dt = Connection.GetDataTable(cmdChiTiet);
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //       Tong += long.Parse(dt.Rows[i]["Tong"].ToString());

                    //}
                    //Nếu dòng dữ liệu nhỏ hơn hoặc bằng 10 và >0 thì insert thêm dòng dữ liệu trắng

                    cmdChiTiet.Dispose();
                }
                if (iDonViDong == "2")
                {
                    String ChienSi = "";
                    ChienSi = SubSelect;
                    LoaiBaoHiem1 = "SUM(rTongSo)*rBHXH_DV";
                    LoaiBaoHiem2 = "SUM(rTongSo)*rBHYT_DV";
                    LoaiBaoHiem3 = "SUM(rTongSo)*rBHTN_DV";

                    String SQLChiTiet2 = String.Format(@" SELECT
                                    SUM(SoTienXH) as SoTienXH
                                    ,SUM(SoTienYT) as SoTienYT
                                    ,SUM(SoTienTN) as SoTienTN
                                    FROM
                                    (
                                    SELECT 
                                    tblB.bLaHangCha,tblB.sKyHieu,tblB.sMoTa
                                  , SoTienXH=CASE WHEN tblB.sKyHieu=115 THEN {7} ELSE {0} END
                                    , SoTienYT=CASE WHEN tblB.sKyHieu=115 THEN {7} ELSE {1} END
                                    , SoTienTN=CASE WHEN tblB.sKyHieu=115 THEN {7} ELSE {2} END

                                    FROM
                                    (
                                    SELECT bLaHangCha,sKyHieu,sMoTa,SUM(rTongSo) as rTongSo,iThang_Quy
                                    FROM BH_PhaiThuChungTuChiTiet
                                    WHERE iTrangThai=1 {4} AND iID_MaDonVi=@iID_MaDonVi AND {5} {3}
                                    GROUP BY bLaHangCha,sKyHieu,sMoTa,iThang_Quy
                                    ) as tblA
                                    INNER JOIN
                                    (
                                    SELECT bLaHangCha,sKyHieu,sMoTa
                                    ,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,rBHXH_CS,rLuongToiThieu,iThang
                                    FROM BH_DanhMucThuBaoHiem
                                    WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND {6}
                                    ) as tblB
                                       ON tblA.sKyHieu=tblB.sKyHieu AND iThang_Quy=iThang
                                    GROUP BY tblB.bLaHangCha,tblB.sKyHieu,tblB.sMoTa
                                    ,rBHTN_CN,rBHTN_DV,rBHXH_CN,rBHXH_DV,rBHYT_CN,rBHYT_DV,rBHXH_CS,rLuongToiThieu
                                    ) as tblMain", LoaiBaoHiem1, LoaiBaoHiem2, LoaiBaoHiem3, iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThangQuy, DK_ThangQuy_DM, ChienSi);
                    SqlCommand cmdChiTiet2 = new SqlCommand(SQLChiTiet2);
                    //if (LoaiThongTri == "TongHop")
                    //{
                    //    for (int i = 0; i < arrDonVi.Length; i++)
                    //    {
                    //        cmdChiTiet2.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    //    }
                    //}
                    //else
                    //{
                    cmdChiTiet2.Parameters.AddWithValue("@iID_MaDonVi", sMaDonVi);
                    //}
                    cmdChiTiet2.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                    if (LoaiThangQuy == "0")
                    {
                        cmdChiTiet2.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
                        cmdChiTiet2.Parameters.AddWithValue("@iThang", Thang_Quy);
                    }

                    dt = Connection.GetDataTable(cmdChiTiet2);
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //       Tong += long.Parse(dt.Rows[i]["Tong"].ToString());

                    //}
                    //Nếu dòng dữ liệu nhỏ hơn hoặc bằng 10 và >0 thì insert thêm dòng dữ liệu trắng
                    cmdChiTiet2.Dispose();
                }
            }

            return dt;
        }


        #endregion

        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {
                DataTable DtDonViDong = rptBH_ThongTri64TongHopDonVi(MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
                DtDonViDong.TableName = "TongHopDV";
                fr.AddTable("TongHopDV", DtDonViDong);
                DtDonViDong.Dispose();
            }
            else if (LoaiThongTri == "TongHop" && LoaiBaoHiem == "4" && iDonViDong != "3")
            {
                DataTable dataTongHop = rptBH_ThongTri64TongHopBaoHiem(MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
                dataTongHop.TableName = "TongHop";
                fr.AddTable("TongHop", dataTongHop);
                dataTongHop.Dispose();
            }
            else if (LoaiThongTri == "TongHop" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {
                DataTable dataTongHopDVTinhVaLoaiBaoHiem = rptBH_ThongTri64TongHopDVTinhVaLoaiBaoHiem(MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
                dataTongHopDVTinhVaLoaiBaoHiem.TableName = "TongHopDVvaLBH";
                fr.AddTable("TongHopDVvaLBH", dataTongHopDVTinhVaLoaiBaoHiem);
                dataTongHopDVTinhVaLoaiBaoHiem.Dispose();
            }

            else if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3")
            {
                DataTable data = rptBH_ThongTri64TongHopDVTinh(MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
                SortedDictionary<string, Hashtable> DuLieu = LayDuLieu_HasTable(data);
                foreach (DataRow row in data.Rows)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                    {
                        Hashtable hs = DuLieu[Convert.ToString(row["sKyHieu"])];
                        row["CaNhanDong"] = hs["CaNhanDong"];
                        row["DonViDong"] = hs["DonViDong"];
                    }
                }
                data.TableName = "DVTinh";
                fr.AddTable("DVTinh", data);
                data.Dispose();
            }
            else if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4")
            {
                DataTable dataTongHop = rptBH_ThongTri64TongHopBaoHiem(MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
                dataTongHop.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", dataTongHop);
                dataTongHop.Dispose();
            }
            else
            {
                DataTable data = rptBH_ThongTri64(MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
                if (LoaiThongTri == "TongHopDonVi")
                {
                    SortedDictionary<string, Decimal> DuLieu = LayDuLieu(data);
                    foreach (DataRow row in data.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row["sKyHieu"])))
                        {
                            row["SoTien"] = DuLieu[Convert.ToString(row["sKyHieu"])];
                        }
                    }
                    data.TableName = "ChiTiet";
                    fr.AddTable("ChiTiet", data);
                    data.Dispose();
                }
                else
                {
                    data.TableName = "ChiTiet";
                    fr.AddTable("ChiTiet", data);
                    data.Dispose();
                }

            }

        }
        /// <summary>
        /// Hàm xuất ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (LoaiThongTri == "TongHopDonVi")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64.xls";
            }
            else if (LoaiThongTri == "TongHop")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHop.xls";
            }
            if (LoaiThongTri == "TongHop" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiem.xls";
            }
            else if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinh.xls";
            }
            if (LoaiThongTri == "TongHop" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiemVaDonViTinh.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopLoaiBaoHiem.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinhVaTongHopBaoHiem.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
            HamChung.Language();
            using (MemoryStream ms = new MemoryStream())
            {

                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTriBaoHiem.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hà xuất ra file PDF
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            String sFilePath = "";
            if (LoaiThongTri == "TongHopDonVi")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64.xls";
            }
            else if (LoaiThongTri == "TongHop")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHop.xls";
            }
            if (LoaiThongTri == "TongHop" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiem.xls";
            }
            else if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinh.xls";
            }
            if (LoaiThongTri == "TongHop" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiemVaDonViTinh.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopLoaiBaoHiem.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinhVaTongHopBaoHiem.xls";
            }
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
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
        /// View PDf
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String LoaiThangQuy, String Thang_Quy, String LoaiBaoHiem, String LoaiThongTri, String iDonViDong, String iID_MaTrangThaiDuyet, String sMaDonVi)
        {
            HamChung.Language();
            String sFilePath = "";
            if (LoaiThongTri == "TongHopDonVi")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64.xls";
            }
            else if (LoaiThongTri == "TongHop")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHop.xls";
            }
            if (LoaiThongTri == "TongHop" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiem.xls";
            }
            else if (LoaiThongTri == "TongHopDonVi" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinh.xls";
            }
            if (LoaiThongTri == "TongHop" && iDonViDong == "3" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TonghopBaohiemVaDonViTinh.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopLoaiBaoHiem.xls";
            }
            if (LoaiThongTri == "TongHopDonVi" && LoaiBaoHiem == "4" && iDonViDong == "3")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_ThongTri64TongHopDVTinhVaTongHopBaoHiem.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, LoaiThangQuy, Thang_Quy, LoaiBaoHiem, LoaiThongTri, iDonViDong, iID_MaTrangThaiDuyet, sMaDonVi);
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

        public static DataTable LayDSDonVi(String MaND, String LoaiThangQuy, String Thang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            String DKThangQuy = "";
            if (LoaiThangQuy == "1")
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
                }
            }
            else
            {
                DKThangQuy = "iThang_Quy=@iThang_Quy";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@"SELECT DISTINCT BH.iID_MaDonVi,DV.sTen
                                            FROM BH_PhaiThuChungTuChiTiet as BH
                                             INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV
                                            ON BH.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE BH.iTrangThai=1 AND BH.rTongSo>0 {0} {1} AND {2}
                                            GROUP BY BH.iID_MaDonVi,DV.sTen", ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet, DKThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThangQuy == "0")
            {
                cmd.Parameters.AddWithValue("@iThang_Quy", Thang_Quy);
            }
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public JsonResult Ds_DonVi(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = LayDSDonVi(MaND, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, "rptBH_ThongTri64");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Trạng Thái Duyệt
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
    }
}
