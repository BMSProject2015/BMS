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
    public class rptTCDN_BaoCaoTongHop_Bieu1Controller : Controller
    {
        //
       
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu1.xls";
        private const String sFilePath_21 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu21.xls";
        private const String sFilePath_22 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu22.xls";
        private const String sFilePath_23 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu23.xls";
        private const String sFilePath_31 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu31.xls";
        private const String sFilePath_32 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu32.xls";
        private const String sFilePath_33 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu33.xls";
        private const String sFilePath_4 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoTongHop_Bieu4.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoTongHop_Bieu1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptTCDN_BaoCaoTongHop_Bieu1(String MaND,String iQuy,String bTrongKy,String iID_MaDonVi,String iID_MaNhom,String iID_MaKhoi,String iID_MaHinhThucHoatDong,String iID_MaLoaiHinhDoanhNghiep,String iLoaiBaoCao)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "", DK = "", DKSELECT = "", DKHAVING = "";
            string[] arrLoaiBaoCao = iLoaiBaoCao.Split(',');
            for (int i = 0; i < arrLoaiBaoCao.Length; i++)
            {
                DK += " sKyHieu LIKE '"+arrLoaiBaoCao[i]+"%'";
                if (i < arrLoaiBaoCao.Length - 1)
                    DK += " OR ";
                if (iLoaiBaoCao == "4") DK = "1=1";
            }
            DataTable vR = new DataTable();
            SQL = String.Format(@"SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND ({0}) ",DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            DataTable dtChitieu = vR;
            int DVT = 1;
            for (int i = 0; i < vR.Rows.Count; i++)
            {
                DKSELECT += String.Format(@",s{0}=SUM(CASE WHEN sKyHieu='{0}' THEN rThucHien/{1} ELSE 0 END)",vR.Rows[i]["sKyHieu"],DVT);
                DKHAVING += String.Format(@" SUM(CASE WHEN sKyHieu='{0}' THEN rThucHien/{1} ELSE 0 END)<>0  ", vR.Rows[i]["sKyHieu"], DVT);
                if (i < vR.Rows.Count - 1)
                    DKHAVING += " OR ";

            }
            cmd = new SqlCommand();
            DK = "";
            if (!String.IsNullOrEmpty(iQuy) && iQuy != "-1")
            {
                if (bTrongKy == "on")
                {
                    DK += " AND iQuy=@iQuy";
                }
                else
                {
                    DK += " AND iQuy<=@iQuy";
                }
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi)&& iID_MaDonVi!=Guid.Empty.ToString() )
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaNhom) && iID_MaNhom != Guid.Empty.ToString())
            {
                DK += " AND iID_MaNhom=@iID_MaNhom";
                cmd.Parameters.AddWithValue("@iID_MaNhom", iID_MaNhom);
            }
            if (!String.IsNullOrEmpty(iID_MaKhoi) && iID_MaKhoi != Guid.Empty.ToString())
            {
                DK += " AND iID_MaKhoi=@iID_MaKhoi";
                cmd.Parameters.AddWithValue("@iID_MaKhoi", iID_MaKhoi);
            }
            if (!String.IsNullOrEmpty(iID_MaHinhThucHoatDong) && iID_MaHinhThucHoatDong != Guid.Empty.ToString())
            {
                DK += " AND iID_MaHinhThucHoatDong=@iID_MaHinhThucHoatDong";
                cmd.Parameters.AddWithValue("@iID_MaHinhThucHoatDong", iID_MaHinhThucHoatDong);
            }
            if (!String.IsNullOrEmpty(iID_MaLoaiHinhDoanhNghiep) && iID_MaLoaiHinhDoanhNghiep != Guid.Empty.ToString())
            {
                DK += " AND iID_MaLoaiHinhDoanhNghiep=@iID_MaLoaiHinhDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaLoaiHinhDoanhNghiep", iID_MaLoaiHinhDoanhNghiep);
            }
            if (iLoaiBaoCao == "4")
            {
                SQL = String.Format(@"SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND  sKyHieu LIKE'4%' ", DK);
                cmd.CommandText = SQL;
                DataTable dtLoai4 = Connection.GetDataTable(cmd);

                String sCongThuc = "";
                String[] arrCongThuc = new String[2];
                for (int i = 0; i < dtLoai4.Rows.Count; i++)
                {
                    
                }

                SQL = String.Format(@"SELECT 
TCDN_ChungTuChiTiet.iID_MaDoanhNghiep,sTenDoanhNghiep
{0}
 FROM TCDN_ChungTuChiTiet
 RIGHT JOIN TCDN_DoanhNghiep
 ON TCDN_ChungTuChiTiet.iID_MaDoanhNghiep=TCDN_DoanhNghiep.iID_MaDoanhNghiep
 WHERE TCDN_ChungTuChiTiet.iTrangThai=1 AND TCDN_ChungTuChiTiet.iNamLamViec=@iNamLamViec AND TCDN_DoanhNghiep.iTrangThai=1 {1}
 GROUP BY TCDN_ChungTuChiTiet.iID_MaDoanhNghiep,sTenDoanhNghiep --HAVING {2}
 ", DKSELECT, DK, DKHAVING);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();

                for (int i = dtChitieu.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dtChitieu.Rows[i]["bLaTong"]))
                    {
                        String iiD_MaChiTieu = Convert.ToString(dtChitieu.Rows[i]["iiD_MaChiTieu"]);
                        for (int j = 0; j < vR.Rows.Count; j++)
                        {
                            double S;
                            S = 0;
                            for (int c = i + 1; c < dtChitieu.Rows.Count; c++)
                            {
                                if (iiD_MaChiTieu == Convert.ToString(dtChitieu.Rows[c]["iiD_MaChiTieu_Cha"]))
                                {
                                    S += Convert.ToDouble(vR.Rows[j]["s" + dtChitieu.Rows[c]["sKyHieu"]]);
                                }
                            }
                            vR.Rows[j]["s" + dtChitieu.Rows[i]["sKyHieu"]] = S;
                        }
                    }
                }
                for (int i = 0; i < dtLoai4.Rows.Count; i++)
                {
                    //lấy công thức
                    sCongThuc = "";
                    List<string> arrCongThucSo = new List<String>();
                    List<string> arrCongThucDau = new List<String>();
                    List<decimal> arrGiaTriNamTruoc = new List<decimal>();
                    List<decimal> arrGiaTriNamNay = new List<decimal>();
                    if (!String.IsNullOrEmpty(Convert.ToString(dtLoai4.Rows[i]["sCongThuc"])))
                    {
                        sCongThuc = Convert.ToString(dtLoai4.Rows[i]["sCongThuc"]);
                    }
                    if (!string.IsNullOrEmpty(sCongThuc))
                    {
                        arrCongThuc = sCongThuc.Split(' ');
                        for (int z = 0; z < arrCongThuc.Length; z++)
                        {
                            if (arrCongThuc[z] == "+" || arrCongThuc[z] == "-" || arrCongThuc[z] == "*" || arrCongThuc[z] == "/")
                            {
                                arrCongThucDau.Add(arrCongThuc[z]);
                            }
                            else
                            {
                                arrCongThucSo.Add(arrCongThuc[z]);
                            }
                        }
                    }
                    


                    for (int j = 0; j < vR.Rows.Count; j++)
                    {
                        decimal KQ = 0;
                        if (arrCongThucSo.Count>0)
                        KQ=Convert.ToDecimal(vR.Rows[j]["s" + arrCongThucSo[0]]);
                        List<decimal> arrGiaTri= new List<decimal>();
                        for (int c = 1; c < arrCongThucSo.Count; c++)
                        {
                           
                            if (arrCongThucDau[c - 1] == "+")
                            {
                                KQ += Convert.ToDecimal(vR.Rows[j]["s" + arrCongThucSo[c]]);
                            }
                            else if (arrCongThucDau[c - 1] == "-")
                            {
                                KQ -= Convert.ToDecimal(vR.Rows[j]["s" + arrCongThucSo[c]]);
                            }
                            else if (arrCongThucDau[c - 1] == "*")
                            {
                                KQ *= Convert.ToDecimal(vR.Rows[j]["s" + arrCongThucSo[c]]);
                            }
                            else
                            {
                                if (Convert.ToDecimal(vR.Rows[j]["s" + arrCongThucSo[c]]) != 0)
                                    KQ /= Convert.ToDecimal(vR.Rows[j]["s" + arrCongThucSo[c]]);
                                else
                                    KQ = 0;
                            }
                        }
                        vR.Rows[j]["s" + dtLoai4.Rows[i]["sKyHieu"]] = Math.Round(KQ * 100, 2);
                    }
                }
            }
            else
            {
                SQL = String.Format(@"SELECT 
TCDN_ChungTuChiTiet.iID_MaDoanhNghiep,sTenDoanhNghiep
{0}
 FROM TCDN_ChungTuChiTiet
 INNER JOIN TCDN_DoanhNghiep
 ON TCDN_ChungTuChiTiet.iID_MaDoanhNghiep=TCDN_DoanhNghiep.iID_MaDoanhNghiep
 WHERE TCDN_ChungTuChiTiet.iTrangThai=1 AND TCDN_ChungTuChiTiet.iNamLamViec=@iNamLamViec AND TCDN_DoanhNghiep.iTrangThai=1 {1}
 GROUP BY TCDN_ChungTuChiTiet.iID_MaDoanhNghiep,sTenDoanhNghiep HAVING {2}
 ", DKSELECT, DK, DKHAVING);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();

                for (int i = dtChitieu.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dtChitieu.Rows[i]["bLaTong"]))
                    {
                        String iiD_MaChiTieu = Convert.ToString(dtChitieu.Rows[i]["iiD_MaChiTieu"]);
                        for (int j = 0; j < vR.Rows.Count; j++)
                        {
                            double S;
                            S = 0;
                            for (int c = i + 1; c < dtChitieu.Rows.Count; c++)
                            {
                                if (iiD_MaChiTieu == Convert.ToString(dtChitieu.Rows[c]["iiD_MaChiTieu_Cha"]))
                                {
                                    S += Convert.ToDouble(vR.Rows[j]["s" + dtChitieu.Rows[c]["sKyHieu"]]);
                                }
                            }
                            vR.Rows[j]["s" + dtChitieu.Rows[i]["sKyHieu"]] = S;
                        }
                    }
                }
            }
            return vR;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String bTrongKy = Request.Form[ParentID + "_bTrongKy"];
            String iID_MaNhom = Request.Form[ParentID + "_iID_MaNhom"];
            String iID_MaKhoi = Request.Form[ParentID + "_iID_MaKhoi"];
            String iID_MaLoaiHinhDoanhNghiep = Request.Form[ParentID + "_iID_MaLoaiHinhDoanhNghiep"];
            String iID_MaHinhThucHoatDong = Request.Form[ParentID + "_iID_MaHinhThucHoatDong"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iLoaiBaoCao = Request.Form[ParentID + "_iLoaiBaoCao"];
            ViewData["PageLoad"] = "1";
            ViewData["iQuy"] = iQuy;
            ViewData["bTrongKy"] = bTrongKy;
            ViewData["iID_MaNhom"] = iID_MaNhom;
            ViewData["iID_MaKhoi"] = iID_MaKhoi;
            ViewData["iID_MaLoaiHinhDoanhNghiep"] = iID_MaLoaiHinhDoanhNghiep;
            ViewData["iID_MaHinhThucHoatDong"] = iID_MaHinhThucHoatDong;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iLoaiBaoCao"] = iLoaiBaoCao;
            ViewData["path"] = "~/Report_Views//TCDN/rptTCDN_BaoCaoTongHop_Bieu1.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND,String iQuy,String bTrongKy, String iID_MaDonVi, String iID_MaNhom, String iID_MaKhoi, String iID_MaHinhThucHoatDong, String iID_MaLoaiHinhDoanhNghiep,String iLoaiBaoCao)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BaoCaoTongHop_Bieu1");
            LoadData(fr, MaND, iQuy, bTrongKy,iID_MaDonVi, iID_MaNhom, iID_MaKhoi, iID_MaHinhThucHoatDong, iID_MaLoaiHinhDoanhNghiep,iLoaiBaoCao);
            String Nam = ReportModels.LayNamLamViec(MaND);
            String TenBieu1 = "", TenBieu2 = "",Quy="";
            if (bTrongKy == "on")
            {
                Quy = "Quý " + iQuy+ " Năm "+Nam;
            }
            else
            {
                Quy = "Đến Quý " + iQuy + " Năm " + Nam;
            }
            if (iQuy == "-1" || iQuy=="")
            {
                Quy = " Năm: " + Nam;
            }
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1,MaND));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2,MaND));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("TenBieu1", TenBieu1);
            fr.SetValue("TenBieu2", TenBieu2);
            fr.SetValue("Quy", Quy);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND,String iQuy,String bTrongKy, String iID_MaDonVi, String iID_MaNhom, String iID_MaKhoi, String iID_MaHinhThucHoatDong, String iID_MaLoaiHinhDoanhNghiep,String iLoaiBaoCao)
        {
            DataRow r;
            DataTable data = rptTCDN_BaoCaoTongHop_Bieu1(MaND, iQuy, bTrongKy, iID_MaDonVi, iID_MaNhom, iID_MaKhoi, iID_MaHinhThucHoatDong, iID_MaLoaiHinhDoanhNghiep, iLoaiBaoCao);
            
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);


           
            data.Dispose();

        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND,String iQuy,String bTrongKy, String iID_MaDonVi, String iID_MaNhom, String iID_MaKhoi, String iID_MaHinhThucHoatDong, String iID_MaLoaiHinhDoanhNghiep,String iLoaiBaoCao)
        {
            HamChung.Language();
            String sDuongDan = sFilePath;
            if (iLoaiBaoCao == "21")
                sDuongDan = sFilePath_21;
            else if(iLoaiBaoCao == "22")
                sDuongDan=sFilePath_22;
            else if (iLoaiBaoCao == "23")
                sDuongDan = sFilePath_23;
            else if (iLoaiBaoCao == "31")
                sDuongDan = sFilePath_31;
            else if (iLoaiBaoCao == "32,33")
                sDuongDan = sFilePath_32;
            else if (iLoaiBaoCao == "34")
                sDuongDan = sFilePath_33;
            else if (iLoaiBaoCao == "4")
                sDuongDan = sFilePath_4;
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iQuy, bTrongKy, iID_MaDonVi, iID_MaNhom, iID_MaKhoi, iID_MaHinhThucHoatDong, iID_MaLoaiHinhDoanhNghiep, iLoaiBaoCao);
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


        public clsExcelResult ExportToExcel(String MaND,String iQuy,String bTrongKy, String iID_MaDonVi, String iID_MaNhom, String iID_MaKhoi, String iID_MaHinhThucHoatDong, String iID_MaLoaiHinhDoanhNghiep,String iLoaiBaoCao)
        {
            HamChung.Language();
            String sDuongDan = sFilePath;
            if (iLoaiBaoCao == "21")
                sDuongDan = sFilePath_21;
            else if (iLoaiBaoCao == "22")
                sDuongDan = sFilePath_22;
            else if (iLoaiBaoCao == "23")
                sDuongDan = sFilePath_23;
            else if (iLoaiBaoCao == "31")
                sDuongDan = sFilePath_31;
            else if (iLoaiBaoCao == "32,33")
                sDuongDan = sFilePath_32;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iQuy, bTrongKy, iID_MaDonVi, iID_MaNhom, iID_MaKhoi, iID_MaHinhThucHoatDong, iID_MaLoaiHinhDoanhNghiep, iLoaiBaoCao);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCaoTongHop.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        

    }
}

