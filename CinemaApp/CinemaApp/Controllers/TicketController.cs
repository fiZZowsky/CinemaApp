using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateCheckoutSession;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement;
using CinemaApp.Application.CinemaApp.Queries.GetAllTickets;
using CinemaApp.Application.CinemaApp.Queries.GetHallByNumber;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShow;
using CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket;
using CinemaApp.Application.CinemaApp.Queries.GetSeat;
using CinemaApp.Application.CinemaApp.Queries.GetTicketByUser;
using CinemaApp.Application.CinemaApp.Queries.GetUnavailableSeats;
using CinemaApp.Domain.Entities;
using DinkToPdf.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CinemaApp.MVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConverter _pdfConverter;
        private readonly StripeSettings _stripeSettings;

        public TicketController(IMediator mediator, IConverter pdfConverter, IOptions<StripeSettings> stripeSettings)
        {
            _mediator = mediator;
            _pdfConverter = pdfConverter;
            _stripeSettings = stripeSettings.Value;
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

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(TicketDto ticketDto, string selectedSeatNumbers, string selectedRowNumbers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ticketDto.SeatNumber = selectedSeatNumbers.Split(',').Select(int.Parse).ToList();
            ticketDto.RowNumber = selectedRowNumbers.Split(',').Select(int.Parse).ToList();

            Func<string> successUrl = () => Url.Action("CreateTicket", "Ticket", ticketDto, Request.Scheme);
            Func<string> cancelURL = () => Url.Action("CancelledPurchase", "Ticket", ticketDto, Request.Scheme);

            CreateCheckoutSessionCommand command = new CreateCheckoutSessionCommand(ticketDto, successUrl, cancelURL);
            var session = await _mediator.Send(command);

            return Ok(new { sessionUrl = session.Url });
        }

        [Authorize]
        public async Task<IActionResult> CreateTicket(TicketDto ticketDto)
        {
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

            //Create New Ticket in Db
            CreateTicketCommand command = new CreateTicketCommand(ticketDto, movieShow.Id, seat);

            await _mediator.Send(command);
            var ticket = await _mediator.Send(new GetTicketByUserQuery(formattedPurchaseDate, ticketDto.MovieTitle));

            //Send Created Ticket in Mail
            await SendEmailWithTicket(ticket.Id);

            return RedirectToAction("Index", "Ticket");
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotAvailableSeats(int hallNumber, DateTime startTime)
        {
            var unavailableSeats = await _mediator.Send(new GetUnavailableSeatsQuery(hallNumber, startTime));
            Console.WriteLine("Unavailable seats from controller: " + unavailableSeats);
            return Ok(unavailableSeats);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CancelledPurchase()
        {
            return View();
        }
    }
}
