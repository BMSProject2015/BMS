using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.NguoiCoCong
{
    public class rptNCC_TCKK_63_8Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";     
        public ActionResult Index(String Loai="",String KhoGiay="")
        {
            String sFilePath = "";
            if (Loai == "1")
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_TCKK_63_8.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String Loai = Convert.ToString(Request.Form[ParentID + "_Loai"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["sLNS"] = sLNS;
            ViewData["Loai"] = Loai;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_TCKK_63_8.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// CreateReport
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="TongHop"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String sLNS, String iID_MaDonVi, String Loai, String iID_MaTrangThaiDuyet, String UserName,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV;
            if (Loai=="1")
            {

                if (!String.IsNullOrEmpty(iID_MaDonVi))
                {
                    TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
                }
                else
                {
                    TenDV = "";
                }
            }
            else
            {
                TenDV = "";
            }
            if (Loai == "2")
            {
                TenDV = "";
            }
           
            String TenLNS = "";
            DataTable dt = ReportModels.MoTa(sLNS);
            if (dt.Rows.Count > 0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            long Tong = 0;
            DataTable dtTien = NCC_TCKK_63_8(iID_MaDonVi, sLNS, Loai, iID_MaTrangThaiDuyet, UserName,KhoGiay);
            for (int i = 0; i < dtTien.Rows.Count; i++)
            {
                if (dtTien.Rows[i]["ThucHien"].ToString() != "")
                {
                    Tong += long.Parse(dtTien.Rows[i]["ThucHien"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(Tong).ToString();
            dtTien.Dispose();
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptNCC_TCKK_63_8");
            LoadData(fr, iID_MaDonVi, sLNS, Loai, iID_MaTrangThaiDuyet, UserName,KhoGiay);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(UserName));
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1).ToString());
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToString());
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("TenDV", TenDV);
                fr.SetValue("TenLNS", TenLNS);
                fr.SetValue("Tong", Tien);
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="TongHop"></param>
        private void LoadData(FlexCelReport fr, String sLNS, String iID_MaDonVi, String Loai, String iID_MaTrangThaiDuyet, String UserName,String KhoGiay)
        {

            DataTable data = NCC_TCKK_63_8(sLNS, iID_MaDonVi, Loai, iID_MaTrangThaiDuyet, UserName,KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            if (dtMuc.Rows.Count == 0)
            {
                DataRow dr = dtMuc.NewRow();
                dtMuc.Rows.InsertAt(dr, 0);
            }
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// ExportToPDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="TongHop"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String sLNS, String iID_MaDonVi, String Loai, String iID_MaTrangThaiDuyet, String UserName,String KhoGiay)
        {
            String sFilePath = "";
            if (Loai == "1")
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), sLNS, iID_MaDonVi, Loai, iID_MaTrangThaiDuyet, UserName,KhoGiay);
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
        /// ExportToExcel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="TongHop"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String sLNS, String iID_MaDonVi, String Loai, String iID_MaTrangThaiDuyet, String UserName, String KhoGiay)
        {
            String sFilePath = "";
            if (Loai == "1")
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), sLNS, iID_MaDonVi, Loai, iID_MaTrangThaiDuyet, UserName,KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "NCC_TCKK_63_8.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sLNS"></param>
        /// <param name="TongHop"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String sLNS, String iID_MaDonVi, String Loai, String iID_MaTrangThaiDuyet, String UserName, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (Loai == "1")
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            else
            {
                if (KhoGiay == "1")
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8.xls";
                }
                else
                {
                    sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_63_8_A3.xls";
                }
            }
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), sLNS, iID_MaDonVi, Loai, iID_MaTrangThaiDuyet, UserName,KhoGiay);
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

        public static DataTable NCC_TCKK_63_8(String iID_MaDonVi, String sLNS, String Loai, String iID_MaTrangThaiDuyet, String UserName,String KhoGiay)
        {
            DataTable dt = new DataTable();
           
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
           
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                DKDuyet = " ";
            }
            String DKDuyet_ChiTieu = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet_ChiTieu = "AND PB_PhanBoChiTiet.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                DKDuyet_ChiTieu = " ";
            }
            if (Loai == "2")
            {
                DataTable dtDonVi = dtDanhsach_DonVi(sLNS, iID_MaTrangThaiDuyet, UserName);
                String DK_DonVi = "";
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DK_DonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < dtDonVi.Rows.Count - 1)
                            DK_DonVi += " OR ";
                    }
                }
                String SQL1 = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi) as ThucHien
                                        FROM NCC_ChungTuChiTiet as CT
                                        WHERE sLNS=@sLNS {1} AND sNG<>'' AND iTrangThai=1 AND iLoai=2  {0} AND ({2})
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rTuChi)<>0", DKDuyet,ReportModels.DieuKien_NganSach(UserName),DK_DonVi);
                SqlCommand cmd1 = new SqlCommand(SQL1);
                cmd1.Parameters.AddWithValue("@sLNS", sLNS);
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmd1.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }
                }
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmd1.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong));
                }
                dt = Connection.GetDataTable(cmd1);
                cmd1.Dispose();

                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE sLNS=@sLNS  AND
                                                        sNG<>'' AND
                                                        YEAR(dNgayDotPhanBo)=@NamLamViec {1} AND
                                                        PB_PhanBoChiTiet.iTrangThai=1 
                                                        {0} AND ({2})
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", DKDuyet_ChiTieu,ReportModels.DieuKien_NganSach(UserName,"PB_PhanBoChiTiet"),DK_DonVi);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", ReportModels.LayNamLamViec(UserName));
                cmdChiTieu.Parameters.AddWithValue("@sLNS", sLNS);
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }
                }
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                }
                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                //ghep 2 dt
                DataRow addR, R2;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
                String[] arrCol = sCol.Split(',');
                dt.Columns.Add("ChiTieu", typeof(Decimal));
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"NguonNS={7} AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"]
                                                      );
                    DataRow[] R = dt.Select(xauTruyVan);

                    if (R == null || R.Length == 0)
                    {
                        addR = dt.NewRow();
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
                        }
                        dt.Rows.Add(addR);
                    }
                    else
                    {
                        foreach (DataRow R1 in dtChiTieu.Rows)
                        {

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                Boolean okTrung = true;
                                R2 = dt.Rows[j];

                                for (int c = 0; c < arrCol.Length - 2; c++)
                                {
                                    if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                    {
                                        okTrung = false;
                                        break;
                                    }
                                }
                                if (okTrung)
                                {
                                    dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

                                    break;
                                }

                            }
                        }

                    }

                }
                //sắp xếp datatable sau khi ghép
                DataView dv = dt.DefaultView;
                dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG";
                dt = dv.ToTable();
                return dt;
            }
            else
            {
                String SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi) as ThucHien
                                        FROM NCC_ChungTuChiTiet as CT
                                        WHERE sLNS=@sLNS {1} AND 
                                              sNG<>0 AND  iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi
                                              AND  iLoai=2  {0}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(rTuChi)<>0", DKDuyet,ReportModels.DieuKien_NganSach(UserName));
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong));
                }
                dt = Connection.GetDataTable(cmd);            
                cmd.Dispose();
                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE sLNS=@sLNS  AND
                                                        sNG<>'' AND
                                                        YEAR(dNgayDotPhanBo)=@NamLamViec {1}
                                                        AND  PB_PhanBoChiTiet.iTrangThai=1 AND
                                                        PB_PhanBoChiTiet.iID_MaDonVi=@iID_MaDonVi 
                                                        {0} 
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", DKDuyet_ChiTieu,ReportModels.DieuKien_NganSach(UserName,"PB_PhanBoChiTiet"));
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", ReportModels.LayNamLamViec(UserName));
                cmdChiTieu.Parameters.AddWithValue("@sLNS", sLNS);
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                }
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                //ghep 2 dt
                DataRow addR, R2;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
                String[] arrCol = sCol.Split(',');
                dt.Columns.Add("ChiTieu", typeof(Decimal));
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"NguonNS={7} AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"]
                                                      );
                    DataRow[] R = dt.Select(xauTruyVan);

                    if (R == null || R.Length == 0)
                    {
                        addR = dt.NewRow();
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
                        }
                        dt.Rows.Add(addR);
                    }
                    else
                    {
                        foreach (DataRow R1 in dtChiTieu.Rows)
                        {

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                Boolean okTrung = true;
                                R2 = dt.Rows[j];

                                for (int c = 0; c < arrCol.Length - 2; c++)
                                {
                                    if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                    {
                                        okTrung = false;
                                        break;
                                    }
                                }
                                if (okTrung)
                                {
                                    dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

                                    break;
                                }

                            }
                        }

                    }

                }
                //sắp xếp datatable sau khi ghép
                DataView dv = dt.DefaultView;
                dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG";
                dt = dv.ToTable();
               
                return dt;
            }
        }
       
        [HttpGet]

        public JsonResult ds_DonVi(String ParentID , String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String UserName)
        {
            return Json(obj_DonVi(ParentID, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,UserName), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonVi(String ParentID, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String UserName)
        {
            DataTable dtDonvi = dtDanhsach_DonVi(sLNS, iID_MaTrangThaiDuyet,UserName);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strDonVi = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return strDonVi;

        }
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable dtDanhsach_DonVi(String sLNS, String iID_MaTrangThaiDuyet,String UserName)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = string.Format(@"SELECT 
                                        DV.iID_MaDonVi,DV.iID_MaDonVi+'-'+DV.sTen AS sTen
                                        FROM NCC_ChungTuChiTiet AS NCC
                                        INNER JOIN NS_DonVi AS DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE NCC.iTrangThai=1 {1} AND sLNS=@sLNS  {0} AND iLoai=2 AND rTuChi>0
                                        GROUP BY DV.iID_MaDonVi,DV.sTen
                                        ORDER BY DV.iID_MaDonVi,DV.sTen", iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(UserName,"NCC"));
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
    }


}