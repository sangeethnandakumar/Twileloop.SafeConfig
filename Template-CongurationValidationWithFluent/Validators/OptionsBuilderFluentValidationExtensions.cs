using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Template_CongurationValidationWithFluent.Validators
{
    public static class OptionsBuilderFluentValidationExtensions
    {
        public static OptionsBuilder<T> ValidateFluently<T>(this OptionsBuilder<T> optionsBuilder) where T : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<T>>(
                s => new FluentValidationOptions<T>(
                    optionsBuilder.Name, 
                    s.GetRequiredService<IValidator<T>>(),
                    s.GetRequiredService<IHostEnvironment>()));
            return optionsBuilder;
        }
    }
}
