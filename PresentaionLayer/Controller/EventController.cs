using Microsoft.AspNetCore.Mvc;
using PresentaionLayer.Controller;
using ServiceAbstraction;
using Shared.Dtos.EventDtos;

namespace EventManagament.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(IEventService eventService) : ApiBaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await eventService.GetAllAsync();
        if (result == null || result.IsFailure)
            return NotFound(new { message = result?.Error?.Descriprion ?? "No events found." });

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await eventService.GetByIdAsync(id);
        if (result == null || result.IsFailure)
            return NotFound(new { message = result?.Error?.Descriprion ?? $"Event with id {id} not found." });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventDto createDto)
    {
        if (createDto == null)
            return BadRequest(new { message = "Event payload cannot be null." });

        var result = await eventService.AddAsync(createDto);
        if (result == null || result.IsFailure)
            return BadRequest(new { message = result?.Error?.Descriprion ?? "Failed to create event." });

        var createdId = result.Value;

        //if (createdId <= 0)
        //    return BadRequest(new { message = "Failed to create event." });

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DetailedEventDto updateDto)
    {
        if (updateDto == null)
            return BadRequest(new { message = "Event payload cannot be null." });

        if (id != updateDto.Id)
            return BadRequest(new { message = "Id in route must match id in payload." });

        var result = await eventService.Update(updateDto);

        if (result == null || result.IsFailure)
            return NotFound(new { message = result?.Error?.Descriprion ?? $"Event with id {id} not found." });

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await eventService.Delete(id);
        if (result == null || result.IsFailure)
            return NotFound(new { message = result?.Error?.Descriprion ?? $"Event with id {id} not found." });

        return Ok(result.Value);
    }
}
