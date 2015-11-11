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

namespace VIETTEL.Controllers.BaoHiem
{
    public class BaoHiem_CauHinhDoiTuongNganSachController : Controller
    {
        //
        // GET: /BaoHiem_CauHinhDoiTuongNganSach/

        //
        // GET: /BaoHiem_PhaiThuChiTiet/
        public string sViewPath = "~/Views/BaoHiem/CauHinhNganSach/";
        [Authorize]
        public ActionResult Index()
        {
            String MaND = User.Identity.Name;
            String IPSua=Request.UserHostAddress;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            Boolean Trung = HamChung.Check_Trung("BH_CauHinh_DoiTuong_NganSach", "iID_MaCauHinhBaoHiem", Guid.Empty.ToString(), "iNamLamViec", iNamLamViec, true);
            if (Trung == false)
            {
                BaoHiem_CauHinhDoiTuongNganSachModels.ThemChiTiet(iNamLamViec, MaND, IPSua);
            }
            return View(sViewPath + "BaoHiem_CauHinhDoiTuongNganSach.aspx");
        }
        [Authorize]
        public ActionResult Edit()
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "BH_CauHinh_DoiTuong_NganSach", "Edit") == false)
            {
            
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "BH_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "1";          
            return View(sViewPath + "BaoHiem_PhaiThu_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "BH_CauHinh_DoiTuong_NganSach", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec=Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            Boolean CoLoi = false;
            if (CoLoi)
            {                
                ViewData["ThongBaoLoi"] = "Đã có cấu hình của năm "+ iNamLamViec;
                return View(sViewPath + "BaoHiem_PhaiThuMaBaoHiemPhaiThu_Edit.aspx");
            }
            else
            {                         
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "BaoHiem_PhaiThu");
        }

   
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit()
        {
            String MaND = User.Identity.Name;
            String iNamLamViec=Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND,"iNamLamViec"));
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String TenBangChiTiet = "BH_CauHinh_DoiTuong_NganSach";
            String iID_MaCauHinhBaoHiem;
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                  iID_MaCauHinhBaoHiem = arrMaHang[i];
                  if (arrHangDaXoa[i] == "1")
                  {
                      //Lưu các hàng đã xóa
                      if (iID_MaCauHinhBaoHiem != "")
                      {
                          //Dữ liệu đã có
                          Bang bang = new Bang(TenBangChiTiet);
                          bang.DuLieuMoi = false;
                          bang.GiaTriKhoa = iID_MaCauHinhBaoHiem;
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
                          Bang bang = new Bang("BH_CauHinh_DoiTuong_NganSach");                          
                          if (String.IsNullOrEmpty(iID_MaCauHinhBaoHiem))
                          {
                              bang.DuLieuMoi = true;
                          }
                          else
                          {
                              bang.GiaTriKhoa = arrMaHang[i];
                              bang.DuLieuMoi = false;
                          }
                          bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
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

            return RedirectToAction("Index");
        }

    }
}
