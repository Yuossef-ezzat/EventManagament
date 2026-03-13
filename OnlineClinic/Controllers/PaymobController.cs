using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManagament.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymobController : ControllerBase
    {
        //[HttpPost("callback")]
        //[AllowAnonymous]
        //public async Task<IActionResult> PaymobCallback([FromBody] PaymobCallback payload)
        //{
        //    ResponseModel<object> result = null!;
        //    try
        //    {
        //        Logger.LogInformation("Received Paymob callback: {@Payload}", payload);

        //        var hmacHeader = Request.Query["hmac"].FirstOrDefault();

        //        result = await walletService.PaymobCallback(payload, hmacHeader);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Error logging Paymob callback");
        //    }
        //    return result.IsSuccess ? Ok(result) : BadRequest(result.Message);
        //}
    }
}
