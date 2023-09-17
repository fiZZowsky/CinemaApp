using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket
{
    public class GetPdfQuery : IRequest<byte[]>
    {
        public Guid Guid { get; set; }
        public string HtmlContent { get; set; }

        public GetPdfQuery(Guid guid, string htmlContent)
        {
            Guid = guid;
            HtmlContent = htmlContent;
        }
    }
}
