using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class BangLuongChiTiet_PhuCapModels
    {
        public static void CapNhapLenBangLuongChiTiet(String iID_MaBangLuongChiTiet, String UserID, String IPSua)
        {
            DataTable dtChiTiet = LuongModels.Get_ChiTietBangLuongChiTiet(iID_MaBangLuongChiTiet);
            DataTable dtChiTiet_PhuCap = LuongModels.Get_dtLuongPhuCap(iID_MaBangLuongChiTiet);
            Bang bang = new Bang("L_BangLuongChiTiet");
            bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
            bang.DuLieuMoi = false;
            bang.MaNguoiDungSua = UserID;
            bang.IPSua = IPSua;
            for (int j = 0; j < dtChiTiet.Columns.Count; j++)
            {
                String TenTruong = dtChiTiet.Columns[j].ColumnName;
                if (TenTruong.StartsWith("rPhuCap_"))
                {
                    double rHeSo = 0;
                    String sMoTa = "";
                    double rTongTien = 0;
                    for (int i = 0; i < dtChiTiet_PhuCap.Rows.Count; i++)
                    {
                        if (Convert.ToString(dtChiTiet_PhuCap.Rows[i]["sMaTruongHeSo_BangLuong"]) == dtChiTiet.Columns[j].ColumnName)
                        {
                            rHeSo = Convert.ToDouble(dtChiTiet_PhuCap.Rows[i]["rHeSo"]);
                        }
                        if (Convert.ToString(dtChiTiet_PhuCap.Rows[i]["sMaTruongSoTien_BangLuong"]) == dtChiTiet.Columns[j].ColumnName)
                        {
                            rTongTien += Convert.ToDouble(dtChiTiet_PhuCap.Rows[i]["rSoTien"]);
                            if (sMoTa != "") sMoTa += ",";
                            sMoTa += Convert.ToString(dtChiTiet_PhuCap.Rows[i]["iID_MaPhuCap"]);
                        }
                    }
                    if (TenTruong.EndsWith("_HeSo"))
                    {
                        bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, rHeSo);
                    }
                    else
                    {
                        bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, rTongTien);
                        bang.CmdParams.Parameters.AddWithValue("@s" + TenTruong.Substring(1) + "_MoTa", sMoTa);
                    }
                }
            }
            bang.Save();

            dtChiTiet.Dispose();
            dtChiTiet_PhuCap.Dispose();
        }
    }
}