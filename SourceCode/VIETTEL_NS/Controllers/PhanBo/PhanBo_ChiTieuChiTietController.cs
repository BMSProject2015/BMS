using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Data;
using System.Data.SqlClient;
using VIETTEL.Models;

namespace VIETTEL.Controllers.PhanBo
{
    public class PhanBo_ChiTieuChiTietController : Controller
    {
        //
        // GET: /PhanBo_ChiTieuChiTiet/
        public string sViewPath = "~/Views/PhanBo/ChiTieuChiTiet/";

        [Authorize]
        public ActionResult Index(String iID_MaChiTieu)
        {
            return View(sViewPath + "ChiTieuChiTiet_Index.aspx");
        }

        [Authorize]
        public ActionResult ChiTieuChiTiet_Frame(String iID_MaChiTieu)
        {
            return View(sViewPath + "ChiTieuChiTiet_Index_DanhSach_Frame.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            String TenBangChiTiet = "PB_ChiTieuChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];

            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
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
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    //Them tham so
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
                        }
                    }
                    bang.Save();
                    //update chi tiêu vào phân bổ
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrMaCot[j].StartsWith("r") && arrMaCot[j].IndexOf("_")<0)
                        {
                            String SQL = String.Format(@"UPDATE PB_PhanBoChiTiet  SET {0}=ISNULL( (
                                            SELECT {0}={0}ChiTieu-ISNULL({0}PhanBo,0) FROM (
                                         SELECT {0} as {0}ChiTieu,sXauNoiMa FROM PB_ChiTieuChiTiet WHERE iTrangThai=1 AND iID_MaChiTieuChiTiet=@iID_MaChiTieuChiTiet) as a
                                            LEFT JOIN ( 
                                                SELECT SUM({0}) as {0}PhanBo, sXauNoiMa
                                                FROM PB_PhanBoChiTiet 
                                                WHERE iTrangThai=1 AND iID_MaDonVi<>@iID_MaDonVi
                AND sXauNoiMa=(SELECT sXauNoiMa FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieuChiTiet=@iID_MaChiTieuChiTiet) 
                AND iID_MaChiTieu=(SELECT iID_MaChiTieu FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieuChiTiet=@iID_MaChiTieuChiTiet)
                                            GROUP BY sXauNoiMa ) as b
            ON a.sXauNoiMa=b.sXauNoiMa),0)
                                           WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND sXauNoiMa=(SELECT sXauNoiMa FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieuChiTiet=@iID_MaChiTieuChiTiet) AND iID_MaChiTieu=(SELECT iID_MaChiTieu FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieuChiTiet=@iID_MaChiTieuChiTiet)", arrMaCot[j]);
                            SqlCommand cmd = new SqlCommand(SQL);
                            cmd.Parameters.AddWithValue("@iID_MaChiTieuChiTiet", arrMaHang[i]);
                            cmd.Parameters.AddWithValue("@iID_MaDonVi", PhanBo_PhanBoNganh_BangDuLieu.iID_MaDonViChoPhanBo);
                            Connection.UpdateDatabase(cmd);
                            cmd.Dispose();

                        }
                    }

                }
            }


            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "PhanBo_ChiTieu", new { iID_MaChiTieu = iID_MaChiTieu });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "PhanBo_ChiTieu", new { iID_MaChiTieu = iID_MaChiTieu });
            }
            return RedirectToAction("ChiTieuChiTiet_Frame", new { iID_MaChiTieu = iID_MaChiTieu });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaChiTieu)
        {
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Index", new { iID_MaChiTieu = iID_MaChiTieu, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }


    }
}