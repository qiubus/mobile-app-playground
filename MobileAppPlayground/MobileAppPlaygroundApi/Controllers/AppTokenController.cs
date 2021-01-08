using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using MobileAppPlaygroundApi.Models;

namespace MobileAppPlaygroundApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppTokenRegisterController : ControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> Put(ClientRegistration clientRegistration)
        {
            //var registrationToken = "YOUR_REGISTRATION_TOKEN";

            // See documentation on defining a message payload.
            var message = new Message
            {
                Data = new Dictionary<string, string>()
                {
                    { "clientId", clientRegistration.ClientId }
                },
                Notification = new Notification
                {
                    Body = "text send when app token registered"
                },
                Token = clientRegistration.Token
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);

            return Ok();
        }
    }
}
