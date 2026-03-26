using DomainLayer.Abstractions;
using Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthService
    {
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto);
        Task<Result<UserDto>> RgisterAsync(RegisterDto RegisterDto);
        Task<Result<bool>> CheckEmailAsync(string email);
        Task<Result<UserDto>> GetCurrentUserAsync(string email);
    }
}
