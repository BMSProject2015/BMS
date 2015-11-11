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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_nghiepvu_45_9Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_nghiepvu_45_9.xls";
        public static String iID_MaPhongBan, iID_MaDonVi, iThang_Quy, iID_MaNamNganSach, MaND,iID_MaTrangThaiDuyet,sLNS;
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_nghiepvu_45_9.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// edit submit nhận các giá trị từ view
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
          
            iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);
            sLNS = Convert.ToString(Request.Form["sLNS"]);
            iID_MaPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            iThang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang_Quy"]);
            iID_MaNamNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaNamNganSach"]);
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["sLNS"] = sLNS;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_nghiepvu_45_9.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport()
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePath));
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2,MaND);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1, MaND);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_nghiepvu_45_9");
            LoadData(fr);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Lấy các range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        private void LoadData(FlexCelReport fr)
        {
            DataTable data = QT_NV_45_9();
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtsTM = null, dtsM = null, dtsL = null, dtsLNS = null, dtsLNS5 = null, dtsLNS3 = null, dtsLNS1 = null;
            ReportModels.getDataTable7Cap(data, ref dtsTM, ref dtsM, ref dtsL, ref dtsLNS, ref dtsLNS5, ref dtsLNS3, ref dtsLNS1);
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

        /// <summary>
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String viID_MaTrangThaiDuyet, String viThang_Quy, String viID_MaDonVi, String viID_MaPhongBan, String vsLNS, String viID_MaNamNganSach)
        {
            HamChung.Language();
            MaND = User.Identity.Name;
            iID_MaPhongBan = viID_MaPhongBan;
            iID_MaTrangThaiDuyet = viID_MaTrangThaiDuyet;
            iThang_Quy = viThang_Quy;
            iID_MaDonVi = viID_MaDonVi;
            sLNS = vsLNS;
            iID_MaNamNganSach = viID_MaNamNganSach;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport();
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_45_9.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String viID_MaTrangThaiDuyet, String viThang_Quy, String viID_MaDonVi, String viID_MaPhongBan, String vsLNS, String viID_MaNamNganSach)
        {
            HamChung.Language();
            MaND = User.Identity.Name;
            iID_MaPhongBan = viID_MaPhongBan;
            iID_MaTrangThaiDuyet = viID_MaTrangThaiDuyet;
            iThang_Quy = viThang_Quy;
            iID_MaDonVi = viID_MaDonVi;
            sLNS = vsLNS;
            iID_MaNamNganSach = viID_MaNamNganSach;
            ExcelFile xls = CreateReport();
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
        /// onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String jiID_MaTrangThaiDuyet, String jiThang_Quy, String jiID_MaPhongBan, String jiID_MaDonVi, String jsLNS, String jiID_MaNamNganSach)
        {
            String MaND = User.Identity.Name;
            DataTable dtDonVi = LayDSDonVi(jiID_MaTrangThaiDuyet, jiThang_Quy, jiID_MaPhongBan, jsLNS, jiID_MaNamNganSach, MaND);
            if (String.IsNullOrEmpty(jiID_MaDonVi))
            {
                jiID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, jiID_MaDonVi, dtDonVi, "rptQuyetToan_ThongTri_5");
            dtDonVi.Dispose();
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet); ;
        }

        /// <summary>
        /// Quyết toán nghiệp vụ 45_9
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public DataTable QT_NV_45_9()
        {
            DataTable dt = new DataTable();
            String DK = ""; String LNS = "", DKDotNay = "", DKLuyKe = "", DKDonVi = "", DKPhongBan = "";
            String DKDuyet = "";
            SqlCommand cmd = new SqlCommand();
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = " ";
            }

            DKDotNay = " iThang_Quy=@ThangQuy";
            DKLuyKe = " iThang_Quy<=@ThangQuy";

            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                LNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    LNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = Guid.Empty.ToString();
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                DK = " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaNamNganSach) && iID_MaNamNganSach != "0")
            {
                DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);

            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as sLNS1,SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = SUM(CASE WHEN {5} THEN SUM(rTuChi) ELSE 0 END)
                                        ,LuyKe = SUM(CASE WHEN  {6} THEN SUM(rTuChi) ELSE 0 END)
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1 {3} AND ({1}) AND sNG<>'' {0}  {4}
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        HAVING SUM({2})<>0) as a
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDonVi, LNS, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDotNay, DKLuyKe);
//            SqlCommand cmd = new SqlCommand(SQL);
//            if (LoaiThang_Quy != "1")
//            {
//                cmd.Parameters.AddWithValue("@iThangQuy", iThangQuy);
//            }
//            if (LoaiIn == "TongHop")
//            {
//                for (int i = 0; i < arrDonVi.Length; i++)
//                {
//                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
//                }
//                if (LoaiIn == "TongHop")
//                {
//                    for (int i = 0; i < arrDonVi.Length; i++)
//                    {
//                        DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
//                        if (i < arrDonVi.Length - 1)
//                            DKDonVi += " OR ";
//                    }
//                    DKDonVi = " AND(" + DKDonVi + ")";
//                }
//                else
//                {
//                    DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
//                }

