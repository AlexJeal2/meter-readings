using MeterReadings.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<Account> Accounts => Set<Account>();
    }
}