using MeterReadings.Data.Models;

namespace MeterReadings.Data.Repositories
{
    public class MeterReadingRepository : RepositoryBase, IMeterReadingRepository
    {
        private readonly AppDbContext _dbContext;
        public MeterReadingRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddMeterReadings(IEnumerable<MeterReading> meterReadings)
        {
            _dbContext.MeterReadings.AddRange(meterReadings);
        }

        public IQueryable<MeterReading> GetReadingsForAccount(int accountId)
        {
            return _dbContext.MeterReadings.Where(mr => mr.AccountId == accountId);
        }
    }
}
