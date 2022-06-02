using Business.Handlers.Urls.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Concrete;
using Business.Handlers.Urls.Commands;
using Business.Constants;
using MediatR;
using FluentAssertions;
using Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class UrlHandlerTests
    {
        Mock<IUrlRepository> _urlRepository;
        Mock<IMediator> _mediator;
        private const string longUrl = "https://www.sample-site.com/karriere/berufserfahrene/direkteinstieg/";
        private const string shortUrl = "http://sample.site/GUKA8w/";

        [SetUp]
        public void Setup()
        {
            _urlRepository = new Mock<IUrlRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Url_GetQuery_Success()
        {
            //Arrange
            var query = new GetUrlQuery();

            _urlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>())).ReturnsAsync(new Url
            {
                Id = 1,
                LongUrl = longUrl,
                ShortUrl = shortUrl,
                UrlType = UrlType.System,
            });

            var handler = new GetUrlQueryHandler(_urlRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            _urlRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>()), Times.Once);
            x.Success.Should().BeTrue();
            x.Data.Should().Be(longUrl);
        }

        [Test]
        public async Task Url_GetQuery_UrlNotFound()
        {
            //Arrange
            var query = new GetUrlQuery();
            Url url = null;

            _urlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>())).ReturnsAsync(url);

            var handler = new GetUrlQueryHandler(_urlRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            _urlRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>()), Times.Once);
            x.Success.Should().BeFalse();
            x.Data.Should().Be(null);
            x.Message.Should().Be(Messages.UrlNotFound);
        }

        [Test]
        public async Task Url_CreateShortUrlCommand_Success()
        {
            Url rt = null;
            var command = new CreateShortUrlCommand { LongUrl = longUrl };

            _urlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>())).ReturnsAsync(rt);
            _urlRepository.Setup(x => x.AddAsync(It.IsAny<Url>())).ReturnsAsync(new Url { Id = 1, ShortUrl = shortUrl, LongUrl = command.LongUrl });

            var handler = new CreateUrlCommandHandler(_urlRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _urlRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>()), Times.Once);
            _urlRepository.Verify(x => x.AddAsync(It.IsAny<Url>()), Times.Once);
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.ShortUrlCreated);
        }

        [Test]
        public async Task Url_CreateCustomUrlCommand_Success()
        {
            var command = new CreateCustomUrlCommand { LongUrl = longUrl, ShortUrl = shortUrl };

            _urlRepository.Setup(x => x.GetQuery(It.IsAny<Expression<Func<Url, bool>>>()))
                .Returns(new List<Url>().AsQueryable().BuildMockDbSet().Object);
            _urlRepository.Setup(x => x.AddAsync(It.IsAny<Url>()))
                .ReturnsAsync(new Url { Id = 1, ShortUrl = command.ShortUrl, LongUrl = command.LongUrl });

            var handler = new CreateCustomUrlCommandHandler(_urlRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _urlRepository.Verify(x => x.GetQuery(It.IsAny<Expression<Func<Url, bool>>>()), Times.Once);
            _urlRepository.Verify(x => x.AddAsync(It.IsAny<Url>()), Times.Once);
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.ShortUrlCreated);
        }

        [Test]
        public async Task Url_CreateShortUrlCommand_ExistShortUrlSuccess()
        {
            Url rt = new Url { ShortUrl = shortUrl };
            var command = new CreateShortUrlCommand { LongUrl = longUrl };

            _urlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>())).ReturnsAsync(rt);
            _urlRepository.Setup(x => x.AddAsync(It.IsAny<Url>())).ReturnsAsync(new Url { Id = 1, ShortUrl = shortUrl, LongUrl = command.LongUrl });

            var handler = new CreateUrlCommandHandler(_urlRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _urlRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>()), Times.Once);
            _urlRepository.Verify(x => x.AddAsync(It.IsAny<Url>()), Times.Never);
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.ShortUrlCreated);
            x.Data.Should().Be(shortUrl);
        }

        [Test]
        public async Task Url_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteUrlCommand();

            _urlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Url, bool>>>()))
                .ReturnsAsync(new Url() { Id = 1, LongUrl = longUrl, ShortUrl = shortUrl });

            _urlRepository.Setup(x => x.DeleteAsync(It.IsAny<Url>()));

            var handler = new DeleteUrlCommandHandler(_urlRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _urlRepository.Verify(x => x.DeleteAsync(It.IsAny<Url>()), Times.Once);
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

