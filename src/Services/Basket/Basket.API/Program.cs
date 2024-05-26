using BuildingBlocks.Exceptions.Handler;
using Microsoft.Extensions.Caching.Distributed;

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
builder.Services.AddMarten(option => {
    option.Connection(builder.Configuration.GetConnectionString("Database")!);
    option.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//builder.Services.AddScoped<IBasketRepository>(provider => { 
//    var basketRepository = provider.GetService<BasketRepository>();
//    var distributedCache = provider.GetRequiredService<IDistributedCache>();
//    return new CachedBasketRepository(basketRepository!, distributedCache);
//});
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
/*
 * what is decorate ?
 */
builder.Services.AddStackExchangeRedisCache(optionss => {
    optionss.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddExceptionHandler<CustomeExceptionHandler>();

var app = builder.Build();

app.MapCarter();
//Configure the HTTP request pipeline
app.UseExceptionHandler(options => { });
app.Run();
