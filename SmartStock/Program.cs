var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDefaultFiles();  // Habilita la carga de index.html automáticamente
app.UseStaticFiles();   // Permite servir archivos estáticos

app.MapFallbackToFile("index.html");

app.Run();
