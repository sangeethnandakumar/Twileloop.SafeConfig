# Configuration Validation In ASP.NET Core API
The following template allows validating configuration using custom FluentValidator and enforce a fail early policy

## Step 1: Register your options in appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "SMTPOptions": {
    "Server": "https://localhot:5000",
    "Port": 5001,
    "ServerType": "DONGLY"
  }
}

```

## Step 2: Create a model for binding it
```csharp
    public enum ServerType
    {
        GOOGLE,
        MICROSOFT
    }

    public class SMTPOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string ServerType { get; set; }
    }
```

## Step 3: In Program.cs, Configure IOptions
```csharp
            //Step 1: Configure option
            builder.Services
                .AddOptions<SMTPOptions>()
                .Bind(builder.Configuration.GetSection(nameof(SMTPOptions)))
                .ValidateFluently()
                .ValidateOnStart();
```

## Step 4: Register all validators
```csharp
            //Step 2: Add validators
            builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
```

## Step 5: To use 'ValidateFluently()', Build the extension
```csharp
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
```

## Step 6: Write validation provider
```csharp
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
```

## Step 7: Write validators
```csharp
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
```

# Screenshots
![image](https://github.com/sangeethnandakumar/Template-CongurationValidationWithFluent/assets/24974154/3d3faff9-b8ab-4329-8d58-d17b14254392)
