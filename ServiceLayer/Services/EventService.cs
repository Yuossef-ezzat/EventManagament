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
    public class EventService(IGenaricRepository<Event,int> _repository) : IEventService 
    {
        public Task<int> AddAsync(CreateEventDto Dto)
        {
            if (Dto.Date <= DateTimeOffset.UtcNow)
                throw new Exception("Event date must be in the future");
            if (Dto.MaxAttendance <= 0)
                throw new Exception("MaxAttendance must be greater than 0");
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
            return _repository.AddAsync(newEvent);
        }

        public async Task<bool> Delete(int Id)
        {
            var eventToDelete = await _repository.GetByIdAsync(Id);
            if (eventToDelete == null)
                return false;
            return await _repository.Delete(eventToDelete);
        }

        public async Task<IEnumerable<AllEventsDtos>> GetAllAsync()
        {
            var events = await _repository.GetAllAsync();
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
            return eventsDtos;
        }

        public async Task<DetailedEventDto?> GetByIdAsync(int id)
        {
            var eventEntity = await _repository.GetByIdAsync(id);
            if (eventEntity == null)
                return null;
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
                Registrations = eventEntity.Registrations?.Select(r => r.Id.ToString()).ToList() ?? [], 
                Payments = eventEntity.Payments?.Select(p => p.Id.ToString()).ToList() ?? [],
                Notifications = eventEntity.Notifications?.Select(n => n.Id.ToString()).ToList() ?? [],
            };
            return eventDto;
        }

        public async Task<bool> Update(DetailedEventDto dto)
        {
            if (dto == null) return false;
            if (dto.Date <= DateTimeOffset.UtcNow)
                throw new Exception("Event date must be in the future");
            var existingEvent = await _repository.GetByIdAsync(dto.Id);
            if (existingEvent == null) return false;

            existingEvent.Title = dto.Title;
            existingEvent.Description = dto.Description;
            existingEvent.Date = dto.Date;
            existingEvent.Location = dto.Location;
            existingEvent.OrganizerId = dto.OrganizerId;
            existingEvent.CategoryId = dto.CategoryId;
            existingEvent.EventStatus = dto.EventStatus;
            existingEvent.PaymentRequired = dto.PaymentRequired;
            existingEvent.MaxAttendance = dto.MaxAttendance;

            return  await _repository.Update(existingEvent);
        }
    }
}
