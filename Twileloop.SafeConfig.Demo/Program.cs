
using FluentValidation;
using Microsoft.Extensions.Options;
using Twileloop.SafeConfig.Validators;
using Twileloop.SafeConfig;

namespace Twileloop.SafeConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var config = builder.Configuration.GetSection(nameof(SMTPOptions)).Get<SMTPOptions>();

            builder.Services
                .AddOptionsWithValidateOnStart<SMTPOptions>()
                .Bind(builder.Configuration.GetSection(nameof(SMTPOptions)))
                .ValidateFluently()
                .ValidateOnStart();

            //Step 2: Add validators
            builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
