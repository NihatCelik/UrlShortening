using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Urls.Commands
{
    public class DeleteUrlCommand : IRequest<IResult>
    {
        public string ShortUrl { get; set; }
    }

    public class DeleteUrlCommandHandler : IRequestHandler<DeleteUrlCommand, IResult>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMediator _mediator;

        public DeleteUrlCommandHandler(IUrlRepository urlRepository, IMediator mediator)
        {
            _urlRepository = urlRepository;
            _mediator = mediator;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
        {
            var urlToDelete = await _urlRepository.GetAsync(p => p.ShortUrl == request.ShortUrl);
            if (urlToDelete == null)
            {
                return new ErrorResult(Messages.UrlNotFound);
            }

            await _urlRepository.DeleteAsync(urlToDelete);
            return new SuccessResult(Messages.Deleted);
        }
    }
}
