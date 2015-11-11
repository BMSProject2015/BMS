using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;

namespace VIETTEL.Controllers.ChungTuChiTiet
{
    public class DuToan_ChungTuChiTiet_GomController : Controller
    {
        //
        // GET: /DuToan_ChungTuChiTiet_Gom/
        public string sViewPath = "~/Views/DuToan/ChungTuChiTiet/";

        [Authorize]
        public ActionResult Index(String iID_MaChungTu, String sLNS, String iID_MaDonVi)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            return View(sViewPath + "ChungTuChiTiet_Gom_Index.aspx");
        }
       
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame(String sLNS, String MaLoai)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaChungTu = Request.Form["DuToan_iID_MaChungTu"];
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }


            MaLoai = Request.Form["DuToan_MaLoai"];
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["MaLoai"] = MaLoai;
            return View(sViewPath + "ChungTuChiTiet_Index_DanhSach_Frame_Gom.aspx", new { sLNS = sLNS });
            //return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, LoadLai = "1" });
        }

        
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ChiNganSach, String iID_MaChungTu, String sLNS)
        {
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "DuToan_ChungTuChiTiet_Gom", new {  iID_MaChungTu = iID_MaChungTu,sLNS=sLNS });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "DuToan_ChungTuChiTiet_Gom", new {iID_MaChungTu = iID_MaChungTu,sLNS=sLNS });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu,sLNS=sLNS});
        }


        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTu,String iLoai,String sLNS)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_BaoDam_TrinhDuyet(MaND, iID_MaChungTu);
            else
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //update trangthai bang DT_ChungTu_TLTH
            String SQL = "";
            SQL = String.Format(@"UPDATE DT_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iID_MaChungTu_TLTH=@iID_MaChungTu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            //update trangthai bang dt_chungtu

            String DK = "";
            String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
            if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Guid.Empty.ToString();
            String[] arrChungtu = iID_MaChungTu_CT.Split(',');
             cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

            }
            SQL = String.Format(@"UPDATE DT_ChungTu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iTrangThai=1 AND ({0})",DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //update bang chung tu chi tiet
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DuToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(arrChungtu[j], iID_MaTrangThaiDuyet_TrinhDuyet, false, MaND, Request.UserHostAddress);
            }
            if (iLoai == "1")
            {
                if (sLNS == "1040100")
                    return RedirectToAction("Index", "DuToan_ChungTu_BaoDam", new { sLNS = sLNS, iLoai = iLoai });
                else
                    return RedirectToAction("Index", "DuToan_ChungTu", new { sLNS = sLNS, iLoai = iLoai });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS });
        }
        [Authorize]
        public ActionResult TrinhDuyet_THCuc(String iID_MaChungTu, String iLoai, String sLNS)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_THCuc_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String SQL = "";
            //update trangthai bang DT_ChungTu_TLTHCuc
            SQL = String.Format(@"UPDATE DT_ChungTu_TLTHCuc SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iID_MaChungTu_TLTHCuc=@iID_MaChungTu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);

            //update trangthai bang DT_ChungTu_TLTH
            String DK = "";
            String iID_MaChungTu_TLTH = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", iID_MaChungTu, "iID_MaChungTu_TLTH"));
            if (String.IsNullOrEmpty(iID_MaChungTu_TLTH)) iID_MaChungTu_TLTH = Guid.Empty.ToString();
            String[] arrChungtu = iID_MaChungTu_TLTH.Split(',');
            cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DK += " iID_MaChungTu_TLTH =@iID_MaChungTu" + j;
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

            }
            SQL = String.Format(@"UPDATE DT_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iTrangThai=1 AND ( {0})",DK);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);


            //update trangthai bang dt_chungtu
            DK = "";
             String iID_MaChungTu_CT="";
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                String s = "";
                s = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTH", "iID_MaChungTu_TLTH", arrChungtu[j], "iID_MaChungTu"));
                iID_MaChungTu_CT += s + ",";
            }

            if (String.IsNullOrEmpty(iID_MaChungTu_CT)) iID_MaChungTu_CT = Guid.Empty.ToString();
            arrChungtu = iID_MaChungTu_CT.Split(',');
            cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                if (arrChungtu[j] == "")
                {
                    arrChungtu[j] = Guid.Empty.ToString();
                }
                    DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                   
                    cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";

            }
            SQL = String.Format(@"UPDATE DT_ChungTu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iTrangThai=1 AND ({0})", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //update bang chung tu chi tiet
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DuToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(arrChungtu[j], iID_MaTrangThaiDuyet_TrinhDuyet, false, MaND, Request.UserHostAddress);
            }
            if (iLoai == "2")
                return RedirectToAction("Index", "DuToan_ChungTu", new { sLNS = sLNS, iLoai = iLoai });
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTu, String iLoai, String sLNS)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_BaoDam_TuChoi(MaND, iID_MaChungTu);
            else
                iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //update trangthai bang DT_ChungTu_TLTH
            String SQL = "";
            SQL = String.Format(@"UPDATE DT_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iID_MaChungTu_TLTH=@iID_MaChungTu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TuChoi);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            //update trangthai bang dt_chungtu

            String DK = "";
            String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
            if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Guid.Empty.ToString();
            String[] arrChungtu = iID_MaChungTu_CT.Split(',');
            cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

            }
            SQL = String.Format(@"UPDATE DT_ChungTu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iTrangThai=1 AND ({0})", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TuChoi);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //update bang chung tu chi tiet
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DuToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(arrChungtu[j], iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);
            }
            if (iLoai == "1")
            {
                if (sLNS == "1040100")
                    return RedirectToAction("Index", "DuToan_ChungTu_BaoDam", new { sLNS = sLNS, iLoai = iLoai });
                else
                    return RedirectToAction("Index", "DuToan_ChungTu", new { sLNS = sLNS, iLoai = iLoai });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS });
        }


        [Authorize]
        public ActionResult TuChoi_THCuc(String iID_MaChungTu, String iLoai, String sLNS)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_THCuc_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String SQL = "";
            //update trangthai bang DT_ChungTu_TLTHCuc
            SQL = String.Format(@"UPDATE DT_ChungTu_TLTHCuc SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iID_MaChungTu_TLTHCuc=@iID_MaChungTu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);

            //update trangthai bang DT_ChungTu_TLTH
            String DK = "";
            String iID_MaChungTu_TLTH = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTHCuc", "iID_MaChungTu_TLTHCuc", iID_MaChungTu, "iID_MaChungTu_TLTH"));
            if (String.IsNullOrEmpty(iID_MaChungTu_TLTH)) iID_MaChungTu_TLTH = Guid.Empty.ToString();
            String[] arrChungtu = iID_MaChungTu_TLTH.Split(',');
            cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DK += " iID_MaChungTu_TLTH =@iID_MaChungTu" + j;
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

            }
            SQL = String.Format(@"UPDATE DT_ChungTu_TLTH SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iTrangThai=1 AND ( {0})",DK);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);


            //update trangthai bang dt_chungtu
            DK = "";
             String iID_MaChungTu_CT="";
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                String s = "";
                s = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTH", "iID_MaChungTu_TLTH", arrChungtu[j], "iID_MaChungTu"));
                iID_MaChungTu_CT += s + ",";
            }

            if (String.IsNullOrEmpty(iID_MaChungTu_CT)) iID_MaChungTu_CT = Guid.Empty.ToString();
            arrChungtu = iID_MaChungTu_CT.Split(',');
            cmd = new SqlCommand();
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                if (arrChungtu[j] == "")
                {
                    arrChungtu[j] = Guid.Empty.ToString();
                }
                    DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                   
                    cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);
                if (j < arrChungtu.Length - 1)
                    DK += " OR ";

            }
            SQL = String.Format(@"UPDATE DT_ChungTu SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_TrinhDuyet WHERE iTrangThai=1 AND ({0})", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_TrinhDuyet", iID_MaTrangThaiDuyet_TrinhDuyet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //update bang chung tu chi tiet
            for (int j = 0; j < arrChungtu.Length; j++)
            {
                DuToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(arrChungtu[j], iID_MaTrangThaiDuyet_TrinhDuyet, false, MaND, Request.UserHostAddress);
            }
            if (iLoai == "2")
                return RedirectToAction("Index", "DuToan_ChungTu", new { sLNS = sLNS, iLoai = iLoai });
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS });
        }
    }
}
