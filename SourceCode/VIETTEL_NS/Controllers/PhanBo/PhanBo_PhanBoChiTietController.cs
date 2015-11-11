using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Data.SqlClient;
namespace VIETTEL.Controllers.PhanBo
{
    public class PhanBo_PhanBoChiTietController : Controller
    {
        //
        // GET: /PhanBo_PhanBoChiTiet/
        public string sViewPath = "~/Views/PhanBo/PhanBoChiTiet/";

        [Authorize]
        public ActionResult Index(String iID_MaPhanBo)
        {
            return View(sViewPath + "PhanBoChiTiet_Index.aspx");
        }

        [Authorize]
        public ActionResult PhanBoChiTiet_Frame(String iID_MaPhanBo)
        {
            return View(sViewPath + "PhanBoChiTiet_Index_DanhSach_Frame.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            String TenBangChiTiet = "PB_PhanBoChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachO }, StringSplitOptions.None);
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
                    SqlCommand cmd = new SqlCommand();
                    String SQL = "UPDATE PB_PhanBoChiTiet SET ";
                    String DK = "";
                    Bang bang = new Bang(TenBangChiTiet);
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    //Them tham so
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            if (arrMaCot[j].EndsWith("_ConLai") == false && arrMaCot[j].EndsWith("_ChiTieu") == false)
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
                                else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
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
                            else
                            {
                                if (arrMaCot[j].IndexOf("ChiTieu") < 0)
                                {

                                    String Truong = "@" + arrMaCot[j].Substring(0, arrMaCot[j].IndexOf('_'));

                                    DK += arrMaCot[j].Substring(0, arrMaCot[j].IndexOf('_')) + "=" + Truong + ",";
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1")
                                        {
                                            cmd.Parameters.AddWithValue(Truong, true);
                                        }
                                        else
                                        {
                                            cmd.Parameters.AddWithValue(Truong, false);
                                        }
                                    }
                                    else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            cmd.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        cmd.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }
                                }
                            }

                        }
                    }
                    //lưu số tiền còn lại vào đơn chờ phân bổ 
                    DK = DK.Remove(DK.Length - 1);
                    SQL = SQL + DK + " WHERE iID_MaPhanBoChiTiet=@iID_MaPhanBoChiTiet";
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaPhanBoChiTiet", LayMaHangChoPhanBo(arrMaHang[i], iID_MaPhanBo));
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    bang.Save();
                }
            }

            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "PhanBo_PhanBo", new { iID_MaPhanBo = iID_MaPhanBo });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "PhanBo_PhanBo", new { iID_MaPhanBo = iID_MaPhanBo });
            }
            return RedirectToAction("PhanBoChiTiet_Frame", new { iID_MaPhanBo = iID_MaPhanBo });
        }

        public static String LayMaHangChoPhanBo(String MaHang, String iID_MaPhanBo)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT iID_MaChiTieu FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            String iID_MaChiTieu = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            cmd = new SqlCommand();
            SQL = "SELECT iID_MaPhanBoChiTiet FROM PB_PhanBoChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu AND iID_MaDonVi='99' AND sXauNoiMa=(SELECT sXauNoiMa FROM PB_PhanBoChiTiet WHERE iID_MaPhanBoChiTiet=@iID_MaPhanBoChiTiet)";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhanBoChiTiet", MaHang);
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaPhanBo)
        {
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Index", new { iID_MaPhanBo = iID_MaPhanBo, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }

        public ActionResult TaoMoi(String iID_MaDonVi, String iID_MaChiTieu)
        {

            String iID_MaPhanBo = PhanBo_PhanBoChiTietModels.ThemPhanBoChoDonVi(iID_MaChiTieu, iID_MaDonVi, User.Identity.Name, Request.UserHostAddress);
            return RedirectToAction("Index", new { iID_MaPhanBo = iID_MaPhanBo });
        }
    }
}