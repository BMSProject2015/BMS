using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SSO.Client.References;
using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null)
        {
        }

        public AccountController(IFormsAuthentication formsAuth)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public ActionResult Authenticate()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Authenticate(string token, bool createPersistentCookie)
        {
            SSOPartnerServiceClient client = new SSOPartnerServiceClient("partnerEndpoint");
            SSOUser user = client.ValidateToken(token);

            if (string.IsNullOrEmpty(user.Username)
                || Guid.Empty.Equals(user.SessionToken))
            {
                return Json(new { result = "DENIED" });
            }

            FormsAuth.SignIn(user, createPersistentCookie);

            return Json(new { result = "SUCCESS" });
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [Authorize]
        public ActionResult Register()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult ForgotPasswordSubmit()
        {
            return View();
        }

        public ActionResult ConfirmResetPassword()
        {
            return View();
        }

        public ActionResult SSOLogOff()
        {
            FormsAuth.SignOut();

            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuth.SignOut();

            return RedirectToAction("Exit", "Account");
        }

        public ActionResult Exit()
        {
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }
    }

    public interface IFormsAuthentication
    {
        void SignIn(SSOUser user, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public FormsAuthenticationService()
        {

        }

        public void SignIn(SSOUser user, bool createPersistentCookie)
        {
            DateTime issueDate = DateTime.Now;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Username,
                issueDate, issueDate.AddMinutes(HamRiengModels.SSOTimeout), true, user.SessionToken.ToString());

            string protectedTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, protectedTicket);
            cookie.HttpOnly = true;
            cookie.Expires = issueDate.AddMinutes(HamRiengModels.SSOTimeout);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public void SignOut()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            cookie.Expires = DateTime.Now.AddDays(-10000.0);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
