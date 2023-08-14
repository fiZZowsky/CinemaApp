using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IMovieShowService _movieShowService;
        private readonly ISeatService _seatService;

        public TicketController(ITicketService ticketService, IMovieShowService movieShowService, ISeatService seatService)
        {
            _ticketService = ticketService;
            _movieShowService = movieShowService;
            _seatService = seatService;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAll();
            return View(tickets);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieShow = await _movieShowService.GetByData(ticketDto.StartTime, ticketDto.HallNumber);
            var seat = await _seatService.GetByData(ticketDto.HallNumber, ticketDto.RowNumber, ticketDto.SeatNumber);

            if (movieShow == null || seat == null)
            {
                ModelState.AddModelError("", "Invalid movie show or seat selection.");
                return RedirectToAction(nameof(Create));
            }

            ticketDto.MovieTitle = movieShow.Movie.Title;
            ticketDto.Language = movieShow.Movie.Language;
            ticketDto.Duration = movieShow.Movie.Duration;

            await _ticketService.Create(ticketDto, movieShow.Id, seat.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
