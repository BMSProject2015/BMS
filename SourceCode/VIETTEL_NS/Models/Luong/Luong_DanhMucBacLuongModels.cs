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
    public class Luong_DanhMucBacLuongModels
    {
        public static String DanhSachQuanHam = "rBinhNhi,rBinhNhat,rHaSi,rTrungSi,rThuongSi,rThieuUy,rTrungUy,rThuongUy,rDaiUy,rThieuTa,rTrungTa,rThuongTa,rDaiTa,rTuong,rQNCN,rCNVQPCT,rQNVQPHD,rTSQ";
        public static String DanhSachTenQuanHam = "Binh nhì,Binh nhất,Hạ sĩ,Trung sĩ,Thượng sĩ,Thiếu úy,Trung úy,Thượng úy,Đại úy,Thiếu tá,Trung tá,Thượng tá,Đại tá,Tướng,QN chuyên nghiệp,CNVQP Chính Thức,QNVQ Hợp đồng,Thiếu sinh quân";

        public static DataTable dt_QuanHam()
        {
            DataTable dt= new DataTable();
            dt.Columns.Add("sQuanHam",typeof(String));
            dt.Columns.Add("sTenQuanHam",typeof(String));
            String[] arrQuanHam=DanhSachQuanHam.Split(',');
            String[] arrTenQuanHam=DanhSachTenQuanHam.Split(',');
            DataRow R;
            for(int i=0;i<arrQuanHam.Length;i++)
            {
                R=dt.NewRow();
                R["sQuanHam"]=arrQuanHam[i];
                R["sTenQuanHam"]=arrTenQuanHam[i];
                dt.Rows.Add(R);
            }
            return dt;
        }
    }
}