using DomainLayer.Abstractions;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
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
    public class AuthService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration) : IAuthService
    {
        public async Task<Result<bool>> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return Result<bool>.Success(user is not null);
        }

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<UserDto>.Failure(new Error("Email not found"));

            try
            {
                var token = await GenerateJwtToken(user);
                return Result<UserDto>.Success(new UserDto()
                {
                    Email = user.Email!,
                    userName = user.UserName!,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                return Result<UserDto>.Failure(new Error("Email not found"));

            var res = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!res)
                return Result<UserDto>.Failure(new Error("Unauthorized"));

            try
            {
                var token = await GenerateJwtToken(user);
                return Result<UserDto>.Success(new UserDto
                {
                    Email = user.Email!,
                    userName = user.UserName!,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(new Error(ex.Message));
            }
        }
        public async Task<Result<UserDto>> CreateOrganizerAsync(RegisterDto RegisterDto)
        {
            var user = new ApplicationUser
            {
                Email = RegisterDto.Email,
                UserName = RegisterDto.UserName ?? RegisterDto.Email.Split("@")[0],
            };
            var result = await _userManager.CreateAsync(user, RegisterDto.Password);
            if (result.Succeeded)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, "Organizer");
                    var token = await GenerateJwtToken(user);
                    return Result<UserDto>.Success(new UserDto
                    {
                        Email = user.Email!,
                        userName = user.UserName!,
                        Token = token
                    });
                }
                catch (Exception ex)
                {
                    return Result<UserDto>.Failure(new Error(ex.Message));
                }
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<UserDto>.Failure(new Error($"Validation Failed: {string.Join(", ", errors)}"));
            }
        }
        
        public async Task<Result<UserDto>> RgisterAsync(RegisterDto RegisterDto)
        {
            var user = new ApplicationUser
            {
                Email = RegisterDto.Email,
                UserName = RegisterDto.UserName ?? RegisterDto.Email.Split("@")[0],
            };
            var result = await _userManager.CreateAsync(user, RegisterDto.Password);
            if (result.Succeeded)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, "Attendee");
                    //await _userManager.AddToRoleAsync(user, "Admin");
                    var token = await GenerateJwtToken(user);
                    return Result<UserDto>.Success(new UserDto
                    {
                        Email = user.Email!,
                        userName = user.UserName!,
                        Token = token
                    });
                }
                catch (Exception ex)
                {
                    return Result<UserDto>.Failure(new Error(ex.Message));
                }
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<UserDto>.Failure(new Error($"Validation Failed: {string.Join(", ", errors)}"));
            }
        }
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            // create the payload from the user info {Claims}
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!)
                };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var SecretKey = _configuration["JwtOptions:SecretKey"];
            if (string.IsNullOrEmpty(SecretKey))
                throw new Exception("JWT SecretKey is not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: Credentials
            );

            var TokenHandler = new JwtSecurityTokenHandler().WriteToken(Token);
            return TokenHandler;
        }
    }
}
