using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class LoaiTaiKhoanController : Controller
    {
        //
        // GET: /LoaiTaiKhoan/

        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/TaiKhoan/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "TaiKhoan_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaTaiKhoan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", new { iID_MaTaiKhoan_Cha = iID_MaTaiKhoan_Cha });
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaTaiKhoan, String iID_MaTaiKhoan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaTaiKhoan))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["iID_MaTaiKhoan_Cha"] = iID_MaTaiKhoan_Cha;
            return View(sViewPath + "TaiKhoan_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaTaiKhoan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            
            NameValueCollection arrLoi = new NameValueCollection();
            String siID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaTaiKhoan_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan_Cha"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
            String iLoaiTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iLoaiTaiKhoan"]);
            String iCapTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iCapTaiKhoan"]);
            String bHienThi = Convert.ToString(Request.Form[ParentID + "_bHienThi"]);
            String bLaHangCha = Convert.ToString(Request.Form[ParentID + "_bLaHangCha"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            if (siID_MaTaiKhoan == string.Empty || siID_MaTaiKhoan == "")
            {
                arrLoi.Add("err_iID_MaTaiKhoan", MessageModels.sKyHieu);
            }
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", MessageModels.sTen);
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (CheckMaTaiKhoan(siID_MaTaiKhoan) == true)
                {
                    arrLoi.Add("err_iID_MaTaiKhoan", "Mã tài khoản đã tồn tại!");
                }
            }   
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaTaiKhoan))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
                ViewData["iID_MaTaiKhoan_Cha"] = iID_MaTaiKhoan_Cha;
                return View(sViewPath + "TaiKhoan_Edit.aspx");
            }
            else
            {
                //Bang bang = new Bang("KT_TaiKhoan");
                //bang.MaNguoiDungSua = User.Identity.Name;
                //bang.IPSua = Request.UserHostAddress;
                //bang.TruyenGiaTri(ParentID, Request.Form);
              
                //if (DuLieuMoi == "1")
                //{
                //    string SQL = "SELECT  MAX(iSTT) AS  iSTT FROM KT_TaiKhoan WHERE 1=1";
                //    cmd = new SqlCommand();
                //    if (iID_MaTaiKhoan_Cha != null && iID_MaTaiKhoan_Cha != "")
                //    {
                //        SQL += " AND iID_MaTaiKhoan_Cha=@iID_MaTaiKhoan_Cha";
                //        cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Cha", iID_MaTaiKhoan_Cha);
                //    }
                //    cmd.CommandText = SQL;
                //    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                //    cmd.Dispose();
                //    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                //}
                //bang.GiaTriKhoa = iID_MaTaiKhoan;
                //bang.TruongKhoa = "iID_MaTaiKhoan_Khoa";
                //bang.TruongKhoaKieuSo = true;
                ////bang.GiaTriKhoa = siID_MaTaiKhoan;
                ////bang.TruongKhoa = "iID_MaTaiKhoan";
                //if (iID_MaTaiKhoan_Cha == null || iID_MaTaiKhoan_Cha == "")
                //{                 
                //    bang.CmdParams.Parameters["@iID_MaTaiKhoan_Cha"].Value = TaiKhoanModels.LayTaiKhoanCha(iID_MaTaiKhoan);
                //}               
                //bang.CmdParams.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(User.Identity.Name));
                //bang.Save();

                if (DuLieuMoi == "1")
                {
                    string TaiKhoanCha = TaiKhoanModels.LayTaiKhoanCha(iID_MaTaiKhoan);
                    string sTaiKhoanCha = iID_MaTaiKhoan_Cha;
                    SqlCommand cmd;
                    Bang bang = new Bang("KT_TaiKhoan");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);

                    string SQL = "SELECT  MAX(iSTT) AS  iSTT FROM KT_TaiKhoan WHERE 1=1";
                    cmd = new SqlCommand();
                    if (iID_MaTaiKhoan_Cha != null && iID_MaTaiKhoan_Cha != "")
                    {
                        SQL += " AND iID_MaTaiKhoan_Cha=@iID_MaTaiKhoan_Cha";
                        cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Cha", iID_MaTaiKhoan_Cha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);

                    if (iID_MaTaiKhoan_Cha == null || iID_MaTaiKhoan_Cha == "")
                    {
                        bang.CmdParams.Parameters["@iID_MaTaiKhoan_Cha"].Value = TaiKhoanCha;
                        sTaiKhoanCha = TaiKhoanCha;
                    }
                    bang.CmdParams.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(User.Identity.Name));
                    bang.Save();
                    cmd.Dispose();
                    if (String.IsNullOrEmpty(sTaiKhoanCha) == false)
                    {
                        TaiKhoanModels.UpdateTaiKhoan_LaHangCha(sTaiKhoanCha);
                    }
                }
                else
                {
                    Boolean HienThi = false;
                    Boolean LaHangCha = false;
                    if (bHienThi == "on") HienThi = true;
                    if (bLaHangCha == "on") LaHangCha = true;
                    String SQL = "UPDATE KT_TaiKhoan SET iID_MaTaiKhoan=@iID_MaTaiKhoan, sTen=@sTen, iID_MaTaiKhoan_Cha=@iID_MaTaiKhoan_Cha, sMoTa=@sMoTa," +
                        " iLoaiTaiKhoan=@iLoaiTaiKhoan,iCapTaiKhoan=@iCapTaiKhoan, bHienThi=@bHienThi, bLaHangCha=@bLaHangCha WHERE iID_MaTaiKhoan=@iID_MaTaiKhoan AND iNam=@iNam";
                    SqlCommand cmdSQL = new SqlCommand();
                    cmdSQL.CommandText = SQL;
                    cmdSQL.Parameters.AddWithValue("@iID_MaTaiKhoan", siID_MaTaiKhoan);
                    cmdSQL.Parameters.AddWithValue("@sTen", sTen);                  
                    if (iID_MaTaiKhoan_Cha == null || iID_MaTaiKhoan_Cha == "")
                    {                       
                        cmdSQL.Parameters.AddWithValue("@iID_MaTaiKhoan_Cha", TaiKhoanModels.LayTaiKhoanCha(iID_MaTaiKhoan));
                    }
                    else cmdSQL.Parameters.AddWithValue("@iID_MaTaiKhoan_Cha", iID_MaTaiKhoan_Cha);
                    cmdSQL.Parameters.AddWithValue("@sMoTa", sMoTa);
                    cmdSQL.Parameters.AddWithValue("@iLoaiTaiKhoan", Convert.ToInt32(iLoaiTaiKhoan));
                    cmdSQL.Parameters.AddWithValue("@iCapTaiKhoan", Convert.ToInt32(iCapTaiKhoan));
                    cmdSQL.Parameters.AddWithValue("@bHienThi", HienThi);
                    cmdSQL.Parameters.AddWithValue("@bLaHangCha", LaHangCha);
                    cmdSQL.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(User.Identity.Name));
                    Connection.UpdateDatabase(cmdSQL);
                    cmdSQL.Dispose();
                }
                return RedirectToAction("Edit", new { iID_MaTaiKhoan_Cha = iID_MaTaiKhoan_Cha });
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaTaiKhoan_Cha"></param>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaTaiKhoan_Cha, String iID_MaTaiKhoan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaTaiKhoan_Cha))
            {
                iID_MaTaiKhoan_Cha = "";
            }
            ViewData["iID_MaTaiKhoan_Cha"] = iID_MaTaiKhoan_Cha;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            return View(sViewPath + "TaiKhoan_Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaTaiKhoan_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaTaiKhoan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("KT_TaiKhoan");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaTaiKhoan_Cha = iID_MaTaiKhoan_Cha });
        }
        /// <summary>
        /// Lệnh điều hướng xóa tài khoản kế toán
        /// </summary>
        /// <param name="iID_MaTaiKhoan">Mã tài khoản</param>
        /// <param name="iID_MaTaiKhoan_Cha">Mã tài khoản cấp cha</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaTaiKhoan, String iID_MaTaiKhoan_Cha)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = false;
            if (TaiKhoanModels.CheckTaiKhoan(iID_MaTaiKhoan))
            {
                //return;
            }
            else
            {
                bDelete = TaiKhoanModels.DeleteTaiKhoan(iID_MaTaiKhoan);
            }
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaTaiKhoan_Cha = iID_MaTaiKhoan_Cha });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Tìm kiếm loại tài khoản
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String sTaiKhoan = Request.Form[ParentID + "_sTaiKhoan"];
            String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
            return RedirectToAction("Index", "LoaiTaiKhoan", new { ParentID = ParentID, Ten = sTaiKhoan, KyHieu = sKyHieu });
        }

        public Boolean CheckMaTaiKhoan(String iID_MaTaiKhoan)
        {
            Boolean vR = false;
            DataTable dt = TaiKhoanModels.getChiTietTK(iID_MaTaiKhoan);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            return vR;
        }
        private DataTable LayTaiKhoanCon(String iID_MaTaiKhoan, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT iID_MaTaiKhoan, sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iID_MaTaiKhoan_Cha=@iID_MaTaiKhoan AND iNam=@iNam ORDER BY iSTT, sTen ASC");
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            if (vR != null || vR.Rows.Count==0)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = "-1";
                R["sTen"] = "Không có tài khoản con";
                vR.Rows.InsertAt(R, 0);
            }

            cmd.Dispose();          
            return vR;
        }
        /// <summary>
        /// Lấy tài khoản cấp con
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public JsonResult GetTaiKhoanCapCon(String iID_MaTaiKhoan, String iNam)
        {
            var dt = LayTaiKhoanCon(iID_MaTaiKhoan, iNam);
            JsonResult value = Json(HamChung.getGiaTri("iID_MaTaiKhoan", "sTen", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
    }
}
