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
    public class QLDA_DuToan_QuyModels
    {
        public static NameValueCollection LayThongTin(String iID_MaDuToanQuy_QuyetDinh)
        {
            //NameValueCollection Data = new NameValueCollection();
            //Data["dNgayLap"] = dNgayLap;
            //return Data;
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_DuToan_Quy_QuyetDinh(iID_MaDuToanQuy_QuyetDinh);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_Row_DuToan_Quy_QuyetDinh(String iID_MaDuToanQuy_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_DuToan_Quy_ChungTu WHERE iTrangThai = 1 AND iID_MaDuToanQuy_QuyetDinh=@iID_MaDuToanQuy_QuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Grid_dtDuToanQuy(String iID_MaDuToanQuy_QuyetDinh, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            //if (dNgayLap == null) dNgayLap = "";


            //DK = "iTrangThai=1 AND Convert(NVARCHAR(10),dNgayLap,103)=@dNgayLap";
            //cmd.Parameters.AddWithValue("@dNgayLap", dNgayLap);
            if (iID_MaDuToanQuy_QuyetDinh == null) iID_MaDuToanQuy_QuyetDinh = Guid.Empty.ToString();


            DK = "iTrangThai=1 AND iID_MaDuToanQuy_QuyetDinh=@iID_MaDuToanQuy_QuyetDinh";
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
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

            SQL = String.Format("SELECT * FROM QLDA_DuToan_Quy WHERE {0} ORDER BY sTenDuAn", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_Row_Data(String iID_MaDuToanQuy)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_DuToan_Quy WHERE iTrangThai=1 AND iID_MaDuToanQuy=@iID_MaDuToanQuy");
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy", iID_MaDuToanQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSach(String iQuy, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (iQuy != null && iQuy != "")
            {
                DK += " AND iQuy = @iQuy";
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
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
            String SQL = String.Format("SELECT * FROM QLDA_DuToan_Quy WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayLap", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iQuy, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (iQuy != null && iQuy != "")
            {
                DK += " AND iQuy = @iQuy";
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
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
            String SQL = String.Format("SELECT Count(*) FROM QLDA_DuToan_Quy WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Table_KeHoachVon(String iID_MaDanhMucDuAn, String iID_MaMucLucNganSach, String iLoaiKeHoachVon)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += "AND iLoaiKeHoachVon=@iLoaiKeHoachVon AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn AND iID_MaMucLucNganSach=@iID_MaMucLucNganSach";
            cmd.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);

            String SQL = String.Format("SELECT sum(rSoTienDauNam+rSoTienDieuChinh) as rSoTien, sum(rNgoaiTe_DauNam+rNgoaiTe_DieuChinh) as rNgoaiTe, sTenNgoaiTe_DauNam AS sTenNgoaiTe FROM QLDA_KeHoachVon WHERE {0} GROUP BY sTenNgoaiTe_DauNam", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Row_DuToanQuy_ChungTu(String iID_MaDuToanQuy_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_DuToan_Quy_ChungTu WHERE iTrangThai = 1 AND iID_MaDuToanQuy_QuyetDinh=@iID_MaDuToanQuy_QuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static int CheckExits_DuToan_Quy_ChungTu(String iQuy, String iNamLamViec)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(iID_MaDuToanQuy_QuyetDinh) FROM QLDA_DuToan_Quy_ChungTu WHERE  iTrangThai = 1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int CheckExits_DuToan(object iID_MaDuToanQuy_QuyetDinh)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(iID_MaDuToanQuy_QuyetDinh) FROM QLDA_DuToan_Quy WHERE iID_MaDuToanQuy_QuyetDinh=@iID_MaDuToanQuy_QuyetDinh AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_DanhSach_DuToan_Quy_QuyetDinh(String TuNgay, String DenNgay, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
          
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
            String SQL = String.Format("SELECT * FROM QLDA_DuToan_Quy_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_DuToan_Quy_QuyetDinh_Count(String TuNgay, String DenNgay, String sNguoiDung)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
         
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
            String SQL = String.Format("SELECT COUNT(*) FROM QLDA_DuToan_Quy_ChungTu WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }
        public static DataTable DT_Quy_QuyetToan(Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaQuy", typeof(String));
            vR.Columns.Add("TenQuy", typeof(String));

            DataRow R = vR.NewRow();
            R["MaQuy"] = "1";
            R["TenQuy"] = "Quý I";
            vR.Rows.InsertAt(R, 1);

            DataRow R1 = vR.NewRow();
            R1["MaQuy"] = "2";
            R1["TenQuy"] = "Quý II";
            vR.Rows.InsertAt(R1, 2);

            DataRow R2 = vR.NewRow();
            R2["MaQuy"] = "3";
            R2["TenQuy"] = "Quý III";
            vR.Rows.InsertAt(R2, 3);

            DataRow R4 = vR.NewRow();
            R4["MaQuy"] = "4";
            R4["TenQuy"] = "Quý IV";
            vR.Rows.InsertAt(R4, 4);

            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaQuy"] = "-1";
                vR.Rows[0]["TenQuy"] = "-- Quý --";
            }
            return vR;
        }
        public  static string getQuy(string iQuy)
        {
            if (iQuy=="1")
            {
                return "Quý I";
            }
            else if (iQuy == "2")
            {
                return "Quý II";
            }
            else if (iQuy == "3")
            {
                return "Quý III";
            }
            else if (iQuy == "4")
            {
                return "Quý VI";
            }
            else
            {
                return iQuy;
            }
        }
        public static int Insert_DuToanQuy_By_KHV(String iID_MaDuToanQuy_QuyetDinh, String dNgayLap, String iQuy, int iNamLamViec, String sMaNguoiDung, String IP)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL =
                @"insert QLDA_DuToan_Quy(iID_MaDuToanQuy_QuyetDinh,dNgayLap,iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,iQuy,
iID_MaDanhMucDuAn, iID_MaDanhMucDuAn_Cha, sXauNoiMa_DuAn,bLaHangCha_DuAn,sDeAn,
sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaMucLucNganSach,
sXauNoiMa,bLaHangCha,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sID_MaNguoiDungTao,dNgaySua,sIPSua,sID_MaNguoiDungSua)
select @iID_MaDuToanQuy_QuyetDinh,@dNgayLap, iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,@iQuy,
iID_MaDanhMucDuAn, iID_MaDanhMucDuAn_Cha, sXauNoiMa_DuAn,bLaHangCha_DuAn,sDeAn,
sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaMucLucNganSach,
sXauNoiMa,bLaHangCha,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sID_MaNguoiDungTao,GETDATE(),@sIPSua,sID_MaNguoiDungTao from (
select distinct iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,
iID_MaDanhMucDuAn, iID_MaDanhMucDuAn_Cha, sXauNoiMa_DuAn,bLaHangCha_DuAn,sDeAn,
sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,iID_MaMucLucNganSach,
sXauNoiMa,bLaHangCha,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sID_MaNguoiDungTao
 from QLDA_KeHoachVon where iTrangThai=1 and iNamLamViec=@iNamLamViec and sID_MaNguoiDungTao=@sMaNguoiDung) a
order by sXauNoiMa_DuAn, sXauNoiMa";
            //from QLDA_DuToan_Nam where iTrangThai=1 and iNamLamViec=@iNamLamViec and sID_MaNguoiDungTao=@sMaNguoiDung";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgayLap));
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@sIPSua", IP);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            vR = Connection.UpdateDatabase(cmd);
            return vR;
        }
    }
}