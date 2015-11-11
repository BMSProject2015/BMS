using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    public class ImportExcelGiaSPController : Controller
    {
        //
        // GET: /ImportExcelVatTu/
        public string sViewPath = "~/Views/SanPham/ImportExcelGiaSP/";
        public static DataTable dtImportResult;
        public static int CountRecord = 0;
        public static String ResultSheetName = "", sLoi = "";

        public ActionResult Index(String iID_MaSanPham)
        {
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            return View(sViewPath + "Index.aspx");
        }

        public ActionResult Import()
        {
            return View(sViewPath + "Import.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Load(String ParentID, String iID_MaSanPham)
        {
            String FileName = Request.Form[ParentID + "_sFileName"];
            String FilePath = Server.MapPath("~/" + Request.Form[ParentID + "_sDuongDan"] + "");
            String KieuNhap = Request.Form[ParentID + "_KieuNhap"];
            String iID_MaLoaiHinh = Request.Form[ParentID + "_iID_MaLoaiHinh"];
            int isError = 0;
            try
            {
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
                conStr = String.Format(conStr, FilePath);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                DataTable dt = new DataTable();
                cmdExcel.CommandText = "SELECT * FROM [ChiTietGia$]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                if ((KieuNhap == "1" && dt.Columns.Count != 7) || (KieuNhap == "2" && dt.Columns.Count != 18))
                {
                    isError = 1;
                }
            }
            catch (System.Exception ex)
            {
                isError = 1;
            }
            return RedirectToAction("Import", new { iID_MaSanPham = iID_MaSanPham, FileName = FileName, FilePath = FilePath, KieuNhap = KieuNhap, iID_MaLoaiHinh = iID_MaLoaiHinh, isError = isError });
        }

        public class SheetData
        {
            public string sData { get; set; }
        }

        public JsonResult get_dtSheet(String ParentID, String FilePath, String SheetName,String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            return Json(get_objSheet(ParentID, FilePath, SheetName,iID_MaSanPham, iID_MaLoaiHinh), JsonRequestBehavior.AllowGet);
        }

        public static SheetData get_objSheet(String ParentID, String FilePath, String SheetName,String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            String vR = string.Empty;
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
            conStr = String.Format(conStr, FilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            DataTable dt = new DataTable();
            SheetData data = new SheetData();
            if (SheetName == string.Empty)
            {
                data.sData = string.Empty;
                return data;
            }

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            vR += "<table cellpadding='0' cellspacing='0' border='0' class='table_form3'>";
            if (SheetName == "SanPham$")
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        vR += "<tr>";
                        vR += "<td align='right' class='td_form3_td1' style='width: 30%;'><b>" + dt.Columns[i].ColumnName + ":</b></td>";
                        DataRow dataRow = dt.Rows[0];
                        string dataRowText = dataRow[i].ToString().Trim();
                        if (i == 0)
                        {//Cột loại sửa chữa
                            if (dataRowText == "1") dataRowText = "Sửa chữa lớn";
                            else if (dataRowText == "2") dataRowText = "Sửa chữa vừa";
                            else if (dataRowText == "3") dataRowText = "Sửa chữa nhỏ";
                        }
                        vR += "<td style='width: 70%;'>";
                        vR += MyHtmlHelper.Label(dataRowText, dt.Columns[i].ColumnName);
                        vR += "</td>";
                        vR += "</tr>";
                    }
                }
            }
            else
            {
                vR += "<tr class='tr_form3'>";
                vR += "<td style='width: 10px;display:none' align='center'>";
                vR += "<b>";
                vR += "<input type='checkbox' id='checkall' onclick='setCheckboxes();' checked='checked'>";
                vR += "</b>";
                vR += "</td>";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 2; i < dt.Columns.Count; i++)
                    {
                        vR += "<td>";
                        vR += "<b>";
                        vR += dt.Columns[i].ColumnName;
                        vR += "</b>";
                        vR += "</td>";
                    }
                    vR += "</tr>";
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dataRow = dt.Rows[i];
                        String iID_MaDanhMucGia = "0"; //kiem tra cac danh muc import co trong cau hinh ko
                        if (iID_MaSanPham != "") iID_MaDanhMucGia = getIdCauHinh(Convert.ToString(dataRow[3]), iID_MaSanPham, iID_MaLoaiHinh);
                        String HighLight = "";
                        if (iID_MaDanhMucGia == "0") HighLight = " style = 'color:Red'";
                        vR += "<tr>";
                        vR += "<td style='width: 10px;display:none' align='center'>";
                        vR += "<input type='checkbox' name='" + ParentID + "_checkGroup" + "' checked='checked' value='" + i + "'>";
                        vR += "</td>";
                        for (int j = 2; j < dataRow.Table.Columns.Count; j++)
                        {
                            string dataRowText = dataRow[j].ToString().Trim();
                            vR += "<td" + HighLight + ">";
                            vR += MyHtmlHelper.Label(dataRowText, dt.Columns[j].ColumnName);
                            vR += "</td>";
                        }
                        vR += "</tr>";
                    }
                    dt.Dispose();
                }
            }
            vR += "</table>";
            data.sData = vR;
            return data;
        }
        private String ImportDonViTinh(String TenDonVi)
        {
            try
            {
                String iDM_MaDonViTinh = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                if (!String.IsNullOrEmpty(TenDonVi))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 iID_MaDanhMuc FROM DC_DanhMuc " +
                                        "WHERE bHoatDong = 1 AND LTRIM(RTRIM(sTen)) = LTRIM(RTRIM(@sTen)) " +
                                        "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'DonViTinh')");
                    cmd.Parameters.AddWithValue("@sTen", TenDonVi);
                    iDM_MaDonViTinh = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    String iID_MaLoaiDanhMuc = Connection.GetValueString(new SqlCommand("SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'DonViTinh'"), "");
                    if (iDM_MaDonViTinh == "")
                    {
                        //Chua co don vi nay thi nhap vao
                        Bang bang = new Bang("DC_DanhMuc");
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.DuLieuMoi = true;
                        bang.CmdParams.Parameters.AddWithValue("@sTen", TenDonVi);
                        bang.CmdParams.Parameters.AddWithValue("@bHoatDong", true);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
                        iDM_MaDonViTinh = bang.Save().ToString();
                    }
                }
                return iDM_MaDonViTinh;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        private String ImportSanPham(String FileName, String FilePath)
        {
            try
            {
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
                conStr = String.Format(conStr, FilePath);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                connExcel.Open();

                DataTable dt = new DataTable();
                cmdExcel.CommandText = "SELECT * FROM [SanPham$]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                String iID_MaSanPham = "";
                String[] arrTenTruong = "iID_MaLoaiHinh,sMa,sTen,rSoLuong,iDM_MaDonViTinh,sQuyCach".Split(',');
                dtImportResult = dt.Clone();
                ResultSheetName = "SanPham$";

                if (dt.Rows.Count > 0)
                {
                    DataRow Row = dt.Rows[0];
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 iID_MaSanPham FROM DM_SanPham " +
                                    "WHERE iTrangThai = 1 AND LTRIM(RTRIM(sTen)) = LTRIM(RTRIM(@sTen)) AND LTRIM(RTRIM(sMa)) = LTRIM(RTRIM(@sMa))");
                    cmd.Parameters.AddWithValue("@sMa", Row[1]);
                    cmd.Parameters.AddWithValue("@sTen", Row[2]);
                    iID_MaSanPham = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iID_MaSanPham == "")
                    {
                        Bang bang = new Bang("DM_SanPham");
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        for (int i = 1; i < Row.Table.Columns.Count; i++)
                        {
                            string TenTruong = arrTenTruong[i];
                            string DuLieu = Row[i].ToString().Trim();
                            if (TenTruong == "iDM_MaDonViTinh")
                            {
                                if (!String.IsNullOrEmpty(DuLieu))
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, ImportDonViTinh(DuLieu));
                                }
                                else
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, "dddddddd-dddd-dddd-dddd-dddddddddddd");
                                }
                            }
                            else
                            {
                                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                            }
                        }
                        bang.DuLieuMoi = true;
                        iID_MaSanPham = bang.Save().ToString();
                    }
                }
                return iID_MaSanPham;
            }
            catch (System.Exception ex)
            {
                return null;
            } 
        }
        private String TaoPhieuChiTietGiaSanPham(String FileName, String FilePath, String iID_MaSanPham)
        {
            try
            {
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
                conStr = String.Format(conStr, FilePath);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                connExcel.Open();

                DataTable dt = new DataTable();
                cmdExcel.CommandText = "SELECT * FROM [SanPham$]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                String[] arrTenTruong = "iID_MaLoaiHinh,sMa,sTen,rSoLuong,iDM_MaDonViTinh,sQuyCach".Split(',');
                dtImportResult = dt.Clone();
                ResultSheetName = "SanPham$";
                if (dt.Rows.Count > 0)
                {
                    DataRow Row = dt.Rows[0];
                    Bang bang = new Bang("DM_SanPham_ChiTietGia");
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                        for (int i = 0; i < Row.Table.Columns.Count; i++)
                        {
                            string TenTruong = arrTenTruong[i];
                            string DuLieu = Row[i].ToString().Trim();
                            if (TenTruong == "iDM_MaDonViTinh")
                            {
                                if (!String.IsNullOrEmpty(DuLieu))
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, ImportDonViTinh(DuLieu));
                                }
                                else
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, "dddddddd-dddd-dddd-dddd-dddddddddddd");
                                }
                            }
                            else
                            {
                                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                            }
                        }
                        bang.DuLieuMoi = true;
                        bang.Save();
                }
                return SanPham_DanhMucGiaModels.Get_MaxId_ChiTietGia();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        private String ImportVatTu(String sTen, String iDM_MaDonViTinh, String DonGia, String bNganSach)
        {
            try
            {
                if (String.IsNullOrEmpty(sTen))
                {
                    return "dddddddd-dddd-dddd-dddd-dddddddddddd";
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 iID_MaVatTu FROM DM_VatTu " +
                                        "WHERE iTrangThai = 1 AND LTRIM(RTRIM(sTen)) = LTRIM(RTRIM(@sTen)) " +
                                        "ORDER BY dNgaySua DESC");
                    cmd.Parameters.AddWithValue("@sTen", sTen);
                    String iID_MaVatTu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iID_MaVatTu == "")
                    {
                        //Chua co vat tu nay thi nhap vao
                        String iDM_MaNhomLoaiVatTu = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        String iDM_MaNhomChinh = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        String iDM_MaNhomPhu = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        String iDM_MaChiTietVatTu = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        String iDM_MaXuatXu = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        String sMaVatTu = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        DataTable dt = Connection.GetDataTable(new SqlCommand("SELECT TOP 1 * FROM DM_VatTu WHERE iTrangThai = 1 ORDER BY dNgayTao DESC"));
                        if (dt.Rows.Count > 0)
                        {
                            iDM_MaNhomLoaiVatTu = dt.Rows[0]["iDM_MaNhomLoaiVatTu"].ToString();
                            iDM_MaNhomChinh = dt.Rows[0]["iDM_MaNhomChinh"].ToString();
                            iDM_MaNhomPhu = dt.Rows[0]["iDM_MaNhomPhu"].ToString();
                            iDM_MaChiTietVatTu = dt.Rows[0]["iDM_MaChiTietVatTu"].ToString();
                            iDM_MaXuatXu = dt.Rows[0]["iDM_MaXuatXu"].ToString();
                            sMaVatTu = dt.Rows[0]["sMaVatTu"].ToString();
                            if (sMaVatTu.Length == 12)
                            {
                                String subStr = sMaVatTu.Substring(6, 5);
                                int subInt = int.Parse(subStr) + 1;
                                sMaVatTu = sMaVatTu.Substring(0, 6) + String.Format("{0:00000}", subInt) + sMaVatTu.Substring(11, 1);
                            }
                        }

                        Bang bang = new Bang("DM_VatTu");
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.DuLieuMoi = true;
                        bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);
                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", iDM_MaNhomLoaiVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", iDM_MaNhomChinh);
                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);
                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", iDM_MaChiTietVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", iDM_MaXuatXu);
                        bang.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                        bang.Save();
                        iID_MaVatTu = Convert.ToString(bang.GiaTriKhoa);
                    }
                    if (!String.IsNullOrEmpty(DonGia))
                    { //nhap gia vat tu
                        Bang bangGia = new Bang("DM_VatTu_Gia");
                        bangGia.MaNguoiDungSua = User.Identity.Name;
                        bangGia.IPSua = Request.UserHostAddress;
                        bangGia.DuLieuMoi = true;
                        bangGia.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                        bangGia.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);
                        bangGia.CmdParams.Parameters.AddWithValue("@dTuNgay", DateTime.Now);
                        if (bNganSach == "1")
                        {
                            bangGia.CmdParams.Parameters.AddWithValue("@rGia", 0);
                            bangGia.CmdParams.Parameters.AddWithValue("@rGia_NS", DonGia);
                        }
                        else
                        {
                            bangGia.CmdParams.Parameters.AddWithValue("@rGia", DonGia);
                            bangGia.CmdParams.Parameters.AddWithValue("@rGia_NS", 0);
                        }
                        bangGia.Save();
                    }
                    return iID_MaVatTu;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        private void ImportCauHinhDanhMuc(String FileName, String FilePath, String iID_MaSanPham)
        {
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
            conStr = String.Format(conStr, FilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            DataTable dt = new DataTable();
            cmdExcel.CommandText = "SELECT * FROM [ChiTietGia$]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            String strTenTuong = "";
            strTenTuong = "iID_MaDanhMucGia_Cha,sKyHieu,sTen,sTen_DonVi,rSoLuong,rDonGia"; // voi nhap cau hinh thi 2 kieu nhap nhu nhau
            String[] arrTenTruong = strTenTuong.Split(',');
            dtImportResult = dt.Clone();
            ResultSheetName = "ChiTietGia$";

            Bang bang = new Bang("DM_SanPham_DanhMucGia");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.DuLieuMoi = true;
            if (dt.Rows.Count > 0)
            {
                //Kiem tra xem co cau hinh chua, chua co thi them vao nhu la file exel
                int CoCauHinh = 0;
                SqlCommand query = new SqlCommand("SELECT COUNT(iID_MaDanhMucGia) AS counter FROM DM_SanPham_DanhMucGia " +
                                                                    "WHERE iTrangThai = 1 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaChiTietGia = 0");
                query.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                CoCauHinh = Convert.ToInt32(Connection.GetValue(query, 0));
                query.Dispose();
                if (CoCauHinh > 0) return; // co cau hinh roi thi thoat
                int stt = 0;
                foreach (DataRow Row in dt.Rows)
                {
                    String iDM_MaDonViTinh = "";
                    stt++;
                    bang.CmdParams.Parameters.Clear();
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTietGia", 0);
                    bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", stt);
                    for (int i = 0; i < Row.Table.Columns.Count; i++)
                    {
                        String TenTruong = arrTenTruong[i];
                        String DuLieu = Row[i].ToString().Trim();
                        switch (TenTruong)
                        {
                            case "iID_MaDanhMucGia_Cha":
                                String iID_MaDanhMucGia_Cha = "0";
                                if (!String.IsNullOrEmpty(DuLieu))
                                {
                                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 iID_MaDanhMucGia FROM DM_SanPham_DanhMucGia " +
                                                                    "WHERE iTrangThai = 1 AND iID_MaSanPham = @iID_MaSanPham " +
                                                                    "AND iID_MaChiTietGia = 0 AND LTRIM(RTRIM(sKyHieu)) = LTRIM(RTRIM(@sKyHieu)) " + 
                                                                    "ORDER BY dNgayTao DESC");
                                    cmd.Parameters.AddWithValue("@sKyHieu", DuLieu);
                                    cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                                    iID_MaDanhMucGia_Cha = Connection.GetValueString(cmd, "");
                                    cmd.Dispose();
                                }
                                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, iID_MaDanhMucGia_Cha);
                                break;
                            case "sKyHieu":
                                if (!String.IsNullOrEmpty(DuLieu))
                                {   //Danh muc ko phai vat tu
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", "dddddddd-dddd-dddd-dddd-dddddddddddd");
                                    bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", true);
                                }
                                else
                                {   //Import vat tu
                                    String TenDVT = Row[4].ToString().Trim();
                                    iDM_MaDonViTinh = ImportDonViTinh(TenDVT);
                                    String DonGia = Row[6].ToString();// ca 2 kieu nhap
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", ImportVatTu(Row[3].ToString().Trim(), iDM_MaDonViTinh, DonGia, Row[0].ToString()));
                                }
                                break;
                            case "sTen_DonVi":
                                if (String.IsNullOrEmpty(DuLieu))
                                {   //khong co don vi
                                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", "dddddddd-dddd-dddd-dddd-dddddddddddd");
                                }
                                else
                                {   //nhap don vi
                                    if (!String.IsNullOrEmpty(DuLieu) && String.IsNullOrEmpty(iDM_MaDonViTinh)) iDM_MaDonViTinh = ImportDonViTinh(DuLieu);
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
                                }
                                break;
                            case "sTen":
                                bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                break;
                            default:
                                break;
                        }
                    }
                    bang.Save();
                }
            }
        }
        private String ImportChiTiet(String FileName, String FilePath, String iID_MaSanPham, String iID_MaChiTietGia, String iID_LoaiDonVi, String iID_MaLoaiHinh, int KieuNhap)
        {
            try
            {
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
                conStr = String.Format(conStr, FilePath);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                connExcel.Open();

                DataTable dt = new DataTable();
                cmdExcel.CommandText = "SELECT * FROM [ChiTietGia$]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                String strTenTuong = "";
                if (KieuNhap == 1) {
                    strTenTuong = "bNganSach,iID_MaDanhMucGia_Cha,sKyHieu,sTen,sTen_DonVi,rSoLuong,rDonGia";
                }
                else
                {
                    strTenTuong = "bNganSach,iID_MaDanhMucGia_Cha,sKyHieu,sTen,sTen_DonVi,rSoLuong_DangThucHien,rDonGia_DangThucHien,rTien_DangThucHien,"
                                   + "rSoLuong_DV_DeNghi,rDonGia_DV_DeNghi,rTien_DV_DeNghi,rSoLuong_DatHang_DeNghi,rDonGia_DatHang_DeNghi,rTien_DatHang_DeNghi," 
                                   + "rSoLuong_CTC_DeNghi,rDonGia_CTC_DeNghi,rTien_CTC_DeNghi,rSoSanh";
                }
                String[] arrTenTruong = strTenTuong.Split(',');
                dtImportResult = dt.Clone();
                ResultSheetName = "ChiTietGia$";

                Bang bang = new Bang("DM_SanPham_DanhMucGia");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.DuLieuMoi = true;
                if (dt.Rows.Count > 0)
                {
                    int stt = 0;
                    foreach (DataRow Row in dt.Rows)
                    {
                        String iDM_MaDonViTinh = "";
                        stt++;
                        bang.CmdParams.Parameters.Clear();
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", stt);
                        for (int i = 0; i < Row.Table.Columns.Count; i++)
                        {
                            String TenTruong = arrTenTruong[i];
                            String DuLieu = Row[i].ToString().Trim();
                            switch (TenTruong)
                            {
                                case "iID_MaDanhMucGia_Cha":
                                    String iID_MaDanhMucGia_Cha = "0";
                                    if (!String.IsNullOrEmpty(DuLieu))
                                    {
                                        SqlCommand cmd = new SqlCommand("SELECT TOP 1 iID_MaDanhMucGia FROM DM_SanPham_DanhMucGia " +
                                                                        "WHERE iTrangThai = 1 AND iID_MaSanPham = @iID_MaSanPham " +
                                                                        "AND iID_MaChiTietGia = @iID_MaChiTietGia AND LTRIM(RTRIM(sKyHieu)) = LTRIM(RTRIM(@sKyHieu)) " +
                                                                        "ORDER BY dNgayTao DESC");
                                        cmd.Parameters.AddWithValue("@sKyHieu", DuLieu);
                                        cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                                        cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                                        iID_MaDanhMucGia_Cha = Connection.GetValueString(cmd, "0");
                                        cmd.Dispose();
                                    }
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, iID_MaDanhMucGia_Cha);
                                    break;
                                case "sKyHieu":
                                    if (!String.IsNullOrEmpty(DuLieu))
                                    {   //Danh muc ko phai vat tu
                                        bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                        bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", "dddddddd-dddd-dddd-dddd-dddddddddddd");
                                        bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", true);
                                    }
                                    else
                                    {   //Import vat tu
                                        String TenDVT = Row[4].ToString().Trim();
                                        iDM_MaDonViTinh = ImportDonViTinh(TenDVT);
                                        String DonGia = Row[6].ToString();// ca 2 kieu nhap
                                        bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", ImportVatTu(Row[3].ToString().Trim(), iDM_MaDonViTinh, DonGia, Row[0].ToString().Trim()));
                                    }
                                    break;
                                case "sTen_DonVi":
                                    if (String.IsNullOrEmpty(DuLieu))
                                    {   //khong co don vi
                                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", "dddddddd-dddd-dddd-dddd-dddddddddddd");
                                    }
                                    else
                                    {   //nhap don vi
                                        if (!String.IsNullOrEmpty(DuLieu) && String.IsNullOrEmpty(iDM_MaDonViTinh)) iDM_MaDonViTinh = ImportDonViTinh(DuLieu);
                                        bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                        bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
                                    }
                                    break;
                                case "sTen":
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_CauHinh", getIdCauHinh(DuLieu, iID_MaSanPham, iID_MaLoaiHinh));
                                    break;
                                case "bNganSach":
                                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, DuLieu);
                                    break;
                                default:
                                    if (KieuNhap == 1)
                                    {
                                        //nhap nhanh
                                        if (TenTruong == "rDonGia")
                                        {
                                            String SoLuong = Row[5].ToString();
                                            if (String.IsNullOrEmpty(DuLieu)) DuLieu = "0";
                                            if (String.IsNullOrEmpty(SoLuong)) SoLuong = "0";
                                            else SoLuong = SoLuong.Replace(',', '.');
                                            bang.CmdParams.Parameters.AddWithValue("@rSoLuong_DangThucHien", SoLuong);
                                            String iID_MaVatTu = bang.CmdParams.Parameters["@iID_MaVatTu"].Value.ToString();
                                            if (iID_MaVatTu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
                                            {
                                                DataTable dtGia = SanPham_VatTuModels.Get_GiaVatTu_Row(iID_MaVatTu);
                                                String GiaVatTu = "0";
                                                if (dtGia.Rows.Count > 0)
                                                {
                                                    if (Convert.ToString(Row[0]) == "0")
                                                    {
                                                        GiaVatTu = dtGia.Rows[0]["rGia"].ToString();
                                                    }
                                                    else
                                                    {
                                                        GiaVatTu = dtGia.Rows[0]["rGia_NS"].ToString();
                                                    }
                                                }
                                                if (String.IsNullOrEmpty(GiaVatTu)) GiaVatTu = "0";
                                                bang.CmdParams.Parameters.AddWithValue("@rDonGia_DangThucHien", GiaVatTu);
                                                bang.CmdParams.Parameters.AddWithValue("@rTien_DangThucHien", Decimal.Parse(GiaVatTu) * Decimal.Parse(SoLuong));
                                            }
                                            else
                                            {
                                                bang.CmdParams.Parameters.AddWithValue("@rDonGia_DangThucHien", DuLieu);
                                                bang.CmdParams.Parameters.AddWithValue("@rTien_DangThucHien", Decimal.Parse(DuLieu) * Decimal.Parse(SoLuong));
                                            }
                                            bang.CmdParams.Parameters.AddWithValue("@rSoLuong_DV_DeNghi", SoLuong);
                                            bang.CmdParams.Parameters.AddWithValue("@rDonGia_DV_DeNghi", DuLieu);
                                            bang.CmdParams.Parameters.AddWithValue("@rTien_DV_DeNghi", Decimal.Parse(DuLieu) * Decimal.Parse(SoLuong));
                                            Decimal rTien_DangThucHien = Decimal.Parse(bang.CmdParams.Parameters["@rTien_DangThucHien"].Value.ToString());
                                            Decimal rTien_DeNghi = 0;
                                            switch (iID_LoaiDonVi)
                                            {
                                                case "1":
                                                    rTien_DeNghi = Decimal.Parse(bang.CmdParams.Parameters["@rTien_DV_DeNghi"].Value.ToString());
                                                    break;
                                                case "2":
                                                    bang.CmdParams.Parameters.AddWithValue("@rSoLuong_DatHang_DeNghi", SoLuong);
                                                    bang.CmdParams.Parameters.AddWithValue("@rDonGia_DatHang_DeNghi", DuLieu);
                                                    bang.CmdParams.Parameters.AddWithValue("@rTien_DatHang_DeNghi", Decimal.Parse(DuLieu) * Decimal.Parse(SoLuong));
                                                    rTien_DeNghi = Decimal.Parse(bang.CmdParams.Parameters["@rTien_DatHang_DeNghi"].Value.ToString());
                                                    break;
                                                case "3":
                                                    bang.CmdParams.Parameters.AddWithValue("@rSoLuong_DatHang_DeNghi", SoLuong);
                                                    bang.CmdParams.Parameters.AddWithValue("@rDonGia_DatHang_DeNghi", DuLieu);
                                                    bang.CmdParams.Parameters.AddWithValue("@rTien_DatHang_DeNghi", Decimal.Parse(DuLieu) * Decimal.Parse(SoLuong));
                                                    bang.CmdParams.Parameters.AddWithValue("@rSoLuong_CTC_DeNghi", SoLuong);
                                                    bang.CmdParams.Parameters.AddWithValue("@rDonGia_CTC_DeNghi", DuLieu);
                                                    bang.CmdParams.Parameters.AddWithValue("@rTien_CTC_DeNghi", Decimal.Parse(DuLieu) * Decimal.Parse(SoLuong));
                                                    rTien_DeNghi = Decimal.Parse(bang.CmdParams.Parameters["@rTien_CTC_DeNghi"].Value.ToString());
                                                    break;
                                            }
                                            //0936250300
                                            if (rTien_DangThucHien != 0) bang.CmdParams.Parameters.AddWithValue("@rSoSanh", rTien_DeNghi / rTien_DangThucHien * 100);
                                            else bang.CmdParams.Parameters.AddWithValue("@rSoSanh", 0);
                                        }
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(DuLieu))
                                        {
                                            DuLieu = "0";
                                        }
                                        else
                                        {
                                            //DuLieu.Replace(".", "");
                                            DuLieu = DuLieu.Replace(',', '.');
                                        }
                                        bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, Decimal.Parse(DuLieu));
                                    }
                                    break;
                            }
                        }
                        bang.Save();
                    }
                }
                return iID_MaSanPham;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, int KieuNhap, String iID_MaSanPham)
        {
            try
            {
                String FileName = Request.Form[ParentID + "_FileName"];
                String FilePath = Request.Form[ParentID + "_FilePath"];
                //String iID_MaSanPham = ImportSanPham(FileName, FilePath);
                //ImportCauHinhDanhMuc(FileName, FilePath, iID_MaSanPham);
                String iID_MaLoaiHinh = Request.Form[ParentID + "_iID_MaLoaiHinh"];
                String iID_LoaiDonVi = Request.Form[ParentID + "_iID_LoaiDonVi"];
                Bang bang = new Bang("DM_SanPham_ChiTietGia");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.DuLieuMoi = true;
                bang.Save();
                //String iID_MaChiTietGia = TaoPhieuChiTietGiaSanPham(FileName, FilePath, iID_MaSanPham);
                String iID_MaChiTietGia = SanPham_DanhMucGiaModels.Get_MaxId_ChiTietGia();
                ImportChiTiet(FileName, FilePath, iID_MaSanPham, iID_MaChiTietGia, iID_LoaiDonVi, iID_MaLoaiHinh, KieuNhap);
                return RedirectToAction("ChiTiet", "SP_ChiTietGia", new { iID_MaSanPham = iID_MaSanPham, iID_MaChiTietGia = iID_MaChiTietGia, iID_LoaiDonVi = iID_LoaiDonVi, iID_MaLoaiHinh = iID_MaLoaiHinh });
            }
            catch (System.Exception ex)
            {
                return RedirectToAction("Index", "SanPham", new { });
            }
        }

        public ActionResult Result()
        {
            ViewData["dtImportResult"] = dtImportResult;
            ViewData["sLoi"] = sLoi;
            ViewData["ResultSheetName"] = ResultSheetName;
            ViewData["CountRecord"] = CountRecord;

            ClearValue();
            if (dtImportResult != null) dtImportResult.Dispose();
            return View(sViewPath + "Result.aspx");
        }

        public static String KiemTraMaVatTu(String sMaVatTu)
        {
            //=0: sai định dạng
            //=1: đã có trong csdl
            //=2: chưa có trong csdl
            if (sMaVatTu.Length != 12) return "0";

            Regex isnumber = new Regex("[^0-9]");
            if (isnumber.IsMatch(sMaVatTu)) return "0";

            SqlCommand cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
            cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
            String iID_MaVatTu = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            if (iID_MaVatTu != "") return "1";

            return "2";
        }

        public static Double KiemTraSoLuongTon(String SoLuongTonKho)
        {
            try
            {
                String s = "0123456789,";
                int d = 0;
                Double ton = 0;

                SoLuongTonKho = SoLuongTonKho.Replace(".", "");
                for (int i = 0; i < SoLuongTonKho.Length; i++)
                {
                    if (s.IndexOf(SoLuongTonKho[i].ToString()) < 0) return -1;
                    if (SoLuongTonKho[i].ToString() == ",") d += 1;
                    if (d == 2) return -1;
                }
                SoLuongTonKho = SoLuongTonKho.Replace(",", ".");
                ton = Convert.ToDouble(SoLuongTonKho);
                return ton;
            }
            catch
            {
                return -1;
            }
        }

        public static void ClearValue()
        {
            CountRecord = 0;
            ResultSheetName = "";
            sLoi = "";
        }

        public static String CatKyTuXuongDong(Object datarow)
        {
            return datarow.ToString().Replace(Convert.ToString((char)10), " ");
        }
        private static String getIdCauHinh(String sTen, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 iID_MaDanhMucGia FROM DM_SanPham_DanhMucGia WHERE iTrangThai = 1" +
                                    " AND LTRIM(RTRIM(sTen)) = LTRIM(RTRIM(@sTen)) AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaChiTietGia = 0");
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
            return Connection.GetValueString(cmd, "0");
        }
        //Thêm danh sách nhà cung cấp từ Excel
        //String sTen;
        //for (int i = 1; i <= dt.Rows.Count - 1; i++)
        //{
        //    DataRow dataRow = dt.Rows[i];
        //    sTen = Convert.ToString(dataRow[0]).Trim();

        //    Bang bangNCC = new Bang("DM_NhaCungCap");
        //    bangNCC.MaNguoiDungSua = "xmht";
        //    bangNCC.IPSua = "127.0.0.1";
        //    bangNCC.CmdParams.Parameters.AddWithValue("@sTen", sTen);//Tên nhà cung cấp
        //    bangNCC.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", "5");
        //    bangNCC.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");//Hoạt động
        //    bangNCC.Save();
        //}
        //dt.Dispose();
        //return null;
        //...thêm xong danh sách nhà cung cấp

        //Thêm danh sách vật tư từ Excel vào CSDL
        //String iDM_MaNhomLoaiVatTu = "";
        //String iDM_MaNhomChinh = "";
        //String iDM_MaNhomPhu = "";
        //String iDM_MaChiTietVatTu = "";
        //String iDM_MaXuatXu = "";
        //String iDM_MaDonViTinh = "";
        //String sMaVatTu, sTen, sMoTa, sQuyCach;
        //String sMaNhomLoaiVatTu, sMaNhomChinh, sMaNhomPhu, sMaChiTietVatTu, sMaXuatXu, DonViTinh;
        //SqlCommand cmd;
        //for (int i = 1; i <= dt.Rows.Count - 1; i++)
        //{
        //    DataRow dataRow = dt.Rows[i];
        //    sMaVatTu = Convert.ToString(dataRow[0]).Trim();
        //    sTen = Convert.ToString(dataRow[1]).Trim();
        //    DonViTinh = Convert.ToString(dataRow[2]).Trim();
        //    sMoTa = Convert.ToString(dataRow[3]).Trim();
        //    sQuyCach = Convert.ToString(dataRow[4]).Trim();

        //    sMaNhomLoaiVatTu = sMaVatTu.Substring(0,2);
        //    sMaNhomChinh = sMaVatTu.Substring(2, 2);
        //    sMaNhomPhu = sMaVatTu.Substring(4, 2);
        //    sMaChiTietVatTu = sMaVatTu.Substring(6, 4);
        //    sMaXuatXu = sMaVatTu.Substring(10, 2);

        //    Bang bangVT = new Bang("DM_VatTu");
        //    //=1: đã có trong csdl
        //    //=2: chưa có trong csdl
        //    if (KiemTraMaVatTu(sMaVatTu) == "1")
        //    {
        //        bangVT.DuLieuMoi = false;
        //        cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
        //        cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
        //        bangVT.GiaTriKhoa = Connection.GetValueString(cmd, "");
        //    }
        //    else if (KiemTraMaVatTu(sMaVatTu) == "2")
        //    {
        //        bangVT.DuLieuMoi = true;

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //                       "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND " +
        //                       "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                           "FROM DC_LoaiDanhMuc " +
        //                                           "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@sTenBang", "NhomLoaiVatTu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
        //        iDM_MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaNhomLoaiVatTu == "")
        //        {
        //            Bang bangMLDM = new Bang("DC_DanhMuc");
        //            bangMLDM.MaNguoiDungSua = "admin";
        //            bangMLDM.IPSua = "127.0.0.1";
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm loại vật tư " + sMaNhomLoaiVatTu);
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "48d4d999-bed6-4af8-a0a8-a70193915931");
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMLDM.DuLieuMoi = true;
        //            bangMLDM.Save();
        //            iDM_MaNhomLoaiVatTu = Convert.ToString(bangMLDM.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //            "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
        //            "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                "FROM DC_LoaiDanhMuc " +
        //                                "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
        //        cmd.Parameters.AddWithValue("@sTenBang", "NhomChinh");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
        //        iDM_MaNhomChinh = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaNhomChinh == "")
        //        {
        //            Bang bangMNC = new Bang("DC_DanhMuc");
        //            bangMNC.MaNguoiDungSua = "admin";
        //            bangMNC.IPSua = "127.0.0.1";
        //            bangMNC.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm chính " + sMaNhomChinh);
        //            bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "e783b244-a468-4ad7-99f1-74e69f242365");
        //            bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
        //            bangMNC.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
        //            bangMNC.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMNC.DuLieuMoi = true;
        //            bangMNC.Save();
        //            iDM_MaNhomChinh = Convert.ToString(bangMNC.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //            "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
        //            "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                "FROM DC_LoaiDanhMuc " +
        //                                "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
        //        cmd.Parameters.AddWithValue("@sTenBang", "NhomPhu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
        //        iDM_MaNhomPhu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaNhomPhu == "")
        //        {
        //            Bang bangMNP = new Bang("DC_DanhMuc");
        //            bangMNP.MaNguoiDungSua = "admin";
        //            bangMNP.IPSua = "127.0.0.1";
        //            bangMNP.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm phụ " + sMaNhomPhu);
        //            bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "6f0e089b-0f08-45b0-8c5a-a8b96d7393ce");
        //            bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
        //            bangMNP.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
        //            bangMNP.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMNP.DuLieuMoi = true;
        //            bangMNP.Save();
        //            iDM_MaNhomPhu = Convert.ToString(bangMNP.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //           "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
        //           "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                               "FROM DC_LoaiDanhMuc " +
        //                               "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
        //        cmd.Parameters.AddWithValue("@sTenBang", "ChiTietVatTu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
        //        iDM_MaChiTietVatTu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaChiTietVatTu == "")
        //        {
        //            Bang bangMCTVT = new Bang("DC_DanhMuc");
        //            bangMCTVT.MaNguoiDungSua = "admin";
        //            bangMCTVT.IPSua = "127.0.0.1";
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@sTen", "Chi tiết vật tư " + sMaChiTietVatTu);
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "4cdb608e-e9cc-42c2-bec2-f4172baa503e");
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMCTVT.DuLieuMoi = true;
        //            bangMCTVT.Save();
        //            iDM_MaChiTietVatTu = Convert.ToString(bangMCTVT.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //           "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND " +
        //           "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                               "FROM DC_LoaiDanhMuc " +
        //                               "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@sTenBang", "XuatXu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaXuatXu);
        //        iDM_MaXuatXu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaXuatXu == "")
        //        {
        //            Bang bangMXX = new Bang("DC_DanhMuc");
        //            bangMXX.MaNguoiDungSua = "admin";
        //            bangMXX.IPSua = "127.0.0.1";
        //            bangMXX.CmdParams.Parameters.AddWithValue("@sTen", "Xuất xứ " + sMaXuatXu);
        //            bangMXX.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "f743a3a7-9003-4e4e-ae70-50d0979304df");
        //            bangMXX.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaXuatXu);
        //            bangMXX.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMXX.DuLieuMoi = true;
        //            bangMXX.Save();
        //            iDM_MaXuatXu = Convert.ToString(bangMXX.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //                      "WHERE bHoatDong = 1 AND sTen = @sTen AND " +
        //                      "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                          "FROM DC_LoaiDanhMuc " +
        //                                          "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@sTenBang", "DonViTinh");
        //        cmd.Parameters.AddWithValue("@sTen", DonViTinh);
        //        iDM_MaDonViTinh = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaDonViTinh == "")
        //        {
        //            Bang bangMDVT = new Bang("DC_DanhMuc");
        //            bangMDVT.MaNguoiDungSua = "admin";
        //            bangMDVT.IPSua = "127.0.0.1";
        //            bangMDVT.CmdParams.Parameters.AddWithValue("@sTen", DonViTinh);
        //            bangMDVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "1225f0fa-b4fd-4dca-93b0-373384eef5f8");
        //            bangMDVT.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMDVT.DuLieuMoi = true;
        //            bangMDVT.Save();
        //            iDM_MaDonViTinh = Convert.ToString(bangMDVT.GiaTriKhoa);
        //        }
        //    }

        //    bangVT.MaNguoiDungSua = "admin";
        //    bangVT.IPSua = "127.0.0.1";
        //    bangVT.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);//Mã vật tư
        //    bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", iDM_MaNhomLoaiVatTu);//Mã nhóm loại vật tư
        //    bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", iDM_MaNhomChinh);//Mã nhóm chính
        //    bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);//Mã nhóm phụ
        //    bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", iDM_MaChiTietVatTu);//Mã chi tiết vật tư
        //    bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", iDM_MaXuatXu);//Mã xuất xứ
        //    bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);//Mã đơn vị tính
        //    bangVT.CmdParams.Parameters.AddWithValue("@sTen", sTen);//Tên vật tư
        //    bangVT.CmdParams.Parameters.AddWithValue("@sQuyCach", sQuyCach);//Quy cách
        //    bangVT.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);//Mô tả
        //    bangVT.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTen + " " + NgonNgu.LayXauKhongDauTiengViet(sTen));//Từ khóa - tên
        //    bangVT.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sQuyCach + " " + NgonNgu.LayXauKhongDauTiengViet(sQuyCach));//Từ khóa - quy cách
        //    bangVT.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", DateTime.Now);//Ngày phát sinh mã
        //    bangVT.CmdParams.Parameters.AddWithValue("@iTrangThai", "1");//Trạng thái
        //    bangVT.Save();
        //}
        //dt.Dispose();
        //return null;
        //...Thêm xong danh sách


        //Thêm danh sách vật tư - nhà cung cấp từ Excel vào CSDL
        //String iDM_MaNhomLoaiVatTu = "";
        //String iDM_MaNhomChinh = "";
        //String iDM_MaNhomPhu = "";
        //String iDM_MaChiTietVatTu = "";
        //String iDM_MaXuatXu = "";
        //String iDM_MaDonViTinh = "";
        //String iID_MaVatTu, sMaVatTu, sTen, iID_MaNhaCungCap, TenNCC, iID_VatTu_NhaCungCap;
        //String sMaNhomLoaiVatTu, sMaNhomChinh, sMaNhomPhu, sMaChiTietVatTu, sMaXuatXu, DonViTinh;
        //SqlCommand cmd;
        //for (int i = 1; i <= dt.Rows.Count - 1; i++)
        //{
        //    DataRow dataRow = dt.Rows[i];
        //    sMaVatTu = Convert.ToString(dataRow[0]).Trim();
        //    sTen = Convert.ToString(dataRow[1]).Trim();
        //    DonViTinh = Convert.ToString(dataRow[2]).Trim();

        //    sMaNhomLoaiVatTu = sMaVatTu.Substring(0,2);
        //    sMaNhomChinh = sMaVatTu.Substring(2, 2);
        //    sMaNhomPhu = sMaVatTu.Substring(4, 2);
        //    sMaChiTietVatTu = sMaVatTu.Substring(6, 4);
        //    sMaXuatXu = sMaVatTu.Substring(10, 2);

        //    //=1: đã có trong csdl
        //    //=2: chưa có trong csdl
        //    if (KiemTraMaVatTu(sMaVatTu) == "1")
        //    {
        //        cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
        //        cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
        //        iID_MaVatTu = Connection.GetValueString(cmd, "");
        //    }
        //    else if (KiemTraMaVatTu(sMaVatTu) == "2")
        //    {
        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //                       "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND " +
        //                       "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                           "FROM DC_LoaiDanhMuc " +
        //                                           "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@sTenBang", "NhomLoaiVatTu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
        //        iDM_MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaNhomLoaiVatTu == "")
        //        {
        //            Bang bangMLDM = new Bang("DC_DanhMuc");
        //            bangMLDM.MaNguoiDungSua = "admin";
        //            bangMLDM.IPSua = "127.0.0.1";
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm loại vật tư " + sMaNhomLoaiVatTu);
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "48d4d999-bed6-4af8-a0a8-a70193915931");
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
        //            bangMLDM.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMLDM.DuLieuMoi = true;
        //            bangMLDM.Save();
        //            iDM_MaNhomLoaiVatTu = Convert.ToString(bangMLDM.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //            "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
        //            "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                "FROM DC_LoaiDanhMuc " +
        //                                "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
        //        cmd.Parameters.AddWithValue("@sTenBang", "NhomChinh");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
        //        iDM_MaNhomChinh = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaNhomChinh == "")
        //        {
        //            Bang bangMNC = new Bang("DC_DanhMuc");
        //            bangMNC.MaNguoiDungSua = "admin";
        //            bangMNC.IPSua = "127.0.0.1";
        //            bangMNC.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm chính " + sMaNhomChinh);
        //            bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "e783b244-a468-4ad7-99f1-74e69f242365");
        //            bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
        //            bangMNC.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
        //            bangMNC.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMNC.DuLieuMoi = true;
        //            bangMNC.Save();
        //            iDM_MaNhomChinh = Convert.ToString(bangMNC.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //            "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
        //            "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                "FROM DC_LoaiDanhMuc " +
        //                                "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
        //        cmd.Parameters.AddWithValue("@sTenBang", "NhomPhu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
        //        iDM_MaNhomPhu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaNhomPhu == "")
        //        {
        //            Bang bangMNP = new Bang("DC_DanhMuc");
        //            bangMNP.MaNguoiDungSua = "admin";
        //            bangMNP.IPSua = "127.0.0.1";
        //            bangMNP.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm phụ " + sMaNhomPhu);
        //            bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "6f0e089b-0f08-45b0-8c5a-a8b96d7393ce");
        //            bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
        //            bangMNP.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
        //            bangMNP.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMNP.DuLieuMoi = true;
        //            bangMNP.Save();
        //            iDM_MaNhomPhu = Convert.ToString(bangMNP.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //           "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
        //           "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                               "FROM DC_LoaiDanhMuc " +
        //                               "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
        //        cmd.Parameters.AddWithValue("@sTenBang", "ChiTietVatTu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
        //        iDM_MaChiTietVatTu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaChiTietVatTu == "")
        //        {
        //            Bang bangMCTVT = new Bang("DC_DanhMuc");
        //            bangMCTVT.MaNguoiDungSua = "admin";
        //            bangMCTVT.IPSua = "127.0.0.1";
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@sTen", "Chi tiết vật tư " + sMaChiTietVatTu);
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "4cdb608e-e9cc-42c2-bec2-f4172baa503e");
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
        //            bangMCTVT.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMCTVT.DuLieuMoi = true;
        //            bangMCTVT.Save();
        //            iDM_MaChiTietVatTu = Convert.ToString(bangMCTVT.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //           "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND " +
        //           "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                               "FROM DC_LoaiDanhMuc " +
        //                               "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@sTenBang", "XuatXu");
        //        cmd.Parameters.AddWithValue("@sTenKhoa", sMaXuatXu);
        //        iDM_MaXuatXu = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaXuatXu == "")
        //        {
        //            Bang bangMXX = new Bang("DC_DanhMuc");
        //            bangMXX.MaNguoiDungSua = "admin";
        //            bangMXX.IPSua = "127.0.0.1";
        //            bangMXX.CmdParams.Parameters.AddWithValue("@sTen", "Xuất xứ " + sMaXuatXu);
        //            bangMXX.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "f743a3a7-9003-4e4e-ae70-50d0979304df");
        //            bangMXX.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaXuatXu);
        //            bangMXX.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMXX.DuLieuMoi = true;
        //            bangMXX.Save();
        //            iDM_MaXuatXu = Convert.ToString(bangMXX.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
        //                      "WHERE bHoatDong = 1 AND sTen = @sTen AND " +
        //                      "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
        //                                          "FROM DC_LoaiDanhMuc " +
        //                                          "WHERE sTenBang = @sTenBang)");
        //        cmd.Parameters.AddWithValue("@sTenBang", "DonViTinh");
        //        cmd.Parameters.AddWithValue("@sTen", DonViTinh);
        //        iDM_MaDonViTinh = Connection.GetValueString(cmd, "");
        //        cmd.Dispose();
        //        if (iDM_MaDonViTinh == "")
        //        {
        //            Bang bangMDVT = new Bang("DC_DanhMuc");
        //            bangMDVT.MaNguoiDungSua = "admin";
        //            bangMDVT.IPSua = "127.0.0.1";
        //            bangMDVT.CmdParams.Parameters.AddWithValue("@sTen", DonViTinh);
        //            bangMDVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "1225f0fa-b4fd-4dca-93b0-373384eef5f8");
        //            bangMDVT.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
        //            bangMDVT.DuLieuMoi = true;
        //            bangMDVT.Save();
        //            iDM_MaDonViTinh = Convert.ToString(bangMDVT.GiaTriKhoa);
        //        }

        //        Bang bangVT = new Bang("DM_VatTu");
        //        bangVT.DuLieuMoi = true;
        //        bangVT.MaNguoiDungSua = "xmht";
        //        bangVT.IPSua = "127.0.0.1";
        //        bangVT.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);//Mã vật tư
        //        bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", iDM_MaNhomLoaiVatTu);//Mã nhóm loại vật tư
        //        bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", iDM_MaNhomChinh);//Mã nhóm chính
        //        bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);//Mã nhóm phụ
        //        bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", iDM_MaChiTietVatTu);//Mã chi tiết vật tư
        //        bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", iDM_MaXuatXu);//Mã xuất xứ
        //        bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);//Mã đơn vị tính
        //        bangVT.CmdParams.Parameters.AddWithValue("@sTen", sTen);//Tên vật tư
        //        bangVT.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTen + " " + NgonNgu.LayXauKhongDauTiengViet(sTen));//Từ khóa - tên
        //        bangVT.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", DateTime.Now);//Ngày phát sinh mã
        //        bangVT.CmdParams.Parameters.AddWithValue("@iTrangThai", "1");//Trạng thái
        //        bangVT.Save();
        //        iID_MaVatTu = Convert.ToString(bangVT.GiaTriKhoa);

        //        //Kiểm tra và thêm danh sách nhà cung cấp
        //        TenNCC = Convert.ToString(dataRow[3]).Trim();
        //        cmd = new SqlCommand("SELECT iID_MaNhaCungCap FROM DM_NhaCungCap WHERE sTen = @TenNCC");
        //        cmd.Parameters.AddWithValue("@TenNCC", TenNCC);
        //        iID_MaNhaCungCap = Connection.GetValueString(cmd, "");
        //        if (iID_MaNhaCungCap == "")
        //        {
        //            Bang bangNCC = new Bang("DM_NhaCungCap");
        //            bangNCC.DuLieuMoi = true;
        //            bangNCC.MaNguoiDungSua = "xmht";
        //            bangNCC.IPSua = "127.0.0.1";
        //            bangNCC.CmdParams.Parameters.AddWithValue("@sTen", TenNCC);//Tên nhà cung cấp
        //            bangNCC.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", "5");
        //            bangNCC.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");//Hoạt động
        //            bangNCC.Save();
        //            iID_MaNhaCungCap = Convert.ToString(bangNCC.GiaTriKhoa);
        //        }

        //        cmd = new SqlCommand("SELECT iID_VatTu_NhaCungCap FROM DM_VatTu_NhaCungCap WHERE iID_MaVatTu = @iID_MaVatTu AND iID_MaNhaCungCap = @iID_MaNhaCungCap");
        //        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
        //        cmd.Parameters.AddWithValue("@iID_MaNhaCungCap", iID_MaNhaCungCap);
        //        iID_VatTu_NhaCungCap = Connection.GetValueString(cmd, "");

        //        if (iID_VatTu_NhaCungCap == "")
        //        {
        //            Bang bangVTNCC = new Bang("DM_VatTu_NhaCungCap");
        //            bangVTNCC.DuLieuMoi = true;
        //            bangVTNCC.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
        //            bangVTNCC.CmdParams.Parameters.AddWithValue("@iID_MaNhaCungCap", iID_MaNhaCungCap);
        //            bangVTNCC.MaNguoiDungSua = "xmht";
        //            bangVTNCC.IPSua = "127.0.0.1";
        //            bangVTNCC.Save();
        //        }
        //    }
        //}
        //dt.Dispose();
        //return null;
        //...Thêm xong danh sách
    }
}
