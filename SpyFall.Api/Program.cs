using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;
using SpyFall.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins(
				"http://localhost:4200",
				"https://spyfall.collinkoldoff.dev")
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await DatabaseSeeder.SeedAsync(db);

if (app.Environment.IsDevelopment())
	app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.MapControllers();
app.MapHub<GameHub>("/hubs/game");

app.Run();
