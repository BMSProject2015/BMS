using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;


namespace VIETTEL.Controllers.TCDN
{
    public class TCDN_ChungTuChiTietController : Controller
    {
        //
        // GET: /TCDN_ChungTuChiTiet/
        public string sViewPath = "~/Views/TCDN/ChungTuChiTiet/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "TCDN_ChungTuChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu,String iLoai)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            NameValueCollection data = TCDN_ChungTuModels.LayThongTin(iID_MaChungTu);
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { TCDN_ChungTu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { TCDN_ChungTu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String iID_MaChungTuChiTiet, sKyHieu,sTen;
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                   
                    iID_MaChungTuChiTiet = arrMaHang[i].Split('_')[0];
                    sKyHieu = arrMaHang[i].Split('_')[1];

                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaChungTuChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang("TCDN_ChungTuChiTiet");
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
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] {TCDN_ChungTu_BangDuLieu.DauCachO},
                                                                    StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] {TCDN_ChungTu_BangDuLieu.DauCachO},
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
                        Boolean bLaTong = false;
                        if (okCoThayDoi)
                        {
                            Bang bang = new Bang("TCDN_ChungTuChiTiet");
                            if (iID_MaChungTuChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                //Them cac tham so tu bang chung tu
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iQuy", data["iQuy"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", data["iID_MaDoanhNghiep"]);
                                bang.CmdParams.Parameters.AddWithValue("@dNgayChungTu", data["dNgayChungTu"]);
                                bang.CmdParams.Parameters.AddWithValue("@sKyHieu", sKyHieu);
                                String SQL = String.Format(@"SELECT sTen FROM TCDN_ChiTieu WHERE iTrangThai=1 AND sKyHieu={0}", sKyHieu); ;
                                sTen = Connection.GetValueString(SQL, "");
                                bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);

                                //Check blahangtong
                                SQL = String.Format(@"SELECT bLaTong FROM TCDN_ChiTieu WHERE iTrangThai=1 AND sKyHieu={0}", sKyHieu);
                                bLaTong =Convert.ToBoolean(Connection.GetValue(SQL, false));

                                
                            }
                            else
                            {
                                bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                                bang.DuLieuMoi = false;        
                            }
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
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
                            if (bLaTong==false)
                                bang.Save();
                        }
                    }
                }
            }

            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "TCDN_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "TCDN_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu,iLoai=iLoai });
        }
      
    }
}
