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
    public class QLDA_KeHoachVonModels
    {
        public static DataTable DT_LoaiKeHoachVon()
        {
            DataTable vR = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_KeHoachVon_Loai WHERE iTrangThai=1 ORDER BY iSTT");
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            vR.Rows.InsertAt(vR.NewRow(), 0);
            vR.Rows[0]["iID_MaLoaiKeHoachVon"] = "-1";
            vR.Rows[0]["sTen"] = "-- Loại kế hoạch vốn --";
            return vR;
        }
        public static DataTable Get_Row_LoaiKeHoachVon(String iID_MaLoaiKeHoachVon)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_KeHoachVon_Loai WHERE iTrangThai=1 AND iID_MaLoaiKeHoachVon=@iID_MaLoaiKeHoachVon");
            cmd.Parameters.AddWithValue("@iID_MaLoaiKeHoachVon", iID_MaLoaiKeHoachVon);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static NameValueCollection LayThongTin(String dNgayKeHoachVon, String iLoaiKeHoachVon)
        {
            NameValueCollection Data = new NameValueCollection();
            Data["dNgayKeHoachVon"] = dNgayKeHoachVon;
            Data["iLoaiKeHoachVon"] = iLoaiKeHoachVon;
            return Data;
        }
        public static NameValueCollection LayThongTin(String iID_KeHoachVon_QuyetDinh)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_DuToanNam_QuyetDinh(iID_KeHoachVon_QuyetDinh);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_Row_DuToanNam_QuyetDinh(String iID_KeHoachVon_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT ct.sSoQuyetDinh, ct.dNgayKeHoachVon,ct.dNgayQD,ct.sNoiDung,loai.sTen FROM QLDA_KeHoachVon_ChungTu ct, QLDA_KeHoachVon_Loai loai WHERE loai.iTrangThai = 1 AND ct.iTrangThai = 1 AND iID_KeHoachVon_QuyetDinh=@iID_KeHoachVon_QuyetDinh AND ct.iLoaiKeHoachVon=loai.iID_MaLoaiKeHoachVon";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_dtKeHoachVon(String iID_KeHoachVon_QuyetDinh, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_KeHoachVon_QuyetDinh == null) iID_KeHoachVon_QuyetDinh = Guid.Empty.ToString();



            DK = "iTrangThai=1 AND iID_KeHoachVon_QuyetDinh=@iID_KeHoachVon_QuyetDinh";
            cmd.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
            
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

            SQL = String.Format("SELECT * FROM QLDA_KeHoachVon WHERE {0} ORDER BY sTenDuAn", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_dtKeHoachVon(String iLoaiKeHoachVon, String dNgayKeHoachVon, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iLoaiKeHoachVon == null) iLoaiKeHoachVon = "";
            if (dNgayKeHoachVon == null) dNgayKeHoachVon = "";


            DK = "iTrangThai=1 AND iLoaiKeHoachVon=@iLoaiKeHoachVon AND Convert(NVARCHAR(10),dNgayKeHoachVon,103)=@dNgayKeHoachVon";
            cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            cmd.Parameters.AddWithValue("@dNgayKeHoachVon", dNgayKeHoachVon);

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

            SQL = String.Format("SELECT * FROM QLDA_KeHoachVon WHERE {0} ORDER BY sTenDuAn", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_Row_Data(String iID_MaKeHoachVon)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_KeHoachVon WHERE iTrangThai=1 AND iID_MaKeHoachVon=@iID_MaKeHoachVon");
            cmd.Parameters.AddWithValue("@iID_MaKeHoachVon", iID_MaKeHoachVon);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSach(String iLoaiKeHoachVon, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (iLoaiKeHoachVon != null && iLoaiKeHoachVon != "")
            {
                DK += " AND iLoaiKeHoachVon = @iLoaiKeHoachVon";
                cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayKeHoachVon >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayKeHoachVon <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT * FROM QLDA_KeHoachVon WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayKeHoachVon", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iLoaiKeHoachVon, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (iLoaiKeHoachVon != null && iLoaiKeHoachVon != "")
            {
                DK += " AND iLoaiKeHoachVon = @iLoaiKeHoachVon";
                cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayKeHoachVon >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayKeHoachVon <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM QLDA_KeHoachVon WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Row_KHV_ChungTu(String iID_KeHoachVon_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_KeHoachVon_ChungTu WHERE iTrangThai = 1 AND iID_KeHoachVon_QuyetDinh=@iID_KeHoachVon_QuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_DanhSach_KHV_QuyetDinh(String sSoQuyetDinh, String iLoaiKeHoachVon, String TuNgayQD, String DenNgayQD, String TuNgay, String DenNgay, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = " AND ct.iTrangThai = 1 ";
            if (String.IsNullOrEmpty(sSoQuyetDinh) == false && sSoQuyetDinh != "")
            {
                DK += " AND ct.sSoQuyetDinh = @sSoQuyetDinh";
                cmd.Parameters.AddWithValue("@sSoQuyetDinh", sSoQuyetDinh);
            }
            if (String.IsNullOrEmpty(iLoaiKeHoachVon) == false && iLoaiKeHoachVon != "")
            {
                DK += " AND ct.iLoaiKeHoachVon = @iLoaiKeHoachVon";
                cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            }
            if (String.IsNullOrEmpty(TuNgayQD) == false && TuNgayQD != "")
            {
                DK += " AND ct.dNgayQD >= @dTuNgayQD";
                cmd.Parameters.AddWithValue("@dTuNgayQD", CommonFunction.LayNgayTuXau(TuNgayQD));
            }
            if (String.IsNullOrEmpty(DenNgayQD) == false && DenNgayQD != "")
            {
                DK += " AND ct.dNgayQD <= @dDenNgayQD";
                cmd.Parameters.AddWithValue("@dDenNgayQD", CommonFunction.LayNgayTuXau(DenNgayQD));
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND ct.dNgayKeHoachVon >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND ct.dNgayKeHoachVon <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND ct.sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT ct.iID_KeHoachVon_QuyetDinh,ct.iLoaiKeHoachVon,ct.sSoQuyetDinh, ct.dNgayKeHoachVon,ct.dNgayQD,ct.sNoiDung,loai.sTen,ct.sID_MaNguoiDungTao FROM QLDA_KeHoachVon_ChungTu ct, QLDA_KeHoachVon_Loai loai WHERE loai.iTrangThai = 1 AND ct.iLoaiKeHoachVon=loai.iID_MaLoaiKeHoachVon {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "ct.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_KHV_QuyetDinh_Count(String sSoQuyetDinh, String iLoaiKeHoachVon, String TuNgayQD, String DenNgayQD, String TuNgay, String DenNgay, String sNguoiDung)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(sSoQuyetDinh) == false && sSoQuyetDinh != "")
            {
                DK += " AND sSoQuyetDinh = @sSoQuyetDinh";
                cmd.Parameters.AddWithValue("@sSoQuyetDinh", sSoQuyetDinh);
            }
            if (String.IsNullOrEmpty(iLoaiKeHoachVon) == false && iLoaiKeHoachVon != "")
            {
                DK += " AND iLoaiKeHoachVon = @iLoaiKeHoachVon";
                cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            }
            if (String.IsNullOrEmpty(TuNgayQD) == false && TuNgayQD != "")
            {
                DK += " AND dNgayQD >= @dTuNgayQD";
                cmd.Parameters.AddWithValue("@dTuNgayQD", CommonFunction.LayNgayTuXau(TuNgayQD));
            }
            if (String.IsNullOrEmpty(DenNgayQD) == false && DenNgayQD != "")
            {
                DK += " AND dNgayQD <= @dDenNgayQD";
                cmd.Parameters.AddWithValue("@dDenNgayQD", CommonFunction.LayNgayTuXau(DenNgayQD));
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayKeHoachVon >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayKeHoachVon <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM QLDA_KeHoachVon_ChungTu WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }
        public static int CheckExits_DuToan(object iID_KeHoachVon_QuyetDinh)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(iID_MaKeHoachVon) FROM QLDA_KeHoachVon WHERE iID_KeHoachVon_QuyetDinh=@iID_KeHoachVon_QuyetDinh AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int Insert_KHV_DuToanNam(String iID_KeHoachVon_QuyetDinh, String dNgayKeHoachVon, String iLoaiKeHoachVon, int iNamLamViec, String sMaNguoiDung, String IP)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL =
                @"insert QLDA_KeHoachVon(iID_KeHoachVon_QuyetDinh,dNgayKeHoachVon,iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,iLoaiKeHoachVon,
iID_MaDanhMucDuAn, iID_MaDanhMucDuAn_Cha, sXauNoiMa_DuAn,bLaHangCha_DuAn,sDeAn,
sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaMucLucNganSach,
sXauNoiMa,bLaHangCha,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sID_MaNguoiDungTao,dNgaySua,sIPSua,sID_MaNguoiDungSua)
select @iID_KeHoachVon_QuyetDinh,@dNgayKeHoachVon, iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,@iLoaiKeHoachVon,
iID_MaDanhMucDuAn, iID_MaDanhMucDuAn_Cha, sXauNoiMa_DuAn,bLaHangCha_DuAn,sDeAn,
sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaMucLucNganSach,
sXauNoiMa,bLaHangCha,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sID_MaNguoiDungTao,GETDATE(),@sIPSua,sID_MaNguoiDungTao from (select distinct iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,
iID_MaDanhMucDuAn, iID_MaDanhMucDuAn_Cha, sXauNoiMa_DuAn,bLaHangCha_DuAn,sDeAn,
sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaMucLucNganSach,
sXauNoiMa,bLaHangCha,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sID_MaNguoiDungTao
 from QLDA_DuToan_Nam where iTrangThai=1 and iNamLamViec=@iNamLamViec and sID_MaNguoiDungTao=@sMaNguoiDung) a
order by sXauNoiMa_DuAn, sXauNoiMa";
 //from QLDA_DuToan_Nam where iTrangThai=1 and iNamLamViec=@iNamLamViec and sID_MaNguoiDungTao=@sMaNguoiDung";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
            cmd.Parameters.AddWithValue("@dNgayKeHoachVon", CommonFunction.LayNgayTuXau(dNgayKeHoachVon));
            cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            cmd.Parameters.AddWithValue("@sIPSua", IP);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            vR = Connection.UpdateDatabase(cmd);
            return vR;
        }
    }
}