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
    public class Luong_BangDuLieu : BangDuLieu
    {
        public const int iLoaiBangLuong_BangChiTiet = 0;
        public const int iLoaiBangLuong_BangThueTNCN = 1;
        public const int iLoaiBangLuong_BangBaoHiem = 2;
        public const int iLoaiBangLuong_BangTruyLinh = 3;

        private String[] _arrDSTruong_Fixed = null;
        private String[] _arrDSTruongTieuDe_Fixed = null;
        private String[] _arrDSTruongDoRong_Fixed = null;

        private String[] _arrDSTruong_Slide = null;
        private String[] _arrDSTruongTieuDe_Slide = null;
        private String[] _arrDSTruongDoRong_Slide = null;
        private String[] _arrDSTruongSoCotCungNhom_Slide = null;
        private String[] _arrDSTruongTieuDeNhomCot_Slide = null;
        

        int _iLoaiBangLuong;
         /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="iLoaiBangLuong">=0: Bảng lương chi tiết; 1: Bảng thuế</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public Luong_BangDuLieu(String iID_MaBangLuong, int iLoaiBangLuong, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaBangLuong;
            this._MaND = MaND;
            this._IPSua = IPSua;
            this._iLoaiBangLuong = iLoaiBangLuong;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM L_BangLuong WHERE iID_MaBangLuong=@iID_MaBangLuong AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", _iID_Ma);
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
            if (_iLoaiBangLuong == iLoaiBangLuong_BangTruyLinh)
            {
                _ChiDoc = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(LuongModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(LuongModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }



            if (_iLoaiBangLuong == iLoaiBangLuong_BangChiTiet)
            {
                _DuocSuaChiTiet = false;
            }
            else
            {
                _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(LuongModels.iID_MaPhanHe, MaND);
            }

            _dtChiTiet = LuongModels.Get_dtBangLuongChiTiet(iID_MaBangLuong, arrGiaTriTimKiem, _iLoaiBangLuong);

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
                switch (_iLoaiBangLuong)
                {
                    case iLoaiBangLuong_BangThueTNCN:
                        //Bảng lương thuế TNCN
                        _arrDSTruong_Fixed = BangLuongModels.strDSTruong.Split(',');
                        _arrDSTruongTieuDe_Fixed = BangLuongModels.strDSTruongTieuDe.Split(',');
                        _arrDSTruongDoRong_Fixed = BangLuongModels.strDSTruongDoRong.Split(',');

                        _arrDSTruong_Slide= BangLuongModels.strDSTruongTien_ThueTNCN.Split(',');
                        _arrDSTruongTieuDe_Slide = BangLuongModels.strDSTruongTienTieuDe_ThueTNCN.Split(',');
                        _arrDSTruongDoRong_Slide = BangLuongModels.strDSTruongTienDoRong_ThueTNCN.Split(',');
                        break;

                    case iLoaiBangLuong_BangBaoHiem:
                        //Bảng lương Bảo hiểm
                        _arrDSTruong_Fixed = BangLuongModels.strDSTruong.Split(',');
                        _arrDSTruongTieuDe_Fixed = BangLuongModels.strDSTruongTieuDe.Split(',');
                        _arrDSTruongDoRong_Fixed = BangLuongModels.strDSTruongDoRong.Split(',');

                        _arrDSTruong_Slide = BangLuongModels.strDSTruongTien_BaoHiem.Split(',');
                        _arrDSTruongTieuDe_Slide = BangLuongModels.strDSTruongTienTieuDe_BaoHiem.Split(',');
                        _arrDSTruongDoRong_Slide = BangLuongModels.strDSTruongTienDoRong_BaoHiem.Split(',');
                        _arrDSTruongSoCotCungNhom_Slide = BangLuongModels.strDSTruongSoCotCungNhom_BaoHiem.Split(',');
                        _arrDSTruongTieuDeNhomCot_Slide = BangLuongModels.strDSTruongTieuDeNhomCot_BaoHiem.Split(',');
                        break;

                    default:
                        //Bảng lương chi tiết
                        _arrDSTruong_Fixed = BangLuongModels.strDSTruong.Split(',');
                        _arrDSTruongTieuDe_Fixed = BangLuongModels.strDSTruongTieuDe.Split(',');
                        _arrDSTruongDoRong_Fixed = BangLuongModels.strDSTruongDoRong.Split(',');

                        _arrDSTruong_Slide = BangLuongModels.strDSTruongTien.Split(',');
                        _arrDSTruongTieuDe_Slide = BangLuongModels.strDSTruongTienTieuDe.Split(',');
                        _arrDSTruongDoRong_Slide = BangLuongModels.strDSTruongTienDoRong.Split(',');
                        break;
                }

                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
                CapNhap_arrFormat_Rieng();
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
                String MaHang = String.Format("{0}", R["iID_MaBangLuongChiTiet"]);
                _arrDSMaHang.Add(MaHang);

               // _arrMaMucLucNganSach.Add(Convert.ToString(R["iID_MaMucLucNganSach"]));
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
            for (int j = 0; j < _arrDSTruongTieuDe_Fixed.Length; j++)
            {
                _arrDSMaCot.Add(_arrDSTruong_Fixed[j]);
                _arrTieuDe.Add(_arrDSTruongTieuDe_Fixed[j]);
                _arrWidth.Add(Convert.ToInt32(_arrDSTruongDoRong_Fixed[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);//ban đầu là 1
                _arrTieuDeNhomCot.Add("");
            }

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
                if (_arrDSTruongTieuDeNhomCot_Slide == null)
                {
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
                else
                {
                    _arrSoCotCungNhom.Add(Convert.ToInt32(_arrDSTruongSoCotCungNhom_Slide[j]));
                    _arrTieuDeNhomCot.Add(_arrDSTruongTieuDeNhomCot_Slide[j]);
                }
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
            BangCSDL bangCSDL = new BangCSDL("L_BangLuongChiTiet");
            List<String> arrDSTruong_Full = bangCSDL.DanhSachTruong();
            //int csMin = arrDSTruong_Full.IndexOf("iThangLamViec");
            int csMax = arrDSTruong_Full.IndexOf("iSTT");

            for (int j = 0; j < csMax; j++)
            {
                Boolean okThem = true;
                for (int i = 0; okThem && i < _arrDSTruong_Fixed.Length; i++)
                {
                    if (_arrDSTruong_Fixed[i] == arrDSTruong_Full[j])
                    {
                        okThem = false;
                        break;
                    }
                }
                for (int i = 0; okThem && i < _arrDSTruong_Slide.Length; i++)
                {
                    if (_arrDSTruong_Slide[i] == arrDSTruong_Full[j])
                    {
                        okThem = false;
                        break;
                    }
                }
                if (okThem)
                {
                    _arrDSMaCot.Add(arrDSTruong_Full[j]);
                    _arrTieuDe.Add(arrDSTruong_Full[j]);
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

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = (_ChiDoc || okHangChiDoc || _DuocSuaChiTiet == false);
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet)
                        {
                            okOChiDoc = okOChiDoc || false;
                        }
                    }
                    else
                    {
                        if (_iLoaiBangLuong == iLoaiBangLuong_BangThueTNCN)
                        {
                            if (_arrDSMaCot[j] != "iSoNguoiPhuThuoc_CanBo" && 
                                _arrDSMaCot[j] != "rTrichLuong")
                            {
                                okOChiDoc = okOChiDoc || false;
                            }
                        }
                        else if (_iLoaiBangLuong == iLoaiBangLuong_BangBaoHiem)
                        {
                            if (_DuocSuaDuyet == false)
                            {
                                okOChiDoc = okOChiDoc || false;
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

        protected void CapNhap_arrFormat_Rieng()
        {
            CapNhap_arrFormat();
            String[] arrDSTruongFormat = ("rLuongCoBan_HeSo_CanBo").Split(',');
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {   
                for (int i = 0; i < arrDSTruongFormat.Length; i++)
                {
                    if (_arrDSMaCot[j] == arrDSTruongFormat[i])
                    {
                        _arrFormat[j] = "2";
                    }
                }
            }
        }
    }
}