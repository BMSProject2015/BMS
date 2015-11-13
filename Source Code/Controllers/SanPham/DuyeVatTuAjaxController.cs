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
    public class DuyeVatTuAjaxController : Controller
    {
        //
        // GET: /DungChung/

        public ActionResult Index()
        {
            return View();
        }

        public static String LayMa(String Ten)
        {
            String Ma = "";
            if (Ten.Length > 0)
                if (Ten.IndexOf("_") > 0)// lấy thêm ký tự
                    Ma = Ten.Substring(0, Ten.IndexOf("_"));

            return Ma;
        }

        public class VatTu
        {
            public String MaVatTu = "";
            public String Loi = "0";
        }

        public JsonResult get_dtMaVatTu(String ParentID, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iID_MaVatTu)
        {
            return Json(get_objMaVatTu(ParentID, MaNhomLoaiVatTu, MaNhomChinh, MaNhomPhu, MaChiTietVatTu, MaXuatXu, iID_MaVatTu), JsonRequestBehavior.AllowGet);
        }

        public static VatTu get_objMaVatTu(String ParentID, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iID_MaVatTu)
        {
            VatTu CVatTu = new VatTu();

            CVatTu.MaVatTu = LayMa(MaNhomLoaiVatTu) + LayMa(MaNhomChinh) + LayMa(MaNhomPhu) + LayMa(MaChiTietVatTu) + LayMa(MaXuatXu);
            //CVatTu.MaVatTu = CVatTu.MaVatTu.Replace("---All---", "");
            if (CVatTu.MaVatTu != "")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT iID_MaVatTu FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
                    cmd.Parameters.AddWithValue("@sMaVatTu", CVatTu.MaVatTu);
                    String iID_MaVatTuDaCo = Connection.GetValueString(cmd, "");
                    if (iID_MaVatTuDaCo != "" && iID_MaVatTuDaCo != iID_MaVatTu)
                        CVatTu.Loi = "1";//Trùng mã vật tư
                }
                catch { }
            }
            return CVatTu;
        }

        public class NhomChinh
        {
            public String ddlNhomChinh;
            public String iDM_MaNhomChinh;
        }

        public JsonResult get_dtNhomChinh(String ParentID, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh)
        {
            return Json(get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh), JsonRequestBehavior.AllowGet);
        }

        public static NhomChinh get_objNhomChinh(String ParentID, String iDM_MaNhomLoaiVatTu, String iDM_MaNhomChinh)
        {
            String strNhomChinh = String.Empty;
            String DK = String.Empty;
            DataTable dt = null;
            SqlCommand cmd;
            if (iDM_MaNhomLoaiVatTu == "") iDM_MaNhomLoaiVatTu = "dddddddd-dddd-dddd-dddd-dddddddddddd";

            if (!string.IsNullOrEmpty(iDM_MaNhomLoaiVatTu) && iDM_MaNhomLoaiVatTu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomLoaiVatTu + "'";
            else
                iDM_MaNhomChinh = "";

            if (iDM_MaNhomLoaiVatTu == "dddddddd-dddd-dddd-dddd-dddddddddddd")
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
                DataRow R = dt.NewRow();
                R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                R["sTenKhoa"] = "-- Nhóm chính --";
                dt.Rows.InsertAt(R, 0);
            }
            else
            {
                cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
                dt = Connection.GetDataTable(cmd);
                DataRow R = dt.NewRow();
                R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                R["sTenKhoa"] = "-- Nhóm chính --";
                dt.Rows.InsertAt(R, 0);
                cmd.Dispose();
            }
            SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strNhomChinh = MyHtmlHelper.DropDownList(ParentID, slNhomChinh, iDM_MaNhomChinh, "iDM_MaNhomChinh", "", "onchange=\"ChonNhomChinh_Search(this.value)\" style=\"width: 100%;\"");


            NhomChinh _NhomChinh = new NhomChinh();
            _NhomChinh.ddlNhomChinh = strNhomChinh;
            if (dt.Rows.Count > 0)
                _NhomChinh.iDM_MaNhomChinh = Convert.ToString(dt.Rows[0]["iID_MaDanhMuc"]);

            return _NhomChinh;
        }

        public class NhomPhu
        {
            public String ddlNhomPhu;
            public String iDM_MaNhomPhu;
        }

        public JsonResult get_dtNhomPhu(String ParentID, String iDM_MaNhomChinh, String iDM_MaNhomPhu)
        {
            return Json(get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu), JsonRequestBehavior.AllowGet);
        }

        public static NhomPhu get_objNhomPhu(String ParentID, String iDM_MaNhomChinh, String iDM_MaNhomPhu)
        {
            String strNhomPhu = String.Empty;
            String DK = String.Empty;
            String tg = "0";
            DataTable dt = null;
            SqlCommand cmd;
            if (iDM_MaNhomChinh == "") iDM_MaNhomChinh = "dddddddd-dddd-dddd-dddd-dddddddddddd";

            if (!string.IsNullOrEmpty(iDM_MaNhomChinh) && iDM_MaNhomChinh != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            {
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomChinh + "'";
                if (iDM_MaNhomChinh == "null")
                    tg = "1";
            }
            else
                iDM_MaNhomPhu = "";

            if (tg == "0")
            {
                if (iDM_MaNhomChinh == "dddddddd-dddd-dddd-dddd-dddddddddddd")
                {
                    dt = new DataTable();
                    dt.Columns.Add("iID_MaDanhMuc");
                    dt.Columns.Add("sTenKhoa");
                    DataRow R = dt.NewRow();
                    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                    R["sTenKhoa"] = "-- Nhóm phụ --";
                    dt.Rows.InsertAt(R, 0);
                }
                else
                {
                    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
                    dt = Connection.GetDataTable(cmd);
                    DataRow R = dt.NewRow();
                    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                    R["sTenKhoa"] = "-- Nhóm phụ --";
                    dt.Rows.InsertAt(R, 0);
                    cmd.Dispose();
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
                DataRow R = dt.NewRow();
                R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                R["sTenKhoa"] = "-- Nhóm phụ --";
                dt.Rows.InsertAt(R, 0);
            }
            SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strNhomPhu = MyHtmlHelper.DropDownList(ParentID, slNhomPhu, iDM_MaNhomPhu, "iDM_MaNhomPhu", "", "onchange=\"ChonNhomPhu_Search(this.value)\" style=\"width: 100%;\"");

            NhomPhu _NhomPhu = new NhomPhu();
            _NhomPhu.ddlNhomPhu = strNhomPhu;
            if (dt.Rows.Count > 0)
                _NhomPhu.iDM_MaNhomPhu = Convert.ToString(dt.Rows[0]["iID_MaDanhMuc"]);

            return _NhomPhu;
        }

        public JsonResult get_dtChiTietVatTu(String ParentID, String iDM_MaNhomPhu, String iDM_MaChiTietVatTu)
        {
            return Json(get_objChiTietVatTu(ParentID, iDM_MaNhomPhu, iDM_MaChiTietVatTu), JsonRequestBehavior.AllowGet);
        }

        public static String get_objChiTietVatTu(String ParentID, String iDM_MaNhomPhu, String iDM_MaChiTietVatTu)
        {
            String strChiTietVatTu = String.Empty;
            String DK = String.Empty;
            String tg = "0";
            DataTable dt = null;
            SqlCommand cmd;
            if (iDM_MaNhomPhu == "") iDM_MaNhomPhu = "dddddddd-dddd-dddd-dddd-dddddddddddd";

            if (!string.IsNullOrEmpty(iDM_MaNhomPhu) && iDM_MaNhomPhu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            {
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomPhu + "'";
                if (iDM_MaNhomPhu == "null")
                    tg = "1";
            }
            else
                iDM_MaChiTietVatTu = "";

            if (tg == "0")
            {
                if (iDM_MaNhomPhu == "dddddddd-dddd-dddd-dddd-dddddddddddd")
                {
                    dt = new DataTable();
                    dt.Columns.Add("iID_MaDanhMuc");
                    dt.Columns.Add("sTenKhoa");
                    DataRow R = dt.NewRow();
                    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                    R["sTenKhoa"] = "-- Chi tiết vật tư --";
                    dt.Rows.InsertAt(R, 0);
                }
                else
                {
                    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
                    dt = Connection.GetDataTable(cmd);
                    DataRow R = dt.NewRow();
                    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                    R["sTenKhoa"] = "-- Chi tiết vật tư --";
                    dt.Rows.InsertAt(R, 0);
                    cmd.Dispose();
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
                DataRow R = dt.NewRow();
                R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                R["sTenKhoa"] = "-- Chi tiết vật tư --";
                dt.Rows.InsertAt(R, 0);
            }
            SelectOptionList slChiTietVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strChiTietVatTu = MyHtmlHelper.DropDownList(ParentID, slChiTietVatTu, iDM_MaChiTietVatTu, "iDM_MaChiTietVatTu", "", "style=\"width: 100%;\"");

            return strChiTietVatTu;
        }


        public class clsWordResult : ActionResult
        {
            public string Path { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                try
                {
                    string[] FileName = Path.Split('\\');

                    context.HttpContext.Response.Buffer = true;
                    context.HttpContext.Response.Clear();
                    context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName[FileName.Length - 1].ToString());
                    context.HttpContext.Response.ContentType = "application/vnd.ms-word";
                    context.HttpContext.Response.WriteFile(Path);
                }
                catch { }
            }
        }


        public clsWordResult GetFile(string Path)
        {
            clsWordResult objWordResult = new clsWordResult();
            Path = Server.MapPath(Path);
            Path = Path.Replace("\\DungChung", "");
            objWordResult.Path = Path;
            return objWordResult;
        }

        public static String LayMaDonViDung(String sID_MaNguoiDung)
        {
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                                   "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                                "FROM QT_NguoiDung " +
                                                                "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
            String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return iID_MaDonViDangNhap;
        }
    }
}
