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

namespace VIETTEL.Controllers.DuToan
{
    public class MucLucNganSachController : Controller
    {
        //
        // GET: /MucLucNganSach/
        public string sViewPath = "~/Views/DungChung/MucLucNganSach/";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {

                return View(sViewPath + "MucLucNganSach_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit(String MaMucLucNganSach,String LuuThanhCong)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "MucLucNganSach_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult CauHinh()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "MucLucNganSach_CauHinh.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CauHinh_Submit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String sLNS = Request.Form[ParentID + "_sLNS"];
                String[] arrLNS = sLNS.Split(',');
                String strDSDuocNhapTruongTien = MucLucNganSachModels.strDSDuocNhapTruongTien + ",sNhapTheoTruong";
                String[] arr1 = strDSDuocNhapTruongTien.Split(',');
                SqlCommand cmd;
                String SQL = "UPDATE NS_MucLucNganSach SET ";
                for (int j = 0; j < arr1.Length; j++)
                {
                    SQL += arr1[j] + "=@" + arr1[j] + ",";
                    if (arr1[j] != "sNhapTheoTruong" && arr1[j] != "bsTenCongTrinh")
                    {
                        SQL += arr1[j] + "_Donvi=@" + arr1[j] + "_DonVi,";
                    }
                }
                SQL = SQL.Remove(SQL.Length - 1, 1);
                SQL += " WHERE sLNS=@sLNS";


                object GiaTri;
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@sLNS", arrLNS[i]);
                    for (int j = 0; j < arr1.Length; j++)
                    {
                        if (arr1[j] != "sNhapTheoTruong" )
                        {
                            GiaTri = Request.Form[arr1[j] + "_" + arrLNS[i]];
                            if (Convert.ToString(GiaTri) == "on")
                            {
                                GiaTri = true;
                            }
                            else
                            {
                                GiaTri = false;
                            }
                            cmd.Parameters.AddWithValue("@" + arr1[j], GiaTri);
                            if (arr1[j] != "bsTenCongTrinh")
                            {
                                cmd.Parameters.AddWithValue("@" + arr1[j] + "_DonVi", GiaTri);
                            }
                        }
                        else
                        {
                            GiaTri = Request.Form[arr1[j] + "_" + arrLNS[i]];
                            cmd.Parameters.AddWithValue("@" + arr1[j], GiaTri);
                        }
                    }

                    try
                    {
                        Connection.UpdateDatabase(cmd);
                    }
                    catch
                    {
                        return RedirectToAction("CauHinh", "MucLucNganSach");

                    }
                }
                return RedirectToAction("CauHinh");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String sLNS)
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String TenBangChiTiet = "NS_MucLucNganSach";
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

                String iID_MaChungTuChiTiet;
                //Luu cac hang sua
                String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] {BangDuLieu.DauCachHang},
                                                                   StringSplitOptions.None);
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
                                        if (arrGiaTri[j] == "1")
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
                            if (iID_MaChungTuChiTiet == "")
                            {
                                bang.CmdParams.Parameters.RemoveAt(bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucNganSach"));
                                bang.CmdParams.Parameters.RemoveAt(bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucNganSach_Cha"));
                            }
                            bang.Save();
                        }
                    }
                }

                MucLucNganSachModels.CapNhapLai(sLNS);
               
                return RedirectToAction("Edit", "MucLucNganSach", new {sLNS = sLNS,LuuThanhCong="1"});

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LocSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String sLNS = Request.Form[ParentID + "_sLNS"];
                return RedirectToAction("Edit", new {sLNS = sLNS});

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
