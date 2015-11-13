using System;
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


namespace VIETTEL.Controllers.KeToanTongHop
{
    public class KeToan_RutDuToanController : Controller
    {
        //
        // GET: /KeToan_RutDuToan/
        public string sViewPath = "~/Views/KeToanChiTiet/KhoBac/DotRutDuToan/";
        public ActionResult Index()
        {
            return View(sViewPath + "DotRutDuToan_Index.aspx");
        }
        public ActionResult Detail()
        {
            return View(sViewPath + "DotRutDuToan_Detail.aspx");
        }
        
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewSubmit(String ParentID)
        {
            String MaDotRutDuToan = "";
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            Bang bang = new Bang("KT_RutDuToanDot");
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);                        
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

            String dNgayDotRutDuToan = Convert.ToString(Request.Form[ParentID + "_vidNgayDotRutDuToan"]);

            if (HamChung.isDate(dNgayDotRutDuToan) == false)
            {
                arrLoi.Add("err_dNgayDotRutDuToan", "Ngày không đúng");
            }
            DateTime d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayDotRutDuToan));

            if (dNgayDotRutDuToan == string.Empty || dNgayDotRutDuToan == "" || dNgayDotRutDuToan == null)
            {
                arrLoi.Add("err_dNgayDotRutDuToan", "Bạn chưa nhập ngày!");
            }
            if (HamChung.Check_Trung("KT_RutDuToanDot", "iID_MaDotRutDuToan", Guid.Empty.ToString(), "dNgayDotRutDuToan", dNgayDotRutDuToan, true))
            {
                arrLoi.Add("err_dNgayDotRutDuToan", "Trùng đợt phân bổ");
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaDotRutDuToan"] = MaDotRutDuToan;
                return View(sViewPath + "DotRutDuToan_Index.aspx");
            }
            else
            {
                MaDotRutDuToan = Convert.ToString(bang.Save());
                KeToan_RutDuToanDotModels.Save(MaDotRutDuToan, Request.UserHostAddress, User.Identity.Name);
                return RedirectToAction("Index", "KeToan_RutDuToan");
            }

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ParentID, String iID_MaDotRutDuToan)
        {
            String TenBangChiTiet = "KT_RutDuToanChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTuChiTiet;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
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
                        iID_MaChungTuChiTiet = arrMaHang[i];
                        if (iID_MaChungTuChiTiet == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;                                                      
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
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
            return RedirectToAction("Index", "KeToan_RutDuToan", new { iID_MaDotRutDuToan = iID_MaDotRutDuToan});
        }
        /// <summary>
        /// Xoa du toan
        /// </summary>
        /// <param name="iID_MaDotRutDuToan"></param>
        /// <returns></returns>
        [Authorize]
       
        public ActionResult Delete(String iID_MaDotRutDuToan)
        {
            Connection.DeleteRecord("KT_RutDuToanChiTiet", "iID_MaDotRutDuToan", iID_MaDotRutDuToan);
            int kq = Connection.DeleteRecord("KT_RutDuToanDot", "iID_MaDotRutDuToan", iID_MaDotRutDuToan);
            return RedirectToAction("Index", "KeToan_RutDuToan");
        }
    }
}
