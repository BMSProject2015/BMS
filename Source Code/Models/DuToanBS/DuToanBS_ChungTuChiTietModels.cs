using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Models
{
    public class DuToanBS_ChungTuChiTietModels
    {
        public const String sLNSBaoDam = "1040100";
        public const int iID_MaTrangThaiDuyetKT = 106;
        public static DataTable Getdata(String MaChungTu, String sOrder = "", String sLNS = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dt;
            String SQL = "SELECT * FROM DTBS_ChungTuChiTiet";
            String DK = "";
            String SapXep;
            if (sOrder != null && sOrder != "")
            {
                SapXep = sOrder;
            }
            else
            {
                SapXep = "dNgayTao DESC";
            }
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(MaChungTu) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "iID_MaChungTu=@iID_MaChungTu";
                cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
            }
            if (String.IsNullOrEmpty(sLNS) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = " WHERE " + DK;
            }
            SQL = SQL + DK;
            cmd.CommandText = SQL;
            dt = CommonFunction.dtData(cmd, SapXep, Trang, SoBanGhi);
            cmd.Dispose();
            return dt;
        }
        //Lấy dt chi tiết ngân sách
        public static DataTable Getdata(String MaChungTu,
                                    String sOrder = "",
                                    String sLNS = "",
                                    String sM = "",
                                    String sTM = "",
                                    String sTTM = "",
                                    String sNG = "",
                                    String sTNG = "",
                                    String iID_MaTrangThaiDuyet = "",
                                    int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dt;
            String SQL = "SELECT * FROM DTBS_ChungTuChiTiet";
            String DK = "";
            String SapXep;
            if (sOrder != null && sOrder != "")
            {
                SapXep = sOrder;
            }
            else
            {
                SapXep = "dNgayTao DESC";
            }
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(MaChungTu) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "iID_MaChungTu=@iID_MaChungTu";
                cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
            }
            if (String.IsNullOrEmpty(sLNS) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            if (String.IsNullOrEmpty(sM) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sM=@sM";
                cmd.Parameters.AddWithValue("@sM", sM);
            }
            if (String.IsNullOrEmpty(sTM) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sTM=@sTM";
                cmd.Parameters.AddWithValue("@sTM", sTM);
            }
            if (String.IsNullOrEmpty(sTTM) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sTTM=@sTTM";
                cmd.Parameters.AddWithValue("@sTTM", sTTM);
            }
            if (String.IsNullOrEmpty(sNG) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sNG=@sNG";
                cmd.Parameters.AddWithValue("@sNG", sNG);
            }
            if (String.IsNullOrEmpty(sNG) == false)
            {
                if (DK != "") DK += " AND ";
                DK += "sTNG=@sTNG";
                cmd.Parameters.AddWithValue("@sTNG", sTNG);
            }
            if (CommonFunction.IsNumeric(iID_MaTrangThaiDuyet))
            {
                if (DK != "") DK += " AND ";
                DK += "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = " WHERE " + DK;
            }
            SQL = SQL + DK;
            cmd.CommandText = SQL;
            dt = CommonFunction.dtData(cmd, SapXep, Trang, SoBanGhi);
            cmd.Dispose();
            return dt;
        }

        public static DataTable GetChungTuChiTiet_LichSu(String iID_MaChungTu)
        {
            DataTable vR;
            String SQL;
            SqlCommand cmd;
            SQL = "SELECT TOP 10 * FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu ORDER BY dNgaySua DESC";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTu_Gom(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu_TLTH WHERE iTrangThai=1 AND iID_MaChungTu_TLTH=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTuChiTiet(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String sLNS)
        {

            DataTable vR, dtChungTu;
            String SQL, DK = "", Dk1 = "";
            SqlCommand cmd;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);

            dtChungTu = GetChungTu(iID_MaChungTu);
            String[] arrLNS = sLNS.Split(',');
            String DKLNS = "";
            cmd = new SqlCommand();
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += " sLNS LIKE @LNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("LNS" + i, arrLNS[i] + "%");
            }
            if (String.IsNullOrEmpty(DKLNS)) DKLNS = "0=1";
            DK = String.Format("iTrangThai=1 AND ({0}) ", DKLNS);

            DataRow RChungTu = dtChungTu.Rows[0];

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan,sGhiChu,sMaCongTrinh";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');
            //<--Lay toan bo Muc luc ngan sach
            if (arrGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "sLNS")
                        {
                            DK += String.Format(" AND sLNS IN ({0}) ", sLNS);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
                DK += " AND( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                DK += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
            }
            if (dt.Rows.Count > 0)
                DK += " ) ";
            SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2}  ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, DK, MucLucNganSachModels.strDSTruongSapXep);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //Lay toan bo Muc luc ngan sach-->
            cmd = new SqlCommand();
            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);


            if (arrGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaDonVi"]) == false)
                {
                    DK += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                    cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + arrGiaTriTimKiem["iID_MaDonVi"] + "%");
                }
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "sLNS")
                        {
                            DK += String.Format(" AND sLNS IN ({0}) ", sLNS);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
            }
            DK += " AND (";
            for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
            {
                DK += arrDSTruongTien[i] + "<>0 OR ";
            }
            DK = DK.Substring(0, DK.Length - 3);
            DK += ") ";


            SQL = String.Format("SELECT *,sTenDonVi as sTenDonVi_BaoDam FROM DTBS_ChungTuChiTiet WHERE {0} ORDER BY sXauNoiMa,iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;
            DataColumn column;



            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {


                column = new DataColumn(arrDSTruongTien[j], typeof(String));
                column.AllowDBNull = true;
                vR.Columns.Add(column);

            }
            int vRCount = vR.Rows.Count;
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    for (int k = 0; k < arrDSTruong.Length; k++)
                    {
                        if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                if ((vR.Columns[k].ColumnName.StartsWith("b") == false && vR.Columns[k].ColumnName != "iID_MaMucLucNganSach_Cha") || vR.Columns[k].ColumnName == "bLaHangCha")
                                    vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                else
                                    vR.Rows[i][k] = vR.Rows[i][k];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row = vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                if ((vR.Columns[k].ColumnName.StartsWith("b") == false && vR.Columns[k].ColumnName != "iID_MaMucLucNganSach_Cha") || vR.Columns[k].ColumnName == "bLaHangCha")
                                    row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                else
                                {
                                    row[k] = vR.Rows[i][k];
                                }
                            }
                            vR.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }

            }
            vR.Columns.Add("bPhanCap", typeof(Boolean));
            dtChungTu.Dispose();
            dtChungTuChiTiet.Dispose();
            vR.Dispose();
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTuChiTiet_ChiTapTrung(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String sLNS)
        {

            DataTable vR, dtChungTu;
            String SQL, DK = "", Dk1 = "";
            SqlCommand cmd;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);

            dtChungTu = GetChungTu(iID_MaChungTu);
            DK = String.Format("iTrangThai=1 AND sLNS IN ({0}) ", sLNS);

            DataRow RChungTu = dtChungTu.Rows[0];

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan,sGhiChu,sMaCongTrinh";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');



            //<--Lay toan bo Muc luc ngan sach
            cmd = new SqlCommand();


            if (arrGiaTriTimKiem != null )
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "sLNS")
                        {
                            DK += String.Format(" AND sLNS IN ({0}) ", sLNS);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
                DK += " AND( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                DK += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
            }
            if (dt.Rows.Count > 0)
                DK += " ) ";
            SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2}  ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, DK, MucLucNganSachModels.strDSTruongSapXep);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //Lay toan bo Muc luc ngan sach-->
            cmd = new SqlCommand();
            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);


            if (arrGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaDonVi"]) == false)
                {
                    DK += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                    cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + arrGiaTriTimKiem["iID_MaDonVi"] + "%");
                }
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "sLNS")
                        {
                            DK += String.Format(" AND sLNS IN ({0}) ", sLNS);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
            }
            DK += " AND (";
            for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
            {
                DK += arrDSTruongTien[i] + "<>0 OR ";
            }
            DK = DK.Substring(0, DK.Length - 3);
            DK += ") ";


            SQL = String.Format("SELECT *,sTenDonVi as sTenDonVi_BaoDam FROM DTBS_ChungTuChiTiet WHERE {0} ORDER BY sXauNoiMa,iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;
            DataColumn column;



            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {


                column = new DataColumn(arrDSTruongTien[j], typeof(String));
                column.AllowDBNull = true;
                vR.Columns.Add(column);

            }
            int vRCount = vR.Rows.Count;
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    for (int k = 0; k < arrDSTruong.Length; k++)
                    {
                        if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                if ((vR.Columns[k].ColumnName.StartsWith("b") == false && vR.Columns[k].ColumnName != "iID_MaMucLucNganSach_Cha") || vR.Columns[k].ColumnName == "bLaHangCha")
                                    vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                else
                                    vR.Rows[i][k] = vR.Rows[i][k];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row = vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                if ((vR.Columns[k].ColumnName.StartsWith("b") == false && vR.Columns[k].ColumnName != "iID_MaMucLucNganSach_Cha") || vR.Columns[k].ColumnName == "bLaHangCha")
                                    row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                else
                                {
                                    row[k] = vR.Rows[i][k];
                                }
                            }
                            vR.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }
            }
            SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,
