using BankAPI.Data;
using BankAPI.DTO;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class TransactionService
    {
        private readonly BankDbContext _context;

        public TransactionService(BankDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionDto> CreateTransferAsync(TransferDto transferDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Pobierz konta
                var fromAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == transferDto.FromAccountNumber);
                if (fromAccount == null)
                {
                    throw new Exception("Konto źródłowe nie istnieje");
                }

                var toAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == transferDto.ToAccountNumber);
                if (toAccount == null)
                {
                    throw new Exception("Konto docelowe nie istnieje");
                }

                if (!fromAccount.IsActive || !toAccount.IsActive)
                {
                    throw new Exception("Jedno z kont jest nieaktywne");
                }

                if (fromAccount.Balance < transferDto.Amount)
                {
                    throw new Exception("Niewystarczające środki na koncie");
                }

                // Jeśli waluty są różne, należałoby przeprowadzić konwersję walut
                if (fromAccount.Currency != toAccount.Currency)
                {
                    throw new Exception("Przelew pomiędzy różnymi walutami nie jest jeszcze obsługiwany");
                }

                // Przeprowadź przelew
                fromAccount.Balance -= transferDto.Amount;
                toAccount.Balance += transferDto.Amount;

                // Zapisz transakcję
                var transactionRecord = new Transaction
                {
                    FromAccountId = fromAccount.Id,
                    ToAccountId = toAccount.Id,
                    Amount = transferDto.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Description = transferDto.Description
                };

                _context.Transactions.Add(transactionRecord);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TransactionDto
                {
                    Id = transactionRecord.Id,
                    FromAccountNumber = fromAccount.AccountNumber,
                    ToAccountNumber = toAccount.AccountNumber,
                    Amount = transactionRecord.Amount,
                    TransactionDate = transactionRecord.TransactionDate,
                    Description = transactionRecord.Description
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return null;
            }

            return new TransactionDto
            {
                Id = transaction.Id,
                FromAccountNumber = transaction.FromAccount.AccountNumber,
                ToAccountNumber = transaction.ToAccount.AccountNumber,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description
            };
        }
    }
}
