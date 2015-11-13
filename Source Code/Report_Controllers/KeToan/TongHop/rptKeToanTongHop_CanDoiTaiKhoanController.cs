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
namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_CanDoiTaiKhoanController : Controller
    {
        //
        // GET: /rptKeToanTongHop_CanDoiTaiKhoan/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_CanDoiTaiKhoan.xls";
        private const String sFilePath_A3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_CanDoiTaiKhoan_A3.xls";
        public static String NameFile = "";

        public ActionResult Index(String iNam, String iThang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["Pageload"] = 0;
            ViewData["sThongBao"] = "";
            ViewData["iNam"] = iNam;
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_CanDoiTaiKhoan.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID, String iNam, String iThang, String OK)
        {
            if (String.IsNullOrEmpty(OK))
                OK = Request.Form[ParentID + "_OK"];
            if (String.IsNullOrEmpty(iNam))
                iNam = Convert.ToString(Request.Form[ParentID + "_iNam"]);
            if (String.IsNullOrEmpty(iThang))
                iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            if (OK == "OK")
            {
                UPDATE_TaiKhoanHienThi(iNam);
            }

            String sThongBao = ThongBao(iThang, iNam);
            if (String.IsNullOrEmpty(sThongBao) && String.IsNullOrEmpty(OK)) OK = "CANCEL";

            ViewData["sThongBao"] = sThongBao;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_CanDoiTaiKhoan.aspx";
            ViewData["iNam"] = iNam;
            ViewData["iThang"] = iThang;
            ViewData["Pageload"] = 1;
            ViewData["OK"] = OK;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            return View(sViewPath + "ReportView.aspx");
        }
        public String ThongBao(String iThang, String iNam)
        {
            DataRow[] arrR,arrCha;
            DataTable dt = dt_TaiKhoanKhongHienThi(iThang, iNam);
            String sThongBao = "", NoCo = "", TKCap1 = "";
            double rCK_No = 0,rCK_Co=0,rSoDu=0,rTongSoDuTKCon=0;
            Boolean bHienThi = false;
            String TK_Con="";
            if (dt.Rows.Count == 0) return sThongBao;
            arrCha = dt.Select("bHienThi=0 AND TK_Con IS NULL");
            if (arrCha.Length > 0)
            {
                for (int i = 0; i < arrCha.Length; i++)
                {
                    String TK = Convert.ToString(arrCha[i]["iID_MaTaiKhoan"]);
                    TK_Con = Convert.ToString(arrCha[i]["TK_Con"]);
                    bHienThi = Convert.ToBoolean(arrCha[i]["bHienThi"]);
                    if (arrCha[i]["rCK_No"] != DBNull.Value)
                    {
                        rCK_No = Convert.ToDouble(arrCha[i]["rCK_No"]);
                        NoCo = " nợ ";
                    }
                    if (arrCha[i]["rCK_Co"] != DBNull.Value)
                    {
                        rCK_Co = Convert.ToDouble(arrCha[i]["rCK_Co"]);
                        NoCo = " có ";
                    }
                    if (rCK_Co > 0) rSoDu = rCK_Co;
                    else rSoDu = rCK_No;

                    if (bHienThi == false)
                    {
                        sThongBao += " Bảng Cần đối tài khoản không cân Tài khoản " + TK + ":Số dư =" + rSoDu.ToString()+ "\\n";
                    }
                   
                }
                if(String.IsNullOrEmpty(sThongBao)==false)  sThongBao= sThongBao+ "Bạn có muốn thêm tài khoản vào không?";
            }
            else
            {
                arrCha = dt.Select("TK_Con iS NULL");
                for (int i = 0; i < arrCha.Length; i++)
                {
                    String TK = Convert.ToString(arrCha[i]["iID_MaTaiKhoan"]);
                    String TKCon="";
                    TK_Con = Convert.ToString(arrCha[i]["TK_Con"]);
                    bHienThi = Convert.ToBoolean(arrCha[i]["bHienThi"]);
                    if (arrCha[i]["rCK_No"] != DBNull.Value)
                    {
                        rCK_No = Convert.ToDouble(arrCha[i]["rCK_No"]);
                        NoCo = " nợ ";
                    }
                    if (arrCha[i]["rCK_Co"] != DBNull.Value)
                    {
                        rCK_Co = Convert.ToDouble(arrCha[i]["rCK_Co"]);
                        NoCo = " có ";
                    }
                    if (rCK_Co > 0) rSoDu = rCK_Co;
                    else rSoDu = rCK_No;

                    arrR = dt.Select("bHienThi=0 AND TK_Con=1 AND iID_MaTaiKhoan LIKE '"+ TK +"%'");
                    String capTK="";
                    if (arrR.Length > 0)
                    {
                        TKCon=Convert.ToString(arrR[0]["iID_MaTaiKhoan"]);
                        switch(TKCon.Length)
                        {
                            case 3:
                                capTK="cấp 1";
                            break;
                            case 4:
                                capTK="cấp 2";
                            break;
                            case 5:
                                capTK="cấp 3";
                            break;
                            case 6:
                                capTK="cấp 4";
                            break;

                        }
                        
                        for (int j = 0; j < arrR.Length; j++)
                        {
                            if (j < arrCha.Length)
                            {
                                if (arrCha[j]["rCK_No"] != DBNull.Value)
                                {
                                    rCK_No += Convert.ToDouble(arrCha[j]["rCK_No"]);
                                }
                                if (arrCha[j]["rCK_Co"] != DBNull.Value)
                                {
                                    rCK_Co += Convert.ToDouble(arrCha[j]["rCK_Co"]);
                                }
                            }
                        }
                        if(rCK_No-rCK_Co>0)
                            rTongSoDuTKCon = rCK_No-rCK_Co;
                        else
                            rTongSoDuTKCon = rCK_Co - rCK_No;


                        sThongBao += "Tài khoản " +TK +" không bằng tổng các tài khoản"+ capTK + ":" +Convert.ToString(rSoDu-rTongSoDuTKCon)+"\\n";
                    }
                    
                }
            }
           
            return sThongBao;
        }

        public ExcelFile CreateReport(String path, String iNam, String iThang)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            #region DK tháng
            String strThang = "";
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                Int32 dbThang = Convert.ToInt32(iThang);
                if (dbThang <= 12)
                {
                    strThang = "Tháng " + iThang;
                }
                else
                {
                    switch (dbThang)
                    {
                        case 13://Quý I
                            strThang = "Quý I ";
                            break;
                        case 14://Quý II
                            strThang = "Quý II ";
                            break;
                        case 15://Quý III
                            strThang = "Quý III ";
                            break;
                        case 16://Quý IV
                            strThang = "Quý IV ";
                            break;
                        case 17://6 Tháng đầu năm
                            strThang = "6 tháng đầu ";
                            break;
                        case 18://6 Tháng cuối năm
                            strThang = "6 tháng cuối ";
                            break;
                    }
                }
            }
            #endregion
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
               FlexCelReport fr = new FlexCelReport();
               fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_CanDoiTaiKhoan");
                LoadData(fr, iNam, iThang);
                fr.SetValue("Nam", iNam);
                fr.SetValue("Thang", strThang);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("CucTaiChinh", CucTaiChinh);
                fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai()
                    );
                fr.Run(Result);
                fr.Dispose();
                return Result;
            
        }

        public clsExcelResult ExportToPDF(String iNam, String iThang,String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String DuongDan = "";
            if (KhoGiay == "2") DuongDan = sFilePath;
            else DuongDan = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang);
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
        public clsExcelResult ExportToExcel(String iNam, String iThang,String KhoGiay)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            clsExcelResult clsResult = new clsExcelResult();
            String DuongDan = "";
            if (KhoGiay == "2") DuongDan = sFilePath;
            else DuongDan = sFilePath_A3;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CanDoiTaiKhoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String iNam, String iThang,String KhoGiay)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);

            String DuongDan = "";
            if (KhoGiay == "2") DuongDan = sFilePath;
            else DuongDan = sFilePath_A3;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang);
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
        public String LoadData(FlexCelReport fr, String iNam, String iThang)
        {
            data _data = dt_BangCanDoiTaiKhoan(iThang, iNam);

            DataTable dtTKKhongHienThi = dt_TaiKhoanKhongHienThi(iThang, iNam);
            DataTable dtTrongBang = _data.dt_TrongBang;
            dtTrongBang.TableName = "ChiTiet";

            DataTable CapHai = HamChung.SelectDistinct("CapHai", dtTrongBang, "TKCAP1,TKCAP2", "TKCAP1,TKCAP2,TKCAP3,sTen,rSoDuDauKy_No,rSoDuDauKy_Co,rPS_No,rPS_Co,rLuyKe_No,rLuyKe_Co,rCK_No,rCK_Co,bHienThi");

            DataTable CapMot = HamChung.SelectDistinct("CapMot", dtTrongBang, "TKCAP1", "TKCAP1,sTen,rSoDuDauKy_No,rSoDuDauKy_Co,rPS_No,rPS_Co,rLuyKe_No,rLuyKe_Co,rCK_No,rCK_Co,bHienThi");



            #region
            for (int i = 0; i < CapHai.Rows.Count; i++)
            {
                if (Convert.ToString(CapHai.Rows[i]["TKCAP2"]) == "")
                {
                    CapHai.Rows.RemoveAt(i);

                    i = i - 1;
                }
                else
                {
                    DataRow[] arrR = dtTrongBang.Select("TKCAP2='" + Convert.ToString(CapHai.Rows[i]["TKCAP2"]) + "'");
                    Decimal SoDuDauKy_No = 0, SoDuDauKy_Co = 0, rPS_No = 0, rPS_Co = 0, rLuyKe_No = 0, rLuyKe_Co = 0, rCK_No = 0, rCK_Co = 0;
                    for (int j = 0; j < arrR.Length; j++)
                    {
                        SoDuDauKy_No = SoDuDauKy_No + Convert.ToDecimal(arrR[j]["rSoDuDauKy_No"]);
                        SoDuDauKy_Co = SoDuDauKy_Co + Convert.ToDecimal(arrR[j]["rSoDuDauKy_Co"]);

                        rPS_No = rPS_No + Convert.ToDecimal(arrR[j]["rPS_No"]);
                        rPS_Co = rPS_Co + Convert.ToDecimal(arrR[j]["rPS_Co"]);

                        rLuyKe_No = rLuyKe_No + Convert.ToDecimal(arrR[j]["rLuyKe_No"]);
                        rLuyKe_Co = rLuyKe_Co + Convert.ToDecimal(arrR[j]["rLuyKe_Co"]);


                        rCK_No = rCK_No + Convert.ToDecimal(arrR[j]["rCK_No"]);
                        rCK_Co = rCK_Co + Convert.ToDecimal(arrR[j]["rCK_Co"]);
                    }
                    if (SoDuDauKy_No - SoDuDauKy_Co > 0)
                    {
                        CapHai.Rows[i]["rSoDuDauKy_No"] = SoDuDauKy_No - SoDuDauKy_Co;
                        CapHai.Rows[i]["rSoDuDauKy_Co"] = 0;
                    }
                    else
                    {
                        CapHai.Rows[i]["rSoDuDauKy_Co"] = (SoDuDauKy_No - SoDuDauKy_Co) * (-1);
                        CapHai.Rows[i]["rSoDuDauKy_No"] = 0;
                    }

                    CapHai.Rows[i]["rPS_No"] = rPS_No;
                    CapHai.Rows[i]["rPS_Co"] = rPS_Co;

                    CapHai.Rows[i]["rLuyKe_No"] = rLuyKe_No;
                    CapHai.Rows[i]["rLuyKe_Co"] = rLuyKe_Co;

                    if (rCK_No - rCK_Co > 0)
                    {
                        CapHai.Rows[i]["rCK_No"] = rCK_No - rCK_Co;
                        CapHai.Rows[i]["rCK_Co"] = 0;
                    }
                    else
                    {
                        CapHai.Rows[i]["rCK_Co"] = (rCK_No - rCK_Co) * (-1);
                        CapHai.Rows[i]["rCK_No"] = 0;
                    }
                }
            }

            for (int i = 0; i < CapMot.Rows.Count; i++)
            {
                DataRow[] arrR = CapHai.Select("TKCAP1='" + Convert.ToString(CapMot.Rows[i]["TKCAP1"]) + "'");
                Decimal SoDuDauKy_No = 0, SoDuDauKy_Co = 0, rPS_No = 0, rPS_Co = 0, rLuyKe_No = 0, rLuyKe_Co = 0, rCK_No = 0, rCK_Co = 0;
                for (int j = 0; j < arrR.Length; j++)
                {
                    SoDuDauKy_No = SoDuDauKy_No + Convert.ToDecimal(arrR[j]["rSoDuDauKy_No"]);
                    SoDuDauKy_Co = SoDuDauKy_Co + Convert.ToDecimal(arrR[j]["rSoDuDauKy_Co"]);

                    rPS_No = rPS_No + Convert.ToDecimal(arrR[j]["rPS_No"]);
                    rPS_Co = rPS_Co + Convert.ToDecimal(arrR[j]["rPS_Co"]);

                    rLuyKe_No = rLuyKe_No + Convert.ToDecimal(arrR[j]["rLuyKe_No"]);
                    rLuyKe_Co = rLuyKe_Co + Convert.ToDecimal(arrR[j]["rLuyKe_Co"]);


                    rCK_No = rCK_No + Convert.ToDecimal(arrR[j]["rCK_No"]);
                    rCK_Co = rCK_Co + Convert.ToDecimal(arrR[j]["rCK_Co"]);
                }
                if (arrR.Length > 0)
                {
                    if (SoDuDauKy_No - SoDuDauKy_Co > 0)
                    {
                        CapMot.Rows[i]["rSoDuDauKy_No"] = SoDuDauKy_No - SoDuDauKy_Co;
                        CapMot.Rows[i]["rSoDuDauKy_Co"] = 0;
                    }
                    else
                    {
                        CapMot.Rows[i]["rSoDuDauKy_Co"] = (SoDuDauKy_No - SoDuDauKy_Co) * (-1);
                        CapMot.Rows[i]["rSoDuDauKy_No"] = 0;
                    }

                    CapMot.Rows[i]["rPS_No"] = rPS_No;
                    CapMot.Rows[i]["rPS_Co"] = rPS_Co;



                    CapMot.Rows[i]["rLuyKe_No"] = rLuyKe_No;
                    CapMot.Rows[i]["rLuyKe_Co"] = rLuyKe_Co;

                    if (rCK_No - rCK_Co > 0)
                    {
                        CapMot.Rows[i]["rCK_No"] = rCK_No - rCK_Co;
                        CapMot.Rows[i]["rCK_Co"] = 0;
                    }
                    else
                    {
                        CapMot.Rows[i]["rCK_Co"] = (rCK_No - rCK_Co) * (-1);
                        CapMot.Rows[i]["rCK_No"] = 0;
                    }
                }
            }


            for (int i = 0; i < dtTrongBang.Rows.Count; i++)
            {
                if (Convert.ToString(dtTrongBang.Rows[i]["TKCAP3"]) == "")
                {
                    dtTrongBang.Rows.RemoveAt(i);

                    i = i - 1;
                }
            }

            #endregion

            DataTable dtNgoaiBang = _data.dt_NgoaiBang;

            DataTable NB_CapHai = HamChung.SelectDistinct("NB_CapHai", dtNgoaiBang, "TKCAP1,TKCAP2", "TKCAP1,TKCAP2,TKCAP3,sTen,rSoDuDauKy_No,rSoDuDauKy_Co,rPS_No,rPS_Co,rLuyKe_No,rLuyKe_Co,rCK_No,rCK_Co,bHienThi");

            DataTable NB_CapMot = HamChung.SelectDistinct("NB_CapMot", dtNgoaiBang, "TKCAP1", "TKCAP1,sTen,rSoDuDauKy_No,rSoDuDauKy_Co,rPS_No,rPS_Co,rLuyKe_No,rLuyKe_Co,rCK_No,rCK_Co,bHienThi");

            #region tính tổng lên tài khoản cha
            for (int i = 0; i < NB_CapHai.Rows.Count; i++)
            {
                if (Convert.ToString(NB_CapHai.Rows[i]["TKCAP2"]) == "")
                {
                    NB_CapHai.Rows.RemoveAt(i);

                    i = i - 1;
                }
                else
                {
                    DataRow[] arrR = dtNgoaiBang.Select("TKCAP2='" + Convert.ToString(NB_CapHai.Rows[i]["TKCAP2"]) + "'");
                    Decimal SoDuDauKy_No = 0, SoDuDauKy_Co = 0, rPS_No = 0, rPS_Co = 0, rLuyKe_No = 0, rLuyKe_Co = 0, rCK_No = 0, rCK_Co = 0;
                    for (int j = 0; j < arrR.Length; j++)
                    {
                        SoDuDauKy_No = SoDuDauKy_No + Convert.ToDecimal(arrR[j]["rSoDuDauKy_No"]);
                        SoDuDauKy_Co = SoDuDauKy_Co + Convert.ToDecimal(arrR[j]["rSoDuDauKy_Co"]);

                        rPS_No = rPS_No + Convert.ToDecimal(arrR[j]["rPS_No"]);
                        rPS_Co = rPS_Co + Convert.ToDecimal(arrR[j]["rPS_Co"]);

                        rLuyKe_No = rLuyKe_No + Convert.ToDecimal(arrR[j]["rLuyKe_No"]);
                        rLuyKe_Co = rLuyKe_Co + Convert.ToDecimal(arrR[j]["rLuyKe_Co"]);


                        rCK_No = rCK_No + Convert.ToDecimal(arrR[j]["rCK_No"]);
                        rCK_Co = rCK_Co + Convert.ToDecimal(arrR[j]["rCK_Co"]);
                    }
                    if (SoDuDauKy_No - SoDuDauKy_Co > 0)
                    {
                        NB_CapHai.Rows[i]["rSoDuDauKy_No"] = SoDuDauKy_No - SoDuDauKy_Co;
                        NB_CapHai.Rows[i]["rSoDuDauKy_Co"] = 0;
                    }
                    else
                    {
                        NB_CapHai.Rows[i]["rSoDuDauKy_Co"] = (SoDuDauKy_No - SoDuDauKy_Co) * (-1);
                        NB_CapHai.Rows[i]["rSoDuDauKy_No"] = 0;
                    }


                    NB_CapHai.Rows[i]["rPS_No"] = rPS_No;
                    NB_CapHai.Rows[i]["rPS_Co"] = rPS_Co;

                    NB_CapHai.Rows[i]["rLuyKe_No"] = rLuyKe_No;
                    NB_CapHai.Rows[i]["rLuyKe_Co"] = rLuyKe_Co;

                    if (rCK_No - rCK_Co > 0)
                    {
                        NB_CapHai.Rows[i]["rCK_No"] = rCK_No - rCK_Co;
                        NB_CapHai.Rows[i]["rCK_Co"] = 0;
                    }
                    else
                    {
                        NB_CapHai.Rows[i]["rCK_Co"] = (rCK_No - rCK_Co) * (-1);
                        NB_CapHai.Rows[i]["rCK_No"] = 0;
                    }
                }
            }

            for (int i = 0; i < NB_CapMot.Rows.Count; i++)
            {
                DataRow[] arrR = NB_CapHai.Select("TKCAP1='" + Convert.ToString(NB_CapMot.Rows[i]["TKCAP1"]) + "'");
                Decimal SoDuDauKy_No = 0, SoDuDauKy_Co = 0, rPS_No = 0, rPS_Co = 0, rLuyKe_No = 0, rLuyKe_Co = 0, rCK_No = 0, rCK_Co = 0;
                for (int j = 0; j < arrR.Length; j++)
                {
                    SoDuDauKy_No = SoDuDauKy_No + Convert.ToDecimal(arrR[j]["rSoDuDauKy_No"]);
                    SoDuDauKy_Co = SoDuDauKy_Co + Convert.ToDecimal(arrR[j]["rSoDuDauKy_Co"]);

                    rPS_No = rPS_No + Convert.ToDecimal(arrR[j]["rPS_No"]);
                    rPS_Co = rPS_Co + Convert.ToDecimal(arrR[j]["rPS_Co"]);

                    rLuyKe_No = rLuyKe_No + Convert.ToDecimal(arrR[j]["rLuyKe_No"]);
                    rLuyKe_Co = rLuyKe_Co + Convert.ToDecimal(arrR[j]["rLuyKe_Co"]);


                    rCK_No = rCK_No + Convert.ToDecimal(arrR[j]["rCK_No"]);
                    rCK_Co = rCK_Co + Convert.ToDecimal(arrR[j]["rCK_Co"]);
                }
                if (arrR.Length > 0)
                {
                    if (SoDuDauKy_No - SoDuDauKy_Co > 0)
                    {
                        NB_CapMot.Rows[i]["rSoDuDauKy_No"] = SoDuDauKy_No - SoDuDauKy_Co;
                        NB_CapMot.Rows[i]["rSoDuDauKy_Co"] = 0;
                    }
                    else
                    {
                        NB_CapMot.Rows[i]["rSoDuDauKy_Co"] = (SoDuDauKy_No - SoDuDauKy_Co) * (-1);
                        NB_CapMot.Rows[i]["rSoDuDauKy_No"] = 0;
                    }
                    if (rPS_No - rPS_Co > 0)
                    {
                        NB_CapMot.Rows[i]["rPS_No"] = rPS_No ;
                        NB_CapMot.Rows[i]["rPS_Co"] = rPS_Co;
                    }
                    else
                    {
                        NB_CapMot.Rows[i]["rPS_Co"] = rPS_Co;
                        NB_CapMot.Rows[i]["rPS_No"] = rPS_No;
                    }

                    if (rLuyKe_No - rLuyKe_Co > 0)
                    {
                        NB_CapMot.Rows[i]["rLuyKe_No"] = rLuyKe_No ;
                        NB_CapMot.Rows[i]["rLuyKe_Co"] = rLuyKe_Co;
                    }
                    else
                    {
                        NB_CapMot.Rows[i]["rLuyKe_Co"] = rLuyKe_Co;
                        NB_CapMot.Rows[i]["rLuyKe_No"] = rLuyKe_No;
                    }
                    if (rCK_No - rCK_Co > 0)
                    {
                        NB_CapMot.Rows[i]["rCK_No"] = rCK_No - rCK_Co;
                        NB_CapMot.Rows[i]["rCK_Co"] = 0;
                    }
                    else
                    {
                        NB_CapMot.Rows[i]["rCK_Co"] = (rCK_No - rCK_Co) * (-1);
                        NB_CapMot.Rows[i]["rCK_No"] = 0;
                    }
                }
            }


            for (int i = 0; i < dtNgoaiBang.Rows.Count; i++)
            {
                if (Convert.ToString(dtNgoaiBang.Rows[i]["TKCAP3"]) == "")
                {
                    dtNgoaiBang.Rows.RemoveAt(i);

                    i = i - 1;
                }
            }

            #endregion

            DataTable dtTKC1 = dt_TaiKhoan(iNam, 1);
            DataTable dtTKC2 = dt_TaiKhoan(iNam, 2);

            #region xóa những tài khoản không được hiển thị
            String TK1 = "", TK2 = "";
            Boolean CoTK = true;
            for (int i = 0; i < dtTrongBang.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtTrongBang.Rows[i]["bHienThi"]) == false)
                {
                    dtTrongBang.Rows.RemoveAt(i);
                    if (i > 0) i = i - 1;
                }
            }

            for (int i = 0; i < CapHai.Rows.Count; i++)
            {
                TK1 = Convert.ToString(CapHai.Rows[i]["TKCAP2"]);
                CoTK = false;
                for (int j = 0; j < dtTKC2.Rows.Count; j++)
                {
                    TK2 = Convert.ToString(dtTKC2.Rows[j]["iID_MaTaiKhoan"]);
                    if (TK1 == TK2)
                    {
                        if (Convert.ToBoolean(dtTKC2.Rows[j]["bHienThi"]) == false)
                        {
                            CapHai.Rows[i]["bHienThi"] = 0;
                        }
                        CoTK = true;
                    }
                }
                if (CoTK == false)
                {
                    CapHai.Rows[i]["bHienThi"] = 0;
                }
            }

            for (int i = 0; i < CapMot.Rows.Count; i++)
            {
                TK1 = Convert.ToString(CapMot.Rows[i]["TKCAP1"]);
                CoTK = false;
                for (int j = 0; j < dtTKC1.Rows.Count; j++)
                {
                    TK2 = Convert.ToString(dtTKC1.Rows[j]["iID_MaTaiKhoan"]);
                    if (TK1 == TK2)
                    {
                        if (Convert.ToBoolean(dtTKC1.Rows[j]["bHienThi"]) == false)
                        {
                            CapMot.Rows[i]["bHienThi"] = 0;
                        }
                        CoTK = true;
                    }
                }
                if (CoTK == false)
                {
                    CapMot.Rows[i]["bHienThi"] = 0;
                }
            }

            for (int i = 0; i < dtNgoaiBang.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtNgoaiBang.Rows[i]["bHienThi"]) == false)
                {
                    dtNgoaiBang.Rows.RemoveAt(i);
                    if (i > 0) i = i - 1;
                }
            }

            for (int i = 0; i < NB_CapHai.Rows.Count; i++)
            {
                TK1 = Convert.ToString(NB_CapHai.Rows[i]["TKCAP2"]);
                CoTK = false;
                for (int j = 0; j < dtTKC2.Rows.Count; j++)
                {
                    TK2 = Convert.ToString(dtTKC2.Rows[j]["iID_MaTaiKhoan"]);
                    if (TK1 == TK2)
                    {
                        if (Convert.ToBoolean(dtTKC2.Rows[j]["bHienThi"]) == false)
                        {
                            NB_CapHai.Rows[i]["bHienThi"] = 0;
                        }
                        CoTK = true;
                    }
                }
                if (CoTK == false)
                {
                    NB_CapHai.Rows[i]["bHienThi"] = 0;
                }
            }

            for (int i = 0; i < NB_CapMot.Rows.Count; i++)
            {
                TK1 = Convert.ToString(NB_CapMot.Rows[i]["TKCAP1"]);
                CoTK = false;
                for (int j = 0; j < dtTKC1.Rows.Count; j++)
                {
                    TK2 = Convert.ToString(dtTKC1.Rows[j]["iID_MaTaiKhoan"]);
                    if (TK1 == TK2)
                    {
                        if (Convert.ToBoolean(dtTKC1.Rows[j]["bHienThi"]) == false)
                        {
                            NB_CapMot.Rows[i]["bHienThi"] = 0;
                        }
                        CoTK = true;
                    }
                }
                if (CoTK == false)
                {
                    NB_CapMot.Rows[i]["bHienThi"] = 0;
                }
            }
            #endregion

            fr.AddTable("ChiTiet", dtTrongBang);
            fr.AddTable("CapHai", CapHai);
            fr.AddTable("CapMot", CapMot);

            fr.AddTable("NB_ChiTiet", dtNgoaiBang);
            fr.AddTable("NB_CapHai", NB_CapHai);
            fr.AddTable("NB_CapMot", NB_CapMot);

            dtTrongBang.Dispose();
            CapHai.Dispose();
            CapMot.Dispose();

            dtNgoaiBang.Dispose();
            NB_CapHai.Dispose();
            NB_CapMot.Dispose();
            return null;
        }


        public DataTable dt_TaiKhoan(String iNam, int Cap)
        {
            int DoDaiTaiKhoan = 3;
            switch (Cap)
            {
                case 1:
                    DoDaiTaiKhoan = 3;
                    break;
                case 2:
                    DoDaiTaiKhoan = 4;
                    break;
                case 3:
                    DoDaiTaiKhoan = 5;
                    break;
                case 4:
                    DoDaiTaiKhoan = 6;
                    break;
            }
            String SQL = String.Format("SELECT iID_MaTaiKhoan,bHienThi FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam={0} AND LEN(iID_MaTaiKhoan)={1} ORDER BY iID_MaTaiKhoan", iNam, DoDaiTaiKhoan);
            return Connection.GetDataTable(SQL);
        }

        public DataTable dt_TaiKhoanKhongHienThi(String iThang, String iNamLamViec)
        {
            String SQL = "select iID_MaTaiKhoan FROM KT_TaiKhoan WHERE bHienThi=0 AND iTrangThai=1 ORDER BY iID_MaTaiKhoan";
            DataTable dt = Connection.GetDataTable(SQL);
            DataTable dtTK = new DataTable();
            if (dt.Rows.Count > 0)
            {
                String DK_Co = "", DK_No = "";
                SqlCommand cmd = new SqlCommand();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(DK_Co) == false) DK_Co += " OR ";
                    DK_Co += " iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan_Co" + i.ToString();

                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co" + i.ToString(), dt.Rows[i]["iID_MaTaiKhoan"].ToString() + "%");
                    if (String.IsNullOrEmpty(DK_No) == false) DK_No += " OR ";
                    DK_No += " iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan_No" + i.ToString();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No" + i.ToString(), dt.Rows[i]["iID_MaTaiKhoan"].ToString() + "%");
                }

                if (String.IsNullOrEmpty(DK_Co) == false) DK_Co = " AND (" + DK_Co + ")";
                if (String.IsNullOrEmpty(DK_No) == false) DK_No = " AND (" + DK_No + ")";

                SQL = "SELECT iID_MaTaiKhoan_No as iID_MaTaiKhoan ,SUM(rPS_No) AS rLK_No,SUM(rPS_Co) AS rLK_Co";
                SQL += ",rCK_No=CASE WHEN SUM(rPS_No)-SUM(rPS_Co)>0 THEN SUM(rPS_No)-SUM(rPS_Co) ELSE 0 END";
                SQL += ",rCK_Co=CASE WHEN SUM(rPS_No)-SUM(rPS_Co)<0 THEN SUM(rPS_Co)-SUM(rPS_No) ELSE 0 END";
                SQL += " FROM (";
                SQL += " SELECT ";
                SQL += " iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co=iID_MaTaiKhoan_No,SUM(rSoTien) as rPS_No,rPS_Co=0.0";
                SQL += " FROM KT_ChungTuChiTiet";
                SQL += " WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaTaiKhoan_No <> ''";
                SQL += " AND iThangCT=@iThangCT";
                SQL += " AND iNamLamViec=@iNamLamViec";
                SQL += " {0}";
                SQL += " GROUP BY iID_MaTaiKhoan_No";
                SQL += " UNION";
                SQL += " SELECT ";
                SQL += " iID_MaTaiKhoan_No=iID_MaTaiKhoan_Co,iID_MaTaiKhoan_Co,rPS_No=0.0,SUM(rSoTien) as rPS_Co";
                SQL += " FROM KT_ChungTuChiTiet";
                SQL += " WHERE iTrangThai=1  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaTaiKhoan_Co <> ''";
                SQL += " AND iThangCT=@iThangCT";
                SQL += " AND iNamLamViec=@iNamLamViec";
                SQL += " {1}";
                SQL += " GROUP BY iID_MaTaiKhoan_Co";
                SQL += " ) KT";
                SQL += " GROUP BY iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co";

                String strSQL = String.Format(SQL, DK_No, DK_Co);
                if (String.IsNullOrEmpty(DK_Co) == false) DK_Co = " AND (" + DK_Co + ")";
                if (String.IsNullOrEmpty(DK_No) == false) DK_No = " AND (" + DK_No + ")";

                cmd.CommandText = strSQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iThangCT", iThang);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                dtTK = Connection.GetDataTable(cmd);
                cmd.Dispose();
                dtTK = KeToanTongHopModels.DienTaiKhoanCha(dtTK, iNamLamViec, "iID_MaTaiKhoan");
            }


            return dtTK;
        }

        public void UPDATE_TaiKhoanHienThi(String iNam)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE KT_TaiKhoan SET bHienThi=1  WHERE iNam=@iNam";
            cmd.Parameters.AddWithValue("@iNam", iNam);
            Connection.UpdateDatabase(cmd);
        }

        public class data
        {
            public DataTable dt_TrongBang { get; set; }
            public DataTable dt_NgoaiBang { get; set; }
            public DataTable dt_TB_CapMot { get; set; }
            public DataTable dt_NB_CapMot { get; set; }
            public DataTable dt_TB_CapHai { get; set; }
            public DataTable dt_NB_CapHai { get; set; }

        }

        public data dt_BangCanDoiTaiKhoan(String iThang, String iNam)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "", sDauKy = "", sCuoiKy = "", DK_LuyKe = "";
            #region Điều kiện
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                Int32 dbThang = Convert.ToInt32(iThang);
                if (dbThang <= 12)
                {
                    DK += " AND iThang IN (" + iThang + ")";
                    DK_LuyKe = " AND iThangCT <=" + iThang;
                    //                  DK += " AND iThang = @iThang";
                    //                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    sDauKy = Convert.ToString(dbThang - 1);
                    sCuoiKy = Convert.ToString(iThang);
                }
                else
                {
                    switch (dbThang)
                    {
                        case 13://Quý I
                            DK += " AND iThang IN (1,2,3) ";
                            sDauKy = Convert.ToString(0);
                            sCuoiKy = Convert.ToString(3);
                            DK_LuyKe = " AND iThangCT <= 3";
                            break;
                        case 14://Quý II
                            DK += " AND iThang IN (4,5,6) ";
                            sDauKy = Convert.ToString(3);
                            sCuoiKy = Convert.ToString(6);
                            DK_LuyKe = " AND iThangCT <= 6";
                            break;
                        case 15://Quý III
                            DK += " AND iThang IN (7,8,9) ";
                            sDauKy = Convert.ToString(6);
                            sCuoiKy = Convert.ToString(9);
                            DK_LuyKe = " AND iThangCT <= 9";
                            break;
                        case 16://Quý IV
                            DK += " AND iThang IN (10,11,12) ";
                            sDauKy = Convert.ToString(9);
                            sCuoiKy = Convert.ToString(12);
                            DK_LuyKe = " AND iThangCT <= 12";
                            break;
                        case 17://6 Tháng đầu năm
                            DK += " AND iThang IN (1,2,3,4,5,6) ";
                            sDauKy = Convert.ToString(0);
                            sCuoiKy = Convert.ToString(6);
                            DK_LuyKe = " AND iThangCT <= 6";
                            break;
                        case 18://6 Tháng cuối năm
                            DK += " AND iThang IN (7,8,9,10,11,12) ";
                            sDauKy = Convert.ToString(6);
                            sCuoiKy = Convert.ToString(12);
                            DK_LuyKe = " AND iThangCT <= 12";
                            break;
                        case 19://Cả năm
                            DK += " AND iThang IN (1,2,3,4,5,6,7,8,9,10,11,12) ";
                            sDauKy = Convert.ToString(0);
                            sCuoiKy = Convert.ToString(12);
                            DK_LuyKe = " AND iThangCT <= 12";
                            break;
                    }
                }
            }
            #endregion
            String strDSTK = "SELECT iID_MaTaiKhoan as sTKNo";
            strDSTK += ",TKCAP1=SUBSTRING(iID_MaTaiKhoan,1,3)";
            strDSTK += ",TKCAP2=CASE WHEN Len(SUBSTRING(iID_MaTaiKhoan,1,4))=4 Then SUBSTRING(iID_MaTaiKhoan,1,4) else '' end";
            strDSTK += ",TKCAP3=CASE WHEN Len(SUBSTRING(iID_MaTaiKhoan,1,5))=5 Then SUBSTRING(iID_MaTaiKhoan,1,5) else '' end";
            strDSTK += ",sTen";
            strDSTK += ",rSoDuDauKy_No=0.0";
            strDSTK += ",rSoDuDauKy_Co=0.0";
            strDSTK += ",rPS_No=0.0";
            strDSTK += ",rPS_Co=0.0";
            strDSTK += ",rLuyKe_No=0.0";
            strDSTK += ",rLuyKe_Co=0.0";
            strDSTK += ",rCK_No=0.0";
            strDSTK += ",rCK_Co=0.0";
            strDSTK += ",bHienThi";
            strDSTK += " FROM KT_TaiKhoan";
            strDSTK += " WHERE iTrangThai=1 AND iNam='{1}'  {0} ORDER By iID_MaTaiKhoan";
            String strTB = "";
            strTB = String.Format(strDSTK, " AND SUBSTRING(iID_MaTaiKhoan,1,1)<>'0' ", iNam);
            DataTable dtDSTK_TB = Connection.GetDataTable(strTB);
            String strNB = "";
            strNB = String.Format(strDSTK, " AND SUBSTRING(iID_MaTaiKhoan,1,1)='0' ", iNam);
            DataTable dtDSTK_NB = Connection.GetDataTable(strNB);

            DataTable dtTrongBang = null;
            String DKTK_TrongBang = " AND SUBSTRING(sTKNo,1,1)<>'0'";
            String DKTK_NgoaiBang = " AND SUBSTRING(sTKNo,1,1) = '0'";
            String strSQL = "";
            String SQL = "SELECT sTKNo";
            SQL += ",TKCAP1=SUBSTRING(sTKNo,1,3)";
            SQL += ",TKCAP2=CASE WHEN Len(SUBSTRING(sTKNo,1,4))=4 Then SUBSTRING(sTKNo,1,4) else '' end";
            SQL += ",TKCAP3=CASE WHEN Len(SUBSTRING(sTKNo,1,5))=5 Then SUBSTRING(sTKNo,1,5) else '' end";
            SQL += ",sTen";
            SQL += ",rSoDuDauKy_No=0.0";
            SQL += ",rSoDuDauKy_Co=0.0";
            SQL += ",isNull(rPS_No,0) AS rPS_No ";
            SQL += ",isNull(rPS_Co,0) as rPS_Co";
            SQL += ",rLK_No AS rLuyKe_No";
            SQL += ",rLK_Co AS rLuyKe_Co";
            SQL += ",rCK_No";
            SQL += ",rCK_Co";
            SQL += " FROM KT_LuyKe";
            SQL += " WHERE iNam=@iNam {1}";
            SQL += " {0} ";
            SQL += " order by sTKNo";
            strSQL = String.Format(SQL, DKTK_TrongBang, DK);
            cmd.CommandText = strSQL;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            //Lấy dữ liệu những tài khoản trong bảng
            dtTrongBang = Connection.GetDataTable(cmd);




            //Lấy Số dự đầu kỳ của tháng là số dư cuối kỳ của tháng-1 tài khoản trong bảng
            //if(CommonFunction.IsNumeric(iThang)
            strSQL = "SELECT sTKNo ,ISNULL(rCK_No,0.0) AS rCK_No,ISNULL(rCK_Co,0.0) AS rCK_Co FROM KT_LuyKe WHERE iThang=@iThang AND iNam=@iNam";
            strSQL += " {0} ";
            strSQL += " order by sTKNo";
            cmd = new SqlCommand(String.Format(strSQL, DKTK_TrongBang, DK));
            cmd.Parameters.AddWithValue("@iThang", sDauKy);
            cmd.Parameters.AddWithValue("@iNam", iNam);

            //Lấy Số dự đầu kỳ của tháng là số dư cuối kỳ của tháng-1 tài khoản trong bảng
            DataTable dtDK_TrongBang = Connection.GetDataTable(cmd);
            cmd.CommandText = String.Format(strSQL, DKTK_NgoaiBang);
            DataTable dtDK_NgoaiBang = Connection.GetDataTable(cmd);

            //Lấy Số dự đầu kỳ của tháng là số dư cuối kỳ của tháng-1 tài khoản trong bảng
            //if(CommonFunction.IsNumeric(iThang)
            strSQL = "SELECT sTKNo ,ISNULL(rCK_No,0.0) AS rCK_No,ISNULL(rCK_Co,0.0) AS rCK_Co FROM KT_LuyKe WHERE iThang=@iThang AND iNam=@iNam";
            strSQL += " {0} ";
            strSQL += " order by sTKNo";
            cmd = new SqlCommand(String.Format(strSQL, DKTK_TrongBang, DK));
            cmd.Parameters.AddWithValue("@iThang", sCuoiKy);
            cmd.Parameters.AddWithValue("@iNam", iNam);

            //Lấy Số dự cuối kỳ của tháng tài khoản trong bảng  
            DataTable dtCK_TrongBang = Connection.GetDataTable(cmd);
            cmd.CommandText = String.Format(strSQL, DKTK_NgoaiBang);
            DataTable dtCK_NgoaiBang = Connection.GetDataTable(cmd);

            //Lấy dữ liệu những tài khoản ngoài bảng là những tài khoản đầu 0
            strSQL = String.Format(SQL, DKTK_NgoaiBang, DK);
            cmd.CommandText = strSQL;
            DataTable dtNgoaiBang = Connection.GetDataTable(cmd);
            cmd.Dispose();



            #region Edit số phát sinh lũy kế đầu kỳ cuối kỳ tài khoản trong bảng
            DataRow Rtb, RlkCo, RlkNo, RCK;
            for (int i = 0; i < dtTrongBang.Rows.Count; i++)
            {
                Rtb = dtTrongBang.Rows[i];
                String sTKNo = Convert.ToString(Rtb["sTKNo"]).Trim();
                //Lấy số dư cuối kỳ của Thang-1 thành số dư đầu kỳ của Thang
                for (int j = 0; j < dtDK_TrongBang.Rows.Count; j++)
                {
                    RCK = dtDK_TrongBang.Rows[j];
                    String sTKNo_CK = Convert.ToString(RCK["sTKNo"]).Trim();
                    if (sTKNo == sTKNo_CK)
                    {
                        Rtb["rSoDuDauKy_No"] = RCK["rCK_No"];
                        Rtb["rSoDuDauKy_Co"] = RCK["rCK_Co"];
                        break;
                    }
                }
            }

            #endregion

            for (int i = 0; i < dtNgoaiBang.Rows.Count; i++)
            {
                Rtb = dtNgoaiBang.Rows[i];

                String sTKNo = Convert.ToString(Rtb["sTKNo"]).Trim();
                //Lấy số dư cuối kỳ của Thang-1 thành số dư đầu kỳ của Thang
                for (int j = 0; j < dtDK_NgoaiBang.Rows.Count; j++)
                {
                    RCK = dtDK_NgoaiBang.Rows[j];

                    String sTKNo_CK = Convert.ToString(RCK["sTKNo"]).Trim();
                    if (sTKNo == sTKNo_CK)
                    {
                        Rtb["rSoDuDauKy_No"] = RCK["rCK_No"];
                        Rtb["rSoDuDauKy_Co"] = RCK["rCK_Co"];
                        break;
                    }
                }

            }

            int cs = 0, cs1 = 0;
            String TK, TK1;
            String[] arrTruongTien = { "rSoDuDauKy_No", "rSoDuDauKy_Co", "rPS_No", "rPS_Co", "rLuyKe_No", "rLuyKe_Co", "rCK_No", "rCK_Co" };
            Boolean ok = false, bHienThi = false;
            for (int i = 0; i < dtDSTK_TB.Rows.Count; i++)
            {
                TK = Convert.ToString(dtDSTK_TB.Rows[i]["sTKNo"]).Trim();
                bHienThi = Convert.ToBoolean(dtDSTK_TB.Rows[i]["bHienThi"]);
                ok = false;

                for (int j = cs; j < dtTrongBang.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtTrongBang.Rows[j]["sTKNo"]).Trim();
                    if (TK == TK1)
                    {
                        //if (bHienThi)
                        //{
                        for (int c = 0; c < arrTruongTien.Length; c++)
                        {
                            dtDSTK_TB.Rows[i][arrTruongTien[c]] = dtTrongBang.Rows[j][arrTruongTien[c]];
                        }
                        //}
                        //else
                        //{
                        //    dtDSTK_TB.Rows.RemoveAt(i);
                        //    if (i > 0) i = i - 1;
                        //}
                        cs = j + 1;
                        ok = true;
                        break;
                    }
                }
                if (ok == false && bHienThi == false)
                {
                    dtDSTK_TB.Rows.RemoveAt(i);
                    if (i > 0) i = i - 1;
                }
            }
            cs = 0;
            cs1 = 0;
            for (int i = 0; i < dtDSTK_NB.Rows.Count; i++)
            {
                TK = Convert.ToString(dtDSTK_NB.Rows[i]["sTKNo"]).Trim();
                for (int j = cs; j < dtNgoaiBang.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtNgoaiBang.Rows[j]["sTKNo"]).Trim();
                    if (TK == TK1)
                    {
                        for (int c = 0; c < arrTruongTien.Length; c++)
                        {
                            dtDSTK_NB.Rows[i][arrTruongTien[c]] = dtNgoaiBang.Rows[j][arrTruongTien[c]];
                        }
                        cs = j + 1;
                        break;
                    }
                }
            }
            data _data = new data();
            _data.dt_TrongBang = dtDSTK_TB;
            _data.dt_NgoaiBang = dtDSTK_NB;
            return _data;
        }

        public JsonResult CheckKhoaSo(String iThang,String iNam)
        {
            String[] arrThang;
            SqlCommand cmd=new SqlCommand();
            switch (iThang)
            {
                case "13":
                    {
                        arrThang = "1,2,3".Split(',');
                    }
                    break;
                case "14":
                    {
                        arrThang = "4,5,6".Split(',');
                    }
                    break;
                case "15":
                    {
                        arrThang = "7,8,9".Split(',');
                    }
                    break;
                case "16":
                    {
                        arrThang = "10,11,12".Split(',');
                    }
                    break;
                case "17":
                    {
                        arrThang = "1,2,3,4,5,6".Split(',');
                    }
                    break;
                case "18":
                    {
                        arrThang = "7,8,9,10,11,12".Split(',');
                    }
                    break;
                case "19":
                    {
                        arrThang = "1,2,3,4,5,6,7,8,9,10,11,12".Split(',');
                    }
                    break;
                default:
                    arrThang=iThang.Split(',');
                    break;
            }
            String DK = "";
            for (int i = 0; i < arrThang.Length; i++)
            {
                if(DK!="") DK+=" OR ";
                DK += "iThang=@iThang" + i;
                cmd.Parameters.AddWithValue("@iThang"+i, arrThang[i]);
            }
            if (DK != "")
                DK = " AND (" + DK + ")";
            String SQL = String.Format("SELECT distinct iThang FROM KT_LuyKe WHERE iNam=@iNam {0} ORDER BY iThang ",DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            Boolean CoThangKhoaSo=false;
            String sThangChuaKhoaSo="";
            if (iThang != "")
            {
                for (int i = 0; i < arrThang.Length; i++)
                {
                    CoThangKhoaSo = false;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (arrThang[i] == Convert.ToString(dt.Rows[j]["iThang"]))
                        {
                            CoThangKhoaSo = true;
                        }
                    }
                    if (CoThangKhoaSo == false)
                    {
                        sThangChuaKhoaSo += arrThang[i] + ",";
                    }
                }
            }
            return Json(sThangChuaKhoaSo, JsonRequestBehavior.AllowGet);
            
        }
    }
}
