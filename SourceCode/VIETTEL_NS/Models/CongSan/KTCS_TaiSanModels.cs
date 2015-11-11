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
    public class KTCS_TaiSanModels
    {
        
        public static NameValueCollection LayThongTin(String iLoai, String iID_MaNhomTaiSan)
        {
            NameValueCollection Data = new NameValueCollection();
            Data["iLoai"] = iLoai;
            Data["iID_MaNhomTaiSan"] = iID_MaNhomTaiSan;
            return Data;
        }
        public static NameValueCollection LayThongTinTaiSanChiTiet(String iID_MaTaiSan)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_dtTaiSanChiTiet(iID_MaTaiSan);
            DataTable dtTaiSan = null;
            String colName = "";
            if (dt != null && dt.Rows.Count>0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    if (dt.Rows.Count > 0)
                    {
                        Data[colName] = Convert.ToString(dt.Rows[0][i]);
                    }
                    else
                    {
                        Data[colName] = "";
                    }
                }
                dt.Dispose();
                dtTaiSan = Get_dtTaiSan(iID_MaTaiSan);
                
                //Data["rNguyenGiaBanDau"] = Convert.ToString(dt.Rows[0]["rNguyenGia"]);
                //Data["iNamSX"] = Convert.ToString(dt.Rows[0]["iNamSX"]);
                //Data["sNuocSX"] = Convert.ToString(dt.Rows[0]["sNuocSX"]);
                //if (dt.Rows[0]["dNgayMua"] != DBNull.Value)
                //{
                //    Data["sThoiGianDuaVaoSuDung"] = CommonFunction.LayXauNgay(Convert.ToDateTime(dt.Rows[0]["dNgayDuaVaoKhauHao"]));
                //}
                if (dt.Rows[0]["dNgayMua"] != DBNull.Value)
                {
                    Data["dNgayMua"] = CommonFunction.LayXauNgay(Convert.ToDateTime(dt.Rows[0]["dNgayMua"]));
                }
                if (dt.Rows[0]["dNgayDuaVaoKhauHao"] != DBNull.Value)
                {
                    Data["dNgayDuaVaoKhauHao"] = CommonFunction.LayXauNgay(Convert.ToDateTime(dt.Rows[0]["dNgayDuaVaoKhauHao"]));
                }
                DataTable dtDV = DonViModels.Get_dtDonVi(Convert.ToString(dtTaiSan.Rows[0]["iID_MaDonVi"]));
                if (dtDV.Rows.Count > 0)
                {
                    Data["sTenDonVi"] = DonViModels.Get_TenDonVi(Convert.ToString(dtTaiSan.Rows[0]["iID_MaDonVi"]));
                    Data["iID_MaDonVi"] = Convert.ToString(dtTaiSan.Rows[0]["iID_MaDonVi"]);
                    Data["sLoaiHinhDonVi"] = CommonFunction.LayTenDanhMuc(Convert.ToString(dtDV.Rows[0]["iID_MaLoaiDonVi"]));
                    dtDV.Dispose();
                }
                dtTaiSan.Dispose();
            }
            dt.Dispose();
            
            return Data;
        }
        public static DataTable Get_dtTaiSanChiTiet(String iID_MaTaiSan)
        {
            String SQL = String.Format("SELECT *,CONVERT(nvarchar(10),dNgayMua,103) as dNgayMua1,CONVERT(nvarchar(10),dNgayDuaVaoKhauHao,103) as dNgayDuaVaoKhauHao1 FROM KTCS_TaiSanChiTiet WHERE iTrangThai=1 AND iID_MaTaiSan= @iID_MaTaiSan ORDER BY dNgayTao");
           SqlCommand cmd = new SqlCommand(SQL);
           cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
           DataTable dt = Connection.GetDataTable(cmd);
           cmd.Dispose();
           return dt;
        }
        public static DataTable Get_dtTaiSan(String iID_MaTaiSan)
        {
            String SQL = String.Format("SELECT * FROM KTCS_TaiSan WHERE iTrangThai=1 AND iID_MaTaiSan= @iID_MaTaiSan ORDER BY dNgayTao");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_Table_TaiSan(String iLoai, String iID_MaNhomTaiSan, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iLoai == null) iLoai = "";
            if (iID_MaNhomTaiSan == null) iID_MaNhomTaiSan = "";

            DK = "iTrangThai=1";

            if (iLoai != "")
            {
                DK += " AND iLoaiTS=@iLoaiTS";
                cmd.Parameters.AddWithValue("@iLoaiTS", iLoai);
            }
            if (iID_MaNhomTaiSan != "")
            {
                DK += " AND iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
                cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            }
            
            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = "iNamLamViec,iID_MaTaiSan";
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

            SQL = String.Format("SELECT * FROM KTCS_TaiSan WHERE {0} ORDER BY iID_MaTaiSan,dNgayTao", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable DT_NhomTaiSan()
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaLoaiTS", typeof(String));
            vR.Columns.Add("sTen", typeof(String));
            DataRow Row;

            Row = vR.NewRow();
            vR.Rows.Add(Row);
            Row[0] = Convert.ToString(1);
            Row[1] = Convert.ToString("Đất đai");

            Row = vR.NewRow();
            vR.Rows.Add(Row);
            Row[0] = Convert.ToString(2);
            Row[1] = Convert.ToString("Nhà cửa");

            Row = vR.NewRow();
            vR.Rows.Add(Row);
            Row[0] = Convert.ToString(3);
            Row[1] = Convert.ToString("Ôtô");

            Row = vR.NewRow();
            vR.Rows.Add(Row);
            Row[0] = Convert.ToString(4);
            Row[1] = Convert.ToString("Tài sản trên 500 triệu");

            vR.Rows.InsertAt(vR.NewRow(), 0);
            vR.Rows[0]["MaLoaiTS"] = "";
            vR.Rows[0]["sTen"] = "-- Loại tài sản --";
            return vR;
        }

        public static DataTable ddl_DanhMucNhomTaiSan(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaNhomTaiSan, sTen " +
                    "FROM KTCS_NhomTaiSan WHERE iTrangThai = 1 ORDER By iID_MaNhomTaiSan");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaNhomTaiSan"] = "";
                R["sTen"] = "--- Danh mục loại tài sản ---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        public static Boolean KiemTra_MaTaiSan(String iID_MaTaiSan)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KTCS_TaiSan WHERE iTrangThai=1 AND iID_MaTaiSan=@iID_MaTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            if (String.IsNullOrEmpty(iID_MaTaiSan) == false && Convert.ToInt32(Connection.GetValue(cmd, 0)) == 0)
            {
                return true;
            }
            return false;
        }

        public static Boolean KiemTraCoChiTietTaiSan(String iID_MaTaiSan)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KTCS_TaiSanChiTiet WHERE iTrangThai=1 AND iID_MaTaiSan=@iID_MaTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            int vR = 0;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (String.IsNullOrEmpty(iID_MaTaiSan) || vR == 0)
            {                
                return false;
            }
            return true;
            
        }

        public static DataTable LayThongTinNhomTaiSan(String iID_MaTaiSan)
        {
            DataTable vR;
            String iID_MaNhomTaiSan = Convert.ToString(LayThongTinTaiSanChiTiet(iID_MaTaiSan)["iID_MaNhomTaiSan"]);
            vR = KTCS_NhomTaiSanModels.getChiTietTK(iID_MaNhomTaiSan);
            return vR;
        }
        /// <summary>
        /// Lấy mã tài sản từ bảng KTCS_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTuChiTiet"></param>
        /// <returns></returns>
        public static String LayMaTaiSan(String iID_MaChungTuChiTiet)
        {
           String iID_MaTaiSan=Convert.ToString(CommonFunction.LayTruong("KTCS_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaChungTuChiTiet, "iID_MaTaiSan"));
           return iID_MaTaiSan;
        }
        
    }
}