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
    public class PhanBo_PhanBoChiTietModels
    {
        /// <summary>
        /// Thêm phân bổ cho đơn vị
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String ThemPhanBoChoDonVi(String iID_MaChiTieu, String iID_MaDonVi, String MaND, String IPSua)
        {
            int iID_MaTrangThaiDuyetMoi = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanBoModels.iID_MaPhanHePhanBo);
            DataTable dtChiTieu = PhanBo_ChiTieuModels.GetChiTieu(iID_MaChiTieu);
            Bang bang;
            String iID_MaPhanBo;

            bang = new Bang("PB_PhanBo");
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", dtChiTieu.Rows[0]["iID_MaChiTieu"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", dtChiTieu.Rows[0]["iID_MaDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", dtChiTieu.Rows[0]["dNgayDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChiTieu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChiTieu.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChiTieu.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChiTieu.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChiTieu.Rows[0]["bChiNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanBoModels.iID_MaPhanHePhanBo));
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyetMoi);
            iID_MaPhanBo = bang.Save().ToString();
            String MaDotPhanBo = Convert.ToString(dtChiTieu.Rows[0]["iID_MaDotPhanBo"]);
            String MaDotPhanBoDauNam = Get_MaDotPhanBoDauNam(Convert.ToString(dtChiTieu.Rows[0]["iNamLamViec"]),Convert.ToString(dtChiTieu.Rows[0]["iID_MaNguonNganSach"]),Convert.ToString(dtChiTieu.Rows[0]["iID_MaNamNganSach"]));
            //Thêm chi tiết phân bổ cho phân bổ
            ThemChiTiet(iID_MaPhanBo, MaND, IPSua, true);
            //if (MaDotPhanBo == MaDotPhanBoDauNam)
            //{
            //    ThemChiTiet(iID_MaPhanBo, MaND, IPSua,true);
            //}
            //else
            //{
            //    ThemChiTiet(iID_MaPhanBo, MaND, IPSua);
            //}
            InsertDuyetPhanBo(iID_MaPhanBo, "Tạo mới", MaND, IPSua);
            dtChiTieu.Dispose();
            return iID_MaPhanBo;
        }

        public static String InsertDuyetPhanBo(String iID_MaPhanBo, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("PB_DuyetPhanBo");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }
        /// <summary>
        /// Thêm chi tiết cho phân bổ
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        public static void ThemChiTiet(String iID_MaPhanBo, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            DataTable dtPhanBo = PhanBo_PhanBoModels.GetPhanBo(iID_MaPhanBo);

            String iID_MaChiTieu = Convert.ToString(dtPhanBo.Rows[0]["iID_MaChiTieu"]);
            int iNamLamViec = Convert.ToInt32(dtPhanBo.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtPhanBo.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtPhanBo.Rows[0]["bChiNganSach"]);
            DataTable dt = PhanBo_ChiTieuChiTietModels.Get_dtChiTieuChiTiet(iID_MaChiTieu, null);

            Bang bang = new Bang("PB_PhanBoChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", dtPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtPhanBo.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtPhanBo.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtPhanBo.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtPhanBo.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtPhanBo.Rows[0]["bChiNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtPhanBo.Rows[0]["iID_MaDonVi"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", dtPhanBo.Rows[0]["iID_MaChiTieu"]);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Dien thong tin cua Muc luc ngan sach
                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dt.Rows[i], bang.CmdParams.Parameters);
                bang.Save();
            }
            dt.Dispose();
            dtPhanBo.Dispose();
        }

        public static void ThemChiTiet(String iID_MaPhanBo, String MaND, String IPSua, Boolean CoTienDuToan)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            DataTable dtPhanBo = PhanBo_PhanBoModels.GetPhanBo(iID_MaPhanBo);

            String iID_MaChiTieu = Convert.ToString(dtPhanBo.Rows[0]["iID_MaChiTieu"]);
            int iNamLamViec = Convert.ToInt32(dtPhanBo.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtPhanBo.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtPhanBo.Rows[0]["bChiNganSach"]);
            DataTable dt = PhanBo_ChiTieuChiTietModels.Get_dtChiTieuChiTiet(iID_MaChiTieu, null);

            Bang bang = new Bang("PB_PhanBoChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", dtPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtPhanBo.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtPhanBo.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtPhanBo.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtPhanBo.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtPhanBo.Rows[0]["bChiNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtPhanBo.Rows[0]["iID_MaDonVi"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", dtPhanBo.Rows[0]["iID_MaChiTieu"]);
            //Chỉnh lại lấy dự toán theo đơn vị và chứng từ dự toán được chọn
            DataTable dtDuToan = Get_dtDuToanCuaDonVi(Convert.ToString(dtPhanBo.Rows[0]["iID_MaDonVi"]), Convert.ToString(dtPhanBo.Rows[0]["iID_MaChiTieu"]));
            //DataTable dtDuToan = Get_dtDuToanCuaDonVi(Convert.ToString(dtPhanBo.Rows[0]["iID_MaDonVi"]), Convert.ToString(dtPhanBo.Rows[0]["iNamLamViec"]), Convert.ToString(dtPhanBo.Rows[0]["iID_MaNguonNganSach"]), Convert.ToString(dtPhanBo.Rows[0]["iID_MaNamNganSach"]));
            Boolean DonViChoPhanBo=false;
            if (Convert.ToString(dtPhanBo.Rows[0]["iID_MaDonVi"])==PhanBo_Tong_BangDuLieu.iID_MaDonViChoPhanBo)
                DonViChoPhanBo=true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Dien thong tin cua Muc luc ngan sach
                ThemThongTinMucLucNganSach_CoTruongTien(dt.Rows[i], bang.CmdParams.Parameters, dtDuToan, DonViChoPhanBo);
                bang.Save();
            }
            
            //update lai cho phan bo
            dt.Dispose();
            dtPhanBo.Dispose();
        }

        public static void ThemThongTinMucLucNganSach_CoTruongTien(DataRow RMucLucNganSach, SqlParameterCollection Params,DataTable dt,Boolean DonViChoPhanBo=false)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String[] arrDSDuocNhapTruongTien = MucLucNganSachModels.strDSDuocNhapTruongTien.Split(',');
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrTruongTienSo = MucLucNganSachModels.strDSTruongTien_So.Split(',');
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
            for (int i = 0; i < arrDSDuocNhapTruongTien.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSDuocNhapTruongTien[i]) >= 0)
                {
                    Params["@" + arrDSDuocNhapTruongTien[i]].Value = RMucLucNganSach[arrDSDuocNhapTruongTien[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSDuocNhapTruongTien[i], RMucLucNganSach[arrDSDuocNhapTruongTien[i]]);
                }
            }
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
            if (DonViChoPhanBo==false)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string sXauNoiMa1 = Convert.ToString(dt.Rows[j]["sXauNoiMa"]);
                    if (sXauNoiMa == sXauNoiMa1)
                    {
                        for (int i = 0; i < arrTruongTienSo.Length; i++)
                        {
                            if (Params.IndexOf("@" + arrTruongTienSo[i]) >= 0)
                            {
                                Params["@" + arrTruongTienSo[i]].Value = dt.Rows[j][arrTruongTienSo[i]];
                                Params["@" + arrTruongTienSo[i]+"_ChiTieu"].Value = dt.Rows[j][arrTruongTienSo[i]];
                            }
                            else
                            {
                                Params.AddWithValue("@" + arrTruongTienSo[i], dt.Rows[j][arrTruongTienSo[i]]);
                                Params.AddWithValue("@" + arrTruongTienSo[i] + "_ChiTieu", dt.Rows[j][arrTruongTienSo[i]]);
                            }
                        }
                        break;
                    }
                    else
                    {
                        for (int i = 0; i < arrTruongTienSo.Length; i++)
                        {
                            if (Params.IndexOf("@" + arrTruongTienSo[i]) >= 0)
                            {
                                Params["@" + arrTruongTienSo[i]].Value = 0;
                                Params["@" + arrTruongTienSo[i] + "_ChiTieu"].Value = 0;
                            }
                            else
                            {
                                Params.AddWithValue("@" + arrTruongTienSo[i], 0);
                                Params.AddWithValue("@" + arrTruongTienSo[i]+"_ChiTieu", 0);
                            }
                        }                        
                    }
                }
            }
            else
            {
               
                for (int i = 0; i < arrTruongTienSo.Length; i++)
                {
                    if (Params.IndexOf("@" + arrTruongTienSo[i]) >= 0)
                    {
                        Params["@" + arrTruongTienSo[i]].Value =Convert.ToInt64(RMucLucNganSach[arrTruongTienSo[i]+"_DuToan"])*(-1);
                      //  Params["@" + arrTruongTienSo[i]].Value = 0;
                        Params["@" + arrTruongTienSo[i]+"_ChiTieu"].Value = 0;
                    }
                    else
                    {
                        Params.AddWithValue("@" + arrTruongTienSo[i], Convert.ToInt64(RMucLucNganSach[arrTruongTienSo[i]+"_DuToan"])*(-1));
                       // Params.AddWithValue("@" + arrTruongTienSo[i], 0);
                        Params.AddWithValue("@" + arrTruongTienSo[i]+"_ChiTieu", 0);
                    }
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
        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaPhanBo)
        {
            int vR = -1;
            DataTable dt = PhanBo_PhanBoModels.GetPhanBo(iID_MaPhanBo);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHePhanBo, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                ////Bỏ trong trường hợp phân bổ tổng thì ko có chi tiết phân bổ
                //if (iID_MaTrangThaiDuyet_TuChoi > 0)
                //{
                //    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM PB_PhanBoChiTiet WHERE iID_MaPhanBo=@iID_MaPhanBo AND bDongY=0");
                //    cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
                //    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                //    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                //    }
                //    cmd.Dispose();
                //}
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaPhanBo)
        {
            int vR = -1;
            DataTable dt = PhanBo_PhanBoModels.GetPhanBo(iID_MaPhanBo);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHePhanBo, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable Get_dtPhanBoChiTiet_TheoChiTieu(String iID_MaChiTieu)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM PB_PhanBoChiTiet WHERE iTrangThai=1 AND iID_MaChiTieu=@iID_MaChiTieu ORDER BY sXauNoiMa, iID_MaDonVi, sMaCongTrinh");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtPhanBoNganh_ChiTiet(String iID_MaChiTieu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand();
            String strDKTruongTien = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "<>0 OR ") + "<>0";
            String DKThem = "iID_MaDonVi=@iID_MaDonVi";
            DKThem += String.Format(" OR (bLaHangCha=0 AND ({0}))", strDKTruongTien);
            String DK = String.Format("iTrangThai=1 AND iID_MaChiTieu=@iID_MaChiTieu AND ({0})", DKThem);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong+",iID_MaDonVi";
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (arrDSTruong[i] == "iID_MaDonVi")
                        {
                            DK += String.Format(" AND {0}=@{0}1", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i]+"1", arrGiaTriTimKiem[arrDSTruong[i]]);
                        }
                        else
                        {
                            DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                        }
                       
                    }
                }
            }

            String SQL = String.Format("SELECT * FROM PB_PhanBoChiTiet WHERE {0} ORDER BY {1}, iID_MaDonVi, sMaCongTrinh", DK, MucLucNganSachModels.strDSTruongSapXep);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", PhanBo_Tong_BangDuLieu.iID_MaDonViChoPhanBo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable LayTongPhanBoCuaDonVi(String sLNS,
                                                      String iID_MaDonVi,
                                                      int iNamLamViec,
                                                      int iID_MaNguonNganSach,
                                                      int iID_MaNamNganSach,
                                                      Boolean bChiNganSach)
        {
            DataTable vR;
            String SQL = "", DK = "";
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            String strTruong = "iID_MaMucLucNganSach";
            String strTruongGroup = "iID_MaMucLucNganSach";

            for (int i = 0; i < arrDSTruongTien.Length; i++)
            {
                if (arrDSTruongTien[i].StartsWith("r"))
                {
                    strTruong += String.Format(",SUM({0}) AS Sum{0}", arrDSTruongTien[i]);
                }
                else
                {
                    strTruong += String.Format(",{0}", arrDSTruongTien[i]);
                    strTruongGroup += String.Format(",{0}", arrDSTruongTien[i]);
                }
            }
            DK = "sLNS=@sLNS AND " +
                 "iID_MaDonVi=@iID_MaDonVi AND " +
                 "iNamLamViec=@iNamLamViec AND " +
                 "iID_MaNguonNganSach=@iID_MaNguonNganSach AND " +
                 "iID_MaNamNganSach=@iID_MaNamNganSach AND " +
                 "bChiNganSach=@bChiNganSach";
            SQL = String.Format("SELECT {0} FROM PB_PhanBoChiTiet WHERE {1} GROUP BY {2} ", strTruong, DK, strTruongGroup);
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@bChiNganSach", bChiNganSach);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtTongPhanBoChoDonVi(String iID_MaMucLucNganSach,
                                                         String iID_MaDonVi,
                                                         int iNamLamViec,
                                                         String dNgayDotPhanBo,
                                                         int iID_MaNguonNganSach,
                                                         int iID_MaNamNganSach)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanBoModels.iID_MaPhanHePhanBo));
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            DK += " AND iID_MaMucLucNganSach=@iID_MaMucLucNganSach";
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            DK += " AND iID_MaDonVi=@iID_MaDonVi";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DK += " AND iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE iNamLamViec=@iNamLamViec  AND iID_MaNamNganSach=@iID_MaNamNganSach AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND dNgayDotPhanBo <= @dNgayDotPhanBo)";
            cmd.Parameters.AddWithValue("@dNgayDotPhanBo", dNgayDotPhanBo);

            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = "";
            for (int i = 0; i < arrDSTruongTien_So.Length; i++)
            {
                if (i > 0) strTruong += ",";
                strTruong += String.Format("SUM({0}) AS Sum{0}", arrDSTruongTien_So[i]);
            }

            cmd.CommandText = String.Format("SELECT {0} FROM PB_PhanBoChiTiet WHERE {1}", strTruong, DK);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtPhanBoChiTiet(String iID_MaPhanBo, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaPhanBo=@iID_MaPhanBo";
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0} LIKE @{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]] + "%");
                    }
                }
            }

            SQL = String.Format("SELECT * FROM PB_PhanBoChiTiet WHERE {0} ORDER BY sXauNoiMa", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy chờ phân bổ lũy kế đến trước ngày chứng từ của chi tiêu truyền vào
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <returns></returns>
        public static DataTable Get_dtDonViChoPhanBo(String iID_MaChiTieu)
        {
            DataTable dtCT = PhanBo_ChiTieuModels.GetChiTieu(iID_MaChiTieu);
            String SQL = @"SELECT iID_MaMucLucNganSach,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG, iID_MaDonVi
                        ,SUM(rNgay) AS rNgay,SUM(rSoNguoi) AS rSoNguoi,SUM(rChiTaiKhoBac) AS rChiTaiKhoBac,SUM(rTonKho) AS rTonKho
                        ,SUM(rTuChi) AS rTuChi
                        ,SUM(rTienThu) AS rTienThu
                        ,SUM(rTongSo) AS rTongSo
                        ,SUM(rChiTapTrung) AS rChiTapTrung
                        ,SUM(rHangNhap) AS rHangNhap
                        ,SUM(rHangMua) AS rHangMua
                        ,SUM(rHienVat) AS rHienVat
                        ,SUM(rDuPhong) AS rDuPhong
                        ,SUM(rPhanCap) AS rPhanCap
                        ,SUM(rNgay_ChiTieu) AS rNgay_ChiTieu,SUM(rSoNguoi_ChiTieu) AS rSoNguoi_ChiTieu,SUM(rChiTaiKhoBac_ChiTieu) AS rChiTaiKhoBac_ChiTieu,SUM(rTonKho_ChiTieu) AS rTonKho_ChiTieu
                        ,SUM(rTuChi_ChiTieu) AS rTuChi_ChiTieu
                        ,SUM(rTienThu_ChiTieu) AS rTienThu_ChiTieu
                        ,SUM(rTongSo_ChiTieu) AS rTongSo_ChiTieu
                        ,SUM(rChiTapTrung_ChiTieu) AS rChiTapTrung_ChiTieu
                        ,SUM(rHangNhap_ChiTieu) AS rHangNhap_ChiTieu
                        ,SUM(rHangMua_ChiTieu) AS rHangMua_ChiTieu
                        ,SUM(rHienVat_ChiTieu) AS rHienVat_ChiTieu
                        ,SUM(rDuPhong_ChiTieu) AS rDuPhong_ChiTieu
                        ,SUM(rPhanCap_ChiTieu) AS rPhanCap_ChiTieu
                        ,SUM(rTongSo) AS rTongSo
                        FROM PB_PhanBoChiTiet 
                        WHERE iTrangThai=1 AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND
                        iID_MaNamNganSach=@iID_MaNamNganSach AND (iID_MaDonVi=@iID_MaDonVi 
                        AND (iID_MaChiTieu in (SELECT  iID_MaChiTieu FROM PB_ChiTieu WHERE dNgayChungTu<(SELECT dNgayChungTu FROM PB_ChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu) )
                            OR iID_MaChiTieu in (SELECT  iID_MaChiTieu FROM PB_ChiTieu 
                               WHERE dNgayTao<(SELECT dNgayTao FROM PB_ChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu)))
                        AND
                        (bLaHangCha=0 AND (rNgay<>0 OR rSoNguoi<>0 
                        OR rChiTaiKhoBac<>0 OR rTonKho<>0 OR rTuChi<>0 OR rChiTapTrung<>0 
                        OR rHangNhap<>0 OR rHangMua<>0 OR rHienVat<>0 OR rDuPhong<>0 
                        OR rPhanCap<>0))) 
                        GROUP BY iID_MaMucLucNganSach,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG, iID_MaDonVi
                        ORDER BY sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG, iID_MaDonVi";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonVi", PhanBo_Tong_BangDuLieu.iID_MaDonViChoPhanBo);
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCT.Rows[0]["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtCT.Rows[0]["iID_MaNamNganSach"]);
            dtCT.Dispose();
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_DanhSachDonViDuToanDuocChon(String iID_MaChiTieu)
        {
            String SQL = @"SELECT DISTINCT iID_MaDonVi
                        FROM DT_ChungTuChiTiet WHERE 
                        iID_MaChungTu in (SELECT iID_MaDuToan FROM PB_ChiTieu_DuToan WHERE iID_MaChiTieu =@iID_MaChiTieu )";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị của người dùng của đợt dự toán đầu năm
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_dtDonViDuToan(String MaND, String iNamLamViec, String iID_MaNguonNganSach,String iID_MaNamNganSach)
        {
            String SQL = @"SELECT DISTINCT iID_MaDonVi
                        FROM DT_ChungTuChiTiet WHERE 
                        iID_MaNguonNganSach=@iID_MaNguonNganSach AND
                        iID_MaNamNganSach=@iID_MaNamNganSach AND
                        iID_MaDotNganSach =(SELECT TOP 1 iID_MaDotNganSach FROM DT_DotNganSach WHERE iNamLamViec=@iNamLamViec ORDER BY dNgayDotNganSach)
                        AND iID_MaDonVi in (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iNamLamViec=@iNamLamViec AND sMaNguoiDung=@MaND)";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaND",MaND);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_dtDuToanCuaDonVi(String iID_MaDonVi, String iID_MaChiTieu)
        {
            String DSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            String[] arrTruongTien = DSTruongTien.Split(',');
            String sSELECT = "";
            for (int i = 0; i < arrTruongTien.Length; i++)
            {
                sSELECT += String.Format(",SUM({0}) AS {0}", arrTruongTien[i]);
            }
            String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sXauNoiMa
                        {0}
                        FROM DT_ChungTuChiTiet 
                        WHERE iID_MaDonVi=@iID_MaDonVi	 
                        AND iID_MaChungTu IN  (SELECT iID_MaDuToan 
                        FROM PB_ChiTieu_DuToan 
                        WHERE iID_MaChiTieu=@iID_MaChiTieu)
                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sXauNoiMa
                        ORDER BY sXauNoiMa", sSELECT);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_dtDuToanCuaDonVi(String iID_MaDonVi, String iNamLamViec, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            String DSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            String[] arrTruongTien = DSTruongTien.Split(',');
            String sSELECT = "";
            for (int i = 0; i < arrTruongTien.Length; i++)
            {
                sSELECT += String.Format(",SUM({0}) AS {0}",arrTruongTien[i]);
            }
            String SQL = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sXauNoiMa
                        {0}
                        FROM DT_ChungTuChiTiet 
                        WHERE
                        iID_MaNguonNganSach=@iID_MaNguonNganSach AND
                        iID_MaNamNganSach=@iID_MaNamNganSach AND iID_MaDonVi=@iID_MaDonVi	 
                        AND iID_MaDotNganSach =(SELECT TOP 1 iID_MaDotNganSach 
                        FROM DT_DotNganSach 
                        WHERE iNamLamViec=@iNamLamViec ORDER BY dNgayDotNganSach)
                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sXauNoiMa
                        ORDER BY sXauNoiMa", sSELECT);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static String Get_MaDotPhanBoDauNam(String iNamLamViec, String iID_MaNguonNganSach, String iID_MaNamNganSach)
        {
            String SQL = @"SELECT TOP 1 iID_MaDotPhanBo FROM PB_DotPhanBo WHERE 
                        iID_MaNguonNganSach=@iID_MaNguonNganSach AND
                        iID_MaNamNganSach=@iID_MaNamNganSach AND iNamLamViec=@iNamLamViec ORDER BY dNgayDotPhanBo";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            String MaDotPhanBo = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return MaDotPhanBo;
        }


        public static DataTable Get_dtTongPhanBoTheoMucLucNganSach(String iID_MaChiTieu)
        {            
            String SQL = @"SELECT sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                        ,SUM(rNgay) AS rNgay,SUM(rSoNguoi) AS rSoNguoi,SUM(rChiTaiKhoBac) AS rChiTaiKhoBac,SUM(rTonKho) AS rTonKho
                        ,SUM(rTienThu) AS rTienThu
                        ,SUM(rTongSo) AS rTongSo
                        ,SUM(rTuChi) AS rTuChi                        
                        ,SUM(rChiTapTrung) AS rChiTapTrung
                        ,SUM(rHangNhap) AS rHangNhap
                        ,SUM(rHangMua) AS rHangMua
                        ,SUM(rHienVat) AS rHienVat
                        ,SUM(rDuPhong) AS rDuPhong
                        ,SUM(rPhanCap) AS rPhanCap
                        ,SUM(rNgay_ChiTieu) AS rNgay_ChiTieu,SUM(rSoNguoi_ChiTieu) AS rSoNguoi_ChiTieu,SUM(rChiTaiKhoBac_ChiTieu) AS rChiTaiKhoBac_ChiTieu,SUM(rTonKho_ChiTieu) AS rTonKho_ChiTieu
                        ,SUM(rTuChi_ChiTieu) AS rTuChi_ChiTieu
                        ,SUM(rTienThu_ChiTieu) AS rTienThu_ChiTieu
                        ,SUM(rTongSo_ChiTieu) AS rTongSo_ChiTieu
                        ,SUM(rChiTapTrung_ChiTieu) AS rChiTapTrung_ChiTieu
                        ,SUM(rHangNhap_ChiTieu) AS rHangNhap_ChiTieu
                        ,SUM(rHangMua_ChiTieu) AS rHangMua_ChiTieu
                        ,SUM(rHienVat_ChiTieu) AS rHienVat_ChiTieu
                        ,SUM(rDuPhong_ChiTieu) AS rDuPhong_ChiTieu
                        ,SUM(rPhanCap_ChiTieu) AS rPhanCap_ChiTieu
                        ,SUM(rTongSo) AS rTongSo
                        FROM PB_PhanBoChiTiet 
                        WHERE iTrangThai=1 AND
                        iID_MaChiTieu =@iID_MaChiTieu                  
                        AND
                        bLaHangCha=0 AND (rNgay<>0 OR rSoNguoi<>0 
                        OR rChiTaiKhoBac<>0 OR rTonKho<>0 OR rTuChi<>0 OR rChiTapTrung<>0 
                        OR rHangNhap<>0 OR rHangMua<>0 OR rHienVat<>0 OR rDuPhong<>0 
                        OR rPhanCap<>0) 
                        GROUP BY sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                        ORDER BY sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;         
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_dtTongPhanBoTheoMucLucNganSach1(String iID_MaChiTieu)
        {
            String SQL = @"SELECT sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                        ,SUM(rNgay) AS rNgay,SUM(rSoNguoi) AS rSoNguoi,SUM(rChiTaiKhoBac) AS rChiTaiKhoBac,SUM(rTonKho) AS rTonKho
                        ,SUM(rTienThu) AS rTienThu
                        ,SUM(rTongSo) AS rTongSo
                        ,SUM(rTuChi) AS rTuChi                        
                        ,SUM(rChiTapTrung) AS rChiTapTrung
                        ,SUM(rHangNhap) AS rHangNhap
                        ,SUM(rHangMua) AS rHangMua
                        ,SUM(rHienVat) AS rHienVat
                        ,SUM(rDuPhong) AS rDuPhong
                        ,SUM(rPhanCap) AS rPhanCap
                        ,SUM(rNgay_DuToan) AS rNgay_ChiTieu
,SUM(rSoNguoi_DuToan) AS rSoNguoi_ChiTieu
,SUM(rChiTaiKhoBac_DuToan) AS rChiTaiKhoBac_ChiTieu
,SUM(rTonKho_DuToan) AS rTonKho_ChiTieu
                        ,SUM(rTuChi_DuToan) AS rTuChi_ChiTieu
                        ,SUM(rTienThu_DuToan) AS rTienThu_ChiTieu
                        ,SUM(rTongSo_DuToan) AS rTongSo_ChiTieu
                        ,SUM(rChiTapTrung_DuToan) AS rChiTapTrung_ChiTieu
                        ,SUM(rHangNhap_DuToan) AS rHangNhap_ChiTieu
                        ,SUM(rHangMua_DuToan) AS rHangMua_ChiTieu
                        ,SUM(rHienVat_DuToan) AS rHienVat_ChiTieu
                        ,SUM(rDuPhong_DuToan) AS rDuPhong_ChiTieu
                        ,SUM(rPhanCap_DuToan) AS rPhanCap_ChiTieu
                        ,SUM(rTongSo) AS rTongSo
                        FROM PB_ChiTieuChiTiet 
                        WHERE iTrangThai=1 AND
                        iID_MaChiTieu =@iID_MaChiTieu                            
                        AND
                        bLaHangCha=0 AND (rNgay<>0 OR rSoNguoi<>0 
                        OR rChiTaiKhoBac<>0 OR rTonKho<>0 OR rTuChi<>0 OR rChiTapTrung<>0 
                        OR rHangNhap<>0 OR rHangMua<>0 OR rHienVat<>0 OR rDuPhong<>0 
                        OR rPhanCap<>0) 
                        GROUP BY sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                        ORDER BY sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}