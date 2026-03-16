using DomainLayer.Abstractions;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaionLayer.Controller;
using ServiceAbstraction;

namespace EventManagament.Controllers
{

    public class PaymobController(ILogger<PaymobCallback> logger,IPayMobService payMobService) : ApiBaseController
    {
        [HttpPost("Pay/{amountCents}/{attendid}")]
        public async Task<IActionResult> Pay(int amountCents, int attendid)
        {
            try
            {
                var iframeUrl = await payMobService.PayWithCard(amountCents,attendid) ;

                return Ok(new { iframeUrl = iframeUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("callback/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymobCallback([FromBody] PaymobCallback payload,int Id)
        {
            bool result = true;
            try
            {
                logger.LogInformation("Received Paymob callback: {@Payload}", payload);

                var hmacHeader = Request.Query["hmac"].FirstOrDefault();

                result = await payMobService.PaymobCallback(payload, hmacHeader,Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error logging Paymob callback");
                result = false;
            }
            return result ? Ok(result) : BadRequest(result);
        }
    }
}
