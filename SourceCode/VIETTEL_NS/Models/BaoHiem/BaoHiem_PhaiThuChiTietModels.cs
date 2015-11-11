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
    public class BaoHiem_PhaiThuChiTietModels
    {
        public static String[] arrDSTruongTien = "rSoNguoi,rLuongCoBan,rThamNien,rPCChucVu,rOmDauNgan,rKhac,rTongSo".Split(',');
        public static void ThemChiTiet(String iID_MaBaoHiemPhaiThu, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            //String[] arrDSHangChiTiet = BaoHiemChiModels.strDSHangChiTiet.Split(',');

            DataTable dtChungTu = BaoHiem_PhaiThuModels.GetChungTu(iID_MaBaoHiemPhaiThu);

            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaDonVi = Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]);
            String sTenDonVi = Convert.ToString(dtChungTu.Rows[0]["sTenDonVi"]);
            String iThang_Quy = Convert.ToString(dtChungTu.Rows[0]["iThang_Quy"]);

            DataTable dtDV = DonViModels.Get_dtDonVi(iID_MaDonVi);

            String iID_MaLoaiDonVi = Convert.ToString(dtDV.Rows[0]["iID_MaLoaiDonVi"]);

            Bang bang = new Bang("BH_PhaiThuChungTuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang_Quy); ;

            DataTable dt = DanhMucThuBaoHiemModels.Get_DTDanhMucThuBaoHiem(iNamLamViec.ToString(), iThang_Quy);
            DataTable dtQuyetToan = Get_dtQuyetToan(iThang_Quy, iNamLamViec.ToString(), iID_MaDonVi);

            DataTable dtQTBaoHiem = Get_dtQuyetToanBaoHiem(iThang_Quy, iNamLamViec.ToString(), iID_MaDonVi);

            String sKyHieu = "", sKyHieu1 = "";
            Double rSoNguoi = 0, rLuongCoBan = 0, rThamNien = 0, rChucVu = 0, rKhac = 0, rOmDauNgan = 0;
            Double TongLuongCoBan = 0, TongSoNguoi = 0, TongThamNien = 0, TongChucVu = 0, TongKhac = 0, TongOmNgan = 0, TongHang = 0, TongHangCha = 0;


            String sXauNoiMa = "";
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                TongHang = 0;
                rSoNguoi = 0; rLuongCoBan = 0; rThamNien = 0; rChucVu = 0; rKhac = 0; rOmDauNgan = 0;
                sKyHieu = Convert.ToString(dt.Rows[i]["sKyHieu"]);

                for (int j = dtQuyetToan.Rows.Count - 1; j >= 0; j--)
                {
                    //03de0e56-e874-4320-a01b-f524beb47b71 Loại đơn vị dự toán
                    //25086e34-3c2e-4c6c-8c79-786d013e36b0 Loại đơn vị doanh nghiệp
                    sKyHieu1 = Convert.ToString(dtQuyetToan.Rows[j]["sKyHieu"]);
                    if (iID_MaLoaiDonVi == "25086e34-3c2e-4c6c-8c79-786d013e36b0")
                    {
                        sKyHieu1 = "2" + sKyHieu1.Substring(1, 2);
                    }
                    if (sKyHieu.Length > 2 && sKyHieu1.Substring(0, 2).Equals(sKyHieu.Substring(1, 2)))
                    {
                        switch (sKyHieu1.Substring(2, 1))
                        {
                            case "1":
                                rLuongCoBan = Convert.ToDouble(dtQuyetToan.Rows[j]["rTuChi"]);
                                rSoNguoi = Convert.ToDouble(dtQuyetToan.Rows[j]["rSoNguoi"]);
                                break;
                            case "2":
                                rChucVu = Convert.ToDouble(dtQuyetToan.Rows[j]["rTuChi"]);
                                break;
                            case "3":
                                rThamNien = Convert.ToDouble(dtQuyetToan.Rows[j]["rTuChi"]);
                                break;
                            case "5":
                                rKhac = Convert.ToDouble(dtQuyetToan.Rows[j]["rTuChi"]);
                                break;
                        }
                    }
                }

                for (int j = 0; j < dtQTBaoHiem.Rows.Count; j++)
                {
                    sKyHieu1 = Convert.ToString(dtQTBaoHiem.Rows[j]["sKyHieuDoiTuong"]);

                    if (sKyHieu1 != "77")
                    {
                        if (iID_MaLoaiDonVi == "25086e34-3c2e-4c6c-8c79-786d013e36b0")
                        {
                            sKyHieu1 = sKyHieu1.Replace("7", "12");
                        }
                        else
                        {
                            sKyHieu1 = sKyHieu1.Replace("7", "11");
                        }
                    }
                    else
                    {
                        if (iID_MaLoaiDonVi == "25086e34-3c2e-4c6c-8c79-786d013e36b0")
                        {
                            sKyHieu1 = "127";
                        }
                        else
                        {
                            sKyHieu1 = "117";
                        }


                    }
                    if (sKyHieu1 == sKyHieu)
                    {
                        rOmDauNgan = Convert.ToDouble(dtQTBaoHiem.Rows[j]["rTongSo"]);
                        break;
                    }

                }

                TongHang = rLuongCoBan + rThamNien + rChucVu + rOmDauNgan + rKhac;

                TongOmNgan = TongOmNgan + rOmDauNgan;
                TongLuongCoBan = TongLuongCoBan + rLuongCoBan;
                TongSoNguoi = TongSoNguoi + rSoNguoi;
                TongThamNien = TongThamNien + rThamNien;
                TongChucVu = TongChucVu + rChucVu;
                TongKhac = TongKhac + rKhac;
                TongHangCha = TongLuongCoBan + TongThamNien + TongChucVu + TongKhac + TongOmNgan;
                if (i == dt.Rows.Count - 1)
                {
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucDoiTuong", dt.Rows[i]["iID_MaMucLucDoiTuong"]);
                    bang.CmdParams.Parameters.AddWithValue("@sKyHieu", dt.Rows[i]["sKyHieu"]);
                    bang.CmdParams.Parameters.AddWithValue("@sMoTa", dt.Rows[i]["sMoTa"]);
                    bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", dt.Rows[i]["bLaHangCha"]);
                    bang.CmdParams.Parameters.AddWithValue("@rSoNguoi", rSoNguoi);
                    bang.CmdParams.Parameters.AddWithValue("@rLuongCoBan", rLuongCoBan);
                    bang.CmdParams.Parameters.AddWithValue("@rThamNien", rThamNien);
                    bang.CmdParams.Parameters.AddWithValue("@rPCChucVu", rChucVu);
                    bang.CmdParams.Parameters.AddWithValue("@rOmDauNgan", rOmDauNgan);
                    bang.CmdParams.Parameters.AddWithValue("@rKhac", rKhac);
                    bang.CmdParams.Parameters.AddWithValue("@rTongSo", TongHang);
                }
                else
                {
                    bang.CmdParams.Parameters["@iID_MaMucLucDoiTuong"].Value = dt.Rows[i]["iID_MaMucLucDoiTuong"];
                    bang.CmdParams.Parameters["@sKyHieu"].Value = dt.Rows[i]["sKyHieu"];
                    bang.CmdParams.Parameters["@sMoTa"].Value = dt.Rows[i]["sMoTa"];
                    bang.CmdParams.Parameters["@bLaHangCha"].Value = dt.Rows[i]["bLaHangCha"];
                    bang.CmdParams.Parameters["@rSoNguoi"].Value = rSoNguoi;
                    bang.CmdParams.Parameters["@rLuongCoBan"].Value = rLuongCoBan;
                    bang.CmdParams.Parameters["@rThamNien"].Value = rThamNien;
                    bang.CmdParams.Parameters["@rPCChucVu"].Value = rChucVu;
                    bang.CmdParams.Parameters["@rKhac"].Value = rKhac;
                    bang.CmdParams.Parameters["@rOmDauNgan"].Value = rOmDauNgan;
                    bang.CmdParams.Parameters["@rTongSo"].Value = TongHang;

                }
                //loai don vi doanh nghiep
                if (iID_MaLoaiDonVi == "25086e34-3c2e-4c6c-8c79-786d013e36b0")
                {
                    if (i == 9 || i < 1)
                    {
                        bang.CmdParams.Parameters["@rSoNguoi"].Value = TongSoNguoi;
                        bang.CmdParams.Parameters["@rLuongCoBan"].Value = TongLuongCoBan;
                        bang.CmdParams.Parameters["@rThamNien"].Value = TongThamNien;
                        bang.CmdParams.Parameters["@rPCChucVu"].Value = TongChucVu;
                        bang.CmdParams.Parameters["@rKhac"].Value = TongKhac;
                        bang.CmdParams.Parameters["@rOmDauNgan"].Value = TongOmNgan;
                        bang.CmdParams.Parameters["@rTongSo"].Value = TongHangCha;
                    }
                }
                else
                {
                    if (i <= 1)
                    {
                        bang.CmdParams.Parameters["@rSoNguoi"].Value = TongSoNguoi;
                        bang.CmdParams.Parameters["@rLuongCoBan"].Value = TongLuongCoBan;
                        bang.CmdParams.Parameters["@rThamNien"].Value = TongThamNien;
                        bang.CmdParams.Parameters["@rPCChucVu"].Value = TongChucVu;
                        bang.CmdParams.Parameters["@rKhac"].Value = TongKhac;
                        bang.CmdParams.Parameters["@rOmDauNgan"].Value = TongOmNgan;
                        bang.CmdParams.Parameters["@rTongSo"].Value = TongHangCha;
                    }
                }
                if (dt.Rows[i]["iID_MaMucLucDoiTuong_Cha"] != DBNull.Value)
                {
                    if (bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucDoiTuong_Cha") >= 0)
                    {
                        bang.CmdParams.Parameters["@iID_MaMucLucDoiTuong_Cha"].Value = dt.Rows[i]["iID_MaMucLucDoiTuong_Cha"];
                    }
                    else
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucDoiTuong_Cha", dt.Rows[i]["iID_MaMucLucDoiTuong_Cha"]);
                    }
                }
                bang.Save();
            }

            dt.Dispose();
            dtChungTu.Dispose();
        }

        public static String InsertDuyetBaoHiem(String iID_MaBaoHiemPhaiThu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("BH_DuyetChungTuThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static void UpdateBangChiTiet(String UserName, String Address, String iID_MaBaoHiemPhaiThu, String iID_MaDonVi, String sTenDonVi, String iThang_Quy, String bLoaiThang_Quy)
        {
            Bang bang = new Bang("BH_PhaiThuChungTuChiTiet");
            bang.MaNguoiDungSua = UserName;
            bang.IPSua = Address;
            bang.DuLieuMoi = false;
            String SQL = "UPDATE BH_PhaiThuChungTuChiTiet SET iID_MaDonVi=@iID_MaDonVi,sTenDonVi=@sTenDonVi";
            SQL += ",iThang_Quy=@iThang_Quy,bLoaiThang_Quy=@bLoaiThang_Quy ";
            SQL += ",sID_MaNguoiDungSua=@sID_MaNguoiDungSua,sIPSua=@sIPSua ";
            SQL += " WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@bLoaiThang_Quy", bLoaiThang_Quy);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", UserName);
            cmd.Parameters.AddWithValue("@sIPSua", Address);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaBaoHiemPhaiThu)
        {
            int vR = -1;
            DataTable dt = BaoHiem_PhaiThuModels.GetChungTu(iID_MaBaoHiemPhaiThu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM BH_PhaiThuChungTuChiTiet WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaBaoHiemPhaiThu)
        {
            int vR = -1;
            DataTable dt = BaoHiem_PhaiThuModels.GetChungTu(iID_MaBaoHiemPhaiThu);
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

        public static DataTable Get_dtBaoHiemChiChiTiet(String iID_MaBaoHiemPhaiThu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu";
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);

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

            SQL = String.Format("SELECT * FROM BH_PhaiThuChungTuChiTiet WHERE {0} ORDER BY  sKyHieu", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_dtQuyetToan(String iThang_Quy, String iNamLamViec, String iID_MaDonVi)
        {
            //String SQL1 = "SELECT SUBSTRING(sKyHieu,2,2) FROM NS_MucLucDoiTuong WHERE bLaHangCha=0 ORDER BY sKyHieu";
            //String[] arr_MaCot = "1,2,3,4,5".Split(',');
            //String SQLCH = "SELECT iID_MaMucLucNganSach FROM BH_CauHinh_DoiTuong_NganSach";
            //String SQL = "SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa";
            //SQL += ",ISNULL(SUM(rSoNguoi),0) AS rSoNguoi,ISNULL(SUM(rTuChi),0) AS rTuChi";
            //SQL += " FROM QTA_ChungTuChiTiet WHERE sLNS='1010000' AND sL='460' AND sK='468'";
            //SQL += " AND sNG='00' AND sTNG='00'";
            //SQL += " AND iID_MaDonVi=@iID_MaDonVi AND iThang_Quy=@iThang_Quy AND iNamLamViec=@iNamLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            //SQL += " AND ((sM='6000' AND sTM='6001' AND (sTTM='10' OR sTTM='20' OR sTTM='30' ))";
            //SQL += " OR(sM='6100' AND (sTM='6101' OR sTM='6115') AND (sTTM='10' OR sTTM='20')))";
            //SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa";
            //SQL += " ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa";
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = SQL;
            //cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            //cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            //cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            //DataTable dt = Connection.GetDataTable(cmd);
            //cmd.Dispose();
            String SQL = String.Format(@"SELECT sKyHieu,ISNULL(SUM(rSoNguoi),0) AS rSoNguoi,ISNULL(SUM(rTuChi),0) AS rTuChi
            FROM
                ((SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa
            ,ISNULL(SUM(rSoNguoi),0) AS rSoNguoi
            ,ISNULL(SUM(rTuChi),0) AS rTuChi
            FROM QTA_ChungTuChiTiet
            WHERE iID_MaMucLucNganSach IN (SELECT iID_MaMucLucNganSach FROM BH_CauHinh_DoiTuong_NganSach 
            WHERE sKyHieu IS NOT  NULL) AND iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND iThang_Quy=@iThang_Quy AND iNamLamViec=@iNamLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa
            ) QT
            INNER JOIN 
            (SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa,sKyHieu FROM BH_CauHinh_DoiTuong_NganSach 
            WHERE sKyHieu IS NOT  NULL AND iTrangThai=1 AND sKyHieu <> '' AND iNamLamViec=@iNamLamViec) BH
            ON QT.sXauNoiMa=BH.sXauNoiMa)
            GROUP BY sKyHieu");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtDonViQuyetToan(int iThang_Quy, String iNamLamViec)
        {
            String SQL = @"SELECT Distinct iID_MaDonVi FROM QTA_ChungTuChiTiet 
                          WHERE iTrangThai=1 AND sLNS='1010000' AND iThang_Quy=@iThang_Quy AND iNamLamViec=@iNamLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ORDER BY iID_MaDonVi";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtQuyetToanBaoHiem(String iThang_Quy, String iNamLamViec, String iID_MaDonVi)
        {
            String SQL = @"SELECT sKyHieuDoiTuong,SUM(rTongSo) AS rTongSo FROM QTA_QuyetToanBaoHiem
                            WHERE SUBSTRING(sKyHieuDoiTuong,1,1)='7' AND iID_MaDonVi=@iID_MaDonVi AND iThang_Quy=@iThang_Quy
                            GROUP BY sKyHieuDoiTuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}