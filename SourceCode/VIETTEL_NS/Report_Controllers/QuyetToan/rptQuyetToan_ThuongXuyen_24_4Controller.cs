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
    public class rptQuyetToan_ThuongXuyen_24_4Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_24_4.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_ThuongXuyen_24_4_1.xls";
        public static String NameFile = "";
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_24_4.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID,int ChiSo)
        {
            String Thang_Quy = "";
           // String NamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);           
            Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);                 
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID+ "_iID_MaTrangThaiDuyet"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);    
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy,iID_MaDonVi = iID_MaDonVi});
            //ViewData["NamLamViec"] = NamLamViec;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_ThuongXuyen_24_4.aspx";
            return View(sViewPath + "ReportView.aspx");   
        }
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String LoaiIn)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            String TenDV;
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1" && iID_MaDonVi != "-2")
            {
                TenDV = iID_MaDonVi + "  -  " + Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }
            if (LoaiIn == "TongHopAll" || LoaiIn == "TongHop") TenDV = "";
            String TenLoaiThangQuy = "Tháng";
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_thuongxuyen_24_4");
            LoadData(fr, Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet,MaND,LoaiIn);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("LoaiThangQuy", TenLoaiThangQuy);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Tạo các range báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String MaND, String LoaiIn)
        {
            DataTable data;
            if (LoaiIn == "DonVi")
            {
                DataTable DonVi;
                DonVi = QT_TX_24_4(Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet, MaND, LoaiIn);
                fr.AddTable("DonVi", DonVi);
                data = HamChung.SelectDistinct("TieuMuc", DonVi, "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG", "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "");
                fr.AddTable("ChiTiet", data);
            }
            else
            {

                data = QT_TX_24_4(Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet, MaND, LoaiIn);
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
       
       
        /// <summary>
        /// Xuất ra excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel( String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String LoaiIn)
        {

            HamChung.Language();
            String DuongDanFile = sFilePath;
            if (LoaiIn == "DonVi") DuongDanFile = sFilePath1;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet,LoaiIn);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_ThuongXuyen_24_4.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String LoaiIn)
        {
            String DuongDanFile = sFilePath;
            if (LoaiIn == "DonVi") DuongDanFile = sFilePath1;
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), Thang_Quy, iID_MaDonVi, iID_MaTrangThaiDuyet, LoaiIn);
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
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public String obj_DonVi(String ParentID, String MaND, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonVi = QuyetToan_ReportModels.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, "0", MaND);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenDV");
            String DonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            dtDonVi.Dispose();
            return DonVi;
        }
        [HttpGet]
        public JsonResult ds_DonVi(String ParentID, String MaND, String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonVi = DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, "0", MaND);
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dtDonVi, "");
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            dtDonVi.Dispose();

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Quyết toán thường xuyên 24_4
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable QT_TX_24_4(String Thang_Quy, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String MaND,String LoaiIn)
        {
            DataTable dt;     
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            String DKDonVi = "";
         
                //DanhSachDonVi(MaND, Thang_Quy,iID_MaTrangThaiDuyet);
            if (String.IsNullOrEmpty(iID_MaDonVi))
                iID_MaDonVi = Guid.Empty.ToString();
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String DKDonVi_PB = "";
            //Neu la tonghop hoac donvi
            if (LoaiIn!="ChiTiet")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi" + i;
                    DKDonVi_PB += " PB_PhanBoChiTiet.iID_MaDonVi=@iID_MaDonVi" + i;
                    if (i < arrDonVi.Length - 1)
                    {
                        DKDonVi += " OR ";
                        DKDonVi_PB += " OR ";
                    }
                }
                DKDonVi = " AND(" + DKDonVi + ")";
                DKDonVi_PB = " AND(" + DKDonVi_PB + ")";
            }
            else 
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            if (LoaiIn != "DonVi")
            {
                String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,DotNay = CASE WHEN iThang_Quy=@ThangQuy THEN SUM(rTuChi) ELSE 0 END
                                        ,LuyKe = CASE WHEN  iThang_Quy<=@ThangQuy THEN SUM(rTuChi) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1  AND sLNS=1010000 AND sL=460 AND sK=468 AND sNG<>'' {0} {1} {2} AND bLoaiThang_Quy=0 
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                        HAVING SUM(rTuChi)<>0) as a
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet1);
                SqlCommand cmd = new SqlCommand(SQL);
                //cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
                if (LoaiIn == "TongHop")
                {
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));

                dt = Connection.GetDataTable(cmd);

                //tao dt chi tieu
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iNamLamViec = DateTime.Now.Year.ToString();
                String iID_MaNguonNganSach = "1";
                String iID_MaNamNganSach = "2";
                String DKDuyet = "";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
                }
                else
                {
                    DKDuyet = "";
                }
                if (dtCauHinh.Rows.Count > 0)
                {
                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

                }
                dtCauHinh.Dispose();
                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                 WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
                                                 AND sLNS=1010000 AND sL=460 AND sK= 468 AND sNG<>'' AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay AND PB_PhanBoChiTiet.iTrangThai=1 {0} {1}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                 HAVING SUM(rTuChi)<>0", DKDonVi, DKDuyet);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmdChiTieu.Parameters.AddWithValue("@dNgay", iThangQuy);
                if (LoaiIn == "TongHop")
                {
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                }
                else
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
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
            }
                //chon loaiin=donvi
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iNamLamViec = DateTime.Now.Year.ToString();
                if (dtCauHinh.Rows.Count > 0)
                {
                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

                }
                dtCauHinh.Dispose();
                String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe,a.iID_MaDonVi,a.iID_MaDonVi+' - '+sTen as sTen
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi
                                        ,DotNay = CASE WHEN iThang_Quy=@ThangQuy THEN SUM(rTuChi) ELSE 0 END
                                        ,LuyKe = CASE WHEN  iThang_Quy<=@ThangQuy THEN SUM(rTuChi) ELSE 0 END
                                        FROM QTA_ChungTuChiTiet
                                        WHERE iTrangThai=1  AND sLNS=1010000 AND sL=460 AND sK=468 AND sNG<>'' {0} {1} {2} AND bLoaiThang_Quy=0 
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy,iID_MaDonVi
                                        HAVING SUM(rTuChi)<>0) as a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi ON a.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,a.iID_MaDonVi,sTen
                                        HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet1);
                SqlCommand cmd = new SqlCommand(SQL);
                //cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
                    cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
                //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));

                dt = Connection.GetDataTable(cmd);

                //tao dt chi tieu
                String iID_MaNguonNganSach = "1";
                String iID_MaNamNganSach = "2";
                String DKDuyet = "";
                if (iID_MaTrangThaiDuyet == "0")
                {
                    DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
                }
                else
                {
                    DKDuyet = "";
                }
                if (dtCauHinh.Rows.Count > 0)
                {
                    iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

                }
                dtCauHinh.Dispose();
                String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa,PB_PhanBoChiTiet.iID_MaDonVi,PB_PhanBoChiTiet.iID_MaDonVi+' - '+sTen as sTen, SUM(rTuChi) as ChiTieu
                                                 FROM PB_PhanBoChiTiet INNER JOIN PB_DotPhanBo ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo
                                                                       INNER JOIN NS_DONVI ON PB_PhanBoChiTiet.iID_MaDonVi=NS_DONVI.iID_MaDonVi
                                                 WHERE PB_PhanBoChiTiet.iTrangThai=1 AND PB_PhanBoChiTiet.iNamLamViec=@NamLamViec AND PB_PhanBoChiTiet.iID_MaNguonNganSach=@iID_MaNguonNganSach AND PB_PhanBoChiTiet.iID_MaNamNganSach=@iID_MaNamNganSach
                                                 AND sLNS=1010000 AND sL=460 AND sK= 468 AND sNG<>'' AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay AND PB_PhanBoChiTiet.iTrangThai=1 {0} {1}
                                                 GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,PB_PhanBoChiTiet.sMoTa,PB_PhanBoChiTiet.iID_MaDonVi,sTen
                                                 HAVING SUM(rTuChi)<>0", DKDonVi_PB, DKDuyet);
                SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
                cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                cmdChiTieu.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmdChiTieu.Parameters.AddWithValue("@dNgay", iThangQuy);
                    for (int i = 0; i < arrDonVi.Length; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, arrDonVi[i]);
                    }
               
                if (iID_MaTrangThaiDuyet == "0")
                {
                    cmdChiTieu.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
                }
                DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
                cmdChiTieu.Dispose();

                //ghep 2 dt
                DataRow addR, R2;
                String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTen,ChiTieu";
                String[] arrCol = sCol.Split(',');
                dt.Columns.Add("ChiTieu", typeof(Decimal));
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    String xauTruyVan = String.Format(@"NguonNS={7} AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                       AND sTTM='{5}' AND sNG='{6}'AND iID_MaDonVi='{8}' ",
                                                      dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                      dtChiTieu.Rows[i]["sK"],
                                                      dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                      dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["NguonNS"], dtChiTieu.Rows[i]["iID_MaDonVi"]
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
                dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaDonVi";
                dt = dv.ToTable();
            }
            return dt;


        }
        /// <summary>
        /// danh sách đơn vị có dữ liệu đã duyệt
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="Thang_Quy"></param>
        /// <returns></returns>
        public static DataTable DanhSachDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String MaND)
        {
            String DkDuyet = "";
            String DkDuyet_PB = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DkDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
                DkDuyet_PB = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            }
            dtCauHinh.Dispose();
            int iThangQuy = Convert.ToInt32(Thang_Quy);
            int NgayChiTieu = iThangQuy;
            //nếu là quý ngày chỉ tiêu sẽ bằng quý *3= tháng
            if (LoaiThang_Quy == "1")
            {
                NgayChiTieu = iThangQuy * 3;
            }
            String SQL = String.Format(@"SELECT b.sTen as TenDV,a.iID_MaDonVi, b.sTen as sTen 
                                         FROM( SELECT DISTINCT iID_MaDonVi
	                                           FROM QTA_ChungTuChiTiet
	                                           WHERE sLNS=1010000 AND sL=460 AND sK= 468 AND iTrangThai=1 {1} {2}
	                                                 AND iThang_Quy<=@dNgay AND bLoaiThang_Quy=0 AND rTuChi>0) a
                                        INNER JOIN (SELECT iID_MaDonVi,iID_MaNhomDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b
                                        ON a.iID_MaDonVi=b.iID_MaDonVi
                                        UNION
                                         SELECT DISTINCT b.sTen,a.iID_MaDonVi, b.sTen as sTen
                                        FROM (SELECT iID_MaDonVi
	                                          FROM PB_PhanBoChiTiet
	                                          WHERE  iTrangThai=1 {1} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
	                                                AND sLNS=1010000 {3} AND rTuChi>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi      
                                        ORDER BY   iID_MaDonVi  
", "", ReportModels.DieuKien_NganSach(MaND), DkDuyet, DkDuyet_PB);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("dNgay", NgayChiTieu);
            if (LoaiThang_Quy != "1")
            {
                cmd.Parameters.AddWithValue("@ThangQuy", iThangQuy);
            }
            cmd.Parameters.AddWithValue("iNamLamViec", iNamLamViec);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //if (dtDonVi.Rows.Count > 0)
            //{
            //    DataRow R = dtDonVi.NewRow();
            //    R["iID_MaDonVi"] = "-1";
            //    R["TenDV"] = "Chọn tất cả đơn vị";
            //    dtDonVi.Rows.InsertAt(R, 0);
            //}
            //else
            //{
            //    DataRow R = dtDonVi.NewRow();
            //    R["iID_MaDonVi"] = "-2";
            //    R["TenDV"] = "Không có đơn vị";
            //    dtDonVi.Rows.InsertAt(R, 0);
            //}
            return dtDonVi;
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
