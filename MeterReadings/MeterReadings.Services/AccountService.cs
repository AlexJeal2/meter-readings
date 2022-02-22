using AutoMapper;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using System.Text.Json;

namespace MeterReadings.Services
{
    public class AccountService : IAccountService
    {
        private IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IMapper mapper, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        public async Task SeedDatabaseAsync()
        {
            var lines = (await File.ReadAllLinesAsync(@"Test_Accounts.csv")).ToList();

            List<Account> accounts = new List<Account>();
            foreach (string line in lines.Skip(1))
            {
                var colData = line.Split(',');

                var account = new Account(int.Parse(colData[0]), colData[1].ToString(), colData[2].ToString());
                accounts.Add(account);
            }

            await _accountRepository.AddAccounts(accounts);
            await _accountRepository.SaveChangesAsync();
        }
    }
}