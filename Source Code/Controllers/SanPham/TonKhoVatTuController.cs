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


namespace VIETTEL.Controllers
{
    public class TonKhoVatTuController : Controller
    {
        //
        // GET: /TonKhoVatTu/
        public string sViewPath = "~/Views/SanPham/TonKhoVatTu/";
        public ActionResult Index(int? TonKhoVatTu_page, String Searchid, String sMaVatTu, String sTen, String sTenGoc, String sQuyCach, String cbsMaVatTu, String cbsTen, String cbsTenGoc, String cbsQuyCach, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iTrangThai)
        {
            ViewData["TonKhoVatTu_page"] = TonKhoVatTu_page;
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

                return RedirectToAction("Index", new { Searchid = Searchid, sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach, cbsMaVatTu = cbsMaVatTu, cbsTen = cbsTen, cbsTenGoc = cbsTenGoc, cbsQuyCach = cbsQuyCach});
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

        public class ThuocTinhVatTu
        {
            public Double SoLuongTonKho = 0;
            public String DonViTinh = "";
        }

        public JsonResult get_dtSoLuongTonKho(String ParentID, String iID_MaDonVi, String sMaVatTu)
        {
            return Json(get_objSoLuongTonKho(ParentID, iID_MaDonVi, sMaVatTu), JsonRequestBehavior.AllowGet);
        }

        public static ThuocTinhVatTu get_objSoLuongTonKho(String ParentID, String iID_MaDonVi, String sMaVatTu)
        {
            ThuocTinhVatTu _ThuocTinhVatTu = new ThuocTinhVatTu();
            String iID_MaVatTu = "";
            if (sMaVatTu != "")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT iID_MaVatTu, iDM_MaDonViTinh FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                    cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                    DataTable dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        iID_MaVatTu = Convert.ToString(dt.Rows[0]["iID_MaVatTu"]);
                        String iDM_MaDonViTinh = Convert.ToString(dt.Rows[0]["iDM_MaDonViTinh"]);

                        cmd = new SqlCommand("SELECT sTen FROM DC_DanhMuc WHERE iID_MaDanhMuc = @iDM_MaDonViTinh");
                        cmd.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
                        _ThuocTinhVatTu.DonViTinh = Convert.ToString(Connection.GetValueString(cmd, ""));
                        cmd.Dispose();

                        cmd = new SqlCommand("SELECT rSoLuongTonKho FROM DM_DonVi_TonKho " +
                            "WHERE iID_MaDonVi = @iID_MaDonVi AND iID_MaVatTu = @iID_MaVatTu");
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        _ThuocTinhVatTu.SoLuongTonKho= Convert.ToDouble(Connection.GetValueString(cmd, "0"));
                        cmd.Dispose();
                    }
                }
                catch { }
            }
            return _ThuocTinhVatTu;
        }

        public JsonResult get_dtLuuTonKho(String iID_MaDonVi, String sMaVatTu, String sID_MaNguoiDungSua, String IPSua, Double rSoLuongTonKho)
        {
            return Json(get_objLuuTonKho(iID_MaDonVi, sMaVatTu, sID_MaNguoiDungSua, IPSua, rSoLuongTonKho), JsonRequestBehavior.AllowGet);
        }

        public static String get_objLuuTonKho(String iID_MaDonVi, String sMaVatTu, String sID_MaNguoiDungSua, String IPSua, Double rSoLuongTonKho)
        {
            String iID_MaTonKho = "";
            String iID_MaVatTu = "";
            String tg = "0";
            String SoLuongTonKho = "0";
            if (sMaVatTu != "")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                    cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                    iID_MaVatTu = Convert.ToString(Connection.GetValueString(cmd, ""));
                    cmd.Dispose();

                    if (iID_MaVatTu != "")
                    {
                        cmd = new SqlCommand("SELECT iID_MaTonKho FROM DM_DonVi_TonKho " +
                            "WHERE iID_MaDonVi = @iID_MaDonVi AND iID_MaVatTu = @iID_MaVatTu");
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        iID_MaTonKho = Convert.ToString(Connection.GetValueString(cmd, ""));
                        cmd.Dispose();

                        Bang bang = new Bang("DM_DonVi_TonKho");
                        if (iID_MaTonKho != "")
                        {
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaTonKho;
                        }
                        else
                        {
                            bang.DuLieuMoi = true;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        }
                        bang.MaNguoiDungSua = sID_MaNguoiDungSua;
                        bang.IPSua = IPSua;
                        bang.CmdParams.Parameters.AddWithValue("@rSoLuongTonKho", rSoLuongTonKho);
                        bang.Save();

                        cmd = new SqlCommand("SELECT SUM(rSoLuongTonKho) FROM DM_DonVi_TonKho WHERE iID_MaVatTu = @iID_MaVatTu");
                        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        SoLuongTonKho = Convert.ToString(Connection.GetValueString(cmd, "0"));
                        cmd.Dispose();
                                                
                        cmd = new SqlCommand ("UPDATE DM_VatTu SET rSoLuongTonKho = @rSoLuongTonKho, " +
                                                        "dNgayCapNhatTonKho = @dNgayCapNhatTonKho " +
                                                        "WHERE iID_MaVatTu = @iID_MaVatTu");
                        cmd.Parameters.AddWithValue("@rSoLuongTonKho", SoLuongTonKho);
                        cmd.Parameters.AddWithValue("@dNgayCapNhatTonKho", DateTime.Now);
                        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();

                        tg = "1";
                    }
                }
                catch { }
            }
            return tg;
        }

        public JavaScriptResult Edit_Fast_Submit(String ParentID, String OnSuccess, String sMaVatTu, String iID_MaDonVi, String sID_MaNguoiDungSua, String IPSua, String MaDiv, String MaDivDate)
        {
            String str = Request.Form[ParentID + "_rSoLuongTonKhoMoi"];
            str = Request.Form[ParentID + "_rSoLuongTonKhoMoi_show"];
            Double rSoLuongTonKho = Convert.ToDouble(Request.Form[ParentID + "_rSoLuongTonKhoMoi_show"]);
            SqlCommand cmd;
            cmd = new SqlCommand();
            String iID_MaTonKho = "";
            String iID_MaVatTu = "";
            String SoLuongTonKho = "0";
            if (sMaVatTu != "")
            {
                cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                iID_MaVatTu = Convert.ToString(Connection.GetValueString(cmd, ""));
                cmd.Dispose();

                if (iID_MaVatTu != "")
                {
                    cmd = new SqlCommand("SELECT iID_MaTonKho FROM DM_DonVi_TonKho " +
                        "WHERE iID_MaDonVi = @iID_MaDonVi AND iID_MaVatTu = @iID_MaVatTu");
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                    iID_MaTonKho = Convert.ToString(Connection.GetValueString(cmd, ""));
                    cmd.Dispose();

                    Bang bang = new Bang("DM_DonVi_TonKho");
                    if (iID_MaTonKho != "")
                    {
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaTonKho;
                    }
                    else
                    {
                        bang.DuLieuMoi = true;
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    }
                    bang.MaNguoiDungSua = sID_MaNguoiDungSua;
                    bang.IPSua = IPSua;
                    bang.CmdParams.Parameters.AddWithValue("@rSoLuongTonKho", rSoLuongTonKho);
                    bang.Save();

                    cmd = new SqlCommand("SELECT SUM(rSoLuongTonKho) FROM DM_DonVi_TonKho WHERE iID_MaVatTu = @iID_MaVatTu");
                    cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                    SoLuongTonKho = Convert.ToString(Connection.GetValueString(cmd, "0"));
                    cmd.Dispose();

                    cmd = new SqlCommand("UPDATE DM_VatTu SET rSoLuongTonKho = @rSoLuongTonKho, " +
                                                    "dNgayCapNhatTonKho = @dNgayCapNhatTonKho " +
                                                    "WHERE iID_MaVatTu = @iID_MaVatTu");
                    cmd.Parameters.AddWithValue("@rSoLuongTonKho", SoLuongTonKho);
                    cmd.Parameters.AddWithValue("@dNgayCapNhatTonKho", DateTime.Now);
                    cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }

                String strJ = "";
                if (String.IsNullOrEmpty(OnSuccess) == false)
                {
                    strJ = String.Format("Dialog_close('{0}');{1}('{2}','{3}');", ParentID, OnSuccess, rSoLuongTonKho + "#;" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), MaDiv + "#;" + MaDivDate);
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
