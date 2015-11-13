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
    public class BaoHiem_ChungTuChiChiTietModels
    {
        public static void ThemChiTiet(String iID_MaChungTuChi, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            //String[] arrDSHangChiTiet = BaoHiemChiModels.strDSHangChiTiet.Split(',');

            DataTable dtChungTu = BaoHiem_ChungTuChiModels.GetChungTu(iID_MaChungTuChi);
            
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaDonVi = Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]);
            String sTenDonVi = Convert.ToString(dtChungTu.Rows[0]["sTenDonVi"]);
            String iThang_Quy = Convert.ToString(dtChungTu.Rows[0]["iThang_Quy"]);
            int bLoaiThang_Quy = Convert.ToInt16(dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            int bChi = Convert.ToInt16(dtChungTu.Rows[0]["bChi"]);
            DataTable dt;
            if (bChi == 1)
            {
                dt = NganSach_HamChungModels.DT_MucLucNganSach_sLNS("2200000");
            }
            else
            {
                String sLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);
                dt = NganSach_HamChungModels.DT_MucLucNganSach_sLNS(sLNS);
            }
            Bang bang = new Bang("BH_ChungTuChiChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi",sTenDonVi);
            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", bLoaiThang_Quy);
            bang.CmdParams.Parameters.AddWithValue("@bChi", bChi);
            //add params muc luc ngan sach
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Dien thong tin cua Muc luc ngan sach
                ThemThongTinCuaMucLucNganSach(dt.Rows[i], bang.CmdParams.Parameters);
                //Xet rieng ngan sach thuong xuyen              
                bang.Save();
            }

            dt.Dispose();
            dtChungTu.Dispose();
        }
        public static void ThemChiTiet2(String iID_MaChungTuChi, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSHangChiTiet = BaoHiemChiModels.strDSHangChiTiet.Split(',');
            String[] arrDSTruong_HangChiTiet = BaoHiemChiModels.strDSTruong_HangChiTiet.Split(',');

            DataTable dtChungTu = BaoHiem_ChungTuChiModels.GetChungTu(iID_MaChungTuChi);

            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaDonVi = Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]);
            String sTenDonVi = Convert.ToString(dtChungTu.Rows[0]["sTenDonVi"]);
            String iThang_Quy = Convert.ToString(dtChungTu.Rows[0]["iThang_Quy"]);
            int bLoaiThang_Quy = Convert.ToInt16(dtChungTu.Rows[0]["bLoaiThang_Quy"]);
            int bChi = Convert.ToInt16(dtChungTu.Rows[0]["bChi"]);

            Bang bang = new Bang("BH_ChungTuChiChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;

            DataTable tempTbl = Get_dtLuongBaoHiem(iNamLamViec.ToString(), iThang_Quy, bLoaiThang_Quy, iID_MaDonVi);
            for (int i = 0; i < arrDSHangChiTiet.Length; i++)
            {
                bang.CmdParams.Parameters.Clear();
                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
                bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", bLoaiThang_Quy);
                bang.CmdParams.Parameters.AddWithValue("@bChi", bChi);
                bang.CmdParams.Parameters.AddWithValue("@sMoTa", arrDSHangChiTiet[i]);
                if (arrDSTruong_HangChiTiet[i] != "")
                {
                    bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", 0);
                }
                else
                {
                    bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", 1);
                }
                if (tempTbl.Rows.Count > 0)
                {
                    Decimal TongSo = 0;
                    for (int j = 0; j < tempTbl.Rows.Count; j++)
                    {
                        int MaNgachLuong = Convert.ToInt32(tempTbl.Rows[j]["iID_MaNgachLuong_CanBo"]);
                        String truongSoLuong = "";
                        String truongSoTien = "";
                        switch (MaNgachLuong)
                        {
                            case 1:
                                truongSoLuong = "@rSLSQ";
                                truongSoTien = "@rTienSQ";
                                break;
                            case 2:
                                truongSoLuong = "@rSLQNCN";
                                truongSoTien = "@rTienQNCN";
                                break;
                            case 3:
                                truongSoLuong = "@rSLCNV";
                                truongSoTien = "@rTienCNV";
                                break;
                            case 4:
                                truongSoLuong = "@rSLHSQ_CS";
                                truongSoTien = "@rTienHSQ_CS";
                                break;
                            case 5:
                                truongSoLuong = "@rSLHD";
                                truongSoTien = "@rTienHD";
                                break;
                        }
                        if (bang.CmdParams.Parameters.IndexOf(truongSoLuong) >= 0)
                        {
                            if (arrDSTruong_HangChiTiet[i] != "")
                            {
                                bang.CmdParams.Parameters[truongSoLuong].Value = (int)(bang.CmdParams.Parameters[truongSoLuong].Value) + (int)(tempTbl.Rows[j][arrDSTruong_HangChiTiet[i] + "_Count"]);
                                bang.CmdParams.Parameters[truongSoTien].Value = (Decimal)(bang.CmdParams.Parameters[truongSoTien].Value) + (Decimal)(tempTbl.Rows[j][arrDSTruong_HangChiTiet[i]]);
                                TongSo = TongSo + (Decimal)(tempTbl.Rows[j][arrDSTruong_HangChiTiet[i]]);
                            }
                            else
                            {
                                //bang.CmdParams.Parameters[truongSoLuong].Value = 0;
                                //bang.CmdParams.Parameters[truongSoTien].Value = 0;
                            }
                        }
                        else
                        {
                            if (arrDSTruong_HangChiTiet[i] != "")
                            {
                                bang.CmdParams.Parameters.AddWithValue(truongSoLuong, (int)tempTbl.Rows[j][arrDSTruong_HangChiTiet[i] + "_Count"]);
                                bang.CmdParams.Parameters.AddWithValue(truongSoTien, (Decimal)tempTbl.Rows[j][arrDSTruong_HangChiTiet[i]]);
                                TongSo = TongSo + (Decimal)(tempTbl.Rows[j][arrDSTruong_HangChiTiet[i]]);
                            }
                            else
                            {
                                //bang.CmdParams.Parameters.AddWithValue(truongSoLuong, 0);
                                //bang.CmdParams.Parameters.AddWithValue(truongSoTien, 0);
                            }
                        }
                    }
                    bang.CmdParams.Parameters.AddWithValue("@rTongSo", TongSo);
                }
                bang.Save();
            }
            //dt.Dispose();
            tempTbl.Dispose();
            dtChungTu.Dispose();
        }

        public static void ThemThongTinCuaMucLucNganSach(DataRow RMucLucNganSach, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            //String[] arrDSDuocNhapTruongTien = MucLucNganSachModels.strDSDuocNhapTruongTien.Split(',');
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String sXauNoiMa = "";
            if (Params.IndexOf("@iID_MaMucLucNganSach") >= 0)
            {
                Params["@iID_MaMucLucNganSach"].Value = RMucLucNganSach["iID_MaMucLucNganSach"];
                Params["@iID_MaMucLucNganSach_Cha"].Value = RMucLucNganSach["iID_MaMucLucNganSach_Cha"];
                Params["@bLaHangCha"].Value = RMucLucNganSach["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaMucLucNganSach", RMucLucNganSach["iID_MaMucLucNganSach"]);
                Params.AddWithValue("@iID_MaMucLucNganSach_Cha", RMucLucNganSach["iID_MaMucLucNganSach_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucNganSach["bLaHangCha"]);
            }
            //for (int i = 0; i < arrDSDuocNhapTruongTien.Length; i++)
            //{
            //    if (Params.IndexOf("@" + arrDSDuocNhapTruongTien[i]) >= 0)
            //    {
            //        Params["@" + arrDSDuocNhapTruongTien[i]].Value = RMucLucNganSach[arrDSDuocNhapTruongTien[i]];
            //    }
            //    else
            //    {
            //        Params.AddWithValue("@" + arrDSDuocNhapTruongTien[i], RMucLucNganSach[arrDSDuocNhapTruongTien[i]]);
            //    }
            //}
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucNganSach[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucNganSach[arrDSTruong[i]]);
                }
                if (i < arrDSTruong.Length - 1 && String.IsNullOrEmpty(Convert.ToString(RMucLucNganSach[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucNganSach[arrDSTruong[i]]);
                }
            }
            if (Params.IndexOf("@sXauNoiMa") >= 0)
            {
                Params["@sXauNoiMa"].Value = sXauNoiMa;
            }
            else
            {
                Params.AddWithValue("@sXauNoiMa", sXauNoiMa);
            }
        }

        public static String InsertDuyetBaoHiem(String iID_MaChungTuChi, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("BH_DuyetChungTuChi");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static void UpdateBangChiTiet(String UserName, String Address, String iID_MaChungTuChi, String iID_MaDonVi, String sTenDonVi, String iThang_Quy, String bLoaiThang_Quy)
        {
            Bang bang = new Bang("BH_ChungTuChiChiTiet");
            bang.MaNguoiDungSua = UserName;
            bang.IPSua = Address;
            bang.DuLieuMoi = false;
            String SQL = "UPDATE BH_ChungTuChiChiTiet SET iID_MaDonVi=@iID_MaDonVi,sTenDonVi=@sTenDonVi";
            SQL += ",iThang_Quy=@iThang_Quy,bLoaiThang_Quy=@bLoaiThang_Quy ";
            SQL += ",sID_MaNguoiDungSua=@sID_MaNguoiDungSua,sIPSua=@sIPSua ";
            SQL += " WHERE iID_MaChungTuChi=@iID_MaChungTuChi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@bLoaiThang_Quy", bLoaiThang_Quy);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", UserName);
            cmd.Parameters.AddWithValue("@sIPSua", Address);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTuChi)
        {
            int vR = -1;
            DataTable dt = BaoHiem_ChungTuChiModels.GetChungTu(iID_MaChungTuChi);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM BH_ChungTuChiChiTiet WHERE iID_MaChungTuChi=@iID_MaChungTuChi AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChungTuChi)
        {
            int vR = -1;
            DataTable dt = BaoHiem_ChungTuChiModels.GetChungTu(iID_MaChungTuChi);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtBaoHiemChiChiTiet(String iID_MaChungTuChi, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChungTuChi=@iID_MaChungTuChi";
            cmd.Parameters.AddWithValue("@iID_MaChungTuChi", iID_MaChungTuChi);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
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

            SQL = String.Format("SELECT * FROM BH_ChungTuChiChiTiet WHERE {0} ORDER BY  sXauNoiMa, iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_dtLuongBaoHiem(String iNamLamViec, String iThang_Quy, int bLoaiThang_Quy, String iID_MaDonVi)
        {
            String strThang = "";
            if (bLoaiThang_Quy == 0){
                strThang = iThang_Quy;
            }else if (bLoaiThang_Quy == 1){
                int ThangCuoi = Convert.ToInt16(iThang_Quy)*3;
                int ThangDau = ThangCuoi - 2;
                for (int thang = ThangDau; thang <= ThangCuoi; thang++)
                {
                    if (strThang == "") { strThang = thang.ToString();}
                    else{ strThang +="," + thang.ToString();}
                }
            }else if (bLoaiThang_Quy == 2){
                int ThangCuoi = 12;
                int ThangDau = 1;
                for (int thang = ThangDau; thang <= ThangCuoi; thang++)
                {
                    if (strThang == "") { strThang = thang.ToString();}
                    else { strThang += "," + thang.ToString(); }
                }
            }
            DataTable tempTbl;
            SqlCommand cmd = new SqlCommand();
            String countQuery = String.Format(@"SELECT COUNT(iID_MaNgachLuong_CanBo) AS c FROM L_BangLuongChiTiet AS tbl 
                              WHERE tbl.iNamBangLuong = tblL.iNamBangLuong AND tbl.iThangBangLuong = tblL.iThangBangLuong AND tbl.iID_MaDonVi = @iID_MaDonVi
                              AND tbl.iID_MaNgachLuong_CanBo = tblL.iID_MaNgachLuong_CanBo AND tbl.iTrangThai = 1");
            String query = String.Format(@"SELECT 
                              iNamBangLuong,iThangBangLuong,iID_MaNgachLuong_CanBo
                              ,COUNT(iID_MaNgachLuong_CanBo) AS SoMaNgachLuong
                              ,SUM(rBaoHiemChi_OmDau_BanThanOm) AS rBaoHiemChi_OmDau_BanThanOm
                              ,({0} AND tbl.rBaoHiemChi_OmDau_BanThanOm > 0)AS rBaoHiemChi_OmDau_BanThanOm_Count
                              ,SUM(rBaoHiemChi_OmDau_ConOm) AS rBaoHiemChi_OmDau_ConOm
                              ,({0} AND tbl.rBaoHiemChi_OmDau_ConOm > 0)AS rBaoHiemChi_OmDau_ConOm_Count
                              ,SUM(rBaoHiemChi_OmDau_DuongSuc) AS rBaoHiemChi_OmDau_DuongSuc
                              ,({0} AND tbl.rBaoHiemChi_OmDau_DuongSuc > 0)AS rBaoHiemChi_OmDau_DuongSuc_Count
                              ,SUM(rBaoHiemChi_ThaiSan_SinhCon) AS rBaoHiemChi_ThaiSan_SinhCon
                              ,({0} AND tbl.rBaoHiemChi_ThaiSan_SinhCon > 0)AS rBaoHiemChi_ThaiSan_SinhCon_Count
                              ,SUM(rBaoHiemChi_ThaiSan_KhamThai) AS rBaoHiemChi_ThaiSan_KhamThai
                              ,({0} AND tbl.rBaoHiemChi_ThaiSan_KhamThai > 0)AS rBaoHiemChi_ThaiSan_KhamThai_Count
                              ,SUM(rBaoHiemChi_ThaiSan_DuongSuc) AS rBaoHiemChi_ThaiSan_DuongSuc
                              ,({0} AND tbl.rBaoHiemChi_ThaiSan_DuongSuc > 0)AS rBaoHiemChi_ThaiSan_DuongSuc_Count
                              ,SUM(rBaoHiemChi_ThaiSan_1Lan) AS rBaoHiemChi_ThaiSan_1Lan
                              ,({0}1 AND tbl.rBaoHiemChi_ThaiSan_1Lan > 0)AS rBaoHiemChi_ThaiSan_1Lan_Count
                              ,SUM(rBaoHiemChi_TaiNan_1Lan) AS rBaoHiemChi_TaiNan_1Lan
                              ,({0} AND tbl.rBaoHiemChi_TaiNan_1Lan > 0)AS rBaoHiemChi_TaiNan_1Lan_Count
                              ,SUM(rBaoHiemChi_TaiNan_HangThang) AS rBaoHiemChi_TaiNan_HangThang
                              ,({0} AND tbl.rBaoHiemChi_TaiNan_HangThang > 0)AS rBaoHiemChi_TaiNan_HangThang_Count
                              ,SUM(rBaoHiemChi_TaiNan_PhucHoi) AS rBaoHiemChi_TaiNan_PhucHoi
                              ,({0} AND tbl.rBaoHiemChi_TaiNan_PhucHoi > 0)AS rBaoHiemChi_TaiNan_PhucHoi_Count
                              ,SUM(rBaoHiemChi_TaiNan_NguoiPhucVu) AS rBaoHiemChi_TaiNan_NguoiPhucVu
                              ,({0} AND tbl.rBaoHiemChi_TaiNan_NguoiPhucVu > 0)AS rBaoHiemChi_TaiNan_NguoiPhucVu_Count
                              ,SUM(rBaoHiemChi_TaiNan_Chet) AS rBaoHiemChi_TaiNan_Chet
                              ,({0} AND tbl.rBaoHiemChi_TaiNan_Chet > 0)AS rBaoHiemChi_TaiNan_Chet_Count
                              ,SUM(rBaoHiemChi_TaiNan_DuongSuc) AS rBaoHiemChi_TaiNan_DuongSuc
                              ,({0} AND tbl.rBaoHiemChi_TaiNan_DuongSuc > 0)AS rBaoHiemChi_TaiNan_DuongSuc_Count
                              ,SUM(rBaoHiemChi_HuuTri) AS rBaoHiemChi_HuuTri
                              ,({0} AND tbl.rBaoHiemChi_HuuTri > 0)AS rBaoHiemChi_HuuTri_Count
                              ,SUM(rBaoHiemChi_PhucVien) AS rBaoHiemChi_PhucVien
                              ,({0} AND tbl.rBaoHiemChi_PhucVien > 0)AS rBaoHiemChi_PhucVien_Count
                              ,SUM(rBaoHiemChi_XuatNgu) AS rBaoHiemChi_XuatNgu
                              ,({0} AND tbl.rBaoHiemChi_XuatNgu > 0)AS rBaoHiemChi_XuatNgu_Count
                              ,SUM(rBaoHiemChi_ThoiViec) AS rBaoHiemChi_ThoiViec
                              ,({0} AND tbl.rBaoHiemChi_ThoiViec > 0)AS rBaoHiemChi_ThoiViec_Count
                              ,SUM(rBaoHiemChi_TuTuat) AS rBaoHiemChi_TuTuat
                              ,({0} AND tbl.rBaoHiemChi_TuTuat > 0)AS rBaoHiemChi_TuTuat_Count
                          FROM L_BangLuongChiTiet AS tblL
                          WHERE iTrangThai = 1 AND iNamBangLuong = @iNamBangLuong AND iThangBangLuong IN (@iThangBangLuong) AND iID_MaDonVi = @iID_MaDonVi   
                          GROUP BY iNamBangLuong,iThangBangLuong,iID_MaNgachLuong_CanBo
                          ORDER BY iNamBangLuong,iThangBangLuong,iID_MaNgachLuong_CanBo",countQuery);
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangBangLuong", strThang);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            tempTbl = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return tempTbl;
        }
    }
}