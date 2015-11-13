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

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class rptQuyetToan_ThongTri_PhongBanController : Controller
    {
        //
        // GET: /rptQuyetToanThongTri_PhongBan/     

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptQuyetToanThongTri_PhongBan.xls";
        public static String NameFile = "";
        public int count = 0;
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptQuyetToanThongTri_PhongBan.aspx";
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
            String SoCT = Convert.ToString(Request.Form[ParentID + "_iSoCT"]);           
            String iID_MaPhongBan = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan"]);
            String LoaiTK = Convert.ToString(Request.Form[ParentID + "_LoaiTK"]);
            String TKCo = Convert.ToString(Request.Form[ParentID + "_iTKCo"]);
            String TKNo = Convert.ToString(Request.Form[ParentID + "_iTKNo"]);
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            return RedirectToAction("Index", new { SoCT = SoCT, iID_MaPhongBan = iID_MaPhongBan, LoaiTK = LoaiTK, TKCo = TKCo, TKNo = TKNo });
        }
        //Lấy dữ liệu
        public static DataTable rptQuyetToan_ThongTri_PhongBan( String SoCT, String iID_MaPhongBan,String LoaiTK, String TKNo,String TKCo )
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (LoaiTK == "0")
            {
                DK += " CTCT.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_Co ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", TKCo);
            }
            else if(LoaiTK=="1")
            {
                DK += " CTCT.iID_MaTaiKhoan_No=@iID_MaTaiKhoan_No ";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No", TKNo);
            }
            String SQL = String.Format(@"select CTCT.sNoiDung,
                                        CTCT.rSoTien, 
                                        CT.iNgay, 
                                        CT.iThang,CT.iNamLamViec
                                        ,LTR.sLoaiThongTri
                                        ,LTR.sTenLoaiNS
                                        ,CTCT.sSoChungTuChiTiet
                                        ,CTCT.iID_MaTaiKhoan_Co
                                        ,CTCT.iID_MaTaiKhoan_No
                                        FROM KT_ChungTuChiTiet as CTCT
                                        INNER JOIN KT_ChungTu as CT
                                        ON CTCT.iID_MaChungTu=CT.iID_MaChungTu                                        
                                        INNER JOIN  KT_LoaiThongTri AS LTR
                                        ON LTR.iID_MaTaiKhoanCo=CTCT.iID_MaTaiKhoan_Co
                                        WHERE
                                            {0}
                                            AND CT.iSoChungTu=@iSoChungTu
                                            AND CTCT.iID_MaPhongBan=@iID_MaPhongBan
                                            AND CTCT.iTrangThai=1
                                             --AND CTCT.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iSoChungTu", SoCT);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        //
        public static DataTable getTaiKhoan()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("select DISTINCT TK.iID_MaTaiKhoan FROM KT_TaiKhoan AS  TK order by TK.iID_MaTaiKhoan");
            return dt = Connection.GetDataTable(cmd);
        }
        //Tạo file báo cáo
        public ExcelFile CreateReport(String path, String SoCT, String iID_MaPhongBan, String LoaiTK, String TKNo, String TKCo)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);            
            using (FlexCelReport fr = new FlexCelReport())
            {
                LoadData(fr, SoCT, iID_MaPhongBan,LoaiTK,TKNo,TKCo);               
                fr.SetValue("SoCT", SoCT);
                fr.SetValue("LoaiTR", LoaiTR);
                fr.SetValue("TenNS", TenTR);
                fr.SetValue("ThangNam", ThangNam);
                fr.SetValue("TenDV", Convert.ToString(CommonFunction.LayTruong("NS_PhongBan", "iID_MaPhongBan", iID_MaPhongBan, "sTen")));
                fr.Run(Result);
                return Result;
            }
        }
        //Số chứng từ
        public string ThangNam = "......";
        //Loại thông tri
        public string LoaiTR = "Thông tri";
        //
        public string TenTR = ".......";
        //
        public string So = "...";
        //
        public string TKno = "....",TKco=".....";
        //Đổ dữ liệu xuống file báo cáo
        private void LoadData(FlexCelReport fr, String SoCT, String iID_MaPhongBan, String LoaiTK, String TKNo, String TKCo)
        {
            DataTable data = rptQuyetToan_ThongTri_PhongBan(SoCT, iID_MaPhongBan, LoaiTK, TKNo, TKCo);
            if (data.Rows.Count != 0  && data.Rows.Count<=15)
                for (int i = 0; i <= 15 - data.Rows.Count; ++i)
                {
                    DataRow r = data.NewRow();
                    data.Rows.Add(r);
                }
            if (data.Rows.Count != 0)
            {               
                ThangNam = data.Rows[0]["iThang"].ToString() + "/" + data.Rows[0]["iNamLamViec"].ToString();
                if(data.Rows[0]["sLoaiThongTri"].ToString()!="")
                    LoaiTR = data.Rows[0]["sLoaiThongTri"].ToString();
                So = data.Rows[0]["sSoChungTuChiTiet"].ToString();
                TKco = data.Rows[0]["iID_MaTaiKhoan_Co"].ToString();
                TKno = data.Rows[0]["iID_MaTaiKhoan_Co"].ToString();
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                TenTR += data.Rows[i]["sTenLoaiNS"].ToString();
            }
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            count = data.Rows.Count;            
            data.Dispose();
        }
        public static DataTable GetsoCT()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("select kt.iSoChungTu from KT_ChungTu kt order by iSoChungTu");
            return dt = Connection.GetDataTable(cmd);
        }
        public clsExcelResult ExportToPDF(String SoCT, String iID_MaPhongBan, String LoaiTK, String TKNo, String TKCo)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), SoCT, iID_MaPhongBan, LoaiTK, TKNo, TKCo);
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
        public ActionResult ViewPDF(String SoCT, String iID_MaPhongBan, String LoaiTK, String TKNo, String TKCo)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), SoCT, iID_MaPhongBan,LoaiTK,TKNo,TKCo);
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
    }
}