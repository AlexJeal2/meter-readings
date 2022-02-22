using MeterReadings.Data.Models;

namespace MeterReadings.Data.Repositories
{
    public interface IAccountRepository : IRepositoryBase
    {
        Task AddAccounts(IEnumerable<Account> accounts);
    }
}