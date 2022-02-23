using MeterReadings.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Data.Repositories
{
    public class MeterReadingRepository : RepositoryBase, IMeterReadingRepository
    {
        private readonly AppDbContext _dbContext;
        public MeterReadingRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddMeterReadings(Dictionary<int, List<MeterReading>> readingsByAccount)
        {
            foreach (var (accountId, readings) in readingsByAccount)
            {
                AddMeterReadingsForAccount(accountId, readings);
            }
            return;
        }

        public void AddMeterReadingsForAccount(int accountId, List<MeterReading> meterReadings)
        {
            _dbContext.MeterReadings.AddRange(meterReadings);
        }
    }
}
