using MeterReadings.Data.Models;

namespace MeterReadings.Data.Repositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddAccounts(IEnumerable<Account> accounts)
        {
            //Get the account IDs to check for existing accounts
            var accountIds = new HashSet<int>(accounts.Select(x => x.AccountId));

            var existingAccountsQuery = _dbContext.Accounts
                .Where(account => accountIds.Contains(account.AccountId))
                .Select(x => x.AccountId);

            var existingAccountIds = new HashSet<int>(existingAccountsQuery);

            var accountsToAdd = accounts.Where(x => !existingAccountIds.Contains(x.AccountId));
            _dbContext.Accounts.AddRange(accountsToAdd);
        }
    }
}
