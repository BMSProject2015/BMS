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
    public class CapPhat_ChungTuChiTiet_CucController : Controller
    {
        //
        // GET: /CapPhat_ChungTuChiTiet/
        public string sViewPath = "~/Views/CapPhat/ChungTuChiTiet_Cuc/";
        [Authorize]
        public ActionResult Index(String DonVi)
        {
          
            return View(sViewPath + "CapPhatChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ChiNganSach,String iID_MaCapPhat,String DonVi)
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
                iID_MaCapPhatChiTiet = arrMaHang[i];
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

                            //Them cac tham so tu bang CP_CapPhat
                            bang.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", data["iDM_MaLoaiCapPhat"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", data["iID_MaTrangThaiDuyet"]);
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

                            //Dien thong tin cua Muc luc ngan sach
                            DataTable dtMucLucNganSach = MucLucNganSachModels.dt_ChiTiet_TheoDSTruong(sXauNoiMa.Split('-'));
                            if (dtMucLucNganSach.Rows.Count == 1)
                            {
                                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLucNganSach.Rows[0], bang.CmdParams.Parameters);
                            }
                            else
                            {
                                bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                            }
                            dtMucLucNganSach.Dispose();
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
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "CapPhat_ChungTu_Cuc", new { ChiNganSach = ChiNganSach, iID_MaCapPhat = iID_MaCapPhat, DonVi = DonVi });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "CapPhat_ChungTu_Cuc", new { ChiNganSach = ChiNganSach, iID_MaCapPhat = iID_MaCapPhat, DonVi = DonVi });
            }
            return RedirectToAction("Index", new { iID_MaCapPhat = iID_MaCapPhat,DonVi = DonVi });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaCapPhat,String DonVi)
        {
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Index", new { iID_MaCapPhat = iID_MaCapPhat, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG, DonVi = DonVi });
        }

        #region Lấy 1 hang AJAX: rTongSoNamTruoc
        [Authorize]
        public JsonResult get_GiaTri(String Truong, String GiaTri, String DSGiaTri)
        {
            if (Truong == "PhanBo_DaCapPhat")
            {
                return get_PhanBo_DaCapPhat(GiaTri, DSGiaTri);
            }
            return null;
        }

        private JsonResult get_PhanBo_DaCapPhat(String GiaTri, String DSGiaTri)
        {
            String iID_MaCapPhat = GiaTri;
            String[] arrDSGiaTri = DSGiaTri.Split(',');
            String iID_MaDonVi = arrDSGiaTri[0];
            String iID_MaMucLucNganSach = arrDSGiaTri[1];

            NameValueCollection data = CapPhat_ChungTuModels.LayThongTin(iID_MaCapPhat);
            int iNamLamViec = Convert.ToInt32(data["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(data["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(data["iID_MaNamNganSach"]);
            Object obNgayCapPhat=data["dNgayCapPhat"];
            DateTime dNgayCapPhat = Convert.ToDateTime(obNgayCapPhat);
            int iThangCapPhat = dNgayCapPhat.Month;
            Object item = new
            {
                data = CapPhat_BangDuLieu.LayGiaTri_ChiTieu_DaCap(iID_MaMucLucNganSach, iID_MaDonVi, iNamLamViec,Convert.ToString(dNgayCapPhat), iID_MaNguonNganSach, iID_MaNamNganSach)
            };

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
