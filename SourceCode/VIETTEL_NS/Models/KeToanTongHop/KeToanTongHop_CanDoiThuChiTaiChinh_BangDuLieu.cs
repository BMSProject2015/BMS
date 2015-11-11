using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;

namespace VIETTEL.Models
{
    /// <summary>
    /// Lớp PhanBo_ChiTieu_BangDuLieu cho phần bảng của Phân bổ chỉ tiêu
    /// </summary>
    public class KeToanTongHop_CanDoiThuChiTaiChinh_BangDuLieu : BangDuLieu
    {
        public String strDSMaND = "";

        /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KeToanTongHop_CanDoiThuChiTaiChinh_BangDuLieu(String iNam, String iQuy, String iLoaiThuChi, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iNam;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            cmd = new SqlCommand();
            _dtBang = new DataTable();
            _CoCotDuyet = false;
            _DuocSuaChiTiet = true;
            _ChiDoc = false;
            _dtChiTiet = KeToanTongHopModels.Get_dtCanDoiThuChiTaiChinh(iNam, iQuy, iLoaiThuChi, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu();
        }
       
         //<summary>
         //Hàm điền dữ liệu
         //Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
         //</summary>
        protected void DienDuLieu()
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrCotDuocPhepNhap();
                CapNhap_arrType_Rieng();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
            }
        }

