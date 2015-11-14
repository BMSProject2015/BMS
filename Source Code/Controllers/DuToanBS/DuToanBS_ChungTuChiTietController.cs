using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;

namespace VIETTEL.Controllers.ChungTuChiTiet
{
    public class DuToanBS_ChungTuChiTietController : Controller
    {
        //
        // GET: /DuToanBS_ChungTuChiTiet/
        public string sViewPath = "~/Views/DuToanBS/ChungTuChiTiet/";

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iLoai"></param>
        /// <param name="iChiTapTrung"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(string iID_MaChungTu, string sLNS, string iID_MaDonVi, string iLoai, string iChiTapTrung)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS"] = sLNS;
            ViewData["iLoai"] = iLoai;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iChiTapTrung"] = iChiTapTrung;
            return View(sViewPath + "ChungTuChiTiet_Index.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index_Gom(string iID_MaChungTu)
        {

            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            //loai gom
            ViewData["iLoai"] = 1;
            return View(sViewPath + "ChungTuChiTiet_Index.aspx");
        }

        /// <summary>
        /// Luoi chung tu chi tiet
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="MaLoai"></param>
        /// <param name="iLoai"></param>
        /// <param name="iChiTapTrung"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame(string sLNS, string MaLoai, string iLoai, string iChiTapTrung)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string iID_MaChungTu = Request.Form["DuToan_iID_MaChungTu"];

            //TuNB delete

            //string DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            //string[] arrDSTruong = DSTruong.Split(',');
            //Dictionary<string, string> dicGiaTriTimKiem = new Dictionary<string, string>();
            //for (int i = 0; i < arrDSTruong.Length; i++)
            //{
            //    dicGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            //}
            MaLoai = Request.Form["DuToan_MaLoai"];
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["MaLoai"] = MaLoai;
            ViewData["iLoai"] = iLoai;
            ViewData["iChiTapTrung"] = iChiTapTrung;
            return View(sViewPath + "ChungTuChiTiet_Index_DanhSach_Frame.aspx", new { sLNS = sLNS });
            //return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, LoadLai = "1" });
        }

        
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ChiNganSach, String iID_MaChungTu, String sLNS,String iLoai,String iChiTapTrung)
        {
            NameValueCollection data;
            if (iLoai == "4")
            {
                 data = DuToanBS_ChungTuModels.LayThongTin_KyThuatLan2(iID_MaChungTu);
            }

            else
            {
                data = DuToanBS_ChungTuModels.LayThongTinChungTu(iID_MaChungTu);
            }
           

            String MaND = User.Identity.Name;
            String TenBangChiTiet = "DTBS_ChungTuChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idMaMucLucNganSach = Request.Form["idMaMucLucNganSach"];
            string MaLoai = Request.Form["DuToan_MaLoai1"];
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }

