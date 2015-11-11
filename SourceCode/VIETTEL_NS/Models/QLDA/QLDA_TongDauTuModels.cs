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
    public class QLDA_TongDauTuModels
    {
        public static NameValueCollection LayThongTin(String sSoQuyetDinh)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_TongDauTu_QuyetDinh(sSoQuyetDinh);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_dtTongDauTu(String sSoPheDuyet, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (sSoPheDuyet == null) sSoPheDuyet = "";

            DK = "iTrangThai=1 AND sSoPheDuyet=@sSoPheDuyet";
            cmd.Parameters.AddWithValue("@sSoPheDuyet", sSoPheDuyet);

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

            SQL = String.Format("SELECT * FROM QLDA_TongDautu WHERE {0} ORDER BY sTenDuAn", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        /// <summary>
        /// Lấy trạng thái từ chối
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDanhMucDuAn"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaDanhMucDuAn, String dNgayLap)
        {
            int vR = -1;
            DataTable dt = KTCT_TienMat_ChungTuModels.GetChungTu(iID_MaDanhMucDuAn);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(QLDAModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM KTTM_ChungTuChiTiet WHERE iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }
        /// <summary>
        /// Lấy trạng thái trình duyệt
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDanhMucDuAn"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaDanhMucDuAn, String dNgayLap)
        {
            int vR = -1;
            DataTable dt = KTCT_TienMat_ChungTuModels.GetChungTu(iID_MaDanhMucDuAn);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(QLDAModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_DanhSachTongDauTu(String iID_MaDanhMucDuAn, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(iID_MaDanhMucDuAn) == false && iID_MaDanhMucDuAn != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayTao >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayTao <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT * FROM QLDA_TongDauTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachTongDauTu_Count(String iID_MaDanhMucDuAn, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(iID_MaDanhMucDuAn) == false && iID_MaDanhMucDuAn != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayTao >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayTao <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT COUNT(*) FROM QLDA_TongDauTu WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd,0));
            cmd.Dispose();            
            
            return vR;
        }

        public static DataTable Get_DanhSachTongDauTu_QuyetDinh(String sSoQuyetDinh, String TuNgay, String DenNgay, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(sSoQuyetDinh) == false && sSoQuyetDinh != "")
            {
                DK += " AND sSoQuyetDinh = @sSoQuyetDinh";
                cmd.Parameters.AddWithValue("@sSoQuyetDinh", sSoQuyetDinh);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT * FROM QLDA_TongDauTu_QuyetDinh WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachTongDauTu_QuyetDinh_Count(String sSoQuyetDinh, String TuNgay, String DenNgay, String sNguoiDung)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(sSoQuyetDinh) == false && sSoQuyetDinh != "")
            {
                DK += " AND sSoQuyetDinh = @sSoQuyetDinh";
                cmd.Parameters.AddWithValue("@sSoQuyetDinh", sSoQuyetDinh);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM QLDA_TongDauTu_QuyetDinh WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_Row_TongDauTu_QuyetDinh(String sSoQuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_TongDauTu_QuyetDinh WHERE iTrangThai = 1 AND sSoQuyetDinh=@sSoQuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sSoQuyetDinh", sSoQuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Row_TongDauTu_QuyetDinh1(String iID_MaTongDauTu_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_TongDauTu_QuyetDinh WHERE iTrangThai = 1 AND iID_MaTongDauTu_QuyetDinh=@iID_MaTongDauTu_QuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTongDauTu_QuyetDinh", iID_MaTongDauTu_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static Int32 CheckSoQuyetDinhTrongTongDauTu(String sSoQuyetDinh)
        {
            Int32 vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(*) FROM QLDA_TongDauTu WHERE iTrangThai = 1 AND sSoPheDuyet=@sSoPheDuyet";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sSoPheDuyet", sSoQuyetDinh);
            vR = Convert.ToInt32(Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_NguoiTaoQuyetDinhTongDauTu()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT distinct sID_MaNguoiDungTao, sID_MaNguoiDungTao as sTen FROM QLDA_TongDauTu_QuyetDinh WHERE iTrangThai = 1 ORDER BY sID_MaNguoiDungTao");
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            DataRow R = vR.NewRow();
            R["sID_MaNguoiDungTao"] = "";
            R["sTen"] = "--- Danh sách người tạo ---";
            vR.Rows.InsertAt(R, 0);

            return vR;
        }

        public static DataTable Get_Table_TongDauTu(String sSoPheDuyet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND sSoPheDuyet = @sSoPheDuyet";
            cmd.Parameters.AddWithValue("@sSoPheDuyet", sSoPheDuyet);
            
            String SQL = String.Format("SELECT * FROM QLDA_TongDauTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static void DienDuLieuCuaSoQuyetDinhCu(String sSoQuyetDinh, String sSoQuyetDinh_Cu, String MaND, String IP)
        {
            NameValueCollection data = LayThongTin(sSoQuyetDinh);
            DataTable dt = Get_Table_TongDauTu(sSoQuyetDinh_Cu);
            DataRow R;
            int i, j;

            String TenBangChiTiet = "QLDA_TongDauTu";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                R = dt.Rows[i];

                Bang bang = new Bang(TenBangChiTiet);

                bang.DuLieuMoi = true;

                bang.CmdParams.Parameters.AddWithValue("@dNgayLap", data["dNgayLap"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucDuAn", R["iID_MaDanhMucDuAn"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucDuAn_Cha", R["iID_MaDanhMucDuAn_Cha"]);
                bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa_DuAn", R["sXauNoiMa_DuAn"]);
                bang.CmdParams.Parameters.AddWithValue("@bLaHangCha_DuAn", R["bLaHangCha_DuAn"]);
                bang.CmdParams.Parameters.AddWithValue("@sDeAn", R["sDeAn"]);
                bang.CmdParams.Parameters.AddWithValue("@sDuAn", R["sDuAn"]);
                bang.CmdParams.Parameters.AddWithValue("@sDuAnThanhPhan", R["sDuAnThanhPhan"]);
                bang.CmdParams.Parameters.AddWithValue("@sCongTrinh", R["sCongTrinh"]);
                bang.CmdParams.Parameters.AddWithValue("@sHangMucCongTrinh", R["sHangMucCongTrinh"]);
                bang.CmdParams.Parameters.AddWithValue("@sHangMucChiTiet", R["sHangMucChiTiet"]);
                bang.CmdParams.Parameters.AddWithValue("@sTenDuAn", R["sTenDuAn"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", R["iID_MaMucLucNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", R["iID_MaMucLucNganSach_Cha"]);
                bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", R["sXauNoiMa"]);
                bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", R["bLaHangCha"]);
                bang.CmdParams.Parameters.AddWithValue("@sLNS", R["sLNS"]);
                bang.CmdParams.Parameters.AddWithValue("@sL", R["sL"]);
                bang.CmdParams.Parameters.AddWithValue("@sK", R["sK"]);
                bang.CmdParams.Parameters.AddWithValue("@sM", R["sM"]);
                bang.CmdParams.Parameters.AddWithValue("@sTM", R["sTM"]);
                bang.CmdParams.Parameters.AddWithValue("@sTTM", R["sTTM"]);
                bang.CmdParams.Parameters.AddWithValue("@sNG", R["sNG"]);
                bang.CmdParams.Parameters.AddWithValue("@sTNG", R["sTNG"]);
                bang.CmdParams.Parameters.AddWithValue("@sMoTa", R["sMoTa"]);
                bang.CmdParams.Parameters.AddWithValue("@sTienDo", R["sTienDo"]);
                bang.CmdParams.Parameters.AddWithValue("@sSoPheDuyet", data["sSoQuyetDinh"]);
                bang.CmdParams.Parameters.AddWithValue("@dNgayPheDuyet", data["dNgayQuyetDinh"]);
                bang.CmdParams.Parameters.AddWithValue("@sCapPheDuyet", data["sCapPheDuyet"]);
                bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rSoTien"]);
                bang.CmdParams.Parameters.AddWithValue("@rNgoaiTe_SoTien", R["rNgoaiTe_SoTien"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNgoaiTe_SoTien", R["iID_MaNgoaiTe_SoTien"]);
                bang.CmdParams.Parameters.AddWithValue("@sTenNgoaiTe_SoTien", R["sTenNgoaiTe_SoTien"]);
                bang.CmdParams.Parameters.AddWithValue("@rNgoaiTe", R["rNgoaiTe"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNgoaiTe", R["iID_MaNgoaiTe"]);
                bang.CmdParams.Parameters.AddWithValue("@sTenNgoaiTe", R["sTenNgoaiTe"]);
                bang.CmdParams.Parameters.AddWithValue("@rTyGia", R["rTyGia"]);

                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IP;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            dt.Dispose();
        }
    }
}