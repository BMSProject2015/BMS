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
    public class QLDA_KeHoachVon_BangDuLieu : BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public QLDA_KeHoachVon_BangDuLieu(String iID_KeHoachVon_QuyetDinh, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_KeHoachVon_QuyetDinh;
            this._MaND = MaND;
            this._IPSua = IPSua;


            //int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            //Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(QLDAModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet);
            //if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(QLDAModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            //{
            //    _ChiDoc = true;
            //}

            //if (ND_DuocSuaChungTu == false)
            //{
            //    _ChiDoc = true;
            //}

            //if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(QLDAModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) &&
            //    ND_DuocSuaChungTu)
            //{
            //    _CoCotDuyet = true;
            //    _DuocSuaDuyet = true;
            //}

            //if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(QLDAModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            //{
            //    _CoCotDuyet = true;
            //}

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(QLDAModels.iID_MaPhanHe, MaND);

            _dtChiTiet = QLDA_KeHoachVonModels.Get_dtKeHoachVon(_iID_Ma, arrGiaTriTimKiem);
            ThemHangTongCong();
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu();
        }

        protected void ThemHangTongCong()
        {
            //Thêm hàng tổng cộng 
            DataRow R = _dtChiTiet.NewRow();
            _dtChiTiet.Rows.Add(R);

            for (int j = 0; j < _dtChiTiet.Columns.Count; j++)
            {
                String TenCot = _dtChiTiet.Columns[j].ColumnName;
                if (TenCot.StartsWith("r") && TenCot != "rTyGia")
                {
                    Double S = 0;
                    for (int i = 0; i < _dtChiTiet.Rows.Count - 1; i++)
                    {
                        S += Convert.ToDouble(_dtChiTiet.Rows[i][TenCot]);
                    }
                    R[TenCot] = S;
                }
            }
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
                String MaHang = Convert.ToString(R["iID_MaKeHoachVon"]);
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
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruong = "sTenDuAn,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,rSoTienDauNam,rNgoaiTe_SoTienDauNam,sTenNgoaiTe_SoTienDauNam,rNgoaiTe_DauNam,sTenNgoaiTe_DauNam,rTyGia_DauNam,rSoTienDieuChinh,rNgoaiTe_SoTienDieuChinh,sTenNgoaiTe_SoTienDieuChinh,rNgoaiTe_DieuChinh,sTenNgoaiTe_DieuChinh,rTyGia_DieuChinh,rTongTien,rTongNgoaiTeTrongDo,rTongNgoaiTeDieuChinh".Split(',');
            String[] arrDSTruongTieuDe = "Thông tin dự án,LNS,L,K,M,TM,TTM,NG,TNG,Nội dung,Số tiền đầu năm,Trong đó ngoại tệ,Loại NT,Ngoại tệ đầu năm,Loại NT,Tỷ giá,Số tiền điều chỉnh,Trong đó ngoại tệ,Loại NT,Ngoại tệ điều chỉnh,Loại NT,Tỷ giá điều chỉnh,Số tiền,Trong đó ngoại tệ,Ngoại tệ".Split(',');
            String[] arrDSTruongDoRong = "450,60,30,30,30,30,30,30,30,300,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150".Split(',');
            String[] arrDSNhom_TieuDe = ",,,,,,,,,,Số đầu năm,Số đầu năm,Số đầu năm,Số đầu năm,Số đầu năm,Số đầu năm,Số điều chỉnh,Số điều chỉnh,Số điều chỉnh,Số điều chỉnh,Số điều chỉnh,Số điều chỉnh,Tổng số,Tổng số,Tổng số".Split(',');
            String[] arrDSNhom_SoCot = "1,1,1,1,1,1,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,3,3,3".Split(',');
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
            String[] arrDSTruong = "iID_MaDanhMucDuAn,iID_MaMucLucNganSach,sXauNoiMa,iID_MaNgoaiTe_SoTienDauNam,iID_MaNgoaiTe_DauNam,iID_MaNgoaiTe_SoTienDieuChinh,iID_MaNgoaiTe_DieuChinh,sMauSac,sFontColor,sFontBold".Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruong[j]);
                _arrWidth.Add(0);
                _arrHienThiCot.Add(false);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
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
            String[] arrDSTruongAutocomplete = "sTenDuAn,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sTenNgoaiTe_SoTienDauNam,sTenNgoaiTe_DauNam,sTenNgoaiTe_SoTienDieuChinh,sTenNgoaiTe_DieuChinh".Split(',');
            //Xac dinh kieu truong nhap du lieu
            _arrType = new List<string>();
            String KieuNhap;
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                KieuNhap = "0";
                if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    KieuNhap = "2";
                }
                else if (_arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    //Nhap Kieu so
                    KieuNhap = "1";
                }
                else
                {
                    //Nhap kieu xau
                    for (int i = 0; i < arrDSTruongAutocomplete.Length; i++)
                    {
                        if (_arrDSMaCot[j] == arrDSTruongAutocomplete[i])
                        {
                            //Nhap Kieu autocomplete
                            KieuNhap = "3";
                            break;
                        }
                    }
                }
                _arrType.Add(KieuNhap);
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
                    //if (_arrDSMaCot[j] == "rTongTien" ||
                    //   _arrDSMaCot[j] == "rTongNgoaiTeTrongDo" ||
                    //   _arrDSMaCot[j] == "rTongNgoaiTeDieuChinh" 
                    //   )
                    //{
                    //    okOChiDoc = true;
                    //}
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
                        if (i >= _dtChiTiet.Rows.Count - 1)
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

            //Hàng tổng cộng
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                _arrEdit.Add(new List<string>());
                _arrEdit[_dtChiTiet.Rows.Count - 1].Add("");
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
                if (_ChiDoc == false)
                {
                    if (_arrDSMaCot[j] == "rTongTien" || _arrDSMaCot[j] == "rTongNgoaiTeTrongDo" || _arrDSMaCot[j] == "rTongNgoaiTeDieuChinh")
                    {
                        okCotDuocPhepNhap = false;
                    }
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
        /// Hàm cập nhập mảng dữ liệu
        /// </summary>
        protected void CapNhap_arrDuLieu()
        {
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count -3; j++)
                {
                    //Xac dinh gia tri
                    _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                }

                if (i == _dtChiTiet.Rows.Count - 1)
                {
                    _arrDuLieu[i].Add("#A0A0A0");
                    _arrDuLieu[i].Add("#FF0000");
                    _arrDuLieu[i].Add("bold");
                }
                else
                {
                    _arrDuLieu[i].Add("");
                    _arrDuLieu[i].Add("");
                    _arrDuLieu[i].Add("");
                }
            }
        }
    }
}