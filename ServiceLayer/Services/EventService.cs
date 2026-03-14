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
                //Notifications = Dto.Notifications
                PaymentRequired = Dto.PaymentRequired,
            };
            return _repository.AddAsync(newEvent);
        }

        public Task<bool> Delete(DetailedEventDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllEventsDtos>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DetailedEventDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(DetailedEventDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
