using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        [StringLength(26)]
        public string AccountNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(5, MinimumLength = 3)]
        public string Currency { get; set; } = "PLN";
        public decimal Balance { get; set; }
        public bool IsActive { get; set; } = true;
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
    }
}