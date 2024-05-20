using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
//builder.Services.AddCarterWithAssemblies(typeof(Program).Assembly);
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter(
 new DependencyContextAssemblyCatalog(assemblies: assembly)
);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);

    // check IValidator from FluentValidation. How it is linked to AbstractValidator in ValidationBehaviour of BuildingBlocks
    // check IPipelineBehavior from Mediatr. how it is linked to ICommandHandler in ValidationBehaviour of BuildingBlocks
    // check what is AddOpenBehavior
    // check next property and method in ValidationBehaviour of BuildingBlocks
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context => {
    var exception = context.Features.Get<IExceptionHandlerFeature>()!.Error;
    if (exception == null)
    {
        return;
    }
    var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = exception.Message,
        Status = StatusCodes.Status500InternalServerError,
        Detail = exception.StackTrace
    };
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogError(exception, exception.Message);
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    context.Response.ContentType = "application/problem+json";

    await context.Response.WriteAsJsonAsync(problemDetails);
}));
app.Run();
