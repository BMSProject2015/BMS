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
    public class TCDN_ChiTieuTaiChinhModels
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
            for (int i = 0; i < vR.Rows.Count; i++) { 
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
        public static void ThemChiTiet(String iID_MaDoanhNghiep, String iQuy, String iNam, String MaND, String IPSua)
        {
            String iLoai = "1";
            DataTable dt = DT_MucLucChiTieu(iLoai);
            int i, j;
            Bang bang = new Bang("TCDN_ChiTieuTaiChinh");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);


            int Nam =Convert.ToInt16(iNam);
            int Quy = Convert.ToInt16(iQuy);
            if (iQuy == "1")
            {
                Nam = Convert.ToInt16(iNam) - 1;
                Quy = 4;
            }
            else
            {
                Quy = Convert.ToInt16(iQuy) - 1;
            }
            //lấy dữ liệu quý trước
            DataTable dtDauKy = Get_dtQuyNam(Nam, Quy, iID_MaDoanhNghiep);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //Thêm thông tin vào bảng TCDN_ChiTieuTaiChinh
                ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(dt.Rows[i],dtDauKy, bang.CmdParams.Parameters);
                bang.Save();
            }

            dt.Dispose();

            //Đánh STT cho bảng TCDN_ChiTieuTaiChinh vừa insert
            DataTable dtChiTieu = TCDN_HoSoDoanhNghiep_ChiTieuModels.Get_MucLucChiTieu(iLoai);
            DataTable dtChungTuChiTiet = Get_dtChungTuChiTiet_TheoChungTu(iID_MaDoanhNghiep, iQuy, iNam);
            int STT = 0;
            DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, iID_MaDoanhNghiep, 0, iQuy, iNam, ref STT);
            dtChiTieu.Dispose();
            dtChungTuChiTiet.Dispose();

            //Update trường số liệu từ chi tiêu cho bảng
            LaySoLieuCanDoiTaiKhoanVaKinhDoanh(iID_MaDoanhNghiep, iNam, iQuy);
        }

        public static void ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(DataRow RMucLucChiTieu,DataTable dtDauKy, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "sKyHieu,sTen,sMaSoBangCanDoi,bTinhTong";
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
                if (arrDSTruong[i] == "sTen" && Convert.ToInt32(RMucLucChiTieu["iID_MaChiTieuHoSo_Cha"]) > 0)
                {
                    if (Convert.ToInt32(RMucLucChiTieu["bLaHangCha"])>0)
                        RMucLucChiTieu[arrDSTruong[i]] = Convert.ToString(RMucLucChiTieu[arrDSTruong[i]]);
                    else
                    {
                        RMucLucChiTieu[arrDSTruong[i]] = Convert.ToString(RMucLucChiTieu[arrDSTruong[i]]);
                    }
                }
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
                    if (Params.IndexOf("@rSoDauNam") >= 0)
                    {
                        Params["@rSoDauNam"].Value = dtDauKy.Rows[i]["rSoCuoiKy"];
                    }
                    else
                    {
                        Params.AddWithValue("@rSoDauNam", dtDauKy.Rows[i]["rSoCuoiKy"]);
                    }
                    dtDauKy.Rows.RemoveAt(i);
                    break;
                }
            }
            //End Lấy dữ liệu từ quý trước

        }

        public static DataTable Get_dtQuyNam(int iNamLamViec, int iQuy, String iID_MaDoanhNghiep)
        {
            String SQL = "SELECT * FROM TCDN_ChiTieuTaiChinh WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
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
                            cmd.CommandText = String.Format("UPDATE TCDN_ChiTieuTaiChinh " +
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
                    DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, MaChungTu, MaChiTieu,iQuy, iNam, ref STT);
                }
            }
        }
        public static void LaySoLieuCanDoiTaiKhoanVaKinhDoanh(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            int i,j;
            DataTable dtNamHienTai = LietKeGiaTriCuaChiTieuTaiChinh_Nam(iID_MaDoanhNghiep, iNam, iQuy);
            DataRow R, R1;
            for (i = 0; i < dtNamHienTai.Rows.Count; i++)
            {
                R = dtNamHienTai.Rows[i];
                Double rSoCuoiNam = 0, rSoDauNam = 0;
                DataTable dtMap = LayChiTieuMapHoSoVaCanDoi(Convert.ToString(R["iID_MaChiTieuHoSo"]));
                if (dtMap.Rows.Count > 0)
                {
                    for (j = 0; j < dtMap.Rows.Count; j++)
                    {
                        int iLoaiChiTieu = TCDN_ChiTieuModels.CheckLoaiChiTieu(Convert.ToString(dtMap.Rows[j]["iID_MaChiTieu"]));
                        if (iLoaiChiTieu == 2)
                        {
                            DataTable dtValues = LayGiaTriBangCanDoiKinhDoanh(Convert.ToString(dtMap.Rows[j]["iID_MaChiTieu"]), iID_MaDoanhNghiep, iNam, iQuy);
                            if (Convert.ToString(dtValues.Rows[0]["rNamNay"]) != null && Convert.ToString(dtValues.Rows[0]["rNamNay"]) != "")
                            {
                                rSoCuoiNam += Convert.ToDouble(dtValues.Rows[0]["rNamNay"]);
                            }
                            if (Convert.ToString(dtValues.Rows[0]["rNamTruoc"]) != null && Convert.ToString(dtValues.Rows[0]["rNamTruoc"]) != "")
                            {
                                rSoDauNam += Convert.ToDouble(dtValues.Rows[0]["rNamTruoc"]);
                            }
                            dtValues.Dispose();
                        }
                        else {
                            DataTable dtValues = LayGiaTriBangCanDoi(Convert.ToString(dtMap.Rows[j]["iID_MaChiTieu"]), iID_MaDoanhNghiep, iNam, iQuy);
                            if (Convert.ToString(dtValues.Rows[0]["rSoCuoiNam"]) != null && Convert.ToString(dtValues.Rows[0]["rSoCuoiNam"]) != "")
                            {
                                rSoCuoiNam += Convert.ToDouble(dtValues.Rows[0]["rSoCuoiNam"]);
                            }
                            if (Convert.ToString(dtValues.Rows[0]["rSoDauNam"]) != null && Convert.ToString(dtValues.Rows[0]["rSoDauNam"]) != "")
                            {
                                rSoDauNam += Convert.ToDouble(dtValues.Rows[0]["rSoDauNam"]);
                            }
                            dtValues.Dispose();
                        }
                    }
                }
                dtMap.Dispose();

                String SQL = "UPDATE TCDN_ChiTieuTaiChinh SET rSoDauNam=@rSoDauNam, rSoCuoiKy=@rSoCuoiKy WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep " +
                        "AND iNamLamViec=@iNamLamViec AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND iQuy=@iQuy";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@rSoDauNam", rSoDauNam);
                cmd.Parameters.AddWithValue("@rSoCuoiKy", rSoCuoiNam);
                cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", Convert.ToString(R["iID_MaChiTieuHoSo"]));
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            dtNamHienTai.Dispose();
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
                String DSTruong = "sTen,sMaSoBangCanDoi";
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

            SQL = String.Format("SELECT * FROM TCDN_ChiTieuTaiChinh WHERE {0} ORDER BY iSTT", DK);
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

            SQL = String.Format("SELECT * FROM TCDN_ChiTieuTaiChinh WHERE {0} ORDER BY iSTT", DK);
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

            SQL = String.Format("SELECT * FROM TCDN_ChiTieuTaiChinh WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static Boolean CheckData(String iQuy, String iNam, String iID_MaDoanhNghiep)
        {
            Boolean vR = false;

            SqlCommand cmd;
            cmd = new SqlCommand("SELECT COUNT(*) FROM TCDN_ChiTieuTaiChinh WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
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
        private static DataTable LietKeGiaTriCuaChiTieuTaiChinh_Nam(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_ChiTieuTaiChinh WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable LayChiTieuMapHoSoVaCanDoi(String iID_MaChiTieuHoSo)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable LayGiaTriBangCanDoi(String iID_MaChiTieu, String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT SUM(rSoCuoiNam) AS rSoCuoiNam, SUM(rSoDauNam) AS rSoDauNam FROM TCDN_ChungTuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable LayGiaTriBangCanDoiKinhDoanh(String iID_MaChiTieu, String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT SUM(rNamNay) AS rNamNay, SUM(rNamTruoc) AS rNamTruoc FROM TCDN_KinhDoanh_ChungTuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}