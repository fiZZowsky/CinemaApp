using AutoMapper;
using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateCheckoutInDatabase;
using CinemaApp.Application.CinemaApp.Commands.CreateCheckoutSession;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Commands.EditTicket;
using CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement;
using CinemaApp.Application.CinemaApp.Queries.GetAllTickets;
using CinemaApp.Application.CinemaApp.Queries.GetHallByNumber;
using CinemaApp.Application.CinemaApp.Queries.GetMovieById;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShow;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShowByEncodedTitle;
using CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket;
using CinemaApp.Application.CinemaApp.Queries.GetSeat;
using CinemaApp.Application.CinemaApp.Queries.GetTicketByGuid;
using CinemaApp.Application.CinemaApp.Queries.GetTicketByUser;
using CinemaApp.Application.CinemaApp.Queries.GetUnavailableSeats;
using CinemaApp.Domain.Entities;
using CinemaApp.MVC.Extensions;
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
        private readonly IMapper _mapper;
        private readonly IConverter _pdfConverter;
        private readonly StripeSettings _stripeSettings;

        public TicketController(IMediator mediator, IMapper mapper, IConverter pdfConverter, IOptions<StripeSettings> stripeSettings)
        {
            _mediator = mediator;
            _mapper = mapper;
            _pdfConverter = pdfConverter;
            _stripeSettings = stripeSettings.Value;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var tickets = await _mediator.Send(new GetAllTicketsQuery());
            return View(tickets);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string encodedTitle, DateTime startTime)
        {
            var movieDto = await _mediator.Send(new GetMovieShowByEncodedTitleQuery(encodedTitle, startTime));
            var ticketDto = _mapper.Map<TicketDto>(movieDto);

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

            string[] seatNumbersArray = selectedSeatNumbers.Split(',');
            int selectedSeatsCount = seatNumbersArray.Length;

            if (ticketDto.NormalPriceSeats + ticketDto.ReducedPriceSeats != selectedSeatsCount)
            {
                this.SetNotification("error", "Sum of normal and reduced price seats must match the number of selected seats");
                return BadRequest(ModelState);
            }

            ticketDto.SeatNumber = selectedSeatNumbers.Split(',').Select(int.Parse).ToList();
            ticketDto.RowNumber = selectedRowNumbers.Split(',').Select(int.Parse).ToList();

            Func<string> successUrl = () => Url.Action("CreateTicket", "Ticket", ticketDto, Request.Scheme);
            Func<string> cancelURL = () => Url.Action("CancelledPurchase", "Ticket", new
            {
                movieTitle = ticketDto.MovieTitle,
                language = ticketDto.Language,
                duration = ticketDto.Duration,
                startTime = ticketDto.StartTime,
                hallNumber = ticketDto.HallNumber
            }, Request.Scheme);

            CreateCheckoutSessionCommand command = new CreateCheckoutSessionCommand(ticketDto, successUrl, cancelURL);
            var session = await _mediator.Send(command);
            TempData["SessionId"] = session.Id;

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

            // Add purchase to user history
            if (TempData.TryGetValue("SessionId", out object sessionIdObj) && sessionIdObj is string sessionId)
            {
                CreateCheckoutInDatabaseCommand checkoutCommand = new CreateCheckoutInDatabaseCommand(sessionId, ticket.Guid);
                await _mediator.Send(checkoutCommand);
            }

            // Send Created Ticket in Mail
            await SendEmailWithTicket(ticket.Guid);

            this.SetNotification("success", "Successfully bought new ticket.");

            return RedirectToAction("Index", "Ticket");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPdfTicketFile(Guid guid)
        {
            byte[] pdfBytes = await GeneratePDF(guid);
            return File(pdfBytes, "application/pdf", "ticket.pdf");
        }

        [Authorize]
        private async Task<byte[]> GeneratePDF(Guid guid)
        {
            var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "Ticket.html");
            var htmlContent = System.IO.File.ReadAllText("Templates\\Ticket.html");

            byte[] pdfBytes = await _mediator.Send(new GetPdfQuery(guid, htmlContent));

            return pdfBytes;
        }

        [Authorize]
        private async Task<IActionResult> SendEmailWithTicket(Guid guid)
        {
            var ticketPdf = await GeneratePDF(guid);
            string emailTemplateText = System.IO.File.ReadAllText("Templates\\Email.html");

            SendEmailWithAttachementCommand command = new SendEmailWithAttachementCommand(emailTemplateText, ticketPdf);

            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetNotAvailableSeats(int hallNumber, DateTime startTime)
        {
            var unavailableSeats = await _mediator.Send(new GetUnavailableSeatsQuery(hallNumber, startTime));
            return Ok(unavailableSeats);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CancelledPurchase(string movieTitle, string language, int duration, DateTime startTime, int hallNumber)
        {
            var ticketDto = new TicketDto
            {
                MovieTitle = movieTitle,
                Language = language,
                Duration = duration,
                StartTime = startTime,
                HallNumber = hallNumber
            };

            return RedirectToAction("Create", "Ticket", ticketDto);
        }

        // https://10.0.2.2:7190/Ticket/TicketCheck/{guid}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Ticket/TicketCheck/{guid}")]
        public async Task<IActionResult> TicketCheck(Guid guid)
        {
            var ticket = await _mediator.Send(new GetTicketByGuidQuery(guid));
            return View(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TicketCheck(Guid guid, bool isScanned)
        {
            if (isScanned == true)
            {
                this.SetNotification("success", "Ticket has already been scanned.");
            }
            else
            {
                EditTicketCommand command = new EditTicketCommand(guid);
                await _mediator.Send(command);
                this.SetNotification("success", "Ticket scanned successfully.");
            }

            return RedirectToAction(nameof(TicketCheck), new { guid });
        }
    }
}
