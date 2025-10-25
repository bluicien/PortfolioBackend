using Microsoft.EntityFrameworkCore;
using PortoflioBackend.Models;

namespace PortoflioBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //DbSet
    public DbSet <Feedback> Feedback { get; set; }
}