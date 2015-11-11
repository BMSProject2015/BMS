using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Collections.Specialized;
namespace VIETTEL.Models
{
    public class QLDA_CapPhatModels
    {
        public static String Get_Max_Dot(String iNam)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iDot) FROM QLDA_CapPhat_Dot WHERE iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static int Delete_Dot_CapPhat(String iID_MaDotCapPhat, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng TN_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM QLDA_CapPhat WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng TN_ChungTu
            Bang bang = new Bang("QLDA_CapPhat_Dot");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaDotCapPhat;
            bang.Delete();
            return 1;
        }
        public static DataTable Get_Row_DotCapPhat(String iID_MaDotCapPhat, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_CapPhat_Dot WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static int Get_Max_SoPheDuyetCapPhat()
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iSoPheDuyet) FROM QLDA_CapPhat WHERE iTrangThai = 1";
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Row_CapPhat(String iID_MaDotCapPhat, String iID_MaCapPhat, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_CapPhat WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iID_MaCapPhat=@iID_MaCapPhat AND iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static NameValueCollection LayThongTin(String iID_MaDotCapPhat, String iID_MaCapPhat, String iNam)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, iNam);
            String colName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            dt.Dispose();
            return Data;
        }
        public static NameValueCollection LayThongTinHopDong(String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = QLDA_HopDongModels.Get_Row_HopDongChiTiet(iID_MaHopDong, iID_MaDanhMucDuAn);
            String colName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_MaxRow_CapPhat(String iID_MaDotCapPhat, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT TOP 1 * FROM QLDA_CapPhat WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iNamLamViec=@iNamLamViec AND iTrangThai = 1 ORDER BY dNgayLap DESC";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_GiaTriNamTruoc_CapPhat(String iID_MaHopDong, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT SUM(rChuDauTuTamUng) AS rChuDauTuTamUng, SUM(rChuDauTuThanhToan) AS rChuDauTuThanhToan, " +
                           " SUM(rChuDauTuThuTamUng) AS rChuDauTuThuTamUng, " +
                           " SUM(rPheDuyetTamUng) AS rPheDuyetTamUng, SUM(rPheDuyetThanhToanTrongNam) AS rPheDuyetThanhToanTrongNam, " +
                           " SUM(rPheDuyetThanhToanHoanThanh) AS rPheDuyetThanhToanHoanThanh, SUM(rPheDuyetThuTamUng) AS rPheDuyetThuTamUng, " +
                           " SUM(rPheDuyetThuKhac) AS rPheDuyetThuKhac " +
                           "FROM QLDA_CapPhat WHERE iID_MaHopDong=@iID_MaHopDong AND iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam)-1);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_PrevRow_CapPhat(String iID_MaDotCapPhat, String iID_MaCapPhat, String iNam)
        {
            DataTable vR = null;
            DataTable dt = Get_Row_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, iNam);
            if (dt.Rows.Count > 0)
            {
                DateTime dNgayLap = Convert.ToDateTime(dt.Rows[0]["dNgayLap"]);                
                SqlCommand cmd = new SqlCommand();
                String SQL = "SELECT TOP 1 * FROM QLDA_CapPhat WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iID_MaCapPhat<>@iID_MaCapPhat AND iNamLamViec=@iNamLamViec AND dNgayLap<=@dNgayLap AND iTrangThai = 1 ORDER BY dNgayLap DESC";
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
                cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                cmd.Parameters.AddWithValue("@dNgayLap", dNgayLap);
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();
                dt.Dispose();
            }
            return vR;
        }
        public static DataTable Get_NextRow_CapPhat(String iID_MaDotCapPhat, String iID_MaCapPhat, String iNam)
        {
            DataTable vR =null;
            DataTable dt = Get_Row_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, iNam);
            if (dt.Rows.Count > 0)
            {
                DateTime dNgayLap = Convert.ToDateTime(dt.Rows[0]["dNgayLap"]);
                SqlCommand cmd = new SqlCommand();
                String SQL = "SELECT TOP 1 * FROM QLDA_CapPhat WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iID_MaCapPhat<>@iID_MaCapPhat AND iNamLamViec=@iNamLamViec AND dNgayLap>=@dNgayLap AND iTrangThai = 1 ORDER BY dNgayLap ASC";
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
                cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                cmd.Parameters.AddWithValue("@dNgayLap", dNgayLap);
                vR = Connection.GetDataTable(cmd);
                cmd.Dispose();
                dt.Dispose();
            }
            return vR;
        }
        public static DataTable Get_Table_CapPhat(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_MaDotCapPhat == null) iID_MaDotCapPhat = "";
            if (iID_MaHopDong == null) iID_MaHopDong = "";
            if (iID_MaDanhMucDuAn == null) iID_MaDanhMucDuAn = "";


            DK = "iTrangThai=1";
            //if (iID_MaDotCapPhat != "")
            //{
                DK += " AND iID_MaDotCapPhat=@iID_MaDotCapPhat";
                cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            //}
            //if (iID_MaHopDong != "") {
                DK += " AND iID_MaHopDong=@iID_MaHopDong";
                cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            //}
            //if (iID_MaDanhMucDuAn != "") { 
                DK += " AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            //}

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

            SQL = String.Format("SELECT * FROM QLDA_CapPhat WHERE {0} ORDER BY dNgayTao DESC", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_Table_CapPhatChiTiet(String iID_MaDotCapPhat,  Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_MaDotCapPhat == null) iID_MaDotCapPhat = "";


            DK = "QLDA_CapPhat.iTrangThai=1";
            //if (iID_MaDotCapPhat != "")
            //{
            DK += " AND QLDA_CapPhat.iID_MaDotCapPhat=@iID_MaDotCapPhat";
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            //}
            //if (iID_MaHopDong != "") {
            //}

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

            SQL = String.Format(@"SELECT * FROM QLDA_CapPhat
                                    INNER JOIN (SELECT iID_MaDanhMucDuAn,sDeAn+'-'+sDuAn+'-'+sDuAnThanhPhan+'-'+sCongTrinh+'-'+sHangMucChiTiet +'-'+sTenDuAn as sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                          sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                  FROM QLDA_DanhMucDuAn 
                  WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as QLDA_DanhMucDuAn
                  ON QLDA_CapPhat.iID_MaDanhMucDuAn=QLDA_DanhMucDuAn.iID_MaDanhMucDuAn
   LEFT JOIN(
   SELECT * FROM QLDA_HopDong  WHERE    iTrangThai=1) QLDA_HopDong
   ON QLDA_CapPhat.iID_MaHopDong=QLDA_HopDong.iID_MaHopDong
                                    WHERE {0} ORDER BY sTenDuAn ", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy DataTable thông tin của một chứng từ thu nộp ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable GetChungTu(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, String iNam)
        {
            DataTable vR;
            SqlCommand cmd;
            cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iID_MaDotCapPhat=@iID_MaDotCapPhat AND iNamLamViec=@iNamLamViec";
            if (iID_MaHopDong != null && iID_MaHopDong != "") {
                DK += " AND iID_MaHopDong=@iID_MaHopDong";
                cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            }
            if (iID_MaHopDong != null && iID_MaHopDong != "")
            {
                DK += " AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            }
            String SQL = String.Format("SELECT * FROM QLDA_CapPhat_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng KTTM_ChungTu
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="Params"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertRecord(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, NameValueCollection data, String MaND, String IPSua)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            Bang bang = new Bang("QLDA_CapPhat_ChungTu");
            bang.DuLieuMoi = true;

            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(QLDAModels.iID_MaPhanHe));
            // bang.Save();

            String MaChungTuAddNew = Convert.ToString(bang.Save());
            QLDA_CapPhatModels.InsertDuyetChungTu(MaChungTuAddNew, MessageModels.sMoiTao, MaND, IPSua);
            dtCauHinh.Dispose();

            return MaChungTuAddNew;
        }
        public static String InsertDuyetChungTu(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChungTu;
            Bang bang = new Bang("QLDA_CapPhat_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }
        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng KTTM_ChungTu, KTTM_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, String iNam, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            String strSQL = "UPDATE QLDA_CapPhat_ChungTu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iID_MaHopDong=@iID_MaHopDong AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND iNamLamViec=@iNamLamViec";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.CommandText = strSQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng KTTM_ChungTuChiTiet            
            String SQL = "UPDATE QLDA_CapPhat SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iID_MaHopDong=@iID_MaHopDong AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND iNamLamViec=@iNamLamViec";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE QLDA_CapPhat SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat AND iID_MaHopDong=@iID_MaHopDong AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND iNamLamViec=@iNamLamViec";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        /// <summary>
        /// Lấy trạng thái từ chối
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, String iNam)
        {
            int vR = -1;
            DataTable dt = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
            if (dt.Rows.Count > 0)
            {
                String iID_MaChungTu = Convert.ToString(dt.Rows[0]["iID_MaChungTu"]);
                int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
                dt.Dispose();
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(QLDAModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
                {
                    int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                    if (iID_MaTrangThaiDuyet_TuChoi > 0)
                    {
                        SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM QLDA_CapPhat WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                        cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                        {
                            vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                        }
                        cmd.Dispose();
                    }
                }
            }
            return vR;
        }
        /// <summary>
        /// Lấy trạng thái trình duyệt
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, String iNam)
        {
            int vR = -1;
            DataTable dt = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
            if (dt.Rows.Count > 0)
            {
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
            }
            return vR;
        }

        public static DataTable Get_GiaTri_MucLucNganSach_HopDong(String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iID_MaHopDong=@iID_MaHopDong AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);

            String SQL = String.Format("SELECT TOP 1 iID_MaMucLucNganSach, sXauNoiMa, sLNS, sL, sK, sM, sTM, sTTM, sNG, sTNG, sMoTa FROM QLDA_HopDongChiTiet WHERE {0} ORDER BY dNgayLap DESC", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            return vR;
        }

        public static String GetSoDuTamUng(String iID_MaDanhMucDuAn)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);

            String SQL = String.Format("SELECT SUM(rDeNghiPheDuyetTamUng) - SUM(rDeNghiPheDuyetThuTamUng) AS rSoDuTamUng FROM QLDA_CapPhat WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            return vR;
        }

        public static String GetSoDaThanhToan(String iID_MaDanhMucDuAn)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);

            String SQL = String.Format("SELECT SUM(rDeNghiPheDuyetThanhToan) AS rSoDaThanhToan FROM QLDA_CapPhat WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            return vR;
        }
    }
}