using System;
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

namespace VIETTEL.Controllers.PhanBo
{
    public class PhanBo_TongController : Controller
    {
        //
        // GET: /PhanBo_Tong/
        public string sViewPath = "~/Views/PhanBo/PhanBoTong/";
        public ActionResult Index()
        {
            return View(sViewPath + "PhanBoTong_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(String iID_MaPhanBo)
        {
            ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
            return View(sViewPath + "PhanBoTong_Detail.aspx");
        }

        [Authorize]
        public ActionResult PhanTongChiTiet_Frame(String iID_MaPhanBo)
        {
            return View(sViewPath + "PhanBoTong_Detail_Frame.aspx");
        }

        [Authorize]
        public ActionResult Create(String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            //if (String.IsNullOrEmpty(iID_MaChiTieu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            //{
            //    //Phải có quyền thêm chứng từ
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (BaoMat.ChoPhepLamViec(MaND, "PB_PhanBo", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaPhanBo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
            return View(sViewPath + "PhanBoTong_Create.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            //if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            //{
            //    //Phải có quyền thêm chứng từ
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (BaoMat.ChoPhepLamViec(MaND, "PB_PhanBo", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaPhanBo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
            return View(sViewPath + "PhanBoTong_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            //if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("PB_PhanBo_PhanBo");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            iID_MaPhanBo = Request.Form[ParentID + "_iID_MaPhanBo"];
            String dNgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
                return View(sViewPath + "PhanBo_Create.aspx");
            }
            else
            {

                String SQL = "DELETE FROM PB_PhanBo_PhanBo WHERE iID_MaPhanBoTong=@iID_MaPhanBoTong";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaPhanBoTong",iID_MaPhanBo);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                String sMaPhanBo = Request.Form["iID_MaPhanBo"];
                String[] arrPhanBo=sMaPhanBo.Split(',');
                bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBoTong", iID_MaPhanBo);
                for (i = 0; i<arrPhanBo.Length; i++)
                {
                    if (i == 0)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBo", arrPhanBo[i]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBo_PhanBo", Guid.NewGuid());
                    }
                    else
                    {
                        bang.CmdParams.Parameters["@iID_MaPhanBo"].Value = arrPhanBo[i];
                        bang.CmdParams.Parameters["@iID_MaPhanBo_PhanBo"].Value = Guid.NewGuid();
                    }
                    bang.Save();
                }
               
            }
            return RedirectToAction("Index", "PhanBo_Tong", new { iID_MaPhanBo = iID_MaPhanBo });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            String TenBangChiTiet = "PB_PhanBoChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachHang }, StringSplitOptions.None);
            string idAction = Request.Form["idAction"];
            String iID_MaPhanBo1 = "";
            String sDSMaPhanBo = "", bDongY = ""; ;
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_PhanBo_BangDuLieu.DauCachO }, StringSplitOptions.None);
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
                    SqlCommand cmd = new SqlCommand();
                    String SQL = "UPDATE PB_PhanBoChiTiet SET ";
                    String DK = "";
                    Bang bang = new Bang(TenBangChiTiet);
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    //Them tham so
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrMaCot[j]=="iID_MaPhanBo")
                        {
                            iID_MaPhanBo1 = arrGiaTri[j];
                        }
                        if (arrMaCot[j] == "bDongY")
                        {
                            bDongY = arrGiaTri[j];
                        }
                        if (arrThayDoi[j] == "1")
                        {
                            if (arrMaCot[j].EndsWith("_ConLai") == false)
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
                                else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
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
                            else
                            {
                                String Truong = "@" + arrMaCot[j].Substring(0, arrMaCot[j].IndexOf('_'));
                                DK += arrMaCot[j].Substring(0, arrMaCot[j].IndexOf('_')) + "=" + Truong + ",";
                                if (arrMaCot[j].StartsWith("b"))
                                {
                                    //Nhap Kieu checkbox
                                    if (arrGiaTri[j] == "1")
                                    {
                                        cmd.Parameters.AddWithValue(Truong, true);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue(Truong, false);
                                    }
                                }
                                else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
                                {
                                    //Nhap Kieu so
                                    if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                    {
                                        cmd.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                    }
                                }
                                else
                                {
                                    //Nhap kieu xau
                                    cmd.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                }
                            }
                        }
                    }
                    //lưu số tiền còn lại vào đơn chờ phân bổ 
                    //DK = DK.Remove(DK.Length - 1);
                    //SQL = SQL + DK + " WHERE iID_MaPhanBoChiTiet=@iID_MaPhanBoChiTiet";
                    //cmd.CommandText = SQL;
                    //cmd.Parameters.AddWithValue("@iID_MaPhanBoChiTiet", LayMaHangChoPhanBo(arrMaHang[i], iID_MaPhanBo));
                    //Connection.UpdateDatabase(cmd);
                    //cmd.Dispose();
                   
                    bang.Save();
                    if (idAction == "1")
                    {
                        if ((sDSMaPhanBo.IndexOf(iID_MaPhanBo1) < 0 || sDSMaPhanBo=="") && bDongY != "")
                        {
                            PhanBo_TongModels.TuChoi(iID_MaPhanBo,iID_MaPhanBo1, User.Identity.Name, Request.UserHostAddress);
                            sDSMaPhanBo = sDSMaPhanBo + iID_MaPhanBo1 + ",";
                        }
                    }
                }
            }

            
            if (idAction == "2")
            {
                PhanBo_TongModels.TrinhDuyet(iID_MaPhanBo, User.Identity.Name, Request.UserHostAddress);
            }
            return RedirectToAction("PhanTongChiTiet_Frame","PhanBo_Tong", new { iID_MaPhanBo = iID_MaPhanBo });
        }
    }
}
