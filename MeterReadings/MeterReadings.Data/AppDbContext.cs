using MeterReadings.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public virtual DbSet<Account> Accounts => Set<Account>();
        public virtual DbSet<MeterReading> MeterReadings => Set<MeterReading>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var meterReading = modelBuilder.Entity<MeterReading>();
            //Use a smalldatetime for MeterReadingDateTime ad we only need to store to the minute
            meterReading.Property(r => r.MeterReadingDateTime).HasColumnType("smalldatetime");

            base.OnModelCreating(modelBuilder);
        }
    }
}