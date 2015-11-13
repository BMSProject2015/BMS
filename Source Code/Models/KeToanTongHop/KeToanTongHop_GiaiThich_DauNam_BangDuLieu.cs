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
    public class KeToanTongHop_GiaiThich_DauNam_BangDuLieu : BangDuLieu
    {
        public String strDSMaND = "";

        /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KeToanTongHop_GiaiThich_DauNam_BangDuLieu(String iNamLamViec, String iID_MaTaiKhoan, String MaND, String IPSua)
        {
            this._iID_Ma = iNamLamViec;
            this._MaND = MaND;
            this._IPSua = IPSua;
            this._ChiDoc = false;
            this._DuocSuaChiTiet = true;
            String SQL;
            SqlCommand cmd;
            cmd = new SqlCommand();
            SQL = "SELECT * FROM KT_SoDuTaiKhoanGiaiThich WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1";
            if (String.IsNullOrEmpty(iID_MaTaiKhoan) == false && iID_MaTaiKhoan != "-1")
            {
                SQL += " AND ( iID_MaTaiKhoan_No=@iID_MaTaiKhoan OR iID_MaTaiKhoan_Co=@iID_MaTaiKhoan)";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            }
            SQL += " ORDER BY iSTT";
          
            cmd.Parameters.AddWithValue("@iNamLamViec", _iID_Ma);
            cmd.CommandText = SQL;
            _dtChiTiet = Connection.GetDataTable(cmd);
            cmd.Dispose();
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu();
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu()
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
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
                String MaHang = Convert.ToString(R["iID_MaChungTuChiTiet"]);
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

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        /// <param name="iLoai"></param>
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            
            String[] arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sNoiDung,rSoTien,sTenTaiKhoan_No,sTenTaiKhoanGiaiThich_No,sTenPhongBan_No,sTenDonVi_No,sTenTaiKhoan_Co,sTenTaiKhoanGiaiThich_Co,sTenPhongBan_Co,sTenDonVi_Co".Split(',');
            String[] arrDSTruongTieuDe = "Ngày<span style=\"color: Red;\">*</span>,Tháng<span style=\"color: Red;\">*</span>,Số ChTu<span style=\"color: Red;\">*</span>,Nội dung<span style=\"color: Red;\">*</span>,Số tiền<span style=\"color: Red;\">*</span>,Tài khoản nợ,Chi tiết TK nợ,B nợ,Đơn vị nợ,Tài khoản có,Chi tiết TK có,B có,Đơn vị có".Split(',');
           
            String[] arrDSTruongDoRong = "40,40,60,500,150,250,200,50,200,250,200,50,200".Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
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
            String strDSTruong = "iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co,iID_MaDonVi_No,iID_MaDonVi_Co,iID_MaPhongBan_No,iID_MaPhongBan_Co,iID_MaTaiKhoanGiaiThich_No,iID_MaTaiKhoanGiaiThich_Co,iSTT,sMauSac";
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
                    Boolean okOChiDoc = true;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo" || _arrDSMaCot[j] == "sID_MaNguoiDungTao")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet && _ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                        if (_arrDSMaCot[j] == "sID_MaNguoiDungTao") okOChiDoc = true;

                    }
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet &&
                                _ChiDoc == false &&
                                okHangChiDoc == false)
                        {
                            //Phuonglt yêu cầu tài khoản cấp trợ lý tổng hợp thì được sửa tất cả dữ liệu của tài khoản cấp trợ lý phòng ban.
                            // Tài khoản cấp trợ lý phòng ban thì dữ liệu của tài khoản nào sửa dữ liệu của tài khoản đó.
                            if (Convert.ToString(R["sID_MaNguoiDungTao"]) == _MaND || LuongCongViecModel.KiemTra_TroLyTongHop(_MaND))
                            {
                                okOChiDoc = false;
                            }
                        }
                    }
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
                else if (_arrDSMaCot[j] =="sSoChungTu" || _arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    if (_arrDSMaCot[j].StartsWith("iNgay") || _arrDSMaCot[j].StartsWith("iThang"))
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
                else if (_arrDSMaCot[j].StartsWith("sTenTaiKhoan") ||
                         _arrDSMaCot[j].StartsWith("sTenTaiKhoanGiaiThich") ||
                         _arrDSMaCot[j].StartsWith("sTenPhongBan") ||
                         _arrDSMaCot[j].StartsWith("sTenDonVi"))
                {
                    //Nhap Kieu autocomplete
                    _arrType.Add("3");
                }
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
                    if (_arrDSMaCot[j] == "sMauSac")
                    {
                        String sMauSac = "";
                        String sLyDo = Convert.ToString(R["sLyDo"]);
                        Boolean bDongY = Convert.ToBoolean(R["bDongY"]);
                        if (bDongY)
                        {
                            sMauSac = sMauSac_DongY;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(sLyDo) == false)
                            {
                                sMauSac = sMauSac_TuChoi;
                            }
                        }
                        _arrDuLieu[i].Add(sMauSac);
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                }
                if (strDSMaND.IndexOf(String.Format("#|{0}##", R["sID_MaNguoiDungTao"])) == -1)
                {
                    strDSMaND += String.Format("#|{0}##{0}", R["sID_MaNguoiDungTao"]);
                }
            }
        }
    }
}