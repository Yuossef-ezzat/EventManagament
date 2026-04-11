
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;

namespace PresentaionLayer.Controller;
[ApiController]
[Route("api/[controller]")]
public class NotificationController(INotifService notifService) : ApiBaseController
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetNotificationById(int Id)
    {
        var result = await notifService.GetNotifsById(Id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("{notifid}")]
    public async Task<IActionResult> MarkNotificationAsRead(int notifid)
    {
        var result = await notifService.MarkNotifAsRead(notifid);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
