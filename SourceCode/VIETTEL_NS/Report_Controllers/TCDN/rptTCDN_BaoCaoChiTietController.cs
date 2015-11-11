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
using System.IO;

namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptTCDN_BaoCaoChiTietController : Controller
    {
        //
        // GET: /rptThuNop_4CC/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_HoSoDoanhNghiep = "/Report_ExcelFrom/TCDN/rptTCDN_HoSoDoanhNghiep.xls";
        private const String sFilePath = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoChiTiet.xls";
        private const String sFilePath_Loai4 = "/Report_ExcelFrom/TCDN/rptTCDN_BaoCaoChiTiet_Loai4.xls";
        public ActionResult Index()
        {
            FlexCelReport fr = new FlexCelReport();
            ViewData["path"] = "~/Report_Views/TCDN/rptTCDN_BaoCaoChiTiet.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNamNganSach">2:Nam nay 1.Nam Truoc</param>
        /// <returns></returns>
        public static DataTable rptTCDN_BaoCaoChiTiet(String MaND, String iQuy, String bTrongKy, String iID_MaDoanhNghiep, String iLoai, String DVT)
        {

            SqlCommand cmd = new SqlCommand();
            String SQL = "", DK = "", DKQUy = "";
            DataTable vR = new DataTable();
            //bieu ho so doanh nghiep
            if (iLoai == "0")
            {
                SQL =
                    String.Format(
                        "SELECT * FROM TCDN_DoanhNghiep WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                vR = Connection.GetDataTable(cmd);
            }
            else
            {
                DK = "iTrangThai=1  AND SUBSTRING(sKyHieu,1,1)=@iLoai";
                cmd.Parameters.AddWithValue("@iLoai", iLoai);
                SQL = String.Format("SELECT * FROM TCDN_ChiTieu WHERE {0} ORDER BY sKyHieu", DK);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd);

                //lấy dt chi tiết

                if (bTrongKy != "on")
                {
                    DKQUy = " AND iQuy<=@iQuy";
                }
                else
                {
                    DKQUy = " AND iQuy=@iQuy";
                }
                cmd = new SqlCommand();
                DK = "iTrangThai=1  AND  SUBSTRING(sKyHieu,1,1)=@iLoai AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep " +
                     DKQUy;
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                cmd.Parameters.AddWithValue("@iLoai", iLoai);
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                SQL = String.Format(@"
SELECT sKyHieu,SUM(rKeHoach/{1}) as rKeHoach,SUM(rThucHien/{1}) as  rThucHien 
FROM TCDN_ChungTuChiTiet
WHERE {0}  AND iNamLamViec=@iNamLamViec GROUP BY sKyHieu  ORDER BY sKyHieu
", DK, DVT);
                cmd.CommandText = SQL;
                DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);

                //Lấy dtNamTruoc
                cmd = new SqlCommand();
                DK = "iTrangThai=1  AND  SUBSTRING(sKyHieu,1,1)=@iLoai AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
                cmd.Parameters.AddWithValue("@iLoai", iLoai);
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(ReportModels.LayNamLamViec(MaND)) - 1);
                SQL = String.Format(@"
SELECT sKyHieu,SUM(rThucHien/{1}) as rNamTruoc 
FROM TCDN_ChungTuChiTiet
WHERE {0}   AND iNamLamViec=@iNamLamViec  GROUP BY sKyHieu ORDER BY sKyHieu
", DK, DVT);
                cmd.CommandText = SQL;
                DataTable dtNamTruoc = Connection.GetDataTable(cmd);


                //Ghep dtNamTruoc vao dt Nam Nay và tinh ty le



                String sDSTruong = "sKyHieu,rKeHoach,rThucHien,rNamTruoc,rNTNN,rTHKH";
                String[] arrDSTruong = sDSTruong.Split(',');
                for (int i = 1; i < arrDSTruong.Length; i++)
                {
                    DataColumn dtc = new DataColumn();
                    dtc.DefaultValue = 0;
                    dtc.DataType = typeof(decimal);
                    dtc.ColumnName = arrDSTruong[i];
                    vR.Columns.Add(dtc);
                }
                int vRCount = vR.Rows.Count;
                int cs0 = 0;
                for (int i = 0; i < vRCount; i++)
                {
                    //Ghép dt chi tiết vào dt danh muc
                    for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(vR.Rows[i]["sKyHieu"]) !=
                            Convert.ToString(dtChungTuChiTiet.Rows[j]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {
                            for (int k = 1; k < arrDSTruong.Length - 3; k++)
                            {
                                vR.Rows[i][arrDSTruong[k]] = dtChungTuChiTiet.Rows[j][arrDSTruong[k]];
                            }
                        }
                    }
                    //Ghép dtnamtruoc vào dt danh muc
                    for (int c = 0; c < dtNamTruoc.Rows.Count; c++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(vR.Rows[i]["sKyHieu"]) != Convert.ToString(dtNamTruoc.Rows[c]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {
                            vR.Rows[i]["rNamTruoc"] = dtNamTruoc.Rows[c]["rNamTruoc"];
                        }
                    }
                    //Tinh cot ty le
                    if (Convert.ToDecimal(vR.Rows[i]["rNamTruoc"]) != 0)
                        vR.Rows[i]["rNTNN"] = Math.Round(Convert.ToDecimal(vR.Rows[i]["rThucHien"]) /
                                                         Convert.ToDecimal(vR.Rows[i]["rNamTruoc"]) * 100, 2);
                    if (Convert.ToDecimal(vR.Rows[i]["rKeHoach"]) != 0)
                        vR.Rows[i]["rTHKH"] = Math.Round(Convert.ToDecimal(vR.Rows[i]["rThucHien"]) /
                                                         Convert.ToDecimal(vR.Rows[i]["rKeHoach"]) * 100, 2);
                }

                cmd.Dispose();
                // ghép phần dự án đang đâu tư nếu bảng là bảng 3
                if (iLoai == "3")
                {
                    SQL =
                        String.Format(
                            "SELECT * FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    DataTable dtDuAnDauTu = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    //Lay cac du an co du lieu nam nay
                    if (bTrongKy != "on")
                    {
                        DKQUy = " AND iQuy<=@iQuy";
                    }
                    else
                    {
                        DKQUy = " AND iQuy=@iQuy";
                    }
                    SQL =
                        String.Format(
                            @"SELECT  sKyHieu,SUM(rThucHien/{1}) as  rThucHien, SUM(rKeHoach/{1}) as rKeHoach
                         FROM TCDN_ChungTuChiTiet
                        WHERE iTrangThai=1 {0}  AND iNamLamViec=@iNamLamViec AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep)
                        GROUP BY sKyHieu ORDER By sKyHieu", DKQUy, DVT);
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
                    cmd.Parameters.AddWithValue("@iQuy", iQuy);
                    DataTable dtDuAn_NamNay = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    //Lay cac du an co du lieu nam truoc

                    SQL =
                        String.Format(@"SELECT sKyHieu,SUM(rThucHien/{0}) as rNamTruoc  FROM TCDN_ChungTuChiTiet WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep) 
                                         GROUP BY sKyHieu", DVT);
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(ReportModels.LayNamLamViec(MaND)) - 1);
                    DataTable dtDuAn_NamTruoc = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    for (int i = 1; i < arrDSTruong.Length; i++)
                    {
                        DataColumn dtc = new DataColumn();
                        dtc.DefaultValue = 0;
                        dtc.DataType = typeof(decimal);
                        dtc.ColumnName = arrDSTruong[i];
                        dtDuAnDauTu.Columns.Add(dtc);
                    }
                    for (int i = 0; i < dtDuAnDauTu.Rows.Count; i++)
                    {
                        //Ghép dtDuAn_NamNay vào dtDuAnDauTu
                        for (int j = cs0; j < dtDuAn_NamNay.Rows.Count; j++)
                        {
                            Boolean ok = true;
                            if (Convert.ToString(dtDuAnDauTu.Rows[i]["sKyHieu"]) !=
                                Convert.ToString(dtDuAn_NamNay.Rows[j]["sKyHieu"]))
                                ok = false;
                            if (ok)
                            {
                                for (int k = 1; k < arrDSTruong.Length - 3; k++)
                                {
                                    dtDuAnDauTu.Rows[i][arrDSTruong[k]] = dtDuAn_NamNay.Rows[j][arrDSTruong[k]];
                                }
                            }
                        }
                        //Ghép dtnamtruoc vào dt danh muc
                        for (int c = 0; c < dtDuAn_NamTruoc.Rows.Count; c++)
                        {
                            Boolean ok = true;
                            if (Convert.ToString(dtDuAnDauTu.Rows[i]["sKyHieu"]) !=
                                Convert.ToString(dtDuAn_NamTruoc.Rows[c]["sKyHieu"]))
                                ok = false;
                            if (ok)
                            {
                                dtDuAnDauTu.Rows[i]["rNamTruoc"] = dtDuAn_NamTruoc.Rows[c]["rNamTruoc"];
                            }
                        }
                        //Tinh cot ty le
                        if (Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rNamTruoc"]) != 0)
                            dtDuAnDauTu.Rows[i]["rNTNN"] =
                                Math.Round(Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rThucHien"]) /
                                           Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rNamTruoc"]) * 100, 2);
                        if (Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rKeHoach"]) != 0)
                            dtDuAnDauTu.Rows[i]["rTHKH"] =
                                Math.Round(Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rThucHien"]) /
                                           Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rKeHoach"]) * 100, 2);
                    }
                    //ghep cac du an dang dau tu vao VR
                    for (int i = 0; i < dtDuAnDauTu.Rows.Count; i++)
                    {
                        DataRow dr = vR.NewRow();

                        dr["sKyHieu"] = dtDuAnDauTu.Rows[i]["sKyHieu"];
                        dr["sTen"] = dtDuAnDauTu.Rows[i]["sTenDuAn"];
                        dr["iID_MaChiTieu_Cha"] = dtDuAnDauTu.Rows[i]["iID_MaChiTieu_Cha"];
                        dr["bLahangCha"] = "False";
                        dr["bLaTong"] = "False";
                        dr["bLaText"] = "False";
                        for (int j = 1; j < arrDSTruong.Length; j++)
                        {
                            dr[arrDSTruong[j]] = dtDuAnDauTu.Rows[i][arrDSTruong[j]];
                        }
                        vR.Rows.Add(dr);
                    }
                    //sap xep lai dr

                    DataView dv = vR.DefaultView;
                    dv.Sort = "sKyHieu";
                    vR = dv.ToTable();
                }

                if (iLoai == "4")
                {
                    DataColumn dtc1 = new DataColumn();
                    dtc1.DefaultValue = "";
                    dtc1.DataType = typeof(String);
                    dtc1.ColumnName = "sNamTruoc_4";
                    vR.Columns.Add(dtc1);

                    dtc1 = new DataColumn();
                    dtc1.DefaultValue = "";
                    dtc1.DataType = typeof(String);
                    dtc1.ColumnName = "sThucHien_4";
                    vR.Columns.Add(dtc1);
                    String sCongThuc = "";
                    String[] arrCongThuc = new String[2];

                    for (int i = 0; i < vR.Rows.Count; i++)
                    {
                        sCongThuc = "";
                        List<string> arrCongThucSo = new List<String>();
                        List<string> arrCongThucDau = new List<String>();
                        List<decimal> arrGiaTriNamTruoc = new List<decimal>();
                        List<decimal> arrGiaTriNamNay = new List<decimal>();
                        if (!String.IsNullOrEmpty(Convert.ToString(vR.Rows[i]["sCongThuc"])))
                        {
                            sCongThuc = Convert.ToString(vR.Rows[i]["sCongThuc"]);
                        }
                        if (!string.IsNullOrEmpty(sCongThuc))
                        {
                            arrCongThuc = sCongThuc.Split(' ');
                            for (int z = 0; z < arrCongThuc.Length; z++)
                            {
                                if (arrCongThuc[z] == "+" || arrCongThuc[z] == "-" || arrCongThuc[z] == "*" || arrCongThuc[z] == "/")
                                {
                                    arrCongThucDau.Add(arrCongThuc[z]);
                                }
                                else
                                {
                                    arrCongThucSo.Add(arrCongThuc[z]);
                                }
                            }
                            //namnay
                            SQL = String.Format(@"SELECT SUM(rThucHien) as rThucHien 
FROM TCDN_ChungTuChiTiet
 WHERE iTrangThai=1  AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy<=iQuy
AND (sKyHieu IN (
SELECT sKyHieu
 FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND
   iID_MaChiTieu_Cha=(SELECT iID_MaChiTieu FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND sKyHieu=@sKyHieu AND bLaTong=1)
   ) OR sKyHieu =@sKyHieu) ");
                            //duyet lan luot cac so
                            for (int j = 0; j < arrCongThucSo.Count; j++)
                            {
                                cmd = new SqlCommand(SQL);
                                cmd.Parameters.AddWithValue("@sKyHieu", arrCongThucSo[j]);
                                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                                cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(ReportModels.LayNamLamViec(MaND)));
                                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                                decimal dtNamNaySo1 = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                                arrGiaTriNamNay.Add(dtNamNaySo1);
                            }
                            //nam truoc

                            SQL = String.Format(@"SELECT SUM(rThucHien) as rThucHien 
FROM TCDN_ChungTuChiTiet
 WHERE iTrangThai=1  AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec 
AND (sKyHieu IN (
SELECT sKyHieu
 FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND
   iID_MaChiTieu_Cha=(SELECT iID_MaChiTieu FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND sKyHieu=@sKyHieu AND bLaTong=1)
   ) OR sKyHieu =@sKyHieu) ");
                            //duyet lan luot cac so
                            for (int j = 0; j < arrCongThucSo.Count; j++)
                            {
                                cmd = new SqlCommand(SQL);
                                cmd.Parameters.AddWithValue("@sKyHieu", arrCongThucSo[j]);
                                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                                cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(ReportModels.LayNamLamViec(MaND)) - 1);
                                decimal dtNamTruocSo1 = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                                arrGiaTriNamTruoc.Add(dtNamTruocSo1);
                            }
                            decimal rNamNay = 0, rNamTruoc = 0;
                            rNamNay = arrGiaTriNamNay[0];
                            rNamTruoc = arrGiaTriNamTruoc[0];
                            for (int c = 1; c < arrCongThucSo.Count; c++)
                            {
                                if (arrGiaTriNamNay[c] != 0)
                                {
                                    if (arrCongThucDau[c - 1] == "+")
                                    {
                                        rNamNay += arrGiaTriNamNay[c];
                                    }
                                    else if (arrCongThucDau[c - 1] == "-")
                                    {
                                        rNamNay -= arrGiaTriNamNay[c];
                                    }
                                    else if (arrCongThucDau[c - 1] == "*")
                                    {
                                        rNamNay *= arrGiaTriNamNay[c];
                                    }
                                    else
                                    {
                                        rNamNay /= arrGiaTriNamNay[c];
                                    }
                                }
                                if (arrGiaTriNamTruoc[c] != 0)
                                {
                                    if (arrCongThucDau[c - 1] == "+")
                                    {
                                        rNamTruoc += arrGiaTriNamTruoc[c];
                                    }
                                    else if (arrCongThucDau[c - 1] == "-")
                                    {
                                        rNamTruoc -= arrGiaTriNamTruoc[c];
                                    }
                                    else if (arrCongThucDau[c - 1] == "*")
                                    {
                                        rNamTruoc *= arrGiaTriNamTruoc[c];
                                    }
                                    else
                                    {
                                        rNamTruoc /= arrGiaTriNamTruoc[c];
                                    }
                                }

                            }
                            if (rNamNay == 0)
                                vR.Rows[i]["sThucHien_4"] = "";
                            else
                                vR.Rows[i]["sThucHien_4"] = Math.Round(rNamNay * 100, 2);
                            if (rNamTruoc == 0)
                                vR.Rows[i]["sNamTruoc_4"] = "";
                            else
                                vR.Rows[i]["sNamTruoc_4"] = Math.Round(rNamTruoc * 100, 2);
                            //if(dtNamNaySo2!=0)
                            //{
                            //    vR.Rows[i]["rThucHien"] = Math.Round(dtNamNaySo1/dtNamNaySo2*100,0);
                            //}
                            //if (dtNamTruocSo2 != 0)
                            //{
                            //    vR.Rows[i]["rNamTruoc"] = Math.Round(dtNamTruocSo1 / dtNamTruocSo2*100,0);
                            //}

                        }
                    }// end for

                    //ghep phan cac cty thanh vien
                    SQL =
                       String.Format(
                           "SELECT * FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND (iID_MaDoanhNghiep=@iID_MaDoanhNghiep OR sKyHieu IN (414,4141,4142)) ORDER BY iLoai,sKyHieu");
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    DataTable dtDonViThanhVien = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    //Lay cac du an co du lieu nam nay

                    SQL =
                       String.Format(
                           @"SELECT  sKyHieu,iID_MaChungTuChiTiet,sThucHien_4 
                         FROM TCDN_ChungTuChiTiet
                        WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  AND iQuy=@iQuy AND (sKyHieu IN (SELECT sKyHieu FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep) OR sKyHieu IN (4141,4142)) ");
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    cmd.Parameters.AddWithValue("@iQuy", iQuy);
                    cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(ReportModels.LayNamLamViec(MaND)));
                    DataTable dtDonViThanhVien_NamNay = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    //Lay cac du an co du lieu nam truoc

                    SQL =
                       String.Format(
                           "SELECT sKyHieu,sThucHien_4 as sNamTruoc_4  FROM TCDN_ChungTuChiTiet WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  AND iQuy=4 AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND (iID_MaDoanhNghiep=@iID_MaDoanhNghiep) OR sKyHieu IN (4141,4142) )");

                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(ReportModels.LayNamLamViec(MaND)) - 1);
                    DataTable dtDonViThanhVien_NamTruoc = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    //for (int i = 2; i < arrDSTruong.Length; i++)
                    //{
                    //    DataColumn dtc = new DataColumn();
                    //    dtc.DefaultValue = 0;
                    //    dtc.DataType = typeof(decimal);
                    //    dtc.ColumnName = arrDSTruong[i];
                    //    dtDonViThanhVien.Columns.Add(dtc);
                    //}
                    dtDonViThanhVien.Columns.Add("iID_MaChungTuChiTiet", typeof(String));
                    dtDonViThanhVien.Columns.Add("sThucHien_4", typeof(String));
                    dtDonViThanhVien.Columns.Add("sNamTruoc_4", typeof(String));
                    for (int i = 0; i < dtDonViThanhVien.Rows.Count; i++)
                    {
                        //Ghép dt_NamNay vào dtdonvithanhvien
                        for (int j = cs0; j < dtDonViThanhVien_NamNay.Rows.Count; j++)
                        {
                            Boolean ok = true;
                            if (Convert.ToString(dtDonViThanhVien.Rows[i]["sKyHieu"]) != Convert.ToString(dtDonViThanhVien_NamNay.Rows[j]["sKyHieu"]))
                                ok = false;
                            if (ok)
                            {

                                dtDonViThanhVien.Rows[i]["sThucHien_4"] = dtDonViThanhVien_NamNay.Rows[j]["sThucHien_4"];
                                dtDonViThanhVien.Rows[i]["iID_MaChungTuChiTiet"] = dtDonViThanhVien_NamNay.Rows[j]["iID_MaChungTuChiTiet"];
                                dtDonViThanhVien.Rows[i]["sKyHieu"] = dtDonViThanhVien_NamNay.Rows[j]["sKyHieu"];
                                break;
                            }
                        }
                        //Ghép dtDonVi_namTruoc vào dt danh muc
                        for (int c = 0; c < dtDonViThanhVien_NamTruoc.Rows.Count; c++)
                        {
                            Boolean ok = true;
                            if (Convert.ToString(dtDonViThanhVien.Rows[i]["sKyHieu"]) != Convert.ToString(dtDonViThanhVien_NamTruoc.Rows[c]["sKyHieu"]))
                                ok = false;
                            if (ok)
                            {
                                dtDonViThanhVien.Rows[i]["sNamTruoc_4"] = dtDonViThanhVien_NamTruoc.Rows[c]["sNamTruoc_4"];
                                break;
                            }
                        }
                    }
                    //ghep cac don vi thanh vien vao VR
                    for (int i = 0; i < dtDonViThanhVien.Rows.Count; i++)
                    {
                        DataRow dr = vR.NewRow();
                        //3 dong dau lay gia tri ghep vao vr
                        if (i == 0 || i == 1)
                        {
                            for (int j = vR.Rows.Count - 1; j >= 0; j--)
                            {

                                if (Convert.ToString(vR.Rows[j]["sKyHieu"]) == Convert.ToString(dtDonViThanhVien.Rows[i]["sKyHieu"]))
                                {
                                    vR.Rows[j]["sThucHien_4"] = dtDonViThanhVien.Rows[i]["sThucHien_4"];
                                    vR.Rows[j]["sNamTruoc_4"] = dtDonViThanhVien.Rows[i]["sNamTruoc_4"];
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dr["sKyHieu"] = dtDonViThanhVien.Rows[i]["sKyHieu"];
                            dr["sTen"] = dtDonViThanhVien.Rows[i]["sTen"];
                            dr["iID_MaChiTieu_Cha"] = dtDonViThanhVien.Rows[i]["iID_MaChiTieu_Cha"];
                            dr["bLahangCha"] = "False";
                            dr["bLaTong"] = "False";
                            dr["bLaText"] = "False";
                            //for (int j = 1; j < arrDSTruong.Length; j++)
                            //{
                            //    dr[arrDSTruong[j]] = dtDonViThanhVien.Rows[i][arrDSTruong[j]];
                            //}
                            dr["sThucHien_4"] = dtDonViThanhVien.Rows[i]["sThucHien_4"];
                            dr["sNamTruoc_4"] = dtDonViThanhVien.Rows[i]["sNamTruoc_4"];
                            vR.Rows.Add(dr);
                        }
                        //sap xep lai dr

                        //DataView dv = vR.DefaultView;
                        //dv.Sort = "sKyHieu";
                        //vR = dv.ToTable();
                    }


                    //tinh o tong cong
                    String strDSTruongTien = "rNamTruoc,rKeHoach,rThucHien";
                    String[] arrDSTruongTien = strDSTruongTien.Split(',');
                    int len = arrDSTruongTien.Length;
                    for (int i = vR.Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(vR.Rows[i]["bLaTong"]))
                        {
                            //TInh tong cac truong tien
                            String iiD_MaChiTieu = Convert.ToString(vR.Rows[i]["iiD_MaChiTieu"]);
                            for (int k = 0; k < len; k++)
                            {
                                double S;
                                S = 0;
                                for (int j = i + 1; j < vR.Rows.Count; j++)
                                {
                                    if (iiD_MaChiTieu == Convert.ToString(vR.Rows[j]["iiD_MaChiTieu_Cha"]))
                                    {
                                        if (!String.IsNullOrEmpty(Convert.ToString(vR.Rows[j][arrDSTruongTien[k]])))
                                        {
                                            S += Convert.ToDouble(vR.Rows[j][arrDSTruongTien[k]]);
                                        }
                                    }
                                }
                                vR.Rows[i][arrDSTruongTien[k]] = S;
                            }
                            //Tinh 2 cot ty le
                            if (Convert.ToDecimal(vR.Rows[i]["rNamTruoc"]) != 0)
                                vR.Rows[i]["rNTNN"] = (Math.Round(Convert.ToDecimal(vR.Rows[i]["rThucHien"]) /
                                                                  Convert.ToDecimal(vR.Rows[i]["rNamTruoc"]) * 100, 2));
                            if (Convert.ToDecimal(vR.Rows[i]["rKeHoach"]) != 0)
                                vR.Rows[i]["rTHKH"] = (Math.Round(Convert.ToDecimal(vR.Rows[i]["rThucHien"]) /
                                                                  Convert.ToDecimal(vR.Rows[i]["rKeHoach"]) * 100, 2));
                        }
                    }
                }
            }
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String iID_MaDoanhNghiep = Request.Form["iID_MaDonVi"];
            String DVT = Request.Form[ParentID + "_DVT"];
            String bTrongKy = Request.Form[ParentID + "_bTrongKy"];
            String iLoai = Request.Form[ParentID + "_iLoai"];
            ViewData["PageLoad"] = "1";
            ViewData["iQuy"] = iQuy;
            ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
            ViewData["DVT"] = DVT;
            ViewData["bTrongKy"] = bTrongKy;
            ViewData["iLoai"] = iLoai;
            ViewData["path"] = "~/Report_Views//TCDN/rptTCDN_BaoCaoChiTiet.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iQuy, String bTrongKy, String iID_MaDoanhNghiep, String iLoai, String DVT)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptTCDN_BaoCaoChiTiet");
            LoadData(fr, MaND, iQuy, bTrongKy, iID_MaDoanhNghiep, iLoai, DVT);
            String Nam = ReportModels.LayNamLamViec(MaND);
            String TenBieu1 = "", TenBieu2 = "";
            if (iLoai == "1")
            {
                TenBieu1 = "Biểu số 1";
                TenBieu2 = "CHỈ TIÊU HOẠT ĐỘNG SẢN XUẤT KINH DOANH";
            }
            else if (iLoai == "2")
            {
                TenBieu1 = "Biểu số 2";
                TenBieu2 = "CHỈ TIÊU THU CHI NGÂN SÁCH VÀ THU NHẬP";
            }
            else if (iLoai == "3")
            {
                TenBieu1 = "Biểu số 3";
                TenBieu2 = "CHỈ TIÊU TÀI CHÍNH";
            }
            else if (iLoai == "4")
            {
                TenBieu1 = "Biểu số 4";
                TenBieu2 = "CHỈ TIÊU ĐÁNH GIÁ HIỆU QUẢ";
            }
            String sTrongKy = "Đến";
            if (bTrongKy == "True" || bTrongKy == "on")
                sTrongKy = "";
            String sTenDoanhNghiep = Convert.ToString(CommonFunction.LayTruong("TCDN_DoanhNghiep", "iID_MaDoanhNghiep",
                                                     iID_MaDoanhNghiep,
                                                     "sTenDoanhNghiep"));
            
            String ThongTin = " Doanh nghiệp: " + sTenDoanhNghiep ;
            String DonViTinh = "";
            if (DVT == "1") DonViTinh = "Triệu đồng";
            else if (DVT == "1000") DonViTinh = "Nghìn đồng";
            else if (DVT == "1000000") DonViTinh = "Triệu đồng";
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iQuy);
            fr.SetValue("ThongTin", ThongTin);
            fr.SetValue("TenBieu1", TenBieu1);
            fr.SetValue("TenBieu2", TenBieu2);
            fr.SetValue("bTrongKy", sTrongKy);
            fr.SetValue("DVT", DonViTinh);
            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iQuy, String bTrongKy, String iID_MaDoanhNghiep, String iLoai, String DVT)
        {
            DataRow r;
            DataTable data = rptTCDN_BaoCaoChiTiet(MaND, iQuy, bTrongKy, iID_MaDoanhNghiep, iLoai, DVT);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);


            if (iLoai == "0")
            {
                String sTenDonViDauMoi = "", sTenKhoi = "", sTenNhom = "", sTenHinhThucHoatDong = "", sTenLoaiHinhDoanhNghiep = "";
                if (data.Rows.Count > 0)
                {
                    DataRow R = data.Rows[0];
                    sTenDonViDauMoi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
                    sTenKhoi = Convert.ToString(
                                 DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaKhoi"])).Rows[0]["sTen"]);
                    sTenNhom = Convert.ToString(
                                 DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaNhom"])).Rows[0]["sTen"]);
                    sTenHinhThucHoatDong = Convert.ToString(
                                 DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaHinhThucHoatDong"])).Rows[0]["sTen"]);
                    sTenLoaiHinhDoanhNghiep = Convert.ToString(
                                 DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaLoaiHinhDoanhNghiep"])).Rows[0]["sTen"]);

                }
                fr.SetValue("sTenDonViDauMoi", sTenDonViDauMoi);
                fr.SetValue("sTenKhoi", sTenKhoi);
                fr.SetValue("sTenNhom", sTenNhom);
                fr.SetValue("sTenHinhThucHoatDong", sTenHinhThucHoatDong);
                fr.SetValue("sTenLoaiHinhDoanhNghiep", sTenLoaiHinhDoanhNghiep);

                //Linh Vuc
                String SQL =
                  String.Format(
                      "SELECT * FROM TCDN_LinhVuc WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep ");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                DataTable dtLinhVuc = Connection.GetDataTable(cmd);
                fr.AddTable("dtLinhVuc", dtLinhVuc);
                dtLinhVuc.Dispose();

                //Don vi hach toan doc lap
                SQL =
                 String.Format(
                     "SELECT * FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND iLoai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep ");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                DataTable dtDonVi1 = Connection.GetDataTable(cmd);
                fr.AddTable("dtDonVi1", dtDonVi1);
                dtDonVi1.Dispose();

                //Don vi hach toan phụ thuộc
                SQL =
                 String.Format(
                     "SELECT * FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND iLoai=2 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep ");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                DataTable dtDonVi2 = Connection.GetDataTable(cmd);
                fr.AddTable("dtDonVi2", dtDonVi2);
                dtDonVi2.Dispose();

                //lien doanh lien ket
                SQL =
                String.Format(
                    "SELECT * FROM TCDN_CongTyLienDoanhLienKet WHERE iTrangThai=1  AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep ");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                DataTable dtCongTyLienDoanhLienKet = Connection.GetDataTable(cmd);
                fr.AddTable("dtCongTyLienDoanhLienKet", dtCongTyLienDoanhLienKet);
                dtCongTyLienDoanhLienKet.Dispose();
                cmd.Dispose();
            }
            data.Dispose();

        }
        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iQuy, String bTrongKy, String iID_MaDoanhNghiep, String iLoai, String DVT)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iLoai == "0")
            {
                sDuongDan = sFilePath_HoSoDoanhNghiep;
            }
            else if(iLoai=="4")
            {
                sDuongDan = sFilePath_Loai4;
            }
            else
            {
                sDuongDan = sFilePath;
            }


            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iQuy, bTrongKy, iID_MaDoanhNghiep, iLoai, DVT);
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

        public clsExcelResult ExportToExcel(String MaND, String iQuy, String bTrongKy, String iID_MaDoanhNghiep, String iLoai, String DVT)
        {
            HamChung.Language();
            String sDuongDan = "";
            if (iLoai == "0")
            {
                sDuongDan = sFilePath_HoSoDoanhNghiep;
            }
            else if (iLoai == "4")
            {
                sDuongDan = sFilePath_Loai4;
            }
            else
            {
                sDuongDan = sFilePath;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, iQuy, bTrongKy, iID_MaDoanhNghiep, iLoai, DVT);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "HoSoDoanhNghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaDonVi)
        {
            String MaND = User.Identity.Name;
            DataTable dt = TCSN_DoanhNghiepModels.Get_ListDoanhNghiep();
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

