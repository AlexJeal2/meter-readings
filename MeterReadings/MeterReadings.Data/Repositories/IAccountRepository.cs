using MeterReadings.Data.Models;

namespace MeterReadings.Data.Repositories
{
    public interface IAccountRepository : IRepositoryBase
    {
        void AddAccounts(IEnumerable<Account> accounts);
    }
}