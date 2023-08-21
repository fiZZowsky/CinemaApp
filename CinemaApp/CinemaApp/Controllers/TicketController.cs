using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Queries.GetAllTickets;
using CinemaApp.Application.CinemaApp.Queries.GetTicketById;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShow;
using CinemaApp.Application.CinemaApp.Queries.GetSeat;
using DinkToPdf.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf;

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
        public async Task<IActionResult> Index()
        {
            var tickets = await _mediator.Send(new GetAllTicketsQuery());
            return View(tickets);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GeneratePDF(int Id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery(Id));

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Ticket"
            };

            var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "Ticket.html");
            var htmlContent = System.IO.File.ReadAllText("Templates\\Ticket.html");

            string base64Image = Convert.ToBase64String(ticket.QRCode);
            string QRCode = $"data:image/png;base64,{base64Image}";

            htmlContent = htmlContent
                .Replace("@Model.Title", ticket.MovieTitle)
                .Replace("@Model.Language", ticket.Language)
                .Replace("@Model.Duration", ticket.Duration.ToString())
                .Replace("@Model.StartTime", ticket.StartTime.ToString())
                .Replace("@Model.HallNumber", ticket.HallNumber.ToString())
                .Replace("@Model.RowNumber", ticket.RowNumber.ToString())
                .Replace("@Model.SeatNumber", ticket.SeatNumber.ToString())
                .Replace("@Model.QRCode", QRCode);

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _pdfConverter.Convert(pdf);

            return File(file, "application/pdf", "Ticket.pdf");
        }
    }
}
