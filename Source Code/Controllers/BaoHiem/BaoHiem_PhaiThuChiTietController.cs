using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Data.SqlClient;

namespace VIETTEL.Controllers.BaoHiem
{
    public class BaoHiem_PhaiThuChiTietController : Controller
    {
        //
        // GET: /BaoHiem_PhaiThuChiTiet/
        public string sViewPath = "~/Views/BaoHiem/PhaiThuChiTiet/";
        [Authorize]
        public ActionResult Index(String bChi)
        {
            ViewData["bChi"] = bChi;
            return View(sViewPath + "BaoHiem_PhaiThuChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaBaoHiemPhaiThu, String bChi)
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
                    Bang bang = new Bang("BH_PhaiThuChungTuChiTiet");
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    //Them tham so

                    Double TongSo = 0;
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
                                        if (arrMaCot[j].StartsWith("rTien"))
                                            TongSo = TongSo + Convert.ToDouble(arrGiaTri[j]);
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

                    bang.CmdParams.Parameters.AddWithValue("@rTongSo", TongSo);                    
                    bang.Save();
                }
            }

            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "BaoHiem_PhaiThu", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu});
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "BaoHiem_PhaiThu", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu});
            }
            return RedirectToAction("Index", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu});
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LayLaiSoLieu(String iID_MaBaoHiemPhaiThu)
        {
            String MaND = User.Identity.Name;
            String SQL = "DELETE FROM BH_PhaiThuChungTuChiTiet WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu",iID_MaBaoHiemPhaiThu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            SQL = "DELETE FROM BH_DuyetPhaiThuChiTiet WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu",iID_MaBaoHiemPhaiThu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            
            BaoHiem_PhaiThuChiTietModels.ThemChiTiet(iID_MaBaoHiemPhaiThu, MaND, Request.UserHostAddress);

            BaoHiem_PhaiThuModels.InsertDuyetChungTu(iID_MaBaoHiemPhaiThu, "Mới mới", User.Identity.Name, Request.UserHostAddress);
            
            return RedirectToAction("Index", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu });
        }
    }
}