//                String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
//                                        FROM(
//                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
//                                        ,DotNay = CASE WHEN {5} THEN SUM({2}) ELSE 0 END
//                                        ,LuyKe = CASE WHEN  {6} THEN SUM({2}) ELSE 0 END
//                                        FROM QTA_ChungTuChiTiet
//                                        WHERE iTrangThai=1 {3} AND ({1}) AND sNG<>'' {0}  {4}
//                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
//                                        HAVING SUM({2})<>0) as a
//                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
//                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDonVi, LNS, TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDotNay, DKLuyKe);
//                SqlCommand cmd = new SqlCommand(SQL);
//                if (LoaiThang_Quy != "1")
//                {
//                    cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
//                }
//                if (LoaiIn == "TongHop")
//                {
//                    for (int i = 0; i < arrDonVi.Length; i++)
//                    {
//                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
//                    }
//                }
//                else
//                {
//                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
//                }
//                for (int i = 0; i < arrLNS.Length; i++)
//                {
//                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
//                }

//                dt = Connection.GetDataTable(cmd);
//                cmd.Dispose();
//                //tao dt chi tieu
//                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
//                String iNamLamViec = DateTime.Now.Year.ToString();
//                String iID_MaNguonNganSach = "1";
//                String iID_MaNamNganSach = "2";
//                String DKDuyet1 = "";
//                if (iID_MaTrangThaiDuyet == "0")
//                {
//                    DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
//                }
//                else
//                {
//                    DKDuyet1 = "";
//                }
//                if (dtCauHinh.Rows.Count > 0)
//                {
//                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
//                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
//                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

//                }
//                dtCauHinh.Dispose();
//                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM({0}) as ChiTieu
//                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
//                                                 WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
//                                                AND ({1})  AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay  AND PB_PhanBoChiTiet.iTrangThai=1 {2} AND sNG<>'' {3}
//                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
//                                                 HAVING SUM({0})<>0", TruongTien, LNS, DKDonVi, DKDuyet1);
//                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
//                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
//                cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
//                cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
//                cmdChiTieu.Parameters.AddWithValue("@dNgay", NgayChiTieu);
//                for (int i = 0; i < arrLNS.Length; i++)
//                {
//                    cmdChiTieu.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
//                }
//                if (LoaiIn == "TongHop")
//                {
//                    for (int i = 0; i < arrDonVi.Length; i++)
//                    {
//                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
//                    }
//                }
//                else
//                {
//                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
//                }
//                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
//                cmdChiTieu.Dispose();
//                #region  //Ghép DTChiTieu vào dt
//                DataRow addR, R2;
//                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
//                String[] arrCol = sCol.Split(',');

//                dt.Columns.Add("ChiTieu", typeof(Decimal));

//                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
//                {
//                    String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
//                                                       AND sTTM='{5}' AND sNG='{6}'",
//                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
//                                                      dtChiTieu.Rows[i]["sK"],
//                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
//                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"]
//                                                      );
//                    DataRow[] R = dt.Select(xauTruyVan);

