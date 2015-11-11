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
    public class rptQuyetToan_25_5Controller : Controller
    {
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath= "/Report_ExcelFrom/QuyetToan/rptQuyetToan_25_5.xls";
        public static String NameFile = "";
        /// <summary>
        /// Hàm index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["PageLoad"] = 0;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_25_5.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Hàm EditSubmit: Bắt các giá trị từ View
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
           
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String Cachin = Convert.ToString(Request.Form[ParentID + "_Cachin"]);
            ViewData["iID_MaDonVi"] = iID_MaDonVi;

            ViewData["iThang"] = iThang;
            
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Cachin"] = Cachin;
            ViewData["PageLoad"] = 1;
            ViewData["path"] = "~/Report_Views/QuyetToan/rptQuyetToan_25_5.aspx";
            return View(sViewPath + "ReportView.aspx");   
            
        }
        /// <summary>
        /// tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iThang, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Cachin)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenDV = "";
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                TenDV = "Đơn vị: " + Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }
            if (iID_MaDonVi == "-1")
            {
                TenDV = "";
            }
             String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dtCauHinh.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            }
           
            //tính tổng tiền
            DataTable dt = QT_ThuongXuyen_25_5(iThang, iID_MaDonVi, iID_MaTrangThaiDuyet, MaND, Cachin);
            long TongTien = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["TuChiThangNay"].ToString() != "")
                {
                    TongTien += long.Parse(dt.Rows[i]["TuChiThangNay"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            //lấy ngày hiện tại
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            //set các thông số
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_25_5");
            LoadData(fr, iThang, iID_MaDonVi, iID_MaTrangThaiDuyet, MaND, Cachin);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("TenDV", TenDV);
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("Tien", Tien);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("QuanKhu", QuanKhu);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// lấy dữ liệu fill vào báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String iThang, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String MaND, String Cachin)
        {

            DataTable data = QT_ThuongXuyen_25_5(iThang, iID_MaDonVi, iID_MaTrangThaiDuyet, MaND, Cachin);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
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
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iThang, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Cachin)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iThang, iID_MaDonVi, iID_MaTrangThaiDuyet, Cachin);
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
        /// Xuất ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iThang, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Cachin)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iThang, iID_MaDonVi, iID_MaTrangThaiDuyet, Cachin);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QuyetToan_25_5.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        /// <summary>
        /// Xem File PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iThang, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Cachin)
        {
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iThang, iID_MaDonVi, iID_MaTrangThaiDuyet, Cachin);
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
        /// bắt sự kiện onchange 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String iThang, String MaND, String iID_MaDonVi,String iID_MaTrangThaiDuyet)
        {

            return Json(obj_DSDonVi(ParentID, iThang, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String iThang, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            String dsDonVi = "";
           // DataTable dtDonVi = QuyetToan_ReportModels.DanhSach_DonVi( iThang,MaND, "1010000");
            DataTable dtDonVi = DanhSach_DonVi(iThang, MaND,iID_MaTrangThaiDuyet, "1010000");
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi","", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }
       
        
        /// <summary>
        /// Data của báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable QT_ThuongXuyen_25_5(String iThang, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String MaND, String Cachin)
        {
                     
            DataTable dtDenThangNay=new DataTable();
            DataTable dtThangNay= new DataTable();
            DataTable dtDonVi = DanhSach_DonVi(iThang, MaND, iID_MaTrangThaiDuyet,"1010000");
            String DKDonVi = "";
            if (iID_MaDonVi == "-2")
            {                             
                DKDonVi = "AND iID_MaDonVi='" + Guid.Empty.ToString()+"'";              
            }
            else if (iID_MaDonVi == "-1")
            {
                for (int i = 1; i < dtDonVi.Rows.Count;i++ )
                {
                    DKDonVi += " iID_MaDonVi=@iID_MaDonVi"+i;
                    if (i < dtDonVi.Rows.Count-1)
                        DKDonVi += " OR ";
                }
                DKDonVi = " AND(" + DKDonVi + ")";
            }
            else
            {
                DKDonVi = " AND iID_MaDonVi=@iID_MaDonVi";
            }
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            //Tao datatable tháng này
            
                String SQLThangNay = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi) as TuChiThangNay
                                                    FROM QTA_ChungTuChiTiet
                                                    WHERE sNG<>'' AND sLNS='1010000' AND sL='460' AND sK='468' AND iTrangThai=1
                                                   {0} AND iThang_Quy=@ThangQuy
                                                    {1}
                                                    AND bLoaiThang_Quy=0 AND iTrangThai=1 {2}
                                                    GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,SK,sM,sTM,sTTM,sNG,sMoTa
                                                    HAVING SUM(rTuChi)<>0", DKDonVi, ReportModels.DieuKien_NganSach(MaND), DKDuyet1);
                SqlCommand cmdThangNay = new SqlCommand(SQLThangNay);

                cmdThangNay.Parameters.AddWithValue("@ThangQuy", iThang);
                if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
                {
                    cmdThangNay.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                if (iID_MaDonVi == "-1")
                {
                    for (int i = 1; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdThangNay.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                    }
                }

                dtThangNay = Connection.GetDataTable(cmdThangNay);


                cmdThangNay.Dispose();
          
            // tạo data đến tháng này
            
                String SQlDenThangNay = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(rTuChi) as TuChiDenThangNay
                                                        FROM QTA_ChungTuChiTiet
                                                        WHERE sNG<>'' AND sLNS='1010000'AND sL='460' AND sK='468' AND iTrangThai=1 
                                                        {0} AND iThang_Quy<= @ThangQuy   
                                                         {2}
                                                        AND bLoaiThang_Quy=0 AND iTrangThai=1 {1}
                                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,SK,sM,sTM,sTTM,sNG,sMoTa
                                                        HAVING SUM(rTuChi)<>0", DKDonVi, DKDuyet1, ReportModels.DieuKien_NganSach(MaND));
                SqlCommand cmdDenThangNay = new SqlCommand(SQlDenThangNay);

                cmdDenThangNay.Parameters.AddWithValue("@ThangQuy", iThang);

                if (iID_MaDonVi != "-1" || iID_MaDonVi != "-2")
                {
                    cmdDenThangNay.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                if (iID_MaDonVi == "-1")
                {
                    for (int i = 1; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdDenThangNay.Parameters.AddWithValue("@iID_MaDonVi" + i, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]));
                    }
                }


                dtDenThangNay = Connection.GetDataTable(cmdDenThangNay);
                cmdDenThangNay.Dispose();
           
            
            #region //Ghep 2 dt thang nay+ den thang nay
            DataRow addR, R2;
            String sCol = "sM,sTM,sTTM,sNG,sMoTa,TuChiThangNay";
            String[] arrCol = sCol.Split(',');
            dtDenThangNay.Columns.Add("TuChiThangNay", typeof(Decimal));
            for (int i = 0; i < dtThangNay.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"sM='{0}' AND sTM='{1}'
                                                       AND sTTM='{2}'  AND sNG='{3}'",
                                                  dtThangNay.Rows[i]["sM"],
                                                   dtThangNay.Rows[i]["sTM"], dtThangNay.Rows[i]["sTTM"], dtThangNay.Rows[i]["sNG"]);
                DataRow[] R = dtDenThangNay.Select(xauTruyVan);
                if (R == null || R.Length == 0)
                {
                    addR = dtDenThangNay.NewRow();
                    foreach (DataRow R1 in R)
                    {
                        for (int j = 0; j < arrCol.Length; j++)
                        {
                            addR[arrCol[j]] = dtThangNay.Rows[i][arrCol[j]];
                        }
                    }
                    dtDenThangNay.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtThangNay.Rows)
                    {

                        for (int j = 0; j < dtDenThangNay.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dtDenThangNay.Rows[j];

                            for (int c = 0; c < arrCol.Length - 1; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dtDenThangNay.Rows[j]["TuChiThangNay"] = R1["TuChiThangNay"];
                                break;
                            }

                        }
                    }

                }
                DataView dv = dtDenThangNay.DefaultView;
                dv.Sort = "NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG";
                dtDenThangNay = dv.ToTable();

            }
            #endregion
            return dtDenThangNay;
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
        public static DataTable DanhSach_DonVi(String iThang, String MaND, String iID_MaTrangThaiDuyet,String sLNS = "1010000")
        {
            String DKDuyet1 = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                DKDuyet1 = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan) + "'";
            }
            else
            {
                DKDuyet1 = " ";
            }
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtNguoiDung = NguoiDungCauHinhModels.LayCauHinh(MaND);
            iNamLamViec = dtNguoiDung.Rows[0]["iNamLamViec"].ToString();
            dtNguoiDung.Dispose();
            String SQL = String.Format(@"SELECT DISTINCT NS_DonVi.sTen,QT.iID_MaDonVi
                                         FROM (SELECT iID_MaDonVi,rTuChi FROM QTA_ChungTuChiTiet                                     
                                         WHERE 1=1 AND rTuChi>0 AND bLoaiThang_Quy=0  AND sLNS=@sLNS AND iThang_Quy<=@iThang_Quy AND iTrangThai=1 {1} {0}
                                         )as QT
					                    INNER JOIN (SELECT iID_MaDonVi,sTen FROM  NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi on QT.iID_MaDonVi=NS_DonVi.iID_MaDonVi
					                    ORDER BY iID_MaDonVi", ReportModels.DieuKien_NganSach(MaND),DKDuyet1);
            SqlCommand cmd = new SqlCommand(SQL);
           
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "-1";
                R["sTen"] = "Chọn tất cả";
                dt.Rows.InsertAt(R, 0);
            }
            else
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "-2";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
    }
}
