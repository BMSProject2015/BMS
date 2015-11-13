using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Models
{
    public class KeToanTongHop_ChungTuModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ thu nộp ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu(iID_MaChungTu);
            String colName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            if (dt != null) dt.Dispose();
            return Data;
        }

        /// <summary>
        /// Lấy DataTable thông tin của một chứng từ thu nộp ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable GetChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String LayMaChungTu(String sSoChungTu)
        {
            String vR;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTu WHERE iTrangThai=1 AND sSoChungTu=@sSoChungTu AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            vR = Connection.GetValueString(cmd,Guid.Empty.ToString());
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy số chứng từ lớn nhất trong bảng KT_ChungTu
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChungTu_GoiY(String iNamLamViec)
        {
            
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT Max(Convert(int,sSoChungTu)) FROM KT_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chứng từ ghi sổ cuối cùng trong bảng KT_ChungTu
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChungTu_CuoiCung(String iNamLamViec)
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT Max(Convert(int,sSoChungTu)) FROM KT_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Kiểm tra tính hợp lệ của sSoChungTu
        /// </summary>
        /// <returns></returns>
        public static Boolean KiemTra_sSoChungTu(String sSoChungTu,String iNamLamViec)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTu WHERE iTrangThai=1 AND sSoChungTu=@sSoChungTu AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(sSoChungTu) == false && Convert.ToInt32(Connection.GetValue(cmd, 0)) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Kiểm tra tính hợp lệ của sSoChungTu có bị trùng ko?
        /// </summary>
        /// <returns></returns>
        public static Boolean KiemTra_sSoChungTu_Trung(String iID_MaChungTu, String sSoChungTu, String iNamLamViec)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTu WHERE sSoChungTu=@sSoChungTu AND iTrangThai=1 AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            Int32 vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (String.IsNullOrEmpty(sSoChungTu) == false && vR == 0)
            {
                return true;
            }
            else
            {
                cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1 ");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Int32 vR1 = Convert.ToInt32(Connection.GetValue(cmd, 0));
                cmd.Dispose();
                if (String.IsNullOrEmpty(iID_MaChungTu) == false && vR1 == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public static Boolean KiemTra_iID_MaChungTu_Trung(String iID_MaChungTu)
        {
            SqlCommand cmd =
                new SqlCommand("SELECT COUNT(*) FROM KT_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Int32 vR1 = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (String.IsNullOrEmpty(iID_MaChungTu) == false && vR1 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng KT_ChungTu, Chỉ có TroLyTongHop mới được quyền thêm
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="data"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertRecord(String iID_MaChungTu, NameValueCollection data, String MaND, String IPSua)
        {
            String sSoChungTu = data["sSoChungTu"];
            sSoChungTu = sSoChungTu.Trim().ToUpper();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            if (KiemTra_sSoChungTu(sSoChungTu, Convert.ToString(R["iNamLamViec"])))
            {
                
                Bang bang = new Bang("KT_ChungTu");
                bang.GiaTriKhoa = iID_MaChungTu;
                bang.DuLieuMoi = true;

                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iThang", R["iThangLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
                if (CommonFunction.IsNumeric(sSoChungTu))
                {
                    bang.CmdParams.Parameters.AddWithValue("@iSoChungTu_GoiY", Convert.ToInt32(sSoChungTu));
                }
                bang.CmdParams.Parameters.AddWithValue("@iNgay", data["iNgay"]);
                bang.CmdParams.Parameters.AddWithValue("@iTapSo", data["iTapSo"]);
                bang.CmdParams.Parameters.AddWithValue("@sDonVi", data["sDonVi"]);
                bang.CmdParams.Parameters.AddWithValue("@sNoiDung", data["sNoiDung"]);
                // bang.Save();

                //Chen  du lieu moi lich su
                String MaChungTuAddNew = Convert.ToString(bang.Save());
                KeToanTongHop_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, MessageModels.sMoiTao, MaND, IPSua);
                if (dtCauHinh != null) dtCauHinh.Dispose();
            }
            return iID_MaChungTu;
        }

        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng KT_ChungTu, Chỉ có TroLyTongHop mới được quyền thêm
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="data"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertChungTuNhan(String ParentID, NameValueCollection data, String MaND, String IPSua, int iLoai)
        {
            String sSoChungTu = data[ParentID + "_sSoChungTu"];
            sSoChungTu = sSoChungTu.Trim().ToUpper();
            String iID_MaChungTuNhan = data[ParentID + "_txt"];
            String iID_MaChungTu = Guid.NewGuid().ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            if (KiemTra_sSoChungTu(sSoChungTu, Convert.ToString(R["iNamLamViec"])))
            {
                
                Bang bang = new Bang("KT_ChungTu");
                bang.GiaTriKhoa = iID_MaChungTu;
                bang.DuLieuMoi = true;

                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iThang", R["iThangLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
                if (CommonFunction.IsNumeric(sSoChungTu))
                {
                    bang.CmdParams.Parameters.AddWithValue("@iSoChungTu_GoiY", Convert.ToInt32(sSoChungTu));
                }
                bang.CmdParams.Parameters.AddWithValue("@iNgay", data[ParentID + "_iNgay"]);
                bang.CmdParams.Parameters.AddWithValue("@sDonVi", data[ParentID + "_sDonVi"]);
                bang.CmdParams.Parameters.AddWithValue("@sNoiDung", data[ParentID + "_" + iID_MaChungTuNhan]);
                String TenTruong = "iID_MaChungTu";
                switch (iLoai)
                {
                    case 0:
                        TenTruong = "iID_MaChungTu";
                        break;
                    case 1:
                        TenTruong = "iID_MaChungTu_KhoBac";
                        break;
                    case 2:
                        TenTruong = "iID_MaChungTu_TienGui";
                        break;
                    case 3:
                        TenTruong = "iID_MaChungTu_TienMat";
                        break;
                }
                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, iID_MaChungTuNhan);
                // bang.Save();

                //Chen  du lieu moi lich su
                String MaChungTuAddNew = Convert.ToString(bang.Save());
                KeToanTongHop_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, MessageModels.sMoiTao, MaND, IPSua);
                if (dtCauHinh != null) dtCauHinh.Dispose();
                InsertChungTuChiTietNhan(ParentID, data, MaChungTuAddNew, iID_MaChungTuNhan, data[ParentID + "_iNgay"], MaND, IPSua, iLoai);
            }
            return iID_MaChungTu;
        }

        public static void InsertChungTuChiTietNhan(String ParentID, NameValueCollection data, String iID_MaChungTu, String iID_MaChungTuNhan, String iNgayCT, String MaND, String IPSua, int iLoai)
        {
            //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTM_ChungTuChiTiet
            DataTable dt = KeToanTongHop_ChungTuChiTietModels.LayDanhSachChiTietChungTuNhan(iID_MaChungTuNhan, iLoai);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            String optNoCo = "";
            String sTaiKhoan = "";
            String sTenTaiKhoan = "";

            String TenTruong = "iID_MaChungTuChiTiet";
            switch (iLoai)
            {
                case 0:
                    TenTruong = "iID_MaChungTuChiTiet";
                    break;
                case 1:
                    {
                        TenTruong = "iID_MaChungTuChiTiet_KhoBac";
                        optNoCo = Convert.ToString(data[ParentID + "_optNoCo"]);
                        sTaiKhoan = Convert.ToString(data[ParentID + "_sTaiKHoan"]);
                        sTenTaiKhoan = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(sTaiKhoan);
                    }
                    break;
                case 2:
                    TenTruong = "iID_MaChungTuChiTiet_TienGui";
                    break;
                case 3:
                    TenTruong = "iID_MaChungTuChiTiet_TienMat";
                    break;
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow R = dt.Rows[i];

                //Insert into data vào bảng: KT_ChungTuChiTiet
                Bang bang = new Bang("KT_ChungTuChiTiet");
                bang.DuLieuMoi = true;
                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iNgayCT", iNgayCT);
                bang.CmdParams.Parameters.AddWithValue("@iThangCT", dtCauHinh.Rows[0]["iThangLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, R["iID_MaChungTuChiTiet"]);
                bang.CmdParams.Parameters.AddWithValue("@iNgay", R["iNgay"]);
                bang.CmdParams.Parameters.AddWithValue("@iThang", R["iThang"]);
                bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", R["sSoChungTuChiTiet"]);
                bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
                //=1 kho bạc .2 tiền gửi, 3 tiền mặt
                if (iLoai != 1)
                {
                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rSoTien"]);                   
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Co"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Co"]);
                    
                }
                else
                {
                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rDTRut"]);
                    switch (optNoCo)
                    {
                        case "tkNo":
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoan);
                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", sTenTaiKhoan);
                            break;
                        case "tkCo":
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoan);
                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", sTenTaiKhoan);
                            break;
                        case "tkTrong":
                            break;
                    }
                
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_Nhan"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_Nhan"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Tra"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Tra"]);
                
                }
                                
                bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);
                bang.Save();
            }
            dt.Dispose();
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
            Bang bang = new Bang("KT_ChungTu");
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
        /// Xóa dữ liệu chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaNguoiDungSua"></param>
        /// <returns></returns>
        public static int Delete_ChungTu(String iID_MaChungTu, String sIPSua, String sID_MaNguoiDungSua)
        {
            DataTable dt = GetChungTu(iID_MaChungTu);

            if (dt != null && dt.Rows.Count > 0)
            {
                int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
                String MaND = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);
                //Phuonglt yêu cầu Tài khoản cấp trợ lý tổng hợp được phép xóa,sửa dữ liệu của tất cả tài khoản thuộc cấp trợ lý phòng ban
                //Tài khoản thuộc cấp trợ lý phòng ban thì tài khoản nào được sửa và xóa dữ liệu của tài khoản đó.
                if ((sID_MaNguoiDungSua == MaND || LuongCongViecModel.KiemTra_TroLyTongHop(sID_MaNguoiDungSua)) && LuongCongViecModel.KiemTra_TrangThaiKhoiTao(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                {
                    //Xóa dữ liệu trong bảng KT_ChungTuChiTiet
                    SqlCommand cmd;
                    cmd = new SqlCommand("UPDATE KT_ChungTuChiTiet SET iTrangThai=0, sIPSua=@sIPSua, sID_MaNguoiDungSua=@sID_MaNguoiDungSua  WHERE iID_MaChungTu=@iID_MaChungTu");
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@sIPSua", sIPSua);
                    cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", sID_MaNguoiDungSua);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    //Xóa dữ liệu trong bảng KT_ChungTu
                    Bang bang = new Bang("KT_ChungTu");
                    bang.MaNguoiDungSua = sID_MaNguoiDungSua;
                    bang.IPSua = sIPSua;
                    bang.GiaTriKhoa = iID_MaChungTu;
                    bang.Delete();

                    dt.Dispose();
                }
            }
            return 1;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng KT_ChungTu, KT_ChungTuChiTiet
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

            //Sửa dữ liệu trong bảng KT_ChungTuChiTiet            
            String SQL = "UPDATE KT_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE KT_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChungTu=@iID_MaChungTu";
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
            Bang bang = new Bang("KT_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }

        public static DataTable Get_DanhSachChungTu(String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(KeToanTongHopModels.iID_MaPhanHe, MaND);
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT * FROM KT_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet, dNgayChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(KeToanTongHopModels.iID_MaPhanHe, MaND);
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM KT_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable List_DanhSachChungTu(int iThang, int iNam, Dictionary<String, String> arrGiaTriTimKiem = null)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iNamLamViec = @iNam AND iThang = @iThang";
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);

            if (arrGiaTriTimKiem != null && String.IsNullOrEmpty(arrGiaTriTimKiem["sSoChungTu"]) == false)
            {
                DK += String.Format(" AND sSoChungTu LIKE @sSoChungTu");
                cmd.Parameters.AddWithValue("@sSoChungTu", '%' + arrGiaTriTimKiem["sSoChungTu"] + '%');
            }
            if (arrGiaTriTimKiem != null && String.IsNullOrEmpty(arrGiaTriTimKiem["iID_MaTrangThaiDuyet"]) == false && HamChung.ConvertToString(arrGiaTriTimKiem["iID_MaTrangThaiDuyet"]) != "-1")
            {
                DK += String.Format(" AND iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet");
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", HamChung.ConvertToString(arrGiaTriTimKiem["iID_MaTrangThaiDuyet"]));
            }
            //String SQL = String.Format("SELECT * FROM KT_ChungTu WHERE {0} ORDER BY iNgay DESC,sSoChungTu DESC", DK);
            String SQL = String.Format("SELECT * FROM KT_ChungTu WHERE {0} ORDER BY iThang, iNgay, CONVERT(int, RTrim(LTrim(sSoChungTu))) ASC", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy nội dung chứng từ ghi sổ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static string getSoChungTuGhiSo(String iID_MaChungTu)
        {
            string strGiaTri = "";
            if (String.IsNullOrEmpty(iID_MaChungTu) == false)
            {
                DataTable dt = null;
                dt = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
                if (dt.Rows.Count > 0)
                {
                    
                    DataRow dr = dt.Rows[0];
                    String KT_ChiTiet = "";
                    if (dr["iID_MaChungTu_TienMat"] != DBNull.Value) KT_ChiTiet = " - TM";
                    if (dr["iID_MaChungTu_TienGui"] != DBNull.Value) KT_ChiTiet = " - TG";
                    if (dr["iID_MaChungTu_KhoBac"] != DBNull.Value) KT_ChiTiet = " - KB";
                    strGiaTri = "Số: " + HamChung.ConvertToString(dr["sSoChungTu"]) + " - " + HamChung.ConvertToString(dr["sNoiDung"]) +
                        " Ngày: " + HamChung.ConvertToString(dr["iNgay"]) + " Tháng " + HamChung.ConvertToString(dr["iThang"]) + KT_ChiTiet;
                }
                if (dt != null) dt.Dispose();
            }
            return strGiaTri;
        }

        public static Boolean TrinhDuyetChungTu(String iID_MaChungTu, String MaND, String IPSua)
        {
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = -1;
            iID_MaTrangThaiDuyet_TrinhDuyet = KeToanTongHop_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return false;
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            KeToanTongHop_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, IPSua);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = KeToanTongHop_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, IPSua);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            KeToanTongHop_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();
            return true;
        }

        public static Boolean TuChoiChungTu(String iID_MaChungTu, String MaND, String IPSua)
        {
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = KeToanTongHop_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return false;
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            KeToanTongHop_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            KeToanTongHop_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, IPSua);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = KeToanTongHop_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, IPSua);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            KeToanTongHop_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();
            return true;
        }
        /// <summary>
        /// Lấy danh sách lịch sử chứng từ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable getLichSuChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM KT_DuyetChungTu WHERE iID_MaChungTu=@iID_MaChungTu ORDER BY dNgayTao DESC");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static int InsertChungTu_By_CTGS(String ParentID, NameValueCollection data, String MaND, String IPSua, String sSoChungTu_SaoChep)
        {   
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            String iNamLamViec = Convert.ToString(R["iNamLamViec"]);
            String iThangLamViec = Convert.ToString(R["iThangLamViec"]);
            String iDisplay = data[ParentID + "_iDisplay"];
            String sSoChungTu = data[ParentID + "_sSoChungTu"];

            String iDonVi = data[ParentID + "_iDonVi"];
            String sDonVi_Sua = data[ParentID + "_sDonVi_Sua"];

            if (iDisplay=="on")
            {
                String MaChungTuAddNew = Insert_SaoChep_CTGS(sSoChungTu, sSoChungTu_SaoChep, iThangLamViec, Convert.ToInt32(iNamLamViec), MaND,
                                                             IPSua, iDonVi, sDonVi_Sua);
                Insert_SaoChep_CT(MaChungTuAddNew, sSoChungTu, sSoChungTu_SaoChep, "", iThangLamViec,
                             Convert.ToInt32(iNamLamViec), MaND,
                             IPSua);
            }
            else
            {
                String iNgay = data[ParentID + "_iNgay"];
                Bang bang = new Bang("KT_ChungTu");
                bang.DuLieuMoi = true;
                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iThang", iThangLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
                if (CommonFunction.IsNumeric(sSoChungTu))
                {
                    bang.CmdParams.Parameters.AddWithValue("@iSoChungTu_GoiY", Convert.ToInt32(sSoChungTu));
                }
                bang.CmdParams.Parameters.AddWithValue("@iNgay", data[ParentID + "_iNgay"]);
                bang.CmdParams.Parameters.AddWithValue("@iTapSo", data[ParentID + "_iTapSo"]);
                bang.CmdParams.Parameters.AddWithValue("@sDonVi", data[ParentID + "_sDonVi"]);
                bang.CmdParams.Parameters.AddWithValue("@sNoiDung", data[ParentID + "_sNoiDung"]);
                String MaChungTuAddNew = Convert.ToString(bang.Save());
                Insert_SaoChep_CT(MaChungTuAddNew,sSoChungTu, sSoChungTu_SaoChep, iNgay, iThangLamViec,
                                  Convert.ToInt32(iNamLamViec), MaND,
                                  IPSua);
            }

            return 1;
        }
        public static string Insert_SaoChep_CTGS(String sSoChungTu, String sSoChungTu_SaoChep, String iThang, int iNamLamViec, String sMaNguoiDung, String IP, String iDonVi, String sDonVi_Sua)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (iDonVi == "on")
            {
                SQL =
                    @"insert KT_ChungTu(iID_MaChungTu,iID_MaDuyetChungTuCuoiCung,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaTrangThaiDuyet,iNamLamViec,iThang,iNgay,sSoChungTu,iSoChungTu_GoiY,iTapSo,sDonVi,sNoiDung,rTongSo,sIPSua,sID_MaNguoiDungSua,sID_MaNguoiDungTao)
SELECT @iID_MaChungTu,iID_MaDuyetChungTuCuoiCung,iID_MaNguonNganSach,iID_MaNamNganSach,@iID_MaTrangThaiDuyet,@iNamLamViec,@iThang,iNgay,@sSoChungTu,@iSoChungTu_GoiY,iTapSo,@sDonVi,sNoiDung,rTongSo,@sIPSua,@sMaNguoiDung,@sMaNguoiDung
  FROM KT_ChungTu where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTu=@sSoChungTu_SaoChep";
                cmd.Parameters.AddWithValue("@sDonVi", sDonVi_Sua);
            }
            else
            {
                SQL =
                    @"insert KT_ChungTu(iID_MaChungTu,iID_MaDuyetChungTuCuoiCung,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaTrangThaiDuyet,iNamLamViec,iThang,iNgay,sSoChungTu,iSoChungTu_GoiY,iTapSo,sDonVi,sNoiDung,rTongSo,sIPSua,sID_MaNguoiDungSua,sID_MaNguoiDungTao)
SELECT @iID_MaChungTu,iID_MaDuyetChungTuCuoiCung,iID_MaNguonNganSach,iID_MaNamNganSach,@iID_MaTrangThaiDuyet,@iNamLamViec,@iThang,iNgay,@sSoChungTu,@iSoChungTu_GoiY,iTapSo,sDonVi,sNoiDung,rTongSo,@sIPSua,@sMaNguoiDung,@sMaNguoiDung
  FROM KT_ChungTu where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTu=@sSoChungTu_SaoChep";
            }

            cmd.CommandText = SQL;
            Guid iID_MaChungTu = Guid.NewGuid();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                        LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            cmd.Parameters.AddWithValue("@iSoChungTu_GoiY", Convert.ToInt32(sSoChungTu));
            cmd.Parameters.AddWithValue("@sIPSua", IP);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            cmd.Parameters.AddWithValue("@sSoChungTu_SaoChep", sSoChungTu_SaoChep);
            vR = Connection.UpdateDatabase(cmd);
            if (vR > 0)
            {
                return iID_MaChungTu.ToString();
            }
            else
            {
                return "0";
            }
        }

        public static int Insert_SaoChep_CT(String iID_MaChungTu, String sSoChungTu, String sSoChungTu_SaoChep, String iNgay, String iThang, int iNamLamViec, String sMaNguoiDung, String IP)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (!String.IsNullOrEmpty(iNgay))
            {
                SQL =
                    @"insert KT_ChungTuChiTiet(iID_MaChungTu,iID_MaTrangThaiDuyet,iNamLamViec,iThangCT,iNgayCT
      ,sSoChungTuGhiSo,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDuyetChiTieuChiTiet,iNgay,iThang,sSoChungTuChiTiet,sNoiDung,rSoTien
      ,iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_No,sTenDonVi_No
      ,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co,sTenPhongBan_Co
      ,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co
      ,sGhiChu,sID_MaNguoiDungTao,sIPSua,sID_MaNguoiDungSua)
