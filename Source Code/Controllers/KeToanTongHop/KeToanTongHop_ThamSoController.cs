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
namespace VIETTEL.Controllers.KeToanTongHop
{
    public class KeToanTongHop_ThamSoController : Controller
    {
        //
        // GET: /KeToanTongHop_ThamSo/

        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/ThamSo/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_DanhMucThamSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KeToanTongHop_ThamSo_Index.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaDonVi"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaThamSo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_DanhMucThamSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaThamSo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaThamSo"] = iID_MaThamSo;
            return View(sViewPath + "KeToanTongHop_ThamSo_Edit.aspx");
        }
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="MaDonVi"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaThamSo, String bChoPhepXoa)
        {
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_DanhMucThamSo", "Delete") == false || bChoPhepXoa == "False")
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (bChoPhepXoa == "False")
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("KT_DanhMucThamSo");
            bang.GiaTriKhoa = iID_MaThamSo;
            bang.Delete();
            return View(sViewPath + "KeToanTongHop_ThamSo_Index.aspx");
        }
        /// <summary>
        /// Lưu trữ CSDL
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaThamSo"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaThamSo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_DanhMucThamSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sKyHieu = Convert.ToString(Request.Form[ParentID + "_sKyHieu"]);
            String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
            String sThamSo = Convert.ToString(Request.Form[ParentID + "_sThamSo"]);
            String sBaoCao_ControllerName = Convert.ToString(Request.Form[ParentID + "_sBaoCao_ControllerName"]);

            if ((String.IsNullOrEmpty(sKyHieu) == true || sKyHieu == "") && Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                arrLoi.Add("err_sKyHieu", "Bạn chưa nhập ký hiệu!");
            }
            if (String.IsNullOrEmpty(sNoiDung) == true || sNoiDung == "")
            {
                arrLoi.Add("err_sNoiDung", "Bạn chưa nhập nội dung!");
            }
            if (String.IsNullOrEmpty(sThamSo) == true || sThamSo == "")
            {
                arrLoi.Add("err_sThamSo", "Bạn chưa nhập tham số!");
            }
            if ((String.IsNullOrEmpty(sBaoCao_ControllerName) == true || sBaoCao_ControllerName == "") && Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                arrLoi.Add("err_optLoaiBaoCao", "Bạn phải chọn loại báo cáo!");
            }

            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (CheckMaThamSo(sKyHieu) == true)
                {
                    arrLoi.Add("err_sKyHieu", "Mã ký hiệu đã tồn tại!");
                }
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["sKyHieu"] = sKyHieu;
                ViewData["sNoiDung"] = sNoiDung;
                ViewData["sThamSo"] = sThamSo;

                ViewData["iID_MaThamSo"] = iID_MaThamSo;
                //ViewData["DuLieuMoi"] = Request.Form[ParentID + "_DuLieuMoi"];
                return View(sViewPath + "KeToanTongHop_ThamSo_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("KT_DanhMucThamSo");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = iID_MaThamSo;
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", DanhMucModels.NamLamViec(User.Identity.Name));
                bang.Save();
                return View(sViewPath + "KeToanTongHop_ThamSo_Index.aspx");
            }
        }

        [Authorize]
        public ActionResult CopyDuLieu(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_DanhMucThamSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iNamLamViec = Convert.ToInt16(ReportModels.LayNamLamViec(User.Identity.Name));
            //Check Tham so đã có hay ko
            int count = KeToan_DanhMucThamSoModels.GetDanhSach_Count(iNamLamViec);
            if (count <= 0)
            {
                String SQL = String.Format(@"INSERT INTO KT_DanhMucThamSo(sKyHieu,sLoaiThamSo,sBaoCao_ControllerName,
sNoiDung,sThamSo,iNamLamViec,bChoPhepXoa,iSTT,iTrangThai,bPublic,
iID_MaNhomNguoiDung_Public,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao) 
SELECT sKyHieu,sLoaiThamSo,sBaoCao_ControllerName,
sNoiDung,sThamSo,{0},bChoPhepXoa,iSTT,iTrangThai,bPublic,
iID_MaNhomNguoiDung_Public,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao
 FROM KT_DanhMucThamSo
  WHERE iNamLamViec={1}", iNamLamViec, iNamLamViec - 1);
                SqlCommand cmd = new SqlCommand(SQL);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Kiem tra ma tham so
        /// </summary>
        /// <param name="iID_MaThamSo"></param>
        /// <returns></returns>
        public Boolean CheckMaThamSo(String sKyHieu)
        {
            Boolean vR = false;
            DataTable dt = KeToan_DanhMucThamSoModels.GetRow_ThamSo(sKyHieu);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }

    }
}
