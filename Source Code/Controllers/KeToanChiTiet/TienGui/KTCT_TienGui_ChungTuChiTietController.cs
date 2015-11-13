using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace VIETTEL.Controllers.KeToanChiTiet.TienGui
{
    public class KTCT_TienGui_ChungTuChiTietController : Controller
    {
        //
        // GET: /KTCT_TienGui_ChungTuChiTiet/
        public string sViewPath = "~/Views/KeToanChiTiet/TienGui/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "KTCT_TienGui_ChungTuChiTiet_Index.aspx");
        }
        [Authorize]
        public ActionResult ChungTu_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_TienGui_ChungTu_Frame.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTiet_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_TienGui_ChungTuChiTiet_Frame.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu)
        {
            iID_MaChungTu = Request.Form["iID_MaChungTu"];
            String MaND = User.Identity.Name;
            DataTable dtChungTu = KTCT_TienGui_ChungTuModels.GetChungTu(iID_MaChungTu);
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
                KTCT_TienGui_ChungTuModels.InsertRecord(iID_MaChungTu, Request.Form, MaND, Request.UserHostAddress);
                dtChungTu = KTCT_TienGui_ChungTuModels.GetChungTu(iID_MaChungTu);
            }
            if (dtChungTu.Rows.Count == 1)
            {
                String TenBangChiTiet = "KTTG_ChungTuChiTiet";

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
                            String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                            if (!String.IsNullOrEmpty(arrGiaTri[5]))
                            {
                                try
                                {
                                    SqlCommand cmdNew = new SqlCommand("Update KTTG_ChungTuCapThu_Duyet set rSoTien=rSoTien + @rSoTien where iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet");
                                    cmdNew.Parameters.AddWithValue("@rSoTien", arrGiaTri[10]);
                                    cmdNew.Parameters.AddWithValue("@iID_MaChungTu_Duyet", arrGiaTri[28]);
                                    Connection.UpdateDatabase(cmdNew);
                                    cmdNew.Dispose();
                                }
                                catch (Exception)
                                {
                                    
                                    throw;
                                }
                               
                            }
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
                                        //if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]) && arrMaCot[j] != "rTongSo") // khong cho cap nhat lai truong tong so
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
                            if (KTCT_TienMat_ChungTuChiTietModels.KiemTraCoDuocCapNhapBang(bang))
                            {
                                bang.Save();
                                //cap nhat lai bang duyet thu chi
                                if  (!String.IsNullOrEmpty( arrGiaTri[5]))
                                {
                                    try
                                    {
                                        SqlCommand cmdNew = new SqlCommand("Update KTTG_ChungTuCapThu_Duyet set rSoTien=rSoTien - @rSoTien where iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet");
                                        cmdNew.Parameters.AddWithValue("@rSoTien", arrGiaTri[10]);
                                        cmdNew.Parameters.AddWithValue("@iID_MaChungTu_Duyet", arrGiaTri[28]);
                                        Connection.UpdateDatabase(cmdNew);
                                        cmdNew.Dispose();
                                    }
                                    catch (Exception)
                                    {
                                        
                                        throw;
                                    }
                                  
                                }
                              
                            }
                        }
                    }
                }
                SqlCommand cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTTG_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Double rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                cmd.Dispose();
                if (rTongSo == 0)
                {
                    cmd = new SqlCommand("DELETE KTTG_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }
                else
                {

                    Bang bangChungTu = new Bang("KTTG_ChungTu");
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
            return View(sViewPath + "KeToanChiTiet_TienGui_ChungTuChiTiet_Frame.aspx");
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
            int iSoChungTu_GoiY = KTCT_TienGui_ChungTuModels.GetMaxChungTu_GoiY(iNamLamViec);
            string sSoChungTu = "";

            Boolean ok = false;
            int d = 0;
            do
            {
                iSoChungTu_GoiY++;
                ok = KTCT_TienGui_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY), iNamLamViec);
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
            return View(sViewPath + "KeToanChiTiet_TienGui_ChungTuChiTietTuChoi.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTietTuChoi_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_TienGui_ChungTuChiTietTuChoi_Frame.aspx");
        }

        [Authorize]
        public JsonResult get_ChungTuCapThu(String iID_MaChungTu_Duyet)
        {
            String rTongCap = "", rTongThu = "",sRutDuToan="", rSoTien = "", iID_MaDonVi_Co = "", sTenDonVi_Co = "", iID_MaDonVi_No = "", sTenDonVi_No = "", iID_MaTaiKhoan_No = "", sTenTaiKhoan_No = "", iID_MaTaiKhoan_Co = "", sTenTaiKhoan_Co="",sNoiDung="";
            DataTable vR;
            vR = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Row_ChungTu_Duyet(iID_MaChungTu_Duyet);
            Double rRutDuToan = 0;
            rRutDuToan = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_SoTienRutDuToan(iID_MaChungTu_Duyet);
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
                sRutDuToan = Convert.ToString(rRutDuToan);
                rSoTien = Convert.ToString(Convert.ToDouble(vR.Rows[0]["rTongCap"]) - Convert.ToDouble(vR.Rows[0]["rTongThu"])-rRutDuToan);
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
                sRutDuToan = sRutDuToan,
                rSoTien = rSoTien,
                sNoiDung = sNoiDung
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult get_ThongTinDonViNhanUyNhiemChi(String iID_MaDonViNhanUNC)
        {
            String sSoTaiKhoan = "", sDiaChi = "";
            DataTable vR;
            vR = DonViModels.Get_dtDonVi(iID_MaDonViNhanUNC);
            if (vR.Rows.Count > 0)
            {
                sSoTaiKhoan = Convert.ToString(vR.Rows[0]["sSoTaiKhoan"]);
                sDiaChi = Convert.ToString(vR.Rows[0]["sDiaChi"]);
            }

            Object item = new
            {
                sSoTaiKhoan = sSoTaiKhoan,
                sDiaChi = sDiaChi
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getMaxSoChungTuChiTiet()
        {
            String sql = "";
            sql = "select Max(sSoChungTuChiTiet) AS sSoChungTuChiTiet from KTTG_ChungTuChiTiet where iTrangThai=1";
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
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
    }
}
