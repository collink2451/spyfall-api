using Microsoft.EntityFrameworkCore;

namespace SpyFall.Api.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
