using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Models.DuToanBS
{
    public class DuToanBSChungTuChiTietModels
    {
        public const String sLNSBaoDam = "1040100";
        public const int iMaTrangThaiDuyetKT = 106;

        #region Tính tổng còn lại sau phân cấp
        /// <summary>
        /// Tính tổng còn lại sau phân cấp
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="TruongTien"></param>
        /// <returns></returns>
        public static double TongConLai(String iID_MaChungTu, String TruongTien)
        {
            String SQL = "";
            SqlCommand cmd;
            double dTong;
            SQL = String.Format(@"SELECT SUM({0})  
                                FROM (
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
        #endregion

        #region Kiểm tra đơn vị tổng cục
        /// <summary>
        /// Kiểm tra có phải đơn vị tổng cục
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static bool KiemTraDonViPhanCap2Lan(String iID_MaDonVi)
        {
            string SQL = "";
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
        #endregion

        #region Xóa dòng chứng từ chi tiết
        /// <summary>
        /// Xóa dòng chứng từ chi tiết
        /// </summary>
        /// <param name="maChungTuChiTiet"></param>
        /// <param name="IPSua"></param>
        /// <param name="maND"></param>
        /// <returns></returns>
        public static int XoaChungTuChiTiet(String maChungTuChiTiet, String IPSua, String maND)
        {
            int vR = 0;
            try
            {
                //Xóa dữ liệu trong bảng DT_DotNganSach
                Bang bang = new Bang("DTBS_ChungTuChiTiet");
                bang.MaNguoiDungSua = maND;
                bang.IPSua = IPSua;
                bang.GiaTriKhoa = maChungTuChiTiet;
                bang.Delete();
                vR = 1;
            }
            catch
            {
                vR = 0;
            }
            return vR;
        } 
        #endregion

        #region Lấy dòng chứng từ chi tiết
        /// <summary>
        /// Lấy dòng chứng từ chi tiết
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable LayDongChungTuChiTiet(String iID_MaChungTu)
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
        #endregion

        #region Lấy chứng từ chi tiết của chứng từ
        /// <summary>
        /// Lấy chứng từ chi tiết của chứng từ.
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="dicGiaTriTimKiem"></param>
        /// <param name="maND"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable LayChungTuChiTiet(string maChungTu, Dictionary<string, string> dicGiaTriTimKiem, string maND, string sLNS)
        {
            DataTable dtResult;
            string sql;
            string dk = "";
            SqlCommand cmd;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(maND);

            cmd = new SqlCommand();
            dk = "iTrangThai=1";
            //Ngân sách 109
            if (sLNS == "109")
            {
                dk += " AND sLNS Like @LNS";
                cmd.Parameters.AddWithValue("@LNS", sLNS);
            }
            else
            {
                dk += " AND sLNS IN (" + sLNS + ")";
            }

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan,sGhiChu,sMaCongTrinh";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');
            //<--Lay toan bo Muc luc ngan sach
            if (dicGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        dk += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + dicGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }
            if (dt.Rows.Count > 0)
                dk += " AND( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                //dk += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                dk += "  (sLNS=@sLNS" + i + " OR sLNS=" + Nguon + " OR  sLNS=" + LoaiNS + ")";
                if (i < dt.Rows.Count - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
            }
            if (dt.Rows.Count > 0)
                dk += " ) ";
            sql = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2}  ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, dk, MucLucNganSachModels.strDSTruongSapXep);
            cmd.CommandText = sql;
            dtResult = Connection.GetDataTable(cmd);
            cmd.Dispose();

            //Lấy dữ liệu từ bảng DTBS_ChungTuChiTiet
            cmd = new SqlCommand();
            dk = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            if (dicGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(dicGiaTriTimKiem["iID_MaDonVi"]) == false)
                {
                    dk += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                    cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + dicGiaTriTimKiem["iID_MaDonVi"] + "%");
                }
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        dk += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + dicGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }
            dk += " AND (";
            for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
            {
                dk += arrDSTruongTien[i] + "<>0 OR ";
            }
            dk = dk.Substring(0, dk.Length - 3);
            dk += ") ";

            sql = String.Format("SELECT *,sTenDonVi as sTenDonVi_BaoDam FROM DTBS_ChungTuChiTiet WHERE {0} ORDER BY sXauNoiMa,iID_MaDonVi", dk);
            cmd.CommandText = sql;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;
            DataColumn column;

            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                column = new DataColumn(arrDSTruongTien[j], typeof(String));
                column.AllowDBNull = true;
                dtResult.Columns.Add(column);

            }
            int vRCount = dtResult.Rows.Count;
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    for (int k = 0; k < arrDSTruong.Length; k++)
                    {
                        if (Convert.ToString(dtResult.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < dtResult.Columns.Count - 1; k++)
                            {
                                if ((dtResult.Columns[k].ColumnName.StartsWith("b") == false && dtResult.Columns[k].ColumnName != "iID_MaMucLucNganSach_Cha") || dtResult.Columns[k].ColumnName == "bLaHangCha")
                                    dtResult.Rows[i][k] = dtChungTuChiTiet.Rows[j][dtResult.Columns[k].ColumnName];
                                else
                                    dtResult.Rows[i][k] = dtResult.Rows[i][k];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row = dtResult.NewRow();
                            for (int k = 0; k < dtResult.Columns.Count - 1; k++)
                            {
                                if ((dtResult.Columns[k].ColumnName.StartsWith("b") == false && dtResult.Columns[k].ColumnName != "iID_MaMucLucNganSach_Cha") || dtResult.Columns[k].ColumnName == "bLaHangCha")
                                    row[k] = dtChungTuChiTiet.Rows[j][dtResult.Columns[k].ColumnName];
                                else
                                {
                                    row[k] = dtResult.Rows[i][k];
                                }
                            }
                            dtResult.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }
            }
            dtResult.Columns.Add("bPhanCap", typeof(Boolean));
            dtChungTuChiTiet.Dispose();
            dtResult.Dispose();
            cmd.Dispose();
            return dtResult;
        } 
        #endregion

        #region Lấy chứng từ chi tiết phân cấp
        /// <summary>
        /// Lấy chứng từ chi tiết phân cấp
        /// </summary>
        /// <param name="maChungTu">Mã chứng từ</param>
        /// <param name="dicGiaTriTimKiem">Giá trị tìm kiếm</param>
        /// <param name="MaND">Mã ND</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="sXauNoiMa">Xâu nối mã</param>
        /// <param name="iKyThuat"></param>
        /// <param name="MaLoai"></param>
        /// <returns></returns>
        public static DataTable LayChungTuChiTietPhanCap(string maChungTu, Dictionary<string, string> dicGiaTriTimKiem, string MaND, string sLNS, string sXauNoiMa, string iKyThuat, string MaLoai)
        {
            DataTable vR;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            string sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan,sGhiChu,sMaCongTrinh";
            string[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            string[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            string[] arrDSTruongTien = sTruongTien.Split(',');

            String sql, dk;
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
                    dk = String.Format("iTrangThai=1 AND sLNS LIKE '{0}%' AND sXauNoiMa LIKE '%{1}%'", sLNS, sXauNoiMa);
                }
                else
                {
                    dk = String.Format("iTrangThai=1 AND sLNS LIKE '{0}%' AND sXauNoiMa LIKE '%{1}%'", sLNS, sXauNoiMa);
                }

                if (dicGiaTriTimKiem != null)
                {
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            dk += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + dicGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                    dk += " AND( ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                    String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                    dk += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                    if (i < dt.Rows.Count - 1)
                        dk += " OR ";
                    cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
                }
                if (dt.Rows.Count > 0)
                    dk += " ) ";
                sql = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2} ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, dk, MucLucNganSachModels.strDSTruongSapXep);
                cmd.CommandText = sql;
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();
                //Lay toan bo Muc luc ngan sach-->
                dk = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";

                if (dicGiaTriTimKiem != null)
                {
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem["iID_MaDonVi"]) == false)
                    {
                        dk += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                        cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + dicGiaTriTimKiem["iID_MaDonVi"] + "%");
                    }
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem["iID_MaPhongBanDich"]) == false)
                    {
                        dk += String.Format(" AND {0} LIKE @{0}", "iID_MaPhongBanDich");
                        cmd.Parameters.AddWithValue("@" + "iID_MaPhongBanDich", "%" + dicGiaTriTimKiem["iID_MaPhongBanDich"] + "%");
                    }
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            dk += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);

                        }
                    }
                }
                dk += " AND (";
                for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
                {
                    dk += arrDSTruongTien[i] + "<>0 OR ";
                }
                dk = dk.Substring(0, dk.Length - 3);
                dk += ") ";

                cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
                sql = String.Format(@"SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sTenCongTrinh,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi as sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan
                                        ,rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,sGhiChu
                                        FROM DTBS_ChungTuChiTiet_PhanCap
                                        WHERE  {0}

                                        UNION

                                        SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sTenCongTrinh, sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi as sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan
                                        ,rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,sGhiChu
                                        FROM DTBS_ChungTuChiTiet
                                        WHERE {0}

                                        ORDER BY sM,sTM,sTTM,sNG, iID_MaDonVi", dk);
                cmd.CommandText = sql;

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
                dk = String.Format("iTrangThai=1 AND sLNS LIKE '{0}%' AND sXauNoiMa LIKE '%{1}%'", sLNS, sXauNoiMa);
                if (dicGiaTriTimKiem != null)
                {
                    //if (arrGiaTriTimKiem["iID_MaDonVi"] != null)
                    //    DK += String.Format("iID_MaDonVi={0}", arrGiaTriTimKiem["iID_MaDonVi"]);
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            dk += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + dicGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                    dk += " AND( ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                    String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                    dk += "  (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                    if (i < dt.Rows.Count - 1)
                        dk += " OR ";
                    cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
                }
                if (dt.Rows.Count > 0)
                    dk += " ) ";
                sql = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2} ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, dk, MucLucNganSachModels.strDSTruongSapXep);
                cmd.CommandText = sql;
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();
                //Lay toan bo Muc luc ngan sach-->
                dk = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";

                if (dicGiaTriTimKiem != null)
                {
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem["iID_MaDonVi"]) == false)
                    {
                        dk += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                        cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + dicGiaTriTimKiem["iID_MaDonVi"] + "%");
                    }
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem["iID_MaPhongBanDich"]) == false)
                    {
                        dk += String.Format(" AND {0} LIKE @{0}", "iID_MaPhongBanDich");
                        cmd.Parameters.AddWithValue("@" + "iID_MaPhongBanDich", "%" + dicGiaTriTimKiem["iID_MaPhongBanDich"] + "%");
                    }
                    for (int i = 0; i < arrDSTruong.Length; i++)
                    {
                        if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                        {
                            dk += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);

                        }
                    }
                }
                dk += " AND (";
                for (int i = 1; i < arrDSTruongTien.Length - 8; i++)
                {
                    dk += arrDSTruongTien[i] + "<>0 OR ";
                }
                dk = dk.Substring(0, dk.Length - 3);
                dk += ") ";

                cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
                sql = String.Format("SELECT *,sTenDonVi as sTenDonVi_BaoDam  FROM DTBS_ChungTuChiTiet_PhanCap WHERE {0} ORDER BY sXauNoiMa,iID_MaDonVi", dk);
                cmd.CommandText = sql;

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
        #endregion

        #region Lấy chứng từ chi tiết lần phân cấp 2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="dicGiaTriTimKiem"></param>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable LayChungTuChiTietLan2(String maChungTu, Dictionary<string, string> dicGiaTriTimKiem, string maND, string sLNS)
        {
            DataTable vR, dt;
            String SQL, DK = "";
            SqlCommand cmd = new SqlCommand();
            SQL = String.Format(@"SELECT sTenDonVi as sTenDonVi_BaoDam,* FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTu");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            dt = Connection.GetDataTable(cmd);
            String iID_MaNganh = Convert.ToString(dt.Rows[0]["iID_MaDonVi"]);
            cmd = new SqlCommand();
            SQL = String.Format(@"SELECT sTenDonVi as sTenDonVi_BaoDam,* FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND MaLoai=2");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            vR = Connection.GetDataTable(cmd);
            //Nếu có chua co sẽ them 1 dong với MaLoai=2
            if (vR.Rows.Count == 0)
            {
                String iID_MaPhongBan = "";
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
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
                }
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
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
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
        #endregion

        #region Lấy tất cả chứng từ chi tiết của chứng từ
        /// <summary>
        /// Lấy tất cả chứng từ chi tiết của chứng từ
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuChiTiet(string maChungTu)
        {
            DataTable dt = null;
            string sql;
            SqlCommand cmd;

            sql = "SELECT * FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
            cmd.CommandText = sql;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        } 
        #endregion

        #region Lấy chứng từ chi tiết chi tập trung
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="dicGiaTriTimKiem"></param>
        /// <param name="maND"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable LayChungTuChiTietChiTapTrung(string maChungTu, Dictionary<string, string> dicGiaTriTimKiem, string maND, string sLNS)
        {

            DataTable vR, dtChungTu;
            String SQL, DK = "", Dk1 = "";
            SqlCommand cmd;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(maND);

            dtChungTu = DuToanBSChungTuModels.LayChungTu(maChungTu);
            DK = String.Format("iTrangThai=1 AND sLNS IN ({0}) ", sLNS);

            DataRow RChungTu = dtChungTu.Rows[0];

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sTenDonVi_BaoDam,iID_MaPhongBanDich,iID_MaPhongBan,sGhiChu,sMaCongTrinh";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruong_TK = (MucLucNganSachModels.strDSTruong + ",iID_MaDonVi").Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');



            //<--Lay toan bo Muc luc ngan sach
            cmd = new SqlCommand();


            if (dicGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "sLNS")
                        {
                            DK += String.Format(" AND sLNS IN ({0}) ", sLNS);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + dicGiaTriTimKiem[arrDSTruong[i]] + "%");
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
            cmd.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);


            if (dicGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(dicGiaTriTimKiem["iID_MaDonVi"]) == false)
                {
                    DK += String.Format(" AND {0} LIKE @{0}", "iID_MaDonVi");
                    cmd.Parameters.AddWithValue("@" + "iID_MaDonVi", "%" + dicGiaTriTimKiem["iID_MaDonVi"] + "%");
                }
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(dicGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "sLNS")
                        {
                            DK += String.Format(" AND sLNS IN ({0}) ", sLNS);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], "%" + dicGiaTriTimKiem[arrDSTruong[i]] + "%");
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
        #endregion

        #region Cập nhật chứng từ chi tiết
        /// <summary>
        /// Cập nhật chứng từ chi tiết
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="iLoai"></param>
        /// <param name="iChiTapTrung"></param>
        /// <param name="maND"></param>
        /// <param name="sXauDuLieuThayDoi"></param>
        /// <param name="ipAddress"></param>
        /// <param name="sMaMucLucNganSach"></param>
        /// <param name="sXauMaCacHang"></param>
        /// <param name="sXauCacHangDaXoa"></param>
        /// <param name="sXauMaCacCot"></param>
        /// <param name="sXauGiaTriChiTiet"></param>
        public static void CapNhatChungTuChiTiet(string maChungTu, string iLoai, string iChiTapTrung, string maND, string sXauDuLieuThayDoi, string ipAddress,
            string sMaMucLucNganSach, string sXauMaCacHang, string sXauCacHangDaXoa, string sXauMaCacCot, string sXauGiaTriChiTiet)
        {
            NameValueCollection data;
            if (iLoai == "4")
            {
                data = DuToanBSChungTuModels.LayThongTinChungTuKyThuatLan2(maChungTu);
            }
            else
            {
                data = DuToanBSChungTuModels.LayThongTinChungTu(maChungTu);
            }

            string TenBangChiTiet = "DTBS_ChungTuChiTiet";

            string[] arrMaMucLucNganSach = sMaMucLucNganSach.Split(',');
            string[] arrMaHang = sXauMaCacHang.Split(',');
            string[] arrHangDaXoa = sXauCacHangDaXoa.Split(',');
            string[] arrMaCot = sXauMaCacCot.Split(',');
            string[] arrHangGiaTri = sXauGiaTriChiTiet.Split(new string[] { CapPhat_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            string maChungTuChiTiet;

            //Luu cac hang sua
            string[] arrHangThayDoi = sXauDuLieuThayDoi.Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    maChungTuChiTiet = arrMaHang[i].Split('_')[0];

                    //Trường hợp delete.
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (maChungTuChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = maChungTuChiTiet;
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = maND;
                            bang.IPSua = ipAddress;
                            bang.Save();
                        }
                    }
                    else
                    {
                        string[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { DuToanBSBangDuLieu.DauCachO }, StringSplitOptions.None);
                        string[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { DuToanBSBangDuLieu.DauCachO }, StringSplitOptions.None);

                        // Kiểm tra thay đổi.
                        bool coThayDoi = false;
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                                {
                                    if (arrGiaTri[j] == "")
                                    {
                                        if (maChungTuChiTiet == "")
                                        {
                                            coThayDoi = false;
                                            break;
                                        }
                                        else
                                        {
                                            coThayDoi = true;
                                            break;
                                        }
                                    }
                                }
                                coThayDoi = true;
                            }
                        }
                        if (coThayDoi)
                        {
                            if (i == 1 && iLoai == "4")
                                TenBangChiTiet = "DTBS_ChungTuChiTiet_PhanCap";
                            else
                                TenBangChiTiet = "DTBS_ChungTuChiTiet";
                            Bang bang = new Bang(TenBangChiTiet);
                            if (maChungTuChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", maChungTu);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", data["sTenPhongBan"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguon", data["iID_MaNguon"]);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", data["iID_MaTrangThaiDuyet"]);
                                bang.CmdParams.Parameters.AddWithValue("@iKyThuat", data["iKyThuat"]);
                                bang.CmdParams.Parameters.AddWithValue("@dNgayCapPhat", data["dngayChungTu"]);
                                if (iChiTapTrung == "1")
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", data["iID_MaDonVi"]);
                                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", data["iID_MaDonVi"] + " - " + DonViModels.Get_TenDonVi(data["iID_MaDonVi"]));
                                }

                                string maMucLucNganSach = arrMaHang[i].Split('_')[1];
                                DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(maMucLucNganSach);
                                //Dien thong tin cua Muc luc ngan sach
                                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                                dtMucLuc.Dispose();
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = maChungTuChiTiet;
                                bang.DuLieuMoi = false;

                            }
                            if (iLoai == "4")
                                bang.CmdParams.Parameters.AddWithValue("@MaLoai", "2");
                            bang.MaNguoiDungSua = maND;
                            bang.IPSua = ipAddress;

                            //Them tham so
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {
                                if (arrThayDoi[j] == "1")
                                {
                                    if (arrMaCot[j].EndsWith("_ConLai") == false)
                                    {
                                        string Truong = "@" + arrMaCot[j];
                                        //doi lai ten truong
                                        if (arrMaCot[j] == "sTenDonVi_BaoDam")
                                        {
                                            Truong = "@sTenDonVi";
                                        }
                                        if (arrMaCot[j].StartsWith("b"))
                                        {
                                            //Nhap Kieu checkbox
                                            if (arrGiaTri[j] == "1")
                                            {
                                                bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                            }
                                            else
                                            {
                                                bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                            }
                                        }
                                        else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                        {
                                            //Nhap Kieu so
                                            if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                            {
                                                bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                            }
                                        }
                                        else
                                        {
                                            //Nhap kieu xau
                                            bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                        }
                                    }
                                }
                            }
                            if (!bang.CmdParams.Parameters.Contains("iID_MaPhongBanDich"))
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", data["iID_MaPhongBan"]);
                            bang.Save();
                        }
                    }
                }
            }
        }
        #endregion
    }
}