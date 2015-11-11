using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using DomainModel.Abstract;
using System.Data.SqlClient;
using System.Collections.Specialized;
using VIETTEL.Models;


namespace VIETTEL.Controllers.DungChung
{
    public class DanhMuc_BaoCao_ChuKyController : Controller
    {
        //
        // GET: /DanhMuc_BaoCao_ChuKy/

        public string sViewPath = "~/Views/DungChung/DanhMuc_BaoCao_ChuKy/";

        [Authorize]
        public ActionResult Index(String iID_MaPhanHe)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["iID_MaPhanHe"] = iID_MaPhanHe;
                return View(sViewPath + "DanhMucBaoCaoChuKy_EditIndex.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string iID_MaBaoCao_ChuKy)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("NS_DanhMuc_BaoCao_ChuKy");
                bang.GiaTriKhoa = iID_MaBaoCao_ChuKy;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
                if (dicData != null)
                {
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Detail.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string iID_MaBaoCao_ChuKy)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("NS_DanhMuc_BaoCao_ChuKy");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = iID_MaBaoCao_ChuKy;
                bang.Delete();
                return RedirectToAction("Index", "DanhMuc_BaoCao_ChuKy");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaBaoCao_ChuKy, String iID_MaPhanHe)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["iID_MaBaoCao_ChuKy"] = iID_MaBaoCao_ChuKy;

