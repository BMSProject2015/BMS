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
    public class TCDN_ChiTieuThuNopController : Controller
    {
        //
        // GET: /TCDN_ChiTieuThuNop/
        public string sViewPath = "~/Views/TCDN/HoSoDoanhNghiep/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "TCDN_ChiTieuThuNop_Index.aspx");
        }
        [Authorize]
        public ActionResult List(String iQuy, String iNam, String iID_MaDoanhNghiep)
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(MaND, "TCDN_ChiTieuThuNop", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            Boolean bCheck = TCDN_ChiTieuThuNopModels.CheckData(iQuy, iNam, iID_MaDoanhNghiep);
            if (bCheck == false)
            {
                //Thêm dữ liệu ban đầu vào bảng TCDN_ChiTieuThuNop
                TCDN_ChiTieuThuNopModels.ThemChiTiet(iID_MaDoanhNghiep, iQuy, iNam, MaND, Request.UserHostAddress);
            }

            return RedirectToAction("Edit", "TCDN_ChiTieuThuNop", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iQuy = iQuy, iNam = iNam });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaDoanhNghiep, String iQuy, String iNam)
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(MaND, "TCDN_KinhDoanh_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
            ViewData["iQuy"] = iQuy;
            ViewData["iNam"] = iNam;
            return View(sViewPath + "TCDN_ChiTieuThuNop_Edit.aspx");
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
            String iID_MaDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_iID_MaDoanhNghiep"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            if (iID_MaDoanhNghiep == Convert.ToString(Guid.Empty) || iID_MaDoanhNghiep == "")
            {
                arrLoi.Add("err_iID_MaDoanhNghiep", "Bạn chưa chọn doanh nghiệp để thực hiện!");
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
                return View(sViewPath + "TCDN_ChiTieuThuNop_Index.aspx");
            }
            else
            {
                Boolean bCheck = TCDN_ChiTieuThuNopModels.CheckData(iQuy, iNam, iID_MaDoanhNghiep);
                if (bCheck == false)
                {
                    //Thêm dữ liệu ban đầu vào bảng TCDN_ChiTieuThuNop
                    TCDN_ChiTieuThuNopModels.ThemChiTiet(iID_MaDoanhNghiep, iQuy, iNam, MaND, Request.UserHostAddress);
                }
            }
            return RedirectToAction("Edit", "TCDN_ChiTieuThuNop", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iQuy = iQuy, iNam = iNam });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaDoanhNghiep, String iQuy, String iNam)
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
                    Bang bang = new Bang("TCDN_ChiTieuThuNop");
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
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

            return RedirectToAction("Edit", "TCDN_ChiTieuThuNop", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iQuy = iQuy, iNam = iNam });
        }
    }
}
