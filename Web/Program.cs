using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var fileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath);

app.UseFileServer(new FileServerOptions
{
    FileProvider = fileProvider,
    EnableDefaultFiles = true
});

app.Run();
