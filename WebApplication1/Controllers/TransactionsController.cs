using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Requests;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;


namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;


        public TransactionsController(ITransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] TransactionRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID claim is missing" });
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID claim" });
            }

            var user = _userService.GetById(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            try
            {
                _transactionService.Deposit(userId, request.Amount);
                return Ok(new { balance = user.Balance + request.Amount });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] TransactionRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID claim is missing" });
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID claim" });
            }

            var user = _userService.GetById(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            try
            {
                _transactionService.Withdraw(userId, request.Amount);
                return Ok(new { balance = user.Balance - request.Amount });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID claim is missing" });
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID claim" });
            }

            var user = _userService.GetById(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            try
            {
                _transactionService.Transfer(userId, request.ToUserId, request.Amount);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("history")]
        public IActionResult GetTransactionHistory()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID claim is missing" });
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID claim" });
            }

            var user = _userService.GetById(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            var transactions = _transactionService.GetTransactionHistory(userId);
            return Ok(transactions);
        }
    }
}
