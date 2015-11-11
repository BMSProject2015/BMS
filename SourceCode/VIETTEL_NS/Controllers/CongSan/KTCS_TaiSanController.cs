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
    public class KTCS_TaiSanController : Controller
    {
        //
        // GET: /KTCS_TaiSan/
        public string sViewPath = "~/Views/CongSan/TaiSan/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_TaiSan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KTCS_TaiSan_Index.aspx");
        }

        [Authorize]
        public ActionResult List()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_TaiSan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KTCS_TaiSan_List.aspx");
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String List = Request.Form[ParentID + "_List"];
            String iLoai = Request.Form[ParentID + "_slLoaiTS"];
            String iID_MaNhomTaiSan = Request.Form[ParentID + "_iID_MaNhomTaiSan"];
            String iNamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            String iID_MaTaiSan = Request.Form[ParentID + "_iID_MaTaiSan"];
            if (List == "List")
            {
                return RedirectToAction("List", "KTCS_TaiSan", new { iLoai = iLoai, iID_MaNhomTaiSan = iID_MaNhomTaiSan,iNamLamViec=iNamLamViec,iID_MaTaiSan=iID_MaTaiSan });
            }
            else
            {
                return RedirectToAction("Index", "KTCS_TaiSan", new { iLoai = iLoai, iID_MaNhomTaiSan = iID_MaNhomTaiSan });
            }
        }

        [Authorize]
        public ActionResult ThanhLy(String iID_MaTaiSan)
        {
            SqlCommand cmd = new SqlCommand("UPDATE KTCS_TaiSan SET bDaKhauHao=0 WHERE iID_MaTaiSan = @iID_MaTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan",iID_MaTaiSan);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("List", "KTCS_TaiSan", new {iID_MaTaiSan = iID_MaTaiSan });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iLoai, String iID_MaLoaiTaiSan)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "KTCS_TaiSan";

            //Lấy thông tin bảng quyết định
            NameValueCollection data = KTCS_TaiSanModels.LayThongTin(iLoai, iID_MaLoaiTaiSan);

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaDanhMucDuAnChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaDanhMucDuAnChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaDanhMucDuAnChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
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
                    Boolean bMaTaiSan_Null = false;
                    String iID_MaTaiSan = "",iID_MaDonVi="";
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrMaCot[j] == "iID_MaTaiSan" && (arrGiaTri[j] == "" && ((iID_MaDanhMucDuAnChiTiet != "" && arrThayDoi[j] == "1") || iID_MaDanhMucDuAnChiTiet=="")))
                        {
                            bMaTaiSan_Null = true;
                        }
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            if(j>3)//Đợi chạy qua cột iID_MaTaiSan thì break
                                break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        if (bMaTaiSan_Null == false)
                        {
                            Bang bang = new Bang(TenBangChiTiet);
                            iID_MaDanhMucDuAnChiTiet = arrMaHang[i];
                            if (iID_MaDanhMucDuAnChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
                                bang.DuLieuMoi = false;
                                iID_MaTaiSan = Convert.ToString(CommonFunction.LayTruong("KTCS_TaiSan", "iID_MaBangTaiSan",iID_MaDanhMucDuAnChiTiet,"iID_MaTaiSan"));
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

                                    if (arrMaCot[j] == "iID_MaDonVi")
                                    {
                                        iID_MaDonVi = arrGiaTri[j];
                                    }
                                    if (arrMaCot[j] == "iID_MaTaiSan")
                                    {
                                        iID_MaTaiSan = arrGiaTri[j];
                                    }
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

                            //if (iID_MaDanhMucDuAnChiTiet == "")
                            //{
                            //    //Them chung tu - chung tu chi tiet
                            //    for (int j = 0; j < arrMaCot.Length; j++)
                            //    {
                            //        if (arrThayDoi[j] == "1")
                            //        {
                            //            if (arrMaCot[j] == "iID_MaTaiSan")
                            //            {
                            //                iID_MaTaiSan = arrGiaTri[j];
                            //                KTCS_ChungTuModels.InsertRecord_ChungTu_ChungTuChiTiet(iID_MaTaiSan, User.Identity.Name, Request.UserHostAddress);
                            //            }
                            //        }
                            //    }
                            //}
                            //Thêm bảng KTCS_TaiSanDonVi
                            KTCS_TaiSan_DonViModels.ThemDieuChuyenTaiSan(iID_MaDonVi, iID_MaTaiSan, User.Identity.Name, Request.UserHostAddress);
                        }
                    }
                }
            }
            return RedirectToAction("Index", new { iLoai = iLoai, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan });
        }

        [Authorize]
        public JsonResult get_Check_MaTaiSan(String iID_MaTaiSan)
        {
            Boolean strGiaTri = KTCS_TaiSanModels.KiemTra_MaTaiSan(iID_MaTaiSan);
            return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChiTiet_Submit(String ParentID)
        {
            Bang bang = new Bang("KTCS_TaiSanChiTiet");                        
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            String dNgayMua = Convert.ToString(Request.Form[ParentID + "_vidNgayMua"]);
            String dNgayKhauHao = Convert.ToString(Request.Form[ParentID + "_vidNgayDuaVaoKhauHao"]);
            String iID_MaTaiSan = Request.Form[ParentID + "_iID_MaTaiSan"];
            DataTable dt =KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
            bang.CmdParams.Parameters.AddWithValue("dNgayMua", CommonFunction.LayNgayTuXau(dNgayMua));
            bang.CmdParams.Parameters.AddWithValue("dNgayKhauHao", CommonFunction.LayNgayTuXau(dNgayKhauHao));
            bang.Save();
            String SQL = "UPDATE KTCS_TaiSan SET dNgayMua=@dNgayMua,dNgayDuaVaoKhauHao=@dNgayDuaVaoKhauHao WHERE iID_MaTaiSan=@iID_MaTaiSan";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgayMua", CommonFunction.LayNgayTuXau(dNgayMua));
            cmd.Parameters.AddWithValue("@dNgayDuaVaoKhauHao", CommonFunction.LayNgayTuXau(dNgayKhauHao));
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            Connection.UpdateDatabase(cmd);
            String _in=Request.Form[ParentID+"_in"];
            //if (_in == "0")
            //{
            //    return RedirectToAction("ViewPDF", "rptCongSan_ChiTietTaiSan", new { iID_MaTaiSan = iID_MaTaiSan });
            //}
            return RedirectToAction(ParentID, "KTCS_TaiSan", new { Saved = 1, In = _in, iID_MaTaiSan = iID_MaTaiSan });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TrangThai_Submit(String ParentID)
        {
            String iID_MaTaiSan = Request.Form[ParentID + "_iID_MaTaiSan"];
            String iTrangThaiTaiSan = Request.Form[ParentID + "_iTrangThaiTaiSan"];
            String DK = "iTrangThaiTaiSan=@iTrangThaiTaiSan ";
            if (iTrangThaiTaiSan == "3")
            {
                DK += ",bDaKhauHao=0 ";
            }
            SqlCommand cmd = new SqlCommand("UPDATE KTCS_TaiSan SET "+DK+" WHERE iID_MaTaiSan=@iID_MaTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            cmd.Parameters.AddWithValue("@iTrangThaiTaiSan", iTrangThaiTaiSan);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("List", "KTCS_TaiSan", new { Saved = 1});
        }


        [Authorize]
        public JsonResult get_GiaTriSoNamSuDung(String iID_MaNhomTaiSan)
        {
            Double rSoNamKhauHao = 0;
            String vR = "";
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iID_MaNhomTaiSan = @iID_MaNhomTaiSan";
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);

            String SQL = String.Format("SELECT rSoNamKhauHao FROM KTCS_NhomTaiSan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            if (vR != "")
            {
                rSoNamKhauHao = Convert.ToDouble(vR);
            }

            Object item = new
            {
                rSoNamKhauHao = rSoNamKhauHao,
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TaiSan_Dat()
        {
            return View("~/Views/CongSan/ChiTietTaiSan/CongSan_Dialog_ChiTietTaiSan_Dat.aspx");
        }
        [Authorize]
        public ActionResult TaiSan_Nha()
        {
            return View("~/Views/CongSan/ChiTietTaiSan/CongSan_Dialog_ChiTietTaiSan_Nha.aspx");
        }
        [Authorize]
        public ActionResult TaiSan_Oto()
        {
            return View("~/Views/CongSan/ChiTietTaiSan/CongSan_Dialog_ChiTietTaiSan_OTo.aspx");
        }
        [Authorize]
        public ActionResult TaiSan_Tren500Trieu()
        {
            return View("~/Views/CongSan/ChiTietTaiSan/CongSan_Dialog_ChiTietTaiSan_Tren500Trieu.aspx");
        }
        [Authorize]
        public ActionResult TaiSan_Chung()
        {
            return View("~/Views/CongSan/ChiTietTaiSan/CongSan_Dialog_ChiTietTaiSan_Chung.aspx");
        }
        
    }
}
