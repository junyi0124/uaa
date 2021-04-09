using System;
using System.ComponentModel.DataAnnotations;

namespace UAA.Entity
{
    public class UserAccount
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(40)]
        public string UserName { get; set; }

        [Required]
        [StringLength(40)] 
        public string DisplayName { get; set; }

        [Required]
        [StringLength(40)] 
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
