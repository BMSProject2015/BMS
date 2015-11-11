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
    public class KTCS_ChungTuModels
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
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTCS_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy số chứng từ lớn nhất trong bảng KTCS_ChungTu
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChungTu_GoiY()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoChungTu_GoiY) FROM KTCS_ChungTu WHERE iTrangThai=1");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Kiểm tra tính hợp lệ của sSoChungTu
        /// </summary>
        /// <returns></returns>
        public static Boolean KiemTra_sSoChungTu(String sSoChungTu)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KTCS_ChungTu WHERE iTrangThai=1 AND sSoChungTu=@sSoChungTu");
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
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
        public static Boolean KiemTra_sSoChungTu_Trung(String iID_MaChungTu, String sSoChungTu)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KTCS_ChungTu WHERE sSoChungTu=@sSoChungTu AND iTrangThai=1 ");
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
            Int32 vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (String.IsNullOrEmpty(sSoChungTu) == false && vR == 0)
            {
                return true;
            }
            else
            {
                cmd = new SqlCommand("SELECT COUNT(*) FROM KTCS_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1 ");
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
        /// <summary>
        /// Lấy số chứng từ lớn nhất trong bảng KTCS_ChungTu
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChungTu()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoChungTu) FROM KTCS_ChungTu WHERE iTrangThai=1");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng KTCS_ChungTu
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="Params"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertRecord(String iID_MaChungTu, NameValueCollection data, String MaND, String IPSua)
        {
            String sSoChungTu = data["sSoChungTu"];
            sSoChungTu = sSoChungTu.Trim().ToUpper();

            if (KiemTra_sSoChungTu(sSoChungTu))
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];
                Bang bang = new Bang("KTCS_ChungTu");
                bang.GiaTriKhoa = iID_MaChungTu;
                bang.DuLieuMoi = true;

                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(CongSanModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iThang", R["iThangLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(CongSanModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
                if (CommonFunction.IsNumeric(sSoChungTu))
                {
                    bang.CmdParams.Parameters.AddWithValue("@iSoChungTu_GoiY", Convert.ToInt32(sSoChungTu));
                }
                bang.CmdParams.Parameters.AddWithValue("@iNgay", data["iNgay"]);
                bang.CmdParams.Parameters.AddWithValue("@sNoiDung", data["sNoiDung"]);
                bang.Save();
                dtCauHinh.Dispose();
            }
            return iID_MaChungTu;
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
            Bang bang = new Bang("KTCS_ChungTu");
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
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            String MaND = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);

            //Phuonglt yêu cầu cấp trợ lý tổng hợp được xóa tất cả cấp trợ lý phòng ban, tài khoản nào thuộc cấp trợ lý phòng ban 
            // thì chỉ được xóa chứng từ của tài khoản đó tạo                       
            if ((sID_MaNguoiDungSua == MaND || LuongCongViecModel.KiemTra_TroLyTongHop(sID_MaNguoiDungSua)) && LuongCongViecModel.KiemTra_TrangThaiKhoiTao(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            {
                //Xóa dữ liệu trong bảng KTCS_ChungTuChiTiet
                SqlCommand cmd;
                cmd = new SqlCommand("UPDATE KTCS_ChungTuChiTiet SET iTrangThai=0, sIPSua=@sIPSua, sID_MaNguoiDungSua=@sID_MaNguoiDungSua WHERE iID_MaChungTu=@iID_MaChungTu");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@sIPSua", sIPSua);
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", sID_MaNguoiDungSua);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Xóa dữ liệu trong bảng KT_ChungTu
                Bang bang = new Bang("KTCS_ChungTu");
                bang.MaNguoiDungSua = sID_MaNguoiDungSua;
                bang.IPSua = sIPSua;
                bang.GiaTriKhoa = iID_MaChungTu;
                bang.Delete();

                dt.Dispose();
            }
            return 1;
        }



        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng KTCS_ChungTu, KTCS_ChungTuChiTiet
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

            //Sửa dữ liệu trong bảng KTCS_ChungTuChiTiet            
            String SQL = "UPDATE KTCS_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE KTCS_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChungTu=@iID_MaChungTu";
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
            Bang bang = new Bang("KTTM_DuyetChungTu");
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
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(CongSanModels.iID_MaPhanHe, MaND);
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
            String SQL = String.Format("SELECT * FROM KTCS_ChungTu WHERE iTrangThai = 1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet, dNgayChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(CongSanModels.iID_MaPhanHe, MaND);
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
            String SQL = String.Format("SELECT Count(*) FROM KTCS_ChungTu WHERE iTrangThai = 1 AND {0}", DK);
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
            String SQL = String.Format("SELECT * FROM KTCS_ChungTu WHERE {0} ORDER BY iNgay DESC,sSoChungTu DESC", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static string getSoChungTuGhiSo(String iID_MaChungTu)
        {
            string strGiaTri = "";
            if (String.IsNullOrEmpty(iID_MaChungTu) == false)
            {
                DataTable dt = null;
                dt = GetChungTu(iID_MaChungTu);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    strGiaTri = "Số: " + HamChung.ConvertToString(dr["sSoChungTu"]) + " - " + HamChung.ConvertToString(dr["sNoiDung"]) +
                        " Ngày: " + HamChung.ConvertToString(dr["iNgay"]) + " Tháng " + HamChung.ConvertToString(dr["iThang"]);
                }
                if (dt != null) dt.Dispose();
            }
            return strGiaTri;
        }

        public static Boolean TrinhDuyetChungTu(String iID_MaChungTu, String MaND, String IPSua)
        {
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = -1;
            iID_MaTrangThaiDuyet_TrinhDuyet = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return false;
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            KTCT_TienMat_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, IPSua);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = KTCT_TienMat_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, IPSua);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();
            return true;
        }

        public static Boolean TuChoiChungTu(String iID_MaChungTu, String MaND, String IPSua)
        {
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return false;
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            KTCT_TienMat_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            KTCT_TienMat_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, IPSua);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = KTCT_TienMat_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, IPSua);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();
            return true;
        }

        public static void InsertRecord_ChungTu_ChungTuChiTiet(String iID_MaTaiSan, String MaND, String IPSua)
        {
            int iSoChungTu_GoiY = KTCS_ChungTuModels.GetMaxChungTu_GoiY();
            String sSoChungTu = "";

            Boolean ok = false;
            int d = 0;
            do
            {
                iSoChungTu_GoiY++;
                ok = KTCS_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY));
                d++;
            } while (ok == false && d < 10);
            if (ok)
            {
                sSoChungTu = Convert.ToString(iSoChungTu_GoiY);
            }

            sSoChungTu = sSoChungTu.Trim().ToUpper();

            SqlCommand cmd;
            String SQL = String.Format("SELECT *,YEAR(dNgayDuaVaoKhauHao) as NamDuaVaoKhauHao FROM KTCS_TaiSan WHERE iID_MaTaiSan=@iID_MaTaiSan AND iTrangThai=1");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            DataTable dtTaiSan =Connection.GetDataTable(cmd);
            cmd.Dispose();
            int rSoNamKhauHao = Convert.ToInt32((dtTaiSan.Rows[0]["rSoNamKhauHao"]));
            int iNamDuaVaoKhauHao = Convert.ToInt32((dtTaiSan.Rows[0]["NamDuaVaoKhauHao"]));
            int iNamLamViec = Convert.ToInt32(NguoiDungCauHinhModels.iNamLamViec);
            if (iNamLamViec >= iNamDuaVaoKhauHao)
                rSoNamKhauHao = rSoNamKhauHao - (iNamLamViec - iNamDuaVaoKhauHao);
            if (KiemTra_sSoChungTu(sSoChungTu))
            {
                DateTime dNgayMuaTaiSan = Convert.ToDateTime(dtTaiSan.Rows[0]["dNgayMua"]);
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                //Them vao bang chung tu ghi so cong san
                Bang bang = new Bang("KTCS_ChungTu");
                bang.DuLieuMoi = true;

                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(CongSanModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(CongSanModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
                if (CommonFunction.IsNumeric(sSoChungTu))
                {
                    bang.CmdParams.Parameters.AddWithValue("@iSoChungTu_GoiY", Convert.ToInt32(sSoChungTu));
                }
                bang.CmdParams.Parameters.AddWithValue("@iNgay", dNgayMuaTaiSan.Day);
                bang.CmdParams.Parameters.AddWithValue("@iThang", dNgayMuaTaiSan.Month);
                bang.CmdParams.Parameters.AddWithValue("@sNoiDung", dtTaiSan.Rows[0]["sTenTaiSan"]);
                String iID_MaChungTu = Convert.ToString(bang.Save());
                dtCauHinh.Dispose();

                //Them chung tu chi tiet vào bang chung tu chi tiet
                Bang bang1 = new Bang("KTCS_ChungTuChiTiet");
                bang1.DuLieuMoi = true;

                bang1.MaNguoiDungSua = MaND;
                bang1.IPSua = IPSua;
                bang1.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                bang1.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(CongSanModels.iID_MaPhanHe));
                bang1.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang1.CmdParams.Parameters.AddWithValue("@iNgayCT", dNgayMuaTaiSan.Day);
                bang1.CmdParams.Parameters.AddWithValue("@iThangCT", dNgayMuaTaiSan.Month);
                bang1.CmdParams.Parameters.AddWithValue("@sSoChungTuGhiSo", sSoChungTu);
                bang1.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang1.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang1.CmdParams.Parameters.AddWithValue("@sTinhChat", 'T');
                bang1.CmdParams.Parameters.AddWithValue("@iNgay", dNgayMuaTaiSan.Day);
                bang1.CmdParams.Parameters.AddWithValue("@iThang", dNgayMuaTaiSan.Month);
                bang1.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", iID_MaTaiSan);
                bang1.CmdParams.Parameters.AddWithValue("@sNoiDung", dtTaiSan.Rows[0]["sTenTaiSan"]);
                bang1.CmdParams.Parameters.AddWithValue("@iID_MaTaiSan", dtTaiSan.Rows[0]["iID_MaTaiSan"]);
                bang1.CmdParams.Parameters.AddWithValue("@sTenTaiSan", dtTaiSan.Rows[0]["sTenTaiSan"]);
                bang1.CmdParams.Parameters.AddWithValue("@rSoTien", dtTaiSan.Rows[0]["rNguyenGia"]);
                bang1.CmdParams.Parameters.AddWithValue("@rNamKhauHao", rSoNamKhauHao);
                bang1.CmdParams.Parameters.AddWithValue("@dThoiGianDuaVaoSuDung", dtTaiSan.Rows[0]["dNgayDuaVaoKhauHao"]);
                bang1.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtTaiSan.Rows[0]["iID_MaDonVi"]);
                bang1.CmdParams.Parameters.AddWithValue("@sTenDonVi", dtTaiSan.Rows[0]["sTenDonVi"]);
                bang1.Save();
            }
        }
    }
}