                NameValueCollection data = new NameValueCollection();
                if (String.IsNullOrEmpty(iID_MaBaoCao_ChuKy) == false)
                {

                    ViewData["DuLieuMoi"] = "0";
                    data = DanhMuc_BaoCao_ChuKyModels.LayThongTinBaoCaoChuKy(iID_MaBaoCao_ChuKy);
                }
                else
                {
                    data["iID_MaPhanHe"] = iID_MaPhanHe;
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["data"] = data;
                return View(sViewPath + "DanhMucBaoCaoChuKy_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }


        public ActionResult Loc(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String iID_MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
                ViewData["iID_MaPhanHe"] = iID_MaPhanHe;
                return View(sViewPath + "DanhMucBaoCaoChuKy_EditIndex.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaPhanHe)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {

                // NameValueCollection arrLoi = new NameValueCollection();//bang.TruyenGiaTri(ParentID, Request.Form);

                String sMaBaoCao_ChuKy = Request.Form[ParentID + "_iID_MaBaoCao_ChuKy"];


                String[] arrMaBaoCao_ChuKy = sMaBaoCao_ChuKy.Split(',');

                String iID_MaChucDanh1 = "",
                       iID_MaThuaLenh1 = "",
                       iID_MaChucDanh2 = "",
                       iID_MaThuaLenh2 = "",
                       iID_MaChucDanh3 = "",
                       iID_MaThuaLenh3 = "",
                       iID_MaChucDanh4 = "",
                       iID_MaThuaLenh4 = "",
                       iID_MaChucDanh5 = "",
                       iID_MaThuaLenh5 = "";
                String iID_MaTen1 = "", iID_MaTen2 = "", iID_MaTen3 = "", iID_MaTen4 = "", iID_MaTen5 = "";

                for (int i = 0; i < arrMaBaoCao_ChuKy.Length; i++)
                {
                    iID_MaChucDanh1 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaChucDanh1"];
                    iID_MaChucDanh2 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaChucDanh2"];
                    iID_MaChucDanh3 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaChucDanh3"];
                    iID_MaChucDanh4 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaChucDanh4"];
                    iID_MaChucDanh5 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaChucDanh5"];

                    iID_MaThuaLenh1 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaThuaLenh1"];
                    iID_MaThuaLenh2 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaThuaLenh2"];
                    iID_MaThuaLenh3 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaThuaLenh3"];
                    iID_MaThuaLenh4 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaThuaLenh4"];
                    iID_MaThuaLenh5 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaThuaLenh5"];

                    iID_MaTen1 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaTen1"];
                    iID_MaTen2 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaTen2"];
                    iID_MaTen3 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaTen3"];
                    iID_MaTen4 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaTen4"];
                    iID_MaTen5 = Request.Form[ParentID + "_" + arrMaBaoCao_ChuKy[i] + "_iID_MaTen5"];


                    Bang bang = new Bang("NS_DanhMuc_BaoCao_ChuKy");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.DuLieuMoi = false;

                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChucDanh1", iID_MaChucDanh1);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChucDanh2", iID_MaChucDanh2);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChucDanh3", iID_MaChucDanh3);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChucDanh4", iID_MaChucDanh4);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChucDanh5", iID_MaChucDanh5);

                    bang.CmdParams.Parameters.AddWithValue("@sTenChucDanh1",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaChucDanh1));
                    bang.CmdParams.Parameters.AddWithValue("@sTenChucDanh2",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaChucDanh2));
                    bang.CmdParams.Parameters.AddWithValue("@sTenChucDanh3",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaChucDanh3));
                    bang.CmdParams.Parameters.AddWithValue("@sTenChucDanh4",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaChucDanh4));
                    bang.CmdParams.Parameters.AddWithValue("@sTenChucDanh5",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaChucDanh5));


                    bang.CmdParams.Parameters.AddWithValue("@iID_MaThuaLenh1", iID_MaThuaLenh1);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaThuaLenh2", iID_MaThuaLenh2);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaThuaLenh3", iID_MaThuaLenh3);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaThuaLenh4", iID_MaThuaLenh4);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaThuaLenh5", iID_MaThuaLenh5);

                    bang.CmdParams.Parameters.AddWithValue("@sTenThuaLenh1",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaThuaLenh1));
                    bang.CmdParams.Parameters.AddWithValue("@sTenThuaLenh2",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaThuaLenh2));
                    bang.CmdParams.Parameters.AddWithValue("@sTenThuaLenh3",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaThuaLenh3));
                    bang.CmdParams.Parameters.AddWithValue("@sTenThuaLenh4",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaThuaLenh4));
                    bang.CmdParams.Parameters.AddWithValue("@sTenThuaLenh5",
                                                           DanhMucChuKyModels.LayTenChuKy(iID_MaThuaLenh5));


                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTen1", iID_MaTen1);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTen2", iID_MaTen2);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTen3", iID_MaTen3);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTen4", iID_MaTen4);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTen5", iID_MaTen5);


                    bang.CmdParams.Parameters.AddWithValue("@sTen1", DanhMucChuKyModels.LayTenChuKy(iID_MaTen1));
                    bang.CmdParams.Parameters.AddWithValue("@sTen2", DanhMucChuKyModels.LayTenChuKy(iID_MaTen2));
                    bang.CmdParams.Parameters.AddWithValue("@sTen3", DanhMucChuKyModels.LayTenChuKy(iID_MaTen3));
                    bang.CmdParams.Parameters.AddWithValue("@sTen4", DanhMucChuKyModels.LayTenChuKy(iID_MaTen4));
                    bang.CmdParams.Parameters.AddWithValue("@sTen5", DanhMucChuKyModels.LayTenChuKy(iID_MaTen5));

                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
                    bang.GiaTriKhoa = arrMaBaoCao_ChuKy[i];
                    bang.Save();
                }


                ViewData["iID_MaPhanHe"] = iID_MaPhanHe;
                return View(sViewPath + "DanhMucBaoCaoChuKy_EditIndex.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_Controller(String ParentID, String iID_MaBaoCao_ChuKy)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_DanhMuc_BaoCao_ChuKy", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                
                String iID_MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
                Bang bang = new Bang("NS_DanhMuc_BaoCao_ChuKy");
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
                String sController = Request.Form[ParentID + "_sController"];
                String MaND=User.Identity.Name;
                String GiaTri=sController+","+MaND;
                    
                bang.MaNguoiDungSua = MaND;
                bang.IPSua = Request.UserHostAddress;
                if (HamChung.Check_Trung("NS_DanhMuc_BaoCao_ChuKy", "iID_MaBaoCao_ChuKy", iID_MaBaoCao_ChuKy, "sController,sID_MaNguoiDungTao",GiaTri, bang.DuLieuMoi) == false)
                {
                    bang.Save();
                }

                return RedirectToAction("Index", new {iID_MaPhanHe = iID_MaPhanHe});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CopyTuTaiKhoanKhac(String ParentID)
        {
            String sID_MaNguoiDungTao = User.Identity.Name;
            String MaND = Request.Form[ParentID + "_sID_MaNguoiDungTao"];
            String iID_MaPhanHe = Request.Form[ParentID + "_iID_MaPhanHe"];
            String SQL=String.Format(@"INSERT INTO NS_DanhMuc_BaoCao_ChuKy(
            sTenBaoCao,sController,iID_MaPhanHe
            ,iID_MaChucDanh1,iID_MaChucDanh2,iID_MaChucDanh3,iID_MaChucDanh4,iID_MaChucDanh5,
            sTenChucDanh1,sTenChucDanh2,sTenChucDanh3,sTenChucDanh4,sTenChucDanh5,
            iID_MaThuaLenh1,iID_MaThuaLenh2,iID_MaThuaLenh3,iID_MaThuaLenh4,iID_MaThuaLenh5,
            sTenThuaLenh1,sTenThuaLenh2,sTenThuaLenh3,sTenThuaLenh4,sTenThuaLenh5,
            iID_MaTen1,iID_MaTen2,iID_MaTen3,iID_MaTen4,iID_MaTen5,
            sTen1,sTen2,sTen3,sTen4,sTen5,
            iSTT,iTrangThai,bPublic,
            iID_MaNhomNguoiDung_Public,
            iID_MaNhomNguoiDung_DuocGiao,
            sID_MaNguoiDung_DuocGiao,
            sID_MaNguoiDungTao,
            sID_MaNguoiDungSua
            )
            SELECT sTenBaoCao,sController,iID_MaPhanHe
            ,iID_MaChucDanh1,iID_MaChucDanh2,iID_MaChucDanh3,iID_MaChucDanh4,iID_MaChucDanh5,
            sTenChucDanh1,sTenChucDanh2,sTenChucDanh3,sTenChucDanh4,sTenChucDanh5,
            iID_MaThuaLenh1,iID_MaThuaLenh2,iID_MaThuaLenh3,iID_MaThuaLenh4,iID_MaThuaLenh5,
            sTenThuaLenh1,sTenThuaLenh2,sTenThuaLenh3,sTenThuaLenh4,sTenThuaLenh5,
            iID_MaTen1,iID_MaTen2,iID_MaTen3,iID_MaTen4,iID_MaTen5,
            sTen1,sTen2,sTen3,sTen4,sTen5,
            iSTT,iTrangThai,bPublic,
            iID_MaNhomNguoiDung_Public,
            iID_MaNhomNguoiDung_DuocGiao,
            '{0}',
            '{0}',
            '{0}'
            FROM NS_DanhMuc_BaoCao_ChuKy
            WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao AND iID_MaPhanHe=@iID_MaPhanHe", sID_MaNguoiDungTao);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            Connection.UpdateDatabase(cmd);
            return RedirectToAction("Index", new { iID_MaPhanHe = iID_MaPhanHe });
        }
    }
}