//                    if (R == null || R.Length == 0)
//                    {
//                        addR = dt.NewRow();
//                        for (int j = 0; j < arrCol.Length; j++)
//                        {
//                            addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
//                        }
//                        dt.Rows.Add(addR);
//                    }
//                    else
//                    {
//                        foreach (DataRow R1 in dtChiTieu.Rows)
//                        {

//                            for (int j = 0; j < dt.Rows.Count; j++)
//                            {
//                                Boolean okTrung = true;
//                                R2 = dt.Rows[j];

//                                for (int c = 0; c < arrCol.Length - 2; c++)
//                                {
//                                    if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
//                                    {
//                                        okTrung = false;
//                                        break;
//                                    }
//                                }
//                                if (okTrung)
//                                {
//                                    dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

//                                    break;
//                                }

//                            }
//                        }

//                    }

//                }
//                //sắp xếp datatable sau khi ghép
//                DataView dv = dt.DefaultView;
//                dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG";
//                dt = dv.ToTable();
//                #endregion
//            }
//            //Muc luc theo don vi
//            else
//            {
//                String DKDonVi_CT = "";
//                for (int i = 0; i < arrDonVi.Length; i++)
//                {
//                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
//                    DKDonVi_CT += " PB_PhanBoChiTiet.iID_MaDonVi=@iID_MaDonVi" + i;
//                    if (i < arrDonVi.Length - 1)
//                        DKDonVi += " OR ";
//                    if (i < arrDonVi.Length - 1)
//                        DKDonVi_CT += " OR ";
//                }
//                DKDonVi = " AND(" + DKDonVi + ")";
//                DKDonVi_CT = " AND(" + DKDonVi_CT + ")";
//                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
//                String iNamLamViec = DateTime.Now.Year.ToString();
//                String iID_MaNguonNganSach = "1";
//                String iID_MaNamNganSach = "2";
//                if (dtCauHinh.Rows.Count > 0)
//                {
//                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
//                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
//                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

//                }
//                dtCauHinh.Dispose();
//                String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,a.iID_MaDonVi,a.iID_MaDonVi+' - '+sTen as sTen,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
//                                        FROM(
//                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi
//                                        ,DotNay = CASE WHEN {5} THEN SUM({2}) ELSE 0 END
//                                        ,LuyKe = CASE WHEN  {6} THEN SUM({2}) ELSE 0 END
//                                        FROM QTA_ChungTuChiTiet
//                                        WHERE iTrangThai=1 {3} AND ({1}) AND sNG<>'' {0}  {4}
//                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy,iID_MaDonVi
//                                        HAVING SUM({2})<>0) as a
//                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON a.iID_MaDonVi=NS_DonVi.iID_MaDonVi
//                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,a.iID_MaDonVi,sTen
//                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDonVi, LNS, TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKDotNay, DKLuyKe);
//                SqlCommand cmd = new SqlCommand(SQL);
//                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
//                if (LoaiThang_Quy != "1")
//                {
//                    cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
//                }

//                for (int i = 0; i < arrDonVi.Length; i++)
//                {
//                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
//                }
//                for (int i = 0; i < arrLNS.Length; i++)
//                {
//                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
//                }

//                dt = Connection.GetDataTable(cmd);
//                cmd.Dispose();

//                String DKDuyet1 = "";
//                if (iID_MaTrangThaiDuyet == "0")
//                {
//                    DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
//                }
//                else
//                {
//                    DKDuyet1 = "";
//                }

//                dtCauHinh.Dispose();
//                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa,PB_PhanBoChiTiet.iID_MaDonVi,PB_PhanBoChiTiet.iID_MaDonVi+' - '+sTen as sTen, SUM({0}) as ChiTieu
//                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
//                                                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON PB_PhanBoChiTiet.iID_MaDonVi=NS_DONVI.iID_MaDonVi
//                                                 WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
//                                                AND ({1})  AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay  AND PB_PhanBoChiTiet.iTrangThai=1 {2} AND sNG<>'' {3}
//                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa,PB_PhanBoChiTiet.iID_MaDonVi,sTen
//                                                 HAVING SUM({0})<>0", TruongTien, LNS, DKDonVi_CT, DKDuyet1);
//                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
//                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
//                cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
//                cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
//                cmdChiTieu.Parameters.AddWithValue("@dNgay", NgayChiTieu);
//                for (int i = 0; i < arrLNS.Length; i++)
//                {
//                    cmdChiTieu.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
//                }

