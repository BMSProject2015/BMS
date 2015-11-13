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
    public class QuyetToan_ChungTuKeToanController : Controller
    {
        //
        // GET: /QuyetToan_ChungTuKeToan/
        public string sViewPath = "~/Views/QuyetToan/ChungTuKeToan/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "QuyetToan_ChungTuKeToan_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("QTA_ChungTu_KeToan");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaMucLucNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaMucLucNganSach"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String NhapLieu = Convert.ToString(Request.Form[ParentID + "_NhapLieu"]);
            String InBangKe = Convert.ToString(Request.Form[ParentID + "_InBangKe"]);


            if (iID_MaMucLucNganSach == string.Empty || iID_MaMucLucNganSach == "" || iID_MaMucLucNganSach == null || iID_MaMucLucNganSach == "00000000-0000-0000-0000-000000000000")
            {
                arrLoi.Add("err_iID_MaMucLucNganSach", "Bạn chưa chọn mục lục ngân sách!");
            }
            if (iQuy == string.Empty || iQuy == "" || iQuy == null || iQuy == "-1")
            {
                arrLoi.Add("err_iQuy", "Bạn chưa chọn quý làm việc!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return View(sViewPath + "QuyetToan_ChungTuKeToan_Index.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];
                switch (Convert.ToInt32(NhapLieu))
                {
                    case 1:
                        return RedirectToAction("Detail", "QuyetToan_ChungTuKeToan", new { iID_MaMucLucNganSach = iID_MaMucLucNganSach , iQuy = iQuy});
                    case 2:
                        break;
                    case 3:
                        QuyetToan_ChungTuKeToanModels.ChuyenSoLieuSangQuyetToan(iID_MaMucLucNganSach, iQuy, Convert.ToString(R["iNamLamViec"]));
                        break;
                    case 4:
                        QuyetToan_ChungTuKeToanModels.ChuyenSoLieuSangQuyetToan_All(iQuy, Convert.ToString(R["iNamLamViec"]));
                        break;
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "QuyetToan_ChungTuKeToan");
        }

        [Authorize]
        public ActionResult Detail(String iID_MaMucLucNganSach, String iQuy)
        {
            ViewData["iID_MaMucLucNganSach"] = iID_MaMucLucNganSach;
            ViewData["iQuy"] = iQuy;
            return View(sViewPath + "QuyetToan_ChungTuKeToan_Detail.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaMucLucNganSach, String iQuy)
        {
            String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(User.Identity.Name);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "QTA_ChungTu_KeToan";

            DataTable dtMLNS = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTu_KeToan;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length - 1; i++)
            {
                iID_MaChungTu_KeToan = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaChungTu_KeToan != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaChungTu_KeToan;
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
                        iID_MaChungTu_KeToan = arrMaHang[i];
                        if (iID_MaChungTu_KeToan == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBanNguoiDung);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
                            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                            //Điền thông tin các trường mục lục ngân sách                       
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dtMLNS.Rows[0]["iID_MaMucLucNganSach_Cha"]);
                            bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", dtMLNS.Rows[0]["sXauNoiMa"]);
                            bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", dtMLNS.Rows[0]["bLaHangCha"]);
                            bang.CmdParams.Parameters.AddWithValue("@sLNS", dtMLNS.Rows[0]["sLNS"]);
                            bang.CmdParams.Parameters.AddWithValue("@sL", dtMLNS.Rows[0]["sL"]);
                            bang.CmdParams.Parameters.AddWithValue("@sK", dtMLNS.Rows[0]["sK"]);
                            bang.CmdParams.Parameters.AddWithValue("@sM", dtMLNS.Rows[0]["sM"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTM", dtMLNS.Rows[0]["sTM"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTTM", dtMLNS.Rows[0]["sTTM"]);
                            bang.CmdParams.Parameters.AddWithValue("@sNG", dtMLNS.Rows[0]["sNG"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTNG", dtMLNS.Rows[0]["sTNG"]);
                            bang.CmdParams.Parameters.AddWithValue("@sMoTa", dtMLNS.Rows[0]["sMoTa"]);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTu_KeToan;
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
                                if (arrMaCot[j].StartsWith("d"))
                                {
                                    //Nhap kieu date
                                    bang.CmdParams.Parameters.AddWithValue(Truong, CommonFunction.ChuyenXauSangNgay(arrGiaTri[j]));
                                }
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
                        bang.Save();
                    }
                }
            }
            return RedirectToAction("Detail", new { iID_MaMucLucNganSach = iID_MaMucLucNganSach, iQuy = iQuy });
        }

        public JsonResult get_dtMucLucNganSach(String ParentID, String sLNS)
        {
            return Json(get_objMucLucNganSach(ParentID, sLNS), JsonRequestBehavior.AllowGet);
        }

        public static String get_objMucLucNganSach(String ParentID, String sLNS)
        {
            String strDanhMucDuAn = string.Empty;
            DataTable dt = DanhMucModels.NS_LoaiNganSach_Ma_Con(sLNS);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaMucLucNganSach"] = "00000000-0000-0000-0000-000000000000";
                R["TenHT"] = "--- Danh sách mục lục ngân sách ---";
                dt.Rows.InsertAt(R, 0);
            }

            SelectOptionList slDanhMucDuan = new SelectOptionList(dt, "iID_MaMucLucNganSach", "TenHT");
            strDanhMucDuAn = MyHtmlHelper.DropDownList(ParentID, slDanhMucDuan, "", "iID_MaMucLucNganSach", null, "SIZE=\"30\" class=\"input1_2\"");
            
            return "<div>" + strDanhMucDuAn + "</div>";
        }
    }
}
