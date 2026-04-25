using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await DatabaseSeeder.SeedAsync(db);

if (app.Environment.IsDevelopment())
	app.MapOpenApi();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