SELECT @iID_MaChungTu,@iID_MaTrangThaiDuyet ,@iNamLamViec,@iThangCT,@iNgayCT,@sSoChungTu,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDuyetChiTieuChiTiet,iNgay,iThang,sSoChungTuChiTiet,sNoiDung
      ,rSoTien,iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_No,sTenDonVi_No
      ,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co
      ,sTenPhongBan_Co,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co
,sGhiChu,@sMaNguoiDung,@sIPSua ,@sMaNguoiDung  FROM KT_ChungTuChiTiet where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTuGhiSo=@sSoChungTu_SaoChep";
            }
            else
            {

                SQL =
                   @"insert KT_ChungTuChiTiet(iID_MaChungTu,iID_MaTrangThaiDuyet,iNamLamViec,iThangCT,iNgayCT
      ,sSoChungTuGhiSo,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDuyetChiTieuChiTiet,iNgay,iThang,sSoChungTuChiTiet,sNoiDung,rSoTien
      ,iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_No,sTenDonVi_No
      ,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co,sTenPhongBan_Co
      ,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co
      ,sGhiChu,sID_MaNguoiDungTao,sIPSua,sID_MaNguoiDungSua)
SELECT @iID_MaChungTu,@iID_MaTrangThaiDuyet ,@iNamLamViec,@iThangCT,iNgayCT,@sSoChungTu,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDuyetChiTieuChiTiet,iNgay,iThang,sSoChungTuChiTiet,sNoiDung
      ,rSoTien,iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_No,sTenDonVi_No
      ,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co
      ,sTenPhongBan_Co,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co
