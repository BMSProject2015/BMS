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

namespace VIETTEL.Controllers.KeToanChiTiet.TienMat
{
    public class KTCT_TienMat_ChungTuController : Controller
    {
        //
        // GET: /KTCT_TienMat_ChungTu/
        public string sViewPath = "~/Views/KeToanChiTiet/TienMat/ChungTu/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "KTCT_TienMat_ChungTu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Index", "KTCT_TienMat_ChungTu", new { SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "KTTM_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaChungTu"] = iID_MaChungTu;
            return View(sViewPath + "KTCT_TienMat_ChungTu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaChungTu)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("KTTM_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            if (iThang == string.Empty || iThang == "" || iThang == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập tháng chứng từ!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["MaChungTu"] = MaChungTu;
                return View(sViewPath + "KTCT_TienMat_ChungTu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(KeToanTongHopModels.iID_MaPhanHe));
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(KeToanTongHopModels.iID_MaPhanHe));
                    bang.CmdParams.Parameters.AddWithValue("@dNgayChungTu", DateTime.Now);
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //KTCT_TienMat_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    KTCT_TienMat_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTu;
                    bang.Save();
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "KTCT_TienMat_ChungTu");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(int iNam, int iThang)
        {
            string idAction = Request.Form["idAction"];
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;

            String TenBangChiTiet = "KTTM_ChungTu";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            string iHuyDuyet = Request.Form["iHuyDuyet"];
            String iID_MaChungTu;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaChungTu = arrMaHang[i];
                if (iHuyDuyet=="0" && arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaChungTu != "")
                    {
                        //Phuonglt yêu cầu cấp trợ lý tổng hợp được xóa tất cả cấp trợ lý phòng ban, tài khoản nào thuộc cấp trợ lý phòng ban 
                        // thì chỉ được xóa chứng từ của tài khoản đó tạo                                                   
                            KTCT_TienMat_ChungTuModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);
                       
                    }
                }
                else
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    Boolean bChon = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            if (arrMaCot[j] == "bChon")
                            {
                                if (arrGiaTri[j] == "1")
                                {
                                    bChon = true;
                                }
                            }
                            else
                            {
                                okCoThayDoi = true;
                            }
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        DataTable dt = KTCT_TienMat_ChungTuModels.GetChungTu(iID_MaChungTu);
                        String MaNguoiTao = Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]);
                        if (LuongCongViecModel.KiemTra_TroLyTongHop(MaND) || (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) && MaNguoiTao==MaND))
                        {
                            SqlCommand cmdChungTuChiTiet = new SqlCommand();
                            String strSet = "";
                            Bang bang = new Bang(TenBangChiTiet);
                            iID_MaChungTu = arrMaHang[i];
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTu;
                            bang.DuLieuMoi = false;
                            bang.MaNguoiDungSua = MaND;
                            bang.IPSua = IPSua;
                            //Them tham so
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {
                                if (arrThayDoi[j] == "1" && arrMaCot[j] != "dNgayChungTu")
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
                                    if (arrMaCot[j] == "iNgay" || arrMaCot[j] == "iNam")
                                    {
                                        if (strSet != "") strSet += ",";
                                        strSet += String.Format("{0}CT=@{0}", arrMaCot[j]);
                                        cmdChungTuChiTiet.Parameters.AddWithValue("@" + arrMaCot[j], bang.CmdParams.Parameters[Truong].Value);
                                    }
                                    else if (arrMaCot[j] == "sSoChungTu")
                                    {
                                        if (strSet != "") strSet += ",";
                                        strSet += String.Format("{0}GhiSo=@{0}", arrMaCot[j]);
                                        cmdChungTuChiTiet.Parameters.AddWithValue("@" + arrMaCot[j], bang.CmdParams.Parameters[Truong].Value);
                                    }
                                }
                            }
                            bang.Save();
                            //Lưu thêm chứng từ chi tiết
                            if (strSet != "")
                            {
                                cmdChungTuChiTiet.CommandText = String.Format("UPDATE KTTM_ChungTuChiTiet SET {0} WHERE iID_MaChungTu=@iID_MaChungTu", strSet);
                                cmdChungTuChiTiet.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                Connection.UpdateDatabase(cmdChungTuChiTiet, MaND, IPSua);
                            }
                        }
                    }
                    //Trình duyệt, từ chối chứng từ
                    if (idAction == "1")
                    {
                        if (bChon)
                        {
                            KTCT_TienMat_ChungTuModels.TuChoiChungTu(iID_MaChungTu, MaND, IPSua);
                        }
                    }
                    else if (idAction == "2")
                    {
                        if (bChon)
                        {
                            KTCT_TienMat_ChungTuModels.TrinhDuyetChungTu(iID_MaChungTu, MaND, IPSua);
                        }
                    }
                }
            }
            //string idAction = Request.Form["idAction"];
            string iID_MaChungTu_Action = Request.Form["id_iID_MaChungTu_Action"];
            string id_iID_MaChungTu_Focus = Request.Form["id_iID_MaChungTu_Focus"];
            String sSoChungTu = Request.Form["id_sSoChungTu"];
            //if (idAction == "1" && iHuyDuyet=="1")
            //{

            //    KTCT_TienMat_ChungTuModels.TuChoiChungTu(iID_MaChungTu_Action, MaND, IPSua);
                
            //}
            if (idAction == "1" && iHuyDuyet == "1")
            {
                String[] arriID_MaChungTu = iID_MaChungTu_Action.Split(',');
                for (int i = 0; i < arriID_MaChungTu.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(arriID_MaChungTu[i]))
                    {
                        KTCT_TienGui_ChungTuModels.TuChoiChungTu(arriID_MaChungTu[i], MaND, IPSua);
                    }
                }
            }
            //if (String.IsNullOrEmpty(iID_MaChungTu_Action) == false)
            //{
            //    if (idAction == "1")
            //    {
            //        return RedirectToAction("TuChoi", "KeToanTongHop_ChungTu", new { iID_MaChungTu = iID_MaChungTu_Action, id_iID_MaChungTu_Focus = id_iID_MaChungTu_Focus });
            //    }
            //    else if (idAction == "2")
            //    {
            //        return RedirectToAction("TrinhDuyet", "KeToanTongHop_ChungTu", new { iID_MaChungTu = iID_MaChungTu_Action, id_iID_MaChungTu_Focus = id_iID_MaChungTu_Focus });
            //    }
            //}
            return RedirectToAction("ChungTu_Frame", "KTCT_TienMat_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu_Action, sSoChungTu = sSoChungTu });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChungTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTTM_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = KTCT_TienMat_ChungTuModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "KTCT_TienMat_ChungTu");
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return RedirectToAction("Index", "KTCT_TienMat_ChungTu");
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            KTCT_TienMat_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = KTCT_TienMat_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            KTCT_TienMat_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            return RedirectToAction("Index", "KTCT_TienMat_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            KTCT_TienGui_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            KTCT_TienMat_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = KTCT_TienMat_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            KTCT_TienMat_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "KTCT_TienMat_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        /// <summary>
        /// Lấy số chứng từ ghi sổ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JsonResult getSoChungTuGhiSo(String iID_MaChungTu)
        {
            string strGiaTri = KTCT_TienMat_ChungTuModels.getSoChungTuGhiSo(iID_MaChungTu);
            JsonResult value = Json(strGiaTri, JsonRequestBehavior.AllowGet);
            return value;
        }
        [Authorize]
        public JsonResult get_CheckSoChungTuGhiSo(String iID_MaChungTu, String sSoChungTu, String iNamLamViec)
        {
            Boolean strGiaTri = KTCT_TienMat_ChungTuModels.KiemTra_sSoChungTu_Trung(iID_MaChungTu, sSoChungTu, iNamLamViec);
            return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }

        //Tuannn
        /// <summary>
        /// trùng trả về giá trị true, ngược lại trả lại giá trị false
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sSoChungTu"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult CheckTrungSoGhiSo(String iID_MaChungTu, String sSoChungTu,String iNamLamViec)
        {
            int TrangThaiDuyet = 0;
            Boolean DuLieuMoi = true;
            Boolean DaDuyet = false;
            if (String.IsNullOrEmpty(iID_MaChungTu) == false)
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KTTM_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1 ");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Int32 vR1 = Convert.ToInt32(Connection.GetValue(cmd, 0));
                cmd.Dispose();
                if (vR1 > 0)
                {
                    DuLieuMoi = false;
                    DataTable dt = KTCT_TienMat_ChungTuModels.GetChungTu(iID_MaChungTu);
                    TrangThaiDuyet = Convert.ToInt16(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
                    if (TrangThaiDuyet == LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop))
                    {
                        DaDuyet = true;
                    }
                }
            }
            Boolean Trung = HamChung.Check_Trung("KTTM_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sSoChungTu,iNamLamViec", sSoChungTu + "," + iNamLamViec, DuLieuMoi);
            if (Trung)
            {
                iID_MaChungTu = KTCT_TienMat_ChungTuModels.LayMaChungTu(sSoChungTu);
                DataTable dt = KTCT_TienMat_ChungTuModels.GetChungTu(iID_MaChungTu);
                TrangThaiDuyet=Convert.ToInt16(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
                if (TrangThaiDuyet == LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop))
                {
                    DaDuyet = true;
                }
            }

            Object item = new
            {
                iID_MaChungTu = iID_MaChungTu,
                Trung = Trung,
                DaDuyet=DaDuyet
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult get_Check_iID_MaChungTu(String iID_MaChungTu)
        {
            Boolean Trung = KTCT_TienMat_ChungTuModels.KiemTra_iID_MaChungTu_Trung(iID_MaChungTu);
            // get_Check_iID_MaChungTu
            Object item = new
            {
                Trung = Trung
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            // return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
    }
}
