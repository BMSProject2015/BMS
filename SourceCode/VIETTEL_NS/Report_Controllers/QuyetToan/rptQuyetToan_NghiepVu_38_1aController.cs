using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
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
    public class rptQuyetToan_NghiepVu_38_1aController : Controller
    {
        // Edit date: 17-07-2012
        // GET: /rptQuyetToan_NghiepVu_38/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/rptQuyetToan_NghiepVu_38_1a.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = 0;
                ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_NghiepVu_38_1a.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Thực hiện xem báo cáo
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID,int ChiSo)
        {
            String Quy = Convert.ToString(Request.Form[ParentID + "_Thang_Quy"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String iID_MaDonVi = Convert.ToString(Request.Form["iID_MaDonVi"]);           
            String sLNS = Convert.ToString(Request.Form["sLNS"]);
            String TruongTien = Convert.ToString(Request.Form[ParentID + "_TruongTien"]);
            String LoaiIn = Convert.ToString(Request.Form[ParentID + "_LoaiIn"]);
            ViewData["pageload"] = 1;
            ViewData["iQuy"] = Quy;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["isLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iTruongTien"] = TruongTien;
            ViewData["LoaiIn"] = LoaiIn;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_NghiepVu_38_1a.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Nam"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="MaDV"></param>
        /// <param name="sLNS"></param>
        /// <param name="LoaiTQ"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String sLNS, String TruongTien, String MaND,String LoaiIn)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV;
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            else
            {
                TenDV = "";
            }
            //tính tổng tiền
            DataTable dtTien = NghiepVu_38_1a(iID_MaTrangThaiDuyet,Thang_Quy, iID_MaDonVi,sLNS, TruongTien,MaND);
            long TongTien = 0;
            String tien = "0";
            if (dtTien.Rows.Count > 0)           
                for (int i = 0; i < dtTien.Rows.Count; i++)
                {
                    tien = String.IsNullOrEmpty(dtTien.Rows[i]["DotNay"].ToString()) ? "0" : dtTien.Rows[i]["DotNay"].ToString();
                    TongTien += long.Parse(tien);
                }            
            else
                TongTien = 0;
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_NghiepVu_38_1a");
            LoadData(fr, iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, sLNS, TruongTien,MaND);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Quy", Thang_Quy);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("LoaiThangQuy", Thang_Quy);           
            fr.SetValue("cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("cap2", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Tien", Tien);
            fr.Run(Result);
            return Result;           
        }
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Nam"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="MaDV"></param>
        /// <param name="sLNS"></param>
        /// <param name="LoaiTQ"></param>
        /// <param name="TruongTien"></param>
        private void LoadData(FlexCelReport fr, String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String sLNS, String TruongTien, String MaND)
        {
            DataTable data = NghiepVu_38_1a(iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, sLNS, TruongTien,MaND);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS,sL,sK", "NguonNS,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtLoaiNS, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("NguonNS", dtNguonNS);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
            dtNguonNS.Dispose();
            dtTieuMuc.Dispose();
        }
        
      
        /// <summary>
        /// View file pdf
        /// </summary>
        /// <param name="Nam"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="MaDV"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <param name="LoaiTQ"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String sLNS, String TruongTien, String MaND,String LoaiIn)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, sLNS, TruongTien,MaND,LoaiIn);
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
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Nam">Năm làm việc</param>
        /// <param name="Quy">Quý </param>
        /// <param name="MaDV">Mã đơn vị</param>
        /// <param name="sLNS">Loại ngân sách</param>        
        /// <param name="TruongTien">Trường tiền</param>
        /// <returns></returns>
        public DataTable NghiepVu_38_1a(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String sLNS, String TruongTien,String MaND)
        {
            DataTable dt=new DataTable();
            int iQuy = Convert.ToInt32(Thang_Quy);
            //
            int NgayChiTieu = iQuy * 3;
            String DKDonVi = " AND iID_MaDonVi IN(";
            String[] arrDV = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDV.Length; i++)
            {
                DKDonVi += "@iID_MaDonVi" + i;
                if (i < arrDV.Length - 1)
                    DKDonVi += " , ";
            }

            DKDonVi += " ) ";
            String DotNay = "";
            String LuyKe = "";
            switch (Thang_Quy)
            {
                case "1": DotNay = "(iThang_Quy between 1  and 3)";
                        LuyKe = "(iThang_Quy between 1  and 3)";
                    break;
                case "2": DotNay = "(iThang_Quy between 4  and 6)";
                        LuyKe = "(iThang_Quy between 1  and 6)";
                    break;
                case "3": DotNay = "(iThang_Quy between 7  and 9)";
                        LuyKe = "(iThang_Quy between 1  and 9)";
                    break;
                case "4": DotNay = "(iThang_Quy between 10  and 12)";
                        LuyKe = "(iThang_Quy between 1  and 12)";
                    break;
            }
            String DKLNS = " AND sLNS IN(";
            String[] arrLNS = sLNS.Split(',');
            for (int j = 0; j < arrLNS.Length; j++)
            {
                DKLNS += "@sLNS" + j;
                if (j < arrLNS.Length - 1)
                    DKLNS += " , ";
            }
            DKLNS += " ) ";
            String DKDuyet = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet = " ";
            }
            String SQL = String.Format(@"SELECT NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(DotNay) as DotNay,SUM(LuyKe) as LuyKe
                                        FROM(
                                            SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                   ,DotNay = CASE WHEN {3} THEN SUM({2}) ELSE 0 END
                                                   ,LuyKe = CASE WHEN  {4} THEN SUM({2}) ELSE 0 END
                                            FROM QTA_ChungTuChiTiet
                                            WHERE iTrangThai=1 {5} {0} AND sNG<>''
                                                 {1}  {6}
                                            GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iThang_Quy
                                            HAVING SUM({2})<>0) as a
                                            GROUP BY NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                            HAVING SUM(DotNay)<>0 OR SUM(LuyKe)<>0", DKLNS, DKDonVi, TruongTien, DotNay, LuyKe, ReportModels.DieuKien_NganSach(MaND), DKDuyet);
            SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@ThangQuy", Quy);
            for (int z = 0; z<arrDV.Length; z++)
            {
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + z, arrDV[z]);
            }
            for (int t = 0; t < arrLNS.Length; t++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + t, arrLNS[t]);
            }
            
            
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            }
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            String SQLChiTieu = String.Format(@" SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa, SUM({0}) as ChiTieu
                                                FROM PB_PhanBoChiTiet
                                                INNER JOIN PB_DotPhanBo
                                                ON PB_PhanBoChiTiet.iID_MaDotPhanBo=PB_DotPhanBo.iID_MaDotPhanBo AND YEAR(PB_DotPhanBo.dNgayDotPhanBo)=@NamLamViec AND MONTH(PB_DotPhanBo.dNgayDotPhanBo)<= @dNgay
                                                WHERE PB_PhanBoChiTiet.iTrangThai=1 {1} {2} 
                                                        AND sNG<>'' 
                                                     {3}
                                                GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                                HAVING SUM({0})<>0", TruongTien, DKDonVi, DKLNS, DKDuyet1);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);
            cmdChiTieu.Parameters.AddWithValue("@NamLamViec", iNamLamViec);
            cmdChiTieu.Parameters.AddWithValue("@dNgay", NgayChiTieu);
            for (int z = 0; z < arrDV.Length; z++)
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + z, arrDV[z]);
            }
            for (int t = 0; t < arrLNS.Length; t++)
            {
                cmdChiTieu.Parameters.AddWithValue("@sLNS" + t, arrLNS[t]);
            }
            DataTable dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();

            DataRow addR, R2;
            String sCol = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,ChiTieu";
            String[] arrCol = sCol.Split(',');

            dt.Columns.Add("ChiTieu", typeof(Decimal));

            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"NguonNS='{7}' AND sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
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
            dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG";
            dt = dv.ToTable();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị
        /// </summary>
        /// <param name="iLNS">Loại ngân sách</param>
        /// <param name="iNam">Năm</param>
        /// <param name="iQuy">Quý</param>
        /// <returns></returns>
        public static DataTable LayDSDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String LoaiThang_Quy, String sLNS, String TruongTien, String MaND)
        {
            String DKThangQuy = "";
            if (LoaiThang_Quy == "1")
            {
                {
                    if (Thang_Quy == "1")
                        DKThangQuy = "iThang_Quy <=3";
                    else if (Thang_Quy == "2")
                        DKThangQuy = "iThang_Quy <=6";
                    else if (Thang_Quy == "3")
                        DKThangQuy = "iThang_Quy <=9";
                    else if (Thang_Quy == "4")
                        DKThangQuy = "iThang_Quy <=12";
                    else DKThangQuy = "iThang_Quy <=-1";
                }

            }
            else
            {
                DKThangQuy = "iThang_Quy<=@iThangQuy";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            String iID_MaNguonNganSach = "1";
            String iID_MaNamNganSach = "2";
            String DKDuyet = "", DKDuyet_PB = "";
            if (iID_MaTrangThaiDuyet != "0")
            {
                DKDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                DKDuyet_PB = "AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_PB";
            }
            else
            {
                DKDuyet = "";
                DKDuyet_PB = "";
            }
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
            NgayChiTieu = iThangQuy * 3;
            //DKLoaiNganSach
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = String.Format(@"SELECT DISTINCT b.sTen,a.iID_MaDonVi,a.iID_MaDonVi+'-'+b.sTen as TenHT
                                        FROM (SELECT iID_MaDonVi
	                                          FROM QTA_ChungTuChiTiet
	                                          WHERE   iTrangThai=1 {1}
	                                                AND ({5}) AND {3} {2} AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi
                                        UNION  
                                        SELECT DISTINCT b.sTen,a.iID_MaDonVi,a.iID_MaDonVi+'-'+b.sTen as TenHT
                                        FROM (SELECT iID_MaDonVi
	                                          FROM PB_PhanBoChiTiet
	                                          WHERE  iTrangThai=1 {1} AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_PhanBo WHERE iTrangThai=1 AND YEAR(dNgayDotPhanBo)=@NamLamViec AND MONTH(dNgayDotPhanBo)<= @dNgay)
	                                                AND ({5}) {4} AND {0}>0) as A 	
                                         INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi) as B 
                                        ON A.iID_MaDonVi=b.iID_MaDonVi      
                                        ORDER BY   iID_MaDonVi                           

", TruongTien, ReportModels.DieuKien_NganSach(MaND), DKDuyet, DKThangQuy, DKDuyet_PB, DKLNS);
            SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@LoaiThangQuy", LoaiThang_Quy);
            if (LoaiThang_Quy == "0")
            {
                cmd.Parameters.AddWithValue("@iThangQuy", Thang_Quy);
            }
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            cmd.Parameters.AddWithValue("NamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("dNgay", NgayChiTieu);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
                cmd.Parameters.AddWithValue("iID_MaTrangThaiDuyet_PB", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            }
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dtDonVi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLNS">Loại ngân sách</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="arrDV">Mã đơn vị</param>
        /// <returns></returns>
        public String obj_DSDonVi(String iID_MaTrangThaiDuyet, String Thang_Quy, String sLNS, String TruongTien, String MaND,String iID_MaDonVi)
        {
            DataTable dt = LayDSDonVi(iID_MaTrangThaiDuyet, Thang_Quy, "1", sLNS, TruongTien, MaND);
            String input = "";
            StringBuilder stbDonVi = new StringBuilder();
            stbDonVi.Append("<div style=\"width: 100%; height: 400px; overflow: scroll; border:1px solid black;\">");
            stbDonVi.Append("<table class=\"mGrid\">");
            stbDonVi.Append("<tr>");
            stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"ChonallDV(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

            String TenDonVi = "", MaDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dt.Rows[i - 1]["iID_MaDonVi"]);
                TenDonVi = Convert.ToString(dt.Rows[i - 1]["sTen"]);
                _Checked = "";
                for (int j = 1; j <= arrDonVi.Length; j++)
                {
                    if (MaDonVi == arrDonVi[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" onchange=\"ChonTo()\" />", MaDonVi, _Checked);
                stbDonVi.Append("<tr>");
                stbDonVi.Append("<td style=\"width: 15%;\">");
                stbDonVi.Append(input);
                stbDonVi.Append("</td>");
                stbDonVi.Append("<td>" + TenDonVi + "</td>");

                stbDonVi.Append("</tr>");
            }
            stbDonVi.Append("</table>");
            stbDonVi.Append("</div>");
            dt.Dispose();
            String s = stbDonVi.ToString();
            return s;
        }
        /// <summary>
        /// Hàm ajax lấy danh sách đơn vị
        /// </summary>
        /// <param name="iLNS">Loại ngân sách</param>
        /// <param name="iQuy">Quý</param>
        /// <param name="iNam">Năm</param>
        /// <param name="arrDV">Mã đơn vị</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ds_DonVi( String iID_MaTrangThaiDuyet, String Thang_Quy, String sLNS, String TruongTien, String MaND, String iID_MaDonVi)
        {
            return Json(obj_DSDonVi( iID_MaTrangThaiDuyet, Thang_Quy, sLNS, TruongTien, MaND, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        //Lấy loại ngân sách       
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Xuất ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="ThangQuy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String Thang_Quy, String iID_MaDonVi, String sLNS, String TruongTien, String MaND,String LoaiIn)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, Thang_Quy, iID_MaDonVi, sLNS, TruongTien,MaND,LoaiIn);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_NghiepVu_38_1a.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNghiepVu(Boolean All = false,String iID_MaPhongBan="")
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT A.sLNS as sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7  AND SUBSTRING(A.sLNS,1,1) <> '1' AND A.sL = '' ORDER By A.sLNS");
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
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
            DK = String.Format(" AND (QTA.iNamLamViec={0} AND QTA.iID_MaNamNganSach={1} AND QTA.iID_MaNguonNganSach={2})", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
        }
    }
}