# LunchApp

This repository contains an ASP.NET Core Web API backend and a Blazor WebAssembly client.

## Requirements
- .NET 8 SDK
- A SQL Server instance accessible by the connection string in `Server/appsettings.json`.

## Running the app
From the repository root, start the backend API and the Blazor client in separate terminals:

```bash
# Start the Web API
 dotnet run --project Server/OfficeCafeApp.API.csproj

# Start the Blazor WebAssembly client
 dotnet run --project Client/LunchApp.Frontend.csproj
```

Browse to `http://localhost:5025/login` to access the application.

## Simple static website

A basic HTML version of the site is available in the `Website` folder. Open `index.html` in a browser to view it.
