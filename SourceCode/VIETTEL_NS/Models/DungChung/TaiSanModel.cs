using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;

namespace VIETTEL.Models
{
    public class TaiSanModel
    {/// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="TKNo"></param>
        /// <param name="TKCo"></param>
        /// <param name="LoaiTT"></param>
        /// <param name="LoaiNS"></param>
        /// <param name="page"></param>
        public TaiSanModel(String sKyHieu, String sTen,String LoaiTS, String DV, String page)
        {
            this.sKyHieu = sKyHieu;
            this.sTen = sTen;
            this.LoaiTS = LoaiTS;
            this.DV = DV;
            this.page = page;
        }
        public String sKyHieu { get; set; }
        public String sTen { get; set; }
        public String LoaiTS { get; set; }
        public String DV { get; set; }
        public String page { get; set; }

        /// <summary>
        /// Danh sách thông tri
        /// </summary>
        /// <param name="sTaiKhoan"></param>
        /// <param name="sKyHieu"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable getList(String KyHieu = "", String Ten = "", String LoaiTS= "", String DV= "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT TS.iID_MaTaiSan, TS.sKyHieu, TS.sTen, TS.sMoTa, TS.iID_MaDanhMuc, TS.iID_MaLoaiTaiSan, " +
                " L.sKyHieu + ' - ' + L.sTen AS LoaiTS FROM KTCS_LoaiTaiSan AS L, KTCS_TaiSan AS TS WHERE L.iID_MaLoaiTaiSan = TS.iID_MaLoaiTaiSan";
            if (String.IsNullOrEmpty(KyHieu) == false && KyHieu != "")
            {
                SQL += " AND TS.sKyHieu= @KyHieu";
                cmd.Parameters.AddWithValue("@KyHieu", KyHieu);
            }
            if (String.IsNullOrEmpty(Ten) == false && Ten != "")
            {
                SQL += " AND TS.sTen= @Ten";
                cmd.Parameters.AddWithValue("@Ten", Ten);
            }
            if (String.IsNullOrEmpty(LoaiTS) == false && LoaiTS != "" && LoaiTS != Convert.ToString(Guid.Empty))
            {
                SQL += " AND TS.iID_MaLoaiTaiSan= @LoaiTS";
                cmd.Parameters.AddWithValue("@LoaiTS", LoaiTS);
            }
            if (String.IsNullOrEmpty(DV) == false && DV != "" && DV != Convert.ToString(Guid.Empty))
            {
                SQL += " AND TS.iID_MaDanhMuc= @iID_MaDanhMuc";
                cmd.Parameters.AddWithValue("@iID_MaDanhMuc", DV);
            }
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "TS.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Đếm số bản ghi 
        /// </summary>
        /// <param name="LoaiNS"></param>
        /// <param name="LoaiTT"></param>
        /// <param name="TKCo"></param>
        /// <param name="TKNo"></param>
        /// <returns></returns>
        public static int getList_Count(String KyHieu = "", String Ten = "", String LoaiTS = "", String DV = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = " 1=1";        
            if (String.IsNullOrEmpty(KyHieu) == false && KyHieu != "")
            {
                DK += " AND sKyHieu= @KyHieu";
                cmd.Parameters.AddWithValue("@KyHieu", KyHieu);
            }
            if (String.IsNullOrEmpty(Ten) == false && Ten != "")
            {
                DK += " AND sTen= @Ten";
                cmd.Parameters.AddWithValue("@Ten", Ten);
            }
            if (String.IsNullOrEmpty(LoaiTS) == false && LoaiTS != "" && LoaiTS != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaLoaiTaiSan= @LoaiTS";
                cmd.Parameters.AddWithValue("@LoaiTS", LoaiTS);
            }
            if (String.IsNullOrEmpty(DV) == false && DV != "" && DV != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaDanhMuc= @iID_MaDanhMuc";
                cmd.Parameters.AddWithValue("@iID_MaDanhMuc", DV);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM KTCS_TaiSan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chi tiết thông tri
        /// </summary>
        /// <param name="iID_MaThongTri"></param>
        /// <returns></returns>
        public static DataTable getChiTiet(String iID_MaTaiSan)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTCS_TaiSan WHERE iTrangThai=1 AND iID_MaTaiSan=@iID_MaTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

       
    }
}