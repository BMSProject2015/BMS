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

namespace VIETTEL.Report_Controllers.NguoiCoCong
{
    public class rptNCC_TCCK_LuyKe_54Controller : Controller
    {
        //
        // GET: /rptNCC_TCCK_LuyKe_54/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile = "";
        public ActionResult Index(String KhoGiay="")
        {
            String sFilePath="";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54_A3.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_TCKK_LuyKe_54.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        ///Hàm lấy các giá trị trên Form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {

            String Thang_Quy = "";
            String LoaiThang_Quy = Convert.ToString(Request.Form[ParentID+"_LoaiThang_Quy"]);
            if (LoaiThang_Quy == "1")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            String TongHop = Convert.ToString(Request.Form[ParentID + "_TongHop"]);
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThang_Quy"] = LoaiThang_Quy;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["TongHop"] = TongHop;
            ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_TCKK_LuyKe_54.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public  long Tong = 0;
        /// <summary>
        /// DataTable dữ liệu báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable rptNCC_TCKK_5(String MaND, String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay,String TongHop)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = Guid.Empty.ToString();
            }
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = NgayChiTieu * 3;
            }
            String DkDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
            }
            else
            {
                DkDuyet = " ";
            }
            
            String DK_ThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_ThangQuy = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_ThangQuy = "iThang_Quy IN (4,5,6)";
                        break;
                    case "3": DK_ThangQuy = "iThang_Quy IN (7,8,9)";
                        break;
                    case "4": DK_ThangQuy = "iThang_Quy IN (10,11,12)";
                        break;
                }
            }
            else
            {
                DK_ThangQuy = "iThang_Quy=@ThangQuy";
            }
            String DK_ThangDotNay = "", DK_ThangDenKy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_ThangDotNay = "iThang_Quy IN (1,2,3)";
                        DK_ThangDenKy = "iThang_Quy<=3";
                        break;
                    case "2": DK_ThangDotNay = "iThang_Quy IN (4,5,6)";
                        DK_ThangDenKy = "iThang_Quy<=6";
                        break;
                    case "3": DK_ThangDotNay = "iThang_Quy IN (7,8,9)";
                        DK_ThangDenKy = "iThang_Quy<=9";
                        break;
                    case "4": DK_ThangDotNay = "iThang_Quy IN (10,11,12)";
                        DK_ThangDenKy = "iThang_Quy<=12";
                        break;
                }
            }
            else
            {
                DK_ThangDotNay = "iThang_Quy=@ThangQuy";
                DK_ThangDenKy = "iThang_Quy<=@ThangQuy";
            }
            if (TongHop == "on")
            {
                DataTable dtDonVi = dtDanhsach_DonVi(MaND, Thang_Quy, LoaiThang_Quy, sLNS, iID_MaTrangThaiDuyet);
                String DKDonVi = "";
                //DK Dot phan bo
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DKDonVi += "iID_MaDonVi=@iID_MaDonVi"+i;
                        if (i < dtDonVi.Rows.Count - 1)
                            DKDonVi += " OR ";
                    }
                }
                String SQL = String.Format(@"SELECT 
                                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rSoNguoi) as rSoNguoi,SUM(TongSoTien) as TongSoTien                                     
                                            ,SUM(DotNay) as DotNay
                                            ,SUM(LuyKe) as LuyKe
                                            FROM
                                            ( 
                                              SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,NCC.sMoTa,SUM(rSoNguoi) as rSoNguoi,SUM(rTuChi) as TongSoTien
                                            ,DotNay=CASE WHEN {4} THEN SUM(rTuChi) ELSE 0 END
                                            ,LuyKe=CASE WHEN {5}  THEN SUM(rTuChi) ELSE 0 END
                                            FROM NCC_ChungTuChiTiet AS NCC
                                            WHERE NCC.iTrangThai=1 {0} {1} AND iLoai=1 AND sNG<>'' AND ({2}) AND sLNS=@sLNS AND {3} AND NCC.iTrangThai=1
                                            GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,NCC.sMoTa,iThang_Quy
                                            HAVING SUM(rTuChi)!=0
                                            ) 
                                            as A
                                            GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", DkDuyet, ReportModels.DieuKien_NganSach(MaND), DKDonVi, DK_ThangQuy, DK_ThangDotNay, DK_ThangDenKy);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue(@"ThangQuy", Thang_Quy);
                }
                cmd.Parameters.AddWithValue(@"sLNS", sLNS);
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }

                }
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["TongSoTien"].ToString());
                }
          
                cmd.Dispose();
                //dt Dự Toán
                String DkDuyetChiTieu = "";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    DkDuyetChiTieu = "AND PB_PhanBoChiTiet.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
                }
                else
                {
                    DkDuyetChiTieu = " ";
                }
                            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as DuToan
                            FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                            WHERE sLNS=@sLNS  AND
                            sNG<>'' AND
                            YEAR(dNgayDotPhanBo)= @NamLamViec  {0}
                            AND MONTH(dNgayDotPhanBo)<= @NgayChiTieu  AND ({2}) AND PB_PhanBoChiTiet.iTrangThai=1 {1}
                            GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                            HAVING SUM(rTuChi)<>0", ReportModels.DieuKien_NganSach(MaND, "PB_PhanBoChiTiet"), DkDuyetChiTieu, DKDonVi);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", ReportModels.LayNamLamViec(MaND));
                cmdChiTieu.Parameters.AddWithValue("@sLNS", sLNS);
                cmdChiTieu.Parameters.AddWithValue("@NgayChiTieu", NgayChiTieu);
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }

                }
                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                //ghep 2 dt
                DataRow addR, R2;
                String sCol = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,DuToan";
                String[] arrCol = sCol.Split(',');
                dt.Columns.Add("DuToan", typeof(Decimal));
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"]);
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
                                    dt.Rows[j]["DuToan"] = R1["DuToan"];

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
            }
            else
            {
                String SQL = String.Format(@"SELECT 
                                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rSoNguoi) as rSoNguoi,SUM(TongSoTien) as TongSoTien                                     
                                            ,SUM(DotNay) as DotNay
                                            ,SUM(LuyKe) as LuyKe
                                            FROM
                                            (
                                              SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,NCC.sMoTa,SUM(rSoNguoi) as rSoNguoi,SUM(rTuChi) as TongSoTien
                                            ,DotNay=CASE WHEN {3} THEN SUM(rTuChi) ELSE 0 END
                                            ,LuyKe=CASE WHEN {4} THEN SUM(rTuChi) ELSE 0 END
                                            FROM NCC_ChungTuChiTiet AS NCC
                                            INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                                            WHERE DV.iID_MaDonVi=@iID_MaDonVi  {0} {1} AND iLoai=1 AND sNG<>'' AND {2} AND sLNS=@sLNS AND NCC.iTrangThai=1
                                            GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,NCC.sMoTa,iThang_Quy
                                            HAVING SUM(rTuChi)!=0
                                            ) 
                                            as A
                                            GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", DkDuyet, ReportModels.DieuKien_NganSach(MaND), DK_ThangQuy, DK_ThangDotNay, DK_ThangDenKy);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThang_Quy == "0")
                {
                    cmd.Parameters.AddWithValue(@"ThangQuy", Thang_Quy);
                }
                cmd.Parameters.AddWithValue(@"sLNS", sLNS);
                cmd.Parameters.AddWithValue(@"iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["TongSoTien"].ToString());
                }
                cmd.Dispose();
                //dt Dự Toán
                String DkDuyetChiTieu = "";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    DkDuyetChiTieu = "AND PB_PhanBoChiTiet.iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
                }
                else
                {
                    DkDuyetChiTieu = " ";
                }
                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as DuToan
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE sLNS=@sLNS  AND
                                                        sNG<>'' AND
                                                        YEAR(dNgayDotPhanBo)=@NamLamViec {0} AND MONTH(dNgayDotPhanBo)<=@NgayChiTieu  AND
                                                        PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iID_MaDonVi=@iID_MaDonVi
                                                      {1}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", ReportModels.DieuKien_NganSach(MaND, "PB_PhanBoChiTiet"),DkDuyetChiTieu);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", ReportModels.LayNamLamViec(MaND));
                cmdChiTieu.Parameters.AddWithValue("@sLNS", sLNS);
                cmdChiTieu.Parameters.AddWithValue("@NgayChiTieu", NgayChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                //ghep 2 dt
                DataRow addR, R2;
                String sCol = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,DuToan";
                String[] arrCol = sCol.Split(',');
                dt.Columns.Add("DuToan", typeof(Decimal));
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"]);
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
                                    dt.Rows[j]["DuToan"] = R1["DuToan"];

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
            }
            return dt;
        }
       
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay, String TongHop)
        {
            String MaND = User.Identity.Name;
           
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
            String TenLNS = "";
            DataTable dt = ReportModels.MoTa(sLNS);
            if (dt.Rows.Count > 0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
            String tendv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = tendonvi(iID_MaDonVi);
            if (teN.Rows.Count>0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptNCC_TCCK_LuyKe_54");
            LoadData(fr, MaND, Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,KhoGiay,TongHop);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("Thang_Quy", Thang_Quy);
                fr.SetValue("LoaiThangQuy", LoaiThangQuy);
                fr.SetValue("Ngay", NgayThang);
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1).ToString());
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToString());
                fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("TenLNS", TenLNS);
                if (TongHop == "on")
                {
                    fr.SetValue("TenDV","");
                }
                else
                {
                    fr.SetValue("TenDV", tendv);
                }
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// Hàm lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Hàm hiển thị dữ liệu ra ngoài báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay, String TongHop)
        {
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
           
            DataTable data = rptNCC_TCKK_5(MaND, Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,KhoGiay,TongHop);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa", "sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            data.Dispose();
            dtTieuMuc.Dispose();
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay, String TongHop)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54_A3.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, KhoGiay, TongHop);
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
        /// Hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay, String TongHop)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54_A3.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, KhoGiay, TongHop);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                if (KhoGiay == "1")
                {
                    clsResult.FileName = "LuyKeTroCapKhoKhan_A4.xls";
                }
                else
                {
                    clsResult.FileName = "LuyKeTroCapKhoKhan_A3.xls";
                }
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// hàm View PDf
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String KhoGiay, String TongHop)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_TCKK_Luyke_54_A3.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, KhoGiay, TongHop);
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

        public JsonResult ds_DonVi(String ParentID,  String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;
            return Json(obj_DonVi(ParentID, MaND, Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonVi(String ParentID, String MaND, String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonvi = dtDanhsach_DonVi(MaND, Thang_Quy, LoaiThang_Quy, sLNS, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strDonVi = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 90%\"");
            return strDonVi;

        }
        /// <summary>
        /// Hàm lấy danh sách đơn vị theo loại ngân sách và tháng quy
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable dtDanhsach_DonVi(String MaND, String Thang_Quy, String LoaiThang_Quy, String sLNS, String iID_MaTrangThaiDuyet)
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
            String DK_ThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_ThangQuy = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_ThangQuy = "iThang_Quy IN (4,5,6)";
                        break;
                    case "3": DK_ThangQuy = "iThang_Quy IN (7,8,9)";
                        break;
                    case "4": DK_ThangQuy = "iThang_Quy IN (10,11,12)";
                        break;
                }
            }
            else
            {
                DK_ThangQuy = "iThang_Quy=@ThangQuy";
            }
            String SQL = string.Format(@"SELECT 
                                        DV.iID_MaDonVi,DV.iID_MaDonVi+'-'+DV.sTen AS sTen
                                        FROM NCC_ChungTuChiTiet AS NCC
                                        INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) AS DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                                        WHERE NCC.iTrangThai=1 {1} AND sLNS=@sLNS  {0}
                                        AND {2} AND iLoai=1 AND rTuChi>0
                                        GROUP BY DV.iID_MaDonVi,DV.sTen
                                        ORDER BY DV.iID_MaDonVi,DV.sTen",iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND,"NCC"),DK_ThangQuy);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
            }
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
    }
}
