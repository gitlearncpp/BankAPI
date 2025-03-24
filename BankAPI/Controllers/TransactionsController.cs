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
    public class TransactionsController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly TransactionService _transactionService;

        public TransactionsController(BankDbContext context, TransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        [HttpPost("transfer")]
        [Authorize]
        public async Task<ActionResult<TransactionDto>> Transfer(TransferDto transferDto)
        {
            try
            {
                var transaction = await _transactionService.CreateTransferAsync(transferDto);
                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }
    }
}