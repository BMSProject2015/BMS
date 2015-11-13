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
    public class PhanBo_ChiTieuChiTietModels
    {
        /// <summary>
        /// Thêm chi tiết cho 1 chỉ tiêu
        ///     - Thêm tất cả mục lục ngân sách mà người dùng được phép làm việc cho chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        public static void ThemChiTiet(String iID_MaChiTieu, String MaND, String IPSua)
        {
            //nghiepnc bo ten cong trinh
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            //Lấy các mục lục ngân sách của người dùng
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            DataTable dtChiTieu = PhanBo_ChiTieuModels.GetChiTieu(iID_MaChiTieu);
            DateTime dNgayChungTu = Convert.ToDateTime(dtChiTieu.Rows[0]["dNgayChungTu"]);
            int iThangCT = dNgayChungTu.Month;
            int iNamLamViec = Convert.ToInt32(dtChiTieu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChiTieu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChiTieu.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtChiTieu.Rows[0]["bChiNganSach"]);

            Bang bang = new Bang("PB_ChiTieuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", dtChiTieu.Rows[0]["iID_MaDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", dtChiTieu.Rows[0]["iID_MaChiTieu"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChiTieu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChiTieu.Rows[0]["iID_MaTrangThaiDuyet"]);            
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChiTieu.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChiTieu.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChiTieu.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChiTieu.Rows[0]["bChiNganSach"]);


            for (int l = 0; l < arrDSTruongTien.Length; l++)
            {
                String tgTenTruong = "@" + arrDSTruongTien[l];
                String tgTenTruong_DuToan;
                Object tgGiaTri;
                if (arrDSTruongTien[l].StartsWith("s"))
                {
                    tgTenTruong_DuToan = tgTenTruong;
                    tgGiaTri = "";
                }
                else
                {
                    tgTenTruong_DuToan = tgTenTruong + "_DuToan";
                    tgGiaTri = 0;
                }
                bang.CmdParams.Parameters.AddWithValue(tgTenTruong_DuToan, tgGiaTri);
            }
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dt.Rows[i]["sLNS"]);

                DataTable dtMucLucNganSach = NganSach_HamChungModels.DT_MucLucNganSach_sLNS(sLNS);
                //Lấy tổng dự toán đã duyệt của sLNS trong iNamLamViec
                DataTable dtDuToan = DuToan_ChungTuChiTietModels.LayTongDuToan(sLNS, iNamLamViec, Convert.ToString(dNgayChungTu), iID_MaNguonNganSach, iID_MaNamNganSach, bChiNganSach, iID_MaChiTieu);
                //Chỉ số csMin để xác định chỉ số nhỏ nhất đã được duyệt qua
                int csMin = 0;
                for (int j = 0; j < dtMucLucNganSach.Rows.Count; j++)
                {
                    Boolean DaThem = false;
                    //Điền thông tin của mục lục ngân sách
                    NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLucNganSach.Rows[j], bang.CmdParams.Parameters);

                    //Lấy mã mục lục ngân sách
                    String iID_MaMucLucNganSach = Convert.ToString(dtMucLucNganSach.Rows[j]["iID_MaMucLucNganSach"]);
                    Boolean okDaQua = false;
                    for (int k = csMin; k < dtDuToan.Rows.Count; k++)
                    {
                        if (Convert.ToString(dtDuToan.Rows[k]["iID_MaMucLucNganSach"]) == iID_MaMucLucNganSach)
                        {
                            if (k == csMin) csMin++;
                            okDaQua = true;
                            for (int l = 0; l < arrDSTruongTien.Length; l++)
                            {
                                String tgTenTruong = "@" + arrDSTruongTien[l];
                                String tgTenTruong_DuToan;
                                Object tgGiaTri;
                                if (arrDSTruongTien[l].StartsWith("s"))
                                {
                                    tgTenTruong_DuToan = tgTenTruong;
                                    tgGiaTri = dtDuToan.Rows[k][arrDSTruongTien[l]];
                                }
                                else
                                {
                                    tgTenTruong_DuToan = tgTenTruong + "_DuToan";
                                    tgGiaTri = dtDuToan.Rows[k]["Sum" + arrDSTruongTien[l]];
                                }
                                bang.CmdParams.Parameters[tgTenTruong_DuToan].Value = tgGiaTri;
                            }
                            DaThem = true;
                            bang.Save(); 
                        }
                        else
                        {
                            if (okDaQua)
                            {
                                break;
                            }
                        }
                    }

                    if (DaThem == false)
                    {
                        for (int l = 0; l < arrDSTruongTien.Length; l++)
                        {
                            String tgTenTruong = "@" + arrDSTruongTien[l];
                            String tgTenTruong_DuToan;
                            Object tgGiaTri;
                            if (arrDSTruongTien[l].StartsWith("s"))
                            {
                                tgTenTruong_DuToan = tgTenTruong;
                                tgGiaTri = "";
                            }
                            else
                            {
                                tgTenTruong_DuToan = tgTenTruong + "_DuToan";
                                tgGiaTri = 0;
                            }
                            bang.CmdParams.Parameters[tgTenTruong_DuToan].Value = tgGiaTri;
                        }
                        bang.Save();
                    }
                }
                dtMucLucNganSach.Dispose();
                dtDuToan.Dispose();
            }
            dt.Dispose();
            dtChiTieu.Dispose();
            //Thêm phân bổ cho đơn vị 'Chờ phân bổ'
            PhanBo_PhanBoChiTietModels.ThemPhanBoChoDonVi(iID_MaChiTieu, PhanBo_PhanBoNganh_BangDuLieu.iID_MaDonViChoPhanBo, MaND, IPSua);            
            //Tạo phân bổ ngành cho đơn vị
          //  DataTable dtDV = PhanBo_PhanBoChiTietModels.Get_dtDonViDuToan(MaND, iNamLamViec.ToString(),iID_MaNguonNganSach.ToString(),iID_MaNamNganSach.ToString());
             DataTable dtDV = PhanBo_PhanBoChiTietModels.Get_DanhSachDonViDuToanDuocChon(iID_MaChiTieu);
            for (int i = 0; i < dtDV.Rows.Count; i++)
            {
                String MaDV = Convert.ToString(dtDV.Rows[i]["iID_MaDonVi"]);
                PhanBo_PhanBoChiTietModels.ThemPhanBoChoDonVi(iID_MaChiTieu, MaDV, MaND, IPSua);
            }

        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChiTieu)
        {
            int vR = -1;
            DataTable dt = PhanBo_ChiTieuModels.GetChiTieu(iID_MaChiTieu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChiTieu)
        {
            int vR = -1;
            DataTable dt = PhanBo_ChiTieuModels.GetChiTieu(iID_MaChiTieu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtChiTieuChiTiet(String iID_MaChiTieu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChiTieu=@iID_MaChiTieu";
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);

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

            SQL = String.Format("SELECT * FROM PB_ChiTieuChiTiet WHERE {0} ORDER BY sXauNoiMa", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}