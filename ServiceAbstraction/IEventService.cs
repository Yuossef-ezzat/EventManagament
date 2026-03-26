using DomainLayer.Abstractions;
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
        Task<Result<DetailedEventDto>> GetByIdAsync(int id);
        Task<Result<IEnumerable<AllEventsDtos>>> GetAllAsync();
        Task<Result<int>> AddAsync(CreateEventDto entity);
        Task<Result<bool>> Update(DetailedEventDto entity);
        Task<Result<bool>> Delete(int Id);
    }
}
