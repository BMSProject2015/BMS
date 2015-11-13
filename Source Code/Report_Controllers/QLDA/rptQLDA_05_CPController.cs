using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using VIETTEL.Controllers;
using System.IO;
namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_05_CPController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_05_CP.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_05_CP.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_viiID_MaDotCapPhat"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["MaTien"] = MaTien;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_05_CP.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dt_rptQLDA_05_CP(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            String dNgayLap = "01/01/2000";
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            dNgayLap = iID_MaDotCapPhat;
            String dNam = "";
            if (dNgayLap != "01/01/2000")
                dNam = dNgayLap.Substring(6, 4);
           // dtDotCapPhat.Dispose();
           
            String NamLamViec = "2000";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            if (dtCauHinh.Rows.Count > 0)
            {
                NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String DK_TongDauTu = "";
            String DK_KHV = "";
            String DK_CP = "";
            String DK_LoaiNgoaiTe_CP = "";
            String DK_LoaiNgoaiTe_TongDauTu = "";
            String DK_LoaiNgoaiTe_KHV = "";
            if (MaTien == "0")
            {
                DK_TongDauTu = "rSoTien/1000000";
                DK_KHV = "rSoTienDieuChinh/1000000+rSoTienDauNam/1000000";
                DK_CP = "rDeNghiPheDuyetDonViThuHuong/1000000";
               
                
            }
            else
            {
                DK_TongDauTu = "rNgoaiTe_SoTien";
                DK_KHV = "rNgoaiTe_SoTienDieuChinh+rNgoaiTe_SoTienDauNam";
                DK_CP = "rNgoaiTe_DeNghiPheDuyetDonViThuHuong";
                DK_LoaiNgoaiTe_CP = " iID_MaNgoaiTe_DeNghiPheDuyetDonViThuHuong=@iID_MaNgoaiTe AND ";
                DK_LoaiNgoaiTe_TongDauTu = "iID_MaNgoaiTe_SoTien=@iID_MaNgoaiTe AND ";
                DK_LoaiNgoaiTe_KHV = "(iID_MaNgoaiTe_SoTienDieuChinh=@iID_MaNgoaiTe OR iID_MaNgoaiTe_SoTienDauNam=@iID_MaNgoaiTe) AND ";
            }
            String SQL = String.Format(@"SELECT NguonNS,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
,sTenDuAn,sTienDo=''
,SUM(rTongDauTu) as rTongDauTu
,SUM(KHVUngTruoc) as KHVUngTruoc
,SUM(KHVNamNay) as KHVNamNay
,SUM(KHVLuyKe) as KHVLuyKe
,SUM(CPUngTruoc_NamNay) as CPUngTruoc_NamNay
,SUM(CPUngTruoc_LuyKe) as CPUngTruoc_LuyKe
,SUM(CP_NamNay) as CP_NamNay
,SUM(CP_LuyKe) as CP_LuyKe
 FROM (
SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
,SUBSTRING(sTenDuAn,19,100000) as sTenDuAn
,SUM({0}) as rTongDauTu
,KHVUngTruoc=0
,KHVNamNay=0
,KHVLuyKe=0
,CPUngTruoc_NamNay=0
,CPUngTruoc_LuyKe=0
,CP_NamNay=0
,CP_LuyKe=0
FROM QLDA_TongDuToan
WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec AND {3} dNgayPheDuyet<=@dNgayLap AND sHangMucChiTiet<>''
GROUP BY SUBSTRING(sLNS,1,1) ,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
,SUBSTRING(sTenDuAn,19,100000)
HAVING SUM({0})<>0

UNION

SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
,SUBSTRING(sTenDuAn,19,100000) as sTenDuAn
,rTongDauTu=0
,KHVUngTruoc=SUM(CASE WHEN (iNamLamViec<=@iNamLamViec AND iLoaiKeHoachVon=2) THEN ({1}) ELSE 0 END)
,KHVNamNay=SUM(CASE WHEN (iNamLamViec=@iNamLamViec AND iLoaiKeHoachVon=1) THEN ({1}) ELSE 0 END)
,KHVLuyKe=SUM(CASE WHEN (iNamLamViec<=@iNamLamViec AND iLoaiKeHoachVon=1) THEN ({1}) ELSE 0 END)
,CPUngTruoc_NamNay=0
,CPUngTruoc_LuyKe=0
,CP_NamNay=0
,CP_LuyKe=0
FROM QLDA_KeHoachVon
WHERE iTrangThai=1 AND {4} dNgayKeHoachVon<=@dNgayLap AND sHangMucChiTiet<>''
GROUP BY SUBSTRING(sLNS,1,1) ,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
,SUBSTRING(sTenDuAn,19,100000)
HAVING 
SUM(CASE WHEN (iNamLamViec<=@iNamLamViec AND iLoaiKeHoachVon=2) THEN ({1}) ELSE 0 END)<>0
OR SUM(CASE WHEN (iNamLamViec<=@iNamLamViec AND iLoaiKeHoachVon=1) THEN ({1}) ELSE 0 END)<>0

UNION

SELECT  SUBSTRING(sLNS,1,1) as NguonNS,sLNS,CP.iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,
 sTenDuAn 
 ,rTongDauTu=0
 ,KHVUngTruoc=0
,KHVNamNay=0
,KHVLuyKe=0
,CPUngTruoc_NamNay
,CPUngTruoc_LuyKe
,CP_NamNay
,CP_LuyKe
 FROM (
SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,iID_MaDanhMucDuAn
,CPUngTruoc_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=2 THEN {2} ELSE 0 END )
,CPUngTruoc_LuyKe=SUM(CASE WHEN iNamLamViec<=@iNamLamViec AND iID_MaLoaiKeHoachVon=2 THEN {2} ELSE 0 END )
,CP_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {2} ELSE 0 END )
,CP_LuyKe=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {2} ELSE 0 END )
FROM QLDA_CapPhat
WHERE iTrangThai=1 AND {5} iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE iTrangThai=1 AND dNgayLap<=@dNgayLap)
GROUP BY SUBSTRING(sLNS,1,1) ,sLNS,iID_MaDanhMucDuAn) as CP
 INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                                                                                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTienDo 
                                                                                FROM QLDA_DanhMucDuAn 
                                                                                WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as DM
                                        ON CP.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn) as ChiTiet
