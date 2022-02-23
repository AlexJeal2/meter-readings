using AutoMapper;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using System.IO.Abstractions;

namespace MeterReadings.Services
{
    public class AccountService : IAccountService
    {
        private IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IFileSystem _fileSystem;

        public AccountService(IAccountRepository accountRepository, IFileSystem fileSystem, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _fileSystem = fileSystem;
            _mapper = mapper;
        }

        public async Task SeedDatabaseAsync()
        {
            var lines = (await _fileSystem.File.ReadAllLinesAsync(@"Test_Accounts.csv")).ToList();

            List<Account> accounts = new List<Account>();
            foreach (string line in lines.Skip(1))
            {
                var colData = line.Split(',');

                var account = new Account(int.Parse(colData[0]), colData[1].ToString(), colData[2].ToString());
                accounts.Add(account);
            }

            _accountRepository.AddAccounts(accounts);
            await _accountRepository.SaveChangesAsync();
        }
    }
}