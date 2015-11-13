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
    public class Luong_CanBo_BangDuLieu : BangDuLieu
    {
        private String[] _arrDSTruong_Fixed;
        private String[] _arrDSTruongTieuDe_Fixed;
        private String[] _arrDSTruongDoRong_Fixed;

        private String[] _arrDSTruong_Slide ;
        private String[] _arrDSTruongTieuDe_Slide;
        private String[] _arrDSTruongDoRong_Slide;

        private String[] _arrDSTruong_Them;

         /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public Luong_CanBo_BangDuLieu(String iID_MaBangLuong, String iID_MaBangLuongChiTiet, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaBangLuongChiTiet;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM L_BangLuong WHERE iID_MaBangLuong=@iID_MaBangLuong AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(LuongModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(LuongModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }
            _ChiDoc = false;
            _CoCotDuyet = false;
            _DuocSuaDuyet = false;

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(LuongModels.iID_MaPhanHe, MaND);

            _dtChiTiet = LuongModels.Get_dtBangLuongChiTiet_PhuCap_HienThi(iID_MaBangLuongChiTiet);

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
                _arrDSTruong_Fixed = "".Split(',');
                _arrDSTruongTieuDe_Fixed = "".Split(',');
                _arrDSTruongDoRong_Fixed = "".Split(',');

                _arrDSTruong_Slide = ("sMaHT,sTenPhuCap,rHeSo,rSoTien").Split(',');
                _arrDSTruongTieuDe_Slide = ("Mã PC,Tên phụ cấp,Mức(%),Số tiền").Split(',');
                _arrDSTruongDoRong_Slide = ("40,200,40,100").Split(',');

                _arrDSTruong_Them = ("iID_MaPhuCap,bDuocSuaChiTiet,bDuocSuaHeSo,iLoaiMa,bCongThuc,sCongThuc,sMaTruongHeSo_BangLuong,sMaTruongSoTien_BangLuong").Split(',');


                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
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
                String MaHang = String.Format("{0}", R["iID_MaBangLuongChiTiet_PhuCap"]);
                _arrDSMaHang.Add(MaHang);

                //_arrMaMucLucNganSach.Add(Convert.ToString(R["iID_MaMucLucNganSach"]));
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
        ///         + Trường sMaCongTrinh, sTenCongTrinh của cột tiền
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();
            
            //Tiêu đề fix
            ////for (int j = 0; j < _arrDSTruongTieuDe_Fixed.Length; j++)
            ////{
            ////    _arrDSMaCot.Add(_arrDSTruong_Fixed[j]);
            ////    _arrTieuDe.Add(_arrDSTruongTieuDe_Fixed[j]);
            ////    _arrWidth.Add(Convert.ToInt32(_arrDSTruongDoRong_Fixed[j]));
            ////    _arrHienThiCot.Add(true);
            ////    _arrSoCotCungNhom.Add(1);//ban đầu là 1
            ////    _arrTieuDeNhomCot.Add("");
            ////}

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường iID_MaDonVi
        ///         + Trường của cột tiền trừ sMaCongTrinh, sTenCongTrinh
        ///             - Cột phân bổ: Cột tổng phân bổ cho đơn vị
        ///             - Cột đã cấp: Cột đã cấp cho đơn vị trong năm ngân sách
        ///             - Cột còn lại: Cột còn lại chưa cấp cho đơn vị
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            //Tiêu đề tiền
            for (int j = 0; j < _arrDSTruong_Slide.Length; j++)
            {
                _arrDSMaCot.Add(_arrDSTruong_Slide[j]);
                _arrTieuDe.Add(_arrDSTruongTieuDe_Slide[j]);
                _arrWidth.Add(Convert.ToInt32(_arrDSTruongDoRong_Slide[j]));
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
            for (int j = 0; j < _arrDSTruong_Them.Length; j++)
            {
                if (_arrDSTruong_Them[j] != "")
                {
                    _arrDSMaCot.Add(_arrDSTruong_Them[j]);
                    _arrTieuDe.Add(_arrDSTruong_Them[j]);
                    _arrWidth.Add(0);
                    _arrHienThiCot.Add(false);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
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

                Boolean bDuocSuaChiTiet = Convert.ToBoolean(R["bDuocSuaChiTiet"]);
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = _ChiDoc == true || okHangChiDoc == true;
                    if (_arrDSMaCot[j] == "sMaHT" || _arrDSMaCot[j] == "rHeSo" || _arrDSMaCot[j] == "rSoTien")
                    {
                        okOChiDoc = okOChiDoc || false;
                    }
                    else
                    {
                        okOChiDoc = true;
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
            }
        }
    }
}