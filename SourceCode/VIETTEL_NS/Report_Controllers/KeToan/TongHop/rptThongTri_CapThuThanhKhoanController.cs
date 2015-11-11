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
using VIETTEL.Report_Views;
using System.IO;


namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptThongTri_CapThuThanhKhoanController : Controller
    {
       
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptThongTri_CapThuThanhKhoan.xls";
        private const String sFilePath1 = "/Report_ExcelFrom/KeToan/TongHop/rptThongTri_CapThuThanhKhoan1.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptThongTri_CapThuThanhKhoan.aspx";
                return View(sViewPath + "ReportView.aspx");
             }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String iNamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String Loai = Convert.ToString(Request.Form[ParentID + "_Loai"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThang"] = iThang;
            ViewData["Loai"] = Loai;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptThongTri_CapThuThanhKhoan.aspx";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,Loai=Loai });
        }
        public ExcelFile CreateReport(String path, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet,String Loai)
        {
           
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String TenTK = "";
            DataTable dtTK = dtTenTaiKhoan(iID_MaTaiKhoan);
            if (dtTK.Rows.Count > 0)
            {
                TenTK = dtTK.Rows[0][0].ToString();
            }
            else
            {
                TenTK = "";
            }
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            string ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongTri_CapThuThanhKhoan");
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LoadData(fr, iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang,iID_MaTrangThaiDuyet,Loai);
            fr.SetValue("TenTaiKhoan", iID_MaTaiKhoan + "-" + TenTK);
            fr.SetValue("Ma", iID_MaTaiKhoan);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("Nams", DateTime.Now.Year);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("ngay", ngay);
            fr.SetValue("DV", CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen")); 
            
            fr.Run(Result);
            return Result;

        }
        public clsExcelResult ExportToExcel(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet,String Loai)
        {
            HamChung.Language();
            String DuongDan = "";
            if (Loai == "0")
            {
                DuongDan = sFilePath;
            }
            else 
            {
                DuongDan = sFilePath1;
            }
           
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyet, Loai);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public clsExcelResult ExportToPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet,String Loai)
        {
            HamChung.Language();
            String DuongDan = "";
            if (Loai == "0")
            {
                DuongDan = sFilePath;
            }
            else 
            {
                DuongDan = sFilePath1;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyet, Loai);

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
        public ActionResult ViewPDF(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyet,String Loai)
        {
            HamChung.Language();
           
            String DuongDan = "";
            if (Loai == "0")
            {
                DuongDan = sFilePath;
            }
            else 
            {
                DuongDan = sFilePath1;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyet, Loai);
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

        private void LoadData(FlexCelReport fr, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyets,String Loai)
        {
            DataTable data = Thongtricapthuthanhkhoan(iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan, iThang, iID_MaTrangThaiDuyets, Loai);
            data.TableName = "ChiTiet";
            int a = data.Rows.Count;
            if (a < 15 && a > 0)
            {
                for (int i = 0; i < (15 - a); i++)
                {
                    DataRow r = data.NewRow();
                    data.Rows.Add(r);
                }
            }
            fr.AddTable("ChiTiet", data);
            data.Dispose();
           
        }
      
        public DataTable Thongtricapthuthanhkhoan(String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan, String iThang, String iID_MaTrangThaiDuyets, String Loai)
        {
            String DKDuyet = iID_MaTrangThaiDuyets.Equals("0") ? "" : "and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
            String DK = " AND iID_MaTaiKhoan_No IN (";
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            SqlCommand cmd = new SqlCommand();
            //SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                DK += " @iID_MaTaiKhoan" + i;
                if (i < arrTaiKhoan.Length - 1)
                    DK += " , ";

            }
            DK += " ) ";
            String DK1 = " AND iID_MaTaiKhoan_Co IN (";
            String[] arrTaiKhoan1 = iID_MaTaiKhoan.Split(',');
            //SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrTaiKhoan1.Length; i++)
            {
                DK1 += " @iID_MaTaiKhoan" + i;
                if (i< arrTaiKhoan1.Length - 1)
                    DK1 += " , ";

            }
            DK1 += " ) ";
            if (Loai == "1")
            {
                String SQL = " SELECT iID_MaTaiKhoan_No as taikhoanNo,N'Thu thanh khoản' + '_' + sTenTaiKhoan_No as sTenTaiKhoan_No,sum(rSoTien)as SoTien ";
                SQL += " FROM KT_ChungTuChiTiet";
                SQL += " WHERE iID_MaDonVi_No =@iID_MaDonVi {0} and iThangCT=@iThang {1} AND iNamLamViec = @iNamLamViec";
                SQL += " group by sTenTaiKhoan_No,iID_MaTaiKhoan_No";
                SQL = String.Format(SQL, DK, DKDuyet, DK1);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                if (!String.IsNullOrEmpty(DKDuyet))
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);

                for (int i = 0; i < arrTaiKhoan1.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan1[i]);
                }
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
            else //(Loai =="0")
            {
             
                String SQL1 = " SELECT iID_MaTaiKhoan_Co as taikhoanCo,N'Cấp thanh khoản' + '_' + sTenTaiKhoan_Co as sTenTaiKhoan_Co, sum(rSoTien)as SoTien";
                SQL1 += " FROM KT_ChungTuChiTiet"; 
                SQL1 += " WHERE iID_MaDonVi_Co =@iID_MaDonVi {2} and iThangCT=@iThang {1} AND iNamLamViec = @iNamLamViec";
                SQL1 += " group by iID_MaTaiKhoan_Co,sTenTaiKhoan_Co";
                SQL1 = String.Format(SQL1, DK, DKDuyet, DK1);
                cmd.CommandText = SQL1;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                if (!String.IsNullOrEmpty(DKDuyet))
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);

                for (int i = 0; i < arrTaiKhoan1.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan1[i]);
                }
                //for (int i = 0; i < arrTaiKhoan.Length; i++)
                //{
                //    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
                //}
                DataTable dt1 = Connection.GetDataTable(cmd);
                cmd.Dispose();
                
                return dt1;
            }
            
            
        }
        public static DataTable TenTaiKhoan(String NamChungTu)
        {
            DataTable dt;
            String KyHieu = "72";
            String[] arriID_MaTaiKhoan;
            String iID_MaTaiKhoan = "";
            String SQL = String.Format(@"SELECT sThamSo as iID_MaTaiKhoan  FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sKyHieu=@sKyHieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", KyHieu);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            //DataTable dtThamSo = Connection.GetDataTable(cmdThamSo);
            //arrThamSo = Convert.ToString(dtThamSo.Rows[0]["sThamSo"]).Split(',');

            //for (int i = 0; i < arrThamSo.Length; i++)
            //{
            //    ThamSo += arrThamSo[i];
            //    if (i < arrThamSo.Length - 1)
            //        ThamSo += " , ";
            //}

            //String SQL = String.Format(@"SELECT iID_MaTaiKhoan,sTen+'-'+iID_MaTaiKhoan as TenTK FROM KT_TaiKhoan WHERE iID_MaTaiKhoan IN ({0}) AND iNam=@Nam", ThamSo);
            //SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@Nam", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        //dt lấy tên tài khoản
        public static DataTable dtTenTaiKhoan(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        public static DataTable dtDonVi(String iThang, String iNamLamViec)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDonVi_No as iID_MaDonVi, sTenDonVi_No as Ten from KT_ChungTuChiTiet Where iThangCT=@iThang and iNamLamViec =@iNamLamViec and iID_MaDonVi_No<>'' AND sTenDonVi_No<>'' group by iID_MaDonVi_No,sTenDonVi_No union Select iID_MaDonVi_Co as iID_MaDonVi,sTenDonVi_Co as Ten from KT_ChungTuChiTiet Where iThangCT=@iThang and iNamLamViec =@iNamLamViec and iID_MaDonVi_Co<>''  AND sTenDonVi_Co<>'' group by iID_MaDonVi_Co,sTenDonVi_Co");
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            return dt = Connection.GetDataTable(cmd);
        }
        public static DataTable DanhSach_Loai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "0";
            R1[1] = "Cấp thanh khoản";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "1";
            R2[1] = "Thu thanh khoản";
            dt.Dispose();
            return dt;

        }
        public static DataTable thu (String iThang, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan)
        {
           
            String DK = " AND iID_MaTaiKhoan_No IN (";
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            SqlCommand cmd = new SqlCommand();
            //SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                DK += " @iID_MaTaiKhoan" + i;
                if (i < arrTaiKhoan.Length - 1)
                    DK += " , ";

            }
            DK += " ) ";
           
            String SQL = " SELECT iID_MaDonVi_No as iID_MaDonVi,sum(rSoTien)as rSoTien ";
                SQL += " FROM KT_ChungTuChiTiet";
                SQL += " WHERE iID_MaDonVi_No =@iID_MaDonVi  and iThangCT=@iThang  AND iNamLamViec = @iNamLamViec  {0} group by iID_MaDonVi_No ";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                for (int i = 0; i < arrTaiKhoan.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
                }
                DataTable dt = Connection.GetDataTable(cmd);
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(dt.NewRow());
                }
               //Double Thu = Convert.ToDouble(dt.Rows[0]["rSoTien"]);
                cmd.Dispose();
                return dt;
        }
        public static DataTable Cap (String iThang, String iID_MaDonVi, String iNamLamViec, String iID_MaTaiKhoan)
        {
             String DK = " AND iID_MaTaiKhoan_Co IN (";
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            SqlCommand cmd = new SqlCommand();
            //SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrTaiKhoan.Length; i++)
            {
                DK += " @iID_MaTaiKhoan" + i;
                if (i < arrTaiKhoan.Length - 1)
                    DK += " , ";

            }
            DK += " ) ";

                String SQL1 = " SELECT  iID_MaDonVi_Co as iID_MaDonVi,sum(rSoTien)as rSoTien";
                SQL1 += " FROM KT_ChungTuChiTiet";
                SQL1 += "  WHERE iID_MaDonVi_Co =@iID_MaDonVi and iThangCT=@iThang  AND iNamLamViec = @iNamLamViec {0}";
                SQL1 += " group by iID_MaDonVi_Co";
                SQL1 = String.Format(SQL1, DK);
                cmd.CommandText = SQL1;
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                for (int i = 0; i < arrTaiKhoan.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
                }
                DataTable dt1 = Connection.GetDataTable(cmd);
                if (dt1.Rows.Count == 0)
                {
                    dt1.Rows.Add(dt1.NewRow());
                }
               
                //Double Cap = Convert.ToDouble(dt1.Rows[0]["rSoTien"]);
                cmd.Dispose();
                return dt1;
            
        }
        //Don vi
        public JsonResult ObjDanhSachDonVi(String ParentID, String iThang, String iNamLamviec, String iID_MaDonVi)
        {
            return Json(get_sDanhSachDonVi(ParentID, iThang, iNamLamviec, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String ParentID,String iThang, String iNamLamviec,String iID_MaDonVi)
        {
            DataTable dtDonVi = rptThongTri_CapThuThanhKhoanController.dtDonVi(iThang, iNamLamviec);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "Ten");
            return MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", " onchange=\"chonCap()\" class=\"input1_2\" style=\"width: 200px;height:50px; font-size:13px;\" size='3' tab-index='-1'");
        }
        //cap
        public JsonResult ObjCap(String ParentID, String iThang, String iNamLamviec, String iID_MaDonVi,String iID_MaTaiKhoan)
        {
            return Json(get_sCap(ParentID, iThang, iNamLamviec, iID_MaDonVi, iID_MaTaiKhoan), JsonRequestBehavior.AllowGet);
        }

        public Object get_sCap(String ParentID, String iThang, String iNamLamviec, String iID_MaDonVi, String iID_MaTaiKhoan)
        {
            String Loai = "0";
            DataTable dtcap = rptThongTri_CapThuThanhKhoanController.Cap(iThang, iID_MaDonVi, iNamLamviec, iID_MaTaiKhoan);
            SelectOptionList slcap = new SelectOptionList(dtcap, "iID_MaDonVi", "rSoTien");
            DataTable dtThu = rptThongTri_CapThuThanhKhoanController.thu(iThang, iID_MaDonVi, iNamLamviec, iID_MaTaiKhoan);
            SelectOptionList slthu = new SelectOptionList(dtThu, "iID_MaDonVi", "rSoTien");
            if (dtcap.Rows.Count > 0 && dtcap.Rows[0]["rSoTien"] != DBNull.Value && Convert.ToDouble(dtcap.Rows[0]["rSoTien"]) > 0)
            {
                Loai = "0";
            }
            else if (dtThu.Rows.Count > 0 && dtThu.Rows[0]["rSoTien"] != DBNull.Value && Convert.ToDouble(dtThu.Rows[0]["rSoTien"]) > 0)
            {
                Loai = "1";
            }
            String sCap=MyHtmlHelper.Option(ParentID, "0", Loai, "Loai", "") +"<span>1. Cấp thanh khoản</span>";
            sCap += MyHtmlHelper.DropDownList(ParentID, slcap, iID_MaDonVi, "rSoTien", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"");
            String sThu=MyHtmlHelper.Option(ParentID, "1", Loai, "Loai", "")+ "<span> 2. Thu thanh khoản</span>";
            sThu += MyHtmlHelper.DropDownList(ParentID, slthu, iID_MaDonVi, "rSoTien", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"");

            Object data = new
            {
                sCap = sCap,
                sThu = sThu
            };
            return data;
        }



    }
}

