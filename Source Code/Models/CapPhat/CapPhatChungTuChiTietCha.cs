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
    public class CapPhatChungTuChiTietCha
    {
        /// <summary>
        /// Lấy mã trạng thái (nếu) sau khi được duyệt của chứng từ
        /// </summary>
        /// <param name="sMaND">Mã người dùng</param>
        /// <param name="siID_MaCapPhat">Mã chứng từ cấp phát</param>
        /// <returns></returns>
        public static int LayMaTrangThaiDuyetTrinhDuyet(String sMaND, String siID_MaCapPhat)
        {
            int vR = -1;
            DataTable dt = CapPhat_ChungTuModels.LayChungTuCapPhat(siID_MaCapPhat);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeCapPhat, sMaND, iID_MaTrangThaiDuyet))
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
        ///  Lấy mã trạng thái (nếu) sau khi bị từ chối của chứng từ
        /// </summary>
        /// <param name="sMaND"></param>
        /// <param name="siID_MaCapPhat"></param>
        /// <returns></returns>
        public static int LayMaTrangThaiDuyetTuChoi(String sMaND, String siID_MaCapPhat)
        {
            int vR = -1;
            DataTable dt = CapPhat_ChungTuModels.LayChungTuCapPhat(siID_MaCapPhat);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeCapPhat, sMaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM CP_CapPhatChiTiet WHERE iID_MaCapPhat=@iID_MaCapPhat AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaCapPhat", siID_MaCapPhat);
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
        /// Lay danh sach tuy chinh chung tu chi tiet cap phat
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachTuyChinhCapPhat()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["MaLoai"] = "TatCa";
            dr["sTen"] = "Tất cả mục lục ngân sách";
            dt.Rows.InsertAt(dr, 0);
            dr = dt.NewRow();
            dr["MaLoai"] = "CapPhat";
            dr["sTen"] = "Hiện dữ liệu cấp phát đã nhập";
            dt.Rows.InsertAt(dr, 1);
            dr = dt.NewRow();
            dr["MaLoai"] = "ChuaCapPhat";
            dr["sTen"] = "Hiện dữ liệu cấp phát chưa nhập";
            dt.Rows.InsertAt(dr, 2);
            dt.Dispose();
            return dt;
        }
    }
}