//                for (int i = 0; i < arrDonVi.Length; i++)
//                {
//                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
//                }
//                cmdChiTieu.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
//                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
//                cmdChiTieu.Dispose();
//                #region  //Ghép DTChiTieu vào dt
//                DataRow addR, R2;
//                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTen,ChiTieu";
//                String[] arrCol = sCol.Split(',');

//                dt.Columns.Add("ChiTieu", typeof(Decimal));

//                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
//                {
//                    String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
//                                                       AND sTTM='{5}' AND sNG='{6}' AND iID_MaDonVi='{8}'",
//                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
//                                                      dtChiTieu.Rows[i]["sK"],
//                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
//                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"], dtChiTieu.Rows[i]["iID_MaDonVi"]
//                                                      );
//                    DataRow[] R = dt.Select(xauTruyVan);

//                    if (R == null || R.Length == 0)
//                    {
//                        addR = dt.NewRow();
//                        for (int j = 0; j < arrCol.Length; j++)
//                        {
//                            addR[arrCol[j]] = dtChiTieu.Rows[i][arrCol[j]];
//                        }
//                        dt.Rows.Add(addR);
//                    }
//                    else
//                    {
//                        foreach (DataRow R1 in dtChiTieu.Rows)
//                        {

//                            for (int j = 0; j < dt.Rows.Count; j++)
//                            {
//                                Boolean okTrung = true;
//                                R2 = dt.Rows[j];

//                                for (int c = 0; c < arrCol.Length - 2; c++)
//                                {
//                                    if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
//                                    {
//                                        okTrung = false;
//                                        break;
//                                    }
//                                }
//                                if (okTrung)
//                                {
//                                    dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

//                                    break;
//                                }

//                            }
//                        }

//                    }

//                }
//                //sắp xếp datatable sau khi ghép
//                DataView dv = dt.DefaultView;
//                dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaDonVi";
//                dt = dv.ToTable();
//                #endregion
//            }
            return dt;
        }
        /// <summary>
        /// lấy danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="LoaiThang_Quy"></param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String jiID_MaTrangThaiDuyet, String jiThang_Quy, String jiID_MaPhongBan, String jsLNS, String jiID_MaNamNganSach, String MaND)
        {
            String DKThangQuy = "";
            DKThangQuy = "iThang_Quy<=@iThangQuy";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String DK = "",DKPhongBan="";
            SqlCommand cmd = new SqlCommand();
            if (jiID_MaTrangThaiDuyet == "0")
            {
                DK = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (!String.IsNullOrEmpty(jiID_MaNamNganSach) && jiID_MaNamNganSach != "0")
            {
                DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", jiID_MaNamNganSach);
            }
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            }
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, jiID_MaPhongBan);
            dtCauHinh.Dispose();
            //DKLoaiNganSach
            String DKLNS = "";
            String[] arrLNS = jsLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            String SQL = String.Format(@"SELECT DISTINCT b.sTen,a.iID_MaDonVi,a.iID_MaDonVi+'-'+b.sTen as TenHT
                                        FROM (SELECT iID_MaDonVi
	                                          FROM QTA_ChungTuChiTiet
	                                          WHERE   iTrangThai=1 {1}
	                                                 AND {3} {2} {5} AND ({6}) AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi  WHERE iTrangThai=1 AND iNamLamViec_DonVi=@NamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi
                                        ORDER BY   iID_MaDonVi                           

", "rTuChi", "", DK, DKThangQuy, DKLNS, DKPhongBan, DKLNS);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThangQuy", jiThang_Quy);
                        cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
            if (jiID_MaTrangThaiDuyet == "0")
            {
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            }

            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonVi;
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
