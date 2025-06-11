var builder = WebApplication.CreateBuilder(args);

// Serve static files from the project root so HTML, CSS and JS work
builder.WebHost.UseWebRoot(builder.Environment.ContentRootPath);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
