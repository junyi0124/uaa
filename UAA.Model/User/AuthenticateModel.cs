using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAA.Model.User
{
  /// <summary>
  /// login model
  /// </summary>
  public class AuthenticateModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
  }

  /// <summary>
  /// user model
  /// </summary>
  public class UserModel
  {
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }

    public int Status { get; set; }
  }

  /// <summary>
  /// user register model
  /// </summary>
  public class RegisterModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string DisplayName { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "两次输入密码不同，请重试")]
    public string ConfirmPassword { get; set; }
  }

  /// <summary>
  /// user update model
  /// </summary>
  public class UpdateModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string DisplayName { get; set; }
  }

  /// <summary>
  /// user change password model
  /// </summary>
  public class ChangePasswordModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string OldPassword { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "两次输入密码不同，请重试")]
    public string ConfirmPassword { get; set; }
  }


}
