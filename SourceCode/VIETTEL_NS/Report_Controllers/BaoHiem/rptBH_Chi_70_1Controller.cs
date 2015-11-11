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


namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_Chi_70_1Controller : Controller
    {
        //
        // GET: /rptBH_Chi_70_1/
        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay = "")
        {

            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau_A4.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_70_1.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID)
        {

            String ThangQuy = "";
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID + "_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "1")
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                ThangQuy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iALL = Convert.ToString(Request.Form[ParentID + "_iALL"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String BoDongTrong = Convert.ToString(Request.Form[ParentID + "_BoDongTrong"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);

            ViewData["ThangQuy"] = ThangQuy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iALL"] = iALL;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["BoDongTrong"] = BoDongTrong;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_70_1.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }


        public ExcelFile CreateReport(String path, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iALL, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String LoaiThangQuy = "";
            switch (LoaiThang_Quy)
            {
                case "0":
                    LoaiThangQuy = "Tháng";
                    break;
                case "1":
                    LoaiThangQuy = "Quý";
                    break;
                case "2":
                    LoaiThangQuy = "Năm";
                    break;
            }
            String sTenDonVi = "";
            if (iALL != "on")
            {
                String SQL = "SELECT iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=@iID_MaDonVi";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                sTenDonVi = Connection.GetValueString(cmd, "");
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_Chi_70_1");
            LoadData(fr, MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iALL, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
            fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("ThangQuy", ThangQuy);
            fr.SetValue("LoaiThangQuy", LoaiThangQuy);
            fr.SetValue("TenDV", sTenDonVi);
            fr.Run(Result);
            return Result;

        }
        public DataTable rptBH_Chi_70_1(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iALL, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {

            #region tao dt BH_Chi
            DataTable dt = new DataTable();
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
                DKDuyet = "";
            }
            String DKThang = "";
            String DKLKe = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "iThang_Quy between 1 and 3";
                        DKLKe = "iThang_Quy <= 3";
                        break;
                    case "2": DKThang = "iThang_Quy between 4 and 6";
                        DKLKe = "iThang_Quy <= 6";
                        break;
                    case "3": DKThang = "iThang_Quy between 7 and 9";
                        DKLKe = "iThang_Quy <= 9";
                        break;
                    case "4": DKThang = "iThang_Quy between 10 and 12";
                        DKLKe = "iThang_Quy <= 12";
                        break;
                }
            }
            else
            {
                DKThang = "iThang_Quy=@ThangQuy";
                DKLKe = "iThang_Quy <=@ThangQuy ";
            }
            String DKDonvi = "";
            //xoa dong trong
            DataTable dtDonVi = HienThiDonViTheoNam(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
            #region Xóa dòng không có dữ liệu
            if (BoDongTrong == "on")
            {
                #region Tất cả đơn vị
                if (iALL == "on")
                {

                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            DKDonvi += " iID_MaDonVi =@iID_MaDonVi" + i;
                            if (i < dtDonVi.Rows.Count - 1)
                                DKDonvi += " OR ";
                        }
                    }

                }
                #endregion
                #region từng đơn vị
                else
                {
                    DKDonvi = "iID_MaDonVi =@iID_MaDonVi";
                }
                #endregion
                String SQL = string.Format(@" SELECT b.sLNS,b.sL,b.sK,b.sM,b.sTM,b.sTTM,b.sNG,b.sTNG,b.sMoTa ,b.bLaHangCha,sum (SLLuyKe) as SLLuyKe,sum (TienLuyKe) as TienLuyKe
                                    ,sum (SLSQ) as SLSQ,sum (TienSQ) as TienSQ,sum (SLQNCN) as SLQNCN,sum (TienQNCN) as TienQNCN,sum (SLCNV) as SLCNV
                                    ,sum (TienCNV) as TienCNV,sum (SLHD) as SLHD,sum (TienLDHD) as TienLDHD,sum (SLHDKhac) as SLHDKhac
                                    ,sum (TienHDKhac) as TienHDKhac,sum (SLHSQ) as SLHSQ,sum (TienHSQ) as TienHSQ
                                    FROM(
                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                                         ,sMoTa=CASE WHEN bLaHangCha=1 THEN UPPER(sMoTa) ELSE sMoTa END
                                         ,bLaHangCha=CASE WHEN bLaHangCha=1 THEN 1 ELSE 0 END
                                        ,SLLuyKe=CASE WHEN {3}  THEN SUM(rSLSQ) + SUM(rSLQNCN) +SUM(rSLCNV)+SUM(rSLHD)+SUM(rSLHD_Khac)+SUM(rSLHSQ_CS) ELSE 0 END
                                        ,TienLuyKe=CASE WHEN {3}  THEN SUM(rTienSQ) + SUM(rTienQNCN) +SUM(rTienCNV)+SUM(rTienHD)+SUM(rTienHD_Khac)+SUM(rTienHSQ_CS) ELSE 0 END
                                        ,SLSQ= CASE WHEN {2} THEN SUM(rSLSQ) ELSE 0 END
                                        ,TienSQ= CASE WHEN {2} THEN SUM(rTienSQ) ELSE 0 END 
                                        ,SLQNCN= CASE WHEN {2} THEN SUM(rSLQNCN) ELSE 0 END   
                                        ,TienQNCN= CASE WHEN {2} THEN SUM(rTienQNCN) ELSE 0 END  
                                        ,SLCNV= CASE WHEN {2} THEN SUM(rSLCNV) ELSE 0 END  
                                        ,TienCNV= CASE WHEN {2} THEN SUM(rTienCNV) ELSE 0 END  
                                        ,SLHD= CASE WHEN {2} THEN SUM(rSLHD) ELSE 0 END  
                                        ,TienLDHD= CASE WHEN {2} THEN SUM(rTienHD) ELSE 0 END  
                                        ,SLHDKhac= CASE WHEN {2} THEN SUM(rSLHD_Khac) ELSE 0 END  
                                        ,TienHDKhac= CASE WHEN {2} THEN SUM(rTienHD_Khac) ELSE 0 END 
                                        ,SLHSQ= CASE WHEN {2} THEN SUM(rSLHSQ_CS) ELSE 0 END  
                                        ,TienHSQ= CASE WHEN {2} THEN SUM(rTienHSQ_CS) ELSE 0 END  
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE  bChi=1 AND iTrangThai=1 {1}  {0}    AND ({4}) AND sTNG<>''
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iThang_Quy
                                        HAVING SUM(rSLSQ)!=0 OR SUM(rTienSQ)!=0 OR SUM(rSLQNCN)!=0
                                        OR SUM(rTienQNCN)!=0 OR SUM(rSLCNV)!=0 
                                        OR SUM(rTienCNV)!=0 OR SUM(rSLHD)!=0 OR SUM(rTienHD)!=0
                                        OR SUM(rSLHD_Khac)!=0 OR SUM(rTienHD_Khac)!=0 OR SUM(rSLHSQ_CS)!=0  OR SUM(rTienHSQ_CS)!=0
                                        OR bLaHangCha>0) as b
                                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa ,bLaHangCha
                                    HAVING sum (SLLuyKe) != 0 or sum (TienLuyKe) !=0
                                    or sum (SLSQ) !=0 or sum (TienSQ) !=0 or sum (SLQNCN) !=0 or sum (TienQNCN) !=0 or sum (SLCNV) !=0
                                    or sum (TienCNV) !=0 or sum (SLHD) !=0 or sum (TienLDHD)!=0 or sum (SLHDKhac) !=0
                                    or sum (TienHDKhac) !=0 or sum (SLHSQ) !=0 or sum (TienHSQ) !=0", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThang, DKLKe, DKDonvi);
                SqlCommand cmd = new SqlCommand(SQL);
                if (iALL == "on")
                {
                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                        }
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion
            #region hiển thị tất cả
            else
            {
                #region Tất cả đơn vị
                if (iALL == "on")
                {
                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            DKDonvi += " iID_MaDonVi =@iID_MaDonVi" + i;
                            if (i < dtDonVi.Rows.Count - 1)
                                DKDonvi += " OR ";
                        }
                    }

                }
                #endregion
                #region từng đơn vị
                else
                {
                    DKDonvi = "iID_MaDonVi =@iID_MaDonVi";
                }
                #endregion
                String SQL1 = string.Format(@" SELECT b.sLNS,b.sL,b.sK,b.sM,b.sTM,b.sTTM,b.sNG,b.sTNG,b.sMoTa ,b.bLaHangCha,sum (SLLuyKe) as SLLuyKe,sum (TienLuyKe) as TienLuyKe
                                    ,sum (SLSQ) as SLSQ,sum (TienSQ) as TienSQ,sum (SLQNCN) as SLQNCN,sum (TienQNCN) as TienQNCN,sum (SLCNV) as SLCNV
                                    ,sum (TienCNV) as TienCNV,sum (SLHD) as SLHD,sum (TienLDHD) as TienLDHD,sum (SLHDKhac) as SLHDKhac
                                    ,sum (TienHDKhac) as TienHDKhac,sum (SLHSQ) as SLHSQ,sum (TienHSQ) as TienHSQ
                                    FROM(
                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                                         ,sMoTa=CASE WHEN bLaHangCha=1 THEN UPPER(sMoTa) ELSE sMoTa END
                                         ,bLaHangCha=CASE WHEN bLaHangCha=1 THEN 1 ELSE 0 END
                                        ,SLLuyKe=CASE WHEN {3}  THEN SUM(rSLSQ) + SUM(rSLQNCN) +SUM(rSLCNV)+SUM(rSLHD)+SUM(rSLHD_Khac)+SUM(rSLHSQ_CS) ELSE 0 END
                                        ,TienLuyKe=CASE WHEN {3}  THEN SUM(rTienSQ) + SUM(rTienQNCN) +SUM(rTienCNV)+SUM(rTienHD)+SUM(rTienHD_Khac)+SUM(rTienHSQ_CS) ELSE 0 END
                                        ,SLSQ= CASE WHEN {2} THEN SUM(rSLSQ) ELSE 0 END
                                        ,TienSQ= CASE WHEN {2} THEN SUM(rTienSQ) ELSE 0 END 
                                        ,SLQNCN= CASE WHEN {2} THEN SUM(rSLQNCN) ELSE 0 END   
                                        ,TienQNCN= CASE WHEN {2} THEN SUM(rTienQNCN) ELSE 0 END  
                                        ,SLCNV= CASE WHEN {2} THEN SUM(rSLCNV) ELSE 0 END  
                                        ,TienCNV= CASE WHEN {2} THEN SUM(rTienCNV) ELSE 0 END  
                                        ,SLHD= CASE WHEN {2} THEN SUM(rSLHD) ELSE 0 END  
                                        ,TienLDHD= CASE WHEN {2} THEN SUM(rTienHD) ELSE 0 END  
                                        ,SLHDKhac= CASE WHEN {2} THEN SUM(rSLHD_Khac) ELSE 0 END  
                                        ,TienHDKhac= CASE WHEN {2} THEN SUM(rTienHD_Khac) ELSE 0 END 
                                        ,SLHSQ= CASE WHEN {2} THEN SUM(rSLHSQ_CS) ELSE 0 END  
                                        ,TienHSQ= CASE WHEN {2} THEN SUM(rTienHSQ_CS) ELSE 0 END  
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE  bChi=1 AND iTrangThai=1 {1}  {0}    AND ({4}) AND bLaHangCha=0
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iThang_Quy
                                       -- HAVING SUM(rSLSQ)!=0 OR SUM(rTienSQ)!=0 OR SUM(rSLQNCN)!=0
                                       -- OR SUM(rTienQNCN)!=0 OR SUM(rSLCNV)!=0 
                                       -- OR SUM(rTienCNV)!=0 OR SUM(rSLHD)!=0 OR SUM(rTienHD)!=0
                                      --  OR SUM(rSLHD_Khac)!=0 OR SUM(rTienHD_Khac)!=0 OR SUM(rSLHSQ_CS)!=0  OR SUM(rTienHSQ_CS)!=0
                                       -- OR bLaHangCha>0
                                            ) as b
                                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa ,bLaHangCha
                                    --HAVING sum (SLLuyKe) != 0 or sum (TienLuyKe) !=0
                                   -- or sum (SLSQ) !=0 or sum (TienSQ) !=0 or sum (SLQNCN) !=0 or sum (TienQNCN) !=0 or sum (SLCNV) !=0
                                  --  or sum (TienCNV) !=0 or sum (SLHD) !=0 or sum (TienLDHD)!=0 or sum (SLHDKhac) !=0
                                  --  or sum (TienHDKhac) !=0 or sum (SLHSQ) !=0 or sum (TienHSQ) !=0", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKThang, DKLKe, DKDonvi);
                SqlCommand cmd1 = new SqlCommand(SQL1);
                if (iALL == "on")
                {
                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            cmd1.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                        }
                    }
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                if (LoaiThang_Quy == "0")
                {
                    cmd1.Parameters.AddWithValue("@ThangQuy", ThangQuy);
                }
                dt = Connection.GetDataTable(cmd1);
                cmd1.Dispose();

            }
            DataTable dtChiTieu = new DataTable();
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "MONTH(dNgayDotPhanBo) <= 3";
                        break;
                    case "2": DKThang = "MONTH(dNgayDotPhanBo) <= 6";
                        break;
                    case "3": DKThang = "MONTH(dNgayDotPhanBo) <= 9";
                        break;
                    case "4": DKThang = "MONTH(dNgayDotPhanBo) <= 12";
                        break;
                }
            }
            else
            {
                DKThang = "MONTH(dNgayDotPhanBo) <=@ThangQuy";
            }

            String SQLChiTieu = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,ChiTieu
                                                        FROM(
                                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iID_MaDotPhanBo,sum(rTuChi) as ChiTieu
                                                        FROM PB_PhanBoChiTiet
                                                        WHERE iTrangThai = 1  AND  ({0}) {1}
                                                        {1} AND sLNS = 2200000 
                                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iID_MaDotPhanBo
                                                        HAVING SUM(rTuChi)<>0) as a
                                                        INNER JOIN (SELECT iID_MaDotPhanBo,dNgayDotPhanBo FROM PB_DotPhanBo WHERE {3}) as b
                                                       ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo 
                                                    ", DKDonvi, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKThang);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            if (LoaiThang_Quy == "0")
            {
                cmdChiTieu.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
            if (iALL == "on")
            {
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }
                }
            }
            else
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();
            #endregion
            #endregion
            #region ghep dt vao dt chi tieu
            DataRow _addR;
            String _sCol = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,ChiTieu,sMoTa";
            String[] _arrCol = _sCol.Split(',');

            dt.Columns.Add("ChiTieu", typeof(Decimal));
            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@" sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                    AND sTTM='{5}' AND sNG='{6}' AND sTNG='{7}'",
                                                  dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                  dtChiTieu.Rows[i]["sK"],
                                                  dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                  dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["sTNG"]
                                                  );
                DataRow[] R = dt.Select(xauTruyVan);
                DataRow R2;

                if (R == null || R.Length == 0)
                {
                    _addR = dt.NewRow();
                    for (int j = 0; j < _arrCol.Length; j++)
                    {
                        _addR[_arrCol[j]] = dtChiTieu.Rows[i][_arrCol[j]];
                    }
                    dt.Rows.Add(_addR);
                }
                else
                {
                    foreach (DataRow R1 in dtChiTieu.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < _arrCol.Length - 2; c++)
                            {
                                if (R2[_arrCol[c]].Equals(R1[_arrCol[c]]) == false)
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
            DataView dv = dt.DefaultView;
            dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            dt = dv.ToTable();

            #endregion


            return dt;
        }
        private void LoadData(FlexCelReport fr, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iALL, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable data = rptBH_Chi_70_1(MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iALL, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtNganh;
            dtNganh = HamChung.SelectDistinct("Nganh", data, "sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "sLNS,sL,sK,sNG,sTNG");
            fr.AddTable("Nganh", dtNganh);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", dtNganh, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sTTM,sNG");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTTM");
            fr.AddTable("Muc", dtMuc);

            data.Dispose();
            dtNganh.Dispose();
            dtTieuMuc.Dispose();
            dtMuc.Dispose();
        }

        public clsExcelResult ExportToPDF(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iALL, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau_A4.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iALL, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
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

        public clsExcelResult ExportToExcel(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iALL, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            clsExcelResult clsResult = new clsExcelResult();
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau_A4.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iALL, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "baocaoquyettoanChiCacCheDoBHXH.xls";
                }
                else
                {
                    clsResult.FileName = "baocaoquyettoanChiCacCheDoBHXH_A4.xls";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iALL, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_70_1_Mau_A4.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iALL, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
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
        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonViTheoLNS(ParentID, MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoLNS(String ParentID, String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonvi = HienThiDonViTheoNam(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTenDonVi");
            String strLNS = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 70%\"");
            return strLNS;

        }

        public static DataTable HienThiDonViTheoNam(String MaND, String ThangQuy, String LoaiThang_Quy, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND BH.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKThang = "";
            if (LoaiThang_Quy == "1")
            {
                switch (ThangQuy)
                {
                    case "1": DKThang = "BH.iThang_Quy between 1 and 3";
                        break;
                    case "2": DKThang = "BH.iThang_Quy between 1 and 6";
                        break;
                    case "3": DKThang = "BH.iThang_Quy between 1 and 9";
                        break;
                    case "4": DKThang = "BH.iThang_Quy between 4 and 12";
                        break;
                    default: DKThang = "BH.iThang_Quy between -1 and -1";
                        break;
                }
            }
            else
            {
                DKThang = "BH.iThang_Quy<=@ThangQuy";
            }
            String SQL = string.Format(@"SELECT DISTINCT 
                                        BH.iID_MaDonVi,BH.sTenDonVi 
                                        FROM BH_ChungTuChiChiTiet as BH
                                        inner join (Select * from NS_DonVi Where iNamLamViec_DonVi = {3}) as DV 
                                        on DV.iID_MaDonVi=BH.iID_MaDonVi
                                        WHERE BH.iTrangThai=1  {1} 
                                        AND BH.bChi=1  AND {2}  {0} AND BH.sLNS<>''
                                        ORDER BY iID_MaDonVi", iID_MaTrangThaiDuyet, DieuKien_NganSach(MaND), DKThang, NguoiDungCauHinhModels.iNamLamViec);
            SqlCommand cmd = new SqlCommand(SQL);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", ThangQuy);
            }
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTenDonVi"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
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
        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(" AND (BH.iNamLamViec={0} AND BH.iID_MaNamNganSach={1} AND BH.iID_MaNguonNganSach={2})", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
        }
    }
}
