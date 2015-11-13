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
    public class QuyetToan_QuanSo_ChungTuChiTietModels
    {

        public static void ThemChiTiet(String iID_MaChungTu, String MaND, String IPSua)
        {
            DataTable dtChungTu = QuyetToan_QuanSo_ChungTuModels.GetChungTu(iID_MaChungTu);
            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iThang_Quy = Convert.ToInt32(dtChungTu.Rows[0]["iThang_Quy"]);
            int bLoaiThang_Quy = Convert.ToInt32(dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtChungTu.Rows[0]["bChiNganSach"]);
            String sLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);

            DataTable dt = QuyetToan_QuanSo_MucLucModels.DT_MucLucQuanSo();

            Bang bang = new Bang("QTQS_ChungTuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);

            DataTable dtLuongChiTiet = new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Dien thong tin cua Muc luc quân số vào bảng QTQS_ChungTuChiTiet
                NganSach_HamChungModels.ThemThongTinCuaMucLucQuanSoKhongLayTruongTien(dt.Rows[i], bang.CmdParams.Parameters);
                bang.Save();
            }
            //lấy thông tin quân số từ lương
            //  Insert_FROM_Luong(iID_MaChungTu, MaND, IPSua);

            dt.Dispose();
            dtChungTu.Dispose();
        }

        public static void ThemQuanSoBienChe(String iID_MaChungTu, String MaND, String IPSua)
        {
            DataTable dtChungTu = QuyetToan_QuanSo_ChungTuModels.GetChungTu(iID_MaChungTu);
            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iThang_Quy = Convert.ToInt32(dtChungTu.Rows[0]["iThang_Quy"]);
            int bLoaiThang_Quy = Convert.ToInt32(dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtChungTu.Rows[0]["bChiNganSach"]);
            String sLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);

            Bang bang = new Bang("QTQS_ChungTuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);

            DataTable dtBienChe = Get_QuanSoBienChe(iNamLamViec, iThang_Quy,
                                                    Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]),
                                                    Convert.ToString(dtChungTu.Rows[0]["iLoai"]));
            if (dtBienChe.Rows.Count > 0)
            {
                for (int i = 0; i < dtBienChe.Columns.Count; i++)
                {
                    bang.CmdParams.Parameters.AddWithValue("@" + dtBienChe.Columns[i].ColumnName, dtBienChe.Rows[0][i]);
                }
                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", dtChungTu.Rows[0]["sTenPhongBan"]);
                bang.CmdParams.Parameters.AddWithValue("@sKyHieu", "000");
                bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucQuanSo", "3F5120A6-0B80-4E42-A68F-9CA20A08295A");
                bang.CmdParams.Parameters.AddWithValue("@sMoTa", "Quân số theo biên chế");
                bang.Save();

            }



            dtChungTu.Dispose();
        }

        public static String InsertDuyetQuyetToan(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("QTQS_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = QuyetToan_QuanSo_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeQuyetToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM QTQS_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = QuyetToan_QuanSo_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeQuyetToan, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtQuyetToanChiTiet(String iLoai, String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            String sTruongTien = MucLucQuanSoModels.strDSTruongTien + ",rTongSo,iID_MaChungTuChiTiet,iThang_Quy";
            String[] arrDSTruongTien = sTruongTien.Split(',');

            DataTable dtChungTu = QuyetToan_QuanSo_ChungTuModels.GetChungTu(iID_MaChungTu);

            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iThang = Convert.ToInt32(dtChungTu.Rows[0]["iThang_Quy"]);
            String iID_MaDonVi = Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]);
            DataTable dtBienChe = Get_QuanSoBienChe(iNamLamViec, iThang, iID_MaDonVi, iLoai);
            DataTable dtThangtruoc = Get_QuanSoThangTruoc(iNamLamViec, iThang, iID_MaDonVi, iLoai);
            DataTable dtThangNay = Get_QuanSoThangTruoc(iNamLamViec, iThang + 1, iID_MaDonVi, iLoai);
            //So ke Hoach
            DK = "iTrangThai=1 ";
            if (iLoai == "2")
            {
                sTruongTien = "rSQ_KH,rHSQBS_KH,rCNVQP_KH,rQNCN_KH,rLDHD_KH,sGhiChu" + ",rTongSo,iID_MaChungTuChiTiet,iThang_Quy";
                arrDSTruongTien = sTruongTien.Split(',');
                DK += " AND sHienThi<>'1' ";

            }
            else
            {
                DK += "AND sHienThi<>'2' ";

            }

            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }

            SQL = String.Format("SELECT * FROM NS_MucLucQuanSo WHERE {0} ORDER BY sKyHieu", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            //lay du lieu bang cchi tiet




            cmd.Dispose();
            DataColumn column;
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j].StartsWith("r") == true)
                {
                    column = new DataColumn(arrDSTruongTien[j], typeof(Double));
                    column.DefaultValue = 0;
                    vR.Columns.Add(column);
                }
                else
                {
                    column = new DataColumn(arrDSTruongTien[j], typeof(String));
                    column.AllowDBNull = true;
                    vR.Columns.Add(column);
                }
            }
            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            SQL = String.Format("SELECT * FROM QTQS_ChungTuChiTiet WHERE {0} ORDER BY sKyHieu", DK);
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;

            dtChungTuChiTiet.Columns.Add("sHienThi", typeof(String));
            int vRCount = vR.Rows.Count;

            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    //for (int k = 0; k < arrDSTruong.Length; k++)
                    //{
                    if (Convert.ToString(vR.Rows[i]["sKyHieu"]) != Convert.ToString(dtChungTuChiTiet.Rows[j]["sKyHieu"]))
                    {
                        ok = false;
                        //  break;
                    }
                    //}
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < vR.Columns.Count; k++)
                            {
                                if (vR.Columns[k].ColumnName == "sMoTa") continue;
                                vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row = vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count; k++)
                            {
                                row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            vR.Rows.InsertAt(row, i + 1);
                            i++;
                            vRCount++;
                        }
                    }
                }
                if (Convert.ToString(vR.Rows[i]["sKyHieu"]) == "000")
                {
                    for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                    {

                        if (dtBienChe.Rows.Count > 0)
                            vR.Rows[i][dtBienChe.Columns[j + 1].ColumnName] = dtBienChe.Rows[0][j + 1];


                    }
                }
                if (Convert.ToString(vR.Rows[i]["sKyHieu"]) == "100")
                {
                    for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                    {
                            vR.Rows[i][dtThangtruoc.Columns[j].ColumnName] = dtThangtruoc.Rows[0][j];
                    }
                }
                if (Convert.ToString(vR.Rows[i]["sKyHieu"]) == "700")
                {
                    for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                    {
                            vR.Rows[i][dtThangNay.Columns[j].ColumnName] = dtThangNay.Rows[0][j];
                    }
                }
                //Tinh o tong số

                decimal tong = 0;

                for (int j = 0; j < arrDSTruongTien.Length - 4; j++)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(vR.Rows[i][arrDSTruongTien[j]])))
                        tong += Convert.ToDecimal(vR.Rows[i][arrDSTruongTien[j]]);
                }
                vR.Rows[i]["rTongSo"] = tong;



            }
            return vR;
        }

        public static DataTable Get_dtQuanSoTuLuong(int iNamBangLuong, int iThangBangLuong, int bLoaiThang_Quy, String iID_MaDonVi)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (bLoaiThang_Quy == 1)
            {
                switch (iThangBangLuong)
                {
                    case 1:
                        DK = " AND (iThangBangLuong=1 OR iThangBangLuong=2 OR iThangBangLuong=3) ";
                        break;
                    case 2:
                        DK = " AND (iThangBangLuong=4 OR iThangBangLuong=5 OR iThangBangLuong=6) ";
                        break;
                    case 3:
                        DK = " AND (iThangBangLuong=7 OR iThangBangLuong=8 OR iThangBangLuong=9) ";
                        break;
                    case 4:
                        DK = " AND (iThangBangLuong=10 OR iThangBangLuong=11 OR iThangBangLuong=12) ";
                        break;
                }
            }
            else
            {
                DK = " AND iThangBangLuong=@iThangBangLuong";
                cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            }
            DK += " AND iNamBangLuong=@iNamBangLuong ";

            String SQL = "SELECT COUNT(*) AS QuanSo,sHieuTangGiam";
            SQL += ",sKyHieu_MucLucQuanSo_HieuTangGiam AS sKyHieu";
            SQL += ",iID_MaNgachLuong_CanBo,iID_MaBacLuong_CanBo ";
            SQL += " FROM L_BangLuongChiTiet ";
            SQL += " WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND iID_MaDonVi=@iID_MaDonVi {0}";
            SQL += " GROUP BY sHieuTangGiam,sKyHieu_MucLucQuanSo_HieuTangGiam,iID_MaBacLuong_CanBo,iID_MaNgachLuong_CanBo";
            SQL += " order by sKyHieu_MucLucQuanSo_HieuTangGiam";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        private static String Get_QuanHam(String iID_MaNgachLuong, String iID_MaBacLuong)
        {
            String SQL = "SELECT sQuanHam FROM L_DanhMucBacLuong WHERE iID_MaNgachLuong=@iID_MaNgachLuong AND iID_MaBacLuong=@iID_MaBacLuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
            cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }

        public static void Insert_FROM_Luong(String iID_MaChungTu, String MaND, String IPSua)
        {
            DataTable dtChungTu = QuyetToan_QuanSo_ChungTuModels.GetChungTu(iID_MaChungTu);
            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iThang_Quy = Convert.ToInt32(dtChungTu.Rows[0]["iThang_Quy"]);
            int bLoaiThang_Quy = Convert.ToInt32(dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaDonVi = Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtChungTu.Rows[0]["bChiNganSach"]);
            String sLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);

            DataTable dtQuanSo = Get_dtQuanSoTuLuong(iNamLamViec, iThang_Quy, bLoaiThang_Quy, iID_MaDonVi);

            DataTable dt = Get_dtQuyetToanChiTiet("", iID_MaChungTu, null);


            Int64 Tong_Tang = 0, Tong_Giam = 0;
            String MaTangTrongThang = "", MaGiamTrongThang = "", MaHangQuanSoThangTruoc = "", Ma400 = "", Ma700 = "";
            if (iThang_Quy == 0)
            {
                iThang_Quy = 13;
                iNamLamViec = iNamLamViec - 1;
            }
            DataTable dtQSThangTruoc = Get_QuanSoThangTruoc(iNamLamViec, iThang_Quy - 1, iID_MaDonVi);
            DataTable dtQSBienChe = Get_QuanSoBienChe(iNamLamViec, iThang_Quy - 1, iID_MaDonVi, "1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String sKyHieu = Convert.ToString(dt.Rows[i]["sKyHieu"]);

                if ((sKyHieu == "100") && dtQSThangTruoc.Rows.Count > 0)
                {
                    MaHangQuanSoThangTruoc = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);
                    for (int j = 0; j < dtQSThangTruoc.Columns.Count; j++)
                    {
                        String TenCot = dtQSThangTruoc.Columns[j].ColumnName;

                        Bang bang = new Bang("QTQS_ChungTuChiTiet");
                        bang.DuLieuMoi = false;
                        bang.MaNguoiDungSua = MaND;
                        bang.IPSua = IPSua;
                        bang.GiaTriKhoa = MaHangQuanSoThangTruoc;
                        bang.CmdParams.Parameters.AddWithValue("@" + TenCot, dtQSThangTruoc.Rows[0][TenCot]);
                        bang.Save();
                    }
                }
                if (sKyHieu == "000" && dtQSBienChe.Rows.Count > 0)
                {
                    MaHangQuanSoThangTruoc = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);
                    for (int j = 0; j < dtQSBienChe.Columns.Count; j++)
                    {
                        String TenCot = dtQSBienChe.Columns[j].ColumnName;

                        Bang bang = new Bang("QTQS_ChungTuChiTiet");
                        bang.DuLieuMoi = false;
                        bang.MaNguoiDungSua = MaND;
                        bang.IPSua = IPSua;
                        bang.GiaTriKhoa = MaHangQuanSoThangTruoc;
                        bang.CmdParams.Parameters.AddWithValue("@" + TenCot, dtQSBienChe.Rows[0][TenCot]);
                        bang.Save();
                    }
                }




                if (sKyHieu == "2") MaTangTrongThang = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);

                if (sKyHieu == "3") MaGiamTrongThang = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);

                if (sKyHieu == "400") Ma400 = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);
                if (sKyHieu == "700") Ma700 = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);

                for (int j = 0; j < dtQuanSo.Rows.Count; j++)
                {
                    String sKyHieu1 = Convert.ToString(dtQuanSo.Rows[j]["sKyHieu"]);

                    if (sKyHieu == sKyHieu1)
                    {

                        String iID_MaBacLuong = Convert.ToString(dtQuanSo.Rows[j]["iID_MaBacLuong_CanBo"]);
                        String iID_MaNgachLuong = Convert.ToString(dtQuanSo.Rows[j]["iID_MaNgachLuong_CanBo"]);
                        String TenTruong = Get_QuanHam(iID_MaNgachLuong, iID_MaBacLuong);

                        Bang bang = new Bang("QTQS_ChungTuChiTiet");
                        bang.DuLieuMoi = false;
                        bang.MaNguoiDungSua = MaND;
                        bang.IPSua = IPSua;
                        bang.GiaTriKhoa = dt.Rows[i]["iID_MaChungTuChiTiet"];

                        for (int c = 0; c < dtQSThangTruoc.Columns.Count; c++)
                        {
                            String TenCot = dtQSThangTruoc.Columns[c].ColumnName;
                            if (TenTruong == TenCot)
                            {
                                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, dtQuanSo.Rows[j]["QuanSo"]);
                            }
                            else
                            {
                                bang.CmdParams.Parameters.AddWithValue("@" + TenCot, 0);
                            }
                        }
                        bang.Save();
                    }
                }
            }
            dt.Dispose();
            dtChungTu.Dispose();
            dtQuanSo.Dispose();
            dt = Get_dtQuyetToanChiTiet("", iID_MaChungTu, null);

            Bang _bang;
            _bang = new Bang("QTQS_ChungTuChiTiet");
            _bang.DuLieuMoi = false;
            _bang.MaNguoiDungSua = MaND;
            _bang.IPSua = IPSua;
            _bang.GiaTriKhoa = MaTangTrongThang;

            Bang _bang400;
            _bang400 = new Bang("QTQS_ChungTuChiTiet");
            _bang400.DuLieuMoi = false;
            _bang400.MaNguoiDungSua = MaND;
            _bang400.IPSua = IPSua;
            _bang400.GiaTriKhoa = Ma400;

            Int64 QSThangTruoc = 0, QS = 0;
            //if(dtQSThangTruoc.Rows.Count > 0){
            for (int j = 0; j < dtQSThangTruoc.Columns.Count; j++)
            {
                Tong_Giam = 0;
                Tong_Tang = 0;
                String TenCot = dtQSThangTruoc.Columns[j].ColumnName;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String sKyHieu = Convert.ToString(dt.Rows[i]["sKyHieu"]);
                    if (sKyHieu.StartsWith("2") && sKyHieu != "2")
                        Tong_Tang = Tong_Tang + Convert.ToInt64(dt.Rows[i][TenCot]);
                    if (sKyHieu.StartsWith("3") && sKyHieu != "3")
                        Tong_Giam = Tong_Giam + Convert.ToInt64(dt.Rows[i][TenCot]);
                }
                _bang.CmdParams.Parameters.AddWithValue("@" + TenCot, Tong_Tang);
                if (dtQSThangTruoc.Rows.Count > 0)
                {
                    QSThangTruoc = Convert.ToInt64(dtQSThangTruoc.Rows[0][TenCot]);
                }
                QS = QSThangTruoc + Tong_Tang - Tong_Giam;
                _bang400.CmdParams.Parameters.AddWithValue("@" + TenCot, QS);

            }
            //}
            _bang.Save();
            _bang400.Save();
            _bang400.CmdParams.Parameters.RemoveAt(_bang400.CmdParams.Parameters.IndexOf("@iID_MaChungTuChiTiet"));
            _bang400.GiaTriKhoa = Ma700;
            _bang400.Save();

            _bang = new Bang("QTQS_ChungTuChiTiet");
            _bang.DuLieuMoi = false;
            _bang.MaNguoiDungSua = MaND;
            _bang.IPSua = IPSua;
            _bang.GiaTriKhoa = MaGiamTrongThang;
            for (int j = 0; j < dtQSThangTruoc.Columns.Count; j++)
            {
                Tong_Giam = 0;
                String TenCot = dtQSThangTruoc.Columns[j].ColumnName;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String sKyHieu = Convert.ToString(dt.Rows[i]["sKyHieu"]);
                    if (sKyHieu.StartsWith("3") && sKyHieu != "3")
                        Tong_Giam = Tong_Giam + Convert.ToInt64(dt.Rows[i][TenCot]);
                }
                _bang.CmdParams.Parameters.AddWithValue("@" + TenCot, Tong_Giam);
            }
            _bang.Save();
            dtQSThangTruoc.Dispose();
        }

        public static DataTable Get_QuanSoThangTruoc(int iNamLamViec, int iThang, String iID_MaDonVi)
        {
            String SQL = "SELECT ";
            SQL += "rThieuUy,rTrungUy,rThuongUy,rDaiUy,rThieuTa,rTrungTa,rThuongTa,rDaiTa,rTuong,rTSQ,rBinhNhi";
            SQL += ",rBinhNhat,rHaSi,rTrungSi,rThuongSi,rQNCN,rCNVQPCT,rQNVQPHD";
            SQL += " FROM QTQS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy AND sKyHieu=@sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            cmd.Parameters.AddWithValue("@sKyHieu", "700");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_QuanSoBienChe(int iNamLamViec, int iThang, String iID_MaDonVi, String iLoai)
        {
            String sTruongTien = MucLucQuanSoModels.strDSTruongTien;
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String DKSELECT = "";

            if (iLoai == "2")
            {
                sTruongTien = "rSQ_KH,rHSQBS_KH,rCNVQP_KH,rQNCN_KH,rLDHD_KH,sGhiChu";
                arrDSTruongTien = sTruongTien.Split(',');
            }
            for (int i = 0; i < arrDSTruongTien.Length - 1; i++)
            {
                DKSELECT += "SUM (" + arrDSTruongTien[i] + ") as " + arrDSTruongTien[i];
                if (i < arrDSTruongTien.Length - 2)
                    DKSELECT += " , ";
            }
            String SQL = String.Format(@"SELECT skyHIeu, {0}  FROM QTQS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy<=@iThang_Quy AND sKyHieu=@sKyHieu GROUP BY skyHIeu", DKSELECT);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            cmd.Parameters.AddWithValue("@sKyHieu", "000");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_QuanSoBienChe_ThangTruoc(int iNamLamViec, int iThang, String iID_MaDonVi, String iLoai)
        {
            String sTruongTien = MucLucQuanSoModels.strDSTruongTien;
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String DKSELECT = "";

            if (iLoai == "2")
            {
                sTruongTien = "rSQ_KH,rHSQBS_KH,rCNVQP_KH,rQNCN_KH,rLDHD_KH,sGhiChu";
                arrDSTruongTien = sTruongTien.Split(',');
            }
            for (int i = 0; i < arrDSTruongTien.Length - 1; i++)
            {
                DKSELECT += "SUM (" + arrDSTruongTien[i] + ") as " + arrDSTruongTien[i];
                if (i < arrDSTruongTien.Length - 2)
                    DKSELECT += " , ";
            }
            String SQL = String.Format(@"SELECT skyHIeu, {0}  FROM QTQS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy<@iThang_Quy AND sKyHieu=@sKyHieu GROUP BY skyHIeu", DKSELECT);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            cmd.Parameters.AddWithValue("@sKyHieu", "000");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static int Get_MaxThang(int iNamLamViec, String iID_MaDonVi)
        {

            String SQL = String.Format(@"SELECT Max(iThang_Quy)  FROM QTQS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy<=@iThang_Quy AND sKyHieu=@sKyHieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang_Quy", 12);
            cmd.Parameters.AddWithValue("@sKyHieu", "000");
            int iThang = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return iThang;
        }
        public static DataTable Get_QuanSoThangTruoc(int iNamLamViec, int iThang, String iID_MaDonVi, String iLoai)
        {
            String sTruongTien = MucLucQuanSoModels.strDSTruongTien;
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String DKSELECT = "";

            if (iLoai == "2")
            {
                sTruongTien = "rSQ_KH,rHSQBS_KH,rCNVQP_KH,rQNCN_KH,rLDHD_KH,sGhiChu";
                arrDSTruongTien = sTruongTien.Split(',');
            }
            for (int i = 0; i < arrDSTruongTien.Length - 1; i++)
            {
                DKSELECT += arrDSTruongTien[i] + "=SUM(CASE WHEN sKyHieu='2' THEN " + arrDSTruongTien[i] + " ELSE 0 END)- SUM(CASE WHEN sKyHieu='3' THEN " + arrDSTruongTien[i] + " ELSE 0 END) +SUM(CASE WHEN sKyHieu='500' THEN " + arrDSTruongTien[i] + " ELSE 0 END) -SUM(CASE WHEN sKyHieu='600' THEN " + arrDSTruongTien[i] + " ELSE 0 END)";
                if (i < arrDSTruongTien.Length - 2)
                    DKSELECT += " , ";
            }
            String SQL = String.Format(@"SELECT {0}  FROM QTQS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy<=@iThang_Quy ", DKSELECT);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang - 1);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static int update_QuanSoBienChe(int iNamLamViec, int iThang, String iID_MaDonVi, String iLoai, String iID_MaChungTu)
        {
            String sTruongTien = MucLucQuanSoModels.strDSTruongTien;
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String DKSELECT = "";


            String SQLTHangTruoc = "";
            if (iLoai == "2")
            {
                sTruongTien = "rSQ_KH,rHSQBS_KH,rCNVQP_KH,rQNCN_KH,rLDHD_KH,sGhiChu";
                arrDSTruongTien = sTruongTien.Split(',');
            }
            for (int i = 0; i < arrDSTruongTien.Length - 1; i++)
            {
                DKSELECT += arrDSTruongTien[i];
                if (i < arrDSTruongTien.Length - 2)
                    DKSELECT += " , ";
            }
            //Lay dt quan so bien che thang truoc

            String SQL = String.Format("@UPDATE QTQS_ChungTuChiTiet SET ");
            return 1;
        }

    }
}