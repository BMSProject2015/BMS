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
    public class Luong_DanhMucPhuCapModels
    {
        public static NameValueCollection LayThongTinDanhMucPhuCap(String iID_MaPhuCap)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucPhuCap(iID_MaPhuCap);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="iID_MaPhuCap"></param>
        /// <returns></returns>
        public static DataTable Get_ChiTietDanhMucPhuCap(String iID_MaPhuCap)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM L_DanhMucPhuCap WHERE iTrangThai=1 AND iID_MaPhuCap=@iID_MaPhuCap";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhuCap", iID_MaPhuCap);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtDanhMucPhuCap(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucPhuCap WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTenPhuCap", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucPhuCap()
        {

            String SQL = "SELECT COUNT(*) FROM L_DanhMucPhuCap WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

        public static DataTable get_dtPhuCapLuonCo()
        {
            String SQL = String.Format("SELECT * FROM L_DanhMucPhuCap WHERE iTrangThai=1 AND bLuonCo=1 ORDER BY iID_MaPhuCap");
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtDanhMucPhuCap_MucLucNganSach(String iID_MaPhuCap="")
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            if (String.IsNullOrEmpty(iID_MaPhuCap) == false)
            {
                DK = " AND sMaTruong=@sMaTruong";
                cmd.Parameters.AddWithValue("@sMaTruong", "PhuCap_" + iID_MaPhuCap);
            }
            else
            {
                DK = " AND sMaTruong NOT LIKE @sMaTruong";
                cmd.Parameters.AddWithValue("@sMaTruong", "PhuCap_%");
            }
            String SQL = String.Format("SELECT * FROM L_DanhMucTruong_MucLucNganSach WHERE iTrangThai=1 {0} ORDER BY sXauNoiMa,sMaTruong",DK);
            cmd.CommandText = SQL;            
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String Get_CongThuc(String MaTruongPhuCap)
        {
            
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sCongThuc FROM L_DanhMucPhuCap WHERE iTrangThai=1 AND sMaTruongSoTien_BangLuong=@sMaTruongSoTien_BangLuong";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaTruongSoTien_BangLuong", MaTruongPhuCap);
            String vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }


        public static NameValueCollection Get_dataPhuCapMucLuc(NameValueCollection data, String iID_MaPhuCap)
        {

            String SQL = "SELECT ";
           SQL +=" iID_MaDanhMucPhuCap_MucLucNganSach,iID_MaMucLucNganSach,sXauNoiMa";
           SQL += ",sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sMaTruong,iID_MaNgachLuong,sTenNgachLuong,iID_MaBacLuong,sTenBacLuong";
           SQL += " FROM L_DanhMucTruong_MucLucNganSach WHERE iTrangThai=1 AND sMaTruong=@sMaTruong";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaTruong","PhuCap_"+iID_MaPhuCap);
            DataTable dt=Connection.GetDataTable(cmd);
            cmd.Dispose();
            String colName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            dt.Dispose();
            return data;
        }

        public static void DetailSubmit(String iID_MaPhuCap,String UserID, String IPSua, NameValueCollection Values = null)
        {
            String TenBangChiTiet = "L_DanhMucTruong_MucLucNganSach";
            string idXauMaCacHang = Values["idXauMaCacHang"];
            string idXauMaCacCot = Values["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Values["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Values["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Values["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTuChiTiet;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaChungTuChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaChungTuChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang.MaNguoiDungSua = UserID;
                        bang.IPSua = IPSua;
                        bang.Save();
                    }
                }
                else
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        iID_MaChungTuChiTiet = arrMaHang[i];
                        if (iID_MaChungTuChiTiet == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }
                        bang.MaNguoiDungSua = UserID;
                        bang.IPSua = IPSua;
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                String Truong = "@" + arrMaCot[j];
                                if (arrMaCot[j].StartsWith("b"))
                                {
                                    //Nhap Kieu checkbox
                                    if (arrGiaTri[j] == "1")
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                    }
                                    else
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                    }
                                }
                                else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                {
                                    //Nhap Kieu so
                                    if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                    }
                                }
                                else
                                {
                                    //Nhap kieu xau
                                    bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                }
                            }
                        }
                        if (bang.CmdParams.Parameters.IndexOf("@sMaTruong") < 0)                        
                        {
                            bang.CmdParams.Parameters.AddWithValue("@sMaTruong","PhuCap_" + iID_MaPhuCap);
                        }
                        bang.Save();
                    }
                }
            }
        }
    }
}