using System;
using System.Collections.Generic;
using DomainModel;
using System.Data;
using System.Data.SqlClient;

namespace VIETTEL.Models.DuToanBS
{
    /// <summary>
    /// Lớp DuToanBS_PhanCapBangDuLieu cho phần bảng của Phân bổ chỉ tiêu
    /// </summary>
    public class DuToanBSPhanCapBangDuLieu : BangDuLieu
    {
        protected int _iNamLamViec;
        private List<List<Double>> _arrGiaTriNhom = new List<List<Double>>();
        private List<int> _arrChiSoNhom = new List<int>();
        private List<string> _arrMaMucLucNganSach = new List<string>();
        private DataTable _dtDonVi = null;

        /// <summary>
        /// Thuộc tính lấy chỉ số nhóm
        /// </summary>
        public string strDSChiSoNhom
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
        /// Thuộc tính lấy danh sách mã mục lục ngân sách
        /// </summary>
        public string strMaMucLucNganSach
        {
            get
            {
                String vR = "";
                for (int i = 0; i < _arrMaMucLucNganSach.Count; i++)
                {
                    if (i > 0) vR += ",";
                    vR += _arrMaMucLucNganSach[i];
                }
                return vR;
            }
        }
        /// <summary>
        /// Thuộc tính lấy danh sách mã đơn vị và tên đơn vị cho Javascript
        /// </summary>
        public string strDSDonVi
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

