using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
namespace VIETTEL.Models
{
    public class TCDN_KinhDoanh_ChungTuChiTietModels
    {
        public static void ThemChiTiet(String iID_MaChungTu, String iLoai, String MaND, String IPSua)
        {
            DataTable dtChungTu = TCDN_KinhDoanh_ChungTuModels.GetChungTu(iID_MaChungTu);

            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtChungTu.Rows[0]["bChiNganSach"]);
            String iID_MaDoanhNghiep = Convert.ToString(dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);
            int iQuy = Convert.ToInt32(dtChungTu.Rows[0]["iQuy"]);

            DataTable dt = TCDN_ChiTieuModels.DT_MucLucChiTieu(iLoai);

            int Nam = iNamLamViec;
            int Quy = iQuy;
            if (iQuy == 1)
            {
                Nam = iNamLamViec - 1;
                Quy = 4;
            }
            else
            {
                Quy = iQuy - 1;
            }
            //lấy dữ liệu quý trước
            DataTable dtDauKy = Get_dtQuyNam(Nam, Quy, iID_MaDoanhNghiep);

            Bang bang = new Bang("TCDN_KinhDoanh_ChungTuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iQuy", dtChungTu.Rows[0]["iQuy"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Thêm thông tin vào bảng TCDN_ChungTuChiTiet
                ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(dt.Rows[i],dtDauKy, bang.CmdParams.Parameters);
                bang.Save();
            }

            //Đánh STT cho bảng TCDN_ChungTuChiTiet vừa insert
            DataTable dtChiTieu = TCDN_ChiTieuModels.Get_MucLucChiTieu();
            DataTable dtChungTuChiTiet = Get_dtChungTuChiTiet_TheoChungTu(iID_MaChungTu);
            int STT = 0;
            DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, iID_MaChungTu, 0, ref STT);
            dtChiTieu.Dispose();
            dtChungTuChiTiet.Dispose();
        }

        public static void ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(DataRow RMucLucChiTieu,DataTable dtDauKy, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "sKyHieu,sTen,sThuyetMinh";
            String sXauNoiMa = "";
            String[] arrDSTruong = strDSTruong.Split(',');
            if (Params.IndexOf("@iID_MaChiTieu") >= 0)
            {
                Params["@iID_MaChiTieu"].Value = RMucLucChiTieu["iID_MaChiTieu"];
                Params["@iID_MaChiTieu_Cha"].Value = RMucLucChiTieu["iID_MaChiTieu_Cha"];
                Params["@bLaHangCha"].Value = RMucLucChiTieu["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaChiTieu", RMucLucChiTieu["iID_MaChiTieu"]);
                Params.AddWithValue("@iID_MaChiTieu_Cha", RMucLucChiTieu["iID_MaChiTieu_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucChiTieu["bLaHangCha"]);
            }
            //Lấy dữ liệu từ quý trước
            String iID_MaChiTieu = Convert.ToString(RMucLucChiTieu["iID_MaChiTieu"]);
            String iID_MaChiTieu1 = "";
            for (int i = 0; i < dtDauKy.Rows.Count; i++)
            {
                iID_MaChiTieu1 = Convert.ToString(dtDauKy.Rows[i]["iID_MaChiTieu"]);
                if (iID_MaChiTieu.Equals(iID_MaChiTieu1))
                {
                    if (Params.IndexOf("@rNamTruoc") >= 0)
                    {
                        Params["@rNamTruoc"].Value = dtDauKy.Rows[i]["rNamNay"];
                    }
                    else
                    {
                        Params.AddWithValue("@rNamTruoc", dtDauKy.Rows[i]["rNamNay"]);
                    }
                    dtDauKy.Rows.RemoveAt(i);
                    break;
                }
            }
            //End Lấy dữ liệu từ quý trước

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
                if (i < arrDSTruong.Length - 1 && String.IsNullOrEmpty(Convert.ToString(RMucLucChiTieu[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucChiTieu[arrDSTruong[i]]);
                }
            }
        }

        public static DataTable Get_dtQuyNam(int iNamLamViec, int iQuy, String iID_MaDoanhNghiep)
        {
            String SQL = "SELECT * FROM TCDN_KinhDoanh_ChungTuChiTiet WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static String InsertDuyetQuyetToan(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("TCDN_KinhDoanh_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = TCDN_KinhDoanh_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(TCDNModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM TCDN_KinhDoanh_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = TCDN_KinhDoanh_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(TCDNModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtChungTuChiTiet(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = "sKyHieu,sTen";
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

            SQL = String.Format("SELECT * FROM TCDN_KinhDoanh_ChungTuChiTiet WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_dtChungTuChiTiet_TheoChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

            SQL = String.Format("SELECT * FROM TCDN_KinhDoanh_ChungTuChiTiet WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        private static void DanhSTTChoCay(DataTable dtChiTieu, DataTable dtChungTuChiTiet, String MaChungTu, int MaHangMauCha, ref int STT)
        {
            int i, j, MaChiTieu;
            SqlCommand cmd;
            for (i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                if (MaHangMauCha == Convert.ToInt32(dtChiTieu.Rows[i]["iID_MaChiTieu_Cha"]))
                {
                    MaChiTieu = Convert.ToInt32(dtChiTieu.Rows[i]["iID_MaChiTieu"]);
                    for (j = 0; j < dtChungTuChiTiet.Rows.Count; j++)
                    {
                        if (MaChiTieu == Convert.ToInt32(dtChungTuChiTiet.Rows[j]["iID_MaChiTieu"]))
                        {
                            STT++;
                            cmd = new SqlCommand();
                            cmd.CommandText = String.Format("UPDATE TCDN_KinhDoanh_ChungTuChiTiet " +
                                                            "SET iSTT=@iSTT " +
                                                            "WHERE iID_MaChiTieu=@iID_MaChiTieu AND " +
                                                                  "iID_MaChungTu=@iID_MaChungTu");
                            cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                            cmd.Parameters.AddWithValue("@iID_MaChiTieu", MaChiTieu);
                            cmd.Parameters.AddWithValue("@iSTT", STT);
                            Connection.UpdateDatabase(cmd);
                        }
                    }
                    DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, MaChungTu, MaChiTieu, ref STT);
                }
            }
        }
    }
}