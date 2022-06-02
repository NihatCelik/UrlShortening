using Business.Handlers.Urls.Commands;
using FluentValidation;
using System;

namespace Business.Handlers.Urls.ValidationRules
{
    public class CreateUrlValidator : AbstractValidator<CreateShortUrlCommand>
    {
        public CreateUrlValidator()
        {
            RuleFor(x => x.LongUrl).NotEmpty();
            RuleFor(x => x.LongUrl).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));
        }
    }

    public class CreateCustomUrlValidator : AbstractValidator<CreateCustomUrlCommand>
    {
        public CreateCustomUrlValidator()
        {
            RuleFor(x => x.LongUrl).NotEmpty();
            RuleFor(x => x.LongUrl).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));
            RuleFor(x => x.ShortUrl).NotEmpty();
            RuleFor(x => x.ShortUrl).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));
        }
    }
}