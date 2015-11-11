using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class TCDN_HoSoDoanhNghiepModels
    {
        public static void ThemChiTiet(String iID_MaDoanhNghiep, String iNam, String iQuy, String MaND, String IPSua)
        {
            String iLoai = "2";
            DataTable dt = TCDN_HoSoDoanhNghiep_ChiTieuModels.DT_MucLucChiTieu(iLoai);
            int i, j, k;
            Bang bang = new Bang("TCDN_HoSoDoanhNghiepChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //Thêm thông tin vào bảng TCDN_HoSoDoanhNghiepChiTiet
                ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(dt.Rows[i], bang.CmdParams.Parameters);
                bang.Save();
            }

            dt.Dispose();

            //Đánh STT cho bảng TCDN_HoSoDoanhNghiepChiTiet vừa insert
            DataTable dtChiTieu = TCDN_HoSoDoanhNghiep_ChiTieuModels.Get_MucLucChiTieu(iLoai);
            DataTable dtChungTuChiTiet = Get_dtChungTuChiTiet_TheoChungTu(iID_MaDoanhNghiep, iNam, iQuy);
            int STT = 0;
            DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, iID_MaDoanhNghiep, 0, ref STT);
            dtChiTieu.Dispose();
            dtChungTuChiTiet.Dispose();

            Int32 iNamTruoc = Convert.ToInt32(iNam) - 1;
            DataTable dtNamTruoc = LietKeGiaTriCuaHoSo_Nam(iID_MaDoanhNghiep, Convert.ToString(iNamTruoc), iQuy);
            DataTable dtNamHienTai = LietKeGiaTriCuaHoSo_Nam(iID_MaDoanhNghiep, iNam, iQuy);
            DataRow R, R1;
            for (i = 0; i < dtNamHienTai.Rows.Count; i++) {
                R = dtNamHienTai.Rows[i];
                SqlCommand cmd;
                String SQL;
                //Update trường số liệu quý trước cho bảng
                for (j = 0; j < dtNamTruoc.Rows.Count; j++) {
                    R1 = dtNamTruoc.Rows[j];
                    if (Convert.ToString(R1["iID_MaChiTieuHoSo"]) == Convert.ToString(R["iID_MaChiTieuHoSo"]))
                    {
                        if (Convert.ToString(R1["rNamBaoCao"]) != null && Convert.ToString(R1["rNamBaoCao"]) != "")
                        {
                            SQL = "UPDATE TCDN_HoSoDoanhNghiepChiTiet SET rNamTruoc=@rNamTruoc WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep " +
                                    "AND iNamLamViec=@iNamLamViec AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND iQuy=@iQuy";
                            cmd = new SqlCommand();
                            cmd.CommandText = SQL;
                            cmd.Parameters.AddWithValue("@rNamTruoc", Convert.ToString(R1["rNamBaoCao"]));
                            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", Convert.ToString(R["iID_MaChiTieuHoSo"]));
                            cmd.Parameters.AddWithValue("@iQuy", iQuy);
                            Connection.UpdateDatabase(cmd);
                            cmd.Dispose();
                        }
                        break;
                    }
                }

                //Update hàng số liệu lấy từ bang cân đối kế toán sang
                Double vGiaTri = 0;
                DataTable dtMap = LayChiTieuMapHoSoVaCanDoi(Convert.ToString(R["iID_MaChiTieuHoSo"]));

                String DonViTienTe=Convert.ToString(R["sDonViTinh"]);

                if (dtMap.Rows.Count > 0)
                {
                    for (j = 0; j < dtMap.Rows.Count; j++)
                    {
                        int iLoaiChiTieu = TCDN_ChiTieuModels.CheckLoaiChiTieu(Convert.ToString(dtMap.Rows[j]["iID_MaChiTieu"]));
                        if (iLoaiChiTieu == 2) {
                            vGiaTri += LayGiaTriBangCanDoiKinHDoanh(Convert.ToString(dtMap.Rows[j]["iID_MaChiTieu"]), iID_MaDoanhNghiep, iNam, iQuy);
                        }
                        else {
                            vGiaTri += LayGiaTriBangCanDoi(Convert.ToString(dtMap.Rows[j]["iID_MaChiTieu"]), iID_MaDoanhNghiep, iNam, iQuy);
                        }
                        if (DonViTienTe.IndexOf("Tr") >= 0 || DonViTienTe.IndexOf("tr") > 0)
                        {
                            vGiaTri = vGiaTri / 1000000;
                        }
                    }
                }
                dtMap.Dispose();

                SQL = "UPDATE TCDN_HoSoDoanhNghiepChiTiet SET rNamBaoCao=@rNamBaoCao WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep " +
                        "AND iNamLamViec=@iNamLamViec AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND iQuy=@iQuy";
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@rNamBaoCao", vGiaTri);
                cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", Convert.ToString(R["iID_MaChiTieuHoSo"]));
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            dtNamTruoc.Dispose();
            dtNamHienTai.Dispose();
        }

        public static void ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(DataRow RMucLucChiTieu, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "sKyHieu,sTen,sDonViTinh";
            String sXauNoiMa = "";
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
                if (i < arrDSTruong.Length - 1 && String.IsNullOrEmpty(Convert.ToString(RMucLucChiTieu[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucChiTieu[arrDSTruong[i]]);
                }
            }
            if (Params.IndexOf("@sXauNoiMa") >= 0)
            {
                Params["@sXauNoiMa"].Value = sXauNoiMa;
            }
            else
            {
                Params.AddWithValue("@sXauNoiMa", sXauNoiMa);
            }
        }

        public static String InsertDuyetQuyetToan(String iID_MaDoanhNghiep, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("TN_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaDoanhNghiep)
        {
            int vR = -1;
            DataTable dt = TCDN_ChungTuModels.GetChungTu(iID_MaDoanhNghiep);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(TCDNModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM TCDN_HoSoDoanhNghiepChiTiet WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaDoanhNghiep)
        {
            int vR = -1;
            DataTable dt = TCDN_ChungTuModels.GetChungTu(iID_MaDoanhNghiep);
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

        public static DataTable Get_dtChungTuChiTiet(String iID_MaDoanhNghiep, String iNam, String iQuy, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);

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

            SQL = String.Format("SELECT * FROM TCDN_HoSoDoanhNghiepChiTiet WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_dtChungTuChiTiet_TheoChungTu(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);

            SQL = String.Format("SELECT * FROM TCDN_HoSoDoanhNghiepChiTiet WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static Boolean Check_ChiTieu_Nam(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            Boolean vR = false;
            Int32 iCount = 0;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);

            SQL = String.Format("SELECT * FROM TCDN_HoSoDoanhNghiepChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;

            iCount = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            if (iCount > 0) {
                vR = true;
            }
            return vR;
        }

        private static void DanhSTTChoCay(DataTable dtChiTieu, DataTable dtChungTuChiTiet, String MaChungTu, int MaHangMauCha, ref int STT)
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
                            cmd.CommandText = String.Format("UPDATE TCDN_HoSoDoanhNghiepChiTiet " +
                                                            "SET iSTT=@iSTT " +
                                                            "WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND " +
                                                                  "iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
                            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", MaChungTu);
                            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", MaChiTieu);
                            cmd.Parameters.AddWithValue("@iSTT", STT);
                            Connection.UpdateDatabase(cmd);
                        }
                    }
                    DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, MaChungTu, MaChiTieu, ref STT);
                }
            }
        }

        private static DataTable LietKeGiaTriCuaHoSo_Nam(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_HoSoDoanhNghiepChiTiet WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable LayChiTieuMapHoSoVaCanDoi(String iID_MaChiTieuHoSo) {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static Double LayGiaTriBangCanDoi(String iID_MaChiTieu, String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            Double vR;

            SqlCommand cmd = new SqlCommand("SELECT SUM(rSoCuoiNam) FROM TCDN_ChungTuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Convert.ToDouble( Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }
        public static Double LayGiaTriBangCanDoiKinHDoanh(String iID_MaChiTieu, String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            Double vR;

            SqlCommand cmd = new SqlCommand("SELECT SUM(rNamNay) FROM TCDN_KinhDoanh_ChungTuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}