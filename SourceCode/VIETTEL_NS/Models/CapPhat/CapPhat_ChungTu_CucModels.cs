using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Controls;
using System.Collections.Specialized;
using DomainModel.Abstract;


namespace VIETTEL.Models
{
    public class CapPhat_ChungTu_CucModels
    {
        public static NameValueCollection LayThongTin(String iID_MaCapPhat)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetCapPhat(iID_MaCapPhat);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable GetCapPhat(String iID_MaCapPhat)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT *,Day(dNgayCapPhat) as Ngay,MONTH(dNgayCapPhat) as Thang,YEAR(dNgayCapPhat) as Nam FROM CP_CapPhat WHERE iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChungTu(String MaPhongBan, String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, String iDM_MaLoaiCapPhat, Boolean LayTheoMaNDTao=false, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, MaND);
            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (iDM_MaLoaiCapPhat != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != "")
            {
                DK += " AND iDM_MaLoaiCapPhat = @iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoCapPhat = @iSoCapPhat";
                cmd.Parameters.AddWithValue("@iSoCapPhat", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayCapPhat >= @dTuNgayCapPhat";
                cmd.Parameters.AddWithValue("@dTuNgayCapPhat", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayCapPhat <= @dDenNgayCapPhat";
                cmd.Parameters.AddWithValue("@dDenNgayCapPhat", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT * FROM CP_CapPhat WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,iSoCapPhat DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String MaPhongBan = "", String MaND = "", String SoChungTu = "", String TuNgay = "", String DenNgay = "", String iID_MaTrangThaiDuyet = "", String iDM_MaLoaiCapPhat = "", Boolean LayTheoMaNDTao=false)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, MaND);
            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (iDM_MaLoaiCapPhat != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != "")
            {
                DK += " AND iDM_MaLoaiCapPhat = @iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoCapPhat = @iSoCapPhat";
                cmd.Parameters.AddWithValue("@iSoCapPhat", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayCapPhat >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayCapPhat <= @dDenNgayCapPhat";
                cmd.Parameters.AddWithValue("@dDenNgayCapPhat", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT COUNT(*) FROM CP_CapPhat WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static void Delete_ChungTu(String iID_MaCapPhat, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng CP_CapPhatChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("UPDATE CP_CapPhatChiTiet SET iTrangThai=0 WHERE iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng CP_CapPhat
            Bang bang = new Bang("CP_CapPhat");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaCapPhat;
            bang.Delete();

        }

        public static Boolean UpdateRecord(String iID_MaCapPhat, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("CP_CapPhat");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaCapPhat;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }

        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaCapPhat, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaCapPhat, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng CP_CapPhatChiTiet            
            String SQL = "UPDATE CP_CapPhatChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaCapPhat=@iID_MaCapPhat";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE CP_CapPhatChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaCapPhat=@iID_MaCapPhat";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }

        public static String InsertDuyetChungTu(String iID_MaCapPhat, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("CP_DuyetCapPhat");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaCapPhat)
        {
            String iID_MaDuyetCapPhat;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetCapPhatCuoiCung FROM CP_CapPhat WHERE iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            iID_MaDuyetCapPhat = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM CP_CapPhatChiTiet WHERE iID_MaCapPhat=@iID_MaCapPhat AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE CP_DuyetCapPhat SET sSua=@sSua WHERE iID_MaDuyetCapPhat=@iID_MaDuyetCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaDuyetCapPhat", iID_MaDuyetCapPhat);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        //Lấy danh sách đơn vị cấp phát dùng cho thông tri cấp phát
        public static DataTable dt_DonViCapPhat(String iID_MaCapPhat)
        {
            String SQL = "SELECT sTen,CP.iID_MaDonVi FROM (SELECT Distinct(iID_MaDonVi) FROM CP_CapPhatChiTiet ";
            SQL += " WHERE iID_MaCapPhat=@iID_MaCapPhat";
            SQL += " AND iID_MaDonVi <> '99' ";
            SQL += " AND iID_MaDonVi<>'') CP";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi ) NS_DonVi ON CP.iID_MaDonVi=NS_DonVi.iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}