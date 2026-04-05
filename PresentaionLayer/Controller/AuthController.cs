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

namespace PresentaionLayer.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emailCheck = await authService.CheckEmailAsync(dto.Email);

        if (emailCheck == null || emailCheck.IsFailure)
            return BadRequest(new { Message = emailCheck?.Error?.Descriprion ?? "Failed to check email." });
        
        if (emailCheck.Value)
            return Conflict(new { Message = "Email is already registered." });

        var result = await authService.RgisterAsync(dto);
        if (result == null || result.IsFailure)
            return BadRequest(new { Message = result?.Error?.Descriprion ?? "Registration failed." });

        return Ok(result.Value);
    }
    [HttpPost("CreateOrganizer")]
    public async Task<IActionResult> CreateOrganizer([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emailCheck = await authService.CheckEmailAsync(dto.Email);

        if (emailCheck == null || emailCheck.IsFailure)
            return BadRequest(new { Message = emailCheck?.Error?.Descriprion ?? "Failed to check email." });
        
        if (emailCheck.Value)
            return Conflict(new { Message = "Email is already registered." });

        var result = await authService.CreateOrganizerAsync(dto);
        if (result == null || result.IsFailure)
            return BadRequest(new { Message = result?.Error?.Descriprion ?? "Registration failed." });

        return Ok(result.Value);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.LoginAsync(dto);
        if (result == null || result.IsFailure)
        {
            var message = result?.Error?.Descriprion ?? "Login failed.";
            var isUnauth = message.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase);
            return isUnauth
                ? Unauthorized(new { Message = "Invalid email or password." })
                : BadRequest(new { Message = message });
        }

        return Ok(result.Value);
    }

    // GET: api/auth/me
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null)
            return Unauthorized(new { Message = "Invalid token." });

        var result = await authService.GetCurrentUserAsync(email);
        if (result == null || result.IsFailure)
            return NotFound(new { Message = result?.Error?.Descriprion ?? "User not found." });

        return Ok(result.Value);
    }


    // GET: api/auth/check-email?email=...
    [HttpGet("check-email")]
    public async Task<IActionResult> CheckEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { Message = "Email is required." });

        var res = await authService.CheckEmailAsync(email);
        if (res == null || res.IsFailure)
            return BadRequest(new { Message = res?.Error?.Descriprion ?? "Failed to check email." });

        return Ok(new { Exists = res.Value });
    }
}
