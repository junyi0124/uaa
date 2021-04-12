using System.ComponentModel.DataAnnotations;

namespace UAA.Model.User
{
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
