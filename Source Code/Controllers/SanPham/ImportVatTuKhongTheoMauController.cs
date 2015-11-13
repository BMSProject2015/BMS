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
namespace VIETTEL.Controllers
{
    public class ImportVatTuKhongTheoMauController : Controller
    {
        public static DataTable dtImportResult;
        public static int CountRecord = 0;
        public static String ResultSheetName = "", sLoi = "";
        public string sViewPath = "~/Views/SanPham/ImportVatTuKhongTheoMau/";
        public ActionResult Index(String TImport)
        {
            ViewData["TImport"] = TImport;
            return View(sViewPath + "Index.aspx");
        }

        public ActionResult Import()
        {
            return View(sViewPath + "Import.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Load(String ParentID, String TImport)
        {
            String FileName = Request.Form[ParentID + "_sFileName"];
            String FilePath = Server.MapPath("~/" + Request.Form[ParentID + "_sDuongDan"] + "");

            return RedirectToAction("Import", new { FileName = FileName, FilePath = FilePath, TImport = TImport });
        }

        public class SheetData
        {
            public string sData { get; set; }
        }

        public JsonResult get_dtSheet(String ParentID, String FilePath, String SheetName)
        {
            return Json(get_objSheet(ParentID, FilePath, SheetName), JsonRequestBehavior.AllowGet);
        }

        public static SheetData get_objSheet(String ParentID, String FilePath, String SheetName)
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
            vR += "<tr class='tr_form3'>";
            vR += "<td style='width: 10px;' align='center'>";
            vR += "<b>";
            vR += "<input type='checkbox' id='checkall' onclick='setCheckboxes();' checked='checked'>";
            vR += "</b>";
            vR += "</td>";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
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
                    vR += "<tr>";
                    vR += "<td style='width: 10px;' align='center'>";
                    vR += "<input type='checkbox' name='" + ParentID + "_checkGroup" + "' checked='checked' value='" + i + "'>";
                    vR += "</td>";
                    for (int j = 0; j < dataRow.Table.Columns.Count; j++)
                    {
                        string dataRowText = dataRow[j].ToString().Trim();
                        vR += "<td>";
                        vR += MyHtmlHelper.Label(dataRowText, dt.Columns[j].ColumnName);
                        vR += "</td>";
                    }
                    vR += "</tr>";
                }
                dt.Dispose();
            }
            vR += "</table>";
            data.sData = vR;
            return data;
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            #region code khởi tạo
            String FileName = Request.Form[ParentID + "_FileName"];
            String FilePath = Request.Form[ParentID + "_FilePath"];
            String SheetName = Request.Form[ParentID + "_SheetName"];

            String sTen;
            String iDM_MaNhomLoaiVatTu = "";
            String iDM_MaNhomChinh = "";
            String iDM_MaNhomPhu = "";
            String iDM_MaChiTietVatTu = "";
            String iDM_MaXuatXu = "";
            String iDM_MaDonViTinh = "";
            String sMaVatTu, sQuyCach, sNhaSanXuat, sMoTa, sMaCu, sTenGoc, sMoTaGoc;
            String sMaNhomLoaiVatTu, sMaNhomChinh, sMaNhomPhu, sMaChiTietVatTu, sMaXuatXu, DonViTinh;
            String iID_MaVatTu, iID_MaNhaCungCap, TenNCC, iID_VatTu_NhaCungCap;

