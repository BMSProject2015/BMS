﻿using System;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Abstract;
using DomainModel;
namespace VIETTEL.Models
{
    public class CapPhatModels
    {
        public static int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeCapPhat;
        public static void ChuyenNamSau(String sMaND, String sTenBangChungTu = "CP_CapPhat", String sTenBangChiTiet = "CP_CapPhatChiTiet")
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sMaND);
            int iNamLamViec = Convert.ToInt16(dtCauHinh.Rows[0]["iNamLamViec"]);

            String SQL = String.Format("SELECT * FROM {0} WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND  iID_MaNamNganSach=3 AND iNamLamViec=@iNamLamViec", sTenBangChungTu,LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat));
            DataTable dtChungTu;
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            dtChungTu = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            Bang bang = new Bang(sTenBangChungTu);
            Bang bangChiTiet = new Bang(sTenBangChiTiet);
            String iID_MaCapPhat, iID_MaCapPhatatGoc;
            DataTable dtChiTiet;
            for (int i = 0; i < dtChungTu.Rows.Count; i++)
            {

                iID_MaCapPhatatGoc = Convert.ToString(dtChungTu.Rows[i]["iID_MaCapPhat"]);
                SQL = String.Format("SELECT * FROM {0} WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND  iID_MaCapPhat=@iID_MaCapPhat", sTenBangChiTiet,LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat));
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhatatGoc);
                dtChiTiet = Connection.GetDataTable(cmd);
                cmd.Dispose();
                //add du liệu các cột vào param     
                for (int j = 0; j < dtChungTu.Columns.Count; j++)
                {
                    String TenCot = dtChungTu.Columns[j].ColumnName;
                    String GiaTri = Convert.ToString(dtChungTu.Rows[i][j]);
                    if (TenCot == "iID_MaNamNganSach")
                    {
                        GiaTri = "1";//chuyển mã năm ngân sách thành 1 (Năm nay)
                    }
                    if (TenCot == "iNamLamViec")
                    {
                        GiaTri = Convert.ToString(iNamLamViec + 1);
                    }
                        bang.CmdParams.Parameters.AddWithValue("@" + TenCot, GiaTri);
                }
                iID_MaCapPhat= Guid.NewGuid().ToString();
                //Remove trường tự tăng iSoCapPhat
                bang.CmdParams.Parameters.RemoveAt(bang.CmdParams.Parameters.IndexOf("@iSoCapPhat"));
                bang.CmdParams.Parameters["@" + bang.TruongKhoa].Value = iID_MaCapPhat;
                bang.Save();

                for (int h = 0; h < dtChiTiet.Rows.Count; h++)
                {
                    for (int j = 1; j < dtChiTiet.Columns.Count; j++)
                    {
                        Type _type = dtChiTiet.Columns[j].DataType;
                        String TenCot = dtChiTiet.Columns[j].ColumnName;
                        Object GiaTri = dtChiTiet.Rows[h][j];
                        
                        if (TenCot == "iID_MaNamNganSach")
                        {
                            GiaTri = "1";//chuyển mã năm ngân sách thành 1 (Năm nay)
                        }
                        if (TenCot == "iNamLamViec")
                        {
                            GiaTri = iNamLamViec + 1;
                        }
                        if (TenCot == "iID_MaCapPhat")
                        {
                            GiaTri = iID_MaCapPhat;
                        }
                        if (h == 0)
                        {
                            bangChiTiet.CmdParams.Parameters.AddWithValue("@" + TenCot, GiaTri);                          
                        }
                        else
                        {
                            bangChiTiet.CmdParams.Parameters["@" + TenCot].Value = GiaTri;
                        }

                    }                                    
                    bangChiTiet.Save();
                }
            }
        }
        
    }
}