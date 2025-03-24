using BankAPI.Data;
using BankAPI.DTO;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class ClientService
    {
        private readonly BankDbContext _context;
        private readonly AccountService _accountService;

        public ClientService(BankDbContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientDto createClientDto)
        {
            var client = new Client
            {
                FirstName = createClientDto.FirstName,
                LastName = createClientDto.LastName,
                Email = createClientDto.Email,
                PhoneNumber = createClientDto.PhoneNumber,
                PostalCode = createClientDto.PostalCode,
                City = createClientDto.City,
                Street = createClientDto.Street,
                HouseNumber = createClientDto.HouseNumber,
                ApartmentNumber = createClientDto.ApartmentNumber,
                Country = createClientDto.Country,
                MotherName = createClientDto.MotherName,
                VIP = createClientDto.VIP,
                CreditCard = createClientDto.CreditCard,
                Mortgage = createClientDto.Mortgage,
                Loan = createClientDto.Loan,
                GoodCustomer = createClientDto.GoodCustomer,
                MarketingConsent = createClientDto.MarketingConsent,
                Blacklist = createClientDto.Blacklist,
                IsDeleted = createClientDto.IsDeleted
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Jeśli podano informacje o koncie, utwórz konto dla klienta
            if (createClientDto.Account != null)
            {
                var accountDto = createClientDto.Account;
                accountDto.ClientId = client.Id;
                await _accountService.CreateAccountAsync(accountDto);
            }

            return MapToClientDto(client);
        }

        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            return client == null ? null : MapToClientDto(client);
        }

        private ClientDto MapToClientDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                PostalCode = client.PostalCode,
                City = client.City,
                Street = client.Street,
                HouseNumber = client.HouseNumber,
                ApartmentNumber = client.ApartmentNumber,
                Country = client.Country,
                MarketingConsent = client.MarketingConsent,
            };
        }
    }
}