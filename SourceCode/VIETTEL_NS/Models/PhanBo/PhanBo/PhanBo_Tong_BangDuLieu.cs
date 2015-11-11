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
using VIETTEL.Models;
namespace VIETTEL.Models
{
    public class PhanBo_Tong_BangDuLieu : BangDuLieu
    {
        public static string iID_MaDonViChoPhanBo = "99";
        public static string TenDonViChoPhanBo = "Chờ phân bổ";
        private String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
        private String[] arrDSTruongTien = "sTenCongTrinh_ChiTieu,sTenCongTrinh,rNgay_ChiTieu,rNgay,rSoNguoi_ChiTieu,rSoNguoi,rChiTaiKhoBac_ChiTieu,rChiTaiKhoBac,rTonKho_ChiTieu,rTonKho,rTuChi_ChiTieu,rTuChi,rChiTapTrung_ChiTieu,rChiTapTrung,rHangNhap_ChiTieu,rHangNhap,rHangMua_ChiTieu,rHangMua,rHienVat_ChiTieu,rHienVat,rDuPhong_ChiTieu,rDuPhong,rPhanCap_ChiTieu,rPhanCap,rTongSo".Split(',');
        private String[] arrDSDuocNhapTruongTien = MucLucNganSachModels.strDSDuocNhapTruongTien.Split(',');
        private DataTable _dtDonVi = null;
        private List<List<Double>> _arrGiaTriNhom = new List<List<Double>>();
        private List<int> _arrChiSoNhom = new List<int>();


        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaPhanBo"> Mã chỉ tiêu</param>
        /// <param name="MaND">Mã người dùng</param>
        public PhanBo_Tong_BangDuLieu(String iID_MaPhanBo, Dictionary<String, String> arrGiaTriTimKiem, String MaND)
        {
            this._iID_Ma = iID_MaPhanBo;
            this._MaND = MaND;

            _dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(_MaND);

            _DuocSuaChiTiet = true;
            SqlCommand cmd;
            String SQL = "SELECT * FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            //Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND, iID_MaTrangThaiDuyet);
            //if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanBoModels.iID_MaPhanHePhanBo, iID_MaTrangThaiDuyet) &&
            // ND_DuocSuaChungTu)
            //{
            //    _CoCotDuyet = true;
            //    _DuocSuaDuyet = true;
            //}

            //if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanBoModels.iID_MaPhanHePhanBo, iID_MaTrangThaiDuyet))
            //{
                _CoCotDuyet = true;
            //}
                //_DuocSuaDuyet = true;
            _dtChiTiet = PhanBo_TongModels.Get_dtPhanBoTong_ChiTiet(_iID_Ma, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();

            //CapNhapNhom();

            DienDuLieu();
        }

        /// <summary>
        /// Hàm hủy bỏ sẽ hủy dữ liệu của bảng _dtDonVi
        /// </summary>
        ~PhanBo_Tong_BangDuLieu()
        {
            if (_dtDonVi != null) _dtDonVi.Dispose();
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
                CapNhap_arrType_Rieng();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhapHangTongCong();
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
                String MaHang = String.Format("{0}", R["iID_MaPhanBoChiTiet"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột fixed vào bảng
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

            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');

            String[] arrDSTruongTien = "sTenCongTrinh_ChiTieu,sTenCongTrinh,rNgay_ChiTieu,rNgay,rSoNguoi_ChiTieu,rSoNguoi,rChiTaiKhoBac_ChiTieu,rChiTaiKhoBac,rTonKho_ChiTieu,rTonKho,rTuChi_ChiTieu,rTuChi,rChiTapTrung_ChiTieu,rChiTapTrung,rHangNhap_ChiTieu,rHangNhap,rHangMua_ChiTieu,rHangMua,rHienVat_ChiTieu,rHienVat,rDuPhong_ChiTieu,rDuPhong,rPhanCap_ChiTieu,rPhanCap".Split(',');//MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = "Tên công trình DV,Tên công trình,Ngày DV,Ngày,Người DV,Người,Chi tại kho bạc DV,Chi tại kho bạc,Tồn kho DV,Tồn kho,Đơn vị đề nghị TC,Tự chi,Chi tập trung DV,Chi tập trung,Hàng nhập DV,Hàng nhập,Hàng mua DV,Hàng mua,Đon vị đề nghị HV,Hiện vật,Dự phòng DV,Dự phòng,Phân cấp DV,Phân cấp".Split(',');//MucLucNganSachModels.strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = "150,150,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100".Split(',');//MucLucNganSachModels.strDSTruongTienDoRong.Split(',');
            String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
                for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
                {
                    DataRow R = _dtChiTiet.Rows[i];
                    if (arrDSTruongTien[j].EndsWith("_ChiTieu")==false && Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
                    {
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]+"_ChiTieu"] = true;
                        break;
                    }
                }
            }

            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            //for (int j = 0; j < arrDSTruongTien.Length; j++)
            //{
            //    if ((arrDSTruongTien[j] == "sMaCongTrinh" || arrDSTruongTien[j] == "sTenCongTrinh")
            //        && _arrCotTienDuocHienThi[arrDSTruongTien[j]])
            //    {
            //        _arrDSMaCot.Add(arrDSTruongTien[j]);
            //        _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
            //        _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        _arrHienThiCot.Add(true);
            //        _arrSoCotCungNhom.Add(1);
            //        _arrTieuDeNhomCot.Add("");
            //    }
            //}

            //Thêm cột số chứng từ
            _arrDSMaCot.Add("sSoCT");
            _arrTieuDe.Add("CT");
            _arrWidth.Add(50);
            _arrHienThiCot.Add(true);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");
            //Thêm cột ngày chứng từ
            _arrDSMaCot.Add("dNgayChungTu");
            _arrTieuDe.Add("Ngày CT");
            _arrWidth.Add(100);
            _arrHienThiCot.Add(true);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");

            //Tiêu đề tiền: Bỏ qua trường sMaCongTrinh, sTenCongTrinh
            _arrDSMaCot.Add("iID_MaDonVi");
            _arrTieuDe.Add("Đơn vị");
            _arrWidth.Add(50);
            _arrHienThiCot.Add(true);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường iID_MaDonVi
        ///         + Trường của cột tiền trừ sMaCongTrinh, sTenCongTrinh
        ///         + Trường sTongSo
        ///         + Trường sTenDonVi
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            //String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            //String[] arrDSTruongTienTieuDe = MucLucNganSachModels.strDSTruongTienTieuDe.Split(',');
            //String[] arrDSTruongTienDoRong = MucLucNganSachModels.strDSTruongTienDoRong.Split(',');
            String[] arrDSTruongTien = "sTenCongTrinh_ChiTieu,sTenCongTrinh,rNgay_ChiTieu,rNgay,rSoNguoi_ChiTieu,rSoNguoi,rChiTaiKhoBac_ChiTieu,rChiTaiKhoBac,rTonKho_ChiTieu,rTonKho,rTuChi_ChiTieu,rTuChi,rChiTapTrung_ChiTieu,rChiTapTrung,rHangNhap_ChiTieu,rHangNhap,rHangMua_ChiTieu,rHangMua,rHienVat_ChiTieu,rHienVat,rDuPhong_ChiTieu,rDuPhong,rPhanCap_ChiTieu,rPhanCap".Split(',');//MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = "Tên công trình DV,Tên công trình,Ngày DV,Ngày,Người DV,Người,Chi tại kho bạc DV,Chi tại kho bạc,Tồn kho DV,Tồn kho,Đơn vị đề nghị TC,Tự chi,Chi tập trung DV,Chi tập trung,Hàng nhập DV,Hàng nhập,Hàng mua DV,Hàng mua,Đon vị đề nghị HV,Hiện vật,Dự phòng DV,Dự phòng,Phân cấp DV,Phân cấp".Split(',');//MucLucNganSachModels.strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = "150,150,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100".Split(',');//MucLucNganSachModels.strDSTruongTienDoRong.Split(',');

           
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (_arrCotTienDuocHienThi[arrDSTruongTien[j]])
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
            }
            //Thêm cột tổng số cuối cùng
            _arrDSMaCot.Add("rTongSo");
            _arrTieuDe.Add("Tổng số");
            _arrWidth.Add(100);
            _arrHienThiCot.Add(true);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");

            _arrDSMaCot.Add("TenDonVi");
            _arrTieuDe.Add("Tên đơn vị");
            _arrWidth.Add(100);
            _arrHienThiCot.Add(true);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");

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
            _arrDSMaCot.Add("iID_MaPhanBo");
            _arrTieuDe.Add("iID_MaPhanBo");
            _arrWidth.Add(100);
            _arrHienThiCot.Add(false);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");
            

            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
            String strDSTruong = "sMaCongTrinh";
            String[] arrDSTruong = strDSTruong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                if (String.IsNullOrEmpty(arrDSTruong[j]) == false)
                {
                    _arrDSMaCot.Add(arrDSTruong[j]);
                    _arrTieuDe.Add(arrDSTruong[j]);
                    _arrWidth.Add(0);
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
                    if (Convert.ToString(R["iID_MaMucLucNganSach_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach"]))
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
            String[] arrDSTruongAutocomplete = "sTenCongTrinh".Split(',');
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

                if (Convert.ToBoolean(R["bLaHangCha"]))
                {
                    okHangChiDoc = true;
                }
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
                    //Cot tien
                    if (_arrDSMaCot[j] == "iID_MaDonVi")
                    {
                        if (_DuocSuaChiTiet &&
                            _ChiDoc == false &&
                            okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    else if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    else if (_DuocSuaChiTiet &&
                            _ChiDoc == false &&
                            okHangChiDoc == false &&
                            _arrDSMaCot[j] != "rTongSo" &&
                            _arrDSMaCot[j] != "TenDonVi" &&
                            _dtChiTiet.Columns.IndexOf('b' + _arrDSMaCot[j]) >= 0 &&
                            Convert.ToBoolean(R['b' + _arrDSMaCot[j]]))
                    {
                        okOChiDoc = false;
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
                    if (_arrDSMaCot[j] == "TenDonVi")
                    {
                        if (Convert.ToBoolean(R["bLaHangCha"]))
                        {
                            //Nếu là hàng cha thi không có mã đơn vị
                            _arrDuLieu[i].Add("");
                        }
                        else
                        {
                            //Lấy tên đơn vị
                            _arrDuLieu[i].Add(LayTenDonVi(Convert.ToString(R["iID_MaDonVi"])));
                        }

                    }
                    else if (_arrDSMaCot[j] == "iID_MaDonVi")
                    {
                        if (Convert.ToBoolean(R["bLaHangCha"]))
                        {
                            //Nếu là hàng cha thi không có mã đơn vị
                            _arrDuLieu[i].Add("");
                        }
                        else
                        {
                            _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                        }
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                }
            }
        }

        /// <summary>
        /// Hàm tính lại tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            //Cap nhap cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (_arrLaHangCha[i])
                {
                    for (int k = _nCotFixed + 1; k < _arrDSMaCot.Count - 2; k++)
                    {
                        if (_arrDSMaCot[k].StartsWith("r"))
                        {
                            double S = 0;
                            for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                            {
                                if (i == _arrCSCha[j] && _arrDSMaCot[k].StartsWith("r") && _arrDSMaCot[k] != "rTongSo")
                                {
                                    S += Convert.ToDouble(_dtChiTiet.Rows[j][_arrDSMaCot[k]]);
                                }
                            }
                            _dtChiTiet.Rows[i][_arrDSMaCot[k]] = S;
                            arrDuLieu[i][k] = Convert.ToString(S);
                        }
                    }
                }
            }
            //Cap nhap cot tong so
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                double S = 0;
                int k;
                for (k = _nCotFixed; k < _arrDSMaCot.Count - 6; k++)
                {
                    if (_arrDSMaCot[k].StartsWith("r") && _arrDSMaCot[k] != "rTongSo" && _arrDSMaCot[k] != "rChiTapTrung" && _arrDSMaCot[k].EndsWith("_ChiTieu")==false)
                    {
                        S += Convert.ToDouble(_dtChiTiet.Rows[i][_arrDSMaCot[k]]);
                    }
                }
                k = _arrDSMaCot.Count -6;
                _dtChiTiet.Rows[i][_arrDSMaCot[k]] = S;
                arrDuLieu[i][k] = Convert.ToString(S);
            }
        }

