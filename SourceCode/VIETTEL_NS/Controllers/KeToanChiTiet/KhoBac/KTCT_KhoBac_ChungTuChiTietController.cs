using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace VIETTEL.Controllers.KeToanChiTiet.KhoBac
{
    public class KTCT_KhoBac_ChungTuChiTietController : Controller
    {
        //
        // GET: /KTCT_KhoBac_ChungTuChiTiet/
        public string sViewPath = "~/Views/KeToanChiTiet/KhoBac/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "KTCT_KhoBac_ChungTuChiTiet_Index.aspx");
        }
        [Authorize]
        public ActionResult ChungTu_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTu_Frame.aspx");
        }
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTuChiTiet_Frame.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(int iLoai)
        {
            String iID_MaChungTu = Request.Form["iID_MaChungTu"];
            String MaND = User.Identity.Name;
            DataTable dtChungTu = KTCT_KhoBac_ChungTuModels.GetChungTu(iID_MaChungTu);
            if (dtChungTu.Rows.Count == 0)
            {
                dtChungTu.Dispose();

                if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) ||
                    LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
                {
                    //Trợ lý phòng ban và trợ lý tổng hợp được quyền thêm chứng từ
                }
                else
                {
                    //Không có quyền thêm chứng từ
                    return RedirectToAction("Index", "PermitionMessage");
                }
                KTCT_KhoBac_ChungTuModels.InsertRecord(iID_MaChungTu, iLoai, Request.Form, MaND, Request.UserHostAddress);
                dtChungTu = KTCT_KhoBac_ChungTuModels.GetChungTu(iID_MaChungTu);
            }
            if (dtChungTu.Rows.Count == 1)
            {
                iLoai = Convert.ToInt32(dtChungTu.Rows[0]["iLoai"]);
                String TenBangChiTiet = "KTKB_ChungTuChiTiet";

                string idXauMaCacHang = Request.Form["idXauMaCacHang"];
                string idXauMaCacCot = Request.Form["idXauMaCacCot"];
                string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
                string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
                string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
                String[] arrMaHang = idXauMaCacHang.Split(',');
                String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
                String[] arrMaCot = idXauMaCacCot.Split(',');
                String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

                String iID_MaChungTuChiTiet;
                //Luu cac hang sua
                String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
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
                            Bang bang = new Bang(TenBangChiTiet);
                            iID_MaChungTuChiTiet = arrMaHang[i];
                            if (iID_MaChungTuChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iNgayCT", dtChungTu.Rows[0]["iNgay"]);
                                bang.CmdParams.Parameters.AddWithValue("@iThangCT", dtChungTu.Rows[0]["iThang"]);
                                bang.CmdParams.Parameters.AddWithValue("@sSoChungTuGhiSo", dtChungTu.Rows[0]["sSoChungTu"]);
                                bang.CmdParams.Parameters.AddWithValue("@iLoai", iLoai);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaChungTuChiTiet;
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
                                        if (arrMaCot[j].StartsWith("iID_MaMucLucNganSach") == true)
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, "00000000-0000-0000-0000-000000000000");
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                        }
                                    }
                                }
                            }
                            if (KTCT_KhoBac_ChungTuChiTietModels.KiemTraCoDuocCapNhapBang(bang, iLoai))
                            {
                                bang.Save();
                            }
                        }
                    }
                }                
                SqlCommand cmd;
                Double rTongSo = 0;
                switch (iLoai)
                {
                    case 1:
                    case 5:
                        //Rút dự toán
                        cmd = new SqlCommand("SELECT SUM(rDTRut) FROM KTKB_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                        cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();
                        break;
                    case 2:
                        //Nhập số duyệt tạm ứng
                        cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTKB_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                        cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();
                        break;
                    case 3:
                        //Khôi phục dự toán
                        cmd = new SqlCommand("SELECT SUM(rDTKhoiPhuc) FROM KTKB_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                        cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();
                        break;
                    case 4:
                        //Hủy dự toán
                        cmd = new SqlCommand("SELECT SUM(rDTHuy) FROM KTKB_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                        cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();
                        break;
                }

                if (rTongSo == 0)
                {
                    cmd = new SqlCommand("DELETE KTKB_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }
                else
                {
                    Bang bangChungTu = new Bang("KTKB_ChungTu");
                    bangChungTu.DuLieuMoi = false;
                    bangChungTu.GiaTriKhoa = iID_MaChungTu;
                    bangChungTu.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);
                    bangChungTu.MaNguoiDungSua = User.Identity.Name;
                    bangChungTu.IPSua = Request.UserHostAddress;
                    bangChungTu.Save();
                }
            }
            else
            {
                ViewData["KhongThemDuoc"] = "1";
            }
            dtChungTu.Dispose();
            ViewData["Saved"] = "1";
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTuChiTiet_Frame.aspx");
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
        public JsonResult get_ThongTinChungTuMoi(String iNamLamViec)
        {
            int iSoChungTu_GoiY = KTCT_KhoBac_ChungTuModels.GetMaxChungTu_GoiY(iNamLamViec);
            string sSoChungTu = "";

            Boolean ok = false;
            int d = 0;
            do
            {
                iSoChungTu_GoiY++;
                ok = KTCT_KhoBac_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY), iNamLamViec);
                d++;
            } while (ok == false && d < 10);
            if (ok)
            {
                sSoChungTu = Convert.ToString(iSoChungTu_GoiY);
            }
            Object item = new
            {
                iID_MaChungTu = Globals.getNewGuid(),
                iNgay = DateTime.Now.Day,
                sSoChungTu = sSoChungTu
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ChungTuChiTietTuChoi()
        {
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTuChiTietTuChoi.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTietTuChoi_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTuChiTietTuChoi_Frame.aspx");
        }

        [Authorize]
        public JsonResult get_ChungTuCapThu(String iID_MaChungTu_Duyet)
        {
            String rTongCap = "", rTongThu = "", rSoTien = "", iID_MaDonVi_Co = "", sTenDonVi_Co = "", iID_MaDonVi_No = "", sTenDonVi_No = "", iID_MaTaiKhoan_No = "", sTenTaiKhoan_No = "", iID_MaTaiKhoan_Co = "", sTenTaiKhoan_Co = "", sNoiDung = "";
            DataTable vR;
            vR = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Row_ChungTu_Duyet(iID_MaChungTu_Duyet);
            Double SoTienUyNhiemChi = Get_SoTienUyNhiemChi(iID_MaChungTu_Duyet);
            if (vR.Rows.Count > 0)
            {
                iID_MaDonVi_Co = Convert.ToString(vR.Rows[0]["iID_MaDonVi_Co"]);
                sTenDonVi_Co = Convert.ToString(vR.Rows[0]["sTenDonVi_Co"]);
                iID_MaDonVi_No = Convert.ToString(vR.Rows[0]["iID_MaDonVi_No"]);
                sTenDonVi_No = Convert.ToString(vR.Rows[0]["sTenDonVi_No"]);
                iID_MaTaiKhoan_No = Convert.ToString(vR.Rows[0]["iID_MaTaiKhoan_No"]);
                sTenTaiKhoan_No = Convert.ToString(vR.Rows[0]["sTenTaiKhoan_No"]);
                iID_MaTaiKhoan_Co = Convert.ToString(vR.Rows[0]["iID_MaTaiKhoan_Co"]);
                sTenTaiKhoan_Co = Convert.ToString(vR.Rows[0]["sTenTaiKhoan_Co"]);
                rTongCap = Convert.ToString(vR.Rows[0]["rTongCap"]);
                rTongThu = Convert.ToString(vR.Rows[0]["rTongThu"]);
                Double SoTien = Convert.ToDouble(vR.Rows[0]["rSoTien"].ToString()) - SoTienUyNhiemChi;
                rSoTien = Convert.ToString(SoTien);
                sNoiDung = Convert.ToString(vR.Rows[0]["sNoiDung"]);
            }

            Object item = new
            {
                iID_MaDonVi_Co = iID_MaDonVi_Co,
                sTenDonVi_Co = sTenDonVi_Co,
                iID_MaDonVi_No = iID_MaDonVi_No,
                sTenDonVi_No = sTenDonVi_No,
                iID_MaTaiKhoan_No = iID_MaTaiKhoan_No,
                sTenTaiKhoan_No = sTenTaiKhoan_No,
                iID_MaTaiKhoan_Co = iID_MaTaiKhoan_Co,
                sTenTaiKhoan_Co = sTenTaiKhoan_Co,
                rTongCap = rTongCap,
                rTongThu = rTongThu,
                rSoTien = rSoTien,
                sNoiDung = sNoiDung
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_CapNS(String iID_MaDonVi)
        {
            String iCapNS = "";
            iCapNS = Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "iCapNS"));

            Object item = new
            {
                iCapNS = iCapNS,
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
        public static Double Get_SoTienUyNhiemChi(String iID_MaChungTu_Duyet)
        {
            Double vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT SUM(rSoTien) FROM KTTG_ChungTuChiTiet WHERE iTrangThai = 1 AND iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            vR = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        [Authorize]
        public JsonResult getMaxSoChungTuChiTiet(int iLoai)
        {
            String sql = "";
            sql = "select Max(sSoChungTuChiTiet) AS sSoChungTuChiTiet from KTKB_ChungTuChiTiet where iTrangThai=1 AND iLoai=@iLoai";
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            vR = Connection.GetDataTable(cmd);
            string sSoChungTuChiTiet = "";
            if (vR.Rows.Count > 0)
            {
                sSoChungTuChiTiet = Convert.ToString(vR.Rows[0]["sSoChungTuChiTiet"]);

            }

            Object item = new
            {
                sSoChungTuChiTiet = sSoChungTuChiTiet,

            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult get_ChiTietNgoaiTe(String iID_MaNgoaiTe)
        {
            Double rTyGia = 0;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT rTyGia FROM KTKB_NgoaiTe WHERE iTrangThai = 1 AND iID_MaNgoaiTe=@iID_MaNgoaiTe";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iID_MaNgoaiTe);
            rTyGia = Convert.ToDouble(Connection.GetValue(cmd, 0));
            
            Object item = new
            {
                rTyGia = rTyGia
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}
