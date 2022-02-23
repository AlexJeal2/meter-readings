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
            ConfigureAccountTable(modelBuilder);
            ConfigureMeterReadingTable(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureMeterReadingTable(ModelBuilder modelBuilder)
        {
            var meterReading = modelBuilder.Entity<MeterReading>();
            //Use a smalldatetime for MeterReadingDateTime ad we only need to store to the minute
            meterReading.Property(r => r.MeterReadingDateTime).HasColumnType("smalldatetime");
            //Create composite PK for MeterReading table:
            meterReading.HasKey(c => new { c.AccountId, c.MeterReadingDateTime });
            //Index the A
            meterReading.HasIndex(c => new { c.AccountId, c.MeterReadingDateTime })
                .IncludeProperties(p => p.MeterReadValue)
                .HasDatabaseName("IX_MeterReading_MeterReadValue");
        }
        private void ConfigureAccountTable(ModelBuilder modelBuilder)
        {
            var account = modelBuilder.Entity<Account>();
            //Not likely to do this in production but it helps with data seeding
            account.Property(et => et.AccountId).ValueGeneratedNever();
        }
    }
}