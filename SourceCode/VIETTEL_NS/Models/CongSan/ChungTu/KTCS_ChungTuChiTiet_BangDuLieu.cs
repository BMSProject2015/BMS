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
    public class KTCS_ChungTuChiTiet_BangDuLieu : BangDuLieu
    {
        public String strDSMaND = "";

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KTCS_ChungTuChiTiet_BangDuLieu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaChungTu;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM KTCS_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int iID_MaTrangThaiDuyet = -1;
            if (_dtBang.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);
                Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(CongSanModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet);
                if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                {
                    _ChiDoc = true;
                }

                if (ND_DuocSuaChungTu == false)
                {
                    _ChiDoc = true;
                }

                if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND))
                {
                    if (LuongCongViecModel.KiemTra_TrangThaiKhoiTao(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                        LuongCongViecModel.KiemTra_TrangThaiTuChoi(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                    {
                        _ChiDoc = false;
                    }
                }

                if ((LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                      LuongCongViecModel.KiemTra_TrangThaiKhoiTao(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet)) &&
                      ND_DuocSuaChungTu)
                {
                    _DuocSuaDuyet = true;
                }
                _CoCotDuyet = true;
            }
            else
            {
                _ChiDoc = false;
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
                _DuocSuaChiTiet = true;
            }

            if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND))
            {
                _DuocSuaDuyet = false;
            }

            //_DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(CongSanModels.iID_MaPhanHe, MaND);
            if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) || LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
            {
                //_DuocSuaChiTiet = true;
                if (iID_MaTrangThaiDuyet >= 0)
                {
                    if (LuongCongViecModel.KiemTra_TrangThaiKhoiTao(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                        LuongCongViecModel.KiemTra_TrangThaiTuChoi(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                    {
                        _DuocSuaChiTiet = true;
                    }
                }
            }


            if (_DuocSuaChiTiet == false && iID_MaTrangThaiDuyet >= 0)
            {
                if (LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
                {
                    if (_ChiDoc == false ||
                        LuongCongViecModel.KiemTra_TrangThaiKhoiTao(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                        LuongCongViecModel.KiemTra_TrangThaiTuChoi(CongSanModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                    {
                        _DuocSuaChiTiet = true;
                        _ChiDoc = false;
                    }
                }
            }
            _dtChiTiet = KTCS_ChungTuChiTietModels.Get_dtChungTuChiTiet(_iID_Ma, arrGiaTriTimKiem, _MaND);
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
                CapNhap_arrFormat_Rieng();
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
            String[] arrDSTruong = "sTinhChat,iNgay,iThang,sSoChungTuChiTiet,iID_MaKyHieuHachToan,sNoiDung,sTenTaiSan,rSoTien,rNamKhauHao,dThoiGianDuaVaoSuDung,sTenDonVi,sGhiChu,sLyDo".Split(',');
            String[] arrDSTruongTieuDe = "Tính chất,Ngày,Tháng,Số chứng từ chi tiết,KH Hạch toán,Nội dung,Tên tài sản,Số tiền,Số năm hao mòn,Thời gian đưa vào sử dụng,Đơn vị,Ghi chú,Lý do".Split(',');
            String[] arrDSTruongDoRong = "60,30,40,150,100,400,450,150,100,150,250,400,300".Split(',');
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
            //if (CoCotDuyet)
            //{
            //    //Cột đồng ý
            //    _arrDSMaCot.Add("bDongY");
            //    if (_ChiDoc)
            //    {
            //        _arrTieuDe.Add("<div class='check'></div>");
            //    }
            //    else
            //    {
            //        _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
            //    }
            //    _arrWidth.Add(20);
            //    _arrHienThiCot.Add(true);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //    //Cột Lý do
            //    _arrDSMaCot.Add("sLyDo");
            //    _arrTieuDe.Add("Nhận xét");
            //    _arrWidth.Add(200);
            //    _arrHienThiCot.Add(true);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //}
            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
            String strDSTruong = "iID_MaTaiSan,iID_MaDonVi";
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
                if (_ChiDoc == false)
                {
                    if (_arrDSMaCot[j] == "bDongY")
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
                    if (_arrDSMaCot[j] == "bDongY")
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
                if (_arrDSMaCot[j].StartsWith("d"))
                {
                    //Nhap Kieu datetime
                    _arrType.Add("4");
                }
                else if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    _arrType.Add("2");
                }
                else if (_arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    //Nhap Kieu so
                    _arrType.Add("1");
                }
                else if (_arrDSMaCot[j].StartsWith("sTenTaiSan") ||
                         _arrDSMaCot[j].StartsWith("sTenDonVi") || _arrDSMaCot[j].StartsWith("iID_MaKyHieuHachToan"))
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
        /// Hàm cập nhập định dạng cho các cột
        ///     - Cột có prefix 'd': 'dd/MM/yyyy'(datetime)
        ///     - Ngược lại: kiểu ''(textbox)
        /// </summary>
        protected void CapNhap_arrFormat_Rieng()
        {
            //Xac dinh kieu truong nhap du lieu
            _arrFormat = new List<string>();
            String strTG;
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                strTG = "";
                if (_arrDSMaCot[j].StartsWith("d"))
                {
                    //Nhap Kieu datetime
                    strTG = "dd/MM/yyyy";
                }
                _arrFormat.Add(strTG);
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
                    else if (_arrDSMaCot[j].StartsWith("d"))
                    {
                        _arrDuLieu[i].Add(CommonFunction.ChuyenNgaySangXau(R[_arrDSMaCot[j]]));
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