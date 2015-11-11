using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VIETTEL.Models;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VIETTEL.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /NguoiDung/

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Detail") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult List()
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View();
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }

        [Authorize]
        public ActionResult Edit(String MaNguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            String sChucNang = "Create";
            if (String.IsNullOrEmpty( MaNguoiDung)==false)
            {
                sChucNang = "Edit";
            }
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View();

             }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Delete(String MaNguoiDung, String MaNhomNguoiDung)
        {
              if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {

            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("QT_NguoiDung");
            bang.GiaTriKhoa = MaNguoiDung;
            bang.Delete();
            return RedirectToAction("List");
            }
              else
              {
                  return RedirectToAction("Index", "PermitionMessage");
              }
        }

        [Authorize]
        public ActionResult CapNhapKichHoat(String MaNguoiDung, String MaNhomNguoiDung, Boolean HoatDong)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("QT_NguoiDung");
            bang.GiaTriKhoa = MaNguoiDung;
            bang.CmdParams.Parameters.AddWithValue("@bHoatDong", HoatDong);
            bang.DuLieuMoi = false;
            bang.Save();
            return RedirectToAction("List");
            //return RedirectToAction("Index", new { MaNhomNguoiDung = MaNhomNguoiDung });
        }

        [Authorize]
        public ActionResult PasswordReset(String MaNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            MembershipUser user = Membership.GetUser(MaNguoiDung);
            if (user.IsLockedOut)
            {
                user.UnlockUser();
            }
            if (user.IsLockedOut==false)
            {   
                String NewPassword = user.ResetPassword();
                String MailServer = ConfigurationManager.AppSettings["Mailserver"];
                String MailFrom = ConfigurationManager.AppSettings["MailFrom"];
                String PassMailFrom = ConfigurationManager.AppSettings["Pass"];      
                String CC = null;
                String BCC = null;
                String Subject = NgonNgu.LayXau("Thiết lập lại mật khẩu ")+"("+DateTime.Now.ToString("HH:mm dd/MM/yyyy")+")";
                String Body = NgonNgu.LayXau("Ban quản trị phần mềm BQP gửi đến bạn mật khẩu mới: ") + "<span style=\"background: yellow;\"><b>" + NewPassword + "</b></font>";
                String[] Attach = new String[0];
                SendMail(MailFrom, PassMailFrom, MailServer, user.Email, CC, BCC, Subject, Body, Attach);

                ViewData["NewPassword"] = NewPassword;
                return View();
            }
            return RedirectToAction("Index", "PermitionMessage");
        }

        private void SendMail(String MailFrom, String PassMailFrom, String MailServer, String MailTo, String CC, String BCC, String Subject, String Body, String[] attach)
        {
            //Lay dia chi basicCredential
            Char[] ch = { '@' };
            String[] tmp;
            String mailfrom;
            tmp = MailFrom.Split(ch);
            mailfrom = tmp[0];
            //Co che chung thuc cua Mang
            NetworkCredential basicCredential = new NetworkCredential(mailfrom, PassMailFrom);            
            SmtpClient client = new SmtpClient(MailServer, 25);
            MailMessage mail = new MailMessage();
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential;            
            //Mail Gửi
            mail.From = new MailAddress(MailFrom);
            //Mail nhận
            String[] a;
            a = MailTo.Split(',');
            if (a.Length > 1)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != "")
                    {
                        mail.To.Add(new MailAddress(a[i]));
                    }
                }

            }
            else
            {
                mail.To.Add(new MailAddress(MailTo));
            }
            //Gửi CC
            if (string.IsNullOrEmpty(CC) == false)
            {
                String[] c;
                c = CC.Split(',');
                if (c.Length > 1)
                {
                    for (int i = 0; i < c.Length; i++)
                    {
                        if (c[i] != "")
                        {
                            mail.CC.Add(new MailAddress(c[i]));
                        }
                    }
                }
                else
                {
                    mail.CC.Add(new MailAddress(CC));
                }
            }
            //Gửi BCC
            if (string.IsNullOrEmpty(BCC) == false)
            {
                String[] b;
                b = BCC.Split(',');
                if (b.Length > 1)
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        if (b[i] != "")
                        {
                            mail.Bcc.Add(new MailAddress(b[i]));
                        }
                    }

                }
                else
                {
                    mail.Bcc.Add(new MailAddress(BCC));
                }
            }
            if (attach.Length > 0)
            {
                if (attach.Length > 1)
                {
                    for (int i = 0; i < attach.Length; i++)
                    {
                        string duongdan = attach[i];
                        mail.Attachments.Add(new Attachment(duongdan));
                    }
                }
                else
                {
                    string duongdan1 = attach[0];
                    mail.Attachments.Add(new Attachment(duongdan1));
                }
            }
            mail.Subject = Subject;
            mail.Body = Server.HtmlDecode(Body);
            mail.IsBodyHtml = true;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            try
            {
                client.Send(mail);
            }
            catch
            {
            }
        }

        [Authorize]
        public ActionResult Detail(String MaNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View();
        }
    }
}
