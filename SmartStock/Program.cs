var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDefaultFiles();  // Habilita la carga de index.html autom�ticamente
app.UseStaticFiles();   // Permite servir archivos est�ticos

app.MapFallbackToFile("index.html");

app.Run();
