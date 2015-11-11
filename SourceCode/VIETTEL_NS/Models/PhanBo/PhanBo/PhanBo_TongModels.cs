using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;

namespace VIETTEL.Models
{
    public class PhanBo_TongModels
    {

        public static DataTable Get_dtPhanBoTong_ChiTiet(String iID_MaPhanBo, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand();
            String strDKTruongTien = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "<>0 OR ") + "<>0";
            String DKThem = "PB_PhanBoChiTiet.iID_MaDonVi=@iID_MaDonVi";
            DKThem += String.Format(" OR (bLaHangCha=0 AND ({0}))", strDKTruongTien);
            String DK = String.Format("PB_PhanBoChiTiet.iID_MaPhanBo IN (SELECT iID_MaPhanBo FROM PB_PhanBo_PhanBo WHERE iID_MaPhanBoTong=@iID_MaPhanBo) AND ({0})", DKThem);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }

            String SQL = String.Format(@"SELECT PB_PhanBo.sTienToChungTu+'-'+convert(varchar,PB_PhanBo.iSoChungTu) AS sSoCT,Convert(varchar,PB_PhanBo.dNgayChungTu,103) AS dNgayChungTu,PB_PhanBoChiTiet.* FROM PB_PhanBoChiTiet 
            INNER JOIN PB_PhanBo ON PB_PhanBo.iID_MaPhanBo=PB_PhanBoChiTiet.iID_MaPhanBo
            WHERE {0} ORDER BY {1}, iID_MaDonVi, sMaCongTrinh", DK, MucLucNganSachModels.strDSTruongSapXep);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", PhanBo_Tong_BangDuLieu.iID_MaDonViChoPhanBo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Từ chối phân bổ
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <param name="MaND"></param>
        /// <param name="sIPSua"></param>
        public static void TuChoi(String iID_MaPhanBoTong,String iID_MaPhanBo, String MaND,String sIPSua)
        {
            SqlCommand cmd;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = PhanBo_PhanBoChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaPhanBo);
            if (iID_MaTrangThaiDuyet_TuChoi > 0)
            {
                DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
                String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
                dtTrangThaiDuyet.Dispose();

                //Cập nhập trường sSua
                PhanBo_DuyetPhanBoModels.CapNhapLaiTruong_sSua(iID_MaPhanBo);

                ///Update trạng thái cho bảng chứng từ
                PhanBo_PhanBoModels.Update_iID_MaTrangThaiDuyet(iID_MaPhanBo, iID_MaTrangThaiDuyet_TuChoi, false, MaND, sIPSua);

                ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
                String MaDuyetPhanBo = PhanBo_PhanBoModels.InsertDuyetPhanBo(iID_MaPhanBo, NoiDung, NoiDung, sIPSua);

                ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
                
                cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@iID_MaDuyetPhanBoCuoiCung", MaDuyetPhanBo);
                PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBo, cmd.Parameters, MaND, sIPSua);
                cmd.Dispose();
            }
            //update trạng thái từ chối
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet_TuChoi);
            PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBoTong, cmd.Parameters, MaND, sIPSua);
            cmd.Dispose();
            //return RedirectToAction("Index", "PermitionMessage");
        }
        public static void TrinhDuyet(String iID_MaPhanBoTong, String MaND, String sIPSua)
        {
            //Xác định trạng thái duyệt tiếp theo
            DataTable dt = Get_DanhSachPhanBo(iID_MaPhanBoTong);
            String iID_MaPhanBo = "";
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            SqlCommand cmd;
            for (int i = 0; i < dt.Rows.Count;i++)
            {
                iID_MaPhanBo = Convert.ToString(dt.Rows[i]["iID_MaPhanBo"]);
                iID_MaTrangThaiDuyet_TrinhDuyet = PhanBo_PhanBoChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaPhanBo);

                DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
                String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
                dtTrangThaiDuyet.Dispose();

                ///Update trạng thái cho bảng chứng từ
                PhanBo_PhanBoModels.Update_iID_MaTrangThaiDuyet(iID_MaPhanBo, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, sIPSua);

                ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
                String MaDuyetPhanBo = PhanBo_PhanBoModels.InsertDuyetPhanBo(iID_MaPhanBo, NoiDung, MaND, sIPSua);

                ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
                cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@iID_MaDuyetPhanBoCuoiCung", MaDuyetPhanBo);
                PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBo, cmd.Parameters, MaND, sIPSua);
                cmd.Dispose();

                int iID_MaTrangThaiTuChoi = PhanBo_PhanBoChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaPhanBo);
            }
            
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBoTong, cmd.Parameters, MaND, sIPSua);
            cmd.Dispose();
        }
        /// <summary>
        /// update trạng thái trình duyệt của phân bổ tổng khi tất cả các phân bổ con ở trạng thái trình duyệt
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <param name="MaND"></param>
        /// <param name="sIPSua"></param>
        public static void UpdateTrinhDuyet(String iID_MaPhanBo, String MaND, String sIPSua)
        {
            //kiểm tra có phân bổ tổng hay không
            String iID_MaPhanBoTong=LayMaPhanBoTong(iID_MaPhanBo);
            if (String.IsNullOrEmpty(iID_MaPhanBoTong) == false)
            {
                DataTable dtPB = Get_DanhSachPhanBo(iID_MaPhanBoTong);
                String MaPhanBo = "";
                int iID_MaTrangThaiDuyet = 0;
                Boolean DuyetOK=true;
                for (int i = 0; i < dtPB.Rows.Count; i++)
                {
                    MaPhanBo = Convert.ToString(dtPB.Rows[i]["iID_MaPhanBo"]);
                    iID_MaTrangThaiDuyet = LayMaTrangThaiDuyet(MaPhanBo);
                    if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHePhanBo, iID_MaTrangThaiDuyet)==false)
                    {
                        DuyetOK = false;
                        break;
                    }
                }
                if (DuyetOK)
                {
                    SqlCommand cmd;
                    cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBoTong, cmd.Parameters, MaND, sIPSua);
                    cmd.Dispose();
                }
            }
        }

        public static int LayMaTrangThaiDuyet(String iID_MaPhanBo)
        {
            DataTable dt = PhanBo_PhanBoModels.GetPhanBo(iID_MaPhanBo);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            return iID_MaTrangThaiDuyet;
        }

        public static DataTable Get_DanhSachPhanBo(String iID_MaPhanBoTong)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PB_PhanBo_PhanBo WHERE iID_MaPhanBoTong=@iID_MaPhanBoTong");
            cmd.Parameters.AddWithValue("@iID_MaPhanBoTong", iID_MaPhanBoTong);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static String LayMaPhanBoTong(String iID_MaPhanBo)
        {
            String SQL = "SELECT iID_MaPhanBoTong FROM PB_PhanBo_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            String iID_MaPhanBoTong = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return iID_MaPhanBoTong;                
        }
    }
}