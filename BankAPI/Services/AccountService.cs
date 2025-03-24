using BankAPI.Data;
using BankAPI.DTO;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class AccountService
    {
        private readonly BankDbContext _context;

        public AccountService(BankDbContext context)
        {
            _context = context;
        }

        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            // Sprawdź czy klient istnieje
            var client = await _context.Clients.FindAsync(createAccountDto.ClientId);
            if (client == null)
            {
                throw new Exception("Klient o podanym ID nie istnieje");
            }

            // Generuj unikalny numer konta
            string accountNumber = GenerateAccountNumber();
            while (await _context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber))
            {
                accountNumber = GenerateAccountNumber();
            }

            var account = new Account
            {
                AccountNumber = accountNumber,
                Currency = createAccountDto.Currency,
                Balance = createAccountDto.InitialBalance,
                IsActive = true,
                ClientId = createAccountDto.ClientId
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Pobierz konto z klientem
            account = await _context.Accounts
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == account.Id);

            return MapToAccountDto(account!);
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == id);

            return account == null ? null : MapToAccountDto(account);
        }

        public async Task<AccountDto?> GetAccountByNumberAsync(string accountNumber)
        {
            var account = await _context.Accounts
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            return account == null ? null : MapToAccountDto(account);
        }

        private string GenerateAccountNumber()
        {
            // Format: 2 cyfry kodu banku + 24 cyfry losowe
            Random random = new Random();
            string bankCode = "12"; // przykładowy kod banku
            string randomPart = "";
            for (int i = 0; i < 24; i++)
            {
                randomPart += random.Next(0, 10).ToString();
            }
            return bankCode + randomPart;
        }

        private AccountDto MapToAccountDto(Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Currency = account.Currency,
                Balance = account.Balance,
                IsActive = account.IsActive,
                Client = new ClientDto
                {
                    Id = account.Client.Id,
                    FirstName = account.Client.FirstName,
                    LastName = account.Client.LastName,
                    Email = account.Client.Email,
                    PhoneNumber = account.Client.PhoneNumber,
                    PostalCode = account.Client.PostalCode,
                    City = account.Client.City,
                    Street = account.Client.Street,
                    HouseNumber = account.Client.HouseNumber,
                    ApartmentNumber = account.Client.ApartmentNumber,
                    Country = account.Client.Country,
                    MarketingConsent = account.Client.MarketingConsent,
                    Blacklist = account.Client.Blacklist,
                }
            };
        }
    }
}
