using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;

namespace VIETTEL.Models
{
    public class SanPham_DanhMucGiaModels
    {
        public static ArrayList LayDanhSachDanhMuc(String iID_MaSanPham, String iID_MaLoaiHinh, String MaDanhMucGiaCha, int Cap, ref int ThuTu)
        {
            String SQL = "";
            ArrayList ListChiTiet = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(MaDanhMucGiaCha)) MaDanhMucGiaCha = "0";
            if (String.IsNullOrEmpty(iID_MaLoaiHinh)) iID_MaLoaiHinh = "0";
            if (MaDanhMucGiaCha != null && MaDanhMucGiaCha != "")
                SQL = string.Format("SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND iID_MaDanhMucGia_Cha = '{0}' AND iID_MaLoaiHinh = '{1}'", MaDanhMucGiaCha, iID_MaLoaiHinh);
                SQL += " AND iID_MaSanPham ='" + iID_MaSanPham + "'";
            SQL += " AND iID_MaChiTietGia = 0 ORDER BY iSTT, iID_MaDanhMucGia";
            cmd.CommandText = SQL;
            //DataTable dt = CommonFunction.dtData(cmd, "", Trang, SoBanGhi);
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;
                String strDoanTrang = "";
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    Hashtable rowChiTiet = new Hashtable();
                    ArrayList listMucLucQuanSoCon = new ArrayList();
                    rowChiTiet.Add("Cap", Cap);
                    rowChiTiet.Add("tgThuTu", tgThuTu);
                    rowChiTiet.Add("iID_MaDanhMucGia", Row["iID_MaDanhMucGia"]);
                    rowChiTiet.Add("bLaHangCha", Row["bLaHangCha"]);
                    rowChiTiet.Add("sTen_DonVi", Row["sTen_DonVi"]);
                    int DuocXoa = 1;
                    if ((bool)Row["bTheoMau"]) DuocXoa = 0;
                    if (String.IsNullOrEmpty(Row["iID_MaVatTu"].ToString())== false && Row["iID_MaVatTu"].ToString().ToLower() != "dddddddd-dddd-dddd-dddd-dddddddddddd")
                    {
                        rowChiTiet.Add("iID_MaVatTu", Row["iID_MaVatTu"]);
                        if (CheckXoaDanhMuc(Convert.ToString(Row["iID_MaDanhMucGia"])) > 0) DuocXoa = 0;
                    }
                    rowChiTiet.Add("DuocXoa", DuocXoa);
                    rowChiTiet.Add("sKyHieu", strDoanTrang + Row["sKyHieu"].ToString());
                    rowChiTiet.Add("sTen",strDoanTrang + Row["sTen"].ToString());
                    listMucLucQuanSoCon = LayDanhSachDanhMuc(iID_MaSanPham,iID_MaLoaiHinh, Convert.ToString(Row["iID_MaDanhMucGia"]), Cap + 1, ref ThuTu);
                    if (listMucLucQuanSoCon.Count > 0){
                        Boolean coNhomCon = false;
                        foreach (Hashtable con in listMucLucQuanSoCon)
                        {
                            if (con["iID_MaVatTu"] == null)
                            {
                                coNhomCon = true;
                                break;
                            }
                        }
                        if (!coNhomCon)
                        {
                            rowChiTiet.Add("laCha", 1);
                        }else{
                            rowChiTiet.Add("laCha", 0);
                        }
                    }else{
                        rowChiTiet.Add("laCha", -1);
                    }
                    ListChiTiet.Add(rowChiTiet);
                    if (listMucLucQuanSoCon.Count > 0) ListChiTiet.AddRange(listMucLucQuanSoCon);
                }
            }
            dt.Dispose();
            return ListChiTiet;
        }
        public static void CapNhatSttDanhMucGia(String iID_MaSanPham, String iID_MaChiTietGia, String MaDanhMucGiaCha, int Cap, ref int ThuTu)
        {
            String SQL = "";
            ArrayList ListChiTiet = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            if (MaDanhMucGiaCha != null && MaDanhMucGiaCha != "")
            {
                SQL = string.Format(@"SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND iID_MaDanhMucGia_Cha = '{0}'
                                      AND iID_MaSanPham = '{1}' AND iID_MaChiTietGia = '{2}'", MaDanhMucGiaCha, iID_MaSanPham, iID_MaChiTietGia);
            }
            else
            {
                SQL = string.Format(@"SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND iID_MaDanhMucGia_Cha = 0
                                AND iID_MaSanPham = '{0}' AND iID_MaChiTietGia = '{1}'", iID_MaSanPham, iID_MaChiTietGia);
            }
            SQL += " ORDER BY iSTT, iID_MaDanhMucGia";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(SQL);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;
                String strDoanTrang = "";
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String updateStr = "UPDATE DM_SanPham_DanhMucGia";
                    updateStr += " SET iSTT =" + tgThuTu;
                    updateStr += " WHERE iID_MaDanhMucGia =" + Convert.ToString(Row["iID_MaDanhMucGia"]);
                    SqlCommand updateCmd = new SqlCommand(updateStr);
                    Connection.UpdateDatabase(updateCmd);
                    updateCmd.Dispose();
                    CapNhatSttDanhMucGia(iID_MaSanPham, iID_MaChiTietGia, Convert.ToString(Row["iID_MaDanhMucGia"]), Cap + 1, ref ThuTu);
                }
            }
            dt.Dispose();
        }
        public static void CopyDanhMucTheoCauHinh(String iID_MaSanPham,String iID_MaLoaiHinh, String iID_MaChiTietGia, String MaDanhMucGiaCha,String iID_MaDanhMucGia_Cha_Moi, String MaNguoiDungSua, String IPSua, int Cap, ref int ThuTu)
        {
            String SQL = "";
            ArrayList ListChiTiet = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(MaDanhMucGiaCha)) MaDanhMucGiaCha = "0";
            SQL = string.Format(@"SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND iID_MaChiTietGia = 0
                                AND iID_MaDanhMucGia_Cha = '{0}' AND iID_MaSanPham = '{1}' AND iID_MaLoaiHinh = '{2}' ", MaDanhMucGiaCha, iID_MaSanPham, iID_MaLoaiHinh);
            SQL += " ORDER BY iSTT, iID_MaDanhMucGia";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(SQL);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;
                String strDoanTrang = "";
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    Bang bangDM = new Bang("DM_SanPham_DanhMucGia");
                    bangDM.MaNguoiDungSua = MaNguoiDungSua;
                    bangDM.IPSua = IPSua;
                    bangDM.DuLieuMoi = true;
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                    bangDM.CmdParams.Parameters.AddWithValue("@iSTT", tgThuTu);
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha_Moi);
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_CauHinh",Row["iID_MaDanhMucGia"]);
                    if (Row["iID_MaVatTu"].ToString() == "dddddddd-dddd-dddd-dddd-dddddddddddd")
                    {
                        bangDM.CmdParams.Parameters.AddWithValue("@rDonGia_DangThucHien", 0);
                    }else {
                        DataTable dtGia = SanPham_VatTuModels.Get_GiaVatTu_Row(Row["iID_MaVatTu"].ToString());
                        String GiaVatTu = "0";
                        if (dtGia.Rows.Count > 0)
                        {
                            if (!Convert.ToBoolean(Row["bNganSach"]))
                            {
                                GiaVatTu = dtGia.Rows[0]["rGia"].ToString();
                            }
                            else
                            {
                                GiaVatTu = dtGia.Rows[0]["rGia_NS"].ToString();
                            }
                        }
                        if (String.IsNullOrEmpty(GiaVatTu)) GiaVatTu = "0";
                        bangDM.CmdParams.Parameters.AddWithValue("@rDonGia_DangThucHien", GiaVatTu);
                        bangDM.CmdParams.Parameters.AddWithValue("@rDonGia_DV_DeNghi", GiaVatTu);
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String TenTruong = dt.Columns[j].ColumnName;
                        if (TenTruong != "iID_MaDanhMucGia" && TenTruong != "iID_MaDanhMucGia_Cha" && TenTruong != "iID_MaChiTietGia" 
                            && TenTruong != "dNgayTao" && TenTruong != "iSTT" && TenTruong != "iID_MaLoaiHinh" 
                            && TenTruong != "rDonGia_DangThucHien" && TenTruong != "rDonGia_DV_DeNghi")
                        {
                            bangDM.CmdParams.Parameters.AddWithValue("@" + TenTruong, Row[j]);
                        }
                    }
                    bangDM.Save();
                    CopyDanhMucTheoCauHinh(iID_MaSanPham,iID_MaLoaiHinh, iID_MaChiTietGia, Convert.ToString(Row["iID_MaDanhMucGia"]), Get_MaxId_DanhMucGia(), MaNguoiDungSua, IPSua, Cap + 1, ref ThuTu);
                }
            }
            dt.Dispose();
        }
        public static void CopyDanhMucTheoPhieu(String iID_LoaiDonVi, String iID_MaChiTietGia_Cu, String iID_MaChiTietGia, String MaDanhMucGiaCha, String iID_MaDanhMucGia_Cha_Moi, String MaNguoiDungSua, String IPSua, int Cap, ref int ThuTu)
        {
            String SQL = "";
            ArrayList ListChiTiet = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(MaDanhMucGiaCha)) MaDanhMucGiaCha = "0";
            SQL = string.Format(@"SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 
                                AND iID_MaDanhMucGia_Cha = '{0}' AND iID_MaChiTietGia = '{1}' ", MaDanhMucGiaCha, iID_MaChiTietGia_Cu);
            SQL += " ORDER BY iSTT, iID_MaDanhMucGia";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(SQL);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;
                String strDoanTrang = "";
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    Bang bangDM = new Bang("DM_SanPham_DanhMucGia");
                    bangDM.MaNguoiDungSua = MaNguoiDungSua;
                    bangDM.IPSua = IPSua;
                    bangDM.DuLieuMoi = true;
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                    bangDM.CmdParams.Parameters.AddWithValue("@iSTT", tgThuTu);
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha_Moi);
                    String strTenTruongCu = "",strTenTruongMoi = "";
                    switch (iID_LoaiDonVi)
                    {
                        case "2":
                            strTenTruongCu = "rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi";
                            strTenTruongMoi = "rSoLuong_DatHang_DeNghi,rDonGia_DatHang_DeNghi,rTien_DatHang_DeNghi";
                            break;
                        case "3":
                            strTenTruongCu = "rSoLuong_DatHang_DeNghi,rDonGia_DatHang_DeNghi,rTien_DatHang_DeNghi";
                            strTenTruongMoi = "rSoLuong_CTC_DeNghi,rDonGia_CTC_DeNghi,rTien_CTC_DeNghi";
                            break;
                    }
                    String[] arrTenTruongCu = strTenTruongCu.Split(',');
                    String[] arrTenTruongMoi = strTenTruongMoi.Split(',');
                    for (int k = 0; k < arrTenTruongCu.Length; k++)
                    {
                        bangDM.CmdParams.Parameters.AddWithValue("@" + arrTenTruongMoi[k], Row[arrTenTruongCu[k]]);
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String TenTruong = dt.Columns[j].ColumnName;
                        if (TenTruong != "iID_MaDanhMucGia" && TenTruong != "dNgayTao" && bangDM.CmdParams.Parameters.IndexOf("@" + TenTruong) < 0)
                        {
                            bangDM.CmdParams.Parameters.AddWithValue("@" + TenTruong, Row[j]);
                        }
                    }
                    bangDM.Save();
                    CopyDanhMucTheoPhieu(iID_LoaiDonVi, iID_MaChiTietGia_Cu, iID_MaChiTietGia, Convert.ToString(Row["iID_MaDanhMucGia"]), Get_MaxId_DanhMucGia(), MaNguoiDungSua, IPSua, Cap + 1, ref ThuTu);
                }
            }
            dt.Dispose();
        }
        public static void CopyDanhMucTheoPhieu2(String iID_LoaiDonVi, String iID_MaChiTietGia_Cu, String iID_MaChiTietGia, String MaDanhMucGiaCha, String iID_MaDanhMucGia_Cha_Moi, String MaNguoiDungSua, String IPSua, int Cap, ref int ThuTu)
        {
            String SQL = "";
            ArrayList ListChiTiet = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(MaDanhMucGiaCha)) MaDanhMucGiaCha = "0";
            SQL = string.Format(@"SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 
                                AND iID_MaDanhMucGia_Cha = '{0}' AND iID_MaChiTietGia = '{1}' ", MaDanhMucGiaCha, iID_MaChiTietGia_Cu);
            SQL += " ORDER BY iSTT, iID_MaDanhMucGia";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(SQL);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;
                String strDoanTrang = "";
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    Bang bangDM = new Bang("DM_SanPham_DanhMucGia");
                    bangDM.MaNguoiDungSua = MaNguoiDungSua;
                    bangDM.IPSua = IPSua;
                    bangDM.DuLieuMoi = true;
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                    bangDM.CmdParams.Parameters.AddWithValue("@iSTT", tgThuTu);
                    bangDM.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha_Moi);
                    String strTenTruongCu = "";
                    String strTenTruongMoi = "rSoLuong_DangThucHien,rDonGia_DangThucHien,rTien_DangThucHien,rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi";
                    switch (iID_LoaiDonVi)
                    {
                        case "1":
                            strTenTruongCu = "rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi";
                            break;
                        case "2":
                            strTenTruongCu = "rSoLuong_DatHang_DeNghi,rDonGia_DatHang_DeNghi,rTien_DatHang_DeNghi";
                            break;
                        case "3":
                            strTenTruongCu = "rSoLuong_CTC_DeNghi,rDonGia_CTC_DeNghi,rTien_CTC_DeNghi";
                            break;
                    }
                    String[] arrTenTruongCu = strTenTruongCu.Split(',');
                    String[] arrTenTruongMoi = strTenTruongMoi.Split(',');
                    for (int k = 0; k < arrTenTruongMoi.Length; k++)
                    {
                        bangDM.CmdParams.Parameters.AddWithValue("@" + arrTenTruongMoi[k], Row[arrTenTruongCu[k%3]]);
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String TenTruong = dt.Columns[j].ColumnName;
                        if (TenTruong != "iID_MaDanhMucGia" && TenTruong != "dNgayTao" && bangDM.CmdParams.Parameters.IndexOf("@" + TenTruong) < 0)
                        {
                            bangDM.CmdParams.Parameters.AddWithValue("@" + TenTruong, Row[j]);
                        }
                    }
                    bangDM.Save();
                    CopyDanhMucTheoPhieu2(iID_LoaiDonVi, iID_MaChiTietGia_Cu, iID_MaChiTietGia, Convert.ToString(Row["iID_MaDanhMucGia"]), Get_MaxId_DanhMucGia(), MaNguoiDungSua, IPSua, Cap + 1, ref ThuTu);
                }
            }
            dt.Dispose();
        }
        public static DataTable Get_ChiTietDanhMucGia_Row(String iID_MaDanhMucGia)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND iID_MaDanhMucGia=@iID_MaDanhMucGia");
            cmd.Parameters.AddWithValue("@iID_MaDanhMucGia", iID_MaDanhMucGia);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            return vR;
        }
        public static DataTable Get_dtDanhMucGiaChiTiet(String iID_MaSanPham, String iID_MaChiTietGia, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            DK = "iTrangThai=1 AND iID_MaSanPham=@iID_MaSanPham AND iID_MaChiTietGia=@iID_MaChiTietGia";
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = SanPham_ChiTietGiaModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }
            SQL = String.Format("SELECT * FROM DM_SanPham_DanhMucGia WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static int Get_So_KyHieuChiTieu(String sKyHieu)
        {
            int vR = 0;
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND sKyHieu=@sKyHieu");
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }
        public static Boolean Delete(String iID_MaDanhMucGia)
        {
            Boolean vR = false;
            Bang bang = new Bang("DM_SanPham_DanhMucGia");
            bang.GiaTriKhoa = iID_MaDanhMucGia;
            bang.Delete();
            SqlCommand cmd = new SqlCommand("UPDATE DM_SanPham_DanhMucGia SET iTrangThai = 0 FROM DM_SanPham_DanhMucGia WHERE iTrangThai=1 AND iID_MaDanhMucGia_CauHinh=@iID_MaDanhMucGia");
            cmd.Parameters.AddWithValue("@iID_MaDanhMucGia", iID_MaDanhMucGia);
            Connection.UpdateDatabase(cmd);
            vR = true;
            return vR;
        }
        public static DataTable DT_MucLucChiTieu(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai = 1 AND iLoai=@iLoai ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            return vR;
        }
        public static DataTable Get_MucLucChiTieu()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM DM_SanPham_DanhMucGia WHERE iTrangThai = 1 ORDER BY iSTT";
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_ChiTietGia(String iID_MaChiTietGia)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 DM_SanPham_ChiTietGia.* FROM DM_SanPham_ChiTietGia WHERE iTrangThai=1 AND iID_MaChiTietGia=@iID_MaChiTietGia");
            cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChiTietGia(String iID_MaSanPham, String iID_LoaiDonVi, String iID_MaDonVi, String iID_MaLoaiHinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "iTrangThai = 1 AND iID_MaSanPham IN (SELECT DM_SanPham.iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1)";
            if (!String.IsNullOrEmpty(iID_MaSanPham))
            {
                DK += " AND iID_MaSanPham = @iID_MaSanPham ";
                cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            }
            if (!String.IsNullOrEmpty(iID_LoaiDonVi))
            {
                DK += " AND iID_LoaiDonVi = @iID_LoaiDonVi ";
                cmd.Parameters.AddWithValue("@iID_LoaiDonVi", iID_LoaiDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaLoaiHinh))
            {
                DK += " AND iID_MaLoaiHinh = @iID_MaLoaiHinh ";
                cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
            }
            String SQL = String.Format("SELECT * FROM DM_SanPham_ChiTietGia WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChiTietGia(String MaND, String iID_MaSanPham, String iID_LoaiDonVi, String iID_MaDonVi, String iID_MaLoaiHinh, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "iTrangThai = 1 AND iID_MaSanPham IN (SELECT DM_SanPham.iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1)";
            if (!String.IsNullOrEmpty(iID_MaSanPham))
            {
                DK += " AND iID_MaSanPham = @iID_MaSanPham ";
                cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            }
            if (!String.IsNullOrEmpty(iID_LoaiDonVi))
            {
                DK += " AND iID_LoaiDonVi = @iID_LoaiDonVi ";
                cmd.Parameters.AddWithValue("@iID_LoaiDonVi", iID_LoaiDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaLoaiHinh))
            {
                DK += " AND iID_MaLoaiHinh = @iID_MaLoaiHinh ";
                cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
            }
            String SQL = String.Format("SELECT * FROM DM_SanPham_ChiTietGia WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int Get_DanhSachChiTietGia_Count(String MaND, String iID_MaSanPham, String iID_LoaiDonVi, String iID_MaDonVi, String iID_MaLoaiHinh)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iID_MaSanPham IN (SELECT DM_SanPham.iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1)";
            if (!String.IsNullOrEmpty(iID_MaSanPham))
            {
                DK += " AND iID_MaSanPham = @iID_MaSanPham ";
                cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            }
            if (!String.IsNullOrEmpty(iID_LoaiDonVi))
            {
                DK += " AND iID_LoaiDonVi = @iID_LoaiDonVi ";
                cmd.Parameters.AddWithValue("@iID_LoaiDonVi", iID_LoaiDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(iID_MaLoaiHinh))
            {
                DK += " AND iID_MaLoaiHinh = @iID_MaLoaiHinh ";
                cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
            }
            String SQL = String.Format("SELECT Count(*) FROM DM_SanPham_ChiTietGia WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static String Get_MaxId_ChiTietGia()
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL ="SELECT MAX(iID_MaChiTietGia) AS maxId FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1";
            cmd.CommandText = SQL;
            vR = Connection.GetValueString(cmd,"0");
            cmd.Dispose();
            return vR;
        }
        public static String Get_MaxId_DanhMucGia()
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iID_MaDanhMucGia) AS maxId FROM DM_SanPham_DanhMucGia WHERE iTrangThai = 1";
            cmd.CommandText = SQL;
            vR = Connection.GetValueString(cmd, "0");
            cmd.Dispose();
            return vR;
        }
        private static Decimal CheckXoaDanhMuc(String iID_MaDanhMucGia)
        {
            SqlCommand cmd = new SqlCommand();
            String querry = String.Format(@"SELECT (SUM (rTien_DangThucHien) + SUM (rTien_DV_DeNghi)+ SUM (rTien_DatHang_DeNghi)+ SUM (rTien_CTC_DeNghi)) AS tong
                                            FROM DM_SanPham_DanhMucGia WHERE iTrangThai = 1 AND iID_MaChiTietGia > 0 AND iID_MaDanhMucGia_CauHinh = @iID_MaDanhMucGia
                                            AND iID_MaSanPham IN (SELECT iID_MaSanPham FROM DM_SanPham WHERE  iTrangThai = 1)
                                            AND iID_MaChiTietGia IN (SELECT iID_MaChiTietGia FROM DM_SanPham_ChiTietGia WHERE  iTrangThai = 1)");
            cmd.Parameters.AddWithValue("@iID_MaDanhMucGia", iID_MaDanhMucGia);
            cmd.CommandText = querry;
            return Convert.ToDecimal(Connection.GetValue(cmd,-1));
        }
    }
}