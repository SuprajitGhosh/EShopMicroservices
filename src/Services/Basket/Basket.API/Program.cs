var builder = WebApplication.CreateBuilder(args);

// Add Services 
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter(
 new DependencyContextAssemblyCatalog(assemblies: assembly)
);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
var app = builder.Build();

app.MapCarter();
//Configure the HTTP request pipeline

app.Run();
