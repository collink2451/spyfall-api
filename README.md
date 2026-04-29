# Spyfall API

ASP.NET Core 9 Web API backend for the Spyfall party game. Manages game sessions, player state, voting, and game timers. Uses SignalR for real-time communication with the [spyfall-client](../spyfall-client) Angular frontend.

## Features

- Create and join game lobbies with 6-character room codes
- Player ready state and host assignment (first player to connect becomes host)
- Real-time events via SignalR (player joined, game started, timer sync, vote updates)
- Game timer with sync service
- Automatic cleanup of inactive game sessions
- Rate limiting: 30 requests/minute per IP
- MySQL database via Entity Framework Core

## Tech Stack

- **Framework:** ASP.NET Core 9
- **Real-time:** SignalR (`/hubs/game`)
- **Database:** MySQL (via Pomelo Entity Framework Core)
- **ORM:** Entity Framework Core with migrations

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| `POST` | `/api/games` | Create a new game, returns room code |
| `POST` | `/api/games/{code}/join` | Join a game by code |
| `GET` | `/api/games/{code}` | Get game status and player list |
| `GET` | `/api/locations` | Get available game locations |

## SignalR Hub

Connect to `/hubs/game` for real-time game events.

## Setup

### Requirements

- .NET 9 SDK
- MySQL 8+ instance

### Configuration

The database connection string is read from `ConnectionStrings:DefaultConnection` in `appsettings.json` or via environment variable.

Add to `appsettings.Development.json` (for local dev):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=spyfall;User=root;Password=yourpassword;"
  }
}
```

Or set the environment variable:

```
ConnectionStrings__DefaultConnection=Server=localhost;Database=spyfall;User=root;Password=yourpassword;
```

### Running

1. Apply database migrations:

```bash
cd SpyFall.Api
dotnet ef database update
```

2. Start the API:

```bash
dotnet run
```

The API runs at `http://localhost:5000` by default (see `Properties/launchSettings.json`).

### CORS

The API allows requests from `http://localhost:4200` (Angular dev server) and `https://spyfall.collinkoldoff.dev`. Update `Program.cs` to add additional origins.
