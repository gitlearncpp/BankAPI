using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Account FromAccount { get; set; } = null!;
        [Required]
        public Account ToAccount { get; set; } = null!;
    }
}