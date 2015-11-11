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

namespace VIETTEL.Controllers.KeToanChiTiet.TienGui
{
    public class KTCT_DuyetCapThuController : Controller
    {
        //
        // GET: /KTCT_DuyetCapThu/
        public string sViewPath = "~/Views/KeToanChiTiet/TienGui/CapThuDuyet/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "CapThuDuyet_Index.aspx");
        }
        [Authorize]
        public ActionResult Edit(String ParentID)
        {
            ViewData["DuLieuMoi"] = "1";
            String iID_MaChungTuCapPhat = Convert.ToString(Request.Form["iID_MaCapPhat"]);
            if(String.IsNullOrEmpty(iID_MaChungTuCapPhat))
                iID_MaChungTuCapPhat = Convert.ToString(Request.QueryString["iID_MaChungTuCapPhat"]);
            String strNoiDungCapPhat = CapPhat_ChungTuModels.LayThongTin(iID_MaChungTuCapPhat)["sNoiDung"];
            Boolean bCheck = CapPhat_ChungTuChiTiet_DonViModels.Check_ChungTuCapThu(iID_MaChungTuCapPhat);
            String SQL = String.Format(@"SELECT MAX(iSoChungTu)
                                        FROM KTTG_ChungTuChiTietCapThu
                                        WHERE iTrangThai=1 {0}", ReportModels.DieuKien_NganSach(User.Identity.Name));
            SqlCommand cmd = new SqlCommand(SQL);

            int iSoChungTu = 0;
            iSoChungTu = Convert.ToInt16(Connection.GetValue(cmd, 0)) + 1;

            if (bCheck == false)
            {
                DataTable dtCP = CapPhat_ChungTuChiTiet_DonViModels.Get_dtCapPhatChiTiet_CapThu(iID_MaChungTuCapPhat);
                DataRow R;
                int i;
                for (i = 0; i < dtCP.Rows.Count; i++)
                {
                    R = dtCP.Rows[i];
                    String strTenTinhChatCapThu = Convert.ToString(R["iID_MaTinhChatCapThu"]) + " - " + Convert.ToString(TinhChatCapThuModels.Get_RowTinhChatCapThu(Convert.ToString(R["iID_MaTinhChatCapThu"])).Rows[0]["sTen"]);

                    Bang bangchungtu = new Bang("KTTG_ChungTuChiTietCapThu");
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", R["iDM_MaLoaiCapPhat"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", R["iID_MaCapPhat"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaCapPhatChiTiet", R["iID_MaCapPhatChiTiet"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@bLoai", R["bLoai"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", R["iID_MaPhongBan"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sTenPhongBan", R["sTenPhongBan"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", R["iID_MaDonVi"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sTenDonVi", R["sTenDonVi"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", R["iID_MaMucLucNganSach"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sXauNoiMa", R["sXauNoiMa"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@bLaHangCha", R["bLaHangCha"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sLNS", R["sLNS"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sL", R["sL"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sK", R["sK"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sM", R["sM"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sTM", R["sTM"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sTTM", R["sTTM"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sNG", R["sNG"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sTNG", R["sTNG"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sMoTa", R["sMoTa"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iNgay", Convert.ToDateTime(R["dNgayCapPhat"]).Day);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iThang", Convert.ToDateTime(R["dNgayCapPhat"]).Month);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaTinhChatCapThu", R["iID_MaTinhChatCapThu"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sTinhChatCapThu", strTenTinhChatCapThu);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@rSoTien", R["rTuChi"]);
                    bangchungtu.CmdParams.Parameters.AddWithValue("@sNoiDung", strNoiDungCapPhat);
                    bangchungtu.MaNguoiDungSua = User.Identity.Name;
                    bangchungtu.IPSua = Request.UserHostAddress;
                    bangchungtu.Save();
                }
                ViewData["DuLieuMoi"] = "0";
            }
            else
            {
                DataTable dtCP = CapPhat_ChungTuChiTiet_DonViModels.Get_dtCapPhatChiTiet_CapThu(iID_MaChungTuCapPhat);
                DataRow R;
                int i;
                for (i = 0; i < dtCP.Rows.Count; i++)
                {
                    R = dtCP.Rows[i];
                    CapThuModels.Update_rSoTien_KTTG_ChungTuChiTietCapThu(iID_MaChungTuCapPhat, Convert.ToString(R["iID_MaCapPhatChiTiet"]), Convert.ToDouble(R["rTuChi"]));
                }
            }
            ViewData["iID_MaCapPhat"] = iID_MaChungTuCapPhat;
            return View(sViewPath + "CapThuDuyet_Edit.aspx");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaChungTu_Duyet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            CapThuModels.Delete_CapThu_Duyet(iID_MaChungTu_Duyet);

            return RedirectToAction("Index", "KTCT_DuyetCapThu");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            return RedirectToAction("Index", "KTCT_DuyetCapThu");
        }
        [Authorize]
        public ActionResult AddNew()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "CapThuDuyet_AddNew.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewSubmit(String ParentID)
        {
            String ThemMoi = Request.Form[ParentID + "_iThemMoi"];
            if (ThemMoi == "on")
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                Int32 iSoDot = 1;

                if (KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Max_ChungTu(NamLamViec) != "")
                {
                    iSoDot = Convert.ToInt32(KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Max_ChungTu(NamLamViec)) + 1;
                };

                String sSoChungTu = Convert.ToString(Request.Form[ParentID + "_sSoChungTu"]);
                String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
                String dTuNgay = Convert.ToString(Request.Form[ParentID + "_vidTuNgay"]);
                String dDenNgay = Convert.ToString(Request.Form[ParentID + "_vidDenNgay"]);
                String iID_MaTaiKhoan_No = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan_No"]);
                String iID_MaTaiKhoan_Co = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan_Co"]);
                String iID_MaDonVi_No = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi_No"]);
                String iID_MaDonVi_Co = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi_Co"]);

                NameValueCollection arrLoi = new NameValueCollection();
                if (sSoChungTu == string.Empty || sSoChungTu == "" || sSoChungTu == null)
                {
                    arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
                }
                if (iID_MaDonVi == string.Empty || iID_MaDonVi == "" || iID_MaDonVi == null)
                {
                    arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
                }
                if (HamChung.isDate(dTuNgay) == false && dTuNgay != "")
                {
                    arrLoi.Add("err_dTuNgay", "Từ ngày nhập sai");
                }
                if (HamChung.isDate(dDenNgay) == false && dDenNgay != "")
                {
                    arrLoi.Add("err_dDenNgay", "Đến ngày nhập sai");
                }
                if (iID_MaTaiKhoan_No == string.Empty || iID_MaTaiKhoan_No == "" || iID_MaTaiKhoan_No == null)
                {
                    arrLoi.Add("err_iID_MaTaiKhoan_No", "Bạn chưa chọn tài khoản nợ!");
                }
                if (iID_MaTaiKhoan_Co == string.Empty || iID_MaTaiKhoan_Co == "" || iID_MaTaiKhoan_Co == null)
                {
                    arrLoi.Add("err_iID_MaTaiKhoan_Co", "Bạn chưa chọn tài khoản có!");
                }
                if (iID_MaDonVi_No == string.Empty || iID_MaDonVi_No == "" || iID_MaDonVi_No == null)
                {
                    arrLoi.Add("err_iID_MaDonVi_No", "Bạn chưa chọn đơn vị nợ!");
                }
                if (iID_MaDonVi_Co == string.Empty || iID_MaDonVi_Co == "" || iID_MaDonVi_Co == null)
                {
                    arrLoi.Add("err_iID_MaDonVi_Co", "Bạn chưa chọn đơn vị có!");
                }

                if (arrLoi.Count == 0)
                {
                    String sTenDonVi_No = "";
                    sTenDonVi_No = Connection.GetValueString(String.Format("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi='{0}'", iID_MaDonVi_No), "");

                    String sTenDonVi_Co = "";
                    sTenDonVi_Co = Connection.GetValueString(String.Format("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi='{0}'", iID_MaDonVi_Co), "");

                    String sTenTaiKhoan_No = "";
                    sTenTaiKhoan_No = Connection.GetValueString(String.Format("SELECT iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan='{0}'", iID_MaTaiKhoan_No), "");

                    String sTenTaiKhoan_Co = "";
                    sTenTaiKhoan_Co = Connection.GetValueString(String.Format("SELECT iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan='{0}'", iID_MaTaiKhoan_Co), "");

                    Bang bang = new Bang("KTTG_ChungTuCapThu_Duyet");
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                    bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoDot);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", sTenDonVi_No);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", sTenDonVi_Co);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", sTenTaiKhoan_No);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", sTenTaiKhoan_Co);
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    bang.Save();
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["bThemMoi"] = true;
                    return View(sViewPath + "CapThuDuyet_Index.aspx");
                }
            }
            return RedirectToAction("Index", "KTCT_DuyetCapThu");
        }
        [Authorize]
        public ActionResult Duyet()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "CapThuDuyet_Duyet.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DuyetSubmit(String iID_MaCapPhat)
        {
            String ParentID = "KTTG_ChungTuCapThu_Duyet";
            SqlCommand cmd;
            String iID_MaChungTu_Duyet = "";
            NameValueCollection arrLoi = new NameValueCollection();
            string idTaiKhoanCo = Request.Form["idTaiKhoanCo"];
            string idTaiKhoanNo = Request.Form["idTaiKhoanNo"];
            //String idDonViCo = Convert.ToString(Request.Form["iID_iID_MaDonVi_Co"]);
            //String idDonViNo = Convert.ToString(Request.Form["iID_iID_MaDonVi_No"]);
            string idDonViCo = Request.Form["idDonViCo"];
            string idDonViNo = Request.Form["idDonViNo"];
            string iID_sSoChungTu = Request.Form["iID_sSoChungTu"];
            string iID_sNoiDungChungTu = Request.Form["iID_sNoiDungChungTu"];
            String dNgayChungTu = Request.Form["iID_dNgayChungTu"];
            String sTenDonVi_No = "";
            if (iID_sSoChungTu == "" && String.IsNullOrEmpty(iID_sSoChungTu) == true)
            {
              //  HamChung.MessageBox(this, "Hãy nhập số UNC/RDT");
                return RedirectToAction("Edit", new { iID_MaChungTuCapPhat = iID_MaCapPhat });
            }
            //if (iID_sSoChungTu == "" && String.IsNullOrEmpty(iID_sSoChungTu) == true)
            //{
            //    arrLoi.Add("KTTG_ChungTuCapThu_Duyet_err_sSoChungTu", "Hãy nhập số UNC/RDT");
            //}
            //if (iID_sNoiDungChungTu == "" && String.IsNullOrEmpty(iID_sNoiDungChungTu) == true)
            //{
            //    arrLoi.Add("KTTG_ChungTuCapThu_Duyet_err_sNoiDung", "Hãy nhập nội dung chứng từ UNC/DT");
            //}
           
            //if (arrLoi.Count == 0)
            //{
                sTenDonVi_No =
                    Connection.GetValueString(
                        String.Format("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi='{0}'", idDonViNo), "");

                String sTenDonVi_Co = "";
                sTenDonVi_Co =
                    Connection.GetValueString(
                        String.Format("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi='{0}'", idDonViCo), "");

                String sTenTaiKhoan_No = "";
                sTenTaiKhoan_No =
                    Connection.GetValueString(
                        String.Format(
                            "SELECT iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan='{0}'",
                            idTaiKhoanNo), "");

                String sTenTaiKhoan_Co = "";
                sTenTaiKhoan_Co =
                    Connection.GetValueString(
                        String.Format(
                            "SELECT iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan='{0}'",
                            idTaiKhoanCo), "");

                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];
                String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

                Bang bangchungtu = new Bang("KTTG_ChungTuCapThu_Duyet");
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaTam", Guid.NewGuid());
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sSoChungTu", iID_sSoChungTu);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", idDonViCo);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", idDonViNo);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", idTaiKhoanNo);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", idTaiKhoanCo);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", sTenDonVi_No);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", sTenDonVi_Co);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", sTenTaiKhoan_No);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", sTenTaiKhoan_Co);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sNoiDung", iID_sNoiDungChungTu);
                bangchungtu.CmdParams.Parameters.AddWithValue("@dNgayChungTu", CommonFunction.LayNgayTuXau(dNgayChungTu));
                bangchungtu.MaNguoiDungSua = User.Identity.Name;
                bangchungtu.IPSua = Request.UserHostAddress;
                bangchungtu.Save();