        #region DuToanBSPhanCapBangDuLieu
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        ///sLNS,sL,sK,sM,sTM,sTTM
        public DuToanBSPhanCapBangDuLieu(String iID_MaChungTu, Dictionary<string, string> arrGiaTriTimKiem, string MaND, string IPSua, string sLNS, string sKieuXem)
        {
            _dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(_MaND);
            this._iID_Ma = iID_MaChungTu;
            this._MaND = MaND;
            this._IPSua = IPSua;

            _dtBang = DuToanBSChungTuChiTietModels.LayDanhSachChungTuChiTiet(_iID_Ma);
            _iNamLamViec = Convert.ToInt32(_dtBang.Rows[0]["iNamLamViec"]);

            string sXauNoiMa = Convert.ToString(_dtBang.Rows[0]["sXauNoiMa"]);
            if (!String.IsNullOrEmpty(sXauNoiMa))
                sXauNoiMa = sXauNoiMa.Substring(8);
            int maTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);
            string maNDTao = Convert.ToString(_dtBang.Rows[0]["sID_MaNguoiDungTao"]);
            string iID_MaChungTuTo = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaChungTu, "iID_MaChungTu"));
            string iKyThuat = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTuTo, "iKyThuat")); ;
            // String sLNS = Convert.ToString(_dtBang.Rows[0]["sDSLNS"]);
            bool bDuocSuaCT = false;
            bool bTaoMoi = false;
            if (iKyThuat == "1")
            {
                bDuocSuaCT = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND, maTrangThaiDuyet);
                _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
                bTaoMoi = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, maTrangThaiDuyet);
            }
            else
            {
                bDuocSuaCT = LuongCongViecModel.NguoiDung_DuocSuaChungTu(DuToanBSModels.iID_MaPhanHe, MaND, maTrangThaiDuyet);
                _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanBSModels.iID_MaPhanHe, MaND);
                bTaoMoi = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeDuToan, maTrangThaiDuyet);
            }

            //Trolytonghop duoc sua chung tu do minh tạo
            bool bTroLyTH = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
            if (bTroLyTH && bTaoMoi)
            {
                bDuocSuaCT = true;
                _DuocSuaChiTiet = true;
            }
            if (bDuocSuaCT == false)
            {
                _ChiDoc = true;
            }
            //duoc sua khi o trang thai moi tao
            if (bTaoMoi == false) _DuocSuaChiTiet = false;

            if (iKyThuat == "1")
            {
                //Trang thai tu choi
                if (MaND == maNDTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeChiTieu, maTrangThaiDuyet))
                {
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
                //Tro ly tong hop dc sua chung tu
                if (bTroLyTH && maTrangThaiDuyet == DuToanBSChungTuChiTietModels.iMaTrangThaiDuyetKT)
                {
                    bDuocSuaCT = true;
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
            }
            else
            {
                //Trang thai tu choi
                if (MaND == maNDTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, maTrangThaiDuyet))
                {
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
                if (bTroLyTH && maTrangThaiDuyet == DuToanBSChungTuChiTietModels.iMaTrangThaiDuyetKT)
                {
                    bDuocSuaCT = true;
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
            }
            sKieuXem = Convert.ToString(_dtBang.Rows[0]["MaLoai"]);
            //Phan cap lan 2
            _dtChiTiet = DuToanBSChungTuChiTietModels.LayChungTuChiTietPhanCap(_iID_Ma, arrGiaTriTimKiem, MaND, sLNS, sXauNoiMa, iKyThuat, sKieuXem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(sLNS, sKieuXem, iKyThuat);
        } 
        #endregion

        #region CapNhapHangTongCong
        /// <summary>
        /// Cập nhật giá trị tổng cộng
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            string strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            string[] arrDSTruongTien = strDSTruongTien.Split(',');
            int len = arrDSTruongTien.Length;
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        if (i == 2 && k > len - 2)
                        {
                            i = 2;
                        }
                        double sTongSo;
                        //rTongSo
                        sTongSo = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[j][arrDSTruongTien[k]])))
                                {
                                    sTongSo += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                }
                            }
                        }
                        _dtChiTiet.Rows[i][arrDSTruongTien[k]] = sTongSo;
                    }
                }
            }
            //Them Hàng tổng cộng
            DataRow dr = _dtChiTiet.NewRow();
            DataRow[] arrdr = _dtChiTiet.Select("sNG<>''");
            double Tong = 0;
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
        #endregion

        #region DienDuLieu
        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(string sLNS, string sKieuXem, string iKyThuat)
        {
            string maPhongBan = "";
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(_MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                maPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
                dtPhongBan.Dispose();
            }

            string[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            if (_arrDuLieu == null)
            {
                //Chỉ xem dữ liệu đã nhập
                if (sKieuXem == "DuToan")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool isHienThiDong = true;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        isHienThiDong = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                            {
                                isHienThiDong = false;
                            }
                            else if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                            {
                                isHienThiDong = true;
                            }
                        }
                        //Loai bo het dong khong co du lieu
                        if (isHienThiDong == false)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }
                    }
                }
                //Chỉ xem dữ liệu chưa nhập
                else if (sKieuXem == "ChuaDuToan")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool coDuLieu = true;

                    for (int i = count; i >= 0; i--)
                    {
                        coDuLieu = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                            {
                                coDuLieu = false;
                            }
                            else if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                            {
                                coDuLieu = true;
                            }
                        }
                        //Loai bo het dong  co du lieu
                        if (coDuLieu)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }

                    }
                    CapNhapHangTongCong();
                }
                //Chỉ xem dữ liệu có phân cấp
                else if (sKieuXem == "PhanCap")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool isHienThiDong = true;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        isHienThiDong = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (arrDSTruongTien[j] == "rPhanCap" || arrDSTruongTien[j] == "rHangNhap" || arrDSTruongTien[j] == "rHangMua")
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                                {
                                    isHienThiDong = false;
                                }
                                else if (
                                    (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) &&
                                     Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                                {
                                    isHienThiDong = true;
                                }
                            }
                        }
                        //Loai bo het dong khong co du lieu
                        if (isHienThiDong == false)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }

                    }
                    count = _dtChiTiet.Rows.Count;
                    _dtChiTiet.Rows.RemoveAt(count - 1);
                    CapNhapHangTongCong();
                }
                // Xem tất cả
                else
                {
                    CapNhapHangTongCong();
                }

                //Them thong tin phần đầu ngân sách bảo đảm
                DataTable dtCT = _dtBang;
                DataRow rows = dtCT.Rows[0];
                DataRow dr = _dtChiTiet.NewRow();
                dr["sMoTa"] = "TỔNG Cộng";
                dr["sTenDonVi"] = rows["sTenDonVi"];
                dr["sTenDonVi_BaoDam"] = rows["sTenDonVi"];
                dr["rTuChi"] = rows["rPhanCap"];
                dr["rHienVat"] = rows["rHienVat"];
                dr["bLaHangCha"] = "true";
                dr["sLNS"] = rows["sLNS"];
                dr["sL"] = rows["sL"];
                dr["sK"] = rows["sK"];
                dr["sM"] = rows["sM"];
                dr["sTM"] = rows["sTM"];
                dr["sTTM"] = rows["sTTM"];
                dr["sNG"] = rows["sNG"];
                dtChiTiet.Rows.InsertAt(dr, 0);


                //Tinh o con lai
                DataRow[] arrdr = _dtChiTiet.Select("sNG<>'' AND bLaHangCha='False'");
                double Tong_TC = 0, Tong_HV = 0, Tong_HN = 0, Tong_DP = 0, Tong_HM = 0, Tong_PC = 0;

                for (int i = 0; i < arrdr.Length; i++)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rTuChi"])))
                    {
                        Tong_TC += Convert.ToDouble(arrdr[i]["rTuChi"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rHienVat"])))
                    {
                        Tong_HV += Convert.ToDouble(arrdr[i]["rHienVat"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rDuPhong"])))
                    {
                        Tong_DP += Convert.ToDouble(arrdr[i]["rDuPhong"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rHangNhap"])))
                    {
                        Tong_HN += Convert.ToDouble(arrdr[i]["rHangNhap"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rHangMua"])))
                    {
                        Tong_HM += Convert.ToDouble(arrdr[i]["rHangMua"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rPhanCap"])))
                    {
                        Tong_PC += Convert.ToDouble(arrdr[i]["rPhanCap"]);
                    }
                }

                dr = _dtChiTiet.NewRow();
                dr["sMoTa"] = "Còn lại";
                dr["sTenDonVi_BaoDam"] = "";
                dr["rTuChi"] = Convert.ToDouble(rows["rPhanCap"]) - (Tong_TC);
                dr["rHienVat"] = Convert.ToDouble(rows["rHienVat"]) - (Tong_HV);
                dr["rDuPhong"] = 0;
                dr["rHangNhap"] = 0;
                dr["rHangMua"] = 0;
                dr["rPhanCap"] = 0;
                dr["bLaHangCha"] = "true";
                dtChiTiet.Rows.InsertAt(dr, 1);
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCotFixed(sLNS);
                CapNhapDanhSachMaCotSlide(sLNS, sKieuXem);
                CapNhapDanhSachMaCotThem(sLNS);
                CapNhapArrEdit(sLNS);
                CapNhatArrDuLieu();
                CapNhap_arrThayDoi();
                CapNhatArrLaHangCha();

            }
        } 
        #endregion

        #region CapNhapDanhSachMaHang
        /// <summary>
        /// Hàm cập nhập vào tham số _arrDSMaHang
        /// </summary>
        protected void CapNhapDanhSachMaHang()
        {
            _arrDSMaHang = new List<string>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                String MaHang = "";
                if (Convert.ToBoolean(R["bLaHangCha"]) == false)
                {
                    MaHang = String.Format("{0}_{1}", R["iID_MaChungTuChiTiet"], R["iID_MaMucLucNganSach"]);
                }
                _arrDSMaHang.Add(MaHang);

                _arrMaMucLucNganSach.Add(Convert.ToString(R["iID_MaMucLucNganSach"]));
            }
        } 
        #endregion

        #region CapNhapDanhSachMaCotFixed
        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
        ///         + Trường sMaCongTrinh, sTenCongTrinh của cột tiền
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCotFixed(String sLNS)
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            string[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            string[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
            string[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            string[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
                for (int i = 2; i < _dtChiTiet.Rows.Count - 1; i++)
                {
                    DataRow R = _dtChiTiet.Rows[i];
                    if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
                    {
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
                        break;
                    }
                }
            }

            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if (arrDSTruong[j] == "rTongSo" || arrDSTruong[j] == "sTNG")
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
        #endregion

        #region CapNhapDanhSachMaCotSlide
        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCotSlide(String sLNS, String MaLoai)
        {
            string strDSTruong = "sTenDonVi_BaoDam,iID_MaPhongBanDich,rTuChi,rHienVat,rHangNhap,rHangMua,rDuPhong,rPhanCap";
            string strDSTruongTieuDe = "Đơn vị,B,Tự chi, Hiện vật, Hàng nhập, Hàng mua, Dự phòng, Phân cấp";
            string strDSTruongDoRong = "200,30,130,130,130,130,130,130";
            string[] arrDSTruong = strDSTruong.Split(',');
            string[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
            string[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');

            //Phần mục lục ngân sách
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if (arrDSTruong[j] == "sNG" && sLNS.Substring(0, 1) == "8")
                {
                    _arrHienThiCot.Add(false);
                }
                else if (arrDSTruong[j] == "rHangNhap" || arrDSTruong[j] == "rHangMua" || arrDSTruong[j] == "rDuPhong" || arrDSTruong[j] == "rPhanCap")
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

            arrDSMaCot.Add("sGhiChu");
            _arrTieuDe.Add("Ghi Chú");
            _arrWidth.Add(300);
            _arrHienThiCot.Add(true);
            _arrSoCotCungNhom.Add(1);
            _arrTieuDeNhomCot.Add("");

            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        } 
        #endregion

        #region CapNhapDanhSachMaCotThem
        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCotThem(String sLNS)
        {
            String strDSTruong = "sLNS,sL,sK,iID_MaDonVi,iID_MaMucLucNganSach,sMaCongTrinh,sMauSac,sFontColor,sFontBold";
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
        #endregion

        #region CapNhapArrEdit
        /// <summary>
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhapArrEdit(String sLNS)
        {
            _arrEdit = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                bool bHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                //Không được sửa hàng cha
                if (Convert.ToBoolean(R["bLaHangCha"]))
                {
                    bHangChiDoc = true;
                }

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    bool bOChiDoc = true;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet && !_ChiDoc && !bHangChiDoc)
                        {
                            bOChiDoc = false;
                        }
                    }
                    else if (_DuocSuaChiTiet && !bHangChiDoc && arrDSMaCot[j] == "sGhiChu")
                    {
                        bOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && !bHangChiDoc && arrDSMaCot[j] == "sTenDonVi")
                    {
                        bOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && !bHangChiDoc && arrDSMaCot[j] == "sTenDonVi_BaoDam")
                    {
                        bOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && !bHangChiDoc && arrDSMaCot[j] == "iID_MaPhongBanDich")
                    {
                        bOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && !bHangChiDoc && arrDSMaCot[j] == "bPhanCap")
                    {
                        bOChiDoc = false;
                    }
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet &&
                            !_ChiDoc &&
                            !bHangChiDoc &&
                            _arrDSMaCot[j] != "rTongSo" &&
                            !_arrDSMaCot[j].EndsWith("_PhanBo") &&
                            !_arrDSMaCot[j].EndsWith("_DaCap") &&
                            !_arrDSMaCot[j].EndsWith("_ConLai") &&
                            _dtChiTiet.Columns.IndexOf('b' + _arrDSMaCot[j]) >= 0 &&
                            Convert.ToBoolean(R['b' + _arrDSMaCot[j]]))
                        {
                            bOChiDoc = false;
                        }
                    }
                    if (bOChiDoc)
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
        #endregion

        #region CapNhatArrDuLieu
        /// <summary>
        /// Hàm cập nhập mảng dữ liệu
        /// </summary>
        protected void CapNhatArrDuLieu()
        {
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count - 3; j++)
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
                else if (i == 1)
                {

                    double CL_TC = Convert.ToDouble(_dtChiTiet.Rows[1]["rTuChi"]);
                    double CL_HV = Convert.ToDouble(_dtChiTiet.Rows[1]["rHienVat"]);
                    double CL_DP = Convert.ToDouble(_dtChiTiet.Rows[1]["rDuPhong"]);

                    double CL_HN = Convert.ToDouble(_dtChiTiet.Rows[1]["rHangNhap"]);
                    double CL_HM = Convert.ToDouble(_dtChiTiet.Rows[1]["rHangMua"]);
                    double CL_PC = Convert.ToDouble(_dtChiTiet.Rows[1]["rPhanCap"]);

                    double dConLaiTC = DuToanBSChungTuChiTietModels.TongConLai(Convert.ToString(_dtBang.Rows[0]["iID_MaChungTuChiTiet"]), "rTuChi");
                    double dConLaiHV = DuToanBSChungTuChiTietModels.TongConLai(Convert.ToString(_dtBang.Rows[0]["iID_MaChungTuChiTiet"]), "rHienVat");
                    double dPhanCap = Convert.ToDouble(_dtBang.Rows[0]["rPhanCap"]);
                    double dHienVat = Convert.ToDouble(_dtBang.Rows[0]["rHienVat"]);
                    double TongCLTC = dPhanCap - dConLaiTC;
                    double TongCLHV = dHienVat - dConLaiHV;
                    if (TongCLTC < 0 || TongCLHV < 0)
                    {
                        _arrDuLieu[i].Add("#FF0000");
                        _arrDuLieu[i].Add("");
                        _arrDuLieu[i].Add("");
                    }
                    else if (TongCLTC == 0 && TongCLHV == 0)
                    {

                        _arrDuLieu[i].Add("#3399FF");
                        _arrDuLieu[i].Add("");
                        _arrDuLieu[i].Add("");
                    }
                    else
                    {
                        _arrDuLieu[i].Add("#f8e6d1");
                        _arrDuLieu[i].Add("");
                        _arrDuLieu[i].Add("");
                    }
                }
                else
                {
                    _arrDuLieu[i].Add("");
                    _arrDuLieu[i].Add("");
                    _arrDuLieu[i].Add("");
                }
            }
        } 
        #endregion

        #region CapNhatArrLaHangCha
        /// <summary>
        /// Gán cờ hàng cha
        /// </summary>
        protected void CapNhatArrLaHangCha()
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
        #endregion

    }
}