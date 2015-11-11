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

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class KeToanTongHop_ChungTuController : Controller
    {
        //
        // GET: /KeToanTongHop_ChungTu/
        public string sViewPath = "~/Views/KeToanTongHop/ChungTu/";
        [Authorize]
        public ActionResult Index_TapSo()
        {
            ViewData["iTapSo"] = "";
            ViewData["iTuSoChungTu"] = "";
            ViewData["iDenSoChungTu"] = "";
            return View(sViewPath + "KeToanTongHop_TapSo_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult HuyDuyet()
        {
            return RedirectToAction("ChungTu_Frame", "KeToanTongHop_ChungTuChiTiet");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(int iNam, int iThang)
        {
            string idAction = Request.Form["idAction"];
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;

            String TenBangChiTiet = "KT_ChungTu";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string iHuyDuyet = Request.Form["iHuyDuyet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTu;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaChungTu = arrMaHang[i];
                if (iHuyDuyet == "0")
                {
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaChungTu != "")
                        {

                            //Phuonglt yêu cầu cấp trợ lý tổng hợp được xóa tất cả cấp trợ lý phòng ban, tài khoản nào thuộc cấp trợ lý phòng ban 
                            // thì chỉ được xóa chứng từ của tài khoản đó tạo 
                            //Dữ liệu đã có
                            KeToanTongHop_ChungTuModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);

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
                            if (LuongCongViecModel.KiemTra_TroLyTongHop(MaND) || LuongCongViecModel.KiemTra_TroLyPhongBan(MaND))
                            {
                                SqlCommand cmdChungTuChiTiet = new SqlCommand();
                                String strSet = "";
                                String strUpDateSCT = "";
                                String strSoChungTuSua = "";
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
                                            if (CommonFunction.IsNumeric(arrGiaTri[j]) && arrMaCot[j] != "rTongSo") // khong cho cap nhat lai truong tong so
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
                                           // if (strUpDateSCT != "") strUpDateSCT += ",";
                                            strUpDateSCT += String.Format("{0}GhiSo=@{0}", arrMaCot[j]);
                                            strSoChungTuSua = Convert.ToString(bang.CmdParams.Parameters[Truong].Value);
                                            cmdChungTuChiTiet.Parameters.AddWithValue("@" + arrMaCot[j], bang.CmdParams.Parameters[Truong].Value);
                                        }
                                    }
                                }
                                int idSoChungTu = KeToanTongHop_ChungTuModels.KiemTra_ChungTuGhiSo_ConTonTai(iNam, iID_MaChungTu, strSoChungTuSua);
                                //bang.Save();

                                if (idSoChungTu > 0)
                                { // truong hop luu vao sgs da ton tai
                                    if (!String.IsNullOrEmpty(strUpDateSCT.Trim()) &&
                                        !String.IsNullOrEmpty(strSoChungTuSua))
                                    {
                                        String mySQL =
                                            String.Format(
                                                @"UPDATE KT_ChungTuChiTiet SET {0}, iNgayCT=(select top 1 iNgay from KT_ChungTu where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTu=@sSoChungTu and iID_MaChungTu !=@iID_MaChungTu),
iThangCT=(select top 1 iThang from KT_ChungTu where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTu=@sSoChungTu and iID_MaChungTu !=@iID_MaChungTu),
iID_MaChungTu=(select top 1 iID_MaChungTu from KT_ChungTu where iTrangThai=1 and iNamLamViec=@iNamLamViec and sSoChungTu=@sSoChungTu and iID_MaChungTu !=@iID_MaChungTu) where iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1;
DELETE KT_ChungTu where iID_MaChungTu=@iID_MaChungTu;",
                                                strUpDateSCT);
                                        cmdChungTuChiTiet.CommandText = mySQL;
                                        cmdChungTuChiTiet.Parameters.AddWithValue("@iNamLamViec", iNam);
                                        cmdChungTuChiTiet.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                        Connection.UpdateDatabase(cmdChungTuChiTiet, MaND, IPSua);
                                    }
                                   // bang.Save();
                                   
                                }
                                else
                                {
                                    bang.Save();
                                    //Lưu thêm chứng từ chi tiết
                                    if (strSet != "")
                                    {
                                        cmdChungTuChiTiet.CommandText = String.Format("UPDATE KT_ChungTuChiTiet SET {0} WHERE iID_MaChungTu=@iID_MaChungTu", strSet);
                                        cmdChungTuChiTiet.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                        Connection.UpdateDatabase(cmdChungTuChiTiet, MaND, IPSua);
                                    }
                                }
                                cmdChungTuChiTiet.Dispose();
                            }
                        }
                        //Trình duyệt, từ chối chứng từ
                        if (idAction == "1")
                        {
                            if (bChon)
                            {
                                KeToanTongHop_ChungTuModels.TuChoiChungTu(iID_MaChungTu, MaND, IPSua);
                            }
                        }
                        else if (idAction == "2")
                        {
                            if (bChon)
                            {
                                KeToanTongHop_ChungTuModels.TrinhDuyetChungTu(iID_MaChungTu, MaND, IPSua);
                            }
                        }
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
                        
                            if (arrMaCot[j] == "bChon")
                            {
                                if (arrGiaTri[j] == "1")
                                {
                                    bChon = true;
                                    break;
                                }
                            }                                                 
                    }
                    //Trình duyệt, từ chối chứng từ
                    if (idAction == "1")
                    {
                        if (bChon)
                        {
                            KeToanTongHop_ChungTuModels.TuChoiChungTu(iID_MaChungTu, MaND, IPSua);
                        }
                    }
                    else if (idAction == "2")
                    {
                        if (bChon)
                        {
                            KeToanTongHop_ChungTuModels.TrinhDuyetChungTu(iID_MaChungTu, MaND, IPSua);
                        }
                    }
                }
            }
            //string idAction = Request.Form["idAction"];
            string iID_MaChungTu_Action = Request.Form["id_iID_MaChungTu_Action"];
            string id_iID_MaChungTu_Focus = Request.Form["id_iID_MaChungTu_Focus"];
            String sSoChungTu = Request.Form["id_sSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form["iID_MaTrangThaiDuyet"];
            if (idAction == "1" && iHuyDuyet == "1")
            {
                String[] arriID_MaChungTu = iID_MaChungTu_Action.Split(',');
                for (int i = 0; i < arriID_MaChungTu.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(arriID_MaChungTu[i]))
                    {
                        KeToanTongHop_ChungTuModels.TuChoiChungTu(arriID_MaChungTu[i], MaND, IPSua);
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
            return RedirectToAction("ChungTu_Frame", "KeToanTongHop_ChungTuChiTiet", new { iID_MaChungTu = id_iID_MaChungTu_Focus, sSoChungTu = sSoChungTu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_TapSo(String ParentID)
        {
            String iTuSoChungTu = Request.Form[ParentID + "_iTuSoChungTu"];
            String iDenSoChungTu = Request.Form[ParentID + "_iDenSoChungTu"];
            String iTapSo = Request.Form[ParentID + "_iTapSo"];
            NameValueCollection arrLoi = new NameValueCollection();

            if (String.IsNullOrEmpty(iTapSo))
            {
                arrLoi.Add("err_iTapSo", "Bạn chưa nhập tập số");
            }

            if (String.IsNullOrEmpty(iTuSoChungTu))
            {
                arrLoi.Add("err_iTuSoChungTu", "Bạn chưa nhập từ số chứng từ");
            }
            if (String.IsNullOrEmpty(iDenSoChungTu))
            {
                arrLoi.Add("err_iDenSoChungTu", "Bạn chưa nhập đến số chứng từ");
            }
            if (!String.IsNullOrEmpty(iTuSoChungTu) && !String.IsNullOrEmpty(iDenSoChungTu) && Int32.Parse(iTuSoChungTu) > Int32.Parse(iDenSoChungTu))
            {
                arrLoi.Add("err_iDenSoChungTu", "Hãy kiểm tra khoảng cách chứng từ từ và đến");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                ViewData["iTapSo"] = iTapSo;
                ViewData["iTuSoChungTu"] = iTuSoChungTu;
                ViewData["iDenSoChungTu"] = iDenSoChungTu;
                return View(sViewPath + "KeToanTongHop_TapSo_Edit.aspx");

            }
            else
            {
                String SQL = "UPDATE KT_ChungTu SET iTapSo=@iTapSo,sID_MaNguoiDungTao=@sID_MaNguoiDungTao WHERE Convert(int,RTrim(LTrim(sSoChungTu)))>=@iTuSoChungTu AND Convert(int,RTrim(LTrim(sSoChungTu))) <=@iDenSoChungTu ";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iTapSo", iTapSo);
                cmd.Parameters.AddWithValue("@iTuSoChungTu", iTuSoChungTu);
                cmd.Parameters.AddWithValue("@iDenSoChungTu", iDenSoChungTu);
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", User.Identity.Name);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                ViewData["iTapSo"] = iTapSo;
                ViewData["iTuSoChungTu"] = iTuSoChungTu;
                ViewData["iDenSoChungTu"] = iDenSoChungTu;
                ///return View(sViewPath + "KeToanTongHop_TapSo_Edit.aspx");
                return RedirectToAction("Index", "KeToanTongHop");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Fast_Submit_Nhan(String ParentID,int iLoai)
        {
            String iID_MaChungTu=KeToanTongHop_ChungTuModels.InsertChungTuNhan(ParentID, Request.Form, User.Identity.Name, Request.UserHostAddress,iLoai);

            String TenBangChungTu="KT_ChungTu", TenBangChiTiet="KT_ChungTuChiTiet";

            //switch (iLoai)
            //{
            //    case 0:
            //        TenBangChungTu = "KT_ChungTu";
            //        TenBangChiTiet = "KT_ChungTuChiTiet";
            //        break;
            //    case 1:
            //        TenBangChungTu = "KTKB_ChungTu";
            //        TenBangChiTiet = "KTKB_ChungTuChiTiet";
            //        break;
            //    case 2:
            //        TenBangChungTu = "KTTG_ChungTu";
            //        TenBangChiTiet = "KTTG_ChungTuChiTiet";
            //        break;
            //    case 3:
            //        TenBangChungTu = "KTTM_ChungTu";
            //        TenBangChiTiet = "KTTM_ChungTuChiTiet";
            //        break;
            //}

            String SQL = String.Format("SELECT SUM(rSoTien) FROM {0} WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu",TenBangChiTiet);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Double rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (rTongSo == 0)
            {
                SQL = String.Format("DELETE {0} WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu",TenBangChungTu);
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            else
            {
                Bang bangChungTu = new Bang(TenBangChungTu);
                bangChungTu.DuLieuMoi = false;
                bangChungTu.GiaTriKhoa = iID_MaChungTu;
                bangChungTu.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);
                bangChungTu.MaNguoiDungSua = User.Identity.Name;
                bangChungTu.IPSua = Request.UserHostAddress;
                bangChungTu.Save();
            }
            return RedirectToAction("Index", "KeToanTongHop");

        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Fast_Submit_Tao_CTGS(String ParentID, String iNamLamViec)
        {
            String sSoChungTu_SaoChep = Request.Form[ParentID + "_sSoChungTu_SaoChep"];
            String iDisplay = Request.Form[ParentID + "_iDisplay"];
            if (String.IsNullOrEmpty(sSoChungTu_SaoChep))
            {
                return RedirectToAction("Index", "KeToanTongHop");
            }
            if (!String.IsNullOrEmpty(sSoChungTu_SaoChep))
            {
                Boolean kt_ChungTu = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu(sSoChungTu_SaoChep, iNamLamViec);
                Boolean kt_ChungTu_ChiTiet = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu_ChiTiet(sSoChungTu_SaoChep, iNamLamViec);
                if (kt_ChungTu == true || kt_ChungTu_ChiTiet == true)
                {
                    return RedirectToAction("Index", "KeToanTongHop");  
                }
                else
                {
                    int iID_MaChungTu = KeToanTongHop_ChungTuModels.InsertChungTu_By_CTGS(ParentID, Request.Form, User.Identity.Name, Request.UserHostAddress, sSoChungTu_SaoChep);
                    return RedirectToAction("Index", "KeToanTongHop");

                }
            }
            else
            {
                return RedirectToAction("Index", "KeToanTongHop");
            }
           
        }

        /// <summary>
        /// Lấy số chứng từ ghi sổ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult getSoChungTuGhiSo(String iID_MaChungTu)
        {
            string strGiaTri = KeToanTongHop_ChungTuModels.getSoChungTuGhiSo(iID_MaChungTu);
            JsonResult value = Json(strGiaTri, JsonRequestBehavior.AllowGet);
            return value;
        }
        [Authorize]
        public JsonResult get_CheckSoChungTuGhiSo(String iID_MaChungTu, String sSoChungTu,String iNamLamViec)
        {
            Boolean strGiaTri = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu_Trung(iID_MaChungTu, sSoChungTu, iNamLamViec);
            return Json(strGiaTri, JsonRequestBehavior.AllowGet);

            //Boolean Trung = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu_Trung(iID_MaChungTu, sSoChungTu, iNamLamViec);
            //// get_Check_iID_MaChungTu
            //Object item = new
            //{
            //    Trung = Trung
            //};
            //return Json(item, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult get_Check_iID_MaChungTu(String iID_MaChungTu)
        {
            Boolean Trung = KeToanTongHop_ChungTuModels.KiemTra_iID_MaChungTu_Trung(iID_MaChungTu);
           // get_Check_iID_MaChungTu
            Object item = new
            {
                Trung = Trung
            };
            return Json(item, JsonRequestBehavior.AllowGet);
           // return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
        //tuannn 28/9/2012
        /// <summary>
        /// trùng trả về giá trị true, ngược lại trả lại giá trị false
        /// </summary>D:\VIETTEL\VIETTEL\VIETTEL\Controllers\KeToanTongHop\KeToanTongHop_TimKiemController.cs
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
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);                
                Int32 vR1 = Convert.ToInt32(Connection.GetValue(cmd, 0));
                cmd.Dispose();
                if (vR1 > 0)
                {
                    DuLieuMoi = false;

                    DataTable dt = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
                    TrangThaiDuyet = Convert.ToInt16(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
                    if (TrangThaiDuyet != LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeKeToanTongHop))
                    {
                        DaDuyet = true;
                    }
                }
            }
            String GiaTri = String.Format("{0},{1}",sSoChungTu,iNamLamViec);
            String Field = "sSoChungTu,iNamLamViec";
            Boolean Trung = HamChung.Check_Trung("KT_ChungTu", "iID_MaChungTu", iID_MaChungTu, Field, GiaTri, DuLieuMoi);
            if (Trung)
            {
                iID_MaChungTu = KeToanTongHop_ChungTuModels.LayMaChungTu(sSoChungTu);
                DataTable dt = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
                TrangThaiDuyet = Convert.ToInt16(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                int iThangLamViec = Convert.ToInt16(dtCauHinh.Rows[0]["iThangLamViec"]);
                int iThang = Convert.ToInt16(dtCauHinh.Rows[0]["iThangLamViec"]);
                if (TrangThaiDuyet != LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeKeToanTongHop))
                {
                    DaDuyet = true;
                }
                if (iThangLamViec!=iThang)
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


        public String obj_ChungTu(String ParentID, String iNamLamViec, String iThang, int iLoai,Boolean LayChungTuChuaNhan)
        {
            DataTable dtCB = KeToanTongHop_ChungTuChiTietModels.LayDanhSachChungTuKeToan(iNamLamViec, iThang, iLoai);
            DataTable dtDaNhan = KeToanTongHop_ChungTuChiTietModels.LayDanhSachChungTuDaNhan(iNamLamViec, iThang, iLoai);
            String stbCB = " <div style=\"width: 100%; height: 250px; overflow: scroll; position: relative;\">";
            stbCB += "<table class=\"mGrid\">";
            for (int i = 0; i < dtCB.Rows.Count; i++)
            {
                String iID_MaChungTu = Convert.ToString(dtCB.Rows[i]["iID_MaChungTu"]);

                String strSoGhiSo = "";
                Boolean DaNhan = false;
                for (int j = 0; j < dtDaNhan.Rows.Count; j++)
                {
                    strSoGhiSo = "";
                    String TenTruong = "";
                    switch (iLoai)
                    {
                        case 0:
                            TenTruong = "iID_MaChungTu";
                            break;
                        case 1:
                            TenTruong = "iID_MaChungTu_KhoBac";
                            break;
                        case 2:
                            TenTruong = "iID_MaChungTu_TienGui";
                            break;
                        case 3:
                            TenTruong = "iID_MaChungTu_TienMat";
                            break;
                    }
                    String MaChungTuDaNhan = Convert.ToString(dtDaNhan.Rows[j][TenTruong]);
                    if (iID_MaChungTu.Equals(MaChungTuDaNhan))
                    {
                        DaNhan = true;
                        strSoGhiSo = Convert.ToString(dtDaNhan.Rows[j]["sSoChungTu"]);
                        break;
                    }
                }
                if (LayChungTuChuaNhan)
                {
                    if (DaNhan == false)
                    {
                        String strClass = "";
                        if (i % 2 == 0) strClass = "alt";

                        stbCB += "<tr class=" + strClass + ">";
                        stbCB += " <td align=\"center\" style=\"width: 25px; padding: 3px 2px;\">";


                        if (DaNhan == false)
                        {
                            stbCB += "<input type=\"radio\" name=\"" + ParentID + "_txt\" id=\"" + ParentID + "_txt\" value=\"" + iID_MaChungTu + "\" />";
                            stbCB += "<input type=\"hidden\" name=\"" + ParentID + "_" + iID_MaChungTu + "\" id=\"" + ParentID + "_" + iID_MaChungTu + "\" value=\"" + dtCB.Rows[i]["sNoiDung"] + "\" />";
                        }

                        stbCB += "</td> <td align=\"center\" style=\"padding: 3px 2px; width: 150px; font-weight: bold;\">" + strSoGhiSo + "</td>";
                        stbCB += "<td style=\"padding: 3px 2px;\">" + dtCB.Rows[i]["sSoChungTu"] + " - " + dtCB.Rows[i]["sNoiDung"] + " </td> </tr>";
                    }
                }
                else
                {
                    String strClass = "";
                    if (i % 2 == 0) strClass = "alt";

                    stbCB += "<tr class=" + strClass + ">";
                    stbCB += " <td align=\"center\" style=\"width: 25px; padding: 3px 2px;\">";


                    if (DaNhan == false)
                    {
                        stbCB += "<input type=\"radio\" name=\"" + ParentID + "_txt\" id=\"" + ParentID + "_txt\" value=\"" + iID_MaChungTu + "\" />";
                        stbCB += "<input type=\"hidden\" name=\"" + ParentID + "_" + iID_MaChungTu + "\" id=\"" + ParentID + "_" + iID_MaChungTu + "\" value=\"" + dtCB.Rows[i]["sNoiDung"] + "\" />";
                    }

                    stbCB += "</td> <td align=\"center\" style=\"padding: 3px 2px; width: 150px; font-weight: bold;\">" + strSoGhiSo + "</td>";
                    stbCB += "<td style=\"padding: 3px 2px;\">" + dtCB.Rows[i]["sSoChungTu"] + " - " + dtCB.Rows[i]["sNoiDung"] + " </td> </tr>";
                }

            }
            stbCB += "</table> </div>";
            dtCB.Dispose();
            return stbCB;
        }

        [Authorize]
        public JsonResult ds_ChungTu(String ParentID, String iNamLamViec, String iThang, int iLoai, Boolean LayChungTuChuaNhan)
        {
            return Json(obj_ChungTu(ParentID, iNamLamViec, iThang, iLoai, LayChungTuChuaNhan), JsonRequestBehavior.AllowGet);
        }
        //end tuannn 28/9/2012


        public String obj_LayPhieuThu_PhieuChi(String NamLuong, String ThangLuong, String iID_MaChungTu, String Loai, String ParentID)
        {
            DataTable dtCB = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietTienMat_TheoThang(NamLuong, ThangLuong, Loai);
            String stbCB = " <div style=\"width: 100%; height: 250px; overflow: scroll; position: relative;\">";
            stbCB += "<table class=\"mGrid\">";
            for (int i = 0; i < dtCB.Rows.Count; i++)
            {
                String strValue = Convert.ToString(dtCB.Rows[i]["iID_MaChungTuChiTiet"]);

                String strSoGhiSo = KeToanTongHop_ChungTuChiTietModels.LayChuoiChungTuGhiSoCuaChungTuChiTiet(strValue, ThangLuong, NamLuong, 3);

                String strClass = "";
                if (i % 2 == 0) strClass = "alt";


                stbCB += "<tr class=" + strClass + ">";
                stbCB += " <td align=\"center\" style=\"width: 25px; padding: 3px 2px;\">";

                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, ThangLuong, NamLuong, strValue, 3);
                if (intDaCo == 0)
                {

                    stbCB += "<input type=\"checkbox\" name=\"" + ParentID + "_txt\" id=\"" + ParentID + "_txt\" value=\"" + strValue + "\" group-index=\"1\" />";

                }

                stbCB += "</td> <td align=\"center\" style=\"padding: 3px 2px; width: 150px; font-weight: bold;\">" + strSoGhiSo + "</td>";
                stbCB += "<td style=\"padding: 3px 2px;\">" + dtCB.Rows[i]["sSoChungTuChiTiet"] + " - " + dtCB.Rows[i]["sNoiDung"] + " </td> </tr>";

            }
            stbCB += "</table> </div>";
            dtCB.Dispose();
            return stbCB;
        }
        /// <summary>
        /// Ajax hiển thị danh sách đơn vị
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="DuyetLuong"></param>
        /// <param name="arrDV"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult ds_LayPhieuThu_PhieuChi(String NamLuong, String ThangLuong, String iID_MaChungTu, String Loai, String ParentID)
        {
            return Json(obj_LayPhieuThu_PhieuChi(NamLuong, ThangLuong, iID_MaChungTu, Loai, ParentID), JsonRequestBehavior.AllowGet);
        }




        public String obj_UyNhiemChi(String NamLuong, String ThangLuong, String iID_MaChungTu, String Loai, String ParentID)
        {
            DataTable dtCB = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietTienGui_TheoThang(NamLuong, ThangLuong, Loai);
            String stbCB = " <div style=\"width: 100%; height: 250px; overflow: scroll; position: relative;\">";
            stbCB += "<table class=\"mGrid\">";
            for (int i = 0; i < dtCB.Rows.Count; i++)
            {
                String strValue = Convert.ToString(dtCB.Rows[i]["iID_MaChungTuChiTiet"]);

                String strSoGhiSo = KeToanTongHop_ChungTuChiTietModels.LayChuoiChungTuGhiSoCuaChungTuChiTiet(strValue, ThangLuong, NamLuong, 2);
                String strClass = "";
                if (i % 2 == 0) strClass = "alt";


                stbCB += "<tr class=" + strClass + ">";
                stbCB += " <td align=\"center\" style=\"width: 25px; padding: 3px 2px;\">";

                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, ThangLuong, NamLuong, strValue, 2);
                if (intDaCo == 0)
                {

                    stbCB += "<input type=\"checkbox\" name=\"" + ParentID + "_txt\" id=\"" + ParentID + "_txt\" value=\"" + strValue + "\" group-index=\"1\" />";

                }

                stbCB += "</td> <td align=\"center\" style=\"padding: 3px 2px; width: 150px; font-weight: bold;\">" + strSoGhiSo + "</td>";
                stbCB += "<td style=\"padding: 3px 2px;\">" + dtCB.Rows[i]["sSoChungTuChiTiet"] + " - " + dtCB.Rows[i]["sNoiDung"] + " </td> </tr>";

            }
            stbCB += "</table> </div>";
            dtCB.Dispose();
            return stbCB;
        }
        /// <summary>
        /// Lây danh sách ủy nhiệm chi
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="Loai"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult ds_UyNhiemChi(String NamLuong, String ThangLuong, String iID_MaChungTu, String Loai, String ParentID)
        {
            return Json(obj_UyNhiemChi(NamLuong, ThangLuong, iID_MaChungTu, Loai, ParentID), JsonRequestBehavior.AllowGet);
        }


        public String obj_RutDuToan(String NamLuong, String ThangLuong, String iID_MaChungTu, String Loai, String ParentID)
        {
            DataTable dtCB = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietKhoBac_TheoThang(NamLuong, ThangLuong, Loai);
            String stbCB = " <div style=\"width: 100%; height: 250px; overflow: scroll; position: relative;\">";
            stbCB += "<table class=\"mGrid\">";
            for (int i = 0; i < dtCB.Rows.Count; i++)
            {
                String strValue = Convert.ToString(dtCB.Rows[i]["iID_MaChungTuChiTiet"]);

                String strSoGhiSo = KeToanTongHop_ChungTuChiTietModels.LayChuoiChungTuGhiSoCuaChungTuChiTiet(strValue, ThangLuong, NamLuong, 1);
                String strClass = "";
                if (i % 2 == 0) strClass = "alt";


                stbCB += "<tr class=" + strClass + ">";
                stbCB += " <td align=\"center\" style=\"width: 25px; padding: 3px 2px;\">";

                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, ThangLuong, NamLuong, strValue, 1);
                if (intDaCo == 0)
                {

                    stbCB += "<input type=\"checkbox\" name=\"" + ParentID + "_txt\" id=\"" + ParentID + "_txt\" value=\"" + strValue + "\" group-index=\"1\" />";

                }

                stbCB += "</td> <td align=\"center\" style=\"padding: 3px 2px; width: 150px; font-weight: bold;\">" + strSoGhiSo + "</td>";
                stbCB += "<td style=\"padding: 3px 2px;\">" + dtCB.Rows[i]["sSoChungTuChiTiet"] + " - " + dtCB.Rows[i]["sNoiDung"] + " </td> </tr>";

            }
            stbCB += "</table> </div>";
            dtCB.Dispose();
            return stbCB;
        }
        /// <summary>
        /// Lây danh sách ủy nhiệm chi
        /// </summary>
        /// <param name="NamLuong"></param>
        /// <param name="ThangLuong"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="Loai"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult ds_RutDuToan(String NamLuong, String ThangLuong, String iID_MaChungTu, String Loai, String ParentID)
        {
            return Json(obj_RutDuToan(NamLuong, ThangLuong, iID_MaChungTu, Loai, ParentID), JsonRequestBehavior.AllowGet);
        }
        
    }
}
