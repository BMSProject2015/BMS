using System;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Abstract;
using DomainModel;
namespace VIETTEL.Models.CapPhat
{
    public class CapPhat_ReportModels
    {
        public static int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeCapPhat;

        /// <summary>
        /// Lấy dữ liệu danh sách chứng từ thông tri cấp phát
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Mã loại ngân sách</param>
        /// <param name="iNamCapPhat">Năm cấp phát</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="LoaiTongHop">Loại báo cáo: chi tiết hay tổng hợp</param>
        /// <returns></returns>
        /// VungNV: 2015/11/11
        public static DataTable rptCapPhat_ThongTri(String MaND, String sLNS, String iNamCapPhat, String iID_MaDonVi, String LoaiTongHop)
        {
            String DKDonVi = "";
            String DKPhongBan = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            //Báo cáo chi tiết từng đơn vị
            if (LoaiTongHop == "ChiTiet")
            {
                if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "-1")
                {
                    DK += " AND iID_MaDonVi=@iID_MaDonVi";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            //Báo cáo tổng hợp các đơn vị
            else
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();

                String[] arrDonVi = iID_MaDonVi.Split(',');

                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DK += "iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DK += " OR ";
                }

                if (!String.IsNullOrEmpty(DK))
                    DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
            }

            //Nếu là báo cáo tổng hợp thì lấy thêm mã đơn vị và tên đơn vị
            string strDonVi = "";

            //Báo cáo tổng hợp các đơn vị
            if (LoaiTongHop == "TongHop")
            {
                strDonVi = " ,iID_MaDonVi, sTenDonVi";
            }
            
            SQL = String.Format(@"
                    SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                            SUBSTRING(sLNS,1,3) as sLNS3,
                            SUBSTRING(sLNS,1,5) as sLNS5,
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa {4}
                            ,SUM(rTuChi) as rTuChi
                     FROM CP_CapPhatChiTiet
                     WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                            AND CONVERT(VARCHAR(10), dNgayCapPhat, 103) = '{3}' 
                     GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa {4}
                     HAVING SUM(rTuChi)<>0 ", DK, DKDonVi, DKPhongBan, iNamCapPhat, strDonVi);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        

        //VungNV: 2015/09/28 lấy danh sách phòng ban
        public static DataTable LayDSPhongBan(String MaND)
        {
            SqlCommand cmd = new SqlCommand();
            String DKPhongBan="";
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            
            String SQL = String.Format(
                        @"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
                        FROM CP_CapPhatChiTiet
                        WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} 
                        ", DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["sTenPhongBan"] = "--Chọn tất cả các B--";
            dt.Rows.InsertAt(dr, 0);

            return dt;
        }

       /// <summary>
       /// Lấy danh sách các đợt cấp phát
       /// </summary>
       /// <param name="MaND">Mã người dùng</param>
       /// <returns></returns>
        public static DataTable LayDotCapPhat(String MaND) 
        {
            SqlCommand cmd = new SqlCommand();
            String DKPhongBan="";
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            String SQL = String.Format(
                @"SELECT DISTINCT CONVERT(VARCHAR(10), CP_CapPhat.dNgayCapPhat, 103) AS iNamCapPhat
                        ,CP_CapPhat.dNgayCapPhat
                FROM CP_CapPhat JOIN CP_CapPhatChiTiet 
                        ON CP_CapPhat.iID_MaCapPhat = CP_CapPhatChiTiet.iID_MaCapPhat
                WHERE CP_CapPhatChiTiet.iTrangThai=1 
                        AND CP_CapPhatChiTiet.iNamLamViec=@iNamLamViec {0}
                ORDER BY CP_CapPhat.dNgayCapPhat DESC", DKPhongBan);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        /// <summary>
        /// Lấy giá trị của loại cấp phát
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <returns></returns>
        /// VungNV: 2015/09/30
        public static String LayLoaiCapPhat(String MaND)
        {
            String sLoaiCapPhat = "";
            String sTen = "rptCapPhat_ThongTri_LoaiCapPhat";
            String SQL = "";
            
            SQL = String.Format(
                @"SELECT sGhiChu 
                  FROM CP_GhiChu 
                  WHERE sTen=@sTen AND sID_MaNguoiDung=@sID_MaNguoiDung"
               );

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@sTen", sTen);
            sLoaiCapPhat = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            return sLoaiCapPhat;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật loại cấp phát
        /// </summary>
        /// <param name="LoaiCapPhat">Nội dung cấp phát</param>
        /// <param name="MaND">Mã người dùng</param>
        /// VungNV: 2015/11/11
        public static void CapNhatLoaiCapPhat(String LoaiCapPhat, String MaND)
        {
            String sTen = "rptCapPhat_ThongTri_LoaiCapPhat";
            String SQL = "";
            SQL = String.Format(
                @"IF NOT EXISTS(
	                SELECT sGhiChu 
	                FROM CP_GhiChu 
	                WHERE sTen =  @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung
	            )
            INSERT INTO CP_GhiChu(sTen, sID_MaNguoiDung, sGhiChu) 
		            VALUES( @sTen,@sID_MaNguoiDung, @sGhiChu)
            ELSE 
                UPDATE CP_GhiChu 
                SET sGhiChu=@sGhiChu 
                WHERE  sTen = @sTen AND	 sID_MaNguoiDung = @sID_MaNguoiDung");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@sGhiChu", LoaiCapPhat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

        }

        /// <summary>
        /// Lấy ghi chú của đơn vị
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <returns></returns>
        /// VungNV: 2015/11/11
        public static String LayGhiChu( String MaND, String iID_MaDonVi)
        {
            String sGhiChu = "";
            String sTen = "rptCapPhat_ThongTri";
            String SQL = 
                string.Format(
                      @"SELECT sGhiChu 
                        FROM CP_GhiChu 
                        WHERE sTen=@sTen AND sID_MaNguoiDung=@sID_MaNguoiDung AND iID_MaDonVi=@iID_MaDonVi");
            
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            
            sGhiChu = Connection.GetValueString(cmd, "");

            return sGhiChu;

        }

        /// <summary>
        /// Thêm mới hoặc cập nhật ghi chú cho đơn vị
        /// </summary>
        /// <param name="sGhiChu">Nội dung ghi chú</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// VungNV: 2015/11/11
        public static void CapNhatGhiChu(String sGhiChu, String MaND, String iID_MaDonVi)
        {
            String sTen = "rptCapPhat_ThongTri";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";

            SQL = String.Format(
                @"IF NOT EXISTS(
		            SELECT sGhiChu 
		            FROM CP_GhiChu 
		            WHERE sTen = @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung AND iID_MaDonVi = @iID_MaDonVi
	            )
            INSERT INTO CP_GhiChu(sTen, sID_MaNguoiDung, iID_MaDonVi, sGhiChu) 
		            VALUES(@sTen, @sID_MaNguoiDung, @iID_MaDonVi, @sGhiChu)
            ELSE 
	            UPDATE CP_GhiChu 
	            SET sGhiChu=@sGhiChu 
	            WHERE  sTen = @sTen AND	 sID_MaNguoiDung = @sID_MaNguoiDung AND iID_MaDonVi =@iID_MaDonVi");

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            sGhiChu = sGhiChu.Replace("^", "&#10;");
            cmd.Parameters.AddWithValue("@sGhiChu", sGhiChu);
            Connection.UpdateDatabase(cmd);

            cmd.Dispose();

        }
    }
}