            String TableName = "";
            String sTenBang = "";
            //String sTenBangCha = "";
            String GiaTriKhoa = "";
            SqlCommand cmd;
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
            conStr = String.Format(conStr, FilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            DataTable dt = new DataTable();
            cmdExcel.CommandText = "SELECT * FROM [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            #endregion
            #region Thêm danh sách nhà cung cấp từ Excel
            //Thêm danh sách nhà cung cấp từ Excel

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
            #endregion
            #region Thêm danh sách vật tư từ Excel vào CSDL
            //Thêm danh sách vật tư từ Excel vào CSDL

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DataRow dataRow = dt.Rows[i];
                sMaVatTu = Convert.ToString(dataRow[0]).Trim();
                sTen = Convert.ToString(dataRow[1]).Trim();
                DonViTinh = Convert.ToString(dataRow[2]).Trim();
                sNhaSanXuat = Convert.ToString(dataRow[3]).Trim();
                sQuyCach = Convert.ToString(dataRow[4]).Trim();
                sMoTa = Convert.ToString(dataRow[5]).Trim();
                sMaCu = Convert.ToString(dataRow[6]).Trim();
                sTenGoc = Convert.ToString(dataRow[7]).Trim();
                sMoTaGoc = Convert.ToString(dataRow[8]).Trim();

                sMaNhomLoaiVatTu = sMaVatTu.Substring(0, 2);
                sMaNhomChinh = sMaVatTu.Substring(2, 2);
                sMaNhomPhu = sMaVatTu.Substring(4, 2);
                sMaChiTietVatTu = sMaVatTu.Substring(6, 5);
                sMaXuatXu = sMaVatTu.Substring(11, 1);

                Bang bangVT = new Bang("DM_VatTu");
                //=1: đã có trong csdl
                //=2: chưa có trong csdl
                if (KiemTraMaVatTu(sMaVatTu) == "1")
                {
                    bangVT.DuLieuMoi = false;
                    cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                    cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                    bangVT.GiaTriKhoa = Connection.GetValueString(cmd, "");
                }
                else if (KiemTraMaVatTu(sMaVatTu) == "2")
                {
                    bangVT.DuLieuMoi = true;

                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                   "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND " +
                                   "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                       "FROM DC_LoaiDanhMuc " +
                                                       "WHERE sTenBang = @sTenBang)");
                    cmd.Parameters.AddWithValue("@sTenBang", "NhomLoaiVatTu");
                    cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
                    iDM_MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iDM_MaNhomLoaiVatTu == "")
                    {
                        Bang bangMLDM = new Bang("DC_DanhMuc");
                        bangMLDM.MaNguoiDungSua = "admin";
                        bangMLDM.IPSua = "127.0.0.1";
                        bangMLDM.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm loại vật tư " + sMaNhomLoaiVatTu);
                        bangMLDM.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "48D4D999-BED6-4AF8-A0A8-A70193915931");
                        bangMLDM.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
                        bangMLDM.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
                        bangMLDM.DuLieuMoi = true;
                        bangMLDM.Save();
                        iDM_MaNhomLoaiVatTu = Convert.ToString(bangMLDM.GiaTriKhoa);
                    }

                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
                        "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                            "FROM DC_LoaiDanhMuc " +
                                            "WHERE sTenBang = @sTenBang)");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
                    cmd.Parameters.AddWithValue("@sTenBang", "NhomChinh");
                    cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
                    iDM_MaNhomChinh = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iDM_MaNhomChinh == "")
                    {
                        Bang bangMNC = new Bang("DC_DanhMuc");
                        bangMNC.MaNguoiDungSua = "admin";
                        bangMNC.IPSua = "127.0.0.1";
                        bangMNC.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm chính " + sMaNhomChinh);
                        bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "E783B244-A468-4AD7-99F1-74E69F242365");
                        bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
                        bangMNC.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
                        bangMNC.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
                        bangMNC.DuLieuMoi = true;
                        bangMNC.Save();
                        iDM_MaNhomChinh = Convert.ToString(bangMNC.GiaTriKhoa);
                    }

                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
                        "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                            "FROM DC_LoaiDanhMuc " +
                                            "WHERE sTenBang = @sTenBang)");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
                    cmd.Parameters.AddWithValue("@sTenBang", "NhomPhu");
                    cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
                    iDM_MaNhomPhu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iDM_MaNhomPhu == "")
                    {
                        Bang bangMNP = new Bang("DC_DanhMuc");
                        bangMNP.MaNguoiDungSua = "admin";
                        bangMNP.IPSua = "127.0.0.1";
                        bangMNP.CmdParams.Parameters.AddWithValue("@sTen", "Nhóm phụ " + sMaNhomPhu);
                        bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "6F0E089B-0F08-45B0-8C5A-A8B96D7393CE");
                        bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
                        bangMNP.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
                        bangMNP.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
                        bangMNP.DuLieuMoi = true;
                        bangMNP.Save();
                        iDM_MaNhomPhu = Convert.ToString(bangMNP.GiaTriKhoa);
                    }

                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
                       "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                           "FROM DC_LoaiDanhMuc " +
                                           "WHERE sTenBang = @sTenBang)");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
                    cmd.Parameters.AddWithValue("@sTenBang", "ChiTietVatTu");
                    cmd.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
                    iDM_MaChiTietVatTu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iDM_MaChiTietVatTu == "")
                    {
                        Bang bangMCTVT = new Bang("DC_DanhMuc");
                        bangMCTVT.MaNguoiDungSua = "admin";
                        bangMCTVT.IPSua = "127.0.0.1";
                        bangMCTVT.CmdParams.Parameters.AddWithValue("@sTen", "Chi tiết vật tư " + sMaChiTietVatTu);
                        bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "4CDB608E-E9CC-42C2-BEC2-F4172BAA503E");
                        bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
                        bangMCTVT.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
                        bangMCTVT.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
                        bangMCTVT.DuLieuMoi = true;
                        bangMCTVT.Save();
                        iDM_MaChiTietVatTu = Convert.ToString(bangMCTVT.GiaTriKhoa);
                    }

                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa AND " +
                       "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                           "FROM DC_LoaiDanhMuc " +
                                           "WHERE sTenBang = @sTenBang)");
                    cmd.Parameters.AddWithValue("@sTenBang", "XuatXu");
                    cmd.Parameters.AddWithValue("@sTenKhoa", sMaXuatXu);
                    iDM_MaXuatXu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iDM_MaXuatXu == "")
                    {
                        Bang bangMXX = new Bang("DC_DanhMuc");
                        bangMXX.MaNguoiDungSua = "admin";
                        bangMXX.IPSua = "127.0.0.1";
                        bangMXX.CmdParams.Parameters.AddWithValue("@sTen", "Tình trạng vật tư " + sMaXuatXu);
                        bangMXX.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "F743A3A7-9003-4E4E-AE70-50D0979304DF");
                        bangMXX.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaXuatXu);
                        bangMXX.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
                        bangMXX.DuLieuMoi = true;
                        bangMXX.Save();
                        iDM_MaXuatXu = Convert.ToString(bangMXX.GiaTriKhoa);
                    }

                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                  "WHERE bHoatDong = 1 AND sTen = @sTen AND " +
                                  "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                      "FROM DC_LoaiDanhMuc " +
                                                      "WHERE sTenBang = @sTenBang)");
                    cmd.Parameters.AddWithValue("@sTenBang", "DonViTinh");
                    cmd.Parameters.AddWithValue("@sTen", DonViTinh);
                    iDM_MaDonViTinh = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    if (iDM_MaDonViTinh == "")
                    {
                        Bang bangMDVT = new Bang("DC_DanhMuc");
                        bangMDVT.MaNguoiDungSua = "admin";
                        bangMDVT.IPSua = "127.0.0.1";
                        bangMDVT.CmdParams.Parameters.AddWithValue("@sTen", DonViTinh);
                        bangMDVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "1225F0FA-B4FD-4DCA-93B0-373384EEF5F8");
                        bangMDVT.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
                        bangMDVT.DuLieuMoi = true;
                        bangMDVT.Save();
                        iDM_MaDonViTinh = Convert.ToString(bangMDVT.GiaTriKhoa);
                    }
                }
                String sMaYeuCau = "IMPORTS-" + i;

                bangVT.MaNguoiDungSua = "admin";
                bangVT.IPSua = "127.0.0.1";
                bangVT.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);//Mã vật tư
                bangVT.CmdParams.Parameters.AddWithValue("@sMaYeuCau", sMaYeuCau);
                bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", iDM_MaNhomLoaiVatTu);//Mã nhóm loại vật tư
                bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", iDM_MaNhomChinh);//Mã nhóm chính
                bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);//Mã nhóm phụ
                bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", iDM_MaChiTietVatTu);//Mã chi tiết vật tư
                bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", iDM_MaXuatXu);//Mã xuất xứ
                bangVT.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);//Mã đơn vị tính
                bangVT.CmdParams.Parameters.AddWithValue("@sTen", sTen);//Tên vật tư
                bangVT.CmdParams.Parameters.AddWithValue("@sNhaSanXuat", sNhaSanXuat);//Nhà sản xuất
                bangVT.CmdParams.Parameters.AddWithValue("@sQuyCach", sQuyCach);//Quy cách
                bangVT.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);//Mô tả
                bangVT.CmdParams.Parameters.AddWithValue("@sMaCu", sMaCu);//Mã cũ
                bangVT.CmdParams.Parameters.AddWithValue("@sTenGoc", sTenGoc);//Tên cũ
                bangVT.CmdParams.Parameters.AddWithValue("@sMoTaGoc", sMoTaGoc);//Mô tả cũ
                bangVT.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTen + " " + NgonNgu.LayXauKhongDauTiengViet(sTen));//Từ khóa - tên
                bangVT.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sQuyCach + " " + NgonNgu.LayXauKhongDauTiengViet(sQuyCach));//Từ khóa - quy cách
                bangVT.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", DateTime.Now);//Ngày phát sinh mã
                bangVT.CmdParams.Parameters.AddWithValue("@iTrangThai", "1");//Trạng thái
                bangVT.Save();
            }
            dt.Dispose();
            return null;
            //...Thêm xong danh sách
            #endregion
            #region Thêm danh sách vật tư - nhà cung cấp từ Excel vào CSDL
            //Thêm danh sách vật tư - nhà cung cấp từ Excel vào CSDL

            //for (int i = 1; i <= dt.Rows.Count - 1; i++)
            //{
            //    DataRow dataRow = dt.Rows[i];
            //    sMaVatTu = Convert.ToString(dataRow[0]).Trim();
            //    sTen = Convert.ToString(dataRow[1]).Trim();
            //    DonViTinh = Convert.ToString(dataRow[2]).Trim();

            //    sMaNhomLoaiVatTu = sMaVatTu.Substring(0, 2);
            //    sMaNhomChinh = sMaVatTu.Substring(2, 2);
            //    sMaNhomPhu = sMaVatTu.Substring(4, 2);
            //    sMaChiTietVatTu = sMaVatTu.Substring(6, 5);
            //    sMaXuatXu = sMaVatTu.Substring(11, 1);

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
            //            bangMLDM.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "48D4D999-BED6-4AF8-A0A8-A70193915931");
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
            //            bangMNC.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "E783B244-A468-4AD7-99F1-74E69F242365");
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
            //            bangMNP.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "6F0E089B-0F08-45B0-8C5A-A8B96D7393CE");
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
            //            bangMCTVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "4CDB608E-E9CC-42C2-BEC2-F4172BAA503E");
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
            //            bangMXX.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "F743A3A7-9003-4E4E-AE70-50D0979304DF");
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
            //            bangMDVT.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", "1225F0FA-B4FD-4DCA-93B0-373384EEF5F8");
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
            #endregion
        }
    }
}