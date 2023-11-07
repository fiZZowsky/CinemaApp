using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket
{
    public class GetPdfQueryHandler : IRequestHandler<GetPdfQuery, byte[]>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetPdfQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<byte[]> Handle(GetPdfQuery request, CancellationToken cancellationToken)
        {
            var pdf = await _ticketRepository.CreatePdf(request.Uid, request.HtmlContent);
            return pdf;
        }
    }
}
