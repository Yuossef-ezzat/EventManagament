using DomainLayer.Abstractions;
using DomainLayer.Contract;
using DomainLayer.Models.EventModule;
using ServiceAbstraction;
using Shared.Dtos.EventDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class EventService(IGenaricRepository<Event,int> _repository,INotifService notifService) : IEventService 
    {
        
        public async Task<Result<int>> AddAsync(CreateEventDto Dto)
        {
            if (Dto.Date <= DateTimeOffset.UtcNow)
                return Result<int>.Failure(new Error("Event date must be in the future"));
            if (Dto.MaxAttendance <= 0)
                return Result<int>.Failure(new Error("MaxAttendance must be greater than 0"));

            var newEvent = new Event
            {
                Title = Dto.Title,
                Description = Dto.Description,
                Date = Dto.Date,
                Location = Dto.Location,
                CategoryId = Dto.CategoryId,
                OrganizerId = Dto.OrganizerId,
                MaxAttendance = Dto.MaxAttendance,
                EventStatus = Dto.EventStatus,
                PaymentRequired = Dto.PaymentRequired,
            };

            var addedId = await _repository.AddAsync(newEvent);

            await notifService.SendNotification($"New event created: {newEvent.Title} on {newEvent.Date:MMMM dd, yyyy} at {newEvent.Location}.");
            return Result<int>.Success(addedId);
        }
        public async Task<Result<bool>> Delete(int Id)
        {
            var eventToDelete = await _repository.GetByIdAsync(Id);
            if (eventToDelete == null)
                return Result<bool>.Failure(new Error("Event not found"));

            var deleted = await _repository.Delete(eventToDelete);
            if (!deleted)
                return Result<bool>.Failure(new Error("Failed to delete event"));

            await notifService.SendNotification($"Event deleted: {eventToDelete.Title} scheduled on {eventToDelete.Date:MMMM dd, yyyy} at {eventToDelete.Location}.");

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<AllEventsDtos>>> GetAllAsync()
        {
            var events = await _repository.GetAllAsync();
            if (events == null || !events.Any())
                return Result<IEnumerable<AllEventsDtos>>.Failure(new Error ("No Events"));
            var eventsDtos = events.Select(e => new AllEventsDtos
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Location = e.Location,
                OrganizerId = e.OrganizerId,
                OrganizerName = e.Organizer.UserName, 
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name ,
                EventStatus = e.EventStatus.ToString(),
                PaymentRequired = e.PaymentRequired,
                MaxAttendance = e.MaxAttendance,
            });
            return Result< IEnumerable < AllEventsDtos >>.Success(eventsDtos) ;
        }

        public async Task<Result<DetailedEventDto>> GetByIdAsync(int id)
        {
            var eventEntity = await _repository.GetByIdAsync(id);
            if (eventEntity == null)
                return Result<DetailedEventDto>.Failure(new Error("Event not found"));

            var eventDto = new DetailedEventDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                Location = eventEntity.Location,
                OrganizerId = eventEntity.OrganizerId,
                OrganizerName = eventEntity.Organizer?.UserName ?? "",
                CategoryId = eventEntity.CategoryId,
                CategoryName = eventEntity.Category.Name,
                EventStatus = eventEntity.EventStatus,
                PaymentRequired = eventEntity.PaymentRequired,
                MaxAttendance = eventEntity.MaxAttendance,
                Registrations = eventEntity.Registrations?.Select(r => r.Id.ToString()).ToList() ?? new List<string>(),
                Payments = eventEntity.Payments?.Select(p => p.Id.ToString()).ToList() ?? new List<string>(),
                Notifications = eventEntity.Notifications?.Select(n => n.Id.ToString()).ToList() ?? new List<string>(),
            };
            return Result<DetailedEventDto>.Success(eventDto);
        }

        public async Task<Result<bool>> Update(DetailedEventDto dto)
        {
            if (dto == null)
                return Result<bool>.Failure(new Error("Invalid event data"));
            if (dto.Date <= DateTimeOffset.UtcNow)
                return Result<bool>.Failure(new Error("Event date must be in the future"));

            var existingEvent = await _repository.FindAsync(e => e.Id == dto.Id,new string[] { "Registrations.User" });
            if (existingEvent == null)
                return Result<bool>.Failure(new Error("Event not found"));

            existingEvent.Title = dto.Title;
            existingEvent.Description = dto.Description;
            existingEvent.Date = dto.Date;
            existingEvent.Location = dto.Location;
            existingEvent.OrganizerId = dto.OrganizerId;
            existingEvent.CategoryId = dto.CategoryId;
            existingEvent.EventStatus = dto.EventStatus;
            existingEvent.PaymentRequired = dto.PaymentRequired;
            existingEvent.MaxAttendance = dto.MaxAttendance;

            var updated = await _repository.Update(existingEvent);
            if (!updated)
                return Result<bool>.Failure(new Error("Failed to update event"));

            var registerdUsers = existingEvent.Registrations;

            await notifService.SendNotificationForRegisterdUserAtEvent(registerdUsers,$"Event updated: {existingEvent.Title} now scheduled on {existingEvent.Date:MMMM dd, yyyy} at {existingEvent.Location}.");

            return Result<bool>.Success(true);
        }
    }
}
