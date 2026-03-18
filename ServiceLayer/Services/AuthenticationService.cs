using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration)
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }
        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new Exception(message:"Email Not Found");
            return new UserDto()
            {
                Email = user.Email!,
                Token = await GenerateJwtToken(user)
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User is null)
                throw new Exception(message:"Email not found");

            var res = await _userManager.CheckPasswordAsync(User, loginDto.Password);

            if (!res)
                throw new Exception(message: "UnauthorizedException");

            return new UserDto
            {
                Email = User.Email!,
                Token = await GenerateJwtToken(User)
            };

        }


        public async Task<UserDto> RgisterAsync(RegisterDto RegisterDto)
        {
            var user = new ApplicationUser
            {
                Email = RegisterDto.Email,
                UserName = RegisterDto.UserName ?? RegisterDto.Email.Split("@")[0],
            };
            var result = await _userManager.CreateAsync(user, RegisterDto.Password);
            if (result.Succeeded)
                return new UserDto
                {
                    Email = user.Email!,
                    Token = await GenerateJwtToken(user)
                };
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new Exception(message:$"Validation Failed {errors.Select(s=>s)}");
            }
        }


        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            // create the payload from the user info {Claims}
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()!)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var SecretKey = _configuration["JwtOptions:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));

            var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Credentials
            );

            var TokenHandler = new JwtSecurityTokenHandler().WriteToken(Token);

            return TokenHandler;
        }

    }
}
