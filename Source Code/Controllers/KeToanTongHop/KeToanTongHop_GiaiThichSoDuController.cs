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
    public class KeToanTongHop_GiaiThichSoDuController : Controller
    {
        //
        // GET: /KeToanTongHop_GiaiThichSoDu/
        public string sViewPath = "~/Views/KeToanTongHop/GiaiThichSoDu/";
        public ActionResult Index(String iThang,String iNam,String iLoai)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_GiaiThichSoDu", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KeToanTongHop_GiaiThichSoDu_Index.aspx");
        }

        [Authorize]
        public ActionResult Edit(String iID_MaGiaiThich)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_GiaiThichSoDu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaGiaiThich))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaGiaiThich"] = iID_MaGiaiThich;

            return View(sViewPath + "KeToanTongHop_GiaiThichSoDu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            String iThang = Request.Form[ParentID + "_iThang"];
            String iLoai = Request.Form[ParentID + "_iLoai"];
            String iTrangThai = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iNam =Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND,"iNamLamViec"));
            String Xem = Request.Form[ParentID + "_Xem"];
            String TuDong = Request.Form[ParentID + "_TuDong"];
            String optThu = Request.Form[ParentID + "_optThu"];
            String optTamUng = Request.Form[ParentID + "_optTamUng"];
            String optTra = Request.Form[ParentID + "_optTra"];
            String KhoGiay = Request.Form[ParentID + "_KhoGiay"];
            if (TuDong == "on")
            {
                Xoa_GiaiThichSoDu(iThang, iNam, User.Identity.Name, Request.UserHostAddress);
                Them_GiaiThichSoDu(iThang, iNam, User.Identity.Name, Request.UserHostAddress, iTrangThai);                
            }
            if(Xem=="1")
            {
                return RedirectToAction("Index", "rptKeToanTongHop_GiaiThichSoDu", new { iNam = iNam, iThang = iThang, optThu = optThu, optTra = optTra, optTamUng = optTamUng, KhoGiay = KhoGiay, iTrangThai = iTrangThai });
            }
            else
            {
            return RedirectToAction("Index", "KeToanTongHop_GiaiThichSoDu", new { iThang = iThang, iNam=iNam, iLoai = iLoai});
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ParentID, String iThang, String iNam, String iLoai)
        {
            String TenBangChiTiet = "KT_GiaiThichSoDu";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTuChiTiet;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaChungTuChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaChungTuChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.Save();
                    }
                }
                else
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
                        Bang bang = new Bang(TenBangChiTiet);
                        iID_MaChungTuChiTiet = arrMaHang[i];
                        if (iID_MaChungTuChiTiet == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;                            
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec",iNam);
                            bang.CmdParams.Parameters.AddWithValue("@iLoai", iLoai);
                            bang.CmdParams.Parameters.AddWithValue("@iThangLamViec",iThang);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        //Them tham so
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
            }
            return RedirectToAction("Index", "KeToanTongHop_GiaiThichSoDu", new { iThang = iThang, iNam = iNam, iLoai = iLoai });
        }

        public void Them_GiaiThichSoDu(String iThang, String iNam, String User, String UserHostAddress, String iTrangThai)
        {
            DataTable dt = KeToanTongHop_GiaiThichSoDuModels.Get_dtCuoiKy(iThang, iNam, iTrangThai);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Bang bang = new Bang("KT_GiaiThichSoDu");
                    bang.DuLieuMoi = true;
                    bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
                    bang.CmdParams.Parameters.AddWithValue("@iThangLamViec", iThang);
                    bang.CmdParams.Parameters.AddWithValue("@sLyDo", dt.Rows[i]["sTen"]);
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", dt.Rows[i]["iLoai"]);
                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", dt.Rows[i]["rSoTien"]);
                    bang.CmdParams.Parameters.AddWithValue("@sNoiDung", dt.Rows[i]["sTenDonVi"]);
                    bang.MaNguoiDungSua = User;
                    bang.IPSua = UserHostAddress;
                    bang.Save();
                }
                dt.Dispose();
            }
            
        }

        public void Xoa_GiaiThichSoDu(String iThang, String iNam, String User, String UserHostAddress)
        {
            String SQL = "UPDATE KT_GiaiThichSoDu SET iTrangThai=0,sID_MaNguoiDungSua=@sID_MaNguoiDungSua,sIPSua=@sIPSua";
                   SQL += " WHERE iThangLamViec=@iThang AND iNamLamViec=@iNam";
                   SqlCommand cmd = new SqlCommand(SQL);
                   cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", User);
                   cmd.Parameters.AddWithValue("@sIPSua", UserHostAddress);
                   cmd.Parameters.AddWithValue("@iThang", iThang);
                   cmd.Parameters.AddWithValue("@iNam", iNam);
                   Connection.UpdateDatabase(cmd);
                   cmd.Dispose();
        }

    }
}
