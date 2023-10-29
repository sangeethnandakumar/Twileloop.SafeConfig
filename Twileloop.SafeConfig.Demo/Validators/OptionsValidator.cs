using FluentValidation;

namespace Twileloop.SafeConfig.Validators
{
    public class OptionsValidator : AbstractValidator<SMTPOptions>
    {
        public OptionsValidator()
        {
            //If strings to be casted to Enums, This check is usefull
            RuleFor(x => x.ServerType)
                .IsEnumName(typeof(ServerType), caseSensitive: false);

            RuleFor(x => x.Port)
                .InclusiveBetween(0, 9)
                .WithMessage("Should be between 0-9");

            RuleFor(x => x.Server)
                .Empty()
                .WithMessage("Must be empty");
        }
    }
}
