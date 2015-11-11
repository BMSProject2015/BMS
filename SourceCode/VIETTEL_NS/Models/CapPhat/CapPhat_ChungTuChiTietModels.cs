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
    public class CapPhat_ChungTuChiTietModels
    {
        private static DataTable _Chitiet;
        public static void ThemChiTiet(String iID_MaCapPhat, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            DataTable dtCapPhat = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            //DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            int iNamLamViec = Convert.ToInt32(dtCapPhat.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtCapPhat.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtCapPhat.Rows[0]["iID_MaNamNganSach"]);
            Boolean bChiNganSach = Convert.ToBoolean(dtCapPhat.Rows[0]["bChiNganSach"]);

            Bang bang = new Bang("CP_CapPhatChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", dtCapPhat.Rows[0]["iID_MaCapPhat"]);
            bang.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", dtCapPhat.Rows[0]["iDM_MaLoaiCapPhat"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtCapPhat.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtCapPhat.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtCapPhat.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCapPhat.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtCapPhat.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtCapPhat.Rows[0]["bChiNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", "");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dt.Rows[i]["sLNS"]);

                DataTable dtMucLucNganSach = NganSach_HamChungModels.DT_MucLucNganSach_sLNS(sLNS);
                for (int j = 0; j < dtMucLucNganSach.Rows.Count; j++)
                {   
                    //Dien thong tin cua Muc luc ngan sach
                    NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLucNganSach.Rows[j], bang.CmdParams.Parameters);
                    //if (Convert.ToBoolean(dtMucLucNganSach.Rows[j]["bLaHangCha"]))
                    //{
                    //bang.CmdParams.Parameters["@iID_MaDonVi"].Value = "";
                        bang.Save();
                    //}
                    //else
                    //{
                    //    for (int csDonVi = 0; csDonVi < dtDonVi.Rows.Count; csDonVi++)
                    //    {
                    //        bang.CmdParams.Parameters["@iID_MaDonVi"].Value = dtDonVi.Rows[csDonVi]["iID_MaDonVi"];
                    //        bang.Save();
                    //    }
                    //}
                }
                dtMucLucNganSach.Dispose();
            }

            dt.Dispose();
            dtCapPhat.Dispose();
            //dtDonVi.Dispose();
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaCapPhat)
        {
            int vR = -1;
            DataTable dt = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeCapPhat, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM CP_CapPhatChiTiet WHERE iID_MaCapPhat=@iID_MaCapPhat AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaCapPhat)
        {
            int vR = -1;
            DataTable dt = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeCapPhat, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        /*<summary>
         * Hàm lấy dự liệu của bảng chức từ chi tiết để fill vào lưới nhập
         * Dư liệu bao gồm:
         * Lấy tất cả ngân sách từ bảng mục lục ngân sách left join với 
         * bản chứng từ cấp phát chi tiết
         * </summary>
         */
        public static DataTable Get_dtChungTuChiTiet(String iID_MaCapPhat, Dictionary<String, String> arrGiaTriTimKiem,String Loai="",String MaND="")
        {
            // HungPX: lấy giá trị chi tiết đến của chứng từ
            DataTable dtCapPhat;
            SqlCommand cmdCapPhat = new SqlCommand();
            String sqlCapPhat = "Select * from Cp_CapPhat where iID_MaCapPhat = @iID_MaCapPhat";
            cmdCapPhat.Parameters.AddWithValue("@iID_MaCapPhat",iID_MaCapPhat);
            cmdCapPhat.CommandText = sqlCapPhat;
            dtCapPhat = Connection.GetDataTable(cmdCapPhat);
            String sLoai = Convert.ToString( dtCapPhat.Rows[0]["sLoai"] );
            dtCapPhat.Dispose();
            cmdCapPhat.Dispose();
            
            //HungPX : điều kiện chỉ lấy những hàng chứng từ nhập chi tiết đến "sLoai"
            String DKsLoai="";
            if (string.IsNullOrEmpty(sLoai) || sLoai != "sNG")
            {
                int IndexChiTietDen = CapPhat_BangDuLieu.getIndex(sLoai);
                String TenLoaiCon = MucLucNganSachModels.arrDSTruong[IndexChiTietDen + 1];
                DKsLoai += String.Format(@"AND {0} = '' ",TenLoaiCon);
            }
           
            DataTable vR;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            DataTable dtChungTu = GetChungTu(iID_MaCapPhat);
            DataRow RChungTu = dtChungTu.Rows[0];
            String iLoai = Convert.ToString(dtChungTu.Rows[0]["iLoai"]);
            String sDSLNS = Convert.ToString(dtChungTu.Rows[0]["sDSLNS"]);

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaCapPhatChiTiet,iID_MaDonVi,sTenDonVi,rTongSo,sMaCongTrinh";
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
                DK += " (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + LoaiNS + " AND LEN(sLNS)=3))";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
            }
            if (dt.Rows.Count > 0)
                DK += " ) ";

            String DKLoai = "";
            //ngan sach quoc phong tung loai ns 
            if (iLoai == "1")
            {
               
                if (!string.IsNullOrEmpty(sDSLNS))
                {
                    DKLoai += String.Format("and sLNS = @sDSLNS");
                    cmd.Parameters.AddWithValue("@sDSLNS", sDSLNS);
                }
                else
                    DKLoai += String.Format("and sLNS like '1%'");
            }

            SQL = String.Format("SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} FROM NS_MucLucNganSach WHERE {2} {4} {5} ORDER BY {3}", MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, DK, MucLucNganSachModels.strDSTruongSapXep, DKLoai, DKsLoai);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //Lay toan bo Muc luc ngan sach-->

            DataColumn column;

           

            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j] == "rTongSo" ||
                   (arrDSTruongTien[j] != "sTenCongTrinh" && arrDSTruongTien[j] != "iID_MaCapPhatChiTiet" && arrDSTruongTien[j] != "iID_MaDonVi" && arrDSTruongTien[j] != "sTenDonVi" && arrDSTruongTien[j] != "sMaCongTrinh"))
                {
                    column = new DataColumn(arrDSTruongTien[j], typeof(Double));
                    column.DefaultValue = 0;
                    vR.Columns.Add(column);
                    column = new DataColumn(arrDSTruongTien[j] + "_PhanBo", typeof(Double));
                    column.DefaultValue = 0;
                    vR.Columns.Add(column);
                    column = new DataColumn(arrDSTruongTien[j] + "_DaCap", typeof(Double));
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

            cmd = new SqlCommand();

            //Lay Du Lieu Trong Bang QTA_ChungTuChiTiet
            DK = "iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat";
            DK += " AND (";
            for (int i = 1; i < arrDSTruongTien.Length - 5;i++)
            {
                DK += arrDSTruongTien[i] + "<>0 OR ";
            }
            DK = DK.Substring(0, DK.Length - 3);
            DK += ") ";

            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            SQL = String.Format("SELECT * FROM CP_CapPhatChiTiet WHERE {0} ORDER BY sXauNoiMa", DK);
            cmd.CommandText = SQL;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            int cs0 = 0;

            int vRCount = vR.Rows.Count;
            for (int i = 0; i < vRCount; i++)
            {
                int count = 0;
                for (int j = cs0; j < dtChungTuChiTiet.Rows.Count; j++)
                {
                  
                    Boolean ok = true;
                    for (int k = 0; k < arrDSTruong.Length; k++)
                    {
                        if (Convert.ToString(vR.Rows[i][arrDSTruong[k]]) != Convert.ToString(dtChungTuChiTiet.Rows[j][arrDSTruong[k]]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (count == 0)
                        {
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            count++;
                        }
                        else
                        {
                            DataRow row =vR.NewRow();
                            for (int k = 0; k < vR.Columns.Count - 1; k++)
                            {
                                row[k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                            }
                            vR.Rows.InsertAt(row,i+1);
                            i++;
                            vRCount++;
                        }
                    }
                }

            }
            //HungPX: Lấy thông tin cột tiền đã cấp
            DataRow dr = dtChungTu.Rows[0];
            Dien_TruongDaCapPhat(dr, vR);


            dtChungTu.Dispose();
            dtChungTuChiTiet.Dispose();
            cmd.Dispose();

            //_Chitiet = vR.Copy();
            return vR;
        }

        public static DataTable GetChungTu(String iID_MaCapPhat)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CP_CapPhat WHERE iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_dtTongCapPhatChoDonVi(String iID_MaMucLucNganSach,
                                                         String iID_MaDonVi,
                                                         int iNamLamViec,
                                                         String dNgayCapPhat,
                                                         int iID_MaNguonNganSach,
                                                         int iID_MaNamNganSach)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(CapPhatModels.iID_MaPhanHe));
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
            DK += " AND iID_MaCapPhat IN (SELECT iID_MaCapPhat FROM CP_CapPhat WHERE dNgayCapPhat <= @dNgayCapPhat)";
            cmd.Parameters.AddWithValue("@dNgayCapPhat", dNgayCapPhat);

            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = "";
            for (int i = 0; i < arrDSTruongTien_So.Length; i++)
            {
                if (i > 0) strTruong += ",";
                strTruong += String.Format("SUM({0}) AS Sum{0}",arrDSTruongTien_So[i]);
            }

            cmd.CommandText = String.Format("SELECT {0} FROM CP_CapPhatChiTiet WHERE {1}", strTruong, DK);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        private static void Dien_TruongDaCapPhat(DataRow RChungTuCP, DataTable dtChiTiet)
        {
            DataRow R_ChiTiet, R_CP;
            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            DataTable dtCapPhat = Get_dtTongDaCapPhat(Convert.ToString(RChungTuCP["iNamLamViec"]), Convert.ToString(RChungTuCP["iDM_MaLoaiCapPhat"]), Convert.ToString(RChungTuCP["iID_MaTinhChatCapThu"]), Convert.ToString(RChungTuCP["sDSLNS"]), Convert.ToString(RChungTuCP["sLoai"]), Convert.ToString(RChungTuCP["dNgayCapPhat"]));
            _Chitiet = dtCapPhat.Copy();
            int tg = 0;
            for (int i = 0; i < dtCapPhat.Rows.Count; i++)
            {

                R_CP = dtCapPhat.Rows[i];

                for (int j = tg; j < dtChiTiet.Rows.Count; j++)
                {
                    R_ChiTiet = dtChiTiet.Rows[j];
                    if ( Convert.ToString(R_ChiTiet["iID_MaMucLucNganSach"]) == Convert.ToString(R_CP["iID_MaMucLucNganSach"]) &&
                        Convert.ToString(R_CP["iID_MaDonVi"]) == Convert.ToString(R_ChiTiet["iID_MaDonVi"])) 
                    {
                        Double rValues = 0;
                        rValues = Convert.ToDouble(R_CP["rTuChi"]);
                        R_ChiTiet["rTuChi_DaCap"] = rValues;
                        tg = j++;
                        break;
                    }
                }
            }



            dtCapPhat.Dispose();
        }

        //hungpx
        /*
         * <summary>
         * hàm lấy dữ liệu từ bảng CP_ChungTuChiTiet. Các hàng được lấy thuộc về các chứng từ cùng loại và thuộc đợt trược.
         * Dữ liệu lấy về là tổng trường tiền cấp phát vào đợt trước, giá trị này sẽ trở thành đã cấp ở đợt này
         * </summary>
         */
        private static DataTable Get_dtTongDaCapPhat(String iNamLamViec,
                                                String iDM_MaLoaiCapPhat,
                                                String iID_MaTinhChatCapThu,
                                                String sDSLNS,
                                                String sChiTietDen,
                                                String dNgayCapPhat)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";

            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DK += " AND iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
            cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            DK += " AND iID_MaTinhChatCapThu=@iID_MaTinhChatCapThu";
            cmd.Parameters.AddWithValue("@iID_MaTinhChatCapThu", iID_MaTinhChatCapThu);
            DK += " AND sLNS=@sLNS";
            cmd.Parameters.AddWithValue("@sLNS", sDSLNS);
            //DK += " AND sChiTietDen=@sChiTietDen";
            //cmd.Parameters.AddWithValue("@sChiTietDen", sChiTietDen);
            DK += " AND dNgayCapPhat<@dNgayCapPhat";
            cmd.Parameters.AddWithValue("@dNgayCapPhat", dNgayCapPhat);

            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = "";
            for (int i = 0; i < arrDSTruongTien_So.Length; i++)
            {
                if (i > 0) strTruong += ",";
                strTruong += String.Format("SUM({0}) AS {0}", arrDSTruongTien_So[i]);
            }

            String selectTable = String.Format("SELECT iID_MaMucLucNganSach,sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaDonVi,{0} FROM CP_CapPhatChiTiet WHERE {1} GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaMucLucNganSach,iID_MaDonVi", strTruong, DK);
            String orderTable = "SELECT * FROM (" + selectTable + ") A ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaMucLucNganSach";
            cmd.CommandText = orderTable ;
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;    
        }
        //HungPx: lay tien da cap tu chi
        public static string getTienDaCap(String sLNS,
                                        String sL,
                                        String sK,
                                        String sM,
                                        String sTM,
                                        String sTTM,
                                        String sNG,
                                        String iID_MaDonVi)
        {
                for (int i = 0; i < _Chitiet.Rows.Count; i++)
                {
                        if (Convert.ToString(_Chitiet.Rows[i]["sLNS"]) == sLNS &&
                    Convert.ToString(_Chitiet.Rows[i]["sL"]) == sL &&
                    Convert.ToString(_Chitiet.Rows[i]["sK"]) == sK &&
                    Convert.ToString(_Chitiet.Rows[i]["sM"]) == sM &&
                    Convert.ToString(_Chitiet.Rows[i]["sTM"]) == sTM &&
                    Convert.ToString(_Chitiet.Rows[i]["sTTM"]) == sTTM &&
                    Convert.ToString(_Chitiet.Rows[i]["sNG"]) == sNG &&
                    Convert.ToString(_Chitiet.Rows[i]["iID_MaDonVi"]) == iID_MaDonVi
                    )
                        {
                            return Convert.ToString(_Chitiet.Rows[i]["rTuChi"]);
                        }
                }
                return "";
        }
    }
}