            String[] arrMaMucLucNganSach = idMaMucLucNganSach.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { CapPhat_BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaCapPhatChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    iID_MaCapPhatChiTiet = arrMaHang[i].Split('_')[0];
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
                            if (sLNS == "1040100")
                            {
                                String SQL = "UPDATE DTBS_ChungTuChiTIet_PhanCap SET iTrangThai=0 WHERE iID_MaChungTu=@iID_MaChungTu";
                                SqlCommand cmd = new SqlCommand(SQL);
                                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaCapPhatChiTiet);
                                Connection.UpdateDatabase(cmd);

                                 SQL = "UPDATE DTBS_ChungTuChiTIet SET iTrangThai=0 WHERE iID_MaChungTu=@iID_MaChungTu";
                                 cmd = new SqlCommand(SQL);
                                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaCapPhatChiTiet);
                                Connection.UpdateDatabase(cmd);
                            }
                        }
                    }
                    else
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                        Boolean okCoThayDoi = false;
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {

                            if (arrThayDoi[j] == "1")
                            {
                                //kiem tra xem co ma dơn vi hay ko, vi tri la 26
                                if (iChiTapTrung == "1")
                                {
                                }
                                else
                                {
                                    if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                                    {
                                        if (arrGiaTri[j] == "")
                                        {
                                            if (iID_MaCapPhatChiTiet == "")
                                            {
                                                okCoThayDoi = false;
                                                break;
                                            }
                                            else
                                            {
                                                okCoThayDoi = true;
                                                break;

                                            }
                                        }
                                    }
                                }
                                okCoThayDoi = true;
                            }

                        }
                        if (okCoThayDoi)
                        {
                            if (i == 1 && iLoai == "4")
                                TenBangChiTiet = "DTBS_ChungTuChiTiet_PhanCap";
                            else
                                TenBangChiTiet = "DTBS_ChungTuChiTiet";
                            Bang bang = new Bang(TenBangChiTiet);
                            if (iID_MaCapPhatChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                //Them cac tham so tu bang CP_CapPhat
                                
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                                bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", data["sTenPhongBan"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguon", data["iID_MaNguon"]);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", data["iID_MaTrangThaiDuyet"]);
                                bang.CmdParams.Parameters.AddWithValue("@iKyThuat", data["iKyThuat"]);
                                bang.CmdParams.Parameters.AddWithValue("@dNgayCapPhat", data["dngayChungTu"]);
                                if (iChiTapTrung == "1")
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", data["iID_MaDonVi"]);
                                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", data["iID_MaDonVi"]+" - "+DonViModels.Get_TenDonVi(data["iID_MaDonVi"]));
                                }
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.DuLieuMoi = false;
                               
                            }
                            if (iLoai == "4")
                                bang.CmdParams.Parameters.AddWithValue("@MaLoai", "2");
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;

                            if (iID_MaCapPhatChiTiet == "")
                            {
                                //Xác định xâu mã nối


                                String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                                DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                                //Dien thong tin cua Muc luc ngan sach
                                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                                dtMucLuc.Dispose();
                            }


                            //Them tham so
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {
                                if (arrThayDoi[j] == "1")
                                {
                                    if (arrMaCot[j].EndsWith("_ConLai") == false)
                                    {
                                        String Truong = "@" + arrMaCot[j];
                                        //doi lai ten truong
                                        if (arrMaCot[j] == "sTenDonVi_BaoDam")
                                        {
                                            Truong = "@sTenDonVi";
                                        }
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
                            }
                            if (!bang.CmdParams.Parameters.Contains("iID_MaPhongBanDich"))
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", data["iID_MaPhongBan"]);
                            bang.Save();
                        }
                    }
                }
            }
            string idAction = Request.Form["idAction"];
            string sLyDo = Request.Form["sLyDo"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS,iLoai=iLoai,iChiTapTrung=iChiTapTrung, iKyThuat = data["iKyThuat"], sLyDo = sLyDo });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS,iLoai=iLoai,iChiTapTrung=iChiTapTrung, iKyThuat = data["iKyThuat"], sLyDo = sLyDo });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS, iLoai = iLoai,iChiTapTrung=iChiTapTrung });
        }


        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTu, String iLoai, String sLNS, String iKyThuat,String sLyDo, String iID_MaChungTu_TLTH)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            if (sLNS.StartsWith("1040100") && iKyThuat=="1")
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_BaoDam_TrinhDuyet(MaND, iID_MaChungTu);
            else
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);
            //update ly do
            DuToanBS_ChungTuModels.updateLyDo_ChungTu(iID_MaChungTu, sLyDo);
            ///Update trạng thái cho bảng chứng từ
            DuToanBS_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);
            DuToanBS_ChungTuModels.updateChungTu_TLTH(iID_MaChungTu, MaND, sLNS);
            ViewData["LoadLai"] = "1";
            //Duyet tu danh sach chung tu
            if (iLoai == "1")
            {
                //neu la ngân sách bảo đảm
                if (sLNS.Contains("1040100"))
                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS,iKyThuat=iKyThuat, iID_MaChungTu = iID_MaChungTu_TLTH });
                return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS, iKyThuat = iKyThuat,iID_MaChungTu = iID_MaChungTu_TLTH });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTu, String iLoai, String sLNS, String sLyDo, String iID_MaChungTu_TLTH)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_BaoDam_TuChoi(MaND, iID_MaChungTu);
            else
                iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);
            //update ly do
            DuToanBS_ChungTuModels.updateLyDo_ChungTu(iID_MaChungTu, sLyDo);
            DuToanBS_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, true, MaND, Request.UserHostAddress);
            DuToanBS_ChungTuModels.updateChungTu_TLTH(iID_MaChungTu, MaND, sLNS);
            if (iLoai == "1")
            {
                //neu la ngân sách bảo đảm
                if (sLNS.Contains("1040100"))
                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS, iID_MaChungTu = iID_MaChungTu_TLTH });
                return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS, iID_MaChungTu = iID_MaChungTu_TLTH });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChungTuChiTiet, String iID_MaChungTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTuChiTiet", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = DuToanBS_ChungTuChiTietModels.Delete_ChungTuChiTiet(iID_MaChungTuChiTiet, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaChungTu)
        {
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }

        #region Lấy 1 hang AJAX: rTongSoNamTruoc
        [Authorize]
        public JsonResult get_GiaTri(String Truong, String GiaTri, String DSGiaTri)
        {
            if (Truong == "rTongSoNamTruoc")
            {
                return get_GiaTriTongSoNamTruoc(GiaTri, DSGiaTri);
            }
            return null;
        }

        private JsonResult get_GiaTriTongSoNamTruoc(String GiaTri, String DSGiaTri)
        {
            String iID_MaChungTu = GiaTri;
            String strDSTruong = "iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSGiaTri = DSGiaTri.Split(',');

            SqlCommand cmd = new SqlCommand();
            String DK = "";

            NameValueCollection data = DuToan_ChungTuModels.LayThongTin(iID_MaChungTu);
            //DK = String.Format("iNamLamViec={0} AND iID_MaNguonNganSach={1} AND iID_MaNamNganSach={2}", Convert.ToInt32(data["iNamLamViec"]) - 1, data["iID_MaNguonNganSach"], data["iID_MaNamNganSach"]);
            DK = "iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach";
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(data["iNamLamViec"]) - 1);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
            int i = 0;

            for (i = 0; i < arrDSGiaTri.Length; i++)
            {
                DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                //DK += String.Format(" AND {0}='{1}'", arrDSTruong[i], arrDSGiaTri[i]);
            }

            String SQL = String.Format("SELECT SUM(rTongSo) FROM DT_ChungTuChiTiet WHERE bLaHangCha=0 AND {0}", DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            Object item = new
            {
                value = "0",
                label = "0"
            };

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                item = new
                {
                    value = dt.Rows[0][0],
                    label = dt.Rows[0][0]
                };
            }
            dt.Dispose();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lấy 1 hang AJAX: LayPhongBan
        [Authorize]
        public JsonResult GetPhongBanCuaDonVi(String iID_MaDonVi)
        {
            String MaDonVi = iID_MaDonVi.Split('-')[0];
            String iID_MaPhongBan = DonViModels.getPhongBanCuaDonVi(MaDonVi);
            Object item = new
            {
                value = iID_MaPhongBan,
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