        /// <summary>
        /// Thuộc tính lấy danh sách mã đơn vị và tên đơn vị cho Javascript
        /// </summary>
        public String strDSDonVi
        {
            get
            {
                String _strDSDonVi = "";
                for (int csDonVi = 0; csDonVi < _dtDonVi.Rows.Count; csDonVi++)
                {
                    if (csDonVi > 0) _strDSDonVi += "##";
                    _strDSDonVi += String.Format("{0}##{1}", _dtDonVi.Rows[csDonVi]["iID_MaDonVi"], _dtDonVi.Rows[csDonVi]["sTen"]);
                }
                return _strDSDonVi;
            }
        }

        /// <summary>
        /// Hàm lấy tên đơn vị từ mã đơn vị
        /// </summary>
        public String LayTenDonVi(String iID_MaDonVi)
        {
            String TenDonVi = "";
            if (iID_MaDonVi == iID_MaDonViChoPhanBo)
            {
                TenDonVi = TenDonViChoPhanBo;
            }
            else
            {
                for (int csDonVi = 0; csDonVi < _dtDonVi.Rows.Count; csDonVi++)
                {
                    if (iID_MaDonVi == Convert.ToString(_dtDonVi.Rows[csDonVi]["iID_MaDonVi"]))
                    {
                        TenDonVi = Convert.ToString(_dtDonVi.Rows[csDonVi]["sTen"]);
                        break;
                    }
                }
            }
            return TenDonVi;
        }

