using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models.DungChung;

namespace VIETTEL.Models
{
    public class DuToanBS_ChungTuModels
    {
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

        public static NameValueCollection LayThongTin_Gom(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu_Gom(iID_MaChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }

            dt.Dispose();
            return Data;
        }

        public static NameValueCollection LayThongTin_Gom_THCuc(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu_Gom_THCuc(iID_MaChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }

            dt.Dispose();
            return Data;
        }
        public static NameValueCollection LayThongTin_KyThuatLan2(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu_KyThuatLan2(iID_MaChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }

            dt.Dispose();
            return Data;
        }
        public static DataTable Get_DanhSachChungTu(String iID_MaChungTu, String bTLTH, String MaPhongBan, String MaLoaiNganSach, String NgayDotNganSach, String MaND, String SoChungTu, String TuNgay, String DenNgay, String sLNS_TK, String iID_MaTrangThaiDuyet, Boolean LayTheoMaNDTao=false, String iKyThuat = "0", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            //nếu là ngân sách bảo đảm nganh ky thuat
            if (MaLoaiNganSach == "1040100" && iKyThuat == "1")
            {
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
            }
                //Ngan sach bao dam nganh khac
            else if (MaLoaiNganSach == "1040100,109")
            {
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
            }
                //cac loai ngan sach khac
            else
            {
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
                DK += " AND (sDSLNS NOT LIKE '104%'  AND sDSLNS NOT LIKE '109%'  )  ";
            }
            DK += " AND iTrangThai = 1 AND iiD_MaDonVi is null";

            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(MaND));

