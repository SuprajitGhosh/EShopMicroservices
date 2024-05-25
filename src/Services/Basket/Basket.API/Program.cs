using Carter;

var builder = WebApplication.CreateBuilder(args);

// Add Services 
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter(
 new DependencyContextAssemblyCatalog(assemblies: assembly)
);

var app = builder.Build();

//Configure the HTTP request pipeline

app.MapGet("/", () => "Hello World!");

app.Run();
