using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Models
{
    public class Luong_ReportModel
    {
        /// <summary>
        /// Danh sách đơn vị có dữ liệu 
        /// </summary>
        /// <param name="NamBangLuong">Năm bảng lương</param>
        /// <param name="ThangBangLuong">Tháng bảng lương</param>
        /// <param name="iID_MaTrangThaiDuyet">Mã trạng thái duyệt</param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String NamBangLuong, String ThangBangLuong, String iID_MaTrangThaiDuyet)
        {
            String DKTrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "2")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            if (iID_MaTrangThaiDuyet == "-100")
            {
                DKTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=-100";
            }
            String SQLDonVi = String.Format(@"SELECT  DISTINCT iID_MaDonVi,sTenDonVi,sTenDonVi as TenHT 
                                              FROM l_BangLuongChiTiet
                                              WHERE iNamBangLuong=@NamBangLuong AND
                                                    iThangBangLuong=@ThangBangLuong AND 
                                                    iTrangThai=1 {0}", DKTrangThaiDuyet);
            SqlCommand cmd = new SqlCommand(SQLDonVi);
            cmd.Parameters.AddWithValue("@NamBangLuong", NamBangLuong);
            cmd.Parameters.AddWithValue("ThangBangLuong", ThangBangLuong);
            if (iID_MaTrangThaiDuyet != "-1")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong));
            }
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow r = dtDonVi.NewRow();
            r[2] = "-- Chọn đơn vị --";
            dtDonVi.Rows.InsertAt(r, 0);
           // dtDonVi.Dispose();
            return dtDonVi;
        }
        public static DataTable DachSachTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet");
            dt.Columns.Add("sTen");
            DataRow r = dt.NewRow();
            r[0] = "-100";
            r[1] = "--Chọn trạng thái duyệt--";
            dt.Rows.Add(r);
            DataRow r1 = dt.NewRow();
            r1[0] = "1";
            r1[1] = "Tất cả";
            dt.Rows.Add(r1);
            DataRow r2 = dt.NewRow();
            r2[0] = "2";
            r2[1] = "Đã duyệt";
            dt.Rows.Add(r2);
            dt.Dispose();
            return dt;
        }
    }
}