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

namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptKTCS_HoachToanTSCDController : Controller
    {
        //
        // GET: /rptKTCS_HoachToanTSCD/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String LoaiBieu = "")
        {
            String sFilePath = "";
            if (LoaiBieu == "rDonVi")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCDTheoDonVi.xls";
            }
            else if (LoaiBieu == "rLoaiTaiSanDonVi")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCD_TheoLTSvaDonVi.xls";
            }
            else if (LoaiBieu == "rLoaiTaiSan")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCDTheoLoaitaisan.xls";
            }
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_HoachToanTSCD.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Action thực hiện truyền các giá trị trên các from
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String LoaiBieu = Request.Form[ParentID + "_LoaiBieu"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaNhomTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["LoaiBieu"] = LoaiBieu;
            ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHopDonVi"] = TongHopDonVi;
            ViewData["TongHopLTS"] = TongHopLTS;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_HoachToanTSCD.aspx";
            return View(sViewPath + "ReportView.aspx");
//            return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS });
        }
       /// <summary>
       /// khơỉ tạo báo cáo
       /// </summary>
       /// <param name="path"></param>
       /// <param name="NamChungTu"></param>
       /// <param name="iID_MaLoaiTaiSan"></param>
       /// <param name="iID_MaDonVi"></param>
       /// <param name="LoaiBieu"></param>
       /// <param name="TongHopDonVi"></param>
       /// <param name="TongHopLTS"></param>
       /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //KLấy tên loại tài sản
            String TenLoaiTaiSan = "";
            if (TongHopLTS == "on")
            {
                TenLoaiTaiSan = "Tổng hợp toàn bộ loại tài sản ";
            }
            else
            {
                if (!String.IsNullOrEmpty(iID_MaLoaiTaiSan))
                {
                    TenLoaiTaiSan = CommonFunction.LayTruong("KTCS_NhomTaiSan", "iID_MaNhomTaiSan", iID_MaLoaiTaiSan, "sTen").ToString();
                }
            }
            //Lấy tên đơn vị
            String tendv = "";
                
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //Hàm đổi số tiền ra chữ
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_HoachToanTSCD");
            LoadData(fr, NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS);
                fr.SetValue("Nam", NamChungTu);
                if (TongHopDonVi == "on")
                {
                    fr.SetValue("TenDV", "Tổng hợp toàn bộ đơn vị");
                }
                else
                {
                    if (!String.IsNullOrEmpty(iID_MaDonVi))
                    {
                        fr.SetValue("TenDV", iID_MaDonVi+" " +CommonFunction.LayTruong("NS_DonVi","iID_MaDonVi",iID_MaDonVi,"sTen"));
                    }
                }
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                if (TongHopLTS == "on")
                {
                    fr.SetValue("TenLoaiTaiSan", "Tổng hợp toàn bộ loại tài sản");
                }
                else
                {
                    fr.SetValue("TenLoaiTaiSan", TenLoaiTaiSan);
                }
                fr.SetValue("Ngay", NgayThang);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// DataTabel lấy tên loại tài sản
        /// </summary>
        /// <returns></returns>
        public DataTable dtTenLoaiTaiSan(String iID_MaLoaiTaiSan)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM KTCS_LoaiTaiSan WHERE iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// DataTable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable rptKTCS_HoachToanTSCD(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS)
        {

            DataTable dt = null;
           // int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCongSan);
           
        
            String DKDonVi="", DKNhomTaiSan = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = ListDonVi();
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    DKDonVi += "iID_MaDonVi = '" +dtDonVi.Rows[i]["iID_MaDonVi"].ToString()+"'";
                    if (i < dtDonVi.Rows.Count - 1)
                        DKDonVi += " OR ";
                    
                }
                dtDonVi.Dispose();
            }
            else
            {
                DKDonVi = " iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'"; 
            }
            if (TongHopLTS == "on")
            {
                DataTable dtNhomTaiSan = DT_LoaiTS();
                for (int i = 0; i < dtNhomTaiSan.Rows.Count; i++)
                {
                    DKNhomTaiSan += "iID_MaNhomTaiSan like N'" + dtNhomTaiSan.Rows[i]["iID_MaNhomTaiSan"].ToString() + "%'";
                    if (i < dtNhomTaiSan.Rows.Count - 1)
                        DKNhomTaiSan += " OR ";
                }
            }
            else
            {
                DKNhomTaiSan = " iID_MaNhomTaiSan LIKE N'" + iID_MaLoaiTaiSan + "%'"; 
            }
            String SQL = String.Format(@"
SELECT SUBSTRING(b.iID_MaNhomTaiSan,1,1) as Cap1,SUBSTRING(b.iID_MaNhomTaiSan,1,2) as Cap2,a.iID_MaTaiSan,b.sTenTaiSan,b.iID_MaNhomTaiSan,e.iID_MaDonVi,e.iID_MaDonVi+ '-'+ e.sTen as sTen,e.sTen as sTenDV,rNguyenGia,rGiaTriConLai FROM (
SELECT iID_MaTaiSan,SUM(rNguyenGia) as rNguyenGia, SUM(rGiaTriConLai) as rGiaTriConLai 
FROM KTCS_KhauHaoHangNam
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec 
GROUP BY iID_MaTaiSan) as a
INNER JOIN (SELECT iID_MaTaiSan,iID_MaNhomTaiSan,sTenTaiSan
			FROM KTCS_TaiSan
			WHERE iTrangThai=1 ) as b
ON a.iID_MaTaiSan=b.iID_MaTaiSan
INNER JOIN(
SELECT iID_MaTaiSan,iID_MaDonVi
FROM KTCS_TaiSan_DonVi
WHERE iTrangThai=1  AND ({0})) as c
ON a.iID_MaTaiSan=c.iID_MaTaiSan
INNER JOIN(
SELECT iID_MaNhomTaiSan
FROM KTCS_NhomTaiSan
WHERE iTrangThai=1 AND bLahangCha=0 AND ({1})) as d
ON b.iID_MaNhomTaiSan=d.iID_MaNhomTaiSan
INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as e
on c.iID_MaDonVi=e.iID_MaDonVi
", DKDonVi,DKNhomTaiSan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// DataTable lấy tên đơn vị
        /// </summary>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTenDonVi FROM KTCS_ChungTuChiTiet WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }




        /// <summary>
        /// hiển thị dữ liệu 
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            //DataTable 
            DataTable data = rptKTCS_HoachToanTSCD(NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS);
             data.TableName = "ChiTiet";
             fr.AddTable("ChiTiet", data);
            if (LoaiBieu == "rLoaiTaiSan")
            {
                DataTable Cap2;
                Cap2 = HamChung.SelectDistinct("Cap2", data, "Cap1,Cap2", "Cap1,Cap2");
                Cap2.Columns.Add("sTen2", typeof(String));
                foreach (DataRow r2 in Cap2.Rows)
                {
                    String sTen2 = "";
                    sTen2 = CommonFunction.LayTruong("KTCS_NhomTaiSan", "iID_MaNhomTaiSan", r2["Cap2"].ToString(), "sTen").ToString();
                    r2["sTen2"] = sTen2;
                }
                fr.AddTable("Cap2", Cap2);
                Cap2.Dispose();
                DataTable Cap1;
                Cap1 = HamChung.SelectDistinct("Cap1", Cap2, "Cap1", "Cap1,Cap2");
                Cap1.Columns.Add("sTen1", typeof(String));
                foreach (DataRow r1 in Cap1.Rows)
                {
                    String sTen1 = "";
                    sTen1 = CommonFunction.LayTruong("KTCS_NhomTaiSan", "iID_MaNhomTaiSan", r1["Cap1"].ToString(), "sTen").ToString();
                    r1["sTen1"] = sTen1;
                }
                fr.AddTable("Cap1", Cap1);
                Cap1.Dispose();
            }
             else if (LoaiBieu == "rDonVi")
            {
                DataTable dtDonVi;
                dtDonVi = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi,sTen");
                dtDonVi.Columns.Add("STT", typeof(String));
                int a = 0;
                foreach (DataRow r in dtDonVi.Rows)
                {
                    a++;
                    r["STT"] = a;
                }
                fr.AddTable("DonVi", dtDonVi);
                dtDonVi.Dispose();
            }
            else if (LoaiBieu == "rLoaiTaiSanDonVi")
            {
                DataTable Cap2;
                Cap2 = HamChung.SelectDistinct("Cap2", data, "iID_MaDonVi,Cap1,Cap2", "Cap1,Cap2,iID_MaDonVi,sTen,sTenDV");
                Cap2.Columns.Add("sTen2", typeof(String));
                foreach (DataRow r2 in Cap2.Rows)
                {
                    String sTen2 = "";
                    sTen2 = CommonFunction.LayTruong("KTCS_NhomTaiSan", "iID_MaNhomTaiSan", r2["Cap2"].ToString(), "sTen").ToString();
                    r2["sTen2"] = sTen2;
                }
                fr.AddTable("Cap2", Cap2);
                Cap2.Dispose();
                DataTable Cap1;
                Cap1 = HamChung.SelectDistinct("Cap1", Cap2, "iID_MaDonVi,Cap1", "Cap1,Cap2,iID_MaDonVi,sTen,sTenDV");
                Cap1.Columns.Add("sTen1", typeof(String));
                foreach (DataRow r1 in Cap1.Rows)
                {
                    String sTen1 = "";
                    sTen1 = CommonFunction.LayTruong("KTCS_NhomTaiSan", "iID_MaNhomTaiSan", r1["Cap1"].ToString(), "sTen").ToString();
                    r1["sTen1"] = sTen1;
                }
                fr.AddTable("Cap1", Cap1);
                Cap1.Dispose();
                DataTable dtDonVi;
                dtDonVi = HamChung.SelectDistinct("DonVi", data, "iID_MaDonVi", "iID_MaDonVi,sTen,sTenDV");
               
                fr.AddTable("DonVi", dtDonVi);
                dtDonVi.Dispose();
            }
        }
        /// <summary>
        /// ACtion thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS)
        {
            String sFilePath = "";
            if (LoaiBieu == "rDonVi")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCDTheoDonVi.xls";
            }
            else if (LoaiBieu == "rLoaiTaiSanDonVi")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCD_TheoLTSvaDonVi.xls";
            }
            else if (LoaiBieu == "rLoaiTaiSan")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCDTheoLoaitaisan.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopbaoHiem.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Action xem báo cao
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String LoaiBieu, String TongHopDonVi, String TongHopLTS)
        {
            String sFilePath = "";
            if (LoaiBieu == "rDonVi")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCDTheoDonVi.xls";
            }
            else if (LoaiBieu == "rLoaiTaiSanDonVi")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCD_TheoLTSvaDonVi.xls";
            }
            else if (LoaiBieu == "rLoaiTaiSan")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_HoachToanTSCDTheoLoaitaisan.xls";
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, LoaiBieu, TongHopDonVi, TongHopLTS);
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
        public static DataTable ListDonVi()
        {
            DataTable dt;
            String SQL = String.Format(@"SELECT DISTINCT NS_DonVi.iID_MaDonVi,sTen, NS_DonVi.iID_MaDonVi + '-' + sTen as TenHT
                                        FROM KTCS_TaiSan_DonVi
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi 
                                        ON KTCS_TaiSan_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        ORDER BY NS_DonVi.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            return dt = Connection.GetDataTable(cmd);
        }
        public static DataTable DT_LoaiTS(Boolean All = false, String TieuDe = "")
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT iID_MaNhomTaiSan,iID_MaNhomTaiSan +' - '+  sTen AS TenHT
                                                FROM KTCS_NhomTaiSan
                                                WHERE iTrangThai=1
                                                ORDER BY iSTT,iID_MaNhomTaiSan ");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaLoaiTaiSan"] = Guid.Empty;
                R["TenHT"] = TieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static DataTable SelectDistinct_TaiSan(string TableName, DataTable SourceTable, string FieldName, String sFieldAdd, String sField_DK = "", String sField_DK_Value = "", String strSort = "")
        {
            DataTable dt = new DataTable(TableName);
            String[] arrFieldAdd = sFieldAdd.Split(',');
            String[] arrFieldName = FieldName.Split(',');
            for (int i = 0; i < arrFieldAdd.Length; i++)
            {
                dt.Columns.Add(arrFieldAdd[i], SourceTable.Columns[arrFieldAdd[i]].DataType);
            }
            if (SourceTable.Rows.Count > 0)
            {
                object[] LastValue = new object[arrFieldName.Length];
                for (int i = 0; i < LastValue.Length; i++)
                {
                    LastValue[i] = null;
                }


                foreach (DataRow dr in SourceTable.Select("", FieldName + " " + strSort))
                {
                    Boolean ok = true;
                    for (int i = 0; i < arrFieldName.Length; i++)
                    {
                        if (LastValue[i] != null && (ColumnEqual(LastValue[i], dr[arrFieldName[i]])))
                        {
                            ok = false;
                        }
                        else
                        {
                            ok = true;
                            break;
                        }
                    }
                    for (int i = 0; i < arrFieldName.Length; i++)
                    {
                        if (ok)
                        {
                            LastValue[i] = dr[arrFieldName[i]];
                        }
                    }
                    if (ok)
                    {
                        DataRow R = dt.NewRow();
                        for (int j = 0; j < arrFieldAdd.Length; j++)
                        {
                            R[arrFieldAdd[j]] = dr[arrFieldAdd[j]];
                        }
                        if (String.IsNullOrEmpty(sField_DK) == false && arrFieldAdd[arrFieldAdd.Length - 1] == "sTen")
                            R[arrFieldAdd[arrFieldAdd.Length - 1]] = LayMoTa_LoaiTaiSan(dr, sField_DK, sField_DK_Value);
                        dt.Rows.Add(R);
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// Lấy mô tả quản lý dự án
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sField_DK"></param>
        /// <param name="sField_DK_Value"></param>
        /// <returns></returns>
        private static String LayMoTa_LoaiTaiSan(DataRow dr, String sField_DK, String sField_DK_Value = "")
        {
            String MoTa = "";
            if (String.IsNullOrEmpty(sField_DK) == false)
            {
                String[] arrDK = sField_DK.Split(',');
                String DK = "";
                for (int i = 0; i < arrDK.Length; i++)
                {
                    DK += arrDK[i] + "=@" + arrDK[i];
                    if (i < arrDK.Length - 1)
                        DK += " AND ";
                }
                if (String.IsNullOrEmpty(DK) == false) DK = " WHERE " + DK;
                String SQL = "SELECT sTen FROM KTCS_NhomTaiSan" + DK;
                SqlCommand cmd = new SqlCommand(SQL);
                for (int i = 0; i < arrDK.Length; i++)
                {
                    if (i < arrDK.Length - 1)
                        cmd.Parameters.AddWithValue("@" + arrDK[i], dr[arrDK[i]]);
                    else
                        cmd.Parameters.AddWithValue("@" + arrDK[i], "");
                }
                if (String.IsNullOrEmpty(sField_DK_Value) == false)
                {
                    cmd.Parameters.RemoveAt(cmd.Parameters.IndexOf("@" + arrDK[0]));
                    cmd.Parameters.AddWithValue("@" + arrDK[0], dr[sField_DK_Value]);
                }
                MoTa = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            return MoTa;
        }
        private static bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if ((A == DBNull.Value || A == null) && (B == DBNull.Value || B == null)) //  both are DBNull.Value
                return true;
            if ((A == DBNull.Value || A == null) || (B == DBNull.Value || B == null)) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
    }
}
