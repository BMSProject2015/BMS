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
    public class rptQuyetToan_TongHop_LNSController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS.xls";
        private const String sFilePath_To2 = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS_To2.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS.aspx";
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
        public static DataTable rptQuyetToan_TongHop_LNS(String MaND, String sLNS, String iThang_Quy, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dtDonVi = QuyetToan_ReportModels.dtLNS_DonVi(iThang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
            }
            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }
            if (iID_MaPhongBan != "-1")
            {
                DK += " AND iID_MaPhongBan = @MaPhongBan";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }
            dtDonVi.Dispose();
            String DKSUMDonVi = "", DKCASEDonVi = "", DKHAVINGDonVi = "";
            int SoCotTrang1 = 4;
            int SoCotTrang2 = 7;
            if (MaTo == "1")
            {
                if (arrDonVi.Length < SoCotTrang1)
                {
                    int a = SoCotTrang1 - arrDonVi.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDonVi += ",-1";
                    }
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                for (int i = 1; i <= SoCotTrang1; i++)
                {

                    DKSUMDonVi += ",SUM(DonVi" + i + ") AS DonVi" + i;
                    DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                    DKCASEDonVi += " ,DonVi" + i + "=SUM(CASE WHEN (iID_MaDonVi=@MaDonVi" + i + " AND {1}) THEN {0} ELSE 0 END)";
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i-1]);
                }
                DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", "iThang_Quy=@iThang_Quy");
            }
            else
            {
                if (arrDonVi.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(MaTo) - 1)))
                {
                    int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(MaTo) - 1) - arrDonVi.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDonVi += ",-1";
                    }
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                int tg = Convert.ToInt16(MaTo) - 2;
                int x = 1;
                for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                {
                    DKSUMDonVi += ",SUM(DonVi" + x + ") AS DonVi" + x;
                    DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                    DKCASEDonVi += " ,DonVi" + x + "=SUM(CASE WHEN (iID_MaDonVi=@MaDonVi" + x + " AND {1}) THEN {0} ELSE 0 END)";
                    cmd.Parameters.AddWithValue("@MaDonVi" + x, arrDonVi[i]);
                    x++;
                    
                }
                DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi", "iThang_Quy=@iThang_Quy");
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
            }
            if (iID_MaNamNganSach == "2")
            {
                DK += " AND iID_MaNamNganSach IN (2) ";
            }
            else if (iID_MaNamNganSach == "1")
            {
                DK += " AND iID_MaNamNganSach IN (1) ";
            }
            else
            {
                DK += " AND iID_MaNamNganSach IN (1,2) ";
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            String SQL = "";
//            if (MaTo == "1")
//            {
//                SQL =
//                   String.Format(@"SELECT 
//                                        SUBSTRING(sLNS,1,1) as sLNS1,
//                                        SUBSTRING(sLNS,1,3) as sLNS3,
//                                        SUBSTRING(sLNS,1,5) as sLNS5,
//                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
//                                        SUM(rTuChi) as rTuChi,
//                                        SUM(CongTrongKy) as CongTrongKy,
//                                        SUM(DenKyNay) as DenKyNay
//                                        --DKSUM
//                                        {4}
//                                    FROM
//                                    (
//
//                                        SELECT 
//                                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
//                                            ,rTuChi=0
//                                            ,CongTrongKy=SUM(CASE WHEN iThang_Quy=@iThang_Quy THEN rTuChi ELSE 0 END)
//                                            ,DenKyNay=SUM(CASE WHEN iThang_Quy<=@iThang_Quy THEN rTuChi ELSE 0 END)
//                                            {3}
//                                        FROM 
//                                            QTA_ChungTuChiTiet
//                                        WHERE 
//                                            iTrangThai=1 AND 
//                                            iNamLamViec=@iNamLamViec AND 
//                                            iThang_Quy<=@iThang_Quy {0} {1} {2}
//                                        GROUP BY 
//                                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,iThang_Quy
//                                    ) a
//                                    GROUP BY  
//                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
//                                    HAVING 
//                                        SUM(rTuChi)<>0 OR 
//                                        SUM(CongTrongKy) <>0 OR 
//                                        SUM(DenKyNay)<>0 
//                                        --DKHaVing 
//                                        {5} ", DK, DKDonVi, DKPhongBan, DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi);
//            }
//            else
//            {
                SQL =
                    String.Format(@"SELECT 
                                        SUBSTRING(sLNS,1,1) as sLNS1,
                                        SUBSTRING(sLNS,1,3) as sLNS3,
                                        SUBSTRING(sLNS,1,5) as sLNS5,
                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
                                        SUM(rTuChi) as rTuChi,
                                        SUM(CongTrongKy) as CongTrongKy,
                                        SUM(DenKyNay) as DenKyNay
                                        --DKSUM
                                        {4}
                                    FROM
                                    (
                                        SELECT 
                                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                            ,rTuChi=0
                                            ,CongTrongKy=SUM(CASE WHEN iThang_Quy=@iThang_Quy THEN rTuChi ELSE 0 END)
                                            ,DenKyNay=SUM(CASE WHEN iThang_Quy<=@iThang_Quy THEN rTuChi ELSE 0 END)
                                            {3}
                                        FROM 
                                            QTA_ChungTuChiTiet
                                        WHERE 
                                            iTrangThai=1 AND 
                                            iNamLamViec=@iNamLamViec AND 
                                            iThang_Quy<=@iThang_Quy {0} {1} {2}
                                        GROUP BY 
                                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,iThang_Quy
                                    ) a
                                    GROUP BY  
                                        sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                    HAVING 
                                        SUM(rTuChi)<>0 OR 
                                        SUM(CongTrongKy) <>0 OR 
                                        SUM(DenKyNay)<>0 
                                        --DKHaVing 
                                        {5} ", DK, DKDonVi, DKPhongBan, DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi);
            //}
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
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
            String sLNS = Request.Form["sLNS"];
            String MaTo = Request.Form["MaTo"];
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["MaTo"] = MaTo;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_TongHop_LNS");

            LoadData(fr, MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach, iID_MaPhongBan);
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
            String sTenDonVi = "B" + iID_MaPhongBan;
            if (iID_MaPhongBan == "-1")
            {
                sTenDonVi = "";
            }
            DataTable dtDonVi = QuyetToan_ReportModels.dtLNS_DonVi(iThang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);
           //Lay ten don vi
            
            String iID_MaDonVi = "";
           for (int i = 0; i < dtDonVi.Rows.Count; i++)
           {
               iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
           }
           if (!String.IsNullOrEmpty(iID_MaDonVi))
           {
               iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
           }
           String[] arrDonVi = iID_MaDonVi.Split(',');
           String DonVi = iID_MaDonVi;
           String[] TenDV;
           if (MaTo == "1")
           {
               if (arrDonVi.Length < 4)
               {
                   int a1 = 4 - arrDonVi.Length;
                   for (int i = 0; i < a1; i++)
                   {
                       DonVi += ",-1";
                   }
               }
               arrDonVi = DonVi.Split(',');
               TenDV = new String[4];
               for (int i = 0; i < 4; i++)
               {
                   if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                   {
                       TenDV[i] = DonViModels.Get_TenDonVi(arrDonVi[i]);
                   }
               }

               for (int i = 1; i <= TenDV.Length; i++)
               {
                   fr.SetValue("DonVi" + i, TenDV[i - 1]);
               }
           }
           else
           {
               if (arrDonVi.Length < 4 + 7 * (Convert.ToInt16(MaTo) - 1))
               {
                   int a1 = 4 + 7 * (Convert.ToInt16(MaTo) - 1) - arrDonVi.Length;
                   for (int i = 0; i < a1; i++)
                   {
                       DonVi += ",-1";
                   }
                   arrDonVi = DonVi.Split(',');
               }
               TenDV = new String[7];
               int x = 1;
               for (int i = 4 + 7 * ((Convert.ToInt16(MaTo) - 2)); i < 4 + 7 * ((Convert.ToInt16(MaTo) - 1)); i++)
               {
                   if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                   {
                       TenDV[x - 1] = DonViModels.Get_TenDonVi(arrDonVi[i]);
                       x++;
                   }
               }

               for (int i = 1; i <= TenDV.Length; i++)
               {
                   fr.SetValue("DonVi" + i, TenDV[i - 1]);
               }
           }
            dtDonVi.Dispose();

            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));

            fr.SetValue("ToSo", MaTo);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data= new DataTable();

            data = rptQuyetToan_TongHop_LNS(MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS1,sLNS3,sLNS5,sLNS", "sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "sLNS1,sLNS3,sLNS5", "sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (MaTo == "1")
                sDuongDan = sFilePath;
            else
                sDuongDan = sFilePath_To2;


            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach, iID_MaPhongBan);
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

        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iThang_Quy, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";

            if (MaTo == "1")
                sDuongDan = sFilePath;
            else
                sDuongDan = sFilePath_To2;
           
            
           
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach,iID_MaPhongBan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "TongHopTinhHinhQuyetToanNganSach_LNS.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String Thang_Quy, String MaTo, String sLNS, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            DataTable dt = QuyetToan_ReportModels.dtTo_LNS(Thang_Quy, iID_MaNamNganSach, MaND, sLNS,iID_MaPhongBan);

            if (String.IsNullOrEmpty(MaTo))
            {
                MaTo = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/To_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, MaTo, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

