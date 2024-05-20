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
    config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.Run();
