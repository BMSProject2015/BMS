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

namespace VIETTEL.Controllers.CongSan
{
    public class KTCS_ChungTuChiTietController : Controller
    {
        //
        // GET: /KTCS_ChungTuChiTiet/
        public string sViewPath = "~/Views/CongSan/ChungTu/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "KTCS_ChungTuChiTiet_Index.aspx");
        }
        [Authorize]
        public ActionResult ChungTu_Frame()
        {
            return View(sViewPath + "KeToanCongSan_ChungTu_Frame.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTiet_Frame()
        {
            return View(sViewPath + "KeToanCongSan_ChungTuChiTiet_Frame.aspx");
        }

        [Authorize]
        public ActionResult HachToan_Frame(String iID_MaChungTuChiTiet)
        {
            return View(sViewPath + "KeToanCongSan_HachToan_Dialog.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            DataTable dtChungTu = KTCS_ChungTuModels.GetChungTu(iID_MaChungTu);
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
                KTCS_ChungTuModels.InsertRecord(iID_MaChungTu, Request.Form, MaND, Request.UserHostAddress);
                dtChungTu = KTCS_ChungTuModels.GetChungTu(iID_MaChungTu);
            }
            if (dtChungTu.Rows.Count == 1)
            {
                String TenBangChiTiet = "KTCS_ChungTuChiTiet";

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
                            //Xóa trong bảng hạch toán
                            KTCS_HachToanModels.XoaHachToan(iID_MaChungTuChiTiet);   
                        }
                    }
                    else
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                        Boolean okCoThayDoi = false;
                        String iID_MaTaiSan = "", rNamKhauHao = "", iID_MaKyHieuHachToan = "", rSoTien = "";
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
                            Boolean DuLieuDaCo = false;
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
                                DuLieuDaCo = true;
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                                bang.DuLieuMoi = false;
                                
                            }
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            //Them tham so

                           
                            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
                            
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {
                                if (arrMaCot[j] == "iID_MaTaiSan")
                                {
                                    iID_MaTaiSan = arrGiaTri[j];
                                }
                                if (arrThayDoi[j] == "1")
                                {                                    
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("d"))
                                    {
                                        //Nhap kieu date
                                        bang.CmdParams.Parameters.AddWithValue(Truong, CommonFunction.ChuyenXauSangNgay(arrGiaTri[j]));
                                    }
                                    else if (arrMaCot[j].StartsWith("b"))
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


                            iID_MaChungTuChiTiet = Convert.ToString(bang.Save());
                            DataTable dtChiTiet = KTCS_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(iID_MaChungTuChiTiet);
                            int iThang = 0;
                            if (rSoTien == "")
                            {
                                rSoTien = Convert.ToString(dtChiTiet.Rows[0]["rSoTien"]);
                            }
                            if (iID_MaKyHieuHachToan == "")
                            {
                                iID_MaKyHieuHachToan = Convert.ToString(dtChiTiet.Rows[0]["iID_MaKyHieuHachToan"]);
                            }
                            if (rNamKhauHao == "")
                            {
                                rNamKhauHao = Convert.ToString(dtChiTiet.Rows[0]["rNamKhauHao"]);
                            }
                            if (iID_MaTaiSan == "")
                            {
                                iID_MaTaiSan = Convert.ToString(dtChiTiet.Rows[0]["iID_MaTaiSan"]);
                            }
                            iThang = Convert.ToInt32(dtChiTiet.Rows[0]["iThang"]);
                            //Nếu có hao mòn thay đổi thi lưu sang bảng hao mòn năm của bảng tài sản năm trước
                            //if (rNamKhauHao != "")
                            //{
                            //    if (iID_MaTaiSan == "")
                            //    {
                            //        iID_MaTaiSan = KTCS_TaiSanModels.LayMaTaiSan(iID_MaChungTuChiTiet);
                            //    }
                            //    Update_SoNamKhauHaoThayDoi(rNamKhauHao, iID_MaTaiSan, iNamLamViec - 1);
                            //}

                            //Thêm mới hạch toán trước khi thêm thì xóa những hạch toán trước
                           // KTCS_HachToanModels.XoaHachToan(iID_MaChungTuChiTiet);
                           // String vGiaTriTinhToan = KTCS_HachToanModels.TinhHachToanTaiSan(iID_MaTaiSan, iNamLamViec, iThang, MaND, Request.UserHostAddress);
                            //String[] arrHT = vGiaTriTinhToan.Split('#');
                            //NGIA#TANG#GIAM#HMON#LKHM#CONLAI#CONLAITKH#NAMKHCL
                            //KTCS_HachToanModels.ThemMoiHachToan(iID_MaChungTuChiTiet, iID_MaKyHieuHachToan, arrHT[0], arrHT[1], arrHT[2], arrHT[3], arrHT[4], arrHT[5], arrHT[6], arrHT[7], User.Identity.Name, Request.UserHostAddress);
                        }
                    }
                }
                SqlCommand cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Double rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                cmd.Dispose();

                Bang bangChungTu = new Bang("KTCS_ChungTu");
                bangChungTu.DuLieuMoi = false;
                bangChungTu.GiaTriKhoa = iID_MaChungTu;
                bangChungTu.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);
                bangChungTu.MaNguoiDungSua = User.Identity.Name;
                bangChungTu.IPSua = Request.UserHostAddress;
                bangChungTu.Save();

            }
            else
            {
                ViewData["KhongThemDuoc"] = "1";
            }
            dtChungTu.Dispose();
            ViewData["Saved"] = "1";
            return View(sViewPath + "KeToanCongSan_ChungTuChiTiet_Frame.aspx");
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
        public JsonResult get_ThongTinChungTuMoi()
        {
            int iSoChungTu_GoiY = KTCS_ChungTuModels.GetMaxChungTu_GoiY();
            string sSoChungTu = "";

            Boolean ok = false;
            int d = 0;
            do
            {
                iSoChungTu_GoiY++;
                ok = KTCS_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY));
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
            return View(sViewPath + "KeToanCongSan_ChungTuChiTietTuChoi.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTietTuChoi_Frame()
        {
            return View(sViewPath + "KeToanCongSan_ChungTuChiTietTuChoi_Frame.aspx");
        }

        [Authorize]
        public JsonResult get_SoNamKhauHao(String iID_MaTaiSan)
        {
            String sSoNamKhauHao = "";
            DataTable vR;
            //vR = KTCS_TaiSanModels.LayThongTinNhomTaiSan(iID_MaTaiSan);
            //if (vR.Rows.Count > 0)
            //{
            //    sSoNamKhauHao = Convert.ToString(vR.Rows[0]["rSoNamKhauHao"]);
            //}
            vR = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
            DataTable dtChiTiet = KTCS_TaiSanModels.Get_dtTaiSanChiTiet(iID_MaTaiSan);
            String iID_MaDonVi = "";
            String dNgayDuaVaoKhauHao ="";
            String sTenDonVi = "";
            if (vR != null && vR.Rows.Count > 0 && dtChiTiet != null && dtChiTiet.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(vR.Rows[0]["iID_MaDonVi"]);
                sSoNamKhauHao=Convert.ToString(vR.Rows[0]["rSoNamKhauHao"]).ToString();
                if (dtChiTiet.Rows[0]["dNgayDuaVaoKhauHao"] != DBNull.Value && Convert.ToString(dtChiTiet.Rows[0]["dNgayDuaVaoKhauHao"]) != "")
                {
                    dNgayDuaVaoKhauHao = Convert.ToString(dtChiTiet.Rows[0]["dNgayDuaVaoKhauHao1"]);
                }

                 sTenDonVi = Convert.ToString(vR.Rows[0]["sTenDonVi"]);
            }
                Object item = new
                {
                    sSoNamKhauHao = sSoNamKhauHao,
                    iID_MaDonVi = iID_MaDonVi,
                    sTenDonVi = sTenDonVi,
                    dNgayDuaVaoKhauHao = dNgayDuaVaoKhauHao
                };
            
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
      


        public static void Update_SoNamKhauHaoThayDoi(String iSoNamKhauHao,String iID_MaTaiSan,int iNamLamViec)
        {
            String SQL = "UPDATE KTCS_KhauHaoHangNam SET iSoNamKhauHaoThayDoi=@iSoNamKhauHaoThayDoi WHERE iID_MaTaiSan=@iID_MaTaiSan AND iNamLamViec=@iNamLamViec";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iSoNamKhauHaoThayDoi", iSoNamKhauHao);
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
    }
}
