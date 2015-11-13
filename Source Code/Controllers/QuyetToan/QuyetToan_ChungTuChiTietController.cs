using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.QuyetToan
{
    public class QuyetToan_ChungTuChiTietController : Controller
    {
        //
        // GET: /QuyetToan_ChungTuChiTiet/
        public string sViewPath = "~/Views/QuyetToan/ChungTuChiTiet/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "QuyetToan_ChungTuChiTiet_Index.aspx");
        }
        [Authorize]
        public ActionResult Index_BH()
        {
            return View(sViewPath + "QuyetToan_BaoHiem_Index.aspx");
        }
        //Giải thích số tiền
        [Authorize]
        public ActionResult Index_GTST()
        {
            return View(sViewPath + "QuyetToan_GiaiThichSoTien_Dialog.aspx");
        }
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame(String MaLoai)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QTA_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaChungTu = Request.Form["QuyetToanNganSach_iID_MaChungTu"];
             MaLoai = Request.Form["QuyetToanNganSach_MaLoai"];
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["MaLoai"] = MaLoai;
            return View(sViewPath + "QuyetToan_ChungTuChiTiet_Index_DanhSach_Frame.aspx");
        }
        //Giải thích bằng lời
        [Authorize]
        public ActionResult Index_GTBL()
        {
            return View(sViewPath + "QuyetToan_GiaiThichBangLoi_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu,String MaLoai)
        {
            String TenBangChiTiet = "QTA_ChungTuChiTiet";

            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            String sTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]));
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        String MaChungTuChiTiet = arrMaHang[i].Split('_')[0];

                        if (MaChungTuChiTiet == "")
                        {
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", dtChungTu.Rows[0]["sTenPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", dtChungTu.Rows[0]["bLoaiThang_Quy"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                            String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                            DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                            //Dien thong tin cua Muc luc ngan sach
                            NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                            //Xet rieng ngan sach thuong xuyen
                            dtMucLuc.Dispose();
                        }
                        else
                        {
                            bang.GiaTriKhoa = MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].EndsWith("_ConLai") == false)
                                {
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1")
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                        }
                                    }
                                    else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }
                                }
                            }
                        }
                   
                        bang.Save();
                    }
                }
            }

            dtChungTu.Dispose();
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "QuyetToan_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "QuyetToan_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            ViewData["LoadLai"] = "";
            ViewData["MaLoai"] = MaLoai;
            return View(sViewPath + "QuyetToan_ChungTuChiTiet_Index_DanhSach_Frame.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit_BH(String iID_MaChungTu)
        {
            String TenBangChiTiet = "QTA_QuyetToanBaoHiem";

            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            

            DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String MaChungTuChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (MaChungTuChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = MaChungTuChiTiet;
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.Save();
                    }
                }
                else
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        

                        if (MaChungTuChiTiet == "")
                        {
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);

                            String iID_MaMucLucNganSach = "";// arrMaHang[i].Split('_')[1];

                            //DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                            ////Dien thong tin cua Muc luc ngan sach
                            //NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                            //Xet rieng ngan sach thuong xuyen
                            // dtMucLuc.Dispose();
                        }
                        else
                        {
                            bang.GiaTriKhoa = MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].EndsWith("_ConLai") == false)
                                {
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1")
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                        }
                                    }
                                    else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }
                                }
                            }
                        }
                        bang.Save();
                    }

                }

                dtChungTu.Dispose();
            }
            //string idAction = Request.Form["idAction"];
            //if (idAction == "1")
            //{
            //    return RedirectToAction("TuChoi", "QuyetToan_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            //}
            //else if (idAction == "2")
            //{
            //    return RedirectToAction("TrinhDuyet", "QuyetToan_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            //}
            return RedirectToAction("Index_BH", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit_GTBL(String iID_MaChungTu)
        {
            String TenBangChiTiet = "QTA_GiaiThichBangLoi";

            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);


            DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String MaChungTuChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (MaChungTuChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = MaChungTuChiTiet;
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.Save();
                    }
                }
                else
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;


                        if (MaChungTuChiTiet == "")
                        {
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                        }
                        else
                        {
                            bang.GiaTriKhoa = MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].EndsWith("_ConLai") == false)
                                {
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1")
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                        }
                                    }
                                    else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }
                                }
                            }
                        }
                        bang.Save();
                    }

                }

                dtChungTu.Dispose();
            }           
            return RedirectToAction("Index_GTBL", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GiaiThichSoTien_Submit(String ParentID, String iID_MaChungTu)
        {
            String[] arrsKyHieu = "310,320,330".Split(',');
            String iSoNgayAn_1=Request.Form[ParentID+"_iSoNgayAn_1"];
            String iSoNgayAn_2=Request.Form[ParentID+"_iSoNgayAn_2"];
            String iSoNgayAn_3=Request.Form[ParentID+"_iSoNgayAn_3"];
            String iSoNgayAn_4=Request.Form[ParentID+"_iSoNgayAn_4"];
            DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            String[] arrTruongTien = "iID_MaGiaiThichSoTien,iSiQuan,iQNCN,iCNVCQP,iHSQCS,rSiQuan,rQNCN,rCNVCQP,rHSQCS".Split(',');
            String iID_MaGiaiThichSoTien, iSiQuan, iQNCN, iCNVCQP, iHSQCS, rSiQuan, rQNCN, rCNVCQP, rHSQCS, sKyHieuDoiTuong;
            for (int i = 0; i < 3; i++)
            {                
                Bang bang = new Bang("QTA_GiaiThichSoTien");
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

                iID_MaGiaiThichSoTien=Request.Form[ParentID+"_iID_MaGiaiThichSoTien_"+arrsKyHieu[i]];
                iSiQuan = Request.Form[ParentID + "_iSiQuan_" + arrsKyHieu[i]];
                iQNCN = Request.Form[ParentID + "_iQNCN_" + arrsKyHieu[i]];
                iCNVCQP = Request.Form[ParentID + "_iCNVCQP_" + arrsKyHieu[i]];
                iHSQCS = Request.Form[ParentID + "_iHSQCS_" + arrsKyHieu[i]];
                rSiQuan = Request.Form[ParentID + "_rSiQuan_" + arrsKyHieu[i]];
                rQNCN = Request.Form[ParentID + "_rQNCN_" + arrsKyHieu[i]];
                rCNVCQP = Request.Form[ParentID + "_rCNVCQP_" + arrsKyHieu[i]];
                rHSQCS = Request.Form[ParentID + "_rHSQCS_" + arrsKyHieu[i]];
                sKyHieuDoiTuong = Request.Form[ParentID + "_sKyHieuDoiTuong_" + arrsKyHieu[i]];

                if (String.IsNullOrEmpty(iSiQuan)) iSiQuan = "0";
                if (String.IsNullOrEmpty(iQNCN)) iQNCN = "0";
                if (String.IsNullOrEmpty(iCNVCQP)) iCNVCQP = "0";
                if (String.IsNullOrEmpty(iHSQCS)) iHSQCS = "0";
                if (String.IsNullOrEmpty(rSiQuan)) rSiQuan = "0";
                if (String.IsNullOrEmpty(rQNCN)) rQNCN = "0";
                if (String.IsNullOrEmpty(rCNVCQP)) rCNVCQP = "0";
                if (String.IsNullOrEmpty(rHSQCS)) rHSQCS = "0";

                if (String.IsNullOrEmpty(iID_MaGiaiThichSoTien) == false)
                {
                    bang.GiaTriKhoa = iID_MaGiaiThichSoTien;
                }
                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                bang.CmdParams.Parameters.AddWithValue("@sKyHieuDoiTuong", sKyHieuDoiTuong);
                
                bang.CmdParams.Parameters.AddWithValue("@iSiQuan", iSiQuan);
                bang.CmdParams.Parameters.AddWithValue("@iQNCN", iQNCN);
                bang.CmdParams.Parameters.AddWithValue("@iCNVCQP", iCNVCQP);
                bang.CmdParams.Parameters.AddWithValue("@iHSQCS", iHSQCS);

                bang.CmdParams.Parameters.AddWithValue("@rSiQuan", rSiQuan);
                bang.CmdParams.Parameters.AddWithValue("@rQNCN", rQNCN);
                bang.CmdParams.Parameters.AddWithValue("@rCNVCQP", rCNVCQP);
                bang.CmdParams.Parameters.AddWithValue("@rHSQCS", rHSQCS);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.Save();
            }
            dtChungTu.Dispose();
            return RedirectToAction("Index_GTST", new { iID_MaChungTu = iID_MaChungTu });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaChungTu)
        {
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }



        [Authorize]
        public ActionResult LoadDataLuong(String iID_MaChungTu)
        {
            DataTable dtChungTuChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtChungTuChiTiet(iID_MaChungTu);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iThang = Convert.ToInt16(dtChungTu.Rows[0]["iThang_Quy"]);
            int iNamLamViec = Convert.ToInt16(dtChungTu.Rows[0]["iNamLamViec"]);
            BangLuongChiTietModels.ChuyenBangQuyetToan(iThang, iNamLamViec, User.Identity.Name, Request.UserHostAddress);
            QuyetToan_ChungTuChiTietModels.Update_TuChiTuLuong(dtChungTuChiTiet);
            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu });
        }
    }
}
