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
    public class CapPhat_ChungTuChiTiet_CucModels
    {
        public static void ThemChiTiet(String iID_MaCapPhat, String MaND, String IPSua,String sLoai)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            DataTable dtCapPhat = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            //DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            int iNamLamViec = Convert.ToInt32(dtCapPhat.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtCapPhat.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtCapPhat.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtCapPhat.Rows[0]["bChiNganSach"]);

            Bang bang = new Bang("CP_CapPhatChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", dtCapPhat.Rows[0]["iID_MaCapPhat"]);
            bang.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", dtCapPhat.Rows[0]["iDM_MaLoaiCapPhat"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtCapPhat.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtCapPhat.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtCapPhat.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCapPhat.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtCapPhat.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtCapPhat.Rows[0]["bChiNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", "");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dt.Rows[i]["sLNS"]);
                sLoai = Convert.ToString(dt.Rows[i]["sNhapTheoTruong"]);
                DataTable dtMucLucNganSach = NganSach_HamChungModels.DT_MucLucNganSach_sLNS(sLNS, sLoai);
                for (int j = 0; j < dtMucLucNganSach.Rows.Count; j++)
                {   
                    //Dien thong tin cua Muc luc ngan sach
                    NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLucNganSach.Rows[j], bang.CmdParams.Parameters);
                    //if (Convert.ToBoolean(dtMucLucNganSach.Rows[j]["bLaHangCha"]))
                    //{
                    //bang.CmdParams.Parameters["@iID_MaDonVi"].Value = "";
                        bang.Save();
                    //}
                    //else
                    //{
                    //    for (int csDonVi = 0; csDonVi < dtDonVi.Rows.Count; csDonVi++)
                    //    {
                    //        bang.CmdParams.Parameters["@iID_MaDonVi"].Value = dtDonVi.Rows[csDonVi]["iID_MaDonVi"];
                    //        bang.Save();
                    //    }
                    //}
                }
                dtMucLucNganSach.Dispose();
            }

            dt.Dispose();
            dtCapPhat.Dispose();
            //dtDonVi.Dispose();
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaCapPhat)
        {
            int vR = -1;
            DataTable dt = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeCapPhat, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM CP_CapPhatChiTiet WHERE iID_MaCapPhat=@iID_MaCapPhat AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaCapPhat)
        {
            int vR = -1;
            DataTable dt = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeCapPhat, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtChungTuChiTiet(String iID_MaCapPhat, Dictionary<String, String> arrGiaTriTimKiem,String Loai="")
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            String DKLoai = "";
            if (String.IsNullOrEmpty(Loai) == false)
            {
                switch (Loai)
                {
                    case "sTM":
                        DKLoai = " AND sTTM='' ";
                        break;
                    case "sM":
                        DKLoai = " AND sTM='' ";
                        break;
                    case "sLNS":
                        DKLoai = "AND sM='' ";
                        break;
                }
            }
            
            DK = "iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat " +DKLoai;
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
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

            SQL = String.Format("SELECT * FROM CP_CapPhatChiTiet WHERE {0} ORDER BY sXauNoiMa, iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_dtTongCapPhatChoDonVi(String iID_MaMucLucNganSach,
                                                         String iID_MaDonVi,
                                                         int iNamLamViec,
                                                         String dNgayCapPhat,
                                                         int iID_MaNguonNganSach,
                                                         int iID_MaNamNganSach)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(CapPhatModels.iID_MaPhanHe));
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            DK += " AND iID_MaMucLucNganSach=@iID_MaMucLucNganSach";
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            DK += " AND iID_MaDonVi=@iID_MaDonVi";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DK += " AND iID_MaCapPhat IN (SELECT iID_MaCapPhat FROM CP_CapPhat WHERE dNgayCapPhat <= @dNgayCapPhat)";
            cmd.Parameters.AddWithValue("@dNgayCapPhat", dNgayCapPhat);

            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = "";
            for (int i = 0; i < arrDSTruongTien_So.Length; i++)
            {
                if (i > 0) strTruong += ",";
                strTruong += String.Format("SUM({0}) AS Sum{0}",arrDSTruongTien_So[i]);
            }

            cmd.CommandText = String.Format("SELECT {0} FROM CP_CapPhatChiTiet WHERE {1}", strTruong, DK);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}