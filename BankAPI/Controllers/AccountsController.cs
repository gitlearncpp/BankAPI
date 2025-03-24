using BankAPI.Data;
using BankAPI.DTO;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly AccountService _accountService;

        public AccountsController(BankDbContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        [HttpPost]
        [Authorize(Roles = "BankEmployee,Administrator")]
        public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto createAccountDto)
        {
            var account = await _accountService.CreateAccountAsync(createAccountDto);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpGet("number/{accountNumber}")]
        [Authorize]
        public async Task<ActionResult<AccountDto>> GetAccountByNumber(string accountNumber)
        {
            var account = await _accountService.GetAccountByNumberAsync(accountNumber);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
    }
}