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
    public class KTTG_TinhChatCapThu_ChungTuModels
    {
        public static String Get_Max_ChungTu(String iNam)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iSoChungTu) FROM KTTG_ChungTuCapThu WHERE iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static int Delete_ChungTu(String iID_MaChungTu, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng TN_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM KTTG_ChungTuChiTietCapThu WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng TN_ChungTu
            Bang bang = new Bang("KTTG_ChungTuCapThu");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.Delete();
            return 1;
        }
        public static DataTable Get_Row_ChungTu(String iID_MaChungTu, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM KTTG_ChungTuCapThu WHERE iID_MaChungTu=@iID_MaChungTu AND iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChungTu(String iNamLamViec, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iNamLamViec=@iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT * FROM KTTG_ChungTuCapThu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String iNamLamViec, String sNguoiDung)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iNamLamViec=@iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM KTTG_ChungTuCapThu WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }
        public static NameValueCollection LayThongTin(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_ChungTu_ChiTiet(iID_MaChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                if (dt.Rows.Count > 0)
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                else Data[colName] = colName;
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_Row_ChungTu_ChiTiet(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM KTTG_ChungTuCapThu WHERE iTrangThai = 1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Grid_dtChungTu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_MaChungTu == null) iID_MaChungTu = "";


            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

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

            SQL = String.Format("SELECT * FROM KTTG_ChungTuChiTietCapThu WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Cập nhập dữ liệu 1 Record của Chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="Params">Params là của cmd.Parameters</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean UpdateRecord(String iID_MaChungTu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("KTTG_ChungTuCapThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }
        /// <summary>
        /// Lấy trạng thái từ chối
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = Get_Row_ChungTu_ChiTiet(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = 0;
            if (dt.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM KTTG_ChungTuChiTietCapThu WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
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
        /// <summary>
        /// Lấy trạng thái trình duyệt của phần kế toán tổng hợp
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = Get_Row_ChungTu_ChiTiet(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = 0;
            if (dt.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng KTTG_ChungTu, KTTG_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaChungTu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng KTTG_ChungTuChiTiet            
            String SQL = "UPDATE KTTG_ChungTuChiTietCapThu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE KTTG_ChungTuChiTietCapThu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChungTu=@iID_MaChungTu";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        public static String InsertDuyetChungTu(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChungTu;
            Bang bang = new Bang("KTTG_DuyetChungTuCapThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaChungTu)
        {
            String iID_MaDuyetChungTu;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetChungTuCuoiCung FROM KTTG_ChungTuCapThu WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            iID_MaDuyetChungTu = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM KTTG_ChungTuChiTietCapThu WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("{0}<br/>", dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE KTTG_DuyetChungTuCapThu SET sSua=@sSua WHERE iID_MaDuyetChungTu=@iID_MaDuyetChungTu");
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTu", iID_MaDuyetChungTu);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
    }
}