                //bangchungtu.TruongKhoa = "iID_MaChungTu_Duyet";
                //iID_MaChungTu_Duyet = Convert.ToString(bangchungtu.GiaTriKhoa);

                cmd =
                    new SqlCommand("SELECT MAX(iID_MaChungTu_Duyet) FROM KTTG_ChungTuCapThu_Duyet WHERE iTrangThai = 1");
                iID_MaChungTu_Duyet = Convert.ToString(Connection.GetValue(cmd, ""));
                cmd.Dispose();

                String TenBangChiTiet = "KTTG_ChungTuChiTietCapThu";

                string idXauMaCacHang = Request.Form["idXauMaCacHang"];
                string idXauMaCacCot = Request.Form["idXauMaCacCot"];
                string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
                string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
                string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
                String[] arrMaHang = idXauMaCacHang.Split(',');
                String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
                String[] arrMaCot = idXauMaCacCot.Split(',');
                String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] {BangDuLieu.DauCachHang},
                                                                  StringSplitOptions.None);
                arrMaCot[17] = "rSoTienDuyet";
                String iID_MaChungTuChiTiet;

                //Luu cac hang sua
                String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] {BangDuLieu.DauCachHang},
                                                                   StringSplitOptions.None);
                for (int i = 0; i < arrMaHang.Length - 1; i++)
                {
                    iID_MaChungTuChiTiet = arrMaHang[i];
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaChungTuChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.Save();
                        }
                    }
                    else
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] {BangDuLieu.DauCachO},
                                                                    StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] {BangDuLieu.DauCachO},
                                                                      StringSplitOptions.None);
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
                            iID_MaChungTuChiTiet = arrMaHang[i];
                            if (iID_MaChungTuChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                                bang.DuLieuMoi = false;
                            }

                            if (arrGiaTri[0] == "1" || arrGiaTri[0] == "true")
                            {

                                int intCheck =
                                    KTCT_TienGui_ChungTuCapThuDuyetModels.Check_Ma_ChungTuChiTiet(iID_MaChungTuChiTiet);
                                if (intCheck > 0)
                                {
                                }
                                else
                                {
                                    Bang bang1 = new Bang("KTTG_ChungTuCapThu_Duyet_ChiTiet");
                                    bang1.DuLieuMoi = true;
                                    bang1.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
                                    bang1.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet",
                                                                            iID_MaChungTuChiTiet);
                                    bang1.MaNguoiDungSua = User.Identity.Name;
                                    bang1.IPSua = Request.UserHostAddress;
                                    bang1.Save();
                                }

                            }
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                            //Them tham so
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {
                                if (arrThayDoi[j] == "1")
                                {
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1" || arrGiaTri[j] == "true")
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
                //Sự kiện bấn vào nút trình duyệt
                string idAction = Request.Form["idAction"];
                //if (idAction == "1")
                //{
                //    return RedirectToAction("TuChoi", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = iID_MaChungTu_Duyet });
                //}
                //else if (idAction == "2")
                //{
                //    return RedirectToAction("TrinhDuyet", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = iID_MaChungTu_Duyet });
                //}

                //Update lai truong togn cap, tong thu và tien
                CapThuModels.CapNhapTruongTongCapTongThu(iID_MaChungTu_Duyet);
                //return View(sViewPath + "CapThuDuyet_Index.aspx");
               // return RedirectToAction("Detail", new {iID_MaChungTu_Duyet = iID_MaChungTu_Duyet});
                return RedirectToAction("Index", "KTCT_DuyetCapThu");
            //}
            //else
            //{
            //   for (int i = 0; i <= arrLoi.Count - 1; i++)
            //        {
            //            ModelState.AddModelError("KTTG_ChungTuCapThu_Duyet_" + arrLoi.GetKey(i), arrLoi[i]);
            //        }

            //   return RedirectToAction("Edit", new { iID_MaChungTuCapPhat = iID_MaCapPhat });

            //}
        }
        [Authorize]
        public ActionResult Detail()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "CapThuDuyet_Detail.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu_Duyet)
        {
            SqlCommand cmd;

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            String TenBangChiTiet = "KTTG_ChungTuCapThu_Duyet_ChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTuChiTiet_Duyet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length - 1; i++)
            {
                iID_MaChungTuChiTiet_Duyet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaChungTuChiTiet_Duyet != "")
                    {
                        //Lay ma iID_MaChungTuChiTiet cua bang duyet
                        String iID_MaChungTuChiTietDuyet = "";
                        cmd = new SqlCommand("SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTuChiTiet_Duyet = @iID_MaChungTuChiTiet_Duyet AND iTrangThai = 1");
                        cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_Duyet", iID_MaChungTuChiTiet_Duyet);
                        iID_MaChungTuChiTietDuyet = Convert.ToString(Connection.GetValue(cmd, ""));
                        cmd.Dispose();

                        //Dữ liệu đã có
                        cmd = new SqlCommand("DELETE FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTuChiTiet_Duyet=@iID_MaChungTuChiTiet_Duyet");
                        cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_Duyet", iID_MaChungTuChiTiet_Duyet);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();

                        //Update lai truong duyet trên bang KTTG_ChungTuChiTietCapThu
                        if (iID_MaChungTuChiTietDuyet != "")
                        {
                            cmd = new SqlCommand("UPDATE KTTG_ChungTuChiTietCapThu SET bDuyet = 0 WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet");
                            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTietDuyet);
                            Connection.UpdateDatabase(cmd);
                            cmd.Dispose();
                        }
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
                        TenBangChiTiet = "KTTG_ChungTuChiTietCapThu";
                        String iID_MaChungTuChiTiet = "";
                        cmd = new SqlCommand("SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTuChiTiet_Duyet = @iID_MaChungTuChiTiet_Duyet AND iTrangThai = 1");
                        cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_Duyet", iID_MaChungTuChiTiet_Duyet);
                        iID_MaChungTuChiTiet = Convert.ToString(Connection.GetValue(cmd, ""));
                        cmd.Dispose();
                        Bang bang = new Bang(TenBangChiTiet);
                        if (iID_MaChungTuChiTiet_Duyet == "")
                        {
                            //Du Lieu Moi
                            //  bang.DuLieuMoi = true;
                            //  bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
                            ////  bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
                            //  bang.MaNguoiDungSua = User.Identity.Name;
                            //  bang.IPSua = Request.UserHostAddress;

                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                String Truong = "@" + arrMaCot[j];
                                if (arrMaCot[j].StartsWith("b"))
                                {
                                    //Nhap Kieu checkbox
                                    if (arrGiaTri[j] == "1" || arrGiaTri[j] == "true" )
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
                        bang.Save();

                    }
                }
            }
            //Sự kiện bấn vào nút trình duyệt
            string idAction = Request.Form["idAction"];
            //if (idAction == "1")
            //{
            //    return RedirectToAction("TuChoi", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = iID_MaChungTu_Duyet });
            //}
            //else if (idAction == "2")
            //{
            //    return RedirectToAction("TrinhDuyet", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = iID_MaChungTu_Duyet });
            //}

            //Update lai truong togn cap, tong thu và tien
            CapThuModels.CapNhapTruongTongCapTongThu(iID_MaChungTu_Duyet);

            return RedirectToAction("Detail", new { iID_MaChungTu_Duyet = iID_MaChungTu_Duyet });
        }
        public JsonResult get_SoChungTuDuyet(String sSoUyNhiemChi)
        {
            return Json(get_objSoChungTuDuyet(sSoUyNhiemChi), JsonRequestBehavior.AllowGet);
        }
        public static String get_objSoChungTuDuyet(String sSoUyNhiemChi)
        {
            String strMess = "";
            DataTable dt = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_So_ChungTu(sSoUyNhiemChi);
            if (dt.Rows.Count > 0)
            {
                strMess = "Số quyết định đã tồn tại!";
            }
            else
            {
                strMess = "Số quyết định có thể thêm được!";
            }
            return strMess;
        }
        public JsonResult get_DSChungTuCapPhat(String iNamLamViec,String iID_MaDonVi,String TuNgay,String DenNgay,String iID_MaTinhChatCapThu,String chkTatCa)
        {
            String s="";
            String input = "";
            DataTable dtDSChungTuCapPhat = CapPhat_ChungTuChiTiet_DonViModels.GetDanhSachCapPhat_CapThu(iNamLamViec, iID_MaDonVi, TuNgay, DenNgay, iID_MaTinhChatCapThu, chkTatCa);
            StringBuilder stbDonVi = new StringBuilder();
            stbDonVi.Append("<div style=\"width: 100%; height: 200px; overflow: scroll; border:1px solid black;margin-right:5px;\">");
            stbDonVi.Append("<table class=\"mGrid\">");
            stbDonVi.Append("<tr>");
            stbDonVi.Append("<th  style=\"width:30px;\"><input type=\"checkbox\" id=\"checkAll\" onclick=\"ChonallDV(this.checked)\"></th><th>Danh sách chứng từ</th>");

            String TenDonVi = "", MaDonVi = "";
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dtDSChungTuCapPhat.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dtDSChungTuCapPhat.Rows[i - 1]["iID_MaCapPhat"]);
                TenDonVi = Convert.ToString(dtDSChungTuCapPhat.Rows[i - 1]["TENHT"]);
                _Checked = "";
                for (int j = 1; j <= arrDonVi.Length; j++)
                {
                    if (MaDonVi == arrDonVi[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaCapPhat\" id=\"iID_MaCapPhat\" name=\"iID_MaCapPhat\"  />", MaDonVi, _Checked);
                stbDonVi.Append("<tr>");
                stbDonVi.Append("<td style=\"text-align:center;\">");
                stbDonVi.Append(input);
                stbDonVi.Append("</td>");
                stbDonVi.Append("<td>" + TenDonVi + "</td>");

                stbDonVi.Append("</tr>");
            }
            stbDonVi.Append("</table>");
            stbDonVi.Append("</div>");
            s = stbDonVi.ToString();
            return Json(s, JsonRequestBehavior.AllowGet);
        }
    }
}
