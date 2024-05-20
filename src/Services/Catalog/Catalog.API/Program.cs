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
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();
builder.Services.AddExceptionHandler<CustomeExceptionHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.Run();
