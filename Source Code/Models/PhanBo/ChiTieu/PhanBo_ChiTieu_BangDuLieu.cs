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
    public class PhanBo_ChiTieu_BangDuLieu: BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public PhanBo_ChiTieu_BangDuLieu(String iID_MaChiTieu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua,Boolean LayTuDuToan=false)
        {
            this._iID_Ma = iID_MaChiTieu;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM PB_ChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanBoModels.iID_MaPhanHeChiTieu, MaND, iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanBoModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanBoModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanBoModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHeChiTieu, MaND);

            _dtChiTiet = PhanBo_ChiTieuChiTietModels.Get_dtChiTieuChiTiet(_iID_Ma, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();

            DienDuLieu(LayTuDuToan);
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(Boolean LayTuDuToan)
        {
            if (_arrDuLieu == null)
            {
                CapNhapHangTongCong();
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrThayDoi();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu(LayTuDuToan);
                
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
                String MaHang = Convert.ToString(R["iID_MaChiTieuChiTiet"]);
                _arrDSMaHang.Add(MaHang);
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

            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = MucLucNganSachModels.strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = MucLucNganSachModels.strDSTruongTienDoRong.Split(',');
            String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
                for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
                {
                    DataRow R = _dtChiTiet.Rows[i];
                    if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
                    {
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
                        break;
                    }
                }
            }

            //Tiêu đề fix
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {

            String[] arrDSTruongTien = "rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,rTongSo".Split(',');
            String[] arrDSTruongTienTieuDe = "Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập,Hàng mua,Hiện vật,Dự phòng,Phân cấp,Tổng số".Split(',');
            String[] arrDSTruongTienDoRong = "100,100,100,100,100,100,100,100,100,100,100,100,100".Split(',');

            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j] == "sTenCongTrinh" &&
                    _arrCotTienDuocHienThi[arrDSTruongTien[j]])
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
            }

            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j] == "rTongSo" ||
                    (arrDSTruongTien[j] != "sTenCongTrinh" && _arrCotTienDuocHienThi[arrDSTruongTien[j]]) )
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_DuToan");
                    _arrTieuDe.Add("Đơn vị đề nghị");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(3);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add("Dự toán duyệt");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(3);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_ConLai");
                    _arrTieuDe.Add("Chênh lệch");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(3);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);
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
            //String strDSTruong = "sMaCongTrinh";
            //String[] arrDSTruong = strDSTruong.Split(',');
            //for (int j = 0; j < arrDSTruong.Length; j++)
            //{
            //    _arrDSMaCot.Add(arrDSTruong[j]);
            //    _arrTieuDe.Add(arrDSTruong[j]);
            //    _arrWidth.Add(0);
            //    _arrHienThiCot.Add(false);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //}
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
                                okHangChiDoc == false &&
                                _arrDSMaCot[j] != "rTongSo" &&
                                _arrDSMaCot[j].EndsWith("_DuToan") == false &&
                                _arrDSMaCot[j].EndsWith("_ConLai") == false &&
                                _dtChiTiet.Columns.IndexOf("b" + _arrDSMaCot[j]) >=0 && 
                                Convert.ToBoolean(R["b" + _arrDSMaCot[j]]))
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
                        _arrEdit[i].Add("1");
                    }
                }
            }
        }

        /// <summary>
        /// Hàm cập nhập mảng dữ liệu
        /// </summary>
        protected void CapNhap_arrDuLieu(Boolean LayTuDuToan)
        {
            CapNhap_arrThayDoi();
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    //Xac dinh gia tri
                    if (_arrDSMaCot[j].EndsWith("_ConLai"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        _arrDuLieu[i].Add(Convert.ToString(GT2 - GT1));
                    }
                    else if (_arrDSMaCot[j].EndsWith("_DuToan"))
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                    else
                    {
                        if (_arrDSMaCot[j].StartsWith("r") && (Convert.ToString(R[_arrDSMaCot[j]]) == "0" || Convert.ToString(R[_arrDSMaCot[j]]) == ""))
                        {
                            if (LayTuDuToan)
                            {
                                _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j] + "_DuToan"]));
                                _arrThayDoi[i][j] = true;
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
        }

        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So + ",rTongSo";
            String strDSTruongTien_DuToan = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_DuToan,") + "_DuToan,rTongSo_DuToan";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTien_DuToan = strDSTruongTien_DuToan.Split(',');

            int len = arrDSTruongTien.Length;
            //Tinh lai cot tong so
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                {
                    double S;
                    //rTongSo
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien[len - 1]] = S;
                    }
                    //rTongSo_DuToan
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien_DuToan[k].StartsWith("rChiTapTrung")==false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DuToan[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DuToan[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien_DuToan[len - 1]] = S;
                    }
                }
            }
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S, S_DuToan;
                        //rTongSo
                        S = 0;
                        S_DuToan = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                S_DuToan += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_DuToan[k]]);
                            }
                        }
                        if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]) != S ||
                            Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DuToan[k]]) != S_DuToan)
                        {
                            _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                            _dtChiTiet.Rows[i][arrDSTruongTien_DuToan[k]] = S_DuToan;
                        }
                    }
                }
            }
        }
    }
}