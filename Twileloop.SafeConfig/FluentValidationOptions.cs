using FluentValidation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text;

namespace Twileloop.SafeConfig
{
    public class FluentValidationOptions<T> : IValidateOptions<T> where T : class
    {
        public string Name { get; }
        private readonly IValidator<T> validator;
        private readonly IHostEnvironment _env;


        public FluentValidationOptions(string name, IValidator<T> validator, IHostEnvironment Env)
        {
            Name = name;
            this.validator = validator;
            _env = Env;
        }

        public ValidateOptionsResult Validate(string? name, T options)
        {
            if (Name is not null && Name != name)
            {
                return ValidateOptionsResult.Skip;
            }

            ArgumentNullException.ThrowIfNull(options, nameof(options));

            var validate = validator.Validate(options);
            if(validate.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            var errors = new List<string>();
            validate.Errors.ForEach(error =>
            {
                errors.Add($"{error.PropertyName} - {error.ErrorMessage}");
            });

            return ValidateOptionsResult.Fail(errors);
        }
    }
}