        /// <summary>
        /// Hàm xác định các hàng cùng nhóm và giá trị của 1 nhóm
        /// </summary>
        protected void CapNhapNhom()
        {
            DataTable dtChiTieu = PhanBo_ChiTieuChiTietModels.Get_dtChiTieuChiTiet(_iID_Ma, null);
            DataTable dtChoPhanBo = PhanBo_PhanBoChiTietModels.Get_dtDonViChoPhanBo(_iID_Ma);
            List<Double> ChoPhanBoTien = new List<double>();//tuannn tạo thêm để lấy tiền chờ phân bổ còn thừa
            List<Double> STien = new List<double>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                STien.Add(0);
                ChoPhanBoTien.Add(0);
            }
            int csChiTieu = 0;
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrChiSoNhom.Add(-1);
                DataRow R = _dtChiTiet.Rows[i];
                String sXauNoiMa = Convert.ToString(R["sXauNoiMa"]);
                if (Convert.ToBoolean(R["bLaHangCha"]) == false)
                {
                    //Truong hop la hang con
                    if (sXauNoiMa == Convert.ToString(_dtChiTiet.Rows[i - 1]["sXauNoiMa"]))
                    {
                        //Truong hop cung 1 nhom voi hang truoc
                        _arrChiSoNhom[i] = _arrChiSoNhom[i - 1];
                    }
                    else
                    {
                        //Truong hop nhom moi
                        _arrGiaTriNhom.Add(new List<double>());
                        _arrChiSoNhom[i] = _arrGiaTriNhom.Count - 1;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            _arrGiaTriNhom[_arrGiaTriNhom.Count - 1].Add(0);
                        }
                        //Lấy tiền chờ phân bổ của những đợt trước tuannn thêm
                        for (int c = 0; c < dtChoPhanBo.Rows.Count; c++)
                        {
                            String sXauNoiMa1 = Convert.ToString(dtChoPhanBo.Rows[c]["sXauNoiMa"]);
                            if (sXauNoiMa == sXauNoiMa1)
                            {
                                for (int j = 0; j < arrDSTruongTien.Length; j++)
                                {
                                    if (arrDSTruongTien[j].StartsWith("r"))
                                    {
                                        ChoPhanBoTien[j] = Convert.ToDouble(dtChoPhanBo.Rows[c][arrDSTruongTien[j]]);
                                    }
                                }
                                break;
                            }
                            else
                            {
                                for (int j = 0; j < arrDSTruongTien.Length; j++)
                                {
                                    if (arrDSTruongTien[j].StartsWith("r"))
                                    {
                                        ChoPhanBoTien[j] = 0;
                                    }
                                }
                            }
                        }
                        //END Lấy tiền chờ phân bổ của những đợt trước
                        //Neu ton tai trong chi tieu thi lay thong tin cua chi tieu
                        while (csChiTieu < dtChiTieu.Rows.Count)
                        {
                            DataRow RChiTieu = dtChiTieu.Rows[csChiTieu];
                            String tg_sXauNoiMa = Convert.ToString(RChiTieu["sXauNoiMa"]);
                            if (tg_sXauNoiMa == sXauNoiMa)
                            {
                                for (int j = 0; j < arrDSTruongTien.Length; j++)
                                {
                                    
                                    if (arrDSTruongTien[j].StartsWith("r"))
                                    {
                                        String sTruongTien=arrDSTruongTien[j];
                                        if (sTruongTien.EndsWith("_ChiTieu"))
                                        {
                                            sTruongTien = sTruongTien.Replace("_ChiTieu", "_DuToan");
                                        }
                                        Double rGT = Convert.ToDouble(RChiTieu[sTruongTien]);
                                        // Trước khi sửa 
                                        //_arrGiaTriNhom[_arrGiaTriNhom.Count - 1][j] = rGT;
                                        //STien[j] = rGT;
                                        _arrGiaTriNhom[_arrGiaTriNhom.Count - 1][j] = rGT + ChoPhanBoTien[j];//Cộng thêm tiền chờ phân bổ còn thừa của đợt trước vào đợt này
                                        STien[j] = rGT + ChoPhanBoTien[j];//Cộng thêm tiền chờ phân bổ còn thừa của đợt trước vào đợt này
                                    }
                                }
                            }
                            else if (String.Compare(tg_sXauNoiMa, sXauNoiMa) > 0)
                            {
                                break;
                            }
                            csChiTieu++;
                        }
                    }

