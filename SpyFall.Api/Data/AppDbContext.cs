using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Models;

namespace SpyFall.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Location> Locations => Set<Location>();
	public DbSet<Role> Roles => Set<Role>();
	public DbSet<Game> Games => Set<Game>();
	public DbSet<Player> Players => Set<Player>();
}
