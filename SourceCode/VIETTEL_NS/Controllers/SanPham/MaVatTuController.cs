using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace VIETTEL.Controllers
{
    public class MaVatTuController : Controller
    {
        //
        // GET: /MaVatTu/
        public string sViewPath = "~/Views/SanPham/MaVatTu/";
        [Authorize]
        public ActionResult Index(int? MaVatTu_page, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iTrangThai)
        {
            ViewData["MaVatTu_page"] = MaVatTu_page;
            ViewData["MaNhomLoaiVatTu"] = MaNhomLoaiVatTu;
            ViewData["MaNhomChinh"] = MaNhomChinh;
            ViewData["MaNhomPhu"] = MaNhomPhu;
            ViewData["MaChiTietVatTu"] = MaChiTietVatTu;
            ViewData["MaXuatXu"] = MaXuatXu;
            ViewData["iTrangThai"] = iTrangThai;
            return View(sViewPath + "Index.aspx");
        }
        [Authorize]
        public ActionResult Edit(int? VTNCC, String iID_MaVatTu, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu)
        {
            ViewData["MaNhomLoaiVatTu"] = MaNhomLoaiVatTu;
            ViewData["MaNhomChinh"] = MaNhomChinh;
            ViewData["MaNhomPhu"] = MaNhomPhu;
            ViewData["MaChiTietVatTu"] = MaChiTietVatTu;
            ViewData["MaXuatXu"] = MaXuatXu;

            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaVatTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaVatTu"] = iID_MaVatTu;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        public ActionResult CapNhatNCC(int? VTNCC_page, String iID_MaVatTu, String sMaVatTu, String sTen)
        {
            ViewData["VTNCC_page"] = VTNCC_page;
            ViewData["iID_MaVatTu"] = iID_MaVatTu;
            ViewData["sMaVatTu"] = sMaVatTu;
            ViewData["sTen"] = sTen;
            return View(sViewPath + "CapNhatNCC.aspx");
        }

        public JsonResult Update_VTNCC(String ck, String iID_MaVatTu, String iID_MaNhaCungCap, String sID_MaNguoiDungSua, String IPSua)
        {
            return Json(Update_objVTNCC(ck, iID_MaVatTu, iID_MaNhaCungCap, sID_MaNguoiDungSua, IPSua), JsonRequestBehavior.AllowGet);
        }

        public static object Update_objVTNCC(String ck, String iID_MaVatTu, String iID_MaNhaCungCap, String sID_MaNguoiDungSua, String IPSua)
        {
            try
            {
                if (ck == "true")
                {
                    if (iID_MaVatTu != "" && iID_MaNhaCungCap != "")
                    {
                        Bang bang = new Bang("DM_VatTu_NhaCungCap");
                        bang.DuLieuMoi = true;
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNhaCungCap", iID_MaNhaCungCap);
                        bang.MaNguoiDungSua = sID_MaNguoiDungSua;
                        bang.IPSua = IPSua;
                        bang.Save();
                        //cmd = new SqlCommand("INSERT INTO DM_NhaCungCap()iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("DELETE  FROM DM_VatTu_NhaCungCap WHERE iID_MaVatTu = @iID_MaVatTu AND " +
                                                      "iID_MaNhaCungCap = @iID_MaNhaCungCap");
                    cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                    cmd.Parameters.AddWithValue("@iID_MaNhaCungCap", iID_MaNhaCungCap);                    
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }
            }
            catch
            {}
            
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaVatTu, String iTrangThai)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_VatTu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String TenVatTu = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String DonViTinh = Convert.ToString(Request.Form[ParentID + "_iDM_MaDonViTinh"]);
            String sMaVatTu = Convert.ToString(Request.Form[ParentID + "_sMaVatTu"]);
            if (TenVatTu == string.Empty || TenVatTu == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên vật tư!");
            }
            if (DonViTinh == string.Empty || DonViTinh == "dddddddd-dddd-dddd-dddd-dddddddddddd")
            {
                arrLoi.Add("err_iDM_MaDonViTinh", "Bạn chưa chọn đơn vị tính!");
            }
            if (iTrangThai != "1")
            {
                if (sMaVatTu != string.Empty || sMaVatTu != "")
                {
                    cmd = new SqlCommand();
                    if (String.IsNullOrEmpty(iID_MaVatTu))
                    {
                        cmd.CommandText = "SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu=@sMaVatTu";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu=@sMaVatTu AND iID_MaVatTu <> @iID_MaVatTu";
                        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                    }
                    cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                    DataTable dtVatTu = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    if (dtVatTu.Rows.Count > 0)
                    {
                        arrLoi.Add("err_sMaVatTu", "Trùng mã vật tư!");
                    }
                }
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaVatTu"] = iID_MaVatTu;
                ViewData["sTen"] = TenVatTu;
                ViewData["iDM_MaDonViTinh"] = DonViTinh;
                if (sMaVatTu != string.Empty || sMaVatTu != "")
                {
                    String iDM_MaNhomLoaiVatTu = Convert.ToString(Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"]);
                    String iDM_MaNhomChinh = Convert.ToString(Request.Form[ParentID + "_iDM_MaNhomChinh"]);
                    String iDM_MaNhomPhu = Convert.ToString(Request.Form[ParentID + "_iDM_MaNhomPhu"]);
                    String iDM_MaChiTietVatTu = Convert.ToString(Request.Form[ParentID + "_iDM_MaChiTietVatTu"]);
                    String sTenChiTietVatTu = Convert.ToString(Request.Form[ParentID + "_sTenKhoa"]);
                    String iDM_MaXuatXu = Convert.ToString(Request.Form[ParentID + "_iDM_MaXuatXu"]);
                    ViewData["sMaVatTu"] = sMaVatTu;
                    ViewData["MaNhomLoaiVatTu"] = iDM_MaNhomLoaiVatTu;
                    ViewData["MaNhomChinh"] = iDM_MaNhomChinh;
                    ViewData["MaNhomPhu"] = iDM_MaNhomPhu;
                    ViewData["MaChiTietVatTu"] = iDM_MaChiTietVatTu;
                    ViewData["sTenChiTietVatTu"] = sTenChiTietVatTu;
                    ViewData["MaXuatXu"] = iDM_MaXuatXu;
                }
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                String MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"];
                String MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String MaChiTietVatTu = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
                String sTenKhoa = Convert.ToString(Request.Form[ParentID + "_sTenKhoa"]);
                String MaXuatXu = Request.Form[ParentID + "_iDM_MaXuatXu"];

                if (MaNhomLoaiVatTu != "" && MaNhomChinh != "" && MaNhomPhu != "")
                {
                    cmd = new SqlCommand("SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=@iID_MaLoaiDanhMuc AND iID_MaDanhMucCha=@iID_MaDanhMucCha AND sTenKhoa=@sTenKhoa");
                    cmd.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "4cdb608e-e9cc-42c2-bec2-f4172baa503e");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", MaNhomPhu);
                    cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
                    DataTable dtDanhMuc = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    if (dtDanhMuc.Rows.Count > 0)
                    {
                        MaChiTietVatTu = Convert.ToString(dtDanhMuc.Rows[0]["iID_MaDanhMuc"]);
                    }
                    else
                    {
                        MaChiTietVatTu = "";
                    }
                }
                if (MaChiTietVatTu == "" && sTenKhoa != "" && MaNhomLoaiVatTu != "" && MaNhomChinh != "" && MaNhomPhu != "")
                {
                    if (sTenKhoa.Length == 5) {
                        String MaLoaiDanhMuc = "";
                        cmd = new SqlCommand("SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang");
                        cmd.Parameters.AddWithValue("@sTenBang", "ChiTietVatTu");
                        MaLoaiDanhMuc = Connection.GetValueString(cmd, "");
                        cmd.Dispose();

                        Bang bangdm = new Bang("DC_DanhMuc");
                        bangdm.DuLieuMoi = true;
                        bangdm.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", MaLoaiDanhMuc);
                        bangdm.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", MaNhomPhu);
                        bangdm.CmdParams.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
                        bangdm.CmdParams.Parameters.AddWithValue("@bHoatDong", 1);
                        bangdm.CmdParams.Parameters.AddWithValue("@bDangDung", 0);
                        bangdm.Save();

                        MaChiTietVatTu = Convert.ToString(bangdm.GiaTriKhoa);
                    }
                }

                Bang bang = new Bang("DM_VatTu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                String sTuKhoa_sTen = Request.Form[ParentID + "_sTen"] + " ";
                sTuKhoa_sTen += NgonNgu.LayXauKhongDauTiengViet(Request.Form[ParentID + "_sTen"]);
                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTuKhoa_sTen);

                String sTuKhoa_sTenGoc = Request.Form[ParentID + "_sTenGoc"] + " ";
                sTuKhoa_sTenGoc += NgonNgu.LayXauKhongDauTiengViet(Request.Form[ParentID + "_sTenGoc"]) + " ";
                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTenGoc", sTuKhoa_sTenGoc);

                String sTuKhoa_sQuyCach = Request.Form[ParentID + "_sQuyCach"] + " ";
                sTuKhoa_sQuyCach += NgonNgu.LayXauKhongDauTiengViet(Request.Form[ParentID + "_sQuyCach"]);
                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sTuKhoa_sQuyCach);                

                if (String.IsNullOrEmpty(MaNhomLoaiVatTu) && bang.CmdParams.Parameters.IndexOf("@iDM_MaNhomLoaiVatTu") >= 0)
                    bang.CmdParams.Parameters["@iDM_MaNhomLoaiVatTu"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(MaNhomChinh) && bang.CmdParams.Parameters.IndexOf("@iDM_MaNhomChinh") >= 0)
                    bang.CmdParams.Parameters["@iDM_MaNhomChinh"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(MaNhomPhu) && bang.CmdParams.Parameters.IndexOf("@iDM_MaNhomPhu") >= 0)
                    bang.CmdParams.Parameters["@iDM_MaNhomPhu"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(MaChiTietVatTu) && bang.CmdParams.Parameters.IndexOf("@iDM_MaChiTietVatTu") >= 0)
                {
                    bang.CmdParams.Parameters["@iDM_MaChiTietVatTu"].Value = DBNull.Value;
                }
                else {
                    bang.CmdParams.Parameters["@iDM_MaChiTietVatTu"].Value = MaChiTietVatTu;
                }
                if (String.IsNullOrEmpty(MaXuatXu) && bang.CmdParams.Parameters.IndexOf("@iDM_MaXuatXu") >= 0)
                    bang.CmdParams.Parameters["@iDM_MaXuatXu"].Value = DBNull.Value;

                String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
                if (String.IsNullOrEmpty(iID_MaDonVi) && bang.CmdParams.Parameters.IndexOf("@iID_MaDonVi") >= 0)
                    bang.CmdParams.Parameters["@iID_MaDonVi"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(Request.Form[ParentID + "_iTrangThai"]))
                {
                    //Nếu đơn vị sửa vật tư -> vật tư đó chuyển về trạng thái chờ duyệt
                    if (bang.CmdParams.Parameters["@iID_MaDonVi"].Value != DBNull.Value) iTrangThai = "2";
                    //if (iTrangThai == "3") iTrangThai = "2";
                    bang.CmdParams.Parameters.AddWithValue("@iTrangThai", iTrangThai);
                }
                else
                    iTrangThai = Request.Form[ParentID + "_iTrangThai"];
                int iHanhDong = 1;
                if (!bang.DuLieuMoi)
                {
                    bang.GiaTriKhoa = iID_MaVatTu;
                    iHanhDong = 2;
                }
                String dNgayPhatSinhMa = Request.Form[ParentID + "_NgayPhatSinhMa"];
                String[] tg = dNgayPhatSinhMa.Split('/');
                if(tg.Length>=3) dNgayPhatSinhMa = tg[1] + "/" + tg[0] + "/" + tg[2];
                bang.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", dNgayPhatSinhMa);
                bang.Save();

                //Thêm giá trị giá vào bảng giá của vật tư
                Bang banggia = new Bang("DM_VatTu_Gia");
                banggia.MaNguoiDungSua = User.Identity.Name;
                banggia.IPSua = Request.UserHostAddress;
                //banggia.DuLieuMoi = bang.DuLieuMoi;
                //if (!banggia.DuLieuMoi)
                //{
                //    SqlCommand _CMD = new SqlCommand("SELECT TOP 1 iID_MaGiaVatTu FROM DM_VatTu_Gia WHERE iID_MaVatTu = @iID_MaVatTu ORDER BY dNgayTao DESC");
                //    _CMD.Parameters.AddWithValue("@iID_MaVatTu", bang.GiaTriKhoa);
                //    banggia.GiaTriKhoa = Connection.GetValue(_CMD,"0");
                //}
                banggia.TruyenGiaTri(ParentID, Request.Form);
                banggia.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", bang.GiaTriKhoa);
                banggia.DuLieuMoi = true;
                banggia.Save();

                //Thêm vào bảng lịch sử giao dịch
                Bang bangls = new Bang("DM_LichSuGiaoDich");
                bangls.MaNguoiDungSua = User.Identity.Name;
                bangls.IPSua = Request.UserHostAddress;
                bangls.TruyenGiaTri(ParentID, Request.Form);
                bangls.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTuKhoa_sTen);
                bangls.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTenGoc", sTuKhoa_sTenGoc);
                bangls.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sTuKhoa_sQuyCach);
                if (String.IsNullOrEmpty(MaNhomLoaiVatTu) && bangls.CmdParams.Parameters.IndexOf("@iDM_MaNhomLoaiVatTu") >= 0)
                    bangls.CmdParams.Parameters["@iDM_MaNhomLoaiVatTu"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(MaNhomChinh) && bangls.CmdParams.Parameters.IndexOf("@iDM_MaNhomChinh") >= 0)
                    bangls.CmdParams.Parameters["@iDM_MaNhomChinh"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(MaNhomPhu) && bangls.CmdParams.Parameters.IndexOf("@iDM_MaNhomPhu") >= 0)
                    bangls.CmdParams.Parameters["@iDM_MaNhomPhu"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(MaChiTietVatTu) && bangls.CmdParams.Parameters.IndexOf("@iDM_MaChiTietVatTu") >= 0)
                {
                    bangls.CmdParams.Parameters["@iDM_MaChiTietVatTu"].Value = DBNull.Value;
                }
                else
                {
                    bangls.CmdParams.Parameters["@iDM_MaChiTietVatTu"].Value = MaChiTietVatTu;
                }
                if (String.IsNullOrEmpty(MaXuatXu) && bangls.CmdParams.Parameters.IndexOf("@iDM_MaXuatXu") >= 0)
                    bangls.CmdParams.Parameters["@iDM_MaXuatXu"].Value = DBNull.Value;
                if (String.IsNullOrEmpty(iID_MaDonVi) && bangls.CmdParams.Parameters.IndexOf("@iID_MaDonVi") >= 0)
                    bangls.CmdParams.Parameters["@iID_MaDonVi"].Value = DBNull.Value;
                bangls.DuLieuMoi = true;
                if (String.IsNullOrEmpty(Request.Form[ParentID + "_iTrangThai"]))
                    bangls.CmdParams.Parameters.AddWithValue("@iTrangThai", iTrangThai);
                bangls.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", bang.GiaTriKhoa);
                bangls.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", dNgayPhatSinhMa);
                bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", iHanhDong);
                bangls.Save();

                return RedirectToAction("Index", new { MaNhomLoaiVatTu = MaNhomLoaiVatTu, MaNhomChinh = MaNhomChinh, MaNhomPhu = MaNhomPhu, MaChiTietVatTu = MaChiTietVatTu, MaXuatXu = MaXuatXu, iTrangThai = iTrangThai });
            }
        }

        public ActionResult Delete(String ParentID, String iID_MaVatTu, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iTrangThai)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DM_VatTu WHERE iID_MaVatTu = @iID_MaVatTu");
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                Bang bangls = new Bang("DM_LichSuGiaoDich");
                bangls.MaNguoiDungSua = User.Identity.Name;
                bangls.IPSua = Request.UserHostAddress;

                bangls.CmdParams.Parameters.AddWithValue("@sMaVatTu", Convert.ToString(dt.Rows[0]["sMaVatTu"]));
                bangls.CmdParams.Parameters.AddWithValue("@sTen", Convert.ToString(dt.Rows[0]["sTen"]));
                bangls.CmdParams.Parameters.AddWithValue("@sTenGoc", Convert.ToString(dt.Rows[0]["sTenGoc"]));
                bangls.CmdParams.Parameters.AddWithValue("@sQuyCach", Convert.ToString(dt.Rows[0]["sQuyCach"]));
                bangls.CmdParams.Parameters.AddWithValue("@sMoTa", Convert.ToString(dt.Rows[0]["sMoTa"]));
                bangls.CmdParams.Parameters.AddWithValue("@sMoTaGoc", Convert.ToString(dt.Rows[0]["sMoTaGoc"]));
                bangls.CmdParams.Parameters.AddWithValue("@sGhiChu", Convert.ToString(dt.Rows[0]["sGhiChu"]));
                bangls.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", Convert.ToString(dt.Rows[0]["dNgayPhatSinhMa"]));
                bangls.CmdParams.Parameters.AddWithValue("@rSoLuongTonKho", Convert.ToString(dt.Rows[0]["rSoLuongTonKho"]));
                if (Convert.ToString(dt.Rows[0]["dNgayCapNhatTonKho"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@dNgayCapNhatTonKho", Convert.ToString(dt.Rows[0]["dNgayCapNhatTonKho"]));
                bangls.CmdParams.Parameters.AddWithValue("@iTrangThai", Convert.ToString(dt.Rows[0]["iTrangThai"]));
                bangls.CmdParams.Parameters.AddWithValue("@sFileDinhKem", Convert.ToString(dt.Rows[0]["sFileDinhKem"]));
                if (Convert.ToString(dt.Rows[0]["iDM_MaDonViTinh"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", Convert.ToString(dt.Rows[0]["iDM_MaDonViTinh"]));
                if (Convert.ToString(dt.Rows[0]["iDM_MaNhomLoaiVatTu"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", Convert.ToString(dt.Rows[0]["iDM_MaNhomLoaiVatTu"]));
                if (Convert.ToString(dt.Rows[0]["iDM_MaNhomChinh"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", Convert.ToString(dt.Rows[0]["iDM_MaNhomChinh"]));
                if (Convert.ToString(dt.Rows[0]["iDM_MaNhomPhu"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", Convert.ToString(dt.Rows[0]["iDM_MaNhomPhu"]));
                if (Convert.ToString(dt.Rows[0]["iDM_MaChiTietVatTu"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", Convert.ToString(dt.Rows[0]["iDM_MaChiTietVatTu"]));
                if (Convert.ToString(dt.Rows[0]["iDM_MaXuatXu"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", Convert.ToString(dt.Rows[0]["iDM_MaXuatXu"]));
                if (Convert.ToString(dt.Rows[0]["iID_MaDonVi"]) != "")
                    bangls.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", Convert.ToString(dt.Rows[0]["iID_MaDonVi"]));
                bangls.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", 6);
                bangls.DuLieuMoi = true;
                bangls.Save();
            }
            Bang bang = new Bang("DM_VatTu");
            bang.GiaTriKhoa = iID_MaVatTu;
            bang.Delete();
            return RedirectToAction("Index", new { MaNhomLoaiVatTu = MaNhomLoaiVatTu, MaNhomChinh = MaNhomChinh, MaNhomPhu = MaNhomPhu, MaChiTietVatTu = MaChiTietVatTu, MaXuatXu = MaXuatXu, iTrangThai = iTrangThai });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            String MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"];
            String MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
            String MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
            String MaChiTietVatTu = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
            String MaXuatXu = Request.Form[ParentID + "_iDM_MaXuatXu"];
            String iTrangThai = Request.Form[ParentID + "_iTrangThai"];

            return RedirectToAction("Index", new { MaNhomLoaiVatTu = MaNhomLoaiVatTu, MaNhomChinh = MaNhomChinh, MaNhomPhu = MaNhomPhu, MaChiTietVatTu = MaChiTietVatTu, MaXuatXu = MaXuatXu, iTrangThai = iTrangThai });
        }

        [Authorize]
        public ActionResult ExportExcel(String ParentID, String DK)
        {
            Export("SELECT sMaVatTu, sTen, iDM_MaDonViTinh, sQuyCach, sMoTa, sMaCu, sTenGoc, sMoTaGoc, sNhaSanXuat, sFileDinhKem, iID_MaDonVi FROM DM_VatTu WHERE " + DK);
            return RedirectToAction("Search", new { ParentID = ParentID });
        }

        public void Export(String SQL)
        {
            SqlCommand cmd = new SqlCommand(SQL + " ORDER BY sMaVatTu");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM NS_DonVi");
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();

            cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'DonViTinh') ORDER BY sTen");
            DataTable dtDonViTinh = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i, j, z;
            Response.BufferOutput = false;
            Response.Charset = string.Empty;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            string attachment = "attachment; filename=DanhMucVatTu.xls";
            Response.AddHeader("content-disposition", attachment);
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());

            string textOutput =
            "<table border='1' bordercolor='black' rules='all'>\r\n";
            Response.Write(textOutput);

            textOutput = "<tr height=17 style='height:12.75pt'>\r\n";
            string sTitle = "<th align='left'>{0}</th>";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "sMaVatTu")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Mã vật tư");
                if (dt.Columns[i].ColumnName == "sTen")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Tên");
                if (dt.Columns[i].ColumnName == "iDM_MaDonViTinh")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Đơn vị tính");
                if (dt.Columns[i].ColumnName == "sQuyCach")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Quy cách");
                if (dt.Columns[i].ColumnName == "sMoTa")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Mô tả");
                if (dt.Columns[i].ColumnName == "sMaCu")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Mô hệ thống cũ");
                if (dt.Columns[i].ColumnName == "sTenGoc")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Tên gốc");
                if (dt.Columns[i].ColumnName == "sMoTaGoc")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Mô tả gốc");
                if (dt.Columns[i].ColumnName == "sNhaSanXuat")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Nhà sản xuất");
                if (dt.Columns[i].ColumnName == "sFileDinhKem")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "File đính kèm");
                if (dt.Columns[i].ColumnName == "iID_MaDonVi")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Đơn vị phát sinh");
            }
            textOutput += "</tr>\r\n";
            Response.Write(textOutput);

            textOutput = string.Empty;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = dt.Rows[i];
                textOutput = "<tr height=17 style='height:12.75pt'>\r\n";

                for (j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    string dataRowText = dataRow[j].ToString().Trim();

                    dataRowText = dataRowText.Replace("\t", " ");
                    dataRowText = dataRowText.Replace("\r", string.Empty);
                    dataRowText = dataRowText.Replace("\n", " ");

                    string sContent = "<td align='left' style='mso-number-format:\\@;'>{0}</td>";

                    if (dt.Columns[j].ColumnName == "sMaVatTu")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sTen")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "iDM_MaDonViTinh")
                    {
                        if (dataRowText == "") dataRowText = "";
                        else
                        {
                            for (z = 0; z < dtDonViTinh.Rows.Count; z++)
                            {
                                if (dataRowText == Convert.ToString(dtDonViTinh.Rows[z]["iID_MaDanhMuc"]))
                                {
                                    dataRowText = Convert.ToString(dtDonViTinh.Rows[z]["sTen"]);
                                    break;

                                }
                            }
                        }
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    }
                    if (dt.Columns[j].ColumnName == "sQuyCach")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sMoTa")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sMaCu")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sTenGoc")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sMoTaGoc")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sNhaSanXuat")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "sFileDinhKem")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    if (dt.Columns[j].ColumnName == "iID_MaDonVi")
                    {
                        if (dataRowText == "") dataRowText = "BQP";
                        else
                        {
                            for (z = 0; z < dtDonVi.Rows.Count; z++)
                            {
                                if (dataRowText == Convert.ToString(dtDonVi.Rows[z]["iID_MaDonVi"]))
                                {
                                    dataRowText = Convert.ToString(dtDonVi.Rows[z]["sTen"]);
                                    break;

                                }
                            }
                        }
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, dataRowText);
                    }
                }
                textOutput += "</tr>\r\n";
                Response.Write(textOutput);
            }

            textOutput = "</table>\r\n";
            Response.Write(textOutput);

            Response.Flush();
            Response.Close();
            Response.End();
        }

        public JavaScriptResult Edit_Fast_Submit(String ParentID, String OnSuccess,String MaDanhMuc,  String LoaiDanhMuc, String MaDiv, String MaDivDate)
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            String iID_MaTonKho = "";
            String iID_MaVatTu = "";
            String SoLuongTonKho = "0";
            if (LoaiDanhMuc != "")
            {
                String iDM_MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"];
                String iDM_MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String iDM_MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String sTenKhoa = Request.Form[ParentID + "_sTenKhoa"];

                Bang bang = new Bang("DC_DanhMuc");
                String iID_MaDanhMucCha = "";
                String iID_MaDanhMuc = "";
                switch (LoaiDanhMuc)
                {
                    case "NhomChinh":
                        iID_MaDanhMucCha = iDM_MaNhomLoaiVatTu;
                        break;

                    case "NhomPhu":
                        iID_MaDanhMucCha = iDM_MaNhomChinh;
                        break;

                    case "ChiTietVatTu":
                        iID_MaDanhMucCha = iDM_MaNhomPhu;
                        break;
                }

                //Kiểm tra trùng mã dữ liệu
                if (LoaiDanhMuc == "DonViTinh")
                    //Kiểm tra tùng tên
                    iID_MaDanhMuc = KiemTraTrungDuLieu(Request.Form[ParentID + "_sTen"], LoaiDanhMuc, iID_MaDanhMucCha);
                else
                    //Kiểm tra trùng mã danh mục
                    iID_MaDanhMuc = KiemTraTrungDuLieu(sTenKhoa, LoaiDanhMuc, iID_MaDanhMucCha);

                if (iID_MaDanhMuc != "" && iID_MaDanhMuc != MaDanhMuc)
                    //return RedirectToAction("Edit", new { MaDanhMuc = MaDanhMuc, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu, Loi = "1" });
                //------------

                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = MaDanhMuc;
                if (!String.IsNullOrEmpty(iID_MaDanhMucCha))
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iID_MaDanhMucCha);
                bang.Save();

                String strJ = "";
                if (String.IsNullOrEmpty(OnSuccess) == false)
                {
                    strJ = String.Format("Dialog_close('{0}');{1}('{2}','{3}');", ParentID, OnSuccess, Request.Form[ParentID + "_sTen"] + "#;" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), MaDiv + "#;" + MaDivDate);
                }
                else
                {
                    strJ = String.Format("Dialog_close('{0}');", ParentID);
                }
                return JavaScript(strJ);
            }
            return null;
        }

        public String KiemTraTrungDuLieu(String sTenKhoa, String LoaiDanhMuc, String iID_MaDanhMucCha)
        {
            String iID_MaDanhMuc = "";
            if (LoaiDanhMuc == "DonViTinh")
            {
                SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                    "WHERE sTen = @sTen AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = @LoaiDanhMuc)");
                cmd.Parameters.AddWithValue("@sTen", sTenKhoa);
                cmd.Parameters.AddWithValue("@LoaiDanhMuc", LoaiDanhMuc);
                iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            else
            {
                String DK = "";
                if (iID_MaDanhMucCha != "") DK = "AND iID_MaDanhMucCha = '" + iID_MaDanhMucCha + "' ";
                SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                    "WHERE sTenKhoa = @sTenKhoa " + DK +
                        "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                "FROM DC_LoaiDanhMuc " +
                                                "WHERE sTenBang = @LoaiDanhMuc)");
                cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
                cmd.Parameters.AddWithValue("@LoaiDanhMuc", LoaiDanhMuc);
                iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            return iID_MaDanhMuc;
        }

        public JavaScriptResult Edit_Gia_Fast_Submit(String ParentID, String OnSuccess, String sMaVatTu, String sID_MaNguoiDungSua, String IPSua, String MaDiv, String MaDivDate)
        {
            Double rGia = Convert.ToDouble(Request.Form[ParentID + "_rGia"]);
            String dTuNgay = Convert.ToString(Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"]);
            String dDenNgay = Convert.ToString(Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"]);
            if (sMaVatTu != "")
            {
                Bang bang = new Bang("DM_VatTu_Gia");
                bang.MaNguoiDungSua = sID_MaNguoiDungSua;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", sMaVatTu);
                bang.CmdParams.Parameters.AddWithValue("@rGia", rGia);
                bang.CmdParams.Parameters.AddWithValue("@dTuNgay", CommonFunction.LayNgayTuXau(dTuNgay));
                bang.CmdParams.Parameters.AddWithValue("@dDenNgay", CommonFunction.LayNgayTuXau(dDenNgay));
                bang.Save();

                String strJ = "";
                if (String.IsNullOrEmpty(OnSuccess) == false)
                {
                    strJ = String.Format("Dialog_close('{0}');{1}('{2}','{3}');", ParentID, OnSuccess, rGia + "#;" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), MaDiv + "#;" + MaDivDate);
                }
                else
                {
                    strJ = String.Format("Dialog_close('{0}');", ParentID);
                }
                return JavaScript(strJ);
            }
            return null;
        }
    }
}