                    if (Convert.ToString(R["iID_MaDonVi"]) == iID_MaDonViChoPhanBo)
                    {
                        
                        //Truong hop la hang CHO PHAN BO
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (arrDSTruongTien[j].StartsWith("r"))
                            {
                                R[arrDSTruongTien[j]] = STien[j];
                            }
                        }
                    }
                    else
                    {
                        //Truong hop khong phai la hang CHO PHAN BO
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (arrDSTruongTien[j].StartsWith("r"))
                            {
                                Double rGT = Convert.ToDouble(R[arrDSTruongTien[j]]);
                                STien[j] -= rGT;
                            }
                        }
                    }
                }
            }

            dtChiTieu.Dispose();
        }

        /// <summary>
        /// Thuộc tính lấy xâu danh sách chỉ số nhóm cho Javascript
        /// </summary>
        public String strDSChiSoNhom
        {
            get
            {
                String vR = "";
                for (int i = 0; i < _arrChiSoNhom.Count; i++)
                {
                    if (i > 0) vR += ",";
                    vR += _arrChiSoNhom[i];
                }
                return vR;
            }
        }

        /// <summary>
        /// Thuộc tính lấy xâu danh sách giá trị nhóm cho Javascript
        /// </summary>
        public String strGiaTriNhom
        {
            get
            {
                String vR = "";
                for (int i = 0; i < _arrGiaTriNhom.Count; i++)
                {
                    if (i > 0) vR += DauCachHang;
                    Boolean ok = false;
                    for (int j = 0; j < _arrDSMaCot.Count; j++)
                    {
                        for (int cs = 0; cs < arrDSTruongTien.Length; cs++)
                        {
                            if (arrDSTruongTien[cs] == _arrDSMaCot[j])
                            {
                                if (ok) vR += DauCachO;
                                vR += _arrGiaTriNhom[i][cs];
                                ok = true;
                                break;
                            }
                        }
                    }
                }
                return vR;
            }
        }
    }
}