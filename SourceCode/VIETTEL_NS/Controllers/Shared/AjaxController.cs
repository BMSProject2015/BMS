using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using System.Web.Script.Serialization;

namespace Oneres.Controllers.Shared
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JavaScriptResult Index(string PartialView, string OnLoad)
        {
            // Điền dữ liệu vào bảng
            String tg = CommonFunction.RenderPartialViewToString(PartialView, this);
            String strJ = "";
            BocTachDuLieu(ref tg, ref strJ);
            tg = tg.Trim();
            strJ = strJ.Trim();
            if (String.IsNullOrEmpty(OnLoad) == false)
            {
                strJ = JavaScriptEncode(strJ);
                strJ = String.Format("{0}({1});ImportJavascript({2});", OnLoad, JavaScriptEncode(tg), strJ);
            }
            if (strJ == "")
            {
                return null;
            }
            return JavaScript(strJ);
        }

        private string JavaScriptEncode(string str)
        {
            // Encode certain characters, or the JavaScript expression could be invalid

            return new JavaScriptSerializer().Serialize(str);
            //return "";
        }

        public void BocTachDuLieu(ref string str1, ref string str2)
        {
            Boolean ok = true;
            str1 = str1.Replace("\r", "");
            str1 = str1.Replace("\n", "");

            while (ok)
            {
                int cs1 = str1.IndexOf("<script");

                if (cs1 >= 0)
                {
                    int cs2 = str1.IndexOf("</script>");
                    string tg = str1.Substring(cs1, cs2 - cs1 + 9);
                    str1 = str1.Remove(cs1, cs2 - cs1 + 9);
                    cs1 = tg.IndexOf(">");
                    tg = tg.Substring(cs1 + 1, tg.Length - cs1 - 10);
                    //tg = tg.Replace("\r", "");
                    //tg = tg.Replace("\n", "");
                    str2 += tg;
                }
                else
                {
                    ok = false;
                }
            }
        }

    }
}
