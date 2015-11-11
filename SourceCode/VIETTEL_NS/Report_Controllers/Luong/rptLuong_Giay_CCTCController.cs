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

namespace VIETTEL.Report_Controllers.Luong
{
    public class rptLuong_Giay_CCTCController : Controller
    {
        // Edit: Thương
        // GET: /rptLuong_Giay_CCTC/
        public string sViewPath = "~/Report_Views/";
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_Giay_CCTC.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            ViewData["PageLoad"] = 1;
            String MaDV = Convert.ToString(Request.Form["iID_MaDonVi"]);
            String MaCB = Convert.ToString(Request.Form["iID_MaCanBo"]);
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThangLuong"]);
            String Nam = Convert.ToString(Request.Form[ParentID + "_iNamLuong"]);
            String TrangThai = Convert.ToString(Request.Form[ParentID + "_iTrangThai"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_divPages"]);
            ViewData["iTrangThai"] = TrangThai;
            ViewData["iNamLuong"] = Nam;
            ViewData["iThangLuong"] = Thang;
            ViewData["iMaDV"] = MaDV;
            ViewData["iMaCB"] = MaCB;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/Luong/rptLuong_Giay_CCTC.aspx";
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_GiayCungCapTaiChinh.xls";
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_GiayCungCapTaiChinh_A3.xls";
                    break;
            }
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DonVi"></param>
        /// <param name="Duyet"></param>
        /// <returns></returns>
        public DataTable Giay_CCTC(String NamLuong, String ThangLuong, String DonVi, String MaCB, String Duyet)
        {
            String DKDuyet = Duyet.Equals("0") ? "" : " AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
            String DKDonvi = "AND L.iID_MaDonVi IN(";
            String[] arrDonvi = DonVi.Split(',');
            for (int i = 0; i < arrDonvi.Length; i++)
            {
                DKDonvi += "@iID_MaDonVi" + i;
                if (i < arrDonvi.Length - 1)
                    DKDonvi += " , ";
            }
            DKDonvi += ")";
            String DKMaCB = "AND L.iID_MaCanBo IN(";
            String[] arrMaCB = MaCB.Split(',');
            for (int j = 0; j < arrMaCB.Length; j++)
            {
                DKMaCB += "@iID_MaCanBo" + j;
                if (j < arrMaCB.Length - 1)
                    DKMaCB += ",";
            }
            DKMaCB += ")";
            String SQL = String.Format(@"SELECT L.sHoDem_CanBo +' '+L.sTen_CanBo as TenCB
	                                          ,SUBSTRING(CONVERT(varchar(10),L.dNgayNhapNgu_CanBo, 5), 4, 5) as NN
	                                          ,SUBSTRING(CONVERT(varchar(10),L.dNgayXuatNgu_CanBo, 5), 4, 5) AS XN
	                                          ,SUBSTRING(CONVERT(varchar(10),L.dNgayTaiNgu_CanBo, 5), 4, 5) AS TN
                                              ,L.sSoSoLuong_CanBo AS SSL_CB
                                              ,SUM(L.rPhuCap_ChucVu) AS PCCV
                                              ,SUM(CASE WHEN L.iID_MaNgachLuong_CanBo<>3 THEN L.rPhuCap_ThamNien ELSE L.rPhuCap_AnNinhQuocPhong END) AS PCThN
                                              ,SUM(L.rPhuCap_VuotKhung)+SUM(L.rPhuCap_BaoLuu) AS VKBL
	                                          ,SUM(L.rPhuCap_TrachNhiem) AS PCTrN
	                                          ,SUM(L.rPhuCap_DacBiet+L.rPhuCap_CongVu) AS PCDBCV
	                                          ,SUM(L.rPhuCap_KhuVuc) AS PCKV
	                                          ,SUM(L.rPhuCap_Khac) AS PCKhac
	                                          ,SUM(L.rTienAn1Ngay) AS TienAn
	                                          ,SUM(L.rLuongCoBan) as LCB
	                                          ,BL.sTenBacLuong AS CB
                                              ,CV=''
                                              ,T_N=(select SUM(case when TS.sNoiDung in(N'Nước uống',N'Tem thư') then convert(decimal, TS.sThamSo) else '0' end) ThamSo
		                                            from L_DanhMucThamSo as TS )
	                                          ,BG_Gao=(select SUM(case when TS.sNoiDung=N'Bù giá gạo' then convert(decimal, TS.sThamSo) else '0' end) ThamSo
		                                            from L_DanhMucThamSo as TS )  
	                                          ,DV_D=(select TS.sThamSo
		                                            from L_DanhMucThamSo as TS where TS.sNoiDung=N'Đơn vị làm lương' )  
	                                          ,DV_T=(select TS.sThamSo
		                                            from L_DanhMucThamSo as TS where TS.sNoiDung=N'Đơn vị cấp trên' ) 
                                        FROM L_BangLuongChiTiet AS L
                                        INNER JOIN L_DanhMucBacLuong AS BL
                                        ON L.iID_MaBacLuong_CanBo=BL.iID_MaBacLuong AND L.iID_MaNgachLuong_CanBo=BL.iID_MaNgachLuong 
                                        WHERE L.iTrangThai=1
                                          AND L.iNamBangLuong=@iNamBangLuong
                                          AND L.iThangBangLuong=@iThangBangLuong
                                          {0}
                                          {1}                                          
                                          {2}
                                        GROUP BY L.sHoDem_CanBo +' '+L.sTen_CanBo
	                                            ,L.dNgayNhapNgu_CanBo
	                                            ,L.dNgayXuatNgu_CanBo
	                                            ,L.dNgayTaiNgu_CanBo
	                                            ,L.sSoSoLuong_CanBo	  
	                                            ,BL.sTenBacLuong", DKDuyet, DKDonvi, DKMaCB);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", NamLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", ThangLuong);
            for (int z = 0; z < arrDonvi.Length; z++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + z, arrDonvi[z]);
            }
            for (int t = 0; t < arrMaCB.Length; t++)
            {
                cmd.Parameters.AddWithValue("@iID_MaCanBo" + t, arrMaCB[t]);
            }
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dtKQ = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtKQ;
        }
        /// <summary>
        /// Đổ dữ liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLuong">Năm</param>
        /// <param name="ThangLuong">Tháng</param>
        /// <param name="DonVi">Mã đơn vị</param>
        /// <param name="MaCB">Mã đơn vị</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        private void LoadData(FlexCelReport fr, String NamLuong, String ThangLuong, String DonVi, String MaCB, String Duyet)
        {
            DataTable dt = Giay_CCTC(NamLuong, ThangLuong, DonVi, MaCB, Duyet);
            dt.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", dt);
            dt.Dispose();
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DonVi"></param>
        /// <param name="MaCB"></param>
        /// <param name="Duyet"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamLuong, String ThangLuong, String DonVi, String MaCB, String Duyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            //Thêm chữ ký vào báo cáo

            fr = ReportModels.LayThongTinChuKy(fr, "rptLuong_Giay_CCTC");
            LoadData(fr, NamLuong, ThangLuong, DonVi, MaCB, Duyet);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Thang", DateTime.Now.Month.ToString());
            fr.SetValue("Nam", DateTime.Now.Year.ToString());
            fr.SetValue("ThangNext", (DateTime.Now.Month + 1).ToString());
            fr.SetValue("NamNext", DateTime.Now.Month.Equals(12) ? (DateTime.Now.Year + 1).ToString() : DateTime.Now.Year.ToString());
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLuong">Năm</param>
        /// <param name="ThangLuong">Tháng</param>
        /// <param name="DonVi">Mã đơn vị</param>
        /// <param name="MaCB">Mã cán bộ</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <param name="KhoGiay">Khổ giấy in</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String NamLuong, String ThangLuong, String DonVi, String MaCB, String Duyet, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_GiayCungCapTaiChinh.xls";
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_GiayCungCapTaiChinh_A3.xls";
                    break;
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLuong, ThangLuong, DonVi, MaCB, Duyet);
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
        /// Xuất báo cáo ra file Excel
        /// </summary>
        /// <param name="NamLuong">Năm</param>
        /// <param name="ThangLuong">Tháng</param>
        /// <param name="DonVi">Mã đơn vị</param>
        /// <param name="MaCB">Mã cán bộ</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <param name="KhoGiay">Khổ giấy in</param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamLuong, String ThangLuong, String DonVi, String MaCB, String Duyet, String KhoGiay)
        {
            String sFilePath = "";
            switch (KhoGiay)
            {
                case "A4":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_GiayCungCapTaiChinh.xls";
                    break;
                case "A3":
                    sFilePath = "/Report_ExcelFrom/Luong/rptLuong_GiayCungCapTaiChinh_A3.xls";
                    break;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamLuong, ThangLuong, DonVi, MaCB, Duyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "GiayCungCap_TaiChinh.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Lấy danh sách đơn vị có dữ liệu
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DuyetLuong"></param>
        /// <returns></returns>
        public static DataTable GetDonVi(String NamLuong, String ThangLuong, String DuyetLuong)
        {
            String DKDuyet = DuyetLuong.Equals("0") ? "" : "AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            String SQL = String.Format(@"SELECT L.iID_MaDonVi,L.sTenDonVi AS TenHT
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                          AND L.iNamBangLuong=@iNamBangLuong
                                          AND L.iThangBangLuong=@iThangBangLuong
                                          {0}
                                        GROUP BY L.iID_MaDonVi,L.sTenDonVi", DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", NamLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", ThangLuong);
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            DataTable dtDonvi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonvi;
        }
        /// <summary>
        /// Lấy danh sách cán bộ
        /// </summary>
        /// <param name="NamLuong">Năm</param>
        /// <param name="ThangLuong">Tháng</param>
        /// <param name="Duyet">Trạng thái duyệt</param>
        /// <param name="Donvi">Mã đơn vị</param>
        /// <returns></returns>
        public static DataTable GetCanBo(String NamLuong, String ThangLuong, String Duyet, String Donvi)
        {
            String DKDuyet = Duyet.Equals("0") ? "" : "AND L.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            String DKDonvi = "AND L.iID_MaDonVi IN(";
            String[] arrDonvi = Donvi.Split(',');
            for (int i = 0; i < arrDonvi.Length; i++)
            {
                DKDonvi += "@iID_MaDonVi" + i;
                if (i < arrDonvi.Length - 1)
                    DKDonvi += " , ";
            }
            DKDonvi += ")";
            String SQL = String.Format(@"SELECT L.iID_MaCanBo
                                              ,L.sHoDem_CanBo+' '+ L.sTen_CanBo AS TenCB
                                              ,L.iID_MaDonVi
                                              ,L.sSoSoLuong_CanBo
                                              ,L.iID_MaBacLuong_CanBo
                                        FROM L_BangLuongChiTiet AS L
                                        WHERE L.iTrangThai=1
                                        AND L.iNamBangLuong=@iNamBangLuong
                                        AND L.iThangBangLuong=@iThangBangLuong
                                        {0}
                                        {1}
                                        GROUP BY L.sTen_CanBo
		                                        ,L.sHoDem_CanBo
		                                        ,L.iID_MaDonVi
		                                        ,L.sSoSoLuong_CanBo
		                                        ,L.iID_MaBacLuong_CanBo
		                                        ,L.iID_MaCanBo", DKDuyet, DKDonvi);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", NamLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", ThangLuong);
            int tt = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong);
            if (!String.IsNullOrEmpty(DKDuyet))
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", tt);
            for (int z = 0; z < arrDonvi.Length; z++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + z, arrDonvi[z]);
            }
            DataTable dtDonvi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonvi;
        }
        /// <summary>
        /// Hiển thị danh sách đơn vị lên trình duyệt
        /// </summary>
        /// <param name="NamLuong">Năm</param>
        /// <param name="ThangLuong">Tháng</param>
        /// <param name="DuyetLuong">Trạng thái</param>
        /// <param name="arrDV"></param>
        /// <returns></returns>
        public String obj_DSDonVi(String NamLuong, String ThangLuong, String DuyetLuong, String[] arrDV)
        {
            DataTable dt = GetDonVi(NamLuong, ThangLuong, DuyetLuong);
            String stbDonVi = "<table class=\"mGrid\">";
            String TenDV = ""; String idDV = "";
            String _Checked1 = "checked=\"checked\"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Checked1 = "";
                TenDV = Convert.ToString(dt.Rows[i]["TenHT"]);
                idDV = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < arrDV.Length; j++)
                {
                    if (idDV == arrDV[j])
                    {
                        _Checked1 = "checked=\"checked\"";
                        break;
                    }
                }
                stbDonVi += "<tr style=\" height: 20px; font-size: 12px; \"><td style=\"width: 20px; text-align:center; height:auto; line-height:7px;\">";
                stbDonVi += "<input type=\"checkbox\" value=\"" + idDV + "\"" + _Checked1 + " check-group=\"iID_MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" style=\"cursor:pointer;\" onchange=\"ChonCB()\" />";
                stbDonVi += "</td><td>" + TenDV + "</td></tr>";
            }
            stbDonVi += "</table>";
            dt.Dispose();
            return stbDonVi;
        }
        /// <summary>
        /// Hiển thị danh sách cán bộ lên trình duyệt
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="Duyet"></param>
        /// <param name="Donvi"></param>
        /// <param name="arrCB"></param>
        /// <returns></returns>
        public String obj_DSCanBo(String NamLuong, String ThangLuong, String Duyet, String Donvi, String[] arrCB)
        {
            DataTable dtCB = GetCanBo(NamLuong, ThangLuong, Duyet, Donvi);
            String stbCB = "<table class=\"mGrid\">";
            String TenCB = ""; String idCB = "", Dv = "", capbac = "", ssLuong = "";
            String _Checked1 = "checked=\"checked\"";
            for (int i = 0; i < dtCB.Rows.Count; i++)
            {
                _Checked1 = "";
                TenCB = Convert.ToString(dtCB.Rows[i]["TenCB"]);
                idCB = Convert.ToString(dtCB.Rows[i]["iID_MaCanBo"]);
                Dv = Convert.ToString(dtCB.Rows[i]["iID_MaDonVi"]);
                capbac = Convert.ToString(dtCB.Rows[i]["iID_MaBacLuong_CanBo"]);
                ssLuong = Convert.ToString(dtCB.Rows[i]["sSoSoLuong_CanBo"]);
                for (int j = 0; j < arrCB.Length; j++)
                {
                    if (idCB == arrCB[j])
                    {
                        _Checked1 = "checked=\"checked\"";
                        break;
                    }
                }
                stbCB += "<tr style=\" height: 20px; font-size: 12px; \"><td style=\"width: 20px; text-align:center; height:auto; line-height:7px;\">";
                stbCB += "<input type=\"checkbox\" value=\"" + idCB + "\"" + _Checked1 + " check-group=\"iID_MaCanBo\" id=\"iID_MaCanBo\" name=\"iID_MaCanBo\" style=\"cursor:pointer;\" />";
                stbCB += "</td><td style=\"width:30px;\">" + Dv + "</td><td style=\"width:30px;\">" + capbac + "</td><td style=\"width:80px; text-align:left;\">" + TenCB + "</td><td style=\"width:50px;\">" + ssLuong + "</td></tr>";
            }
            stbCB += "</table>";
            dtCB.Dispose();
            return stbCB;
        }
        /// <summary>
        /// Ajax hiển thị danh sách đơn vị
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DuyetLuong"></param>
        /// <param name="arrDV"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ds_DonVi(String NamLuong, String ThangLuong, String DuyetLuong, String[] arrDV)
        {
            return Json(obj_DSDonVi(NamLuong, ThangLuong, DuyetLuong, arrDV), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Ajax hiển thị danh sách cán bộ
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="Duyet"></param>
        /// <param name="Donvi"></param>
        /// <param name="arrCB"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ds_CanBo(String NamLuong, String ThangLuong, String Duyet, String Donvi, String[] arrCB)
        {
            return Json(obj_DSCanBo(NamLuong, ThangLuong, Duyet, Donvi, arrCB), JsonRequestBehavior.AllowGet);
        }
    }
}