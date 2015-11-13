using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;


namespace VIETTEL.Models
{
    #region Models
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class RegisterModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("FullName")]
        public string FullName { get; set; }
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email, string fullname);
        MembershipCreateStatus CreateUser(string userName, string password, string email, string fullname, string NhomNguoiDung, Boolean HoatDong);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email, string fullname)
        {
            return CreateUser(userName, password, email, fullname, "", false);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email, string fullname, string NhomNguoiDung, Boolean HoatDong)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "password");
            //if (String.IsNullOrEmpty(email)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "email");
            //if (String.IsNullOrEmpty(fullname)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "fullname");

            MembershipCreateStatus status;
            MembershipUser us =  _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            if (status == MembershipCreateStatus.Success)
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", userName);
                cmd.Parameters.AddWithValue("@sHoTen", fullname);
                if (String.IsNullOrEmpty(NhomNguoiDung) == false)
                {
                    cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", NhomNguoiDung);
                }
                cmd.Parameters.AddWithValue("@bHoatDong", HoatDong);
                Connection.InsertOrUpdateRecord("QT_NguoiDung", "", cmd);
                cmd.Dispose();
            }
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException(NgonNgu.LayXau("Phải nhập giá trị."), "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return NgonNgu.LayXau("Tài khoản này đã tồn tại. Bạn có thể thử tài khoản khác.");

                case MembershipCreateStatus.DuplicateEmail:
                    return NgonNgu.LayXau("Email đã tồn tại. Xin hãy nhập lại e-mail khác.");

                case MembershipCreateStatus.InvalidPassword:
                    return NgonNgu.LayXau("Nhập sai mật khẩu. Xin hãy nhập lại mật khẩu.");

                case MembershipCreateStatus.InvalidEmail:
                    return NgonNgu.LayXau("Nhập sai e-mail. Xin hãy nhập lại e-mail.");

                case MembershipCreateStatus.InvalidAnswer:
                    return NgonNgu.LayXau("Nhập sai câu trả lời cho mật khẩu. Xin hãy nhập lại câu trả lời.");

                case MembershipCreateStatus.InvalidQuestion:
                    return NgonNgu.LayXau("Nhập sai câu hỏi cho mật khẩu. Xin hãy nhập lại câu hỏi.");

                case MembershipCreateStatus.InvalidUserName:
                    return NgonNgu.LayXau("Tài khoản nhập không đúng quy cách. Xin hãy nhập lại tài khoản.");

                case MembershipCreateStatus.ProviderError:
                    return NgonNgu.LayXau("Việc kiểm tra tài khoản bị sai. Xin hãy kiểm tra lại thông tin.");

                case MembershipCreateStatus.UserRejected:
                    return NgonNgu.LayXau("Tạo tài khoản sai. Xin hãy kiểm tra lại.");

                default:
                    return NgonNgu.LayXau("Lỗi bất thường. Xin hãy kiểm tra lại.");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(NgonNgu.LayXau("Mật khẩu có độ dài tối thiểu {1} ký tự."))
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion
}

