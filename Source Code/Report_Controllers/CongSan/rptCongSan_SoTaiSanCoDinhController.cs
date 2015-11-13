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
using System.Globalization;

namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptCongSan_SoTaiSanCoDinhController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh.xls";
            }
            ViewData["path"] = "~/Report_Views/CongSan/rptCongSan_SoTaiSanCoDinh.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["srcFile"] = NameFile;
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String ThangChungTu = Request.Form[ParentID + "_ThangChungTu"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);

            ViewData["PageLoad"] = "1";
            ViewData["NamChungTu"] = NamChungTu;
            ViewData["ThangChungTu"] = ThangChungTu;
            ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TongHopDonVi"] = TongHopDonVi;
            ViewData["TongHopLTS"] = TongHopLTS;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/CongSan/rptCongSan_SoTaiSanCoDinh.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// So_Tai_San_Co_Dinh
        /// </summary>
        /// <param name="TuNam"></param>
        /// <param name="DenNam"></param>
        /// <param name="MaDV"></param>
        /// <param name="MaLTS"></param>
        /// <returns></returns>
        public DataTable So_Tai_San_Co_Dinh(String NamChungTu, String ThangChungTu,String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            DataTable dt = null;
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            String DKDonVi = "KTCS_TaiSan.iID_MaDonVi='-111'", DKNhomTaiSan = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = KTCS_ReportModel.ListDonVi();
                if (dtDonVi.Rows.Count > 0)
                {
                    DKDonVi = "";
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DKDonVi += "KTCS_TaiSan.iID_MaDonVi = '" + dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + "'";
                        if (i < dtDonVi.Rows.Count - 1)
                            DKDonVi += " OR ";

                    }
                    dtDonVi.Dispose();
                }
            }
            else
            {
                DKDonVi = "";
                DKDonVi = "  KTCS_TaiSan.iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'";
            }
            if (TongHopLTS == "on")
            {
                DataTable dtNhomTaiSan = KTCS_ReportModel.DT_LoaiTS();
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
SELECT iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,a.iID_MaTaiSan,sTenTaiSan,sLyDo,sDonViTinh,iNamSX,YEAR(dNgayDuaVaoKhauHao) as iNamSD
,SUM(rSoLuong) as rSoLuong,SUM(rSoTien) as rSoTien ,SUM(rSoLuong_1) as rSoLuong_1,SUM(rSoTien_1) as rSoTien_1
FROM(
SELECT
iID_MaChungTuChiTiet=(SELECT TOP 1 iID_MaChungTuChiTiet FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iNgayCT=(SELECT TOP 1 iNgayCT FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iThangCT=(SELECT TOP 1 iThangCT FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
sSoChungTuGhiSo=(SELECT TOP 1 sSoChungTuGhiSo FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iNgay=(SELECT TOP 1 iNgay FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iThang=(SELECT TOP 1 iThang FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
sSoChungTuChiTiet=(SELECT TOP 1 sSoChungTuChiTiet FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
KTCS_TaiSan.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan,KTCS_TaiSanChiTiet.dNgayDuaVaoKhauHao,KTCS_TaiSanChiTiet.iNamSX
,sDonViTinh
,SUM(CASE WHEN sTinhChat='1' OR sTinhChat='T' THEN rSoLuong ELSE 0 END) as rSoLuong
, rSoTien=(SELECT rNguyenGia-rSoTienGiam FROM KTCS_HachToanTaiSan WHERE KTCS_ChungTuChiTiet.iID_MaTaiSan=KTCS_HachToanTaiSan.iID_MaTaiSan  AND iNamLamViec=@iNamLamViec)
,sLyDo=(SELECT TOP 1 sLyDo FROM KTCS_ChungTuChiTiet as CT WHERE sTinhChat='G' AND CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC)
,rSoLuong_1=0
,rSoTien_1=0
FROM KTCS_ChungTuChiTiet
INNER JOIN KTCS_TaiSan
on KTCS_ChungTuChiTiet.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
INNER JOIN KTCS_TaiSanChiTiet
ON KTCS_TaiSanChiTiet.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
WHERE (sTinhChat='1' OR sTinhChat='2' OR sTinhChat='T')  AND  KTCS_TaiSan.iNamLamViec=@iNamLamViec AND KTCS_TaiSan.iTrangThai=1 AND KTCS_ChungTuChiTiet.iTrangThai=1 AND ({0})
GROUP BY iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,KTCS_TaiSan.iID_MaTaiSan,KTCS_ChungTuChiTiet.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan,sDonViTinh,sLyDo,KTCS_TaiSanChiTiet.dNgayDuaVaoKhauHao,KTCS_TaiSanChiTiet.iNamSX

UNION

SELECT
iID_MaChungTuChiTiet=(SELECT TOP 1 iID_MaChungTuChiTiet FROM KTCS_ChungTuChiTiet as CT WHERE   CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iNgayCT=(SELECT TOP 1 iNgayCT FROM KTCS_ChungTuChiTiet as CT WHERE   CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iThangCT=(SELECT TOP 1 iThangCT FROM KTCS_ChungTuChiTiet as CT WHERE CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
sSoChungTuGhiSo=(SELECT TOP 1 sSoChungTuGhiSo FROM KTCS_ChungTuChiTiet as CT WHERE   CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iNgay=(SELECT TOP 1 iNgay FROM KTCS_ChungTuChiTiet as CT WHERE   CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
iThang=(SELECT TOP 1 iThang FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
sSoChungTuChiTiet=(SELECT TOP 1 sSoChungTuChiTiet FROM KTCS_ChungTuChiTiet as CT WHERE  CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC),
KTCS_TaiSan.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan,KTCS_TaiSanChiTiet.dNgayDuaVaoKhauHao,KTCS_TaiSanChiTiet.iNamSX
,sDonViTinh
,rSoLuong=0
,rSoTien=0
,sLyDo=(SELECT TOP 1 sLyDo FROM KTCS_ChungTuChiTiet as CT WHERE sTinhChat='G' AND CT.iID_MaTaiSan=KTCS_ChungTuChiTiet.iID_MaTaiSan ORDER BY iThangCT DESC,iNgayCT DESC)
,SUM(CASE WHEN sTinhChat='G' THEN rSoLuong ELSE 0 END) as rSoLuong_1
, rSoTien_1=(SELECT rNguyenGia-rSoTienGiam FROM KTCS_HachToanTaiSan WHERE sTinhChat='G' AND KTCS_ChungTuChiTiet.iID_MaTaiSan=KTCS_HachToanTaiSan.iID_MaTaiSan  AND iNamLamViec=@iNamLamViec)
FROM KTCS_ChungTuChiTiet
INNER JOIN KTCS_TaiSan
on KTCS_ChungTuChiTiet.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
INNER JOIN KTCS_TaiSanChiTiet
ON KTCS_TaiSanChiTiet.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
WHERE sTinhChat='G'  AND KTCS_TaiSan.iNamLamViec=@iNamLamViec AND KTCS_TaiSan.iTrangThai=1 AND KTCS_ChungTuChiTiet.iTrangThai=1 AND ({0})
GROUP BY iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,KTCS_TaiSan.iID_MaTaiSan ,KTCS_ChungTuChiTiet.iID_MaTaiSan,KTCS_TaiSan.sTenTaiSan,sDonViTinh,sLyDo,KTCS_TaiSanChiTiet.dNgayDuaVaoKhauHao,KTCS_TaiSanChiTiet.iNamSX,sTinhChat) as a
INNER JOIN(SELECT iID_MaTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 AND ( {1})) as B
                                          ON a.iID_MaTaiSan=B.iID_MaTaiSan 
GROUP BY iID_MaChungTuChiTiet,iNgayCT,iThangCT,sSoChungTuGhiSo,iNgay,iThang,sSoChungTuChiTiet,a.iID_MaTaiSan,sTenTaiSan,sLyDo,sDonViTinh,iNamSX,YEAR(dNgayDuaVaoKhauHao)
", DKDonVi, DKNhomTaiSan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            //cmd.Parameters.AddWithValue("@iThangLamViec", ThangChungTu);
            dt = Connection.GetDataTable(cmd);


            //datatable tính hao mòn

            String SQLHM = @"SELECT iID_MaTaiSan,rSoTienKhauHao,rSoTienKhauHao_LuyKe,iSoNamKhauHao
                          FROM KTCS_KhauHaoHangNam WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.CommandText = SQLHM;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);

            DataTable dtHM = Connection.GetDataTable(cmd);
            // ghep dtHM vao dt

            dt.Columns.Add("rSoTienKhauHao", typeof(decimal));
            dt.Columns.Add("rSoTienKhauHao_LuyKe", typeof(decimal));
            dt.Columns.Add("iSoNamKhauHao", typeof(decimal));

            foreach (DataRow r in dt.Rows)
            {
                foreach (DataRow rHM in dtHM.Rows)
                {
                    if (r["iID_MaTaiSan"].ToString() == rHM["iID_MaTaiSan"].ToString())
                    {
                        r["rSoTienKhauHao"] = Convert.ToDecimal(rHM["rSoTienKhauHao"].ToString());
                        r["rSoTienKhauHao_LuyKe"] = Convert.ToDecimal(rHM["rSoTienKhauHao_LuyKe"].ToString());
                        r["iSoNamKhauHao"] = Convert.ToDecimal(rHM["iSoNamKhauHao"].ToString());
                        break;
                    }
                }
            }

            cmd.Dispose();
            return dt;
        }

        public ExcelFile CreateReport(String path, String NamChungTu,String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
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
            fr = ReportModels.LayThongTinChuKy(fr, "rptCongSan_SoTaiSanCoDinh");
            LoadData(fr, NamChungTu,ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
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
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Nam", NamChungTu);
            fr.SetValue("Thang", ThangChungTu);
            fr.SetValue("TongCucKiThuat", "TỔNG CỤC KĨ THUẬT");
            fr.SetValue("Ngay", NgayThang);
            if (TongHopLTS == "on")
            {
                fr.SetValue("TaiSan", "Tất cả loại tài sản");
            }
            else { fr.SetValue("TaiSan", TenLoaiTaiSan); }
            fr.Run(Result);
            return Result;

        }
          public DataTable dtTenLoaiTaiSan(String iID_MaLoaiTaiSan)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan=@iID_MaLoaiTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
            return dt = Connection.GetDataTable(cmd);
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
        private void LoadData(FlexCelReport fr, String NamChungTu,String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            DataTable data = So_Tai_San_Co_Dinh(NamChungTu,ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi,TongHopDonVi,TongHopLTS,KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
        }
      
        /// <summary>
        /// Action thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String ThangChungTu,String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu,ThangChungTu, iID_MaLoaiTaiSan, iID_MaDonVi,TongHopDonVi,TongHopLTS,KhoGiay);
            
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "SoTaiSanCoDinh_A3";
                }
                else
                {
                    clsResult.FileName = "SoTaiSanCoDinh";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Action Xem báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamChungTu,String ThangChungTu, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh_A3.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_SoTaiSanCoDinh.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, ThangChungTu,iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
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

    




  