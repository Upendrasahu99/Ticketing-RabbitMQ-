using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Threading.Tasks;
using System;

namespace Ticketing.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus; //Bus class is core Infrastructure that handles the routing, queuing and delivery of messages.
        public TicketController(IBus bus)
        {
            _bus = bus;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
         {
            if (ticket != null)
            {
                ticket.BookedOn = DateTime.Now; // set the booking time 
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");// URI(Uniform Resource Identifier), Uri is used to identify resource on the internet or wihin a systemm. 
                //"uri" is created to represent the address of a RabbitMQ message queue on the local machine.
                var endPoint = await _bus.GetSendEndpoint(uri);// Get an endpoint to send the message to the specific queue.
                await endPoint.Send(ticket); // send the ticket object to the the specified queue.
                return Ok();
            }
            return BadRequest();
        }
    }
}