        /// <summary>
        /// Hàm cập nhập vào tham số _arrDSMaHang
        /// </summary>
        protected void CapNhapDanhSachMaHang()
        {
            _arrDSMaHang = new List<string>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                String MaHang = Convert.ToString(R["iID_MaThuChi"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Không có cột Fixed
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();
            _arrAlign = new List<string>();
            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng n  có
        /// <param name="iLoai"></param>
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            //String[] arrDSTruong = "sKyHieu,sKyHieu_Cha,sNoiDung,iLoaiThuChi,sTenTaiKhoan_No_Thu,sTenTaiKhoanGiaiThich_No_Thu,sTenTaiKhoan_Co_Thu,sTenTaiKhoanGiaiThich_Co_Thu,sTenTaiKhoan_No_Thu_NgoaiTe,sTenTaiKhoanGiaiThich_No_Thu_NgoaiTe,sTenTaiKhoan_Co_Thu_NgoaiTe,sTenTaiKhoanGiaiThich_Co_Thu_NgoaiTe".Split(',');
            //String[] arrDSTruongTieuDe = "Mã thu/chi<span style=\"color: Red;\">*</span>,Mã cha thu/chi,Nội dung thu/chi<span style=\"color: Red;\">*</span>,Loại thu/chi<span style=\"color: Red;\">*</span>,Tài khoản nợ,Chi tiết Tài khoản nợ,Tài khoản có,Chi tiết Tài khoản có,Tài khoản nợ,Chi tiết Tài khoản nợ,Tài khoản có,Chi tiết Tài khoản có".Split(',');
            //String[] arrDSTruongDoRong = "100,100,400,80,250,250,250,250,250,250,250,250".Split(',');
            //String[] arrDSTruongAlign = "left,left,left,left,left,left,left,left,left,left,left,right".Split(',');
            //String[] arrDSNhom_TieuDe = ",,,,TIỀN VIỆT,TIỀN VIỆT,TIỀN VIỆT,TIỀN VIỆT,TỔNG CỘNG,TỔNG CỘNG,TỔNG CỘNG,TỔNG CỘNG".Split(',');
            //String[] arrDSNhom_SoCot = "1,1,1,1,4,4,4,4,4,4,4,4".Split(',');
            String[] arrDSTruong = "sKyHieu,sKyHieu_Cha,sNoiDung,iLoaiThuChi,bHienThi,sTenTaiKhoan_NgoaiTe,bCoTKGT_NgoaiTe,bTuDongTinh,sTenTaiKhoan_Tong,bCoTKGT_Tong,bTuTinhTong".Split(',');
            String[] arrDSTruongTieuDe = "Mã T/C<span style=\"color: Red;\">*</span>,Mã T/C cha,Nội dung thu/chi<span style=\"color: Red;\">*</span>,Loại T/C<span style=\"color: Red;\">*</span>,H.thị CĐTC,Tài khoản/TKGT,Có TKGT,Tự tính,Tài khoản/TKGT,Có TKGT,Tự tính".Split(',');
            String[] arrDSTruongDoRong = "60,80,300,60,60,300,60,60,300,60,60".Split(',');
            String[] arrDSTruongAlign = "left,left,left,left,center,left,left,left,left,left,left".Split(',');
            String[] arrDSNhom_TieuDe = ",,,,,NGOẠI TỆ,NGOẠI TỆ,NGOẠI TỆ,TỔNG CỘNG,TỔNG CỘNG,TỔNG CỘNG".Split(',');
            String[] arrDSNhom_SoCot = "1,1,1,1,1,3,3,3,3,3,3".Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                _arrAlign.Add(arrDSTruongAlign[j]);
            }

            //Them cot duyet
            if (CoCotDuyet)
            {
                //Cột đồng ý
                _arrDSMaCot.Add("bDongY");
                if (_ChiDoc)
                {
                    _arrTieuDe.Add("<div class='check'></div>");
                }
                else
                {
                    _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
                }
                _arrWidth.Add(20);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
                //Cột Lý do
                _arrDSMaCot.Add("sLyDo");
                _arrTieuDe.Add("Nhận xét");
                _arrWidth.Add(200);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
            //String strDSTruong = "iID_MaTaiKhoan_No_Thu,iID_MaTaiKhoan_Co_Thu," +
            //                     "iID_MaTaiKhoan_No_Thu_NgoaiTe,iID_MaTaiKhoan_Co_Thu_NgoaiTe,iID_MaTaiKhoanGiaiThich_No_Thu,iID_MaTaiKhoanGiaiThich_Co_Thu," +
            //                     "iID_MaTaiKhoanGiaiThich_No_Thu_NgoaiTe,iID_MaTaiKhoanGiaiThich_Co_Thu_NgoaiTe,iSTT";
            String strDSTruong = "iSTT";
            String[] arrDSTruong = strDSTruong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruong[j]);
                _arrWidth.Add(100);
                _arrHienThiCot.Add(false);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
        }

        /// <summary>
        /// Hàm xác định các cột có được Edit hay không
        /// </summary>
        protected void CapNhap_arrCotDuocPhepNhap()
        {
            _arrCotDuocPhepNhap = new List<Boolean>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                Boolean okCotDuocPhepNhap = true;
                //Xac dinh o chi doc
                if(_ChiDoc==false)
                {
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet == false)
                        {
                            okCotDuocPhepNhap = false;
                        }
                    }
                    else
                    {
                        if (_DuocSuaChiTiet == false)
                        {
                            okCotDuocPhepNhap = false;
                        }
                    }
                }
                _arrCotDuocPhepNhap.Add(okCotDuocPhepNhap);
            }
        }

        /// <summary>
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhap_arrEdit()
        {
            _arrEdit = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = false;
                    if (okOChiDoc)
                    {
                        _arrEdit[i].Add("");
                    }
                    else
                    {
                        _arrEdit[i].Add("1");
                    }
                }
            }
        }
        /// <summary>
        /// Hàm xác định hàng cha, con
        /// </summary>
        protected void CapNhap_arrLaHangCha()
        {
            //Xác định hàng là hàng cha, con
            _arrCSCha = new List<int>();
            _arrLaHangCha = new List<bool>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                //Xac dinh hang nay co phai la hang cha khong?
                _arrLaHangCha.Add(Convert.ToBoolean(R["bLaHangCha"]));
                int CSCha = -1;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Convert.ToString(R["iID_MaThuChi_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaThuChi"]))
                    {
                        CSCha = j;
                        break;
                    }
                }
                _arrCSCha.Add(CSCha);
            }
        }
        /// <summary>
        /// Hàm cập nhập kiểu nhập cho các cột
        ///     - Cột có prefix 'b': kiểu '2' (checkbox)
        ///     - Cột có prefix 'r' hoặc 'i' (trừ 'iID'): kiểu '1' (textbox number)
        ///     - Ngược lại: kiểu '0' (textbox)
        /// </summary>
        protected void CapNhap_arrType_Rieng()
        {
            //Xac dinh kieu truong nhap du lieu
            _arrType = new List<string>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    _arrType.Add("2");
                }
                else if (_arrDSMaCot[j] == "sSoChungTu" || _arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    if (_arrDSMaCot[j].StartsWith("iNgay") || _arrDSMaCot[j].StartsWith("iThang") || _arrDSMaCot[j].StartsWith("iLoaiThuChi"))
                    {
                        //Nhap Kieu autocomplete
                        _arrType.Add("3");
                    }
                    else
                    {
                        //Nhap Kieu so
                        _arrType.Add("1");
                    }

                }
                //else if (_arrDSMaCot[j].StartsWith("sTenTaiKhoan") ||
                //         _arrDSMaCot[j].StartsWith("sTenTaiKhoanGiaiThich") ||
                //         _arrDSMaCot[j].StartsWith("sTenPhongBan") ||
                //         _arrDSMaCot[j].StartsWith("sTenDonVi"))
                //{
                //    //Nhap Kieu autocomplete
                //    _arrType.Add("3");
                //}
                else
                {
                    //Nhap kieu xau
                    _arrType.Add("0");
                }
            }
        }

        /// <summary>
        /// Hàm cập nhập mảng dữ liệu
        /// </summary>
        protected void CapNhap_arrDuLieu()
        {
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    //Xac dinh gia tri
                    _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                }
                if (strDSMaND.IndexOf(String.Format("#|{0}##", R["sID_MaNguoiDungTao"])) == -1)
                {
                    strDSMaND += String.Format("#|{0}##{0}", R["sID_MaNguoiDungTao"]);
                }
            }
        }
    }
}