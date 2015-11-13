using System;
using System.Data;
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
    public class TCDN_ChiTieuThuNopModels
    {
        public static DataTable DT_MucLucChiTieu(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iLoai=@iLoai AND iTrangThai = 1 ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            vR.Columns.Add("sMaSoBangCanDoi");
            for (int i = 0; i < vR.Rows.Count; i++)
            {
                cmd = new SqlCommand("SELECT iID_MaChiTieu FROM TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo");
                cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", Convert.ToString(vR.Rows[i]["iID_MaChiTieuHoSo"]));
                String iID_MaChiTieu = Convert.ToString(Connection.GetValue(cmd, ""));
                cmd.Dispose();

                cmd = new SqlCommand("SELECT sKyHieu + ' - ' + sTen FROM TCDN_ChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu");
                cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
                String sChuoiChiTieu = Convert.ToString(Connection.GetValue(cmd, ""));
                cmd.Dispose();

                vR.Rows[i]["sMaSoBangCanDoi"] = sChuoiChiTieu;
            }
            return vR;
        }
        private static DataTable LietKeGiaTriCuaHoSo_Nam(String iID_MaDoanhNghiep, String iQuy, String iNam)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_ChiTieuThuNop WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static void ThemChiTiet(String iID_MaDoanhNghiep, String iQuy, String iNam, String MaND, String IPSua)
        {
            String iLoai = "0";
            DataTable dt = DT_MucLucChiTieu(iLoai);
            int i, j;
            Bang bang = new Bang("TCDN_ChiTieuThuNop");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);

            //Update trường số liệu năm trước cho bảng
            Int32 iNamTruoc = Convert.ToInt32(iNam);
            Int32 iQuyTruoc = Convert.ToInt32(iQuy) - 1;
            if (iQuy == "1")
            {
                iQuyTruoc = 4;
                iNamTruoc = Convert.ToInt32(iNam) - 1;
            }
            DataTable dtDauKy = LietKeGiaTriCuaHoSo_Nam(iID_MaDoanhNghiep, Convert.ToString(iQuyTruoc), Convert.ToString(iNamTruoc));                        
            for (i = 0; i < dt.Rows.Count; i++)
            {
                //Thêm thông tin vào bảng TCDN_ChiTieuThuNop
                ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(dt.Rows[i], dtDauKy, bang.CmdParams.Parameters);
                bang.Save();
            }

            dt.Dispose();

            //Đánh STT cho bảng TCDN_ChiTieuThuNop vừa insert
            DataTable dtChiTieu = TCDN_HoSoDoanhNghiep_ChiTieuModels.Get_MucLucChiTieu(iLoai);
            DataTable dtChungTuChiTiet = Get_dtChungTuChiTiet_TheoChungTu(iID_MaDoanhNghiep,iQuy,iNam);
            int STT = 0;
            DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, iID_MaDoanhNghiep, 0, iQuy,iNam, ref STT);
            dtChiTieu.Dispose();
            dtChungTuChiTiet.Dispose();




            dtDauKy.Dispose();
        }

        public static void ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(DataRow RMucLucChiTieu, DataTable dtDauKy, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "sKyHieu,sTen,sMaSoBangCanDoi";
            String[] arrDSTruong = strDSTruong.Split(',');
            if (Params.IndexOf("@iID_MaChiTieuHoSo") >= 0)
            {
                Params["@iID_MaChiTieuHoSo"].Value = RMucLucChiTieu["iID_MaChiTieuHoSo"];
                Params["@iID_MaChiTieuHoSo_Cha"].Value = RMucLucChiTieu["iID_MaChiTieuHoSo_Cha"];
                Params["@bLaHangCha"].Value = RMucLucChiTieu["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaChiTieuHoSo", RMucLucChiTieu["iID_MaChiTieuHoSo"]);
                Params.AddWithValue("@iID_MaChiTieuHoSo_Cha", RMucLucChiTieu["iID_MaChiTieuHoSo_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucChiTieu["bLaHangCha"]);
            }
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucChiTieu[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucChiTieu[arrDSTruong[i]]);
                }
            }

            //Lấy dữ liệu từ quý trước
            String iID_MaChiTieu = Convert.ToString(RMucLucChiTieu["iID_MaChiTieuHoSo"]);
            String iID_MaChiTieu1 = "";
            for (int i = 0; i < dtDauKy.Rows.Count; i++)
            {
                iID_MaChiTieu1 = Convert.ToString(dtDauKy.Rows[i]["iID_MaChiTieuHoSo"]);
                if (iID_MaChiTieu.Equals(iID_MaChiTieu1))
                {
                    if (Params.IndexOf("@rKyTruocChuyenSang") >= 0)
                    {
                        Params["@rKyTruocChuyenSang"].Value = dtDauKy.Rows[i]["rSoConNoCuoiKy"];
                    }
                    else
                    {
                        Params.AddWithValue("@rKyTruocChuyenSang", dtDauKy.Rows[i]["rSoConNoCuoiKy"]);
                    }
                    dtDauKy.Rows.RemoveAt(i);
                    break;
                }
            }
            //End Lấy dữ liệu từ quý trước
        }
        private static void DanhSTTChoCay(DataTable dtChiTieu, DataTable dtChungTuChiTiet, String MaChungTu, int MaHangMauCha, String iQuy, String iNam, ref int STT)
        {
            int i, j, MaChiTieu;
            SqlCommand cmd;
            for (i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                if (MaHangMauCha == Convert.ToInt32(dtChiTieu.Rows[i]["iID_MaChiTieuHoSo_Cha"]))
                {
                    MaChiTieu = Convert.ToInt32(dtChiTieu.Rows[i]["iID_MaChiTieuHoSo"]);
                    for (j = 0; j < dtChungTuChiTiet.Rows.Count; j++)
                    {
                        if (MaChiTieu == Convert.ToInt32(dtChungTuChiTiet.Rows[j]["iID_MaChiTieuHoSo"]))
                        {
                            STT++;
                            cmd = new SqlCommand();
                            cmd.CommandText = String.Format("UPDATE TCDN_ChiTieuThuNop " +
                                                            "SET iSTT=@iSTT " +
                                                            "WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND " +
                                                                  "iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec");
                            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", MaChungTu);
                            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", MaChiTieu);
                            cmd.Parameters.AddWithValue("@iQuy", iQuy);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            cmd.Parameters.AddWithValue("@iSTT", STT);
                            Connection.UpdateDatabase(cmd);
                        }
                    }
                    DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, MaChungTu, MaChiTieu, iQuy, iNam, ref STT);
                }
            }
        }
        public static DataTable Get_dtChungTuChiTiet(String iID_MaDoanhNghiep, String iQuy, String iNam, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = "sTen";
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

            SQL = String.Format("SELECT * FROM TCDN_ChiTieuThuNop WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_dtChungTuChiTiet_TheoChungTu(String iID_MaDoanhNghiep, String iQuy, String iNam)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);

            SQL = String.Format("SELECT * FROM TCDN_ChiTieuThuNop WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_dtBaoCaoChiTiet(String iNam, String iQuy, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);

            SQL = String.Format("SELECT * FROM TCDN_ChiTieuThuNop WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static Boolean CheckData(String iQuy, String iNam, String iID_MaDoanhNghiep)
        {

            Boolean vR = false;

            SqlCommand cmd;
            cmd = new SqlCommand("SELECT COUNT(*) FROM TCDN_ChiTieuThuNop WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            Int32 iCount = Convert.ToInt32(Connection.GetValue(cmd, 0));

            if (iCount > 0)
            {
                vR = true;
            }
            return vR;
        }
        public static DataTable DT_DanhMucDoanhNghiep(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_DoanhNghiep WHERE iTrangThai = 1 AND iID_MaLoaiDoanhNghiep=@iID_MaLoaiDoanhNghiep ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}