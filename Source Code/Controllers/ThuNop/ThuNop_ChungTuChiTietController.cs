using System;
using System.Data;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Collections.Specialized;
namespace VIETTEL.Controllers.ThuNop
{
    public class ThuNop_ChungTuChiTietController : Controller
    {
        //
        // GET: /ThuNop_ChungTuChiTiet/
        public string sViewPath = "~/Views/ThuNop/ChungTuChiTiet/";
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "ThuNop_ChungTuChiTiet_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult ThuNopChiTiet_Frame(String iID_MaChungTu, String iLoai)
        {
            return View(sViewPath + "ThuNopChiTiet_Index_DanhSach_Frame.aspx", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai });
        }
        /// <summary>
        /// Luu tru vao CSDL
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="iLoai"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu, String iLoai)
        {

            //if (String.IsNullOrEmpty(iID_MaChungTu) || String.IsNullOrEmpty(iLoai))
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            //else if (iLoai != "1" || iLoai != "2")
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            NameValueCollection data = ThuNop_ChungTuModels.LayThongTin(iID_MaChungTu);
            String TenBangChiTiet = "TN_ChungTuChiTiet";

            String MaND = User.Identity.Name;
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            String strTenPhongBan = "", iID_MaPhongBan = "";
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                strTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
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
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] {CapPhat_BangDuLieu.DauCachHang},
                                                              StringSplitOptions.None);

            String iID_MaCapPhatChiTiet,sNG,sMoTa;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] {PhanBo_ChiTieu_BangDuLieu.DauCachHang},
                                                               StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    iID_MaCapPhatChiTiet = arrMaHang[i].Split('_')[0];
                    sNG = arrMaHang[i].Split('_')[1];
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
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] {PhanBo_ChiTieu_BangDuLieu.DauCachO},
                                                                    StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(
                            new string[] {PhanBo_ChiTieu_BangDuLieu.DauCachO}, StringSplitOptions.None);
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
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);

                            
                                //Them cac tham so tu bang chung tu

                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", strTenPhongBan);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach",
                                                                       data["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                                                       data["iID_MaTrangThaiDuyet"]);

                                bang.CmdParams.Parameters.AddWithValue("@dNgayChungTu", data["dNgayChungTu"]);
                                bang.CmdParams.Parameters.AddWithValue("@iLoai", iLoai);
                                bang.CmdParams.Parameters.AddWithValue("@sNG", sNG);
                                String SQL = String.Format(@"SELECT REPLACE(sTen,sTenKhoa+'-','') as sMoTa FROM DC_DanhMuc
WHERE bHoatDong=1 AND sTenKhoa='{0}' 
AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=N'TN_LoaiHinh')", sNG); ;
                                sMoTa = Connection.GetValueString(SQL, "");
                                bang.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.DuLieuMoi = false;
                            }
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;

                            //if (iID_MaCapPhatChiTiet == "")
                            //{
                            //    //Xác định xâu mã nối
                            //    String sXauNoiMa = "";
                            //    for (int k = 0; k < MucLucNganSachModels.arrDSTruong.Length; k++)
                            //    {
                            //        for (int j = 0; j < arrMaCot.Length; j++)
                            //        {
                            //            if (arrMaCot[j] == MucLucNganSachModels.arrDSTruong[k])
                            //            {
                            //                sXauNoiMa += String.Format("{0}-", arrGiaTri[j]);
                            //                break;
                            //            }
                            //        }
                            //    }

                            //    String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                            //    DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                            //    //Dien thong tin cua Muc luc ngan sach
                            //    NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0],
                            //                                                          bang.CmdParams.Parameters);
                            //    dtMucLuc.Dispose();
                            //}


                            //Them tham so
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {
                                if (arrThayDoi[j] == "1")
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
                                    else if (arrMaCot[j].StartsWith("r") ||
                                             (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong,
                                                                                   Convert.ToDouble(arrGiaTri[j]));
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }

                                }
                            }
                            bang.Save();
                        }
                    }
                }
            }

            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "ThuNop_ChungTu", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "ThuNop_ChungTu", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai });
            }
            return RedirectToAction("ThuNopChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai });
           
           
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
        public JsonResult get_GiaTri(String Truong, String GiaTri, String DSGiaTri, String iLoai)
        {
            if (iLoai=="1")
            {
                if (Truong == "PhanBo_DaCapPhat")
                {
                    return get_PhanBo_DaCapPhat(GiaTri, DSGiaTri);
                }
            }
            return null;
        }

        private JsonResult get_PhanBo_DaCapPhat(String GiaTri, String DSGiaTri)
        {
            String iID_MaChungTu = GiaTri;
            String[] arrDSGiaTri = DSGiaTri.Split(',');
            String iID_MaDonVi = arrDSGiaTri[0];
            String iID_MaMucLucNganSach = arrDSGiaTri[1];
            if (!String.IsNullOrEmpty(iID_MaChungTu))
            {


                NameValueCollection data = ThuNop_ChungTuModels.LayThongTin(iID_MaChungTu);
                int iNamLamViec = Convert.ToInt32(data["iNamLamViec"]);
                int iID_MaNguonNganSach = Convert.ToInt32(data["iID_MaNguonNganSach"]);
                int iID_MaNamNganSach = Convert.ToInt32(data["iID_MaNamNganSach"]);
                Object obNgayCapPhat = data["dNgayChungTu"];

                Object item = new
                                  {
                                      data =
                                          ThuNop_ChungTuChiTietModels.LayGiaTri_ChiTieu_DaCap(iID_MaMucLucNganSach,
                                                                                              iID_MaDonVi, iNamLamViec,
                                                                                              iID_MaNguonNganSach,
                                                                                              iID_MaNamNganSach)
                                  };

                return Json(item, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
