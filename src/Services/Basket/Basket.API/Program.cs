var builder = WebApplication.CreateBuilder(args);

// Add Services 

var app = builder.Build();

//Configure the HTTP request pipeline

app.MapGet("/", () => "Hello World!");

app.Run();
