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

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToan_TongHop_PhongBan_DonViController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_ChiTiet = "/Report_ExcelFrom/DuToan/rptDuToan_TongHop_PhongBan_DonVi_ChiTiet.xls";
        private const String sFilePath_TongHop = "/Report_ExcelFrom/DuToan/rptDuToan_TongHop_PhongBan_DonVi_TongHop.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHop_PhongBan_DonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public static DataTable rptDuToan_TongHop_PhongBan_DonVi(String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String LoaiTongHop)
        {
            SqlCommand cmd = new SqlCommand();
            String DKPhongBan = "", DKDonVi = "", DK = "", DKDot = "";
            //dk LNS
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = "-100";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            else
            {
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    DK += "sLNS=@sLNS" + i;
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                    if (i < arrLNS.Length - 1)
                    {
                        DK += " OR ";
                    }
                }
            }

            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }

            //dk Dot
            if (!String.IsNullOrEmpty(iID_MaDot) && iID_MaDot != "-1")
            {
                DK += " AND CONVERT(VARCHAR(24),dNgayChungTu,105)=@MaDot ";
                cmd.Parameters.AddWithValue("@MaDot", iID_MaDot);
            }

            //dk phong ban
            if (iID_MaPhongBan != "-1")
            {
                DKPhongBan += " AND iID_MaPhongBan=@MaPhongBan ";
                cmd.Parameters.AddWithValue("@MaPhongBan", iID_MaPhongBan);
            }
            
            //dk donvi
            if (LoaiTongHop == "ChiTiet")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
                {
                    DKDonVi += " AND iID_MaDonVi=@iID_MaDonVi ";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();
                String[] arrDonVi = iID_MaDonVi.Split(',');
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DKDonVi += " iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DKDonVi += " OR ";
                }
                if (String.IsNullOrEmpty(DKDonVi) == false)
                    DKDonVi = " AND (" + DKDonVi + ") ";
            }

            DKDonVi += ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan += ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            int DVT = 1000;

            String SQL =
                String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                                ,rTuChi=SUM(rTuChi)/{3}
                                ,rHienVat=SUM(rHienVat)/{3}
                                ,rHangNhap=SUM(rHangNhap)/{3}
                                ,rHangMua=SUM(rHangMua)/{3}
                                ,rPhanCap=SUM(rPhanCap)/{3}
                                ,rDuPhong=SUM(rDuPhong)/{3}
                                FROM DT_ChungTuChiTiet where iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaChungTu IN
                                                                            (SELECT iID_MaChungTu FROM DT_ChungTu WHERE iTrangThai=1 {0} ) {1} {2} {4}
                                GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                                HAVING SUM(rTuChi)<>0 
                                    OR SUM(rHangNhap)<>0 
                                    OR SUM(rHienVat)<>0  
                                    OR SUM(rHangMua)<>0  
                                    OR SUM(rPhanCap)<>0 
                                    OR SUM(rDuPhong)<>0", DK, DKDonVi, DKPhongBan, DVT, DKDot);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dtAll = Connection.GetDataTable(cmd);

            return dtAll;
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iID_MaDot = Request.Form[ParentID + "_iID_MaDot"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String LoaiTongHop = Request.Form[ParentID + "_LoaiTongHop"];


            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iID_MaDot"] = iID_MaDot;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_TongHop_PhongBan_DonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String LoaiTongHop)
        {
            DataTable dtDonVi = new DataTable();
            dtDonVi = rptDuToan_TongHop_PhongBan_DonVi(MaND, sLNS, iID_MaDonVi, iID_MaDot, iID_MaPhongBan, LoaiTongHop);
            DataTable data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            fr.AddTable("dtDonVi", dtDonVi);
            dtDonVi.Dispose();
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK", "sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS", "sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);



            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
        }

        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String LoaiTongHop)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, " rptDuToan_1040100_BoSung");
            LoadData(fr, MaND, sLNS, iID_MaDonVi, iID_MaDot, iID_MaPhongBan, LoaiTongHop);
            String Nam = ReportModels.LayNamLamViec(MaND);
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            //lay ten phong ban
            //if (iID_MaPhongBan != "-1")
            //{
            //    String sTenPhongBan = "B" + iID_MaPhongBan;
            //    fr.SetValue("sTenPhongBan", sTenPhongBan);
            //}
            //else
            //{
            //    String sTenPhongBan = "";
            //    fr.SetValue("sTenPhongBan", sTenPhongBan);
            //}

            fr.SetValue("Nam", Nam);
            if (LoaiTongHop == "ChiTiet")
            {
                fr.SetValue("sTenDonVi", sTenDonVi);
                fr.SetValue("sTenPhongBan", "");
            }
            else
            {
                fr.SetValue("sTenDonVi", "");
                if (iID_MaPhongBan != "-1")
                {
                    String sTenPhongBan = "Tổng hợp theo B" + iID_MaPhongBan;
                    fr.SetValue("sTenPhongBan", sTenPhongBan);
                }
                else
                {
                    String sTenPhongBan = "";
                    fr.SetValue("sTenPhongBan", sTenPhongBan);
                }
            }

            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("madot", iID_MaDot);
            fr.Run(Result);
            return Result;
        }

            public ActionResult ViewPDF(String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String LoaiTongHop)
            {
                HamChung.Language();
                String sDuongDan = "";
                if (LoaiTongHop == "ChiTiet")
                {
                    sDuongDan = sFilePath_ChiTiet;
                }
                else
                {
                    sDuongDan = sFilePath_TongHop;
                }

                ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iID_MaDonVi, iID_MaDot, iID_MaPhongBan, LoaiTongHop);
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
            }

            public clsExcelResult ExportToExcel(String MaND, String sLNS, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String LoaiTongHop)       
            {
                HamChung.Language();
                String sDuongDan = "";
                    if (LoaiTongHop == "ChiTiet")
                {
                    sDuongDan = sFilePath_ChiTiet;
                }
                else
                {
                    sDuongDan = sFilePath_TongHop;
                }
                clsExcelResult clsResult = new clsExcelResult();
                ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iID_MaDonVi, iID_MaDot, iID_MaPhongBan, LoaiTongHop);
                using (MemoryStream ms = new MemoryStream())
                {
                    xls.Save(ms);
                    ms.Position = 0;
                    clsResult.ms = ms;
                    clsResult.FileName = "DuToan_1020000_TungDonVi.xls";
                    clsResult.type = "xls";
                    return clsResult;
                }
            }

            public JsonResult Ds_DonVi(String ParentID, String iID_MaDonVi, String iID_MaDot, String iID_MaPhongBan, String sLNS)
            {
                String MaND = User.Identity.Name;
                DataTable dt = DuToan_ReportModels.getdtPhongBan_LNS_DonVi(MaND, iID_MaDot, iID_MaPhongBan, sLNS);

                if (String.IsNullOrEmpty(iID_MaDonVi))
                {
                    iID_MaDonVi = Guid.Empty.ToString();
                }
                String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
                DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
                String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
                return Json(strDonVi, JsonRequestBehavior.AllowGet);
            }
           
        }
 }
