using DomainLayer.Abstractions;
using Shared.Dtos.RegisrationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IRegisrationService
    {
        Task<Result<RegistrationDto>> RegisterUserToEventAsync(RegistrationRequestDto requestDto);
        Task<Result<RegistrationDto>> GetUserRegistrationsAsync(int userId);
        Task<Result<bool>> CancelRegistrationAsync(CancleRegisrationDto cancleRegisration);
        Task<bool> IsUserAlreadyRegisteredAsync(int userId, int eventId);
    }
}
