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
    public class TCDN_DuAn_BangDuLieu:BangDuLieu
    {
        private static string dsTruong = "sTen";
        private static string dsTruongTieuDe = "Tên dự án";
        private static string dsTruongDoRong = "400";
        private string[] arrDSTruongTieuDe = dsTruongTieuDe.Split(',');
        private string[] arrDSTruong = dsTruong.Split(',');
        private string[] arrDSTruongDoRong = dsTruongDoRong.Split(',');

        // Hàm khởi tạo
        public TCDN_DuAn_BangDuLieu(string iID_MaDoanhNghiep)
        {
            this._iID_Ma = string.Empty;
            this._MaND = string.Empty;
            this._IPSua = string.Empty;

            _dtBang = null;
            _ChiDoc = false;
            _DuocSuaChiTiet = true;
            _dtChiTiet = TCDN_HoSo_DoanhNghepModels.GetDuAn(iID_MaDoanhNghiep);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu();
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// </summary>
        private void DienDuLieu()
        {
            if (_arrDuLieu==null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrType_Rieng();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
            }
        }

        private void CapNhap_arrDuLieu()
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

        private void CapNhap_arrEdit()
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

                    //Cot tien
                    if (_DuocSuaChiTiet &&
                            _ChiDoc == false &&
                            okHangChiDoc == false)
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

        private void CapNhap_arrType_Rieng()
        {
            //Xac dinh kieu truong nhap du lieu
            _arrType = new List<string>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    _arrType.Add("2");
                }
                else if (_arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    //Nhap Kieu so
                    _arrType.Add("0");
                }
                else
                {
                    //Nhap kieu xau
                    _arrType.Add("0");
                }
            }
        }


       /// <summary>
       /// Hàm thêm các cột thêm của bảng
       /// </summary>
        private void CapNhapDanhSachMaCot_Them()
        {
            
        }

        private void CapNhapDanhSachMaCot_Slide()
        {
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                _arrDSMaCot.Add(arrDSTruong[i]);
                _arrTieuDe.Add(arrDSTruongTieuDe[i]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[i]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        private void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm cập nhận vào tham số _arrDSMaHang
        /// </summary>
        private void CapNhapDanhSachMaHang()
        {
            _arrDSMaHang = new List<string>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                String MaHang = Convert.ToString(R["iID_MaDuAn"]);
                _arrDSMaHang.Add(MaHang);
            }
        }
    }
}