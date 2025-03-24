using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankAPI.Data;
using BankAPI.DTO;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BankAPI.Services
{
    public class AuthService
    {
        private readonly BankDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ClientService _clientService;

        public AuthService(BankDbContext context, IConfiguration configuration, ClientService clientService)
        {
            _context = context;
            _configuration = configuration;
            _clientService = clientService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
        {
            // Sprawdź, czy użytkownik już istnieje
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                throw new Exception("Użytkownik o podanej nazwie już istnieje");
            }

            // Utwórz klienta
            var clientDto = new CreateClientDto
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                PostalCode = registerDto.PostalCode,
                City = registerDto.City,
                Street = registerDto.Street,
                HouseNumber = registerDto.HouseNumber,
                ApartmentNumber = registerDto.ApartmentNumber,
                MotherName = registerDto.MotherName,
                
                
            };

            var client = await _clientService.CreateClientAsync(clientDto);

            // Utwórz użytkownika
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password); // Pełna nazwa klasy
            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = hashedPassword,
                Role = UserRole.Client,
                ClientId = client.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generuj token
            return await GenerateToken(user);
        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
            // Szukaj użytkownika po nazwie
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            // Weryfikacja hasła
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) // Pełna nazwa klasy
            {
                throw new Exception("Nieprawidłowa nazwa użytkownika lub hasło");
            }

            // Generuj token
            return await GenerateToken(user);
        }

        private async Task<AuthResponse> GenerateToken(User user)
        {
            // Klucz do generowania JWT
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "defaultSecretKey12345")
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Lista roszczeń użytkownika (JWT claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // Generowanie tokena JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            // Zwróć odpowiedź jako AuthResponse
            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = user.Role.ToString()
            };
        }
    }
}