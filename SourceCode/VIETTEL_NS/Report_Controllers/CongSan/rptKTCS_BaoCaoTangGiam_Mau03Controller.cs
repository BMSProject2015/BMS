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
    public class rptKTCS_BaoCaoTangGiam_Mau03Controller : Controller
    {
        //
        // GET: /rptKTCS_BaoCaoTangGiam_Mau03/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_BaoCaoTangGiam_Mau03.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Acion thực hiện truyền các tham số trên form
        /// </summary>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            return RedirectToAction("Index", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Xuất ra báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên tài sản
            String TenLoaiTaiSan = "";
            DataTable dtLoaiTaiSan = dtTenLoaiTaiSan(iID_MaLoaiTaiSan);
            if (dtLoaiTaiSan.Rows.Count > 0)
            {
                TenLoaiTaiSan = dtLoaiTaiSan.Rows[0][0].ToString();
            }
            //Lấy tên đơn vị
            String tendv = "";
            DataTable teN = TenDonVi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            //Lấy ngày tháng năm hiện tại
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            //Hàm đổi tiền từ số sang chữ
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_BaoCaoTangGiam_Mau03");
            LoadData(fr, NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            fr.SetValue("Nam", NamChungTu);
            if (TongHopDonVi == "on")
            {
                fr.SetValue("TenDV", "Tổng hợp đơn vị");
            }
            else
            {
                fr.SetValue("TenDV", tendv);
            }
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("TongCucKiThuat", "TỔNG CỤC KĨ THUẬT");
            if (TongHopDonVi == "on")
            {
                fr.SetValue("iID_MaDonVi", "");
            }
            fr.SetValue("iID_MaDonVi", iID_MaDonVi);
            fr.SetValue("Ngay", NgayThang);
            if (TongHopLTS == "on")
            {
                fr.SetValue("TenLoaiTS", "Tất cả loại tài sản");
            }
            else { fr.SetValue("TenLoaiTS", TenLoaiTaiSan); }
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// DataTable lấy tên loại tài sản
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
        /// DataTable lấy dự liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable rptKTCS_KeKhaiTaiSan(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {

            DataTable dt = null;
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            String DKMaLoaiTaiSan = "";
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCongSan);
            if (TongHopLTS == "on")
            {
                DKMaLoaiTaiSan = "";
            }
            else
            {
                DKMaLoaiTaiSan = " AND iID_MaLoaiTaiSan=@iID_LoaiTaiSan";
            }
            String DKMaPhongBan = "";
            if (TongHopDonVi == "on")
            {
                DKMaPhongBan = "";
            }
            else
            {
                DKMaPhongBan = "AND iID_MaDonVi=@iID_MaDonVi";
            }

            String SQL = string.Format(@"SELECT D.bLaHangCha,C.iID_MaLoaiTaiSan,SUBSTRING(D.iID_MaLoaiTaiSan,1,1)as LoaiTaiSan
                                        ,SUBSTRING(D.iID_MaLoaiTaiSan,1,2) as NhomTaiSan
                                        ,SUBSTRING(D.iID_MaLoaiTaiSan,1,3) as TaiSan
                                        ,D.sTen as TenLoaiTaiSan
                                        ,B.sTenTaiSan as TenChiTiet,sDonViTinh
                                        ,SUM(SoDauNam) as SoDauNam
                                        ,SUM(TangTrongKy) as TangTrongKy
                                        ,SUM(GiamTrongKy) as GiamTrongKy
                                        FROM
                                        ( 
                                                SELECT
                                                sTenTaiSan,iID_MaTaiSan
                                                ,SUM(SoDauNam) as SoDauNam
                                                ,SUM(TangTrongKy) as  TangTrongKy
                                                ,SUM(GiamTrongKy) as  GiamTrongKy
                                        FROM
                                        (
                                                SELECT iID_MaTaiSan,sTenTaiSan,sTinhChat
                                                ,SoDauNam=CASE WHEN sTinhChat='S' THEN SUM(rNguyenGia) ELSE 0 END
                                                ,TangTrongKy=CASE WHEN sTinhChat='T' THEN SUM(rNguyenGia) ELSE 0 END
                                                ,GiamTrongKy=CASE WHEN sTinhChat='G' THEN SUM(rNguyenGia) ELSE 0 END
                                                FROM KTCS_ChungTuChiTiet 
                                                WHERE iTrangThai=1 AND iNamLamViec=@NamLamViec {0}
                                                GROUP BY sTenTaiSan,sTinhChat,iID_MaTaiSan
                                                ) as A
                                                GROUP BY sTenTaiSan,iID_MaTaiSan
                                                ) as B
                                        INNER JOIN
                                               (
                                                SELECT iID_MaTaiSan,iID_MaLoaiTaiSan,sTenTaiSan,sDonViTinh
                                                FROM KTCS_TaiSan 
                                                WHERE 1=1 {1}
                                               ) as C
                                        ON B.iID_MaTaiSan=C.iID_MaTaiSan
                                        INNER JOIN
                                               (
                                               SELECT bLaHangCha,iID_MaLoaiTaiSan,sTen
                                                FROM KTCS_LoaiTaiSan
                                               ) 
                                        as D ON D.iID_MaLoaiTaiSan=C.iID_MaLoaiTaiSan
                                        WHERE LEN(D.iID_MaLoaiTaiSan)=4
                                        GROUP BY C.iID_MaLoaiTaiSan,sTen 
                                        ,B.sTenTaiSan ,sDonViTinh,SUBSTRING(D.iID_MaLoaiTaiSan,1,1),SUBSTRING(D.iID_MaLoaiTaiSan,1,2),SUBSTRING(D.iID_MaLoaiTaiSan,1,3),D.bLaHangCha
                                        HAVING SUM(SoDauNam)!=0 OR SUM(TangTrongKy)!=0 OR SUM(GiamTrongKy)!=0
                                        ORDER BY iID_MaLoaiTaiSan", DKMaPhongBan, DKMaLoaiTaiSan);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamChungTu);
            if (TongHopDonVi != "on")
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (TongHopLTS != "on")
            {
                cmd.Parameters.AddWithValue("@iID_LoaiTaiSan", iID_MaLoaiTaiSan);
            }
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
        /// Load dự liệu ra báo cao
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            DataTable data = rptKTCS_KeKhaiTaiSan(NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable TaiSan;
            TaiSan = SelectDistinct_TaiSan("TaiSan", data, "LoaiTaiSan,NhomTaiSan,TaiSan", "LoaiTaiSan,NhomTaiSan,TaiSan,TenLoaiTaiSan", "LoaiTaiSan,NhomTaiSan");
            fr.AddTable("TaiSan", TaiSan);

            DataTable NhomTaiSan;
            NhomTaiSan = SelectDistinct_TaiSan("NhomTaiSan", TaiSan, "LoaiTaiSan,NhomTaiSan", "LoaiTaiSan,NhomTaiSan,TenLoaiTaiSan", "LoaiTaiSan");
            fr.AddTable("NhomTaiSan", NhomTaiSan);

            DataTable LoaiTaiSan;
            LoaiTaiSan = SelectDistinct_TaiSan("LoaiTaiSan", NhomTaiSan, "LoaiTaiSan", "LoaiTaiSan,TenLoaiTaiSan", "");
            fr.AddTable("LoaiTaiSan", LoaiTaiSan);

            data.Dispose();
        }
        /// <summary>
        /// Action Thực hiện xuất dữ liệu ra file PDF
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {

            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
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
        /// Action thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "SoTheoDoiTaiSanCoDinh_A3";
                }
                else
                {
                    clsResult.FileName = "SoTheoDoiTaiSanCoDinh";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Action Xem báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_03.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
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
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT iID_MaDonVi,sTenDonVi FROM KTCS_ChungTuChiTiet WHERE iTrangThai=1 ORDER BY iID_MaDonVi");
            return dt = Connection.GetDataTable(cmd);
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
                String SQL = "SELECT sTen FROM KTCS_LoaiTaiSan" + DK;
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
        /// <summary>
        /// Lấy danh sách loại tài sản
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_LoaiTS()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT iID_MaLoaiTaiSan,iID_MaLoaiTaiSan+' -  '+sTen AS TenHT FROM 
                                            KTCS_LoaiTaiSan
                                                WHERE iTrangThai=1
                                            ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
