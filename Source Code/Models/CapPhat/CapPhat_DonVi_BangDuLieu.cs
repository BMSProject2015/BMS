using System;
using System.Collections.Generic;
using DomainModel;
using System.Data;
using System.Data.SqlClient;

namespace VIETTEL.Models
{
    public class CapPhat_DonVi_BangDuLieu : BangDuLieu
    {
        // loai hien thi chung tu chi tiet: tat ca, cap phat, chua cap phat
        public static string MALOAI = "TATCA";
        
        /// <summary>
        /// Thiết lập hiển thị bảng dữ liệu
        /// </summary>
        public static void ThietLapHienThi(int option)
        { 
            switch(option)
            {
                case 0: MALOAI = "TATCA";
                    break;
                case 1: MALOAI = "CAPPHAT";
                    break;
                case 2: MALOAI = "CHUACAPPHAT";
                    break;
                default: MALOAI = "TATCA";
                    break;
            }
            
        }
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public CapPhat_DonVi_BangDuLieu(String iID_MaCapPhat, Dictionary<String, String> arrGiaTriTimKiem,String sMaLoai, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaCapPhat;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM CP_CapPhat WHERE iID_MaCapPhat=@iID_MaCapPhat AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(CapPhatModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(CapPhatModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(CapPhatModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(CapPhatModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND);

            _dtChiTiet = CapPhat_ChungTuChiTiet_DonViModels.LayDtCapPhatChiTietDonVi(_iID_Ma, arrGiaTriTimKiem,MaND);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(sMaLoai);
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(String sMaLoai)
        {
            // lấy danh sách trường tiền chứng từ
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            if (_arrDuLieu == null)
            {
                if (sMaLoai.ToUpper() == "CAPPHAT")
                {
                    CapNhapHangTongCong();

                    if (sMaLoai == "CapPhat")
                    {
                        int count = dtChiTiet.Rows.Count - 1;
                        bool ok = true;
                        for (int i = count; i >= 0; i--)
                        {
                            ok = false;
                            for (int j = 0; j < arrDSTruongTien.Length; j++)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0)
                                {
                                    ok = true;
                                }
                                //neu ma phong ban nguoi dung khac ma phong ban tao
                            }

                            //Loai bo het dong khong co du lieu
                            if (ok == false)
                            {
                                _dtChiTiet.Rows.RemoveAt(i);
                            }

                        }

                    }
                }
                else if (sMaLoai.ToUpper() == "CHUACAPPHAT")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool ok = true;

                    for (int i = count; i >= 0; i--)
                    {
                        ok = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                            {
                                ok = true;
                            }
                        }

                        //Loai bo het dong  co du lieu
                        if (ok)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }

                    }
                    CapNhapHangTongCong();
                }
                else
                    CapNhapHangTongCong();

                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                //CapNhapDanhSachMaCot_Them();
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
                String MaHang = String.Format("{0}_{1}", R["iID_MaCapPhatChiTiet"], R["iID_MaMucLucNganSach"]);
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
                    //VungNV: Hiện thị các trường có ký tự [b] ở đầu trong DB có giá trị = 1 và trường rTuChi
                    if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]) && arrDSTruongTien[j] == "rTuChi")
                    {
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
                        break;
                    }
                }
            }
            //VungNV: lấy giá trị của trường sLoai 
            DataTable dtCapPhat = CapPhat_ChungTuModels.LayToanBoThongTinChungTu(_iID_Ma);
            DataRow dr = dtCapPhat.Rows[0];
            int index = -1;
            if (dtCapPhat != null && dtCapPhat.Rows.Count > 0)
            {
                index = getIndex(Convert.ToString(dr["sLoai"]));
            }
            dtCapPhat.Dispose();

            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                
                if (arrDSTruong[j] != "sMoTa" && (getIndex(arrDSTruong[j]) > index || index == -1))
                {
                    _arrHienThiCot.Add(false);
                }
                else {
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
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruongTien = "rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap".Split(',');
            String[] arrDSTruongTienTieuDe = "Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập,Hàng mua,Hiện vật,Dự phòng,Phân cấp".Split(',');
            String[] arrDSTruongTienDoRong = "100,100,100,100,100,100,100,100,100,100,100,100".Split(',');

            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {

                if (arrDSTruongTien[j] == "rTuChi" && _arrCotTienDuocHienThi[arrDSTruongTien[j]])
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_Phanbo");
                    _arrTieuDe.Add("Phân Bổ");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(false);
                    _arrSoCotCungNhom.Add(4);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_DaCap");
                    _arrTieuDe.Add("Đã cấp");
                    //_arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrWidth.Add(200);//VungNV: độ rộng 200
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(4);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add("Cấp phát");
                    //_arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrWidth.Add(200);//VungNV: độ rộng 200
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(4);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_ConLai");
                    _arrTieuDe.Add("Còn lại");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(false);
                    _arrSoCotCungNhom.Add(4);
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
            String strDSTruong = "iID_MaMucLucNganSach";
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
                //Hang cha thi chi doc
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
                                _dtChiTiet.Columns.IndexOf("b" + _arrDSMaCot[j]) >= 0 &&
                                Convert.ToBoolean(R["b" + _arrDSMaCot[j]])
                            )
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
                for (int j = 0; j < _arrDSMaCot.Count; j++)
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
            }
        }

        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            String strDSTruongTien_ChiTieu = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_PhanBo,")+"_PhanBo";
            String strDSTruongTien_DaQuyetToan = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_DaCap,")+ "_DaCap";
            String strDSTruongTien_ConLai = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_ConLai,") + "_DaCap";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTien_ChiTieu = strDSTruongTien_ChiTieu.Split(',');
            String[] arrDSTruongTien_DaQuyetToan = strDSTruongTien_DaQuyetToan.Split(',');
            String[] arrDSTruongTien_ConLai = strDSTruongTien_ConLai.Split(',');

            int len = arrDSTruongTien.Length;
            //Tinh lai cot tong so
            /*
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

                    //rTongSo_ChiTieu
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien_ChiTieu[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[len - 1]] = S;
                    }

                    //rTongSo_DaQuyetToan
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien_DaQuyetToan[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[len - 1]] = S;
                    }

                }
            } */
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {

                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {

                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S, S_ChiTieu, S_DaQuyetToan;
                        S = 0;
                        S_ChiTieu = 0;
                        S_DaQuyetToan = 0;

                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                S_ChiTieu += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_ChiTieu[k]]);
                                S_DaQuyetToan += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_DaQuyetToan[k]]);
                            }
                        }
                        //VungNV: them Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[k]])!=S_DaQuyetToan 
                        if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]) != S ||
                            Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]]) != S_ChiTieu ||
                            Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[k]]) !=S_DaQuyetToan  
                            )
                        {
                            _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                            _dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]] = S_ChiTieu;
                            _dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[k]] = S_DaQuyetToan;
                        }
                    }
                }
            }
        }

        /*
        * <summary>
        * lấy index của chuỗi loại ngân sách: sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
        * index cha lớn hơn index con
        * </summary>
        * */
        private int getIndex(string val)
        {
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            int index = -1;
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (val == arrDSTruong[i])
                {
                    index = i;
                    break;
                }
            }
                
            return index;
        }
    }
}