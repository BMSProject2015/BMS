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
    public class KeToanTongHop_KhoaSoKeToanModels
    {
        /// <summary>
        /// Thêm dữ liệu vào bảng KT_LuyKe khi khóa sổ kế toán
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        public static void ThemChiTiet(String iNam, String iThang, String MaND, String IPSua)
        {
            //Thêm dữ liệu vào bảng KT_PhatSinhCanDoi
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "KT_INSERTBANGPHATSINHCANDOI";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable iGiaTri = Connection.GetDataTable(cmd);
            cmd.Dispose();

            if (iGiaTri.Rows.Count > -1)
            {
                //Xóa giá trị đã có trong bảng
                cmd = new SqlCommand("DELETE FROM KT_LuyKe WHERE iNam=@iNam AND iThang=@iThang");
                cmd.Parameters.AddWithValue("@iNam", iNam);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Lấy bảng cân đối tháng 0
                DataTable dt0 = LayGiaTriCanDoiTrongKy(Convert.ToInt32(iNam), Convert.ToInt32(0));

                //Lấy bảng cân đối kế toán
                DataTable dt = LayGiaTriCanDoiTrongKy(Convert.ToInt32(iNam), Convert.ToInt32(iThang));
                DataRow R;
                int i;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    R = dt.Rows[i];

                    Double rLK_No = 0, rLK_Co = 0;
                    Double rPS_No = 0, rPS_Co = 0;
                    Double rDK_No = 0, rDK_Co = 0;
                    Double rLK_No_0 = 0, rLK_Co_0 = 0;

                    if(Convert.ToString(R["PSNO"]) != null && Convert.ToString(R["PSNO"]) != ""){
                        rPS_No = Convert.ToDouble(R["PSNO"]);
                    }
                    if (Convert.ToString(R["PSCO"]) != null && Convert.ToString(R["PSCO"]) != "")
                    {
                        rPS_Co = Convert.ToDouble(R["PSCO"]);
                    }
                    if (Convert.ToString(R["NODAUKY"]) != null && Convert.ToString(R["NODAUKY"]) != "")
                    {
                        rDK_No = Convert.ToDouble(R["NODAUKY"]);
                    }
                    if (Convert.ToString(R["CODAUKY"]) != null && Convert.ToString(R["CODAUKY"]) != "")
                    {
                        rDK_Co = Convert.ToDouble(R["CODAUKY"]);
                    }

                    for (int j = 0; j < dt0.Rows.Count;j++ ){
                        if (Convert.ToString(dt0.Rows[j]["iID_MaTaiKhoan"]) == Convert.ToString(R["iID_MaTaiKhoan"])) {
                            if (Convert.ToString(dt0.Rows[j]["PSNO"]) != null && Convert.ToString(dt0.Rows[j]["PSNO"]) != "")
                            {
                                rLK_No_0 = Convert.ToDouble(dt0.Rows[j]["PSNO"]);
                            }
                            if (Convert.ToString(dt0.Rows[j]["PSCO"]) != null && Convert.ToString(dt0.Rows[j]["PSCO"]) != "")
                            {
                                rLK_Co_0 = Convert.ToDouble(dt0.Rows[j]["PSCO"]);
                            }
                            break;
                        }
                    }

                    if (Convert.ToInt32(iThang) != 0)
                    {
                        rLK_No = rDK_No + rPS_No - rLK_No_0;
                        rLK_Co = rDK_Co + rPS_Co - rLK_Co_0;
                    }
                    else {
                        rLK_No = rDK_No + rPS_No;
                        rLK_Co = rDK_Co + rPS_Co;
                    }

                    Bang bang = new Bang("KT_LuyKe");
                    bang.DuLieuMoi = true;
                    bang.MaNguoiDungSua = MaND;
                    bang.IPSua = IPSua;
                    bang.CmdParams.Parameters.AddWithValue("@iNam", iNam);
                    bang.CmdParams.Parameters.AddWithValue("@iThang", iThang);
                    bang.CmdParams.Parameters.AddWithValue("@sTKNo", R["iID_MaTaiKhoan"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTen", R["sTen"]);
                    bang.CmdParams.Parameters.AddWithValue("@rPS_No", R["PSNO"]);
                    bang.CmdParams.Parameters.AddWithValue("@rPS_Co", R["PSCO"]);
                    bang.CmdParams.Parameters.AddWithValue("@rLK_No", rLK_No);
                    bang.CmdParams.Parameters.AddWithValue("@rLK_Co", rLK_Co);
                    bang.CmdParams.Parameters.AddWithValue("@rCK_No", R["DUNOCK"]);
                    bang.CmdParams.Parameters.AddWithValue("@rCK_Co", R["DUCOCK"]);
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                    bang.Save();
                }
                dt.Dispose();
            }
        }
        /// <summary>
        /// Lấy bảng cân đối tài khoản
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable LayGiaTriCanDoiTrongKy(int iNam, int iThang)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "KT_BANGCANDOITAIKHOAN";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            return vR;
        }
        public static DataTable LayGiaTriCanDoiThang0(int iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "KT_BANGCANDOITAIKHOAN";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", 0);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            return vR;
        }
        /// <summary>
        /// Lấy danh sách dữ liệu trgn bảng KT_LuyKe theo năm và tháng
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_All(String iNam, String iThang,Boolean CanDoiTaiKhoan=false)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND SUBSTRING(sTKNo,1,1)<>'0'";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
           
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                Int32 dbThang = Convert.ToInt32(iThang);
                if (dbThang <= 12)
                {
                    DK += " AND iThang = @iThang";
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                }    
            }            
            String SQL = String.Format("SELECT * FROM KT_LuyKe WHERE {0} ORDER BY sTKNo, iThang", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataTable dtKQ;
            dtKQ = KeToanTongHopModels.DienTaiKhoanCha(vR, iNam, "sTKNo", CanDoiTaiKhoan);
            vR.Dispose();
            return dtKQ;
        }
        /// <summary>
        /// Lấy danh sách dữ liệu trgn bảng KT_LuyKe theo năm và tháng
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_All_NgoaiBang(String iNam, String iThang, Boolean CanDoiTaiKhoan = false)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND SUBSTRING(sTKNo,1,1)='0'";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }

            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                Int32 dbThang = Convert.ToInt32(iThang);
                if (dbThang <= 12)
                {
                    DK += " AND iThang = @iThang";
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                }
            }
            String SQL = String.Format("SELECT * FROM KT_LuyKe WHERE {0} ORDER BY sTKNo, iThang", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataTable dtKQ;
            dtKQ = KeToanTongHopModels.DienTaiKhoanCha(vR, iNam, "sTKNo", CanDoiTaiKhoan);
            vR.Dispose();
            return dtKQ;
        }
        /// <summary>
        /// Lấy danh sách các tháng trong năm trong bảng KT_LuyKe
        /// </summary>
        /// <param name="iNam"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_ThangTrongNam(String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            String SQL = String.Format("SELECT DISTINCT(iThang) FROM KT_LuyKe WHERE {0} ORDER BY iThang", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy lũy kế kỳ trước của tài khoản
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static DataTable Get_LuyKeKyTruocChoMotTaiKhoan(String iNam, String iThang, String iID_MaTaiKhoan)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            if (String.IsNullOrEmpty(iID_MaTaiKhoan) == false && iID_MaTaiKhoan != "")
            {
                DK += " AND sTKNo = @sTKNo";
                cmd.Parameters.AddWithValue("@sTKNo", iID_MaTaiKhoan);
            }
            String SQL = String.Format("SELECT * FROM KT_LuyKe WHERE {0} ", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách bảng KT_LuyKe theo điều kiện tìm
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach(String iNam, String iThang, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            String SQL = String.Format("SELECT * FROM KT_LuyKe WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iSTT, iThang DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số bản ghi trong bảng KT_LuyKe với các tham số tìm
        /// </summary>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static int Get_DanhSach_Count(String iNam, String iThang)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            String SQL = String.Format("SELECT Count(*) FROM KT_LuyKe WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ chi tiết phát sinh trong kỳ
        /// </summary>
        /// <param name="sTaiKhoan"></param>
        /// <param name="iNam"></param>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_ChiTietTaiKhoanPhatSinhTrongKy(String sTaiKhoan, String iNam, String iThang)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                DK += " AND iID_MaTaiKhoan = @iID_MaTaiKhoan";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", sTaiKhoan);
            }
            String SQL = String.Format("SELECT * FROM KT_PhatSinhCanDoi WHERE {0} ORDER BY iNgay, iID_MaTaiKhoan", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static int CheckDuyetChungTuGhiSo(String iNam, String iThang)
        {
            int vR = 0;

            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iID_MaTrangThaiDuyet != " + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNamLamViec = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM KT_ChungTu WHERE {0} ", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            return vR;
        }
        public static int CheckThangLonNhatCoPhatSinh(String iNam) {
            int vR = 0;

            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iID_MaTrangThaiDuyet = " + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop) + "";
            if (String.IsNullOrEmpty(iNam) == false && iNam != "")
            {
                DK += " AND iNamLamViec = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            String SQL = String.Format("SELECT MAX(iThangCT) FROM KT_ChungTuChiTiet WHERE {0} ", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            return vR;
        }
        public static void ThemChiTietKhoaSoNam(String iNam, String iThang, String MaND, String IPSua)
        {
            int i,j;
            SqlCommand cmd;
            //Thêm dữ liệu vào bảng KT_NamChotSo
            cmd = new SqlCommand("DELETE FROM KT_NamChotSo WHERE iNam=@iNam");
            cmd.Parameters.AddWithValue("@iNam", iNam);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bangnamchotso = new Bang("KT_NamChotSo");
            bangnamchotso.DuLieuMoi = true;
            bangnamchotso.MaNguoiDungSua = MaND;
            bangnamchotso.IPSua = IPSua;
            bangnamchotso.CmdParams.Parameters.AddWithValue("@iNam", iNam);
            bangnamchotso.Save();

            //Nhân bản giá trị tài khoản cho bảng KT_TaiKhoan
            cmd = new SqlCommand("SELECT * FROM KT_TaiKhoan WHERE iNam=@iNam");
            cmd.Parameters.AddWithValue("@iNam", iNam);
            DataTable dtTaiKhoan = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd = new SqlCommand("SELECT * FROM KT_TaiKhoan WHERE iNam=@iNam");
            cmd.Parameters.AddWithValue("@iNam", Convert.ToInt32(iNam) + 1);
            DataTable dtTaiKhoanNamSau = Connection.GetDataTable(cmd);
            cmd.Dispose();
            for (i = 0; i < dtTaiKhoan.Rows.Count; i++) {
                if (dtTaiKhoanNamSau.Rows.Count > 0)
                {
                    Boolean bGiaTri = false;
                    for (j = 0; j < dtTaiKhoanNamSau.Rows.Count; j++)
                    {
                        String iID_MaTaiKhoan_Cu = Convert.ToString(dtTaiKhoan.Rows[i]["iID_MaTaiKhoan"]);
                        String iID_MaTaiKhoan_Moi = Convert.ToString(dtTaiKhoanNamSau.Rows[j]["iID_MaTaiKhoan"]);
                        if (iID_MaTaiKhoan_Cu != iID_MaTaiKhoan_Moi
                            && Convert.ToString(dtTaiKhoanNamSau.Rows[j]["iNam"]) != Convert.ToString(Convert.ToInt32(iNam) + 1))
                        {
                            bGiaTri = true;
                            break;
                        }
                    }
                    if (bGiaTri == true)
                    {
                        Bang bangtk = new Bang("KT_TaiKhoan");
                        bangtk.DuLieuMoi = true;
                        bangtk.MaNguoiDungSua = MaND;
                        bangtk.IPSua = IPSua;
                        bangtk.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan", dtTaiKhoan.Rows[i]["iID_MaTaiKhoan"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Cha", dtTaiKhoan.Rows[i]["iID_MaTaiKhoan_Cha"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@bLaHangCha", dtTaiKhoan.Rows[i]["bLaHangCha"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@bHienThi", dtTaiKhoan.Rows[i]["bHienThi"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@sTen", dtTaiKhoan.Rows[i]["sTen"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@sMoTa", dtTaiKhoan.Rows[i]["sMoTa"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@iLoaiTaiKhoan", dtTaiKhoan.Rows[i]["iLoaiTaiKhoan"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@iCapTaiKhoan", dtTaiKhoan.Rows[i]["iCapTaiKhoan"]);
                        bangtk.CmdParams.Parameters.AddWithValue("@iNam", Convert.ToInt32(iNam) + 1);
                        bangtk.CmdParams.Parameters.AddWithValue("@iSTT", dtTaiKhoan.Rows[i]["iSTT"]);
                        bangtk.Save();
                    }
                }
                else {
                    Bang bangtk = new Bang("KT_TaiKhoan");
                    bangtk.DuLieuMoi = true;
                    bangtk.MaNguoiDungSua = MaND;
                    bangtk.IPSua = IPSua;
                    bangtk.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan", dtTaiKhoan.Rows[i]["iID_MaTaiKhoan"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Cha", dtTaiKhoan.Rows[i]["iID_MaTaiKhoan_Cha"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@bLaHangCha", dtTaiKhoan.Rows[i]["bLaHangCha"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@bHienThi", dtTaiKhoan.Rows[i]["bHienThi"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@sTen", dtTaiKhoan.Rows[i]["sTen"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@sMoTa", dtTaiKhoan.Rows[i]["sMoTa"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@iLoaiTaiKhoan", dtTaiKhoan.Rows[i]["iLoaiTaiKhoan"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@iCapTaiKhoan", dtTaiKhoan.Rows[i]["iCapTaiKhoan"]);
                    bangtk.CmdParams.Parameters.AddWithValue("@iNam", Convert.ToInt32(iNam) + 1);
                    bangtk.CmdParams.Parameters.AddWithValue("@iSTT", dtTaiKhoan.Rows[i]["iSTT"]);
                    bangtk.Save();
                }
            }

            //Nhân bản giá trị danh mục tham số cho bảng KT_DanhMucThamSo
            cmd = new SqlCommand("SELECT * FROM KT_DanhMucThamSo WHERE iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            DataTable dtThamSo = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd = new SqlCommand("SELECT * FROM KT_DanhMucThamSo WHERE iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
            DataTable dtThamSoNamSau = Connection.GetDataTable(cmd);
            cmd.Dispose();
            for (i = 0; i < dtThamSo.Rows.Count; i++)
            {
                if (dtThamSoNamSau.Rows.Count > 0)
                {
                    Boolean bGiaTri = false;
                    for (j = 0; j < dtThamSoNamSau.Rows.Count; j++)
                    {
                        if (Convert.ToString(dtThamSo.Rows[i]["sKyHieu"]) != Convert.ToString(dtThamSoNamSau.Rows[j]["sKyHieu"]) 
                            && Convert.ToString(dtThamSoNamSau.Rows[j]["iNamLamViec"]) != Convert.ToString(Convert.ToInt32(iNam) + 1))
                        {
                            bGiaTri = true;
                            break;
                        }
                    }
                    if (bGiaTri == true)
                    {
                        Bang bangdmts = new Bang("KT_DanhMucThamSo");
                        bangdmts.DuLieuMoi = true;
                        bangdmts.MaNguoiDungSua = MaND;
                        bangdmts.IPSua = IPSua;
                        bangdmts.CmdParams.Parameters.AddWithValue("@sKyHieu", dtThamSo.Rows[i]["sKyHieu"]);
                        bangdmts.CmdParams.Parameters.AddWithValue("@sLoaiThamSo", dtThamSo.Rows[i]["sLoaiThamSo"]);
                        bangdmts.CmdParams.Parameters.AddWithValue("@sBaoCao_ControllerName", dtThamSo.Rows[i]["sBaoCao_ControllerName"]);
                        bangdmts.CmdParams.Parameters.AddWithValue("@sNoiDung", dtThamSo.Rows[i]["sNoiDung"]);
                        bangdmts.CmdParams.Parameters.AddWithValue("@sThamSo", dtThamSo.Rows[i]["sThamSo"]);
                        bangdmts.CmdParams.Parameters.AddWithValue("@iThangLamViec", dtThamSo.Rows[i]["iThangLamViec"]);
                        bangdmts.CmdParams.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                        bangdmts.CmdParams.Parameters.AddWithValue("@iSTT", dtThamSo.Rows[i]["iSTT"]);
                        bangdmts.Save();
                    }
                }
                else
                {
                    Bang bangdmts = new Bang("KT_DanhMucThamSo");
                    bangdmts.DuLieuMoi = true;
                    bangdmts.MaNguoiDungSua = MaND;
                    bangdmts.IPSua = IPSua;
                    bangdmts.CmdParams.Parameters.AddWithValue("@sKyHieu", dtThamSo.Rows[i]["sKyHieu"]);
                    bangdmts.CmdParams.Parameters.AddWithValue("@sLoaiThamSo", dtThamSo.Rows[i]["sLoaiThamSo"]);
                    bangdmts.CmdParams.Parameters.AddWithValue("@sBaoCao_ControllerName", dtThamSo.Rows[i]["sBaoCao_ControllerName"]);
                    bangdmts.CmdParams.Parameters.AddWithValue("@sNoiDung", dtThamSo.Rows[i]["sNoiDung"]);
                    bangdmts.CmdParams.Parameters.AddWithValue("@sThamSo", dtThamSo.Rows[i]["sThamSo"]);
                    bangdmts.CmdParams.Parameters.AddWithValue("@bChoPhepXoa", dtThamSo.Rows[i]["bChoPhepXoa"]);
                    bangdmts.CmdParams.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                    bangdmts.CmdParams.Parameters.AddWithValue("@iSTT", dtThamSo.Rows[i]["iSTT"]);
                    bangdmts.Save();
                }
            }
            // Nhân bản đơn vị - quyền
            String strQuyen =
                @"INSERT INTO NS_DonVi(iID_Ma,iID_MaDonVi,sMaSo,sTen,sTenTomTat,sSoTaiKhoan,sDiaChi,sKhoBac,sMoTa,iCap,iHuongLuong,iID_MaDonViCha
,iNamLamViec_DonVi,iID_MaPhongBan,iID_MaKhoiDonVi,iID_MaLoaiDonVi,sTenLoaiDonVi,iID_MaNhomDonVi,iSTT,iCapNS)
select NEWID(),iID_MaDonVi,sMaSo,sTen,sTenTomTat,sSoTaiKhoan,sDiaChi,sKhoBac,sMoTa,iCap,iHuongLuong,iID_MaDonViCha,iNamLamViec_DonVi+1
,iID_MaPhongBan,iID_MaKhoiDonVi,iID_MaLoaiDonVi,sTenLoaiDonVi,iID_MaNhomDonVi,iSTT,iCapNS from NS_DonVi dv
where iTrangThai=1 and iNamLamViec_DonVi=@iNam and not  exists (select iID_MaDonVi from NS_DonVi ns where ns.iTrangThai=1 
and ns.iNamLamViec_DonVi=@iNamSau and ns.iID_MaDonVi=dv.iID_MaDonVi)
INSERT INTO NS_PhongBan_DonVi
(iID_MaPhongBan,iID_MaDonVi,iNamLamViec,iSTT)
select iID_MaPhongBan,iID_MaDonVi, iNamLamViec+1,iSTT from NS_PhongBan_DonVi dv where iTrangThai=1 and iNamLamViec=@iNam
and not  exists (select iID_MaPhongBanDonVi from NS_PhongBan_DonVi ns where ns.iTrangThai=1 
and ns.iNamLamViec=@iNamSau and ns.iID_MaDonVi=dv.iID_MaDonVi)
INSERT INTO NS_NguoiDung_DonVi
(sMaNguoiDung,iID_MaDonVi,iNamLamViec,iSTT)
select sMaNguoiDung,iID_MaDonVi, iNamLamViec+1,iSTT from NS_NguoiDung_DonVi dv where iTrangThai=1 and iNamLamViec=@iNam
and not  exists (select iID_MaNguoiDungDonVi from NS_NguoiDung_DonVi ns where ns.iTrangThai=1 
and ns.iNamLamViec=@iNamSau and ns.iID_MaDonVi=dv.iID_MaDonVi);";
            cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = strQuyen;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iNamSau", Convert.ToInt32(iNam) + 1);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();


            // Tinh so du tai khoan chi tiet
            String sqlTKChiTiet =
                @"DELETE KT_SoDuTaiKhoanGiaiThich  where iNamLamViec=@iNamSau and iNamLamViec!=2013;
Insert into KT_SoDuTaiKhoanGiaiThich(sSoChungTuGhiSo,iNamLamViec,iNgay,iThang,sSoChungTuChiTiet,sNoiDung,iID_MaTaiKhoan_No,
sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_No,sTenDonVi_No,iID_MaDonVi_Co,sTenDonVi_Co,
iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co,sTenPhongBan_Co,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co, rSoTien)

select '',@iNamSau,1,1,'',@sNoiDung,ct.iID_MaTaiKhoan_No,ct.sTenTaiKhoan_No,
 ct.iID_MaTaiKhoan_Co, ct.sTenTaiKhoan_Co,ct.iID_MaDonVi_No,ct.sTenDonVi_No,ct.iID_MaDonVi_Co,ct.sTenDonVi_Co,
  ct.iID_MaPhongBan_No,ct.sTenPhongBan_No,ct.iID_MaPhongBan_Co,ct.sTenPhongBan_Co,ct.iID_MaTaiKhoanGiaiThich_No, ct.sTenTaiKhoanGiaiThich_No, ct.iID_MaTaiKhoanGiaiThich_Co,
  ct.sTenTaiKhoanGiaiThich_Co, SoTien=CASE WHEN (SUM(ct.rSoTienNo)-SUM(ct.rSoTienCo)>=0) THEN (SUM(ct.rSoTienNo)-SUM(ct.rSoTienCo)) ELSE (SUM(ct.rSoTienNo)-SUM(ct.rSoTienCo))*-1 END 
 from (SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No, iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No, iID_MaTaiKhoan_Co=0,sTenTaiKhoan_Co='', iID_MaTaiKhoanGiaiThich_Co='',sTenTaiKhoanGiaiThich_Co='',
 iID_MaDonVi_No,sTenDonVi_No,iID_MaDonVi_Co='',sTenDonVi_Co='',
iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co='',sTenPhongBan_Co='',(CASE WHEN Sum(rSoTien) IS NULL THEN 0 ELSE Sum(rSoTien) END) AS rSoTienNo, rSoTienCo=0
	FROM KT_ChungTuChiTiet
	WHERE (iThangCT<=12 and iThangCT>@iThangCT AND iNamLamViec=@iNam AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
	)
	GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
	HAVING (((iID_MaTaiKhoan_No)<>'') and ((iID_MaTaiKhoanGiaiThich_No)<>'') and ((sTenTaiKhoanGiaiThich_No)<>''))
	
union

SELECT iID_MaTaiKhoan_No=0,sTenTaiKhoan_No='', iID_MaTaiKhoanGiaiThich_No='',sTenTaiKhoanGiaiThich_No='',iID_MaTaiKhoan_Co,sTenTaiKhoan_Co, iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co,
 iID_MaDonVi_No='',sTenDonVi_No='',iID_MaDonVi_Co,sTenDonVi_Co,
iID_MaPhongBan_No='',sTenPhongBan_No='',iID_MaPhongBan_Co,sTenPhongBan_Co,
rSoTienNo=0,(CASE WHEN Sum(rSoTien) IS NULL THEN 0 ELSE Sum(rSoTien) END) AS rSoTienCo
	FROM KT_ChungTuChiTiet
	WHERE (iThangCT<=12 and iThangCT>@iThangCT AND iNamLamViec=@iNam AND iTrangThai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
	)
	GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co,iID_MaPhongBan_Co,sTenPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
	HAVING (((iID_MaTaiKhoan_Co)<>'') and ((iID_MaTaiKhoanGiaiThich_Co)<>'')  and ((sTenTaiKhoanGiaiThich_Co)<>''))
	union

    SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No, iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No, iID_MaTaiKhoan_Co=0,sTenTaiKhoan_Co='', iID_MaTaiKhoanGiaiThich_Co='',sTenTaiKhoanGiaiThich_Co='',
	iID_MaDonVi_No,sTenDonVi_No,iID_MaDonVi_Co='',sTenDonVi_Co='',
iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co='',sTenPhongBan_Co='',(CASE WHEN Sum(rSoTien) IS NULL THEN 0 ELSE Sum(rSoTien) END) AS rSoTienNo, rSoTienCo=0
	FROM KT_SoDuTaiKhoanGiaiThich
	WHERE (iNamLamViec=@iNam AND iTrangThai=1)
	GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaPhongBan_No,sTenPhongBan_No,iID_MaDonVi_No,sTenDonVi_No
	HAVING (((iID_MaTaiKhoan_No)<>'') and ((iID_MaTaiKhoanGiaiThich_No)<>'')  and ((sTenTaiKhoanGiaiThich_No)<>''))
	union
SELECT iID_MaTaiKhoan_No=0,sTenTaiKhoan_No='', iID_MaTaiKhoanGiaiThich_No='',sTenTaiKhoanGiaiThich_No='',iID_MaTaiKhoan_Co,sTenTaiKhoan_Co, iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co,	
iID_MaDonVi_No='',sTenDonVi_No='',iID_MaDonVi_Co,sTenDonVi_Co,
iID_MaPhongBan_No='',sTenPhongBan_No='',iID_MaPhongBan_Co,sTenPhongBan_Co,rSoTienNo=0,(CASE WHEN Sum(rSoTien) IS NULL THEN 0 ELSE Sum(rSoTien) END) AS rSoTienCo
	FROM KT_SoDuTaiKhoanGiaiThich
	WHERE (iNamLamViec=@iNam AND iTrangThai=1)
	GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co,iID_MaPhongBan_Co,sTenPhongBan_Co,iID_MaDonVi_Co,sTenDonVi_Co
	HAVING (((iID_MaTaiKhoan_Co)<>'') and ((iID_MaTaiKhoanGiaiThich_Co)<>'')  and ((sTenTaiKhoanGiaiThich_Co)<>''))) as ct	
	group by ct.iID_MaTaiKhoan_No,ct.sTenTaiKhoan_No,ct.iID_MaTaiKhoan_Co,ct.sTenTaiKhoan_Co,ct.iID_MaDonVi_No,ct.sTenDonVi_No,ct.iID_MaDonVi_Co,ct.sTenDonVi_Co,
	ct.iID_MaPhongBan_No,ct.sTenPhongBan_No,ct.iID_MaPhongBan_Co,ct.sTenPhongBan_Co,ct.iID_MaTaiKhoanGiaiThich_No,ct.sTenTaiKhoanGiaiThich_No,
	ct.iID_MaTaiKhoanGiaiThich_Co,ct.sTenTaiKhoanGiaiThich_Co
	having  
	SUM(ct.rSoTienNo)-SUM(ct.rSoTienCo)!=0
order by ct.iID_MaTaiKhoan_No,ct.iID_MaTaiKhoan_Co;";
            cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = sqlTKChiTiet;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@iNamSau", Convert.ToInt32(iNam) + 1);
            cmd.Parameters.AddWithValue("@sNoiDung", "Số dư tài khoản chi tiết năm " + iNam + " chuyển sang");
            cmd.Parameters.AddWithValue("@iThangCT", Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNam)));
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            //Thêm dữ liệu vào bảng KT_PhatSinhCanDoi
            cmd = new SqlCommand();
            cmd.CommandText = "KT_INSERTBANGPHATSINHCANDOI_NAM";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iNamSau", Convert.ToInt32(iNam) + 1);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable iGiaTri = Connection.GetDataTable(cmd);
            cmd.Dispose();

            if (iGiaTri.Rows.Count > -1)
            {
                //Xóa giá trị đã có trong bảng
                cmd = new SqlCommand("DELETE FROM KT_LuyKe WHERE iNam=@iNam AND iThang=@iThang");
                cmd.Parameters.AddWithValue("@iNam", Convert.ToInt32(iNam) + 1);
                cmd.Parameters.AddWithValue("@iThang", 0);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Lấy bảng cân đối tháng 0
                DataTable dt0 = LayGiaTriCanDoiTrongKy(Convert.ToInt32(iNam), 0);

                //Lấy bảng cân đối kế toán
                DataTable dt = LayGiaTriCanDoiTrongKy(Convert.ToInt32(iNam), Convert.ToInt32(iThang));
                DataRow R;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    R = dt.Rows[i];

                    Double rLK_No = 0, rLK_Co = 0;
                    Double rPS_No = 0, rPS_Co = 0;
                    Double rDK_No = 0, rDK_Co = 0;
                    Double rLK_No_0 = 0, rLK_Co_0 = 0;

                    if (Convert.ToString(R["PSNO"]) != null && Convert.ToString(R["PSNO"]) != "")
                    {
                        rPS_No = Convert.ToDouble(R["PSNO"]);
                    }
                    if (Convert.ToString(R["PSCO"]) != null && Convert.ToString(R["PSCO"]) != "")
                    {
                        rPS_Co = Convert.ToDouble(R["PSCO"]);
                    }
                    if (Convert.ToString(R["NODAUKY"]) != null && Convert.ToString(R["NODAUKY"]) != "")
                    {
                        rDK_No = Convert.ToDouble(R["NODAUKY"]);
                    }
                    if (Convert.ToString(R["CODAUKY"]) != null && Convert.ToString(R["CODAUKY"]) != "")
                    {
                        rDK_Co = Convert.ToDouble(R["CODAUKY"]);
                    }

                    for (j = 0; j < dt0.Rows.Count; j++)
                    {
                        if (Convert.ToString(dt0.Rows[j]["iID_MaTaiKhoan"]) == Convert.ToString(R["iID_MaTaiKhoan"]))
                        {
                            if (Convert.ToString(dt0.Rows[j]["PSNO"]) != null && Convert.ToString(dt0.Rows[j]["PSNO"]) != "")
                            {
                                rLK_No_0 = Convert.ToDouble(dt0.Rows[j]["PSNO"]);
                            }
                            if (Convert.ToString(dt0.Rows[j]["PSCO"]) != null && Convert.ToString(dt0.Rows[j]["PSCO"]) != "")
                            {
                                rLK_Co_0 = Convert.ToDouble(dt0.Rows[j]["PSCO"]);
                            }
                            break;
                        }
                    }

                    if (Convert.ToInt32(iThang) != 0)
                    {
                        rLK_No = rDK_No + rPS_No - rLK_No_0;
                        rLK_Co = rDK_Co + rPS_Co - rLK_Co_0;
                    }
                    else
                    {
                        rLK_No = rDK_No + rPS_No;
                        rLK_Co = rDK_Co + rPS_Co;
                    }

                    Bang bang = new Bang("KT_LuyKe");
                    bang.DuLieuMoi = true;
                    bang.MaNguoiDungSua = MaND;
                    bang.IPSua = IPSua;
                    bang.CmdParams.Parameters.AddWithValue("@iNam", Convert.ToInt32(iNam) + 1);
                    bang.CmdParams.Parameters.AddWithValue("@iThang", 0);
                    bang.CmdParams.Parameters.AddWithValue("@sTKNo", R["iID_MaTaiKhoan"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTen", R["sTen"]);
                    bang.CmdParams.Parameters.AddWithValue("@rPS_No", R["PSNO"]);
                    bang.CmdParams.Parameters.AddWithValue("@rPS_Co", R["PSCO"]);
                    bang.CmdParams.Parameters.AddWithValue("@rLK_No", rLK_No);
                    bang.CmdParams.Parameters.AddWithValue("@rLK_Co", rLK_Co);
                    bang.CmdParams.Parameters.AddWithValue("@rCK_No", R["DUNOCK"]);
                    bang.CmdParams.Parameters.AddWithValue("@rCK_Co", R["DUCOCK"]);
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                    bang.Save();
                }
                dt.Dispose();
            
                //Thêm chứng từ phát sinh vào tháng 0 năm sau:
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();

                //cmd = new SqlCommand("DELETE FROM KT_ChungTu WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang");
                //cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                //cmd.Parameters.AddWithValue("@iThang", 0);
                //Connection.UpdateDatabase(cmd);
                //cmd.Dispose();

                //cmd = new SqlCommand("DELETE FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThangCT=@iThangCT");
                //cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                //cmd.Parameters.AddWithValue("@iThangCT", 0);
                //Connection.UpdateDatabase(cmd);
                //cmd.Dispose();
                cmd = new SqlCommand("UPDATE KT_ChungTu SET iTrangThai=0 WHERE iNamLamViec=@iNamLamViec AND iThang=@iThangCT;" +
                                     " UPDATE KT_ChungTuChiTiet SET iTrangThai=0 WHERE iNamLamViec=@iNamLamViec AND iThangCT=@iThangCT;");
                cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                cmd.Parameters.AddWithValue("@iThangCT", 0);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                cmd.Parameters.Clear();
                // ---------------------------------------------//
                Bang bangct = new Bang("KT_ChungTu");
                bangct.DuLieuMoi = true;
                bangct.MaNguoiDungSua = MaND;
                bangct.IPSua = IPSua;
                bangct.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
               // bangct.CmdParams.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", Guid.NewGuid());
                bangct.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                bangct.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                bangct.CmdParams.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                bangct.CmdParams.Parameters.AddWithValue("@iThang", 0);
                bangct.CmdParams.Parameters.AddWithValue("@iNgay", 0);
                bangct.CmdParams.Parameters.AddWithValue("@sSoChungTu", 0);
                bangct.CmdParams.Parameters.AddWithValue("@iTapSo", 0);
                bangct.CmdParams.Parameters.AddWithValue("@sDonVi", "");
                bangct.CmdParams.Parameters.AddWithValue("@sNoiDung", "Chuyển số dư cuối năm " + iNam + " sang");
                bangct.CmdParams.Parameters.AddWithValue("@dNgayChungTu", DateTime.Now);
                String MaChungTuAddNew = Convert.ToString(bangct.Save());
                String MaDuyetChungTu = KeToanTongHop_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Chuyển số dư cuối năm " + iNam + " sang", MaND, IPSua);

                cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
                KeToanTongHop_ChungTuModels.UpdateRecord(MaChungTuAddNew, cmd.Parameters, MaND, IPSua);
                cmd.Dispose();
               // ---------------------------------------------//
                String iID_MaChungTu = Convert.ToString(bangct.GiaTriKhoa);

                cmd = new SqlCommand();
                cmd.CommandText = "KT_LAYSOTONGCHUNGTUPHATSINH_KHOANAM";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iNam", iNam);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                DataTable dtChungTuPhatSinh = Connection.GetDataTable(cmd);               
                cmd.Dispose();

                //chuyen doi ma tai khoan nam truoc - nam sau
                String strThamSoKeToan = KeToan_DanhMucThamSoModels.LayThongTinThamSo("35", iNam);
                //DataTable dtDanhSachTK=Connection.GetDataTable("SELECT * FROM ")
                String TK = "", BaKyTuDau = "", KyTuCuoi = "", TK_Chuyen = "", TenTK = "", KyTuKetThuc = "";
                String[] arrThamSo = strThamSoKeToan.Split(',');
                if (arrThamSo.Length > 0)
                {
                    for (i = 0; i < dtChungTuPhatSinh.Rows.Count; i++)
                    {
                        TK = Convert.ToString(dtChungTuPhatSinh.Rows[i]["iID_MaTaiKhoan"]);

                        if (TK.Length >= 5)
                        {
                            if (strThamSoKeToan.IndexOf(TK.Substring(0, 3)) >= 0)
                            {
                                //String iID_MaTaiKhoan_NamTruoc = iID_MaTaiKhoan.Substring(0, 3) + "0" + iID_MaTaiKhoan.Substring(4, iID_MaTaiKhoan.Length - 4);
                                //String iID_MaTaiKhoan_NamSau = iID_MaTaiKhoan.Substring(0, 3) + "1" + iID_MaTaiKhoan.Substring(4, iID_MaTaiKhoan.Length - 4);
                                BaKyTuDau = TK.Substring(0, 3);
                                KyTuCuoi = TK.Substring(3, 1);
                                KyTuKetThuc = TK.Substring(4, TK.Length - 4);
                                if (KyTuCuoi == "2") KyTuCuoi = "1";
                                if (KyTuCuoi == "3") KyTuCuoi = "2";
                                TK_Chuyen = BaKyTuDau + KyTuCuoi + KyTuKetThuc;
                                dtChungTuPhatSinh.Rows[i]["iID_MaTaiKhoan"] = TK_Chuyen;
                                TenTK = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(TK_Chuyen, Convert.ToInt16(iNam));
                                if (TenTK != "")
                                {
                                    dtChungTuPhatSinh.Rows[i]["sTenTaiKhoan"] = TK_Chuyen + " - " + TenTK;
                                }
                            }
                        }

                    }
                }
                DataRow R1;
                for (i = 0; i < dtChungTuPhatSinh.Rows.Count; i++)
                {
                    R1 = dtChungTuPhatSinh.Rows[i];

                    Bang bangctct = new Bang("KT_ChungTuChiTiet");
                    bangctct.DuLieuMoi = true;
                    bangctct.MaNguoiDungSua = MaND;
                    bangctct.IPSua = IPSua;
                    //neu la du No
                    Double rSoTien_Du = Convert.ToDouble(R1["SoDu"]);
                    if (rSoTien_Du >= 0)
                    {
                        bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R1["iID_MaTaiKhoan"]);
                        bangctct.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R1["sTenTaiKhoan"]);
                        bangctct.CmdParams.Parameters.AddWithValue("@rSoTien", rSoTien_Du);
                    }
                    else
                    {
                        bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R1["iID_MaTaiKhoan"]);
                        bangctct.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R1["sTenTaiKhoan"]);
                        bangctct.CmdParams.Parameters.AddWithValue("@rSoTien", rSoTien_Du * (-1));
                    }
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                    bangctct.CmdParams.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
                    bangctct.CmdParams.Parameters.AddWithValue("@iThangCT", 0);
                    bangctct.CmdParams.Parameters.AddWithValue("@iNgayCT", 1);
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    bangctct.CmdParams.Parameters.AddWithValue("@iNgay", 1);
                    bangctct.CmdParams.Parameters.AddWithValue("@iThang", 0);
                    bangctct.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", i);
                    bangctct.CmdParams.Parameters.AddWithValue("@sNoiDung", "Chuyển số liệu năm " + iNam + " sang");
                    bangctct.CmdParams.Parameters.AddWithValue("@sGhiChu", "Chuyển số liệu năm " + iNam + " sang");

                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R1["iID_MaDonVi_No"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R1["sTenDonVi_No"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R1["iID_MaDonVi_Co"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R1["sTenDonVi_Co"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan_No", R1["iID_MaPhongBan_No"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@sTenPhongBan_No", R1["sTenPhongBan_No"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan_Co", R1["iID_MaPhongBan_Co"]);
                    bangctct.CmdParams.Parameters.AddWithValue("@sTenPhongBan_Co", R1["sTenPhongBan_Co"]);

                    bangctct.CmdParams.Parameters.AddWithValue("@iSTT", i);
                    bangctct.Save();
                }
                dtChungTuPhatSinh.Dispose();

//                String SQL = @"
//	                        SELECT (CASE WHEN Sum(rSoTien) IS NULL THEN 0 ELSE Sum(rSoTien) END) AS SumOfTTienVND, 
//		                        iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,
//		                        iID_MaDonVi_No,sTenDonVi_No,iID_MaDonVi_Co,sTenDonVi_Co,
//		                        iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co,sTenPhongBan_Co,
//		                        iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co
//	                        FROM KT_ChungTuChiTiet_Temp
//	                        GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,
//		                        iID_MaDonVi_No,sTenDonVi_No,iID_MaDonVi_Co,sTenDonVi_Co,
//		                        iID_MaPhongBan_No,sTenPhongBan_No,iID_MaPhongBan_Co,sTenPhongBan_Co,
//		                        iID_MaTaiKhoanGiaiThich_No,sTenTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,sTenTaiKhoanGiaiThich_Co
//	                        HAVING (((iID_MaTaiKhoan_Co)<>'' OR iID_MaPhongBan_No<>''))";

//                cmd = new SqlCommand();
//                cmd.CommandText = SQL;                                                
//                dtChungTuPhatSinh = Connection.GetDataTable(cmd);
//                cmd.Dispose();
                
//                for (i = 0; i < dtChungTuPhatSinh.Rows.Count; i++)
//                {
//                    R1 = dtChungTuPhatSinh.Rows[i];
//                    Bang bangctct = new Bang("KT_ChungTuChiTiet");
//                    bangctct.DuLieuMoi = true;
//                    bangctct.MaNguoiDungSua = MaND;
//                    bangctct.IPSua = IPSua;
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
//                    bangctct.CmdParams.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(iNam) + 1);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iThangCT", 0);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iNgayCT", 1);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iNgay", 1);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iThang", 0);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", i);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sNoiDung", "Chuyển số liệu năm " + iNam + " sang!");
//                    bangctct.CmdParams.Parameters.AddWithValue("@rSoTien", R1["SumOfTTienVND"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R1["iID_MaTaiKhoan_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R1["sTenTaiKhoan_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R1["iID_MaTaiKhoan_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R1["sTenTaiKhoan_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R1["iID_MaDonVi_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R1["sTenDonVi_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R1["iID_MaDonVi_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R1["sTenDonVi_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan_No", R1["iID_MaPhongBan_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenPhongBan_No", R1["sTenPhongBan_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan_Co", R1["iID_MaPhongBan_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenPhongBan_Co", R1["sTenPhongBan_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanGiaiThich_No", R1["iID_MaTaiKhoanGiaiThich_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenTaiKhoanGiaiThich_No", R1["sTenTaiKhoanGiaiThich_No"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanGiaiThich_Co", R1["iID_MaTaiKhoanGiaiThich_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sTenTaiKhoanGiaiThich_Co", R1["sTenTaiKhoanGiaiThich_Co"]);
//                    bangctct.CmdParams.Parameters.AddWithValue("@sGhiChu", "Chuyển số liệu năm " + iNam + " sang!");
//                    bangctct.CmdParams.Parameters.AddWithValue("@iSTT", i);
//                    bangctct.Save();
//                }
//                dtChungTuPhatSinh.Dispose();
//                SQL = "DELETE FROM KT_ChungTuChiTiet_Temp";
//                cmd = new SqlCommand(SQL);
//               // Connection.UpdateDatabase(cmd);
//                cmd.Dispose();
            }
        }
    }
}