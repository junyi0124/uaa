using System.ComponentModel.DataAnnotations;

namespace UAA.Model.User
{
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


}
