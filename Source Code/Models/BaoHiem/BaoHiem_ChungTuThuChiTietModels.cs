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
    public class BaoHiem_ChungTuThuChiTietModels
    {
        public static void ThemChiTiet(String iID_MaChungTuThu, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            DataTable dtChungTu = BaoHiem_ChungTuThuModels.GetChungTu(iID_MaChungTuThu);
            
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            int iLoaiBaoHiem = Convert.ToInt32(dtChungTu.Rows[0]["iLoaiBaoHiem"]);

            String sTenLoaiBaoHiem = "";
            switch (iLoaiBaoHiem)
            {
                case 1:
                    sTenLoaiBaoHiem = "BHXH";
                    break;
                case 2:
                    sTenLoaiBaoHiem = "BHYT";
                    break;
                case 3:
                    sTenLoaiBaoHiem = "BHTN";
                    break;
            }
            DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);

            Bang bang = new Bang("BH_ChungTuThuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iLoaiBaoHiem", iLoaiBaoHiem);
            bang.CmdParams.Parameters.AddWithValue("@sTenLoaiBaoHiem", sTenLoaiBaoHiem);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", "");
            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi","");
            //add params bảng đơng vị
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                bang.CmdParams.Parameters["@iID_MaDonVi"].Value = dtDonVi.Rows[i]["iID_MaDonVi"];
                bang.CmdParams.Parameters["@sTenDonVi"].Value = dtDonVi.Rows[i]["sTen"];
                bang.Save();
            }

            dtDonVi.Dispose();
            dtChungTu.Dispose();
        }


        public static void UpdateBangChiTiet(String UserName, String Address, String iID_MaChungTuThu, String iLoaiBaoHiem, String sTenLoaiBaoHiem, String iThang_Quy, String bLoaiThang_Quy)
        {

            String SQL = "UPDATE BH_ChungTuThuChiTiet SET iLoaiBaoHiem=@iLoaiBaoHiem,sTenLoaiBaoHiem=@sTenLoaiBaoHiem";
            SQL += ",iThang_Quy=@iThang_Quy,bLoaiThang_Quy=@bLoaiThang_Quy ";
            SQL += ",sID_MaNguoiDungSua=@sID_MaNguoiDungSua,sIPSua=@sIPSua ";
            SQL += " WHERE iID_MaChungTuThu=@iID_MaChungTuThu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            cmd.Parameters.AddWithValue("@iLoaiBaoHiem", iLoaiBaoHiem);
            cmd.Parameters.AddWithValue("@sTenLoaiBaoHiem", sTenLoaiBaoHiem);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@bLoaiThang_Quy", bLoaiThang_Quy);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", UserName);
            cmd.Parameters.AddWithValue("@sIPSua", Address);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

      

        public static String InsertDuyetBaoHiem(String iID_MaChungTuThu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("BH_DuyetChungTuThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTuThu)
        {
            int vR = -1;
            DataTable dt = BaoHiem_ChungTuThuModels.GetChungTu(iID_MaChungTuThu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM BH_ChungTuThuChiTiet WHERE iID_MaChungTuThu=@iID_MaChungTuThu AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChungTuThu)
        {
            int vR = -1;
            DataTable dt = BaoHiem_ChungTuThuModels.GetChungTu(iID_MaChungTuThu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtBaoHiemThuChiTiet(String iID_MaChungTuThu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChungTuThu=@iID_MaChungTuThu";
            cmd.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
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

            SQL = String.Format("SELECT * FROM BH_ChungTuThuChiTiet WHERE {0} ORDER BY iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}