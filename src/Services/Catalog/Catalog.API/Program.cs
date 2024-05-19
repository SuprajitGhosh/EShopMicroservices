var builder = WebApplication.CreateBuilder(args);

// Add services to the container
//builder.Services.AddCarterWithAssemblies(typeof(Program).Assembly);
builder.Services.AddCarter(
 new DependencyContextAssemblyCatalog(assemblies: typeof(Program).Assembly)
);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});
builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.Run();
