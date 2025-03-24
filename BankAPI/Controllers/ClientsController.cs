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
    public class ClientsController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly ClientService _clientService;

        public ClientsController(BankDbContext context, ClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        [HttpPost]
        [Authorize(Roles = "BankEmployee,Administrator")]
        public async Task<ActionResult<ClientDto>> CreateClient(CreateClientDto createClientDto)
        {
            var client = await _clientService.CreateClientAsync(createClientDto);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return client;
        }
    }
}