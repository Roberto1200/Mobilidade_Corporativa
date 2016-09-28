using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations.Resources;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Manager.Models
{
    public class UsersContext
    {

    }


    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class SuperLoginModel
    {
        public LoginModel LoginObject { get; set; }
        public ForgotPasswordModel ForgotPasswordObject { get; set; }
        public RequestTokenModel RequestTokenObject { get; set; }
        public ResetPasswordModel ResetPasswordObject { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [AllowHtml]
        [Display(Name = "User name")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Detectado uso de caracteres inválidos")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Used to record failures for the purposes of lockout
        public virtual int contagem { get; set; } = 3;
        // Is lockout enabled for this user
        public virtual bool LockoutEnabled { get; set; }
        public virtual string data_liberacao { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        //public LoginModel()
        //{
        //    this.contagem = 3;
        //}
    }

    public class ForgotPasswordModel
    {
        [Required]

        [AllowHtml]
        [Display(Name = "User name")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Detectado uso de caracteres inválidos")]
        public string Email { get; set; }

    }

    public class RequestTokenModel
    {
        [Required]

        [AllowHtml]
        [Display(Name = "Token")]
        public string Token { get; set; }

    }

    public class ResetPasswordModel
    {
        [Required]

        [AllowHtml]
        [Display(Name = "User name")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Detectado uso de caracteres inválidos")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Token")]
        [StringLength(6, ErrorMessage = "{0} deve ter entre {2} e 6 caracteres.", MinimumLength = 1)]
        public string Token { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter máximo {2} carateres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e confirmação não conferem.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
