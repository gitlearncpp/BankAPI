using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTO
{
    public class CreateClientDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string City { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Street { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string HouseNumber { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string MotherName { get; set; } = string.Empty;
        public bool VIP { get; set; }
        public bool CreditCard { get; set; }
        public bool Mortgage { get; set; }
        public bool Loan { get; set; }
        public bool GoodCustomer { get; set; }
        public bool MarketingConsent { get; set; }
        public bool Blacklist { get; set; }
        public bool IsDeleted { get; set; }
        public CreateAccountDto? Account { get; set; }
    }

    public class ClientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public bool VIP { get; set; }
        public bool CreditCard { get; set; }
        public bool Mortgage { get; set; }
        public bool Loan { get; set; }
        public bool GoodCustomer { get; set; }
        public bool MarketingConsent { get; set; }
        public bool Blacklist { get; set; }
        public bool IsDeleted { get; set; }
        public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }
}