using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PresentaionLayer.Controller
{
    public class AuthController(IAuthService authService) : ApiBaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailExists = await authService.CheckEmailAsync(dto.Email);
            if (emailExists)
                return Conflict(new { Message = "Email is already registered." });

            try
            {
                var result = await authService.RgisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var isUnauth = ex.Message.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase);
                return isUnauth
                    ? Unauthorized(new { Message = "Invalid email or password." })
                    : NotFound(new { Message = ex.Message });
            }
        }

        // GET: api/auth/me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email is null)
                return Unauthorized(new { Message = "Invalid token." });

            try
            {
                var result = await authService.GetCurrentUserAsync(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // GET: api/auth/check-email?email=...
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { Message = "Email is required." });

            var exists = await authService.CheckEmailAsync(email);
            return Ok(new { Exists = exists });
        }
    }
}
