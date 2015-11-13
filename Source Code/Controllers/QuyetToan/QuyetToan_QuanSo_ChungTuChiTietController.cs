using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;

namespace VIETTEL.Controllers.QuyetToan
{
    public class QuyetToan_QuanSo_ChungTuChiTietController : Controller
    {
        //
        // GET: /QuyetToan_QuanSo_ChungTuChiTiet/
        public string sViewPath = "~/Views/QuyetToan/QuanSo/ChungTuChiTiet/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "QuyetToan_QuanSo_ChungTuChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu)
        {
            NameValueCollection data = QuyetToan_QuanSo_ChungTuModels.LayThongTin(iID_MaChungTu);
            String TenBangChiTiet = "QTQS_ChungTuChiTiet";
            String MaND = User.Identity.Name;
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            String strTenPhongBan = "", iID_MaPhongBan = "";
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                strTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String iID_MaCapPhatChiTiet, sKyHieu;

            DataTable dtBienChe = QuyetToan_QuanSo_ChungTuChiTietModels.Get_QuanSoBienChe_ThangTruoc(Convert.ToInt32(data["iNamLamViec"]), Convert.ToInt32(data["iThang_Quy"]),
                                        data["iID_MaDonVi"], data["iLoai"]);


            //Luu cac hang sua
            
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    iID_MaCapPhatChiTiet = arrMaHang[i].Split('_')[0];
                    sKyHieu = arrMaHang[i].Split('_')[1];
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaCapPhatChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.Save();
                        }
                    }
                    else
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO },
                                                                    StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(
                            new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
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
                            if (iID_MaCapPhatChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                //Them cac tham so tu bang chung tu
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", strTenPhongBan);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                                                       data["iID_MaTrangThaiDuyet"]);

                                bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", data["iThang_Quy"]);
                                bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", data["bLoaiThang_Quy"]);
                                bang.CmdParams.Parameters.AddWithValue("@sKyHieu", sKyHieu);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", data["iID_MaDonVi"]);
                                DataTable dt = QuyetToan_QuanSo_MucLucModels.DT_MucLucQuanSo(sKyHieu);
                                DataRow dr = dt.Rows[0];
                                NganSach_HamChungModels.ThemThongTinCuaMucLucQuanSoKhongLayTruongTien(dr, bang.CmdParams.Parameters);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.DuLieuMoi = false;
                            }
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
                                    else if (arrMaCot[j].StartsWith("r") ||
                                             (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            double GiaTri = Convert.ToDouble(arrGiaTri[j]);

                                            //hang quan so bien che
                                            if (i == 0)
                                            {
                                                if (dtBienChe.Rows.Count > 0 && j < arrMaCot.Length - 2)
                                                {
                                                    GiaTri = Convert.ToDouble(arrGiaTri[j]) -
                                                             Convert.ToDouble(dtBienChe.Rows[0][j - 1]);
                                                }
                                            }
                                            bang.CmdParams.Parameters.AddWithValue(Truong,GiaTri);
                                                                                   
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

                            //update lai skyhieu='000' quan so bien che
                            if(i==0)
                            {
                                DataTable dtbienChe =
                                    QuyetToan_QuanSo_ChungTuChiTietModels.Get_QuanSoBienChe(
                                        Convert.ToInt32(data["iNamLamViec"]), Convert.ToInt32(data["iThang_Quy"]),
                                        data["iID_MaDonVi"], data["iLoai"]);
                                DataTable dtbienCheThangTruoc =
                                   QuyetToan_QuanSo_ChungTuChiTietModels.Get_QuanSoBienChe(
                                       Convert.ToInt32(data["iNamLamViec"]), Convert.ToInt32(data["iThang_Quy"])-1,
                                       data["iID_MaDonVi"], data["iLoai"]);
                                String SQL = String.Format("UPDATE DTChungTuChi");
                            }
                        }
                    }
                }
            }

            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "QuyetToan_QuanSo_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "QuyetToan_QuanSo_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaChungTu)
        {
            String sLNS = Request.Form["Search_sLNS"];
            String sL = Request.Form["Search_sL"];
            String sK = Request.Form["Search_sK"];
            String sM = Request.Form["Search_sM"];
            String sTM = Request.Form["Search_sTM"];
            String sTTM = Request.Form["Search_sTTM"];
            String sNG = Request.Form["Search_sNG"];
            String sTNG = Request.Form["Search_sTNG"];

            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }

        [Authorize]
        public ActionResult LayThongTinTuLuong(String iID_MaChungTu)
        {
            QuyetToan_QuanSo_ChungTuChiTietModels.Insert_FROM_Luong(iID_MaChungTu, User.Identity.Name, Request.UserHostAddress);
            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu });
        }

    }
}
