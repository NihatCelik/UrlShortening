using Business.Handlers.Urls.Commands;
using Core.Utilities.IoC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public static class FakeDataMiddleware
    {
        public static async Task UseDbFakeDataCreator(this IApplicationBuilder app)
        {
            var mediator = ServiceTool.ServiceProvider.GetService<IMediator>();

            await mediator.Send(new CreateCustomUrlCommand
            {
                LongUrl = "https://www.sample-site.com/karriere/berufserfahrene/direkteinstieg/",
                ShortUrl = "http://sample.site/GUKA8w/"
            });

            await mediator.Send(new CreateShortUrlCommand
            {
                LongUrl = "http://www.sample-site.com/karriere/jobsuche/"
            });
        }
    }
}
