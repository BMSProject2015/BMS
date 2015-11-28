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

namespace VIETTEL.Models.DuToanBS
{
    /// <summary>
    /// Lớp DuToanBS_BangDuLieu cho phần bảng của Phân bổ chỉ tiêu
    /// </summary>
    public class DuToanBS_BangDuLieu : BangDuLieu
    {
        protected int _iNamLamViec;
        private List<List<Double>> _arrGiaTriNhom = new List<List<Double>>();
        private List<int> _arrChiSoNhom = new List<int>();
        private List<String> _arrMaMucLucNganSach = new List<String>();
        private DataTable _dtDonVi = null;
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
        public String strMaMucLucNganSach
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
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        ///sLNS,sL,sK,sM,sTM,sTTM
        public DuToanBS_BangDuLieu(String maChungTu, Dictionary<String, String> dicGiaTriTimKiem, String maND, String IPSua, String sLNS, String MaLoai, String iLoai, String iChiTapTrung)
        {
            this._iID_Ma = maChungTu;
            this._MaND = maND;
            this._IPSua = IPSua;
            _dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(_MaND);
            string SQL;
            SqlCommand cmd;

            //Chứng từ TLTH.
            if (iLoai == "1")
            {
                SQL = "SELECT * FROM DTBS_ChungTu_TLTH WHERE iID_MaChungTu_TLTH=@iID_MaChungTu AND iTrangThai=1";
            }
            //Chung tu nganh ky thuat
            else if (iLoai == "4")
            {
                SQL = "SELECT * FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTu AND iTrangThai=1";
                sLNS = "1040100";
            }
            else
            {
                SQL = "SELECT * FROM DTBS_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1";
            }

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            _iNamLamViec = Convert.ToInt32(_dtBang.Rows[0]["iNamLamViec"]);
            string iID_MaNguoiDungTao = Convert.ToString(_dtBang.Rows[0]["sID_MaNguoiDungTao"]);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);
            string iKyThuat = Convert.ToString(_dtBang.Rows[0]["iKyThuat"]);
            // String sLNS = Convert.ToString(_dtBang.Rows[0]["sDSLNS"]);
            if (sLNS != "1040100")
            {
                bool ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(DuToanBSModels.iID_MaPhanHe, maND,
                                                                                        iID_MaTrangThaiDuyet);
                _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanBSModels.iID_MaPhanHe, maND);
                //Trolytonghop duoc sua chung tu do minh tạo
                bool checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(maND);
                bool CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(DuToanBSModels.iID_MaPhanHe, iID_MaTrangThaiDuyet);
                if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
                {
                    ND_DuocSuaChungTu = true;
                    _DuocSuaChiTiet = true;
                }
                if (ND_DuocSuaChungTu == false)
                {
                    _ChiDoc = true;
                }
                //duoc sua khi o trang thai moi tao
                if (CheckTrangThaiDuyetMoiTao == false) _DuocSuaChiTiet = false;
                if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(DuToanBSModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                {
                    _ChiDoc = true;
                }
                //Trang thai tu choi
                if (maND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet))
                {
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }

            }
            //bao dam
            else
            {
                bool ND_DuocSuaChungTu = false;
                bool CheckTrangThaiDuyetMoiTao = false;
                if (iKyThuat == "1")
                {
                    ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, maND,iID_MaTrangThaiDuyet);
                    _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeChiTieu, maND);
                    CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet);
                }
                else
                {
                    ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(DuToanBSModels.iID_MaPhanHe, maND,iID_MaTrangThaiDuyet);
                    _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanBSModels.iID_MaPhanHe, maND);
                    CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet);
                }

                //Trolytonghop duoc sua chung tu do minh tạo
                bool checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(maND);

                if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
                {
                    ND_DuocSuaChungTu = true;
                    _DuocSuaChiTiet = true;
                }
                if (ND_DuocSuaChungTu == false)
                {
                    _ChiDoc = true;
                }
                //duoc sua khi o trang thai moi tao
                if (CheckTrangThaiDuyetMoiTao == false) _DuocSuaChiTiet = false;

                if (iKyThuat == "1")
                {
                    //Trang thai tu choi
                    if (maND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet))
                    {
                        _DuocSuaChiTiet = true;
                        _ChiDoc = false;
                    }
                    //Tro ly tong hop dc sua chung tu
                    if (checkTroLyTongHop && iID_MaTrangThaiDuyet == DuToanBS_ChungTuChiTietModels.iMaTrangThaiDuyetKT)
                    {
                        ND_DuocSuaChungTu = true;
                        _DuocSuaChiTiet = true;
                    }

                }
                else
                {
                    //Trang thai tu choi
                    if (maND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet))
                    {
                        _DuocSuaChiTiet = true;
                        _ChiDoc = false;
                    }
                }
            }
            //phần nhaapk chi tâp chng
            if (iChiTapTrung == "1")
            {
                _dtChiTiet = DuToanBS_ChungTuChiTietModels.GetChungTuChiTiet_ChiTapTrung(_iID_Ma, dicGiaTriTimKiem, maND, sLNS);
                _DuocSuaChiTiet = true;
            }
            else
            {
                //phan nhap ky thuat lan 2
                if (iLoai == "4")
                {
                    _dtChiTiet = DuToanBS_ChungTuChiTietModels.GetChungTuChiTietLan2(_iID_Ma, dicGiaTriTimKiem, maND, sLNS);
                    _DuocSuaChiTiet = true;
                }
                else
                {
                    _dtChiTiet = DuToanBS_ChungTuChiTietModels.LayChungTuChiTiet(_iID_Ma, dicGiaTriTimKiem, maND, sLNS);
                }
            }

            ThemHangTongCong();

            _dtChiTiet_Cu = _dtChiTiet.Copy();
            if (iLoai == "1") MaLoai = "DuToan";
            DienDuLieu(sLNS, MaLoai, iLoai, iKyThuat, iChiTapTrung);
        }
        protected void CapNhapHangTongCong(String iLoai = "",String iChiTapTrung="")
        {
            String strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            String[] arrDSTruongTien = strDSTruongTien.Split(',');


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
                        double S, S_PhanBo, S_DaCap, S_ConLai;
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

            DataRow[] arrdr = _dtChiTiet.Select("sNG<>'' AND sLNS<>'1020100' ");
            if (iChiTapTrung == "1")
            {
                arrdr = _dtChiTiet.Select("sNG<>'' ");
            }
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
            if (iLoai == "4")
            {
                dr["sMoTa"] = "TỔNG CỘNG BẢO ĐẢM";
            }
            else
            {
                dr["sMoTa"] = "TỔNG CỘNG";
            }
            dr["bLaHangCha"] = true;
            _dtChiTiet.Rows.Add(dr);
        }
        protected void ThemHangTongCong()
        {
            ////Thêm hàng tổng cộng 
            //DataRow R = _dtChiTiet.NewRow();
            //_dtChiTiet.Rows.Add(R);

            //for (int j = 0; j < _dtChiTiet.Columns.Count; j++)
            //{
            //    String TenCot = _dtChiTiet.Columns[j].ColumnName;
            //    if (TenCot.StartsWith("r"))
            //    {
            //        Double S = 0;
            //        for (int i = 0; i < _dtChiTiet.Rows.Count - 1; i++)
            //        {
            //            S += Convert.ToDouble(_dtChiTiet.Rows[i][TenCot]);
            //        }
            //        R[TenCot] = S;
            //    }

            //}
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(String sLNS, String MaLoai, String iLoai, String iKyThuat,String iChiTapTrung)
        {
            String iID_MaPhongBan = "";
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(_MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                dtPhongBan.Dispose();
            }
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            if (_arrDuLieu == null)
            {
                if (!String.IsNullOrEmpty(sLNS))
                {
                    if (sLNS.Substring(0, 1) == "8")
                    {
                        int count = dtChiTiet.Rows.Count - 1;
                        for (int i = count; i >= 0; i--)
                        {
                            if (Convert.ToString(dtChiTiet.Rows[i]["bLaHangCha"]) == "True")
                            {
                                _dtChiTiet.Rows.RemoveAt(i);
                            }
                        }
                    }
                }
                //Neu la form chi tap trung
                if (iChiTapTrung == "1")
                {
                    CapNhapHangTongCong(iLoai, iChiTapTrung);
                    int count = dtChiTiet.Rows.Count - 1;
                    bool ok = true;
                    for (int i = count; i >= 0; i--)
                    {
                        ok = true;
                        //neu ma phong ban nguoi dung khac ma phong ban tao
                        if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rTuChi"])) || Convert.ToString(_dtChiTiet.Rows[i]["rTuChi"])=="0")
                        {
                            ok = false;
                        }


                        if (ok == false)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }
                    }
                  
                }
                else
                {
                    if (MaLoai == "DuToan")
                    {
                        int count = dtChiTiet.Rows.Count - 1;
                        bool ok = true;
                        CapNhapHangTongCong(iLoai, iChiTapTrung);
                        for (int i = count; i >= 0; i--)
                        {
                            ok = false;
                            for (int j = 0; j < arrDSTruongTien.Length; j++)
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                                {
                                    ok = false;
                                }
                                else if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                                {
                                    ok = true;
                                }


                            }
                            //neu ma phong ban nguoi dung khac ma phong ban tao
                            if (iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBan"]) && iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBanDich"]) && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                            {
                                ok = false;
                            }
                            //Loai bo het dong khong co du lieu
                            if (ok == false)
                            {
                                _dtChiTiet.Rows.RemoveAt(i);
                            }

                        }

                    }
                    else if (MaLoai == "ChuaDuToan")
                    {
                        int count = dtChiTiet.Rows.Count - 1;
                        bool ok = true;

                        for (int i = count; i >= 0; i--)
                        {
                            ok = false;
                            for (int j = 0; j < arrDSTruongTien.Length; j++)
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                                {
                                    ok = false;
                                }
                                else if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
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
                        CapNhapHangTongCong(iLoai, iChiTapTrung);
                    }
                    else if (MaLoai == "PhanCap")
                    {
                        int count = dtChiTiet.Rows.Count - 1;
                        bool ok = true;
                        CapNhapHangTongCong(iLoai, iChiTapTrung);
                        for (int i = count; i >= 0; i--)
                        {
                            ok = false;
                            for (int j = 0; j < arrDSTruongTien.Length; j++)
                            {
                                if (arrDSTruongTien[j] == "rPhanCap" || arrDSTruongTien[j] == "rHienVat")
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                                    {
                                        ok = false;
                                    }
                                    else if (
                                        (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) &&
                                         Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                                    {
                                        ok = true;
                                    }
                                }
                            }
                            // Nếu ngân sách ngành kỹ thuật thì lọc mã phòng ban
                            if (Convert.ToBoolean(_dtBang.Rows[0]["iKyThuat"]) == true)
                            {

                                //neu ma phong ban nguoi dung khac ma phong ban tao
                                if (iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBanDich"]) && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                                {
                                    ok = false;
                                }
                            }
                            else
                            {
                                //if (iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBan"]) && iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBanDich"]) && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                                //{
                                //    ok = false;
                                //}
                            }
                            //Loai bo het dong khong co du lieu
                            if (ok == false)
                            {
                                _dtChiTiet.Rows.RemoveAt(i);
                            }

                        }
                        count = _dtChiTiet.Rows.Count;
                        _dtChiTiet.Rows.RemoveAt(count - 1);
                        CapNhapHangTongCong(iLoai, iChiTapTrung);
                    }

                    else
                    {
                        int count = dtChiTiet.Rows.Count - 1;
                        bool ok = true;
                        for (int i = count; i >= 0; i--)
                        {
                            ok = true;
                            //neu ma phong ban nguoi dung khac ma phong ban tao
                            if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBan"])) && iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBan"]) && iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBanDich"]) && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                            {
                                ok = false;
                            }


                            if (ok == false)
                            {
                                _dtChiTiet.Rows.RemoveAt(i);
                            }
                        }
                        CapNhapHangTongCong(iLoai, iChiTapTrung);
                    }
                }//end ko phai chi tap trung
                if (iLoai == "4")
                {
                    DataTable dtCT = _dtBang;

                    DataRow rows = dtCT.Rows[0];


                    DataRow dr = _dtChiTiet.NewRow();
                    dr["sMoTa"] = "Tổng ngành kỹ thuật phân sang";
                    dr["sTenDonVi"] = "";
                    dr["sTenDonVi_BaoDam"] = "";
                    dr["rTuChi"] = Convert.ToDecimal(rows["rTuChi"]);
                    dr["rHienVat"] = Convert.ToDecimal(rows["rHienVat"]);
                    dr["bLaHangCha"] = "true";
                    dr["sLNS"] = rows["sLNS"];
                    dr["sL"] = rows["sL"];
                    dr["sK"] = rows["sK"];
                    dr["sM"] = rows["sM"];
                    dr["sTM"] = rows["sTM"];
                    dr["sTTM"] = rows["sTTM"];
                    dr["sNG"] = rows["sNG"];
                    dr["brTuChi"] = true;
                    dr["brHienVat"] = true;
                    dr["brTonKho"] = true;
                    dr["brDuPhong"] = true;
                    dr["brPhanCap"] = true;
                    dr["brHangNhap"] = true;
                    dr["brHangMua"] = true;
                    dr["bsTenCongTrinh"] = false;
                    dr["brNgay"] = false;
                    dr["brSoNguoi"] = false;
                    dr["brChiTaiKhoBac"] = false;
                    dr["brChiTapTrung"] = false;
                    dtChiTiet.Rows.InsertAt(dr, 0);


                }


                String SQL = "UPDATE DTBS_ChungTu SET MaLoai='" + MaLoai + "' WHERE iID_MaChungTu='" + _iID_Ma + "'";
                SqlCommand cmd = new SqlCommand(SQL);
                Connection.UpdateDatabase(cmd);
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed(sLNS,iChiTapTrung);
                CapNhapDanhSachMaCot_Slide(sLNS, MaLoai, iLoai, iKyThuat, iChiTapTrung);
                CapNhapDanhSachMaCot_Them(sLNS);
                CapNhap_arrEdit(sLNS, iLoai);
                CapNhap_arrDuLieu(MaLoai,iChiTapTrung);
                CapNhap_arrThayDoi();
                CapNhap_arrLaHangCha();

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
                String MaHang = "";
                if (Convert.ToBoolean(R["bLaHangCha"]) == false)
                {
                    MaHang = String.Format("{0}_{1}", R["iID_MaChungTuChiTiet"], R["iID_MaMucLucNganSach"]);
                }

                _arrDSMaHang.Add(MaHang);

                _arrMaMucLucNganSach.Add(Convert.ToString(R["iID_MaMucLucNganSach"]));
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
        ///         + Trường sMaCongTrinh, sTenCongTrinh của cột tiền
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed(String sLNS, String iChiTapTrung)
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
                for (int i = 0; i < _dtChiTiet.Rows.Count - 1; i++)
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
            if (iChiTapTrung == "1")
            {
                _arrCotTienDuocHienThi["rTuChi"] = true;
                _arrDSMaCot.Add("rTuChi");
                _arrTieuDe.Add("Chỉ tiêu");
                _arrWidth.Add(Convert.ToInt32(130));
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
        protected void CapNhapDanhSachMaCot_Slide(String sLNS, String MaLoai, String iLoai, String iKyThuat,String iChiTapTrung)
        {
            String strDSTruong = "";
            String strDSTruongTieuDe = "";
            String strDSTruongDoRong = "";


            strDSTruong = "sTenDonVi";
            strDSTruongTieuDe = "Đơn vị";
            strDSTruongDoRong = "200";
            //nêu loai ngan sach bao dam
            if (sLNS == "1040100")
            {

                strDSTruong = "sTenDonVi_BaoDam,iID_MaPhongBanDich";
                strDSTruongTieuDe = "Đơn vị,B";
                strDSTruongDoRong = "200,30";
            }
            if (iChiTapTrung == "1")
            {
                strDSTruong = "rChiTapTrung";
                strDSTruongTieuDe = "Chi Tập trung";
                strDSTruongDoRong = "130";
            }

            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
            String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');

            String[] arrDSTruongTien = DuToanBSModels.strDSTruongTien_Full.Split(',');
            String strDSTruongTienTieuDe_Full = "";

            strDSTruongTienTieuDe_Full = DuToanBSModels.strDSTruongTienTieuDe_Full;

            String[] arrDSTruongTienTieuDe = strDSTruongTienTieuDe_Full.Split(',');
            String[] arrDSTruongTienDoRong = DuToanBSModels.strDSTruongTienDoRong_Full.Split(',');

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
                else
                    _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            //Phần tiền: Bỏ qua trường sMaCongTrinh, sTenCongTrinh
            if (iChiTapTrung == "1")
            {
                
            }
            else
            {
                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    if (_arrCotTienDuocHienThi[arrDSTruongTien[j]])
                    {
                        _arrHienThiCot.Add(true);
                    }
                    else
                    {
                        _arrHienThiCot.Add(false);
                    }
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
            }

            //ngan sach bao dam- cac nganh phan cap

            if (MaLoai == "PhanCap")
            {
                arrDSMaCot.Add("bPhanCap");
                _arrTieuDe.Add("Phân cấp đi");
                _arrWidth.Add(100);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            if (iLoai == "4")
            {
                arrDSMaCot.Add("bPhanCap");
                _arrTieuDe.Add("Phân cấp đi");
                _arrWidth.Add(100);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            //Nếu là loại ngân sách 1020500 thêm cot ghi chu
            if (sLNS == "1020500" || iKyThuat == "1")
            {
                arrDSMaCot.Add("sGhiChu");
                _arrTieuDe.Add("Ghi Chú");
                _arrWidth.Add(300);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            //Them cot duyet
            if (CoCotDuyet)
            {
                ////Cột đồng ý
                //_arrDSMaCot.Add("bDongY");
                //if (_ChiDoc)
                //{
                //    _arrTieuDe.Add("<div class='check'></div>");
                //}
                //else
                //{
                //    _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
                //}
                //_arrWidth.Add(20);
                //_arrHienThiCot.Add(true);
                //_arrSoCotCungNhom.Add(1);
                //_arrTieuDeNhomCot.Add("");
                ////Cột Lý do
                //_arrDSMaCot.Add("sLyDo");
                //_arrTieuDe.Add("Nhận xét");
                //_arrWidth.Add(200);
                //_arrHienThiCot.Add(true);
                //_arrSoCotCungNhom.Add(1);
                //_arrTieuDeNhomCot.Add("");
            }

            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them(String sLNS)
        {
            String strDSTruong = "";


            strDSTruong =
                "sLNS,sL,sK,iID_MaDonVi,iID_MaMucLucNganSach,sMaCongTrinh,sMauSac,sFontColor,sFontBold";



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
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhap_arrEdit(String sLNS, String iLoai)
        {
            _arrEdit = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                //neu phan cap lan 2, dong so 1 lan ngan sach su dung se ko duoc sua
                if (iLoai == "4")
                {
                    if (i == 1)
                    {
                        okHangChiDoc = true;
                    }
                }
                if (Convert.ToBoolean(R["bLaHangCha"]))
                {
                    okHangChiDoc = true;
                }

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
                    //neu la cop phan cap di cua ngan sach 1040100 thi dc phep chọn
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "sGhiChu")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "sTenDonVi")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "sTenDonVi_BaoDam")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "iID_MaPhongBanDich")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "bPhanCap")
                    {
                        okOChiDoc = false;
                    }
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet &&
                                _ChiDoc == false &&
                                okHangChiDoc == false &&
                                _arrDSMaCot[j] != "rTongSo" &&
                                _arrDSMaCot[j].EndsWith("_PhanBo") == false &&
                                _arrDSMaCot[j].EndsWith("_DaCap") == false &&
                                _arrDSMaCot[j].EndsWith("_ConLai") == false &&
                                _dtChiTiet.Columns.IndexOf('b' + _arrDSMaCot[j]) >= 0 &&
                                Convert.ToBoolean(R['b' + _arrDSMaCot[j]]))
                        {
                            okOChiDoc = false;
                        }
                    }

                    if (_arrDSMaCot[j] == "bPhanCap")
                    {
                        okOChiDoc = false;
                        _ChiDoc = false;
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
        protected void CapNhap_arrDuLieu(String MaLoai,String iChiTapTrung)
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
                else
                {

                    if (MaLoai == "PhanCap" && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                    {
                        double dConLaiTC = DuToanBS_ChungTuChiTietModels.dTongConLai(Convert.ToString(_dtChiTiet.Rows[i]["iID_MaChungTuChiTiet"]), "rTuChi");
                        double dConLaiHV = DuToanBS_ChungTuChiTietModels.dTongConLai(Convert.ToString(_dtChiTiet.Rows[i]["iID_MaChungTuChiTiet"]), "rHienVat");
                        double dPhanCap = Convert.ToDouble(_dtChiTiet.Rows[i]["rPhanCap"]);
                        double dHienVat = Convert.ToDouble(_dtChiTiet.Rows[i]["rHienVat"]);
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
                    else if (iChiTapTrung == "1" &&  Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                    {
                        double dChiTieu = 0;
                        if(!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rTuChi"])))
                            dChiTieu=Convert.ToDouble(_dtChiTiet.Rows[i]["rTuChi"]);
                        double dChiTapTrung = 0;
                        if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["rChiTapTrung"])))
                            dChiTapTrung = Convert.ToDouble(_dtChiTiet.Rows[i]["rChiTapTrung"]);
                        if (dChiTapTrung - dChiTieu > 0)
                        {
                            _arrDuLieu[i].Add("#FF0000");
                            _arrDuLieu[i].Add("");
                            _arrDuLieu[i].Add("");
                        }
                        else
                        {
                            _arrDuLieu[i].Add("");
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
        }
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

    }
}