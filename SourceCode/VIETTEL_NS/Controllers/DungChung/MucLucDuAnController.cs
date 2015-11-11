using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;

namespace VIETTEL.Controllers.DungChung
{
    public class MucLucDuAnController : Controller
    {
        //
        // GET: /MucLucDuAn/
        public string sViewPath = "~/Views/DungChung/MucLucDuAn/";

        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "MucLucDuAnGrid_Index.aspx");
        }
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame()
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MucLucDuAn", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
           
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            String[] arrdstruong = "iID_MaDonVi,iLoai,iThamQuyen,iTinhChat,iNhom,sTen,bHoanThanh".Split(',');
            for (int i = 0; i < arrdstruong.Length; i++)
            {
                arrGiaTriTimKiem.Add(arrdstruong[i], Request.QueryString[arrdstruong[i]]);
            }

            ViewData["arrGiaTriTimKiem"] = arrGiaTriTimKiem;
            return View(sViewPath + "MucLucDuAnGrid_Index_frame.aspx");
            //return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, LoadLai = "1" });
        }
        [Authorize]
        public ActionResult SearchSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDuAn", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                String sTen = Request.Form[ParentID + "_sTen"];
                String MaThamQuyen = Request.Form[ParentID + "_MaThamQuyen"];
                String MaLoaiCT = Request.Form[ParentID + "_MaLoaiCT"];
                String MaTinhChatCT = Request.Form[ParentID + "_MaTinhChatCT"];
                String bHoanThanh = Request.Form[ParentID + "_bHoanThanh"];
                String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
                ViewData["sTen"] = sTen;
                ViewData["MaThamQuyen"] = MaThamQuyen;
                ViewData["MaLoaiCT"] = MaLoaiCT;
                ViewData["MaTinhChatCT"] = MaTinhChatCT;
                ViewData["bHoanThanh"] = bHoanThanh;
                ViewData["iID_MaDonVi"] = iID_MaDonVi;

                return View(sViewPath + "MucLucDuAn_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDuAn", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(Code))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaDanhMucDuAn"] = Code;
                return View(sViewPath + "MucLucDuAn_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDuAn", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                NameValueCollection arrLoi = new NameValueCollection();
                String _iID_MaDanhMucDuAn = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMucDuAn"]);

                String _iID_LoaiDuAn = Convert.ToString(Request.Form[ParentID + "_iID_LoaiDuAn"]);
                String iLoaiDuAn = Convert.ToString(CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", _iID_LoaiDuAn, "sTenKhoa"));
                String _iID_TinhChatDuAn = Convert.ToString(Request.Form[ParentID + "_iID_TinhChatDuAn"]);
                String iTinhChatDuAn = Convert.ToString(CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", _iID_TinhChatDuAn, "sTenKhoa"));
                String _iID_MaThamQuyen = Convert.ToString(Request.Form[ParentID + "_iID_MaThamQuyen"]);
                String iMaThamQuyen = Convert.ToString(CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", _iID_MaThamQuyen, "sTenKhoa"));
                String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
                String MaND = User.Identity.Name;
                String iNamLamViec = ReportModels.LayNamLamViec(MaND);
                String sMaCongTrinh = iNamLamViec.Substring(2, 2) + iID_MaDonVi;
                int sMaxMaCongTrinh = MucLucDuAnModels.getMax(sMaCongTrinh);
                //Them mới
                if (_iID_MaDanhMucDuAn == Convert.ToString(Guid.Empty) || _iID_MaDanhMucDuAn == "")
                {

                    sMaxMaCongTrinh += 1;
                    String sMaCongTrinh_string = sMaxMaCongTrinh.ToString();
                    String Chuoi0 = "";
                    for (int i = 0; i < 5 - sMaCongTrinh_string.Length; i++)
                    {
                        Chuoi0 += "0";
                    }
                    sMaCongTrinh_string = Chuoi0 + sMaCongTrinh_string;
                    sMaCongTrinh = sMaCongTrinh + sMaCongTrinh_string;
                }
                    //sửa
                else
                {

                    String sMaCongTrinh_string = sMaxMaCongTrinh.ToString();
                    String Chuoi0 = "";
                    for (int i = 0; i < 5 - sMaCongTrinh_string.Length; i++)
                    {
                        Chuoi0 += "0";
                    }
                    sMaCongTrinh_string = Chuoi0 + sMaCongTrinh_string;
                    sMaCongTrinh = sMaCongTrinh + sMaCongTrinh_string;
                    sMaxMaCongTrinh = MucLucDuAnModels.getMax(sMaCongTrinh);
                    //mã thay đổi
                    if (sMaxMaCongTrinh <= 0)
                    {
                        sMaCongTrinh = iNamLamViec.Substring(2, 2) + iID_MaDonVi + iLoaiDuAn + iTinhChatDuAn +
                                         iMaThamQuyen;
                        sMaxMaCongTrinh = MucLucDuAnModels.getMax(sMaCongTrinh);
                        sMaxMaCongTrinh += 1;
                         sMaCongTrinh_string = sMaxMaCongTrinh.ToString();
                         Chuoi0 = "";
                        for (int i = 0; i < 5 - sMaCongTrinh_string.Length; i++)
                        {
                            Chuoi0 += "0";
                        }
                        sMaCongTrinh_string = Chuoi0 + sMaCongTrinh_string;
                        sMaCongTrinh = sMaCongTrinh + sMaCongTrinh_string;
                    }
                }
                String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
                Boolean DuLieuMoi = false;
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                    DuLieuMoi = true;

                if (_iID_LoaiDuAn == Convert.ToString(Guid.Empty) || _iID_LoaiDuAn == "")
                {
                    arrLoi.Add("err_iID_LoaiDuAn", "Bạn chưa chọn loại dự án!");
                }
                if (_iID_TinhChatDuAn == Convert.ToString(Guid.Empty) || _iID_TinhChatDuAn == "")
                {
                    arrLoi.Add("err_iID_TinhChatDuAn", "Bạn chưa chọn tính chất dự án!");
                }
                if (_iID_MaThamQuyen == Convert.ToString(Guid.Empty) || _iID_MaThamQuyen == "")
                {
                    arrLoi.Add("err_iID_MaThamQuyen", "Bạn chưa chọn thẩm quyền dự án!");
                }


                if (sTen == string.Empty || sTen == "")
                {
                    arrLoi.Add("err_sTen", "Bạn chưa nhập tên dự án!");
                }


                if (arrLoi.Count > 0)
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["iID_MaDanhMucDuAn"] = Code;
                    return View(sViewPath + "MucLucDuAn_Edit.aspx");
                }
                else
                {
                    Bang bang = new Bang("NS_MucLucDuAn");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    if (_iID_MaDanhMucDuAn == Convert.ToString(Guid.Empty) || _iID_MaDanhMucDuAn == "")
                    {
                        bang.GiaTriKhoa = Guid.NewGuid();
                    }
                    else
                        bang.GiaTriKhoa = _iID_MaDanhMucDuAn;
                    bang.CmdParams.Parameters.AddWithValue("@sMaCongTrinh", sMaCongTrinh);
                    bang.Save();
                    ViewData["DuLieuMoi"] = "1";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit()
        {

            String MaND = User.Identity.Name;
            String TenBangChiTiet = "NS_MucLucDuAn";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idMaMucLucNganSach = Request.Form["idMaMucLucNganSach"];
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }

            String[] arrMaMucLucNganSach = idMaMucLucNganSach.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { CapPhat_BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaCapPhatChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String iNamLamViec = ReportModels.LayNamLamViec(MaND);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    iID_MaCapPhatChiTiet = arrMaHang[i].Split('_')[0];
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaCapPhatChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.Save();
                                                   }
                    }
                    else
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                        Boolean okCoThayDoi = false;
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {

                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                                {
                                    if (arrGiaTri[j] == "")
                                    {
                                        if (iID_MaCapPhatChiTiet == "")
                                        {
                                            okCoThayDoi = false;
                                            break;
                                        }
                                        else
                                        {
                                            okCoThayDoi = true;
                                            break;

                                        }
                                    }

                                }
                                okCoThayDoi = true;
                            }

                        }
                        if (okCoThayDoi)
                        {
                             Bang bang = new Bang(TenBangChiTiet);
                             string iID_MaDonVi = "";
                             for (int j = 0; j < arrMaCot.Length; j++)
                             {
                                 if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                                 {
                                     iID_MaDonVi = arrGiaTri[j];
                                 }
                             }

                            if (iID_MaCapPhatChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                String sMaCongTrinh = iNamLamViec.Substring(2, 2) + iID_MaDonVi;
                                int sMaxMaCongTrinh = MucLucDuAnModels.getMax(sMaCongTrinh);
                                sMaxMaCongTrinh += 1;
                                String sMaCongTrinh_string = sMaxMaCongTrinh.ToString();
                                String Chuoi0 = "";
                                for (int c = 0; c < 5 - sMaCongTrinh_string.Length; c++)
                                {
                                    Chuoi0 += "0";
                                }
                                sMaCongTrinh_string = Chuoi0 + sMaCongTrinh_string;
                                sMaCongTrinh = sMaCongTrinh + sMaCongTrinh_string;
                                bang.CmdParams.Parameters.AddWithValue("@sMaCongTrinh", sMaCongTrinh);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.DuLieuMoi = false;

                            }
                                                       bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                                                   


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
                                        else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
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
            }
            return RedirectToAction("ChungTuChiTiet_Frame");
        }

        public Boolean CheckMaDonVi(String MaDonVi)
        {
            Boolean vR = false;
            DataTable dt = MucLucDuAnModels.getMaDuAn(MaDonVi);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }

        [Authorize]
        public ActionResult Delete(String Code)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDuAn", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("NS_MucLucDuAn");
                bang.GiaTriKhoa = Code;
                bang.Delete();
                return View(sViewPath + "MucLucDuAn_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }


    }
}
