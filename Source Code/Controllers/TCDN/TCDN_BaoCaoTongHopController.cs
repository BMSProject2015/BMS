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

namespace VIETTEL.Controllers.TCDN
{
    public class TCDN_BaoCaoTongHopController : Controller
    {

        //
        // GET: /TCDN_BaoCaoTongHop/
        public string sViewPath = "~/Views/TCDN/BaoCaoTongHop/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "TCDN_BaoCaoTongHop_Index.aspx");
        }
        [Authorize]
        public ActionResult List(String iQuy, String iNam)
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(MaND, "TCDN_BaoCaoTongHop", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            Boolean bCheck = TCDN_BaoCaoTongHopModels.CheckData(iQuy, iNam);
            if (bCheck == false)
            {
                //Thêm dữ liệu ban đầu vào bảng TCDN_BaoCaoTongHop
                return View(sViewPath + "TCDN_BaoCaoTongHop_Index.aspx");
            }

            return RedirectToAction("Edit", "TCDN_BaoCaoTongHop", new { iQuy = iQuy, iNam = iNam });
        }
        [Authorize]
        public ActionResult Edit(String iQuy, String iNam, String iID_MaLoaiDoanhNghiep)
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(MaND, "TCDN_BaoCaoTongHop", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iQuy"] = iQuy;
            ViewData["iNam"] = iNam;
            ViewData["iID_MaLoaiDoanhNghiep"] = iID_MaLoaiDoanhNghiep;
            return View(sViewPath + "TCDN_BaoCaoTongHop_Edit.aspx");
        }
        [Authorize]
        public ActionResult Reports(String iNamLamViec, String iQuy, String iID_MaLoaiDoanhNghiep)
        {
            return RedirectToAction("Index", "rptTCDN_BaoCaoTongHop", new { iNamLamViec = iNamLamViec, iQuy = iQuy, iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            Bang bang = new Bang("TCDN_KinhDoanh_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaLoaiDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiDoanhNghiep"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            if (String.IsNullOrEmpty(iID_MaLoaiDoanhNghiep))
            {
                arrLoi.Add("err_iID_MaLoaiDoanhNghiep", "Bạn chưa chọn loại doanh nghiệp!");
            }
            if (iQuy == "" || iQuy == null)
            {
                arrLoi.Add("err_iQuy", "Bạn chưa chọn quý thực hiện!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return View(sViewPath + "TCDN_BaoCaoTongHop_Index.aspx");
            }
            else
            {
                Boolean bCheck = TCDN_BaoCaoTongHopModels.CheckData(iQuy, iNam);
                if (bCheck == false)
                {
                    //Thêm dữ liệu ban đầu vào bảng TCDN_BaoCaoTongHop
                    TCDN_BaoCaoTongHopModels.ThemChiTiet(iID_MaLoaiDoanhNghiep, iQuy, iNam, MaND, Request.UserHostAddress);
                }
            }
            return RedirectToAction("Edit", "TCDN_BaoCaoTongHop", new { iQuy = iQuy, iNam = iNam, iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iQuy, String iNam)
        {
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
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { TCDN_ChungTu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { TCDN_ChungTu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { TCDN_ChungTu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { TCDN_ChungTu_BangDuLieu.DauCachO }, StringSplitOptions.None);
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
                    Bang bang = new Bang("TCDN_BaoCaoTongHop");
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
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
                    bang.Save();
                }
            }

            return RedirectToAction("Edit", "TCDN_BaoCaoTongHop", new { iQuy = iQuy, iNam = iNam });
        }
    }
}
