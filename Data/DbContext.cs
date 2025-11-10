using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Models;

namespace PortfolioBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //DbSet
    public DbSet <Feedback> Feedback { get; set; }
}