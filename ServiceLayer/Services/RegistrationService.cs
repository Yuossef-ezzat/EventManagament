using DomainLayer.Abstractions;
using DomainLayer.Contract;
using DomainLayer.Models.EventModule;
using DomainLayer.Models.PaymentModule;
using DomainLayer.Models.Registeration;
using Microsoft.EntityFrameworkCore;
using ServiceAbstraction;
using Shared.Dtos.RegisrationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class RegistrationService(IGenaricRepository<Registration, int> repository) : IRegisrationService
    {
        private readonly IGenaricRepository<Registration, int> _repository = repository;

        private readonly IPayMobService _paymentService;
        public async Task<Result<bool>> CancelRegistrationAsync(CancleRegisrationDto cancleRegisration)
        {
            var registration = await _repository.FindAsync(
        r => r.Id == cancleRegisration.RegistrationId && r.UserId == cancleRegisration.UserId, includes: null
    );

            if (registration == null)
                return Result<bool>.Failure(new Error("There Is No Regestration"));

            if (registration.UserId != cancleRegisration.UserId)
                return Result<bool>.Failure(new Error("You Don't Authorized To Cancel the Registration"));

            await _repository.Delete(registration);

            return Result<bool>.Success(true);
        }

        public async Task<Result<RegistrationDto>> GetUserRegistrationsAsync(int userId)
        {
            var registration = await _repository.FindAsync(
                                         r => r.UserId == userId,
                                         includes: new[] { "Event" }
                                                           );

            if (registration == null)
                return Result<RegistrationDto>.Failure(new Error("There is no information about that user"));

            var result = new RegistrationDto
            {
                RegistrationId = registration.Id,
                UserId = registration.UserId,
                EventId = registration.EventId,
                EventTitle = registration.Event.Title,
                EventDate = registration.Event.Date,
                EventLocation = registration.Event.Location,
                PaymentRequired = registration.Event.PaymentRequired,
                Status = registration.RegisterationStatus.ToString(),
                PaymentStatus = registration.paymentStatus.ToString(),
                RegisteredAt = registration.RegisteredAt
            };

            return Result<RegistrationDto>.Success(result);
        }

        public async Task<bool> IsUserAlreadyRegisteredAsync(int userId, int eventId)
        {
            return await 
                _repository.AnyAsync(r => r.UserId == userId && r.EventId == eventId);
        }

        public async Task<Result<RegistrationDto>>  RegisterUserToEventAsync(RegistrationRequestDto requestDto)
        {
            var UserRegisterd = await IsUserAlreadyRegisteredAsync(requestDto.UserId, requestDto.EventId);
            if (UserRegisterd)
                throw new InvalidOperationException("You'r already registered for this event.");
            var eventEntity = await _repository.FindAsync(e => e.Id == requestDto.EventId, includes: null);

            if (eventEntity == null)
                throw new KeyNotFoundException("الحدث غير موجود");

            
            if (eventEntity.RegisterationStatus == RegistrationStatus.canceled || eventEntity.RegisterationStatus == RegistrationStatus.finished)
                throw new InvalidOperationException("لا يمكن التسجيل في حدث ملغي أو منتهي");

            
            var registration = new Registration
            {
                UserId = requestDto.UserId,
                EventId = requestDto.EventId,
                RegisteredAt = DateTime.UtcNow,

                // لو الحدث مجاني → Confirmed مباشرة
                // لو الحدث مدفوع → Pending لحد ما يدفع
                RegisterationStatus = eventEntity.Event.PaymentRequired ? RegistrationStatus.Pending : RegistrationStatus.Paid,
                paymentStatus = eventEntity.Event.PaymentRequired ? PaymentStatus.pending : PaymentStatus.NorRequired

            };
            await _repository.AddAsync(registration);

            var response = new RegistrationDto
            {
                RegistrationId = registration.Id,
                UserId = registration.UserId,
                EventId = registration.EventId,
                Status = registration.RegisterationStatus.ToString(),
                PaymentStatus = registration.paymentStatus.ToString(),
                RegisteredAt = registration.RegisteredAt
            };

            return Result<RegistrationDto>.Success(response);


        }
    }
}
