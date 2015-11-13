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

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptQuyetToanNam_SoPheDuyetController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_TongHop_LNS = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet_TongHopDenLNS.xls";
        private const String sFilePath_TongHop_Khoi = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet_TongHopDenKhoiDonVi.xls";
        private const String sFilePath_TongHop_DonVi = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet_TongHopDenDonVi.xls";

        private const String sFilePath_TongHop_PhuLuc_LNS = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet_PhuLuc_TongHopDenLNS.xls";
        private const String sFilePath_TongHop_PhuLuc_Khoi = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet_PhuLuc_TongHopDenKhoiDonVi.xls";
        private const String sFilePath_TongHop_PhuLuc_DonVi = "/Report_ExcelFrom/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet_PhuLuc_TongHopDenDonVi.xls";
        
        public static String NameFile = "";

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptQuyetToanNam_SoPheDuyet(String MaND,String iID_MaPhongBan,String iID_MaDonVi,String iID_MaNamNganSach,String iID_MaNguonNganSach)
        {
            String DKDonVi = "",DKPhongBan="",DK=""; 
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi!="-1")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            //DK ma nam ngan sach
            if(iID_MaNamNganSach=="2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if(iID_MaNamNganSach=="1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            //DK ma nguon ngan sach
            switch (iID_MaNguonNganSach)
            {
                case "QuocPhong":
                    DK += " AND SUBSTRING(sLNS,1,1) IN (1)";
                    break;
                case "NhaNuoc":
                     DK += " AND SUBSTRING(sLNS,1,1) IN (2)";
                    break;
                case "Khac":
                    DK += " AND SUBSTRING(sLNS,1,1) NOT IN (1,2)";
                    break;
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
                 DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
              
            String SQL =
                String.Format(@" SELECT QT.iID_MaDonVi,sTenDonVi,iID_MaKhoiDonVi,sTenKhoiDonVi,iSTT,
 SUBSTRING(QT.sLNS,1,1) as sLNS1
,SUBSTRING(QT.sLNS,1,3) as sLNS3
,SUBSTRING(QT.sLNS,1,5)as sLNS5,QT.sLNS,sMoTa
,rQuyetToan
,rBoSung
,rDonViDeNghi
,rDaCapTien
,rChuaCapTien
 FROM (
SELECT iID_MaDonVi,sTenDonVi,sLNS
,rQuyetToan=SUM(CASE WHEN iThang_Quy <=4 THEN rTuChi ELSE 0 END)
,rBoSung=SUM(CASE WHEN iThang_Quy =5 THEN rTuChi ELSE 0 END)
,SUM(rDonViDeNghi) as rDonViDeNghi
,SUM(rDaCapTien) as rDaCapTien
,SUM(rChuaCapTien) as rChuaCapTien
FROM QTA_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
GROUP BY iID_MaDonVi,sTenDonVi,sLNS
HAVING SUM(CASE WHEN iThang_Quy <=5 THEN rTuChi ELSE 0 END) <>0
OR SUM(rDonViDeNghi)<>0
OR SUM(rDaCapTien)<>0
OR SUM(rChuaCapTien)<>0
) QT
INNER JOIN 
--Lây tên khối đơn vị
(SELECT iID_MaDonVi,iID_MaKhoiDonVi,DM.sTen as sTenKhoiDonVi,iSTT FROM(
SELECT iID_MaDonVi,iID_MaKhoiDonVi FROM NS_DonVi
WHERE iNamLamViec_DonVi=@iNamLamViec) DV
INNER JOIN (SELECT iID_MaDanhMuc,sTen,iSTT FROM DC_DanhMuc 
		WHERE iID_MaLoaiDanhMuc='bc5f9bb8-1dba-40fc-95b6-1098e5b0e68f') DM
		ON dv.iID_MaKhoiDonVi=DM.iID_MaDanhMuc) Khoi
ON QT.iID_MaDonVi=Khoi.iID_MaDonVi

INNER JOIN 
(SELECT sLNS,sMoTa FROM NS_MucLucNganSach
WHERE iTrangThai=1 AND LEN(sLNS)=7 AND sL='') ML
ON QT.sLNS=ML.sLNS
", DKPhongBan,DKDonVi,DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iLoai = Request.Form[ParentID + "_iLoai"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            String iID_MaNguonNganSach = Request.Form[ParentID + "_iID_MaNguonNganSach"];
            String iID_MaMauBaoCao = Request.Form[ParentID + "_iID_MaMauBaoCao"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iLoai"] = iLoai;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaNguonNganSach"] = iID_MaNguonNganSach;
            ViewData["iID_MaMauBaoCao"] = iID_MaMauBaoCao;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanNhanh/rptQuyetToanNam_SoPheDuyet.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaPhongBan, String iID_MaDonVi, String iID_MaNamNganSach, String iLoai, String iID_MaNguonNganSach, String iID_MaMauBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToanNam_SoPheDuyet");

            LoadData(fr, MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach, iID_MaNguonNganSach);
            String Nam = ReportModels.LayNamLamViec(MaND);

            //lay ten nam ngan sach
            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
                NamNganSach = "QUYẾT TOÁN NẮM TRƯỚC";
            else if (iID_MaNamNganSach == "2")
                NamNganSach = "QUYẾT TOÁN NĂM NAY";
            else
            {
                NamNganSach = "TỔNG HỢP";
            }
            //Ten mâu bieu
            String TenBieu = "",NguonNganSach="";
            if (iID_MaNguonNganSach =="QuocPhong")
            {
                if (iLoai == "KhoiDonVi") TenBieu = "Phụ lục số 2";
                else if (iLoai == "DonVi") TenBieu = "Phụ lục số 2a";
                NguonNganSach = "NGÂN SÁCH QUỐC PHÒNG";
            }
            else if (iID_MaNguonNganSach == "NhaNuoc")
            {

                if (iLoai == "KhoiDonVi") TenBieu = "Phụ lục số 3";
                else if (iLoai == "DonVi") TenBieu = "Phụ lục số 3a";
                NguonNganSach = "NGÂN SÁCH NHÀ NƯỚC GIAO";
            }
            else if (iID_MaNguonNganSach == "Khac")
            {

                if (iLoai == "KhoiDonVi") TenBieu = "Phụ lục số 4";
                else if (iLoai == "DonVi") TenBieu = "Phụ lục số 4a";
                NguonNganSach = "KINH PHÍ KHÁC";
            }
            else
            {
                NguonNganSach = "NGÂN SÁCH";
            }
            //Ten BQL
            String BQuanLy = "";
            if (iID_MaPhongBan != "-1")
                BQuanLy = " BQL - " + iID_MaPhongBan;
            //Ten Don vi
            String sTenDonVi = "";
            if (iID_MaDonVi != "-1")
                sTenDonVi = "Đơn vị : " + DonViModels.Get_TenDonVi(iID_MaDonVi);
            String TenPB = "";
            if (iID_MaPhongBan != "-1")
                TenPB = " B - " + iID_MaPhongBan;
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("NamNganSach", NamNganSach);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("TenBieu", TenBieu);
            fr.SetValue("NguonNganSach", NguonNganSach);
            fr.SetValue("BQuanLy", BQuanLy);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaPhongBan, String iID_MaDonVi,String iID_MaNamNganSach,String iID_MaNguonNganSach)
        {
            DataRow r;
            DataTable data = rptQuyetToanNam_SoPheDuyet(MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach, iID_MaNguonNganSach);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtKhoiDonVi = HamChung.SelectDistinct("KhoiDV", data, "sLNS1,sLNS3,sLNS5,sLNS,iID_MaKhoiDonVi", "iID_MaKhoiDonVi,sTenKhoiDonVi,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa,iSTT");
            DataView dv = dtKhoiDonVi.DefaultView;
            dv.Sort = "iSTT";
            dtKhoiDonVi = dv.ToTable();

            DataTable dtLNS = HamChung.SelectDistinct("dtLNS", dtKhoiDonVi, "sLNS1,sLNS3,sLNS5,sLNS", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");

           
           
            DataTable dtLNS5 = HamChung.SelectDistinct("dtLNS5", dtLNS, "sLNS1,sLNS3,sLNS5", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");
            for (int i = 0; i < dtLNS5.Rows.Count; i++)
            {
                r = dtLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
           
            DataTable dtLNS3 = HamChung.SelectDistinct("dtLNS3", dtLNS5, "sLNS1,sLNS3", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");
            for (int i = 0; i < dtLNS3.Rows.Count; i++)
            {
                r = dtLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }

            DataTable dtLNS1 = HamChung.SelectDistinct("dtLNS1", dtLNS3, "sLNS1", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa");

            for (int i = 0; i < dtLNS1.Rows.Count; i++)
            {
                r = dtLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }
            fr.AddTable("KhoiDV", dtKhoiDonVi);
            fr.AddTable("dtLNS", dtLNS);
            fr.AddTable("dtLNS5", dtLNS5);
            fr.AddTable("dtLNS3", dtLNS3);
            fr.AddTable("dtLNS1", dtLNS1);
            dtKhoiDonVi.Dispose();
            dtLNS.Dispose();
            dtLNS5.Dispose();
            dtLNS3.Dispose();
            dtLNS1.Dispose();
            data.Dispose();
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}",sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaPhongBan, String iID_MaDonVi,String iID_MaNamNganSach,String iLoai,String iID_MaNguonNganSach,String iID_MaMauBaoCao)
        {
            HamChung.Language();
            String sDuongDan = "";
            if(iID_MaMauBaoCao=="PhuLuc")
            {
                if (iLoai == "DonVi")
                    sDuongDan = sFilePath_TongHop_PhuLuc_DonVi;
                else if (iLoai == "KhoiDonVi")
                    sDuongDan = sFilePath_TongHop_PhuLuc_Khoi;
                else
                    sDuongDan = sFilePath_TongHop_PhuLuc_LNS;
            }
            else
            {
                if (iLoai == "DonVi")
                    sDuongDan = sFilePath_TongHop_DonVi;
                else if (iLoai == "KhoiDonVi")
                    sDuongDan = sFilePath_TongHop_Khoi;
                else
                    sDuongDan = sFilePath_TongHop_LNS;
            }
          

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach, iLoai, iID_MaNguonNganSach, iID_MaMauBaoCao);
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

        public clsExcelResult ExportToExcel(String MaND, String iID_MaPhongBan, String iID_MaDonVi, String iID_MaNamNganSach, String iLoai, String iID_MaNguonNganSach, String iID_MaMauBaoCao)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iID_MaMauBaoCao == "PhuLuc")
            {
                if (iLoai == "DonVi")
                    sDuongDan = sFilePath_TongHop_PhuLuc_DonVi;
                else if (iLoai == "KhoiDonVi")
                    sDuongDan = sFilePath_TongHop_PhuLuc_Khoi;
                else
                    sDuongDan = sFilePath_TongHop_PhuLuc_LNS;
            }
            else
            {
                if (iLoai == "DonVi")
                    sDuongDan = sFilePath_TongHop_DonVi;
                else if (iLoai == "KhoiDonVi")
                    sDuongDan = sFilePath_TongHop_Khoi;
                else
                    sDuongDan = sFilePath_TongHop_LNS;
            }

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iID_MaPhongBan, iID_MaDonVi, iID_MaNamNganSach, iLoai, iID_MaNguonNganSach, iID_MaMauBaoCao);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCaoSoLieuKetLuanQuyetToan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi, String MaND)
        {
            return Json(obj_DonVi(ParentID, iID_MaPhongBan, iID_MaDonVi,MaND), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iID_MaPhongBan, String iID_MaDonVi,String MaND)
        {
            String input = "";
            DataTable dt = DonViModels.DanhSach_DonVi_QuyetToan_PhongBan(iID_MaPhongBan, MaND);
            SelectOptionList slDonvi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return input;
        }

    }
}