GROUP BY    NguonNS,sLNS,iID_MaDanhMucDuAn,sDeAN,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
,sTenDuAn                                      ", DK_TongDauTu,DK_KHV,DK_CP,DK_LoaiNgoaiTe_TongDauTu,DK_LoaiNgoaiTe_KHV,DK_LoaiNgoaiTe_CP);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgayLap));
            if (MaTien != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public ActionResult ViewPDF(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDotCapPhat, MaND,MaTien);
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
        public ExcelFile CreateReport(String path, String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            String dNgayLap = "01/01/2000";
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            //dtDotCapPhat.Dispose();
            dNgayLap = iID_MaDotCapPhat;
            String DotCapPhat = " Đến ngày " + dNgayLap.Substring(0, 2) + " tháng " + dNgayLap.Substring(3, 2) + " năm " + dNgayLap.Substring(6, 4);
            String nam = dNgayLap.Substring(6, 4);
            DataTable dtDVT = QLDA_ReportModel.dt_LoaiTien_CP_03(dNgayLap, MaND);
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                    break;
                }

            }
            dtDVT.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_05_CP");
            LoadData(fr, iID_MaDotCapPhat, MaND,MaTien);
            fr.SetValue("DVT", DVT);
            fr.SetValue("DotCapPhat", DotCapPhat);
            fr.SetValue("Nam", nam);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iID_MaDotCapPhat, String MaND,String MaTien)
        {

            DataTable data = dt_rptQLDA_05_CP(iID_MaDotCapPhat, MaND, MaTien);
            data.TableName = "ChiTiet";
            //Hạng mục chi tiết
            DataTable dtHangMucChiTiet = HamChung.SelectDistinct_QLDA("HMChiTiet", data, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sTienDo");
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", dtHangMucChiTiet, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHangMucCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DATP", dtCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DuAn", dtDuAnThanhPhan, "NguonNS,sLNS,sDeAn,sDuAn", "NguonNS,sLNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sLNS,sDeAn", "NguonNS,sLNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //sLNS
            DataTable dtLNS = HamChung.SelectDistinct("sLNS", dtDeAn, "NguonNS,sLNS", "NguonNS,sLNS,sTenDuAn", "", "");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");

            //Thêm tên Loại ngân sách của dtLNS
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                DataRow r = dtLNS.Rows[i];
                String SQL = "SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND sLNS=@sLNS";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                String sMoTa = "";
                sMoTa = Connection.GetValueString(cmd, "");
                r["sTenDuAn"] = sMoTa;
            }

            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CongTrinh", dtCongTrinh);
            fr.AddTable("DATP", dtDuAnThanhPhan);
            fr.AddTable("DuAn", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("sLNS", dtLNS);
            fr.AddTable("Nguon", dtNguon);
            dtDeAn.Dispose();
            dtDuAn.Dispose();
            dtDuAnThanhPhan.Dispose();
            dtCongTrinh.Dispose();
            dtNguon.Dispose();
            data.Dispose();
        }
        public JsonResult ds_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            return Json(obj_QLDA(ParentID, iID_MaDotCapPhat, MaND, MaTien), JsonRequestBehavior.AllowGet);
        }
        public String obj_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            String dNgayLap = "01/01/2000";
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            {
                if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
                {
                    dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
                    break;
                }
            }
            dtDotCapPhat.Dispose();
            DataTable dtNgoaiTe = QLDA_ReportModel.dt_LoaiTien_CP_03(dNgayLap, MaND);
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTenNgoaiTe");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            return NgoaiTe;
        }
    }
}
