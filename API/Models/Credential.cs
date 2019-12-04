using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Credential
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public AccountType UserType { get; set; }

        public enum AccountType
        {
            NormalUser,
            Admin
        }
    }
}
