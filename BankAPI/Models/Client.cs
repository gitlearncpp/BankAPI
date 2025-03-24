namespace BankAPI.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
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
        public bool IsActive { get; set; }
        public int? AccountId { get; set; }
        public bool MarketingConsent { get; set; }
        public bool Blacklist { get; set; }
        public bool IsDeleted { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}