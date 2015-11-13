using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Data.SqlClient;
using System.Data;

namespace VIETTEL.Controllers.PhanBo
{
    public class PhanBo_PhanBoNganhController : Controller
    {
        //
        // GET: /PhanBo_PhanBoNganh/
        public string sViewPath = "~/Views/PhanBo/PhanBoNganh/";

        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "PhanBoNganh_Index.aspx");
        }

        [Authorize]
        public ActionResult PhanBoNganhChiTiet_Frame(String iID_MaChiTieu)
        {
            return View(sViewPath + "PhanBoNganh_Index_DanhSach_Frame.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            DataTable dtChoPhanBo = PhanBo_PhanBoChiTietModels.Get_dtDonViChoPhanBo(iID_MaChiTieu);
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            List<String> arrMaPhanBo = new List<string>();
            String iID_MaDonVi, iID_MaPhanBo = "";
            int CSCotDonVi = -1;
            for (int j = 0; j < arrMaCot.Length - 1; j++)
            {
                if (arrMaCot[j] == "iID_MaDonVi")
                {
                    CSCotDonVi = j;
                    break;
                }
            }

            //Xac dinh cac bang PhanBo cua cac don vi da them
            DataTable dtPhanBo = PhanBo_PhanBoModels.Get_DanhSachPhanBo_TheoChiTieu(iID_MaChiTieu, MaND);
            for (int j = 0; j < dtDonVi.Rows.Count; j++)
            {
                arrMaPhanBo.Add("");
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[j]["iID_MaDonVi"]);
                for (int i = 0; i < dtPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDonVi == Convert.ToString(dtPhanBo.Rows[i]["iID_MaDonVi"]))
                    {
                        arrMaPhanBo[j] = Convert.ToString(dtPhanBo.Rows[i]["iID_MaPhanBo"]);
                        break;
                    }
                }
            }
            //Thêm bảng phân bổ của đơn vị Chờ phân bổ
            String iID_MaPhanBo_DonViChoPhanBo = "";
            iID_MaDonVi = PhanBo_Tong_BangDuLieu.iID_MaDonViChoPhanBo;
            for (int i = 0; i < dtPhanBo.Rows.Count; i++)
            {
                if (iID_MaDonVi == Convert.ToString(dtPhanBo.Rows[i]["iID_MaDonVi"]))
                {
                    iID_MaPhanBo_DonViChoPhanBo = Convert.ToString(dtPhanBo.Rows[i]["iID_MaPhanBo"]);
                    break;
                }
            }
            dtPhanBo.Dispose();

            for (int i = 0; i < arrMaHang.Length; i++)
            {

                if (arrMaHang[i].Length > 10)
                {
                    String[] arrMaHangChiTiet = arrMaHang[i].Split('_');
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (arrMaHangChiTiet[2] != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang("PB_PhanBoChiTiet");
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = arrMaHangChiTiet[2];
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.Save();
                        }
                    }
                    else
                    {
                        if (arrMaHangChiTiet.Length > 1)
                        {
                            String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                            iID_MaDonVi = (arrGiaTri[CSCotDonVi] == "") ? arrMaHangChiTiet[1] : arrGiaTri[CSCotDonVi];
                            Boolean okDonViThoaMan = false;
                            int csMaPhanBo = -1;
                            if (iID_MaDonVi == PhanBo_Tong_BangDuLieu.iID_MaDonViChoPhanBo)
                            {
                                iID_MaPhanBo = iID_MaPhanBo_DonViChoPhanBo;
                                okDonViThoaMan = true;
                            }
                            else
                            {
                                for (int j = 0; j < dtDonVi.Rows.Count; j++)
                                {
                                    if (iID_MaDonVi == Convert.ToString(dtDonVi.Rows[j]["iID_MaDonVi"]))
                                    {
                                        iID_MaPhanBo = arrMaPhanBo[j];
                                        csMaPhanBo = j;
                                        okDonViThoaMan = true;
                                        break;
                                    }
                                }
                            }
                            if (okDonViThoaMan)
                            {
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
                                    if (iID_MaPhanBo == "")
                                    {
                                        //Neu chua co trong bang PhanBo thi se them moi
                                        iID_MaPhanBo = PhanBo_PhanBoChiTietModels.ThemPhanBoChoDonVi(iID_MaChiTieu, iID_MaDonVi, MaND, IPSua);
                                        arrMaPhanBo[csMaPhanBo] = iID_MaPhanBo;
                                    }
                                    else
                                    {

                                    }
                                    String iID_MaMucLucNganSach = arrMaHangChiTiet[0];
                                    String SQL = "";
                                    SqlCommand cmd = new SqlCommand();
                                    String strDK = "iID_MaPhanBo=@iID_MaPhanBo AND " +
                                                    "iID_MaDonVi=@iID_MaDonVi AND " +
                                                    "iID_MaMucLucNganSach=@iID_MaMucLucNganSach";

                                    cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
                                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                                    cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);

                                    String strSet = "sIPSua=@sIPSua,sID_MaNguoiDungSua=@sID_MaNguoiDungSua";
                                    cmd.Parameters.AddWithValue("@sIPSua", IPSua);
                                    cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", MaND);
                                    //neu la ma don vi cho phan bo
                                    if (iID_MaDonVi == "99" && dtChoPhanBo.Rows.Count>0)
                                    {
                                        for (int z=0;z<dtChoPhanBo.Rows.Count;z++)
                                        {
                                            String MaMucLucNganSachChoPhanBo = Convert.ToString(dtChoPhanBo.Rows[z]["iID_MaMucLucNganSach"]);
                                            if (MaMucLucNganSachChoPhanBo == arrMaHangChiTiet[0])
                                            {
                                                for (int j = CSCotDonVi + 1; j < arrMaCot.Length - 1; j++)
                                                {
                                                    if (CommonFunction.IsNumeric(arrGiaTri[j]) && arrMaCot[j] != "TenDonVi")
                                                    {
                                                        strSet += String.Format(",{0}=@{0}", arrMaCot[j]);
                                                        cmd.Parameters.AddWithValue("@" + arrMaCot[j], Convert.ToDouble(arrGiaTri[j]) -Convert.ToDouble(dtChoPhanBo.Rows[z][arrMaCot[j]]));
                                                    }
                                                }
                                                break;
                                            }

                                        }
                                       
                                    }
                                    else
                                    {
                                        for (int j = CSCotDonVi + 1; j < arrMaCot.Length - 1; j++)
                                        {
                                            if (CommonFunction.IsNumeric(arrGiaTri[j]) && arrMaCot[j] != "TenDonVi")
                                            {
                                                strSet += String.Format(",{0}=@{0}", arrMaCot[j]);
                                                cmd.Parameters.AddWithValue("@" + arrMaCot[j], Convert.ToDouble(arrGiaTri[j]));
                                            }
                                        }
                                    }
                                    SQL = String.Format("UPDATE PB_PhanBoChiTiet SET {0} WHERE {1}", strSet, strDK);
                                    cmd.CommandText = SQL;
                                    Connection.UpdateDatabase(cmd);
                                    cmd.Dispose();
                                }
                            }

                        }
                    }
                }

            }
            //Sua thong tin bang chi tieu: Muc dich de lan tiep theo khong phai chay ham 'Bang_CapNhapToanBoHangCha'
            //Bang bangChiTieu = new Bang("PB_PhanBo");
            //bangChiTieu.GiaTriKhoa = iID_MaPhanBo;
            //bangChiTieu.DuLieuMoi = false;
            //bangChiTieu.MaNguoiDungSua = User.Identity.Name;
            //bangChiTieu.IPSua = Request.UserHostAddress;
            //bangChiTieu.CmdParams.Parameters.AddWithValue("@bPhaiTinhTongCong", false);
            //bangChiTieu.Save();
            return RedirectToAction("PhanBoNganhChiTiet_Frame", new { iID_MaChiTieu = iID_MaChiTieu });
        }
    }
}
