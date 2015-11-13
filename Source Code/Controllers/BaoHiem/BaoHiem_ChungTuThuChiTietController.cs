using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;

namespace VIETTEL.Controllers.BaoHiem
{
    public class BaoHiem_ChungTuThuChiTietController : Controller
    {
        //
        // GET: /BaoHiem_ChungTuThuChiTiet/
        public string sViewPath = "~/Views/BaoHiem/ChungTuThuChiTiet/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "BaoHiem_ChungTuThuChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTuThu)
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
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
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
                    Bang bang = new Bang("BH_ChungTuThuChiTiet");
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
                }
            }

            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "BaoHiem_ChungTuThu", new { iID_MaChungTuThu = iID_MaChungTuThu });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "BaoHiem_ChungTuThu", new { iID_MaChungTuThu = iID_MaChungTuThu });
            }
            return RedirectToAction("Index", new { iID_MaChungTuThu = iID_MaChungTuThu });
        }
    }
}
