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
using System.Collections;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_5bController : Controller
    {
        //
        // GET: /rptDuToanChiNganSachQuocPhong_1B/
        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath1 = "/Report_ExcelFrom/DuToan/rptDuToan_5B_1.xls";
        private const String sFilePath2 = "/Report_ExcelFrom/DuToan/rptDuToan_5B_2.xls";
        private static DataTable dtSoTo;
        public class dataDuLieu
        {
            public DataTable dtDuLieu { get; set; }
            public DataTable dtdtDuLieuAll { get; set; }
            public ArrayList arrMoTa1 { get; set; }
            public ArrayList arrMoTa2 { get; set; }
            public ArrayList arrMoTa3 { get; set; }
        }

        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_5B.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String Nganh = Request.Form[ParentID + "_Nganh"];
            String ToSo = Request.Form[ParentID + "_ToSo"];
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            return RedirectToAction("Index", new { NamLamViec = NamLamViec, Nganh = Nganh, ToSo = ToSo, sLNS = sLNS, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        private static dataDuLieu _data;
        public ExcelFile CreateReport(String path, String NamLamViec, String Nganh, String ToSo, String sLNS, String iID_MaTrangThaiDuyet)
        {
            String MaND = User.Identity.Name;

            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptDuToan_5B");

            DataTable data = _data.dtDuLieu;
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            ArrayList arrMoTa1 = _data.arrMoTa1;
            ArrayList arrMoTa2 = _data.arrMoTa2;
            ArrayList arrMoTa3 = _data.arrMoTa3;
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("ToSo", ToSo);
            int i = 1;
            foreach (object obj in arrMoTa1)
            {
                fr.SetValue("MoTa1_" + i, obj);
                i++;
            }
            i = 1;
            foreach (object obj in arrMoTa2)
            {
                fr.SetValue("MoTa2_" + i, obj);
                i++;
            }
            i = 1;
            foreach (object obj in arrMoTa3)
            {
                fr.SetValue("MoTa3_" + i, obj);
                i++;
            }
            String TenNganh = "";
            DataTable dtNganh = Connection.GetDataTable("SELECT iID_MaDanhMuc,sTenKhoa, DC_DanhMuc.sTen FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'Nganh' ORDER BY iSTT");
            for (i = 0; i < dtNganh.Rows.Count; i++)
            {
                if (Nganh == dtNganh.Rows[i]["sTenKhoa"].ToString())
                {
                    TenNganh = dtNganh.Rows[i]["sTen"].ToString();
                    break;
                }
            }
            String TenLNS = "";
            TenLNS = MoTaLNS_Cha(sLNS);
            dtNganh.Dispose();
            fr.SetValue("Nganh", TenNganh);
            fr.SetValue("TenLNS", TenLNS);
            fr.SetValue("LNS", sLNS);
            fr.Run(Result);
            return Result;

        }
        public static String MoTaLNS_Cha(String sLNS)
        {
            String SQL = String.Format(@"SELECT sLNS,sMoTa
                                        FROM NS_MucLucNganSach
                                        WHERE sLNS=SUBSTRING('{0}',1,3)", sLNS);
            DataTable dt = Connection.GetDataTable(SQL);
            String MoTa = "";
            if (dt.Rows.Count > 0)
                MoTa = dt.Rows[0]["sMota"].ToString();
            return MoTa;
        }

        public ActionResult ViewPDF(String NamLamViec, String Nganh, String ToSo, String sLNS, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = "";
            if (ToSo == "1")
                DuongDan = sFilePath1;
            else
                DuongDan = sFilePath2;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Nganh, ToSo, sLNS, iID_MaTrangThaiDuyet);
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

        public clsExcelResult ExportToExcel(String NamLamViec, String Nganh, String ToSo, String sLNS, String iID_MaTrangThaiDuyet)
        {
            HamChung.Language();
            String DuongDan = "";
            if (ToSo == "1")
                DuongDan = sFilePath1;
            else
                DuongDan = sFilePath2;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), NamLamViec, Nganh, ToSo, sLNS, iID_MaTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "DuToan_5b.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        [HttpGet]
        public JsonResult Get_dsDonVi(string ParentID, String NamLamViec, String Nganh, String ToSo, String sLNS, String MaND, String iID_MaTrangThaiDuyet)
        {

            return Json(obj_DSDonVi(ParentID, NamLamViec, Nganh, ToSo, sLNS, MaND, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }
        public String obj_DSDonVi(String ParentID, String NamLamViec, String Nganh, String ToSo, String sLNS, String MaND, String iID_MaTrangThaiDuyet)
        {
            String dsDonVi = "";
            DataTable dtTo = DanhSachToIn(NamLamViec, Nganh, ToSo, sLNS, MaND, iID_MaTrangThaiDuyet);
            SelectOptionList slTo = new SelectOptionList(dtTo, "MaTo", "TenTo");
            dsDonVi = MyHtmlHelper.DropDownList(ParentID, slTo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100%\"");
            return dsDonVi;
        }



        public static dataDuLieu get_dtDuToan_PhuLuc1B(String NamLamViec, String Nganh, String ToSo, String sLNS, String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dtNganh = MucLucNganSach_NganhModels.Get_dtMucLucNganSach_Nganh(Nganh);
            int cs = 0, i = 0;
            String DSNganh = "";
            for (i = 0; i < dtNganh.Rows.Count; i++)
            {
                DSNganh = DSNganh + "'" + Convert.ToString(dtNganh.Rows[i]["iID_MaNganhMLNS"]) + "',";
            }
            if (DSNganh != "") DSNganh = " AND sNG IN (" + DSNganh.Remove(DSNganh.Length - 1) + ")";
            else DSNganh = " AND sNG IN(123)";

            String SQLNganh = "SELECT distinct sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQLNganh += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQLNganh += " FROM DT_ChungTuChiTiet WHERE sLNS=@sLNS  AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet  {0}{1}";
            SQLNganh = String.Format(SQLNganh, DSNganh, ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmdNG = new SqlCommand(SQLNganh);
            cmdNG.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmdNG.Parameters.AddWithValue("@sLNS", sLNS);
            cmdNG.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dtNG = Connection.GetDataTable(cmdNG);
            cmdNG.Dispose();
            String SQL;
            DataTable dtDonVi;
            //SQL= "SELECT DISTINCT iID_MaDonVi";
            //SQL += " FROM DT_ChungTuChiTiet WHERE sNG IN ({0})";
            //SQL += " GROUP BY iID_MaDonVi";
            //SQL += " HAVING SUM(rTuChi)>0 OR SUM(rHienVat)>0";
            //SQL = String.Format(SQL, DSNganh);
            //dtDonVi = Connection.GetDataTable(SQL);

            // String SQL1 = "SELECT iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@iID_MaNganh";

            SQL = "SELECT iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
            SQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            SQL += ",SUM(rPhanCap) AS rPhanCap";
            SQL += ",SUM(rHienVat) AS rHienVat";
            SQL += " FROM DT_ChungTuChiTiet WHERE sLNS=@sLNS  AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet   {0} {1}";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,iID_MaDonVi,sMoTa";
            SQL += " HAVING SUM(rPhanCap)>0 OR SUM(rHienVat)>0";
            SQL = String.Format(SQL, DSNganh, ReportModels.DieuKien_NganSach(MaND));

            String strSQL = "SELECT CT.iID_MaDonVi,CT.iID_MaDonVi +' - '+ NS_DonVi.sTen AS TenDonVi";
            strSQL += ",sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,CT.sMoTa";
            strSQL += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
            strSQL += ",rPhanCap,rHienVat";
            strSQL += " FROM ({0}) CT ";
            strSQL += " INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=CT.iID_MaDonVi";
            strSQL = String.Format(strSQL, SQL);
            SqlCommand cmd = new SqlCommand(strSQL);
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            dtDonVi = HamChung.SelectDistinct("dtDonVi", dt, "iID_MaDonVi", "iID_MaDonVi,TenDonVi");

            i = 0;
            //cs = 3;//tờ 1 4 cột
            dtDonVi.Columns.Add("TongPhanCap", typeof(Decimal));
            dtDonVi.Columns.Add("TongHienVat", typeof(Decimal));
            while (i < dtNG.Rows.Count)
            {
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_PhanCap") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_PhanCap", typeof(Decimal));
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_HienVat") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_HienVat", typeof(Decimal));
                i = i + 1;
            }

            i = 0;
            cs = 0;
            String MaDonVi, MaDonVi1, TenCot;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]).Trim();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    MaDonVi1 = Convert.ToString(dt.Rows[j]["iID_MaDonVi"]).Trim();
                    TenCot = Convert.ToString(dt.Rows[j]["NG"]).Trim();
                    if (MaDonVi == MaDonVi1 && dtDonVi.Columns.IndexOf(TenCot + "_PhanCap") >= 0)
                    {
                        dtDonVi.Rows[i][TenCot + "_PhanCap"] = dt.Rows[j]["rPhanCap"];
                        dtDonVi.Rows[i][TenCot + "_HienVat"] = dt.Rows[j]["rHienVat"];
                        dt.Rows.RemoveAt(j);
                        j = j - 1;
                    }
                }
            }
            i = 0;
            //j=4 vì trừ cột madv, đơn vị và 2 cột tổng cộng
            Double Tong = 0;
            for (int j = 4; j < dtDonVi.Columns.Count; j++)
            {
                Tong = 0;
                for (i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        Tong = Tong + Convert.ToDouble(dtDonVi.Rows[i][j]);
                    }
                }
                if (Tong == 0)
                {
                    dtDonVi.Columns.RemoveAt(j);
                    if (j == 1) j = 1;
                    else j = j - 1;
                }
            }
            Double TongHienVat = 0, TongPhanCap = 0;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                TongHienVat = 0; TongPhanCap = 0;
                //j=4 vì trừ cột MaDV, đơn vị và 2 cột tổng cộng
                for (int j = 4; j < dtDonVi.Columns.Count; j++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        if (dtDonVi.Columns[j].ColumnName.IndexOf("_HienVat") >= 0)
                        {
                            TongHienVat = TongHienVat + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                        else
                        {
                            TongPhanCap = TongPhanCap + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                    }
                }
                dtDonVi.Rows[i]["iID_MaDonVi"] = (i + 1).ToString();
                dtDonVi.Rows[i]["TongHienVat"] = TongHienVat;
                dtDonVi.Rows[i]["TongPhanCap"] = TongPhanCap;
            }
            DataTable _dtDonVi = new DataTable();
            DataTable _dtDonVi1 = new DataTable();

            int TongSoCot = 0;
            int SoTrang = 1;
            int SoCotCanThem = 0;
            
            if ((dtDonVi.Columns.Count - 4) ==0)
            {
                SoCotCanThem = 4;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else if ((dtDonVi.Columns.Count - 4) <= 4)
            {

                int SoCotDu = ((dtDonVi.Columns.Count - 4)) % 4;
                if(SoCotDu!=0)
                    SoCotCanThem = 4 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (dtDonVi.Columns.Count - 4 - 4) % 6;
                if (SoCotDu != 0)
                    SoCotCanThem = 6 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
                SoTrang = 1 + (TongSoCot - 4) / 6;
            }

            for (i = 0; i < SoCotCanThem; i++)
            {
                dtDonVi.Columns.Add();
            }
            int _ToSo = Convert.ToInt16(ToSo);
            int SoCotTrang1 = 4;
            int SoCotTrangLonHon1 = 6;
            _dtDonVi = dtDonVi.Copy();
            int _CS = 0;
            String BangTien_HienVat = "";
            //Mổ tả xâu nối mã
            ArrayList arrMoTa1 = new ArrayList();
            //Mỏ tả ngành
            ArrayList arrMoTa2 = new ArrayList();
            //Bằng Tiền hay bằng hiện vật
            ArrayList arrMoTa3 = new ArrayList();
            if (ToSo == "1")
            {

                for (i = 4; i < 4 + SoCotTrang1; i++)
                {
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_PhanCap") >= 0) BangTien_HienVat = "Bằng phân cấp";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + (i - 3);
                }
            }
            else
            {
                int tg = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2);
                int dem = 1;
                for (i = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2); i < 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 1); i++)
                {
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_PhanCap") >= 0) BangTien_HienVat = "Bằng phân cấp";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + dem;
                    dem++;
                }
            }

            dataDuLieu _data = new dataDuLieu();
            _data.dtDuLieu = _dtDonVi;
            _data.arrMoTa1 = arrMoTa1;
            _data.arrMoTa2 = arrMoTa2;
            _data.arrMoTa3 = arrMoTa3;
            _data.dtdtDuLieuAll = dtDonVi;
            return _data;
        }
        public static DataTable DanhSachToIn(String NamLamViec, String Nganh, String ToSo, String sLNS,String MaND,String iID_MaTrangThaiDuyet)
        {
            _data = get_dtDuToan_PhuLuc1B(NamLamViec, Nganh, ToSo, sLNS, MaND, iID_MaTrangThaiDuyet);
            DataTable dtToIn = new DataTable();
            dtToIn.Columns.Add("MaTo", typeof(String));
            dtToIn.Columns.Add("TenTo", typeof(String));
            DataRow R = dtToIn.NewRow();
            dtToIn.Rows.Add(R);
            R[0] = "1";
            R[1] = "Tờ 1";
            if (_data.dtdtDuLieuAll != null)
            {
                int a = 2;
                for (int i = 0; i < _data.dtdtDuLieuAll.Columns.Count - 8; i = i + 6)
                {
                    DataRow R1 = dtToIn.NewRow();
                    dtToIn.Rows.Add(R1);
                    R1[0] = a;
                    R1[1] = "Tờ " + a;
                    a++;
                }
            }
            return dtToIn;
        }
    }
}
