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
    public class DungChungController : Controller
    {
        //
        // GET: /DungChung/

        public ActionResult Index()
        {
            return View();
        }

        public static String LayMa(String Ten)
        {
            String Ma ="";
            if (Ten.Length > 0)
                if (Ten.IndexOf("_") > 0)// lấy thêm ký tự
                Ma  = Ten.Substring(0 , Ten.IndexOf("_"));

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

            CVatTu.MaVatTu =  LayMa(MaNhomLoaiVatTu) + LayMa(MaNhomChinh) + LayMa(MaNhomPhu) + MaChiTietVatTu + LayMa(MaXuatXu);
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

            if (!string.IsNullOrEmpty(iDM_MaNhomLoaiVatTu))
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomLoaiVatTu + "'";
            else
                iDM_MaNhomChinh = "";

            if (string.IsNullOrEmpty(iDM_MaNhomLoaiVatTu))
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
                DataRow R = dt.NewRow();
                dt.Rows.InsertAt(R, 0);
            }
            else
            {
                cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    DataRow R = dt.NewRow();
                    dt.Rows.InsertAt(R, 0);   
                }
            }
            SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strNhomChinh = MyHtmlHelper.DropDownList(ParentID, slNhomChinh, iDM_MaNhomChinh, "iDM_MaNhomChinh", "", "onchange=\"ChonNhomChinh(this.value)\" style=\"width: 100%;\"");
            

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

            if (!string.IsNullOrEmpty(iDM_MaNhomChinh))
            {
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomChinh + "'";
                if (iDM_MaNhomChinh == "null")
                    tg = "1";
            }
            else
                iDM_MaNhomPhu = "";

            if (tg == "0")
            {
                if (string.IsNullOrEmpty(iDM_MaNhomChinh))
                {
                    //cmd = new SqlCommand("SELECT DISTINCT '' AS iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                    //"WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                    //                                            "FROM DC_LoaiDanhMuc " +
                    //                                            "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
                    //dt = Connection.GetDataTable(cmd);
                    //cmd.Dispose();

                    //if (dt.Rows.Count > 0)
                    //{
                    //    DataRow R = dt.NewRow();
                    //    if (ParentID == "Index") R[1] = "---All---";
                    //    dt.Rows.InsertAt(R, 0);
                    //}

                    dt = new DataTable();
                    dt.Columns.Add("iID_MaDanhMuc");
                    dt.Columns.Add("sTenKhoa");
                    DataRow R = dt.NewRow();
                    //if (ParentID == "Index") R[1] = "---All---";
                    dt.Rows.InsertAt(R, 0);
                }
                else
                {
                    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow R = dt.NewRow();
                        dt.Rows.InsertAt(R, 0);
                    }
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
            }
            SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strNhomPhu = MyHtmlHelper.DropDownList(ParentID, slNhomPhu, iDM_MaNhomPhu, "iDM_MaNhomPhu", "", "onchange=\"timeGepMaVatTu();\" onfocus=\"timeGepMaVatTu();\" onblur=\"timeGepMaVatTu();\" style=\"width: 100%;\"");

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

            if (!string.IsNullOrEmpty(iDM_MaNhomPhu))
            {
                DK = " AND iID_MaDanhMucCha = '" + iDM_MaNhomPhu + "'";
                if (iDM_MaNhomPhu == "null")
                    tg = "1";
            }
            else
                iDM_MaChiTietVatTu = "";

            if (tg == "0")
            {
                if (string.IsNullOrEmpty(iDM_MaNhomPhu))
                {
                    dt = new DataTable();
                    dt.Columns.Add("iID_MaDanhMuc");
                    dt.Columns.Add("sTenKhoa");
                    DataRow R = dt.NewRow();
                    dt.Rows.InsertAt(R, 0);
                }
                else
                {
                    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow R = dt.NewRow();
                        dt.Rows.InsertAt(R, 0);
                    }
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
            }

            SelectOptionList slChiTietVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
            strChiTietVatTu = MyHtmlHelper.DropDownList(ParentID, slChiTietVatTu, iDM_MaChiTietVatTu, "iDM_MaChiTietVatTu", "", "onchange=\"ChonMa()\" style=\"width: 100%;\"");

            return strChiTietVatTu;
        }

        //Lấy thông tin bạn hàng
        public JsonResult get_Auto_ChiTietVatTu_sTen(String term, String term1)
        {
            return get_Auto_ChiTietVatTu(term, term1);
        }
        public JsonResult get_Auto_ChiTietVatTu(String term, String term1)
        {
            List<Object> list = new List<Object>();
            String DK = String.Empty;
            String tg = "0";
            DataTable dt = null;
            SqlCommand cmd;
            int i;

            if (!string.IsNullOrEmpty(term1))
            {
                DK = " AND iID_MaDanhMucCha = '" + term1 + "' AND sTenKhoa LIKE '%" + term + "%'";
                if (term1 == "null")
                    tg = "1";
            }

            if (tg == "0")
            {
                if (string.IsNullOrEmpty(term1))
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
                    "WHERE bHoatDong = 1 AND bDangDung = 0" + DK + " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                "FROM DC_LoaiDanhMuc " +
                                                                "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow R = dt.NewRow();
                        R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
                        R["sTenKhoa"] = "-- Chi tiết vật tư --";
                        dt.Rows.InsertAt(R, 0);
                    }
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("iID_MaDanhMuc");
                dt.Columns.Add("sTenKhoa");
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    label = CommonFunction.ValueToString(dt.Rows[i]["sTenKhoa"]),
                    value = CommonFunction.ValueToString(dt.Rows[i]["iID_MaDanhMuc"]),
                    //iID_MaDanhMuc = CommonFunction.ValueToString(dt.Rows[i]["iID_MaDanhMuc"]),
                    //sTenKhoa = CommonFunction.ValueToString(dt.Rows[i]["sTenKhoa"]),
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult get_dtMaChiTietVatTu(String ParentID, String iDM_MaNhomPhu)
        {
            return get_objMaChiTietVatTu(ParentID, iDM_MaNhomPhu);
        }

        public JsonResult get_objMaChiTietVatTu(String ParentID, String iDM_MaNhomPhu)
        {
            SqlCommand cmd = new SqlCommand("SELECT Top 1 sTenKhoa FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1 AND iID_MaDanhMucCha = @iDM_MaNhomPhu " +
                        "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                    "FROM DC_LoaiDanhMuc " +
                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa DESC");
            cmd.Parameters.AddWithValue("@iDM_MaNhomPhu", iDM_MaNhomPhu);
            String MaChiTietVatTuGoiY = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            if (MaChiTietVatTuGoiY != "")
            {
                MaChiTietVatTuGoiY = Convert.ToString(Convert.ToInt32(MaChiTietVatTuGoiY) + 1);
                while (MaChiTietVatTuGoiY.Length < 5)
                    MaChiTietVatTuGoiY = MaChiTietVatTuGoiY.Insert(0, "0");
            }
            else
                MaChiTietVatTuGoiY = "00000";

            //return MaChiTietVatTuGoiY;
            return Json(MaChiTietVatTuGoiY, JsonRequestBehavior.AllowGet);
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
                    context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName[FileName.Length-1].ToString());
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

        //Thêm bản ghi vào CSDL để test
        //public void Insert()
        //{
            //int i, j, z, t, k;
            //String sTen;
            //String iID_MaLoaiDanhMuc, sTenKhoa, iID_MaDanhMucCha;

            //for (i = 5; i < 91; i++)
            //{
            //    Bang bang = new Bang("DC_DanhMuc");
            //    bang.MaNguoiDungSua = User.Identity.Name;
            //    bang.IPSua = Request.UserHostAddress;

            //    if (i < 10)
            //        sTenKhoa = "0" + i.ToString();
            //    else
            //        sTenKhoa = i.ToString();

            //    sTen = "Tình trạng vật tư " + i;
            //    iID_MaLoaiDanhMuc = "f743a3a7-9003-4e4e-ae70-50d0979304df";

            //    bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);
            //    bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
            //    bang.CmdParams.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
            //    bang.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
            //    bang.DuLieuMoi = true;
            //    bang.Save();
            //}


            //SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMuc FROM DC_DanhMuc " +
            //            "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
            //                                                        "FROM DC_LoaiDanhMuc " +
            //                                                        "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
            //DataTable dt = Connection.GetDataTable(cmd);
            //for (i = 0; i < dt.Rows.Count - 1; i++)
            //{
            //    for (j = 3; j < 6; j++)
            //    {
            //        Bang bang = new Bang("DC_DanhMuc");
            //        bang.MaNguoiDungSua = User.Identity.Name;
            //        bang.IPSua = Request.UserHostAddress;

            //        sTenKhoa = "000" + j.ToString();
            //        sTen = "Chi tiết vật tư 000" + j;
            //        iID_MaLoaiDanhMuc = "4cdb608e-e9cc-42c2-bec2-f4172baa503e";
            //        iID_MaDanhMucCha = dt.Rows[i]["iID_MaDanhMuc"].ToString();

            //        bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);
            //        bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
            //        bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucCha", iID_MaDanhMucCha);
            //        bang.CmdParams.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
            //        bang.CmdParams.Parameters.AddWithValue("@bHoatDong", "1");
            //        bang.DuLieuMoi = true;
            //        bang.Save();
            //    }
            //}


            //String sMaVatTu, sTenGoc, sQuyCach;
            //SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
            //            "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
            //                                                        "FROM DC_LoaiDanhMuc " +
            //                                                        "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
            //DataTable dtNhomLoaiVatTu = Connection.GetDataTable(cmd);
            //for (i = 0; i < dtNhomLoaiVatTu.Rows.Count - 1; i++)
            //{
            //    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
            //            "WHERE bHoatDong = 1 AND iID_MaDanhMucCha = @iID_MaDanhMucCha " +
            //                        "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
            //                                                "FROM DC_LoaiDanhMuc " +
            //                                                "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
            //    cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", dtNhomLoaiVatTu.Rows[i]["iID_MaDanhMuc"].ToString());
            //    DataTable dtNhomChinh = Connection.GetDataTable(cmd);
            //    for (j = 0; j < dtNhomChinh.Rows.Count - 1; j++)
            //    {
            //        cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
            //           "WHERE bHoatDong = 1 AND iID_MaDanhMucCha = @iID_MaDanhMucCha " +
            //                       "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
            //                                               "FROM DC_LoaiDanhMuc " +
            //                                               "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
            //        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", dtNhomChinh.Rows[j]["iID_MaDanhMuc"].ToString());
            //        DataTable dtNhomPhu = Connection.GetDataTable(cmd);
            //        for (z = 0; z < dtNhomPhu.Rows.Count - 1; z++)
            //        {
            //            cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
            //           "WHERE bHoatDong = 1 AND iID_MaDanhMucCha = @iID_MaDanhMucCha " +
            //                "AND (sTenKhoa = '0003' OR sTenKhoa = '0005' OR sTenKhoa = '0004') " +
            //                       "AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
            //                                               "FROM DC_LoaiDanhMuc " +
            //                                               "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
            //            cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", dtNhomPhu.Rows[z]["iID_MaDanhMuc"].ToString());
            //            DataTable dtChiTietVatTu = Connection.GetDataTable(cmd);
            //            for (t = 0; t < dtChiTietVatTu.Rows.Count - 1; t++)
            //            {
            //                cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
            //                    "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
            //                                                                "FROM DC_LoaiDanhMuc " +
            //                                                                "WHERE sTenBang = 'XuatXu') ORDER BY sTenKhoa");
            //                DataTable dtXuatXu = Connection.GetDataTable(cmd);
            //                for (k = 0; k < dtXuatXu.Rows.Count - 1; k++)
            //                {
            //                    Bang bang = new Bang("DM_VatTu");
            //                    bang.MaNguoiDungSua = User.Identity.Name;
            //                    bang.IPSua = Request.UserHostAddress;
            //                    sMaVatTu = dtNhomLoaiVatTu.Rows[i]["sTenKhoa"].ToString() + dtNhomChinh.Rows[j]["sTenKhoa"].ToString() + dtNhomPhu.Rows[z]["sTenKhoa"].ToString() + dtChiTietVatTu.Rows[t]["sTenKhoa"].ToString() + dtXuatXu.Rows[k]["sTenKhoa"].ToString();
            //                    sTen = "Mã vật tư BQP " + i + j + z + t + k + " tạo lúc " + DateTime.Now.ToString();
            //                    sTenGoc = "material code " + i + j + z + t + k;
            //                    sQuyCach = (i * j) + "x" + (z * t) + "x" + (k * i);

            //                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomLoaiVatTu", dtNhomLoaiVatTu.Rows[i]["iID_MaDanhMuc"].ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomChinh", dtNhomChinh.Rows[j]["iID_MaDanhMuc"].ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaNhomPhu", dtNhomPhu.Rows[z]["iID_MaDanhMuc"].ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaChiTietVatTu", dtChiTietVatTu.Rows[t]["iID_MaDanhMuc"].ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaXuatXu", dtXuatXu.Rows[k]["iID_MaDanhMuc"].ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", "75996E04-AC3F-4DF3-A1B7-A08484DBF1CA");
            //                    bang.CmdParams.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
            //                    bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);
            //                    bang.CmdParams.Parameters.AddWithValue("@sTenGoc", sTenGoc);
            //                    bang.CmdParams.Parameters.AddWithValue("@sQuyCach", sQuyCach);
            //                    bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);
            //                    bang.CmdParams.Parameters.AddWithValue("@dNgayPhatSinhMa", DateTime.Now.ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@dNgayCapNhatTonKho", DateTime.Now.ToString());
            //                    bang.CmdParams.Parameters.AddWithValue("@rSoLuongTonKho", k * t);

            //                    String sTuKhoa_sTen = sTen + " ";
            //                    sTuKhoa_sTen += NgonNgu.LayXauKhongDauTiengViet(sTen);
            //                    bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTen", sTuKhoa_sTen);

            //                    String sTuKhoa_sTenGoc = sTenGoc + " ";
            //                    sTuKhoa_sTenGoc += NgonNgu.LayXauKhongDauTiengViet(sTenGoc) + " ";
            //                    bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sTenGoc", sTuKhoa_sTenGoc);

            //                    String sTuKhoa_sQuyCach = sQuyCach + " ";
            //                    sTuKhoa_sQuyCach += NgonNgu.LayXauKhongDauTiengViet(sQuyCach);
            //                    bang.CmdParams.Parameters.AddWithValue("@sTuKhoa_sQuyCach", sTuKhoa_sQuyCach);

            //                    bang.DuLieuMoi = true;
            //                    bang.Save();
            //                }
            //            }
            //        }
            //    }
            //}


            //String sMaVatTu, iID_MaVatTu;
            //SqlCommand cmd = new SqlCommand("SELECT iID_MaVatTu, sMaVatTu FROM DM_VatTu");
            //DataTable dtVatTu = Connection.GetDataTable(cmd);
            //for (i = 0; i < dtVatTu.Rows.Count - 1; i++)
            //{
            //    sMaVatTu = dtVatTu.Rows[i]["sMaVatTu"].ToString();
            //    iID_MaVatTu = dtVatTu.Rows[i]["iID_MaVatTu"].ToString();
            //    for (j = 0; j < dtVatTu.Rows.Count - 1; j++)
            //    {
            //        if (sMaVatTu == dtVatTu.Rows[j]["sMaVatTu"].ToString() && iID_MaVatTu != dtVatTu.Rows[j]["iID_MaVatTu"].ToString())
            //        {
            //            Bang bang = new Bang("DM_VatTu");
            //            bang.GiaTriKhoa = dtVatTu.Rows[j]["iID_MaVatTu"].ToString();
            //            bang.Delete();
            //        }
            //    }
            //}
        //}
    }
}
