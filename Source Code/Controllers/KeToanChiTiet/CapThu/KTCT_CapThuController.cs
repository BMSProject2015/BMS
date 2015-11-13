using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.KeToanChiTiet.CapThu
{
    public class KTCT_CapThuController : Controller
    {
        //
        // GET: /KTCT_CapThu/
        public string sViewPath = "~/Views/KeToanChiTiet/CapThu/";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "CapThu_Index.aspx");
        }
        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "CapThu_Edit.aspx");
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaChungTu_Duyet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTG_ChungTuCapThu_Duyet", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            CapThuModels.Delete_CapThu_Duyet(iID_MaChungTu_Duyet);
            return RedirectToAction("Index", "KTCT_CapThu");
        }
        /// <summary>
        /// DetailSubmit
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu_Duyet)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThangLamViec = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            dtCauHinh.Dispose();

            String iID_MaPhongBan = "";

            //Luu lai truong bRutDuToan = 1
            String TenBangChiTiet = "KTTG_ChungTuChiTietCapThu";

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
                                    bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                }
                            }
                        }
                        bang.Save();
                    }
                }
            }

            //Kiem tra xem có chung tu chi tiet nao la rut du toan
            Boolean iCheck = CapThuModels.CheckChungTuChiTiet_ChuyenRutDuToan(iID_MaChungTu_Duyet);
            if (iCheck == true)
            {
                int iSoChungTu_GoiY = KTCT_KhoBac_ChungTuModels.GetMaxChungTu_GoiY(NamLamViec);
                string sSoChungTu = "";

                Boolean ok = false;
                int d = 0;
                do
                {
                    iSoChungTu_GoiY++;
                    ok = KTCT_KhoBac_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY), NamLamViec);
                    d++;
                } while (ok == false && d < 10);
                if (ok)
                {
                    sSoChungTu = Convert.ToString(iSoChungTu_GoiY);
                }
                //Lay bang cap thu da co truong bRutDuToan = 1
                DataTable dtCapThuChiTiet = CapThuModels.Get_RowCapThuRutDuToan(iID_MaChungTu_Duyet);

                DataRow RCTCT_D;
                DataTable dtCapThu_Duyet = CapThuModels.Get_RowCapThu_Duyet(iID_MaChungTu_Duyet);
                DataRow RCT_D = dtCapThu_Duyet.Rows[0];
                //Tao chung tu ghi so & chung tu chi tiet rut du toan
                double rTongSoTien = 0;
                for (int i = 0; i < dtCapThuChiTiet.Rows.Count; i++)
                {
                    RCTCT_D = dtCapThuChiTiet.Rows[i];
                    if (!String.IsNullOrEmpty(RCTCT_D["rSoTien"].ToString()) && RCTCT_D["rSoTien"].ToString()!="0")
                    {
                        rTongSoTien += double.Parse(RCTCT_D["rSoTien"].ToString());
                    }
                    
                }
                String iID_MaChungTuKhoBac = "";
                Bang bangchungtu = new Bang("KTKB_ChungTu");
                bangchungtu.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iLoai", 1);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(KeToanTongHopModels.iID_MaPhanHe));
                bangchungtu.CmdParams.Parameters.AddWithValue("@sSoChungTu", sSoChungTu);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iSoChungTu_GoiY", iSoChungTu_GoiY + 1);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iNgay", DateTime.Now.Day);
                bangchungtu.CmdParams.Parameters.AddWithValue("@iThang", iThangLamViec);
                bangchungtu.CmdParams.Parameters.AddWithValue("@sNoiDung", RCT_D["sNoiDung"]);
                bangchungtu.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSoTien);
                //bangchungtu.CmdParams.Parameters.AddWithValue("@sNoiDung", "Chuyển cấp thu sang!");
                bangchungtu.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
                bangchungtu.MaNguoiDungSua = User.Identity.Name;
                bangchungtu.IPSua = Request.UserHostAddress;
                iID_MaChungTuKhoBac = Convert.ToString(bangchungtu.Save());

               
                for (int i = 0; i < dtCapThuChiTiet.Rows.Count; i++)
                {
                    RCTCT_D = dtCapThuChiTiet.Rows[i];

                    String strXauNoiMaMucLuc = Convert.ToString(RCTCT_D["sLNS"] + "-" + RCTCT_D["sL"] + "-" + RCTCT_D["sK"] + "-" + RCTCT_D["sM"] + "-" + RCTCT_D["sTM"]);
                    DataTable dtMucLucNganSach = MucLucNganSachModels.dt_ChiTietMucLucNganSach_XauNoiMa(strXauNoiMaMucLuc);
                    DataTable dtMucLucNganSachKeCan = MucLucNganSachModels.dt_ChiTietMucLucNganSach_XauNoiMa_KeCan(strXauNoiMaMucLuc);

                    //Them chung tu chi tiet cho bang KTKB_ChungTuChiTiet
                    Bang bangchungtuchitiet = new Bang("KTKB_ChungTuChiTiet");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTuKhoBac);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iNgayCT", DateTime.Now.Day);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iThangCT", iThangLamViec);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sSoChungTuGhiSo", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sSoChungTuCapThu", RCT_D["sSoChungTu"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iLoai", 1);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iNgay", DateTime.Now.Day);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iThang", iThangLamViec);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sCapNganSach", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sLoaiST", "S");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sThuChi", "1");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Nhan", RCT_D["iID_MaDonVi_No"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTenDonVi_Nhan", RCT_D["sTenDonVi_No"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Tra", RCT_D["iID_MaDonVi_Co"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTenDonVi_Tra", RCT_D["sTenDonVi_Co"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaNhanVien_Nhan", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTenNhanVien_Nhan", "");

                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", RCT_D["iID_MaTaiKhoan_No"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", RCT_D["sTenTaiKhoan_No"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", RCT_D["iID_MaTaiKhoan_Co"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", RCT_D["sTenTaiKhoan_Co"]);

                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@rTongCap", RCT_D["rTongCap"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@rTongThu", RCT_D["rTongThu"]);

                    if (dtMucLucNganSach.Rows.Count > 0)
                    {
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLucNganSach.Rows[0]["iID_MaMucLucNganSach"]);
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@bLaHangCha", dtMucLucNganSach.Rows[0]["bLaHangCha"]);
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sXauNoiMa", dtMucLucNganSach.Rows[0]["sXauNoiMa"]);
                    }
                    else
                    {
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLucNganSachKeCan.Rows[0]["iID_MaMucLucNganSach"]);
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@bLaHangCha", dtMucLucNganSachKeCan.Rows[0]["bLaHangCha"]);
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sXauNoiMa", dtMucLucNganSachKeCan.Rows[0]["sXauNoiMa"]);
                    }
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sC", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sLNS", RCTCT_D["sLNS"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sL", RCTCT_D["sL"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sK", RCTCT_D["sK"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sM", RCTCT_D["sM"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTM", RCTCT_D["sTM"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTTM", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sNG", "");
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sTNG", "");
                    if (dtMucLucNganSach.Rows.Count > 0)
                    {
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sMoTa", dtMucLucNganSach.Rows[0]["sMoTa"]);
                    }
                    else
                    {
                        bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sMoTa", dtMucLucNganSachKeCan.Rows[0]["sMoTa"]);
                    }
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@rDTRut", RCTCT_D["rSoTien"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sNoiDung", RCT_D["sNoiDung"]);
                    bangchungtuchitiet.CmdParams.Parameters.AddWithValue("@sGhiChu", "");
                    bangchungtuchitiet.MaNguoiDungSua = User.Identity.Name;
                    bangchungtuchitiet.IPSua = Request.UserHostAddress;
                    bangchungtuchitiet.Save();

                    dtMucLucNganSach.Dispose();
                    dtMucLucNganSachKeCan.Dispose();
                }
                dtCapThu_Duyet.Dispose();
                dtCapThuChiTiet.Dispose();

                CapThuModels.CapNhapTruongTongCapTongThu_Duyet(iID_MaChungTu_Duyet);
            }

            return RedirectToAction("Index", "KeToanChiTietKhoBac", new { iLoai = 1 });
        }
    }
}
