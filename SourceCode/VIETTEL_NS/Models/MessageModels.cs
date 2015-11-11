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
    public class MessageModels
    {
        public const string sTuChoi = "Bạn phải nhập lý do từ chối!";
        public const string sNgayTra = "Bạn chưa chọn ngày trả!";
        public const string sNguoiTra = "Bạn chưa chọn tên người trả!";
        public const string sSoCMND = "Bạn chưa nhập số CMND!";
        public const string sTienVon = "Bạn chưa nhập số CMND!";
        public const string sTienLai = "Bạn chưa nhập tiền lãi!";
        public const string sVonLai = "Bạn phải nhập tiền vốn hoặc lãi!"; 
        
        public const string sMoiTao = "Tạo mới chứng từ";

        //Vay no
        public const string iSochungTu= "Bạn phải nhập số chứng từ!";
        public const string iSochungTuTonTai = "Số chứng từ đã tồn tại!";
        public const string iNgayChungTu = "Bạn chưa nhập ngày chứng từ!";

        public const string dNgayVay = "Nhập ngày vay!";
        public const string rLaiSuat = "Nhập lãi suất!";

        public const string rMienLai = "Nhập miễn lãi";
        public const string rVayTrongThang = "Nhập số tiền vay"; 

        public const string dHanPhaiTra = "Nhập hạn phải trả";
        public const string rThoiGianThuVon = "Nhập thời gian thu hồi vốn";

        ///
        public const string sKyHieu = "Bạn chưa nhập ký hiệu";
        public const string sTen = "Bạn chưa nhập tên"; 
        ///tai san
        public const string sKyHieuTaiSan = "Bạn phải nhập ký hiệu tài sản!";
        public const string sTenTaiSan = "Bạn phải nhập tên tài sản!";
        public const string sLoaiTS = "Bạn phải chọn loại tài sản!";
        public const string sDV = "Bạn phải chọn đơn vị tính!";
        //hố sơ cán bộ
        public const string sSoHieuCBCC = "Bạn phải nhập số hiệu CBCC";
        public const string sHoDem = "Bạn phải nhập họ đệm";
        public const string sTenCB = "Bạn phải nhập tên";
        public const string sDoiTuong = "Bạn phải chọn đối tượng";
        public const string sNoiO = "Bạn phải nhập nơi ở của cán bộ";

        public const string sPhongBan = "Bạn phải chọn đơn vị";
        public const string sTrinhDoHV = "Bạn phải chọn trình độ học vấn";
        public const string sTrinhDoChuyenMon = "Bạn phải chọn trình độ chuyên môn";
        public const string sNgayVaoCQ = "Bạn phải nhập ngày vào cơ quan";
        public const string sNgachCongChuc = "Bạn phải chọn ngạch công chức";
        public const string sBacLuong = "Bạn phải chọn bậc lương";
    }
   
}