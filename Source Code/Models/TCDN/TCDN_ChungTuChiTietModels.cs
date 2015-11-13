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
    public class TCDN_ChungTuChiTietModels
    {
        public static void ThemChiTiet(String iID_MaChungTu, String iLoai, String MaND, String IPSua)
        {
            DataTable dtChungTu = TCDN_ChungTuModels.GetChungTu(iID_MaChungTu);

            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaDoanhNghiep = Convert.ToString(dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);
            int iQuy = Convert.ToInt32(dtChungTu.Rows[0]["iQuy"]);
            String[] arrLoai = iLoai.Split(';');
            int Nam = iNamLamViec;
            int Quy = iQuy;
            if (iQuy == 1)
            {
                Nam = iNamLamViec - 1;
                Quy = 4;
            }
            else
            {
                Quy=iQuy-1;
            }
            DataTable dtDauKy = Get_dtQuyNam(Nam, Quy, iID_MaDoanhNghiep);

            for (int j = 0; j < arrLoai.Length; j++)
            {
                if (arrLoai[j] != "")
                {
                    DataTable dt = TCDN_ChiTieuModels.DT_MucLucChiTieu(arrLoai[j]);

                    Bang bang = new Bang("TCDN_ChungTuChiTiet");
                    bang.GiaTriKhoa = null;
                    bang.DuLieuMoi = true;
                    bang.MaNguoiDungSua = MaND;
                    bang.IPSua = IPSua;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iQuy", dtChungTu.Rows[0]["iQuy"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //Thêm thông tin vào bảng TCDN_ChungTuChiTiet
                        ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(dt.Rows[i], dtDauKy, bang.CmdParams.Parameters);
                        bang.Save();
                    }

                    dt.Dispose();
                }
            }
            dtChungTu.Dispose();

            //Đánh STT cho bảng TCDN_ChungTuChiTiet vừa insert
            DataTable dtChiTieu = TCDN_ChiTieuModels.Get_MucLucChiTieu();
            DataTable dtChungTuChiTiet = Get_dtChungTuChiTiet_TheoChungTu(iID_MaChungTu);
            int STT = 0;
            DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, iID_MaChungTu, 0, ref STT);
            dtChiTieu.Dispose();
            dtChungTuChiTiet.Dispose();
        }

        public static void ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(DataRow RMucLucChiTieu, DataTable dtDauKy, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "sKyHieu,sTen,sThuyetMinh";
            String sXauNoiMa = "";
            String[] arrDSTruong = strDSTruong.Split(',');
            if (Params.IndexOf("@iID_MaChiTieu") >= 0)
            {
                Params["@iID_MaChiTieu"].Value = RMucLucChiTieu["iID_MaChiTieu"];
                Params["@iID_MaChiTieu_Cha"].Value = RMucLucChiTieu["iID_MaChiTieu_Cha"];
                Params["@bLaHangCha"].Value = RMucLucChiTieu["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaChiTieu", RMucLucChiTieu["iID_MaChiTieu"]);
                Params.AddWithValue("@iID_MaChiTieu_Cha", RMucLucChiTieu["iID_MaChiTieu_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucChiTieu["bLaHangCha"]);
            }
            String iID_MaChiTieu = Convert.ToString(RMucLucChiTieu["iID_MaChiTieu"]);
            String iID_MaChiTieu1 = "";
            for (int i = 0; i < dtDauKy.Rows.Count; i++)
            {
                iID_MaChiTieu1 = Convert.ToString(dtDauKy.Rows[i]["iID_MaChiTieu"]);
                if (iID_MaChiTieu.Equals(iID_MaChiTieu1))
                {
                    if (Params.IndexOf("@rSoDauNam") >= 0)
                    {
                        Params["@rSoDauNam"].Value = dtDauKy.Rows[i]["rSoCuoiNam"];
                    }
                    else
                    {
                        Params.AddWithValue("@rSoDauNam", dtDauKy.Rows[i]["rSoCuoiNam"]);
                    }
                    dtDauKy.Rows.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucChiTieu[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucChiTieu[arrDSTruong[i]]);
                }
                if (i < arrDSTruong.Length - 1 && String.IsNullOrEmpty(Convert.ToString(RMucLucChiTieu[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucChiTieu[arrDSTruong[i]]);
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

        public static String InsertDuyetQuyetToan(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("TN_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        public static DataTable Get_dtQuyNam(int iNamLamViec, int iQuy, String iID_MaDoanhNghiep)
        {
            String SQL = "SELECT * FROM TCDN_ChungTuChiTiet WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = TCDN_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(TCDNModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM TCDN_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
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
            DataTable dt = TCDN_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(TCDNModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtChungTuChiTiet(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String iLoai)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            //lấy dt chưng từ
            DataTable dtChungTu = TCDN_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iQuy = Convert.ToInt32(dtChungTu.Rows[0]["iQuy"]);
            String iID_MaDoanhNghiep = Convert.ToString(dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);
            //lấy dt danh mục
            DK = "iTrangThai=1  AND SUBSTRING(sKyHieu,1,1)=@iLoai";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            SQL = String.Format("SELECT * FROM TCDN_ChiTieu WHERE {0} ORDER BY sKyHieu",DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);

            //lấy dt chi tiết

            cmd= new SqlCommand();
            DK = "iTrangThai=1  AND  SUBSTRING(sKyHieu,1,1)=@iLoai AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            SQL = String.Format(@"
SELECT sKyHieu,iID_MaChungTuChiTiet,rThucHien 
FROM TCDN_ChungTuChiTiet
WHERE {0}  AND iNamLamViec=@iNamLamViec  AND iID_MaChungTu=@iID_MaChungTu ORDER BY sKyHieu
", DK);
            cmd.CommandText = SQL;
            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);

            //Lấy dtNamTruoc
            cmd = new SqlCommand();
            DK = "iTrangThai=1  AND  SUBSTRING(sKyHieu,1,1)=@iLoai AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec-1);
            SQL = String.Format(@"
SELECT sKyHieu,SUM(rThucHien) as rNamTruoc 
FROM TCDN_ChungTuChiTiet
WHERE {0}   AND iNamLamViec=@iNamLamViec  GROUP BY sKyHieu  HAVING SUM(rThucHien)<>0 ORDER BY sKyHieu
", DK);
            cmd.CommandText = SQL;
            DataTable dtNamTruoc = Connection.GetDataTable(cmd);

            //Lấy dtDaThucHien
            cmd = new SqlCommand();
            DK = "iTrangThai=1  AND  SUBSTRING(sKyHieu,1,1)=@iLoai AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep ";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            SQL = String.Format(@"
SELECT sKyHieu,
rDaThucHien=SUM(CASE WHEN iquy<@iQuy THEN rThucHien ELSE 0 END),
rKeHoach =SUM(CASE WHEN iquy=1 THEN rKeHoach ELSE 0 END)
FROM TCDN_ChungTuChiTiet
WHERE {0}   AND iNamLamViec=@iNamLamViec  GROUP BY sKyHieu    ORDER BY sKyHieu
", DK);

            cmd.CommandText = SQL;
            DataTable dtDaThucHien = Connection.GetDataTable(cmd);


            //Ghep dtNamTruoc vao dt Nam Nay và tinh ty le
            String sDSTruong = "sKyHieu,iID_MaChungTuChiTiet,rThucHien,rKeHoach,rNamTruoc,rDaThucHien,rNTNN,rTHKH";
            String[] arrDSTruong = sDSTruong.Split(',');
            for (int i = 2; i < arrDSTruong.Length; i++)
            {
                DataColumn dtc= new DataColumn();
                dtc.DefaultValue = 0;
                dtc.DataType = typeof(decimal);
                dtc.ColumnName = arrDSTruong[i];
                vR.Columns.Add(dtc);
            }
            vR.Columns.Add("iID_MaChungTuChiTiet", typeof (String));
            int vRCount = vR.Rows.Count;
            int cs0 = 0;
            for (int i = 0; i < vRCount; i++)
            {
                //Ghép dt chi tiết vào dt danh muc
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {
                    Boolean ok = true;
                    if (Convert.ToString(vR.Rows[i]["sKyHieu"]) != Convert.ToString(dtChungTuChiTiet.Rows[j]["sKyHieu"]))
                        ok = false;
                    if (Convert.ToBoolean(vR.Rows[i]["bLaTong"])==true)
                        ok = false;
                    if (ok)
                    {
                        // bo 4 truong cuoi
                        for (int k = 1; k < arrDSTruong.Length-5; k++)
                            {
                                vR.Rows[i][arrDSTruong[k]] = dtChungTuChiTiet.Rows[j][arrDSTruong[k]];
                            }
                    }
                }
                //Ghép dtnamtruoc vào dt danh muc
                for (int c= 0; c < dtNamTruoc.Rows.Count; c++)
                {
                    Boolean ok = true;
                    if (Convert.ToString(vR.Rows[i]["sKyHieu"]) != Convert.ToString(dtNamTruoc.Rows[c]["sKyHieu"]))
                        ok = false;
                    if (Convert.ToBoolean(vR.Rows[i]["bLaTong"]) == true)
                        ok = false;
                    if (ok)
                    {
                        vR.Rows[i]["rNamTruoc"] = dtNamTruoc.Rows[c]["rNamTruoc"];
                    }
                }
                //ghep dtdaThucHien vao dt danh muc
                for (int c = 0; c < dtDaThucHien.Rows.Count; c++)
                {
                    Boolean ok = true;
                    if (Convert.ToString(vR.Rows[i]["sKyHieu"]) != Convert.ToString(dtDaThucHien.Rows[c]["sKyHieu"]))
                        ok = false;
                    if (Convert.ToBoolean(vR.Rows[i]["bLaTong"]) == true)
                        ok = false;
                    if (ok)
                    {
                        vR.Rows[i]["rDaThucHien"] = dtDaThucHien.Rows[c]["rDaThucHien"];
                        vR.Rows[i]["rKeHoach"] = dtDaThucHien.Rows[c]["rKeHoach"];
                    }
                }
                //Tinh cot ty le
                if (Convert.ToDecimal(vR.Rows[i]["rNamTruoc"]) != 0)
                    vR.Rows[i]["rNTNN"] =  (Math.Round((Convert.ToDecimal(vR.Rows[i]["rThucHien"])+Convert.ToDecimal(vR.Rows[i]["rDaThucHien"])) /
                                          Convert.ToDecimal(vR.Rows[i]["rNamTruoc"])*100,0));
                if (Convert.ToDecimal(vR.Rows[i]["rKeHoach"]) != 0)
                    vR.Rows[i]["rTHKH"] = (Math.Round((Convert.ToDecimal(vR.Rows[i]["rThucHien"]) + Convert.ToDecimal(vR.Rows[i]["rDaThucHien"])) /
                                          Convert.ToDecimal(vR.Rows[i]["rKeHoach"]) * 100, 0));
            }
            

            // ghép phần dự án đang đâu tư nếu bảng là bảng 3
            if(iLoai=="3")
            {
                SQL =
                    String.Format(
                        "SELECT * FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
                cmd= new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                DataTable dtDuAnDauTu = Connection.GetDataTable(cmd);
                cmd.Dispose();

                //Lay cac du an co du lieu nam nay

                SQL =
                   String.Format(
                       @"SELECT  sKyHieu,iID_MaChungTuChiTiet,rThucHien 
                         FROM TCDN_ChungTuChiTiet
                        WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu  AND iNamLamViec=@iNamLamViec AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep) ");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                DataTable dtDuAn_NamNay = Connection.GetDataTable(cmd);
                cmd.Dispose();

                //Lay cac du an co du lieu nam truoc

                SQL =
                   String.Format(
                       "SELECT sKyHieu,SUM(rThucHien) as rNamTruoc  FROM TCDN_ChungTuChiTiet WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep) " +
                       "GROUP BY sKyHieu");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec-1);
                DataTable dtDuAn_NamTruoc = Connection.GetDataTable(cmd);
                cmd.Dispose();

                //Lấy dtDaThucHien
                cmd = new SqlCommand();
                DK = "iTrangThai=1  AND  SUBSTRING(sKyHieu,1,1)=@iLoai AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep ";
                cmd.Parameters.AddWithValue("@iLoai", iLoai);
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                SQL = String.Format(@"
SELECT sKyHieu,
rDaThucHien=SUM(CASE WHEN iquy<@iQuy THEN rThucHien ELSE 0 END),
rKeHoach =SUM(CASE WHEN iquy=1 THEN rKeHoach ELSE 0 END)
FROM TCDN_ChungTuChiTiet
WHERE {0}   AND iNamLamViec=@iNamLamViec AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND bHoanThanh=0 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep)   GROUP BY sKyHieu    ORDER BY sKyHieu
", DK);

                cmd.CommandText = SQL;
                DataTable dtDuAn_DaThucHien = Connection.GetDataTable(cmd);

                for (int i = 2; i < arrDSTruong.Length; i++)
                {
                    DataColumn dtc = new DataColumn();
                    dtc.DefaultValue = 0;
                    dtc.DataType = typeof(decimal);
                    dtc.ColumnName = arrDSTruong[i];
                    dtDuAnDauTu.Columns.Add(dtc);
                }
                dtDuAnDauTu.Columns.Add("iID_MaChungTuChiTiet", typeof(String));
                for (int i = 0; i < dtDuAnDauTu.Rows.Count; i++)
                {
                    //Ghép dtDuAn_NamNay vào dtDuAnDauTu
                    for (int j = cs0; j < dtDuAn_NamNay.Rows.Count; j++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(dtDuAnDauTu.Rows[i]["sKyHieu"]) != Convert.ToString(dtDuAn_NamNay.Rows[j]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {
                            for (int k = 1; k < arrDSTruong.Length - 5; k++)
                            {
                                dtDuAnDauTu.Rows[i][arrDSTruong[k]] = dtDuAn_NamNay.Rows[j][arrDSTruong[k]];
                            }
                        }
                    }
                    //Ghép dtnamtruoc vào dt danh muc
                    for (int c = 0; c < dtDuAn_NamTruoc.Rows.Count; c++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(dtDuAnDauTu.Rows[i]["sKyHieu"]) != Convert.ToString(dtDuAn_NamTruoc.Rows[c]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {
                            dtDuAnDauTu.Rows[i]["rNamTruoc"] = dtDuAn_NamTruoc.Rows[c]["rNamTruoc"];
                        }
                    }
                    //ghep dtdaThucHien vao dt danh muc
                    for (int c = 0; c < dtDuAn_DaThucHien.Rows.Count; c++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(dtDuAnDauTu.Rows[i]["sKyHieu"]) != Convert.ToString(dtDuAn_DaThucHien.Rows[c]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {
                            dtDuAnDauTu.Rows[i]["rDaThucHien"] = dtDuAn_DaThucHien.Rows[c]["rDaThucHien"];
                            dtDuAnDauTu.Rows[i]["rKeHoach"] = dtDuAn_DaThucHien.Rows[c]["rKeHoach"];
                        }
                    }
                    //Tinh cot ty le
                    if (Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rNamTruoc"]) != 0)
                        dtDuAnDauTu.Rows[i]["rNTNN"] = Math.Round((Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rDaThucHien"]) + Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rThucHien"])) /
                                              Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rNamTruoc"]) * 100, 0);
                    if (Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rKeHoach"]) != 0)
                        dtDuAnDauTu.Rows[i]["rTHKH"] = Math.Round((Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rDaThucHien"]) + Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rThucHien"])) /
                                              Convert.ToDecimal(dtDuAnDauTu.Rows[i]["rKeHoach"]) * 100, 0);
                }
                //ghep cac du an dang dau tu vao VR
                for (int i = 0; i < dtDuAnDauTu.Rows.Count; i++)
                {
                    DataRow dr = vR.NewRow();

                    dr["sKyHieu"] = dtDuAnDauTu.Rows[i]["sKyHieu"];
                    dr["sTen"] = dtDuAnDauTu.Rows[i]["sTenDuAn"];
                    dr["iID_MaChiTieu_Cha"] = dtDuAnDauTu.Rows[i]["iID_MaChiTieu_Cha"];
                    dr["bLahangCha"] = "False";
                    dr["bLaTong"] = "False";
                    dr["bLaText"] = "False";
                    for (int j = 1; j < arrDSTruong.Length; j++)
                    {
                        dr[arrDSTruong[j]] = dtDuAnDauTu.Rows[i][arrDSTruong[j]];
                    }
                    vR.Rows.Add(dr);
                }
                //sap xep lai dr

                DataView dv = vR.DefaultView;
                dv.Sort = "sKyHieu";
                vR = dv.ToTable();
            }
             //neu bang 4
            else if(iLoai=="4")
            {
                 DataColumn dtc1 = new DataColumn();
                dtc1.DefaultValue = "";
                dtc1.DataType = typeof(String);
                dtc1.ColumnName = "sNamTruoc_4";
                vR.Columns.Add(dtc1);

                dtc1 = new DataColumn();
                dtc1.DefaultValue = "";
                dtc1.DataType = typeof(String);
                dtc1.ColumnName = "sThucHien_4";
                vR.Columns.Add(dtc1);
                String sCongThuc = "";
                String [] arrCongThuc= new String[2];
                
                for (int i = 0; i < vR.Rows.Count; i++)
                {
                    sCongThuc = "";
                    List<string> arrCongThucSo = new List<String>();
                    List<string> arrCongThucDau = new List<String>();
                    List<decimal> arrGiaTriNamTruoc = new List<decimal>();
                    List<decimal> arrGiaTriNamNay = new List<decimal>();
                    if(!String.IsNullOrEmpty(Convert.ToString(vR.Rows[i]["sCongThuc"])))
                    {
                        sCongThuc = Convert.ToString(vR.Rows[i]["sCongThuc"]);
                    }
                    if(!string.IsNullOrEmpty(sCongThuc))
                    {
                        arrCongThuc = sCongThuc.Split(' ');
                        for (int z = 0; z < arrCongThuc.Length; z++)
                        {
                            if (arrCongThuc[z] == "+" || arrCongThuc[z] == "-" || arrCongThuc[z] == "*" || arrCongThuc[z] == "/")
                            {
                                arrCongThucDau.Add(arrCongThuc[z]);
                            }
                            else
                            {
                                arrCongThucSo.Add(arrCongThuc[z]);
                            }
                        }
                        //namnay
                        SQL = String.Format(@"SELECT SUM(rThucHien) as rThucHien 
FROM TCDN_ChungTuChiTiet
 WHERE iTrangThai=1  AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy<=iQuy
AND (sKyHieu IN (
SELECT sKyHieu
 FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND
   iID_MaChiTieu_Cha=(SELECT iID_MaChiTieu FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND sKyHieu=@sKyHieu AND bLaTong=1)
   ) OR sKyHieu =@sKyHieu) ");
                        //duyet lan luot cac so
                        for (int j = 0; j < arrCongThucSo.Count; j++)
                        {
                            cmd = new SqlCommand(SQL);
                            cmd.Parameters.AddWithValue("@sKyHieu", arrCongThucSo[j]);
                            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                            cmd.Parameters.AddWithValue("@iQuy", iQuy);
                            decimal dtNamNaySo1 = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                            arrGiaTriNamNay.Add(dtNamNaySo1);
                        }
                        //nam truoc
                        
                        SQL = String.Format(@"SELECT SUM(rThucHien) as rThucHien 
FROM TCDN_ChungTuChiTiet
 WHERE iTrangThai=1  AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec 
AND (sKyHieu IN (
SELECT sKyHieu
 FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND
   iID_MaChiTieu_Cha=(SELECT iID_MaChiTieu FROM TCDN_ChiTieu
  WHERE iTrangThai=1 AND sKyHieu=@sKyHieu AND bLaTong=1)
   ) OR sKyHieu =@sKyHieu)");
                        //duyet lan luot cac so
                        for (int j = 0; j < arrCongThucSo.Count; j++)
                        {
                            cmd = new SqlCommand(SQL);
                            cmd.Parameters.AddWithValue("@sKyHieu", arrCongThucSo[j]);
                            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec - 1);
                            decimal dtNamTruocSo1 = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                            arrGiaTriNamTruoc.Add(dtNamTruocSo1);
                        }
                        decimal rNamNay=0,rNamTruoc=0;
                        rNamNay=arrGiaTriNamNay[0];
                        rNamTruoc = arrGiaTriNamTruoc[0];
                        for (int c = 1; c < arrCongThucSo.Count; c++)
                        {
                            if (arrGiaTriNamNay[c] != 0)
                            {
                                if (arrCongThucDau[c - 1] == "+")
                                {
                                    rNamNay += arrGiaTriNamNay[c];
                                }
                                else if (arrCongThucDau[c - 1] == "-")
                                {
                                    rNamNay -= arrGiaTriNamNay[c];
                                }
                                else if (arrCongThucDau[c - 1] == "*")
                                {
                                    rNamNay *= arrGiaTriNamNay[c];
                                }
                                else
                                {
                                    rNamNay /= arrGiaTriNamNay[c];
                                }
                            }
                            if (arrGiaTriNamTruoc[c] != 0)
                            {
                                if (arrCongThucDau[c - 1] == "+")
                                {
                                    rNamTruoc += arrGiaTriNamTruoc[c];
                                }
                                else if (arrCongThucDau[c - 1] == "-")
                                {
                                    rNamTruoc -= arrGiaTriNamTruoc[c];
                                }
                                else if (arrCongThucDau[c - 1] == "*")
                                {
                                    rNamTruoc *= arrGiaTriNamTruoc[c];
                                }
                                else
                                {
                                    rNamTruoc /= arrGiaTriNamTruoc[c];
                                }
                            }

                        }
                        if (rNamNay == 0)
                            vR.Rows[i]["sThucHien_4"] = "";
                        else
                        vR.Rows[i]["sThucHien_4"] = Math.Round(rNamNay*100,2);
                        if (rNamTruoc == 0)
                            vR.Rows[i]["sNamTruoc_4"] = "";
                        else
                            vR.Rows[i]["sNamTruoc_4"] = Math.Round(rNamTruoc * 100, 2);
                        //if(dtNamNaySo2!=0)
                        //{
                        //    vR.Rows[i]["rThucHien"] = Math.Round(dtNamNaySo1/dtNamNaySo2*100,0);
                        //}
                        //if (dtNamTruocSo2 != 0)
                        //{
                        //    vR.Rows[i]["rNamTruoc"] = Math.Round(dtNamTruocSo1 / dtNamTruocSo2*100,0);
                        //}

                    }
                }// end for

                //ghep phan cac cty thanh vien
                SQL =
                   String.Format(
                       "SELECT * FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND (iID_MaDoanhNghiep=@iID_MaDoanhNghiep OR sKyHieu IN (414,4141,4142)) ORDER BY iLoai,sKyHieu");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                DataTable dtDonViThanhVien = Connection.GetDataTable(cmd);
                cmd.Dispose();

                //Lay cac du an co du lieu nam nay

                SQL =
                   String.Format(
                       @"SELECT  sKyHieu,iID_MaChungTuChiTiet,sThucHien_4 
                         FROM TCDN_ChungTuChiTiet
                        WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu  AND iNamLamViec=@iNamLamViec AND (sKyHieu IN (SELECT sKyHieu FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep) OR sKyHieu IN (4141,4142)) ");
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                DataTable dtDonViThanhVien_NamNay = Connection.GetDataTable(cmd);
                cmd.Dispose();

                //Lay cac du an co du lieu nam truoc

                SQL =
                   String.Format(
                       "SELECT sKyHieu,sThucHien_4 as sNamTruoc_4  FROM TCDN_ChungTuChiTiet WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec  AND iQuy=4 AND sKyHieu IN (SELECT sKyHieu FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND (iID_MaDoanhNghiep=@iID_MaDoanhNghiep) OR sKyHieu IN (4141,4142) )");
                       
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec - 1);
                DataTable dtDonViThanhVien_NamTruoc = Connection.GetDataTable(cmd);
                cmd.Dispose();
                for (int i = 2; i < arrDSTruong.Length; i++)
                {
                    DataColumn dtc = new DataColumn();
                    dtc.DefaultValue = 0;
                    dtc.DataType = typeof(decimal);
                    dtc.ColumnName = arrDSTruong[i];
                    dtDonViThanhVien.Columns.Add(dtc);
                }
                dtDonViThanhVien.Columns.Add("iID_MaChungTuChiTiet", typeof(String));
                dtDonViThanhVien.Columns.Add("sThucHien_4", typeof(String));
                dtDonViThanhVien.Columns.Add("sNamTruoc_4", typeof(String));
                for (int i = 0; i < dtDonViThanhVien.Rows.Count; i++)
                {
                    //Ghép dt_NamNay vào dtdonvithanhvien
                    for (int j = cs0; j < dtDonViThanhVien_NamNay.Rows.Count; j++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(dtDonViThanhVien.Rows[i]["sKyHieu"]) != Convert.ToString(dtDonViThanhVien_NamNay.Rows[j]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {

                            dtDonViThanhVien.Rows[i]["sThucHien_4"] = dtDonViThanhVien_NamNay.Rows[j]["sThucHien_4"];
                            dtDonViThanhVien.Rows[i]["iID_MaChungTuChiTiet"] = dtDonViThanhVien_NamNay.Rows[j]["iID_MaChungTuChiTiet"];
                            dtDonViThanhVien.Rows[i]["sKyHieu"] = dtDonViThanhVien_NamNay.Rows[j]["sKyHieu"];
                            break;
                        }
                    }
                    //Ghép dtDonVi_namTruoc vào dt danh muc
                    for (int c = 0; c < dtDonViThanhVien_NamTruoc.Rows.Count; c++)
                    {
                        Boolean ok = true;
                        if (Convert.ToString(dtDonViThanhVien.Rows[i]["sKyHieu"]) != Convert.ToString(dtDonViThanhVien_NamTruoc.Rows[c]["sKyHieu"]))
                            ok = false;
                        if (ok)
                        {
                            dtDonViThanhVien.Rows[i]["sNamTruoc_4"] = dtDonViThanhVien_NamTruoc.Rows[c]["sNamTruoc_4"];
                            break;
                        }
                    }
                }
                //ghep cac don vi thanh vien vao VR
                for (int i = 0; i < dtDonViThanhVien.Rows.Count; i++)
                {
                    DataRow dr = vR.NewRow();
                    //3 dong dau lay gia tri ghep vao vr
                    if (i == 0 || i == 1 )
                    {
                        for (int j = vR.Rows.Count-1; j >= 0; j--)
                        {

                            if (Convert.ToString(vR.Rows[j]["sKyHieu"]) == Convert.ToString(dtDonViThanhVien.Rows[i]["sKyHieu"]))
                            {
                                vR.Rows[j]["sThucHien_4"] = dtDonViThanhVien.Rows[i]["sThucHien_4"];
                                vR.Rows[j]["sNamTruoc_4"] = dtDonViThanhVien.Rows[i]["sNamTruoc_4"];
                                break;
                            }
                        }
                    }
                    else
                    {
                        dr["sKyHieu"] = dtDonViThanhVien.Rows[i]["sKyHieu"];
                        dr["sTen"] = dtDonViThanhVien.Rows[i]["sTen"];
                        dr["iID_MaChiTieu_Cha"] = dtDonViThanhVien.Rows[i]["iID_MaChiTieu_Cha"];
                        dr["bLahangCha"] = "False";
                        dr["bLaTong"] = "False";
                        dr["bLaText"] = "False";
                        for (int j = 1; j < arrDSTruong.Length; j++)
                        {
                            dr[arrDSTruong[j]] = dtDonViThanhVien.Rows[i][arrDSTruong[j]];
                        }
                        dr["sThucHien_4"] = dtDonViThanhVien.Rows[i]["sThucHien_4"];
                        dr["sNamTruoc_4"] = dtDonViThanhVien.Rows[i]["sNamTruoc_4"];
                        vR.Rows.Add(dr);
                    }
                }
                //sap xep lai dr

                //DataView dv = vR.DefaultView;
                //dv.Sort = "sKyHieu";
                //vR = dv.ToTable();
            }
            vR.Dispose();
            return vR;
        }

        public static DataTable Get_dtChungTuChiTiet_TheoChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

            SQL = String.Format("SELECT * FROM TCDN_ChungTuChiTiet WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        private static void DanhSTTChoCay(DataTable dtChiTieu, DataTable dtChungTuChiTiet, String MaChungTu, int MaHangMauCha, ref int STT)
        {
            int i, j, MaChiTieu;
            SqlCommand cmd;
            for (i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                if (MaHangMauCha == Convert.ToInt32(dtChiTieu.Rows[i]["iID_MaChiTieu_Cha"]))
                {
                    MaChiTieu = Convert.ToInt32(dtChiTieu.Rows[i]["iID_MaChiTieu"]);
                    for (j = 0; j < dtChungTuChiTiet.Rows.Count; j++)
                    {
                        if (MaChiTieu == Convert.ToInt32(dtChungTuChiTiet.Rows[j]["iID_MaChiTieu"]))
                        {
                            STT++;
                            cmd = new SqlCommand();
                            cmd.CommandText = String.Format("UPDATE TCDN_ChungTuChiTiet " +
                                                            "SET iSTT=@iSTT " +
                                                            "WHERE iID_MaChiTieu=@iID_MaChiTieu AND " +
                                                                  "iID_MaChungTu=@iID_MaChungTu");
                            cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                            cmd.Parameters.AddWithValue("@iID_MaChiTieu", MaChiTieu);
                            cmd.Parameters.AddWithValue("@iSTT", STT);
                            Connection.UpdateDatabase(cmd);
                        }
                    }
                    DanhSTTChoCay(dtChiTieu, dtChungTuChiTiet, MaChungTu, MaChiTieu, ref STT);
                }
            }
        }
        public  static  String  GetMeNuTCDN(String iID_MaChungTu,String Loai)
        {
            String style1 = "", style2 = "", style3 = "", style4 = "";
            if (Loai == "1") style1 = "style=\"color: red\"";
            if (Loai == "2") style2 = "style=\"color: red\"";
            if (Loai == "3") style3 = "style=\"color: red\"";
            if (Loai == "4") style4 = "style=\"color: red\"";

            String s = "<ul>";
            s +=
                String.Format(
                    "<li><a {1} href=\"TCDN_ChungTuChiTiet?iLoai=1&iID_MaChungTu={0}\">Hoạt động sản xuất KD</a></li>",iID_MaChungTu,style1);
            s +=
                String.Format(
                    "<li><a {1} href=\"TCDN_ChungTuChiTiet?iLoai=2&iID_MaChungTu={0}\">Thu chi ngân sách và TN</a></li>", iID_MaChungTu, style2);
            s +=
                String.Format(
                    "<li><a {1} href=\"TCDN_ChungTuChiTiet?iLoai=3&iID_MaChungTu={0}\">Chỉ tiêu tài chính</a></li>", iID_MaChungTu, style3);
            s +=
                String.Format(
                    "<li><a {1} href=\"TCDN_ChungTuChiTiet?iLoai=4&iID_MaChungTu={0}\">Chỉ tiêu đánh giá hiệu quả</a></li>", iID_MaChungTu, style4);
            s += "</ul>";
            return s;
        }

        public static String GetMenuHoSoDN(String iID_MaDoanhNghiep, String Loai,String DuLieuMoi="")
        {
            String s="",style1 = "", style2 = "", style3 = "", style4 = "", style5 = "";
            if(DuLieuMoi=="1")
            {
                if (Loai == "1") style1 = "style=\"color: red\"";
                if (Loai == "2") style2 = "style=\"color: red\"";
                if (Loai == "3") style3 = "style=\"color: red\"";
                if (Loai == "4") style4 = "style=\"color: red\"";
                if (Loai == "5") style5 = "style=\"color: red\"";

                 s = "<ul>";
                s +=
                    String.Format(
                        "<li><a {1} href=\"Edit?iLoai=1&iID_MaDoanhNghiep={0}\">Thông tin chung</a></li>", iID_MaDoanhNghiep, style1);
               
            }
            else
            {
                if (Loai == "1") style1 = "style=\"color: red\"";
                if (Loai == "2") style2 = "style=\"color: red\"";
                if (Loai == "3") style3 = "style=\"color: red\"";
                if (Loai == "4") style4 = "style=\"color: red\"";
                if (Loai == "5") style5 = "style=\"color: red\"";

                 s = "<ul>";
                s +=
                    String.Format(
                        "<li><a {1} href=\"Edit?iLoai=1&iID_MaDoanhNghiep={0}\">Thông tin chung</a></li>", iID_MaDoanhNghiep, style1);
                s +=
                    String.Format(
                        "<li><a {1} href=\"Edit?iLoai=2&iID_MaDoanhNghiep={0}\">Lĩnh vực hoạt động, ngành nghề</a></li>", iID_MaDoanhNghiep, style2);
                s +=
                    String.Format(
                        "<li><a {1} href=\"Edit?iLoai=3&iID_MaDoanhNghiep={0}\">Đơn vị thành viên</a></li>", iID_MaDoanhNghiep, style3);
                s +=
                    String.Format(
                        "<li><a {1} href=\"Edit?iLoai=4&iID_MaDoanhNghiep={0}\">Công ty liên doanh liên kết</a></li>", iID_MaDoanhNghiep, style4);
                s +=
                   String.Format(
                       "<li><a {1} href=\"Edit?iLoai=5&iID_MaDoanhNghiep={0}\">Dự án đang đầu tư</a></li>", iID_MaDoanhNghiep, style5);
                s += "</ul>";
            }
           
          
            return s;
        }
    }
}