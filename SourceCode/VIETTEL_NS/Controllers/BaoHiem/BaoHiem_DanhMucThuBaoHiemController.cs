using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.BaoHiem
{
    public class BaoHiem_DanhMucThuBaoHiemController : Controller
    {
        //
        // GET: /BaoHiem_DanhMucThuBaoHiem/

        public string sViewPath = "~/Views/BaoHiem/DanhMucThuBaoHiem/";
        public ActionResult Index()
        {
            return View(sViewPath + "BaoHiem_DanhMucThuBaoHiem_Index.aspx");
        }
        [Authorize]
        public ActionResult Edit()
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(MaND, "BH_DanhMucThuBaoHiem", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }

            ViewData["DuLieuMoi"] = "1";
            return View(sViewPath + "BaoHiem_DanhMucThuBaoHiem_Edit.aspx");

        }
        [Authorize]
        public ActionResult Detail()
        {
            String MaND = User.Identity.Name;
            if (BaoMat.ChoPhepLamViec(MaND, "BH_DanhMucThuBaoHiem", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }

            return View(sViewPath + "BaoHiem_DanhMucThuBaoHiem_Detail_Index.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaDanhMucThuBaoHiem)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";



            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "BH_DanhMucThuBaoHiem", sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String Thang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String ThangCopy = Convert.ToString(Request.Form[ParentID + "_iThangCopy"]);
            String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
            if (DanhMucThuBaoHiemModels.Check_Trung(iNamLamViec, Thang))
            {
                arrLoi.Add("err_iThang", "Đã có dữ liệu của tháng " + Thang + " năm " + iNamLamViec);
            }

            if (String.IsNullOrEmpty(Thang) || Thang=="-1")
            {
                arrLoi.Add("err_iThang", "Bạn chưa chọn tháng!");
            }
            if (Thang != "1" && ThangCopy == "-1")
            {
                arrLoi.Add("err_iThangCopy", "Bạn chưa chọn tháng copy!");
            }
            if (Convert.ToInt16(Thang) < Convert.ToInt16(ThangCopy))
            {
                arrLoi.Add("err_iThang", "Tháng chọn bé hơn tháng copy!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaDanhMucThuBaoHiem))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["MaDanhMucBaoHiem"] = MaDanhMucThuBaoHiem;
                ViewData["Thang"] = Thang;
                ViewData["ThangCopy"] = ThangCopy;
                return View(sViewPath + "BaoHiem_DanhMucThuBaoHiem_Edit.aspx");
            }
            else
            {
                String[] arrTruongTienBaoHiem = new String[] { "rBHXH_CN", "rBHYT_CN", "rBHTN_CN", "rBHXH_DV", "rBHYT_DV", "rBHTN_DV", "rBHXH_CS", "rLuongToiThieu" };
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];
                iNamLamViec = Convert.ToString(R["iNamLamViec"]);
                Bang bang = new Bang("BH_DanhMucThuBaoHiem");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucDoiTuong", "");
                bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucDoiTuong_Cha", "");
                bang.CmdParams.Parameters.AddWithValue("@sKyHieu", "");
                bang.CmdParams.Parameters.AddWithValue("@sMoTa", "");
                bang.CmdParams.Parameters.AddWithValue("@iSTT", "");
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", "");
                for (int j = 0; j < arrTruongTienBaoHiem.Length; j++)
                {
                    bang.CmdParams.Parameters.AddWithValue("@" + arrTruongTienBaoHiem[j], 0);
                }
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //copy từ một tháng

                    DataTable dtCopy = DanhMucThuBaoHiemModels.Get_DTDanhMucThuBaoHiemCopy(iNamLamViec,Thang,ThangCopy);
                    if (dtCopy.Rows.Count > 0)
                    {
                        for (i = 0; i < dtCopy.Rows.Count; i++)
                        {
                            bang.CmdParams.Parameters["@iID_MaMucLucDoiTuong"].Value = dtCopy.Rows[i]["iID_MaMucLucDoiTuong"];
                            bang.CmdParams.Parameters["@iID_MaMucLucDoiTuong_Cha"].Value = dtCopy.Rows[i]["iID_MaMucLucDoiTuong_Cha"];
                            bang.CmdParams.Parameters["@bLaHangCha"].Value = dtCopy.Rows[i]["bLaHangCha"];
                            bang.CmdParams.Parameters["@sKyHieu"].Value = dtCopy.Rows[i]["sKyHieu"];
                            bang.CmdParams.Parameters["@sMoTa"].Value = dtCopy.Rows[i]["sMoTa"];
                            bang.CmdParams.Parameters["@iSTT"].Value = dtCopy.Rows[i]["iSTT"];
                            for (int j = 0; j < arrTruongTienBaoHiem.Length; j++)
                            {
                                bang.CmdParams.Parameters["@" + arrTruongTienBaoHiem[j]].Value = dtCopy.Rows[i][arrTruongTienBaoHiem[j]];
                            }
                            bang.Save();
                        }
                    }
                    else
                    {
                        DataTable dtMucLucDoiTuong = NganSach_DoiTuongModels.DT_MucLucDoiTuong();
                        for (i = 0; i < dtMucLucDoiTuong.Rows.Count; i++)
                        {
                            bang.CmdParams.Parameters["@iID_MaMucLucDoiTuong"].Value = dtMucLucDoiTuong.Rows[i]["iID_MaMucLucDoiTuong"];
                            bang.CmdParams.Parameters["@iID_MaMucLucDoiTuong_Cha"].Value = dtMucLucDoiTuong.Rows[i]["iID_MaMucLucDoiTuong_Cha"];
                            bang.CmdParams.Parameters["@bLaHangCha"].Value = dtMucLucDoiTuong.Rows[i]["bLaHangCha"];
                            bang.CmdParams.Parameters["@sKyHieu"].Value = dtMucLucDoiTuong.Rows[i]["sKyHieu"];
                            bang.CmdParams.Parameters["@sMoTa"].Value = dtMucLucDoiTuong.Rows[i]["sMoTa"];
                            bang.CmdParams.Parameters["@iSTT"].Value = dtMucLucDoiTuong.Rows[i]["iSTT"];
                            bang.Save();
                        }

                        dtMucLucDoiTuong.Dispose();
                    }
                }
                else
                {
                    bang.GiaTriKhoa = MaDanhMucThuBaoHiem;
                    bang.Save();
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Detail", "BaoHiem_DanhMucThuBaoHiem", new { iNamLamViec = iNamLamViec, iThang = Thang });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iNamLamViec, String iThang)
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
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BaoHiemThu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BaoHiemThu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BaoHiemThu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BaoHiemThu_BangDuLieu.DauCachO }, StringSplitOptions.None);
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
                    Bang bang = new Bang("BH_DanhMucThuBaoHiem");

                    if (String.IsNullOrEmpty(arrMaHang[i]) == false)
                    {
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = arrMaHang[i];
                    }
                    else
                    {
                        bang.DuLieuMoi = true;
                    }
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    bang.CmdParams.Parameters.AddWithValue("@iThang", iThang);
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
                                if (arrMaCot[j] == "iID_MaMucLucDoiTuong_Cha" && String.IsNullOrEmpty(arrGiaTri[j]))
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, DBNull.Value);
                                }
                                else
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                }
                            }
                        }
                    }
                    bang.Save();
                }
            }

            return RedirectToAction("Detail", "BaoHiem_DanhMucThuBaoHiem", new { iNamLamViec = iNamLamViec, iThang = iThang });
        }

    }
}
