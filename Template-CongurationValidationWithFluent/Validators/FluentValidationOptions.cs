using FluentValidation;
using Microsoft.Extensions.Options;
using System.Text;

namespace Template_CongurationValidationWithFluent.Validators
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

            var validationResult = validator.Validate(options);
            if(validationResult.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Validation of '{typeof(T).Name}' failed. Please revalidate configuration files in '{_env.EnvironmentName}' environment.");
            stringBuilder.Append($"\n\nDETECTED ERRORS\n=============");
            validationResult.Errors.ForEach(error =>
            {
                stringBuilder.Append($"\n{error.PropertyName} - {error.ErrorMessage}");
            });

            return ValidateOptionsResult.Fail(stringBuilder.ToString());
        }
    }
}
