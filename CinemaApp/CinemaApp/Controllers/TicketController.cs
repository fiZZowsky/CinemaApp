using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Queries.GetAllTickets;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShow;
using CinemaApp.Application.CinemaApp.Queries.GetSeat;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _mediator.Send(new GetAllTicketsQuery());
            return View(tickets);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieShow = await _mediator.Send(new GetMovieShowQuery(ticketDto.StartTime, ticketDto.HallNumber));
            var seat = await _mediator.Send(new GetSeatQuery(ticketDto.HallNumber, ticketDto.RowNumber, ticketDto.SeatNumber));

            if (movieShow == null || seat == null)
            {
                ModelState.AddModelError("", "Invalid movie show or seat selection.");
                return RedirectToAction(nameof(Create));
            }

            ticketDto.MovieTitle = movieShow.Movie.Title;
            ticketDto.Language = movieShow.Movie.Language;
            ticketDto.Duration = movieShow.Movie.Duration;

            CreateTicketCommand command = new CreateTicketCommand(ticketDto, movieShow.Id, seat.Id);

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
