using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Controls;
using System.Collections.Specialized;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class CapPhat_ChungTuChiTiet_DonViModels : CapPhatChungTuChiTietCha
    {
        /// <summary>
        /// Get_dtTongCapPhatChoDonVi
        /// </summary>
        /// <param name="iID_MaMucLucNganSach"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="dNgayCapPhat"></param>
        /// <param name="iID_MaNguonNganSach"></param>
        /// <param name="iID_MaNamNganSach"></param>
        /// <returns></returns>
        //VungNV: 2015/10/29 update param
        public static DataTable Get_dtTongCapPhatChoDonVi(  String iDM_MaLoaiCapPhat,
                                                            String iID_MaTinhChatCapThu,
                                                            String sLNS, 
                                                            String iID_MaDonVi,
                                                            String dNgayCapPhat,
                                                            String dNgaySua, 
                                                            int iNamLamViec)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            DK += " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(CapPhatModels.iID_MaPhanHe));
            DK += " AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);

            DK += " AND iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat";
            cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);

            DK += " AND iID_MaTinhChatCapThu=@iID_MaTinhChatCapThu";
            cmd.Parameters.AddWithValue("@iID_MaTinhChatCapThu", iID_MaTinhChatCapThu);

            DK += " AND sLNS LIKE @sLNS";
            cmd.Parameters.AddWithValue("@sLNS", sLNS);

            DK += " AND iID_MaDonVi=@iID_MaDonVi";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);

            DK += " AND iID_MaCapPhat IN (SELECT iID_MaCapPhat FROM CP_CapPhat WHERE (dNgayCapPhat < @dNgayCapPhat) OR (dNgayCapPhat=@dNgayCapPhat AND dNgaySua<@dNgaySua))";
            cmd.Parameters.AddWithValue("@dNgayCapPhat", dNgayCapPhat);
            cmd.Parameters.AddWithValue("@dNgaySua", dNgaySua);

            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String strTruong = "";
            for (int i = 0; i < arrDSTruongTien_So.Length; i++)
            {
                if (i > 0) strTruong += ",";
                strTruong += String.Format("SUM({0}) AS {0}", arrDSTruongTien_So[i]);
            }

            cmd.CommandText = String.Format("SELECT sXauNoiMa, {0} FROM CP_CapPhatChiTiet WHERE {1} GROUP BY sXauNoiMa", strTruong, DK);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        
        /// <summary>
        /// Dien_TruongDaCap
        /// </summary>
        /// <param name="RChungTu"></param>
        /// <param name="dt"></param>
        private static void DienTruongDaCap(DataRow RChungTu, DataTable dt)
        {
            String[] arrDSTruongTien_So = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            DataRow R;
            DataRow RChiTiet;
            //VungNV: lay gia tri cac truong tu bang CP_ChiTiet
            String iDM_MaLoaiCapPhat = Convert.ToString(RChungTu["iDM_MaLoaiCapPhat"]);
            String iID_MaTinhChatCapThu = Convert.ToString(RChungTu["iID_MaTinhChatCapThu"]);
            String sLNS = Convert.ToString(RChungTu["sDSLNS"]);
            String iID_MaDonVi = Convert.ToString(RChungTu["iID_MaDonVi"]);
            String dNgayCapPhat = Convert.ToString(RChungTu["dNgayCapPhat"]);
            String dNgaySua = Convert.ToString(RChungTu["dNgaySua"]);
            int iNamLamViec = Convert.ToInt32(RChungTu["iNamLamViec"]);

            DataTable dtChiTieu =
                Get_dtTongCapPhatChoDonVi(iDM_MaLoaiCapPhat, iID_MaTinhChatCapThu,
                        sLNS, iID_MaDonVi, dNgayCapPhat, dNgaySua, iNamLamViec);

            int tg = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                R = dt.Rows[i];
                if(dtChiTieu != null )
                {
                    for (int j = tg; j < dtChiTieu.Rows.Count; j++)
                    {
                        RChiTiet = dtChiTieu.Rows[j];

                        if (dt.Columns.IndexOf("rTuChi_DaCap") >= 0 && Convert.ToString(R["sXauNoiMa"]) == Convert.ToString(RChiTiet["sXauNoiMa"]))
                        {
                            Double rValues = 0;

                            if (!String.IsNullOrEmpty(Convert.ToString(RChiTiet["rTuChi"]))) 
                            {
                                rValues = Convert.ToDouble(RChiTiet["rTuChi"]);
                            }

                            R["rTuChi_DaCap"] = rValues;
                            tg = j++;
                            break;
                        }
                    }
                }
            }
            dtChiTieu.Dispose();
        }
        /// <summary>
        /// Hàm lấy toàn bộ thông tin chi tiết của Chứng từ cấp phát chi tiết
        /// trong danh sách chứng từ đơn vị để hiện thị lên lưới nhập
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="arrGiaTriTimKiem"></param>
        /// <returns></returns>
        public static DataTable LayDtCapPhatChiTietDonVi(String iID_MaCapPhat, Dictionary<String, String> arrGiaTriTimKiem, String sMaND)
        {
            DataTable vR;
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(sMaND);
            DataTable dtChungTu = CapPhat_ChungTuModels.LayToanBoThongTinChungTu(iID_MaCapPhat);
            DataRow RChungTu = dtChungTu.Rows[0];
            //VungNV: 2015/10/27 get sLoai and sLNS
            String sLoai = Convert.ToString(RChungTu["sLoai"]);
            String sLNS = Convert.ToString(RChungTu["sDSLNS"]);

            String sTruongTien = MucLucNganSachModels.strDSTruongTien + ",iID_MaCapPhatChiTiet";
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTien = sTruongTien.Split(',');

            String SQL;
            String DK;
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
            
            if (dt != null && dt.Rows.Count > 0)
            { 
                DK += " AND( "; 
            }
                
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Nguon = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 1);
                String Loai = Convert.ToString(dt.Rows[i]["sLNS"]).Substring(0, 3);
                DK += " (sLNS=@sLNS" + i + " OR (SUBSTRING(sLNS,1,1)=" + Nguon + " AND LEN(sLNS)=1) OR  (SUBSTRING(sLNS,1,3)=" + Loai + " AND LEN(sLNS)=3))";
                if (i < dt.Rows.Count - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dt.Rows[i]["sLNS"]);
            }

            if (dt.Rows.Count > 0)
            { 
                 DK += " ) ";
            }
            //VungNV: 2015/10/27 them dieu kien theo sLoai
            String DKLoai = "";
            if(!String.IsNullOrEmpty(sLoai))
            {    
                for (int i = 0; i < arrDSTruong.Length; i++ )
                {
                    if (sLoai == arrDSTruong[i]) 
                    {
                        DKLoai = " AND " + arrDSTruong[i + 1] + " = ''";
                        break;
                    }
                }
            }

            //VungNV: 2015/10/27 them dieu kien theo sLNS
            String DKLNS = " AND sLNS = @sLNS";
            cmd.Parameters.AddWithValue("@sLNS", sLNS);

            SQL = String.Format(
                @"SELECT iID_MaMucLucNganSach, iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,{0},{1} 
                FROM NS_MucLucNganSach 
                WHERE {2} {3} {4}
                ORDER BY {5}", 
                MucLucNganSachModels.strDSTruong, MucLucNganSachModels.strDSDuocNhapTruongTien, 
                DK, DKLoai, DKLNS, MucLucNganSachModels.strDSTruongSapXep);
           
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
           //Lay toan bo Muc luc ngan sach-->

            DataColumn column;

            //Them Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j] == "sTenCongTrinh" || arrDSTruongTien[j] == "iID_MaCapPhatChiTiet")
                {
                    column = new DataColumn(arrDSTruongTien[j], typeof(String));
                    column.AllowDBNull = true;
                    vR.Columns.Add(column);
                }
            }

            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j] == "rTongSo" ||
                   (arrDSTruongTien[j] != "sTenCongTrinh" && arrDSTruongTien[j] != "iID_MaCapPhatChiTiet"))
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
            }

            //Them cot duyet
            column = new DataColumn("bDongY", typeof(Boolean));
            column.AllowDBNull = true;
            vR.Columns.Add(column);
            column = new DataColumn("sLyDo", typeof(String));
            column.AllowDBNull = true;
            vR.Columns.Add(column);

            cmd = new SqlCommand();

            //Lấy dữ liệu từ bảng CP_ChungTuChiTiet
            DK = "iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat";
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            SQL = String.Format("SELECT * FROM CP_CapPhatChiTiet WHERE {0} ORDER BY sXauNoiMa", DK);
            cmd.CommandText = SQL;

            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            
            int cs0 = 0;

            for (int i = 0; i < vR.Rows.Count; i++)
            {
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
                        for (int k = 0; k < vR.Columns.Count - 1; k++)
                        {
                            vR.Rows[i][k] = dtChungTuChiTiet.Rows[j][vR.Columns[k].ColumnName];
                        }
                        break;
                        cs0 = j;
                    }
                }

            }

            DienTruongDaCap(RChungTu, vR);
            cmd.Dispose();

            dtChungTuChiTiet.Dispose();
            dtChungTu.Dispose();
            return vR;
        }
        /// <summary>
        /// Get_dtCapPhatChiTiet_CapThu
        /// Hàm nằm ngoài scope phân hệ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        public static DataTable Get_dtCapPhatChiTiet_CapThu(String iID_MaCapPhat)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd;

            //<--Lay toan bo Muc luc ngan sach
            cmd = new SqlCommand();
            DK = "iTrangThai=1 ";
            String[] arrMaCapPhat = iID_MaCapPhat.Split(',');
            if (arrMaCapPhat.Length > 0)
            {
                DK += " AND ( ";
                for (int i = 0; i < arrMaCapPhat.Length; i++)
                {
                    DK += " iID_MaCapPhat=@iID_MaCapPhat" + i;
                    if (i < arrMaCapPhat.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaCapPhat" + i, arrMaCapPhat[i]);
                }
                DK += " ) ";
            }
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            SQL = String.Format("SELECT * FROM CP_CapPhatChiTiet WHERE {0} ORDER BY sXauNoiMa", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);

            return vR;
        }
        /// <summary>
        /// Check_ChungTuCapThu
        /// Hàm nằm ngoài scope phân hệ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        public static Boolean Check_ChungTuCapThu(String iID_MaCapPhat)
        {
            Boolean vR = false;
            String SQL, DK = "";
            DK = "iTrangThai=1 ";
            SqlCommand cmd = new SqlCommand(); ;
            String[] arrMaCapPhat = iID_MaCapPhat.Split(',');
            if (arrMaCapPhat.Length > 0)
            {
                DK += " AND ( ";
                for (int i = 0; i < arrMaCapPhat.Length; i++)
                {
                    DK += " iID_MaCapPhat=@iID_MaCapPhat" + i;
                    if (i < arrMaCapPhat.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaCapPhat" + i, arrMaCapPhat[i]);
                }
                DK += " ) ";
            }

            //<--Lay toan bo Muc luc ngan sach


            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            SQL = String.Format("SELECT * FROM KTTG_ChungTuChiTietCapThu WHERE {0} ", DK);
            cmd.CommandText = SQL;

            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            return vR;
        }
        /// <summary>
        /// Hàm nằm ngoài scope phân hệ
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        /// <param name="iID_MaTinhChatThu"></param>
        /// <param name="chkTatCa"></param>
        /// <returns></returns>
        public static DataTable GetDanhSachCapPhat_CapThu(String iNamLamViec, String iID_MaDonVi, String TuNgay, String DenNgay, String iID_MaTinhChatThu, String chkTatCa)
        {
            DataTable vR;
            String DK = "", DKNgayCapPhat = "";
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (!String.IsNullOrEmpty(TuNgay))
            {
                DKNgayCapPhat += "  AND dNgayCapPhat>=@TuNgay";
                cmd.Parameters.AddWithValue("@TuNgay", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (!String.IsNullOrEmpty(DenNgay))
            {
                DKNgayCapPhat += " AND dNgayCapPhat<=@DenNgay";
                cmd.Parameters.AddWithValue("@DenNgay", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (!String.IsNullOrEmpty(iID_MaTinhChatThu))
            {
                DK += " AND iID_MaTinhChatCapThu=@iID_MaTinhChatThu";
                cmd.Parameters.AddWithValue("@iID_MaTinhChatThu", iID_MaTinhChatThu);
            }
            if (chkTatCa != "on")
            {
                DK += " AND (iID_MaCapPhat NOT IN (SELECT DISTINCT iID_MaCapPhat FROM KTTG_ChungTuChiTietCapThu WHERE iTrangThai=1) OR iID_MaCapPhat IN (SELECT DISTINCT iID_MaCapPhat FROM KTTG_ChungTuChiTietCapThu WHERE iTrangThai=1 AND bDuyet=0)) ";
            }
            String SQL = "";
            SQL = String.Format(@"SELECT DISTINCT TENHT,CP_CapPhat.iID_MaCapPhat FROM (
                SELECT iID_MaCapPhat, sTienToChungTu + '' + Convert(nvarchar, iSoCapPhat) + ' - ' + sNoiDung AS TENHT 
                 FROM CP_CapPhat WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {1} ) as CP_CapPhat
                INNER JOIN
                (
                SELECT iID_MaCapPhat FROM CP_CapPhatChiTiet WHERE iTrangThai=1  {0} ) as CP_CapPhatChiTiet
                ON CP_CapPhat.iID_MaCapPhat=CP_CapPhatChiTiet.iID_MaCapPhat
                ORDER BY TENHT DESC
                SELECT * FROM CP_CapPhat", DK, DKNgayCapPhat);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// HungPX QUP
        /// Hàm cập nhật chứng từ chi tiết sau khi cập nhật chứng từ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        public static void DongBoChungTuChiTiet(string iID_MaCapPhat)
        {
            DataTable dtChungTu = CapPhat_ChungTuModels.LayToanBoThongTinChungTu(iID_MaCapPhat);

            String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]));

            SqlCommand cmd;
            String SQL = "UPDATE CP_CapPhatChiTiet SET iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat, iID_MaPhongBan=@iID_MaPhongBan, iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, " +
                    "iNamLamViec=@iNamLamViec,iID_MaNguonNganSach=@iID_MaNguonNganSach,iID_MaNamNganSach=@iID_MaNamNganSach,bChiNganSach=@bChiNganSach, " +
                    "iID_MaDonVi=@iID_MaDonVi, sTenDonVi=@sTenDonVi " +
                    "WHERE iID_MaCapPhat=@iID_MaCapPhat";

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", dtChungTu.Rows[0]["iDM_MaLoaiCapPhat"]);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            cmd.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
            cmd.Parameters.AddWithValue("@sTenDonVi", strTenDonVi);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            dtChungTu.Dispose();
        }
    }
}