namespace BankAPI.DTO
{
    public class CreateAccountDto
    {
        public int ClientId { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal InitialBalance { get; set; } = 0;
    }

    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public ClientDto Client { get; set; } = null!;
    }
}