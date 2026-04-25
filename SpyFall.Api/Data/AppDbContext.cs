using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Models;

namespace SpyFall.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Location> Locations => Set<Location>();
	public DbSet<Role> Roles => Set<Role>();
	public DbSet<Game> Games => Set<Game>();
	public DbSet<Player> Players => Set<Player>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Player>()
			.HasIndex(p => new { p.GameId, p.Name })
			.IsUnique();

		modelBuilder.Entity<Game>()
			.HasOne(g => g.Host)
			.WithMany()
			.HasForeignKey(g => g.HostPlayerId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<Game>()
			.HasMany(g => g.Players)
			.WithOne(p => p.Game)
			.HasForeignKey(p => p.GameId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
