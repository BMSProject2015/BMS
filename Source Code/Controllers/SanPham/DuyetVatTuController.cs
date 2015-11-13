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
using VIETTEL.Models;
namespace VIETTEL.Controllers
{
    public class DuyetVatTuController : Controller
    {
        //
        // GET: /DuyetVatTu/
        public string sViewPath = "~/Views/SanPham/DuyetVatTu/";
        public ActionResult Index(int? DuyetVatTu_page, int? TimKiemVatTu_page, String Searchid, String sMaVatTu, String sTen, String sTenGoc, String sQuyCach, String cbsMaVatTu, String cbsTen, String cbsTenGoc, String cbsQuyCach, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iTrangThai)
        { 
            ViewData["DuyetVatTu_page"] = DuyetVatTu_page;
            ViewData["TimKiemVatTu_page"] = TimKiemVatTu_page;
            ViewData["Searchid"] = Searchid;
            ViewData["sMaVatTu"] = sMaVatTu;
            ViewData["sTen"] = sTen;
            ViewData["sTenGoc"] = sTenGoc;
            ViewData["sQuyCach"] = sQuyCach;
            ViewData["cbsMaVatTu"] = cbsMaVatTu;
            ViewData["cbsTen"] = cbsTen;
            ViewData["cbsTenGoc"] = cbsTenGoc;
            ViewData["cbsQuyCach"] = cbsQuyCach;

            ViewData["MaNhomLoaiVatTu"] = MaNhomLoaiVatTu;
            ViewData["MaNhomChinh"] = MaNhomChinh;
            ViewData["MaNhomPhu"] = MaNhomPhu;
            ViewData["MaChiTietVatTu"] = MaChiTietVatTu;
            ViewData["MaXuatXu"] = MaXuatXu;
            ViewData["iTrangThai"] = iTrangThai;
            return View(sViewPath + "Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID, int Searchid)
        {
            if (Searchid == 1)
            {
                String sMaVatTu = Request.Form[ParentID + "_sMaVatTu"];
                String sTen = Request.Form[ParentID + "_sTen"];
                String sTenGoc = Request.Form[ParentID + "_sTenGoc"];
                String sQuyCach = Request.Form[ParentID + "_sQuyCach"];
                String cbsMaVatTu = Request.Form[ParentID + "_cbsMaVatTu"];
                String cbsTen = Request.Form[ParentID + "_cbsTen"];
                String cbsTenGoc = Request.Form[ParentID + "_cbsTenGoc"];
                String cbsQuyCach = Request.Form[ParentID + "_cbsQuyCach"];

                return RedirectToAction("Index", new {Searchid = Searchid,  sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach, cbsMaVatTu = cbsMaVatTu, cbsTen = cbsTen, cbsTenGoc = cbsTenGoc, cbsQuyCach = cbsQuyCach });
            }
            else
            {
                String iDM_MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"]; ;
                String iDM_MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String iDM_MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String iDM_MaChiTietVatTu = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
                String iDM_MaXuatXu = Request.Form[ParentID + "_iDM_MaXuatXu"];
                String iTrangThai = Request.Form[ParentID + "_iTrangThai"];
                return RedirectToAction("Index", new { Searchid = Searchid, MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, MaNhomChinh = iDM_MaNhomChinh, MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTu = iDM_MaChiTietVatTu, MaXuatXu = iDM_MaXuatXu, iTrangThai = iTrangThai});
            }
        }

        public ActionResult Edit(int? TimKiemVatTu_page, String iID_MaVatTu,
            String Searchid, String sMaVatTu_Search, String sTen_Search, String sTenGoc_Search, String sQuyCach_Search, String cbsMaVatTu_Search,
            String cbsTen_Search, String cbsTenGoc_Search, String cbsQuyCach_Search, String MaNhomLoaiVatTu_Search, String MaNhomChinh_Search,
            String MaNhomPhu_Search, String MaChiTietVatTu_Search, String MaXuatXu_Search, String iTrangThai_Search)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaVatTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaVatTu"] = iID_MaVatTu;

            ViewData["TimKiemVatTu_page"] = TimKiemVatTu_page;
            ViewData["Searchid"] = Searchid;
            ViewData["sMaVatTu_Search"] = sMaVatTu_Search;
            ViewData["sTen_Search"] = sTen_Search;
            ViewData["sTenGoc_Search"] = sTenGoc_Search;
            ViewData["sQuyCach_Search"] = sQuyCach_Search;
            ViewData["cbsMaVatTu_Search"] = cbsMaVatTu_Search;
            ViewData["cbsTen_Search"] = cbsTen_Search;
            ViewData["cbsTenGoc_Search"] = cbsTenGoc_Search;
            ViewData["cbsQuyCach_Search"] = cbsQuyCach_Search;

            ViewData["MaNhomLoaiVatTu_Search"] = MaNhomLoaiVatTu_Search;
            ViewData["MaNhomChinh_Search"] = MaNhomChinh_Search;
            ViewData["MaNhomPhu_Search"] = MaNhomPhu_Search;
            ViewData["MaChiTietVatTu_Search"] = MaChiTietVatTu_Search;
            ViewData["MaXuatXu_Search"] = MaXuatXu_Search;
            ViewData["iTrangThai_Search"] = iTrangThai_Search;
            return View(sViewPath + "Edit.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaVatTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_VatTu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String DoiTrangThai = Request.Form[ParentID + "_DoiTrangThai"];
            NameValueCollection arrLoi = new NameValueCollection();
            String iDM_MaNhomLoaiVatTu = Convert.ToString(Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"]);
            String iDM_MaNhomChinh = Convert.ToString(Request.Form[ParentID + "_iDM_MaNhomChinh"]);
            String iDM_MaNhomPhu = Convert.ToString(Request.Form[ParentID + "_iDM_MaNhomPhu"]);
            String iDM_MaChiTietVatTu = Convert.ToString(Request.Form[ParentID + "_iDM_MaChiTietVatTu"]);
            String iDM_MaXuatXu = Convert.ToString(Request.Form[ParentID + "_iDM_MaXuatXu"]);
            String sMaVatTu = Convert.ToString(Request.Form[ParentID + "_sMaVatTu"]);
            String sMaYeuCau = Convert.ToString(Request.Form[ParentID + "_sMaYeuCau"]);
            String TenVatTu = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String DonViTinh = Convert.ToString(Request.Form[ParentID + "_iDM_MaDonViTinh"]);
            String sCapMaCu = Convert.ToString(Request.Form[ParentID + "_sCapMaCu"]);
            String iID_sTenKhoa = Convert.ToString(Request.Form[ParentID + "_sTenKhoa"]);
            if (Convert.ToInt32(DoiTrangThai) == 5)
            {
                if (sCapMaCu == string.Empty || sCapMaCu == "")
                {
                    arrLoi.Add("err_sCapMaCu", "Chưa có mã vật tư cũ để cấp!");
                }
            }
            if (Convert.ToInt32(DoiTrangThai) != 3 && Convert.ToInt32(DoiTrangThai) != 5)
            {            
                if (iDM_MaNhomLoaiVatTu == string.Empty || iDM_MaNhomLoaiVatTu == "")
                {
                    arrLoi.Add("err_iDM_MaNhomLoaiVatTu", "Chưa có mã nhóm loại vật tư!");
                }
                if (iDM_MaNhomChinh == string.Empty || iDM_MaNhomChinh == "")
                {
                    arrLoi.Add("err_iDM_MaNhomChinh", "Chưa có mã nhóm chính vật tư!");
                }
                if (iDM_MaNhomPhu == string.Empty || iDM_MaNhomPhu == "")
                {
                    arrLoi.Add("err_iDM_MaNhomPhu", "Chưa có mã nhóm phụ vật tư!");
                }
                if (iID_sTenKhoa == string.Empty || iID_sTenKhoa == "")
                {
                    arrLoi.Add("err_iDM_MaChiTietVatTu", "Chưa có mã chi tiết vật tư!");
                }
                if (iDM_MaXuatXu == string.Empty || iDM_MaXuatXu == "")
                {
                    arrLoi.Add("err_iDM_MaXuatXu", "Bạn chưa chọn tình trạng vật tư!");
                }
                if (sMaVatTu == string.Empty || sMaVatTu == "")
                {
                    arrLoi.Add("err_sMaVatTu", "Chưa có mã vật tư!");
                }
                if (sMaVatTu.ToString().Length < 12 || sMaVatTu.ToString().Length > 12)
                {
                    arrLoi.Add("err_sMaVatTu", "Mã vật tư phải có 12 ký tự!");
                }
                if (TenVatTu == string.Empty || TenVatTu == "")
                {
                    arrLoi.Add("err_sTen", "Bạn chưa nhập tên vật tư!");
                }
                if (DonViTinh == string.Empty || DonViTinh == "dddddddd-dddd-dddd-dddd-dddddddddddd")
                {
                    arrLoi.Add("err_iDM_MaDonViTinh", "Bạn chưa chọn đơn vị tính!");
                }
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaVatTu"] = iID_MaVatTu;
                ViewData["iDM_MaNhomLoaiVatTu"] = iDM_MaNhomLoaiVatTu;
                ViewData["iDM_MaNhomChinh"] = iDM_MaNhomChinh;
                ViewData["iDM_MaNhomPhu"] = iDM_MaNhomPhu;
                ViewData["iDM_MaChiTietVatTu"] = iDM_MaChiTietVatTu;
                ViewData["iDM_MaXuatXu"] = iDM_MaXuatXu;
                ViewData["TenVatTu"] = TenVatTu;
                ViewData["DonViTinh"] = DonViTinh;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                SqlCommand cmd;
                String MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"];
                String MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String MaChiTietVatTu = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
                String MaXuatXu = Request.Form[ParentID + "_iDM_MaXuatXu"];
                String sTenKhoa = Convert.ToString(Request.Form[ParentID + "_sTenKhoa"]);

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
                    if (sTenKhoa.Length == 5)
                    {
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
                        bangdm.CmdParams.Parameters.AddWithValue("@bDangDung", 1);
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
                else
                {
                    bang.CmdParams.Parameters["@iDM_MaChiTietVatTu"].Value = MaChiTietVatTu;
                }
                if (String.IsNullOrEmpty(MaXuatXu) && bang.CmdParams.Parameters.IndexOf("@iDM_MaXuatXu") >= 0)
                    bang.CmdParams.Parameters["@iDM_MaXuatXu"].Value = DBNull.Value;
                String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
                if (String.IsNullOrEmpty(iID_MaDonVi) && bang.CmdParams.Parameters.IndexOf("@iID_MaDonVi") >= 0)
                    bang.CmdParams.Parameters["@iID_MaDonVi"].Value = DBNull.Value;
                bang.DuLieuMoi = false;
                
                bang.CmdParams.Parameters.AddWithValue("@iTrangThai", DoiTrangThai);

                if (!bang.DuLieuMoi)
                    bang.GiaTriKhoa = iID_MaVatTu;
                bang.Save();

                //Update trạng thái đang dùng của mã chi tiết vật tư
                if (Convert.ToInt32(DoiTrangThai) == 1){
                    cmd = new SqlCommand("UPDATE DC_DanhMuc SET bDangDung = 1 WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMuc", MaChiTietVatTu);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }

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
                if (String.IsNullOrEmpty(MaChiTietVatTu) && bang.CmdParams.Parameters.IndexOf("@iDM_MaChiTietVatTu") >= 0)
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
                    bangls.CmdParams.Parameters.AddWithValue("@iTrangThai", DoiTrangThai);
                bangls.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", bang.GiaTriKhoa);
                String dNgayPhatSinhMa = Request.Form[ParentID + "_NgayPhatSinhMa"];
                String[] tg = dNgayPhatSinhMa.Split('/');
                dNgayPhatSinhMa = tg[1] + "/" + tg[0] + "/" + tg[2];
                bangls.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", dNgayPhatSinhMa);
                if (DoiTrangThai == "3")
                    bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", 4);//Từ chối
                else if (DoiTrangThai == "1")
                    bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", 3);//Duyệt
                else if (DoiTrangThai == "4")
                    bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", 7);//Gửi BQP
                bangls.Save();


                String sTenNhomLoaiVattu = Convert.ToString(Request.Form[ParentID + "_MaNhomLoaiVatTu"]);
                String sTenNhomChinh = Convert.ToString(Request.Form[ParentID + "_MaNhomChinh"]);
                String sTenNhomPhu = Convert.ToString(Request.Form[ParentID + "_MaNhomPhu"]);
                String[] tgsTenNhomLoaiVattu = sTenNhomLoaiVattu.Split('_');
                sTenNhomLoaiVattu = tgsTenNhomLoaiVattu[0];
                String[] tgsTenNhomChinh = sTenNhomChinh.Split('_');
                sTenNhomChinh = tgsTenNhomChinh[0];
                String[] tgsTenNhomPhu = sTenNhomPhu.Split('_');
                sTenNhomPhu = tgsTenNhomPhu[0];
                String MaNhom = sTenNhomLoaiVattu + sTenNhomChinh + sTenNhomPhu;
                String sTenGoc = Convert.ToString(Request.Form[ParentID + "_sTenGoc"]);
                String sTenDonViTinh = Convert.ToString(Request.Form[ParentID + "_MaDonViTinh"]);
                String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
                String sGhiChu = Convert.ToString(Request.Form[ParentID + "_sGhiChu"]);
                String sNhaSX = Convert.ToString(Request.Form[ParentID + "_sNhaSanXuat"]);
                String sMoTaGoc = Convert.ToString(Request.Form[ParentID + "_sMoTaGoc"]);
                String sFileDinhKem = Convert.ToString(Request.Form[ParentID + "_sFileDinhKem"]);
                String rSoLuongTonKho = Convert.ToString(Request.Form[ParentID + "_rSoLuongTonKho"]);
                String sQuyCach = Convert.ToString(Request.Form[ParentID + "_sQuyCach"]);
                String dNgayCapNhatTonKho = Convert.ToString(Request.Form[ParentID + "_dNgayCapNhatTonKho"]);
                String sLyDo = Convert.ToString(Request.Form[ParentID + "_sLyDo"]);
                if (dNgayCapNhatTonKho != null && dNgayCapNhatTonKho != "")
                {
                    String[] tg1 = dNgayCapNhatTonKho.Split('/');
                    dNgayCapNhatTonKho = tg1[1] + "/" + tg1[0] + "/" + tg1[2];
                }
                String sMaCu = Convert.ToString(Request.Form[ParentID + "_sMaCu"]);

                if (iID_MaDonVi != "" && Convert.ToInt32(iID_MaDonVi) == 5)
                {
                    int iHanhDong = 0;
                    switch (DoiTrangThai)
                    {
                        case "1":
                            iHanhDong = 3;
                            break;

                        case "3":
                            iHanhDong = 4;
                            break;

                        case "4":
                            iHanhDong = 7;
                            break;

                        case "5":
                            iHanhDong = 5;
                            break;
                    }
                }

                return RedirectToAction("Index", "DuyetVatTu");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search_Search(String ParentID, int Searchid, String iID_MaVatTu)
        {
            if (Searchid == 1)
            {
                String sMaVatTu_Search = Request.Form[ParentID + "_sMaVatTu_Search"];
                String sTen_Search = Request.Form[ParentID + "_sTen_Search"];
                String sTenGoc_Search = Request.Form[ParentID + "_sTenGoc_Search"];
                String sQuyCach_Search = Request.Form[ParentID + "_sQuyCach_Search"];
                String cbsMaVatTu_Search = Request.Form[ParentID + "_cbsMaVatTu_Search"];
                String cbsTen_Search = Request.Form[ParentID + "_cbsTen_Search"];
                String cbsTenGoc_Search = Request.Form[ParentID + "_cbsTenGoc_Search"];
                String cbsQuyCach_Search = Request.Form[ParentID + "_cbsQuyCach_Search"];

                return RedirectToAction("Edit", new
                {
                    iID_MaVatTu = iID_MaVatTu,
                    Searchid = Searchid,
                    sMaVatTu_Search = sMaVatTu_Search,
                    sTen_Search = sTen_Search,
                    sTenGoc_Search = sTenGoc_Search,
                    sQuyCach_Search = sQuyCach_Search,
                    cbsMaVatTu_Search = cbsMaVatTu_Search,
                    cbsTen_Search = cbsTen_Search,
                    cbsTenGoc_Search = cbsTenGoc_Search,
                    cbsQuyCach_Search = cbsQuyCach_Search
                });
            }
            else
            {
                String iDM_MaNhomLoaiVatTu_Search = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu_Search"]; ;
                String iDM_MaNhomChinh_Search = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String iDM_MaNhomPhu_Search = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String iDM_MaChiTietVatTu_Search = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
                String iDM_MaXuatXu_Search = Request.Form[ParentID + "_iDM_MaXuatXu_Search"];
                String iTrangThai_Search = Request.Form[ParentID + "_iTrangThai_Search"];
                return RedirectToAction("Edit", new
                {
                    iID_MaVatTu = iID_MaVatTu,
                    Searchid = Searchid,
                    MaNhomLoaiVatTu_Search = iDM_MaNhomLoaiVatTu_Search,
                    MaNhomChinh_Search = iDM_MaNhomChinh_Search,
                    MaNhomPhu_Search = iDM_MaNhomPhu_Search,
                    MaChiTietVatTu_Search = iDM_MaChiTietVatTu_Search,
                    MaXuatXu_Search = iDM_MaXuatXu_Search,
                    iTrangThai_Search = iTrangThai_Search
                });
            }
        }
    }
}
