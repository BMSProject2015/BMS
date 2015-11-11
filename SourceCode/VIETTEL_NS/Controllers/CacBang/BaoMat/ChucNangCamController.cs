using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using DomainModel.Abstract;

namespace Oneres.Controllers.ChucNangCam
{
    public class ChucNangCamController : Controller
    {
        public string sViewPath = "~/Views/CacBang/BaoMat/ChucNangCam/";

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string MaLuat)
        {
            ViewData["MaLuat"] = MaLuat;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID, String MaLuat)
        {
            String tg = Request.Form[ControlID + "_txt"];
            String[] arr = tg.Split(',');
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM PQ_Bang_ChucNangCam WHERE iID_MaLuat=@iID_MaLuat";
            cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
            Connection.UpdateDatabase(cmd);
            int i, j;
            for (i = 0; i < arr.Length; i++)
            {
                String[] arr1 = arr[i].Split(';');

                tg = "";
                for (j = 0; j < arr1.Length - 1; j++)
                {
                    switch (arr1[j])
                    {
                        case "0":
                            tg += "Detail" + BaoMat.KyTuTach;
                            break;
                        case "1":
                            tg += "Create" + BaoMat.KyTuTach;
                            break;
                        case "2":
                            tg += "Delete" + BaoMat.KyTuTach;
                            break;
                        case "3":
                            tg += "Edit" + BaoMat.KyTuTach;
                            break;
                        case "4":
                            tg += "Share" + BaoMat.KyTuTach;
                            break;
                        case "5":
                            tg += "Responsibility" + BaoMat.KyTuTach;
                            break;
                    }
                }
                if (tg != "")
                {
                    Bang bang = new Bang("PQ_Bang_ChucNangCam");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                    bang.CmdParams.Parameters.AddWithValue("@sTenBang", arr1[0]);
                    bang.CmdParams.Parameters.AddWithValue("@sChucNang", tg);
                    bang.Save();
                }
            }
            return RedirectToAction("Detail", "Luat", new { MaLuat = MaLuat });
        }
    }
}
