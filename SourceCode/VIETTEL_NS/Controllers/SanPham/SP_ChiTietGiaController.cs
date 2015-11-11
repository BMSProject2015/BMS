using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;

namespace VIETTEL.Controllers.BaoHiem
{
    public class SP_ChiTietGiaController : Controller
    {
        //
        // GET: /BaoHiem_ChungTuChiChiTiet/
        public string sViewPath = "~/Views/SanPham/ChiTietGia/";
        [Authorize]
        public ActionResult Index(String ParentID, String iID_MaSanPham)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_ChiTietGia", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_LoaiDonVi"] = Request.Form[ParentID + "_iID_LoaiDonVi"];
            ViewData["iID_MaDonVi"] = Request.Form[ParentID + "_iID_MaDonVi"];
            ViewData["iID_MaLoaiHinh"] = Request.Form[ParentID + "_iID_MaLoaiHinh"];
            return View(sViewPath + "Index.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaSanPham, String iID_MaChiTietGia)
        {
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaChiTietGia"] = iID_MaChiTietGia;
            return View(sViewPath + "Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChiTietGia)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, " DM_SanPham_ChiTietGia", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaChiTietGia_Cu = "", query = "";
            SqlCommand cmd = new SqlCommand();
            String iID_MaSanPham = Request.Form[ParentID + "_iID_MaSanPham"];
            String iID_MaLoaiHinh = Request.Form[ParentID + "_iID_MaLoaiHinh"];
            String iID_LoaiDonVi = Request.Form[ParentID + "_iID_LoaiDonVi"];
            Bang bang = new Bang("DM_SanPham_ChiTietGia");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (String.IsNullOrEmpty(iID_MaChiTietGia))
            {
                bang.DuLieuMoi = true;
            }
            else
            {
                bang.DuLieuMoi = false;
                bang.GiaTriKhoa = iID_MaChiTietGia;
            }
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String iID_MaDonVi_SX =  Request.Form[ParentID + "_iID_MaDonVi_SX"];
            String iID_MaDonVi_DH = Request.Form[ParentID + "_iID_MaDonVi_DH"];
            switch (iID_LoaiDonVi)
                {
                case "1":// phieu san xuat chi luu 1 loai don vi
                    break;
                case "2":// phieu dat hang luu dv san xuat vao cho dv lien quan
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_LienQuan", iID_MaDonVi_SX);
                    break;
                case "3":// ctc luu dv dat hang
                    bang.CmdParams.Parameters["@iID_MaDonVi"].Value = iID_MaDonVi_DH;
                    // tim dv san xuat o phieu dat hang truoc do
                    query = "SELECT TOP 1 iID_MaDonVi_LienQuan FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 AND iID_LoaiDonVi = 2 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaDonVi = @iID_MaDonVi "
                              + "ORDER BY dNgayTao DESC";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                    cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi_DH);
                    iID_MaDonVi_SX = Convert.ToString(Connection.GetValue(cmd, "-1"));
                    //uu dv san xuat vao cho dv lien quan
                    if (iID_MaDonVi_SX != "-1") bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_LienQuan", iID_MaDonVi_SX);
                    break;
                }
            bang.Save();
            if (String.IsNullOrEmpty(iID_MaChiTietGia))
            {
                iID_MaChiTietGia = SanPham_DanhMucGiaModels.Get_MaxId_ChiTietGia();
                int ThuTu = 0;
                switch (iID_LoaiDonVi)
                {
                    case "1":
                        //tao moi thi phai copy tu ben danh muc sang
                        query = "SELECT TOP 1 iID_MaChiTietGia, iID_LoaiDonVi FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 "
                              + "AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaDonVi_LienQuan = @iID_MaDonVi_LienQuan AND iID_MaChiTietGia < @iID_MaChiTietGia "
                              + "ORDER BY dNgayTao DESC";
                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                        cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                        cmd.Parameters.AddWithValue("@iID_MaDonVi_LienQuan", iID_MaDonVi);
                        cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                        DataTable ChiTietCu = Connection.GetDataTable(cmd);
                        if (ChiTietCu.Rows.Count > 0)
                        {
                            iID_MaChiTietGia_Cu = Convert.ToString(ChiTietCu.Rows[0]["iID_MaChiTietGia"]);
                            iID_LoaiDonVi = Convert.ToString(ChiTietCu.Rows[0]["iID_LoaiDonVi"]);
                            SanPham_DanhMucGiaModels.CopyDanhMucTheoPhieu2(iID_LoaiDonVi, iID_MaChiTietGia_Cu, iID_MaChiTietGia, "", "0", User.Identity.Name, Request.UserHostAddress, 0, ref ThuTu);
                        }else{
                            SanPham_DanhMucGiaModels.CopyDanhMucTheoCauHinh(iID_MaSanPham, iID_MaLoaiHinh, iID_MaChiTietGia, "", "0", User.Identity.Name, Request.UserHostAddress, 0, ref ThuTu);
                        }
                        break;
                    case "2":
                        //tao moi thi phai copy tu ben phieu cua dv san xuat sang 
                        query = "SELECT TOP 1 iID_MaChiTietGia FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 AND iID_LoaiDonVi = 1 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaDonVi = @iID_MaDonVi "
                              + "ORDER BY dNgayTao DESC";
                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                        cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi_SX);
                        iID_MaChiTietGia_Cu = Convert.ToString(Connection.GetValue(cmd, "-1"));
                        SanPham_DanhMucGiaModels.CopyDanhMucTheoPhieu(iID_LoaiDonVi, iID_MaChiTietGia_Cu, iID_MaChiTietGia, "", "0", User.Identity.Name, Request.UserHostAddress, 0, ref ThuTu);
                        break;
                    case "3":
                        //tao moi thi phai copy tu ben phieu cua dv dat hang sang 
                        query = "SELECT TOP 1 iID_MaChiTietGia FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 AND iID_LoaiDonVi = 2 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaDonVi = @iID_MaDonVi "
                                  + "ORDER BY dNgayTao DESC";
                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                        cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi_DH);
                        iID_MaChiTietGia_Cu = Convert.ToString(Connection.GetValue(cmd, "-1"));
                        SanPham_DanhMucGiaModels.CopyDanhMucTheoPhieu(iID_LoaiDonVi, iID_MaChiTietGia_Cu, iID_MaChiTietGia, "", "0", User.Identity.Name, Request.UserHostAddress, 0, ref ThuTu);
                        break;
                }
            }
            return RedirectToAction("ChiTiet", new { iID_MaSanPham = iID_MaSanPham, iID_MaChiTietGia = iID_MaChiTietGia, iID_LoaiDonVi = iID_LoaiDonVi });
        }
        public ActionResult Delete(String iID_MaChiTietGia, String iID_MaSanPham)
        {
            Bang bang = new Bang("DM_SanPham_ChiTietGia");
            bang.GiaTriKhoa = iID_MaChiTietGia;
            bang.Delete();
            return RedirectToAction("Index", new { iID_MaSanPham = iID_MaSanPham });
        }
        [Authorize]
        public ActionResult ChiTiet(String iID_MaSanPham, String iID_MaChiTietGia)
        {
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaChiTietGia"] = iID_MaChiTietGia;
            return View(sViewPath + "ChiTiet_Index.aspx");
        }
        [Authorize]
        public ActionResult ChiTiet_NS(String iID_MaSanPham, String iID_MaChiTietGia)
        {
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaChiTietGia"] = iID_MaChiTietGia;
            return View(sViewPath + "NS_ChiTiet_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaSanPham, String iID_MaChiTietGia, String iID_LoaiDonVi)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                Boolean okCoThayDoi = false;
                for (int j = 0; j < arrMaCot.Length; j++)
                {
                    if (arrThayDoi[j] == "1")
                    {
                        okCoThayDoi = true;
                        break;
                    }
                }
                if (okCoThayDoi)
                {
                    Bang bang = new Bang("DM_SanPham_DanhMucGia");
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    //Them tham so

                    Double TongSo = 0;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            String Truong = "@" + arrMaCot[j];
                            if (arrMaCot[j].StartsWith("b"))
                            {
                                //Nhap Kieu checkbox
                                if (arrGiaTri[j] == "1")
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                }
                                else
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                }
                            }
                            else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                            {
                                //Nhap Kieu so
                                if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                }
                            }
                            else
                            {
                                //Nhap kieu xau
                                bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                            }
                        }
                    }              
                    bang.Save();
                }
            }
            return RedirectToAction("ChiTiet", "SP_ChiTietGia", new { iID_MaChiTietGia = iID_MaChiTietGia, iID_MaSanPham = iID_MaSanPham, iID_LoaiDonVi = iID_LoaiDonVi});
        }
        public class Data
        {
            public Decimal rSoLuong { get; set; }
            public Decimal rLoiNhuan { get; set; }
            public Decimal rThueGTGT { get; set; }
            public String sQuyCach { get; set; }
        }
        [HttpGet]
        public JsonResult Get_dtThongTinChiTietGia(string ParentID, String iID_LoaiDonVi, String iID_MaDonVi, String iID_MaSanPham, String iID_MaLoaiHinh)
        {

            return Json(obj_dtThongTinChiTietGia(ParentID, iID_LoaiDonVi, iID_MaDonVi, iID_MaSanPham, iID_MaLoaiHinh), JsonRequestBehavior.AllowGet);
        }

        public Data obj_dtThongTinChiTietGia(string ParentID, String iID_LoaiDonVi, String iID_MaDonVi, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            Data _data = new Data();
            DataTable dt = new DataTable();
            String query = "";
            SqlCommand cmd = new SqlCommand();
            switch (iID_LoaiDonVi)
            {
                case "1":
                    //don vi san xuat thi ko thay doi
                    break;
                case "2":
                    //don vi dat hang copy tu ben phieu cua dv san xuat sang 
                    query = "SELECT TOP 1 * FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 AND iID_LoaiDonVi = 1 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaDonVi = @iID_MaDonVi "
                          + "ORDER BY dNgayTao DESC";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                    cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    dt = Connection.GetDataTable(cmd);
                    break;
                case "3":
                    //tao moi thi phai copy tu ben phieu cua dv dat hang sang 
                    query = "SELECT TOP 1 * FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 AND iID_LoaiDonVi = 2 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh AND iID_MaDonVi = @iID_MaDonVi "
                              + "ORDER BY dNgayTao DESC";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                    cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    dt = Connection.GetDataTable(cmd);
                    break;
            }
            if (dt.Rows.Count>0)
            {
                _data.rSoLuong =Convert.ToDecimal(dt.Rows[0]["rSoLuong"]);
                _data.rLoiNhuan = Convert.ToDecimal(dt.Rows[0]["rLoiNhuan"]);
                _data.rThueGTGT = Convert.ToDecimal(dt.Rows[0]["rThueGTGT"]);
                _data.sQuyCach =Convert.ToString( dt.Rows[0]["sQuyCach"]);
            }
            return _data;
        }
    }
}
