using System;
using System.Collections.Generic;
using System.Collections;
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
using DomainModel.Abstract;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptThuNopThongTriController : Controller
    {
      
        //
        // GET: /rptQuyetToanThongTri_PhongBan/     

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/ThuNop/rptThuNopThongTri.xls";
        public static String NameFile = "";
        public int count = 0;
     
        public String LoaiTT = "";
        public String LoaiNS = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["path"] = "~/Views/ThuNop/rptThuNop_ThongTri.aspx";
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
            String iID_MaChungTu = Convert.ToString(Request.Form[ParentID + "_iID_MaChungTu"]).Trim();
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]).Trim();
            String sSoCT = Convert.ToString(Request.Form[ParentID + "_sSoCT"]).Trim();
            String iLoai = Convert.ToString(Request.Form[ParentID + "_iLoai"]).Trim();
            String sGhiChu = Convert.ToString(Request.Form[ParentID + "_sGhiChu"]).Trim();
            String iID_MaThongTri = Convert.ToString(Request.Form["ThongTri" + "_iID_MaThongTri"]);
            String chkThemMoi = Convert.ToString(Request.Form[ParentID + "_chkThemMoi"]);
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sSoCT"] = sSoCT;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaThongTri"] = iID_MaThongTri;
            ViewData["iThang"] = iThang;
            ViewData["iNam"] = iNam;
            ViewData["PageLoad"] = 1;
            ViewData["iLoai"] = iLoai;
          
           
                sGhiChu=sGhiChu.Replace("\r\n", "&#10;");
                Bang bangTN = new Bang("TN_ChungTu");
                bangTN.MaNguoiDungSua = User.Identity.Name;
                bangTN.IPSua = Request.UserHostAddress;
                bangTN.GiaTriKhoa = iID_MaChungTu;
                bangTN.DuLieuMoi = false;
                bangTN.CmdParams.Parameters.AddWithValue("@sGhiChu", sGhiChu);
                bangTN.Save();
            
            if (iID_MaDonVi.Length < 2)
                return RedirectToAction("Index", "ThuNop_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            else
            {
               
                if (chkThemMoi == "on")
                {
                   
                   
                    String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
                    String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
                    Bang bang = new Bang("KT_LoaiThongTri");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                  
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanHe", ThuNopModels.iID_MaPhanHe);
                    bang.CmdParams.Parameters.AddWithValue("@sLoaiKhoan", "");
                    bang.CmdParams.Parameters.AddWithValue("@sLoaiThongTri", sTen);
                    bang.CmdParams.Parameters.AddWithValue("@sTenLoaiNS", sNoiDung);
                    iID_MaThongTri = Convert.ToString(bang.Save());
                    ViewData["iID_MaThongTri"] = iID_MaThongTri;
                }
                ViewData["path"] = "~/Report_Views/ThuNop/rptThuNop_InThongTri.aspx";
                return View(sViewPath + "ReportView_NoMaster.aspx");
                //return View(sViewPath + "ThuNop/rptThuNop_InThongTri.aspx");
               
            }
        }
        //Lấy dữ liệu
        public DataTable rptQuyetToan_ThongTri_PhongBan(String iID_MaChungTu, String iID_MaDonVi, String sSoCT,String iLoai)
        {
            String DK = "";
            if (iLoai == "1")
                DK = "rNopNSQP";
            else if (iLoai=="2")
            {
                DK = "rNopNSNNQuaBQP";
            }
            else
            {
                DK = "rNopNSNNKhac";
            }
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT a.sLNS,sMoTa,rSoTien FROM (
select  TN_sLNS as sLNS,
rSoTien=SUM({0})
 from TN_ChungTuChiTiet
  where iTrangThai=1 AND sSoCT=@sSoCT
   and iID_MaDonVi=@iID_MaDonVi and iID_MaChungTu=@iID_MaChungTu 
   group by TN_sLNS HAVING SUM({0})<>0) as a
   INNER JOIN
   (
   SELECT sLNS,sMoTa
   FROM NS_MucLucNganSach
   WHERE iTrangThai=1 AND SUBSTRING(sLNS,1,1)='8' AND LEN(sLNS)=7 AND sL='')  as b
   ON a.sLNS=b.sLNS
   ", DK);
           
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sSoCT", sSoCT);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
          
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
        public ExcelFile CreateReport(String path, String iID_MaChungTu, String iID_MaDonVi, String iThang, String iNam, String iID_MaThongTri, String sSoCT, String iLoai)
        {

          
            Thang = iThang + "/" + iNam;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThuNopThongTri");
            LoadData(fr, iID_MaChungTu, iID_MaDonVi, sSoCT, iLoai);
          
            ////lay ten thong tri
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
                LoaiTT = "Thu nộp Ngân sách";
                LoaiNS = "Ngân sách...";
            }
            String NoiDung = "", sNgayLap = "", Nam = "", dNgayChungTu="";
            DataTable dt = ThuNop_ChungTuModels.GetChungTu(iID_MaChungTu);
           
            if (dt.Rows.Count > 0)
            {
                NoiDung = dt.Rows[0]["sNoiDung"].ToString();
                dNgayChungTu = dt.Rows[0]["dNgayChungTu"].ToString();
                Nam = dt.Rows[0]["iNamLamViec"].ToString();
                dt.Dispose();
               
            }
            if (!String.IsNullOrEmpty(dNgayChungTu))
            {
                sNgayLap = " năm  " + Nam;
            }
            fr.SetValue("NgayThangNam", "Ngày "+dNgayChungTu.Substring(0, 2)+ " tháng  " + dNgayChungTu.Substring(3, 2) + "  năm  " + dNgayChungTu.Substring(6, 4));
            fr.SetValue("NoiDung", NoiDung);
            fr.SetValue("Loai", LoaiTT);
            fr.SetValue("DonVi", DonViModels.Get_TenDonVi(iID_MaDonVi));
            fr.SetValue("dNgayLap", sNgayLap);
            fr.SetValue("LNS", LoaiNS);
            fr.SetValue("sL", "");
            fr.SetValue("sK", "");
           
            fr.SetValue("Nam", Nam);
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.Run(Result);
            return Result;
        }

        private void LoadData(FlexCelReport fr, String iID_MaChungTu, String iID_MaDonVi, string sSoCT, String iLoai)
        {
            DataTable data = rptQuyetToan_ThongTri_PhongBan(iID_MaChungTu, iID_MaDonVi, sSoCT, iLoai);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            
            //DataTable dtLNS = HamChung.SelectDistinct("LNS", data, "sLNS", "sLNS,sL,sK,sM,sTM,sTTM,sMoTa", "sLNS,sL");
            //fr.AddTable("sLNS", dtLNS);
           
            int a = data.Rows.Count;
            //int b = dtLNS.Rows.Count;
            int count = 15 - (a );
            DataTable dt = new DataTable();
            dt.Columns.Add("sGhiChu", typeof (String));
            int soChu1Trang = 54;
            String sGhiChu = Convert.ToString(CommonFunction.LayTruong("TN_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sGhiChu"));
           ArrayList   arrDongTong = new ArrayList();
            String[] arrDong = Regex.Split(sGhiChu, "&#10;");
            
            for (int i = 0; i < arrDong.Length; i++)
            {
                if (arrDong[i] != "")
                {
                    int tg = 0;
                    String s = "";
                    String[] arrDongCon = arrDong[i].Split(' ');
                    for (int j = 0; j < arrDongCon.Length; j++)
                    {
                         if (arrDongCon[j] != "")
                         {
                             int x = arrDongCon[j].Length;
                                 tg =tg+ x+1;
                             if(tg>soChu1Trang)
                             {
                                 arrDongTong.Add(s);
                                 j--;
                                 tg = 0;
                                 s = "";
                                 continue;
                             }
                             s += arrDongCon[j].Trim() + " ";
                         }
                    }
                    if (tg <= soChu1Trang) arrDongTong.Add(s);
                   
                }
            }
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DataRow r = dt.NewRow();
                   
                        for (int j = i; j <arrDongTong.Count; j++)
                        {
                            r["sGhiChu"] = arrDongTong[j];
                            break;
                        }
                    dt.Rows.Add(r);
                }
            }
            fr.AddTable("dtDongTrang", dt);
            long TongTien = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["rSoTien"].ToString() != "0")
                {
                    TongTien += long.Parse(data.Rows[i]["rSoTien"].ToString());
                }
            }
            fr.SetValue("Tien", CommonFunction.TienRaChu(TongTien));
            data.Dispose();
            //dtLNS.Dispose();
            dt.Dispose();
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

        public ActionResult ViewPDF(String iID_MaChungTu, String iID_MaDonVi, String iThang, String iNam, String iID_MaThongTri, String sSoCT, String iLoai)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaChungTu, iID_MaDonVi, iThang, iNam, iID_MaThongTri, sSoCT, iLoai);
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
        public static DataTable getDSLoaiThongTri(String MaND)
        {
            String SQL = "SELECT * FROM KT_LoaiThongTri WHERE iTrangThai=1 AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao AND iID_MaPhanHe=@iID_MaPhanHe ORDER BY dNgayTao";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", ThuNopModels.iID_MaPhanHe);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaChungTu, String sSoCT, String iID_MaDonVi)
        {
            return Json(obj_DonVi(ParentID, iID_MaChungTu, sSoCT, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }
        public String obj_DonVi(String ParentID, String iID_MaChungTu, String sSoCT, String iID_MaDonVi)
        {
            String input = "";
            DataTable dt = DonViModels.DanhSach_DonVi_ThuNop(iID_MaChungTu, sSoCT);
            SelectOptionList slDonvi= new SelectOptionList(dt,"iID_MaDonVi","sTen");
            input = MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            return input;
        }
    }
}
