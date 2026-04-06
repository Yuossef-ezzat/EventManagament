using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using ServiceLayer.Services;
using Shared.Dtos.AuthDtos;
using Shared.Dtos.RegisrationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentaionLayer.Controller
{
    public class RegistrationController(IRegisrationService regisrationService) : ApiBaseController
    {
        private readonly IRegisrationService _registrationService= regisrationService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _registrationService.RegisterUserToEventAsync(requestDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
        [HttpGet("{userId}")]
        [Authorize(Roles = "Attendee, Admin")]
        public async Task<IActionResult> GetUserRegistrations(int userId)
        {
            var result = await _registrationService.GetUserRegistrationsAsync(userId);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }
        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelRegistration([FromBody] CancleRegisrationDto cancleRegisration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _registrationService.CancelRegistrationAsync(cancleRegisration);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(new { message = "You Are Canceld Successfully" });
        }
    }
}
