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
    /// Lớp QuyetToan_ChungTu_BangDuLieu cho phần bảng của Quyết toán
    /// </summary>
    public class QuyetToan_ChungTu_BangDuLieu : BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public QuyetToan_ChungTu_BangDuLieu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem,
                                            String MaND, String IPSua,String MaLoai)
        {
            this._iID_Ma = iID_MaChungTu;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM QTA_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            String iQuy = Convert.ToString(_dtBang.Rows[0]["iThang_Quy"]);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu =
                LuongCongViecModel.NguoiDung_DuocSuaChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND,
                                                            iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (
                LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan,
                                                               iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(QuyetToanModels.iID_MaPhanHeQuyetToan, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);

            _dtChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtQuyetToanChiTiet(_iID_Ma, MaND, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(iQuy,MaLoai);
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(String iQuy,String MaLoai)
        {
            if (_arrDuLieu == null)
            {
                if(MaLoai=="ChiTieu")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        if ((String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rChiTieu"])) || Convert.ToDecimal(_dtChiTiet.Rows[i]["rChiTieu"]) <= 0) )
                            _dtChiTiet.Rows.RemoveAt(i);
                    }
                  
                }

                else if(MaLoai=="KhongChiTieu")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    for (int i = count; i >= 0; i--)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rChiTieu"])))
                        {
                            if (Convert.ToDecimal(_dtChiTiet.Rows[i]["rChiTieu"]) > 0 && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"])==false)
                                _dtChiTiet.Rows.RemoveAt(i);
                        }
                    }
                    CapNhapHangTongCong();
                }
                else if(MaLoai=="DonViDeNghiKhac")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rTuChi"])))
                        {
                            if (Convert.ToDecimal(_dtChiTiet.Rows[i]["rTuChi"]) - Convert.ToDecimal(_dtChiTiet.Rows[i]["rDonViDeNghi"]) == 0)
                                _dtChiTiet.Rows.RemoveAt(i);
                        }
                    }
                  
                }
                else if (MaLoai == "QuyetToan")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        if ((String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rTuChi"])) || Convert.ToDecimal(_dtChiTiet.Rows[i]["rTuChi"]) <= 0))
                            _dtChiTiet.Rows.RemoveAt(i);
                    }
                }
                else
                {
                    CapNhapHangTongCong();
                }
                String SQL = "UPDATE QTA_ChungTu SET MaLoai='" + MaLoai + "' WHERE iID_MaChungTu='" + _iID_Ma+"'";
               SqlCommand cmd= new SqlCommand(SQL);
               Connection.UpdateDatabase(cmd);
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed(iQuy);
                CapNhapDanhSachMaCot_Slide(iQuy);
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
                //Khong ghi cac hang cha
                for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
                {
                    DataRow R = _dtChiTiet.Rows[i];
                    if (Convert.ToBoolean(R["bLaHangCha"]))
                    {
                        _arrDSMaHang[i] = "";
                    }
                }
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
                String MaHang = String.Format("{0}_{1}", R["iID_MaChungTuChiTiet"], R["iID_MaMucLucNganSach"]);
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
        protected void CapNhapDanhSachMaCot_Fixed(String iQuy)
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            String[] arrDSTruong = (MucLucNganSachModels.strDSTruong + ",rChiTieu,rDaQuyetToan").Split(',');
            String[] arrDSTruongTieuDe =
                (MucLucNganSachModels.strDSTruongTieuDe + ",Chi tiêu ngân sách,Đã quyết toán").Split(',');
            // String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSTruongDoRong;
            if (iQuy == "4")
            {
                arrDSTruongDoRong = "50,25,25,35,35,22,20,20,250,120,120".Split(',');
            }
            else
            {
                arrDSTruongDoRong = (MucLucNganSachModels.strDSTruongDoRong + ",120,120").Split(',');
            }

            //Xác định các cột tiền sẽ hiển thị
            //_arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            //for (int j = 0; j < arrDSTruongTien.Length; j++)
            //{
            //    _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
            //    for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            //    {
            //        DataRow R = _dtChiTiet.Rows[i];
            //        if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
            //        {
            //            _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
            //            break;
            //        }
            //    }
            //}

            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if (arrDSTruong[j] == "sTNG")
                {
                    _arrHienThiCot.Add(false);
                }
                else
                {
                    _arrHienThiCot.Add(true);
                }
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền trừ sMaCongTrinh, sTenCongTrinh
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide(String iQuy)
        {
            //String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            //String[] arrDSTruongTienTieuDe = MucLucNganSachModels.strDSTruongTienTieuDe.Split(',');
            //String[] arrDSTruongTienDoRong = MucLucNganSachModels.strDSTruongTienDoRong.Split(',');

            String[] arrDSTruongTien =
                //"rChiTieu,rDaQuyetToan,rSoTien,rDonViDeNghi,rVuotChiTieu,rTonThatTonDong".Split(',');
                "rDonViDeNghi,rTuChi,rVuotChiTieu,rTonThatTonDong,rDaCapTien,rChuaCapTien,sGhiChu".Split(',');
            String[] arrDSTruongTienTieuDe =
                "Đơn vị đề nghị,Số quyết toán duyệt,Vượt chỉ tiêu, Tổn thất-tồn đọng,Đã cấp tiền,Chưa cấp tiền,Ghi chú".
                    Split(',');
            String[] arrDSTruongTienDoRong = "115,115,115,115,115,115,300".Split(',');
            String[] arrDSNhom_TieuDe =
                ",,Đề nghị bộ xử lý,Đề nghị bộ xử lý,Chi tiêu ngân sách,Chi tiêu ngân sách,Chi tiêu ngân sách".Split(',');
            String[] arrDSNhom_SoCot = "1,1,2,2,3,3,3".Split(',');

            if (iQuy == "4")
            {
                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));

                    _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                    _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                    // _arrAlign.Add(arrDSTruongAlign[j]);

                    _arrHienThiCot.Add(true);

                }
            }
                //các quý khác sẽ ẩn các truong tien, trừ cot rTuChi
            else
            {
                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {
                    if (arrDSTruongTien[j] == "rTuChi" || arrDSTruongTien[j] == "rDonViDeNghi")
                    {
                        _arrDSMaCot.Add(arrDSTruongTien[j]);
                        _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                        _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));

                        _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                        _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                        // _arrAlign.Add(arrDSTruongAlign[j]);

                        _arrHienThiCot.Add(true);
                    }

                }
            }

            //Tiêu đề tiền
            //for (int j = 0; j < arrDSTruongTien.Length; j++)
            //{
            //    if (arrDSTruongTien[j] == "sTenCongTrinh" &&
            //        _arrCotTienDuocHienThi[arrDSTruongTien[j]])
            //    {
            //        _arrDSMaCot.Add(arrDSTruongTien[j]);
            //        _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
            //        _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        _arrHienThiCot.Add(true);
            //        _arrSoCotCungNhom.Add(1);
            //        _arrTieuDeNhomCot.Add("");
            //    }
            //    if (arrDSTruongTien[j] == "rNgay" &&
            //        _arrCotTienDuocHienThi[arrDSTruongTien[j]])
            //    {
            //        _arrDSMaCot.Add(arrDSTruongTien[j]);
            //        _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
            //        _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        _arrHienThiCot.Add(true);
            //        _arrSoCotCungNhom.Add(1);
            //        _arrTieuDeNhomCot.Add("");
            //    }
            //    if (arrDSTruongTien[j] == "rSoNguoi" &&
            //        _arrCotTienDuocHienThi[arrDSTruongTien[j]])
            //    {
            //        _arrDSMaCot.Add(arrDSTruongTien[j]);
            //        _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
            //        _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        _arrHienThiCot.Add(true);
            //        _arrSoCotCungNhom.Add(1);
            //        _arrTieuDeNhomCot.Add("");
            //    }
            //}

            //for (int j = 0; j < arrDSTruongTien.Length; j++)
            //{
            //    if (arrDSTruongTien[j] == "rTongSo" ||
            //        (arrDSTruongTien[j] != "sTenCongTrinh" && arrDSTruongTien[j] != "rNgay" && arrDSTruongTien[j] != "rSoNguoi" && _arrCotTienDuocHienThi[arrDSTruongTien[j]]))
            //    {
            //        //_arrDSMaCot.Add(arrDSTruongTien[j] + "_DonVi");
            //        //_arrTieuDe.Add("Đơn vị đề nghị");
            //        //_arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        //_arrHienThiCot.Add(true);
            //        //_arrSoCotCungNhom.Add(5);
            //        //_arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

            //        //_arrDSMaCot.Add(arrDSTruongTien[j] + "_ChiTieu");
            //        //_arrTieuDe.Add("Chỉ tiêu");
            //        //_arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        //_arrHienThiCot.Add(true);
            //        //_arrSoCotCungNhom.Add(5);
            //        //_arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

            //        _arrDSMaCot.Add(arrDSTruongTien[j]);
            //        _arrTieuDe.Add("Quyết toán");
            //        _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //        _arrHienThiCot.Add(true);
            //        _arrSoCotCungNhom.Add(1);
            //        _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

            //_arrDSMaCot.Add(arrDSTruongTien[j] + "_DaQuyetToan");
            //_arrTieuDe.Add("Đã quyết toán");
            //_arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //_arrHienThiCot.Add(true);
            //_arrSoCotCungNhom.Add(5);
            //_arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

            //_arrDSMaCot.Add(arrDSTruongTien[j] + "_ConLai");
            //_arrTieuDe.Add("Còn lại");
            //_arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
            //_arrHienThiCot.Add(true);
            //_arrSoCotCungNhom.Add(5);
            //_arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);
            //    }
            //}

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
            String strDSTruong = "iID_MaMucLucNganSach,sMauSac,sFontColor,sFontBold";
            String[] arrDSTruong = strDSTruong.Split(',');
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
                    if (Convert.ToString(R["iID_MaMucLucNganSach_Cha"]) ==
                        Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach"]))
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
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) &&
                            (_arrDSMaCot[j] != "rTongSo" &&
                             //_arrDSMaCot[j].EndsWith("_DonVi") == false &&
                             _arrDSMaCot[j].EndsWith("_ChiTieu") == false &&
                             _arrDSMaCot[j].EndsWith("_DaQuyetToan") == false &&
                             _arrDSMaCot[j].EndsWith("_ConLai") == false &&
                             _dtChiTiet.Columns.IndexOf("b" + _arrDSMaCot[j]) >= 0 &&
                             Convert.ToBoolean(R["b" + _arrDSMaCot[j]])) || _arrDSMaCot[j].EndsWith("_DonVi"))
                        {
                            okOChiDoc = false;
                        }
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) && _arrDSMaCot[j] == "rDonViDeNghi")
                        {
                            okOChiDoc = false;
                        } 
                        if ((_DuocSuaChiTiet &&
                              _ChiDoc == false &&
                              okHangChiDoc == false) && _arrDSMaCot[j] == "rTuChi")
                        {
                            okOChiDoc = false;
                        }
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) && _arrDSMaCot[j] == "rVuotChiTieu")
                        {
                            okOChiDoc = false;
                        }
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) && _arrDSMaCot[j] == "rTonThatTonDong")
                        {
                            okOChiDoc = false;
                        }
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) && _arrDSMaCot[j] == "rDaCapTien")
                        {
                            okOChiDoc = false;
                        }
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) && _arrDSMaCot[j] == "rChuaCapTien")
                        {
                            okOChiDoc = false;
                        }
                        if ((_DuocSuaChiTiet &&
                             _ChiDoc == false &&
                             okHangChiDoc == false) && _arrDSMaCot[j] == "sGhiChu")
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
        protected void CapNhap_arrDuLieu()
        {
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count-3; j++)
                {
                    //Xac dinh gia tri
                    if (_arrDSMaCot[j].EndsWith("_ConLai"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        Double GT3 = Convert.ToDouble(_arrDuLieu[i][j - 3]);
                        _arrDuLieu[i].Add(Convert.ToString(GT3 - GT2 - GT1));
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
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

        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = "rChiTieu,rDaQuyetToan,rDonViDeNghi,rTuChi";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
           
            int len = arrDSTruongTien.Length;
            //Tinh lai cot tong so
            double Tong = 0;
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S;
                        //rTongSo
                        S = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[j][arrDSTruongTien[k]])))
                                {
                                    S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                }
                            }
                        }
                        _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                    }
                }
            }
            //Them Hàng tổng cộng
            DataRow dr = _dtChiTiet.NewRow();

            DataRow[] arrdr = _dtChiTiet.Select("sNG<>''");

            for (int k = 0; k < len; k++)
            {
                Tong = 0;
                for (int i = 0; i < arrdr.Length; i++)
                {
                  
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i][arrDSTruongTien[k]])))
                    {
                        Tong += Convert.ToDouble(arrdr[i][arrDSTruongTien[k]]);
                    }
                   
                }
                dr[arrDSTruongTien[k]] = Tong;
            }
            dr["sMoTa"] = "TỔNG CỘNG";
            dr["bLaHangCha"] = true;
            _dtChiTiet.Rows.Add(dr);
        }
    }
}