using Demo.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Demo.Api;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IConfiguration configuration)
    : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(_configuration.GetConnectionString("Sqlite"));
}
