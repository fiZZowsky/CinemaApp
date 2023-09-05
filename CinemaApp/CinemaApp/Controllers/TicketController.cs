using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement;
using CinemaApp.Application.CinemaApp.Queries.GetAllTickets;
using CinemaApp.Application.CinemaApp.Queries.GetHallByNumber;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShow;
using CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket;
using CinemaApp.Application.CinemaApp.Queries.GetSeat;
using CinemaApp.Application.CinemaApp.Queries.GetTicketByUser;
using DinkToPdf.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConverter _pdfConverter;

        public TicketController(IMediator mediator, IConverter pdfConverter)
        {
            _mediator = mediator;
            _pdfConverter = pdfConverter;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tickets = await _mediator.Send(new GetAllTicketsQuery());
            return View(tickets);
        }

        [Authorize]
        public ActionResult Create(string movieTitle, string language, int duration, DateTime startTime, int hallNumber)
        {
            var ticketDto = new TicketDto
            {
                MovieTitle = movieTitle,
                Language = language,
                Duration = duration,
                StartTime = startTime,
                HallNumber = hallNumber,
                RowNumber = new List<int>(),
                SeatNumber = new List<int>()
            };

            return View(ticketDto);
        }

        [HttpGet]
        [Route("Ticket/GetHallData/{hallNumber}")]
        public async Task<IActionResult> GetHallData(int hallNumber)
        {
            var hall = await _mediator.Send(new GetHallByNumberQuery(hallNumber));
            return Json(new { Seats = hall.PlacesInARow, Rows = hall.NumberOfRows });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(TicketDto ticketDto, string selectedSeatNumbers, string selectedRowNumbers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ticketDto.SeatNumber = selectedSeatNumbers.Split(',').Select(int.Parse).ToList();
            ticketDto.RowNumber = selectedRowNumbers.Split(',').Select(int.Parse).ToList();

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
            DateTime currentTime = DateTime.Now;
            DateTime formattedPurchaseDate = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 0);
            ticketDto.PurchaseDate = formattedPurchaseDate;

            CreateTicketCommand command = new CreateTicketCommand(ticketDto, movieShow.Id, seat);

            await _mediator.Send(command);
            var ticket = await _mediator.Send(new GetTicketByUserQuery(formattedPurchaseDate, ticketDto.MovieTitle));
            await SendEmailWithTicket(ticket.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPdfTicketFile(int id)
        {
            byte[] pdfBytes = await GeneratePDF(id);
            return File(pdfBytes, "application/pdf", "ticket.pdf");
        }

        private async Task<byte[]> GeneratePDF(int id)
        {
            var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "Ticket.html");
            var htmlContent = System.IO.File.ReadAllText("Templates\\Ticket.html");

            byte[] pdfBytes = await _mediator.Send(new GetPdfQuery(id, htmlContent));

            return pdfBytes;
        }

        private async Task<IActionResult> SendEmailWithTicket(int id)
        {
            var ticketPdf = await GeneratePDF(id);
            string emailTemplateText = System.IO.File.ReadAllText("Templates\\Email.html");

            SendEmailWithAttachementCommand command = new SendEmailWithAttachementCommand(emailTemplateText, ticketPdf);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
