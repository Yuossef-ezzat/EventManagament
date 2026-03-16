using Shared.Dtos.EventDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IEventService
    {
        Task<DetailedEventDto?> GetByIdAsync(int id);
        Task<IEnumerable<AllEventsDtos>> GetAllAsync();
        Task<int> AddAsync(CreateEventDto entity);
        Task<bool> Update(DetailedEventDto entity);
        Task<bool> Delete(int Id);


    }
}
