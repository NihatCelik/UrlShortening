using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Urls.ValidationRules;

namespace Business.Handlers.Urls.Commands
{
    public class CreateShortUrlCommand : IRequest<IDataResult<string>>
    {
        public string LongUrl { get; set; }
    }

    public class CreateUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, IDataResult<string>>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMediator _mediator;
        public CreateUrlCommandHandler(IUrlRepository urlRepository, IMediator mediator)
        {
            _urlRepository = urlRepository;
            _mediator = mediator;
        }

        [ValidationAspect(typeof(CreateUrlValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<string>> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetAsync(u => u.LongUrl == request.LongUrl);
            if (url != null)
            {
                return new SuccessDataResult<string>(url.ShortUrl, Messages.ShortUrlCreated);
            }

            var addedUrl = new Url
            {
                LongUrl = request.LongUrl,
                ShortUrl = Helpers.UrlHelper.GenerateShortUrl(request.LongUrl),
            };

            await _urlRepository.AddAsync(addedUrl);
            return new SuccessDataResult<string>(addedUrl.ShortUrl, Messages.ShortUrlCreated);
        }
    }
}