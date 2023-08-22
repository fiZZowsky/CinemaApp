using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket
{
    public class GetPdfQuery : IRequest<byte[]>
    {
        public int Id { get; set; }
        public string HtmlContent { get; set; }

        public GetPdfQuery(int id, string htmlContent)
        {
            Id = id;
            HtmlContent = htmlContent;
        }
    }
}
