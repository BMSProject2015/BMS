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
using System.Data.OleDb;
using System.Data;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDT_2bcController : Controller
    {
        //
        // GET: /rptDT_2bc/

        //
        // GET: /DuToanChiNganSachSuDung/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDT_2bC.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if(HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath,User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/DuToan/rptDT_2bc.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Hàm lấy giá trị trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
         [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String sPath = "/Libraries/DuToan/Excel";// + DateTime.Now.ToString("yyyy/MM/dd");
            String path2 = Server.MapPath(sPath);
            HamChung.CreateDirectory(path2);
            NameFile = path2 + "" + FileName;
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);

            string path = string.Empty;
            string sTenKhoa = "TMTL";
            path = TuLieuLichSuModels.ThuMucLuu(sTenKhoa);
            String sFileName = "";
            DateTime today = DateTime.Now;
            String dateString = today.ToString("yyyy/MM/dd");
            string newPath = AppDomain.CurrentDomain.BaseDirectory + path + "/" + dateString;
            //string newPath = path + dateString;
            if (Directory.Exists(newPath) == false)
            {
                Directory.CreateDirectory(newPath);
            }
            string filename = "";
            var myFile = System.Web.HttpContext.Current.Request.Files["uploadFile"];
            sFileName = Path.GetFileName(Request.Files["uploadFile"].FileName);
            //
            String filename1 = "";
            filename1 = Convert.ToString(Request.Form[ParentID + "_filename1"]);
            //if (String.IsNullOrEmpty(filename1))
            //{
            //upload file len serverl
            filename = Path.GetFileName(Request.Files["uploadFile"].FileName);
            //Request.Files["uploadFile"].SaveAs(Path.Combine(newPath, filename));
            filename = Path.Combine(newPath, "MauPCB10.xls");
            String ConnectionString = String.Format(ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'", filename);
            OleDbConnection connExcel = new OleDbConnection(ConnectionString);
            OleDbCommand cmdExcel = new OleDbCommand();
            cmdExcel.Connection = connExcel;
            connExcel.Open();
            OleDbConnection conn = null;
            conn = new OleDbConnection(ConnectionString);
            conn.Open();

            DataTable dtSheet = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            String Sheetname = Convert.ToString(dtSheet.Rows[0]["TABLE_NAME"]);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = String.Format(@"SELECT * FROM [{0}]", Sheetname);
            cmd.Connection = conn;
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
           
            adapter.Fill(dt);
            dt.Columns[0].ColumnName = "STT";
            dt.Columns[1].ColumnName = "B";
            dt.Columns[2].ColumnName = "Đơn vị";
            dt.Columns[3].ColumnName = "Tên đơn vị";
            dt.Columns[4].ColumnName = "sLNS";
            dt.Columns[5].ColumnName = "sL";
            dt.Columns[6].ColumnName = "sK";
            dt.Columns[7].ColumnName = "sM";
            dt.Columns[8].ColumnName = "sTM";
            dt.Columns[9].ColumnName = "sTTM";
            dt.Columns[10].ColumnName = "sNG";
            dt.Columns[11].ColumnName = "TC";
            dt.Columns[12].ColumnName = "HV";
             String sXau=String.Format(@"sL=460 AND sK=468 AND sM=6500 AND sTM=6501 AND sTTM=00 AND sNG=56");
             DataRow[] dr = dt.Select(sXau);
            cmd.Dispose();
            conn.Close();
            conn.Dispose();

            return RedirectToAction("Index", new {iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet)
        {
          
            XlsFile Result = new XlsFile(true);
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            Result.Open(path);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDT_2bc");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet);
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="MaKhoi"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_DuToanChiNganSachSuDung(String MaKhoi,  String MaND, String iID_MaTrangThaiDuyet)
        {
            String[] dsKhoi;
            dsKhoi = MaKhoi.Split(',');
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < dsKhoi.Length; i++)
            {
                DK += "iID_MaKhoiDonVi=@iID_MaKhoiDonVi" + i;
                cmd.Parameters.AddWithValue("@iID_MaKhoiDonVi" + i, dsKhoi[i]);
                if (i < dsKhoi.Length - 1)
                    DK += " OR ";
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = " AND (" + DK + ")";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan) + "'";
            }
            if (iID_MaTrangThaiDuyet == "1")
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String SQL = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,DT_ChungTuChiTiet.sMoTa";
            SQL += " FROM DT_ChungTuChiTiet";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ) as NS_DonVi ON DT_ChungTuChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi";
            SQL += " WHERE SUBSTRING(sLNS,1,3)='102' AND sL='460' AND sK='468' AND bChiNganSach=1 AND DT_ChungTuChiTiet.iTrangThai=1 {0}  " + ReportModels.DieuKien_NganSach(MaND, "DT_ChungTuChiTiet") + DK;
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,DT_ChungTuChiTiet.sMoTa";
            SQL = string.Format(SQL, iID_MaTrangThaiDuyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);

            cmd.Dispose();
            cmd = new SqlCommand();
            for (int i = 0; i < dsKhoi.Length; i++)
            {
                cmd.Parameters.AddWithValue("@iID_MaKhoiDonVi" + i, dsKhoi[i]);
            }
            String SQLData = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaKhoiDonVi,SUM(rTongSo) as rTongSo";
            SQLData += " FROM DT_ChungTuChiTiet";
            SQLData += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ) as NS_DonVi ON DT_ChungTuChiTiet.iID_MaDonVi=NS_DonVi.iID_MaDonVi";
            SQLData += " WHERE SUBSTRING(sLNS,1,3)='102' AND sL='460' AND sK='468' AND bChiNganSach=1 AND DT_ChungTuChiTiet.iTrangThai=1  {0} " + ReportModels.DieuKien_NganSach(MaND, "DT_ChungTuChiTiet")+ DK;
            SQLData += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaKhoiDonVi";
            SQLData += " HAVING SUM(rTongSo)!=0";
            SQLData = string.Format(SQLData, iID_MaTrangThaiDuyet);
            cmd.CommandText = SQLData;
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dtData = Connection.GetDataTable(cmd);

            DataTable dtKQ = new DataTable();
            for (int i = 0; i < dsKhoi.Length; i++)
            {
                dt.Columns.Add("rK" + i, typeof(Decimal));
            }
            dt.Columns.Add("rT", typeof(Decimal));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Decimal T = 0;
                for (int k = 0; k < dsKhoi.Length; k++)
                {
                    String xauTruyVan = String.Format("sM='{0}' AND sTM='{1}' AND sTTM='{2}' AND sNG='{3}' AND iID_MaKhoiDonVi='{4}'",
                                                       dt.Rows[i]["sM"], dt.Rows[i]["sTM"], dt.Rows[i]["sTTM"], dt.Rows[i]["sNG"], dsKhoi[k]);
                    DataRow[] R = dtData.Select(xauTruyVan);
                    for (int j = 0; j < R.Length; j++)
                    {

                        dt.Rows[i]["rK" + k] = R[0][8];
                        if (R[0][8] != DBNull.Value)
                            T += Convert.ToDecimal(R[0][8]);
                    }
                }

                dt.Rows[i]["rT"] = T;
            }


            return dt;
        }
        /// <summary>
        /// hàm hiển thị dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr,String MaND, String iID_MaTrangThaiDuyet)
        {
            String dsKhoi = "bb052603-be5d-494d-a4bf-7651894d5a0a,2ea265f1-6db9-42d8-8fb3-067356f31bc9,687860da-8dc1-4bca-b810-5cc7d6634846";
            DataTable data = DT_DuToanChiNganSachSuDung(dsKhoi, MaND, iID_MaTrangThaiDuyet);
            data.TableName = "NSSuDung";
            fr.AddTable("NSSuDung", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sM,sTM", "sM,sTM,sMoTa", "sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sM", "sM,sMoTa", "sM,sTM");
            fr.AddTable("Muc", dtMuc);
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
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
        /// Hàm xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>

        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DTChiNganSachSuDung_PhanNghiepVu00.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet);
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
