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

namespace VIETTEL.Controllers.NhanSu
{
    public class CanBo_KyLuatController : Controller
    {
        //
        // GET: /KyLuat/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Thêm dữ liệu vào bảng CB_KyLuat
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaKhenThuong"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_KyLuat_Submit(String ParentID, String OnSuccess, String iID_MaCanBo, String iID_MaKyLuat)
        {
            Bang bang = new Bang("CB_KyLuat");
            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                bang.Save();
            }
            else
            {
                bang.GiaTriKhoa = iID_MaKyLuat;
                bang.Save();
            }

            String strJ = "";
            if (String.IsNullOrEmpty(OnSuccess) == false)
            {
                strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
            }
            else
            {
                strJ = String.Format("Dialog_close('{0}');", ParentID);
            }
            return JavaScript(strJ);
        }

        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="iID_MaQuaTrinhCongTac"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaKyLuat, String iID_MaCanBo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "CB_KyLuat", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Xóa dữ liệu trong bảng CB_KyLuat
            Bang bang = new Bang("CB_KyLuat");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruongKhoa = "iID_MaKyLuat";
            bang.GiaTriKhoa = iID_MaKyLuat;
            bang.Delete();
            return RedirectToAction("Edit", "CanBo_HoSoNhanSu", new
            {
                iID_MaCanBo = iID_MaCanBo
            });
        }
    }
}
