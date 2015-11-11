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
using DomainModel.Abstract;
using DomainModel.Controls;

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTongHop_ThongTri_PhongBanController : Controller
    {
        //
        // GET: /rptQuyetToanThongTri_PhongBan/     

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKT_ThongTri_PhongBan.xls";
        public static String NameFile = "";
        public int count = 0;
        public long tong = 0;   
        public  String LoaiTT = "";
        public String LoaiNS = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ThongTri_PhongBan.aspx";
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            return View(sViewPath + "ReportView.aspx");
             }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID, String iThang, String iNam)
        {
            String iID_MaChungTu = Convert.ToString(Request.Form[ParentID + "_iID_MaChungTu"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaThongTri = Convert.ToString(Request.Form["ThongTri" + "_iID_MaThongTri"]);
            String chkThemMoi = Convert.ToString(Request.Form[ParentID + "_chkThemMoi"]);
            //if (chkThemMoi == "on")
            //{
            //    String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            //    String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
            //    Bang bang = new Bang("KT_LoaiThongTri");
            //    bang.MaNguoiDungSua = User.Identity.Name;
            //    bang.IPSua = Request.UserHostAddress;
            //    bang.CmdParams.Parameters.AddWithValue("@sLoaiThongTri",sTen);
            //    bang.CmdParams.Parameters.AddWithValue("@sTenLoaiNS", sNoiDung);
            //    iID_MaThongTri = Convert.ToString(bang.Save());
            //}

            String LoaiTK = "0";
            if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = "x";
            if (iID_MaDonVi.Length < 2)
                return RedirectToAction("Index", "KeToanTongHop");
            else
            {
                if (iID_MaDonVi.Substring(0, 2) == "N-") LoaiTK = "0";
                else LoaiTK = "1";
                if (chkThemMoi == "on")
                {
                    string MaTaiKhoanNo = "00000000-0000-0000-0000-000000000000", MaTaiKhoanCo = "00000000-0000-0000-0000-000000000000";
                    DataTable dtTaiKhoan = Lay_DS_TaiKhoan(iID_MaChungTu,
                                                           iID_MaDonVi.Substring(2, iID_MaDonVi.Length - 2), LoaiTK);
                    if (dtTaiKhoan != null && dtTaiKhoan.Rows.Count>0)
                    {
                        MaTaiKhoanNo = HamChung.ConvertToString(dtTaiKhoan.Rows[0]["iID_MaTaiKhoan_No"]);
                        MaTaiKhoanCo = HamChung.ConvertToString(dtTaiKhoan.Rows[0]["iID_MaTaiKhoan_Co"]);
                        dtTaiKhoan.Dispose();
                    }
                    String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
                    String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
                    Bang bang = new Bang("KT_LoaiThongTri");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanNo", MaTaiKhoanNo);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanCo", MaTaiKhoanCo);
                    bang.CmdParams.Parameters.AddWithValue("@sLoaiKhoan", "");
                    bang.CmdParams.Parameters.AddWithValue("@sLoaiThongTri", sTen);
                    bang.CmdParams.Parameters.AddWithValue("@sTenLoaiNS", sNoiDung);
                    iID_MaThongTri = Convert.ToString(bang.Save());
                }
                return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu, iID_MaDonVi = iID_MaDonVi.Substring(2, iID_MaDonVi.Length - 2), LoaiTK = LoaiTK, iThang, iNam, iID_MaThongTri = iID_MaThongTri });
            }
        }
        //Lấy dữ liệu
        public  DataTable rptQuyetToan_ThongTri_PhongBan(String iID_MaChungTu, String iID_MaDonVi, String LoaiTK)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT KT.sNoiDung
                                              , KT.rSoTien
                                              , CT.sSoChungTu
                                              , CT.iNgay, CT.iThang
                                              , CT.iNamLamViec
                                              , KT.iID_MaTaiKhoan_Co
                                              , KT.iID_MaTaiKhoan_No
                                        FROM KT_ChungTuChiTiet AS KT
                                        INNER JOIN KT_ChungTu AS CT 
                                        ON KT.iID_MaChungTu = CT.iID_MaChungTu
                                        WHERE CT.iTrangThai=1 AND KT.iTrangThai=1
                                          AND CT.iID_MaChungTu=@iID_MaChungTu");
            if (LoaiTK == "0")//tài khoản nợ
                SQL += " AND KT.iID_MaDonVi_No=@iID_MaDonVi";
            else SQL += " AND KT.iID_MaDonVi_Co=@iID_MaDonVi";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);            
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                    tong += long.Parse(dt.Rows[i]["rSoTien"].ToString());
                ngayGS = "Ghi sổ số " + HamChung.ConvertToString(dr["sSoChungTu"]) + ", ngày  " + HamChung.ConvertToString(dt.Rows[0]["iNgay"]) + " tháng  " +
                    HamChung.ConvertToString(dt.Rows[0]["iThang"]) + "  năm  " +
                    HamChung.ConvertToString(dt.Rows[0]["iNamLamViec"]);
                string TaiKhoanNo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_No"]);
                string TaiKhoanCo = HamChung.ConvertToString(dr["iID_MaTaiKhoan_Co"]);
                LoaiThongTriModels.LayLoaiThongTri(TaiKhoanNo, TaiKhoanCo, ref LoaiNS, ref LoaiTT);            
            }
            return dt;
        }
        //Lấy dữ liệu
        public DataTable Lay_DS_TaiKhoan(String iID_MaChungTu, String iID_MaDonVi, String LoaiTK)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (LoaiTK == "0")//tài khoản nợ
                SQL += " select iID_MaTaiKhoan_Co, iID_MaTaiKhoan_No from KT_ChungTuChiTiet where iTrangThai=1 and iID_MaChungTu=@iID_MaChungTu and iID_MaDonVi_No=@iID_MaDonVi";
            else SQL += " select iID_MaTaiKhoan_Co, iID_MaTaiKhoan_No from KT_ChungTuChiTiet where iTrangThai=1 and iID_MaChungTu=@iID_MaChungTu and iID_MaDonVi_Co=@iID_MaDonVi";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
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
        public static String ngayGS = "";
        public static String Thang = "";
        public ExcelFile CreateReport(String path, String iID_MaChungTu, String iID_MaDonVi, String LoaiTK, String iThang, String iNam, String iID_MaThongTri)
        {
           
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            Thang = iThang + "/" + iNam;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);            
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTongHop_ThongTri_PhongBan");
            LoadData(fr, iID_MaChungTu, iID_MaDonVi, LoaiTK);
            fr.SetValue("TenDV", DonViModels.Get_TenDonVi(iID_MaDonVi));
            fr.SetValue("ThangNam", Thang);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("NgayGS", ngayGS);
            fr.SetValue("Ngay", ngay);
            //lay ten thong tri
            if (!String.IsNullOrEmpty(iID_MaThongTri))
            {
                DataTable dtLoaiThongTri;
                String SQL = "SELECT sLoaiThongTri,sTenLoaiNS FROM KT_LoaiThongTri WHERE iID_MaThongTri=@iID_MaThongTri";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaThongTri", iID_MaThongTri);
                dtLoaiThongTri = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dtLoaiThongTri != null && dtLoaiThongTri.Rows.Count > 0)
                {
                    LoaiTT = Convert.ToString(dtLoaiThongTri.Rows[0]["sLoaiThongTri"]);
                    LoaiNS = Convert.ToString(dtLoaiThongTri.Rows[0]["sTenLoaiNS"]);
                    dtLoaiThongTri.Dispose();
                }
            }
            else
            {
                LoaiTT = "Thông tri Cấp";
                LoaiNS = "Kinh phí...";
            }
            fr.SetValue("LoaiTT", LoaiTT.ToUpper());
            fr.SetValue("LoaiNS", LoaiNS.ToUpper());
            fr.SetValue("Tien", CommonFunction.TienRaChu(tong));
            fr.Run(Result);
            return Result;            
        }       
        
        private void LoadData(FlexCelReport fr, String iID_MaChungTu, String iID_MaDonVi, String LoaiTK)
        {
            DataTable data = rptQuyetToan_ThongTri_PhongBan(iID_MaChungTu, iID_MaDonVi, LoaiTK);          
            data.TableName = "ChiTiet";
            if (data.Rows.Count > 0 && data.Rows.Count < 15)
            {
                int count = data.Rows.Count;
                for (int i = 0; i < 15 - count; i++)
                {
                    DataRow dR = data.NewRow();
                    data.Rows.Add(dR);
                }
            }
            fr.AddTable("ChiTiet", data);              
            data.Dispose();
        }

        public static DataTable GetsoCT()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("select kt.sSoChungTu from KT_ChungTu kt order by kt.sSoChungTu");
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiTK"></param>
        /// <param name="iThang"></param>
        /// <param name="iNam"></param>
        /// <returns></returns>

        public ActionResult ViewPDF(String iID_MaChungTu, String iID_MaDonVi, String LoaiTK, String iThang, String iNam, String iID_MaThongTri)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaChungTu, iID_MaDonVi, LoaiTK, iThang, iNam, iID_MaThongTri);
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
        public static DataTable getDSLoaiThongTri()
        {
            String SQL = "SELECT * FROM KT_LoaiThongTri WHERE iTrangThai=1";
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
    }
}