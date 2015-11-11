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

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_GiaiThichSoDuController : Controller
    {
        
         //GET: /rptKeToanTongHop_GaiThichSoDu/
        public string sViewPath = "~/Report_Views/";
        public String sFilePath_a4 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDu.xls";
        public String sFilePath_a3 = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDu_a3.xls";
        public ActionResult Index(String iNam, String iThang, String optThu, String optTamUng, String optTra, String KhoGiay)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
            ViewData["iNam"] = iNam;
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_GiaiThichSoDu.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ExcelFile CreateReport(String path, String iNam, String iThang, String optThu, String optTamUng, String optTra,String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String strThang = "";
   
             FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_GiaiThichSoDu");
            DataTable dt = DanhMuc_BaoCao_ChuKyModels.Get_dtLayThongTinChuKy("rptKeToanTongHop_GiaiThichSoDu_1");
            if (dt != null && dt.Rows.Count > 0)
            {

                fr.SetValue("ThuaLenh11", dt.Rows[0]["sTenThuaLenh1"]);
                fr.SetValue("ThuaLenh22", dt.Rows[0]["sTenThuaLenh2"]);
                fr.SetValue("ThuaLenh33", dt.Rows[0]["sTenThuaLenh3"]);
                fr.SetValue("ChucDanh11", dt.Rows[0]["sTenChucDanh1"]);
                fr.SetValue("ChucDanh22", dt.Rows[0]["sTenChucDanh2"]);
                fr.SetValue("ChucDanh33", dt.Rows[0]["sTenChucDanh3"]);
                fr.SetValue("Ten11", dt.Rows[0]["sTen1"]);
                fr.SetValue("Ten22", dt.Rows[0]["sTen2"]);
                fr.SetValue("Ten33", dt.Rows[0]["sTen3"]);
            }
            else
            {

                fr.SetValue("ThuaLenh11", "");
                fr.SetValue("ThuaLenh22", "");
                fr.SetValue("ThuaLenh33", "");


                fr.SetValue("ChucDanh11", "");
                fr.SetValue("ChucDanh22", "");
                fr.SetValue("ChucDanh33", "");
                fr.SetValue("Ten11", "");
                fr.SetValue("Ten22", "");
                fr.SetValue("Ten33", "");
            }
                LoadData(fr, iNam, iThang,optThu,optTamUng,optTra,KhoGiay);
                fr.SetValue("Nam", iNam);
                fr.SetValue("Thang", strThang);
                fr.Run(Result);
                return Result;
        }

        public clsExcelResult ExportToExcel(String iNam, String iThang, String optThu, String optTamUng, String optTra,String KhoGiay)
        {
             HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "1") DuongDan = sFilePath_a3;
            else DuongDan = sFilePath_a4;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang, optThu, optTamUng, optTra,KhoGiay);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "GiaiThichSoDu.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ActionResult ViewPDF(String iNam, String iThang,String optThu,String optTamUng,String optTra,String KhoGiay)
        {
            HamChung.Language();
            String DuongDan = "";
            if (KhoGiay == "1") DuongDan = sFilePath_a3;
            else DuongDan = sFilePath_a4;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang, optThu, optTamUng, optTra,KhoGiay);

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
         //<summary>
         //Đẩy dữ liệu xuống báo cáo 
         //</summary>
         //<param name="fr"></param>
         //<param name="NamLamViec"></param>
         //<param name="sNG"></param>
        private void LoadData(FlexCelReport fr, String NamLamViec, String Thang, String optThu, String optTamUng, String optTra,String KhoGiay)
        {
            data _data = Get_dataGiaiThich(Thang, NamLamViec, optThu, optTamUng, optTra);
            DataTable dtPhaiThu = _data.dtThu;
            DataTable dtTamTung = _data.dtTamUng;
            DataTable dtPhaiTra = _data.dtPhaiTra;
            DataTable dt = new DataTable();
            dt.Columns.Add("sNoiDung_ThuTamUng", typeof(String));
            dt.Columns.Add("sLyDo_ThuTamUng", typeof(String));
            dt.Columns.Add("rSoTien_ThuTamUng", typeof(decimal));
            dt.Columns.Add("sNoiDung_PhaiTra", typeof(String));
            dt.Columns.Add("sLyDo_PhaiTra", typeof(String));
            dt.Columns.Add("rSoTien_PhaiTra", typeof(decimal));
            dt.Columns.Add("InDam", typeof(string));
            dt.Columns.Add("InDam1", typeof(string));
            DataRow R;

            decimal tong = 0;
            //thu
            R = dt.NewRow();
            R["sLyDo_ThuTamUng"] = "           1.Các khoản phải thu-TK 311";
            R["InDam"]="1";
            dt.Rows.Add(R);
            for (int i = 0; i < dtPhaiThu.Rows.Count; i++)
            {
                R = dt.NewRow();
                R["sNoiDung_ThuTamUng"] = dtPhaiThu.Rows[i]["sNoiDung"].ToString();
                R["sLyDo_ThuTamUng"] = dtPhaiThu.Rows[i]["sLyDo"].ToString();
                R["rSoTien_ThuTamUng"] = dtPhaiThu.Rows[i]["rSoTien"].ToString();
                tong += Convert.ToDecimal(dtPhaiThu.Rows[i]["rSoTien"]);
                dt.Rows.Add(R);
            }

            R = dt.NewRow();
            R["sLyDo_ThuTamUng"] = "                             Cộng";
            R["rSoTien_ThuTamUng"] = tong.ToString();
            R["InDam"] = "1";
            dt.Rows.Add(R);

            //tamung
            R = dt.NewRow();
            R["sLyDo_ThuTamUng"] = "           2.Các khoản tạm ứng -TK 312";
            R["InDam"] = "1";
            dt.Rows.Add(R);
            tong = 0;
            for (int i = 0; i < dtTamTung.Rows.Count; i++)
            {
                R = dt.NewRow();
                R["sNoiDung_ThuTamUng"] = dtTamTung.Rows[i]["sNoiDung"].ToString();
                R["sLyDo_ThuTamUng"] = dtTamTung.Rows[i]["sLyDo"].ToString();
                R["rSoTien_ThuTamUng"] = dtTamTung.Rows[i]["rSoTien"].ToString();
                tong += Convert.ToDecimal(dtTamTung.Rows[i]["rSoTien"]);
                dt.Rows.Add(R);
            }
            R = dt.NewRow();
            R["sLyDo_ThuTamUng"] = "                             Cộng";
            R["rSoTien_ThuTamUng"] = tong.ToString();
            R["InDam"] = "1";
            dt.Rows.Add(R);

            int a = dt.Rows.Count;
            int b = dtPhaiTra.Rows.Count+1;
            if (a < b)
            {
                for (int i = 0; i < (b - a);i++)
                {
                    R = dt.NewRow();
                    dt.Rows.Add(R);
                }
            }
            tong = 0;
            for (int i = 0; i < dtPhaiTra.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                r["sNoiDung_PhaiTra"] = dtPhaiTra.Rows[i]["sNoiDung"].ToString();
                r["sLyDo_PhaiTra"] = dtPhaiTra.Rows[i]["sLyDo"].ToString();
                r["rSoTien_PhaiTra"] = dtPhaiTra.Rows[i]["rSoTien"].ToString();
                tong += Convert.ToDecimal(dtPhaiTra.Rows[i]["rSoTien"]);
            }
            //dong tong cong phai tra
            DataRow r1 = dt.Rows[dtPhaiTra.Rows.Count];
            r1["sLyDo_PhaiTra"] = "                             Cộng";
            r1["rSoTien_PhaiTra"] = tong.ToString();
            r1["InDam1"] = "1";
            a = dt.Rows.Count;
            int sodong;
            if (KhoGiay == "1")
                //sodong = 30;
                sodong = 14;
            else sodong = 13;
            if (a < sodong)
            {
                for (int i = 0; i < sodong - a; i++)
                {
                    R = dt.NewRow();
                    dt.Rows.Add(R);
                }
            }
            int iR = _data.dtGiaiThich.Rows.Count;
            if (iR < 10)
            {
                for (int i = 0; i < 10 - iR; i++)
                {
                    R = _data.dtGiaiThich.NewRow();
                    _data.dtGiaiThich.Rows.Add(R);
                }
            }

            fr.AddTable("ChiTiet", dt);
           
            fr.AddTable("GiaiThich", _data.dtGiaiThich);

        }

        public class data
        {
            public DataTable dtThu { get; set; }
            public DataTable dtTamUng { get; set; }
            public DataTable dtPhaiTra { get; set; }
            public DataTable dtGiaiThich { get; set; }
        }

        private static data Get_dataGiaiThich(String iThang, String iNam,String optThu,String optTamUng,String optTra)
        {
            data _data = new data();
            String SQL = "";
             //dt phai thu 
             //0: Nội dung, 1 : Đơn vị 2: Kết hợp
            if (optThu == "0")
            {
                SQL=@"SELECT sNoiDung='',sLyDo,SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =1) AND sLyDo<>'' AND sLyDo IS NOT NULL
                      GROUP BY sLyDo";
            }
            else if(optThu=="1")
            {
                 SQL=@"SELECT sNoiDung,sLyDo='',SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =1) AND sNoiDung<>'' AND sNoiDung IS NOT NULL
                      GROUP BY sNoiDung";
            }
            else
            {
                SQL=@"SELECT sNoiDung,sLyDo,SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =1) AND sNoiDung<>'' AND sNoiDung IS NOT NULL   AND sLyDo<>'' AND sLyDo IS NOT NULL
                     GROUP BY sLyDo,sNoiDung";
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThangLamViec",iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            _data.dtThu = Connection.GetDataTable(cmd);

            if (optTamUng == "0")
            {
                SQL=@"SELECT sNoiDung='',sLyDo,SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =2) AND sLyDo<>'' AND sLyDo IS NOT NULL
                      GROUP BY sLyDo";
            }
            else if(optTamUng=="1")
            {
                 SQL=@"SELECT sNoiDung,sLyDo='',SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =2) AND sNoiDung<>'' AND sNoiDung IS NOT NULL
                      GROUP BY sNoiDung";
            }
            else
            {
                SQL=@"SELECT sNoiDung,sLyDo,SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =2) AND sNoiDung<>'' AND sNoiDung IS NOT NULL   AND sLyDo<>'' AND sLyDo IS NOT NULL
                     GROUP BY sLyDo,sNoiDung";
            }
            cmd.CommandText = SQL;
            _data.dtTamUng = Connection.GetDataTable(cmd);

            //iloai=3
            if (optTra == "0")
            {
                SQL=@"SELECT sNoiDung='',sLyDo,SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =3) AND sLyDo<>'' AND sLyDo IS NOT NULL
                      GROUP BY sLyDo";
            }
            else if(optTra=="1")
            {
                 SQL=@"SELECT sNoiDung,sLyDo='',SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =3) AND sNoiDung<>'' AND sNoiDung IS NOT NULL
                      GROUP BY sNoiDung";
            }
            else
            {
                SQL=@"SELECT sNoiDung,sLyDo,SUM(rSoTien) as rSoTien
                      FROM KT_GiaiThichSoDu
                      WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND (iLoai =3) AND sNoiDung<>'' AND sNoiDung IS NOT NULL   AND sLyDo<>'' AND sLyDo IS NOT NULL
                     GROUP BY sLyDo,sNoiDung";
            }
            cmd.CommandText = SQL;
            _data.dtPhaiTra = Connection.GetDataTable(cmd);

            //iloai=4
            SQL = "SELECT * FROM KT_GiaiThichSoDu WHERE iTrangThai=1 AND iThangLamViec=@iThangLamViec AND iNamLamViec=@iNamLamViec AND iLoai =4 ";
            cmd.CommandText = SQL;
            _data.dtGiaiThich = Connection.GetDataTable(cmd);

            cmd.Dispose();
            
            return _data;
        }

    }
}
