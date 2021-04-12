using System.ComponentModel.DataAnnotations;

namespace UAA.Model.User
{
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


}