            //gom chung tu
            if (iID_MaChungTu != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iID_MaChungTu) == false && iID_MaChungTu != "")
            {
                String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
                String[] arrChungtu = iID_MaChungTu_CT.Split(',');
                DK += " AND(";
                for (int j = 0; j < arrChungtu.Length; j++)
                {
                    DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                    if (j < arrChungtu.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

                }
                DK += " )";
            }
            //Nếu ở phần nhập chứng từ
            else
            {
                if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
                {
                    DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
                }
                if (String.IsNullOrEmpty(iKyThuat) == false && iKyThuat != "" && iKyThuat != "0")
                {
                    DK += " AND iKyThuat = @iKyThuat";
                    cmd.Parameters.AddWithValue("@iKyThuat", iKyThuat);
                }
                else
                {
                    DK += " AND iKyThuat = @iKyThuat";
                    cmd.Parameters.AddWithValue("@iKyThuat", "0");
                }
                //kiem tra tro ly tong hop
                Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
                if (checkTroLyTongHop == false)
                    LayTheoMaNDTao = true;
                
            }
            if (String.IsNullOrEmpty(MaLoaiNganSach) == false && MaLoaiNganSach != "" && MaLoaiNganSach != "-1")
            {
                String[] arrLNS = MaLoaiNganSach.Split(',');
                DK += " AND ( ";
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    DK += "  sDSLNS LIKE @sLNS" + i;
                    if (i < arrLNS.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i] + "%");
                }
                DK += " ) ";
            }
            if (String.IsNullOrEmpty(sLNS_TK) == false && sLNS_TK != "")
            {
                DK += String.Format(" AND sDSLNS LIKE '{0}%'", sLNS_TK);
            }
            if (String.IsNullOrEmpty(NgayDotNganSach) == false && NgayDotNganSach != "")
            {
                DK += " AND CONVERT(nvarchar, dNgayDotNganSach, 103) = @dNgayDotNganSach";
                cmd.Parameters.AddWithValue("@dNgayDotNganSach", NgayDotNganSach);
            }

            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
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


            String SQL = String.Format("SELECT * FROM DTBS_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,sDSLNS, iSoChungTu,dNgayChungTu", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String iID_MaChungTu, String bTLTH, String MaPhongBan = "", String MaLoaiNganSach = "", String NgayDotNganSach = "", String MaDotNganSach = "", String MaND = "", String SoChungTu = "", String TuNgay = "", String DenNgay = "", String sLNS_TK = "", String iID_MaTrangThaiDuyet = "", Boolean LayTheoMaNDTao = false)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 ";

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            DK += " AND iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach ";
            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);

            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            }
            if (iID_MaChungTu != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iID_MaChungTu) == false && iID_MaChungTu != "")
            {
                String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
                String[] arrChungtu = iID_MaChungTu_CT.Split(',');
                DK += " AND(";
                for (int j = 0; j < arrChungtu.Length; j++)
                {
                    DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                    if (j < arrChungtu.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

                }
                DK += " )";
            }
            if (String.IsNullOrEmpty(MaLoaiNganSach) == false && MaLoaiNganSach != "")
            {
                DK += String.Format(" AND sDSLNS LIKE '{0}%'", MaLoaiNganSach);
            }
            if (String.IsNullOrEmpty(sLNS_TK) == false && sLNS_TK != "")
            {
                DK += String.Format(" AND sDSLNS LIKE '{0}%'", sLNS_TK);
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (String.IsNullOrEmpty(NgayDotNganSach) == false && NgayDotNganSach != "")
            {
                DK += " AND CONVERT(nvarchar, dNgayDotNganSach, 103) = @dNgayDotNganSach";
                cmd.Parameters.AddWithValue("@dNgayDotNganSach", NgayDotNganSach);
            }
            if (String.IsNullOrEmpty(MaDotNganSach) == false && MaDotNganSach != "")
            {
                DK += " AND iID_MaDotNganSach = @iID_MaDotNganSach";
                cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
            }
            if (String.IsNullOrEmpty(MaND) == false && MaND != "" && MaND != "admin")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
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

            String SQL = String.Format("SELECT COUNT(*) FROM DTBS_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        //Danh sach chung tu gom lan 2 cua ngan sách bao dam
        public static DataTable Get_DanhSachChungTu_GomLan2(String MaPhongBan, String MaND, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, Boolean LayTheoMaNDTao, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
            DK += " AND iTrangThai = 1 ";

            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(MaND));
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
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }

            String SQL = String.Format("SELECT * FROM DTBS_ChungTu_TLTH WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,dNgayChungTu", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_DanhSachChungTu_Gom(String iKyThuat,String iLoai,string iID_MaChungTu, String MaPhongBan, String MaND, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, Boolean LayTheoMaNDTao, int Trang = 1, int SoBanGhi = 0, String sLNS = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            if (sLNS == "1040100")
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeDuToan, MaND);
            else
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 ";

            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(MaND));
            if (iID_MaChungTu != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iID_MaChungTu) == false && iID_MaChungTu != "")
            {
                String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", iID_MaChungTu, "iID_MaChungTu_TLTH"));
                String[] arrChungtu = iID_MaChungTu_CT.Split(',');
                DK += " AND(";
                for (int j = 0; j < arrChungtu.Length; j++)
                {
                    DK += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                    if (j < arrChungtu.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrChungtu[j]);

                }
                DK += " )";
            }
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
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(iLoai) == false && iLoai != "")
            {
                DK += " AND iLoai = @iLoai";
                cmd.Parameters.AddWithValue("@iLoai", iLoai);
            }
            String SQL = String.Format("SELECT * FROM DTBS_ChungTu_TLTH WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,dNgayChungTu", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Gom_Count(String MaPhongBan, String MaND, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, Boolean LayTheoMaNDTao, int Trang = 1, int SoBanGhi = 0, String sLNS = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            if (sLNS == "1040100")
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
            else
                DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 ";

            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(MaND));
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
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM DTBS_ChungTu_TLTH WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_DanhSachChungTu_Gom_THCuc(String MaPhongBan, String MaND, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, Boolean LayTheoMaNDTao, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 ";

            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(MaND));
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
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }

            String SQL = String.Format("SELECT * FROM DTBS_ChungTu_TLTHCuc WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,dNgayChungTu", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Gom_THCuc_Count(String MaPhongBan, String MaND, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, Boolean LayTheoMaNDTao, int Trang = 1, int SoBanGhi = 0)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 ";

            DK += String.Format(" AND iNamLamViec={0}", ReportModels.LayNamLamViec(MaND));
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
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM DTBS_ChungTu_TLTHCuc WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

      

        public static DataTable GetChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTu_Gom(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu_TLTH WHERE iTrangThai=1 AND iID_MaChungTu_TLTH=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTu_KyThuatLan2(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTuChiTiet=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetChungTu_Gom_THCuc(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DTBS_ChungTu_TLTHCUc WHERE iTrangThai=1 AND iID_MaChungTu_TLTHCUc=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static Boolean UpdateRecord(String iID_MaChungTu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("DTBS_ChungTu");
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
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaChungTu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //update trang Thai duyet dtChungTu
            DuToanBS_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng DTBS_ChungTuChiTiet            
            String SQL = "UPDATE DTBS_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);


            //Update Trang Thai duyet bang DTBS_ChungTuChiTiet_PhanCap
            SQL = "UPDATE DTBS_ChungTuChiTiet_PhanCap SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu IN (SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            //Update Trang Thai duyet bang DTBS_ChungTuChiTiet NSBD lan 1
            SQL = "UPDATE DTBS_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu IN (SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)";
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
            String MaDuyetChungTu;
            Bang bang = new Bang("DT_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }
        public static int iSoChungTu(String iNamLamViec)
        {
            String SQL = "SELECT MAX(iSoChungTu) FROM DTBS_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            int iSoCT = Convert.ToInt32(Connection.GetValue(cmd, 0));
            return iSoCT;
        }

        /// <summary>
        /// Nguoi Dung Phong Ban
        /// </summary>
        /// <param name="sID_MaNguoiDung"></param>
        /// <returns></returns>
        public static DataTable NguoiDung_PhongBan(String sID_MaNguoiDung)
        {
            DataTable vR;
            string SQL = @"SELECT pb.iID_MaPhongBan, sTen,pb.sTen + ' - ' + pb.sMoTa AS sTenPB FROM NS_NguoiDung_PhongBan AS nd INNER JOIN 
NS_PhongBan AS pb ON nd.iID_MaPhongBan = pb.iID_MaPhongBan WHERE     (nd.iTrangThai = 1) AND (pb.iTrangThai = 1) AND (nd.sMaNguoiDung=@sMaNguoiDung)";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", sID_MaNguoiDung);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        //danh sach chung tu gom trolytonghop lần 2
        public static DataTable getDanhSachChungTu_TongHopDuyetLan2(String sMaND, String sLNS)
        {
            DataTable vR;
            int iID_MaTrangThaiDuyet;
            SqlCommand cmd = new SqlCommand();
            bool bTrolyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(sMaND);
            String iID_MaPhongBan = "";
            String DK = "";
            String[] arrLNS = sLNS.Split(',');
            DK += " AND ( ";
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += "  sDSLNS LIKE @sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i] + "%");
            }
            DK += " ) ";
            if (bTrolyTongHop)
            {
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(sMaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                }
                DK += " AND 1=1 AND iID_MaPhongBanDich=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                dtPhongBan.Dispose();

            }
            else
            {
                DK += " AND 0=1";
            }
            iID_MaTrangThaiDuyet = 107;//Trang thai duyet truong phong lan1
            string SQL = String.Format(@"SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}
AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iCheckLan2=0", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable getDanhSachChungTu_TongHopDuyet(String sMaND, String sLNS)
        {
            DataTable vR;
            int iID_MaTrangThaiDuyet;
            SqlCommand cmd = new SqlCommand();
            bool bTrolyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(sMaND);
            String iID_MaPhongBan = "";
            String DK = "";
            String[] arrLNS = sLNS.Split(',');
            //DK += " AND ( ";
            //for (int i = 0; i < arrLNS.Length; i++)
            //{
            //    DK += "  sDSLNS LIKE @sLNS" + i;
            //    if (i < arrLNS.Length - 1)
            //        DK += " OR ";
            //    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i] + "%");
            //}
            //DK += " ) ";
            if (bTrolyTongHop)
            {

                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(sMaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                }
                DK += " AND 1=1 AND iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                dtPhongBan.Dispose();

            }
            else
            {
                DK += " AND 0=1";
            }
            //nếu là ngân sách bảo đảm
            if (sLNS == "1040100")
            {
                iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
                DK += " AND sDSLNS LIKE '104%'";

            }
            else
                iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
            string SQL = 
                String.Format(@"SELECT * 
                                FROM 
                                    DTBS_ChungTu 
                                WHERE 
                                    iTrangThai=1 AND 
                                    iNamLamViec=@iNamLamViec {0} AND 
                                    iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND 
                                    iCheck=0", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable getDanhSachChungTu_TongHopDuyet_Sua(String sMaND, String sLNS,String iID_MaChungTu)
        {
            DataTable vR = getDanhSachChungTu_TongHopDuyet(sMaND,sLNS);
            DataTable vR2;

            //int iID_MaTrangThaiDuyet;
            SqlCommand cmd = new SqlCommand();
            //bool bTrolyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(sMaND);
            //String iID_MaPhongBan = "";
            //String DK = "", DKCT = "" ;
            //String[] arrLNS = sLNS.Split(',');
            //DK += " AND ( ";
            //for (int i = 0; i < arrLNS.Length; i++)
            //{
            //    DK += "  sDSLNS LIKE @sLNS" + i;
            //    if (i < arrLNS.Length - 1)
            //        DK += " OR ";
            //    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i] + "%");
            //}
            //DK += " ) ";

            string DKCT ="";
            String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
            String[] arrChungtu = iID_MaChungTu_CT.Split(',');
            DKCT += " (";
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DKCT += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrChungtu.Length - 1)
                    DKCT += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

            }
            DKCT += " )";
            string SQL = string.Format(@"SELECT * FROM DTBS_ChungTu 
                                        WHERE 
                                            iTrangThai=1 AND 
                                            iNamLamViec=@iNamLamViec AND {0}", DKCT);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            vR2 = Connection.GetDataTable(cmd);
            cmd.Dispose();
            vR.Merge(vR2);
//            DKCT += " )";
//            if (bTrolyTongHop)
//            {

//                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(sMaND);
//                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
//                {
//                    DataRow drPhongBan = dtPhongBan.Rows[0];
//                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
//                }
//                DK += " AND 1=1 AND iID_MaPhongBan=@iID_MaPhongBan";
//                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
//                dtPhongBan.Dispose();

//            }
//            else
//            {
//                DK += " AND 0=1";
//            }
//            //nếu là ngân sách bảo đảm
//            if (sLNS == "1040100")
//            {
//                iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
//                DK += " AND sDSLNS LIKE '104%'";

//            }
//            else
//                iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
//            string SQL = String.Format(@"SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}
//AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND ( iCheck=0 OR {1} )   ", DK,DKCT);
//            cmd.CommandText = SQL;
//            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
//            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
//            vR = Connection.GetDataTable(cmd);
//            cmd.Dispose();
            return vR;
        }

        public static DataTable getDanhSachChungTu_TongHopCucDuyet(String sMaND)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            //Ma trang thai duyet =3: trolytonghop duyet
            int iID_MaTrangThaiDuyet = 3;
            string SQL = @"SELECT * FROM DTBS_ChungTu_TLTH WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec 
                         AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iCheck=0";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable getDanhSachChungTu_TongHopCucDuyet_Sua(String sMaND, string iID_MaChungTu)
        {
            DataTable vR;
            vR = getDanhSachChungTu_TongHopCucDuyet(sMaND);
            DataTable vR2;
            SqlCommand cmd = new SqlCommand();

            string DKCT = "";
            String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", iID_MaChungTu, "iID_MaChungTu_TLTH"));
            String[] arrChungtu = iID_MaChungTu_CT.Split(',');
            DKCT += " (";
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DKCT += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrChungtu.Length - 1)
                    DKCT += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrChungtu[j]);

            }
            DKCT += " )";

            string SQL = String.Format(@" SELECT * 
                            FROM 
                                DTBS_ChungTu_TLTH 
                            WHERE 
                                iTrangThai=1 AND 
                                iNamLamViec=@iNamLamViec AND {0}",DKCT);
                         
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            vR2 = Connection.GetDataTable(cmd);
            cmd.Dispose();
            vR.Merge(vR2);
            return vR;
        }


        public static DataTable getDanhSachChungTuNganhKyThuatChuyenBKhac(String sMaND)
        {
            DataTable vR;
            int iID_MaTrangThaiDuyet;
            SqlCommand cmd = new SqlCommand();
            bool bTrolyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(sMaND);
            String iID_MaPhongBan = "";
            String DK = "";

            if (bTrolyTongHop)
            {
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(sMaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                }
                DK += " AND 1=1 AND iID_MaPhongBan<>iID_MaPhongBanDich ";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                dtPhongBan.Dispose();
                DK += " AND 1=1 AND iID_MaPhongBanDich=@iID_MaPhongBan";

            }
            else
            {
                DK += " AND 0=1";
            }
            //nếu là ngân sách bảo đảm

            iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeChiTieu));


            string SQL = String.Format(@"SELECT * FROM (
SELECT DISTINCT iID_MaChungTu FROM DTBS_ChungTuChiTiet_PhanCap
WHERE sLNS=1040100 AND  iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}
 ) as a
INNER JOIN 
(SELECT iID_MaChungTuChiTiet,iID_MaChungTu as MaChungTu
 FROM DTBS_ChungTuChiTiet 
 WHERE iTrangThai=1 AND sLNS='1040100' AND iNamLamViec=@iNamLamViec) as CTCT
 ON CTCT.iID_MaChungTuChiTiet=a.iID_MaChungTu
INNER JOIN (SELECT * FROM DTBS_ChungTu WHERE iTrangThai=1 AND sDSLNS='1040100' AND iNamLamViec=@iNamLamViec AND iKyThuat=0) as b
ON CTCT.MaChungTu=b.iID_MaChungTu", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(sMaND));
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// kiem tra trang thai chung tu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int CheckTrangThaiChungTu_TLTH(String iID_MaChungTu_TLTH, String MaND, String sLNS = "")
        {
            String DK = "";
            String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu_TLTH, "iID_MaChungTu"));
            if (String.IsNullOrEmpty(iID_MaChungTu_TLTH)) iID_MaChungTu_TLTH = Guid.Empty.ToString();
            String[] arrChungtu = iID_MaChungTu_CT.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

            }
            String SQL = String.Format(@"SELECT a.iID_MaTrangThaiDuyet,iID_MaNhomNguoiDung FROM (
SELECT DISTINCT iID_MaTrangThaiDuyet
FROM DTBS_ChungTu
WHERE iTrangThai=1 AND ({0})) as a
INNER JOIN 
(SELECT * FROM NS_PhanHe_TrangThaiDuyet
WHERE iID_MaPhanHe=@iID_MaPhanHe AND iTrangThai=1) as b
ON a.iID_MaTrangThaiDuyet=b.iID_MaTrangThaiDuyet 

                                            ", DK);
            cmd.CommandText = SQL;
            if (sLNS == "1040100")
                cmd.Parameters.AddWithValue("@iID_MaPhanHe", PhanHeModels.iID_MaPhanHeDuToan);
            else
                cmd.Parameters.AddWithValue("@iID_MaPhanHe", PhanHeModels.iID_MaPhanHeDuToan);
            DataTable dtTrangThai = Connection.GetDataTable(cmd);
            bool TuChoi = false;
            int iTrangThaiTuChoi = 0;
            int iTrangThaiDangDuyet = 0;
            int iTrangThaiDaDuyet = 0;
            for (int i = 0; i < dtTrangThai.Rows.Count; i++)
            {
                if (sLNS == "1040100")
                    TuChoi = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]));
                else
                    TuChoi = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]));
                if (TuChoi == true)
                    return Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                else
                {
                    String iID_MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaND);
                    //nếu còn trạng thái đang duyệt
                    if (iID_MaNhomNguoiDung == Convert.ToString(dtTrangThai.Rows[i]["iID_MaNhomNguoiDung"]))
                        iTrangThaiDangDuyet = Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                    //nếu tất cả đều duyệt
                    else
                        iTrangThaiDaDuyet = Convert.ToInt16(dtTrangThai.Rows[i]["iID_MaTrangThaiDuyet"]);
                }
            }
            if (iTrangThaiDangDuyet > 0) return iTrangThaiDangDuyet;
            else return iTrangThaiDaDuyet;
        }

        public static void updateChungTu_TLTH(String iID_MaChungTu, String MaND, String sLNS)
        {
            String SQL = "";
            SQL = String.Format(@"SELECT * FROM DTBS_ChungTu_TLTH
WHERE iTrangThai=1 
AND iID_MaChungTu LIKE @iID_MaChungTu
");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", "%" + iID_MaChungTu + "%");
            String iID_MaChungTu_TLTH = Connection.GetValueString(cmd, Guid.Empty.ToString());
            int iID_MaTrangThaiDuyet = CheckTrangThaiChungTu_TLTH(iID_MaChungTu_TLTH, MaND, sLNS);

            SQL = "UPDATE DTBS_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu_TLTH=@iID_MaChungTu_TLTH";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH", iID_MaChungTu_TLTH);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        public static void updateLyDo_ChungTu(String iID_MaChungTu, String sLyDo)
        {
            String SQL = "";
            SqlCommand cmd = new SqlCommand(SQL);

            if (String.IsNullOrEmpty(sLyDo)) sLyDo = "";
            SQL = "UPDATE DTBS_ChungTu SET sLyDo=@sLyDo WHERE iID_MaChungTu=@iID_MaChungTu";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@sLyDo", sLyDo);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable getDanhSachChungTuKyThuat(String MaND,String iID_MaChungTu,String iID_MaDonVi,String sM,String sTM,String sTTM,String sNG)
        {
            DataTable vR;
            String DK = "";
            //Trang thai tro ly tong hop duyet lan 1
            int iID_MaTrangThaiDuyet = DuToan_ChungTuChiTietModels.iID_MaTrangThaiDuyetKT;
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "", SQL = ""; ;

            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
            
            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            DK = String.Format(@" iTrangThai=1 AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2}) "
                               , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(sM))
            {
                DK += " AND sM=@sM";
                cmd.Parameters.AddWithValue("@sM", sM);
            }
            if (!String.IsNullOrEmpty(sTM))
            {
                DK += " AND sTM=@sTM";
                cmd.Parameters.AddWithValue("@sTM", sTM);
            }
            if (!String.IsNullOrEmpty(sTTM))
            {
                DK += " AND sTTM=@sTTM";
                cmd.Parameters.AddWithValue("@sTTM", sTTM);
            }
            if (!String.IsNullOrEmpty(sNG))
            {
                DK += " AND sNG=@sNG";
                cmd.Parameters.AddWithValue("@sNG", sNG);
            }
            
            //nếu là ngân sách bảo đảm nganh ky thuat
            if (iID_MaPhongBan == "06")
            {

                 SQL = String.Format(@"SELECT * FROM DTBS_ChungTuChiTiet
WHERE {1} AND iKyThuat=1 AND MaLoai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaPhongBanDich='06' AND iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)
ORDER BY iID_MaDonVi,sM,sTM,sTTM,sNG", MaND, DK);
            }
            else
            {
                 SQL = String.Format(@"SELECT * FROM DTBS_ChungTuChiTiet
WHERE {1} AND iKyThuat=1 AND MaLoai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaDonVi IN (SELECT iID_MaNganh FROM NS_MucLucNganSach_Nganh
WHERE sMaNguoiQuanLy LIKE '%{0}%') AND iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)
ORDER BY iID_MaDonVi,sM,sTM,sTTM,sNG", MaND, DK);
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
          
            cmd.Dispose();
            return vR;
        }
        public static DataTable getDanhSachDonViKyThuat(String MaND, String iID_MaChungTu)
        {
            DataTable vR;
            String DK = "";
            //Trang thai tro ly tong hop duyet lan 1
            int iID_MaTrangThaiDuyet = DuToan_ChungTuChiTietModels.iID_MaTrangThaiDuyetKT;
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "", SQL = ""; ;

            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }

            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            DK = String.Format(@" iTrangThai=1 AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2}) "
                               , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            SqlCommand cmd = new SqlCommand();
            //nếu là ngân sách bảo đảm nganh ky thuat
            if (iID_MaPhongBan == "06")
            {

                SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi FROM DTBS_ChungTuChiTiet
WHERE {1} AND iKyThuat=1 AND MaLoai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaPhongBanDich='06' AND iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)
ORDER BY iID_MaDonVi", MaND, DK);
            }
            else
            {
                SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi FROM DTBS_ChungTuChiTiet
WHERE {1} AND iKyThuat=1 AND MaLoai=1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaDonVi IN (SELECT iID_MaNganh FROM NS_MucLucNganSach_Nganh
WHERE sMaNguoiQuanLy LIKE '%{0}%') AND iID_MaChungTu IN ( SELECT iID_MaChungTuChiTiet FROM DTBS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)
ORDER BY iID_MaDonVi", MaND, DK);
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return vR;
        }
        public static DataTable getDanhSachChungTuKyThuat_Bia(String MaND)
        {
            DataTable vR;
            String DK = "";
            //Trang thai tro ly tong hop duyet lan 1
            int iID_MaTrangThaiDuyet = DuToanBS_ChungTuChiTietModels.iID_MaTrangThaiDuyetKT;
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            // iSoChungTu = DuToanBS_ChungTuModels.iSoChungTu(iNamLamViec)+1;
            //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
            // bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
            String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }
            DK = String.Format(@" iTrangThai=1 AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2}) "
                               , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            SqlCommand cmd = new SqlCommand();
            //nếu là ngân sách bảo đảm nganh ky thuat
            String SQL = String.Format(@"SELECT * FROM DTBS_ChungTu
WHERE {0}  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet  ORDER BY dNgayChungTu DESC", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            vR = Connection.GetDataTable(cmd);

            cmd.Dispose();
            return vR;
        }
        public static void TaoDanhSachChungTuChiTapTrung(String MaND,String IP)
        {
            String iNamLamViec =Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            SQL = String.Format(@"SELECT DISTINCT NS_DonVi.iID_MaDonVi+' - '+NS_DonVi.sTen as TenHT, NS_DonVi.* FROM (SELECT * FROM NS_NguoiDung_DonVi
		                  WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sMaNguoiDung=@sMaND) AS NS_NguoiDung_DonVi 
                          INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) NS_DonVi
                           ON (NS_NguoiDung_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi)
                            WHERE NS_NguoiDung_DonVi.iTrangThai=1 AND NS_NguoiDung_DonVi.sMaNguoiDung=@sMaND
                             AND NS_DonVi.iID_MaDonVi  NOT IN (SELECT iID_MaDonVi FROM DTBS_ChungTu WHERE iTrangThai=1 AND sID_MaNguoiDungTao=@sMaND AND iNamLamViec=@iNamLamViec AND iID_MaDonVi IS NOT NULL)
                             ORDER BY NS_DonVi.iID_MaDonVi
                          ");
            cmd.Parameters.AddWithValue("@sMaND", MaND);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            // iSoChungTu = DuToanBS_ChungTuModels.iSoChungTu(iNamLamViec)+1;
            //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
            // bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
            String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Bang bang = new Bang("DTBS_ChungTu");
                bang.MaNguoiDungSua = MaND;
                bang.IPSua = IP;
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@sDSLNS", "1010000,1020000,1020100");
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dt.Rows[i]["iID_MaDonVi"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", iID_MaPhongBan);
                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(DuToanModels.iID_MaPhanHe));
                bang.Save();
            }
        }
        public static DataTable getNguon()
        {
            String SQL = "SELECT iID_MaNguon,sKyHieu+sTen as TenHT FROM DT_Nguon";
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }

        #region Thêm Chứng Từ Dự Toán Bổ Sung
        #region Thêm Chứng Từ
        /// <summary>
        /// Thêm chứng từ 
        /// </summary>
        /// <param name="bang">bảng dữ liệu</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <returns>Mã chứng từ mới</returns>
        public static string ThemChungTu(Bang bang, string maND, string sLNS)
        {
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);

            string iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
            if (dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
            //Gán giá trị trường
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(DuToanModels.iID_MaPhanHe));

            return Convert.ToString(bang.Save());
        }
        #endregion

        #region Thêm Chứng Từ TLTH
        /// <summary>
        /// Thêm Chứng Từ TLTH
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maND">Mã Người dùng</param>
        /// <param name="dsMaChungTu">Danh sách mã chứng từ</param>
        /// <returns></returns>
        public static string ThemChungTuTLTH(Bang bang, string maND, string dsMaChungTu)
        {
            string iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            string iID_MaNguonNganSach = "";
            string iID_MaNamNganSach = "";
            string iID_MaPhongBan = "";
            string sTenPhongBan = "";
            int iID_MaTrangThaiDuyet;
            //int iSoChungTu = 0;

            //Lấy cấu hình.
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);
            if (dtCauHinh != null && dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }
            //iSoChungTu = DuToanBS_ChungTuModels.iSoChungTu(iNamLamViec) + 1;
            //Lấy mã phòng ban.
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                iID_MaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                sTenPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sTen"]);
                dtPhongBan.Dispose();
            }

            #region Update trạng thái check chứng từ

            string dk = "";
            dsMaChungTu = Guid.Empty.ToString();
            string[] arrMaChungtu = dsMaChungTu.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungtu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungtu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungtu[j]);
            }

            string SQL = @"UPDATE DTBS_ChungTu SET iCheck=1 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            #endregion

            iID_MaTrangThaiDuyet = Convert.ToInt32(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", arrMaChungtu[0], "iID_MaTrangThaiDuyet"));
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", dsMaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
            //bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
            return Convert.ToString(bang.Save());
        }
        #endregion 

        #region Thêm Chứng Từ TLTH Cục
        /// <summary>
        /// Thêm Chứng Từ TLTH Cục
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="dsMaChungTuTLTH">Danh sách mã chứng từ TLTH</param>
        /// <returns>Mã chứng từ TLTHCuc mới tạo</returns>
        public static string ThemChungTuTLTHCuc(Bang bang, string maND, string dsMaChungTuTLTH)
        {
            //int iSoChungTu = 0;
            string iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            string iID_MaNguonNganSach = "";
            string iID_MaNamNganSach = "";
            string iID_MaPhongBan = "";
            string sTenPhongBan = "";

            //iSoChungTu = DuToanBS_ChungTuModels.iSoChungTu(iNamLamViec)+1;

            //Lấy cấu hinh
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(maND);
            if (dtCauHinh!=null && dtCauHinh.Rows.Count > 0)
            {
                iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                dtCauHinh.Dispose();
            }

            //Lấy phòng ban
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(maND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                iID_MaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                sTenPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sTen"]);
                dtPhongBan.Dispose();
            }
            #region Update trạng thái check chứng từ TLTH
            string dk = "";
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTuTLTH[j]);
            }
            String SQL = @"UPDATE DTBS_ChungTu_TLTH SET iCheck=1 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose(); 
            #endregion

            //Lấy mã trạng thái duyệt
            int iID_MaTrangThaiDuyet = Convert.ToInt32(
                CommonFunction.LayTruong(BangModels.DTBS_ChungTu_TLTH, "iID_MaChungTu_TLTH", arrMaChungTuTLTH[0], "iID_MaTrangThaiDuyet"));
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_TLTH", dsMaChungTuTLTH);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
            //bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
            return Convert.ToString(bang.Save());
        } 
        #endregion
        #endregion

        #region Sửa Chứng Từ Dự Toán Bổ Sung
        #region Sửa Chứng Từ
        /// <summary>
        /// Sửa chứng từ
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="MaChungTu">Mã chứng từ</param>
        /// <returns>kết quả</returns>
        public static int SuaChungTu(Bang bang, string MaChungTu)
        {
            bang.GiaTriKhoa = MaChungTu;
            bang.DuLieuMoi = false;
            bang.Save();
            return 1;
        }
        #endregion

        #region Sửa Chứng Từ TLTH
        /// <summary>
        /// Sửa Chứng Từ TLTH
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maChungTuTLTH">Mã chứng từ TLTH</param>
        /// <param name="dsMaChungTu">Danh sách mã chứng từ</param>
        /// <returns></returns>
        public static int SuaChungTuTLTH(Bang bang, string maChungTuTLTH, string dsMaChungTu)
        {
            string SQL = "";
            string dk = "";
            SqlCommand cmd = new SqlCommand();

            //update lai trang thai icheck=0 tat ca các đợt chứng từ của TLTH
            String dsMaChungTuCu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", maChungTuTLTH, "iID_MaChungTu"));
            String[] arrMaChungTuCu = dsMaChungTuCu.Split(',');
            dk += "AND (";
            for (int j = 0; j < arrMaChungTuCu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungTuCu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTuCu[j]);

            }
            dk += " )";
            SQL = String.Format("UPDATE DTBS_ChungTu SET iCheck=0 WHERE iTrangThai=1 {0}", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);

            //update lai trang thai icheck=1 cac chung tu duoc chon
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                dsMaChungTu = Guid.Empty.ToString();
            }
            string[] arrMaChungTu = dsMaChungTu.Split(',');
            cmd = new SqlCommand();
            dk = "";
            for (int j = 0; j < arrMaChungTu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungTu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungTu[j]);

            }
            SQL = String.Format("UPDATE DTBS_ChungTu SET iCheck=1 WHERE iTrangThai=1 AND({0})", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Cập nhật bản DTBS_ChungTu_TLTH
            bang.GiaTriKhoa = maChungTuTLTH;
            bang.CmdParams.Parameters.Add("@iID_MaChungTu", dsMaChungTu);
            bang.DuLieuMoi = false;
            bang.Save();
            return 1;
        }
        #endregion 

        #region Sửa Shứng Từ TLTH Cục
        /// <summary>
        /// Sửa chứng từ TLTH Cục
        /// </summary>
        /// <param name="bang">Bảng dữ liệu</param>
        /// <param name="maChungTuTLTHCuc">Mã chứng từ TLTH Cục</param>
        /// <param name="dsMaChungTuTLTH">Danh sách chứng từ TLTH</param>
        /// <returns></returns>
        public static int SuaChungTuTLTHCuc(Bang bang, string maChungTuTLTHCuc, string dsMaChungTuTLTH)
        {
            string SQL = "";
            string dk = "";
            SqlCommand cmd = new SqlCommand();

            //update lai trang thai icheck=0 tat ca các đợt chứng từ của TLTH
            String dsMaChungTuCu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", maChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            String[] arrMaChungTuCu = dsMaChungTuCu.Split(',');
            dk += "AND (";
            for (int j = 0; j < arrMaChungTuCu.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuCu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuCu[j]);

            }
            dk += " )";
            SQL = String.Format("UPDATE DTBS_ChungTu_TLTH SET iCheck=0 WHERE iTrangThai=1 {0}", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);

            //update lai trang thai icheck=1 cac chung tu duoc chon
            if (String.IsNullOrEmpty(dsMaChungTuTLTH))
            {
                dsMaChungTuTLTH = Guid.Empty.ToString();
            }
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            cmd = new SqlCommand();
            dk = "";
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuTLTH[j]);

            }
            SQL = String.Format("UPDATE DTBS_ChungTu_TLTH SET iCheck=1 WHERE iTrangThai=1 AND({0})", dk);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Cập nhật bản DTBS_ChungTu_TLTH
            bang.GiaTriKhoa = maChungTuTLTHCuc;
            bang.CmdParams.Parameters.Add("@iID_MaChungTu_TLTH", dsMaChungTuTLTH);
            bang.DuLieuMoi = false;
            bang.Save();
            return 1;
        } 
        #endregion
        #endregion

        #region Xóa Chứng Từ Dự Toán Bổ Sung
        #region Xóa Chứng Từ
        /// <summary>
        /// Xóa chứng dự toán bổ sung
        /// </summary>
        /// <param name="iID_MaChungTu">Mã chứng từ</param>
        /// <returns></returns>
        public static int XoaChungTu(string iID_MaChungTu)
        {
            SqlCommand cmd;
            //Xoa dữ liệu bang DTBS_ChungTuChiTiet_PhanCap
            string sql = String.Format(@"UPDATE DTBS_ChungTuChiTiet_PhanCap 
                                         SET iTrangThai = 0 
                                         WHERE iID_MaChungTuChiTiet IN (
                                             SELECT iID_MaChungTuChiTiet 
                                             FROM DTBS_ChungTuChiTiet_PhanCap
                                             WHERE iID_MaChungTu IN (
                                                 SELECT iID_MaChungTuChiTiet
                                                 FROM DTBS_ChungTuChiTiet
                                                 WHERE iID_MaChungTu=@iID_MaChungTu
                                            )
                                        ) ");
            cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu bảng DTBS_ChungTuChiTiet
            cmd = new SqlCommand("UPDATE DTBS_ChungTuChiTiet SET iTrangThai = 0 WHERE iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu bảng DTBS_ChungTu
            Bang bang = new Bang("DTBS_ChungTu");
            //bang.MaNguoiDungSua = MaNguoiDungSua;
            //bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.Delete();
            return 1;
        }
        #endregion

        #region Xóa Chứng Từ TLTH
        /// <summary>
        /// Xóa chứng từ gom TLTH
        /// </summary>
        /// <param name="iID_MaChungTu">Mã chứng từ TLTH</param>
        /// <returns></returns>
        public static int XoaChungTuTLTH(string iID_MaChungTuTLTH)
        {

            // Xóa dữ liệu bảng DTBS_ChungTu_TLTH
            Bang bang = new Bang(BangModels.DTBS_ChungTu_TLTH);
            bang.GiaTriKhoa = iID_MaChungTuTLTH;
            bang.Delete();

            // Update trạng thái gom của chứng từ bảng DTBS_ChungTu
            string dsMaChungTu = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTuTLTH,
                    "iID_MaChungTu"));
            string dk = "";
            string[] arrMaChungtu = dsMaChungTu.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungtu.Length; j++)
            {
                dk += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrMaChungtu.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrMaChungtu[j]);
            }
            string SQL = @"UPDATE DTBS_ChungTu SET iCheck=0 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return 1;
        }
        #endregion

        #region Xóa Chứng Từ TLTHCuc
        /// <summary>
        /// Xóa chứng từ TLTH Cục
        /// </summary>
        /// <param name="iID_MaChungTuTLTHCuc">Mã chứng tứ TLTH Cuc</param>
        /// <returns></returns>
        public static int XoaChungTuTLTHCuc(string iID_MaChungTuTLTHCuc)
        {
            //Xóa dữ liệu bảng DTBS_ChungTu_TLTHCuc
            Bang bang = new Bang(BangModels.DTBS_ChungTu_TLTHCuc);
            bang.GiaTriKhoa = iID_MaChungTuTLTHCuc;
            bang.Delete();

            //update lai trang thai check chứng từ TLTH
            string dsMaChungTuTLTH = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc",
                iID_MaChungTuTLTHCuc, "iID_MaChungTu_TLTH"));
            string dk = "";
            string[] arrMaChungTuTLTH = dsMaChungTuTLTH.Split(',');
            SqlCommand cmd = new SqlCommand();
            for (int j = 0; j < arrMaChungTuTLTH.Length; j++)
            {
                dk += " iID_MaChungTu_TLTH =@iID_MaChungTu_TLTH" + j;
                if (j < arrMaChungTuTLTH.Length - 1)
                    dk += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu_TLTH" + j, arrMaChungTuTLTH[j]);
            }
            String SQL = @"UPDATE DTBS_ChungTu_TLTH SET iCheck=0 WHERE iTrangThai=1 AND (" + dk + ")";
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return 1;
        }
        #endregion
        #endregion
    }
}