,sGhiChu,@sMaNguoiDung,@sIPSua ,@sMaNguoiDung  FROM KT_ChungTuChiTiet where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTuGhiSo=@sSoChungTu_SaoChep";
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangCT", iThang);
            cmd.Parameters.AddWithValue("@sSoChungTu_SaoChep", sSoChungTu_SaoChep);
            if (!String.IsNullOrEmpty(iNgay))
            {
                cmd.Parameters.AddWithValue("@iNgayCT", iNgay);
            }
          
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sMaNguoiDung);
            cmd.Parameters.AddWithValue("@sIPSua", IP);
       
    
            vR = Connection.UpdateDatabase(cmd);
            return vR;
        }
        public static Boolean KiemTra_sSoChungTu_ChiTiet(String sSoChungTu, String iNamLamViec)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND sSoChungTuGhiSo=@sSoChungTu AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            int vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (String.IsNullOrEmpty(sSoChungTu) == false && vR == 0)
            {
                return true;
            }
            return false;
        }
        public static string TinhTong_HienThiTaiKhoan(string iID_MaChungTu, ref  DataTable dt, ref Boolean IsNo)
        {
            string vR = "";
            SqlCommand cmd = new SqlCommand();
            string SQL =
                "select distinct iID_MaTaiKhoan_No from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable tbl = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int vR_Count = tbl.Rows.Count;
            if (tbl != null && vR_Count <= 1)
            {
                if (vR_Count == 1)
                {


                    string iID_MaTaiKhoan_No = tbl.Rows[0]["iID_MaTaiKhoan_No"].ToString();
                    if (!String.IsNullOrEmpty(iID_MaTaiKhoan_No) && iID_MaTaiKhoan_No != "")
                    {
                        SQL =
                            "select SUM(rSoTien) as rSoTien from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaTaiKhoan_No=@iID_MaTaiKhoan_No and iID_MaChungTu=@iID_MaChungTu";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No", iID_MaTaiKhoan_No);
                        cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        cmd.CommandText = SQL;
                        Double rSoTien = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        string SoTien = "";
                        if (rSoTien < 0)
                        {
                            SoTien = "-" + CommonFunction.DinhDangSo(rSoTien);
                        }
                        else
                        {
                            SoTien = CommonFunction.DinhDangSo(rSoTien);
                        }
                        vR = "Nợ tài khoản " + iID_MaTaiKhoan_No + ":  <span style=\"color: #ec3237;\">" + SoTien +
                             "</span>";
                        tbl.Dispose();
                        cmd.Dispose();
                    }
                }
                else
                {
                    vR = "";
                }
                SQL =
                    "select distinct iID_MaTaiKhoan_Co as iID_MaTaiKhoan, ISNULL(SUM(rSoTien),0) as rSoTien from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu Group by iID_MaTaiKhoan_Co ORDER by iID_MaTaiKhoan_Co";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                IsNo = true;
                cmd.Dispose();
            }
          
            else
            {
                SQL =
                   "select distinct TOP 1 iID_MaTaiKhoan_Co from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
                cmd.CommandText = SQL;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                string iID_MaTaiKhoan_Co = Convert.ToString(Connection.GetValue(cmd, ""));
                cmd.Dispose();
                if (!String.IsNullOrEmpty(iID_MaTaiKhoan_Co) && iID_MaTaiKhoan_Co!="")
                {
                    SQL =
                     "select SUM(rSoTien) as rSoTien from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaTaiKhoan_Co=@iID_MaTaiKhoan_Co and iID_MaChungTu=@iID_MaChungTu";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", iID_MaTaiKhoan_Co);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.CommandText = SQL;
                    Double rSoTien = Convert.ToDouble(Connection.GetValue(cmd, 0));
                    string SoTien = "";
                    if (rSoTien < 0)
                    {
                        SoTien = "-" + CommonFunction.DinhDangSo(rSoTien);
                    }
                    else
                    {
                        SoTien = CommonFunction.DinhDangSo(rSoTien);
                    }
                    vR = "Có tài khoản " + iID_MaTaiKhoan_Co + ": <span style=\"color: #ec3237;\">" + SoTien + "</span>";
                    cmd.Dispose();   
                }
                SQL =
                   "select distinct iID_MaTaiKhoan_No as iID_MaTaiKhoan, ISNULL(SUM(rSoTien),0) as rSoTien from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu Group by iID_MaTaiKhoan_No ORDER by iID_MaTaiKhoan_No";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                IsNo = false;
                cmd.Dispose();
              
            }
            //int vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            return vR;
        }
        /// <summary>
        /// Kiem tra so chung tu thay doi co ton tai hay khong
        /// </summary>
        /// <param name="idSoChungTu"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="idSoChungTuCu"></param>
        /// <returns></returns>
        public static DataTable Get_So_ChungTu(String idSoChungTu, String iNamLamViec, String idSoChungTuCu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sSoChungTu FROM KT_ChungTu WHERE  iTrangThai = 1 AND iNamLamViec=@iNamLamViec AND sSoChungTu=@sSoChungTu AND sSoChungTu !=@idSoChungTuCu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@sSoChungTu", idSoChungTu);
            cmd.Parameters.AddWithValue("@idSoChungTuCu", idSoChungTuCu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Kiem tra so chung tu ghi so co ton tai ko
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int KiemTra_ChungTuGhiSo_ConTonTai(int iNamLamViec, String iID_MaChungTu, String strSoChungTuSua)
        {
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT COUNT(*) FROM KT_ChungTu WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec AND sSoChungTu=@sSoChungTu AND sSoChungTu NOT IN (SELECT sSoChungTu FROM KT_ChungTu WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu)");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@sSoChungTu", strSoChungTuSua);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            int vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}