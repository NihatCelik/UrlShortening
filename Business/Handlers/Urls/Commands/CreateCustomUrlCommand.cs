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
    public class CreateCustomUrlCommand : IRequest<IResult>
    {
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
    }

    public class CreateCustomUrlCommandHandler : IRequestHandler<CreateCustomUrlCommand, IResult>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMediator _mediator;
        public CreateCustomUrlCommandHandler(IUrlRepository urlRepository, IMediator mediator)
        {
            _urlRepository = urlRepository;
            _mediator = mediator;
        }

        [ValidationAspect(typeof(CreateCustomUrlValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IResult> Handle(CreateCustomUrlCommand request, CancellationToken cancellationToken)
        {
            var isThereUrlRecord = _urlRepository.GetQuery().Any(u => u.LongUrl == request.LongUrl || u.ShortUrl == request.ShortUrl);
            if (isThereUrlRecord)
            {
                return new ErrorResult(Messages.UrlAlreadyExist);
            }

            var addedUrl = new Url
            {
                LongUrl = request.LongUrl,
                ShortUrl = request.ShortUrl,
            };

            await _urlRepository.AddAsync(addedUrl);
            return new SuccessResult(Messages.ShortUrlCreated);
        }
    }
}