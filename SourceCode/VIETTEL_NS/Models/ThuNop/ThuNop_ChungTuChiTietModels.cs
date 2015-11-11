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
    public class ThuNop_ChungTuChiTietModels
    {
        public static void ThemChiTiet(String iID_MaChungTu, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien_ThuNop.Split(',');
            DataTable dtChungTu = ThuNop_ChungTuModels.GetChungTu(iID_MaChungTu);

            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtChungTu.Rows[0]["bChiNganSach"]);
            String sLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);
            int iLoai = Convert.ToInt32(dtChungTu.Rows[0]["iLoai"]);

            DataTable dt = NganSach_HamChungModels.DT_MucLucNganSach_sLNS_TheoDau(sLNS);

            Bang bang = new Bang("TN_ChungTuChiTiet");
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
            bang.CmdParams.Parameters.AddWithValue("@iLoai", dtChungTu.Rows[0]["iLoai"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
            for (int i = 0; i < arrDSTruongTien.Length; i++)
            {
                bang.CmdParams.Parameters.AddWithValue("@b" + arrDSTruongTien[i], false);
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //Dien thong tin cua Muc luc ngan sach
                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSachKhongLayTruongTien(dt.Rows[i], bang.CmdParams.Parameters);
                //Điền các cột theo loại
                switch (iLoai)
                {
                    case 1:
                        bang.CmdParams.Parameters["@brNopNSQP"].Value = true;
                        bang.CmdParams.Parameters["@bsGhiChu"].Value = true;

                        break;
                    case 2:

                        bang.CmdParams.Parameters["@brTongThu"].Value = true;
                        bang.CmdParams.Parameters["@brKhauHaoTSCD"].Value = true;
                        bang.CmdParams.Parameters["@brTienLuong"].Value = true;
                        bang.CmdParams.Parameters["@brQTNSKhac"].Value = true;
                        bang.CmdParams.Parameters["@brChiPhiKhac"].Value = true;
                        bang.CmdParams.Parameters["@brChenhLech"].Value = true;
                        bang.CmdParams.Parameters["@brNopNSQP"].Value = true;
                        bang.CmdParams.Parameters["@brNopCapTren"].Value = true;
                        bang.CmdParams.Parameters["@brBoSungKinhPhi"].Value = true;
                        bang.CmdParams.Parameters["@brTrichQuyDonVi"].Value = true;
                        bang.CmdParams.Parameters["@brSoChuaPhanPhoi"].Value = true;
                        bang.CmdParams.Parameters["@bsGhiChu"].Value = true;

                        break;
                    case 3:
                        bang.CmdParams.Parameters["@brDuToanDuocDuyet"].Value = true;
                        bang.CmdParams.Parameters["@brThucHien"].Value = true;
                        bang.CmdParams.Parameters["@brSoXacNhan"].Value = true;
                        bang.CmdParams.Parameters["@bsGhiChu"].Value = true;
                        break;
                }
                bang.Save();
            }
            dt.Dispose();
            dtChungTu.Dispose();
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

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = ThuNop_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(ThuNopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM TN_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
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
            DataTable dt = ThuNop_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(ThuNopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        /// <summary>
        /// Lay danh sach chung tu chi tiet
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="arrGiaTriTimKiem"></param>
        /// <param name="Loai"></param>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable Get_dtChungTuChiTiet(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String Loai = "", String MaND = "", int iLoai = 1)
        {

            DataTable vR;
            DataTable dt = NganSach_HamChungModels.DSLNSTheoLNS(MaND, "8");
            // DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            DataTable dtChungTu = ThuNop_ChungTuModels.GetChungTu(iID_MaChungTu);
            DataRow RChungTu = dtChungTu.Rows[0];
            String sTruongTien = "";
            if (iLoai == 1)
            {
                sTruongTien = MucLucNganSachModels.strDSTruongTien_ThuNop_So_Loai1 + ",iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi,sGhiChu";

            }
            else
            {
                sTruongTien = "iID_MaChungTuChiTiet,iID_MaDonVi,sTenDonVi," + MucLucNganSachModels.strDSTruongTien_ThuNop;

            }
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String SQL, DK;
            SqlCommand cmd;

            //<--Lay toan bo Muc luc ngan sach
            cmd = new SqlCommand();
            DK = "iTrangThai=1";

            if (arrGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }
            if (dt.Rows.Count > 0)
                DK += " AND( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                String LoaiNS = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                DK += " (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=@Nguon" + i + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=@LoaiNS" + i + " AND LEN(sLNS)=3))";
                //DK += " (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
                cmd.Parameters.AddWithValue("@Nguon" + i, Nguon);
                cmd.Parameters.AddWithValue("@LoaiNS" + i, LoaiNS);
            }
            if (dt.Rows.Count > 0)
                DK += " ) ";


            SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0} FROM NS_MucLucNganSach WHERE {1} AND bLaHangCha=0 AND sLNS=8010101 AND sNG<>'00'  AND SUBSTRING(sLNS,1,1)='8' ORDER BY {2}", MucLucNganSachModels.strDSTruong, DK, MucLucNganSachModels.strDSTruongSapXep);

            //vR = Connection.GetDataTable(cmd);
            SQL =
                String.Format(
                    @"SELECT bLaHangCha=0,iID_MaMucLucNganSach='', iID_MaMucLucNganSach_Cha='',TN_sLNS='',sL='',sK='',sM='',sTM='',sTTM='',sTNG='',REPLACE(DC_DanhMuc.sTen,DC_DanhMuc.sTenKhoa+'-','') as sMoTa,DC_DanhMuc.sTenKhoa as sNG FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'TN_LoaiHinh' ORDER BY iSTT");
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //Lay toan bo Muc luc ngan sach-->

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

            //Them cot duyet
            column = new DataColumn("bDongY", typeof(Boolean));
            column.AllowDBNull = true;
            vR.Columns.Add(column);
            column = new DataColumn("sLyDo", typeof(String));
            column.AllowDBNull = true;
            vR.Columns.Add(column);
            column = new DataColumn("sSoCT", typeof(String));
            column.AllowDBNull = true;
            vR.Columns.Add(column);
            column = new DataColumn("bThoaiThu", typeof(Boolean));
            column.AllowDBNull = true;
            vR.Columns.Add(column);


            cmd = new SqlCommand();


            //Lay Du Lieu Trong Bang QTA_ChungTuChiTiet
            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            //DK += " AND (";
            //for (int i = 1; i < arrDSTruongTien.Length - 5; i++)
            //{
            //    DK += arrDSTruongTien[i] + "<>0 OR ";
            //}
            //DK = DK.Substring(0, DK.Length - 3);
            //DK += ") ";

            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            SQL = String.Format("SELECT * FROM TN_ChungTuChiTiet WHERE {0} ORDER BY iID_MaDonVi,bThoaiThu", DK);
            cmd.CommandText = SQL;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;

            //Dien_TruongDaQuyetToan(RChungTu, vR);            
            //Dien_TuChiTuLuong(RChungTu, vR);
            //Dien_TruongChiTieu(RChungTu, vR, bLoaiThang_Quy, iThang_Quy);
            int vRCount = vR.Rows.Count;
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {

                    Boolean ok = true;
                    //for (int k = 0; k < arrDSTruong.Length; k++)
                    //{
                    if (Convert.ToString(vR.Rows[i]["sNG"]) != Convert.ToString(dtChungTuChiTiet.Rows[j]["sNG"]))
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

            }
            return vR;
        }
        /// <summary>
        /// Lay gia tri chi tieu da cap
        /// </summary>
        /// <param name="iID_MaMucLucNganSach"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iID_MaNguonNganSach"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <returns></returns>
        public static String LayGiaTri_ChiTieu_DaCap(String iID_MaMucLucNganSach,
                                                     String iID_MaDonVi,
                                                     int iNamLamViec,

                                                     int iID_MaNguonNganSach,
                                                     int iID_MaNamNganSach)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL =
                "SELECT SUM(rTuChi) as rTuChi FROM DT_ChungTuChiTiet WHERE iTrangThai=1";
            // SQL += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            // cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan));
            SQL += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            SQL += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            SQL += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            SQL += " AND iID_MaMucLucNganSach=@iID_MaMucLucNganSach";
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            SQL += " AND iID_MaDonVi=@iID_MaDonVi";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.CommandText = String.Format(SQL);
            DataTable dtChiTieu_DuToan = Connection.GetDataTable(cmd);
            cmd.Parameters.Clear();
            cmd.Dispose();

            String vR = "";
            if (dtChiTieu_DuToan != null && dtChiTieu_DuToan.Rows.Count > 0)
            {
                vR = Convert.ToString(dtChiTieu_DuToan.Rows[0]["rTuChi"]);
                dtChiTieu_DuToan.Dispose();
            }


            return vR;
        }
        public static DataTable Get_dtThuNopChiTiet(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
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

            SQL = String.Format("c WHERE {0} ORDER BY sXauNoiMa, iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}