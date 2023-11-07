using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetPdfFromTicket
{
    public class GetPdfQuery : IRequest<byte[]>
    {
        public string Uid { get; set; }
        public string HtmlContent { get; set; }

        public GetPdfQuery(string uid, string htmlContent)
        {
            Uid = uid;
            HtmlContent = htmlContent;
        }
    }
}
