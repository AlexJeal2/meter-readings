using AutoMapper;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using MeterReadings.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestAccountService
    {
        private Mock<IMapper> mockMapper = new Mock<IMapper>();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task SeedDatabaseAsync()
        {
            var mockAccountRepo = new Mock<IAccountRepository>();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("AccountId,FirstName,LastName");
            csvBuilder.AppendLine("3,Baz,Test");
            csvBuilder.AppendLine("4,Joe,Test");
            csvBuilder.AppendLine("5,Sam,Test");

            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile("Test_Accounts.csv", new MockFileData(csvBuilder.ToString()));

            mockAccountRepo.Setup(x => x.AddAccounts(It.IsAny<List<Account>>()));

            var accountService = new AccountService(mockAccountRepo.Object, mockFileSystem, mockMapper.Object);
            await accountService.SeedDatabaseAsync();

            var expectedAccounts = new List<Account>()
            {
                new Account(3, "Baz", "Test"),
                new Account(4, "Joe", "Test"),
                new Account(5, "Sam", "Test"),
            };

            mockAccountRepo.Verify(x => x.AddAccounts(
                It.Is<List<Account>>(x => x.All(account => HasAccount(expectedAccounts, account)))),
                Times.Once);
        }

        private bool HasAccount(List<Account> accounts, Account account)
        {
            return accounts.Any(x => x.AccountId == account.AccountId 
            && x.FirstName == account.FirstName
            && x.LastName == account.LastName);
        }
    }
}