SUM(rTuChi) as rTuChi
FROM
(
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
,rTuChi=SUM(rTuChi)
 FROM DTBS_ChungTuChiTiet
 WHERE iTrangThai=1  AND sLNS='1010000' AND iID_MaDonVi =@iID_MaDonVi AND iNamLamViec=@iNamLamViec 
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
 UNION ALL
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
,rTuChi=SUM(rTuChi)
 FROM DTBS_ChungTuChiTiet
 WHERE iTrangThai=1  AND (sLNS='1020100' OR sLNS='1020000') 
 AND iID_MaDonVi =@iID_MaDonVi AND iNamLamViec=@iNamLamViec
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi

 UNION ALL
 SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
,rTuChi=SUM(rTuChi)
 FROM DTBS_ChungTuChiTiet_PhanCap
 WHERE iTrangThai=1  AND (sLNS='1020100' OR sLNS='1020000') 
 AND iID_MaDonVi =@iID_MaDonVi AND iNamLamViec=@iNamLamViec
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi) a
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
 HAVING SUM(rTuChi)<>0
 ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
 ");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", RChungTu["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", RChungTu["iID_MaDonVi"]);
            DataTable dtChiTieu = Connection.GetDataTable(cmd);

            //ghep dtChiTieu vao VR
            arrDSTruong = "sLNS,sL,sK,sM,sTM,sTM,sTTM,sNG".Split(',');
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChiTieu.Rows.Count; j++)
                {

                    Boolean ok = true;
                    for (int k = 0; k < arrDSTruong.Length; k++)
                    {
                        if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChiTieu.Rows[j][arrDSTruong[k]]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (count == 0)
                        {
                        
                                    vR.Rows[i]["rTuChi"] = dtChiTieu.Rows[j]["rTuChi"];
                            
                            count++;
                        }
                        else
                        {
                            DataRow row = vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                if (vR.Columns[k].ColumnName.StartsWith("b") == false)
                                    row[k] = dtChiTieu.Rows[j][vR.Columns[k].ColumnName];
                                else
                                {
                                    row[k] = vR.Rows[i][k];
                                }
                            }
                            vR.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }
            }





            vR.Columns.Add("bPhanCap", typeof(Boolean));
            dtChungTu.Dispose();
            dtChungTuChiTiet.Dispose();
            vR.Dispose();
            cmd.Dispose();
            return vR;
        }


        public static DataTable GetChungTuChiTietLan2(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String sLNS)
        {

            DataTable vR, dt;
            String SQL, DK = "", Dk1 = "";
            SqlCommand cmd = new SqlCommand();
            SQL = String.Format(@"SELECT sTenDonVi as sTenDonVi_BaoDam,* FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTu");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            dt = Connection.GetDataTable(cmd);
            String iID_MaNganh = Convert.ToString(dt.Rows[0]["iID_MaDonVi"]);
            cmd = new SqlCommand();
            SQL = String.Format(@"SELECT sTenDonVi as sTenDonVi_BaoDam,* FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND MaLoai=2");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            //Nếu có chua co sẽ them 1 dong với MaLoai=2
            if (vR.Rows.Count == 0)
            {
                String iID_MaPhongBan = "";
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                    
                    dtPhongBan.Dispose();
                }
                if (iID_MaPhongBan == "06")
                {
                    //Lay mo ta nganh 
                    DataRow r = dt.Rows[0];
                    String sXauNoiMa = "1050000-460-468-8950-8999-20-60";
                    String sMoTa = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach", "sXauNoiMa", sXauNoiMa, "sMoTa"));
                    Bang bang = new Bang("DTBS_ChungTuChiTiet");
                    bang.DuLieuMoi = true;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", r["iID_MaChungTuChiTiet"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", r["iID_MaPhongBan"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", r["sTenPhongBan"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", r["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", r["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", r["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", r["bChiNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", r["iID_MaTrangThaiDuyet"]);
                    bang.CmdParams.Parameters.AddWithValue("@iKyThuat", r["iKyThuat"]);
                    bang.CmdParams.Parameters.AddWithValue("@MaLoai", 2);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", r["iID_MaDonVi"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", r["sTenDonVi"]);
                    bang.CmdParams.Parameters.AddWithValue("@sGhiChu", r["sGhiChu"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", r["iID_MaPhongBanDich"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", r["iID_MaMucLucNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", r["iID_MaMucLucNganSach_Cha"]);
                    bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                    bang.CmdParams.Parameters.AddWithValue("@sLNS", "1050000");
                    bang.CmdParams.Parameters.AddWithValue("@sL", "460");
                    bang.CmdParams.Parameters.AddWithValue("@sK", "468");
                    bang.CmdParams.Parameters.AddWithValue("@sM", "8950");
                    bang.CmdParams.Parameters.AddWithValue("@sTM", "8999");
                    bang.CmdParams.Parameters.AddWithValue("@sTTM", "20");
                    bang.CmdParams.Parameters.AddWithValue("@sNG", "60");
                    bang.CmdParams.Parameters.AddWithValue("@sTNG", r["sTNG"]);
                    bang.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);
                    bang.CmdParams.Parameters.AddWithValue("@bsMaCongTrinh", false);
                    bang.CmdParams.Parameters.AddWithValue("@bsTenCongTrinh", false);
                    bang.CmdParams.Parameters.AddWithValue("@brNgay", false);
                    bang.CmdParams.Parameters.AddWithValue("@brSoNguoi", false);
                    bang.CmdParams.Parameters.AddWithValue("@brChiTaiKhoBac", false);
                    bang.CmdParams.Parameters.AddWithValue("@brChiTapTrung", false);
                    bang.Save();
                }
                else
                {
                    String iID_MaNganh_MLNS = "";
                    SQL = String.Format("SELECT TOP 1 iID_MaNganhMLNS FROM NS_MucLucNganSach_Nganh WHERE iTrangThai=1 AND iID_MaNganh=@iID_MaNganh");
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iID_MaNganh", iID_MaNganh);
                    iID_MaNganh_MLNS = Connection.GetValueString(cmd, "");
                    String[] arrMaNganh = iID_MaNganh_MLNS.Split(',');

                    for (int i = 0; i < arrMaNganh.Length; i++)
                    {
                        //Lay mo ta nganh 
                        DataRow r = dt.Rows[0];
                        String sXauNoiMa = r["sLNS"] + "-" + r["sL"] + "-" + r["sK"] + "-" + r["sM"] + "-" + r["sTM"] + "-" + r["sTTM"] + "-" + arrMaNganh[i];
                        String sMoTa = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach", "sXauNoiMa", sXauNoiMa, "sMoTa"));
                        Bang bang = new Bang("DTBS_ChungTuChiTiet");
                        bang.DuLieuMoi = true;
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", r["iID_MaChungTuChiTiet"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", r["iID_MaPhongBan"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", r["sTenPhongBan"]);
                        bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", r["iNamLamViec"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", r["iID_MaNguonNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", r["iID_MaNamNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", r["bChiNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", r["iID_MaTrangThaiDuyet"]);
                        bang.CmdParams.Parameters.AddWithValue("@iKyThuat", r["iKyThuat"]);
                        bang.CmdParams.Parameters.AddWithValue("@MaLoai", 2);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", r["iID_MaDonVi"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", r["sTenDonVi"]);
                        bang.CmdParams.Parameters.AddWithValue("@sGhiChu", r["sGhiChu"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", r["iID_MaPhongBanDich"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", r["iID_MaMucLucNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", r["iID_MaMucLucNganSach_Cha"]);
                        bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                        bang.CmdParams.Parameters.AddWithValue("@sLNS", r["sLNS"]);
                        bang.CmdParams.Parameters.AddWithValue("@sL", r["sL"]);
                        bang.CmdParams.Parameters.AddWithValue("@sK", r["sK"]);
                        bang.CmdParams.Parameters.AddWithValue("@sM", r["sM"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTM", r["sTM"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTTM", r["sTTM"]);
                        bang.CmdParams.Parameters.AddWithValue("@sNG", arrMaNganh[i]);
                        bang.CmdParams.Parameters.AddWithValue("@sTNG", r["sTNG"]);
                        bang.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);
                        bang.CmdParams.Parameters.AddWithValue("@bsMaCongTrinh", false);
                        bang.CmdParams.Parameters.AddWithValue("@bsTenCongTrinh", false);
                        bang.CmdParams.Parameters.AddWithValue("@brNgay", false);
                        bang.CmdParams.Parameters.AddWithValue("@brSoNguoi", false);
                        bang.CmdParams.Parameters.AddWithValue("@brChiTaiKhoBac", false);
                        bang.CmdParams.Parameters.AddWithValue("@brChiTapTrung", false);
                        bang.Save();
                    }
                }//end if maphongban06
              

            }
            cmd = new SqlCommand();
            SQL = String.Format(@"SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sTenCongTrinh,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi as sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan
,rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,sGhiChu,sMaCongTrinh
,brTuChi,brHienVat,brHangNhap,brHangMua,brTonKho,brPhanCap,brDuPhong,bsTenCongTrinh,brNgay,brSoNguoi,brChiTaiKhoBac,brChiTapTrung
FROM DTBS_ChungTuChiTiet_PhanCap
WHERE  iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu

UNION

SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sTenCongTrinh, sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi as sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan
,rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,sGhiChu,sMaCongTrinh
,brTuChi,brHienVat,brHangNhap,brHangMua,brTonKho,brPhanCap,brDuPhong,bsTenCongTrinh,brNgay,brSoNguoi,brChiTaiKhoBac,brChiTapTrung
FROM DTBS_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu

ORDER BY sLNS, sM,sTM,sTTM,sNG, iID_MaDonVi");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            if (vR.Rows.Count > 0)
            {
                vR.Rows[0]["brTuChi"] = true;
                vR.Rows[0]["brHienVat"] = true;
                vR.Rows[0]["brHangNhap"] = true;
                vR.Rows[0]["brHangMua"] = true;
                vR.Rows[0]["brTonKho"] = true;
                vR.Rows[0]["brPhanCap"] = true;
                vR.Rows[0]["brDuPhong"] = true;
            }
            vR.Columns.Add("bPhanCap", typeof(Boolean));
            vR.Dispose();

            return vR;
        }


        public static DataTable GetChungTuChiTiet_Gom(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND)
        {

            DataTable vR, dtChungTu;
            String SQL, DK = "", Dk1 = "";
            SqlCommand cmd;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);

            dtChungTu = GetChungTu_Gom(iID_MaChungTu);

            String[] arrChungTu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]).Split(',');
            cmd = new SqlCommand();
            for (int i = 0; i < arrChungTu.Length; i++)
            {
                DK += " iID_MaChungTu=@iID_MaChungTu" + i;
                if (i < arrChungTu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("iID_MaChungTu" + i, arrChungTu[i]);
            }
            if (!String.IsNullOrEmpty(DK))
            {
                DK = "AND(" + DK + ")";
            }
            SQL = String.Format(@"SELECT DISTINCT sDSLNS FROM DTBS_ChungTu WHERE iTrangThai=1 {0} ", DK);
            DK = "";
            cmd.CommandText = SQL;
            DataTable dtsLNS = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtsLNS.Rows.Count > 0)
            {
                for (int i = 0; i < dtsLNS.Rows.Count; i++)
                {
                    Dk1 += String.Format(" sLNS LIKE '{0}%' ", dtsLNS.Rows[i]["sDSLNS"]);
                    if (i < dtsLNS.Rows.Count - 1)
                        Dk1 += " OR ";
                }
                DK = String.Format("iTrangThai=1 AND ({0}) ", Dk1);
            }


            DataRow RChungTu = dtChungTu.Rows[0];

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sMaCongTrinh";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');



            //<--Lay toan bo Muc luc ngan sach
            cmd = new SqlCommand();


            if (arrGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }
            if (dt.Rows.Count > 0)
                DK += " AND( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                DK += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
            }
            if (dt.Rows.Count > 0)
                DK += " ) ";
            SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2} ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, DK, MucLucNganSachModels.strDSTruongSapXep);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //Lay toan bo Muc luc ngan sach-->


            DK = "";
            cmd = new SqlCommand();
            for (int i = 0; i < arrChungTu.Length; i++)
            {
                DK += " iID_MaChungTu=@iID_MaChungTu" + i;
                if (i < arrChungTu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("iID_MaChungTu" + i, arrChungTu[i]);
            }

            DK = "iTrangThai=1 AND (" + DK + ")";

            if (arrGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaDonVi"]) == false)
                {
                    DK += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                    cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + arrGiaTriTimKiem["iID_MaDonVi"] + "%");
                }
            }
            DK += " AND (";
            for (int i = 1; i < arrDSTruongTien.Length - 4; i++)
            {
                DK += arrDSTruongTien[i] + "<>0 OR ";
            }
            DK = DK.Substring(0, DK.Length - 3);
            DK += ") ";


            SQL = String.Format("SELECT * FROM DTBS_ChungTuChiTiet WHERE {0} ORDER BY sXauNoiMa,iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;
            DataColumn column;



            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {


                column = new DataColumn(arrDSTruongTien[j], typeof(String));
                column.AllowDBNull = true;
                vR.Columns.Add(column);

            }
            int vRCount = vR.Rows.Count;
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    for (int k = 0; k < arrDSTruong.Length; k++)
                    {
                        if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row = vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            vR.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }

            }
            vR.Columns.Add("bPhanCap", typeof(Boolean));
            dtChungTu.Dispose();
            dtChungTuChiTiet.Dispose();
            vR.Dispose();
            cmd.Dispose();
            return vR;
        }

        public static DataTable GetChungTuChiTiet_PhanCap(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String sLNS, String sXauNoiMa, String iKyThuat, String MaLoai)
        {

            DataTable vR;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan,sGhiChu,sMaCongTrinh";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String SQL, DK;
            SqlCommand cmd;

            #region NganhKyThuat
            if (iKyThuat == "1")
            {
                sLNS = "1020100";
                //<--Lay toan bo Muc luc ngan sach
                cmd = new SqlCommand();
                //Phan cap lan 2: danh sach nganh theo nguoi quan ly
                if (MaLoai == "2")
                {
                    DK = String.Format("iTrangThai=1 AND sLNS LIKE '{0}%' AND sXauNoiMa LIKE '%{1}%'", sLNS, sXauNoiMa);
                }
                else
                {
                    DK = String.Format("iTrangThai=1 AND sLNS LIKE '{0}%' AND sXauNoiMa LIKE '%{1}%'", sLNS, sXauNoiMa);
                }

                if (arrGiaTriTimKiem != null)
                {
                    //if (arrGiaTriTimKiem["iID_MaDonVi"] != null)
                    //    DK += String.Format("iID_MaDonVi={0}", arrGiaTriTimKiem["iID_MaDonVi"]);
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                    DK += " AND( ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                    String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                    DK += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                    if (i < dt.Rows.Count - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
                }
                if (dt.Rows.Count > 0)
                    DK += " ) ";
                SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2} ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, DK, MucLucNganSachModels.strDSTruongSapXep);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();
                //Lay toan bo Muc luc ngan sach-->
                DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";

                if (arrGiaTriTimKiem != null)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaDonVi"]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                        cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + arrGiaTriTimKiem["iID_MaDonVi"] + "%");
                    }
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaPhongBanDich"]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", "iID_MaPhongBanDich");
                        cmd.Parameters.AddWithValue("@" + "iID_MaPhongBanDich", "%" + arrGiaTriTimKiem["iID_MaPhongBanDich"] + "%");
                    }
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);

                        }
                    }
                }
                DK += " AND (";
                for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
                {
                    DK += arrDSTruongTien[i] + "<>0 OR ";
                }
                DK = DK.Substring(0, DK.Length - 3);
                DK += ") ";

                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                SQL = String.Format(@"SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sTenCongTrinh,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi as sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan
,rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,sGhiChu
FROM DTBS_ChungTuChiTiet_PhanCap
WHERE  {0}

UNION

SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sTenCongTrinh, sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi as sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan
,rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,sGhiChu
FROM DTBS_ChungTuChiTiet
WHERE {0}

ORDER BY sM,sTM,sTTM,sNG, iID_MaDonVi", DK);
                cmd.CommandText = SQL;

                DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
                int cs0 = 0;
                DataColumn column;



                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {


                    column = new DataColumn(arrDSTruongTien[j], typeof(String));
                    column.AllowDBNull = true;
                    vR.Columns.Add(column);

                }
                int vRCount = vR.Rows.Count;
                for (int i = 0; i < vRCount; i++)
                {
                    int count = 0;
                    for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                    {

                        Boolean ok = true;
                        for (int k = 2; k < arrDSTruong.Length; k++)
                        {
                            if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                        {
                            if (count == 0)
                            {
                                for (int k = 0; k < vR.Columns.Count - 1; k++)
                                {
                                    if (vR.Columns[k].ColumnName.StartsWith("b") == false || vR.Columns[k].ColumnName == "bLaHangCha")
                                        vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                    else
                                        vR.Rows[i][k] = vR.Rows[i][k];
                                }
                                count++;
                            }
                            else
                            {
                                DataRow row = vR.NewRow();
                                for (int k = 0; k < vR.Columns.Count - 1; k++)
                                {
                                    if (vR.Columns[k].ColumnName.StartsWith("b") == false || vR.Columns[k].ColumnName == "bLaHangCha")
                                        row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                    else
                                    {
                                        row[k] = vR.Rows[i][k];
                                    }
                                }
                                vR.Rows.InsertAt(row, i + 1);
                                i++;
                                vRCount++;
                            }
                        }
                    }

                }
            }
            #endregion
            #region Nganh khac

            else
            {
                sLNS = "1020100";
                //<--Lay toan bo Muc luc ngan sach
                cmd = new SqlCommand();
                DK = String.Format("iTrangThai=1 AND sLNS LIKE '{0}%' AND sXauNoiMa LIKE '%{1}%'", sLNS, sXauNoiMa);
                if (arrGiaTriTimKiem != null)
                {
                    //if (arrGiaTriTimKiem["iID_MaDonVi"] != null)
                    //    DK += String.Format("iID_MaDonVi={0}", arrGiaTriTimKiem["iID_MaDonVi"]);
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                    DK += " AND( ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                    String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                    DK += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                    if (i < dt.Rows.Count - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
                }
                if (dt.Rows.Count > 0)
                    DK += " ) ";
                SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2} ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, DK, MucLucNganSachModels.strDSTruongSapXep);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();
                //Lay toan bo Muc luc ngan sach-->
                DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";

                if (arrGiaTriTimKiem != null)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaDonVi"]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                        cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + arrGiaTriTimKiem["iID_MaDonVi"] + "%");
                    }
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaPhongBanDich"]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", "iID_MaPhongBanDich");
                        cmd.Parameters.AddWithValue("@" + "iID_MaPhongBanDich", "%" + arrGiaTriTimKiem["iID_MaPhongBanDich"] + "%");
                    }
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);

                        }
                    }
                }
                DK += " AND (";
                for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
                {
                    DK += arrDSTruongTien[i] + "<>0 OR ";
                }
                DK = DK.Substring(0, DK.Length - 3);
                DK += ") ";

                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                SQL = String.Format("SELECT *,sTenDonVi as sTenDonVi_BaoDam  FROM DTBS_ChungTuChiTiet_PhanCap WHERE {0} ORDER BY sXauNoiMa,iID_MaDonVi", DK);
                cmd.CommandText = SQL;

                DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
                int cs0 = 0;
                DataColumn column;



                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {


                    column = new DataColumn(arrDSTruongTien[j], typeof(String));
                    column.AllowDBNull = true;
                    vR.Columns.Add(column);

                }
                int vRCount = vR.Rows.Count;
                for (int i = 0; i < vRCount; i++)
                {
                    int count = 0;
                    for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                    {

                        Boolean ok = true;
                        for (int k = 0; k < arrDSTruong.Length; k++)
                        {
                            if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                        {
                            if (count == 0)
                            {
                                for (int k = 0; k < vR.Columns.Count - 1; k++)
                                {
                                    if (vR.Columns[k].ColumnName.StartsWith("b") == false || vR.Columns[k].ColumnName == "bLaHangCha")
                                        vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                    else
                                        vR.Rows[i][k] = vR.Rows[i][k];
                                }
                                count++;
                            }
                            else
                            {
                                DataRow row = vR.NewRow();
                                for (int k = 0; k < vR.Columns.Count - 1; k++)
                                {
                                    if (vR.Columns[k].ColumnName.StartsWith("b") == false || vR.Columns[k].ColumnName == "bLaHangCha")
                                        row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                                    else
                                    {
                                        row[k] = vR.Rows[i][k];
                                    }
                                }
                                vR.Rows.InsertAt(row, i + 1);
                                i++;
                                vRCount++;
                            }
                        }
                    }

                }
            }
            #endregion
            vR.Columns.Add("bPhanCap", typeof(Boolean));
            return vR;
        }

        public static DataTable GetChungTuChiTiet_TongCong(String iID_MaChungTu, String[] arrDSTruongTien, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');

            cmd = new SqlCommand();

            DK = "iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                {
                    DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                    cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                }
            }

            String Truong = "SUM(rTongSoNamTruoc) AS SumrTongSoNamTruoc,SUM(rUocThucHien) AS SumrUocThucHien,SUM(rTongSo) AS SumrTongSo";
            for (int i = 0; i < arrDSTruongTien.Length; i++)
            {
                if (arrDSTruongTien[i].StartsWith("r"))
                {
                    if (Truong != "") Truong += ",";
                    Truong += String.Format("SUM({0}) AS Sum{0}", arrDSTruongTien[i]);
                }
            }
            SQL = String.Format("SELECT {0} FROM DTBS_ChungTuChiTiet WHERE {1}", Truong, DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int Delete_ChungTuChiTiet(String MaChungTuChiTiet, String IPSua, String MaNguoiDungSua)
        {
            int vR = 0;
            try
            {
                //Xóa dữ liệu trong bảng DT_DotNganSach
                Bang bang = new Bang("DTBS_ChungTuChiTiet");
                bang.MaNguoiDungSua = MaNguoiDungSua;
                bang.IPSua = IPSua;
                bang.GiaTriKhoa = MaChungTuChiTiet;
                bang.Delete();
                vR = 1;
            }
            catch
            {
                vR = 0;
            }
            return vR;
        }

        public static DataTable Get_dtChiTietMucLucNganSach(String[] arrDSTruong, List<String> lstDSGiaTri)
        {
            SqlCommand cmd = new SqlCommand();

            String DK = "";
            for (int i = 0; i < lstDSGiaTri.Count; i++)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("@{0}={0}", arrDSTruong[i]);
                cmd.Parameters.AddWithValue("@" + arrDSTruong[i], lstDSGiaTri[i]);
            }
            cmd.CommandText = "SELECT * FROM NS_MucLucNganSach WHERE " + DK;
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Thêm 1 chứng từ chi tiết
        /// </summary>
        /// <param name="ParentID">Phần trước của danh sách giá trị truyền vào</param>
        /// <param name="Values">Danh sách giá trị truyền vào (Đối với Form là Form.Request)</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns>Danh sách các lỗi của giá trị truyền vào</returns>
        public static NameValueCollection ThemChungTuChiTiet(String iID_MaChungTu, String ParentID, NameValueCollection Values, String MaND, String IPSua)
        {
            String sLNS = Values[ParentID + "_sLNS"];
            String iID_MaDonVi = Values[ParentID + "_iID_MaDonVi"];
            String sTenDonVi = Values[ParentID + "_sTenDonVi"];
            String DSTruong = MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');

            List<String> lstDSGiaTri = new List<String>();
            for (int i = 0; i < arrDSTruong.Length - 1; i++)
            {
                lstDSGiaTri.Add(Convert.ToString(Values[ParentID + "_" + arrDSTruong[i]]));
            }
            DataTable dtMucLucNganSach = DuToanBS_ChungTuChiTietModels.Get_dtChiTietMucLucNganSach(arrDSTruong, lstDSGiaTri);

            Bang bang = new Bang("DTBS_ChungTuChiTiet");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Values);
            bang.DuLieuMoi = true;

            //bang.CmdParams.Parameters.AddWithValue("@sTenCongTrinh", Values[ParentID + "_sTenCongTrinh"]);

            //Bắt buộc phải có iID_MaMucLucNganSach
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Chưa chọn đơn vị");
            }

            if (arrLoi.Count == 0)
            {
                String iID_MaMucLucNganSach = "";
                String sXauNoiMa = "";

                for (int i = 0; i < arrDSTruong.Length - 1; i++)
                {
                    String sXauMaTG = Convert.ToString(Values[ParentID + "_" + arrDSTruong[i]]);
                    if (String.IsNullOrEmpty(sXauMaTG))
                    {
                        break;
                    }
                    else
                    {
                        if (sXauNoiMa != "") sXauNoiMa += "-";
                        sXauNoiMa += sXauMaTG;
                    }
                }

                if (dtMucLucNganSach.Rows.Count > 0)
                {
                    iID_MaMucLucNganSach = Convert.ToString(dtMucLucNganSach.Rows[0]["iID_MaMucLucNganSach"]);
                    //<--Thêm tham số từ bảng MucLucNganSach
                    String[] arrDSDuocNhapTruongTien = DuToanModels.strDSDuocNhapTruongTien.Split(',');//suadutoan
                    for (int i = 0; i < arrDSDuocNhapTruongTien.Length; i++)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@" + arrDSDuocNhapTruongTien[i], dtMucLucNganSach.Rows[0][arrDSDuocNhapTruongTien[i]]);
                    }
                    //-->Thêm tham số từ bảng MucLucNganSach
                }

                //<--Xác định trường rTongSo
                Double rTongSo = 0;
                Double rTongSo_DonVi = 0;
                String DSTruongTien = DuToanModels.strDSTruongTien;
                String[] arrDSTruongTien = DSTruongTien.Split(',');
                for (int i = 0; i < arrDSTruongTien.Length; i++)
                {
                    if (arrDSTruongTien[i] != "rChiTapTrung" && arrDSTruongTien[i] != "rNgay" && arrDSTruongTien[i] != "rSoNguoi" && arrDSTruongTien[i] != "rChiTapTrung_DonVi" && arrDSTruongTien[i] != "rNgay_DonVi" && arrDSTruongTien[i] != "rSoNguoi_DonVi" && arrDSTruongTien[i].StartsWith("r"))
                    {
                        if (CommonFunction.IsNumeric(bang.CmdParams.Parameters["@" + arrDSTruongTien[i]].Value) && arrDSTruongTien[i].EndsWith("_DonVi") == false)
                        {
                            rTongSo += Convert.ToDouble(bang.CmdParams.Parameters["@" + arrDSTruongTien[i]].Value);
                        }
                        if (CommonFunction.IsNumeric(bang.CmdParams.Parameters["@" + arrDSTruongTien[i]].Value) && arrDSTruongTien[i].EndsWith("_DonVi") == true)
                        {
                            rTongSo_DonVi += Convert.ToDouble(bang.CmdParams.Parameters["@" + arrDSTruongTien[i]].Value);
                        }
                    }
                }
                bang.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);
                bang.CmdParams.Parameters.AddWithValue("@rTongSo_DonVi", rTongSo_DonVi);
                //-->Xác định trường rTongSo

                //<--Thêm tham số từ bảng DTBS_ChungTu
                DataTable dtChungTu = DuToan_ChungTuModels.GetChungTu(iID_MaChungTu);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDotNganSach", dtChungTu.Rows[0]["iID_MaDotNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", dtChungTu.Rows[0]["iID_MaChungTu"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                //-->Thêm tham số từ bảng DTBS_ChungTu

                if (iID_MaMucLucNganSach != "")
                {
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
                }
                bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                bang.Save();

                //<--Cập nhập lại bảng DTBS_ChungTu
                SqlCommand cmd;
                String sDSLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);
                if (sDSLNS.IndexOf(sLNS + ";") < 0)
                {
                    sDSLNS += sLNS + ";";
                    cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@sDSLNS", sDSLNS);
                    DuToan_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
                    cmd.Dispose();
                }
                dtChungTu.Dispose();
                //-->Cập nhập lại bảng DTBS_ChungTu

                //<--Cập nhập lại bảng DT_DotNganSach
                String iID_MaDotNganSach = Convert.ToString(dtChungTu.Rows[0]["iID_MaDotNganSach"]);
                cmd = new SqlCommand("SELECT sDSLNS FROM DT_DotNganSach WHERE iID_MaDotNganSach=@iID_MaDotNganSach");
                cmd.Parameters.AddWithValue("@iID_MaDotNganSach", iID_MaDotNganSach);
                sDSLNS = Connection.GetValueString(cmd, "");
                cmd.Dispose();
                if (sDSLNS.IndexOf(sLNS + ";") < 0)
                {
                    sDSLNS += sLNS + ";";
                    cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@sDSLNS", sDSLNS);
                    DuToan_DotNganSachModels.UpdateRecord(iID_MaDotNganSach, cmd.Parameters, MaND, IPSua);
                    cmd.Dispose();
                }
                //-->Cập nhập lại bảng DT_DotNganSach
            }
            return arrLoi;
        }

        public static String DinhDangTruongTien(Object GT)
        {
            String sGT = "&nbsp;";
            if (CommonFunction.IsNumeric(GT))
            {
                if (Convert.ToDouble(GT) != 0)
                {
                    sGT = CommonFunction.DinhDangSo(Convert.ToString(GT));
                }
            }
            else
            {
                sGT = Convert.ToString(GT);
            }
            return sGT;
        }

        public static DataTable Get_dtCayChiTiet(String iID_MaChungTu)
        {
            DataTable vR = null;
            String SQL;

            SQL = "SELECT  sLNS, iID_MaDonVi FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu GROUP BY sLNS, iID_MaDonVi";
            SQL = String.Format("SELECT DISTINCT NS_DonVi.sTen AS TenDonVi, tb1.* FROM ({0}) tb1 INNER JOIN NS_DonVi ON tb1.iID_MaDonVi=NS_DonVi.iID_MaDonVi ORDER BY sLNS, tb1.iID_MaDonVi,NS_DonVi.sTen", SQL);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            vR.Columns.Add("TenLoaiNganSach", typeof(String));
            vR.Columns.Add("iID_MaMucLucNganSach", typeof(String));

            for (int i = 0; i < vR.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(vR.Rows[i]["sLNS"]);
                SQL = "SELECT * FROM NS_MucLucNganSach WHERE sLNS=@sLNS AND sL=''";
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                DataTable dt = Connection.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    vR.Rows[i]["iID_MaMucLucNganSach"] = dt.Rows[0]["iID_MaMucLucNganSach"];
                    vR.Rows[i]["TenLoaiNganSach"] = dt.Rows[0]["sMoTa"];
                }
                else
                {
                    vR.Rows[i]["TenLoaiNganSach"] = sLNS;
                }
                cmd.Dispose();
                dt.Dispose();
            }

            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            dt.Dispose();


            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                }

            }
            //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else
            {
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                {
                    int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                    if (iID_MaTrangThaiDuyet_TuChoi > 0)
                    {
                        //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                        //cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                        //if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                        //{
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        //}
                        //cmd.Dispose();
                    }
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            String iID_MaNguoiDungTao = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);
            dt.Dispose();

            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }

            }

                //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else if (checkTroLyTongHop && MaND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            else
            {
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                {
                    int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                    if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                    }
                }
            }
            return vR;
        }
        //Lay trang thai duyet từ chối ngân sách bảo đảm
        public static int Get_iID_MaTrangThaiDuyet_BaoDam_TuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            String iKyThuat = Convert.ToString(dt.Rows[0]["iKyThuat"]);
            dt.Dispose();


            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = false;
            if (iKyThuat == "1")
            {
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet);
            }
            else
            {
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
            }
            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                }

            }
            //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else
            {
                if (iKyThuat == "1")
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
                    {
                        int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                        if (iID_MaTrangThaiDuyet_TuChoi > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        }
                    }
                }
                else
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                    {
                        int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                        if (iID_MaTrangThaiDuyet_TuChoi > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        }
                    }
                }
            }
            return vR;
        }
        //Lay trang thai duyet ngân sách bảo đảm
        public static int Get_iID_MaTrangThaiDuyet_BaoDam_TrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            int iCheck = Convert.ToInt32(dt.Rows[0]["iCheck"]);
            String iID_MaNguoiDungTao = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);
            String iKyThuat = Convert.ToString(dt.Rows[0]["iKyThuat"]);
            dt.Dispose();
            //Trolytonghop duoc trinh chung tu minh tao
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            Boolean CheckTrangThaiDuyetMoiTao = false;
            int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
            bool bcheckTC = false;
            if (iKyThuat == "1")
            {
                //     CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet);
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
                return vR;
            }
            else
            {
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
                bcheckTC = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet);
            }

            //la tro tong hop va la trang thai moi tao
            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {

                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }

            }

                //la tro ly tong hop chua gom chung tu
            else if (checkTroLyTongHop && iCheck == 0)
            {
                vR = -1;
            }
            else if (checkTroLyTongHop && MaND == iID_MaNguoiDungTao && bcheckTC)
            {
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            else
            {
                if (iKyThuat == "1")
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
                    {
                        if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                        }
                    }
                }
                else
                {
                    if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
                    {
                        if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                        }
                    }
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_Gom_BaoDam_TuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    //if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    //{
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    //}
                    //cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_Gom_BaoDam_TrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_Gom_TuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    //if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    //{
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    //}
                    //cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_Gom_TrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTH(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_Gom_THCuc_TrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTHCuc(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        public static int Get_iID_MaTrangThaiDuyet_Gom_THCuc_TuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = DuToanBS_ChungTuModels.LayChungTuTLTHCuc(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeDuToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        /// <summary>
        /// Lấy tổng dự toán đã duyệt của sLNS trong iNamLamViec
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaNguonNganSach"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <param name="bChiNganSach"></param>
        /// <returns></returns>
        public static DataTable LayTongDuToan(String sLNS,
                                              int iNamLamViec,
                                              String dNgayDotNganSach,
                                              int iID_MaNguonNganSach,
                                              int iID_MaNamNganSach,
                                              Boolean bChiNganSach, String iID_MaChiTieu)
        {
            DataTable vR;
            String SQL = "", DK = "";
            //nghiep edit : ko group ma hang muc cong trinh
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = MucLucNganSachModels.strDSTruongSapXep + ",iID_MaMucLucNganSach";
            String strTruongGroup = MucLucNganSachModels.strDSTruongSapXep + ",iID_MaMucLucNganSach";

            for (int i = 0; i < arrDSTruongTien.Length; i++)
            {
                if (arrDSTruongTien[i].StartsWith("r"))
                {
                    strTruong += String.Format(",SUM({0}) AS Sum{0}", arrDSTruongTien[i]);
                }
                else
                {
                    strTruong += String.Format(",{0}", arrDSTruongTien[i]);
                    strTruongGroup += String.Format(",{0}", arrDSTruongTien[i]);
                }
            }

            int iID_MaTrangThaiDuyet_DaDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan);
            DK = " sLNS=@sLNS AND iID_MaChungTu IN ( SELECT iID_MaDuToan FROM PB_ChiTieu_DuToan WHERE iID_MaChiTieu=@iID_MaChiTieu)";
            //DK = "iTrangThai=1 AND " +
            //     "iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_DaDuyet AND " +
            //     "sLNS=@sLNS AND " +
            //     "iNamLamViec=@iNamLamViec AND " +
            //     "iID_MaNguonNganSach=@iID_MaNguonNganSach AND " +
            //     "iID_MaNamNganSach=@iID_MaNamNganSach AND " +
            //     "bChiNganSach=@bChiNganSach AND " +
            //     "iID_MaDotNganSach IN (SELECT iID_MaDotNganSach FROM DT_DotNganSach WHERE iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach AND dNgayDotNganSach <= @dNgayDotNganSach)";

            SQL = String.Format("SELECT {0} FROM DTBS_ChungTuChiTiet WHERE {1} GROUP BY {2} ", strTruong, DK, strTruongGroup);
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_DaDuyet", iID_MaTrangThaiDuyet_DaDuyet);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            //cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            //cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            //cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            //cmd.Parameters.AddWithValue("@bChiNganSach", bChiNganSach);
            //cmd.Parameters.AddWithValue("@dNgayDotNganSach", String.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(dNgayDotNganSach)));
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Xac dinh bao nhieu LNS
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="isCheck"></param>
        /// <returns></returns>
        public static DataTable Get_LNS(String sLNS, ref int iCheck)
        {
            DataTable vR = null;
            String SQL;
            SQL =
                "select  distinct sL,sK from NS_MucLucNganSach where sLNS=@sLNS and sL<>'' and sK<>'' ";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            vR = Connection.GetDataTable(cmd);
            iCheck = vR.Rows.Count;

            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_LNS_80(String sLNS, ref int iCheck)
        {
            DataTable vR = null;
            String SQL;
            SQL =
                "select  distinct sL,sK,sM,sTM,sTTM from NS_MucLucNganSach where sLNS LIKE @sLNS and sL<>'' and sK<>'' ";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS + "%");
            vR = Connection.GetDataTable(cmd);
            iCheck = vR.Rows.Count;

            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_RowChungTuChiTiet(String iID_MaChungTu)
        {
            DataTable vR = null;
            String SQL;
            SQL =
                "select  * from DTBS_ChungTuChiTiet where iID_MaChungTuChiTiet=@iID_MaChungTu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static double dTongConLai(String iID_MaChungTu, String TruongTien)
        {
            String SQL = "";
            SqlCommand cmd;
            double dTong;
            SQL = String.Format(@"SELECT SUM({0})  FROM (
SELECT SUM({0}) as {0}
 FROM  DTBS_ChungTuChiTiet_PhanCap
 WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu

UNION
SELECT SUM({0}) as {0} 
 FROM  DTBS_ChungTuChiTiet
 WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu
) a", TruongTien);
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            dTong = Convert.ToDouble(Connection.GetValue(cmd, 0));
            return dTong;
        }
        public static Boolean CheckDonViBaoDam2Lan(String iID_MaDonVi)
        {
            String SQL = "";
            SqlCommand cmd;
            SQL = String.Format(@"SELECT COUNT(sTenKhoa) FROM DC_DanhMuc
WHERE sTenKhoa=@sTenKhoa AND iID_MaLoaiDanhMuc IN(
SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc
WHERE sTenBang='DVBDKT')");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", iID_MaDonVi);
            int vR = Convert.ToInt16(Connection.GetValue(cmd, 0));
            if (vR > 0) return true;
            else return false;
        }

    }
}