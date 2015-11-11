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
    /// Lớp BaoHiemChi_BangDuLieu cho phần bảng của Bảo hiểm chi
    /// </summary>
    public class GiaSanPham_BangDuLieu:BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTuChi"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public GiaSanPham_BangDuLieu(String iID_MaSanPham, String iID_MaChiTietGia, String iID_LoaiDonVi, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaSanPham;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM DM_SanPham_DanhMucGia WHERE iID_MaSanPham=@iID_MaSanPham AND iID_MaChiTietGia=@iID_MaChiTietGia AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaSanPham", _iID_Ma);
            cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            //int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            //Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeSanPham, MaND, iID_MaTrangThaiDuyet);
            //if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanHeModels.iID_MaPhanHeSanPham, iID_MaTrangThaiDuyet))
            //{
            //    _ChiDoc = true;
            //}

            //if (ND_DuocSuaChungTu == false)
            //{
            //    _ChiDoc = true;
            //}
            //////////////////////
            //_ChiDoc = false;
            //if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHeBaoHiem, iID_MaTrangThaiDuyet) &&
            //    ND_DuocSuaChungTu)
            //{
            //    _CoCotDuyet = true;
            //    _DuocSuaDuyet = true;
            //}

            //if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeBaoHiem, iID_MaTrangThaiDuyet))
            //{
            //    _CoCotDuyet = true;
            //}

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeSanPham, MaND);
            _DuocSuaChiTiet = true;
            //int ThuTu = 0;
            //SanPham_DanhMucGiaModels.CapNhatSttDanhMucGia(_iID_Ma, iID_MaChiTietGia, "", 0, ref ThuTu);
            _dtChiTiet = SanPham_DanhMucGiaModels.Get_dtDanhMucGiaChiTiet(_iID_Ma, iID_MaChiTietGia, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(iID_LoaiDonVi);
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(String iID_LoaiDonVi)
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide(iID_LoaiDonVi);
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit(iID_LoaiDonVi);
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
                String MaHang = Convert.ToString(R["iID_MaDanhMucGia"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
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
            _arrFormat = new List<string>();
            String[] arrDSTruong = "sKyHieu,sTen,sTen_DonVi".Split(',');
            String[] arrDSTruongTieuDe = "TT,Khoản mục chi phí trong giá thành sản phẩm,ĐVT".Split(',');
            String[] arrDSTruongDoRong = "50,400,60".Split(',');
            String[] arrDSTruongAlign = "center,left,center".Split(',');
            //Tiêu đề fix
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
                _arrAlign.Add(arrDSTruongAlign[j]);
                _arrFormat.Add("");
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide(String iID_LoaiDonVi)
        {
            String DSTruong = "", DSTruongTieuDe = "", DSTruongDoRong = "", DSNhom_TieuDe = "", DSNhom_SoCot = "", DSTruongAlign = "", DSTruongFormat = "";
            switch (iID_LoaiDonVi)
            {
                case "1":
                    DSTruong = "rSoLuong_DangThucHien,rDonGia_DangThucHien,rTien_DangThucHien,rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi,rSoSanh";
                    DSTruongTieuDe = "Số lượng,Đơn giá,Thành tiền,Số lượng,Đơn giá,Thành tiền,So Sánh (%)";
                    DSTruongDoRong = "100,120,120,100,120,120,70";
                    DSNhom_TieuDe = "Giá đang thực hiện,Giá đang thực hiện,Giá đang thực hiện,Giá đơn vị đề nghị,Giá đơn vị đề nghị,Giá đơn vị đề nghị,Giá đơn vị đề nghị";
                    DSNhom_SoCot = "3,3,3,4,4,4,4";
                    DSTruongAlign = "right,right,right,right,right,right,right";
                    DSTruongFormat = "4,2,2,4,2,2,2";
                    //DSEdit = "0,0,0,1,1,1,1";
                    break;
                case "2":
                    DSTruong = "rSoLuong_DangThucHien,rDonGia_DangThucHien,rTien_DangThucHien,rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi,rSoLuong_DatHang_DeNghi,rDonGia_DatHang_DeNghi,rTien_DatHang_DeNghi,rSoSanh";
                    DSTruongTieuDe = "Số lượng,Đơn giá,Thành tiền,Số lượng,Đơn giá,Thành tiền,Số lượng,Đơn giá,Thành tiền,So Sánh (%)";
                    DSTruongDoRong = "50,75,100,50,75,100,50,75,100,70";
                    DSNhom_TieuDe = "Giá đang thực hiện,Giá đang thực hiện,Giá đang thực hiện,Giá đơn vị đề nghị,Giá đơn vị đề nghị,Giá đơn vị đề nghị,Giá đơn đặt hàng đề nghị,Giá đơn đặt hàng đề nghị,Giá đơn đặt hàng đề nghị,Giá đơn đặt hàng đề nghị";
                    DSNhom_SoCot = "3,3,3,3,3,3,4,4,4,4";
                    DSTruongAlign = "right,right,right,right,right,right,right,right,right,right";
                    DSTruongFormat = "4,2,2,4,2,2,4,2,2,2";
                    //DSEdit = "0,0,0,0,0,0,1,1,1,1";
                    break;
                case "3":
                    DSTruong = "rSoLuong_DangThucHien,rDonGia_DangThucHien,rTien_DangThucHien,rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi,rSoLuong_DatHang_DeNghi,rDonGia_DatHang_DeNghi,rTien_DatHang_DeNghi,rSoLuong_CTC_DeNghi,rDonGia_CTC_DeNghi,rTien_CTC_DeNghi,rSoSanh";
                    DSTruongTieuDe = "Số lượng,Đơn giá,Thành tiền,Số lượng,Đơn giá,Thành tiền,Số lượng,Đơn giá,Thành tiền,Số lượng,Đơn giá,Thành tiền,So Sánh (%)";
                    DSTruongDoRong = "70,100,120,70,100,120,70,100,120,70,100,120,70";
                    DSNhom_TieuDe = "Giá đang thực hiện,Giá đang thực hiện,Giá đang thực hiện,Giá đơn vị đề nghị,Giá đơn vị đề nghị,Giá đơn vị đề nghị,Giá đơn đặt hàng đề nghị,Giá đơn đặt hàng đề nghị,Giá đơn đặt hàng đề nghị,Giá CTC đề nghị,Giá CTC đề nghị,Giá CTC đề nghị,Giá CTC đề nghị";
                    DSNhom_SoCot = "3,3,3,3,3,3,3,3,3,4,4,4,4";
                    DSTruongAlign = "right,right,right,right,right,right,right,right,right,right,right,right,right";
                    DSTruongFormat = "4,2,2,4,2,2,4,2,2,4,2,2,2";
                    //DSEdit = "0,0,0,0,0,0,0,0,0,1,1,1,1";
                    break;
            }
            String[] arrDSTruong = DSTruong.Split(',');
            String[] arrDSTruongTieuDe = DSTruongTieuDe.Split(',');
            String[] arrDSTruongDoRong = DSTruongDoRong.Split(',');
            String[] arrDSNhom_TieuDe = DSNhom_TieuDe.Split(',');
            String[] arrDSNhom_SoCot = DSNhom_SoCot.Split(',');
            String[] arrDSTruongAlign = DSTruongAlign.Split(',');
            String[] arrDSTruongFormat = DSTruongFormat.Split(',');
            //String[] arrDSEdit = DSEdit.Split(',');

            //_arrSoCotCungNhom.Add(1);
            //_arrTieuDeNhomCot.Add("");

            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                _arrAlign.Add(arrDSTruongAlign[j]);
                _arrFormat.Add(arrDSTruongFormat[j]);
               
                //_arrEdit.Add(arrDSEdit[j]);
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
            //Them cot an 
            _arrDSMaCot.Add("bNganSach");
            _arrTieuDe.Add("Là ngân sách");
            _arrWidth.Add(0);
            _arrHienThiCot.Add(false);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");
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
                    if (Convert.ToString(R["iID_MaDanhMucGia_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaDanhMucGia"]))
                    {
                        CSCha = j;
                        break;
                    }
                }
                _arrCSCha.Add(CSCha);
            }
        }

        /// <summary>
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhap_arrEdit(String iID_LoaiDonVi)
        {
            _arrEdit = new List<List<string>>();
            String DSEdit = "";
            switch (iID_LoaiDonVi)// them 1 cot dang sau DSEdit cho cot an bNganSach
            {
                case "1":
                    DSEdit = ",,,1,1,1,1,1,1,1,";
                    break;
                case "2":
                    DSEdit = ",,,1,1,1,,,,1,1,1,1,";
                    break;
                case "3":
                    DSEdit = ",,,1,1,1,,,,,,,1,1,1,1,";
                    break;
            }
            String[] arrDSEdit = DSEdit.Split(',');
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                if (Convert.ToBoolean(R["bLaHangCha"]))
                {
                    okHangChiDoc = true;
                }

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet && _ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet &&
                                _ChiDoc == false &&
                                okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    if (okOChiDoc)
                    {
                        _arrEdit[i].Add("");
                    }
                    else
                    {
                        _arrEdit[i].Add(arrDSEdit[j]);
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
        /// <summary>
        /// Hàm cập nhập mảng căn lề cho các cột
        ///     - Cột có prefix 'b': 'center'
        ///     - Cột có prefix 'r' hoặc 'i' (trừ 'iID'): 'right'
        ///     - Ngược lại: 'left'
        ///     - Các cột 'iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG': 'right'
        /// </summary>
        protected void CapNhap_arrAlign()
        {
            //Xac dinh kieu truong nhap du lieu
            _arrAlign = new List<string>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    _arrAlign.Add("center");
                }
                else if (_arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    //Nhap Kieu so
                    _arrAlign.Add("right");
                }
                else
                {
                    //Nhap kieu xau
                    _arrAlign.Add("left");
                }
            }
            String[] arrDSTruongChuyenSangPhai = "iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG".Split(',');

            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                for (int i = 0; i < arrDSTruongChuyenSangPhai.Length; i++)
                {
                    if (_arrDSMaCot[j] == arrDSTruongChuyenSangPhai[i])
                    {
                        _arrAlign[j] = "right";
                        break;
                    }
                }
            }
        }
    }
}