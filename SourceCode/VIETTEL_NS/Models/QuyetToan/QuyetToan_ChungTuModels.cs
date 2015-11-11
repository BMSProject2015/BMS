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
    public class QuyetToan_ChungTuModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ quyết toán ngân sách
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
        /// Lấy DataTable thông tin của một chứng từ chỉ tiêu phân bổ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable GetChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM QTA_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int GetMaxChungTu()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoChungTu) FROM QTA_ChungTu WHERE iTrangThai=1");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng QTA_ChungTu
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="Params"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertRecord(String iID_MaDotPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            String MaChungTu = "";
            Bang bang = new Bang("QTA_ChungTu");
            DataTable dtDotPhanBo = PhanBo_DotPhanBoModels.GetDotPhanBo(iID_MaDotPhanBo);
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            bang.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", dtDotPhanBo.Rows[0]["dNgayDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtDotPhanBo.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtDotPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtDotPhanBo.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(QuyetToanModels.iID_MaPhanHeQuyetToan));
            bang.DuLieuMoi = true;
            String MaChungTuAddNew = Convert.ToString(bang.Save());

            //Thêm chi tiết chỉ tiêu
            //PhanBo_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, IPSua);

            return MaChungTu;
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
            Bang bang = new Bang("QTA_ChungTu");
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
        public static int Delete_ChungTu(String iID_MaChungTu, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng QTA_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("UPDATE QTA_ChungTuChiTiet SET iTrangThai=0 WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng QTA_ChungTu
            Bang bang = new Bang("QTA_ChungTu");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.Delete();
            return 1;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng QTA_ChungTu, QTA_ChungTuChiTiet
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

            //Sửa dữ liệu trong bảng QTA_ChungTuChiTiet            
            String SQL = "UPDATE QTA_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE QTA_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChungTu=@iID_MaChungTu";
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
            Bang bang = new Bang("QTA_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }

        public static DataTable Get_DanhSachChungTu(String MaPhongBan, String Loai, String MaND, String MaDonVi, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, String sThangQuy, Boolean LayTheoMaNDTao = false, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            DK += " AND iNamLamViec=@iNamLamViec AND iLoai=@iLoai  AND iID_MaNamNganSach=@iID_MaNamNganSach ";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            //cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@iLoai", Loai);

            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                if (Loai != "2")
                {
                    DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
                }
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (String.IsNullOrEmpty(MaDonVi) == false && MaDonVi != "" && MaDonVi != null)
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            }
            if (Loai == "1")
            {
                if (String.IsNullOrEmpty(sThangQuy) == false && sThangQuy != "" && sThangQuy != "-1")
                {
                    DK += " AND iThang_Quy = @iThang_Quy";
                    cmd.Parameters.AddWithValue("@iThang_Quy", sThangQuy);
                    DK += " AND bLoaiThang_Quy = @bLoaiThang_Quy";
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", 0);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(sThangQuy) == false && sThangQuy != "" && sThangQuy != "-1")
                {
                    DK += " AND iThang_Quy = @iThang_Quy";
                    cmd.Parameters.AddWithValue("@iThang_Quy", sThangQuy);
                    DK += " AND bLoaiThang_Quy = @bLoaiThang_Quy";
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                }
            }
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "" && SoChungTu != null)
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
            String SQL = String.Format("SELECT * FROM QTA_ChungTu WHERE iTrangThai = 1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,iThang_Quy DESC, dNgayChungTu DESC,iID_MaDonVi,sDSLNS,iSoChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String MaPhongBan, String Loai, String MaND, String MaDonVi, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, String sThangQuy, Boolean LayTheoMaNDTao = false)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            DK += " AND iNamLamViec=@iNamLamViec AND iLoai=@iLoai AND iID_MaNamNganSach=@iID_MaNamNganSach ";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            //cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@iLoai", Loai);

            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            }
            if (String.IsNullOrEmpty(MaDonVi) == false && MaDonVi != "" && MaDonVi != null)
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            }
            if (Loai == "1")
            {
                if (String.IsNullOrEmpty(sThangQuy) == false && sThangQuy != "" && sThangQuy != "-1")
                {
                    DK += " AND iThang_Quy = @iThang_Quy";
                    cmd.Parameters.AddWithValue("@iThang_Quy", sThangQuy);
                    DK += " AND bLoaiThang_Quy = @bLoaiThang_Quy";
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", 0);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(sThangQuy) == false && sThangQuy != "" && sThangQuy != "-1")
                {
                    DK += " AND iThang_Quy = @iThang_Quy";
                    cmd.Parameters.AddWithValue("@iThang_Quy", sThangQuy);
                    DK += " AND bLoaiThang_Quy = @bLoaiThang_Quy";
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                }
            }
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "" && SoChungTu != null)
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
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
            String SQL = String.Format("SELECT Count(*) FROM QTA_ChungTu WHERE iTrangThai = 1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable SoQuyetToanTrongKy(String sLNS, String iID_MaDonVi, String iNamLamViec, String Thang_Quy, String LoaiThang_Quy, String MaND)
        {
            DataTable vR;
            if (sLNS == null) sLNS = "";
            if (iID_MaDonVi == null) iID_MaDonVi = "";
            if (iNamLamViec == null) iNamLamViec = "";
            if (Thang_Quy == null || Thang_Quy == "-1") Thang_Quy = "";
            if (LoaiThang_Quy == null) LoaiThang_Quy = "";

            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);

            //Dieu kien thang quy
            String DKThang_Quy = "";
            if (Thang_Quy != "")
            {
                if (LoaiThang_Quy == "1")
                {
                    switch (Convert.ToInt32(Thang_Quy))
                    {
                        case 3:
                            DKThang_Quy = "(iThang_Quy between 1  and 3)";
                            break;
                        case 6:
                            DKThang_Quy = "(iThang_Quy between 4  and 6)";
                            break;
                        case 9:
                            DKThang_Quy = "(iThang_Quy between 7  and 9)";
                            break;
                        case 12:
                            DKThang_Quy = "(iThang_Quy between 10  and 12)";
                            break;
                    }
                }
                else
                {
                    DKThang_Quy = " iThang_Quy=" + Thang_Quy + "";
                }
                if (DKThang_Quy != "")
                {
                    DK += " AND " + DKThang_Quy + "";
                }
            }

            //Dieu kien don vi
            if (iID_MaDonVi != "")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            String strLNS = "";
            if (sLNS != "")
            {
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    strLNS += String.Format("sLNS = '{0}'", arrLNS[i]);
                    if (i < arrLNS.Length - 1)
                        strLNS += " OR ";
                }
                strLNS += " AND sNG<>'' ";
            }
            if (strLNS != "")
            {
                DK += " AND ( " + strLNS + " )";
            }
            else
            {
                DK += " AND sLNS='-1'";
            }
            String SQL = String.Format("SELECT SUM(rTuChi) AS TuChi, SUM(rHienVat) AS HienVat FROM QTA_ChungTuChiTiet WHERE {0} {1}", DK,ReportModels.DieuKien_PhongBan_DonVi(MaND));
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable SoQuyetToanDenKy(String sLNS, String iID_MaDonVi, String iNamLamViec, String Thang_Quy, String LoaiThang_Quy, String MaND)
        {
            DataTable vR;
            if (sLNS == null) sLNS = "";
            if (iID_MaDonVi == null) iID_MaDonVi = "";
            if (iNamLamViec == null) iNamLamViec = "";
            if (Thang_Quy == null || Thang_Quy == "-1") Thang_Quy = "";
            if (LoaiThang_Quy == null) LoaiThang_Quy = "";

            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);

            //Dieu kien thang quy
            String DKThang_Quy = "";
            if (Thang_Quy != "")
            {
                if (LoaiThang_Quy == "1")
                {
                    switch (Convert.ToInt32(Thang_Quy))
                    {
                        case 3:
                            DKThang_Quy = "(iThang_Quy between 1  and 3)";
                            break;
                        case 6:
                            DKThang_Quy = "(iThang_Quy between 1  and 6)";
                            break;
                        case 9:
                            DKThang_Quy = "(iThang_Quy between 1  and 9)";
                            break;
                        case 12:
                            DKThang_Quy = "(iThang_Quy between 1  and 12)";
                            break;
                    }
                }
                else
                {
                    DKThang_Quy = " iThang_Quy<=" + Thang_Quy + "";
                }
                if (DKThang_Quy != "")
                {
                    DK += " AND " + DKThang_Quy + "";
                }
            }

            //Dieu kien don vi
            if (iID_MaDonVi != "")
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }

            String strLNS = "";
            if (sLNS != "")
            {
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    strLNS += String.Format("sLNS = '{0}'", arrLNS[i]);
                    if (i < arrLNS.Length - 1)
                        strLNS += " OR ";
                }
                strLNS += " AND sNG<>'' ";
            }
            if (strLNS != "")
            {
                DK += " AND  (" + strLNS + " )";
            }
            else
            {
                DK += " AND sLNS='-1'";
            }
            String SQL = String.Format("SELECT SUM(rTuChi) AS TuChi, SUM(rHienVat) AS HienVat FROM QTA_ChungTuChiTiet WHERE {0} {1}", DK, ReportModels.DieuKien_PhongBan_DonVi(MaND));
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }


        public static DataTable ThongKeTongSoQuyetToan(String sLNS, String iID_MaDonVi, String iNamLamViec, String MaND)
        {
            DataTable vR;
            if (sLNS == null) sLNS = "";
            if (iID_MaDonVi == null) iID_MaDonVi = "";
            if (iNamLamViec == null) iNamLamViec = "";

            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);

            //Dieu kien don vi
            //if (iID_MaDonVi != "")
            //{
            //    DK += " AND iID_MaDonVi=@iID_MaDonVi";
            //    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            //}

            String strLNS = "";
            if (sLNS != "")
            {
                String[] arrLNS = sLNS.Split(',');
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    strLNS += String.Format("sLNS = '{0}'", arrLNS[i]);
                    if (i < arrLNS.Length - 1)
                        strLNS += " OR ";
                }
                strLNS += " AND sNG<>'' ";
            }
            if (strLNS != "")
            {
                DK += " AND (" + strLNS + ")";
            }
            else
            {
                DK += " AND sLNS='-1'";
            }
            String SQL = String.Format("SELECT SUM(rTuChi) AS TuChi, SUM(rHienVat) AS HienVat FROM QTA_ChungTuChiTiet WHERE {0} {1}", DK, ReportModels.DieuKien_PhongBan_DonVi(MaND));
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String ThongKeTongSoQuyetToan_ChungTu(String iID_MaChungTu)
        {
            String vR;

            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND sNG<>''";
            DK += " AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

            String SQL = String.Format("SELECT (SUM(rTuChi) + SUM(rHienVat)) AS TuChi FROM QTA_ChungTuChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
    }
}