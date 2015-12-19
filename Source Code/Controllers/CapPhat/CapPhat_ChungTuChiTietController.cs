using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;


namespace VIETTEL.Controllers.CapPhat
{
    public class CapPhat_ChungTuChiTietController : Controller
    {
        public static readonly string VIEW_ROOTPATH = "~/Views/CapPhat/ChungTuChiTiet/";
        public static readonly string VIEW_CAPPHATCHITIET_INDEX_DONVI = "CapPhatChiTiet_Index_DonVi.aspx";
        public static readonly string VIEW_CAPPHATCHITIET_INDEX = "CapPhatChiTiet_Index.aspx";
        public static readonly string VIEW_CAPPHATCHITIET_DANHSACH_FRAME = "CapPhatChiTiet_Index_DanhSach_Frame.aspx";

        [Authorize]
        public ActionResult Index(String DonVi,String iID_MaCapPhat)
        {
            if (String.IsNullOrEmpty(DonVi) == false)
            {
                return View(VIEW_ROOTPATH + VIEW_CAPPHATCHITIET_INDEX_DONVI);
            }
            return View(VIEW_ROOTPATH + VIEW_CAPPHATCHITIET_INDEX);
        }
        [Authorize]
        public ActionResult CapPhatChiTiet_Frame(String iID_MaCapPhat)
        {
            return View(VIEW_ROOTPATH + VIEW_CAPPHATCHITIET_DANHSACH_FRAME);
        }
        /// <summary>
        /// Lưu chứng từ chi tiết
        /// </summary>
        /// <param name="ChiNganSach"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="DonVi"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LuuChungTuChiTiet(String ChiNganSach,String iID_MaCapPhat,String DonVi)
        {
            NameValueCollection data = CapPhat_ChungTuModels.LayThongTin(iID_MaCapPhat);

            String MaND = User.Identity.Name;
            String TenBangChiTiet = "CP_CapPhatChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idMaMucLucNganSach = Request.Form["idMaMucLucNganSach"];

            String[] arrMaMucLucNganSach = idMaMucLucNganSach.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { CapPhat_BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaCapPhatChiTiet;
            
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
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
                        //hungPX: check dieu kien ma don vi khac null
                        bool dk = false;
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrMaCot[j] == "iID_MaDonVi" )
                            {
                                if(string.IsNullOrEmpty(arrGiaTri[j]))
                                    dk = true;
                                break;
                            }
                        }
                        if (dk)
                            continue;
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
                            if (iID_MaCapPhatChiTiet == "")
                               {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);

                                String strTenPhongBan = Convert.ToString(PhongBanModels.Get_Table(Convert.ToString(data["iID_MaPhongBan"])).Rows[0]["sTen"]);
                                //Them cac tham so tu bang CP_CapPhat
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", data["iDM_MaLoaiCapPhat"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", strTenPhongBan);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", data["iID_MaTrangThaiDuyet"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaTinhChatCapThu", data["iID_MaTinhChatCapThu"]);
                                bang.CmdParams.Parameters.AddWithValue("@dNgayCapPhat", data["dNgayCapPhat"]);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.DuLieuMoi = false;
                            }
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;

                            if (iID_MaCapPhatChiTiet == "")
                            {
                                //Xác định xâu mã nối
                                String sXauNoiMa = "";
                                for (int k = 0; k < MucLucNganSachModels.arrDSTruong.Length; k++)
                                {
                                    for (int j = 0; j < arrMaCot.Length; j++)
                                    {
                                        if (arrMaCot[j] == MucLucNganSachModels.arrDSTruong[k])
                                        {
                                            sXauNoiMa += String.Format("{0}-", arrGiaTri[j]);
                                            break;
                                        }
                                    }
                                }

                                String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                                DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                                //Dien thong tin cua Muc luc ngan sach
                                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                                dtMucLuc.Dispose();
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
            string idAction = Request.Form["idAction"];
            string sLyDo = Request.Form["sLyDo"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "CapPhat_ChungTu", new { ChiNganSach = ChiNganSach, iID_MaCapPhat = iID_MaCapPhat, DonVi = DonVi, sLyDo = sLyDo });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "CapPhat_ChungTu", new { ChiNganSach = ChiNganSach, iID_MaCapPhat = iID_MaCapPhat, DonVi = DonVi, sLyDo = sLyDo });
            }
            return RedirectToAction("CapPhatChiTiet_Frame", new { iID_MaCapPhat = iID_MaCapPhat, DonVi = DonVi });
        }
        /// <summary>
        /// get_GiaTri
        /// </summary>
        /// <param name="Truong"></param>
        /// <param name="GiaTri"></param>
        /// <param name="DSGiaTri"></param>
        /// <returns></returns>
        #region Lấy 1 hang AJAX: rTongSoNamTruoc
        [Authorize]
        public JsonResult get_GiaTri(String Truong, String GiaTri, String DSGiaTri)
        {
            if (Truong == "PhanBo_DaCapPhat")
            {
                return LayGiaTriDaCapPhat(GiaTri, DSGiaTri);
            }
            return null;
        }
        /// <summary>
        /// Hàm lấy giá trị đã cấp phát của chứng từ chi tiết
        /// </summary>
        /// <param name="GiaTri"></param>
        /// <param name="DSGiaTri"></param>
        /// <returns></returns>
        private JsonResult LayGiaTriDaCapPhat(String GiaTri, String DSGiaTri)
        {
            String iID_MaCapPhat = GiaTri;
            String[] arrDSGiaTri = DSGiaTri.Split(',');
            String iID_MaDonVi = arrDSGiaTri[0];
            String iID_MaMucLucNganSach = arrDSGiaTri[1];

            NameValueCollection data = CapPhat_ChungTuModels.LayThongTin(iID_MaCapPhat);
            int iNamLamViec = Convert.ToInt32(data["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(data["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(data["iID_MaNamNganSach"]);
            String iDM_MaLoaiCapPhat = Convert.ToString(data["iDM_MaLoaiCapPhat"]);
            String iID_MaTinhChatCapThu = Convert.ToString(data["iID_MaTinhChatCapThu"]);
            String sDSLNS = Convert.ToString(data["sDSLNS"]);

            Object obNgayCapPhat=data["dNgayCapPhat"];
            DateTime dNgayCapPhat = Convert.ToDateTime(obNgayCapPhat);
            int iThangCapPhat = dNgayCapPhat.Month;
            Object item = new
            {
                data = CapPhat_BangDuLieu.LayGiaTri_ChiTieu_DaCap(iDM_MaLoaiCapPhat, iID_MaTinhChatCapThu, sDSLNS, iID_MaMucLucNganSach, iID_MaDonVi, iNamLamViec, Convert.ToString(dNgayCapPhat), iID_MaNguonNganSach, iID_MaNamNganSach)
            };

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
