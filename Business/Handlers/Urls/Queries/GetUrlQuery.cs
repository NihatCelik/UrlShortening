using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Urls.Queries
{
    public class GetUrlQuery : IRequest<IDataResult<string>>
    {
        public string ShortUrl { get; set; }
    }

    public class GetUrlQueryHandler : IRequestHandler<GetUrlQuery, IDataResult<string>>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMediator _mediator;

        public GetUrlQueryHandler(IUrlRepository urlRepository, IMediator mediator)
        {
            _urlRepository = urlRepository;
            _mediator = mediator;
        }

        public async Task<IDataResult<string>> Handle(GetUrlQuery request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetAsync(p => p.ShortUrl == request.ShortUrl);
            if (url == null)
            {
                return new ErrorDataResult<string>(null, Messages.UrlNotFound);
            }

            return new SuccessDataResult<string>(url.LongUrl, "");
        }
    }
}
