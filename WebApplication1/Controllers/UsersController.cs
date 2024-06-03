using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("accountinfo")]
        public IActionResult GetAccountInfo()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _userService.GetById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { username = user.Username, balance = user.Balance ,userId = user.Id});
        }

        [HttpGet("username/{userId}")]
        public IActionResult GetUsername(int userId)
        {
            var user = _userService.GetById(userId);
            if (user != null)
            {
                return Ok(new { username = user.Username });
            }
            return NotFound("User not found");
        }


        [HttpGet("balance")]
        public IActionResult GetBalance()
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

            return Ok(new { balance = user.Balance });
        }
    }
}