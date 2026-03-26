using Microsoft.AspNetCore.Mvc;
using PresentaionLayer.Controller;
using ServiceAbstraction;
using Shared.Dtos.EventDtos;

namespace EventManagament.Controllers
{
    public class EventController(IEventService eventService) : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventDto = await eventService.GetByIdAsync(id);
            if (eventDto == null)
                return NotFound(new { message = $"Event with id {id} not found." });

            return Ok(eventDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto createDto)
        {
            if (createDto == null)
                return BadRequest(new { message = "Event payload cannot be null." });

            try
            {
                var createdId = await eventService.AddAsync(createDto);
                if (createdId <= 0)
                    return BadRequest(new { message = "Failed to create event." });

                return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetailedEventDto updateDto)
        {
            if (updateDto == null)
                return BadRequest(new { message = "Event payload cannot be null." });

            if (id != updateDto.Id)
                return BadRequest(new { message = "Id in route must match id in payload." });

            try
            {
                var updated = await eventService.Update(updateDto);
                if (!updated)
                    return NotFound(new { message = $"Event with id {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await eventService.Delete(id);
                if (!deleted)
                    return NotFound(new { message = $"Event with id {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
