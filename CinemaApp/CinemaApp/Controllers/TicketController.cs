using CinemaApp.Application.Dtos;
using CinemaApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task <IActionResult> Create(TicketDto ticket)
        {
            await _ticketService.Create(ticket);
            return RedirectToAction(nameof(Create)); // TODO: Refactor
        }
    }
}
