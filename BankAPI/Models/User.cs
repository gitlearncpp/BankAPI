using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models
{
    public enum UserRole
    {
        Client,
        BankEmployee,
        Administrator
    }

    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; }
        [Required]
        public int? ClientId { get; set; }
        [Required]
        public Client? Client { get; set; }
    }
}