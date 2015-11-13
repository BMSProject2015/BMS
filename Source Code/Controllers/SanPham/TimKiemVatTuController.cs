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
    public class TimKiemVatTuController : Controller
    {
        //
        // GET: /TimKiemVatTu/
        public string sViewPath = "~/Views/SanPham/TimKiemVatTu/";
        [Authorize]
        public ActionResult Index(int? TimKiemVatTu_page, String sMaVatTu, String sTen, String sTenGoc, String sQuyCach, String iTrangThai, String iID_MaDonVi, String TuNgay, String DenNgay, String cbsMaVatTu, String cbsTen, String cbsTenGoc, String cbsQuyCach, String HienThiTaoMoi)
        {
            ViewData["TimKiemVatTu_page"] = TimKiemVatTu_page;
            ViewData["sMaVatTu"] = sMaVatTu;
            ViewData["sTen"] = sTen;
            ViewData["sTenGoc"] = sTenGoc;
            ViewData["sQuyCach"] = sQuyCach;
            ViewData["iTrangThai"] = iTrangThai;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["TuNgay"] = TuNgay;
            ViewData["DenNgay"] = DenNgay;
            ViewData["cbsMaVatTu"] = cbsMaVatTu;
            ViewData["cbsTen"] = cbsTen;
            ViewData["cbsTenGoc"] = cbsTenGoc;
            ViewData["cbsQuyCach"] = cbsQuyCach;
            ViewData["HienThiTaoMoi"] = HienThiTaoMoi;
            return View(sViewPath + "Index.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaVatTu, String sMaVatTu, String sTen, String sTenGoc, String sQuyCach)
        {
            ViewData["sTen"] = sTen;
            ViewData["sTenGoc"] = sTenGoc;
            ViewData["sQuyCach"] = sQuyCach;

            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaVatTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaVatTu"] = iID_MaVatTu;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
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
            if (sMaVatTu != string.Empty || sMaVatTu != "")
            {
                cmd = new SqlCommand();
                cmd.CommandText = "SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu=@sMaVatTu";
                cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                DataTable dtVatTu = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dtVatTu.Rows.Count > 0)
                {
                    arrLoi.Add("err_sMaVatTu", "Trùng mã vật tư!");
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
                if (String.IsNullOrEmpty(Request.Form[ParentID + "_iTrangThai"]))
                {
                    if (iTrangThai == "3") iTrangThai = "2";
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
                dNgayPhatSinhMa = tg[1] + "/" + tg[0] + "/" + tg[2];
                bang.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", dNgayPhatSinhMa);
                bang.Save();


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
                    bangls.CmdParams.Parameters.AddWithValue("@iTrangThai", iTrangThai);
                bangls.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", bang.GiaTriKhoa);
                bangls.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", dNgayPhatSinhMa);
                bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", iHanhDong);
                bangls.Save();

                return RedirectToAction("Index", "MaVatTu", new { MaNhomLoaiVatTu = MaNhomLoaiVatTu, MaNhomChinh = MaNhomChinh, MaNhomPhu = MaNhomPhu, MaChiTietVatTu = MaChiTietVatTu, MaXuatXu = MaXuatXu, iTrangThai = iTrangThai });
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID, String HienThiTaoMoi)
        {
            String sMaVatTu = Request.Form[ParentID + "_sMaVatTu"];
            String sTen = Request.Form[ParentID + "_sTen"];
            String sTenGoc = Request.Form[ParentID + "_sTenGoc"];
            String sQuyCach = Request.Form[ParentID + "_sQuyCach"];
            String iTrangThai = Request.Form[ParentID + "_iTrangThai"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String TuNgay = Request.Form[ParentID +"_"+ NgonNgu.MaDate + "TuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "DenNgay"];
            String cbsMaVatTu = Request.Form[ParentID + "_cbsMaVatTu"];
            String cbsTen = Request.Form[ParentID + "_cbsTen"];
            String cbsTenGoc = Request.Form[ParentID + "_cbsTenGoc"];
            String cbsQuyCach = Request.Form[ParentID + "_cbsQuyCach"];

            return RedirectToAction("Index", new { sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach, iTrangThai = iTrangThai, iID_MaDonVi = iID_MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay, cbsMaVatTu = cbsMaVatTu, cbsTen = cbsTen, cbsTenGoc = cbsTenGoc, cbsQuyCach = cbsQuyCach, HienThiTaoMoi = HienThiTaoMoi });
        }

        [Authorize]
        public ActionResult Detail(String iID_MaVatTu)
        {
            ViewData["iID_MaVatTu"] = iID_MaVatTu;
            return View(sViewPath + "Detail.aspx");
        }

        [Authorize]
        public ActionResult XemTonKho(String iID_MaVatTu, String sMaVatTu, String SoLuongTonKho, String DonViTinh)
        {
            ViewData["iID_MaVatTu"] = iID_MaVatTu;
            ViewData["sMaVatTu"] = sMaVatTu;
            ViewData["SoLuongTonKho"] = SoLuongTonKho;
            ViewData["DonViTinh"] = DonViTinh;
            return View(sViewPath + "XemTonKho.aspx");
        }

        [Authorize]
        public ActionResult XemNCC(int? VTNCC_page, String iID_MaVatTu, String sMaVatTu, String sTen)
        {
            ViewData["VTNCC_page"] = VTNCC_page;
            ViewData["iID_MaVatTu"] = iID_MaVatTu;
            ViewData["sMaVatTu"] = sMaVatTu;
            ViewData["sTen"] = sTen;
            return View(sViewPath + "XemNCC.aspx");
        }

        [Authorize]
        public ActionResult ExportExcel(String SQL, String iID_MaVatTu, String sMaVatTu, String SoLuongTonKho, String DonViTinh)
        {
            Export(SQL, sMaVatTu, DonViTinh);
            return RedirectToAction("XemTonKho", new { iID_MaVatTu = iID_MaVatTu, sMaVatTu = sMaVatTu, SoLuongTonKho = SoLuongTonKho, DonViTinh = DonViTinh });
        }

        public void Export(String SQL, String sMaVatTu, String DonViTinh)
        {
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt == null) return;

            cmd = new SqlCommand("SELECT * FROM NS_DonVi ORDER BY iID_MaDonVi");
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i, j;
            Response.BufferOutput = false;
            Response.Charset = string.Empty;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            string attachment = "attachment; filename=TonKho_" + sMaVatTu + ".xls";
            Response.AddHeader("content-disposition", attachment);
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());

            string textOutput =
            "<table border='1' bordercolor='black' rules='all'>\r\n";
            Response.Write(textOutput);

            textOutput = "<tr height=17 style='height:12.75pt'>\r\n";
            string sTitle = "<th align='left'>{0}</th>";
            if (dt == null) return;
            for (i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "iID_MaDonVi")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Đơn vị");
                if (dt.Columns[i].ColumnName == "rSoLuongTonKho")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Số lượng tồn kho");
                if (dt.Columns[i].ColumnName == "dNgaySua")
                    textOutput += string.Format(CultureInfo.InvariantCulture, sTitle, "Ngày cập nhật tồn kho");
            }
            textOutput += "</tr>\r\n";
            Response.Write(textOutput);

            textOutput = string.Empty;

            DateTime dNgayCapNhatTonKho = DateTime.Now;
            String TenDonVi = "";
            String SoLuongTonKho = "0";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = dt.Rows[i];
                textOutput = "<tr height=17 style='height:12.75pt'>\r\n";

                SoLuongTonKho = CommonFunction.DinhDangSo(dt.Rows[i]["rSoLuongTonKho"]);
                dNgayCapNhatTonKho = Convert.ToDateTime(dt.Rows[i]["dNgaySua"]);
                if (Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == "") TenDonVi = "BQP";
                else
                {
                    for (j = 0; j < dtDonVi.Rows.Count; j++)
                    {
                        if (Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == Convert.ToString(dtDonVi.Rows[j]["iID_MaDonVi"]))
                        {
                            TenDonVi = Convert.ToString(dtDonVi.Rows[j]["sTen"]);
                            break;
                        }
                    }
                }

                for (j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    string sContent = "<td align='left' style='mso-number-format:\\@;'>{0}</td>";

                    if (dt.Columns[j].ColumnName == "iID_MaDonVi")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, TenDonVi);
                    if (dt.Columns[j].ColumnName == "rSoLuongTonKho")
                        textOutput += string.Format(CultureInfo.InvariantCulture, sContent, SoLuongTonKho + ' ' + DonViTinh);
                    if (dt.Columns[j].ColumnName == "dNgaySua")
                    {
                        try
                        {
                            textOutput += string.Format(CultureInfo.InvariantCulture, sContent, String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho));
                        }
                        catch
                        {
                            textOutput += string.Format(CultureInfo.InvariantCulture, sContent, string.Empty);
                        }
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
    }
}
