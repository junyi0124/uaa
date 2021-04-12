using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAA.Entity
{
  public class UserAccount
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(40)]
    public string UserName { get; set; }

    [Required]
    [StringLength(40)]
    public string DisplayName { get; set; }

    [Required]
    [StringLength(40)]
    public string Email { get; set; }

    [MaxLength(64)]
    public byte[] PasswordHash { get; set; }
    [MaxLength(128)]
    public byte[] PasswordSalt { get; set; }

    /// <summary>
    /// user status
    /// bit 1: user active
    /// bit 2: locked - allow to login
    /// bit 3: verify email
    /// bit 4: verify phone
    /// bit 5: 
    /// bit 6: 
    /// </summary>
    public int Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdate { get; set; }

  }
}
