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
    public class ImportExcelVatTuController : Controller
    {
        //
        // GET: /ImportExcelVatTu/
        public string sViewPath = "~/Views/SanPham/ImportExcelVatTu/";
        public static DataTable dtImportResult;
        public static int CountRecord = 0;
        public static String ResultSheetName = "", sLoi = "";

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = Request.Form[ParentID + "_FileName"];
            String FilePath = Request.Form[ParentID + "_FilePath"];
            String SheetName = Request.Form[ParentID + "_SheetName"];
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

            switch (SheetName)
            {
                case "VatTu$":
                    TableName = "DM_VatTu";
                    GiaTriKhoa = "@iID_MaVatTu";
                    break;

                case "NhomLoaiVatTu$":
                    TableName = "DC_DanhMuc";
                    sTenBang = "NhomLoaiVatTu";
                    GiaTriKhoa = "@iID_MaDanhMuc";
                    break;

                case "NhomChinh$":
                    TableName = "DC_DanhMuc";
                    sTenBang = "NhomChinh";
                    GiaTriKhoa = "@iID_MaDanhMuc";
                    break;

                case "NhomPhu$":
                    TableName = "DC_DanhMuc";
                    sTenBang = "NhomPhu";
                    GiaTriKhoa = "@iID_MaDanhMuc";
                    break;

                case "ChiTietVatTu$":
                    TableName = "DC_DanhMuc";
                    sTenBang = "ChiTietVatTu";
                    GiaTriKhoa = "@iID_MaDanhMuc";
                    break;

                case "TinhTrang$":
                    TableName = "DC_DanhMuc";
                    sTenBang = "XuatXu";
                    GiaTriKhoa = "@iID_MaDanhMuc";
                    break;

                case "DonViTinh$":
                    TableName = "DC_DanhMuc";
                    sTenBang = "DonViTinh";
                    GiaTriKhoa = "@iID_MaDanhMuc";
                    break;

                case "TonKho$":
                    TableName = "DM_DonVi_TonKho";
                    GiaTriKhoa = "@iID_MaTonKho";
                    break;
            }
            dtImportResult = dt.Clone();
            ResultSheetName = SheetName;

            String sCheck = Request.Form[ParentID + "_checkGroup"];
            String sTenKhoa = "";
            String iDM_MaNhomLoaiVatTu = null;
            String iDM_MaNhomChinh = null;
            String iDM_MaNhomPhu = null;
            String iDM_MaChiTietVatTu = null;
            String iDM_MaXuatXu = null;
            String iDM_MaDonViTinh = null;
            String iID_MaLoaiDanhMuc = null;
            String iID_MaDanhMuc = null;
            String iID_MaDanhMucCha = null;
            String sMaNhomLoaiVatTu = "";
            String sMaNhomChinh = "";
            String sMaNhomPhu = "";
            String sMaChiTietVatTu = "";
            String sMaXuatXu = "";
            String sMaVatTu = "";
            String sTen = "";
            String DonViTinh = "";
            String NhaSanXuat = "";
            String sQuyCach = "";
            String sMoTa = "";
            String MaHeThongCu = "";                     
            String sTenGoc = "";
            String sMoTaGoc = "";
            String FileDinhKem = "";
            String TrangThai = "";
            String sGhiChu = "";   
            String sTuKhoa_sTen="";
            String sTuKhoa_sTenGoc="";
            String sTuKhoa_sQuyCach = "";
            String iID_MaDonViDangNhap = "";
            String iID_MaVatTu = "";
            Double SoLuongTonKho = 0;
            String iID_MaTonKho = "";
            if (string.IsNullOrEmpty(sCheck) == false)
            {
                String[] arrCheck = sCheck.Split(',');
                
                for (int i = 0; i <= arrCheck.Length - 1; i++)
                {
                    DataRow dataRow = dt.Rows[int.Parse(arrCheck[i])];
                    //DateTime startTime = DateTime.Now;

                    try
                    {
                        Bang bang = new Bang(TableName);

                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;

                        //if (dataRow[0].ToString() == "") continue;
                        if (sTenBang != "")
                        {
                            cmd = new SqlCommand("SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = @sTenBang");
                            cmd.Parameters.AddWithValue("@sTenBang", sTenBang);
                            iID_MaLoaiDanhMuc = Connection.GetValueString(cmd, "");
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);//Loại danh mục

                            if (sTenBang == "DonViTinh")
                            {
                                sTen = dataRow[0].ToString();
                                sGhiChu = dataRow[1].ToString();

                                cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                    "WHERE bHoatDong = 1 AND sTen = @sTen AND iID_MaLoaiDanhMuc = @iID_MaLoaiDanhMuc");
                                cmd.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
                                cmd.Parameters.AddWithValue("@sTen", sTen);
                                iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                                if (iID_MaDanhMuc != "")
                                {
                                    sLoi = sLoi +
                                        NgonNgu.LayXau("Trùng dữ liệu: ") + sTen +
                                        NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                    continue;
                                }
                            }
                            else
                            {
                                if (sTenBang == "NhomChinh" || sTenBang == "NhomPhu" || sTenBang == "ChiTietVatTu")
                                {
                                    if (dataRow[1].ToString() == "") continue;
                                    //Kiểm tra mã nhóm loại vật tư
                                    sMaNhomLoaiVatTu = dataRow[0].ToString();
                                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                        "WHERE sTenKhoa = @sTenKhoa AND " +
                                        "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                            "FROM DC_LoaiDanhMuc " +
                                                            "WHERE sTenBang = @sTenBang)");
                                    cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomLoaiVatTu);
                                    cmd.Parameters.AddWithValue("@sTenBang", "NhomLoaiVatTu");
                                    iDM_MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
                                    cmd.Dispose();
                                    if (iDM_MaNhomLoaiVatTu == "")
                                    {
                                        sLoi = sLoi +
                                            NgonNgu.LayXau("Mã nhóm loại vật tư: ") + sMaNhomLoaiVatTu + NgonNgu.LayXau(" không tồn tại") +
                                            NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                        continue;
                                    }
                                    if (sTenBang == "NhomChinh")
                                    {
                                        iID_MaDanhMucCha = iDM_MaNhomLoaiVatTu;
                                        sTenKhoa = dataRow[1].ToString();
                                        sTen = dataRow[2].ToString();
                                        sGhiChu = dataRow[3].ToString();
                                    }
                                    else if (sTenBang == "NhomPhu" || sTenBang == "ChiTietVatTu")
                                    {
                                        if (dataRow[2].ToString() == "") continue;
                                        //Kiểm tra mã nhóm chính
                                        sMaNhomChinh = dataRow[1].ToString();
                                        cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                            "WHERE sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
                                            "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = @sTenBang)");
                                        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomLoaiVatTu);
                                        cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomChinh);
                                        cmd.Parameters.AddWithValue("@sTenBang", "NhomChinh");
                                        iDM_MaNhomChinh = Connection.GetValueString(cmd, "");
                                        cmd.Dispose();
                                        if (iDM_MaNhomChinh == "")
                                        {
                                            sLoi = sLoi +
                                                NgonNgu.LayXau("Mã nhóm chính: ") + sMaNhomChinh + NgonNgu.LayXau(" không tồn tại hoặc không thuộc ") +
                                                NgonNgu.LayXau("nhóm loại vật tư: ") + sMaNhomLoaiVatTu +
                                                NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                            continue;
                                        }
                                        if (sTenBang == "NhomPhu")
                                        {
                                            iID_MaDanhMucCha = iDM_MaNhomChinh;
                                            sTenKhoa = dataRow[2].ToString();
                                            sTen = dataRow[3].ToString();
                                            sGhiChu = dataRow[4].ToString();
                                        }
                                        else
                                        {
                                            if (dataRow[3].ToString() == "") continue;
                                            //Kiểm mã nhóm phụ
                                            sMaNhomPhu = dataRow[2].ToString();
                                            cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                                "WHERE sTenKhoa = @sTenKhoa AND iID_MaDanhMucCha = @iID_MaDanhMucCha AND " +
                                                "iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = @sTenBang)");
                                            cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomChinh);
                                            cmd.Parameters.AddWithValue("@sTenKhoa", sMaNhomPhu);
                                            cmd.Parameters.AddWithValue("@sTenBang", "NhomPhu");
                                            iDM_MaNhomPhu = Connection.GetValueString(cmd, "");
                                            cmd.Dispose();

                                            if (iDM_MaNhomPhu == "")
                                            {
                                                sLoi = sLoi +
                                                    NgonNgu.LayXau("Mã nhóm phụ: ") + sMaNhomPhu + NgonNgu.LayXau(" không tồn tại hoặc không thuộc ") +
                                                    NgonNgu.LayXau("nhóm chính: ") + sMaNhomChinh +
                                                    NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                                continue;
                                            }
                                            iID_MaDanhMucCha = iDM_MaNhomPhu;
                                            sTenKhoa = dataRow[3].ToString();
                                            sTen = dataRow[4].ToString();
                                            sGhiChu = dataRow[5].ToString();
                                        }
                                    }      
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iID_MaDanhMucCha);//Danh mục cha
                                }
                                else if (sTenBang == "NhomLoaiVatTu" || sTenBang == "XuatXu")
                                {
                                    sTenKhoa = dataRow[0].ToString();
                                    sTen = dataRow[1].ToString();
                                    sGhiChu = dataRow[2].ToString();
                                }

                                if (iID_MaDanhMucCha == "")
                                {
                                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                    "WHERE sTenKhoa = @sTenKhoa AND iID_MaLoaiDanhMuc = @iID_MaLoaiDanhMuc");
                                    cmd.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
                                    cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
                                    iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                                    cmd.Dispose();
                                }
                                else
                                {
                                    cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
                                    "WHERE iID_MaDanhMucCha = @iID_MaDanhMucCha AND sTenKhoa = @sTenKhoa AND iID_MaLoaiDanhMuc = @iID_MaLoaiDanhMuc");
                                    cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iID_MaDanhMucCha);
                                    cmd.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
                                    cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
                                    iID_MaDanhMuc = Connection.GetValueString(cmd, "");
                                    cmd.Dispose();
                                }
                                
                                if (iID_MaDanhMuc != "")
                                {
                                    sLoi = sLoi +
                                        NgonNgu.LayXau("Trùng dữ liệu: ") + sTenKhoa +
                                        NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                    continue;
                                }
                                bang.CmdParams.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);//Tên khóa
                            }
                            bang.CmdParams.Parameters.AddWithValue("@sTen", CatKyTuXuongDong(sTen));//Tên danh mục
                            bang.CmdParams.Parameters.AddWithValue("@sGhiChu", CatKyTuXuongDong(sGhiChu));//Ghi chú
                            bang.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");//Hoạt động
                            bang.DuLieuMoi = true;
                            bang.Save();
                        }
                        else 
                        {
                            if (TableName == "DM_VatTu")
                            {
                                //if (dataRow[0].ToString() == "" ) continue;

                                String sID_MaNguoiDung = User.Identity.Name;
                                String sMaDonViLayMa = "";
                                cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                                                    "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
                                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
                                String iID_MaDonViDangNhapTaoMa = Connection.GetValueString(cmd, "");
                                cmd.Dispose();
                                if (iID_MaDonViDangNhapTaoMa == "-1")
                                {
                                    iID_MaDonViDangNhapTaoMa = null;
                                    sMaDonViLayMa = "VIC";
                                }
                                else if (iID_MaDonViDangNhapTaoMa != null)
                                {

                                    cmd = new SqlCommand("SELECT sTen,iID_MaDonVi FROM NS_DonVi ORDER BY iSTT WHERE iID_MaDonVi = " + iID_MaDonViDangNhapTaoMa);
                                    DataTable dtDonViLayTaoMa = Connection.GetDataTable(cmd);
                                    sMaDonViLayMa = Convert.ToString(dtDonViLayTaoMa.Rows[0]["iID_MaDonVi"]);
                                    cmd.Dispose();
                                    dtDonViLayTaoMa.Dispose();
                                }

                                DateTime dDate = DateTime.Now;
                                dDate.ToString();

                                String strMaYeuCau = "";
                                cmd = new SqlCommand("select top 1 sMaYeuCau from DM_VatTu where convert(varchar(10),dNgayPhatSinhMa,111) =  '" + dDate.ToString("yyyy/MM/dd") + "' order by dNgayPhatSinhMa desc");
                                strMaYeuCau = Convert.ToString(Connection.GetValueString(cmd, ""));
                                cmd.Dispose();

                                String numHT = "00";
                                int num = 0;
                                if (strMaYeuCau != "" && strMaYeuCau.IndexOf("IMPORTS") == -1)
                                {
                                    String[] arr = strMaYeuCau.Split('-');
                                    num = Convert.ToInt32(arr[3]) + 1;
                                    if (num.ToString().Length == 1)
                                    {
                                        numHT = "0" + num;
                                    }
                                    else
                                    {
                                        numHT = Convert.ToString(num);
                                    }
                                }

                                String sMaYeuCau = "";
                                sMaYeuCau = "YC-" + sMaDonViLayMa + "-" + dDate.ToString("yy") + dDate.ToString("MM") + dDate.ToString("dd") + "-" + numHT;
                                if (!String.IsNullOrEmpty(iID_MaVatTu))
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        sMaYeuCau = Convert.ToString(dt.Rows[0]["sMaYeuCau"]);
                                    }
                                }
                                
                                sMaVatTu = dataRow[0].ToString().Trim();
                                sTen = CatKyTuXuongDong(dataRow[1].ToString());
                                DonViTinh = dataRow[2].ToString().Trim();
                                sQuyCach = CatKyTuXuongDong(dataRow[3].ToString());
                                sMoTa = CatKyTuXuongDong(dataRow[4].ToString());
                                NhaSanXuat = CatKyTuXuongDong(dataRow[5].ToString().Trim());
                                MaHeThongCu = dataRow[6].ToString();
                                sTenGoc = CatKyTuXuongDong(dataRow[7].ToString());
                                sMoTaGoc = CatKyTuXuongDong(dataRow[8].ToString());
                                FileDinhKem = dataRow[9].ToString();
                                sGhiChu = "";

                                cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                                 "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                              "FROM QT_NguoiDung " +
                                                              "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
                                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", User.Identity.Name);
                                iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
                                cmd.Dispose();
                                if (iID_MaDonViDangNhap == "-1") TrangThai = dataRow[10].ToString();

                                if (sMaVatTu != "")
                                {
                                    sMaNhomLoaiVatTu = sMaVatTu.Substring(0, 2);
                                    sMaNhomChinh = sMaVatTu.Substring(2, 2);
                                    sMaNhomPhu = sMaVatTu.Substring(4, 2);
                                    if (sMaVatTu.Length == 12)
                                    {
                                        sMaChiTietVatTu = sMaVatTu.Substring(6, 5);
                                        sMaXuatXu = sMaVatTu.Substring(11, 1);
                                    }
                                
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
                                        sLoi = sLoi +
                                            NgonNgu.LayXau("Mã nhóm loại vật tư: ") + sMaNhomLoaiVatTu + NgonNgu.LayXau(" không tồn tại") +
                                            NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                        continue;
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
                                        sLoi = sLoi +
                                            NgonNgu.LayXau("Mã nhóm chính: ") + sMaNhomChinh + NgonNgu.LayXau(" không tồn tại hoặc không thuộc ") +
                                            NgonNgu.LayXau("nhóm loại vật tư: ") + sMaNhomLoaiVatTu +
                                            NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                        continue;
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
                                        sLoi = sLoi +
                                            NgonNgu.LayXau("Mã nhóm phụ: ") + sMaNhomPhu + NgonNgu.LayXau(" không tồn tại hoặc không thuộc ") +
                                            NgonNgu.LayXau("nhóm chính: ") + sMaNhomChinh +
                                            NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                        continue;
                                    }

                                    if (sMaXuatXu != "")
                                    {
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
                                            sLoi = sLoi +
                                                NgonNgu.LayXau("Mã tình trạng vật tư: ") + sMaXuatXu + NgonNgu.LayXau(" không tồn tại") +
                                                NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                            continue;
                                        }
                                    }

                                    if (sMaChiTietVatTu != "" && sMaXuatXu != "")
                                    {
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
                                            if (sMaChiTietVatTu != "" && iDM_MaNhomLoaiVatTu != "" && iDM_MaNhomChinh != "" && iDM_MaNhomPhu != "")
                                            {
                                                if (sMaChiTietVatTu.Length == 5)
                                                {
                                                    String MaLoaiDanhMuc = "";
                                                    cmd = new SqlCommand("SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang");
                                                    cmd.Parameters.AddWithValue("@sTenBang", "ChiTietVatTu");
                                                    MaLoaiDanhMuc = Connection.GetValueString(cmd, "");
                                                    cmd.Dispose();

                                                    Bang bangdm = new Bang("DC_DanhMuc");
                                                    bangdm.DuLieuMoi = true;
                                                    bangdm.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", MaLoaiDanhMuc);
                                                    bangdm.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iDM_MaNhomPhu);
                                                    bangdm.CmdParams.Parameters.AddWithValue("@sTenKhoa", sMaChiTietVatTu);
                                                    bangdm.CmdParams.Parameters.AddWithValue("@bHoatDong", 1);
                                                    if (Convert.ToInt32(sMaXuatXu) == 1)
                                                    {
                                                        bangdm.CmdParams.Parameters.AddWithValue("@bDangDung", 1);
                                                    }
                                                    else {
                                                        bangdm.CmdParams.Parameters.AddWithValue("@bDangDung", 0);
                                                    }
                                                    bangdm.Save();

                                                    iDM_MaChiTietVatTu = Convert.ToString(bangdm.GiaTriKhoa);
                                                }
                                            }
                                            //sLoi = sLoi +
                                            //    NgonNgu.LayXau("Mã chi tiết vật tư: ") + sMaChiTietVatTu + NgonNgu.LayXau(" không tồn tại hoặc không thuộc ") +
                                            //    NgonNgu.LayXau("nhóm phụ: ") + sMaNhomPhu +
                                            //    NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                            //continue;
                                        }
                                    }

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
                                    sLoi = sLoi +
                                        NgonNgu.LayXau("Đơn vị tính: ") + DonViTinh + NgonNgu.LayXau(" không tồn tại") +
                                        NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                    continue;
                                }
                                if (sMaVatTu != "" && sMaVatTu.Length == 12)
                                {
                                    if (KiemTraMaVatTu(sMaVatTu) != "2")
                                    {
                                        sLoi = sLoi +
                                            NgonNgu.LayXau("Mã vật tư: ") + sMaVatTu + NgonNgu.LayXau(" sai định dạng hoặc đã có") +
                                            NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                        continue;
                                    }
                                }
                                bang.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);//Mã vật tư
                                bang.CmdParams.Parameters.AddWithValue("@sMaYeuCau", sMaYeuCau); //Mã yêu cầu
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", iDM_MaNhomLoaiVatTu);//Mã nhóm loại vật tư
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", iDM_MaNhomChinh);//Mã nhóm chính
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);//Mã nhóm phụ
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", iDM_MaChiTietVatTu);//Mã chi tiết vật tư
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", iDM_MaXuatXu);//Mã xuất xứ
                                bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);//Mã đơn vị tính
                                bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);//Tên vật tư
                                bang.CmdParams.Parameters.AddWithValue("@sNhaSanXuat", NhaSanXuat);//Nhà sản xuất
                                bang.CmdParams.Parameters.AddWithValue("@sQuyCach", sQuyCach);//Quy cách
                                bang.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);//Mô tả
                                bang.CmdParams.Parameters.AddWithValue("@sMaCu", MaHeThongCu);//Mã hệ thống cũ
                                bang.CmdParams.Parameters.AddWithValue("@sTenGoc", sTenGoc);//Tên gốc                                
                                bang.CmdParams.Parameters.AddWithValue("@sMoTaGoc", sMoTaGoc);//Mô tả gốc
                                bang.CmdParams.Parameters.AddWithValue("@sFileDinhKem", FileDinhKem);//File đính kèm
                                bang.CmdParams.Parameters.AddWithValue("@sGhiChu", sGhiChu);//Ghi chú

                                if (iID_MaDonViDangNhap != "-1")
                                {
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonViDangNhap);//Mã đơn vị
                                    TrangThai = "2";
                                }
                                else
                                {
                                    switch (TrangThai)
                                    {
                                        case "Ngừng hoạt động":
                                            TrangThai = "0";
                                            break;

                                        case "Đang sử dụng":
                                            TrangThai = "1";
                                            break;

                                        case "Chờ duyệt":
                                            TrangThai = "2";
                                            break;

                                        case "Từ chối":
                                            TrangThai = "3";
                                            break;
                                    }
                                    if (TrangThai != "0" && TrangThai != "1" && TrangThai != "2" && TrangThai != "3")
                                    {
                                        sLoi = sLoi +
                                            NgonNgu.LayXau("Trạng thái: ") + TrangThai + ", " +
                                            NgonNgu.LayXau("không phù hợp ") + ", " +
                                            NgonNgu.LayXau("lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                        continue;
                                    }
                                }
                                bang.CmdParams.Parameters.AddWithValue("@iTrangThai", TrangThai);//Trạng thái
                                sTuKhoa_sTen = sTen + " ";
                                sTuKhoa_sTen += NgonNgu.LayXauKhongDauTiengViet(sTen);
                                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTuKhoa_sTen);//Từ khóa - tên

                                sTuKhoa_sTenGoc = sTenGoc + " ";
                                sTuKhoa_sTenGoc += NgonNgu.LayXauKhongDauTiengViet(sTenGoc) + " ";
                                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTenGoc", sTuKhoa_sTenGoc);//Từ khóa - tên gốc

                                sTuKhoa_sQuyCach = sQuyCach + " ";
                                sTuKhoa_sQuyCach += NgonNgu.LayXauKhongDauTiengViet(sQuyCach);
                                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sTuKhoa_sQuyCach);//Từ khóa - quy cách
                                bang.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", DateTime.Now);//Ngày phát sinh mã
                                bang.Save();

                                Bang bangls = new Bang("DM_LichSuGiaoDich");
                                bangls.MaNguoiDungSua = User.Identity.Name;
                                bangls.IPSua = Request.UserHostAddress;
                                bangls.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                                bangls.CmdParams.Parameters.AddWithValue("@sMaYeuCau", sMaYeuCau);
                                bangls.CmdParams.Parameters.AddWithValue("@sTen", sTen);//Tên vật tư
                                bangls.CmdParams.Parameters.AddWithValue("@sNhaSanXuat", NhaSanXuat);//Tên vật tư
                                bangls.CmdParams.Parameters.AddWithValue("@sMaCu", MaHeThongCu);//Mã hệ thống cũ
                                bangls.CmdParams.Parameters.AddWithValue("@sTenGoc", sTenGoc);//Tên gốc
                                bangls.CmdParams.Parameters.AddWithValue("@sQuyCach", sQuyCach);//Quy cách
                                bangls.CmdParams.Parameters.AddWithValue("@sMoTa", sMoTa);//Mô tả
                                bangls.CmdParams.Parameters.AddWithValue("@sMoTaGoc", sMoTaGoc);//Mô tả gốc
                                bangls.CmdParams.Parameters.AddWithValue("@sGhiChu", sGhiChu);
                                bangls.CmdParams.Parameters.AddWithValue("@sFileDinhKem", FileDinhKem);//File đính kèm
                                bangls.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", DateTime.Now);
                                bangls.CmdParams.Parameters.AddWithValue("@iTrangThai", TrangThai);
                                bangls.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", iDM_MaNhomLoaiVatTu);//Mã nhóm loại vật tư
                                bangls.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", iDM_MaNhomChinh);//Mã nhóm chính
                                bangls.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);//Mã nhóm phụ
                                bangls.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", iDM_MaChiTietVatTu);//Mã chi tiết vật tư
                                bangls.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", iDM_MaXuatXu);//Mã xuất xứ
                                bangls.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
                                bangls.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTuKhoa_sTen);
                                bangls.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTenGoc", sTuKhoa_sTenGoc);
                                bangls.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sTuKhoa_sQuyCach);
                                if (iID_MaDonViDangNhap != "-1")
                                    bangls.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonViDangNhap);//Mã đơn vị
                                bangls.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", bang.GiaTriKhoa);
                                bangls.CmdParams.Parameters.AddWithValue("@iHanhDong", "1");
                                bangls.DuLieuMoi = true;
                                bangls.Save();
                            }
                            else if (TableName == "DM_DonVi_TonKho")
                            {
                                sMaVatTu = dataRow[0].ToString().Trim();
                                SoLuongTonKho = KiemTraSoLuongTon(dataRow[1].ToString().Trim());

                                if (KiemTraMaVatTu(sMaVatTu) != "1")
                                {
                                    sLoi = sLoi +
                                        NgonNgu.LayXau("Mã vật tư: ") + sMaVatTu + NgonNgu.LayXau(" sai định dạng hoặc chưa có") +
                                        NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                    continue;
                                }
                                if (SoLuongTonKho == -1)
                                {
                                    sLoi = sLoi +
                                        NgonNgu.LayXau("Số lượng tồn kho: ") + dataRow[1].ToString().Trim() + NgonNgu.LayXau(" sai định dạng") +
                                        NgonNgu.LayXau(", lỗi tại dòng ") + (i + 1) + @"<br/>&nbsp;";
                                    continue;
                                }

                                cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                                cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
                                iID_MaVatTu = Connection.GetValueString(cmd, "");
                                cmd.Dispose();

                                cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                                 "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                              "FROM QT_NguoiDung " +
                                                              "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
                                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", User.Identity.Name);
                                iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
                                cmd.Dispose();

                                cmd = new SqlCommand("SELECT iID_MaTonKho FROM DM_DonVi_TonKho " +
                                                "WHERE iID_MaDonVi = @iID_MaDonVi AND iID_MaVatTu = @iID_MaVatTu");
                                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonViDangNhap);
                                cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                                iID_MaTonKho = Convert.ToString(Connection.GetValueString(cmd, ""));
                                cmd.Dispose();

                                if (iID_MaTonKho != "")
                                {
                                    bang.DuLieuMoi = false;
                                    bang.GiaTriKhoa = iID_MaTonKho;
                                }
                                else
                                {
                                    bang.DuLieuMoi = true;
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonViDangNhap);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                                }
                                bang.CmdParams.Parameters.AddWithValue("@rSoLuongTonKho", SoLuongTonKho);
                                bang.Save();
                                
                                cmd = new SqlCommand("SELECT SUM(rSoLuongTonKho) FROM DM_DonVi_TonKho WHERE iID_MaVatTu = @iID_MaVatTu");
                                cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                                SoLuongTonKho = Convert.ToDouble(Connection.GetValueString(cmd, "0"));
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
                        }

                        //String iID_MaDonVi ="";
                        //cmd = new SqlCommand("SELECT iID_MaTonKho, iID_MaVatTu, iID_MaDonVi FROM DM_DonVi_TonKho");
                        //DataTable dtTonKho = Connection.GetDataTable(cmd);
                        //for (i = 0; i < dtTonKho.Rows.Count - 1; i++)
                        //{
                        //    iID_MaTonKho = dtTonKho.Rows[i]["iID_MaTonKho"].ToString();
                        //    iID_MaDonVi = dtTonKho.Rows[i]["iID_MaDonVi"].ToString();
                        //    iID_MaVatTu = dtTonKho.Rows[i]["iID_MaVatTu"].ToString();
                        //    for (int j = 0; j < dtTonKho.Rows.Count - 1; j++)
                        //    {
                        //        if (iID_MaDonVi == dtTonKho.Rows[j]["iID_MaDonVi"].ToString() && iID_MaVatTu == dtTonKho.Rows[j]["iID_MaVatTu"].ToString() && iID_MaTonKho != dtTonKho.Rows[j]["iID_MaTonKho"].ToString())
                        //        {
                        //            Bang bangTK = new Bang("DM_DonVi_TonKho");
                        //            bangTK.GiaTriKhoa = dtTonKho.Rows[j]["iID_MaTonKho"].ToString();
                        //            bangTK.Delete();
                        //        }
                        //    }
                        //}

                        dtImportResult.ImportRow(dataRow);
                        CountRecord = CountRecord + 1;
                        //saveLog(User.Identity.Name, Request.UserHostAddress, SheetName, CountRecord, 1, DBNull.Value, startTime, DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        sLoi = ex.Message + " " + NgonNgu.LayXau("tại dòng") + " " + (i + 1);
                        //saveLog(User.Identity.Name, Request.UserHostAddress, SheetName, 0, 2, ex.Message, startTime, DateTime.Now);
                        return RedirectToAction("Result", "ImportExcelVatTu");
                    }
                }
            }
            return RedirectToAction("Result", "ImportExcelVatTu");
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
