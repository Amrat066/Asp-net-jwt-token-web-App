using AspNetSecurityApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetSecurityApplication.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
}
