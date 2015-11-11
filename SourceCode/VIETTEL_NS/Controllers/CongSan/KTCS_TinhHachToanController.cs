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

namespace VIETTEL.Controllers.CongSan
{
    public class KTCS_TinhHachToanController : Controller
    {
        // GET: /KTCS_TinhHachToan/
        public string sViewPath = "~/Views/CongSan/TinhHachToan/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_ChungTuChiTietHachToan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KTCS_HachToanTaiSan_Index.aspx");
        }
        [Authorize]
        public ActionResult Detail()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KhauHaoHangNam", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KTCS_HachToanTaiSan_Detail.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ParentID, String NamLamViec)
        {
            String MaND = User.Identity.Name;
            String iTuThangHachToan = Request.Form[ParentID + "_iTuThangHachToan"];
            String iDenThangHachToan = Request.Form[ParentID + "_iDenThangHachToan"];

            String SQL,DK;
            SqlCommand cmd;
            //Xóa bảng KTCS_KhauHaoHangNam
            //cmd = new SqlCommand("TRUNCATE TABLE KTCS_KhauHaoHangNam");
            //Connection.UpdateDatabase(cmd);
            //cmd.Dispose();

            SQL = String.Format("SELECT * FROM KTCS_ChungTuChiTiet WHERE (iNamLamViec<@iNamLamViec OR (iNamLamViec=@iNamLamViec AND iThang<=@iThang)) AND iTrangThai=1 ORDER BY iNamLamViec,iThang,iNgay");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iDenThangHachToan);
            DataTable dtChungTuChiTiet = Connection.GetDataTable(cmd);
            cmd.Dispose();
            DataRow R;
            if (dtChungTuChiTiet.Rows.Count > 0)
            {
                for (int i = 0; i < dtChungTuChiTiet.Rows.Count; i++)
                {
                    R = dtChungTuChiTiet.Rows[i];
                    String iID_MaChungTuChiTiet = Convert.ToString(R["iID_MaChungTuChiTiet"]);

                    KTCS_HachToanModels.XoaHachToan(iID_MaChungTuChiTiet);
                    String vGiaTriTinhToan = KTCS_HachToanModels.TinhHachToanTaiSan(Convert.ToString(R["iID_MaTaiSan"]), Convert.ToInt32(NamLamViec), Convert.ToInt32(iDenThangHachToan), MaND, Request.UserHostAddress);
                    String[] arrHT = vGiaTriTinhToan.Split('#');
                    //NGIA#TANG#GIAM#HMON#LKHM#CONLAI#CONLAITKH#NAMKHCL
                    KTCS_HachToanModels.ThemMoiHachToan(iID_MaChungTuChiTiet, Convert.ToString(R["iID_MaKyHieuHachToan"]), arrHT[0], arrHT[1], arrHT[2], arrHT[3], arrHT[4], arrHT[5], arrHT[6], arrHT[7], User.Identity.Name, Request.UserHostAddress);
                }
            }

            return RedirectToAction("Index", "KTCS_TinhHachToan", new { iTuThang = iTuThangHachToan, iDenThang = iDenThangHachToan });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult F4_Submit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            NameValueCollection arrLoi = new NameValueCollection();
            String siID_MaLoaiTaiSan = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiTaiSan"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            Boolean bDuLieuMoi = false;

            String iID_MaKyHieuHachToan = Convert.ToString(Request.Form[ParentID + "_iID_MaKyHieuHachToan"]);
            String iID_MaChungTuChiTiet = Convert.ToString(Request.Form[ParentID + "_iID_MaChungTuChiTiet"]);
            //Lấy dt cấu hình chi tiết
            //DataTable dtCauHinhCT = KTCS_CauHinhHachToanModels.Get_dtCauHinhHachToanChiTiet(iID_MaKyHieuHachToan);
            //Xóa hạch toán trước khi lưu
            String SQL = "DELETE FROM KTCS_ChungTuChiTietHachToan WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            //NGIA#TANG#GIAM#HMON#LKHM#CONLAI#CONLAITKH#NAMKHCL
            String NGIA = Request.Form[ParentID + "_NGIA"];
            String TANG = Request.Form[ParentID + "_TANG"];
            String GIAM = Request.Form[ParentID + "_GIAM"];
            String HMON = Request.Form[ParentID + "_HMON"];
            String LKHM = Request.Form[ParentID + "_LKHM"];
            String CONLAI = Request.Form[ParentID + "_CONLAI"];
            String CONLAITKH = Request.Form[ParentID + "_CONLAITKH"];
            String NAMKHCL = Request.Form[ParentID + "_NAMKHCL"];
            //Thêm mới hạch toán
            KTCS_HachToanModels.ThemMoiHachToan(iID_MaChungTuChiTiet, iID_MaKyHieuHachToan, NGIA, TANG, GIAM, HMON, LKHM, CONLAI, CONLAITKH, NAMKHCL, User.Identity.Name, Request.UserHostAddress);

            return RedirectToAction("Index");

        }

    }
}
